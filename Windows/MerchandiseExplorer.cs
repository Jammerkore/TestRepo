// Begin Track #5005 - JSmith - Explorer Organization
// Too many changes to mark.  Use difference tool for comparison.
// End Track #5005
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Configuration;
using System.Timers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Windows.Controls;
using MIDRetail.DataCommon;
using MIDRetail.Data;


namespace MIDRetail.Windows
{
	/// <summary>
	/// This control is used to scroll through Merchandise
	/// available to the user.  The is done by dispaying
	/// a tree view control in which the user can navigate
	/// to find appropriate filters.
	/// </summary>
	public class MerchandiseExplorer : ExplorerBase
	{
        // Begin TT#335 - JSmith - Menu should say Remove and not Delete
        //private string sDelete = null;
        //private string sRemove = null;
        // End TT#335
        private System.Windows.Forms.ImageList ilMerchandiseHierarchy;

        private System.Windows.Forms.ToolStripMenuItem cmiSearch;
        private System.Windows.Forms.ToolStripMenuItem cmiHierarchyProperties;
        private System.Windows.Forms.ToolStripMenuItem cmiNodeProperties;

		private System.ComponentModel.IContainer components;

		public MerchandiseExplorer(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB, Form aMainMDIForm)
			: base(aSAB, aEAB, aMainMDIForm)
		{
			aEAB.MerchandiseExplorer = this;
		}

