
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
    public partial class ROWorkflowMethodExplorer : ROBaseExplorer
    {

        private ROWorkflowMethodManager _ROWorkflowMethodManager = null;


        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROWorkflowMethodExplorer(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
		{
            _treeView = new WorkflowMethodTreeView();
        }

        override public void CleanUp()
        {
            if (_ROWorkflowMethodManager != null)
            {
                _ROWorkflowMethodManager.CleanUp();
            }
        }

        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                case eRORequest.GetWorkflowMethodExplorerData:
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
                    return GetWorkflowMethodManager((ROBaseUpdateParms)Parms).SaveAs((RODataExplorerSaveAsParms)Parms);
                case eRORequest.Delete:
                    return DeleteExplorerData(Parms: (ROTreeNodeParms)Parms);
            }

            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

        public ROWorkflowMethodManager GetWorkflowMethodManager(ROBaseUpdateParms updateParms)
        {
            if (_ROWorkflowMethodManager != null
                && _ROWorkflowMethodManager.ProfileType == updateParms.ProfileType
                && _ROWorkflowMethodManager.Key == updateParms.Key
                )
            {
                return _ROWorkflowMethodManager;
            }

            eROApplicationType applicationType = eROApplicationType.All;

            if (updateParms.ProfileType == MIDRetail.DataCommon.eProfileType.Workflow)
            {
                DataTable dtWorkflows = new DataTable();
                WorkflowBaseData workflowData = new WorkflowBaseData();
                DataTable dt = workflowData.GetWorkflow(updateParms.Key);
                if (dt.Rows.Count > 0)
                {
                    eWorkflowType workflow_Type_ID = (eWorkflowType)Convert.ToInt32(dt.Rows[0]["WORKFLOW_TYPE_ID"]);
                    if (workflow_Type_ID == eWorkflowType.Allocation)
                    {
                        applicationType = eROApplicationType.Allocation;
                    }
                    else if (workflow_Type_ID == eWorkflowType.Forecast)
                    {
                        applicationType = eROApplicationType.Forecast;
                    }
                }
            }

            if (applicationType == eROApplicationType.Allocation
                && (_ROWorkflowMethodManager == null || _ROWorkflowMethodManager.ApplicationType != eROApplicationType.Allocation || _ROWorkflowMethodManager.Key != updateParms.Key)
                )
            {
                if (_ROWorkflowMethodManager != null)
                {
                    _ROWorkflowMethodManager.CleanUp();
                    _ROWorkflowMethodManager = null;
                }
                _ROWorkflowMethodManager = new ROAllocationWorkflowMethodManager(SAB: SAB, applicationSessionTransaction: null, ROWebTools: ROWebTools, ROInstanceID: ROInstanceID);
            }
            else if (applicationType == eROApplicationType.Forecast
                && (_ROWorkflowMethodManager == null || _ROWorkflowMethodManager.ApplicationType != eROApplicationType.Forecast || _ROWorkflowMethodManager.Key != updateParms.Key))
            {
                if (_ROWorkflowMethodManager != null)
                {
                    _ROWorkflowMethodManager.CleanUp();
                    _ROWorkflowMethodManager = null;
                }
                _ROWorkflowMethodManager = new ROPlanningWorkflowMethodManager(SAB: SAB, applicationSessionTransaction: null, ROWebTools: ROWebTools, ROInstanceID: ROInstanceID);
            }
            else
            {
                throw new Exception("Unable to determine Workflow Method Manager Type");
            }

            return _ROWorkflowMethodManager;
        }


        #region "abstract overrides"
        /// <summary>
        /// Virtual method that is called to refresh the ExplorerBase TreeView
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

        override protected void RefreshTreeView(TreeNode node)
        {
            FolderDataLayer DlFolder = new FolderDataLayer();
            try
            {
                MIDWorkflowMethodTreeNode groupNode = ((MIDWorkflowMethodTreeNode)node).GetGroupNode();

                if (groupNode.Profile.ProfileType != eProfileType.WorkflowMethodAllocationFolder
                    && groupNode.Profile.ProfileType != eProfileType.WorkflowMethodOTSForcastFolder)
                {
                    ArrayList lookupNode = new ArrayList();
                    lookupNode.Add(node);
                    ArrayList folders = _treeView.GetFolderNodes(lookupNode);
                    foreach (MIDWorkflowMethodTreeNode wmNode in folders)
                    {
                        if (wmNode.Profile.ProfileType == eProfileType.WorkflowMethodAllocationFolder
                            || wmNode.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastFolder)
                        {
                            RebuildBranch(DlFolder, wmNode, wmNode);
                        }
                    }
                }
                else
                {
                    RebuildBranch(DlFolder, node, groupNode);
                }

                //_treeView.ReloadCache(groupNode.UserId, groupNode.Profile.Key);

                //node.Nodes.Clear();
                //MIDWorkflowMethodTreeNode midTreeNode = (MIDWorkflowMethodTreeNode)node;
                //if (midTreeNode.Profile.ProfileType == eProfileType.WorkflowMethodAllocationFolder)
                //{
                //    DlFolder = new FolderDataLayer();
                //    DataTable children = DlFolder.Folder_Children_Read(midTreeNode.UserId, midTreeNode.Key);
                //    ((WorkflowMethodTreeView)midTreeNode.TreeView).BuildAllocationGroupChildren(groupNode, groupNode.Key, groupNode.UserId, groupNode.OwnerUserRID, children, false);
                //}
                //else if (midTreeNode.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastFolder)
                //{
                //    DlFolder = new FolderDataLayer();
                //    DataTable children = DlFolder.Folder_Children_Read(midTreeNode.UserId, midTreeNode.Key);
                //    ((WorkflowMethodTreeView)midTreeNode.TreeView).BuildOTSForecastGroupChildren(groupNode, groupNode.Key, groupNode.UserId, groupNode.OwnerUserRID, children, false);
                //}
                //else
                //{
                //    ((MIDTreeNode)node).ChildrenLoaded = false;
                //    _treeView.ExpandNode(node);
                //}
            }
            catch
            {
                throw;
            }
        }

        private void RebuildBranch(FolderDataLayer DlFolder, TreeNode node, MIDWorkflowMethodTreeNode groupNode)
        {
            try
            {
                 _treeView.ReloadCache(groupNode.UserId, groupNode.Profile.Key);

                node.Nodes.Clear();
                MIDWorkflowMethodTreeNode midTreeNode = (MIDWorkflowMethodTreeNode)node;
                if (midTreeNode.Profile.ProfileType == eProfileType.WorkflowMethodAllocationFolder)
                {
                    DlFolder = new FolderDataLayer();
                    DataTable children = DlFolder.Folder_Children_Read(midTreeNode.UserId, midTreeNode.Key);
                    ((WorkflowMethodTreeView)midTreeNode.TreeView).BuildAllocationGroupChildren(groupNode, groupNode.Key, groupNode.UserId, groupNode.OwnerUserRID, children, false);
                }
                else if (midTreeNode.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastFolder)
                {
                    DlFolder = new FolderDataLayer();
                    DataTable children = DlFolder.Folder_Children_Read(midTreeNode.UserId, midTreeNode.Key);
                    ((WorkflowMethodTreeView)midTreeNode.TreeView).BuildOTSForecastGroupChildren(groupNode, groupNode.Key, groupNode.UserId, groupNode.OwnerUserRID, children, false);
                }
                else
                {
                    ((MIDTreeNode)node).ChildrenLoaded = false;
                    _treeView.ExpandNode(node);
                }
            }
            catch
            {
                throw;
            }
        }

        override protected eProfileType GetFolderType(eProfileType parentProfileType, int parentKey, int parentUserKey, eProfileType profileType, string uniqueID)
        {
            FolderDataLayer dlFolder = new FolderDataLayer();
            eProfileType folderProfileType = eProfileType.None;

            try
            {
                int folderKey = WorkflowMethodUtilities.GetWorkflowMethodFolderRID(applicationFolderType: parentProfileType, folderKey: parentKey, userKey: parentUserKey, profileType: profileType, uniqueID: uniqueID);

                folderProfileType = dlFolder.Folder_GetType(folderKey);


                return folderProfileType;
            }
            catch
            {
                throw;
            }
        }

        #endregion

    }
}
