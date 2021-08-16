using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
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
    public class TaskHeaderReconcile : TaskBase
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============
        public TaskHeaderReconcile(
            SessionAddressBlock sessionAddressBlock, 
            ROWebTools ROWebTools,
            ROTaskListProperties taskListProperties
            ) 
            :
            base(
                sessionAddressBlock: sessionAddressBlock, 
                ROWebTools: ROWebTools, 
                taskType: eTaskType.HeaderReconcile, 
                taskListProperties: taskListProperties
                )
        {
           
        }

        //===========
        // PROPERTIES
        //===========



        //========
        // METHODS
        //========

        /// <summary>
        /// Performs data cleanup as the objecct is being closed
        /// </summary>
        /// <returns>The status of the clean up</returns>
        override public bool OnClosing()
        {
            return true;
        }

        /// <summary>
        /// Gets the data and builds the data to send
        /// </summary>
        /// <param name="taskParameters">The parameters of the task to build</param>
        /// <param name="message">Message during processing</param>
        /// <param name="applyOnly">Flag identifying if apply is being processed</param>
        /// <returns></returns>
        override public ROTaskProperties TaskGetData(
            ROTaskParms taskParameters, 
            ref string message, 
            bool applyOnly = false
            )
        {
            ROTaskHeaderReconcile ROTask = null;
            ROTaskProperties baseTask = null;
            eMIDMessageLevel MIDMessageLevel = eMIDMessageLevel.Severe;
            string messageLevel, name;

            // get the values from the database if not already retrieved
            if (TaskData == null)
            {
                TaskGetValues();
            }

            // check if object already converted to derived class.  If so, use it.  Else convert to derived class.
            if (taskParameters.Sequence < TaskListProperties.Tasks.Count
                && TaskListProperties.Tasks[taskParameters.Sequence] is ROTaskHeaderReconcile)
            {
                ROTask = (ROTaskHeaderReconcile)TaskListProperties.Tasks[taskParameters.Sequence];
            }
            else
            {
                // get the message level from the task list properties
                if (taskParameters.Sequence < TaskListProperties.Tasks.Count)
                {
                    baseTask = TaskListProperties.Tasks[taskParameters.Sequence];
                    MIDMessageLevel = (eMIDMessageLevel)baseTask.MaximumMessageLevel.Key;
                }

                name = MIDText.GetTextOnly((int)taskParameters.TaskType);
                messageLevel = MIDText.GetTextOnly((int)MIDMessageLevel);
                KeyValuePair<int, string> task = new KeyValuePair<int, string>(key: taskParameters.Sequence, value: name);
                KeyValuePair<int, string> maximumMessageLevel = new KeyValuePair<int, string>((int)MIDMessageLevel, messageLevel);
                ROTask = new ROTaskHeaderReconcile(
                    task: task,
                    maximumMessageLevel: maximumMessageLevel
                    );

                // copy values from base class to derived class
                if (baseTask != null)
                {
                    ROTask.CopyValuesToDerivedClass(taskProperties: baseTask);
                }
            }

            // populate using Windows\TaskListProperties.cs as a reference

            AddValues(taskParameters: taskParameters, task: ROTask);

            // update task list class with derived class
            UpdateTaskList(ROTask: ROTask, sequence: taskParameters.Sequence);

            return ROTask;
        }

        /// <summary>
        /// Populate the data class with the task values
        /// </summary>
        /// <param name="taskParameters">The parameters of the task to build</param>
        /// <param name="task">The data class of the task</param>
        private void AddValues(ROTaskParms taskParameters, ROTaskHeaderReconcile task)
        {
            string selectString;
            selectString = "TASK_SEQUENCE=" + taskParameters.Sequence;
            DataRow headerDataRow = TaskData.Select(selectString).First();
            string inputDirectory = Convert.ToString(headerDataRow["INPUT_DIRECTORY"]);
            string outputDirectory = Convert.ToString(headerDataRow["OUTPUT_DIRECTORY"]);
            inputDirectory = string.IsNullOrEmpty(inputDirectory) ? TaskDirectory : inputDirectory;            
            outputDirectory = string.IsNullOrEmpty(outputDirectory) ?
                string.IsNullOrEmpty(MIDConfigurationManager.AppSettings["FileDirectory"]) ? @"C:\Logility\ROData\Headers" :
                string.Concat(MIDConfigurationManager.AppSettings["FileDirectory"], @"\Headers") : outputDirectory;
            task.HeaderFileName = Convert.ToString(headerDataRow["HEADER_KEYS_FILE_NAME"]);
            task.HeaderTypes = Convert.ToString(headerDataRow["HEADER_TYPES"]);
            task.InputDirectory = inputDirectory;
            task.OutputDirectory = outputDirectory;
            task.RemoveTransactionFileName = Convert.ToString(headerDataRow["REMOVE_TRANS_FILE_NAME"]);
            task.RemoveTransactionTriggerSuffix = Convert.ToString(headerDataRow["REMOVE_TRANS_TRIGGER_SUFFIX"]);
            task.TriggerSuffix = Convert.ToString(headerDataRow["TRIGGER_SUFFIX"]);
        }

        private string TaskDirectory => string.IsNullOrEmpty(MIDConfigurationManager.AppSettings["FileDirectory"]) ? @"C:\Logility\ROData\Header Reconcile" :
                    string.Concat(MIDConfigurationManager.AppSettings["FileDirectory"], @"\Header Reconcile");

        /// <summary>
        /// Accepts data, updates the memory object and conditionally updates the database if values are being saved and not applied
        /// </summary>
        /// <param name="taskData">The data associated with the task</param>
        /// <param name="message">Message during processing</param>
        /// <param name="successful">Flag identifying if the process was successful</param>
        /// <param name="applyOnly">Flag identifying if apply is being processed</param>
        /// <returns>The updated task data</returns>
        override public ROTaskProperties TaskUpdateData(
            ROTaskProperties taskData, 
            ref string message, 
            out bool successful, 
            bool applyOnly = false
            )
        {
            successful = true;
            ROTaskHeaderReconcile taskHeaderReconcileData = (ROTaskHeaderReconcile)taskData;

            // get the values from the database if not already retrieved
            if (TaskData == null)
            {
                TaskGetValues();
            }

            if (!SetTask(taskData: taskHeaderReconcileData, message: ref message))
            {
                successful = false;
            }


            return taskData;
        }

        /// <summary>
        /// Takes values from input class and updates the memory object
        /// </summary>
        /// <param name="taskData">Input values for the task</param>
        /// <param name="message">Message to return</param>
        private bool SetTask(
            ROTaskHeaderReconcile taskData, 
            ref string message
            )
        {
            // delete old entries from data tables so new ones can be added
            DeleteTaskRows(
                sequence: taskData.Task.Key
                );

            DataRow headerReconcileDataRow = TaskData.NewRow();
            headerReconcileDataRow["TASKLIST_RID"] = TaskListProperties.TaskList.Key;
            headerReconcileDataRow["TASK_SEQUENCE"] = taskData.Task.Key;
            headerReconcileDataRow["INPUT_DIRECTORY"] = TaskDirectory;
            headerReconcileDataRow["OUTPUT_DIRECTORY"] = taskData.OutputDirectory;
            headerReconcileDataRow["TRIGGER_SUFFIX"] = taskData.TriggerSuffix;
            headerReconcileDataRow["REMOVE_TRANS_FILE_NAME"] = taskData.RemoveTransactionFileName;
            headerReconcileDataRow["REMOVE_TRANS_TRIGGER_SUFFIX"] = taskData.RemoveTransactionTriggerSuffix;
            headerReconcileDataRow["HEADER_TYPES"] = taskData.HeaderTypes;
            headerReconcileDataRow["HEADER_KEYS_FILE_NAME"] = taskData.HeaderFileName;
            TaskData.Rows.Add(headerReconcileDataRow);
            TaskData.AcceptChanges();
            // order the rows in the data tables
            TaskData = TaskData.DefaultView.ToTable();

            return true;
        }

        /// <summary>
        /// Update the sequence number of the task
        /// </summary>
        /// <param name="oldSequence">The current sequence number</param>
        /// <param name="newSequence">The new sequence number</param>
        /// <param name="message">Message during processing</param>
        /// <returns></returns>
        override public bool TaskUpdateSequence(
            int oldSequence,
            int newSequence,
            ref string message
            )
        {
            UpdateTaskRowSequence(
                oldSequence: oldSequence,
                newSequence: newSequence
                );

            return true;
        }

        /// <summary>
        /// Saves the task to the database
        /// </summary>
        /// <param name="scheduleDataLayer">The data layer to communicate with the database for schedule tables</param>
        /// <param name="cloneDates">Flag identifying if calendar dates are to be cloned</param>
        /// <param name="message">Message during processing</param>
        /// <returns></returns>
        override public bool TaskSaveData(
            ScheduleData scheduleDataLayer,
            bool cloneDates,
            ref string message)
        {
            scheduleDataLayer.TaskHeaderReconcile_Insert(TaskData);

            return true;
        }

        /// <summary>
        /// Deletes the task from the data table
        /// </summary>
        /// <param name="taskParameters">The parameters of the task to delete</param>
        /// <param name="message">Message during processing</param>
        /// <returns></returns>
        override public bool TaskDelete(ROTaskParms taskParameters, ref string message)
        {
            DeleteTaskRows(
                sequence: taskParameters.Sequence
                );

            return true;
        }


        /// <summary>
        /// Gets the task values from the database
        /// </summary>
        public override void TaskGetValues()
        {
            TaskData = ScheduleDataLayer.TaskHeaderReconcile_ReadByTaskList(aTaskListRID: TaskListKey);

        }
    }
}
