using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Drawing;
using System.Data;
using System.Timers;
using System.Windows.Forms;
using System.Diagnostics;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;


namespace MIDRetail.Windows
{
	/// <summary>
	/// This control is used to scroll through all Workflows/Methods
	/// available to the user.  The is done by dispaying
	/// a tree view control in which the user can navigate
	/// to find appropriate Workflows/Methods.
	/// </summary>
	public class WorkflowMethodExplorer : ExplorerBase
	{
		#region Member Variables
		System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WorkflowMethodExplorer));

        private System.Windows.Forms.ToolStripMenuItem cmiProcess;
        private System.Windows.Forms.ToolStripMenuItem cmiNewForecastGroup;
        private System.Windows.Forms.ToolStripMenuItem cmiNewAllocationGroup;

		private System.ComponentModel.IContainer components;

		#endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // WorkflowMethodExplorer
            // 
            this.Name = "WorkflowMethodExplorer";
            this.Size = new System.Drawing.Size(232, 424);
            this.Leave += new System.EventHandler(this.WorkflowMethodExplorer_Leave);
            this.Enter += new System.EventHandler(this.WorkflowMethodExplorer_Enter);
            this.ResumeLayout(false);

        }

        void WorkflowMethodExplorer_Leave(object sender, EventArgs e)
        {
            // undo all menu changes
        }

        void WorkflowMethodExplorer_Enter(object sender, EventArgs e)
        {
            // make sure menu is set
            ShowMenuItem(this, eMIDMenuItem.EditClear);
            // Begin Track #6365 - JSmith - Save As Grayed out 
            //ShowMenuItem(this, eMIDMenuItem.FileSave);
            //ShowMenuItem(this, eMIDMenuItem.FileSaveAs);
            HideMenuItem(this, eMIDMenuItem.FileSave);
            HideMenuItem(this, eMIDMenuItem.FileSaveAs);
            // End Track #6365
            EnableMenuItem(this, eMIDMenuItem.EditDelete);
        }
        #endregion

		#region Constructor
		/// <summary>
		/// Standard constructor
		/// </summary>
		public WorkflowMethodExplorer(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB, Form aMainMDIForm)
			: base(aSAB, aEAB, aMainMDIForm)
		{
			aEAB.WorkflowMethodExplorer = this;
		}
        /// <summary>
        /// Standard deconstruction
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        //========
        // METHODS
        //========

        //------------------
        // Virtual overrides
        //------------------

        /// <summary>
        /// Virtual method that is called to initialize the ExplorerBase TreeView
        /// </summary>

        protected override void InitializeExplorer()
        {
            try
            {
                base.InitializeExplorer();

                InitializeComponent();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Virtual method that is called to initialize the ExplorerBase TreeView
        /// </summary>

        override protected void InitializeTreeView()
        {
            try
            {
                TreeView = new WorkflowMethodTreeView();
                ((WorkflowMethodTreeView)TreeView).InitializeTreeView(SAB, false, MainMDIForm, EAB);

                TreeView.AllowDrop = true;
                TreeView.AllowAutoExpand = true;
                TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
                TreeView.ImageList = MIDGraphics.ImageList; ;
                TreeView.LabelEdit = true;
                TreeView.Location = new System.Drawing.Point(0, 0);
                TreeView.Name = "TreeView";
                TreeView.PathSeparator = ".";
                TreeView.Size = new System.Drawing.Size(216, 352);
                TreeView.TabIndex = 0;

                TreeView.OnMIDDoubleClick += new MIDTreeView.MIDTreeViewDoubleClickHandler(this.MIDTreeView_OnMIDDoubleClick);

                Controls.Add(this.TreeView);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Virtual method that is called to perform Form Load tasks
        /// </summary>

        override protected void ExplorerLoad()
        {
            try
            {
                TreeView.ImageList = MIDGraphics.ImageList;

                //_dlFolder = new FolderDataLayer();

                _cutCopyOperation = eCutCopyOperation.None;

                //_wmManager = new WorkflowMethodManager(SAB.ClientServerSession.UserRID);

                //_globalWorkflows = new Hashtable();
                //_globalMethods = new Hashtable();
                //_userWorkflows = new Hashtable();
                //_userMethods = new Hashtable();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Virtual method that is called to build the ExplorerBase TreeView
        /// </summary>

        override protected void BuildTreeView()
        {
            try
            {
                // Begin TT#2 - JSmith - Assortment Security
                if (SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersWorkflowMethod).AccessDenied)
                {
                    return;
                }
                // End TT#2

                TreeView.LoadNodes();
                BuildContextmenu();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Virtual method that is called to refresh the ExplorerBase TreeView
        /// </summary>

        override protected void RefreshTreeView()
        {
            try
            {
                TreeView.LoadNodes();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //BEGIN TT#459-MD -jsobek -Method created with File->Save As is not viewable until after a refresh of the explorer
        public void RefreshData()
        {
            //Be nice and keep a list of expanded nodes.
            nodeExpandedList.Clear();
            int nodeLevel = 0;
            foreach (TreeNode n in TreeView.Nodes)
            {
                AddExpandedNodeWithChildren(nodeLevel, n);
            }

            RefreshTreeView();

            foreach (TreeNode n in TreeView.Nodes)
            {
                DoExpandNodeWithChildren(nodeLevel, n);
            }
        }
        private System.Collections.Generic.List<string> nodeExpandedList = new System.Collections.Generic.List<string>();
        private void AddExpandedNodeWithChildren(int level, TreeNode n)
        {
            if (n.IsExpanded)
            {
                AddNodeToExpandedList(((MIDWorkflowMethodTreeNode)n).Key, level, n.Text); // TT#3570 - JSmith - Global folders expand when user save as
            }
            foreach (TreeNode nChild in n.Nodes)
            {
                AddExpandedNodeWithChildren(level + 1, nChild);
            }
        }
        private void DoExpandNodeWithChildren(int level, TreeNode n)
        {
            if (WasNodeExpanded(((MIDWorkflowMethodTreeNode)n).Key, level, n.Text)) // TT#3570 - JSmith - Global folders expand when user save as
            {
                n.Expand();
            }
            foreach (TreeNode nChild in n.Nodes)
            {
                DoExpandNodeWithChildren(level + 1, nChild);
            }
        }
        // Begin TT#3570 - JSmith - Global folders expand when user save as
        //private void AddNodeToExpandedList(int level, string nodeText)
        //{
        //    nodeExpandedList.Add(level.ToString() + "-" + nodeText);
        //}
        //private bool WasNodeExpanded(int level, string nodeText)
        //{
        //    if (nodeExpandedList.Contains(level.ToString() + "-" + nodeText))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        private void AddNodeToExpandedList(int Key, int level, string nodeText)
        {
            nodeExpandedList.Add(Key.ToString() + "-" + level.ToString() + "-" + nodeText);
        }
        private bool WasNodeExpanded(int Key, int level, string nodeText)
        {
            if (nodeExpandedList.Contains(Key.ToString() + "-" + level.ToString() + "-" + nodeText))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // End TT#3570 - JSmith - Global folders expand when user save as

        //END TT#459-MD -jsobek -Method created with File->Save As is not viewable until after a refresh of the explorer

        /// <summary>
        /// Virtual method that gets the text for the New Item menu item.
        /// </summary>
        /// <returns>
        /// The text for the New Item menu item.
        /// </returns>

        override protected string GetNewItemText(MIDTreeNode aCurrentNode)
        {
            if (aCurrentNode.Profile.ProfileType == eProfileType.WorkflowMethodAllocationWorkflowsFolder ||
                aCurrentNode.Profile.ProfileType == eProfileType.WorkflowMethodAllocationWorkflowsSubFolder ||
                aCurrentNode.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastWorkflowsFolder ||
                aCurrentNode.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder)
            {
                return "Workflow";
            }
            else
            {
                return "Method";
            }
        }

        override protected void CustomizeActionMenu(MIDTreeNode aNode)
        {
            MIDWorkflowMethodTreeNode node;
            try
            {
                node = (MIDWorkflowMethodTreeNode)aNode;

                cmiProcess.Visible = false;
                cmiNewForecastGroup.Visible = false;
                cmiNewAllocationGroup.Visible = false;

                if ((node.isObject ||
                    node.isChildObjectShortcut)&&
                    node.FunctionSecurityProfile.AllowExecute)
                {
                    cmiProcess.Visible = true;
                }

                // Begin TT#42 - JSmith - Explorers and My Favorites cannot create Folders and My Favorites Workflow/Methods not behaving as expected.
                if (node.GetTopNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                {
                    CustomizeActionMenuItem(eExplorerActionMenuItem.NewFolder, true);
                }
                else if (node.isShortcut ||
                //if (node.isShortcut ||
                // End TT#42
                    node.Profile.ProfileType == eProfileType.WorkflowMethodAllocationFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodAllocationMethodsFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodAllocationSizeMethodsFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastMethodsFolder ||
                    (node.isUserItem && !((WorkflowMethodTreeView)TreeView).isUserFolderMaintenanceAllowed) ||
                    (node.isGlobalItem && !((WorkflowMethodTreeView)TreeView).isGlobalFolderMaintenanceAllowed))
                {
                    CustomizeActionMenuItem(eExplorerActionMenuItem.NewFolder, false);
                }
                else
                {
                    CustomizeActionMenuItem(eExplorerActionMenuItem.NewFolder, true);
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
                    CustomizeActionMenuItem(eExplorerActionMenuItem.NewItem, false);
                }
                else
                {
                    CustomizeActionMenuItem(eExplorerActionMenuItem.NewItem, true);
                }

                if (!node.isShortcut &&
                    !node.GetTopSourceNode().isMainFavoriteFolder &&
                    (node.Profile.ProfileType == eProfileType.WorkflowMethodMainGlobalFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodMainUserFolder))
                {
                    if (node.isGlobalItem &&
                        ((WorkflowMethodTreeView)TreeView).GlobalFolderSecurity.AllowUpdate &&
                        ((WorkflowMethodTreeView)TreeView).isGlobalForecastingAllowed)
                    {
                        cmiNewForecastGroup.Visible = true;
                    }
                    else if (node.isUserItem &&
                        ((WorkflowMethodTreeView)TreeView).UserFolderSecurity.AllowUpdate &&
                        ((WorkflowMethodTreeView)TreeView).isUserForecastingAllowed)
                    {
                        cmiNewForecastGroup.Visible = true;
                    }
                
                    if (node.isGlobalItem &&
                        ((WorkflowMethodTreeView)TreeView).GlobalFolderSecurity.AllowUpdate &&
                        ((WorkflowMethodTreeView)TreeView).isGlobalAllocationAllowed)
                    {
                        cmiNewAllocationGroup.Visible = true;
                    }
                    else if (node.isUserItem &&
                        ((WorkflowMethodTreeView)TreeView).UserFolderSecurity.AllowUpdate &&
                        ((WorkflowMethodTreeView)TreeView).isUserAllocationAllowed)
                    {
                        cmiNewAllocationGroup.Visible = true;
                    }
                }

                if (node.Profile.ProfileType == eProfileType.WorkflowMethodAllocationFolder ||
                    node.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastFolder)
                {
                    if (node.FunctionSecurityProfile.AccessDenied)
                    {
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Copy, false);
                    }
                    else
                    {
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Copy, true);
                    }

                    if (node.isGlobalItem)
                    {
                        if (((WorkflowMethodTreeView)TreeView).GlobalFolderSecurity.AllowDelete)
                        {
                            CustomizeActionMenuItem(eExplorerActionMenuItem.Delete, true);
                            CustomizeActionMenuItem(eExplorerActionMenuItem.Cut, true);
                        }
                        else
                        {
                            CustomizeActionMenuItem(eExplorerActionMenuItem.Delete, false);
                            CustomizeActionMenuItem(eExplorerActionMenuItem.Cut, false);
                        }
                    }
                    else if (node.isUserItem)
                    {
                        if (((WorkflowMethodTreeView)TreeView).UserFolderSecurity.AllowDelete)
                        {
                            CustomizeActionMenuItem(eExplorerActionMenuItem.Delete, true);
                            CustomizeActionMenuItem(eExplorerActionMenuItem.Cut, true);
                        }
                        else
                        {
                            CustomizeActionMenuItem(eExplorerActionMenuItem.Delete, false);
                            CustomizeActionMenuItem(eExplorerActionMenuItem.Cut, false);
                        }
                    }

                    // Begin TT#75 - JSmith - Workflow Method Explorer when right click on OTS Forecast>New>Allocation Group
                    //if (node.isGlobalItem &&
                    //    ((WorkflowMethodTreeView)TreeView).GlobalFolderSecurity.AllowUpdate)
                    //{
                    //    cmiNewAllocationGroup.Visible = true;
                    //}
                    //else if (node.isUserItem &&
                    //    ((WorkflowMethodTreeView)TreeView).UserFolderSecurity.AllowUpdate)
                    //{
                    //    cmiNewAllocationGroup.Visible = true;
                    //}
                    // End TT#75
                }

                // cannot have nested groups 
                if (!node.isShortcut &&
                    !node.GetTopSourceNode().isMainFavoriteFolder &&
                    (node.Profile.ProfileType == eProfileType.WorkflowMethodSubFolder &&
                    node.GetGroupKey() == Include.NoRID))
                {
                    if (node.isGlobalItem &&
                        ((WorkflowMethodTreeView)TreeView).GlobalFolderSecurity.AllowUpdate)
                    {
                        cmiNewForecastGroup.Visible = true;
                    }
                    else if (node.isUserItem &&
                        ((WorkflowMethodTreeView)TreeView).UserFolderSecurity.AllowUpdate)
                    {
                        cmiNewForecastGroup.Visible = true;
                    }
                
                    if (node.isGlobalItem &&
                        ((WorkflowMethodTreeView)TreeView).GlobalFolderSecurity.AllowUpdate)
                    {
                        cmiNewAllocationGroup.Visible = true;
                    }
                    else if (node.isUserItem &&
                        ((WorkflowMethodTreeView)TreeView).UserFolderSecurity.AllowUpdate)
                    {
                        cmiNewAllocationGroup.Visible = true;
                    }
                }

                // do not allow new groups in shared folder
                if (node.isShared)
                {
                    CustomizeActionMenuItem(eExplorerActionMenuItem.NewFolder, false);
                    CustomizeActionMenuItem(eExplorerActionMenuItem.NewItem, false);
                    cmiNewForecastGroup.Visible = false;
                    cmiNewAllocationGroup.Visible = false;
                }
            }
            catch
            {
                throw;
            }
        }

        //----------------
        // Private methods
        //----------------

        private void BuildContextmenu()
        {
            cmiProcess = new System.Windows.Forms.ToolStripMenuItem();
            cmiProcess.Name = "cmiProcess";
            cmiProcess.Size = new System.Drawing.Size(195, 22);
            cmiProcess.Text = "Process";
            cmiProcess.Click += new EventHandler(cmiProcess_Click);
            AddContextMenuItem(cmiProcess, eExplorerActionMenuItem.None, eExplorerActionMenuItem.Open);

            cmiNewForecastGroup = new System.Windows.Forms.ToolStripMenuItem();
            cmiNewForecastGroup.Name = "cmiNewForecastGroup";
            cmiNewForecastGroup.Size = new System.Drawing.Size(195, 22);
            cmiNewForecastGroup.Text = "OTS Forecast Group";
            cmiNewForecastGroup.Click += new EventHandler(cmiNewForecastGroup_Click);
            AddContextMenuItem(cmiNewForecastGroup, eExplorerActionMenuItem.New, eExplorerActionMenuItem.NewItem);

            cmiNewAllocationGroup = new System.Windows.Forms.ToolStripMenuItem();
            cmiNewAllocationGroup.Name = "cmiNewAllocationGroup";
            cmiNewAllocationGroup.Size = new System.Drawing.Size(195, 22);
            cmiNewAllocationGroup.Text = "Allocation Group";
            cmiNewAllocationGroup.Click += new EventHandler(cmiNewAllocationGroup_Click);
            AddContextMenuItem(cmiNewAllocationGroup, eExplorerActionMenuItem.New, eExplorerActionMenuItem.NewItem);

        }

        void cmiNewAllocationGroup_Click(object sender, EventArgs e)
        {
            ((WorkflowMethodTreeView)TreeView).CreateNewAllocationGroup((MIDWorkflowMethodTreeNode)TreeView.SelectedNode);
        }

        void cmiNewForecastGroup_Click(object sender, EventArgs e)
        {
            ((WorkflowMethodTreeView)TreeView).CreateNewOTSForecastGroup((MIDWorkflowMethodTreeNode)TreeView.SelectedNode);
        }

        void cmiProcess_Click(object sender, EventArgs e)
        {
            ((WorkflowMethodTreeView)TreeView).ProcessNode((MIDWorkflowMethodTreeNode)TreeView.SelectedNode);
        }

	}
}
