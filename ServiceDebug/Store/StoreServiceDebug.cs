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
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.ServiceDebug.Store
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class StoreServiceDebug
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

			appThread = new Thread(new ThreadStart(StoreThread));
			appThread.Name = "appThread";
			appThread.Start();
			System.Console.ReadLine();
			appThread.Abort();
            // wait for thread to exit
            appThread.Join();
		}

		static public void StoreThread()
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

			// Register StoreServiceSession

			try
			{
				RemotingConfiguration.RegisterActivatedServiceType(typeof(StoreServerSession));
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Error registering StoreServiceSession - " + e.Message);
				return;
			}

			// Create static object

			MIDConnectionString.ConnectionString = MIDConfigurationManager.AppSettings["ConnectionString"];
            // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
            //StoreServerGlobal.Load();
            StoreServerGlobal.Load(false);
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
			//    _pingObjRef = RemotingServices.Marshal(new ServerPing(eServerType.Store), "MRSServerPing.rem");
			//}
			//catch (Exception e)
			//{
			//    System.Console.WriteLine("Error creating or marshalling MRSServerPing - " + e.Message);
			//    return;
			//}
			// Create ServerObjectFactory object

			try
			{
				_sofObjRef = RemotingServices.Marshal(new ServerObjectFactory(eServerType.Store), "MRSServerObjectFactory.rem");
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Error creating or marshalling MRSServerPing - " + e.Message);
				return;
			}
			//End Fix - JScott - Correct Group Dynamite Conneciton problem

			// Service Started...

			System.Console.WriteLine("MIDStoreService started successfully and listening on port #" + _port);
		}
	}
}
