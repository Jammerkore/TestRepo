using MIDRetail.Windows.Controls;
//Begin Track #5006 - JScott - Display Low-levels one at a time
using System.Windows.Forms;

//End Track #5006 - JScott - Display Low-levels one at a time
namespace MIDRetail.Windows
{
	partial class PlanViewRT
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
				if (_transaction != null)
				{
					_transaction.Dispose();
                    _transaction = null;
				}

				this.btnApply.Click -= new System.EventHandler(this.btnApply_Click);
				//Begin Track #5006 - JScott - Display Low-levels one at a time
				//this.btnThemeProperties.Click -= new System.EventHandler(this.btnThemeProperties_Click);
				//End Track #5006 - JScott - Display Low-levels one at a time
                //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                this.cboDollarScaling.SelectionChangeCommitted -= new System.EventHandler(this.cboDollarScaling_SelectionChangeCommitted);
                this.cboUnitScaling.SelectionChangeCommitted -= new System.EventHandler(this.cboUnitScaling_SelectionChangeCommitted);
                //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                this.cboDollarScaling.DropDownClosed -= new System.EventHandler(cboDollarScaling_DropDownClosed);
                this.cboUnitScaling.DropDownClosed -= new System.EventHandler(cboUnitScaling_DropDownClosed);
                // End TT#301-MD - JSmith - Controls are not functioning properly
                //BEGIN TT#6-MD-VStuart - Single Store Select
                //this.cboFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
                //END TT#6-MD-VStuart - Single Store Select
                this.cboFilter.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
				this.cboFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
				this.cboFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
				this.cboFilter.DropDown -= new System.EventHandler(this.cboFilter_DropDown);
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                this.cboFilter.DropDownClosed -= new System.EventHandler(cboFilter_DropDownClosed);
                // End TT#301-MD - JSmith - Controls are not functioning properly
                
                //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                this.cboView.SelectionChangeCommitted -= new System.EventHandler(this.cboView_SelectionChangeCommitted);
                this.cboAttributeSet.SelectionChangeCommitted -= new System.EventHandler(this.cboAttributeSet_SelectionChangeCommitted);  //TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                 this.cboView.DropDownClosed -= new System.EventHandler(cboView_DropDownClosed);
                this.cboAttributeSet.DropDownClosed -= new System.EventHandler(cboAttributeSet_DropDownClosed);
                // End TT#301-MD - JSmith - Controls are not functioning properly

