using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.Data;
using System.Configuration;
using System.Runtime.InteropServices; // TT#3034 - RMatelic -Display of arrows vs '+' with Window 7 

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;


namespace MIDRetail.Windows
{
	/// <summary>
	/// Form for security administration
	/// </summary>

	public class frmSecurity : MIDFormBase
	{
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.ComponentModel.IContainer components;
		FunctionSecurityProfile _groupSecurity;
		FunctionSecurityProfile _userSecurity;
		private Rectangle _dragBoxFromMouseDown;
		//		private int _multiSelectStartIdx;
		//		private int _multiSelectEndIdx;
		private int _indexOfItemUnderMouseToDrag;
        //private bool _mouseDown = false;
		private System.Windows.Forms.DragDropEffects _currentEffect = DragDropEffects.Move;
		DataTable _dtGroups;
		DataTable _dtActiveGroups;
		DataTable _dtUsers;
		DataTable _dtActiveUsers;
		DataTable _dtVersions;
		DataTable _dtFunctions;
		DataTable _dtSecurityFunctions;
		SecurityAdmin _securityData;
		MIDRetail.Business.Security _security;
		private int    _mouseDownX, _mouseDownY;
		TreeNode _currGroupFolder;
		TreeNode _currUserFolder;
		//		TreeNode _currVersionFolder;
		TreeNode _currFunctionFolder;
		TreeNode _currGroup;
		TreeNode _currUser;
		TreeNode _currNode;
		System.Windows.Forms.CheckBox _currCheckBox;
		bool _rebuildUserPanel;
		//		bool _rebuildInheritedPanel;
		bool _rebuildGroupPanel;
		//		bool _radioNoChange;
		bool _permissionChanged;
		TreeNode _createUserLike;
		TreeNode _createGroupLike;
////Begin Track #3876 - JSmith - show deny instead of blank
//		private eSecurityLevel _fullControlSecurity = eSecurityLevel.Initialize;
////End Track #3876
//Begin Track #3875 - JSmith - remove permission error
		private bool _removingPermission = false;
//End Track #3875
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		private bool _assignedToChanged;
		private bool _changeUserPanelLoaded;
		private bool _rebuildUserHierarchies = false;
		//End Track #4815
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
		private Hashtable _permissionSetHash;
		private bool _fullControlChanged;
		private bool _actionChanged;
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes

		//		private string _sourceModule = "Security.cs";
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel pnlSearchGroup;
		private System.Windows.Forms.RadioButton rdoGroupSearchInactive;
		private System.Windows.Forms.RadioButton rdoGroupSearchActive;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Panel pnlSearchUser;
		private System.Windows.Forms.RadioButton rdoSearchUserInactive;
		private System.Windows.Forms.RadioButton rdoSearchUserActive;
		private System.Windows.Forms.Panel pnlChangeGroup;
		private System.Windows.Forms.RadioButton rdoChangeGroupInactive;
		private System.Windows.Forms.RadioButton rdoChangeGroupActive;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.TextBox txtChangeGroupName;
		private System.Windows.Forms.Panel pnlChangeUser;
		private System.Windows.Forms.TextBox txtChangeUserPassword;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.RadioButton rdoChangeUserInactive;
		private System.Windows.Forms.RadioButton rdoChangeUserActive;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.TextBox txtChangeUserID;
		private System.Windows.Forms.Panel pnlNewUser;
		private System.Windows.Forms.RadioButton rdoNewUserInactive;
		private System.Windows.Forms.RadioButton rdoNewUserActive;
		private System.Windows.Forms.Panel pnlAddGroup;
		private System.Windows.Forms.ListBox lstAvailableGroups;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Panel pnlPermissions;
		private System.Windows.Forms.Panel pnlAddStoreGroup;
		private System.Windows.Forms.TreeView trvAvailableStoreGroups;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Panel pnlAddStore;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ListBox lstAvailableStores;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Panel pnlAddMLID;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TreeView trvAvailableMLIDs;
		private System.Windows.Forms.Panel pnlAddUser;
		private System.Windows.Forms.ListBox lstAvailableUsers;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel pnlNewGroup;
		private System.Windows.Forms.RadioButton rdoNewGroupInactive;
		private System.Windows.Forms.RadioButton rdoNewGroupActive;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabGroups;
		private System.Windows.Forms.TreeView trvGroups;
		private System.Windows.Forms.TabPage tabUsers;
		private System.Windows.Forms.TreeView trvUsers;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnGroupSearch;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtChangeGroupDesc;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Button btnUserSearch;
		private System.Windows.Forms.TextBox txtGroupSearchName;
		private System.Windows.Forms.Button btnChangeGroup;
		private System.Windows.Forms.Button btnChangeUser;
		private System.Windows.Forms.Button btnNewUser;
		private System.Windows.Forms.Button btnNewGroup;
		private System.Windows.Forms.TextBox txtNewGroupName;
		private System.Windows.Forms.TextBox txtGroupSearchDesc;
		private System.Windows.Forms.TextBox txtNewGroupDescription;
		private System.Windows.Forms.TextBox txtChangeUserPassConfirm;
		private System.Windows.Forms.TextBox txtNewUserPassConfirm;
		private System.Windows.Forms.TextBox txtNewUserPassword;
		private System.Windows.Forms.TextBox txtNewUserDesc;
		private System.Windows.Forms.TextBox txtNewUserFullName;
		private System.Windows.Forms.TextBox txtNewUserID;
		private System.Windows.Forms.TextBox txtSearchUserDesc;
		private System.Windows.Forms.TextBox txtSearchUserFullName;
		private System.Windows.Forms.TextBox txtSearchUserName;
		private System.Windows.Forms.Label lblCreateGroupLike;
		private System.Windows.Forms.Label lblCreateUserLike;
		private System.Windows.Forms.TextBox txtChangeUserDesc;
		private System.Windows.Forms.TextBox txtChangeUserFullName;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel pnlFunctionPermission;
		private System.Windows.Forms.Label lblPermission;
		private System.Windows.Forms.Label lblAllow;
		private System.Windows.Forms.Label lblDeny;
		private System.Windows.Forms.Button btnPermissions;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ListBox lstAvailableViews;
		private System.Windows.Forms.Panel pnlAddView;
		private System.Windows.Forms.MenuItem miHowDetermined;
		private System.Windows.Forms.ContextMenu cmPermissions;
		private System.Windows.Forms.GroupBox gbxNewGroupStatus;
		private System.Windows.Forms.GroupBox gbxGroupSearchStatus;
		private System.Windows.Forms.GroupBox gbxSearchUserStatus;
		private System.Windows.Forms.GroupBox gbxChangeGroupStatus;
		private System.Windows.Forms.GroupBox gbxChangeUserStatus;
		private System.Windows.Forms.GroupBox gbxNewUserStatus;
		private System.Windows.Forms.Button btnRemovePassword;
		private System.Windows.Forms.CheckBox cbxPermanentlyMove;
		private System.Windows.Forms.GroupBox gbxAssignTo;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboAssignToUser;


		public frmSecurity(SessionAddressBlock aSAB) : base(aSAB)
		{
			AllowDragDrop = true;

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_groupSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSecurityGroups);
			_userSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSecurityUsers);

			if (_groupSecurity.AccessDenied)
			{
				tabControl1.Controls.Remove(tabGroups);
			}
			else if (_userSecurity.AccessDenied)
			{
				tabControl1.Controls.Remove(tabUsers);
			}
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes

