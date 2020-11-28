using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using MIDRetail.Windows;
using MIDRetail.Windows.Controls;

namespace Logility.ROWeb
{
    abstract public class ROBaseExplorer : ROWebFunction
    {
        internal MIDTreeView _treeView = null;
        private bool _treeViewInitialized = false;
        private bool _isWorkflowMethodData = false;

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROBaseExplorer(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {

        }

        override public void CleanUp()
        {

        }

        abstract protected void RefreshTreeView();

        abstract protected void RefreshTreeView(TreeNode node);

        abstract protected eProfileType GetFolderType(eProfileType parentProfileType, int parentKey, int parentUserKey, eProfileType profileType, string uniqueID);


        #region "Method to Get Explorer Data"
        internal ROOut GetExplorerData(ROTreeNodeParms Parms)
        {
            System.Windows.Forms.TreeNodeCollection nodes = null;

            if (!_treeViewInitialized)
            {
                if (_treeView is WorkflowMethodTreeView)
                {
                    ((WorkflowMethodTreeView)_treeView).InitializeTreeView(SAB, false, null, null);
                    _isWorkflowMethodData = true;
                }
                else
                {
                    _treeView.InitializeTreeView(SAB, false, null);
                    _isWorkflowMethodData = false;
                }
                _treeView.LoadNodes();
                nodes = _treeView.Nodes;
                _treeViewInitialized = true;
            }
            else if (Parms.ProfileType == eProfileType.None && Parms.Key == Include.NoRID)
            {
                nodes = _treeView.Nodes;
            }
            else
            {
                MIDTreeNode node = null;
                if (Parms.UniqueID != null)
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, uniqueID: Parms.UniqueID, autoExpandWhileFinding: false);
                }
                else if (Parms.OwnerUserRID != Include.NoRID)
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, aNodeType: Parms.ProfileType, aNodeRID: Parms.Key, ownerUserRID: Parms.OwnerUserRID, autoExpandWhileFinding: false);
                }
                else
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, aNodeType: Parms.ProfileType, aNodeRID: Parms.Key, autoExpandWhileFinding: false);
                }
                if (node != null)
                {
                    _treeView.ExpandNode(node);

                    nodes = node.Nodes;
                }
            }

            if (nodes == null)
            {
                throw new Exception("Unable to get nodes for ProfileType:" + Parms.ProfileType + " and Key:" +  Parms.Key);
            }

            ROIListOut RONodeList = new ROIListOut(eROReturnCode.Successful, null, ROInstanceID, BuildNodeList(nodes, Parms));

            return RONodeList;
        }

        internal List<ROTreeNodeOut> BuildNodeList(System.Windows.Forms.TreeNodeCollection nodes, ROTreeNodeParms Parms)
        {
            List<ROTreeNodeOut> nodeList = new List<ROTreeNodeOut>();
            eROApplicationType applicationType = eROApplicationType.All;
            string qualifiedNodeID = null;

            foreach (MIDTreeNode node in nodes)
            {
                applicationType = eROApplicationType.All;
                if (_isWorkflowMethodData)
                {
                    if (node.Profile != null)
                    {
                        if (Enum.IsDefined(typeof(eWorkflowMethodNodeAllocationType), Convert.ToInt32(node.NodeProfileType))
                            || Enum.IsDefined(typeof(eAllocationMethodType), Convert.ToInt32(node.Profile.ProfileType))
                            || node.Profile is AllocationWorkFlow)
                        {
                            applicationType = eROApplicationType.Allocation;
                        }
                        else if (Enum.IsDefined(typeof(eWorkflowMethodNodePlanningType), Convert.ToInt32(node.NodeProfileType))
                            || Enum.IsDefined(typeof(eForecastMethodType), Convert.ToInt32(node.Profile.ProfileType))
                            || node.Profile is OTSPlanWorkFlow)
                        {
                            applicationType = eROApplicationType.Forecast;
                        }
                    }

                    if (Parms.ROApplicationType != eROApplicationType.All)
                    {
                        if (applicationType != eROApplicationType.All)
                        {
                            if (Parms.ROApplicationType == eROApplicationType.Allocation
                                && applicationType != eROApplicationType.Allocation)
                            {
                                continue;
                            }
                            else if (Parms.ROApplicationType == eROApplicationType.Forecast
                                && applicationType != eROApplicationType.Forecast)
                            {
                                continue;
                            }
                            else if (Parms.ROApplicationType == eROApplicationType.Assortment
                                && applicationType != eROApplicationType.Assortment)
                            {
                                continue;
                            }
                        }
                    }
                }

                bool isReadOnly = node.NodeSecurityGroup.FunctionSecurityProfile == null ? true : node.NodeSecurityGroup.FunctionSecurityProfile.IsReadOnly;
                bool canBeDeleted = node.NodeSecurityGroup.FunctionSecurityProfile == null ? false : node.NodeSecurityGroup.FunctionSecurityProfile.AllowDelete;
                bool canCreateNewFolder = node.NodeSecurityGroup.FunctionSecurityProfile == null ? false : node.FunctionSecurityProfile.AllowUpdate;
                bool canCreateNewItem = node.NodeSecurityGroup.FunctionSecurityProfile == null ? false : node.FunctionSecurityProfile.AllowUpdate;
                bool canBeProcessed = node.NodeSecurityGroup.FunctionSecurityProfile == null ? false : node.FunctionSecurityProfile.AllowExecute;
                bool canBeCopied = node.NodeSecurityGroup.FunctionSecurityProfile == null ? false : node.FunctionSecurityProfile.AllowMove;
                bool canBeCut = node.NodeSecurityGroup.FunctionSecurityProfile == null ? false : node.FunctionSecurityProfile.AllowMove;

                if (_isWorkflowMethodData)
                {
                    SetWorkflowMethodSecurity(
                        explorerNode: node,
                        isReadOnly: ref isReadOnly,
                        canBeDeleted: ref canBeDeleted,
                        canCreateNewFolder: ref canCreateNewFolder,
                        canCreateNewItem: ref canCreateNewItem,
                        canBeProcessed: ref canBeProcessed,
                        canBeCopied: ref canBeCopied,
                        canBeCut: ref canBeCut
                        );
                }

                node.BuildUniqueID();

                if (this is Logility.ROWeb.ROMerchandiseExplorer)
                {
                    if (((MIDHierarchyNode)node).HierarchyLevelType == eHierarchyLevelType.Color
                        || ((MIDHierarchyNode)node).HierarchyLevelType == eHierarchyLevelType.Size)
                    {
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(((MIDHierarchyNode)node).NodeRID, true, true);
                        qualifiedNodeID = hnp.Text;
                    }
                }

                nodeList.Add(new ROTreeNodeOut(
                    key: node.Profile.Key,
                    text: node.Text,
                    ownerUserRID: node.OwnerUserRID,
                    treeNodeType: node.TreeNodeType,
                    profileType: node.Profile.ProfileType,
                    isReadOnly: isReadOnly,
                    canBeDeleted: canBeDeleted,
                    canCreateNewFolder: canCreateNewFolder,
                    canCreateNewItem: canCreateNewItem,
                    canBeProcessed: canBeProcessed,
                    canBeCopied: canBeCopied,
                    canBeCut: canBeCut,
                    hasChildren: node.HasChildren,
                    ROApplicationType: applicationType,
                    uniqueID: node.UniqueID,
                    qualifiedText: qualifiedNodeID
                    ));

            }

            return nodeList;
        }

        private void SetWorkflowMethodSecurity(MIDTreeNode explorerNode, ref bool isReadOnly, ref bool canBeDeleted, ref bool canCreateNewFolder, ref bool canCreateNewItem,
                                                ref bool canBeProcessed, ref bool canBeCopied, ref bool canBeCut)
        {
            MIDWorkflowMethodTreeNode node;
            try
            {
                node = (MIDWorkflowMethodTreeNode)explorerNode;

                canBeProcessed = false;
                //cmiNewForecastGroup.Visible = false;
                //cmiNewAllocationGroup.Visible = false;

                if ((node.isObject ||
                    node.isChildObjectShortcut) &&
                    node.FunctionSecurityProfile.AllowExecute)
                {
                    canBeProcessed = true;
                }

                if (node.GetTopNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                {
                    canCreateNewFolder = true;
                }
                else if (node.isShortcut ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodAllocationFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodAllocationMethodsFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodAllocationSizeMethodsFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastMethodsFolder ||
                    (node.isUserItem && !((WorkflowMethodTreeView)_treeView).isUserFolderMaintenanceAllowed) ||
                    (node.isGlobalItem && !((WorkflowMethodTreeView)_treeView).isGlobalFolderMaintenanceAllowed))
                {
                    canCreateNewFolder = false;
                }
                else
                {
                    canCreateNewFolder = true;
                }

                if (node.isShortcut ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodMainFavoritesFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodMainGlobalFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodMainUserFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodAllocationFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodAllocationMethodsFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodAllocationSizeMethodsFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastMethodsFolder ||
                    node.GetTopSourceNode().isMainFavoriteFolder ||
                    !node.isWithinItemFolder ||
                    !node.FunctionSecurityProfile.AllowUpdate)
                {
                    canCreateNewItem = false;
                }
                else
                {
                    canCreateNewItem = true;
                }

                //if (!node.isShortcut &&
                //    !node.GetTopSourceNode().isMainFavoriteFolder &&
                //    (node.Profile.ProfileType == eProfileType.WorkflowMethodMainGlobalFolder ||
                //    node.Profile.ProfileType == eProfileType.WorkflowMethodMainUserFolder))
                //{
                //    if (node.isGlobalItem &&
                //        ((WorkflowMethodTreeView)_treeView).GlobalFolderSecurity.AllowUpdate &&
                //        ((WorkflowMethodTreeView)_treeView).isGlobalForecastingAllowed)
                //    {
                //        cmiNewForecastGroup.Visible = true;
                //    }
                //    else if (node.isUserItem &&
                //        ((WorkflowMethodTreeView)_treeView).UserFolderSecurity.AllowUpdate &&
                //        ((WorkflowMethodTreeView)_treeView).isUserForecastingAllowed)
                //    {
                //        cmiNewForecastGroup.Visible = true;
                //    }

                //    if (node.isGlobalItem &&
                //        ((WorkflowMethodTreeView)_treeView).GlobalFolderSecurity.AllowUpdate &&
                //        ((WorkflowMethodTreeView)_treeView).isGlobalAllocationAllowed)
                //    {
                //        cmiNewAllocationGroup.Visible = true;
                //    }
                //    else if (node.isUserItem &&
                //        ((WorkflowMethodTreeView)_treeView).UserFolderSecurity.AllowUpdate &&
                //        ((WorkflowMethodTreeView)_treeView).isUserAllocationAllowed)
                //    {
                //        cmiNewAllocationGroup.Visible = true;
                //    }
                //}

                if (node.Profile.ProfileType == eProfileType.WorkflowMethodAllocationFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastFolder)
                {
                    if (node.FunctionSecurityProfile.AccessDenied)
                    {
                        canBeCopied = false;
                    }
                    else
                    {
                        canBeCopied = true;
                    }

                    if (node.isGlobalItem)
                    {
                        if (((WorkflowMethodTreeView)_treeView).GlobalFolderSecurity.AllowDelete)
                        {
                            canBeDeleted = true;
                            canBeCut = true;
                        }
                        else
                        {
                            canBeDeleted = false;
                            canBeCut = false;
                        }
                    }
                    else if (node.isUserItem)
                    {
                        if (((WorkflowMethodTreeView)_treeView).UserFolderSecurity.AllowDelete)
                        {
                            canBeDeleted = true;
                            canBeCut = true;
                        }
                        else
                        {
                            canBeDeleted = false;
                            canBeCut = false;
                        }
                    }
                }

                // cannot have nested groups 
                //if (!node.isShortcut &&
                //    !node.GetTopSourceNode().isMainFavoriteFolder &&
                //    (node.Profile.ProfileType == eProfileType.WorkflowMethodSubFolder &&
                //    node.GetGroupKey() == Include.NoRID))
                //{
                //    if (node.isGlobalItem &&
                //        ((WorkflowMethodTreeView)_treeView).GlobalFolderSecurity.AllowUpdate)
                //    {
                //        cmiNewForecastGroup.Visible = true;
                //    }
                //    else if (node.isUserItem &&
                //        ((WorkflowMethodTreeView)_treeView).UserFolderSecurity.AllowUpdate)
                //    {
                //        cmiNewForecastGroup.Visible = true;
                //    }

                //    if (node.isGlobalItem &&
                //        ((WorkflowMethodTreeView)_treeView).GlobalFolderSecurity.AllowUpdate)
                //    {
                //        cmiNewAllocationGroup.Visible = true;
                //    }
                //    else if (node.isUserItem &&
                //        ((WorkflowMethodTreeView)_treeView).UserFolderSecurity.AllowUpdate)
                //    {
                //        cmiNewAllocationGroup.Visible = true;
                //    }
                //}

                // do not allow new groups in shared folder
                if (node.isShared)
                {
                    canCreateNewFolder = false;
                    canCreateNewItem = false;
                    //cmiNewForecastGroup.Visible = false;
                    //cmiNewAllocationGroup.Visible = false;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region "Method to Refresh Explorer Data"
        internal ROOut RefreshExplorerData(ROTreeNodeParms Parms)
        {
            Hashtable expandedList;
            expandedList = new Hashtable();
            SaveExpandedNodes(_treeView.Nodes, expandedList);

            if (Parms.Key == Include.NoRID)
            {
                RefreshTreeView(); 
            }
            else
            {
                MIDTreeNode node = null;
                if (Parms.UniqueID != null)
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, uniqueID: Parms.UniqueID, autoExpandWhileFinding: false);
                }
                else if (Parms.OwnerUserRID != Include.NoRID)
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, aNodeType: Parms.ProfileType, aNodeRID: Parms.Key, ownerUserRID: Parms.OwnerUserRID, autoExpandWhileFinding: false);
                }
                else
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, aNodeType: Parms.ProfileType, aNodeRID: Parms.Key, autoExpandWhileFinding: false);
                }
                if (node != null)
                {
                    RefreshTreeView(node);
                }
            }


            ResetExpandedNodes(_treeView.Nodes, expandedList);

            ROTreeNodeParms ro = new ROTreeNodeParms(
                sROUserID: Parms.ROUserID, 
                sROSessionID: Parms.ROSessionID, 
                ROClass: Parms.ROClass, 
                RORequest: eRORequest.GetWorkflowMethodExplorerData, 
                ROInstanceID: Parms.ROInstanceID, 
                profileType: Parms.ProfileType, 
                key: Parms.Key, 
                ROApplicationType: Parms.ROApplicationType,
                ownerUserRID: Parms.OwnerUserRID,
            	uniqueID: Parms.UniqueID
                );
            

            return GetExplorerData(ro);
        }

        private void SaveExpandedNodes(System.Windows.Forms.TreeNodeCollection aNodes, Hashtable aExpandedList)
        {
            try
            {
                foreach (MIDTreeNode node in aNodes)
                {
                    if (node.IsExpanded)
                    {
                        aExpandedList[new HashKeyObject((int)node.TreeNodeType, (int)node.NodeProfileType, node.Profile.Key)] = null;
                    }

                    SaveExpandedNodes(node.Nodes, aExpandedList);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ResetExpandedNodes(System.Windows.Forms.TreeNodeCollection aNodes, Hashtable aExpandedList)
        {
            try
            {
                foreach (MIDTreeNode node in aNodes)
                {
                    if (node.Nodes.Count > 0 && node.Profile != null)
                    {
                        if (aExpandedList.Contains(new HashKeyObject((int)node.TreeNodeType, (int)node.NodeProfileType, node.Profile.Key)))
                        {
                            node.Expand();
                        }

                        ResetExpandedNodes(node.Nodes, aExpandedList);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        #endregion

        internal ROOut DeleteExplorerData(ROTreeNodeParms Parms)
        {

            MIDTreeNode node = null;
            if (Parms.UniqueID != null)
            {
                node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, uniqueID: Parms.UniqueID, autoExpandWhileFinding: false);
            }
            else if (Parms.OwnerUserRID != Include.NoRID)
            {
                node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, aNodeType: Parms.ProfileType, aNodeRID: Parms.Key, ownerUserRID: Parms.OwnerUserRID, autoExpandWhileFinding: false);
            }
            else
            {
                node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, aNodeType: Parms.ProfileType, aNodeRID: Parms.Key, autoExpandWhileFinding: false);
            }

            if (node == null)
            {
                MIDEnvironment.requestFailed = true;
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                    messageCode: eMIDTextCode.msg_ValueWasNotFound,
                    addToAuditReport: true,
                    args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_OTS_Node) }
                    );
            }
            else
            {
                _treeView.AddSelectedNode(aTreeNode: node);

                _treeView.DeleteTreeNode();
            }

            if (MIDEnvironment.requestFailed)
            {
                return new RONoDataOut(ROReturnCode: eROReturnCode.Failure, sROMessage: MIDEnvironment.Message, ROInstanceID: ROInstanceID);
            }
            else
            {
                return new RONoDataOut(ROReturnCode: eROReturnCode.Successful, sROMessage: null, ROInstanceID: ROInstanceID);
            }
        }

        #region Command Processing

        public ROOut Rename(RODataExplorerRenameParms Parms)
        {

            if (Parms.NewName.Trim().Length == 0)
            {
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                            messageCode: eMIDTextCode.msg_NameRequired,
                            addToAuditReport: true
                            );
                MIDEnvironment.requestFailed = true;
                return new RONoDataOut(ROReturnCode: eROReturnCode.Failure, sROMessage: null, ROInstanceID: ROInstanceID);
            }

            try
            {
                MIDTreeNode node = FindTreeNode(Parms: Parms);

                //_treeView.AddSelectedNode(aTreeNode: node);

                //_treeView.RenameNode(node: node, label: Parms.NewName);

                if (node != null)
                {
                    RenameNode(node: node, newName: Parms.NewName);
                }

                if (MIDEnvironment.requestFailed)
                {
                    return new RONoDataOut(ROReturnCode: eROReturnCode.Failure, sROMessage: MIDEnvironment.Message, ROInstanceID: ROInstanceID);
                }
                else
                {
                    return new RONoDataOut(ROReturnCode: eROReturnCode.Successful, sROMessage: null, ROInstanceID: ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        private bool RenameNode(MIDTreeNode node, string newName)
        {

            if (newName.Length == 0)
            {
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                            messageCode: eMIDTextCode.msg_NameRequired,
                            addToAuditReport: true
                            );
                MIDEnvironment.requestFailed = true;
                return false;
            }

            try
            {
                _treeView.AddSelectedNode(aTreeNode: node);

                _treeView.RenameNode(node: node, label: newName);

                if (MIDEnvironment.requestFailed)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }

        public ROOut AddShortCut(RODataExplorerShortcutParms Parms)
        {
            MIDTreeNode node = null;
            MIDTreeNode parentNode = null;
            ArrayList nodes = new ArrayList();

            try
            {
                node = FindTreeNode(Parms: Parms);

                parentNode = FindTreeNode(Parms: Parms, lookUpToParent: true);

                nodes.Add(node);

                _treeView.AddSelectedNode(parentNode);

                _treeView.PasteTreeNode(aCutCopyNodes: nodes, aCutCopyOperation: eCutCopyOperation.Copy);

                if (MIDEnvironment.requestFailed)
                {
                    return new RONoDataOut(ROReturnCode: eROReturnCode.Failure, sROMessage: MIDEnvironment.Message, ROInstanceID: ROInstanceID);
                }
                else
                {
                    return new RONoDataOut(ROReturnCode: eROReturnCode.Successful, sROMessage: null, ROInstanceID: ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        public ROOut DeleteShortCut(RODataExplorerShortcutParms Parms)
        {
            MIDTreeNode node = null;
            MIDTreeNode parentNode = null;
            ArrayList nodes = new ArrayList();

            try
            {
                node = FindTreeNode(Parms: Parms);

                parentNode = FindTreeNode(Parms: Parms, lookUpParent: true);

                _treeView.AddSelectedNode(aTreeNode: node);

                _treeView.DeleteTreeNode();

                if (MIDEnvironment.requestFailed)
                {
                    return new RONoDataOut(ROReturnCode: eROReturnCode.Failure, sROMessage: MIDEnvironment.Message, ROInstanceID: ROInstanceID);
                }
                else
                {
                    return new RONoDataOut(ROReturnCode: eROReturnCode.Successful, sROMessage: null, ROInstanceID: ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        public ROOut Copy(RODataExplorerCopyParms Parms)
        {
            MIDTreeNode node = null;
            MIDTreeNode parentNode = null;
            ArrayList nodes = new ArrayList();

            try
            {
                node = FindTreeNode(Parms: Parms);

                parentNode = FindTreeNode(Parms: Parms, lookUpToParent: true);

                nodes.Add(node);

                _treeView.AddSelectedNode(parentNode);

                _treeView.PasteTreeNode(aCutCopyNodes: nodes, aCutCopyOperation: eCutCopyOperation.Copy);

                if (MIDEnvironment.requestFailed)
                {
                    return new RONoDataOut(ROReturnCode: eROReturnCode.Failure, sROMessage: MIDEnvironment.Message, ROInstanceID: ROInstanceID);
                }
                else
                {
                    return new RONoDataOut(ROReturnCode: eROReturnCode.Successful, sROMessage: null, ROInstanceID: ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        public ROOut AddFolder(RODataExplorerFolderParms Parms)
        {
            MIDTreeNode node = null;
            MIDTreeNode parentNode = null;

            try
            {
                parentNode = FindTreeNode(Parms: Parms, lookUpToParent: true);

                node = _treeView.CreateNewTreeFolder(aNode: parentNode, aUserId: Parms.NewUserKey);

                if (node != null)
                {
                    RenameNode(node: node, newName: Parms.Name);
                }

                if (MIDEnvironment.requestFailed)
                {
                    return new RONoDataOut(ROReturnCode: eROReturnCode.Failure, sROMessage: MIDEnvironment.Message, ROInstanceID: ROInstanceID);
                }
                else
                {
                    return new RONoDataOut(ROReturnCode: eROReturnCode.Successful, sROMessage: null, ROInstanceID: ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        public ROOut DeleteFolder(RODataExplorerFolderParms Parms)
        {
            MIDTreeNode node = null;
            MIDTreeNode parentNode = null;

            try
            {
                node = FindTreeNode(Parms: Parms);

                parentNode = FindTreeNode(Parms: Parms, lookUpParent: true);

                _treeView.AddSelectedNode(aTreeNode: node);

                _treeView.DeleteTreeNode();

                if (MIDEnvironment.requestFailed)
                {
                    return new RONoDataOut(ROReturnCode: eROReturnCode.Failure, sROMessage: MIDEnvironment.Message, ROInstanceID: ROInstanceID);
                }
                else
                {
                    return new RONoDataOut(ROReturnCode: eROReturnCode.Successful, sROMessage: null, ROInstanceID: ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        private MIDTreeNode FindTreeNode(ROBaseUpdateParms Parms, bool lookUpParent = false, bool lookUpToParent = false)
        {
            MIDTreeNode node = null;
            
            if (lookUpParent)
            {
                eProfileType parentProfileType = GetFolderType(parentProfileType: Parms.ParentProfileType, parentKey: Parms.ParentKey, parentUserKey: Parms.ParentUserKey, profileType: Parms.ProfileType, uniqueID: Parms.ParentUniqueID);
                if (Parms.ParentUniqueID != null)
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, uniqueID: Parms.ParentUniqueID, autoExpandWhileFinding: true);
                }
                else if (Parms.NewUserKey != Include.NoRID)
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, aNodeType: Parms.ParentProfileType, aNodeRID: Parms.ParentKey, ownerUserRID: Parms.ParentUserKey, autoExpandWhileFinding: true);
                }
                else
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, aNodeType: Parms.ParentProfileType, aNodeRID: Parms.ParentKey, autoExpandWhileFinding: true);
                }
            }
            else if (lookUpToParent)
            {
                eProfileType parentProfileType = GetFolderType(parentProfileType: Parms.ToParentProfileType, parentKey: Parms.ToParentKey, parentUserKey: Parms.NewUserKey, profileType: Parms.ToParentProfileType, uniqueID: Parms.ToParentUniqueID);
                if (Parms.ToParentUniqueID != null)
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, uniqueID: Parms.ToParentUniqueID, autoExpandWhileFinding: true);
                }
                else if (Parms.NewUserKey != Include.NoRID)
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, aNodeType: Parms.ToParentProfileType, aNodeRID: Parms.ToParentKey, ownerUserRID: Parms.NewUserKey, autoExpandWhileFinding: true);
                }
                else
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, aNodeType: Parms.ToParentProfileType, aNodeRID: Parms.ToParentKey, autoExpandWhileFinding: true);
                }
            }
            else
            {
                if (Parms.UniqueID != null)
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, uniqueID: Parms.UniqueID, autoExpandWhileFinding: true);
                }
                else if (Parms.UserKey != Include.NoRID)
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, aNodeType: Parms.ProfileType, aNodeRID: Parms.Key, ownerUserRID: Parms.UserKey, autoExpandWhileFinding: true);
                }
                else
                {
                    node = _treeView.FindTreeNode(aNodes: _treeView.Nodes, aNodeType: Parms.ProfileType, aNodeRID: Parms.Key, autoExpandWhileFinding: true);
                }
            }

            if (node == null)
            {
                MIDEnvironment.requestFailed = true;
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                    messageCode: eMIDTextCode.msg_ValueWasNotFound,
                    addToAuditReport: true,
                    args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_OTS_Node) }
                    );
            }

            return node;
        }

        #endregion Command Processing

    }
}
