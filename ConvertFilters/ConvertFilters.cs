using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
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

//Begin TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)
namespace ConvertFilters
{
    class ConvertFilters
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            ConvertFiltersWorker cw = new ConvertFiltersWorker();
            return cw.StartConversion(args);
        }
        public class ConvertFiltersWorker
        {
            private Colors _colors = null;
            string sourceModule = "ConvertFilters.cs";
            string eventLogID = "MIDConvertFilters";
            string message = null;
            SessionAddressBlock _SAB;
            SessionSponsor _sponsor;
            IMessageCallback _messageCallback;
            Dictionary<string, int> _colorIDHash;
            int _recordsRead = 0;
            int _addRecords = 0;
            int _updateRecords = 0;
            int _recordsWithErrors = 0;
            eMIDMessageLevel highestMessage;

            string fileLocation = null;
            char[] delimiter = { '~' };
            bool errorFound = false;
            System.Runtime.Remoting.Channels.IChannel channel;
        
            int _processId = Include.NoRID;
        

            public int StartConversion(string[] args)
            {

                try
                {
                    _messageCallback = new BatchMessageCallback();
                    _sponsor = new SessionSponsor();
                    _SAB = new SessionAddressBlock(_messageCallback, _sponsor);

                    if (!EventLog.SourceExists(eventLogID))
                    {
                        EventLog.CreateEventSource(eventLogID, null);
                    }

                    // Register callback channel

                    try
                    {
                        channel = _SAB.OpenCallbackChannel();
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + ex.Message, EventLogEntryType.Error);
                        return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
                    }

                    // Create Sessions

                    try
                    {
                        _SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Hierarchy);
                        _SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Store);
                        _SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Application);
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
                        return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
                    }

                    eSecurityAuthenticate authentication = _SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
                        MIDConfigurationManager.AppSettings["Password"], eProcesses.convertFilters);
                    if (authentication != eSecurityAuthenticate.UserAuthenticated)
                    {
                        EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
                        System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
                        return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
                    }

               
                    _SAB.ClientServerSession.Initialize();
                    _SAB.HierarchyServerSession.Initialize();
                    _SAB.StoreServerSession.Initialize();
                    _SAB.ApplicationServerSession.Initialize();

                    //Save Store Filter XRef
                    ProfileList variableProfList;
                    ProfileList timeTotalVariableProfList;
                    ProfileList versionProfList;
                    variableProfList = _SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;
                    timeTotalVariableProfList = _SAB.ApplicationServerSession.DefaultPlanComputations.PlanTimeTotalVariables.TimeTotalVariableProfileList;
                    versionProfList = _SAB.ClientServerSession.GetUserForecastVersions();

                    StoreFilterData storeFilterDL = new StoreFilterData();
                    System.Data.DataTable dtStoreFilters = storeFilterDL.StoreFilter_ReadAll();
                 
                    foreach (System.Data.DataRow row in dtStoreFilters.Rows)
                    {
                        int storeFilterRID = (int)row["STORE_FILTER_RID"];
                        try
                        {
                            StoreFilterDefinition filterDef = new StoreFilterDefinition(_SAB, _SAB.ClientServerSession, storeFilterDL, null, versionProfList, variableProfList, timeTotalVariableProfList, storeFilterRID);
                            //filters have to be "redrawn" for the operand list to match what is really saved.
                            filterDef.RebuildOperandsWithNoUI(); 

                            filterDef.SaveFilterXRefOnly(storeFilterRID); 
                        }
                        catch(MIDRetail.Business.FilterSyntaxErrorException ex)
                        {
                            //Suppress Filter Syntax errors
                        }
                    }


                    //Save Product Filter XRef
                    ProductFilterData productFilterDL = new ProductFilterData();
                    System.Data.DataTable dtProductFilters = productFilterDL.ProductSearchObject_ReadIDs();
                    foreach (System.Data.DataRow row in dtProductFilters.Rows)
                    {
                        int productFilterRID = (int)row["USER_RID"];
                        try
                        {
                            ProductSearchFilterDefinition filterDef = new ProductSearchFilterDefinition(_SAB, _SAB.ClientServerSession, productFilterDL, null, productFilterRID);
                            //filters have to be "redrawn" for the operand list to match what is really saved.
                            filterDef.RebuildOperandsWithNoUI();

                            filterDef.SaveFilterXRefOnly(productFilterRID);
                        }
                        catch (MIDRetail.Business.FilterSyntaxErrorException ex)
                        {
                            //Suppress Filter Syntax errors
                        }
                    }

                }



                catch (Exception ex)
                {
                    errorFound = true;
                    message = "";
                    while (ex != null)
                    {
                        message += " -- " + ex.Message;
                        ex = ex.InnerException;
                    }
                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
                }
                finally
                {
                    if (!errorFound)
                    {
                        if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
                        {
                            _SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
                           // _SAB.ClientServerSession.Audit.ColorCodeLoadAuditInfo_Add(_recordsRead, _recordsWithErrors, _addRecords, _updateRecords);
                        }
                    }
                    else
                    {
                        if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
                        {
                            _SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                           // _SAB.ClientServerSession.Audit.ColorCodeLoadAuditInfo_Add(_recordsRead, _recordsWithErrors, _addRecords, _updateRecords);
                        }
                    }

                    highestMessage = _SAB.CloseSessions();
                }

                return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
            }

        }

    }
}
//End TT#252 -MD - JSobek - Filter Cross-Reference (for In Use Tool)