using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace MIDRetail.Common
{
    [Serializable]
    public class SocketClientManager
    {
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private String response = String.Empty; //The response from the server
        private Socket client;
        //private System.Timers.Timer clientTimer;
        private SocketSharedRoutines.ClientCommandResponderDelegate _controlServiceCommandResponderDelegate;
        public bool ableToConnect;
        public string connectedMessage;

        //public object lockThreadCount = new object();
        //public int threadCount = 0;
        public bool isClosing = false;
        //public void IncreaseTreadCount()
        //{
        //    lock (lockThreadCount)
        //    {
        //        threadCount++;
        //    }
        //}
        //public void DecreaseThreadCount()
        //{
        //    lock (lockThreadCount)
        //    {
        //        threadCount--;
        //    }
        //}

        public SocketClientManager()
        {
            //clientTimer = new System.Timers.Timer();
            //clientTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
        }


        public void StartClient(string controlServerName, int controlServerPort, double clientTimerIntervalInMilliseconds, SocketSharedRoutines.ClientCommandResponderDelegate controlServiceCommandResponderDelegate)
        {
            try
            {
                _controlServiceCommandResponderDelegate = controlServiceCommandResponderDelegate;
                //clientTimer.Interval = clientTimerIntervalInMilliseconds;

                ableToConnect = false;
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo = Dns.GetHostEntry(controlServerName);
                IPAddress ipAddress = SocketSharedRoutines.GetGoodIPAddressFromList(ipHostInfo.AddressList);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, controlServerPort);


                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Create a TCP/IP socket.

                // Connect to the remote endpoint.
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne(); //wait

                ableToConnect = true;
            }
            catch (Exception ex)
            {
                HandleClientException(ex);
            }
            //finally
            //{
            //    if (ableToConnect)
            //    {
            //        clientTimer.Enabled = true;
            //    }
            //}
        }
  
        //private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        //{
        //    IncreaseTreadCount();
        //    try
        //    {
        //        if (client != null && client.Connected)
        //        {
        //            // Create the state object.
        //            SocketSharedRoutines.StateObject state = new SocketSharedRoutines.StateObject();
        //            state.workSocket = client;

        //            // Begin receiving the data from the remote device.
        //            client.BeginReceive(state.buffer, 0, SocketSharedRoutines.SocketBufferSize, 0, new AsyncCallback(ReceiveCallback), state);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleClientException(ex);
        //    }
        //    finally
        //    {
        //        DecreaseThreadCount();
        //    }
        //}
        public void StopClient()
        {
            isClosing = true;
            //clientTimer.Enabled = false;
            // Release the socket.
            try
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch
            {
                //swallow any errors when cleaning up
            }
        }
        public void SendCommandToServer(SocketSharedRoutines.SocketServerCommands commandForServer, string commandInfoTag)
        {
            try
            {
                string cmd = commandForServer.commandName + SocketSharedRoutines.Tags.tagStart + commandInfoTag + SocketSharedRoutines.Tags.tagEnd;
                SocketSharedRoutines.ServerCommandEncode(ref cmd);
                //send the command to the server
                if (client.Connected)
                {
                    byte[] byteData = Encoding.ASCII.GetBytes(cmd);
                    client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
                }
            }
            catch (SocketException se)
            {
                HandleSocketException(se);
            }
            catch (Exception ex)
            {
                HandleClientException(ex);
            }
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);
                this.connectedMessage = "Connected to control service at: " + client.RemoteEndPoint.ToString();
                Console.WriteLine(connectedMessage);
                WaitForServerData(client);  //returns immediately - and when data is received it will invoke its own callback

            }
            catch (SocketException se)
            {
                HandleUnableToConnect(se);
            }
            catch (Exception ex)
            {
                HandleClientException(ex);
            }
            finally
            {
                //Signal that the connection has been made.
                connectDone.Set();
            }
        }

        public AsyncCallback pfnCallBack = null;
        private void WaitForServerData(Socket soc)
        {
            try
            {
                if (pfnCallBack == null)
                {
                    pfnCallBack = new AsyncCallback(HandleServerDataReceived);
                }

                SocketSharedRoutines.StateObject state = new SocketSharedRoutines.StateObject();
                state.workSocket = soc;
                IAsyncResult asynResult = soc.BeginReceive(state.buffer, 0, SocketSharedRoutines.SocketBufferSize, SocketFlags.None, pfnCallBack, state);  //begin listening for data from the server
            }
            catch (SocketException se)
            {
                HandleUnableToConnect(se);
            }
            catch (Exception ex)
            {
                HandleClientException(ex);
            }
        }


        private void HandleServerDataReceived(IAsyncResult ar)
        {
            try
            {
                //Retrieve the state object and the client socket from the asynchronous state object.
                SocketSharedRoutines.StateObject state = (SocketSharedRoutines.StateObject)ar.AsyncState;
                Socket client = state.workSocket;
                if (client != null)
                {
                    // Read data from the server
                    int bytesRead = client.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        // There  might be more data, so store the data received so far.
                        state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                        // Check for end-of-file tag. If it is not there, read more data.
                        response = state.sb.ToString();

                        if (response.StartsWith(SocketSharedRoutines.commandClientPrefix)) //only respond to data intended for the MID client app
                        {
                            if (response.IndexOf(SocketSharedRoutines.commandEOF) > -1)
                            {
                                //All the data has been read from the server
                                if (response.StartsWith(SocketSharedRoutines.commandClientPrefix)) //only respond to data intended for the MID client app
                                {
                                    string tagInfo = string.Empty;
                                    SocketSharedRoutines.ClientCommandDecode(ref response, ref tagInfo);
                                    _controlServiceCommandResponderDelegate.Invoke(response, tagInfo);

                                    //IAsyncResult asynResult = client.BeginReceive(state.buffer, 0, SocketSharedRoutines.SocketBufferSize, SocketFlags.None, pfnCallBack, state);  //loop back and listen for a new command from the server
                                    WaitForServerData(client);
                                }

                            }
                            else
                            {
                                // Get the rest of the data.
                                //client.BeginReceive(state.buffer, 0, SocketSharedRoutines.SocketBufferSize, 0, new AsyncCallback(ReceiveCallback), state);

                                IAsyncResult asynResult = client.BeginReceive(state.buffer, 0, SocketSharedRoutines.SocketBufferSize, SocketFlags.None, pfnCallBack, state);  //loop back and listen for more data
 
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
                HandleClientException(ex);
            }
        }

    
      
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                if (client.Connected)
                {
                    int bytesSent = client.EndSend(ar);
                    //Console.WriteLine("Sent {0} bytes to server.", bytesSent);
                }

                // Signal that all bytes have been sent.
                //sendDone.Set();
            }
            catch (SocketException se)
            {
                HandleUnableToConnect(se);
            }
            catch (Exception ex)
            {
                HandleClientException(ex);
            }
        }


        private void HandleRetryToConnect(Socket client)
        {
        }
        private void HandleUnableToConnect(SocketException se)
        {
            Console.WriteLine("Client unable to connect to control service at: {0}", client.RemoteEndPoint.ToString());

            throw se;
        }
        private void HandleSocketException(SocketException se)
        {
            if (isClosing == false) //suppress all errors when closing
            {
                //System.Windows.Forms.MessageBox.Show("Client Socket Error:" + se.ToString());
                if (se.ToString().Contains("An existing connection was forcibly closed by the remote host"))
                {
                    //We have been disconnected from the control service.
                    //just do nothing for now.
                }
                else
                {
                    Console.WriteLine(se.ToString());
                    throw se;
                }
            }
        }
        private void HandleClientException(Exception ex)
        {
            if (isClosing == false) //suppress all errors when closing
            {
                //System.Windows.Forms.MessageBox.Show("Client Socket General Error:" + ex.ToString());
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

    }
}
