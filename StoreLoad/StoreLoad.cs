using System;
using System.IO;
using System.Xml.Serialization;
using System.Configuration;
using System.Globalization;
using System.Diagnostics;
//using System.Runtime.Remoting;
//using System.Runtime.Remoting.Lifetime;
//using System.Runtime.Remoting.Channels;
//using System.Runtime.Remoting.Channels.Tcp;
//
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.StoreLoad
{
	/// <summary>
	/// The StoreLoad class in a console application that calls StoreLoadWorker class
	/// that performs the actual functionality
	/// </summary>
	class StoreLoad
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args) 
		{
			StoreLoadWorker sw = new StoreLoadWorker();
			return sw.LoadStore(args);
		}
	}
	public class StoreLoadWorker : LoadBase
    {
        // Begin TT#796 - JSmith - Security violation running application on Windows Server 2008
        #region Constructors
        //=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public StoreLoadWorker()
			: base("MIDStoreLoad")
		{
			
		}
		#endregion Constructors
        // End TT#796

		public int LoadStore(string[] args)
		{
//			int returnCode = -1;	// Default to an error and success will be determined...
			bool errorFound = true; // ...by the base class batch load method
			string eventLogID = "MIDStoreLoad";
			eMIDMessageLevel highestMessage;
			
			try
			{	
				// This could be implemented in the base class but it seems like this should be here instead.
				try
				{
					SAB.CreateSessions((int)eServerType.Client|(int)eServerType.Hierarchy|(int)eServerType.Store);
				}
				catch (Exception ex)
				{
					errorFound = true;
					Exception innerE = ex;
					while (innerE.InnerException != null) 
					{
						innerE = innerE.InnerException;
					}
					EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);
					//					return Convert.ToInt32(eReturnCode.fatal,CultureInfo.CurrentUICulture);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}
				eSecurityAuthenticate authentication = SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"], MIDConfigurationManager.AppSettings["Password"], eProcesses.storeLoad);

                //BEGIN TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID
                //BEGIN TT#1644-VSuart-Process Control-MID
                if (authentication == eSecurityAuthenticate.Unavailable)
                {
                    //return Convert.ToInt32(eMIDMessageLevel.ProcessUnavailable);
                    return Convert.ToInt32(eMIDMessageLevel.Severe);
                }
                //END TT#1644-VSuart-Process Control-MID
                //END TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID

				if (authentication != eSecurityAuthenticate.UserAuthenticated)
				{
					EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
					System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
					//					return Convert.ToInt32(eErrorLevel.fatal,CultureInfo.CurrentUICulture);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//				SAB.ClientServerSession.Initialize();
				ProcessArgs(args);

				if (processId != Include.NoRID)
				{
					SAB.ClientServerSession.Initialize(processId);
				}
				else
				{
					SAB.ClientServerSession.Initialize();
				}

//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                //SAB.HierarchyServerSession.Initialize();  // TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
				SAB.StoreServerSession.Initialize();
                // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                // StoreServerSession must be initialized before HierarchyServerSession 
                SAB.HierarchyServerSession.Initialize();
                // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.


				// The base class call is this simple
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//				errorFound = BatchLoad(args, SAB.StoreServerSession); // We invert the return because BatchLoad returns true on success
				errorFound = BatchLoad(SAB.StoreServerSession); // We invert the return because BatchLoad returns true on success
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
					
			}
			catch (XMLStoreLoadProcessException XMLslpe)
			{
				errorFound = true;
				SAB.ClientServerSession.Audit.Log_Exception(XMLslpe, sourceModule);
			}
			catch ( Exception ex )
			{
				errorFound = true;
				SAB.ClientServerSession.Audit.Log_Exception(ex, sourceModule);
			}
			finally
			{ 
				if (!errorFound)
				{
					if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
					{
						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
					}
				}
				else
				{
					if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
					{
						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
					}
				}

				highestMessage = SAB.CloseSessions();
			}

			return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
		}
	}
}
