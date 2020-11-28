using System;

namespace MIDRetail.MIDAdvInstaller {
	/// <summary>
	/// Defines the possible Execution Paths
	/// </summary>
	public enum eExecPath {
		Install,
        Repair,
		Upgrade,
        Remove,
        Configure,
        Copy,
        AutoUpgrade,
        DBMaint,
        Register
	}

	[Flags()]
	public enum eOptionType : short {
		Client = 1,
		Services = 2,
		API = 4
	}

	/// <summary>
	/// defines the different types of configs that are possible
	/// </summary>
	public enum eConfigType {
		String,
		Boolean,
		ConnectionString
	}

    /// <summary>
    /// defines the special types of a node
    /// </summary>
    public enum eSpecialNodeType
    {
        Undefined,
        AutoUpgrade,
        ClientConfig,
        ServerConfig,
        ConfigGroup,
        ServicesGroup,
        ClientGroup
    }

    /// <summary>
    /// defines the installer used
    /// </summary>
    public enum eInstallerType
    {
        Undefined,
        Windows,
        MID
    }

	/// <summary>
	/// the different status codes that are defined
	/// </summary>
	public enum eStatus: int {
		RemovingInstalledServices = 0,
		RemovingFiles = 1,
		SavingXML = 2,
		RepairingServices = 3,
		InstallingFiles = 4,
		InstallingServices = 5
	}

	/// <summary>
	/// the different panels that are available to be displayed.
	/// </summary>
	public enum eInstallPanel: int {
		Intro = 0,
        FirstUse = 1,
		AddRemoveRepair = 2,
		ChooseExisting = 3,
		Location = 4,
		ModuleSelector = 5,
		ServicesSelector = 6,
		ProgressStatus = 7,
		Finished = 8,
		ModifyConfigValues = 9,
        DBMaintenance = 10,
        Copy = 11,
        AutoUpgradeClient = 12,
        ClientSettings = 13
	}

    /// <summary>
    /// the different processes that can be installed.
    /// </summary>
    public enum eProcessType : int
    {
        Undefined = 0,
        Client = 1,
        ControlService = 2,
        StoreService = 3,
        MerchandiseService = 4,
        SchedulerService = 5,
        ApplicationService = 6,
        APIs = 7
    }

    /// <summary>
    /// the stage of the first time process.
    /// </summary>
    public enum eFirstTimeStage
    {
        Clean,
        Search,
        Register
    }

    /// <summary>
    /// the type of configuration file.
    /// </summary>
    public enum eConfigFileType
    {
        Undefined,
        Server,
        Client,
        API
    }

    //Warning: Do not change the values below.  They are used in the registry to identify the data.
    /// <summary>
    /// the different processes that can be installed.
    /// </summary>
    public enum eRegistryEntryType : int
    {
        Undefined = 0,
        Client = 1,
        ControlService = 2,
        StoreService = 3,
        MerchandiseService = 4,
        SchedulerService = 5,
        ApplicationService = 6,
        APIs = 7,
        MIDSettings = 8
    }
}