using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;



namespace MIDRetail.Common
{
    public static class SocketSharedRoutines
    {
        public const int SocketBufferSize = 1024;


        // State object for reading data asynchronously
        public class StateObject
        {
            public Socket workSocket = null;
         
            public byte[] buffer = new byte[SocketBufferSize];
            public StringBuilder sb = new StringBuilder(); // Received data string.
        }

        /// <summary>
        /// Returns a good IPv4 IP address from a list containing both IPv6 and IPv4 addresses
        /// </summary>
        /// <param name="addressList"></param>
        /// <returns></returns>
        public static IPAddress GetGoodIPAddressFromList(IPAddress[] addressList)
        {
            IPAddress goodIPAddr = null;
            bool foundGoodOne = false;
            int i = 0;
            while (foundGoodOne == false && i < addressList.Length)
            {
                IPAddress ipAddr = addressList[i];
                if (ipAddr.AddressFamily == AddressFamily.InterNetwork) // IPv4 address
                {
                    foundGoodOne = true;
                    goodIPAddr = ipAddr;
                }
                i++;
            }
            return goodIPAddr;
        }
        public static bool AreSocketRemoteEndPointsEqual(Socket a, Socket b)
        {
            IPEndPoint aRemoteIpEndPoint = a.RemoteEndPoint as IPEndPoint;
            IPEndPoint bRemoteIpEndPoint = b.RemoteEndPoint as IPEndPoint;

            if (aRemoteIpEndPoint.Address.Equals(bRemoteIpEndPoint.Address) && aRemoteIpEndPoint.Port == bRemoteIpEndPoint.Port)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public delegate void ClientCommandResponderDelegate(string commandFromControlService, string commandInfoTag);
       
        public const string commandEOF = "<EOF>"; //required suffix for all commands sent to the server(control service) or client apps
        public const string commandClientPrefix = "<MID_ClientCmd>";  //required prefix for all commands sent to the client apps
        public const string commandServerPrefix = "<MID_ServerCmd>";  //required prefix for all commands sent to the server(control service)
        public const string controlServiceNameForEventLogs = "MIDControlService";
        public const string moduleNameForAuditLogs = "BatchOnlyMode";
        public const int showCurrentUsers_ResponseWaitLimitInSeconds = 15;

        public static class Tags
        {
            public const string tagStart = "<tag>";
            public const string tagEnd = "</tag>";
            public const string rowStart = "<row>";
            public const string rowEnd = "</row>";
            public const string requestIdStart = "<requestID>";
            public const string requestIdEnd = "</requestID>";
            public const string clientIPAddressStart = "<clientIP>";
            public const string clientIPAddressEnd = "</clientIP>";
            public const string clientPortStart = "<clientPort>";
            public const string clientPortEnd = "</clientPort>";
            public const string issuedByStart = "<issuedBy>";
            public const string issuedByEnd = "</issuedBy>";
            public const string userNameStart = "<userName>";
            public const string userNameEnd = "</userName>";
            public const string machineNameStart = "<machineName>";
            public const string machineNameEnd = "</machineName>";
            public const string appStatusStart = "<appStatus>";
            public const string appStatusEnd = "</appStatus>";
            public const string clientTypeStart = "<clientType>";
            public const string clientTypeEnd = "</clientType>";
        }


  


        /// <summary>
        /// Adds the suffix and prefix to the client command
        /// </summary>
        /// <param name="cmd"></param>
        public static void ClientCommandEncode(ref string cmd)
        {
            cmd = commandClientPrefix + cmd + commandEOF;
        }

        /// <summary>
        /// Removes the suffix and prefix from the client command
        /// </summary>
        /// <param name="cmd"></param>
        public static void ClientCommandDecode(ref string cmd, ref string cmdTagInfo)
        {
            cmdTagInfo = string.Empty;
            cmd = cmd.Replace(commandClientPrefix, string.Empty);
            cmd = cmd.Replace(commandEOF, string.Empty);

            int tagInfoStart = cmd.IndexOf(Tags.tagStart);
            if (tagInfoStart > -1)
            {
                int tagInfoEnd = cmd.IndexOf(Tags.tagEnd);
                cmdTagInfo = cmd.Substring(tagInfoStart + 5, (tagInfoEnd - tagInfoStart - 5));
                cmd = cmd.Substring(0, tagInfoStart);
            }
        }

        /// <summary>
        /// Adds the suffix and prefix to the server command
        /// </summary>
        /// <param name="cmd"></param>
        public static void ServerCommandEncode(ref string cmd)
        {
            cmd = commandServerPrefix + cmd + commandEOF;
        }

        /// <summary>
        /// Removes the suffix and prefix from the server command
        /// </summary>
        /// <param name="cmd"></param>
        public static void ServerCommandDecode(ref string cmd, ref string cmdTagInfo)
        {
            cmdTagInfo = string.Empty;
            cmd = cmd.Replace(commandServerPrefix, string.Empty);
            cmd = cmd.Replace(commandEOF, string.Empty);

            int tagInfoStart = cmd.IndexOf(Tags.tagStart);
            if (tagInfoStart > -1)
            {
                int tagInfoEnd = cmd.IndexOf(Tags.tagEnd);
                cmdTagInfo = cmd.Substring(tagInfoStart + 5, (tagInfoEnd - tagInfoStart - 5));
                cmd = cmd.Substring(0, tagInfoStart);
            }

        }
        public static int GetRequestIDFromTagInfo(string taginfo)
        {
            int requestID_start = taginfo.IndexOf(Tags.requestIdStart) + Tags.requestIdStart.Length;
            int requestID_end = taginfo.IndexOf(Tags.requestIdEnd);
            string sRequestID = taginfo.Substring(requestID_start, requestID_end - requestID_start);
            int requestID = -1;
            int.TryParse(sRequestID, out requestID);
            return requestID;
        }
        public static string MakeRequestIDForTagInfo(int requestID)
        {
            string taginfo = Tags.requestIdStart + requestID + Tags.requestIdEnd;
            return taginfo;
        }
        public static string MakeClientIPForTagInfo(Socket s)
        {
            IPEndPoint remoteIpEndPoint = s.RemoteEndPoint as IPEndPoint;
            string taginfo = Tags.clientIPAddressStart + remoteIpEndPoint.Address.ToString() + Tags.clientIPAddressEnd;
            return taginfo;
        }
        public static string MakeClientIPForTagInfo(string IPAddress)
        {
            string taginfo = Tags.clientIPAddressStart + IPAddress + Tags.clientIPAddressEnd;
            return taginfo;
        }
        public static string MakeClientPortForTagInfo(Socket s)
        {
            IPEndPoint remoteIpEndPoint = s.RemoteEndPoint as IPEndPoint;
            string taginfo = Tags.clientPortStart + remoteIpEndPoint.Port.ToString() + Tags.clientPortEnd;
            return taginfo;
        }
        public static string MakeClientPortForTagInfo(int port)
        {
            string taginfo = Tags.clientPortStart + port.ToString() + Tags.clientPortEnd;
            return taginfo;
        }
        public static string MakeUserNameForTagInfo(string userName)
        {
            string taginfo = Tags.userNameStart + userName.ToString() + Tags.userNameEnd;
            return taginfo;
        }
        public static string GetInfoFromTags(string taginfo, string startTag, string endTag)
        {
            int start = taginfo.IndexOf(startTag) + startTag.Length;
            int end = taginfo.IndexOf(endTag);
            return taginfo.Substring(start, end - start);
        }

        public sealed class SocketClientCommands
        {
            public static readonly SocketClientCommands ShutDown = new SocketClientCommands("ShutDown");
            public static readonly SocketClientCommands ShowMessage = new SocketClientCommands("ShowMessage");
            public static readonly SocketClientCommands GiveUserInfo = new SocketClientCommands("GiveUserInfo");
            public static readonly SocketClientCommands ReceiveClientList = new SocketClientCommands("ReceiveClientList");
            public static readonly SocketClientCommands ReceiveCurrentUsers = new SocketClientCommands("ReceiveCurrentUsers");

            private SocketClientCommands(string commandName)
            {
                this.commandName = commandName;
            }
            public string commandName { get; private set; }
            public static implicit operator string(SocketClientCommands op) { return op.commandName; }
        }

        public sealed class SocketServerCommands
        {
            public static List<string> serverCommandList = new List<string>();
            public static readonly SocketServerCommands IssueShutDown = new SocketServerCommands("IssueShutDown");
            public static readonly SocketServerCommands IssueShutDownForSelectedUsers = new SocketServerCommands("IssueShutDownForSelectedUsers");
            public static readonly SocketServerCommands IssueShowMessage = new SocketServerCommands("IssueShowMessage");
            public static readonly SocketServerCommands SetBatchOnlyModeOn = new SocketServerCommands("SetBatchOnlyModeOn");
            public static readonly SocketServerCommands SetBatchOnlyModeOff = new SocketServerCommands("SetBatchOnlyModeOff");
            public static readonly SocketServerCommands GetClientList = new SocketServerCommands("GetClientList");  //returns a list of all connected clients and their IP
            public static readonly SocketServerCommands GetCurrentUsers = new SocketServerCommands("GetCurrentUsers");
            public static readonly SocketServerCommands TakeUserInfo = new SocketServerCommands("TakeUserInfo");
            public static readonly SocketServerCommands SetAsJobServiceClient = new SocketServerCommands("SetAsJobServiceClient");

            private SocketServerCommands(string commandName)
            {
                this.commandName = commandName;
                serverCommandList.Add(commandName);
            }
            public string commandName { get; private set; }
            public static implicit operator string(SocketServerCommands op) { return op.commandName; }
        }
    }
}
