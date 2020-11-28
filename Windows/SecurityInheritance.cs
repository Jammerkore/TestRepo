using System;
using System.Drawing;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for SecurityInheritance.
	/// </summary>
	public class frmSecurityInheritance : MIDFormBase
	{
		private SessionAddressBlock _SAB;
		private System.Windows.Forms.CheckBox _currCheckBox;
		private System.Windows.Forms.TreeNode _currNode;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.CheckedListBox clbPermissions;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSecurityInheritance(SessionAddressBlock aSAB, CheckBox aCheckBox, TreeNode aCurrNode) : base(aSAB)
		{
			_SAB = aSAB;
			_currCheckBox = aCheckBox;
			_currNode = aCurrNode;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

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
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnClose = new System.Windows.Forms.Button();
			this.clbPermissions = new System.Windows.Forms.CheckedListBox();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Location = new System.Drawing.Point(252, 232);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// clbPermissions
			// 
			this.clbPermissions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.clbPermissions.Location = new System.Drawing.Point(24, 32);
			this.clbPermissions.Name = "clbPermissions";
			this.clbPermissions.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.clbPermissions.Size = new System.Drawing.Size(292, 184);
			this.clbPermissions.TabIndex = 2;
			// 
			// frmSecurityInheritance
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(344, 266);
			this.Controls.Add(this.clbPermissions);
			this.Controls.Add(this.btnClose);
			this.Name = "frmSecurityInheritance";
			this.Text = "SecurityInheritance";
			this.Load += new System.EventHandler(this.frmSecurityInheritance_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmSecurityInheritance_Load(object sender, System.EventArgs e)
		{
			MIDRetail.Business.Security security = new Security();
			SecurityAdmin securityData = new SecurityAdmin();
			SecurityProfile securityProfile = null;
			NodeTag nodeTag = (NodeTag)_currNode.Tag;
			PermissionTag permissionTag = (PermissionTag)_currCheckBox.Tag;
			switch (nodeTag.Type)
			{
				case NodeType.NT_MLID:
					if (nodeTag.OwnerType == eSecurityOwnerType.Group)
					{
						switch (permissionTag.SecurityType)
						{
							case eDatabaseSecurityTypes.Allocation:
								securityProfile = security.GetGroupNodeAssignment(_SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Allocation, true);
								break;
							case eDatabaseSecurityTypes.Chain:
								securityProfile = security.GetGroupNodeAssignment(_SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Chain, true);
								break;
							case eDatabaseSecurityTypes.Store:
								securityProfile = security.GetGroupNodeAssignment(_SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Store, true);
								break;
						}
					}
					else
					{
						switch (permissionTag.SecurityType)
						{
							case eDatabaseSecurityTypes.Allocation:
								securityProfile = security.GetUserNodeAssignment(_SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Allocation, true);
								break;
							case eDatabaseSecurityTypes.Chain:
								securityProfile = security.GetUserNodeAssignment(_SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Chain, true);
								break;
							case eDatabaseSecurityTypes.Store:
								securityProfile = security.GetUserNodeAssignment(_SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Store, true);
								break;
						}
					}
					break;
				case NodeType.NT_Version:
					if (nodeTag.OwnerType == eSecurityOwnerType.Group)
					{
						switch (permissionTag.SecurityType)
						{
							case eDatabaseSecurityTypes.Allocation:
								securityProfile = security.GetGroupVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Allocation, true);
								break;
							case eDatabaseSecurityTypes.Chain:
								securityProfile = security.GetGroupVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Chain, true);
								break;
							case eDatabaseSecurityTypes.Store:
								securityProfile = security.GetGroupVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Store, true);
								break;
						}
					}
					else
					{
						switch (permissionTag.SecurityType)
						{
							case eDatabaseSecurityTypes.Allocation:
								securityProfile = security.GetUserVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Allocation, true);
								break;
							case eDatabaseSecurityTypes.Chain:
								securityProfile = security.GetUserVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Chain, true);
								break;
							case eDatabaseSecurityTypes.Store:
								securityProfile = security.GetUserVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Store, true);
								break;
						}
					}
					
					break;
				case NodeType.NT_Function:
					if (nodeTag.OwnerType == eSecurityOwnerType.Group)
					{
						securityProfile = security.GetGroupFunctionAssignment(nodeTag.OwnerRID, (eSecurityFunctions)nodeTag.Item, true);
					}
					else
					{
						securityProfile = security.GetUserFunctionAssignment(nodeTag.OwnerRID, (eSecurityFunctions)nodeTag.Item, true);
					}
					break;
			}

			SecurityPermission permission = securityProfile.GetSecurityPermission(permissionTag.Action);
			foreach (InheritancePathItem ipi in permission.InheritancePath) 
			{
//				DataRow [] rows;
				string message = MIDText.GetTextOnly(Convert.ToInt32(ipi.OwnerType, CultureInfo.CurrentCulture)) + " "; 
				switch (ipi.OwnerType)
				{
					case eSecurityOwnerType.Group:
						DataTable dtGroup = securityData.GetGroup(ipi.OwnerKey);
						if (dtGroup.Rows.Count > 0)
						{
							DataRow dr = dtGroup.Rows[0];
							message += Convert.ToString(dr["GROUP_NAME"], CultureInfo.CurrentUICulture);
						}
						else
						{
							message += "Unknown";
						}
						break;
					case eSecurityOwnerType.User:
						DataTable dtUser = securityData.GetUser(ipi.OwnerKey);
						if (dtUser.Rows.Count > 0)
						{
							DataRow dr = dtUser.Rows[0];
							message += Convert.ToString(dr["USER_NAME"], CultureInfo.CurrentUICulture);
						}
						else
						{
							message += "Unknown";
						}
						break;
					default:
						message += "Unknown";
						break;
				}

				switch (ipi.InheritanceType)
				{
					case eSecurityInheritanceTypes.Function:
						DataTable dtFunction = securityData.GetSecurityFunction((eSecurityFunctions)ipi.InheritanceItem);
						if (dtFunction.Rows.Count > 0)
						{
							DataRow dr = dtFunction.Rows[0];
							message += ":" + Convert.ToString(dr["TEXT_VALUE"], CultureInfo.CurrentUICulture);
						}
						else
						{
							message += ":Unknown";
						}
						break;
					case eSecurityInheritanceTypes.HierarchyNode:
						HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(ipi.InheritanceItem);
						if (hnp != null)
						{
							message += ":" + hnp.Text;
						}
						else
						{
							message += ":Unknown";
						}
						break;
					default:
						message += ":Unknown";
						break;
				}
				message += ":" + MIDText.GetTextOnly(Convert.ToInt32(ipi.SecurityLevel, CultureInfo.CurrentCulture)); 
				
				clbPermissions.Items.Add(message);
			}

			clbPermissions.SetItemChecked(permission.SecurityInheritanceIndex, true);

			Format_Title(eDataState.Updatable, eMIDTextCode.frm_SecurityInheritance,null);
			SetReadOnly(true);
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
