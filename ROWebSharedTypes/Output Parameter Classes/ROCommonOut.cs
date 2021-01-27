using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using MIDRetail.DataCommon;

namespace Logility.ROWebSharedTypes
{

    [DataContract(Name = "ROTreeNodeOut", Namespace = "http://Logility.ROWeb/")]
    public class ROTreeNodeOut
    {
        #region "DataMember"
        //=======
        // FIELDS
        //=======
        [DataMember(IsRequired = true)]
        private int _key;
        [DataMember(IsRequired = true)]
        private string _text;
        [DataMember(IsRequired = true)]
        private int _ownerUserRID;
        [DataMember(IsRequired = true)]
        private eTreeNodeType _treeNodeType;
        [DataMember(IsRequired = true)]
        private eProfileType _profileType;
        [DataMember(IsRequired = true)]
        private bool _isReadOnly;
        [DataMember(IsRequired = true)]
        private bool _canBeDeleted;
        [DataMember(IsRequired = true)]
        private bool _canBeCopied;
        [DataMember(IsRequired = true)]
        private bool _canBeCut;
        [DataMember(IsRequired = true)]
        private bool _canCreateNewFolder;
        [DataMember(IsRequired = true)]
        private bool _canCreateNewItem;
        [DataMember(IsRequired = true)]
        private bool _canBeProcessed;
        [DataMember(IsRequired = true)]
        private bool _hasChildren;
        [DataMember(IsRequired = true)]
        private eROApplicationType _ROApplicationType;
        [DataMember(IsRequired = true)]
        private string _uniqueID;
        [DataMember(IsRequired = true)]
        private string _qualifiedText;
        [DataMember(IsRequired = true)]
        private ROTreeNodeData _treeNodeData;
        #endregion Fields

        #region "Properties"
        public int Key { get { return _treeNodeData.Key; } }

        public string Text { get { return _text; } }

        public int OwnerUserRID { get { return _ownerUserRID; } }

        public eTreeNodeType TreeNodeType { get { return _treeNodeType; } }

        public eProfileType ProfileType { get { return _treeNodeData.ProfileType; } }

        public bool IsReadOnly { get { return _isReadOnly; } }

        public bool CanBeDeleted { get { return _canBeDeleted; } }

        public bool CanBeCopied { get { return _canBeCopied; } }

        public bool CanBeCut { get { return _canBeCut; } }

        public bool CanCreateNewFolder { get { return _canCreateNewFolder; } }

        public bool CanCreateNewItem { get { return _canCreateNewItem; } }

        public bool CanBeProcessed { get { return _canBeProcessed; } }
        public bool HasChildren { get { return _hasChildren; } }

        public eROApplicationType ROApplicationType { get { return _ROApplicationType; } }

        public string UniqueID { get { return _uniqueID; } }

        public string QualifiedText
        {
            get
            {
                if (_qualifiedText == null)
                {
                    return _text;
                }
                return _qualifiedText;
            }
        }

        public ROTreeNodeData TreeNodeData { get { return _treeNodeData; } }

        #endregion Properties

        #region "Constructor"
        public ROTreeNodeOut(int key, string text, int ownerUserRID, eTreeNodeType treeNodeType, eProfileType profileType,
            bool isReadOnly, bool canBeDeleted, bool canBeCopied, bool canBeCut, bool canCreateNewFolder, bool canCreateNewItem, bool canBeProcessed,
            bool hasChildren, eROApplicationType ROApplicationType = eROApplicationType.All, string uniqueID = null, string qualifiedText = null)
        {
            _key = key;
            _text = text;
            _ownerUserRID = ownerUserRID;
            _treeNodeType = treeNodeType;
            _profileType = profileType;
            _isReadOnly = isReadOnly;
            _canBeDeleted = canBeDeleted;
            _canBeCopied = canBeCopied;
            _canBeCut = canBeCut;
            _canCreateNewFolder = canCreateNewFolder;
            _canCreateNewItem = canCreateNewItem;
            _canBeProcessed = canBeProcessed;
            _hasChildren = hasChildren;
            _ROApplicationType = ROApplicationType;
            _uniqueID = uniqueID;
            _qualifiedText = qualifiedText;
            _treeNodeData = new ROTreeNodeData(key: key, profileType: profileType);
        }

