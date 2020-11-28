using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace MIDRetail.DataCommon
{
    [Serializable()]
    public class ServiceProfile : Profile
    {
        private DateTime _startDateTime;
        private string _version;
        private int _fileMajorPart;
        private int _fileMinorPart;
        private int _fileBuildPart;
        private int _filePrivatePart;

        /// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
        public ServiceProfile(int aKey)
			: base(aKey)
		{

		}

        /// <summary>
        /// Returns the eProfileType of this profile.
        /// </summary>

        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.Service;
            }
        }

        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set { _startDateTime = value; }
        }

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public int FileMajorPart
        {
            get { return _fileMajorPart; }
            set { _fileMajorPart = value; }
        }

        public int FileMinorPart
        {
            get { return _fileMinorPart; }
            set { _fileMinorPart = value; }
        }

        public int FileBuildPart
        {
            get { return _fileBuildPart; }
            set { _fileBuildPart = value; }
        }

        public int FilePrivatePart
        {
            get { return _filePrivatePart; }
            set { _filePrivatePart = value; }
        }

        public override bool Equals(Object aObject)
        {
            // Check for null values and compare run-time types.
            if (aObject == null || GetType() != aObject.GetType())
                return false;

            if (Key != ((ServiceProfile)aObject).Key ||
                // Begin TT#2848 - JSmith - Error creating sessions on StoreLoad.exe
                //StartDateTime.ToString() != ((ServiceProfile)aObject).StartDateTime.ToString() ||
                StartDateTime.Year != ((ServiceProfile)aObject).StartDateTime.Year ||
                StartDateTime.Month != ((ServiceProfile)aObject).StartDateTime.Month ||
                StartDateTime.Day != ((ServiceProfile)aObject).StartDateTime.Day ||
                StartDateTime.Hour != ((ServiceProfile)aObject).StartDateTime.Hour ||
                StartDateTime.Minute != ((ServiceProfile)aObject).StartDateTime.Minute ||
                // End TT#2848 - JSmith - Error creating sessions on StoreLoad.exe
                Version != ((ServiceProfile)aObject).Version) 
            { 
                return false; 
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Used to retrieve a list of service profiles
    /// </summary>
    [Serializable()]
    public class ServicesProfileList : ProfileList
    {
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ServicesProfileList(eProfileType aProfileType)
            : base(aProfileType)
        {

        }
    }

    [Serializable()]
    public class UpgradeProfile : Profile
    {
        private string _upgradeVersion;
        private DateTime _upgradeDateTime;
        private string _upgradeUser;
        private string _upgradeMachine;
        private string _upgradeRemoteMachine;
        private string _upgradeConfiguration;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public UpgradeProfile(int aKey)
            : base(aKey)
        {

        }

        /// <summary>
        /// Returns the eProfileType of this profile.
        /// </summary>

        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.None;
            }
        }

        public DateTime UpgradeDateTime
        {
            get { return _upgradeDateTime; }
            set { _upgradeDateTime = value; }
        }

        public string UpgradeVersion
        {
            get { return _upgradeVersion; }
            set { _upgradeVersion = value; }
        }

        public string UpgradeUser
        {
            get { return _upgradeUser; }
            set { _upgradeUser = value; }
        }

        public string UpgradeMachine
        {
            get { return _upgradeMachine; }
            set { _upgradeMachine = value; }
        }

        public string UpgradeRemoteMachine
        {
            get { return _upgradeRemoteMachine; }
            set { _upgradeRemoteMachine = value; }
        }

        public string UpgradeConfiguration
        {
            get { return _upgradeConfiguration; }
            set { _upgradeConfiguration = value; }
        }


        public override bool Equals(Object aObject)
        {
            // Check for null values and compare run-time types.
            if (aObject == null || GetType() != aObject.GetType())
                return false;

            if (Key != ((UpgradeProfile)aObject).Key ||
                // Begin TT#3005 - JSmith - Environment Verification Error
                //UpgradeDateTime.ToString() != ((UpgradeProfile)aObject).UpgradeDateTime.ToString() ||
                UpgradeDateTime.Year != ((UpgradeProfile)aObject).UpgradeDateTime.Year ||
                UpgradeDateTime.Month != ((UpgradeProfile)aObject).UpgradeDateTime.Month ||
                UpgradeDateTime.Day != ((UpgradeProfile)aObject).UpgradeDateTime.Day ||
                UpgradeDateTime.Hour != ((UpgradeProfile)aObject).UpgradeDateTime.Hour ||
                UpgradeDateTime.Minute != ((UpgradeProfile)aObject).UpgradeDateTime.Minute ||
                // End TT#3005 - JSmith - Environment Verification Error
                UpgradeVersion != ((UpgradeProfile)aObject).UpgradeVersion ||
                UpgradeUser != ((UpgradeProfile)aObject).UpgradeUser ||
                UpgradeMachine != ((UpgradeProfile)aObject).UpgradeMachine ||
                UpgradeConfiguration != ((UpgradeProfile)aObject).UpgradeConfiguration)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [Serializable()]
    public class ClientProfile : Profile
    {
        private string _DBName;
        private string _configuration;
        private string _version;
        private int _fileMajorPart;
        private int _fileMinorPart;
        private int _fileBuildPart;
        private int _filePrivatePart;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ClientProfile(int aKey)
            : base(aKey)
        {

        }

        /// <summary>
        /// Returns the eProfileType of this profile.
        /// </summary>

        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.None;
            }
        }

        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }

        public string Configuration
        {
            get { return _configuration; }
            set { _configuration = value; }
        }

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public int FileMajorPart
        {
            get { return _fileMajorPart; }
            set { _fileMajorPart = value; }
        }

        public int FileMinorPart
        {
            get { return _fileMinorPart; }
            set { _fileMinorPart = value; }
        }

        public int FileBuildPart
        {
            get { return _fileBuildPart; }
            set { _fileBuildPart = value; }
        }

        public int FilePrivatePart
        {
            get { return _filePrivatePart; }
            set { _filePrivatePart = value; }
        }

        public override bool Equals(Object aObject)
        {
            // Check for null values and compare run-time types.
            if (aObject == null || GetType() != aObject.GetType())
                return false;

            if (Key != ((ClientProfile)aObject).Key ||
                DBName != ((ClientProfile)aObject).DBName ||
                Configuration != ((ClientProfile)aObject).Configuration)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