		protected override void Dispose(bool disposing)
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MerchandiseExplorer));
            this.ilMerchandiseHierarchy = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // ilMerchandiseHierarchy
            // 
            this.ilMerchandiseHierarchy.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilMerchandiseHierarchy.ImageStream")));
            this.ilMerchandiseHierarchy.TransparentColor = System.Drawing.Color.Transparent;
            this.ilMerchandiseHierarchy.Images.SetKeyName(0, "");
            this.ilMerchandiseHierarchy.Images.SetKeyName(1, "");
            this.ilMerchandiseHierarchy.Images.SetKeyName(2, "");
            // 
            // MerchandiseExplorer
            // 
            this.AllowDrop = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.Name = "MerchandiseExplorer";
            this.Size = new System.Drawing.Size(232, 344);
            this.Leave += new System.EventHandler(this.MerchandiseExplorer_Leave);
            this.Enter += new System.EventHandler(this.MerchandiseExplorer_Enter);
            this.ResumeLayout(false);

		}

		#endregion

        protected override void InitializeExplorer()
        {
            try
            {
                base.InitializeExplorer();

                InitializeComponent();
                SetText();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //------------------
        // Virtual overrides
        //------------------

        /// <summary>
        /// Virtual method that is called to initialize the ExplorerBase TreeView
        /// </summary>

        override protected void InitializeTreeView()
        {
            try
            {
                TreeView = new HierarchyTreeView();
                TreeView.InitializeTreeView(SAB, false, MainMDIForm);

                TreeView.AllowDrop = true;
                TreeView.AllowAutoExpand = true;
                TreeView.AllowMultiSelect = true;
                TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
                TreeView.ImageList = MIDGraphics.ImageList;
                TreeView.LabelEdit = true;
                TreeView.Location = new System.Drawing.Point(0, 0);
                TreeView.Name = "TreeView";
                TreeView.PathSeparator = ".";
                TreeView.Size = new System.Drawing.Size(216, 352);
                TreeView.TabIndex = 0;
                TreeView.TimerInterval = 2000;

                TreeView.OnMIDDoubleClick += new MIDTreeView.MIDTreeViewDoubleClickHandler(TreeView_OnMIDDoubleClick);
                TreeView.KeyDown +=new KeyEventHandler(TreeView_KeyDown);
                // Begin Track #6204 - JSmith - Open Node Prop - OTS Frcst - Attempt to Drag node - results in save chgs message
                //TreeView.OnMIDNodeSelect += new MIDTreeView.MIDTreeViewNodeSelectHandler(TreeView_OnMIDNodeSelect);
                TreeView.MouseUp += new MouseEventHandler(TreeView_MouseUp);
                // End Track #6204
                //// Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
                //// Begin TT#335 - JSmith - Menu should say Remove and not Delete
                //TreeView.OnMIDNodeSelect += new MIDTreeView.MIDTreeViewNodeSelectHandler(TreeView_OnMIDNodeSelect);
                //// End TT#335
                //// Begin TT#373

                Controls.Add(this.TreeView);

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
        // Begin TT#335 - JSmith - Menu should say Remove and not Delete
        //void TreeView_OnMIDNodeSelect(object source, MIDTreeNode node)
        //{
        //    if (node.isFolderShortcut &&
        //                node.FunctionSecurityProfile.AllowDelete)
        //    {
        //        RenameMenuItem(this, eMIDMenuItem.EditDelete, sRemove);
        //        EnableMenuItem(this, eMIDMenuItem.EditDelete);
        //    }
        //    else if (node.TreeNodeType == eTreeNodeType.ChildObjectShortcutNode)
        //    {
        //        RenameMenuItem(this, eMIDMenuItem.EditDelete, sRemove);
        //        DisableMenuItem(this, eMIDMenuItem.EditDelete);
        //    }
        //    else
        //    {
        //        RenameMenuItem(this, eMIDMenuItem.EditDelete, sDelete);
        //        if (node.FunctionSecurityProfile.AllowDelete)
        //        {
        //            EnableMenuItem(this, eMIDMenuItem.EditDelete);
        //        }
        //        else
        //        {
        //            DisableMenuItem(this, eMIDMenuItem.EditDelete);
        //        }
        //    }
        //}
        // End TT#335
        // End TT#373

        // Begin Track #6204 - JSmith - Open Node Prop - OTS Frcst - Attempt to Drag node - results in save chgs message
        //void TreeView_OnMIDNodeSelect(object source, MIDTreeNode node)
        void TreeView_MouseUp(object sender, MouseEventArgs e)
        // End Track #6204
        {
            if (TreeView.RightMouseDown ||
                TreeView.MouseDownOnNode == null)
            {
                return;
            }
            object[] args = null;
            args = new object[] { SAB };
            // Begin Track #6204 - JSmith - Open Node Prop - OTS Frcst - Attempt to Drag node - results in save chgs message
            //MIDHierarchyNode selectedNode = (MIDHierarchyNode)node;
            MIDHierarchyNode selectedNode = (MIDHierarchyNode)TreeView.MouseDownOnNode;
            // End Track #6204
            frmNodeProperties formNodeProperties = (frmNodeProperties)GetForm(typeof(frmNodeProperties), args, false, true);
            if (formNodeProperties != null && formNodeProperties.Visible == true)
            {
                if (selectedNode.Profile.ProfileType != eProfileType.HierarchyNode)
                {
                    return;
                    // do nothing can not select folder nodes for display
                }
                else
                {
                    bool nodeFound = false;
                    foreach (Form frm in this.ParentForm.MdiChildren)  // see if store profile already open
                    {
                        if (frm.GetType() == typeof(frmNodeProperties))
                        {
                            frmNodeProperties fnp = (frmNodeProperties)frm;
                            if (fnp.NodeRID == selectedNode.NodeRID)
                            {
                                nodeFound = true;
                                formNodeProperties = fnp;
                                break;
                            }
                        }
                    }

                    if (nodeFound)
                    {
                        formNodeProperties.BringToFront();
                        formNodeProperties.Focus();
                    }
                    else
                    {
                        if (selectedNode.HierarchyNodeSecurityProfile.AccessDenied)
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode));
                        }
                        else
                        {
                            try
                            {
                                formNodeProperties.InitializeForm(selectedNode, selectedNode.HomeHierarchyParentRID);
                                formNodeProperties.BringToFront();
                            }
                            catch (HierarchyNodeConflictException)
                            {
                                // catch exception and do not show form
                            }
                        }
                    }
                }

                args = new object[] { SAB };
                frmHierarchyDetails formHierarchyDetails = (frmHierarchyDetails)GetForm(typeof(frmHierarchyDetails), args, false, true);
                if (formHierarchyDetails != null && formHierarchyDetails.Visible == true)
                {
                    if (selectedNode.Profile.ProfileType != eProfileType.HierarchyNode)
                    {
                        return;
                        // do nothing can not select folder nodes for display
                    }
                    else
                    {
                        formHierarchyDetails.RefreshForm(selectedNode);
                    }
                }
            }

            args = new object[] { SAB };
            frmHierarchyProperties formHierarchyProperties = (frmHierarchyProperties)GetForm(typeof(frmHierarchyProperties), args, false, true);
            if (formHierarchyProperties != null && formHierarchyProperties.Visible == true)
            {
                if (selectedNode.Profile.ProfileType != eProfileType.HierarchyNode)
                {
                    return;
                    // do nothing can not select folder nodes for display
                }
                else
                {
                    MIDHierarchyNode hierarchyNode;
                    // get hierarchy node for selected node
                    if (selectedNode.isHierarchyRoot) // stop if hierarchy
                    {
                        hierarchyNode = selectedNode;
                    }
                    else
                    {
                        hierarchyNode = (MIDHierarchyNode)selectedNode.Parent;
                        while (selectedNode.Profile.ProfileType != eProfileType.HierarchyNode)
                        {
                            hierarchyNode = (MIDHierarchyNode)hierarchyNode.Parent;
                        }
                    }
                    bool nodeFound = false;
                    foreach (Form frm in this.ParentForm.MdiChildren)  // see if store profile already open
                    {
                        if (frm.GetType() == typeof(frmHierarchyProperties))
                        {
                            frmHierarchyProperties fhp = (frmHierarchyProperties)frm;
                            if (fhp.HierarchyRID == selectedNode.HomeHierarchyRID)
                            {
                                nodeFound = true;
                                formHierarchyProperties = fhp;
                                break;
                            }
                        }
                    }

                    if (nodeFound)
                    {
                        if (selectedNode.NodeLevel > 0)		// if selected hierarchy
                        {
                            formHierarchyProperties.Level_Information_Load(selectedNode.HomeHierarchyLevel - 1);
                        }
                        else
                        {
                            formHierarchyProperties.Level_Information_Load(0);		// load 1st level
                        }
                        formHierarchyProperties.BringToFront();
                        formHierarchyProperties.Focus();
                    }
                    else
                    {
                        if (selectedNode.HierarchyNodeSecurityProfile.AccessDenied)
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
                        }
                        else
                        {
                            try
                            {
                                formHierarchyProperties.InitializeForm(selectedNode, selectedNode.HomeHierarchyLevel);
                                formHierarchyProperties.BringToFront();
                            }
                            catch (HierarchyConflictException)
                            {
                                // catch exception and do not show form
                            }
                        }
                    }
                }
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
                if (SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersMerchandise).AccessDenied)
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
				if (FormLoaded)
				{
					SAB.ClientServerSession.RefreshHierarchy();
					SAB.HierarchyServerSession.Refresh();
					SAB.ApplicationServerSession.RefreshHierarchy();
                    TreeView.LoadNodes();
				}
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

        /// <summary>
        /// Virtual method that gets the text for the New Item menu item.
        /// </summary>
        /// <returns>
        /// The text for the New Item menu item.
        /// </returns>

        override protected string GetNewItemText(MIDTreeNode aCurrentNode)
        {
            if (aCurrentNode.TreeNodeType == eTreeNodeType.MainSourceFolderNode)
            {
                return "Hierarchy";
            }
            else
            {
                return "Node";
            }
        }

        override protected void CustomizeActionMenu(MIDTreeNode aNode)
        {
            MIDHierarchyNode node;
            FunctionSecurityProfile securityNodeProperties;
            FunctionSecurityProfile securityProfile;
            FunctionSecurityProfile securityEligibility;
            FunctionSecurityProfile securityStoreGrades;
            FunctionSecurityProfile securityVelocity;
            FunctionSecurityProfile securityCapacity;
            FunctionSecurityProfile securityDailyPcts;
            FunctionSecurityProfile securityPurge;
            FunctionSecurityProfile securityNodes;

            try
            {
                node = (MIDHierarchyNode)aNode;

                cmiSearch.Visible = false;
                cmiNodeProperties.Visible = false;
                cmiHierarchyProperties.Visible = false;
                CustomizeActionMenuItem(eExplorerActionMenuItem.NewFolder, false);
                //// Begin TT#335 - JSmith - Menu should say Remove and not Delete
                //CustomizeTextMenuItem(eExplorerActionMenuItem.Delete, sDelete);
                //RenameMenuItem(this, eMIDMenuItem.EditDelete, sDelete);
                //// End TT#335

                if (node.isHierarchyNode)
                {
                    if (node.HierarchyType == eHierarchyType.organizational)
                    {
                        securityNodeProperties = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesOrgNodeProperty, (int)eSecurityTypes.All);
                        securityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyProfile, (int)eSecurityTypes.All);
                        securityEligibility = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyEligibility, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
                        securityStoreGrades = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyStoreGrades, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
                        securityVelocity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyVelocity, (int)eSecurityTypes.Allocation);
                        securityCapacity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyCapacity, (int)eSecurityTypes.Allocation);
                        securityDailyPcts = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyDailyPcts, (int)eSecurityTypes.Allocation);
                        securityPurge = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyPurge, (int)eSecurityTypes.All);
                        securityNodes = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesOrgNodes, (int)eSecurityTypes.All);
                    }
                    else if (node.HomeHierarchyOwner != Include.GlobalUserRID) 
                    {
                        securityNodeProperties = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.All);
                        securityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.All);
                        securityEligibility = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
                        securityStoreGrades = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
                        securityVelocity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.Allocation);
                        securityCapacity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.Allocation);
                        securityDailyPcts = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.Allocation);
                        securityPurge = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.All);
                        securityNodes = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.All);
                    }
                    else
                    {
                        securityNodeProperties = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltGlobalNodeProperty, (int)eSecurityTypes.All);
                        securityProfile = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyProfile, (int)eSecurityTypes.All);
                        securityEligibility = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyEligibility, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
                        securityStoreGrades = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyStoreGrades, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
                        securityVelocity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyVelocity, (int)eSecurityTypes.Allocation);
                        securityCapacity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyCapacity, (int)eSecurityTypes.Allocation);
                        securityDailyPcts = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyDailyPcts, (int)eSecurityTypes.Allocation);
                        securityPurge = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyPurge, (int)eSecurityTypes.All);
                        securityNodes = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(node.Profile.Key, eSecurityFunctions.AdminHierarchiesAltNodes, (int)eSecurityTypes.All);
                    }

                    cmiSearch.Visible = true;
                    cmiNodeProperties.Visible = true;
                    if (node.isHierarchyRoot &&
                        !node.FunctionSecurityProfile.AccessDenied)
                    {
                        cmiHierarchyProperties.Visible = true;
                    }
                    if (node.TreeNodeType == eTreeNodeType.ObjectShortcutNode)
                    {
                        // cannot add item to shortcut
                        CustomizeActionMenuItem(eExplorerActionMenuItem.NewItem, false);
                        // cannot delete shortcut except at root
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Delete, false);
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Cut, false);
                    }
                    // Begin TT#23 - JSmith - Cannot Cut/Paste referenced node in My Hierarchies
                    else if (node.isFolderShortcut &&
                        node.FunctionSecurityProfile.AllowDelete)
                    {
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Cut, true);
                        //// Begin TT#335 - JSmith - Menu should say Remove and not Delete
                        //CustomizeTextMenuItem(eExplorerActionMenuItem.Delete, sRemove);
                        //RenameMenuItem(this, eMIDMenuItem.EditDelete, sRemove);
                        //// End TT#335

                    }
                    // End TT#23

                    // Begin TT#394 - JSmith - Cut and Paste not performing consistently
                    if (node.isFolderShortcut &&
                        !node.FunctionSecurityProfile.AccessDenied)
                    {
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Copy, true);
                    }
                    // End TT#394

                    // Begin TT#1116 - JSmith - Unable to remove Alt Hier nodes from My Hierarchy
                    //if (!node.HierarchyNodeSecurityProfile.AllowDelete)
                    //{
                    //    CustomizeActionMenuItem(eExplorerActionMenuItem.Delete, false);
                    //    CustomizeActionMenuItem(eExplorerActionMenuItem.Cut, false);
                    //}
                    if ((((MIDHierarchyNode)node.Parent).HomeHierarchyRID != node.HomeHierarchyRID &&
                        !((MIDHierarchyNode)node.Parent).HierarchyNodeSecurityProfile.AllowDelete) 
                        ||
                        (((MIDHierarchyNode)node.Parent).HomeHierarchyRID == node.HomeHierarchyRID &&
                        !node.HierarchyNodeSecurityProfile.AllowDelete))
                    {
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Delete, false);
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Cut, false);
                    }
                    // End TT#1116

                    if (!node.HierarchyNodeSecurityProfile.AllowUpdate)
                    {
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Paste, false);
                    }

                    if (!securityProfile.AccessDenied ||
                        !securityEligibility.AccessDenied ||
                        !securityStoreGrades.AccessDenied ||
                        !securityVelocity.AccessDenied ||
                        !securityCapacity.AccessDenied ||
                        !securityDailyPcts.AccessDenied ||
                        !securityPurge.AccessDenied)
                    {
                        cmiNodeProperties.Visible = true;
                    }

                    if (node.HierarchyNodeSecurityProfile.AllowView)
                    {
                        CustomizeActionMenuItem(eExplorerActionMenuItem.Open, true);
                    }
                }
				else if (aNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode ||
                    aNode.TreeNodeType == eTreeNodeType.SubFolderNode)
                {
                    CustomizeActionMenuItem(eExplorerActionMenuItem.NewFolder, true);
                    CustomizeActionMenuItem(eExplorerActionMenuItem.NewItem, false);
                }
                
                if (aNode.Profile.ProfileType == eProfileType.MerchandiseMainOrganizationalFolder)
                {
                    if (aNode.Nodes.Count == 0)	// only allow one organizational hierarchy
                    {
                        CustomizeActionMenuItem(eExplorerActionMenuItem.NewItem, true);
                    }
                    else
                    {
                        CustomizeActionMenuItem(eExplorerActionMenuItem.NewItem, false);
                    }
                }

                if (aNode.Profile == null ||
                    aNode.Profile.ProfileType == eProfileType.MerchandiseMainAlternatesFolder ||
                    aNode.Profile.ProfileType == eProfileType.MerchandiseMainOrganizationalFolder ||
                    aNode.Profile.ProfileType == eProfileType.MerchandiseMainUserFolder)
                {
                    CustomizeActionMenuItem(eExplorerActionMenuItem.Paste, false);
                }

                // Begin TT#74 - JSmith - Merchandise Explorer can Drag and Drop an organizational node in to My Hierarchy, but cannot Copy and Paste
                if (aNode.TreeNodeType == eTreeNodeType.ObjectNode)
                {
                    CustomizeActionMenuItem(eExplorerActionMenuItem.Copy, true);
                }
                // End TT#74
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#384 - JSmith - Error removing child from hierarchy
        override protected bool OutOfDate(MIDTreeNode aNode)
        {
            MIDHierarchyNode node, parent;
            // Begin TT#401 - JSmith - Getting out of date message on new database
            HierarchyProfile hp;
            // End TT#401
            try
            {
                node = (MIDHierarchyNode)aNode;
                // Begin TT#401 - JSmith - Getting out of date message on new database
                if (node.NodeProfileType == eProfileType.HierarchyNode)
                {
                // End TT#401
                    if (aNode.Parent != null &&
                        ((MIDHierarchyNode)aNode.Parent).isHierarchyNode)
                    {
                        parent = (MIDHierarchyNode)aNode.Parent;
                    }
                    else
                    {
                        parent = null;
                    }

                    if (!SAB.HierarchyServerSession.NodeExists(node.NodeRID))
                    {
                        return true;
                    }
                    // Begin TT#390 - JSmith - Merchandise Explorer is out of date message when it is current
                    //else if (parent != null &&
                    //    !SAB.HierarchyServerSession.IsParentChild(node.HierarchyRID, parent.NodeRID, node.NodeRID))
                    //{
                    //    return true;
                    //}
                    //Begin TT#956 - JSmith - Hierarchy reports out of date when it is current
                    //else if (parent != null &&
                    //    node.TreeNodeType != eTreeNodeType.ChildObjectShortcutNode &&
                    //    !SAB.HierarchyServerSession.IsParentChild(node.HierarchyRID, parent.NodeRID, node.NodeRID))
                    //{
                    //    return true;
                    //}
                    //else if (parent != null &&
                    //    node.TreeNodeType == eTreeNodeType.ChildObjectShortcutNode &&
                    //    //Begin TT#xxx - JSmith - Hierarchy reports out of date when it is current
                    //    !SAB.HierarchyServerSession.IsParentChild(node.HomeHierarchyRID, parent.NodeRID, node.NodeRID))
                    //{
                    //    return true;
                    //}
                    else if (parent != null &&
                        node.TreeNodeType != eTreeNodeType.ChildObjectShortcutNode &&
                        !SAB.HierarchyServerSession.IsParentChild(parent.HierarchyRID, parent.NodeRID, node.NodeRID))
                    {
                        return true;
                    }
                    else if (parent != null &&
                        node.TreeNodeType == eTreeNodeType.ChildObjectShortcutNode &&
                        !SAB.HierarchyServerSession.IsParentChild(parent.HomeHierarchyRID, parent.NodeRID, node.NodeRID))
                    {
                        return true;
                    }
                    //End TT#956
                    // End TT#390
                    // Begin TT#401 - JSmith - Getting out of date message on new database
                    else if (parent == null &&
                        !node.GetTopNode().isMainFavoriteFolder)
                    {
                        hp = SAB.HierarchyServerSession.GetHierarchyData(node.HierarchyRID);
                        if (hp == null ||
                            hp.Key == Include.NoRID)
                        {
                            return true;
                        }
                    }
                    // End TT#401
                }

                return false;
            }
            catch
            {
                throw;
            }
        }
        // End TT#384


		private void SetText()
		{
            // Begin TT#335 - JSmith - Menu should say Remove and not Delete
            //sDelete = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Delete);
            //sRemove = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Remove);
            // End TT#335
           
		}

        private void BuildContextmenu()
        {
            cmiHierarchyProperties = new System.Windows.Forms.ToolStripMenuItem();
            cmiHierarchyProperties.Name = "cmiHierarchyProperties";
            cmiHierarchyProperties.Size = new System.Drawing.Size(195, 22);
            cmiHierarchyProperties.Text = "Hierarchy Properties...";
            cmiHierarchyProperties.Click += new System.EventHandler(this.cmiHierarchyProperties_Click);
            AddContextMenuItem(cmiHierarchyProperties, eExplorerActionMenuItem.None, eExplorerActionMenuItem.Open);

            cmiNodeProperties = new System.Windows.Forms.ToolStripMenuItem();
            cmiNodeProperties.Name = "cmiNodeProperties";
            cmiNodeProperties.Size = new System.Drawing.Size(195, 22);
            cmiNodeProperties.Text = "Node Properties...";
            cmiNodeProperties.Click += new System.EventHandler(this.cmiNodeProperties_Click);
            AddContextMenuItem(cmiNodeProperties, eExplorerActionMenuItem.None, cmiHierarchyProperties.Name);

            cmiSearch = new System.Windows.Forms.ToolStripMenuItem();
            cmiSearch.Name = "cmiSearch";
            cmiSearch.Size = new System.Drawing.Size(195, 22);
            cmiSearch.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search) + "...";
            cmiSearch.Image = MIDGraphics.GetImage(MIDGraphics.FindImage);
            cmiSearch.Click += new EventHandler(cmiSearch_Click);
            AddContextMenuItem(cmiSearch, eExplorerActionMenuItem.None, eExplorerActionMenuItem.RefreshSeparator);
        }

        private void cmiHierarchyProperties_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (MIDHierarchyNode selectedNode in TreeView.GetSelectedNodes())
                {
                    ((HierarchyTreeView)TreeView).EditHierarchyProperties(selectedNode);
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void cmiNodeProperties_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (MIDHierarchyNode selectedNode in TreeView.GetSelectedNodes())
                {
                    ((HierarchyTreeView)TreeView).EditNodeProperties(selectedNode);
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

		private void TreeView_OnMIDDoubleClick(object source, MIDTreeNode node)
		{
            ((HierarchyTreeView)TreeView).OpenForecastReview((MIDHierarchyNode)node);
        }


        protected void TreeView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F && e.Modifiers == Keys.Control)
                {
                    // Begin TT#564 - JSmith - Copy/Paste from search not working
                    //((HierarchyTreeView)TreeView).Search();
                    ((HierarchyTreeView)TreeView).Search(EAB);
                    // End TT#564 
                    e.Handled = true;
                }
            }
            catch
            {
                throw;
            }
        }

        private void DisplayMessages(EditMsgs em)
        {
            MIDRetail.Windows.DisplayMessages.Show(em, SAB, Include.MIDMerchandiseExplorer);
        }

		#region IFormBase Members

		#endregion

        private void MerchandiseExplorer_Enter(object sender, EventArgs e)
        {
            // make sure menu is set
            HideMenuItem(this, eMIDMenuItem.EditClear);
            HideMenuItem(this, eMIDMenuItem.FileSave);
            HideMenuItem(this, eMIDMenuItem.FileSaveAs);
        }

        private void MerchandiseExplorer_Leave(object sender, EventArgs e)
        {
            // undo all menu changes
            ShowMenuItem(this, eMIDMenuItem.EditClear);
            ShowMenuItem(this, eMIDMenuItem.FileSave);
            ShowMenuItem(this, eMIDMenuItem.FileSaveAs);
            EnableMenuItem(this, eMIDMenuItem.EditDelete);
        }

        void cmiSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // Begin TT#564 - JSmith - Copy/Paste from search not working
                //((HierarchyTreeView)TreeView).Search();
                ((HierarchyTreeView)TreeView).Search(EAB);
                // End TT#564
			}
			catch
			{
				throw;
			}
		}

        public void DisplayNode(int hnRID, int hierarchyRID)
        {
            ((HierarchyTreeView)TreeView).LocateAndDisplayNode(hnRID, hierarchyRID);
        }
        public void ClearSelectedNode()
        {
            ((HierarchyTreeView)TreeView).ClearSelectedNode();
        }
	}
}