        public ROTreeNodeOut(string text, int ownerUserRID, eTreeNodeType treeNodeType,
            bool isReadOnly, bool canBeDeleted, bool canBeCopied, bool canBeCut, bool canCreateNewFolder, bool canCreateNewItem, bool canBeProcessed,
            bool hasChildren, eROApplicationType ROApplicationType = eROApplicationType.All, string uniqueID = null, string qualifiedText = null,
            ROTreeNodeData treeNodeData = null)
        {
            _text = text;
            _ownerUserRID = ownerUserRID;
            _treeNodeType = treeNodeType;
            _isReadOnly = isReadOnly;
            _canBeDeleted = canBeDeleted;
            _canBeCopied = canBeCopied;
            _canBeCut = canBeCut;
            _canCreateNewFolder = canCreateNewFolder;
            _canCreateNewItem = canCreateNewItem;
            _canBeProcessed = canBeProcessed;
            _hasChildren = hasChildren;
            _ROApplicationType = ROApplicationType;
            _uniqueID = uniqueID;
            _qualifiedText = qualifiedText;
            _treeNodeData = treeNodeData;
        }
        #endregion

    };

    /// <summary>
    /// Used to retrieve data values specific to a tree node
    /// </summary>

    [DataContract(Name = "ROTreeNodeData", Namespace = "http://Logility.ROWeb/")]
    public class ROTreeNodeData
    {
        #region "Fields"

        [DataMember(IsRequired = true)]
        private int _key;
        [DataMember(IsRequired = true)]
        private eProfileType _profileType;

        #endregion

        #region "Constructor"
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ROTreeNodeData(int key, eProfileType profileType)
        {
            _key = key;
            _profileType = profileType;
        }

        #endregion

        #region "Properties"

        /// <summary>
        /// Gets the key for the tree node data.
        /// </summary>
        public int Key { get { return _key; } }

        /// <summary>
        /// Gets the eProfileType for the tree node data.
        /// </summary>
        public eProfileType ProfileType { get { return _profileType; } }

        #endregion
    }

    /// <summary>
    /// Used to retrieve and update information about a model
    /// </summary>

    [DataContract(Name = "ROModelProfile", Namespace = "http://Logility.ROWeb/")]
    public class ROModelProfile : ROIntOut
    {
        #region "Fields"

        [DataMember(IsRequired = true)]
        private eChangeType _modelChangeType;
        [DataMember(IsRequired = true)]
        private eLockStatus _modelLockStatus;
        [DataMember(IsRequired = true)]
        private string _modelID;
        [DataMember(IsRequired = true)]
        private DateTime _updateDateTime;
        [DataMember(IsRequired = true)]
        private bool _needsRebuilt;

        #endregion

        #region "Constructor"
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ROModelProfile(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, int aKey)
            : base(ROReturnCode, sROMessage, ROInstanceID, aKey)
        {
            _modelChangeType = eChangeType.none;
        }

        #endregion

        #region "Properties"

        /// <summary>
        /// Gets or sets the type of change for the model.
        /// </summary>
        public eChangeType ModelChangeType
        {
            get { return _modelChangeType; }
            set { _modelChangeType = value; }
        }
        /// <summary>
        /// Gets or sets the status of lock for the model.
        /// </summary>
        public eLockStatus ModelLockStatus
        {
            get { return _modelLockStatus; }
            set { _modelLockStatus = value; }
        }
        /// <summary>
        /// Gets or sets the id of the  model.
        /// </summary>
        public string ModelID
        {
            get { return _modelID; }
            set { _modelID = value; }
        }
        /// <summary>
        /// Gets or sets the date and time stamp of the last time this model was updated.
        /// </summary>
        public DateTime UpdateDateTime
        {
            get { return _updateDateTime; }
            set { _updateDateTime = value; }
        }
        /// <summary>
        /// Gets the string value for date and time stamp of the last time this model was updated.
        /// </summary>
        public string UpdateDateTimeString
        {
            get
            {
                return _updateDateTime.ToShortDateString() + " " + _updateDateTime.ToShortTimeString();
            }
        }
        /// <summary>
        /// Gets or sets a flag identifying if the model date information needs rebuilt before it can be used.
        /// </summary>
        public bool NeedsRebuilt
        {
            get { return _needsRebuilt; }
            set { _needsRebuilt = value; }
        }

