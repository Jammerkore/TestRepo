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
    public class TaskAllocate : TaskBase
    {
        //=======
        // FIELDS
        //=======
        private DataTable _taskData = null;
        private DataTable _taskDetailData = null;

        //=============
        // CONSTRUCTORS
        //=============
        public TaskAllocate(
            SessionAddressBlock SAB, 
            ROWebTools ROWebTools,
            ROTaskListProperties taskListProperties
            ) 
            :
            base(
                SAB: SAB, 
                ROWebTools: ROWebTools, 
                taskType: eTaskType.Allocate, 
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
            eMIDMessageLevel MIDMessageLevel = eMIDMessageLevel.Severe;
            string messageLevel, name;

            // get the values from the database if not already retrieved
            if (_taskData == null)
            {
                TaskGetValues(taskParameters: taskParameters);
            }

            // get the message level from the task list properties
            if (TaskListProperties.Tasks.Count > taskParameters.Sequence)
            {
                MIDMessageLevel = (eMIDMessageLevel)TaskListProperties.Tasks[taskParameters.Sequence].MaximumMessageLevel.Key;
            }

            name = MIDText.GetTextOnly((int)taskParameters.TaskType);
            messageLevel = MIDText.GetTextOnly((int)MIDMessageLevel);
            KeyValuePair<int, string> task = new KeyValuePair<int, string>(key: taskParameters.Sequence, value: name);
            KeyValuePair<int, string> maximumMessageLevel = new KeyValuePair<int, string>((int)MIDMessageLevel, messageLevel);
            ROTaskAllocate ROTask = new ROTaskAllocate(
                task: task,
                maximumMessageLevel: maximumMessageLevel
                );

            // populate using Windows\TaskListProperties.cs as a reference

            AddValues(taskParameters: taskParameters, task: ROTask);

            return ROTask;
        }

        /// <summary>
        /// Populate the data class with the task values
        /// </summary>
        /// <param name="taskParameters">The parameters of the task to build</param>
        /// <param name="task">The data class of the task</param>
        private void AddValues(
            ROTaskParms taskParameters, 
            ROTaskAllocate task
            )
        {
            int merchandiseKey, filterKey, allocateSequence = 0;
            ROTaskAllocateMerchandise taskAllocateMerchandise;
            KeyValuePair<int, string> merchandise = default(KeyValuePair<int, string>);
            KeyValuePair<int, string> filter = default(KeyValuePair<int, string>);
            string selectString;

            // add merchandise to allocate
            selectString = "TASK_SEQUENCE=" + taskParameters.Sequence;
            DataRow[] merchandiseDataRows = _taskData.Select(selectString);

            foreach (DataRow dataRow in merchandiseDataRows)
            {
                allocateSequence = Convert.ToInt32(dataRow["ALLOCATE_SEQUENCE"]);

                merchandise = default(KeyValuePair<int, string>);
                filter = default(KeyValuePair<int, string>);

                if (dataRow["HN_RID"] != DBNull.Value)
                {
                    merchandiseKey = Convert.ToInt32(dataRow["HN_RID"]);
                    merchandise = GetName.GetMerchandiseName(
                        nodeRID: merchandiseKey,
                        SAB: SAB
                        );
                }

                if (dataRow["FILTER_RID"] != DBNull.Value)
                {
                    filterKey = Convert.ToInt32(dataRow["FILTER_RID"]);
                    filter = GetName.GetFilterName(
                        key: filterKey
                        );
                }

                taskAllocateMerchandise = new ROTaskAllocateMerchandise(
                    merchandise: merchandise,
                    filter: filter
                    );

                AddWorkflowValues(
                    taskParameters: taskParameters,
                    taskAllocateMerchandise: taskAllocateMerchandise,
                    allocateSequence: allocateSequence
                    );

                task.Merchandise.Add(taskAllocateMerchandise);

            }

        }

        /// <summary>
        /// Populate the workflow items for each merchandise
        /// </summary>
        /// <param name="taskParameters">The parameters of the task to build</param>
        /// <param name="taskAllocateMerchandise">The merchandise class of the task</param>
        private void AddWorkflowValues(
            ROTaskParms taskParameters, 
            ROTaskAllocateMerchandise taskAllocateMerchandise,
            int allocateSequence
            )
        {
            int workflowOrMethodKey, executeDateKey;
            bool isWorkflow;
            ROTaskAllocateMerchandiseWorkflowMethod allocateMerchandiseWorkflow;
            KeyValuePair<int, string> workflowOrMethod = default(KeyValuePair<int, string>);
            KeyValuePair<int, string> executeDate = default(KeyValuePair<int, string>);
            string selectString;

            // add workflow to merchandise
            selectString = "TASK_SEQUENCE=" + taskParameters.Sequence + " AND ALLOCATE_SEQUENCE = " + allocateSequence;
            DataRow[] workflowDataRows = _taskDetailData.Select(selectString);

            foreach (DataRow dataRow in workflowDataRows)
            {
                workflowOrMethod = default(KeyValuePair<int, string>);
                executeDate = default(KeyValuePair<int, string>);
                isWorkflow = true;

                if (dataRow["WORKFLOW_RID"] != DBNull.Value)
                {
                    workflowOrMethodKey = Convert.ToInt32(dataRow["WORKFLOW_RID"]);
                    workflowOrMethod = GetName.GetWorkflowName(
                        key: workflowOrMethodKey
                        );
                }
                else if (dataRow["METHOD_RID"] != DBNull.Value)
                {
                    workflowOrMethodKey = Convert.ToInt32(dataRow["METHOD_RID"]);
                    workflowOrMethod = GetName.GetMethod(
                        key: workflowOrMethodKey
                        );
                }

                if (dataRow["EXECUTE_CDR_RID"] != DBNull.Value)
                {
                    executeDateKey = Convert.ToInt32(dataRow["EXECUTE_CDR_RID"]);
                    executeDate = GetName.GetCalendarDateRange(
                        calendarDateRID: executeDateKey,
                        SAB: SAB
                        );
                }

                allocateMerchandiseWorkflow = new ROTaskAllocateMerchandiseWorkflowMethod(
                    workflowOrMethod: workflowOrMethod,
                    isWorkflow: isWorkflow,
                    executeDate: executeDate
                    );

                taskAllocateMerchandise.Workflow.Add(allocateMerchandiseWorkflow);
            }

        }


        /// <summary>
        /// Accepts data, updates the memory object and conditionally updates the database if values are being saved and not applied
        /// </summary>
        /// <param name="taskData">The data associated with the task</param>
        /// <param name="cloneDates">Flag identifying if dates should be cloned if task is being copied</param>
        /// <param name="message">Message during processing</param>
        /// <param name="successful">Flag identifying if the process was successful</param>
        /// <param name="applyOnly">Flag identifying if apply is being processed</param>
        /// <returns>The updated task data</returns>
        override public ROTaskProperties TaskUpdateData(
            ROTaskProperties taskData, 
            bool cloneDates, 
            ref string message, 
            out bool successful, 
            bool applyOnly = false
            )
        {
            successful = true;
            ROTaskAllocate taskAllocateData = (ROTaskAllocate)taskData;

            if (SetTask(taskData: taskAllocateData, message: ref message))
            {
                if (!applyOnly)
                {
                    UpdateTask();
                }
            }
            else
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
            ROTaskAllocate taskData, 
            ref string message
            )
        {
            throw new Exception("Not Implemented");

            return true;
        }

        /// <summary>
        /// Updates the database for the task
        /// </summary>
        /// <returns>The status of the update</returns>
        private bool UpdateTask()
        {
            throw new Exception("Not Implemented");

            return true;
        }

        /// <summary>
        /// Deletes the task from the database
        /// </summary>
        /// <param name="key">The key of the task</param>
        /// <param name="message">Message during processing</param>
        /// <returns></returns>
        override public bool TaskDelete(int key, ref string message)
        {
            throw new Exception("Not Implemented");

            return true;
        }


        /// <summary>
        /// Gets the task values from the database
        /// </summary>
        /// <param name="taskParameters"></param>
        /// <returns></returns>
        private void TaskGetValues(ROTaskParms taskParameters)
        {
            _taskData = DataLayerSchedule.TaskAllocate_ReadByTaskList(aTaskListRID: TaskListKey);
            _taskData.DefaultView.Sort = "TASK_SEQUENCE, ALLOCATE_SEQUENCE";
            _taskDetailData = DataLayerSchedule.TaskAllocateDetail_ReadByTaskList(aTaskListRID: TaskListKey);
            _taskDetailData.DefaultView.Sort = "TASK_SEQUENCE, ALLOCATE_SEQUENCE, DETAIL_SEQUENCE";
        }
    }
}
