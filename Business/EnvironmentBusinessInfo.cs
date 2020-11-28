using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
    public static class EnvironmentBusinessInfo
    {

        //public static MIDInfoSupport MIDInfo = new MIDInfoSupport();  //Use this instance to get data about the MID Retail application

        //public class MIDInfoSupport
        //{
        //}


        public static string GetAllocationInstalledInfo(SessionAddressBlock SAB)
        {
            return GetAddOnInfo(eMIDTextCode.lbl_Allocation, SAB.ClientServerSession.GlobalOptions.AppConfig.AllocationInstalled,
                    SAB.ClientServerSession.GlobalOptions.AppConfig.AllocationTempLicense, SAB.ClientServerSession.GlobalOptions.AppConfig.AllocationExpirationDays);
        }
        public static string GetSizeInstalledInfo(SessionAddressBlock SAB)
        {
            return GetAddOnInfo(eMIDTextCode.lbl_Size, SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled,
                SAB.ClientServerSession.GlobalOptions.AppConfig.SizeTempLicense, SAB.ClientServerSession.GlobalOptions.AppConfig.SizeExpirationDays);
        }
        public static string GetPlanningInstalledInfo(SessionAddressBlock SAB)
        {
            return GetAddOnInfo(eMIDTextCode.lbl_Planning, SAB.ClientServerSession.GlobalOptions.AppConfig.PlanningInstalled,
                            SAB.ClientServerSession.GlobalOptions.AppConfig.PlanningTempLicense, SAB.ClientServerSession.GlobalOptions.AppConfig.PlanningExpirationDays);
        }
        // Begin TT#862 - MD - stodd - Assortment Upgrade Issues
        public static string GetAssortmentInstalledInfo(SessionAddressBlock SAB)
        {
            return GetAddOnInfo(eMIDTextCode.lbl_Assortment, SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled,
                            SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentTempLicense, SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentExpirationDays);
        }
        // End TT#862 - MD - stodd - Assortment Upgrade Issues
        // Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
        public static string GetGroupAllocationInstalledInfo(SessionAddressBlock SAB)
        {
            return GetAddOnInfo(eMIDTextCode.lbl_GroupAllocation, SAB.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationInstalled,
                            SAB.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationTempLicense, SAB.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationExpirationDays);
        }
        // End TT#1247-MD - stodd - Add Group Allocation as a License Key option -
        

        private static string GetAddOnInfo(eMIDTextCode aApplication, bool aApplicationInstalled, bool aTempLicense, int aExpireDays)
        {
                StringBuilder text;
                string installed = MIDText.GetTextOnly(eMIDTextCode.msg_IsInstalled);
                string notInstalled = MIDText.GetTextOnly(eMIDTextCode.msg_IsNotInstalled);
                string tempInstalled = MIDText.GetTextOnly(eMIDTextCode.msg_IsTempInstalled);
                if (aTempLicense)
                {
                    text = new StringBuilder(tempInstalled);
                    text.Replace("{1}", aExpireDays.ToString());
                }
                else if (aApplicationInstalled)
                {
                    text = new StringBuilder(installed);
                }
                else
                {
                    text = new StringBuilder(notInstalled);
                }
                text.Replace("{0}", MIDText.GetTextOnly(aApplication));
                return text.ToString();
          
        }
        public static string ConfigurationLabelText
        {
            get {return MIDText.GetTextOnly(eMIDTextCode.lbl_Configuration); }
        }

        public static string AddOnLabelText
        {
            get { return MIDText.GetTextOnly(eMIDTextCode.lbl_AddOns); }
        }

        public static string GetSessionEnviroment(SessionAddressBlock SAB)
        {
            return SAB.ControlServerSession.GetMIDEnvironment();
        }
  
        public static string GetClientDatbaseName(SessionAddressBlock SAB)
        {
            return SAB.ClientServerSession.GetDatabaseName();
        }
        public static string GetClientApplicationUserName(SessionAddressBlock SAB)
        {
            return SAB.ClientServerSession.GetUserName(SAB.ClientServerSession.UserRID);
        }
        public static string GetControlServerSessionInfo(SessionAddressBlock SAB)
        {
            return GetServerInformation(SAB.ControlServerSession, "Control", SAB.ControlServer);
        }
        public static string GetApplicationServerSessionInfo(SessionAddressBlock SAB)
        {
            return GetServerInformation(SAB.ApplicationServerSession, "Application", SAB.ServerGroup.ApplicationServer);
        }
        public static string GetHierarchyServerSessionInfo(SessionAddressBlock SAB)
        {
            return GetServerInformation(SAB.HierarchyServerSession, "Hierarchy", SAB.ServerGroup.HierarchyServer);
        }
        public static string GetSchedulerServerSessionInfo(SessionAddressBlock SAB)
        {
            return GetServerInformation(SAB.SchedulerServerSession, "Scheduler", SAB.ServerGroup.SchedulerServer);
        }
        public static string GetStoreServerSessionInfo(SessionAddressBlock SAB)
        {
            return GetServerInformation(SAB.StoreServerSession, "Store", SAB.ServerGroup.StoreServer);
        }

        private static string GetServerInformation(Session aSession, string aSessionName, string aControlServerServerGroup)
        {
                if (aSession == null)
                {
                    return aSessionName + " Server is not defined";
                }
                else
                    //Begin TT#708 - JScott - Services need a Retry availalbe.
                    //if (!RemotingServices.IsTransparentProxy(aSession))
                    if (aSession.isSessionRunningLocal)
                    //End TT#708 - JScott - Services need a Retry availalbe.
                    {
                        return aSessionName + " Server is running local";
                    }
                    else
                    {
                        int lastIndex = aControlServerServerGroup.LastIndexOf(":");
                        string portNumber = aControlServerServerGroup.Substring(lastIndex, aControlServerServerGroup.Length - lastIndex);
                        return aSessionName + " Server(" + aSession.GetProductVersion() + ") is running remote on " + aSession.GetMachineName() + "(" + aSession.GetIPAddress() + ")" + portNumber;
                    }

        }

        /// <summary>
        /// Get all business environmental information in a single string, separated by the newline string parameter.
        /// Used for system emails.
        /// </summary>
        /// <param name="SAB"></param>
        /// <param name="newline"></param>
        /// <returns></returns>
        public static string GetAllBusinessEnvironmentInfo(SessionAddressBlock SAB, string newline)
        {
            string s = string.Empty;

            //Configuration Info
            s += ConfigurationLabelText + ": " + newline;
            s += "Database=" + GetClientDatbaseName(SAB) + newline;
            s += "Application User=" + GetClientApplicationUserName(SAB) + newline;

            s += GetControlServerSessionInfo(SAB) + newline;
            //s += GetApplicationServerSessionInfo(SAB) + newline;  // TT#3434 - JSmith - Index error generating email message from service
            s += GetHierarchyServerSessionInfo(SAB) + newline;
            s += GetSchedulerServerSessionInfo(SAB) + newline;
            s += GetStoreServerSessionInfo(SAB) + newline;

            //Add On Info
            s += newline + EnvironmentBusinessInfo.AddOnLabelText + ": " + newline;
            s += GetAllocationInstalledInfo(SAB) + newline;
            s += GetSizeInstalledInfo(SAB) + newline;
            s += GetPlanningInstalledInfo(SAB) + newline;
            s += GetAssortmentInstalledInfo(SAB) + newline;

            return s;
        }

    }
}
