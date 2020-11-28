using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.UnitTestingControlProcessing
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main()
        {
            string moduleName = "UnitTestingContolProcessing";
            string message;
            bool errorFound = false;
            SessionSponsor sponsor;
            SessionAddressBlock SAB;
            IMessageCallback messageCallback;
            System.Runtime.Remoting.Channels.IChannel channel;
            sponsor = new SessionSponsor();
            messageCallback = new BatchMessageCallback();
            SAB = new SessionAddressBlock(messageCallback, sponsor);
            ClientServerSession clientServerSession = null;
            try
            {
                // =========================
                // Register callback channel
                // =========================
                try
                {
                    channel = SAB.OpenCallbackChannel();
                }
                catch (Exception Ex)
                {
                    message = moduleName + ":  Error opening port #0 - " + Ex.Message;
                    try
                    {
                        System.Console.Write(message);
                    }
                    catch (Exception e)
                    {
                        System.Console.Write("Event Log Error [" + e.Message + "] writing event log messge [" + message + "]");
                    }
                    return (int)eMIDMessageLevel.Severe;
                }
                // ===============
                // Create Sessions
                // ===============
                try
                {
                    SAB.CreateSessions((int)eServerType.Client);
                }
                catch (Exception Ex)
                {
                    errorFound = true;
                    Exception innerE = Ex;
                    while (innerE.InnerException != null)
                    {
                        innerE = innerE.InnerException;
                    }
                    message = moduleName + ": Error creating session - " + innerE.Message;
                    try
                    {
                        System.Console.Write(message);
                    }
                    catch (Exception e)
                    {
                        System.Console.Write("Event Log Error [" + e.Message + "] writing event log message [" + message + "]");
                    }
                    return (int)eMIDMessageLevel.Severe;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                try
                {
                    SAB.CreateSessions((int)eServerType.Client);
                }
                catch 
                {
                    errorFound = true;

                    return Convert.ToInt32(eMIDMessageLevel.Severe);
                }

                Application.Run(new Form1(SAB));
            }
            catch 
            {
                return (int)eMIDMessageLevel.Severe;
            }

            return (int)eMIDMessageLevel.Information;
        }
    }
}
