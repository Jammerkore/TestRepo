using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmScheduleBrowser : MIDFormBase
	{
		private Infragistics.Win.UltraWinGrid.UltraGrid ulgSchedule;
		private System.Windows.Forms.MenuItem mniProperties;
		private frmScheduleProperties schedProperties;
		private System.Windows.Forms.ContextMenu ctxSchedule;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.MenuItem mniHold;
		private System.Windows.Forms.MenuItem mniSep1;
		private System.Windows.Forms.MenuItem mniRunNow;
		private System.Windows.Forms.MenuItem mniCancel;
		private System.Windows.Forms.CheckBox chkRefresh;
		private System.Windows.Forms.Label lblRefresh2;
		private System.Windows.Forms.Label lblRefresh1;
		private System.Windows.Forms.NumericUpDown nudRefresh;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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

				this.ctxSchedule.Popup -= new System.EventHandler(this.ctxSchedule_Popup);
				this.mniHold.Click -= new System.EventHandler(this.mniHold_Click);
				this.mniRunNow.Click -= new System.EventHandler(this.mniRunNow_Click);
				this.mniCancel.Click -= new System.EventHandler(this.mniCancel_Click);
				this.mniProperties.Click -= new System.EventHandler(this.mniProperties_Click);
				this.btnRefresh.Click -= new System.EventHandler(this.btnRefresh_Click);
				this.chkRefresh.CheckedChanged -= new System.EventHandler(this.chkRefresh_CheckedChanged);
				this.nudRefresh.ValueChanged -= new System.EventHandler(this.nudRefresh_ValueChanged);
//Begin Track #4876 - JScott - Delete key removes entry from grid but not from DB
				this.ulgSchedule.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgSchedule_BeforeRowsDeleted);
