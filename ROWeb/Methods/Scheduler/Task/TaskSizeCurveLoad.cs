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
    public class TaskSizeCurveLoad : TaskBase
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============
        public TaskSizeCurveLoad(
            SessionAddressBlock sessionAddressBlock, 
            ROWebTools ROWebTools,
            ROTaskListProperties taskListProperties
            ) 
            :
            base(
                sessionAddressBlock: sessionAddressBlock, 
                ROWebTools: ROWebTools, 
                taskType: eTaskType.SizeCurveLoad, 
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
            ROTaskSizeCurveLoad ROTask = null;
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
                && TaskListProperties.Tasks[taskParameters.Sequence] is ROTaskSizeCurveLoad)
            {
                ROTask = (ROTaskSizeCurveLoad)TaskListProperties.Tasks[taskParameters.Sequence];
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
                ROTask = new ROTaskSizeCurveLoad(
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
        private void AddValues(ROTaskParms taskParameters, ROTaskSizeCurveLoad task)
        {
            
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
            ROTaskSizeCurveLoad taskSizeCurveLoadData = (ROTaskSizeCurveLoad)taskData;

            if (!SetTask(taskData: taskSizeCurveLoadData, message: ref message))
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
            ROTaskSizeCurveLoad taskData, 
            ref string message
            )
        {
            // delete old entries from data tables so new ones can be added
            DeleteTaskRows(
                sequence: taskData.Task.Key
                );

            throw new Exception("Not Implemented");

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
            TaskData = ScheduleDataLayer.TaskPosting_ReadByTaskList(aTaskListRID: TaskListKey);
        }
    }
}
