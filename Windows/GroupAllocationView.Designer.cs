using MIDRetail.Windows.Controls;
using MIDRetail.Business;
namespace MIDRetail.Windows
{
	partial class GroupAllocationView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			if (disposing)
			{
                //  at bottom of method
                //if (_transaction != null)
                //{
                //    _transaction.Dispose();
                //}

				//this.btnApply.Click -= new System.EventHandler(this.btnApply_Click);
				//this.cboFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
				//this.cboFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
				//this.cboFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
				////Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
				//this.cboFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboFilter_MIDComboBoxPropertiesChangedEvent);
				////End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
				//this.cboFilter.DropDown -= new System.EventHandler(this.cboFilter_DropDown);
				//// Begin TT#301-MD - JSmith - Controls are not functioning properly
				//this.cboFilter.DropDownClosed -= new System.EventHandler(cboFilter_DropDownClosed);
				//// End TT#301-MD - JSmith - Controls are not functioning properly
				//this.cboView.SelectionChangeCommitted -= new System.EventHandler(this.cboView_SelectionChangeCommitted);
				////Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
				//this.cboView.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboView_MIDComboBoxPropertiesChangedEvent);
				////End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
				//// Begin TT#301-MD - JSmith - Controls are not functioning properly
				//this.cboView.DropDownClosed -= new System.EventHandler(cboView_DropDownClosed);
				//// End TT#301-MD - JSmith - Controls are not functioning properly
				//this.cboStoreGroupLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreGroupLevel_SelectionChangeCommitted);
				////Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
				//this.cboStoreGroupLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreGroupLevel_MIDComboBoxPropertiesChangedEvent);
				////End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
				//this.cboStoreGroup.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreGroup_SelectionChangeCommitted);
				////Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
				//this.cboStoreGroup.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreGroup_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
				//// Begin TT#301-MD - JSmith - Controls are not functioning properly
				//this.cboStoreGroupLevel.DropDownClosed -= new System.EventHandler(cboStoreGroupLevel_DropDownClosed);
				//// End TT#301-MD - JSmith - Controls are not functioning properly
				//this.g4.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
				//this.g4.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				//this.g4.Resize -= new System.EventHandler(this.g4_Resize);
				//this.g4.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g4_OwnerDrawCell);
				//this.g4.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g4_BeforeScroll);
				//this.cmiRowChooser.Click -= new System.EventHandler(this.cmiRowChooser_Click);
				//this.cmiLockRow.Click -= new System.EventHandler(this.cmiLockRow_Click);
				//this.cmiUnlockRow.Click -= new System.EventHandler(this.cmiUnlockRow_Click);
				//this.g7.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
				//this.g7.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				//this.g7.Resize -= new System.EventHandler(this.g7_Resize);
				//this.g7.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g7_OwnerDrawCell);
				//this.g7.VisibleChanged -= new System.EventHandler(this.g7_VisibleChanged);
				//this.g7.Click -= new System.EventHandler(this.g7_Click);
				//this.g7.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g7_BeforeScroll);
				//this.spcHScrollLevel1.DoubleClick -= new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
				//this.spcHScrollLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				//this.spcHScrollLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
				//this.cmiLockSplitter.Click -= new System.EventHandler(this.cmiLockSplitter_Click);
				//this.spcHScrollLevel2.DoubleClick -= new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
				//this.spcHScrollLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				//this.spcHScrollLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
				//this.vScrollBar2.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.vScrollBar2_Scroll);
				//this.vScrollBar3.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.vScrollBar3_Scroll);
				//this.g5.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				//this.g5.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				//this.g5.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				//this.g5.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g5_OwnerDrawCell);
				//this.g5.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				//this.g5.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g5_BeforeScroll);
				//this.g5.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				//this.cmiLockCell.Click -= new System.EventHandler(this.cmiLockCell_Click);
				//this.g2.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.g2_MouseMove);
				//this.g2.BeforeResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.BeforeResizeColumn);
				//this.g2.DragEnter -= new System.Windows.Forms.DragEventHandler(this.g2_DragEnter);
				//this.g2.DragOver -= new System.Windows.Forms.DragEventHandler(this.g2_DragOver);
				//this.g2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				//this.g2.Resize -= new System.EventHandler(this.g2_Resize);
				//this.g2.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_AfterResizeColumn);
				//this.g2.QueryContinueDrag -= new System.Windows.Forms.QueryContinueDragEventHandler(this.g2_QueryContinueDrag);
				//this.g2.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.g2_MouseUp);
				//this.g1.VisibleChanged -= new System.EventHandler(this.g1_VisibleChanged);
				//this.g2.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g2_BeforeScroll);
				//this.g2.DragDrop -= new System.Windows.Forms.DragEventHandler(this.g2_DragDrop);
				//this.g2.BeforeAutosizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_BeforeAutosizeColumn);
				////Begin TT#1196 - JScott - Average units in the summary section should spread when changed
				//this.g2.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				//this.g2.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				//End TT#1196 - JScott - Average units in the summary section should spread when changed
				//this.cmsGrid.Opening -= new System.ComponentModel.CancelEventHandler(this.cmsGrid_Opening);
				//this.cmiColChooser.Click -= new System.EventHandler(this.cmiColChooser_Click);
				//this.cmiLockColumn.Click -= new System.EventHandler(this.cmiLockColumn_Click);
				//this.cmiUnlockColumn.Click -= new System.EventHandler(this.cmiUnlockColumn_Click);
				//this.cmiFreezeColumn.Click -= new System.EventHandler(this.cmiFreezeColumn_Click);
				//this.g8.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				//this.g8.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				//this.g8.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				//this.g8.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g8_OwnerDrawCell);
				//this.g8.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				//this.g8.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g8_BeforeScroll);
				//this.g8.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				//this.hScrollBar2.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
				//this.g6.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				//this.g6.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				//this.g6.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				//this.g6.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g6_OwnerDrawCell);
				//this.g6.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				//this.g6.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g6_BeforeScroll);
				//this.g6.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				//this.g3.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.g3_MouseMove);
				//this.g3.BeforeResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.BeforeResizeColumn);
				//this.g3.DragEnter -= new System.Windows.Forms.DragEventHandler(this.g3_DragEnter);
				//this.g3.DragOver -= new System.Windows.Forms.DragEventHandler(this.g3_DragOver);
				//this.g3.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				//this.g3.Resize -= new System.EventHandler(this.g3_Resize);
				//this.g3.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g3_OwnerDrawCell);
				//this.g3.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_AfterResizeColumn);
				//this.g3.QueryContinueDrag -= new System.Windows.Forms.QueryContinueDragEventHandler(this.g3_QueryContinueDrag);
				//this.g3.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.g3_MouseUp);
				//this.g3.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g3_BeforeScroll);
				//this.g3.DragDrop -= new System.Windows.Forms.DragEventHandler(this.g3_DragDrop);
				//this.g3.BeforeAutosizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_BeforeAutosizeColumn);
				////Begin TT#1196 - JScott - Average units in the summary section should spread when changed
				//this.g3.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				//this.g3.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				////End TT#1196 - JScott - Average units in the summary section should spread when changed
				//this.g9.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				//this.g9.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				//this.g9.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				//this.g9.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g9_OwnerDrawCell);
				//this.g9.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				//this.g9.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g9_BeforeScroll);
				//this.g9.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				//this.hScrollBar3.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.hScrollBar3_Scroll);
				//this.utmMain.ToolClick -= new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.utmMain_ToolClick);
				//this.spcHDetailLevel1.DoubleClick -= new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
				//this.spcHDetailLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				//this.spcHDetailLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
				//this.spcHDetailLevel2.DoubleClick -= new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
				//this.spcHDetailLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				//this.spcHDetailLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
				//this.spcHHeaderLevel1.DoubleClick -= new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
				//this.spcHHeaderLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				//this.spcHHeaderLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
				//this.spcHHeaderLevel2.DoubleClick -= new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
				//this.spcHHeaderLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				//this.spcHHeaderLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
				//this.cmiLockSheet.Click -= new System.EventHandler(this.cmiLockSheet_Click);
				//this.cmiUnlockSheet.Click -= new System.EventHandler(this.cmiUnlockSheet_Click);
				//this.spcHTotalLevel1.DoubleClick -= new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
				//this.spcHTotalLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				//this.spcHTotalLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
				//this.spcHTotalLevel2.DoubleClick -= new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
				//this.spcHTotalLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				//this.spcHTotalLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
				//this.spcVLevel1.DoubleClick -= new System.EventHandler(this.spcVLevel1_DoubleClick);
				//this.spcVLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				//this.spcVLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcVLevel1_SplitterMoved);
				//this.spcVLevel2.DoubleClick -= new System.EventHandler(this.spcVLevel2_DoubleClick);
				//this.spcVLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				//this.spcVLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcVLevel2_SplitterMoved);
				//this.Activated -= new System.EventHandler(this.AssortmentView_Activated);
				this.Closing -= new System.ComponentModel.CancelEventHandler(this.AssortmentView_Closing);
				this.Load -= new System.EventHandler(this.AssortmentView_Load);
                this.tabControl.SelectedIndexChanged -= new System.EventHandler(this.tabControl_SelectedIndexChanged);