        #endregion
    }

    [DataContract(Name = "ROSizeGroupProfileOut", Namespace = "http://Logility.RoWeb/")]
    public class ROSizeGroupProfileOut : ROModelProfile
    {
        #region "DataMember" 

        [DataMember(IsRequired = true)]
        private string _sizeGroupName;

        [DataMember(IsRequired = true)]
        private string _sizeGroupDescription;

        [DataMember(IsRequired = true)]
        private List<SizeCodeProfile> _sizeCodeProfiles;

        #endregion

        #region "Properties"
        public string SizeGroupName
        {
            get { return _sizeGroupName; }
            set { _sizeGroupName = value; }
        }

        public string SizeGroupDescription
        {
            get { return _sizeGroupDescription; }
            set { _sizeGroupDescription = value; }
        }
        public List<SizeCodeProfile> SizeCodeProfiles
        {
            get { return _sizeCodeProfiles; }
        }

        #endregion

        #region "Constructor"

        public ROSizeGroupProfileOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, int aKey, string sizeGroupName,
            string sizeGroupDescription, List<SizeCodeProfile> sizeCodeList)
            : base(ROReturnCode, sROMessage, ROInstanceID, aKey)
        {
            _sizeGroupName = sizeGroupName;
            _sizeGroupDescription = sizeGroupDescription;
            _sizeCodeProfiles = sizeCodeList;
        }