//End Track #4876 - JScott - Delete key removes entry from grid but not from DB
				this.Closing -= new System.ComponentModel.CancelEventHandler(this.frmScheduleBrowser_Closing);
				this.Load -= new System.EventHandler(this.frmSchedule_Load);

				if (schedProperties != null)
				{
					schedProperties.OnSchedulePropertiesSaveHandler -= new frmScheduleProperties.SchedulePropertiesSaveEventHandler(OnSchedulePropertiesSave);
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.ulgSchedule = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ctxSchedule = new System.Windows.Forms.ContextMenu();
            this.mniHold = new System.Windows.Forms.MenuItem();
            this.mniRunNow = new System.Windows.Forms.MenuItem();
            this.mniCancel = new System.Windows.Forms.MenuItem();
            this.mniSep1 = new System.Windows.Forms.MenuItem();
            this.mniProperties = new System.Windows.Forms.MenuItem();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.chkRefresh = new System.Windows.Forms.CheckBox();
            this.nudRefresh = new System.Windows.Forms.NumericUpDown();
            this.lblRefresh2 = new System.Windows.Forms.Label();
            this.lblRefresh1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ulgSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRefresh)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // ulgSchedule
            // 
            this.ulgSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ulgSchedule.ContextMenu = this.ctxSchedule;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ulgSchedule.DisplayLayout.Appearance = appearance1;
            this.ulgSchedule.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ulgSchedule.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ulgSchedule.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgSchedule.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ulgSchedule.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ulgSchedule.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ulgSchedule.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ulgSchedule.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ulgSchedule.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgSchedule.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ulgSchedule.Location = new System.Drawing.Point(8, 8);
            this.ulgSchedule.Name = "ulgSchedule";
            this.ulgSchedule.Size = new System.Drawing.Size(768, 432);
            this.ulgSchedule.TabIndex = 0;
            this.ulgSchedule.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgSchedule_BeforeRowsDeleted);
            this.ulgSchedule.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgSchedule_InitializeLayout);
            // 
            // ctxSchedule
            // 
            this.ctxSchedule.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniHold,
            this.mniRunNow,
            this.mniCancel,
            this.mniSep1,
            this.mniProperties});
            this.ctxSchedule.Popup += new System.EventHandler(this.ctxSchedule_Popup);
            // 
            // mniHold
            // 
            this.mniHold.Index = 0;
            this.mniHold.Text = "Hold";
            this.mniHold.Click += new System.EventHandler(this.mniHold_Click);
            // 
            // mniRunNow
            // 
            this.mniRunNow.Index = 1;
            this.mniRunNow.Text = "Run Now";
            this.mniRunNow.Click += new System.EventHandler(this.mniRunNow_Click);
            // 
            // mniCancel
            // 
            this.mniCancel.Index = 2;
            this.mniCancel.Text = "Cancel";
            this.mniCancel.Click += new System.EventHandler(this.mniCancel_Click);
            // 
            // mniSep1
            // 
            this.mniSep1.Index = 3;
            this.mniSep1.Text = "-";
            // 
            // mniProperties
            // 
            this.mniProperties.Index = 4;
            this.mniProperties.Text = "Properties";
            this.mniProperties.Click += new System.EventHandler(this.mniProperties_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(8, 448);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(72, 24);
            this.btnRefresh.TabIndex = 37;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chkRefresh
            // 
            this.chkRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkRefresh.Location = new System.Drawing.Point(120, 452);
            this.chkRefresh.Name = "chkRefresh";
            this.chkRefresh.Size = new System.Drawing.Size(16, 16);
            this.chkRefresh.TabIndex = 38;
            this.chkRefresh.Visible = false;
            this.chkRefresh.CheckedChanged += new System.EventHandler(this.chkRefresh_CheckedChanged);
            // 
            // nudRefresh
            // 
            this.nudRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudRefresh.Enabled = false;
            this.nudRefresh.Location = new System.Drawing.Point(232, 450);
            this.nudRefresh.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudRefresh.Name = "nudRefresh";
            this.nudRefresh.Size = new System.Drawing.Size(40, 20);
            this.nudRefresh.TabIndex = 39;
            this.nudRefresh.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudRefresh.Visible = false;
            this.nudRefresh.ValueChanged += new System.EventHandler(this.nudRefresh_ValueChanged);
            // 
            // lblRefresh2
            // 
            this.lblRefresh2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRefresh2.Enabled = false;
            this.lblRefresh2.Location = new System.Drawing.Point(272, 452);
            this.lblRefresh2.Name = "lblRefresh2";
            this.lblRefresh2.Size = new System.Drawing.Size(48, 16);
            this.lblRefresh2.TabIndex = 40;
            this.lblRefresh2.Text = "seconds";
            this.lblRefresh2.Visible = false;
            // 
            // lblRefresh1
            // 
            this.lblRefresh1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRefresh1.Enabled = false;
            this.lblRefresh1.Location = new System.Drawing.Point(136, 452);
            this.lblRefresh1.Name = "lblRefresh1";
            this.lblRefresh1.Size = new System.Drawing.Size(104, 16);
            this.lblRefresh1.TabIndex = 41;
            this.lblRefresh1.Text = "Auto refresh every";
            this.lblRefresh1.Visible = false;
            // 
            // frmScheduleBrowser
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(784, 478);
            this.Controls.Add(this.nudRefresh);
            this.Controls.Add(this.lblRefresh1);
            this.Controls.Add(this.lblRefresh2);
            this.Controls.Add(this.chkRefresh);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.ulgSchedule);
            this.Name = "frmScheduleBrowser";
            this.Text = "Schedule Browser";
            this.Load += new System.EventHandler(this.frmSchedule_Load);
            this.Closed += new System.EventHandler(this.frmScheduleBrowser_Closed);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmScheduleBrowser_Closing);
            this.Controls.SetChildIndex(this.ulgSchedule, 0);
            this.Controls.SetChildIndex(this.btnRefresh, 0);
            this.Controls.SetChildIndex(this.chkRefresh, 0);
            this.Controls.SetChildIndex(this.lblRefresh2, 0);
            this.Controls.SetChildIndex(this.lblRefresh1, 0);
            this.Controls.SetChildIndex(this.nudRefresh, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ulgSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRefresh)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private FunctionSecurityProfile _browserSecLvl;
		private ScheduleData _dlSchedule;
		private DataTable _dtScheduledJobs;
		private BrowserRefresh _browserRefresh;
		private MIDReaderWriterLock _gridLock;

		public frmScheduleBrowser(SessionAddressBlock aSAB) : base(aSAB)
		{
			try
			{
				InitializeComponent();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public decimal CurrentRefreshRate
		{
			get
			{
				return nudRefresh.Value;
			}
		}

		public void HandleExceptions(Exception exc)
		{
			Debug.WriteLine(exc.ToString());
			MessageBox.Show(exc.ToString());
		}

		private void frmSchedule_Load(object sender, System.EventArgs e)
		{
			DataTable dtExecutionStatus;
			DataTable dtCompletionStatus;
			Infragistics.Win.ValueList valList;
			Infragistics.Win.ValueListItem valListItem;

			try
			{
				_browserSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerBrowser);

				_dlSchedule = new ScheduleData();
				_gridLock = new MIDReaderWriterLock();

				dtExecutionStatus = MIDText.GetTextType(eMIDTextType.eProcessExecutionStatus, eMIDTextOrderBy.TextCode);

				valList = ulgSchedule.DisplayLayout.ValueLists.Add("ExecutionStatus");
			
				foreach (DataRow row in dtExecutionStatus.Rows)
				{
					valListItem = new Infragistics.Win.ValueListItem();
					valListItem.DataValue= Convert.ToInt32(row["TEXT_CODE"]);
					valListItem.DisplayText = Convert.ToString(row["TEXT_VALUE"]);
					valList.ValueListItems.Add(valListItem);
				}

				dtCompletionStatus = MIDText.GetTextType(eMIDTextType.eProcessCompletionStatus, eMIDTextOrderBy.TextCode);

				valList = ulgSchedule.DisplayLayout.ValueLists.Add("CompletionStatus");
			
				foreach (DataRow row in dtCompletionStatus.Rows)
				{
					valListItem = new Infragistics.Win.ValueListItem();
					valListItem.DataValue= Convert.ToInt32(row["TEXT_CODE"]);
					valListItem.DisplayText = Convert.ToString(row["TEXT_VALUE"]);
					valList.ValueListItems.Add(valListItem);
				}

				LoadData();
				BindGridData();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void frmScheduleBrowser_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				if (_browserRefresh != null)
				{
					_browserRefresh.EndRefresh();
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void btnRefresh_Click(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				Refresh();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void chkRefresh_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (chkRefresh.Checked)
				{
					lblRefresh1.Enabled = true;
					nudRefresh.Enabled = true;
					lblRefresh2.Enabled = true;

					_browserRefresh = new BrowserRefresh(this, (int)nudRefresh.Value);
				}
				else
				{
					lblRefresh1.Enabled = false;
					nudRefresh.Enabled = false;
					lblRefresh2.Enabled = false;

					_browserRefresh.EndRefresh();
					_browserRefresh = null;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void nudRefresh_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (_browserRefresh != null)
				{
					_browserRefresh.SleepTime = (int)nudRefresh.Value;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ctxSchedule_Popup(object sender, System.EventArgs e)
		{
			try
			{
				mniHold.Visible = false;
				mniRunNow.Visible = false;
				mniCancel.Visible = false;
				mniSep1.Visible = false;
				mniProperties.Visible = false;

				if (ulgSchedule.Selected.Rows.Count == 1)
				{
					if (_browserSecLvl.AllowUpdate)
					{
						switch ((eProcessExecutionStatus)(int)ulgSchedule.Selected.Rows[0].Cells["EXECUTION_STATUS"].Value)
						{
							case eProcessExecutionStatus.Waiting :

								mniHold.Visible = true;
								mniHold.Checked = false;
								mniRunNow.Visible = true;
								mniCancel.Visible = true;
								mniSep1.Visible = true;
								break;

							case eProcessExecutionStatus.OnHold :

								mniHold.Visible = true;
								mniHold.Checked = true;
								mniCancel.Visible = true;
								mniSep1.Visible = true;
								break;

							case eProcessExecutionStatus.Running :

								mniCancel.Visible = true;
								mniSep1.Visible = true;
								break;

							case eProcessExecutionStatus.Cancelled :

								mniRunNow.Visible = true;
								mniSep1.Visible = true;
								break;

							case eProcessExecutionStatus.Completed :

								mniRunNow.Visible = true;
								mniSep1.Visible = true;
								break;

							case eProcessExecutionStatus.InError :

								mniRunNow.Visible = true;
								mniCancel.Visible = true;
								mniSep1.Visible = true;
								break;
						}
					}

					mniProperties.Visible = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void mniHold_Click(object sender, System.EventArgs e)
		{
			ScheduleProfile schedProf;
			JobProfile jobProf;

			Cursor.Current = Cursors.WaitCursor;

			try
			{
				_gridLock.AcquireReaderLock(SAB.ClientServerSession.ReaderLockTimeOut);

				try
				{
					if (ulgSchedule.Selected.Rows.Count == 1)
					{
						schedProf = new ScheduleProfile(_dlSchedule.Schedule_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["SCHED_RID"].Value)));
						jobProf = new JobProfile(_dlSchedule.Job_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["JOB_RID"].Value)));

						switch ((eProcessExecutionStatus)(int)ulgSchedule.Selected.Rows[0].Cells["EXECUTION_STATUS"].Value)
						{
							case eProcessExecutionStatus.Waiting :

								SAB.SchedulerServerSession.HoldJob(schedProf.Key, jobProf.Key, _SAB.ClientServerSession.UserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
								break;

							case eProcessExecutionStatus.OnHold :

                                SAB.SchedulerServerSession.ResumeJob(schedProf.Key, jobProf.Key, _SAB.ClientServerSession.UserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
								break;
						}

						Refresh();
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_gridLock.ReleaseReaderLock();
				}
			}
			catch (InvalidJobStatusForAction)
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidStatusForAction), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (JobDoesNotExist)
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_JobDoesNotExist), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void mniRunNow_Click(object sender, System.EventArgs e)
		{
			ScheduleProfile schedProf;
			JobProfile jobProf;

			Cursor.Current = Cursors.WaitCursor;

			try
			{
				_gridLock.AcquireReaderLock(SAB.ClientServerSession.ReaderLockTimeOut);

				try
				{
					if (ulgSchedule.Selected.Rows.Count == 1)
					{
						schedProf = new ScheduleProfile(_dlSchedule.Schedule_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["SCHED_RID"].Value)));
						jobProf = new JobProfile(_dlSchedule.Job_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["JOB_RID"].Value)));
// Begin Alert Events Code -- DO NOT REMOVE
						SAB.SchedulerServerSession.RunJobNow(schedProf.Key, jobProf.Key);
//						SAB.SchedulerServerSession.RunJobNow(schedProf.Key, jobProf.Key, new JobFinishAlertEvent(new AlertEventHandler(SAB.MessageCallback.HandleAlert)));
// End Alert Events Code -- DO NOT REMOVE
						Refresh();
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_gridLock.ReleaseReaderLock();
				}
			}
			catch (InvalidJobStatusForAction)
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidStatusForAction), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (JobDoesNotExist)
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_JobDoesNotExist), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void mniCancel_Click(object sender, System.EventArgs e)
		{
			ScheduleProfile schedProf;
			JobProfile jobProf;
			eProcessExecutionStatus currProcStat;

			Cursor.Current = Cursors.WaitCursor;

			try
			{
				_gridLock.AcquireReaderLock(SAB.ClientServerSession.ReaderLockTimeOut);

				try
				{
					if (ulgSchedule.Selected.Rows.Count == 1)
					{
						schedProf = new ScheduleProfile(_dlSchedule.Schedule_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["SCHED_RID"].Value)));
						jobProf = new JobProfile(_dlSchedule.Job_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["JOB_RID"].Value)));
						currProcStat = SAB.SchedulerServerSession.GetJobStatus(schedProf.Key, jobProf.Key);

						if (currProcStat == eProcessExecutionStatus.Running)
						{
							if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CancelRunningJob), Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
							{
								SAB.SchedulerServerSession.CancelJob(schedProf.Key, jobProf.Key, currProcStat);
							}
						}
						else
						{
							SAB.SchedulerServerSession.CancelJob(schedProf.Key, jobProf.Key, currProcStat);
						}

						Refresh();
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_gridLock.ReleaseReaderLock();
				}
			}
			catch (InvalidJobStatusForAction)
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidStatusForAction), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (JobDoesNotExist)
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_JobDoesNotExist), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void mniProperties_Click(object sender, System.EventArgs e)
		{
			ScheduleProfile schedProf;
			JobProfile jobProf;
//			frmScheduleProperties schedProperties;

			Cursor.Current = Cursors.WaitCursor;

			try
			{
				_gridLock.AcquireReaderLock(SAB.ClientServerSession.ReaderLockTimeOut);

				try
				{
					if (ulgSchedule.Selected.Rows.Count == 1)
					{
						schedProf = new ScheduleProfile(_dlSchedule.Schedule_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["SCHED_RID"].Value)));
						jobProf = new JobProfile(_dlSchedule.Job_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["JOB_RID"].Value)));

						schedProperties = new frmScheduleProperties(SAB, _browserSecLvl, schedProf, jobProf);
						schedProperties.OnSchedulePropertiesSaveHandler += new frmScheduleProperties.SchedulePropertiesSaveEventHandler(OnSchedulePropertiesSave);

						if (this.ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer")
						{
							schedProperties.MdiParent = this.ParentForm;
						}
						else
						{
							schedProperties.MdiParent = this.ParentForm.Owner;
						}

						schedProperties.Show();
						schedProperties.BringToFront();
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_gridLock.ReleaseReaderLock();
				}
			}
			catch (InvalidJobStatusForAction)
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_InvalidStatusForAction), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (JobDoesNotExist)
			{
				MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_JobDoesNotExist), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

