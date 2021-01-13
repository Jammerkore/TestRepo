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
    public class TaskRollup : TaskBase
    {
        //=======
        // FIELDS
        //=======
        private Dictionary<int, List<HierarchyLevelComboObject>> _levelLists;

        //=============
        // CONSTRUCTORS
        //=============
        public TaskRollup(
            SessionAddressBlock sessionAddressBlock, 
            ROWebTools ROWebTools,
            ROTaskListProperties taskListProperties
            ) 
            :
            base(
                sessionAddressBlock: sessionAddressBlock, 
                ROWebTools: ROWebTools, 
                taskType: eTaskType.Rollup, 
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
            ROTaskRollup ROTask = null;
            ROTaskProperties baseTask = null;
            eMIDMessageLevel MIDMessageLevel = eMIDMessageLevel.Severe;
            string messageLevel, name;

            // get the values from the database if not already retrieved
            if (TaskData == null)
            {
                TaskGetValues();
                _levelLists = new Dictionary<int, List<HierarchyLevelComboObject>>();
            }

            // check if object already converted to derived class.  If so, use it.  Else convert to derived class.
            if (taskParameters.Sequence < TaskListProperties.Tasks.Count
                && TaskListProperties.Tasks[taskParameters.Sequence] is ROTaskRollup)
            {
                ROTask = (ROTaskRollup)TaskListProperties.Tasks[taskParameters.Sequence];
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
                ROTask = new ROTaskRollup(
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
        private void AddValues(ROTaskParms taskParameters, ROTaskRollup task)
        {
            int merchandiseKey, versionKey, rollupSequence = 0, dateRangeKey;
            int hierarchyKey, hierarchyLevelSequence, hierarchyOffsetIndicator, hierarchyOffset, hierarchyLevel;
            ROTaskRollupMerchandise taskRollupMerchandise;
            KeyValuePair<int, string> merchandise = default(KeyValuePair<int, string>);
            KeyValuePair<int, string> version = default(KeyValuePair<int, string>);
            KeyValuePair<int, string> dateRange = default(KeyValuePair<int, string>);
            KeyValuePair<int, string> fromLevel = default(KeyValuePair<int, string>);
            KeyValuePair<int, string> toLevel = default(KeyValuePair<int, string>);
            string selectString;
            List<HierarchyLevelComboObject> levelList;
            HierarchyLevelComboObject hierarchyLevelItem;
            ePlanLevelLevelType planLevelType;
            bool rollPosting, rollReclass, rollHierarchyLevels, rollDayToWeek, rollDay, rollWeek, rollStore, rollChain;
            bool rollStoreToChain, rollIntransit;

            // get the list of versions for which the user is authorized
            ProfileList versionProfList = SessionAddressBlock.ClientServerSession.GetUserForecastVersions();
            foreach (VersionProfile versionProfile in versionProfList)
            {
                task.Versions.Add(new KeyValuePair<int, string>(versionProfile.Key, versionProfile.Description));
            }

            // get rows for the requested sequence from the DataTable
            selectString = "TASK_SEQUENCE=" + taskParameters.Sequence;
            DataRow[] merchandiseDataRows = TaskData.Select(selectString);
            task.Merchandise.Clear();

            // add each merchandise row to Rollup data
            foreach (DataRow dataRow in merchandiseDataRows)
            {
                // get the sequence of this row within the task sequence
                rollupSequence = Convert.ToInt32(dataRow["ROLLUP_SEQUENCE"]);

                // set default values for fields
                merchandise = default(KeyValuePair<int, string>);
                version = default(KeyValuePair<int, string>);
                dateRange = default(KeyValuePair<int, string>);
                fromLevel = default(KeyValuePair<int, string>);
                toLevel = default(KeyValuePair<int, string>);
                merchandiseKey = Include.NoRID;
                hierarchyKey = Include.NoRID;
                hierarchyLevelSequence = Include.Undefined;
                hierarchyOffsetIndicator = Include.Undefined;
                hierarchyOffset = Include.Undefined;
                hierarchyLevel = Include.Undefined;
                planLevelType = ePlanLevelLevelType.Undefined;
                rollPosting = false;
                rollReclass = false;
                rollHierarchyLevels = false;
                rollDayToWeek = false;
                rollDay = false;
                rollWeek = false;
                rollStore = false;
                rollChain = false;
                rollStoreToChain = false;
                rollIntransit = false;

                // set the merchandise if provided
                if (dataRow["HN_RID"] != DBNull.Value)
                {
                    merchandiseKey = Convert.ToInt32(dataRow["HN_RID"]);
                    merchandise = GetName.GetMerchandiseName(
                        nodeRID: merchandiseKey,
                        SAB: SessionAddressBlock
                        );
                }

                // set the version if provided 
                if (dataRow["FV_RID"] != DBNull.Value)
                {
                    versionKey = Convert.ToInt32(dataRow["FV_RID"]);
                    version = GetName.GetVersion(
                        versionRID: versionKey,
                        SAB: SessionAddressBlock
                        );
                }

                // set the date if provided
                if (dataRow["ROLLUP_CDR_RID"] != DBNull.Value)
                {
                    dateRangeKey = Convert.ToInt32(dataRow["ROLLUP_CDR_RID"]);
                    dateRange = GetName.GetCalendarDateRange(
                        calendarDateRID: dateRangeKey,
                        SAB: SessionAddressBlock
                        );
                }

                // build hierarchy level lists for from and to level based on merchandise in the task
                if (merchandiseKey != Include.NoRID)
                {
                    if (!_levelLists.TryGetValue(merchandiseKey, out levelList))
                    {
                        levelList = FillLevelLists(
                            nodeKey: merchandiseKey
                            );
                        _levelLists.Add(merchandiseKey, levelList);
                    }
                }
                else
                {
                    levelList = new List<HierarchyLevelComboObject>();
                }

                // set the from date
                // determine if from level is an organizational hierarchy level or an alternate hierarchy offset
                // based on the FROM_PH_OFFSET_IND
                if (dataRow["FROM_PH_RID"] != DBNull.Value)
                {
                    hierarchyKey = Convert.ToInt32(dataRow["FROM_PH_RID"]);
                }

                // this will be set for an organizational hierarchy entry
                if (dataRow["FROM_PHL_SEQUENCE"] != DBNull.Value)
                {
                    hierarchyLevelSequence = Convert.ToInt32(dataRow["FROM_PHL_SEQUENCE"]);
                }

                // this will be set for an alternate hierarchy entry
                if (dataRow["FROM_OFFSET"] != DBNull.Value)
                {
                    hierarchyOffset = Convert.ToInt32(dataRow["FROM_OFFSET"]);
                }

                // set the appropriate fields to look up the from date is the list based on the level information
                if (dataRow["FROM_PH_OFFSET_IND"] != DBNull.Value)
                {
                    hierarchyOffsetIndicator = Convert.ToInt32(dataRow["FROM_PH_OFFSET_IND"]);
                    if (hierarchyOffsetIndicator == eHierarchyDescendantType.offset.GetHashCode())
                    {
                        planLevelType = ePlanLevelLevelType.LevelOffset;
                        hierarchyLevel = hierarchyOffset;
                    }
                    else
                    {
                        planLevelType = ePlanLevelLevelType.HierarchyLevel;
                        hierarchyLevel = hierarchyLevelSequence;
                    }
                }

                // locate the from level in the list of levels
                // the value used with include the offset in the list and the text for the level
                for (int i = 0; i < levelList.Count; i++)
                {
                    hierarchyLevelItem = (HierarchyLevelComboObject)levelList[i];

                    if (hierarchyLevelItem.PlanLevelLevelType == planLevelType
                        && hierarchyLevelItem.HierarchyRID == hierarchyKey
                        && hierarchyLevelItem.Level == hierarchyLevel
                        )
                    {
                        fromLevel = new KeyValuePair<int, string>(hierarchyLevelItem.LevelIndex, hierarchyLevelItem.ToString());
                        break;
                    }
                }

                // set the to date
                // determine if from level is an organizational hierarchy level or an alternate hierarchy offset
                // based on the TO_PH_OFFSET_IND
                if (dataRow["TO_PH_RID"] != DBNull.Value)
                {
                    hierarchyKey = Convert.ToInt32(dataRow["TO_PH_RID"]);
                }

                // this will be set for an organizational hierarchy entry
                if (dataRow["TO_PHL_SEQUENCE"] != DBNull.Value)
                {
                    hierarchyLevelSequence = Convert.ToInt32(dataRow["TO_PHL_SEQUENCE"]);
                }

                // this will be set for an alternate hierarchy entry
                if (dataRow["TO_OFFSET"] != DBNull.Value)
                {
                    hierarchyOffset = Convert.ToInt32(dataRow["TO_OFFSET"]);
                }

                // set the appropriate fields to look up the from date is the list based on the level information
                if (dataRow["TO_PH_OFFSET_IND"] != DBNull.Value)
                {
                    hierarchyOffsetIndicator = Convert.ToInt32(dataRow["TO_PH_OFFSET_IND"]);
                    if (hierarchyOffsetIndicator == eHierarchyDescendantType.offset.GetHashCode())
                    {
                        planLevelType = ePlanLevelLevelType.LevelOffset;
                        hierarchyLevel = hierarchyOffset;
                    }
                    else
                    {
                        planLevelType = ePlanLevelLevelType.HierarchyLevel;
                        hierarchyLevel = hierarchyLevelSequence;
                    }
                }

                // locate the to level in the list of levels
                // the value used with include the offset in the list and the text for the level
                for (int i = 0; i < levelList.Count; i++)
                {
                    hierarchyLevelItem = (HierarchyLevelComboObject)levelList[i];

                    if (hierarchyLevelItem.PlanLevelLevelType == planLevelType
                        && hierarchyLevelItem.HierarchyRID == hierarchyKey
                        && hierarchyLevelItem.Level == hierarchyLevel
                        )
                    {
                        toLevel = new KeyValuePair<int, string>(hierarchyLevelItem.LevelIndex, hierarchyLevelItem.ToString());
                        break;
                    }
                }

                // flags on the database are held as characters which are converted to boolean
                // if the value is not set, the flag will use the defaulted value from above

                if (dataRow["POSTING_IND"] != DBNull.Value)
                {
                    rollPosting = Include.ConvertCharToBool(Convert.ToChar(dataRow["POSTING_IND"]));
                }

                if (dataRow["RECLASS_IND"] != DBNull.Value)
                {
                    rollReclass = Include.ConvertCharToBool(Convert.ToChar(dataRow["RECLASS_IND"]));
                }

                if (dataRow["HIERARCHY_LEVELS_IND"] != DBNull.Value)
                {
                    rollHierarchyLevels = Include.ConvertCharToBool(Convert.ToChar(dataRow["HIERARCHY_LEVELS_IND"]));
                }

                if (dataRow["DAY_TO_WEEK_IND"] != DBNull.Value)
                {
                    rollDayToWeek = Include.ConvertCharToBool(Convert.ToChar(dataRow["DAY_TO_WEEK_IND"]));
                }

                if (dataRow["DAY_IND"] != DBNull.Value)
                {
                    rollDay = Include.ConvertCharToBool(Convert.ToChar(dataRow["DAY_IND"]));
                }

                if (dataRow["WEEK_IND"] != DBNull.Value)
                {
                    rollWeek = Include.ConvertCharToBool(Convert.ToChar(dataRow["WEEK_IND"]));
                }

                if (dataRow["STORE_IND"] != DBNull.Value)
                {
                    rollStore = Include.ConvertCharToBool(Convert.ToChar(dataRow["STORE_IND"]));
                }

                if (dataRow["CHAIN_IND"] != DBNull.Value)
                {
                    rollChain = Include.ConvertCharToBool(Convert.ToChar(dataRow["CHAIN_IND"]));
                }

                if (dataRow["STORE_TO_CHAIN_IND"] != DBNull.Value)
                {
                    rollStoreToChain = Include.ConvertCharToBool(Convert.ToChar(dataRow["STORE_TO_CHAIN_IND"]));
                }

                if (dataRow["INTRANSIT_IND"] != DBNull.Value)
                {
                    rollIntransit = Include.ConvertCharToBool(Convert.ToChar(dataRow["INTRANSIT_IND"]));
                }

                // create class
                taskRollupMerchandise = new ROTaskRollupMerchandise(
                    merchandise: merchandise,
                    version: version,
                    dateRange: dateRange,
                    fromLevel: fromLevel,
                    toLevel: toLevel,
                    rollPosting: rollPosting,
                    rollReclass: rollReclass,
                    rollHierarchyLevels: rollHierarchyLevels,
                    rollDayToWeek: rollDayToWeek,
                    rollDay: rollDay,
                    rollWeek: rollWeek,
                    rollStore: rollStore,
                    rollChain: rollChain,
                    rollStoreToChain: rollStoreToChain,
                    rollIntransit: rollIntransit
                    );
                

                // add list for merchandise drop down to be used for both from and to levels
                foreach (HierarchyLevelComboObject level in levelList)
                {
                    taskRollupMerchandise.HierarchyLevels.Add(new KeyValuePair<int, string>(level.LevelIndex, level.ToString()));
                }

                // add object to merchandise list
                task.Merchandise.Add(taskRollupMerchandise);

            }
        }

        private List<HierarchyLevelComboObject> FillLevelLists(
            int nodeKey
            )
        {
            HierarchyNodeProfile nodeProfile;
            HierarchyProfile hierarchyProfile;
            int startLevel;
            int i;
            HierarchyProfile mainHierarchyProfile;
            int highestGuestLevel;
            int longestBranchCount;
            int offset;
            ArrayList guestLevels;
            HierarchyLevelProfile hierarchyLevelProfile;
            List<HierarchyLevelComboObject> levelList;

            try
            {
                levelList = new List<HierarchyLevelComboObject>();
                if (nodeKey != Include.NoRID)
                {
                    nodeProfile = SessionAddressBlock.HierarchyServerSession.GetNodeData(nodeKey);
                    hierarchyProfile = SessionAddressBlock.HierarchyServerSession.GetHierarchyData(nodeProfile.HomeHierarchyRID);

                    // Load Level arrays

                    if (hierarchyProfile.HierarchyType == eHierarchyType.organizational)
                    {
                        if (nodeProfile.HomeHierarchyLevel == 0)
                        {
                            levelList.Add(new HierarchyLevelComboObject(
                                levelList.Count,
                                ePlanLevelLevelType.HierarchyLevel,
                                hierarchyProfile.Key,
                                0,
                                hierarchyProfile.HierarchyID
                                ));
                            startLevel = 1;
                        }
                        else
                        {
                            levelList.Add(new HierarchyLevelComboObject(
                                levelList.Count,
                                ePlanLevelLevelType.HierarchyLevel,
                                hierarchyProfile.Key,
                                ((HierarchyLevelProfile)hierarchyProfile.HierarchyLevels[nodeProfile.HomeHierarchyLevel]).Key,
                                ((HierarchyLevelProfile)hierarchyProfile.HierarchyLevels[nodeProfile.HomeHierarchyLevel]).LevelID
                                ));
                            startLevel = nodeProfile.HomeHierarchyLevel + 1;
                        }

                        for (i = startLevel; i <= hierarchyProfile.HierarchyLevels.Count; i++)
                        {
                            levelList.Add(new HierarchyLevelComboObject(
                                levelList.Count,
                                ePlanLevelLevelType.HierarchyLevel,
                                hierarchyProfile.Key, ((HierarchyLevelProfile)hierarchyProfile.HierarchyLevels[i]).Key,
                                ((HierarchyLevelProfile)hierarchyProfile.HierarchyLevels[i]).LevelID
                                ));
                        }
                    }
                    else
                    {
                        mainHierarchyProfile = SessionAddressBlock.HierarchyServerSession.GetMainHierarchyData();
                        highestGuestLevel = SessionAddressBlock.HierarchyServerSession.GetHighestGuestLevel(nodeProfile.Key);
                        guestLevels = SessionAddressBlock.HierarchyServerSession.GetAllGuestLevels(nodeProfile.Key);

                        levelList.Add(new HierarchyLevelComboObject(
                            levelList.Count,
                            ePlanLevelLevelType.HierarchyLevel,
                            hierarchyProfile.Key,
                            0,
                            nodeProfile.NodeID
                            ));
                        startLevel = 1;

                        if (guestLevels.Count == 1)
                        {
                            hierarchyLevelProfile = (HierarchyLevelProfile)guestLevels[0];
                            levelList.Add(new HierarchyLevelComboObject(
                                levelList.Count,
                                ePlanLevelLevelType.HierarchyLevel,
                                mainHierarchyProfile.Key,
                                hierarchyLevelProfile.Key,
                                hierarchyLevelProfile.LevelID
                                ));
                        }

                        longestBranchCount = SessionAddressBlock.HierarchyServerSession.GetLongestBranch(nodeProfile.Key, true);
                        DataTable hierarchyLevels = SessionAddressBlock.HierarchyServerSession.GetHierarchyDescendantLevels(nodeProfile.Key);
                        longestBranchCount = hierarchyLevels.Rows.Count - 1;


                        offset = 0;

                        for (i = 0; i < longestBranchCount; i++)
                        {
                            offset++;
                            levelList.Add(new HierarchyLevelComboObject(
                                levelList.Count,
                                ePlanLevelLevelType.LevelOffset,
                                hierarchyProfile.Key,
                                offset,
                                "+" + offset.ToString()
                                ));
                        }
                    }
                }

                return levelList;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
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
            ROTaskRollup taskRollupData = (ROTaskRollup)taskData;

            // get the values from the database if not already retrieved
            if (TaskData == null)
            {
                TaskGetValues();
            }

            if (!SetTask(
                taskData: taskRollupData,
                applyOnly: applyOnly,
                message: ref message))
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
            ROTaskRollup taskData,
            bool applyOnly,
            ref string message
            )
        {
            DataRow merchandiseDataRow;
            int rollupSequence = 0;
            List<HierarchyLevelComboObject> levelList;

            // delete old entries from data tables so new ones can be added
            DeleteTaskRows(
                sequence: taskData.Task.Key
                );

            if (taskData.Merchandise.Count == 0)
            {
                message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneMerchandiseRequired);
                return false;
            }

            // add merchandise  rows to the data tables
            foreach (ROTaskRollupMerchandise taskRollupMerchandise in taskData.Merchandise)
            {
                // create data row from table
                merchandiseDataRow = TaskData.NewRow();

                // set primary fields for entry
                merchandiseDataRow["TASKLIST_RID"] = TaskListProperties.TaskList.Key;
                merchandiseDataRow["TASK_SEQUENCE"] = taskData.Task.Key;
                merchandiseDataRow["ROLLUP_SEQUENCE"] = rollupSequence;

                if (taskRollupMerchandise.MerchandiseIsSet
                    && taskRollupMerchandise.Merchandise.Key != Include.NoRID)
                {
                    merchandiseDataRow["HN_RID"] = taskRollupMerchandise.Merchandise.Key;
                }
                else if (!applyOnly) // only validate during save
                {
                    message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MerchandiseRequired);
                    return false;
                }

                if (taskRollupMerchandise.VersionIsSet
                    && taskRollupMerchandise.Version.Key != Include.NoRID)
                {
                    merchandiseDataRow["FV_RID"] = taskRollupMerchandise.Version.Key;
                }
                else if (!applyOnly) // only validate during save
                {
                    message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VersionRequired);
                    return false;
                }

                if (taskRollupMerchandise.DateRangeIsSet
                    && taskRollupMerchandise.DateRange.Key != Include.NoRID)
                {
                    merchandiseDataRow["ROLLUP_CDR_RID"] = taskRollupMerchandise.DateRange.Key;
                }
                else if (!applyOnly) // only validate during save
                {
                    message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RollupDateRequired);
                    return false;
                }

                // determine data row value for from level
                // if merchandise is not in the collection, build the list
                if (!_levelLists.TryGetValue(taskRollupMerchandise.Merchandise.Key, out levelList))
                {
                    levelList = FillLevelLists(
                        nodeKey: taskRollupMerchandise.Merchandise.Key
                        );
                    _levelLists.Add(taskRollupMerchandise.Merchandise.Key, levelList);
                }

                // if from level is set, locate corresponding entry in level list and update fields appropriately
                if (taskRollupMerchandise.FromLevelIsSet
                    && taskRollupMerchandise.FromLevel.Key != Include.NoRID)
                {
                    if (taskRollupMerchandise.FromLevel.Key < levelList.Count)
                    {
                        HierarchyLevelComboObject hierarchyLevelItem = levelList[taskRollupMerchandise.FromLevel.Key];
                        merchandiseDataRow["FROM_PH_RID"] = hierarchyLevelItem.HierarchyRID;
                        if (hierarchyLevelItem.PlanLevelLevelType == ePlanLevelLevelType.LevelOffset)
                        {
                            merchandiseDataRow["FROM_PH_OFFSET_IND"] = eHierarchyDescendantType.offset.GetHashCode();
                            merchandiseDataRow["FROM_OFFSET"] = hierarchyLevelItem.Level;
                        }
                        else
                        {
                            merchandiseDataRow["FROM_PH_OFFSET_IND"] = eHierarchyDescendantType.levelType.GetHashCode();
                            merchandiseDataRow["FROM_PHL_SEQUENCE"] = hierarchyLevelItem.Level;
                        }
                    }
                }
                else if (!applyOnly) // only validate during save
                {
                    message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RollupFromLevelRequired);
                    return false;
                }

                // if to level is set, locate corresponding entry in level list and update fields appropriately
                if (taskRollupMerchandise.ToLevelIsSet
                    && taskRollupMerchandise.ToLevel.Key != Include.NoRID)
                {
                    if (taskRollupMerchandise.ToLevel.Key < levelList.Count)
                    {
                        HierarchyLevelComboObject hierarchyLevelItem = levelList[taskRollupMerchandise.ToLevel.Key];
                        merchandiseDataRow["TO_PH_RID"] = hierarchyLevelItem.HierarchyRID;
                        if (hierarchyLevelItem.PlanLevelLevelType == ePlanLevelLevelType.LevelOffset)
                        {
                            merchandiseDataRow["TO_PH_OFFSET_IND"] = eHierarchyDescendantType.offset.GetHashCode();
                            merchandiseDataRow["TO_OFFSET"] = hierarchyLevelItem.Level;
                        }
                        else
                        {
                            merchandiseDataRow["TO_PH_OFFSET_IND"] = eHierarchyDescendantType.levelType.GetHashCode();
                            merchandiseDataRow["TO_PHL_SEQUENCE"] = hierarchyLevelItem.Level;
                        }
                    }
                }
                else if (!applyOnly) // only validate during save
                {
                    message = SessionAddressBlock.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RollupToLevelRequired);
                    return false;
                }

                // set flag fields
                merchandiseDataRow["POSTING_IND"] = Include.ConvertBoolToChar(taskRollupMerchandise.RollPosting);
                merchandiseDataRow["RECLASS_IND"] = Include.ConvertBoolToChar(taskRollupMerchandise.RollReclass);
                merchandiseDataRow["HIERARCHY_LEVELS_IND"] = Include.ConvertBoolToChar(taskRollupMerchandise.RollHierarchyLevels);
                merchandiseDataRow["DAY_TO_WEEK_IND"] = Include.ConvertBoolToChar(taskRollupMerchandise.RollDayToWeek);
                merchandiseDataRow["DAY_IND"] = Include.ConvertBoolToChar(taskRollupMerchandise.RollDay);
                merchandiseDataRow["WEEK_IND"] = Include.ConvertBoolToChar(taskRollupMerchandise.RollWeek);
                merchandiseDataRow["STORE_IND"] = Include.ConvertBoolToChar(taskRollupMerchandise.RollStore);
                merchandiseDataRow["CHAIN_IND"] = Include.ConvertBoolToChar(taskRollupMerchandise.RollChain);
                merchandiseDataRow["STORE_TO_CHAIN_IND"] = Include.ConvertBoolToChar(taskRollupMerchandise.RollStoreToChain);
                merchandiseDataRow["INTRANSIT_IND"] = Include.ConvertBoolToChar(taskRollupMerchandise.RollIntransit);

                TaskData.Rows.Add(merchandiseDataRow);

                ++rollupSequence;
            }

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
        /// <param name="message">Message during processing</param>
        /// <returns></returns>
        override public bool TaskSaveData(
            ScheduleData scheduleDataLayer,
            ref string message)
        {
            scheduleDataLayer.TaskRollup_Insert(TaskData);

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
            TaskData = ScheduleDataLayer.TaskRollup_ReadByTaskList(aTaskListRID: TaskListKey);
        }
    }
}
