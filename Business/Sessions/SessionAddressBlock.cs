using System;
using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// SessionAddressBlock is a class that contains the references to all sessions that are available to the system.
	/// </summary>
	/// <remarks>
	/// The SessionAddressBlock class holds the references to all sessions that are available in the system.  These references
	/// may be proxy or local references, depending on how the session was created.  All or selected sessions can be created through
	/// the CreateSessions method.
	/// </remarks>

	[Serializable]
	public class SessionAddressBlock
	{
		//Begin TT#708 - JScott - Services need a Retry availalbe.
		//==========
		// CONSTANTS
		//==========

		const int cServiceRetryCount = 4;
		const int cServiceRetryInterval = 2000;

		//End TT#708 - JScott - Services need a Retry availalbe.
		//=======
		// FIELDS
		//=======

		private IMessageCallback _messageCallback;
		private ISponsor _sponsor;
		//Begin TT#708 - JScott - Services need a Retry availalbe.
		private ControlServerSessionRemote _controlServerSessionRemote;
		private ClientServerSessionRemote _clientServerSessionRemote;
		private StoreServerSessionRemote _storeServerSessionRemote;
		private HierarchyServerSessionRemote _hierarchyServerSessionRemote;
		private ApplicationServerSessionRemote _applicationServerSessionRemote;
		private SchedulerServerSessionRemote _schedulerServerSessionRemote;
		private HeaderServerSessionRemote _headerServerSessionRemote;
		//End TT#708 - JScott - Services need a Retry availalbe.
		private ControlServerSession _controlServerSession;
		private ClientServerSession _clientServerSession;
		private StoreServerSession _storeServerSession;
		private HierarchyServerSession _hierarchyServerSession;
		private ApplicationServerSession _applicationServerSession;
		private SchedulerServerSession _schedulerServerSession;
		private HeaderServerSession _headerServerSession;
//		private bool _monitorForecastAppSetting;
		private bool _remoteServices;
		private ControlServerServerGroup _serverGroup;
		private string _controlServer;
// (CSMITH) - BEG MID Track #3369: DB Connection String hardcoded in config files
		private string _connectionString = null;
// (CSMITH) - END MID Track #3369
        private string _ROExtractConnectionString = null;   // TT#2131-MD - JSmith - Halo Integration
        private bool _ROExtractEnabled = false;    // TT#2131-MD - JSmith - Halo Integration
        private MIDMenuEvent _MIDMenuEvent;
		private bool _forceLocal;
        private bool _allowDebugging;
		//Begin TT#708 - JScott - Services need a Retry availalbe.
		private int _serviceRetryCount;
		private int _serviceRetryInterval;
		//End TT#708 - JScott - Services need a Retry availalbe.
		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		private bool _controlGlobalLoaded;
		private bool _clientGlobalLoaded;
		private bool _storeGlobalLoaded;
		private bool _hierarchyGlobalLoaded;
		private bool _applicationGlobalLoaded;
		private bool _headerGlobalLoaded;
		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
        // Begin Track #6346 - JSmith - Duplicate rows after Rollup
        private int _sessionID = 0;
        // End Track #6346
        // Begin TT#2004 - JSmith - Infragistics layouts getting corrupted resulting in key not found during startup
        private bool _applicationStarted = false;
        // End TT#2004
		// BEGIN TT#217-MD - stodd - Running methods from the explorer on Assortments
		private ProcessMethodOnAssortmentEvent _processMethodOnAssortmentEvent;
		// END TT#217-MD - stodd - Running methods from the explorer on Assortments

		// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		private AssortmentSelectedHeaderEvent _assortmentSelectedHeaderEvent;
		// END TT#371-MD - stodd -  Velocity Interactive on Assortment
		private AssortmentTransactionEvent _assortmentTransactionEvent;		// TT#698-MD - Stodd - add ability for workflows to be run against assortments.


        private bool _isServicesAvailable = false; //TT#899-MD -jsobek -Services Unavailable Login Message
        public SocketClientManager clientSocketManager = null; //TT#901-MD -jsobek -Batch Only Mode
        private int _allocationWorkspaceCurrentHeaderFilter = Include.NoRID; //TT#1313-MD -jsobek -Header Filters

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of SessionAddressBlock
		/// </summary>
		/// <param name="aMessageCallback">
		/// Object of the type IMessageCallback that the server can request real-time error handling from the client
		/// </param>
		/// <param name="aSponsor">
		/// Object of the type ISponsor that will be used to sponsor all sessions created
		/// </param>

		public SessionAddressBlock(IMessageCallback aMessageCallback, ISponsor aSponsor)
		{
			_messageCallback = aMessageCallback;
			_sponsor = aSponsor;
			_remoteServices = false;
			_forceLocal = false;
			_MIDMenuEvent = new MIDMenuEvent();
			_allowDebugging = false;
			//Begin TT#708 - JScott - Services need a Retry availalbe.
			_serviceRetryCount = cServiceRetryCount;
			_serviceRetryInterval = cServiceRetryInterval;
			//End TT#708 - JScott - Services need a Retry availalbe.
			// BEGIN TT#217-MD - stodd - Running methods from the explorer on Assortments
			_processMethodOnAssortmentEvent = new ProcessMethodOnAssortmentEvent();
			// END TT#217-MD - stodd - Running methods from the explorer on Assortments
			// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
			_assortmentSelectedHeaderEvent = new AssortmentSelectedHeaderEvent();
			// END TT#371-MD - stodd -  Velocity Interactive on Assortment
			_assortmentTransactionEvent = new AssortmentTransactionEvent();		// TT#698-MD - Stodd - add ability for workflows to be run against assortments.
		}

		public SessionAddressBlock(IMessageCallback aMessageCallback, ISponsor aSponsor, string aConnectionString, bool aForceLocal)
		{
			_messageCallback = aMessageCallback;
			_sponsor = aSponsor;
			_remoteServices = false;
			ConnectionString = aConnectionString;
			_forceLocal = aForceLocal;
			_MIDMenuEvent = new MIDMenuEvent();
			_allowDebugging = false;
			//Begin TT#708 - JScott - Services need a Retry availalbe.
			_serviceRetryCount = cServiceRetryCount;
			_serviceRetryInterval = cServiceRetryInterval;
			//End TT#708 - JScott - Services need a Retry availalbe.
			// BEGIN TT#217-MD - stodd - Running methods from the explorer on Assortments
			_processMethodOnAssortmentEvent = new ProcessMethodOnAssortmentEvent();
			// END TT#217-MD - stodd - Running methods from the explorer on Assortments
			// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
			_assortmentSelectedHeaderEvent = new AssortmentSelectedHeaderEvent();
			// END TT#371-MD - stodd -  Velocity Interactive on Assortment
			_assortmentTransactionEvent = new AssortmentTransactionEvent();		// TT#698-MD - Stodd - add ability for workflows to be run against assortments.
		}

		//===========
		// PROPERTIES
		//===========
        public MIDMenuEvent MIDMenuEvent
		{
			get
			{
                return _MIDMenuEvent;
			}
		}

		public IMessageCallback MessageCallback
		{
			get
			{
				return _messageCallback;
			}
		}

		public ISponsor Sponsor
		{
			get
			{
				return _sponsor;
			}
		}

		public ControlServerServerGroup ServerGroup
		{
			get
			{
				return _serverGroup;
			}
		}

		public string ControlServer
		{
			get
			{
				return _controlServer;
			}
		}

		/// <summary>
		/// Returns the reference to the Control Session
		/// </summary>

		public ControlServerSession ControlServerSession
		{
			get
			{
				return _controlServerSession;
			}
			//Begin TT#708 - JScott - Services need a Retry availalbe.
			//set
			//{
			//    _controlServerSession = value;
			//}
			//End TT#708 - JScott - Services need a Retry availalbe.
		}

		/// <summary>
		/// Returns the reference to the Client Session
		/// </summary>

		public ClientServerSession ClientServerSession
		{
			get
			{
				return _clientServerSession;
			}
			//Begin TT#708 - JScott - Services need a Retry availalbe.
			//set
			//{
			//    _clientServerSession = value;
			//}
			//End TT#708 - JScott - Services need a Retry availalbe.
		}

		/// <summary>
		/// Returns the reference to the Store Session
		/// </summary>

		public StoreServerSession StoreServerSession
		{
			get
			{
				return _storeServerSession;
			}
			//Begin TT#708 - JScott - Services need a Retry availalbe.
			//set
			//{
			//    _storeServerSession = value;
			//}
			//End TT#708 - JScott - Services need a Retry availalbe.
		}

		/// <summary>
		/// Returns the reference to the Hierarchy Session
		/// </summary>

		public HierarchyServerSession HierarchyServerSession
		{
			get
			{
				return _hierarchyServerSession;
			}
			//Begin TT#708 - JScott - Services need a Retry availalbe.
			//set
			//{
			//    _hierarchyServerSession = value;
			//}
			//End TT#708 - JScott - Services need a Retry availalbe.
		}

		/// <summary>
		/// Returns the reference to the Application Session
		/// </summary>

		public ApplicationServerSession ApplicationServerSession
		{
			get
			{
				return _applicationServerSession;
			}
			//Begin TT#708 - JScott - Services need a Retry availalbe.
			//set
			//{
			//    _applicationServerSession = value;
			//}
			//End TT#708 - JScott - Services need a Retry availalbe.
		}

		/// <summary>
		/// Returns the reference to the Scheduler Session
		/// </summary>

		public SchedulerServerSession SchedulerServerSession
		{
			get
			{
				return _schedulerServerSession;
			}
			//Begin TT#708 - JScott - Services need a Retry availalbe.
			//set
			//{
			//    _schedulerServerSession = value;
			//}
			//End TT#708 - JScott - Services need a Retry availalbe.
		}

		/// <summary>
		/// Returns the reference to the Header Session
		/// </summary>

		public HeaderServerSession HeaderServerSession
		{
			get
			{
				return _headerServerSession;
			}
			//Begin TT#708 - JScott - Services need a Retry availalbe.
			//set
			//{
			//    _headerServerSession = value;
			//}
			//End TT#708 - JScott - Services need a Retry availalbe.
		}

//		public bool MonitorForecastAppSetting
//		{
//			get
//			{
//				return _monitorForecastAppSetting;
//			}
//			set
//			{
//				_monitorForecastAppSetting = value;
//			}
//		}

		public bool RemoteServices
		{
			get
			{
				return _remoteServices;
			}
		}

// (CSMITH) - BEG MID Track #3369: DB Connection String hardcoded in config files
		public string ConnectionString
		{
			get
			{
				return _connectionString;
			}

			set
			{
				_connectionString = value;
			}
		}
// (CSMITH) - END MID Track #3369

        // Begin TT#2131-MD - JSmith - Halo Integration
        public string ROExtractConnectionString
        {
            get
            {
                return _ROExtractConnectionString;
            }

            set
            {
                _ROExtractConnectionString = value;
            }
        }

        public bool ROExtractEnabled
        {
            get
            {
                return _ROExtractEnabled;
            }
        }
        // End TT#2131-MD - JSmith - Halo Integration

        public bool AllowDebugging
        {
            get
            {
                return _allowDebugging;
            }
        }

        // Begin Track #6346 - JSmith - Duplicate rows after Rollup
        public int SessionID
        {
            get
            {
                return _sessionID;
            }
        }
        // End Track #6346

        // Begin TT#2004 - JSmith - Infragistics layouts getting corrupted resulting in key not found during startup
        public bool ApplicationStarted
        {
            get
            {
                return _applicationStarted;
            }

            set
            {
                _applicationStarted = value;
            }
        }
        // End TT#2004

		// BEGIN TT#217-MD - stodd - Running methods from the explorer on Assortments
		public ProcessMethodOnAssortmentEvent ProcessMethodOnAssortmentEvent
		{
			get
			{
				return _processMethodOnAssortmentEvent;
			}

			set
			{
				_processMethodOnAssortmentEvent = value;
			}
		}
		// BEGIN TT#217-MD - stodd - Running methods from the explorer on Assortments

		// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		public AssortmentSelectedHeaderEvent AssortmentSelectedHeaderEvent
		{
			get
			{
				return _assortmentSelectedHeaderEvent;
			}

			set
			{
				_assortmentSelectedHeaderEvent = value;
			}
		}

		// END TT#371-MD - stodd -  Velocity Interactive on Assortment


        public bool IsServicesAvailable
        {
            get
            {
                return _isServicesAvailable;
            }
        }


		// BEGIN TT#698-MD - Stodd - add ability for workflows to be run against assortments.
		public AssortmentTransactionEvent AssortmentTransactionEvent
		{
			get
			{
				return _assortmentTransactionEvent;
			}

			set
			{
				_assortmentTransactionEvent = value;
			}
		}
		// END TT#698-MD - Stodd - add ability for workflows to be run against assortments.

        //Begin TT#1313-MD -jsobek -Header Filters
        public int AllocationWorkspaceCurrentHeaderFilter
        {
            get
            {
                return _allocationWorkspaceCurrentHeaderFilter;
            }

            set
            {
                _allocationWorkspaceCurrentHeaderFilter = value;
            }
        }
        //End TT#1313-MD -jsobek -Header Filters


		//========
		// METHODS
		//========

		/// <summary>
		/// Creates and opens the Callback channel for the client.
		/// </summary>
		/// <remarks>
		/// The callback channel allows the services to communicate back to the client.  This channel is used to handle
		/// error conditions that require user input and sponsors of MBR objects.
		/// </remarks>
		/// <returns>
		/// The newly created and openend IChannel object.
		/// </returns>

		public IChannel OpenCallbackChannel()
		{
			BinaryServerFormatterSinkProvider provider; 
			Hashtable port;
			IPAddress clientIP;
			IChannel channel;

			try
			{
				// Register callback channel

				provider = new BinaryServerFormatterSinkProvider();
				provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
				port = new Hashtable();
				port.Add("port", 0);
				channel = new TcpChannel(port, null, provider);
                ChannelServices.RegisterChannel(channel, true);

				// Retrieve the client's IP Address by calling the SAB.GetClientIPAddress.  This is necessary for VPN connections, as the
				// client does not know what the local VPN address is.

				clientIP = GetClientIPAddress();

				// If the client IP is not null, unregister callback channel and re-register with the machineName parameter

				if (clientIP != null)
				{
					System.Runtime.Remoting.Channels.ChannelServices.UnregisterChannel(channel);

					provider = new BinaryServerFormatterSinkProvider();
					provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
					port = new Hashtable();
					port.Add("port", 0);
					port.Add("machineName", clientIP.ToString());
					channel = new TcpChannel(port, null, provider);
                    ChannelServices.RegisterChannel(channel, true);
				}

				return channel;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Creates selected sessions based upon the aSessions parameter.
		/// </summary>
		/// <remarks>
		/// A Control Session is always created, regardless if it requested or not.  If the ControlServer AppSetting is not
		/// specified in the config file, it is assumed that the user is running all sessions on the client machine.  If a
		/// valid server name is specified in the ControlServer AppSetting, then the Control Session is created remotely.
		/// An invalid or unavailable server name in the ControlServer AppSetting will result in the user being prompted to
		/// run in "emergency" mode, which will create all sessions on the client machine.  Regardless of whether the Control
		/// Session is created locally or remotely, the Control Session then reads the config file of its host machine,
		/// looking for AppSetting entries for each type of server.
		/// 
		/// After the lists of servers have been determined, a session of each type requested is created.  If the server specified
		/// for the session is missing or invalid, the user will be prompted to run in "emergency" mode, which will create the
		/// session on the client machine.  Each session can be independently remote or local.
		/// 
		/// After each session has been created, the session is sponsored by an ISponsor object that is received from the client.
		/// </remarks>
		/// <param name="aSessions">
		/// Integer that contains bit flags indicating which types of sessions to create.  The eServerType enumerator contains
		/// the valid values for this parameter, and may be or'd together to request multiple sessions.
		/// </param>

        public void CreateSessions(int aSessions, bool verifyEnvironment = true)    // TT#1627-MD - stodd - attribute set filter
		{
			string allowDebugging;
			//Begin TT#708 - JScott - Services need a Retry availalbe.
			string serviceRetryCount;
			string serviceRetryInterval;
			//End TT#708 - JScott - Services need a Retry availalbe.
			string appSetControlServer = "";
			string appSetLocalStoreServer;
			string appSetLocalHierarchyServer;
			string appSetLocalApplicationServer;
			string appSetLocalHeaderServer;
			string appSetSuppressSchedulerMessage;
			bool localCtrlServer;
			bool localStoreServer;
			bool localHierServer;
			bool localAppServer;
			bool localHeaderServer;
			bool supressSchedulerMessage;
            // Begin TT#195 MD - JSmith - Add environment authentication
            //string controlServerDatabaseName;
            // End TT#195 MD
			string clientServerDatabaseName;
            // Begin TT#195 MD - JSmith - Add environment authentication
            //string storeServerDatabaseName;
            //string hierarchyServerDatabaseName;
            //string applicationServerDatabaseName;
            //string schedulerServerDatabaseName;
            //string headerServerDatabaseName;
            // End TT#195 MD
			ServerList controlServerList;
			ServerObjectFactory soFactory = null;
			ILease lease;

			try
			{
				// Read in AppSettings
                allowDebugging = MIDConfigurationManager.AppSettings["AllowDebugging"];

                if (allowDebugging != null)
                {
                    _allowDebugging = ConvertLocalAppSetting(allowDebugging);
                }

				//Begin TT#708 - JScott - Services need a Retry availalbe.
				serviceRetryCount = MIDConfigurationManager.AppSettings["ServiceRetryCount"];

				if (serviceRetryCount != null)
				{
					try
					{
						_serviceRetryCount = Math.Max(Convert.ToInt32(serviceRetryCount), 1);
					}
					catch (FormatException)
					{
						_serviceRetryCount = cServiceRetryCount;
					}
					catch (OverflowException)
					{
						_serviceRetryCount = cServiceRetryCount;
					}
					catch (Exception err)
					{
						string message = err.ToString();
						throw;
					}
				}

				serviceRetryInterval = MIDConfigurationManager.AppSettings["ServiceRetryInterval"];

				if (serviceRetryInterval != null)
				{
					try
					{
						_serviceRetryInterval = Math.Max(Convert.ToInt32(serviceRetryInterval), 500);
					}
					catch (FormatException)
					{
						_serviceRetryInterval = cServiceRetryInterval;
					}
					catch (OverflowException)
					{
						_serviceRetryInterval = cServiceRetryInterval;
					}
					catch (Exception err)
					{
						string message = err.ToString();
						throw;
					}
				}

				//End TT#708 - JScott - Services need a Retry availalbe.
				if (!_forceLocal)
				{
					appSetControlServer = MIDConfigurationManager.AppSettings["ControlServer"];
					appSetLocalStoreServer = MIDConfigurationManager.AppSettings["LocalStoreServer"];
					appSetLocalHierarchyServer = MIDConfigurationManager.AppSettings["LocalHierarchyServer"];
					appSetLocalApplicationServer = MIDConfigurationManager.AppSettings["LocalApplicationServer"];
					appSetLocalHeaderServer = "True";
					
					// Translate Local settings into boolean

					if (appSetControlServer == null)
					{
						localCtrlServer = true;
					}
					else
					{
						localCtrlServer = false;
					}

					localStoreServer = ConvertLocalAppSetting(appSetLocalStoreServer);
					localHierServer = ConvertLocalAppSetting(appSetLocalHierarchyServer);
					localAppServer = ConvertLocalAppSetting(appSetLocalApplicationServer);
					localHeaderServer = ConvertLocalAppSetting(appSetLocalHeaderServer);
				}
				else
				{
					localCtrlServer = true;
					localStoreServer = true;
					localHierServer = true;
					localAppServer = true;
					localHeaderServer = true;
				}

				//-----------------------
				// Create Control Session
				//
				// Check to see if a ControlServer AppSetting has been specified.  If so, contact the server to see if it
				// is alive.  If it is alive, register the ControlServerSession as a CAO.  Otherwise, send a message to the user
				// to confirm emergency mode.
				//-----------------------

				if (!localCtrlServer)
				{
					controlServerList = new ServerList(appSetControlServer);
					_controlServer = controlServerList.GetServer(eServerType.Control);

					if (_controlServer.Length > 0)
					{
						soFactory = (ServerObjectFactory)RemotingServices.Connect(typeof(ServerObjectFactory), "tcp://" + _controlServer + "/MRSServerObjectFactory.rem");

						if (soFactory == null)
						{
							throw new MIDException(eErrorLevel.severe,
								(int)eMIDTextCode.msg_ControlServerNotFound,
								"ServerObjectFactory object not defined on Control Service - processing terminated");
						}
					}
					else
					{
                        //Begin TT#900-MD -jsobek -Disable Emergency Mode Login
                        //if (_messageCallback.HandleMessage("Control Server could not be found.  Start in emergency mode?", "Control Server Error", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.OK)
                        //{
                        //    localCtrlServer = true;
                        //    localAppServer = true;
                        //    localStoreServer = true;
                        //    localHierServer = true;
                        //}
                        //else
                        //{
							// hard-code message if no connection string
							if (ConnectionString == null)
							{
								ConnectionString = MIDConfigurationManager.AppSettings["ConnectionString"];
							}

                            // Begin TT#2131-MD - JSmith - Halo Integration
                            if (ROExtractConnectionString == null)
                            {
                                ROExtractConnectionString = MIDConfigurationManager.AppSettings["ROExtractConnectionString"];
                            }
                            // End TT#2131-MD - JSmith - Halo Integration
							
							//Begin Assortment
							if (ConnectionString == null)
							{
								throw new MIDException (eErrorLevel.severe,
									(int)eMIDTextCode.msg_ControlServerNotFound,
									"Control Server not found - processing terminated");
							}
							else
							{
								MIDConnectionString.ConnectionString = ConnectionString;
								throw new MIDException (eErrorLevel.severe,
									(int)eMIDTextCode.msg_ControlServerNotFound,
									MIDText.GetText(eMIDTextCode.msg_ControlServerNotFound));
							}
						//}
                        //End TT#900-MD -jsobek -Disable Emergency Mode Login Unless Services are Local
					}
				}
				else
				{
					if (!localAppServer || !localStoreServer || !localHierServer)
					{
                        //Begin TT#900-MD -jsobek -Disable Emergency Mode Login
                        //if (_messageCallback.HandleMessage("Control Server was not specified, but some services were marked as remote.  Start all services local?", "Control Server Error", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.OK)
                        //{
                        //    localAppServer = true;
                        //    localStoreServer = true;
                        //    localHierServer = true;
                        //}
                        //else
                        //{
							// hard-code message if no connection string
							if (ConnectionString == null)
							{
								ConnectionString = MIDConfigurationManager.AppSettings["ConnectionString"];
							}

                            // Begin TT#2131-MD - JSmith - Halo Integration
                            if (ROExtractConnectionString == null)
                            {
                                ROExtractConnectionString = MIDConfigurationManager.AppSettings["ROExtractConnectionString"];
                            }
                            // End TT#2131-MD - JSmith - Halo Integration

							if (ConnectionString == null)
							{
								throw new MIDException (eErrorLevel.severe,
									(int)eMIDTextCode.msg_ControlServerNotFound,
									"Control Server not found - processing terminated");
							}
							else
							{
								MIDConnectionString.ConnectionString = ConnectionString;
								throw new MIDException (eErrorLevel.severe,
									(int)eMIDTextCode.msg_ControlServerNotFound,
									MIDText.GetText(eMIDTextCode.msg_ControlServerNotFound));
							}
						//}
                        //End TT#900-MD -jsobek -Disable Emergency Mode Login
					}
				}

                if (localCtrlServer)
                {
                    if (!ControlServerGlobal.Loaded)
                    {
                        ControlServerGlobal.Load(true);
                        _controlGlobalLoaded = true;
                    }

					//Begin TT#708 - JScott - Services need a Retry availalbe.
					//_controlServerSession = new ControlServerSession(true);
					_controlServerSessionRemote = new ControlServerSessionRemote(true);
					//End TT#708 - JScott - Services need a Retry availalbe.
				}
                else
                {
					//Begin TT#708 - JScott - Services need a Retry availalbe.
					//_controlServerSession = (ControlServerSession)soFactory.CreateSession(_controlServer.Split(':')[0]);
					//lease = (ILease)_controlServerSession.GetLifetimeService();
					_controlServerSessionRemote = (ControlServerSessionRemote)soFactory.CreateSession(_controlServer.Split(':')[0]);
					lease = (ILease)_controlServerSessionRemote.GetLifetimeService();
					//End TT#708 - JScott - Services need a Retry availalbe.
                    lease.Register(_sponsor);
                    _remoteServices = true;
				}

				//Begin TT#708 - JScott - Services need a Retry availalbe.
				//// Begin Track #6346 - JSmith - Duplicate rows after Rollup
				//_sessionID = _controlServerSession.GetSessionID();
				//// End Track #6346
				_sessionID = _controlServerSessionRemote.GetSessionID();
				//End TT#708 - JScott - Services need a Retry availalbe.

				//---------------------------
				// Get Connection String
				//
				// Retrieve Connections String from Control Service is running remote; otherwise pull from config file
				//---------------------------

				if (ConnectionString == null)
				{
					if (!localCtrlServer)
					{
						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//ConnectionString = ControlServerSession.GetConnectionString();
						ConnectionString = _controlServerSessionRemote.GetConnectionString();
						//End TT#708 - JScott - Services need a Retry availalbe.
					}
					else
					{
						ConnectionString = MIDConfigurationManager.AppSettings["ConnectionString"];
					}
				}

				MIDConnectionString.ConnectionString = ConnectionString;

                // Begin TT#2131-MD - JSmith - Halo Integration
                if (ROExtractConnectionString == null)
                {
                    if (!localCtrlServer)
                    {
                        ROExtractConnectionString = _controlServerSessionRemote.GetROExtractConnectionString();
                    }
                    else
                    {
                        ROExtractConnectionString = MIDConfigurationManager.AppSettings["ROExtractConnectionString"];
                    }
                }
                // End TT#2131-MD - JSmith - Halo Integration

                //---------------------------
                // Retrieve list of servers from Control Service
                //
                // Get list of the remaining services from the Control Service
                //---------------------------

                //Begin TT#1165 - JScott - Login Performance
                ////Begin TT#708 - JScott - Services need a Retry availalbe.
                ////_serverGroup = _controlServerSession.GetServers();
                //_serverGroup = _controlServerSessionRemote.GetServers();
                ////End TT#708 - JScott - Services need a Retry availalbe.
                _serverGroup = _controlServerSessionRemote.GetServers(true, localStoreServer, localHierServer, localAppServer, false, localHeaderServer);
				//End TT#1165 - JScott - Login Performance

				//---------------------------
				// Create Client Session
				//
				// Always created as local
				//---------------------------

				if (System.Convert.ToBoolean(aSessions & (int)eServerType.Client, CultureInfo.CurrentUICulture))
				{
					if (!ClientServerGlobal.Loaded)
					{
						ClientServerGlobal.Load();
						_clientGlobalLoaded = true;
					}

					//Begin TT#708 - JScott - Services need a Retry availalbe.
					//_clientServerSession = new ClientServerSession(true, _controlServerSession.ThreadID);
					_clientServerSessionRemote = new ClientServerSessionRemote(true, _controlServerSessionRemote.ThreadID);
					//End TT#708 - JScott - Services need a Retry availalbe.
				}

				//---------------------------
				// Create Store Session
				//
				// Check to see if the Client has specified a local Store Server (or OK'd emergency mode).  If not, check to see
				// if the ControlServerSession found a valid Store Server.  If so, register the StoreServerSession
				// as a CAO.  Otherwise, send a message to the user to confirm emergency mode.
				//---------------------------

				if (System.Convert.ToBoolean(aSessions & (int)eServerType.Store, CultureInfo.CurrentUICulture))
				{
					if (!localStoreServer)
					{
						if (_serverGroup.StoreServer.Length > 0)
						{
							soFactory = (ServerObjectFactory)RemotingServices.Connect(typeof(ServerObjectFactory), "tcp://" + _serverGroup.StoreServer + "/MRSServerObjectFactory.rem");

							if (soFactory == null)
							{
								throw new MIDException(eErrorLevel.severe,
									(int)eMIDTextCode.msg_ControlServerNotFound,
									"ServerObjectFactory object not defined on Store Service");
							}
						}
						else
						{
                            //Begin TT#900-MD -jsobek -Disable Emergency Mode Login
                            //if (_messageCallback.HandleMessage("Store Server could not be found.  Start in emergency mode?", "Store Server Error", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.OK)
                            //{
                            //    localStoreServer = true;
                            //}
                            //else
                            //{
								throw new MIDException (eErrorLevel.severe,
									(int)eMIDTextCode.msg_StoreServerNotFound,
									MIDText.GetText(eMIDTextCode.msg_StoreServerNotFound));
							//}
                            //End TT#900-MD -jsobek -Disable Emergency Mode Login
						}
					}

					if (localStoreServer)
					{
						if (!StoreServerGlobal.Loaded)
						{
                            // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                            //StoreServerGlobal.Load();
                            StoreServerGlobal.Load(true);
                            // End TT#189
							_storeGlobalLoaded = true;
						}

						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//_storeServerSession = new StoreServerSession(true);
						_storeServerSessionRemote = new StoreServerSessionRemote(true);
						//End TT#708 - JScott - Services need a Retry availalbe.
					}
					else
					{
						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//_storeServerSession = (StoreServerSession)soFactory.CreateSession(_serverGroup.StoreServer.Split(':')[0]);
						//lease = (ILease)_storeServerSession.GetLifetimeService();
						_storeServerSessionRemote = (StoreServerSessionRemote)soFactory.CreateSession(_serverGroup.StoreServer.Split(':')[0]);
						lease = (ILease)_storeServerSessionRemote.GetLifetimeService();
						//End TT#708 - JScott - Services need a Retry availalbe.
						lease.Register(_sponsor);
						_remoteServices = true;
					}
				}

				//---------------------------
				// Create Hierarchy Session
				//
				// Check to see if the Client has specified a local Hierarchy Server (or OK'd emergency mode).  If not, check to see
				// if the ControlServerSession found a valid Hierarchy Server.  If so, register the HierarchyServerSession
				// as a CAO.  Otherwise, send a message to the user to confirm emergency mode.
				//---------------------------

				if (System.Convert.ToBoolean(aSessions & (int)eServerType.Hierarchy, CultureInfo.CurrentUICulture))
				{
					if (!localHierServer)
					{
						if (_serverGroup.HierarchyServer.Length > 0)
						{
							soFactory = (ServerObjectFactory)RemotingServices.Connect(typeof(ServerObjectFactory), "tcp://" + _serverGroup.HierarchyServer + "/MRSServerObjectFactory.rem");

							if (soFactory == null)
							{
								throw new MIDException(eErrorLevel.severe,
									(int)eMIDTextCode.msg_ControlServerNotFound,
									"ServerObjectFactory object not defined on Hierarchy Service");
							}
						}
						else
						{
                            //Begin TT#900-MD -jsobek -Disable Emergency Mode Login
                            //if (_messageCallback.HandleMessage("Hierarchy Server could not be found.  Start in emergency mode?", "Hierarchy Server Error", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.OK)
                            //{
                            //    localHierServer = true;
                            //}
                            //else
                            //{
								throw new MIDException (eErrorLevel.severe,
									(int)eMIDTextCode.msg_HierarchyServerNotFound,
									MIDText.GetText(eMIDTextCode.msg_HierarchyServerNotFound));
							//}
                            //End TT#900-MD -jsobek -Disable Emergency Mode Login
						}
					}

					if (localHierServer)
					{
						if (!HierarchyServerGlobal.Loaded)
						{
                            // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                            //HierarchyServerGlobal.Load();
                            HierarchyServerGlobal.Load(true);
                            // End TT#189
							_hierarchyGlobalLoaded = true;
						}

						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//_hierarchyServerSession = new HierarchyServerSession(true);
						_hierarchyServerSessionRemote = new HierarchyServerSessionRemote(true);
						//End TT#708 - JScott - Services need a Retry availalbe.
					}
					else
					{
						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//_hierarchyServerSession = (HierarchyServerSession)soFactory.CreateSession(_serverGroup.HierarchyServer.Split(':')[0]);
						//lease = (ILease)_hierarchyServerSession.GetLifetimeService();
						_hierarchyServerSessionRemote = (HierarchyServerSessionRemote)soFactory.CreateSession(_serverGroup.HierarchyServer.Split(':')[0]);
						lease = (ILease)_hierarchyServerSessionRemote.GetLifetimeService();
						//End TT#708 - JScott - Services need a Retry availalbe.
						lease.Register(_sponsor);
						_remoteServices = true;
					}
				}

				//---------------------------
				// Create Application Session
				//
				// Check to see if the Client has specified a local Application Server (or OK'd emergency mode).  If not, check to see
				// if the ControlServerSession found a valid Application Server.  If so, register the ApplicationServerSession
				// as a CAO.  Otherwise, send a message to the user to confirm emergency mode.
				//---------------------------

				if (System.Convert.ToBoolean(aSessions & (int)eServerType.Application, CultureInfo.CurrentUICulture))
				{
					if (!localAppServer)
					{
						if (_serverGroup.ApplicationServer.Length > 0)
						{
							soFactory = (ServerObjectFactory)RemotingServices.Connect(typeof(ServerObjectFactory), "tcp://" + _serverGroup.ApplicationServer + "/MRSServerObjectFactory.rem");

							if (soFactory == null)
							{
								throw new MIDException(eErrorLevel.severe,
									(int)eMIDTextCode.msg_ControlServerNotFound,
									"ServerObjectFactory object not defined on Application Service");
							}
						}
						else
						{
                            //Begin TT#900-MD -jsobek -Disable Emergency Mode Login
                            //if (_messageCallback.HandleMessage("Application Server could not be found.  Start in emergency mode?", "Application Server Error", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.OK)
                            //{
                            //    localAppServer = true;
                            //}
                            //else
                            //{
								throw new MIDException (eErrorLevel.severe,
									(int)eMIDTextCode.msg_ApplicationServerNotFound,
									MIDText.GetText(eMIDTextCode.msg_ApplicationServerNotFound));
							//}
                            //End TT#900-MD -jsobek -Disable Emergency Mode Login
						}
					}

					if (localAppServer)
					{
						if (!ApplicationServerGlobal.Loaded)
						{
                            // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                            //ApplicationServerGlobal.Load();
                            ApplicationServerGlobal.Load(true);
                            // End TT#189  
							_applicationGlobalLoaded = true;
						}

						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//_applicationServerSession = new ApplicationServerSession(true);
						_applicationServerSessionRemote = new ApplicationServerSessionRemote(true);
						//End TT#708 - JScott - Services need a Retry availalbe.
					}
					else
					{
						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//_applicationServerSession = (ApplicationServerSession)soFactory.CreateSession(_serverGroup.ApplicationServer.Split(':')[0]);
						//lease = (ILease)_applicationServerSession.GetLifetimeService();
						_applicationServerSessionRemote = (ApplicationServerSessionRemote)soFactory.CreateSession(_serverGroup.ApplicationServer.Split(':')[0]);
						lease = (ILease)_applicationServerSessionRemote.GetLifetimeService();
						//End TT#708 - JScott - Services need a Retry availalbe.
						lease.Register(_sponsor);
						_remoteServices = true;
					}
				}

				//---------------------------
				// Create Scheduler Session
				//
				// Check to see if the Client has specified a local Scheduler Server (or OK'd emergency mode).  If not, check to see
				// if the ControlServerSession found a valid Scheduler Server.  If so, register the SchedulerServerSession
				// as a CAO.  Otherwise, send a message to the user to confirm emergency mode.
				//---------------------------

				if (System.Convert.ToBoolean(aSessions & (int)eServerType.Scheduler, CultureInfo.CurrentUICulture))
				{
					if (_serverGroup.SchedulerServer.Length > 0)
					{
						soFactory = (ServerObjectFactory)RemotingServices.Connect(typeof(ServerObjectFactory), "tcp://" + _serverGroup.SchedulerServer + "/MRSServerObjectFactory.rem");

						if (soFactory == null)
						{
							throw new MIDException(eErrorLevel.severe,
								(int)eMIDTextCode.msg_ControlServerNotFound,
								"ServerObjectFactory object not defined on Scheduler Service");
						}

						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//_schedulerServerSession = (SchedulerServerSession)soFactory.CreateSession(_serverGroup.SchedulerServer.Split(':')[0]);
						//lease = (ILease)_schedulerServerSession.GetLifetimeService();
						_schedulerServerSessionRemote = (SchedulerServerSessionRemote)soFactory.CreateSession(_serverGroup.SchedulerServer.Split(':')[0]);
						lease = (ILease)_schedulerServerSessionRemote.GetLifetimeService();
						//End TT#708 - JScott - Services need a Retry availalbe.
						lease.Register(_sponsor);
						_remoteServices = true;
					}
					else
					{
						appSetSuppressSchedulerMessage = MIDConfigurationManager.AppSettings["SupressMissingSchedulerServiceMessage"];
						supressSchedulerMessage = ConvertLocalAppSetting(appSetSuppressSchedulerMessage);

						if (supressSchedulerMessage ||
							_messageCallback.HandleMessage(eMIDTextCode.msg_ContinueWithoutScheduler, "Scheduler Server Error", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.OK)
						{
							//Begin TT#708 - JScott - Services need a Retry availalbe.
							//_schedulerServerSession = null;
							_schedulerServerSessionRemote = null;
							//End TT#708 - JScott - Services need a Retry availalbe.
						}
						else
						{
							throw new MIDException (eErrorLevel.severe,
								(int)eMIDTextCode.msg_SchedulerServerNotFound,
								MIDText.GetText(eMIDTextCode.msg_SchedulerServerNotFound));
						}
					}
				}

				//---------------------------
				// Create Header Session
				//
				// Check to see if the Client has specified a local Header Server (or OK'd emergency mode).  If not, check to see
				// if the ControlServerSession found a valid Header Server.  If so, register the HeaderServerSession
				// as a CAO.  Otherwise, send a message to the user to confirm emergency mode.
				//---------------------------

				if (System.Convert.ToBoolean(aSessions & (int)eServerType.Header, CultureInfo.CurrentUICulture))
				{
					if (!localHeaderServer)
					{
						if (_serverGroup.HeaderServer.Length > 0)
						{
							soFactory = (ServerObjectFactory)RemotingServices.Connect(typeof(ServerObjectFactory), "tcp://" + _serverGroup.HeaderServer + "/MRSServerObjectFactory.rem");

							if (soFactory == null)
							{
								throw new MIDException(eErrorLevel.severe,
									(int)eMIDTextCode.msg_ControlServerNotFound,
									"ServerObjectFactory object not defined on Header Service");
							}
						}
						else
						{
                            //Begin TT#900-MD -jsobek -Disable Emergency Mode Login
                            //if (_messageCallback.HandleMessage("Header Server could not be found.  Start in emergency mode?", "Header Server Error", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.OK)
                            //{
                            //    localHeaderServer = true;
                            //}
                            //else
                            //{
								throw new MIDException (eErrorLevel.severe,
									(int)eMIDTextCode.msg_HeaderServerNotFound,
									MIDText.GetText(eMIDTextCode.msg_HeaderServerNotFound));
							//}
                            //End TT#900-MD -jsobek -Disable Emergency Mode Login
						}
					}

					if (localHeaderServer)
					{
						if (!HeaderServerGlobal.Loaded)
						{
                            // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                            //HeaderServerGlobal.Load();
                            HeaderServerGlobal.Load(true);
                            // End TT#189  
							_headerGlobalLoaded = true;
						}

						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//_headerServerSession = new HeaderServerSession(true);
						_headerServerSessionRemote = new HeaderServerSessionRemote(true);
						//End TT#708 - JScott - Services need a Retry availalbe.
					}
					else
					{
						//Begin TT#708 - JScott - Services need a Retry availalbe.
						//_headerServerSession = (HeaderServerSession)soFactory.CreateSession(_serverGroup.HeaderServer.Split(':')[0]);
						//lease = (ILease)_headerServerSession.GetLifetimeService();
						_headerServerSessionRemote = (HeaderServerSessionRemote)soFactory.CreateSession(_serverGroup.HeaderServer.Split(':')[0]);
						lease = (ILease)_headerServerSessionRemote.GetLifetimeService();
						//End TT#708 - JScott - Services need a Retry availalbe.
						lease.Register(_sponsor);
						_remoteServices = true;
					}
				}
				//End Fix - JScott - Correct Group Dynamite Conneciton problem

				//Begin TT#708 - JScott - Services need a Retry availalbe.
				//--------------------
				// Create Local Sessions
				//
				// Create Local Sessions for pass-through calls to the Remote Sessions
				//--------------------

				if (_controlServerSessionRemote != null)
				{
					_controlServerSession = new ControlServerSession(_controlServerSessionRemote, _serviceRetryCount, _serviceRetryInterval);
				}

				if (_clientServerSessionRemote != null)
				{
					_clientServerSession = new ClientServerSession(_clientServerSessionRemote, _serviceRetryCount, _serviceRetryInterval);
				}

				if (_storeServerSessionRemote != null)
				{
					_storeServerSession = new StoreServerSession(_storeServerSessionRemote, _serviceRetryCount, _serviceRetryInterval);
				}

				if (_hierarchyServerSessionRemote != null)
				{
					_hierarchyServerSession = new HierarchyServerSession(_hierarchyServerSessionRemote, _serviceRetryCount, _serviceRetryInterval);
				}

				if (_applicationServerSessionRemote != null)
				{
					_applicationServerSession = new ApplicationServerSession(_applicationServerSessionRemote, _serviceRetryCount, _serviceRetryInterval);
				}

				if (_schedulerServerSessionRemote != null)
				{
					_schedulerServerSession = new SchedulerServerSession(_schedulerServerSessionRemote, _serviceRetryCount, _serviceRetryInterval);
				}

				if (_headerServerSessionRemote != null)
				{
					_headerServerSession = new HeaderServerSession(_headerServerSessionRemote, _serviceRetryCount, _serviceRetryInterval);
				}

				//End TT#708 - JScott - Services need a Retry availalbe.
				//--------------------
				// Initialize Sessions
				//
				// Pass each session a pointer to this SessionAddressBlock to allow each session to communicate with each other.
				//--------------------


//				//----------------------------------
//				// Assign Misc Application Settings
//				//----------------------------------
//				appSetMonitorForecast = MIDConfigurationManager.AppSettings["MonitorForecast"];
//				_monitorForecastAppSetting = ConvertLocalAppSetting(appSetMonitorForecast);
//				//----------------------------------

				//Begin TT#708 - JScott - Services need a Retry availalbe.
				//_controlServerSession.SetSAB(this);
				//controlServerDatabaseName = this.ControlServerSession.GetDatabaseName();

				//if (_clientServerSession != null)
				//{
				//    _clientServerSession.SetSAB(this);
				//    clientServerDatabaseName = this.ClientServerSession.GetDatabaseName();
				//}
				//else
				//{
				//    clientServerDatabaseName = controlServerDatabaseName;
				//}

				//if (_storeServerSession != null)
				//{
				//    _storeServerSession.SetSAB(this);
				//    storeServerDatabaseName = this.StoreServerSession.GetDatabaseName();
				//}
				//else
				//{
				//    storeServerDatabaseName = controlServerDatabaseName;
				//}

				//if (_hierarchyServerSession != null)
				//{
				//    _hierarchyServerSession.SetSAB(this);
				//    hierarchyServerDatabaseName = this.HierarchyServerSession.GetDatabaseName();
				//}
				//else
				//{
				//    hierarchyServerDatabaseName = controlServerDatabaseName;
				//}

				//if (_applicationServerSession != null)
				//{
				//    _applicationServerSession.SetSAB(this);
				//    applicationServerDatabaseName = this.ApplicationServerSession.GetDatabaseName();
				//}
				//else
				//{
				//    applicationServerDatabaseName = controlServerDatabaseName;
				//}

				//if (_schedulerServerSession != null)
				//{
				//    _schedulerServerSession.SetSAB(this);
				//    schedulerServerDatabaseName = this.SchedulerServerSession.GetDatabaseName();
				//}
				//else
				//{
				//    schedulerServerDatabaseName = controlServerDatabaseName;
				//}

				//if (_headerServerSession != null)
				//{
				//    _headerServerSession.SetSAB(this);
				//    headerServerDatabaseName = this.HeaderServerSession.GetDatabaseName();
				//}
				//else
				//{
				//    headerServerDatabaseName = controlServerDatabaseName;
				//}
				_controlServerSessionRemote.SetSAB(this);
                // Begin TT#195 MD - JSmith - Add environment authentication
                //controlServerDatabaseName = _controlServerSessionRemote.GetDatabaseName();
                // End TT#195 MD

				if (_clientServerSessionRemote != null)
				{
					_clientServerSessionRemote.SetSAB(this);
					clientServerDatabaseName = _clientServerSessionRemote.GetDatabaseName();
				}
				else
				{
                    clientServerDatabaseName = _controlServerSessionRemote.GetDatabaseName();
				}

                // Begin TT#2131-MD - JSmith - Halo Integration
                _ROExtractEnabled = false;
                if (!string.IsNullOrWhiteSpace(_ROExtractConnectionString)
                    && ClientServerSession.GlobalOptions.AppConfig.AnalyticsInstalled)
                {
                   try
                    {
                        ROExtractData ROExtractData = new ROExtractData(_ROExtractConnectionString);
                        if (ROExtractData.RO_Extract_Exists())
                        {
                            _ROExtractEnabled = true;
                        }
                    }
                    catch
                    {
                        EventLog.WriteEntry("MIDRetail", "Not an Analytics database.  Extract will not be allowed.", EventLogEntryType.Error);
                    }
                }
                MIDEnvironment.ExtractIsEnabled = ROExtractEnabled;
                // End TT#2131-MD - JSmith - Halo Integration

                // Begin TT#195 MD - JSmith - Add environment authentication
                if (_storeServerSessionRemote != null)
                {
                    _storeServerSessionRemote.SetSAB(this);
                    //storeServerDatabaseName = _storeServerSessionRemote.GetDatabaseName();
                    // Begin TT#1884-MD - JSmith - Color Code Load does not process correctly
                    StoreMgmt.LoadInitialStoresAndGroups(this, this.ClientServerSession);
                    // End TT#1884-MD - JSmith - Color Code Load does not process correctly
                }
                // Begin TT#1861-MD - JSmith - Serialization error accessing the Audit
                //StoreMgmt.LoadInitialStoresAndGroups(this); //TT#1517-MD -jsobek -Store Service Optimization
                // Begin TT#1884-MD - JSmith - Color Code Load does not process correctly
                //StoreMgmt.LoadInitialStoresAndGroups(this, this.ClientServerSession); //TT#1517-MD -jsobek -Store Service Optimization
                // End TT#1884-MD - JSmith - Color Code Load does not process correctly
                // End TT#1861-MD - JSmith - Serialization error accessing the Audit
                //else
                //{
                //    storeServerDatabaseName = controlServerDatabaseName;
                //}

                if (_hierarchyServerSessionRemote != null)
                {
                    _hierarchyServerSessionRemote.SetSAB(this);
                    //hierarchyServerDatabaseName = _hierarchyServerSessionRemote.GetDatabaseName();
                }
                //else
                //{
                //    hierarchyServerDatabaseName = controlServerDatabaseName;
                //}

                if (_applicationServerSessionRemote != null)
                {
                    _applicationServerSessionRemote.SetSAB(this);
                    //applicationServerDatabaseName = _applicationServerSessionRemote.GetDatabaseName();
                }
                //else
                //{
                //    applicationServerDatabaseName = controlServerDatabaseName;
                //}

                if (_schedulerServerSessionRemote != null)
                {
                    _schedulerServerSessionRemote.SetSAB(this);
                    //schedulerServerDatabaseName = _schedulerServerSessionRemote.GetDatabaseName();
                }
                //else
                //{
                //    schedulerServerDatabaseName = controlServerDatabaseName;
                //}

                if (_headerServerSessionRemote != null)
                {
                    _headerServerSessionRemote.SetSAB(this);
                    //headerServerDatabaseName = _headerServerSessionRemote.GetDatabaseName();
                }
                //else
                //{
                //    headerServerDatabaseName = controlServerDatabaseName;
                //}
                ////End TT#708 - JScott - Services need a Retry availalbe.

            //    if (clientServerDatabaseName != controlServerDatabaseName ||
            //        clientServerDatabaseName != storeServerDatabaseName ||
            //        clientServerDatabaseName != hierarchyServerDatabaseName ||
            //        clientServerDatabaseName != applicationServerDatabaseName ||
            //        clientServerDatabaseName != schedulerServerDatabaseName ||
            //        clientServerDatabaseName != headerServerDatabaseName)
            //{
            //        if (!EventLog.SourceExists("MIDRetail"))
            //        {
            //            EventLog.CreateEventSource("MIDRetail", null);
            //        }

            //        EventLog.WriteEntry("MIDRetail", "Client database:" + clientServerDatabaseName, EventLogEntryType.Error);
            //        EventLog.WriteEntry("MIDRetail", "Control database:" + controlServerDatabaseName, EventLogEntryType.Error);
            //        EventLog.WriteEntry("MIDRetail", "Store database:" + storeServerDatabaseName, EventLogEntryType.Error);
            //        EventLog.WriteEntry("MIDRetail", "Hierarchy database:" + hierarchyServerDatabaseName, EventLogEntryType.Error);
            //        EventLog.WriteEntry("MIDRetail", "Application database:" + applicationServerDatabaseName, EventLogEntryType.Error);
            //        EventLog.WriteEntry("MIDRetail", "Scheduler database:" + schedulerServerDatabaseName, EventLogEntryType.Error);
            //        EventLog.WriteEntry("MIDRetail", "Header database:" + headerServerDatabaseName, EventLogEntryType.Error);

            //        throw new MIDException (eErrorLevel.severe,
            //            (int)eMIDTextCode.msg_DatabaseMismatch,
            //            MIDText.GetText(eMIDTextCode.msg_DatabaseMismatch));
            //    }

                
                try
                {
                    ClientProfile cp = new ClientProfile(Include.NoRID);
                    cp.DBName = clientServerDatabaseName;
                    object[] customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyConfigurationAttribute), true);
                    cp.Configuration = string.IsNullOrEmpty(((AssemblyConfigurationAttribute)customAttributes[0]).Configuration) ? "Base" : ((AssemblyConfigurationAttribute)customAttributes[0]).Configuration;

                    System.Diagnostics.FileVersionInfo fvi = null;
                    try
                    {
                        fvi = Include.GetMainAssemblyInfo();
                    }
                    catch
                    {
                    }

                    if (fvi == null)
                    {
                        fvi = Include.GetCurrentAssemblyInfo();
                    }

                    cp.Version = fvi.ProductVersion;
                    cp.FileMajorPart = fvi.FileMajorPart;
                    cp.FileMinorPart = fvi.FileMinorPart;
                    cp.FileBuildPart = fvi.FileBuildPart;
                    cp.FilePrivatePart = fvi.FilePrivatePart;

                    // Begin TT#1627-MD - stodd - attribute set filter
                    if (verifyEnvironment)
                    {
                        // verify client
                        if (ClientServerSession != null)
                        {
                            ClientServerSession.VerifyEnvironment(cp);
                        }
                        // verify control service
                        if (_controlServerSessionRemote != null &&
                            ControlServerSession.isSessionRunningRemote())
                        {
                            _controlServerSessionRemote.VerifyEnvironment(cp);
                        }
                        // verify application service
                        if (_applicationServerSessionRemote != null &&
                            ApplicationServerSession.isSessionRunningRemote())
                        {
                            _applicationServerSessionRemote.VerifyEnvironment(cp);
                        }
                        // verify hierarchy service
                        if (_hierarchyServerSessionRemote != null &&
                            HierarchyServerSession.isSessionRunningRemote())
                        {
                            _hierarchyServerSessionRemote.VerifyEnvironment(cp);
                        }
                        // verify store service
                        if (_storeServerSessionRemote != null &&
                            StoreServerSession.isSessionRunningRemote())
                        {
                            _storeServerSessionRemote.VerifyEnvironment(cp);
                        }
                        // verify header service
                        if (_headerServerSessionRemote != null &&
                            HeaderServerSession.isSessionRunningRemote()
                            )
                        {
                            _headerServerSessionRemote.VerifyEnvironment(cp);
                        }
                        // verify scheduler service
                        if (_schedulerServerSessionRemote != null &&
                            SchedulerServerSession.isSessionRunningRemote())
                        {
                            _schedulerServerSessionRemote.VerifyEnvironment(cp);
                        }
                    }
                    // End TT#1627-MD - stodd - attribute set filter
                }
                //catch (MIDException ex)
                //{
                //    switch (ex.ErrorNumber)
                //    {
                //        case (int)eMIDTextCode.msg_DatabaseMismatch:
                //            _clientServerSession.Audit.Log_Exception(
                //            break;
                //        case (int)eMIDTextCode.msg_ServicesMismatch:
                //            break;
                //    }

                //    throw;
                //}
                catch
                {
                    throw;
                }
                // End TT#195 MD

                _isServicesAvailable = true; //TT#899-MD -jsobek -Services Unavailable Login Message


			}
			catch
			{
				throw;
			}
		}
		// BEGIN TT#1156
        public void GetServiceServerInfo(out string appSetControlServer, out string appSetLocalStoreServer, out string appSetLocalHierarchyServer, out string appSetLocalApplicationServer)
        {
            appSetControlServer = MIDConfigurationManager.AppSettings["ControlServer"];
            appSetLocalStoreServer = MIDConfigurationManager.AppSettings["LocalStoreServer"];
            appSetLocalHierarchyServer = MIDConfigurationManager.AppSettings["LocalHierarchyServer"];
            appSetLocalApplicationServer = MIDConfigurationManager.AppSettings["LocalApplicationServer"];
        }
		// END TT#1156
        //Begin TT#901-MD -jsobek -Batch Only Mode
        public void GetSocketSettingsFromConfigFile(out string controlServerName, out int controlServerPort, out double clientTimerIntervalInMilliseconds, out double serverTimerIntervalInMilliseconds)
        {
            _controlServerSessionRemote.GetSocketSettingsFromConfigFile(out controlServerName, out controlServerPort, out clientTimerIntervalInMilliseconds, out serverTimerIntervalInMilliseconds);   
        }

        public bool IsApplicationInBatchOnlyMode()
        {
            return this._controlServerSessionRemote.IsApplicationInBatchOnlyMode();
        }
        public string GetBatchModeLastChangedBy()
        {
            return this._controlServerSessionRemote.BatchModeLastChangedBy();
        }
        public bool IsRemoteServices()
        {
            return _remoteServices;
        }
        public void PerformClientCommandFromControlService(string command, string tagInfo)
        {
            if (command == SocketSharedRoutines.SocketClientCommands.ShutDown.commandName)
            {
                this.PerformClientShutDown(tagInfo);
            }
            if (command == SocketSharedRoutines.SocketClientCommands.ShowMessage.commandName)
            {
                System.Windows.Forms.MessageBox.Show(tagInfo, RemoteSystemOptions.Messages.MessageForClientTitle);
            }
            if (command == SocketSharedRoutines.SocketClientCommands.GiveUserInfo.commandName)
            {
                string userInfo = tagInfo;
                userInfo += SocketSharedRoutines.Tags.rowStart;
                userInfo += SocketSharedRoutines.Tags.userNameStart + ClientServerSession.GetUserName(ClientServerSession.UserRID) + SocketSharedRoutines.Tags.userNameEnd;
                userInfo += SocketSharedRoutines.Tags.clientTypeStart + MIDText.GetTextOnly(eMIDTextCode.lbl_RemoteSystemOptions_ShowCurrentUserGrid_ClientType_Windows) + SocketSharedRoutines.Tags.clientTypeEnd;
                userInfo += SocketSharedRoutines.Tags.machineNameStart + ClientServerSession.GetMachineName() + SocketSharedRoutines.Tags.machineNameEnd;
                userInfo += SocketSharedRoutines.Tags.appStatusStart + "Logged In" + SocketSharedRoutines.Tags.appStatusEnd;
                userInfo += SocketSharedRoutines.Tags.rowEnd;
                clientSocketManager.SendCommandToServer(SocketSharedRoutines.SocketServerCommands.TakeUserInfo, userInfo);
            }
            if (command == SocketSharedRoutines.SocketClientCommands.ReceiveCurrentUsers.commandName)
            {
                string info = tagInfo;
                int currentRowPosition = info.IndexOf(SocketSharedRoutines.Tags.rowStart);
                while (currentRowPosition != -1)
                {
                    string sRow = SocketSharedRoutines.GetInfoFromTags(info, SocketSharedRoutines.Tags.rowStart, SocketSharedRoutines.Tags.rowEnd);
                    System.Data.DataRow dr = dsCurrentUsers.Tables[0].NewRow();
                    dr["USER_NAME"] = SocketSharedRoutines.GetInfoFromTags(info, SocketSharedRoutines.Tags.userNameStart, SocketSharedRoutines.Tags.userNameEnd);
                    dr["CLIENT_TYPE"] = SocketSharedRoutines.GetInfoFromTags(info, SocketSharedRoutines.Tags.clientTypeStart, SocketSharedRoutines.Tags.clientTypeEnd);
                    dr["MACHINE_NAME"] = SocketSharedRoutines.GetInfoFromTags(info, SocketSharedRoutines.Tags.machineNameStart, SocketSharedRoutines.Tags.machineNameEnd);
                    dr["APP_STATUS"] = SocketSharedRoutines.GetInfoFromTags(info, SocketSharedRoutines.Tags.appStatusStart, SocketSharedRoutines.Tags.appStatusEnd);
                    dr["CLIENT_IP"] = SocketSharedRoutines.GetInfoFromTags(info, SocketSharedRoutines.Tags.clientIPAddressStart, SocketSharedRoutines.Tags.clientIPAddressEnd);
                    dr["CLIENT_PORT"] = SocketSharedRoutines.GetInfoFromTags(info, SocketSharedRoutines.Tags.clientPortStart, SocketSharedRoutines.Tags.clientPortEnd);
                    dsCurrentUsers.Tables[0].Rows.Add(dr);
                    int nextRowPosition = info.IndexOf(SocketSharedRoutines.Tags.rowEnd) + SocketSharedRoutines.Tags.rowEnd.Length;
                    info = info.Substring(nextRowPosition);
                    currentRowPosition = info.IndexOf(SocketSharedRoutines.Tags.rowStart);
                }
                globalOptionsUpdateCurrentUserCount.Invoke(dsCurrentUsers.Tables[0].Rows.Count);
            }
        }
        public System.Data.DataSet dsCurrentUsers;
        public delegate void UpdateCurrentUserCountDelegate(int currentUserCount);
        public UpdateCurrentUserCountDelegate globalOptionsUpdateCurrentUserCount;
        public void InitializeCurrentUserDataSet()
        {
            dsCurrentUsers = new System.Data.DataSet();
            dsCurrentUsers.Tables.Add("CurrentUsers");
            dsCurrentUsers.Tables[0].Columns.Add("USER_NAME");
            dsCurrentUsers.Tables[0].Columns.Add("CLIENT_TYPE");
            dsCurrentUsers.Tables[0].Columns.Add("MACHINE_NAME");
            dsCurrentUsers.Tables[0].Columns.Add("APP_STATUS");
            dsCurrentUsers.Tables[0].Columns.Add("CLIENT_IP");
            dsCurrentUsers.Tables[0].Columns.Add("CLIENT_PORT");
        }
        private void PerformClientShutDown(string tagInfo)
        {
            //Add audit log message
            _clientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Client App for " + _clientServerSession.GetUserName(_clientServerSession.UserRID) + " on " + _clientServerSession.GetMachineName() + "was forced closed by " + tagInfo, SocketSharedRoutines.moduleNameForAuditLogs);

            // make sure all locks are cleaned up.
            MIDEnqueue midNQ = new MIDEnqueue();
            try
            {
                midNQ.OpenUpdateConnection();
                midNQ.Enqueue_DeleteAll(this.ClientServerSession.UserRID, this.ClientServerSession.ThreadID);
                midNQ.CommitData();
            }
            catch
            {
                //swallow any errors during shutdown
            }
            finally
            {
                midNQ.CloseUpdateConnection();
            }

            if (this.clientSocketManager != null)
            {
                this.clientSocketManager.StopClient();
            }

            try
            {
                this.CloseSessions();
            }
            catch
            {
                //swallow any errors during shutdown
            }


            string args = "\"" + RemoteSystemOptions.Messages.ShutdownDisplayMessagePrefix + tagInfo + "\"";
            ProcessStartInfo DisplayMsgProcess = new ProcessStartInfo("DisplayMessage.exe", args);

            DisplayMsgProcess.WorkingDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            Process.Start(DisplayMsgProcess);

            try
            {
                Environment.Exit(0);
            }
            catch
            {
                //swallow any errors during shutdown
            }
        }
        //End TT#901-MD -jsobek -Batch Only Mode

//		public string GetMonitorFilePathAppSetting()
//		{
//			return _applicationServerSession.MonitorForecastFilePath;
//		}

		public eMIDMessageLevel CloseSessions()
		{
			try
			{
                //BEGIN TT#1644 - MD - DOConnell - API Process Control
                string MIDOnlyFunctionsStr = MIDConfigurationManager.AppSettings["MIDOnlyFunctions"];
               
                if (MIDOnlyFunctionsStr != null && _allowDebugging)
                {
                    MIDOnlyFunctionsStr = MIDOnlyFunctionsStr.ToLower();

                    if (MIDOnlyFunctionsStr == "true" || MIDOnlyFunctionsStr == "yes" || MIDOnlyFunctionsStr == "t" || MIDOnlyFunctionsStr == "y" || MIDOnlyFunctionsStr == "1")
                    {
                        EventLog.WriteEntry("MIDRetail", "Waiting 20 seconds");
                        if (ControlServerSession != null
                            && ControlServerSession.isSessionRunningRemote())
                        {
                            System.Threading.Thread.Sleep(20000);
                        }
                    }

                }
                //END TT#1644 - MD - DOConnell - API Process Control

				// Get highest message level
				eMIDMessageLevel highestMessage = eMIDMessageLevel.None;

				try
				{
					highestMessage = GetHighestAuditMessageLevel();
				}
				catch
				{
					highestMessage = eMIDMessageLevel.Severe;
				}

				// if services are running local, close the global audit
				//Begin TT#708 - JScott - Services need a Retry availalbe.
				//if (_controlServerSession != null)
				//{
				//    _controlServerSession.CloseSession();
				//    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//    //if (!RemotingServices.IsTransparentProxy(_controlServerSession))
				//    if (_controlGlobalLoaded && !RemotingServices.IsTransparentProxy(_controlServerSession))
				//    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//    {
				//        _controlServerSession.CleanUpGlobal();
				//    }
				//}

				//if (_clientServerSession != null)
				//{
				//    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//    //if (!RemotingServices.IsTransparentProxy(_clientServerSession))
				//    if (_clientGlobalLoaded && !RemotingServices.IsTransparentProxy(_clientServerSession))
				//    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//    {
				//        _clientServerSession.CleanUpGlobal();
				//    }
				//}

				//if (_hierarchyServerSession != null)
				//{
				//    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//    //if (!RemotingServices.IsTransparentProxy(_hierarchyServerSession))
				//    if (_hierarchyGlobalLoaded && !RemotingServices.IsTransparentProxy(_hierarchyServerSession))
				//    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//    {
				//        _hierarchyServerSession.CleanUpGlobal();
				//    }
				//}

				//if (_applicationServerSession != null)
				//{
				//    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//    //if (!RemotingServices.IsTransparentProxy(_applicationServerSession))
				//    if (_applicationGlobalLoaded && !RemotingServices.IsTransparentProxy(_applicationServerSession))
				//    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//    {
				//        _applicationServerSession.CleanUpGlobal();
				//    }
				//}

				//if (_storeServerSession != null)
				//{
				//    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//    //if (!RemotingServices.IsTransparentProxy(_storeServerSession))
				//    if (_storeGlobalLoaded && !RemotingServices.IsTransparentProxy(_storeServerSession))
				//    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//    {
				//        _storeServerSession.CleanUpGlobal();
				//    }
				//}

				////Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				////if (_schedulerServerSession != null)
				////{
				////    if (!RemotingServices.IsTransparentProxy(_schedulerServerSession))
				////    {
				////        _schedulerServerSession.CleanUpGlobal();
				////    }
				////}

				////End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//if (_headerServerSession != null)
				//{
				//    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//    //if (!RemotingServices.IsTransparentProxy(_headerServerSession))
				//    if (_headerGlobalLoaded && !RemotingServices.IsTransparentProxy(_headerServerSession))
				//    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//    {
				//        _headerServerSession.CleanUpGlobal();
				//    }
				//}
				if (_controlServerSessionRemote != null)
				{
					_controlServerSessionRemote.CloseSession();
					if (_controlGlobalLoaded && !RemotingServices.IsTransparentProxy(_controlServerSessionRemote))
					{
						_controlServerSessionRemote.CleanUpGlobal();
					}
				}

				if (_clientServerSessionRemote != null)
				{
                    // Begin TT#1243 - JSmith - Audit Performance
                    _clientServerSessionRemote.CloseSession();
                    // End TT#1243
					if (_clientGlobalLoaded && !RemotingServices.IsTransparentProxy(_clientServerSessionRemote))
					{
						_clientServerSessionRemote.CleanUpGlobal();
					}
				}

				if (_hierarchyServerSessionRemote != null)
				{
                    // Begin TT#1243 - JSmith - Audit Performance
                    _hierarchyServerSessionRemote.CloseSession();
                    // End TT#1243
					if (_hierarchyGlobalLoaded && !RemotingServices.IsTransparentProxy(_hierarchyServerSessionRemote))
					{
						_hierarchyServerSessionRemote.CleanUpGlobal();
					}
				}

				if (_applicationServerSessionRemote != null)
				{
                    // Begin TT#1243 - JSmith - Audit Performance
                    _applicationServerSessionRemote.CloseSession();
                    // End TT#1243
					if (_applicationGlobalLoaded && !RemotingServices.IsTransparentProxy(_applicationServerSessionRemote))
					{
						_applicationServerSessionRemote.CleanUpGlobal();
					}
				}

				if (_storeServerSessionRemote != null)
				{
                    // Begin TT#1243 - JSmith - Audit Performance
                    _storeServerSessionRemote.CloseSession();
                    // End TT#1243
					if (_storeGlobalLoaded && !RemotingServices.IsTransparentProxy(_storeServerSessionRemote))
					{
						_storeServerSessionRemote.CleanUpGlobal();
					}
				}

				if (_headerServerSessionRemote != null)
				{
                    // Begin TT#1243 - JSmith - Audit Performance
                    _headerServerSessionRemote.CloseSession();
                    // End TT#1243
					if (_headerGlobalLoaded && !RemotingServices.IsTransparentProxy(_headerServerSessionRemote))
					{
						_headerServerSessionRemote.CleanUpGlobal();
					}
				}
				//End TT#708 - JScott - Services need a Retry availalbe.

				return highestMessage;
			}
			catch
			{
				throw;
			}
		}

		public eMIDMessageLevel GetHighestAuditMessageLevel()
		{
			try
			{
				eMIDMessageLevel highestMessageLevel = eMIDMessageLevel.None;

				if (_controlServerSession != null)
				{
					if (_controlServerSession.GetHighestMessageLevel() > highestMessageLevel)
					{
						highestMessageLevel = _controlServerSession.GetHighestMessageLevel();
					}
				}

				if (_clientServerSession != null)
				{
					if (_clientServerSession.GetHighestMessageLevel() > highestMessageLevel)
					{
						highestMessageLevel = _clientServerSession.GetHighestMessageLevel();
					}
				}

				if (_hierarchyServerSession != null)
				{
					if (_hierarchyServerSession.GetHighestMessageLevel() > highestMessageLevel)
					{
						highestMessageLevel = _hierarchyServerSession.GetHighestMessageLevel();
					}
				}

				if (_applicationServerSession != null)
				{
					if (_applicationServerSession.GetHighestMessageLevel() > highestMessageLevel)
					{
						highestMessageLevel = _applicationServerSession.GetHighestMessageLevel();
					}
				}

				if (_storeServerSession != null)
				{
					if (_storeServerSession.GetHighestMessageLevel() > highestMessageLevel)
					{
						highestMessageLevel = _storeServerSession.GetHighestMessageLevel();
					}
				}

				if (_schedulerServerSession != null)
				{
					if (_schedulerServerSession.GetHighestMessageLevel() > highestMessageLevel)
					{
						highestMessageLevel = _schedulerServerSession.GetHighestMessageLevel();
					}
				}

				if (_headerServerSession != null)
				{
					if (_headerServerSession.GetHighestMessageLevel() > highestMessageLevel)
					{
						highestMessageLevel = _headerServerSession.GetHighestMessageLevel();
					}
				}

				return highestMessageLevel;
			}
			catch
			{
				throw;
			}
		}
        
		/// <summary>
		/// Converts a string of "True", "T", "Yes", or "Y" to a true value.  All others receive false.
		/// </summary>
		/// <param name="aLocalAppSetting">
		/// String to convert
		/// </param>
		/// <returns>
		/// Boolean of converted string value
		/// </returns>

		private bool ConvertLocalAppSetting(string aLocalAppSetting)
		{
			try
			{
				if (aLocalAppSetting != null)
				{
                    //BEGIN TT#0906-VStuart-Config Values of Yes and T not read correctly-MID
					if (aLocalAppSetting.ToLower(CultureInfo.CurrentUICulture) == "true" || aLocalAppSetting.ToLower(CultureInfo.CurrentUICulture) == "yes" ||
						aLocalAppSetting.ToLower(CultureInfo.CurrentUICulture) == "t" || aLocalAppSetting.ToLower(CultureInfo.CurrentUICulture) == "y")
                    //END TT#0906-VStuart-Config Values of Yes and T not read correctly-MID
                    {
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves the IPAddress of the calling client.
		/// </summary>
		/// <remarks>
		/// For a VPN connection, the IP Address of the client is not available (nor obvious) at the client for callback functions.
		/// In order for the client to determine what the local VPN address is, the client calls this routine to return the address.
		//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
		///// Logic is embedded in the ServerPing class that pulls the address from a custom Sink.
		/// Logic is embedded in the ServerObjectFactory class that pulls the address from a custom Sink.
		//End Fix - JScott - Correct Group Dynamite Conneciton problem
		/// </remarks>
		/// <returns>
		/// The IPAddress of the calling client.
		/// </returns>

		private IPAddress GetClientIPAddress()
		{
			string appSetControlServer;
			ServerList controlServerList;
			//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
			//ServerPing serverPing;
			ServerObjectFactory soFactory;
			//End Fix - JScott - Correct Group Dynamite Conneciton problem

			try
			{
				if (!_forceLocal)
				{
					appSetControlServer = MIDConfigurationManager.AppSettings["ControlServer"];

				if (appSetControlServer != null)
				{
					controlServerList = new ServerList(appSetControlServer);
					_controlServer = controlServerList.GetServer(eServerType.Control);

					if (_controlServer.Length > 0)
					{
							//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
							//serverPing = (ServerPing)RemotingServices.Connect(typeof(ServerPing), "tcp://" + _controlServer + "/MRSServerPing.rem");

							//return serverPing.GetClientIPAddress();
							soFactory = (ServerObjectFactory)RemotingServices.Connect(typeof(ServerObjectFactory), "tcp://" + _controlServer + "/MRSServerObjectFactory.rem");

							return soFactory.GetClientIPAddress();
							//End Fix - JScott - Correct Group Dynamite Conneciton problem
						}
					else
					{
						return null;
					}
				}
				else
				{
					return null;
				}
			}
				else
				{
					return null;
				}
			}
			catch
			{
				throw;
			}
		}
	}

	// BEGIN TT#217-MD - stodd - unable to run workflow methods against assortment
	// BEGIN TT#458-MD - stodd - not serializable error
	[Serializable]
	// END TT#458-MD - stodd - not serializable error
	public class ProcessMethodOnAssortmentEvent
	{
		// event to process methods on assortment headers & placeholders
		public delegate void ProcessMethodOnAssortmentEventHandler(object source, ProcessMethodOnAssortmentEventArgs e);
		public event ProcessMethodOnAssortmentEventHandler OnProcessMethodOnAssortmentEventHandler;

		public void ProcessMethod(object source, int asrtRid, eMethodType methodType, int methodRid)
		{
			ProcessMethodOnAssortmentEventArgs ea;
			// fire the event if handler is defined
			if (OnProcessMethodOnAssortmentEventHandler != null)
			{
				ea = new ProcessMethodOnAssortmentEventArgs(asrtRid, methodType, methodRid);
				OnProcessMethodOnAssortmentEventHandler(source, ea);
			}
			return;
		}
	}

	// BEGIN TT#458-MD - stodd - not serializable error
	[Serializable]
	// END TT#458-MD - stodd - not serializable error
	public class ProcessMethodOnAssortmentEventArgs : EventArgs
	{

		int _asrtRid;
		eMethodType _methodType;
		int _methodRid;

		public ProcessMethodOnAssortmentEventArgs(int asrtRid, eMethodType methodType, int methodRid)
		{
			_asrtRid = asrtRid;
			_methodType = methodType;
			_methodRid = methodRid;
		}

		public int AsrtRid
		{
			get { return _asrtRid; }
			set { _asrtRid = value; }
		}

		public eMethodType MethodType
		{
			get { return _methodType; }
			set { _methodType = value; }
		}

		public int MethodRid
		{
			get { return _methodRid; }
			set { _methodRid = value; }
		}

	}
	// END TT#217-MD - stodd - unable to run workflow methods against assortment


	// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
	//=====================================================================
	// Get the selected headers in an assortment window
	//=====================================================================
	[Serializable]
	public class AssortmentSelectedHeaderEvent
	{
		
		public delegate void AssortmentSelectedHeaderEventHandler(object source, AssortmentSelectedHeaderEventArgs e);
		public event AssortmentSelectedHeaderEventHandler OnAssortmentSelectedHeaderEventHandler;

		// Event to get the selected headers in an assortment window
		// BEGIN TT#696-MD - Stodd - add "active process"
		public SelectedHeaderList GetSelectedHeaders(object source, int asrtRid, eMethodType methodType)
		{
			SelectedHeaderList selectedHdrList = new SelectedHeaderList(eProfileType.SelectedHeader);
			AssortmentSelectedHeaderEventArgs ea = new AssortmentSelectedHeaderEventArgs(asrtRid, methodType, ref selectedHdrList);
		// END TT#696-MD - Stodd - add "active process"
			// fire the event if handler is defined
			if (OnAssortmentSelectedHeaderEventHandler != null)
			{
				OnAssortmentSelectedHeaderEventHandler(source, ea);
			}
			return ea.SelectedHdrList;
		}
	}


	[Serializable]
	public class AssortmentSelectedHeaderEventArgs : EventArgs
	{
		int _asrtRid;	//  TT#696-MD - Stodd - add "active process"
		eMethodType _methodType;
		SelectedHeaderList _selectedHdrList;

		// BEGIN TT#696-MD - Stodd - add "active process"
		public AssortmentSelectedHeaderEventArgs(int asrtRid, eMethodType methodType, ref SelectedHeaderList selectedHdrList)
		{
			_asrtRid = asrtRid;
		// END TT#696-MD - Stodd - add "active process"
			_methodType = methodType;
			_selectedHdrList = selectedHdrList;
		}

		// BEGIN TT#696-MD - Stodd - add "active process"
		public int AsrtRid
		{
			get { return _asrtRid; }
			set { _asrtRid = value; }
		}
		// END TT#696-MD - Stodd - add "active process"

		public eMethodType MethodType
		{
			get { return _methodType; }
			set { _methodType = value; }
		}

		public SelectedHeaderList SelectedHdrList
		{
			get { return _selectedHdrList; }
			set { _selectedHdrList = value; }
		}

	}

	// BEGIN TT#696-MD - Stodd - add "active process"
	//=====================================================================
	// Get the selected headers in an assortment window
	//=====================================================================
	[Serializable]
	public class AssortmentTransactionEvent
	{

		public delegate void AssortmentTransactionEventHandler(object source, AssortmentTransactionEventArgs e);
		public event AssortmentTransactionEventHandler OnAssortmentTransactionEventHandler;

		// Event to get the selected headers in an assortment window
		public ApplicationSessionTransaction GetAssortmentTransaction(object source, int asrtRid)	// TT#698-MD - Stodd - add ability for workflows to be run against assortments.
		{
			ApplicationSessionTransaction aTrans = null;
			AssortmentTransactionEventArgs ea = new AssortmentTransactionEventArgs(asrtRid, ref aTrans);
			// fire the event if handler is defined
			if (OnAssortmentTransactionEventHandler != null)
			{
				OnAssortmentTransactionEventHandler(source, ea);
			}
			return ea.Transaction;
		}
	}


	[Serializable]
	public class AssortmentTransactionEventArgs : EventArgs
	{
		int _asrtRid;
		ApplicationSessionTransaction _trans;

		public AssortmentTransactionEventArgs(int asrtRid, ref ApplicationSessionTransaction trans)
		{
			_asrtRid = asrtRid;
			_trans = trans;
		}

		public int AsrtRid
		{
			get { return _asrtRid; }
			set { _asrtRid = value; }
		}

		public ApplicationSessionTransaction Transaction
		{
			get { return _trans; }
			set { _trans = value; }
		}
	}
	// END TT#696-MD - Stodd - add "active process"

	// END TT#371-MD - stodd -  Velocity Interactive on Assortment
}
