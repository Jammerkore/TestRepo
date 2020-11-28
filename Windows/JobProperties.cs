using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
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
	/// Summary description for JobProperties.
	/// </summary>
	public class frmJobProperties : MIDFormBase
	{
		#region Windows Form Designer generated code

		private System.Windows.Forms.Label lblFilterName;
		private System.Windows.Forms.TextBox txtJobName;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private Infragistics.Win.UltraWinGrid.UltraGrid ulgJobs;
		private System.Windows.Forms.Button btnSaveAs;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnRunNow;
		private System.Windows.Forms.ContextMenu ctmTaskLists;
		private System.Windows.Forms.MenuItem mniDelete;
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
				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.ulgJobs.SelectionDrag -= new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
				this.ulgJobs.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgJobs_CellChange);
				this.ulgJobs.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
				this.ulgJobs.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgJobs_BeforeRowsDeleted);
				this.ulgJobs.DragOver -= new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
				this.ulgJobs.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgJobs_AfterCellUpdate);
                this.ulgJobs.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgJobs_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ulgJobs);
                //End TT#169
				this.ctmTaskLists.Popup -= new System.EventHandler(this.ctmTaskLists_Popup);
				this.mniDelete.Click -= new System.EventHandler(this.mniDelete_Click);
				this.btnSaveAs.Click -= new System.EventHandler(this.btnSaveAs_Click);
				this.btnRunNow.Click -= new System.EventHandler(this.btnRunNow_Click);
                this.txtJobName.TextChanged -= new System.EventHandler(this.txtJobName_TextChanged);
				this.Load -= new System.EventHandler(this.frmJobProperties_Load);
				this.Activated -= new System.EventHandler(this.frmJobProperties_Activated);
				this.Closing -= new System.ComponentModel.CancelEventHandler(this.frmJobProperties_Closing);
			}
			base.Dispose( disposing );
		}

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
            this.lblFilterName = new System.Windows.Forms.Label();
            this.txtJobName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ulgJobs = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ctmTaskLists = new System.Windows.Forms.ContextMenu();
            this.mniDelete = new System.Windows.Forms.MenuItem();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnRunNow = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ulgJobs)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // lblFilterName
            // 
            this.lblFilterName.Location = new System.Drawing.Point(8, 8);
            this.lblFilterName.Name = "lblFilterName";
            this.lblFilterName.Size = new System.Drawing.Size(64, 23);
            this.lblFilterName.TabIndex = 33;
            this.lblFilterName.Text = "Job Name:";
            this.lblFilterName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtJobName
            // 
            this.txtJobName.Location = new System.Drawing.Point(80, 8);
            this.txtJobName.Name = "txtJobName";
            this.txtJobName.Size = new System.Drawing.Size(184, 20);
            this.txtJobName.TabIndex = 34;
            this.txtJobName.TextChanged += new System.EventHandler(this.txtJobName_TextChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(280, 240);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 24);
            this.btnSave.TabIndex = 35;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(440, 240);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 24);
            this.btnOK.TabIndex = 36;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(520, 240);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 24);
            this.btnCancel.TabIndex = 37;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ulgJobs
            // 
            this.ulgJobs.AllowDrop = true;
            this.ulgJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ulgJobs.ContextMenu = this.ctmTaskLists;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ulgJobs.DisplayLayout.Appearance = appearance1;
            this.ulgJobs.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ulgJobs.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ulgJobs.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgJobs.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ulgJobs.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ulgJobs.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ulgJobs.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ulgJobs.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ulgJobs.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgJobs.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ulgJobs.Location = new System.Drawing.Point(8, 40);
            this.ulgJobs.Name = "ulgJobs";
            this.ulgJobs.Size = new System.Drawing.Size(584, 192);
            this.ulgJobs.TabIndex = 38;
            this.ulgJobs.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgJobs_AfterCellUpdate);
            this.ulgJobs.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgJobs_InitializeLayout);
            this.ulgJobs.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgJobs_CellChange);
            this.ulgJobs.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
            this.ulgJobs.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgJobs_BeforeRowsDeleted);
            this.ulgJobs.DragDrop += new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
            this.ulgJobs.DragOver += new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
            // 
            // ctmTaskLists
            // 
            this.ctmTaskLists.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniDelete});
            this.ctmTaskLists.Popup += new System.EventHandler(this.ctmTaskLists_Popup);
            // 
            // mniDelete
            // 
            this.mniDelete.Index = 0;
            this.mniDelete.Text = "Delete";
            this.mniDelete.Click += new System.EventHandler(this.mniDelete_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.Location = new System.Drawing.Point(360, 240);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(72, 24);
            this.btnSaveAs.TabIndex = 39;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnRunNow
            // 
            this.btnRunNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRunNow.Location = new System.Drawing.Point(8, 240);
            this.btnRunNow.Name = "btnRunNow";
            this.btnRunNow.Size = new System.Drawing.Size(72, 24);
            this.btnRunNow.TabIndex = 40;
            this.btnRunNow.Text = "Run Now";
            this.btnRunNow.Click += new System.EventHandler(this.btnRunNow_Click);
            // 
            // frmJobProperties
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(600, 270);
            this.Controls.Add(this.btnRunNow);
            this.Controls.Add(this.btnSaveAs);
            this.Controls.Add(this.ulgJobs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtJobName);
            this.Controls.Add(this.lblFilterName);
            this.Name = "frmJobProperties";
            this.Text = "Job Properties";
            this.Activated += new System.EventHandler(this.frmJobProperties_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmJobProperties_Closing);
            this.Load += new System.EventHandler(this.frmJobProperties_Load);
            this.Controls.SetChildIndex(this.lblFilterName, 0);
            this.Controls.SetChildIndex(this.txtJobName, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.ulgJobs, 0);
            this.Controls.SetChildIndex(this.btnSaveAs, 0);
            this.Controls.SetChildIndex(this.btnRunNow, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ulgJobs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public delegate void JobPropertiesSaveEventHandler(object source, JobPropertiesSaveEventArgs e);
		public event JobPropertiesSaveEventHandler OnJobPropertiesSaveHandler;

		public delegate void JobPropertiesCloseEventHandler(object source, JobPropertiesCloseEventArgs e);
		public event JobPropertiesCloseEventHandler OnJobPropertiesCloseHandler;

		private SessionAddressBlock _SAB;
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private MIDTaskListNode _parentNode;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private bool _readOnly;
		private ArrayList _heldJobs;
		private ScheduleData _dlSchedule;
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private FolderDataLayer _dlFolder;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private JobProfile _jobProf;
		private DataTable _dtTaskLists;
		private int _nextJobSequence;
		private bool _bypassBeforeDelete;

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public frmJobProperties(SessionAddressBlock aSAB, bool aReadOnly)
		public frmJobProperties(SessionAddressBlock aSAB, MIDTaskListNode aParentNode, bool aReadOnly)
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			: base(aSAB)
		{
			try
			{
				InitializeComponent();

				_SAB = aSAB;
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_parentNode = aParentNode;
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_readOnly = aReadOnly;
				_jobProf = new JobProfile(-1);
				_heldJobs = null;
			
				txtJobName.Focus();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public frmJobProperties(SessionAddressBlock aSAB, JobProfile aJobProfile, ArrayList aHeldJobs, bool aReadOnly)
		public frmJobProperties(SessionAddressBlock aSAB, MIDTaskListNode aParentNode, JobProfile aJobProfile, ArrayList aHeldJobs, bool aReadOnly)
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			: base(aSAB)
		{
			try
			{
				InitializeComponent();

				_SAB = aSAB;
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_parentNode = aParentNode;
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_jobProf = aJobProfile;
				_heldJobs = aHeldJobs;
				_readOnly = aReadOnly;
			
				txtJobName.Focus();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void HandleExceptions(Exception exc)
		{
			Debug.WriteLine(exc.ToString());
			MessageBox.Show(exc.ToString());
		}

		private void frmJobProperties_Load(object sender, System.EventArgs e)
		{
			int i;

			try
			{
				FormLoaded = false;

				FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerSystemJobs);

				_dlSchedule = new ScheduleData();
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_dlFolder = new FolderDataLayer();
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

				if (!_readOnly && FunctionSecurity.AllowUpdate)
				{
					FunctionSecurity.SetFullControl();
				}
				else
				{
					FunctionSecurity.SetReadOnly();  
				}

				// Setup Message Level ValueList

				_dtTaskLists = _dlSchedule.TaskList_ReadByJob(_jobProf.Key);

				_dtTaskLists.Columns.Add("DisplaySequence", typeof(int));
				_dtTaskLists.Columns.Add("User", typeof(string));

				for (i = 0; i < _dtTaskLists.Rows.Count; i++)
				{
					_dtTaskLists.Rows[i]["DisplaySequence"] = i;
				}

				txtJobName.Text = _jobProf.Name;

				BindGridData();

				_nextJobSequence = _dtTaskLists.Rows.Count;

				// Set MIDFormBase security

				SetReadOnly(FunctionSecurity.AllowUpdate);

				btnRunNow.Enabled = false;

				if (_SAB.SchedulerServerSession != null)
				{
					if (FunctionSecurity.AllowExecute)
					{
						btnRunNow.Enabled = true;
					}
				}

                btnSaveAs.Enabled = false;   // TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.
				FormLoaded = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void frmJobProperties_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				if (_heldJobs != null)
				{
					_SAB.SchedulerServerSession.ResumeAllJobs(_heldJobs, _SAB.ClientServerSession.UserRID);		// TT#1386-MD - stodd - Scheduler Job Manager
				}

				if (OnJobPropertiesCloseHandler != null)
				{
					OnJobPropertiesCloseHandler(this, new JobPropertiesCloseEventArgs());
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void frmJobProperties_Activated(object sender, System.EventArgs e)
		{
			try
			{
				txtJobName.Focus();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (Save(eUpdateMode.Update))
				{
					this.Close();
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				Save(eUpdateMode.Update);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void btnSaveAs_Click(object sender, System.EventArgs e)
		{
			try
			{
				Save(eUpdateMode.Create);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void btnRunNow_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (Save(eUpdateMode.Update))
				{
					_SAB.SchedulerServerSession.ScheduleExistingJob(new ScheduleProfile(-1, _jobProf.GetUniqueName()), _jobProf.Key, _SAB.ClientServerSession.UserRID);

					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_JobHasBeenSubmitted), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgJobs_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				e.Cell.Column.PerformAutoResize(PerformAutoSizeType.VisibleRows);

				ChangePending = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgJobs_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			ChangePending = true;
		}

		private void ulgJobs_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
			try
			{
				if (!FunctionSecurity.AllowDelete)
				{
					e.Cancel = true;
					return;
				}

				if (!_bypassBeforeDelete)
				{
					_bypassBeforeDelete = true;

					try
					{
						ulgJobs.Selected.Rows[0].Delete(false);
						ulgJobs.UpdateData();
						_dtTaskLists.AcceptChanges();
						ulgJobs.Selected.Rows.Clear();

						ChangePending = true;

						e.Cancel = true;
					}
					catch (Exception exc)
					{
						string message = exc.ToString();
						throw;
					}
					finally
					{
						_bypassBeforeDelete = false;
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ctmTaskLists_Popup(object sender, System.EventArgs e)
		{
			try
			{
				if (FunctionSecurity.AllowDelete)
				{
					if (ulgJobs.Selected.Rows.Count > 0)
					{
						mniDelete.Enabled = true;
					}
					else
					{
						mniDelete.Enabled = false;
					}
				}
				else
				{
					mniDelete.Enabled = false;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void mniDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				ulgJobs.Selected.Rows[0].Delete(false);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void grid_SelectionDrag(object sender, System.ComponentModel.CancelEventArgs e)
		{
			UltraGrid grid;

			try
			{
				if (!FunctionSecurity.AllowUpdate)
				{
					e.Cancel = true;
					return;
				}

				grid = (UltraGrid)sender;

				if (grid.Selected.Rows.Count > 0)
				{
					grid.DoDragDrop(grid.Selected.Rows, DragDropEffects.Move);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void grid_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			UltraGrid grid;
			Infragistics.Win.UIElement element;
			SelectedRowsCollection selectedRows;
			UltraGridRow selectedRow;
			UltraGridRow dropRow;
            TreeNodeClipboardList cbList;

			try
			{
                Image_DragOver(sender, e);
				if (!FunctionSecurity.AllowUpdate)
				{
					e.Effect = DragDropEffects.None;
					return;
				}

				grid = (UltraGrid)sender;
				element = grid.DisplayLayout.UIElement.ElementFromPoint(grid.PointToClient(new Point(e.X, e.Y)));

				if (element != null) 
				{
					dropRow = (UltraGridRow)element.GetContext(typeof(UltraGridRow)); 

					if (e.Data.GetDataPresent(typeof(SelectedRowsCollection)))
					{
						selectedRows = (SelectedRowsCollection)e.Data.GetData(typeof(SelectedRowsCollection));

						if (selectedRows.Count == 1)
						{
							selectedRow = ((SelectedRowsCollection)e.Data.GetData(typeof(SelectedRowsCollection)))[0];

							if (dropRow != null && selectedRow.Band == dropRow.Band) 
							{
								e.Effect = e.AllowedEffect;
								return;
							}
							else
							{
								e.Effect = DragDropEffects.None;
								return;
							}
						}
						else
						{
							e.Effect = DragDropEffects.None;
							return;
						}
					}
                    else if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
					{
                        cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));

                        if (cbList.ClipboardDataType == eProfileType.TaskList)
                        {
                            e.Effect = e.AllowedEffect;
                            return;
                        }
					}
				}

				e.Effect = DragDropEffects.None;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void grid_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			UltraGrid grid;
			Infragistics.Win.UIElement element;
			UltraGridRow selectedRow;
			UltraGridRow dropRow;
			int dragSeq;
			int dropSeq;
			TaskListProfile taskListProf;
			UltraGridRow newRow;
            TreeNodeClipboardList cbList;

			try
			{
				grid = (UltraGrid)sender;
				element = grid.DisplayLayout.UIElement.ElementFromPoint(grid.PointToClient(new Point(e.X, e.Y)));

				if (element != null) 
				{
					dropRow = (UltraGridRow)element.GetContext(typeof(UltraGridRow)); 

					if (e.Data.GetDataPresent(typeof(SelectedRowsCollection)))
					{
						selectedRow = ((SelectedRowsCollection)e.Data.GetData(typeof(SelectedRowsCollection)))[0];

						if (dropRow != null) 
						{
							dragSeq = Convert.ToInt32(selectedRow.Cells["DisplaySequence"].Value);
							dropSeq = Convert.ToInt32(dropRow.Cells["DisplaySequence"].Value);

							if (dragSeq != dropSeq)
							{
								if (dragSeq > dropSeq)
								{
									foreach (UltraGridRow row in grid.Rows)
									{
										if (Convert.ToInt32(row.Cells["DisplaySequence"].Value) >= dropSeq && Convert.ToInt32(row.Cells["DisplaySequence"].Value) < dragSeq)
										{
											row.Cells["DisplaySequence"].Value = Convert.ToInt32(row.Cells["DisplaySequence"].Value) + 1;
										}
										else if (Convert.ToInt32(row.Cells["DisplaySequence"].Value) == dragSeq)
										{
											row.Cells["DisplaySequence"].Value = dropSeq;
										}
									}
								}
								else
								{
									foreach (UltraGridRow row in grid.Rows)
									{
										if (Convert.ToInt32(row.Cells["DisplaySequence"].Value) > dragSeq && Convert.ToInt32(row.Cells["DisplaySequence"].Value) <= dropSeq)
										{
											row.Cells["DisplaySequence"].Value = Convert.ToInt32(row.Cells["DisplaySequence"].Value) - 1;
										}
										else if (Convert.ToInt32(row.Cells["DisplaySequence"].Value) == dragSeq)
										{
											row.Cells["DisplaySequence"].Value = dropSeq;
										}
									}
								}

								selectedRow.Band.SortedColumns.RefreshSort(true);
								grid.UpdateData();

								ChangePending = true;
							}
						}
					}
                    else if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
					{
                        cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));

                        if (cbList.ClipboardDataType == eProfileType.TaskList)
                        {
							//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
							//taskListProf = (TaskListProfile)((MIDTaskListNode)cbList.ClipboardProfile.Node).Tag;
							taskListProf = (TaskListProfile)((MIDTaskListNode)cbList.ClipboardProfile.Node).Profile;
							//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

                            ulgJobs.BeginUpdate();

                            newRow = ulgJobs.DisplayLayout.Bands[0].AddNew();
                            newRow.Cells["JOB_RID"].Value = _jobProf.Key;
                            newRow.Cells["TASKLIST_RID"].Value = taskListProf.Key;
                            newRow.Cells["TASKLIST_RID"].Value = taskListProf.Key;
                            newRow.Cells["TASKLIST_SEQUENCE"].Value = -1;
                            newRow.Cells["TASKLIST_NAME"].Value = taskListProf.Name;
                            newRow.Cells["USER_RID"].Value = taskListProf.UserRID;
                            newRow.Cells["User"].Value = GetUserName(taskListProf.UserRID);
                            newRow.Cells["DisplaySequence"].Value = _nextJobSequence;
                            ulgJobs.PerformAction(UltraGridAction.ExitEditMode);

                            _nextJobSequence++;

                            newRow.Band.SortedColumns.RefreshSort(true);
                            grid.UpdateData();

                            ulgJobs.EndUpdate();

                            ChangePending = true;
                        }
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		override protected bool SaveChanges()
		{
			try
			{
				if (Save(eUpdateMode.Update))
				{
					ErrorFound = false;
				}
				else
				{
					ErrorFound = true;
				}

				return true;
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
				ulgJobs.DataSource = _dtTaskLists;

				ulgJobs.DisplayLayout.Override.SelectTypeRow = SelectType.SingleAutoDrag;
				ulgJobs.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;

				ulgJobs.DisplayLayout.Bands[0].Columns["TASKLIST_NAME"].Header.Caption = "Task List";
				ulgJobs.DisplayLayout.Bands[0].Columns["TASKLIST_NAME"].Header.VisiblePosition = 0;
				ulgJobs.DisplayLayout.Bands[0].Columns["TASKLIST_NAME"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
				ulgJobs.DisplayLayout.Bands[0].Columns["User"].Header.VisiblePosition = 1;
				ulgJobs.DisplayLayout.Bands[0].Columns["User"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
				ulgJobs.DisplayLayout.Bands[0].Columns["DisplaySequence"].Hidden = true;
				ulgJobs.DisplayLayout.Bands[0].Columns["JOB_RID"].Hidden = true;
				ulgJobs.DisplayLayout.Bands[0].Columns["TASKLIST_RID"].Hidden = true;
				ulgJobs.DisplayLayout.Bands[0].Columns["TASKLIST_SEQUENCE"].Hidden = true;
				ulgJobs.DisplayLayout.Bands[0].Columns["USER_RID"].Hidden = true;

				foreach (UltraGridRow row in ulgJobs.Rows)
				{
					row.Cells["User"].Value = GetUserName(Convert.ToInt32(row.Cells["USER_RID"].Value));
				}
				
				ulgJobs.UpdateData();

				ulgJobs.DisplayLayout.Bands[0].Columns["TASKLIST_NAME"].MinWidth = 200;
				ulgJobs.DisplayLayout.Bands[0].Columns["User"].MinWidth = 200;

				ulgJobs.DisplayLayout.Bands[0].Columns["TASKLIST_NAME"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgJobs.DisplayLayout.Bands[0].Columns["User"].PerformAutoResize(PerformAutoSizeType.VisibleRows);

				ulgJobs.DisplayLayout.Bands[0].SortedColumns.Add(ulgJobs.DisplayLayout.Bands[0].Columns["DisplaySequence"], false);

				SetControlReadOnly(ulgJobs, FunctionSecurity.IsReadOnly);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private string GetUserName(int aUserRID)
		{
			try
			{
				switch (aUserRID)
				{
					case Include.SystemUserRID :
						return "System";
					case Include.GlobalUserRID :
						return "Global";
					default:
						return _SAB.ClientServerSession.GetUserName(aUserRID);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private bool Save(eUpdateMode aUpdateMode)
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				if (CheckValues(aUpdateMode))
				{
					SaveJobValues(aUpdateMode);
					return true;
				}
				else
				{
					return false;
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

		private bool CheckValues(eUpdateMode aUpdateMode)
		{
			try
			{
				if (!FunctionSecurity.AllowUpdate)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				if (txtJobName.Text.Length == 0)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_JobNameRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

                // Begin TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.
                //if (_jobProf.Key == -1 || aUpdateMode == eUpdateMode.Create || _jobProf.Name != txtJobName.Text)
                if ((_jobProf.Key == Include.NoRID || aUpdateMode == eUpdateMode.Update) && _jobProf.Name != txtJobName.Text)
                // End TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.
                {
                    if (_dlSchedule.Job_GetKey(txtJobName.Text.Trim()) != -1)
                    {
                        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_JobNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

				if (ulgJobs.Rows.Count == 0)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneTaskListRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.
        //private void SaveJobValues(eUpdateMode aUpdateMode)
        //{
        //    int i;

        //    try
        //    {
        //        _dlSchedule.OpenUpdateConnection();

        //        try
        //        {
        //            ulgJobs.PerformAction(UltraGridAction.ExitEditMode);
        //            ulgJobs.UpdateData();
        //            _dtTaskLists.AcceptChanges();

        //            _jobProf.Name = txtJobName.Text;

        //            if (_jobProf.Key == -1 || aUpdateMode == eUpdateMode.Create)
        //            {
        //                _jobProf.Key = _dlSchedule.Job_Insert(_jobProf, _SAB.ClientServerSession.UserRID);
        //                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

        //                _dlFolder.OpenUpdateConnection();

        //                try
        //                {
        //                    _dlFolder.Folder_Item_Insert(_parentNode.Profile.Key, _jobProf.Key, eProfileType.Job);
        //                    _dlFolder.CommitData();
        //                }
        //                catch (Exception exc)
        //                {
        //                    string message = exc.ToString();
        //                    throw;
        //                }
        //                finally
        //                {
        //                    _dlFolder.CloseUpdateConnection();
        //                }
        //                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //            }
        //            else
        //            {
        //                _dlSchedule.Job_Update(_jobProf, _SAB.ClientServerSession.UserRID);
        //            }


        //            for (i = 0; i < ulgJobs.Rows.Count; i++)
        //            {
        //                ulgJobs.Rows[i].Cells["JOB_RID"].Value = _jobProf.Key;
        //                ulgJobs.Rows[i].Cells["TASKLIST_SEQUENCE"].Value = i;
        //            }

        //            ulgJobs.UpdateData();
        //            _dtTaskLists.AcceptChanges();

        //            _dlSchedule.JobTaskListJoin_DeleteByJob(_jobProf.Key);
        //            _dlSchedule.JobTaskListJoin_Insert(_dtTaskLists);

        //            _dlSchedule.CommitData();
        //        }
        //        catch (Exception exc)
        //        {
        //            string message = exc.ToString();
        //            throw;
        //        }
        //        finally
        //        {
        //            _dlSchedule.CloseUpdateConnection();
        //        }

        //        ChangePending = false;

        //        if (OnJobPropertiesSaveHandler != null)
        //        {
        //            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //            //OnJobPropertiesSaveHandler(this, new JobPropertiesSaveEventArgs());
        //            OnJobPropertiesSaveHandler(this, new JobPropertiesSaveEventArgs(_parentNode, _jobProf));
        //            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}
		
		private void SaveJobValues(eUpdateMode aUpdateMode)
		{
			int i;
            JobProfile jobProf = null; 

			try
			{
				_dlSchedule.OpenUpdateConnection();

				try
				{
					ulgJobs.PerformAction(UltraGridAction.ExitEditMode);
					ulgJobs.UpdateData();
					_dtTaskLists.AcceptChanges();

                    if (aUpdateMode == eUpdateMode.Create)
                    {
                        jobProf = (JobProfile)_jobProf.Clone();
                        frmSaveAs formSaveAs = new frmSaveAs(SAB);
                        formSaveAs.SaveAsName = txtJobName.Text.Trim();
                        formSaveAs.isGlobalChecked = true;

                        formSaveAs.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                        bool continueSave = false;
                        bool saveAsCanceled = false;
                        while (!continueSave)
                        {
                            formSaveAs.ShowDialog(this);
                            saveAsCanceled = formSaveAs.SaveCanceled;
                            if (!saveAsCanceled)
                            {
                                saveAsCanceled = false;
                                continueSave = true;
                                if (formSaveAs.SaveMethod)
                                {
                                    if (_dlSchedule.Job_GetKey(formSaveAs.SaveAsName.Trim()) != -1)
                                    {
                                        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_JobNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        continueSave = false;
                                    }
                                    else
                                    {
                                        txtJobName.Text = formSaveAs.SaveAsName;
                                    }
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        jobProf = _jobProf;
                    }
                    
                    jobProf.Name = txtJobName.Text;
                    
					if (jobProf.Key == -1 || aUpdateMode == eUpdateMode.Create)
					{
						jobProf.Key = _dlSchedule.Job_Insert(jobProf, _SAB.ClientServerSession.UserRID);
						
                        _dlFolder.OpenUpdateConnection();

						try
						{
                            _dlFolder.Folder_Item_Insert(_parentNode.Profile.Key, jobProf.Key, eProfileType.Job);
                            _dlFolder.CommitData();
						}
						catch (Exception exc)
						{
							string message = exc.ToString();
							throw;
						}
						finally
						{
							_dlFolder.CloseUpdateConnection();
						}
					}
					else
					{
                        _dlSchedule.Job_Update(jobProf, _SAB.ClientServerSession.UserRID);
					}


					for (i = 0; i < ulgJobs.Rows.Count; i++)
					{
						ulgJobs.Rows[i].Cells["JOB_RID"].Value = jobProf.Key;
						ulgJobs.Rows[i].Cells["TASKLIST_SEQUENCE"].Value = i;
					}

					ulgJobs.UpdateData();
					_dtTaskLists.AcceptChanges();

					_dlSchedule.JobTaskListJoin_DeleteByJob(jobProf.Key);
					_dlSchedule.JobTaskListJoin_Insert(_dtTaskLists);

					_dlSchedule.CommitData();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_dlSchedule.CloseUpdateConnection();
				}

				ChangePending = false;

				if (OnJobPropertiesSaveHandler != null)
				{
					OnJobPropertiesSaveHandler(this, new JobPropertiesSaveEventArgs(_parentNode, jobProf));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        private void txtJobName_TextChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                btnSaveAs.Enabled = true;
            }
        }

        override public void ISave()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    Save(eUpdateMode.Update);
                }
                catch (Exception exc)
                {
                    HandleExceptions(exc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        override public void ISaveAs()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    Save(eUpdateMode.Create);
                }
                catch (Exception exc)
                {
                    HandleExceptions(exc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        // End TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.

        private void ulgJobs_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            //End TT#169
        }
	}

	public class JobPropertiesSaveEventArgs : EventArgs
	{
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private MIDTaskListNode _parentNode;
		private JobProfile _jobProf;

		public JobPropertiesSaveEventArgs(MIDTaskListNode aParentNode, JobProfile aJobProf)
		{
			_parentNode = aParentNode;
			_jobProf = aJobProf;
		}

		public MIDTaskListNode ParentNode
		{
			get
			{
				return _parentNode;
			}
		}

		public JobProfile JobProfile
		{
			get
			{
				return _jobProf;
			}
		}
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
	}

	public class JobPropertiesCloseEventArgs : EventArgs
	{
	}
}
