using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.DataCommon;

namespace Logility.ROWebSharedTypes
{
    [DataContract(Name = "ROTreeNodeParms", Namespace = "http://Logility.ROWeb/")]
    public class ROTreeNodeParms : ROParms 
    {
        [DataMember(IsRequired = true)]
        private eProfileType _profileType;
        [DataMember(IsRequired = true)]
        private int _key;
        [DataMember(IsRequired = true)]
        private int _ownerUserRID;
        [DataMember(IsRequired = true)]
        private eROApplicationType _ROApplicationType;
        [DataMember(IsRequired = true)]
        private string _uniqueID;

        public ROTreeNodeParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                        eProfileType profileType, int key, int ownerUserRID = Include.NoRID, eROApplicationType ROApplicationType = eROApplicationType.All,
                                        string uniqueID = null)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _profileType = profileType;
            _key = key;
            _ownerUserRID = ownerUserRID;
            _ROApplicationType = ROApplicationType;
            _uniqueID = uniqueID;
        }

        public eProfileType ProfileType { get { return _profileType; } }

        public int Key { get { return _key; } }

        public int OwnerUserRID { get { return _ownerUserRID; } }

        public eROApplicationType ROApplicationType { get { return _ROApplicationType; } }

        public string UniqueID { get { return _uniqueID; } }
    }

    [DataContract(Name = "ROMethodParms", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private eMethodType _methodType;
        [DataMember(IsRequired = true)]
        private int _key;
        [DataMember(IsRequired = true)]
        private bool _readOnly;
        [DataMember(IsRequired = true)]
        private int _workflowStep;
        [DataMember(IsRequired = true)]
        private bool _keyIsMerchandise;

        public ROMethodParms(
            string sROUserID, 
            string sROSessionID, 
            eROClass ROClass, 
            eRORequest RORequest, 
            long ROInstanceID,
            eMethodType methodType, 
            int key = Include.Undefined, 
            bool readOnly = false,
            int workflowStep = Include.Undefined,
            bool keyIsMerchandise = false
            )
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _methodType = methodType;
            _key = key;
            _readOnly = readOnly;
            _workflowStep = workflowStep;
            _keyIsMerchandise = keyIsMerchandise;
        }

        public eMethodType MethodType { get { return _methodType; } set { _methodType = value; } }

        public int Key { get { return _key; } }

        public bool ReadOnly { get { return _readOnly; } }

        public int WorkflowStep { get { return _workflowStep; } set { _workflowStep = value; } }

        public bool KeyIsMerchandise { get { return _keyIsMerchandise; } set { _keyIsMerchandise = value; } }
    }

    [DataContract(Name = "ROMethodOverrideModelListParms", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodOverrideModelListParms : ROMethodParms
    {
        [DataMember(IsRequired = true)]
        private ROOverrideLowLevel _overrideLowLevel;

        public ROMethodOverrideModelListParms(
            string sROUserID,
            string sROSessionID,
            eROClass ROClass,
            eRORequest RORequest,
            long ROInstanceID,
            eMethodType methodType,
            int key = Include.Undefined,
            int workflowStep = Include.Undefined,
            ROOverrideLowLevel overrideLowLevel = null
            )
            : base(
                  sROUserID: sROUserID, 
                  sROSessionID: sROSessionID, 
                  ROClass: ROClass, 
                  RORequest: RORequest, 
                  ROInstanceID: ROInstanceID,
                  methodType: methodType, 
                  key: key, 
                  readOnly: false,
                  workflowStep: workflowStep
                  )
        {
            _overrideLowLevel = overrideLowLevel;
        }

        public ROOverrideLowLevel OverrideLowLevel { get { return _overrideLowLevel; } set { _overrideLowLevel = value; } }
    }

    [DataContract(Name = "ROMethodPropertiesParms", Namespace = "http://Logility.ROWeb/")]
    public class ROMethodPropertiesParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROMethodProperties _ROMethodProperties;
        [DataMember(IsRequired = true)]
        private int _folderKey;
        [DataMember(IsRequired = true)]
        private string _folderUniqueID;

        public ROMethodPropertiesParms(
            string sROUserID, 
            string sROSessionID, 
            eROClass ROClass, 
            eRORequest RORequest, 
            long ROInstanceID, 
            ROMethodProperties ROMethodProperties, 
            int folderKey = Include.NoRID, 
            string folderUniqueID = null
            ) 
            :
            base(
                sROUserID, 
                sROSessionID, 
                ROClass, 
                RORequest, 
                ROInstanceID
                )
        {
            _ROMethodProperties = ROMethodProperties;
            _folderKey = folderKey;
            _folderUniqueID = folderUniqueID;
        }

        public ROMethodProperties ROMethodProperties { get { return _ROMethodProperties; } }

        public int FolderKey { get { return _folderKey; } set { _folderKey = value; } }

        public string FolderUniqueID { get { return _folderUniqueID; } set { _folderUniqueID = value; } }

    }

    [DataContract(Name = "ROInUseParms", Namespace = "http://Logility.ROWeb/")]
    public class ROInUseParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private eProfileType _itemProfileType;
        [DataMember(IsRequired = true)]
        private int _key;

        public ROInUseParms(
            string sROUserID, 
            string sROSessionID, 
            eROClass ROClass, 
            eRORequest RORequest, 
            long ROInstanceID, 
            eProfileType itemProfileType, 
            int key
            ) 
            :
            base(
                sROUserID, 
                sROSessionID, 
                ROClass, 
                RORequest, 
                ROInstanceID
                )
        {
            _itemProfileType = itemProfileType;
            _key = key;
        }

        public eProfileType ItemProfileType { get { return _itemProfileType; } }

        public int Key { get { return _key; } set { _key = value; } }

    }


    [DataContract(Name = "ROWorkflowPropertiesParms", Namespace = "http://Logility.ROWeb/")]
    public class ROWorkflowPropertiesParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROWorkflow _ROWorkflow;

        [DataMember(IsRequired = true)]
        private int _folderKey;

        [DataMember(IsRequired = true)]
        private string _folderUniqueID;

        public ROWorkflowPropertiesParms(
            string sROUserID, 
            string sROSessionID, 
            eROClass ROClass, 
            eRORequest RORequest, 
            long ROInstanceID, 
            ROWorkflow ROWorkflow, 
            int folderKey = Include.NoRID, 
            string folderUniqueID = null
            ) 
            :
            base(
                sROUserID, 
                sROSessionID, 
                ROClass, 
                RORequest, 
                ROInstanceID
                )
        {
            _ROWorkflow = ROWorkflow;
            _folderKey = folderKey;
            _folderUniqueID = folderUniqueID;
        }

        public ROWorkflow ROWorkflow { get { return _ROWorkflow; } }

        public int FolderKey { get { return _folderKey; } set { _folderKey = value; } }

        public string FolderUniqueID { get { return _folderUniqueID; } set { _folderUniqueID = value; } }

    }

    [DataContract(Name = "ROReleaseResourceParms", Namespace = "http://Logility.ROWeb/")]
    public class ROReleaseResourceParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private bool _allUsers;

        [DataMember(IsRequired = true)]
        private int _userKey;

        public ROReleaseResourceParms(
            string sROUserID, 
            string sROSessionID, 
            eROClass ROClass, 
            eRORequest RORequest, 
            long ROInstanceID, 
            bool AllUsers = false, 
            int userKey = Include.Undefined
            ) 
            :
            base(
                sROUserID, 
                sROSessionID, 
                ROClass, 
                RORequest, 
                ROInstanceID
                )
        {
            _allUsers = AllUsers;
            _userKey = userKey;
        }

        public bool AllUsers { get { return _allUsers; } }

        public int UserKey { get { return _userKey; } set { _userKey = value; } }

    }

    [DataContract(Name = "ROBaseUpdateParms", Namespace = "http://Logility.ROWeb/")]
    public class ROBaseUpdateParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private eProfileType _profileType;

        [DataMember(IsRequired = true)]
        private int _key;

        [DataMember(IsRequired = true)]
        private string _name;

        [DataMember(IsRequired = true)]
        private string _newName;

        [DataMember(IsRequired = true)]
        private int _userKey;

        [DataMember(IsRequired = true)]
        private eProfileType _parentProfileType;

        [DataMember(IsRequired = true)]
        private int _parentKey;

        [DataMember(IsRequired = true)]
        private int _parentUserKey;

        [DataMember(IsRequired = true)]
        private eProfileType _toParentProfileType;

        [DataMember(IsRequired = true)]
        private int _toParentKey;

        [DataMember(IsRequired = true)]
        private int _newUserKey;

        [DataMember(IsRequired = true)]
        private string _uniqueID;

        [DataMember(IsRequired = true)]
        private string _parentUniqueID;

        [DataMember(IsRequired = true)]
        private string _toParentUniqueID;

        public ROBaseUpdateParms(
            string sROUserID, 
            string sROSessionID, 
            eROClass ROClass, 
            eRORequest RORequest, 
            long ROInstanceID, 
            eProfileType profileType, 
            int key, 
            string name, 
            string newName = null, 
            int userKey = Include.NoRID,
            eProfileType parentProfileType = eProfileType.None,
            int parentKey = Include.NoRID,
            int parentUserKey = Include.NoRID,
            eProfileType toParentProfileType = eProfileType.None,
            int toParentKey = Include.NoRID,
            int newUserKey = Include.NoRID,
            string uniqueID = null,
            string parentUniqueID = null,
            string toParentUniqueID = null
            ) 
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _profileType = profileType;
            _key = key;
            _name = name;
            _newName = newName;
            _userKey = userKey;
            _parentProfileType = parentProfileType;
            _parentKey = parentKey;
            _parentUserKey = parentUserKey;
            _toParentProfileType = toParentProfileType;
            _toParentKey = toParentKey;
            _newUserKey = newUserKey;
            _uniqueID = uniqueID;
            _parentUniqueID = parentUniqueID;
            _toParentUniqueID = toParentUniqueID;
        }

        public eProfileType ProfileType { get { return _profileType; } }
        public int Key { get { return _key; } }
        public string Name { get { return _name; } }
        public string NewName { get { return _newName; } }
        public int UserKey { get { return _userKey; } }
        public eProfileType ParentProfileType { get { return _parentProfileType; } }
        public int ParentKey { get { return _parentKey; } }
        public int ParentUserKey { get { return _parentUserKey; } }
        public eProfileType ToParentProfileType { get { return _toParentProfileType; } }
        public int ToParentKey { get { return _toParentKey; } }
        public int NewUserKey { get { return _newUserKey; } }
        public string UniqueID { get { return _uniqueID; } }
        public string ParentUniqueID { get { return _parentUniqueID; } }
        public string ToParentUniqueID { get { return _toParentUniqueID; } }

    }

    [DataContract(Name = "ROWorklistUpdateParms", Namespace = "http://Logility.ROWeb/")]
    public class ROWorklistUpdateParms : ROBaseUpdateParms
    {
        public ROWorklistUpdateParms(
            string sROUserID,
            string sROSessionID,
            eROClass ROClass,
            eRORequest RORequest,
            long ROInstanceID,
            eProfileType profileType,
            int key,
            string name,
            string newName = null,
            int userKey = Include.NoRID,
            eProfileType parentProfileType = eProfileType.None,
            int parentKey = Include.NoRID,
            int toParentKey = Include.NoRID,
            int newUserKey = Include.NoRID
            )
            : base(sROUserID: sROUserID,
                    sROSessionID: sROSessionID,
                    ROClass: ROClass,
                    RORequest: RORequest,
                    ROInstanceID: ROInstanceID,
                    profileType: profileType,
                    key: key,
                    name: name,
                    newName: newName,
                    userKey: userKey,
                    parentProfileType: parentProfileType,
                    parentKey: parentKey,
                    toParentKey: toParentKey,
                    newUserKey: newUserKey
                  )
        {

        }
    }

    [DataContract(Name = "ROWorklistRenameParms", Namespace = "http://Logility.ROWeb/")]
    public class ROWorklistRenameParms : ROBaseUpdateParms
    {
        public ROWorklistRenameParms(
            string sROUserID,
            string sROSessionID,
            eROClass ROClass,
            eRORequest RORequest,
            long ROInstanceID,
            eProfileType profileType,
            int key,
            string name,
            string newName
            )
            : base(sROUserID : sROUserID,
                    sROSessionID: sROSessionID,
                    ROClass : ROClass,
                    RORequest: eRORequest.Rename,
                    ROInstanceID: ROInstanceID,
                    profileType: profileType,
                    key: key,
                    name: name,
                    newName: newName
                  )
        {

        }
    }

    [DataContract(Name = "ROWorklistDeleteParms", Namespace = "http://Logility.ROWeb/")]
    public class ROWorklistDeleteParms : ROBaseUpdateParms
    {
        public ROWorklistDeleteParms(
            string sROUserID,
            string sROSessionID,
            eROClass ROClass,
            eRORequest RORequest,
            long ROInstanceID,
            eProfileType profileType,
            int key,
            string name
            )
            : base(sROUserID: sROUserID,
                    sROSessionID: sROSessionID,
                    ROClass: ROClass,
                    RORequest: eRORequest.Delete,
                    ROInstanceID: ROInstanceID,
                    profileType: profileType,
                    key: key,
                    name: name
                  )
        {

        }
    }

    [DataContract(Name = "ROWorklistCopyParms", Namespace = "http://Logility.ROWeb/")]
    public class ROWorklistCopyParms : ROBaseUpdateParms
    {
        public ROWorklistCopyParms(
            string sROUserID,
            string sROSessionID,
            eROClass ROClass,
            eRORequest RORequest,
            long ROInstanceID,
            eProfileType profileType,
            int key,
            string name,
            int userKey,
            eProfileType parentProfileType,
            int parentKey,
            int toParentKey,
            int newUserKey
            )
            : base(sROUserID: sROUserID,
                    sROSessionID: sROSessionID,
                    ROClass: ROClass,
                    RORequest: eRORequest.Copy,
                    ROInstanceID: ROInstanceID,
                    profileType: profileType,
                    key: key,
                    name: name,
                    userKey: userKey,
                    parentProfileType: parentProfileType,
                    parentKey: parentKey,
                    toParentKey: toParentKey,
                    newUserKey: newUserKey
                  )
        {

        }
    }

    [DataContract(Name = "RODataExplorerRenameParms", Namespace = "http://Logility.ROWeb/")]
    public class RODataExplorerRenameParms : ROBaseUpdateParms
    {
        public RODataExplorerRenameParms(
            string sROUserID,
            string sROSessionID,
            eROClass ROClass,
            eRORequest RORequest,
            long ROInstanceID,
            eProfileType profileType,
            int key,
            string name,
            string newName,
            string uniqueID = null
            )
            : base(sROUserID: sROUserID,
                    sROSessionID: sROSessionID,
                    ROClass: ROClass,
                    RORequest: eRORequest.Rename,
                    ROInstanceID: ROInstanceID,
                    profileType: profileType,
                    key: key,
                    name: name,
                    newName: newName,
                    uniqueID: uniqueID
                  )
        {

        }
    }

    [DataContract(Name = "RODataExplorerCopyParms", Namespace = "http://Logility.ROWeb/")]
    public class RODataExplorerCopyParms : ROBaseUpdateParms
    {
        public RODataExplorerCopyParms(
            string sROUserID,
            string sROSessionID,
            eROClass ROClass,
			eRORequest RORequest,
            long ROInstanceID,
            eProfileType profileType,
            int key,
            string name,
            int userKey,
            //eProfileType parentProfileType = eProfileType.None,
            //int parentKey = Include.NoRID,
            //int parentUserKey = Include.NoRID,
            eProfileType toParentProfileType = eProfileType.None,
            int toParentKey = Include.NoRID,
            int toParentUserKey = Include.NoRID,
            string toParentUniqueID = null
            )
            : base(sROUserID: sROUserID,
                    sROSessionID: sROSessionID,
                    ROClass: ROClass,
                    RORequest:eRORequest.Copy,
                    ROInstanceID: ROInstanceID,
                    profileType: profileType,
                    key: key,
                    name: name,
                    userKey: userKey,
                    //parentProfileType: parentProfileType,
                    //parentKey: parentKey,
                    //parentUserKey: parentUserKey,
                    toParentProfileType: toParentProfileType,
                    toParentKey: toParentKey,
                    newUserKey: toParentUserKey,
                    toParentUniqueID: toParentUniqueID
                  )
        {

        }
    }

    [DataContract(Name = "RODataExplorerShortcutParms", Namespace = "http://Logility.ROWeb/")]
    public class RODataExplorerShortcutParms : ROBaseUpdateParms
    {
        public RODataExplorerShortcutParms(
            string sROUserID,
            string sROSessionID,
            eROClass ROClass,
            eRORequest RORequest,
            long ROInstanceID,
            eProfileType profileType,
            int key,
            string name,
            int userKey = Include.NoRID,
            eProfileType parentProfileType = eProfileType.None,
            int parentKey = Include.NoRID,
            int parentUserKey = Include.NoRID,
            eProfileType toParentProfileType = eProfileType.None,
            int toParentKey = Include.NoRID,
            int toParentUserKey = Include.NoRID,
            string parentUniqueID = null,
            string toParentUniqueID = null
            )
            : base(sROUserID: sROUserID,
                    sROSessionID: sROSessionID,
                    ROClass: ROClass,
                    RORequest: RORequest,
                    ROInstanceID: ROInstanceID,
                    profileType: profileType,
                    key: key,
                    name: name,
                    userKey: userKey,
                    parentProfileType: parentProfileType,
                    parentKey: parentKey,
                    parentUserKey: parentUserKey,
                    toParentProfileType: toParentProfileType,
                    toParentKey: toParentKey,
                    newUserKey: toParentUserKey,
                    parentUniqueID: parentUniqueID,
                    toParentUniqueID: toParentUniqueID
                  )
        {

        }
    }

    [DataContract(Name = "RODataExplorerFolderParms", Namespace = "http://Logility.ROWeb/")]
    public class RODataExplorerFolderParms : ROBaseUpdateParms
    {
        public RODataExplorerFolderParms(
            string sROUserID,
            string sROSessionID,
            eROClass ROClass,
            eRORequest RORequest,
            long ROInstanceID,
            eProfileType profileType = eProfileType.None,
            int key = Include.NoRID,
            string name = null,
            int userKey = Include.NoRID,
            eProfileType parentProfileType = eProfileType.None,
            int parentKey = Include.NoRID,
            int parentUserKey = Include.NoRID,
            eProfileType toParentProfileType = eProfileType.None,
            int toParentKey = Include.NoRID,
            int toParentUserKey = Include.NoRID,
            string uniqueID = null,
            string parentUniqueID = null,
            string toParentUniqueID = null
            )
            : base(sROUserID: sROUserID,
                    sROSessionID: sROSessionID,
                    ROClass: ROClass,
                    RORequest: RORequest,
                    ROInstanceID: ROInstanceID,
                    profileType: profileType,
                    key: key,
                    name: name,
                    userKey: userKey,
                    parentProfileType: parentProfileType,
                    parentKey: parentKey,
                    parentUserKey: parentUserKey,
                    toParentProfileType: toParentProfileType,
                    toParentKey: toParentKey,
                    newUserKey: toParentUserKey,
                    uniqueID: uniqueID,
                    parentUniqueID: parentUniqueID,
                    toParentUniqueID: toParentUniqueID
                  )
        {

        }
    }

    [DataContract(Name = "RODataExplorerSaveAsParms", Namespace = "http://Logility.ROWeb/")]
    public class RODataExplorerSaveAsParms : ROBaseUpdateParms
    {
        public RODataExplorerSaveAsParms(
            string sROUserID,
            string sROSessionID,
            eROClass ROClass,
            eRORequest RORequest,
            long ROInstanceID,
            eProfileType profileType,
            int key,
            string name,
			string newName,
            int userKey,
            eProfileType toParentProfileType,
            int toParentKey,
            int toParentUserKey
            )
            : base(sROUserID: sROUserID,
                    sROSessionID: sROSessionID,
                    ROClass: ROClass,
                    RORequest: eRORequest.SaveAs,
                    ROInstanceID: ROInstanceID,
                    profileType: profileType,
                    key: key,
                    name: name,
					newName: newName,
                    userKey: userKey,
                    toParentProfileType: toParentProfileType,
                    toParentKey: toParentKey,
                    newUserKey: toParentUserKey
                  )
        {

        }
    }

    [DataContract(Name = "ROMessageResponseParms", Namespace = "http://Logility.ROWeb/")]
    public class ROMessageResponseParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private eMessageResponse _messageResponse;

        [DataMember(IsRequired = true)]
        private ROMessageDetails _messageDetails;

        public ROMessageResponseParms(string sROUserID, string sROSessionID, eROClass ROClass, eRORequest RORequest, long ROInstanceID,
                                        eMessageResponse messageResponse  = eMessageResponse.None, ROMessageDetails messageDetails = null)
            : base(sROUserID, sROSessionID, ROClass, RORequest, ROInstanceID)
        {
            _messageResponse = messageResponse;
            _messageDetails = messageDetails;
        }

        public eMessageResponse MessageResponse { get { return _messageResponse; } }

        public bool MessageDetailsPresent
        {
            get { return _messageDetails != null; }
        }

        public ROMessageDetails MessageDetails
        {
            get { return _messageDetails; }
        }

    }

    [DataContract(Name = "ROColumnFormat", Namespace = "http://Logility.ROWeb/")]
    public class ROColumnFormat
    {
        [DataMember(IsRequired = true)]
        private eDataType _dataType;  // the table the column is from
        [DataMember(IsRequired = true)]
        private int _columnIndex; // the column 
        [DataMember(IsRequired = true)]
        private int _width;   // the width

        public ROColumnFormat(eDataType dataType, int columnIndex, int width)
        {
            _dataType = dataType;
            _columnIndex = columnIndex;
            _width = width;
        }

        public eDataType DataType { get { return _dataType; } }
        public int ColumnIndex { get { return _columnIndex; } }
        public int Width { get { return _width; } }
    };

    [DataContract(Name = "ROViewFormatParms", Namespace = "http://Logility.ROWeb/")]
    public class ROViewFormatParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROColumnFormat _headingColumn;

        [DataMember(IsRequired = true)]
        private List<ROColumnFormat> _columnFormats;

        [DataMember(IsRequired = true)]
        private List<double> _verticalSplitterPercentages;

        [DataMember(IsRequired = true)]
        private List<double> _horizontalSplitterPercentages;

        public ROViewFormatParms(
            string sROUserID,
            string sROSessionID,
            eROClass ROClass,
            eRORequest RORequest,
            long ROInstanceID,
            ROColumnFormat headingColumn
            )
            : base(
                  sROUserID: sROUserID,
                  sROSessionID: sROSessionID,
                  ROClass: ROClass,
                  RORequest: RORequest,
                  ROInstanceID: ROInstanceID
                  )
        {
            _headingColumn = headingColumn;
            _columnFormats = new List<ROColumnFormat>();
            _verticalSplitterPercentages = new List<double>();
            _horizontalSplitterPercentages = new List<double>();
        }

        public ROColumnFormat HeadingColumn { get { return _headingColumn; } }

        public List<ROColumnFormat> ColumnFormats { get { return _columnFormats; } }

        public List<double> VerticalSplitterPercentages { get { return _verticalSplitterPercentages; } }

        public List<double> HorizontalSplitterPercentages { get { return _horizontalSplitterPercentages; } }

    }

    [DataContract(Name = "ROTaskListPropertiesParms", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskListPropertiesParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROTaskListProperties _ROTaskListProperties;
        [DataMember(IsRequired = true)]
        private int _folderKey;
        [DataMember(IsRequired = true)]
        private string _folderUniqueID;

        public ROTaskListPropertiesParms(
            string sROUserID, 
            string sROSessionID, 
            eROClass ROClass, 
            eRORequest RORequest, 
            long ROInstanceID, 
            ROTaskListProperties ROTaskListProperties, 
            int folderKey = Include.NoRID, 
            string folderUniqueID = null
            ) :
            base(
                sROUserID: sROUserID, 
                sROSessionID: sROSessionID, 
                ROClass: ROClass, 
                RORequest: RORequest, 
                ROInstanceID: ROInstanceID
                )
        {
            _ROTaskListProperties = ROTaskListProperties;
            _folderKey = folderKey;
            _folderUniqueID = folderUniqueID;
        }

        public ROTaskListProperties ROTaskListProperties { get { return _ROTaskListProperties; } }

        public int FolderKey { get { return _folderKey; } set { _folderKey = value; } }

        public string FolderUniqueID { get { return _folderUniqueID; } set { _folderUniqueID = value; } }

    }

    [DataContract(Name = "ROTaskPropertiesParms", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskPropertiesParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private ROTaskProperties _ROTaskProperties;
        [DataMember(IsRequired = true)]
        private int _folderKey;
        [DataMember(IsRequired = true)]
        private string _folderUniqueID;

        public ROTaskPropertiesParms(string sROUserID,
            string sROSessionID,
            eROClass ROClass,
            eRORequest RORequest,
            long ROInstanceID,
            ROTaskProperties ROTaskProperties,
            int folderKey = Include.NoRID,
            string folderUniqueID = null
            ) :
            base(
                sROUserID: sROUserID,
                sROSessionID: sROSessionID,
                ROClass: ROClass,
                RORequest: RORequest,
                ROInstanceID: ROInstanceID
                )
        {
            _ROTaskProperties = ROTaskProperties;
            _folderKey = folderKey;
            _folderUniqueID = folderUniqueID;
        }

        public ROTaskProperties ROTaskProperties { get { return _ROTaskProperties; } }

        public int FolderKey { get { return _folderKey; } set { _folderKey = value; } }

        public string FolderUniqueID { get { return _folderUniqueID; } set { _folderUniqueID = value; } }

    }

    [DataContract(Name = "ROTaskParms", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private eTaskType _taskType;
        [DataMember(IsRequired = true)]
        private int _sequence;
        [DataMember(IsRequired = true)]
        private bool _readOnly;

        public ROTaskParms(
            string sROUserID, 
            string sROSessionID, 
            eROClass ROClass, 
            eRORequest RORequest, 
            long ROInstanceID,
            eTaskType taskType, 
            int sequence, 
            bool readOnly = false
            )
            : 
            base(
                sROUserID: sROUserID,
                sROSessionID: sROSessionID,
                ROClass: ROClass,
                RORequest: RORequest,
                ROInstanceID: ROInstanceID
                )
        {
            _taskType = taskType;
            _sequence = sequence;
            _readOnly = readOnly;
        }

        public eTaskType TaskType { get { return _taskType; } set { _taskType = value; } }

        public int Sequence { get { return _sequence; } }

        public bool ReadOnly { get { return _readOnly; } }
    }

    [DataContract(Name = "ROWorkflowMethodParms", Namespace = "http://Logility.ROWeb/")]
    public class ROWorkflowMethodParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private eMethodType _methodType;
        [DataMember(IsRequired = true)]
        private int _workflowKey;
        [DataMember(IsRequired = true)]
        private int _workflowStep;

        public ROWorkflowMethodParms(
            string sROUserID, 
            string sROSessionID, 
            eROClass ROClass, 
            eRORequest RORequest, 
            long ROInstanceID, 
            eMethodType methodType,
            int workflowKey = Include.NoRID,
            int workflowStep = Include.Undefined
            ) 
            : base(
                  sROUserID, 
                  sROSessionID, 
                  ROClass, 
                  RORequest, 
                  ROInstanceID
                  )
        {
            _methodType = methodType;
            _workflowKey = workflowKey;
            _workflowStep = workflowStep;
        }

        public eMethodType MethodType { get { return _methodType; } }

        public int WorkflowKey { get { return _workflowKey; } }

        public int WorkflowStep { get { return _workflowStep; } }

    };

    [DataContract(Name = "ROTaskJobsParms", Namespace = "http://Logility.ROWeb/")]
    public class ROTaskJobsParms : ROParms
    {
        [DataMember(IsRequired = true)]
        private List<ROTaskJobs> _ROTaskJobs;
        [DataMember(IsRequired = true)]
        private int _folderKey;
        [DataMember(IsRequired = true)]
        private KeyValuePair<int, string> _key;

        public ROTaskJobsParms(
            string sROUserID,
            string sROSessionID,
            eROClass ROClass,
            eRORequest RORequest,
            long ROInstanceID,
            List<ROTaskJobs> ROTaskJobs,
            KeyValuePair<int, string> key,
            int folderKey = Include.NoRID
            ) :
            base(
                sROUserID: sROUserID,
                sROSessionID: sROSessionID,
                ROClass: ROClass,
                RORequest: RORequest,
                ROInstanceID: ROInstanceID
                )
        {
            _ROTaskJobs = ROTaskJobs;
            _folderKey = folderKey;
            _key = key;
        }

        public List<ROTaskJobs> ROTaskJobs { get { return _ROTaskJobs; } }

        public int FolderKey { get { return _folderKey; } set { _folderKey = value; } }

        public KeyValuePair<int, string> Key { get { return _key; } set { _key = value; } }

    }
}
