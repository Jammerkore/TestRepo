using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for AssignUser.
	/// </summary>
	public class frmAssignUser : MIDFormBase
	{
		//=======
		// FIELDS
		//=======
		private int _userRID;
		private bool _cancelPressed = false;
		private int _selectedUserRID = Include.NoRID;
		private string _selectedUserName = string.Empty;

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.CheckBox cbxPermanentlyMove;
		private System.Windows.Forms.Label lblAssignTo;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboUser;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		//=============
		// CONSTRUCTORS
		//=============
		public frmAssignUser(SessionAddressBlock aSAB, int aUserRID) : base(aSAB)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_userRID = aUserRID;

		}

		//===========
		// PROPERTIES
		//===========
		public int SelectedUserRID
		{
			get { return _selectedUserRID ; }
		}

		public string SelectedUserName
		{
			get { return _selectedUserName ; }
		}

		public bool CancelPressed
		{
			get { return _cancelPressed ; }
		}

		public bool PermanentMove
		{
			get { return cbxPermanentlyMove.Checked ; }
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
				this.Load -= new System.EventHandler(this.frmAssignUser_Load);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.cboUser.SelectionChangeCommitted -= new System.EventHandler(this.cboUser_SelectionChangeCommitted);

                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboUser.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboUser_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
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
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.cbxPermanentlyMove = new System.Windows.Forms.CheckBox();
			this.lblAssignTo = new System.Windows.Forms.Label();
			this.cboUser = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			this.SuspendLayout();
			// 
			// utmMain
			// 
			this.utmMain.MenuSettings.ForceSerialization = true;
			this.utmMain.ToolbarSettings.ForceSerialization = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(212, 164);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 17;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(124, 164);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 16;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// cbxPermanentlyMove
			// 
			this.cbxPermanentlyMove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbxPermanentlyMove.Location = new System.Drawing.Point(16, 120);
			this.cbxPermanentlyMove.Name = "cbxPermanentlyMove";
			this.cbxPermanentlyMove.Size = new System.Drawing.Size(184, 24);
			this.cbxPermanentlyMove.TabIndex = 18;
			this.cbxPermanentlyMove.Text = "Permanently move user";
			this.cbxPermanentlyMove.CheckedChanged += new System.EventHandler(this.cbxPermanentlyMove_CheckedChanged);
			// 
			// lblAssignTo
			// 
			this.lblAssignTo.Location = new System.Drawing.Point(24, 56);
			this.lblAssignTo.Name = "lblAssignTo";
			this.lblAssignTo.Size = new System.Drawing.Size(64, 23);
			this.lblAssignTo.TabIndex = 19;
			this.lblAssignTo.Text = "Assign To";
			this.lblAssignTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lblAssignTo.Click += new System.EventHandler(this.lblAssignTo_Click);
			// 
			// cboUser
			// 
			this.cboUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboUser.Location = new System.Drawing.Point(96, 57);
			this.cboUser.Name = "cboUser";
			this.cboUser.Size = new System.Drawing.Size(176, 21);
			this.cboUser.TabIndex = 20;
			this.cboUser.SelectionChangeCommitted += new System.EventHandler(this.cboUser_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboUser.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboUser_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
			// 
			// frmAssignUser
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(296, 198);
			this.Controls.Add(this.cboUser);
			this.Controls.Add(this.lblAssignTo);
			this.Controls.Add(this.cbxPermanentlyMove);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Name = "frmAssignUser";
			this.Text = "AssignUser";
			this.Load += new System.EventHandler(this.frmAssignUser_Load);
			this.Controls.SetChildIndex(this.btnOK, 0);
			this.Controls.SetChildIndex(this.btnCancel, 0);
			this.Controls.SetChildIndex(this.cbxPermanentlyMove, 0);
			this.Controls.SetChildIndex(this.lblAssignTo, 0);
			this.Controls.SetChildIndex(this.cboUser, 0);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void frmAssignUser_Load(object sender, System.EventArgs e)
		{
			SetText();
			LoadUsers();
			Format_Title(eDataState.Updatable, eMIDTextCode.frm_AssignUser ,null);
			cbxPermanentlyMove.Enabled = false;
		}

		private void SetText()
		{
			btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
			btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
			cbxPermanentlyMove.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PermanentlyMoveUser);
			lblAssignTo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AssignTo);
		}

		private void LoadUsers()
		{
			SecurityAdmin securityData;
			DataTable dtUsers;
			int userRID;

			securityData = new SecurityAdmin();
			dtUsers = securityData.GetUsers();
			dtUsers.DefaultView.Sort = "USER_NAME ASC"; 
			dtUsers.AcceptChanges();
			
			cboUser.Items.Clear();
			foreach (DataRow dr in dtUsers.Rows)
			{
				// do not show system user or user to assign
				userRID = Convert.ToInt32(dr["USER_RID"]);
				if (userRID == Include.UndefinedUserRID ||
					userRID == Include.AdministratorUserRID ||
					userRID == Include.SystemUserRID ||
					userRID == Include.GlobalUserRID ||
					userRID == _userRID)
				{
					continue;
				}
				// do not show deleted or inactive user
				if (!Include.ConvertCharToBool(Convert.ToChar(dr["USER_ACTIVE_IND"])) ||
					Include.ConvertCharToBool(Convert.ToChar(dr["USER_DELETE_IND"])))
				{
					continue;
				}
				cboUser.Items.Add(new ComboObject(Convert.ToInt32(dr["USER_RID"]), Convert.ToString(dr["USER_NAME"])));
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				_cancelPressed = true;
				Cancel_Click();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			string error;
			try
			{
				if (cboUser.SelectedIndex < 0)
				{
					error = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired, false);
					ErrorProvider.SetError (this.cboUser,error);
					MessageBox.Show (error, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					OK_Click();
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		override protected bool SaveChanges()
		{
			ErrorFound = false;
			
			if (ValuesValid())
			{
				_selectedUserRID = ((ComboObject)cboUser.SelectedItem).Key;
				_selectedUserName = ((ComboObject)cboUser.SelectedItem).Value;
				ChangePending = false;
			}
			else
			{
				ErrorFound = true;
				MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect));
			}

			return true;
		}

		private bool ValuesValid()
		{
			string error;
			if (cboUser.SelectedIndex < 0)
			{
				error = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired, false);
				ErrorProvider.SetError (this.cboUser,error);
				return false;
			}
			else
			{
				return true;
			}
		}

		private void cboUser_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			ErrorProvider.SetError (this.cboUser,string.Empty);
			cbxPermanentlyMove.Enabled = true;
		}
        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cboUser_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboUser_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

		private void cbxPermanentlyMove_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

		private void lblAssignTo_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
