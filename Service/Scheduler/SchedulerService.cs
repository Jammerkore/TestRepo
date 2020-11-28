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

namespace MIDRetail.Service.Scheduler
{
	public class SchedulerService : System.ServiceProcess.ServiceBase
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

		public SchedulerService()
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
			//   ServicesToRun = new System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
			//
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new SchedulerService() };

			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// SchedulerService
			// 
			this.ServiceName = "MIDSchedulerService";

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
			// Register to Schedule EventLog

			if (!EventLog.SourceExists("MIDSchedulerService"))
			{
				EventLog.CreateEventSource("MIDSchedulerService", null);
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
                //RemotingConfiguration.Configure(System.Reflection.Assembly.GetExecutingAssembly().Location + ".config");  obsolsete
                RemotingConfiguration.Configure(System.Reflection.Assembly.GetExecutingAssembly().Location + ".config", true);
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDSchedulerService", "Error configuring Remoting - " + e.Message, EventLogEntryType.Error);
				return;
			}

			//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
			//// Register ScheduleServiceSession

			//try
			//{
			//    RemotingConfiguration.RegisterActivatedServiceType(typeof(SchedulerServerSession));
			//}
			//catch (Exception e)
			//{
			//    EventLog.WriteEntry("MIDSchedulerService", "Error registering ScheduleServiceSession - " + e.Message, EventLogEntryType.Error);
			//    return;
			//}

			//End Fix - JScott - Correct Group Dynamite Conneciton problem
			// Create static object

// (CSMITH) - BEG MID Track #3369: DB Connection String hardcoded in config files
			ConnectionString = MIDConfigurationManager.AppSettings["ConnectionString"];

			if (ConnectionString == "" || ConnectionString == null)
			{
				EventLog.WriteEntry("MIDSchedulerService", "Database Connection String MUST be specified", EventLogEntryType.Error);

				return;
			}

//			MIDConnectionString MIDConnectionString = new MIDConnectionString();
			MIDConnectionString.ConnectionString = ConnectionString;
// (CSMITH) - END MID Track #3369

            // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
            //SchedulerServerGlobal.Load();
            SchedulerServerGlobal.Load(false);
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
					EventLog.WriteEntry("MIDSchedulerService", "Error opening port #" + _port + " - " + e.Message, EventLogEntryType.Error);
					return;
				}
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDSchedulerService", "Error getting port from MyPort on config file - " + e.Message, EventLogEntryType.Error);
				return;
			}

			//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
			//// Create ServerPing object

			//try
			//{
			//    _pingObjRef = RemotingServices.Marshal(new ServerPing(eServerType.Scheduler), "MRSServerPing.rem");
			//}
			//catch (Exception e)
			//{
			//    EventLog.WriteEntry("MIDSchedulerService", "Error creating or marshalling MRSServerPing - " + e.Message, EventLogEntryType.Error);
			//    return;
			//}
			// Create ServerObjectFactory object

			try
			{
				_sofObjRef = RemotingServices.Marshal(new ServerObjectFactory(eServerType.Scheduler), "MRSServerObjectFactory.rem");
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDSchedulerService", "Error creating or marshalling MRSServerObjectFactory - " + e.Message, EventLogEntryType.Error);
				return;
			}
			//End Fix - JScott - Correct Group Dynamite Conneciton problem

			// Service Started...

			EventLog.WriteEntry("MIDSchedulerService", "MIDSchedulerService started successfully and listening on port #" + _port, EventLogEntryType.Information);
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
			//    SchedulerServerGlobal.EndProcessScheduleThread();
			//    SchedulerServerGlobal.CleanUp();
			//    RemotingServices.Unmarshal(_pingObjRef);
			//}
			//catch (Exception e)
			//{
			//    EventLog.WriteEntry("MIDSchedulerService", "Error unmarshalling MRSServerPing - " + e.Message, EventLogEntryType.Error);
			//    return;
			//}
			// Stop Process Schedule thread

			try
			{
				SchedulerServerGlobal.EndProcessScheduleThread();
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDSchedulerService", "Error stopping Process Schedule thread - " + e.Message, EventLogEntryType.Error);
				return;
			}

            // Begin TT#2307 - JSmith - Incorrect Stock Values
            SchedulerServerGlobal.MessageProcessor.StopMessageListener();
            // End TT#2307

			// Clean up global area

			try
			{
				SchedulerServerGlobal.CleanUp();
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDSchedulerService", "Error cleaning up global area - " + e.Message, EventLogEntryType.Error);
				return;
			}

			// Unmarshal ServerObjectFactory object

			try
			{
				RemotingServices.Unmarshal(_sofObjRef);
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDSchedulerService", "Error unmarshalling MRSServerObjectFactory - " + e.Message, EventLogEntryType.Error);
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
				EventLog.WriteEntry("MIDSchedulerService", "Error closing port #" + _port + " - " + e.Message, EventLogEntryType.Error);
				return;
			}

			// Service Stopped...

			EventLog.WriteEntry("MIDSchedulerService", "MIDSchedulerService stopped successfully.", EventLogEntryType.Information);
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
