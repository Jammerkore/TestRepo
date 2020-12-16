using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    abstract public class TaskBase
    {
        //=======
        // FIELDS
        //=======

        protected HierarchyNodeSecurityProfile _nodeSecurity = null;
        protected HierarchyNodeProfile _hierarchyNodeProfile = null;
        private ROTaskListProperties _taskListProperties;
        private HierarchyProfile _hierarchyProfile = null;
        private HierarchyProfile _mainHierProf = null;
        private Dictionary<int, HierarchyNodeProfile> _hnpDict = new Dictionary<int, HierarchyNodeProfile>();
        private Dictionary<int, DateRangeProfile> _dateRangeDict = new Dictionary<int, DateRangeProfile>();
        private ScheduleData _dataLayerSchedule = null;

        private SessionAddressBlock _SAB;
        private ROWebTools _ROWebTools;
        private eTaskType _taskType;
        private eLockStatus _lockStatus = eLockStatus.Undefined;

        //=============
        // CONSTRUCTORS
        //=============
        public TaskBase(
            SessionAddressBlock SAB,
            ROWebTools ROWebTools,
            eTaskType taskType,
            ROTaskListProperties taskListProperties
            )
        {
            _SAB = SAB;
            _ROWebTools = ROWebTools;
            _taskType = taskType;
            _taskListProperties = taskListProperties;
        }

        //===========
        // PROPERTIES
        //===========

        public string Name
        {
            get
            {
                return MIDText.GetTextOnly((int)_taskType);
            }
        }

        /// <summary>
        /// Gets SessionAddressBlock.
        /// </summary>
        public SessionAddressBlock SAB
        {
            get
            {
                return _SAB;
            }
        }
        /// <summary>
        /// Gets the ROWebTools
        /// </summary>
        public ROWebTools ROWebTools
        {
            get { return _ROWebTools; }
        }

        public eTaskType TaskType
        {
            get
            {
                return _taskType;
            }
        }

        public ROTaskListProperties TaskListProperties
        {
            get
            {
                return _taskListProperties;
            }
        }

        public int TaskListKey
        {
            get
            {
                return _taskListProperties.TaskList.Key;
            }
        }


        public ScheduleData DataLayerSchedule
        {
            get
            {
                if (_dataLayerSchedule == null)
                {
                    _dataLayerSchedule = new ScheduleData();
                }
                return _dataLayerSchedule;
            }
        }

        public HierarchyNodeSecurityProfile HierarchyNodeSecurity
        {
            get
            {
                if (_nodeSecurity == null)
                {
                    _nodeSecurity = new HierarchyNodeSecurityProfile(aKey: Include.NoRID);
                }
                return _nodeSecurity;
            }
        }

        public HierarchyNodeProfile HierarchyNodeProfile
        {
            get
            {
                return _hierarchyNodeProfile;
            }
        }

        public HierarchyProfile HierarchyProfile
        {
            get
            {
                return _hierarchyProfile;
            }
            set
            {
                _hierarchyProfile = value;
            }
        }

        public eLockStatus LockStatus
        {
            get
            {
                return _lockStatus;
            }
            set
            {
                _lockStatus = value;
            }
        }

        protected HierarchyProfile MainHierProf
        {
            get
            {
                if (_mainHierProf == null)
                {
                    _mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();
                }
                return _mainHierProf;
            }
        }

        //========
        // METHODS
        //========


        abstract public ROTaskProperties TaskGetData(
            ROTaskParms parms,
            ref string message,
            bool applyOnly = false
            );

        abstract public ROTaskProperties TaskUpdateData(
            ROTaskProperties task,
            bool cloneDates,
            ref string message,
            out bool successful,
            bool applyOnly = false
            );

        abstract public bool TaskDelete(
            int key,
            ref string message
            );

        abstract public bool OnClosing();

        virtual public ROProfileKeyParms TaskGetParms(
            ROProfileKeyParms parms,
            eProfileType profileType,
            int key,
            bool readOnly = false
            )
        {
            ROProfileKeyParms profileKeyParms = new ROProfileKeyParms(
                sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.GetTask,
                ROInstanceID: parms.ROInstanceID,
                profileType: profileType,
                key: key,
                readOnly: readOnly
                );

            return profileKeyParms;
        }

        public void UnlockNode(
            int key
            )
        {
            SAB.HierarchyServerSession.DequeueNode(nodeRID: key);
            _hierarchyNodeProfile.NodeLockStatus = eLockStatus.Undefined;
        }

        public eLockStatus LockNode(
            eModelType modelType, 
            int key, 
            string name, 
            bool allowReadOnly, 
            out string message
            )
        {
            message = null;
            eLockStatus lockStatus = eLockStatus.Undefined;

            return lockStatus;
        }

        protected HierarchyNodeProfile GetHierarchyNodeProfile(
            int key, 
            bool chaseHierarchy = false, 
            bool buildQualifiedID = false
            )
        {
            HierarchyNodeProfile hnp = null;
            if (chaseHierarchy
                || buildQualifiedID)
            {
                hnp = SAB.HierarchyServerSession.GetNodeData(
                    aNodeRID: key, 
                    aChaseHierarchy: chaseHierarchy, 
                    aBuildQualifiedID: buildQualifiedID
                    );
            }
            else if (!_hnpDict.TryGetValue(key, out hnp))
            {
                hnp = SAB.HierarchyServerSession.GetNodeData(
                    nodeRID: key, 
                    chaseHierarchy: chaseHierarchy
                    );
                _hnpDict.Add(key, hnp);
            }
            return hnp;
        }

        protected HierarchyNodeProfile GetHierarchyNodeProfile(
            string ID, 
            bool chaseHierarchy = false, 
            bool buildQualifiedID = false
            )
        {
            HierarchyNodeProfile hnp = null;
            if (chaseHierarchy
                || buildQualifiedID)
            {
                hnp = SAB.HierarchyServerSession.GetNodeData(
                    aNodeID: ID, 
                    aChaseHierarchy: chaseHierarchy, 
                    aBuildQualifiedID: buildQualifiedID
                    );
            }
            else
            {
                hnp = SAB.HierarchyServerSession.GetNodeData(
                    aNodeID: ID, 
                    aChaseHierarchy: chaseHierarchy, 
                    aBuildQualifiedID: buildQualifiedID
                    );
                _hnpDict.Add(hnp.Key, hnp);
            }
            return hnp;
        }

        protected DateRangeProfile GetDateRangeProfile(
            int key
            )
        {
            DateRangeProfile dateRange = null;
            if (!_dateRangeDict.TryGetValue(key, out dateRange))
            {
                dateRange = SAB.ClientServerSession.Calendar.GetDateRange(key);
                _dateRangeDict.Add(dateRange.Key, dateRange);
            }
            return dateRange;
        }

    }
}
