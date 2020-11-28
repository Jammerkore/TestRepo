using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
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
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmScheduleBrowserFlexGrid : MIDFormBase
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
		private MIDRetail.Windows.Controls.MIDFlexGroup fgSchedule;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScheduleBrowserFlexGrid));
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
            this.fgSchedule = new MIDRetail.Windows.Controls.MIDFlexGroup();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ulgSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fgSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fgSchedule.Grid)).BeginInit();
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
            this.ulgSchedule.Location = new System.Drawing.Point(8, 8);
            this.ulgSchedule.Name = "ulgSchedule";
            this.ulgSchedule.Size = new System.Drawing.Size(768, 432);
            this.ulgSchedule.TabIndex = 0;
            this.ulgSchedule.Text = "Schedule";
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
            // fgSchedule
            // 
            this.fgSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fgSchedule.BackColor = System.Drawing.SystemColors.ControlDark;
            this.fgSchedule.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.fgSchedule.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            // 
            // 
            // 
            this.fgSchedule.Grid.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Nodes;
            this.fgSchedule.Grid.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.fgSchedule.Grid.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.fgSchedule.Grid.ColumnInfo = "10,1,0,0,0,85,Columns:0{Width:17;}\t1{Style:\"TextAlign:LeftCenter;\";}\t";
            this.fgSchedule.Grid.ContextMenu = this.ctxSchedule;
            this.fgSchedule.Grid.Cursor = System.Windows.Forms.Cursors.Default;
            this.fgSchedule.Grid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fgSchedule.Grid.DrawMode = C1.Win.C1FlexGrid.DrawModeEnum.OwnerDraw;
            this.fgSchedule.Grid.Location = new System.Drawing.Point(0, 34);
            this.fgSchedule.Grid.Name = "";
            this.fgSchedule.Grid.Rows.DefaultSize = 17;
            this.fgSchedule.Grid.ShowCursor = true;
            this.fgSchedule.Grid.Size = new System.Drawing.Size(764, 394);
            this.fgSchedule.Grid.StyleInfo = resources.GetString("fgSchedule.Grid.StyleInfo");
            this.fgSchedule.Grid.TabIndex = 1;
            this.fgSchedule.Grid.Tree.Column = 1;
            this.fgSchedule.Grid.Tree.Style = C1.Win.C1FlexGrid.TreeStyleFlags.Symbols;
            this.fgSchedule.GroupMessage = "Drag column headers here to create groups";
            this.fgSchedule.Groups = "";
            this.fgSchedule.Image = null;
            this.fgSchedule.Location = new System.Drawing.Point(8, 8);
            this.fgSchedule.Name = "fgSchedule";
            this.fgSchedule.Size = new System.Drawing.Size(768, 432);
            this.fgSchedule.TabIndex = 42;
            this.fgSchedule.TabStop = false;
            // 
            // frmScheduleBrowserFlexGrid
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(784, 478);
            this.Controls.Add(this.fgSchedule);
            this.Controls.Add(this.nudRefresh);
            this.Controls.Add(this.lblRefresh1);
            this.Controls.Add(this.lblRefresh2);
            this.Controls.Add(this.chkRefresh);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.ulgSchedule);
            this.Name = "frmScheduleBrowserFlexGrid";
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
            this.Controls.SetChildIndex(this.fgSchedule, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ulgSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fgSchedule.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fgSchedule)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private SessionAddressBlock _SAB;
		private FunctionSecurityProfile _systemSecLvl;
		private ScheduleData _dlSchedule;
		private DataTable _dtScheduledJobs;
		private BrowserRefreshFlexGrid _browserRefresh;
		private MIDReaderWriterLock _gridLock;
		// FLEXGRID TEST
		private ListDictionary _executionStatusMap;
		private ListDictionary _completionStatusMap;
		// END FLEXGRID TEST

		public frmScheduleBrowserFlexGrid(SessionAddressBlock aSAB) : base(aSAB)
		{
			try
			{
				InitializeComponent();

				_SAB = aSAB;
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
			// FLEXGRID TEST
			//			Infragistics.Win.ValueList valList;
			//			Infragistics.Win.ValueListItem valListItem;
			// END FLEXGRID TEST

			try
			{
				_systemSecLvl = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerSystem);

				_dlSchedule = new ScheduleData();
				_gridLock = new MIDReaderWriterLock();

				dtExecutionStatus = MIDText.GetTextType(eMIDTextType.eProcessExecutionStatus, eMIDTextOrderBy.TextCode);

				// FLEXGRID TEST
				//				valList = ulgSchedule.DisplayLayout.ValueLists.Add("ExecutionStatus");
				//			
				//				foreach (DataRow row in dtExecutionStatus.Rows)
				//				{
				//					valListItem = new Infragistics.Win.ValueListItem();
				//					valListItem.DataValue= Convert.ToInt32(row["TEXT_CODE"]);
				//					valListItem.DisplayText = Convert.ToString(row["TEXT_VALUE"]);
				//					valList.ValueListItems.Add(valListItem);
				//				}
				_executionStatusMap = new ListDictionary();

				foreach (DataRow row in dtExecutionStatus.Rows)
				{
					_executionStatusMap.Add(Convert.ToInt32(row["TEXT_CODE"]), Convert.ToString(row["TEXT_VALUE"]));
				}
				// END FLEXGRID TEST

				dtCompletionStatus = MIDText.GetTextType(eMIDTextType.eProcessCompletionStatus, eMIDTextOrderBy.TextCode);

				// FLEXGRID TEST
				//				valList = ulgSchedule.DisplayLayout.ValueLists.Add("CompletionStatus");
				//			
				//				foreach (DataRow row in dtCompletionStatus.Rows)
				//				{
				//					valListItem = new Infragistics.Win.ValueListItem();
				//					valListItem.DataValue= Convert.ToInt32(row["TEXT_CODE"]);
				//					valListItem.DisplayText = Convert.ToString(row["TEXT_VALUE"]);
				//					valList.ValueListItems.Add(valListItem);
				//				}
				_completionStatusMap = new ListDictionary();

				foreach (DataRow row in dtCompletionStatus.Rows)
				{
					_completionStatusMap.Add(Convert.ToInt32(row["TEXT_CODE"]), Convert.ToString(row["TEXT_VALUE"]));
				}

				_dtScheduledJobs = _SAB.SchedulerServerSession.GetSchedule();
				_dtScheduledJobs.Columns.Add("Scheduled By", typeof(string));
				// END FLEXGRID TEST

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

					_browserRefresh = new BrowserRefreshFlexGrid(this, (int)nudRefresh.Value);
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

				// FLEXGRID TEST
				//				if (ulgSchedule.Selected.Rows.Count == 1)
				if (fgSchedule.Grid.Selection.IsValid)
					// END FLEXGRID TEST
				{
					if (_systemSecLvl.AllowUpdate)
					{
						// FLEXGRID TEST
						//						switch ((eProcessExecutionStatus)(int)ulgSchedule.Selected.Rows[0].Cells["EXECUTION_STATUS"].Value)
						switch ((eProcessExecutionStatus)(int)fgSchedule.Grid.GetData(fgSchedule.Grid.Row, "EXECUTION_STATUS"))
								// END FLEXGRID TEST
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
				_gridLock.AcquireReaderLock(_SAB.ClientServerSession.ReaderLockTimeOut);

				try
				{
					// FLEXGRID TEST
					//					if (ulgSchedule.Selected.Rows.Count == 1)
					if (fgSchedule.Grid.Selection.IsValid)
						// END FLEXGRID TEST
					{
						// FLEXGRID TEST
						//						schedProf = new ScheduleProfile(_dlSchedule.Schedule_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["SCHED_RID"].Value)));
						//						jobProf = new JobProfile(_dlSchedule.Job_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["JOB_RID"].Value)));
						schedProf = new ScheduleProfile(_dlSchedule.Schedule_Read(Convert.ToInt32(fgSchedule.Grid.GetData(fgSchedule.Grid.Row, "SCHED_RID"))));
						jobProf = new JobProfile(_dlSchedule.Job_Read(Convert.ToInt32(fgSchedule.Grid.GetData(fgSchedule.Grid.Row, "JOB_RID"))));
						// END FLEXGRID TEST

						// FLEXGRID TEST
						//						switch ((eProcessExecutionStatus)(int)ulgSchedule.Selected.Rows[0].Cells["EXECUTION_STATUS"].Value)
						switch ((eProcessExecutionStatus)(int)fgSchedule.Grid.GetData(fgSchedule.Grid.Row, "EXECUTION_STATUS"))
								// END FLEXGRID TEST
						{
							case eProcessExecutionStatus.Waiting :

                                _SAB.SchedulerServerSession.HoldJob(schedProf.Key, jobProf.Key, _SAB.ClientServerSession.UserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
								break;

							case eProcessExecutionStatus.OnHold :

                                _SAB.SchedulerServerSession.ResumeJob(schedProf.Key, jobProf.Key, _SAB.ClientServerSession.UserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
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
				_gridLock.AcquireReaderLock(_SAB.ClientServerSession.ReaderLockTimeOut);

				try
				{
					// FLEXGRID TEST
					//					if (ulgSchedule.Selected.Rows.Count == 1)
					if (fgSchedule.Grid.Selection.IsValid)
						// END FLEXGRID TEST
					{
						// FLEXGRID TEST
						//						schedProf = new ScheduleProfile(_dlSchedule.Schedule_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["SCHED_RID"].Value)));
						//						jobProf = new JobProfile(_dlSchedule.Job_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["JOB_RID"].Value)));
						schedProf = new ScheduleProfile(_dlSchedule.Schedule_Read(Convert.ToInt32(fgSchedule.Grid.GetData(fgSchedule.Grid.Row, "SCHED_RID"))));
						jobProf = new JobProfile(_dlSchedule.Job_Read(Convert.ToInt32(fgSchedule.Grid.GetData(fgSchedule.Grid.Row, "JOB_RID"))));
						// END FLEXGRID TEST
// Begin Alert Events Code -- DO NOT REMOVE
						_SAB.SchedulerServerSession.RunJobNow(schedProf.Key, jobProf.Key);
//						_SAB.SchedulerServerSession.RunJobNow(schedProf.Key, jobProf.Key, new JobFinishAlertEvent(new AlertEventHandler(_SAB.MessageCallback.HandleAlert)));
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
				_gridLock.AcquireReaderLock(_SAB.ClientServerSession.ReaderLockTimeOut);

				try
				{
					// FLEXGRID TEST
					//					if (ulgSchedule.Selected.Rows.Count == 1)
					if (fgSchedule.Grid.Selection.IsValid)
						// END FLEXGRID TEST
					{
						// FLEXGRID TEST
						//						schedProf = new ScheduleProfile(_dlSchedule.Schedule_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["SCHED_RID"].Value)));
						//						jobProf = new JobProfile(_dlSchedule.Job_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["JOB_RID"].Value)));
						schedProf = new ScheduleProfile(_dlSchedule.Schedule_Read(Convert.ToInt32(fgSchedule.Grid.GetData(fgSchedule.Grid.Row, "SCHED_RID"))));
						jobProf = new JobProfile(_dlSchedule.Job_Read(Convert.ToInt32(fgSchedule.Grid.GetData(fgSchedule.Grid.Row, "JOB_RID"))));
						// END FLEXGRID TEST
						currProcStat = _SAB.SchedulerServerSession.GetJobStatus(schedProf.Key, jobProf.Key);

						if (currProcStat == eProcessExecutionStatus.Running)
						{
							if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CancelRunningJob), Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
							{
								_SAB.SchedulerServerSession.CancelJob(schedProf.Key, jobProf.Key, currProcStat);
							}
						}
						else
						{
							_SAB.SchedulerServerSession.CancelJob(schedProf.Key, jobProf.Key, currProcStat);
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

			Cursor.Current = Cursors.WaitCursor;

			try
			{
				_gridLock.AcquireReaderLock(_SAB.ClientServerSession.ReaderLockTimeOut);

				try
				{
					// FLEXGRID TEST
					//					if (ulgSchedule.Selected.Rows.Count == 1)
					if (fgSchedule.Grid.Selection.IsValid)
						// END FLEXGRID TEST
					{
						// FLEXGRID TEST
						//						schedProf = new ScheduleProfile(_dlSchedule.Schedule_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["SCHED_RID"].Value)));
						//						jobProf = new JobProfile(_dlSchedule.Job_Read(Convert.ToInt32(ulgSchedule.Selected.Rows[0].Cells["JOB_RID"].Value)));
						schedProf = new ScheduleProfile(_dlSchedule.Schedule_Read(Convert.ToInt32(fgSchedule.Grid.GetData(fgSchedule.Grid.Row, "SCHED_RID"))));
						jobProf = new JobProfile(_dlSchedule.Job_Read(Convert.ToInt32(fgSchedule.Grid.GetData(fgSchedule.Grid.Row, "JOB_RID"))));
						// END FLEXGRID TEST

						schedProperties = new frmScheduleProperties(_SAB, _systemSecLvl, schedProf, jobProf);
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

		private void LoadData()
		{
			// FLEXGRID TEST
			//			int i;
			DataTable schedJobs;
			DataRow newRow;
			// END FLEXGRID TEST

			try
			{
				// FLEXGRID TEST
				//				_dtScheduledJobs = _SAB.SchedulerServerSession.GetSchedule();
				//
				//				_dtScheduledJobs.Columns.Add("Scheduled By", typeof(string));
				//
				//				for (i = 0; i < _dtScheduledJobs.Rows.Count; i++)
				//				{
				//					_dtScheduledJobs.Rows[i]["Scheduled By"] = _SAB.ClientServerSession.GetUserName(Convert.ToInt32(_dtScheduledJobs.Rows[i]["USER_RID"]));
				//				}
				//
				//				_dtScheduledJobs.AcceptChanges();
				_dtScheduledJobs.BeginLoadData();
				_dtScheduledJobs.Clear();
				schedJobs = _SAB.SchedulerServerSession.GetSchedule();

				foreach (DataRow row in schedJobs.Rows)
				{
					newRow = _dtScheduledJobs.LoadDataRow(row.ItemArray, false);
					newRow["Scheduled By"] = _SAB.ClientServerSession.GetUserName(Convert.ToInt32(newRow["USER_RID"]));
				}

				_dtScheduledJobs.AcceptChanges();
				_dtScheduledJobs.EndLoadData();
				// END FLEXGRID TEST
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
				_gridLock.AcquireWriterLock(_SAB.ClientServerSession.WriterLockTimeOut);

				try
				{
					// FLEXGRID TEST
					//					ulgSchedule.DataSource = _dtScheduledJobs;
					//
					//					ulgSchedule.DisplayLayout.GroupByBox.Hidden = false;
					//					ulgSchedule.DisplayLayout.Override.SelectTypeRow = SelectType.Single;
					//					ulgSchedule.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
					//					ulgSchedule.DisplayLayout.Bands[0].ColHeaderLines = 2;
					//
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["SCHED_NAME"].Header.Caption = "Schedule Name";
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["SCHED_NAME"].CellActivation = Activation.NoEdit;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["JOB_NAME"].Header.Caption = "Job Name";
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["JOB_NAME"].CellActivation = Activation.NoEdit;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["EXECUTION_STATUS"].Header.Caption = "Execution\nStatus";
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["EXECUTION_STATUS"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["EXECUTION_STATUS"].ValueList = ulgSchedule.DisplayLayout.ValueLists["ExecutionStatus"];
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["EXECUTION_STATUS"].CellActivation = Activation.NoEdit;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_STATUS"].Header.Caption = "Last Completion\nStatus";
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_STATUS"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_STATUS"].ValueList = ulgSchedule.DisplayLayout.ValueLists["CompletionStatus"];
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_STATUS"].CellActivation = Activation.NoEdit;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_RUN_DATETIME"].Header.Caption = "Last Run\nDate";
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_RUN_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_RUN_DATETIME"].CellActivation = Activation.NoEdit;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_DATETIME"].Header.Caption = "Last Completion\nDate";
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_DATETIME"].CellActivation = Activation.NoEdit;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["NEXT_RUN_DATETIME"].Header.Caption = "Next Run\nDate";
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["NEXT_RUN_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["NEXT_RUN_DATETIME"].CellActivation = Activation.NoEdit;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["REPEAT_UNTIL_DATETIME"].Header.Caption = "Repeat\nUntil";
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["REPEAT_UNTIL_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["REPEAT_UNTIL_DATETIME"].CellActivation = Activation.NoEdit;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["Scheduled By"].Header.Caption = "Scheduled\nBy";
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["SCHED_RID"].Hidden = true;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["JOB_RID"].Hidden = true;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["USER_RID"].Hidden = true;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["CURRENT_TASKLIST_RID"].Hidden = true;
					//					ulgSchedule.DisplayLayout.Bands[0].Columns["CURRENT_TASKLIST_SEQUENCE"].Hidden = true;
					//
					//					ResizeGridColumns();
					fgSchedule.Grid.DataSource = _dtScheduledJobs;

					fgSchedule.Grid.Cols["SCHED_RID"].Visible = false;
					fgSchedule.Grid.Cols["JOB_RID"].Visible = false;
					fgSchedule.Grid.Cols["USER_RID"].Visible = false;
					fgSchedule.Grid.Cols["CURRENT_TASKLIST_RID"].Visible = false;
					fgSchedule.Grid.Cols["CURRENT_TASKLIST_SEQUENCE"].Visible = false;
// Begin Alert Events Code -- DO NOT REMOVE
//					fgSchedule.Grid.Cols["JobFinishAlertEvent"].Visible = false;
// End Alert Events Code -- DO NOT REMOVE

					fgSchedule.Grid.Cols["SCHED_NAME"].Caption = "Schedule Name";
					fgSchedule.Grid.Cols["JOB_NAME"].Caption = "Job Name";
					fgSchedule.Grid.Cols["EXECUTION_STATUS"].Caption = "Execution\nStatus";
					fgSchedule.Grid.Cols["LAST_COMPLETION_STATUS"].Caption = "Last Completion\nStatus";
					fgSchedule.Grid.Cols["LAST_RUN_DATETIME"].Caption = "Last Run\nDate";
					fgSchedule.Grid.Cols["LAST_COMPLETION_DATETIME"].Caption = "Last Completion\nDate";
					fgSchedule.Grid.Cols["NEXT_RUN_DATETIME"].Caption = "Next Run\nDate";
					fgSchedule.Grid.Cols["REPEAT_UNTIL_DATETIME"].Caption = "Repeat\nUntil";
					fgSchedule.Grid.Cols["Scheduled By"].Caption = "Scheduled\nBy";

					fgSchedule.Grid.Cols["SCHED_NAME"].AllowEditing = false;
					fgSchedule.Grid.Cols["JOB_NAME"].AllowEditing = false;
					fgSchedule.Grid.Cols["EXECUTION_STATUS"].AllowEditing = false;
					fgSchedule.Grid.Cols["LAST_COMPLETION_STATUS"].AllowEditing = false;
					fgSchedule.Grid.Cols["LAST_RUN_DATETIME"].AllowEditing = false;
					fgSchedule.Grid.Cols["LAST_COMPLETION_DATETIME"].AllowEditing = false;
					fgSchedule.Grid.Cols["NEXT_RUN_DATETIME"].AllowEditing = false;
					fgSchedule.Grid.Cols["REPEAT_UNTIL_DATETIME"].AllowEditing = false;
				
					fgSchedule.Grid.Cols["LAST_RUN_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";
					fgSchedule.Grid.Cols["LAST_COMPLETION_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";
					fgSchedule.Grid.Cols["NEXT_RUN_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";
					fgSchedule.Grid.Cols["REPEAT_UNTIL_DATETIME"].Format = "MM/dd/yyyy hh:mm:ss tt";

					fgSchedule.Grid.Cols["EXECUTION_STATUS"].DataType = typeof(int);
					fgSchedule.Grid.Cols["EXECUTION_STATUS"].DataMap = _executionStatusMap;
					fgSchedule.Grid.Cols["LAST_COMPLETION_STATUS"].DataType = typeof(int);
					fgSchedule.Grid.Cols["LAST_COMPLETION_STATUS"].DataMap = _completionStatusMap;
					// END FLEXGRID TEST

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
			// FLEXGRID TEST
			//			MemoryStream layoutStream = null;
			// END FLEXGRID TEST

			try
			{
				_gridLock.AcquireWriterLock(_SAB.ClientServerSession.WriterLockTimeOut);

				try
				{
					base.Refresh();

					Cursor.Current = Cursors.WaitCursor;

					// FLEXGRID TEST
					//					if (ulgSchedule.DisplayLayout != null)
					//					{
					//						layoutStream = new MemoryStream(); 
					//						ulgSchedule.DisplayLayout.Save(layoutStream, Infragistics.Win.UltraWinGrid.PropertyCategories.All);
					//					}
					//
					//					ulgSchedule.DataSource = null;
					//					ulgSchedule.Refresh();
					//					ulgSchedule.BeginUpdate();
					// END FLEXGRID TEST
					LoadData();
					// FLEXGRID TEST
					//
					//					ulgSchedule.DataSource = _dtScheduledJobs;
					//
					//					if (layoutStream != null)
					//					{
					//						layoutStream.Position = 0;
					//						ulgSchedule.DisplayLayout.Load(layoutStream);
					//					}
					//
					//					ResizeGridColumns();
					//
					//					ulgSchedule.EndUpdate();
					// END FLEXGRID TEST
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
				// FLEXGRID TEST
				//				ulgSchedule.DisplayLayout.Bands[0].Columns["SCHED_NAME"].Width = 100;
				//				ulgSchedule.DisplayLayout.Bands[0].Columns["JOB_NAME"].Width = 100;
				//				ulgSchedule.DisplayLayout.Bands[0].Columns["EXECUTION_STATUS"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				//				ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_STATUS"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				//				ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_RUN_DATETIME"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				//				ulgSchedule.DisplayLayout.Bands[0].Columns["LAST_COMPLETION_DATETIME"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				//				ulgSchedule.DisplayLayout.Bands[0].Columns["NEXT_RUN_DATETIME"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				//				ulgSchedule.DisplayLayout.Bands[0].Columns["REPEAT_UNTIL_DATETIME"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				//				ulgSchedule.DisplayLayout.Bands[0].Columns["Scheduled By"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				fgSchedule.Grid.AutoSizeCols();
				fgSchedule.Grid.Cols["SCHED_NAME"].WidthDisplay = 100;
				fgSchedule.Grid.Cols["JOB_NAME"].WidthDisplay = 100;
				// END FLEXGRID TEST
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
			}
			catch
			{

			}
		}
	}

	public class BrowserRefreshFlexGrid
	{
		private frmScheduleBrowserFlexGrid _browserForm;
		private int _sleepTime;
		private bool _endRefreshThread;
		private Thread _refreshThread;

		public BrowserRefreshFlexGrid(frmScheduleBrowserFlexGrid aBrowserForm, int aSleepTime)
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