        #endregion

    }

    [DataContract(Name = "ROMethodPropertiesOut", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodPropertiesOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROMethodProperties _ROMethodProperties;

        public ROMethodPropertiesOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROMethodProperties ROMethodProperties) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _ROMethodProperties = ROMethodProperties;
        }

        public ROMethodProperties ROMethodProperties { get { return _ROMethodProperties; } }

    }

    [DataContract(Name = "ROLowLevelsOut", Namespace = "http://Logility.RoWeb/")]
    public class ROLowLevelsOut : ROModelProfile
    {
        #region "DataMember" 

        [DataMember(IsRequired = true)]
        private List<LowLevelCombo> _lowLevelCombo;

        public List<LowLevelCombo> LowLevelCombo
        {
            get { return _lowLevelCombo; }
        }
        #endregion

        #region "Constructor"

        public ROLowLevelsOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID,
            List<LowLevelCombo> lowLevelCombo, int aKey)
            : base(ROReturnCode, sROMessage, ROInstanceID, aKey)
        {
            _lowLevelCombo = lowLevelCombo;
        }

        #endregion
    }

    [DataContract(Name = "ROInUseEntry", Namespace = "http://Logility.RoWeb/")]
    public class ROInUseEntry
    {
        //=======
        // FIELDS
        //=======

        [DataMember(IsRequired = true)]
        private List<string> _inUseValues;

        //=============
        // CONSTRUCTORS
        //=============

        public ROInUseEntry()
        {
            _inUseValues = new List<string>();
        }

        //===========
        // PROPERTIES
        //===========

        public List<string> InUseValues
        {
            get
            {
                return _inUseValues;
            }
        }
    }

    [DataContract(Name = "ROInUse", Namespace = "http://Logility.RoWeb/")]
    public class ROInUse
    {
        #region "DataMember" 

        [DataMember(IsRequired = true)]
        private bool _allowDelete;

        [DataMember(IsRequired = true)]
        private string _title;

        [DataMember(IsRequired = true)]
        private List<string> _columnLabels;

        [DataMember(IsRequired = true)]
        private List<ROInUseEntry> _inUseList;

        public bool AllowDelete
        {
            get { return _allowDelete; }
        }

        public string Title
        {
            get { return _title; }
        }

        public List<ROInUseEntry> InUseList
        {
            get { return _inUseList; }
        }

        public List<string> ColumnLabels
        {
            get { return _columnLabels; }
        }
        #endregion

        #region "Constructor"

        public ROInUse(bool allowDelete, string title)
        {
            _allowDelete = allowDelete;
            _title = title;
            _inUseList = new List<ROInUseEntry>();
            _columnLabels = new List<string>();
        }

        #endregion
    }

    [DataContract(Name = "ROInUseOut", Namespace = "http://Logility.RoWeb/")]
    public class ROInUseOut : ROOut
    {
        #region "DataMember" 

        [DataMember(IsRequired = true)]
        private ROInUse _ROInUse;

        public ROInUse ROInUse
        {
            get { return _ROInUse; }
        }
        #endregion

        #region "Constructor"

        public ROInUseOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID,
            ROInUse ROInUse)
            : base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _ROInUse = ROInUse;
        }

        #endregion
    }


    [DataContract(Name = "ROSecurityProfile", Namespace = "http://Logility.RoWeb/")]
    public class ROSecurityProfile
    {
        #region "DataMember" 

        [DataMember(IsRequired = true)]
        private Dictionary<eSecurityActions, eSecurityLevel> _ROActions;
        [DataMember(IsRequired = true)]
        private bool _accessDenied;
        [DataMember(IsRequired = true)]
        private bool _fullControl;

        public Dictionary<eSecurityActions, eSecurityLevel> ROActions
        {
            get { return _ROActions; }
        }
        #endregion

        #region "Constructor"

        public ROSecurityProfile(bool fullControll, bool accessdenied)
        {
            _fullControl = fullControll;
            _accessDenied = accessdenied;
            _ROActions = new Dictionary<eSecurityActions, eSecurityLevel>();
        }

        /// <summary>
        /// Gets a flag identifying if the user has read only security to this item.
        /// </summary>

        public bool IsReadOnly
        {
            get { return (AllowView && !AllowUpdate); }
        }

        /// <summary>
        /// Gets a flag identifying if the user has full control this item.
        /// </summary>

        public bool AllowFullControl
        {
            get { return _fullControl; }
        }

        /// <summary>
        /// Gets a flag identifying if the user is denied access to this item.
        /// </summary>

        public bool AccessDenied
        {
            get { return _accessDenied; }
        }

        /// <summary>
        /// Gets a flag identifying if the user can update this item.
        /// </summary>

        public bool AllowUpdate
        {
            get { return AllowFullControl || GetSecurityLevel(eSecurityActions.Maintain) == eSecurityLevel.Allow; }
        }

        /// <summary>
        /// Gets a flag identifying if the user can view this item.
        /// </summary>

        public bool AllowView
        {
            get { return AllowFullControl || GetSecurityLevel(eSecurityActions.View) == eSecurityLevel.Allow; }
        }

        /// <summary>
        /// Gets a flag identifying if the user can delete this item.
        /// </summary>

        public bool AllowDelete
        {
            get { return AllowFullControl || GetSecurityLevel(eSecurityActions.Delete) == eSecurityLevel.Allow; }
        }

        /// <summary>
        /// Gets a flag identifying if the user can execute this item.
        /// </summary>

        public bool AllowExecute
        {
            get { return AllowFullControl || GetSecurityLevel(eSecurityActions.Execute) == eSecurityLevel.Allow; }
        }

        /// <summary>
        /// Gets a flag identifying if the user can move this item.
        /// </summary>

        public bool AllowMove
        {
            get { return AllowFullControl || GetSecurityLevel(eSecurityActions.Move) == eSecurityLevel.Allow; }
        }

        /// <summary>
        /// Gets a flag identifying if the user can inactivate this item.
        /// </summary>

        public bool AllowInactivate
        {
            get { return AllowFullControl || GetSecurityLevel(eSecurityActions.Inactivate) == eSecurityLevel.Allow; }
        }

        /// <summary>
        /// Gets a flag identifying if the user can process interactive.
        /// </summary>

        public bool AllowInteractive
        {
            get { return AllowFullControl || GetSecurityLevel(eSecurityActions.Interactive) == eSecurityLevel.Allow; }
        }

        public bool AllowApplyChangesToLowerLevels
        {
            get { return AllowFullControl || GetSecurityLevel(eSecurityActions.ApplyChangesToLowerLevels) == eSecurityLevel.Allow; }
        }

        /// <summary>
        /// Gets a flag identifying if the user can apply a setting to lower levels.
        /// </summary>

        public bool AllowApplyToLowerLevels
        {
            get { return AllowFullControl || GetSecurityLevel(eSecurityActions.ApplyToLowerLevels) == eSecurityLevel.Allow; }
        }

        /// <summary>
        /// Gets a flag identifying if the user can remove the current settings to inherit from a higher level.
        /// </summary>

        public bool AllowInheritFromHigherLevel
        {
            get { return AllowFullControl || GetSecurityLevel(eSecurityActions.InheritFromHigherLevel) == eSecurityLevel.Allow; }
        }

        /// <summary>
        /// Gets a flag identifying if the user can assign a value.
        /// </summary>

        public bool AllowAssign
        {
            get { return AllowFullControl || GetSecurityLevel(eSecurityActions.Assign) == eSecurityLevel.Allow; }
        }

        /// <summary>
        /// Gets the security level for the provided action
        /// </summary>
        /// <param name="aActionID">The eSecurityActions of the action
        /// </param>
        /// <returns>
        /// An eSecurityLevel containing the security level for the action
        /// </returns>

        public eSecurityLevel GetSecurityLevel(eSecurityActions aActionID)
        {
            if (ROActions.ContainsKey(aActionID))
            {
                return ROActions[aActionID];
            }
            else
            {
                return eSecurityLevel.NotSpecified;
            }
        }

        #endregion
    }

    [DataContract(Name = "ROFunctionSecurityOut", Namespace = "http://Logility.RoWeb/")]
    public class ROFunctionSecurityOut : ROOut
    {
        #region "DataMember" 

        [DataMember(IsRequired = true)]
        private Dictionary<eSecurityFunctions, ROSecurityProfile> _ROFunctionSecurity;

        public Dictionary<eSecurityFunctions, ROSecurityProfile> ROFunctionSecurity
        {
            get { return _ROFunctionSecurity; }
        }
        #endregion

        #region "Constructor"

        public ROFunctionSecurityOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, Dictionary<eSecurityFunctions, ROSecurityProfile> ROFunctionSecurity)
            : base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _ROFunctionSecurity = ROFunctionSecurity;
        }

        public ROSecurityProfile GetFunctionSecurity(eSecurityFunctions securityFunction)
        {
            if (_ROFunctionSecurity.ContainsKey(securityFunction))
            {
                return _ROFunctionSecurity[securityFunction];
            }
            else
            {
                return new ROSecurityProfile(fullControll: false, accessdenied: true);
            }
        }
        #endregion
    }

    public class ROAboutProperties
    {
        public string CurrentUser { get; set; }
        public string Environment { get; set; }
        public string ProductName { get; set; }
        public string ProductVersion { get; set; }
        public string EmailSupport { get; set; }
        public string AfterHoursPhone { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string SessionEnviroment { get; set; }
        public string OpertatingSystem { get; set; }
        public string OpertatingSystemVersion { get; set; }
        public string OpertatingSystemEdition { get; set; }
        public string OpertatingSystemServicePack { get; set; }
        public string LegalCopyright { get; set; }
        public List<string> AddOns { get; set; }
        public List<string> Configurations { get; set; }

        public ROAboutProperties()
        {
            AddOns = new List<string>();
            Configurations = new List<string>();
        }
    }

    [DataContract(Name = "ROAboutOut", Namespace = "http://Logility.ROWeb/")]
    public class ROAboutOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROAboutProperties _ROAboutProperties;

        public ROAboutOut(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROAboutProperties ROAboutProperties) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _ROAboutProperties = ROAboutProperties;
        }

        public ROAboutProperties ROAboutProperties { get { return _ROAboutProperties; } }

    }

    [DataContract(Name = "ROColorGroup", Namespace = "http://Logility.ROWeb/")]
    public class ROColorGroup
    {
        #region "DataMember"
        //=======
        // FIELDS
        //=======

        [DataMember(IsRequired = true)]
        private string _colorGroupName;

        [DataMember(IsRequired = true)]
        private List<ROColor> _colors;

        #endregion Fields

        #region "Properties"
        
        public string ColorGroupName
        {
            get { return _colorGroupName; }
        }

        public List<ROColor> Colors
        {
            get { return _colors; }
        }

        #endregion Properties

        #region "Constructor"
        public ROColorGroup(string colorGroupName)
        {
            _colorGroupName = colorGroupName;
            _colors = new List<ROColor>();
        }


        #endregion
    }

    [DataContract(Name = "ROColor", Namespace = "http://Logility.ROWeb/")]
    public class ROColor
    {
        #region "DataMember"
        //=======
        // FIELDS
        //=======

        [DataMember(IsRequired = true)]
        private int _key;

        [DataMember(IsRequired = true)]
        private string _colorID;

        [DataMember(IsRequired = true)]
        private string _colorName;

        [DataMember(IsRequired = true)]
        private string _colorGroupName;

        #endregion Fields

        #region "Properties"
        public int Key
        {
            get { return _key; }
        }

        public string ColorID
        {
            get { return _colorID; }
        }

        public string ColorName
        {
            get { return _colorName; }
        }

        public string ColorGroupName
        {
            get { return _colorGroupName; }
        }

        #endregion Properties

        #region "Constructor"
        public ROColor(int key, string colorID, string colorName, string colorGroupName)
        {
            _key = key;
            _colorID = colorID;
            _colorName = colorName;
            _colorGroupName = colorGroupName;
        }


        #endregion
    }

    [DataContract(Name = "ROMessageRequest", Namespace = "http://Logility.ROWeb/")]
    public class ROMessageRequest : ROOut
    {
        #region "DataMember"
        //=======
        // FIELDS
        //=======

        [DataMember(IsRequired = true)]
        private eMessageRequest _messageRequest;

        [DataMember(IsRequired = true)]
        private ROMessageDetails _messageDetails;

        #endregion Fields

        #region "Properties"
        public eMessageRequest MessageRequest
        {
            get { return _messageRequest; }
        }

        public bool MessageDetailsPresent
        {
            get { return _messageDetails != null; }
        }

        public ROMessageDetails MessageDetails
        {
            get { return _messageDetails; }
        }
        #endregion Properties

        #region "Constructor"
        public ROMessageRequest(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, eMessageRequest messageRequest, ROMessageDetails messageDetails = null) :
            base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _messageRequest = messageRequest;
            _messageDetails = messageDetails;
        }


        #endregion
    }

    [DataContract(Name = "ROMessageDetails", Namespace = "http://Logility.ROWeb/")]
    public class ROMessageDetails
    {
        #region "DataMember"
        //=======
        // FIELDS
        //=======

        #endregion Fields

        #region "Properties"
        #endregion Properties

        #region "Constructor"
        public ROMessageDetails() 
        {

        }


        #endregion
    }

    [DataContract(Name = "ROMessageDetailsCreatePlaceholders", Namespace = "http://Logility.ROWeb/")]
    public class ROMessageDetailsCreatePlaceholders : ROMessageDetails
    {
        #region "DataMember"
        //=======
        // FIELDS
        //=======

        [DataMember(IsRequired = true)]
        private int _placeholderCount;

        #endregion Fields

        #region "Properties"
        public int PlaceholderCount
        {
            get { return _placeholderCount; }
        }
        #endregion Properties

        #region "Constructor"
        public ROMessageDetailsCreatePlaceholders(int placeholderCount) :
            base()
        {
            _placeholderCount = placeholderCount;
        }


        #endregion
    }

    [DataContract(Name = "ROTaskListPropertiesOut", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskListPropertiesOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROTaskListProperties _ROTaskListProperties;

        public ROTaskListPropertiesOut(
            eROReturnCode ROReturnCode, 
            string sROMessage, 
            long ROInstanceID, 
            ROTaskListProperties ROTaskListProperties
            ) 
            :
            base(
                ROReturnCode, 
                sROMessage, 
                ROInstanceID
                )
        {
            _ROTaskListProperties = ROTaskListProperties;
        }

        public ROTaskListProperties ROTaskListProperties { get { return _ROTaskListProperties; } }

    }

    [DataContract(Name = "ROTaskPropertiesOut", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskPropertiesOut : ROOut
    {
        [DataMember(IsRequired = true)]
        private ROTaskProperties _ROTaskProperties;

        public ROTaskPropertiesOut(
            eROReturnCode ROReturnCode, 
            string sROMessage, 
            long ROInstanceID, 
            ROTaskProperties ROTaskProperties
            ) 
            :
            base(
                ROReturnCode: ROReturnCode, 
                sROMessage: sROMessage,
                ROInstanceID: ROInstanceID
                )
        {
            _ROTaskProperties = ROTaskProperties;
        }

        public ROTaskProperties ROTaskProperties { get { return _ROTaskProperties; } }

    }
}
