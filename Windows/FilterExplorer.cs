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
	public partial class FilterExplorer : ExplorerBase
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public FilterExplorer(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB, Form aMainMDIForm)
			: base(aSAB, aEAB, aMainMDIForm)
		{
			aEAB.FilterExplorer = this;
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
				TreeView = new FilterTreeView();
				TreeView.InitializeTreeView(SAB, false, MainMDIForm);

				TreeView.AllowDrop = true;
				TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
				TreeView.ImageList = this.imageListFilter;
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
				TreeView.LoadNodes();
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

        //BEGIN TT#406-MD -jsobek -The User's Filter Explorer is not refreshed when a new Filter is added through the Filter Wizard
        public void RefreshData()
        {
            //Be nice and keep a list of expanded nodes.
            System.Collections.Generic.List<string> sList = new System.Collections.Generic.List<string>();
            foreach (TreeNode n in TreeView.Nodes)
            {
                if (n.IsExpanded)
                {
                    sList.Add(n.Text);
                }
            }

            RefreshTreeView();

            foreach (TreeNode n in TreeView.Nodes)
            {
                if (sList.Contains(n.Text))
                {
                    n.Expand();
                }
            }
        }
        //BEGIN TT#406-MD -jsobek -The User's Filter Explorer is not refreshed when a new Filter is added through the Filter Wizard

		/// <summary>
		/// Virtual method that gets the text for the New Item menu item.
		/// </summary>
		/// <returns>
		/// The text for the New Item menu item.
		/// </returns>

		override protected string GetNewItemText(MIDTreeNode aCurrentNode)
		{
			return "Filter";
		}

		//----------------
		// Private methods
		//----------------

		//private void OnFilterPropertiesSave(object source, FilterPropertiesSaveEventArgs e)
		//{
		//    MIDFilterNode node;
		//    MIDFilterNode parentNode;

		//    try
		//    {
		//        node = (MIDFilterNode)TreeView.ItemNodeHash[e.FilterProfile.Key];

		//        if (node == null)
		//        {
		//            switch (e.ParentNode.NodeProfileType)
		//            {
		//                case eProfileType.StoreFilter:

		//                    parentNode = (MIDFilterNode)e.ParentNode.Parent;
		//                    break;

		//                default:

		//                    parentNode = e.ParentNode;
		//                    break;
		//            }

		//            node = ((FilterTreeView)TreeView).BuildFilterNode(e.FilterProfile, parentNode, e.FilterProfile.UserRID);

		//            TreeView.ItemNodeHash[e.FilterProfile.Key] = node;
		//            parentNode.Nodes.Add(node);
		//            TreeView.SelectedNode = node;
		//        }
		//        else
		//        {
		//            parentNode = (MIDFilterNode)node.Parent;
		//            node.Text = e.FilterProfile.Name;
		//        }

		//        TreeView.RefreshShortcuts(TreeView.FavoritesNode, parentNode);
		//        TreeView.SortChildNodes(parentNode);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
	}
}