			_permissionSetHash = new Hashtable();
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
		}

        // Begin TT#3034 - RMatelic -Display of arrows vs '+' with Window 7 
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(trvGroups.Handle, "explorer", null);
            SetWindowTheme(trvUsers.Handle, "explorer", null);
        }
        // End TT#3034

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
				this.contextMenu1.Popup -= new System.EventHandler(this.contextMenu1_Popup);
				this.btnNewUser.Click -= new System.EventHandler(this.btnNewUser_Click);
				this.lstAvailableGroups.DoubleClick -= new System.EventHandler(this.lstAvailableGroups_DoubleClick);
				this.lstAvailableUsers.DoubleClick -= new System.EventHandler(this.lstAvailableUsers_DoubleClick);
				this.btnNewGroup.Click -= new System.EventHandler(this.btnNewGroup_Click);
				this.btnChangeGroup.Click -= new System.EventHandler(this.btnChangeGroup_Click);
				this.btnChangeUser.Click -= new System.EventHandler(this.btnChangeUser_Click);
				this.tabControl1.SelectedIndexChanged -= new System.EventHandler(this.tabControl1_SelectedIndexChanged);
				this.trvGroups.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.frmSecurity_MouseDown);
				this.trvGroups.AfterExpand -= new System.Windows.Forms.TreeViewEventHandler(this.trv_AfterExpand);
				this.trvGroups.AfterCollapse -= new System.Windows.Forms.TreeViewEventHandler(this.trv_AfterCollapse);
				this.trvGroups.BeforeSelect -= new System.Windows.Forms.TreeViewCancelEventHandler(this.trv_BeforeSelect);
				this.trvGroups.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.trv_AfterSelect);
				this.trvGroups.DragEnter -= new System.Windows.Forms.DragEventHandler(this.trv_DragEnter);
				this.trvGroups.DragDrop -= new System.Windows.Forms.DragEventHandler(this.trv_DragDrop);
				this.trvUsers.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.frmSecurity_MouseDown);
				this.trvUsers.AfterExpand -= new System.Windows.Forms.TreeViewEventHandler(this.trv_AfterExpand);
				this.trvUsers.AfterCollapse -= new System.Windows.Forms.TreeViewEventHandler(this.trv_AfterCollapse);
				this.trvUsers.BeforeSelect -= new System.Windows.Forms.TreeViewCancelEventHandler(this.trv_BeforeSelect);
				this.trvUsers.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.trv_AfterSelect);
				this.trvUsers.DragEnter -= new System.Windows.Forms.DragEventHandler(this.trv_DragEnter);
				this.trvUsers.DragDrop -= new System.Windows.Forms.DragEventHandler(this.trv_DragDrop);
				this.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.frmSecurity_MouseDown);

                this.cboAssignToUser.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboAssignToUser_MIDComboBoxPropertiesChangedEvent);

				this.Load -= new System.EventHandler(this.frmSecurity_Load);
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
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pnlChangeUser = new System.Windows.Forms.Panel();
			this.btnRemovePassword = new System.Windows.Forms.Button();
			this.txtChangeUserPassConfirm = new System.Windows.Forms.TextBox();
			this.label26 = new System.Windows.Forms.Label();
			this.txtChangeUserPassword = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.gbxChangeUserStatus = new System.Windows.Forms.GroupBox();
			this.rdoChangeUserInactive = new System.Windows.Forms.RadioButton();
			this.rdoChangeUserActive = new System.Windows.Forms.RadioButton();
			this.txtChangeUserDesc = new System.Windows.Forms.TextBox();
			this.label17 = new System.Windows.Forms.Label();
			this.txtChangeUserFullName = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.btnChangeUser = new System.Windows.Forms.Button();
			this.label19 = new System.Windows.Forms.Label();
			this.txtChangeUserID = new System.Windows.Forms.TextBox();
			this.pnlNewUser = new System.Windows.Forms.Panel();
			this.lblCreateUserLike = new System.Windows.Forms.Label();
			this.txtNewUserPassConfirm = new System.Windows.Forms.TextBox();
			this.label27 = new System.Windows.Forms.Label();
			this.txtNewUserPassword = new System.Windows.Forms.TextBox();
			this.label28 = new System.Windows.Forms.Label();
			this.txtNewUserDesc = new System.Windows.Forms.TextBox();
			this.label29 = new System.Windows.Forms.Label();
			this.txtNewUserFullName = new System.Windows.Forms.TextBox();
			this.label30 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.txtNewUserID = new System.Windows.Forms.TextBox();
			this.gbxNewUserStatus = new System.Windows.Forms.GroupBox();
			this.rdoNewUserInactive = new System.Windows.Forms.RadioButton();
			this.rdoNewUserActive = new System.Windows.Forms.RadioButton();
			this.btnNewUser = new System.Windows.Forms.Button();
			this.pnlAddGroup = new System.Windows.Forms.Panel();
			this.lstAvailableGroups = new System.Windows.Forms.ListBox();
			this.label11 = new System.Windows.Forms.Label();
			this.pnlPermissions = new System.Windows.Forms.Panel();
			this.btnPermissions = new System.Windows.Forms.Button();
			this.lblDeny = new System.Windows.Forms.Label();
			this.lblAllow = new System.Windows.Forms.Label();
			this.lblPermission = new System.Windows.Forms.Label();
			this.pnlFunctionPermission = new System.Windows.Forms.Panel();
			this.pnlAddStoreGroup = new System.Windows.Forms.Panel();
			this.trvAvailableStoreGroups = new System.Windows.Forms.TreeView();
			this.label9 = new System.Windows.Forms.Label();
			this.pnlAddStore = new System.Windows.Forms.Panel();
			this.label8 = new System.Windows.Forms.Label();
			this.lstAvailableStores = new System.Windows.Forms.ListBox();
			this.pnlAddView = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.lstAvailableViews = new System.Windows.Forms.ListBox();
			this.pnlAddMLID = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.trvAvailableMLIDs = new System.Windows.Forms.TreeView();
			this.pnlAddUser = new System.Windows.Forms.Panel();
			this.lstAvailableUsers = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.pnlNewGroup = new System.Windows.Forms.Panel();
			this.lblCreateGroupLike = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.txtNewGroupDescription = new System.Windows.Forms.TextBox();
			this.gbxNewGroupStatus = new System.Windows.Forms.GroupBox();
			this.rdoNewGroupInactive = new System.Windows.Forms.RadioButton();
			this.rdoNewGroupActive = new System.Windows.Forms.RadioButton();
			this.btnNewGroup = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.txtNewGroupName = new System.Windows.Forms.TextBox();
			this.pnlSearchGroup = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.txtGroupSearchDesc = new System.Windows.Forms.TextBox();
			this.gbxGroupSearchStatus = new System.Windows.Forms.GroupBox();
			this.rdoGroupSearchInactive = new System.Windows.Forms.RadioButton();
			this.rdoGroupSearchActive = new System.Windows.Forms.RadioButton();
			this.btnGroupSearch = new System.Windows.Forms.Button();
			this.label21 = new System.Windows.Forms.Label();
			this.txtGroupSearchName = new System.Windows.Forms.TextBox();
			this.pnlSearchUser = new System.Windows.Forms.Panel();
			this.label13 = new System.Windows.Forms.Label();
			this.txtSearchUserDesc = new System.Windows.Forms.TextBox();
			this.txtSearchUserFullName = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.label32 = new System.Windows.Forms.Label();
			this.txtSearchUserName = new System.Windows.Forms.TextBox();
			this.gbxSearchUserStatus = new System.Windows.Forms.GroupBox();
			this.rdoSearchUserInactive = new System.Windows.Forms.RadioButton();
			this.rdoSearchUserActive = new System.Windows.Forms.RadioButton();
			this.btnUserSearch = new System.Windows.Forms.Button();
			this.pnlChangeGroup = new System.Windows.Forms.Panel();
			this.label10 = new System.Windows.Forms.Label();
			this.txtChangeGroupDesc = new System.Windows.Forms.TextBox();
			this.gbxChangeGroupStatus = new System.Windows.Forms.GroupBox();
			this.rdoChangeGroupInactive = new System.Windows.Forms.RadioButton();
			this.rdoChangeGroupActive = new System.Windows.Forms.RadioButton();
			this.btnChangeGroup = new System.Windows.Forms.Button();
			this.label20 = new System.Windows.Forms.Label();
			this.txtChangeGroupName = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabGroups = new System.Windows.Forms.TabPage();
			this.trvGroups = new System.Windows.Forms.TreeView();
			this.tabUsers = new System.Windows.Forms.TabPage();
			this.trvUsers = new System.Windows.Forms.TreeView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.miHowDetermined = new System.Windows.Forms.MenuItem();
			this.cmPermissions = new System.Windows.Forms.ContextMenu();
			this.cboAssignToUser = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.cbxPermanentlyMove = new System.Windows.Forms.CheckBox();
			this.gbxAssignTo = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			this.panel1.SuspendLayout();
			this.pnlChangeUser.SuspendLayout();
			this.gbxChangeUserStatus.SuspendLayout();
			this.pnlNewUser.SuspendLayout();
			this.gbxNewUserStatus.SuspendLayout();
			this.pnlAddGroup.SuspendLayout();
			this.pnlPermissions.SuspendLayout();
			this.pnlAddStoreGroup.SuspendLayout();
			this.pnlAddStore.SuspendLayout();
			this.pnlAddView.SuspendLayout();
			this.pnlAddMLID.SuspendLayout();
			this.pnlAddUser.SuspendLayout();
			this.pnlNewGroup.SuspendLayout();
			this.gbxNewGroupStatus.SuspendLayout();
			this.pnlSearchGroup.SuspendLayout();
			this.gbxGroupSearchStatus.SuspendLayout();
			this.pnlSearchUser.SuspendLayout();
			this.gbxSearchUserStatus.SuspendLayout();
			this.pnlChangeGroup.SuspendLayout();
			this.gbxChangeGroupStatus.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabGroups.SuspendLayout();
			this.tabUsers.SuspendLayout();
			this.gbxAssignTo.SuspendLayout();
			this.SuspendLayout();
			// 
			// utmMain
			// 
			this.utmMain.MenuSettings.ForceSerialization = true;
			this.utmMain.ToolbarSettings.ForceSerialization = true;
			// 
			// contextMenu1
			// 
			this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.pnlChangeUser);
			this.panel1.Controls.Add(this.pnlNewUser);
			this.panel1.Controls.Add(this.pnlAddGroup);
			this.panel1.Controls.Add(this.pnlPermissions);
			this.panel1.Controls.Add(this.pnlAddStoreGroup);
			this.panel1.Controls.Add(this.pnlAddStore);
			this.panel1.Controls.Add(this.pnlAddView);
			this.panel1.Controls.Add(this.pnlAddMLID);
			this.panel1.Controls.Add(this.pnlAddUser);
			this.panel1.Controls.Add(this.pnlNewGroup);
			this.panel1.Controls.Add(this.pnlSearchGroup);
			this.panel1.Controls.Add(this.pnlSearchUser);
			this.panel1.Controls.Add(this.pnlChangeGroup);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(227, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(357, 486);
			this.panel1.TabIndex = 20;
			// 
			// pnlChangeUser
			// 
			this.pnlChangeUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlChangeUser.Controls.Add(this.gbxAssignTo);
			this.pnlChangeUser.Controls.Add(this.btnRemovePassword);
			this.pnlChangeUser.Controls.Add(this.txtChangeUserPassConfirm);
			this.pnlChangeUser.Controls.Add(this.label26);
			this.pnlChangeUser.Controls.Add(this.txtChangeUserPassword);
			this.pnlChangeUser.Controls.Add(this.label16);
			this.pnlChangeUser.Controls.Add(this.gbxChangeUserStatus);
			this.pnlChangeUser.Controls.Add(this.txtChangeUserDesc);
			this.pnlChangeUser.Controls.Add(this.label17);
			this.pnlChangeUser.Controls.Add(this.txtChangeUserFullName);
			this.pnlChangeUser.Controls.Add(this.label18);
			this.pnlChangeUser.Controls.Add(this.btnChangeUser);
			this.pnlChangeUser.Controls.Add(this.label19);
			this.pnlChangeUser.Controls.Add(this.txtChangeUserID);
			this.pnlChangeUser.Location = new System.Drawing.Point(0, 0);
			this.pnlChangeUser.Name = "pnlChangeUser";
			this.pnlChangeUser.Size = new System.Drawing.Size(357, 486);
			this.pnlChangeUser.TabIndex = 31;
			this.pnlChangeUser.Visible = false;
			// 
			// btnRemovePassword
			// 
			this.btnRemovePassword.Location = new System.Drawing.Point(200, 208);
			this.btnRemovePassword.Name = "btnRemovePassword";
			this.btnRemovePassword.Size = new System.Drawing.Size(120, 23);
			this.btnRemovePassword.TabIndex = 27;
			this.btnRemovePassword.Text = "Remove Password";
			this.btnRemovePassword.Click += new System.EventHandler(this.btnRemovePassword_Click);
			// 
			// txtChangeUserPassConfirm
			// 
			this.txtChangeUserPassConfirm.Location = new System.Drawing.Point(24, 232);
			this.txtChangeUserPassConfirm.Name = "txtChangeUserPassConfirm";
			this.txtChangeUserPassConfirm.PasswordChar = '*';
			this.txtChangeUserPassConfirm.Size = new System.Drawing.Size(140, 20);
			this.txtChangeUserPassConfirm.TabIndex = 12;
			this.txtChangeUserPassConfirm.Text = "";
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(24, 216);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(120, 14);
			this.label26.TabIndex = 11;
			this.label26.Text = "Confirm Password:";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtChangeUserPassword
			// 
			this.txtChangeUserPassword.Location = new System.Drawing.Point(24, 184);
			this.txtChangeUserPassword.Name = "txtChangeUserPassword";
			this.txtChangeUserPassword.PasswordChar = '*';
			this.txtChangeUserPassword.Size = new System.Drawing.Size(140, 20);
			this.txtChangeUserPassword.TabIndex = 10;
			this.txtChangeUserPassword.Text = "";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(24, 72);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(67, 14);
			this.label16.TabIndex = 9;
			this.label16.Text = "Full Name:";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gbxChangeUserStatus
			// 
			this.gbxChangeUserStatus.Controls.Add(this.rdoChangeUserInactive);
			this.gbxChangeUserStatus.Controls.Add(this.rdoChangeUserActive);
			this.gbxChangeUserStatus.Location = new System.Drawing.Point(112, 264);
			this.gbxChangeUserStatus.Name = "gbxChangeUserStatus";
			this.gbxChangeUserStatus.Size = new System.Drawing.Size(120, 77);
			this.gbxChangeUserStatus.TabIndex = 8;
			this.gbxChangeUserStatus.TabStop = false;
			this.gbxChangeUserStatus.Text = "Status";
			// 
			// rdoChangeUserInactive
			// 
			this.rdoChangeUserInactive.Location = new System.Drawing.Point(13, 49);
			this.rdoChangeUserInactive.Name = "rdoChangeUserInactive";
			this.rdoChangeUserInactive.Size = new System.Drawing.Size(74, 13);
			this.rdoChangeUserInactive.TabIndex = 1;
			this.rdoChangeUserInactive.Text = "Inactive";
			// 
			// rdoChangeUserActive
			// 
			this.rdoChangeUserActive.Location = new System.Drawing.Point(13, 28);
			this.rdoChangeUserActive.Name = "rdoChangeUserActive";
			this.rdoChangeUserActive.Size = new System.Drawing.Size(74, 14);
			this.rdoChangeUserActive.TabIndex = 0;
			this.rdoChangeUserActive.Text = "Active";
			// 
			// txtChangeUserDesc
			// 
			this.txtChangeUserDesc.Location = new System.Drawing.Point(24, 136);
			this.txtChangeUserDesc.Name = "txtChangeUserDesc";
			this.txtChangeUserDesc.Size = new System.Drawing.Size(240, 20);
			this.txtChangeUserDesc.TabIndex = 6;
			this.txtChangeUserDesc.Text = "";
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(24, 168);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(67, 14);
			this.label17.TabIndex = 5;
			this.label17.Text = "Password:";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtChangeUserFullName
			// 
			this.txtChangeUserFullName.Location = new System.Drawing.Point(24, 88);
			this.txtChangeUserFullName.Name = "txtChangeUserFullName";
			this.txtChangeUserFullName.Size = new System.Drawing.Size(168, 20);
			this.txtChangeUserFullName.TabIndex = 4;
			this.txtChangeUserFullName.Text = "";
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(24, 120);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(67, 14);
			this.label18.TabIndex = 3;
			this.label18.Text = "Description:";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnChangeUser
			// 
			this.btnChangeUser.Location = new System.Drawing.Point(272, 456);
			this.btnChangeUser.Name = "btnChangeUser";
			this.btnChangeUser.Size = new System.Drawing.Size(74, 21);
			this.btnChangeUser.TabIndex = 2;
			this.btnChangeUser.Text = "Save";
			this.btnChangeUser.Click += new System.EventHandler(this.btnChangeUser_Click);
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(24, 24);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(67, 13);
			this.label19.TabIndex = 1;
			this.label19.Text = "User ID:";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtChangeUserID
			// 
			this.txtChangeUserID.Location = new System.Drawing.Point(24, 40);
			this.txtChangeUserID.Name = "txtChangeUserID";
			this.txtChangeUserID.ReadOnly = true;
			this.txtChangeUserID.Size = new System.Drawing.Size(140, 20);
			this.txtChangeUserID.TabIndex = 0;
			this.txtChangeUserID.Text = "";
			// 
			// pnlNewUser
			// 
			this.pnlNewUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlNewUser.Controls.Add(this.lblCreateUserLike);
			this.pnlNewUser.Controls.Add(this.txtNewUserPassConfirm);
			this.pnlNewUser.Controls.Add(this.label27);
			this.pnlNewUser.Controls.Add(this.txtNewUserPassword);
			this.pnlNewUser.Controls.Add(this.label28);
			this.pnlNewUser.Controls.Add(this.txtNewUserDesc);
			this.pnlNewUser.Controls.Add(this.label29);
			this.pnlNewUser.Controls.Add(this.txtNewUserFullName);
			this.pnlNewUser.Controls.Add(this.label30);
			this.pnlNewUser.Controls.Add(this.label31);
			this.pnlNewUser.Controls.Add(this.txtNewUserID);
			this.pnlNewUser.Controls.Add(this.gbxNewUserStatus);
			this.pnlNewUser.Controls.Add(this.btnNewUser);
			this.pnlNewUser.Location = new System.Drawing.Point(0, 0);
			this.pnlNewUser.Name = "pnlNewUser";
			this.pnlNewUser.Size = new System.Drawing.Size(357, 486);
			this.pnlNewUser.TabIndex = 30;
			this.pnlNewUser.Visible = false;
			// 
			// lblCreateUserLike
			// 
			this.lblCreateUserLike.Location = new System.Drawing.Point(24, 416);
			this.lblCreateUserLike.Name = "lblCreateUserLike";
			this.lblCreateUserLike.Size = new System.Drawing.Size(248, 23);
			this.lblCreateUserLike.TabIndex = 25;
			// 
			// txtNewUserPassConfirm
			// 
			this.txtNewUserPassConfirm.Location = new System.Drawing.Point(24, 240);
			this.txtNewUserPassConfirm.Name = "txtNewUserPassConfirm";
			this.txtNewUserPassConfirm.PasswordChar = '*';
			this.txtNewUserPassConfirm.Size = new System.Drawing.Size(140, 20);
			this.txtNewUserPassConfirm.TabIndex = 24;
			this.txtNewUserPassConfirm.Text = "";
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(24, 224);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(120, 14);
			this.label27.TabIndex = 23;
			this.label27.Text = "Confirm Password:";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtNewUserPassword
			// 
			this.txtNewUserPassword.Location = new System.Drawing.Point(24, 192);
			this.txtNewUserPassword.Name = "txtNewUserPassword";
			this.txtNewUserPassword.PasswordChar = '*';
			this.txtNewUserPassword.Size = new System.Drawing.Size(140, 20);
			this.txtNewUserPassword.TabIndex = 22;
			this.txtNewUserPassword.Text = "";
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(24, 80);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(67, 14);
			this.label28.TabIndex = 21;
			this.label28.Text = "Full Name:";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtNewUserDesc
			// 
			this.txtNewUserDesc.Location = new System.Drawing.Point(24, 144);
			this.txtNewUserDesc.Name = "txtNewUserDesc";
			this.txtNewUserDesc.Size = new System.Drawing.Size(240, 20);
			this.txtNewUserDesc.TabIndex = 19;
			this.txtNewUserDesc.Text = "";
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(24, 176);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(67, 14);
			this.label29.TabIndex = 18;
			this.label29.Text = "Password:";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtNewUserFullName
			// 
			this.txtNewUserFullName.Location = new System.Drawing.Point(24, 96);
			this.txtNewUserFullName.Name = "txtNewUserFullName";
			this.txtNewUserFullName.Size = new System.Drawing.Size(168, 20);
			this.txtNewUserFullName.TabIndex = 17;
			this.txtNewUserFullName.Text = "";
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(24, 128);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(67, 14);
			this.label30.TabIndex = 16;
			this.label30.Text = "Description:";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label31
			// 
			this.label31.Location = new System.Drawing.Point(24, 32);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(67, 13);
			this.label31.TabIndex = 14;
			this.label31.Text = "User ID:";
			this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtNewUserID
			// 
			this.txtNewUserID.Location = new System.Drawing.Point(24, 48);
			this.txtNewUserID.Name = "txtNewUserID";
			this.txtNewUserID.Size = new System.Drawing.Size(140, 20);
			this.txtNewUserID.TabIndex = 13;
			this.txtNewUserID.Text = "";
			// 
			// gbxNewUserStatus
			// 
			this.gbxNewUserStatus.Controls.Add(this.rdoNewUserInactive);
			this.gbxNewUserStatus.Controls.Add(this.rdoNewUserActive);
			this.gbxNewUserStatus.Location = new System.Drawing.Point(24, 280);
			this.gbxNewUserStatus.Name = "gbxNewUserStatus";
			this.gbxNewUserStatus.Size = new System.Drawing.Size(120, 77);
			this.gbxNewUserStatus.TabIndex = 8;
			this.gbxNewUserStatus.TabStop = false;
			this.gbxNewUserStatus.Text = "Status";
			// 
			// rdoNewUserInactive
			// 
			this.rdoNewUserInactive.Location = new System.Drawing.Point(13, 49);
			this.rdoNewUserInactive.Name = "rdoNewUserInactive";
			this.rdoNewUserInactive.Size = new System.Drawing.Size(74, 13);
			this.rdoNewUserInactive.TabIndex = 1;
			this.rdoNewUserInactive.Text = "Inactive";
			// 
			// rdoNewUserActive
			// 
			this.rdoNewUserActive.Location = new System.Drawing.Point(13, 28);
			this.rdoNewUserActive.Name = "rdoNewUserActive";
			this.rdoNewUserActive.Size = new System.Drawing.Size(74, 14);
			this.rdoNewUserActive.TabIndex = 0;
			this.rdoNewUserActive.Text = "Active";
			// 
			// btnNewUser
			// 
			this.btnNewUser.Location = new System.Drawing.Point(24, 376);
			this.btnNewUser.Name = "btnNewUser";
			this.btnNewUser.Size = new System.Drawing.Size(74, 21);
			this.btnNewUser.TabIndex = 2;
			this.btnNewUser.Text = "Create";
			this.btnNewUser.Click += new System.EventHandler(this.btnNewUser_Click);
			// 
			// pnlAddGroup
			// 
			this.pnlAddGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlAddGroup.Controls.Add(this.lstAvailableGroups);
			this.pnlAddGroup.Controls.Add(this.label11);
			this.pnlAddGroup.Location = new System.Drawing.Point(0, 0);
			this.pnlAddGroup.Name = "pnlAddGroup";
			this.pnlAddGroup.Size = new System.Drawing.Size(357, 486);
			this.pnlAddGroup.TabIndex = 29;
			this.pnlAddGroup.Visible = false;
			// 
			// lstAvailableGroups
			// 
			this.lstAvailableGroups.Location = new System.Drawing.Point(7, 28);
			this.lstAvailableGroups.Name = "lstAvailableGroups";
			this.lstAvailableGroups.Size = new System.Drawing.Size(226, 316);
			this.lstAvailableGroups.TabIndex = 6;
			this.lstAvailableGroups.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstAvailableGroups_MouseDown);
			this.lstAvailableGroups.DoubleClick += new System.EventHandler(this.lstAvailableGroups_DoubleClick);
			this.lstAvailableGroups.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstAvailableGroups_MouseUp);
			this.lstAvailableGroups.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstAvailableGroups_MouseMove);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(7, 7);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(93, 14);
			this.label11.TabIndex = 5;
			this.label11.Text = "Available Groups:";
			// 
			// pnlPermissions
			// 
			this.pnlPermissions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlPermissions.Controls.Add(this.btnPermissions);
			this.pnlPermissions.Controls.Add(this.lblDeny);
			this.pnlPermissions.Controls.Add(this.lblAllow);
			this.pnlPermissions.Controls.Add(this.lblPermission);
			this.pnlPermissions.Controls.Add(this.pnlFunctionPermission);
			this.pnlPermissions.Location = new System.Drawing.Point(0, 0);
			this.pnlPermissions.Name = "pnlPermissions";
			this.pnlPermissions.Size = new System.Drawing.Size(357, 486);
			this.pnlPermissions.TabIndex = 28;
			this.pnlPermissions.Visible = false;
			// 
			// btnPermissions
			// 
			this.btnPermissions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnPermissions.Location = new System.Drawing.Point(16, 352);
			this.btnPermissions.Name = "btnPermissions";
			this.btnPermissions.TabIndex = 8;
			this.btnPermissions.Text = "Save";
			this.btnPermissions.Click += new System.EventHandler(this.btnPermissions_Click);
			// 
			// lblDeny
			// 
			this.lblDeny.Location = new System.Drawing.Point(296, 48);
			this.lblDeny.Name = "lblDeny";
			this.lblDeny.Size = new System.Drawing.Size(56, 16);
			this.lblDeny.TabIndex = 7;
			this.lblDeny.Text = "Deny";
			// 
			// lblAllow
			// 
			this.lblAllow.Location = new System.Drawing.Point(240, 48);
			this.lblAllow.Name = "lblAllow";
			this.lblAllow.Size = new System.Drawing.Size(48, 16);
			this.lblAllow.TabIndex = 6;
			this.lblAllow.Text = "Allow";
			// 
			// lblPermission
			// 
			this.lblPermission.Location = new System.Drawing.Point(24, 32);
			this.lblPermission.Name = "lblPermission";
			this.lblPermission.Size = new System.Drawing.Size(200, 32);
			this.lblPermission.TabIndex = 5;
			this.lblPermission.Text = "Permission";
			// 
			// pnlFunctionPermission
			// 
			this.pnlFunctionPermission.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlFunctionPermission.AutoScroll = true;
            // Begin TT#383 - JSmith - Security setting panel is blacked out
            //this.pnlFunctionPermission.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pnlFunctionPermission.BackColor = System.Drawing.SystemColors.Control;
            // End TT#383
			this.pnlFunctionPermission.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlFunctionPermission.Location = new System.Drawing.Point(16, 64);
			this.pnlFunctionPermission.Name = "pnlFunctionPermission";
			this.pnlFunctionPermission.Size = new System.Drawing.Size(328, 272);
			this.pnlFunctionPermission.TabIndex = 4;
			// 
			// pnlAddStoreGroup
			// 
			this.pnlAddStoreGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlAddStoreGroup.Controls.Add(this.trvAvailableStoreGroups);
			this.pnlAddStoreGroup.Controls.Add(this.label9);
			this.pnlAddStoreGroup.Location = new System.Drawing.Point(0, 0);
			this.pnlAddStoreGroup.Name = "pnlAddStoreGroup";
			this.pnlAddStoreGroup.Size = new System.Drawing.Size(357, 486);
			this.pnlAddStoreGroup.TabIndex = 27;
			this.pnlAddStoreGroup.Visible = false;
			// 
			// trvAvailableStoreGroups
			// 
			this.trvAvailableStoreGroups.ImageIndex = -1;
			this.trvAvailableStoreGroups.Location = new System.Drawing.Point(7, 28);
			this.trvAvailableStoreGroups.Name = "trvAvailableStoreGroups";
			this.trvAvailableStoreGroups.SelectedImageIndex = -1;
			this.trvAvailableStoreGroups.Size = new System.Drawing.Size(226, 322);
			this.trvAvailableStoreGroups.TabIndex = 2;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(7, 7);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(146, 14);
			this.label9.TabIndex = 1;
			this.label9.Text = "Available Store Groups:";
			// 
			// pnlAddStore
			// 
			this.pnlAddStore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlAddStore.Controls.Add(this.label8);
			this.pnlAddStore.Controls.Add(this.lstAvailableStores);
			this.pnlAddStore.Location = new System.Drawing.Point(0, 0);
			this.pnlAddStore.Name = "pnlAddStore";
			this.pnlAddStore.Size = new System.Drawing.Size(357, 486);
			this.pnlAddStore.TabIndex = 26;
			this.pnlAddStore.Visible = false;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(7, 7);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(106, 14);
			this.label8.TabIndex = 1;
			//Begin TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
			//this.label8.Text = "Available Stores:";
			this.label8.Text = Include.AvailableStoresGroupLevelName;
			//End TT#724 - JScott - The "Available Stores" attribute set can be renamed in the Store Group Explorer and could cause processing problems
			// 
			// lstAvailableStores
			// 
			this.lstAvailableStores.Location = new System.Drawing.Point(7, 28);
			this.lstAvailableStores.Name = "lstAvailableStores";
			this.lstAvailableStores.Size = new System.Drawing.Size(226, 316);
			this.lstAvailableStores.TabIndex = 0;
			// 
			// pnlAddView
			// 
			this.pnlAddView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlAddView.Controls.Add(this.label7);
			this.pnlAddView.Controls.Add(this.lstAvailableViews);
			this.pnlAddView.Location = new System.Drawing.Point(0, 0);
			this.pnlAddView.Name = "pnlAddView";
			this.pnlAddView.Size = new System.Drawing.Size(357, 486);
			this.pnlAddView.TabIndex = 25;
			this.pnlAddView.Visible = false;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(7, 7);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(133, 14);
			this.label7.TabIndex = 1;
			this.label7.Text = "Available Views:";
			// 
			// lstAvailableViews
			// 
			this.lstAvailableViews.Location = new System.Drawing.Point(7, 28);
			this.lstAvailableViews.Name = "lstAvailableViews";
			this.lstAvailableViews.Size = new System.Drawing.Size(226, 316);
			this.lstAvailableViews.TabIndex = 0;
			// 
			// pnlAddMLID
			// 
			this.pnlAddMLID.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlAddMLID.Controls.Add(this.label4);
			this.pnlAddMLID.Controls.Add(this.trvAvailableMLIDs);
			this.pnlAddMLID.Location = new System.Drawing.Point(0, 0);
			this.pnlAddMLID.Name = "pnlAddMLID";
			this.pnlAddMLID.Size = new System.Drawing.Size(357, 486);
			this.pnlAddMLID.TabIndex = 22;
			this.pnlAddMLID.Visible = false;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(7, 7);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 14);
			this.label4.TabIndex = 1;
			this.label4.Text = "Available Merchandise:";
			// 
			// trvAvailableMLIDs
			// 
			this.trvAvailableMLIDs.ImageIndex = -1;
			this.trvAvailableMLIDs.Location = new System.Drawing.Point(7, 28);
			this.trvAvailableMLIDs.Name = "trvAvailableMLIDs";
			this.trvAvailableMLIDs.SelectedImageIndex = -1;
			this.trvAvailableMLIDs.Size = new System.Drawing.Size(226, 322);
			this.trvAvailableMLIDs.TabIndex = 0;
			// 
			// pnlAddUser
			// 
			this.pnlAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlAddUser.Controls.Add(this.lstAvailableUsers);
			this.pnlAddUser.Controls.Add(this.label3);
			this.pnlAddUser.Location = new System.Drawing.Point(0, 0);
			this.pnlAddUser.Name = "pnlAddUser";
			this.pnlAddUser.Size = new System.Drawing.Size(357, 486);
			this.pnlAddUser.TabIndex = 21;
			this.pnlAddUser.Visible = false;
			// 
			// lstAvailableUsers
			// 
			this.lstAvailableUsers.Location = new System.Drawing.Point(7, 28);
			this.lstAvailableUsers.Name = "lstAvailableUsers";
			this.lstAvailableUsers.Size = new System.Drawing.Size(226, 316);
			this.lstAvailableUsers.TabIndex = 6;
			this.lstAvailableUsers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstAvailableUsers_MouseDown);
			this.lstAvailableUsers.DoubleClick += new System.EventHandler(this.lstAvailableUsers_DoubleClick);
			this.lstAvailableUsers.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstAvailableUsers_MouseUp);
			this.lstAvailableUsers.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstAvailableUsers_MouseMove);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(7, 7);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(93, 14);
			this.label3.TabIndex = 5;
			this.label3.Text = "Available Users:";
			// 
			// pnlNewGroup
			// 
			this.pnlNewGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlNewGroup.Controls.Add(this.lblCreateGroupLike);
			this.pnlNewGroup.Controls.Add(this.label23);
			this.pnlNewGroup.Controls.Add(this.txtNewGroupDescription);
			this.pnlNewGroup.Controls.Add(this.gbxNewGroupStatus);
			this.pnlNewGroup.Controls.Add(this.btnNewGroup);
			this.pnlNewGroup.Controls.Add(this.label2);
			this.pnlNewGroup.Controls.Add(this.txtNewGroupName);
			this.pnlNewGroup.Location = new System.Drawing.Point(0, 0);
			this.pnlNewGroup.Name = "pnlNewGroup";
			this.pnlNewGroup.Size = new System.Drawing.Size(357, 486);
			this.pnlNewGroup.TabIndex = 20;
			this.pnlNewGroup.Visible = false;
			// 
			// lblCreateGroupLike
			// 
			this.lblCreateGroupLike.Location = new System.Drawing.Point(24, 272);
			this.lblCreateGroupLike.Name = "lblCreateGroupLike";
			this.lblCreateGroupLike.Size = new System.Drawing.Size(256, 23);
			this.lblCreateGroupLike.TabIndex = 12;
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(24, 88);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(120, 14);
			this.label23.TabIndex = 11;
			this.label23.Text = "Group Description:";
			// 
			// txtNewGroupDescription
			// 
			this.txtNewGroupDescription.Location = new System.Drawing.Point(24, 104);
			this.txtNewGroupDescription.Name = "txtNewGroupDescription";
			this.txtNewGroupDescription.Size = new System.Drawing.Size(240, 20);
			this.txtNewGroupDescription.TabIndex = 10;
			this.txtNewGroupDescription.Text = "";
			// 
			// gbxNewGroupStatus
			// 
			this.gbxNewGroupStatus.Controls.Add(this.rdoNewGroupInactive);
			this.gbxNewGroupStatus.Controls.Add(this.rdoNewGroupActive);
			this.gbxNewGroupStatus.Location = new System.Drawing.Point(24, 136);
			this.gbxNewGroupStatus.Name = "gbxNewGroupStatus";
			this.gbxNewGroupStatus.Size = new System.Drawing.Size(120, 76);
			this.gbxNewGroupStatus.TabIndex = 9;
			this.gbxNewGroupStatus.TabStop = false;
			this.gbxNewGroupStatus.Text = "Status";
			// 
			// rdoNewGroupInactive
			// 
			this.rdoNewGroupInactive.Location = new System.Drawing.Point(13, 49);
			this.rdoNewGroupInactive.Name = "rdoNewGroupInactive";
			this.rdoNewGroupInactive.Size = new System.Drawing.Size(74, 13);
			this.rdoNewGroupInactive.TabIndex = 1;
			this.rdoNewGroupInactive.Text = "Inactive";
			// 
			// rdoNewGroupActive
			// 
			this.rdoNewGroupActive.Location = new System.Drawing.Point(13, 28);
			this.rdoNewGroupActive.Name = "rdoNewGroupActive";
			this.rdoNewGroupActive.Size = new System.Drawing.Size(74, 14);
			this.rdoNewGroupActive.TabIndex = 0;
			this.rdoNewGroupActive.Text = "Active";
			// 
			// btnNewGroup
			// 
			this.btnNewGroup.Location = new System.Drawing.Point(24, 232);
			this.btnNewGroup.Name = "btnNewGroup";
			this.btnNewGroup.Size = new System.Drawing.Size(73, 21);
			this.btnNewGroup.TabIndex = 2;
			this.btnNewGroup.Text = "Create";
			this.btnNewGroup.Click += new System.EventHandler(this.btnNewGroup_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(74, 14);
			this.label2.TabIndex = 1;
			this.label2.Text = "Group Name:";
			// 
			// txtNewGroupName
			// 
			this.txtNewGroupName.Location = new System.Drawing.Point(24, 56);
			this.txtNewGroupName.Name = "txtNewGroupName";
			this.txtNewGroupName.Size = new System.Drawing.Size(140, 20);
			this.txtNewGroupName.TabIndex = 0;
			this.txtNewGroupName.Text = "";
			// 
			// pnlSearchGroup
			// 
			this.pnlSearchGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlSearchGroup.Controls.Add(this.label1);
			this.pnlSearchGroup.Controls.Add(this.txtGroupSearchDesc);
			this.pnlSearchGroup.Controls.Add(this.gbxGroupSearchStatus);
			this.pnlSearchGroup.Controls.Add(this.btnGroupSearch);
			this.pnlSearchGroup.Controls.Add(this.label21);
			this.pnlSearchGroup.Controls.Add(this.txtGroupSearchName);
			this.pnlSearchGroup.Location = new System.Drawing.Point(0, 0);
			this.pnlSearchGroup.Name = "pnlSearchGroup";
			this.pnlSearchGroup.Size = new System.Drawing.Size(357, 486);
			this.pnlSearchGroup.TabIndex = 33;
			this.pnlSearchGroup.Visible = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 12);
			this.label1.TabIndex = 11;
			this.label1.Text = "Group Description";
			// 
			// txtGroupSearchDesc
			// 
			this.txtGroupSearchDesc.Location = new System.Drawing.Point(24, 96);
			this.txtGroupSearchDesc.Name = "txtGroupSearchDesc";
			this.txtGroupSearchDesc.Size = new System.Drawing.Size(232, 20);
			this.txtGroupSearchDesc.TabIndex = 10;
			this.txtGroupSearchDesc.Text = "";
			// 
			// gbxGroupSearchStatus
			// 
			this.gbxGroupSearchStatus.Controls.Add(this.rdoGroupSearchInactive);
			this.gbxGroupSearchStatus.Controls.Add(this.rdoGroupSearchActive);
			this.gbxGroupSearchStatus.Location = new System.Drawing.Point(24, 128);
			this.gbxGroupSearchStatus.Name = "gbxGroupSearchStatus";
			this.gbxGroupSearchStatus.Size = new System.Drawing.Size(120, 76);
			this.gbxGroupSearchStatus.TabIndex = 9;
			this.gbxGroupSearchStatus.TabStop = false;
			this.gbxGroupSearchStatus.Text = "Status";
			// 
			// rdoGroupSearchInactive
			// 
			this.rdoGroupSearchInactive.Location = new System.Drawing.Point(13, 49);
			this.rdoGroupSearchInactive.Name = "rdoGroupSearchInactive";
			this.rdoGroupSearchInactive.Size = new System.Drawing.Size(74, 13);
			this.rdoGroupSearchInactive.TabIndex = 1;
			this.rdoGroupSearchInactive.Text = "Inactive";
			// 
			// rdoGroupSearchActive
			// 
			this.rdoGroupSearchActive.Location = new System.Drawing.Point(13, 28);
			this.rdoGroupSearchActive.Name = "rdoGroupSearchActive";
			this.rdoGroupSearchActive.Size = new System.Drawing.Size(74, 14);
			this.rdoGroupSearchActive.TabIndex = 0;
			this.rdoGroupSearchActive.Text = "Active";
			// 
			// btnGroupSearch
			// 
			this.btnGroupSearch.Location = new System.Drawing.Point(24, 232);
			this.btnGroupSearch.Name = "btnGroupSearch";
			this.btnGroupSearch.Size = new System.Drawing.Size(73, 21);
			this.btnGroupSearch.TabIndex = 2;
			this.btnGroupSearch.Text = "Search";
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(24, 32);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(74, 14);
			this.label21.TabIndex = 1;
			this.label21.Text = "Group Name:";
			// 
			// txtGroupSearchName
			// 
			this.txtGroupSearchName.Location = new System.Drawing.Point(24, 48);
			this.txtGroupSearchName.Name = "txtGroupSearchName";
			this.txtGroupSearchName.Size = new System.Drawing.Size(140, 20);
			this.txtGroupSearchName.TabIndex = 0;
			this.txtGroupSearchName.Text = "";
			// 
			// pnlSearchUser
			// 
			this.pnlSearchUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlSearchUser.Controls.Add(this.label13);
			this.pnlSearchUser.Controls.Add(this.txtSearchUserDesc);
			this.pnlSearchUser.Controls.Add(this.txtSearchUserFullName);
			this.pnlSearchUser.Controls.Add(this.label15);
			this.pnlSearchUser.Controls.Add(this.label32);
			this.pnlSearchUser.Controls.Add(this.txtSearchUserName);
			this.pnlSearchUser.Controls.Add(this.gbxSearchUserStatus);
			this.pnlSearchUser.Controls.Add(this.btnUserSearch);
			this.pnlSearchUser.Location = new System.Drawing.Point(0, 0);
			this.pnlSearchUser.Name = "pnlSearchUser";
			this.pnlSearchUser.Size = new System.Drawing.Size(357, 486);
			this.pnlSearchUser.TabIndex = 34;
			this.pnlSearchUser.Visible = false;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(24, 88);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(67, 14);
			this.label13.TabIndex = 21;
			this.label13.Text = "Full Name:";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtSearchUserDesc
			// 
			this.txtSearchUserDesc.Location = new System.Drawing.Point(24, 152);
			this.txtSearchUserDesc.Name = "txtSearchUserDesc";
			this.txtSearchUserDesc.Size = new System.Drawing.Size(240, 20);
			this.txtSearchUserDesc.TabIndex = 19;
			this.txtSearchUserDesc.Text = "";
			// 
			// txtSearchUserFullName
			// 
			this.txtSearchUserFullName.Location = new System.Drawing.Point(24, 104);
			this.txtSearchUserFullName.Name = "txtSearchUserFullName";
			this.txtSearchUserFullName.Size = new System.Drawing.Size(168, 20);
			this.txtSearchUserFullName.TabIndex = 17;
			this.txtSearchUserFullName.Text = "";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(24, 136);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(67, 14);
			this.label15.TabIndex = 16;
			this.label15.Text = "Description:";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(24, 40);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(67, 13);
			this.label32.TabIndex = 14;
			this.label32.Text = "User ID:";
			this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtSearchUserName
			// 
			this.txtSearchUserName.Location = new System.Drawing.Point(24, 56);
			this.txtSearchUserName.Name = "txtSearchUserName";
			this.txtSearchUserName.Size = new System.Drawing.Size(140, 20);
			this.txtSearchUserName.TabIndex = 13;
			this.txtSearchUserName.Text = "";
			// 
			// gbxSearchUserStatus
			// 
			this.gbxSearchUserStatus.Controls.Add(this.rdoSearchUserInactive);
			this.gbxSearchUserStatus.Controls.Add(this.rdoSearchUserActive);
			this.gbxSearchUserStatus.Location = new System.Drawing.Point(24, 200);
			this.gbxSearchUserStatus.Name = "gbxSearchUserStatus";
			this.gbxSearchUserStatus.Size = new System.Drawing.Size(120, 76);
			this.gbxSearchUserStatus.TabIndex = 8;
			this.gbxSearchUserStatus.TabStop = false;
			this.gbxSearchUserStatus.Text = "Status";
			// 
			// rdoSearchUserInactive
			// 
			this.rdoSearchUserInactive.Location = new System.Drawing.Point(13, 49);
			this.rdoSearchUserInactive.Name = "rdoSearchUserInactive";
			this.rdoSearchUserInactive.Size = new System.Drawing.Size(74, 13);
			this.rdoSearchUserInactive.TabIndex = 1;
			this.rdoSearchUserInactive.Text = "Inactive";
			// 
			// rdoSearchUserActive
			// 
			this.rdoSearchUserActive.Location = new System.Drawing.Point(13, 28);
			this.rdoSearchUserActive.Name = "rdoSearchUserActive";
			this.rdoSearchUserActive.Size = new System.Drawing.Size(74, 14);
			this.rdoSearchUserActive.TabIndex = 0;
			this.rdoSearchUserActive.Text = "Active";
			// 
			// btnUserSearch
			// 
			this.btnUserSearch.Location = new System.Drawing.Point(24, 296);
			this.btnUserSearch.Name = "btnUserSearch";
			this.btnUserSearch.Size = new System.Drawing.Size(74, 21);
			this.btnUserSearch.TabIndex = 2;
			this.btnUserSearch.Text = "Search";
			// 
			// pnlChangeGroup
			// 
			this.pnlChangeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlChangeGroup.Controls.Add(this.label10);
			this.pnlChangeGroup.Controls.Add(this.txtChangeGroupDesc);
			this.pnlChangeGroup.Controls.Add(this.gbxChangeGroupStatus);
			this.pnlChangeGroup.Controls.Add(this.btnChangeGroup);
			this.pnlChangeGroup.Controls.Add(this.label20);
			this.pnlChangeGroup.Controls.Add(this.txtChangeGroupName);
			this.pnlChangeGroup.Location = new System.Drawing.Point(0, 0);
			this.pnlChangeGroup.Name = "pnlChangeGroup";
			this.pnlChangeGroup.Size = new System.Drawing.Size(357, 486);
			this.pnlChangeGroup.TabIndex = 32;
			this.pnlChangeGroup.Visible = false;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(24, 80);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(104, 14);
			this.label10.TabIndex = 11;
			this.label10.Text = "Group Description:";
			// 
			// txtChangeGroupDesc
			// 
			this.txtChangeGroupDesc.Location = new System.Drawing.Point(24, 96);
			this.txtChangeGroupDesc.Name = "txtChangeGroupDesc";
			this.txtChangeGroupDesc.Size = new System.Drawing.Size(248, 20);
			this.txtChangeGroupDesc.TabIndex = 10;
			this.txtChangeGroupDesc.Text = "";
			// 
			// gbxChangeGroupStatus
			// 
			this.gbxChangeGroupStatus.Controls.Add(this.rdoChangeGroupInactive);
			this.gbxChangeGroupStatus.Controls.Add(this.rdoChangeGroupActive);
			this.gbxChangeGroupStatus.Location = new System.Drawing.Point(24, 136);
			this.gbxChangeGroupStatus.Name = "gbxChangeGroupStatus";
			this.gbxChangeGroupStatus.Size = new System.Drawing.Size(120, 76);
			this.gbxChangeGroupStatus.TabIndex = 9;
			this.gbxChangeGroupStatus.TabStop = false;
			this.gbxChangeGroupStatus.Text = "Status";
			// 
			// rdoChangeGroupInactive
			// 
			this.rdoChangeGroupInactive.Location = new System.Drawing.Point(13, 49);
			this.rdoChangeGroupInactive.Name = "rdoChangeGroupInactive";
			this.rdoChangeGroupInactive.Size = new System.Drawing.Size(74, 13);
			this.rdoChangeGroupInactive.TabIndex = 1;
			this.rdoChangeGroupInactive.Text = "Inactive";
			// 
			// rdoChangeGroupActive
			// 
			this.rdoChangeGroupActive.Location = new System.Drawing.Point(13, 28);
			this.rdoChangeGroupActive.Name = "rdoChangeGroupActive";
			this.rdoChangeGroupActive.Size = new System.Drawing.Size(74, 14);
			this.rdoChangeGroupActive.TabIndex = 0;
			this.rdoChangeGroupActive.Text = "Active";
			// 
			// btnChangeGroup
			// 
			this.btnChangeGroup.Location = new System.Drawing.Point(24, 232);
			this.btnChangeGroup.Name = "btnChangeGroup";
			this.btnChangeGroup.Size = new System.Drawing.Size(73, 21);
			this.btnChangeGroup.TabIndex = 2;
			this.btnChangeGroup.Text = "Save";
			this.btnChangeGroup.Click += new System.EventHandler(this.btnChangeGroup_Click);
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(24, 32);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(74, 14);
			this.label20.TabIndex = 1;
			this.label20.Text = "Group Name:";
			// 
			// txtChangeGroupName
			// 
			this.txtChangeGroupName.Location = new System.Drawing.Point(24, 48);
			this.txtChangeGroupName.Name = "txtChangeGroupName";
			this.txtChangeGroupName.ReadOnly = true;
			this.txtChangeGroupName.Size = new System.Drawing.Size(140, 20);
			this.txtChangeGroupName.TabIndex = 0;
			this.txtChangeGroupName.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(0, 0);
			this.label5.Name = "label5";
			this.label5.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.tabControl1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(224, 486);
			this.panel2.TabIndex = 21;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabGroups);
			this.tabControl1.Controls.Add(this.tabUsers);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.ItemSize = new System.Drawing.Size(50, 18);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(224, 486);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabGroups
			// 
			this.tabGroups.Controls.Add(this.trvGroups);
			this.tabGroups.Location = new System.Drawing.Point(4, 22);
			this.tabGroups.Name = "tabGroups";
			this.tabGroups.Size = new System.Drawing.Size(216, 460);
			this.tabGroups.TabIndex = 0;
			this.tabGroups.Text = "Groups";
			// 
			// trvGroups
			// 
			this.trvGroups.AllowDrop = true;
			this.trvGroups.ContextMenu = this.contextMenu1;
			this.trvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trvGroups.ImageIndex = -1;
			this.trvGroups.Location = new System.Drawing.Point(0, 0);
			this.trvGroups.Name = "trvGroups";
			this.trvGroups.SelectedImageIndex = -1;
			this.trvGroups.Size = new System.Drawing.Size(216, 460);
			this.trvGroups.TabIndex = 3;
			this.trvGroups.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmSecurity_MouseDown);
			this.trvGroups.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.trv_AfterExpand);
			this.trvGroups.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.trv_AfterCollapse);
			this.trvGroups.DragOver += new System.Windows.Forms.DragEventHandler(this.trv_DragOver);
			this.trvGroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trv_AfterSelect);
			this.trvGroups.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.trv_BeforeSelect);
			this.trvGroups.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trv_BeforeExpand);
			this.trvGroups.DragEnter += new System.Windows.Forms.DragEventHandler(this.trv_DragEnter);
			this.trvGroups.DragDrop += new System.Windows.Forms.DragEventHandler(this.trv_DragDrop);
			// 
			// tabUsers
			// 
			this.tabUsers.Controls.Add(this.trvUsers);
			this.tabUsers.Location = new System.Drawing.Point(4, 22);
			this.tabUsers.Name = "tabUsers";
			this.tabUsers.Size = new System.Drawing.Size(216, 460);
			this.tabUsers.TabIndex = 1;
			this.tabUsers.Text = "Users";
			this.tabUsers.Visible = false;
			// 
			// trvUsers
			// 
			this.trvUsers.AllowDrop = true;
			this.trvUsers.ContextMenu = this.contextMenu1;
			this.trvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trvUsers.ImageIndex = -1;
			this.trvUsers.Location = new System.Drawing.Point(0, 0);
			this.trvUsers.Name = "trvUsers";
			this.trvUsers.SelectedImageIndex = -1;
			this.trvUsers.Size = new System.Drawing.Size(216, 460);
			this.trvUsers.TabIndex = 0;
			this.trvUsers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmSecurity_MouseDown);
			this.trvUsers.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.trv_AfterExpand);
			this.trvUsers.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.trv_AfterCollapse);
			this.trvUsers.DragOver += new System.Windows.Forms.DragEventHandler(this.trv_DragOver);
			this.trvUsers.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trv_AfterSelect);
			this.trvUsers.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.trv_BeforeSelect);
			this.trvUsers.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trv_BeforeExpand);
			this.trvUsers.DragEnter += new System.Windows.Forms.DragEventHandler(this.trv_DragEnter);
			this.trvUsers.DragDrop += new System.Windows.Forms.DragEventHandler(this.trv_DragDrop);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(224, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 486);
			this.splitter1.TabIndex = 22;
			this.splitter1.TabStop = false;
			// 
			// miHowDetermined
			// 
			this.miHowDetermined.Index = -1;
			this.miHowDetermined.Text = "";
			// 
			// cboAssignToUser
			// 
			this.cboAssignToUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboAssignToUser.Location = new System.Drawing.Point(24, 32);
			this.cboAssignToUser.Name = "cboAssignToUser";
			this.cboAssignToUser.Size = new System.Drawing.Size(208, 21);
			this.cboAssignToUser.TabIndex = 30;
			this.cboAssignToUser.SelectionChangeCommitted += new System.EventHandler(this.cboAssignToUser_SelectionChangeCommitted);
            this.cboAssignToUser.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboAssignToUser_MIDComboBoxPropertiesChangedEvent);
			// 
			// cbxPermanentlyMove
			// 
			this.cbxPermanentlyMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbxPermanentlyMove.Location = new System.Drawing.Point(8, 72);
			this.cbxPermanentlyMove.Name = "cbxPermanentlyMove";
			this.cbxPermanentlyMove.Size = new System.Drawing.Size(184, 24);
			this.cbxPermanentlyMove.TabIndex = 28;
			this.cbxPermanentlyMove.Text = "Permanently move user";
			// 
			// gbxAssignTo
			// 
			this.gbxAssignTo.Controls.Add(this.cbxPermanentlyMove);
			this.gbxAssignTo.Controls.Add(this.cboAssignToUser);
			this.gbxAssignTo.Location = new System.Drawing.Point(56, 344);
			this.gbxAssignTo.Name = "gbxAssignTo";
			this.gbxAssignTo.Size = new System.Drawing.Size(248, 100);
			this.gbxAssignTo.TabIndex = 31;
			this.gbxAssignTo.TabStop = false;
			this.gbxAssignTo.Text = "Assign To";
			// 
			// frmSecurity
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 486);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel2);
			this.Name = "frmSecurity";
			this.Text = "Security";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmSecurity_MouseDown);
			this.Load += new System.EventHandler(this.frmSecurity_Load);
			this.Controls.SetChildIndex(this.panel2, 0);
			this.Controls.SetChildIndex(this.splitter1, 0);
			this.Controls.SetChildIndex(this.panel1, 0);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			this.panel1.ResumeLayout(false);
			this.pnlChangeUser.ResumeLayout(false);
			this.gbxAssignTo.ResumeLayout(false);
			this.gbxChangeUserStatus.ResumeLayout(false);
			this.pnlNewUser.ResumeLayout(false);
			this.gbxNewUserStatus.ResumeLayout(false);
			this.pnlAddGroup.ResumeLayout(false);
			this.pnlPermissions.ResumeLayout(false);
			this.pnlAddStoreGroup.ResumeLayout(false);
			this.pnlAddStore.ResumeLayout(false);
			this.pnlAddView.ResumeLayout(false);
			this.pnlAddMLID.ResumeLayout(false);
			this.pnlAddUser.ResumeLayout(false);
			this.pnlNewGroup.ResumeLayout(false);
			this.gbxNewGroupStatus.ResumeLayout(false);
			this.pnlSearchGroup.ResumeLayout(false);
			this.gbxGroupSearchStatus.ResumeLayout(false);
			this.pnlSearchUser.ResumeLayout(false);
			this.gbxSearchUserStatus.ResumeLayout(false);
			this.pnlChangeGroup.ResumeLayout(false);
			this.gbxChangeGroupStatus.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabGroups.ResumeLayout(false);
			this.tabUsers.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		//		private enum eImageIndex
		//		{
		//			userImage		= 0,
		//			groupImage		= 1,
		//			closedFolder	= 2,
		//			openFolder		= 3,
		//			noAccess		= 4,
		//			readOnlyAccess	= 5,
		//			fullAccess		= 6
		//		};

		private void frmSecurity_Load(object sender, System.EventArgs e)
		{
			trvGroups.ImageList = MIDGraphics.ImageList;
			//trvGroups.Tag = "Groups";
			trvUsers.ImageList = MIDGraphics.ImageList;
			//trvUsers.Tag = "Users";
			BuildContextMenu();
			cmPermissions.MenuItems.Add("Show How Determined...", new System.EventHandler(ShowInheritance));

			rdoNewGroupActive.Checked = true;
			rdoChangeGroupActive.Checked = true;
			rdoNewUserActive.Checked = true;
			rdoChangeUserActive.Checked = true;

			_securityData = new SecurityAdmin();
			_security = new MIDRetail.Business.Security();
			_dtSecurityFunctions = _securityData.GetSecurityFunctions();

			// Version List
			ForecastVersion fv = new ForecastVersion();
			_dtVersions = fv.GetForecastVersions();
			
			BuildGroupList();

			BuildUserList();

			if (!_groupSecurity.AccessDenied)
			{
				BuildGroupPanel();
				_rebuildUserPanel = true;
			}
			else
			{
				BuildUserPanel();
				_rebuildUserPanel = false;
			}

			//			_rebuildInheritedPanel = true;

			// Function List
			_dtFunctions = MIDText.GetTextType(eMIDTextType.eSecurityFunctions, eMIDTextOrderBy.TextValue);

			// View List
			//			_dtViews = MIDText.GetTextType(eMIDTextType.eSecurityViews, eMIDTextOrderBy.TextValue);
			//			lstAvailableViews.DataSource = _dtViews;
			//			lstAvailableViews.DisplayMember = "TEXT_VALUE";
			//			lstAvailableViews.ValueMember = "TEXT_CODE";
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			SetText();
			//End Track #4815
			SetReadOnly(true);
		}

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		private void SetText()
		{
			cbxPermanentlyMove.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PermanentlyMoveUser);
			gbxAssignTo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AssignTo);
		}
		//End Track #4815

		private void BuildGroupList()
		{
			_dtGroups = _securityData.GetGroups();
			_dtActiveGroups = _securityData.GetActiveGroups();
			//			lstAvailableGroups.DataSource = dt;
			//			lstAvailableGroups.DisplayMember = "GROUP_NAME";
			//			lstAvailableGroups.ValueMember = "GROUP_RID";
		}

		// build the Group Panel now
		private void BuildGroupPanel()
		{
			_rebuildGroupPanel = false;
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			bool itemDeleted;
			//End Track #4815

			System.Windows.Forms.TreeNode level1;

			trvGroups.BeginUpdate();

			try
			{
				Cursor.Current = Cursors.WaitCursor;
				trvGroups.Nodes.Clear();

				// add groups first
				foreach (DataRow drGroup in _dtGroups.Rows)
				{
					level1 = new TreeNode((string)drGroup["GROUP_NAME"]);
					if ((string)drGroup["GROUP_ACTIVE_IND"] == "0")
						level1.ForeColor = System.Drawing.Color.SlateBlue;
					//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//					level1.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecGroupImage);
//					level1.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecGroupImage);
//					level1.Tag = new NodeTag(NodeType.NT_Group, Convert.ToInt32(drGroup["GROUP_RID"], CultureInfo.CurrentUICulture), eSecurityOwnerType.Group, Convert.ToInt32(drGroup["GROUP_RID"]));
					itemDeleted = Include.ConvertCharToBool(Convert.ToChar(drGroup["GROUP_DELETE_IND"]));
					if (itemDeleted)
					{
						level1.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteImage);
						level1.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteImage);
					}
					else
					{
						level1.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
						level1.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
					}
					level1.Tag = new NodeTag(NodeType.NT_Group, Convert.ToInt32(drGroup["GROUP_RID"], CultureInfo.CurrentUICulture), eSecurityOwnerType.Group, Convert.ToInt32(drGroup["GROUP_RID"]), itemDeleted);
					//End Track #4815
					trvGroups.Nodes.Add(level1);

					AddGroupSubFolders(level1, Convert.ToInt32(drGroup["GROUP_RID"], CultureInfo.CurrentUICulture));
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				trvGroups.EndUpdate();
				Cursor.Current = Cursors.Default;
			}
		}

		private void AddGroupSubFolders(TreeNode level1, int groupRID)
		{
			System.Windows.Forms.TreeNode level2;
			System.Windows.Forms.TreeNode level3;

			// Users Folder
			level2 = new System.Windows.Forms.TreeNode("Users");
			level2.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.Tag = new NodeTag(NodeType.NT_UserFolder, 0, eSecurityOwnerType.Group, groupRID);
			level1.Nodes.Add(level2);

			// Get users in group
			DataTable dtUsersInGroup = _securityData.GetUsers(groupRID);
			foreach (DataRow drUser in dtUsersInGroup.Rows)
			{
				level3 = new TreeNode((string)drUser["USER_NAME"]);
				if ((string)drUser["USER_ACTIVE_IND"] == "0")
					level3.ForeColor = System.Drawing.Color.SlateBlue;
				level3.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
				level3.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
				level3.Tag = new NodeTag(NodeType.NT_User, Convert.ToInt32(drUser["USER_RID"], CultureInfo.CurrentUICulture), eSecurityOwnerType.Group, groupRID);
				level2.Nodes.Add(level3);
			}

			// HNID folder
			level2 = new TreeNode("Merchandise");
			level2.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.Tag = new NodeTag(NodeType.NT_MLIDFolder, 0, eSecurityOwnerType.Group, groupRID);
			level1.Nodes.Add(level2);

			// Get node group security
			//			DataTable dt = _securityData.GetGroupNodesAssignment(groupRID);
			DataTable dt = _securityData.GetDistinctGroupNodesAssignment(groupRID);
			foreach (DataRow dr in dt.Rows)
			{
				int nodeRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(nodeRID, false);
				level3 = new TreeNode(hnp.Text);
				level3.Tag = new NodeTag(NodeType.NT_MLID, nodeRID, eSecurityOwnerType.Group, groupRID);
				level2.Nodes.Add(level3);
			}

			//Begin TT#882 - JScott - Hierarchies or Merchandise in the list of security should be in alphanumeric order.
			SortChildNodes(level2);

			//End TT#882 - JScott - Hierarchies or Merchandise in the list of security should be in alphanumeric order.
			// Version folders
			level2 = new System.Windows.Forms.TreeNode("Versions");
			level2.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.Tag = new NodeTag(NodeType.NT_VersionFolder, 0, eSecurityOwnerType.Group, groupRID);
			level1.Nodes.Add(level2);


			// Get version group security
			//			dt = _securityData.GetGroupVersionsAssignment(groupRID);
			foreach (DataRow dr in _dtVersions.Rows)
			{
				level3 = new TreeNode((string)dr["DESCRIPTION"]);
				level3.Tag = new NodeTag(NodeType.NT_Version, Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture), eSecurityOwnerType.Group, groupRID);
				level2.Nodes.Add(level3);
			}


			// Function folder
			level2 = new System.Windows.Forms.TreeNode("Functions");
			level2.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.Tag = new NodeTag(NodeType.NT_FunctionFolder, 0, eSecurityOwnerType.Group, groupRID);
			level1.Nodes.Add(level2);


			// Get function group security
			BuildFunctionGroups(level2, 0, 0, eSecurityOwnerType.Group, groupRID);
		}


		private void BuildUserList()
		{
			_dtUsers = _securityData.GetUsers();
			foreach (DataRow dr in _dtUsers.Rows)
			{
				int userRID = (Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture));
				if (userRID == Include.SystemUserRID ||
					userRID == Include.GlobalUserRID)
				{
					dr.Delete();
				}
			}
			_dtUsers.AcceptChanges();

			_dtActiveUsers = _securityData.GetActiveUsers();
			foreach (DataRow dr in _dtActiveUsers.Rows)
			{
				int userRID = (Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture));
				if (userRID == Include.SystemUserRID ||
					userRID == Include.GlobalUserRID)
				{
					dr.Delete();
				}
			}
			_dtActiveUsers.AcceptChanges();
			//			lstAvailableUsers.DataSource = dt;
			//			lstAvailableUsers.DisplayMember = "USER_NAME";
			//			lstAvailableUsers.ValueMember = "USER_RID";
		}

		// build the User Panel now
		private void BuildUserPanel()
		{
			int userRID;
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			bool itemDeleted;
			//End Track #4815

			_rebuildUserPanel = false;

			System.Windows.Forms.TreeNode level1;

			trvUsers.BeginUpdate();

			try
			{
				Cursor.Current = Cursors.WaitCursor;
				trvUsers.Nodes.Clear();

				// add users first
				foreach (DataRow drUser in _dtUsers.Rows)
				{
					level1 = new TreeNode((string)drUser["USER_NAME"]);
					if ((string)drUser["USER_ACTIVE_IND"] == "0")
						level1.ForeColor = System.Drawing.Color.SlateBlue;
					//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//					level1.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
//					level1.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
//					level1.Tag = new NodeTag(NodeType.NT_User, Convert.ToInt32(drUser["USER_RID"], CultureInfo.CurrentUICulture), eSecurityOwnerType.User, Convert.ToInt32(drUser["USER_RID"], CultureInfo.CurrentUICulture));
					itemDeleted = Include.ConvertCharToBool(Convert.ToChar(drUser["USER_DELETE_IND"]));
					if (itemDeleted)
					{
						level1.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteImage);
						level1.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteImage);
					}
					else
					{
						level1.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
						level1.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
					}
					level1.Tag = new NodeTag(NodeType.NT_User, Convert.ToInt32(drUser["USER_RID"], CultureInfo.CurrentUICulture), eSecurityOwnerType.User, Convert.ToInt32(drUser["USER_RID"], CultureInfo.CurrentUICulture), itemDeleted);
					//End Track #4815
					trvUsers.Nodes.Add(level1);

					userRID = Convert.ToInt32(drUser["USER_RID"], CultureInfo.CurrentUICulture);

					if (userRID != Include.AdministratorUserRID)
					{
						AddUserSubFolders(level1, userRID, false);
					}
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				trvUsers.EndUpdate();
				Cursor.Current = Cursors.Default;
			}
		}


		private void AddUserSubFolders(TreeNode level1, int userRID, bool inheritedFlag)
		{
			System.Windows.Forms.TreeNode level2;
			System.Windows.Forms.TreeNode level3;
			DataTable dt;

			// Groups folder
			if (! inheritedFlag)
			{
				level2 = new System.Windows.Forms.TreeNode("Groups");
				level2.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
				level2.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
				level2.Tag = new NodeTag(NodeType.NT_GroupFolder, 0, eSecurityOwnerType.User, userRID);
				level1.Nodes.Add(level2);

				// Get groups for this user
				dt = _securityData.GetGroups(userRID);
				foreach (DataRow dr in dt.Rows)
				{
					level3 = new TreeNode((string)dr["GROUP_NAME"]);
					if ((string)dr["GROUP_ACTIVE_IND"] == "0")
						level3.ForeColor = System.Drawing.Color.SlateBlue;
					level3.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecGroupImage);
					level3.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecGroupImage);
					level3.Tag = new NodeTag(NodeType.NT_Group, Convert.ToInt32(dr["GROUP_RID"], CultureInfo.CurrentUICulture), eSecurityOwnerType.User, userRID);
					level2.Nodes.Add(level3);
				}
			}

			// MLIDs folder
			level2 = new System.Windows.Forms.TreeNode("Merchandise");
			level2.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.Tag = new NodeTag(NodeType.NT_MLIDFolder, 0, eSecurityOwnerType.User, userRID);
			level1.Nodes.Add(level2);

			// Get node user security
			Hashtable nodes = new Hashtable();
			//			dt = _securityData.GetUserNodesAssignment(userRID, inheritedFlag);
			dt  = _securityData.GetDistinctUserNodesAssignment(userRID);
			foreach (DataRow dr in dt.Rows)
			{
				int nodeRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(nodeRID, false);
				//				level3 = new TreeNode((string)dr["BN_ID"]);
				level3 = new TreeNode(hnp.Text);
				level3.Tag = new NodeTag(NodeType.NT_MLID, nodeRID, eSecurityOwnerType.User, userRID);
				level2.Nodes.Add(level3);
				nodes.Add(nodeRID, null);
				// this was just stuffed in for testing
				//				Security testSec = new Security();
				//				testSec.GetUserNodeAssignment(userRID, Convert.ToInt32(dr["HN_RID"]), SAB);

			}

			// Get node group security for user
			dt = _securityData.GetDistinctUserGroupsNodesAssignment(userRID);
			foreach (DataRow dr in dt.Rows)
			{
				int nodeRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
				if (!nodes.Contains(nodeRID))
				{
					HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(nodeRID, false);
					level3 = new TreeNode(hnp.Text);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//					int groupRID = Convert.ToInt32(dr["GROUP_RID"], CultureInfo.CurrentUICulture);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
					level3.Tag = new NodeTag(NodeType.NT_MLID, nodeRID, eSecurityOwnerType.Group, eSecurityOwnerType.User, userRID);
					level2.Nodes.Add(level3);
				}
			}

			//Begin TT#882 - JScott - Hierarchies or Merchandise in the list of security should be in alphanumeric order.
			SortChildNodes(level2);

			//End TT#882 - JScott - Hierarchies or Merchandise in the list of security should be in alphanumeric order.
			// Versions folder
			level2 = new System.Windows.Forms.TreeNode("Versions");
			level2.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.Tag = new NodeTag(NodeType.NT_VersionFolder, 0, eSecurityOwnerType.User, userRID);
			level1.Nodes.Add(level2);

			// Get version user security
			foreach (DataRow dr in _dtVersions.Rows)
			{
				level3 = new TreeNode((string)dr["DESCRIPTION"]);
				level3.Tag = new NodeTag(NodeType.NT_Version, Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture), eSecurityOwnerType.User, userRID);
				level2.Nodes.Add(level3);
			}


			// Functions folder
			level2 = new System.Windows.Forms.TreeNode("Functions");
			level2.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
			level2.Tag = new NodeTag(NodeType.NT_FunctionFolder, 0, eSecurityOwnerType.User, userRID);
			level1.Nodes.Add(level2);

			// Get function user security
			
			BuildFunctionGroups(level2, 0, 0, eSecurityOwnerType.User, userRID);
		}

		private void BuildFunctionGroups(TreeNode aParentNode, int aParentLevel, int aParentFunctionID, eSecurityOwnerType aOwnerType, int aOwnerRID)
		{
			try
			{
				TreeNode functionNode;
				int functionID;
				eSecurityFunctionTypes functionType;
				int functionLevel;
				int functionParent;
				string functionName;
				bool includeFunction = true;
				DataRow [] rows = _dtSecurityFunctions.Select("FUNC_PARENT = " + aParentFunctionID.ToString(CultureInfo.CurrentUICulture) + " and FUNC_LEVEL = " + aParentLevel.ToString(CultureInfo.CurrentUICulture));
				if (rows != null)
				{
					foreach (DataRow dr in rows)
					{
						functionID = Convert.ToInt32(dr["FUNC_ID"], CultureInfo.CurrentUICulture);
						functionType = (eSecurityFunctionTypes)Convert.ToInt32(dr["FUNC_TYPE"], CultureInfo.CurrentUICulture);
						functionLevel = Convert.ToInt32(dr["FUNC_LEVEL"], CultureInfo.CurrentUICulture);
						functionParent = Convert.ToInt32(dr["FUNC_PARENT"], CultureInfo.CurrentUICulture);
						functionName = Convert.ToString(dr["TEXT_VALUE"], CultureInfo.CurrentUICulture);
                        // Begin Track #5492 - JSmith - Remove Forecast Balance Model
                        if (functionID == Convert.ToInt32(eSecurityFunctions.AdminModelsForecastBalance) ||
                            functionID == Convert.ToInt32(eSecurityFunctions.AdminModelsForecasting))
                        {
                            includeFunction = false;
                        }
                        // Begin TT#1999-MD - JSmith - Assortment Actions 
                        // Remove Assortment actions not implemented
                        else if (functionID == Convert.ToInt32(eSecurityFunctions.AssortmentActionsChargeCommitted) 
                            || functionID == Convert.ToInt32(eSecurityFunctions.AssortmentActionsCancelCommitted)
                            || functionID == Convert.ToInt32(eSecurityFunctions.AssortmentActionsChargeIntransit)
                            || functionID == Convert.ToInt32(eSecurityFunctions.AssortmentActionsCancelIntransit)
                            )
                        {
                            includeFunction = false;
                        }
                        // End TT#1999-MD - JSmith - Assortment Actions 
                        else if (functionType == eSecurityFunctionTypes.ApplicationBase)
                        //if (functionType == eSecurityFunctionTypes.ApplicationBase)
                        // End Track #5492
						{
							includeFunction = true;
						}
						else if (functionType == eSecurityFunctionTypes.Size &&
							SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
						{
							includeFunction = true;
						}
                        else if (functionType == eSecurityFunctionTypes.Assortment &&
                            SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
                        {
                            includeFunction = true;
                        }
						// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option
                        else if (functionType == eSecurityFunctionTypes.GroupAllocation &&
                            SAB.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationInstalled)
                        {
                            includeFunction = true;
                        }
						// End TT#1247-MD - stodd - Add Group Allocation as a License Key option
                        else
                        {
                            includeFunction = false;
                        }
						if (includeFunction)
						{
							functionNode = new TreeNode(functionName);
							functionNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecFullAccessImage);
							functionNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecFullAccessImage);
							functionNode.Tag = new NodeTag(NodeType.NT_Function, functionID, aOwnerType, aOwnerRID);
							aParentNode.Nodes.Add(functionNode);
							BuildFunctionGroups(functionNode, aParentLevel + 1, functionID, aOwnerType, aOwnerRID);
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void trv_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			switch (((NodeTag)(e.Node.Tag)).Type)
			{
				case NodeType.NT_FunctionFolder:
				case NodeType.NT_Function:
				case NodeType.NT_VersionFolder:
				case NodeType.NT_MLIDFolder:
					foreach (TreeNode tn in e.Node.Nodes)
					{
						SetImage (tn);
					}
					break;
			}
		}

		private void SetImage(TreeNode aTreeNode)
		{
			try
			{
				SecurityProfile securityProfile = null;
				SecurityProfile allocationSecurityProfile = null;
				SecurityProfile chainSecurityProfile = null;
				SecurityProfile storeSecurityProfile = null;
				NodeTag nodeTag = (NodeTag)aTreeNode.Tag;
				switch (nodeTag.Type)
				{
					case NodeType.NT_MLID:
						if (nodeTag.OwnerType == eSecurityOwnerType.Group)
						{
							allocationSecurityProfile = _security.GetGroupNodeAssignment(SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Allocation, false);
							chainSecurityProfile = _security.GetGroupNodeAssignment(SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Chain, false);
							storeSecurityProfile = _security.GetGroupNodeAssignment(SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Store, false);
						}
						else
						{
							allocationSecurityProfile = _security.GetUserNodeAssignment(SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Allocation, false);
							chainSecurityProfile = _security.GetUserNodeAssignment(SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Chain, false);
							storeSecurityProfile = _security.GetUserNodeAssignment(SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Store, false);
						}

						if (allocationSecurityProfile.AllowFullControl &&
							chainSecurityProfile.AllowFullControl &&
							storeSecurityProfile.AllowFullControl)
						{
							aTreeNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecFullAccessImage);
							aTreeNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecFullAccessImage);
						}
						else if (allocationSecurityProfile.AccessDenied &&
							chainSecurityProfile.AccessDenied &&
							storeSecurityProfile.AccessDenied)
						{
							aTreeNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecNoAccessImage);
							aTreeNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecNoAccessImage);
						}
						else
						{
							aTreeNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecReadOnlyAccessImage);
							aTreeNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecReadOnlyAccessImage);
						}
						break;
					case NodeType.NT_Version:
						if (nodeTag.OwnerType == eSecurityOwnerType.Group)
						{
							chainSecurityProfile = _security.GetGroupVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Chain, false);
							storeSecurityProfile = _security.GetGroupVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Store, false);
						}
						else
						{
							chainSecurityProfile = _security.GetUserVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Chain, false);
							storeSecurityProfile = _security.GetUserVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Store, false);
						}

						if (chainSecurityProfile.AllowFullControl &&
							storeSecurityProfile.AllowFullControl)
						{
							aTreeNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecFullAccessImage);
							aTreeNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecFullAccessImage);
						}
						else if (chainSecurityProfile.AccessDenied &&
							storeSecurityProfile.AccessDenied)
						{
							aTreeNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecNoAccessImage);
							aTreeNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecNoAccessImage);
						}
						else
						{
							aTreeNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecReadOnlyAccessImage);
							aTreeNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecReadOnlyAccessImage);
						}
					
						break;
					case NodeType.NT_Function:
						if (nodeTag.OwnerType == eSecurityOwnerType.Group)
						{
							securityProfile = _security.GetGroupFunctionAssignment(nodeTag.OwnerRID, (eSecurityFunctions)nodeTag.Item, false);
						}
						else
						{
							securityProfile = _security.GetUserFunctionAssignment(nodeTag.OwnerRID, (eSecurityFunctions)nodeTag.Item, false);
						}

						if (securityProfile.AllowFullControl)
						{
							aTreeNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecFullAccessImage);
							aTreeNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecFullAccessImage);
						}
						else if (securityProfile.AccessDenied)
						{
							aTreeNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecNoAccessImage);
							aTreeNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecNoAccessImage);
						}
						else
						{
							aTreeNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecReadOnlyAccessImage);
							aTreeNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecReadOnlyAccessImage);
						}

						break;
				}
			}
			catch
			{
				throw;
			}
		}
		
		private void trv_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			switch (((NodeTag)(e.Node.Tag)).Type)
			{
				case NodeType.NT_GroupFolder:
				case NodeType.NT_UserFolder:
				case NodeType.NT_MLIDFolder:
				case NodeType.NT_VersionFolder:
				case NodeType.NT_FunctionFolder:
					//				case NodeType.NT_ViewFolder:
					//				case NodeType.NT_StoreFolder:
					//				case NodeType.NT_StoreGroupFolder:
					e.Node.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecOpenFolderImage);
					e.Node.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecOpenFolderImage);
					break;
			}
		}


		private void trv_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			switch (((NodeTag)(e.Node.Tag)).Type)
			{
				case NodeType.NT_GroupFolder:
				case NodeType.NT_UserFolder:
				case NodeType.NT_MLIDFolder:
				case NodeType.NT_VersionFolder:
				case NodeType.NT_FunctionFolder:
					//				case NodeType.NT_ViewFolder:
					//				case NodeType.NT_StoreFolder:
					//				case NodeType.NT_StoreGroupFolder:
					e.Node.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
					e.Node.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecClosedFolderImage);
					break;
			}
		}


		private void trv_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			try
			{
				// Begin MID Track# 2931 - pending changes issue
				//Begin Track #3875 - JSmith - remove permission error
				if (!_removingPermission)
				{
					CheckForSecurityChanges();
				}
				//End Track #3875 
//				if (_permissionChanged)
//				{
//					if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges),  "Save Changes",
//						MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//						== DialogResult.Yes) 
//					{
//						SaveChanges();
//						DisplayPermissions(_currNode);
//						SetImage(_currNode);
//					}
//					else
//					{
//						_permissionChanged = false;
//					}
//				}
				// End MID Track# 2931
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		
		}

		private void trv_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			DisplayPermissions(e.Node);
		}

		private void DisplayPermissions(TreeNode aNode)
		{
			NodeTag nodeTag = (NodeTag)aNode.Tag;
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
			_permissionSetHash.Clear();
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
			switch (nodeTag.Type)
			{
				case NodeType.NT_MLID:
				case NodeType.NT_Version:
				case NodeType.NT_Function:
					_currNode = aNode;
					pnlPermissions.Enabled = true;

					pnlFunctionPermission.Controls.Clear();
					this.lblPermission.Text = "Permissions for " + _currNode.Text;
					DataTable dtActions = null;
					int functionID = 0;
				switch (nodeTag.Type)
				{
					case NodeType.NT_MLID:
						dtActions = _securityData.GetActionsForMerchandise();
						break;
					case NodeType.NT_Version:
						dtActions = _securityData.GetActionsForVersion();
						break;
					case NodeType.NT_Function:
						functionID = ((NodeTag)_currNode.Tag).Item;
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//						dtActions = _securityData.GetActionsForFunction(functionID);
						dtActions = _security.GetActionsForFunction(functionID);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
						break;
				}
					int y = 5;
				switch (nodeTag.Type)
				{
					case NodeType.NT_MLID:
						HierarchyNodeSecurityProfile hierarchyNodeSecurityProfile = null;
						if (nodeTag.OwnerType == eSecurityOwnerType.Group)
						{
							hierarchyNodeSecurityProfile = _security.GetGroupNodeAssignment(SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Allocation);
						}
						else
						{
							hierarchyNodeSecurityProfile = _security.GetUserNodeAssignment(SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Allocation);
						}
						addPermissionType(eDatabaseSecurityTypes.Allocation, ref y);
						foreach (DataRow row in dtActions.Rows)
						{
							addPermission(Convert.ToString(row["TEXT_VALUE"], CultureInfo.CurrentUICulture), ref y, nodeTag.Type, (eSecurityActions)(Convert.ToInt32(row["ACTION_ID"], CultureInfo.CurrentUICulture)), functionID, eDatabaseSecurityTypes.Allocation, hierarchyNodeSecurityProfile);
						}

						y += 3;
						if (nodeTag.OwnerType == eSecurityOwnerType.Group)
						{
							hierarchyNodeSecurityProfile = _security.GetGroupNodeAssignment(SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Chain);
						}
						else
						{
							hierarchyNodeSecurityProfile = _security.GetUserNodeAssignment(SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Chain);
						}
						addPermissionType(eDatabaseSecurityTypes.Chain, ref y);
						foreach (DataRow row in dtActions.Rows)
						{
							addPermission(Convert.ToString(row["TEXT_VALUE"], CultureInfo.CurrentUICulture), ref y, nodeTag.Type, (eSecurityActions)(Convert.ToInt32(row["ACTION_ID"], CultureInfo.CurrentUICulture)), functionID, eDatabaseSecurityTypes.Chain, hierarchyNodeSecurityProfile);
						}

						y += 3;
						if (nodeTag.OwnerType == eSecurityOwnerType.Group)
						{
							hierarchyNodeSecurityProfile = _security.GetGroupNodeAssignment(SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Store);
						}
						else
						{
							hierarchyNodeSecurityProfile = _security.GetUserNodeAssignment(SAB, nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Store);
						}
						addPermissionType(eDatabaseSecurityTypes.Store, ref y);
						foreach (DataRow row in dtActions.Rows)
						{
							addPermission(Convert.ToString(row["TEXT_VALUE"], CultureInfo.CurrentUICulture), ref y, nodeTag.Type, (eSecurityActions)(Convert.ToInt32(row["ACTION_ID"], CultureInfo.CurrentUICulture)), functionID, eDatabaseSecurityTypes.Store, hierarchyNodeSecurityProfile);
						}
						break;
					case NodeType.NT_Version:
						VersionSecurityProfile versionSecurityProfile = null;
						if (nodeTag.OwnerType == eSecurityOwnerType.Group)
						{
							versionSecurityProfile = _security.GetGroupVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Chain);
						}
						else
						{
							versionSecurityProfile = _security.GetUserVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Chain);
						}
						addPermissionType(eDatabaseSecurityTypes.Chain, ref y);
						foreach (DataRow row in dtActions.Rows)
						{
							addPermission(Convert.ToString(row["TEXT_VALUE"], CultureInfo.CurrentUICulture), ref y, ((NodeTag)aNode.Tag).Type, (eSecurityActions)(Convert.ToInt32(row["ACTION_ID"], CultureInfo.CurrentUICulture)), functionID, eDatabaseSecurityTypes.Chain, versionSecurityProfile);
						}

						if (nodeTag.OwnerType == eSecurityOwnerType.Group)
						{
							versionSecurityProfile = _security.GetGroupVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Store);
						}
						else
						{
							versionSecurityProfile = _security.GetUserVersionAssignment(nodeTag.OwnerRID, nodeTag.Item, (int)eSecurityTypes.Store);
						}
						y += 3;
						addPermissionType(eDatabaseSecurityTypes.Store, ref y);
						foreach (DataRow row in dtActions.Rows)
						{
							addPermission(Convert.ToString(row["TEXT_VALUE"], CultureInfo.CurrentUICulture), ref y, nodeTag.Type, (eSecurityActions)(Convert.ToInt32(row["ACTION_ID"], CultureInfo.CurrentUICulture)), functionID, eDatabaseSecurityTypes.Store, versionSecurityProfile);
						}
						break;
					case NodeType.NT_Function:
						FunctionSecurityProfile functionSecurityProfile = null;
						if (nodeTag.OwnerType == eSecurityOwnerType.Group)
						{
							functionSecurityProfile = _security.GetGroupFunctionAssignment(nodeTag.OwnerRID, (eSecurityFunctions)nodeTag.Item);
						}
						else
						{
							functionSecurityProfile = _security.GetUserFunctionAssignment(nodeTag.OwnerRID, (eSecurityFunctions)nodeTag.Item);
						}
						foreach (DataRow row in dtActions.Rows)
						{
							addPermission(Convert.ToString(row["TEXT_VALUE"], CultureInfo.CurrentUICulture), ref y, nodeTag.Type, (eSecurityActions)(Convert.ToInt32(row["ACTION_ID"], CultureInfo.CurrentUICulture)), functionID, eDatabaseSecurityTypes.NotSpecified, functionSecurityProfile);
						}
						break;
				}
					pnlPermissions.Visible = true;
					// set if read only
					if (nodeTag.OwnerType == eSecurityOwnerType.Group &&
						_groupSecurity.IsReadOnly)
					{
						SetControlReadOnly(pnlPermissions, true);
					}
					else if (nodeTag.OwnerType == eSecurityOwnerType.User &&
						_userSecurity.IsReadOnly)
					{
						SetControlReadOnly(pnlPermissions, true);
					}
					pnlPermissions.BringToFront();
					break;
				case NodeType.NT_Group:
					_currGroup = aNode;
					// Begin MID Track #345 do not allow user information be changed on group tab
				switch (GetSelectedTab())  // Issue 4684
				{
					case eTabSelected.Groups:
						SetControlReadOnly(pnlChangeGroup, false);
						btnChangeGroup.Visible = true;
						//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//						txtChangeGroupName.ReadOnly = true;
						//End Track #4815
						break;
					case eTabSelected.Users:
						SetControlReadOnly(pnlChangeGroup, true);
						btnChangeGroup.Visible = false;
						//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//						txtChangeGroupName.ReadOnly = true;
						//End Track #4815
						break;
				}
					// End MID Track #345

					string groupRID = nodeTag.Item.ToString(CultureInfo.CurrentUICulture);
					string filter = "GROUP_RID = " + "'" + groupRID + "'";

					DataRow[] drs = _dtGroups.Select(filter);
					DataRow dr = drs[0];
					txtChangeGroupName.Text = (string)dr["GROUP_NAME"];
					txtChangeGroupDesc.Text = ((string)dr["GROUP_DESCRIPTION"]).Trim();
					if ((string)dr["GROUP_ACTIVE_IND"] == "1")
					{
						rdoChangeGroupActive.Checked = true;
					}
					else
					{
						rdoChangeGroupInactive.Checked = true;
					}
					pnlChangeGroup.Visible = true;
					// set if read only
					if (_groupSecurity.IsReadOnly)
					{
						SetControlReadOnly(pnlChangeGroup, true);
					}
					else if (!_groupSecurity.AllowInactivate)
					{
						this.gbxChangeGroupStatus.Enabled = false;
					}
                    //BEGIN TT#4308-VStuart-Change the Administrator User Security Group so it cannot be deleted-MID
                    if (groupRID == "1")
                    {
                        this.pnlChangeGroup.Enabled = false;
                    }
                    else
                    {
                        this.pnlChangeGroup.Enabled = true;
                    }
                    //END TT#4308-VStuart-Change the Administrator User Security Group so it cannot be deleted-MID
					pnlChangeGroup.BringToFront();
					break;
				case NodeType.NT_User:
					_currUser = aNode;
					// Begin MID Track #345 do not allow group information be changed on user tab
				switch (GetSelectedTab())	// Issue 4684
				{
					case eTabSelected.Groups:
						SetControlReadOnly(pnlChangeUser, true);
						btnChangeUser.Visible = false;
						//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//						txtChangeUserID.ReadOnly = true;
						//End Track #4815
						break;
					case eTabSelected.Users:
						//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
						_changeUserPanelLoaded = false;
						//End Track #4815
						SetControlReadOnly(pnlChangeUser, false);
						btnChangeUser.Visible = true;
						//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//						txtChangeUserID.ReadOnly = true;
						//End Track #4815
						//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
						_assignedToChanged = false;
						if (_userSecurity.AllowAssign)
						{
							LoadUsers (((NodeTag)(aNode.Tag)).Item);
							cbxPermanentlyMove.Enabled = false;
						}
						else
						{
							gbxAssignTo.Visible = false;
						}
                        //BEGIN TT#4309-VStuart-Change administrator user security so it cannot be set to Inactive-MID
                        if (((NodeTag)(aNode.Tag)).OwnerRID == Include.AdministratorUserRID)
                        {
                            txtChangeUserID.Enabled = false;
                            txtChangeUserFullName.Enabled = false;
                            txtChangeUserDesc.Enabled = false;

                            txtChangeUserPassword.Enabled = true;
                            txtChangeUserPassConfirm.Enabled = true;
                            btnRemovePassword.Enabled = true;

                            gbxChangeUserStatus.Enabled = false;
                        }
                        else
                        {
                            txtChangeUserID.Enabled = true;
                            txtChangeUserFullName.Enabled = true;
                            txtChangeUserDesc.Enabled = true;
                            txtChangeUserPassword.Enabled = true;
                            txtChangeUserPassConfirm.Enabled = true;
                            btnRemovePassword.Enabled = true;
                            gbxChangeUserStatus.Enabled = true;
                        }
                        //END TT#4309-VStuart-Change administrator user security so it cannot be set to Inactive-MID
                        _changeUserPanelLoaded = true;
						//End Track #4815
						break;
				}
					// End MID Track #345

					string userRID = (((NodeTag)(aNode.Tag)).Item).ToString(CultureInfo.CurrentUICulture);
					filter = "USER_RID = " + "'" + userRID + "'";

					drs = _dtUsers.Select(filter);
					dr = drs[0];

					txtChangeUserID.Text = (string)dr["USER_NAME"];
					txtChangeUserFullName.Text = ((string)dr["USER_FULLNAME"]).Trim();
					txtChangeUserDesc.Text = ((string)dr["USER_DESCRIPTION"]).Trim();
					if ((string)dr["USER_ACTIVE_IND"] == "1")
					{
						rdoChangeUserActive.Checked = true;
					}
					else
					{
						rdoChangeUserInactive.Checked = true;
					}
					//					if (isInheritedTree)
					//						pnlChangeUser.Enabled = false;
					//					else
					pnlChangeUser.Enabled = true;
					pnlChangeUser.Visible = true;
					// set if read only
					// Only administrator can change administrator
					if (((NodeTag)(aNode.Tag)).Item == Include.AdministratorUserRID &&
						SAB.ClientServerSession.UserRID != Include.AdministratorUserRID)
					{
						SetControlReadOnly(pnlChangeUser, true);
					}
					else if (_userSecurity.IsReadOnly)
					{
						SetControlReadOnly(pnlChangeUser, true);
					}
					else if (!_userSecurity.AllowInactivate)
					{
						this.gbxChangeUserStatus.Enabled = false;
					}
		
					pnlChangeUser.BringToFront();
					break;
				case NodeType.NT_FunctionFolder:
					_currFunctionFolder = aNode;
                    // Begin Track #5481 - JSmith - Panel showing for folder
                    pnlPermissions.Visible = false;
                    // End Track #5481
					break;
				case NodeType.NT_GroupFolder:
					_currGroupFolder = aNode;
					_currUser = _currGroupFolder.Parent;
					UpdateGroupsPanel(_currGroupFolder);
					pnlAddGroup.Visible = true;
					// set if read only
					if (_userSecurity.IsReadOnly)
					{
						SetControlReadOnly(pnlAddGroup, true);
					}
					pnlAddGroup.BringToFront();
					break;
					//				case NodeType.NT_MLIDFolder:
					//					pnlAddMLID.Visible = true;
					//					pnlAddMLID.BringToFront();
					//					break;
				case NodeType.NT_UserFolder:
					_currUserFolder = aNode;
					_currGroup = _currUserFolder.Parent;
					UpdateUsersPanel(_currUserFolder);
					pnlAddUser.Visible = true;
					// set if read only
					if (_groupSecurity.IsReadOnly)
					{
						SetControlReadOnly(pnlAddUser, true);
					}
					pnlAddUser.BringToFront();
					break;
					//				case NodeType.NT_VersionFolder:
					//					_currVersionFolder = e.Node;
					//					break;
				default:
					pnlNewGroup.Visible = false;
					pnlNewUser.Visible = false;
					pnlChangeGroup.Visible = false;
					pnlChangeUser.Visible = false;
					pnlSearchGroup.Visible = false;
					pnlSearchUser.Visible = false;
					pnlAddGroup.Visible = false;
					pnlAddUser.Visible = false;
					pnlAddMLID.Visible = false;
					pnlAddView.Visible = false;
					pnlAddStore.Visible = false;
					pnlPermissions.Visible = false;
					break;
			}
		}

		private void addPermissionType(eDatabaseSecurityTypes aSecurityTypes, ref int y)
		{
			System.Windows.Forms.Label label = new Label();
			string permissionType = MIDText.GetTextOnly(Convert.ToInt32(aSecurityTypes, CultureInfo.CurrentCulture));
			if (permissionType != "")
			{
				label.Text = permissionType + ":";
				label.Height -= 5;
				label.Location = new Point(lblPermission.Left - pnlFunctionPermission.Left + 2,y) + (Size) AutoScrollPosition;
				pnlFunctionPermission.Controls.Add(label);

				y += label.Height;

			}
		}

		private void addPermission(string permissionName, ref int y, NodeType aNodeType, eSecurityActions aSecurityAction, int aSecurityFunction, eDatabaseSecurityTypes aSecurityType, SecurityProfile aSecurityProfile)
		{
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
			PermissionSet permSet;
			CheckBox allowCheckBox;
			CheckBox denyCheckBox;
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
			System.Windows.Forms.CheckBox checkbox;
			System.Windows.Forms.Label label = new Label();
			if (permissionName != "")
			{
				SecurityPermission permission = aSecurityProfile.GetSecurityPermission(aSecurityAction);
////Begin Track #3876 - JSmith - show deny instead of blank
//				if (aSecurityAction == eSecurityActions.FullControl)
//				{
//					_fullControlSecurity = permission.SecurityLevel;
//				}
////End Track #3876
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes

				permSet = (PermissionSet)_permissionSetHash[aSecurityType];

				if (permSet == null)
				{
					permSet = new PermissionSet();
					_permissionSetHash[aSecurityType] = permSet;
				}

//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				label.Text = permissionName;
				label.Height -= 5;
				// Begin Track #4961 - JSmith - Add security for apply to lower levels.  
                // Begin TT#2015 - JSmith - Apply Changes to Lower Level
                //label.Width = 150;
                label.Width = 160;
                // End TT#2015
				// End Track #4961
				label.Location = new Point(lblPermission.Left - pnlFunctionPermission.Left + 2,y) + (Size) AutoScrollPosition;
				pnlFunctionPermission.Controls.Add(label);

				checkbox = new CheckBox();
				checkbox.Height = label.Height;
				checkbox.Location = new Point(lblAllow.Left  - pnlFunctionPermission.Left + 10,y) + (Size) AutoScrollPosition;
				checkbox.Tag = new PermissionTag(aNodeType, aSecurityAction, aSecurityType, ePermissionType.Allow, aSecurityFunction, permission);
				checkbox.Width = 20;
				if (permission.SecurityLevel == eSecurityLevel.Allow)
				{
					if (permission.IsInherited)
					{
						checkbox.ThreeState = true;
						checkbox.CheckState = CheckState.Indeterminate;
						checkbox.MouseHover += new EventHandler(checkBox_MouseHover);
						checkbox.ContextMenu = this.cmPermissions;

					}
					else
					{
						checkbox.ThreeState = false;
						checkbox.CheckState = CheckState.Checked;
					}
				}
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//				checkbox.CheckStateChanged += new EventHandler(permissionChanged);

				if (aSecurityAction == eSecurityActions.FullControl)
				{
					checkbox.CheckStateChanged += new EventHandler(FullControlPermissionChanged);
				}
				else
				{
					checkbox.CheckStateChanged += new EventHandler(ActionPermissionChanged);
				}

//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				checkbox.MouseDown +=new MouseEventHandler(checkbox_MouseDown);
				checkbox.MouseUp +=new MouseEventHandler(checkbox_MouseUp);
				pnlFunctionPermission.Controls.Add(checkbox);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
				allowCheckBox = checkbox;

				if (aSecurityAction == eSecurityActions.FullControl)
				{
					permSet.FullControlAllowCheckBox = checkbox;
				}
				else
				{
					permSet.ActionAllowCheckBoxList.Add(checkbox);
				}
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes

				checkbox = new CheckBox();
				checkbox.Height = label.Height;
				checkbox.Location = new Point(lblDeny.Left  - pnlFunctionPermission.Left + 10,y) + (Size) AutoScrollPosition;
				checkbox.Tag = new PermissionTag(aNodeType, aSecurityAction, aSecurityType, ePermissionType.Deny, aSecurityFunction, permission);
				checkbox.Width = 20;
////Begin Track #3876 - JSmith - show deny instead of blank
//				if (aSecurityAction != eSecurityActions.FullControl &&
//					!permission.IsInherited &&
//					(_fullControlSecurity == eSecurityLevel.Initialize ||
//					_fullControlSecurity == eSecurityLevel.NotSpecified) &&
//					(permission.SecurityLevel == eSecurityLevel.Initialize ||
//					permission.SecurityLevel == eSecurityLevel.NotSpecified))
//				{
//					permission.SecurityLevel = eSecurityLevel.Deny;
//				}
////End Track #3876

				if (permission.SecurityLevel == eSecurityLevel.Deny)
				{
					if (permission.IsInherited)
					{
						checkbox.ThreeState = true;
						checkbox.CheckState = CheckState.Indeterminate;
						checkbox.MouseHover += new EventHandler(checkBox_MouseHover);
						checkbox.ContextMenu = this.cmPermissions;
					}
					else
					{
						checkbox.ThreeState = false;
						checkbox.CheckState = CheckState.Checked;
					}
				}
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//				checkbox.CheckStateChanged += new EventHandler(permissionChanged);

				if (aSecurityAction == eSecurityActions.FullControl)
				{
					checkbox.CheckStateChanged += new EventHandler(FullControlPermissionChanged);
				}
				else
				{
					checkbox.CheckStateChanged += new EventHandler(ActionPermissionChanged);
				}

				checkbox.MouseDown +=new MouseEventHandler(checkbox_MouseDown);
				checkbox.MouseUp +=new MouseEventHandler(checkbox_MouseUp);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				pnlFunctionPermission.Controls.Add(checkbox);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
				denyCheckBox = checkbox;

				if (aSecurityAction == eSecurityActions.FullControl)
				{
					permSet.FullControlDenyCheckBox = checkbox;
				}
				else
				{
					permSet.ActionDenyCheckBoxList.Add(checkbox);
				}

				((PermissionTag)allowCheckBox.Tag).OpposingCheckBox = denyCheckBox;
				((PermissionTag)denyCheckBox.Tag).OpposingCheckBox = allowCheckBox;
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes

				y += label.Height;

			}
		}

		private void checkbox_MouseDown(object sender, MouseEventArgs e)
		{
			// keep track of the checkbox that was clicked
			_currCheckBox = (System.Windows.Forms.CheckBox)sender;
		}

		private void checkbox_MouseUp(object sender, MouseEventArgs e)
		{
			//			Debug.WriteLine("mouseup");

		}

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//		void permissionChanged(object obj, EventArgs ea)
//		{
//			//			Debug.WriteLine("permissionChanged");
//			System.Windows.Forms.CheckBox changedCheckbox = (System.Windows.Forms.CheckBox) obj;
//			PermissionTag changedCheckBoxTag = (PermissionTag)changedCheckbox.Tag;
//			_permissionChanged = true;
//			ChangePending = true;
//			PermissionTag checkBoxTag;
//
//			// if value was inherited, override check state to checked 
//			if (changedCheckbox.ThreeState)
//			{
//				changedCheckbox.ThreeState = false;
//				// check to see if they checked other permission type before setting CheckState
//				foreach (Control control in pnlFunctionPermission.Controls)
//				{
//					if (control is CheckBox)
//					{
//						CheckBox checkBox = (CheckBox)control;
//						checkBoxTag = (PermissionTag)checkBox.Tag;
//						if (checkBoxTag.Action == changedCheckBoxTag.Action &&
//							checkBoxTag.SecurityType == changedCheckBoxTag.SecurityType &&
//							checkBoxTag.PermissionType != changedCheckBoxTag.PermissionType)
//						{
//							if (!checkBox.Checked)
//							{
//								changedCheckbox.CheckState = CheckState.Checked;
//							}
//						}
//					}
//				}
//				changedCheckbox.MouseHover -= new EventHandler(checkBox_MouseHover);
//				changedCheckbox.ContextMenu = null;
//				SecurityPermission permission = changedCheckBoxTag.Permission;
//				permission.IsInherited = false;
//			}
//			else
//			{
//				// uncheck access or deny for same action
//				foreach (Control control in pnlFunctionPermission.Controls)
//				{
//					if (control is CheckBox)
//					{
//						CheckBox checkBox = (CheckBox)control;
//						checkBoxTag = (PermissionTag)checkBox.Tag;
//						if (changedCheckbox.Checked)
//						{
//							// uncheck same action of other security type (allow/deny) if user checks action 
//							if (checkBoxTag.Action == changedCheckBoxTag.Action &&
//								checkBoxTag.SecurityType == changedCheckBoxTag.SecurityType &&
//								checkBoxTag.PermissionType != changedCheckBoxTag.PermissionType)
//							{
//								checkBox.Checked = false;
//							}
//								// check all other actions if user checks full control
//							else if (changedCheckBoxTag.Action == eSecurityActions.FullControl &&
//								checkBoxTag.SecurityType == changedCheckBoxTag.SecurityType &&
//								checkBoxTag.Action != changedCheckBoxTag.Action &&
//								checkBoxTag.PermissionType == changedCheckBoxTag.PermissionType)
//							{
//								checkBox.Checked = true;
//							}
//								// uncheck full control if user checks other action of other security type (allow/deny)
//							else if (checkBoxTag.Action == eSecurityActions.FullControl &&
//								checkBoxTag.SecurityType == changedCheckBoxTag.SecurityType &&
//								checkBoxTag.PermissionType != changedCheckBoxTag.PermissionType &&
//								//								changedCheckbox.Checked &&
//								!checkBoxTag.Permission.IsInherited)
//							{
//								checkBox.Checked = false;
//							}
//						}
//							// uncheck full control if user checks action of same security type (allow/deny)
//						else if (changedCheckBoxTag.Action != eSecurityActions.FullControl &&
//							checkBoxTag.Action == eSecurityActions.FullControl &&
//							checkBoxTag.SecurityType == changedCheckBoxTag.SecurityType &&
//							checkBoxTag.PermissionType == changedCheckBoxTag.PermissionType &&
//							!checkBoxTag.Permission.IsInherited)
//						{
//							checkBox.Checked = false;
//						}
//					}
//				}
//			}
//		}
		void FullControlPermissionChanged(object obj, EventArgs ea)
		{
			System.Windows.Forms.CheckBox changedCheckbox;
			PermissionTag changedCheckBoxTag;
			PermissionSet permSet;
			ArrayList actionCheckBoxList;
            //PermissionTag checkBoxTag;
            //CheckBox checkBox;
            //SecurityPermission permission;

			if (!_fullControlChanged && !_actionChanged)
			{
				_fullControlChanged = true;

				try
				{
					changedCheckbox = (System.Windows.Forms.CheckBox) obj;
					changedCheckBoxTag = (PermissionTag)changedCheckbox.Tag;
					permSet = (PermissionSet)_permissionSetHash[changedCheckBoxTag.SecurityType];

					if (changedCheckBoxTag.PermissionType == ePermissionType.Allow)
					{
						actionCheckBoxList = permSet.ActionAllowCheckBoxList;
					}
					else
					{
						actionCheckBoxList = permSet.ActionDenyCheckBoxList;
					}

					foreach (CheckBox actionCheckBox in actionCheckBoxList)
					{
						actionCheckBox.Checked = changedCheckbox.Checked;
					}

					if (changedCheckbox.Checked)
					{
						changedCheckBoxTag.OpposingCheckBox.Checked = false;
					}
				}
				catch (Exception)
				{
					throw;
				}
				finally
				{
					_fullControlChanged = false;
				}
			}
		}

		void ActionPermissionChanged(object obj, EventArgs ea)
		{
			System.Windows.Forms.CheckBox changedCheckbox;
			PermissionTag changedCheckBoxTag;
			PermissionSet permSet;
            //PermissionTag checkBoxTag;
            //CheckBox checkBox;
			SecurityPermission permission;

			if (!_actionChanged)
			{
				_actionChanged = true;

				try
				{
					changedCheckbox = (System.Windows.Forms.CheckBox) obj;
					changedCheckBoxTag = (PermissionTag)changedCheckbox.Tag;
					permSet = (PermissionSet)_permissionSetHash[changedCheckBoxTag.SecurityType];
					_permissionChanged = true;
					ChangePending = true;

					if (changedCheckbox.ThreeState)
					{
						changedCheckbox.ThreeState = false;
						changedCheckbox.MouseHover -= new EventHandler(checkBox_MouseHover);
						changedCheckbox.ContextMenu = null;
						permission = changedCheckBoxTag.Permission;
						permission.IsInherited = false;
					}
					else
					{
						if (changedCheckbox.Checked)
						{
							if (changedCheckBoxTag.OpposingCheckBox.ThreeState)
							{
								changedCheckBoxTag.OpposingCheckBox.ThreeState = false;
								changedCheckBoxTag.OpposingCheckBox.MouseHover -= new EventHandler(checkBox_MouseHover);
								changedCheckBoxTag.OpposingCheckBox.ContextMenu = null;
								permission = ((PermissionTag)changedCheckbox.Tag).Permission;
								permission.IsInherited = false;
							}

							changedCheckBoxTag.OpposingCheckBox.Checked = false;
						}
						else
						{
							if (!_fullControlChanged && !changedCheckbox.Checked)
							{
								if (changedCheckBoxTag.PermissionType == ePermissionType.Allow)
								{
									permSet.FullControlAllowCheckBox.Checked = false;
								}
								else
								{
									permSet.FullControlDenyCheckBox.Checked = false;
								}
							}
						}
					}
				}
				catch (Exception)
				{
					throw;
				}
				finally
				{
					_actionChanged = false;
				}
			}
		}
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes

		private void checkBox_MouseHover(object sender, System.EventArgs e)
		{
			if(ToolTip != null && ToolTip.Active) 
			{
				ToolTip.Active = false; //turn it off 
			}

			CheckBox checkBox = (CheckBox)sender;
			PermissionTag permissionTag = (PermissionTag)checkBox.Tag;
			SecurityPermission permission = permissionTag.Permission;

			if (permission.IsInherited)
			{
				DataRow [] rows;
				string message = "Inherited from " + MIDText.GetTextOnly(Convert.ToInt32(permission.OwnerType, CultureInfo.CurrentCulture)) + " "; 
				switch (permission.OwnerType)
				{
					case eSecurityOwnerType.Group:
						rows = _dtGroups.Select("GROUP_RID = " + permission.OwnerKey.ToString(CultureInfo.CurrentUICulture));
						if (rows != null)
						{
							DataRow dr = rows[0];
							message += Convert.ToString(dr["GROUP_NAME"], CultureInfo.CurrentUICulture);
						}
						else
						{
							message += "Unknown";
						}
						break;
					case eSecurityOwnerType.User:
						rows = _dtUsers.Select("USER_RID = " + permission.OwnerKey.ToString(CultureInfo.CurrentUICulture));
						if (rows != null)
						{
							DataRow dr = rows[0];
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

				switch (permission.SecurityInheritanceType)
				{
					case eSecurityInheritanceTypes.Function:
						rows = _dtSecurityFunctions.Select("FUNC_ID = " + permission.SecurityInheritedFrom.ToString(CultureInfo.CurrentUICulture));
						if (rows != null)
						{
							DataRow dr = rows[0];
							message += ":" + Convert.ToString(dr["TEXT_VALUE"], CultureInfo.CurrentUICulture);
						}
						else
						{
							message += ":Unknown";
						}
						break;
					case eSecurityInheritanceTypes.HierarchyNode:
						HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(permission.SecurityInheritedFrom);
						if (hnp != null)
						{
							message += ":" + hnp.Text;
						}
						else
						{
							message += ":Unknown";
						}
						break;
					case eSecurityInheritanceTypes.Version:
						break;
					default:
						message += ":Unknown";
						break;
				}
				
				ToolTip.Active = true; 
				ToolTip.SetToolTip((System.Windows.Forms.Control)sender, message);
			}
		}

		private void UpdateGroupsPanel(TreeNode currentGroupsFolder)
		{
			DataTable dt = _dtActiveGroups.Copy();
			foreach (TreeNode node in currentGroupsFolder.Nodes)
			{
				foreach (DataRow dr in dt.Rows)
				{
					if (Convert.ToInt32(dr["GROUP_RID"], CultureInfo.CurrentUICulture) == ((NodeTag)(node.Tag)).Item)
					{
						dt.Rows.Remove(dr);
						break;
					}
				}
			}
			lstAvailableGroups.DataSource = dt;
			lstAvailableGroups.DisplayMember = "GROUP_NAME";
			lstAvailableGroups.ValueMember = "GROUP_RID";
		}

		private void UpdateUsersPanel(TreeNode currentUsersFolder)
		{
			DataTable dt = _dtActiveUsers.Copy();
			foreach (TreeNode node in currentUsersFolder.Nodes)
			{
				foreach (DataRow dr in dt.Rows)
				{
					if (Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture) == ((NodeTag)(node.Tag)).Item)
					{
						dt.Rows.Remove(dr);
						break;
					}
				}
			}
			lstAvailableUsers.DataSource = dt;
			lstAvailableUsers.DisplayMember = "USER_NAME";
			lstAvailableUsers.ValueMember = "USER_RID";
		}


		private enum eTabSelected
		{
			Groups		= 0,
			Users		= 1,
			None		= 2				// Issue 4684
		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			BuildContextMenu();
			switch (GetSelectedTab())	// Issue 4684
			{
				case eTabSelected.Groups:
					// Begin MID Track# 2931 - pending changes issue
					CheckForSecurityChanges();
					// End MID Track# 2931
					if (_rebuildGroupPanel)	BuildGroupPanel();
					trvGroups.SelectedNode = null;
					break;
				case eTabSelected.Users:
					// Begin MID Track# 2931 - pending changes issue
					CheckForSecurityChanges();
					// End MID Track# 2931
					if (_rebuildUserPanel)	BuildUserPanel();
					trvUsers.SelectedNode = null;
					break;
			}
		
			pnlNewGroup.Visible = false;
			pnlNewUser.Visible = false;
			pnlChangeGroup.Visible = false;
			pnlChangeUser.Visible = false;
			pnlSearchGroup.Visible = false;
			pnlSearchUser.Visible = false;
			pnlAddGroup.Visible = false;
			pnlAddUser.Visible = false;
			pnlAddMLID.Visible = false;
			pnlAddView.Visible = false;
			pnlAddStore.Visible = false;
			pnlPermissions.Visible = false;
		}

		// Begin MID Track# 2931 - pending changes issue
		private void CheckForSecurityChanges()
		{
			try
			{
				if (_permissionChanged)
				{
					if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges),  "Save Changes",
						MessageBoxButtons.YesNo, MessageBoxIcon.Question)
						== DialogResult.Yes) 
					{
						SaveChanges();
						DisplayPermissions(_currNode);
						SetImage(_currNode);
					}
					else
					{
						_permissionChanged = false;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		// End MID Track# 2931
		
		private void BuildContextMenu()
		{
			switch (GetSelectedTab())	// Issue 4684
			{
				case eTabSelected.Groups:
					contextMenu1.MenuItems.Clear();
					if (_groupSecurity.AllowUpdate)
					{
						contextMenu1.MenuItems.Add("Create Group...", new System.EventHandler(NewGroup));
						contextMenu1.MenuItems.Add("Create Group Like...", new System.EventHandler(NewGroupLike));
						contextMenu1.MenuItems.Add("Remove User from Group...", new System.EventHandler(RemoveUserFromGroup));
						//					contextMenu1.MenuItems.Add("Add User to Group...", new System.EventHandler(AddUser));
						//					contextMenu1.MenuItems.Add("Add Permission...", new System.EventHandler(AddPerm));
						contextMenu1.MenuItems.Add("Remove Permission...", new System.EventHandler(RemovePerm));
						//					contextMenu1.MenuItems.Add("Search...", new System.EventHandler(SearchGroup));
					}
					//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
					if (_groupSecurity.AllowUpdate)
					{
						contextMenu1.MenuItems.Add(MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Delete), new System.EventHandler(DeleteGroup));
						contextMenu1.MenuItems.Add(MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Recover), new System.EventHandler(RecoverGroup));
					}
					//End Track #4815
					break;
				case eTabSelected.Users:
					contextMenu1.MenuItems.Clear();
					if (_userSecurity.AllowUpdate)
					{
						contextMenu1.MenuItems.Add("Create User...", new System.EventHandler(NewUser));
						contextMenu1.MenuItems.Add("Create User Like...", new System.EventHandler(NewUserLike));
						contextMenu1.MenuItems.Add("Remove Group from User...", new System.EventHandler(RemoveGroupFromUser));
						//					contextMenu1.MenuItems.Add("Add Group...", new System.EventHandler(AddGroup));
						//					contextMenu1.MenuItems.Add("Add Permission...", new System.EventHandler(AddPerm));
						contextMenu1.MenuItems.Add("Remove Permission...", new System.EventHandler(RemovePerm));
						//					contextMenu1.MenuItems.Add("Search...", new System.EventHandler(SearchUser));
					}
					//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
					if (_userSecurity.AllowDelete)
					{
						contextMenu1.MenuItems.Add(MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Delete), new System.EventHandler(DeleteUser));
						contextMenu1.MenuItems.Add(MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Recover), new System.EventHandler(RecoverUser));
					}
					if (_userSecurity.AllowAssign)
					{
						contextMenu1.MenuItems.Add(MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Assign), new System.EventHandler(AssignUser));
						contextMenu1.MenuItems.Add(MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Unassign), new System.EventHandler(UnassignUser));
					}
					//End Track #4815
					break;
				default:
					break;
			}
		}


		private void contextMenu1_Popup(object sender, System.EventArgs e)
		{
			int i;

			for (i = 0; i < contextMenu1.MenuItems.Count; i++)
			{
				contextMenu1.MenuItems[i].Enabled = false;
			}

//			if ((string)(((System.Windows.Forms.TreeView)(contextMenu1.SourceControl)).Tag) == "Groups")
			if (GetSelectedTab() == eTabSelected.Groups)
			{
				_currNode = trvGroups.GetNodeAt(_mouseDownX, _mouseDownY);
				if (_currNode == null) 
				{
					if (_groupSecurity.AllowUpdate)
					{
						contextMenu1.MenuItems[0].Enabled = true;
					}
					return;
				}

				switch (((NodeTag)(_currNode.Tag)).Type)
				{
					case NodeType.NT_Group:
						if (_groupSecurity.AllowUpdate)
						{
							contextMenu1.MenuItems[0].Enabled = true;
							contextMenu1.MenuItems[1].Enabled = true;
							//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
							if (_groupSecurity.AllowDelete)
							{
								if (((NodeTag)(_currNode.Tag)).ItemDeleted)
								{
									contextMenu1.MenuItems[5].Enabled = true;  // recover
								}
								else
								{
                                    //BEGIN TT#4308-VStuart-Change the Administrator User Security Group so it cannot be deleted-MID
                                    if ((((NodeTag)(_currNode.Tag))).OwnerRID != 1)
                                    {
                                        contextMenu1.MenuItems[4].Enabled = true;  // delete
                                    }
                                    //END TT#4308-VStuart-Change the Administrator User Security Group so it cannot be deleted-MID
                                }
							}
							//End Track #4815
						}
						//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
						else if (_groupSecurity.AllowDelete)
						{
							if (((NodeTag)(_currNode.Tag)).ItemDeleted)
							{
								contextMenu1.MenuItems[1].Enabled = true;  // recover
							}
							else
							{
								contextMenu1.MenuItems[0].Enabled = true;  // delete
							}
						}
						//End Track #4815
						break;
					case NodeType.NT_GroupFolder:
						if (_groupSecurity.AllowUpdate)
						{
							contextMenu1.MenuItems[0].Enabled = true;
						}
						break;
					case NodeType.NT_User:
						if (_groupSecurity.AllowUpdate)
						{
							contextMenu1.MenuItems[2].Enabled = true;
						}
						break;
					case NodeType.NT_UserFolder:
						break;
					case NodeType.NT_MLID:
						if (((NodeTag)(_currNode.Tag)).OwnerType == eSecurityOwnerType.Group &&
							_groupSecurity.AllowUpdate)
						{
							contextMenu1.MenuItems[3].Enabled = true;
						}
						break;
					case NodeType.NT_Version:
					case NodeType.NT_Function:
						break;
					case NodeType.NT_MLIDFolder:
					case NodeType.NT_VersionFolder:
					case NodeType.NT_FunctionFolder:
						break;
				}
			}
			else
			{
				_currNode = trvUsers.GetNodeAt(_mouseDownX, _mouseDownY);
				if (_currNode == null) 
				{
					if (_userSecurity.AllowUpdate)
					{
						contextMenu1.MenuItems[0].Enabled = true;
					}
					return;
				}

				switch (((NodeTag)(_currNode.Tag)).Type)
				{
					case NodeType.NT_User:
						if (_userSecurity.AllowUpdate)
						{
							contextMenu1.MenuItems[0].Enabled = true;
							if (((NodeTag)(_currNode.Tag)).Item == Include.AdministratorUserRID)
							{
								contextMenu1.MenuItems[1].Enabled = false;
							}
							else
							{
								contextMenu1.MenuItems[1].Enabled = true;
								//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
								if (_userSecurity.AllowDelete &&
									((NodeTag)(_currNode.Tag)).Item != SAB.ClientServerSession.UserRID)  // can't delete yourself
								{
									if (((NodeTag)(_currNode.Tag)).ItemDeleted)
									{
										contextMenu1.MenuItems[5].Enabled = true;  // recover
									}
									else
									{
										contextMenu1.MenuItems[4].Enabled = true;  // delete
									}
								}
								if (_userSecurity.AllowAssign)
								{
									if (_userSecurity.AllowDelete)
									{
										if (GetAssignedToUser(((NodeTag)(_currNode.Tag)).Item) == Include.NoRID)
										{
											// allow to be assign if user does not have another user assigned to them
											if (GetUsersAssignedToMe(((NodeTag)(_currNode.Tag)).Item)== Include.NoRID)
											{
												contextMenu1.MenuItems[6].Enabled = true;  // assign
											}
										}
										else
										{
											contextMenu1.MenuItems[7].Enabled = true;  // unassign
										}
									}
									else
									{
										if (GetAssignedToUser(((NodeTag)(_currNode.Tag)).Item) == Include.NoRID)
										{
											// allow to be assign if user does not have another user assigned to them
											if (GetUsersAssignedToMe(((NodeTag)(_currNode.Tag)).Item)== Include.NoRID)
											{
												contextMenu1.MenuItems[4].Enabled = true;  // assign
											}
										}
										else
										{
											contextMenu1.MenuItems[5].Enabled = true;  // unassign
										}
									}
								}
								//End Track #4815
							}
						}
						//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
						else if (_userSecurity.AllowDelete &&
							(((NodeTag)(_currNode.Tag)).Item != Include.AdministratorUserRID &&  // can't delete administrator
							((NodeTag)(_currNode.Tag)).Item != SAB.ClientServerSession.UserRID))  // can't delete yourself
						{
							if (((NodeTag)(_currNode.Tag)).ItemDeleted)
							{
								contextMenu1.MenuItems[1].Enabled = true;  // delete
							}
							else
							{
								contextMenu1.MenuItems[0].Enabled = true;  // recover
							}
							if (_userSecurity.AllowAssign)
							{
								if (GetAssignedToUser(((NodeTag)(_currNode.Tag)).Item) == Include.NoRID)
								{
									// allow to be assign if user does not have another user assigned to them
									if (GetUsersAssignedToMe(((NodeTag)(_currNode.Tag)).Item)== Include.NoRID)
									{
										contextMenu1.MenuItems[2].Enabled = true;  // assign
									}
								}
								else
								{
									contextMenu1.MenuItems[3].Enabled = true;  // unassign
								}
							}
						}
						else if (_userSecurity.AllowAssign)
						{
							if (GetAssignedToUser(((NodeTag)(_currNode.Tag)).Item) == Include.NoRID)
							{
								// allow to be assign if user does not have another user assigned to them
								if (GetUsersAssignedToMe(((NodeTag)(_currNode.Tag)).Item)== Include.NoRID)
								{
									contextMenu1.MenuItems[0].Enabled = true;  // assign
								}
							}
							else
							{
								contextMenu1.MenuItems[1].Enabled = true;  // unassign
							}
						}
						//End Track #4815
						break;
					case NodeType.NT_UserFolder:
						if (_userSecurity.AllowUpdate)
						{
							contextMenu1.MenuItems[0].Enabled = true;
						}
						break;
					case NodeType.NT_Group:
						if (_userSecurity.AllowUpdate)
						{
							contextMenu1.MenuItems[2].Enabled = true;
						}
						break;
					case NodeType.NT_GroupFolder:
						break;
					case NodeType.NT_MLID:
						if (((NodeTag)(_currNode.Tag)).InheritedType == eSecurityOwnerType.User &&
							_userSecurity.AllowUpdate)
						{
							contextMenu1.MenuItems[3].Enabled = true;
						}
						break;
					case NodeType.NT_Version:
					case NodeType.NT_Function:
						break;
					case NodeType.NT_MLIDFolder:
					case NodeType.NT_VersionFolder:
					case NodeType.NT_FunctionFolder:
						break;
				}
			}

			for (i = 0; i < contextMenu1.MenuItems.Count; i++)
			{
				if (contextMenu1.MenuItems[i].Enabled)
				{
					contextMenu1.MenuItems[i].Visible = true;
				}
				else
				{
					contextMenu1.MenuItems[i].Visible = false;
				}
			}
		}

		private void NewGroup(object sender, System.EventArgs e)
		{
			_createGroupLike = null;
			string buttonLabel = "Create";
			btnNewGroup.Text = buttonLabel;
			btnNewGroup.Width = 30 + buttonLabel.Length * 5;
			pnlNewGroup.Visible = true;
			pnlNewGroup.BringToFront();
		}

		private void NewGroupLike(object sender, System.EventArgs e)
		{
			_createGroupLike = _currNode;
			string buttonLabel = "Create like " + _currNode.Text;
			btnNewGroup.Text = buttonLabel;
			btnNewGroup.Width = 30 + buttonLabel.Length * 5;
			pnlNewGroup.Visible = true;
			pnlNewGroup.BringToFront();
		}

		private void NewUser(object sender, System.EventArgs e)
		{
			_createUserLike = null;
			string buttonLabel = "Create";
			btnNewUser.Text = buttonLabel;
			btnNewUser.Width = 30 + buttonLabel.Length * 5;
			pnlNewUser.Visible = true;
			pnlNewUser.BringToFront();
		}

		private void NewUserLike(object sender, System.EventArgs e)
		{
			_createUserLike = _currNode;
			string buttonLabel = "Create like " + _currNode.Text;
			btnNewUser.Text = buttonLabel;
			btnNewUser.Width = 30 + buttonLabel.Length * 5;
			pnlNewUser.Visible = true;
			pnlNewUser.BringToFront();
		}

		private void AddGroup(object sender, System.EventArgs e)
		{
			pnlAddGroup.Visible = true;
			pnlAddGroup.BringToFront();
		}
		
		private void AddUser(object sender, System.EventArgs e)
		{
			pnlAddUser.Visible = true;
			pnlAddUser.BringToFront();
		}
		
		private void RemoveUserFromGroup(object sender, System.EventArgs e)
		{
			int userRID = ((NodeTag)(_currNode.Tag)).Item;
			int groupRID = ((NodeTag)(_currNode.Parent.Parent.Tag)).Item;

			try	// remove user from group
			{
				_securityData.OpenUpdateConnection();
				_securityData.RemoveUserFromGroup(userRID, groupRID);
				_securityData.CommitData();
				SAB.ClientServerSession.RefreshSecurity();
			}
			catch (Exception error)
			{
				MessageBox.Show(error.Message);
				return;
			}
			finally
			{
				_securityData.CloseUpdateConnection();
			}

			// remove user from the tree
			TreeNode parentNode = _currNode.Parent;
			if (parentNode.Nodes.Count == 1)
				parentNode.Collapse();
			parentNode.Nodes.Remove(_currNode);
			UpdateUsersPanel(parentNode);
			_rebuildUserPanel = true;
//			_rebuildInheritedPanel = true;
			SAB.ClientServerSession.RefreshSecurity();
			// if last item removed, redisplay selection
			if (parentNode.Nodes.Count == 0)
			{
				trvGroups.SelectedNode = null;
				trvGroups.SelectedNode = parentNode;
			}
		}


		private void RemoveGroupFromUser(object sender, System.EventArgs e)
		{
			int groupRID = ((NodeTag)(_currNode.Tag)).Item;
			int userRID = ((NodeTag)(_currNode.Parent.Parent.Tag)).Item;

			try	// remove user from group
			{
				_securityData.OpenUpdateConnection();
				_securityData.RemoveUserFromGroup(userRID, groupRID);
				_securityData.CommitData();
				SAB.ClientServerSession.RefreshSecurity();
			}
			catch (Exception error)
			{
				MessageBox.Show(error.Message);
				return;
			}
			finally
			{
				_securityData.CloseUpdateConnection();
			}

			// remove group from the tree
			TreeNode parentNode = _currNode.Parent;
			if (parentNode.Nodes.Count == 1)
				parentNode.Collapse();
			parentNode.Nodes.Remove(_currNode);
			UpdateGroupsPanel(parentNode);
//			_rebuildInheritedPanel = true;
			_rebuildGroupPanel = true;
			SAB.ClientServerSession.RefreshSecurity();
			// if last item removed, redisplay selection
			if (parentNode.Nodes.Count == 0)
			{
				trvUsers.SelectedNode = null;
				trvUsers.SelectedNode = parentNode;
			}
		}

		private void RemovePerm(object sender, System.EventArgs e)
		{
//Begin Track #3875 - JSmith - remove permission error
			_removingPermission = true;
//End Track #3875
			bool rootIsGroup = (((NodeTag)_currNode.Parent.Parent.Tag).Type == NodeType.NT_Group);
			int nodeRID = ((NodeTag)(_currNode.Tag)).Item;
			int rootRID = ((NodeTag)(_currNode.Parent.Parent.Tag)).Item;
			NodeType nodeType = ((NodeTag)_currNode.Tag).Type;

			try	
			{
				_securityData.OpenUpdateConnection();
				switch (nodeType)
				{
					case NodeType.NT_MLID:
						if (rootIsGroup)
							_securityData.RemoveGroupNode(rootRID, nodeRID);
						else
							_securityData.RemoveUserNode(rootRID, nodeRID);
						break;
					case NodeType.NT_Version:
						if (rootIsGroup)
							_securityData.RemoveGroupVersion(rootRID, nodeRID);
						else
							_securityData.RemoveUserVersion(rootRID, nodeRID);
						break;
					case NodeType.NT_Function:
						if (rootIsGroup)
							_securityData.RemoveGroupFunction(rootRID, (eSecurityFunctions)nodeRID);
						else
							_securityData.RemoveUserFunction(rootRID, (eSecurityFunctions)nodeRID);
						break;
					default:
						break;
				}
				_securityData.CommitData();
			}
			catch (Exception error)
			{
				MessageBox.Show(error.Message);
				return;
			}
			finally
			{
				_securityData.CloseUpdateConnection();
			}

			// remove node from the tree
			TreeNode parentNode = _currNode.Parent;
			if (parentNode.Nodes.Count == 1)
			{
				parentNode.Collapse();
			}

			parentNode.Nodes.Remove(_currNode);
//			switch (nodeType)
//			{
//				case NodeType.NT_Version:
//					UpdateVersionsPanel(parentNode);
//					break;
//				case NodeType.NT_Function:
//					UpdateFunctionsPanel(parentNode);
//					break;
//				case NodeType.NT_MLID:
//				default:
//					break;
//			}
//			_rebuildInheritedPanel = true;
			SAB.ClientServerSession.RefreshSecurity();
			// if last item removed, redisplay selection
			if (parentNode.Nodes.Count == 0)
			{
				if (rootIsGroup)
				{
					trvGroups.SelectedNode = null;
					trvGroups.SelectedNode = parentNode;
				}
				else
				{
					trvUsers.SelectedNode = null;
					trvUsers.SelectedNode = parentNode;
				}
			}
//Begin Track #3875 - JSmith - remove permission error
			_removingPermission = false;
//End Track #3875
		}

		private void AddPerm(object sender, System.EventArgs e)
		{
			switch (((NodeTag)(_currNode.Tag)).Type)
			{
				case NodeType.NT_MLID:
				case NodeType.NT_MLIDFolder:
					// make sure Merchandise Explorer is front and center
					break;
				case NodeType.NT_Version:
				case NodeType.NT_VersionFolder:
					break;
				case NodeType.NT_Function:
				case NodeType.NT_FunctionFolder:
					break;
				default:
					break;
			}
		}

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance

		private void LoadUsers(int aUserRID)
		{
			DataTable dtUsers;
			int userRID, assignedUserRID, assignedUserIndex = Include.Undefined, assignedToMeUserRID;
			int index = 0;

			assignedUserRID = GetAssignedToUser(aUserRID);
			assignedToMeUserRID = GetUsersAssignedToMe(aUserRID);
			
			dtUsers = _securityData.GetUsers();
			dtUsers.DefaultView.Sort = "USER_NAME ASC"; 
			dtUsers.AcceptChanges();
			
			cboAssignToUser.Items.Clear();
			if (aUserRID < 100 ||
				assignedToMeUserRID != Include.NoRID)
			{
				this.gbxAssignTo.Enabled = false;
			}
			else
			{
				this.gbxAssignTo.Enabled = true;
				cboAssignToUser.Items.Add(new ComboObject(Include.NoRID, MIDText.GetTextOnly(eMIDTextCode.lbl_None)));
				foreach (DataRow dr in dtUsers.Rows)
				{
					// do not show system user or user to assign
					userRID = Convert.ToInt32(dr["USER_RID"]);
					if (userRID == Include.UndefinedUserRID ||
						userRID == Include.AdministratorUserRID ||
						userRID == Include.SystemUserRID ||
						userRID == Include.GlobalUserRID ||
						userRID == aUserRID)
					{
						continue;
					}
					// do not show deleted or inactive user
					if (!Include.ConvertCharToBool(Convert.ToChar(dr["USER_ACTIVE_IND"])) ||
						Include.ConvertCharToBool(Convert.ToChar(dr["USER_DELETE_IND"])))
					{
						continue;
					}
					cboAssignToUser.Items.Add(new ComboObject(Convert.ToInt32(dr["USER_RID"]), Convert.ToString(dr["USER_NAME"])));
					++index;
					if (userRID == assignedUserRID)
					{
						assignedUserIndex = index;
					}
				}

				dtUsers = _securityData.GetAssignedToUsers(aUserRID);
				if (assignedUserIndex != Include.Undefined)
				{
					cboAssignToUser.SelectedIndex = assignedUserIndex;
				}
			}
		}

		private void SetAssignedUserComboBox(int aUserRID)
		{
			int assignedUserRID;
			int index = Include.NoRID;

			assignedUserRID = GetAssignedToUser(aUserRID);
			foreach (ComboObject co in cboAssignToUser.Items)
			{
				++index;
				if (co.Key == assignedUserRID)
				{
					break;
				}
			}
			if (index > 0)
			{
				cboAssignToUser.SelectedIndex = index;
			}
		}

		private int GetUsersAssignedToMe(int aUserRID)
		{
			DataTable dtUsers;
			DataRow drUsers;
			int assignedUserRID = Include.NoRID;

			dtUsers = _securityData.GetUsersAssignedToMe(aUserRID);
			if (dtUsers.Rows.Count > 0)
			{
				drUsers = dtUsers.Rows[0];
				assignedUserRID = Convert.ToInt32(drUsers["OWNER_USER_RID"]);
			}

			return assignedUserRID;
		}

		private int GetAssignedToUser(int aUserRID)
		{
			DataTable dtUsers;
			DataRow drUsers;
			int assignedUserRID = Include.NoRID;

			dtUsers = _securityData.GetAssignedToUsers(aUserRID);
			if (dtUsers.Rows.Count > 0)
			{
				drUsers = dtUsers.Rows[0];
				assignedUserRID = Convert.ToInt32(drUsers["USER_RID"]);
			}

			return assignedUserRID;
		}

		private void DeleteGroup(object sender, System.EventArgs e)
		{
			string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDelete);
			message = message.Replace("{0}", _currNode.Text);
			if (MessageBox.Show (message,  this.Text,
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
				== DialogResult.Yes)
			{
				try	
				{
					_securityData.OpenUpdateConnection();
					_securityData.MarkGroupForDelete(((NodeTag)_currNode.Tag).Item);
					_securityData.CommitData();
					_currNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteImage);
					_currNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteImage);
					_currNode.ForeColor = System.Drawing.Color.SlateBlue;
					((NodeTag)_currNode.Tag).ItemDeleted = true;
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ToBeDeletedDuringPurge), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (Exception error)
				{
					MessageBox.Show(error.Message);
					return;
				}
				finally
				{
					_securityData.CloseUpdateConnection();
				}
			}
		}

		private void RecoverGroup(object sender, System.EventArgs e)
		{
			string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmRecover);
			message = message.Replace("{0}", _currNode.Text);
			if (MessageBox.Show (message,  this.Text,
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
				== DialogResult.Yes)
			{
				try	
				{
					_securityData.OpenUpdateConnection();
					_securityData.RecoverDeletedGroup(((NodeTag)_currNode.Tag).Item);
					_securityData.CommitData();
					_currNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
					_currNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
					_currNode.ForeColor = System.Drawing.Color.Black;
					((NodeTag)_currNode.Tag).ItemDeleted = false;
				}
				catch (Exception error)
				{
					MessageBox.Show(error.Message);
					return;
				}
				finally
				{
					_securityData.CloseUpdateConnection();
				}
			}
		}

		private void DeleteUser(object sender, System.EventArgs e)
		{
			string message;
			int assignedUserRID;

			assignedUserRID = GetAssignedToUser(((NodeTag)_currNode.Tag).Item);
			if (assignedUserRID == Include.NoRID)
			{
				message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDelete);
			}
			else
			{
				message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteAssignedWarning);
			}
			message = message.Replace("{0}", _currNode.Text);
			if (MessageBox.Show (message,  this.Text,
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
				== DialogResult.Yes)
			{
				try	
				{
					_securityData.OpenUpdateConnection();
					_securityData.MarkUserForDelete(((NodeTag)_currNode.Tag).Item);
					_securityData.CommitData();
					_currNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteImage);
					_currNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.DeleteImage);
					_currNode.ForeColor = System.Drawing.Color.SlateBlue;
					((NodeTag)_currNode.Tag).ItemDeleted = true;
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ToBeDeletedDuringPurge), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (Exception error)
				{
					MessageBox.Show(error.Message);
					return;
				}
				finally
				{
					_securityData.CloseUpdateConnection();
				}
			}
		}

		private void RecoverUser(object sender, System.EventArgs e)
		{
			string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmRecover);
			message = message.Replace("{0}", _currNode.Text);
			if (MessageBox.Show (message,  this.Text,
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
				== DialogResult.Yes)
			{
				try	
				{
					_securityData.OpenUpdateConnection();
					_securityData.RecoverDeletedUser(((NodeTag)_currNode.Tag).Item);
					_securityData.CommitData();
					_currNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
					_currNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
					_currNode.ForeColor = System.Drawing.Color.Black;
					((NodeTag)_currNode.Tag).ItemDeleted = false;
				}
				catch (Exception error)
				{
					MessageBox.Show(error.Message);
					return;
				}
				finally
				{
					_securityData.CloseUpdateConnection();
				}
			}
		}

		private void AssignUser(object sender, System.EventArgs e)
		{
			frmAssignUser form;
			string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmAssign);
			message = message.Replace("{0}", _currNode.Text);
			if (MessageBox.Show (message,  this.Text,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				== DialogResult.Yes)
			{
				try	
				{
					form = new frmAssignUser(SAB, ((NodeTag)_currNode.Tag).Item);
					form.StartPosition = FormStartPosition.CenterScreen;
					form.ShowDialog();
					if (form.CancelPressed)
					{
						return;
					}
					if (form.PermanentMove)
					{
						message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PermanentMoveWarning);
						message = message.Replace("{0}", _currNode.Text);
						message = message.Replace("{1}", form.SelectedUserName);
						if (MessageBox.Show (message,  this.Text,
							MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
							== DialogResult.No)
						{
							return;
						}
					}
					AssignUser(((NodeTag)_currNode.Tag).Item, form.SelectedUserRID, form.PermanentMove);
					_currNode.ForeColor = System.Drawing.Color.SlateBlue;

					if (form.PermanentMove)
					{
						message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PermanentMoveConfirmation);
					}
					else
					{
						message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AssignConfirmation);
					}
					message = message.Replace("{0}", _currNode.Text);
					message = message.Replace("{1}", form.SelectedUserName);
					MessageBox.Show(message,  this.Text, MessageBoxButtons.OK);
					// if the user being assigned is the current user being displayed
					// set the assigned user
					if (_currUser != null &&
						((NodeTag)_currNode.Tag).Item == ((NodeTag)_currUser.Tag).Item)
					{
						SetAssignedUserComboBox(((NodeTag)_currNode.Tag).Item);
					}
				}
				catch (Exception error)
				{
					MessageBox.Show(error.Message);
					return;
				}
			}
		}

		private void AssignUser (int aUserRID, int aAssignUser, bool aPermanentMove)
		{
			DataTable dtUserItems, dtFolder, dtUserNodeAssignment, dtAssignUserNodeAssignment;
			DataRow drFolder;
			DataRow[] nodeAssignmentList;
			int userRID, ownerUserRID, itemType, itemRID, nodeRID;
			FolderDataLayer folderDataLayer;
			//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			//eFolderType folderType;
            //eProfileType folderType;
			//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			eSecurityActions userSecurityAction;
			eDatabaseSecurityTypes userSecurityType;
			eSecurityLevel userSecurityLevel, assignUserSecurityLevel;
            // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over
            string userName;
            ArrayList alHierarchyKeys, alStoreGroupKeys, alFilterKeys, alTasklistKeys, alWorkflowKeys, alMethodKeys;
            // End Track #6302
            HierarchyProfileList hierarchyList;
            HierarchyProfile hierarchyProf;

			try	
			{
                // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over
                userName = " (" + SAB.ClientServerSession.GetUserName(aUserRID) + ")";
                alHierarchyKeys = new ArrayList();
                alStoreGroupKeys = new ArrayList();
                alFilterKeys = new ArrayList();
                alTasklistKeys = new ArrayList();
                alWorkflowKeys = new ArrayList();
                alMethodKeys = new ArrayList();
                if (aPermanentMove)
                {
                    foreach (int type in Enum.GetValues(typeof(eMainUserProfileType)))
                    {
                        ChangeParent(aUserRID, aAssignUser, type);
                    }
                }
                // End Track #6302

				_securityData.OpenUpdateConnection();
				// process user items
				folderDataLayer = new FolderDataLayer();
				dtUserItems = _securityData.GetUserItems(aUserRID);
				if (dtUserItems.Rows.Count > 0)
				{
					foreach (DataRow dr in dtUserItems.Rows)
					{
						userRID = Convert.ToInt32(dr["USER_RID"]); 
						itemType = Convert.ToInt32(dr["ITEM_TYPE"]); 
						itemRID = Convert.ToInt32(dr["ITEM_RID"]);
						ownerUserRID = Convert.ToInt32(dr["OWNER_USER_RID"]);
                        // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over
                        if (aPermanentMove &&
                            Enum.IsDefined(typeof(eDoNotAssignProfileTypes), itemType))
                        {
                            continue;
                        }
                        // End Track #6302

                        // Begin TT#126 - JSmith - Temporary assign shows empty shared folders
                        if (Enum.IsDefined(typeof(eMainUserProfileType), itemType))
                        {
                            if (!HasUserItems(userRID, (eMainUserProfileType)itemType))
                            {
                                continue;
                            }
                        }
                        // End TT#126

						// do not share master folders
						//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
						//if (itemType == Convert.ToInt32(eSharedDataType.AllFilters) ||
						//    itemType == Convert.ToInt32(eSharedDataType.AllHierarchies) ||
						//    itemType == Convert.ToInt32(eSharedDataType.AllMethods) ||
						//    itemType == Convert.ToInt32(eSharedDataType.AllPlanViews) ||
						//    itemType == Convert.ToInt32(eSharedDataType.AllTasklists) ||
						//    itemType == Convert.ToInt32(eSharedDataType.AllWorkflows))
						//{
						//    continue;
						//}
						//if (itemType == Convert.ToInt32(eSharedDataType.FilterFolder))
						//{
						//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                        //dtFolder = folderDataLayer.Folder_Read(itemRID);
                        //if (dtFolder.Rows.Count != 1)
                        //{
                        //    continue;
                        //}
                        //drFolder = dtFolder.Rows[0];
						//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
						//folderType = (eFolderType)(Convert.ToInt32(drFolder["FOLDER_TYPE"]));
						//if (folderType != eFolderType.FilterUser)
                        //folderType = (eProfileType)(Convert.ToInt32(drFolder["FOLDER_TYPE"]));
                        //if (folderType != eProfileType.FilterSubFolder)
                        //if (itemType != Convert.ToInt32(eProfileType.FilterMainUserFolder) &&
                        //    itemType != Convert.ToInt32(eProfileType.StoreGroupMainUserFolder) &&
                        //    itemType != Convert.ToInt32(eProfileType.TaskListTaskListMainUserFolder) &&
                        //    itemType != Convert.ToInt32(eProfileType.WorkflowMethodMainUserFolder))
                        ////End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                        //{
                        //    continue;
                        //}
						//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
						//}
						//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
						if (aPermanentMove)
						{
							// delete item from original user
							_securityData.DeleteUserItem(aUserRID, itemType, itemRID, ownerUserRID);
							// add item to assign user with assign user as owner
							_securityData.AddUserItem(aAssignUser, itemType, itemRID, aAssignUser);
                            // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over
                            switch ((eProfileType)itemType)
                            {
                                case eProfileType.StoreGroup:
                                    alStoreGroupKeys.Add(itemRID);
                                    break;
                                case eProfileType.Hierarchy:
                                    alHierarchyKeys.Add(itemRID);
                                    break;
                                case eProfileType.FilterStore:
                                    alFilterKeys.Add(itemRID);
                                    break;
                                case eProfileType.TaskList:
                                    alTasklistKeys.Add(itemRID);
                                    break;
                                case eProfileType.Workflow:
                                    alWorkflowKeys.Add(itemRID);
                                    break;
                                default:
                                    if (Enum.IsDefined(typeof(eMethodProfileType), itemType))
                                    {
                                        alMethodKeys.Add(itemRID);
                                    }
                                    break;
                            }
                            // End Track #6302
						}
						else
						{
                            // delete item from original user
                            _securityData.DeleteUserItem(aUserRID, itemType, itemRID, ownerUserRID);
							// make sure item does not exist for assign user
							_securityData.DeleteUserItem(aAssignUser, itemType, itemRID, ownerUserRID);
							// add item to assign user with original user as owner
							_securityData.AddUserItem(aAssignUser, itemType, itemRID, aUserRID);
						}
					}
				}
				// process user hierarchy security
				dtAssignUserNodeAssignment = _securityData.GetUserNodesAssignment(aAssignUser);
				dtUserNodeAssignment = _securityData.GetUserNodesAssignment(aUserRID);
				foreach (DataRow dr in dtUserNodeAssignment.Rows)
				{
					nodeRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
					userSecurityAction = (eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture);
					userSecurityType = (eDatabaseSecurityTypes)Convert.ToInt32(dr["SEC_TYPE"], CultureInfo.CurrentUICulture);
					userSecurityLevel = (eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture);

					nodeAssignmentList = dtAssignUserNodeAssignment.Select("HN_RID = " + nodeRID + " AND ACTION_ID = " + Convert.ToInt32(userSecurityAction)+ " AND SEC_TYPE = " + Convert.ToInt32(userSecurityType));
					if (nodeAssignmentList.Length > 0)
					{
						assignUserSecurityLevel = (eSecurityLevel)(Convert.ToInt32(nodeAssignmentList[0]["SEC_LVL_ID"], CultureInfo.CurrentUICulture));
						if (assignUserSecurityLevel == eSecurityLevel.Deny &&
							userSecurityLevel == eSecurityLevel.Allow)
						{
							_securityData.AssignUserNode(aAssignUser, nodeRID, userSecurityAction, userSecurityType, userSecurityLevel);
						}
					}
					else
					{
						_securityData.AssignUserNode(aAssignUser, nodeRID, userSecurityAction, userSecurityType, userSecurityLevel);
					}
				}

                // process user
				_securityData.MarkUserInactive(aUserRID);
				rdoChangeUserInactive.Checked = true;
				_securityData.CommitData();

                // make sure user has security 
                hierarchyList = SAB.HierarchyServerSession.GetHierarchiesForUser(aUserRID);
                dtAssignUserNodeAssignment = _securityData.GetUserNodesAssignment(aAssignUser);
                foreach (HierarchyProfile hierProf in hierarchyList)
                {
                    nodeAssignmentList = dtAssignUserNodeAssignment.Select("HN_RID = " + hierProf.HierarchyRootNodeRID);
                    if (nodeAssignmentList.Length == 0)
                    {
                        _securityData.AssignUserNode(aAssignUser, hierProf.HierarchyRootNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Chain, eSecurityLevel.Allow);
                        _securityData.AssignUserNode(aAssignUser, hierProf.HierarchyRootNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Store, eSecurityLevel.Allow);
                        _securityData.AssignUserNode(aAssignUser, hierProf.HierarchyRootNodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Allocation, eSecurityLevel.Allow);
                    }
                }
                _securityData.CommitData();

				_rebuildUserHierarchies = true;
			}
			catch (Exception error)
			{
				MessageBox.Show(error.Message);
				return;
			}
			finally
			{
				_securityData.CloseUpdateConnection();
			}

            // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over
            if (aPermanentMove)
            {
                MoveStoreGroups(alStoreGroupKeys, aUserRID, aAssignUser, userName);
                MoveStoreFilters(alFilterKeys, aUserRID, aAssignUser, userName);
                MoveTasklists(alTasklistKeys, aUserRID, aAssignUser, userName);
                MoveWorkflows(alWorkflowKeys, aUserRID, aAssignUser, userName);
                MoveMethods(alMethodKeys, aUserRID, aAssignUser, userName);
                MoveHierarchies(alHierarchyKeys, aUserRID, aAssignUser, userName);  
            }
            // End Track #6302
	    }

        // Begin TT#126 - JSmith - Temporary assign shows empty shared folders
        private bool HasUserItems(int aUserRID, eMainUserProfileType aItemType)
        {
            bool hasItems = false;
            ArrayList user;
            ProfileList storeGroupList;
            HierarchyProfileList hierarchyList;
            //StoreFilterData filterData;
            FilterData filterData;
            DataTable dtStoreFilters;
            DataTable dtHeaderFilters; //TT#1313-MD -jsobek -Header Filters
            DataTable dtAssortmentFilters; //TT#1313-MD -jsobek -Header Filters
            ScheduleData scheduleData;
            DataTable dtTasklists;
            WorkflowBaseData workflowData;
            DataTable dtWorkflows;
            MethodBaseData methodData;
            DataTable dtMethods;

            switch (aItemType)
            {
                case eMainUserProfileType.StoreGroupMainUserFolder:
                    storeGroupList = StoreMgmt.StoreGroup_GetListViewList(aUserRID); //SAB.StoreServerSession.GetStoreGroupListViewList(aUserRID);
                    if (storeGroupList.Count > 0)
                    {
                        hasItems = true;
                    }
                    break;
                case eMainUserProfileType.MerchandiseMainUserFolder:
                    hierarchyList = SAB.HierarchyServerSession.GetHierarchiesForUser(aUserRID);
                    if (hierarchyList.Count > 0)
                    {
                        hasItems = true;
                    }
                    break;
                case eMainUserProfileType.FilterStoreMainUserFolder:
                    user = new ArrayList();
                    //filterData = new StoreFilterData();
                    filterData = new FilterData();
                    user.Add(aUserRID);
                    dtStoreFilters = filterData.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, user);
                    if (dtStoreFilters.Rows.Count > 0)
                    {
                        hasItems = true;
                    }
                    break;
                //Begin TT#1313-MD -jsobek -Header Filters
                case eMainUserProfileType.FilterHeaderMainUserFolder:
                    user = new ArrayList();
                    filterData = new FilterData();
                    user.Add(aUserRID);
                    dtHeaderFilters = filterData.FilterRead(filterTypes.HeaderFilter, eProfileType.FilterHeader, user);
                    if (dtHeaderFilters.Rows.Count > 0)
                    {
                        hasItems = true;
                    }
                    break;
                case eMainUserProfileType.FilterAssortmentMainUserFolder:
                    user = new ArrayList();
                    filterData = new FilterData();
                    user.Add(aUserRID);
                    dtAssortmentFilters = filterData.FilterRead(filterTypes.AssortmentFilter, eProfileType.FilterAssortment, user);
                    if (dtAssortmentFilters.Rows.Count > 0)
                    {
                        hasItems = true;
                    }
                    break;
                //End TT#1313-MD -jsobek -Header Filters
                case eMainUserProfileType.TaskListTaskListMainUserFolder:
                    user = new ArrayList();
                    scheduleData = new ScheduleData();
                    user.Add(aUserRID);
                    dtTasklists = scheduleData.TaskList_Read(user, true, false);
                    if (dtTasklists.Rows.Count > 0)
                    {
                        hasItems = true;
                    }
                    break;
                case eMainUserProfileType.WorkflowMethodMainUserFolder:
                    workflowData = new WorkflowBaseData();
                    dtWorkflows = workflowData.GetWorkflows(aUserRID);
                    if (dtWorkflows.Rows.Count > 0)
                    {
                        hasItems = true;
                    }
                    methodData = new MethodBaseData();
                    dtMethods = methodData.GetMethods(aUserRID);
                    if (dtMethods.Rows.Count > 0)
                    {
                        hasItems = true;
                    }
                    break;
            }

            return hasItems;
        }
        // End TT#126

        // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over
        private void ChangeParent(int aUserRID, int aAssignUser, int aFolderType)
        {
            FolderDataLayer dlFolder;
            DataTable dtFolders;
            DataTable dtFolder;
            DataTable dtChildren;
            FolderProfile userFolderProf, assignFolderProf;
            eProfileType childItemType;
            int childItemRID, userRID, ownerUserRID, key;
            string defaultText, folderName, userName;
            bool isShortcut;
            ArrayList alFolderGroups;

            try
            {
                alFolderGroups = new ArrayList();
                dlFolder = new FolderDataLayer();
                dtFolders = dlFolder.Folder_Read(aUserRID, (eProfileType)aFolderType);
                //if (dtFolders != null || dtFolders.Rows.Count == 1)
                if (dtFolders != null && dtFolders.Rows.Count == 1)
                {
                    userFolderProf = new FolderProfile(dtFolders.Rows[0]);
                    try
                    {
                        dtChildren = dlFolder.Folder_Children_Read(aUserRID, userFolderProf.Key);
                        if (dtChildren.Rows.Count > 0)
                        {
                            dtFolders = dlFolder.Folder_Read(aAssignUser, (eProfileType)aFolderType);
                            if (dtFolders == null || dtFolders.Rows.Count == 0)
                            {
                                switch ((eProfileType)aFolderType)
                                {
                                    case eProfileType.FilterStoreMainUserFolder:
                                        defaultText = "My Filters";
                                        break;
                                    //Begin TT#1313-MD -jsobek -Header Filters
                                    case eProfileType.FilterHeaderMainUserFolder:
                                        defaultText = "My Filters";
                                        break;
                                    case eProfileType.FilterAssortmentMainUserFolder:
                                        defaultText = "My Filters";
                                        break;
                                    //End TT#1313-MD -jsobek -Header Filters
                                    case eProfileType.MerchandiseMainUserFolder:
                                        defaultText = SAB.ClientServerSession.MyHierarchyName;
                                        break;
                                    case eProfileType.StoreGroupMainUserFolder:
                                        defaultText = "My Attributes";
                                        break;
                                    case eProfileType.TaskListTaskListMainUserFolder:
                                        defaultText = "My Task Lists";
                                        break;
                                    case eProfileType.WorkflowMethodMainUserFolder:
                                        defaultText = "My Workflow/Methods";
                                        break;
                                    default:
                                        defaultText = "My Items";
                                        break;
                                }
                                // Begin Track #6375 - JSmith - Assign user multiple times does not work
                                //key = Folder_Create(aUserRID, defaultText, (eProfileType)aFolderType);
                                key = Folder_Create(aAssignUser, defaultText, (eProfileType)aFolderType);
                                // End Track #6375
                                assignFolderProf = new FolderProfile(key, aUserRID, (eProfileType)aFolderType, defaultText, aUserRID);
                            }
                            else
                            {
                                assignFolderProf = new FolderProfile(dtFolders.Rows[0]);
                            }

                            dlFolder.OpenUpdateConnection();
                            foreach (DataRow dr in dtChildren.Rows)
                            {
                                childItemType = (eProfileType)Convert.ToInt32(dr["CHILD_ITEM_TYPE"]);
                                childItemRID = Convert.ToInt32(dr["CHILD_ITEM_RID"]);
                                userRID = Convert.ToInt32(dr["USER_RID"]);
                                ownerUserRID = Convert.ToInt32(dr["OWNER_USER_RID"]);
                                isShortcut = Include.ConvertCharToBool(Convert.ToChar(dr["SHORTCUT_IND"], CultureInfo.CurrentUICulture));
                                if (!isShortcut)
                                {
                                    dlFolder.Folder_Item_Delete(childItemRID, (eProfileType)childItemType);
                                    dlFolder.Folder_Item_Insert(assignFolderProf.Key, childItemRID, (eProfileType)childItemType);
                                    if (childItemType == eProfileType.WorkflowMethodOTSForcastFolder ||
                                        childItemType == eProfileType.WorkflowMethodAllocationFolder)
                                    {
                                        alFolderGroups.Add(childItemRID);
                                    }
                                }
                            }
                            dlFolder.CommitData();

                            if (alFolderGroups.Count > 0)
                            {
                                userName = SAB.ClientServerSession.GetUserName(aUserRID);
                                foreach (int folderRID in alFolderGroups)
                                {
                                    dtFolder = dlFolder.Folder_Read(folderRID);
                                    if (dtFolder.Rows.Count > 0)
                                    {
                                        folderName = Convert.ToString(dtFolder.Rows[0]["FOLDER_ID"], CultureInfo.CurrentUICulture);
                                        if (!dlFolder.ConnectionIsOpen)
                                        {
                                            dlFolder.OpenUpdateConnection();
                                        }
                                        dlFolder.Folder_Rename(folderRID, folderName + " (" + userName + ")");
                                        dlFolder.CommitData();
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (dlFolder.ConnectionIsOpen)
                        {
                            dlFolder.CloseUpdateConnection();
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a folder for a user
        /// </summary>
        /// <param name="aUserRID">The key of the user</param>
        /// <param name="aText">The text for the folder</param>
        /// <param name="aFolderType">The type of the folder</param>
        /// <returns>The key of the new folder</returns>

        protected int Folder_Create(int aUserRID, string aText, eProfileType aFolderType)
        {
            int key;
            FolderDataLayer dlFolder;
            try
            {
                dlFolder = new FolderDataLayer();
                dlFolder.OpenUpdateConnection();

                try
                {
                    key = dlFolder.Folder_Create(aUserRID, aText, aFolderType);
                    dlFolder.CommitData();
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    dlFolder.CloseUpdateConnection();
                }

                return key;
            }
            catch
            {
                throw;
            }
        }

        private void MoveStoreGroups(ArrayList aKeys, int aUserRID, int aAssignUser, string aUserName)
        {
            ProfileList storeGroupList;
            StoreGroupProfile storeGroupProf;
            Hashtable names;

            try
            {
                names = new Hashtable();
                storeGroupList = StoreMgmt.StoreGroup_GetListViewList(aAssignUser); //SAB.StoreServerSession.GetStoreGroupListViewList(aAssignUser);
                foreach (StoreGroupProfile groupProf in storeGroupList)
                {
                    if (!names.ContainsKey(groupProf.GroupId))
                    {
                        names.Add(groupProf.GroupId, groupProf.Key);
                    }
                }

                foreach (int key in aKeys)
                {
                    storeGroupProf = StoreMgmt.StoreGroup_Get(key); //SAB.StoreServerSession.GetStoreGroup(key);
                    storeGroupProf.OwnerUserRID = aAssignUser;
                    if (names.ContainsKey(storeGroupProf.GroupId))
                    {
                        // Begin Track #6376 - JSmith - Perm Assign user and get truncation error
                        if (storeGroupProf.GroupId.Length + aUserName.Length > 50)
                        {
                            storeGroupProf.GroupId = storeGroupProf.GroupId.Substring(0, 50 - aUserName.Length);
                        }
                        // End Track #6376
                        
                        storeGroupProf.GroupId += aUserName;
                    }
                    StoreMgmt.StoreGroup_UpdateIdAndUser(storeGroupProf); //SAB.StoreServerSession.UpdateStoreGroup(storeGroupProf);
                }
            }
            catch
            {
                throw;
            }
        }

        private void MoveHierarchies(ArrayList aKeys, int aUserRID, int aAssignUser, string aUserName)
        {
            HierarchyProfileList hierarchyList;
            HierarchyProfile hierarchyProf;
            Hashtable names;
            int lockedHierarchyKey;

            try
            {
                names = new Hashtable();
                hierarchyList = SAB.HierarchyServerSession.GetHierarchiesForUser(aAssignUser);
                foreach (HierarchyProfile hierProf in hierarchyList)
                {
                    if (!names.ContainsKey(hierProf.HierarchyID))
                    {
                        names.Add(hierProf.HierarchyID, hierProf.Key);
                    }
                }

                foreach (int key in aKeys)
                {
                    lockedHierarchyKey = Include.Undefined;
                    try
                    {
                        hierarchyProf = SAB.HierarchyServerSession.GetHierarchyDataForUpdate(key, false);
                        lockedHierarchyKey = hierarchyProf.Key;
                        if (hierarchyProf.HierarchyLockStatus == eLockStatus.Locked)
                        {
                            hierarchyProf.Owner = aAssignUser;
                            if (names.ContainsKey(hierarchyProf.HierarchyID))
                            {
                                if (hierarchyProf.HierarchyID.Length + aUserName.Length > 50)
                                {
                                    hierarchyProf.HierarchyID = hierarchyProf.HierarchyID.Substring(0, 50 - aUserName.Length);
                                }
                                
                                hierarchyProf.HierarchyID += aUserName;
                            }
                            hierarchyProf.HierarchyChangeType = eChangeType.update;
                            SAB.HierarchyServerSession.HierarchyUpdate(hierarchyProf);

                            SAB.HierarchyServerSession.DequeueHierarchy(hierarchyProf.Key);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (lockedHierarchyKey != Include.Undefined)
                        {
                            SAB.HierarchyServerSession.DequeueHierarchy(lockedHierarchyKey);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#72 - JSmith -  Sharing did not meet expectation for the tasklist explorer
        //private void MoveStoreFilters(ArrayList aKeys, int aUserRID, int aAssignUser, string aUserName)
        //{
        //    StoreFilterData filterData;
        //    DataTable dtFilters;
        //    ArrayList users;
        //    Hashtable names;
        //    string name;
        //    int filterKey;

        //    try
        //    {
        //        names = new Hashtable();
        //        users = new ArrayList();
        //        filterData = new StoreFilterData();
        //        users.Add(aAssignUser);
        //        dtFilters = filterData.StoreFilter_Read(users);
        //        foreach (DataRow row in dtFilters.Rows)
        //        {
        //            name = Convert.ToString(row["STORE_FILTER_NAME"], CultureInfo.CurrentCulture);
        //            filterKey = Convert.ToInt32(row["STORE_FILTER_RID"]);
        //            if (!names.ContainsKey(name))
        //            {
        //                names.Add(name, filterKey);
        //            }
        //        }

        //        try
        //        {
        //            filterData.OpenUpdateConnection();
        //            foreach (int key in aKeys)
        //            {
        //                dtFilters = filterData.StoreFilter_Read(key);
        //                if (dtFilters.Rows.Count > 0)
        //                {
        //                    name = Convert.ToString(dtFilters.Rows[0]["STORE_FILTER_NAME"], CultureInfo.CurrentCulture);
        //                    if (names.ContainsKey(name))
        //                    {
        //                        // Begin Track #6376 - JSmith - Perm Assign user and get truncation error
        //                        if (name.Length + aUserName.Length > 250)
        //                        {
        //                            name = name.Substring(0, 250 - aUserName.Length);
        //                        }
        //                        // End Track #6376

        //                        name += aUserName;
        //                    }
        //                    filterData.StoreFilter_Update(key, aAssignUser, name);
        //                }
        //            }
        //            filterData.CommitData();
        //        }
        //        catch
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            if (filterData.ConnectionIsOpen)
        //            {
        //                filterData.CloseUpdateConnection();
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        private void MoveStoreFilters(ArrayList aKeys, int aUserRID, int aAssignUser, string aUserName)
        {
            //StoreFilterData filterData;
            FilterData filterData; //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
            DataTable dtFilters;
            ArrayList users;
            Hashtable userNames, assignNames;
            string name;
            int filterKey, filterUserRID;

            try
            {
                userNames = new Hashtable();
                assignNames = new Hashtable();
                users = new ArrayList();
                //filterData = new StoreFilterData();
                filterData = new FilterData();
                users.Add(aAssignUser);
                dtFilters = filterData.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, users);
                foreach (DataRow row in dtFilters.Rows)
                {
                    //name = Convert.ToString(row["STORE_FILTER_NAME"], CultureInfo.CurrentCulture);
                    //filterKey = Convert.ToInt32(row["STORE_FILTER_RID"]);
                    //filterUserRID = Convert.ToInt32(row["STORE_FILTER_USER_RID"]);
                    name = Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentCulture);
                    filterKey = Convert.ToInt32(row["FILTER_RID"]);
                    filterUserRID = Convert.ToInt32(row["FILTER_USER_RID"]);
                    if (filterUserRID == aUserRID)
                    {
                        if (!userNames.ContainsKey(name))
                        {
                            userNames.Add(name, filterKey);
                        }
                    }
                    else
                    {
                        if (!assignNames.ContainsKey(name))
                        {
                            assignNames.Add(name, filterKey);
                        }
                    }
                }

                try
                {
                    filterData.OpenUpdateConnection();
                    foreach (int key in aKeys)
                    {
                        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
                        dtFilters = filterData.FilterRead(key);
                        if (dtFilters.Rows.Count > 0)
                        {
                            name = Convert.ToString(dtFilters.Rows[0]["FILTER_NAME"], CultureInfo.CurrentCulture);
                            if (assignNames.ContainsKey(name))
                            {
                                // Begin Track #6376 - JSmith - Perm Assign user and get truncation error
                                if (name.Length + aUserName.Length > 250)
                                {
                                    name = name.Substring(0, 250 - aUserName.Length);
                                }
                                // End Track #6376

                                name += aUserName;
                            }
                            //filterData.FilterUpdate(key, aAssignUser, name);
                            bool isLimited = Include.ConvertIntToBool((int)dtFilters.Rows[0]["IS_LIMITED"]);
                            int resultLimit = (int)dtFilters.Rows[0]["RESULT_LIMIT"];
                            // Begin TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
                            //filterData.UpdateFilter(key, aAssignUser, name, isLimited, resultLimit);
                            filterData.UpdateFilter(key, aAssignUser, aAssignUser, name, isLimited, resultLimit);
                            // End TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
                        }
                        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
                    }
                    filterData.CommitData();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (filterData.ConnectionIsOpen)
                    {
                        filterData.CloseUpdateConnection();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#72

        // Begin TT#72 - JSmith -  Sharing did not meet expectation for the tasklist explorer
        //private void MoveTasklists(ArrayList aKeys, int aUserRID, int aAssignUser, string aUserName)
        //{
        //    ScheduleData scheduleData;
        //    DataTable dtTasklists;
        //    DataRow drTasklists;
        //    ArrayList users;
        //    Hashtable names;
        //    string name;
        //    int tasklistKey;

        //    try
        //    {
        //        names = new Hashtable();
        //        users = new ArrayList();
        //        scheduleData = new ScheduleData();
        //        users.Add(aAssignUser);
        //        dtTasklists = scheduleData.TaskList_Read(users, true, false);
        //        foreach (DataRow row in dtTasklists.Rows)
        //        {
        //            name = Convert.ToString(row["TASKLIST_NAME"], CultureInfo.CurrentCulture);
        //            tasklistKey = Convert.ToInt32(row["TASKLIST_RID"]);
        //            if (!names.ContainsKey(name))
        //            {
        //                names.Add(name, tasklistKey);
        //            }
        //        }

        //        try
        //        {
        //            scheduleData.OpenUpdateConnection();
        //            foreach (int key in aKeys)
        //            {
        //                drTasklists = scheduleData.TaskList_Read(key);
        //                if (drTasklists != null)
        //                {
        //                    name = Convert.ToString(drTasklists["TASKLIST_NAME"], CultureInfo.CurrentCulture);
        //                    if (names.ContainsKey(name))
        //                    {
        //                        // Begin Track #6376 - JSmith - Perm Assign user and get truncation error
        //                        if (name.Length + aUserName.Length > 50)
        //                        {
        //                            name = name.Substring(0, 50 - aUserName.Length);
        //                        }
        //                        // End Track #6376

        //                        name += aUserName;
        //                    }
        //                    scheduleData.TaskList_UpdateName(key, name, aAssignUser);
        //                }
        //            }
        //            scheduleData.CommitData();
        //        }
        //        catch
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            if (scheduleData.ConnectionIsOpen)
        //            {
        //                scheduleData.CloseUpdateConnection();
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        private void MoveTasklists(ArrayList aKeys, int aUserRID, int aAssignUser, string aUserName)
        {
            ScheduleData scheduleData;
            DataTable dtTasklists;
            DataRow drTasklists;
            ArrayList users;
            Hashtable userNames, assignNames;
            string name;
            int tasklistKey, tasklistUserRID;

            try
            {
                userNames = new Hashtable();
                assignNames = new Hashtable();
                users = new ArrayList();
                scheduleData = new ScheduleData();
                users.Add(aAssignUser);
                dtTasklists = scheduleData.TaskList_Read(users, true, false);
                foreach (DataRow row in dtTasklists.Rows)
                {
                    name = Convert.ToString(row["TASKLIST_NAME"], CultureInfo.CurrentCulture);
                    tasklistKey = Convert.ToInt32(row["TASKLIST_RID"]);
                    tasklistUserRID = Convert.ToInt32(row["TASKLIST_USER_RID"]);
                    if (tasklistUserRID == aUserRID)
                    {
                        if (!userNames.ContainsKey(name))
                        {
                            userNames.Add(name, tasklistKey);
                        }
                    }
                    else
                    {
                        if (!assignNames.ContainsKey(name))
                        {
                            assignNames.Add(name, tasklistKey);
                        }
                    }
                }

                try
                {
                    scheduleData.OpenUpdateConnection();
                    foreach (int key in aKeys)
                    {
                        drTasklists = scheduleData.TaskList_Read(key);
                        if (drTasklists != null)
                        {
                            name = Convert.ToString(drTasklists["TASKLIST_NAME"], CultureInfo.CurrentCulture);
                            if (assignNames.ContainsKey(name))
                            {
                                // Begin Track #6376 - JSmith - Perm Assign user and get truncation error
                                if (name.Length + aUserName.Length > 50)
                                {
                                    name = name.Substring(0, 50 - aUserName.Length);
                                }
                                // End Track #6376

                                name += aUserName;
                            }
                            scheduleData.TaskList_UpdateNameandUserRID(key, name, aAssignUser, Include.SystemUserRID);
                        }
                    }
                    scheduleData.CommitData();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (scheduleData.ConnectionIsOpen)
                    {
                        scheduleData.CloseUpdateConnection();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#72

        private void MoveWorkflows(ArrayList aKeys, int aUserRID, int aAssignUser, string aUserName)
        {
            WorkflowBaseData workflowData;
            DataTable dtWorkflows;
            Hashtable names;
            string name;
            int workflowKey, type;

            try
            {
                // append the type to the name to make sure it is unique by type
                names = new Hashtable();
                workflowData = new WorkflowBaseData();
                dtWorkflows = workflowData.GetWorkflows(aAssignUser);
                foreach (DataRow row in dtWorkflows.Rows)
                {
                    if (Convert.ToInt32(row["WORKFLOW_USER"]) == aAssignUser)
                    {
                        name = Convert.ToString(row["WORKFLOW_NAME"], CultureInfo.CurrentCulture);
                        workflowKey = Convert.ToInt32(row["WORKFLOW_RID"]);
                        type = Convert.ToInt32(row["WORKFLOW_TYPE_ID"]);
                        name += "|" + type;
                        if (!names.ContainsKey(name))
                        {
                            names.Add(name, workflowKey);
                        }
                    }
                }

                try
                {
                    workflowData.OpenUpdateConnection();
                    foreach (int key in aKeys)
                    {
                        if (workflowData.PopulateWorkflow(key))
                        {
                            if (names.ContainsKey(workflowData.Workflow_Name + "|" + workflowData.Workflow_Type_ID.GetHashCode()))
                            {
                                // Begin Track #6376 - JSmith - Perm Assign user and get truncation error
                                if (workflowData.Workflow_Name.Length + aUserName.Length > 50)
                                {
                                    workflowData.Workflow_Name = workflowData.Workflow_Name.Substring(0, 50 - aUserName.Length);
                                }
                                // End Track #6376

                                workflowData.Workflow_Name += aUserName;
                            }
                            // Begin TT#72 - JSmith -  Sharing did not meet expectation for the tasklist explorer
                            //workflowData.User_RID = aUserRID;
                            workflowData.User_RID = aAssignUser;
                            // End TT#72
                            // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                            //workflowData.UpdateWorkflow();
                            workflowData.UpdateWorkflow(SAB.ClientServerSession.UserRID);
                            // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                        }
                    }
                    workflowData.CommitData();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (workflowData.ConnectionIsOpen)
                    {
                        workflowData.CloseUpdateConnection();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void MoveMethods(ArrayList aKeys, int aUserRID, int aAssignUser, string aUserName)
        {
            MethodBaseData methodData;
            DataTable dtMethods;
            Hashtable names;
            string name;
            int methodKey, type;

            try
            {
                // append the type to the name to make sure it is unique by type
                names = new Hashtable();
                methodData = new MethodBaseData();
                dtMethods = methodData.GetMethods(aAssignUser);
                foreach (DataRow row in dtMethods.Rows)
                {
                    if (Convert.ToInt32(row["METHOD_USER"]) == aAssignUser)
                    {
                        name = Convert.ToString(row["METHOD_NAME"], CultureInfo.CurrentCulture);
                        methodKey = Convert.ToInt32(row["METHOD_RID"]);
                        type = Convert.ToInt32(row["METHOD_TYPE_ID"]);
                        name += "|" + type;
                        if (!names.ContainsKey(name))
                        {
                            names.Add(name, methodKey);
                        }
                    }
                }

                try
                {
                    methodData.OpenUpdateConnection();
                    foreach (int key in aKeys)
                    {
                        if (methodData.PopulateMethod(key))
                        {
                            if (names.ContainsKey(methodData.Method_Name + "|" + methodData.Method_Type_ID.GetHashCode()))
                            {
                                // Begin Track #6376 - JSmith - Perm Assign user and get truncation error
                                if (methodData.Method_Name.Length + aUserName.Length > 50)
                                {
                                    methodData.Method_Name = methodData.Method_Name.Substring(0, 50 - aUserName.Length);
                                }
                                // End Track #6376

                                methodData.Method_Name += aUserName;
                            }
                            // Begin TT#72 - JSmith -  Sharing did not meet expectation for the tasklist explorer
                            methodData.User_RID = aAssignUser;
                            // End TT#72
                            // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                            //methodData.UpdateMethod();
                            methodData.UpdateMethod(SAB.ClientServerSession.UserRID);
                            // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                        }
                    }
                    methodData.CommitData();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (methodData.ConnectionIsOpen)
                    {
                        methodData.CloseUpdateConnection();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        // End Track #6302

		private void UnassignUser(object sender, System.EventArgs e)
		{
			int userRID;
			string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmUnassign);
			message = message.Replace("{0}", _currNode.Text);
			if (MessageBox.Show (message,  this.Text,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				== DialogResult.Yes)
			{
				userRID = ((NodeTag)_currNode.Tag).Item;
				UnassignUser(userRID, GetAssignedToUser(userRID));
				_currNode.ForeColor = System.Drawing.Color.Black;
				// if the user being assigned is the current user being displayed
				// set the assigned user
				if (_currUser != null &&
					((NodeTag)_currNode.Tag).Item == ((NodeTag)_currUser.Tag).Item)
				{
					cboAssignToUser.SelectedIndex = -1;
				}
			}
		}

		private void UnassignUser(int aUserRID, int aAssignedToUserRID)
		{
			DataTable dtUserItems;
			int userRID, ownerUserRID, itemType, itemRID;
			try	
			{
				dtUserItems = _securityData.GetUserItems(aAssignedToUserRID, aUserRID );
				if (dtUserItems.Rows.Count > 0)
				{
					_securityData.OpenUpdateConnection();
					foreach (DataRow dr in dtUserItems.Rows)
					{
						userRID = Convert.ToInt32(dr["USER_RID"]); 
						itemType = Convert.ToInt32(dr["ITEM_TYPE"]); 
						itemRID = Convert.ToInt32(dr["ITEM_RID"]);
						ownerUserRID = Convert.ToInt32(dr["OWNER_USER_RID"]);
						_securityData.DeleteUserItem(userRID, itemType, itemRID, ownerUserRID);
                        // make sure record does not exist
                        _securityData.DeleteUserItem(ownerUserRID, itemType, itemRID, ownerUserRID);
                        // add item with original user as owner
                        _securityData.AddUserItem(ownerUserRID, itemType, itemRID, ownerUserRID);
					}
					if (rdoChangeUserInactive.Checked)
					{
						string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ActivateUserConfirmation);
						message = message.Replace("{0}", SAB.ClientServerSession.GetUserName(aUserRID));
						if (MessageBox.Show (message,  this.Text,
							MessageBoxButtons.YesNo, MessageBoxIcon.Question)
							== DialogResult.Yes)
						{
							_securityData.MarkUserActive(aUserRID);
							rdoChangeUserActive.Checked = true;
						}
					}
					_securityData.CommitData();
					_rebuildUserHierarchies = true;
				}
			}
			catch (Exception error)
			{
				MessageBox.Show(error.Message);
				return;
			}
			finally
			{
				_securityData.CloseUpdateConnection();
			}
		}
		//End Track #4815

		private void ShowInheritance(object sender, System.EventArgs e)
		{
			frmSecurityInheritance securityInheritance = new frmSecurityInheritance(SAB, _currCheckBox, _currNode);
			securityInheritance.ShowDialog();
		}
		
		private void SearchGroup(object sender, System.EventArgs e)
		{
			pnlSearchGroup.Visible = true;
			pnlSearchGroup.BringToFront();
		}

		private void SearchUser(object sender, System.EventArgs e)
		{
			pnlSearchUser.Visible = true;
			pnlSearchUser.BringToFront();
		}

		private void btnNewUser_Click(object sender, System.EventArgs e)
		{
			// password confirmation ?
			if (txtNewUserPassword.Text != txtNewUserPassConfirm.Text)
			{
				MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PasswordConfirmationFailed));
				return;
			}

			// Begin MID Track# 2546 - require ID
			ErrorProvider.SetError (txtNewUserID,string.Empty);
			if (txtNewUserID.Text.Trim().Length == 0)
			{
				ErrorProvider.SetError (txtNewUserID,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				return;
			}
			// End MID Track# 2546 

            // Begin TT#64 - JSmith - User cannot create group folders
            //int newUserRID;
            int newUserRID = Include.NoRID;
            // End TT#64
			if (_createUserLike == null) 
			{
				try	// create user
				{
					_securityData.OpenUpdateConnection();
					newUserRID = _securityData.CreateUser(txtNewUserID.Text, txtNewUserPassword.Text, txtNewUserFullName.Text, txtNewUserDesc.Text, 
						(rdoNewUserActive.Checked) ? eSecurityActivation.Activate : eSecurityActivation.Deactivate);
					_securityData.CommitData();
                    
				}
				catch (Exception error)
				{
					MessageBox.Show(error.Message);
					return;
				}
				finally
				{
					_securityData.CloseUpdateConnection();
				}
			}
			else
			{
				try	// create user like
				{
					// first get information about existing user
					int sourceUserRID = ((NodeTag)_currNode.Tag).Item;
					DataTable dtSrcGroups = _securityData.GetGroups(sourceUserRID);
					DataTable dtSrcNodes = _securityData.GetUserNodesAssignment(sourceUserRID);
					DataTable dtSrcVersions = _securityData.GetUserVersionsAssignment(sourceUserRID);
					DataTable dtSrcFunctions = _securityData.GetUserFunctionsAssignment(sourceUserRID);

					// create the new user
					_securityData.OpenUpdateConnection();
					newUserRID = _securityData.CreateUser(txtNewUserID.Text, txtNewUserPassword.Text, txtNewUserFullName.Text, txtNewUserDesc.Text, 
						(rdoNewUserActive.Checked) ? eSecurityActivation.Activate : eSecurityActivation.Deactivate);

					// add new user to groups
					foreach (DataRow dr in dtSrcGroups.Rows)
					{
						_securityData.AddUserToGroup(newUserRID, Convert.ToInt32(dr["GROUP_RID"], CultureInfo.CurrentUICulture));
					}

					// assign node permissions to new user
					foreach (DataRow dr in dtSrcNodes.Rows)
					{
						_securityData.AssignUserNode(newUserRID, 
							Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture),
							(eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture),
							(eDatabaseSecurityTypes)Convert.ToInt32(dr["SEC_TYPE"], CultureInfo.CurrentUICulture),
							(eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture));
					}

					// assign version permissions to new user
					foreach (DataRow dr in dtSrcVersions.Rows)
					{
						_securityData.AssignUserVersion(newUserRID, 
							Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture), 
							(eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture),
							(eDatabaseSecurityTypes)Convert.ToInt32(dr["SEC_TYPE"], CultureInfo.CurrentUICulture),
							(eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture));
					}

					// assign function permissions to new user
					foreach (DataRow dr in dtSrcFunctions.Rows)
					{
						_securityData.AssignUserFunction(newUserRID, 
							(eSecurityFunctions)Convert.ToInt32(dr["FUNC_ID"], CultureInfo.CurrentUICulture),
							(eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture),
							(eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture));
					}

                    // Begin TT#1347-MD - stodd - Initial Security for Globabl Options>System should be Deny
                    // Determined to be not needed
                    //// =============================================================================
                    //// If the new user isn't in the Admin group AND
                    //// If they don't already have a AdminBatchOnlyMode security record,
                    //// A DENY record is added for that function
                    ////==============================================================================
                    //DataRow[] userAdminRows = dtSrcGroups.Select("GROUP_RID = 1");
                    //if (userAdminRows.Length == 0)   // Not in Admin Group
                    //{
                    //    DataRow[] userSystemFunctionRows = dtSrcFunctions.Select("FUNC_ID = " + (int)eSecurityFunctions.AdminBatchOnlyMode);
                    //    if (userSystemFunctionRows.Length == 0)
                    //    {
                    //        _securityData.AssignUserFunction(newUserRID, eSecurityFunctions.AdminBatchOnlyMode, eSecurityActions.FullControl, eSecurityLevel.Deny);
                    //    }
                    //}
                    // End TT#1347-MD - stodd - Initial Security for Globabl Options>System should be Deny

                    _securityData.CommitData();
                    
				}
				catch (Exception error)
				{
					MessageBox.Show(error.Message);
					return;
				}
				finally
				{
					_securityData.CloseUpdateConnection();
				}
			}

            // Begin TT#64 - JSmith - User cannot create group folders
            int key;
            if (newUserRID > Include.NoRID)
            {
                key = CreateNewWorkflowMethodExplorerFolder(newUserRID);
                CreateNewOTSForecastGroup(newUserRID, key);
                CreateNewAllocationGroup(newUserRID, key);
            }
            // End TT#64

			// add user to the tree
			BuildUserList();

			TreeNode newNode = new TreeNode(txtNewUserID.Text);
			if (!rdoNewUserActive.Checked)
				newNode.ForeColor = System.Drawing.Color.SlateBlue;
			newNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
			newNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
			newNode.Tag = new NodeTag(NodeType.NT_User, newUserRID, eSecurityOwnerType.User, newUserRID);
			trvUsers.Nodes.Insert(0,newNode);
			AddUserSubFolders(newNode, newUserRID,false);

//			_rebuildInheritedPanel = true;
			_rebuildGroupPanel = true;

			// clear fields
			txtNewUserID.Clear();
			txtNewUserFullName.Clear();
			txtNewUserPassword.Clear();
			txtNewUserPassConfirm.Clear();
			txtNewUserDesc.Clear();
			SAB.ClientServerSession.RefreshSecurity();
		}

        // Begin TT#64 - JSmith - User cannot create group folders
        public int CreateNewWorkflowMethodExplorerFolder(int aUserRID)
        {
            DataTable dtFolders;
            FolderDataLayer dlFolder;
            int key;

            try
            {
                dlFolder = new FolderDataLayer();

                dtFolders = dlFolder.Folder_Read(aUserRID, eProfileType.WorkflowMethodMainUserFolder);
                if (dtFolders == null || dtFolders.Rows.Count == 0)
                {
                    key = Folder_Create(aUserRID, "My Workflow/Methods", eProfileType.WorkflowMethodMainUserFolder);
                }
                else
                {
                    key = Convert.ToInt32(dtFolders.Rows[0]["FOLDER_RID"]);
                }

                return key;
            }
            catch
            {
                throw;
            }
        }

        public void CreateNewOTSForecastGroup(int aUserRID, int aFolderRID)
        {
            string newNodeName;
            FolderProfile newFolderProf;
            FolderDataLayer dlFolder;

            try
            {
                dlFolder = new FolderDataLayer();

                newNodeName = MIDText.GetTextOnly((int)eWorkflowType.Forecast);

                dlFolder.OpenUpdateConnection();

                try
                {
                    newFolderProf = new FolderProfile(Include.NoRID, aUserRID, eProfileType.WorkflowMethodOTSForcastFolder, newNodeName, aUserRID);
                    newFolderProf.Key = dlFolder.Folder_Create(newFolderProf.UserRID, newFolderProf.Name, newFolderProf.FolderType);
                    dlFolder.Folder_Item_Insert(aFolderRID, newFolderProf.Key, eProfileType.WorkflowMethodOTSForcastFolder);

                    dlFolder.CommitData();
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    dlFolder.CloseUpdateConnection();
                }
            }
            catch
            {
                throw;
            }
        }

        public void CreateNewAllocationGroup(int aUserRID, int aFolderRID)
        {
            string newNodeName;
            FolderProfile newFolderProf;
            FolderDataLayer dlFolder;

            try
            {
                dlFolder = new FolderDataLayer();

                newNodeName = MIDText.GetTextOnly((int)eWorkflowType.Allocation);

                dlFolder.OpenUpdateConnection();

                try
                {
                    newFolderProf = new FolderProfile(Include.NoRID, aUserRID, eProfileType.WorkflowMethodAllocationFolder, newNodeName, aUserRID);
                    newFolderProf.Key = dlFolder.Folder_Create(newFolderProf.UserRID, newFolderProf.Name, newFolderProf.FolderType);
                    dlFolder.Folder_Item_Insert(aFolderRID, newFolderProf.Key, eProfileType.WorkflowMethodAllocationFolder);

                    dlFolder.CommitData();
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    dlFolder.CloseUpdateConnection();
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#64
		
		private void btnNewGroup_Click(object sender, System.EventArgs e)
		{
			int newGroupRID;
			// Begin MID Track# 2546 - require ID
			ErrorProvider.SetError (txtNewGroupName,string.Empty);
			if (txtNewGroupName.Text.Trim().Length == 0)
			{
				ErrorProvider.SetError (txtNewGroupName,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired));
				return;
			}
			// End MID Track# 2546
			if (_createGroupLike == null) 
			{
				try	// create Group
				{
					_securityData.OpenUpdateConnection();
					newGroupRID = _securityData.CreateGroup(txtNewGroupName.Text, txtNewGroupDescription.Text,  
						(rdoNewGroupActive.Checked) ? eSecurityActivation.Activate : eSecurityActivation.Deactivate);

					_securityData.CommitData();
				}
				catch (Exception error)
				{
					MessageBox.Show(error.Message);
					return;
				}
				finally
				{
					_securityData.CloseUpdateConnection();
				}
			}
			else
			{
				try	// create Group like
				{
					// first get information about existing Group
					int sourceGroupRID = ((NodeTag)_currNode.Tag).Item;
					DataTable dtSrcUsers = _securityData.GetUsers(sourceGroupRID);
					DataTable dtSrcNodes = _securityData.GetGroupNodesAssignment(sourceGroupRID);
					DataTable dtSrcVersions = _securityData.GetGroupVersionsAssignment(sourceGroupRID);
					DataTable dtSrcFunctions = _securityData.GetGroupFunctionsAssignment(sourceGroupRID);

					// create the new Group
					_securityData.OpenUpdateConnection();
					newGroupRID = _securityData.CreateGroup(txtNewGroupName.Text, txtNewGroupDescription.Text,  
						(rdoNewGroupActive.Checked) ? eSecurityActivation.Activate : eSecurityActivation.Deactivate);

					// add users to new group
					foreach (DataRow dr in dtSrcUsers.Rows)
					{
						_securityData.AddUserToGroup(Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture), newGroupRID);
					}

					// assign node permissions to new Group
					foreach (DataRow dr in dtSrcNodes.Rows)
					{
						_securityData.AssignGroupNode(newGroupRID, 
							Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture), 
							(eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture),
							(eDatabaseSecurityTypes)Convert.ToInt32(dr["SEC_TYPE"], CultureInfo.CurrentUICulture),
							(eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture));
					}

					// assign version permissions to new Group
					foreach (DataRow dr in dtSrcVersions.Rows)
					{
						_securityData.AssignGroupVersion(newGroupRID, 
							Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture), 
							(eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture),
							(eDatabaseSecurityTypes)Convert.ToInt32(dr["SEC_TYPE"], CultureInfo.CurrentUICulture),
							(eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture));
					}

					// assign function permissions to new Group
					foreach (DataRow dr in dtSrcFunctions.Rows)
					{
						_securityData.AssignGroupFunction(newGroupRID, 
							(eSecurityFunctions)Convert.ToInt32(dr["FUNC_ID"], CultureInfo.CurrentUICulture),
							(eSecurityActions)Convert.ToInt32(dr["ACTION_ID"], CultureInfo.CurrentUICulture),
							(eSecurityLevel)Convert.ToInt32(dr["SEC_LVL_ID"], CultureInfo.CurrentUICulture));
					}

					_securityData.CommitData();
				}
				catch (Exception error)
				{
					MessageBox.Show(error.Message);
					return;
				}
				finally
				{
					_securityData.CloseUpdateConnection();
				}
			}
			// add Group to the tree
			BuildGroupList();

			TreeNode newNode = new TreeNode(txtNewGroupName.Text);
			if (!rdoNewGroupActive.Checked)
				newNode.ForeColor = System.Drawing.Color.SlateBlue;
			newNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecGroupImage);
			newNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecGroupImage);
			newNode.Tag = new NodeTag(NodeType.NT_Group, newGroupRID, eSecurityOwnerType.Group, newGroupRID);
			trvGroups.Nodes.Insert(0,newNode);
			AddGroupSubFolders(newNode, newGroupRID);

			_rebuildUserPanel = true;
//			_rebuildInheritedPanel = true;  // inherited permissions may have changed

			// clear fields
			txtNewGroupName.Clear();
			txtNewGroupDescription.Clear();
		}


		private void frmSecurity_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_mouseDownX = e.X;
			_mouseDownY = e.Y;
		}

//		private void lstAvailableUsers_SelectionChangeCommitted(object sender, System.EventArgs e)
//		{
//		
//		}

		private void lstAvailableUsers_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Size dragSize;
		
			try
			{
                //_mouseDown = true;
				_indexOfItemUnderMouseToDrag = ((ListBox)sender).IndexFromPoint(e.X, e.Y);

				if (_indexOfItemUnderMouseToDrag != ListBox.NoMatches) 
				{
					dragSize = SystemInformation.DragSize;
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//					_dragBoxFromMouseDown = new Rectangle(new Point(lstAvailableUsers.Left,  lstAvailableUsers.Top), lstAvailableUsers.Size);
					_dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width /2), e.Y - (dragSize.Height /2)), dragSize);
					lstAvailableUsers.SetSelected(_indexOfItemUnderMouseToDrag, true);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
				} 
				else
				{
					_dragBoxFromMouseDown = Rectangle.Empty;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void lstAvailableUsers_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
                //_mouseDown = false;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void lstAvailableUsers_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//				if ((e.Button & MouseButtons.Left) == MouseButtons.Left &&
//					_mouseDown) 
//				{
//					int X = e.X + lstAvailableUsers.Left;
//					int Y = e.Y + lstAvailableUsers.Top;
//					if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(X, Y)) 
//					{
//						DoDragDrop(new UserDragObject(lstAvailableUsers), DragDropEffects.Move);
//					}
//				}
				if ((e.Button & MouseButtons.Left) == MouseButtons.Left) 
				{
					if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y)) 
					{
						DoDragDrop(new UserDragObject(lstAvailableUsers, _indexOfItemUnderMouseToDrag), DragDropEffects.Move);
					}
				}
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void lstAvailableUsers_DoubleClick(object sender, System.EventArgs e)
		{
			int userRID = Convert.ToInt32(lstAvailableUsers.SelectedValue, CultureInfo.CurrentUICulture);
			AddUserToGroup((DataRowView)lstAvailableUsers.SelectedItem);
//			DataRow dr = ((DataRowView)lstAvailableUsers.SelectedItem).Row;
//			string userName = (string)dr["USER_NAME"];
//
//			int groupRID = ((NodeTag)(_currGroup.Tag)).Item;
//
//			try	// add user to group
//			{
//				_securityData.OpenUpdateConnection();
//				_securityData.AddUserToGroup(userRID, groupRID);
//				_securityData.CommitData();
//			}
//			catch (Exception error)
//			{
//				MessageBox.Show(error.Message);
//				return;
//			}
//			finally
//			{
//				_securityData.CloseUpdateConnection();
//			}
//
//			// add user to the tree
//			TreeNode newNode = new TreeNode(userName);
//			if ((string)dr["USER_ACTIVE_IND"] == "0")
//				newNode.ForeColor = System.Drawing.Color.SlateBlue;
//			newNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
//			newNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
//			newNode.Tag = new NodeTag(NodeType.NT_User, userRID, eSecurityOwnerType.User, userRID);
//			_currUserFolder.Nodes.Add(newNode);
			_currUserFolder.Expand();
			UpdateUsersPanel(_currUserFolder);
			_rebuildUserPanel = true;
//			_rebuildInheritedPanel = true;
			SAB.ClientServerSession.RefreshSecurity();
		}

		private void AddDragDropUsers(UserDragObject aUserDragObject)
		{
			ListBox listBox = (ListBox)aUserDragObject.DragObject;
			int userRID = Convert.ToInt32(lstAvailableUsers.SelectedValue, CultureInfo.CurrentUICulture);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//			for (int i=0;i<listBox.SelectedItems.Count;i++)
//			{
//				AddUserToGroup((DataRowView)listBox.SelectedItems[i]);
//			}
			AddUserToGroup((DataRowView)listBox.Items[aUserDragObject.DragIndex]);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
			_currUserFolder.Expand();
			UpdateUsersPanel(_currUserFolder);
			_rebuildUserPanel = true;
//			_rebuildInheritedPanel = true;
			SAB.ClientServerSession.RefreshSecurity();
		}

		private void AddUserToGroup(DataRowView aDataRowView)
		{
//			int userRID = Convert.ToInt32(lstAvailableUsers.SelectedValue, CultureInfo.CurrentUICulture);
			DataRow dr = aDataRowView.Row;
			int userRID = Convert.ToInt32(dr["USER_RID"],CultureInfo.CurrentCulture);
			string userName = Convert.ToString(dr["USER_NAME"],CultureInfo.CurrentCulture);

			int groupRID = ((NodeTag)(_currGroup.Tag)).Item;

			try	// add user to group
			{
				_securityData.OpenUpdateConnection();
				_securityData.AddUserToGroup(userRID, groupRID);
				_securityData.CommitData();
			}
			catch (Exception error)
			{
				MessageBox.Show(error.Message);
				return;
			}
			finally
			{
				_securityData.CloseUpdateConnection();
			}

			// add user to the tree
			TreeNode newNode = new TreeNode(userName);
			if ((string)dr["USER_ACTIVE_IND"] == "0")
				newNode.ForeColor = System.Drawing.Color.SlateBlue;
			newNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
			newNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
			newNode.Tag = new NodeTag(NodeType.NT_User, userRID, eSecurityOwnerType.User, userRID);
			_currUserFolder.Nodes.Add(newNode);
			//			_currUserFolder.Expand();
//			UpdateUsersPanel(_currUserFolder);
//			_rebuildUserPanel = true;
//			_rebuildInheritedPanel = true;
//			SAB.ClientServerSession.RefreshSecurity();
		}

		private void lstAvailableGroups_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Size dragSize;
		
			try
			{
                //_mouseDown = true;
				_indexOfItemUnderMouseToDrag = ((ListBox)sender).IndexFromPoint(e.X, e.Y);

				if (_indexOfItemUnderMouseToDrag != ListBox.NoMatches) 
				{
					dragSize = SystemInformation.DragSize;
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//					_dragBoxFromMouseDown = new Rectangle(new Point(lstAvailableGroups.Left,  lstAvailableGroups.Top), lstAvailableGroups.Size);
					_dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width /2), e.Y - (dragSize.Height /2)), dragSize);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
					//Begin Track #3981 - JSmith - Dragging wrong group
					lstAvailableGroups.SetSelected(_indexOfItemUnderMouseToDrag, true);
					//End Track #3981
				} 
				else
				{
					_dragBoxFromMouseDown = Rectangle.Empty;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void lstAvailableGroups_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
                //_mouseDown = false;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void lstAvailableGroups_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//				if ((e.Button & MouseButtons.Left) == MouseButtons.Left &&
//					_mouseDown) 
//				{
//					int X = e.X + lstAvailableGroups.Left;
//					int Y = e.Y + lstAvailableGroups.Top;
//					if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(X, Y)) 
//					{
//						DoDragDrop(new GroupDragObject(lstAvailableGroups), DragDropEffects.Move);
//					}
//				}
				if ((e.Button & MouseButtons.Left) == MouseButtons.Left) 
				{
					if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y)) 
					{
						DoDragDrop(new GroupDragObject(lstAvailableGroups, _indexOfItemUnderMouseToDrag), DragDropEffects.Move);
					}
				}
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void lstAvailableGroups_DoubleClick(object sender, System.EventArgs e)
		{
			AddGroupToUser((DataRowView)lstAvailableGroups.SelectedItem);
//			int groupRID = Convert.ToInt32(lstAvailableGroups.SelectedValue, CultureInfo.CurrentUICulture);
//			DataRow dr = ((DataRowView)lstAvailableGroups.SelectedItem).Row;
//			string groupName = (string)dr["GROUP_NAME"];
//
//			int userRID = ((NodeTag)(_currUser.Tag)).Item;
//
//			try	// add user to group
//			{
//				_securityData.OpenUpdateConnection();
//				_securityData.AddUserToGroup(userRID, groupRID);
//				_securityData.CommitData();
//			}
//			catch (Exception error)
//			{
//				MessageBox.Show(error.Message);
//				return;
//			}
//			finally
//			{
//				_securityData.CloseUpdateConnection();
//			}
//
//			// add group to the tree
//			TreeNode newNode = new TreeNode(groupName);
//			if ((string)dr["GROUP_ACTIVE_IND"] == "0")
//				newNode.ForeColor = System.Drawing.Color.SlateBlue;
//			newNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecGroupImage);
//			newNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecGroupImage);
//			newNode.Tag = new NodeTag(NodeType.NT_Group, groupRID, ((NodeTag)_currGroupFolder.Tag).OwnerType, ((NodeTag)_currGroupFolder.Tag).OwnerRID);
//			_currGroupFolder.Nodes.Add(newNode);
			_currGroupFolder.Expand();
			UpdateGroupsPanel(_currGroupFolder);
//			_rebuildInheritedPanel = true;
			_rebuildGroupPanel = true;
			SAB.ClientServerSession.RefreshSecurity();
		}

		private void AddDragDropGroups(GroupDragObject aGroupDragObject)
		{
			ListBox listBox = (ListBox)aGroupDragObject.DragObject;
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//			for (int i=0;i<listBox.SelectedItems.Count;i++)
//			{
//				AddGroupToUser((DataRowView)listBox.SelectedItems[i]);
//			}
			AddGroupToUser((DataRowView)listBox.Items[aGroupDragObject.DragIndex]);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
			_currGroupFolder.Expand();
			UpdateGroupsPanel(_currGroupFolder);
//			_rebuildInheritedPanel = true;
			_rebuildGroupPanel = true;
			SAB.ClientServerSession.RefreshSecurity();
		}

		private void AddGroupToUser(DataRowView aDataRowView)
		{
			DataRow dr = aDataRowView.Row;
			int groupRID = Convert.ToInt32(dr["GROUP_RID"],CultureInfo.CurrentCulture);
			string groupName = (string)dr["GROUP_NAME"];

			int userRID = ((NodeTag)(_currUser.Tag)).Item;

			try	// add user to group
			{
				_securityData.OpenUpdateConnection();
				_securityData.AddUserToGroup(userRID, groupRID);
				_securityData.CommitData();
			}
			catch (Exception error)
			{
				MessageBox.Show(error.Message);
				return;
			}
			finally
			{
				_securityData.CloseUpdateConnection();
			}

			// add group to the tree
			TreeNode newNode = new TreeNode(groupName);
			if ((string)dr["GROUP_ACTIVE_IND"] == "0")
				newNode.ForeColor = System.Drawing.Color.SlateBlue;
			newNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecGroupImage);
			newNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecGroupImage);
			newNode.Tag = new NodeTag(NodeType.NT_Group, groupRID, ((NodeTag)_currGroupFolder.Tag).OwnerType, ((NodeTag)_currGroupFolder.Tag).OwnerRID);
			_currGroupFolder.Nodes.Add(newNode);
		}

		private void trv_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            TreeNodeClipboardList cbList = null;
			// BEGIN Issue 4684 stodd 9.20.2007
			bool isRootGroup = false;
			if (GetSelectedTab() == eTabSelected.Groups)
				isRootGroup = true;
			// END Issue 4684


			TreeNode DropNode;
			if (isRootGroup)
			{
				Point pt = trvGroups.PointToClient(new Point(e.X,e.Y)); 
				DropNode = (TreeNode)trvGroups.GetNodeAt(pt.X, pt.Y);
			}
			else
			{
				Point pt = trvUsers.PointToClient(new Point(e.X,e.Y)); 
				DropNode = (TreeNode)trvUsers.GetNodeAt(pt.X, pt.Y);
			}

			if (DropNode == null) return;

			if (e.Data.GetDataPresent(typeof(UserDragObject)))
			{
				AddDragDropUsers((UserDragObject)e.Data.GetData(typeof(UserDragObject)));
			}
			else
				if (e.Data.GetDataPresent(typeof(GroupDragObject)))
			{
				AddDragDropGroups((GroupDragObject)e.Data.GetData(typeof(GroupDragObject)));
			}
			else
			{
				if (((NodeTag)DropNode.Tag).Type == NodeType.NT_MLID)
					DropNode = DropNode.Parent;
				if (((NodeTag)DropNode.Tag).Type != NodeType.NT_MLIDFolder)
				{
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DropOnMerchandiseFolder));
					return;
				}
				int rootRID = ((NodeTag)DropNode.Parent.Tag).Item;

                //// Create a new instance of the DataObject interface.
                //IDataObject data = Clipboard.GetDataObject();

                //// If the data is ClipboardProfile, then retrieve the data.
                //ClipboardProfile cbp;
				//			HierarchyClipboardData MIDTreeNode_cbd;
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
				{
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
					//Begin TT#831 - JScott - Multi-Select Hierarchies to Drag/Drop into Merchandise Security does not work correctly
					//if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
					foreach (TreeNodeClipboardProfile clipBoardProfile in cbList.ClipboardItems)
					//End TT#831 - JScott - Multi-Select Hierarchies to Drag/Drop into Merchandise Security does not work correctly
					{
                        //if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
                        //{
							//						MIDTreeNode_cbd = (HierarchyClipboardData)cbp.ClipboardData;
							//						InitializeForm(MIDTreeNode_cbd.HierarchyRID, MIDTreeNode_cbd.ParentRID, cbp.Key);
						//Begin TT#831 - JScott - Multi-Select Hierarchies to Drag/Drop into Merchandise Security does not work correctly
						//int nodeRID = cbList.ClipboardProfile.Key;
                        int nodeRID = clipBoardProfile.Key;
						//End TT#831 - JScott - Multi-Select Hierarchies to Drag/Drop into Merchandise Security does not work correctly
							HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(nodeRID);
							HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(hnp.HomeHierarchyRID);

							if ((hnp.LevelType == eHierarchyLevelType.Color) || (hnp.LevelType == eHierarchyLevelType.Size))
							{
								MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CannotAssignSecurityAtColorOrSize));
								return;
							}

							// can not add personal node to other user
							if ((hp.Owner > Include.GlobalUserRID) && (hp.Owner != rootRID))	// Issue 3806
							{
								MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CannotAssignSecurityToPersonalItems));
								return;
							}

							if (((NodeTag)DropNode.Tag).OwnerType == eSecurityOwnerType.User &&
								_userSecurity.IsReadOnly)
							{
								MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
								return;
							}
							else if (((NodeTag)DropNode.Tag).OwnerType == eSecurityOwnerType.Group &&
								_groupSecurity.IsReadOnly)
							{
								MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
								return;
							}


							if (((isRootGroup) ? _securityData.IsGroupNodeAssigned(rootRID, nodeRID) :
								_securityData.IsUserNodeAssigned(rootRID, nodeRID)))
							{
								string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SecurityItemAlreadyExistsInFolder);
								message = message.Replace("{0}", hnp.Text);
								MessageBox.Show(message);
								return;
							}

							try	// add HNID to group
							{
								_securityData.OpenUpdateConnection();
								if (isRootGroup)
								{
									_securityData.AssignGroupNode(rootRID, nodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Allocation, eSecurityLevel.Allow);
									_securityData.AssignGroupNode(rootRID, nodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Chain, eSecurityLevel.Allow);
									_securityData.AssignGroupNode(rootRID, nodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Store, eSecurityLevel.Allow);
								}
								else
								{
									_securityData.AssignUserNode(rootRID, nodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Allocation, eSecurityLevel.Allow);
									_securityData.AssignUserNode(rootRID, nodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Chain, eSecurityLevel.Allow);
									_securityData.AssignUserNode(rootRID, nodeRID, eSecurityActions.FullControl, eDatabaseSecurityTypes.Store, eSecurityLevel.Allow);
								}
								_securityData.CommitData();
							}
							catch (Exception error)
							{
								MessageBox.Show(error.Message);
								return;
							}
							finally
							{
								_securityData.CloseUpdateConnection();
							}

							// add HNID to the tree
							TreeNode newNode = new TreeNode(hnp.Text);
							newNode.ImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecFullAccessImage);
							newNode.SelectedImageIndex = MIDGraphics.ImageIndex(MIDGraphics.SecFullAccessImage);
							newNode.Tag = new NodeTag(NodeType.NT_MLID, nodeRID, ((NodeTag)DropNode.Tag).OwnerType, ((NodeTag)DropNode.Tag).OwnerRID);
							DropNode.Nodes.Add(newNode);
							DropNode.Expand();

							//Begin TT#882 - JScott - Hierarchies or Merchandise in the list of security should be in alphanumeric order.
							SortChildNodes(DropNode);

							//End TT#882 - JScott - Hierarchies or Merchandise in the list of security should be in alphanumeric order.
//							_rebuildInheritedPanel = true;

                        //}
                        //else
                        //{
                        //    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard));
                        //}
					}
					//Begin TT#831 - JScott - Multi-Select Hierarchies to Drag/Drop into Merchandise Security does not work correctly
					//else
					//{
					//    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
					//}
					//End TT#831 - JScott - Multi-Select Hierarchies to Drag/Drop into Merchandise Security does not work correctly
				}
				else
				{
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
				}
				SAB.ClientServerSession.RefreshSecurity();
			}
		}

		private void trv_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Image_DragEnter(sender, e);
			//			if (e.Data.GetDataPresent(DataFormats.Text)) 
			//			if (e.Data.GetDataPresent(typeof(MIDTreeNode))) 
			//				e.Effect = DragDropEffects.All;
			//			else
			//				e.Effect = DragDropEffects.None;
		}

		private void trv_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
            TreeNodeClipboardList cbList = null;
            HierarchyNodeClipboardList hnList = null;
			e.Effect = _currentEffect;
            Image_DragOver(sender, e);
			Point pt = ((TreeView)sender).PointToClient(new Point(e.X,e.Y)); 
			TreeNode treeNode = (TreeNode)((TreeView)sender).GetNodeAt(pt.X, pt.Y);
			if (treeNode != null)
			{
				if (e.Data.GetDataPresent(typeof(UserDragObject)) &&
					((NodeTag)treeNode.Tag).Type == NodeType.NT_UserFolder)
				{
					e.Effect = DragDropEffects.All;
				}
				else
					if (e.Data.GetDataPresent(typeof(GroupDragObject)) &&
					((NodeTag)treeNode.Tag).Type == NodeType.NT_GroupFolder)
				{
					e.Effect = DragDropEffects.All;
				}
                    else if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)) &&
                    ((NodeTag)treeNode.Tag).Type == NodeType.NT_MLIDFolder)
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode ||
                    //    cbp.ClipboardDataType == eClipboardDataType.HierarchyNodeList)
                    if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                    {
                        e.Effect = DragDropEffects.All;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                    else if (e.Data.GetDataPresent(typeof(HierarchyNodeClipboardList)) &&
                    ((NodeTag)treeNode.Tag).Type == NodeType.NT_MLIDFolder)
                    {
                        hnList = (HierarchyNodeClipboardList)e.Data.GetData(typeof(HierarchyNodeClipboardList));
                        //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode ||
                        //    cbp.ClipboardDataType == eClipboardDataType.HierarchyNodeList)
                        if (hnList.ClipboardDataType == eProfileType.HierarchyNode)
                        {
                            e.Effect = DragDropEffects.All;
                        }
                        else
                        {
                            e.Effect = DragDropEffects.None;
                        }
                    }
				else
				{
					e.Effect = DragDropEffects.None;
				}
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

//		private void genericRadioButton_CheckedChanged(object sender, System.EventArgs e)
//		{
//			if (((RadioButton)sender).Checked == false) return;
//			if (_radioNoChange)
//			{
//				_radioNoChange = false;
//				return;
//			}
//			try
//			{
//				eSecurityLevel secLevel;
//				eImageIndex imageIndex;
//				if (sender == rdbFullAccess)
//				{
//					secLevel.SetFullControl();
//					imageIndex = eImageIndex.fullAccess;
//				}
//				else if (sender == rdbReadOnly)
//				{
//					secLevel.SetReadOnly();
//					imageIndex = eImageIndex.readOnlyAccess;
//				}
//				else
//				{
//					secLevel = eSecurityLevel.NoAccess;
//					imageIndex = eImageIndex.noAccess;
//				}
//
//				int rootRID = ((NodeTag)_currNode.Parent.Parent.Tag).Item;
//				int nodeRID = ((NodeTag)_currNode.Tag).Item;
//
//				_securityData.OpenUpdateConnection();
//				if ((eTabSelected)tabControl1.SelectedIndex == eTabSelected.Groups)
//				{
//					switch(((NodeTag)_currNode.Parent.Tag).Type)
//					{
//						case NodeType.NT_MLIDFolder:
//							_securityData.AssignGroupNode(rootRID,nodeRID,secLevel);
//							break;
//						case NodeType.NT_VersionFolder:
//							_securityData.AssignGroupVersion(rootRID,nodeRID,secLevel);
//							break;
//						case NodeType.NT_FunctionFolder:
//							_securityData.AssignGroupFunction(rootRID,(eSecurityFunctions)nodeRID,secLevel);
//							break;
//					}
//				}
//				else 
//				{
//					switch(((NodeTag)_currNode.Parent.Tag).Type)
//					{
//						case NodeType.NT_MLIDFolder:
//							_securityData.AssignUserNode(rootRID,nodeRID,secLevel);
//							break;
//						case NodeType.NT_VersionFolder:
//							_securityData.AssignUserVersion(rootRID,nodeRID,secLevel);
//							break;
//						case NodeType.NT_FunctionFolder:
//							_securityData.AssignUserFunction(rootRID,(eSecurityFunctions)nodeRID,secLevel);
//							break;
//					}
//
//				}
//				_securityData.CommitData();
//				_currNode.ImageIndex = (int)imageIndex;
//				_currNode.SelectedImageIndex = (int)imageIndex;
//			}
//			catch (Exception error)
//			{
//				MessageBox.Show(error.Message);
//				return;
//			}
//			finally
//			{
//				_securityData.CloseUpdateConnection();
//			}
//			_rebuildInheritedPanel = true;
//			SAB.ClientServerSession.RefreshSecurity();
//		}
	
		// BEGIN Issue 4684 stodd 9.20.2007
		private eTabSelected GetSelectedTab()
		{
			eTabSelected selectedTab = eTabSelected.None;

			if (tabControl1.SelectedTab != null)
			{
				if (tabControl1.SelectedTab.Text == tabGroups.Text)
					selectedTab = eTabSelected.Groups;
				else if (tabControl1.SelectedTab.Text == tabUsers.Text)
					selectedTab = eTabSelected.Users;
			}
			return selectedTab;
		}
		// END Issue 4684

		private void btnChangeGroup_Click(object sender, System.EventArgs e)
		{
			int groupRID = ((NodeTag)_currGroup.Tag).Item;

			try
			{
				_securityData.OpenUpdateConnection();
				_securityData.UpdateGroup(groupRID,
					txtChangeGroupName.Text,
					txtChangeGroupDesc.Text,
					(rdoChangeGroupActive.Checked) ? eSecurityActivation.Activate : eSecurityActivation.Deactivate);
				_securityData.CommitData();
				//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
				_currGroup.Text = txtChangeGroupName.Text;
				//End Track #4815
			}
			catch (Exception error)
			{
				MessageBox.Show(error.Message);
			}
			finally
			{
				_securityData.CloseUpdateConnection();
			}
			if (!rdoChangeGroupActive.Checked)
				_currGroup.ForeColor = System.Drawing.Color.SlateBlue;
			else
				_currGroup.ForeColor = System.Drawing.Color.Black;
			BuildGroupList();
			_rebuildUserPanel = true;
//			_rebuildInheritedPanel = true;   // inherited permissions may have changed
			_rebuildGroupPanel = true;
			SAB.ClientServerSession.RefreshSecurity();

		}

		private void btnChangeUser_Click(object sender, System.EventArgs e)
		{
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			string message = string.Empty;
			int assignedUserRID = Include.NoRID;
			//End Track #4815
			int userRID = ((NodeTag)_currUser.Tag).Item;
			//Begin TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
			DataTable dtUserSession;
			//End TT#677 - JScott - Tasklists disappearing from Tasklist Explorer

			string newPassword = txtChangeUserPassword.Text;
			if (newPassword.Length > 0)
			{
				// password confirmation ?
				if (newPassword != txtChangeUserPassConfirm.Text)
				{
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PasswordConfirmationFailed));
					return;
				}
			}
			else
			{
				newPassword = null;
			}

			try
			{
				_securityData.OpenUpdateConnection();
				_securityData.UpdateUser(userRID,
					//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//					null,
					txtChangeUserID.Text,
					//End Track #4815
					newPassword,
					txtChangeUserFullName.Text,
					txtChangeUserDesc.Text,
					(rdoChangeUserActive.Checked) ? eSecurityActivation.Activate : eSecurityActivation.Deactivate);
				_securityData.CommitData();
				//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
				_currUser.Text = txtChangeUserID.Text;

                UserNameStorage.PopulateUserNameStorageCache(_securityData.GetUserNameStorageCache()); //TT#827-MD -jsobek -Allocation Reviews Performance
				
				if (_assignedToChanged)
				{
					assignedUserRID = GetAssignedToUser(userRID);
					if (cboAssignToUser.SelectedIndex > 0)
					{
						//Begin TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
						dtUserSession = _securityData.GetUserSession(userRID, eSessionStatus.LoggedIn);

						if (dtUserSession.Rows.Count > 0)
						{
							message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CannotAssignLoggedOnUser);
							MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}

						//End TT#677 - JScott - Tasklists disappearing from Tasklist Explorer
						if (cbxPermanentlyMove.Checked)
						{
							message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PermanentMoveWarning);
							message = message.Replace("{0}", txtChangeUserID.Text);
                            // Begin TT#532-MD - JSmith - Export method tried to create a new method and receive a TargetInvocationException.  Also tried to open all existing methods and received the same error.
                            //message = message.Replace("{1}", cboAssignToUser.SelectedText);
                            message = message.Replace("{1}", cboAssignToUser.Get_SelectedText());
                            // End TT#532-MD - JSmith - Export method tried to create a new method and receive a TargetInvocationException.  Also tried to open all existing methods and received the same error.
							if (MessageBox.Show (message,  this.Text,
								MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
								== DialogResult.No)
							{
								return;
							}
						}
						// if already assigned to different user, unassign first
						if (assignedUserRID != Include.NoRID &&
							((ComboObject)(cboAssignToUser.SelectedItem)).Key != assignedUserRID)
						{
							UnassignUser(userRID, assignedUserRID);
						}
						AssignUser(userRID, ((ComboObject)(cboAssignToUser.SelectedItem)).Key, cbxPermanentlyMove.Checked);
						if (cbxPermanentlyMove.Checked)
						{
							message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PermanentMoveConfirmation);
						}
						else
						{
							message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AssignConfirmation);
						}
						message = message.Replace("{0}", txtChangeUserID.Text);
						message = message.Replace("{1}", cboAssignToUser.Text);
						MessageBox.Show(message,  this.Text, MessageBoxButtons.OK);
					}
					else
					{
						UnassignUser(userRID, assignedUserRID);
					}
				}
				//End Track #4815
			}
			catch (Exception error)
			{
				MessageBox.Show(error.Message);
			}
			finally
			{
				_securityData.CloseUpdateConnection();
			}
			txtChangeUserPassword.Text = "";
			txtChangeUserPassConfirm.Text = "";
			if (!rdoChangeUserActive.Checked)
				_currUser.ForeColor = System.Drawing.Color.SlateBlue;
			else
				_currUser.ForeColor = System.Drawing.Color.Black;
			BuildUserList();
			_rebuildUserPanel = true;
