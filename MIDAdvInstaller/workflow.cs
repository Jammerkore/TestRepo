using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDRetailInstaller
{
    class workflow
    {
        //class variables
        ucScan scan_pane = null;
        ucInstallChooser install_chooser_pane = null;
        eWorkflowType wtWorkflow;
        ucClient client_pane = null;
        ucServer server_pane = null;
        ucUtilities utilities_pane = null;
        ucConfig config_pane = null;
        DateTime dtClient = DateTime.Now;
        DateTime dtServer = DateTime.Now;
        DateTime dtConfig = DateTime.Now;
        DateTime dtChooser = DateTime.Now;
        DateTime dtScan = DateTime.Now;
        DateTime dtUtilities = DateTime.Now;

        public eWorkflowType Workflow
        {
            get
            {
                return wtWorkflow;
            }
            set
            {
                wtWorkflow = value;
            }
        }

        //scan object
        public ucScan ScanPane
        {
            get
            {
                return scan_pane;
            }
            set
            {
                dtScan = DateTime.Now;
                scan_pane = value;
            }
        }


        //time stamp the panel loads
        public DateTime ScanLoad
        {
            get {return dtScan;}
        }
        public DateTime InstallChooserLoad
        {
            get {return dtChooser;}
        }
        public DateTime ClientLoad
        {
            get{return dtClient;}
        }
        public DateTime ServerLoad
        {
            get{return dtServer;}
        }
        public DateTime ConfigLoad
        {
            get { return dtConfig; }
        }

        //install chooser object
        public ucInstallChooser InstallChooserPane
        {
            get
            {
                return install_chooser_pane;
            }
            set
            {
                dtChooser = DateTime.Now;
                install_chooser_pane = value;
            }
        }

        //client object
        public ucClient ClientPane
        {
            get
            {
                return client_pane;
            }
            set
            {
                dtClient = DateTime.Now;
                client_pane = value;
            }
        }

        //server object
        public ucServer ServerPane
        {
            get
            {
                return server_pane;
            }
            set
            {
                dtServer = DateTime.Now;
                server_pane = value;
            }
        }

        //client object
        public ucUtilities UtilitiesPane
        {
            get
            {
                return utilities_pane;
            }
            set
            {
                dtUtilities = DateTime.Now;
                utilities_pane = value;
            }
        }

        //config object
        public ucConfig ConfigurationPane
        {
            get
            {
                return config_pane;
            }
            set
            {
                dtConfig = DateTime.Now;
                config_pane = value;
            }
        }
    }
}
