using System;
using System.Collections;
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
	public partial class AssortmentExplorer : ExplorerBase
	{
		private System.Windows.Forms.ToolStripMenuItem cmiAssortmentDetails;
		private System.Windows.Forms.ToolStripMenuItem cmiReviewSelection;

		public AssortmentExplorer(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB, Form aMainMDIForm)
			: base(aSAB, aEAB, aMainMDIForm)
		{
			aEAB.AssortmentExplorer = this;
		}



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
				TreeView = new AssortmentTreeView(EAB);
				TreeView.InitializeTreeView(SAB, false, MainMDIForm);

				TreeView.AllowDrop = true;
				TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
				TreeView.ImageList = this.imageListAssortment;
				TreeView.LabelEdit = true;
				TreeView.Location = new System.Drawing.Point(0, 0);
				TreeView.Name = "TreeView";
				TreeView.PathSeparator = ".";
				TreeView.Size = new System.Drawing.Size(216, 352);
				TreeView.TabIndex = 0;

				TreeView.OnMIDNodeSelect += new MIDTreeView.MIDTreeViewNodeSelectHandler(this.AssortmentTreeView_OnMIDNodeSelect);
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
                if (SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortment).AccessDenied)
                {
                    return;
                }
                // End TT#2

				BuildContextmenu();
				TreeView.LoadNodes();
				// Begin TT#1227 - stodd
				((AssortmentTreeView)TreeView).InitialExpand();
				// End TT#1227 - stodd
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

		/// <summary>
		/// Virtual method that gets the text for the New Item menu item.
		/// </summary>
		/// <returns>
		/// The text for the New Item menu item.
		/// </returns>

		override protected string GetNewItemText(MIDTreeNode aCurrentNode)
		{
			return "Assortment";   
		}

		protected override void CustomizeActionMenu(MIDTreeNode aNode)
		{
            // BEGIN TT#2026-MD - AGallagher - Asst Explorer - Right Click>Select In Use and receive System Exception
		    try
		    {
                CustomizeActionMenuItem(eExplorerActionMenuItem.InUse, false);
            // END TT#2026-MD - AGallagher - Asst Explorer - Right Click>Select In Use and receive System Exception
		//        cmiStoreProfiles.Visible = true;
		//        cmiStoreCharacteristics.Visible = true;
		//        cmiRemoveStoreFromSet.Visible = false;

		//        switch (aNode.NodeProfileType)
		//        {
		//            case eProfileType.Store:
		//                CustomizeActionMenuItem(eExplorerActionMenuItem.Cut, false);
		//                CustomizeActionMenuItem(eExplorerActionMenuItem.Copy, false);
		//                CustomizeActionMenuItem(eExplorerActionMenuItem.Delete, false);
		//                cmiRemoveStoreFromSet.Visible = true;
		//                cmiStoreProfiles.Visible = false;
		//                break;

		//            case eProfileType.StoreGroupLevel:
		//                CustomizeActionMenuItem(eExplorerActionMenuItem.Copy, false);
		//                break;
		//        }
            // BEGIN TT#2026-MD - AGallagher - Asst Explorer - Right Click>Select In Use and receive System Exception
            }
            catch
            {
                throw;
            }
            // END TT#2026-MD - AGallagher - Asst Explorer - Right Click>Select In Use and receive System Exception
        }

		//----------------
		// Private methods
		//----------------

		private void BuildContextmenu()
		{
			try
			{
				cmiAssortmentDetails = new System.Windows.Forms.ToolStripMenuItem();
				cmiAssortmentDetails.Name = "cmiAssortmentDetails";
				cmiAssortmentDetails.Size = new System.Drawing.Size(195, 22);
				cmiAssortmentDetails.Text = "Assortment Workspace";
				cmiAssortmentDetails.Click += new System.EventHandler(this.cmiAssortmentDetails_Click);
				AddContextMenuItem(cmiAssortmentDetails, eExplorerActionMenuItem.None, eExplorerActionMenuItem.Open);

                // BEGIN TT#1985-MD - AGallagher - Select Review Selection and receive a Severe Error - Invalid Date Range
                //cmiReviewSelection = new System.Windows.Forms.ToolStripMenuItem();
                //cmiReviewSelection.Name = "cmiReviewSelection";
                //cmiReviewSelection.Size = new System.Drawing.Size(195, 22);
                //cmiReviewSelection.Text = "Review Selection";
                //cmiReviewSelection.Click += new System.EventHandler(this.cmiReviewSelection_Click);
                //AddContextMenuItem(cmiReviewSelection, eExplorerActionMenuItem.None, eExplorerActionMenuItem.Open);
                // END TT#1985-MD - AGallagher - Select Review Selection and receive a Severe Error - Invalid Date Range
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void cmiAssortmentReview_Click(object sender, EventArgs e)
		{
			try
			{
				foreach (MIDAssortmentNode selectedNode in TreeView.GetSelectedNodes())
				{
					((AssortmentTreeView)TreeView).OpenAssortmentReview(selectedNode);
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cmiAssortmentDetails_Click(object sender, EventArgs e)
		{
			try
			{
				ArrayList selectedNodes = TreeView.GetSelectedNodes();
				if (selectedNodes.Count > 0)
				{
					MIDAssortmentNode selectedNode = (MIDAssortmentNode)selectedNodes[0];
					// Opens Assortment Workspace
					((AssortmentTreeView)TreeView).OpenAssortmentWorkspace(selectedNode);
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cmiReviewSelection_Click(object sender, EventArgs e)
		{
			try
			{
				ArrayList selectedNodes = TreeView.GetSelectedNodes();

				((AssortmentTreeView)TreeView).OpenReviewSelection(selectedNodes);
				
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		protected void AssortmentTreeView_OnMIDNodeSelect(object source, MIDTreeNode node)
		{
			//StoreProfileMaint storeProfileMaint;

			try
			{
				if (node != null)
				{
					//if (node.NodeProfileType == eProfileType.Store)
					//{
					//    storeProfileMaint = GetStoreProfileMaintWindow();

					//    if (storeProfileMaint != null)
					//    {
					//        storeProfileMaint.ShowStore(node.Profile.Key);
					//    }
					//}
				}
			}
			catch (Exception error)
			{
				HandleException(error);
			}
		}
	}
}
