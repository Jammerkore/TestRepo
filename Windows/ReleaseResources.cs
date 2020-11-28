using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.Business;
using MIDRetail.Windows.Controls;


namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for ReleaseResources.
	/// </summary>
	public class frmReleaseResources : MIDFormBase
	{
		#region Windows Form Designer generated code

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDeleteLayouts;
        private Button btnRelease;
        private RadioButton rdoAllUsers;
        private RadioButton rdoUser;
        private MIDRetail.Windows.Controls.MIDComboBoxEnh cboUser;
        
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDeleteLayouts = new System.Windows.Forms.Button();
            this.btnRelease = new System.Windows.Forms.Button();
            this.rdoAllUsers = new System.Windows.Forms.RadioButton();
            this.rdoUser = new System.Windows.Forms.RadioButton();
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
            this.btnCancel.Location = new System.Drawing.Point(130, 108);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(114, 24);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDeleteLayouts
            // 
            this.btnDeleteLayouts.Location = new System.Drawing.Point(130, 77);
            this.btnDeleteLayouts.Name = "btnDeleteLayouts";
            this.btnDeleteLayouts.Size = new System.Drawing.Size(114, 25);
            this.btnDeleteLayouts.TabIndex = 9;
            this.btnDeleteLayouts.Text = "Clear Layouts";
            this.btnDeleteLayouts.UseVisualStyleBackColor = true;
            this.btnDeleteLayouts.Click += new System.EventHandler(this.btnDeleteLayout_Click);
            // 
            // btnRelease
            // 
            this.btnRelease.Location = new System.Drawing.Point(10, 77);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(114, 25);
            this.btnRelease.TabIndex = 1;
            this.btnRelease.Text = "Release Resources";
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);
            // 
            // rdoAllUsers
            // 
            this.rdoAllUsers.Location = new System.Drawing.Point(12, 12);
            this.rdoAllUsers.Name = "rdoAllUsers";
            this.rdoAllUsers.Size = new System.Drawing.Size(112, 16);
            this.rdoAllUsers.TabIndex = 4;
            this.rdoAllUsers.Text = "All Users";
            this.rdoAllUsers.CheckedChanged += new System.EventHandler(this.rdoAllUsers_CheckedChanged);
            // 
            // rdoUser
            // 
            this.rdoUser.Location = new System.Drawing.Point(12, 34);
            this.rdoUser.Name = "rdoUser";
            this.rdoUser.Size = new System.Drawing.Size(104, 24);
            this.rdoUser.TabIndex = 5;
            this.rdoUser.Text = "User:";
            this.rdoUser.CheckedChanged += new System.EventHandler(this.rdoUser_CheckedChanged);
            // 
            // cboUser
            // 
            this.cboUser.Location = new System.Drawing.Point(108, 37);
            this.cboUser.Name = "cboUser";
            this.cboUser.Size = new System.Drawing.Size(136, 21);
            this.cboUser.TabIndex = 6;
            // 
            // frmReleaseResources
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(261, 146);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDeleteLayouts);
            this.Controls.Add(this.cboUser);
            this.Controls.Add(this.btnRelease);
            this.Controls.Add(this.rdoAllUsers);
            this.Controls.Add(this.rdoUser);
            this.Name = "frmReleaseResources";
            this.Text = "Release Resources";
            this.Load += new System.EventHandler(this.frmReleaseResources_Load);
            this.Controls.SetChildIndex(this.rdoUser, 0);
            this.Controls.SetChildIndex(this.rdoAllUsers, 0);
            this.Controls.SetChildIndex(this.btnRelease, 0);
            this.Controls.SetChildIndex(this.cboUser, 0);
            this.Controls.SetChildIndex(this.btnDeleteLayouts, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		SessionAddressBlock _SAB;
		SecurityAdmin _secAdmin;
		DataTable _dtUser;
		MIDEnqueue _midNQ;
		//Begin TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
        InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
		//End TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
        //Begin Track #5194 - JScott - User Release is allowed when permission is denied
		FunctionSecurityProfile _personalSecurity;
		FunctionSecurityProfile _othersSecurity;
        //End Track #5194 - JScott - User Release is allowed when permission is denied

        // Begin TT#856 - JSmith - Out of memory
		//public frmReleaseResources(SessionAddressBlock aSAB)
        public frmReleaseResources(SessionAddressBlock aSAB)
            : base(aSAB)
		// End TT#856
		{
			InitializeComponent();

			_SAB = aSAB;
			_secAdmin = new SecurityAdmin();
			_midNQ = new MIDEnqueue();
		}

		protected override void Dispose(bool disposing)
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}

				this.btnRelease.Click -= new System.EventHandler(this.btnRelease_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.rdoAllUsers.CheckedChanged -= new System.EventHandler(this.rdoAllUsers_CheckedChanged);
				this.rdoUser.CheckedChanged -= new System.EventHandler(this.rdoUser_CheckedChanged);
				this.Load -= new System.EventHandler(this.frmReleaseResources_Load);
			}
			base.Dispose( disposing );
		}

		private void frmReleaseResources_Load(object sender, System.EventArgs e)
		{
			try
			{
//Begin Track #5194 - JScott - User Release is allowed when permission is denied
//				_dtUser = _secAdmin.GetUsers();
				_personalSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsReleaseResourcesPersonal);
				_othersSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsReleaseResourcesOthers);

				_dtUser = _secAdmin.GetUsers();

				//Begin Track #5328 - JSmith - User only sees administrator
