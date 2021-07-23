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
    public class TaskBuildPackCriteriaLoad : TaskBase
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============
        public TaskBuildPackCriteriaLoad(
            SessionAddressBlock sessionAddressBlock, 
            ROWebTools ROWebTools,
            ROTaskListProperties taskListProperties
            ) 
            :
            base(
                sessionAddressBlock: sessionAddressBlock, 
                ROWebTools: ROWebTools, 
                taskType: eTaskType.BuildPackCriteriaLoad, 
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
            ROTaskBuildPackCriteriaLoad ROTask = null;
            ROTaskProperties baseTask = null;
            eMIDMessageLevel MIDMessageLevel = eMIDMessageLevel.Severe;
            string messageLevel, name;
            Sequence = taskParameters.Sequence;
            // get the values from the database if not already retrieved
            //if (TaskData == null)
            {
                TaskGetValues();
            }

            // check if object already converted to derived class.  If so, use it.  Else convert to derived class.
            if (taskParameters.Sequence < TaskListProperties.Tasks.Count
                && TaskListProperties.Tasks[taskParameters.Sequence] is ROTaskBuildPackCriteriaLoad)
            {
                ROTask = (ROTaskBuildPackCriteriaLoad)TaskListProperties.Tasks[taskParameters.Sequence];
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
                ROTask = new ROTaskBuildPackCriteriaLoad(
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
        private void AddValues(ROTaskParms taskParameters, ROTaskBuildPackCriteriaLoad task)
        {
            if (TaskData.Rows.Count > 0)
            {
                string selectString;
                selectString = "TASK_SEQUENCE=" + taskParameters.Sequence;
                DataRow headerDataRow = TaskData.Select(selectString).First();
                string inputDirectory = Convert.ToString(headerDataRow["INPUT_DIRECTORY"]);
                inputDirectory = string.IsNullOrEmpty(inputDirectory) ?
                    string.IsNullOrEmpty(MIDConfigurationManager.AppSettings["FileDirectory"]) ? @"C:\Logility\ROData\Build Pack Criteria" :
                    string.Concat(MIDConfigurationManager.AppSettings["FileDirectory"], @"\Build Pack Criteria") : inputDirectory;
                task.ProcessingDirection = Convert.ToInt32(headerDataRow["FILE_PROCESSING_DIRECTION"]);
                task.Directory = inputDirectory;
                task.FlagFileSuffix = Convert.ToString(headerDataRow["FILE_MASK"]);
            }
        }


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
            ROTaskBuildPackCriteriaLoad taskBuildPackCriteriaLoadData = (ROTaskBuildPackCriteriaLoad)taskData;
            Sequence = taskData.Task.Key;
            // get the values from the database if not already retrieved
            if (TaskData == null)
            {
                TaskGetValues();
            }

            if (!SetTask(taskData: taskBuildPackCriteriaLoadData, message: ref message))
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
            ROTaskBuildPackCriteriaLoad taskData, 
            ref string message
            )
        {
            // delete old entries from data tables so new ones can be added
            DeleteTaskRows(
                sequence: taskData.Task.Key
                );

            DataRow headerDataRow = TaskData.NewRow();
            headerDataRow["TASKLIST_RID"] = TaskListProperties.TaskList.Key;
            headerDataRow["TASK_SEQUENCE"] = taskData.Task.Key;
            headerDataRow["FILE_PROCESSING_DIRECTION"] = taskData.ProcessingDirection;
            headerDataRow["INPUT_DIRECTORY"] = taskData.Directory;
            headerDataRow["FILE_MASK"] = taskData.FlagFileSuffix;
            TaskData.Rows.Add(headerDataRow);
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
            scheduleDataLayer.TaskPosting_Insert(TaskData);

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
            TaskData = DatabaseSchema.GetTableSchema("TASK_POSTING");
            // Add the data row for the task
            TaskData.ImportRow(ScheduleDataLayer.TaskPosting_Read(aTaskListRID: TaskListKey, aTaskSequence: Sequence));
        }
    }
}
