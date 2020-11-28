using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.ServiceProcess;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
using System.Runtime.Remoting.Services;
//End Fix - JScott - Correct Group Dynamite Conneciton problem
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.Service.Header
{
	public class HeaderService : System.ServiceProcess.ServiceBase
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private string _port;
		private System.Runtime.Remoting.Channels.IChannel _channel;
		//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
		//private System.Runtime.Remoting.ObjRef _pingObjRef;
		private System.Runtime.Remoting.ObjRef _sofObjRef;
		//End Fix - JScott - Correct Group Dynamite Conneciton problem
		private BinaryServerFormatterSinkProvider _provider; 
		private Hashtable _myPort;
// (CSMITH) - BEG MID Track #3369: DB Connection String hardcoded in config files
		private string ConnectionString = null;
// (CSMITH) - END MID Track #3369

		public HeaderService()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
		}

		// The main entry point for the process
		static void Main()
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptions);

			System.ServiceProcess.ServiceBase[] ServicesToRun;
	
			// More than one user Service may run within the same process. To add
			// another service to this process, change the following line to
			// create a second service object. For example,
			//
			//   ServicesToRun = New System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
			//
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new HeaderService() };

			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// HeaderService
			// 
			this.AutoLog = false;
			this.ServiceName = "MIDHeaderService";

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			// Register to Header EventLog

			if (!EventLog.SourceExists("MIDHeaderService"))
			{
				EventLog.CreateEventSource("MIDHeaderService", null);
			}

            // check environment
            string message = string.Empty;
            if (!MIDEnvironment.ValidEnvironment(out message))
            {
                EventLog.WriteEntry("MIDApplicationService", message, EventLogEntryType.Error);
                return;
            }

			// Configure Remoting

			try
			{
                //RemotingConfiguration.Configure(System.Reflection.Assembly.GetExecutingAssembly().Location + ".config");  obsolete
                RemotingConfiguration.Configure(System.Reflection.Assembly.GetExecutingAssembly().Location + ".config", true);
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDHeaderService", "Error configuring Remoting - " + e.Message, EventLogEntryType.Error);
				return;
			}

			//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
			//// Register HeaderServiceSession

			//try
			//{
			//    RemotingConfiguration.RegisterActivatedServiceType(typeof(HeaderServerSession));
			//}
			//catch (Exception e)
			//{
			//    EventLog.WriteEntry("MIDHeaderService", "Error registering HeaderServiceSession - " + e.Message, EventLogEntryType.Error);
			//    return;
			//}

			//End Fix - JScott - Correct Group Dynamite Conneciton problem
			// Create static object

// (CSMITH) - BEG MID Track #3369: DB Connection String hardcoded in config files
			ConnectionString = MIDConfigurationManager.AppSettings["ConnectionString"];

			if (ConnectionString == "" || ConnectionString == null)
			{
				EventLog.WriteEntry("MIDHeaderService", "Database Connection String MUST be specified", EventLogEntryType.Error);

				return;
			}

//			MIDConnectionString MIDConnectionString = new MIDConnectionString();
			MIDConnectionString.ConnectionString = ConnectionString;
// (CSMITH) - END MID Track #3369

			// Begin TT#189 - RMatelic - Remove excessive entries from the Audit
            //HeaderServerGlobal.Load();
            HeaderServerGlobal.Load(false);
            // End TT#189  

			// Open port to receive requests

			try
			{
				_port = MIDConfigurationManager.AppSettings["MyPort"];

				if (_port == null)
				{
					throw(new Exception("MyPort not assigned"));
				}

				try
				{
					//					_channel = new TcpChannel(Convert.ToInt16(_port));
					_provider = new BinaryServerFormatterSinkProvider();
					_provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
					_myPort = new Hashtable();
					_myPort.Add("port", Convert.ToInt16(_port));
					_channel = new TcpChannel(_myPort, null, _provider);
                    //ChannelServices.RegisterChannel(_channel);  obsolete
                    ChannelServices.RegisterChannel(_channel, true);
					//Begin Fix - JScott - Correct Group Dynamite Conneciton problem

					TrackingServices.RegisterTrackingHandler(new SessionTrackingHandler());
					//End Fix - JScott - Correct Group Dynamite Conneciton problem
				}
				catch (Exception e)
				{
					EventLog.WriteEntry("MIDHeaderService", "Error opening port #" + _port + " - " + e.Message, EventLogEntryType.Error);
					return;
				}
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDHeaderService", "Error getting port from MyPort on config file - " + e.Message, EventLogEntryType.Error);
				return;
			}

			//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
			//// Create ServerPing object

			//try
			//{
			//    _pingObjRef = RemotingServices.Marshal(new ServerPing(eServerType.Header), "MRSServerPing.rem");
			//}
			//catch (Exception e)
			//{
			//    EventLog.WriteEntry("MIDHeaderService", "Error creating or marshalling MRSServerPing - " + e.Message, EventLogEntryType.Error);
			//    return;
			//}
			// Create ServerObjectFactory object

			try
			{
				_sofObjRef = RemotingServices.Marshal(new ServerObjectFactory(eServerType.Header), "MRSServerObjectFactory.rem");
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDHeaderService", "Error creating or marshalling MRSServerObjectFactory - " + e.Message, EventLogEntryType.Error);
				return;
			}
			//End Fix - JScott - Correct Group Dynamite Conneciton problem

			// Service Started...

			EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService started successfully and listening on port #" + _port, EventLogEntryType.Information);
		}
 
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
			//// Unmarshal ServerPing object

			//try
			//{
			//    HeaderServerGlobal.CleanUp();
			//    RemotingServices.Unmarshal(_pingObjRef);
			//}
			//catch (Exception e)
			//{
			//    EventLog.WriteEntry("MIDHeaderService", "Error unmarshalling MRSServerPing - " + e.Message, EventLogEntryType.Error);
			//    return;
			//}
			// Clean up global area

			try
			{
				HeaderServerGlobal.CleanUp();
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDHeaderService", "Error cleaning up global area - " + e.Message, EventLogEntryType.Error);
				return;
			}

			// Unmarshal ServerObjectFactory object

			try
			{
				RemotingServices.Unmarshal(_sofObjRef);
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDHeaderService", "Error unmarshalling MRSServerObjectFactory - " + e.Message, EventLogEntryType.Error);
				return;
			}
			//End Fix - JScott - Correct Group Dynamite Conneciton problem

			// Close port to receive requests

			try
			{
				ChannelServices.UnregisterChannel(_channel);
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDHeaderService", "Error closing port #" + _port + " - " + e.Message, EventLogEntryType.Error);
				return;
			}

			// Service Stopped...

			EventLog.WriteEntry("MIDHeaderService", "MIDHeaderService stopped successfully.", EventLogEntryType.Information);
		}

		private static void UnhandledExceptions(object sender, UnhandledExceptionEventArgs args)
		{
			string message;
			Exception e = (Exception) args.ExceptionObject;
			message = e.ToString();
			while (e.InnerException != null) 
			{
				message += Environment.NewLine;
				e = e.InnerException;
				message = e.ToString();
			}
			EventLog.WriteEntry("MIDHierarchyService", "Error encountered -- " + message, EventLogEntryType.Error);
		}
	}
}
