
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Windows;
using MIDRetail.Windows.Controls;

using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace Logility.ROWeb
{
    public partial class ROTaskListExplorer : ROBaseExplorer
    {

        private ROTaskListManager _ROTaskListManager = null;


        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="sessionAddressBlock">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROTaskListExplorer(SessionAddressBlock sessionAddressBlock, ROWebTools ROWebTools)
            : base(sessionAddressBlock, ROWebTools)
		{
            _treeView = new TaskListTreeView();
            _ROTaskListManager = new ROTaskListManager(
                sessionAddressBlock: sessionAddressBlock, 
                applicationSessionTransaction: null, 
                ROWebTools: ROWebTools, 
                ROInstanceID: ROInstanceID
                );
        }

        /// <summary>
        /// Removes all locks and cleans up all necessary memory
        /// </summary>
        override public void CleanUp()
        {
            if (_ROTaskListManager != null)
            {
                _ROTaskListManager.CleanUp();
            }
        }

        /// <summary>
        /// TaskList manager class to perform maintenance on task lists and jobs
        /// </summary>
        public ROTaskListManager ROTaskListManager
        {
            get
            {
                if (_ROTaskListManager == null)
                {
                    _ROTaskListManager = new ROTaskListManager(
                        sessionAddressBlock: SAB, 
                        applicationSessionTransaction: null, 
                        ROWebTools: ROWebTools, 
                        ROInstanceID: ROInstanceID
                        );
                }
                return _ROTaskListManager;
            }
        }

        /// <summary>
        /// Process request
        /// </summary>
        /// <param name="Parms">Abstracted class containing parameters for the specific request</param>
        /// <returns></returns>
        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                // Explorer
                case eRORequest.GetTaskListExplorerData:
                    return GetExplorerData(Parms: (ROTreeNodeParms)Parms);
                case eRORequest.RefreshExplorerData:
                    return RefreshExplorerData(Parms: (ROTreeNodeParms)Parms);

                case eRORequest.AddFolder:
                    return AddFolder((RODataExplorerFolderParms)Parms);
                case eRORequest.DeleteFolder:
                    return DeleteFolder((RODataExplorerFolderParms)Parms);
                case eRORequest.Rename:
                    return Rename((RODataExplorerRenameParms)Parms);
                case eRORequest.Copy:
                    return Copy((RODataExplorerCopyParms)Parms);
                case eRORequest.AddShortCut:
                    return AddShortCut((RODataExplorerShortcutParms)Parms);
                case eRORequest.DeleteShortCut:
                    return DeleteShortCut((RODataExplorerShortcutParms)Parms);
                case eRORequest.SaveAs:
                    return GetTaskListManager((ROBaseUpdateParms)Parms).SaveAs((RODataExplorerSaveAsParms)Parms);
                case eRORequest.Delete:
                    return DeleteExplorerData(Parms: (ROTreeNodeParms)Parms);

                // TaskLists
                case eRORequest.GetTaskList:
                    return ROTaskListManager.GetTaskList((ROProfileKeyParms)Parms);
                case eRORequest.SaveTaskList:
                    return ROTaskListManager.SaveTaskList((ROTaskListPropertiesParms)Parms, false);
                case eRORequest.SaveAsTaskList:
                    return ROTaskListManager.SaveTaskList((ROTaskListPropertiesParms)Parms, true);
                case eRORequest.ApplyTaskList:
                    return ROTaskListManager.ApplyTaskList((ROTaskListPropertiesParms)Parms);
                case eRORequest.ProcessTaskList:
                    return ROTaskListManager.ProcessTaskList((ROProfileKeyParms)Parms);
                case eRORequest.CopyTaskList:
                    return ROTaskListManager.CopyTaskList((ROProfileKeyParms)Parms);
                case eRORequest.DeleteTaskList:
                    return ROTaskListManager.DeleteTaskList((ROProfileKeyParms)Parms);

                // Tasks
                case eRORequest.GetListOfTasks:
                    return ROTaskListManager.GetListOfTasks();
                case eRORequest.GetTask:
                    return ROTaskListManager.GetTask((ROTaskParms)Parms);
                case eRORequest.SaveTask:
                    return ROTaskListManager.SaveTask((ROTaskPropertiesParms)Parms);
                case eRORequest.ApplyTask:
                    return ROTaskListManager.ApplyTask((ROTaskPropertiesParms)Parms);
                case eRORequest.DeleteTask:
                    return ROTaskListManager.DeleteTask((ROTaskParms)Parms);
                case eRORequest.GetVersions:
                    return ROTaskListManager.GetForecastVersions();
                    // Jobs
            }

            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

        /// <summary>
        /// Refreshes the Task List manager if the data changes
        /// </summary>
        /// <param name="updateParms">The parameters for the requested data</param>
        /// <returns></returns>
        public ROTaskListManager GetTaskListManager(ROBaseUpdateParms updateParms)
        {
            if (_ROTaskListManager != null
                && _ROTaskListManager.ProfileType == updateParms.ProfileType
                && _ROTaskListManager.Key == updateParms.Key
                )
            {
                return _ROTaskListManager;
            }


            if (_ROTaskListManager == null || _ROTaskListManager.Key != updateParms.Key)
            {
                if (_ROTaskListManager != null)
                {
                    _ROTaskListManager.CleanUp();
                    _ROTaskListManager = null;
                }
                _ROTaskListManager = new ROTaskListManager(
                    sessionAddressBlock: SAB, 
                    ROWebTools: ROWebTools, 
                    ROInstanceID: ROInstanceID,
                    applicationSessionTransaction: null
                    );
            }
            else
            {
                throw new Exception("Unable to determine Task List Manager Type");
            }

            return _ROTaskListManager;
        }


        #region "abstract overrides"
        /// <summary>
        /// Virtual method that is called to refresh the entire explorer data
        /// </summary>

        override protected void RefreshTreeView()
        {
            try
            {
                _treeView.LoadNodes();
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Virtual method that is called to refresh a section of the explorer data
        /// </summary>
        /// <param name="node">The node that identifies the branch to refresh</param>
        override protected void RefreshTreeView(
            TreeNode node
            )
        {
            try
            {
                node.Nodes.Clear();
                ((MIDTreeNode)node).ChildrenLoaded = false;
                _treeView.ExpandNode(node);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get the folder type for the item
        /// </summary>
        /// <param name="parentProfileType">The profile type of the parent of the item</param>
        /// <param name="parentKey">The key of the parent</param>
        /// <param name="parentUserKey">The key of the user for the parent of the item</param>
        /// <param name="profileType">The profile type of the item</param>
        /// <param name="uniqueID">The uniqueID of the item</param>
        /// <returns>The eProfileType of the folder</returns>
        override protected eProfileType GetFolderType(
            eProfileType parentProfileType, 
            int parentKey, 
            int parentUserKey, 
            eProfileType profileType, 
            string uniqueID
            )
        {
            try
            {
                return parentProfileType;
            }
            catch
            {
                throw;
            }
        }

        #endregion

    }
}
