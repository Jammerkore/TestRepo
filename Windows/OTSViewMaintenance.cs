using System;
using System.Drawing;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.Configuration;
using System.Runtime.InteropServices; // TT#609-MD - RMatelic - OTS Forecast Chain Ladder View change explorers for Windows arrow expanders

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public class OTSViewMaintenance : MIDFormBase
	{
		#region Fields
		private SessionAddressBlock _sab;
        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
        //private FunctionSecurityProfile _globalFunctionSecurity;
        //private FunctionSecurityProfile _userFunctionSecurity;
        private FunctionSecurityProfile _forecastViewsGlobalSecurity;
        private FunctionSecurityProfile _forecastViewsUserSecurity;
        // End TT#231
      	private Audit _audit;
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		private TreeNode _tooltipNode = null;
		//End Track #4815
        
        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
        private FunctionSecurityProfile _allocViewsGlobalFunctionSecurity;
        private FunctionSecurityProfile _allocViewsUserFunctionSecurity;
        private FunctionSecurityProfile _allocViewsGlobalVelocitySecurity;
        private FunctionSecurityProfile _allocViewsUserVelocitySecurity;
        private FunctionSecurityProfile _allocViewsGlobalVelocityDetailSecurity;
        private FunctionSecurityProfile _allocViewsUserVelocityDetailSecurity;
        private string _forecastText;
        private string _forecastGlobalText;
        private string _forecastUserText;
        private string _allocationText;
        private string _velocityMethodText;
        private string _velocityStoreDetailText;
        private string _velocityGlobalText;
        private string _velocityUserText;
        private string _velocityDetailGlobalText;
        private string _velocityDetailUserText;
        // End TT#231 
        // Begin TT#454 - RMatelic - Add Views in Style Review
        private FunctionSecurityProfile _allocViewsGlobalStyleReviewSecurity;
        private FunctionSecurityProfile _allocViewsUserStyleReviewSecurity;
        private string _styleReviewText;
        private string _styleReviewGlobalText;
        private string _styleReviewUserText;
        GridViewData _gridViewData;
        // End TT#454  
        // Begin TT#456 - RMatelic - Add Views in Size Review
        private FunctionSecurityProfile _allocViewsGlobalSizeReviewSecurity;
        private FunctionSecurityProfile _allocViewsUserSizeReviewSecurity;
        private string _sizeReviewText;
        private string _sizeReviewGlobalText;
        private string _sizeReviewUserText;
        // End TT#456  
        // Begin TT#1181 - RMatelic - Assortment View Maintenance
        private FunctionSecurityProfile _assortViewsGlobalAssortReviewSecurity;
        private FunctionSecurityProfile _assortViewsUserAssortReviewSecurity;
        private string _assortmentText;
        //private string _assortReviewText;
        private string _assortReviewGlobalText;
        private string _assortReviewUserText;
        private AssortmentViewData _assortViewData;
        // End TT#1181
		// Begin TT#1077 - MD - stodd - cannot create GA views 
        private FunctionSecurityProfile _groupAllocationViewsGlobalGroupAllocationReviewSecurity;
        private FunctionSecurityProfile _groupAllocationViewsUserGroupAllocationReviewSecurity;
        private string _groupAllocationText;
        private string _groupAllocationReviewGlobalText;
        private string _groupAllocationReviewUserText;
		// End TT#1077 - MD - stodd - cannot create GA views 
        // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
        private FunctionSecurityProfile _allocViewsGlobalAllocWorkspaceViewSecurity;
        private FunctionSecurityProfile _allocViewsUserAllocWorkspaceViewSecurity;
        private string _allocWorkspaceViewText;
        private string _allocWorkspaceViewGlobalText;
        private string _allocWorkspaceViewUserText;
        private string _assortmentReviewText;
        private FunctionSecurityProfile _asrtViewsGlobalAsrtWorkspaceViewSecurity;
        private FunctionSecurityProfile _asrtViewsUserAsrtWorkspaceViewSecurity;
        private string _asrtWorkspaceViewText;
        private string _asrtWorkspaceViewGlobalText;
        private string _asrtWorkspaceViewUserText;
        // End TT#1411-MD  

		#endregion

		#region Constructors
		public OTSViewMaintenance(SessionAddressBlock SAB) : base(SAB)
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			_sab = SAB;
			_forecastViewsGlobalSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsGlobal);
			_forecastViewsUserSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsUser);
			_audit = _sab.ClientServerSession.Audit;

            // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
            _allocViewsGlobalFunctionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobal);
            _allocViewsUserFunctionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUser);
            _allocViewsGlobalVelocitySecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalVelocity);
            _allocViewsUserVelocitySecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUserVelocity);
            _allocViewsGlobalVelocityDetailSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalVelocityDetail);
            _allocViewsUserVelocityDetailSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUserVelocityDetail);

            // Begin TT#454 - RMatelic - Add Views in Style Review
            _allocViewsGlobalStyleReviewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalStyleReview);
            _allocViewsUserStyleReviewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUserStyleReview);
            _styleReviewText = MIDText.GetTextOnly((int)eSecurityFunctions.AllocationViewsGlobalStyleReview);
            _styleReviewGlobalText = "StyleReviewGlobalViews";
            _styleReviewUserText = "StyleReviewUserViews";
            _gridViewData = new GridViewData();
            // End TT#454  

            _forecastText = MIDText.GetTextOnly((int)eSecurityFunctions.Forecast);
            _allocationText = MIDText.GetTextOnly(eMIDTextCode.lbl_Allocation);
            _velocityMethodText = MIDText.GetTextOnly((int)eSecurityFunctions.AllocationViewsGlobalVelocity);
            _velocityStoreDetailText = MIDText.GetTextOnly((int)eSecurityFunctions.AllocationViewsGlobalVelocityDetail);
            _forecastGlobalText = "ForecastGlobalViews";
            _forecastUserText = "ForecastUserViews";
            _velocityGlobalText = "VelocityGlobalViews";
            _velocityUserText = "VelocityUserViews";
            _velocityDetailGlobalText = "VelocityDetailGlobalViews";
            _velocityDetailUserText = "VelocityDetailUserViews";
            // End TT#231  

            // Begin TT#456 - RMatelic - Add Views in Size Review
            _allocViewsGlobalSizeReviewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalSizeReview);
            _allocViewsUserSizeReviewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUserSizeReview);
            _sizeReviewText = MIDText.GetTextOnly((int)eSecurityFunctions.AllocationViewsGlobalSizeReview);
            _sizeReviewGlobalText = "SizeReviewGlobalViews";
            _sizeReviewUserText = "SizeReviewUserViews";
            // End TT#456  

            // Begin TT#1181 - RMatelic - Assortment View Maintenance
            // Begin TT#1995-MD - JSmith - Security - Assortment Views
            //_assortViewsGlobalAssortReviewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentViewsGlobalAssortmentReview);    // TT#1409-md - stodd - assortment view security wrong
            _assortViewsGlobalAssortReviewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentViewsGlobal);
            // End TT#1995-MD - JSmith - Security - Assortment Views
            _assortViewsUserAssortReviewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentViewsUser);
            _assortmentText = MIDText.GetTextOnly(eMIDTextCode.lbl_Assortment);
            _assortReviewGlobalText = "AssortmentReviewGlobalViews";
            _assortReviewUserText = "AssortmentReviewUserViews";
            _assortViewData = new AssortmentViewData();
            // End TT#1181

			// Begin TT#1077 - MD - stodd - cannot create GA views 
            // Begin TT#1995-MD - JSmith - Security - Assortment Views
            //_groupAllocationViewsGlobalGroupAllocationReviewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentViewsUserAssortmentReview);    // TT#1409-md - stodd - assortment view security wrong
            _groupAllocationViewsGlobalGroupAllocationReviewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentViewsUser);
            // End TT#1995-MD - JSmith - Security - Assortment Views
            _groupAllocationViewsUserGroupAllocationReviewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.GroupAllocationViewsUser);
            _groupAllocationText = MIDText.GetTextOnly(eMIDTextCode.lbl_GroupAllocation);
            _groupAllocationReviewGlobalText = "GroupAllocationReviewGlobalViews";
            _groupAllocationReviewUserText = "GroupAllocationReviewUserViews";
			// End TT#1077 - MD - stodd - cannot create GA views 

            // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
            _allocViewsGlobalAllocWorkspaceViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalWorkspace);
            // Begin TT#5738 - JSmith - Permissions for Deleting Views
            //_allocViewsUserAllocWorkspaceViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalWorkspace);
            _allocViewsUserAllocWorkspaceViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUserWorkspace);
            // End TT#5738 - JSmith - Permissions for Deleting Views
            _allocWorkspaceViewText = MIDText.GetTextOnly((int)eSecurityFunctions.AllocationViewsGlobalWorkspace);
            _allocWorkspaceViewGlobalText = "AllocWorkspaceGlobalViews";
            _allocWorkspaceViewUserText = "AllocWorkspaceUserViews";
            // Begin TT#1995-MD - JSmith - Security - Assortment Views
            //_assortmentReviewText = MIDText.GetTextOnly((int)eSecurityFunctions.AssortmentViewsGlobalAssortmentReview);
            _assortmentReviewText = MIDText.GetTextOnly((int)eSecurityFunctions.AssortmentViewsGlobal);
            // End TT#1995-MD - JSmith - Security - Assortment Views
            _asrtViewsGlobalAsrtWorkspaceViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentWorkspaceViewsGlobal);
            _asrtViewsUserAsrtWorkspaceViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentWorkspaceViewsUser);
            _asrtWorkspaceViewText = MIDText.GetTextOnly((int)eSecurityFunctions.ExplorersAssortmentWorkspace);
            _asrtWorkspaceViewGlobalText = "AsrtWorkspaceGlobalViews";
            _asrtWorkspaceViewUserText = "AsrtWorkspaceUserViews";
            // End TT#1411-MD  
        }
		#endregion

		#region Windows Form Designer generated code
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.TreeView trvViewMaintenance;
		private System.Windows.Forms.ContextMenu viewMenu;
		private System.Windows.Forms.MenuItem mnuDeleteView;
		private System.Windows.Forms.Button btnClose;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
                this.trvViewMaintenance.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.trvViewMaintenance_KeyPress);
				this.trvViewMaintenance.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.trvViewMaintenance_MouseDown);
				this.trvViewMaintenance.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.trvViewMaintenance_AfterSelect);
				this.trvViewMaintenance.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.trvViewMaintenance_MouseMove);
                this.trvViewMaintenance.AfterCollapse -= new System.Windows.Forms.TreeViewEventHandler(this.trvViewMaintenance_AfterCollapse);
                this.trvViewMaintenance.AfterExpand -= new System.Windows.Forms.TreeViewEventHandler(this.trvViewMaintenance_AfterExpand);
                this.btnDelete.Click -= new System.EventHandler(this.btnDelete_Click);
                this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
                this.Load -= new System.EventHandler(this.OTSViewMaintenance_Load);
			}
			base.Dispose( disposing );
		}
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.trvViewMaintenance = new System.Windows.Forms.TreeView();
            this.viewMenu = new System.Windows.Forms.ContextMenu();
            this.mnuDeleteView = new System.Windows.Forms.MenuItem();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // trvViewMaintenance
            // 
            this.trvViewMaintenance.ContextMenu = this.viewMenu;
            this.trvViewMaintenance.Location = new System.Drawing.Point(0, 8);
            this.trvViewMaintenance.Name = "trvViewMaintenance";
            this.trvViewMaintenance.Size = new System.Drawing.Size(296, 600);
            this.trvViewMaintenance.TabIndex = 0;
            this.trvViewMaintenance.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.trvViewMaintenance_AfterCollapse);
            this.trvViewMaintenance.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvViewMaintenance_AfterSelect);
            this.trvViewMaintenance.MouseMove += new System.Windows.Forms.MouseEventHandler(this.trvViewMaintenance_MouseMove);
            this.trvViewMaintenance.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trvViewMaintenance_MouseDown);
            this.trvViewMaintenance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trvViewMaintenance_KeyPress);
            this.trvViewMaintenance.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.trvViewMaintenance_AfterExpand);
            // 
            // viewMenu
            // 
            this.viewMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuDeleteView});
            this.viewMenu.Popup += new System.EventHandler(this.viewMenu_Popup);
            // 
            // mnuDeleteView
            // 
            this.mnuDeleteView.Index = 0;
            this.mnuDeleteView.Text = "Delete View";
            this.mnuDeleteView.Click += new System.EventHandler(this.mnuDeleteView_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(128, 616);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(216, 616);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // OTSViewMaintenance
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(296, 645);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.trvViewMaintenance);
            this.Name = "OTSViewMaintenance";
            this.Text = "View Maintenance";
            this.Load += new System.EventHandler(this.OTSViewMaintenance_Load);
            this.Controls.SetChildIndex(this.trvViewMaintenance, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);

		}
        // Begin TT#609-MD - RMatelic - OTS Forecast Chain Ladder View change explorers for Windows arrow expanders
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        protected override void CreateHandle()
        {
            base.CreateHandle();

            SetWindowTheme(this.trvViewMaintenance.Handle, "explorer", null);
        }
        // End TT#609-MD 
		#endregion

		#region Events
		public override void IRefresh()
		{
			LoadTreeView();
			base.IRefresh ();
		}


		private void OTSViewMaintenance_Load(object sender, System.EventArgs e)
		{
			trvViewMaintenance.ImageList = MIDGraphics.ImageList;
			LoadTreeView();
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if(trvViewMaintenance.SelectedNode.Text == MIDText.GetTextOnly(eMIDTextCode.msg_UserViews))
				DeleteAllUserViews();
			else
				DeleteView();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void mnuDeleteView_Click(object sender, System.EventArgs e)
		{
			if(trvViewMaintenance.SelectedNode.Text == MIDText.GetTextOnly(eMIDTextCode.msg_UserViews))
				DeleteAllUserViews();
			else
				DeleteView();
		}

		private void trvViewMaintenance_KeyPress(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Delete)
			{
				if(!HasPermission())
					return;

				if(trvViewMaintenance.SelectedNode.Text == MIDText.GetTextOnly(eMIDTextCode.msg_UserViews))
					DeleteAllUserViews();
				else
					DeleteView();
			}
		}

		private void viewMenu_Popup(object sender, EventArgs e)
		{
			bool hasPermission = HasPermission();

			if(trvViewMaintenance.SelectedNode.Text == MIDText.GetTextOnly(eMIDTextCode.msg_GlobalViews))
			{
				viewMenu.MenuItems[0].Enabled = false;
				viewMenu.MenuItems[0].Text="Delete View";
			}
			else if(trvViewMaintenance.SelectedNode.Text == MIDText.GetTextOnly(eMIDTextCode.msg_UserViews))
			{
				viewMenu.MenuItems[0].Enabled = hasPermission;
				viewMenu.MenuItems[0].Text="Delete All User Views";
			}
			else
			{
				viewMenu.MenuItems[0].Enabled = hasPermission;
				viewMenu.MenuItems[0].Text="Delete View";
			}
		}

		private void trvViewMaintenance_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				TreeNode selectedNode = trvViewMaintenance.GetNodeAt(e.X, e.Y);
				if(selectedNode != null)
					trvViewMaintenance.SelectedNode = selectedNode;
			}
		}

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		private void trvViewMaintenance_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			TreeNode currNode = (TreeNode)trvViewMaintenance.GetNodeAt(trvViewMaintenance.PointToClient(Cursor.Position));
			if (currNode == null)
			{
				_tooltipNode = null;
				ToolTip.Active = false; //turn it off 
				ToolTip.SetToolTip(trvViewMaintenance,"");
				return;
			}

			ViewNodeTag nodeTag = (ViewNodeTag)currNode.Tag;
   
			string message;
			if (currNode != _tooltipNode)
			{
				// moved to a new node so show the tip
				if (nodeTag.OwnerUserRID != Include.NoRID &&
					nodeTag.OwnerUserRID != nodeTag.UserRID)
				{
					ToolTip.Active = true;
					message = MIDText.GetText(eMIDTextCode.msg_SharedBy);
					message = message.Replace("{0}", SAB.ClientServerSession.GetUserName(nodeTag.OwnerUserRID));
					ToolTip.SetToolTip(trvViewMaintenance,message);
				}
				else 
				{
					ToolTip.Active = false; //turn it off 
					ToolTip.SetToolTip(trvViewMaintenance,"");
				}
				_tooltipNode = currNode;
			}		
		}
		//End Track #4815

		private void trvViewMaintenance_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if(trvViewMaintenance.SelectedNode.Text == MIDText.GetTextOnly(eMIDTextCode.msg_GlobalViews) || !HasPermission())
				btnDelete.Enabled = false;
			else
				btnDelete.Enabled = true;
		}

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
        private void trvViewMaintenance_AfterExpand(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Name == _forecastText || e.Node.Name == _allocationText ||
                    e.Node.Name == _assortmentText ||                                    // TT#1181 - RMatelic - Assortment View Maintenance    
                    e.Node.Name == _velocityMethodText || e.Node.Name == _velocityStoreDetailText ||
                    e.Node.Name == _styleReviewText || e.Node.Name == _sizeReviewText)   // TT#454/#456 - add _styleReviewText & _sizeReviewText
                {
                    e.Node.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.OpenTreeFolder);
                    e.Node.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.OpenTreeFolder);
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void trvViewMaintenance_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Name == _forecastText || e.Node.Name == _allocationText ||
                    e.Node.Name == _assortmentText ||                                    // TT#1181 - RMatelic - Assortment View Maintenance 
                    e.Node.Name == _velocityMethodText || e.Node.Name == _velocityStoreDetailText ||
                    e.Node.Name == _styleReviewText || e.Node.Name == _sizeReviewText)   // TT#454/#456 - add _styleReviewText & _sizeReviewText
                {
                    e.Node.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.ClosedTreeFolder);
                    e.Node.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.ClosedTreeFolder);
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
        // End TT#231  
		#endregion

		#region Private Methods
		private void ShowDeleteError()
		{
			_audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_DeleteFailed + " Invalid View_RID.", "OTSViewMaintenance.cs");
            MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_DeleteFailed), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void ShowDeleteError(string message)
		{
            MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private bool HasPermission()
		{ 
			TreeNode selectedNode = trvViewMaintenance.SelectedNode;

            // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail  
            //if(selectedNode.Parent != null)
            //{
            //    if(trvViewMaintenance.SelectedNode.Parent.Text == MIDText.GetTextOnly(eMIDTextCode.msg_GlobalViews))
            //        return _forecastViewsGlobalSecurity.AllowDelete;
            //    else
            //        return _forecastViewsUserSecurity.AllowDelete;
            //}
            //else
            //{
            //    if(trvViewMaintenance.SelectedNode.Text == MIDText.GetTextOnly(eMIDTextCode.msg_UserViews))
            //        return _forecastViewsUserSecurity.AllowDelete;
            //    else
            //        return false;
            //}

            if (selectedNode.Parent == null || selectedNode.Parent.Name == _allocationText ||
                selectedNode.Text == MIDText.GetTextOnly(eMIDTextCode.msg_GlobalViews))
            {
                return false;
			}
            else
            {
                return CanDeleteNode(selectedNode);
            }
        }   // End TT#231

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
        private bool CanDeleteNode(TreeNode aSelectedNode)
        {
            bool allowDelete = false;
            try
            {
                if (aSelectedNode.Parent.Name == _forecastGlobalText)   // single forecast global view
                {
                    allowDelete = _forecastViewsGlobalSecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _forecastText)    // all forecast user views
                {
                    if (aSelectedNode.Nodes.Count > 0)
                    {
                        allowDelete = _forecastViewsUserSecurity.AllowDelete;
                    }
                }
                else if (aSelectedNode.Parent.Name == _forecastUserText)  //forecast user views
                {
                    allowDelete = _forecastViewsUserSecurity.AllowDelete;
                }    
                else if (aSelectedNode.Parent.Name == _velocityMethodText)
                {
                     if (aSelectedNode.Text == MIDText.GetTextOnly(eMIDTextCode.msg_UserViews)) // all velocity method user views 
                     {
                         if (aSelectedNode.Nodes.Count > 0)
                         {
                             allowDelete = _allocViewsUserVelocitySecurity.AllowDelete;
                         }   
                     } 
                }
                else if (aSelectedNode.Parent.Name == _velocityGlobalText)
                {
                    allowDelete = _allocViewsGlobalVelocitySecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _velocityUserText)
                {
                    allowDelete = _allocViewsUserVelocitySecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _velocityDetailGlobalText)
                {
                    allowDelete = _allocViewsGlobalVelocityDetailSecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _velocityDetailUserText)
                {
                    allowDelete = _allocViewsUserVelocityDetailSecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _velocityStoreDetailText)
                {
                    if (aSelectedNode.Nodes.Count > 0)
                    {
                        allowDelete = _allocViewsUserVelocityDetailSecurity.AllowDelete;
                    }
                }
                // Begin TT#454 - RMatelic - Add Views in Style Review
                else if (aSelectedNode.Parent.Name == _styleReviewGlobalText)
                {
                    allowDelete = _allocViewsGlobalStyleReviewSecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _styleReviewUserText)
                {
                    allowDelete = _allocViewsUserStyleReviewSecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _styleReviewText)
                {
                    if (aSelectedNode.Nodes.Count > 0)
                    {
                        allowDelete = _allocViewsUserStyleReviewSecurity.AllowDelete;
                    }
                }
                // End TT#454  
                // Begin TT#456 - RMatelic - Add Views to Size Review
                else if (aSelectedNode.Parent.Name == _sizeReviewGlobalText)
                {
                    allowDelete = _allocViewsGlobalSizeReviewSecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _sizeReviewUserText)
                {
                    allowDelete = _allocViewsUserSizeReviewSecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _sizeReviewText)
                {
                    if (aSelectedNode.Nodes.Count > 0)
                    {
                        allowDelete = _allocViewsUserSizeReviewSecurity.AllowDelete;
                    }
                }
                // End TT#456  
                // Begin TT#1181 - RMatelic - Assortment View Maintenance
                else if (aSelectedNode.Parent.Name == _assortReviewGlobalText)
                {
                    int viewRID = ((ViewNodeTag)aSelectedNode.Tag).ViewRID;
                    if (viewRID == Include.DefaultAssortmentViewRID)    // disallow Delete of Default Assortment View
                    {
                        allowDelete = false;
                    }
                    else
                    {
                        allowDelete = _assortViewsGlobalAssortReviewSecurity.AllowDelete;
                    }
                }
                else if (aSelectedNode.Parent.Name == _assortReviewUserText)
                {
                    allowDelete = _assortViewsUserAssortReviewSecurity.AllowDelete;
                }
                //else if (aSelectedNode.Parent.Name == _assortmentText)        // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
                else if (aSelectedNode.Parent.Name ==_assortmentReviewText)     // End TT#1411-MD
                {
                    if (aSelectedNode.Nodes.Count > 0)
                    {
                        allowDelete = _assortViewsUserAssortReviewSecurity.AllowDelete;
                    }
                }
                // End TT#1181 
				// Begin TT#1077 - MD - stodd - cannot create GA views 
                else if (aSelectedNode.Parent.Name == _groupAllocationReviewGlobalText)
                {
                    int viewRID = ((ViewNodeTag)aSelectedNode.Tag).ViewRID;
                    if (viewRID == Include.DefaultGroupAllocationViewRID)    // disallow Delete of Default group allocation View
                    {
                        allowDelete = false;
                    }
                    else
                    {
                        allowDelete = _groupAllocationViewsGlobalGroupAllocationReviewSecurity.AllowDelete;
                    }
                }
                else if (aSelectedNode.Parent.Name == _groupAllocationReviewUserText)
                {
                    allowDelete = _groupAllocationViewsUserGroupAllocationReviewSecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _groupAllocationText)
                {
                    if (aSelectedNode.Nodes.Count > 0)
                    {
                        allowDelete = _groupAllocationViewsUserGroupAllocationReviewSecurity.AllowDelete;
                    }
                }
				// End TT#1077 - MD - stodd - cannot create GA views 
                // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
                else if (aSelectedNode.Parent.Name == _allocWorkspaceViewGlobalText)
                {
                    allowDelete = _allocViewsGlobalAllocWorkspaceViewSecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _allocWorkspaceViewUserText)
                {
                    allowDelete = _allocViewsUserAllocWorkspaceViewSecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _allocWorkspaceViewText)
                {
                    if (aSelectedNode.Nodes.Count > 0)
                    {
                        allowDelete = _allocViewsUserAllocWorkspaceViewSecurity.AllowDelete;
                    }
                }
                else if (aSelectedNode.Parent.Name == _asrtWorkspaceViewGlobalText)
                {
                    allowDelete = _asrtViewsGlobalAsrtWorkspaceViewSecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _asrtWorkspaceViewUserText)
                {
                    allowDelete = _asrtViewsUserAsrtWorkspaceViewSecurity.AllowDelete;
                }
                else if (aSelectedNode.Parent.Name == _asrtWorkspaceViewText)
                {
                    if (aSelectedNode.Nodes.Count > 0)
                    {
                        allowDelete = _asrtViewsUserAsrtWorkspaceViewSecurity.AllowDelete;
                    }
                }
                // End TT#1411-MD  
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return allowDelete;
        }
         
        private void DeleteView()
        {
            if (MessageBox.Show(String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_ConfirmDelete), trvViewMaintenance.SelectedNode.Text), this.Text,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (trvViewMaintenance.SelectedNode.Parent.Name == _forecastUserText ||
                    trvViewMaintenance.SelectedNode.Parent.Name == _forecastGlobalText)
                {
                    DeletePlanView();
                }
                else if (trvViewMaintenance.SelectedNode.Parent.Name == _velocityUserText ||
                         trvViewMaintenance.SelectedNode.Parent.Name == _velocityGlobalText || 
                         trvViewMaintenance.SelectedNode.Parent.Name == _velocityDetailUserText ||
                         trvViewMaintenance.SelectedNode.Parent.Name == _velocityDetailGlobalText ||
                         trvViewMaintenance.SelectedNode.Parent.Name == _styleReviewUserText ||         // Begin TT#454 - RMatelic - Add Views in Style Review
                         trvViewMaintenance.SelectedNode.Parent.Name == _styleReviewGlobalText ||       // End TT#454  
                         trvViewMaintenance.SelectedNode.Parent.Name == _sizeReviewUserText ||          // Begin TT#456 - RMatelic - Add Views to Size Review
                         trvViewMaintenance.SelectedNode.Parent.Name == _sizeReviewGlobalText ||        // End TT#456  
                         trvViewMaintenance.SelectedNode.Parent.Name == _allocWorkspaceViewGlobalText || // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
                         trvViewMaintenance.SelectedNode.Parent.Name == _allocWorkspaceViewUserText ||
                         trvViewMaintenance.SelectedNode.Parent.Name == _asrtWorkspaceViewGlobalText ||
                         trvViewMaintenance.SelectedNode.Parent.Name == _asrtWorkspaceViewUserText)      // End TT#1422-MD
                {
                    DeleteGridView();
                }
                // Begin TT#1181 - RMatelic - Assortment View Maintenance
                //BEGIN TT#42 - MD - DOConnell - Recieve unhandled exception when deleting views
                if (trvViewMaintenance.SelectedNode != null)
                {
					// Begin TT#1077 - MD - stodd - cannot create GA views 
                    if (trvViewMaintenance.SelectedNode.Parent.Name == _assortReviewUserText ||
                        trvViewMaintenance.SelectedNode.Parent.Name == _assortReviewGlobalText)
                    {
                        DeleteAssortmentView(eAssortmentViewType.Assortment);
                    }
                    // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window>> added 'else'
                    //if (trvViewMaintenance.SelectedNode.Parent.Name == _groupAllocationReviewUserText ||
                    //    trvViewMaintenance.SelectedNode.Parent.Name == _groupAllocationReviewGlobalText)
                    else if (trvViewMaintenance.SelectedNode.Parent.Name == _groupAllocationReviewUserText ||
                             trvViewMaintenance.SelectedNode.Parent.Name == _groupAllocationReviewGlobalText)
                    // End TT#1411-MD
                    {
                        DeleteAssortmentView(eAssortmentViewType.GroupAllocation);
                    }
					// End TT#1077 - MD - stodd - cannot create GA views 
                }
                //END TT#42 - MD - DOConnell - Recieve unhandled exception when deleting views
                // End TT#1181
            }
        }
        // End TT#231  
       
		
        //private void DeleteView()
        private void DeletePlanView()   // TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
		{

            //if(MessageBox.Show(String.Format(MIDText.GetTextOnly(eMIDTextCode.msg_ConfirmDelete), trvViewMaintenance.SelectedNode.Text), String.Empty, 
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //{
				PlanViewData data = new PlanViewData();

				try
				{
					data.OpenUpdateConnection();
					//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//					int view_RID = int.Parse(trvViewMaintenance.SelectedNode.Tag.ToString());
					int view_RID = ((ViewNodeTag)trvViewMaintenance.SelectedNode.Tag).ViewRID;
					//End Track #4815

					if(view_RID > 0)
					{
						if(data.PlanView_Delete(view_RID) > 0)
						{
							data.CommitData();
							_audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_UserViews, "OTSViewMaintenance.cs");
                            MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_DeleteComplete), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
							LoadTreeView();
						}
						else
						{
							ShowDeleteError();
						}
					}
					else
					{
						ShowDeleteError();
					}
				}
				catch (Exception ex)
				{
					HandleException(ex);
					ShowDeleteError(ex.Message);
				}
				finally
				{
					data.CloseUpdateConnection();
				}
            //}
		}

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
        private void DeleteAllUserViews()
        {
            if (MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_ConfirmDeleteAllUserViews), this.Text,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (trvViewMaintenance.SelectedNode.Name == _forecastUserText)
                {
                    DeleteAllUserPlanViews();
                }
                else if (trvViewMaintenance.SelectedNode.Name == _velocityUserText ||
                         trvViewMaintenance.SelectedNode.Name == _velocityDetailUserText ||
                         trvViewMaintenance.SelectedNode.Name == _styleReviewUserText ||          // TT#454 - RMatelic - Add Views in Style Review
                         trvViewMaintenance.SelectedNode.Name == _sizeReviewUserText ||           // TT#456 - RMatelic - Add Views to Size Review
                         trvViewMaintenance.SelectedNode.Name == _allocWorkspaceViewUserText ||   // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
                         trvViewMaintenance.SelectedNode.Name == _asrtWorkspaceViewUserText)      // End TT#1422-MD
                {   
                    DeleteAllUserGridViews();
                }
                // Begin TT#1181 - RMatelic - Assortment View Maintenance
				// Begin TT#1077 - MD - stodd - cannot create GA views 
                else if (trvViewMaintenance.SelectedNode.Name == _assortReviewUserText)
                {
                    DeleteAllUserAssortmentViews(eAssortmentViewType.Assortment);
                }
                // End TT#1181
                else if (trvViewMaintenance.SelectedNode.Name == _groupAllocationReviewUserText)
                {
                    DeleteAllUserAssortmentViews(eAssortmentViewType.GroupAllocation);
                }
				// End TT#1077 - MD - stodd - cannot create GA views 
            }
        }
        // End TT#231  

        //private void DeleteAllUserViews()
        private void DeleteAllUserPlanViews()   // TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
		{
            //if(MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_ConfirmDeleteAllUserViews), String.Empty, 
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //{
				PlanViewData data = new PlanViewData();
				
				try
				{
					data.OpenUpdateConnection();
					foreach(TreeNode thisNode in trvViewMaintenance.SelectedNode.Nodes)
					{
						//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//						int view_RID = int.Parse(thisNode.Tag.ToString());
						int view_RID = ((ViewNodeTag)thisNode.Tag).ViewRID;
						//End Track #4815
						
						if(view_RID > 0)
						{
							if(data.PlanView_Delete(view_RID) > 0)
							{
								data.CommitData();
							}
							else
							{
								ShowDeleteError();
								return;
							}
						}
						else
						{
							ShowDeleteError();
							return;
						}
                    }

                    MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_DeleteComplete), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
					LoadTreeView();
                }
				catch (Exception ex)
				{
					HandleException(ex);
					ShowDeleteError(ex.Message);
				}
				finally
				{
					data.CloseUpdateConnection();
				}
            //}
		}
      
        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
        private void DeleteAllUserGridViews()    
        { 
            //GridViewData gridViewData = new GridViewData();

            try
            {
                _gridViewData.OpenUpdateConnection();
                foreach (TreeNode thisNode in trvViewMaintenance.SelectedNode.Nodes)
                {
                    int view_RID = ((ViewNodeTag)thisNode.Tag).ViewRID;
                    _gridViewData.GridView_Delete(view_RID); 
                }
                _gridViewData.CommitData();
                MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_DeleteComplete), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTreeView();
            }
            catch (Exception ex)
            {
                _gridViewData.Rollback();
                HandleException(ex);
                ShowDeleteError(ex.Message);
            }
            finally
            {
                _gridViewData.CloseUpdateConnection();
            }
        }

        private void DeleteGridView()
        {
            //GridViewData gridViewData = new GridViewData();

            try
            {
                _gridViewData.OpenUpdateConnection();
                int view_RID = ((ViewNodeTag)trvViewMaintenance.SelectedNode.Tag).ViewRID;
                _gridViewData.GridView_Delete(view_RID);
                _gridViewData.CommitData();
                MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_DeleteComplete), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTreeView();
            }
            catch (Exception ex)
            {
                _gridViewData.Rollback();
                HandleException(ex);
                ShowDeleteError(ex.Message);
            }
            finally
            {
                _gridViewData.CloseUpdateConnection();
            }
        }
        // End TT#231

        // Begin TT#1181 - RMatelic - Assortment View Maintenance
        private void DeleteAssortmentView(eAssortmentViewType viewType)    	// TT#1077 - MD - stodd - cannot create GA views 
        {
            try
            {
                _assortViewData.OpenUpdateConnection();
                int view_RID = ((ViewNodeTag)trvViewMaintenance.SelectedNode.Tag).ViewRID;
                _assortViewData.AssortmentView_Delete(view_RID, viewType);	// TT#1077 - MD - stodd - cannot create GA views 
                _assortViewData.CommitData();
                MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_DeleteComplete), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTreeView();
            }
            catch (Exception ex)
            {
                _assortViewData.Rollback();
                HandleException(ex);
                ShowDeleteError(ex.Message);
            }
            finally
            {
                _assortViewData.CloseUpdateConnection();
            }
        }

        private void DeleteAllUserAssortmentViews(eAssortmentViewType viewType)		// TT#1077 - MD - stodd - cannot create GA views 
        {
            try
            {
                _assortViewData.OpenUpdateConnection();
                foreach (TreeNode thisNode in trvViewMaintenance.SelectedNode.Nodes)
                {
                    int view_RID = ((ViewNodeTag)thisNode.Tag).ViewRID;
                    _assortViewData.AssortmentView_Delete(view_RID, viewType);		// TT#1077 - MD - stodd - cannot create GA views 
                }
                _assortViewData.CommitData();
                MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_DeleteComplete), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTreeView();
            }
            catch (Exception ex)
            {
                _assortViewData.Rollback();
                HandleException(ex);
                ShowDeleteError(ex.Message);
            }
            finally
            {
                _assortViewData.CloseUpdateConnection();
            }
        }
        // End TT#1181  

		private void LoadTreeView()
		{
            try
            {
                // Clear the view
                if (trvViewMaintenance.Nodes.Count > 0)
                    trvViewMaintenance.Nodes.Clear();

                PlanViewData data = new PlanViewData();
                ArrayList viewUsers = new ArrayList();

                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
                //GridViewData gridViewData = new GridViewData();

                TreeNode forecastNode = new TreeNode();
                forecastNode.Text = _forecastText;
                forecastNode.Name = _forecastText;
                forecastNode.Tag = new ViewNodeTag(-1, Include.GlobalUserRID, Include.GlobalUserRID);

                forecastNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DefaultClosedFolder);
                forecastNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DefaultOpenFolder);
                trvViewMaintenance.Nodes.Add(forecastNode);
                // End TT#231  


                if (_forecastViewsGlobalSecurity.AllowView)
                {
                    // Get Global Views
                    viewUsers.Add(Include.GlobalUserRID);
                    DataTable views = data.PlanView_Read(viewUsers);

                    TreeNode headerNode = new TreeNode();
                    headerNode.Text = MIDText.GetTextOnly(eMIDTextCode.msg_GlobalViews);
                    //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
                    //				headerNode.Tag = -1;
                    headerNode.Tag = new ViewNodeTag(-1, Include.GlobalUserRID, Include.GlobalUserRID);
                    //End Track #4815
                    headerNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                    headerNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                    // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
                    //trvViewMaintenance.Nodes.Add(headerNode);  
                    headerNode.Name = _forecastGlobalText;
                    trvViewMaintenance.Nodes[0].Nodes.Add(headerNode);
                    // End TT#231

                    if (views.Rows.Count > 0)
                    {
                        foreach (DataRow row in views.Rows)
                        {
                            TreeNode thisNode = new TreeNode(row["VIEW_ID"].ToString());
                            //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
                            //						thisNode.Tag = row["VIEW_RID"];
                            thisNode.Tag = new ViewNodeTag(Convert.ToInt32(row["VIEW_RID"]), Convert.ToInt32(row["USER_RID"]), Convert.ToInt32(row["OWNER_USER_RID"]));
                            //End Track #4815
                            thisNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                            thisNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                            // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
                            //trvViewMaintenance.Nodes[0].Nodes.Add(thisNode);
                            headerNode.Nodes.Add(thisNode);
                            // End TT#231
                        }
                    }

                    // Get User Views
                    viewUsers.Clear();
                }

                if (_forecastViewsUserSecurity.AllowView)
                {
                    viewUsers.Add(_sab.ClientServerSession.UserRID);
                    DataTable views = data.PlanView_Read(viewUsers);

                    TreeNode headerNode = new TreeNode();
                    headerNode.Text = MIDText.GetTextOnly(eMIDTextCode.msg_UserViews);
                    //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
                    //				headerNode.Tag = -1;
                    headerNode.Tag = new ViewNodeTag(-1, _sab.ClientServerSession.UserRID, _sab.ClientServerSession.UserRID);
                    //End Track #4815
                    headerNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                    headerNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                    // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
                    //trvViewMaintenance.Nodes.Add(headerNode);
                    headerNode.Name = _forecastUserText;
                    trvViewMaintenance.Nodes[0].Nodes.Add(headerNode);


                    if (views.Rows.Count > 0)
                    {
                        foreach (DataRow row in views.Rows)
                        {
                            TreeNode thisNode = new TreeNode(row["VIEW_ID"].ToString());
                            //Begin Track #4815 - JSmith - #283-User (Security) Maintenance
                            //						thisNode.Tag = row["VIEW_RID"];
                            //						thisNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                            //						thisNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                            thisNode.Tag = new ViewNodeTag(Convert.ToInt32(row["VIEW_RID"]), Convert.ToInt32(row["USER_RID"]), Convert.ToInt32(row["OWNER_USER_RID"]));
                            if (Convert.ToInt32(row["USER_RID"]) != Convert.ToInt32(row["OWNER_USER_RID"]))
                            {
                                thisNode.ImageIndex = MIDGraphics.ImageSharedIndex(MIDGraphics.SecUserImage);
                                thisNode.SelectedImageIndex = thisNode.ImageIndex;
                            }
                            else
                            {
                                thisNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                                thisNode.SelectedImageIndex = thisNode.ImageIndex;
                            }
                            //End Track #4815
                            // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
                            //trvViewMaintenance.Nodes[1].Nodes.Add(thisNode);
                            headerNode.Nodes.Add(thisNode);
                            // End TT#231
                        }
                    }
                }

                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail 
                ArrayList userRIDList = new ArrayList();
                TreeNode allocationNode = new TreeNode();
                allocationNode.Text = _allocationText;
                allocationNode.Name = _allocationText;
                allocationNode.Tag = new ViewNodeTag(-1, Include.GlobalUserRID, Include.GlobalUserRID);

                trvViewMaintenance.Nodes.Add(allocationNode);
                if (_allocViewsGlobalFunctionSecurity.AllowView || _allocViewsUserFunctionSecurity.AllowView)
                {
                    // Begin TT#454 - RMatelic - Add Views in Style Review - rewrote this part with new methods
                    TreeNode velocityNode = CreateFolderNode(_velocityMethodText);
                    allocationNode.Nodes.Add(velocityNode);

                    if (_allocViewsGlobalVelocitySecurity.AllowView)
                    {
                        AddGridViewGlobalNodes(velocityNode, _velocityGlobalText, eLayoutID.velocityMatrixGrid);
                    }
                    if (_allocViewsUserVelocitySecurity.AllowView)
                    {
                        AddGridViewUserNodes(velocityNode, _velocityUserText, eLayoutID.velocityMatrixGrid);
                    }
                    velocityNode.Expand();

                    TreeNode velocityDetailNode = CreateFolderNode(_velocityStoreDetailText);
                    allocationNode.Nodes.Add(velocityDetailNode);

                    if (_allocViewsGlobalVelocityDetailSecurity.AllowView)
                    {
                        AddGridViewGlobalNodes(velocityDetailNode, _velocityDetailGlobalText, eLayoutID.velocityStoreDetailGrid);
                    }
                    if (_allocViewsUserVelocityDetailSecurity.AllowView)
                    {
                        AddGridViewUserNodes(velocityDetailNode, _velocityDetailUserText, eLayoutID.velocityStoreDetailGrid);
                    }
                    velocityDetailNode.Expand();

                    TreeNode styleReviewNode = CreateFolderNode(_styleReviewText);
                    allocationNode.Nodes.Add(styleReviewNode);

                    if (_allocViewsGlobalStyleReviewSecurity.AllowView)
                    {
                        AddGridViewGlobalNodes(styleReviewNode, _styleReviewGlobalText, eLayoutID.styleReviewGrid);
                    }
                    if (_allocViewsUserStyleReviewSecurity.AllowView)
                    {
                        AddGridViewUserNodes(styleReviewNode, _styleReviewUserText, eLayoutID.styleReviewGrid);
                    }
                    styleReviewNode.Expand();
                    // End TT#454  
                    // Begin TT#456 - RMAtelic - Add Views to Size Reviww
                    TreeNode sizeReviewNode = CreateFolderNode(_sizeReviewText);
                    allocationNode.Nodes.Add(sizeReviewNode);

                    if (_allocViewsGlobalSizeReviewSecurity.AllowView)
                    {
                        AddGridViewGlobalNodes(sizeReviewNode, _sizeReviewGlobalText, eLayoutID.sizeReviewGrid);
                    }
                    if (_allocViewsUserSizeReviewSecurity.AllowView)
                    {
                        AddGridViewUserNodes(sizeReviewNode, _sizeReviewUserText, eLayoutID.sizeReviewGrid);
                    }
                    sizeReviewNode.Expand();
                    // End TT#456  
                    // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
                    TreeNode allocWorkspaceViewNode = CreateFolderNode(_allocWorkspaceViewText);
                    allocationNode.Nodes.Add(allocWorkspaceViewNode);

                    if (_allocViewsGlobalAllocWorkspaceViewSecurity.AllowView)
                    {
                        AddGridViewGlobalNodes(allocWorkspaceViewNode, _allocWorkspaceViewGlobalText, eLayoutID.allocationWorkspaceGrid);
                    }
                    if (_allocViewsUserAllocWorkspaceViewSecurity.AllowView)
                    {
                        AddGridViewUserNodes(allocWorkspaceViewNode, _allocWorkspaceViewUserText, eLayoutID.allocationWorkspaceGrid);
                    }
                    allocWorkspaceViewNode.Expand();
                    // End TT#1411-MD
                }
                // Begin TT#1181 - RMatelic - Assortment View Maintenance
                TreeNode assortmentNode = new TreeNode();
				// Begin TT#1077 - MD - stodd - cannot create GA views 
                if (_sab.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
                {
                    assortmentNode.Text = _assortmentText;
                    assortmentNode.Name = _assortmentText;
                    assortmentNode.Tag = new ViewNodeTag(-1, Include.GlobalUserRID, Include.GlobalUserRID);
                    assortmentNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DefaultClosedFolder);
                    assortmentNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DefaultOpenFolder);
                    trvViewMaintenance.Nodes.Add(assortmentNode);

                    // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
                    // Add outer if... condition and AssortmentReview node
                    if (_assortViewsGlobalAssortReviewSecurity.AllowView || _assortViewsUserAssortReviewSecurity.AllowView)
                    {
                        TreeNode assortmentReviewNode = CreateFolderNode(_assortmentReviewText);
                        assortmentNode.Nodes.Add(assortmentReviewNode); 

                        if (_assortViewsGlobalAssortReviewSecurity.AllowView)
                        {
                            userRIDList.Clear();
                            userRIDList.Add(Include.GlobalUserRID);
                            TreeNode globalNode = new TreeNode();
                            globalNode.Text = MIDText.GetTextOnly(eMIDTextCode.msg_GlobalViews);
                            globalNode.Tag = new ViewNodeTag(-1, Include.GlobalUserRID, Include.GlobalUserRID);
                            globalNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                            globalNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                            globalNode.Name = _assortReviewGlobalText;
                            // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
                            //assortmentNode.Nodes.Add(globalNode);
                            assortmentReviewNode.Nodes.Add(globalNode);
                            // End TT#1411-MD
                            DataTable views = _assortViewData.AssortmentView_Read(userRIDList, eAssortmentViewType.Assortment);
                            if (views.Rows.Count > 0)
                            {
                                foreach (DataRow row in views.Rows)
                                {
                                    TreeNode viewNode = new TreeNode(row["VIEW_ID"].ToString());
                                    viewNode.Tag = new ViewNodeTag(Convert.ToInt32(row["VIEW_RID"]), Convert.ToInt32(row["USER_RID"]), Convert.ToInt32(row["USER_RID"]));
                                    viewNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                                    viewNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                                    globalNode.Nodes.Add(viewNode);
                                }
                            }
                        }

                        if (_assortViewsUserAssortReviewSecurity.AllowView)
                        {
                            userRIDList.Clear();
                            userRIDList.Add(_sab.ClientServerSession.UserRID);
                            TreeNode userNode = new TreeNode();
                            userNode.Text = MIDText.GetTextOnly(eMIDTextCode.msg_UserViews);
                            userNode.Tag = new ViewNodeTag(-1, _sab.ClientServerSession.UserRID, _sab.ClientServerSession.UserRID);
                            userNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                            userNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                            userNode.Name = _assortReviewUserText;
                            // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
                            //assortmentNode.Nodes.Add(userNode);
                            assortmentReviewNode.Nodes.Add(userNode);
                            // End TT#1411-MD
                            DataTable views = _assortViewData.AssortmentView_Read(userRIDList, eAssortmentViewType.Assortment);
                            if (views.Rows.Count > 0)
                            {
                                foreach (DataRow row in views.Rows)
                                {
                                    TreeNode viewNode = new TreeNode(row["VIEW_ID"].ToString());
                                    viewNode.Tag = new ViewNodeTag(Convert.ToInt32(row["VIEW_RID"]), Convert.ToInt32(row["USER_RID"]), Convert.ToInt32(row["USER_RID"]));
                                    viewNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                                    viewNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                                    userNode.Nodes.Add(viewNode);
                                }
                            }
                        }
                        assortmentReviewNode.Expand(); // TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
                        // End TT#1181
                        // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
                        if (_asrtViewsGlobalAsrtWorkspaceViewSecurity.AllowView || _asrtViewsUserAsrtWorkspaceViewSecurity.AllowView)
                        {
                            TreeNode asrtWorkspaceViewNode = CreateFolderNode(_asrtWorkspaceViewText);
                            assortmentNode.Nodes.Add(asrtWorkspaceViewNode);

                            if (_asrtViewsGlobalAsrtWorkspaceViewSecurity.AllowView)
                            {
                                AddGridViewGlobalNodes(asrtWorkspaceViewNode, _asrtWorkspaceViewGlobalText, eLayoutID.assortmentWorkspaceGrid);
                            }
                            if (_asrtViewsUserAsrtWorkspaceViewSecurity.AllowView)
                            {
                                AddGridViewUserNodes(asrtWorkspaceViewNode, _asrtWorkspaceViewUserText, eLayoutID.assortmentWorkspaceGrid);
                            }
                            asrtWorkspaceViewNode.Expand();
                        }
                        // End TT#1411-MD
                    }
                    // End TT#1411-MD
                }

                TreeNode groupAllocationNode = new TreeNode();
                if (_sab.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationInstalled)    // TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                {
                    groupAllocationNode.Text = _groupAllocationText;
                    groupAllocationNode.Name = _groupAllocationText;
                    groupAllocationNode.Tag = new ViewNodeTag(-1, Include.GlobalUserRID, Include.GlobalUserRID);
                    groupAllocationNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DefaultClosedFolder);
                    groupAllocationNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DefaultOpenFolder);
                    trvViewMaintenance.Nodes.Add(groupAllocationNode);

                    if (_groupAllocationViewsGlobalGroupAllocationReviewSecurity.AllowView)
                    {
                        userRIDList.Clear();
                        userRIDList.Add(Include.GlobalUserRID);
                        TreeNode globalNode = new TreeNode();
                        globalNode.Text = MIDText.GetTextOnly(eMIDTextCode.msg_GlobalViews);
                        globalNode.Tag = new ViewNodeTag(-1, Include.GlobalUserRID, Include.GlobalUserRID);
                        globalNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                        globalNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                        globalNode.Name = _groupAllocationReviewGlobalText;
                        groupAllocationNode.Nodes.Add(globalNode);

                        // Group Allocation uses the _assortViewData object
                        DataTable views = _assortViewData.AssortmentView_Read(userRIDList, eAssortmentViewType.GroupAllocation);
                        if (views.Rows.Count > 0)
                        {
                            foreach (DataRow row in views.Rows)
                            {
                                TreeNode viewNode = new TreeNode(row["VIEW_ID"].ToString());
                                viewNode.Tag = new ViewNodeTag(Convert.ToInt32(row["VIEW_RID"]), Convert.ToInt32(row["USER_RID"]), Convert.ToInt32(row["USER_RID"]));
                                viewNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                                viewNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                                globalNode.Nodes.Add(viewNode);
                            }
                        }
                    }

                    if (_groupAllocationViewsUserGroupAllocationReviewSecurity.AllowView)
                    {
                        userRIDList.Clear();
                        userRIDList.Add(_sab.ClientServerSession.UserRID);
                        TreeNode userNode = new TreeNode();
                        userNode.Text = MIDText.GetTextOnly(eMIDTextCode.msg_UserViews);
                        userNode.Tag = new ViewNodeTag(-1, _sab.ClientServerSession.UserRID, _sab.ClientServerSession.UserRID);
                        userNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                        userNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                        userNode.Name = _groupAllocationReviewUserText;
                        groupAllocationNode.Nodes.Add(userNode);

                        // Group Allocation uses the _assortViewData object
                        DataTable views = _assortViewData.AssortmentView_Read(userRIDList, eAssortmentViewType.GroupAllocation);
                        if (views.Rows.Count > 0)
                        {
                            foreach (DataRow row in views.Rows)
                            {
                                TreeNode viewNode = new TreeNode(row["VIEW_ID"].ToString());
                                viewNode.Tag = new ViewNodeTag(Convert.ToInt32(row["VIEW_RID"]), Convert.ToInt32(row["USER_RID"]), Convert.ToInt32(row["USER_RID"]));
                                viewNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                                viewNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                                userNode.Nodes.Add(viewNode);
                            }
                        }
                    }
                }


                forecastNode.Expand();
                allocationNode.Expand();
                // End TT#231  
                if (_sab.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
                {
                    assortmentNode.Expand();    // TT#1181 - RMatelic - Assortment View Maintenance
                }
                // Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                if (_sab.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationInstalled)
                {
                    groupAllocationNode.Expand();
                }
				// End TT#1077 - MD - stodd - cannot create GA views 
                // End TT#1247-MD - stodd - Add Group Allocation as a License Key option -

            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
		}

        // Begin TT#454 - RMatelic - Add Views in Style Review
        private TreeNode CreateFolderNode(string aNodeName)
        {
            TreeNode node = new TreeNode();
            node.Text = aNodeName;
            node.Name = aNodeName;
            node.Tag = new ViewNodeTag(-1, Include.GlobalUserRID, Include.GlobalUserRID);
            node.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.ClosedTreeFolder);
            node.SelectedImageIndex = node.ImageIndex;
            return node;
        }

        private void AddGridViewGlobalNodes(TreeNode addToTreeNode, string aNodeName, eLayoutID aLayoutID)
        {
            try
            {
                ArrayList userRIDList = new ArrayList();
                TreeNode globalNode = new TreeNode();
                globalNode.Text = MIDText.GetTextOnly(eMIDTextCode.msg_GlobalViews);
                globalNode.Tag = new ViewNodeTag(-1, Include.GlobalUserRID, Include.GlobalUserRID);
                globalNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                globalNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                globalNode.Name = aNodeName;
                addToTreeNode.Nodes.Add(globalNode);
                // Get Global Views
                userRIDList.Add(Include.GlobalUserRID);
                DataTable views = _gridViewData.GridView_Read((int)aLayoutID, userRIDList);
                if (views.Rows.Count > 0)
                {
                    foreach (DataRow row in views.Rows)
                    {
                        TreeNode thisNode = new TreeNode(row["VIEW_ID"].ToString());
                        thisNode.Tag = new ViewNodeTag(Convert.ToInt32(row["VIEW_RID"]), Convert.ToInt32(row["USER_RID"]), Convert.ToInt32(row["USER_RID"]));
                        thisNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                        thisNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
                        globalNode.Nodes.Add(thisNode);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void AddGridViewUserNodes(TreeNode addToTreeNode, string aNodeName, eLayoutID aLayoutID)
        {
            try
            {
                ArrayList userRIDList = new ArrayList();
                TreeNode userNode = new TreeNode();
                userNode.Text = MIDText.GetTextOnly(eMIDTextCode.msg_UserViews);
                userNode.Tag = new ViewNodeTag(-1, _sab.ClientServerSession.UserRID, _sab.ClientServerSession.UserRID);
                userNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                userNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                userNode.Name = aNodeName;
                addToTreeNode.Nodes.Add(userNode);
                // Get User Views
                userRIDList.Add(_sab.ClientServerSession.UserRID);
                DataTable views = _gridViewData.GridView_Read((int)aLayoutID, userRIDList);
                if (views.Rows.Count > 0)
                {
                    foreach (DataRow row in views.Rows)
                    {
                        TreeNode thisNode = new TreeNode(row["VIEW_ID"].ToString());
                        thisNode.Tag = new ViewNodeTag(Convert.ToInt32(row["VIEW_RID"]), Convert.ToInt32(row["USER_RID"]), Convert.ToInt32(row["USER_RID"]));
                        thisNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                        thisNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
                        userNode.Nodes.Add(thisNode);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#454  

		#endregion
	}

	//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
	/// <summary>
	/// Class that defines the tag class for a View TreeNode
	/// </summary>

	public class ViewNodeTag
	{
		private int _viewRID;
		private int _userRID;
		private int _ownerUserRID;

		public ViewNodeTag(int aViewRID, int aUserRID, int aOwnerUserRID)
		{
			_viewRID = aViewRID;
			_userRID = aUserRID;
			_ownerUserRID = aOwnerUserRID;
		}

		public int ViewRID
		{
			get
			{
				return _viewRID;
			}
		}

		public int UserRID
		{
			get
			{
				return _userRID;
			}
		}

		public int OwnerUserRID
		{
			get
			{
				return _ownerUserRID;
			}
		}
	}
	//End Track #4815
}
