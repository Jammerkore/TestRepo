using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;


namespace BatchOnlyMode
{
    class Program
    {

        private static SessionAddressBlock _SAB;
        public sealed class BatchOnlyModeCommands 
        {
            public static List<BatchOnlyModeCommands> commandList = new List<BatchOnlyModeCommands>();
            public static readonly BatchOnlyModeCommands TurnOn = new BatchOnlyModeCommands("TurnOn", "Sets Batch Only Mode on.");
            public static readonly BatchOnlyModeCommands TurnOff = new BatchOnlyModeCommands("TurnOff", "Sets Batch Only Modeo off.");
            public static readonly BatchOnlyModeCommands SendMsg = new BatchOnlyModeCommands("SendMsg", "Sends a message to all client application instances.");
            public static readonly BatchOnlyModeCommands GetClientList = new BatchOnlyModeCommands("GetClientList", "Gets a list of all connected clients.");
            public static readonly BatchOnlyModeCommands ShowList = new BatchOnlyModeCommands("/?", "Lists available commands.");
           
            private BatchOnlyModeCommands(string commandName, string commandDescription)
            {
                this.commandName = commandName;
                this.commandDescription = commandDescription;
                commandList.Add(this);
            }
            public string commandName { get; private set; }
            public string commandDescription { get; private set; }
            public static implicit operator string(BatchOnlyModeCommands op) { return op.commandName; }
        }
        private static ManualResetEvent clientListDone = new ManualResetEvent(false);
        static int Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("No argument specified, use /? to see list of commands.");
                //Console.WriteLine("Please any key to continue..");
                //Console.ReadKey();
                return 1;
            }
            string cmdToSend = (string)args[0];
            if (IsCommandValid(cmdToSend) == false)
            {
                Console.WriteLine("Unknown argument specified, use /? to see list of commands.");
                //Console.WriteLine("Please any key to continue..");
                //Console.ReadKey();
                return 1;
            }
            if (cmdToSend.ToLower() == BatchOnlyModeCommands.ShowList.commandName.ToLower())
            {
                foreach (BatchOnlyModeCommands cmd in BatchOnlyModeCommands.commandList)
                {
                    Console.WriteLine(cmd.commandName + ": " + cmd.commandDescription);
                }
                return 0;
            }



