using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using System.Diagnostics;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public partial class StoreGroupExplorer : ExplorerBase
    {
        //=======
        // FIELDS
        //=======

        private System.Windows.Forms.ToolStripMenuItem cmiStoreProfiles;
        private System.Windows.Forms.ToolStripMenuItem cmiStoreCharacteristics;
        private System.Windows.Forms.ToolStripMenuItem cmiNewDynamicAttribute;

        //=============
        // CONSTRUCTORS
        //=============

        public StoreGroupExplorer(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB, Form aMainMDIForm)
            : base(aSAB, aEAB, aMainMDIForm)
        {
            aEAB.StoreGroupExplorer = this;
        }

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
                TreeView = new StoreTreeView(EAB);
                TreeView.InitializeTreeView(SAB, false, MainMDIForm);

                TreeView.AllowDrop = true;
                TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
                TreeView.ImageList = this.imageListStoreGroup;
                TreeView.LabelEdit = true;
                TreeView.Location = new System.Drawing.Point(0, 0);
                TreeView.Name = "TreeView";
                TreeView.PathSeparator = ".";
                TreeView.Size = new System.Drawing.Size(216, 352);
                TreeView.TabIndex = 0;

                TreeView.OnMIDNodeSelect += new MIDTreeView.MIDTreeViewNodeSelectHandler(this.StoreTreeView_OnMIDNodeSelect);
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

                _cutCopyOperation = eCutCopyOperation.None;
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
                if (SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersStore).AccessDenied)
                {
                    return;
                }
                // End TT#2

                BuildContextmenu();
                TreeView.LoadNodes();
                ((StoreTreeView)TreeView).InitialExpand();
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
                //Do not allow refresh if Store Load API is running
                GenericEnqueue genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);
                if (genericEnqueueStoreLoad.DoesHaveConflicts())
                {
                    MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(SAB, genericEnqueueStoreLoad, "Store Load API");
                    return;
                }



                // Begin Track #6359 - JSmith - Err when refreshing str exp
                this.SAB.StoreServerSession.Refresh();

                //=====================================================================================
                // the application sesson caches store infor that both allocation and planning uses.
                // the cache is cleared
                //=====================================================================================
                this.SAB.ApplicationServerSession.Refresh();
                // End Track #6359

                //TreeView.LoadNodes();
                ReloadNodes();

                // Begin TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
                this.EAB.Explorer.RemoveStoreExplorerPendingRefresh();
                // End TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public class NodeTextAndLevel
        {
            public string Text;
            public int Level;

            public NodeTextAndLevel(string text, int level)
            {
                this.Text = text;
                this.Level = level;
            }
        }
        List<NodeTextAndLevel> expandedList = new List<NodeTextAndLevel>();
        public void ReloadNodes()
        {
            //Be nice and keep a list of expanded nodes.
            expandedList.Clear();
            foreach (TreeNode n in TreeView.Nodes)
            {
                AddExpandedNodesToList(n, 1);    
            }

            TreeView.LoadNodes();
           
            foreach (TreeNode n in TreeView.Nodes)
            {
                ExpandedNodesFromList(n, 1);
            }
        }

        private void AddExpandedNodesToList(TreeNode n, int level)
        {
            if (n.IsExpanded)
            {
                expandedList.Add(new NodeTextAndLevel(n.Text, level));
                foreach (TreeNode nChild in n.Nodes)
                {
                    AddExpandedNodesToList(nChild, level + 1);
                }
            }
        }
        private void ExpandedNodesFromList(TreeNode n, int level)
        {

            if (expandedList.Find(x => x.Text == n.Text && x.Level == level) != null)
            {
                n.Expand();
                foreach (TreeNode nChild in n.Nodes)
                {
                    ExpandedNodesFromList(nChild, level + 1);
                }
            }
        }

        public void RenameAndSelectStoreGroupNode(int sgRID, string sgID)
        {
            MIDStoreNode n = FindStoreGroupNode(sgRID);
            if (n != null)
            {
                n.InternalText = sgID;
                //MIDStoreNode mNode = (MIDTreeNode)n;
                ((StoreGroupProfile)n.Profile).Name = sgID;
                ((StoreTreeView)this.TreeView).ClearSelectedNode();
                ((StoreTreeView)this.TreeView).SelectNode(n);
            }
        }
      
        public MIDStoreNode FindStoreGroupNode(int sgRID)
        {
            //bool isFound = false;
            //TreeNode foundNode = null;
            //foreach (TreeNode n in TreeView.Nodes)
            //{
            //    FindStoreGroupNodeInTree(n, sgRID, ref isFound, ref foundNode);
            //    if (isFound)
            //    {
            //        break;
            //    }
            //}
            //return foundNode;
            MIDStoreNode foundNode = ((StoreTreeView)this.TreeView).FindStoreGroup(sgRID);
            return foundNode;
        }
        //private void FindStoreGroupNodeInTree(TreeNode n, int sgRID, ref bool isFound, ref TreeNode foundNode)
        //{
        //    MIDTreeNode mNode = (MIDTreeNode)n;
        //    if (mNode.NodeProfileType == eProfileType.StoreGroup && mNode.Profile.Key == sgRID)
        //    {
        //        isFound = true;
        //        foundNode = n;
        //    }
        //    if (isFound == false)
        //    {
        //        foreach (TreeNode nChild in n.Nodes)
        //        {
        //            FindStoreGroupNodeInTree(nChild, sgRID, ref isFound, ref foundNode);
        //            if (isFound)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //}

        // Begin TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
        public void AddStoreExplorerPendingRefresh()
        {
            EAB.Explorer.AddStoreExplorerPendingRefresh();
        }
        // End TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields

        /// <summary>
        /// Virtual method that gets the text for the New Item menu item.
        /// </summary>
        /// <returns>
        /// The text for the New Item menu item.
        /// </returns>

        override protected string GetNewItemText(MIDTreeNode aCurrentNode)
        {
             return "Attribute";  //Shows under the "New" right-click menu item
        }

        protected override void CustomizeActionMenu(MIDTreeNode aNode)
        {
            try
            {
                cmiStoreProfiles.Visible = true;
                cmiStoreCharacteristics.Visible = true;
                cmiNewDynamicAttribute.Visible = true;

                switch (aNode.NodeProfileType)
                {
                    case eProfileType.Store:
                        cmiNewDynamicAttribute.Visible = false; 
                        CustomizeActionMenuItem(eExplorerActionMenuItem.NewItem, false); 
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Cut, false);
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Copy, false);
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Delete, false);

                        cmiStoreProfiles.Visible = false;
                   
                        CustomizeActionMenuItem(eExplorerActionMenuItem.InUse, false);
                        break;

                    case eProfileType.StoreGroupLevel: //TT#3898 - JSmith - Do not allow the "All Store" Attribute or "All Stores Set" within the attribute to be deleted. TT#716 - JScott - prevent the "Available Stores' set from being deleted from the Store Explorer
                        cmiNewDynamicAttribute.Visible = false; 
                        CustomizeActionMenuItem(eExplorerActionMenuItem.NewItem, false); 
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Copy, false);
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Cut, false);
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Delete, false);
                        
                        if (((StoreGroupLevelProfile)aNode.Profile).LevelType == eGroupLevelTypes.DynamicSet)
                        {
                            CustomizeActionMenuItem(eExplorerActionMenuItem.Rename, false);
                        }

                        // Begin TT#1856-MD - JSmith - All Stores Attribute - Open Option
                        if (((StoreGroupLevelProfile)aNode.Profile).Sequence == int.MaxValue ||
                            ((StoreGroupLevelProfile)aNode.Profile).Name == Include.AvailableStoresGroupLevelName ||
                            ((StoreGroupLevelProfile)aNode.Profile).Key == Include.AllStoreGroupLevelRID)
                        {
                            CustomizeActionMenuItem(eExplorerActionMenuItem.Open, false); 
                        }
                        // End TT#1856-MD - JSmith - All Stores Attribute - Open Option

                        //if (((StoreGroupLevelProfile)aNode.Profile).Sequence == int.MaxValue ||
                        //    ((StoreGroupLevelProfile)aNode.Profile).Name == Include.AvailableStoresGroupLevelName ||
                        //    ((StoreGroupLevelProfile)aNode.Profile).Key == Include.AllStoreGroupLevelRID)
                        //{
                        //    //CustomizeActionMenuItem(eExplorerActionMenuItem.Cut, false);
                        //    //CustomizeActionMenuItem(eExplorerActionMenuItem.Delete, false);
                        //    //CustomizeActionMenuItem(eExplorerActionMenuItem.Open, false); 
                        //}
                        break;
                    case eProfileType.StoreGroup:
                        if (((StoreGroupProfile)aNode.Profile).Key == Include.AllStoreGroupRID)
                        {
                            CustomizeActionMenuItem(eExplorerActionMenuItem.Cut, false);
                            CustomizeActionMenuItem(eExplorerActionMenuItem.Open, false);
                            if (aNode.GetTopSourceNode().TreeNodeType != eTreeNodeType.MainFavoriteFolderNode) //Do not let the All Store Group to be deleted, unless it is copy in the Favorites Folder.
                            {
                                CustomizeActionMenuItem(eExplorerActionMenuItem.Delete, false);
                            }
                        }
                     
                        break;
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
            try
            {
                cmiNewDynamicAttribute = new System.Windows.Forms.ToolStripMenuItem();
                cmiNewDynamicAttribute.Name = "cmiNewDynamicAttribute";
                cmiNewDynamicAttribute.Size = new System.Drawing.Size(195, 22);
                cmiNewDynamicAttribute.Text = "Dynamic Attribute";
                cmiNewDynamicAttribute.Click += new System.EventHandler(this.cmiAddDynamicGroup_Click);
                base.AddContextMenuItem(cmiNewDynamicAttribute, eExplorerActionMenuItem.New, eExplorerActionMenuItem.NewItem);

                cmiStoreCharacteristics = new System.Windows.Forms.ToolStripMenuItem();
                cmiStoreCharacteristics.Name = "cmiStoreCharacteristics";
                cmiStoreCharacteristics.Size = new System.Drawing.Size(195, 22);
                cmiStoreCharacteristics.Text = "Store Characteristics...";
                cmiStoreCharacteristics.Click += new System.EventHandler(this.cmiStoreCharacteristics_Click);
                base.AddContextMenuItem(cmiStoreCharacteristics, eExplorerActionMenuItem.None, eExplorerActionMenuItem.RefreshSeparator);

                cmiStoreProfiles = new System.Windows.Forms.ToolStripMenuItem();
                cmiStoreProfiles.Name = "cmiStoreProfiles";
                cmiStoreProfiles.Size = new System.Drawing.Size(195, 22);
                cmiStoreProfiles.Text = "Store Profiles...";
                cmiStoreProfiles.Click += new System.EventHandler(this.cmiStoreProfiles_Click);
                base.AddContextMenuItem(cmiStoreProfiles, eExplorerActionMenuItem.None, eExplorerActionMenuItem.RefreshSeparator);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void cmiAddDynamicGroup_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (base.TreeView.SelectedNode != null)
                {
                    // Begin TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail
                    if (StoreMgmt.StoresAdded
                        || StoreMgmt.StoreGroupsAdded)
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_StoresOrGroupsChanged), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                    // End TT#5799 - JSmith - Store Explorer not refreshing when new store is added through API, causing allocation workflows to fail

                    int ownerUserRID = TreeView.SelectedNode.GetTopSourceNode().UserId;
                    System.Windows.Forms.Form frmFilter = SharedRoutines.GetFilterFormForNewFilters(filterTypes.StoreGroupDynamicFilter, SAB, _EAB, ownerUserRID);
                    frmFilter.MdiParent = MainMDIForm; 
                    frmFilter.Show();
                    frmFilter.BringToFront();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

     

        private void cmiStoreProfiles_Click(object sender, System.EventArgs e)
        {
            //StoreProfileMaint storeProfMaint;

            try
            {
                //storeProfMaint = GetStoreProfileMaintWindow();

                //if (storeProfMaint == null || storeProfMaint.IsDisposed)
                //{
                //    storeProfMaint = new StoreProfileMaint(SAB, EAB);
                //    storeProfMaint.MdiParent = MainMDIForm;
                //    storeProfMaint.Anchor = AnchorStyles.Right;
                //    storeProfMaint.Dock = DockStyle.Fill;
                //}

                //storeProfMaint.Show();
                //storeProfMaint.BringToFront();
                object[] args = new object[] { SAB, EAB };
                System.Windows.Forms.Form frmStoreMaint = SharedRoutines.GetForm(_EAB.Explorer.MdiChildren, typeof(StoreProfileMaintForm), args, false);
                frmStoreMaint.MdiParent = MainMDIForm;
                frmStoreMaint.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                frmStoreMaint.Dock = DockStyle.Fill;
                ((StoreProfileMaintForm)frmStoreMaint).LoadStores();
                frmStoreMaint.Show();
                frmStoreMaint.BringToFront();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void cmiStoreCharacteristics_Click(object sender, System.EventArgs e)
        {
           // StoreCharacteristics storeCharMaint;

            try
            {
                //storeCharMaint = GetStoreCharacteristicMaintWindow();

                //if (storeCharMaint == null || storeCharMaint.IsDisposed)
                //{
                //    storeCharMaint = new StoreCharacteristics(SAB, EAB);
                //    storeCharMaint.MdiParent = MainMDIForm;
                //    storeCharMaint.Anchor = AnchorStyles.Right;
                //    storeCharMaint.Dock = DockStyle.Fill;
                //    ((StoreCharacteristics)storeCharMaint).LoadCharacteristics(_SAB, _EAB);
                //}

                //storeCharMaint.Show();
                //storeCharMaint.BringToFront();
                object[] args = new object[] { };
                System.Windows.Forms.Form frmCharMaint = SharedRoutines.GetForm(_EAB.Explorer.MdiChildren, typeof(StoreCharacteristics), args, false);
                frmCharMaint.MdiParent = MainMDIForm;
                frmCharMaint.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                //frmCharMaint.Dock = DockStyle.Fill;
                ((StoreCharacteristics)frmCharMaint).LoadCharacteristics(SAB, _EAB);
                frmCharMaint.Show();
                frmCharMaint.BringToFront();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        protected void StoreTreeView_OnMIDNodeSelect(object source, MIDTreeNode node)
        {
            StoreProfileMaintForm storeProfileMaint;

            try
            {
                if (node != null)
                {
                    if (node.NodeProfileType == eProfileType.Store)
                    {
                        storeProfileMaint = GetStoreProfileMaintWindow();

                        if (storeProfileMaint != null)
                        {
                            storeProfileMaint.ShowStore(node.Profile.Key);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                HandleException(error);
            }
        }

        private StoreProfileMaintForm GetStoreProfileMaintWindow()
        {
            try
            {
                foreach (Form childForm in MainMDIForm.MdiChildren)
                {
                    if (childForm.GetType() == typeof(StoreProfileMaintForm))
                    {
                        return (StoreProfileMaintForm)childForm;
                    }
                }

                return null;
            }
            catch (Exception error)
            {
                string message = error.ToString();
                throw;
            }
        }

        //private StoreCharacteristics GetStoreCharacteristicMaintWindow()
        //{
        //    try
        //    {
        //        foreach (Form childForm in MainMDIForm.MdiChildren)
        //        {
        //            if (childForm.GetType() == typeof(StoreCharacteristics))
        //            {
        //                return (StoreCharacteristics)childForm;
        //            }
        //        }

        //        return null;
        //    }
        //    catch (Exception error)
        //    {
        //        string message = error.ToString();
        //        throw;
        //    }
        //}
    }
}