                this.ugDetails.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDetails_ClickCellButton);
                this.ugDetails.AfterRowsDeleted -= new System.EventHandler(this.ugDetails_AfterRowsDeleted);
                this.ugDetails.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugDetails_BeforeCellUpdate);
                this.ugDetails.Error -= new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.ugDetails_Error);
                this.ugDetails.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugDetails_MouseDown);
                this.ugDetails.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugDetails_InitializeLayout);
                this.ugDetails.AfterSelectChange -= new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ugDetails_AfterSelectChange);
                this.ugDetails.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugDetails_BeforeRowsDeleted);
                this.ugDetails.DragOver -= new System.Windows.Forms.DragEventHandler(this.ugDetails_DragOver);
                this.ugDetails.BeforeColumnChooserDisplayed -= new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.ugDetails_BeforeColumnChooserDisplayed);
                this.ugDetails.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.ugDetails_BeforeCellDeactivate);
                this.ugDetails.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDetails_AfterCellUpdate);
                this.ugDetails.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugDetails_InitializeRow);
                this.ugDetails.DragLeave -= new System.EventHandler(this.ugDetails_DragLeave);
                this.ugDetails.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDetails_CellChange);
                this.ugDetails.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugDetails_MouseEnterElement);
                this.ugDetails.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugDetails_DragDrop);
                this.ugDetails.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.ugDetails_KeyDown);
                this.ugDetails.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugDetails_AfterSortChange);
                this.ugDetails.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugDetails_DragEnter);
                this.ugDetails.BeforeRowDeactivate -= new System.ComponentModel.CancelEventHandler(this.ugDetails_BeforeRowDeactivate);                 
                
				//this.ugCharacteristics.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugCharacteristics_MouseDown);
				//this.ugCharacteristics.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugCharacteristics_InitializeLayout);
				//this.ugCharacteristics.DragOver -= new System.Windows.Forms.DragEventHandler(this.ugCharacteristics_DragOver);
				//this.ugCharacteristics.DragLeave -= new System.EventHandler(this.ugCharacteristics_DragLeave);
				//this.ugCharacteristics.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugCharacteristics_CellChange);
				//this.ugCharacteristics.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugCharacteristics_MouseEnterElement);
				//this.ugCharacteristics.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugCharacteristics_DragDrop);
				//this.ugCharacteristics.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugCharacteristics_DragEnter);

				//g2MouseUpRefireHandler -= new System.Windows.Forms.MouseEventHandler(this.g2_MouseUp);
				//g3MouseUpRefireHandler -= new System.Windows.Forms.MouseEventHandler(this.g3_MouseUp);

				_sab.ProcessMethodOnAssortmentEvent.OnProcessMethodOnAssortmentEventHandler -= new ProcessMethodOnAssortmentEvent.ProcessMethodOnAssortmentEventHandler(OnProcessMethodOnAssortmentEvent);
				_sab.AssortmentSelectedHeaderEvent.OnAssortmentSelectedHeaderEventHandler -= new AssortmentSelectedHeaderEvent.AssortmentSelectedHeaderEventHandler(OnAssortmentSelectedHeaderEvent);

				if (_saveForm != null)
				{
					_saveForm.OnAssortmentSaveClosingEventHandler -= new AssortmentViewSave.AssortmentSaveClosingEventHandler(OnAssortmentSaveClosing);
				}

				if (_frmThemeProperties != null)
				{
					_frmThemeProperties.ApplyButtonClicked -= new System.EventHandler(StylePropertiesOnChanged);
				}

				// BEGIN TT#488-MD - STodd - Group Allocation 
				if (_headerList != null)
				{
					foreach (MIDRetail.Business.Allocation.AllocationHeaderProfile ahp in _headerList)
					{
						if (ahp.HeaderType == MIDRetail.DataCommon.eHeaderType.Assortment)
						{
							_sab.ApplicationServerSession.RemoveOpenAsrtView(ahp.Key);
						}
					}
				}
				// END TT#488-MD - STodd - Group Allocation 
				if (_transaction.StyleView == null &&
					_transaction.SummaryView == null &&
					_transaction.SizeView == null &&
					_transaction.AssortmentView == null &&
					_transaction.VelocityWindow == null)
				{
					_transaction.Dispose();
				}
			}
			
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("UltraToolbar1");
			Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool1 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("actionComboBox");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnProcess");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Grid");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblProcessBy");
			Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool1 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainer");
			Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool2 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("TextBoxTool1");
			Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("actionComboBox");
			Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
			Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnProcess");
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupAllocationView));
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Grid");
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Expand All");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Collapse All");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnExport");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnEmail");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Expand All");
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Collapse All");
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnExport");
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnEmail");
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnSaveView");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblProcessBy");
			Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool5 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainer");
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
			this.pnlTop = new System.Windows.Forms.Panel();
			this.midToolbarRadioButton1 = new MIDRetail.Windows.Controls.MIDToolbarRadioButton();
			this.pnlTop_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
			this._pnlTop_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this._pnlTop_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._pnlTop_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._pnlTop_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this.cmsGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmiColChooser = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiRowChooser = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiLockSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.cmiLockCell = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiLockColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiUnlockColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiLockRow = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiUnlockRow = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiLockSheet = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiUnlockSheet = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCascadeLockCell = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCascadeUnlockCell = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCascadeLockColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCascadeUnlockColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCascadeLockRow = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCascadeUnlockRow = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCascadeLockSection = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCascadeUnlockSection = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiFreezeSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.cmiFreezeColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCloseStyle = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCloseColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiOpenColumn = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiCloseRow = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiOpenRow = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsSplitter = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmiLockSplitter = new System.Windows.Forms.ToolStripMenuItem();
			this.rtbScrollText = new System.Windows.Forms.RichTextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this._AssortmentView_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._AssortmentView_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._AssortmentView_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._AssortmentView_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this.lblFindSize = new System.Windows.Forms.Label();
			this.cmsActions = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmiAssortmentActions = new System.Windows.Forms.ToolStripMenuItem();
			this.cmiAllocationActions = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsContentGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsInsert = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsContentInsert = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsInsertPhStyle = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsInsertPhTextBox = new System.Windows.Forms.ToolStripTextBox();
			this.cmsInsertPack = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsInsertPackColor = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsContentInsertColor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsInsertMTColorRow = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsChooseColor = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsInsertBulkColor = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsInsertPackSize = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsInsertBulkSize = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsContentColChooser = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmsColSelectrAll = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsColClearAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tabContent = new System.Windows.Forms.TabPage();
			this.ugDetails = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.tabControl = new System.Windows.Forms.TabControl();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			this.pnlTop.SuspendLayout();
			this.pnlTop_Fill_Panel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
			this.cmsGrid.SuspendLayout();
			this.cmsSplitter.SuspendLayout();
			this.cmsActions.SuspendLayout();
			this.cmsContentGrid.SuspendLayout();
			this.cmsContentInsert.SuspendLayout();
			this.cmsContentInsertColor.SuspendLayout();
			this.cmsContentColChooser.SuspendLayout();
			this.tabContent.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ugDetails)).BeginInit();
			this.tabControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// utmMain
			// 
			this.utmMain.DockWithinContainerBaseType = typeof(MIDRetail.Windows.MIDFormBase);
			this.utmMain.MenuSettings.ForceSerialization = true;
			this.utmMain.ToolbarSettings.ForceSerialization = true;
			// 
			// pnlTop
			// 
			this.pnlTop.AutoSize = true;
			this.pnlTop.Controls.Add(this.midToolbarRadioButton1);
			this.pnlTop.Controls.Add(this.pnlTop_Fill_Panel);
			this.pnlTop.Controls.Add(this._pnlTop_Toolbars_Dock_Area_Left);
			this.pnlTop.Controls.Add(this._pnlTop_Toolbars_Dock_Area_Right);
			this.pnlTop.Controls.Add(this._pnlTop_Toolbars_Dock_Area_Bottom);
			this.pnlTop.Controls.Add(this._pnlTop_Toolbars_Dock_Area_Top);
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(0, 0);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(956, 30);
			this.pnlTop.TabIndex = 0;
			// 
			// midToolbarRadioButton1
			// 
			this.midToolbarRadioButton1.AutoSize = true;
			this.midToolbarRadioButton1.BackColor = System.Drawing.SystemColors.Control;
			this.midToolbarRadioButton1.Location = new System.Drawing.Point(762, 3);
			this.midToolbarRadioButton1.Name = "midToolbarRadioButton1";
			this.midToolbarRadioButton1.RadioButton1Text = "radioButton1";
			this.midToolbarRadioButton1.RadioButton2Text = "radioButton2";
			this.midToolbarRadioButton1.Size = new System.Drawing.Size(182, 24);
			this.midToolbarRadioButton1.TabIndex = 20;
			// 
			// pnlTop_Fill_Panel
			// 
			this.pnlTop_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
			this.pnlTop_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlTop_Fill_Panel.Location = new System.Drawing.Point(0, 25);
			this.pnlTop_Fill_Panel.Name = "pnlTop_Fill_Panel";
			this.pnlTop_Fill_Panel.Size = new System.Drawing.Size(956, 5);
			this.pnlTop_Fill_Panel.TabIndex = 0;
			// 
			// _pnlTop_Toolbars_Dock_Area_Left
			// 
			this._pnlTop_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._pnlTop_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._pnlTop_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._pnlTop_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._pnlTop_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
			this._pnlTop_Toolbars_Dock_Area_Left.Name = "_pnlTop_Toolbars_Dock_Area_Left";
			this._pnlTop_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 5);
			this._pnlTop_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
			// 
			// ultraToolbarsManager1
			// 
			this.ultraToolbarsManager1.DesignerFlags = 1;
			this.ultraToolbarsManager1.DockWithinContainer = this.pnlTop;
			this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
			ultraToolbar1.DockedColumn = 0;
			ultraToolbar1.DockedRow = 0;
			ultraToolbar1.FloatingLocation = new System.Drawing.Point(703, 309);
			ultraToolbar1.FloatingSize = new System.Drawing.Size(612, 48);
			comboBoxTool1.InstanceProps.Width = 206;
			popupMenuTool1.InstanceProps.IsFirstInGroup = true;
			labelTool1.InstanceProps.Width = 90;
			controlContainerTool1.ControlName = "midToolbarRadioButton1";
			controlContainerTool1.InstanceProps.Width = 182;
			ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            comboBoxTool1,
            buttonTool1,
            popupMenuTool1,
            labelTool1,
            controlContainerTool1});
			ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Text = "UltraToolbar1";
			this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
			textBoxTool2.SharedPropsInternal.Caption = "Group Allocation:";
			textBoxTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			textBoxTool2.SharedPropsInternal.Width = 300;
			comboBoxTool2.SharedPropsInternal.Caption = "Action:";
			comboBoxTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			comboBoxTool2.Text = "Select an action...";
			valueListItem1.DataValue = "Select an action...";
			valueListItem1.DisplayText = "Select an action...";
			valueList1.ValueListItems.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1});
			comboBoxTool2.ValueList = valueList1;
			appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
			buttonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance1;
			buttonTool2.SharedPropsInternal.Caption = "Process";
			buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
			popupMenuTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance2;
			popupMenuTool2.SharedPropsInternal.Caption = "Grid";
			popupMenuTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			buttonTool7.InstanceProps.IsFirstInGroup = true;
			popupMenuTool2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool3,
            buttonTool4,
            buttonTool7,
            buttonTool8});
			appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
			buttonTool5.SharedPropsInternal.AppearancesSmall.Appearance = appearance3;
			buttonTool5.SharedPropsInternal.Caption = "Expand All";
			appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
			buttonTool6.SharedPropsInternal.AppearancesSmall.Appearance = appearance4;
			buttonTool6.SharedPropsInternal.Caption = "Collapse All";
			appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
			buttonTool9.SharedPropsInternal.AppearancesSmall.Appearance = appearance5;
			buttonTool9.SharedPropsInternal.Caption = "Export";
			appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
			buttonTool10.SharedPropsInternal.AppearancesSmall.Appearance = appearance6;
			buttonTool10.SharedPropsInternal.Caption = "Email";
			buttonTool12.SharedPropsInternal.Caption = "Save View";
			buttonTool12.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			labelTool2.SharedPropsInternal.Caption = "Process As:";
			controlContainerTool5.ControlName = "midToolbarRadioButton1";
			controlContainerTool5.SharedPropsInternal.Caption = "ControlContainer";
			controlContainerTool5.SharedPropsInternal.Width = 182;
			this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            textBoxTool2,
            comboBoxTool2,
            buttonTool2,
            popupMenuTool2,
            buttonTool5,
            buttonTool6,
            buttonTool9,
            buttonTool10,
            buttonTool12,
            labelTool2,
            controlContainerTool5});
			this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
			this.ultraToolbarsManager1.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(this.ultraToolbarsManager1_ToolValueChanged);
			// 
			// _pnlTop_Toolbars_Dock_Area_Right
			// 
			this._pnlTop_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._pnlTop_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._pnlTop_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._pnlTop_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._pnlTop_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(956, 25);
			this._pnlTop_Toolbars_Dock_Area_Right.Name = "_pnlTop_Toolbars_Dock_Area_Right";
			this._pnlTop_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 5);
			this._pnlTop_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
			// 
			// _pnlTop_Toolbars_Dock_Area_Bottom
			// 
			this._pnlTop_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._pnlTop_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._pnlTop_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._pnlTop_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._pnlTop_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 30);
			this._pnlTop_Toolbars_Dock_Area_Bottom.Name = "_pnlTop_Toolbars_Dock_Area_Bottom";
			this._pnlTop_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(956, 0);
			this._pnlTop_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
			// 
			// _pnlTop_Toolbars_Dock_Area_Top
			// 
			this._pnlTop_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._pnlTop_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._pnlTop_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._pnlTop_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._pnlTop_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._pnlTop_Toolbars_Dock_Area_Top.Name = "_pnlTop_Toolbars_Dock_Area_Top";
			this._pnlTop_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(956, 25);
			this._pnlTop_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
			// 
			// cmsGrid
			// 
			this.cmsGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiColChooser,
            this.cmiRowChooser,
            this.cmiLockSeparator,
            this.cmiLockCell,
            this.cmiLockColumn,
            this.cmiUnlockColumn,
            this.cmiLockRow,
            this.cmiUnlockRow,
            this.cmiLockSheet,
            this.cmiUnlockSheet,
            this.cmiCascadeLockCell,
            this.cmiCascadeUnlockCell,
            this.cmiCascadeLockColumn,
            this.cmiCascadeUnlockColumn,
            this.cmiCascadeLockRow,
            this.cmiCascadeUnlockRow,
            this.cmiCascadeLockSection,
            this.cmiCascadeUnlockSection,
            this.cmiFreezeSeparator,
            this.cmiFreezeColumn,
            this.cmiCloseStyle,
            this.cmiCloseColumn,
            this.cmiOpenColumn,
            this.cmiCloseRow,
            this.cmiOpenRow});
			this.cmsGrid.Name = "cmsGrid";
			this.cmsGrid.Size = new System.Drawing.Size(205, 522);
			this.cmsGrid.Opening += new System.ComponentModel.CancelEventHandler(this.cmsGrid_Opening);
			// 
			// cmiColChooser
			// 
			this.cmiColChooser.Name = "cmiColChooser";
			this.cmiColChooser.Size = new System.Drawing.Size(204, 22);
			this.cmiColChooser.Text = "Column Chooser...";
			// 
			// cmiRowChooser
			// 
			this.cmiRowChooser.Name = "cmiRowChooser";
			this.cmiRowChooser.Size = new System.Drawing.Size(204, 22);
			this.cmiRowChooser.Text = "Row Chooser...";
			// 
			// cmiLockSeparator
			// 
			this.cmiLockSeparator.Name = "cmiLockSeparator";
			this.cmiLockSeparator.Size = new System.Drawing.Size(201, 6);
			// 
			// cmiLockCell
			// 
			this.cmiLockCell.Name = "cmiLockCell";
			this.cmiLockCell.Size = new System.Drawing.Size(204, 22);
			this.cmiLockCell.Text = "Lock Cell";
			// 
			// cmiLockColumn
			// 
			this.cmiLockColumn.Name = "cmiLockColumn";
			this.cmiLockColumn.Size = new System.Drawing.Size(204, 22);
			this.cmiLockColumn.Text = "Lock Column";
			// 
			// cmiUnlockColumn
			// 
			this.cmiUnlockColumn.Name = "cmiUnlockColumn";
			this.cmiUnlockColumn.Size = new System.Drawing.Size(204, 22);
			this.cmiUnlockColumn.Text = "Unlock Column";
			// 
			// cmiLockRow
			// 
			this.cmiLockRow.Name = "cmiLockRow";
			this.cmiLockRow.Size = new System.Drawing.Size(204, 22);
			this.cmiLockRow.Text = "Lock Row";
			// 
			// cmiUnlockRow
			// 
			this.cmiUnlockRow.Name = "cmiUnlockRow";
			this.cmiUnlockRow.Size = new System.Drawing.Size(204, 22);
			this.cmiUnlockRow.Text = "Unlock Row";
			// 
			// cmiLockSheet
			// 
			this.cmiLockSheet.Name = "cmiLockSheet";
			this.cmiLockSheet.Size = new System.Drawing.Size(204, 22);
			this.cmiLockSheet.Text = "Lock Sheet";
			// 
			// cmiUnlockSheet
			// 
			this.cmiUnlockSheet.Name = "cmiUnlockSheet";
			this.cmiUnlockSheet.Size = new System.Drawing.Size(204, 22);
			this.cmiUnlockSheet.Text = "Unlock Sheet";
			// 
			// cmiCascadeLockCell
			// 
			this.cmiCascadeLockCell.Name = "cmiCascadeLockCell";
			this.cmiCascadeLockCell.Size = new System.Drawing.Size(204, 22);
			this.cmiCascadeLockCell.Text = "Cascade Lock Cell";
			// 
			// cmiCascadeUnlockCell
			// 
			this.cmiCascadeUnlockCell.Name = "cmiCascadeUnlockCell";
			this.cmiCascadeUnlockCell.Size = new System.Drawing.Size(204, 22);
			this.cmiCascadeUnlockCell.Text = "Cascade Unlock Cell";
			// 
			// cmiCascadeLockColumn
			// 
			this.cmiCascadeLockColumn.Name = "cmiCascadeLockColumn";
			this.cmiCascadeLockColumn.Size = new System.Drawing.Size(204, 22);
			this.cmiCascadeLockColumn.Text = "Cascade Lock Column";
			// 
			// cmiCascadeUnlockColumn
			// 
			this.cmiCascadeUnlockColumn.Name = "cmiCascadeUnlockColumn";
			this.cmiCascadeUnlockColumn.Size = new System.Drawing.Size(204, 22);
			this.cmiCascadeUnlockColumn.Text = "Cascade Unlock Column";
			// 
			// cmiCascadeLockRow
			// 
			this.cmiCascadeLockRow.Name = "cmiCascadeLockRow";
			this.cmiCascadeLockRow.Size = new System.Drawing.Size(204, 22);
			this.cmiCascadeLockRow.Text = "Cascade Lock Row";
			// 
			// cmiCascadeUnlockRow
			// 
			this.cmiCascadeUnlockRow.Name = "cmiCascadeUnlockRow";
			this.cmiCascadeUnlockRow.Size = new System.Drawing.Size(204, 22);
			this.cmiCascadeUnlockRow.Text = "Cascade Unlock Row";
			// 
			// cmiCascadeLockSection
			// 
			this.cmiCascadeLockSection.Name = "cmiCascadeLockSection";
			this.cmiCascadeLockSection.Size = new System.Drawing.Size(204, 22);
			this.cmiCascadeLockSection.Text = "Cascade Lock Section";
			// 
			// cmiCascadeUnlockSection
			// 
			this.cmiCascadeUnlockSection.Name = "cmiCascadeUnlockSection";
			this.cmiCascadeUnlockSection.Size = new System.Drawing.Size(204, 22);
			this.cmiCascadeUnlockSection.Text = "Cascade Unlock Section";
			// 
			// cmiFreezeSeparator
			// 
			this.cmiFreezeSeparator.Name = "cmiFreezeSeparator";
			this.cmiFreezeSeparator.Size = new System.Drawing.Size(201, 6);
			// 
			// cmiFreezeColumn
			// 
			this.cmiFreezeColumn.Name = "cmiFreezeColumn";
			this.cmiFreezeColumn.Size = new System.Drawing.Size(204, 22);
			this.cmiFreezeColumn.Text = "Freeze Column";
			// 
			// cmiCloseStyle
			// 
			this.cmiCloseStyle.Name = "cmiCloseStyle";
			this.cmiCloseStyle.Size = new System.Drawing.Size(204, 22);
			this.cmiCloseStyle.Text = "Close Style";
			// 
			// cmiCloseColumn
			// 
			this.cmiCloseColumn.Name = "cmiCloseColumn";
			this.cmiCloseColumn.Size = new System.Drawing.Size(204, 22);
			this.cmiCloseColumn.Text = "Close Column";
			// 
			// cmiOpenColumn
			// 
			this.cmiOpenColumn.Name = "cmiOpenColumn";
			this.cmiOpenColumn.Size = new System.Drawing.Size(204, 22);
			this.cmiOpenColumn.Text = "Open Column";
			// 
			// cmiCloseRow
			// 
			this.cmiCloseRow.Name = "cmiCloseRow";
			this.cmiCloseRow.Size = new System.Drawing.Size(204, 22);
			this.cmiCloseRow.Text = "Close Row";
			// 
			// cmiOpenRow
			// 
			this.cmiOpenRow.Name = "cmiOpenRow";
			this.cmiOpenRow.Size = new System.Drawing.Size(204, 22);
			this.cmiOpenRow.Text = "Open Row";
			// 
			// cmsSplitter
			// 
			this.cmsSplitter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiLockSplitter});
			this.cmsSplitter.Name = "cmsSplitter";
			this.cmsSplitter.Size = new System.Drawing.Size(140, 26);
			// 
			// cmiLockSplitter
			// 
			this.cmiLockSplitter.Name = "cmiLockSplitter";
			this.cmiLockSplitter.Size = new System.Drawing.Size(139, 22);
			this.cmiLockSplitter.Text = "Lock Splitter";
			// 
			// rtbScrollText
			// 
			this.rtbScrollText.BackColor = System.Drawing.SystemColors.Info;
			this.rtbScrollText.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtbScrollText.Location = new System.Drawing.Point(672, 384);
			this.rtbScrollText.Name = "rtbScrollText";
			this.rtbScrollText.ReadOnly = true;
			this.rtbScrollText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.rtbScrollText.Size = new System.Drawing.Size(100, 96);
			this.rtbScrollText.TabIndex = 0;
			this.rtbScrollText.Text = "";
			this.rtbScrollText.Visible = false;
			this.rtbScrollText.WordWrap = false;
			// 
			// _AssortmentView_Toolbars_Dock_Area_Left
			// 
			this._AssortmentView_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._AssortmentView_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._AssortmentView_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._AssortmentView_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._AssortmentView_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 0);
			this._AssortmentView_Toolbars_Dock_Area_Left.Name = "_AssortmentView_Toolbars_Dock_Area_Left";
			this._AssortmentView_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 383);
			this._AssortmentView_Toolbars_Dock_Area_Left.ToolbarsManager = this.utmMain;
			// 
			// _AssortmentView_Toolbars_Dock_Area_Right
			// 
			this._AssortmentView_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._AssortmentView_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._AssortmentView_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._AssortmentView_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._AssortmentView_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(956, 0);
			this._AssortmentView_Toolbars_Dock_Area_Right.Name = "_AssortmentView_Toolbars_Dock_Area_Right";
			this._AssortmentView_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 383);
			this._AssortmentView_Toolbars_Dock_Area_Right.ToolbarsManager = this.utmMain;
			// 
			// _AssortmentView_Toolbars_Dock_Area_Top
			// 
			this._AssortmentView_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._AssortmentView_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._AssortmentView_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._AssortmentView_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._AssortmentView_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._AssortmentView_Toolbars_Dock_Area_Top.Name = "_AssortmentView_Toolbars_Dock_Area_Top";
			this._AssortmentView_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(956, 0);
			this._AssortmentView_Toolbars_Dock_Area_Top.ToolbarsManager = this.utmMain;
			// 
			// _AssortmentView_Toolbars_Dock_Area_Bottom
			// 
			this._AssortmentView_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._AssortmentView_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._AssortmentView_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._AssortmentView_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._AssortmentView_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 383);
			this._AssortmentView_Toolbars_Dock_Area_Bottom.Name = "_AssortmentView_Toolbars_Dock_Area_Bottom";
			this._AssortmentView_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(956, 0);
			this._AssortmentView_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.utmMain;
			// 
			// lblFindSize
			// 
			this.lblFindSize.AutoSize = true;
			this.lblFindSize.Location = new System.Drawing.Point(672, 344);
			this.lblFindSize.Name = "lblFindSize";
			this.lblFindSize.Size = new System.Drawing.Size(35, 13);
			this.lblFindSize.TabIndex = 1;
			this.lblFindSize.Text = "label1";
			this.lblFindSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblFindSize.Visible = false;
			// 
			// cmsActions
			// 
			this.cmsActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiAssortmentActions,
            this.cmiAllocationActions});
			this.cmsActions.Name = "cmsActions";
			this.cmsActions.Size = new System.Drawing.Size(179, 48);
			// 
			// cmiAssortmentActions
			// 
			this.cmiAssortmentActions.Name = "cmiAssortmentActions";
			this.cmiAssortmentActions.Size = new System.Drawing.Size(178, 22);
			this.cmiAssortmentActions.Text = "Assortment Actions";
			// 
			// cmiAllocationActions
			// 
			this.cmiAllocationActions.Name = "cmiAllocationActions";
			this.cmiAllocationActions.Size = new System.Drawing.Size(178, 22);
			this.cmiAllocationActions.Text = "Allocation Actions";
			// 
			// cmsContentGrid
			// 
			this.cmsContentGrid.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuPopup;
			this.cmsContentGrid.AllowMerge = false;
			this.cmsContentGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsInsert,
            this.cmsRemove});
			this.cmsContentGrid.Name = "cmsContentGrid";
			this.cmsContentGrid.ShowImageMargin = false;
			this.cmsContentGrid.Size = new System.Drawing.Size(93, 48);
			this.cmsContentGrid.Opening += new System.ComponentModel.CancelEventHandler(this.cmsContentGrid_Opening);
			// 
			// cmsInsert
			// 
			this.cmsInsert.DropDown = this.cmsContentInsert;
			this.cmsInsert.Name = "cmsInsert";
			this.cmsInsert.Size = new System.Drawing.Size(92, 22);
			this.cmsInsert.Text = "Insert";
			// 
			// cmsContentInsert
			// 
			this.cmsContentInsert.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuPopup;
			this.cmsContentInsert.AllowMerge = false;
			this.cmsContentInsert.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsInsertPhStyle,
            this.cmsInsertPack,
            this.cmsInsertPackColor,
            this.cmsInsertPackSize,
            this.cmsInsertBulkColor,
            this.cmsInsertBulkSize});
			this.cmsContentInsert.Name = "cmsContentInsert";
			this.cmsContentInsert.OwnerItem = this.cmsInsert;
			this.cmsContentInsert.ShowImageMargin = false;
			this.cmsContentInsert.Size = new System.Drawing.Size(140, 136);
			// 
			// cmsInsertPhStyle
			// 
			this.cmsInsertPhStyle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsInsertPhTextBox});
			this.cmsInsertPhStyle.Name = "cmsInsertPhStyle";
			this.cmsInsertPhStyle.Size = new System.Drawing.Size(139, 22);
			this.cmsInsertPhStyle.Text = "Placeholder Style";
			this.cmsInsertPhStyle.DropDownOpening += new System.EventHandler(this.cmsInsertPhStyle_DropDownOpening);
			this.cmsInsertPhStyle.DropDownOpened += new System.EventHandler(this.cmsInsertPhStyle_DropDownOpened);
			// 
			// cmsInsertPhTextBox
			// 
			this.cmsInsertPhTextBox.BackColor = System.Drawing.SystemColors.Info;
			this.cmsInsertPhTextBox.Name = "cmsInsertPhTextBox";
			this.cmsInsertPhTextBox.Size = new System.Drawing.Size(21, 23);
			this.cmsInsertPhTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmsInsertPhTextBox_KeyUp);
			this.cmsInsertPhTextBox.DoubleClick += new System.EventHandler(this.cmsInsertPhTextBox_DoubleClick);
			// 
			// cmsInsertPack
			// 
			this.cmsInsertPack.Name = "cmsInsertPack";
			this.cmsInsertPack.Size = new System.Drawing.Size(139, 22);
			this.cmsInsertPack.Text = "Pack";
			this.cmsInsertPack.Click += new System.EventHandler(this.cmsInsertPack_Click);
			// 
			// cmsInsertPackColor
			// 
			this.cmsInsertPackColor.DropDown = this.cmsContentInsertColor;
			this.cmsInsertPackColor.Name = "cmsInsertPackColor";
			this.cmsInsertPackColor.Size = new System.Drawing.Size(139, 22);
			this.cmsInsertPackColor.Text = "Pack Color";
			// 
			// cmsContentInsertColor
			// 
			this.cmsContentInsertColor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsInsertMTColorRow,
            this.cmsChooseColor});
			this.cmsContentInsertColor.Name = "cmxContentInsertColor";
			this.cmsContentInsertColor.OwnerItem = this.cmsInsertPackColor;
			this.cmsContentInsertColor.ShowImageMargin = false;
			this.cmsContentInsertColor.Size = new System.Drawing.Size(124, 48);
			// 
			// cmsInsertMTColorRow
			// 
			this.cmsInsertMTColorRow.Name = "cmsInsertMTColorRow";
			this.cmsInsertMTColorRow.Size = new System.Drawing.Size(123, 22);
			this.cmsInsertMTColorRow.Text = "Empty Row";
			this.cmsInsertMTColorRow.Click += new System.EventHandler(this.cmsInsertMTColorRow_Click);
			// 
			// cmsChooseColor
			// 
			this.cmsChooseColor.Name = "cmsChooseColor";
			this.cmsChooseColor.Size = new System.Drawing.Size(123, 22);
			this.cmsChooseColor.Text = "Color Browser";
			this.cmsChooseColor.Click += new System.EventHandler(this.cmsChooseColor_Click);
			// 
			// cmsInsertBulkColor
			// 
			this.cmsInsertBulkColor.DropDown = this.cmsContentInsertColor;
			this.cmsInsertBulkColor.Name = "cmsInsertBulkColor";
			this.cmsInsertBulkColor.Size = new System.Drawing.Size(139, 22);
			this.cmsInsertBulkColor.Text = "Bulk Color";
			// 
			// cmsInsertPackSize
			// 
			this.cmsInsertPackSize.Name = "cmsInsertPackSize";
			this.cmsInsertPackSize.Size = new System.Drawing.Size(139, 22);
			this.cmsInsertPackSize.Text = "Pack Size";
			this.cmsInsertPackSize.Click += new System.EventHandler(this.cmsInsertPackSize_Click);
			// 
			// cmsInsertBulkSize
			// 
			this.cmsInsertBulkSize.Name = "cmsInsertBulkSize";
			this.cmsInsertBulkSize.Size = new System.Drawing.Size(139, 22);
			this.cmsInsertBulkSize.Text = "Bulk Size";
			this.cmsInsertBulkSize.Click += new System.EventHandler(this.cmsInsertBulkSize_Click);
			// 
			// cmsRemove
			// 
			this.cmsRemove.Name = "cmsRemove";
			this.cmsRemove.Size = new System.Drawing.Size(92, 22);
			this.cmsRemove.Text = "Remove";
			this.cmsRemove.Click += new System.EventHandler(this.cmsRemove_Click);
			// 
			// cmsContentColChooser
			// 
			this.cmsContentColChooser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsColSelectrAll,
            this.cmsColClearAll});
			this.cmsContentColChooser.Name = "cmsContentColChooser";
			this.cmsContentColChooser.Size = new System.Drawing.Size(123, 48);
			// 
			// cmsColSelectrAll
			// 
			this.cmsColSelectrAll.Name = "cmsColSelectrAll";
			this.cmsColSelectrAll.Size = new System.Drawing.Size(122, 22);
			this.cmsColSelectrAll.Text = "Select All";
			this.cmsColSelectrAll.Click += new System.EventHandler(this.cmsColSelectrAll_Click);
			// 
			// cmsColClearAll
			// 
			this.cmsColClearAll.Name = "cmsColClearAll";
			this.cmsColClearAll.Size = new System.Drawing.Size(122, 22);
			this.cmsColClearAll.Text = "Clear All";
			this.cmsColClearAll.Click += new System.EventHandler(this.cmsColClearAll_Click);
			// 
			// tabContent
			// 
			this.tabContent.Controls.Add(this.ugDetails);
			this.tabContent.Location = new System.Drawing.Point(4, 22);
			this.tabContent.Name = "tabContent";
			this.tabContent.Padding = new System.Windows.Forms.Padding(3);
			this.tabContent.Size = new System.Drawing.Size(948, 327);
			this.tabContent.TabIndex = 1;
			this.tabContent.Text = "Content";
			this.tabContent.UseVisualStyleBackColor = true;
			// 
			// ugDetails
			// 
			this.ugDetails.AllowDrop = true;
			this.ugDetails.ContextMenuStrip = this.cmsContentGrid;
			appearance7.BackColor = System.Drawing.SystemColors.Window;
			appearance7.BorderColor = System.Drawing.SystemColors.InactiveCaption;
			this.ugDetails.DisplayLayout.Appearance = appearance7;
			this.ugDetails.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
			this.ugDetails.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
			this.ugDetails.DisplayLayout.GroupByBox.Hidden = true;
			this.ugDetails.DisplayLayout.MaxColScrollRegions = 1;
			this.ugDetails.DisplayLayout.MaxRowScrollRegions = 1;
			this.ugDetails.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
			this.ugDetails.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
			appearance8.BackColor = System.Drawing.SystemColors.Window;
			this.ugDetails.DisplayLayout.Override.CardAreaAppearance = appearance8;
			appearance9.BorderColor = System.Drawing.Color.Silver;
			appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
			this.ugDetails.DisplayLayout.Override.CellAppearance = appearance9;
			this.ugDetails.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			this.ugDetails.DisplayLayout.Override.CellPadding = 0;
			appearance10.BackColor = System.Drawing.SystemColors.Control;
			appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark;
			appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			appearance10.BorderColor = System.Drawing.SystemColors.Window;
			this.ugDetails.DisplayLayout.Override.GroupByRowAppearance = appearance10;
			this.ugDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.ugDetails.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
			appearance11.BackColor = System.Drawing.SystemColors.Window;
			appearance11.BorderColor = System.Drawing.Color.Silver;
			this.ugDetails.DisplayLayout.Override.RowAppearance = appearance11;
			this.ugDetails.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
			appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
			this.ugDetails.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
			this.ugDetails.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.ugDetails.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.ugDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ugDetails.Location = new System.Drawing.Point(3, 3);
			this.ugDetails.Name = "ugDetails";
			this.ugDetails.Size = new System.Drawing.Size(942, 321);
			this.ugDetails.TabIndex = 0;
			this.ugDetails.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDetails_AfterCellUpdate);
			this.ugDetails.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugDetails_InitializeLayout);
			this.ugDetails.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugDetails_InitializeRow);
			this.ugDetails.AfterRowsDeleted += new System.EventHandler(this.ugDetails_AfterRowsDeleted);
			this.ugDetails.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugDetails_AfterRowInsert);
			this.ugDetails.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDetails_CellChange);
			this.ugDetails.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDetails_ClickCellButton);
			this.ugDetails.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ugDetails_AfterSelectChange);
			this.ugDetails.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.ugDetails_BeforeCellDeactivate);
			this.ugDetails.BeforeRowDeactivate += new System.ComponentModel.CancelEventHandler(this.ugDetails_BeforeRowDeactivate);
			this.ugDetails.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugDetails_BeforeCellUpdate);
			this.ugDetails.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugDetails_BeforeRowsDeleted);
			this.ugDetails.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.ugDetails_Error);
			this.ugDetails.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugDetails_AfterSortChange);
			this.ugDetails.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.ugDetails_BeforeColumnChooserDisplayed);
			this.ugDetails.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugDetails_MouseEnterElement);
			this.ugDetails.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugDetails_DragDrop);
			this.ugDetails.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugDetails_DragEnter);
			this.ugDetails.DragOver += new System.Windows.Forms.DragEventHandler(this.ugDetails_DragOver);
			this.ugDetails.DragLeave += new System.EventHandler(this.ugDetails_DragLeave);
			this.ugDetails.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ugDetails_KeyDown);
			this.ugDetails.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugDetails_MouseDown);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabContent);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 30);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(956, 353);
			this.tabControl.TabIndex = 5;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Selecting);
			// 
			// GroupAllocationView
			// 
			this.AllowDragDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(956, 383);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.rtbScrollText);
			this.Controls.Add(this.lblFindSize);
			this.Controls.Add(this.pnlTop);
			this.Controls.Add(this._AssortmentView_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._AssortmentView_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._AssortmentView_Toolbars_Dock_Area_Bottom);
			this.Controls.Add(this._AssortmentView_Toolbars_Dock_Area_Top);
			this.Name = "GroupAllocationView";
			this.Text = "Group Allocation";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.AssortmentView_Closing);
			this.Load += new System.EventHandler(this.AssortmentView_Load);
			this.Controls.SetChildIndex(this._AssortmentView_Toolbars_Dock_Area_Top, 0);
			this.Controls.SetChildIndex(this._AssortmentView_Toolbars_Dock_Area_Bottom, 0);
			this.Controls.SetChildIndex(this._AssortmentView_Toolbars_Dock_Area_Right, 0);
			this.Controls.SetChildIndex(this._AssortmentView_Toolbars_Dock_Area_Left, 0);
			this.Controls.SetChildIndex(this.pnlTop, 0);
			this.Controls.SetChildIndex(this.lblFindSize, 0);
			this.Controls.SetChildIndex(this.rtbScrollText, 0);
			this.Controls.SetChildIndex(this.tabControl, 0);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			this.pnlTop.ResumeLayout(false);
			this.pnlTop.PerformLayout();
			this.pnlTop_Fill_Panel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
			this.cmsGrid.ResumeLayout(false);
			this.cmsSplitter.ResumeLayout(false);
			this.cmsActions.ResumeLayout(false);
			this.cmsContentGrid.ResumeLayout(false);
			this.cmsContentInsert.ResumeLayout(false);
			this.cmsContentInsertColor.ResumeLayout(false);
			this.cmsContentColChooser.ResumeLayout(false);
			this.tabContent.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ugDetails)).EndInit();
			this.tabControl.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.ToolTip toolTip1;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AssortmentView_Toolbars_Dock_Area_Left;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AssortmentView_Toolbars_Dock_Area_Right;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AssortmentView_Toolbars_Dock_Area_Top;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AssortmentView_Toolbars_Dock_Area_Bottom;
		//private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager utmMain;
		private System.Windows.Forms.RichTextBox rtbScrollText;
		private System.Windows.Forms.Label lblFindSize;
		private System.Windows.Forms.ContextMenuStrip cmsSplitter;
		private System.Windows.Forms.ToolStripMenuItem cmiLockSplitter;
		private System.Windows.Forms.ContextMenuStrip cmsGrid;
		private System.Windows.Forms.ToolStripMenuItem cmiColChooser;
		private System.Windows.Forms.ToolStripSeparator cmiLockSeparator;
		private System.Windows.Forms.ToolStripMenuItem cmiLockColumn;
		private System.Windows.Forms.ToolStripMenuItem cmiUnlockColumn;
		private System.Windows.Forms.ToolStripSeparator cmiFreezeSeparator;
		private System.Windows.Forms.ToolStripMenuItem cmiFreezeColumn;
		private System.Windows.Forms.ToolStripMenuItem cmiLockCell;
		private System.Windows.Forms.ToolStripMenuItem cmiRowChooser;
		private System.Windows.Forms.ToolStripMenuItem cmiLockRow;
		private System.Windows.Forms.ToolStripMenuItem cmiUnlockRow;
		private System.Windows.Forms.ToolStripMenuItem cmiLockSheet;
		private System.Windows.Forms.ToolStripMenuItem cmiUnlockSheet;
		private System.Windows.Forms.ContextMenuStrip cmsActions;
		private System.Windows.Forms.ToolStripMenuItem cmiAssortmentActions;
		private System.Windows.Forms.ToolStripMenuItem cmiAllocationActions;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeLockCell;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeUnlockCell;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeLockColumn;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeUnlockColumn;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeLockRow;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeUnlockRow;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeLockSection;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeUnlockSection;
		private System.Windows.Forms.ToolStripMenuItem cmiCloseStyle;
		private System.Windows.Forms.ToolStripMenuItem cmiCloseColumn;
		private System.Windows.Forms.ToolStripMenuItem cmiOpenColumn;
		private System.Windows.Forms.ToolStripMenuItem cmiCloseRow;
		private System.Windows.Forms.ToolStripMenuItem cmiOpenRow;
        private System.Windows.Forms.ContextMenuStrip cmsContentGrid;
        private System.Windows.Forms.ToolStripMenuItem cmsInsert;
        private System.Windows.Forms.ToolStripMenuItem cmsRemove;
        private System.Windows.Forms.ContextMenuStrip cmsContentInsert;
        private System.Windows.Forms.ToolStripMenuItem cmsInsertPhStyle;
        private System.Windows.Forms.ToolStripMenuItem cmsInsertBulkColor;
        private System.Windows.Forms.ToolStripMenuItem cmsInsertPack;
        private System.Windows.Forms.ToolStripTextBox cmsInsertPhTextBox;
        private System.Windows.Forms.ContextMenuStrip cmsContentInsertColor;
        private System.Windows.Forms.ToolStripMenuItem cmsInsertMTColorRow;
        private System.Windows.Forms.ToolStripMenuItem cmsChooseColor;
        private System.Windows.Forms.ToolStripMenuItem cmsInsertPackColor;
        private System.Windows.Forms.ToolStripMenuItem cmsInsertPackSize;
		private System.Windows.Forms.ToolStripMenuItem cmsInsertBulkSize;
        private System.Windows.Forms.ContextMenuStrip cmsContentColChooser;
        private System.Windows.Forms.ToolStripMenuItem cmsColSelectrAll;
        private System.Windows.Forms.ToolStripMenuItem cmsColClearAll;
		private Infragistics.Win.Misc.UltraPanel pnlTop_Fill_Panel;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
		private System.Windows.Forms.TabPage tabContent;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugDetails;
		private System.Windows.Forms.TabControl tabControl;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _pnlTop_Toolbars_Dock_Area_Left;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _pnlTop_Toolbars_Dock_Area_Right;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _pnlTop_Toolbars_Dock_Area_Bottom;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _pnlTop_Toolbars_Dock_Area_Top;
		private MIDToolbarRadioButton midToolbarRadioButton1;
	}
}
