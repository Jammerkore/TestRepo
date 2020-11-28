using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Xml;
using System.Xml.Serialization;


using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.RelieveHeaders
{
    class RelieveHeaders
    {
        static int Main(string[] args)
        {
            eMIDMessageLevel highestMessage;
            int returncode = -1;
            SessionAddressBlock SAB;
            SessionSponsor sponsor;
            IMessageCallback messageCallback;
            string eventLogID = "RelieveHeaders";

            messageCallback = new BatchMessageCallback();
            sponsor = new SessionSponsor();
            SAB = new SessionAddressBlock(messageCallback, sponsor);
            System.Runtime.Remoting.Channels.IChannel channel;
            eSecurityAuthenticate authentication = eSecurityAuthenticate.UnknownUser;

            // Register callback channel

            try
            {
                if (!EventLog.SourceExists(eventLogID))
                {
                    EventLog.CreateEventSource(eventLogID, null);
                }

                channel = SAB.OpenCallbackChannel();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + e.Message, EventLogEntryType.Error);
                throw;
            }

            // Create Sessions

            try
            {
                SAB.CreateSessions((int)eServerType.Client|(int)eServerType.Control);
            }
            catch (Exception ex)
            {
                Exception innerE = ex;
                while (innerE.InnerException != null)
                {
                    innerE = innerE.InnerException;
                }
                EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);
                return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
            }

            //BEGIN TT#1676 - MD - DOConnell - Relieve Headers API is not reporting errors correctly to the Audit
            try
            {
            //END TT#1676 - MD - DOConnell - Relieve Headers API is not reporting errors correctly to the Audit


            authentication = SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
                MIDConfigurationManager.AppSettings["Password"], eProcesses.generateRelieveIntransit);

            //BEGIN TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID
            //BEGIN TT#1644 - MD- DOConnell - Process Control
            if (authentication == eSecurityAuthenticate.Unavailable)
            {
                //return Convert.ToInt32(eMIDMessageLevel.ProcessUnavailable);
                returncode = 1; //TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                return Convert.ToInt32(eMIDMessageLevel.Severe);
            }
            //END TT#1644 - MD- DOConnell - Process Control
            //END TT#1663-VSuart-Header Load and Header Reconcile should return Severe-MID

            if (authentication != eSecurityAuthenticate.UserAuthenticated)
            {
                EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
                System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
                return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
            }

            SAB.ClientServerSession.Initialize();
            //BEGIN TT#1676 - MD - DOConnell - Relieve Headers API is not reporting errors correctly to the Audit
            //try
            //{
            //END TT#1676 - MD - DOConnell - Relieve Headers API is not reporting errors correctly to the Audit
                RelieveHeadersWork worker = new RelieveHeadersWork();
                returncode = worker.GenFile(args, SAB);
                worker.AddMessage(MIDText.GetText(eMIDTextCode.msg_ReturnCode).Replace("{0}", returncode.ToString()), eMIDMessageLevel.Information);
            }
            catch (Exception ex)
            {
                SAB.ClientServerSession.Audit.Log_Exception(ex);
                return 1;
            }
            finally
            {
                if (returncode == 0)
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
                        //BEGIN TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                        //SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                        SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                        //END TT#1665 - MD - DOConnell - Completion Status is not being logged in the Audit correctly.
                    }
                }

                highestMessage = SAB.CloseSessions();
            }

            return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);

        }
    }

    /// <summary>
	/// Relieve selected headers
	/// </summary>
    public class RelieveHeadersWork
    {
        private SessionAddressBlock _SAB;
        private int _totalErrors = 0;


        public int GenFile(string[] args, SessionAddressBlock aSAB)
        {
            _SAB = aSAB;
            string outputFilePath, outputFileName, flagFileSuffix, parm;
            int headersPerFile = 100;

            try
            {
                outputFilePath = MIDConfigurationManager.AppSettings["RelieveIntransitFilePath"];
                outputFileName = MIDConfigurationManager.AppSettings["RelieveIntransitFile"];
                flagFileSuffix = MIDConfigurationManager.AppSettings["FlagFileSuffix"];
                parm = MIDConfigurationManager.AppSettings["HeadersPerFile"];
                if (parm != null)
                {
                    try
                    {
                        headersPerFile = Convert.ToInt32(parm);
                    }
                    catch
                    {
                    }
                }

                AddMessage(MIDText.GetText(eMIDTextCode.msg_OutputFileLocation).Replace("{0}", outputFilePath), eMIDMessageLevel.Information);
                AddMessage(MIDText.GetText(eMIDTextCode.msg_OutputFileName).Replace("{0}", outputFileName), eMIDMessageLevel.Information);
                AddMessage(MIDText.GetText(eMIDTextCode.msg_HeadersPerFile).Replace("{0}", headersPerFile.ToString()), eMIDMessageLevel.Information);
                AddMessage(MIDText.GetText(eMIDTextCode.msg_FlagFileSuffix).Replace("{0}", flagFileSuffix), eMIDMessageLevel.Information);


                if (!outputFilePath.EndsWith("\\"))
                {
                    outputFilePath += "\\";
                }

                GetHeaders(outputFilePath, outputFileName, flagFileSuffix, headersPerFile);

                return 0;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates files for headers to be relieved
        /// </summary>
        /// <param name="aOutputFilePath">The directory where the files are to be written</param>
        /// <param name="aOutputFileName">The root to use for the file name</param>
        /// <param name="aFlagFileSuffix">The suffix to use for the flag file</param>
        /// <param name="aHeadersPerFile">The number of headers to be written to each file</param>
        /// <param name="alog">The log file for messages</param>
        private void GetHeaders(string aOutputFilePath, string aOutputFileName, string aFlagFileSuffix, 
            int aHeadersPerFile)
        {
            string sqlCommand, fileName;
            Header headerData;
            ArrayList relieveIntransitList = null;
            int nextFileNumber = 1;
            int headerCount = 0;
            int fileCount = 0;
            RelieveIntransits relieveIntransits = null;
            XmlSerializer serializer = null;
            // Begin TT#644-MD - JSmith - Modify install values for several configuration settings
            string line = null;
            StreamReader reader;
            // End TT#644-MD - JSmith - Modify install values for several configuration settings

            try
            {
                relieveIntransitList = new ArrayList();
                // Begin TT#644-MD - JSmith - Modify install values for several configuration settings
                //sqlCommand = MIDConfigurationManager.AppSettings["HeaderSQL"];
                sqlCommand = null;
                reader = new StreamReader(MIDConfigurationManager.AppSettings["HeaderSQL"].ToString());
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Trim().Length > 0)
                    {
                        sqlCommand += line.Trim() + " ";
                    }
                }
                reader.Close();
                // End TT#644-MD - JSmith - Modify install values for several configuration settings

                AddMessage(MIDText.GetText(eMIDTextCode.msg_RelieveIntransitHeaderCommand).Replace("{0}", sqlCommand), eMIDMessageLevel.Information);

                headerData = new Header();
                DataTable dt = headerData.ExecuteSQLQuery(sqlCommand, "Headers");

                if (dt.Rows.Count == 0)
                {
                    AddMessage(MIDText.GetText(eMIDTextCode.msg_NoHeadersToRelieve), eMIDMessageLevel.Information);
                    return;
                }

                foreach (DataRow dr in dt.Rows)
                {
                    if (relieveIntransitList.Count == 0)
                    {
                        relieveIntransits = new RelieveIntransits();
                        serializer = new XmlSerializer(typeof(RelieveIntransits));
                    }

                    RelieveIntransitsRelieveIntransit relieveIntransit = new RelieveIntransitsRelieveIntransit();
                    relieveIntransit.HeaderID = Convert.ToString(dr["HeaderID"]);
                    relieveIntransit.ProductID = Convert.ToString(dr["ProductID"]);

                    AddMessage(MIDText.GetText(eMIDTextCode.msg_GenerageRelieveForHeader).Replace("{0}", relieveIntransit.HeaderID), eMIDMessageLevel.Information);

                    relieveIntransit.RelieveStore = new RelieveIntransitsRelieveIntransitRelieveStore[1];
                    RelieveIntransitsRelieveIntransitRelieveStore RIStore = new RelieveIntransitsRelieveIntransitRelieveStore();
                    RIStore.RelieveAllStores = true;
                    RIStore.RelieveAllStoresSpecified = true;
                    relieveIntransit.RelieveStore[0] = RIStore;
                    ++headerCount;

                    relieveIntransitList.Add(relieveIntransit);

                    if (relieveIntransitList.Count == aHeadersPerFile)
                    {
                        relieveIntransits.RelieveIntransit = new RelieveIntransitsRelieveIntransit[relieveIntransitList.Count];
                        relieveIntransitList.CopyTo(0, relieveIntransits.RelieveIntransit, 0, relieveIntransitList.Count);

                        fileName = GetFileName(aOutputFilePath, aOutputFileName, aFlagFileSuffix, ref nextFileNumber);
                        TextWriter writer = new StreamWriter(fileName);
                        serializer.Serialize(writer, relieveIntransits);
                        writer.Close();

                        // create flag file
                        writer = new StreamWriter(fileName + "." + aFlagFileSuffix);
                        writer.Close();

                        ++nextFileNumber;
                        ++fileCount;
                        relieveIntransitList.Clear();
                    }
                }

                if (relieveIntransitList.Count > 0)
                {
                    relieveIntransits.RelieveIntransit = new RelieveIntransitsRelieveIntransit[relieveIntransitList.Count];
                    relieveIntransitList.CopyTo(0, relieveIntransits.RelieveIntransit, 0, relieveIntransitList.Count);

                    fileName = GetFileName(aOutputFilePath, aOutputFileName, aFlagFileSuffix, ref nextFileNumber);
                    TextWriter writer = new StreamWriter(fileName);
                    serializer.Serialize(writer, relieveIntransits);
                    writer.Close();

                    // create flag file
                    writer = new StreamWriter(fileName + "." + aFlagFileSuffix);
                    writer.Close();
                    ++fileCount;
                }

                _SAB.ClientServerSession.Audit.GenerateRelieveIntransitAuditInfo_Add(headerCount, fileCount, _totalErrors);

            }
            // Begin TT#644-MD - JSmith - Modify install values for several configuration settings
            //catch
            //{
            //    throw;
            //}
            catch (Exception ex)
            {
                AddException(ex);
                throw;
            }
            // End TT#644-MD - JSmith - Modify install values for several configuration settings
        }

        /// <summary>
        /// Makes sure file name does not exist.
        /// </summary>
        /// <param name="aOutputFilePath">The directory where the files are to be written</param>
        /// <param name="aOutputFileName">The root to use for the file name</param>
        /// <param name="aFlagFileSuffix">The suffix to use for the flag file</param>
        /// <param name="aNextFileNumber">The number used for the last file written </param>
        /// <returns>unique name for the file</returns>
        private string GetFileName(string aOutputFilePath, string aOutputFileName, string aFlagFileSuffix, ref int aNextFileNumber)
        {
            string fileName = null;
            bool fileExists = true;

            while (fileExists)
            {
                if (aNextFileNumber == 1)
                {
                    fileName = aOutputFilePath + aOutputFileName + ".xml";
                }
                else
                {
                    fileName = aOutputFilePath + aOutputFileName + aNextFileNumber + ".xml";
                }

                if (!File.Exists(fileName))
                {
                    if (!File.Exists(fileName + "." + aFlagFileSuffix))
                    {
                        fileExists = false;
                    }
                }
                ++aNextFileNumber;
            }

            return fileName;
        }

        public void AddMessage(string aMessage, eMIDMessageLevel aMIDMessageLevel)
        {
            _SAB.ClientServerSession.Audit.Add_Msg(aMIDMessageLevel, aMessage, this.GetType().Name);
        }

        public void AddException(Exception ex)
        {
            _SAB.ClientServerSession.Audit.Log_Exception(ex, this.GetType().Name);
        }
    }
}
