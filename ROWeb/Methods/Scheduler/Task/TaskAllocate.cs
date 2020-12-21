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
        

        //=============
        // CONSTRUCTORS
        //=============
        public TaskAllocate(
            SessionAddressBlock sessionAddressBlock, 
            ROWebTools ROWebTools,
            ROTaskListProperties taskListProperties
            ) 
            :
            base(
                sessionAddressBlock: sessionAddressBlock, 
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
            ROTaskAllocate ROTask = null;
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
                && TaskListProperties.Tasks[taskParameters.Sequence] is ROTaskAllocate)
            {
                ROTask = (ROTaskAllocate)TaskListProperties.Tasks[taskParameters.Sequence];
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
                ROTask = new ROTaskAllocate(
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
            TaskListProperties.Tasks[taskParameters.Sequence] = ROTask;

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
            DataRow[] merchandiseDataRows = TaskData.Select(selectString);

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
                        SAB: SessionAddressBlock
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
            DataRow[] workflowDataRows = TaskDetailData.Select(selectString);

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
                        SAB: SessionAddressBlock
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

            if (!SetTask(taskData: taskAllocateData, message: ref message))
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
            DataRow merchandiseDataRow;
            DataRow workflowDataRow;
            int allocateSequence = 0;

            // delete old entries from data tables so new ones can be added
            DeleteTaskRows(
                sequence: taskData.Task.Key
                );

            // add merchandise and detail workflow rows to the data tables
            foreach (ROTaskAllocateMerchandise taskAllocateMerchandise in taskData.Merchandise)
            {
                merchandiseDataRow = TaskData.NewRow();

                merchandiseDataRow["TASKLIST_RID"] = TaskListProperties.TaskList.Key;
                merchandiseDataRow["TASK_SEQUENCE"] = taskData.Task.Key;
                merchandiseDataRow["ALLOCATE_SEQUENCE"] = allocateSequence;
                if (taskAllocateMerchandise.MerchandiseIsSet
                    && taskAllocateMerchandise.Merchandise.Key != Include.NoRID)
                {
                    merchandiseDataRow["HN_RID"] = taskAllocateMerchandise.Merchandise.Key;
                }
                if (taskAllocateMerchandise.FilterIsSet
                    && taskAllocateMerchandise.Filter.Key != Include.NoRID)
                {
                    merchandiseDataRow["FILTER_RID"] = taskAllocateMerchandise.Filter.Key;
                }

                int detailSequence = 0;
                foreach (ROTaskAllocateMerchandiseWorkflowMethod allocateMerchandiseWorkflow in taskAllocateMerchandise.Workflow)
                {
                    workflowDataRow = TaskDetailData.NewRow();

                    workflowDataRow["TASKLIST_RID"] = TaskListProperties.TaskList.Key;
                    workflowDataRow["TASK_SEQUENCE"] = taskData.Task.Key;
                    workflowDataRow["ALLOCATE_SEQUENCE"] = allocateSequence;
                    workflowDataRow["DETAIL_SEQUENCE"] = detailSequence;
                    if (allocateMerchandiseWorkflow.WorkflowOrMethodIsSet
                        && allocateMerchandiseWorkflow.WorkflowOrMethod.Key != Include.NoRID)
                    {
                        if (allocateMerchandiseWorkflow.IsWorkflow)
                        {
                            workflowDataRow["WORKFLOW_RID"] = allocateMerchandiseWorkflow.WorkflowOrMethod.Key;
                            workflowDataRow["WORKFLOW_METHOD_IND"] = eWorkflowMethodType.Workflow.GetHashCode();
                        }
                        else
                        {
                            workflowDataRow["METHOD_RID"] = allocateMerchandiseWorkflow.WorkflowOrMethod.Key;
                            workflowDataRow["WORKFLOW_METHOD_IND"] = eWorkflowMethodType.Method.GetHashCode();
                        }

                        if (allocateMerchandiseWorkflow.ExecuteDateIsSet)
                        {
                            workflowDataRow["EXECUTE_CDR_RID"] = allocateMerchandiseWorkflow.ExecuteDate.Key;
                        }
                    }

                    TaskDetailData.Rows.Add(workflowDataRow);

                    ++detailSequence;
                }

                TaskData.Rows.Add(merchandiseDataRow);

                ++allocateSequence;
            }

            TaskData.AcceptChanges();
            TaskDetailData.AcceptChanges();

            // order the rows in the data tables
            TaskData = TaskData.DefaultView.ToTable();
            TaskDetailData = TaskDetailData.DefaultView.ToTable();

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
        /// <param name="message">Message during processing</param>
        /// <returns></returns>
        override public bool TaskSaveData(
            ScheduleData scheduleDataLayer,
            ref string message)
        {
            scheduleDataLayer.TaskAllocate_Insert(TaskData);
            scheduleDataLayer.TaskAllocateDetail_Insert(TaskDetailData);

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
            TaskData = ScheduleDataLayer.TaskAllocate_ReadByTaskList(aTaskListRID: TaskListKey);
            TaskData.DefaultView.Sort = "TASK_SEQUENCE, ALLOCATE_SEQUENCE";
            TaskDetailData = ScheduleDataLayer.TaskAllocateDetail_ReadByTaskList(aTaskListRID: TaskListKey);
            TaskDetailData.DefaultView.Sort = "TASK_SEQUENCE, ALLOCATE_SEQUENCE, DETAIL_SEQUENCE";
        }
    }
}
