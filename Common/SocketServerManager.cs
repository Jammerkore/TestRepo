using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.ServiceProcess;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common
{
    public class SocketServerManager
    {
        //private System.Timers.Timer serverTimer;
        private Socket serverListener;

        private List<Socket> clientSocketList = new List<Socket>();
        private List<ActiveRequest> activeRequestList = new List<ActiveRequest>();
        private object listLock = new object();
        public Audit audit;
        public bool IsApplicationInBatchOnlyMode;
        public string BatchModeLastChangedBy = string.Empty;
        public System.Net.Sockets.Socket jobServiceClientSocket = null;

        private System.Timers.Timer activeRequestTimer;

        public SocketServerManager()
        {
            //serverTimer = new System.Timers.Timer();
            //serverTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);

            activeRequestTimer = new System.Timers.Timer();
            activeRequestTimer.Elapsed += new System.Timers.ElapsedEventHandler(ActiveRequests_Process);
        }

        public void StartListening(string controlServerName, int controlServerPort, double serverTimerIntervalInMilliseconds, Audit audit)
        {
            this.audit = audit;

            // Data buffer for incoming data.
            byte[] bytes = new Byte[SocketSharedRoutines.SocketBufferSize];

          
            //if (logEvents)
            //{
            EventLog.WriteEntry(SocketSharedRoutines.controlServiceNameForEventLogs, "Attempting to establish server listener on:" + System.Environment.NewLine + "ControlServerName: " + controlServerName + System.Environment.NewLine + "Port: " + controlServerPort.ToString(), EventLogEntryType.Information);
            //}

            //Establish the local endpoint for the socket
            IPHostEntry ipHostInfo = Dns.GetHostEntry(controlServerName); 
            IPAddress ipAddress = SocketSharedRoutines.GetGoodIPAddressFromList(ipHostInfo.AddressList);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, controlServerPort);

            serverListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Create a TCP/IP socket

            //if (logEvents)
            //{
            EventLog.WriteEntry(SocketSharedRoutines.controlServiceNameForEventLogs, "Established server listener on:" + System.Environment.NewLine + "ControlServerName: " + controlServerName + System.Environment.NewLine + "Port: " + controlServerPort.ToString(), EventLogEntryType.Information);
            //}
            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                serverListener.Bind(localEndPoint);
                serverListener.Listen(100);



                //serverTimer.Interval = serverTimerIntervalInMilliseconds;
                //serverTimer.Enabled = true;

                activeRequestTimer.Interval = serverTimerIntervalInMilliseconds;


                serverListener.BeginAccept(new AsyncCallback(HandleClientConnect), null);  //begin listening for clients
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }
        private void HandleClientConnect(IAsyncResult ar)
        {
            Socket socClient = serverListener.EndAccept(ar);

            lock (listLock)
            {
                clientSocketList.Add(socClient); //add client socket to the list
            }

            WaitForClientData(socClient);  //returns immediately - and when data is received it will invoke its own callback

            serverListener.BeginAccept(new AsyncCallback(HandleClientConnect), null); //loop back and listen for more clients
        }
        public AsyncCallback pfnCallBack = null;

        /// <summary>
        /// handles receiving all client data for all client sockets
        /// </summary>
        /// <param name="socClient"></param>
        private void WaitForClientData(Socket socClient)
        {
            if (pfnCallBack == null)
            {
                pfnCallBack = new AsyncCallback(HandleClientDataReceived);
            }

            SocketSharedRoutines.StateObject state = new SocketSharedRoutines.StateObject();
            state.workSocket = socClient;
            IAsyncResult asynResult = socClient.BeginReceive(state.buffer, 0, SocketSharedRoutines.SocketBufferSize, SocketFlags.None, pfnCallBack, state);  //begin listening for data from the client
        }
        public void HandleClientDataReceived(IAsyncResult ar)
        {
            try
            {
                String content = String.Empty;

                SocketSharedRoutines.StateObject state = (SocketSharedRoutines.StateObject)ar.AsyncState;
                Socket socClient = state.workSocket;

                // Read data from the client socket. 
                if (socClient.Connected)
                {
                    int bytesRead = socClient.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        // There  might be more data, so store the data received so far.
                        state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                        // Check for end-of-file tag. If it is not there, read more data.
                        content = state.sb.ToString();
                        if (content.StartsWith(SocketSharedRoutines.commandServerPrefix)) //only respond to data intended for the MID server(the control service)
                        {
                            if (content.IndexOf(SocketSharedRoutines.commandEOF) > -1)
                            {
                                // All the data has been read from the client. 
                                if (content.StartsWith(SocketSharedRoutines.commandServerPrefix)) //only respond to data intended for the MID server(the control service)
                                {
                                    string tagInfo = string.Empty;
                                    SocketSharedRoutines.ServerCommandDecode(ref content, ref tagInfo);

                                    //_serverCommandResponderDelegate.Invoke(content, tagInfo, handler);
                                    this.PerformServerCommandFromClientApp(content, tagInfo, socClient);

                                    //IAsyncResult asynResult = socClient.BeginReceive(state.buffer, 0, SocketSharedRoutines.SocketBufferSize, SocketFlags.None, pfnCallBack, state);  //loop back and listen for a new command from this client
                                    WaitForClientData(socClient);
                                }
                            }
                            else
                            {
                                // Not all data received. Get more.
                                IAsyncResult asynResult = socClient.BeginReceive(state.buffer, 0, SocketSharedRoutines.SocketBufferSize, SocketFlags.None, pfnCallBack, state);  //loop back and listen for more data
                            }
                        }
                    }
                }
            }
            catch (SocketException se)
            {
                HandleSocketException(se);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void PerformServerCommandFromClientApp(string commandFromClient, string tagInfo, System.Net.Sockets.Socket fromClientSocket)
        {
            if (commandFromClient == SocketSharedRoutines.SocketServerCommands.IssueShowMessage.commandName)
            {
                SendCommandToClients(SocketSharedRoutines.SocketClientCommands.ShowMessage, tagInfo, fromClientSocket);
            }
            if (commandFromClient == SocketSharedRoutines.SocketServerCommands.IssueShutDown.commandName)
            {
                SendCommandToClients(SocketSharedRoutines.SocketClientCommands.ShutDown, tagInfo, fromClientSocket);
            }
            if (commandFromClient == SocketSharedRoutines.SocketServerCommands.SetBatchOnlyModeOn.commandName)
            {
                if (StopJobService())
                {
                    IsApplicationInBatchOnlyMode = true;
                    BatchModeLastChangedBy = RemoteSystemOptions.Messages.BatchOnlyModeOnLastChangedByPrefix + tagInfo;

                    audit.Add_Msg(eMIDMessageLevel.Information, BatchModeLastChangedBy, SocketSharedRoutines.moduleNameForAuditLogs, true);
                    //Automatically issue a client shutdown command when Batch Only Mode is set on
                    SendCommandToClients(SocketSharedRoutines.SocketClientCommands.ShutDown, tagInfo, fromClientSocket);

                    //Begin TT#1517-MD -jsobek -Store Service Optimization
                    //Wait 30 seconds then remove old data
                    Thread.Sleep(30000);
                    StoreGroupMaint groupData = new StoreGroupMaint();
                    try
                    {
                        groupData.OpenUpdateConnection();
                        groupData.StoreGroupJoinHistory_DeleteAll();
                        groupData.CommitData();
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                    finally
                    {
                        groupData.CloseUpdateConnection();
                    }
                    //End TT#1517-MD -jsobek -Store Service Optimization
                }
            }
            if (commandFromClient == SocketSharedRoutines.SocketServerCommands.SetBatchOnlyModeOff.commandName)
            {
                if (StartJobService())
                {
                    // Begin RO-3962 - JSmith - Purge is not deleting dynamic empty set attribute sets
                    //Wait 30 seconds then remove old data
                    Thread.Sleep(30000);
                    StoreGroupMaint groupData = new StoreGroupMaint();
                    try
                    {
                        groupData.OpenUpdateConnection();
                        groupData.StoreGroupJoinHistory_DeleteAll();
                        groupData.CommitData();
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                    finally
                    {
                        groupData.CloseUpdateConnection();
                    }
                    // End RO-3962 - JSmith - Purge is not deleting dynamic empty set attribute sets

                    IsApplicationInBatchOnlyMode = false;
                    BatchModeLastChangedBy = RemoteSystemOptions.Messages.BatchOnlyModeOffLastChangedByPrefix + tagInfo;

                    audit.Add_Msg(eMIDMessageLevel.Information, BatchModeLastChangedBy, SocketSharedRoutines.moduleNameForAuditLogs, true);
                }
            }
            if (commandFromClient == SocketSharedRoutines.SocketServerCommands.GetClientList.commandName)
            {
                string sClientList = GetCurrentClientList();
                SendCommandToClient(SocketSharedRoutines.SocketClientCommands.ReceiveClientList, sClientList, fromClientSocket);
            }
            if (commandFromClient == SocketSharedRoutines.SocketServerCommands.GetCurrentUsers.commandName)
            {
                ActiveRequest request = new ActiveRequest();
                request.adminClientWhoIssuedCommand = fromClientSocket;
                request.serverCommandIssuedFromClient = SocketSharedRoutines.SocketServerCommands.GetCurrentUsers;
                request.requestStartTime = DateTime.Now;
                request.requestID = ActiveRequests_GetNewRequestID();
                ActiveRequests_Add(request);
                activeRequestTimer.Enabled = true;
                string requestTag = SocketSharedRoutines.MakeRequestIDForTagInfo(request.requestID);
                SendCommandToClients(SocketSharedRoutines.SocketClientCommands.GiveUserInfo, requestTag, fromClientSocket);
            }
            if (commandFromClient == SocketSharedRoutines.SocketServerCommands.TakeUserInfo.commandName)
            {
                string clientIP = SocketSharedRoutines.MakeClientIPForTagInfo(fromClientSocket);
                string clientPort = SocketSharedRoutines.MakeClientPortForTagInfo(fromClientSocket);
                tagInfo = tagInfo.Replace(SocketSharedRoutines.Tags.rowEnd, clientIP + clientPort + SocketSharedRoutines.Tags.rowEnd);
                ActiveRequests_AddResponse(tagInfo, fromClientSocket);
            }
            if (commandFromClient == SocketSharedRoutines.SocketServerCommands.IssueShutDownForSelectedUsers.commandName)
            {

                string info = tagInfo;
                string issuedBy = SocketSharedRoutines.GetInfoFromTags(info, SocketSharedRoutines.Tags.issuedByStart, SocketSharedRoutines.Tags.issuedByEnd);

                int currentRowPosition = info.IndexOf(SocketSharedRoutines.Tags.rowStart);
                while (currentRowPosition != -1)
                {
                    string sRow = SocketSharedRoutines.GetInfoFromTags(info, SocketSharedRoutines.Tags.rowStart, SocketSharedRoutines.Tags.rowEnd);
                    string user = SocketSharedRoutines.GetInfoFromTags(info, SocketSharedRoutines.Tags.userNameStart, SocketSharedRoutines.Tags.userNameEnd);
                    string clientIP = SocketSharedRoutines.GetInfoFromTags(info, SocketSharedRoutines.Tags.clientIPAddressStart, SocketSharedRoutines.Tags.clientIPAddressEnd);
                    string clientPort = SocketSharedRoutines.GetInfoFromTags(info, SocketSharedRoutines.Tags.clientPortStart, SocketSharedRoutines.Tags.clientPortEnd);

                    //find client socket from IP and port
                    Socket clientToShutDown = clientSocketList.Find(delegate(Socket o) 
                        {
                            IPEndPoint aRemoteIpEndPoint = o.RemoteEndPoint as IPEndPoint;
                            return (aRemoteIpEndPoint.Address.ToString() == clientIP && aRemoteIpEndPoint.Port.ToString() == clientPort); 
                        });
                    if (clientToShutDown != null)
                    {
                        string commandInfoTag = issuedBy;
                        if (jobServiceClientSocket != null
						    && SocketSharedRoutines.AreSocketRemoteEndPointsEqual(clientToShutDown, jobServiceClientSocket))
                        {
                            commandInfoTag += "|" + user;
                        }
                        SendCommandToClient(SocketSharedRoutines.SocketClientCommands.ShutDown, commandInfoTag, clientToShutDown);
                    }

                    int nextRowPosition = info.IndexOf(SocketSharedRoutines.Tags.rowEnd) + SocketSharedRoutines.Tags.rowEnd.Length;
                    info = info.Substring(nextRowPosition);
                    currentRowPosition = info.IndexOf(SocketSharedRoutines.Tags.rowStart);
                }
            
            }
            if (commandFromClient == SocketSharedRoutines.SocketServerCommands.SetAsJobServiceClient)
            {
                jobServiceClientSocket = fromClientSocket;
            }
        }

        public void StopListening()
        {
            //serverTimer.Enabled = false;
            foreach (Socket s in clientSocketList)
            {
                try
                {
                    s.Shutdown(SocketShutdown.Both);
                    s.Close();
                }
                catch
                {
                    //swallow any errors when attempting to clean up
                }
            }
        }

        //private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        //{
        //    try
        //    {
        //        serverListener.BeginAccept(new AsyncCallback(AcceptCallback), serverListener); //Start an asynchronous socket to listen for connections.
        //        ListenForIncomingClients(); //listen for incoming
        //        RemoveDroppedClients(); //remove sockets that are no longer connected
        //        ActiveRequests_Process(); //Process active requests
        //        GarbageCollect();   //TT#1499-MD -jsobek -Control Service - Add explicit garbage collection
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //Begin TT#1499-MD -jsobek -Control Service - Add explicit garbage collection
        private void GarbageCollect()
        {
            try
            {
                System.GC.Collect();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        //End TT#1499-MD -jsobek -Control Service - Add explicit garbage collection

        //private void ListenForIncomingClients()
        //{
        //    try
        //    {
        //        foreach (Socket s in clientSocketList)
        //        {
        //            SocketSharedRoutines.StateObject state = new SocketSharedRoutines.StateObject();
        //            state.workSocket = s;
        //            try
        //            {
        //                if (s.Connected)
        //                {
        //                    s.BeginReceive(state.buffer, 0, SocketSharedRoutines.SocketBufferSize, 0, new AsyncCallback(ReadCallback), state);
        //                }
        //            }
        //            catch (SocketException se)
        //            {
        //                HandleSocketException(se);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        private void RemoveDroppedClients()
        {
            try
            {
                lock (listLock)
                {

                    for (int i = clientSocketList.Count - 1; i >= 0; i--)
                    {
                        Socket s = clientSocketList.ElementAt(i);
                        try
                        {
                            if (s.Connected == false)
                            {
                                //Begin TT#1499-MD -jsobek -Control Service - Add explicit garbage collection
                                s.Shutdown(SocketShutdown.Both);
                                s.Close();
                                //End TT#1499-MD -jsobek -Control Service - Add explicit garbage collection
                                clientSocketList.RemoveAt(i);
                            }
                        }
                        catch (SocketException se)
                        {
                            HandleSocketException(se);
                            clientSocketList.RemoveAt(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        private void ActiveRequests_Process(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                foreach (ActiveRequest request in activeRequestList)
                {
                    bool doCompleteRequest = false;
                    //Check to see if longer than 15 seconds, if so, complete the request
                    TimeSpan ts = DateTime.Now - request.requestStartTime;
                    if (ts.TotalSeconds >= SocketSharedRoutines.showCurrentUsers_ResponseWaitLimitInSeconds)
                    {
                        doCompleteRequest = true;
                    }

                    //check the total number of repsonses, if greater than or equal to the number of current client connections, complete the request
                    if (request.tagResultList.Count >= clientSocketList.Count)
                    {
                        doCompleteRequest = true;
                    }


                    if (doCompleteRequest)
                    {
                        if (request.serverCommandIssuedFromClient == SocketSharedRoutines.SocketServerCommands.GetCurrentUsers)
                        {
                            //send the results to the client who request it

                            //combine all the results into one big taginfo string
                            string finalResults = string.Empty;
                            foreach (string s in request.tagResultList)
                            {
                                finalResults += s;
                            }

                            SendCommandToClient(SocketSharedRoutines.SocketClientCommands.ReceiveCurrentUsers, finalResults, request.adminClientWhoIssuedCommand);
                            //mark as completed so the request can be removed
                            request.isCompleted = true;
                        }
                    }

                }

                //Remove completed requests
                for (int i = activeRequestList.Count - 1; i >= 0; i--)
                {
                    if (activeRequestList[i].isCompleted == true)
                    {
                        activeRequestList.RemoveAt(i);
                    }
                }

                if (activeRequestList.Count == 0)
                {
                    activeRequestTimer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ActiveRequests_Add(ActiveRequest request)
        {
            try
            {
                lock (listLock)
                {
                    activeRequestList.Add(request);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private int ActiveRequests_GetNewRequestID()
        {
            lock (listLock)
            {
                //get the highest number in the list and then add one
                int highestRequestID = 0;
                foreach (ActiveRequest request in activeRequestList)
                {
                    if (request.requestID > highestRequestID)
                    {
                        highestRequestID = request.requestID;
                    }
                }
                highestRequestID++;
                return highestRequestID;
            }
        }
        private void ActiveRequests_AddResponse(string taginfo, Socket fromClient)
        {
            try
            {
                //add this repsonse to the appropriate request
                int requestID = SocketSharedRoutines.GetRequestIDFromTagInfo(taginfo);
                lock (listLock)
                {
                    ActiveRequest request = activeRequestList.Find(delegate(ActiveRequest o) { return o.requestID == requestID; });
                    if (request != null)
                    {
                        request.tagResultList.Add(taginfo);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public class ActiveRequest
        {
            public List<string> tagResultList = new List<string>();
            public int requestID;
            public SocketSharedRoutines.SocketServerCommands serverCommandIssuedFromClient;
            public System.Net.Sockets.Socket adminClientWhoIssuedCommand;
            public DateTime requestStartTime;
            public bool isCompleted = false;
        }

        private void HandleSocketException(SocketException se)
        {
            if (se.ToString().Contains("An existing connection was forcibly closed by the remote host"))
            {
                //swallow error
            }
            else if (se.ToString().Contains("An established connection was aborted by the software in your host machine"))
            {
                //swallow error
            }
            else
            {
                HandleException(se);
            }
        }
        private void HandleException(Exception ex)
        {
            EventLog.WriteEntry(SocketSharedRoutines.controlServiceNameForEventLogs, ex.ToString(), EventLogEntryType.Error);
        }
        //public void AcceptCallback(IAsyncResult ar)
        //{
        //    try
        //    {
        //        // Get the socket that handles the client request.
        //        Socket listener = (Socket)ar.AsyncState;
        //        Socket handler = listener.EndAccept(ar);

        //        // Create the state object.
        //        SocketSharedRoutines.StateObject state = new SocketSharedRoutines.StateObject();
        //        state.workSocket = handler;
        //        lock (listLock)
        //        {
        //            clientSocketList.Add(handler);
        //        }
        //        //handler.BeginReceive(state.buffer, 0, SocketSharedRoutines.SocketBufferSize, 0, new AsyncCallback(ReadCallback), state);
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //public void ReadCallback(IAsyncResult ar)
        //{
        //    try
        //    {
        //        String content = String.Empty;

        //        // Retrieve the state object and the handler socket
        //        // from the asynchronous state object.
        //        SocketSharedRoutines.StateObject state = (SocketSharedRoutines.StateObject)ar.AsyncState;
        //        Socket handler = state.workSocket;

        //        // Read data from the client socket. 
        //        if (handler.Connected)
        //        {
        //            int bytesRead = handler.EndReceive(ar);

        //            if (bytesRead > 0)
        //            {
        //                // There  might be more data, so store the data received so far.
        //                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

        //                // Check for end-of-file tag. If it is not there, read more data.
        //                content = state.sb.ToString();
        //                if (content.StartsWith(SocketSharedRoutines.commandServerPrefix)) //only respond to data intended for the MID server(the control service)
        //                {
        //                    if (content.IndexOf(SocketSharedRoutines.commandEOF) > -1)
        //                    {
        //                        // All the data has been read from the client. 
        //                        if (content.StartsWith(SocketSharedRoutines.commandServerPrefix)) //only respond to data intended for the MID server(the control service)
        //                        {
        //                            string tagInfo = string.Empty;
        //                            SocketSharedRoutines.ServerCommandDecode(ref content, ref tagInfo);

        //                            //_serverCommandResponderDelegate.Invoke(content, tagInfo, handler);
        //                            this.PerformServerCommandFromClientApp(content, tagInfo, handler);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        // Not all data received. Get more.
        //                        handler.BeginReceive(state.buffer, 0, SocketSharedRoutines.SocketBufferSize, 0, new AsyncCallback(ReadCallback), state);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (SocketException se)
        //    {
        //        HandleSocketException(se);
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        public void SendCommandToClients(SocketSharedRoutines.SocketClientCommands commandForClients, string commandInfoTag, System.Net.Sockets.Socket excludeClientSocket = null)
        {
            try 
            {
                RemoveDroppedClients();
                string cmd = commandForClients.commandName + SocketSharedRoutines.Tags.tagStart + commandInfoTag + SocketSharedRoutines.Tags.tagEnd;
                SocketSharedRoutines.ClientCommandEncode(ref cmd);
                foreach (Socket s in clientSocketList)
                {
                    //exclude a specific client socket if provided
                    bool doSend = true;
                    if (excludeClientSocket != null)
                    {
                        if (SocketSharedRoutines.AreSocketRemoteEndPointsEqual(s, excludeClientSocket))
                        {
                            doSend = false;
                        }
                    }
                    if (doSend)
                    {
                        Send(s, cmd);
                    }
                }
            }
            catch (SocketException se)
            {
                HandleSocketException(se);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        public string GetCurrentClientList()
        {
            try
            {
                RemoveDroppedClients();
                string sList = string.Empty;
                
                foreach (Socket s in clientSocketList)
                {
                    
                    IPEndPoint remoteIpEndPoint = s.RemoteEndPoint as IPEndPoint;
                    //IPEndPoint localIpEndPoint = s.LocalEndPoint as IPEndPoint;
                    sList += "RemoteAddress=" + remoteIpEndPoint.Address + ";RemotePort=" + remoteIpEndPoint.Port.ToString() + System.Environment.NewLine;
                    //if (remoteIpEndPoint != null)
                    //{
                    //    // Using the RemoteEndPoint property.
                    //    Console.WriteLine("I am connected to " + remoteIpEndPoint.Address + "on port number " + remoteIpEndPoint.Port);
                    //}

                    //if (localIpEndPoint != null)
                    //{
                    //    // Using the LocalEndPoint property.
                    //    Console.WriteLine("My local IpAddress is :" + localIpEndPoint.Address + "I am connected on port number " + localIpEndPoint.Port);
                    //}
                    
                }
                return sList;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return ex.ToString();
            }
        }
        public void SendCommandToClient(SocketSharedRoutines.SocketClientCommands commandForClients, string commandInfoTag, System.Net.Sockets.Socket clientSocket)
        {
            try
            {
                string cmd = commandForClients.commandName + SocketSharedRoutines.Tags.tagStart + commandInfoTag + SocketSharedRoutines.Tags.tagEnd;
                SocketSharedRoutines.ClientCommandEncode(ref cmd);
                Send(clientSocket, cmd);
            }
            catch (SocketException se)
            {
                HandleSocketException(se);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void Send(Socket handler, String data)
        {
            try
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.
                if (handler!=null && handler.Connected)
                {
                    handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
                }
            }
            catch (SocketException se)
            {
                HandleSocketException(se);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState; //Retrieve the socket from the state object.

                //Complete sending the data to the client.
                if (handler != null)
                {
                    int bytesSent = handler.EndSend(ar);
                    //Console.WriteLine("Sent {0} bytes to client.", bytesSent);
                }
            }
            catch (SocketException se)
            {
                HandleSocketException(se);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public bool StartJobService()
        {
            string ServiceName = "MIDRetailJobService";

            bool started = false;
            try
            {
                // check if service exists.  If service does not exist.  Return true to not stop process.
                ServiceController ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == ServiceName);
                if (ctl == null)
                {
                    return true;
                }

                // Check whether the service is running.

                ServiceController sc = new ServiceController(ServiceName);

                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    // Start the service if the current status is stopped.
                    try
                    {
                        // Stop the service, and wait until its status is "Stopped".
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 10, 0));
                        started = true;
                    }
                    catch (System.ServiceProcess.TimeoutException)
                    {
                        audit.Add_Msg(eMIDMessageLevel.Severe, "Service " + ServiceName + " did not start in a timely manner. Check Event Viewer", SocketSharedRoutines.moduleNameForAuditLogs, true);
                    }
                    catch (InvalidOperationException)
                    {
                        audit.Add_Msg(eMIDMessageLevel.Severe, "Could not start service " + ServiceName, SocketSharedRoutines.moduleNameForAuditLogs, true);
                    }
                }
                else if (sc.Status == ServiceControllerStatus.Running)
                {
                    started = true;
                }
            }
            catch (InvalidOperationException)
            {
                started = false;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return started;
        }

        public bool StopJobService()
        {
            string ServiceName = "MIDRetailJobService";

            bool stopped = false;
            try
            {
                // check if service exists.  If service does not exist.  Return true to not stop process.
                ServiceController ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == ServiceName);
                if (ctl == null)
                {
                    return true;
                }

                // Check whether the service is running.

                ServiceController sc = new ServiceController(ServiceName);

                if (sc.Status == ServiceControllerStatus.Running)
                {
                    // Stop the service if the current status is running.
                    try
                    {
                        // Stop the service, and wait until its status is "Stopped".
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 10, 0));
                        stopped = true;
                        Thread.Sleep(5000);

                    }
                    catch (System.ServiceProcess.TimeoutException)
                    {
                        audit.Add_Msg(eMIDMessageLevel.Severe, "Service " + ServiceName + " did not stop in a timely manner. Check Event Viewer", SocketSharedRoutines.moduleNameForAuditLogs, true);
                    }
                    catch (InvalidOperationException)
                    {
                        audit.Add_Msg(eMIDMessageLevel.Severe, "Could not stop service " + ServiceName, SocketSharedRoutines.moduleNameForAuditLogs, true);
                    }
                }
                else if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    stopped = true;
                }
            }
            catch (InvalidOperationException)
            {
                stopped = false;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return stopped;
        }
    }

}