//Begin Track #4876 - JScott - Delete key removes entry from grid but not from DB
		private void ulgSchedule_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
			e.Cancel = true;
		}

//End Track #4876 - JScott - Delete key removes entry from grid but not from DB
		private void LoadData()
		{
			int i;

			try
			{
				_dtScheduledJobs = SAB.SchedulerServerSession.GetSchedule();

				_dtScheduledJobs.Columns.Add("Scheduled By", typeof(string));

				for (i = 0; i < _dtScheduledJobs.Rows.Count; i++)
				{
					_dtScheduledJobs.Rows[i]["Scheduled By"] = SAB.ClientServerSession.GetUserName(Convert.ToInt32(_dtScheduledJobs.Rows[i]["USER_RID"]));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		private void BindGridData()
		{
			try
			{
				_gridLock.AcquireWriterLock(SAB.ClientServerSession.WriterLockTimeOut);

				try
				{
					ulgSchedule.DataSource = _dtScheduledJobs;

					ulgSchedule.DisplayLayout.GroupByBox.Hidden = false;
					ulgSchedule.DisplayLayout.Override.SelectTypeRow = SelectType.Single;
					ulgSchedule.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
					ulgSchedule.DisplayLayout.Bands[0].ColHeaderLines = 2;

					ulgSchedule.DisplayLayout.Bands[0].Columns["SCHED_NAME"].Header.Caption = "Schedule Name";
					ulgSchedule.DisplayLayout.Bands[0].Columns["SCHED_NAME"].CellActivation = Activation.NoEdit;
					ulgSchedule.DisplayLayout.Bands[0].Columns["JOB_NAME"].Header.Caption = "Job Name";
					ulgSchedule.DisplayLayout.Bands[0].Columns["JOB_NAME"].CellActivation = Activation.NoEdit;
					ulgSchedule.DisplayLayout.Bands[0].Columns["EXECUTION_STATUS"].Header.Caption = "Execution\nStatus";
					ulgSchedule.DisplayLayout.Bands[0].Columns["EXECUTION_STATUS"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					ulgSchedule.DisplayLayout.Bands[0].Columns["EXECUTION_STATUS"].ValueList = ulgSchedule.DisplayLayout.ValueLists["ExecutionStatus"];
					ulgSchedule.DisplayLayout.Bands[0].Columns["EXECUTION_STATUS"].CellActivation = Activation.NoEdit;
					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_STATUS"].Header.Caption = "Last Completion\nStatus";
					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_STATUS"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_STATUS"].ValueList = ulgSchedule.DisplayLayout.ValueLists["CompletionStatus"];
					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_STATUS"].CellActivation = Activation.NoEdit;
					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_RUN_DATETIME"].Header.Caption = "Last Run\nDate";
					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_RUN_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";
					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_RUN_DATETIME"].CellActivation = Activation.NoEdit;
					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_DATETIME"].Header.Caption = "Last Completion\nDate";
					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";
					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_DATETIME"].CellActivation = Activation.NoEdit;
					ulgSchedule.DisplayLayout.Bands[0].Columns["NEXT_RUN_DATETIME"].Header.Caption = "Next Run\nDate";
					ulgSchedule.DisplayLayout.Bands[0].Columns["NEXT_RUN_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";
					ulgSchedule.DisplayLayout.Bands[0].Columns["NEXT_RUN_DATETIME"].CellActivation = Activation.NoEdit;
					ulgSchedule.DisplayLayout.Bands[0].Columns["REPEAT_UNTIL_DATETIME"].Header.Caption = "Repeat\nUntil";
					ulgSchedule.DisplayLayout.Bands[0].Columns["REPEAT_UNTIL_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";
					ulgSchedule.DisplayLayout.Bands[0].Columns["REPEAT_UNTIL_DATETIME"].CellActivation = Activation.NoEdit;
					ulgSchedule.DisplayLayout.Bands[0].Columns["Scheduled By"].Header.Caption = "Scheduled\nBy";
					ulgSchedule.DisplayLayout.Bands[0].Columns["SCHED_RID"].Hidden = true;
					ulgSchedule.DisplayLayout.Bands[0].Columns["JOB_RID"].Hidden = true;
					ulgSchedule.DisplayLayout.Bands[0].Columns["USER_RID"].Hidden = true;
					ulgSchedule.DisplayLayout.Bands[0].Columns["CURRENT_TASKLIST_RID"].Hidden = true;
					ulgSchedule.DisplayLayout.Bands[0].Columns["CURRENT_TASKLIST_SEQUENCE"].Hidden = true;
                    ulgSchedule.DisplayLayout.Bands[0].Columns["HOLD_BY_USER_RID"].Hidden = true;
                    ulgSchedule.DisplayLayout.Bands[0].Columns["HOLD_BY_DATETIME"].Hidden = true;
                    ulgSchedule.DisplayLayout.Bands[0].Columns["RELEASED_BY_USER_RID"].Hidden = true;
                    ulgSchedule.DisplayLayout.Bands[0].Columns["RELEASED_BY_DATETIME"].Hidden = true;

// Begin Alert Events Code -- DO NOT REMOVE
//					ulgSchedule.DisplayLayout.Bands[0].Columns["JobFinishAlertEvent"].Hidden = true;
// End Alert Events Code -- DO NOT REMOVE

					ResizeGridColumns();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_gridLock.ReleaseWriterLock();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override public void Refresh()
		{
			MemoryStream layoutStream = null;

			try
			{
				_gridLock.AcquireWriterLock(SAB.ClientServerSession.WriterLockTimeOut);

				try
				{
					base.Refresh();

					Cursor.Current = Cursors.WaitCursor;

					if (ulgSchedule.DisplayLayout != null)
					{
						layoutStream = new MemoryStream(); 
						ulgSchedule.DisplayLayout.Save(layoutStream, Infragistics.Win.UltraWinGrid.PropertyCategories.All);
					}

					ulgSchedule.DataSource = null;
					ulgSchedule.Refresh();
					ulgSchedule.BeginUpdate();
					LoadData();

					ulgSchedule.DataSource = _dtScheduledJobs;

					if (layoutStream != null)
					{
						layoutStream.Position = 0;
						ulgSchedule.DisplayLayout.Load(layoutStream);
					}

					ResizeGridColumns();

					ulgSchedule.EndUpdate();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_gridLock.ReleaseWriterLock();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		public void ShowJob(int aJobRID)
		{
			bool found = false;
			
			try
			{
				foreach(UltraGridRow row in ulgSchedule.Rows)
				{
					LocateJob(row, aJobRID, ref found);
					if (found)
					{
						return;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void ResizeGridColumns()
		{
			try
			{
				ulgSchedule.DisplayLayout.Bands[0].Columns["SCHED_NAME"].Width = 100;
				ulgSchedule.DisplayLayout.Bands[0].Columns["JOB_NAME"].Width = 100;
				ulgSchedule.DisplayLayout.Bands[0].Columns["EXECUTION_STATUS"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_STATUS"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_RUN_DATETIME"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_DATETIME"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgSchedule.DisplayLayout.Bands[0].Columns["NEXT_RUN_DATETIME"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgSchedule.DisplayLayout.Bands[0].Columns["REPEAT_UNTIL_DATETIME"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgSchedule.DisplayLayout.Bands[0].Columns["Scheduled By"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void LocateJob(UltraGridRow row, int aJobRID, ref bool aFound)
		{
			try
			{
				if (row.HasChild(null))
				{
					foreach (UltraGridChildBand cb in row.ChildBands)
					{
						foreach (UltraGridRow childRow in cb.Rows)
						{
							LocateJob(childRow, aJobRID, ref aFound);
							if (aFound)
							{
								return;
							}
						}
					}
				}
				else
				{
					if (Convert.ToInt32(row.Cells["JOB_RID"].Value, CultureInfo.CurrentUICulture) == aJobRID)
					{
						ulgSchedule.Selected.Rows.Clear();
						row.Selected = true;
						ulgSchedule.ActiveRow = row;
						row.Expanded = true;
						aFound = true;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void OnSchedulePropertiesSave(object source, SchedulePropertiesSaveEventArgs e)
		{
			try
			{
				Refresh();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void frmScheduleBrowser_Closed(object sender, System.EventArgs e)
		{
			try
			{
				this.Closed -= new System.EventHandler(this.frmScheduleBrowser_Closed);
                this.ulgSchedule.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgSchedule_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ulgSchedule);
                //End TT#169
			}
			catch
			{

			}
		}

        private void ulgSchedule_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            // End TT#1164
            //End TT#169
        }
	}

	public class BrowserRefresh
	{
		private frmScheduleBrowser _browserForm;
		private int _sleepTime;
		private bool _endRefreshThread;
		private Thread _refreshThread;

		public BrowserRefresh(frmScheduleBrowser aBrowserForm, int aSleepTime)
		{
			try
			{
				_browserForm = aBrowserForm;
				_sleepTime = aSleepTime;

				_endRefreshThread = false;
				_refreshThread = new Thread(new ThreadStart(RefreshThread));
				_refreshThread.Start();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public int SleepTime
		{
			set
			{
				_sleepTime = value;
			}
		}

		public void EndRefresh()
		{
			_endRefreshThread = true;
		}

		private void RefreshThread()
		{
			int i;

			try
			{
				while (true)
				{
					try
					{
						_browserForm.Refresh();
					}
					catch
					{
					}

					for (i = 0; i < _sleepTime / 2; i++)
					{
						Thread.Sleep(2000);

						if (_endRefreshThread)
						{
							throw new EndScheduleBrowserRefreshThreadException();
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
