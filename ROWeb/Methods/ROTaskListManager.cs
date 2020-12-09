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
        private JobProfile _jobProfile = null;

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
        public ROOut GetTasksList()
        {
            try
            {
                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, GetTasksLists());
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
        internal List<KeyValuePair<int, string>> GetTasksLists()
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
            ROProfileKeyParms TaskListParameters, 
            bool processingApply = false
            )
        {
            bool successful;
            string message = null;
            eROReturnCode returnCode = eROReturnCode.Successful;
            ROTaskListProperties taskListProperties = null;

            

            return new ROTaskListPropertiesOut(returnCode, message, ROInstanceID, taskListProperties);
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
            ROProfileKeyParms TaskListGetParm = new ROProfileKeyParms(
                sROUserID: TaskListParameters.ROUserID, 
                sROSessionID: TaskListParameters.ROSessionID, 
                ROClass: TaskListParameters.ROClass,
                RORequest: eRORequest.GetTaskList, 
                ROInstanceID: TaskListParameters.ROInstanceID, 
                profileType: eProfileType.TaskList,
                key: TaskListParameters.ROTaskListProperties.TaskList.Key, 
                readOnly: false
                );
            return GetTaskList(TaskListParameters: TaskListGetParm, processingApply: true);
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
            SetTaskListObject(TaskListParameters);

            

            return new RONoDataOut(eROReturnCode.Successful, message, ROInstanceID);
        }


        /// <summary>
        /// Lock the task list or job for update control
        /// </summary>
        /// <param name="profileType">The eProfileType of the item to be locked</param>
        /// <param name="key">The key of the item to be locked</param>
        /// <param name="changeType">The eChangeType being perform to know if can be read only</param>
        /// <param name="name">The name of the item to be locked for messaging</param>
        /// <param name="allowReadOnly">A flag identifying if the item can be accessed read only</param>
        /// <returns></returns>
        private string LockItem(
            eProfileType profileType, 
            int key, 
            eChangeType changeType, 
            string name, 
            bool allowReadOnly
            )
        {
            string message = null;
            _taskListProfile.LockStatus = TaskListUtilities.LockItem(
                    SAB: SAB,
                    profileType: profileType,
                    changeType: changeType,
                    Key: key,
                    Name: name,
                    allowReadOnly: allowReadOnly,
                    message: out message
                    );
            return message;
        }

        /// <summary>
        /// Sets the memory object to the correct profile
        /// </summary>
        /// <param name="TaskListParameters">The parameters containing which task list to be referenced</param>
        private void SetTaskListObject(
            ROProfileKeyParms TaskListParameters
            )
        {

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


    }

    
}