//				if (!_personalSecurity.AllowUpdate)
				if (!_personalSecurity.AllowDelete)
				//End Track #5328
				{
					_dtUser.PrimaryKey = new DataColumn[] {_dtUser.Columns["USER_RID"]};
					_dtUser.Rows.Remove(_dtUser.Rows.Find(_SAB.ClientServerSession.UserRID));
				}

//End Track #5194 - JScott - User Release is allowed when permission is denied
				BindUserComboBox();
				rdoUser.Checked = true;

				ApplySecurity();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void BindUserComboBox()
		{
			try
			{
				cboUser.ValueMember = "USER_RID";
				cboUser.DisplayMember = "USER_NAME";
				cboUser.DataSource = _dtUser;
//Begin Track #5194 - JScott - User Release is allowed when permission is denied
//				cboUser.SelectedValue = _SAB.ClientServerSession.UserRID;

				//Begin Track #5328 - JSmith - User only sees administrator
//				if (_personalSecurity.AllowUpdate)
				if (_personalSecurity.AllowDelete)
				//End Track #5328
				{
					cboUser.SelectedValue = _SAB.ClientServerSession.UserRID;
				}
//End Track #5194 - JScott - User Release is allowed when permission is denied
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}


//Begin Track #5194 - JScott - User Release is allowed when permission is denied
//		private void ApplySecurity()
//		{
//			FunctionSecurityProfile personalSecurity;
//			FunctionSecurityProfile othersSecurity;
//
//			try
//			{
//				personalSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsReleaseResourcesPersonal);
//				othersSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsReleaseResourcesOthers);
//
//				if (othersSecurity.AllowUpdate)
//				{
//					rdoAllUsers.Enabled = true;
//					rdoUser.Enabled = true;
//					cboUser.Enabled = true;
//				}
//				else if (personalSecurity.AllowUpdate)
//				{
//					rdoAllUsers.Enabled = false;
//					rdoUser.Enabled = true;
//					cboUser.Enabled = false;
//				}
//				else
//				{
//					rdoAllUsers.Enabled = false;
//					rdoUser.Enabled = false;
//					cboUser.Enabled = false;
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleException(exc);
//			}
//		}
		private void ApplySecurity()
		{
			try
			{
				//Begin Track #5328 - JSmith - User only sees administrator
//				if (_othersSecurity.AllowUpdate && _personalSecurity.AllowUpdate)
//				{
//					rdoAllUsers.Enabled = true;
//					rdoUser.Enabled = true;
//					cboUser.Enabled = true;
//				}
//				else if (_othersSecurity.AllowUpdate)
//				{
//					rdoAllUsers.Enabled = false;
//					rdoUser.Enabled = true;
//					cboUser.Enabled = true;
//				}
//				else if (_personalSecurity.AllowUpdate)
//				{
//					rdoAllUsers.Enabled = false;
//					rdoUser.Enabled = true;
//					cboUser.Enabled = false;
//				}
//				else
//				{
//					rdoAllUsers.Enabled = false;
//					rdoUser.Enabled = false;
//					cboUser.Enabled = false;
//				}
				if (_othersSecurity.AllowDelete && _personalSecurity.AllowDelete)
				{
					rdoAllUsers.Enabled = true;
					rdoUser.Enabled = true;
					cboUser.Enabled = true;
				}
				else if (_othersSecurity.AllowDelete)
				{
					rdoAllUsers.Enabled = false;
					rdoUser.Enabled = true;
					cboUser.Enabled = true;
				}
				else if (_personalSecurity.AllowDelete)
				{
					rdoAllUsers.Enabled = false;
					rdoUser.Enabled = true;
					cboUser.Enabled = false;
				}
				else
				{
					rdoAllUsers.Enabled = false;
					rdoUser.Enabled = false;
					cboUser.Enabled = false;
				}
				//End Track #5328
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
//End Track #5194 - JScott - User Release is allowed when permission is denied

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Close();
//				this.MdiParent = null;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void btnRelease_Click(object sender, System.EventArgs e)
		{
			try
			{
				_midNQ.OpenUpdateConnection();

				try
				{
					if (rdoAllUsers.Checked)
					{
						foreach (DataRow userRow in _dtUser.Rows)
						{
							ReleaseUser(Convert.ToInt32(userRow["USER_RID"], CultureInfo.CurrentUICulture));
						}
					}
					else
					{
						ReleaseUser(Convert.ToInt32(cboUser.SelectedValue, CultureInfo.CurrentUICulture));
					}

					_midNQ.CommitData();

					MessageBox.Show("Resources successfully released.", "Release Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Begin TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
                    string sessionUserName = Convert.ToString(SAB.ClientServerSession.GetUserName(_SAB.ClientServerSession.UserRID), CultureInfo.CurrentUICulture);
                    string resourceUserName = Convert.ToString(SAB.ClientServerSession.GetUserName(Convert.ToInt32(cboUser.SelectedValue)), CultureInfo.CurrentUICulture);
                    string auditMessage = MIDText.GetText(eMIDTextCode.msg_ResourcesReleased);
                        auditMessage = auditMessage.Replace("{0}", sessionUserName);
                        auditMessage = auditMessage.Replace("{1}", resourceUserName);
                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, auditMessage, "Release Resources", true);
                    //End TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
				}
				catch (Exception exc)
				{
					_midNQ.Rollback();
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_midNQ.CloseUpdateConnection();
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void rdoUser_CheckedChanged(object sender, System.EventArgs e)
		{
			cboUser.Enabled = true;
			//Begin TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
            btnDeleteLayouts.Enabled = true;
			//End TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
		}

		private void rdoAllUsers_CheckedChanged(object sender, System.EventArgs e)
		{
			cboUser.Enabled = false;
			//Begin TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
            btnDeleteLayouts.Enabled = false;
			//End TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
		}

//		private void HandleException(Exception exc)
//		{
//			MessageBox.Show(exc.ToString());
//			Debug.WriteLine(exc.ToString());
//		}

		private void ReleaseUser(int aUserRID)
		{
			_midNQ.ChainWeekEnqueue_Delete(aUserRID);
			_midNQ.StoreWeekEnqueue_Delete(aUserRID);
			_midNQ.Enqueue_Delete(eLockType.Header, aUserRID);
			_midNQ.Enqueue_DeleteAll(aUserRID);
		}
		
		//Begin TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
        private void btnDeleteLayout_Click(object sender, System.EventArgs e)
        {
            //string sessionUserName = Convert.ToString(SAB.ClientServerSession.GetUserName(_SAB.ClientServerSession.UserRID), CultureInfo.CurrentUICulture).ToUpper();
            //string resourceUserName = Convert.ToString(SAB.ClientServerSession.GetUserName(Convert.ToInt32(cboUser.SelectedValue)), CultureInfo.CurrentUICulture).ToUpper();
            string sessionUserName = Convert.ToString(SAB.ClientServerSession.GetUserName(_SAB.ClientServerSession.UserRID), CultureInfo.CurrentUICulture);
            string resourceUserName = Convert.ToString(SAB.ClientServerSession.GetUserName(Convert.ToInt32(cboUser.SelectedValue)), CultureInfo.CurrentUICulture);
            string msg;
            string multiMsg = string.Empty;
            try
            {

                if (sessionUserName == resourceUserName)
                {
                    multiMsg = MIDText.GetText(eMIDTextCode.msg_CurrentLogon);
                    multiMsg = multiMsg.Replace("{0}", sessionUserName);
                    msg = multiMsg + MIDText.GetText(eMIDTextCode.msg_LayoutDeleteScheduled);
                    msg = msg.Replace("{0}", sessionUserName);
                    msg = msg.Replace("{1}", resourceUserName);

                    if (MessageBox.Show(msg + " Do you wish to continue? ", "User Logged on", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        DeleteUserLayouts(Convert.ToInt32(cboUser.SelectedValue, CultureInfo.CurrentUICulture), true);
                        MessageBox.Show("Layouts scheduled to be cleared.", "Clear Layouts Scheduled", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        string auditMessage = MIDText.GetText(eMIDTextCode.msg_LayoutDeleteScheduled);
                        auditMessage = auditMessage.Replace("{0}", sessionUserName);
                        auditMessage = auditMessage.Replace("{1}", resourceUserName);
                        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, auditMessage, "Release Resources", true);
                    }
                }
                else
                {
                    DataTable userDT = _secAdmin.GetUserSession(Convert.ToInt32(cboUser.SelectedValue), eSessionStatus.LoggedIn);
                    if (userDT.Rows.Count > 0)
                    {
                        msg = MIDText.GetText(eMIDTextCode.msg_CurrentLogon) + "Do you wish to continue? ";
                        msg = msg.Replace("{0}", resourceUserName);
                        if (MessageBox.Show(msg, "User Logged on", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            int delRows = DeleteUserLayouts(Convert.ToInt32(cboUser.SelectedValue, CultureInfo.CurrentUICulture), false);
                            msg = MIDText.GetText(eMIDTextCode.msg_ClearSuccess);
                            msg = msg.Replace("{0}", Convert.ToString(delRows));
                            MessageBox.Show(msg, "Clear Layouts Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            string auditMessage = MIDText.GetText(eMIDTextCode.msg_LayoutsDeleted);
                            auditMessage = auditMessage.Replace("{0}", sessionUserName);
                            auditMessage = auditMessage.Replace("{1}", resourceUserName);
                            auditMessage = auditMessage.Replace("{2}", Convert.ToString(delRows));
                            _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, auditMessage, "Release Resources", true);
                        }
                    }
                    else
                    {
                        int delRows = DeleteUserLayouts(Convert.ToInt32(cboUser.SelectedValue, CultureInfo.CurrentUICulture), false);
                        msg = MIDText.GetText(eMIDTextCode.msg_ClearSuccess);
                        msg = msg.Replace("{0}", Convert.ToString(delRows));
                        MessageBox.Show(msg, "Clear Layouts Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        string auditMessage = MIDText.GetText(eMIDTextCode.msg_LayoutsDeleted);
                        auditMessage = auditMessage.Replace("{0}", sessionUserName);
                        auditMessage = auditMessage.Replace("{1}", resourceUserName);
                        auditMessage = auditMessage.Replace("{2}", Convert.ToString(delRows));
                        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, auditMessage, "Release Resources", true);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private int DeleteUserLayouts(int aUserRID, bool sched)
        {
            int retRows = 0;
            try
            {
                if (sched)
                {
                    _SAB.ClientServerSession.ScheduledLayoutDelete(_SAB.ClientServerSession.UserRID, false);
                }
                else
                {
                    retRows = layoutData.InfragisticsUserLayout_Delete(aUserRID);
                }
                return retRows;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }   
        }
    }
	//End TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
}
