using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Windows;
using MIDRetail.Windows.Controls;

namespace Logility.ROWeb
{
    public partial class ROStoreGroupExplorer : ROBaseExplorer
    {
        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROStoreGroupExplorer(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
		{
            _treeView = new StoreTreeView(aEAB: null);
        }

        override public void CleanUp()
        {

        }

        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                case eRORequest.StoreGroupExplorerData:
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
                SAB.StoreServerSession.Refresh();
                SAB.ApplicationServerSession.Refresh();
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
