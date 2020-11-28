using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using System.ServiceModel;
using Logility.ROServices;
using Logility.ROWeb;
using Logility.ROWebCommon;

namespace MIDRetail.Service.Jobs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class JobService : ServiceBase
    {
        /// <summary>
        /// 
        /// </summary>
        public JobService()
        {
            InitializeComponent();
        }

        private ServiceHost _host;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                if (!EventLog.SourceExists("MIDJobService"))
                {
                    EventLog.CreateEventSource("MIDJobService", null);
                }

                // check environment
                string message = string.Empty;
                //if (!MIDEnvironment.ValidEnvironment(out message))
                //{
                //    EventLog.WriteEntry("MIDJobService", message, EventLogEntryType.Error);
                //    return;
                //}

                // Make sure all previous RO Web Hosts have been stopped.
                ROWebManager ROWebManager = new Logility.ROWeb.ROWebManager(new ROWebTools());
                ROWebManager.KillAllROWebHost("Make sure all previous RO Web Hosts have been stopped.");

                // Service Started...
                EventLog.WriteEntry("MIDJobService", "MIDJobService started successfully.", EventLogEntryType.Information);

                //Host the WCF service
                _host = new ServiceHost(typeof(ROWebJobService));
                _host.Open();

                EventLog.WriteEntry("MIDJobService", "MIDJobService hosting WCF service.", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDJobService", ex.ToString(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                // Make sure all RO Web Hosts have been stopped.
                ROWebManager ROWebManager = new Logility.ROWeb.ROWebManager(new ROWebTools());
                ROWebManager.KillAllROWebHost("Stopping RO Web Job Service");

                if (_host.State != CommunicationState.Closed)
                {
                    _host.Close();
                }
            }
            catch(Exception ex)
            {
                EventLog.WriteEntry("MIDJobService", "Error on stop: " + ex.ToString(), EventLogEntryType.Error);
            }
        }
    }
}
