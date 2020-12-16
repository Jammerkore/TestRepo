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
        private SessionAddressBlock _SAB;
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
        private TaskListProfile _taskListProfile = null;
        private ROTaskListProperties _taskListProperties = null;
        private JobProfile _jobProfile = null;
        private TaskBase _task = null;
        // Collection to manage tasks within the task list.  Key is task type.
        // All tasks for a single type are contained in the same class
        private Dictionary<eTaskType, TaskBase> _taskListTasks = new Dictionary<eTaskType, TaskBase>();

        /// <summary>
        /// Gets the SessionAddressBlock
        /// </summary>
        protected SessionAddressBlock SAB
        {
            get { return _SAB; }
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
                    _clientTransaction = SAB.ClientServerSession.CreateTransaction();
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
                _applicationSessionTransaction = SAB.ApplicationServerSession.CreateTransaction();
            }
            return _applicationSessionTransaction;
        }

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        /// <param name="ROInstanceID">The instance ID of the session</param>
        /// <param name="applicationSessionTransaction">The transaction to use for processing</param>
        public ROTaskListManager(
            SessionAddressBlock SAB, 
            ROWebTools ROWebTools, 
            long ROInstanceID, 
            ApplicationSessionTransaction applicationSessionTransaction = null
            )
        {
            _SAB = SAB;
            _ROWebTools = ROWebTools;
            _ROInstanceID = ROInstanceID;
            _applicationSessionTransaction = applicationSessionTransaction;
            FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsScheduler);
            // Get the security for the user
            _userSecLvl = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerUserTaskLists);
            _globalSecLvl = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerGlobalTaskLists);
            _systemSecLvl = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerSystemTaskLists);
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
                    SAB: SAB,
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
                    SAB: SAB,
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
                aIsSizeInstalled: _SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled
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
                    SAB: SAB,
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

            // get the task list values from the database
            ScheduleData scheduleData = new ScheduleData();
            _taskListProfile = new TaskListProfile(scheduleData.TaskList_Read(taskListParameters.Key));
            _taskListProperties = new ROTaskListProperties(
                taskList: new KeyValuePair<int, string>(_taskListProfile.Key, _taskListProfile.Name),
                userKey: _taskListProfile.OwnerUserRID
                );

            DataTable tasks = scheduleData.Task_ReadByTaskList(taskListParameters.Key);

            // populate the task items into the task list
            foreach (DataRow dataRow in tasks.Rows)
            {
                sequence = Convert.ToInt32(dataRow["TASK_SEQUENCE"]);
                taskType = (eTaskType)Convert.ToInt32(dataRow["TASK_TYPE"]);
                name = MIDText.GetTextOnly((int)taskType);
                MIDMessageLevel = (eMIDMessageLevel)Convert.ToInt32(dataRow["MAX_MESSAGE_LEVEL"]);
                messageLevel = MIDText.GetTextOnly((int)MIDMessageLevel);

                taskProperties = new ROTaskProperties(
                    task: new KeyValuePair<int, string>(sequence, name),
                    taskType: taskType,
                    maximumMessageLevel: new KeyValuePair<int, string>((int)MIDMessageLevel, messageLevel)
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
            ROTaskListPropertiesParms TaskListParameters
            )
        {
            string message = null;


            List<ROTreeNodeOut> taskListNode = TaskListUtilities.BuildTaskListNode(
                profileType: eProfileType.TaskList, 
                taskListProfile: _taskListProfile
                );
            return new ROIListOut(eROReturnCode.Successful, message, ROInstanceID, taskListNode);
        }

        /// <summary>
        /// Update memory only with task list properties
        /// </summary>
        /// <param name="TaskListParameters">Contains the task list properties to apply</param>
        /// <returns></returns>
        public ROOut ApplyTaskList(
            ROTaskListPropertiesParms TaskListParameters
            )
        {
            string message = null;

            if (_taskListProfile == null)
            {
                message = SAB.ClientServerSession.Audit.GetText(
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
                sROUserID: TaskListParameters.ROUserID, 
                sROSessionID: TaskListParameters.ROSessionID, 
                ROClass: TaskListParameters.ROClass,
                RORequest: eRORequest.GetTaskList, 
                ROInstanceID: TaskListParameters.ROInstanceID, 
                profileType: eProfileType.TaskList,
                key: TaskListParameters.ROTaskListProperties.TaskList.Key, 
                readOnly: false
                );
            return GetTaskList(taskListParameters: taskListGetParameters, processingApply: true);
        }

        /// <summary>
        /// Determines if the task list name is a duplicate and obtains a unique name if it is
        /// </summary>
        private void CleanseTaskListName()
        {
            string name = _taskListProfile.Name;

            int userKey = SAB.ClientServerSession.UserRID;
            
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
        /// <param name="TaskListParameters">The parameters containing which task list to be deleted</param>
        /// <returns></returns>
        public ROOut DeleteTaskList(ROProfileKeyParms TaskListParameters)
        {
            string message = null;

            

            return new RONoDataOut(eROReturnCode.Successful, message, ROInstanceID);
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
        /// Saves task information to the database
        /// </summary>
        /// <param name="taskParameters">The values for the task</param>
        /// <returns></returns>
        public ROOut SaveTask(ROTaskPropertiesParms taskParameters)
        {
            string message = null;
            bool cloneDates = false;
            bool successful;
            bool getNewClass = true;

            if (_task == null
                || taskParameters.ROTaskProperties.Task.Key == Include.NoRID)
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

            if (taskParameters.ROTaskProperties.Task.Key == Include.NoRID)
            {
                cloneDates = true;
            }

           ROTaskProperties taskProperties = _task.TaskUpdateData(
                task: taskParameters.ROTaskProperties,
                cloneDates: cloneDates,
                message: ref message,
                successful: out successful,
                applyOnly: false
                );

            // add or update the task in the collection
            _taskListTasks[_task.TaskType] = _task;

            return new ROTaskPropertiesOut(
                ROReturnCode: eROReturnCode.Successful,
                sROMessage: message,
                ROInstanceID: ROInstanceID,
                ROTaskProperties: taskProperties);
        }

        /// <summary>
        /// Saves task information to memory only
        /// </summary>
        /// <param name="taskParameters">The values for the task</param>
        /// <returns></returns>
        public ROOut ApplyTask(ROTaskPropertiesParms taskParameters)
        {
            string message = null;
            bool cloneDates = false;
            bool successful;
            bool getNewClass = true;

            if (_task == null
                || taskParameters.ROTaskProperties.Task.Key == Include.NoRID)
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

            // Save values to memory only
            ROTaskProperties taskProperties = _task.TaskUpdateData(
                task: taskParameters.ROTaskProperties,
                cloneDates: cloneDates,
                message: ref message,
                successful: out successful,
                applyOnly: true
                );

            // add or update the task in the collection
            _taskListTasks[_task.TaskType] = _task;

            return new ROTaskPropertiesOut(
                ROReturnCode: eROReturnCode.Successful,
                sROMessage: message,
                ROInstanceID: ROInstanceID,
                ROTaskProperties: taskProperties);
        }

        public ROOut DeleteTask(ROTaskParms taskParameters)
        {
            string message = null;
            bool getNewClass = true;
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

            _taskListTasks.Remove(taskParameters.TaskType);

            return new RONoDataOut(eROReturnCode.Successful, message, ROInstanceID);
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
                        SAB: _SAB, 
                        ROWebTools: _ROWebTools, 
                        taskListProperties: taskListProperties
                        );

                case eTaskType.HierarchyLoad:
                    return new TaskHierarchyLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.StoreLoad:
                    return new TaskStoreLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.HistoryPlanLoad:
                    return new TaskHistoryPlanLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.ColorCodeLoad:
                    return new TaskColorCodeLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.SizeCodeLoad:
                    return new TaskSizeCodeLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.HeaderLoad:
                    return new TaskHeaderLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.Forecasting:
                    return new TaskForecasting(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.Rollup:
                    return new TaskRollup(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.RelieveIntransit:
                    return new TaskRelieveIntransit(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.Purge:
                    return new TaskPurge(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.ExternalProgram:
                    return new TaskExternalProgram(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.SizeCurveLoad:
                    return new TaskSizeCurveLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.SizeConstraintsLoad:
                    return new TaskSizeConstraintsLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.SizeCurveMethod:
                    return new TaskSizeCurveMethod(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.SizeCurves:
                    return new TaskSizeCurves(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.SizeDayToWeekSummary:
                    return new TaskSizeDayToWeekSummary(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.BuildPackCriteriaLoad:
                    return new TaskBuildPackCriteriaLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.ChainSetPercentCriteriaLoad:
                    return new TaskChainSetPercentCriteriaLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.PushToBackStockLoad:
                    return new TaskPushToBackStockLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.DailyPercentagesCriteriaLoad:
                    return new TaskDailyPercentagesCriteriaLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.StoreEligibilityCriteriaLoad:
                    return new TaskStoreEligibilityCriteriaLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.VSWCriteriaLoad:
                    return new TaskVSWCriteriaLoad(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.HeaderReconcile:
                    return new TaskHeaderReconcile(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
                case eTaskType.BatchComp:
                    return new TaskBatchCompute(
                        SAB: _SAB,
                        ROWebTools: _ROWebTools,
                        taskListProperties: taskListProperties
                        );
            }

            return null;
        }

        #endregion Task Processing


    }


}
