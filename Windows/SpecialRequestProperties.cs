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
	/// Summary description for SpecialRequestProperties.
	/// </summary>
	public class frmSpecialRequestProperties : MIDFormBase
	{
		#region Windows Form Designer generated code

		private System.Windows.Forms.Label lblJobName;
		private System.Windows.Forms.TextBox txtJobName;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private Infragistics.Win.UltraWinGrid.UltraGrid ulgJobs;
		private System.Windows.Forms.Button btnSaveAs;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.ContextMenu ctmTaskLists;
		private System.Windows.Forms.MenuItem mniDelete;
		private NumericUpDown nudConcurrent;
		private Label lblConcurrent;
		private ToolTip toolTip;
		private IContainer components;

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
				this.ctmTaskLists.Popup -= new System.EventHandler(this.ctmTaskLists_Popup);
				this.mniDelete.Click -= new System.EventHandler(this.mniDelete_Click);
				this.btnSaveAs.Click -= new System.EventHandler(this.btnSaveAs_Click);
				this.Load -= new System.EventHandler(this.frmSpecialRequestProperties_Load);
				this.Activated -= new System.EventHandler(this.frmSpecialRequestProperties_Activated);
				this.Closing -= new System.ComponentModel.CancelEventHandler(this.frmSpecialRequestProperties_Closing);
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			this.lblJobName = new System.Windows.Forms.Label();
			this.txtJobName = new System.Windows.Forms.TextBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.ulgJobs = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.ctmTaskLists = new System.Windows.Forms.ContextMenu();
			this.mniDelete = new System.Windows.Forms.MenuItem();
			this.btnSaveAs = new System.Windows.Forms.Button();
			this.nudConcurrent = new System.Windows.Forms.NumericUpDown();
			this.lblConcurrent = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ulgJobs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudConcurrent)).BeginInit();
			this.SuspendLayout();
			// 
			// utmMain
			// 
			this.utmMain.MenuSettings.ForceSerialization = true;
			this.utmMain.ToolbarSettings.ForceSerialization = true;
			// 
			// lblJobName
			// 
			this.lblJobName.Location = new System.Drawing.Point(8, 8);
			this.lblJobName.Name = "lblJobName";
			this.lblJobName.Size = new System.Drawing.Size(151, 23);
			this.lblJobName.TabIndex = 33;
			this.lblJobName.Text = "Special Request Job Name:";
			this.lblJobName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtJobName
			// 
			this.txtJobName.Location = new System.Drawing.Point(166, 8);
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
			appearance3.TextHAlignAsString = "Center";
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
			this.ulgJobs.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
			this.ulgJobs.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgJobs_InitializeLayout);
			this.ulgJobs.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgJobs_BeforeRowsDeleted);
			this.ulgJobs.DragOver += new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
			this.ulgJobs.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgJobs_AfterCellUpdate);
			this.ulgJobs.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgJobs_CellChange);
			this.ulgJobs.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ulgJobs_MouseEnterElement);
			this.ulgJobs.DragDrop += new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
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
			// nudConcurrent
			// 
			this.nudConcurrent.Location = new System.Drawing.Point(463, 8);
			this.nudConcurrent.Name = "nudConcurrent";
			this.nudConcurrent.Size = new System.Drawing.Size(39, 20);
			this.nudConcurrent.TabIndex = 40;
			this.nudConcurrent.ValueChanged += new System.EventHandler(this.nudConcurrent_ValueChanged);
			// 
			// lblConcurrent
			// 
			this.lblConcurrent.Location = new System.Drawing.Point(359, 8);
			this.lblConcurrent.Name = "lblConcurrent";
			this.lblConcurrent.Size = new System.Drawing.Size(98, 23);
			this.lblConcurrent.TabIndex = 41;
			this.lblConcurrent.Text = "Concurrent Jobs:";
			this.lblConcurrent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// frmSpecialRequestProperties
			// 
			this.AllowDragDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(600, 270);
			this.Controls.Add(this.lblConcurrent);
			this.Controls.Add(this.nudConcurrent);
			this.Controls.Add(this.btnSaveAs);
			this.Controls.Add(this.ulgJobs);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.txtJobName);
			this.Controls.Add(this.lblJobName);
			this.Name = "frmSpecialRequestProperties";
			this.Text = "Special Request Job Properties";
			this.Load += new System.EventHandler(this.frmSpecialRequestProperties_Load);
			this.Activated += new System.EventHandler(this.frmSpecialRequestProperties_Activated);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmSpecialRequestProperties_Closing);
			this.Controls.SetChildIndex(this.lblJobName, 0);
			this.Controls.SetChildIndex(this.txtJobName, 0);
			this.Controls.SetChildIndex(this.btnSave, 0);
			this.Controls.SetChildIndex(this.btnOK, 0);
			this.Controls.SetChildIndex(this.btnCancel, 0);
			this.Controls.SetChildIndex(this.ulgJobs, 0);
			this.Controls.SetChildIndex(this.btnSaveAs, 0);
			this.Controls.SetChildIndex(this.nudConcurrent, 0);
			this.Controls.SetChildIndex(this.lblConcurrent, 0);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ulgJobs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudConcurrent)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		public delegate void SpecialRequestPropertiesSaveEventHandler(object source, SpecialRequestPropertiesSaveEventArgs e);
		public event SpecialRequestPropertiesSaveEventHandler OnSpecialRequestPropertiesSaveHandler;

		public delegate void SpecialRequestPropertiesCloseEventHandler(object source, SpecialRequestPropertiesCloseEventArgs e);
		public event SpecialRequestPropertiesCloseEventHandler OnSpecialRequestPropertiesCloseHandler;

		private SessionAddressBlock _sab;
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private MIDTaskListNode _parentNode;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private bool _readOnly;
		private ArrayList _heldJobs;
		private ScheduleData _dlSchedule;
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private FolderDataLayer _dlFolder;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private SpecialRequestProfile _SpecialRequestProf;
		private DataTable _dtSpecialRequests;
		private int _nextJobSequence;
		private bool _bypassBeforeDelete;

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public frmSpecialRequestProperties(SessionAddressBlock aSAB, bool aReadOnly)
		public frmSpecialRequestProperties(SessionAddressBlock aSAB, MIDTaskListNode aParentNode, bool aReadOnly)
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			: base(aSAB)
		{
			try
			{
				InitializeComponent();

				_sab = aSAB;
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_parentNode = aParentNode;
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_readOnly = aReadOnly;
				_SpecialRequestProf = new SpecialRequestProfile(-1);
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
		//public frmSpecialRequestProperties(SessionAddressBlock aSAB, SpecialRequestProfile aSpecialRequestProfile, ArrayList aHeldJobs, bool aReadOnly)
		public frmSpecialRequestProperties(SessionAddressBlock aSAB, MIDTaskListNode aParentNode, SpecialRequestProfile aSpecialRequestProfile, ArrayList aHeldJobs, bool aReadOnly)
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			: base(aSAB)
		{
			try
			{
				InitializeComponent();

				_sab = aSAB;
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_parentNode = aParentNode;
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_SpecialRequestProf = aSpecialRequestProfile;
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

		private void frmSpecialRequestProperties_Load(object sender, System.EventArgs e)
		{
			int i;

			try
			{
				FormLoaded = false;

				FunctionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerSystemSpecialReq);

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

				_dtSpecialRequests = _dlSchedule.SpecialRequest_ReadByJob(_SpecialRequestProf.Key);

				_dtSpecialRequests.Columns.Add("DisplaySequence", typeof(int));

				SetText();

				txtJobName.Text = _SpecialRequestProf.Name;
				nudConcurrent.Minimum = 1;
				nudConcurrent.Maximum = 99;
				nudConcurrent.Value = _SpecialRequestProf.ConcurrentJobs;

				BindGridData();

				_nextJobSequence = _dtSpecialRequests.Rows.Count;

				SetReadOnly(FunctionSecurity.AllowUpdate);

                btnSaveAs.Enabled = false;   // TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.

				FormLoaded = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void SetText()
		{
			try
			{
				this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
				this.btnSaveAs.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_SaveAs);
				this.btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
				this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
				this.lblConcurrent.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_JobConcurrency);
				this.lblJobName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SpecialRequest);
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}


		private void frmSpecialRequestProperties_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				if (_heldJobs != null)
				{
					_sab.SchedulerServerSession.ResumeAllJobs(_heldJobs, _SAB.ClientServerSession.UserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
				}

				if (OnSpecialRequestPropertiesCloseHandler != null)
				{
					OnSpecialRequestPropertiesCloseHandler(this, new SpecialRequestPropertiesCloseEventArgs());
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}


		private void frmSpecialRequestProperties_Activated(object sender, System.EventArgs e)
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
						_dtSpecialRequests.AcceptChanges();
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
            TreeNodeClipboardProfile cbProf;

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

						if (cbList.ClipboardDataType == eProfileType.Job)
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
            TreeNodeClipboardList cbList = null;
			JobProfile jobProf;
			UltraGridRow newRow;

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

						if (cbList.ClipboardDataType == eProfileType.Job)
                        {
							//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
							//jobProf = (JobProfile)((MIDTaskListNode)cbList.ClipboardProfile.Node).Tag;
							jobProf = (JobProfile)((MIDTaskListNode)cbList.ClipboardProfile.Node).Profile;
							//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

                            ulgJobs.BeginUpdate();

                            newRow = ulgJobs.DisplayLayout.Bands[0].AddNew();
                            newRow.Cells["SPECIAL_REQ_RID"].Value = this._SpecialRequestProf.Key;
                            newRow.Cells["JOB_RID"].Value = jobProf.Key;
                            newRow.Cells["JOB_SEQUENCE"].Value = -1;
                            newRow.Cells["JOB_NAME"].Value = jobProf.Name;
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

		private void ulgJobs_InitializeLayout(object sender, InitializeLayoutEventArgs e)
		{
			ulgJobs.DisplayLayout.Override.SelectTypeRow = SelectType.SingleAutoDrag;
			ulgJobs.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
			ulgJobs.DisplayLayout.Override.CellClickAction = CellClickAction.CellSelect;

			ulgJobs.DisplayLayout.Bands[0].Columns["JOB_NAME"].Header.Caption = "Job";
			ulgJobs.DisplayLayout.Bands[0].Columns["JOB_NAME"].Header.VisiblePosition = 0;
			ulgJobs.DisplayLayout.Bands[0].Columns["JOB_NAME"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;

			ulgJobs.DisplayLayout.Bands[0].Columns["DisplaySequence"].Hidden = true;
			ulgJobs.DisplayLayout.Bands[0].Columns["SPECIAL_REQ_RID"].Hidden = true;
			ulgJobs.DisplayLayout.Bands[0].Columns["JOB_RID"].Hidden = true;
			ulgJobs.DisplayLayout.Bands[0].Columns["JOB_SEQUENCE"].Hidden = true;

			ulgJobs.UpdateData();

			ulgJobs.DisplayLayout.Bands[0].Columns["JOB_NAME"].MinWidth = 200;
			ulgJobs.DisplayLayout.Bands[0].Columns["JOB_NAME"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
			ulgJobs.DisplayLayout.Bands[0].SortedColumns.Add(ulgJobs.DisplayLayout.Bands[0].Columns["DisplaySequence"], false);

		}

		private void ulgJobs_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowGridToolTip(e);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		/// <summary>
		/// Shows ToolTip to display message in an UntraGrid cell 
		/// </summary>
		/// <param name="ultraGrid">The UltraGrid where the tool tip is to be displayed</param>
		/// <param name="e">The UIElementEventArgs arguments of the MouseEnterElement event</param>
		protected void ShowGridToolTip(Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				if (this.toolTip != null && this.toolTip.Active)
				{
					this.toolTip.Active = false; //turn it off 
				}

				UltraGridCell gridCell = (UltraGridCell)e.Element.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell));
				if (gridCell == null)
				{
					string msg = "Drag desired jobs and drop them here.";
					toolTip.Active = true;
					toolTip.SetToolTip(ulgJobs, msg);
				}
			}
			catch (Exception exception)
			{
				HandleException(exception);
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
				ulgJobs.DataSource = _dtSpecialRequests;

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
						return _sab.ClientServerSession.GetUserName(aUserRID);
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
					MessageBox.Show(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				if (txtJobName.Text.Length == 0)
				{
					MessageBox.Show(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_JobNameRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

                // Begin TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.
                //if (_SpecialRequestProf.Key == -1 || aUpdateMode == eUpdateMode.Create || _SpecialRequestProf.Name != txtJobName.Text)
                if ((_SpecialRequestProf.Key == Include.NoRID || aUpdateMode == eUpdateMode.Update) && _SpecialRequestProf.Name != txtJobName.Text)
                // End TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.
				{
					if (_dlSchedule.SpecialRequest_GetKey(txtJobName.Text.Trim()) != -1)
					{
						MessageBox.Show(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SpecialRequestNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
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
        //            _dtSpecialRequests.AcceptChanges();

        //            _SpecialRequestProf.Name = txtJobName.Text;
        //            _SpecialRequestProf.ConcurrentJobs = (int)nudConcurrent.Value;

        //            if (_SpecialRequestProf.Key == -1 || aUpdateMode == eUpdateMode.Create)
        //            {
        //                _SpecialRequestProf.Key = _dlSchedule.SpecialRequest_Insert(_SpecialRequestProf);
        //                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

        //                _dlFolder.OpenUpdateConnection();

        //                try
        //                {
        //                    _dlFolder.Folder_Item_Insert(_parentNode.Profile.Key, _SpecialRequestProf.Key, eProfileType.SpecialRequest);
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
        //                _dlSchedule.SpecialRequest_Update(_SpecialRequestProf);
        //            }


        //            for (i = 0; i < ulgJobs.Rows.Count; i++)
        //            {
        //                ulgJobs.Rows[i].Cells["SPECIAL_REQ_RID"].Value = _SpecialRequestProf.Key;
        //                ulgJobs.Rows[i].Cells["JOB_SEQUENCE"].Value = i;
        //            }

        //            ulgJobs.UpdateData();
        //            _dtSpecialRequests.AcceptChanges();

        //            _dlSchedule.SpecialRequestJoin_DeleteBySpecialRequest(_SpecialRequestProf.Key);
        //            _dlSchedule.SpecialRequestJoin_Insert(_dtSpecialRequests);

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

        //        if (OnSpecialRequestPropertiesSaveHandler != null)
        //        {
        //            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
        //            //OnSpecialRequestPropertiesSaveHandler(this, new SpecialRequestPropertiesSaveEventArgs());
        //            OnSpecialRequestPropertiesSaveHandler(this, new SpecialRequestPropertiesSaveEventArgs(_parentNode, _SpecialRequestProf));
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
            SpecialRequestProfile specialRequestProf = null; 

            try
            {
                _dlSchedule.OpenUpdateConnection();

                try
                {
                    ulgJobs.PerformAction(UltraGridAction.ExitEditMode);
                    ulgJobs.UpdateData();
                    _dtSpecialRequests.AcceptChanges();

                    if (aUpdateMode == eUpdateMode.Create)
                    {
                        specialRequestProf = (SpecialRequestProfile)_SpecialRequestProf.Clone();
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
                                    if (_dlSchedule.SpecialRequest_GetKey(formSaveAs.SaveAsName.Trim()) != -1)
                                    {
                                        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SpecialRequestNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        specialRequestProf = _SpecialRequestProf;
                    }

                    specialRequestProf.Name = txtJobName.Text;
                    specialRequestProf.ConcurrentJobs = (int)nudConcurrent.Value;

                    if (specialRequestProf.Key == -1 || aUpdateMode == eUpdateMode.Create)
                    {
                        specialRequestProf.Key = _dlSchedule.SpecialRequest_Insert(specialRequestProf);

                        _dlFolder.OpenUpdateConnection();

                        try
                        {
                            _dlFolder.Folder_Item_Insert(_parentNode.Profile.Key, specialRequestProf.Key, eProfileType.SpecialRequest);
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
                        _dlSchedule.SpecialRequest_Update(specialRequestProf);
                    }


                    for (i = 0; i < ulgJobs.Rows.Count; i++)
                    {
                        ulgJobs.Rows[i].Cells["SPECIAL_REQ_RID"].Value = specialRequestProf.Key;
                        ulgJobs.Rows[i].Cells["JOB_SEQUENCE"].Value = i;
                    }

                    ulgJobs.UpdateData();
                    _dtSpecialRequests.AcceptChanges();

                    _dlSchedule.SpecialRequestJoin_DeleteBySpecialRequest(specialRequestProf.Key);
                    _dlSchedule.SpecialRequestJoin_Insert(_dtSpecialRequests);

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

                if (OnSpecialRequestPropertiesSaveHandler != null)
                {
                    OnSpecialRequestPropertiesSaveHandler(this, new SpecialRequestPropertiesSaveEventArgs(_parentNode, specialRequestProf));
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
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

		private void nudConcurrent_ValueChanged(object sender, EventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
		}

		private void txtJobName_TextChanged(object sender, EventArgs e)
		{
            // Begin TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.
            //if (FormLoaded)
            //    ChangePending = true;
            if (FormLoaded)
            {
                ChangePending = true;
                btnSaveAs.Enabled = true;
            }
            // End TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.
		}
	}

	public class SpecialRequestPropertiesSaveEventArgs : EventArgs
	{
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private MIDTaskListNode _parentNode;
		private SpecialRequestProfile _specialRequestProf;

		public SpecialRequestPropertiesSaveEventArgs(MIDTaskListNode aParentNode, SpecialRequestProfile aSpecialRequestProf)
		{
			_parentNode = aParentNode;
			_specialRequestProf = aSpecialRequestProf;
		}

		public MIDTaskListNode ParentNode
		{
			get
			{
				return _parentNode;
			}
		}

		public SpecialRequestProfile SpecialRequestProfile
		{
			get
			{
				return _specialRequestProf;
			}
		}
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
	}

	public class SpecialRequestPropertiesCloseEventArgs : EventArgs
	{
	}
}
