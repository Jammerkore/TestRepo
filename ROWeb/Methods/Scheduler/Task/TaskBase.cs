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
        private HierarchyProfile _mainHierarchyProfile = null;
        private Dictionary<int, HierarchyNodeProfile> _hierarchyNodeProfileDictionary = new Dictionary<int, HierarchyNodeProfile>();
        private Dictionary<int, DateRangeProfile> _dateRangeDictionary = new Dictionary<int, DateRangeProfile>();
        private ScheduleData _ScheduleDataLayer = null;

        private SessionAddressBlock _sessionAddressBlock;
        private ROWebTools _ROWebTools;
        private eTaskType _taskType;
        private eLockStatus _lockStatus = eLockStatus.Undefined;

        private DataTable _taskData = null;
        private DataTable _taskDetailData = null;

        //=============
        // CONSTRUCTORS
        //=============
        public TaskBase(
            SessionAddressBlock sessionAddressBlock,
            ROWebTools ROWebTools,
            eTaskType taskType,
            ROTaskListProperties taskListProperties
            )
        {
            _sessionAddressBlock = sessionAddressBlock;
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
        public SessionAddressBlock SessionAddressBlock
        {
            get
            {
                return _sessionAddressBlock;
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


        public ScheduleData ScheduleDataLayer
        {
            get
            {
                if (_ScheduleDataLayer == null)
                {
                    _ScheduleDataLayer = new ScheduleData();
                }
                return _ScheduleDataLayer;
            }
        }

        public DataTable TaskData
        {
            get
            {
                return _taskData;
            }
            set
            {
                _taskData = value;
            }
        }

        public DataTable TaskDetailData
        {
            get
            {
                return _taskDetailData;
            }
            set
            {
                _taskDetailData = value;
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

        protected HierarchyProfile MainHierarchyProfile
        {
            get
            {
                if (_mainHierarchyProfile == null)
                {
                    _mainHierarchyProfile = SessionAddressBlock.HierarchyServerSession.GetMainHierarchyData();
                }
                return _mainHierarchyProfile;
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

        abstract public bool TaskUpdateSequence(
            int oldSequence,
            int newSequence,
            ref string message
            );

        abstract public bool TaskSaveData(
            ScheduleData scheduleDataLayer,
            ref string message
            );

        abstract public bool TaskDelete(
            ROTaskParms taskParameters,
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

        abstract public void TaskGetValues();

        public void UnlockNode(
            int key
            )
        {
            SessionAddressBlock.HierarchyServerSession.DequeueNode(nodeRID: key);
            _hierarchyNodeProfile.NodeLockStatus = eLockStatus.Undefined;
        }

        /// <summary>
        /// Update the sequence in the data tables
        /// </summary>
        /// <param name="oldSequence">The current sequence number</param>
        /// <param name="newSequence">The new sequence number</param>
        protected void UpdateTaskRowSequence(
            int oldSequence, 
            int newSequence
            )
        {
            string selectString = "TASK_SEQUENCE=" + oldSequence;
            if (_taskData != null)
            {
                DataRow[] taskDataRows = _taskData.Select(selectString);
                foreach (var taskDataRow in taskDataRows)
                {
                    taskDataRow["TASK_SEQUENC"] = newSequence;
                }
                _taskData.AcceptChanges();
            }

            if (_taskDetailData != null)
            {
                DataRow[] detailDataRows = _taskDetailData.Select(selectString);
                foreach (var detailDataRow in detailDataRows)
                {
                    detailDataRow["TASK_SEQUENC"] = newSequence;
                }
                _taskDetailData.AcceptChanges();
            }
        }

        /// <summary>
        /// Update the task list key in the data tables
        /// </summary>
        /// <param name="taskListKey">The key for the task list</param>
        public void UpdateTaskListKey(
            int taskListKey
            )
        {
            if (_taskData != null)
            {
                foreach (DataRow taskDataRow in _taskData.Rows)
                {
                    taskDataRow["TASKLIST_RID"] = taskListKey;
                }
                _taskData.AcceptChanges();
            }

            if (_taskDetailData != null)
            {
                foreach (DataRow detailDataRow in _taskDetailData.Rows)
                {
                    detailDataRow["TASKLIST_RID"] = taskListKey;
                }
                _taskDetailData.AcceptChanges();
            }
        }

        protected void DeleteTaskRows(
            int sequence
            )
        {
            string selectString = "TASK_SEQUENCE=" + sequence;
            if (_taskData != null)
            {
                DataRow[] taskDataRows = _taskData.Select(selectString);
                foreach (var taskDataRow in taskDataRows)
                {
                    taskDataRow.Delete();
                }
                _taskData.AcceptChanges();
            }

            if (_taskDetailData != null)
            {
                DataRow[] detailDataRows = _taskDetailData.Select(selectString);
                foreach (var detailDataRow in detailDataRows)
                {
                    detailDataRow.Delete();
                }
                _taskDetailData.AcceptChanges();
            }
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
            HierarchyNodeProfile hierarchyNodeProfile = null;
            if (chaseHierarchy
                || buildQualifiedID)
            {
                hierarchyNodeProfile = SessionAddressBlock.HierarchyServerSession.GetNodeData(
                    aNodeRID: key, 
                    aChaseHierarchy: chaseHierarchy, 
                    aBuildQualifiedID: buildQualifiedID
                    );
            }
            else if (!_hierarchyNodeProfileDictionary.TryGetValue(key, out hierarchyNodeProfile))
            {
                hierarchyNodeProfile = SessionAddressBlock.HierarchyServerSession.GetNodeData(
                    nodeRID: key, 
                    chaseHierarchy: chaseHierarchy
                    );
                _hierarchyNodeProfileDictionary.Add(key, hierarchyNodeProfile);
            }
            return hierarchyNodeProfile;
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
                hnp = SessionAddressBlock.HierarchyServerSession.GetNodeData(
                    aNodeID: ID, 
                    aChaseHierarchy: chaseHierarchy, 
                    aBuildQualifiedID: buildQualifiedID
                    );
            }
            else
            {
                hnp = SessionAddressBlock.HierarchyServerSession.GetNodeData(
                    aNodeID: ID, 
                    aChaseHierarchy: chaseHierarchy, 
                    aBuildQualifiedID: buildQualifiedID
                    );
                _hierarchyNodeProfileDictionary.Add(hnp.Key, hnp);
            }
            return hnp;
        }

        protected DateRangeProfile GetDateRangeProfile(
            int key
            )
        {
            DateRangeProfile dateRange = null;
            if (!_dateRangeDictionary.TryGetValue(key, out dateRange))
            {
                dateRange = SessionAddressBlock.ClientServerSession.Calendar.GetDateRange(key);
                _dateRangeDictionary.Add(dateRange.Key, dateRange);
            }
            return dateRange;
        }

    }
}
