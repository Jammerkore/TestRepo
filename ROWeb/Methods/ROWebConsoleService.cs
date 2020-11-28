using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;

using Logility.ROWeb;
using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ROWebConsoleService" in both code and config file together.
    public class ROWebConsoleService : IROWebConsoleService, IDisposable
    {
        private SessionAddressBlock _SAB = null;
        private ROWebTools _ROWebTools;
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        private bool _enhancedLogging = false;
        private List<string> _outputList = new List<string>();

        private Dictionary<long, ROWebFunction> _dictROFunctions = new Dictionary<long, ROWebFunction>();
        private long _lMIDGlobalDefaultsID = ROWebFunction.InvalidInstanceID;
        private DateTime _keepAliveTimestamp = DateTime.Now; // TT#1156-MD CTeegarden Add KeepAlive Functionality

        private ArrayList _alLock = new ArrayList(); //setup the lock;

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">Flag identifying if dispose is in progress</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                CleanUp();
                handle.Dispose();
            }

            disposed = true;
        }

        //===========
        // PROPERTIES
        //===========
        private ROWebTools ROWebTools
        {
            get
            {
                if (_ROWebTools == null)
                {
                    _ROWebTools = new ROWebTools();
                    _ROWebTools.CreateLog();
                }
                return _ROWebTools;
            }
        }

        /// <summary>
        /// Return the instance of the ROFunctions
        /// </summary>
        private Dictionary<long, ROWebFunction> ROFunctions
        {
            get
            {
                return _dictROFunctions;
            }
        }

        //========
        // METHODS
        //========

        /// <summary>
        /// Free all managed objects
        /// </summary>
        private void CleanUp()
        {
            _lMIDGlobalDefaultsID = ROWebFunction.InvalidInstanceID;
            try
            {
                foreach (ROWebFunction ROFunction in ROFunctions.Values)
                {
                    ROFunction.CleanUp();
                }

                if (_SAB != null)
                {
                    _SAB.CloseSessions();
                    _SAB = null;
                }
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error during CleanUp..." + ex.ToString() + ex.StackTrace);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
        }

        /// <summary>
        /// Add an ROWebFunction to the collection
        /// </summary>
        /// <param name="ROWebFunction">The function to add</param>
        private void AddFunction(ROWebFunction ROWebFunction)
        {
            lock (_alLock.SyncRoot)
            {
                ROFunctions.Add(ROWebFunction.ROInstanceID, ROWebFunction);
            }
        }

        /// <summary>
        /// Retrieved the ROWebFunction associated with the instance ID
        /// </summary>
        /// <param name="lInstanceID">The instance ID of the function</param>
        /// <returns></returns>
        private ROWebFunction GetFunction(long lInstanceID)
        {
            lock (_alLock.SyncRoot)
            {
                ROWebFunction wf = null;

                if (!ROFunctions.TryGetValue(lInstanceID, out wf))
                {
                    //ROWebTools.LogMessage(eROMessageLevel.Error, "Function ID " + lInstanceID + " not found");
                    //throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
                }

                return wf;
            }
        }

        /// <summary>
        /// Removed the instance of the function from the collection
        /// </summary>
        /// <param name="lInstanceID">The ID of the instance of the function</param>
        /// <returns></returns>
        private ROOut RemoveFunction(long lInstanceID)
        {
            lock (_alLock.SyncRoot)
            {
                if (ROFunctions.ContainsKey(lInstanceID))
                {
                    ROWebFunction webFunction = GetFunction(lInstanceID);
                    webFunction.CleanUp();
                    ROFunctions.Remove(lInstanceID);
                    ROWebTools.LogMessage(eROMessageLevel.Information, "Instance " + lInstanceID + " successfully removed.");
                    return new RONoDataOut(eROReturnCode.Successful, null, lInstanceID);
                }
                else
                {
                    ROWebTools.LogMessage(eROMessageLevel.Warning, "Instance " + lInstanceID + " not found.");
                    return new RONoDataOut(eROReturnCode.Failure, "Instance " + lInstanceID + " not found.", lInstanceID);
                }
            }
        }

        /// <summary>
        /// Notifies the service that the client is still alive. Throws a fault exception on failure
        /// </summary>
        /// 
        public bool KeepAlive()
        {
            DateTime now = DateTime.Now;
            long intervalInMillis = (now.Ticks - _keepAliveTimestamp.Ticks) / TimeSpan.TicksPerMillisecond;

            _keepAliveTimestamp = now;

            return true;
        }

        /// <summary>
        /// Creates the user session area 
        /// </summary>
        /// <param name="sROUserID">The ID of the user</param>
        /// <param name="sPassword">Password</param>
        /// <param name="sROSessionID">The name of the Session</param>
        /// <param name="processDescription">Output messages during process</param>
        /// <returns>A value indicating if a session was successfully created</returns>
        public eROConnectionStatus ConnectToServices(string sROUserID, string sPassword, string sROSessionID, out string processDescription)
        {
            eROConnectionStatus connectionStatus = eROConnectionStatus.Failed;
            processDescription = null;
            try
            {
                ROWebTools.ROUserID = sROUserID;
                ROWebTools.ROSessionID = sROSessionID;

                MIDRetailAPI api = new MIDRetailAPI();

                processDescription = null;
                _SAB = api.CreateSessionAddressBlock(sROUserID, sPassword, ref processDescription, ref connectionStatus);

                if (_SAB == null)
                {
                    ROWebTools.LogMessage(eROMessageLevel.Error, "Connectaion failed: " + processDescription);
                    //connectionStatus = eROConnectionStatus.Failed;
                }
                else
                {
                    //connectionStatus = eROConnectionStatus.Successful;
                    ROWebTools.LogMessage(eROMessageLevel.Information, "Connection established.");
                }

                if (ROWebTools.LogDebugEnabled)
                {
                    string parmStr = MIDConfigurationManager.AppSettings["MIDOnlyFunctions"];
                    if (parmStr != null)
                    {
                        parmStr = parmStr.ToLower();
                        if (parmStr == "true" || parmStr == "yes" || parmStr == "t" || parmStr == "y" || parmStr == "1")
                        {
                            _enhancedLogging = true;
                        }
                    }

                    parmStr = MIDConfigurationManager.AppSettings["LogOutputList"];
                    if (parmStr != null)
                    {
                        string[] entries = parmStr.Split('|');
                        foreach (string entry in entries)
                        {
                            if (entry.Trim().Length > 0)
                            {
                                _outputList.Add(entry.Trim());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                connectionStatus = eROConnectionStatus.Failed;
                processDescription = processDescription + ex.ToString() + ex.StackTrace;
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error during ConnectSession..." + ex.ToString() + ex.StackTrace);
            }

            return connectionStatus;
        }

        /// <summary>
        /// Closes the sessions for the user in the Session
        /// </summary>
        public void DisconnectSession()
        {
            try
            {
                ROWebTools.LogMessage(eROMessageLevel.Information, "Disconnecting session.");
                if (_SAB != null)
                {
                    _SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
                }
                CleanUp();
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error in DisconnectSession..." + ex.ToString());
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
            Environment.Exit(0);
        }

        /// <summary>
        /// Verifies the sessions for the user have been created
        /// </summary>
        private void VerifyConnection()
        {
            if (_SAB == null)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "SessionAddressBlock is null", ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
        }

        /// <summary>
        /// Insures the RO Host is valid and associated with the user and session
        /// </summary>
        /// <param name="sROUserID">The RO User</param>
        /// <param name="sROSessionID">The RO Sesson</param>
        /// <returns>Flag identifying if the environment is correct</returns>
        private bool VerifyEnvironment (string sROUserID, string sROSessionID)
        {
            ROWebTools.LogMessage(eROMessageLevel.Debug, "Verifying environment with user " + ROWebTools.ROUserID + " and session " + ROWebTools.ROSessionID, sROUserID, sROSessionID);
            if (sROUserID != ROWebTools.ROUserID
                || sROSessionID != ROWebTools.ROSessionID)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Invalid environment with user " + ROWebTools.ROUserID + " and session " + ROWebTools.ROSessionID, sROUserID, sROSessionID);
                return false;
            }

            VerifyConnection();

            return true;
        }

        /// <summary>
        /// Gets information about the servic connections
        /// </summary>
        /// <param name="appSetControlServer">The location of the control server</param>
        /// <param name="appSetLocalStoreServer">The location of the store server</param>
        /// <param name="appSetLocalHierarchyServer">The location of the hierarchy server</param>
        /// <param name="appSetLocalApplicationServer">The location of the application server</param>
        public void GetServiceServerInfo(out string appSetControlServer, out string appSetLocalStoreServer, out string appSetLocalHierarchyServer, out string appSetLocalApplicationServer)
        {
            try
            {
                _SAB.GetServiceServerInfo(out appSetControlServer, out appSetLocalStoreServer, out appSetLocalHierarchyServer, out appSetLocalApplicationServer);
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error getting service server info..." + ex.ToString() + ex.StackTrace);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
        }

        /// <summary>
        /// Processes the requested function
        /// </summary>
        /// <param name="Parms">parameter that needs to passed into funtion being called</param>
        /// <returns>returns called function return value</returns>
        public ROOut ProcessRequest(ROParms Parms)
        {
            DateTime startTime = DateTime.Now;
            try
            {
                
                if (VerifyEnvironment(Parms.ROUserID, Parms.ROSessionID))
                {
                    ROWebTools.LogMessage(eROMessageLevel.Debug, "ProcessRequest " + Parms.ROClass.ToString() + " and request " + Parms.RORequest.ToString(), Parms.ROUserID, Parms.ROSessionID);

                    if (_enhancedLogging)
                    {
                        LogInputClass(Parms);
                    }

                    MIDEnvironment.ResetFields();
                    if (Parms.RORequest == eRORequest.RemoveInstance)
                    {
                        return RemoveFunction(Parms.ROInstanceID);
                    }
                    
                    if (Parms.RORequest == eRORequest.GetActiveClassesAndInstances)
                    {
                        return GetActiveClassesAndInstances(Parms);
                    }
                    else
                    {
                        ROWebFunction webFunction = GetFunction(Parms);
                        return VerifyOutput(Parms, webFunction.ProcessRequest(Parms));
                    }
                }

                return new RONoDataOut(eROReturnCode.Failure, "Unable to verify environment", Parms.ROInstanceID);
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "Error processing service data..." + ex.ToString() + ex.StackTrace);
                throw new FaultException(ROWebTools.GetExceptionReason(), new FaultCode(ROWebTools.GetExceptionCode()));
            }
            finally
            {
                if (_enhancedLogging)
                {
                    TimeSpan duration = DateTime.Now.Subtract(startTime);
                    string strDuration = Convert.ToString(duration);
                    ROWebTools.LogMessage(eROMessageLevel.Debug, "Response:ProcessRequest " + Parms.ROClass.ToString() + " and request " + Parms.RORequest.ToString() + "; Duration:" + strDuration, Parms.ROUserID, Parms.ROSessionID);
                }
            }
        }

        private void LogInputClass(ROParms Parms)
        {
            if (_enhancedLogging)
            {
                Type myType = Parms.GetType();
                string line = Environment.NewLine + Environment.NewLine;
                string sJSONResponse = JsonConvert.SerializeObject(Parms);
                line += Deserialize(sJSONResponse);
                ROWebTools.LogMessage(eROMessageLevel.Debug, myType.ToString() + " Input Values: " + line, Parms.ROUserID, Parms.ROSessionID);
            }
        }

        private void LogOutputClass(ROOut Parms, string requestName)
        {
            if (_outputList.Count == 0)
            {
                return;
            }

            if (_enhancedLogging)
            {
                Type myType = Parms.GetType();
                bool include = false;
                foreach (string entry in _outputList)
                {
                    if (entry == "*")
                    {
                        include = true;
                        break;
                    }
                    else if (Regex.IsMatch(requestName, entry.Replace("*", ".*?")))  // check if matches the request
                    {
                        include = true;
                        break;
                    }
                    else if (Regex.IsMatch(myType.Name, entry.Replace("*", ".*?")))  // check if matches the class
                    {
                        include = true;
                        break;
                    }
                }
                if (!include)
                {
                    return;
                }
                string line = Environment.NewLine + Environment.NewLine;
                string sJSONResponse = JsonConvert.SerializeObject(Parms);
                line += Deserialize(sJSONResponse);
                ROWebTools.LogMessage(eROMessageLevel.Debug, myType.ToString() + " Output Values: " + line);
            }
        }

        private string Deserialize(string jsonRawString)
        {
            JToken parsedJson = JToken.Parse(jsonRawString);
            var beautified = parsedJson.ToString(Formatting.Indented);
            //var minified = parsedJson.ToString(Formatting.None);

            return beautified;
        }

        private ROOut GetActiveClassesAndInstances(ROParms Parms)
        {
            ROIListOut ROActiveClassList = new ROIListOut(eROReturnCode.Successful, null, Parms.ROInstanceID, BuildActiveClassList((ROListParms)Parms));
            LogOutputClass(ROActiveClassList, Parms.RORequest.ToString());
            return ROActiveClassList;
        }
        private List<ROActiveClass> BuildActiveClassList(ROListParms listParms)
        {
            ROActiveClass activeClass;
            List<ROActiveClass> activeClasses = new List<ROActiveClass>(); 
            foreach (ROWebFunction webFunction in ROFunctions.Values)
            {
                if (listParms.ListValues.Contains(webFunction.ROClass)
                    || listParms.ListValues.Count == 0)
                {
                    activeClass = GetActiveClass(webFunction.ROClass, activeClasses);
                    activeClass.InstanceIDs.Add(webFunction.ROInstanceID);
                }
            }
            return activeClasses;
        }

        private ROActiveClass GetActiveClass(eROClass roClass, List<ROActiveClass> activeClasses)
        {
            ROActiveClass activeClass = activeClasses.Find(X => X.ActiveClass == roClass);

            if (activeClass == null)
            {
                activeClass = new ROActiveClass(roClass);
                activeClasses.Add(activeClass);
            }
            return activeClass;

        }

        /// <summary>
        /// Make sure output object has fields needed for serialization
        /// </summary>
        /// <param name="Parms">Input Parms</param>
        /// <param name="OutParms">Output Parms</param>
        /// <returns>Updated ROOut object</returns>
        private ROOut VerifyOutput(ROParms Parms, ROOut OutParms)
        {
            if (_enhancedLogging)
            {
                ROWebTools.LogMessage(eROMessageLevel.Debug, "Response: " + OutParms.ROReturnCode +  " : Instance:" + OutParms.ROInstanceID, Parms.ROUserID, Parms.ROSessionID);
            }

            if (OutParms is RODataTableOut)
            {
                RODataTableOut dtOut = (RODataTableOut)OutParms;
                if (string.IsNullOrEmpty(dtOut.dtOutput.TableName))
                {
                    dtOut.dtOutput.TableName = Parms.RORequest.ToString();
                }
                OutParms = dtOut;
            }
            else if (OutParms is RODataSetOut)
            {
                RODataSetOut dsOut = (RODataSetOut)OutParms;
                if (string.IsNullOrEmpty(dsOut.dsOutput.DataSetName))
                {
                    dsOut.dsOutput.DataSetName = Parms.RORequest.ToString();
                }
                int tableCount = 0;
                foreach (DataTable dt in dsOut.dsOutput.Tables)
                {
                    if (string.IsNullOrEmpty(dt.TableName))
                    {
                        ++tableCount;
                        dt.TableName = Parms.RORequest.ToString() + tableCount;
                    }
                }
                OutParms = dsOut;
            }

            LogOutputClass(OutParms, Parms.RORequest.ToString());
            
            return OutParms;
        }

        /// <summary>
        /// Retrieves and instance of an ROWebFunction associated with the ROClass
        /// </summary>
        /// <param name="Parms">The parameters needed for fulfill the request</param>
        /// <returns>instance of ROWebFunction</returns>
        private ROWebFunction GetFunction (ROParms Parms)
        {
            ROWebFunction ROWebFunction = null;
            ROWebFunction = GetFunction(Parms.ROInstanceID);
            if (ROWebFunction == null)
            {
                Type type = Type.GetType("Logility.ROWeb." + Parms.ROClass.ToString());
                object[] args = new object[2];
                args[0] = _SAB;
                args[1] = ROWebTools;
                lock (_alLock.SyncRoot)   // alan temp code to add lock
                {  // alan temp code to add lock
                    System.Threading.Thread.Sleep(100);   // alan temp code to add code sleep to one-tenth of a second
                    ROWebFunction = (ROWebFunction)Activator.CreateInstance(type, args);
                    ROWebFunction.ROClass = Parms.ROClass;
                    AddFunction(ROWebFunction);
                }  // alan temp code to add lock
            } 
            return ROWebFunction;
        }
    }
}
