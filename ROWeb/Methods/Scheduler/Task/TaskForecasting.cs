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
    public class TaskForecasting : TaskBase
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============
        public TaskForecasting(
            SessionAddressBlock sessionAddressBlock, 
            ROWebTools ROWebTools,
            ROTaskListProperties taskListProperties
            ) 
            :
            base(
                sessionAddressBlock: sessionAddressBlock, 
                ROWebTools: ROWebTools, 
                taskType: eTaskType.Forecasting, 
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
            ROTaskForecasting ROTask = null;
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
                && TaskListProperties.Tasks[taskParameters.Sequence] is ROTaskForecasting)
            {
                ROTask = (ROTaskForecasting)TaskListProperties.Tasks[taskParameters.Sequence];
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
                ROTask = new ROTaskForecasting(
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
        private void AddValues(ROTaskParms taskParameters, ROTaskForecasting task)
        {
            int merchandiseKey, versionKey, forecastSequence = 0;
            ROTaskForecastMerchandise taskForecastMerchandise;
            KeyValuePair<int, string> merchandise = default(KeyValuePair<int, string>);
            KeyValuePair<int, string> version = default(KeyValuePair<int, string>);
            string selectString;

            // add merchandise to allocate
            selectString = "TASK_SEQUENCE=" + taskParameters.Sequence;
            DataRow[] merchandiseDataRows = TaskData.Select(selectString);
            task.Merchandise.Clear();

            foreach (DataRow dataRow in merchandiseDataRows)
            {
                forecastSequence = Convert.ToInt32(dataRow["FORECAST_SEQUENCE"]);

                merchandise = default(KeyValuePair<int, string>);
                version = default(KeyValuePair<int, string>);

                if (dataRow["HN_RID"] != DBNull.Value)
                {
                    merchandiseKey = Convert.ToInt32(dataRow["HN_RID"]);
                    merchandise = GetName.GetMerchandiseName(
                        nodeRID: merchandiseKey,
                        SAB: SessionAddressBlock
                        );
                }

                if (dataRow["FV_RID"] != DBNull.Value)
                {
                    versionKey = Convert.ToInt32(dataRow["FV_RID"]);
                    version = GetName.GetVersion(
                        versionRID: versionKey,
                        SAB: SessionAddressBlock
                        );
                }

                taskForecastMerchandise = new ROTaskForecastMerchandise(
                    merchandise: merchandise,
                    version: version
                    );

                AddWorkflowValues(
                    taskParameters: taskParameters,
                    taskForecastMerchandise: taskForecastMerchandise,
                    forecastSequence: forecastSequence
                    );

                task.Merchandise.Add(taskForecastMerchandise);

            }

        }

        private void AddWorkflowValues(
            ROTaskParms taskParameters,
            ROTaskForecastMerchandise taskForecastMerchandise,
            int forecastSequence
            )
        {
            int workflowOrMethodKey, executeDateKey;
            bool isWorkflow;
            ROTaskForecastMerchandiseWorkflowMethod forecastMerchandiseWorkflow;
            KeyValuePair<int, string> workflowOrMethod = default(KeyValuePair<int, string>);
            KeyValuePair<int, string> executeDate = default(KeyValuePair<int, string>);
            string selectString;

            // add workflow to merchandise
            selectString = "TASK_SEQUENCE=" + taskParameters.Sequence + " AND FORECAST_SEQUENCE = " + forecastSequence;
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

                forecastMerchandiseWorkflow = new ROTaskForecastMerchandiseWorkflowMethod(
                    workflowOrMethod: workflowOrMethod,
                    isWorkflow: isWorkflow,
                    executeDate: executeDate
                    );

                taskForecastMerchandise.Workflow.Add(forecastMerchandiseWorkflow);
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
            ROTaskForecasting taskForecastingData = (ROTaskForecasting)taskData;

            // get the values from the database if not already retrieved
            if (TaskData == null)
            {
                TaskGetValues();
            }

            if (!SetTask(taskData: taskForecastingData, message: ref message))
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
            ROTaskForecasting taskData, 
            ref string message
            )
        {
            DataRow merchandiseDataRow;
            DataRow workflowDataRow;
            int forecastSequence = 0;

            // delete old entries from data tables so new ones can be added
            DeleteTaskRows(
                sequence: taskData.Task.Key
                );

            if (taskData.Merchandise.Count == 0)
            {
                message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneMerchandiseRequired);
                return false;
            }

            // add merchandise and detail workflow rows to the data tables
            foreach (ROTaskForecastMerchandise taskForecastMerchandise in taskData.Merchandise)
            {
                merchandiseDataRow = TaskData.NewRow();

                merchandiseDataRow["TASKLIST_RID"] = TaskListProperties.TaskList.Key;
                merchandiseDataRow["TASK_SEQUENCE"] = taskData.Task.Key;
                merchandiseDataRow["FORECAST_SEQUENCE"] = forecastSequence;
                if (taskForecastMerchandise.MerchandiseIsSet
                    && taskForecastMerchandise.Merchandise.Key != Include.NoRID)
                {
                    merchandiseDataRow["HN_RID"] = taskForecastMerchandise.Merchandise.Key;
                }
                if (taskForecastMerchandise.VersionIsSet
                    && taskForecastMerchandise.Version.Key != Include.NoRID)
                {
                    merchandiseDataRow["FV_RID"] = taskForecastMerchandise.Version.Key;
                }

                if (taskForecastMerchandise.Workflow.Count == 0)
                {
                    message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneWorkflowRequired);
                    return false;
                }

                int detailSequence = 0;
                foreach (ROTaskForecastMerchandiseWorkflowMethod forecastMerchandiseWorkflow in taskForecastMerchandise.Workflow)
                {
                    workflowDataRow = TaskDetailData.NewRow();

                    workflowDataRow["TASKLIST_RID"] = TaskListProperties.TaskList.Key;
                    workflowDataRow["TASK_SEQUENCE"] = taskData.Task.Key;
                    workflowDataRow["FORECAST_SEQUENCE"] = forecastSequence;
                    workflowDataRow["DETAIL_SEQUENCE"] = detailSequence;
                    if (forecastMerchandiseWorkflow.WorkflowOrMethodIsSet
                        && forecastMerchandiseWorkflow.WorkflowOrMethod.Key != Include.NoRID)
                    {
                        if (forecastMerchandiseWorkflow.IsWorkflow)
                        {
                            workflowDataRow["WORKFLOW_RID"] = forecastMerchandiseWorkflow.WorkflowOrMethod.Key;
                            workflowDataRow["WORKFLOW_METHOD_IND"] = eWorkflowMethodType.Workflow.GetHashCode();
                        }
                        else
                        {
                            workflowDataRow["METHOD_RID"] = forecastMerchandiseWorkflow.WorkflowOrMethod.Key;
                            workflowDataRow["WORKFLOW_METHOD_IND"] = eWorkflowMethodType.Method.GetHashCode();
                        }

                        if (forecastMerchandiseWorkflow.ExecuteDateIsSet
                            && forecastMerchandiseWorkflow.ExecuteDate.Key != Include.NoRID)
                        {
                            workflowDataRow["EXECUTE_CDR_RID"] = forecastMerchandiseWorkflow.ExecuteDate.Key;
                        }
                    }

                    TaskDetailData.Rows.Add(workflowDataRow);

                    ++detailSequence;
                }

                TaskData.Rows.Add(merchandiseDataRow);

                ++forecastSequence;
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
        /// <param name="cloneDates">Flag identifying if calendar dates are to be cloned</param>
        /// <param name="message">Message during processing</param>
        /// <returns></returns>
        override public bool TaskSaveData(
            ScheduleData scheduleDataLayer,
            bool cloneDates,
            ref string message)
        {
            // If save as is being performed, clone date ranges so new task has separate date object
            if (cloneDates)
            {
                CloneDateRanges(
                    dataTable: TaskDetailData,
                    dateColumnName: "EXECUTE_CDR_RID"
                    );
            }

            scheduleDataLayer.TaskForecast_Insert(TaskData);
            scheduleDataLayer.TaskForecastDetail_Insert(TaskDetailData);

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
            TaskData = ScheduleDataLayer.TaskForecast_ReadByTaskList(aTaskListRID: TaskListKey);
            TaskDetailData = ScheduleDataLayer.TaskForecastDetail_ReadByTaskList(aTaskListRID: TaskListKey);
        }
    }
}
