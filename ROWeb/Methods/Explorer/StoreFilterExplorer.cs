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
    public partial class ROStoreFilterExplorer : ROBaseExplorer
    {
        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROStoreFilterExplorer(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {
            _treeView = new FilterStoreTreeView();
        }

        override public void CleanUp()
        {

        }

        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                case eRORequest.GetStoreFilterExplorerData:
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

    #region "RO TreeNode Security Group Class"
    public class ROTreeNodeSecurityGroup : ICloneable
    {
        //=======
        // FIELDS
        //=======

        private FunctionSecurityProfile _functionSecurityProfile;
        private FunctionSecurityProfile _folderSecurityProfile;

        //=============
        // CONSTRUCTORS
        //=============

        public ROTreeNodeSecurityGroup()
        {
            _functionSecurityProfile = null;
            _folderSecurityProfile = null;
        }

        public ROTreeNodeSecurityGroup(FunctionSecurityProfile aFunctionSecurityProfile, FunctionSecurityProfile aFolderSecurityProfile)
        {
            _functionSecurityProfile = aFunctionSecurityProfile;
            _folderSecurityProfile = aFolderSecurityProfile;
        }

        //===========
        // PROPERTIES
        //===========

        public FunctionSecurityProfile FunctionSecurityProfile
        {
            get
            {
                return _functionSecurityProfile;
            }
            set
            {
                _functionSecurityProfile = value;
            }
        }

        public FunctionSecurityProfile FolderSecurityProfile
        {
            get
            {
                return _folderSecurityProfile;
            }
            set
            {
                _folderSecurityProfile = value;
            }
        }

        //========
        // METHODS
        //========

        public object Clone()
        {
            ROTreeNodeSecurityGroup newSecGrp;

            try
            {
                newSecGrp = new ROTreeNodeSecurityGroup();

                if (_functionSecurityProfile != null)
                {
                    newSecGrp._functionSecurityProfile = (FunctionSecurityProfile)_functionSecurityProfile.Clone();
                }

                if (_folderSecurityProfile != null)
                {
                    newSecGrp._folderSecurityProfile = (FunctionSecurityProfile)_folderSecurityProfile.Clone();
                }

                return newSecGrp;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }

    #endregion
}