            try
            {

                IMessageCallback _messageCallback = new BatchMessageCallback();
                SessionSponsor _sponsor = new SessionSponsor();
                _SAB = new SessionAddressBlock(_messageCallback, _sponsor);

                string eventLogID = "MIDBatchOnlyMode";
                if (!EventLog.SourceExists(eventLogID))
                {
                    EventLog.CreateEventSource(eventLogID, null);
                }

   

           
   
                //Attempt to connect to the control service.
                string controlServerName;
                int controlServerPort;
                double clientTimerIntervalInMilliseconds;
                double tmpInterval;
               
                int returnCode = GetSocketSettingsFromConfigFile(out controlServerName, out controlServerPort, out clientTimerIntervalInMilliseconds, out tmpInterval);
                if (returnCode != 0)
                {
                    return 1;
                }



                SocketClientManager clientSocketManager = new SocketClientManager();
                clientSocketManager.StartClient(controlServerName, controlServerPort, clientTimerIntervalInMilliseconds, PerformClientCommandFromControlService);
                if (clientSocketManager.ableToConnect == false)
                {
                    //show a message and quit
                    //MessageBox.Show("Unable to connect to the control service. Please try again later.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //EventLog.WriteEntry(eventLogID, "Unable to connect to the control service on " + controlServerName + ":" + controlServerPort.ToString(), EventLogEntryType.Error);
                    Console.WriteLine("Unable to connect to the control service on " + controlServerName + ":" + controlServerPort.ToString());
                    return 1;
                }


                if (cmdToSend.ToLower() == BatchOnlyModeCommands.SendMsg.commandName.ToLower())
                {
                    string msg = (string)args[1];
                    clientSocketManager.SendCommandToServer(SocketSharedRoutines.SocketServerCommands.IssueShowMessage, msg);
                    Console.WriteLine("Message has been sent.");
                }
                else if (cmdToSend.ToLower() == BatchOnlyModeCommands.TurnOn.commandName.ToLower())
                {
                    string msg = System.Net.Dns.GetHostName() + " at " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                    clientSocketManager.SendCommandToServer(SocketSharedRoutines.SocketServerCommands.SetBatchOnlyModeOn, msg);
                    Console.WriteLine("Batch Mode has been turned on.");
     
                }
                else if (cmdToSend.ToLower() == BatchOnlyModeCommands.TurnOff.commandName.ToLower())
                {
                    string msg = System.Net.Dns.GetHostName() + " at " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                    clientSocketManager.SendCommandToServer(SocketSharedRoutines.SocketServerCommands.SetBatchOnlyModeOff, msg);
                    Console.WriteLine("Batch Mode has been turned off.");
                }
                else if (cmdToSend.ToLower() == BatchOnlyModeCommands.GetClientList.commandName.ToLower())
                {
                    string msg = System.Net.Dns.GetHostName() + " at " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
                    clientSocketManager.SendCommandToServer(SocketSharedRoutines.SocketServerCommands.GetClientList, msg);
                    clientListDone.WaitOne(); //wait
                }
          
          

                Thread.Sleep(1000);
                clientSocketManager.StopClient();
                //Console.WriteLine("Please any key to continue..");
                //Console.ReadKey();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error encountered: " + ex.Message);
                //Console.ReadKey();
                return 1;
            }
        }

        private static bool IsCommandValid(string cmd)
        {
            bool isValid;
 
            BatchOnlyModeCommands result = BatchOnlyModeCommands.commandList.Find(
              delegate(BatchOnlyModeCommands sp)
              {
                  return sp.commandName.ToLower() == cmd.ToLower();
              }
              );
            if (result != null)
            {
                isValid = true;
            }
            else
            {
                //cmd was not found in the list
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Returns 0 if successful
        /// </summary>
        /// <param name="controlServerName"></param>
        /// <param name="controlServerPort"></param>
        /// <param name="clientTimerIntervalInMilliseconds"></param>
        /// <param name="serverTimerIntervalInMilliseconds"></param>
        /// <param name="serverLogEventsToEventViewer"></param>
        /// <returns></returns>
        private static int GetSocketSettingsFromConfigFile(out string controlServerName, out int controlServerPort, out double clientTimerIntervalInMilliseconds, out double serverTimerIntervalInMilliseconds)
        {
            controlServerName = string.Empty;
            controlServerPort = -1;
            clientTimerIntervalInMilliseconds = -1;
            serverTimerIntervalInMilliseconds = -1;
            //serverLogEventsToEventViewer = true;
            try
            {
                controlServerName = MIDConfigurationManager.AppSettings["RemoteOptions_ServerName"];
                string tempPort = MIDConfigurationManager.AppSettings["RemoteOptions_ServerPort"];
                string tempIntervalClient = MIDConfigurationManager.AppSettings["RemoteOptions_ClientInterval"];
                string tempIntervalServer = MIDConfigurationManager.AppSettings["RemoteOptions_ServerInterval"];
                //string tempserverLogEventsToEventViewer = MIDConfigurationManager.AppSettings["ServerCommandsLogToEventViewer"];
                //if (tempserverLogEventsToEventViewer.ToLower() == "true")
                //{
                //    serverLogEventsToEventViewer = true;
                //}
                //else
                //{
                //    serverLogEventsToEventViewer = false;
                //}

                int.TryParse(tempPort, out controlServerPort);
                double.TryParse(tempIntervalClient, out clientTimerIntervalInMilliseconds);
                double.TryParse(tempIntervalServer, out serverTimerIntervalInMilliseconds);

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error encountered while attempting to read configuration settings: " + ex.Message);
                //Console.ReadKey();
                return 1;
            }
        }
        private static void PerformClientCommandFromControlService(string command, string tagInfo)
        {
            // Do not respond to control service commands here in this console app, except for get client list
            if (command == SocketSharedRoutines.SocketClientCommands.ReceiveClientList.commandName)
            {
                Console.WriteLine(tagInfo);
                clientListDone.Set();
            }
        }

    }
}
