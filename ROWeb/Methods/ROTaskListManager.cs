using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business.Allocation;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    public class ROTaskListManager
    {
        private SessionAddressBlock _sessionAddressBlock;
        private ROWebTools _ROWebTools;
        private FunctionSecurityProfile _functionSecurity;
        private ClientSessionTransaction _clientTransaction = null;
        private ApplicationSessionTransaction _applicationSessionTransaction = null;
        private long _ROInstanceID;
        private bool _nameValid = false;
        private string _nameMessage;
        private FunctionSecurityProfile _userSecLvl;
        private FunctionSecurityProfile _globalSecLvl;
        private FunctionSecurityProfile _systemSecLvl;
        private ScheduleData _scheduleDataLayer = null;
        private FolderDataLayer _folderDataLayer = null;
        private TaskListProfile _taskListProfile = null;
        private ROTaskListProperties _taskListProperties = null;
        private JobProfile _jobProfile = null;
        private TaskBase _task = null;
        private DataTable _tasksDataTable;
        // Collection to manage tasks within the task list.  Key is task type.
        // All tasks for a single type are contained in the same class
        private Dictionary<eTaskType, TaskBase> _taskListTasks = new Dictionary<eTaskType, TaskBase>();

        /// <summary>
        /// Gets the SessionAddressBlock
        /// </summary>
        protected SessionAddressBlock SessionAddressBlock
        {
            get { return _sessionAddressBlock; }
        }

        /// <summary>
        /// Gets the ROWebTools
        /// </summary>
        public ROWebTools ROWebTools
        {
            get { return _ROWebTools; }
        }

        /// <summary>
        /// Gets the unique function ID
        /// </summary>
        protected long ROInstanceID
        {
            get { return _ROInstanceID; }
        }

        /// <summary>
        /// Gets the key associated with the type of object
        /// </summary>
        public int Key
        {
            get
            {
                if (_taskListProfile != null)
                {
                    return _taskListProfile.Key;
                }
                else if (_jobProfile != null)
                {
                    return _jobProfile.Key;
                }
                else
                {
                    return Include.NoRID;
                }
            }
        }

        /// <summary>
        /// Gets the eProfileType associated with the type of object
        /// </summary>
        public eProfileType ProfileType
        {
            get
            {
                if (_taskListProfile != null)
                {
                    return _taskListProfile.ProfileType;
                }
                else if (_jobProfile != null)
                {
                    return _jobProfile.ProfileType;
                }
                else
                {
                    return eProfileType.None;
                }
            }
        }

        /// <summary>
        /// Data layer class to communicate to the database for schedule data
        /// </summary>
        public ScheduleData ScheduleDataLayer
        {
            get
            {
                if (_scheduleDataLayer == null)
                {
                    _scheduleDataLayer = new ScheduleData();
                }
                return _scheduleDataLayer;
            }
        }

        /// <summary>
        /// Data layer class to communicate to the database for folder data
        /// </summary>
        public FolderDataLayer FolderDataLayer
        {
            get
            {
                if (_folderDataLayer == null)
                {
                    _folderDataLayer = new FolderDataLayer();
                }
                return _folderDataLayer;
            }
        }

        

        /// <summary>
        /// Gets or sets the function security profile
        /// </summary>
        protected FunctionSecurityProfile FunctionSecurity
        {
            get { return _functionSecurity; }
            set { _functionSecurity = value; }
        }

        protected ClientSessionTransaction ClientTransaction
        {
            get
            {
                if (_clientTransaction == null)
                {
                    _clientTransaction = SessionAddressBlock.ClientServerSession.CreateTransaction();
                }
                return _clientTransaction;
            }
        }

        protected ApplicationSessionTransaction GetApplicationSessionTransaction(bool getNewTransaction = false)
        {
            if (_applicationSessionTransaction == null
                || getNewTransaction)
            {
                if (_applicationSessionTransaction != null)
                {
                    _applicationSessionTransaction.Dispose();
                }
                _applicationSessionTransaction = SessionAddressBlock.ApplicationServerSession.CreateTransaction();
            }
            return _applicationSessionTransaction;
        }

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SessionAddressBlock">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        /// <param name="ROInstanceID">The instance ID of the session</param>
        /// <param name="applicationSessionTransaction">The transaction to use for processing</param>
        public ROTaskListManager(
            SessionAddressBlock sessionAddressBlock, 
            ROWebTools ROWebTools, 
            long ROInstanceID, 
            ApplicationSessionTransaction applicationSessionTransaction = null
            )
        {
            _sessionAddressBlock = sessionAddressBlock;
            _ROWebTools = ROWebTools;
            _ROInstanceID = ROInstanceID;
            _applicationSessionTransaction = applicationSessionTransaction;
            FunctionSecurity = _sessionAddressBlock.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsScheduler);
            // Get the security for the user
            _userSecLvl = _sessionAddressBlock.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerUserTaskLists);
            _globalSecLvl = _sessionAddressBlock.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerGlobalTaskLists);
            _systemSecLvl = _sessionAddressBlock.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerSystemTaskLists);
        }

        /// <summary>
        /// Removes all locks and cleans up all necessary memory
        /// </summary>
        public void CleanUp()
        {
            if (_taskListProfile != null
                && _taskListProfile.LockStatus == eLockStatus.Locked)
            {
                string message = null;
                _taskListProfile.LockStatus = TaskListUtilities.UnLockItem(
                    sessionAddressBlock: SessionAddressBlock,
                    profileType: eProfileType.TaskList,
                    Key: _taskListProfile.Key,
                    message: out message
                    );
                if (_taskListProfile.LockStatus == eLockStatus.Cancel)
                {
                    MIDEnvironment.Message = message;
                }
            }

            if (_jobProfile != null
                && _jobProfile.LockStatus == eLockStatus.Locked)
            {
                string message = null;
                _jobProfile.LockStatus = TaskListUtilities.UnLockItem(
                    sessionAddressBlock: SessionAddressBlock,
                    profileType: eProfileType.Job,
                    Key: _jobProfile.Key,
                    message: out message
                    );
                if (_jobProfile.LockStatus == eLockStatus.Cancel)
                {
                    MIDEnvironment.Message = message;
                }
            }

        }

        #region Get the TaskLists TaskList List
        /// <summary>
        /// Gets a list of tasks for task lists.
        /// </summary>
        /// <returns>ROIntStringPairListOut with KeyValuePair of eTaskType and task name</returns>
        public ROOut GetListOfTasks()
        {
            try
            {
                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, GetTasksList());
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetTasksList failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        /// <summary>
        /// Build list of tasks based on the user's security
        /// </summary>
        /// <returns>ROIntStringPairListOut with KeyValuePair of eTaskType and task name</returns>
        internal List<KeyValuePair<int, string>> GetTasksList()
        {
            List<KeyValuePair<int, string>> taskList = new List<KeyValuePair<int, string>>();

            ArrayList tasks = Include.GetAvailableTasks(
               aUserSecurity: _userSecLvl, 
                aGlobalSecurity: _globalSecLvl, 
                aSystemSecurity: _systemSecLvl, 
                aIsSizeInstalled: _sessionAddressBlock.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled
                );

            foreach (eTaskType taskType in tasks)
            {
                if (taskType == eTaskType.None)
                {
                    //taskList.Add(new KeyValuePair<int, string>((int)eTaskType.None, "-"));
                }
                else
                {
                    taskList.Add(new KeyValuePair<int, string>((int)taskType, MIDText.GetTextOnly((int)taskType)));
                }
            }

            return taskList;
        }

        #endregion

        #region "TaskList Processing"

        /// <summary>
        /// Get the properties and tasks for a task list
        /// </summary>
        /// <param name="TaskListParameters">The parameters containing which task list to return</param>
        /// <param name="processingApply">A flag identifyinig if an apply is being processed</param>
        /// <returns></returns>
        public ROOut GetTaskList(
            ROProfileKeyParms taskListParameters, 
            bool processingApply = false
            )
        {
            string message = null;
            eROReturnCode returnCode = eROReturnCode.Successful;

            // unlock prior task list if different task list is requested
            if (_taskListProfile != null
                && _taskListProfile.Key != taskListParameters.Key
                && _taskListProfile.LockStatus == eLockStatus.Locked)
            {
                _taskListProfile.LockStatus = TaskListUtilities.UnLockItem(
                    sessionAddressBlock: SessionAddressBlock,
                    profileType: eProfileType.TaskList,
                    Key: _taskListProfile.Key,
                    message: out message
                    );
            }
            _taskListTasks.Clear();
            _task = null;
            ROTaskListProperties taskListProperties = BuildTaskListProperties(taskListParameters);

            // update FunctionSecurity based on user's security
            if (taskListParameters.ReadOnly)
            {
                FunctionSecurity.SetReadOnly();
            }
            else
            {
                switch (_taskListProfile.OwnerUserRID)
                {
                    case Include.GlobalUserRID:
                        if (_globalSecLvl.AllowUpdate)
                        {
                            FunctionSecurity.SetAllowUpdate();
                        }
                        else
                        {
                            FunctionSecurity.SetReadOnly();
                        }
                        break;
                    case Include.SystemUserRID:
                        if (_systemSecLvl.AllowUpdate)
                        {
                            FunctionSecurity.SetAllowUpdate();
                        }
                        else
                        {
                            FunctionSecurity.SetReadOnly();
                        }
                        break;
                    default:
                        if (_userSecLvl.AllowUpdate)
                        {
                            FunctionSecurity.SetAllowUpdate();
                        }
                        else
                        {
                            FunctionSecurity.SetReadOnly();
                        }
                        break;
                }
            }

            // Lock the task list if not read only
            if (!FunctionSecurity.IsReadOnly)
            {
                _taskListProfile.LockStatus = TaskListUtilities.LockItem(
                    sessionAddressBlock: SessionAddressBlock,
                    profileType: _taskListProfile.ProfileType,
                    changeType: eChangeType.update,
                    Key: _taskListProfile.Key,
                    Name: _taskListProfile.Name,
                    allowReadOnly: true,
                    message: out message
                    );
                // set environment variable is was unable to lock the task list
                if (_taskListProfile.LockStatus == eLockStatus.ReadOnly)
                {
                    MIDEnvironment.isChangedToReadOnly = true;
                    MIDEnvironment.Message = message;
                }
            }

            return new ROTaskListPropertiesOut(returnCode, message, ROInstanceID, taskListProperties);
        }

        /// <summary>
        /// Build the task list properties from database values
        /// </summary>
        /// <param name="taskListParameters">The keys of the task list to build</param>
        /// <returns></returns>
        private ROTaskListProperties BuildTaskListProperties(ROProfileKeyParms taskListParameters)
        {
            
            ROTaskProperties taskProperties;
            eTaskType taskType = eTaskType.None;
            string name, messageLevel;
            eMIDMessageLevel MIDMessageLevel;
            int sequence;
            string emailSuccessFrom = null;
            string emailSuccessTo = null;
            string emailSuccessCC = null;
            string emailSuccessBCC = null;
            string emailSuccessSubject = null;
            string emailSuccessBody = null;
            string emailFailureFrom = null;
            string emailFailureTo = null;
            string emailFailureCC = null;
            string emailFailureBCC = null;
            string emailFailureSubject = null;
            string emailFailureBody = null;

            // get the task list values from the database
            // and create the task list properties object
            if (taskListParameters.Key == Include.NoRID)
            {
                _taskListProfile = new TaskListProfile(Include.NoRID);
            }
            else
            {
                _taskListProfile = new TaskListProfile(ScheduleDataLayer.TaskList_Read(taskListParameters.Key));
            }
            _taskListProperties = new ROTaskListProperties(
                taskList: new KeyValuePair<int, string>(_taskListProfile.Key, _taskListProfile.Name),
                userKey: _taskListProfile.OwnerUserRID
                );

            _tasksDataTable = ScheduleDataLayer.Task_ReadByTaskList(taskListParameters.Key);
            _tasksDataTable.DefaultView.Sort = "TASK_SEQUENCE";

            // populate the task items into the task list
            foreach (DataRow dataRow in _tasksDataTable.Rows)
            {
                emailSuccessFrom = string.Empty;
                emailSuccessTo = string.Empty;
                emailSuccessCC = string.Empty;
                emailSuccessBCC = string.Empty;
                emailSuccessSubject = string.Empty;
                emailSuccessBody = string.Empty;
                emailFailureFrom = string.Empty;
                emailFailureTo = string.Empty;
                emailFailureCC = string.Empty;
                emailFailureBCC = string.Empty;
                emailFailureSubject = string.Empty;
                emailFailureBody = string.Empty;

                sequence = Convert.ToInt32(dataRow["TASK_SEQUENCE"]);
                taskType = (eTaskType)Convert.ToInt32(dataRow["TASK_TYPE"]);
                name = MIDText.GetTextOnly((int)taskType);
                MIDMessageLevel = (eMIDMessageLevel)Convert.ToInt32(dataRow["MAX_MESSAGE_LEVEL"]);
                messageLevel = MIDText.GetTextOnly((int)MIDMessageLevel);
                if (dataRow["EMAIL_SUCCESS_FROM"] != DBNull.Value)
                {
                    emailSuccessFrom = Convert.ToString(dataRow["EMAIL_SUCCESS_FROM"]);
                }
	            if (dataRow["EMAIL_SUCCESS_TO"] != DBNull.Value)
                {
                    emailSuccessTo = Convert.ToString(dataRow["EMAIL_SUCCESS_TO"]);
                }
                if (dataRow["EMAIL_SUCCESS_CC"] != DBNull.Value)
                {
                    emailSuccessCC = Convert.ToString(dataRow["EMAIL_SUCCESS_CC"]);
                }
                if (dataRow["EMAIL_SUCCESS_BCC"] != DBNull.Value)
                {
                    emailSuccessBCC = Convert.ToString(dataRow["EMAIL_SUCCESS_BCC"]);
                }
                if (dataRow["EMAIL_SUCCESS_SUBJECT"] != DBNull.Value)
                {
                    emailSuccessSubject = Convert.ToString(dataRow["EMAIL_SUCCESS_SUBJECT"]);
                }
                if (dataRow["EMAIL_SUCCESS_BODY"] != DBNull.Value)
                {
                    emailSuccessBody = Convert.ToString(dataRow["EMAIL_SUCCESS_BODY"]);
                }
                if (dataRow["EMAIL_FAILURE_FROM"] != DBNull.Value)
                {
                    emailFailureFrom = Convert.ToString(dataRow["EMAIL_FAILURE_FROM"]); 
                }
                if (dataRow["EMAIL_FAILURE_TO"] != DBNull.Value)
                {
                    emailFailureTo = Convert.ToString(dataRow["EMAIL_FAILURE_TO"]);
                }
                if (dataRow["EMAIL_FAILURE_CC"] != DBNull.Value)
                {
                    emailFailureCC = Convert.ToString(dataRow["EMAIL_FAILURE_CC"]);
                }
                if (dataRow["EMAIL_FAILURE_BCC"] != DBNull.Value)
                {
                    emailFailureBCC = Convert.ToString(dataRow["EMAIL_FAILURE_BCC"]);
                }
                if (dataRow["EMAIL_FAILURE_SUBJECT"] != DBNull.Value)
                {
                    emailFailureSubject = Convert.ToString(dataRow["EMAIL_FAILURE_SUBJECT"]);
                }
                if (dataRow["EMAIL_FAILURE_BODY"] != DBNull.Value)
                {
                    emailFailureBody = Convert.ToString(dataRow["EMAIL_FAILURE_BODY"]);
                }

                taskProperties = new ROTaskProperties(
                    task: new KeyValuePair<int, string>(sequence, name),
                    taskType: taskType,
                    maximumMessageLevel: new KeyValuePair<int, string>((int)MIDMessageLevel, messageLevel),
                    emailSuccessFrom: emailSuccessFrom,
                    emailSuccessTo: emailSuccessTo,
                    emailSuccessCC: emailSuccessCC,
                    emailSuccessBCC: emailSuccessBCC,
                    emailSuccessSubject: emailSuccessSubject,
                    emailSuccessBody: emailSuccessBody,
                    emailFailureFrom: emailFailureFrom,
                    emailFailureTo: emailFailureTo,
                    emailFailureCC: emailFailureCC,
                    emailFailureBCC: emailFailureBCC,
                    emailFailureSubject: emailFailureSubject,
                    emailFailureBody: emailFailureBody
                );
                _taskListProperties.Tasks.Add(taskProperties);
            }

            _taskListProperties.IsReadOnly = FunctionSecurity.IsReadOnly;
            _taskListProperties.CanBeProcessed = FunctionSecurity.AllowExecute;
            _taskListProperties.CanBeDeleted = FunctionSecurity.AllowDelete;

            return _taskListProperties;
        }

        /// <summary>
        /// Save the task list properties
        /// </summary>
        /// <param name="TaskListParameters">Contains the task list properties to save</param>
        /// <returns>A ROIListOut object containing an ROTreeNodeOut object with updated task list information</returns>
        public ROOut SaveTaskList(
            ROTaskListPropertiesParms taskListParameters,
            bool performingSaveAs = false
            )
        {
            string message = null;
            int sequence = 0;
            eTaskType taskType;            
            bool cloneDates = performingSaveAs;

            // creating new task list 
            if (_taskListProfile == null)
            {
                _taskListProfile = new TaskListProfile(Include.NoRID);
            }
            // Task List must be locked before it can be saved
            else if (_taskListProfile.Key != Include.NoRID
                && _taskListProfile.LockStatus != eLockStatus.Locked)
            {
                return new ROIListOut(eROReturnCode.Failure, "Task list is not locked and cannot be saved", ROInstanceID, null);
            }

            // Task Lists must have at least one task
            if (taskListParameters.ROTaskListProperties.Tasks.Count == 0)
            {
                message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneTaskRequired);
                return new ROIListOut(eROReturnCode.Failure, message, ROInstanceID, null);
            }
            // unlock prior task list if saving as new task list
            else if(_taskListProfile != null
                && taskListParameters.ROTaskListProperties.TaskList.Key == Include.NoRID
                && _taskListProfile.LockStatus == eLockStatus.Locked)
            {
                _taskListProfile.LockStatus = TaskListUtilities.UnLockItem(
                    sessionAddressBlock: SessionAddressBlock,
                    profileType: eProfileType.TaskList,
                    Key: _taskListProfile.Key,
                    message: out message
                    );
            }

            // To remove task instance from memory while tasklist is different
            _taskListTasks = _taskListTasks.
                                    Where(w => w.Value.TaskListKey == taskListParameters.ROTaskListProperties.TaskList.Key).
                                    ToDictionary(d => d.Key, d => d.Value);

            // Get task values from database for types that have not been accessed
            // They are needed based on the way all old values are deleted for a task list
            // and then inserted.
            // If the sequence is incorrect because tasks have been deleted or inserted, update the sequence
            // in the task before it is saved to the database.
            foreach (ROTaskProperties task in taskListParameters.ROTaskListProperties.Tasks)
            {
                if (!_taskListTasks.TryGetValue(task.TaskType, out _task))
                {
                    _task = GetTaskClass(
                        taskType: task.TaskType,
                        taskListProperties: _taskListProperties,
                        getNewClass: true
                        );
                    
                    _task.TaskGetValues();
                    _taskListTasks[_task.TaskType] = _task;
                }

                // if the task key (sequence) does not match, update the sequence
                if (task.Task.Key != sequence)
                {
                    // update the sequence in the main task data table
                    TaskUpdateSequence(
                        oldSequence: task.Task.Key,
                        newSequence: sequence
                        );

                    // update the sequence in the task data tables
                    _task.TaskUpdateSequence(
                        oldSequence: task.Task.Key,
                        newSequence: sequence,
                        message: ref message
                        );
                }

                ++sequence;
            }

            int key = taskListParameters.ROTaskListProperties.TaskList.Key;
            // make sure task list name is unique for the owner
            _taskListProfile.Name = CleanseTaskListName(
                taskListParameters: taskListParameters
                );
            // if the name was changed, update the data class with the new name
            if (_taskListProfile.Name != taskListParameters.ROTaskListProperties.TaskList.Value)
            {
                taskListParameters.ROTaskListProperties.TaskList = new KeyValuePair<int, string>(key, _taskListProfile.Name);
            }

            // get the folder key to locate the task list
            int folderKey = TaskListUtilities.GetTaskListFolderRID(
                profileType: eProfileType.TaskList,
                folderKey: taskListParameters.FolderKey,
                userKey: taskListParameters.ROTaskListProperties.UserKey,
                uniqueID: taskListParameters.FolderUniqueID);

            bool newTaskList = false;
            try
            {
                // open update connection to the database
                ScheduleDataLayer.OpenUpdateConnection();

                // create or update the task list
                if (key == Include.NoRID)
                {
                    newTaskList = true;
                    CreateTaskList(
                        taskListParameters: taskListParameters,
                        folderKey: folderKey
                        );

                    // set task list key in tasks data tables
                    // update main tasks data table
                    UpdateTaskListKey(
                         taskListKey: _taskListProfile.Key
                         );
                }
                else
                {
                    ScheduleDataLayer.TaskList_Update(_taskListProfile, SessionAddressBlock.ClientServerSession.UserRID);
                    DeleteTaskListTasks();
                }

                // insert the tasks for the task list
                ScheduleDataLayer.Task_Insert(_tasksDataTable);
                // To remove Task instance _taskListTasks based on Task sent from client side
                _taskListTasks = _taskListTasks.
                                    Where(w => taskListParameters.ROTaskListProperties.Tasks.Select(s => s.TaskType).Contains(w.Key)).
                                    ToDictionary(d => d.Key, d => d.Value);
                // insert the data for each task type
                foreach (KeyValuePair<eTaskType, TaskBase> task in _taskListTasks)
                {
                    taskType = task.Key;
                    _task = task.Value;

                    // if new task list, set the task list key before saving
                    if (newTaskList)
                    {
                        _task.UpdateTaskListKey(
                                taskListKey: _taskListProfile.Key
                                );
                    }

                    _task.TaskSaveData(
                        scheduleDataLayer: ScheduleDataLayer,
                        cloneDates: cloneDates,
                        message: ref message
                        );
                }

                ScheduleDataLayer.CommitData();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                ScheduleDataLayer.CloseUpdateConnection();
            }

            // build a new task list node to update the task list explorer 
            List<ROTreeNodeOut> taskListNode = TaskListUtilities.BuildTaskListNode(
                profileType: eProfileType.TaskList, 
                taskListProfile: _taskListProfile
                );

            _taskListProfile.LockStatus = TaskListUtilities.UnLockItem(
                    sessionAddressBlock: SessionAddressBlock,
                    profileType: eProfileType.TaskList,
                    Key: _taskListProfile.Key,
                    message: out message
                    );

            return new ROIListOut(eROReturnCode.Successful, message, ROInstanceID, taskListNode);
        }

        /// <summary>
        /// Update the task list key in the data tables
        /// </summary>
        /// <param name="taskListKey">The key for the task list</param>
        private void UpdateTaskListKey(
            int taskListKey
            )
        {
            if (_tasksDataTable != null)
            {
                foreach (DataRow taskDataRow in _tasksDataTable.Rows)
                {
                    taskDataRow["TASKLIST_RID"] = taskListKey;
                }
                _tasksDataTable.AcceptChanges();
            }
        }

        /// <summary>
        /// Update the sequence in the main task data table
        /// </summary>
        /// <param name="oldSequence">The current sequence number</param>
        /// <param name="newSequence">The new sequence number</param>
        private bool TaskUpdateSequence(
            int oldSequence,
            int newSequence
            )
        {
            string selectString = "TASK_SEQUENCE=" + oldSequence;
            if (_tasksDataTable != null)
            {
                DataRow[] taskDataRows = _tasksDataTable.Select(selectString);
                foreach (var taskDataRow in taskDataRows)
                {
                    taskDataRow["TASK_SEQUENCE"] = newSequence;
                }
                _tasksDataTable.AcceptChanges();
            }

            return true;
        }

        private void SetTaskData(
            ROTaskListPropertiesParms taskListParameters
            )
        {
            // Get task values from database for types that have not been accessed
            // They are needed based on the way all old values are deleted for a task list
            // and then inserted
            foreach (ROTaskProperties task in taskListParameters.ROTaskListProperties.Tasks)
            {
                if (!_taskListTasks.TryGetValue(task.TaskType, out _task))
                {
                    _task = GetTaskClass(
                        taskType: task.TaskType,
                        taskListProperties: _taskListProperties,
                        getNewClass: true
                        );

                    _task.TaskGetValues();
                    _taskListTasks[_task.TaskType] = _task;
                }
            }
        }

        private int CreateTaskList(
            ROTaskListPropertiesParms taskListParameters,
            int folderKey
            )
        {
            _taskListProfile.UserRID = taskListParameters.ROTaskListProperties.UserKey;
            _taskListProfile.OwnerUserRID = taskListParameters.ROTaskListProperties.UserKey;
            _taskListProfile.Key = ScheduleDataLayer.TaskList_Insert(_taskListProfile, _taskListProfile.UserRID);

            FolderDataLayer.OpenUpdateConnection();

            try
            {
                FolderDataLayer.Folder_Item_Insert(folderKey, _taskListProfile.Key, eProfileType.TaskList);
                FolderDataLayer.CommitData();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                FolderDataLayer.CloseUpdateConnection();
            }

            return _taskListProfile.Key;
        }

        private void DeleteTaskListTasks(
            )
        {
            ScheduleDataLayer.TaskForecastDetail_Delete(_taskListProfile.Key);
            ScheduleDataLayer.TaskForecast_Delete(_taskListProfile.Key);
            ScheduleDataLayer.TaskAllocateDetail_Delete(_taskListProfile.Key);
            ScheduleDataLayer.TaskAllocate_Delete(_taskListProfile.Key);
            ScheduleDataLayer.TaskRollup_Delete(_taskListProfile.Key);
            ScheduleDataLayer.TaskSizeCurveMethod_Delete(_taskListProfile.Key);
            ScheduleDataLayer.TaskSizeCurves_Delete(_taskListProfile.Key);
            ScheduleDataLayer.TaskSizeCurveGenerateNode_Delete(_taskListProfile.Key);
            ScheduleDataLayer.TaskSizeDayToWeekSummary_Delete(_taskListProfile.Key);
            ScheduleDataLayer.TaskPosting_Delete(_taskListProfile.Key);
            ScheduleDataLayer.TaskProgram_Delete(_taskListProfile.Key);
            ScheduleDataLayer.TaskBatchComp_Delete(_taskListProfile.Key);  
            ScheduleDataLayer.TaskHeaderReconcile_Delete(_taskListProfile.Key);    
            ScheduleDataLayer.Task_Delete(_taskListProfile.Key);
        }

        /// <summary>
        /// Update memory only with task list properties
        /// </summary>
        /// <param name="TaskListParameters">Contains the task list properties to apply</param>
        /// <returns></returns>
        public ROOut ApplyTaskList(
        ROTaskListPropertiesParms taskListParameters
        )
        {
            string message = null;

            if (_taskListProfile == null)
            {
                message = SessionAddressBlock.ClientServerSession.Audit.GetText(
                    messageCode: eMIDTextCode.msg_ValueWasNotFound,
                    addToAuditReport: true,
                    args: new object[] { "Task List" }
                    );
                throw new MIDException(eErrorLevel.severe,
                    (int)eMIDTextCode.msg_ValueWasNotFound,
                    message);
            }

            // Build new object with updated values
            ROProfileKeyParms taskListGetParameters = new ROProfileKeyParms(
                sROUserID: taskListParameters.ROUserID, 
                sROSessionID: taskListParameters.ROSessionID, 
                ROClass: taskListParameters.ROClass,
                RORequest: eRORequest.GetTaskList, 
                ROInstanceID: taskListParameters.ROInstanceID, 
                profileType: eProfileType.TaskList,
                key: taskListParameters.ROTaskListProperties.TaskList.Key, 
                readOnly: false
                );
            return GetTaskList(taskListParameters: taskListGetParameters, processingApply: true);
        }

        /// <summary>
        /// Determines if the task list name is a duplicate and obtains a unique name if it is
        /// </summary>
        private string CleanseTaskListName(
            ROTaskListPropertiesParms taskListParameters
            )
        {
            int key = taskListParameters.ROTaskListProperties.TaskList.Key;

            string name = taskListParameters.ROTaskListProperties.TaskList.Value;

            int userKey = taskListParameters.ROTaskListProperties.UserKey;

            int nameCounter = 0;
            while (true)
            {
                if (!isDuplicateName(name, userKey, key))
                {
                    break;
                }
                else
                {
                    nameCounter++;
                    name = Include.GetNewName(name: name, index: nameCounter);
                }
            }

            return name.Trim();
        }

        /// <summary>
        /// Check the database to see if the name already exists
        /// </summary>
        /// <param name="name">The name of the task list</param>
        /// <param name="userKey">The owner of the task list</param>
        /// <param name="key">The key of the task list</param>
        /// <returns></returns>
        private bool isDuplicateName(string name, int userKey, int key)
        {
            int tasklistKey = ScheduleDataLayer.TaskList_GetKey(name, userKey);

            // if name and key are the same, then not a duplicate
            if (tasklistKey != -1
                && tasklistKey != key)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Request the task list is processed
        /// </summary>
        /// <param name="TaskListParameters">The parameters containing which task list to be processed</param>
        /// <returns></returns>
        public ROOut ProcessTaskList(
            ROProfileKeyParms TaskListParameters
            )
        {
            eROReturnCode returnCode = eROReturnCode.Successful;
            string message = null;

            

            return new RONoDataOut(returnCode, message, ROInstanceID);
        }

        /// <summary>
        /// Makes a copy of the task list
        /// </summary>
        /// <param name="TaskListParameters">The parameters containing which task list to be copied</param>
        /// <returns></returns>
        public ROOut CopyTaskList(
            ROProfileKeyParms TaskListParameters
            )
        {
            eROReturnCode returnCode = eROReturnCode.Successful;
            string message = null;

            return new ROTaskListPropertiesOut(returnCode, message, ROInstanceID, null);
        }

        /// <summary>
        /// Deletes the task list
        /// </summary>
        /// <param name="taskListParameters">The parameters containing which task list to be deleted</param>
        /// <returns></returns>
        public ROOut DeleteTaskList(ROProfileKeyParms taskListParameters)
        {
            string message = null;
            try
            {
                if (TaskListDeleted(taskListParameters, ref message))
                {
                    return new RONoDataOut(eROReturnCode.Successful, message, ROInstanceID);
                }
                else
                {
                    return new RONoDataOut(eROReturnCode.Failure, message, ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        private bool TaskListDeleted(
            ROProfileKeyParms taskListParameters, 
            ref string message
            )
        {
            bool success = true;
            message = null;
            DataTable scheduledJobsDataTable;
            DataTable jobsDataTable;
            DataRow[] runningJobs;
            bool invalidJobStatusFound;

            if (taskListParameters.Key != Include.NoRID)
            {
                if (_taskListProfile == null)
                {
                    _taskListProfile = new TaskListProfile(ScheduleDataLayer.TaskList_Read(taskListParameters.Key));
                    if (!_taskListProfile.isFound)
                    {
                        MIDEnvironment.Message = SessionAddressBlock.ClientServerSession.Audit.GetText(
                            messageCode: eMIDTextCode.msg_ValueWasNotFound,
                            addToAuditReport: true,
                            args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow) }
                            );
                        MIDEnvironment.requestFailed = true;
                        return false;
                    }
                }
            }

            if (taskListParameters.Key != _taskListProfile.Key)
            {
                MIDEnvironment.Message = SessionAddressBlock.ClientServerSession.Audit.GetText(
                       messageCode: eMIDTextCode.msg_CallParmsDoNotMatchInstance,
                       addToAuditReport: true
                       );
                MIDEnvironment.requestFailed = true;
                return false;
            }

            if (!FunctionSecurity.AllowDelete
                || _taskListProfile.LockStatus == eLockStatus.ReadOnly
                || !ApplicationUtilities.AllowDeleteFromInUse(
                    key: _taskListProfile.Key, 
                    profileType: _taskListProfile.ProfileType, 
                    SAB: SessionAddressBlock)
                    )
            {
                message = string.Empty;
                if (!FunctionSecurity.AllowDelete)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                    ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                }
                else if (_taskListProfile.LockStatus == eLockStatus.ReadOnly)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_DataIsReadOnly);
                    ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    _taskListProfile.LockStatus = TaskListUtilities.LockItem(
                            sessionAddressBlock: SessionAddressBlock,
                            profileType: _taskListProfile.ProfileType,
                            changeType: eChangeType.update,
                            Key: _taskListProfile.Key,
                            Name: _taskListProfile.Name,
                            allowReadOnly: true,
                            message: out message
                    );
                    MIDEnvironment.Message = message;
                }
                return false;
            }

            if (_taskListProfile.LockStatus != eLockStatus.Locked)
            {
                _taskListProfile.LockStatus = TaskListUtilities.LockItem(
                    sessionAddressBlock: SessionAddressBlock,
                    profileType: _taskListProfile.ProfileType,
                    changeType: eChangeType.update,
                    Key: _taskListProfile.Key,
                    Name: _taskListProfile.Name,
                    allowReadOnly: true,
                    message: out message
                    );
                if (_taskListProfile.LockStatus != eLockStatus.Locked)
                {
                    MIDEnvironment.Message = message;
                    return false;
                }
            }

            try
            {
                scheduledJobsDataTable = ScheduleDataLayer.ReadNonSystemJobsByTaskList(_taskListProfile.Key);

                if (scheduledJobsDataTable.Rows.Count > 0)
                {
                    message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse);
                    return false;
                }

                scheduledJobsDataTable = ScheduleDataLayer.ReadScheduledSystemJobsByTaskList(_taskListProfile.Key);

                if (scheduledJobsDataTable.Rows.Count > 0)
                {
                    if (SessionAddressBlock.SchedulerServerSession == null)
                    {
                        message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ScheduleSessionNotAvailable);
                        return false;
                    }
                    else
                    {
                        string selectStatement = "EXECUTION_STATUS = " 
                            + (int)eProcessExecutionStatus.Running 
                            + " OR EXECUTION_STATUS = " 
                            + (int)eProcessExecutionStatus.Queued;
                        runningJobs = scheduledJobsDataTable.Select(selectStatement);

                        if (runningJobs.Length > 0)
                        {
                            message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CannotDeleteRunningTaskList);
                            return false;
                        }

                        runningJobs = scheduledJobsDataTable.Select(
                            "EXECUTION_STATUS = " 
                            + (int)eProcessExecutionStatus.Executed 
                            + " OR EXECUTION_STATUS = " 
                            + (int)eProcessExecutionStatus.OnHold 
                            + " OR EXECUTION_STATUS = " 
                            + (int)eProcessExecutionStatus.Waiting);

                        if (runningJobs.Length > 0)
                        {
                            message = MIDText.GetText(eMIDTextCode.msg_TaskListIsScheduled);
                            return false;
                        }
                    }
                }

                jobsDataTable = ScheduleDataLayer.ReadSystemJobsByTaskList(_taskListProfile.Key);

                ScheduleDataLayer.OpenUpdateConnection();
                FolderDataLayer.OpenUpdateConnection();

                try
                {
                    if (scheduledJobsDataTable.Rows.Count > 0)
                    {
                        invalidJobStatusFound = SessionAddressBlock.SchedulerServerSession.DeleteSchedulesFromList(scheduledJobsDataTable);
                    }
                    else
                    {
                        invalidJobStatusFound = false;
                    }

                    if (!invalidJobStatusFound)
                    {
                        if (jobsDataTable.Rows.Count > 0)
                        {
                            ScheduleDataLayer.JobTaskListJoin_DeleteSystemFromList(jobsDataTable);
                            ScheduleDataLayer.Job_DeleteSystemFromList(jobsDataTable);
                        }

                        FolderDataLayer.Folder_Item_Delete(_taskListProfile.Key, eProfileType.TaskList);
                        FolderDataLayer.Folder_Shortcut_DeleteAll(_taskListProfile.Key, eProfileType.TaskList);
                        ScheduleDataLayer.TaskList_Delete(_taskListProfile.Key);

                        ScheduleDataLayer.CommitData();
                        FolderDataLayer.CommitData();
                    }
                    else
                    {
                        message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse);
                        return false;
                    }
                }
                catch (DatabaseForeignKeyViolation)
                {
                    message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse);
                    return false;
                }
                catch (Exception error)
                {
                    message = error.ToString();
                    throw;
                }
                finally
                {
                    ScheduleDataLayer.CloseUpdateConnection();
                    FolderDataLayer.CloseUpdateConnection();
                }
            }
            catch (Exception error)
            {
                message = error.ToString();
                throw;
            }
            finally
            {
                _taskListProfile.LockStatus = TaskListUtilities.UnLockItem(
                    sessionAddressBlock: SessionAddressBlock,
                    profileType: eProfileType.TaskList,
                    Key: _taskListProfile.Key,
                    message: out message
                    );
                if (_taskListProfile.LockStatus == eLockStatus.Cancel)
                {
                    success = false;
                }
            }

            return success;
        }

        #endregion TaskList Processing


        #region SaveAs TaskList/TaskList
        /// <summary>
        /// Save a task list or job as a different name
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>

        public ROOut SaveAs(
            RODataExplorerSaveAsParms parms
            )
        {
            try
            {
                if (parms.ProfileType == eProfileType.TaskList)
                {
                    if (TaskListSaveAs(parms))
                    {
                        return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
                    }
                    else
                    {
                        return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                    }
                }
                else
                {
                    return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        private bool TaskListSaveAs(
            RODataExplorerSaveAsParms parms
            )
        {
            return true;
        }

        #endregion Save As TaskList/TaskList



        private bool isDuplicateName(
            ref string message
            )
        {
            bool duplicateFound = false;
            
            return duplicateFound;
        }


        #region "Task Processing"

        /// <summary>
        /// Get the details for the task
        /// </summary>
        /// <param name="taskParameters">The keys for the task to get</param>
        /// <param name="processingApply">A flag identifying if an apply is being processed</param>
        /// <returns></returns>
        public ROOut GetTask(
            ROTaskParms taskParameters, 
            bool processingApply = false
            )
        {
            string message = null;
            eROReturnCode returnCode = eROReturnCode.Successful;
            bool getNewClass = true;

            // Do not obtain object during Apply since will already have one
            if (!processingApply)
            {
                if (_task == null
                    || _task.TaskType != taskParameters.TaskType)
                {
                    // check if already have task type in the collection.  If not create it.
                    if (!_taskListTasks.TryGetValue(taskParameters.TaskType, out _task))
                    {
                        _task = GetTaskClass(
                            taskType: taskParameters.TaskType, 
                            taskListProperties: _taskListProperties, 
                            getNewClass: getNewClass
                            );
                        _taskListTasks[_task.TaskType] = _task;
                    }
                }
            }

            ROTaskProperties taskProperties = _task.TaskGetData(
                parms: taskParameters, 
                message: ref message, 
                applyOnly: processingApply
                );

            return new ROTaskPropertiesOut(returnCode, message, ROInstanceID, taskProperties);
        }

        /// <summary>
        /// Saves task information to memory
        /// </summary>
        /// <param name="taskParameters">The values for the task</param>
        /// <returns></returns>
        public ROOut SaveTask(ROTaskPropertiesParms taskParameters)
        {
            // tasks cannot be saved to the database without the task list
            // so perform an apply if a save is requested
            return ApplyTask(
                taskParameters: taskParameters,
                applyOnly: false
                );
        }

        /// <summary>
        /// Saves task information to memory only
        /// </summary>
        /// <param name="taskParameters">The values for the task</param>
        /// <param name="applyOnly">Indicates an apply is being performed rather than a save</param>
        /// <returns></returns>
        public ROOut ApplyTask(ROTaskPropertiesParms taskParameters, bool applyOnly = true)
        {
            string message = null;
            bool successful;
            bool getNewClass = true;

            if (_task == null
                || _task.TaskType != taskParameters.ROTaskProperties.TaskType
                )
            {
                // check if already have task type in the collection.  If not create it.
                if (!_taskListTasks.TryGetValue(taskParameters.ROTaskProperties.TaskType, out _task))
                {
                    _task = GetTaskClass(
                        taskType: taskParameters.ROTaskProperties.TaskType,
                        taskListProperties: _taskListProperties,
                        getNewClass: getNewClass
                        );
                    _taskListTasks[_task.TaskType] = _task;
                }
            }

            // add or update task in main tasks data table
            TaskUpdateData(
                task: taskParameters.ROTaskProperties,
                message: ref message
                );

            // Save values to memory only
            ROTaskProperties taskProperties = _task.TaskUpdateData(
                task: taskParameters.ROTaskProperties,
                message: ref message,
                successful: out successful,
                applyOnly: applyOnly
                );

            // add or update the task in the collection
            _taskListTasks[_task.TaskType] = _task;

            // get data from updates
            ROTaskParms taskGetParameters = new ROTaskParms(
                sROUserID: taskParameters.ROUserID,
                sROSessionID: taskParameters.ROSessionID,
                ROClass: taskParameters.ROClass,
                ROInstanceID: taskParameters.ROInstanceID,
                RORequest: eRORequest.GetTask,
                taskType: taskParameters.ROTaskProperties.TaskType,
                sequence: taskParameters.ROTaskProperties.Task.Key                
                );

            //return GetTask(
            //    taskParameters: taskGetParameters,
            //    processingApply: applyOnly
            //    );

            return new ROTaskPropertiesOut(
                ROReturnCode: eROReturnCode.Successful,
                sROMessage: message,
                ROInstanceID: ROInstanceID,
                ROTaskProperties: taskProperties);
        }

        private void TaskUpdateData(
            ROTaskProperties task,
            ref string message)
        {
            // remove the old task entry if one exists
            DeleteTask(
                sequence: task.Task.Key,
                message: ref message
                );

            // get now data row to add task to the data table
            DataRow taskDataRow = _tasksDataTable.NewRow();

            taskDataRow["TASKLIST_RID"] = _taskListProfile.Key;
            taskDataRow["TASK_SEQUENCE"] = task.Task.Key;
            taskDataRow["TASK_TYPE"] = task.TaskType.GetHashCode();
            taskDataRow["MAX_MESSAGE_LEVEL"] = task.MaximumMessageLevel.Key;
            taskDataRow["EMAIL_SUCCESS_FROM"] = task.EmailSuccessFrom;
            taskDataRow["EMAIL_SUCCESS_TO"] = task.EmailSuccessTo;
            taskDataRow["EMAIL_SUCCESS_CC"] = task.EmailSuccessCC;
            taskDataRow["EMAIL_SUCCESS_BCC"] = task.EmailSuccessBCC;
            taskDataRow["EMAIL_SUCCESS_SUBJECT"] = task.EmailFailureSubject;
            taskDataRow["EMAIL_SUCCESS_BODY"] = task.EmailSuccessBody;
            taskDataRow["EMAIL_FAILURE_FROM"] = task.EmailFailureFrom;
            taskDataRow["EMAIL_FAILURE_TO"] = task.EmailFailureTo;
            taskDataRow["EMAIL_FAILURE_CC"] = task.EmailFailureCC;
            taskDataRow["EMAIL_FAILURE_BCC"] = task.EmailFailureBCC;
            taskDataRow["EMAIL_FAILURE_SUBJECT"] = task.EmailFailureSubject;
            taskDataRow["EMAIL_FAILURE_BODY"] = task.EmailFailureBody;
            taskDataRow["USER_RID"] = _taskListProfile.UserRID;
            taskDataRow["ITEM_TYPE"] = eProfileType.TaskList.GetHashCode();
            taskDataRow["ITEM_RID"] = _taskListProfile.Key;
            taskDataRow["OWNER_USER_RID"] = _taskListProfile.OwnerUserRID;

            _tasksDataTable.Rows.Add(taskDataRow);

            _tasksDataTable.AcceptChanges();

            // order the rows in the data table
            _tasksDataTable = _tasksDataTable.DefaultView.ToTable();

        }

        public ROOut DeleteTask(ROTaskParms taskParameters)
        {
            string message = null;
            bool getNewClass = true;
            if (_task == null
                || _task.TaskType != taskParameters.TaskType
               )
            {
                // check if already have task type in the collection.  If not create it.
                if (!_taskListTasks.TryGetValue(taskParameters.TaskType, out _task))
                {
                    _task = GetTaskClass(
                        taskType: taskParameters.TaskType,
                        taskListProperties: _taskListProperties,
                        getNewClass: getNewClass
                        );
                    _taskListTasks[_task.TaskType] = _task;
                }
            }

            DeleteTask(
                sequence: taskParameters.Sequence,
                message: ref message
                );
            _task.TaskDelete(
                taskParameters: taskParameters,
                message: ref message
                );

            return new RONoDataOut(eROReturnCode.Successful, message, ROInstanceID);
        }

        private void DeleteTask(
            int sequence, 
            ref string message
            )
        {
            string selectString = "TASK_SEQUENCE=" + sequence;
            if (_tasksDataTable != null)
            {
                DataRow[] taskDataRows = _tasksDataTable.Select(selectString);
                foreach (var taskDataRow in taskDataRows)
                {
                    taskDataRow.Delete();
                }
                _tasksDataTable.AcceptChanges();
            }
        }


        private TaskBase GetTaskClass(
            eTaskType taskType,
            ROTaskListProperties taskListProperties,
            bool getNewClass = false
            )
        {

            switch (taskType)
            {
                case eTaskType.Allocate:
                    return new TaskAllocate(
                        sessionAddressBlock: _sessionAddressBlock, 
                        ROWebTools: _ROWebTools, 
                        taskListProperties: taskListProperties
                        );

                case eTaskType.HierarchyLoad:
                    return new TaskHierarchyLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.StoreLoad:
                    return new TaskStoreLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.HistoryPlanLoad:
                    return new TaskHistoryPlanLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.ColorCodeLoad:
                    return new TaskColorCodeLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.SizeCodeLoad:
                    return new TaskSizeCodeLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.HeaderLoad:
                    return new TaskHeaderLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.Forecasting:
                    return new TaskForecasting(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.Rollup:
                    return new TaskRollup(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.RelieveIntransit:
                    return new TaskRelieveIntransit(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.Purge:
                    return new TaskPurge(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.ExternalProgram:
                    return new TaskExternalProgram(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.SizeCurveLoad:
                    return new TaskSizeCurveLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.SizeConstraintsLoad:
                    return new TaskSizeConstraintsLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.SizeCurveMethod:
                    return new TaskSizeCurveMethod(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.SizeCurves:
                    return new TaskSizeCurves(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.SizeDayToWeekSummary:
                    return new TaskSizeDayToWeekSummary(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.BuildPackCriteriaLoad:
                    return new TaskBuildPackCriteriaLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.ChainSetPercentCriteriaLoad:
                    return new TaskChainSetPercentCriteriaLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.PushToBackStockLoad:
                    return new TaskPushToBackStockLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.DailyPercentagesCriteriaLoad:
                    return new TaskDailyPercentagesCriteriaLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.StoreEligibilityCriteriaLoad:
                    return new TaskStoreEligibilityCriteriaLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.VSWCriteriaLoad:
                    return new TaskVSWCriteriaLoad(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.HeaderReconcile:
                    return new TaskHeaderReconcile(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.BatchComp:
                    return new TaskBatchCompute(
                        sessionAddressBlock: _sessionAddressBlock,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
            }

            return null;
        }

        #endregion Task Processing


    }


}
