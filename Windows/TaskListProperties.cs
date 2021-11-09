using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
	/// Summary description for TaskListProperties.
	/// </summary>
	public class frmTaskListProperties : MIDFormBase
	{
		#region Windows Form Designer generated code

		private System.Windows.Forms.Label lblFilterName;
		private System.Windows.Forms.Panel pnlTasks;
		internal System.Windows.Forms.Button btnCancel;
		internal System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TextBox txtTaskListName;
        internal System.Windows.Forms.Button btnSave;
		private Infragistics.Win.UltraWinGrid.UltraGrid ulgTasks;
		private System.Windows.Forms.ContextMenu ctmTaskMenu;
		private System.Windows.Forms.RadioButton rdoGlobal;
		private System.Windows.Forms.RadioButton rdoUser;
		private System.Windows.Forms.RadioButton rdoSystem;
		private System.Windows.Forms.MenuItem mniTaskDelete;
		private System.Windows.Forms.ContextMenu ctmForecastMenu;
        private System.Windows.Forms.MenuItem mniForecastDelete;
		private System.Windows.Forms.FolderBrowserDialog fbdDirectory;
        private System.Windows.Forms.ContextMenu ctmAddTask;
        internal System.Windows.Forms.Button btnSaveAs;
        internal System.Windows.Forms.Button btnRunNow;
		private System.Windows.Forms.ContextMenu ctmAllocateMenu;
        private System.Windows.Forms.MenuItem mniAllocateDelete;
		private System.Windows.Forms.MenuItem mniRollupDelete;
        private System.Windows.Forms.ContextMenu ctmRollupMenu;
        private System.Windows.Forms.OpenFileDialog ofdFile;
		private System.Windows.Forms.RadioButton rdoOwner;
        private SplitContainer splitContainer1;
		private ContextMenu ctmSizeCurveMethodMenu;
        private MenuItem mniSizeCurveMethodDelete;
		private ContextMenu ctmSizeCurvesMenu;
        private MenuItem mniSizeCurvesDelete;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        internal Panel pnlPosting;
        private Label lblPostingNote;
        private TextBox txtPostingRunUntilMask;
        private CheckBox chkPostingRunUntil;
        private Label lblPosting;
        private Label lblPostingConcurrentFiles;
        private Label label3;
        private Label label1;
        private NumericUpDown nudPostingConcurrent;
        private Button btnPostingDirectory;
        private TextBox txtPostingMask;
        private TextBox txtPostingDirectory;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        internal Panel pnlSizeDayToWeekSummary;
        private Label lblOverrideNode;
        private TextBox txtOverrideNode;
        private Label lblSizeDayWeekSummaryDt;
        private MIDDateRangeSelector midDateRangeSizeDayWeekSum;
        private Label lblSizeDayWeekSummary;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        internal Panel pnlSizeCurveMethod;
        private UltraGrid ulgSizeCurveMethod;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
        internal Panel pnlRelieve;
        private TextBox txtRelieveRunUntilMask;
        private CheckBox chkRelieveRunUntil;
        private Label lblRelieve;
        private Label label7;
        private Label label8;
        private Button btnRelieveDirectory;
        private TextBox txtRelieveMask;
        private TextBox txtRelieveDirectory;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
        internal Panel pnlAllocate;
        private UltraGrid ulgAllocate;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl7;
        internal Panel pnlForecast;
        private UltraGrid ulgForecast;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl8;
        internal Panel pnlRollup;
        private UltraGrid ulgRollup;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl9;
        internal Panel pnlProgram;
        private Label label2;
        private Label label6;
        private Label label9;
        private Button btnProgram;
        private TextBox txtProgramParms;
        private TextBox txtProgramPath;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl10;
        internal Panel pnlSizeCurves;
        private UltraGrid ulgSizeCurves;
        private NumericUpDown nudSizeCurveConcurrentProcesses;
        private Label lblSizeCurveConcurrentProcesses;
        private EmailTaskList emailTaskList1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private MIDComboBoxEnh cboProcessingDirection;
        private Label lblProcessingDirection;
        private bool _rollUpLoaded = false;   //TT#2744 - MD - Rollup task in 5.0 asking to save when nothing has chenged - RBeck
		// Begin TT#1595-MD - stodd - Batch Comp
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl11;
        private Panel pnlBatchComp;
        private ComboBox cboBatchComp;
        private Label lblBatchComp;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl12;
        private Panel pnlHeaderReconcile;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl13; //TT#4574 - DOConnell - Purge Task List does not have an email option
        private Label lblHROutputDirectory;
        private Label lblHRInputDirectory;
        private Label lblHRHeaderKeysFile;
        private Label lblHRHeaderTypes;
        private CheckedListBox clbHeaderReconcileHeaderTypes;
        private TextBox txtHeaderReconcileRemoveTransactionsTriggerSuffix;
        private Label lblHRRemoveTransactionsTriggerSuffix;
        private TextBox txtHeaderReconcileRemoveTransactionsFileName;
        private TextBox txtHeaderReconcileTriggerSuffix;
        private Label lblHRRemoveTransactionsFileName;
        private Label lblHRTriggerSuffix;
        private Button btnHeaderReconcileOutputDirectory;
        private Button btnHeaderReconcileInputDirectory;
        private TextBox txtHeaderReconcileHeaderKeys;
        private TextBox txtHeaderReconcileOutputDirectory;
        private TextBox txtHeaderReconcileInputDirectory;
        private Button btnHeaderReconcileHeaderKeys;   
		// End TT#1595-MD - stodd - Batch Comp
        private eAPIFileProcessingDirection _FileProcessingDirection = eAPIFileProcessingDirection.Default; // TT#1314-MD - JSmith - Tasklist allows multiple concurrent files with FIFO/FILO processing
        private DataTable _dtHeaderTypes = null;

            
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

                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                //End TT#169

				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.txtTaskListName.TextChanged -= new System.EventHandler(this.txtTaskListName_TextChanged);
				this.btnSaveAs.Click -= new System.EventHandler(this.btnSaveAs_Click);
				this.txtPostingRunUntilMask.TextChanged -= new System.EventHandler(this.txtPostingRunUntilMask_TextChanged);
				this.chkPostingRunUntil.CheckedChanged -= new System.EventHandler(this.chkPostingRunUntil_CheckedChanged);
				this.nudPostingConcurrent.ValueChanged -= new System.EventHandler(this.nudPostingConcurrent_ValueChanged);
				this.btnPostingDirectory.Click -= new System.EventHandler(this.btnPostingDirectory_Click);
				this.txtPostingMask.TextChanged -= new System.EventHandler(this.txtPostingMask_TextChanged);
				this.txtPostingDirectory.TextChanged -= new System.EventHandler(this.txtPostingDirectory_TextChanged);
				this.btnProgram.Click -= new System.EventHandler(this.btnProgram_Click);
				this.txtProgramParms.TextChanged -= new System.EventHandler(this.txtProgramParms_TextChanged);
				this.txtProgramPath.TextChanged -= new System.EventHandler(this.txtProgramPath_TextChanged);
				this.ulgRollup.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgRollup_ClickCellButton);
				this.ulgRollup.SelectionDrag -= new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
				this.ulgRollup.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ulgRollup_BeforeRowInsert);
				this.ulgRollup.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgRollup_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ulgRollup);
                //End TT#169
				this.ulgRollup.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgRollup_BeforeRowsDeleted);
				this.ulgRollup.DragOver -= new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
				this.ulgRollup.AfterCellListCloseUp -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgRollup_AfterCellListCloseUp);
				this.ulgRollup.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ulgRollup_AfterRowInsert);
				this.ulgRollup.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgRollup_AfterCellUpdate);
				this.ulgRollup.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
				this.ctmRollupMenu.Popup -= new System.EventHandler(this.ctmRollupMenu_Popup);
				this.mniRollupDelete.Click -= new System.EventHandler(this.mniRollupDelete_Click);
				this.ulgForecast.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgForecast_ClickCellButton);
				this.ulgForecast.SelectionDrag -= new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
				this.ulgForecast.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ulgForecast_BeforeRowInsert);
				this.ulgForecast.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgForecast_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ulgForecast);
                //End TT#169
				this.ulgForecast.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgForecast_BeforeRowsDeleted);
				this.ulgForecast.DragOver -= new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
				this.ulgForecast.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ulgForecast_AfterRowInsert);
				this.ulgForecast.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgForecast_AfterCellUpdate);
				this.ulgForecast.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgForecast_CellChange);
				this.ulgForecast.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
				this.ctmForecastMenu.Popup -= new System.EventHandler(this.ctmForecastMenu_Popup);
				this.mniForecastDelete.Click -= new System.EventHandler(this.mniForecastDelete_Click);
				this.ulgAllocate.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgAllocate_ClickCellButton);
				this.ulgAllocate.SelectionDrag -= new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
				this.ulgAllocate.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ulgAllocate_BeforeRowInsert);
				this.ulgAllocate.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgAllocate_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ulgAllocate);
                //End TT#169
				this.ulgAllocate.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgAllocate_BeforeRowsDeleted);
				this.ulgAllocate.DragOver -= new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
				this.ulgAllocate.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ulgAllocate_AfterRowInsert);
				this.ulgAllocate.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgAllocate_AfterCellUpdate);
				this.ulgAllocate.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgAllocate_CellChange);
				this.ulgAllocate.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
				this.ctmAllocateMenu.Popup -= new System.EventHandler(this.ctmAllocateMenu_Popup);
				this.mniAllocateDelete.Click -= new System.EventHandler(this.mniAllocateDelete_Click);
				this.txtRelieveRunUntilMask.TextChanged -= new System.EventHandler(this.txtRelieveRunUntilMask_TextChanged);
				this.chkRelieveRunUntil.CheckedChanged -= new System.EventHandler(this.chkRelieveRunUntil_CheckedChanged);
				this.btnRelieveDirectory.Click -= new System.EventHandler(this.btnRelieveDirectory_Click);
				this.txtRelieveMask.TextChanged -= new System.EventHandler(this.txtRelieveMask_TextChanged);
				this.txtRelieveDirectory.TextChanged -= new System.EventHandler(this.txtRelieveDirectory_TextChanged);
				this.ulgTasks.SelectionDrag -= new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
				this.ulgTasks.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ulgTasks_BeforeRowInsert);
				this.ulgTasks.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgTasks_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ulgTasks);
                //End TT#169
				this.ulgTasks.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgTasks_BeforeRowsDeleted);
				this.ulgTasks.DragOver -= new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
				this.ulgTasks.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ulgTasks_AfterRowInsert);
				this.ulgTasks.AfterRowActivate -= new System.EventHandler(this.ulgTasks_AfterRowActivate);
				this.ulgTasks.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgTasks_CellChange);
				this.ulgTasks.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
				this.ctmTaskMenu.Popup -= new System.EventHandler(this.ctmTaskMenu_Popup);
				this.mniTaskDelete.Click -= new System.EventHandler(this.mniTaskDelete_Click);
				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				this.rdoGlobal.CheckedChanged -= new System.EventHandler(this.rdoGlobal_CheckedChanged);
				this.rdoUser.CheckedChanged -= new System.EventHandler(this.rdoUser_CheckedChanged);
				this.rdoSystem.CheckedChanged -= new System.EventHandler(this.rdoSystem_CheckedChanged);
				this.btnRunNow.Click -= new System.EventHandler(this.btnRunNow_Click);
				this.rdoOwner.CheckedChanged -= new System.EventHandler(this.rdoOwner_CheckedChanged);
				this.ulgSizeCurves.SelectionDrag -= new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
				this.ulgSizeCurves.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ulgSizeCurves_BeforeRowInsert);
				this.ulgSizeCurves.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgSizeCurves_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ulgSizeCurves);
                //End TT#169
				this.ulgSizeCurves.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgSizeCurves_BeforeRowsDeleted);
				this.ulgSizeCurves.DragOver -= new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
				this.ulgSizeCurves.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ulgSizeCurves_AfterRowInsert);
				this.ulgSizeCurves.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurves_AfterCellUpdate);
				this.ulgSizeCurves.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurves_CellChange);
				this.ulgSizeCurves.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
				//Begin TT#707 - JScott - Size Curve process needs to multi-thread
				this.nudSizeCurveConcurrentProcesses.ValueChanged -= new System.EventHandler(this.nudSizeCurveConcurrentProcesses_ValueChanged);
				//End TT#707 - JScott - Size Curve process needs to multi-thread
				this.ctmSizeCurvesMenu.Popup -= new System.EventHandler(this.ctmSizeCurvesMenu_Popup);
				this.mniSizeCurvesDelete.Click -= new System.EventHandler(this.mniSizeCurvesDelete_Click);
				this.ulgSizeCurveMethod.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurveMethod_ClickCellButton);
				this.ulgSizeCurveMethod.SelectionDrag -= new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
				this.ulgSizeCurveMethod.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ulgSizeCurveMethod_BeforeRowInsert);
				this.ulgSizeCurveMethod.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgSizeCurveMethod_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ulgSizeCurveMethod);
                //End TT#169
				this.ulgSizeCurveMethod.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgSizeCurveMethod_BeforeRowsDeleted);
				this.ulgSizeCurveMethod.DragOver -= new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
				this.ulgSizeCurveMethod.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ulgSizeCurveMethod_AfterRowInsert);
				this.ulgSizeCurveMethod.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurveMethod_AfterCellUpdate);
				this.ulgSizeCurveMethod.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurveMethod_CellChange);
				this.ulgSizeCurveMethod.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
				this.ctmSizeCurveMethodMenu.Popup -= new System.EventHandler(this.ctmSizeCurveMethodMenu_Popup);
				this.mniSizeCurveMethodDelete.Click -= new System.EventHandler(this.mniSizeCurveMethodDelete_Click);
				this.Load -= new System.EventHandler(this.frmTaskListProperties_Load);
				this.Activated -= new System.EventHandler(this.frmTaskListProperties_Activated);
				this.Closing -= new System.ComponentModel.CancelEventHandler(this.frmTaskListProperties_Closing);
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
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab10 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab8 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab9 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab11 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab12 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			//BEGIN TT#4574 - DOConnell - Purge Task List does not have an email option
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab13 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab14 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab15 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			//END TT#4574 - DOConnell - Purge Task List does not have an email option
            this.ultraTabPageControl10 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlSizeCurves = new System.Windows.Forms.Panel();
            this.ulgSizeCurves = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ctmSizeCurvesMenu = new System.Windows.Forms.ContextMenu();
            this.mniSizeCurvesDelete = new System.Windows.Forms.MenuItem();
            this.nudSizeCurveConcurrentProcesses = new System.Windows.Forms.NumericUpDown();
            this.lblSizeCurveConcurrentProcesses = new System.Windows.Forms.Label();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlPosting = new System.Windows.Forms.Panel();
            this.cboProcessingDirection = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblProcessingDirection = new System.Windows.Forms.Label();
            this.lblPostingNote = new System.Windows.Forms.Label();
            this.txtPostingRunUntilMask = new System.Windows.Forms.TextBox();
            this.chkPostingRunUntil = new System.Windows.Forms.CheckBox();
            this.lblPosting = new System.Windows.Forms.Label();
            this.lblPostingConcurrentFiles = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudPostingConcurrent = new System.Windows.Forms.NumericUpDown();
            this.btnPostingDirectory = new System.Windows.Forms.Button();
            this.txtPostingMask = new System.Windows.Forms.TextBox();
            this.txtPostingDirectory = new System.Windows.Forms.TextBox();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlSizeDayToWeekSummary = new System.Windows.Forms.Panel();
            this.lblOverrideNode = new System.Windows.Forms.Label();
            this.txtOverrideNode = new System.Windows.Forms.TextBox();
            this.lblSizeDayWeekSummaryDt = new System.Windows.Forms.Label();
            this.midDateRangeSizeDayWeekSum = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.lblSizeDayWeekSummary = new System.Windows.Forms.Label();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlSizeCurveMethod = new System.Windows.Forms.Panel();
            this.ulgSizeCurveMethod = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ctmSizeCurveMethodMenu = new System.Windows.Forms.ContextMenu();
            this.mniSizeCurveMethodDelete = new System.Windows.Forms.MenuItem();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlRelieve = new System.Windows.Forms.Panel();
            this.txtRelieveRunUntilMask = new System.Windows.Forms.TextBox();
            this.chkRelieveRunUntil = new System.Windows.Forms.CheckBox();
            this.lblRelieve = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnRelieveDirectory = new System.Windows.Forms.Button();
            this.txtRelieveMask = new System.Windows.Forms.TextBox();
            this.txtRelieveDirectory = new System.Windows.Forms.TextBox();
            this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlAllocate = new System.Windows.Forms.Panel();
            this.ulgAllocate = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ctmAllocateMenu = new System.Windows.Forms.ContextMenu();
            this.mniAllocateDelete = new System.Windows.Forms.MenuItem();
            this.ultraTabPageControl7 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlForecast = new System.Windows.Forms.Panel();
            this.ulgForecast = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ctmForecastMenu = new System.Windows.Forms.ContextMenu();
            this.mniForecastDelete = new System.Windows.Forms.MenuItem();
            this.ultraTabPageControl8 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlRollup = new System.Windows.Forms.Panel();
            this.ulgRollup = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ctmRollupMenu = new System.Windows.Forms.ContextMenu();
            this.mniRollupDelete = new System.Windows.Forms.MenuItem();
            this.ultraTabPageControl9 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlProgram = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnProgram = new System.Windows.Forms.Button();
            this.txtProgramParms = new System.Windows.Forms.TextBox();
            this.txtProgramPath = new System.Windows.Forms.TextBox();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.emailTaskList1 = new MIDRetail.Windows.Controls.EmailTaskList();
            this.ultraTabPageControl11 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlBatchComp = new System.Windows.Forms.Panel();
            this.cboBatchComp = new System.Windows.Forms.ComboBox();
            this.lblBatchComp = new System.Windows.Forms.Label();
            this.ultraTabPageControl12 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlHeaderReconcile = new System.Windows.Forms.Panel();
            this.ultraTabPageControl13 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl(); //TT#4574 - DOConnell - Purge Task List does not have an email option
            this.txtHeaderReconcileHeaderKeys = new System.Windows.Forms.TextBox();
            this.txtHeaderReconcileOutputDirectory = new System.Windows.Forms.TextBox();
            this.txtHeaderReconcileInputDirectory = new System.Windows.Forms.TextBox();
            this.btnHeaderReconcileHeaderKeys = new System.Windows.Forms.Button();
            this.lblHRHeaderKeysFile = new System.Windows.Forms.Label();
            this.lblHRHeaderTypes = new System.Windows.Forms.Label();
            this.clbHeaderReconcileHeaderTypes = new System.Windows.Forms.CheckedListBox();
            this.txtHeaderReconcileRemoveTransactionsTriggerSuffix = new System.Windows.Forms.TextBox();
            this.lblHRRemoveTransactionsTriggerSuffix = new System.Windows.Forms.Label();
            this.txtHeaderReconcileRemoveTransactionsFileName = new System.Windows.Forms.TextBox();
            this.txtHeaderReconcileTriggerSuffix = new System.Windows.Forms.TextBox();
            this.lblHRRemoveTransactionsFileName = new System.Windows.Forms.Label();
            this.lblHRTriggerSuffix = new System.Windows.Forms.Label();
            this.btnHeaderReconcileOutputDirectory = new System.Windows.Forms.Button();
            this.btnHeaderReconcileInputDirectory = new System.Windows.Forms.Button();
            this.lblHROutputDirectory = new System.Windows.Forms.Label();
            this.lblHRInputDirectory = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtTaskListName = new System.Windows.Forms.TextBox();
            this.lblFilterName = new System.Windows.Forms.Label();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.pnlTasks = new System.Windows.Forms.Panel();
            this.ulgTasks = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ctmTaskMenu = new System.Windows.Forms.ContextMenu();
            this.mniTaskDelete = new System.Windows.Forms.MenuItem();
            this.btnSave = new System.Windows.Forms.Button();
            this.rdoGlobal = new System.Windows.Forms.RadioButton();
            this.rdoUser = new System.Windows.Forms.RadioButton();
            this.rdoSystem = new System.Windows.Forms.RadioButton();
            this.fbdDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.ctmAddTask = new System.Windows.Forms.ContextMenu();
            this.btnRunNow = new System.Windows.Forms.Button();
            this.ofdFile = new System.Windows.Forms.OpenFileDialog();
            this.rdoOwner = new System.Windows.Forms.RadioButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.ultraTabPageControl10.SuspendLayout();
            this.pnlSizeCurves.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ulgSizeCurves)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSizeCurveConcurrentProcesses)).BeginInit();
            this.ultraTabPageControl1.SuspendLayout();
            this.pnlPosting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPostingConcurrent)).BeginInit();
            this.ultraTabPageControl3.SuspendLayout();
            this.pnlSizeDayToWeekSummary.SuspendLayout();
            this.ultraTabPageControl4.SuspendLayout();
            this.pnlSizeCurveMethod.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ulgSizeCurveMethod)).BeginInit();
            this.ultraTabPageControl5.SuspendLayout();
            this.pnlRelieve.SuspendLayout();
            this.ultraTabPageControl6.SuspendLayout();
            this.pnlAllocate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ulgAllocate)).BeginInit();
            this.ultraTabPageControl7.SuspendLayout();
            this.pnlForecast.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ulgForecast)).BeginInit();
            this.ultraTabPageControl8.SuspendLayout();
            this.pnlRollup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ulgRollup)).BeginInit();
            this.ultraTabPageControl9.SuspendLayout();
            this.pnlProgram.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            this.ultraTabPageControl11.SuspendLayout();
            this.pnlBatchComp.SuspendLayout();
            this.ultraTabPageControl12.SuspendLayout();
            this.pnlHeaderReconcile.SuspendLayout();
            this.ultraTabPageControl13.SuspendLayout(); //TT#4574 - DOConnell - Purge Task List does not have an email option
            this.pnlTasks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ulgTasks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this.ultraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // ultraTabPageControl10
            // 
            this.ultraTabPageControl10.Controls.Add(this.pnlSizeCurves);
            this.ultraTabPageControl10.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl10.Name = "ultraTabPageControl10";
            this.ultraTabPageControl10.Size = new System.Drawing.Size(570, 400);
            // 
            // pnlSizeCurves
            // 
            this.pnlSizeCurves.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSizeCurves.Controls.Add(this.ulgSizeCurves);
            this.pnlSizeCurves.Controls.Add(this.nudSizeCurveConcurrentProcesses);
            this.pnlSizeCurves.Controls.Add(this.lblSizeCurveConcurrentProcesses);
            this.pnlSizeCurves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSizeCurves.Location = new System.Drawing.Point(0, 0);
            this.pnlSizeCurves.Name = "pnlSizeCurves";
            this.pnlSizeCurves.Size = new System.Drawing.Size(570, 400);
            this.pnlSizeCurves.TabIndex = 24;
            // 
            // ulgSizeCurves
            // 
            this.ulgSizeCurves.AllowDrop = true;
            this.ulgSizeCurves.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ulgSizeCurves.ContextMenu = this.ctmSizeCurvesMenu;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ulgSizeCurves.DisplayLayout.Appearance = appearance1;
            this.ulgSizeCurves.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ulgSizeCurves.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ulgSizeCurves.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgSizeCurves.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ulgSizeCurves.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ulgSizeCurves.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ulgSizeCurves.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ulgSizeCurves.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ulgSizeCurves.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgSizeCurves.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ulgSizeCurves.Location = new System.Drawing.Point(8, 8);
            this.ulgSizeCurves.Name = "ulgSizeCurves";
            this.ulgSizeCurves.Size = new System.Drawing.Size(553, 363);
            this.ulgSizeCurves.TabIndex = 104;
            this.ulgSizeCurves.Text = "Size Curves";
            this.ulgSizeCurves.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurves_AfterCellUpdate);
            this.ulgSizeCurves.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgSizeCurves_InitializeLayout);
            this.ulgSizeCurves.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ulgSizeCurves_AfterRowInsert);
            this.ulgSizeCurves.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurves_CellChange);
            this.ulgSizeCurves.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
            this.ulgSizeCurves.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ulgSizeCurves_BeforeRowInsert);
            this.ulgSizeCurves.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgSizeCurves_BeforeRowsDeleted);
            this.ulgSizeCurves.DragDrop += new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
            this.ulgSizeCurves.DragOver += new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
            // 
            // ctmSizeCurvesMenu
            // 
            this.ctmSizeCurvesMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniSizeCurvesDelete});
            this.ctmSizeCurvesMenu.Popup += new System.EventHandler(this.ctmSizeCurvesMenu_Popup);
            // 
            // mniSizeCurvesDelete
            // 
            this.mniSizeCurvesDelete.Index = 0;
            this.mniSizeCurvesDelete.Text = "Delete";
            this.mniSizeCurvesDelete.Click += new System.EventHandler(this.mniSizeCurvesDelete_Click);
            // 
            // nudSizeCurveConcurrentProcesses
            // 
            this.nudSizeCurveConcurrentProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudSizeCurveConcurrentProcesses.Location = new System.Drawing.Point(119, 377);
            this.nudSizeCurveConcurrentProcesses.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSizeCurveConcurrentProcesses.Name = "nudSizeCurveConcurrentProcesses";
            this.nudSizeCurveConcurrentProcesses.Size = new System.Drawing.Size(48, 20);
            this.nudSizeCurveConcurrentProcesses.TabIndex = 103;
            this.nudSizeCurveConcurrentProcesses.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSizeCurveConcurrentProcesses.ValueChanged += new System.EventHandler(this.nudSizeCurveConcurrentProcesses_ValueChanged);
            // 
            // lblSizeCurveConcurrentProcesses
            // 
            this.lblSizeCurveConcurrentProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSizeCurveConcurrentProcesses.AutoSize = true;
            this.lblSizeCurveConcurrentProcesses.BackColor = System.Drawing.Color.Transparent;
            this.lblSizeCurveConcurrentProcesses.Location = new System.Drawing.Point(2, 379);
            this.lblSizeCurveConcurrentProcesses.Name = "lblSizeCurveConcurrentProcesses";
            this.lblSizeCurveConcurrentProcesses.Size = new System.Drawing.Size(111, 13);
            this.lblSizeCurveConcurrentProcesses.TabIndex = 102;
            this.lblSizeCurveConcurrentProcesses.Text = "Concurrent Processes";
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.pnlPosting);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(570, 400);
            // 
            // pnlPosting
            // 
            this.pnlPosting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPosting.Controls.Add(this.cboProcessingDirection);
            this.pnlPosting.Controls.Add(this.lblProcessingDirection);
            this.pnlPosting.Controls.Add(this.lblPostingNote);
            this.pnlPosting.Controls.Add(this.txtPostingRunUntilMask);
            this.pnlPosting.Controls.Add(this.chkPostingRunUntil);
            this.pnlPosting.Controls.Add(this.lblPosting);
            this.pnlPosting.Controls.Add(this.lblPostingConcurrentFiles);
            this.pnlPosting.Controls.Add(this.label3);
            this.pnlPosting.Controls.Add(this.label1);
            this.pnlPosting.Controls.Add(this.nudPostingConcurrent);
            this.pnlPosting.Controls.Add(this.btnPostingDirectory);
            this.pnlPosting.Controls.Add(this.txtPostingMask);
            this.pnlPosting.Controls.Add(this.txtPostingDirectory);
            this.pnlPosting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPosting.Location = new System.Drawing.Point(0, 0);
            this.pnlPosting.Name = "pnlPosting";
            this.pnlPosting.Size = new System.Drawing.Size(570, 400);
            this.pnlPosting.TabIndex = 18;
            // 
            // cboProcessingDirection
            // 
            this.cboProcessingDirection.AutoAdjust = true;
            this.cboProcessingDirection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboProcessingDirection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboProcessingDirection.DataSource = null;
            this.cboProcessingDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProcessingDirection.DropDownWidth = 120;
            this.cboProcessingDirection.FormattingEnabled = false;
            this.cboProcessingDirection.IgnoreFocusLost = false;
            this.cboProcessingDirection.ItemHeight = 13;
            this.cboProcessingDirection.Location = new System.Drawing.Point(112, 176);
            this.cboProcessingDirection.Margin = new System.Windows.Forms.Padding(0);
            this.cboProcessingDirection.MaxDropDownItems = 25;
            this.cboProcessingDirection.Name = "cboProcessingDirection";
            this.cboProcessingDirection.SetToolTip = "";
            this.cboProcessingDirection.Size = new System.Drawing.Size(120, 23);
            this.cboProcessingDirection.TabIndex = 8;
            this.cboProcessingDirection.Tag = null;
            this.cboProcessingDirection.SelectionChangeCommitted += new System.EventHandler(this.cboProcessingDirection_SelectionChangeCommitted);
            // 
            // lblProcessingDirection
            // 
            this.lblProcessingDirection.AutoSize = true;
            this.lblProcessingDirection.Location = new System.Drawing.Point(3, 180);
            this.lblProcessingDirection.Name = "lblProcessingDirection";
            this.lblProcessingDirection.Size = new System.Drawing.Size(107, 13);
            this.lblProcessingDirection.TabIndex = 7;
            this.lblProcessingDirection.Text = "Processing Direction:";
            this.lblProcessingDirection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPostingNote
            // 
            this.lblPostingNote.Location = new System.Drawing.Point(112, 248);
            this.lblPostingNote.Name = "lblPostingNote";
            this.lblPostingNote.Size = new System.Drawing.Size(360, 128);
            this.lblPostingNote.TabIndex = 8;
            // 
            // txtPostingRunUntilMask
            // 
            this.txtPostingRunUntilMask.Location = new System.Drawing.Point(288, 213);
            this.txtPostingRunUntilMask.Name = "txtPostingRunUntilMask";
            this.txtPostingRunUntilMask.Size = new System.Drawing.Size(96, 20);
            this.txtPostingRunUntilMask.TabIndex = 10;
            this.txtPostingRunUntilMask.TextChanged += new System.EventHandler(this.txtPostingRunUntilMask_TextChanged);
            // 
            // chkPostingRunUntil
            // 
            this.chkPostingRunUntil.Location = new System.Drawing.Point(112, 211);
            this.chkPostingRunUntil.Name = "chkPostingRunUntil";
            this.chkPostingRunUntil.Size = new System.Drawing.Size(184, 24);
            this.chkPostingRunUntil.TabIndex = 9;
            this.chkPostingRunUntil.Text = "Run until file present with suffix:";
            this.chkPostingRunUntil.CheckedChanged += new System.EventHandler(this.chkPostingRunUntil_CheckedChanged);
            // 
            // lblPosting
            // 
            this.lblPosting.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblPosting.Location = new System.Drawing.Point(183, 8);
            this.lblPosting.Name = "lblPosting";
            this.lblPosting.Size = new System.Drawing.Size(203, 16);
            this.lblPosting.TabIndex = 7;
            this.lblPosting.Text = "Posting";
            this.lblPosting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPostingConcurrentFiles
            // 
            this.lblPostingConcurrentFiles.Location = new System.Drawing.Point(14, 146);
            this.lblPostingConcurrentFiles.Name = "lblPostingConcurrentFiles";
            this.lblPostingConcurrentFiles.Size = new System.Drawing.Size(96, 16);
            this.lblPostingConcurrentFiles.TabIndex = 5;
            this.lblPostingConcurrentFiles.Text = "Concurrent Files:";
            this.lblPostingConcurrentFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(22, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Flag File Suffix:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(22, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Directory:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudPostingConcurrent
            // 
            this.nudPostingConcurrent.Location = new System.Drawing.Point(112, 144);
            this.nudPostingConcurrent.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPostingConcurrent.Name = "nudPostingConcurrent";
            this.nudPostingConcurrent.Size = new System.Drawing.Size(48, 20);
            this.nudPostingConcurrent.TabIndex = 6;
            this.nudPostingConcurrent.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPostingConcurrent.ValueChanged += new System.EventHandler(this.nudPostingConcurrent_ValueChanged);
            // 
            // btnPostingDirectory
            // 
            this.btnPostingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPostingDirectory.Location = new System.Drawing.Point(486, 80);
            this.btnPostingDirectory.Name = "btnPostingDirectory";
            this.btnPostingDirectory.Size = new System.Drawing.Size(72, 20);
            this.btnPostingDirectory.TabIndex = 2;
            this.btnPostingDirectory.Text = "Directory...";
            this.btnPostingDirectory.Click += new System.EventHandler(this.btnPostingDirectory_Click);
            // 
            // txtPostingMask
            // 
            this.txtPostingMask.Location = new System.Drawing.Point(112, 112);
            this.txtPostingMask.Name = "txtPostingMask";
            this.txtPostingMask.Size = new System.Drawing.Size(120, 20);
            this.txtPostingMask.TabIndex = 4;
            this.txtPostingMask.TextChanged += new System.EventHandler(this.txtPostingMask_TextChanged);
            // 
            // txtPostingDirectory
            // 
            this.txtPostingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPostingDirectory.Location = new System.Drawing.Point(112, 80);
            this.txtPostingDirectory.Name = "txtPostingDirectory";
            this.txtPostingDirectory.Size = new System.Drawing.Size(366, 20);
            this.txtPostingDirectory.TabIndex = 1;
            this.txtPostingDirectory.TextChanged += new System.EventHandler(this.txtPostingDirectory_TextChanged);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.pnlSizeDayToWeekSummary);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(570, 400);
            // 
            // pnlSizeDayToWeekSummary
            // 
            this.pnlSizeDayToWeekSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSizeDayToWeekSummary.Controls.Add(this.lblOverrideNode);
            this.pnlSizeDayToWeekSummary.Controls.Add(this.txtOverrideNode);
            this.pnlSizeDayToWeekSummary.Controls.Add(this.lblSizeDayWeekSummaryDt);
            this.pnlSizeDayToWeekSummary.Controls.Add(this.midDateRangeSizeDayWeekSum);
            this.pnlSizeDayToWeekSummary.Controls.Add(this.lblSizeDayWeekSummary);
            this.pnlSizeDayToWeekSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSizeDayToWeekSummary.Location = new System.Drawing.Point(0, 0);
            this.pnlSizeDayToWeekSummary.Name = "pnlSizeDayToWeekSummary";
            this.pnlSizeDayToWeekSummary.Size = new System.Drawing.Size(570, 400);
            this.pnlSizeDayToWeekSummary.TabIndex = 2;
            // 
            // lblOverrideNode
            // 
            this.lblOverrideNode.AutoSize = true;
            this.lblOverrideNode.Location = new System.Drawing.Point(103, 189);
            this.lblOverrideNode.Name = "lblOverrideNode";
            this.lblOverrideNode.Size = new System.Drawing.Size(114, 13);
            this.lblOverrideNode.TabIndex = 4;
            this.lblOverrideNode.Text = "Override Merchandise:";
            // 
            // txtOverrideNode
            // 
            this.txtOverrideNode.AllowDrop = true;
            this.txtOverrideNode.Location = new System.Drawing.Point(231, 185);
            this.txtOverrideNode.Name = "txtOverrideNode";
            this.txtOverrideNode.Size = new System.Drawing.Size(156, 20);
            this.txtOverrideNode.TabIndex = 3;
            this.txtOverrideNode.TextChanged += new System.EventHandler(this.txtOverrideNode_TextChanged);
            this.txtOverrideNode.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtOverrideNode_DragDrop);
            this.txtOverrideNode.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtOverrideNode_DragEnter);
            this.txtOverrideNode.DragOver += new System.Windows.Forms.DragEventHandler(this.txtOverrideNode_DragOver);
            this.txtOverrideNode.Validating += new System.ComponentModel.CancelEventHandler(this.txtOverrideNode_Validating);
            this.txtOverrideNode.Validated += new System.EventHandler(this.txtOverrideNode_Validated);
            // 
            // lblSizeDayWeekSummaryDt
            // 
            this.lblSizeDayWeekSummaryDt.AutoSize = true;
            this.lblSizeDayWeekSummaryDt.Location = new System.Drawing.Point(100, 149);
            this.lblSizeDayWeekSummaryDt.Name = "lblSizeDayWeekSummaryDt";
            this.lblSizeDayWeekSummaryDt.Size = new System.Drawing.Size(127, 13);
            this.lblSizeDayWeekSummaryDt.TabIndex = 2;
            this.lblSizeDayWeekSummaryDt.Text = "Week Range to Process:";
            // 
            // midDateRangeSizeDayWeekSum
            // 
            this.midDateRangeSizeDayWeekSum.DateRangeForm = null;
            this.midDateRangeSizeDayWeekSum.DateRangeRID = 0;
            this.midDateRangeSizeDayWeekSum.Location = new System.Drawing.Point(231, 144);
            this.midDateRangeSizeDayWeekSum.Name = "midDateRangeSizeDayWeekSum";
            this.midDateRangeSizeDayWeekSum.Size = new System.Drawing.Size(156, 24);
            this.midDateRangeSizeDayWeekSum.TabIndex = 1;
            this.midDateRangeSizeDayWeekSum.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSizeDayWeekSum_OnSelection);
            this.midDateRangeSizeDayWeekSum.Click += new System.EventHandler(this.midDateRangeSizeDayWeekSum_Click);
            // 
            // lblSizeDayWeekSummary
            // 
            this.lblSizeDayWeekSummary.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblSizeDayWeekSummary.AutoSize = true;
            this.lblSizeDayWeekSummary.Location = new System.Drawing.Point(213, 8);
            this.lblSizeDayWeekSummary.Name = "lblSizeDayWeekSummary";
            this.lblSizeDayWeekSummary.Size = new System.Drawing.Size(143, 13);
            this.lblSizeDayWeekSummary.TabIndex = 0;
            this.lblSizeDayWeekSummary.Text = "Size Day To Week Summary";
            this.lblSizeDayWeekSummary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this.pnlSizeCurveMethod);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(570, 400);
            // 
            // pnlSizeCurveMethod
            // 
            this.pnlSizeCurveMethod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSizeCurveMethod.Controls.Add(this.ulgSizeCurveMethod);
            this.pnlSizeCurveMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSizeCurveMethod.Location = new System.Drawing.Point(0, 0);
            this.pnlSizeCurveMethod.Name = "pnlSizeCurveMethod";
            this.pnlSizeCurveMethod.Size = new System.Drawing.Size(570, 400);
            this.pnlSizeCurveMethod.TabIndex = 23;
            // 
            // ulgSizeCurveMethod
            // 
            this.ulgSizeCurveMethod.AllowDrop = true;
            this.ulgSizeCurveMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ulgSizeCurveMethod.ContextMenu = this.ctmSizeCurveMethodMenu;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ulgSizeCurveMethod.DisplayLayout.Appearance = appearance7;
            this.ulgSizeCurveMethod.DisplayLayout.InterBandSpacing = 10;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.ulgSizeCurveMethod.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ulgSizeCurveMethod.DisplayLayout.Override.HeaderAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgSizeCurveMethod.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ulgSizeCurveMethod.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.ulgSizeCurveMethod.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ulgSizeCurveMethod.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.ulgSizeCurveMethod.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.ulgSizeCurveMethod.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgSizeCurveMethod.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ulgSizeCurveMethod.Location = new System.Drawing.Point(8, 8);
            this.ulgSizeCurveMethod.Name = "ulgSizeCurveMethod";
            this.ulgSizeCurveMethod.Size = new System.Drawing.Size(550, 384);
            this.ulgSizeCurveMethod.TabIndex = 0;
            this.ulgSizeCurveMethod.Text = "Size Curve Method";
            this.ulgSizeCurveMethod.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurveMethod_AfterCellUpdate);
            this.ulgSizeCurveMethod.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgSizeCurveMethod_InitializeLayout);
            this.ulgSizeCurveMethod.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ulgSizeCurveMethod_AfterRowInsert);
            this.ulgSizeCurveMethod.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurveMethod_CellChange);
            this.ulgSizeCurveMethod.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgSizeCurveMethod_ClickCellButton);
            this.ulgSizeCurveMethod.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
            this.ulgSizeCurveMethod.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ulgSizeCurveMethod_BeforeRowInsert);
            this.ulgSizeCurveMethod.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgSizeCurveMethod_BeforeRowsDeleted);
            this.ulgSizeCurveMethod.DragDrop += new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
            this.ulgSizeCurveMethod.DragOver += new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
            // 
            // ctmSizeCurveMethodMenu
            // 
            this.ctmSizeCurveMethodMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniSizeCurveMethodDelete});
            this.ctmSizeCurveMethodMenu.Popup += new System.EventHandler(this.ctmSizeCurveMethodMenu_Popup);
            // 
            // mniSizeCurveMethodDelete
            // 
            this.mniSizeCurveMethodDelete.Index = 0;
            this.mniSizeCurveMethodDelete.Text = "Delete";
            this.mniSizeCurveMethodDelete.Click += new System.EventHandler(this.mniSizeCurveMethodDelete_Click);
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Controls.Add(this.pnlRelieve);
            this.ultraTabPageControl5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(570, 400);
            // 
            // pnlRelieve
            // 
            this.pnlRelieve.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRelieve.Controls.Add(this.txtRelieveRunUntilMask);
            this.pnlRelieve.Controls.Add(this.chkRelieveRunUntil);
            this.pnlRelieve.Controls.Add(this.lblRelieve);
            this.pnlRelieve.Controls.Add(this.label7);
            this.pnlRelieve.Controls.Add(this.label8);
            this.pnlRelieve.Controls.Add(this.btnRelieveDirectory);
            this.pnlRelieve.Controls.Add(this.txtRelieveMask);
            this.pnlRelieve.Controls.Add(this.txtRelieveDirectory);
            this.pnlRelieve.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRelieve.Location = new System.Drawing.Point(0, 0);
            this.pnlRelieve.Name = "pnlRelieve";
            this.pnlRelieve.Size = new System.Drawing.Size(570, 400);
            this.pnlRelieve.TabIndex = 18;
            // 
            // txtRelieveRunUntilMask
            // 
            this.txtRelieveRunUntilMask.Location = new System.Drawing.Point(288, 144);
            this.txtRelieveRunUntilMask.Name = "txtRelieveRunUntilMask";
            this.txtRelieveRunUntilMask.Size = new System.Drawing.Size(96, 20);
            this.txtRelieveRunUntilMask.TabIndex = 4;
            this.txtRelieveRunUntilMask.TextChanged += new System.EventHandler(this.txtRelieveRunUntilMask_TextChanged);
            // 
            // chkRelieveRunUntil
            // 
            this.chkRelieveRunUntil.Location = new System.Drawing.Point(112, 142);
            this.chkRelieveRunUntil.Name = "chkRelieveRunUntil";
            this.chkRelieveRunUntil.Size = new System.Drawing.Size(184, 24);
            this.chkRelieveRunUntil.TabIndex = 3;
            this.chkRelieveRunUntil.Text = "Run until file present with suffix:";
            this.chkRelieveRunUntil.CheckedChanged += new System.EventHandler(this.chkRelieveRunUntil_CheckedChanged);
            // 
            // lblRelieve
            // 
            this.lblRelieve.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblRelieve.Location = new System.Drawing.Point(162, 8);
            this.lblRelieve.Name = "lblRelieve";
            this.lblRelieve.Size = new System.Drawing.Size(245, 16);
            this.lblRelieve.TabIndex = 7;
            this.lblRelieve.Text = "Relieve Intransit";
            this.lblRelieve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 16);
            this.label7.TabIndex = 5;
            this.label7.Text = "Flag File Suffix:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(16, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 16);
            this.label8.TabIndex = 4;
            this.label8.Text = "Directory:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRelieveDirectory
            // 
            this.btnRelieveDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRelieveDirectory.Location = new System.Drawing.Point(482, 80);
            this.btnRelieveDirectory.Name = "btnRelieveDirectory";
            this.btnRelieveDirectory.Size = new System.Drawing.Size(72, 20);
            this.btnRelieveDirectory.TabIndex = 1;
            this.btnRelieveDirectory.Text = "Directory...";
            this.btnRelieveDirectory.Click += new System.EventHandler(this.btnRelieveDirectory_Click);
            // 
            // txtRelieveMask
            // 
            this.txtRelieveMask.Location = new System.Drawing.Point(112, 112);
            this.txtRelieveMask.Name = "txtRelieveMask";
            this.txtRelieveMask.Size = new System.Drawing.Size(120, 20);
            this.txtRelieveMask.TabIndex = 2;
            this.txtRelieveMask.TextChanged += new System.EventHandler(this.txtRelieveMask_TextChanged);
            // 
            // txtRelieveDirectory
            // 
            this.txtRelieveDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRelieveDirectory.Location = new System.Drawing.Point(112, 80);
            this.txtRelieveDirectory.Name = "txtRelieveDirectory";
            this.txtRelieveDirectory.Size = new System.Drawing.Size(362, 20);
            this.txtRelieveDirectory.TabIndex = 0;
            this.txtRelieveDirectory.TextChanged += new System.EventHandler(this.txtRelieveDirectory_TextChanged);
            // 
            // ultraTabPageControl6
            // 
            this.ultraTabPageControl6.Controls.Add(this.pnlAllocate);
            this.ultraTabPageControl6.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl6.Name = "ultraTabPageControl6";
            this.ultraTabPageControl6.Size = new System.Drawing.Size(570, 400);
            // 
            // pnlAllocate
            // 
            this.pnlAllocate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAllocate.Controls.Add(this.ulgAllocate);
            this.pnlAllocate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAllocate.Location = new System.Drawing.Point(0, 0);
            this.pnlAllocate.Name = "pnlAllocate";
            this.pnlAllocate.Size = new System.Drawing.Size(570, 400);
            this.pnlAllocate.TabIndex = 20;
            // 
            // ulgAllocate
            // 
            this.ulgAllocate.AllowDrop = true;
            this.ulgAllocate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ulgAllocate.ContextMenu = this.ctmAllocateMenu;
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ulgAllocate.DisplayLayout.Appearance = appearance13;
            this.ulgAllocate.DisplayLayout.InterBandSpacing = 10;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            this.ulgAllocate.DisplayLayout.Override.CardAreaAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.TextHAlignAsString = "Left";
            appearance15.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ulgAllocate.DisplayLayout.Override.HeaderAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgAllocate.DisplayLayout.Override.RowAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ulgAllocate.DisplayLayout.Override.RowSelectorAppearance = appearance17;
            this.ulgAllocate.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ulgAllocate.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.ulgAllocate.DisplayLayout.Override.SelectedRowAppearance = appearance18;
            this.ulgAllocate.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgAllocate.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ulgAllocate.Location = new System.Drawing.Point(8, 8);
            this.ulgAllocate.Name = "ulgAllocate";
            this.ulgAllocate.Size = new System.Drawing.Size(550, 384);
            this.ulgAllocate.TabIndex = 0;
            this.ulgAllocate.Text = "Allocate";
            this.ulgAllocate.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgAllocate_AfterCellUpdate);
            this.ulgAllocate.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgAllocate_InitializeLayout);
            this.ulgAllocate.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ulgAllocate_AfterRowInsert);
            this.ulgAllocate.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgAllocate_CellChange);
            this.ulgAllocate.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgAllocate_ClickCellButton);
            this.ulgAllocate.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
            this.ulgAllocate.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ulgAllocate_BeforeRowInsert);
            this.ulgAllocate.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgAllocate_BeforeRowsDeleted);
            this.ulgAllocate.DragDrop += new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
            this.ulgAllocate.DragOver += new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
            // 
            // ctmAllocateMenu
            // 
            this.ctmAllocateMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniAllocateDelete});
            this.ctmAllocateMenu.Popup += new System.EventHandler(this.ctmAllocateMenu_Popup);
            // 
            // mniAllocateDelete
            // 
            this.mniAllocateDelete.Index = 0;
            this.mniAllocateDelete.Text = "Delete";
            this.mniAllocateDelete.Click += new System.EventHandler(this.mniAllocateDelete_Click);
            // 
            // ultraTabPageControl7
            // 
            this.ultraTabPageControl7.Controls.Add(this.pnlForecast);
            this.ultraTabPageControl7.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl7.Name = "ultraTabPageControl7";
            this.ultraTabPageControl7.Size = new System.Drawing.Size(570, 400);
            // 
            // pnlForecast
            // 
            this.pnlForecast.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlForecast.Controls.Add(this.ulgForecast);
            this.pnlForecast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlForecast.Location = new System.Drawing.Point(0, 0);
            this.pnlForecast.Name = "pnlForecast";
            this.pnlForecast.Size = new System.Drawing.Size(570, 400);
            this.pnlForecast.TabIndex = 17;
            // 
            // ulgForecast
            // 
            this.ulgForecast.AllowDrop = true;
            this.ulgForecast.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ulgForecast.ContextMenu = this.ctmForecastMenu;
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ulgForecast.DisplayLayout.Appearance = appearance19;
            this.ulgForecast.DisplayLayout.InterBandSpacing = 10;
            appearance20.BackColor = System.Drawing.Color.Transparent;
            this.ulgForecast.DisplayLayout.Override.CardAreaAppearance = appearance20;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance21.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.ForeColor = System.Drawing.Color.Black;
            appearance21.TextHAlignAsString = "Left";
            appearance21.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ulgForecast.DisplayLayout.Override.HeaderAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgForecast.DisplayLayout.Override.RowAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ulgForecast.DisplayLayout.Override.RowSelectorAppearance = appearance23;
            this.ulgForecast.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ulgForecast.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.ForeColor = System.Drawing.Color.Black;
            this.ulgForecast.DisplayLayout.Override.SelectedRowAppearance = appearance24;
            this.ulgForecast.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgForecast.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ulgForecast.Location = new System.Drawing.Point(8, 8);
            this.ulgForecast.Name = "ulgForecast";
            this.ulgForecast.Size = new System.Drawing.Size(550, 384);
            this.ulgForecast.TabIndex = 0;
            this.ulgForecast.Text = "Forecast";
            this.ulgForecast.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgForecast_AfterCellUpdate);
            this.ulgForecast.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgForecast_InitializeLayout);
            this.ulgForecast.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ulgForecast_AfterRowInsert);
            this.ulgForecast.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgForecast_CellChange);
            this.ulgForecast.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgForecast_ClickCellButton);
            this.ulgForecast.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
            this.ulgForecast.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ulgForecast_BeforeRowInsert);
            this.ulgForecast.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgForecast_BeforeRowsDeleted);
            this.ulgForecast.DragDrop += new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
            this.ulgForecast.DragOver += new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
            // 
            // ctmForecastMenu
            // 
            this.ctmForecastMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniForecastDelete});
            this.ctmForecastMenu.Popup += new System.EventHandler(this.ctmForecastMenu_Popup);
            // 
            // mniForecastDelete
            // 
            this.mniForecastDelete.Index = 0;
            this.mniForecastDelete.Text = "Delete";
            this.mniForecastDelete.Click += new System.EventHandler(this.mniForecastDelete_Click);
            // 
            // ultraTabPageControl8
            // 
            this.ultraTabPageControl8.Controls.Add(this.pnlRollup);
            this.ultraTabPageControl8.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl8.Name = "ultraTabPageControl8";
            this.ultraTabPageControl8.Size = new System.Drawing.Size(570, 400);
            // 
            // pnlRollup
            // 
            this.pnlRollup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRollup.Controls.Add(this.ulgRollup);
            this.pnlRollup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRollup.Location = new System.Drawing.Point(0, 0);
            this.pnlRollup.Name = "pnlRollup";
            this.pnlRollup.Size = new System.Drawing.Size(570, 400);
            this.pnlRollup.TabIndex = 21;
            // 
            // ulgRollup
            // 
            this.ulgRollup.AllowDrop = true;
            this.ulgRollup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ulgRollup.ContextMenu = this.ctmRollupMenu;
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ulgRollup.DisplayLayout.Appearance = appearance25;
            this.ulgRollup.DisplayLayout.InterBandSpacing = 10;
            appearance26.BackColor = System.Drawing.Color.Transparent;
            this.ulgRollup.DisplayLayout.Override.CardAreaAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance27.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance27.ForeColor = System.Drawing.Color.Black;
            appearance27.TextHAlignAsString = "Left";
            appearance27.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ulgRollup.DisplayLayout.Override.HeaderAppearance = appearance27;
            appearance28.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgRollup.DisplayLayout.Override.RowAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance29.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ulgRollup.DisplayLayout.Override.RowSelectorAppearance = appearance29;
            this.ulgRollup.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ulgRollup.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance30.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance30.ForeColor = System.Drawing.Color.Black;
            this.ulgRollup.DisplayLayout.Override.SelectedRowAppearance = appearance30;
            this.ulgRollup.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgRollup.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ulgRollup.Location = new System.Drawing.Point(8, 8);
            this.ulgRollup.Name = "ulgRollup";
            this.ulgRollup.Size = new System.Drawing.Size(550, 384);
            this.ulgRollup.TabIndex = 0;
            this.ulgRollup.Text = "Rollup";
            this.ulgRollup.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgRollup_AfterCellUpdate);
            this.ulgRollup.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgRollup_InitializeLayout);
            this.ulgRollup.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ulgRollup_AfterRowInsert);
            this.ulgRollup.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgRollup_ClickCellButton);
            this.ulgRollup.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgRollup_AfterCellListCloseUp);
            this.ulgRollup.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
            this.ulgRollup.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ulgRollup_BeforeRowInsert);
            this.ulgRollup.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgRollup_BeforeRowsDeleted);
            this.ulgRollup.DragDrop += new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
            this.ulgRollup.DragOver += new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
            // 
            // ctmRollupMenu
            // 
            this.ctmRollupMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniRollupDelete});
            this.ctmRollupMenu.Popup += new System.EventHandler(this.ctmRollupMenu_Popup);
            // 
            // mniRollupDelete
            // 
            this.mniRollupDelete.Index = 0;
            this.mniRollupDelete.Text = "Delete";
            this.mniRollupDelete.Click += new System.EventHandler(this.mniRollupDelete_Click);
            // 
            // ultraTabPageControl9
            // 
            this.ultraTabPageControl9.Controls.Add(this.pnlProgram);
            this.ultraTabPageControl9.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl9.Name = "ultraTabPageControl9";
            this.ultraTabPageControl9.Size = new System.Drawing.Size(570, 400);
            // 
            // pnlProgram
            // 
            this.pnlProgram.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProgram.Controls.Add(this.label2);
            this.pnlProgram.Controls.Add(this.label6);
            this.pnlProgram.Controls.Add(this.label9);
            this.pnlProgram.Controls.Add(this.btnProgram);
            this.pnlProgram.Controls.Add(this.txtProgramParms);
            this.pnlProgram.Controls.Add(this.txtProgramPath);
            this.pnlProgram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProgram.Location = new System.Drawing.Point(0, 0);
            this.pnlProgram.Name = "pnlProgram";
            this.pnlProgram.Size = new System.Drawing.Size(570, 400);
            this.pnlProgram.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.Location = new System.Drawing.Point(183, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "External Program";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "Parameters:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(16, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 16);
            this.label9.TabIndex = 4;
            this.label9.Text = "Program Path:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnProgram
            // 
            this.btnProgram.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProgram.Location = new System.Drawing.Point(482, 80);
            this.btnProgram.Name = "btnProgram";
            this.btnProgram.Size = new System.Drawing.Size(72, 20);
            this.btnProgram.TabIndex = 1;
            this.btnProgram.Text = "Program...";
            this.btnProgram.Click += new System.EventHandler(this.btnProgram_Click);
            // 
            // txtProgramParms
            // 
            this.txtProgramParms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProgramParms.Location = new System.Drawing.Point(112, 112);
            this.txtProgramParms.Name = "txtProgramParms";
            this.txtProgramParms.Size = new System.Drawing.Size(412, 20);
            this.txtProgramParms.TabIndex = 2;
            this.txtProgramParms.TextChanged += new System.EventHandler(this.txtProgramParms_TextChanged);
            // 
            // txtProgramPath
            // 
            this.txtProgramPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProgramPath.Location = new System.Drawing.Point(112, 80);
            this.txtProgramPath.Name = "txtProgramPath";
            this.txtProgramPath.Size = new System.Drawing.Size(362, 20);
            this.txtProgramPath.TabIndex = 0;
            this.txtProgramPath.TextChanged += new System.EventHandler(this.txtProgramPath_TextChanged);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.emailTaskList1);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(570, 400);
            // 
            // emailTaskList1
            // 
            this.emailTaskList1.AutoScroll = true;
            this.emailTaskList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emailTaskList1.Location = new System.Drawing.Point(0, 0);
            this.emailTaskList1.Name = "emailTaskList1";
            this.emailTaskList1.Size = new System.Drawing.Size(570, 400);
            this.emailTaskList1.TabIndex = 0;
            // 
            // ultraTabPageControl11
            // 
            this.ultraTabPageControl11.Controls.Add(this.pnlBatchComp);
            this.ultraTabPageControl11.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl11.Name = "ultraTabPageControl11";
            this.ultraTabPageControl11.Size = new System.Drawing.Size(570, 400);
            // 
            // pnlBatchComp
            // 
            this.pnlBatchComp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBatchComp.Controls.Add(this.cboBatchComp);
            this.pnlBatchComp.Controls.Add(this.lblBatchComp);
            this.pnlBatchComp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBatchComp.Location = new System.Drawing.Point(0, 0);
            this.pnlBatchComp.Name = "pnlBatchComp";
            this.pnlBatchComp.Size = new System.Drawing.Size(570, 400);
            this.pnlBatchComp.TabIndex = 0;
            // 
            // cboBatchComp
            // 
            this.cboBatchComp.FormattingEnabled = true;
            this.cboBatchComp.Location = new System.Drawing.Point(121, 80);
            this.cboBatchComp.Name = "cboBatchComp";
            this.cboBatchComp.Size = new System.Drawing.Size(224, 21);
            this.cboBatchComp.TabIndex = 1;
            this.cboBatchComp.SelectionChangeCommitted += new System.EventHandler(this.cboBatchComp_SelectedChangeCommitted);
            // 
            // lblBatchComp
            // 
            this.lblBatchComp.AutoSize = true;
            this.lblBatchComp.Location = new System.Drawing.Point(46, 83);
            this.lblBatchComp.Name = "lblBatchComp";
            this.lblBatchComp.Size = new System.Drawing.Size(68, 13);
            this.lblBatchComp.TabIndex = 0;
            this.lblBatchComp.Text = "Batch Comp:";
            // 
            // ultraTabPageControl12
            // 
            this.ultraTabPageControl12.Controls.Add(this.pnlHeaderReconcile);
            this.ultraTabPageControl12.Location = new System.Drawing.Point(1, 23);
            this.ultraTabPageControl12.Name = "ultraTabPageControl12";
            this.ultraTabPageControl12.Size = new System.Drawing.Size(570, 400);
            // 
            // pnlHeaderReconcile
            // 
            this.pnlHeaderReconcile.Controls.Add(this.txtHeaderReconcileHeaderKeys);
            this.pnlHeaderReconcile.Controls.Add(this.txtHeaderReconcileOutputDirectory);
            this.pnlHeaderReconcile.Controls.Add(this.txtHeaderReconcileInputDirectory);
            this.pnlHeaderReconcile.Controls.Add(this.btnHeaderReconcileHeaderKeys);
            this.pnlHeaderReconcile.Controls.Add(this.lblHRHeaderKeysFile);
            this.pnlHeaderReconcile.Controls.Add(this.lblHRHeaderTypes);
            this.pnlHeaderReconcile.Controls.Add(this.clbHeaderReconcileHeaderTypes);
            this.pnlHeaderReconcile.Controls.Add(this.txtHeaderReconcileRemoveTransactionsTriggerSuffix);
            this.pnlHeaderReconcile.Controls.Add(this.lblHRRemoveTransactionsTriggerSuffix);
            this.pnlHeaderReconcile.Controls.Add(this.txtHeaderReconcileRemoveTransactionsFileName);
            this.pnlHeaderReconcile.Controls.Add(this.txtHeaderReconcileTriggerSuffix);
            this.pnlHeaderReconcile.Controls.Add(this.lblHRRemoveTransactionsFileName);
            this.pnlHeaderReconcile.Controls.Add(this.lblHRTriggerSuffix);
            this.pnlHeaderReconcile.Controls.Add(this.btnHeaderReconcileOutputDirectory);
            this.pnlHeaderReconcile.Controls.Add(this.btnHeaderReconcileInputDirectory);
            this.pnlHeaderReconcile.Controls.Add(this.lblHROutputDirectory);
            this.pnlHeaderReconcile.Controls.Add(this.lblHRInputDirectory);
            this.pnlHeaderReconcile.Location = new System.Drawing.Point(4, 4);
            this.pnlHeaderReconcile.Name = "pnlHeaderReconcile";
            this.pnlHeaderReconcile.Size = new System.Drawing.Size(563, 392);
            this.pnlHeaderReconcile.TabIndex = 0;
            // 
            // txtHeaderReconcileHeaderKeys
            // 
            this.txtHeaderReconcileHeaderKeys.Location = new System.Drawing.Point(158, 322);
            this.txtHeaderReconcileHeaderKeys.Name = "txtHeaderReconcileHeaderKeys";
            this.txtHeaderReconcileHeaderKeys.Size = new System.Drawing.Size(259, 20);
            this.txtHeaderReconcileHeaderKeys.TabIndex = 16;
            this.txtHeaderReconcileHeaderKeys.TextChanged += new System.EventHandler(this.txtHeaderReconcileHeaderKeys_TextChanged);
            // 
            // txtHeaderReconcileOutputDirectory
            // 
            this.txtHeaderReconcileOutputDirectory.Location = new System.Drawing.Point(158, 68);
            this.txtHeaderReconcileOutputDirectory.Name = "txtHeaderReconcileOutputDirectory";
            this.txtHeaderReconcileOutputDirectory.Size = new System.Drawing.Size(259, 20);
            this.txtHeaderReconcileOutputDirectory.TabIndex = 15;
            this.txtHeaderReconcileOutputDirectory.TextChanged += new System.EventHandler(this.txtHeaderReconcileOutputDirectory_TextChanged);
            // 
            // txtHeaderReconcileInputDirectory
            // 
            this.txtHeaderReconcileInputDirectory.Location = new System.Drawing.Point(158, 35);
            this.txtHeaderReconcileInputDirectory.Name = "txtHeaderReconcileInputDirectory";
            this.txtHeaderReconcileInputDirectory.Size = new System.Drawing.Size(259, 20);
            this.txtHeaderReconcileInputDirectory.TabIndex = 14;
            this.txtHeaderReconcileInputDirectory.TextChanged += new System.EventHandler(this.txtHeaderReconcileInputDirectory_TextChanged);
            // 
            // btnHeaderReconcileHeaderKeys
            // 
            this.btnHeaderReconcileHeaderKeys.Location = new System.Drawing.Point(447, 321);
            this.btnHeaderReconcileHeaderKeys.Name = "btnHeaderReconcileHeaderKeys";
            this.btnHeaderReconcileHeaderKeys.Size = new System.Drawing.Size(75, 23);
            this.btnHeaderReconcileHeaderKeys.TabIndex = 13;
            this.btnHeaderReconcileHeaderKeys.Text = "File";
            this.btnHeaderReconcileHeaderKeys.UseVisualStyleBackColor = true;
            this.btnHeaderReconcileHeaderKeys.Click += new System.EventHandler(this.btnHeaderReconcileHeaderKeys_Click);
            // 
            // lblHRHeaderKeysFile
            // 
            this.lblHRHeaderKeysFile.AutoSize = true;
            this.lblHRHeaderKeysFile.Location = new System.Drawing.Point(36, 327);
            this.lblHRHeaderKeysFile.Name = "lblHRHeaderKeysFile";
            this.lblHRHeaderKeysFile.Size = new System.Drawing.Size(35, 13);
            this.lblHRHeaderKeysFile.TabIndex = 12;
            this.lblHRHeaderKeysFile.Text = "label4";
            // 
            // lblHRHeaderTypes
            // 
            this.lblHRHeaderTypes.AutoSize = true;
            this.lblHRHeaderTypes.Location = new System.Drawing.Point(36, 200);
            this.lblHRHeaderTypes.Name = "lblHRHeaderTypes";
            this.lblHRHeaderTypes.Size = new System.Drawing.Size(35, 13);
            this.lblHRHeaderTypes.TabIndex = 11;
            this.lblHRHeaderTypes.Text = "label4";
            // 
            // clbHeaderReconcileHeaderTypes
            // 
            this.clbHeaderReconcileHeaderTypes.CheckOnClick = true;
            this.clbHeaderReconcileHeaderTypes.FormattingEnabled = true;
            this.clbHeaderReconcileHeaderTypes.Location = new System.Drawing.Point(158, 200);
            this.clbHeaderReconcileHeaderTypes.Name = "clbHeaderReconcileHeaderTypes";
            this.clbHeaderReconcileHeaderTypes.Size = new System.Drawing.Size(124, 109);
            this.clbHeaderReconcileHeaderTypes.TabIndex = 10;
            this.clbHeaderReconcileHeaderTypes.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbHeaderReconcileHeaderTypes_ItemCheck);
            this.clbHeaderReconcileHeaderTypes.Click += new System.EventHandler(this.clbHeaderReconcileHeaderTypes_Click);
            this.clbHeaderReconcileHeaderTypes.SelectedIndexChanged += new System.EventHandler(this.clbHeaderReconcileHeaderTypes_SelectedIndexChanged);
            // 
            // txtHeaderReconcileRemoveTransactionsTriggerSuffix
            // 
            this.txtHeaderReconcileRemoveTransactionsTriggerSuffix.Location = new System.Drawing.Point(226, 167);
            this.txtHeaderReconcileRemoveTransactionsTriggerSuffix.Name = "txtHeaderReconcileRemoveTransactionsTriggerSuffix";
            this.txtHeaderReconcileRemoveTransactionsTriggerSuffix.Size = new System.Drawing.Size(143, 20);
            this.txtHeaderReconcileRemoveTransactionsTriggerSuffix.TabIndex = 9;
            this.txtHeaderReconcileRemoveTransactionsTriggerSuffix.TextChanged += new System.EventHandler(this.txtHeaderReconcileRemoveTransactionsTriggerSuffix_TextChanged);
            // 
            // lblHRRemoveTransactionsTriggerSuffix
            // 
            this.lblHRRemoveTransactionsTriggerSuffix.AutoSize = true;
            this.lblHRRemoveTransactionsTriggerSuffix.Location = new System.Drawing.Point(36, 170);
            this.lblHRRemoveTransactionsTriggerSuffix.Name = "lblHRRemoveTransactionsTriggerSuffix";
            this.lblHRRemoveTransactionsTriggerSuffix.Size = new System.Drawing.Size(35, 13);
            this.lblHRRemoveTransactionsTriggerSuffix.TabIndex = 8;
            this.lblHRRemoveTransactionsTriggerSuffix.Text = "label4";
            // 
            // txtHeaderReconcileRemoveTransactionsFileName
            // 
            this.txtHeaderReconcileRemoveTransactionsFileName.Location = new System.Drawing.Point(226, 134);
            this.txtHeaderReconcileRemoveTransactionsFileName.Name = "txtHeaderReconcileRemoveTransactionsFileName";
            this.txtHeaderReconcileRemoveTransactionsFileName.Size = new System.Drawing.Size(191, 20);
            this.txtHeaderReconcileRemoveTransactionsFileName.TabIndex = 7;
            this.txtHeaderReconcileRemoveTransactionsFileName.TextChanged += new System.EventHandler(this.txtHeaderReconcileRemoveTransactionsFileName_TextChanged);
            // 
            // txtHeaderReconcileTriggerSuffix
            // 
            this.txtHeaderReconcileTriggerSuffix.Location = new System.Drawing.Point(158, 101);
            this.txtHeaderReconcileTriggerSuffix.Name = "txtHeaderReconcileTriggerSuffix";
            this.txtHeaderReconcileTriggerSuffix.Size = new System.Drawing.Size(143, 20);
            this.txtHeaderReconcileTriggerSuffix.TabIndex = 6;
            this.txtHeaderReconcileTriggerSuffix.TextChanged += new System.EventHandler(this.txtHeaderReconcileTriggerSuffix_TextChanged);
            // 
            // lblHRRemoveTransactionsFileName
            // 
            this.lblHRRemoveTransactionsFileName.AutoSize = true;
            this.lblHRRemoveTransactionsFileName.Location = new System.Drawing.Point(36, 137);
            this.lblHRRemoveTransactionsFileName.Name = "lblHRRemoveTransactionsFileName";
            this.lblHRRemoveTransactionsFileName.Size = new System.Drawing.Size(35, 13);
            this.lblHRRemoveTransactionsFileName.TabIndex = 5;
            this.lblHRRemoveTransactionsFileName.Text = "label4";
            // 
            // lblHRTriggerSuffix
            // 
            this.lblHRTriggerSuffix.AutoSize = true;
            this.lblHRTriggerSuffix.Location = new System.Drawing.Point(36, 104);
            this.lblHRTriggerSuffix.Name = "lblHRTriggerSuffix";
            this.lblHRTriggerSuffix.Size = new System.Drawing.Size(35, 13);
            this.lblHRTriggerSuffix.TabIndex = 4;
            this.lblHRTriggerSuffix.Text = "label4";
            // 
            // btnHeaderReconcileOutputDirectory
            // 
            this.btnHeaderReconcileOutputDirectory.Location = new System.Drawing.Point(447, 64);
            this.btnHeaderReconcileOutputDirectory.Name = "btnHeaderReconcileOutputDirectory";
            this.btnHeaderReconcileOutputDirectory.Size = new System.Drawing.Size(75, 23);
            this.btnHeaderReconcileOutputDirectory.TabIndex = 3;
            this.btnHeaderReconcileOutputDirectory.Text = "Directory";
            this.btnHeaderReconcileOutputDirectory.UseVisualStyleBackColor = true;
            this.btnHeaderReconcileOutputDirectory.Click += new System.EventHandler(this.btnHeaderReconcileOutputDirectory_Click);
            // 
            // btnHeaderReconcileInputDirectory
            // 
            this.btnHeaderReconcileInputDirectory.Location = new System.Drawing.Point(447, 33);
            this.btnHeaderReconcileInputDirectory.Name = "btnHeaderReconcileInputDirectory";
            this.btnHeaderReconcileInputDirectory.Size = new System.Drawing.Size(75, 23);
            this.btnHeaderReconcileInputDirectory.TabIndex = 2;
            this.btnHeaderReconcileInputDirectory.Text = "Directory";
            this.btnHeaderReconcileInputDirectory.UseVisualStyleBackColor = true;
            this.btnHeaderReconcileInputDirectory.Click += new System.EventHandler(this.btnHeaderReconcileInputDirectory_Click);
            // 
            // lblHROutputDirectory
            // 
            this.lblHROutputDirectory.AutoSize = true;
            this.lblHROutputDirectory.Location = new System.Drawing.Point(36, 71);
            this.lblHROutputDirectory.Name = "lblHROutputDirectory";
            this.lblHROutputDirectory.Size = new System.Drawing.Size(35, 13);
            this.lblHROutputDirectory.TabIndex = 1;
            this.lblHROutputDirectory.Text = "label4";
            // 
            // lblHRInputDirectory
            // 
            this.lblHRInputDirectory.AutoSize = true;
            this.lblHRInputDirectory.Location = new System.Drawing.Point(36, 38);
            this.lblHRInputDirectory.Name = "lblHRInputDirectory";
            this.lblHRInputDirectory.Size = new System.Drawing.Size(35, 13);
            this.lblHRInputDirectory.TabIndex = 0;
            this.lblHRInputDirectory.Text = "label4";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(742, 464);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 93;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(654, 464);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 24);
            this.btnOK.TabIndex = 92;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtTaskListName
            // 
            this.txtTaskListName.Location = new System.Drawing.Point(96, 8);
            this.txtTaskListName.Name = "txtTaskListName";
            this.txtTaskListName.Size = new System.Drawing.Size(184, 20);
            this.txtTaskListName.TabIndex = 94;
            this.txtTaskListName.TextChanged += new System.EventHandler(this.txtTaskListName_TextChanged);
            // 
            // lblFilterName
            // 
            this.lblFilterName.Location = new System.Drawing.Point(8, 8);
            this.lblFilterName.Name = "lblFilterName";
            this.lblFilterName.Size = new System.Drawing.Size(88, 23);
            this.lblFilterName.TabIndex = 35;
            this.lblFilterName.Text = "Task List Name:";
            this.lblFilterName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.Location = new System.Drawing.Point(566, 464);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
            this.btnSaveAs.TabIndex = 91;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // pnlTasks
            // 
            this.pnlTasks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTasks.Controls.Add(this.ulgTasks);
            this.pnlTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTasks.Location = new System.Drawing.Point(0, 0);
            this.pnlTasks.Name = "pnlTasks";
            this.pnlTasks.Size = new System.Drawing.Size(236, 426);
            this.pnlTasks.TabIndex = 14;
            // 
            // ulgTasks
            // 
            this.ulgTasks.AllowDrop = true;
            this.ulgTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ulgTasks.ContextMenu = this.ctmTaskMenu;
            this.ulgTasks.Cursor = System.Windows.Forms.Cursors.Default;
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ulgTasks.DisplayLayout.Appearance = appearance31;
            this.ulgTasks.DisplayLayout.InterBandSpacing = 10;
            appearance32.BackColor = System.Drawing.Color.Transparent;
            this.ulgTasks.DisplayLayout.Override.CardAreaAppearance = appearance32;
            appearance33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance33.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance33.ForeColor = System.Drawing.Color.Black;
            appearance33.TextHAlignAsString = "Left";
            appearance33.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ulgTasks.DisplayLayout.Override.HeaderAppearance = appearance33;
            appearance34.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgTasks.DisplayLayout.Override.RowAppearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance35.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ulgTasks.DisplayLayout.Override.RowSelectorAppearance = appearance35;
            this.ulgTasks.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ulgTasks.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance36.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance36.ForeColor = System.Drawing.Color.Black;
            this.ulgTasks.DisplayLayout.Override.SelectedRowAppearance = appearance36;
            this.ulgTasks.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ulgTasks.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ulgTasks.Location = new System.Drawing.Point(8, 8);
            this.ulgTasks.Name = "ulgTasks";
            this.ulgTasks.Size = new System.Drawing.Size(218, 410);
            this.ulgTasks.TabIndex = 0;
            this.ulgTasks.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ulgTasks_InitializeLayout);
            this.ulgTasks.AfterRowActivate += new System.EventHandler(this.ulgTasks_AfterRowActivate);
            this.ulgTasks.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ulgTasks_AfterRowInsert);
            this.ulgTasks.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ulgTasks_CellChange);
            this.ulgTasks.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.grid_SelectionDrag);
            this.ulgTasks.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ulgTasks_BeforeRowInsert);
            this.ulgTasks.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ulgTasks_BeforeRowsDeleted);
            this.ulgTasks.DragDrop += new System.Windows.Forms.DragEventHandler(this.grid_DragDrop);
            this.ulgTasks.DragOver += new System.Windows.Forms.DragEventHandler(this.grid_DragOver);
            // 
            // ctmTaskMenu
            // 
            this.ctmTaskMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniTaskDelete});
            this.ctmTaskMenu.Popup += new System.EventHandler(this.ctmTaskMenu_Popup);
            // 
            // mniTaskDelete
            // 
            this.mniTaskDelete.Index = 0;
            this.mniTaskDelete.Text = "Delete";
            this.mniTaskDelete.Click += new System.EventHandler(this.mniTaskDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(478, 464);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 24);
            this.btnSave.TabIndex = 90;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // rdoGlobal
            // 
            this.rdoGlobal.Location = new System.Drawing.Point(344, 10);
            this.rdoGlobal.Name = "rdoGlobal";
            this.rdoGlobal.Size = new System.Drawing.Size(56, 16);
            this.rdoGlobal.TabIndex = 96;
            this.rdoGlobal.Text = "Global";
            this.rdoGlobal.CheckedChanged += new System.EventHandler(this.rdoGlobal_CheckedChanged);
            // 
            // rdoUser
            // 
            this.rdoUser.Location = new System.Drawing.Point(296, 10);
            this.rdoUser.Name = "rdoUser";
            this.rdoUser.Size = new System.Drawing.Size(48, 16);
            this.rdoUser.TabIndex = 95;
            this.rdoUser.Text = "User";
            this.rdoUser.CheckedChanged += new System.EventHandler(this.rdoUser_CheckedChanged);
            // 
            // rdoSystem
            // 
            this.rdoSystem.Location = new System.Drawing.Point(400, 10);
            this.rdoSystem.Name = "rdoSystem";
            this.rdoSystem.Size = new System.Drawing.Size(64, 16);
            this.rdoSystem.TabIndex = 97;
            this.rdoSystem.Text = "System";
            this.rdoSystem.CheckedChanged += new System.EventHandler(this.rdoSystem_CheckedChanged);
            // 
            // fbdDirectory
            // 
            this.fbdDirectory.Description = "Select the directory where the posting input files will be found.";
            // 
            // btnRunNow
            // 
            this.btnRunNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRunNow.Location = new System.Drawing.Point(8, 464);
            this.btnRunNow.Name = "btnRunNow";
            this.btnRunNow.Size = new System.Drawing.Size(80, 24);
            this.btnRunNow.TabIndex = 44;
            this.btnRunNow.TabStop = false;
            this.btnRunNow.Text = "Run Now";
            this.btnRunNow.Click += new System.EventHandler(this.btnRunNow_Click);
            // 
            // ofdFile
            // 
            this.ofdFile.InitialDirectory = "Desktop";
            this.ofdFile.Title = "Select External Program";
            // 
            // rdoOwner
            // 
            this.rdoOwner.Location = new System.Drawing.Point(464, 10);
            this.rdoOwner.Name = "rdoOwner";
            this.rdoOwner.Size = new System.Drawing.Size(304, 16);
            this.rdoOwner.TabIndex = 98;
            this.rdoOwner.Text = "Owner ({1})";
            this.rdoOwner.CheckedChanged += new System.EventHandler(this.rdoOwner_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(8, 32);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pnlTasks);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ultraTabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(814, 426);
            this.splitContainer1.SplitterDistance = 236;
            this.splitContainer1.TabIndex = 99;
            // 
            // ultraTabControl1
            // 
            this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl3);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl4);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl5);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl6);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl7);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl8);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl9);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl10);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl11);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl12);
            this.ultraTabControl1.Controls.Add(this.ultraTabPageControl13); //TT#4574 - DOConnell - Purge Task List does not have an email option
            this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl1.Size = new System.Drawing.Size(574, 426);
            this.ultraTabControl1.TabIndex = 101;
            ultraTab10.Key = "tabSizeCurves";
            ultraTab10.TabPage = this.ultraTabPageControl10;
            ultraTab10.Text = "Size Curves";
            ultraTab10.Visible = false;
            ultraTab1.Key = "tabPosting";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Posting";
            ultraTab1.Visible = false;
            ultraTab3.Key = "tabSizeDayToWeekSummary";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Size Day To Week Summary";
            ultraTab3.Visible = false;
            ultraTab4.Key = "tabSizeCurveMethod";
            ultraTab4.TabPage = this.ultraTabPageControl4;
            ultraTab4.Text = "Size Curve Method";
            ultraTab4.Visible = false;
            ultraTab5.Key = "tabRelieveIntransit";
            ultraTab5.TabPage = this.ultraTabPageControl5;
            ultraTab5.Text = "Relieve Intransit";
            ultraTab5.Visible = false;
            ultraTab6.Key = "tabAllocate";
            ultraTab6.TabPage = this.ultraTabPageControl6;
            ultraTab6.Text = "Allocate";
            ultraTab6.Visible = false;
            ultraTab7.Key = "tabForecast";
            ultraTab7.TabPage = this.ultraTabPageControl7;
            ultraTab7.Text = "Forecast";
            ultraTab7.Visible = false;
            ultraTab8.Key = "tabRollup";
            ultraTab8.TabPage = this.ultraTabPageControl8;
            ultraTab8.Text = "Rollup";
            ultraTab8.Visible = false;
            ultraTab9.Key = "tabExternalProgram";
            ultraTab9.TabPage = this.ultraTabPageControl9;
            ultraTab9.Text = "External Program";
            ultraTab9.Visible = false;
            //BEGIN TT#4574 - DOConnell - Purge Task List does not have an email option
            //ultraTab2.Key = "tabEmail";
            //ultraTab2.TabPage = this.ultraTabPageControl2;
            //ultraTab2.Text = "Email";
            //END TT#4574 - DOConnell - Purge Task List does not have an email option
            ultraTab11.Key = "tabBatchComp";
            ultraTab11.TabPage = this.ultraTabPageControl11;
            ultraTab11.Text = "Batch Comp";
            //BEGIN TT#4574 - DOConnell - Purge Task List does not have an email option
            //ultraTab12.Key = "tabHeaderReconcile";
            //ultraTab12.TabPage = this.ultraTabPageControl12;
            //ultraTab12.Text = "Header Reconcile";
            ultraTab2.Key = "tabHeaderReconcile";
            ultraTab2.TabPage = this.ultraTabPageControl12;
            ultraTab2.Text = "Header Reconcile";
            ultraTab12.Key = "tabPurge";
            ultraTab12.TabPage = this.ultraTabPageControl13;
            ultraTab12.Text = "Purge";
            ultraTab13.Key = "tabChainForecasting";
            ultraTab13.TabPage = this.ultraTabPageControl13;
            ultraTab13.Text = "Chain Forecasting";
            ultraTab14.Key = "tabPushToBackStock";
            ultraTab14.TabPage = this.ultraTabPageControl13;
            ultraTab14.Text = "Push to Back-stock";
            //------------------------------------------------------------------------//
            //--  The 'tabEmail' should be the highest ultraTab number in the list  --//
            //------------------------------------------------------------------------//
            ultraTab15.Key = "tabEmail";
            ultraTab15.TabPage = this.ultraTabPageControl2;
            ultraTab15.Text = "Email";
            //END TT#4574 - DOConnell - Purge Task List does not have an email option
            this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab10,
            ultraTab1,
            ultraTab3,
            ultraTab4,
            ultraTab5,
            ultraTab6,
            ultraTab7,
            ultraTab8,
            ultraTab9,
            ultraTab2,
            ultraTab11,
			//BEGIN TT#4574 - DOConnell - Purge Task List does not have an email option
            ultraTab12,
            ultraTab13,
            ultraTab14,
            ultraTab15});
			//END TT#4574 - DOConnell - Purge Task List does not have an email option
            this.ultraTabControl1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraTabControl1.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraTabControl1_SelectedTabChanged);
            this.ultraTabControl1.Validating += new System.ComponentModel.CancelEventHandler(this.ultraTabControl1_Validating);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(570, 400);
            // 
            // frmTaskListProperties
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(830, 524);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.rdoOwner);
            this.Controls.Add(this.btnRunNow);
            this.Controls.Add(this.rdoSystem);
            this.Controls.Add(this.rdoUser);
            this.Controls.Add(this.rdoGlobal);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnSaveAs);
            this.Controls.Add(this.txtTaskListName);
            this.Controls.Add(this.lblFilterName);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "frmTaskListProperties";
            this.Text = "Task List Properties";
            this.Activated += new System.EventHandler(this.frmTaskListProperties_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmTaskListProperties_Closing);
            this.Load += new System.EventHandler(this.frmTaskListProperties_Load);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.lblFilterName, 0);
            this.Controls.SetChildIndex(this.txtTaskListName, 0);
            this.Controls.SetChildIndex(this.btnSaveAs, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.rdoGlobal, 0);
            this.Controls.SetChildIndex(this.rdoUser, 0);
            this.Controls.SetChildIndex(this.rdoSystem, 0);
            this.Controls.SetChildIndex(this.btnRunNow, 0);
            this.Controls.SetChildIndex(this.rdoOwner, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ultraTabPageControl10.ResumeLayout(false);
            this.pnlSizeCurves.ResumeLayout(false);
            this.pnlSizeCurves.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ulgSizeCurves)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSizeCurveConcurrentProcesses)).EndInit();
            this.ultraTabPageControl1.ResumeLayout(false);
            this.pnlPosting.ResumeLayout(false);
            this.pnlPosting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPostingConcurrent)).EndInit();
            this.ultraTabPageControl3.ResumeLayout(false);
            this.pnlSizeDayToWeekSummary.ResumeLayout(false);
            this.pnlSizeDayToWeekSummary.PerformLayout();
            this.ultraTabPageControl4.ResumeLayout(false);
            this.pnlSizeCurveMethod.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ulgSizeCurveMethod)).EndInit();
            this.ultraTabPageControl5.ResumeLayout(false);
            this.pnlRelieve.ResumeLayout(false);
            this.pnlRelieve.PerformLayout();
            this.ultraTabPageControl6.ResumeLayout(false);
            this.pnlAllocate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ulgAllocate)).EndInit();
            this.ultraTabPageControl7.ResumeLayout(false);
            this.pnlForecast.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ulgForecast)).EndInit();
            this.ultraTabPageControl8.ResumeLayout(false);
            this.pnlRollup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ulgRollup)).EndInit();
            this.ultraTabPageControl9.ResumeLayout(false);
            this.pnlProgram.ResumeLayout(false);
            this.pnlProgram.PerformLayout();
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl11.ResumeLayout(false);
            this.pnlBatchComp.ResumeLayout(false);
            this.pnlBatchComp.PerformLayout();
            this.ultraTabPageControl12.ResumeLayout(false);
            this.pnlHeaderReconcile.ResumeLayout(false);
            this.pnlHeaderReconcile.PerformLayout();
            this.pnlTasks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ulgTasks)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this.ultraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public delegate void TaskListPropertiesSaveEventHandler(object source, TaskListPropertiesSaveEventArgs e);
		public event TaskListPropertiesSaveEventHandler OnTaskListPropertiesSaveHandler;

		public delegate void TaskListPropertiesCloseEventHandler(object source, TaskListPropertiesCloseEventArgs e);
		public event TaskListPropertiesCloseEventHandler OnTaskListPropertiesCloseHandler;

		private SessionAddressBlock _SAB;
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private MIDTaskListNode _currParentNode;
		private MIDTaskListNode _userNode;
		private MIDTaskListNode _globalNode;
		private MIDTaskListNode _systemNode;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private int _initialUserRID;
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private int _initialOwnerRID;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private bool _readOnly;
		private ArrayList _heldJobs;
		private bool _canUpdateTaskList;
		private FunctionSecurityProfile _userSecLvl;
		private FunctionSecurityProfile _globalSecLvl;
		private FunctionSecurityProfile _systemSecLvl;
		private TaskListProfile _taskListProf;
		private ScheduleData _dlSchedule;
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private FolderDataLayer _dlFolder;
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private WorkflowMethodData _dlWorkflowMethod;
		private WorkflowBaseData _dlWorkflowBase;
		private ProfileList _versionProfList;
		private DataTable _dtTask;
		private DataTable _dtTaskForecast;
		private DataTable _dtTaskForecastDetail;
		private DataTable _dtTaskAllocate;
		private DataTable _dtTaskAllocateDetail;
		private DataTable _dtTaskRollup;
		//Begin TT#155 - JScott - Size Curve Method
		private DataTable _dtTaskSizeCurveMethod;
		//End TT#155 - JScott - Size Curve Method
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		private DataTable _dtTaskSizeCurves;
		//End TT#155 - JScott - Add Size Curve info to Node Properties
		//Begin TT#707 - JScott - Size Curve process needs to multi-thread
		private DataTable _dtTaskSizeCurvesNode;
		//End TT#707 - JScott - Size Curve process needs to multi-thread
		//Begin TT#391 - stodd - size day to week summary
		private DataTable _dtTaskSizeDayToWeekSummary;
		//End TT#391 - stodd - size day to week summary
		private DataTable _dtTaskPosting;
        private DataTable _dtTaskBatchComp;		// TT#1595-MD - stodd - Batch Comp
        private DataTable _dtTaskHeaderReconcile;	
		private DataTable _dtTaskProgram;
        private DataTable _dtTaskEmail; //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
		private eTaskType _addedType;
		private int _nextTaskSequence;
		private int _nextForecastSequence;
		private int _nextAllocateSequence;
		private int _nextRollupSequence;
		//Begin TT#155 - JScott - Size Curve Method
		private int _nextSizeCurveMethodSequence;
		//End TT#155 - JScott - Size Curve Method
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		private int _nextSizeCurvesSequence;
		//End TT#155 - JScott - Add Size Curve info to Node Properties
		private int _nextForecastDetailSequence;
		private int _nextAllocateDetailSequence;
		private DataRow _currentPostingRow;
        private DataRow _currentBatchCompRow;	// TT#1595-MD - stodd - Batch Comp
        private DataRow _currentHeaderReconcileRow;
		private DataRow _currentProgramRow;
		//Begin TT#707 - JScott - Size Curve process needs to multi-thread
		private DataRow _currentSizeCurveNodeRow;
		//End TT#707 - JScott - Size Curve process needs to multi-thread
		//Begin TT#391 - stodd - size day to week summary
		private DataRow _currentSizeDayToWeekSummaryRow;
		private int _overrideNodeRid = Include.NoRID;
		//End TT#391 - stodd - size day to week summary
		private ArrayList _addMenuList;
		private bool _afterAddMenu;
		private Hashtable _forecastDataSetList;
		private Hashtable _allocateDataSetList;
		private Hashtable _rollupDataTableList;
		//Begin TT#155 - JScott - Size Curve Method
		private Hashtable _sizeCurveMethodDataTableList;
		//End TT#155 - JScott - Size Curve Method
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		private Hashtable _sizeCurvesDataTableList;
		//End TT#155 - JScott - Add Size Curve info to Node Properties
		private bool _bypassBeforeDelete;
		private bool _bypassAfterUpdate;
		private ProfileList _variables;
		private ArrayList _fromLevelList;
		private ArrayList _toLevelList;
        //BEGIN TT#3995-Task List Explorer-Opening a Size Day to Week Summary task
        private bool _ignoreTextChanged;
        //END TT#3995-Task List Explorer-Opening a Size Day to Week Summary task


		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public frmTaskListProperties(SessionAddressBlock aSAB, int aInitialUserRID, bool aReadOnly)
		public frmTaskListProperties(
			SessionAddressBlock aSAB,
			MIDTaskListNode aCurrParentNode,
			MIDTaskListNode aUserNode,
			MIDTaskListNode aGlobalNode,
			MIDTaskListNode aSystemNode,
			int aInitialUserRID,
			int aInitialOwnerRID,
			bool aReadOnly)
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			: base(aSAB)
		{
			try
			{
				InitializeComponent();

				_SAB = aSAB;
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_currParentNode = aCurrParentNode;
				_userNode = aUserNode;
				_globalNode = aGlobalNode;
				_systemNode = aSystemNode;
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_taskListProf = new TaskListProfile(Include.NoRID);
				_initialUserRID = aInitialUserRID;
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_initialOwnerRID = aInitialOwnerRID;
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_readOnly = aReadOnly;
				_heldJobs = null;
				_fromLevelList = new ArrayList();
				_toLevelList = new ArrayList();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		//public frmTaskListProperties(SessionAddressBlock aSAB, TaskListProfile aTaskListProfile, ArrayList aHeldJobs, bool aReadOnly)
		public frmTaskListProperties(
			SessionAddressBlock aSAB,
			MIDTaskListNode aCurrParentNode,
			MIDTaskListNode aUserNode,
			MIDTaskListNode aGlobalNode,
			MIDTaskListNode aSystemNode,
			TaskListProfile aTaskListProfile,
			ArrayList aHeldJobs,
			bool aReadOnly)
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			: base(aSAB)
		{
			try
			{
				InitializeComponent();

				_SAB = aSAB;
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_currParentNode = aCurrParentNode;
				_userNode = aUserNode;
				_globalNode = aGlobalNode;
				_systemNode = aSystemNode;
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_taskListProf = aTaskListProfile;
				_initialUserRID = aTaskListProfile.UserRID;
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_initialOwnerRID = aTaskListProfile.OwnerUserRID;
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_heldJobs = aHeldJobs;
				_readOnly = aReadOnly;
				_fromLevelList = new ArrayList();
				_toLevelList = new ArrayList();
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

		private void frmTaskListProperties_Load(object sender, System.EventArgs e)
		{
			DataTable dtMessageLevels;
			//DataTable dtHeaderTypes; //TT#1313-MD -jsobek -Header Filters
			ArrayList mniAddTasks;
			int i;
            Infragistics.Win.ValueList valList1;
			Infragistics.Win.ValueList valList2;
			Infragistics.Win.ValueListItem valListItem;
			DataSetEntry dataSetEntry;
			DataTableEntry dataTableEntry;

			try
			{
				FormLoaded = false;
                RollUpLoaded = false; //TT#2744 - MD - Rollup task in 5.0 asking to save when nothing has chenged - RBeck
				// Begin TT#391 - Stodd - size day to week summary
				SetText();
				// End TT#391 - Stodd - size day to week summary

				FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsScheduler);
				_userSecLvl = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerUserTaskLists);
				_globalSecLvl = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerGlobalTaskLists);
				_systemSecLvl = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerSystemTaskLists);
				_variables = _SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;

                // Begin TT#1314-MD - JSmith - Tasklist allows multiple concurrent files with FIFO/FILO processing
                string fileProcessingDirection = MIDConfigurationManager.AppSettings["APIFileProcessingDirection"];
                if (fileProcessingDirection == null)
                {
                    _FileProcessingDirection = eAPIFileProcessingDirection.Default;
                }
                else
                {
                    switch (fileProcessingDirection.ToUpper().Trim())
                    {
                        case "FIFO":
                            _FileProcessingDirection = eAPIFileProcessingDirection.FIFO;
                            break;
                        case "FILO":
                            _FileProcessingDirection = eAPIFileProcessingDirection.FILO;
                            break;
                        default:
                            _FileProcessingDirection = eAPIFileProcessingDirection.Default;
                            break;
                    }
                }
                // End TT#1314-MD - JSmith - Tasklist allows multiple concurrent files with FIFO/FILO processing
				
				_dlSchedule = new ScheduleData();
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_dlFolder = new FolderDataLayer();
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				_dlWorkflowMethod = new WorkflowMethodData();
				_dlWorkflowBase = new WorkflowBaseData();
				_forecastDataSetList = new Hashtable();
				_allocateDataSetList = new Hashtable();
				_rollupDataTableList = new Hashtable();
				//Begin TT#155 - JScott - Size Curve Method
				_sizeCurveMethodDataTableList = new Hashtable();
				//End TT#155 - JScott - Size Curve Method
				//Begin TT#155 - JScott - Add Size Curve info to Node Properties
				_sizeCurvesDataTableList = new Hashtable();
				//End TT#155 - JScott - Add Size Curve info to Node Properties

                // Begin TT#4001 - JSmith - Task List Explorer - Read only Header Load system task allows users to edit
                //if (!_readOnly && (_userSecLvl.AllowUpdate || _globalSecLvl.AllowUpdate || _systemSecLvl.AllowUpdate))
                //{
                //    FunctionSecurity.SetAllowUpdate();
                //}
                //else
                //{
                //    FunctionSecurity.SetReadOnly();
                //}
                if (_readOnly)
                {
                    FunctionSecurity.SetReadOnly();
                }
                else
                {
                    switch (_taskListProf.OwnerUserRID)
                    {
                        case Include.GlobalUserRID:
                            if (_globalSecLvl.AllowUpdate)
                            {
                                FunctionSecurity.SetAllowUpdate();
                            }
                            else
                            {
                                FunctionSecurity.SetReadOnly();
                            }
                            break;
                        case Include.SystemUserRID:
                            if (_systemSecLvl.AllowUpdate)
                            {
                                FunctionSecurity.SetAllowUpdate();
                            }
                            else
                            {
                                FunctionSecurity.SetReadOnly();
                            }
                            break;
                        default:
                            if (_userSecLvl.AllowUpdate)
                            {
                                FunctionSecurity.SetAllowUpdate();
                            }
                            else
                            {
                                FunctionSecurity.SetReadOnly();
                            }
                            break;
                    }
                }
                // End TT#4001 - JSmith - Task List Explorer - Read only Header Load system task allows users to edit

				// Create AddMenu popup

                // Begin TT#4106 - JSmith - Task List Explorer - Allows users to copy/paste read only system task lists.
                //_addMenuList = new ArrayList();

                //_addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.Allocate), eTaskType.Allocate));
                //_addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.Forecasting), eTaskType.Forecasting));
                //_addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.computationDriver), eTaskType.computationDriver));
                ////Begin TT#155 - JScott - Size Curve Method

                //if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                //{
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.SizeCurveMethod), eTaskType.SizeCurveMethod));
                //    //Begin TT#155 - JScott - Add Size Curve info to Node Properties
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.SizeCurves), eTaskType.SizeCurves));
                //    //End TT#155 - JScott - Add Size Curve info to Node Properties
                //}

                ////End TT#155 - JScott - Size Curve Method
                //_addMenuList.Add(new AddMenuEntry("-", eTaskType.None));
                //_addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.Rollup), eTaskType.Rollup));

                //if (_systemSecLvl.AllowUpdate)
                //{
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.Purge), eTaskType.Purge));
                //    _addMenuList.Add(new AddMenuEntry("-", eTaskType.None));
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.StoreLoad), eTaskType.StoreLoad));
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.HierarchyLoad), eTaskType.HierarchyLoad));
                //    // BEGIN TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.ChainSetPercentCriteriaLoad), eTaskType.ChainSetPercentCriteriaLoad));
                //    // END TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
                //    // BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.DailyPercentagesCriteriaLoad), eTaskType.DailyPercentagesCriteriaLoad)); //TT#816 - MD - DOConnell - Corrected misspelling
                //    // END TT#43 - MD - DOConnell - Projected Sales Enhancement
                //    // BEGIN TT#1401 - AGallagher - VSW
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.PushToBackStockLoad), eTaskType.PushToBackStockLoad));
                //    // END TT#1401 - AGallagher - VSW
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.HistoryPlanLoad), eTaskType.HistoryPlanLoad));
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.HeaderLoad), eTaskType.HeaderLoad));
                //    //Begin MOD - JScott - Build Pack Criteria Load
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.BuildPackCriteriaLoad), eTaskType.BuildPackCriteriaLoad));
                //    //End MOD - JScott - Build Pack Criteria Load
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.RelieveIntransit), eTaskType.RelieveIntransit));
                //    //BEGIN TT#820 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Add Task List Options for VSW and Store Eligibility Load processes
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.StoreEligibilityCriteriaLoad), eTaskType.StoreEligibilityCriteriaLoad));
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.VSWCriteriaLoad), eTaskType.VSWCriteriaLoad));
                //    //END TT#820 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Add Task List Options for VSW and Store Eligibility Load processes
                //    if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                //    {
                //        _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.SizeCodeLoad), eTaskType.SizeCodeLoad));
                //        _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.SizeCurveLoad), eTaskType.SizeCurveLoad));
                //        _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.SizeConstraintsLoad), eTaskType.SizeConstraintsLoad));
                //        _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.ColorCodeLoad), eTaskType.ColorCodeLoad));
                //    }
                //    // Begin TT#391 - stodd  - moved these below...
                //    //_addMenuList.Add(new AddMenuEntry("-", eTaskType.None));
                //    //_addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.ExternalProgram), eTaskType.ExternalProgram));
                //}

                //// Begin TT#391 - stodd - size day to week summary
                //if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                //{
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.SizeDayToWeekSummary), eTaskType.SizeDayToWeekSummary));
                //}

                //if (_systemSecLvl.AllowUpdate)
                //{
                //    _addMenuList.Add(new AddMenuEntry("-", eTaskType.None));
                //    _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)eTaskType.ExternalProgram), eTaskType.ExternalProgram));
                //}
				// End TT#391 - stodd - size day to week summary

                _addMenuList = new ArrayList();
                foreach (eTaskType taskType in  Include.GetAvailableTasks(_userSecLvl, _globalSecLvl, _systemSecLvl, _SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled))
                {
                    if (taskType == eTaskType.None)
                    {
                        _addMenuList.Add(new AddMenuEntry("-", eTaskType.None));
                    }
                    else
                    {
                        _addMenuList.Add(new AddMenuEntry(MIDText.GetTextOnly((int)taskType), taskType));
                    }
                }
                // End TT#4106 - JSmith - Task List Explorer - Allows users to copy/paste read only system task lists.

				//BEGIN TT#4000 - DOConnell - Task List Explorer - Read only system Hierarchy Load task allows users to edit
                Hashtable taskListHash = new Hashtable();
                foreach (AddMenuEntry ame in _addMenuList)
                {
                    if (!taskListHash.Contains((int)ame.TaskType.GetHashCode()))
                    {
                        taskListHash.Add(ame.TaskType.GetHashCode(), null);
                    }
                }
				//END TT#4000 - DOConnell - Task List Explorer - Read only system Hierarchy Load task allows users to edit
				
				mniAddTasks = new ArrayList();

				foreach (AddMenuEntry addMenuEntry in _addMenuList)
				{
					i = mniAddTasks.Add(new MenuItem());
					((MenuItem)mniAddTasks[i]).Index = i;
					((MenuItem)mniAddTasks[i]).Text = addMenuEntry.DisplayName;
					((MenuItem)mniAddTasks[i]).Click += new System.EventHandler(this.mniAddTask_Click);
				}

				ctmAddTask.MenuItems.AddRange((MenuItem[])mniAddTasks.ToArray(typeof(MenuItem)));

				txtTaskListName.Text = _taskListProf.Name;

				// Setup Message Level ValueList

				dtMessageLevels = MIDText.GetTextType(eMIDTextType.eMIDMessageLevel, eMIDTextOrderBy.TextCode);

				valList1 = ulgTasks.DisplayLayout.ValueLists.Add("MaxMessageLevelDesc");
			
				foreach (DataRow row in dtMessageLevels.Rows)
				{
					valListItem = new Infragistics.Win.ValueListItem();
					valListItem.DataValue= Convert.ToInt32(row["TEXT_CODE"]);
					valListItem.DisplayText = Convert.ToString(row["TEXT_VALUE"]);
					valList1.ValueListItems.Add(valListItem);
				}

				// Setup Version ValueList

				_versionProfList = _SAB.ClientServerSession.GetUserForecastVersions();

				valList1 = ulgForecast.DisplayLayout.ValueLists.Add("Version");
				valList2 = ulgRollup.DisplayLayout.ValueLists.Add("Version");

				valListItem = new Infragistics.Win.ValueListItem();
				valListItem.DataValue= Include.NoRID;
				valListItem.DisplayText = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToMethodValue);
				valList1.ValueListItems.Add(valListItem);

				for (i = 0; i < _versionProfList.Count; i++)
				{
					valListItem = new Infragistics.Win.ValueListItem();
					valListItem.DataValue= _versionProfList[i].Key;
					valListItem.DisplayText = ((VersionProfile)_versionProfList[i]).Description;

					if (((VersionProfile)_versionProfList[i]).StoreSecurity.AllowUpdate && ((VersionProfile)_versionProfList[i]).ChainSecurity.AllowView)
					{
						valList1.ValueListItems.Add(valListItem);
					}

					if (((VersionProfile)_versionProfList[i]).StoreSecurity.AllowUpdate || ((VersionProfile)_versionProfList[i]).ChainSecurity.AllowUpdate)
					{
						valListItem = new Infragistics.Win.ValueListItem();
						valListItem.DataValue = _versionProfList[i].Key;
						valListItem.DisplayText = ((VersionProfile)_versionProfList[i]).Description;
						valList2.ValueListItems.Add(valListItem);
					}
				}

                //Begin TT#1313-MD -jsobek -Header Filters

                //// Setup HeaderTypes ValueList

                //// Begin TT#1043 – JSmith - Assortment and Placeholder show as header types when should not.
                ////dtHeaderTypes = MIDText.GetTextType(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextCode);
                //dtHeaderTypes = MIDText.GetTextType(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextCode, Convert.ToInt32(eHeaderType.Assortment), Convert.ToInt32(eHeaderType.Placeholder));
                //// End TT#1043

                //valList1 = ulgAllocate.DisplayLayout.ValueLists.Add("HeaderTypes");

                //valListItem = new Infragistics.Win.ValueListItem();
                //valListItem.DataValue= Include.NoRID;
                //valListItem.DisplayText = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
                //valList1.ValueListItems.Add(valListItem);

                //foreach (DataRow row in dtHeaderTypes.Rows)
                //{
                //    valListItem = new Infragistics.Win.ValueListItem();
                //    valListItem.DataValue= Convert.ToInt32(row["TEXT_CODE"]);
                //    valListItem.DisplayText = Convert.ToString(row["TEXT_VALUE"]);
                //    valList1.ValueListItems.Add(valListItem);
                //}

                // Setup the header filters ValueList               
           
       
                //Begin TT#1446-MD -jsobek -Header Filter - Opening a Task List with a user Header Filter the filter name appears as a number
        
                _valListHeaderFilters = ulgAllocate.DisplayLayout.ValueLists.Add("HeaderFilters");
                FilterData storeFilterData = new FilterData();
                DataTable dtHeaderFilters;
                if (this._readOnly) //readonly
                {
                    //load all the header filters so anyone can view anything
                    dtHeaderFilters = storeFilterData.ReadFiltersForType(filterTypes.HeaderFilter);
                }
                else if (IsTaskGlobalOrSystem()) //global
                {
                    ArrayList userRIDList = new ArrayList();
                    userRIDList.Add(Include.GlobalUserRID);
                    dtHeaderFilters = storeFilterData.FilterRead(filterTypes.HeaderFilter, eProfileType.FilterHeader, userRIDList);
                }
                else //user
                {
                    ArrayList userRIDList = new ArrayList();
                    userRIDList.Add(Include.GlobalUserRID);
                    userRIDList.Add(SAB.ClientServerSession.UserRID);
                    dtHeaderFilters = storeFilterData.FilterRead(filterTypes.HeaderFilter, eProfileType.FilterHeader, userRIDList);
                }


                _valListHeaderFilters.ValueListItems.Clear();

                valListItem = new Infragistics.Win.ValueListItem();
                valListItem.DataValue = Include.NoRID;
                valListItem.DisplayText = "No Filter"; //MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue); //TT#1313-MD -jsobek -Header Filters
                _valListHeaderFilters.ValueListItems.Add(valListItem);

                foreach (DataRow row in dtHeaderFilters.Rows)
                {
                    valListItem = new Infragistics.Win.ValueListItem();
                    valListItem.DataValue = Convert.ToInt32(row["FILTER_RID"]);
                    valListItem.DisplayText = Convert.ToString(row["FILTER_NAME"]);
                    _valListHeaderFilters.ValueListItems.Add(valListItem);
                }
                //End TT#1446-MD -jsobek -Header Filter - Opening a Task List with a user Header Filter the filter name appears as a number

          
    

                //End TT#1313-MD -jsobek -Header Filters

				// Setup Task datatable and grid

				_dtTask = _dlSchedule.Task_ReadByTaskList(_taskListProf.Key);
            
				_dtTask.Columns.Add("Task", typeof(string));
				_dtTask.Columns.Add("DisplaySequence", typeof(int));

				for (i = 0; i < _dtTask.Rows.Count; i++)
				{
					_dtTask.Rows[i]["Task"] = MIDText.GetTextOnly(Convert.ToInt32(_dtTask.Rows[i]["TASK_TYPE"]));
					_dtTask.Rows[i]["DisplaySequence"] = i;
					//BEGIN TT#4000 - DOConnell - Task List Explorer - Read only system Hierarchy Load task allows users to edit
                    if (!taskListHash.Contains((int)_dtTask.Rows[i]["TASK_TYPE"]))
                    {
                        _readOnly = true;
                        FunctionSecurity.SetReadOnly();
                    }
					//END TT#4000 - DOConnell - Task List Explorer - Read only system Hierarchy Load task allows users to edit
				}

				// Setup Forecast datatable and grid

				_dtTaskForecast = _dlSchedule.TaskForecast_ReadByTaskList(_taskListProf.Key);
				_dtTaskForecastDetail = _dlSchedule.TaskForecastDetail_ReadByTaskList(_taskListProf.Key);

				//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				//_dtTaskForecast.Columns.Add("Merchandise", typeof(string));
				_dtTaskForecast.Columns.Add("ForecastMerchandise", typeof(string));
				//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				_dtTaskForecast.Columns.Add("Version", typeof(int));
				_dtTaskForecast.Columns.Add("DisplaySequence", typeof(int));

				for (i = 0; i < _dtTaskForecast.Rows.Count; i++)
				{
					if (_dtTaskForecast.Rows[i]["HN_RID"] != System.DBNull.Value)
					{
						//Begin TT#352 - JScott - TASK LIST ->Drag and drop style 369621/005 into task list. Only display color. Need style/color to display!!!
						////Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
						////_dtTaskForecast.Rows[i]["Merchandise"] = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtTaskForecast.Rows[i]["HN_RID"])).Text;
						//_dtTaskForecast.Rows[i]["ForecastMerchandise"] = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtTaskForecast.Rows[i]["HN_RID"])).Text;
						////End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
						_dtTaskForecast.Rows[i]["ForecastMerchandise"] = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtTaskForecast.Rows[i]["HN_RID"]), false, true).Text;
						//End TT#352 - JScott - TASK LIST ->Drag and drop style 369621/005 into task list. Only display color. Need style/color to display!!!
					}
					else
					{
						//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
						//_dtTaskForecast.Rows[i]["Merchandise"] = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToMethodValue);
						_dtTaskForecast.Rows[i]["ForecastMerchandise"] = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToMethodValue);
						//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
					}

					if (_dtTaskForecast.Rows[i]["FV_RID"] != System.DBNull.Value)
					{
						_dtTaskForecast.Rows[i]["Version"] = Convert.ToInt32(_dtTaskForecast.Rows[i]["FV_RID"]);
					}
					else
					{
						_dtTaskForecast.Rows[i]["Version"] = Include.NoRID;
					}

					_dtTaskForecast.Rows[i]["DisplaySequence"] = i;
				}

				_dtTaskForecastDetail.Columns.Add("ForecastWorkflowMethod", typeof(string));
				_dtTaskForecastDetail.Columns.Add("WorkflowMethodType", typeof(string));
				_dtTaskForecastDetail.Columns.Add("Execute Date Range", typeof(string));
				_dtTaskForecastDetail.Columns.Add("DisplaySequence", typeof(int));

				for (i = 0; i < _dtTaskForecastDetail.Rows.Count; i++)
				{
					if (Convert.ToInt32(_dtTaskForecastDetail.Rows[i]["WORKFLOW_METHOD_IND"]) == (int)eWorkflowMethodType.Method)
					{
						_dtTaskForecastDetail.Rows[i]["ForecastWorkflowMethod"] = _dlWorkflowMethod.GetMethodName(Convert.ToInt32(_dtTaskForecastDetail.Rows[i]["METHOD_RID"]));
						_dtTaskForecastDetail.Rows[i]["WorkflowMethodType"] = MIDText.GetTextOnly(Convert.ToInt32(_dlWorkflowMethod.GetMethodType(Convert.ToInt32(_dtTaskForecastDetail.Rows[i]["METHOD_RID"]))));
					}
					else
					{
                         
//Begin  TT#282 - MD - User name not designated in Task List - RBeck
                        //_dtTaskForecastDetail.Rows[i]["ForecastWorkflowMethod"] = _dlWorkflowBase.GetWorkflowName(Convert.ToInt32(_dtTaskForecastDetail.Rows[i]["WORKFLOW_RID"]));
                        int _userID = _dlWorkflowBase.GetWorkflowUserId(Convert.ToInt32(_dtTaskForecastDetail.Rows[i]["WORKFLOW_RID"]));
                        string _wfName = _dlWorkflowBase.GetWorkflowName(Convert.ToInt32(_dtTaskForecastDetail.Rows[i]["WORKFLOW_RID"]));
                        _dtTaskForecastDetail.Rows[i]["ForecastWorkflowMethod"] = AddUserName(_userID, _wfName);
//End    TT#282 - MD - User name not designated in Task List - RBeck

						_dtTaskForecastDetail.Rows[i]["WorkflowMethodType"] = MIDText.GetTextOnly(Convert.ToInt32(eWorkflowMethodType.Workflow));
					}

					if (_dtTaskForecastDetail.Rows[i]["EXECUTE_CDR_RID"] != System.DBNull.Value)
					{
						if (Convert.ToInt32(_dtTaskForecastDetail.Rows[i]["EXECUTE_CDR_RID"]) == Include.UndefinedCalendarDateRange)
						{
							_dtTaskForecastDetail.Rows[i]["EXECUTE_CDR_RID"] = System.DBNull.Value;
						}
						else
						{
							_dtTaskForecastDetail.Rows[i]["Execute Date Range"] = _SAB.ClientServerSession.Calendar.GetDisplayDate(_SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(_dtTaskForecastDetail.Rows[i]["EXECUTE_CDR_RID"]), _SAB.ClientServerSession.Calendar.CurrentDate));
						}
					}

					_dtTaskForecastDetail.Rows[i]["DisplaySequence"] = i;
				}

				foreach (DataRow row in _dtTaskForecast.Rows)
				{
					dataSetEntry = GetForecastDataSetEntry(Convert.ToInt32(row["TASK_SEQUENCE"]));
					dataSetEntry.AddMainRow(row);
				}

				foreach (DataRow row in _dtTaskForecastDetail.Rows)
				{
					dataSetEntry = GetForecastDataSetEntry(Convert.ToInt32(row["TASK_SEQUENCE"]));
					dataSetEntry.AddDetailRow(row);
				}

				// Setup Allocate datatable and grid

				_dtTaskAllocate = _dlSchedule.TaskAllocate_ReadByTaskList(_taskListProf.Key);
				_dtTaskAllocateDetail = _dlSchedule.TaskAllocateDetail_ReadByTaskList(_taskListProf.Key);

				//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				//_dtTaskAllocate.Columns.Add("Merchandise", typeof(string));
				_dtTaskAllocate.Columns.Add("AllocateMerchandise", typeof(string));
				//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
                _dtTaskAllocate.Columns.Add("Filter", typeof(int)); //TT#1313-MD -jsobek -Header Filters
                //_dtTaskAllocate.Columns.Add("PO", typeof(string)); //TT#1313-MD -jsobek -Header Filters
                //_dtTaskAllocate.Columns.Add("Header", typeof(string)); //TT#1313-MD -jsobek -Header Filters
				_dtTaskAllocate.Columns.Add("DisplaySequence", typeof(int));

				for (i = 0; i < _dtTaskAllocate.Rows.Count; i++)
				{
					if (_dtTaskAllocate.Rows[i]["HN_RID"] != System.DBNull.Value)
					{
						//Begin TT#352 - JScott - TASK LIST ->Drag and drop style 369621/005 into task list. Only display color. Need style/color to display!!!
						////Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
						////_dtTaskAllocate.Rows[i]["Merchandise"] = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtTaskAllocate.Rows[i]["HN_RID"])).Text;
						//_dtTaskAllocate.Rows[i]["AllocateMerchandise"] = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtTaskAllocate.Rows[i]["HN_RID"])).Text;
						////End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
						_dtTaskAllocate.Rows[i]["AllocateMerchandise"] = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtTaskAllocate.Rows[i]["HN_RID"]), false, true).Text;
						//End TT#352 - JScott - TASK LIST ->Drag and drop style 369621/005 into task list. Only display color. Need style/color to display!!!
					}
					else
					{
						//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
						//_dtTaskAllocate.Rows[i]["Merchandise"] = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
						_dtTaskAllocate.Rows[i]["AllocateMerchandise"] = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
						//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
					}
                    //Begin TT#1313-MD -jsobek -Header Filters
                    //if (_dtTaskAllocate.Rows[i]["ALLOCATE_TYPE"] != System.DBNull.Value)
                    //{
                    //    _dtTaskAllocate.Rows[i]["Type"] = Convert.ToInt32(_dtTaskAllocate.Rows[i]["ALLOCATE_TYPE"]);
                    //}
                    //else
                    //{
                    //    _dtTaskAllocate.Rows[i]["Type"] = Include.NoRID;
                    //}

                    //if (_dtTaskAllocate.Rows[i]["PO_ID"] != System.DBNull.Value)
                    //{
                    //    _dtTaskAllocate.Rows[i]["PO"] = Convert.ToString(_dtTaskAllocate.Rows[i]["PO_ID"]);
                    //}
                    //else
                    //{
                    //    _dtTaskAllocate.Rows[i]["PO"] = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
                    //}

                    //if (_dtTaskAllocate.Rows[i]["HEADER_ID"] != System.DBNull.Value)
                    //{
                    //    _dtTaskAllocate.Rows[i]["Header"] = Convert.ToString(_dtTaskAllocate.Rows[i]["HEADER_ID"]);
                    //}
                    //else
                    //{
                    //    _dtTaskAllocate.Rows[i]["Header"] = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
                    //}

                    if (_dtTaskAllocate.Rows[i]["FILTER_RID"] != System.DBNull.Value)
                    {
                        _dtTaskAllocate.Rows[i]["Filter"] = Convert.ToInt32(_dtTaskAllocate.Rows[i]["FILTER_RID"]);
                    }
                    else
                    {
                        _dtTaskAllocate.Rows[i]["Filter"] = Include.NoRID;
                    }
                    //End TT#1313-MD -jsobek -Header Filters
					_dtTaskAllocate.Rows[i]["DisplaySequence"] = i;
				}

				_dtTaskAllocateDetail.Columns.Add("AllocateWorkflowMethod", typeof(string));
				_dtTaskAllocateDetail.Columns.Add("WorkflowMethodType", typeof(string));
				_dtTaskAllocateDetail.Columns.Add("Execute Date Range", typeof(string));
				_dtTaskAllocateDetail.Columns.Add("DisplaySequence", typeof(int));

				for (i = 0; i < _dtTaskAllocateDetail.Rows.Count; i++)
				{
					if (Convert.ToInt32(_dtTaskAllocateDetail.Rows[i]["WORKFLOW_METHOD_IND"]) == (int)eWorkflowMethodType.Method)
					{
						_dtTaskAllocateDetail.Rows[i]["AllocateWorkflowMethod"] = _dlWorkflowMethod.GetMethodName(Convert.ToInt32(_dtTaskForecastDetail.Rows[i]["METHOD_RID"]));
						_dtTaskAllocateDetail.Rows[i]["WorkflowMethodType"] = MIDText.GetTextOnly(Convert.ToInt32(_dlWorkflowMethod.GetMethodType(Convert.ToInt32(_dtTaskForecastDetail.Rows[i]["METHOD_RID"]))));
					}
					else
					{

//Begin  TT#283 - MD - User name not designated in Task List - RBeck
                        int _userID = _dlWorkflowBase.GetWorkflowUserId(Convert.ToInt32(_dtTaskAllocateDetail.Rows[i]["WORKFLOW_RID"]));
                        string _wfName = _dlWorkflowBase.GetWorkflowName(Convert.ToInt32(_dtTaskAllocateDetail.Rows[i]["WORKFLOW_RID"])); 
                        _dtTaskAllocateDetail.Rows[i]["AllocateWorkflowMethod"] = AddUserName(_userID, _wfName);
//End    TT#283 - MD - User name not designated in Task List - RBeck

						_dtTaskAllocateDetail.Rows[i]["WorkflowMethodType"] = MIDText.GetTextOnly(Convert.ToInt32(eWorkflowMethodType.Workflow));
					}

					if (_dtTaskAllocateDetail.Rows[i]["EXECUTE_CDR_RID"] != System.DBNull.Value)
					{
						if (Convert.ToInt32(_dtTaskAllocateDetail.Rows[i]["EXECUTE_CDR_RID"]) == Include.UndefinedCalendarDateRange)
						{
							_dtTaskAllocateDetail.Rows[i]["EXECUTE_CDR_RID"] = System.DBNull.Value;
						}
						else
						{
							_dtTaskAllocateDetail.Rows[i]["Execute Date Range"] = _SAB.ClientServerSession.Calendar.GetDisplayDate(_SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(_dtTaskAllocateDetail.Rows[i]["EXECUTE_CDR_RID"]), _SAB.ClientServerSession.Calendar.CurrentDate));
						}
					}

					_dtTaskAllocateDetail.Rows[i]["DisplaySequence"] = i;
				}

				foreach (DataRow row in _dtTaskAllocate.Rows)
				{
					dataSetEntry = GetAllocateDataSetEntry(Convert.ToInt32(row["TASK_SEQUENCE"]));
					dataSetEntry.AddMainRow(row);
				}

				foreach (DataRow row in _dtTaskAllocateDetail.Rows)
				{
					dataSetEntry = GetAllocateDataSetEntry(Convert.ToInt32(row["TASK_SEQUENCE"]));
					dataSetEntry.AddDetailRow(row);
				}
				
				// Setup Rollup datatable and grid

				_dtTaskRollup = _dlSchedule.TaskRollup_ReadByTaskList(_taskListProf.Key);

				//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				//_dtTaskRollup.Columns.Add("Merchandise", typeof(string));
				_dtTaskRollup.Columns.Add("RollupMerchandise", typeof(string));
				//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				_dtTaskRollup.Columns.Add("Rollup Date Range", typeof(string));
				_dtTaskRollup.Columns.Add("From Level", typeof(object));
				_dtTaskRollup.Columns.Add("To Level", typeof(object));
				_dtTaskRollup.Columns.Add("Posting", typeof(bool));
				_dtTaskRollup.Columns.Add("Reclass", typeof(bool));
				_dtTaskRollup.Columns.Add("Hierarchy Levels", typeof(bool));
				_dtTaskRollup.Columns.Add("Day To Week", typeof(bool));
				_dtTaskRollup.Columns.Add("Day", typeof(bool));
				_dtTaskRollup.Columns.Add("Week", typeof(bool));
				_dtTaskRollup.Columns.Add("Store", typeof(bool));
				_dtTaskRollup.Columns.Add("Chain", typeof(bool));
				_dtTaskRollup.Columns.Add("Store To Chain", typeof(bool));
				_dtTaskRollup.Columns.Add("Intransit", typeof(bool));

				_dtTaskRollup.Columns.Add("DisplaySequence", typeof(int));

				for (i = 0; i < _dtTaskRollup.Rows.Count; i++)
				{
					if (_dtTaskRollup.Rows[i]["HN_RID"] != System.DBNull.Value)
					{
						//Begin TT#352 - JScott - TASK LIST ->Drag and drop style 369621/005 into task list. Only display color. Need style/color to display!!!
						////Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
						////_dtTaskRollup.Rows[i]["Merchandise"] = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtTaskRollup.Rows[i]["HN_RID"])).Text;
						//_dtTaskRollup.Rows[i]["RollupMerchandise"] = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtTaskRollup.Rows[i]["HN_RID"])).Text;
						////End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
						_dtTaskRollup.Rows[i]["RollupMerchandise"] = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtTaskRollup.Rows[i]["HN_RID"]), false, true).Text;
						//End TT#352 - JScott - TASK LIST ->Drag and drop style 369621/005 into task list. Only display color. Need style/color to display!!!
					}

					if (_dtTaskRollup.Rows[i]["ROLLUP_CDR_RID"] != System.DBNull.Value)
					{
						if (Convert.ToInt32(_dtTaskRollup.Rows[i]["ROLLUP_CDR_RID"]) == Include.UndefinedCalendarDateRange)
						{
							_dtTaskRollup.Rows[i]["ROLLUP_CDR_RID"] = System.DBNull.Value;
						}
						else
						{
							_dtTaskRollup.Rows[i]["Rollup Date Range"] = _SAB.ClientServerSession.Calendar.GetDisplayDate(_SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(_dtTaskRollup.Rows[i]["ROLLUP_CDR_RID"]), _SAB.ClientServerSession.Calendar.CurrentDate));
						}
					}

                    if (_dtTaskRollup.Rows[i]["TO_PH_RID"] != System.DBNull.Value && _dtTaskRollup.Rows[i]["TO_PHL_SEQUENCE"] == System.DBNull.Value)
					{
						_dtTaskRollup.Rows[i]["TO_PHL_SEQUENCE"] = 0;
					}

					if (_dtTaskRollup.Rows[i]["FROM_PH_RID"] != System.DBNull.Value && _dtTaskRollup.Rows[i]["FROM_PHL_SEQUENCE"] == System.DBNull.Value)
					{
						_dtTaskRollup.Rows[i]["FROM_PHL_SEQUENCE"] = 0;
					}

					_dtTaskRollup.Rows[i]["Posting"] = (Convert.ToChar(_dtTaskRollup.Rows[i]["POSTING_IND"]) == '1') ? true : false;
					if (_dtTaskRollup.Rows[i]["RECLASS_IND"] != System.DBNull.Value)
					{
						_dtTaskRollup.Rows[i]["Reclass"] = (Convert.ToChar(_dtTaskRollup.Rows[i]["RECLASS_IND"]) == '1') ? true : false;
					}
					else
					{
						_dtTaskRollup.Rows[i]["Reclass"] = false;
					}

					_dtTaskRollup.Rows[i]["Hierarchy Levels"] = (Convert.ToChar(_dtTaskRollup.Rows[i]["HIERARCHY_LEVELS_IND"]) == '1') ? true : false;
					_dtTaskRollup.Rows[i]["Day To Week"] = (Convert.ToChar(_dtTaskRollup.Rows[i]["DAY_TO_WEEK_IND"]) == '1') ? true : false;
					_dtTaskRollup.Rows[i]["Day"] = (Convert.ToChar(_dtTaskRollup.Rows[i]["DAY_IND"]) == '1') ? true : false;
					_dtTaskRollup.Rows[i]["Week"] = (Convert.ToChar(_dtTaskRollup.Rows[i]["WEEK_IND"]) == '1') ? true : false;
					_dtTaskRollup.Rows[i]["Store"] = (Convert.ToChar(_dtTaskRollup.Rows[i]["STORE_IND"]) == '1') ? true : false;
					_dtTaskRollup.Rows[i]["Chain"] = (Convert.ToChar(_dtTaskRollup.Rows[i]["CHAIN_IND"]) == '1') ? true : false;
					_dtTaskRollup.Rows[i]["Store To Chain"] = (Convert.ToChar(_dtTaskRollup.Rows[i]["STORE_TO_CHAIN_IND"]) == '1') ? true : false;
					if (_dtTaskRollup.Rows[i]["INTRANSIT_IND"] != System.DBNull.Value)
					{
						_dtTaskRollup.Rows[i]["Intransit"] = (Convert.ToChar(_dtTaskRollup.Rows[i]["INTRANSIT_IND"]) == '1') ? true : false;
					}
					else
					{
						_dtTaskRollup.Rows[i]["Intransit"] = false;
					}
					_dtTaskRollup.Rows[i]["DisplaySequence"] = i;
				}

				foreach (DataRow row in _dtTaskRollup.Rows)
				{
					dataTableEntry = GetRollupDataTableEntry(Convert.ToInt32(row["TASK_SEQUENCE"]));
					dataTableEntry.AddRow(row);
				}

				//Begin TT#391 - stodd - Size day to week summary

				_dtTaskSizeDayToWeekSummary = _dlSchedule.TaskSizeDayToWeekSummary_ReadByTaskList(_taskListProf.Key);
				_dtTaskSizeDayToWeekSummary.PrimaryKey = new DataColumn[] { _dtTaskSizeDayToWeekSummary.Columns["TASKLIST_RID"], _dtTaskSizeDayToWeekSummary.Columns["TASK_SEQUENCE"] };

				//End TT#391 - stodd - Size day to week summary


				//Begin TT#155 - JScott - Size Curve Method
				// Setup Size Curve Method datatable and grid

				//Begin TT#155 - JScott - Add Size Curve info to Node Properties
				//_dtTaskSizeCurveMethod = _dlSchedule.TaskSizeCurveGenerate_ReadByTaskList(_taskListProf.Key);
				_dtTaskSizeCurveMethod = _dlSchedule.TaskSizeCurveMethod_ReadByTaskList(_taskListProf.Key);
				//End TT#155 - JScott - Add Size Curve info to Node Properties

				_dtTaskSizeCurveMethod.Columns.Add("SizeCurveMethod", typeof(string));
				_dtTaskSizeCurveMethod.Columns.Add("Execute Date Range", typeof(string));

				_dtTaskSizeCurveMethod.Columns.Add("DisplaySequence", typeof(int));

				for (i = 0; i < _dtTaskSizeCurveMethod.Rows.Count; i++)
				{
					_dtTaskSizeCurveMethod.Rows[i]["SizeCurveMethod"] = _dlWorkflowMethod.GetMethodName(Convert.ToInt32(_dtTaskSizeCurveMethod.Rows[i]["METHOD_RID"]));

					if (_dtTaskSizeCurveMethod.Rows[i]["EXECUTE_CDR_RID"] != System.DBNull.Value)
					{
						if (Convert.ToInt32(_dtTaskSizeCurveMethod.Rows[i]["EXECUTE_CDR_RID"]) == Include.UndefinedCalendarDateRange)
						{
							_dtTaskSizeCurveMethod.Rows[i]["EXECUTE_CDR_RID"] = System.DBNull.Value;
						}
						else
						{
							_dtTaskSizeCurveMethod.Rows[i]["Execute Date Range"] = _SAB.ClientServerSession.Calendar.GetDisplayDate(_SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(_dtTaskSizeCurveMethod.Rows[i]["EXECUTE_CDR_RID"]), _SAB.ClientServerSession.Calendar.CurrentDate));
						}
					}

					_dtTaskSizeCurveMethod.Rows[i]["DisplaySequence"] = i;
				}

				foreach (DataRow row in _dtTaskSizeCurveMethod.Rows)
				{
					dataTableEntry = GetSizeCurveMethodDataTableEntry(Convert.ToInt32(row["TASK_SEQUENCE"]));
					dataTableEntry.AddRow(row);
				}

				//End TT#155 - JScott - Size Curve Method
				//Begin TT#155 - JScott - Add Size Curve info to Node Properties
				// Setup Size Curves datatable and grid

				_dtTaskSizeCurves = _dlSchedule.TaskSizeCurves_ReadByTaskList(_taskListProf.Key);

				_dtTaskSizeCurves.Columns.Add("SizeCurvesMerchandise", typeof(string));
				_dtTaskSizeCurves.Columns.Add("DisplaySequence", typeof(int));

				for (i = 0; i < _dtTaskSizeCurves.Rows.Count; i++)
				{
					if (_dtTaskSizeCurves.Rows[i]["HN_RID"] != System.DBNull.Value)
					{
						//Begin TT#443 - JScott - Task List - Size Curve - Drag and drop Style/color 369621/001 ; but merchandise field only display 001 (should be entire ID of style/color)
						//_dtTaskSizeCurves.Rows[i]["SizeCurvesMerchandise"] = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtTaskSizeCurves.Rows[i]["HN_RID"])).Text;
						_dtTaskSizeCurves.Rows[i]["SizeCurvesMerchandise"] = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(_dtTaskSizeCurves.Rows[i]["HN_RID"]), false, true).Text;
						//End TT#443 - JScott - Task List - Size Curve - Drag and drop Style/color 369621/001 ; but merchandise field only display 001 (should be entire ID of style/color)
					}

					_dtTaskSizeCurves.Rows[i]["DisplaySequence"] = i;
				}

				foreach (DataRow row in _dtTaskSizeCurves.Rows)
				{
					dataTableEntry = GetSizeCurvesDataTableEntry(Convert.ToInt32(row["TASK_SEQUENCE"]));
					dataTableEntry.AddRow(row);
				}

				//End TT#155 - JScott - Add Size Curve info to Node Properties
				//Begin TT#707 - JScott - Size Curve process needs to multi-thread
				_dtTaskSizeCurvesNode = _dlSchedule.TaskSizeCurveGenerateNode_ReadByTaskList(_taskListProf.Key);
				_dtTaskSizeCurvesNode.PrimaryKey = new DataColumn[] { _dtTaskSizeCurvesNode.Columns["TASKLIST_RID"], _dtTaskSizeCurvesNode.Columns["TASK_SEQUENCE"] };

				//End TT#707 - JScott - Size Curve process needs to multi-thread
				// Setup Posting datatable and grid

				_dtTaskPosting = _dlSchedule.TaskPosting_ReadByTaskList(_taskListProf.Key);
				_dtTaskPosting.PrimaryKey = new DataColumn[] { _dtTaskPosting.Columns["TASKLIST_RID"], _dtTaskPosting.Columns["TASK_SEQUENCE"] };

				// Begin TT#1595-MD - stodd - Batch Comp
                // Setup Batch Comp datatable and grid

                _dtTaskBatchComp = _dlSchedule.TaskBatchComp_ReadByTaskList(_taskListProf.Key);
                _dtTaskBatchComp.PrimaryKey = new DataColumn[] { _dtTaskBatchComp.Columns["TASKLIST_RID"], _dtTaskBatchComp.Columns["TASK_SEQUENCE"] };
				// End TT#1595-MD - stodd - Batch Comp

                // Begin TT#1581-MD - stodd - header reconcile
                // Setup Batch Comp datatable and grid
                _dtTaskHeaderReconcile = _dlSchedule.TaskHeaderReconcile_ReadByTaskList(_taskListProf.Key);
                _dtTaskHeaderReconcile.PrimaryKey = new DataColumn[] { _dtTaskHeaderReconcile.Columns["TASKLIST_RID"], _dtTaskHeaderReconcile.Columns["TASK_SEQUENCE"] };
                // End TT#1581-MD - stodd - header reconcile

				
				// Setup Program datatable and grid

				_dtTaskProgram = _dlSchedule.TaskProgram_ReadByTaskList(_taskListProf.Key);
				_dtTaskProgram.PrimaryKey = new DataColumn[] { _dtTaskProgram.Columns["TASKLIST_RID"], _dtTaskProgram.Columns["TASK_SEQUENCE"] };

				// Display

                //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                HideTabs();
                //pnlPosting.Visible = false;
                //pnlProgram.Visible = false;
                //pnlForecast.Visible = false;
                //pnlAllocate.Visible = false;
                //pnlRollup.Visible = false;
                ////Begin TT#155 - JScott - Size Curve Method
                //pnlSizeCurveMethod.Visible = false;
                ////End TT#155 - JScott - Size Curve Method
                ////Begin TT#155 - JScott - Add Size Curve info to Node Properties
                //pnlSizeCurves.Visible = false;
                ////End TT#155 - JScott - Add Size Curve info to Node Properties
                //pnlRelieve.Visible = false;
                ////Begin TT#391 - STodd - Size day to week summary
                //pnlSizeDayToWeekSummary.Visible = false;
                ////End TT#391 - STodd - Size day to week summary
                //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application


				chkPostingRunUntil.Checked = false;
				txtPostingRunUntilMask.Enabled = false;
				chkRelieveRunUntil.Checked = false;
				txtRelieveRunUntilMask.Enabled = false;

				BindTaskGridData();

				_nextTaskSequence = _dtTask.Rows.Count;
				_nextForecastSequence = _dtTaskForecast.Rows.Count;
				_nextAllocateSequence = _dtTaskAllocate.Rows.Count;
				_nextRollupSequence = _dtTaskRollup.Rows.Count;
				//Begin TT#155 - JScott - Size Curve Method
				_nextSizeCurveMethodSequence = _dtTaskSizeCurveMethod.Rows.Count;
				//End TT#155 - JScott - Size Curve Method
				//Begin TT#155 - JScott - Add Size Curve info to Node Properties
				_nextSizeCurvesSequence = _dtTaskSizeCurves.Rows.Count;
				//End TT#155 - JScott - Add Size Curve info to Node Properties
				_nextForecastDetailSequence = _dtTaskForecastDetail.Rows.Count;
				_nextAllocateDetailSequence = _dtTaskAllocateDetail.Rows.Count;

				// Set MIDFormBase security

				SetReadOnly(FunctionSecurity.AllowUpdate);

                // Begin TT#207 Track #6451 RMatelic - Task List - Protect "Date Range" column
                if (ulgTasks.ActiveRow != null)
                {
                    switch ((eTaskType)Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_TYPE"].Value))
                    {
                        case eTaskType.Allocate:

                            ulgAllocate.DisplayLayout.Bands[1].Columns["Execute Date Range"].CellActivation = Activation.NoEdit;
                            break;

                        case eTaskType.Forecasting:

                            ulgForecast.DisplayLayout.Bands[1].Columns["Execute Date Range"].CellActivation = Activation.NoEdit;
                            break;

                        case eTaskType.Rollup:

                            ulgRollup.DisplayLayout.Bands[0].Columns["Rollup Date Range"].CellActivation = Activation.NoEdit;
                            break;
						//Begin TT#155 - JScott - Size Curve Method

						case eTaskType.SizeCurveMethod:

							ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["Execute Date Range"].CellActivation = Activation.NoEdit;
							break;
						//End TT#155 - JScott - Size Curve Method
					}
                }
                // End TT#207 
		
                // Security

				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				//if (_initialUserRID != _SAB.ClientServerSession.UserRID &&
				//    _initialUserRID != Include.GlobalUserRID &&
				//    _initialUserRID != Include.SystemUserRID &&
				//    _initialUserRID != Include.NoRID)
				//{
				//    rdoOwner.Text = rdoOwner.Text.Replace("{1}", _SAB.ClientServerSession.GetUserName(_initialUserRID));
				//}
				//else
				//{
				//    rdoOwner.Visible = false;
				//}

				//rdoUser.Enabled = true;
				//rdoGlobal.Enabled = true;
				//rdoSystem.Enabled = true;
				//rdoOwner.Enabled = true;

				//// Check appropriate User Radio Button

				//if (_initialUserRID == Include.GlobalUserRID)
				//{
				//    if (_SAB.ClientServerSession.UserRID == Include.GlobalUserRID)
				//    {
				//        rdoUser.Checked = true;
				//    }
				//    else
				//    {
				//        rdoGlobal.Checked = true;
				//    }
				//}
				//else if (_initialUserRID == Include.SystemUserRID)
				//{
				//    if (_SAB.ClientServerSession.UserRID == Include.SystemUserRID)
				//    {
				//        rdoUser.Checked = true;
				//    }
				//    else
				//    {
				//        rdoSystem.Checked = true;
				//    }
				//}
				//else if (_initialUserRID == _SAB.ClientServerSession.UserRID || _initialUserRID == Include.NoRID)
				//{
				//    rdoUser.Checked = true;
				//}
				//else
				//{
				//    rdoOwner.Checked = true;
				//}

				//// Enable User Radio Buttons

				//if (_readOnly || !_globalSecLvl.AllowUpdate || _SAB.ClientServerSession.UserRID == Include.GlobalUserRID)
				//{
				//    rdoGlobal.Enabled = false;
				//}

				//if (_readOnly || !_systemSecLvl.AllowUpdate || _SAB.ClientServerSession.UserRID == Include.SystemUserRID)
				//{
				//    rdoSystem.Enabled = false;
				//}

				//if (_readOnly || !_userSecLvl.AllowUpdate)
				//{
				//    rdoUser.Enabled = false;
				//}

				//if (_readOnly || !rdoOwner.Visible || (!_userSecLvl.AllowUpdate && !_systemSecLvl.AllowUpdate))
				//{
				//    rdoOwner.Enabled = false;
				//}

				//// Enable Save/OK/Save As Button


				//if (!_readOnly &&
				//    ((rdoGlobal.Checked && rdoGlobal.Enabled) ||
				//    (rdoSystem.Checked && rdoSystem.Enabled) ||
				//    (rdoUser.Checked && rdoUser.Enabled) ||
				//    (rdoOwner.Checked && rdoOwner.Enabled)))
				//{
				//    _canUpdateTaskList = true;
				//}
				//else
				//{
				//    _canUpdateTaskList = false;
				//}

				//btnSave.Enabled = _canUpdateTaskList;
				//btnOK.Enabled = _canUpdateTaskList;
				//btnSaveAs.Enabled = _canUpdateTaskList;

				//// Enable Run-Now Button

				//btnRunNow.Enabled = false;

				//if (_SAB.SchedulerServerSession != null)
				//{
				//    if (_initialUserRID == Include.GlobalUserRID && _globalSecLvl.AllowExecute)
				//    {
				//        btnRunNow.Enabled = true;
				//    }
				//    else if (_initialUserRID == Include.SystemUserRID && _systemSecLvl.AllowExecute)
				//    {
				//        btnRunNow.Enabled = true;
				//    }
				//    else if (_initialUserRID == _SAB.ClientServerSession.UserRID && _userSecLvl.AllowExecute)
				//    {
				//        btnRunNow.Enabled = true;
				//    }
				//    else if (_systemSecLvl.AllowExecute)
				//    {
				//        btnRunNow.Enabled = true;
				//    }
				//}
				SetUserFields(_taskListProf, _initialUserRID, _initialOwnerRID);
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

				FormLoaded = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		// Begin TT#391 - Stodd - size day to week summary
		private void SetText()
		{
			lblSizeDayWeekSummary.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SizeDayToWeekSummary);
			lblSizeDayWeekSummaryDt.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DateRangeToProcess);
			lblOverrideNode.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OverrideMerchandise);
			//Begin TT#707 - JScott - Size Curve process needs to multi-thread
			lblSizeCurveConcurrentProcesses.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ConcurrentSizeCurveProcesses);
			//End TT#707 - JScott - Size Curve process needs to multi-thread
            lblProcessingDirection.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_File_Processing_Direction) + ":";  // TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files
		}
		// End TT#391 - Stodd - size day to week summary

		private void frmTaskListProperties_Activated(object sender, System.EventArgs e)
		{
			try
			{
				txtTaskListName.Focus();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void frmTaskListProperties_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				if (_heldJobs != null)
				{
					_SAB.SchedulerServerSession.ResumeAllJobs(_heldJobs, _SAB.ClientServerSession.UserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
				}

				if (OnTaskListPropertiesCloseHandler != null)
				{
					OnTaskListPropertiesCloseHandler(this, new TaskListPropertiesCloseEventArgs());
				}
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
                //BEGIN TT#4476-VSuart-Saving a Task list always goes through the Save As-MID
                //BEGIN TT#1435-VStuart-Attempt to Create Duplicate Size Curve Load Task List Causes Database Unique Index
                Save(eUpdateMode.Update);
                //Save(eUpdateMode.Create);
                //END TT#1435-VStuart-Attempt to Create Duplicate Size Curve Load Task List Causes Database Unique Index
                //END TT#4476-VSuart-Saving a Task list always goes through the Save As-MID
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
			string name;
			bool saveRC;

			try
			{
				if (ChangePending)
				{
					if (_canUpdateTaskList)
					{
						//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
						//saveRC = Save(eUpdateMode.Update);
						if (btnSave.Enabled)
						{
							saveRC = Save(eUpdateMode.Update);
						}
						else
						{
							saveRC = Save(eUpdateMode.Create);
						}
						//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
					}
					else
					{
						saveRC = (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoSecurityToSave), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes);
					}
				}
				else
				{
					saveRC = true;
				}

				if (saveRC)
				{
					name = _taskListProf.GetUniqueName();

					_SAB.SchedulerServerSession.ScheduleNewJob(new ScheduleProfile(Include.NoRID, name), new JobProfile(Include.NoRID, name, true), _taskListProf.Key, _SAB.ClientServerSession.UserRID);

					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TaskListHasBeenSubmitted), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void txtTaskListName_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				EnableSaveButtons();
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			}
		}

		private void chkPostingRunUntil_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (chkPostingRunUntil.Checked)
				{
					_currentPostingRow["RUN_UNTIL_FILE_PRESENT_IND"] = '1';
					txtPostingRunUntilMask.Enabled = true;
				}
				else
				{
					_currentPostingRow["RUN_UNTIL_FILE_PRESENT_IND"] = '0';
					txtPostingRunUntilMask.Enabled = false;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void chkRelieveRunUntil_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (chkRelieveRunUntil.Checked)
				{
					_currentPostingRow["RUN_UNTIL_FILE_PRESENT_IND"] = '1';
					txtRelieveRunUntilMask.Enabled = true;
				}
				else
				{
					_currentPostingRow["RUN_UNTIL_FILE_PRESENT_IND"] = '0';
					txtRelieveRunUntilMask.Enabled = false;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void rdoUser_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
                _canUpdateTaskList = true; //TT#4000 - DOConnell - Task List Explorer - Read only system Hierarchy Load task allows users to edit
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				//btnSaveAs.Enabled = true;
				EnableSaveButtons();
                RebindAllocateTaskHeaderFilters();  //TT#1446-MD -jsobek -Header Filter - Opening a Task List with a user Header Filter the filter name appears as a number
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			}
		}

		private void rdoGlobal_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				//btnSaveAs.Enabled = true;
				EnableSaveButtons();
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			}
		}

		private void rdoSystem_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				//btnSaveAs.Enabled = true;
				EnableSaveButtons();
          
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			}
		}

		private void rdoOwner_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				//btnSaveAs.Enabled = true;
				EnableSaveButtons();
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			}
		}

		private void btnPostingDirectory_Click(object sender, System.EventArgs e)
		{
			try
			{
				fbdDirectory.SelectedPath = txtPostingDirectory.Text.Trim();
				fbdDirectory.Description = "Select the directory where the " + ulgTasks.ActiveRow.Cells["Task"].Value + " input file(s) will be found.";

				if (fbdDirectory.ShowDialog() == DialogResult.OK)
				{
					txtPostingDirectory.Text = fbdDirectory.SelectedPath;

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void btnRelieveDirectory_Click(object sender, System.EventArgs e)
		{
			try
			{
				fbdDirectory.SelectedPath = txtRelieveDirectory.Text.Trim();
				fbdDirectory.Description = "Select the directory where the Relieve Intransit input file(s) will be found.";

				if (fbdDirectory.ShowDialog() == DialogResult.OK)
				{
					txtRelieveDirectory.Text = fbdDirectory.SelectedPath;

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void btnProgram_Click(object sender, System.EventArgs e)
		{
			try
			{
				ofdFile.InitialDirectory = "Desktop";
				ofdFile.FileName = txtProgramPath.Text.Trim();
                // Begin TT#491 - JSmith - Modify to allow command files
                //ofdFile.Filter = "Executable Files (*.exe)|*.exe";
                ofdFile.Filter = "Executable Files (*.exe)|*.exe|Command Files (*.cmd)|*.cmd";
                // End TT#491
				ofdFile.FilterIndex = 1;
				ofdFile.RestoreDirectory = true;

				if (ofdFile.ShowDialog() == DialogResult.OK)
				{
					txtProgramPath.Text = ofdFile.FileName;

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgTasks_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			ChangePending = true;
		}
	
		private void ulgTasks_AfterRowActivate(object sender, System.EventArgs e)
		{
			try
			{
				ShowTaskPanel();
                RollUpLoaded = true;  //TT#2744 - MD - Rollup task in 5.0 asking to save when nothing has chenged - RBeck
			}			
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}
		
		private void ulgTasks_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			Point mousePos;

			try
			{
				if (!_afterAddMenu)
				{
					mousePos = System.Windows.Forms.Cursor.Position;
					ctmAddTask.Show(ulgTasks, ulgTasks.PointToClient(mousePos));
					e.Cancel = true;
				}
				else
				{
					ulgTasks.BeginUpdate();
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgTasks_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				e.Row.Cells["Task"].Value = MIDText.GetTextOnly((int)_addedType);
				e.Row.Cells["DisplaySequence"].Value = _nextTaskSequence;
				e.Row.Cells["TASKLIST_RID"].Value = _taskListProf.Key;
				e.Row.Cells["TASK_SEQUENCE"].Value = _nextTaskSequence;
				e.Row.Cells["TASK_TYPE"].Value = (int)_addedType;
                // Begin TT#1127 - JSmith - Severe error didn't roll up to Scheduler Service
                //e.Row.Cells["MAX_MESSAGE_LEVEL"].Value = eMIDMessageLevel.Information;
                e.Row.Cells["MAX_MESSAGE_LEVEL"].Value = eMIDMessageLevel.Severe;
                // End TT#1127
                

                


				_nextTaskSequence++;

				e.Row.Band.SortedColumns.RefreshSort(true);
				ulgTasks.UpdateData();
                ////Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                //DataRow[] drTask = _dtTask.Select("TASKLIST_RID=" + (int)e.Row.Cells["TASKLIST_RID"].Value + " AND TASK_SEQUENCE=" + (int)e.Row.Cells["TASK_SEQUENCE"].Value);
                // this.emailTaskList1.SaveToDataRow(drTask[0]);
                ////End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
				_dtTask.AcceptChanges();
				ulgTasks.EndUpdate();
				ShowTaskPanel();

				ChangePending = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgTasks_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
			try
			{
				if (!_bypassBeforeDelete)
				{
					_bypassBeforeDelete = true;

					try
					{
						switch ((eTaskType)ulgTasks.Selected.Rows[0].Cells["TASK_TYPE"].Value)
						{
							case eTaskType.Allocate :
				
								_allocateDataSetList.Remove(Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value));
								break;

							case eTaskType.Forecasting :
				
								_forecastDataSetList.Remove(Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value));
								break;

							case eTaskType.computationDriver :
				
								_forecastDataSetList.Remove(Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value));
								break;

							case eTaskType.Rollup :

								_rollupDataTableList.Remove(Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value));
								break;

							//Begin TT#418 - JScott - Did not want the Size Curve Method did a Right Click and Delete recived Index Out of Range Error.
							////Begin TT#155 - JScott - Size Curve Method
							//case eTaskType.SizeCurveMethod:

							//    _sizeCurveMethodDataTableList.Remove(Convert.ToInt32(ulgSizeCurveMethod.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value));
							//    break;

							////End TT#155 - JScott - Size Curve Method
							////Begin TT#155 - JScott - Add Size Curve info to Node Properties
							//case eTaskType.SizeCurves:

							//    _sizeCurvesDataTableList.Remove(Convert.ToInt32(ulgSizeCurves.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value));
							//    break;

							////End TT#155 - JScott - Add Size Curve info to Node Properties
							case eTaskType.SizeCurveMethod:

								_sizeCurveMethodDataTableList.Remove(Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value));
								break;

							case eTaskType.SizeCurves:

								_sizeCurvesDataTableList.Remove(Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value));
								//Begin TT#707 - JScott - Size Curve process needs to multi-thread

								foreach (DataRow row in _dtTaskSizeCurvesNode.Rows)
								{
									if (Convert.ToInt32(row["TASKLIST_RID"]) == Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASKLIST_RID"].Value) &&
										Convert.ToInt32(row["TASK_SEQUENCE"]) == Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value))
									{
										row.Delete();
									}
								}

								_dtTaskSizeCurvesNode.AcceptChanges();
								//End TT#707 - JScott - Size Curve process needs to multi-thread
								break;

							//Begin TT#418 - JScott - Did not want the Size Curve Method did a Right Click and Delete recived Index Out of Range Error.
							//Begin TT#391 - stodd - size day to week summary
							case eTaskType.SizeDayToWeekSummary:

								foreach (DataRow row in _dtTaskSizeDayToWeekSummary.Rows)
								{
									if (Convert.ToInt32(row["TASKLIST_RID"]) == Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASKLIST_RID"].Value) &&
										Convert.ToInt32(row["TASK_SEQUENCE"]) == Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value))
									{
										row.Delete();
									}
								}

								_dtTaskSizeDayToWeekSummary.AcceptChanges();
								break;
							//End TT#391 - stodd - size day to week summary
							//Begin MOD - JScott - Build Pack Criteria Load
							case eTaskType.BuildPackCriteriaLoad :
							//End MOD - JScott - Build Pack Criteria Load
							case eTaskType.ColorCodeLoad:
							case eTaskType.HeaderLoad :
							case eTaskType.HierarchyLoad :
							case eTaskType.HistoryPlanLoad :
							case eTaskType.SizeCodeLoad :
							case eTaskType.SizeCurveLoad :
							case eTaskType.SizeConstraintsLoad :
							case eTaskType.StoreLoad :
							case eTaskType.RelieveIntransit :
                            //BEGIN TT#820 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Add Task List Options for VSW and Store Eligibility Load processes
                            case eTaskType.StoreEligibilityCriteriaLoad :
                            case eTaskType.VSWCriteriaLoad :
                            //END TT#820 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Add Task List Options for VSW and Store Eligibility Load processes
                            case eTaskType.DailyPercentagesCriteriaLoad:     // TT#43 - MD - DOConnell - Projected Sales Enhancement //TT#816 - MD - DOConnell - Corrected misspelling
                            case eTaskType.ChainSetPercentCriteriaLoad:    // TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
                            
								foreach (DataRow row in _dtTaskPosting.Rows)
								{
									if (Convert.ToInt32(row["TASKLIST_RID"]) == Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASKLIST_RID"].Value) &&
										Convert.ToInt32(row["TASK_SEQUENCE"]) == Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value))
									{
										row.Delete();
									}
								}

								_dtTaskPosting.AcceptChanges();
								break;

							// Begin TT#1595-MD - stodd - Batch Comp
                            case eTaskType.BatchComp:    

                                foreach (DataRow row in _dtTaskBatchComp.Rows)
                                {
                                    if (Convert.ToInt32(row["TASKLIST_RID"]) == Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASKLIST_RID"].Value) &&
                                        Convert.ToInt32(row["TASK_SEQUENCE"]) == Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value))
                                    {
                                        row.Delete();
                                    }
                                }

                                _dtTaskBatchComp.AcceptChanges();
                                break;
							// End TT#1595-MD - stodd - Batch Comp

                            // Begin TT#1581-MD - stodd - header reconcile
                            case eTaskType.HeaderReconcile:

                                foreach (DataRow row in _dtTaskHeaderReconcile.Rows)
                                {
                                    if (Convert.ToInt32(row["TASKLIST_RID"]) == Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASKLIST_RID"].Value) &&
                                        Convert.ToInt32(row["TASK_SEQUENCE"]) == Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value))
                                    {
                                        row.Delete();
                                    }
                                }

                                _dtTaskHeaderReconcile.AcceptChanges();
                                break;
                            // End TT#1581-MD - stodd - header reconcile

	
							case eTaskType.ExternalProgram :

								foreach (DataRow row in _dtTaskProgram.Rows)
								{
									if (Convert.ToInt32(row["TASKLIST_RID"]) == Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASKLIST_RID"].Value) &&
										Convert.ToInt32(row["TASK_SEQUENCE"]) == Convert.ToInt32(ulgTasks.Selected.Rows[0].Cells["TASK_SEQUENCE"].Value))
									{
										row.Delete();
									}
								}

								_dtTaskProgram.AcceptChanges();
								break;
						}

						ulgTasks.Selected.Rows[0].Delete(false);
						ulgTasks.UpdateData();
						_dtTask.AcceptChanges();
						ClearTaskSelection();

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

		private void ulgForecast_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			string cellValue;
			HierarchyNodeProfile hnp;

			try
			{
				if (!_bypassAfterUpdate)
				{
					_bypassAfterUpdate = true;

					try
					{
						switch (e.Cell.Column.Key)
						{
							case "HN_RID":

								//Begin Track #6479 - JScott - Allocate process - only color displays in merchandise field - need style too
								//hnp = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(e.Cell.Value));
								hnp = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(e.Cell.Value), false, true);
								//End Track #6479 - JScott - Allocate process - only color displays in merchandise field - need style too

								if (hnp.Key != Include.NoRID)
								{
                                    //Begin TT#69 - JSmith - Cannot drag-drop a merchandise node in to an Allocate Task List.  I can type one in.
                                    ////Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
                                    ////e.Cell.Row.Cells["Merchandise"].Value = hnp.Text;
                                    //e.Cell.Row.Cells["ForecastMerchandise"].Value = hnp.Text;
                                    ////End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
                                    if (SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(hnp.Key, (int)eSecurityTypes.Store).AllowUpdate)
                                    {
                                        e.Cell.Row.Cells["ForecastMerchandise"].Value = hnp.Text;
                                    }
                                    else
                                    {
                                        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        e.Cell.Row.Cells["ForecastMerchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToMethodValue);
                                    }
                                    //End TT#69
								}
								else
								{
									MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

									//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
									//e.Cell.Row.Cells["Merchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToMethodValue);
									e.Cell.Row.Cells["ForecastMerchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToMethodValue);
									//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
								}

								break;

							//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
							//case "Merchandise":
							case "ForecastMerchandise":
							//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.

								cellValue = e.Cell.Value.ToString().Trim();
 
								if (cellValue.Length > 0)
								{
//									hnp = _SAB.HierarchyServerSession.GetNodeData(cellValue.Split(new char[] { '[' })[0].Trim());
									hnp =  GetNodeProfile(cellValue);

									if (hnp.Key != Include.NoRID)
									{
										e.Cell.Row.Cells["HN_RID"].Value = hnp.Key;
										//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
										//e.Cell.Row.Cells["Merchandise"].Value = hnp.Text;
										e.Cell.Row.Cells["ForecastMerchandise"].Value = hnp.Text;
										//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
									}
									else
									{
										MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

										e.Cell.Row.Cells["HN_RID"].Value = System.DBNull.Value;
										//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
										//e.Cell.Row.Cells["Merchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToMethodValue);
										e.Cell.Row.Cells["ForecastMerchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToMethodValue);
										//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
									}
								}
								else
								{
									e.Cell.Row.Cells["HN_RID"].Value = System.DBNull.Value;
									//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
									//e.Cell.Row.Cells["Merchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToMethodValue);
									e.Cell.Row.Cells["ForecastMerchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToMethodValue);
									//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
								}

								break;

							case "Version":

								if (Convert.ToInt32(e.Cell.Value) == Include.NoRID)
								{
									e.Cell.Row.Cells["FV_RID"].Value = System.DBNull.Value;
								}
								else
								{
									e.Cell.Row.Cells["FV_RID"].Value = Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture);
								}
 
								break;
						}
					}
					catch (Exception exc)
					{
						string message = exc.ToString();
						throw;
					}
					finally
					{
						_bypassAfterUpdate = false;
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgForecast_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			ChangePending = true;
		}

		private void ulgForecast_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			try
			{
				ulgForecast.BeginUpdate();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgForecast_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				if (e.Row.Band.Index == 0)
				{
					e.Row.Cells["DisplaySequence"].Value = _nextForecastSequence;
					e.Row.Cells["TASKLIST_RID"].Value = _taskListProf.Key;
					e.Row.Cells["TASK_SEQUENCE"].Value = ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value;
					e.Row.Cells["FORECAST_SEQUENCE"].Value = _nextForecastSequence;
					//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
					//e.Row.Cells["Merchandise"].Value = "";
					e.Row.Cells["ForecastMerchandise"].Value = "";
					//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
					e.Row.Cells["Version"].Value = Include.NoRID;

					_nextForecastSequence++;
				}
				else
				{
					e.Row.Cells["DisplaySequence"].Value = _nextForecastDetailSequence;
					e.Row.Cells["TASKLIST_RID"].Value = e.Row.ParentRow.Cells["TASKLIST_RID"].Value;
					e.Row.Cells["TASK_SEQUENCE"].Value = e.Row.ParentRow.Cells["TASK_SEQUENCE"].Value;
					e.Row.Cells["FORECAST_SEQUENCE"].Value = e.Row.ParentRow.Cells["FORECAST_SEQUENCE"].Value;
					e.Row.Cells["DETAIL_SEQUENCE"].Value = _nextForecastDetailSequence;

					_nextForecastDetailSequence++;
				}

				e.Row.Activate();
				e.Row.Band.SortedColumns.RefreshSort(true);
				ulgForecast.UpdateData();
				ForecastAcceptChanges();
				ulgForecast.EndUpdate();

				ChangePending = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgForecast_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (e.Cell.Column.Key == "Execute Date Range")
				{
					ShowDateSelector(ulgForecast, e.Cell, "EXECUTE_CDR_RID");
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgForecast_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
			UltraGridChildBand childBand;
			ArrayList deleteRows;
			int i;

			try
			{
				if (!_bypassBeforeDelete)
				{
					_bypassBeforeDelete = true;

					try
					{
						if (ulgForecast.Selected.Rows.Count > 0 && ulgForecast.Selected.Rows[0].Band.Index == 0)
						{
							deleteRows = new ArrayList();

							childBand = ulgForecast.Selected.Rows[0].ChildBands[0];

							for (i = 0; i < childBand.Rows.Count; i++)
							{
								deleteRows.Add(childBand.Rows[i]);
							}

							foreach (UltraGridRow row in deleteRows)
							{
								row.Delete(false);
							}
						}

						ulgForecast.Selected.Rows[0].Delete(false);
						ulgForecast.Selected.Rows.Clear();
						ulgForecast.UpdateData();
						ForecastAcceptChanges();

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

		private void ulgAllocate_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			string cellValue;
			HierarchyNodeProfile hnp;

			try
			{
				if (!_bypassAfterUpdate)
				{
					_bypassAfterUpdate = true;

					try
					{
						switch (e.Cell.Column.Key)
						{
							case "HN_RID":

								//Begin Track #6479 - JScott - Allocate process - only color displays in merchandise field - need style too
								//hnp = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(e.Cell.Value));
								hnp = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(e.Cell.Value), false, true);
								//End Track #6479 - JScott - Allocate process - only color displays in merchandise field - need style too

								if (hnp.Key != Include.NoRID)
								{
                                    //Begin TT#69 - JSmith - Cannot drag-drop a merchandise node in to an Allocate Task List.  I can type one in.
                                    ////Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
                                    ////e.Cell.Row.Cells["Merchandise"].Value = hnp.Text;
                                    //e.Cell.Row.Cells["AllocateMerchandise"].Value = hnp.Text;
                                    ////End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
                                    if (SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(hnp.Key, (int)eSecurityTypes.Allocation).AllowUpdate)
                                    {
                                        e.Cell.Row.Cells["AllocateMerchandise"].Value = hnp.Text;
                                    }
                                    else
                                    {
                                        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        e.Cell.Row.Cells["AllocateMerchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
                                    }
                                    //End TT#69
								}
								else
								{
									MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

									//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
									//e.Cell.Row.Cells["Merchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
									e.Cell.Row.Cells["AllocateMerchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
									//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
								}

								break;

							//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
							//case "Merchandise":
							case "AllocateMerchandise":
							//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.

								cellValue = e.Cell.Value.ToString().Trim();
 
								if (cellValue.Length > 0)
								{
//									hnp = _SAB.HierarchyServerSession.GetNodeData(cellValue.Split(new char[] { '[' })[0].Trim());
									hnp =  GetNodeProfile(cellValue);

									if (hnp.Key != Include.NoRID)
									{
                                        //Begin TT#69 - JSmith - Cannot drag-drop a merchandise node in to an Allocate Task List.  I can type one in.
                                        //e.Cell.Row.Cells["HN_RID"].Value = hnp.Key;
                                        ////Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
                                        ////e.Cell.Row.Cells["Merchandise"].Value = hnp.Text;
                                        //e.Cell.Row.Cells["AllocateMerchandise"].Value = hnp.Text;
                                        ////End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
                                        if (SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(hnp.Key, (int)eSecurityTypes.Allocation).AllowUpdate)
                                        {
                                            e.Cell.Row.Cells["HN_RID"].Value = hnp.Key;
                                            
                                            e.Cell.Row.Cells["AllocateMerchandise"].Value = hnp.Text;
                                        }
                                        else
                                        {
                                            MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            e.Cell.Row.Cells["HN_RID"].Value = System.DBNull.Value;
                                            e.Cell.Row.Cells["AllocateMerchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
                                        }
                                        //End TT#69
									}
									else
									{
										MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

										e.Cell.Row.Cells["HN_RID"].Value = System.DBNull.Value;
										//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
										//e.Cell.Row.Cells["Merchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
										e.Cell.Row.Cells["AllocateMerchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
										//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
									}
								}
								else
								{
									e.Cell.Row.Cells["HN_RID"].Value = System.DBNull.Value;
									//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
									//e.Cell.Row.Cells["Merchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
									e.Cell.Row.Cells["AllocateMerchandise"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
									//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
								}

								break;
                            //Begin TT#1313-MD -jsobek -Header Filters
                            //case "Header":

                            //    cellValue = e.Cell.Value.ToString().Trim();

                            //    if (cellValue.Length > 0)
                            //    {
                            //        e.Cell.Row.Cells["HEADER_ID"].Value = cellValue;
                            //    }
                            //    else
                            //    {
                            //        e.Cell.Row.Cells["HEADER_ID"].Value = System.DBNull.Value;
                            //        e.Cell.Row.Cells["Header"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
                            //    }

                            //    break;

                            //case "PO":

                            //    cellValue = e.Cell.Value.ToString().Trim();

                            //    if (cellValue.Length > 0)
                            //    {
                            //        e.Cell.Row.Cells["PO_ID"].Value = cellValue;
                            //    }
                            //    else
                            //    {
                            //        e.Cell.Row.Cells["PO_ID"].Value = System.DBNull.Value;
                            //        e.Cell.Row.Cells["PO"].Value = MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue);
                            //    }

                            //    break;

                            //case "Type":

                            //    if (Convert.ToInt32(e.Cell.Value) == Include.NoRID)
                            //    {
                            //        e.Cell.Row.Cells["ALLOCATE_TYPE"].Value = System.DBNull.Value;
                            //    }
                            //    else
                            //    {
                            //        e.Cell.Row.Cells["ALLOCATE_TYPE"].Value = Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture);
                            //    }
 
                            //    break;
                            case "Filter":

                                if (Convert.ToInt32(e.Cell.Value) == Include.NoRID)
                                {
                                    e.Cell.Row.Cells["FILTER_RID"].Value = System.DBNull.Value;
                                }
                                else
                                {
                                    e.Cell.Row.Cells["FILTER_RID"].Value = Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture);
                                }

                                break;
                            //End TT#1313-MD -jsobek -Header Filters
						}
					}
					catch (Exception exc)
					{
						string message = exc.ToString();
						throw;
					}
					finally
					{
						_bypassAfterUpdate = false;
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgAllocate_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			ChangePending = true;
		}

		private void ulgAllocate_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			try
			{
				ulgAllocate.BeginUpdate();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgAllocate_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				if (e.Row.Band.Index == 0)
				{
					e.Row.Cells["DisplaySequence"].Value = _nextAllocateSequence;
					e.Row.Cells["TASKLIST_RID"].Value = _taskListProf.Key;
					e.Row.Cells["TASK_SEQUENCE"].Value = ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value;
					e.Row.Cells["ALLOCATE_SEQUENCE"].Value = _nextAllocateSequence;
					//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
					//e.Row.Cells["Merchandise"].Value = "";
					e.Row.Cells["AllocateMerchandise"].Value = "";
					//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
                    //Begin TT#1313-MD -jsobek -Header Filters
                    //e.Row.Cells["Type"].Value = Include.NoRID;
                    //e.Row.Cells["Header"].Value = "";
                    //e.Row.Cells["PO"].Value = "";
                    e.Row.Cells["Filter"].Value = Include.NoRID;
                    //End TT#1313-MD -jsobek -Header Filters
					_nextAllocateSequence++;
				}
				else
				{
					e.Row.Cells["DisplaySequence"].Value = _nextAllocateDetailSequence;
					e.Row.Cells["TASKLIST_RID"].Value = e.Row.ParentRow.Cells["TASKLIST_RID"].Value;
					e.Row.Cells["TASK_SEQUENCE"].Value = e.Row.ParentRow.Cells["TASK_SEQUENCE"].Value;
					e.Row.Cells["ALLOCATE_SEQUENCE"].Value = e.Row.ParentRow.Cells["ALLOCATE_SEQUENCE"].Value;
					e.Row.Cells["DETAIL_SEQUENCE"].Value = _nextAllocateDetailSequence;

					_nextAllocateDetailSequence++;
				}

				e.Row.Activate();
				e.Row.Band.SortedColumns.RefreshSort(true);
				ulgAllocate.UpdateData();
				AllocateAcceptChanges();
				ulgAllocate.EndUpdate();

				ChangePending = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgAllocate_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (e.Cell.Column.Key == "Execute Date Range")
				{
					ShowDateSelector(ulgAllocate, e.Cell, "EXECUTE_CDR_RID");
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgAllocate_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
			UltraGridChildBand childBand;
			ArrayList deleteRows;
			int i;

			try
			{
				if (!_bypassBeforeDelete)
				{
					_bypassBeforeDelete = true;

					try
					{
						if (ulgAllocate.Selected.Rows.Count > 0 && ulgAllocate.Selected.Rows[0].Band.Index == 0)
						{
							deleteRows = new ArrayList();

							childBand = ulgAllocate.Selected.Rows[0].ChildBands[0];

							for (i = 0; i < childBand.Rows.Count; i++)
							{
								deleteRows.Add(childBand.Rows[i]);
							}

							foreach (UltraGridRow row in deleteRows)
							{
								row.Delete(false);
							}
						}

						ulgAllocate.Selected.Rows[0].Delete(false);
						ulgAllocate.Selected.Rows.Clear();
						ulgAllocate.UpdateData();
						AllocateAcceptChanges();

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

		private void ulgRollup_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			string cellValue;
			HierarchyNodeProfile hnp;

			try
			{
				if (!_bypassAfterUpdate)
				{
					_bypassAfterUpdate = true;

					try
					{
						switch (e.Cell.Column.Key)
						{
							case "HN_RID":

								//Begin Track #6479 - JScott - Allocate process - only color displays in merchandise field - need style too
								//hnp = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(e.Cell.Value));
								hnp = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(e.Cell.Value), false, true);
								//End Track #6479 - JScott - Allocate process - only color displays in merchandise field - need style too

								if (hnp.Key != Include.NoRID)
								{
                                    //Begin TT#69 - JSmith - Cannot drag-drop a merchandise node in to an Allocate Task List.  I can type one in.
                                    ////Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
                                    ////e.Cell.Row.Cells["Merchandise"].Value = hnp.Text;
                                    //e.Cell.Row.Cells["RollupMerchandise"].Value = hnp.Text;
                                    ////End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
                                    if (SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(hnp.Key, (int)eSecurityTypes.Store).AllowUpdate ||
                                        SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(hnp.Key, (int)eSecurityTypes.Chain).AllowUpdate)
                                    {
                                        e.Cell.Row.Cells["RollupMerchandise"].Value = hnp.Text;
                                    }
                                    else
                                    {
                                        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        e.Cell.Row.Cells["RollupMerchandise"].Value = System.DBNull.Value;
                                    }
                                    //End TT#69
								}
								else
								{
									MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

									//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
									//e.Cell.Row.Cells["Merchandise"].Value = System.DBNull.Value;
									e.Cell.Row.Cells["RollupMerchandise"].Value = System.DBNull.Value;
									//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
								}

								e.Cell.Row.Cells["FROM_PH_OFFSET_IND"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["FROM_PH_RID"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["FROM_PHL_SEQUENCE"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["FROM_OFFSET"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["TO_PH_OFFSET_IND"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["TO_PH_RID"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["TO_PHL_SEQUENCE"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["TO_OFFSET"].Value = System.DBNull.Value;

								FillLevelValueLists(e.Cell.Row);

								break;

							//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
							//case "Merchandise":
							case "RollupMerchandise":
							//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.

								cellValue = e.Cell.Value.ToString().Trim();
 
								if (cellValue.Length > 0)
								{
//									hnp = _SAB.HierarchyServerSession.GetNodeData(cellValue.Split(new char[] { '[' })[0].Trim());
									hnp =  GetNodeProfile(cellValue);

									if (hnp.Key != Include.NoRID)
									{
										e.Cell.Row.Cells["HN_RID"].Value = hnp.Key;
										//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
										//e.Cell.Row.Cells["Merchandise"].Value = hnp.Text;
										e.Cell.Row.Cells["RollupMerchandise"].Value = hnp.Text;
										//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
									}
									else
									{
										MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

										e.Cell.Row.Cells["HN_RID"].Value = System.DBNull.Value;
										//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
										//e.Cell.Row.Cells["Merchandise"].Value = System.DBNull.Value;
										e.Cell.Row.Cells["RollupMerchandise"].Value = System.DBNull.Value;
										//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
									}
								}
								else
								{
									e.Cell.Row.Cells["HN_RID"].Value = System.DBNull.Value;
									//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
									//e.Cell.Row.Cells["Merchandise"].Value = System.DBNull.Value;
									e.Cell.Row.Cells["RollupMerchandise"].Value = System.DBNull.Value;
									//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
								}

								e.Cell.Row.Cells["FROM_PH_OFFSET_IND"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["FROM_PH_RID"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["FROM_PHL_SEQUENCE"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["FROM_OFFSET"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["TO_PH_OFFSET_IND"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["TO_PH_RID"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["TO_PHL_SEQUENCE"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["TO_OFFSET"].Value = System.DBNull.Value;

								FillLevelValueLists(e.Cell.Row);

								break;
							//BEGIN TT#4780 - DOConnell - Rollup Task List options
                            case "From Level":

                                if (this.RollUpLoaded)
                                {
                                    int i;
                                    HierarchyLevelValueItem valItem;
                                    Infragistics.Win.ValueList toValList = new Infragistics.Win.ValueList();
                                    int? selFromOffset = e.Cell.Row.Cells["From Level"].ValueList.SelectedItemIndex as int?;
                                    int? selToOffset = e.Cell.Row.Cells["To Level"].Value as int?;

                                        //Add the items to the toValList
                                        toValList.ValueListItems.Clear();
                                        for (i = 0; i <= selFromOffset; i++)
                                        {
                                            valItem = (HierarchyLevelValueItem)_toLevelList[i];
                                            toValList.ValueListItems.Add(i, valItem.LevelName);
                                        }
                                    
                                    e.Cell.Row.Cells["To Level"].ValueList = toValList;

                                    if (e.Cell.Row.Cells["TO_PHL_SEQUENCE"].Value == System.DBNull.Value &&
                                            e.Cell.Row.Cells["TO_OFFSET"].Value == System.DBNull.Value)
                                    {
                                        e.Cell.Row.Cells["To Level"].Value = System.DBNull.Value;
                                    }
                                    else
                                    {
                                        if (selToOffset <= selFromOffset)
                                        {
                                            for (i = 0; i < _toLevelList.Count; i++)
                                            {
                                                valItem = (HierarchyLevelValueItem)_toLevelList[i];
                                                if (e.Cell.Row.Cells["TO_PH_OFFSET_IND"].Value == System.DBNull.Value &&
                                                    (valItem.LevelType == eHierarchyDescendantType.levelType && valItem.HierarchyRID == Convert.ToInt32(e.Cell.Row.Cells["TO_PH_RID"].Value) && valItem.LevelRID == Convert.ToInt32(e.Cell.Row.Cells["TO_PHL_SEQUENCE"].Value)) ||

                                                    (e.Cell.Row.Cells["TO_PH_OFFSET_IND"].Value != System.DBNull.Value && valItem.LevelType == (eHierarchyDescendantType)Convert.ToInt32(e.Cell.Row.Cells["TO_PH_OFFSET_IND"].Value) &&
                                                    ((valItem.LevelType == eHierarchyDescendantType.levelType && valItem.HierarchyRID == Convert.ToInt32(e.Cell.Row.Cells["TO_PH_RID"].Value) && valItem.LevelRID == Convert.ToInt32(e.Cell.Row.Cells["TO_PHL_SEQUENCE"].Value)) ||
                                                    (valItem.LevelType == eHierarchyDescendantType.offset && valItem.Offset == Convert.ToInt32(e.Cell.Row.Cells["TO_OFFSET"].Value)))))
                                                {
                                                    e.Cell.Row.Cells["To Level"].Value = i;
                                                    break;
                                                }
                                            }
                                            if (i == _toLevelList.Count)
                                            {
                                                e.Cell.Row.Cells["To Level"].Value = System.DBNull.Value;
                                            }
                                        }
                                        else
                                        {
                                            e.Cell.Row.Cells["To Level"].Value = System.DBNull.Value;
                                        }
                                    }
                                }
                                break;
                            //BEGIN TT#4791 - DOConnell - Rollup Tasklist - able to save tasklist when a 'To Level' is not selected
                            case "To Level":

                                if (this.RollUpLoaded)
                                {
                                    e.Cell.Row.Cells["To Level"].Value = e.Cell.Row.Cells["To Level"].ValueList.SelectedItemIndex;
                                }
                                break;
							//END TT#4791 - DOConnell - Rollup Tasklist - able to save tasklist when a 'To Level' is not selected
						}
						//END TT#4780 - DOConnell - Rollup Task List options
					}
					catch (Exception exc)
					{
						string message = exc.ToString();
						throw;
					}
					finally
					{
						e.Cell.Column.PerformAutoResize(PerformAutoSizeType.VisibleRows);

                        if (RollUpLoaded) ChangePending = true;  //TT#2744 - MD - Rollup task in 5.0 asking to save when nothing has chenged - RBeck

						_bypassAfterUpdate = false;
					}
				}

			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgRollup_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			try
			{
				ulgRollup.BeginUpdate();

			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgRollup_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				e.Row.Cells["DisplaySequence"].Value = _nextRollupSequence;
				e.Row.Cells["TASKLIST_RID"].Value = _taskListProf.Key;
				e.Row.Cells["TASK_SEQUENCE"].Value = ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value;
				e.Row.Cells["ROLLUP_SEQUENCE"].Value = _nextRollupSequence;
				e.Row.Cells["Posting"].Value = false;
				e.Row.Cells["Reclass"].Value = false;
				e.Row.Cells["Hierarchy Levels"].Value = false;
				e.Row.Cells["Day To Week"].Value = false;
				e.Row.Cells["Day"].Value = false;
				e.Row.Cells["Week"].Value = false;
				e.Row.Cells["Store"].Value = false;
				e.Row.Cells["Chain"].Value = false;
				e.Row.Cells["Store To Chain"].Value = false;
				e.Row.Cells["Intransit"].Value = false;

				_nextRollupSequence++;

				e.Row.Activate();
				e.Row.Band.SortedColumns.RefreshSort(true);
				ulgRollup.UpdateData();
				RollupAcceptChanges();
				ulgRollup.EndUpdate();

				ChangePending = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgRollup_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (e.Cell.Column.Key == "Rollup Date Range")
				{
					ShowDateSelector(ulgRollup, e.Cell, "ROLLUP_CDR_RID");
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgRollup_AfterCellListCloseUp(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			HierarchyLevelValueItem valItem;
			VersionProfile verProf;

			try
			{
				if (ulgRollup.ActiveCell != null)
				{
					if (e.Cell.Column.Key == "FV_RID")
					{
						ulgRollup.PerformAction(UltraGridAction.ExitEditMode);

						if (e.Cell.Value != System.DBNull.Value)
						{
							if (Convert.ToInt32(e.Cell.Value) == Include.FV_ActualRID)
							{
								e.Cell.Row.Cells["Day To Week"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
								e.Cell.Row.Cells["Day"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
								e.Cell.Row.Cells["Intransit"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
							}
							else
							{
								e.Cell.Row.Cells["Day To Week"].Value = false;
								e.Cell.Row.Cells["Day To Week"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
								e.Cell.Row.Cells["Day"].Value = false;
								e.Cell.Row.Cells["Day"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
								e.Cell.Row.Cells["Intransit"].Value = false;
								e.Cell.Row.Cells["Intransit"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
							}

							verProf = (VersionProfile)_versionProfList.FindKey(Convert.ToInt32(e.Cell.Value));

							if (verProf != null)
							{
								if (verProf.ChainSecurity.AllowUpdate)
								{
									e.Cell.Row.Cells["Chain"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
								}
								else
								{
									e.Cell.Row.Cells["Chain"].Value = false;
									e.Cell.Row.Cells["Chain"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
								}

								if (verProf.StoreSecurity.AllowUpdate)
								{
									e.Cell.Row.Cells["Store"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
								}
								else
								{
									e.Cell.Row.Cells["Store"].Value = false;
									e.Cell.Row.Cells["Store"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
								}
							}
							else
							{
								e.Cell.Row.Cells["Chain"].Value = false;
								e.Cell.Row.Cells["Chain"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
								e.Cell.Row.Cells["Store"].Value = false;
								e.Cell.Row.Cells["Store"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
							}
						}
					}
					else if (e.Cell.Column.Key == "From Level")
					{
						ulgRollup.PerformAction(UltraGridAction.ExitEditMode);

						if (e.Cell.Value == System.DBNull.Value)
						{
							e.Cell.Row.Cells["FROM_PH_OFFSET_IND"].Value = System.DBNull.Value;
							e.Cell.Row.Cells["FROM_PH_RID"].Value = System.DBNull.Value;
							e.Cell.Row.Cells["FROM_PHL_SEQUENCE"].Value = System.DBNull.Value;
							e.Cell.Row.Cells["FROM_OFFSET"].Value = System.DBNull.Value;
						}
						else
						{
							valItem = (HierarchyLevelValueItem)_fromLevelList[Convert.ToInt32(e.Cell.Value)];
							e.Cell.Row.Cells["FROM_PH_OFFSET_IND"].Value = valItem.LevelType;

							if (valItem.LevelType == eHierarchyDescendantType.levelType)
							{
								e.Cell.Row.Cells["FROM_PH_RID"].Value = valItem.HierarchyRID;
								e.Cell.Row.Cells["FROM_PHL_SEQUENCE"].Value = valItem.LevelRID;
								e.Cell.Row.Cells["FROM_OFFSET"].Value = System.DBNull.Value;
							}
							else
							{
								e.Cell.Row.Cells["FROM_PH_RID"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["FROM_PHL_SEQUENCE"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["FROM_OFFSET"].Value = valItem.Offset;
							}
						}
					}
					else if (e.Cell.Column.Key == "To Level")
					{
						ulgRollup.PerformAction(UltraGridAction.ExitEditMode);

						if (e.Cell.Value == System.DBNull.Value)
						{
							e.Cell.Row.Cells["TO_PH_OFFSET_IND"].Value = System.DBNull.Value;
							e.Cell.Row.Cells["TO_PH_RID"].Value = System.DBNull.Value;
							e.Cell.Row.Cells["TO_PHL_SEQUENCE"].Value = System.DBNull.Value;
							e.Cell.Row.Cells["TO_OFFSET"].Value = System.DBNull.Value;
						}
						else
						{
							valItem = (HierarchyLevelValueItem)_toLevelList[Convert.ToInt32(e.Cell.Value)];
							e.Cell.Row.Cells["TO_PH_OFFSET_IND"].Value = valItem.LevelType;
						
							if (valItem.LevelType == eHierarchyDescendantType.levelType)
							{
								e.Cell.Row.Cells["TO_PH_RID"].Value = valItem.HierarchyRID;
								e.Cell.Row.Cells["TO_PHL_SEQUENCE"].Value = valItem.LevelRID;
								e.Cell.Row.Cells["TO_OFFSET"].Value = System.DBNull.Value;
							}
							else
							{
								e.Cell.Row.Cells["TO_PH_RID"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["TO_PHL_SEQUENCE"].Value = System.DBNull.Value;
								e.Cell.Row.Cells["TO_OFFSET"].Value = valItem.Offset;
							}
						}
					}
				}

				e.Cell.Column.PerformAutoResize(PerformAutoSizeType.VisibleRows);

				ChangePending = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgRollup_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
			try
			{
				if (!_bypassBeforeDelete)
				{
					_bypassBeforeDelete = true;

					try
					{
						ulgRollup.Selected.Rows[0].Delete(false);
						ulgRollup.Selected.Rows.Clear();
						ulgRollup.UpdateData();
						RollupAcceptChanges();

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

		//Begin TT#155 - JScott - Size Curve Method
		private void ulgSizeCurveMethod_AfterCellUpdate(object sender, CellEventArgs e)
		{
			ChangePending = true;
		}

		private void ulgSizeCurveMethod_CellChange(object sender, CellEventArgs e)
		{
			ChangePending = true;
		}

		private void ulgSizeCurveMethod_BeforeRowInsert(object sender, BeforeRowInsertEventArgs e)
		{
			try
			{
				ulgSizeCurveMethod.BeginUpdate();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgSizeCurveMethod_AfterRowInsert(object sender, RowEventArgs e)
		{
			try
			{
				e.Row.Cells["DisplaySequence"].Value = _nextSizeCurveMethodSequence;
				e.Row.Cells["TASKLIST_RID"].Value = _taskListProf.Key;
				e.Row.Cells["TASK_SEQUENCE"].Value = ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value;
				e.Row.Cells["GENERATE_SEQUENCE"].Value = _nextSizeCurveMethodSequence;

				_nextSizeCurveMethodSequence++;

				e.Row.Activate();
				e.Row.Band.SortedColumns.RefreshSort(true);
				ulgSizeCurveMethod.UpdateData();
				SizeCurveMethodAcceptChanges();
				ulgSizeCurveMethod.EndUpdate();

				ChangePending = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgSizeCurveMethod_ClickCellButton(object sender, CellEventArgs e)
		{
			try
			{
				if (e.Cell.Column.Key == "Execute Date Range")
				{
					ShowDateSelector(ulgSizeCurveMethod, e.Cell, "EXECUTE_CDR_RID");
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgSizeCurveMethod_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
		{
			try
			{
				if (!_bypassBeforeDelete)
				{
					_bypassBeforeDelete = true;

					try
					{
						if (ulgSizeCurveMethod.Selected.Rows.Count > 0)
						{
							ulgSizeCurveMethod.Selected.Rows[0].Delete(false);
							ulgSizeCurveMethod.Selected.Rows.Clear();
							ulgSizeCurveMethod.UpdateData();
							SizeCurveMethodAcceptChanges();

							ChangePending = true;
						}

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

		//End TT#155 - JScott - Size Curve Method
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		private void ulgSizeCurves_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			string cellValue;
			HierarchyNodeProfile hnp;

			try
			{
				if (!_bypassAfterUpdate)
				{
					_bypassAfterUpdate = true;

					try
					{
						switch (e.Cell.Column.Key)
						{
							case "HN_RID":

								hnp = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(e.Cell.Value), false, true);

								if (hnp.Key != Include.NoRID)
								{
									if (SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(hnp.Key, (int)eSecurityTypes.Store).AllowUpdate)
									{
										e.Cell.Row.Cells["SizeCurvesMerchandise"].Value = hnp.Text;
									}
									else
									{
										MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
									}
								}
								else
								{
									MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								}

								break;

							case "SizeCurvesMerchandise":

								cellValue = e.Cell.Value.ToString().Trim();

								if (cellValue.Length > 0)
								{
									hnp = GetNodeProfile(cellValue);

									if (hnp.Key != Include.NoRID)
									{
										e.Cell.Row.Cells["HN_RID"].Value = hnp.Key;
										e.Cell.Row.Cells["SizeCurvesMerchandise"].Value = hnp.Text;
									}
									else
									{
										MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

										e.Cell.Row.Cells["HN_RID"].Value = System.DBNull.Value;
									}
								}
								else
								{
									e.Cell.Row.Cells["HN_RID"].Value = System.DBNull.Value;
								}

								break;
						}
					}
					catch (Exception exc)
					{
						string message = exc.ToString();
						throw;
					}
					finally
					{
						_bypassAfterUpdate = false;
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgSizeCurves_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			ChangePending = true;
		}

		private void ulgSizeCurves_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			try
			{
				ulgSizeCurves.BeginUpdate();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgSizeCurves_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				e.Row.Cells["DisplaySequence"].Value = _nextSizeCurvesSequence;
				e.Row.Cells["TASKLIST_RID"].Value = _taskListProf.Key;
				e.Row.Cells["TASK_SEQUENCE"].Value = ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value;
				e.Row.Cells["GENERATE_SEQUENCE"].Value = _nextSizeCurvesSequence;
				e.Row.Cells["SizeCurvesMerchandise"].Value = "";

				_nextSizeCurvesSequence++;

				e.Row.Activate();
				e.Row.Band.SortedColumns.RefreshSort(true);
				ulgSizeCurves.UpdateData();
				SizeCurvesAcceptChanges();
				ulgSizeCurves.EndUpdate();

				ChangePending = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ulgSizeCurves_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
			UltraGridChildBand childBand;
			ArrayList deleteRows;
			int i;

			try
			{
				if (!_bypassBeforeDelete)
				{
					_bypassBeforeDelete = true;

					try
					{
						if (ulgSizeCurves.Selected.Rows.Count > 0)
						{
							ulgSizeCurves.Selected.Rows[0].Delete(false);
							ulgSizeCurves.Selected.Rows.Clear();
							ulgSizeCurves.UpdateData();
							SizeCurvesAcceptChanges();

							ChangePending = true;
						}

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

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		//Begin TT#707 - JScott - Size Curve process needs to multi-thread
		private void nudSizeCurveConcurrentProcesses_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (nudSizeCurveConcurrentProcesses.Parent.Visible)
				{
					_currentSizeCurveNodeRow["CONCURRENT_PROCESSES"] = nudSizeCurveConcurrentProcesses.Value;

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//End TT#707 - JScott - Size Curve process needs to multi-thread
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
			UltraGridCell dropCell;
            TreeNodeClipboardList cbList = null;
            TreeNodeClipboardProfile cbProf = null;
            MIDWorkflowMethodTreeNode node;

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
                        //if (cbp.ClipboardDataType == eClipboardDataType.Method ||
                        //    cbp.ClipboardDataType == eClipboardDataType.Workflow)
                        // Begin Track #6277 - JSmith - Cannot drag/drop a OTS Forecast Method in to a Task List for Forecasting
                        //if (cbList.ClipboardDataType == eProfileType.Method ||
                        //    cbList.ClipboardDataType == eProfileType.Workflow)
                        if (Enum.IsDefined(typeof(eMethodProfileType), Convert.ToInt32(cbList.ClipboardDataType)) ||
                            Enum.IsDefined(typeof(eWorkflowProfileType), Convert.ToInt32(cbList.ClipboardDataType)))
                        // End Track #6277
                        {
                            dropCell = (UltraGridCell)element.GetContext(typeof(UltraGridCell));

                            if (dropCell != null)
                            {
                                if (dropCell.Column.Key == "ForecastWorkflowMethod")
                                {
                                    //methodClipboardData = (MethodClipboardData)cbp.ClipboardData;
                                    node = (MIDWorkflowMethodTreeNode)cbList.ClipboardProfile.Node;
                                    if ((node.WorkflowMethodIND == eWorkflowMethodIND.Methods))
                                    {
                                        if (Enum.IsDefined(typeof(eForecastMethodType), (eForecastMethodType)node.MethodType))
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

                                    if (node.WorkflowMethodIND == eWorkflowMethodIND.Workflows &&
                                        node.WorkflowType == eWorkflowType.Forecast)
                                    {
                                        e.Effect = e.AllowedEffect;
                                        return;
                                    }
                                    else
                                    {
                                        e.Effect = DragDropEffects.None;
                                        return;
                                    }
                                    //if ((node.WorkflowMethodIND == eWorkflowMethodIND.Methods &&
                                    //    ((eMethodType)node.MethodType == eMethodType.OTSPlan ||
                                    //    (eMethodType)node.MethodType == eMethodType.ForecastBalance ||
                                    //    (eMethodType)node.MethodType == eMethodType.ForecastSpread ||
                                    //    (eMethodType)node.MethodType == eMethodType.CopyChainForecast ||
                                    //    (eMethodType)node.MethodType == eMethodType.Export ||
                                    //    (eMethodType)node.MethodType == eMethodType.ForecastModifySales ||
                                    //    (eMethodType)node.MethodType == eMethodType.CopyStoreForecast ||
                                    //    //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                                    //    (eMethodType)node.MethodType == eMethodType.GlobalUnlock ||
                                    //    (eMethodType)node.MethodType == eMethodType.GlobalLock)) ||
                                    //    //END TT#43 - MD - DOConnell - Projected Sales Enhancement
                                    //    (node.WorkflowMethodIND == eWorkflowMethodIND.Workflows &&
                                    //    node.WorkflowType == eWorkflowType.Forecast))
                                    //{
                                    //    e.Effect = e.AllowedEffect;
                                    //    return;
                                    //}
                                    //else
                                    //{
                                    //    e.Effect = DragDropEffects.None;
                                    //    return;
                                    //}
                                }
                                else if (dropCell.Column.Key == "AllocateWorkflowMethod")
                                {
                                   //methodClipboardData = (MethodClipboardData)cbp.ClipboardData;
                                   node = (MIDWorkflowMethodTreeNode)cbList.ClipboardProfile.Node;

                                   if (node.WorkflowMethodIND == eWorkflowMethodIND.Workflows &&
                                        node.WorkflowType == eWorkflowType.Allocation)
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
								//Begin TT#155 - JScott - Size Curve Method
								else if (dropCell.Column.Key == "SizeCurveMethod")
                                {
                                   node = (MIDWorkflowMethodTreeNode)cbList.ClipboardProfile.Node;

                                   if (node.WorkflowMethodIND == eWorkflowMethodIND.SizeMethods &&
										(eMethodType)node.MethodType == eMethodType.SizeCurve)
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
								//End TT#155 - JScott - Size Curve Method
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
                        else if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                        {
                            dropCell = (UltraGridCell)element.GetContext(typeof(UltraGridCell));

							//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
							//if (dropCell != null && dropCell.Column.Key == "Merchandise")
							//{
							//    if (cbList.ClipboardProfile.FunctionSecurityProfile.AllowUpdate)
							//    {
							//        e.Effect = e.AllowedEffect;
							//    }
							//    else
							//    {
							//        e.Effect = DragDropEffects.None;
							//    }

							//    return;
							//}
							//else
							//{
							//    e.Effect = DragDropEffects.None;
							//    return;
							//}
							if (dropCell != null && dropCell.Column.Key == "ForecastMerchandise")
							{
								if (SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(cbList.ClipboardProfile.Node.Profile.Key, (int)eSecurityTypes.Store).AllowUpdate)
								{
									e.Effect = e.AllowedEffect;
								}
								else
								{
									e.Effect = DragDropEffects.None;
								}

								return;
							}
							else if (dropCell != null && dropCell.Column.Key == "AllocateMerchandise")
							{
                                // Begin TT#827 - JSmith - Allocation Performance
                                //if (SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(cbList.ClipboardProfile.Node.Profile.Key, (int)eSecurityTypes.Allocation).AllowUpdate)
                                if (SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(cbList.ClipboardProfile.Node.Profile.Key, (int)eSecurityTypes.Allocation).AllowUpdate &&
                                    ((HierarchyNodeProfile)cbList.ClipboardProfile.Node.Profile).LevelType != eHierarchyLevelType.Size)
                                // End TT#827 - JSmith - Allocation Performance
								{
									e.Effect = e.AllowedEffect;
								}
								else
								{
									e.Effect = DragDropEffects.None;
								}

								return;
							}
							else if (dropCell != null && dropCell.Column.Key == "RollupMerchandise")
							{
								if (SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(cbList.ClipboardProfile.Node.Profile.Key, (int)eSecurityTypes.Store).AllowUpdate ||
									SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(cbList.ClipboardProfile.Node.Profile.Key, (int)eSecurityTypes.Chain).AllowUpdate)
								{
									e.Effect = e.AllowedEffect;
								}
								else
								{
									e.Effect = DragDropEffects.None;
								}

								return;
							}
							//Begin TT#155 - JScott - Add Size Curve info to Node Properties
							else if (dropCell != null && dropCell.Column.Key == "SizeCurvesMerchandise")
							{
								if (SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(cbList.ClipboardProfile.Node.Profile.Key, (int)eSecurityTypes.Store).AllowUpdate ||
									SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(cbList.ClipboardProfile.Node.Profile.Key, (int)eSecurityTypes.Chain).AllowUpdate)
								{
									e.Effect = e.AllowedEffect;
								}
								else
								{
									e.Effect = DragDropEffects.None;
								}

								return;
							}
							//End TT#155 - JScott - Add Size Curve info to Node Properties
							else
							{
								e.Effect = DragDropEffects.None;
								return;
							}
							//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
						}
                        else
                        {
                            e.Effect = DragDropEffects.None;
                            return;
                        }
					}
					else if (e.Data.GetDataPresent(typeof(SelectedHeaderProfile)))
					{
						dropCell = (UltraGridCell)element.GetContext(typeof(UltraGridCell)); 
						
						if (dropCell != null && dropCell.Column.Key == "HEADER_ID")
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
				}
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
			UltraGridCell dropCell;
			int dragSeq;
			int dropSeq;
			RowsCollection bandRows;
            //MIDHierarchyNode treeNode;
			SelectedHeaderProfile selHdrProf;
            TreeNodeClipboardList cbList = null;
            TreeNodeClipboardProfile cbProf = null;
            MIDWorkflowMethodTreeNode node;

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
								if (selectedRow.Band.ParentBand != null)
								{
									bandRows = selectedRow.ParentRow.ChildBands[0].Rows;
								}
								else
								{
									bandRows = grid.Rows;
								}

								if (dragSeq > dropSeq)
								{
									foreach (UltraGridRow row in bandRows)
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
									foreach (UltraGridRow row in bandRows)
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
                        //if (cbp.ClipboardDataType == eClipboardDataType.Method ||
                        //    cbp.ClipboardDataType == eClipboardDataType.Workflow)
                        // Begin Track #6277 - JSmith - Cannot drag/drop a OTS Forecast Method in to a Task List for Forecasting
                        //if (cbList.ClipboardDataType == eProfileType.Method ||
                        //    cbList.ClipboardDataType == eProfileType.Workflow)
                        if (Enum.IsDefined(typeof(eMethodProfileType), Convert.ToInt32(cbList.ClipboardDataType)) ||
                            Enum.IsDefined(typeof(eWorkflowProfileType), Convert.ToInt32(cbList.ClipboardDataType)))
                        // End Track #6277
                        {
                            dropCell = (UltraGridCell)element.GetContext(typeof(UltraGridCell));

                            if (dropCell != null)
                            {
                                //methodClipboardData = (MethodClipboardData)cbp.ClipboardData;
                                node = (MIDWorkflowMethodTreeNode)cbList.ClipboardProfile.Node;
								//Begin TT#155 - JScott - Size Curve Method
								//if (node.WorkflowMethodIND == eWorkflowMethodIND.Methods)
								if (dropCell.Column.Key == "SizeCurveMethod" && node.WorkflowMethodIND == eWorkflowMethodIND.SizeMethods)
								{
									dropRow.Cells["METHOD_RID"].Value = cbList.ClipboardProfile.Key;
									dropCell.Value = cbList.ClipboardProfile.Text;
								}
								else if (node.WorkflowMethodIND == eWorkflowMethodIND.Methods)
								//End TT#155 - JScott - Size Curve Method
								{
									dropRow.Cells["METHOD_RID"].Value = cbList.ClipboardProfile.Key;
									dropRow.Cells["WORKFLOW_RID"].Value = System.DBNull.Value;
									dropRow.Cells["WORKFLOW_METHOD_IND"].Value = (int)eWorkflowMethodType.Method;
									dropRow.Cells["WorkflowMethodType"].Value = MIDText.GetTextOnly(Convert.ToInt32(_dlWorkflowMethod.GetMethodType(cbList.ClipboardProfile.Key)));
                                    //dropCell.Value =  cbList.ClipboardProfile.Text; 
                                    dropCell.Value = AddUserName(cbList.ClipboardProfile.Node.UserId, cbList.ClipboardProfile.Text);    //TT#283 - MD - User name not designated in Task List - RBeck

								}
								else
								{
									dropRow.Cells["WORKFLOW_RID"].Value = cbList.ClipboardProfile.Key;
									dropRow.Cells["METHOD_RID"].Value = System.DBNull.Value;
									dropRow.Cells["WORKFLOW_METHOD_IND"].Value = (int)eWorkflowMethodType.Workflow;
									dropRow.Cells["WorkflowMethodType"].Value = MIDText.GetTextOnly(Convert.ToInt32(eWorkflowMethodType.Workflow));
                                    //dropCell.Value =  cbList.ClipboardProfile.Text; 
                                    dropCell.Value = AddUserName(cbList.ClipboardProfile.Node.UserId, cbList.ClipboardProfile.Text);    //TT#283 - MD - User name not designated in Task List - RBeck

								}

                                grid.UpdateData();

                                ChangePending = true;
                            }
                        }
                        //else if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
                        else if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                        {
                            dropCell = (UltraGridCell)element.GetContext(typeof(UltraGridCell));

                            if (dropCell != null)
                            {
                                //treeNode = (MIDHierarchyNode)e.Data.GetData(typeof(MIDHierarchyNode));
                                dropRow.Cells["HN_RID"].Value = cbList.ClipboardProfile.Key;

                                grid.UpdateData();

                                ChangePending = true;
                            }
                        }
                    }
                    else if (e.Data.GetDataPresent(typeof(SelectedHeaderProfile)))
                    {
                        dropCell = (UltraGridCell)element.GetContext(typeof(UltraGridCell));

                        if (dropCell != null)
                        {
                            selHdrProf = (SelectedHeaderProfile)e.Data.GetData(typeof(SelectedHeaderProfile));
                            dropCell.Value = selHdrProf.HeaderID;

                            grid.UpdateData();

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

//Begin  TT#283 - MD - User name not designated in Task List - RBeck
        private string AddUserName(int UserId, string wfName)
        {
            WorkflowBaseData wb = new WorkflowBaseData();
            string aUserName = wb.GetWorkflowUserName(UserId);

            if (aUserName != "global") wfName += " [" + aUserName + "]";

            string wfDisplayName = wfName;

            return wfDisplayName;
        }
//End    TT#283 - MD - User name not designated in Task List - RBeck

		private void mniTaskDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				ulgTasks.Selected.Rows[0].Delete(false);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void mniForecastDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				ulgForecast.Selected.Rows[0].Delete(false);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void mniAllocateDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				ulgAllocate.Selected.Rows[0].Delete(false);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void mniRollupDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				ulgRollup.Selected.Rows[0].Delete(false);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//Begin TT#155 - JScott - Size Curve Method
		private void mniSizeCurveMethodDelete_Click(object sender, EventArgs e)
		{
			try
			{
				ulgSizeCurveMethod.Selected.Rows[0].Delete(false);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//End TT#155 - JScott - Size Curve Method
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		private void mniSizeCurvesDelete_Click(object sender, EventArgs e)
		{
			try
			{
				ulgSizeCurves.Selected.Rows[0].Delete(false);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		private void mniAddTask_Click(object sender, System.EventArgs e)
		{
			MenuItem menuItem;

			try
			{
				_afterAddMenu = true;
				menuItem = (MenuItem)sender;
				_addedType = ((AddMenuEntry)_addMenuList[menuItem.Index]).TaskType;
				ulgTasks.DisplayLayout.Bands[0].AddNew();
				_afterAddMenu = false;
				//BEGIN TT#4000 - DOConnell - Task List Explorer - Read only system Hierarchy Load task allows users to edit
                if ((_userSecLvl.AllowUpdate && !_systemSecLvl.AllowUpdate))
                {
                    rdoUser.Checked = true;
                    _taskListProf.Name = "";
                    txtTaskListName.Text = _taskListProf.Name;
                }
				//END TT#4000 - DOConnell - Task List Explorer - Read only system Hierarchy Load task allows users to edit
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ctmTaskMenu_Popup(object sender, System.EventArgs e)
		{
			try
			{
				if (FunctionSecurity.AllowUpdate)
				{
					if (ulgTasks.Selected.Rows.Count > 0)
					{
						mniTaskDelete.Enabled = true;
					}
					else
					{
						mniTaskDelete.Enabled = false;
					}
				}
				else
				{
					mniTaskDelete.Enabled = false;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ctmForecastMenu_Popup(object sender, System.EventArgs e)
		{
			try
			{
				if (FunctionSecurity.AllowUpdate)
				{
					if (ulgForecast.Selected.Rows.Count > 0)
					{
						mniForecastDelete.Enabled = true;
					}
					else
					{
						mniForecastDelete.Enabled = false;
					}
				}
				else
				{
					mniForecastDelete.Enabled = false;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ctmAllocateMenu_Popup(object sender, System.EventArgs e)
		{
			try
			{
				if (FunctionSecurity.AllowUpdate)
				{
					if (ulgAllocate.Selected.Rows.Count > 0)
					{
						mniAllocateDelete.Enabled = true;
					}
					else
					{
						mniAllocateDelete.Enabled = false;
					}
				}
				else
				{
					mniAllocateDelete.Enabled = false;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void ctmRollupMenu_Popup(object sender, System.EventArgs e)
		{
			try
			{
				if (FunctionSecurity.AllowUpdate)
				{
					if (ulgRollup.Selected.Rows.Count > 0)
					{
						mniRollupDelete.Enabled = true;
					}
					else
					{
						mniRollupDelete.Enabled = false;
					}
				}
				else
				{
					mniRollupDelete.Enabled = false;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//Begin TT#155 - JScott - Size Curve Method
		private void ctmSizeCurveMethodMenu_Popup(object sender, System.EventArgs e)
		{
			try
			{
				if (FunctionSecurity.AllowUpdate)
				{
					if (ulgSizeCurveMethod.Selected.Rows.Count > 0)
					{
						mniSizeCurveMethodDelete.Enabled = true;
					}
					else
					{
						mniSizeCurveMethodDelete.Enabled = false;
					}
				}
				else
				{
					mniSizeCurveMethodDelete.Enabled = false;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//End TT#155 - JScott - Size Curve Method
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		private void ctmSizeCurvesMenu_Popup(object sender, System.EventArgs e)
		{
			try
			{
				if (FunctionSecurity.AllowUpdate)
				{
					if (ulgSizeCurves.Selected.Rows.Count > 0)
					{
						mniSizeCurvesDelete.Enabled = true;
					}
					else
					{
						mniSizeCurvesDelete.Enabled = false;
					}
				}
				else
				{
					mniSizeCurvesDelete.Enabled = false;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		private void txtPostingDirectory_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (txtPostingDirectory.Parent.Visible)
				{
					_currentPostingRow["INPUT_DIRECTORY"] = txtPostingDirectory.Text.Trim();

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void txtPostingMask_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (txtPostingMask.Parent.Visible)
				{
					_currentPostingRow["FILE_MASK"] = txtPostingMask.Text.Trim();

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void txtPostingRunUntilMask_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (txtPostingRunUntilMask.Parent.Visible)
				{
					_currentPostingRow["RUN_UNTIL_FILE_MASK"] = txtPostingRunUntilMask.Text.Trim();

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void txtRelieveDirectory_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (txtRelieveDirectory.Parent.Visible)
				{
					_currentPostingRow["INPUT_DIRECTORY"] = txtRelieveDirectory.Text.Trim();

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void txtRelieveMask_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (txtRelieveMask.Parent.Visible)
				{
					_currentPostingRow["FILE_MASK"] = txtRelieveMask.Text.Trim();

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void txtRelieveRunUntilMask_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (txtRelieveRunUntilMask.Parent.Visible)
				{
					_currentPostingRow["RUN_UNTIL_FILE_MASK"] = txtRelieveRunUntilMask.Text.Trim();

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void txtProgramPath_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (txtProgramPath.Parent.Visible)
				{
					_currentProgramRow["PROGRAM_PATH"] = txtProgramPath.Text.Trim();

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void txtProgramParms_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (txtProgramParms.Parent.Visible)
				{
					_currentProgramRow["PROGRAM_PARMS"] = txtProgramParms.Text.Trim();

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void nudPostingConcurrent_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (nudPostingConcurrent.Parent.Visible)
				{
					_currentPostingRow["CONCURRENT_FILES"] = nudPostingConcurrent.Value;
		
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		// Begin TT#391 - stodd - size day to week summary
		private void midDateRangeSizeDayWeekSum_Click(object sender, System.EventArgs e)
		{
			try
			{
				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[] { SAB });
				midDateRangeSizeDayWeekSum.DateRangeForm = frm;
				frm.DateRangeRID = midDateRangeSizeDayWeekSum.DateRangeRID;
				frm.AllowDynamicToPlan = false;
				frm.AllowDynamicToStoreOpen = false;
				frm.AllowDynamicSwitch = false;
				midDateRangeSizeDayWeekSum.ShowSelector();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void midDateRangeSizeDayWeekSum_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			try
			{
				if (e.SelectedDateRange != null)
				{
					LoadDateRangeSelector(midDateRangeSizeDayWeekSum, e.SelectedDateRange);
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void LoadDateRangeSelector(Controls.MIDDateRangeSelector aMIDDRS, DateRangeProfile aDateRangeProf)
		{
			try
			{
				aMIDDRS.Text = aDateRangeProf.DisplayDate;
				aMIDDRS.DateRangeRID = aDateRangeProf.Key;
				_currentSizeDayToWeekSummaryRow["CDR_RID"] = aDateRangeProf.Key;

				if (aDateRangeProf.DateRangeType == eCalendarRangeType.Dynamic)
				{
					aMIDDRS.SetImage(this.DynamicToCurrentImage);
				}
				else
				{
					aMIDDRS.SetImage(null);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// End TT#391 - stodd - size day to week summary

        private void LoadHeaderTypes()
        {
            string headerType = null;
            int headerIndex = -1;
            DataTable dt = HeaderTypesGetDataTable();
            clbHeaderReconcileHeaderTypes.Items.Clear();
            clbHeaderReconcileHeaderTypes.Items.Add("All");

            foreach (DataRow aRow in dt.Rows)
            {
                headerType = (string)aRow["FIELD_NAME"];
                if (headerType == "Multi-Header")
                {
                    continue;
                }
                headerIndex = Convert.ToInt32(aRow["FIELD_INDEX"]);
                clbHeaderReconcileHeaderTypes.Items.Add(headerType);
            }
        }

		//===========================================
		//===========================================
		//===========================================

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private void EnableSaveButtons()
		{
			try
			{
				btnSave.Enabled = _canUpdateTaskList;
				btnOK.Enabled = _canUpdateTaskList;
				btnSaveAs.Enabled = false;

				if (_taskListProf.Key != Include.NoRID)
				{
					if (txtTaskListName.Text.Trim() != _taskListProf.Name.Trim() &&
						!rdoOwner.Checked)
					{
						btnSaveAs.Enabled = _canUpdateTaskList;
					}

					if ((rdoUser.Checked && rdoUser.Tag == null) ||
						(rdoGlobal.Checked && rdoGlobal.Tag == null) ||
						(rdoSystem.Checked && rdoSystem.Tag == null) ||
						(rdoOwner.Checked && rdoOwner.Tag == null))
					{
						btnSaveAs.Enabled = _canUpdateTaskList;
						btnSave.Enabled = false;
						btnOK.Enabled = false;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private void DeleteSelectedForecastRow()
		{
			UltraGridChildBand childBand;
			ArrayList deleteRows;
			int i;

			try
			{
				if (ulgForecast.Selected.Rows[0].Band.Index == 0)
				{
					deleteRows = new ArrayList();

					childBand = ulgForecast.Selected.Rows[0].ChildBands[0];

					for (i = 0; i < childBand.Rows.Count; i++)
					{
						deleteRows.Add(childBand.Rows[i]);
					}

					foreach (UltraGridRow row in deleteRows)
					{
						row.Delete(false);
					}
				}

				ulgForecast.Selected.Rows[0].Delete(false);
				ulgForecast.Selected.Rows.Clear();
				ulgForecast.UpdateData();
				ForecastAcceptChanges();

				ChangePending = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override protected bool SaveChanges()
		{
			try
			{
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				//if (Save(eUpdateMode.Update))
				//{
				//    ErrorFound = false;
				//}
				//else
				//{
				//    ErrorFound = true;
				//}
				if (btnSave.Enabled)
				{
					ErrorFound = Save(eUpdateMode.Update);
				}
				else
				{
					ErrorFound = Save(eUpdateMode.Create);
				}
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindTaskGridData()
		{
			try
			{
				ulgTasks.DataSource = _dtTask;
				ulgTasks.DisplayLayout.Override.SelectTypeRow = SelectType.SingleAutoDrag;
				ulgTasks.DisplayLayout.AddNewBox.Hidden = false;
				ulgTasks.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
				ulgTasks.DisplayLayout.Bands[0].AddButtonCaption = "Task";

				ulgTasks.DisplayLayout.Bands[0].Columns["Task"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
				ulgTasks.DisplayLayout.Bands[0].Columns["Task"].Header.VisiblePosition = 0;
				ulgTasks.DisplayLayout.Bands[0].Columns["MAX_MESSAGE_LEVEL"].Header.Caption = "Max Return";
				ulgTasks.DisplayLayout.Bands[0].Columns["MAX_MESSAGE_LEVEL"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				ulgTasks.DisplayLayout.Bands[0].Columns["MAX_MESSAGE_LEVEL"].ValueList = ulgTasks.DisplayLayout.ValueLists["MaxMessageLevelDesc"];
				ulgTasks.DisplayLayout.Bands[0].Columns["MAX_MESSAGE_LEVEL"].Header.VisiblePosition = 1;
				ulgTasks.DisplayLayout.Bands[0].Columns["DisplaySequence"].Hidden = true;
				ulgTasks.DisplayLayout.Bands[0].Columns["TASKLIST_RID"].Hidden = true;
				ulgTasks.DisplayLayout.Bands[0].Columns["TASK_SEQUENCE"].Hidden = true;
				ulgTasks.DisplayLayout.Bands[0].Columns["TASK_TYPE"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["USER_RID"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["ITEM_TYPE"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["ITEM_RID"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["OWNER_USER_RID"].Hidden = true;
                //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                ulgTasks.DisplayLayout.Bands[0].Columns["EMAIL_SUCCESS_FROM"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["EMAIL_SUCCESS_TO"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["EMAIL_SUCCESS_CC"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["EMAIL_SUCCESS_BCC"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["EMAIL_SUCCESS_SUBJECT"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["EMAIL_SUCCESS_BODY"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["EMAIL_FAILURE_FROM"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["EMAIL_FAILURE_TO"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["EMAIL_FAILURE_CC"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["EMAIL_FAILURE_BCC"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["EMAIL_FAILURE_SUBJECT"].Hidden = true;
                ulgTasks.DisplayLayout.Bands[0].Columns["EMAIL_FAILURE_BODY"].Hidden = true;
                //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

				ulgTasks.DisplayLayout.Bands[0].Columns["Task"].Width = 100;
				ulgTasks.DisplayLayout.Bands[0].Columns["MAX_MESSAGE_LEVEL"].Width = 75;

				ulgTasks.DisplayLayout.Bands[0].SortedColumns.Add(ulgTasks.DisplayLayout.Bands[0].Columns["DisplaySequence"], false);

				ulgTasks.Rows.ExpandAll(true);

				SetControlReadOnly(ulgTasks, FunctionSecurity.IsReadOnly);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindForecastGridData(DataSetEntry aDataSetEntry)
		{
			try
			{
				ulgForecast.DataSource = aDataSetEntry.DataSet;
				ulgForecast.DisplayLayout.Override.SelectTypeRow = SelectType.SingleAutoDrag;
				ulgForecast.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
				ulgForecast.DisplayLayout.AddNewBox.Hidden = false;
				ulgForecast.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
				ulgForecast.DisplayLayout.Bands[0].AddButtonCaption = "Node";
				ulgForecast.DisplayLayout.Bands[1].AddButtonCaption = "Workflow/Method";

				//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				//ulgForecast.DisplayLayout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = 0;
				ulgForecast.DisplayLayout.Bands[0].Columns["ForecastMerchandise"].Header.VisiblePosition = 0;
				ulgForecast.DisplayLayout.Bands[0].Columns["ForecastMerchandise"].Header.Caption = "Merchandise";
				//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				ulgForecast.DisplayLayout.Bands[0].Columns["Version"].ValueList = ulgForecast.DisplayLayout.ValueLists["Version"];
				ulgForecast.DisplayLayout.Bands[0].Columns["Version"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				ulgForecast.DisplayLayout.Bands[0].Columns["Version"].Header.VisiblePosition = 1;
				ulgForecast.DisplayLayout.Bands[0].Columns["DisplaySequence"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[0].Columns["TASKLIST_RID"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[0].Columns["TASK_SEQUENCE"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[0].Columns["FORECAST_SEQUENCE"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[0].Columns["HN_RID"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[0].Columns["FV_RID"].Hidden = true;

				ulgForecast.DisplayLayout.Bands[1].Columns["ForecastWorkflowMethod"].CellActivation = Activation.NoEdit;
				ulgForecast.DisplayLayout.Bands[1].Columns["ForecastWorkflowMethod"].Header.Caption = "Workflow/Method";
				ulgForecast.DisplayLayout.Bands[1].Columns["WorkflowMethodType"].CellActivation = Activation.NoEdit;
				ulgForecast.DisplayLayout.Bands[1].Columns["WorkflowMethodType"].Header.Caption = "Type";
				ulgForecast.DisplayLayout.Bands[1].Columns["Execute Date Range"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
				ulgForecast.DisplayLayout.Bands[1].Columns["Execute Date Range"].CellActivation = Activation.NoEdit;
				ulgForecast.DisplayLayout.Bands[1].Columns["DisplaySequence"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[1].Columns["TASKLIST_RID"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[1].Columns["TASK_SEQUENCE"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[1].Columns["FORECAST_SEQUENCE"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[1].Columns["DETAIL_SEQUENCE"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[1].Columns["WORKFLOW_METHOD_IND"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[1].Columns["METHOD_RID"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[1].Columns["WORKFLOW_RID"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[1].Columns["EXECUTE_CDR_RID"].Hidden = true;
				ulgForecast.DisplayLayout.Bands[1].AddButtonCaption = "Workflow/Method";

				//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				//ulgForecast.DisplayLayout.Bands[0].Columns["Merchandise"].Width = 200;
				ulgForecast.DisplayLayout.Bands[0].Columns["ForecastMerchandise"].Width = 200;
				//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				ulgForecast.DisplayLayout.Bands[0].Columns["Version"].Width = 100;
				ulgForecast.DisplayLayout.Bands[1].Columns["ForecastWorkflowMethod"].Width = 200;
				ulgForecast.DisplayLayout.Bands[1].Columns["WorkflowMethodType"].Width = 100;
				ulgForecast.DisplayLayout.Bands[1].Columns["Execute Date Range"].Width = 200;
				
				ulgForecast.DisplayLayout.Bands[0].SortedColumns.Add(ulgForecast.DisplayLayout.Bands[0].Columns["DisplaySequence"], false);
				ulgForecast.DisplayLayout.Bands[1].SortedColumns.Add(ulgForecast.DisplayLayout.Bands[1].Columns["DisplaySequence"], false);

				ulgForecast.Rows.ExpandAll(true);

				SetControlReadOnly(ulgForecast, FunctionSecurity.IsReadOnly);

                // Begin TT#207 MID Track #6451 RMatelic - Task List - Protedct "Eecute Date Range" column
                ulgForecast.DisplayLayout.Bands[1].Columns["Execute Date Range"].CellActivation = Activation.NoEdit;
                // End TT#207
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindAllocateGridData(DataSetEntry aDataSetEntry)
		{
			try
			{
				ulgAllocate.DataSource = aDataSetEntry.DataSet;
				ulgAllocate.DisplayLayout.Override.SelectTypeRow = SelectType.SingleAutoDrag;
				ulgAllocate.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
				ulgAllocate.DisplayLayout.AddNewBox.Hidden = false;
				ulgAllocate.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
				ulgAllocate.DisplayLayout.Bands[0].AddButtonCaption = "Node";
				ulgAllocate.DisplayLayout.Bands[1].AddButtonCaption = "Workflow";

				//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				//ulgAllocate.DisplayLayout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = 0;
				ulgAllocate.DisplayLayout.Bands[0].Columns["AllocateMerchandise"].Header.VisiblePosition = 0;
				ulgAllocate.DisplayLayout.Bands[0].Columns["AllocateMerchandise"].Header.Caption = "Merchandise";
				//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
                ulgAllocate.DisplayLayout.Bands[0].Columns["Filter"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList; //TT#1313-MD -jsobek -Header Filters
                ulgAllocate.DisplayLayout.Bands[0].Columns["Filter"].ValueList = ulgAllocate.DisplayLayout.ValueLists["HeaderFilters"]; //TT#1313-MD -jsobek -Header Filters
                ulgAllocate.DisplayLayout.Bands[0].Columns["Filter"].Header.VisiblePosition = 1; //TT#1313-MD -jsobek -Header Filters
                //ulgAllocate.DisplayLayout.Bands[0].Columns["Header"].Header.VisiblePosition = 2; //TT#1313-MD -jsobek -Header Filters
                //ulgAllocate.DisplayLayout.Bands[0].Columns["PO"].Header.VisiblePosition = 3; //TT#1313-MD -jsobek -Header Filters
				ulgAllocate.DisplayLayout.Bands[0].Columns["DisplaySequence"].Hidden = true;
				ulgAllocate.DisplayLayout.Bands[0].Columns["TASKLIST_RID"].Hidden = true;
				ulgAllocate.DisplayLayout.Bands[0].Columns["TASK_SEQUENCE"].Hidden = true;
				ulgAllocate.DisplayLayout.Bands[0].Columns["ALLOCATE_SEQUENCE"].Hidden = true;
				ulgAllocate.DisplayLayout.Bands[0].Columns["HN_RID"].Hidden = true;
                //ulgAllocate.DisplayLayout.Bands[0].Columns["ALLOCATE_TYPE"].Hidden = true; //TT#1313-MD -jsobek -Header Filters
                ulgAllocate.DisplayLayout.Bands[0].Columns["FILTER_RID"].Hidden = true; //TT#1313-MD -jsobek -Header Filters
                //ulgAllocate.DisplayLayout.Bands[0].Columns["HEADER_ID"].Hidden = true; //TT#1313-MD -jsobek -Header Filters
                //ulgAllocate.DisplayLayout.Bands[0].Columns["PO_ID"].Hidden = true; //TT#1313-MD -jsobek -Header Filters

				ulgAllocate.DisplayLayout.Bands[1].Columns["AllocateWorkflowMethod"].CellActivation = Activation.NoEdit;
				ulgAllocate.DisplayLayout.Bands[1].Columns["AllocateWorkflowMethod"].Header.Caption = "Workflow";
				ulgAllocate.DisplayLayout.Bands[1].Columns["WorkflowMethodType"].CellActivation = Activation.NoEdit;
				ulgAllocate.DisplayLayout.Bands[1].Columns["WorkflowMethodType"].Header.Caption = "Type";
				ulgAllocate.DisplayLayout.Bands[1].Columns["WorkflowMethodType"].Hidden = true; // FUTURE COLUMN TO ALLOW FOR ALLOCATION METHODS
				ulgAllocate.DisplayLayout.Bands[1].Columns["Execute Date Range"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
				ulgAllocate.DisplayLayout.Bands[1].Columns["Execute Date Range"].CellActivation = Activation.NoEdit;
				ulgAllocate.DisplayLayout.Bands[1].Columns["DisplaySequence"].Hidden = true;
				ulgAllocate.DisplayLayout.Bands[1].Columns["TASKLIST_RID"].Hidden = true;
				ulgAllocate.DisplayLayout.Bands[1].Columns["TASK_SEQUENCE"].Hidden = true;
				ulgAllocate.DisplayLayout.Bands[1].Columns["ALLOCATE_SEQUENCE"].Hidden = true;
				ulgAllocate.DisplayLayout.Bands[1].Columns["DETAIL_SEQUENCE"].Hidden = true;
				ulgAllocate.DisplayLayout.Bands[1].Columns["WORKFLOW_METHOD_IND"].Hidden = true;
				ulgAllocate.DisplayLayout.Bands[1].Columns["METHOD_RID"].Hidden = true;
				ulgAllocate.DisplayLayout.Bands[1].Columns["WORKFLOW_RID"].Hidden = true;
				ulgAllocate.DisplayLayout.Bands[1].Columns["EXECUTE_CDR_RID"].Hidden = true;
				ulgAllocate.DisplayLayout.Bands[1].AddButtonCaption = "Workflow";

				//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				//ulgAllocate.DisplayLayout.Bands[0].Columns["Merchandise"].Width = 200;
				ulgAllocate.DisplayLayout.Bands[0].Columns["AllocateMerchandise"].Width = 200;
				//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
                ulgAllocate.DisplayLayout.Bands[0].Columns["Filter"].Width = 180; //TT#1313-MD -jsobek -Header Filters
                //ulgAllocate.DisplayLayout.Bands[0].Columns["Header"].Width = 80; //TT#1313-MD -jsobek -Header Filters
                //ulgAllocate.DisplayLayout.Bands[0].Columns["PO"].Width = 80; //TT#1313-MD -jsobek -Header Filters
				ulgAllocate.DisplayLayout.Bands[1].Columns["AllocateWorkflowMethod"].Width = 200;
				ulgAllocate.DisplayLayout.Bands[1].Columns["WorkflowMethodType"].Width = 100;
				ulgAllocate.DisplayLayout.Bands[1].Columns["Execute Date Range"].Width = 200;

				ulgAllocate.DisplayLayout.Bands[0].SortedColumns.Add(ulgAllocate.DisplayLayout.Bands[0].Columns["DisplaySequence"], false);
				ulgAllocate.DisplayLayout.Bands[1].SortedColumns.Add(ulgAllocate.DisplayLayout.Bands[1].Columns["DisplaySequence"], false);

				ulgAllocate.Rows.ExpandAll(true);

				SetControlReadOnly(ulgAllocate, FunctionSecurity.IsReadOnly);

                // Begin TT#207 MID Track #6451 RMatelic - Task List - Protedct "Eecute Date Range" column
                ulgAllocate.DisplayLayout.Bands[1].Columns["Execute Date Range"].CellActivation = Activation.NoEdit;
                // End TT#207
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindRollupGridData(DataTableEntry aDataTableEntry)
		{
			int verKey;
			VersionProfile verProf;

			int visiblePosition = 0;
			try
			{
				ulgRollup.DataSource = aDataTableEntry.DataTable;
				ulgRollup.DisplayLayout.Override.SelectTypeRow = SelectType.SingleAutoDrag;
				ulgRollup.DisplayLayout.AddNewBox.Hidden = false;
				ulgRollup.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
				ulgRollup.DisplayLayout.Bands[0].AddButtonCaption = "Node";
				ulgRollup.DisplayLayout.Bands[0].ColHeaderLines = 2;

				//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				//ulgRollup.DisplayLayout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["RollupMerchandise"].Header.VisiblePosition = visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["RollupMerchandise"].Header.Caption = "Merchandise";
				//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				ulgRollup.DisplayLayout.Bands[0].Columns["FV_RID"].Header.Caption = "Version";
				ulgRollup.DisplayLayout.Bands[0].Columns["FV_RID"].ValueList = ulgRollup.DisplayLayout.ValueLists["Version"];
				ulgRollup.DisplayLayout.Bands[0].Columns["FV_RID"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				ulgRollup.DisplayLayout.Bands[0].Columns["FV_RID"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["Rollup Date Range"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
				ulgRollup.DisplayLayout.Bands[0].Columns["Rollup Date Range"].CellActivation = Activation.NoEdit;
				ulgRollup.DisplayLayout.Bands[0].Columns["Rollup Date Range"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["From Level"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["From Level"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				ulgRollup.DisplayLayout.Bands[0].Columns["To Level"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["To Level"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				ulgRollup.DisplayLayout.Bands[0].Columns["Posting"].Header.Caption = "Posting";
				ulgRollup.DisplayLayout.Bands[0].Columns["Posting"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				ulgRollup.DisplayLayout.Bands[0].Columns["Posting"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["Reclass"].Header.Caption = "Reclass";
				ulgRollup.DisplayLayout.Bands[0].Columns["Reclass"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				ulgRollup.DisplayLayout.Bands[0].Columns["Reclass"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["Hierarchy Levels"].Header.Caption = "Hierarchy\nLevels";
				ulgRollup.DisplayLayout.Bands[0].Columns["Hierarchy Levels"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				ulgRollup.DisplayLayout.Bands[0].Columns["Hierarchy Levels"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["Day To Week"].Header.Caption = "Day To\nWeek";
				ulgRollup.DisplayLayout.Bands[0].Columns["Day To Week"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				ulgRollup.DisplayLayout.Bands[0].Columns["Day To Week"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["Day"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				ulgRollup.DisplayLayout.Bands[0].Columns["Day"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["Week"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				ulgRollup.DisplayLayout.Bands[0].Columns["Week"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["Store"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				ulgRollup.DisplayLayout.Bands[0].Columns["Store"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["Chain"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				ulgRollup.DisplayLayout.Bands[0].Columns["Chain"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["Store To Chain"].Header.Caption = "Store To\nChain";
				ulgRollup.DisplayLayout.Bands[0].Columns["Store To Chain"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				ulgRollup.DisplayLayout.Bands[0].Columns["Store To Chain"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["Intransit"].Header.Caption = _SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.IntransitVariable.VariableName;
				ulgRollup.DisplayLayout.Bands[0].Columns["Intransit"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				ulgRollup.DisplayLayout.Bands[0].Columns["Intransit"].Header.VisiblePosition = ++visiblePosition;
				ulgRollup.DisplayLayout.Bands[0].Columns["DisplaySequence"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["TASKLIST_RID"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["TASK_SEQUENCE"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["ROLLUP_SEQUENCE"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["HN_RID"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["ROLLUP_CDR_RID"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["FROM_PH_OFFSET_IND"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["FROM_PH_RID"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["FROM_PHL_SEQUENCE"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["FROM_OFFSET"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["TO_PH_OFFSET_IND"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["TO_PH_RID"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["TO_PHL_SEQUENCE"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["TO_OFFSET"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["POSTING_IND"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["RECLASS_IND"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["HIERARCHY_LEVELS_IND"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["DAY_TO_WEEK_IND"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["DAY_IND"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["WEEK_IND"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["STORE_IND"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["CHAIN_IND"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["STORE_TO_CHAIN_IND"].Hidden = true;
				ulgRollup.DisplayLayout.Bands[0].Columns["INTRANSIT_IND"].Hidden = true;

				foreach (UltraGridRow row in ulgRollup.Rows)
				{
					//BEGIN TT#4791 - DOConnell - Rollup Tasklist - able to save tasklist when a 'To Level' is not selected

					//FillLevelValueLists(row);
                    if (this.RollUpLoaded == false)
                    {
                        FillLevelValueLists(row);
                    }
                    else
                    {
                        foreach (UltraGridRow rollupRow in ulgRollup.Rows)
                        {
                            if (Convert.IsDBNull(rollupRow.Cells["From Level"].Value) ||
                                Convert.IsDBNull(rollupRow.Cells["To Level"].Value))
                            {
                                break;
                            }
                            FillLevelValueLists(row);
                        }
                    }
					//END TT#4791 - DOConnell - Rollup Tasklist - able to save tasklist when a 'To Level' is not selected
					if (row.Cells["FV_RID"].Value != System.DBNull.Value)
					{
						verKey = Convert.ToInt32(row.Cells["FV_RID"].Value);

						if (verKey == Include.FV_ActualRID)
						{
							row.Cells["Day To Week"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
							row.Cells["Day"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
							row.Cells["Intransit"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
						}
						else
						{
							row.Cells["Day To Week"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
							row.Cells["Day"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
							row.Cells["Intransit"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
						}

						verProf = (VersionProfile)_versionProfList.FindKey(verKey);

						if (verProf != null)
						{
							if (verProf.ChainSecurity.AllowUpdate)
							{
								row.Cells["Chain"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
							}
							else
							{
								row.Cells["Chain"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
							}

							if (verProf.StoreSecurity.AllowUpdate)
							{
								row.Cells["Store"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
							}
							else
							{
								row.Cells["Store"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
							}
						}
						else
						{
							row.Cells["Chain"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
							row.Cells["Store"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
						}
					}
				}

				ulgRollup.UpdateData();

				//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				//ulgRollup.DisplayLayout.Bands[0].Columns["Merchandise"].MinWidth = 100;
				ulgRollup.DisplayLayout.Bands[0].Columns["RollupMerchandise"].MinWidth = 100;
				//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				ulgRollup.DisplayLayout.Bands[0].Columns["FV_RID"].MinWidth = 100;
				ulgRollup.DisplayLayout.Bands[0].Columns["Rollup Date Range"].MinWidth = 100;
				ulgRollup.DisplayLayout.Bands[0].Columns["From Level"].MinWidth = 100;
				ulgRollup.DisplayLayout.Bands[0].Columns["To Level"].MinWidth = 100;

				//Begin Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				//ulgRollup.DisplayLayout.Bands[0].Columns["Merchandise"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgRollup.DisplayLayout.Bands[0].Columns["RollupMerchandise"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				//End Track #6281 - JScott - Task List when trying to drag/drop in a merchandise node for Forecasting or Allocate it does not allow.
				ulgRollup.DisplayLayout.Bands[0].Columns["FV_RID"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgRollup.DisplayLayout.Bands[0].Columns["Rollup Date Range"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgRollup.DisplayLayout.Bands[0].Columns["From Level"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgRollup.DisplayLayout.Bands[0].Columns["To Level"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgRollup.DisplayLayout.Bands[0].Columns["Posting"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgRollup.DisplayLayout.Bands[0].Columns["Hierarchy Levels"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgRollup.DisplayLayout.Bands[0].Columns["Day To Week"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgRollup.DisplayLayout.Bands[0].Columns["Day"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgRollup.DisplayLayout.Bands[0].Columns["Week"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgRollup.DisplayLayout.Bands[0].Columns["Store"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgRollup.DisplayLayout.Bands[0].Columns["Chain"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgRollup.DisplayLayout.Bands[0].Columns["Store To Chain"].PerformAutoResize(PerformAutoSizeType.VisibleRows);
				ulgRollup.DisplayLayout.Bands[0].Columns["Intransit"].PerformAutoResize(PerformAutoSizeType.VisibleRows);

				ulgRollup.DisplayLayout.Bands[0].SortedColumns.Add(ulgRollup.DisplayLayout.Bands[0].Columns["DisplaySequence"], false);

				ulgRollup.Rows.ExpandAll(true);

				SetControlReadOnly(ulgRollup, FunctionSecurity.IsReadOnly);

                // Begin MID Track #6451 RMatelic - Task List - Protedct "Eecute Date Range" column
                ulgRollup.DisplayLayout.Bands[0].Columns["Rollup Date Range"].CellActivation = Activation.NoEdit;
                // End MID Track #6451 
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin TT#155 - JScott - Size Curve Method
		private void BindSizeCurveMethodGridData(DataTableEntry aDataTableEntry)
		{
			try
			{
				ulgSizeCurveMethod.DataSource = aDataTableEntry.DataTable;
				ulgSizeCurveMethod.DisplayLayout.Override.SelectTypeRow = SelectType.SingleAutoDrag;
				ulgSizeCurveMethod.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
				ulgSizeCurveMethod.DisplayLayout.AddNewBox.Hidden = false;
				ulgSizeCurveMethod.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;

				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["SizeCurveMethod"].CellActivation = Activation.NoEdit;
				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["SizeCurveMethod"].Header.Caption = "Method";
				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["Execute Date Range"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["Execute Date Range"].CellActivation = Activation.NoEdit;
				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["DisplaySequence"].Hidden = true;
				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["TASKLIST_RID"].Hidden = true;
				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["TASK_SEQUENCE"].Hidden = true;
				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["GENERATE_SEQUENCE"].Hidden = true;
				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["HN_RID"].Hidden = true;
				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["METHOD_RID"].Hidden = true;
				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["EXECUTE_CDR_RID"].Hidden = true;
				ulgSizeCurveMethod.DisplayLayout.Bands[0].AddButtonCaption = "Method";

				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["SizeCurveMethod"].Width = 200;
				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["Execute Date Range"].Width = 200;

				ulgSizeCurveMethod.DisplayLayout.Bands[0].SortedColumns.Add(ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["DisplaySequence"], false);

				ulgSizeCurveMethod.Rows.ExpandAll(true);

				SetControlReadOnly(ulgSizeCurveMethod, FunctionSecurity.IsReadOnly);

				ulgSizeCurveMethod.DisplayLayout.Bands[0].Columns["Execute Date Range"].CellActivation = Activation.NoEdit;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End TT#155 - JScott - Size Curve Method
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		private void BindSizeCurvesGridData(DataTableEntry aDataTableEntry)
		{
			try
			{
				ulgSizeCurves.DataSource = aDataTableEntry.DataTable;
				ulgSizeCurves.DisplayLayout.Override.SelectTypeRow = SelectType.SingleAutoDrag;
				ulgSizeCurves.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
				ulgSizeCurves.DisplayLayout.AddNewBox.Hidden = false;
				ulgSizeCurves.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;

				ulgSizeCurves.DisplayLayout.Bands[0].Columns["SizeCurvesMerchandise"].Header.VisiblePosition = 0;
				ulgSizeCurves.DisplayLayout.Bands[0].Columns["SizeCurvesMerchandise"].Header.Caption = "Merchandise";
				ulgSizeCurves.DisplayLayout.Bands[0].Columns["DisplaySequence"].Hidden = true;
				ulgSizeCurves.DisplayLayout.Bands[0].Columns["TASKLIST_RID"].Hidden = true;
				ulgSizeCurves.DisplayLayout.Bands[0].Columns["TASK_SEQUENCE"].Hidden = true;
				ulgSizeCurves.DisplayLayout.Bands[0].Columns["GENERATE_SEQUENCE"].Hidden = true;
				ulgSizeCurves.DisplayLayout.Bands[0].Columns["HN_RID"].Hidden = true;
				ulgSizeCurves.DisplayLayout.Bands[0].Columns["METHOD_RID"].Hidden = true;
				ulgSizeCurves.DisplayLayout.Bands[0].Columns["EXECUTE_CDR_RID"].Hidden = true;
				ulgSizeCurves.DisplayLayout.Bands[0].AddButtonCaption = "Node";

				ulgSizeCurves.DisplayLayout.Bands[0].Columns["SizeCurvesMerchandise"].Width = 200;

				ulgSizeCurves.DisplayLayout.Bands[0].SortedColumns.Add(ulgSizeCurves.DisplayLayout.Bands[0].Columns["DisplaySequence"], false);

				ulgSizeCurves.Rows.ExpandAll(true);

				SetControlReadOnly(ulgSizeCurves, FunctionSecurity.IsReadOnly);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties

		

		private void FillLevelValueLists(UltraGridRow aRow)
		{
			Infragistics.Win.ValueList fromValList;
			Infragistics.Win.ValueList toValList;
			HierarchyNodeProfile nodeProf;
			HierarchyProfile hierProf;
			int startLevel;
			int i;
			HierarchyProfile mainHierProf;
			int highestGuestLevel;
			int longestBranchCount;
			int offset;
			HierarchyLevelValueItem valItem;
            ArrayList guestLevels;
            HierarchyLevelProfile hlp;
			
			try
			{
				if (aRow.Cells["HN_RID"].Value != System.DBNull.Value)
				{
					nodeProf = _SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(aRow.Cells["HN_RID"].Value));
					hierProf = _SAB.HierarchyServerSession.GetHierarchyData(nodeProf.HomeHierarchyRID);

					// Load Level arrays

					_fromLevelList.Clear();
					_toLevelList.Clear();

					if (hierProf.HierarchyType == eHierarchyType.organizational)
					{
						if (nodeProf.HomeHierarchyLevel == 0)
						{
							_toLevelList.Add(new HierarchyLevelValueItem(hierProf.Key, 0, hierProf.HierarchyID));
							_fromLevelList.Add(new HierarchyLevelValueItem(hierProf.Key, 0, hierProf.HierarchyID));
							startLevel = 1;
						}
						else
						{
							_toLevelList.Add(new HierarchyLevelValueItem(hierProf.Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[nodeProf.HomeHierarchyLevel]).Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[nodeProf.HomeHierarchyLevel]).LevelID));
							_fromLevelList.Add(new HierarchyLevelValueItem(hierProf.Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[nodeProf.HomeHierarchyLevel]).Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[nodeProf.HomeHierarchyLevel]).LevelID));
							startLevel = nodeProf.HomeHierarchyLevel + 1;
						}

						for (i = startLevel; i <= hierProf.HierarchyLevels.Count; i++)
						{
							_fromLevelList.Add(new HierarchyLevelValueItem(hierProf.Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[i]).Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[i]).LevelID));
							_toLevelList.Add(new HierarchyLevelValueItem(hierProf.Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[i]).Key, ((HierarchyLevelProfile)hierProf.HierarchyLevels[i]).LevelID));
						}
					}
					else
					{
						mainHierProf = _SAB.HierarchyServerSession.GetMainHierarchyData();
						highestGuestLevel = _SAB.HierarchyServerSession.GetHighestGuestLevel(nodeProf.Key);
                        guestLevels = SAB.HierarchyServerSession.GetAllGuestLevels(nodeProf.Key);

						_toLevelList.Add(new HierarchyLevelValueItem(0, nodeProf.NodeID));
						_fromLevelList.Add(new HierarchyLevelValueItem(0, nodeProf.NodeID));
						startLevel = 1;

                        if (guestLevels.Count == 1)
                        {
                            hlp = (HierarchyLevelProfile)guestLevels[0];
                            _fromLevelList.Add(new HierarchyLevelValueItem(mainHierProf.Key, hlp.Key, hlp.LevelID));
                            _toLevelList.Add(new HierarchyLevelValueItem(mainHierProf.Key, hlp.Key, hlp.LevelID));
                        }

                        longestBranchCount = SAB.HierarchyServerSession.GetLongestBranch(nodeProf.Key, true);
                        //BEGIN TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                        //_longestBranch = SAB.HierarchyServerSession.GetLongestBranch(aHierarchyNodeProfile.Key);
                        DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(nodeProf.Key);
                        // Begin TT#5091 - JSmith - From Level offset list contains extra offset
                        //longestBranchCount = hierarchyLevels.Rows.Count;
                        longestBranchCount = hierarchyLevels.Rows.Count - 1;
                        // End TT#5091 - JSmith - From Level offset list contains extra offset
                        //END TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly

						offset = 0;

						for (i = 0; i < longestBranchCount; i++)
						{
							offset++;
							_fromLevelList.Add(new HierarchyLevelValueItem(offset, "+" + offset.ToString()));
							_toLevelList.Add(new HierarchyLevelValueItem(offset, "+" + offset.ToString()));
						}
					}

					// Setup Level ValueLists

					if (aRow.Cells["From Level"].ValueList == null)
					{
						fromValList = new Infragistics.Win.ValueList();
						aRow.Cells["From Level"].ValueList = fromValList;
					}
					else
					{
						fromValList = (Infragistics.Win.ValueList)aRow.Cells["From Level"].ValueList;
						fromValList.ValueListItems.Clear();
					}

					if (aRow.Cells["To Level"].ValueList == null)
					{
						toValList = new Infragistics.Win.ValueList();
						aRow.Cells["To Level"].ValueList = toValList;
					}
					else
					{
						toValList = (Infragistics.Win.ValueList)aRow.Cells["To Level"].ValueList;
						toValList.ValueListItems.Clear();
					}

					for (i = 0; i < _fromLevelList.Count; i++)
					{
						valItem = (HierarchyLevelValueItem)_fromLevelList[i];
						fromValList.ValueListItems.Add(i, valItem.LevelName);
					}

					for (i = 0; i < _toLevelList.Count; i++)
					{
						valItem = (HierarchyLevelValueItem)_toLevelList[i];
						toValList.ValueListItems.Add(i, valItem.LevelName);
					}

					if (aRow.Cells["FROM_PHL_SEQUENCE"].Value == System.DBNull.Value &&
						aRow.Cells["FROM_OFFSET"].Value == System.DBNull.Value)
					{
						aRow.Cells["From Level"].Value = System.DBNull.Value;
					}
					else
					{
						for (i = 0; i < _fromLevelList.Count; i++)
						{
							valItem = (HierarchyLevelValueItem)_fromLevelList[i];
                            if (aRow.Cells["FROM_PH_OFFSET_IND"].Value == System.DBNull.Value &&
								(valItem.LevelType == eHierarchyDescendantType.levelType && valItem.HierarchyRID == Convert.ToInt32(aRow.Cells["FROM_PH_RID"].Value) && valItem.LevelRID == Convert.ToInt32(aRow.Cells["FROM_PHL_SEQUENCE"].Value)) ||

                                (aRow.Cells["FROM_PH_OFFSET_IND"].Value != System.DBNull.Value && valItem.LevelType == (eHierarchyDescendantType)Convert.ToInt32(aRow.Cells["FROM_PH_OFFSET_IND"].Value) &&
								((valItem.LevelType == eHierarchyDescendantType.levelType && valItem.HierarchyRID == Convert.ToInt32(aRow.Cells["FROM_PH_RID"].Value) && valItem.LevelRID == Convert.ToInt32(aRow.Cells["FROM_PHL_SEQUENCE"].Value)) ||
								(valItem.LevelType == eHierarchyDescendantType.offset && valItem.Offset == Convert.ToInt32(aRow.Cells["FROM_OFFSET"].Value)))))
							{
								aRow.Cells["From Level"].Value = i;
								break;
							}
						}

						if (i == _fromLevelList.Count)
						{
							aRow.Cells["From Level"].Value = System.DBNull.Value;
						}
					}

					//BEGIN TT#4780 - DOConnell - Rollup Task List options
                    if (aRow.Cells["From Level"].Value != DBNull.Value)
                    {

                            toValList.ValueListItems.Clear();
                            for (i = 0; i <= (int)aRow.Cells["From Level"].Value; i++)
                            {
                                valItem = (HierarchyLevelValueItem)_toLevelList[i];
                                toValList.ValueListItems.Add(i, valItem.LevelName);
                                
                            }
                            aRow.Cells["To Level"].ValueList = toValList;

                    }
					//END TT#4780 - DOConnell - Rollup Task List options
					
					if (aRow.Cells["TO_PHL_SEQUENCE"].Value == System.DBNull.Value &&
						aRow.Cells["TO_OFFSET"].Value == System.DBNull.Value)
					{
						aRow.Cells["To Level"].Value = System.DBNull.Value;
					}
					else
					{
						//BEGIN TT#4780 - DOConnell - Rollup Task List options
                        // Begin TT#5079 - JSmith - To Level does not display on Rollup task
                        //if ((int)aRow.Cells["TO_PH_OFFSET_IND"].Value < (int)aRow.Cells["From Level"].Value)
                        //{
                        // End TT#5079 - JSmith - To Level does not display on Rollup task
                            for (i = 0; i < _toLevelList.Count; i++)
                            {
                                valItem = (HierarchyLevelValueItem)_toLevelList[i];
                                if (aRow.Cells["TO_PH_OFFSET_IND"].Value == System.DBNull.Value &&
                                    (valItem.LevelType == eHierarchyDescendantType.levelType && valItem.HierarchyRID == Convert.ToInt32(aRow.Cells["TO_PH_RID"].Value) && valItem.LevelRID == Convert.ToInt32(aRow.Cells["TO_PHL_SEQUENCE"].Value)) ||

                                    (aRow.Cells["TO_PH_OFFSET_IND"].Value != System.DBNull.Value && valItem.LevelType == (eHierarchyDescendantType)Convert.ToInt32(aRow.Cells["TO_PH_OFFSET_IND"].Value) &&
                                    ((valItem.LevelType == eHierarchyDescendantType.levelType && valItem.HierarchyRID == Convert.ToInt32(aRow.Cells["TO_PH_RID"].Value) && valItem.LevelRID == Convert.ToInt32(aRow.Cells["TO_PHL_SEQUENCE"].Value)) ||
                                    (valItem.LevelType == eHierarchyDescendantType.offset && valItem.Offset == Convert.ToInt32(aRow.Cells["TO_OFFSET"].Value)))))
                                {
                                    aRow.Cells["To Level"].Value = valItem.LevelName;
                                    break;
                                }
                            }

                            if (i == _toLevelList.Count)
                            {
                                aRow.Cells["To Level"].Value = System.DBNull.Value;
                            }
                        // Begin TT#5079 - JSmith - To Level does not display on Rollup task
                        //}
                        //else 
                        //{
                        //    aRow.Cells["To Level"].Value = System.DBNull.Value;
                        //}
                        // End TT#5079 - JSmith - To Level does not display on Rollup task
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        private void HideTabs()
        {
            foreach (Infragistics.Win.UltraWinTabControl.UltraTab t in ultraTabControl1.Tabs)
            {
                    t.Visible = false; 
            }
        }
        
        /// <summary>
        /// List of tab keys in the tab control
        /// </summary>
        private struct tabKeys
        {
            public static string tabEmail = "tabEmail";
            public static string tabAllocate = "tabAllocate";
            public static string tabForecast = "tabForecast";
            public static string tabRollup = "tabRollup";
            public static string tabSizeCurveMethod = "tabSizeCurveMethod";
            public static string tabSizeCurves = "tabSizeCurves";
            public static string tabSizeDayToWeekSummary = "tabSizeDayToWeekSummary";
            public static string tabPosting = "tabPosting";
            public static string tabRelieveIntransit = "tabRelieveIntransit";
            public static string tabExternalProgram = "tabExternalProgram";
            public static string tabBatchComp = "tabBatchComp";		// TT#1595-MD - stodd - Batch Comp
            public static string tabHeaderReconcile = "tabHeaderReconcile";
			//BEGIN TT#4574 - DOConnell - Purge Task List does not have an email option
            public static string tabPurge = "tabPurge";
            public static string tabChainForecasting = "tabChainForecasting";
            public static string tabPushToBackStock = "tabPushToBackStock";
			//END TT#4574 - DOConnell - Purge Task List does not have an email option
        }
        private void ShowTab(string key, string tabCaptionText, bool emailTabSelected)
        {
            //BEGIN TT#4574 - DOConnell - Purge Task List does not have an email option
            //ultraTabControl1.Tabs[key].Text = tabCaptionText;
            if (key == "tabEmail")
            {
                ultraTabControl1.Tabs[key].Text = "Email";
            }
            else
            {
                ultraTabControl1.Tabs[key].Text = tabCaptionText;
            }
            //END TT#4574 - DOConnell - Purge Task List does not have an email option

            ultraTabControl1.Tabs[key].Visible = true;

            ultraTabControl1.Tabs[tabKeys.tabEmail].Visible = true;
            //if the email tab is selected and they change tasks, keep the email tab selected
            if (emailTabSelected == true)
            {
                ultraTabControl1.Tabs[tabKeys.tabEmail].Selected = true;
            }
            else
            {
                ultraTabControl1.Tabs[key].Selected = true;
            }
        }
        //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

		private void ShowTaskPanel()
		{
			try
			{
				if (ulgTasks.ActiveRow != null && ulgTasks.ActiveRow.Cells["TASK_TYPE"].Value != System.DBNull.Value)
				{
                    //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                    //Hide tabs
                    bool emailTabSelected = ultraTabControl1.Tabs[tabKeys.tabEmail].Selected;
                    HideTabs();

                    string taskText = (string)ulgTasks.ActiveRow.Cells["Task"].Value;
                    //this.emailTaskList1.taskTypeName = taskText;
                    this.emailTaskList1.taskListName = this.txtTaskListName.Text;
                    this.emailTaskList1.taskType = (int)ulgTasks.ActiveRow.Cells["TASK_TYPE"].Value;
                    DataRow[] drTask = _dtTask.Select("TASKLIST_RID=" + (int)ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value + " AND TASK_SEQUENCE=" + (int)ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value);

                    this.emailTaskList1.LoadFromDataRow(drTask[0]);
                    bool tempbln = this.emailTaskList1.IsValid(); //redraws any invalid messages on the control
                   

                    //pnlForecast.Visible = false;
                    //pnlAllocate.Visible = false;
                    //pnlRollup.Visible = false;
                    ////Begin TT#155 - JScott - Size Curve Method
                    //pnlSizeCurveMethod.Visible = false;
                    ////End TT#155 - JScott - Size Curve Method
                    ////Begin TT#155 - JScott - Add Size Curve info to Node Properties
                    //pnlSizeCurves.Visible = false;
                    ////End TT#155 - JScott - Add Size Curve info to Node Properties
                    //pnlPosting.Visible = false;
                    //pnlRelieve.Visible = false;
                    //pnlProgram.Visible = false;
                    ////Begin TT#391 - STodd - Size day to week summary
                    //pnlSizeDayToWeekSummary.Visible = false;
                    ////End TT#391 - STodd - Size day to week summary
                    //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

					switch ((eTaskType)Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_TYPE"].Value))
					{
                        //BEGIN TT#4574 - DOConnell - Purge Task List does not have an email option
                        case eTaskType.Purge:

                            ShowTab(tabKeys.tabPurge, taskText, emailTabSelected);
                            break;

                        case eTaskType.PushToBackStockLoad:

                            ShowTab(tabKeys.tabPushToBackStock, taskText, emailTabSelected);
                            break;
                        //END TT#4574 - DOConnell - Purge Task List does not have an email option

						case eTaskType.Allocate:

							BindAllocateGridData(GetAllocateDataSetEntry(Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value)));
                            //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                            ShowTab(tabKeys.tabAllocate, taskText, emailTabSelected);
							//pnlAllocate.Visible = true;
                            //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
							break;

						case eTaskType.Forecasting :

							BindForecastGridData(GetForecastDataSetEntry(Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value)));
                            //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                            ShowTab(tabKeys.tabForecast, taskText, emailTabSelected);
                            //pnlForecast.Visible = true;
                            //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
							break;

						case eTaskType.computationDriver :
                            //BEGIN TT#4574 - DOConnell - Purge Task List does not have an email option
                            ShowTab(tabKeys.tabChainForecasting, taskText, emailTabSelected);
                            //END TT#4574 - DOConnell - Purge Task List does not have an email option
							break;

						case eTaskType.Rollup :

							BindRollupGridData(GetRollupDataTableEntry(Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value)));
                            //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                            ShowTab(tabKeys.tabRollup, taskText, emailTabSelected);
                            //pnlRollup.Visible = true;
                            //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
							break;

						//Begin TT#155 - JScott - Size Curve Method
						case eTaskType.SizeCurveMethod:

							BindSizeCurveMethodGridData(GetSizeCurveMethodDataTableEntry(Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value)));
                            //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                            ShowTab(tabKeys.tabSizeCurveMethod, taskText, emailTabSelected);
                            //pnlSizeCurveMethod.Visible = true;
                            //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
							break;

						//End TT#155 - JScott - Size Curve Method
						//Begin TT#155 - JScott - Add Size Curve info to Node Properties
						case eTaskType.SizeCurves:

							BindSizeCurvesGridData(GetSizeCurvesDataTableEntry(Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value)));
							//Begin TT#707 - JScott - Size Curve process needs to multi-thread

							_currentSizeCurveNodeRow = _dtTaskSizeCurvesNode.Rows.Find(new object[] { Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value), Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value) });

							if (_currentSizeCurveNodeRow == null)
							{
								_currentSizeCurveNodeRow = _dtTaskSizeCurvesNode.NewRow();
								_currentSizeCurveNodeRow["TASKLIST_RID"] = ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value;
								_currentSizeCurveNodeRow["TASK_SEQUENCE"] = ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value;
								_currentSizeCurveNodeRow["CONCURRENT_PROCESSES"] = Include.ConcurrentSizeCurveProcesses;
								_dtTaskSizeCurvesNode.Rows.Add(_currentSizeCurveNodeRow);
								_dtTaskSizeCurvesNode.AcceptChanges();
							}

							nudSizeCurveConcurrentProcesses.Value = Convert.ToInt32(_currentSizeCurveNodeRow["CONCURRENT_PROCESSES"]);

							//End TT#707 - JScott - Size Curve process needs to multi-thread
							
                            //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                            ShowTab(tabKeys.tabSizeCurves, taskText, emailTabSelected);
                            //pnlSizeCurves.Visible = true;
                            //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
							break;

						//End TT#155 - JScott - Add Size Curve info to Node Properties
						//Begin TT#391 - STodd - Size day to week summary
						case eTaskType.SizeDayToWeekSummary:
							txtOverrideNode.Tag = new MIDMerchandiseTextBoxTag(SAB, txtOverrideNode, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
							midDateRangeSizeDayWeekSum.DateRangeRID = Include.UndefinedCalendarDateRange;
							midDateRangeSizeDayWeekSum.Text = string.Empty;
							_currentSizeDayToWeekSummaryRow = _dtTaskSizeDayToWeekSummary.Rows.Find(new object[] { Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value), Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value) });
							if (_currentSizeDayToWeekSummaryRow == null)
							{
								_currentSizeDayToWeekSummaryRow = _dtTaskSizeDayToWeekSummary.NewRow();
								_currentSizeDayToWeekSummaryRow["TASKLIST_RID"] = ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value;
								_currentSizeDayToWeekSummaryRow["TASK_SEQUENCE"] = ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value;
								_currentSizeDayToWeekSummaryRow["CDR_RID"] = Include.UndefinedCalendarDateRange;
								_currentSizeDayToWeekSummaryRow["HN_RID"] = Include.NoRID;
								_dtTaskSizeDayToWeekSummary.Rows.Add(_currentSizeDayToWeekSummaryRow);
								_dtTaskSizeDayToWeekSummary.AcceptChanges();
							}
							else
							{
								midDateRangeSizeDayWeekSum.DateRangeRID = int.Parse(_currentSizeDayToWeekSummaryRow["CDR_RID"].ToString());
								_overrideNodeRid = int.Parse(_currentSizeDayToWeekSummaryRow["HN_RID"].ToString());

								if (_overrideNodeRid != Include.NoRID)
								{
									HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_overrideNodeRid, true, true);
                                    //BEGIN TT#3995-Task List Explorer-Opening a Size Day to Week Summary task
                                    _ignoreTextChanged = true;
                                    txtOverrideNode.Text = hnp.Text;
                                    _ignoreTextChanged = false;
                                    //END TT#3995-Task List Explorer-Opening a Size Day to Week Summary task
                                    ((MIDTag)(txtOverrideNode.Tag)).MIDTagData = hnp;
								}
							}
							if (midDateRangeSizeDayWeekSum.DateRangeRID != Include.UndefinedCalendarDateRange)
							{
								DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(midDateRangeSizeDayWeekSum.DateRangeRID);
								midDateRangeSizeDayWeekSum.Text = drp.DisplayDate;
							}
							
                            //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                            ShowTab(tabKeys.tabSizeDayToWeekSummary, taskText, emailTabSelected);
                            //pnlSizeDayToWeekSummary.Visible = true;
                            //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
							break;
						//End TT#391 - STodd - Size day to week summary
						//Begin MOD - JScott - Build Pack Criteria Load
						case eTaskType.BuildPackCriteriaLoad :
						//End MOD - JScott - Build Pack Criteria Load
						case eTaskType.ColorCodeLoad:
						case eTaskType.HeaderLoad :
						case eTaskType.HierarchyLoad :
						case eTaskType.HistoryPlanLoad :
						case eTaskType.SizeCodeLoad :
						case eTaskType.SizeCurveLoad :
						case eTaskType.SizeConstraintsLoad :
						case eTaskType.StoreLoad :
                        case eTaskType.DailyPercentagesCriteriaLoad:     // TT#43 - MD - DOConnell - Projected Sales Enhancement //TT#816 - MD - DOConnell - Corrected misspelling
                        //BEGIN TT#820 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Add Task List Options for VSW and Store Eligibility Load processes
                        case eTaskType.StoreEligibilityCriteriaLoad:
                        case eTaskType.VSWCriteriaLoad:
                        //END TT#820 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Add Task List Options for VSW and Store Eligibility Load processes
                        case eTaskType.ChainSetPercentCriteriaLoad:    // TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
                      
							//Begin MOD - JScott - Build Pack Criteria Load
							nudPostingConcurrent.Visible = true;
                            nudPostingConcurrent.Enabled = true;	// TT#1581-MD - stodd - API Header Reconcile
							lblPostingConcurrentFiles.Visible = true;
							chkPostingRunUntil.Visible = true;
							txtPostingRunUntilMask.Visible = true;
							lblPostingNote.Text = string.Empty;

							//End MOD - JScott - Build Pack Criteria Load

                            // Begin TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files 
                            cboProcessingDirection.DataSource = MIDText.GetTextType(eMIDTextType.eAPIFileProcessingDirection, eMIDTextOrderBy.TextCode); 
                            cboProcessingDirection.ValueMember = "TEXT_CODE";
                            cboProcessingDirection.DisplayMember = "TEXT_VALUE";
                            // End TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files 

							switch ((eTaskType)Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_TYPE"].Value))
							{
								//Begin MOD - JScott - Build Pack Criteria Load
								case eTaskType.BuildPackCriteriaLoad :

									lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.BuildPackCriteriaLoad);
									nudPostingConcurrent.Visible = false;
									lblPostingConcurrentFiles.Visible = false;
									chkPostingRunUntil.Visible = false;
									txtPostingRunUntilMask.Visible = false;
									break;

								//End MOD - JScott - Build Pack Criteria Load
								case eTaskType.ColorCodeLoad :

									lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.ColorCodeLoad);
									break;

								// Begin TT#1581-MD - stodd - API Header Reconcile
                                //case eTaskType.HeaderReconcile:

                                //    lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.HeaderReconcile);
                                //    nudPostingConcurrent.Value = 1;
                                //    nudPostingConcurrent.Enabled = false;
                                //    cboProcessingDirection.Enabled = false;
                                //    chkPostingRunUntil.Visible = false;
                                //    txtPostingRunUntilMask.Visible = false;
                                //    break;
								// End TT#1581-MD - stodd - API Header Reconcile
								
								case eTaskType.HeaderLoad :

									lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.HeaderLoad);
									break;

								case eTaskType.HierarchyLoad :

									lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.HierarchyLoad);
									break;

                                // BEGIN TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
                                case eTaskType.ChainSetPercentCriteriaLoad:

                                    lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.ChainSetPercentCriteriaLoad);
                                    break;
                                // END TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4

                                // BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                                case eTaskType.DailyPercentagesCriteriaLoad: //TT#816 - MD - DOConnell - Corrected misspelling

                                    lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.DailyPercentagesCriteriaLoad); //TT#816 - MD - DOConnell - Corrected misspelling
                                    break;
                                // END TT#43 - MD - DOConnell - Projected Sales Enhancement
                               case eTaskType.HistoryPlanLoad :

									lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.HistoryPlanLoad);
									break;

								case eTaskType.SizeCodeLoad :

									lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.SizeCodeLoad);
									break;

								case eTaskType.SizeCurveLoad :

									lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.SizeCurveLoad);
									lblPostingNote.Text = MIDText.GetTextOnly((int)eMIDTextCode.msg_SizeCurvePanelNote1);
									lblPostingNote.Text += MIDText.GetTextOnly((int)eMIDTextCode.msg_SizeCurvePanelNote2);
									break;

								case eTaskType.SizeConstraintsLoad :

									lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.SizeConstraintsLoad);
									break;

								case eTaskType.StoreLoad :

									lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.StoreLoad);
									break;
                                //BEGIN TT#820 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Add Task List Options for VSW and Store Eligibility Load processes
                                case eTaskType.StoreEligibilityCriteriaLoad:

                                    lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.StoreEligibilityCriteriaLoad);
                                    break;
                                case eTaskType.VSWCriteriaLoad:

                                    lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.VSWCriteriaLoad);
                                    break;
                                //END TT#820 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Add Task List Options for VSW and Store Eligibility Load processes
							}

							_currentPostingRow = _dtTaskPosting.Rows.Find(new object[] { Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value), Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value) });

							if (_currentPostingRow == null)
							{
								_currentPostingRow = _dtTaskPosting.NewRow();
								_currentPostingRow["TASKLIST_RID"] = ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value;
								_currentPostingRow["TASK_SEQUENCE"] = ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value;
								_currentPostingRow["CONCURRENT_FILES"] = 1;
								_currentPostingRow["RUN_UNTIL_FILE_PRESENT_IND"] = '0';
                                _currentPostingRow["FILE_PROCESSING_DIRECTION"] = eAPIFileProcessingDirection.Config.GetHashCode(); // TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files
								_dtTaskPosting.Rows.Add(_currentPostingRow);
								_dtTaskPosting.AcceptChanges();
							}

							txtPostingDirectory.Text = Convert.ToString(_currentPostingRow["INPUT_DIRECTORY"]);
							txtPostingMask.Text = Convert.ToString(_currentPostingRow["FILE_MASK"]);
							nudPostingConcurrent.Value = Convert.ToInt32(_currentPostingRow["CONCURRENT_FILES"]);

							if (Convert.ToChar(_currentPostingRow["RUN_UNTIL_FILE_PRESENT_IND"]) == '1')
							{
								chkPostingRunUntil.Checked = true;
							}
							else
							{
								chkPostingRunUntil.Checked = false;
							}

							if (_currentPostingRow["RUN_UNTIL_FILE_MASK"] != System.DBNull.Value)
							{
								txtPostingRunUntilMask.Text = Convert.ToString(_currentPostingRow["RUN_UNTIL_FILE_MASK"]);
							}

                            // Begin TT#1581-MD - stodd Header Reconcile 
                            // Header Reconcile is always FIFO.
                            //if ((eTaskType)Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_TYPE"].Value) == eTaskType.HeaderReconcile)
                            //{
                            //    cboProcessingDirection.SelectedValue = (int)eAPIFileProcessingDirection.FIFO;
                            //}
                            //else
                            //{
                                // Begin TT#5116 - JSmith - Header Load Task Processing Direction Parameter Update Not Sticking 
                                cboProcessingDirection.SelectedValue = Convert.ToInt32(_currentPostingRow["FILE_PROCESSING_DIRECTION"]);  // TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files
                                // End TT#5116 - JSmith - Header Load Task Processing Direction Parameter Update Not Sticking 
                            //}
                            // End TT#1581-MD - stodd Header Reconcile 

                            // Begin TT#1314-MD - JSmith - Tasklist allows multiple concurrent files with FIFO/FILO processing
                            if ((eAPIFileProcessingDirection)cboProcessingDirection.SelectedValue == eAPIFileProcessingDirection.Config &&
                                (_FileProcessingDirection == eAPIFileProcessingDirection.FIFO ||
                                _FileProcessingDirection == eAPIFileProcessingDirection.FILO))
                            {
                                if (nudPostingConcurrent.Value != 1)
                                {
                                    MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConcurrentProcessesMustBe1Override, false));
                                    nudPostingConcurrent.Value = 1;
                                }
                                nudPostingConcurrent.Enabled = false;
                            }
                            else if ((eAPIFileProcessingDirection)cboProcessingDirection.SelectedValue == eAPIFileProcessingDirection.FIFO ||
                                (eAPIFileProcessingDirection)cboProcessingDirection.SelectedValue == eAPIFileProcessingDirection.FILO)
                            {
                                nudPostingConcurrent.Enabled = false;
                            }
                            // End TT#1314-MD - JSmith - Tasklist allows multiple concurrent files with FIFO/FILO processing
							
                            //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                            ShowTab(tabKeys.tabPosting, taskText, emailTabSelected); //lblPosting.Text;
                            //pnlPosting.Visible = true;
                            //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
							break;

						case eTaskType.RelieveIntransit :

							lblRelieve.Text = MIDText.GetTextOnly((int)eTaskType.RelieveIntransit);

							_currentPostingRow = _dtTaskPosting.Rows.Find(new object[] { Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value), Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value) });

							if (_currentPostingRow == null)
							{
								_currentPostingRow = _dtTaskPosting.NewRow();
								_currentPostingRow["TASKLIST_RID"] = ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value;
								_currentPostingRow["TASK_SEQUENCE"] = ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value;
								_currentPostingRow["CONCURRENT_FILES"] = 1;
								_currentPostingRow["RUN_UNTIL_FILE_PRESENT_IND"] = '0';
								_dtTaskPosting.Rows.Add(_currentPostingRow);
								_dtTaskPosting.AcceptChanges();
							}

							txtRelieveDirectory.Text = Convert.ToString(_currentPostingRow["INPUT_DIRECTORY"]);
							txtRelieveMask.Text = Convert.ToString(_currentPostingRow["FILE_MASK"]);

							if (Convert.ToChar(_currentPostingRow["RUN_UNTIL_FILE_PRESENT_IND"]) == '1')
							{
								chkRelieveRunUntil.Checked = true;
							}
							else
							{
								chkRelieveRunUntil.Checked = false;
							}

							if (_currentPostingRow["RUN_UNTIL_FILE_MASK"] != System.DBNull.Value)
							{
								txtRelieveRunUntilMask.Text = Convert.ToString(_currentPostingRow["RUN_UNTIL_FILE_MASK"]);
							}

							
                            //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                            ShowTab(tabKeys.tabRelieveIntransit, taskText, emailTabSelected); 
                            //pnlRelieve.Visible = true;
                            //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
							break;

						case eTaskType.ExternalProgram :

							_currentProgramRow = _dtTaskProgram.Rows.Find(new object[] { Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value), Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value) });

							if (_currentProgramRow == null)
							{
								_currentProgramRow = _dtTaskProgram.NewRow();
								_currentProgramRow["TASKLIST_RID"] = ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value;
								_currentProgramRow["TASK_SEQUENCE"] = ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value;
								_dtTaskProgram.Rows.Add(_currentProgramRow);
								_dtTaskProgram.AcceptChanges();
							}

							txtProgramPath.Text = Convert.ToString(_currentProgramRow["PROGRAM_PATH"]);
							txtProgramParms.Text = Convert.ToString(_currentProgramRow["PROGRAM_PARMS"]);

							
                            //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                            ShowTab(tabKeys.tabExternalProgram, taskText, emailTabSelected); 
                            //pnlProgram.Visible = true;
                            //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
							break;


                        case eTaskType.HeaderReconcile:	// TT#1581-MD - stodd - API Header Reconcile
                            
                            lblPosting.Text = MIDText.GetTextOnly((int)eTaskType.HeaderReconcile);
                            lblHRInputDirectory.Text = "Input Directory:";
                            lblHROutputDirectory.Text = "Output Directory:";
                            lblHRTriggerSuffix.Text = "Trigger Suffix:";
                            lblHRHeaderKeysFile.Text = "Header Keys File Name:";
                            lblHRHeaderTypes.Text = "Header Types:";
                            lblHRRemoveTransactionsFileName.Text = "Remove Transactions File Name:";
                            lblHRRemoveTransactionsTriggerSuffix.Text = "Remove Transactions Trigger Suffix:";

                            // Load from DB
                            _currentHeaderReconcileRow = _dtTaskHeaderReconcile.Rows.Find(new object[] { Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value), Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value) });

                            if (_currentHeaderReconcileRow == null)
							{
                                _currentHeaderReconcileRow = _dtTaskHeaderReconcile.NewRow();
                                _currentHeaderReconcileRow["TASKLIST_RID"] = ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value;
                                _currentHeaderReconcileRow["TASK_SEQUENCE"] = ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value;
                                _dtTaskHeaderReconcile.Rows.Add(_currentHeaderReconcileRow);
                                _dtTaskHeaderReconcile.AcceptChanges();
							}

                            txtHeaderReconcileInputDirectory.Text = Convert.ToString(_currentHeaderReconcileRow["INPUT_DIRECTORY"]);
                            txtHeaderReconcileOutputDirectory.Text = Convert.ToString(_currentHeaderReconcileRow["OUTPUT_DIRECTORY"]);
                            txtHeaderReconcileTriggerSuffix.Text = Convert.ToString(_currentHeaderReconcileRow["TRIGGER_SUFFIX"]);
                            txtHeaderReconcileRemoveTransactionsFileName.Text = Convert.ToString(_currentHeaderReconcileRow["REMOVE_TRANS_FILE_NAME"]);
                            txtHeaderReconcileRemoveTransactionsTriggerSuffix.Text = Convert.ToString(_currentHeaderReconcileRow["REMOVE_TRANS_TRIGGER_SUFFIX"]);
                            txtHeaderReconcileHeaderKeys.Text = Convert.ToString(_currentHeaderReconcileRow["HEADER_KEYS_FILE_NAME"]);

                            //Load from DB header type
                            LoadHeaderTypes();

                            // set checked Header Types
                            List<string> headerTypeArray = null;
                            string headerTypes = string.Empty;
                            if (_currentHeaderReconcileRow["HEADER_TYPES"] != DBNull.Value)
                            {
                                headerTypes = _currentHeaderReconcileRow["HEADER_TYPES"].ToString();
                            }

                            if (headerTypes != string.Empty)
                            {
                                headerTypeArray = MIDstringTools.SplitGeneric(headerTypes, ',', true);
                            }
                            else
                            {
                                headerTypeArray = new List<string>();
                            }
                            for (int i = 0; i < headerTypeArray.Count; i++)
                            {
                                string aHeaderType = headerTypeArray[i];
                                switch (aHeaderType.ToUpper())
                                {
                                    case "PO":
                                        aHeaderType = "Purchase Order";
                                        break;

                                    case "DROPSHIP":
                                        aHeaderType = "Drop Ship";
                                        break;

                                    case "WORKUPTOTALBUY":
                                        aHeaderType = "Workup Total Buy";
                                        break;
                                }

                                int lInt = this.clbHeaderReconcileHeaderTypes.FindStringExact(aHeaderType, 0);
				                if (lInt >= 0)
				                {
                                    clbHeaderReconcileHeaderTypes.SetSelected(lInt, true);
                                    clbHeaderReconcileHeaderTypes.SetItemCheckState(lInt, CheckState.Checked);
				                }
                            }

                            ShowTab(tabKeys.tabHeaderReconcile, taskText, emailTabSelected);


                            break;
						// Begin TT#1595-MD - stodd - Batch Comp
                        case eTaskType.BatchComp:

                            DataTable dt = MIDText.GetTextType(eMIDTextType.eBatchComp, eMIDTextOrderBy.TextCode);
                            cboBatchComp.DataSource = dt;
                            cboBatchComp.ValueMember = "TEXT_CODE";
                            cboBatchComp.DisplayMember = "TEXT_VALUE";


                            _currentBatchCompRow = _dtTaskBatchComp.Rows.Find(new object[] { Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value), Convert.ToInt32(ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value) });

                            if (_currentBatchCompRow == null)
                            {
                                _currentBatchCompRow = _dtTaskBatchComp.NewRow();
                                _currentBatchCompRow["TASKLIST_RID"] = ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value;
                                _currentBatchCompRow["TASK_SEQUENCE"] = ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value;
                                _currentBatchCompRow["BATCH_COMP_RID"] = eBatchComp.All;
                                _dtTaskBatchComp.Rows.Add(_currentBatchCompRow);
                                _dtTaskBatchComp.AcceptChanges();
                            }

                            cboBatchComp.SelectedValue = int.Parse(_currentBatchCompRow["BATCH_COMP_RID"].ToString());
                            ShowTab(tabKeys.tabBatchComp, taskText, emailTabSelected);
                            break;
						// End TT#1595-MD - stodd - Batch Comp
					}
                    //ShowEmailTab(); //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
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
					SaveTaskListValues(aUpdateMode);
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
			int newUserRID;
			//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			int newOwnerRID;
			//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			DataRow postingRow;
            DataRow batchCompRow;		// TT#1595-MD - stodd - Batch Comp
            DataRow headerReconcileRow;		// TT#1581-MD - stodd - header reconcile
			DataRow programRow;
			//Begin Track #6423 - JScott - Add Warning when saving task lists
			DialogResult retCode;
			//End Track #6423 - JScott - Add Warning when saving task lists

			try
			{
				_dtTaskPosting.AcceptChanges();
                _dtTaskBatchComp.AcceptChanges();	// TT#1595-MD - stodd - Batch Comp
                _dtTaskHeaderReconcile.AcceptChanges();	// TT#1581-MD - stodd - header reconcile
				_dtTaskProgram.AcceptChanges();

				newUserRID = GetNewUserRID();
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				newOwnerRID = GetNewOwnerRID();
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

				// TT#391 - stodd - size day to week summary
				_dtTaskSizeDayToWeekSummary.AcceptChanges();
				// TT#391 - stodd - size day to week summary

				if (rdoUser.Checked && !_userSecLvl.AllowUpdate ||
					rdoGlobal.Checked && !_globalSecLvl.AllowUpdate ||
					rdoSystem.Checked && !_systemSecLvl.AllowUpdate)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				if (txtTaskListName.Text.Trim().Length == 0)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TaskListNameRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

                // Begin TT#3993 - JSmith - Task File-SaveAs does not work
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				//if (_taskListProf.Key == Include.NoRID || aUpdateMode == eUpdateMode.Create || _taskListProf.Name != txtTaskListName.Text || _taskListProf.UserRID != newUserRID)
                //if (_taskListProf.Key == Include.NoRID ||
                //    aUpdateMode == eUpdateMode.Create ||
                //    _taskListProf.Name != txtTaskListName.Text ||
                //    _taskListProf.UserRID != newUserRID ||
                //    _taskListProf.OwnerUserRID != newOwnerRID)
                ////End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //{
                //    if (_dlSchedule.TaskList_GetKey(txtTaskListName.Text.Trim(), newUserRID) != -1)
                //    {
                //        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TaskListNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return false;
                //    }
                //}
				// Begin TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.
                if (((_taskListProf.Key == Include.NoRID || aUpdateMode == eUpdateMode.Update) && _taskListProf.Name != txtTaskListName.Text) || _taskListProf.UserRID != newUserRID)
                {
                    if (_dlSchedule.TaskList_GetKey(txtTaskListName.Text.Trim(), newUserRID) != -1)
                    {
                        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TaskListNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
				// End TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.
				// End TT#3993 - JSmith - Task File-SaveAs does not work

				if (ulgTasks.Rows.Count == 0)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneTaskRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

                ////Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                //DataRow[] drTask = _dtTask.Select("TASKLIST_RID=" + (int)ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value + " AND TASK_SEQUENCE=" + (int)ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value);
                //this.emailTaskList1.SaveToDataRow(drTask[0]);
                //_dtTask.AcceptChanges();
                ////End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
				foreach (UltraGridRow taskRow in ulgTasks.Rows)
				{
                    

					switch ((eTaskType)Convert.ToInt32(taskRow.Cells["TASK_TYPE"].Value))
					{
						case eTaskType.Allocate :

							BindAllocateGridData((DataSetEntry)_allocateDataSetList[Convert.ToInt32(taskRow.Cells["TASK_SEQUENCE"].Value)]);

							if (ulgAllocate.Rows.Count == 0)
							{
								taskRow.Activate();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneMerchandiseRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
							else
							{
								foreach (UltraGridRow allocateRow in ulgAllocate.Rows)
								{
                                    if ((int)allocateRow.Cells["Filter"].Value == Include.NoRID)  //TT#1313-MD -jsobek -Header Filters //TT#1481-MD -jsobek -Task List requires Merchandise
										//allocateRow.Cells["ALLOCATE_TYPE"].Value == System.DBNull.Value && //TT#1313-MD -jsobek -Header Filters
										//allocateRow.Cells["HEADER_ID"].Value == System.DBNull.Value && //TT#1313-MD -jsobek -Header Filters
										//allocateRow.Cells["PO_ID"].Value == System.DBNull.Value) //TT#1313-MD -jsobek -Header Filters
									{
										taskRow.Activate();
                                        allocateRow.Cells["Filter"].Activate();
										MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneNodeDescriptorRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
										return false;
									}

									//Begin Track #6423 - JScott - Add Warning when saving task lists
                                    //if (allocateRow.Cells["HN_RID"].Value == System.DBNull.Value && (int)allocateRow.Cells["Filter"].Value == Include.NoRID) //&& //TT#1313-MD -jsobek -Header Filters
                                    //    //allocateRow.Cells["HEADER_ID"].Value == System.DBNull.Value && //TT#1313-MD -jsobek -Header Filters
                                    //    //allocateRow.Cells["PO_ID"].Value == System.DBNull.Value) //TT#1313-MD -jsobek -Header Filters
                                    //{
                                    //    taskRow.Activate();
                                    //    allocateRow.Cells["HN_RID"].Activate();
                                    //    retCode = MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneNodeDescriptorRecommended), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                                    //    if (retCode == DialogResult.No)
                                    //    {
                                    //        return false;
                                    //    }
                                    //}

									//End Track #6423 - JScott - Add Warning when saving task lists
									if (allocateRow.ChildBands[0].Rows.Count == 0)
									{
										taskRow.Activate();
										allocateRow.Activate();
										MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneWorkflowRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
										return false;
									}

									foreach (UltraGridRow allocateDetailRow in allocateRow.ChildBands[0].Rows)
									{
										if (allocateDetailRow.Cells["WORKFLOW_RID"].Value == System.DBNull.Value)
										{
											taskRow.Activate();
											allocateDetailRow.Cells["WORKFLOW_RID"].Activate();
											MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_WorkflowRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
											return false;
										}
									}
								}
							}

							break;

						case eTaskType.Forecasting :

							BindForecastGridData((DataSetEntry)_forecastDataSetList[Convert.ToInt32(taskRow.Cells["TASK_SEQUENCE"].Value)]);

							if (ulgForecast.Rows.Count == 0)
							{
								taskRow.Activate();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneMerchandiseRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
							else
							{
								foreach (UltraGridRow forecastRow in ulgForecast.Rows)
								{
									if (forecastRow.ChildBands[0].Rows.Count == 0)
									{
										taskRow.Activate();
										forecastRow.Activate();
										MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneMethodRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
										return false;
									}

									foreach (UltraGridRow forecastDetailRow in forecastRow.ChildBands[0].Rows)
									{
										if (forecastDetailRow.Cells["WORKFLOW_METHOD_IND"].Value == System.DBNull.Value ||
											(forecastDetailRow.Cells["METHOD_RID"].Value == System.DBNull.Value &&
											forecastDetailRow.Cells["WORKFLOW_RID"].Value == System.DBNull.Value))
										{
											taskRow.Activate();
											forecastDetailRow.Cells["ForecastWorkflowMethod"].Activate();
											MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_WorkflowOrMethodRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
											return false;
										}
									}
								}
							}

							break;

						case eTaskType.computationDriver :
							break;

						case eTaskType.Rollup :

							BindRollupGridData((DataTableEntry)_rollupDataTableList[Convert.ToInt32(taskRow.Cells["TASK_SEQUENCE"].Value)]);

							if (ulgRollup.Rows.Count == 0)
							{
								taskRow.Activate();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneMerchandiseRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
							else
							{
								foreach (UltraGridRow rollupRow in ulgRollup.Rows)
								{
                                    if (Convert.ToBoolean(rollupRow.Cells["Hierarchy Levels"].Value) ||
                                        Convert.ToBoolean(rollupRow.Cells["Day To Week"].Value) ||
                                        Convert.ToBoolean(rollupRow.Cells["Day"].Value) ||
                                        Convert.ToBoolean(rollupRow.Cells["Week"].Value) ||
                                        Convert.ToBoolean(rollupRow.Cells["Store"].Value) ||
                                        Convert.ToBoolean(rollupRow.Cells["Chain"].Value) ||
                                        Convert.ToBoolean(rollupRow.Cells["Store To Chain"].Value) ||
                                        Convert.ToBoolean(rollupRow.Cells["Intransit"].Value))
                                    {

                                        if (rollupRow.Cells["HN_RID"].Value == System.DBNull.Value)
                                        {
                                            taskRow.Activate();
                                            rollupRow.Cells["HN_RID"].Activate();
                                            MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MerchandiseRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return false;
                                        }

                                        if (rollupRow.Cells["FV_RID"].Value == System.DBNull.Value)
                                        {
                                            taskRow.Activate();
                                            rollupRow.Cells["FV_RID"].Activate();
                                            MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VersionRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return false;
                                        }

                                        if (rollupRow.Cells["ROLLUP_CDR_RID"].Value == System.DBNull.Value)
                                        {
                                            taskRow.Activate();
                                            rollupRow.Cells["ROLLUP_CDR_RID"].Activate();
                                            MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RollupDateRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return false;
                                        }

                                        if (rollupRow.Cells["From Level"].Value == System.DBNull.Value)
                                        {
                                            taskRow.Activate();
                                            rollupRow.Cells["From Level"].Activate();
                                            MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RollupFromLevelRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return false;
                                        }
                                	//BEGIN TT#4791 - DOConnell - Rollup Tasklist - able to save tasklist when a 'To Level' is not selected
                                        if (rollupRow.Cells["To Level"].Value == System.DBNull.Value)
                                        {
                                            taskRow.Activate();
                                            rollupRow.Cells["To Level"].Activate();
                                            MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RollupToLevelRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (rollupRow.Cells["HN_RID"].Value != System.DBNull.Value)
                                        {
                                            if (rollupRow.Cells["From Level"].Value == System.DBNull.Value)
                                            {
                                                taskRow.Activate();
                                                rollupRow.Cells["From Level"].Activate();
                                                MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RollupFromLevelRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                return false;
                                            }
                                            
                                            if (rollupRow.Cells["To Level"].Value == System.DBNull.Value)
                                            {
                                                taskRow.Activate();
                                                rollupRow.Cells["To Level"].Activate();
                                                MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RollupToLevelRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                return false;
                                            }
                                        }
                                    }
									//END TT#4791 - DOConnell - Rollup Tasklist - able to save tasklist when a 'To Level' is not selected
								}
							}
						
							break;

						//Begin TT#155 - JScott - Size Curve Method
						case eTaskType.SizeCurveMethod:

							BindSizeCurveMethodGridData((DataTableEntry)_sizeCurveMethodDataTableList[Convert.ToInt32(taskRow.Cells["TASK_SEQUENCE"].Value)]);

							if (ulgSizeCurveMethod.Rows.Count == 0)
							{
								taskRow.Activate();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneMethodRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
							else
							{
								foreach (UltraGridRow sizeCurveMethodRow in ulgSizeCurveMethod.Rows)
								{
									if (sizeCurveMethodRow.Cells["METHOD_RID"].Value == System.DBNull.Value)
									{
										taskRow.Activate();
										sizeCurveMethodRow.Cells["SizeCurveMethod"].Activate();
										MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MethodRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
										return false;
									}
								}
							}

							break;

						//End TT#155 - JScott - Size Curve Method
						//Begin TT#155 - JScott - Add Size Curve info to Node Properties
						case eTaskType.SizeCurves:

							BindSizeCurvesGridData((DataTableEntry)_sizeCurvesDataTableList[Convert.ToInt32(taskRow.Cells["TASK_SEQUENCE"].Value)]);

							if (ulgSizeCurves.Rows.Count == 0)
							{
								taskRow.Activate();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneMerchandiseRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
							else
							{
								foreach (UltraGridRow sizeCurvesRow in ulgSizeCurves.Rows)
								{
									if (sizeCurvesRow.Cells["HN_RID"].Value == System.DBNull.Value)
									{
										taskRow.Activate();
										sizeCurvesRow.Cells["SizeCurvesMerchandise"].Activate();
										MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MerchandiseRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
										return false;
									}
								}
							}

							break;

						//End TT#155 - JScott - Add Size Curve info to Node Properties
						case eTaskType.ColorCodeLoad:
						case eTaskType.HeaderLoad :
						case eTaskType.HierarchyLoad :
						case eTaskType.HistoryPlanLoad :
						case eTaskType.SizeCodeLoad :
						case eTaskType.SizeCurveLoad :
						case eTaskType.StoreLoad :
                        case eTaskType.DailyPercentagesCriteriaLoad:     // TT#43 - MD - DOConnell - Projected Sales Enhancement //TT#816 - MD - DOConnell - Corrected misspelling
                        //BEGIN TT#820 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Add Task List Options for VSW and Store Eligibility Load processes
                        case eTaskType.StoreEligibilityCriteriaLoad:
                        case eTaskType.VSWCriteriaLoad:
                        //END TT#820 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Add Task List Options for VSW and Store Eligibility Load processes
                        case eTaskType.ChainSetPercentCriteriaLoad:    // TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
                        
							postingRow = _dtTaskPosting.Rows.Find(new object[] { Convert.ToInt32(taskRow.Cells["TASKLIST_RID"].Value), Convert.ToInt32(taskRow.Cells["TASK_SEQUENCE"].Value) });

							if (postingRow["INPUT_DIRECTORY"] == System.DBNull.Value || Convert.ToString(postingRow["INPUT_DIRECTORY"]).Trim().Length == 0)
							{
								taskRow.Activate();
								txtPostingDirectory.Focus();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InputDirectoryRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
							else if (Convert.ToString(postingRow["INPUT_DIRECTORY"]).Trim().Length > 250)
							{
								taskRow.Activate();
								txtPostingMask.Focus();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InputDirectoryTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}

							if (postingRow["FILE_MASK"] == System.DBNull.Value || Convert.ToString(postingRow["FILE_MASK"]).Trim().Length == 0)
							{
								taskRow.Activate();
								txtPostingMask.Focus();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FileSuffixRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
							else if (!CheckMaskForValidCharacters(Convert.ToString(postingRow["FILE_MASK"])))
							{
								taskRow.Activate();
								txtPostingMask.Focus();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharactersInSuffix), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
							else if (Convert.ToString(postingRow["FILE_MASK"]).Trim().Length > 50)
							{
								taskRow.Activate();
								txtPostingMask.Focus();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FileSuffixTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}

							if (Convert.ToChar(postingRow["RUN_UNTIL_FILE_PRESENT_IND"]) == '1')
							{
								if (postingRow["RUN_UNTIL_FILE_MASK"] == System.DBNull.Value || Convert.ToString(postingRow["RUN_UNTIL_FILE_MASK"]).Trim().Length == 0)
								{
									taskRow.Activate();
									txtPostingRunUntilMask.Focus();
									MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RunUntilSuffixRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
									return false;
								}
								else if (!CheckMaskForValidCharacters(Convert.ToString(postingRow["RUN_UNTIL_FILE_MASK"])))
								{
									taskRow.Activate();
									txtPostingRunUntilMask.Focus();
									MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharactersInSuffix), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
									return false;
								}
								else if (Convert.ToString(postingRow["RUN_UNTIL_FILE_MASK"]).Trim().Length > 50)
								{
									taskRow.Activate();
									txtPostingMask.Focus();
									MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RunUntilSuffixTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
									return false;
								}
							}
							break;

						case eTaskType.RelieveIntransit :

							postingRow = _dtTaskPosting.Rows.Find(new object[] { Convert.ToInt32(taskRow.Cells["TASKLIST_RID"].Value), Convert.ToInt32(taskRow.Cells["TASK_SEQUENCE"].Value) });

							if (postingRow["INPUT_DIRECTORY"] == System.DBNull.Value || Convert.ToString(postingRow["INPUT_DIRECTORY"]).Trim().Length == 0)
							{
								taskRow.Activate();
								txtRelieveDirectory.Focus();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InputDirectoryRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
							else if (Convert.ToString(postingRow["INPUT_DIRECTORY"]).Trim().Length > 250)
							{
								taskRow.Activate();
								txtPostingMask.Focus();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InputDirectoryTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}

							if (postingRow["FILE_MASK"] == System.DBNull.Value || Convert.ToString(postingRow["FILE_MASK"]).Trim().Length == 0)
							{
								taskRow.Activate();
								txtRelieveMask.Focus();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FileSuffixRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
							else if (!CheckMaskForValidCharacters(Convert.ToString(postingRow["FILE_MASK"])))
							{
								taskRow.Activate();
								txtRelieveMask.Focus();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharactersInSuffix), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
							else if (Convert.ToString(postingRow["FILE_MASK"]).Trim().Length > 50)
							{
								taskRow.Activate();
								txtPostingMask.Focus();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FileSuffixTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}

							if (Convert.ToChar(postingRow["RUN_UNTIL_FILE_PRESENT_IND"]) == '1')
							{
								if ((postingRow["RUN_UNTIL_FILE_MASK"] == System.DBNull.Value || Convert.ToString(postingRow["RUN_UNTIL_FILE_MASK"]).Trim().Length == 0))
								{
									taskRow.Activate();
									txtRelieveRunUntilMask.Focus();
									MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RunUntilSuffixRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
									return false;
								}
								else
									if (!CheckMaskForValidCharacters(Convert.ToString(postingRow["RUN_UNTIL_FILE_MASK"])))
								{
									taskRow.Activate();
									txtRelieveRunUntilMask.Focus();
									MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharactersInSuffix), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
									return false;
								}
								else if (Convert.ToString(postingRow["RUN_UNTIL_FILE_MASK"]).Trim().Length > 50)
								{
									taskRow.Activate();
									txtPostingMask.Focus();
									MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RunUntilSuffixTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
									return false;
								}
							}
							break;

						case eTaskType.ExternalProgram :

							programRow = _dtTaskProgram.Rows.Find(new object[] { Convert.ToInt32(taskRow.Cells["TASKLIST_RID"].Value), Convert.ToInt32(taskRow.Cells["TASK_SEQUENCE"].Value) });

							if (programRow["PROGRAM_PATH"] == System.DBNull.Value)
							{
								taskRow.Activate();
								txtRelieveDirectory.Focus();
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ProgramPathRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return false;
							}
							break;


						// Begin TT#1595-MD - stodd - Batch Comp
                        case eTaskType.BatchComp:

                            //programRow = _dtTaskBatchComp.Rows.Find(new object[] { Convert.ToInt32(taskRow.Cells["TASKLIST_RID"].Value), Convert.ToInt32(taskRow.Cells["TASK_SEQUENCE"].Value) });
                            DataTable dt = (DataTable)cboBatchComp.DataSource;
                            if (dt.Rows.Count == 1)
                            {
                                taskRow.Activate();
                                txtRelieveDirectory.Focus();
                                MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoBatchCompsDefined), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            break;
						// End TT#1595-MD - stodd - Batch Comp

                        case eTaskType.HeaderReconcile:

                            headerReconcileRow = _dtTaskHeaderReconcile.Rows.Find(new object[] { Convert.ToInt32(taskRow.Cells["TASKLIST_RID"].Value), Convert.ToInt32(taskRow.Cells["TASK_SEQUENCE"].Value) });

                            if (headerReconcileRow["INPUT_DIRECTORY"] == System.DBNull.Value)
                            {
                                taskRow.Activate();
                                this.txtHeaderReconcileInputDirectory.Focus();
                                string msg = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
                                msg = msg.Replace("{0}", "Input Directory");
                                MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            else if (Convert.ToString(headerReconcileRow["INPUT_DIRECTORY"]).Trim().Length > 400)
                            {
                                taskRow.Activate();
                                txtHeaderReconcileInputDirectory.Focus();
                                MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InputDirectoryTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                            if (headerReconcileRow["OUTPUT_DIRECTORY"] == System.DBNull.Value)
                            {
                                taskRow.Activate();
                                this.txtHeaderReconcileInputDirectory.Focus();
                                string msg = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
                                // BEGIN TT#1618-MD - AGallagher - Header Reconcile API Displays Wrong Task List Property Message 
                                // msg = msg.Replace("{0}", "Input Directory");
                                msg = msg.Replace("{0}", "Output Directory");
                                // END TT#1618-MD - AGallagher - Header Reconcile API Displays Wrong Task List Property Message 
                                MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            else if (Convert.ToString(headerReconcileRow["OUTPUT_DIRECTORY"]).Trim().Length > 400)
                            {
                                taskRow.Activate();
                                txtHeaderReconcileOutputDirectory.Focus();
                                MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InputDirectoryTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                            if (headerReconcileRow["TRIGGER_SUFFIX"] == System.DBNull.Value || Convert.ToString(headerReconcileRow["TRIGGER_SUFFIX"]).Trim().Length == 0)
                            {
                                taskRow.Activate();
                                this.txtHeaderReconcileTriggerSuffix.Focus();
                                MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FileSuffixRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            else if (!CheckMaskForValidCharacters(Convert.ToString(headerReconcileRow["TRIGGER_SUFFIX"])))
                            {
                                taskRow.Activate();
                                txtHeaderReconcileTriggerSuffix.Focus();
                                MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharactersInSuffix), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            else if (Convert.ToString(headerReconcileRow["TRIGGER_SUFFIX"]).Trim().Length > 50)
                            {
                                taskRow.Activate();
                                txtHeaderReconcileTriggerSuffix.Focus();
                                MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FileSuffixTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                            if (headerReconcileRow["REMOVE_TRANS_FILE_NAME"] == System.DBNull.Value)
                            {
                                taskRow.Activate();
                                this.txtHeaderReconcileRemoveTransactionsFileName.Focus();
                                string msg = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
                                msg = msg.Replace("{0}", "Remove Transactions File Name");
                                MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            else if (Convert.ToString(headerReconcileRow["REMOVE_TRANS_FILE_NAME"]).Trim().Length > 50)
                            {
                                taskRow.Activate();
                                txtHeaderReconcileRemoveTransactionsFileName.Focus();
                                MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InputDirectoryTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                            if (headerReconcileRow["REMOVE_TRANS_TRIGGER_SUFFIX"] == System.DBNull.Value || Convert.ToString(headerReconcileRow["REMOVE_TRANS_TRIGGER_SUFFIX"]).Trim().Length == 0)
                            {
                                taskRow.Activate();
                                this.txtHeaderReconcileRemoveTransactionsTriggerSuffix.Focus();
                                MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FileSuffixRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            else if (!CheckMaskForValidCharacters(Convert.ToString(headerReconcileRow["REMOVE_TRANS_TRIGGER_SUFFIX"])))
                            {
                                taskRow.Activate();
                                txtHeaderReconcileRemoveTransactionsTriggerSuffix.Focus();
                                MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharactersInSuffix), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            else if (Convert.ToString(headerReconcileRow["REMOVE_TRANS_TRIGGER_SUFFIX"]).Trim().Length > 50)
                            {
                                taskRow.Activate();
                                txtHeaderReconcileRemoveTransactionsTriggerSuffix.Focus();
                                MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FileSuffixTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                            if (clbHeaderReconcileHeaderTypes.CheckedItems.Count == 0)
                            {
                                taskRow.Activate();
                                this.clbHeaderReconcileHeaderTypes.Focus();
                                string msg = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
                                msg = msg.Replace("{0}", "At least one selected Header Type");
                                MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                            if (headerReconcileRow["HEADER_KEYS_FILE_NAME"] == System.DBNull.Value)
                            {
                                taskRow.Activate();
                                this.txtHeaderReconcileHeaderKeys.Focus();
                                string msg = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
                                msg = msg.Replace("{0}", "Header Keys Files Name");
                                MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            else if (Convert.ToString(headerReconcileRow["HEADER_KEYS_FILE_NAME"]).Trim().Length > 400)
                            {
                                taskRow.Activate();
                                txtHeaderReconcileHeaderKeys.Focus();
                                MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InputDirectoryTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }


                            break;

					}
                    //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                    DataRow[] drTask2 = _dtTask.Select("TASKLIST_RID=" + (int)taskRow.Cells["TASKLIST_RID"].Value + " AND TASK_SEQUENCE=" + (int)taskRow.Cells["TASK_SEQUENCE"].Value);

                    this.emailTaskList1.LoadFromDataRow(drTask2[0]);
                    if (this.emailTaskList1.IsValid() == false)
                    {
                        taskRow.Activate();
                        this.ultraTabControl1.Tabs[tabKeys.tabEmail].Selected = true;
                        MessageBox.Show("Invalid Email Settings", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private bool CheckMaskForValidCharacters(string aMask)
		{
			char[] invalidChars = { '*' };

			try
			{
				if (aMask.IndexOfAny(invalidChars) >= 0)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SaveTaskListValues(eUpdateMode aUpdateMode)
		{
			//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			TaskListProfile taskListProf;
			//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			int taskSeq = -1;
			int i;
			int j;
			Hashtable taskSeqHash;
			UltraGridChildBand childBand;
			Hashtable newDataList;
			IDictionaryEnumerator hashEnum;
			DataSetEntry dsEntry;
			DataTableEntry dtEntry;
			//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			MIDTaskListNode saveToNode;
			int ownerRID;
			int userRID;
			//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			//Begin TT#1330 - JScott - Reordering tasks in tasklist results in error
			//End TT#1330 - JScott - Reordering tasks in tasklist results in error

			try
			{
				_dlSchedule.OpenUpdateConnection();

				try
				{
					ulgForecast.PerformAction(UltraGridAction.ExitEditMode);
					ulgAllocate.PerformAction(UltraGridAction.ExitEditMode);
					ulgRollup.PerformAction(UltraGridAction.ExitEditMode);
					//Begin TT#155 - JScott - Size Curve Method
					ulgSizeCurveMethod.PerformAction(UltraGridAction.ExitEditMode);
					//End TT#155 - JScott - Size Curve Method
					//Begin TT#155 - JScott - Add Size Curve info to Node Properties
					ulgSizeCurves.PerformAction(UltraGridAction.ExitEditMode);
					//End TT#155 - JScott - Add Size Curve info to Node Properties
					ulgTasks.PerformAction(UltraGridAction.ExitEditMode);

					ulgForecast.UpdateData();
					ulgAllocate.UpdateData();
					ulgRollup.UpdateData();
					//Begin TT#155 - JScott - Size Curve Method
					ulgSizeCurveMethod.UpdateData();
					//End TT#155 - JScott - Size Curve Method
					//Begin TT#155 - JScott - Add Size Curve info to Node Properties
					ulgSizeCurves.UpdateData();
					//End TT#155 - JScott - Add Size Curve info to Node Properties
					ulgTasks.UpdateData();

					ForecastAcceptChanges();
					AllocateAcceptChanges();
					RollupAcceptChanges();
					//Begin TT#155 - JScott - Size Curve Method
					SizeCurveMethodAcceptChanges();
					//End TT#155 - JScott - Size Curve Method
					//Begin TT#155 - JScott - Add Size Curve info to Node Properties
					SizeCurvesAcceptChanges();
					//Begin TT#707 - JScott - Size Curve process needs to multi-thread
					_dtTaskSizeCurvesNode.AcceptChanges();
					//End TT#707 - JScott - Size Curve process needs to multi-thread
					//End TT#155 - JScott - Add Size Curve info to Node Properties
					_dtTaskPosting.AcceptChanges();
                    _dtTaskBatchComp.AcceptChanges();	// TT#1595-MD - stodd - Batch Comp
                    _dtTaskHeaderReconcile.AcceptChanges();	// TT#1581-MD - stodd - Header Reconcile
					_dtTaskProgram.AcceptChanges();
					_dtTask.AcceptChanges();

					//Begin TT#391 - stodd - size day to week summary
					_dtTaskSizeDayToWeekSummary.AcceptChanges();
					//End TT#391 - stodd - size day to week summary

					//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
					//_taskListProf.Name = txtTaskListName.Text.Trim();
					//_taskListProf.UserRID = userRID;
					saveToNode = GetNewSaveToNode();
					userRID = GetNewUserRID();
					ownerRID = GetNewOwnerRID();

					if (aUpdateMode == eUpdateMode.Create)
					{
						taskListProf = (TaskListProfile)_taskListProf.Clone();
                        // Begin TT#3993 - JSmith - Task File-SaveAs does not work
                        frmSaveAs formSaveAs = new frmSaveAs(SAB);
                        formSaveAs.SaveAsName = txtTaskListName.Text.Trim();
                        if (userRID == Include.GlobalUserRID)
                        {
                            formSaveAs.isGlobalChecked = true;
                        }
                        else
                        {
                            formSaveAs.isUserChecked = true;
                        }

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
                                    // Begin TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.
                                    //txtTaskListName.Text = formSaveAs.SaveAsName;
                                    //if (isDuplicateName(txtTaskListName.Text, userRID))
                                    //{
                                    //    MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TaskListNameExists), this.Text);
                                    //    continueSave = false;
                                    //}
                                    if (isDuplicateName(formSaveAs.SaveAsName, userRID))
                                    {
                                        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TaskListNameExists), this.Text);
                                        continueSave = false;
                                    }
                                    else
                                    {
                                        txtTaskListName.Text = formSaveAs.SaveAsName;
                                    }
                                    // End TT#4417 - JSmith - Copy a Job (using Save As) creates a new Job in the Explorer, although both Jobs are pointing to the new copied Job.
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        // End TT#3993 - JSmith - Task File-SaveAs does not work
					}
					else
					{
						taskListProf = _taskListProf;
					}

					taskListProf.Name = txtTaskListName.Text.Trim();
					//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

					if (taskListProf.Key == Include.NoRID || aUpdateMode == eUpdateMode.Create)
					{
						//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
						//_taskListProf.OwnerUserRID = GetNewUserRID();
						//_taskListProf.Key = _dlSchedule.TaskList_Insert(_taskListProf, _taskListProf.OwnerUserRID);
						taskListProf.UserRID = userRID;
						taskListProf.OwnerUserRID = ownerRID;
						taskListProf.Key = _dlSchedule.TaskList_Insert(taskListProf, taskListProf.UserRID);

						_dlFolder.OpenUpdateConnection();

						try
						{
							_dlFolder.Folder_Item_Insert(saveToNode.Profile.Key, taskListProf.Key, eProfileType.TaskList);
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
						//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
					}
					else
					{
						_dlSchedule.TaskList_Update(taskListProf, _SAB.ClientServerSession.UserRID);
					}

					taskSeqHash = new Hashtable();

					i = 0;
					foreach (UltraGridRow row in ulgTasks.Rows)
					{
						taskSeqHash.Add(Convert.ToInt32(row.Cells["TASK_SEQUENCE"].Value), i);

						row.Cells["TASKLIST_RID"].Value = taskListProf.Key;
						row.Cells["TASK_SEQUENCE"].Value = i;
						i++;
					}

					ulgTasks.UpdateData();
                    
					ulgForecast.DataSource = null;
					newDataList = new Hashtable();
					hashEnum = _forecastDataSetList.GetEnumerator();
					i = 0;

					while (hashEnum.MoveNext())
					{
						BindForecastGridData((DataSetEntry)hashEnum.Value);
						
						if (ulgForecast.Rows.Count > 0)
						{
							foreach (UltraGridRow row in ulgForecast.Rows)
							{
								taskSeq = (int)taskSeqHash[Convert.ToInt32(row.Cells["TASK_SEQUENCE"].Value)];

								//Begin Track #6277 - JScott - Cannot drag/drop a OTS Forecast Method in to a Task List for Forecasting
								//row.Cells["TASKLIST_RID"].Value = taskListProf.Key;
								//row.Cells["TASK_SEQUENCE"].Value = taskSeq;
								//row.Cells["FORECAST_SEQUENCE"].Value = i;

								//End Track #6277 - JScott - Cannot drag/drop a OTS Forecast Method in to a Task List for Forecasting
								childBand = row.ChildBands[0];
								j = 0;
								foreach (UltraGridRow childRow in childBand.Rows)
								{
									childRow.Cells["TASKLIST_RID"].Value = taskListProf.Key;
									childRow.Cells["TASK_SEQUENCE"].Value = taskSeq;
									childRow.Cells["FORECAST_SEQUENCE"].Value = i;
									childRow.Cells["DETAIL_SEQUENCE"].Value = j;
									j++;
								}
								//Begin Track #6277 - JScott - Cannot drag/drop a OTS Forecast Method in to a Task List for Forecasting

								row.Cells["TASKLIST_RID"].Value = taskListProf.Key;
								row.Cells["TASK_SEQUENCE"].Value = taskSeq;
								row.Cells["FORECAST_SEQUENCE"].Value = i;
								//End Track #6277 - JScott - Cannot drag/drop a OTS Forecast Method in to a Task List for Forecasting
								i++;
							}

							newDataList.Add(taskSeq, (DataSetEntry)hashEnum.Value);

							ulgForecast.UpdateData();
							ForecastAcceptChanges();
						}
					}

					_forecastDataSetList = newDataList;

					ulgAllocate.DataSource = null;
					newDataList = new Hashtable();
					hashEnum = _allocateDataSetList.GetEnumerator();
					i = 0;

					while (hashEnum.MoveNext())
					{
						BindAllocateGridData((DataSetEntry)hashEnum.Value);
						
						if (ulgAllocate.Rows.Count > 0)
						{
							foreach (UltraGridRow row in ulgAllocate.Rows)
							{
								taskSeq = (int)taskSeqHash[Convert.ToInt32(row.Cells["TASK_SEQUENCE"].Value)];

								//Begin Track #6277 - JScott - Cannot drag/drop a OTS Forecast Method in to a Task List for Forecasting
								//row.Cells["TASKLIST_RID"].Value = taskListProf.Key;
								//row.Cells["TASK_SEQUENCE"].Value = taskSeq;
								//row.Cells["ALLOCATE_SEQUENCE"].Value = i;

								//End Track #6277 - JScott - Cannot drag/drop a OTS Forecast Method in to a Task List for Forecasting
								childBand = row.ChildBands[0];
								j = 0;
								foreach (UltraGridRow childRow in childBand.Rows)
								{
									childRow.Cells["TASKLIST_RID"].Value = taskListProf.Key;
									childRow.Cells["TASK_SEQUENCE"].Value = taskSeq;
									childRow.Cells["ALLOCATE_SEQUENCE"].Value = i;
									childRow.Cells["DETAIL_SEQUENCE"].Value = j;
									j++;
								}
								//Begin Track #6277 - JScott - Cannot drag/drop a OTS Forecast Method in to a Task List for Forecasting

								row.Cells["TASKLIST_RID"].Value = taskListProf.Key;
								row.Cells["TASK_SEQUENCE"].Value = taskSeq;
								row.Cells["ALLOCATE_SEQUENCE"].Value = i;
								//End Track #6277 - JScott - Cannot drag/drop a OTS Forecast Method in to a Task List for Forecasting
								i++;
							}

							newDataList.Add(taskSeq, (DataSetEntry)hashEnum.Value);

							ulgAllocate.UpdateData();
							AllocateAcceptChanges();
						}
					}

					_allocateDataSetList = newDataList;

					ulgRollup.DataSource = null;
					newDataList = new Hashtable();
					hashEnum = _rollupDataTableList.GetEnumerator();
					i = 0;

					while (hashEnum.MoveNext())
					{
						BindRollupGridData((DataTableEntry)hashEnum.Value);
		
						if (ulgRollup.Rows.Count > 0)
						{

							foreach (UltraGridRow row in ulgRollup.Rows)
							{
								taskSeq = (int)taskSeqHash[Convert.ToInt32(row.Cells["TASK_SEQUENCE"].Value)];

								row.Cells["TASKLIST_RID"].Value = taskListProf.Key;
								row.Cells["TASK_SEQUENCE"].Value = (int)taskSeqHash[Convert.ToInt32(row.Cells["TASK_SEQUENCE"].Value)];
								row.Cells["ROLLUP_SEQUENCE"].Value = i;

								row.Cells["POSTING_IND"].Value = (Convert.ToBoolean(row.Cells["Posting"].Value)) ? '1' : '0';
								row.Cells["RECLASS_IND"].Value = (Convert.ToBoolean(row.Cells["Reclass"].Value)) ? '1' : '0';
								row.Cells["HIERARCHY_LEVELS_IND"].Value = (Convert.ToBoolean(row.Cells["Hierarchy Levels"].Value)) ? '1' : '0';
								row.Cells["DAY_TO_WEEK_IND"].Value = (Convert.ToBoolean(row.Cells["Day To Week"].Value)) ? '1' : '0';
								row.Cells["DAY_IND"].Value = (Convert.ToBoolean(row.Cells["Day"].Value)) ? '1' : '0';
								row.Cells["WEEK_IND"].Value = (Convert.ToBoolean(row.Cells["Week"].Value)) ? '1' : '0';
								row.Cells["STORE_IND"].Value = (Convert.ToBoolean(row.Cells["Store"].Value)) ? '1' : '0';
								row.Cells["CHAIN_IND"].Value = (Convert.ToBoolean(row.Cells["Chain"].Value)) ? '1' : '0';
								row.Cells["STORE_TO_CHAIN_IND"].Value = (Convert.ToBoolean(row.Cells["Store To Chain"].Value)) ? '1' : '0';
								row.Cells["INTRANSIT_IND"].Value = (Convert.ToBoolean(row.Cells["Intransit"].Value)) ? '1' : '0';
								i++;
							}

							newDataList.Add(taskSeq, (DataTableEntry)hashEnum.Value);

							ulgRollup.UpdateData();
							RollupAcceptChanges();
						}
					}

					_rollupDataTableList = newDataList;

					//Begin TT#155 - JScott - Size Curve Method
					ulgSizeCurveMethod.DataSource = null;
					newDataList = new Hashtable();
					hashEnum = _sizeCurveMethodDataTableList.GetEnumerator();
					i = 0;

					while (hashEnum.MoveNext())
					{
						BindSizeCurveMethodGridData((DataTableEntry)hashEnum.Value);

						if (ulgSizeCurveMethod.Rows.Count > 0)
						{
							foreach (UltraGridRow row in ulgSizeCurveMethod.Rows)
							{
								taskSeq = (int)taskSeqHash[Convert.ToInt32(row.Cells["TASK_SEQUENCE"].Value)];

								row.Cells["TASKLIST_RID"].Value = taskListProf.Key;
								row.Cells["TASK_SEQUENCE"].Value = taskSeq;
								row.Cells["GENERATE_SEQUENCE"].Value = i;
								i++;
							}

							newDataList.Add(taskSeq, (DataTableEntry)hashEnum.Value);

							ulgSizeCurveMethod.UpdateData();
							SizeCurveMethodAcceptChanges();
						}
					}

					_sizeCurveMethodDataTableList = newDataList;

					//End TT#155 - JScott - Size Curve Method
					//Begin TT#155 - JScott - Add Size Curve info to Node Properties
					ulgSizeCurves.DataSource = null;
					newDataList = new Hashtable();
					hashEnum = _sizeCurvesDataTableList.GetEnumerator();
					i = 0;

					while (hashEnum.MoveNext())
					{
						BindSizeCurvesGridData((DataTableEntry)hashEnum.Value);

						if (ulgSizeCurves.Rows.Count > 0)
						{
							foreach (UltraGridRow row in ulgSizeCurves.Rows)
							{
								taskSeq = (int)taskSeqHash[Convert.ToInt32(row.Cells["TASK_SEQUENCE"].Value)];

								row.Cells["TASKLIST_RID"].Value = taskListProf.Key;
								row.Cells["TASK_SEQUENCE"].Value = taskSeq;
								row.Cells["GENERATE_SEQUENCE"].Value = i;
								i++;
							}

							newDataList.Add(taskSeq, (DataTableEntry)hashEnum.Value);

							ulgSizeCurves.UpdateData();
							SizeCurvesAcceptChanges();
						}
					}

					_sizeCurvesDataTableList = newDataList;
					//End TT#155 - JScott - Add Size Curve info to Node Properties
					//Begin TT#1330 - JScott - Reordering tasks in tasklist results in error
					////Begin TT#155 - JScott - Add Size Curve info to Node Properties
					////Begin TT#707 - JScott - Size Curve process needs to multi-thread

					//foreach (DataRow row in _dtTaskSizeCurvesNode.Rows)
					//{
					//    row["TASKLIST_RID"] = taskListProf.Key;
					//    row["TASK_SEQUENCE"] = (int)taskSeqHash[Convert.ToInt32(row["TASK_SEQUENCE"])];
					//}
					
					////End TT#707 - JScott - Size Curve process needs to multi-thread
					////End TT#155 - JScott - Add Size Curve info to Node Properties
					//foreach (DataRow row in _dtTaskPosting.Rows)
					//{
					//    row["TASKLIST_RID"] = taskListProf.Key;
					//    row["TASK_SEQUENCE"] = (int)taskSeqHash[Convert.ToInt32(row["TASK_SEQUENCE"])];
					//}
					
					//foreach (DataRow row in _dtTaskProgram.Rows)
					//{
					//    row["TASKLIST_RID"] = taskListProf.Key;
					//    row["TASK_SEQUENCE"] = (int)taskSeqHash[Convert.ToInt32(row["TASK_SEQUENCE"])];
					//}

					//// Begin TT#391 - stodd - size day to week summary
					//foreach (DataRow row in _dtTaskSizeDayToWeekSummary.Rows)
					//{
					//    row["TASKLIST_RID"] = taskListProf.Key;
					//    row["TASK_SEQUENCE"] = (int)taskSeqHash[Convert.ToInt32(row["TASK_SEQUENCE"])];
					//}
					//// End TT#391 - stodd - size day to week summary

					_dtTaskSizeCurvesNode.PrimaryKey = null;

					foreach (DataRow row in _dtTaskSizeCurvesNode.Rows)
					{
						row["TASKLIST_RID"] = taskListProf.Key;
						row["TASK_SEQUENCE"] = (int)taskSeqHash[Convert.ToInt32(row["TASK_SEQUENCE"])];
					}

					_dtTaskSizeCurvesNode.PrimaryKey = new DataColumn[] { _dtTaskSizeCurvesNode.Columns["TASKLIST_RID"], _dtTaskSizeCurvesNode.Columns["TASK_SEQUENCE"] };

					_dtTaskPosting.PrimaryKey = null;

					foreach (DataRow row in _dtTaskPosting.Rows)
					{
						row["TASKLIST_RID"] = taskListProf.Key;
						row["TASK_SEQUENCE"] = (int)taskSeqHash[Convert.ToInt32(row["TASK_SEQUENCE"])];
					}

					_dtTaskPosting.PrimaryKey = new DataColumn[] { _dtTaskPosting.Columns["TASKLIST_RID"], _dtTaskPosting.Columns["TASK_SEQUENCE"] };

					// Begin TT#1595-MD - stodd - Batch Comp
                    _dtTaskBatchComp.PrimaryKey = null;

                    foreach (DataRow row in _dtTaskBatchComp.Rows)
                    {
                        row["TASKLIST_RID"] = taskListProf.Key;
                        row["TASK_SEQUENCE"] = (int)taskSeqHash[Convert.ToInt32(row["TASK_SEQUENCE"])];
                    }

                    _dtTaskBatchComp.PrimaryKey = new DataColumn[] { _dtTaskBatchComp.Columns["TASKLIST_RID"], _dtTaskBatchComp.Columns["TASK_SEQUENCE"] };
					// End TT#1595-MD - stodd - Batch Comp

                    // Begin TT#1581-MD - stodd - Header Reconcile
                    _dtTaskHeaderReconcile.PrimaryKey = null;

                    foreach (DataRow row in _dtTaskHeaderReconcile.Rows)
                    {
                        row["TASKLIST_RID"] = taskListProf.Key;
                        row["TASK_SEQUENCE"] = (int)taskSeqHash[Convert.ToInt32(row["TASK_SEQUENCE"])];
                    }

                    _dtTaskHeaderReconcile.PrimaryKey = new DataColumn[] { _dtTaskHeaderReconcile.Columns["TASKLIST_RID"], _dtTaskHeaderReconcile.Columns["TASK_SEQUENCE"] };
                    // End TT#1581-MD - stodd - Header Reconcile


					_dtTaskProgram.PrimaryKey = null;

					foreach (DataRow row in _dtTaskProgram.Rows)
					{
						row["TASKLIST_RID"] = taskListProf.Key;
						row["TASK_SEQUENCE"] = (int)taskSeqHash[Convert.ToInt32(row["TASK_SEQUENCE"])];
					}

					_dtTaskProgram.PrimaryKey = new DataColumn[] { _dtTaskProgram.Columns["TASKLIST_RID"], _dtTaskProgram.Columns["TASK_SEQUENCE"] };
					_dtTaskSizeDayToWeekSummary.PrimaryKey = null;

					foreach (DataRow row in _dtTaskSizeDayToWeekSummary.Rows)
					{
						row["TASKLIST_RID"] = taskListProf.Key;
						row["TASK_SEQUENCE"] = (int)taskSeqHash[Convert.ToInt32(row["TASK_SEQUENCE"])];
					}

					_dtTaskSizeDayToWeekSummary.PrimaryKey = new DataColumn[] { _dtTaskSizeDayToWeekSummary.Columns["TASKLIST_RID"], _dtTaskSizeDayToWeekSummary.Columns["TASK_SEQUENCE"] };
					//End TT#1330 - JScott - Reordering tasks in tasklist results in error

					ForecastAcceptChanges();
					AllocateAcceptChanges();
					RollupAcceptChanges();
					//Begin TT#155 - JScott - Size Curve Method
					SizeCurveMethodAcceptChanges();
					//End TT#155 - JScott - Size Curve Method
					//Begin TT#155 - JScott - Add Size Curve info to Node Properties
					SizeCurvesAcceptChanges();
					//Begin TT#707 - JScott - Size Curve process needs to multi-thread
					_dtTaskSizeCurvesNode.AcceptChanges();
					//End TT#707 - JScott - Size Curve process needs to multi-thread
					//End TT#155 - JScott - Add Size Curve info to Node Properties
					_dtTaskPosting.AcceptChanges();
                    _dtTaskBatchComp.AcceptChanges();	// TT#1595-MD - stodd - Batch Comp
                    _dtTaskHeaderReconcile.AcceptChanges();	// TT#1595-MD - stodd - Batch Comp
					_dtTaskProgram.AcceptChanges();
					_dtTask.AcceptChanges();
					// Begin TT#391 - stodd - size day to week summary
					_dtTaskSizeDayToWeekSummary.AcceptChanges();
					// End TT#391 - stodd - size day to week summary

					_dlSchedule.TaskForecastDetail_Delete(taskListProf.Key);
					_dlSchedule.TaskForecast_Delete(taskListProf.Key);
					_dlSchedule.TaskAllocateDetail_Delete(taskListProf.Key);
					_dlSchedule.TaskAllocate_Delete(taskListProf.Key);
					_dlSchedule.TaskRollup_Delete(taskListProf.Key);
					//Begin TT#155 - JScott - Add Size Curve info to Node Properties
					////Begin TT#155 - JScott - Size Curve Method
					//_dlSchedule.TaskSizeCurveGenerate_Delete(taskListProf.Key);
					////End TT#155 - JScott - Size Curve Method
					_dlSchedule.TaskSizeCurveMethod_Delete(taskListProf.Key);
					_dlSchedule.TaskSizeCurves_Delete(taskListProf.Key);
					//End TT#155 - JScott - Add Size Curve info to Node Properties
					//Begin TT#707 - JScott - Size Curve process needs to multi-thread
					_dlSchedule.TaskSizeCurveGenerateNode_Delete(taskListProf.Key);
					//End TT#707 - JScott - Size Curve process needs to multi-thread
					//Begin TT#391 - stodd - size day to week summary
					_dlSchedule.TaskSizeDayToWeekSummary_Delete(taskListProf.Key);
					//End TT#391 - stodd - size day to week summary
					_dlSchedule.TaskPosting_Delete(taskListProf.Key);
					_dlSchedule.TaskProgram_Delete(taskListProf.Key);
                    _dlSchedule.TaskBatchComp_Delete(taskListProf.Key);		// TT#1595-MD - stodd - Batch Comp
                    _dlSchedule.TaskHeaderReconcile_Delete(taskListProf.Key);		// TT#1581-MD - stodd - Header Reconcile
					_dlSchedule.Task_Delete(taskListProf.Key);

					_dlSchedule.Task_Insert(_dtTask);

					hashEnum = _forecastDataSetList.GetEnumerator();

					while (hashEnum.MoveNext())
					{
						dsEntry = (DataSetEntry)hashEnum.Value;
						_dlSchedule.TaskForecast_Insert(dsEntry.MainDataTable);
						_dlSchedule.TaskForecastDetail_Insert(dsEntry.DetailDataTable);
					}

					hashEnum = _allocateDataSetList.GetEnumerator();

					while (hashEnum.MoveNext())
					{
						dsEntry = (DataSetEntry)hashEnum.Value;
						_dlSchedule.TaskAllocate_Insert(dsEntry.MainDataTable);
						_dlSchedule.TaskAllocateDetail_Insert(dsEntry.DetailDataTable);
					}

					hashEnum = _rollupDataTableList.GetEnumerator();
					
					while (hashEnum.MoveNext())
					{
						dtEntry = (DataTableEntry)hashEnum.Value;
						_dlSchedule.TaskRollup_Insert(dtEntry.DataTable);
					}

					//Begin TT#155 - JScott - Size Curve Method
					hashEnum = _sizeCurveMethodDataTableList.GetEnumerator();

					while (hashEnum.MoveNext())
					{
						dtEntry = (DataTableEntry)hashEnum.Value;
						//Begin TT#155 - JScott - Add Size Curve info to Node Properties
						//_dlSchedule.TaskSizeCurveGenerate_Insert(dtEntry.DataTable);
						_dlSchedule.TaskSizeCurveMethod_Insert(dtEntry.DataTable);
						//End TT#155 - JScott - Add Size Curve info to Node Properties
					}

					//End TT#155 - JScott - Size Curve Method
					//Begin TT#155 - JScott - Add Size Curve info to Node Properties
					hashEnum = _sizeCurvesDataTableList.GetEnumerator();

					while (hashEnum.MoveNext())
					{
						dtEntry = (DataTableEntry)hashEnum.Value;
						_dlSchedule.TaskSizeCurves_Insert(dtEntry.DataTable);
					}

					//End TT#155 - JScott - Add Size Curve info to Node Properties
					//Begin TT#707 - JScott - Size Curve process needs to multi-thread
					_dlSchedule.TaskSizeCurveGenerateNode_Insert(_dtTaskSizeCurvesNode);

					//End TT#707 - JScott - Size Curve process needs to multi-thread
					_dlSchedule.TaskPosting_Insert(_dtTaskPosting);
                    _dlSchedule.TaskBatchComp_Insert(_dtTaskBatchComp);		// TT#1595-MD - stodd - Batch Comp
                    _dlSchedule.TaskHeaderReconcile_Insert(_dtTaskHeaderReconcile);		// TT#1581-MD - stodd - header reconcile
					_dlSchedule.TaskProgram_Insert(_dtTaskProgram);

					//Begin TT#391 - stodd - size day to week summary
					_dlSchedule.TaskSizeDayToWeekSummary_Insert(_dtTaskSizeDayToWeekSummary);
					//End TT#391 - stodd - size day to week summary

					_dlSchedule.CommitData();

					ShowTaskPanel();
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

				if (OnTaskListPropertiesSaveHandler != null)
				{
					//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
					//OnTaskListPropertiesSaveHandler(this, new TaskListPropertiesSaveEventArgs());
					OnTaskListPropertiesSaveHandler(this, new TaskListPropertiesSaveEventArgs(saveToNode, taskListProf));
					//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
				}
				//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

				SetUserFields(taskListProf, taskListProf.UserRID, taskListProf.OwnerUserRID);
				//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#3993 - JSmith - Task File-SaveAs does not work
        private bool isDuplicateName(string aName, int userRID)
        {
            if (_dlSchedule.TaskList_GetKey(aName, userRID) != -1)
            {
                return true;
            }
            return false;
        }
        // End TT#3993 - JSmith - Task File-SaveAs does not work

		private int GetNewUserRID()
		{
			try
			{
				if (rdoOwner.Checked)
				{
					return _initialUserRID;
				}
                else if (rdoUser.Checked)
				{
					return _SAB.ClientServerSession.UserRID;
				}
				else if (rdoGlobal.Checked)
				{
					return Include.GlobalUserRID;
				}
				else
				{
					return Include.SystemUserRID;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private void SetUserFields(TaskListProfile aTaskListProf, int aInitialUserRID, int aInitialOwnerRID)
		{
			try
			{
				_taskListProf = aTaskListProf;
				_initialUserRID = aInitialUserRID;
				_initialOwnerRID = aInitialOwnerRID;

				if (_initialOwnerRID != _initialUserRID)
				{
					rdoOwner.Visible = true;
					rdoOwner.Text = rdoOwner.Text.Replace("{1}", _SAB.ClientServerSession.GetUserName(_initialOwnerRID));
				}
				else
				{
					rdoOwner.Visible = false;
				}

				rdoUser.Enabled = true;
				rdoGlobal.Enabled = true;
				rdoSystem.Enabled = true;
				rdoOwner.Enabled = true;

				// Check appropriate User Radio Button

				if (_initialOwnerRID == Include.GlobalUserRID)
				{
					if (_SAB.ClientServerSession.UserRID == Include.GlobalUserRID)
					{
						rdoUser.Checked = true;
						rdoUser.Tag = true;
					}
					else
					{
						rdoGlobal.Checked = true;
						rdoGlobal.Tag = true;
					}
				}
				else if (_initialOwnerRID == Include.SystemUserRID)
				{
					if (_SAB.ClientServerSession.UserRID == Include.SystemUserRID)
					{
						rdoUser.Checked = true;
						rdoUser.Tag = true;
					}
					else
					{
						rdoSystem.Checked = true;
						rdoSystem.Tag = true;
					}
				}
				else if (_initialOwnerRID == _SAB.ClientServerSession.UserRID || _initialOwnerRID == Include.NoRID)
				{
					rdoUser.Checked = true;
					rdoUser.Tag = true;
				}
				else
				{
					rdoOwner.Checked = true;
					rdoOwner.Tag = true;
				}

				// Enable User Radio Buttons

				if (_readOnly || !_globalSecLvl.AllowUpdate || _SAB.ClientServerSession.UserRID == Include.GlobalUserRID)
				{
					rdoGlobal.Enabled = false;
				}

				if (_readOnly || !_systemSecLvl.AllowUpdate || _SAB.ClientServerSession.UserRID == Include.SystemUserRID)
				{
					rdoSystem.Enabled = false;
				}

                
				if (_readOnly || !_userSecLvl.AllowUpdate)
				{
					rdoUser.Enabled = false;
				}

				if (_readOnly || !rdoOwner.Visible || (!_userSecLvl.AllowUpdate && !_systemSecLvl.AllowUpdate))
				{
					rdoOwner.Enabled = false;
				}

				// Enable Save/OK/Save As Button

				if (!_readOnly &&
					((rdoGlobal.Checked && rdoGlobal.Enabled) ||
					(rdoSystem.Checked && rdoSystem.Enabled) ||
					(rdoUser.Checked && rdoUser.Enabled) ||
					(rdoOwner.Checked && rdoOwner.Enabled)))
				{
					_canUpdateTaskList = true;
				}
				else
				{
					_canUpdateTaskList = false;
				}

				EnableSaveButtons();

				// Enable Run-Now Button

				btnRunNow.Enabled = false;

                if (_SAB.SchedulerServerSession != null)
                {
					if (_initialOwnerRID == Include.GlobalUserRID && _globalSecLvl.AllowExecute)
					{
						btnRunNow.Enabled = true;
					}
					else if (_initialOwnerRID == Include.SystemUserRID && _systemSecLvl.AllowExecute)
					{
						btnRunNow.Enabled = true;
					}
                    //BEGIN TT#4239 - DOConnell - Security set to View only for system and global task list. When you open the method the run now button is active and the method can be run.
                    else if (_initialOwnerRID != Include.SystemUserRID && _initialOwnerRID != Include.GlobalUserRID && _userSecLvl.AllowExecute)
                    //else if (_userSecLvl.AllowExecute || _systemSecLvl.AllowExecute)
                    //END TT#4239 - DOConnell - Security set to View only for system and global task list. When you open the method the run now button is active and the method can be run.
					{
						btnRunNow.Enabled = true;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        //Begin TT#1446-MD -jsobek -Header Filter - Opening a Task List with a user Header Filter the filter name appears as a number
        private bool IsTaskGlobalOrSystem()
        {
            if (_initialOwnerRID == Include.GlobalUserRID)
            {
                if (_SAB.ClientServerSession.UserRID == Include.GlobalUserRID)
                {
                    //rdoUser.Checked = true;
                    //rdoUser.Tag = true;
                    return false;
                }
                else
                {
                    //rdoGlobal.Checked = true;
                    //rdoGlobal.Tag = true;
                    return true;
                }
            }
            else if (_initialOwnerRID == Include.SystemUserRID)
            {
                if (_SAB.ClientServerSession.UserRID == Include.SystemUserRID)
                {
                    //rdoUser.Checked = true;
                    //rdoUser.Tag = true;
                    return false;
                }
                else
                {
                    //rdoSystem.Checked = true;
                    //rdoSystem.Tag = true;
                    return true;
                }
            }
            else if (_initialOwnerRID == _SAB.ClientServerSession.UserRID || _initialOwnerRID == Include.NoRID)
            {
                //rdoUser.Checked = true;
                //rdoUser.Tag = true;
                return false;
            }
            else
            {
                //rdoOwner.Checked = true;
                //rdoOwner.Tag = true;
                return true;
            }
        }

        Infragistics.Win.ValueList _valListHeaderFilters;
        private void RebindAllocateTaskHeaderFilters()
        {
            FilterData storeFilterData = new FilterData();
            DataTable dtHeaderFilters;
            if (this._readOnly) //readonly
            {
                //load all the header filters so anyone can view anything
                //dtHeaderFilters = storeFilterData.ReadFiltersForType(filterTypes.HeaderFilter);

                //No need to rebind here - just leave
                return;
            }
            else if (rdoUser.Checked == false) //global or system
            {
                ArrayList userRIDList = new ArrayList();
                userRIDList.Add(Include.GlobalUserRID);
                dtHeaderFilters = storeFilterData.FilterRead(filterTypes.HeaderFilter, eProfileType.FilterHeader, userRIDList);
            }
            else //user
            {
                ArrayList userRIDList = new ArrayList();
                userRIDList.Add(Include.GlobalUserRID);
                userRIDList.Add(SAB.ClientServerSession.UserRID);
                dtHeaderFilters = storeFilterData.FilterRead(filterTypes.HeaderFilter, eProfileType.FilterHeader, userRIDList);
            }


            _valListHeaderFilters.ValueListItems.Clear();

            Infragistics.Win.ValueListItem valListItem = new Infragistics.Win.ValueListItem();
            valListItem.DataValue = Include.NoRID;
            valListItem.DisplayText = "No Filter"; //MIDText.GetTextOnly((int)eMIDTextCode.msg_DefaultToWorkflowValue); //TT#1313-MD -jsobek -Header Filters
            _valListHeaderFilters.ValueListItems.Add(valListItem);

            foreach (DataRow row in dtHeaderFilters.Rows)
            {
                valListItem = new Infragistics.Win.ValueListItem();
                valListItem.DataValue = Convert.ToInt32(row["FILTER_RID"]);
                valListItem.DisplayText = Convert.ToString(row["FILTER_NAME"]);
                _valListHeaderFilters.ValueListItems.Add(valListItem);
            }


            //set the current filter on all rows to NoRID
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow ugRow in ulgAllocate.Rows)
            {
                if (ugRow.Band.Index == 0)
                {
                    ugRow.Cells["Filter"].Value = Include.NoRID;
                }
            }
     



        }
        //End TT#1446-MD -jsobek -Header Filter - Opening a Task List with a user Header Filter the filter name appears as a number

		private int GetNewOwnerRID()
		{
			try
			{
				if (rdoOwner.Checked)
				{
					return _initialOwnerRID;
				}
				else if (rdoUser.Checked)
				{
					return _SAB.ClientServerSession.UserRID;
				}
				else if (rdoGlobal.Checked)
				{
					return Include.GlobalUserRID;
				}
				else
				{
					return Include.SystemUserRID;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private MIDTaskListNode GetNewSaveToNode()
		{
			try
			{
				if (rdoOwner.Checked)
				{
					return _currParentNode;
				}
				else if (rdoUser.Checked)
				{
					if (rdoUser.Tag != null)
					{
						return _currParentNode;
					}
					else
					{
						return _userNode;
					}
				}
				else if (rdoGlobal.Checked)
				{
					if (rdoGlobal.Tag != null)
					{
						return _currParentNode;
					}
					else
					{
						return _globalNode;
					}
				}
				else
				{
					if (rdoSystem.Tag != null)
					{
						return _currParentNode;
					}
					else
					{
						return _systemNode;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private void ClearTaskSelection()
		{
			try
			{
				ulgTasks.Selected.Rows.Clear();

                //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                HideTabs();
                //pnlForecast.Visible = false;
                //pnlAllocate.Visible = false;
                //pnlRollup.Visible = false;
                ////Begin TT#155 - JScott - Size Curve Method
                //pnlSizeCurveMethod.Visible = false;
                ////End TT#155 - JScott - Size Curve Method
                ////Begin TT#155 - JScott - Add Size Curve info to Node Properties
                //pnlSizeCurves.Visible = false;
                ////End TT#155 - JScott - Add Size Curve info to Node Properties
                //pnlPosting.Visible = false;
                //pnlProgram.Visible = false;
                //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void ShowDateSelector(UltraGrid aGrid, UltraGridCell aCell, string aRIDColumnName)
		{
			CalendarDateSelector frmCalDtSelector = null;
			DialogResult DateRangeResult;
			DateRangeProfile SelectedDateRange;

			try
			{
				frmCalDtSelector = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{_SAB});

				if (aGrid.ActiveRow.Cells[aRIDColumnName].Value != null &&
					aGrid.ActiveRow.Cells[aRIDColumnName].Value != System.DBNull.Value &&
					Convert.ToInt32(aGrid.ActiveRow.Cells[aRIDColumnName].Value, CultureInfo.CurrentUICulture) != Include.NoRID)
				{
					frmCalDtSelector.DateRangeRID = Convert.ToInt32(aGrid.ActiveRow.Cells[aRIDColumnName].Value, CultureInfo.CurrentUICulture);
				}

				frmCalDtSelector.StartPosition = FormStartPosition.CenterScreen;
				frmCalDtSelector.RestrictToSingleDate = false;
				frmCalDtSelector.AllowDynamic = true;
				frmCalDtSelector.AllowDynamicToCurrent = true;
				frmCalDtSelector.AllowDynamicToPlan = false;
				frmCalDtSelector.AllowDynamicToStoreOpen = false;

				DateRangeResult = frmCalDtSelector.ShowDialog();

				if (DateRangeResult == DialogResult.OK)
				{
					SelectedDateRange = (DateRangeProfile)frmCalDtSelector.Tag;

					if (SelectedDateRange.Key != Include.UndefinedCalendarDateRange)
					{
						aCell.Value = SelectedDateRange.DisplayDate;
						aGrid.ActiveRow.Cells[aRIDColumnName].Value = SelectedDateRange.Key;
					}
					else
					{
                        aCell.Value = string.Empty;		// TT#207 MID Track #6451 RMatelic - Task List Date Select error 
						aCell.Value = DBNull.Value;
						aGrid.ActiveRow.Cells[aRIDColumnName].Value = DBNull.Value;
					}

					aCell.Tag = null;
					aCell.Appearance.Image = null;
					
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private DataSetEntry GetForecastDataSetEntry(int aTaskSequence)
		{
			DataSetEntry dataSetEntry;

			try
			{
				dataSetEntry = (DataSetEntry)_forecastDataSetList[aTaskSequence];

				if (dataSetEntry == null)
				{
					dataSetEntry = new ForecastDataSetEntry(_dtTaskForecast, _dtTaskForecastDetail);
					_forecastDataSetList.Add(aTaskSequence, dataSetEntry);
				}

				return dataSetEntry;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private DataSetEntry GetAllocateDataSetEntry(int aTaskSequence)
		{
			DataSetEntry dataSetEntry;

			try
			{
				dataSetEntry = (DataSetEntry)_allocateDataSetList[aTaskSequence];

				if (dataSetEntry == null)
				{
					dataSetEntry = new AllocateDataSetEntry(_dtTaskAllocate, _dtTaskAllocateDetail);
					_allocateDataSetList.Add(aTaskSequence, dataSetEntry);
				}
				
				return dataSetEntry;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private DataTableEntry GetRollupDataTableEntry(int aTaskSequence)
		{
			DataTableEntry dataTableEntry;

			try
			{
				dataTableEntry = (DataTableEntry)_rollupDataTableList[aTaskSequence];

				if (dataTableEntry == null)
				{
					dataTableEntry = new DataTableEntry(_dtTaskRollup);
					_rollupDataTableList.Add(aTaskSequence, dataTableEntry);
				}
				
				return dataTableEntry;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin TT#155 - JScott - Size Curve Method
		private DataTableEntry GetSizeCurveMethodDataTableEntry(int aTaskSequence)
		{
			DataTableEntry dataTableEntry;

			try
			{
				dataTableEntry = (DataTableEntry)_sizeCurveMethodDataTableList[aTaskSequence];

				if (dataTableEntry == null)
				{
					dataTableEntry = new DataTableEntry(_dtTaskSizeCurveMethod);
					_sizeCurveMethodDataTableList.Add(aTaskSequence, dataTableEntry);
				}

				return dataTableEntry;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End TT#155 - JScott - Size Curve Method
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		private DataTableEntry GetSizeCurvesDataTableEntry(int aTaskSequence)
		{
			DataTableEntry dataTableEntry;

			try
			{
				dataTableEntry = (DataTableEntry)_sizeCurvesDataTableList[aTaskSequence];

				if (dataTableEntry == null)
				{
					dataTableEntry = new DataTableEntry(_dtTaskSizeCurves);
					_sizeCurvesDataTableList.Add(aTaskSequence, dataTableEntry);
				}

				return dataTableEntry;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		private void ForecastAcceptChanges()
		{
			IDictionaryEnumerator hashEnum;

			try
			{
				hashEnum = _forecastDataSetList.GetEnumerator();
					
				while (hashEnum.MoveNext())
				{
					((DataSetEntry)hashEnum.Value).DataSet.AcceptChanges();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void AllocateAcceptChanges()
		{
			IDictionaryEnumerator hashEnum;

			try
			{
				hashEnum = _allocateDataSetList.GetEnumerator();
					
				while (hashEnum.MoveNext())
				{
					((DataSetEntry)hashEnum.Value).DataSet.AcceptChanges();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	
		private void RollupAcceptChanges()
		{
			IDictionaryEnumerator hashEnum;

			try
			{
				hashEnum = _rollupDataTableList.GetEnumerator();
					
				while (hashEnum.MoveNext())
				{
					((DataTableEntry)hashEnum.Value).DataTable.AcceptChanges();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin TT#155 - JScott - Size Curve Method
		private void SizeCurveMethodAcceptChanges()
		{
			IDictionaryEnumerator hashEnum;

			try
			{
				hashEnum = _sizeCurveMethodDataTableList.GetEnumerator();

				while (hashEnum.MoveNext())
				{
					((DataTableEntry)hashEnum.Value).DataTable.AcceptChanges();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End TT#155 - JScott - Size Curve Method
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		private void SizeCurvesAcceptChanges()
		{
			IDictionaryEnumerator hashEnum;

			try
			{
				hashEnum = _sizeCurvesDataTableList.GetEnumerator();

				while (hashEnum.MoveNext())
				{
					((DataTableEntry)hashEnum.Value).DataTable.AcceptChanges();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		private void ulgTasks_InitializeLayout(object sender, InitializeLayoutEventArgs e)
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

        private void ulgRollup_InitializeLayout(object sender, InitializeLayoutEventArgs e)
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

        private void ulgAllocate_InitializeLayout(object sender, InitializeLayoutEventArgs e)
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

        private void ulgForecast_InitializeLayout(object sender, InitializeLayoutEventArgs e)
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
		//Begin TT#155 - JScott - Size Curve Method
		private void ulgSizeCurveMethod_InitializeLayout(object sender, InitializeLayoutEventArgs e)
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

		//End TT#155 - JScott - Size Curve Method
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		private void ulgSizeCurves_InitializeLayout(object sender, InitializeLayoutEventArgs e)
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

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		private HierarchyNodeProfile GetNodeProfile(string aProductID)
		{
			try
			{
				string productID = aProductID.Trim();
				string[] pArray = productID.Split(new char[] {'['});
				productID = pArray[0].Trim(); 
				HierarchyMaintenance hm = new HierarchyMaintenance(_SAB);
				EditMsgs em = new EditMsgs();
				return hm.NodeLookup(ref em, productID, false);
			}
			catch(Exception ex)
			{
				HandleException(ex);
				throw;
			}
		}

		protected bool ApplySecurity()
		{
			bool securityOk = true;

			//if (FormLoaded)
			//{
			//    securityOk = (((MIDControlTag)(txtOverrideNode.Tag)).IsAuthorized(eSecurityTypes.Store, eSecuritySelectType.Update));
			//}
			return securityOk;
		}

		private void txtOverrideNode_TextChanged(object sender, EventArgs e)
		{
            //BEGIN TT#3995-Task List Explorer-Opening a Size Day to Week Summary task
            ApplySecurity();
            if (_ignoreTextChanged) return;
            //if (FormLoaded)
            //{
                ChangePending = true;
            //}
            //ApplySecurity();
            //END TT#3995-Task List Explorer-Opening a Size Day to Week Summary task
        }

		private void txtOverrideNode_Validated(object sender, EventArgs e)
		{
			try
			{
				if ((((TextBox)sender).Text.Trim() == string.Empty) && (((TextBox)sender).Tag != null))
				{
					_currentSizeDayToWeekSummaryRow["HN_RID"] = Include.NoRID;
				}
				else
				{
					HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)((TextBox)sender).Tag).MIDTagData;
					_overrideNodeRid = hnp.Key;
					_currentSizeDayToWeekSummaryRow["HN_RID"] = _overrideNodeRid;
					ApplySecurity();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void txtOverrideNode_Validating(object sender, CancelEventArgs e)
		{

		}

		private void txtOverrideNode_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				bool isSuccessfull = ((MIDTextBoxTag)(((TextBox)sender).Tag)).TextBox_DragDrop(sender, e);

				if (isSuccessfull)
				{
					HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)(((TextBox)sender).Tag)).MIDTagData;
					_overrideNodeRid = hnp.Key;
					_currentSizeDayToWeekSummaryRow["HN_RID"] = _overrideNodeRid;
					ChangePending = true;
					ApplySecurity();
				}
			}
			catch (BadDataInClipboardException)
			{
				MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void txtOverrideNode_DragEnter(object sender, DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}

		private void txtOverrideNode_DragOver(object sender, DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}

    //Begin  TT#2744 - MD - Rollup task in 5.0 asking to save when nothing has chenged - RBeck
        public bool RollUpLoaded
        {
            get { return _rollUpLoaded; }
            set { _rollUpLoaded = value; }
        }
    //End    TT#2744 - MD - Rollup task in 5.0 asking to save when nothing has chenged - RBeck
	
        //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {

        }
        private void ultraTabControl1_Validating(object sender, CancelEventArgs e)
        {
            //This form uses delayed validation. However, we still need to bind the values (even if they are invalid) to the datatable right away.
            if (this.ultraTabControl1.Tabs[tabKeys.tabEmail].Selected == true)
            {
                DataRow[] drTask = _dtTask.Select("TASKLIST_RID=" + (int)ulgTasks.ActiveRow.Cells["TASKLIST_RID"].Value + " AND TASK_SEQUENCE=" + (int)ulgTasks.ActiveRow.Cells["TASK_SEQUENCE"].Value);
                this.emailTaskList1.SaveToDataRow(drTask[0]);
                _dtTask.AcceptChanges();
            }
        

        }
        //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

        // Begin TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files
        private void cboProcessingDirection_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;

                if (cboProcessingDirection.SelectedItem != null)
                {
                    DataRowView dvRow = (DataRowView)cboProcessingDirection.SelectedItem;
                    eAPIFileProcessingDirection processingDirection = (eAPIFileProcessingDirection)Convert.ToInt32(dvRow.Row["TEXT_CODE"]);
                    if (processingDirection != eAPIFileProcessingDirection.Default &&
                        processingDirection != eAPIFileProcessingDirection.Config &&
                        nudPostingConcurrent.Value != 1)
                    {
                        if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConcurrentProcessesMustBe1, false), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == DialogResult.No)
                        {
                            cboProcessingDirection.SelectedValue = eAPIFileProcessingDirection.Default.GetHashCode();
                            nudPostingConcurrent.Enabled = true;
                        }
                        else
                        {
                            nudPostingConcurrent.Value = 1;
                            nudPostingConcurrent.Enabled = false;
                        }
                    }
                    // Begin TT#1314-MD - JSmith - Tasklist allows multiple concurrent files with FIFO/FILO processing
                    //else if (processingDirection == eAPIFileProcessingDirection.Default ||
                    //    processingDirection == eAPIFileProcessingDirection.Config)
                    //{
                    //    nudPostingConcurrent.Enabled = true;
                    //}
                    else if (processingDirection == eAPIFileProcessingDirection.Config)
                    {
                        if (_FileProcessingDirection == eAPIFileProcessingDirection.FIFO ||
                                _FileProcessingDirection == eAPIFileProcessingDirection.FILO)
                        {
                            if (nudPostingConcurrent.Value != 1)
                            {
                                if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConcurrentProcessesMustBe1, false), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.No)
                                {
                                    cboProcessingDirection.SelectedValue = eAPIFileProcessingDirection.Default.GetHashCode();
                                    nudPostingConcurrent.Enabled = true;
                                }
                                else
                                {
                                    nudPostingConcurrent.Value = 1;
                                    nudPostingConcurrent.Enabled = false;
                                }
                            }
                            else
                            {
                                nudPostingConcurrent.Enabled = false;
                            }
                        }
                        else
                        {
                            nudPostingConcurrent.Enabled = true;
                        }
                    }
                    else if (processingDirection == eAPIFileProcessingDirection.Default)
                    {
                        nudPostingConcurrent.Enabled = true;
                    }
                    // End TT#1314-MD - JSmith - Tasklist allows multiple concurrent files with FIFO/FILO processing
                    else
                    {
                        nudPostingConcurrent.Enabled = false;
                    }
                }

                if (cboProcessingDirection.Parent.Visible)
                {
                    _currentPostingRow["FILE_PROCESSING_DIRECTION"] = cboProcessingDirection.SelectedValue;
                }
            }
            else
            {
                if (cboProcessingDirection.SelectedItem != null)
                {
                    DataRowView dvRow = (DataRowView)cboProcessingDirection.SelectedItem;
                    eAPIFileProcessingDirection processingDirection = (eAPIFileProcessingDirection)Convert.ToInt32(dvRow.Row["TEXT_CODE"]);
                    if (processingDirection == eAPIFileProcessingDirection.Default ||
                        processingDirection == eAPIFileProcessingDirection.Config)
                    {
                        nudPostingConcurrent.Enabled = true;
                    }
                    else
                    {
                        nudPostingConcurrent.Enabled = false;
                    }
                }
            }
        }
        // End TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files

        // Begin TT#3993 - JSmith - Task File-SaveAs does not work

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

        private void cboBatchComp_SelectedChangeCommitted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                if (cboBatchComp.Parent.Visible)
                {
                    _currentBatchCompRow["BATCH_COMP_RID"] = cboBatchComp.SelectedValue;
                }
            }
        }
		// End TT1595-MD - stodd - batch comp

        // End TT#3993 - JSmith - Task File-SaveAs does not work
        private void btnHeaderReconcileHeaderKeys_Click(object sender, EventArgs e)
        {
            try
            {
                //ofdFile.InitialDirectory = "Desktop";
                ofdFile.FileName = this.txtHeaderReconcileRemoveTransactionsFileName.Text.Trim();
                ofdFile.Filter = "Text Files (*.txt)|*.txt";
                ofdFile.FilterIndex = 1;
                ofdFile.RestoreDirectory = true;

                if (ofdFile.ShowDialog() == DialogResult.OK)
                {
                    this.txtHeaderReconcileHeaderKeys.Text = ofdFile.FileName;
                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }

        }

        private void btnHeaderReconcileInputDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                fbdDirectory.SelectedPath = this.txtHeaderReconcileInputDirectory.Text.Trim();
                fbdDirectory.Description = "Select the directory where the Header Reconcile input file(s) will be found.";

                if (fbdDirectory.ShowDialog() == DialogResult.OK)
                {
                    txtHeaderReconcileInputDirectory.Text = fbdDirectory.SelectedPath;

                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }

        }

        private void btnHeaderReconcileOutputDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                fbdDirectory.SelectedPath = this.txtHeaderReconcileOutputDirectory.Text.Trim();
                fbdDirectory.Description = "Select the directory where the Header Reconcile output file(s) will be written.";

                if (fbdDirectory.ShowDialog() == DialogResult.OK)
                {
                    txtHeaderReconcileOutputDirectory.Text = fbdDirectory.SelectedPath;

                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }

        }

        public DataTable HeaderTypesGetDataTable()
        {
            try
            {
                if (_dtHeaderTypes == null)
                {
                    _dtHeaderTypes = new DataTable();
                    _dtHeaderTypes.Columns.Add("FIELD_NAME");
                    _dtHeaderTypes.Columns.Add("FIELD_INDEX", typeof(int));

                    //load header type
                    eHeaderType type;
                    DataTable dtTypes = MIDText.GetTextTypeValueFirst(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextValue, Convert.ToInt32(eHeaderType.Assortment), Convert.ToInt32(eHeaderType.Placeholder));

                    for (int i = dtTypes.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dRow = dtTypes.Rows[i];
                        if (!SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
                        {
                            if (Convert.ToInt32(dRow["TEXT_CODE"]) == Convert.ToInt32(eHeaderType.Assortment))
                            {
                                dtTypes.Rows.Remove(dRow);
                            }
                            else if (Convert.ToInt32(dRow["TEXT_CODE"]) == Convert.ToInt32(eHeaderType.Placeholder))
                            {
                                dtTypes.Rows.Remove(dRow);
                            }
                        }

                        type = (eHeaderType)Convert.ToInt32(dRow["TEXT_CODE"]);
                        // if size, use all statuses
                        if (SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                        {
                        }
                        else
                        {
                            // remove all size statuses
                            if (Enum.IsDefined(typeof(eNonSizeHeaderType), Convert.ToInt32(type)))
                            {
                            }
                            else
                            {
                                dtTypes.Rows.Remove(dRow);
                            }
                        }

                    }
                    foreach (DataRow dr in dtTypes.Rows)
                    {
                        DataRow dr1 = _dtHeaderTypes.NewRow();
                        dr1["FIELD_NAME"] = (string)dr["TEXT_VALUE"];
                        dr1["FIELD_INDEX"] = Convert.ToInt32(dr["TEXT_CODE"]);
                        _dtHeaderTypes.Rows.Add(dr1);
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }

            return _dtHeaderTypes;
        }

        private void txtHeaderReconcileInputDirectory_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtHeaderReconcileInputDirectory.Parent.Visible)
                {
                    _currentHeaderReconcileRow["INPUT_DIRECTORY"] = this.txtHeaderReconcileInputDirectory.Text.Trim();

                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void txtHeaderReconcileOutputDirectory_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtHeaderReconcileOutputDirectory.Parent.Visible)
                {
                    _currentHeaderReconcileRow["OUTPUT_DIRECTORY"] = this.txtHeaderReconcileOutputDirectory.Text.Trim();

                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void txtHeaderReconcileTriggerSuffix_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtHeaderReconcileTriggerSuffix.Parent.Visible)
                {
                    _currentHeaderReconcileRow["TRIGGER_SUFFIX"] = this.txtHeaderReconcileTriggerSuffix.Text.Trim();

                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void txtHeaderReconcileRemoveTransactionsFileName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtHeaderReconcileRemoveTransactionsFileName.Parent.Visible)
                {
                    _currentHeaderReconcileRow["REMOVE_TRANS_FILE_NAME"] = this.txtHeaderReconcileRemoveTransactionsFileName.Text.Trim();

                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void txtHeaderReconcileRemoveTransactionsTriggerSuffix_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtHeaderReconcileRemoveTransactionsTriggerSuffix.Parent.Visible)
                {
                    _currentHeaderReconcileRow["REMOVE_TRANS_TRIGGER_SUFFIX"] = this.txtHeaderReconcileRemoveTransactionsTriggerSuffix.Text.Trim();

                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void txtHeaderReconcileHeaderKeys_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtHeaderReconcileHeaderKeys.Parent.Visible)
                {
                    _currentHeaderReconcileRow["HEADER_KEYS_FILE_NAME"] = this.txtHeaderReconcileHeaderKeys.Text.Trim();

                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void clbHeaderReconcileHeaderTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void clbHeaderReconcileHeaderTypes_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            List<string> headerTypeArray = null;
            try
            {
                if (this.clbHeaderReconcileHeaderTypes.Parent.Visible)
                {
                    string headerTypes = string.Empty;
                    if (_currentHeaderReconcileRow["HEADER_TYPES"] != DBNull.Value)
                    {
                        headerTypes = _currentHeaderReconcileRow["HEADER_TYPES"].ToString();
                    }

                    if (headerTypes != string.Empty)
                    {
                        headerTypeArray = MIDstringTools.SplitGeneric(headerTypes, ',', true);
                    }
                    else
                    {
                        headerTypeArray = new List<string>();
                    }

                    string clickedHeaderType = clbHeaderReconcileHeaderTypes.Text;
                    // BEGIN TT#1622-MD - AGallagher - Header Reconcile Header Types are Saving Unchecked Types
                    switch (clickedHeaderType)
                    {
                        case "Purchase Order":
                            clickedHeaderType = "PO";
                            break;

                        case "Drop Ship":
                            clickedHeaderType = "DropShip";
                            break;

                        case "Workup Total Buy":
                            clickedHeaderType = "WorkupTotalBuy";
                            break;
                    }
                    // END TT#1622-MD - AGallagher - Header Reconcile Header Types are Saving Unchecked Types
                    if (e.NewValue == CheckState.Checked)
                    {
                        headerTypeArray.Add(clickedHeaderType);
                    }
                    else
                    {
                        headerTypeArray.Remove(clickedHeaderType);
                    }

                    // Reload
                    headerTypes = string.Empty;
                    for (int i = 0; i < headerTypeArray.Count; i++)
                    {
                        string aHeaderType = headerTypeArray[i];
                        // BEGIN TT#1622-MD - AGallagher - Header Reconcile Header Types are Saving Unchecked Types
                        //switch (aHeaderType)
                        //{
                        //    case "Purchase Order":
                        //        aHeaderType = "PO";
                        //        break;

                        //    case "Drop Ship":
                        //        aHeaderType = "DropShip";
                        //        break;

                        //    case "Workup Total Buy":
                        //        aHeaderType = "WorkupTotalBuy";
                        //        break;
                        //}
                        // END TT#1622-MD - AGallagher - Header Reconcile Header Types are Saving Unchecked Types

                        if (headerTypes != string.Empty)
                        {
                            headerTypes += ",";
                        }
                        if (!string.IsNullOrEmpty(aHeaderType))
                        {
                            headerTypes += aHeaderType;
                        }
                    }

                    _currentHeaderReconcileRow["HEADER_TYPES"] = headerTypes;

                        //// First show the index and check state of all selected items. 
                        //foreach (int indexChecked in clbHeaderReconcileHeaderTypes.CheckedIndices)
                        //{
                        //    // The indexChecked variable contains the index of the item.
                        //    MessageBox.Show("Index#: " + indexChecked.ToString() + ", is checked. Checked state is:" +
                        //                    clbHeaderReconcileHeaderTypes.GetItemCheckState(indexChecked).ToString() + ".");
                        //}

                        // Next show the object title and check state for each item selected. 
                        //foreach (object itemChecked in clbHeaderReconcileHeaderTypes.CheckedItems)
                        //{

                        //    DataRow aRow = clbHeaderReconcileHeaderTypes.Items.IndexOf(itemChecked) as DataRow;

                        //    // Use the IndexOf method to get the index of an item.
                        //    MessageBox.Show("Item with title: \"" + itemChecked.ToString() +
                        //                    "\", is checked. Checked state is: " +
                        //                    clbHeaderReconcileHeaderTypes.GetItemCheckState(clbHeaderReconcileHeaderTypes.Items.IndexOf(itemChecked)).ToString() + ".");
                        //}




                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }

        }

        private void clbHeaderReconcileHeaderTypes_Click(object sender, EventArgs e)
        {
           
        }

	}

	abstract public class DataSetEntry
	{
		//=======
		// FIELDS
		//=======

		private DataTable _mainTable;
		private DataTable _detailTable;
		private bool _isInitialized;
		private DataSet _dataSet;
		private string _detailSeqColName;

		//=============
		// CONSTRUCTORS
		//=============

		public DataSetEntry(DataTable aMainTable, DataTable aDetailTable, string aDetailSeqColName)
		{
			try
			{
				_mainTable = aMainTable.Clone();
				_detailTable = aDetailTable.Clone();
				_detailSeqColName = aDetailSeqColName;
				_isInitialized = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public DataTable MainDataTable
		{
			get
			{
				return _mainTable;
			}
		}

		public DataTable DetailDataTable
		{
			get
			{
				return _detailTable;
			}
		}

		public DataSet DataSet
		{
			get
			{
				try
				{
					if (!_isInitialized)
					{
						Initialize();
					}

					return _dataSet;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//========
		// METHODS
		//========

		public void AddMainRow(DataRow aRow)
		{
			DataRow row;

			try
			{
				row = _mainTable.NewRow();
				row.ItemArray = aRow.ItemArray;
				_mainTable.Rows.Add(row);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void AddDetailRow(DataRow aRow)
		{
			DataRow row;

			try
			{
				row = _detailTable.NewRow();
				row.ItemArray = aRow.ItemArray;
				_detailTable.Rows.Add(row);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void Initialize()
		{
			try
			{
				_dataSet = MIDEnvironment.CreateDataSet();
				_dataSet.Tables.Add(_mainTable);
				_dataSet.Tables.Add(_detailTable);
				_dataSet.Relations.Add(
					"Detail Relation",
					new DataColumn[] {_mainTable.Columns["TASKLIST_RID"], _mainTable.Columns["TASK_SEQUENCE"], _mainTable.Columns[_detailSeqColName]}, 
					new DataColumn[] {_detailTable.Columns["TASKLIST_RID"], _detailTable.Columns["TASK_SEQUENCE"], _detailTable.Columns[_detailSeqColName]},
					false);
				_isInitialized = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	public class ForecastDataSetEntry : DataSetEntry
	{
		public ForecastDataSetEntry(DataTable aMainTable, DataTable aDetailTable)
			: base(aMainTable, aDetailTable, "FORECAST_SEQUENCE")
		{
		}
	}

	public class AllocateDataSetEntry : DataSetEntry
	{
		public AllocateDataSetEntry(DataTable aMainTable, DataTable aDetailTable)
			: base(aMainTable, aDetailTable, "ALLOCATE_SEQUENCE")
		{
		}
	}

	public class DataTableEntry
	{
		//=======
		// FIELDS
		//=======

		private DataTable _table;

		//=============
		// CONSTRUCTORS
		//=============

		public DataTableEntry(DataTable aTable)
		{
			try
			{
				_table = aTable.Clone();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public DataTable DataTable
		{
			get
			{
				return _table;
			}
		}

		//========
		// METHODS
		//========

		public void AddRow(DataRow aRow)
		{
			DataRow row;

			try
			{
				row = _table.NewRow();
				row.ItemArray = aRow.ItemArray;
				_table.Rows.Add(row);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	public class AddMenuEntry
	{
		//=======
		// FIELDS
		//=======

		public string DisplayName;
		public eTaskType TaskType;

		//=============
		// CONSTRUCTORS
		//=============

		public AddMenuEntry(string aDisplayName, eTaskType aTaskType)
		{
			DisplayName = aDisplayName;
			TaskType = aTaskType;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	public class HierarchyLevelValueItem
	{
		//=======
		// FIELDS
		//=======

		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		//public eHierarchyDescendantType LevelType;
		//public int HierarchyRID;
		//public int LevelRID;
		//public int Offset;
		//public string LevelName;
		private eHierarchyDescendantType _levelType;
		private int _hierarchyRID;
		private int _levelRID;
		private int _offset;
		private string _levelName;
		//End TT#155 - JScott - Add Size Curve info to Node Properties

		//=============
		// CONSTRUCTORS
		//=============

		public HierarchyLevelValueItem(int aOffset, string aLevelName)
		{
			//Begin TT#155 - JScott - Add Size Curve info to Node Properties
			//LevelType = eHierarchyDescendantType.offset;
			//HierarchyRID = Include.NoRID;
			//LevelRID = Include.NoRID;
			//Offset = aOffset;
			//LevelName = aLevelName;
			_levelType = eHierarchyDescendantType.offset;
			_hierarchyRID = Include.NoRID;
			_levelRID = Include.NoRID;
			_offset = aOffset;
			_levelName = aLevelName;
			//End TT#155 - JScott - Add Size Curve info to Node Properties
		}

		public HierarchyLevelValueItem(int aHierarchyRID, int aLevelRID, string aLevelName)
		{
			//Begin TT#155 - JScott - Add Size Curve info to Node Properties
			//LevelType = eHierarchyDescendantType.levelType;
			//HierarchyRID = aHierarchyRID;
			//LevelRID = aLevelRID;
			//Offset = -1;
			//LevelName = aLevelName;
			_levelType = eHierarchyDescendantType.levelType;
			_hierarchyRID = aHierarchyRID;
			_levelRID = aLevelRID;
			_offset = -1;
			_levelName = aLevelName;
			//End TT#155 - JScott - Add Size Curve info to Node Properties
		}

		//===========
		// PROPERTIES
		//===========

		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		public eHierarchyDescendantType LevelType
		{
			get
			{
				return _levelType;
			}
			set
			{
				_levelType = value;
			}
		}

		public int HierarchyRID
		{
			get
			{
				return _hierarchyRID;
			}
			set
			{
				_hierarchyRID = value;
			}
		}

		public int LevelRID
		{
			get
			{
				return _levelRID;
			}
			set
			{
				_levelRID = value;
			}
		}

		public int Offset
		{
			get
			{
				return _offset;
			}
			set
			{
				_offset = value;
			}
		}

		public string LevelName
		{
			get
			{
				return _levelName;
			}
			set
			{
				_levelName = value;
			}
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		//========
		// METHODS
		//========
	}

	public class TaskListPropertiesSaveEventArgs : EventArgs
	{
		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		private MIDTaskListNode _parentNode;
		private TaskListProfile _taskListProf;

		public TaskListPropertiesSaveEventArgs(MIDTaskListNode aParentNode, TaskListProfile aTaskListProf)
		{
			_parentNode = aParentNode;
			_taskListProf = aTaskListProf;
		}

		public MIDTaskListNode ParentNode
		{
			get
			{
				return _parentNode;
			}
		}

		public TaskListProfile TaskListProfile
		{
			get
			{
				return _taskListProf;
			}
		}
		//End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
	}

	public class TaskListPropertiesCloseEventArgs : EventArgs
	{
	}
}
