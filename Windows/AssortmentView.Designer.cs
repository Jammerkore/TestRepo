using MIDRetail.Windows.Controls;
using MIDRetail.Business;
namespace MIDRetail.Windows
{
	partial class AssortmentView
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
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                //this.cboFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboFilter_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
				//this.cboFilter.DropDown -= new System.EventHandler(this.cboFilter_DropDown);
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboFilter.DropDownClosed -= new System.EventHandler(cboFilter_DropDownClosed);
                // End TT#301-MD - JSmith - Controls are not functioning properly
				//this.cboView.SelectionChangeCommitted -= new System.EventHandler(this.cboView_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                //this.cboView.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboView_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboView.DropDownClosed -= new System.EventHandler(cboView_DropDownClosed);
                // End TT#301-MD - JSmith - Controls are not functioning properly
				//this.cboStoreGroupLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreGroupLevel_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                //this.cboStoreGroupLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreGroupLevel_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
				//this.cboStoreGroup.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreGroup_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                //this.cboStoreGroup.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreGroup_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboStoreGroupLevel.DropDownClosed -= new System.EventHandler(cboStoreGroupLevel_DropDownClosed);
                // End TT#301-MD - JSmith - Controls are not functioning properly
				this.g4.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
				this.g4.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g4.Resize -= new System.EventHandler(this.g4_Resize);
				this.g4.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g4_OwnerDrawCell);
				this.g4.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g4_BeforeScroll);
				this.cmiRowChooser.Click -= new System.EventHandler(this.cmiRowChooser_Click);
				this.cmiLockRow.Click -= new System.EventHandler(this.cmiLockRow_Click);
				this.cmiUnlockRow.Click -= new System.EventHandler(this.cmiUnlockRow_Click);
				this.g7.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
				this.g7.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g7.Resize -= new System.EventHandler(this.g7_Resize);
				this.g7.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g7_OwnerDrawCell);
				this.g7.VisibleChanged -= new System.EventHandler(this.g7_VisibleChanged);
				this.g7.Click -= new System.EventHandler(this.g7_Click);
				this.g7.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g7_BeforeScroll);
				this.spcHScrollLevel1.DoubleClick -= new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
				this.spcHScrollLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHScrollLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
				this.cmiLockSplitter.Click -= new System.EventHandler(this.cmiLockSplitter_Click);
				this.spcHScrollLevel2.DoubleClick -= new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
				this.spcHScrollLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHScrollLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
				this.vScrollBar2.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.vScrollBar2_Scroll);
				this.vScrollBar3.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.vScrollBar3_Scroll);
				this.g5.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g5.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g5.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g5.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g5_OwnerDrawCell);
				this.g5.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.g5.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g5_BeforeScroll);
				this.g5.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
                this.g5.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g5_AfterResizeColumn);
				this.cmiLockCell.Click -= new System.EventHandler(this.cmiLockCell_Click);
				this.g2.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.g2_MouseMove);
				this.g2.BeforeResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.BeforeResizeColumn);
				this.g2.DragEnter -= new System.Windows.Forms.DragEventHandler(this.g2_DragEnter);
				this.g2.DragOver -= new System.Windows.Forms.DragEventHandler(this.g2_DragOver);
				this.g2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g2.Resize -= new System.EventHandler(this.g2_Resize);
				this.g2.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_AfterResizeColumn);
				this.g2.QueryContinueDrag -= new System.Windows.Forms.QueryContinueDragEventHandler(this.g2_QueryContinueDrag);
				this.g2.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.g2_MouseUp);
				this.g1.VisibleChanged -= new System.EventHandler(this.g1_VisibleChanged);
				this.g2.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g2_BeforeScroll);
				this.g2.DragDrop -= new System.Windows.Forms.DragEventHandler(this.g2_DragDrop);
				this.g2.BeforeAutosizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_BeforeAutosizeColumn);
				//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
				this.g2.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g2.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				//End TT#1196 - JScott - Average units in the summary section should spread when changed
				this.cmsGrid.Opening -= new System.ComponentModel.CancelEventHandler(this.cmsGrid_Opening);
				this.cmiColChooser.Click -= new System.EventHandler(this.cmiColChooser_Click);
				this.cmiLockColumn.Click -= new System.EventHandler(this.cmiLockColumn_Click);
				this.cmiUnlockColumn.Click -= new System.EventHandler(this.cmiUnlockColumn_Click);
				this.cmiFreezeColumn.Click -= new System.EventHandler(this.cmiFreezeColumn_Click);
				this.g8.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g8.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g8.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g8.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g8_OwnerDrawCell);
				this.g8.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.g8.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g8_BeforeScroll);
				this.g8.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.hScrollBar2.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
				this.g6.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g6.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g6.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g6.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g6_OwnerDrawCell);
				this.g6.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.g6.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g6_BeforeScroll);
				this.g6.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
                this.g6.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g6_AfterResizeColumn);
				this.g3.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.g3_MouseMove);
				this.g3.BeforeResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.BeforeResizeColumn);
				this.g3.DragEnter -= new System.Windows.Forms.DragEventHandler(this.g3_DragEnter);
				this.g3.DragOver -= new System.Windows.Forms.DragEventHandler(this.g3_DragOver);
				this.g3.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g3.Resize -= new System.EventHandler(this.g3_Resize);
				this.g3.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g3_OwnerDrawCell);
				this.g3.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_AfterResizeColumn);
				this.g3.QueryContinueDrag -= new System.Windows.Forms.QueryContinueDragEventHandler(this.g3_QueryContinueDrag);
				this.g3.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.g3_MouseUp);
				this.g3.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g3_BeforeScroll);
				this.g3.DragDrop -= new System.Windows.Forms.DragEventHandler(this.g3_DragDrop);
				this.g3.BeforeAutosizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_BeforeAutosizeColumn);
				//Begin TT#1196 - JScott - Average units in the summary section should spread when changed
				this.g3.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g3.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				//End TT#1196 - JScott - Average units in the summary section should spread when changed
				this.g9.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g9.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g9.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g9.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g9_OwnerDrawCell);
				this.g9.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.g9.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g9_BeforeScroll);
				this.g9.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.hScrollBar3.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.hScrollBar3_Scroll);
				//this.utmMain.ToolClick -= new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.utmMain_ToolClick);
				this.spcHDetailLevel1.DoubleClick -= new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
				this.spcHDetailLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHDetailLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
				this.spcHDetailLevel2.DoubleClick -= new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
				this.spcHDetailLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHDetailLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
				this.spcHHeaderLevel1.DoubleClick -= new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
				this.spcHHeaderLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHHeaderLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
				this.spcHHeaderLevel2.DoubleClick -= new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
				this.spcHHeaderLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHHeaderLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
				this.cmiLockSheet.Click -= new System.EventHandler(this.cmiLockSheet_Click);
				this.cmiUnlockSheet.Click -= new System.EventHandler(this.cmiUnlockSheet_Click);
				this.spcHTotalLevel1.DoubleClick -= new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
				this.spcHTotalLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHTotalLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
				this.spcHTotalLevel2.DoubleClick -= new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
				this.spcHTotalLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHTotalLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
				this.spcVLevel1.DoubleClick -= new System.EventHandler(this.spcVLevel1_DoubleClick);
				this.spcVLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcVLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcVLevel1_SplitterMoved);
				this.spcVLevel2.DoubleClick -= new System.EventHandler(this.spcVLevel2_DoubleClick);
				this.spcVLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcVLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcVLevel2_SplitterMoved);
				this.Activated -= new System.EventHandler(this.AssortmentView_Activated);
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
                
                this.ugCharacteristics.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugCharacteristics_MouseDown);
                this.ugCharacteristics.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugCharacteristics_InitializeLayout);
                this.ugCharacteristics.DragOver -= new System.Windows.Forms.DragEventHandler(this.ugCharacteristics_DragOver);
                this.ugCharacteristics.DragLeave -= new System.EventHandler(this.ugCharacteristics_DragLeave);
                this.ugCharacteristics.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugCharacteristics_CellChange);
                this.ugCharacteristics.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugCharacteristics_MouseEnterElement);
                this.ugCharacteristics.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugCharacteristics_DragDrop);
                this.ugCharacteristics.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugCharacteristics_DragEnter);

				g2MouseUpRefireHandler -= new System.Windows.Forms.MouseEventHandler(this.g2_MouseUp);
				g3MouseUpRefireHandler -= new System.Windows.Forms.MouseEventHandler(this.g3_MouseUp);

                this.midComboBoxView.SelectionChangeCommitted -= new System.EventHandler(this.midComboBoxView_SelectionChangeCommitted);


				_sab.ProcessMethodOnAssortmentEvent.OnProcessMethodOnAssortmentEventHandler -= new ProcessMethodOnAssortmentEvent.ProcessMethodOnAssortmentEventHandler(OnProcessMethodOnAssortmentEvent);
				_sab.AssortmentSelectedHeaderEvent.OnAssortmentSelectedHeaderEventHandler -= new AssortmentSelectedHeaderEvent.AssortmentSelectedHeaderEventHandler(OnAssortmentSelectedHeaderEvent);

                this.midComboBoxSet.SelectedIndexChanged -= new System.EventHandler(this.midComboBoxSet_SelectedIndexChanged);

				if (_saveForm != null)
				{
					_saveForm.OnAssortmentSaveClosingEventHandler -= new AssortmentViewSave.AssortmentSaveClosingEventHandler(OnAssortmentSaveClosing);
				}

				if (_frmThemeProperties != null)
				{
					_frmThemeProperties.ApplyButtonClicked -= new System.EventHandler(StylePropertiesOnChanged);
				}

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
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool13 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAssortmentActions");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnProcessAssort");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool11 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAllocationActions");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnProcessAlloc");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnApply");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar2 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("UltraToolbar2");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool14 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cbxGroupBy");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool1 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerGroupBy");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool3 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAttribute");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool7 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAttributeSet");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar3 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("UltraToolbar3");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool9 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerView");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("popGrid");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnSaveView");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar4 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Process By Toolbar");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool5 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerTool1");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool2 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("TextBoxTool1");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnProcessAlloc");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssortmentView));
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("popGrid");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Expand All");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Collapse All");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnExport");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnEmail");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Expand All");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Collapse All");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnApply");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool6 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cbxAttribute");
            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool7 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cbxSet");
            Infragistics.Win.ValueList valueList2 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool8 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cboView");
            Infragistics.Win.ValueList valueList3 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool11 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cboAllocationAction");
            Infragistics.Win.ValueList valueList4 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool12 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cbxGroupBy");
            Infragistics.Win.ValueList valueList5 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnSaveView");
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnExport");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnEmail");
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool5 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cboAssortmentAction");
            Infragistics.Win.ValueList valueList6 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnProcessAssort");
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool2 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerGroupBy");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool6 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerTool1");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool4 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAttribute");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool8 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAttributeSet");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool10 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerView");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool12 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAllocationActions");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool14 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerAssortmentActions");
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.midComboBoxAssortmentActions = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.midComboBoxAllocationActions = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.midComboBoxView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.midComboBoxSet = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.midAttributeComboBox1 = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.midToolbarRadioButton2 = new MIDRetail.Windows.Controls.MIDToolbarRadioButton();
            this.midToolbarRadioButton1 = new MIDRetail.Windows.Controls.MIDToolbarRadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this._pnlTop_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._pnlTop_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._pnlTop_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._pnlTop_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.g4old = new C1.Win.C1FlexGrid.C1FlexGrid();
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
            this.g7 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.pnlScrollBars = new System.Windows.Forms.Panel();
            this.spcHScrollLevel1 = new System.Windows.Forms.SplitContainer();
            this.cmsSplitter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiLockSplitter = new System.Windows.Forms.ToolStripMenuItem();
            this.spcHScrollLevel2 = new System.Windows.Forms.SplitContainer();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.vScrollBar2 = new System.Windows.Forms.VScrollBar();
            this.vScrollBar3 = new System.Windows.Forms.VScrollBar();
            this.pnlSpacer = new System.Windows.Forms.Panel();
            this.rtbScrollText = new System.Windows.Forms.RichTextBox();
            this.g5 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.g2 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.g8 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.hScrollBar2 = new System.Windows.Forms.HScrollBar();
            this.g6 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.g3 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.g9 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.hScrollBar3 = new System.Windows.Forms.HScrollBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._AssortmentView_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._AssortmentView_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._AssortmentView_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._AssortmentView_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.lblFindSize = new System.Windows.Forms.Label();
            this.spcHDetailLevel1 = new System.Windows.Forms.SplitContainer();
            this.spcHDetailLevel2 = new System.Windows.Forms.SplitContainer();
            this.spcHHeaderLevel1 = new System.Windows.Forms.SplitContainer();
            this.spcHHeaderLevel2 = new System.Windows.Forms.SplitContainer();
            this.g1 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.g4 = new MIDRetail.Windows.Controls.MIDFlexGrid();
            this.spcHTotalLevel1 = new System.Windows.Forms.SplitContainer();
            this.spcHTotalLevel2 = new System.Windows.Forms.SplitContainer();
            this.spcVLevel1 = new System.Windows.Forms.SplitContainer();
            this.spcVLevel2 = new System.Windows.Forms.SplitContainer();
            this.cmsActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiAssortmentActions = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiAllocationActions = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabAssortment = new System.Windows.Forms.TabPage();
            this.tabContent = new System.Windows.Forms.TabPage();
            this.ugDetails = new Infragistics.Win.UltraWinGrid.UltraGrid();
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
            this.tabProductChar = new System.Windows.Forms.TabPage();
            this.ugCharacteristics = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cmsCharGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsApplyToLowerLevels = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsInheritFromHigherLevel = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsContentColChooser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsColSelectrAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsColClearAll = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g4old)).BeginInit();
            this.cmsGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g7)).BeginInit();
            this.pnlScrollBars.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcHScrollLevel1)).BeginInit();
            this.spcHScrollLevel1.Panel1.SuspendLayout();
            this.spcHScrollLevel1.Panel2.SuspendLayout();
            this.spcHScrollLevel1.SuspendLayout();
            this.cmsSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcHScrollLevel2)).BeginInit();
            this.spcHScrollLevel2.Panel1.SuspendLayout();
            this.spcHScrollLevel2.Panel2.SuspendLayout();
            this.spcHScrollLevel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spcHDetailLevel1)).BeginInit();
            this.spcHDetailLevel1.Panel1.SuspendLayout();
            this.spcHDetailLevel1.Panel2.SuspendLayout();
            this.spcHDetailLevel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcHDetailLevel2)).BeginInit();
            this.spcHDetailLevel2.Panel1.SuspendLayout();
            this.spcHDetailLevel2.Panel2.SuspendLayout();
            this.spcHDetailLevel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcHHeaderLevel1)).BeginInit();
            this.spcHHeaderLevel1.Panel1.SuspendLayout();
            this.spcHHeaderLevel1.Panel2.SuspendLayout();
            this.spcHHeaderLevel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcHHeaderLevel2)).BeginInit();
            this.spcHHeaderLevel2.Panel1.SuspendLayout();
            this.spcHHeaderLevel2.Panel2.SuspendLayout();
            this.spcHHeaderLevel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spcHTotalLevel1)).BeginInit();
            this.spcHTotalLevel1.Panel1.SuspendLayout();
            this.spcHTotalLevel1.Panel2.SuspendLayout();
            this.spcHTotalLevel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcHTotalLevel2)).BeginInit();
            this.spcHTotalLevel2.Panel1.SuspendLayout();
            this.spcHTotalLevel2.Panel2.SuspendLayout();
            this.spcHTotalLevel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcVLevel1)).BeginInit();
            this.spcVLevel1.Panel1.SuspendLayout();
            this.spcVLevel1.Panel2.SuspendLayout();
            this.spcVLevel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcVLevel2)).BeginInit();
            this.spcVLevel2.Panel1.SuspendLayout();
            this.spcVLevel2.Panel2.SuspendLayout();
            this.spcVLevel2.SuspendLayout();
            this.cmsActions.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabAssortment.SuspendLayout();
            this.tabContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugDetails)).BeginInit();
            this.cmsContentGrid.SuspendLayout();
            this.cmsContentInsert.SuspendLayout();
            this.cmsContentInsertColor.SuspendLayout();
            this.tabProductChar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugCharacteristics)).BeginInit();
            this.cmsCharGrid.SuspendLayout();
            this.cmsContentColChooser.SuspendLayout();
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
            this.pnlTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlTop.Controls.Add(this.midComboBoxAssortmentActions);
            this.pnlTop.Controls.Add(this.midComboBoxAllocationActions);
            this.pnlTop.Controls.Add(this.midComboBoxView);
            this.pnlTop.Controls.Add(this.midComboBoxSet);
            this.pnlTop.Controls.Add(this.midAttributeComboBox1);
            this.pnlTop.Controls.Add(this.midToolbarRadioButton2);
            this.pnlTop.Controls.Add(this.midToolbarRadioButton1);
            this.pnlTop.Controls.Add(this.panel1);
            this.pnlTop.Controls.Add(this._pnlTop_Toolbars_Dock_Area_Left);
            this.pnlTop.Controls.Add(this._pnlTop_Toolbars_Dock_Area_Right);
            this.pnlTop.Controls.Add(this._pnlTop_Toolbars_Dock_Area_Bottom);
            this.pnlTop.Controls.Add(this._pnlTop_Toolbars_Dock_Area_Top);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1131, 72);
            this.pnlTop.TabIndex = 0;
            this.pnlTop.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTop_Paint);
            // 
            // midComboBoxAssortmentActions
            // 
            this.midComboBoxAssortmentActions.AutoAdjust = true;
            this.midComboBoxAssortmentActions.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.midComboBoxAssortmentActions.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.midComboBoxAssortmentActions.DataSource = null;
            this.midComboBoxAssortmentActions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.midComboBoxAssortmentActions.DropDownWidth = 122;
            this.midComboBoxAssortmentActions.FormattingEnabled = false;
            this.midComboBoxAssortmentActions.IgnoreFocusLost = false;
            this.midComboBoxAssortmentActions.ItemHeight = 13;
            this.midComboBoxAssortmentActions.Location = new System.Drawing.Point(702, 0);
            this.midComboBoxAssortmentActions.Margin = new System.Windows.Forms.Padding(0);
            this.midComboBoxAssortmentActions.MaxDropDownItems = 25;
            this.midComboBoxAssortmentActions.Name = "midComboBoxAssortmentActions";
            this.midComboBoxAssortmentActions.SetToolTip = "";
            this.midComboBoxAssortmentActions.Size = new System.Drawing.Size(175, 23);
            this.midComboBoxAssortmentActions.TabIndex = 48;
            this.midComboBoxAssortmentActions.Tag = null;
            this.midComboBoxAssortmentActions.Visible = false;
            // 
            // midComboBoxAllocationActions
            // 
            this.midComboBoxAllocationActions.AutoAdjust = true;
            this.midComboBoxAllocationActions.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.midComboBoxAllocationActions.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.midComboBoxAllocationActions.DataSource = null;
            this.midComboBoxAllocationActions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.midComboBoxAllocationActions.DropDownWidth = 122;
            this.midComboBoxAllocationActions.FormattingEnabled = false;
            this.midComboBoxAllocationActions.IgnoreFocusLost = false;
            this.midComboBoxAllocationActions.ItemHeight = 13;
            this.midComboBoxAllocationActions.Location = new System.Drawing.Point(815, 4);
            this.midComboBoxAllocationActions.Margin = new System.Windows.Forms.Padding(0);
            this.midComboBoxAllocationActions.MaxDropDownItems = 25;
            this.midComboBoxAllocationActions.Name = "midComboBoxAllocationActions";
            this.midComboBoxAllocationActions.SetToolTip = "";
            this.midComboBoxAllocationActions.Size = new System.Drawing.Size(175, 23);
            this.midComboBoxAllocationActions.TabIndex = 47;
            this.midComboBoxAllocationActions.Tag = null;
            this.midComboBoxAllocationActions.Visible = false;
            // 
            // midComboBoxView
            // 
            this.midComboBoxView.AutoAdjust = true;
            this.midComboBoxView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.midComboBoxView.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.midComboBoxView.DataSource = null;
            this.midComboBoxView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.midComboBoxView.DropDownWidth = 114;
            this.midComboBoxView.FormattingEnabled = false;
            this.midComboBoxView.IgnoreFocusLost = false;
            this.midComboBoxView.ItemHeight = 13;
            this.midComboBoxView.Location = new System.Drawing.Point(925, 0);
            this.midComboBoxView.Margin = new System.Windows.Forms.Padding(0);
            this.midComboBoxView.MaxDropDownItems = 25;
            this.midComboBoxView.Name = "midComboBoxView";
            this.midComboBoxView.SetToolTip = "";
            this.midComboBoxView.Size = new System.Drawing.Size(175, 23);
            this.midComboBoxView.TabIndex = 42;
            this.midComboBoxView.Tag = null;
            this.midComboBoxView.Visible = false;
            this.midComboBoxView.SelectedIndexChanged += new System.EventHandler(this.midComboBoxView_SelectedIndexChanged);
            this.midComboBoxView.SelectionChangeCommitted += new System.EventHandler(this.midComboBoxView_SelectionChangeCommitted);
            this.midComboBoxView.DropDownClosed += new System.EventHandler(this.cboView_DropDownClosed);
            // 
            // midComboBoxSet
            // 
            this.midComboBoxSet.AutoAdjust = true;
            this.midComboBoxSet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.midComboBoxSet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.midComboBoxSet.DataSource = null;
            this.midComboBoxSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.midComboBoxSet.DropDownWidth = 121;
            this.midComboBoxSet.FormattingEnabled = false;
            this.midComboBoxSet.IgnoreFocusLost = false;
            this.midComboBoxSet.ItemHeight = 13;
            this.midComboBoxSet.Location = new System.Drawing.Point(986, 9);
            this.midComboBoxSet.Margin = new System.Windows.Forms.Padding(0);
            this.midComboBoxSet.MaxDropDownItems = 25;
            this.midComboBoxSet.Name = "midComboBoxSet";
            this.midComboBoxSet.SetToolTip = "";
            this.midComboBoxSet.Size = new System.Drawing.Size(121, 23);
            this.midComboBoxSet.TabIndex = 37;
            this.midComboBoxSet.Tag = null;
            this.midComboBoxSet.Visible = false;
            this.midComboBoxSet.SelectedIndexChanged += new System.EventHandler(this.midComboBoxSet_SelectedIndexChanged);
            this.midComboBoxSet.SelectionChangeCommitted += new System.EventHandler(this.midComboBoxSet_SelectionChangeCommitted);
            // 
            // midAttributeComboBox1
            // 
            this.midAttributeComboBox1.AllowDrop = true;
            this.midAttributeComboBox1.AllowUserAttributes = false;
            this.midAttributeComboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.midAttributeComboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.midAttributeComboBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.midAttributeComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.midAttributeComboBox1.FormattingEnabled = true;
            this.midAttributeComboBox1.Location = new System.Drawing.Point(986, 33);
            this.midAttributeComboBox1.Name = "midAttributeComboBox1";
            this.midAttributeComboBox1.Size = new System.Drawing.Size(121, 21);
            this.midAttributeComboBox1.TabIndex = 32;
            this.midAttributeComboBox1.Visible = false;
            this.midAttributeComboBox1.SelectedIndexChanged += new System.EventHandler(this.midAttributeComboBox1_SelectedIndexChanged);
            this.midAttributeComboBox1.SelectionChangeCommitted += new System.EventHandler(this.midAttributeComboBox1_SelectionChangeCommitted);
            // 
            // midToolbarRadioButton2
            // 
            this.midToolbarRadioButton2.AutoSize = true;
            this.midToolbarRadioButton2.BackColor = System.Drawing.SystemColors.Control;
            this.midToolbarRadioButton2.Location = new System.Drawing.Point(790, 34);
            this.midToolbarRadioButton2.Name = "midToolbarRadioButton2";
            this.midToolbarRadioButton2.RadioButton1Text = "radioButton1";
            this.midToolbarRadioButton2.RadioButton2Text = "radioButton2";
            this.midToolbarRadioButton2.Size = new System.Drawing.Size(177, 22);
            this.midToolbarRadioButton2.TabIndex = 27;
            // 
            // midToolbarRadioButton1
            // 
            this.midToolbarRadioButton1.AutoSize = true;
            this.midToolbarRadioButton1.BackColor = System.Drawing.SystemColors.Control;
            this.midToolbarRadioButton1.Location = new System.Drawing.Point(-10000, -10000);
            this.midToolbarRadioButton1.Name = "midToolbarRadioButton1";
            this.midToolbarRadioButton1.RadioButton1Text = "radioButton1";
            this.midToolbarRadioButton1.RadioButton2Text = "radioButton2";
            this.midToolbarRadioButton1.Size = new System.Drawing.Size(185, 22);
            this.midToolbarRadioButton1.TabIndex = 16;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(955, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(25, 24);
            this.panel1.TabIndex = 14;
            // 
            // _pnlTop_Toolbars_Dock_Area_Left
            // 
            this._pnlTop_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._pnlTop_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._pnlTop_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._pnlTop_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._pnlTop_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 72);
            this._pnlTop_Toolbars_Dock_Area_Left.Name = "_pnlTop_Toolbars_Dock_Area_Left";
            this._pnlTop_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 0);
            this._pnlTop_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this.pnlTop;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 1;
            ultraToolbar1.FloatingLocation = new System.Drawing.Point(556, 309);
            ultraToolbar1.FloatingSize = new System.Drawing.Size(255, 48);
            controlContainerTool13.ControlName = "midComboBoxAssortmentActions";
            controlContainerTool13.InstanceProps.Width = 288;
            controlContainerTool11.ControlName = "midComboBoxAllocationActions";
            controlContainerTool11.InstanceProps.Width = 280;
            buttonTool7.InstanceProps.IsFirstInGroup = true;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool13,
            buttonTool15,
            controlContainerTool11,
            buttonTool1,
            buttonTool7});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Text = "Action Toolbar";
            ultraToolbar2.DockedColumn = 0;
            ultraToolbar2.DockedRow = 0;
            ultraToolbar2.FloatingLocation = new System.Drawing.Point(21, 172);
            ultraToolbar2.FloatingSize = new System.Drawing.Size(314, 70);
            controlContainerTool1.ControlName = "midToolbarRadioButton1";
            controlContainerTool1.InstanceProps.Width = 248;
            controlContainerTool3.ControlName = "midAttributeComboBox1";
            controlContainerTool7.ControlName = "midComboBoxSet";
            controlContainerTool7.InstanceProps.Width = 150;
            ultraToolbar2.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            comboBoxTool14,
            controlContainerTool1,
            controlContainerTool3,
            controlContainerTool7});
            ultraToolbar2.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Text = "Group By Toolbar";
            ultraToolbar3.DockedColumn = 0;
            ultraToolbar3.DockedRow = 1;
            controlContainerTool9.ControlName = "midComboBoxView";
            controlContainerTool9.InstanceProps.Width = 213;
            ultraToolbar3.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool9,
            popupMenuTool3,
            buttonTool9});
            ultraToolbar3.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar3.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar3.Text = "View Toolbar";
            ultraToolbar3.Visible = false;
            ultraToolbar4.DockedColumn = 0;
            ultraToolbar4.DockedRow = 2;
            ultraToolbar4.FloatingLocation = new System.Drawing.Point(41, 194);
            ultraToolbar4.FloatingSize = new System.Drawing.Size(354, 24);
            controlContainerTool5.ControlName = "midToolbarRadioButton2";
            ultraToolbar4.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool5});
            ultraToolbar4.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar4.Text = "Process By Toolbar";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1,
            ultraToolbar2,
            ultraToolbar3,
            ultraToolbar4});
            textBoxTool2.SharedPropsInternal.Caption = "Group Allocation:";
            textBoxTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            textBoxTool2.SharedPropsInternal.Width = 300;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            buttonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance1;
            buttonTool2.SharedPropsInternal.Caption = "Process";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            popupMenuTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance2;
            popupMenuTool2.SharedPropsInternal.Caption = "Grid";
            popupMenuTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool11.InstanceProps.IsFirstInGroup = true;
            popupMenuTool2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool3,
            buttonTool4,
            buttonTool11,
            buttonTool12});
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            buttonTool5.SharedPropsInternal.AppearancesSmall.Appearance = appearance3;
            buttonTool5.SharedPropsInternal.Caption = "Expand All";
            buttonTool5.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            buttonTool6.SharedPropsInternal.AppearancesSmall.Appearance = appearance4;
            buttonTool6.SharedPropsInternal.Caption = "Collapse All";
            buttonTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            buttonTool8.SharedPropsInternal.AppearancesSmall.Appearance = appearance5;
            buttonTool8.SharedPropsInternal.Caption = "Apply";
            buttonTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            comboBoxTool6.SharedPropsInternal.Caption = "Attribute:";
            comboBoxTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            comboBoxTool6.ValueList = valueList1;
            comboBoxTool7.SharedPropsInternal.Caption = "Set:";
            comboBoxTool7.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            comboBoxTool7.ValueList = valueList2;
            comboBoxTool8.SharedPropsInternal.Caption = "View:";
            comboBoxTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            comboBoxTool8.ValueList = valueList3;
            comboBoxTool11.SharedPropsInternal.Caption = "Al ac";
            comboBoxTool11.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            valueList4.MaxDropDownItems = 20;
            comboBoxTool11.ValueList = valueList4;
            comboBoxTool12.SharedPropsInternal.Caption = "Group By:";
            comboBoxTool12.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            comboBoxTool12.SharedPropsInternal.Visible = false;
            comboBoxTool12.ValueList = valueList5;
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            buttonTool10.SharedPropsInternal.AppearancesSmall.Appearance = appearance6;
            buttonTool10.SharedPropsInternal.Caption = "Save View";
            buttonTool10.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            buttonTool13.SharedPropsInternal.AppearancesSmall.Appearance = appearance7;
            buttonTool13.SharedPropsInternal.Caption = "Export";
            buttonTool13.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            buttonTool14.SharedPropsInternal.AppearancesSmall.Appearance = appearance8;
            buttonTool14.SharedPropsInternal.Caption = "Email";
            buttonTool14.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            comboBoxTool5.SharedPropsInternal.Caption = "As Ac";
            comboBoxTool5.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            valueList6.MaxDropDownItems = 20;
            comboBoxTool5.ValueList = valueList6;
            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
            buttonTool16.SharedPropsInternal.AppearancesSmall.Appearance = appearance9;
            buttonTool16.SharedPropsInternal.Caption = "Process";
            buttonTool16.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool2.ControlName = "midToolbarRadioButton1";
            controlContainerTool2.SharedPropsInternal.Caption = "Group By:";
            controlContainerTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool2.SharedPropsInternal.Width = 248;
            controlContainerTool6.ControlName = "midToolbarRadioButton2";
            controlContainerTool6.SharedPropsInternal.Caption = "Process As:";
            controlContainerTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool4.ControlName = "midAttributeComboBox1";
            controlContainerTool4.SharedPropsInternal.Caption = "Attribute:";
            controlContainerTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool8.ControlName = "midComboBoxSet";
            controlContainerTool8.SharedPropsInternal.Caption = "Set:";
            controlContainerTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool8.SharedPropsInternal.Width = 150;
            controlContainerTool10.ControlName = "midComboBoxView";
            appearance10.Image = ((object)(resources.GetObject("appearance10.Image")));
            controlContainerTool10.SharedPropsInternal.AppearancesSmall.Appearance = appearance10;
            controlContainerTool10.SharedPropsInternal.Caption = "View:";
            controlContainerTool10.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool10.SharedPropsInternal.Width = 213;
            controlContainerTool12.ControlName = "midComboBoxAllocationActions";
            controlContainerTool12.SharedPropsInternal.Caption = "Allocation Action:";
            controlContainerTool12.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool12.SharedPropsInternal.Width = 280;
            controlContainerTool14.ControlName = "midComboBoxAssortmentActions";
            controlContainerTool14.SharedPropsInternal.Caption = "Assortment Action:";
            controlContainerTool14.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool14.SharedPropsInternal.Width = 288;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            textBoxTool2,
            buttonTool2,
            popupMenuTool2,
            buttonTool5,
            buttonTool6,
            buttonTool8,
            comboBoxTool6,
            comboBoxTool7,
            comboBoxTool8,
            comboBoxTool11,
            comboBoxTool12,
            buttonTool10,
            buttonTool13,
            buttonTool14,
            comboBoxTool5,
            buttonTool16,
            controlContainerTool2,
            controlContainerTool6,
            controlContainerTool4,
            controlContainerTool8,
            controlContainerTool10,
            controlContainerTool12,
            controlContainerTool14});
            this.ultraToolbarsManager1.AfterToolCloseup += new Infragistics.Win.UltraWinToolbars.ToolDropdownEventHandler(this.ultraToolbarsManager1_AfterToolCloseup);
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            this.ultraToolbarsManager1.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(this.ultraToolbarsManager1_ToolValueChanged);
            // 
            // _pnlTop_Toolbars_Dock_Area_Right
            // 
            this._pnlTop_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._pnlTop_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._pnlTop_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._pnlTop_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._pnlTop_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1131, 72);
            this._pnlTop_Toolbars_Dock_Area_Right.Name = "_pnlTop_Toolbars_Dock_Area_Right";
            this._pnlTop_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 0);
            this._pnlTop_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _pnlTop_Toolbars_Dock_Area_Bottom
            // 
            this._pnlTop_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._pnlTop_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._pnlTop_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._pnlTop_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._pnlTop_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 72);
            this._pnlTop_Toolbars_Dock_Area_Bottom.Name = "_pnlTop_Toolbars_Dock_Area_Bottom";
            this._pnlTop_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1131, 0);
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
            this._pnlTop_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1131, 72);
            this._pnlTop_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // g4old
            // 
            this.g4old.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None;
            this.g4old.AllowEditing = false;
            this.g4old.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g4old.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g4old.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g4old.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g4old.ContextMenuStrip = this.cmsGrid;
            this.g4old.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g4old.ExtendLastCol = true;
            this.g4old.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g4old.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g4old.Location = new System.Drawing.Point(0, 0);
            this.g4old.Name = "g4old";
            this.g4old.Rows.DefaultSize = 17;
            this.g4old.Rows.Fixed = 0;
            this.g4old.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g4old.Size = new System.Drawing.Size(83, 75);
            this.g4old.TabIndex = 1;
            this.g4old.Visible = false;
            this.g4old.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g4_BeforeScroll);
            this.g4old.AfterCollapse += new C1.Win.C1FlexGrid.RowColEventHandler(this.g4_AfterCollapse);
            this.g4old.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g4_OwnerDrawCell);
            this.g4old.DragDrop += new System.Windows.Forms.DragEventHandler(this.g4_DragDrop);
            this.g4old.DragEnter += new System.Windows.Forms.DragEventHandler(this.g4_DragEnter);
            this.g4old.DragOver += new System.Windows.Forms.DragEventHandler(this.g4_DragOver);
            this.g4old.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.g4_QueryContinueDrag);
            this.g4old.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            this.g4old.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
            this.g4old.Resize += new System.EventHandler(this.g4_Resize);
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
            this.cmiColChooser.Click += new System.EventHandler(this.cmiColChooser_Click);
            // 
            // cmiRowChooser
            // 
            this.cmiRowChooser.Name = "cmiRowChooser";
            this.cmiRowChooser.Size = new System.Drawing.Size(204, 22);
            this.cmiRowChooser.Text = "Row Chooser...";
            this.cmiRowChooser.Click += new System.EventHandler(this.cmiRowChooser_Click);
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
            this.cmiLockCell.Click += new System.EventHandler(this.cmiLockCell_Click);
            // 
            // cmiLockColumn
            // 
            this.cmiLockColumn.Name = "cmiLockColumn";
            this.cmiLockColumn.Size = new System.Drawing.Size(204, 22);
            this.cmiLockColumn.Text = "Lock Column";
            this.cmiLockColumn.Click += new System.EventHandler(this.cmiLockColumn_Click);
            // 
            // cmiUnlockColumn
            // 
            this.cmiUnlockColumn.Name = "cmiUnlockColumn";
            this.cmiUnlockColumn.Size = new System.Drawing.Size(204, 22);
            this.cmiUnlockColumn.Text = "Unlock Column";
            this.cmiUnlockColumn.Click += new System.EventHandler(this.cmiUnlockColumn_Click);
            // 
            // cmiLockRow
            // 
            this.cmiLockRow.Name = "cmiLockRow";
            this.cmiLockRow.Size = new System.Drawing.Size(204, 22);
            this.cmiLockRow.Text = "Lock Row";
            this.cmiLockRow.Click += new System.EventHandler(this.cmiLockRow_Click);
            // 
            // cmiUnlockRow
            // 
            this.cmiUnlockRow.Name = "cmiUnlockRow";
            this.cmiUnlockRow.Size = new System.Drawing.Size(204, 22);
            this.cmiUnlockRow.Text = "Unlock Row";
            this.cmiUnlockRow.Click += new System.EventHandler(this.cmiUnlockRow_Click);
            // 
            // cmiLockSheet
            // 
            this.cmiLockSheet.Name = "cmiLockSheet";
            this.cmiLockSheet.Size = new System.Drawing.Size(204, 22);
            this.cmiLockSheet.Text = "Lock Sheet";
            this.cmiLockSheet.Click += new System.EventHandler(this.cmiLockSheet_Click);
            // 
            // cmiUnlockSheet
            // 
            this.cmiUnlockSheet.Name = "cmiUnlockSheet";
            this.cmiUnlockSheet.Size = new System.Drawing.Size(204, 22);
            this.cmiUnlockSheet.Text = "Unlock Sheet";
            this.cmiUnlockSheet.Click += new System.EventHandler(this.cmiUnlockSheet_Click);
            // 
            // cmiCascadeLockCell
            // 
            this.cmiCascadeLockCell.Name = "cmiCascadeLockCell";
            this.cmiCascadeLockCell.Size = new System.Drawing.Size(204, 22);
            this.cmiCascadeLockCell.Text = "Cascade Lock Cell";
            this.cmiCascadeLockCell.Click += new System.EventHandler(this.cmiCascadeLockCell_Click);
            // 
            // cmiCascadeUnlockCell
            // 
            this.cmiCascadeUnlockCell.Name = "cmiCascadeUnlockCell";
            this.cmiCascadeUnlockCell.Size = new System.Drawing.Size(204, 22);
            this.cmiCascadeUnlockCell.Text = "Cascade Unlock Cell";
            this.cmiCascadeUnlockCell.Click += new System.EventHandler(this.cmiCascadeUnlockCell_Click);
            // 
            // cmiCascadeLockColumn
            // 
            this.cmiCascadeLockColumn.Name = "cmiCascadeLockColumn";
            this.cmiCascadeLockColumn.Size = new System.Drawing.Size(204, 22);
            this.cmiCascadeLockColumn.Text = "Cascade Lock Column";
            this.cmiCascadeLockColumn.Click += new System.EventHandler(this.cmiCascadeLockColumn_Click);
            // 
            // cmiCascadeUnlockColumn
            // 
            this.cmiCascadeUnlockColumn.Name = "cmiCascadeUnlockColumn";
            this.cmiCascadeUnlockColumn.Size = new System.Drawing.Size(204, 22);
            this.cmiCascadeUnlockColumn.Text = "Cascade Unlock Column";
            this.cmiCascadeUnlockColumn.Click += new System.EventHandler(this.cmiCascadeUnlockColumn_Click);
            // 
            // cmiCascadeLockRow
            // 
            this.cmiCascadeLockRow.Name = "cmiCascadeLockRow";
            this.cmiCascadeLockRow.Size = new System.Drawing.Size(204, 22);
            this.cmiCascadeLockRow.Text = "Cascade Lock Row";
            this.cmiCascadeLockRow.Click += new System.EventHandler(this.cmiCascadeLockRow_Click);
            // 
            // cmiCascadeUnlockRow
            // 
            this.cmiCascadeUnlockRow.Name = "cmiCascadeUnlockRow";
            this.cmiCascadeUnlockRow.Size = new System.Drawing.Size(204, 22);
            this.cmiCascadeUnlockRow.Text = "Cascade Unlock Row";
            this.cmiCascadeUnlockRow.Click += new System.EventHandler(this.cmiCascadeUnlockRow_Click);
            // 
            // cmiCascadeLockSection
            // 
            this.cmiCascadeLockSection.Name = "cmiCascadeLockSection";
            this.cmiCascadeLockSection.Size = new System.Drawing.Size(204, 22);
            this.cmiCascadeLockSection.Text = "Cascade Lock Section";
            this.cmiCascadeLockSection.Click += new System.EventHandler(this.cmiCascadeLockSection_Click);
            // 
            // cmiCascadeUnlockSection
            // 
            this.cmiCascadeUnlockSection.Name = "cmiCascadeUnlockSection";
            this.cmiCascadeUnlockSection.Size = new System.Drawing.Size(204, 22);
            this.cmiCascadeUnlockSection.Text = "Cascade Unlock Section";
            this.cmiCascadeUnlockSection.Click += new System.EventHandler(this.cmiCascadeUnlockSection_Click);
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
            this.cmiFreezeColumn.Click += new System.EventHandler(this.cmiFreezeColumn_Click);
            // 
            // cmiCloseStyle
            // 
            this.cmiCloseStyle.Name = "cmiCloseStyle";
            this.cmiCloseStyle.Size = new System.Drawing.Size(204, 22);
            this.cmiCloseStyle.Text = "Close Style";
            this.cmiCloseStyle.Click += new System.EventHandler(this.cmiCloseStyle_Click);
            // 
            // cmiCloseColumn
            // 
            this.cmiCloseColumn.Name = "cmiCloseColumn";
            this.cmiCloseColumn.Size = new System.Drawing.Size(204, 22);
            this.cmiCloseColumn.Text = "Close Column";
            this.cmiCloseColumn.Click += new System.EventHandler(this.cmiCloseColumn_Click);
            // 
            // cmiOpenColumn
            // 
            this.cmiOpenColumn.Name = "cmiOpenColumn";
            this.cmiOpenColumn.Size = new System.Drawing.Size(204, 22);
            this.cmiOpenColumn.Text = "Open Column";
            this.cmiOpenColumn.Click += new System.EventHandler(this.cmiOpenColumn_Click);
            // 
            // cmiCloseRow
            // 
            this.cmiCloseRow.Name = "cmiCloseRow";
            this.cmiCloseRow.Size = new System.Drawing.Size(204, 22);
            this.cmiCloseRow.Text = "Close Row";
            this.cmiCloseRow.Click += new System.EventHandler(this.cmiCloseRow_Click);
            // 
            // cmiOpenRow
            // 
            this.cmiOpenRow.Name = "cmiOpenRow";
            this.cmiOpenRow.Size = new System.Drawing.Size(204, 22);
            this.cmiOpenRow.Text = "Open Row";
            this.cmiOpenRow.Click += new System.EventHandler(this.cmiOpenRow_Click);
            // 
            // g7
            // 
            this.g7.AllowEditing = false;
            this.g7.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g7.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g7.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g7.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g7.ContextMenuStrip = this.cmsGrid;
            this.g7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g7.ExtendLastCol = true;
            this.g7.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g7.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g7.Location = new System.Drawing.Point(0, 0);
            this.g7.Name = "g7";
            this.g7.Rows.DefaultSize = 17;
            this.g7.Rows.Fixed = 0;
            this.g7.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g7.Size = new System.Drawing.Size(83, 17);
            this.g7.TabIndex = 2;
            this.g7.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g7_BeforeScroll);
            this.g7.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g7_OwnerDrawCell);
            this.g7.VisibleChanged += new System.EventHandler(this.g7_VisibleChanged);
            this.g7.Click += new System.EventHandler(this.g7_Click);
            this.g7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            this.g7.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
            this.g7.Resize += new System.EventHandler(this.g7_Resize);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.Enabled = false;
            this.hScrollBar1.LargeChange = 1;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 17);
            this.hScrollBar1.Maximum = 0;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(83, 17);
            this.hScrollBar1.TabIndex = 4;
            // 
            // pnlScrollBars
            // 
            this.pnlScrollBars.Controls.Add(this.spcHScrollLevel1);
            this.pnlScrollBars.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlScrollBars.Location = new System.Drawing.Point(1103, 3);
            this.pnlScrollBars.Name = "pnlScrollBars";
            this.pnlScrollBars.Size = new System.Drawing.Size(17, 335);
            this.pnlScrollBars.TabIndex = 3;
            // 
            // spcHScrollLevel1
            // 
            this.spcHScrollLevel1.ContextMenuStrip = this.cmsSplitter;
            this.spcHScrollLevel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcHScrollLevel1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spcHScrollLevel1.Location = new System.Drawing.Point(0, 0);
            this.spcHScrollLevel1.Margin = new System.Windows.Forms.Padding(0);
            this.spcHScrollLevel1.Name = "spcHScrollLevel1";
            this.spcHScrollLevel1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcHScrollLevel1.Panel1
            // 
            this.spcHScrollLevel1.Panel1.Controls.Add(this.spcHScrollLevel2);
            this.spcHScrollLevel1.Panel1MinSize = 0;
            // 
            // spcHScrollLevel1.Panel2
            // 
            this.spcHScrollLevel1.Panel2.Controls.Add(this.vScrollBar3);
            this.spcHScrollLevel1.Panel2.Controls.Add(this.pnlSpacer);
            this.spcHScrollLevel1.Panel2MinSize = 0;
            this.spcHScrollLevel1.Size = new System.Drawing.Size(17, 335);
            this.spcHScrollLevel1.SplitterDistance = 295;
            this.spcHScrollLevel1.TabIndex = 5;
            this.spcHScrollLevel1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
            this.spcHScrollLevel1.DoubleClick += new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
            this.spcHScrollLevel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
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
            this.cmiLockSplitter.Click += new System.EventHandler(this.cmiLockSplitter_Click);
            // 
            // spcHScrollLevel2
            // 
            this.spcHScrollLevel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcHScrollLevel2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spcHScrollLevel2.Location = new System.Drawing.Point(0, 0);
            this.spcHScrollLevel2.Margin = new System.Windows.Forms.Padding(0);
            this.spcHScrollLevel2.Name = "spcHScrollLevel2";
            this.spcHScrollLevel2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcHScrollLevel2.Panel1
            // 
            this.spcHScrollLevel2.Panel1.Controls.Add(this.vScrollBar1);
            this.spcHScrollLevel2.Panel1MinSize = 0;
            // 
            // spcHScrollLevel2.Panel2
            // 
            this.spcHScrollLevel2.Panel2.Controls.Add(this.vScrollBar2);
            this.spcHScrollLevel2.Panel2MinSize = 0;
            this.spcHScrollLevel2.Size = new System.Drawing.Size(17, 295);
            this.spcHScrollLevel2.SplitterDistance = 64;
            this.spcHScrollLevel2.TabIndex = 0;
            this.spcHScrollLevel2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
            this.spcHScrollLevel2.DoubleClick += new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
            this.spcHScrollLevel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar1.Enabled = false;
            this.vScrollBar1.LargeChange = 1;
            this.vScrollBar1.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar1.Maximum = 0;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 64);
            this.vScrollBar1.TabIndex = 0;
            // 
            // vScrollBar2
            // 
            this.vScrollBar2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar2.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar2.Name = "vScrollBar2";
            this.vScrollBar2.Size = new System.Drawing.Size(17, 227);
            this.vScrollBar2.TabIndex = 1;
            this.vScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar2_Scroll);
            // 
            // vScrollBar3
            // 
            this.vScrollBar3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar3.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar3.Name = "vScrollBar3";
            this.vScrollBar3.Size = new System.Drawing.Size(17, 19);
            this.vScrollBar3.TabIndex = 2;
            this.vScrollBar3.Visible = false;
            this.vScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar3_Scroll);
            // 
            // pnlSpacer
            // 
            this.pnlSpacer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSpacer.Location = new System.Drawing.Point(0, 19);
            this.pnlSpacer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSpacer.Name = "pnlSpacer";
            this.pnlSpacer.Size = new System.Drawing.Size(17, 17);
            this.pnlSpacer.TabIndex = 4;
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
            // g5
            // 
            this.g5.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g5.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g5.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g5.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g5.ContextMenuStrip = this.cmsGrid;
            this.g5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g5.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g5.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g5.Location = new System.Drawing.Point(0, 0);
            this.g5.Name = "g5";
            this.g5.Rows.DefaultSize = 17;
            this.g5.Rows.Fixed = 0;
            this.g5.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g5.Size = new System.Drawing.Size(75, 225);
            this.g5.TabIndex = 1;
            this.g5.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g5_BeforeScroll);
            this.g5.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g5.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g5.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g5.CellChanged += new C1.Win.C1FlexGrid.RowColEventHandler(this.g5_CellChanged);
            this.g5.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g5_OwnerDrawCell);
            this.g5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            this.g5.AfterResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g5_AfterResizeColumn);
            // 
            // g2
            // 
            this.g2.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g2.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g2.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g2.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g2.ContextMenuStrip = this.cmsGrid;
            this.g2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g2.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g2.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g2.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g2.Location = new System.Drawing.Point(0, 0);
            this.g2.Name = "g2";
            this.g2.Rows.DefaultSize = 17;
            this.g2.Rows.Fixed = 0;
            this.g2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g2.Size = new System.Drawing.Size(75, 62);
            this.g2.TabIndex = 0;
            this.g2.BeforeAutosizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_BeforeAutosizeColumn);
            this.g2.BeforeResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.BeforeResizeColumn);
            this.g2.AfterResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_AfterResizeColumn);
            this.g2.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g2_BeforeScroll);
            this.g2.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g2.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g2.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g2.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g2_OwnerDrawCell);
            this.g2.DragDrop += new System.Windows.Forms.DragEventHandler(this.g2_DragDrop);
            this.g2.DragEnter += new System.Windows.Forms.DragEventHandler(this.g2_DragEnter);
            this.g2.DragOver += new System.Windows.Forms.DragEventHandler(this.g2_DragOver);
            this.g2.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.g2_QueryContinueDrag);
            this.g2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            this.g2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.g2_MouseMove);
            this.g2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.g2_MouseUp);
            this.g2.Resize += new System.EventHandler(this.g2_Resize);
            // 
            // g8
            // 
            this.g8.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g8.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g8.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g8.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g8.ContextMenuStrip = this.cmsGrid;
            this.g8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g8.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g8.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g8.Location = new System.Drawing.Point(0, 0);
            this.g8.Name = "g8";
            this.g8.Rows.DefaultSize = 17;
            this.g8.Rows.Fixed = 0;
            this.g8.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g8.Size = new System.Drawing.Size(75, 17);
            this.g8.TabIndex = 2;
            this.g8.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g8_BeforeScroll);
            this.g8.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g8.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g8.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g8.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g8_OwnerDrawCell);
            this.g8.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // hScrollBar2
            // 
            this.hScrollBar2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar2.Location = new System.Drawing.Point(0, 17);
            this.hScrollBar2.Name = "hScrollBar2";
            this.hScrollBar2.Size = new System.Drawing.Size(75, 17);
            this.hScrollBar2.TabIndex = 4;
            this.hScrollBar2.Visible = false;
            this.hScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
            // 
            // g6
            // 
            this.g6.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g6.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g6.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g6.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g6.ContextMenuStrip = this.cmsGrid;
            this.g6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g6.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g6.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g6.Location = new System.Drawing.Point(0, 0);
            this.g6.Name = "g6";
            this.g6.Rows.DefaultSize = 17;
            this.g6.Rows.Fixed = 0;
            this.g6.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g6.Size = new System.Drawing.Size(928, 225);
            this.g6.TabIndex = 1;
            this.g6.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g6_BeforeScroll);
            this.g6.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g6.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g6.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g6.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g6_OwnerDrawCell);
            this.g6.Click += new System.EventHandler(this.g6_Click);
            this.g6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            this.g6.MouseMove += new System.Windows.Forms.MouseEventHandler(this.g6_MouseMove);
            this.g6.AfterResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g6_AfterResizeColumn);
            // 
            // g3
            // 
            this.g3.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g3.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g3.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g3.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g3.ContextMenuStrip = this.cmsGrid;
            this.g3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g3.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g3.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g3.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g3.Location = new System.Drawing.Point(0, 0);
            this.g3.Name = "g3";
            this.g3.Rows.DefaultSize = 17;
            this.g3.Rows.Fixed = 0;
            this.g3.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g3.Size = new System.Drawing.Size(928, 62);
            this.g3.TabIndex = 0;
            this.g3.BeforeAutosizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_BeforeAutosizeColumn);
            this.g3.BeforeResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.BeforeResizeColumn);
            this.g3.AfterResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_AfterResizeColumn);
            this.g3.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g3_BeforeScroll);
            this.g3.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g3.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g3.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g3.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g3_OwnerDrawCell);
            this.g3.DragDrop += new System.Windows.Forms.DragEventHandler(this.g3_DragDrop);
            this.g3.DragEnter += new System.Windows.Forms.DragEventHandler(this.g3_DragEnter);
            this.g3.DragOver += new System.Windows.Forms.DragEventHandler(this.g3_DragOver);
            this.g3.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.g3_QueryContinueDrag);
            this.g3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            this.g3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.g3_MouseMove);
            this.g3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.g3_MouseUp);
            this.g3.Resize += new System.EventHandler(this.g3_Resize);
            // 
            // g9
            // 
            this.g9.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g9.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g9.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g9.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g9.ContextMenuStrip = this.cmsGrid;
            this.g9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g9.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g9.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g9.Location = new System.Drawing.Point(0, 0);
            this.g9.Name = "g9";
            this.g9.Rows.DefaultSize = 17;
            this.g9.Rows.Fixed = 0;
            this.g9.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g9.Size = new System.Drawing.Size(928, 17);
            this.g9.TabIndex = 2;
            this.g9.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g9_BeforeScroll);
            this.g9.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g9.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g9.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g9.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g9_OwnerDrawCell);
            this.g9.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // hScrollBar3
            // 
            this.hScrollBar3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar3.Location = new System.Drawing.Point(0, 17);
            this.hScrollBar3.Name = "hScrollBar3";
            this.hScrollBar3.Size = new System.Drawing.Size(928, 17);
            this.hScrollBar3.TabIndex = 4;
            this.hScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar3_Scroll);
            // 
            // _AssortmentView_Toolbars_Dock_Area_Left
            // 
            this._AssortmentView_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AssortmentView_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._AssortmentView_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._AssortmentView_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AssortmentView_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 0);
            this._AssortmentView_Toolbars_Dock_Area_Left.Name = "_AssortmentView_Toolbars_Dock_Area_Left";
            this._AssortmentView_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 439);
            this._AssortmentView_Toolbars_Dock_Area_Left.ToolbarsManager = this.utmMain;
            // 
            // _AssortmentView_Toolbars_Dock_Area_Right
            // 
            this._AssortmentView_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AssortmentView_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._AssortmentView_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._AssortmentView_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AssortmentView_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1131, 0);
            this._AssortmentView_Toolbars_Dock_Area_Right.Name = "_AssortmentView_Toolbars_Dock_Area_Right";
            this._AssortmentView_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 439);
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
            this._AssortmentView_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1131, 0);
            this._AssortmentView_Toolbars_Dock_Area_Top.ToolbarsManager = this.utmMain;
            // 
            // _AssortmentView_Toolbars_Dock_Area_Bottom
            // 
            this._AssortmentView_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AssortmentView_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._AssortmentView_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._AssortmentView_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AssortmentView_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 439);
            this._AssortmentView_Toolbars_Dock_Area_Bottom.Name = "_AssortmentView_Toolbars_Dock_Area_Bottom";
            this._AssortmentView_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1131, 0);
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
            // spcHDetailLevel1
            // 
            this.spcHDetailLevel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spcHDetailLevel1.ContextMenuStrip = this.cmsSplitter;
            this.spcHDetailLevel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcHDetailLevel1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spcHDetailLevel1.Location = new System.Drawing.Point(0, 0);
            this.spcHDetailLevel1.Margin = new System.Windows.Forms.Padding(0);
            this.spcHDetailLevel1.Name = "spcHDetailLevel1";
            this.spcHDetailLevel1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcHDetailLevel1.Panel1
            // 
            this.spcHDetailLevel1.Panel1.Controls.Add(this.spcHDetailLevel2);
            this.spcHDetailLevel1.Panel1MinSize = 0;
            // 
            // spcHDetailLevel1.Panel2
            // 
            this.spcHDetailLevel1.Panel2.Controls.Add(this.g9);
            this.spcHDetailLevel1.Panel2.Controls.Add(this.hScrollBar3);
            this.spcHDetailLevel1.Panel2MinSize = 0;
            this.spcHDetailLevel1.Size = new System.Drawing.Size(930, 335);
            this.spcHDetailLevel1.SplitterDistance = 295;
            this.spcHDetailLevel1.TabIndex = 0;
            this.spcHDetailLevel1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
            this.spcHDetailLevel1.DoubleClick += new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
            this.spcHDetailLevel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // spcHDetailLevel2
            // 
            this.spcHDetailLevel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spcHDetailLevel2.ContextMenuStrip = this.cmsSplitter;
            this.spcHDetailLevel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcHDetailLevel2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spcHDetailLevel2.Location = new System.Drawing.Point(0, 0);
            this.spcHDetailLevel2.Margin = new System.Windows.Forms.Padding(0);
            this.spcHDetailLevel2.Name = "spcHDetailLevel2";
            this.spcHDetailLevel2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcHDetailLevel2.Panel1
            // 
            this.spcHDetailLevel2.Panel1.Controls.Add(this.g3);
            this.spcHDetailLevel2.Panel1MinSize = 0;
            // 
            // spcHDetailLevel2.Panel2
            // 
            this.spcHDetailLevel2.Panel2.Controls.Add(this.g6);
            this.spcHDetailLevel2.Panel2MinSize = 0;
            this.spcHDetailLevel2.Size = new System.Drawing.Size(930, 295);
            this.spcHDetailLevel2.SplitterDistance = 64;
            this.spcHDetailLevel2.TabIndex = 0;
            this.spcHDetailLevel2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
            this.spcHDetailLevel2.DoubleClick += new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
            this.spcHDetailLevel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // spcHHeaderLevel1
            // 
            this.spcHHeaderLevel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spcHHeaderLevel1.ContextMenuStrip = this.cmsSplitter;
            this.spcHHeaderLevel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcHHeaderLevel1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spcHHeaderLevel1.Location = new System.Drawing.Point(0, 0);
            this.spcHHeaderLevel1.Margin = new System.Windows.Forms.Padding(0);
            this.spcHHeaderLevel1.Name = "spcHHeaderLevel1";
            this.spcHHeaderLevel1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcHHeaderLevel1.Panel1
            // 
            this.spcHHeaderLevel1.Panel1.Controls.Add(this.spcHHeaderLevel2);
            this.spcHHeaderLevel1.Panel1MinSize = 0;
            // 
            // spcHHeaderLevel1.Panel2
            // 
            this.spcHHeaderLevel1.Panel2.Controls.Add(this.g7);
            this.spcHHeaderLevel1.Panel2.Controls.Add(this.hScrollBar1);
            this.spcHHeaderLevel1.Panel2MinSize = 0;
            this.spcHHeaderLevel1.Size = new System.Drawing.Size(85, 335);
            this.spcHHeaderLevel1.SplitterDistance = 295;
            this.spcHHeaderLevel1.TabIndex = 5;
            this.spcHHeaderLevel1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
            this.spcHHeaderLevel1.DoubleClick += new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
            this.spcHHeaderLevel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // spcHHeaderLevel2
            // 
            this.spcHHeaderLevel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spcHHeaderLevel2.ContextMenuStrip = this.cmsSplitter;
            this.spcHHeaderLevel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcHHeaderLevel2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spcHHeaderLevel2.Location = new System.Drawing.Point(0, 0);
            this.spcHHeaderLevel2.Margin = new System.Windows.Forms.Padding(0);
            this.spcHHeaderLevel2.Name = "spcHHeaderLevel2";
            this.spcHHeaderLevel2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcHHeaderLevel2.Panel1
            // 
            this.spcHHeaderLevel2.Panel1.Controls.Add(this.g1);
            this.spcHHeaderLevel2.Panel1MinSize = 0;
            // 
            // spcHHeaderLevel2.Panel2
            // 
            this.spcHHeaderLevel2.Panel2.Controls.Add(this.g4);
            this.spcHHeaderLevel2.Panel2.Controls.Add(this.g4old);
            this.spcHHeaderLevel2.Panel2MinSize = 0;
            this.spcHHeaderLevel2.Size = new System.Drawing.Size(85, 295);
            this.spcHHeaderLevel2.SplitterDistance = 64;
            this.spcHHeaderLevel2.TabIndex = 0;
            this.spcHHeaderLevel2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
            this.spcHHeaderLevel2.DoubleClick += new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
            this.spcHHeaderLevel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // g1
            // 
            this.g1.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None;
            this.g1.AllowEditing = false;
            this.g1.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g1.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g1.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g1.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g1.ContextMenuStrip = this.cmsGrid;
            this.g1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g1.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g1.ExtendLastCol = true;
            this.g1.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g1.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g1.Location = new System.Drawing.Point(0, 0);
            this.g1.Name = "g1";
            this.g1.Rows.DefaultSize = 17;
            this.g1.Rows.Fixed = 0;
            this.g1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g1.Size = new System.Drawing.Size(83, 62);
            this.g1.TabIndex = 1;
            this.g1.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g1_BeforeScroll);
            this.g1.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g1.VisibleChanged += new System.EventHandler(this.g1_VisibleChanged);
            this.g1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // g4
            // 
            this.g4.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None;
            this.g4.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g4.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g4.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g4.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g4.ContextMenuStrip = this.cmsGrid;
            this.g4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g4.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g4.ExtendLastCol = true;
            this.g4.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g4.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g4.Location = new System.Drawing.Point(0, 0);
            this.g4.Name = "g4";
            this.g4.Rows.DefaultSize = 17;
            this.g4.Rows.Fixed = 0;
            this.g4.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g4.Size = new System.Drawing.Size(83, 225);
            this.g4.TabIndex = 2;
            this.g4.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g4_BeforeScroll);
            this.g4.AfterCollapse += new C1.Win.C1FlexGrid.RowColEventHandler(this.g4_AfterCollapse);
            this.g4.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g4_OwnerDrawCell);
            this.g4.DragDrop += new System.Windows.Forms.DragEventHandler(this.g4_DragDrop);
            this.g4.DragEnter += new System.Windows.Forms.DragEventHandler(this.g4_DragEnter);
            this.g4.DragOver += new System.Windows.Forms.DragEventHandler(this.g4_DragOver);
            this.g4.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.g4_QueryContinueDrag);
            this.g4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            this.g4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
            this.g4.Resize += new System.EventHandler(this.g4_Resize);
            // 
            // spcHTotalLevel1
            // 
            this.spcHTotalLevel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spcHTotalLevel1.ContextMenuStrip = this.cmsSplitter;
            this.spcHTotalLevel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcHTotalLevel1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spcHTotalLevel1.Location = new System.Drawing.Point(0, 0);
            this.spcHTotalLevel1.Margin = new System.Windows.Forms.Padding(0);
            this.spcHTotalLevel1.Name = "spcHTotalLevel1";
            this.spcHTotalLevel1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcHTotalLevel1.Panel1
            // 
            this.spcHTotalLevel1.Panel1.Controls.Add(this.spcHTotalLevel2);
            this.spcHTotalLevel1.Panel1MinSize = 0;
            // 
            // spcHTotalLevel1.Panel2
            // 
            this.spcHTotalLevel1.Panel2.Controls.Add(this.g8);
            this.spcHTotalLevel1.Panel2.Controls.Add(this.hScrollBar2);
            this.spcHTotalLevel1.Panel2MinSize = 0;
            this.spcHTotalLevel1.Size = new System.Drawing.Size(77, 335);
            this.spcHTotalLevel1.SplitterDistance = 295;
            this.spcHTotalLevel1.TabIndex = 5;
            this.spcHTotalLevel1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
            this.spcHTotalLevel1.DoubleClick += new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
            this.spcHTotalLevel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // spcHTotalLevel2
            // 
            this.spcHTotalLevel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spcHTotalLevel2.ContextMenuStrip = this.cmsSplitter;
            this.spcHTotalLevel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcHTotalLevel2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spcHTotalLevel2.Location = new System.Drawing.Point(0, 0);
            this.spcHTotalLevel2.Margin = new System.Windows.Forms.Padding(0);
            this.spcHTotalLevel2.Name = "spcHTotalLevel2";
            this.spcHTotalLevel2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcHTotalLevel2.Panel1
            // 
            this.spcHTotalLevel2.Panel1.Controls.Add(this.g2);
            this.spcHTotalLevel2.Panel1MinSize = 0;
            // 
            // spcHTotalLevel2.Panel2
            // 
            this.spcHTotalLevel2.Panel2.Controls.Add(this.g5);
            this.spcHTotalLevel2.Panel2MinSize = 0;
            this.spcHTotalLevel2.Size = new System.Drawing.Size(77, 295);
            this.spcHTotalLevel2.SplitterDistance = 64;
            this.spcHTotalLevel2.TabIndex = 0;
            this.spcHTotalLevel2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
            this.spcHTotalLevel2.DoubleClick += new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
            this.spcHTotalLevel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // spcVLevel1
            // 
            this.spcVLevel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spcVLevel1.ContextMenuStrip = this.cmsSplitter;
            this.spcVLevel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcVLevel1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spcVLevel1.Location = new System.Drawing.Point(3, 3);
            this.spcVLevel1.Margin = new System.Windows.Forms.Padding(0);
            this.spcVLevel1.Name = "spcVLevel1";
            // 
            // spcVLevel1.Panel1
            // 
            this.spcVLevel1.Panel1.Controls.Add(this.spcHHeaderLevel1);
            this.spcVLevel1.Panel1MinSize = 0;
            // 
            // spcVLevel1.Panel2
            // 
            this.spcVLevel1.Panel2.Controls.Add(this.spcVLevel2);
            this.spcVLevel1.Panel2MinSize = 0;
            this.spcVLevel1.Size = new System.Drawing.Size(1100, 335);
            this.spcVLevel1.SplitterDistance = 85;
            this.spcVLevel1.TabIndex = 12;
            this.spcVLevel1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcVLevel1_SplitterMoved);
            this.spcVLevel1.DoubleClick += new System.EventHandler(this.spcVLevel1_DoubleClick);
            this.spcVLevel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // spcVLevel2
            // 
            this.spcVLevel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spcVLevel2.ContextMenuStrip = this.cmsSplitter;
            this.spcVLevel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcVLevel2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spcVLevel2.Location = new System.Drawing.Point(0, 0);
            this.spcVLevel2.Margin = new System.Windows.Forms.Padding(0);
            this.spcVLevel2.Name = "spcVLevel2";
            // 
            // spcVLevel2.Panel1
            // 
            this.spcVLevel2.Panel1.Controls.Add(this.spcHTotalLevel1);
            this.spcVLevel2.Panel1MinSize = 0;
            // 
            // spcVLevel2.Panel2
            // 
            this.spcVLevel2.Panel2.Controls.Add(this.spcHDetailLevel1);
            this.spcVLevel2.Panel2MinSize = 0;
            this.spcVLevel2.Size = new System.Drawing.Size(1011, 335);
            this.spcVLevel2.SplitterDistance = 77;
            this.spcVLevel2.TabIndex = 0;
            this.spcVLevel2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcVLevel2_SplitterMoved);
            this.spcVLevel2.DoubleClick += new System.EventHandler(this.spcVLevel2_DoubleClick);
            this.spcVLevel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
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
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabAssortment);
            this.tabControl.Controls.Add(this.tabContent);
            this.tabControl.Controls.Add(this.tabProductChar);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 72);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1131, 367);
            this.tabControl.TabIndex = 5;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            this.tabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Selecting);
            // 
            // tabAssortment
            // 
            this.tabAssortment.Controls.Add(this.spcVLevel1);
            this.tabAssortment.Controls.Add(this.pnlScrollBars);
            this.tabAssortment.Location = new System.Drawing.Point(4, 22);
            this.tabAssortment.Name = "tabAssortment";
            this.tabAssortment.Padding = new System.Windows.Forms.Padding(3);
            this.tabAssortment.Size = new System.Drawing.Size(1123, 341);
            this.tabAssortment.TabIndex = 0;
            this.tabAssortment.Text = "Assortment";
            this.tabAssortment.UseVisualStyleBackColor = true;
            // 
            // tabContent
            // 
            this.tabContent.Controls.Add(this.ugDetails);
            this.tabContent.Location = new System.Drawing.Point(4, 22);
            this.tabContent.Name = "tabContent";
            this.tabContent.Padding = new System.Windows.Forms.Padding(3);
            this.tabContent.Size = new System.Drawing.Size(1123, 313);
            this.tabContent.TabIndex = 1;
            this.tabContent.Text = "Content";
            this.tabContent.UseVisualStyleBackColor = true;
            // 
            // ugDetails
            // 
            this.ugDetails.AllowDrop = true;
            this.ugDetails.ContextMenuStrip = this.cmsContentGrid;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            appearance10.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ugDetails.DisplayLayout.Appearance = appearance10;
            this.ugDetails.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugDetails.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ugDetails.DisplayLayout.GroupByBox.Hidden = true;
            this.ugDetails.DisplayLayout.MaxColScrollRegions = 1;
            this.ugDetails.DisplayLayout.MaxRowScrollRegions = 1;
            this.ugDetails.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ugDetails.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            this.ugDetails.DisplayLayout.Override.CardAreaAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            appearance12.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ugDetails.DisplayLayout.Override.CellAppearance = appearance12;
            this.ugDetails.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ugDetails.DisplayLayout.Override.CellPadding = 0;
            appearance13.BackColor = System.Drawing.SystemColors.Control;
            appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance13.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance13.BorderColor = System.Drawing.SystemColors.Window;
            this.ugDetails.DisplayLayout.Override.GroupByRowAppearance = appearance13;
            this.ugDetails.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ugDetails.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            appearance14.BorderColor = System.Drawing.Color.Silver;
            this.ugDetails.DisplayLayout.Override.RowAppearance = appearance14;
            this.ugDetails.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance15.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ugDetails.DisplayLayout.Override.TemplateAddRowAppearance = appearance15;
            this.ugDetails.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugDetails.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugDetails.Location = new System.Drawing.Point(3, 3);
            this.ugDetails.Name = "ugDetails";
            this.ugDetails.Size = new System.Drawing.Size(1117, 307);
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
            this.ugDetails.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ugDetails_MouseUp);
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
            this.cmsContentInsertColor.OwnerItem = this.cmsInsertBulkColor;
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
            // tabProductChar
            // 
            this.tabProductChar.Controls.Add(this.ugCharacteristics);
            this.tabProductChar.Location = new System.Drawing.Point(4, 22);
            this.tabProductChar.Name = "tabProductChar";
            this.tabProductChar.Padding = new System.Windows.Forms.Padding(3);
            this.tabProductChar.Size = new System.Drawing.Size(1123, 313);
            this.tabProductChar.TabIndex = 2;
            this.tabProductChar.Text = "Characteristics";
            this.tabProductChar.UseVisualStyleBackColor = true;
            // 
            // ugCharacteristics
            // 
            this.ugCharacteristics.AllowDrop = true;
            this.ugCharacteristics.ContextMenuStrip = this.cmsCharGrid;
            appearance16.BackColor = System.Drawing.SystemColors.Window;
            appearance16.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ugCharacteristics.DisplayLayout.Appearance = appearance16;
            this.ugCharacteristics.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugCharacteristics.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance17.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance17.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance17.BorderColor = System.Drawing.SystemColors.Window;
            this.ugCharacteristics.DisplayLayout.GroupByBox.Appearance = appearance17;
            appearance18.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugCharacteristics.DisplayLayout.GroupByBox.BandLabelAppearance = appearance18;
            this.ugCharacteristics.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ugCharacteristics.DisplayLayout.GroupByBox.Hidden = true;
            this.ugCharacteristics.DisplayLayout.MaxColScrollRegions = 1;
            this.ugCharacteristics.DisplayLayout.MaxRowScrollRegions = 1;
            this.ugCharacteristics.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ugCharacteristics.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.ugCharacteristics.DisplayLayout.Override.CardAreaAppearance = appearance19;
            this.ugCharacteristics.DisplayLayout.Override.CellAppearance = appearance12;
            this.ugCharacteristics.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ugCharacteristics.DisplayLayout.Override.CellPadding = 0;
            appearance20.BackColor = System.Drawing.SystemColors.Control;
            appearance20.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance20.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance20.BorderColor = System.Drawing.SystemColors.Window;
            this.ugCharacteristics.DisplayLayout.Override.GroupByRowAppearance = appearance20;
            appearance21.TextHAlignAsString = "Left";
            this.ugCharacteristics.DisplayLayout.Override.HeaderAppearance = appearance21;
            this.ugCharacteristics.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ugCharacteristics.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance22.BackColor = System.Drawing.SystemColors.Window;
            appearance22.BorderColor = System.Drawing.Color.Silver;
            this.ugCharacteristics.DisplayLayout.Override.RowAppearance = appearance22;
            this.ugCharacteristics.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance23.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ugCharacteristics.DisplayLayout.Override.TemplateAddRowAppearance = appearance23;
            this.ugCharacteristics.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugCharacteristics.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugCharacteristics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugCharacteristics.Location = new System.Drawing.Point(3, 3);
            this.ugCharacteristics.Name = "ugCharacteristics";
            this.ugCharacteristics.Size = new System.Drawing.Size(1117, 307);
            this.ugCharacteristics.TabIndex = 0;
            this.ugCharacteristics.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugCharacteristics_InitializeLayout);
            this.ugCharacteristics.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugCharacteristics_CellChange);
            this.ugCharacteristics.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugCharacteristics_MouseEnterElement);
            this.ugCharacteristics.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugCharacteristics_DragDrop);
            this.ugCharacteristics.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugCharacteristics_DragEnter);
            this.ugCharacteristics.DragOver += new System.Windows.Forms.DragEventHandler(this.ugCharacteristics_DragOver);
            this.ugCharacteristics.DragLeave += new System.EventHandler(this.ugCharacteristics_DragLeave);
            this.ugCharacteristics.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugCharacteristics_MouseDown);
            // 
            // cmsCharGrid
            // 
            this.cmsCharGrid.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuPopup;
            this.cmsCharGrid.AllowMerge = false;
            this.cmsCharGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsApplyToLowerLevels,
            this.cmsInheritFromHigherLevel});
            this.cmsCharGrid.Name = "cmsCharGrid";
            this.cmsCharGrid.ShowImageMargin = false;
            this.cmsCharGrid.Size = new System.Drawing.Size(177, 48);
            this.cmsCharGrid.Opening += new System.ComponentModel.CancelEventHandler(this.cmsCharGrid_Opening);
            // 
            // cmsApplyToLowerLevels
            // 
            this.cmsApplyToLowerLevels.Name = "cmsApplyToLowerLevels";
            this.cmsApplyToLowerLevels.Size = new System.Drawing.Size(176, 22);
            this.cmsApplyToLowerLevels.Text = "Apply to lower levels";
            this.cmsApplyToLowerLevels.Click += new System.EventHandler(this.cmsApplyToLowerLevels_Click);
            // 
            // cmsInheritFromHigherLevel
            // 
            this.cmsInheritFromHigherLevel.Name = "cmsInheritFromHigherLevel";
            this.cmsInheritFromHigherLevel.Size = new System.Drawing.Size(176, 22);
            this.cmsInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cmsInheritFromHigherLevel.Click += new System.EventHandler(this.cmsInheritFromHigherLevel_Click);
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
            // AssortmentView
            // 
            this.AllowDragDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 439);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.rtbScrollText);
            this.Controls.Add(this.lblFindSize);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this._AssortmentView_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._AssortmentView_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._AssortmentView_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._AssortmentView_Toolbars_Dock_Area_Top);
            this.Name = "AssortmentView";
            this.Text = "AssortmentView";
            this.Activated += new System.EventHandler(this.AssortmentView_Activated);
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
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g4old)).EndInit();
            this.cmsGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g7)).EndInit();
            this.pnlScrollBars.ResumeLayout(false);
            this.spcHScrollLevel1.Panel1.ResumeLayout(false);
            this.spcHScrollLevel1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcHScrollLevel1)).EndInit();
            this.spcHScrollLevel1.ResumeLayout(false);
            this.cmsSplitter.ResumeLayout(false);
            this.spcHScrollLevel2.Panel1.ResumeLayout(false);
            this.spcHScrollLevel2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcHScrollLevel2)).EndInit();
            this.spcHScrollLevel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g9)).EndInit();
            this.spcHDetailLevel1.Panel1.ResumeLayout(false);
            this.spcHDetailLevel1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcHDetailLevel1)).EndInit();
            this.spcHDetailLevel1.ResumeLayout(false);
            this.spcHDetailLevel2.Panel1.ResumeLayout(false);
            this.spcHDetailLevel2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcHDetailLevel2)).EndInit();
            this.spcHDetailLevel2.ResumeLayout(false);
            this.spcHHeaderLevel1.Panel1.ResumeLayout(false);
            this.spcHHeaderLevel1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcHHeaderLevel1)).EndInit();
            this.spcHHeaderLevel1.ResumeLayout(false);
            this.spcHHeaderLevel2.Panel1.ResumeLayout(false);
            this.spcHHeaderLevel2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcHHeaderLevel2)).EndInit();
            this.spcHHeaderLevel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g4)).EndInit();
            this.spcHTotalLevel1.Panel1.ResumeLayout(false);
            this.spcHTotalLevel1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcHTotalLevel1)).EndInit();
            this.spcHTotalLevel1.ResumeLayout(false);
            this.spcHTotalLevel2.Panel1.ResumeLayout(false);
            this.spcHTotalLevel2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcHTotalLevel2)).EndInit();
            this.spcHTotalLevel2.ResumeLayout(false);
            this.spcVLevel1.Panel1.ResumeLayout(false);
            this.spcVLevel1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcVLevel1)).EndInit();
            this.spcVLevel1.ResumeLayout(false);
            this.spcVLevel2.Panel1.ResumeLayout(false);
            this.spcVLevel2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcVLevel2)).EndInit();
            this.spcVLevel2.ResumeLayout(false);
            this.cmsActions.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabAssortment.ResumeLayout(false);
            this.tabContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugDetails)).EndInit();
            this.cmsContentGrid.ResumeLayout(false);
            this.cmsContentInsert.ResumeLayout(false);
            this.cmsContentInsertColor.ResumeLayout(false);
            this.tabProductChar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugCharacteristics)).EndInit();
            this.cmsCharGrid.ResumeLayout(false);
            this.cmsContentColChooser.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.HScrollBar hScrollBar1;
		private System.Windows.Forms.Panel pnlScrollBars;
		private System.Windows.Forms.HScrollBar hScrollBar2;
		private System.Windows.Forms.HScrollBar hScrollBar3;
		private C1.Win.C1FlexGrid.C1FlexGrid g4old;
		private C1.Win.C1FlexGrid.C1FlexGrid g7;
		private System.Windows.Forms.VScrollBar vScrollBar2;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		private System.Windows.Forms.VScrollBar vScrollBar3;
		private System.Windows.Forms.Panel pnlSpacer;
		private C1.Win.C1FlexGrid.C1FlexGrid g5;
		private C1.Win.C1FlexGrid.C1FlexGrid g2;
		private C1.Win.C1FlexGrid.C1FlexGrid g8;
		private C1.Win.C1FlexGrid.C1FlexGrid g6;
		private C1.Win.C1FlexGrid.C1FlexGrid g3;
		private C1.Win.C1FlexGrid.C1FlexGrid g9;
		private System.Windows.Forms.ToolTip toolTip1;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AssortmentView_Toolbars_Dock_Area_Left;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AssortmentView_Toolbars_Dock_Area_Right;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AssortmentView_Toolbars_Dock_Area_Top;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AssortmentView_Toolbars_Dock_Area_Bottom;
		//private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager utmMain;
		private System.Windows.Forms.RichTextBox rtbScrollText;
		private System.Windows.Forms.Label lblFindSize;
		private System.Windows.Forms.SplitContainer spcHDetailLevel1;
		private System.Windows.Forms.SplitContainer spcHDetailLevel2;
		private System.Windows.Forms.SplitContainer spcHHeaderLevel1;
		private System.Windows.Forms.SplitContainer spcHHeaderLevel2;
		private System.Windows.Forms.SplitContainer spcHTotalLevel1;
		private System.Windows.Forms.SplitContainer spcHTotalLevel2;
		private System.Windows.Forms.SplitContainer spcVLevel1;
		private System.Windows.Forms.SplitContainer spcVLevel2;
		private System.Windows.Forms.SplitContainer spcHScrollLevel1;
		private System.Windows.Forms.SplitContainer spcHScrollLevel2;
		private System.Windows.Forms.ContextMenuStrip cmsSplitter;
		private System.Windows.Forms.ToolStripMenuItem cmiLockSplitter;
		private System.Windows.Forms.ContextMenuStrip cmsGrid;
		private System.Windows.Forms.ToolStripMenuItem cmiColChooser;
		private System.Windows.Forms.ToolStripSeparator cmiLockSeparator;
		private System.Windows.Forms.ToolStripMenuItem cmiLockColumn;
		private System.Windows.Forms.ToolStripMenuItem cmiUnlockColumn;
		private System.Windows.Forms.ToolStripSeparator cmiFreezeSeparator;
		private System.Windows.Forms.ToolStripMenuItem cmiFreezeColumn;
		private C1.Win.C1FlexGrid.C1FlexGrid g1;
		private System.Windows.Forms.ToolStripMenuItem cmiLockCell;
		private System.Windows.Forms.ToolStripMenuItem cmiRowChooser;
		private System.Windows.Forms.ToolStripMenuItem cmiLockRow;
		private System.Windows.Forms.ToolStripMenuItem cmiUnlockRow;
		private System.Windows.Forms.ToolStripMenuItem cmiLockSheet;
		private System.Windows.Forms.ToolStripMenuItem cmiUnlockSheet;
		private MIDRetail.Windows.Controls.MIDFlexGrid g4;
		private System.Windows.Forms.Panel panel1;
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
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabAssortment;
        private System.Windows.Forms.TabPage tabContent;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugDetails;
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
        private System.Windows.Forms.TabPage tabProductChar;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugCharacteristics;
        private System.Windows.Forms.ContextMenuStrip cmsCharGrid;
        private System.Windows.Forms.ToolStripMenuItem cmsApplyToLowerLevels;
        private System.Windows.Forms.ToolStripMenuItem cmsInheritFromHigherLevel;
        private System.Windows.Forms.ContextMenuStrip cmsContentColChooser;
        private System.Windows.Forms.ToolStripMenuItem cmsColSelectrAll;
        private System.Windows.Forms.ToolStripMenuItem cmsColClearAll;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _pnlTop_Toolbars_Dock_Area_Left;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _pnlTop_Toolbars_Dock_Area_Right;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _pnlTop_Toolbars_Dock_Area_Bottom;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _pnlTop_Toolbars_Dock_Area_Top;
        private MIDToolbarRadioButton midToolbarRadioButton1;
        private MIDToolbarRadioButton midToolbarRadioButton2;
        private MIDAttributeComboBox midAttributeComboBox1;
        private MIDComboBoxEnh midComboBoxSet;
        private MIDComboBoxEnh midComboBoxView;
        private MIDComboBoxEnh midComboBoxAssortmentActions;
        private MIDComboBoxEnh midComboBoxAllocationActions;
	}
}
