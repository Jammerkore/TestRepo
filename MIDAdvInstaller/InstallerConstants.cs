using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDRetailInstaller
{
    class InstallerConstants
    {
        public const string cBackupExtension = ".upgrade";

        // service names
        //public const string cApplicationServiceExecutableName = "MIDAdvancedAllocationApplicationService.exe";
        public const string cApplicationServiceKey = "MIDApplicationService";
        public const string cApplicationServiceName = "Application Service";
        public const string cApplicationServiceArchive = "ApplicationService.zip";
        public const string cApplicationServiceArchive64 = "ApplicationService64.zip";

        //public const string cControlServiceExecutableName = "MIDAdvancedAllocationControlService.exe";
		// Begin TT#1668 - JSmith - Install Log
        public const string cControlServiceExecutableName = "MIDRetailControlService.exe";
		// End TT#1668
        public const string cControlServiceKey = "MIDControlService";
        public const string cControlServiceName = "Control Service";
        public const string cControlServiceArchive = "ControlService.zip";
        public const string cControlServiceArchive64 = "ControlService64.zip";

        //public const string cHierarchyServiceExecutableName = "MIDAdvancedAllocationHierarchyService.exe";
        public const string cHierarchyServiceKey = "MIDHierarchyService";
        public const string cHierarchyServiceName = "Hierarchy Service";
        public const string cHierarchyServiceArchive = "HierarchyService.zip";
        public const string cHierarchyServiceArchive64 = "HierarchyService64.zip";

        //public const string cSchedulerServiceExecutableName = "MIDAdvancedAllocationSchedulerService.exe";
        public const string cSchedulerServiceKey = "MIDSchedulerService";
        public const string cSchedulerServiceName = "Scheduler Service";
        public const string cSchedulerServiceArchive = "SchedulerService.zip";
        public const string cSchedulerServiceArchive64 = "SchedulerService64.zip";

        //public const string cStoreServiceExecutableName = "MIDAdvancedAllocationStoreService.exe";
        public const string cStoreServiceKey = "MIDStoreService";
        public const string cStoreServiceName = "Store Service";
        public const string cStoreServiceArchive = "StoreService.zip";
        public const string cStoreServiceArchive64 = "StoreService64.zip";

        public const string cServicesKey = "MIDServices";

        //public const string cBatchExecutableName = "HierarchyLoad.exe";
        public const string cBatchKey = "MIDAPI";
        public const string cBatchName = "API";
        public const string cBatchArchive = "Batch.zip";
        public const string cBatchArchive64 = "Batch64.zip";

        public const string cClientKey = "MIDClient";
        public const string cClientApp = "MIDRetail.exe";
        public const string cClientRootFolder = "Client";
        public const string cClientRootFolder64 = "Client64";
        public const string cClientFolder = "Client";
        public const string cGraphicsFolder = "Graphics";
        public const string cConfigKey = "MIDConfig";

        // registry constants
        public const string cRegistryRootKey = "MIDRetailInc";
        public const string cRegistrySoftwareKey = "SOFTWARE";
        // Begin TT#4518 - JSmith - Requiring framework 3.5 when should be 4.5.1
        //public const string cRegistryFrameworkKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP";
        public const string cRegistryFrameworkKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
        // End TT#4518 - JSmith - Requiring framework 3.5 when should be 4.5.1
        public const string cRegistryMIDSoftwareKey = "SOFTWARE\\MIDRetailInc";
        //public const string cBusinessObjectsSoftwareKey = "SOFTWARE\\BusinessObjects";
        //public const string cCrystalReportsSoftwareKey = "Crystal Reports";
        public const string cBusinessObjectsSoftwareKey = "SOFTWARE\\SAP BusinessObjects";
        public const string cCrystalReportsSoftwareKey = "Crystal Reports for .NET Framework";
        public const string cProgramsAndFeaturesUninstallKey = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";

        // Crystal Reports constants
        //public const string cCrystalReports32 = "CRRedist2008_x86.msi";
        //public const string cCrystalReports64 = "CRRedist2008_x64.msi";
        //public const string cCrystalReportsViewer = "ReportViewer.exe";
        public const string cCrystalReports32 = "CRRuntime_32bit_13_0_12.msi";
        public const string cCrystalReports64 = "CRRuntime_64bit_13_0_12.msi";

        public const string cUninstallArgument = @"\u";
        public const string cRepairArgument = @"\u";

    }
}