//			_rebuildInheritedPanel = true;
			_rebuildGroupPanel = true;
		}

		private void btnPermissions_Click(object sender, System.EventArgs e)
		{
			SaveChanges();
			DisplayPermissions(_currNode);
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//			SetImage(_currNode);
//			SetChildImages(_currNode);
			SetImages(_currNode);
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
		}

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//		private void SetChildImages(TreeNode aTreeNode)
//		{
//			foreach (TreeNode tn in aTreeNode.Nodes)
//			{
//				SetImage (tn);
//				foreach (TreeNode child in tn.Nodes)
//				{
//					SetChildImages(child);
//				}
//			}
//		}
		private void SetImages(TreeNode aTreeNode)
		{
			SetImage(aTreeNode);

			foreach (TreeNode child in aTreeNode.Nodes)
			{
				SetImages(child);
			}
		}
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes

		//Begin TT#882 - JScott - Hierarchies or Merchandise in the list of security should be in alphanumeric order.
		public void SortChildNodes(TreeNode aParentNode)
		{
			TreeView treeView;
			TreeNode selectedNode;
			TreeNode[] nodeArray;
			int i;
			SortedList itemList;
			IDictionaryEnumerator dictEnum;

			try
			{
				treeView = aParentNode.TreeView;
				treeView.BeginUpdate();

				try
				{
					selectedNode = treeView.SelectedNode;

					if (aParentNode.Nodes.Count > 0)
					{
						nodeArray = new TreeNode[aParentNode.Nodes.Count];
						aParentNode.Nodes.CopyTo(nodeArray, 0);
						aParentNode.Nodes.Clear();

						itemList = new SortedList();

						for (i = 0; i < nodeArray.Length; i++)
						{
							//Begin TT#1027 - JScott - Received Unhandled Exception when selecting Administation>Security
							//itemList.Add(nodeArray[i].Text, nodeArray[i]);
							itemList.Add(nodeArray[i].Text + ((NodeTag)nodeArray[i].Tag).Item, nodeArray[i]);
							//End TT#1027 - JScott - Received Unhandled Exception when selecting Administation>Security
						}

						dictEnum = itemList.GetEnumerator();

						while (dictEnum.MoveNext())
						{
							aParentNode.Nodes.Add((TreeNode)dictEnum.Value);
						}
					}

					treeView.SelectedNode = (TreeNode)selectedNode;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					treeView.EndUpdate();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End TT#882 - JScott - Hierarchies or Merchandise in the list of security should be in alphanumeric order.
		override protected bool SaveChanges()
		{
			try
			{
				if (_currNode != null &&
					_permissionChanged)
				{
					_securityData.OpenUpdateConnection();
					NodeTag nodeTag = (NodeTag)_currNode.Tag;
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//					PermissionTag allowCheckBoxTag = null;
//					PermissionTag denyCheckBoxTag = null;
//					CheckBox allowCheckBox = null;
//					CheckBox denyCheckBox = null;
//					eSecurityLevel securityLevel = eSecurityLevel.NotSpecified;
//					foreach (Control control in pnlFunctionPermission.Controls)
//					{
//						if (control is CheckBox)
//						{
//							CheckBox checkBox = (CheckBox)control;
//							PermissionTag checkBoxTag = (PermissionTag)checkBox.Tag;
//
//							if (checkBoxTag.PermissionType == ePermissionType.Allow)
//							{
//								allowCheckBox = checkBox;
//								allowCheckBoxTag = checkBoxTag;
//							}
//							else if (!checkBoxTag.Permission.IsInherited)
//							{
//								securityLevel = eSecurityLevel.NotSpecified;
//								denyCheckBox = checkBox;
//								denyCheckBoxTag = checkBoxTag;
//
//								if (allowCheckBox.Checked)
//								{
//									securityLevel = eSecurityLevel.Allow;
//								}
//								else if (denyCheckBox.Checked)
//								{
//									securityLevel = eSecurityLevel.Deny;
//								}
//							
//								switch (nodeTag.Type)
//								{
//									case NodeType.NT_MLID:
//										if (nodeTag.OwnerType == eSecurityOwnerType.Group)
//										{
//											_securityData.AssignGroupNode(nodeTag.OwnerRID, nodeTag.Item, checkBoxTag.Action, checkBoxTag.SecurityType, securityLevel);
//										}
//										else
//										{
//											_securityData.AssignUserNode(nodeTag.OwnerRID, nodeTag.Item, checkBoxTag.Action, checkBoxTag.SecurityType, securityLevel);
//										}
//										break;
//									case NodeType.NT_Version:
//										if (nodeTag.OwnerType == eSecurityOwnerType.Group)
//										{
//											_securityData.AssignGroupVersion(nodeTag.OwnerRID, nodeTag.Item, checkBoxTag.Action, checkBoxTag.SecurityType, securityLevel);
//										}
//										else
//										{
//											_securityData.AssignUserVersion(nodeTag.OwnerRID, nodeTag.Item, checkBoxTag.Action, checkBoxTag.SecurityType, securityLevel);
//										}
//										break;
//									case NodeType.NT_Function:
//										if (nodeTag.OwnerType == eSecurityOwnerType.Group)
//										{
//											_securityData.AssignGroupFunction(nodeTag.OwnerRID, (eSecurityFunctions)nodeTag.Item, checkBoxTag.Action, securityLevel);
//										}
//										else
//										{
//											_securityData.AssignUserFunction(nodeTag.OwnerRID, (eSecurityFunctions)nodeTag.Item, checkBoxTag.Action, securityLevel);
//										}
//										break;
//								}
//							}
//						}
//					}
					PermissionTag checkBoxTag;
					bool allAllow;
					bool allDeny;
					IDictionaryEnumerator iEnum;
					PermissionSet permSet;

					iEnum = _permissionSetHash.GetEnumerator();

					while (iEnum.MoveNext())
					{
						permSet = (PermissionSet)iEnum.Value;
						SaveActionChanges(permSet.FullControlAllowCheckBox, eSecurityLevel.NotSpecified);

						allAllow = true;
						allDeny = true;

						foreach (CheckBox checkBox in permSet.ActionAllowCheckBoxList)
						{
							checkBoxTag = (PermissionTag)checkBox.Tag;

							SaveActionChanges(checkBox, eSecurityLevel.NotSpecified);

							if (checkBoxTag.Permission.IsInherited || !checkBox.Checked)
							{
								allAllow = false;
							}

							if (checkBoxTag.Permission.IsInherited || !checkBoxTag.OpposingCheckBox.Checked)
							{
								allDeny = false;
							}
						}

						if (allAllow)
						{
							SaveActionChanges(permSet.FullControlAllowCheckBox, eSecurityLevel.Allow);
						}
						else if (allDeny)
						{
							SaveActionChanges(permSet.FullControlAllowCheckBox, eSecurityLevel.Deny);
						}
						else
						{
							foreach (CheckBox checkBox in permSet.ActionAllowCheckBoxList)
							{
								checkBoxTag = (PermissionTag)checkBox.Tag;

								if (!checkBoxTag.Permission.IsInherited)
								{
									if (checkBox.Checked)
									{
										SaveActionChanges(checkBox, eSecurityLevel.Allow);
									}
									else if (checkBoxTag.OpposingCheckBox.Checked)
									{
										SaveActionChanges(checkBox, eSecurityLevel.Deny);
									}
								}
							}
						}
					}
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
					_securityData.CommitData();
				}
				_permissionChanged = false;
				ChangePending = false;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				if (_securityData.ConnectionIsOpen)
				{
					_securityData.CloseUpdateConnection();
				}
			}
			return true;
		}

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
		private void SaveActionChanges(CheckBox aCheckBox, eSecurityLevel aSecurityLevel)
		{
			NodeTag nodeTag;
			PermissionTag checkBoxTag = null;
			CheckBox checkBox = null;

			try
			{
				nodeTag = (NodeTag)_currNode.Tag;

				checkBox = aCheckBox;
				checkBoxTag = (PermissionTag)checkBox.Tag;

				switch (nodeTag.Type)
				{
					case NodeType.NT_MLID:
						if (nodeTag.OwnerType == eSecurityOwnerType.Group)
						{
							_securityData.AssignGroupNode(nodeTag.OwnerRID, nodeTag.Item, checkBoxTag.Action, checkBoxTag.SecurityType, aSecurityLevel);
						}
						else
						{
							_securityData.AssignUserNode(nodeTag.OwnerRID, nodeTag.Item, checkBoxTag.Action, checkBoxTag.SecurityType, aSecurityLevel);
						}
						break;
					case NodeType.NT_Version:
						if (nodeTag.OwnerType == eSecurityOwnerType.Group)
						{
							_securityData.AssignGroupVersion(nodeTag.OwnerRID, nodeTag.Item, checkBoxTag.Action, checkBoxTag.SecurityType, aSecurityLevel);
						}
						else
						{
							_securityData.AssignUserVersion(nodeTag.OwnerRID, nodeTag.Item, checkBoxTag.Action, checkBoxTag.SecurityType, aSecurityLevel);
						}
						break;
					case NodeType.NT_Function:
						if (nodeTag.OwnerType == eSecurityOwnerType.Group)
						{
							_securityData.AssignGroupFunction(nodeTag.OwnerRID, (eSecurityFunctions)nodeTag.Item, checkBoxTag.Action, aSecurityLevel);
						}
						else
						{
							_securityData.AssignUserFunction(nodeTag.OwnerRID, (eSecurityFunctions)nodeTag.Item, checkBoxTag.Action, aSecurityLevel);
						}
						break;
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
		private void btnRemovePassword_Click(object sender, System.EventArgs e)
		{
			try
			{
				_securityData.OpenUpdateConnection();
				_securityData.SetUserPassword(txtChangeUserID.Text,null,null, false);
				_securityData.CommitData();
				MessageBox.Show (MIDText.GetText((int)eSecurityAuthenticate.PasswordChanged));
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				if (_securityData.ConnectionIsOpen)
				{
					_securityData.CloseUpdateConnection();
				}
			}
		}

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		private void cboAssignToUser_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (cboAssignToUser.SelectedIndex > 0)
			{
				cbxPermanentlyMove.Enabled = true;
				rdoChangeUserInactive.Checked = true;
				gbxChangeUserStatus.Enabled = false;
			}
			else
			{
				gbxChangeUserStatus.Enabled = true;
			}
			if (_changeUserPanelLoaded)
			{
				_assignedToChanged = true;
			}
		}
		//End Track #4815

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		override protected void BeforeClosing()
		{
			try
			{
				if (_rebuildUserHierarchies)
				{
					SAB.HierarchyServerSession.ReBuildUserHierarchies();
					_rebuildUserHierarchies = false;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}
		//End Track #4815

        private void cboAssignToUser_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboAssignToUser_SelectionChangeCommitted(source, new EventArgs());
        }
 
	}

	public enum NodeType
	{
		NT_Group,
		NT_GroupFolder,
		NT_User,
		NT_UserFolder,
		NT_MLID,
		NT_MLIDFolder,
		NT_Version,
		NT_VersionFolder,
		NT_Function,
		NT_FunctionFolder
//		NT_View,
//		NT_ViewFolder,
//		NT_Store,
//		NT_StoreFolder,
//		NT_StoreGroup,
//		NT_StoreGroupFolder
	}
	
	public class NodeTag
	{
		NodeType _type;
		eSecurityOwnerType _inheritedType;
		eSecurityOwnerType _ownerType;
		int _ownerRID;
		int _item;
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		bool _itemDeleted = false;
		//End Track #4815

		public NodeTag(NodeType aNodeType, int aItem, eSecurityOwnerType aOwnerType, int aOwnerRID)
		{
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//			Initialize(aNodeType, aItem, aOwnerType, aOwnerType, aOwnerRID);
			Initialize(aNodeType, aItem, aOwnerType, aOwnerType, aOwnerRID, false);
			//End Track #4815
		}

		public NodeTag(NodeType aNodeType, int aItem, eSecurityOwnerType aInheritedType, eSecurityOwnerType aOwnerType, int aOwnerRID)
		{
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//			Initialize(aNodeType, aItem, aInheritedType, aOwnerType, aOwnerRID);
			Initialize(aNodeType, aItem, aInheritedType, aOwnerType, aOwnerRID, false);
			//End Track #4815
		}

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		public NodeTag(NodeType aNodeType, int aItem, eSecurityOwnerType aOwnerType, int aOwnerRID, bool aItemDeleted)
		{
			Initialize(aNodeType, aItem, aOwnerType, aOwnerType, aOwnerRID, aItemDeleted);
		}
		//End Track #4815

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//		private void Initialize(NodeType aNodeType, int aItem, eSecurityOwnerType aInheritedType, eSecurityOwnerType aOwnerType, int aOwnerRID)
//		{
//			_type = aNodeType;
//			_item = aItem;
//			_inheritedType = aInheritedType;
//			_ownerType = aOwnerType;
//			_ownerRID = aOwnerRID;
//		}
		private void Initialize(NodeType aNodeType, int aItem, eSecurityOwnerType aInheritedType, eSecurityOwnerType aOwnerType, int aOwnerRID, bool aItemDeleted)
		{
			_type = aNodeType;
			_item = aItem;
			_inheritedType = aInheritedType;
			_ownerType = aOwnerType;
			_ownerRID = aOwnerRID;
			_itemDeleted = aItemDeleted;
		}
		//End Track #4815

		public NodeType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		public eSecurityOwnerType InheritedType
		{
			get
			{
				return _inheritedType;
			}
			set
			{
				_inheritedType = value;
			}
		}

		public eSecurityOwnerType OwnerType
		{
			get
			{
				return _ownerType;
			}
			set
			{
				_ownerType = value;
			}
		}

		public int OwnerRID
		{
			get
			{
				return _ownerRID;
			}
			set
			{
				_ownerRID = value;
			}
		}

		public int Item
		{
			get
			{
				return _item;
			}
			set
			{
				_item = value;
			}
		}

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		public bool ItemDeleted
		{
			get
			{
				return _itemDeleted;
			}
			set
			{
				_itemDeleted = value;
			}
		}
		//End Track #4815
	}

	public enum ePermissionType
	{
		Allow,
		Deny
	}

	public class PermissionTag
	{
		private NodeType _type;
		private eSecurityActions _action;
		private eDatabaseSecurityTypes _securityType;
		private ePermissionType _permissionType;
		private int _securityFunction;
		private SecurityPermission _permission;
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
		private CheckBox _opposingCheckBox;
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes

		public PermissionTag(NodeType aNodeType, eSecurityActions aAction, eDatabaseSecurityTypes aSecurityType, ePermissionType aPermissionType, SecurityPermission aPermission)
		{
			_type = aNodeType;
			_action = aAction;
			_permissionType = aPermissionType;
			_securityType = aSecurityType;
			_permission = aPermission;
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
			_opposingCheckBox = null;
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
		}

		public PermissionTag(NodeType aNodeType, eSecurityActions aAction, eDatabaseSecurityTypes aSecurityType, ePermissionType aPermissionType, int aSecurityFunction, SecurityPermission aPermission)
		{
			_type = aNodeType;
			_action = aAction;
			_permissionType = aPermissionType;
			_securityType = aSecurityType;
			_securityFunction = aSecurityFunction;
			_permission = aPermission;
		}

		public NodeType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		public eSecurityActions Action
		{
			get
			{
				return _action;
			}
			set
			{
				_action = value;
			}
		}

		public ePermissionType PermissionType
		{
			get
			{
				return _permissionType;
			}
			set
			{
				_permissionType = value;
			}
		}

		public eDatabaseSecurityTypes SecurityType
		{
			get
			{
				return _securityType;
			}
			set
			{
				_securityType = value;
			}
		}

		public int SecurityFunction
		{
			get
			{
				return _securityFunction;
			}
			set
			{
				_securityFunction = value;
			}
		}

		public SecurityPermission Permission
		{
			get
			{
				return _permission;
			}
			set
			{
				_permission = value;
			}
		}
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes

		public CheckBox OpposingCheckBox
		{
			get
			{
				return _opposingCheckBox;
			}
			set
			{
				_opposingCheckBox = value;
			}
		}
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
	}

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
	class PermissionSet
	{
		private CheckBox _fullControlAllowCheckBox;
		private CheckBox _fullControlDenyCheckBox;
		private ArrayList _actionAllowCheckBoxList;
		private ArrayList _actionDenyCheckBoxList;

		public PermissionSet()
		{
			_actionAllowCheckBoxList = new ArrayList();
			_actionDenyCheckBoxList = new ArrayList();
		}

		public CheckBox FullControlAllowCheckBox
		{
			get
			{
				return _fullControlAllowCheckBox;
			}
			set
			{
				_fullControlAllowCheckBox = value;
			}
		}

		public CheckBox FullControlDenyCheckBox
		{
			get
			{
				return _fullControlDenyCheckBox;
			}
			set
			{
				_fullControlDenyCheckBox = value;
			}
		}

		public ArrayList ActionAllowCheckBoxList
		{
			get
			{
				return _actionAllowCheckBoxList;
			}
			set
			{
				_actionAllowCheckBoxList = value;
			}
		}

		public ArrayList ActionDenyCheckBoxList
		{
			get
			{
				return _actionDenyCheckBoxList;
			}
			set
			{
				_actionDenyCheckBoxList = value;
			}
		}
	}

//End Track #5091 - JScott - Secuirty Lights don't change when permission changes
	#region UserDragObject
	/// <summary>
	/// Class that defines the UserDragObject, which is a generic object used during drag events.
	/// </summary>

	public class UserDragObject
	{
		//=======
		// FIELDS
		//=======

		public object DragObject;
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
		public int DragIndex;
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes

		//=============
		// CONSTRUCTORS
		//=============

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//		public UserDragObject(object aDragObject)
//		{
//			DragObject = aDragObject;
//		}
		public UserDragObject(object aDragObject, int aDragIndex)
		{
			DragObject = aDragObject;
			DragIndex = aDragIndex;
		}
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}
	#endregion

	#region UserDragObject
	/// <summary>
	/// Class that defines the GroupDragObject, which is a generic object used during drag events.
	/// </summary>

	public class GroupDragObject
	{
		//=======
		// FIELDS
		//=======

		public object DragObject;
//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
		public int DragIndex;
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes

		//=============
		// CONSTRUCTORS
		//=============

//Begin Track #5091 - JScott - Secuirty Lights don't change when permission changes
//		public GroupDragObject(object aDragObject)
//		{
//			DragObject = aDragObject;
//		}
		public GroupDragObject(object aDragObject, int aDragIndex)
		{
			DragObject = aDragObject;
			DragIndex = aDragIndex;
		}
//End Track #5091 - JScott - Secuirty Lights don't change when permission changes

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}
	#endregion
}
