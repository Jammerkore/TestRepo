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
    public class TaskRelieveIntransit : TaskBase
    {
        //=======
        // FIELDS
        //=======
        private DataTable _taskData = null;

        //=============
        // CONSTRUCTORS
        //=============
        public TaskRelieveIntransit(
            SessionAddressBlock SAB, 
            ROWebTools ROWebTools,
            ROTaskListProperties taskListProperties
            ) 
            :
            base(
                SAB: SAB, 
                ROWebTools: ROWebTools, 
                taskType: eTaskType.RelieveIntransit, 
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
            ROTaskRelieveIntransit ROTask = new ROTaskRelieveIntransit(
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
        private void AddValues(ROTaskParms taskParameters, ROTaskRelieveIntransit task)
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
            ROTaskRelieveIntransit taskRelieveIntransitData = (ROTaskRelieveIntransit)taskData;

            if (SetTask(taskData: taskRelieveIntransitData, message: ref message))
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
            ROTaskRelieveIntransit taskData, 
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
            _taskData = DataLayerSchedule.TaskPosting_ReadByTaskList(aTaskListRID: TaskListKey);
        }
    }
}
