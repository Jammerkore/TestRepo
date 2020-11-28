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
using System.Runtime.Remoting.Services;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.ServiceDebug.Control
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class ControlServiceDebug
	{
		static private string _port;
		static private System.Runtime.Remoting.Channels.IChannel _channel;
		//Begin Fix - JScott - Correct Group Dynamite Conneciton problem
		//static private System.Runtime.Remoting.ObjRef _pingObjRef;
		static private System.Runtime.Remoting.ObjRef _sofObjRef;
		//End Fix - JScott - Correct Group Dynamite Conneciton problem
		static private BinaryServerFormatterSinkProvider _provider; 
		static private Hashtable _myPort;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			System.Threading.Thread appThread;

			appThread = new Thread(new ThreadStart(ControlThread));
			appThread.Name = "appThread";
			appThread.Start();
			System.Console.ReadLine();
			appThread.Abort();
            // wait for thread to exit
            appThread.Join();
		}

		static public void ControlThread()
		{
			// Configure Remoting

			try
			{
				RemotingConfiguration.Configure(System.Reflection.Assembly.GetExecutingAssembly().Location + ".config");
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Error configuring Remoting - " + e.Message);
				return;
			}

			// Register ControlServiceSession

			try
			{
				RemotingConfiguration.RegisterActivatedServiceType(typeof(ControlServerSession));
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Error registering ControlServiceSession - " + e.Message);
				return;
			}

            MIDConnectionString.ConnectionString = MIDConfigurationManager.AppSettings["ConnectionString"];

			// Create static object

            // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
            //ControlServerGlobal.Load();
            ControlServerGlobal.Load(false);
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
                    ClientIPInjectorSinkProvider ipInjProvider = new ClientIPInjectorSinkProvider();
					_provider = new BinaryServerFormatterSinkProvider();
					_provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                    _provider.Next = ipInjProvider;
					_myPort = new Hashtable();
					_myPort.Add("port", Convert.ToInt16(_port));
					_channel = new TcpChannel(_myPort, null, _provider);
                    //ChannelServices.RegisterChannel(_channel);  obsolete
                    ChannelServices.RegisterChannel(_channel, true);

                    TrackingServices.RegisterTrackingHandler(new SessionTrackingHandler());
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
			//    _pingObjRef = RemotingServices.Marshal(new ServerPing(eServerType.Control), "MRSServerPing.rem");
			//}
			//catch (Exception e)
			//{
			//    System.Console.WriteLine("Error creating or marshalling MRSServerPing - " + e.Message);
			//    return;
			//}
			// Create ServerObjectFactory object

			try
			{
				_sofObjRef = RemotingServices.Marshal(new ServerObjectFactory(eServerType.Control), "MRSServerObjectFactory.rem");
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Error creating or marshalling MRSServerPing - " + e.Message);
				return;
			}
			//End Fix - JScott - Correct Group Dynamite Conneciton problem

			// Service Started...

			System.Console.WriteLine("MIDControlService started successfully and listening on port #" + _port);
		}
	}
}
