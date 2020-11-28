using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

using MIDRetail.DataCommon;

namespace MIDRetail.Service.GarbageCollection
{
	public class MIDGarbageCollectionService : System.ServiceProcess.ServiceBase
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Thread gcCollectThread;
		private int waitTime;
		private bool continueCollect;

		public MIDGarbageCollectionService()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
		}

		// The main entry point for the process
		static void Main()
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;
	
			// More than one user Service may run within the same process. To add
			// another service to this process, change the following line to
			// create a second service object. For example,
			//
			//   ServicesToRun = new System.ServiceProcess.ServiceBase[] {new MIDGarbageCollectionService(), new MySecondUserService()};
			//
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new MIDGarbageCollectionService() };

			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			this.ServiceName = "MIDGarbageCollectionService";
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
			string waitStr;

			if (!EventLog.SourceExists("MIDGarbageCollectionService"))
			{
				EventLog.CreateEventSource("MIDGarbageCollectionService", null);
			}

			try
			{
				waitStr = MIDConfigurationManager.AppSettings["WaitSeconds"];

				if (waitStr != null)
				{
					waitTime = Convert.ToInt32(waitStr);
				}
				else
				{
					waitTime = 120;
				}

				continueCollect = true;
				gcCollectThread = new Thread(new ThreadStart(GarbageCollect));
				gcCollectThread.Start();
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDGarbageCollectionService", "Error encountered -- " + e.Message, EventLogEntryType.Error);
				return;
			}
		}
 
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			continueCollect = false;
		}

		public void GarbageCollect()
		{
			try
			{
				while (continueCollect)
				{
//					EventLog.WriteEntry("MIDGarbageCollectionService", "Calling Garbage Collection at " + System.DateTime.Now, EventLogEntryType.Information);
					System.GC.Collect();
					System.Threading.Thread.Sleep(waitTime * 1000);
				}
			}
			catch (Exception e)
			{
				EventLog.WriteEntry("MIDGarbageCollectionService", "Error encountered -- " + e.Message, EventLogEntryType.Error);
				return;
			}
		}
	}
}
