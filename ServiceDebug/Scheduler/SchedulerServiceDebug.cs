using System;
using System.Collections;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
using System.Runtime.Remoting.Services;
//End Fix - JScott - Correct Group Dynamite Conneciton problem
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.ServiceDebug.Scheduler
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		static private string _port;
		static private System.Runtime.Remoting.Channels.IChannel _channel;
		//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
		//static private System.Runtime.Remoting.ObjRef _pingObjRef;
		static private System.Runtime.Remoting.ObjRef _sofObjRef;
		//End Fix - JScott - Correct Group Dynamite Conneciton problem
		static private BinaryServerFormatterSinkProvider _provider; 
		static private Hashtable _myPort;
// (CSMITH) - BEG MID Track #3369: DB Connection String hardcoded in config files
		static private string ConnectionString = null;
// (CSMITH) - END MID Track #3369

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			SchedulerThread();

			System.Console.ReadLine();

			SchedulerServerGlobal.EndProcessScheduleThread();
			SchedulerServerGlobal.CleanUp();

			//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
			//RemotingServices.Unmarshal(_pingObjRef);
			RemotingServices.Unmarshal(_sofObjRef);
			//End Fix - JScott - Correct Group Dynamite Conneciton problem
			ChannelServices.UnregisterChannel(_channel);
		}

		static public void SchedulerThread()
		{
			// Configure Remoting

			try
			{
				RemotingConfiguration.Configure(System.Reflection.Assembly.GetExecutingAssembly().Location + ".config", true);
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Error configuring Remoting - " + e.Message);
				return;
			}

            //Begin Fix - JScott - Correct Group Dynamite Conneciton problem
            //// Register SchedulerServiceSession

            //try
            //{
            //    RemotingConfiguration.RegisterActivatedServiceType(typeof(SchedulerServerSession));
            //}
            //catch (Exception e)
            //{
            //    System.Console.WriteLine("Error registering SchedulerServerSession - " + e.Message);
            //    return;
            //}
            //End Fix - JScott - Correct Group Dynamite Conneciton problem

// (CSMITH) - BEG MID Track #3369: DB Connection String hardcoded in config files
			ConnectionString = MIDConfigurationManager.AppSettings["ConnectionString"];

			if (ConnectionString == "" || ConnectionString == null)
			{
				EventLog.WriteEntry("MIDSchedulerService", "Database Connection String MUST be specified", EventLogEntryType.Error);

				return;
			}

			MIDConnectionString.ConnectionString = ConnectionString;

// (CSMITH) - END MID Track #3369
			// Create static object

            // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
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
					System.Console.WriteLine("Error opening port #" + _port + " - " + e.Message);
					return;
				}
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Error getting port from MyPort on config file - " + e.Message);
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
			//    System.Console.WriteLine("Error creating or marshalling MRSServerPing - " + e.Message);
			//    return;
			//}
			// Create ServerObjectFactory object

			try
			{
				_sofObjRef = RemotingServices.Marshal(new ServerObjectFactory(eServerType.Scheduler), "MRSServerObjectFactory.rem");
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Error creating or marshalling MRSServerPing - " + e.Message);
				return;
			}
			//End Fix - JScott - Correct Group Dynamite Conneciton problem

			// Service Started...

			System.Console.WriteLine("MIDSchedulerService started successfully and listening on port #" + _port);
		}
	}
}
