using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows;
using MIDRetail.Windows.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Logility.ROWeb
{
    public partial class ROHeaderFilterExplorer : ROBaseExplorer
    {
       
        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROHeaderFilterExplorer(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {
            _treeView = new FilterHeaderTreeView();
        }

        override public void CleanUp()
        {

        }

        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                case eRORequest.GetHeaderFilterExplorerData:
                    return GetExplorerData(Parms: (ROTreeNodeParms)Parms);
                case eRORequest.RefreshExplorerData:
                    return RefreshExplorerData(Parms: (ROTreeNodeParms)Parms);
            }

            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
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

        override protected eProfileType GetFolderType(eProfileType parentProfileType, int parentKey, int parentUserKey, eProfileType profileType, string uniqueID)
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