                // Begin Track #4872 - JSmith - Global/User Attributes
                this.cboStoreAttribute.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
                this.cboStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
                // End Track #4872
                //this.cboStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
                //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly				
                this.cboStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);  
                //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                this.cboStoreAttribute.DropDownClosed -= new System.EventHandler(cboStoreAttribute_DropDownClosed);
                // End TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
				this.optGroupByTime.CheckedChanged -= new System.EventHandler(this.optGroupByTime_CheckedChanged);
				this.optGroupByVariable.CheckedChanged -= new System.EventHandler(this.optGroupByVariable_CheckedChanged);
				//Begin Track #5006 - JScott - Display Low-levels one at a time
				this.tsbNavigate.Click -= new System.EventHandler(this.tsbNavigate_Click);
				this.tsbFirst.Click -= new System.EventHandler(this.tsbFirst_Click);
				this.tsbPrevious.Click -= new System.EventHandler(this.tsbPrevious_Click);
				this.tsbNext.Click -= new System.EventHandler(this.tsbNext_Click);
				this.tsbLast.Click -= new System.EventHandler(this.tsbLast_Click);
				//End Track #5006 - JScott - Display Low-levels one at a time
				this.g4.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.RowHeaderMouseDown);
				this.g4.Resize -= new System.EventHandler(this.g4_Resize);
				this.g4.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g4_OwnerDrawCell);
				this.g4.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g4_BeforeScroll);
				this.g4.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
				this.cmiRowChooser.Click -= new System.EventHandler(this.cmiRowChooser_Click);
				this.cmiVariableChooser.Click -= new System.EventHandler(this.cmiVariableChooser_Click);
				this.cmiQuantityChooser.Click -= new System.EventHandler(this.cmiQuantityChooser_Click);
				this.cmiLockRow.Click -= new System.EventHandler(this.cmiLockRow_Click);
				this.cmiUnlockRow.Click -= new System.EventHandler(this.cmiUnlockRow_Click);
				this.cmiCascadeLockRow.Click -= new System.EventHandler(this.cmiCascadeLockRow_Click);
				this.cmiCascadeUnlockRow.Click -= new System.EventHandler(this.cmiCascadeUnlockRow_Click);
				this.g7.Click -= new System.EventHandler(this.g7_Click);
				this.g7.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.RowHeaderMouseDown);
				this.g7.Resize -= new System.EventHandler(this.g7_Resize);
				this.g7.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g7_OwnerDrawCell);
				this.g7.VisibleChanged -= new System.EventHandler(this.g7_VisibleChanged);
				this.g7.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g7_BeforeScroll);
				this.g7.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
				this.g10.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.RowHeaderMouseDown);
				this.g10.Resize -= new System.EventHandler(this.g10_Resize);
				this.g10.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g10_OwnerDrawCell);
				this.g10.VisibleChanged -= new System.EventHandler(this.g10_VisibleChanged);
				this.g10.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g10_BeforeScroll);
				this.g10.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
				this.spcHScrollLevel1.DoubleClick -= new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
				this.spcHScrollLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
				this.spcHScrollLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.cmiLockSplitter.Click -= new System.EventHandler(this.cmiLockSplitter_Click);
				this.spcHScrollLevel2.DoubleClick -= new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
				this.spcHScrollLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
				this.spcHScrollLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHScrollLevel3.DoubleClick -= new System.EventHandler(this.spcHScrollLevel3_DoubleClick);
				this.spcHScrollLevel3.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel3_SplitterMoved);
				this.spcHScrollLevel3.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.vScrollBar2.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.vScrollBar2_Scroll);
				this.vScrollBar3.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.vScrollBar3_Scroll);
				this.vScrollBar4.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.vScrollBar4_Scroll);
				this.g6.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g6.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g6.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.DetailMouseDown);
				this.g6.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g5_OwnerDrawCell);
				this.g6.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g5_BeforeScroll);
				this.g6.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g6.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.cmiLockCell.Click -= new System.EventHandler(this.cmiLockCell_Click);
				this.cmiCascadeLockCell.Click -= new System.EventHandler(this.cmiCascadeLockCell_Click);
				this.cmiCascadeUnlockCell.Click -= new System.EventHandler(this.cmiCascadeUnlockCell_Click);
				this.cmiBalance.Click -= new System.EventHandler(this.cmiBalance_Click);
				this.cmiCopyLowToHigh.Click -= new System.EventHandler(this.cmiCopyLowToHigh_Click);
				this.cmiBalanceLowLevels.Click -= new System.EventHandler(this.cmiBalanceLowLevels_Click);
				this.g3.BeforeResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.BeforeResizeColumn);
				this.g3.DragEnter -= new System.Windows.Forms.DragEventHandler(this.g2_DragEnter);
				this.g3.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ColHeaderMouseDown);
				this.g3.Resize -= new System.EventHandler(this.g2_Resize);
				this.g3.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.g2_MouseUp);
				this.g3.DragOver -= new System.Windows.Forms.DragEventHandler(this.g2_DragOver);
				this.g3.VisibleChanged -= new System.EventHandler(this.g2_VisibleChanged);
				this.g3.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g2_BeforeScroll);
				this.g3.DragDrop -= new System.Windows.Forms.DragEventHandler(this.g2_DragDrop);
				this.g3.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_AfterResizeColumn);
				this.g3.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.g2_MouseMove);
				this.g3.BeforeAutosizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_BeforeAutosizeColumn);
				this.g3.QueryContinueDrag -= new System.Windows.Forms.QueryContinueDragEventHandler(this.g2_QueryContinueDrag);
				this.cmsg2g3.Opening -= new System.ComponentModel.CancelEventHandler(this.cmsg2g3_Opening);
				this.cmiColumnChooser.Click -= new System.EventHandler(this.cmiColumnChooser_Click);
				this.cmiLockColumn.Click -= new System.EventHandler(this.cmiLockColumn_Click);
				this.cmiUnlockColumn.Click -= new System.EventHandler(this.cmiUnlockColumn_Click);
				this.cmiCascadeLockColumn.Click -= new System.EventHandler(this.cmiCascadeLockColumn_Click);
				this.cmiCascadeUnlockColumn.Click -= new System.EventHandler(this.cmiCascadeUnlockColumn_Click);
				this.cmiFreezeColumn.Click -= new System.EventHandler(this.cmiFreezeColumn_Click);

                this.cmiShow.Click -= new System.EventHandler(this.cmiShow_Click);

                //this.cmiShowPeriods.Click -= new System.EventHandler(this.cmiShowPeriods_Click);
                //this.cmiShowWeeks.Click -= new System.EventHandler(this.cmiShowWeeks_Click);
				this.g9.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g9.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g9.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.DetailMouseDown);
				this.g9.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g8_OwnerDrawCell);
				this.g9.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g8_BeforeScroll);
				this.g9.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g9.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.g12.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g12.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g12.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.DetailMouseDown);
				this.g12.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g11_OwnerDrawCell);
				this.g12.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g11_BeforeScroll);
				this.g12.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g12.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.hScrollBar2.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
				this.g5.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g5.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g5.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.DetailMouseDown);
				this.g5.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g6_OwnerDrawCell);
				this.g5.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g6_BeforeScroll);
				this.g5.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g5.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.g2.BeforeResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.BeforeResizeColumn);
				this.g2.DragEnter -= new System.Windows.Forms.DragEventHandler(this.g3_DragEnter);
				this.g2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ColHeaderMouseDown);
				this.g2.Resize -= new System.EventHandler(this.g3_Resize);
				this.g2.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g3_OwnerDrawCell);
				this.g2.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.g3_MouseUp);
				this.g2.DragOver -= new System.Windows.Forms.DragEventHandler(this.g3_DragOver);
				this.g2.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g3_BeforeScroll);
				this.g2.DragDrop -= new System.Windows.Forms.DragEventHandler(this.g3_DragDrop);
				this.g2.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_AfterResizeColumn);
				this.g2.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.g3_MouseMove);
				this.g2.BeforeAutosizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_BeforeAutosizeColumn);
				this.g2.QueryContinueDrag -= new System.Windows.Forms.QueryContinueDragEventHandler(this.g3_QueryContinueDrag);
				this.g8.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g8.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g8.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.DetailMouseDown);
				this.g8.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g9_OwnerDrawCell);
				this.g8.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g9_BeforeScroll);
				this.g8.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g8.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.g11.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g11.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g11.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.DetailMouseDown);
				this.g11.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g12_OwnerDrawCell);
				this.g11.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g12_BeforeScroll);
				this.g11.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g11.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.hScrollBar3.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.hScrollBar3_Scroll);
				this.spcHDetailLevel1.DoubleClick -= new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
				this.spcHDetailLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
				this.spcHDetailLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHDetailLevel2.DoubleClick -= new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
				this.spcHDetailLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
				this.spcHDetailLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHDetailLevel3.DoubleClick -= new System.EventHandler(this.spcHScrollLevel3_DoubleClick);
				this.spcHDetailLevel3.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel3_SplitterMoved);
				this.spcHDetailLevel3.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHHeaderLevel1.DoubleClick -= new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
				this.spcHHeaderLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
				this.spcHHeaderLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHHeaderLevel2.DoubleClick -= new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
				this.spcHHeaderLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
				this.spcHHeaderLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.cmiLockSheet.Click -= new System.EventHandler(this.cmiLockSheet_Click);
				this.cmiUnlockSheet.Click -= new System.EventHandler(this.cmiUnlockSheet_Click);
				this.cmiCascadeLockSheet.Click -= new System.EventHandler(this.cmiCascadeLockSheet_Click);
				this.cmiCascadeUnlockSheet.Click -= new System.EventHandler(this.cmiCascadeUnlockSheet_Click);
				this.spcHHeaderLevel3.DoubleClick -= new System.EventHandler(this.spcHScrollLevel3_DoubleClick);
				this.spcHHeaderLevel3.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel3_SplitterMoved);
				this.spcHHeaderLevel3.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHTotalLevel1.DoubleClick -= new System.EventHandler(this.spcHScrollLevel1_DoubleClick);
				this.spcHTotalLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel1_SplitterMoved);
				this.spcHTotalLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHTotalLevel2.DoubleClick -= new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
				this.spcHTotalLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
				this.spcHTotalLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcHTotalLevel3.DoubleClick -= new System.EventHandler(this.spcHScrollLevel3_DoubleClick);
				this.spcHTotalLevel3.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel3_SplitterMoved);
				this.spcHTotalLevel3.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcVLevel1.DoubleClick -= new System.EventHandler(this.spcVLevel1_DoubleClick);
				this.spcVLevel1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcVLevel1_SplitterMoved);
				this.spcVLevel1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
				this.spcVLevel2.DoubleClick -= new System.EventHandler(this.spcVLevel2_DoubleClick);
				this.spcVLevel2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.spcVLevel2_SplitterMoved);
				this.spcVLevel2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
                this.cboDollarScaling.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboDollarScaling_MIDComboBoxPropertiesChangedEvent);
                this.cboUnitScaling.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboUnitScaling_MIDComboBoxPropertiesChangedEvent);
                this.cboFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboFilter_MIDComboBoxPropertiesChangedEvent);
                this.cboView.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboView_MIDComboBoxPropertiesChangedEvent);
                this.cboAttributeSet.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboAttributeSet_MIDComboBoxPropertiesChangedEvent);
				this.Load -= new System.EventHandler(this.PlanView_Load);
				this.Activated -= new System.EventHandler(this.PlanView_Activated);
				this.Closing -= new System.ComponentModel.CancelEventHandler(this.PlanView_Closing);

				g2MouseUpRefireHandler -= new System.Windows.Forms.MouseEventHandler(this.g2_MouseUp);
				g3MouseUpRefireHandler -= new System.Windows.Forms.MouseEventHandler(this.g3_MouseUp);

				if (_cmiBasisList != null)
				{
					foreach (System.Windows.Forms.ToolStripItem menuItem in _cmiBasisList)
					{
						menuItem.Click -= new System.EventHandler(this.cmiBasis_Click);
					}
				}

				if (_saveForm != null)
				{
					_saveForm.OnPlanSaveClosingEventHandler -= new PlanViewSave.PlanSaveClosingEventHandler(OnPlanSaveClosing);
				}

				if (_frmThemeProperties != null)
				{
					_frmThemeProperties.ApplyButtonClicked -= new System.EventHandler(StylePropertiesOnChanged);
				}
				//Begin Track #5006 - JScott - Display Low-levels one at a time

				if (_navigateItemList != null)
				{
					foreach (ToolStripMenuItem menuItem in _navigateItemList)
					{
						menuItem.Click -= new System.EventHandler(this.cmiNavigate_Click);
					}
				}
				//End Track #5006 - JScott - Display Low-levels one at a time

                // Begin TT#856 - JSmith - Out of memory
                ((MIDStoreAttributeComboBoxTag)cboStoreAttribute.Tag).Dispose();
                ((MIDStoreFilterComboBoxTag)cboFilter.Tag).Dispose();
                cboStoreAttribute.Tag = null;
                cboFilter.Tag = null;
                if (_planCubeGroup != null)
                {
                    _planCubeGroup.Dispose();
                    _planCubeGroup = null;
                }
                if (toolTip1 != null)
                {
                    toolTip1.RemoveAll();
                    toolTip1.Dispose();
                    toolTip1 = null;
                }
                // End TT#856
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.pnlSpacer1 = new System.Windows.Forms.Panel();
            this.cboDollarScaling = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboUnitScaling = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboAttributeSet = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.pnlSpacer2 = new System.Windows.Forms.Panel();
            this.pnlGroupBy = new System.Windows.Forms.Panel();
            this.optGroupByTime = new System.Windows.Forms.RadioButton();
            this.optGroupByVariable = new System.Windows.Forms.RadioButton();
            this.pnlSpacer3 = new System.Windows.Forms.Panel();
            this.pnlNavigate = new System.Windows.Forms.Panel();
            this.tspNavigate = new System.Windows.Forms.ToolStrip();
            this.tsbNavigate = new System.Windows.Forms.ToolStripButton();
            this.tsbFirst = new System.Windows.Forms.ToolStripButton();
            this.tsbPrevious = new System.Windows.Forms.ToolStripButton();
            this.tsbNext = new System.Windows.Forms.ToolStripButton();
            this.tsbLast = new System.Windows.Forms.ToolStripButton();
            this.g4 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.cmsg4g7g10 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiRowChooser = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiVariableChooser = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiQuantityChooser = new System.Windows.Forms.ToolStripMenuItem();
            this.cmig4g7g10Seperator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiLockRow = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiUnlockRow = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCascadeLockRow = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCascadeUnlockRow = new System.Windows.Forms.ToolStripMenuItem();
            this.cmig4g7g10Seperator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiFreezeRow = new System.Windows.Forms.ToolStripMenuItem();
            this.g7 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.g10 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.pnlScrollBars = new System.Windows.Forms.Panel();
            this.spcHScrollLevel1 = new System.Windows.Forms.SplitContainer();
            this.cmsSplitter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiLockSplitter = new System.Windows.Forms.ToolStripMenuItem();
            this.spcHScrollLevel2 = new System.Windows.Forms.SplitContainer();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.spcHScrollLevel3 = new System.Windows.Forms.SplitContainer();
            this.vScrollBar2 = new System.Windows.Forms.VScrollBar();
            this.vScrollBar3 = new System.Windows.Forms.VScrollBar();
            this.vScrollBar4 = new System.Windows.Forms.VScrollBar();
            this.pnlSpacer = new System.Windows.Forms.Panel();
            this.rtbScrollText = new System.Windows.Forms.RichTextBox();
            this.g6 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.cmsCell = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiLockCell = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCascadeLockCell = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCascadeUnlockCell = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiBalance = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCopyLowToHigh = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiBalanceLowLevels = new System.Windows.Forms.ToolStripMenuItem();
            this.g3 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.cmsg2g3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiColumnChooser = new System.Windows.Forms.ToolStripMenuItem();
            this.cmig2g3Seperator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiLockColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiUnlockColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCascadeLockColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCascadeUnlockColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.cmig2g3Seperator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiFreezeColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.cmig2g3Seperator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiShow = new System.Windows.Forms.ToolStripMenuItem();
            this.g9 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.g12 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.hScrollBar2 = new System.Windows.Forms.HScrollBar();
            this.g5 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.g2 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.g8 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.g11 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.hScrollBar3 = new System.Windows.Forms.HScrollBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblFindSize = new System.Windows.Forms.Label();
            this.spcHDetailLevel1 = new System.Windows.Forms.SplitContainer();
            this.spcHDetailLevel2 = new System.Windows.Forms.SplitContainer();
            this.spcHDetailLevel3 = new System.Windows.Forms.SplitContainer();
            this.spcHHeaderLevel1 = new System.Windows.Forms.SplitContainer();
            this.spcHHeaderLevel2 = new System.Windows.Forms.SplitContainer();
            this.cmsg1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiLockSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiUnlockSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCascadeLockSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCascadeUnlockSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.spcHHeaderLevel3 = new System.Windows.Forms.SplitContainer();
            this.spcHTotalLevel1 = new System.Windows.Forms.SplitContainer();
            this.spcHTotalLevel2 = new System.Windows.Forms.SplitContainer();
            this.spcHTotalLevel3 = new System.Windows.Forms.SplitContainer();
            this.spcVLevel1 = new System.Windows.Forms.SplitContainer();
            this.spcVLevel2 = new System.Windows.Forms.SplitContainer();
            this.cmsNavigate = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pnlControls.SuspendLayout();
            this.pnlGroupBy.SuspendLayout();
            this.pnlNavigate.SuspendLayout();
            this.tspNavigate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g4)).BeginInit();
            this.cmsg4g7g10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g10)).BeginInit();
            this.pnlScrollBars.SuspendLayout();
            this.spcHScrollLevel1.Panel1.SuspendLayout();
            this.spcHScrollLevel1.Panel2.SuspendLayout();
            this.spcHScrollLevel1.SuspendLayout();
            this.cmsSplitter.SuspendLayout();
            this.spcHScrollLevel2.Panel1.SuspendLayout();
            this.spcHScrollLevel2.Panel2.SuspendLayout();
            this.spcHScrollLevel2.SuspendLayout();
            this.spcHScrollLevel3.Panel1.SuspendLayout();
            this.spcHScrollLevel3.Panel2.SuspendLayout();
            this.spcHScrollLevel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g6)).BeginInit();
            this.cmsCell.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g3)).BeginInit();
            this.cmsg2g3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g11)).BeginInit();
            this.spcHDetailLevel1.Panel1.SuspendLayout();
            this.spcHDetailLevel1.Panel2.SuspendLayout();
            this.spcHDetailLevel1.SuspendLayout();
            this.spcHDetailLevel2.Panel1.SuspendLayout();
            this.spcHDetailLevel2.Panel2.SuspendLayout();
            this.spcHDetailLevel2.SuspendLayout();
            this.spcHDetailLevel3.Panel1.SuspendLayout();
            this.spcHDetailLevel3.Panel2.SuspendLayout();
            this.spcHDetailLevel3.SuspendLayout();
            this.spcHHeaderLevel1.Panel1.SuspendLayout();
            this.spcHHeaderLevel1.Panel2.SuspendLayout();
            this.spcHHeaderLevel1.SuspendLayout();
            this.spcHHeaderLevel2.Panel2.SuspendLayout();
            this.spcHHeaderLevel2.SuspendLayout();
            this.cmsg1.SuspendLayout();
            this.spcHHeaderLevel3.Panel1.SuspendLayout();
            this.spcHHeaderLevel3.Panel2.SuspendLayout();
            this.spcHHeaderLevel3.SuspendLayout();
            this.spcHTotalLevel1.Panel1.SuspendLayout();
            this.spcHTotalLevel1.Panel2.SuspendLayout();
            this.spcHTotalLevel1.SuspendLayout();
            this.spcHTotalLevel2.Panel1.SuspendLayout();
            this.spcHTotalLevel2.Panel2.SuspendLayout();
            this.spcHTotalLevel2.SuspendLayout();
            this.spcHTotalLevel3.Panel1.SuspendLayout();
            this.spcHTotalLevel3.Panel2.SuspendLayout();
            this.spcHTotalLevel3.SuspendLayout();
            this.spcVLevel1.Panel1.SuspendLayout();
            this.spcVLevel1.Panel2.SuspendLayout();
            this.spcVLevel1.SuspendLayout();
            this.spcVLevel2.Panel1.SuspendLayout();
            this.spcVLevel2.Panel2.SuspendLayout();
            this.spcVLevel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.Ribbon.Visible = true;
            this.utmMain.ShowFullMenusDelay = 500;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.pnlControls);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(8, 55);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1146, 40);
            this.pnlTop.TabIndex = 0;
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.btnApply);
            this.pnlControls.Controls.Add(this.pnlSpacer1);
            this.pnlControls.Controls.Add(this.cboDollarScaling);
            this.pnlControls.Controls.Add(this.cboUnitScaling);
            this.pnlControls.Controls.Add(this.cboFilter);
            this.pnlControls.Controls.Add(this.cboView);
            this.pnlControls.Controls.Add(this.cboAttributeSet);
            this.pnlControls.Controls.Add(this.cboStoreAttribute);
            this.pnlControls.Controls.Add(this.pnlSpacer2);
            this.pnlControls.Controls.Add(this.pnlGroupBy);
            this.pnlControls.Controls.Add(this.pnlSpacer3);
            this.pnlControls.Controls.Add(this.pnlNavigate);
            this.pnlControls.Location = new System.Drawing.Point(8, 8);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(901, 24);
            this.pnlControls.TabIndex = 11;
            // 
            // btnApply
            // 
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnApply.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnApply.Location = new System.Drawing.Point(813, 0);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(64, 24);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "Apply";
            this.toolTip1.SetToolTip(this.btnApply, "Apply current changes");
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // pnlSpacer1
            // 
            this.pnlSpacer1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSpacer1.Location = new System.Drawing.Point(797, 0);
            this.pnlSpacer1.Name = "pnlSpacer1";
            this.pnlSpacer1.Size = new System.Drawing.Size(16, 24);
            this.pnlSpacer1.TabIndex = 11;
            // 
            // cboDollarScaling
            // 
            this.cboDollarScaling.AutoAdjust = true;
            this.cboDollarScaling.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDollarScaling.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDollarScaling.DataSource = null;
            this.cboDollarScaling.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboDollarScaling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDollarScaling.DropDownWidth = 68;
            this.cboDollarScaling.FormattingEnabled = false;
            this.cboDollarScaling.IgnoreFocusLost = false;
            this.cboDollarScaling.ItemHeight = 13;
            this.cboDollarScaling.Location = new System.Drawing.Point(729, 0);
            this.cboDollarScaling.Margin = new System.Windows.Forms.Padding(0);
            this.cboDollarScaling.MaxDropDownItems = 25;
            this.cboDollarScaling.Name = "cboDollarScaling";
            this.cboDollarScaling.SetToolTip = "";
            this.cboDollarScaling.Size = new System.Drawing.Size(68, 24);
            this.cboDollarScaling.TabIndex = 15;
            this.cboDollarScaling.Tag = null;
            this.cboDollarScaling.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboDollarScaling_MIDComboBoxPropertiesChangedEvent);
            this.cboDollarScaling.SelectionChangeCommitted += new System.EventHandler(this.cboDollarScaling_SelectionChangeCommitted);
            this.cboDollarScaling.DropDownClosed += new System.EventHandler(this.cboDollarScaling_DropDownClosed);
            // 
            // cboUnitScaling
            // 
            this.cboUnitScaling.AutoAdjust = true;
            this.cboUnitScaling.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboUnitScaling.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboUnitScaling.DataSource = null;
            this.cboUnitScaling.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboUnitScaling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUnitScaling.DropDownWidth = 68;
            this.cboUnitScaling.FormattingEnabled = false;
            this.cboUnitScaling.IgnoreFocusLost = false;
            this.cboUnitScaling.ItemHeight = 13;
            this.cboUnitScaling.Location = new System.Drawing.Point(661, 0);
            this.cboUnitScaling.Margin = new System.Windows.Forms.Padding(0);
            this.cboUnitScaling.MaxDropDownItems = 25;
            this.cboUnitScaling.Name = "cboUnitScaling";
            this.cboUnitScaling.SetToolTip = "";
            this.cboUnitScaling.Size = new System.Drawing.Size(68, 24);
            this.cboUnitScaling.TabIndex = 14;
            this.cboUnitScaling.Tag = null;
            this.cboUnitScaling.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboUnitScaling_MIDComboBoxPropertiesChangedEvent);
            this.cboUnitScaling.SelectionChangeCommitted += new System.EventHandler(this.cboUnitScaling_SelectionChangeCommitted);
            this.cboUnitScaling.DropDownClosed += new System.EventHandler(this.cboUnitScaling_DropDownClosed);
            // 
            // cboFilter
            // 
            this.cboFilter.AllowDrop = true;
            this.cboFilter.AutoAdjust = true;
            this.cboFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFilter.DataSource = null;
            this.cboFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.DropDownWidth = 100;
            this.cboFilter.FormattingEnabled = false;
            this.cboFilter.IgnoreFocusLost = false;
            this.cboFilter.ItemHeight = 13;
            this.cboFilter.Location = new System.Drawing.Point(561, 0);
            this.cboFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboFilter.MaxDropDownItems = 25;
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.SetToolTip = "";
            this.cboFilter.Size = new System.Drawing.Size(100, 24);
            this.cboFilter.TabIndex = 10;
            this.cboFilter.Tag = null;
            this.cboFilter.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboFilter_MIDComboBoxPropertiesChangedEvent);
            this.cboFilter.SelectionChangeCommitted += new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
            this.cboFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboFilter.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
            this.cboFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            this.cboFilter.DropDown += new System.EventHandler(this.cboFilter_DropDown);
            this.cboFilter.DropDownClosed += new System.EventHandler(this.cboFilter_DropDownClosed);
            // 
            // cboView
            // 
            this.cboView.AutoAdjust = true;
            this.cboView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboView.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboView.DataSource = null;
            this.cboView.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboView.DropDownWidth = 100;
            this.cboView.FormattingEnabled = false;
            this.cboView.IgnoreFocusLost = false;
            this.cboView.ItemHeight = 13;
            this.cboView.Location = new System.Drawing.Point(461, 0);
            this.cboView.Margin = new System.Windows.Forms.Padding(0);
            this.cboView.MaxDropDownItems = 25;
            this.cboView.Name = "cboView";
            this.cboView.SetToolTip = "";
            this.cboView.Size = new System.Drawing.Size(100, 24);
            this.cboView.TabIndex = 9;
            this.cboView.Tag = null;
            this.cboView.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboView_MIDComboBoxPropertiesChangedEvent);
            this.cboView.SelectionChangeCommitted += new System.EventHandler(this.cboView_SelectionChangeCommitted);
            this.cboView.DropDownClosed += new System.EventHandler(this.cboView_DropDownClosed);
            // 
            // cboAttributeSet
            // 
            this.cboAttributeSet.AutoAdjust = true;
            this.cboAttributeSet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAttributeSet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAttributeSet.DataSource = null;
            this.cboAttributeSet.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboAttributeSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAttributeSet.DropDownWidth = 100;
            this.cboAttributeSet.FormattingEnabled = false;
            this.cboAttributeSet.IgnoreFocusLost = false;
            this.cboAttributeSet.ItemHeight = 13;
            this.cboAttributeSet.Location = new System.Drawing.Point(361, 0);
            this.cboAttributeSet.Margin = new System.Windows.Forms.Padding(0);
            this.cboAttributeSet.MaxDropDownItems = 25;
            this.cboAttributeSet.Name = "cboAttributeSet";
            this.cboAttributeSet.SetToolTip = "";
            this.cboAttributeSet.Size = new System.Drawing.Size(100, 24);
            this.cboAttributeSet.TabIndex = 2;
            this.cboAttributeSet.Tag = null;
            this.cboAttributeSet.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboAttributeSet_MIDComboBoxPropertiesChangedEvent);
            this.cboAttributeSet.SelectionChangeCommitted += new System.EventHandler(this.cboAttributeSet_SelectionChangeCommitted);
            this.cboAttributeSet.DropDownClosed += new System.EventHandler(this.cboAttributeSet_DropDownClosed);
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.AllowDrop = true;
            this.cboStoreAttribute.AllowUserAttributes = false;
            this.cboStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreAttribute.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreAttribute.Location = new System.Drawing.Point(261, 0);
            this.cboStoreAttribute.Name = "cboStoreAttribute";
            this.cboStoreAttribute.Size = new System.Drawing.Size(100, 21);
            this.cboStoreAttribute.TabIndex = 1;
            this.toolTip1.SetToolTip(this.cboStoreAttribute, "Attributes");
            this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            this.cboStoreAttribute.DropDownClosed += new System.EventHandler(this.cboStoreAttribute_DropDownClosed);
            this.cboStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            this.cboStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
            // 
            // pnlSpacer2
            // 
            this.pnlSpacer2.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSpacer2.Location = new System.Drawing.Point(245, 0);
            this.pnlSpacer2.Name = "pnlSpacer2";
            this.pnlSpacer2.Size = new System.Drawing.Size(16, 24);
            this.pnlSpacer2.TabIndex = 18;
            // 
            // pnlGroupBy
            // 
            this.pnlGroupBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGroupBy.Controls.Add(this.optGroupByTime);
            this.pnlGroupBy.Controls.Add(this.optGroupByVariable);
            this.pnlGroupBy.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlGroupBy.Location = new System.Drawing.Point(125, 0);
            this.pnlGroupBy.Name = "pnlGroupBy";
            this.pnlGroupBy.Size = new System.Drawing.Size(120, 24);
            this.pnlGroupBy.TabIndex = 17;
            // 
            // optGroupByTime
            // 
            this.optGroupByTime.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.optGroupByTime.Checked = true;
            this.optGroupByTime.Location = new System.Drawing.Point(8, 3);
            this.optGroupByTime.Name = "optGroupByTime";
            this.optGroupByTime.Size = new System.Drawing.Size(48, 16);
            this.optGroupByTime.TabIndex = 0;
            this.optGroupByTime.TabStop = true;
            this.optGroupByTime.Text = "Time";
            this.optGroupByTime.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.toolTip1.SetToolTip(this.optGroupByTime, "Group by Time Period");
            this.optGroupByTime.CheckedChanged += new System.EventHandler(this.optGroupByTime_CheckedChanged);
            // 
            // optGroupByVariable
            // 
            this.optGroupByVariable.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.optGroupByVariable.Location = new System.Drawing.Point(56, 3);
            this.optGroupByVariable.Name = "optGroupByVariable";
            this.optGroupByVariable.Size = new System.Drawing.Size(64, 16);
            this.optGroupByVariable.TabIndex = 1;
            this.optGroupByVariable.Text = "Variable";
            this.optGroupByVariable.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.toolTip1.SetToolTip(this.optGroupByVariable, "Group by Variable");
            this.optGroupByVariable.CheckedChanged += new System.EventHandler(this.optGroupByVariable_CheckedChanged);
            // 
            // pnlSpacer3
            // 
            this.pnlSpacer3.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSpacer3.Location = new System.Drawing.Point(109, 0);
            this.pnlSpacer3.Name = "pnlSpacer3";
            this.pnlSpacer3.Size = new System.Drawing.Size(16, 24);
            this.pnlSpacer3.TabIndex = 12;
            // 
            // pnlNavigate
            // 
            this.pnlNavigate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlNavigate.Controls.Add(this.tspNavigate);
            this.pnlNavigate.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlNavigate.Location = new System.Drawing.Point(0, 0);
            this.pnlNavigate.Name = "pnlNavigate";
            this.pnlNavigate.Size = new System.Drawing.Size(109, 24);
            this.pnlNavigate.TabIndex = 20;
            // 
            // tspNavigate
            // 
            this.tspNavigate.CanOverflow = false;
            this.tspNavigate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tspNavigate.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tspNavigate.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tspNavigate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNavigate,
            this.tsbFirst,
            this.tsbPrevious,
            this.tsbNext,
            this.tsbLast});
            this.tspNavigate.Location = new System.Drawing.Point(0, 0);
            this.tspNavigate.MaximumSize = new System.Drawing.Size(121, 0);
            this.tspNavigate.Name = "tspNavigate";
            this.tspNavigate.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tspNavigate.Size = new System.Drawing.Size(107, 22);
            this.tspNavigate.TabIndex = 19;
            this.tspNavigate.Text = "tspNavigate";
            // 
            // tsbNavigate
            // 
            this.tsbNavigate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNavigate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNavigate.Margin = new System.Windows.Forms.Padding(0);
            this.tsbNavigate.Name = "tsbNavigate";
            this.tsbNavigate.Size = new System.Drawing.Size(23, 22);
            this.tsbNavigate.Text = "tsbNavigate";
            this.tsbNavigate.ToolTipText = "Show Navigate Menu";
            this.tsbNavigate.Click += new System.EventHandler(this.tsbNavigate_Click);
            // 
            // tsbFirst
            // 
            this.tsbFirst.AutoSize = false;
            this.tsbFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFirst.Margin = new System.Windows.Forms.Padding(0);
            this.tsbFirst.Name = "tsbFirst";
            this.tsbFirst.Size = new System.Drawing.Size(18, 18);
            this.tsbFirst.Text = "tsbFirst";
            this.tsbFirst.ToolTipText = "Navigate to First";
            this.tsbFirst.Click += new System.EventHandler(this.tsbFirst_Click);
            // 
            // tsbPrevious
            // 
            this.tsbPrevious.AutoSize = false;
            this.tsbPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrevious.Margin = new System.Windows.Forms.Padding(0);
            this.tsbPrevious.Name = "tsbPrevious";
            this.tsbPrevious.Size = new System.Drawing.Size(18, 18);
            this.tsbPrevious.Text = "tsbPrevious";
            this.tsbPrevious.ToolTipText = "Navigate to Previous";
            this.tsbPrevious.Click += new System.EventHandler(this.tsbPrevious_Click);
            // 
            // tsbNext
            // 
            this.tsbNext.AutoSize = false;
            this.tsbNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNext.Margin = new System.Windows.Forms.Padding(0);
            this.tsbNext.Name = "tsbNext";
            this.tsbNext.Size = new System.Drawing.Size(18, 18);
            this.tsbNext.Text = "tsbNext";
            this.tsbNext.ToolTipText = "Navigate to Next";
            this.tsbNext.Click += new System.EventHandler(this.tsbNext_Click);
            // 
            // tsbLast
            // 
            this.tsbLast.AutoSize = false;
            this.tsbLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLast.Margin = new System.Windows.Forms.Padding(0);
            this.tsbLast.Name = "tsbLast";
            this.tsbLast.Size = new System.Drawing.Size(18, 18);
            this.tsbLast.Text = "tsbLast";
            this.tsbLast.ToolTipText = "Navigate to Last";
            this.tsbLast.Click += new System.EventHandler(this.tsbLast_Click);
            // 
            // g4
            // 
            this.g4.AllowEditing = false;
            this.g4.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g4.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g4.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g4.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g4.ContextMenuStrip = this.cmsg4g7g10;
            this.g4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g4.ExtendLastCol = true;
            this.g4.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g4.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g4.Location = new System.Drawing.Point(0, 0);
            this.g4.Name = "g4";
            this.g4.Rows.DefaultSize = 17;
            this.g4.Rows.Fixed = 0;
            this.g4.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g4.Size = new System.Drawing.Size(74, 588);
            this.g4.TabIndex = 1;
            this.g4.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g4_BeforeScroll);
            this.g4.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g4_OwnerDrawCell);
            this.g4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RowHeaderMouseDown);
            this.g4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
            this.g4.Resize += new System.EventHandler(this.g4_Resize);
            // 
            // cmsg4g7g10
            // 
            this.cmsg4g7g10.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiRowChooser,
            this.cmiVariableChooser,
            this.cmiQuantityChooser,
            this.cmig4g7g10Seperator1,
            this.cmiLockRow,
            this.cmiUnlockRow,
            this.cmiCascadeLockRow,
            this.cmiCascadeUnlockRow,
            this.cmig4g7g10Seperator2,
            this.cmiFreezeRow});
            this.cmsg4g7g10.Name = "cmsg4g7g10";
            this.cmsg4g7g10.Size = new System.Drawing.Size(185, 192);
            // 
            // cmiRowChooser
            // 
            this.cmiRowChooser.Name = "cmiRowChooser";
            this.cmiRowChooser.Size = new System.Drawing.Size(184, 22);
            this.cmiRowChooser.Text = "Row Chooser...";
            this.cmiRowChooser.Click += new System.EventHandler(this.cmiRowChooser_Click);
            // 
            // cmiVariableChooser
            // 
            this.cmiVariableChooser.Name = "cmiVariableChooser";
            this.cmiVariableChooser.Size = new System.Drawing.Size(184, 22);
            this.cmiVariableChooser.Text = "Variable Chooser...";
            this.cmiVariableChooser.Click += new System.EventHandler(this.cmiVariableChooser_Click);
            // 
            // cmiQuantityChooser
            // 
            this.cmiQuantityChooser.Name = "cmiQuantityChooser";
            this.cmiQuantityChooser.Size = new System.Drawing.Size(184, 22);
            this.cmiQuantityChooser.Text = "Quantity Chooser...";
            this.cmiQuantityChooser.Click += new System.EventHandler(this.cmiQuantityChooser_Click);
            // 
            // cmig4g7g10Seperator1
            // 
            this.cmig4g7g10Seperator1.Name = "cmig4g7g10Seperator1";
            this.cmig4g7g10Seperator1.Size = new System.Drawing.Size(181, 6);
            // 
            // cmiLockRow
            // 
            this.cmiLockRow.Name = "cmiLockRow";
            this.cmiLockRow.Size = new System.Drawing.Size(184, 22);
            this.cmiLockRow.Text = "Lock Entire Row";
            this.cmiLockRow.Click += new System.EventHandler(this.cmiLockRow_Click);
            // 
            // cmiUnlockRow
            // 
            this.cmiUnlockRow.Name = "cmiUnlockRow";
            this.cmiUnlockRow.Size = new System.Drawing.Size(184, 22);
            this.cmiUnlockRow.Text = "Unlock Entire Row";
            this.cmiUnlockRow.Click += new System.EventHandler(this.cmiUnlockRow_Click);
            // 
            // cmiCascadeLockRow
            // 
            this.cmiCascadeLockRow.Name = "cmiCascadeLockRow";
            this.cmiCascadeLockRow.Size = new System.Drawing.Size(184, 22);
            this.cmiCascadeLockRow.Text = "Cascade Lock Row";
            this.cmiCascadeLockRow.Click += new System.EventHandler(this.cmiCascadeLockRow_Click);
            // 
            // cmiCascadeUnlockRow
            // 
            this.cmiCascadeUnlockRow.Name = "cmiCascadeUnlockRow";
            this.cmiCascadeUnlockRow.Size = new System.Drawing.Size(184, 22);
            this.cmiCascadeUnlockRow.Text = "Cascade Unlock Row";
            this.cmiCascadeUnlockRow.Click += new System.EventHandler(this.cmiCascadeUnlockRow_Click);
            // 
            // cmig4g7g10Seperator2
            // 
            this.cmig4g7g10Seperator2.Name = "cmig4g7g10Seperator2";
            this.cmig4g7g10Seperator2.Size = new System.Drawing.Size(181, 6);
            // 
            // cmiFreezeRow
            // 
            this.cmiFreezeRow.Name = "cmiFreezeRow";
            this.cmiFreezeRow.Size = new System.Drawing.Size(184, 22);
            this.cmiFreezeRow.Text = "Freeze Row";
            this.cmiFreezeRow.Click += new System.EventHandler(this.cmiFreezeRow_Click);
            // 
            // g7
            // 
            this.g7.AllowEditing = false;
            this.g7.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g7.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g7.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g7.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g7.ContextMenuStrip = this.cmsg4g7g10;
            this.g7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g7.ExtendLastCol = true;
            this.g7.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g7.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g7.Location = new System.Drawing.Point(0, 0);
            this.g7.Name = "g7";
            this.g7.Rows.DefaultSize = 17;
            this.g7.Rows.Fixed = 0;
            this.g7.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g7.Size = new System.Drawing.Size(74, 7);
            this.g7.TabIndex = 2;
            this.g7.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g7_BeforeScroll);
            this.g7.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g7_OwnerDrawCell);
            this.g7.VisibleChanged += new System.EventHandler(this.g7_VisibleChanged);
            this.g7.Click += new System.EventHandler(this.g7_Click);
            this.g7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RowHeaderMouseDown);
            this.g7.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
            this.g7.Resize += new System.EventHandler(this.g7_Resize);
            // 
            // g10
            // 
            this.g10.AllowEditing = false;
            this.g10.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g10.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g10.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g10.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g10.ContextMenuStrip = this.cmsg4g7g10;
            this.g10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g10.ExtendLastCol = true;
            this.g10.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g10.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g10.Location = new System.Drawing.Point(0, 0);
            this.g10.Name = "g10";
            this.g10.Rows.DefaultSize = 17;
            this.g10.Rows.Fixed = 0;
            this.g10.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g10.Size = new System.Drawing.Size(74, 81);
            this.g10.TabIndex = 3;
            this.g10.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g10_BeforeScroll);
            this.g10.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g10_OwnerDrawCell);
            this.g10.VisibleChanged += new System.EventHandler(this.g10_VisibleChanged);
            this.g10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RowHeaderMouseDown);
            this.g10.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
            this.g10.Resize += new System.EventHandler(this.g10_Resize);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.Enabled = false;
            this.hScrollBar1.LargeChange = 1;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 758);
            this.hScrollBar1.Maximum = 0;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(76, 17);
            this.hScrollBar1.TabIndex = 4;
            // 
            // pnlScrollBars
            // 
            this.pnlScrollBars.Controls.Add(this.spcHScrollLevel1);
            this.pnlScrollBars.Controls.Add(this.pnlSpacer);
            this.pnlScrollBars.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlScrollBars.Location = new System.Drawing.Point(1137, 95);
            this.pnlScrollBars.Name = "pnlScrollBars";
            this.pnlScrollBars.Size = new System.Drawing.Size(17, 775);
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
            this.spcHScrollLevel1.Panel2.Controls.Add(this.vScrollBar4);
            this.spcHScrollLevel1.Panel2MinSize = 0;
            this.spcHScrollLevel1.Size = new System.Drawing.Size(17, 758);
            this.spcHScrollLevel1.SplitterDistance = 671;
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
            this.spcHScrollLevel2.ContextMenuStrip = this.cmsSplitter;
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
            this.spcHScrollLevel2.Panel2.Controls.Add(this.spcHScrollLevel3);
            this.spcHScrollLevel2.Panel2MinSize = 0;
            this.spcHScrollLevel2.Size = new System.Drawing.Size(17, 671);
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
            // spcHScrollLevel3
            // 
            this.spcHScrollLevel3.ContextMenuStrip = this.cmsSplitter;
            this.spcHScrollLevel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcHScrollLevel3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spcHScrollLevel3.Location = new System.Drawing.Point(0, 0);
            this.spcHScrollLevel3.Margin = new System.Windows.Forms.Padding(0);
            this.spcHScrollLevel3.Name = "spcHScrollLevel3";
            this.spcHScrollLevel3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcHScrollLevel3.Panel1
            // 
            this.spcHScrollLevel3.Panel1.Controls.Add(this.vScrollBar2);
            this.spcHScrollLevel3.Panel1MinSize = 0;
            // 
            // spcHScrollLevel3.Panel2
            // 
            this.spcHScrollLevel3.Panel2.Controls.Add(this.vScrollBar3);
            this.spcHScrollLevel3.Panel2MinSize = 0;
            this.spcHScrollLevel3.Size = new System.Drawing.Size(17, 603);
            this.spcHScrollLevel3.SplitterDistance = 590;
            this.spcHScrollLevel3.TabIndex = 0;
            this.spcHScrollLevel3.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel3_SplitterMoved);
            this.spcHScrollLevel3.DoubleClick += new System.EventHandler(this.spcHScrollLevel3_DoubleClick);
            this.spcHScrollLevel3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // vScrollBar2
            // 
            this.vScrollBar2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar2.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar2.Name = "vScrollBar2";
            this.vScrollBar2.Size = new System.Drawing.Size(17, 590);
            this.vScrollBar2.TabIndex = 1;
            this.vScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar2_Scroll);
            // 
            // vScrollBar3
            // 
            this.vScrollBar3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar3.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar3.Name = "vScrollBar3";
            this.vScrollBar3.Size = new System.Drawing.Size(17, 9);
            this.vScrollBar3.TabIndex = 2;
            this.vScrollBar3.Visible = false;
            this.vScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar3_Scroll);
            // 
            // vScrollBar4
            // 
            this.vScrollBar4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar4.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar4.Name = "vScrollBar4";
            this.vScrollBar4.Size = new System.Drawing.Size(17, 83);
            this.vScrollBar4.TabIndex = 3;
            this.vScrollBar4.Visible = false;
            this.vScrollBar4.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar4_Scroll);
            // 
            // pnlSpacer
            // 
            this.pnlSpacer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSpacer.Location = new System.Drawing.Point(0, 758);
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
            // g6
            // 
            this.g6.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g6.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g6.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g6.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g6.ContextMenuStrip = this.cmsCell;
            this.g6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g6.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g6.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g6.Location = new System.Drawing.Point(0, 0);
            this.g6.Name = "g6";
            this.g6.Rows.DefaultSize = 17;
            this.g6.Rows.Fixed = 0;
            this.g6.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g6.Size = new System.Drawing.Size(139, 588);
            this.g6.TabIndex = 1;
            this.g6.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g6_BeforeScroll);
            this.g6.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g6.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g6.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g6.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g6_OwnerDrawCell);
            this.g6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DetailMouseDown);
            // 
            // cmsCell
            // 
            this.cmsCell.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiLockCell,
            this.cmiCascadeLockCell,
            this.cmiCascadeUnlockCell,
            this.cmiBalance,
            this.cmiCopyLowToHigh,
            this.cmiBalanceLowLevels});
            this.cmsCell.Name = "cmsCell";
            this.cmsCell.Size = new System.Drawing.Size(176, 136);
            // 
            // cmiLockCell
            // 
            this.cmiLockCell.Name = "cmiLockCell";
            this.cmiLockCell.Size = new System.Drawing.Size(175, 22);
            this.cmiLockCell.Text = "Lock Cell";
            this.cmiLockCell.Click += new System.EventHandler(this.cmiLockCell_Click);
            // 
            // cmiCascadeLockCell
            // 
            this.cmiCascadeLockCell.Name = "cmiCascadeLockCell";
            this.cmiCascadeLockCell.Size = new System.Drawing.Size(175, 22);
            this.cmiCascadeLockCell.Text = "Cascade Lock";
            this.cmiCascadeLockCell.Click += new System.EventHandler(this.cmiCascadeLockCell_Click);
            // 
            // cmiCascadeUnlockCell
            // 
            this.cmiCascadeUnlockCell.Name = "cmiCascadeUnlockCell";
            this.cmiCascadeUnlockCell.Size = new System.Drawing.Size(175, 22);
            this.cmiCascadeUnlockCell.Text = "Cascade Unlock";
            this.cmiCascadeUnlockCell.Click += new System.EventHandler(this.cmiCascadeUnlockCell_Click);
            // 
            // cmiBalance
            // 
            this.cmiBalance.Name = "cmiBalance";
            this.cmiBalance.Size = new System.Drawing.Size(175, 22);
            this.cmiBalance.Text = "Balance";
            this.cmiBalance.Click += new System.EventHandler(this.cmiBalance_Click);
            // 
            // cmiCopyLowToHigh
            // 
            this.cmiCopyLowToHigh.Name = "cmiCopyLowToHigh";
            this.cmiCopyLowToHigh.Size = new System.Drawing.Size(175, 22);
            this.cmiCopyLowToHigh.Text = "Copy Low To High";
            this.cmiCopyLowToHigh.Click += new System.EventHandler(this.cmiCopyLowToHigh_Click);
            // 
            // cmiBalanceLowLevels
            // 
            this.cmiBalanceLowLevels.Name = "cmiBalanceLowLevels";
            this.cmiBalanceLowLevels.Size = new System.Drawing.Size(175, 22);
            this.cmiBalanceLowLevels.Text = "Balance Low Levels";
            this.cmiBalanceLowLevels.Click += new System.EventHandler(this.cmiBalanceLowLevels_Click);
            // 
            // g3
            // 
            this.g3.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None;
            this.g3.AllowEditing = false;
            this.g3.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g3.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g3.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g3.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g3.ContextMenuStrip = this.cmsg2g3;
            this.g3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g3.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g3.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g3.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g3.Location = new System.Drawing.Point(0, 0);
            this.g3.Name = "g3";
            this.g3.Rows.DefaultSize = 17;
            this.g3.Rows.Fixed = 0;
            this.g3.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g3.Size = new System.Drawing.Size(139, 62);
            this.g3.TabIndex = 0;
            this.g3.BeforeAutosizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_BeforeAutosizeColumn);
            this.g3.BeforeResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.BeforeResizeColumn);
            this.g3.AfterResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_AfterResizeColumn);
            this.g3.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g3_BeforeScroll);
            this.g3.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g3_OwnerDrawCell);
            this.g3.VisibleChanged += new System.EventHandler(this.g2_VisibleChanged);
            this.g3.DragDrop += new System.Windows.Forms.DragEventHandler(this.g3_DragDrop);
            this.g3.DragEnter += new System.Windows.Forms.DragEventHandler(this.g3_DragEnter);
            this.g3.DragOver += new System.Windows.Forms.DragEventHandler(this.g3_DragOver);
            this.g3.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.g3_QueryContinueDrag);
            this.g3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ColHeaderMouseDown);
            this.g3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.g3_MouseMove);
            this.g3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.g3_MouseUp);
            this.g3.Resize += new System.EventHandler(this.g3_Resize);
            // 
            // cmsg2g3
            // 
            this.cmsg2g3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiColumnChooser,
            this.cmig2g3Seperator1,
            this.cmiLockColumn,
            this.cmiUnlockColumn,
            this.cmiCascadeLockColumn,
            this.cmiCascadeUnlockColumn,
            this.cmig2g3Seperator2,
            this.cmiFreezeColumn,
            this.cmig2g3Seperator3,
            this.cmiShow});
            this.cmsg2g3.Name = "cmsg2g3";
            this.cmsg2g3.Size = new System.Drawing.Size(205, 176);
            this.cmsg2g3.Opening += new System.ComponentModel.CancelEventHandler(this.cmsg2g3_Opening);
            // 
            // cmiColumnChooser
            // 
            this.cmiColumnChooser.Name = "cmiColumnChooser";
            this.cmiColumnChooser.Size = new System.Drawing.Size(204, 22);
            this.cmiColumnChooser.Text = "Column Chooser...";
            this.cmiColumnChooser.Click += new System.EventHandler(this.cmiColumnChooser_Click);
            // 
            // cmig2g3Seperator1
            // 
            this.cmig2g3Seperator1.Name = "cmig2g3Seperator1";
            this.cmig2g3Seperator1.Size = new System.Drawing.Size(201, 6);
            // 
            // cmiLockColumn
            // 
            this.cmiLockColumn.Name = "cmiLockColumn";
            this.cmiLockColumn.Size = new System.Drawing.Size(204, 22);
            this.cmiLockColumn.Text = "Lock Entire Column";
            this.cmiLockColumn.Click += new System.EventHandler(this.cmiLockColumn_Click);
            // 
            // cmiUnlockColumn
            // 
            this.cmiUnlockColumn.Name = "cmiUnlockColumn";
            this.cmiUnlockColumn.Size = new System.Drawing.Size(204, 22);
            this.cmiUnlockColumn.Text = "Unlock Entire Column";
            this.cmiUnlockColumn.Click += new System.EventHandler(this.cmiUnlockColumn_Click);
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
            // cmig2g3Seperator2
            // 
            this.cmig2g3Seperator2.Name = "cmig2g3Seperator2";
            this.cmig2g3Seperator2.Size = new System.Drawing.Size(201, 6);
            // 
            // cmiFreezeColumn
            // 
            this.cmiFreezeColumn.Name = "cmiFreezeColumn";
            this.cmiFreezeColumn.Size = new System.Drawing.Size(204, 22);
            this.cmiFreezeColumn.Text = "Freeze Column";
            this.cmiFreezeColumn.Click += new System.EventHandler(this.cmiFreezeColumn_Click);
            // 
            // cmig2g3Seperator3
            // 
            this.cmig2g3Seperator3.Name = "cmig2g3Seperator3";
            this.cmig2g3Seperator3.Size = new System.Drawing.Size(201, 6);
            // 
            // cmiShow
            // 
            this.cmiShow.Name = "cmiShow";
            this.cmiShow.Size = new System.Drawing.Size(204, 22);
            this.cmiShow.Text = "Show...";
            this.cmiShow.Click += new System.EventHandler(this.cmiShow_Click);
            // 
            // g9
            // 
            this.g9.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g9.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g9.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g9.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g9.ContextMenuStrip = this.cmsCell;
            this.g9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g9.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g9.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g9.Location = new System.Drawing.Point(0, 0);
            this.g9.Name = "g9";
            this.g9.Rows.DefaultSize = 17;
            this.g9.Rows.Fixed = 0;
            this.g9.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g9.Size = new System.Drawing.Size(139, 7);
            this.g9.TabIndex = 2;
            this.g9.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g9_BeforeScroll);
            this.g9.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g9.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g9.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g9.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g9_OwnerDrawCell);
            this.g9.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DetailMouseDown);
            // 
            // g12
            // 
            this.g12.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g12.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g12.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g12.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g12.ContextMenuStrip = this.cmsCell;
            this.g12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g12.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g12.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g12.Location = new System.Drawing.Point(0, 0);
            this.g12.Name = "g12";
            this.g12.Rows.DefaultSize = 17;
            this.g12.Rows.Fixed = 0;
            this.g12.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g12.Size = new System.Drawing.Size(139, 81);
            this.g12.TabIndex = 3;
            this.g12.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g12_BeforeScroll);
            this.g12.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g12.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g12.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g12.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g12_OwnerDrawCell);
            this.g12.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g12.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DetailMouseDown);
            // 
            // hScrollBar2
            // 
            this.hScrollBar2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar2.Location = new System.Drawing.Point(0, 758);
            this.hScrollBar2.Name = "hScrollBar2";
            this.hScrollBar2.Size = new System.Drawing.Size(904, 17);
            this.hScrollBar2.TabIndex = 4;
            this.hScrollBar2.Visible = false;
            this.hScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
            // 
            // g5
            // 
            this.g5.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g5.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g5.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g5.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g5.ContextMenuStrip = this.cmsCell;
            this.g5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g5.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g5.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g5.Location = new System.Drawing.Point(0, 0);
            this.g5.Name = "g5";
            this.g5.Rows.DefaultSize = 17;
            this.g5.Rows.Fixed = 0;
            this.g5.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g5.Size = new System.Drawing.Size(902, 588);
            this.g5.TabIndex = 1;
            this.g5.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g5_BeforeScroll);
            this.g5.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g5.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g5.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g5.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g5_OwnerDrawCell);
            this.g5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DetailMouseDown);
            // 
            // g2
            // 
            this.g2.AllowEditing = false;
            this.g2.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g2.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g2.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g2.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g2.ContextMenuStrip = this.cmsg2g3;
            this.g2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g2.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g2.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g2.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g2.Location = new System.Drawing.Point(0, 0);
            this.g2.Name = "g2";
            this.g2.Rows.DefaultSize = 17;
            this.g2.Rows.Fixed = 0;
            this.g2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g2.Size = new System.Drawing.Size(902, 62);
            this.g2.TabIndex = 0;
            this.g2.BeforeAutosizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_BeforeAutosizeColumn);
            this.g2.BeforeResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.BeforeResizeColumn);
            this.g2.AfterResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_AfterResizeColumn);
            this.g2.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g2_BeforeScroll);
            this.g2.DragDrop += new System.Windows.Forms.DragEventHandler(this.g2_DragDrop);
            this.g2.DragEnter += new System.Windows.Forms.DragEventHandler(this.g2_DragEnter);
            this.g2.DragOver += new System.Windows.Forms.DragEventHandler(this.g2_DragOver);
            this.g2.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.g2_QueryContinueDrag);
            this.g2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ColHeaderMouseDown);
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
            this.g8.ContextMenuStrip = this.cmsCell;
            this.g8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g8.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g8.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g8.Location = new System.Drawing.Point(0, 0);
            this.g8.Name = "g8";
            this.g8.Rows.DefaultSize = 17;
            this.g8.Rows.Fixed = 0;
            this.g8.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g8.Size = new System.Drawing.Size(902, 7);
            this.g8.TabIndex = 2;
            this.g8.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g8_BeforeScroll);
            this.g8.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g8.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g8.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g8.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g8_OwnerDrawCell);
            this.g8.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DetailMouseDown);
            // 
            // g11
            // 
            this.g11.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g11.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g11.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None;
            this.g11.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g11.ContextMenuStrip = this.cmsCell;
            this.g11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g11.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g11.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross;
            this.g11.Location = new System.Drawing.Point(0, 0);
            this.g11.Name = "g11";
            this.g11.Rows.DefaultSize = 17;
            this.g11.Rows.Fixed = 0;
            this.g11.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g11.Size = new System.Drawing.Size(902, 81);
            this.g11.TabIndex = 3;
            this.g11.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g11_BeforeScroll);
            this.g11.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g11.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g11.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g11.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g11_OwnerDrawCell);
            this.g11.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g11.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DetailMouseDown);
            // 
            // hScrollBar3
            // 
            this.hScrollBar3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar3.Location = new System.Drawing.Point(0, 758);
            this.hScrollBar3.Name = "hScrollBar3";
            this.hScrollBar3.Size = new System.Drawing.Size(141, 17);
            this.hScrollBar3.TabIndex = 4;
            this.hScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar3_Scroll);
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
            this.spcHDetailLevel1.Panel2.Controls.Add(this.g11);
            this.spcHDetailLevel1.Panel2MinSize = 0;
            this.spcHDetailLevel1.Size = new System.Drawing.Size(904, 758);
            this.spcHDetailLevel1.SplitterDistance = 671;
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
            this.spcHDetailLevel2.Panel1.Controls.Add(this.g2);
            this.spcHDetailLevel2.Panel1MinSize = 0;
            // 
            // spcHDetailLevel2.Panel2
            // 
            this.spcHDetailLevel2.Panel2.Controls.Add(this.spcHDetailLevel3);
            this.spcHDetailLevel2.Panel2MinSize = 0;
            this.spcHDetailLevel2.Size = new System.Drawing.Size(904, 671);
            this.spcHDetailLevel2.SplitterDistance = 64;
            this.spcHDetailLevel2.TabIndex = 0;
            this.spcHDetailLevel2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
            this.spcHDetailLevel2.DoubleClick += new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
            this.spcHDetailLevel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // spcHDetailLevel3
            // 
            this.spcHDetailLevel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spcHDetailLevel3.ContextMenuStrip = this.cmsSplitter;
            this.spcHDetailLevel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcHDetailLevel3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spcHDetailLevel3.Location = new System.Drawing.Point(0, 0);
            this.spcHDetailLevel3.Margin = new System.Windows.Forms.Padding(0);
            this.spcHDetailLevel3.Name = "spcHDetailLevel3";
            this.spcHDetailLevel3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcHDetailLevel3.Panel1
            // 
            this.spcHDetailLevel3.Panel1.Controls.Add(this.g5);
            this.spcHDetailLevel3.Panel1MinSize = 0;
            // 
            // spcHDetailLevel3.Panel2
            // 
            this.spcHDetailLevel3.Panel2.Controls.Add(this.g8);
            this.spcHDetailLevel3.Panel2MinSize = 0;
            this.spcHDetailLevel3.Size = new System.Drawing.Size(904, 603);
            this.spcHDetailLevel3.SplitterDistance = 590;
            this.spcHDetailLevel3.TabIndex = 0;
            this.spcHDetailLevel3.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel3_SplitterMoved);
            this.spcHDetailLevel3.DoubleClick += new System.EventHandler(this.spcHScrollLevel3_DoubleClick);
            this.spcHDetailLevel3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
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
            this.spcHHeaderLevel1.Panel2.Controls.Add(this.g10);
            this.spcHHeaderLevel1.Panel2MinSize = 0;
            this.spcHHeaderLevel1.Size = new System.Drawing.Size(76, 758);
            this.spcHHeaderLevel1.SplitterDistance = 671;
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
            this.spcHHeaderLevel2.Panel1.ContextMenuStrip = this.cmsg1;
            this.spcHHeaderLevel2.Panel1MinSize = 0;
            // 
            // spcHHeaderLevel2.Panel2
            // 
            this.spcHHeaderLevel2.Panel2.Controls.Add(this.spcHHeaderLevel3);
            this.spcHHeaderLevel2.Panel2MinSize = 0;
            this.spcHHeaderLevel2.Size = new System.Drawing.Size(76, 671);
            this.spcHHeaderLevel2.SplitterDistance = 64;
            this.spcHHeaderLevel2.TabIndex = 0;
            this.spcHHeaderLevel2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
            this.spcHHeaderLevel2.DoubleClick += new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
            this.spcHHeaderLevel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // cmsg1
            // 
            this.cmsg1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiLockSheet,
            this.cmiUnlockSheet,
            this.cmiCascadeLockSheet,
            this.cmiCascadeUnlockSheet});
            this.cmsg1.Name = "cmsg1";
            this.cmsg1.Size = new System.Drawing.Size(191, 92);
            // 
            // cmiLockSheet
            // 
            this.cmiLockSheet.Name = "cmiLockSheet";
            this.cmiLockSheet.Size = new System.Drawing.Size(190, 22);
            this.cmiLockSheet.Text = "Lock Entire Sheet";
            this.cmiLockSheet.Click += new System.EventHandler(this.cmiLockSheet_Click);
            // 
            // cmiUnlockSheet
            // 
            this.cmiUnlockSheet.Name = "cmiUnlockSheet";
            this.cmiUnlockSheet.Size = new System.Drawing.Size(190, 22);
            this.cmiUnlockSheet.Text = "Unlock Entire Sheet";
            this.cmiUnlockSheet.Click += new System.EventHandler(this.cmiUnlockSheet_Click);
            // 
            // cmiCascadeLockSheet
            // 
            this.cmiCascadeLockSheet.Name = "cmiCascadeLockSheet";
            this.cmiCascadeLockSheet.Size = new System.Drawing.Size(190, 22);
            this.cmiCascadeLockSheet.Text = "Cascade Lock Sheet";
            this.cmiCascadeLockSheet.Click += new System.EventHandler(this.cmiCascadeLockSheet_Click);
            // 
            // cmiCascadeUnlockSheet
            // 
            this.cmiCascadeUnlockSheet.Name = "cmiCascadeUnlockSheet";
            this.cmiCascadeUnlockSheet.Size = new System.Drawing.Size(190, 22);
            this.cmiCascadeUnlockSheet.Text = "Cascade Unlock Sheet";
            this.cmiCascadeUnlockSheet.Click += new System.EventHandler(this.cmiCascadeUnlockSheet_Click);
            // 
            // spcHHeaderLevel3
            // 
            this.spcHHeaderLevel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spcHHeaderLevel3.ContextMenuStrip = this.cmsSplitter;
            this.spcHHeaderLevel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcHHeaderLevel3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spcHHeaderLevel3.Location = new System.Drawing.Point(0, 0);
            this.spcHHeaderLevel3.Margin = new System.Windows.Forms.Padding(0);
            this.spcHHeaderLevel3.Name = "spcHHeaderLevel3";
            this.spcHHeaderLevel3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcHHeaderLevel3.Panel1
            // 
            this.spcHHeaderLevel3.Panel1.Controls.Add(this.g4);
            this.spcHHeaderLevel3.Panel1MinSize = 0;
            // 
            // spcHHeaderLevel3.Panel2
            // 
            this.spcHHeaderLevel3.Panel2.Controls.Add(this.g7);
            this.spcHHeaderLevel3.Panel2MinSize = 0;
            this.spcHHeaderLevel3.Size = new System.Drawing.Size(76, 603);
            this.spcHHeaderLevel3.SplitterDistance = 590;
            this.spcHHeaderLevel3.TabIndex = 0;
            this.spcHHeaderLevel3.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel3_SplitterMoved);
            this.spcHHeaderLevel3.DoubleClick += new System.EventHandler(this.spcHScrollLevel3_DoubleClick);
            this.spcHHeaderLevel3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
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
            this.spcHTotalLevel1.Panel2.Controls.Add(this.g12);
            this.spcHTotalLevel1.Panel2MinSize = 0;
            this.spcHTotalLevel1.Size = new System.Drawing.Size(141, 758);
            this.spcHTotalLevel1.SplitterDistance = 671;
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
            this.spcHTotalLevel2.Panel1.Controls.Add(this.g3);
            this.spcHTotalLevel2.Panel1MinSize = 0;
            // 
            // spcHTotalLevel2.Panel2
            // 
            this.spcHTotalLevel2.Panel2.Controls.Add(this.spcHTotalLevel3);
            this.spcHTotalLevel2.Panel2MinSize = 0;
            this.spcHTotalLevel2.Size = new System.Drawing.Size(141, 671);
            this.spcHTotalLevel2.SplitterDistance = 64;
            this.spcHTotalLevel2.TabIndex = 0;
            this.spcHTotalLevel2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel2_SplitterMoved);
            this.spcHTotalLevel2.DoubleClick += new System.EventHandler(this.spcHScrollLevel2_DoubleClick);
            this.spcHTotalLevel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // spcHTotalLevel3
            // 
            this.spcHTotalLevel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spcHTotalLevel3.ContextMenuStrip = this.cmsSplitter;
            this.spcHTotalLevel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcHTotalLevel3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spcHTotalLevel3.Location = new System.Drawing.Point(0, 0);
            this.spcHTotalLevel3.Margin = new System.Windows.Forms.Padding(0);
            this.spcHTotalLevel3.Name = "spcHTotalLevel3";
            this.spcHTotalLevel3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcHTotalLevel3.Panel1
            // 
            this.spcHTotalLevel3.Panel1.Controls.Add(this.g6);
            this.spcHTotalLevel3.Panel1MinSize = 0;
            // 
            // spcHTotalLevel3.Panel2
            // 
            this.spcHTotalLevel3.Panel2.Controls.Add(this.g9);
            this.spcHTotalLevel3.Panel2MinSize = 0;
            this.spcHTotalLevel3.Size = new System.Drawing.Size(141, 603);
            this.spcHTotalLevel3.SplitterDistance = 590;
            this.spcHTotalLevel3.TabIndex = 0;
            this.spcHTotalLevel3.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcHScrollLevel3_SplitterMoved);
            this.spcHTotalLevel3.DoubleClick += new System.EventHandler(this.spcHScrollLevel3_DoubleClick);
            this.spcHTotalLevel3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // spcVLevel1
            // 
            this.spcVLevel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spcVLevel1.ContextMenuStrip = this.cmsSplitter;
            this.spcVLevel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcVLevel1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spcVLevel1.Location = new System.Drawing.Point(8, 95);
            this.spcVLevel1.Margin = new System.Windows.Forms.Padding(0);
            this.spcVLevel1.Name = "spcVLevel1";
            // 
            // spcVLevel1.Panel1
            // 
            this.spcVLevel1.Panel1.Controls.Add(this.spcHHeaderLevel1);
            this.spcVLevel1.Panel1.Controls.Add(this.hScrollBar1);
            this.spcVLevel1.Panel1MinSize = 0;
            // 
            // spcVLevel1.Panel2
            // 
            this.spcVLevel1.Panel2.Controls.Add(this.spcVLevel2);
            this.spcVLevel1.Panel2MinSize = 0;
            this.spcVLevel1.Size = new System.Drawing.Size(1129, 775);
            this.spcVLevel1.SplitterDistance = 76;
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
            this.spcVLevel2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spcVLevel2.Location = new System.Drawing.Point(0, 0);
            this.spcVLevel2.Margin = new System.Windows.Forms.Padding(0);
            this.spcVLevel2.Name = "spcVLevel2";
            // 
            // spcVLevel2.Panel1
            // 
            this.spcVLevel2.Panel1.Controls.Add(this.spcHTotalLevel1);
            this.spcVLevel2.Panel1.Controls.Add(this.hScrollBar3);
            this.spcVLevel2.Panel1MinSize = 0;
            // 
            // spcVLevel2.Panel2
            // 
            this.spcVLevel2.Panel2.Controls.Add(this.spcHDetailLevel1);
            this.spcVLevel2.Panel2.Controls.Add(this.hScrollBar2);
            this.spcVLevel2.Panel2MinSize = 0;
            this.spcVLevel2.Size = new System.Drawing.Size(1049, 775);
            this.spcVLevel2.SplitterDistance = 141;
            this.spcVLevel2.TabIndex = 0;
            this.spcVLevel2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spcVLevel2_SplitterMoved);
            this.spcVLevel2.DoubleClick += new System.EventHandler(this.spcVLevel2_DoubleClick);
            this.spcVLevel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplitterMouseDown);
            // 
            // cmsNavigate
            // 
            this.cmsNavigate.Name = "cmsNavigate";
            this.cmsNavigate.Size = new System.Drawing.Size(61, 4);
            // 
            // PlanViewRT
            // 
            this.AllowDragDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 916);
            this.Controls.Add(this.spcVLevel1);
            this.Controls.Add(this.pnlScrollBars);
            this.Controls.Add(this.rtbScrollText);
            this.Controls.Add(this.lblFindSize);
            this.Controls.Add(this.pnlTop);
            this.Name = "PlanViewRT";
            this.Text = "PlanView";
            this.Activated += new System.EventHandler(this.PlanView_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.PlanView_Closing);
            this.Load += new System.EventHandler(this.PlanView_Load);
            this.Controls.SetChildIndex(this.pnlTop, 0);
            this.Controls.SetChildIndex(this.lblFindSize, 0);
            this.Controls.SetChildIndex(this.rtbScrollText, 0);
            this.Controls.SetChildIndex(this.pnlScrollBars, 0);
            this.Controls.SetChildIndex(this.spcVLevel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlControls.ResumeLayout(false);
            this.pnlGroupBy.ResumeLayout(false);
            this.pnlNavigate.ResumeLayout(false);
            this.pnlNavigate.PerformLayout();
            this.tspNavigate.ResumeLayout(false);
            this.tspNavigate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g4)).EndInit();
            this.cmsg4g7g10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g10)).EndInit();
            this.pnlScrollBars.ResumeLayout(false);
            this.spcHScrollLevel1.Panel1.ResumeLayout(false);
            this.spcHScrollLevel1.Panel2.ResumeLayout(false);
            this.spcHScrollLevel1.ResumeLayout(false);
            this.cmsSplitter.ResumeLayout(false);
            this.spcHScrollLevel2.Panel1.ResumeLayout(false);
            this.spcHScrollLevel2.Panel2.ResumeLayout(false);
            this.spcHScrollLevel2.ResumeLayout(false);
            this.spcHScrollLevel3.Panel1.ResumeLayout(false);
            this.spcHScrollLevel3.Panel2.ResumeLayout(false);
            this.spcHScrollLevel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g6)).EndInit();
            this.cmsCell.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g3)).EndInit();
            this.cmsg2g3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g11)).EndInit();
            this.spcHDetailLevel1.Panel1.ResumeLayout(false);
            this.spcHDetailLevel1.Panel2.ResumeLayout(false);
            this.spcHDetailLevel1.ResumeLayout(false);
            this.spcHDetailLevel2.Panel1.ResumeLayout(false);
            this.spcHDetailLevel2.Panel2.ResumeLayout(false);
            this.spcHDetailLevel2.ResumeLayout(false);
            this.spcHDetailLevel3.Panel1.ResumeLayout(false);
            this.spcHDetailLevel3.Panel2.ResumeLayout(false);
            this.spcHDetailLevel3.ResumeLayout(false);
            this.spcHHeaderLevel1.Panel1.ResumeLayout(false);
            this.spcHHeaderLevel1.Panel2.ResumeLayout(false);
            this.spcHHeaderLevel1.ResumeLayout(false);
            this.spcHHeaderLevel2.Panel2.ResumeLayout(false);
            this.spcHHeaderLevel2.ResumeLayout(false);
            this.cmsg1.ResumeLayout(false);
            this.spcHHeaderLevel3.Panel1.ResumeLayout(false);
            this.spcHHeaderLevel3.Panel2.ResumeLayout(false);
            this.spcHHeaderLevel3.ResumeLayout(false);
            this.spcHTotalLevel1.Panel1.ResumeLayout(false);
            this.spcHTotalLevel1.Panel2.ResumeLayout(false);
            this.spcHTotalLevel1.ResumeLayout(false);
            this.spcHTotalLevel2.Panel1.ResumeLayout(false);
            this.spcHTotalLevel2.Panel2.ResumeLayout(false);
            this.spcHTotalLevel2.ResumeLayout(false);
            this.spcHTotalLevel3.Panel1.ResumeLayout(false);
            this.spcHTotalLevel3.Panel2.ResumeLayout(false);
            this.spcHTotalLevel3.ResumeLayout(false);
            this.spcVLevel1.Panel1.ResumeLayout(false);
            this.spcVLevel1.Panel2.ResumeLayout(false);
            this.spcVLevel1.ResumeLayout(false);
            this.spcVLevel2.Panel1.ResumeLayout(false);
            this.spcVLevel2.Panel2.ResumeLayout(false);
            this.spcVLevel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        

        // Begin Track #4872 - JSmith - Global/User Attributes
        private MIDAttributeComboBox cboStoreAttribute;
        // End Track #4872
		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.HScrollBar hScrollBar1;
		private System.Windows.Forms.Panel pnlScrollBars;
		private System.Windows.Forms.HScrollBar hScrollBar2;
		private System.Windows.Forms.HScrollBar hScrollBar3;
		private C1.Win.C1FlexGrid.C1FlexGrid g4;
		private C1.Win.C1FlexGrid.C1FlexGrid g7;
		private C1.Win.C1FlexGrid.C1FlexGrid g10;
		private System.Windows.Forms.VScrollBar vScrollBar2;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		private System.Windows.Forms.VScrollBar vScrollBar3;
		private System.Windows.Forms.VScrollBar vScrollBar4;
		private System.Windows.Forms.Panel pnlSpacer;
		private C1.Win.C1FlexGrid.C1FlexGrid g6;
		private C1.Win.C1FlexGrid.C1FlexGrid g3;
		private C1.Win.C1FlexGrid.C1FlexGrid g9;
		private C1.Win.C1FlexGrid.C1FlexGrid g12;
		private C1.Win.C1FlexGrid.C1FlexGrid g5;
		private C1.Win.C1FlexGrid.C1FlexGrid g2;
		private C1.Win.C1FlexGrid.C1FlexGrid g8;
		private C1.Win.C1FlexGrid.C1FlexGrid g11;
		private System.Windows.Forms.ToolTip toolTip1;
        // Begin TT#301-MD - JSmith - Controls are not functioning properly
		//private MIDRetail.Windows.Controls.MIDComboBoxEnh cboAttributeSet;
		private MIDComboBoxEnh cboAttributeSet;
        // End TT#301-MD - JSmith - Controls are not functioning properly
		private System.Windows.Forms.Button btnApply;
        // Begin TT#301-MD - JSmith - Controls are not functioning properly
		//private MIDRetail.Windows.Controls.MIDComboBoxEnh cboView;
		//private MIDRetail.Windows.Controls.MIDComboBoxEnh cboFilter;
		private MIDComboBoxEnh cboView;
		private MIDComboBoxEnh cboFilter;
        // End TT#301-MD - JSmith - Controls are not functioning properly
		private System.Windows.Forms.RichTextBox rtbScrollText;
		private System.Windows.Forms.Label lblFindSize;
		private System.Windows.Forms.Panel pnlControls;
		private System.Windows.Forms.Panel pnlSpacer1;
        // Begin TT#301-MD - JSmith - Controls are not functioning properly
		//private MIDRetail.Windows.Controls.MIDComboBoxEnh cboDollarScaling;
		//private MIDRetail.Windows.Controls.MIDComboBoxEnh cboUnitScaling;
		private MIDComboBoxEnh cboDollarScaling;
		private MIDComboBoxEnh cboUnitScaling;
        // End TT#301-MD - JSmith - Controls are not functioning properly
		private System.Windows.Forms.RadioButton optGroupByVariable;
		private System.Windows.Forms.RadioButton optGroupByTime;
		private System.Windows.Forms.Panel pnlGroupBy;
		private System.Windows.Forms.Panel pnlSpacer2;
		private System.Windows.Forms.SplitContainer spcHDetailLevel1;
		private System.Windows.Forms.SplitContainer spcHDetailLevel2;
		private System.Windows.Forms.SplitContainer spcHDetailLevel3;
		private System.Windows.Forms.SplitContainer spcHHeaderLevel1;
		private System.Windows.Forms.SplitContainer spcHHeaderLevel2;
		private System.Windows.Forms.SplitContainer spcHHeaderLevel3;
		private System.Windows.Forms.SplitContainer spcHTotalLevel3;
		private System.Windows.Forms.SplitContainer spcHTotalLevel1;
		private System.Windows.Forms.SplitContainer spcHTotalLevel2;
		private System.Windows.Forms.SplitContainer spcVLevel1;
		private System.Windows.Forms.SplitContainer spcVLevel2;
		private System.Windows.Forms.SplitContainer spcHScrollLevel1;
		private System.Windows.Forms.SplitContainer spcHScrollLevel3;
		private System.Windows.Forms.SplitContainer spcHScrollLevel2;
		private System.Windows.Forms.ContextMenuStrip cmsSplitter;
		private System.Windows.Forms.ToolStripMenuItem cmiLockSplitter;
		private System.Windows.Forms.ContextMenuStrip cmsg4g7g10;
		private System.Windows.Forms.ToolStripMenuItem cmiRowChooser;
		private System.Windows.Forms.ToolStripMenuItem cmiVariableChooser;
		private System.Windows.Forms.ToolStripMenuItem cmiQuantityChooser;
		private System.Windows.Forms.ToolStripSeparator cmig4g7g10Seperator1;
		private System.Windows.Forms.ToolStripMenuItem cmiLockRow;
		private System.Windows.Forms.ToolStripMenuItem cmiUnlockRow;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeLockRow;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeUnlockRow;
		private System.Windows.Forms.ContextMenuStrip cmsg1;
		private System.Windows.Forms.ToolStripMenuItem cmiLockSheet;
		private System.Windows.Forms.ToolStripMenuItem cmiUnlockSheet;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeLockSheet;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeUnlockSheet;
		private System.Windows.Forms.ContextMenuStrip cmsCell;
		private System.Windows.Forms.ToolStripMenuItem cmiLockCell;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeLockCell;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeUnlockCell;
		private System.Windows.Forms.ToolStripMenuItem cmiBalance;
		private System.Windows.Forms.ToolStripMenuItem cmiCopyLowToHigh;
		private System.Windows.Forms.ContextMenuStrip cmsg2g3;
		private System.Windows.Forms.ToolStripMenuItem cmiColumnChooser;
		private System.Windows.Forms.ToolStripSeparator cmig2g3Seperator1;
		private System.Windows.Forms.ToolStripMenuItem cmiLockColumn;
		private System.Windows.Forms.ToolStripMenuItem cmiUnlockColumn;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeLockColumn;
		private System.Windows.Forms.ToolStripMenuItem cmiCascadeUnlockColumn;
		private System.Windows.Forms.ToolStripSeparator cmig2g3Seperator2;
		private System.Windows.Forms.ToolStripMenuItem cmiFreezeColumn;
		private System.Windows.Forms.ToolStripSeparator cmig2g3Seperator3;
		private System.Windows.Forms.ToolStripMenuItem cmiShow;
		private System.Windows.Forms.ToolStripMenuItem cmiBalanceLowLevels;

		#endregion
		private System.Windows.Forms.ToolStripSeparator cmig4g7g10Seperator2;
		private System.Windows.Forms.ToolStripMenuItem cmiFreezeRow;
		private System.Windows.Forms.ToolStrip tspNavigate;
		private System.Windows.Forms.ToolStripButton tsbNavigate;
		private System.Windows.Forms.ToolStripButton tsbFirst;
		private System.Windows.Forms.Panel pnlSpacer3;
		private System.Windows.Forms.ToolStripButton tsbPrevious;
		private System.Windows.Forms.ToolStripButton tsbNext;
		private System.Windows.Forms.ToolStripButton tsbLast;
		private System.Windows.Forms.Panel pnlNavigate;
		private System.Windows.Forms.ContextMenuStrip cmsNavigate;
	}
}
