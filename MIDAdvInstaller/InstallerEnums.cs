using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDRetailInstaller
{
    //error type enumeration
    public enum eErrorType
    {
        error,
        warning,
        message,
        debug
    }

    //workflow enumeration
    public enum eWorkflowType
    {
        // Begin TT#74 MD - JSmith - One-button Upgrade
        upgradeAll,
        // End TT#74 MD
        client,
        server,
        utilities,
        configure,
        rescan
    }

    public enum eEntryType
    {
        None = -1,
        MIDClient = 1,
        MIDControlService = 2,
        MIDStoreService = 3,
        MIDHierarchyService = 4,
        MIDSchedulerService = 5,
        MIDApplicationService = 6,
        MIDAPI = 7,
        MIDConfig = 8
    }

    //install tasks
    public enum eInstallTasks
    {
        install,
        typicalInstall,
        upgrade,
        uninstall,
        setasautoupdate,
        configure,
        databaseMaintenance,
        eventSources,
        crystalReports,
        startServices,
        stopServices,
        installConfiguration,
        scan
    }

    public enum eConfigValueFrom
    {
        Config = 1,
        MIDSettings = 2,
        Default = 3
    }

    public enum eConfigMachineBy
    {
        Name = 1,
        IP = 2
    }

    public enum eConfigType
    {
        None = 0,
        Client = 1,
        Server = 2,
        Config = 3,
        All = 4
    }

    public enum eConfigChangeType
    {
        None = 0,
        Changed = 1,
        Remove = 2
    }

    public enum eShortcutType
    {
        quicklaunch,
        desktop,
        programgroup
    }

    //service list enum
    public enum eAllocationServices
    {
        AllocationApplicationBatch,
        AllocationApplicationService,
        AllocationControlService,
        AllocationHierarchyService,
        AllocationSchedulerService,
        AllocationStoreService
    }

    //Begin TT#883 - JSmith - Desktop Shortcuts do not work on Windows 7
    public enum eOSType
    {
        Unknown,
        Windows95OSR2,
        Windows95,
        Windows98SecondEdition,
        Windows98,
        WindowsMe,
        WindowsNT351,
        WindowsNT40,
        WindowsNT40Server,
        Windows2000,
        WindowsXP,
        WindowsServer2003,
        WindowsVista,
        WindowsServer2008,
        Windows7,
        WindowsServer2008R2,
        Windows8,
        WindowsServer2012,
        WindowsServer2016  // TT#1952-MD - AGallagher - OS 2016 - Installer issues
    }
    //End TT#883

    public enum eCreateShareReturn
    {
        Success = 0,
        AccessDenied = 2,
        UnknownFailure = 8,
        InvalidName = 9,
        InvalidLevel = 10,
        InvalidParameter = 21,
        DuplicateShare = 22,
        RedirectedPath = 23,
        UnknownDeviceOrDirectory = 24,
        NetNameNotFound = 25
    }

}
