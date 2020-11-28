using MIDRetail.Windows.Controls;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;

namespace MIDRetail.Windows
{
	partial class frmNodeProperties
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
                if (tabNodeProperties.SelectedTab.Name == tabStoreEligibility.Name)
                {
                    cbSEStoreAttribute.Focus();
                }

                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                //End TT#169

                this.tabNodeProperties.DragOver -= new System.Windows.Forms.DragEventHandler(this.tabNodeProperties_DragOver);
                this.tabNodeProperties.Click -= new System.EventHandler(this.tabNodeProperties_Click);
                this.tabNodeProperties.DragEnter -= new System.Windows.Forms.DragEventHandler(this.tabNodeProperties_DragEnter);
                this.tabNodeProperties.SelectedIndexChanged -= new System.EventHandler(this.tabNodeProperties_SelectedIndexChanged);
                this.radBasicReplenishmentNo.CheckedChanged -= new System.EventHandler(this.radBasicReplenishmentNo_CheckedChanged);
                this.radBasicReplenishmentYes.CheckedChanged -= new System.EventHandler(this.radBasicReplenishmentYes_CheckedChanged);
                this.radOTSNode.CheckedChanged -= new System.EventHandler(this.radOTSNode_CheckedChanged);
                this.radOTSLevel.CheckedChanged -= new System.EventHandler(this.radOTSLevel_CheckedChanged);
                this.radOTSContainsID.CheckedChanged -= new System.EventHandler(this.radOTSContainsID_CheckedChanged);
                this.radOTSContainsName.CheckedChanged -= new System.EventHandler(this.radOTSContainsName_CheckedChanged);
                this.radOTSContainsDescription.CheckedChanged -= new System.EventHandler(this.radOTSContainsDescription_CheckedChanged);
                this.txtOTSLevelString.TextChanged -= new System.EventHandler(this.txtOTSLevelString_TextChanged);
                this.cboOTSLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboOTSLevel_SelectionChangeCommitted);
                this.txtOTSAnchorNode.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtOTSAnchorNode_DragDrop);
                this.txtOTSAnchorNode.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.txtOTSAnchorNode_KeyDown);
                this.txtOTSAnchorNode.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtOTSAnchorNode_KeyPress);
                this.txtOTSAnchorNode.Validating -= new System.ComponentModel.CancelEventHandler(this.txtOTSAnchorNode_Validating);
                this.txtOTSAnchorNode.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtOTSAnchorNode_DragEnter);
                this.txtOTSAnchorNode.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtOTSAnchorNode_DragOver);
                this.chkOTSTypeOverrideTotal.CheckedChanged -= new System.EventHandler(this.cbxOTSTypeOverrideTotal_CheckedChanged);
                this.chkOTSTypeOverrideRegular.CheckedChanged -= new System.EventHandler(this.cbxOTSTypeOverrideRegular_CheckedChanged);
                this.radTypeSoftline.CheckedChanged -= new System.EventHandler(this.radTypeSoftline_CheckedChanged);
                this.radTypeHardline.CheckedChanged -= new System.EventHandler(this.radTypeHardline_CheckedChanged);
                this.radTypeUndefined.CheckedChanged -= new System.EventHandler(this.radTypeUndefined_CheckedChanged);
                this.txtNodeID.TextChanged -= new System.EventHandler(this.txtNodeID_TextChanged);
                this.txtNodeID.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.txtNodeID_KeyDown);
                this.txtNodeID.Leave -= new System.EventHandler(this.txtNodeID_Leave);
                this.txtDescription.TextChanged -= new System.EventHandler(this.txtDescription_TextChanged);
                //this.cbxSEInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxSEInheritFromHigherLevel_CheckedChanged); 
                this.uddFWOSModifier.RowSelected -= new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.uddFWOSModifier_RowSelected);
                this.uddFWOSMax.RowSelected -= new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.uddFWOSMax_RowSelected); //TT#108 - MD - DOConnell - FWOS Max Model
                //this.cbxSEApplyToLowerLevels.CheckedChanged -= new System.EventHandler(this.cbxSEApplyToLowerLevels_CheckedChanged);
                this.cbSEStoreAttribute.DragOver -= new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragOver);
                this.cbSEStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cbSEStoreAttribute_SelectionChangeCommitted);
                this.cbSEStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cbSEStoreAttribute_DragDrop);
                this.cbSEStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragEnter);
                this.uddSalesModifier.RowSelected -= new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.uddSalesModifier_RowSelected);
                this.uddStockModifier.RowSelected -= new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.uddStockModifier_RowSelected);
                this.uddEligModel.RowSelected -= new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.uddEligModel_RowSelected);
                this.ugStoreEligibility.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreEligibility_ClickCellButton);
                this.ugStoreEligibility.BeforeCellListDropDown -= new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugStoreEligibility_BeforeCellListDropDown);
                this.ugStoreEligibility.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugStoreEligibility_MouseDown);
                this.ugStoreEligibility.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugStoreEligibility_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugStoreEligibility);
                //End TT#169
                this.ugStoreEligibility.DragOver -= new System.Windows.Forms.DragEventHandler(this.ugGrid_DragOver);
                this.ugStoreEligibility.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugStoreEligibility_BeforeExitEditMode);
                this.ugStoreEligibility.AfterCellListCloseUp -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreEligibility_AfterCellListCloseUp);
                this.ugStoreEligibility.AfterColPosChanged -= new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.ugStoreEligibility_AfterColPosChanged);
                this.ugStoreEligibility.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreEligibility_CellChange);
                this.ugStoreEligibility.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugStoreEligibility_MouseEnterElement);
                this.ugStoreEligibility.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugStoreEligibility_DragDrop);
                this.ugStoreEligibility.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.ugStoreEligibility_KeyDown);
                this.ugStoreEligibility.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugStoreEligibility_DragEnter);
                this.ugStoreEligibility.AfterCellActivate -= new System.EventHandler(this.ugStoreEligibility_AfterCellActivate);
                this.cbxCHInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxCHInheritFromHigherLevel_CheckedChanged);
                this.ugCharacteristics.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugCharacteristics_BeforeCellUpdate);
                this.ugCharacteristics.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugCharacteristics_MouseDown);
                this.ugCharacteristics.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugCharacteristics_InitializeLayout);
                this.ugStoreEligibility.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(ugStoreEligibility_AfterCellUpdate); // TT#2015 - gtaylor - apply changes to lower levels
                this.ugCharacteristics.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(ugCharacteristics_AfterCellUpdate); // TT#2015 - gtaylor - apply changes to lower levels
                this.ugStoreCapacity.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(ugStoreCapacity_AfterCellUpdate); // TT#2015 - gtaylor - apply changes to lower levels
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugCharacteristics);
                //End TT#169
                this.ugCharacteristics.DragOver -= new System.Windows.Forms.DragEventHandler(this.ugGrid_DragOver);
                this.ugCharacteristics.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugCharacteristics_CellChange);
                this.ugCharacteristics.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugCharacteristics_MouseEnterElement);
                this.ugCharacteristics.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugCharacteristics_DragDrop);
                this.ugCharacteristics.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugCharacteristics_DragEnter);
                this.cbxVGInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxVGInheritFromHigherLevel_CheckedChanged);
                this.ugSellThruPcts.AfterRowsDeleted -= new System.EventHandler(this.ugSellThruPcts_AfterRowsDeleted);
                this.ugSellThruPcts.BeforeRowUpdate -= new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugSellThruPcts_BeforeRowUpdate);
                this.ugSellThruPcts.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugSellThruPcts_MouseDown);
                this.ugSellThruPcts.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSellThruPcts_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugSellThruPcts);
                //End TT#169
                this.ugSellThruPcts.DragOver -= new System.Windows.Forms.DragEventHandler(this.ugGrid_DragOver);
                this.ugSellThruPcts.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugSellThruPcts_BeforeExitEditMode);
                this.ugSellThruPcts.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugSellThruPcts_AfterRowInsert);
                this.ugSellThruPcts.AfterRowUpdate -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugSellThruPcts_AfterRowUpdate);
                this.ugSellThruPcts.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSellThruPcts_CellChange);
                this.ugSellThruPcts.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugSellThruPcts_MouseEnterElement);
                this.ugSellThruPcts.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugSellThruPcts_DragDrop);
                this.ugSellThruPcts.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugSellThruPcts_AfterSortChange);
                this.ugSellThruPcts.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugSellThruPcts_DragEnter);
                this.cbxVGApplyToLowerLevels.CheckedChanged -= new System.EventHandler(this.cbxVGApplyToLowerLevels_CheckedChanged);
                this.ugVelocityGrades.AfterRowsDeleted -= new System.EventHandler(this.ugVelocityGrades_AfterRowsDeleted);
                this.ugVelocityGrades.BeforeRowUpdate -= new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugVelocityGrades_BeforeRowUpdate);
                this.ugVelocityGrades.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugVelocityGrades_MouseDown);
                this.ugVelocityGrades.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugVelocityGrades_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugVelocityGrades);
                //End TT#169
                this.ugVelocityGrades.DragOver -= new System.Windows.Forms.DragEventHandler(this.ugGrid_DragOver);
                this.ugVelocityGrades.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugVelocityGrades_AfterRowInsert);
                this.ugVelocityGrades.AfterRowUpdate -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugVelocityGrades_AfterRowUpdate);
                this.ugVelocityGrades.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugVelocityGrades_CellChange);
                this.ugVelocityGrades.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugVelocityGrades_MouseEnterElement);
                this.ugVelocityGrades.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugVelocityGrades_DragDrop);
                this.ugVelocityGrades.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugVelocityGrades_AfterSortChange);
                this.ugVelocityGrades.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugVelocityGrades_DragEnter);
                this.tabStoreGrades.Click -= new System.EventHandler(this.tabStoreGrades_Click);
                this.cbxSGInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxSGInheritFromHigherLevel_CheckedChanged);
                this.cbxSGApplyToLowerLevels.CheckedChanged -= new System.EventHandler(this.cbxSGApplyToLowerLevels_CheckedChanged);
                this.ugStoreGrades.AfterRowsDeleted -= new System.EventHandler(this.ugStoreGrades_AfterRowsDeleted);
                this.ugStoreGrades.BeforeRowUpdate -= new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugStoreGrades_BeforeRowUpdate);
                this.ugStoreGrades.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugStoreGrades_MouseDown);
                this.ugStoreGrades.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugStoreGrades_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugStoreGrades);
                //End TT#169
                this.ugStoreGrades.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugStoreGrades_AfterRowInsert);
                this.ugStoreGrades.AfterRowUpdate -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugStoreGrades_AfterRowUpdate);
                this.ugStoreGrades.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreGrades_CellChange);
                this.ugStoreGrades.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugStoreGrades_MouseEnterElement);
                this.ugStoreGrades.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugStoreGrades_DragDrop);
                this.ugStoreGrades.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugStoreGrades_AfterSortChange);
                this.ugStoreGrades.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugStoreGrades_DragEnter);
                this.cbxSMMInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxSMMInheritFromHigherLevel_CheckedChanged);
                this.cbxSMMApplyToLowerLevels.CheckedChanged -= new System.EventHandler(this.cbxSMMApplyToLowerLevels_CheckedChanged);
                this.ugStockMinMax.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStockMinMax_ClickCellButton);
                this.ugStockMinMax.AfterRowsDeleted -= new System.EventHandler(this.ugStockMinMax_AfterRowsDeleted);
                this.ugStockMinMax.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugStockMinMax_MouseDown);
                this.ugStockMinMax.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugStockMinMax_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugStockMinMax);
                //End TT#169
                this.ugStockMinMax.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugStockMinMax_AfterRowInsert);
                this.ugStockMinMax.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStockMinMax_CellChange);
                this.ugStockMinMax.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugStockMinMax_MouseEnterElement);
                this.ugStockMinMax.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugStockMinMax_DragDrop);
                this.ugStockMinMax.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugStockMinMax_DragEnter);
                this.cbSMMStoreAttribute.DragOver -= new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragOver);
                this.cbSMMStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cbSMMStoreAttribute_SelectionChangeCommitted);
                this.cbSMMStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cbSMMStoreAttribute_DragDrop);
                this.cbSMMStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragEnter);
                this.cbSMMStoreAttributeSet.SelectionChangeCommitted -= new System.EventHandler(this.cbSMMStoreAttributeSet_SelectionChangeCommitted);
                //this.cbxSCInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxSCInheritFromHigherLevel_CheckedChanged);
                //this.cbxSCApplyToLowerLevels.CheckedChanged -= new System.EventHandler(this.cbxSCApplyToLowerLevels_CheckedChanged);
                this.cbSCStoreAttribute.DragOver -= new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragOver);
                this.cbSCStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cbSCStoreAttribute_SelectionChangeCommitted);
                this.cbSCStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cbSCStoreAttribute_DragDrop);
                this.cbSCStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragEnter);
                this.ugStoreCapacity.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugStoreCapacity_BeforeCellUpdate);
                this.ugStoreCapacity.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugStoreCapacity_MouseDown);
                this.ugStoreCapacity.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugStoreCapacity_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugStoreCapacity);
                //End TT#169
                this.ugStoreCapacity.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreCapacity_CellChange);
                this.ugStoreCapacity.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugStoreCapacity_MouseEnterElement);
                this.ugStoreCapacity.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugStoreCapacity_DragDrop);
                this.ugStoreCapacity.BeforeCellActivate -= new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugStoreCapacity_BeforeCellActivate);
                this.ugStoreCapacity.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugStoreCapacity_DragEnter);
                //this.cbxDPInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxDPInheritFromHigherLevel_CheckedChanged);
                //this.cbxDPApplyToLowerLevels.CheckedChanged -= new System.EventHandler(this.cbxDPApplyToLowerLevels_CheckedChanged);
                this.btnDeleteDateRange.Click -= new System.EventHandler(this.btnDeleteDateRange_Click);
                this.btnAddDateRange.Click -= new System.EventHandler(this.btnAddDateRange_Click);
                this.ugDailyPercentages.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDailyPercentages_ClickCellButton);
                this.ugDailyPercentages.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugDailyPercentages_MouseDown);
                this.ugDailyPercentages.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugDailyPercentages_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugDailyPercentages);
                //End TT#169
                this.ugDailyPercentages.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugDailyPercentages_BeforeRowsDeleted);
                this.ugDailyPercentages.AfterRowsDeleted -= new System.EventHandler(ugDailyPercentages_AfterRowsDeleted);
                this.ugDailyPercentages.DragOver -= new System.Windows.Forms.DragEventHandler(this.ugGrid_DragOver);
                this.ugDailyPercentages.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDailyPercentages_AfterCellUpdate);
                this.ugDailyPercentages.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDailyPercentages_CellChange);
                this.ugDailyPercentages.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugDailyPercentages_MouseEnterElement);
                this.ugDailyPercentages.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugDailyPercentages_DragDrop);
                this.ugDailyPercentages.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugDailyPercentages_DragEnter);
                this.cbDPStoreAttribute.DragOver -= new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragOver);
                this.cbDPStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cbDPStoreAttribute_SelectionChangeCommitted);
                this.cbDPStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cbDPStoreAttribute_DragDrop);
                this.cbDPStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cbDPStoreAttribute_DragEnter);
                this.cbxPCInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxPCInheritFromHigherLevel_CheckedChanged);
				//Begin TT#400-MD - JSmith - Add Header Purge Criteria by Header Type
                //this.txtPurgeHeaders.TextChanged -= new System.EventHandler(this.txtPurgeHeaders_TextChanged);
                //this.txtPurgeHeaders.Enter -= new System.EventHandler(this.txtPurgeHeaders_Enter);  //  TT#2015 - gtaylor - apply changes to lower levels
                //this.txtPurgeHeaders.Leave -= new System.EventHandler(this.txtPurgeHeaders_Leave);  //  TT#2015 - gtaylor - apply changes to lower levels
                //End TT#400-MD - JSmith - Add Header Purge Criteria by Header Type
                this.txtPurgePlans.TextChanged -= new System.EventHandler(this.txtPurgePlans_TextChanged);
                this.txtPurgePlans.Enter -= new System.EventHandler(this.txtPurgePlans_Enter);      //  TT#2015 - gtaylor - apply changes to lower levels
                this.txtPurgePlans.Leave -= new System.EventHandler(this.txtPurgePlans_Leave);      //  TT#2015 - gtaylor - apply changes to lower levels
                this.txtPurgeWeeklyHistory.TextChanged -= new System.EventHandler(this.txtPurgeWeeklyHistory_TextChanged);
                this.txtPurgeWeeklyHistory.Enter -= new System.EventHandler(this.txtPurgeWeeklyHistory_Enter);  //  TT#2015 - gtaylor - apply changes to lower levels
                this.txtPurgeWeeklyHistory.Leave -= new System.EventHandler(this.txtPurgeWeeklyHistory_Leave);  //  TT#2015 - gtaylor - apply changes to lower levels
                this.txtPurgeDailyHistory.TextChanged -= new System.EventHandler(this.txtPurgeDailyHistory_TextChanged);
                this.txtPurgeDailyHistory.Enter -= new System.EventHandler(this.txtPurgeDailyHistory_Enter);    //  TT#2015 - gtaylor - apply changes to lower levels
                this.txtPurgeDailyHistory.Leave -= new System.EventHandler(this.txtPurgeDailyHistory_Leave);    //  TT#2015 - gtaylor - apply changes to lower levels
                //this.cbxPCApplyToLowerLevels.CheckedChanged -= new System.EventHandler(this.cbxPCApplyToLowerLevels_CheckedChanged);
                this.tabSizeCurvesProperties.SelectedIndexChanged -= new System.EventHandler(this.tabSizeCurvesProperties_SelectedIndexChanged);
                this.ugSizeCurvesInheritedCriteria.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugSizeCurvesInheritedCriteria_MouseDown);
                this.ugSizeCurvesInheritedCriteria.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSizeCurvesInheritedCriteria_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugSizeCurvesInheritedCriteria);
                //End TT#169
                this.ugSizeCurvesInheritedCriteria.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugSizeCurvesInheritedCriteria_BeforeExitEditMode);
                this.ugSizeCurvesInheritedCriteria.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesInheritedCriteria_AfterCellUpdate);
                this.ugSizeCurvesInheritedCriteria.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugSizeCurvesInheritedCriteria_InitializeRow);
                this.ugSizeCurvesInheritedCriteria.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesInheritedCriteria_CellChange);
                this.ugSizeCurvesInheritedCriteria.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugSizeCurvesInheritedCriteria_MouseEnterElement);
                this.ugSizeCurvesCriteria.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesCriteria_ClickCellButton);
                this.ugSizeCurvesCriteria.AfterRowsDeleted -= new System.EventHandler(this.ugSizeCurvesCriteria_AfterRowsDeleted);
                this.ugSizeCurvesCriteria.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ugSizeCurvesCriteria_BeforeRowInsert);
                this.ugSizeCurvesCriteria.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugSizeCurvesCriteria_MouseDown);
                this.ugSizeCurvesCriteria.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSizeCurvesCriteria_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugSizeCurvesCriteria);
                //End TT#169
                this.ugSizeCurvesCriteria.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugSizeCurvesCriteria_BeforeExitEditMode);
                this.ugSizeCurvesCriteria.AfterCellListCloseUp -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesCriteria_AfterCellListCloseUp);
                this.ugSizeCurvesCriteria.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugSizeCurvesCriteria_AfterRowInsert);
                this.ugSizeCurvesCriteria.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesCriteria_AfterCellUpdate);
                this.ugSizeCurvesCriteria.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugSizeCurvesCriteria_InitializeRow);
                this.ugSizeCurvesCriteria.AfterRowUpdate -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugSizeCurvesCriteria_AfterRowUpdate);
                this.ugSizeCurvesCriteria.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesCriteria_CellChange);
                this.ugSizeCurvesCriteria.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugSizeCurvesCriteria_MouseEnterElement);
                this.txtSCTMaximumPct.TextChanged -= new System.EventHandler(this.txtSCTMaximumPct_TextChanged);
                this.txtSCTMinimumPct.TextChanged -= new System.EventHandler(this.txtSCTMinimumPct_TextChanged);
                this.rdoSCTUnits.CheckedChanged -= new System.EventHandler(this.rdoSCTUnits_CheckedChanged);
                this.rdoSCTIndexToAverage.CheckedChanged -= new System.EventHandler(this.rdoSCTIndexToAverage_CheckedChanged);
                this.txtSCTSalesTolerance.TextChanged -= new System.EventHandler(this.txtSCTSalesTolerance_TextChanged);
                this.cboSCTHighestLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboSCTHighestLevel_SelectionChangeCommitted);
                this.txtSCTMinimumAvgPerSize.TextChanged -= new System.EventHandler(this.txtSCTMinimumAvgPerSize_TextChanged);
                this.ugSizeCurvesSimilarStore.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesSimilarStore_ClickCellButton);
                this.ugSizeCurvesSimilarStore.BeforeCellListDropDown -= new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugSizeCurvesSimilarStore_BeforeCellListDropDown);
                this.ugSizeCurvesSimilarStore.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugSizeCurvesSimilarStore_MouseDown);
                this.ugSizeCurvesSimilarStore.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSizeCurvesSimilarStore_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugSizeCurvesSimilarStore);
                //End TT#169
                this.ugSizeCurvesSimilarStore.DragOver -= new System.Windows.Forms.DragEventHandler(this.ugGrid_DragOver);
                this.ugSizeCurvesSimilarStore.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugSizeCurvesSimilarStore_BeforeExitEditMode);
                this.ugSizeCurvesSimilarStore.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesSimilarStore_CellChange);
                this.ugSizeCurvesSimilarStore.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugSizeCurvesSimilarStore_DragDrop);
                this.ugSizeCurvesSimilarStore.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.ugSizeCurvesSimilarStore_KeyDown);
                this.ugSizeCurvesSimilarStore.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugSizeCurvesSimilarStore_DragEnter);
                this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
                this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
                this.mnuStoreEligibilityGrid.Popup -= new System.EventHandler(this.mnuStoreEligibilityGrid_Popup);
                this.mnuIMOGrid.Popup -= new System.EventHandler(this.mnuIMOGrid_Popup);    // TT#1401 - Reservation Stores - gtaylor
                this.mnuSortAsc.Click -= new System.EventHandler(this.mnuSortAsc_Click);
                this.mnuSortDesc.Click -= new System.EventHandler(this.mnuSortDesc_Click);
                this.mnuFind.Click -= new System.EventHandler(this.mnuFind_Click);
                this.mnuStoreCapacityGrid.Popup -= new System.EventHandler(this.mnuStoreCapacityGrid_Popup);
                this.btnApply.Click -= new System.EventHandler(this.btnApply_Click);
                this.mnuDailyPercentagesGrid.Popup -= new System.EventHandler(this.mnuDailyPercentagesGrid_Popup);
                this.txtNodeName.TextChanged -= new System.EventHandler(this.txtNodeName_TextChanged);
                this.txtNodeName.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtNodeName_DragDrop);
                this.txtNodeName.Leave -= new System.EventHandler(this.txtNodeName_Leave);
                this.txtNodeName.Enter -= new System.EventHandler(this.txtNodeName_Enter);
                this.txtNodeName.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtNodeName_DragEnter);
                this.txtNodeName.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtNodeName_DragOver);
                this.mnuSizeCurvesSimilarStoreGrid.Popup -= new System.EventHandler(this.mnuSizeCurvesSimilarStoreGrid_Popup);
                this.Load -= new System.EventHandler(this.frmNodeProperties_Load);
                this.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.frmNodeProperties_KeyDown);
                //Begin TT#??? - JSmith - Add Size Curve info to Node Properties
                this.cbSZStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cbSZStoreAttribute_SelectionChangeCommitted);
                //End TT#??? - JSmith - Add Size Curve info to Node Properties
                //Begin TT#??? - JSmith - Add Size Curve info to Node Properties
                this.cbxSZCriteriaApplyToLowerLevels.CheckedChanged -= new System.EventHandler(this.cbxSZCriteriaApplyToLowerLevels_CheckedChanged);
                this.cbxSZCriteriaInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxSZCriteriaInheritFromHigherLevel_CheckedChanged);
                this.cbxSZToleranceApplyToLowerLevels.CheckedChanged -= new System.EventHandler(this.cbxSZToleranceApplyToLowerLevels_CheckedChanged);
                this.cbxSZToleranceInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxSZToleranceInheritFromHigherLevel_CheckedChanged);
                this.cbxSZSimilarStoreApplyToLowerLevels.CheckedChanged -= new System.EventHandler(this.cbxSZSimilarStoreApplyToLowerLevels_CheckedChanged);
                this.cbxSZSimilarStoreInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxSZSimilarStoreInheritFromHigherLevel_CheckedChanged);
                //End TT#??? - JSmith - Add Size Curve info to Node Properties
                //Begin TT#483 - JScott - Add Size Lost Sales criteria and processing
                this.tabSizeOutOfStockProperties.SelectedIndexChanged -= new System.EventHandler(this.tabSizeOutOfStockProperties_SelectedIndexChanged);
                this.mcColorSizeByAttribute.ValueChanged -= new MIDRetail.Windows.Controls.MIDColorSizeByAttribute.ValueChangedHandler(this.mcColorSizeByAttribute_ValueChanged);
                this.cboSZOOSStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboSZOOSStoreAttribute_SelectionChangeCommitted);
                this.cboSZOOSSizeGroup.SelectionChangeCommitted -= new System.EventHandler(this.cboSZOOSSizeGroup_SelectionChangeCommitted);
                this.picSZOOSSizeGroupFilter.Click -= new System.EventHandler(this.picSZOOSSizeGroupFilter_Click);
                this.picSZOOSSizeGroupFilter.MouseHover -= new System.EventHandler(this.picSZOOSSizeGroupFilter_MouseHover);
                this.cbxSZOOSApplyToLowerLevels.CheckedChanged -= new System.EventHandler(this.cbxSZOOSApplyToLowerLevels_CheckedChanged);
                this.cbxSZOOSInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxSZOOSInheritFromHigherLevel_CheckedChanged);
                this.txtSSTLimit.TextChanged -= new System.EventHandler(this.txtSSTLimit_TextChanged);
                //End TT#483 - JScott - Add Size Lost Sales criteria and processing
                //Begin TT1498 - DOConnell - Chain Plan - Set Percentages - Phase 3
                this.ugChainSetPercent.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugChainSetPercent_BeforeCellUpdate);
                this.ugChainSetPercent.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugStoreCapacity_MouseDown);
                this.ugChainSetPercent.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugChainSetPercent_InitializeLayout);
                this.cbxCSPApplyToLowerLevels.CheckedChanged -= new System.EventHandler(this.cbxApplyInherit_CheckedChanged);   // TT#2015 - gtaylor - apply changes to lower levels
                //this.cbxCSPInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxCSPInheritFromHigherLevel_CheckedChanged);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                ugld.DetachGridEventHandlers(ugChainSetPercent);
                //End TT#169
                this.ugChainSetPercent.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugChainSetPercent_CellChange);
                this.ugChainSetPercent.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugChainSetPercent_MouseEnterElement);
                this.ugChainSetPercent.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugChainSetPercent_DragDrop);
                this.ugChainSetPercent.BeforeCellActivate -= new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugChainSetPercent_BeforeCellActivate);
                this.ugChainSetPercent.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugChainSetPercent_AfterCellUpdate);
                this.ugChainSetPercent.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugChainSetPercent_DragEnter);
                this.cbCSPStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cbCSPStoreAttribute_SelectionChangeCommitted);
                this.cbCSPStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cbCSPStoreAttribute_DragDrop);
                this.cbCSPStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cbCSPStoreAttribute_DragEnter);
                //Begin TT1498 - DOConnell - Chain Plan - Set Percentages - Phase 3
                //Begin TT#1740 - DOConnell - Missing Sub Menu
                this.mnuChainSetPercentGrid.Popup -= new System.EventHandler(this.mnuChainSetPercentGrid_Popup);
                //End TT#1740 - DOConnell - Missing Sub Menu
                //Begin TT#1399 - GTaylor
                this.txtApplyNodePropsFrom.TextChanged -= new System.EventHandler(this.txtApplyNodePropsFrom_TextChanged);
                this.txtApplyNodePropsFrom.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtApplyNodePropsFrom_DragDrop);
                this.txtApplyNodePropsFrom.Validated -= new System.EventHandler(this.txtApplyNodePropsFrom_Validated);
                this.txtApplyNodePropsFrom.Leave -= new System.EventHandler(this.txtApplyNodePropsFrom_Leave);
                this.txtApplyNodePropsFrom.Validating -= new System.ComponentModel.CancelEventHandler(this.txtApplyNodePropsFrom_Validating);
                this.txtApplyNodePropsFrom.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtApplyNodePropsFrom_DragEnter);
                this.txtApplyNodePropsFrom.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtApplyNodePropsFrom_DragOver);
                //End TT#1399 - GTaylor
                //Begin TT#1401 - GRT - Reservation Stores
                this.ugReservation.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugReservation_InitializeLayout);
                this.ugReservation.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugReservation_AfterCellUpdate);
                this.ugReservation.AfterExitEditMode -= new System.EventHandler(this.ugReservation_AfterExitEditMode);
                //End TT#1401 - GRT - Reservation Stores
                //BEGIN TT#108 - MD - DOConnell - FWOS Max Model
                this.ugReservation.AfterCellListCloseUp -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugReservation_AfterCellListCloseUp);
                this.ugReservation.AfterCellActivate -= new System.EventHandler(this.ugReservation_AfterCellActivate);
                //END TT#108 - MD - DOConnell - FWOS Max Model
                // Begin TT#3010 - JSmith - FWOS Max & FWOS Override model drop down not wide enough
				this.ugReservation.AfterColPosChanged -= new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.ugReservation_AfterColPosChanged);
                // End TT#3010 - JSmith - FWOS Max & FWOS Override model drop down not wide enough
                //Begin TT#2015 - gtaylor - Apply Changes to Lower Levels
                this._nodeChangeProfile = null;
                //End TT#2015 - gtaylor - Apply Changes to Lower Levels
                if (_progress != null)
                {
                    _progress.OnProgressOKClickedHandler -= new frmProgress.ProgressOKClickedEventHandler(OnProgressOKClicked);
                    _progress = null;
                }

                if (_mnuItemNewModel != null)
                {
                    _mnuItemNewModel.Click -= new System.EventHandler(this.mnuModelNew_Click);
                    _mnuItemNewModel = null;
                }

                if (_mnuItemOpenModel != null)
                {
                    _mnuItemOpenModel.Click -= new System.EventHandler(this.mnuModelEdit_Click);
                    _mnuItemOpenModel = null;
                }


                this.cbSZStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbSZStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cboOTSLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboOTSLevel_MIDComboBoxPropertiesChangedEvent);
                this.cbSEStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbSEStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cbSMMStoreAttributeSet.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbSMMStoreAttributeSet_MIDComboBoxPropertiesChangedEvent);
                this.cbSMMStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbSMMStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cbSCStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbSCStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cbDPStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbDPStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cboSCTHighestLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSCTHighestLevel_MIDComboBoxPropertiesChangedEvent);
                this.cboSZOOSStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSZOOSStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cbCSPStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbCSPStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.midAttributeCbx.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(midAttributeCbx_MIDComboBoxPropertiesChangedEvent);
                this.cboSZOOSSizeGroup.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSZOOSSizeGroup_MIDComboBoxPropertiesChangedEvent);

                //BEGIN TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
                txtHtASN.Enter -= txtHTASN_Enter;
                txtHtASN.TextChanged -= txtHTASN_TextChanged;
                txtHtDropShip.Enter -= txtHTDropShip_Enter;
                txtHtDropShip.TextChanged -= txtHTDropShip_TextChanged;
                txtHtDummy.Enter -= txtHTDummy_Enter;
                txtHtDummy.TextChanged -= txtHTDummy_TextChanged;
                txtHtPurchase.Enter -= txtHTPurchase_Enter;
                txtHtPurchase.TextChanged -= txtHTPurchase_TextChanged;
                txtHtReceipt.Enter -= txtHTReceipt_Enter;
                txtHtReceipt.TextChanged -= txtHTReceipt_TextChanged;
                txtHtReserve.Enter -= txtHTReserve_Enter;
                txtHtReserve.TextChanged -= txtHTReserve_TextChanged;
                txtHtVSW.Enter -= txtHTVSW_Enter;
                txtHtVSW.TextChanged -= txtHTVSW_TextChanged;
                txtHtWorkupTotBy.Enter -= txtHTWorkupTotBy_Enter;
                txtHtWorkupTotBy.TextChanged -= txtHTWorkupTotBy_TextChanged;
                //END TT#400-MD-VStuart-Add Header Purge Criteria by Header Type
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
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance96 = new Infragistics.Win.Appearance();
            this.lblNodeName = new System.Windows.Forms.Label();
            this.tabNodeProperties = new System.Windows.Forms.TabControl();
            this.tabProfile = new System.Windows.Forms.TabPage();
            this.gbxApplyNodeProperties = new System.Windows.Forms.GroupBox();
            this.lblApplyNodePropsFrom = new System.Windows.Forms.Label();
            this.txtApplyNodePropsFrom = new System.Windows.Forms.TextBox();
            this.gbxActive = new System.Windows.Forms.GroupBox();
            this.radActiveNo = new System.Windows.Forms.RadioButton();
            this.radActiveYes = new System.Windows.Forms.RadioButton();
            this.txtColorGroup = new System.Windows.Forms.TextBox();
            this.lblColorGroup = new System.Windows.Forms.Label();
            this.gbxBasicReplenishment = new System.Windows.Forms.GroupBox();
            this.radBasicReplenishmentNo = new System.Windows.Forms.RadioButton();
            this.radBasicReplenishmentYes = new System.Windows.Forms.RadioButton();
            this.gbxOTSPlanLevelOverride = new System.Windows.Forms.GroupBox();
            this.gbxOTSProduct = new System.Windows.Forms.GroupBox();
            this.cboOTSLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.radOTSNode = new System.Windows.Forms.RadioButton();
            this.radOTSLevel = new System.Windows.Forms.RadioButton();
            this.gbxOTSNode = new System.Windows.Forms.GroupBox();
            this.radOTSContainsID = new System.Windows.Forms.RadioButton();
            this.radOTSContainsName = new System.Windows.Forms.RadioButton();
            this.radOTSContainsDescription = new System.Windows.Forms.RadioButton();
            this.txtOTSLevelString = new System.Windows.Forms.TextBox();
            this.lblOTSLevelStartsWith = new System.Windows.Forms.Label();
            this.txtOTSAnchorNode = new System.Windows.Forms.TextBox();
            this.lblOTSHierarchy = new System.Windows.Forms.Label();
            this.gbxOTSType = new System.Windows.Forms.GroupBox();
            this.chkOTSTypeOverrideTotal = new System.Windows.Forms.CheckBox();
            this.chkOTSTypeOverrideRegular = new System.Windows.Forms.CheckBox();
            this.gbxProductType = new System.Windows.Forms.GroupBox();
            this.radTypeSoftline = new System.Windows.Forms.RadioButton();
            this.radTypeHardline = new System.Windows.Forms.RadioButton();
            this.radTypeUndefined = new System.Windows.Forms.RadioButton();
            this.txtNodeID = new System.Windows.Forms.TextBox();
            this.lblNodeID = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.tabStoreEligibility = new System.Windows.Forms.TabPage();
            this.grpTabButtonElig = new System.Windows.Forms.GroupBox();
            this.rbNoActionSE = new System.Windows.Forms.RadioButton();
            this.rbApplyChangesToLowerLevelsElig = new System.Windows.Forms.RadioButton();
            this.cbxSEInheritFromHigherLevel = new System.Windows.Forms.RadioButton();
            this.cbxSEApplyToLowerLevels = new System.Windows.Forms.RadioButton();
            this.uddFWOSModifier = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.cbSEStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblSEAttribute = new System.Windows.Forms.Label();
            this.uddSalesModifier = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.uddStockModifier = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.uddEligModel = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.ugStoreEligibility = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabCharacteristics = new System.Windows.Forms.TabPage();
            this.grpTabButtonChar = new System.Windows.Forms.GroupBox();
            this.rbNoActionChar = new System.Windows.Forms.RadioButton();
            this.rbApplyChangesToLowerLevelsChar = new System.Windows.Forms.RadioButton();
            this.cbxCHInheritFromHigherLevel = new System.Windows.Forms.RadioButton();
            this.cbxCHApplyToLowerLevels = new System.Windows.Forms.RadioButton();
            this.ugCharacteristics = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabVelocityGrades = new System.Windows.Forms.TabPage();
            this.lblVelocityMinMaxesInheritance = new System.Windows.Forms.Label();
            this.cbxVGInheritFromHigherLevel = new System.Windows.Forms.CheckBox();
            this.lblSellThruPctsInheritance = new System.Windows.Forms.Label();
            this.lblVelocityGradesInheritance = new System.Windows.Forms.Label();
            this.ugSellThruPcts = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cbxVGApplyToLowerLevels = new System.Windows.Forms.CheckBox();
            this.ugVelocityGrades = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabStoreGrades = new System.Windows.Forms.TabPage();
            this.cbxSGInheritFromHigherLevel = new System.Windows.Forms.CheckBox();
            this.lblMinMaxesInheritance = new System.Windows.Forms.Label();
            this.lblStoreGradesInheritance = new System.Windows.Forms.Label();
            this.cbxSGApplyToLowerLevels = new System.Windows.Forms.CheckBox();
            this.ugStoreGrades = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabStockMinMax = new System.Windows.Forms.TabPage();
            this.cbSMMStoreAttributeSet = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cbxSMMInheritFromHigherLevel = new System.Windows.Forms.CheckBox();
            this.cbxSMMApplyToLowerLevels = new System.Windows.Forms.CheckBox();
            this.ugStockMinMax = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.lblSMMAttributeSet = new System.Windows.Forms.Label();
            this.lblSMMAttribute = new System.Windows.Forms.Label();
            this.cbSMMStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.tabStoreCapacity = new System.Windows.Forms.TabPage();
            this.grpTabButtonSC = new System.Windows.Forms.GroupBox();
            this.rbNoActionSC = new System.Windows.Forms.RadioButton();
            this.rbApplyChangesToLowerLevelsSC = new System.Windows.Forms.RadioButton();
            this.cbxSCInheritFromHigherLevel = new System.Windows.Forms.RadioButton();
            this.cbxSCApplyToLowerLevels = new System.Windows.Forms.RadioButton();
            this.cbSCStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.ugStoreCapacity = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.lblSCAttribute = new System.Windows.Forms.Label();
            this.tabDailyPercentages = new System.Windows.Forms.TabPage();
            this.grpTabButtonDailyPct = new System.Windows.Forms.GroupBox();
            this.rbNoActionDP = new System.Windows.Forms.RadioButton();
            this.rbApplyChangesToLowerLevelsDailyPct = new System.Windows.Forms.RadioButton();
            this.cbxDPInheritFromHigherLevel = new System.Windows.Forms.RadioButton();
            this.cbxDPApplyToLowerLevels = new System.Windows.Forms.RadioButton();
            this.btnDeleteDateRange = new System.Windows.Forms.Button();
            this.btnAddDateRange = new System.Windows.Forms.Button();
            this.ugDailyPercentages = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cbDPStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblDPAttribute = new System.Windows.Forms.Label();
            this.tabPurgeCriteria = new System.Windows.Forms.TabPage();
            this.lblHtWorkUpTotTimeframe = new System.Windows.Forms.Label();
            this.txtHtWorkupTotBy = new System.Windows.Forms.TextBox();
            this.lblHtVSWTimeframe = new System.Windows.Forms.Label();
            this.lblHtReserveTimeframe = new System.Windows.Forms.Label();
            this.lblHtPurchaseOrderTimeframe = new System.Windows.Forms.Label();
            this.lblHtReceiptTimeframe = new System.Windows.Forms.Label();
            this.txtHtVSW = new System.Windows.Forms.TextBox();
            this.txtHtReserve = new System.Windows.Forms.TextBox();
            this.txtHtPurchase = new System.Windows.Forms.TextBox();
            this.txtHtReceipt = new System.Windows.Forms.TextBox();
            this.lblHtWorkUpTot = new System.Windows.Forms.Label();
            this.lblHtVSW = new System.Windows.Forms.Label();
            this.lblHtReserve = new System.Windows.Forms.Label();
            this.lblHtReceipt = new System.Windows.Forms.Label();
            this.lblHtPurchaseOrder = new System.Windows.Forms.Label();
            this.lblHtDummyTimeframe = new System.Windows.Forms.Label();
            this.lblHtDropShipTimeframe = new System.Windows.Forms.Label();
            this.lblHtASNTimeframe = new System.Windows.Forms.Label();
            this.txtHtDummy = new System.Windows.Forms.TextBox();
            this.txtHtDropShip = new System.Windows.Forms.TextBox();
            this.txtHtASN = new System.Windows.Forms.TextBox();
            this.lblHtDummy = new System.Windows.Forms.Label();
            this.lblHtDropShip = new System.Windows.Forms.Label();
            this.lblHtASN = new System.Windows.Forms.Label();
            this.grpTabButtonPurge = new System.Windows.Forms.GroupBox();
            this.rbNoActionPC = new System.Windows.Forms.RadioButton();
            this.rbApplyChangesToLowerLevelsPurge = new System.Windows.Forms.RadioButton();
            this.cbxPCInheritFromHigherLevel = new System.Windows.Forms.RadioButton();
            this.cbxPCApplyToLowerLevels = new System.Windows.Forms.RadioButton();
            this.txtPurgePlans = new System.Windows.Forms.TextBox();
            this.txtPurgeWeeklyHistory = new System.Windows.Forms.TextBox();
            this.txtPurgeDailyHistory = new System.Windows.Forms.TextBox();
            this.lblPurgePlansTimeframe = new System.Windows.Forms.Label();
            this.lblPurgeWeeklyHistoryTimeframe = new System.Windows.Forms.Label();
            this.lblPurgeDailyHistoryTimeframe = new System.Windows.Forms.Label();
            this.lblPurgeWeeklyHistory = new System.Windows.Forms.Label();
            this.lblPurgePlans = new System.Windows.Forms.Label();
            this.lblPurgeDailyHistory = new System.Windows.Forms.Label();
            this.tabSizeCurves = new System.Windows.Forms.TabPage();
            this.tabSizeCurvesProperties = new System.Windows.Forms.TabControl();
            this.tabSizeCurvesCriteria = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ugSizeCurvesInheritedCriteria = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ugSizeCurvesCriteria = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbxSZCriteriaApplyToLowerLevels = new System.Windows.Forms.CheckBox();
            this.cbxSZCriteriaInheritFromHigherLevel = new System.Windows.Forms.CheckBox();
            this.tabSizeCurvesTolerance = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbxSZToleranceApplyToLowerLevels = new System.Windows.Forms.CheckBox();
            this.cbxSZToleranceInheritFromHigherLevel = new System.Windows.Forms.CheckBox();
            this.gbxSCTMinMaxTolerancePct = new System.Windows.Forms.GroupBox();
            this.cbxApplyMinToZeroTolerance = new System.Windows.Forms.CheckBox();
            this.txtSCTMaximumPct = new System.Windows.Forms.TextBox();
            this.lblSCTMaximumPct = new System.Windows.Forms.Label();
            this.txtSCTMinimumPct = new System.Windows.Forms.TextBox();
            this.lblSCTMinimumPct = new System.Windows.Forms.Label();
            this.gbxSCTApplyChainSales = new System.Windows.Forms.GroupBox();
            this.gbxSCTIndexUnits = new System.Windows.Forms.GroupBox();
            this.rdoSCTIndexToAverage = new System.Windows.Forms.RadioButton();
            this.rdoSCTUnits = new System.Windows.Forms.RadioButton();
            this.txtSCTSalesTolerance = new System.Windows.Forms.TextBox();
            this.lblSCTSalesTolerance = new System.Windows.Forms.Label();
            this.gbxSCTHigherLevelSalesTolerance = new System.Windows.Forms.GroupBox();
            this.cboSCTHighestLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblSCTMinimumAvgPerSize = new System.Windows.Forms.Label();
            this.lblSCTHighestLevel = new System.Windows.Forms.Label();
            this.txtSCTMinimumAvgPerSize = new System.Windows.Forms.TextBox();
            this.tabSizeCurvesSimilarStore = new System.Windows.Forms.TabPage();
            this.ugSizeCurvesSimilarStore = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbxSZSimilarStoreApplyToLowerLevels = new System.Windows.Forms.CheckBox();
            this.cbxSZSimilarStoreInheritFromHigherLevel = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cbSZStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblSZAttribute = new System.Windows.Forms.Label();
            this.tabSizeOutOfStock = new System.Windows.Forms.TabPage();
            this.tabSizeOutOfStockProperties = new System.Windows.Forms.TabControl();
            this.tabSizeOutOfStockParms = new System.Windows.Forms.TabPage();
            this.mcColorSizeByAttribute = new MIDRetail.Windows.Controls.MIDColorSizeByAttribute();
            this.panel7 = new System.Windows.Forms.Panel();
            this.cboSZOOSSizeGroup = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblSZOOSAttributeSet = new System.Windows.Forms.Label();
            this.lblSZOOSSizeGroup = new System.Windows.Forms.Label();
            this.cboSZOOSStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.picSZOOSSizeGroupFilter = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbxSZOOSApplyToLowerLevels = new System.Windows.Forms.CheckBox();
            this.cbxSZOOSInheritFromHigherLevel = new System.Windows.Forms.CheckBox();
            this.tabSizeSellThru = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.cbxSZSellThruApplyToLowerLevels = new System.Windows.Forms.CheckBox();
            this.cbxSZSellThruInheritFromHigherLevel = new System.Windows.Forms.CheckBox();
            this.gbxSizeSellThru = new System.Windows.Forms.GroupBox();
            this.txtSSTLimit = new System.Windows.Forms.TextBox();
            this.lblSizeSellThruLimit = new System.Windows.Forms.Label();
            this.tabChainSetPct = new System.Windows.Forms.TabPage();
            this.grpTabButtonChainSet = new System.Windows.Forms.GroupBox();
            this.rbNoActionCSP = new System.Windows.Forms.RadioButton();
            this.rbApplyChangesToLowerLevelsChainSet = new System.Windows.Forms.RadioButton();
            this.cbxCSPApplyToLowerLevels = new System.Windows.Forms.RadioButton();
            this.cbxCSPInheritFromHigherLevel = new System.Windows.Forms.RadioButton();
            this.ugChainSetPercent = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.label2 = new System.Windows.Forms.Label();
            this.midDateRangeSelector1 = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCSPStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.tabReservation = new System.Windows.Forms.TabPage();
            this.grpTabButtonIMO = new System.Windows.Forms.GroupBox();
            this.cbxRsrvInheritFromHigherLevel = new System.Windows.Forms.RadioButton();
            this.rbNoActionIMO = new System.Windows.Forms.RadioButton();
            this.rbApplyChangesToLowerLevelsIMO = new System.Windows.Forms.RadioButton();
            this.cbxApplyToLowerLevels = new System.Windows.Forms.RadioButton();
            this.ugReservation = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.midAttributeCbx = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblRsrvStrAtt = new System.Windows.Forms.Label();
            this.uddFWOSMax = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.lblChainSetPercentInheritance = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.mnuStoreGrades = new System.Windows.Forms.ContextMenu();
            this.mnuCharacteristics = new System.Windows.Forms.ContextMenu();
            this.mnuStoreEligibilityGrid = new System.Windows.Forms.ContextMenu();
            this.mnuIMOGrid = new System.Windows.Forms.ContextMenu();
            this.mnuGridColHeader = new System.Windows.Forms.ContextMenu();
            this.mnuSortAsc = new System.Windows.Forms.MenuItem();
            this.mnuSortDesc = new System.Windows.Forms.MenuItem();
            this.mnuFind = new System.Windows.Forms.MenuItem();
            this.mnuStoreCapacityGrid = new System.Windows.Forms.ContextMenu();
            this.mnuVelocityGradesGrid = new System.Windows.Forms.ContextMenu();
            this.picNodeColor = new System.Windows.Forms.PictureBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.mnuDailyPercentagesGrid = new System.Windows.Forms.ContextMenu();
            this.mnuChainSetPercentGrid = new System.Windows.Forms.ContextMenu();
            this.txtNodeName = new System.Windows.Forms.TextBox();
            this.mnuSellThruPctsGrid = new System.Windows.Forms.ContextMenu();
            this.mnuStockMinMax = new System.Windows.Forms.ContextMenu();
            this.cmsCharacteristics = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSizeCurvesCriteria = new System.Windows.Forms.ContextMenu();
            this.mnuSizeCurvesSimilarStoreGrid = new System.Windows.Forms.ContextMenu();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabNodeProperties.SuspendLayout();
            this.tabProfile.SuspendLayout();
            this.gbxApplyNodeProperties.SuspendLayout();
            this.gbxActive.SuspendLayout();
            this.gbxBasicReplenishment.SuspendLayout();
            this.gbxOTSPlanLevelOverride.SuspendLayout();
            this.gbxOTSProduct.SuspendLayout();
            this.gbxOTSNode.SuspendLayout();
            this.gbxOTSType.SuspendLayout();
            this.gbxProductType.SuspendLayout();
            this.tabStoreEligibility.SuspendLayout();
            this.grpTabButtonElig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uddFWOSModifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uddSalesModifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uddStockModifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uddEligModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugStoreEligibility)).BeginInit();
            this.tabCharacteristics.SuspendLayout();
            this.grpTabButtonChar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugCharacteristics)).BeginInit();
            this.tabVelocityGrades.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugSellThruPcts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugVelocityGrades)).BeginInit();
            this.tabStoreGrades.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugStoreGrades)).BeginInit();
            this.tabStockMinMax.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugStockMinMax)).BeginInit();
            this.tabStoreCapacity.SuspendLayout();
            this.grpTabButtonSC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugStoreCapacity)).BeginInit();
            this.tabDailyPercentages.SuspendLayout();
            this.grpTabButtonDailyPct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugDailyPercentages)).BeginInit();
            this.tabPurgeCriteria.SuspendLayout();
            this.grpTabButtonPurge.SuspendLayout();
            this.tabSizeCurves.SuspendLayout();
            this.tabSizeCurvesProperties.SuspendLayout();
            this.tabSizeCurvesCriteria.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugSizeCurvesInheritedCriteria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugSizeCurvesCriteria)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabSizeCurvesTolerance.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbxSCTMinMaxTolerancePct.SuspendLayout();
            this.gbxSCTApplyChainSales.SuspendLayout();
            this.gbxSCTIndexUnits.SuspendLayout();
            this.gbxSCTHigherLevelSalesTolerance.SuspendLayout();
            this.tabSizeCurvesSimilarStore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugSizeCurvesSimilarStore)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabSizeOutOfStock.SuspendLayout();
            this.tabSizeOutOfStockProperties.SuspendLayout();
            this.tabSizeOutOfStockParms.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSZOOSSizeGroupFilter)).BeginInit();
            this.panel5.SuspendLayout();
            this.tabSizeSellThru.SuspendLayout();
            this.panel6.SuspendLayout();
            this.gbxSizeSellThru.SuspendLayout();
            this.tabChainSetPct.SuspendLayout();
            this.grpTabButtonChainSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugChainSetPercent)).BeginInit();
            this.tabReservation.SuspendLayout();
            this.grpTabButtonIMO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugReservation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uddFWOSMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNodeColor)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // lblNodeName
            // 
            this.lblNodeName.Location = new System.Drawing.Point(48, 16);
            this.lblNodeName.Name = "lblNodeName";
            this.lblNodeName.Size = new System.Drawing.Size(64, 16);
            this.lblNodeName.TabIndex = 0;
            this.lblNodeName.Text = "Name:";
            this.lblNodeName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabNodeProperties
            // 
            this.tabNodeProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabNodeProperties.Controls.Add(this.tabProfile);
            this.tabNodeProperties.Controls.Add(this.tabStoreEligibility);
            this.tabNodeProperties.Controls.Add(this.tabCharacteristics);
            this.tabNodeProperties.Controls.Add(this.tabVelocityGrades);
            this.tabNodeProperties.Controls.Add(this.tabStoreGrades);
            this.tabNodeProperties.Controls.Add(this.tabStockMinMax);
            this.tabNodeProperties.Controls.Add(this.tabStoreCapacity);
            this.tabNodeProperties.Controls.Add(this.tabDailyPercentages);
            this.tabNodeProperties.Controls.Add(this.tabPurgeCriteria);
            this.tabNodeProperties.Controls.Add(this.tabSizeCurves);
            this.tabNodeProperties.Controls.Add(this.tabSizeOutOfStock);
            this.tabNodeProperties.Controls.Add(this.tabChainSetPct);
            this.tabNodeProperties.Controls.Add(this.tabReservation);
            this.tabNodeProperties.Location = new System.Drawing.Point(24, 48);
            this.tabNodeProperties.Multiline = true;
            this.tabNodeProperties.Name = "tabNodeProperties";
            this.tabNodeProperties.SelectedIndex = 0;
            this.tabNodeProperties.Size = new System.Drawing.Size(728, 448);
            this.tabNodeProperties.TabIndex = 2;
            this.tabNodeProperties.SelectedIndexChanged += new System.EventHandler(this.tabNodeProperties_SelectedIndexChanged);
            this.tabNodeProperties.Click += new System.EventHandler(this.tabNodeProperties_Click);
            this.tabNodeProperties.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabNodeProperties_DragEnter);
            this.tabNodeProperties.DragOver += new System.Windows.Forms.DragEventHandler(this.tabNodeProperties_DragOver);
            // 
            // tabProfile
            // 
            this.tabProfile.AutoScroll = true;
            this.tabProfile.Controls.Add(this.gbxApplyNodeProperties);
            this.tabProfile.Controls.Add(this.gbxActive);
            this.tabProfile.Controls.Add(this.txtColorGroup);
            this.tabProfile.Controls.Add(this.lblColorGroup);
            this.tabProfile.Controls.Add(this.gbxBasicReplenishment);
            this.tabProfile.Controls.Add(this.gbxOTSPlanLevelOverride);
            this.tabProfile.Controls.Add(this.gbxProductType);
            this.tabProfile.Controls.Add(this.txtNodeID);
            this.tabProfile.Controls.Add(this.lblNodeID);
            this.tabProfile.Controls.Add(this.txtDescription);
            this.tabProfile.Controls.Add(this.lblDescription);
            this.tabProfile.Location = new System.Drawing.Point(4, 40);
            this.tabProfile.Name = "tabProfile";
            this.tabProfile.Size = new System.Drawing.Size(720, 404);
            this.tabProfile.TabIndex = 0;
            this.tabProfile.Text = "Profile";
            this.tabProfile.UseVisualStyleBackColor = true;
            // 
            // gbxApplyNodeProperties
            // 
            this.gbxApplyNodeProperties.Controls.Add(this.lblApplyNodePropsFrom);
            this.gbxApplyNodeProperties.Controls.Add(this.txtApplyNodePropsFrom);
            this.gbxApplyNodeProperties.Location = new System.Drawing.Point(16, 272);
            this.gbxApplyNodeProperties.Name = "gbxApplyNodeProperties";
            this.gbxApplyNodeProperties.Size = new System.Drawing.Size(190, 58);
            this.gbxApplyNodeProperties.TabIndex = 17;
            this.gbxApplyNodeProperties.TabStop = false;
            this.gbxApplyNodeProperties.Text = "Apply Node Properties";
            // 
            // lblApplyNodePropsFrom
            // 
            this.lblApplyNodePropsFrom.AutoSize = true;
            this.lblApplyNodePropsFrom.Location = new System.Drawing.Point(8, 26);
            this.lblApplyNodePropsFrom.Name = "lblApplyNodePropsFrom";
            this.lblApplyNodePropsFrom.Size = new System.Drawing.Size(30, 13);
            this.lblApplyNodePropsFrom.TabIndex = 1;
            this.lblApplyNodePropsFrom.Text = "From";
            // 
            // txtApplyNodePropsFrom
            // 
            this.txtApplyNodePropsFrom.AllowDrop = true;
            this.txtApplyNodePropsFrom.Location = new System.Drawing.Point(44, 23);
            this.txtApplyNodePropsFrom.Name = "txtApplyNodePropsFrom";
            this.txtApplyNodePropsFrom.Size = new System.Drawing.Size(132, 20);
            this.txtApplyNodePropsFrom.TabIndex = 0;
            this.txtApplyNodePropsFrom.TextChanged += new System.EventHandler(this.txtApplyNodePropsFrom_TextChanged);
            this.txtApplyNodePropsFrom.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtApplyNodePropsFrom_DragDrop);
            this.txtApplyNodePropsFrom.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtApplyNodePropsFrom_DragEnter);
            this.txtApplyNodePropsFrom.DragOver += new System.Windows.Forms.DragEventHandler(this.txtApplyNodePropsFrom_DragOver);
            this.txtApplyNodePropsFrom.Leave += new System.EventHandler(this.txtApplyNodePropsFrom_Leave);
            this.txtApplyNodePropsFrom.Validating += new System.ComponentModel.CancelEventHandler(this.txtApplyNodePropsFrom_Validating);
            this.txtApplyNodePropsFrom.Validated += new System.EventHandler(this.txtApplyNodePropsFrom_Validated);
            // 
            // gbxActive
            // 
            this.gbxActive.Controls.Add(this.radActiveNo);
            this.gbxActive.Controls.Add(this.radActiveYes);
            this.gbxActive.Enabled = false;
            this.gbxActive.Location = new System.Drawing.Point(16, 210);
            this.gbxActive.Name = "gbxActive";
            this.gbxActive.Size = new System.Drawing.Size(190, 56);
            this.gbxActive.TabIndex = 17;
            this.gbxActive.TabStop = false;
            this.gbxActive.Text = "Active";
            // 
            // radActiveNo
            // 
            this.radActiveNo.Location = new System.Drawing.Point(24, 16);
            this.radActiveNo.Name = "radActiveNo";
            this.radActiveNo.Size = new System.Drawing.Size(72, 24);
            this.radActiveNo.TabIndex = 11;
            this.radActiveNo.Text = "No";
            // 
            // radActiveYes
            // 
            this.radActiveYes.Location = new System.Drawing.Point(104, 16);
            this.radActiveYes.Name = "radActiveYes";
            this.radActiveYes.Size = new System.Drawing.Size(72, 24);
            this.radActiveYes.TabIndex = 12;
            this.radActiveYes.Text = "Yes";
            // 
            // txtColorGroup
            // 
            this.txtColorGroup.Location = new System.Drawing.Point(408, 24);
            this.txtColorGroup.Name = "txtColorGroup";
            this.txtColorGroup.Size = new System.Drawing.Size(152, 20);
            this.txtColorGroup.TabIndex = 3;
            // 
            // lblColorGroup
            // 
            this.lblColorGroup.Location = new System.Drawing.Point(336, 24);
            this.lblColorGroup.Name = "lblColorGroup";
            this.lblColorGroup.Size = new System.Drawing.Size(72, 23);
            this.lblColorGroup.TabIndex = 16;
            this.lblColorGroup.Text = "Color Group:";
            this.lblColorGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbxBasicReplenishment
            // 
            this.gbxBasicReplenishment.Controls.Add(this.radBasicReplenishmentNo);
            this.gbxBasicReplenishment.Controls.Add(this.radBasicReplenishmentYes);
            this.gbxBasicReplenishment.Location = new System.Drawing.Point(16, 336);
            this.gbxBasicReplenishment.Name = "gbxBasicReplenishment";
            this.gbxBasicReplenishment.Size = new System.Drawing.Size(190, 56);
            this.gbxBasicReplenishment.TabIndex = 7;
            this.gbxBasicReplenishment.TabStop = false;
            this.gbxBasicReplenishment.Text = "Basic Replenishment";
            this.gbxBasicReplenishment.Visible = false;
            // 
            // radBasicReplenishmentNo
            // 
            this.radBasicReplenishmentNo.Location = new System.Drawing.Point(24, 16);
            this.radBasicReplenishmentNo.Name = "radBasicReplenishmentNo";
            this.radBasicReplenishmentNo.Size = new System.Drawing.Size(72, 24);
            this.radBasicReplenishmentNo.TabIndex = 11;
            this.radBasicReplenishmentNo.Text = "No";
            this.radBasicReplenishmentNo.CheckedChanged += new System.EventHandler(this.radBasicReplenishmentNo_CheckedChanged);
            // 
            // radBasicReplenishmentYes
            // 
            this.radBasicReplenishmentYes.Location = new System.Drawing.Point(104, 16);
            this.radBasicReplenishmentYes.Name = "radBasicReplenishmentYes";
            this.radBasicReplenishmentYes.Size = new System.Drawing.Size(72, 24);
            this.radBasicReplenishmentYes.TabIndex = 12;
            this.radBasicReplenishmentYes.Text = "Yes";
            this.radBasicReplenishmentYes.CheckedChanged += new System.EventHandler(this.radBasicReplenishmentYes_CheckedChanged);
            // 
            // gbxOTSPlanLevelOverride
            // 
            this.gbxOTSPlanLevelOverride.Controls.Add(this.gbxOTSProduct);
            this.gbxOTSPlanLevelOverride.Controls.Add(this.gbxOTSType);
            this.gbxOTSPlanLevelOverride.Location = new System.Drawing.Point(240, 104);
            this.gbxOTSPlanLevelOverride.Name = "gbxOTSPlanLevelOverride";
            this.gbxOTSPlanLevelOverride.Size = new System.Drawing.Size(360, 288);
            this.gbxOTSPlanLevelOverride.TabIndex = 6;
            this.gbxOTSPlanLevelOverride.TabStop = false;
            this.gbxOTSPlanLevelOverride.Text = "OTS Forecast";
            // 
            // gbxOTSProduct
            // 
            this.gbxOTSProduct.Controls.Add(this.cboOTSLevel);
            this.gbxOTSProduct.Controls.Add(this.radOTSNode);
            this.gbxOTSProduct.Controls.Add(this.radOTSLevel);
            this.gbxOTSProduct.Controls.Add(this.gbxOTSNode);
            this.gbxOTSProduct.Controls.Add(this.txtOTSAnchorNode);
            this.gbxOTSProduct.Controls.Add(this.lblOTSHierarchy);
            this.gbxOTSProduct.Location = new System.Drawing.Point(16, 24);
            this.gbxOTSProduct.Name = "gbxOTSProduct";
            this.gbxOTSProduct.Size = new System.Drawing.Size(312, 184);
            this.gbxOTSProduct.TabIndex = 17;
            this.gbxOTSProduct.TabStop = false;
            this.gbxOTSProduct.Text = "Product";
            // 
            // cboOTSLevel
            // 
            this.cboOTSLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboOTSLevel.AutoAdjust = true;
            this.cboOTSLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboOTSLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboOTSLevel.DataSource = null;
            this.cboOTSLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOTSLevel.DropDownWidth = 208;
            this.cboOTSLevel.FormattingEnabled = false;
            this.cboOTSLevel.IgnoreFocusLost = false;
            this.cboOTSLevel.ItemHeight = 13;
            this.cboOTSLevel.Location = new System.Drawing.Point(88, 56);
            this.cboOTSLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cboOTSLevel.MaxDropDownItems = 25;
            this.cboOTSLevel.Name = "cboOTSLevel";
            this.cboOTSLevel.SetToolTip = "";
            this.cboOTSLevel.Size = new System.Drawing.Size(208, 21);
            this.cboOTSLevel.TabIndex = 10;
            this.cboOTSLevel.Tag = null;
            this.cboOTSLevel.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboOTSLevel_MIDComboBoxPropertiesChangedEvent);
            this.cboOTSLevel.SelectionChangeCommitted += new System.EventHandler(this.cboOTSLevel_SelectionChangeCommitted);
            // 
            // radOTSNode
            // 
            this.radOTSNode.Location = new System.Drawing.Point(24, 88);
            this.radOTSNode.Name = "radOTSNode";
            this.radOTSNode.Size = new System.Drawing.Size(16, 24);
            this.radOTSNode.TabIndex = 23;
            this.radOTSNode.CheckedChanged += new System.EventHandler(this.radOTSNode_CheckedChanged);
            // 
            // radOTSLevel
            // 
            this.radOTSLevel.Location = new System.Drawing.Point(24, 56);
            this.radOTSLevel.Name = "radOTSLevel";
            this.radOTSLevel.Size = new System.Drawing.Size(56, 24);
            this.radOTSLevel.TabIndex = 21;
            this.radOTSLevel.Text = "Level:";
            this.radOTSLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radOTSLevel.CheckedChanged += new System.EventHandler(this.radOTSLevel_CheckedChanged);
            // 
            // gbxOTSNode
            // 
            this.gbxOTSNode.Controls.Add(this.radOTSContainsID);
            this.gbxOTSNode.Controls.Add(this.radOTSContainsName);
            this.gbxOTSNode.Controls.Add(this.radOTSContainsDescription);
            this.gbxOTSNode.Controls.Add(this.txtOTSLevelString);
            this.gbxOTSNode.Controls.Add(this.lblOTSLevelStartsWith);
            this.gbxOTSNode.Location = new System.Drawing.Point(48, 88);
            this.gbxOTSNode.Name = "gbxOTSNode";
            this.gbxOTSNode.Size = new System.Drawing.Size(248, 80);
            this.gbxOTSNode.TabIndex = 20;
            this.gbxOTSNode.TabStop = false;
            this.gbxOTSNode.Text = "Node";
            // 
            // radOTSContainsID
            // 
            this.radOTSContainsID.Location = new System.Drawing.Point(24, 16);
            this.radOTSContainsID.Name = "radOTSContainsID";
            this.radOTSContainsID.Size = new System.Drawing.Size(40, 24);
            this.radOTSContainsID.TabIndex = 17;
            this.radOTSContainsID.Text = "ID";
            this.radOTSContainsID.CheckedChanged += new System.EventHandler(this.radOTSContainsID_CheckedChanged);
            // 
            // radOTSContainsName
            // 
            this.radOTSContainsName.Location = new System.Drawing.Point(80, 16);
            this.radOTSContainsName.Name = "radOTSContainsName";
            this.radOTSContainsName.Size = new System.Drawing.Size(64, 24);
            this.radOTSContainsName.TabIndex = 18;
            this.radOTSContainsName.Text = "Name";
            this.radOTSContainsName.CheckedChanged += new System.EventHandler(this.radOTSContainsName_CheckedChanged);
            // 
            // radOTSContainsDescription
            // 
            this.radOTSContainsDescription.Location = new System.Drawing.Point(152, 16);
            this.radOTSContainsDescription.Name = "radOTSContainsDescription";
            this.radOTSContainsDescription.Size = new System.Drawing.Size(88, 24);
            this.radOTSContainsDescription.TabIndex = 19;
            this.radOTSContainsDescription.Text = "Description";
            this.radOTSContainsDescription.CheckedChanged += new System.EventHandler(this.radOTSContainsDescription_CheckedChanged);
            // 
            // txtOTSLevelString
            // 
            this.txtOTSLevelString.Location = new System.Drawing.Point(96, 48);
            this.txtOTSLevelString.Name = "txtOTSLevelString";
            this.txtOTSLevelString.Size = new System.Drawing.Size(136, 20);
            this.txtOTSLevelString.TabIndex = 15;
            this.txtOTSLevelString.TextChanged += new System.EventHandler(this.txtOTSLevelString_TextChanged);
            // 
            // lblOTSLevelStartsWith
            // 
            this.lblOTSLevelStartsWith.Location = new System.Drawing.Point(24, 47);
            this.lblOTSLevelStartsWith.Name = "lblOTSLevelStartsWith";
            this.lblOTSLevelStartsWith.Size = new System.Drawing.Size(64, 23);
            this.lblOTSLevelStartsWith.TabIndex = 16;
            this.lblOTSLevelStartsWith.Text = "Starts With:";
            this.lblOTSLevelStartsWith.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtOTSAnchorNode
            // 
            this.txtOTSAnchorNode.AllowDrop = true;
            this.txtOTSAnchorNode.Location = new System.Drawing.Point(88, 24);
            this.txtOTSAnchorNode.Name = "txtOTSAnchorNode";
            this.txtOTSAnchorNode.Size = new System.Drawing.Size(208, 20);
            this.txtOTSAnchorNode.TabIndex = 14;
            this.txtOTSAnchorNode.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtOTSAnchorNode_DragDrop);
            this.txtOTSAnchorNode.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtOTSAnchorNode_DragEnter);
            this.txtOTSAnchorNode.DragOver += new System.Windows.Forms.DragEventHandler(this.txtOTSAnchorNode_DragOver);
            this.txtOTSAnchorNode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtOTSAnchorNode_KeyDown);
            this.txtOTSAnchorNode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOTSAnchorNode_KeyPress);
            this.txtOTSAnchorNode.Validating += new System.ComponentModel.CancelEventHandler(this.txtOTSAnchorNode_Validating);
            // 
            // lblOTSHierarchy
            // 
            this.lblOTSHierarchy.Location = new System.Drawing.Point(24, 24);
            this.lblOTSHierarchy.Name = "lblOTSHierarchy";
            this.lblOTSHierarchy.Size = new System.Drawing.Size(56, 23);
            this.lblOTSHierarchy.TabIndex = 13;
            this.lblOTSHierarchy.Text = "Hierarchy:";
            this.lblOTSHierarchy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbxOTSType
            // 
            this.gbxOTSType.Controls.Add(this.chkOTSTypeOverrideTotal);
            this.gbxOTSType.Controls.Add(this.chkOTSTypeOverrideRegular);
            this.gbxOTSType.Location = new System.Drawing.Point(16, 224);
            this.gbxOTSType.Name = "gbxOTSType";
            this.gbxOTSType.Size = new System.Drawing.Size(312, 48);
            this.gbxOTSType.TabIndex = 9;
            this.gbxOTSType.TabStop = false;
            this.gbxOTSType.Text = "Type";
            // 
            // chkOTSTypeOverrideTotal
            // 
            this.chkOTSTypeOverrideTotal.Location = new System.Drawing.Point(144, 16);
            this.chkOTSTypeOverrideTotal.Name = "chkOTSTypeOverrideTotal";
            this.chkOTSTypeOverrideTotal.Size = new System.Drawing.Size(88, 24);
            this.chkOTSTypeOverrideTotal.TabIndex = 1;
            this.chkOTSTypeOverrideTotal.Text = "Total";
            this.chkOTSTypeOverrideTotal.CheckedChanged += new System.EventHandler(this.cbxOTSTypeOverrideTotal_CheckedChanged);
            // 
            // chkOTSTypeOverrideRegular
            // 
            this.chkOTSTypeOverrideRegular.Location = new System.Drawing.Point(48, 16);
            this.chkOTSTypeOverrideRegular.Name = "chkOTSTypeOverrideRegular";
            this.chkOTSTypeOverrideRegular.Size = new System.Drawing.Size(88, 24);
            this.chkOTSTypeOverrideRegular.TabIndex = 0;
            this.chkOTSTypeOverrideRegular.Text = "Regular";
            this.chkOTSTypeOverrideRegular.CheckedChanged += new System.EventHandler(this.cbxOTSTypeOverrideRegular_CheckedChanged);
            // 
            // gbxProductType
            // 
            this.gbxProductType.Controls.Add(this.radTypeSoftline);
            this.gbxProductType.Controls.Add(this.radTypeHardline);
            this.gbxProductType.Controls.Add(this.radTypeUndefined);
            this.gbxProductType.Location = new System.Drawing.Point(48, 104);
            this.gbxProductType.Name = "gbxProductType";
            this.gbxProductType.Size = new System.Drawing.Size(144, 100);
            this.gbxProductType.TabIndex = 5;
            this.gbxProductType.TabStop = false;
            this.gbxProductType.Text = "Product Type";
            // 
            // radTypeSoftline
            // 
            this.radTypeSoftline.Location = new System.Drawing.Point(16, 64);
            this.radTypeSoftline.Name = "radTypeSoftline";
            this.radTypeSoftline.Size = new System.Drawing.Size(104, 24);
            this.radTypeSoftline.TabIndex = 7;
            this.radTypeSoftline.Text = "Softlines";
            this.radTypeSoftline.CheckedChanged += new System.EventHandler(this.radTypeSoftline_CheckedChanged);
            // 
            // radTypeHardline
            // 
            this.radTypeHardline.Location = new System.Drawing.Point(16, 40);
            this.radTypeHardline.Name = "radTypeHardline";
            this.radTypeHardline.Size = new System.Drawing.Size(104, 24);
            this.radTypeHardline.TabIndex = 6;
            this.radTypeHardline.Text = "Hardlines";
            this.radTypeHardline.CheckedChanged += new System.EventHandler(this.radTypeHardline_CheckedChanged);
            // 
            // radTypeUndefined
            // 
            this.radTypeUndefined.Location = new System.Drawing.Point(16, 16);
            this.radTypeUndefined.Name = "radTypeUndefined";
            this.radTypeUndefined.Size = new System.Drawing.Size(104, 24);
            this.radTypeUndefined.TabIndex = 5;
            this.radTypeUndefined.Text = "Undefined";
            this.radTypeUndefined.CheckedChanged += new System.EventHandler(this.radTypeUndefined_CheckedChanged);
            // 
            // txtNodeID
            // 
            this.txtNodeID.Location = new System.Drawing.Point(120, 24);
            this.txtNodeID.Name = "txtNodeID";
            this.txtNodeID.Size = new System.Drawing.Size(208, 20);
            this.txtNodeID.TabIndex = 2;
            this.txtNodeID.TextChanged += new System.EventHandler(this.txtNodeID_TextChanged);
            this.txtNodeID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNodeID_KeyDown);
            this.txtNodeID.Leave += new System.EventHandler(this.txtNodeID_Leave);
            // 
            // lblNodeID
            // 
            this.lblNodeID.Location = new System.Drawing.Point(48, 24);
            this.lblNodeID.Name = "lblNodeID";
            this.lblNodeID.Size = new System.Drawing.Size(64, 23);
            this.lblNodeID.TabIndex = 9;
            this.lblNodeID.Text = "ID:";
            this.lblNodeID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(120, 64);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(440, 20);
            this.txtDescription.TabIndex = 4;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(24, 64);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(88, 23);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Description:";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabStoreEligibility
            // 
            this.tabStoreEligibility.Controls.Add(this.grpTabButtonElig);
            this.tabStoreEligibility.Controls.Add(this.uddFWOSModifier);
            this.tabStoreEligibility.Controls.Add(this.cbSEStoreAttribute);
            this.tabStoreEligibility.Controls.Add(this.lblSEAttribute);
            this.tabStoreEligibility.Controls.Add(this.uddSalesModifier);
            this.tabStoreEligibility.Controls.Add(this.uddStockModifier);
            this.tabStoreEligibility.Controls.Add(this.uddEligModel);
            this.tabStoreEligibility.Controls.Add(this.ugStoreEligibility);
            this.tabStoreEligibility.Location = new System.Drawing.Point(4, 40);
            this.tabStoreEligibility.Name = "tabStoreEligibility";
            this.tabStoreEligibility.Size = new System.Drawing.Size(720, 404);
            this.tabStoreEligibility.TabIndex = 9;
            this.tabStoreEligibility.Text = "Store Eligibility";
            this.tabStoreEligibility.UseVisualStyleBackColor = true;
            // 
            // grpTabButtonElig
            // 
            this.grpTabButtonElig.Controls.Add(this.rbNoActionSE);
            this.grpTabButtonElig.Controls.Add(this.rbApplyChangesToLowerLevelsElig);
            this.grpTabButtonElig.Controls.Add(this.cbxSEInheritFromHigherLevel);
            this.grpTabButtonElig.Controls.Add(this.cbxSEApplyToLowerLevels);
            this.grpTabButtonElig.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpTabButtonElig.Location = new System.Drawing.Point(0, 370);
            this.grpTabButtonElig.Name = "grpTabButtonElig";
            this.grpTabButtonElig.Size = new System.Drawing.Size(720, 34);
            this.grpTabButtonElig.TabIndex = 27;
            this.grpTabButtonElig.TabStop = false;
            // 
            // rbNoActionSE
            // 
            this.rbNoActionSE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbNoActionSE.AutoSize = true;
            this.rbNoActionSE.Location = new System.Drawing.Point(559, 10);
            this.rbNoActionSE.Name = "rbNoActionSE";
            this.rbNoActionSE.Size = new System.Drawing.Size(72, 17);
            this.rbNoActionSE.TabIndex = 55;
            this.rbNoActionSE.Text = "No Action";
            // 
            // rbApplyChangesToLowerLevelsElig
            // 
            this.rbApplyChangesToLowerLevelsElig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbApplyChangesToLowerLevelsElig.AutoSize = true;
            this.rbApplyChangesToLowerLevelsElig.Location = new System.Drawing.Point(214, 10);
            this.rbApplyChangesToLowerLevelsElig.Name = "rbApplyChangesToLowerLevelsElig";
            this.rbApplyChangesToLowerLevelsElig.Size = new System.Drawing.Size(174, 17);
            this.rbApplyChangesToLowerLevelsElig.TabIndex = 38;
            this.rbApplyChangesToLowerLevelsElig.TabStop = true;
            this.rbApplyChangesToLowerLevelsElig.Text = "Apply Changes to Lower Levels";
            this.rbApplyChangesToLowerLevelsElig.UseVisualStyleBackColor = true;
            this.rbApplyChangesToLowerLevelsElig.CheckedChanged += new System.EventHandler(this.rbApplyChangesToLowerLevels_CheckedChanged);
            // 
            // cbxSEInheritFromHigherLevel
            // 
            this.cbxSEInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxSEInheritFromHigherLevel.Location = new System.Drawing.Point(55, 10);
            this.cbxSEInheritFromHigherLevel.Name = "cbxSEInheritFromHigherLevel";
            this.cbxSEInheritFromHigherLevel.Size = new System.Drawing.Size(134, 17);
            this.cbxSEInheritFromHigherLevel.TabIndex = 28;
            this.cbxSEInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxSEInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxApplyInherit_CheckedChanged);
            // 
            // cbxSEApplyToLowerLevels
            // 
            this.cbxSEApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxSEApplyToLowerLevels.Location = new System.Drawing.Point(413, 10);
            this.cbxSEApplyToLowerLevels.Name = "cbxSEApplyToLowerLevels";
            this.cbxSEApplyToLowerLevels.Size = new System.Drawing.Size(121, 17);
            this.cbxSEApplyToLowerLevels.TabIndex = 27;
            this.cbxSEApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxSEApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxApplyInherit_CheckedChanged);
            // 
            // uddFWOSModifier
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.uddFWOSModifier.DisplayLayout.Appearance = appearance1;
            this.uddFWOSModifier.DisplayLayout.InterBandSpacing = 10;
            this.uddFWOSModifier.DisplayLayout.MaxColScrollRegions = 1;
            this.uddFWOSModifier.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.uddFWOSModifier.DisplayLayout.Override.CardAreaAppearance = appearance2;
            this.uddFWOSModifier.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uddFWOSModifier.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.uddFWOSModifier.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uddFWOSModifier.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.uddFWOSModifier.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.uddFWOSModifier.DisplayLayout.Override.RowSelectorWidth = 12;
            this.uddFWOSModifier.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.uddFWOSModifier.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.uddFWOSModifier.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uddFWOSModifier.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.uddFWOSModifier.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.uddFWOSModifier.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.uddFWOSModifier.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.uddFWOSModifier.Location = new System.Drawing.Point(216, 96);
            this.uddFWOSModifier.Name = "uddFWOSModifier";
            this.uddFWOSModifier.Size = new System.Drawing.Size(160, 104);
            this.uddFWOSModifier.TabIndex = 25;
            this.uddFWOSModifier.Visible = false;
            this.uddFWOSModifier.RowSelected += new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.uddFWOSModifier_RowSelected);
            // 
            // cbSEStoreAttribute
            // 
            this.cbSEStoreAttribute.AllowDrop = true;
            this.cbSEStoreAttribute.AllowUserAttributes = false;
            this.cbSEStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbSEStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSEStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSEStoreAttribute.Location = new System.Drawing.Point(280, 16);
            this.cbSEStoreAttribute.Name = "cbSEStoreAttribute";
            this.cbSEStoreAttribute.Size = new System.Drawing.Size(200, 21);
            this.cbSEStoreAttribute.TabIndex = 2;
            this.cbSEStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cbSEStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cbSEStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cbSEStoreAttribute_SelectionChangeCommitted);
            this.cbSEStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cbSEStoreAttribute_DragDrop);
            this.cbSEStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragEnter);
            this.cbSEStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragOver);
            // 
            // lblSEAttribute
            // 
            this.lblSEAttribute.Location = new System.Drawing.Point(116, 16);
            this.lblSEAttribute.Name = "lblSEAttribute";
            this.lblSEAttribute.Size = new System.Drawing.Size(152, 23);
            this.lblSEAttribute.TabIndex = 24;
            this.lblSEAttribute.Text = "Store Attribute:";
            this.lblSEAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uddSalesModifier
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.uddSalesModifier.DisplayLayout.Appearance = appearance7;
            this.uddSalesModifier.DisplayLayout.InterBandSpacing = 10;
            this.uddSalesModifier.DisplayLayout.MaxColScrollRegions = 1;
            this.uddSalesModifier.DisplayLayout.MaxRowScrollRegions = 1;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.uddSalesModifier.DisplayLayout.Override.CardAreaAppearance = appearance8;
            this.uddSalesModifier.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uddSalesModifier.DisplayLayout.Override.HeaderAppearance = appearance9;
            this.uddSalesModifier.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uddSalesModifier.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.uddSalesModifier.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.uddSalesModifier.DisplayLayout.Override.RowSelectorWidth = 12;
            this.uddSalesModifier.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.uddSalesModifier.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.uddSalesModifier.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uddSalesModifier.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.uddSalesModifier.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.uddSalesModifier.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.uddSalesModifier.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.uddSalesModifier.Location = new System.Drawing.Point(216, 214);
            this.uddSalesModifier.Name = "uddSalesModifier";
            this.uddSalesModifier.Size = new System.Drawing.Size(160, 104);
            this.uddSalesModifier.TabIndex = 21;
            this.uddSalesModifier.Visible = false;
            this.uddSalesModifier.RowSelected += new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.uddSalesModifier_RowSelected);
            // 
            // uddStockModifier
            // 
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.uddStockModifier.DisplayLayout.Appearance = appearance13;
            this.uddStockModifier.DisplayLayout.InterBandSpacing = 10;
            this.uddStockModifier.DisplayLayout.MaxColScrollRegions = 1;
            this.uddStockModifier.DisplayLayout.MaxRowScrollRegions = 1;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            this.uddStockModifier.DisplayLayout.Override.CardAreaAppearance = appearance14;
            this.uddStockModifier.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.TextHAlignAsString = "Left";
            appearance15.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uddStockModifier.DisplayLayout.Override.HeaderAppearance = appearance15;
            this.uddStockModifier.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance16.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uddStockModifier.DisplayLayout.Override.RowAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.uddStockModifier.DisplayLayout.Override.RowSelectorAppearance = appearance17;
            this.uddStockModifier.DisplayLayout.Override.RowSelectorWidth = 12;
            this.uddStockModifier.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.uddStockModifier.DisplayLayout.Override.SelectedRowAppearance = appearance18;
            this.uddStockModifier.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uddStockModifier.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.uddStockModifier.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.uddStockModifier.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.uddStockModifier.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.uddStockModifier.Location = new System.Drawing.Point(48, 214);
            this.uddStockModifier.Name = "uddStockModifier";
            this.uddStockModifier.Size = new System.Drawing.Size(152, 104);
            this.uddStockModifier.TabIndex = 20;
            this.uddStockModifier.Visible = false;
            this.uddStockModifier.RowSelected += new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.uddStockModifier_RowSelected);
            // 
            // uddEligModel
            // 
            appearance19.BackColor = System.Drawing.Color.White;
            appearance19.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.uddEligModel.DisplayLayout.Appearance = appearance19;
            this.uddEligModel.DisplayLayout.InterBandSpacing = 10;
            this.uddEligModel.DisplayLayout.MaxColScrollRegions = 1;
            this.uddEligModel.DisplayLayout.MaxRowScrollRegions = 1;
            appearance20.BackColor = System.Drawing.Color.Transparent;
            this.uddEligModel.DisplayLayout.Override.CardAreaAppearance = appearance20;
            this.uddEligModel.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            appearance21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance21.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance21.ForeColor = System.Drawing.Color.Black;
            appearance21.TextHAlignAsString = "Left";
            appearance21.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.uddEligModel.DisplayLayout.Override.HeaderAppearance = appearance21;
            this.uddEligModel.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance22.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uddEligModel.DisplayLayout.Override.RowAppearance = appearance22;
            appearance23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance23.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.uddEligModel.DisplayLayout.Override.RowSelectorAppearance = appearance23;
            this.uddEligModel.DisplayLayout.Override.RowSelectorWidth = 12;
            this.uddEligModel.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance24.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance24.ForeColor = System.Drawing.Color.Black;
            this.uddEligModel.DisplayLayout.Override.SelectedRowAppearance = appearance24;
            this.uddEligModel.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uddEligModel.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.uddEligModel.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.uddEligModel.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.uddEligModel.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.uddEligModel.Location = new System.Drawing.Point(48, 96);
            this.uddEligModel.Name = "uddEligModel";
            this.uddEligModel.Size = new System.Drawing.Size(152, 112);
            this.uddEligModel.TabIndex = 1;
            this.uddEligModel.Visible = false;
            this.uddEligModel.RowSelected += new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.uddEligModel_RowSelected);
            // 
            // ugStoreEligibility
            // 
            this.ugStoreEligibility.AllowDrop = true;
            this.ugStoreEligibility.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance25.BackColor = System.Drawing.Color.White;
            appearance25.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugStoreEligibility.DisplayLayout.Appearance = appearance25;
            this.ugStoreEligibility.DisplayLayout.InterBandSpacing = 10;
            appearance26.BackColor = System.Drawing.Color.Transparent;
            this.ugStoreEligibility.DisplayLayout.Override.CardAreaAppearance = appearance26;
            appearance27.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance27.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance27.ForeColor = System.Drawing.Color.Black;
            appearance27.TextHAlignAsString = "Left";
            appearance27.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugStoreEligibility.DisplayLayout.Override.HeaderAppearance = appearance27;
            appearance28.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugStoreEligibility.DisplayLayout.Override.RowAppearance = appearance28;
            appearance29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance29.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugStoreEligibility.DisplayLayout.Override.RowSelectorAppearance = appearance29;
            this.ugStoreEligibility.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugStoreEligibility.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance30.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance30.ForeColor = System.Drawing.Color.Black;
            this.ugStoreEligibility.DisplayLayout.Override.SelectedRowAppearance = appearance30;
            this.ugStoreEligibility.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugStoreEligibility.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugStoreEligibility.Location = new System.Drawing.Point(16, 48);
            this.ugStoreEligibility.Name = "ugStoreEligibility";
            this.ugStoreEligibility.Size = new System.Drawing.Size(688, 318);
            this.ugStoreEligibility.TabIndex = 3;
            this.ugStoreEligibility.AfterCellActivate += new System.EventHandler(this.ugStoreEligibility_AfterCellActivate);
            this.ugStoreEligibility.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreEligibility_AfterCellUpdate);
            this.ugStoreEligibility.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugStoreEligibility_InitializeLayout);
            this.ugStoreEligibility.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreEligibility_CellChange);
            this.ugStoreEligibility.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreEligibility_ClickCellButton);
            this.ugStoreEligibility.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreEligibility_AfterCellListCloseUp);
            this.ugStoreEligibility.BeforeCellListDropDown += new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugStoreEligibility_BeforeCellListDropDown);
            this.ugStoreEligibility.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugStoreEligibility_BeforeExitEditMode);
            this.ugStoreEligibility.AfterColPosChanged += new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.ugStoreEligibility_AfterColPosChanged);
            this.ugStoreEligibility.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugStoreEligibility_MouseEnterElement);
            this.ugStoreEligibility.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugStoreEligibility_DragDrop);
            this.ugStoreEligibility.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugStoreEligibility_DragEnter);
            this.ugStoreEligibility.DragOver += new System.Windows.Forms.DragEventHandler(this.ugGrid_DragOver);
            this.ugStoreEligibility.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ugStoreEligibility_KeyDown);
            this.ugStoreEligibility.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugStoreEligibility_MouseDown);
            // 
            // tabCharacteristics
            // 
            this.tabCharacteristics.Controls.Add(this.grpTabButtonChar);
            this.tabCharacteristics.Controls.Add(this.ugCharacteristics);
            this.tabCharacteristics.Location = new System.Drawing.Point(4, 40);
            this.tabCharacteristics.Name = "tabCharacteristics";
            this.tabCharacteristics.Size = new System.Drawing.Size(720, 404);
            this.tabCharacteristics.TabIndex = 14;
            this.tabCharacteristics.Text = "Characteristics";
            this.tabCharacteristics.UseVisualStyleBackColor = true;
            // 
            // grpTabButtonChar
            // 
            this.grpTabButtonChar.Controls.Add(this.rbNoActionChar);
            this.grpTabButtonChar.Controls.Add(this.rbApplyChangesToLowerLevelsChar);
            this.grpTabButtonChar.Controls.Add(this.cbxCHInheritFromHigherLevel);
            this.grpTabButtonChar.Controls.Add(this.cbxCHApplyToLowerLevels);
            this.grpTabButtonChar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpTabButtonChar.Location = new System.Drawing.Point(0, 370);
            this.grpTabButtonChar.Name = "grpTabButtonChar";
            this.grpTabButtonChar.Size = new System.Drawing.Size(720, 34);
            this.grpTabButtonChar.TabIndex = 28;
            this.grpTabButtonChar.TabStop = false;
            // 
            // rbNoActionChar
            // 
            this.rbNoActionChar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbNoActionChar.AutoSize = true;
            this.rbNoActionChar.Location = new System.Drawing.Point(559, 10);
            this.rbNoActionChar.Name = "rbNoActionChar";
            this.rbNoActionChar.Size = new System.Drawing.Size(72, 17);
            this.rbNoActionChar.TabIndex = 55;
            this.rbNoActionChar.Text = "No Action";
            // 
            // rbApplyChangesToLowerLevelsChar
            // 
            this.rbApplyChangesToLowerLevelsChar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbApplyChangesToLowerLevelsChar.AutoSize = true;
            this.rbApplyChangesToLowerLevelsChar.Location = new System.Drawing.Point(214, 10);
            this.rbApplyChangesToLowerLevelsChar.Name = "rbApplyChangesToLowerLevelsChar";
            this.rbApplyChangesToLowerLevelsChar.Size = new System.Drawing.Size(174, 17);
            this.rbApplyChangesToLowerLevelsChar.TabIndex = 37;
            this.rbApplyChangesToLowerLevelsChar.TabStop = true;
            this.rbApplyChangesToLowerLevelsChar.Text = "Apply Changes to Lower Levels";
            this.rbApplyChangesToLowerLevelsChar.UseVisualStyleBackColor = true;
            this.rbApplyChangesToLowerLevelsChar.Visible = false;
            this.rbApplyChangesToLowerLevelsChar.CheckedChanged += new System.EventHandler(this.rbApplyChangesToLowerLevels_CheckedChanged);
            // 
            // cbxCHInheritFromHigherLevel
            // 
            this.cbxCHInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxCHInheritFromHigherLevel.AutoSize = true;
            this.cbxCHInheritFromHigherLevel.Location = new System.Drawing.Point(55, 10);
            this.cbxCHInheritFromHigherLevel.Name = "cbxCHInheritFromHigherLevel";
            this.cbxCHInheritFromHigherLevel.Size = new System.Drawing.Size(134, 17);
            this.cbxCHInheritFromHigherLevel.TabIndex = 29;
            this.cbxCHInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxCHInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxCHInheritFromHigherLevel_CheckedChanged);
            // 
            // cbxCHApplyToLowerLevels
            // 
            this.cbxCHApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxCHApplyToLowerLevels.AutoSize = true;
            this.cbxCHApplyToLowerLevels.Location = new System.Drawing.Point(413, 10);
            this.cbxCHApplyToLowerLevels.Name = "cbxCHApplyToLowerLevels";
            this.cbxCHApplyToLowerLevels.Size = new System.Drawing.Size(121, 17);
            this.cbxCHApplyToLowerLevels.TabIndex = 28;
            this.cbxCHApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxCHApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxApplyInherit_CheckedChanged);
            // 
            // ugCharacteristics
            // 
            this.ugCharacteristics.AllowDrop = true;
            this.ugCharacteristics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance31.BackColor = System.Drawing.Color.White;
            appearance31.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugCharacteristics.DisplayLayout.Appearance = appearance31;
            this.ugCharacteristics.DisplayLayout.InterBandSpacing = 10;
            this.ugCharacteristics.DisplayLayout.MaxColScrollRegions = 1;
            this.ugCharacteristics.DisplayLayout.MaxRowScrollRegions = 1;
            appearance32.BackColor = System.Drawing.Color.Transparent;
            this.ugCharacteristics.DisplayLayout.Override.CardAreaAppearance = appearance32;
            this.ugCharacteristics.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            appearance33.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance33.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance33.ForeColor = System.Drawing.Color.Black;
            appearance33.TextHAlignAsString = "Left";
            appearance33.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugCharacteristics.DisplayLayout.Override.HeaderAppearance = appearance33;
            this.ugCharacteristics.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance34.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugCharacteristics.DisplayLayout.Override.RowAppearance = appearance34;
            appearance35.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance35.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugCharacteristics.DisplayLayout.Override.RowSelectorAppearance = appearance35;
            this.ugCharacteristics.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugCharacteristics.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance36.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance36.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance36.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance36.ForeColor = System.Drawing.Color.Black;
            this.ugCharacteristics.DisplayLayout.Override.SelectedRowAppearance = appearance36;
            this.ugCharacteristics.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugCharacteristics.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugCharacteristics.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugCharacteristics.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugCharacteristics.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugCharacteristics.Location = new System.Drawing.Point(140, 12);
            this.ugCharacteristics.Name = "ugCharacteristics";
            this.ugCharacteristics.Size = new System.Drawing.Size(423, 339);
            this.ugCharacteristics.TabIndex = 6;
            this.ugCharacteristics.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugCharacteristics_AfterCellUpdate);
            this.ugCharacteristics.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugCharacteristics_InitializeLayout);
            this.ugCharacteristics.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugCharacteristics_CellChange);
            this.ugCharacteristics.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugCharacteristics_BeforeCellUpdate);
            this.ugCharacteristics.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugCharacteristics_MouseEnterElement);
            this.ugCharacteristics.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugCharacteristics_DragDrop);
            this.ugCharacteristics.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugCharacteristics_DragEnter);
            this.ugCharacteristics.DragOver += new System.Windows.Forms.DragEventHandler(this.ugGrid_DragOver);
            this.ugCharacteristics.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugCharacteristics_MouseDown);
            // 
            // tabVelocityGrades
            // 
            this.tabVelocityGrades.Controls.Add(this.lblVelocityMinMaxesInheritance);
            this.tabVelocityGrades.Controls.Add(this.cbxVGInheritFromHigherLevel);
            this.tabVelocityGrades.Controls.Add(this.lblSellThruPctsInheritance);
            this.tabVelocityGrades.Controls.Add(this.lblVelocityGradesInheritance);
            this.tabVelocityGrades.Controls.Add(this.ugSellThruPcts);
            this.tabVelocityGrades.Controls.Add(this.cbxVGApplyToLowerLevels);
            this.tabVelocityGrades.Controls.Add(this.ugVelocityGrades);
            this.tabVelocityGrades.Location = new System.Drawing.Point(4, 40);
            this.tabVelocityGrades.Name = "tabVelocityGrades";
            this.tabVelocityGrades.Size = new System.Drawing.Size(720, 404);
            this.tabVelocityGrades.TabIndex = 11;
            this.tabVelocityGrades.Text = "Velocity Grades";
            this.tabVelocityGrades.UseVisualStyleBackColor = true;
            // 
            // lblVelocityMinMaxesInheritance
            // 
            this.lblVelocityMinMaxesInheritance.Location = new System.Drawing.Point(144, 16);
            this.lblVelocityMinMaxesInheritance.Name = "lblVelocityMinMaxesInheritance";
            this.lblVelocityMinMaxesInheritance.Size = new System.Drawing.Size(32, 16);
            this.lblVelocityMinMaxesInheritance.TabIndex = 41;
            // 
            // cbxVGInheritFromHigherLevel
            // 
            this.cbxVGInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxVGInheritFromHigherLevel.Location = new System.Drawing.Point(416, 374);
            this.cbxVGInheritFromHigherLevel.Name = "cbxVGInheritFromHigherLevel";
            this.cbxVGInheritFromHigherLevel.Size = new System.Drawing.Size(144, 24);
            this.cbxVGInheritFromHigherLevel.TabIndex = 40;
            this.cbxVGInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxVGInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxVGInheritFromHigherLevel_CheckedChanged);
            // 
            // lblSellThruPctsInheritance
            // 
            this.lblSellThruPctsInheritance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSellThruPctsInheritance.BackColor = System.Drawing.Color.Transparent;
            this.lblSellThruPctsInheritance.Location = new System.Drawing.Point(533, 16);
            this.lblSellThruPctsInheritance.Name = "lblSellThruPctsInheritance";
            this.lblSellThruPctsInheritance.Size = new System.Drawing.Size(32, 16);
            this.lblSellThruPctsInheritance.TabIndex = 39;
            // 
            // lblVelocityGradesInheritance
            // 
            this.lblVelocityGradesInheritance.Location = new System.Drawing.Point(20, 16);
            this.lblVelocityGradesInheritance.Name = "lblVelocityGradesInheritance";
            this.lblVelocityGradesInheritance.Size = new System.Drawing.Size(32, 16);
            this.lblVelocityGradesInheritance.TabIndex = 38;
            // 
            // ugSellThruPcts
            // 
            this.ugSellThruPcts.AllowDrop = true;
            this.ugSellThruPcts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance37.BackColor = System.Drawing.Color.White;
            appearance37.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugSellThruPcts.DisplayLayout.Appearance = appearance37;
            this.ugSellThruPcts.DisplayLayout.InterBandSpacing = 10;
            appearance38.BackColor = System.Drawing.Color.Transparent;
            this.ugSellThruPcts.DisplayLayout.Override.CardAreaAppearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance39.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance39.ForeColor = System.Drawing.Color.Black;
            appearance39.TextHAlignAsString = "Left";
            appearance39.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugSellThruPcts.DisplayLayout.Override.HeaderAppearance = appearance39;
            appearance40.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSellThruPcts.DisplayLayout.Override.RowAppearance = appearance40;
            appearance41.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance41.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance41.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugSellThruPcts.DisplayLayout.Override.RowSelectorAppearance = appearance41;
            this.ugSellThruPcts.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugSellThruPcts.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance42.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance42.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance42.ForeColor = System.Drawing.Color.Black;
            this.ugSellThruPcts.DisplayLayout.Override.SelectedRowAppearance = appearance42;
            this.ugSellThruPcts.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSellThruPcts.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugSellThruPcts.Location = new System.Drawing.Point(536, 40);
            this.ugSellThruPcts.Name = "ugSellThruPcts";
            this.ugSellThruPcts.Size = new System.Drawing.Size(168, 328);
            this.ugSellThruPcts.TabIndex = 3;
            this.ugSellThruPcts.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSellThruPcts_InitializeLayout);
            this.ugSellThruPcts.AfterRowsDeleted += new System.EventHandler(this.ugSellThruPcts_AfterRowsDeleted);
            this.ugSellThruPcts.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugSellThruPcts_AfterRowInsert);
            this.ugSellThruPcts.AfterRowUpdate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugSellThruPcts_AfterRowUpdate);
            this.ugSellThruPcts.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugSellThruPcts_BeforeRowUpdate);
            this.ugSellThruPcts.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSellThruPcts_CellChange);
            this.ugSellThruPcts.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugSellThruPcts_BeforeExitEditMode);
            this.ugSellThruPcts.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugSellThruPcts_AfterSortChange);
            this.ugSellThruPcts.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugSellThruPcts_MouseEnterElement);
            this.ugSellThruPcts.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugSellThruPcts_DragDrop);
            this.ugSellThruPcts.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugSellThruPcts_DragEnter);
            this.ugSellThruPcts.DragOver += new System.Windows.Forms.DragEventHandler(this.ugGrid_DragOver);
            this.ugSellThruPcts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugSellThruPcts_MouseDown);
            // 
            // cbxVGApplyToLowerLevels
            // 
            this.cbxVGApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxVGApplyToLowerLevels.Location = new System.Drawing.Point(576, 374);
            this.cbxVGApplyToLowerLevels.Name = "cbxVGApplyToLowerLevels";
            this.cbxVGApplyToLowerLevels.Size = new System.Drawing.Size(128, 24);
            this.cbxVGApplyToLowerLevels.TabIndex = 4;
            this.cbxVGApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxVGApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxVGApplyToLowerLevels_CheckedChanged);
            // 
            // ugVelocityGrades
            // 
            this.ugVelocityGrades.AllowDrop = true;
            this.ugVelocityGrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance43.BackColor = System.Drawing.Color.White;
            appearance43.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance43.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugVelocityGrades.DisplayLayout.Appearance = appearance43;
            this.ugVelocityGrades.DisplayLayout.InterBandSpacing = 10;
            appearance44.BackColor = System.Drawing.Color.Transparent;
            this.ugVelocityGrades.DisplayLayout.Override.CardAreaAppearance = appearance44;
            this.ugVelocityGrades.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            appearance45.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance45.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance45.ForeColor = System.Drawing.Color.Black;
            appearance45.TextHAlignAsString = "Left";
            appearance45.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugVelocityGrades.DisplayLayout.Override.HeaderAppearance = appearance45;
            this.ugVelocityGrades.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance46.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugVelocityGrades.DisplayLayout.Override.RowAppearance = appearance46;
            appearance47.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance47.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance47.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugVelocityGrades.DisplayLayout.Override.RowSelectorAppearance = appearance47;
            this.ugVelocityGrades.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugVelocityGrades.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance48.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance48.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance48.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance48.ForeColor = System.Drawing.Color.Black;
            this.ugVelocityGrades.DisplayLayout.Override.SelectedRowAppearance = appearance48;
            this.ugVelocityGrades.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugVelocityGrades.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugVelocityGrades.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugVelocityGrades.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugVelocityGrades.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugVelocityGrades.Location = new System.Drawing.Point(15, 40);
            this.ugVelocityGrades.Name = "ugVelocityGrades";
            this.ugVelocityGrades.Size = new System.Drawing.Size(501, 328);
            this.ugVelocityGrades.TabIndex = 2;
            this.ugVelocityGrades.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugVelocityGrades_InitializeLayout);
            this.ugVelocityGrades.AfterRowsDeleted += new System.EventHandler(this.ugVelocityGrades_AfterRowsDeleted);
            this.ugVelocityGrades.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugVelocityGrades_AfterRowInsert);
            this.ugVelocityGrades.AfterRowUpdate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugVelocityGrades_AfterRowUpdate);
            this.ugVelocityGrades.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugVelocityGrades_BeforeRowUpdate);
            this.ugVelocityGrades.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugVelocityGrades_CellChange);
            this.ugVelocityGrades.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugVelocityGrades_AfterSortChange);
            this.ugVelocityGrades.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugVelocityGrades_MouseEnterElement);
            this.ugVelocityGrades.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugVelocityGrades_DragDrop);
            this.ugVelocityGrades.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugVelocityGrades_DragEnter);
            this.ugVelocityGrades.DragOver += new System.Windows.Forms.DragEventHandler(this.ugGrid_DragOver);
            this.ugVelocityGrades.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugVelocityGrades_MouseDown);
            // 
            // tabStoreGrades
            // 
            this.tabStoreGrades.Controls.Add(this.cbxSGInheritFromHigherLevel);
            this.tabStoreGrades.Controls.Add(this.lblMinMaxesInheritance);
            this.tabStoreGrades.Controls.Add(this.lblStoreGradesInheritance);
            this.tabStoreGrades.Controls.Add(this.cbxSGApplyToLowerLevels);
            this.tabStoreGrades.Controls.Add(this.ugStoreGrades);
            this.tabStoreGrades.Location = new System.Drawing.Point(4, 40);
            this.tabStoreGrades.Name = "tabStoreGrades";
            this.tabStoreGrades.Size = new System.Drawing.Size(720, 404);
            this.tabStoreGrades.TabIndex = 6;
            this.tabStoreGrades.Text = "Store Grades";
            this.tabStoreGrades.UseVisualStyleBackColor = true;
            this.tabStoreGrades.Click += new System.EventHandler(this.tabStoreGrades_Click);
            // 
            // cbxSGInheritFromHigherLevel
            // 
            this.cbxSGInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSGInheritFromHigherLevel.Location = new System.Drawing.Point(416, 374);
            this.cbxSGInheritFromHigherLevel.Name = "cbxSGInheritFromHigherLevel";
            this.cbxSGInheritFromHigherLevel.Size = new System.Drawing.Size(144, 24);
            this.cbxSGInheritFromHigherLevel.TabIndex = 38;
            this.cbxSGInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxSGInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxSGInheritFromHigherLevel_CheckedChanged);
            // 
            // lblMinMaxesInheritance
            // 
            this.lblMinMaxesInheritance.Location = new System.Drawing.Point(264, 16);
            this.lblMinMaxesInheritance.Name = "lblMinMaxesInheritance";
            this.lblMinMaxesInheritance.Size = new System.Drawing.Size(32, 16);
            this.lblMinMaxesInheritance.TabIndex = 37;
            // 
            // lblStoreGradesInheritance
            // 
            this.lblStoreGradesInheritance.Location = new System.Drawing.Point(40, 16);
            this.lblStoreGradesInheritance.Name = "lblStoreGradesInheritance";
            this.lblStoreGradesInheritance.Size = new System.Drawing.Size(32, 16);
            this.lblStoreGradesInheritance.TabIndex = 36;
            // 
            // cbxSGApplyToLowerLevels
            // 
            this.cbxSGApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSGApplyToLowerLevels.Location = new System.Drawing.Point(576, 374);
            this.cbxSGApplyToLowerLevels.Name = "cbxSGApplyToLowerLevels";
            this.cbxSGApplyToLowerLevels.Size = new System.Drawing.Size(128, 24);
            this.cbxSGApplyToLowerLevels.TabIndex = 35;
            this.cbxSGApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxSGApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxSGApplyToLowerLevels_CheckedChanged);
            // 
            // ugStoreGrades
            // 
            this.ugStoreGrades.AllowDrop = true;
            this.ugStoreGrades.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance49.BackColor = System.Drawing.Color.White;
            appearance49.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance49.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugStoreGrades.DisplayLayout.Appearance = appearance49;
            this.ugStoreGrades.DisplayLayout.InterBandSpacing = 10;
            appearance50.BackColor = System.Drawing.Color.Transparent;
            this.ugStoreGrades.DisplayLayout.Override.CardAreaAppearance = appearance50;
            appearance51.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance51.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance51.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance51.ForeColor = System.Drawing.Color.Black;
            appearance51.TextHAlignAsString = "Left";
            appearance51.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugStoreGrades.DisplayLayout.Override.HeaderAppearance = appearance51;
            appearance52.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugStoreGrades.DisplayLayout.Override.RowAppearance = appearance52;
            appearance53.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance53.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance53.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugStoreGrades.DisplayLayout.Override.RowSelectorAppearance = appearance53;
            this.ugStoreGrades.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugStoreGrades.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance54.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance54.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance54.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance54.ForeColor = System.Drawing.Color.Black;
            this.ugStoreGrades.DisplayLayout.Override.SelectedRowAppearance = appearance54;
            this.ugStoreGrades.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugStoreGrades.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugStoreGrades.Location = new System.Drawing.Point(16, 40);
            this.ugStoreGrades.Name = "ugStoreGrades";
            this.ugStoreGrades.Size = new System.Drawing.Size(688, 326);
            this.ugStoreGrades.TabIndex = 4;
            this.ugStoreGrades.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugStoreGrades_InitializeLayout);
            this.ugStoreGrades.AfterRowsDeleted += new System.EventHandler(this.ugStoreGrades_AfterRowsDeleted);
            this.ugStoreGrades.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugStoreGrades_AfterRowInsert);
            this.ugStoreGrades.AfterRowUpdate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugStoreGrades_AfterRowUpdate);
            this.ugStoreGrades.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugStoreGrades_BeforeRowUpdate);
            this.ugStoreGrades.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreGrades_CellChange);
            this.ugStoreGrades.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugStoreGrades_AfterSortChange);
            this.ugStoreGrades.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugStoreGrades_MouseEnterElement);
            this.ugStoreGrades.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugStoreGrades_DragDrop);
            this.ugStoreGrades.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugStoreGrades_DragEnter);
            this.ugStoreGrades.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugStoreGrades_MouseDown);
            // 
            // tabStockMinMax
            // 
            this.tabStockMinMax.Controls.Add(this.cbSMMStoreAttributeSet);
            this.tabStockMinMax.Controls.Add(this.cbxSMMInheritFromHigherLevel);
            this.tabStockMinMax.Controls.Add(this.cbxSMMApplyToLowerLevels);
            this.tabStockMinMax.Controls.Add(this.ugStockMinMax);
            this.tabStockMinMax.Controls.Add(this.lblSMMAttributeSet);
            this.tabStockMinMax.Controls.Add(this.lblSMMAttribute);
            this.tabStockMinMax.Controls.Add(this.cbSMMStoreAttribute);
            this.tabStockMinMax.Location = new System.Drawing.Point(4, 40);
            this.tabStockMinMax.Name = "tabStockMinMax";
            this.tabStockMinMax.Size = new System.Drawing.Size(720, 404);
            this.tabStockMinMax.TabIndex = 13;
            this.tabStockMinMax.Text = "Stock Min/Max";
            this.tabStockMinMax.UseVisualStyleBackColor = true;
            // 
            // cbSMMStoreAttributeSet
            // 
            this.cbSMMStoreAttributeSet.AutoAdjust = true;
            this.cbSMMStoreAttributeSet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbSMMStoreAttributeSet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSMMStoreAttributeSet.DataSource = null;
            this.cbSMMStoreAttributeSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSMMStoreAttributeSet.DropDownWidth = 211;
            this.cbSMMStoreAttributeSet.FormattingEnabled = false;
            this.cbSMMStoreAttributeSet.IgnoreFocusLost = false;
            this.cbSMMStoreAttributeSet.ItemHeight = 13;
            this.cbSMMStoreAttributeSet.Location = new System.Drawing.Point(280, 48);
            this.cbSMMStoreAttributeSet.Margin = new System.Windows.Forms.Padding(0);
            this.cbSMMStoreAttributeSet.MaxDropDownItems = 25;
            this.cbSMMStoreAttributeSet.Name = "cbSMMStoreAttributeSet";
            this.cbSMMStoreAttributeSet.SetToolTip = "";
            this.cbSMMStoreAttributeSet.Size = new System.Drawing.Size(211, 21);
            this.cbSMMStoreAttributeSet.TabIndex = 21;
            this.cbSMMStoreAttributeSet.Tag = null;
            this.cbSMMStoreAttributeSet.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cbSMMStoreAttributeSet_MIDComboBoxPropertiesChangedEvent);
            this.cbSMMStoreAttributeSet.SelectionChangeCommitted += new System.EventHandler(this.cbSMMStoreAttributeSet_SelectionChangeCommitted);
            // 
            // cbxSMMInheritFromHigherLevel
            // 
            this.cbxSMMInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSMMInheritFromHigherLevel.Location = new System.Drawing.Point(416, 366);
            this.cbxSMMInheritFromHigherLevel.Name = "cbxSMMInheritFromHigherLevel";
            this.cbxSMMInheritFromHigherLevel.Size = new System.Drawing.Size(144, 24);
            this.cbxSMMInheritFromHigherLevel.TabIndex = 37;
            this.cbxSMMInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxSMMInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxSMMInheritFromHigherLevel_CheckedChanged);
            // 
            // cbxSMMApplyToLowerLevels
            // 
            this.cbxSMMApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSMMApplyToLowerLevels.Location = new System.Drawing.Point(576, 366);
            this.cbxSMMApplyToLowerLevels.Name = "cbxSMMApplyToLowerLevels";
            this.cbxSMMApplyToLowerLevels.Size = new System.Drawing.Size(128, 24);
            this.cbxSMMApplyToLowerLevels.TabIndex = 36;
            this.cbxSMMApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxSMMApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxSMMApplyToLowerLevels_CheckedChanged);
            // 
            // ugStockMinMax
            // 
            this.ugStockMinMax.AllowDrop = true;
            this.ugStockMinMax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance55.BackColor = System.Drawing.Color.White;
            appearance55.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance55.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugStockMinMax.DisplayLayout.Appearance = appearance55;
            this.ugStockMinMax.DisplayLayout.InterBandSpacing = 10;
            appearance56.BackColor = System.Drawing.Color.Transparent;
            this.ugStockMinMax.DisplayLayout.Override.CardAreaAppearance = appearance56;
            appearance57.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance57.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance57.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance57.ForeColor = System.Drawing.Color.Black;
            appearance57.TextHAlignAsString = "Left";
            appearance57.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugStockMinMax.DisplayLayout.Override.HeaderAppearance = appearance57;
            appearance58.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugStockMinMax.DisplayLayout.Override.RowAppearance = appearance58;
            appearance59.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance59.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance59.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugStockMinMax.DisplayLayout.Override.RowSelectorAppearance = appearance59;
            this.ugStockMinMax.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugStockMinMax.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance60.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance60.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance60.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance60.ForeColor = System.Drawing.Color.Black;
            this.ugStockMinMax.DisplayLayout.Override.SelectedRowAppearance = appearance60;
            this.ugStockMinMax.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugStockMinMax.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugStockMinMax.Location = new System.Drawing.Point(24, 88);
            this.ugStockMinMax.Name = "ugStockMinMax";
            this.ugStockMinMax.Size = new System.Drawing.Size(672, 262);
            this.ugStockMinMax.TabIndex = 25;
            this.ugStockMinMax.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugStockMinMax_InitializeLayout);
            this.ugStockMinMax.AfterRowsDeleted += new System.EventHandler(this.ugStockMinMax_AfterRowsDeleted);
            this.ugStockMinMax.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugStockMinMax_AfterRowInsert);
            this.ugStockMinMax.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStockMinMax_CellChange);
            this.ugStockMinMax.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStockMinMax_ClickCellButton);
            this.ugStockMinMax.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugStockMinMax_MouseEnterElement);
            this.ugStockMinMax.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugStockMinMax_DragDrop);
            this.ugStockMinMax.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugStockMinMax_DragEnter);
            this.ugStockMinMax.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugStockMinMax_MouseDown);
            // 
            // lblSMMAttributeSet
            // 
            this.lblSMMAttributeSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSMMAttributeSet.Location = new System.Drawing.Point(120, 48);
            this.lblSMMAttributeSet.Name = "lblSMMAttributeSet";
            this.lblSMMAttributeSet.Size = new System.Drawing.Size(152, 23);
            this.lblSMMAttributeSet.TabIndex = 24;
            this.lblSMMAttributeSet.Text = "Set:";
            this.lblSMMAttributeSet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSMMAttribute
            // 
            this.lblSMMAttribute.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSMMAttribute.Location = new System.Drawing.Point(116, 16);
            this.lblSMMAttribute.Name = "lblSMMAttribute";
            this.lblSMMAttribute.Size = new System.Drawing.Size(152, 23);
            this.lblSMMAttribute.TabIndex = 22;
            this.lblSMMAttribute.Text = "Attribute:";
            this.lblSMMAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbSMMStoreAttribute
            // 
            this.cbSMMStoreAttribute.AllowDrop = true;
            this.cbSMMStoreAttribute.AllowUserAttributes = false;
            this.cbSMMStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbSMMStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSMMStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSMMStoreAttribute.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSMMStoreAttribute.Location = new System.Drawing.Point(280, 16);
            this.cbSMMStoreAttribute.Name = "cbSMMStoreAttribute";
            this.cbSMMStoreAttribute.Size = new System.Drawing.Size(211, 21);
            this.cbSMMStoreAttribute.TabIndex = 23;
            this.cbSMMStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cbSMMStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cbSMMStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cbSMMStoreAttribute_SelectionChangeCommitted);
            this.cbSMMStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cbSMMStoreAttribute_DragDrop);
            this.cbSMMStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragEnter);
            this.cbSMMStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragOver);
            // 
            // tabStoreCapacity
            // 
            this.tabStoreCapacity.Controls.Add(this.grpTabButtonSC);
            this.tabStoreCapacity.Controls.Add(this.cbSCStoreAttribute);
            this.tabStoreCapacity.Controls.Add(this.ugStoreCapacity);
            this.tabStoreCapacity.Controls.Add(this.lblSCAttribute);
            this.tabStoreCapacity.Location = new System.Drawing.Point(4, 40);
            this.tabStoreCapacity.Name = "tabStoreCapacity";
            this.tabStoreCapacity.Size = new System.Drawing.Size(720, 404);
            this.tabStoreCapacity.TabIndex = 10;
            this.tabStoreCapacity.Text = "Store Capacity";
            this.tabStoreCapacity.UseVisualStyleBackColor = true;
            // 
            // grpTabButtonSC
            // 
            this.grpTabButtonSC.Controls.Add(this.rbNoActionSC);
            this.grpTabButtonSC.Controls.Add(this.rbApplyChangesToLowerLevelsSC);
            this.grpTabButtonSC.Controls.Add(this.cbxSCInheritFromHigherLevel);
            this.grpTabButtonSC.Controls.Add(this.cbxSCApplyToLowerLevels);
            this.grpTabButtonSC.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpTabButtonSC.Location = new System.Drawing.Point(0, 370);
            this.grpTabButtonSC.Name = "grpTabButtonSC";
            this.grpTabButtonSC.Size = new System.Drawing.Size(720, 34);
            this.grpTabButtonSC.TabIndex = 30;
            this.grpTabButtonSC.TabStop = false;
            // 
            // rbNoActionSC
            // 
            this.rbNoActionSC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbNoActionSC.AutoSize = true;
            this.rbNoActionSC.Location = new System.Drawing.Point(559, 10);
            this.rbNoActionSC.Name = "rbNoActionSC";
            this.rbNoActionSC.Size = new System.Drawing.Size(72, 17);
            this.rbNoActionSC.TabIndex = 55;
            this.rbNoActionSC.Text = "No Action";
            // 
            // rbApplyChangesToLowerLevelsSC
            // 
            this.rbApplyChangesToLowerLevelsSC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbApplyChangesToLowerLevelsSC.AutoSize = true;
            this.rbApplyChangesToLowerLevelsSC.Location = new System.Drawing.Point(214, 10);
            this.rbApplyChangesToLowerLevelsSC.Name = "rbApplyChangesToLowerLevelsSC";
            this.rbApplyChangesToLowerLevelsSC.Size = new System.Drawing.Size(174, 17);
            this.rbApplyChangesToLowerLevelsSC.TabIndex = 37;
            this.rbApplyChangesToLowerLevelsSC.TabStop = true;
            this.rbApplyChangesToLowerLevelsSC.Text = "Apply Changes to Lower Levels";
            this.rbApplyChangesToLowerLevelsSC.UseVisualStyleBackColor = true;
            this.rbApplyChangesToLowerLevelsSC.CheckedChanged += new System.EventHandler(this.rbApplyChangesToLowerLevels_CheckedChanged);
            // 
            // cbxSCInheritFromHigherLevel
            // 
            this.cbxSCInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxSCInheritFromHigherLevel.AutoSize = true;
            this.cbxSCInheritFromHigherLevel.Location = new System.Drawing.Point(55, 10);
            this.cbxSCInheritFromHigherLevel.Name = "cbxSCInheritFromHigherLevel";
            this.cbxSCInheritFromHigherLevel.Size = new System.Drawing.Size(134, 17);
            this.cbxSCInheritFromHigherLevel.TabIndex = 31;
            this.cbxSCInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxSCInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxApplyInherit_CheckedChanged);
            // 
            // cbxSCApplyToLowerLevels
            // 
            this.cbxSCApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxSCApplyToLowerLevels.AutoSize = true;
            this.cbxSCApplyToLowerLevels.Location = new System.Drawing.Point(413, 10);
            this.cbxSCApplyToLowerLevels.Name = "cbxSCApplyToLowerLevels";
            this.cbxSCApplyToLowerLevels.Size = new System.Drawing.Size(121, 17);
            this.cbxSCApplyToLowerLevels.TabIndex = 30;
            this.cbxSCApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxSCApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxApplyInherit_CheckedChanged);
            // 
            // cbSCStoreAttribute
            // 
            this.cbSCStoreAttribute.AllowDrop = true;
            this.cbSCStoreAttribute.AllowUserAttributes = false;
            this.cbSCStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbSCStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSCStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSCStoreAttribute.Location = new System.Drawing.Point(280, 16);
            this.cbSCStoreAttribute.Name = "cbSCStoreAttribute";
            this.cbSCStoreAttribute.Size = new System.Drawing.Size(200, 21);
            this.cbSCStoreAttribute.TabIndex = 2;
            this.cbSCStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cbSCStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cbSCStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cbSCStoreAttribute_SelectionChangeCommitted);
            this.cbSCStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cbSCStoreAttribute_DragDrop);
            this.cbSCStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragEnter);
            this.cbSCStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragOver);
            // 
            // ugStoreCapacity
            // 
            this.ugStoreCapacity.AllowDrop = true;
            this.ugStoreCapacity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            appearance61.BackColor = System.Drawing.Color.White;
            appearance61.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance61.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugStoreCapacity.DisplayLayout.Appearance = appearance61;
            this.ugStoreCapacity.DisplayLayout.InterBandSpacing = 10;
            appearance62.BackColor = System.Drawing.Color.Transparent;
            this.ugStoreCapacity.DisplayLayout.Override.CardAreaAppearance = appearance62;
            appearance63.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance63.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance63.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance63.ForeColor = System.Drawing.Color.Black;
            appearance63.TextHAlignAsString = "Left";
            appearance63.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugStoreCapacity.DisplayLayout.Override.HeaderAppearance = appearance63;
            appearance64.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugStoreCapacity.DisplayLayout.Override.RowAppearance = appearance64;
            appearance65.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance65.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance65.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugStoreCapacity.DisplayLayout.Override.RowSelectorAppearance = appearance65;
            this.ugStoreCapacity.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugStoreCapacity.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance66.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance66.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance66.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance66.ForeColor = System.Drawing.Color.Black;
            this.ugStoreCapacity.DisplayLayout.Override.SelectedRowAppearance = appearance66;
            this.ugStoreCapacity.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugStoreCapacity.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugStoreCapacity.Location = new System.Drawing.Point(96, 48);
            this.ugStoreCapacity.Name = "ugStoreCapacity";
            this.ugStoreCapacity.Size = new System.Drawing.Size(432, 308);
            this.ugStoreCapacity.TabIndex = 3;
            this.ugStoreCapacity.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreCapacity_AfterCellUpdate);
            this.ugStoreCapacity.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugStoreCapacity_InitializeLayout);
            this.ugStoreCapacity.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugStoreCapacity_CellChange);
            this.ugStoreCapacity.BeforeCellActivate += new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugStoreCapacity_BeforeCellActivate);
            this.ugStoreCapacity.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugStoreCapacity_BeforeCellUpdate);
            this.ugStoreCapacity.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugStoreCapacity_MouseEnterElement);
            this.ugStoreCapacity.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugStoreCapacity_DragDrop);
            this.ugStoreCapacity.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugStoreCapacity_DragEnter);
            this.ugStoreCapacity.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugStoreCapacity_MouseDown);
            // 
            // lblSCAttribute
            // 
            this.lblSCAttribute.Location = new System.Drawing.Point(120, 16);
            this.lblSCAttribute.Name = "lblSCAttribute";
            this.lblSCAttribute.Size = new System.Drawing.Size(152, 23);
            this.lblSCAttribute.TabIndex = 28;
            this.lblSCAttribute.Text = "Store Attribute:";
            this.lblSCAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabDailyPercentages
            // 
            this.tabDailyPercentages.Controls.Add(this.grpTabButtonDailyPct);
            this.tabDailyPercentages.Controls.Add(this.btnDeleteDateRange);
            this.tabDailyPercentages.Controls.Add(this.btnAddDateRange);
            this.tabDailyPercentages.Controls.Add(this.ugDailyPercentages);
            this.tabDailyPercentages.Controls.Add(this.cbDPStoreAttribute);
            this.tabDailyPercentages.Controls.Add(this.lblDPAttribute);
            this.tabDailyPercentages.Location = new System.Drawing.Point(4, 40);
            this.tabDailyPercentages.Name = "tabDailyPercentages";
            this.tabDailyPercentages.Size = new System.Drawing.Size(720, 404);
            this.tabDailyPercentages.TabIndex = 12;
            this.tabDailyPercentages.Text = "Daily Percentages";
            this.tabDailyPercentages.UseVisualStyleBackColor = true;
            // 
            // grpTabButtonDailyPct
            // 
            this.grpTabButtonDailyPct.Controls.Add(this.rbNoActionDP);
            this.grpTabButtonDailyPct.Controls.Add(this.rbApplyChangesToLowerLevelsDailyPct);
            this.grpTabButtonDailyPct.Controls.Add(this.cbxDPInheritFromHigherLevel);
            this.grpTabButtonDailyPct.Controls.Add(this.cbxDPApplyToLowerLevels);
            this.grpTabButtonDailyPct.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpTabButtonDailyPct.Location = new System.Drawing.Point(0, 370);
            this.grpTabButtonDailyPct.Name = "grpTabButtonDailyPct";
            this.grpTabButtonDailyPct.Size = new System.Drawing.Size(720, 34);
            this.grpTabButtonDailyPct.TabIndex = 34;
            this.grpTabButtonDailyPct.TabStop = false;
            // 
            // rbNoActionDP
            // 
            this.rbNoActionDP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbNoActionDP.AutoSize = true;
            this.rbNoActionDP.Location = new System.Drawing.Point(559, 10);
            this.rbNoActionDP.Name = "rbNoActionDP";
            this.rbNoActionDP.Size = new System.Drawing.Size(72, 17);
            this.rbNoActionDP.TabIndex = 55;
            this.rbNoActionDP.Text = "No Action";
            // 
            // rbApplyChangesToLowerLevelsDailyPct
            // 
            this.rbApplyChangesToLowerLevelsDailyPct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbApplyChangesToLowerLevelsDailyPct.AutoSize = true;
            this.rbApplyChangesToLowerLevelsDailyPct.Location = new System.Drawing.Point(214, 10);
            this.rbApplyChangesToLowerLevelsDailyPct.Name = "rbApplyChangesToLowerLevelsDailyPct";
            this.rbApplyChangesToLowerLevelsDailyPct.Size = new System.Drawing.Size(174, 17);
            this.rbApplyChangesToLowerLevelsDailyPct.TabIndex = 36;
            this.rbApplyChangesToLowerLevelsDailyPct.TabStop = true;
            this.rbApplyChangesToLowerLevelsDailyPct.Text = "Apply Changes to Lower Levels";
            this.rbApplyChangesToLowerLevelsDailyPct.UseVisualStyleBackColor = true;
            this.rbApplyChangesToLowerLevelsDailyPct.CheckedChanged += new System.EventHandler(this.rbApplyChangesToLowerLevels_CheckedChanged);
            // 
            // cbxDPInheritFromHigherLevel
            // 
            this.cbxDPInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxDPInheritFromHigherLevel.AutoSize = true;
            this.cbxDPInheritFromHigherLevel.Location = new System.Drawing.Point(55, 10);
            this.cbxDPInheritFromHigherLevel.Name = "cbxDPInheritFromHigherLevel";
            this.cbxDPInheritFromHigherLevel.Size = new System.Drawing.Size(134, 17);
            this.cbxDPInheritFromHigherLevel.TabIndex = 35;
            this.cbxDPInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxDPInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxApplyInherit_CheckedChanged);
            // 
            // cbxDPApplyToLowerLevels
            // 
            this.cbxDPApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxDPApplyToLowerLevels.AutoSize = true;
            this.cbxDPApplyToLowerLevels.Location = new System.Drawing.Point(413, 10);
            this.cbxDPApplyToLowerLevels.Name = "cbxDPApplyToLowerLevels";
            this.cbxDPApplyToLowerLevels.Size = new System.Drawing.Size(121, 17);
            this.cbxDPApplyToLowerLevels.TabIndex = 34;
            this.cbxDPApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxDPApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxApplyInherit_CheckedChanged);
            // 
            // btnDeleteDateRange
            // 
            this.btnDeleteDateRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteDateRange.Location = new System.Drawing.Point(152, 326);
            this.btnDeleteDateRange.Name = "btnDeleteDateRange";
            this.btnDeleteDateRange.Size = new System.Drawing.Size(120, 23);
            this.btnDeleteDateRange.TabIndex = 5;
            this.btnDeleteDateRange.Text = "Delete Date Range";
            this.btnDeleteDateRange.Click += new System.EventHandler(this.btnDeleteDateRange_Click);
            // 
            // btnAddDateRange
            // 
            this.btnAddDateRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddDateRange.Location = new System.Drawing.Point(16, 326);
            this.btnAddDateRange.Name = "btnAddDateRange";
            this.btnAddDateRange.Size = new System.Drawing.Size(128, 23);
            this.btnAddDateRange.TabIndex = 4;
            this.btnAddDateRange.Text = "Add Date Range";
            this.btnAddDateRange.Click += new System.EventHandler(this.btnAddDateRange_Click);
            // 
            // ugDailyPercentages
            // 
            this.ugDailyPercentages.AllowDrop = true;
            this.ugDailyPercentages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance67.BackColor = System.Drawing.Color.White;
            appearance67.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance67.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugDailyPercentages.DisplayLayout.Appearance = appearance67;
            this.ugDailyPercentages.DisplayLayout.InterBandSpacing = 10;
            appearance68.BackColor = System.Drawing.Color.Transparent;
            this.ugDailyPercentages.DisplayLayout.Override.CardAreaAppearance = appearance68;
            appearance69.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance69.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance69.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance69.ForeColor = System.Drawing.Color.Black;
            appearance69.TextHAlignAsString = "Left";
            appearance69.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugDailyPercentages.DisplayLayout.Override.HeaderAppearance = appearance69;
            appearance70.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugDailyPercentages.DisplayLayout.Override.RowAppearance = appearance70;
            appearance71.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance71.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance71.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugDailyPercentages.DisplayLayout.Override.RowSelectorAppearance = appearance71;
            this.ugDailyPercentages.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugDailyPercentages.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance72.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance72.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance72.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance72.ForeColor = System.Drawing.Color.Black;
            this.ugDailyPercentages.DisplayLayout.Override.SelectedRowAppearance = appearance72;
            this.ugDailyPercentages.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugDailyPercentages.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugDailyPercentages.Location = new System.Drawing.Point(16, 56);
            this.ugDailyPercentages.Name = "ugDailyPercentages";
            this.ugDailyPercentages.Size = new System.Drawing.Size(688, 263);
            this.ugDailyPercentages.TabIndex = 3;
            this.ugDailyPercentages.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDailyPercentages_AfterCellUpdate);
            this.ugDailyPercentages.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugDailyPercentages_InitializeLayout);
            this.ugDailyPercentages.AfterRowsDeleted += new System.EventHandler(this.ugDailyPercentages_AfterRowsDeleted);
            this.ugDailyPercentages.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDailyPercentages_CellChange);
            this.ugDailyPercentages.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugDailyPercentages_ClickCellButton);
            this.ugDailyPercentages.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugDailyPercentages_BeforeRowsDeleted);
            this.ugDailyPercentages.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugDailyPercentages_MouseEnterElement);
            this.ugDailyPercentages.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugDailyPercentages_DragDrop);
            this.ugDailyPercentages.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugDailyPercentages_DragEnter);
            this.ugDailyPercentages.DragOver += new System.Windows.Forms.DragEventHandler(this.ugGrid_DragOver);
            this.ugDailyPercentages.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugDailyPercentages_MouseDown);
            // 
            // cbDPStoreAttribute
            // 
            this.cbDPStoreAttribute.AllowDrop = true;
            this.cbDPStoreAttribute.AllowUserAttributes = false;
            this.cbDPStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbDPStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbDPStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDPStoreAttribute.Location = new System.Drawing.Point(296, 16);
            this.cbDPStoreAttribute.Name = "cbDPStoreAttribute";
            this.cbDPStoreAttribute.Size = new System.Drawing.Size(200, 21);
            this.cbDPStoreAttribute.TabIndex = 2;
            this.cbDPStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cbDPStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cbDPStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cbDPStoreAttribute_SelectionChangeCommitted);
            this.cbDPStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cbDPStoreAttribute_DragDrop);
            this.cbDPStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cbDPStoreAttribute_DragEnter);
            this.cbDPStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cbStoreAttribute_DragOver);
            // 
            // lblDPAttribute
            // 
            this.lblDPAttribute.Location = new System.Drawing.Point(136, 16);
            this.lblDPAttribute.Name = "lblDPAttribute";
            this.lblDPAttribute.Size = new System.Drawing.Size(152, 23);
            this.lblDPAttribute.TabIndex = 32;
            this.lblDPAttribute.Text = "Store Attribute:";
            this.lblDPAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPurgeCriteria
            // 
            this.tabPurgeCriteria.AutoScroll = true;
            this.tabPurgeCriteria.Controls.Add(this.lblHtWorkUpTotTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.txtHtWorkupTotBy);
            this.tabPurgeCriteria.Controls.Add(this.lblHtVSWTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblHtReserveTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblHtPurchaseOrderTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblHtReceiptTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.txtHtVSW);
            this.tabPurgeCriteria.Controls.Add(this.txtHtReserve);
            this.tabPurgeCriteria.Controls.Add(this.txtHtPurchase);
            this.tabPurgeCriteria.Controls.Add(this.txtHtReceipt);
            this.tabPurgeCriteria.Controls.Add(this.lblHtWorkUpTot);
            this.tabPurgeCriteria.Controls.Add(this.lblHtVSW);
            this.tabPurgeCriteria.Controls.Add(this.lblHtReserve);
            this.tabPurgeCriteria.Controls.Add(this.lblHtReceipt);
            this.tabPurgeCriteria.Controls.Add(this.lblHtPurchaseOrder);
            this.tabPurgeCriteria.Controls.Add(this.lblHtDummyTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblHtDropShipTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblHtASNTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.txtHtDummy);
            this.tabPurgeCriteria.Controls.Add(this.txtHtDropShip);
            this.tabPurgeCriteria.Controls.Add(this.txtHtASN);
            this.tabPurgeCriteria.Controls.Add(this.lblHtDummy);
            this.tabPurgeCriteria.Controls.Add(this.lblHtDropShip);
            this.tabPurgeCriteria.Controls.Add(this.lblHtASN);
            this.tabPurgeCriteria.Controls.Add(this.grpTabButtonPurge);
            this.tabPurgeCriteria.Controls.Add(this.txtPurgePlans);
            this.tabPurgeCriteria.Controls.Add(this.txtPurgeWeeklyHistory);
            this.tabPurgeCriteria.Controls.Add(this.txtPurgeDailyHistory);
            this.tabPurgeCriteria.Controls.Add(this.lblPurgePlansTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblPurgeWeeklyHistoryTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblPurgeDailyHistoryTimeframe);
            this.tabPurgeCriteria.Controls.Add(this.lblPurgeWeeklyHistory);
            this.tabPurgeCriteria.Controls.Add(this.lblPurgePlans);
            this.tabPurgeCriteria.Controls.Add(this.lblPurgeDailyHistory);
            this.tabPurgeCriteria.Location = new System.Drawing.Point(4, 40);
            this.tabPurgeCriteria.Name = "tabPurgeCriteria";
            this.tabPurgeCriteria.Size = new System.Drawing.Size(720, 404);
            this.tabPurgeCriteria.TabIndex = 8;
            this.tabPurgeCriteria.Text = "Purge Criteria";
            this.tabPurgeCriteria.UseVisualStyleBackColor = true;
            // 
            // lblHtWorkUpTotTimeframe
            // 
            this.lblHtWorkUpTotTimeframe.Location = new System.Drawing.Point(337, 247);
            this.lblHtWorkUpTotTimeframe.Name = "lblHtWorkUpTotTimeframe";
            this.lblHtWorkUpTotTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtWorkUpTotTimeframe.TabIndex = 104;
            this.lblHtWorkUpTotTimeframe.Text = "weeks";
            this.lblHtWorkUpTotTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtHtWorkupTotBy
            // 
            this.txtHtWorkupTotBy.Location = new System.Drawing.Point(281, 248);
            this.txtHtWorkupTotBy.Name = "txtHtWorkupTotBy";
            this.txtHtWorkupTotBy.Size = new System.Drawing.Size(48, 20);
            this.txtHtWorkupTotBy.TabIndex = 12;
            this.txtHtWorkupTotBy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtWorkupTotBy.TextChanged += new System.EventHandler(this.txtHTWorkupTotBy_TextChanged);
            this.txtHtWorkupTotBy.Enter += new System.EventHandler(this.txtHTWorkupTotBy_Enter);
            // 
            // lblHtVSWTimeframe
            // 
            this.lblHtVSWTimeframe.Location = new System.Drawing.Point(337, 223);
            this.lblHtVSWTimeframe.Name = "lblHtVSWTimeframe";
            this.lblHtVSWTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtVSWTimeframe.TabIndex = 102;
            this.lblHtVSWTimeframe.Text = "weeks";
            this.lblHtVSWTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHtReserveTimeframe
            // 
            this.lblHtReserveTimeframe.Location = new System.Drawing.Point(337, 199);
            this.lblHtReserveTimeframe.Name = "lblHtReserveTimeframe";
            this.lblHtReserveTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtReserveTimeframe.TabIndex = 101;
            this.lblHtReserveTimeframe.Text = "weeks";
            this.lblHtReserveTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHtPurchaseOrderTimeframe
            // 
            this.lblHtPurchaseOrderTimeframe.Location = new System.Drawing.Point(337, 151);
            this.lblHtPurchaseOrderTimeframe.Name = "lblHtPurchaseOrderTimeframe";
            this.lblHtPurchaseOrderTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtPurchaseOrderTimeframe.TabIndex = 100;
            this.lblHtPurchaseOrderTimeframe.Text = "weeks";
            this.lblHtPurchaseOrderTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHtReceiptTimeframe
            // 
            this.lblHtReceiptTimeframe.Location = new System.Drawing.Point(337, 175);
            this.lblHtReceiptTimeframe.Name = "lblHtReceiptTimeframe";
            this.lblHtReceiptTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtReceiptTimeframe.TabIndex = 99;
            this.lblHtReceiptTimeframe.Text = "weeks";
            this.lblHtReceiptTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtHtVSW
            // 
            this.txtHtVSW.Location = new System.Drawing.Point(281, 224);
            this.txtHtVSW.Name = "txtHtVSW";
            this.txtHtVSW.Size = new System.Drawing.Size(48, 20);
            this.txtHtVSW.TabIndex = 11;
            this.txtHtVSW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtVSW.TextChanged += new System.EventHandler(this.txtHTVSW_TextChanged);
            this.txtHtVSW.Enter += new System.EventHandler(this.txtHTVSW_Enter);
            // 
            // txtHtReserve
            // 
            this.txtHtReserve.Location = new System.Drawing.Point(281, 200);
            this.txtHtReserve.Name = "txtHtReserve";
            this.txtHtReserve.Size = new System.Drawing.Size(48, 20);
            this.txtHtReserve.TabIndex = 10;
            this.txtHtReserve.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtReserve.TextChanged += new System.EventHandler(this.txtHTReserve_TextChanged);
            this.txtHtReserve.Enter += new System.EventHandler(this.txtHTReserve_Enter);
            // 
            // txtHtPurchase
            // 
            this.txtHtPurchase.Location = new System.Drawing.Point(281, 152);
            this.txtHtPurchase.Name = "txtHtPurchase";
            this.txtHtPurchase.Size = new System.Drawing.Size(48, 20);
            this.txtHtPurchase.TabIndex = 8;
            this.txtHtPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtPurchase.TextChanged += new System.EventHandler(this.txtHTPurchase_TextChanged);
            this.txtHtPurchase.Enter += new System.EventHandler(this.txtHTPurchase_Enter);
            // 
            // txtHtReceipt
            // 
            this.txtHtReceipt.Location = new System.Drawing.Point(281, 176);
            this.txtHtReceipt.Name = "txtHtReceipt";
            this.txtHtReceipt.Size = new System.Drawing.Size(48, 20);
            this.txtHtReceipt.TabIndex = 9;
            this.txtHtReceipt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtReceipt.TextChanged += new System.EventHandler(this.txtHTReceipt_TextChanged);
            this.txtHtReceipt.Enter += new System.EventHandler(this.txtHTReceipt_Enter);
            // 
            // lblHtWorkUpTot
            // 
            this.lblHtWorkUpTot.Location = new System.Drawing.Point(115, 250);
            this.lblHtWorkUpTot.Name = "lblHtWorkUpTot";
            this.lblHtWorkUpTot.Size = new System.Drawing.Size(161, 16);
            this.lblHtWorkUpTot.TabIndex = 94;
            this.lblHtWorkUpTot.Text = "Header Type: Workup Total Buy";
            this.lblHtWorkUpTot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtVSW
            // 
            this.lblHtVSW.Location = new System.Drawing.Point(115, 226);
            this.lblHtVSW.Name = "lblHtVSW";
            this.lblHtVSW.Size = new System.Drawing.Size(161, 16);
            this.lblHtVSW.TabIndex = 93;
            this.lblHtVSW.Text = "Header Type: VSW";
            this.lblHtVSW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtReserve
            // 
            this.lblHtReserve.Location = new System.Drawing.Point(115, 202);
            this.lblHtReserve.Name = "lblHtReserve";
            this.lblHtReserve.Size = new System.Drawing.Size(161, 16);
            this.lblHtReserve.TabIndex = 92;
            this.lblHtReserve.Text = "Header Type: Reserve";
            this.lblHtReserve.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtReceipt
            // 
            this.lblHtReceipt.Location = new System.Drawing.Point(115, 178);
            this.lblHtReceipt.Name = "lblHtReceipt";
            this.lblHtReceipt.Size = new System.Drawing.Size(161, 16);
            this.lblHtReceipt.TabIndex = 91;
            this.lblHtReceipt.Text = "Header Type: Receipt";
            this.lblHtReceipt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtPurchaseOrder
            // 
            this.lblHtPurchaseOrder.Location = new System.Drawing.Point(115, 154);
            this.lblHtPurchaseOrder.Name = "lblHtPurchaseOrder";
            this.lblHtPurchaseOrder.Size = new System.Drawing.Size(161, 16);
            this.lblHtPurchaseOrder.TabIndex = 90;
            this.lblHtPurchaseOrder.Text = "Header Type: Purchase Order";
            this.lblHtPurchaseOrder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtDummyTimeframe
            // 
            this.lblHtDummyTimeframe.Location = new System.Drawing.Point(337, 128);
            this.lblHtDummyTimeframe.Name = "lblHtDummyTimeframe";
            this.lblHtDummyTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtDummyTimeframe.TabIndex = 87;
            this.lblHtDummyTimeframe.Text = "weeks";
            this.lblHtDummyTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHtDropShipTimeframe
            // 
            this.lblHtDropShipTimeframe.Location = new System.Drawing.Point(337, 104);
            this.lblHtDropShipTimeframe.Name = "lblHtDropShipTimeframe";
            this.lblHtDropShipTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtDropShipTimeframe.TabIndex = 86;
            this.lblHtDropShipTimeframe.Text = "weeks";
            this.lblHtDropShipTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHtASNTimeframe
            // 
            this.lblHtASNTimeframe.Location = new System.Drawing.Point(337, 81);
            this.lblHtASNTimeframe.Name = "lblHtASNTimeframe";
            this.lblHtASNTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblHtASNTimeframe.TabIndex = 84;
            this.lblHtASNTimeframe.Text = "weeks";
            this.lblHtASNTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtHtDummy
            // 
            this.txtHtDummy.Location = new System.Drawing.Point(281, 129);
            this.txtHtDummy.Name = "txtHtDummy";
            this.txtHtDummy.Size = new System.Drawing.Size(48, 20);
            this.txtHtDummy.TabIndex = 7;
            this.txtHtDummy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtDummy.TextChanged += new System.EventHandler(this.txtHTDummy_TextChanged);
            this.txtHtDummy.Enter += new System.EventHandler(this.txtHTDummy_Enter);
            // 
            // txtHtDropShip
            // 
            this.txtHtDropShip.Location = new System.Drawing.Point(281, 105);
            this.txtHtDropShip.Name = "txtHtDropShip";
            this.txtHtDropShip.Size = new System.Drawing.Size(48, 20);
            this.txtHtDropShip.TabIndex = 6;
            this.txtHtDropShip.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtDropShip.TextChanged += new System.EventHandler(this.txtHTDropShip_TextChanged);
            this.txtHtDropShip.Enter += new System.EventHandler(this.txtHTDropShip_Enter);
            // 
            // txtHtASN
            // 
            this.txtHtASN.Location = new System.Drawing.Point(281, 82);
            this.txtHtASN.Name = "txtHtASN";
            this.txtHtASN.Size = new System.Drawing.Size(48, 20);
            this.txtHtASN.TabIndex = 5;
            this.txtHtASN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHtASN.TextChanged += new System.EventHandler(this.txtHTASN_TextChanged);
            this.txtHtASN.Enter += new System.EventHandler(this.txtHTASN_Enter);
            // 
            // lblHtDummy
            // 
            this.lblHtDummy.Location = new System.Drawing.Point(115, 131);
            this.lblHtDummy.Name = "lblHtDummy";
            this.lblHtDummy.Size = new System.Drawing.Size(161, 16);
            this.lblHtDummy.TabIndex = 75;
            this.lblHtDummy.Text = "Header Type: Dummy";
            this.lblHtDummy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtDropShip
            // 
            this.lblHtDropShip.Location = new System.Drawing.Point(115, 107);
            this.lblHtDropShip.Name = "lblHtDropShip";
            this.lblHtDropShip.Size = new System.Drawing.Size(161, 16);
            this.lblHtDropShip.TabIndex = 74;
            this.lblHtDropShip.Text = "Header Type: Drop Ship";
            this.lblHtDropShip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHtASN
            // 
            this.lblHtASN.Location = new System.Drawing.Point(115, 84);
            this.lblHtASN.Name = "lblHtASN";
            this.lblHtASN.Size = new System.Drawing.Size(161, 16);
            this.lblHtASN.TabIndex = 72;
            this.lblHtASN.Text = "Header Type: ASN";
            this.lblHtASN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpTabButtonPurge
            // 
            this.grpTabButtonPurge.Controls.Add(this.rbNoActionPC);
            this.grpTabButtonPurge.Controls.Add(this.rbApplyChangesToLowerLevelsPurge);
            this.grpTabButtonPurge.Controls.Add(this.cbxPCInheritFromHigherLevel);
            this.grpTabButtonPurge.Controls.Add(this.cbxPCApplyToLowerLevels);
            this.grpTabButtonPurge.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpTabButtonPurge.Location = new System.Drawing.Point(0, 370);
            this.grpTabButtonPurge.Name = "grpTabButtonPurge";
            this.grpTabButtonPurge.Size = new System.Drawing.Size(720, 34);
            this.grpTabButtonPurge.TabIndex = 51;
            this.grpTabButtonPurge.TabStop = false;
            // 
            // rbNoActionPC
            // 
            this.rbNoActionPC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbNoActionPC.AutoSize = true;
            this.rbNoActionPC.Location = new System.Drawing.Point(559, 10);
            this.rbNoActionPC.Name = "rbNoActionPC";
            this.rbNoActionPC.Size = new System.Drawing.Size(72, 17);
            this.rbNoActionPC.TabIndex = 54;
            this.rbNoActionPC.Text = "No Action";
            // 
            // rbApplyChangesToLowerLevelsPurge
            // 
            this.rbApplyChangesToLowerLevelsPurge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbApplyChangesToLowerLevelsPurge.AutoSize = true;
            this.rbApplyChangesToLowerLevelsPurge.Location = new System.Drawing.Point(214, 10);
            this.rbApplyChangesToLowerLevelsPurge.Name = "rbApplyChangesToLowerLevelsPurge";
            this.rbApplyChangesToLowerLevelsPurge.Size = new System.Drawing.Size(174, 17);
            this.rbApplyChangesToLowerLevelsPurge.TabIndex = 53;
            this.rbApplyChangesToLowerLevelsPurge.TabStop = true;
            this.rbApplyChangesToLowerLevelsPurge.Text = "Apply Changes to Lower Levels";
            this.rbApplyChangesToLowerLevelsPurge.UseVisualStyleBackColor = true;
            this.rbApplyChangesToLowerLevelsPurge.Visible = false;
            this.rbApplyChangesToLowerLevelsPurge.CheckedChanged += new System.EventHandler(this.rbApplyChangesToLowerLevels_CheckedChanged);
            // 
            // cbxPCInheritFromHigherLevel
            // 
            this.cbxPCInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxPCInheritFromHigherLevel.AutoSize = true;
            this.cbxPCInheritFromHigherLevel.Location = new System.Drawing.Point(55, 10);
            this.cbxPCInheritFromHigherLevel.Name = "cbxPCInheritFromHigherLevel";
            this.cbxPCInheritFromHigherLevel.Size = new System.Drawing.Size(134, 17);
            this.cbxPCInheritFromHigherLevel.TabIndex = 52;
            this.cbxPCInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxPCInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxPCInheritFromHigherLevel_CheckedChanged);
            // 
            // cbxPCApplyToLowerLevels
            // 
            this.cbxPCApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxPCApplyToLowerLevels.AutoSize = true;
            this.cbxPCApplyToLowerLevels.Location = new System.Drawing.Point(413, 10);
            this.cbxPCApplyToLowerLevels.Name = "cbxPCApplyToLowerLevels";
            this.cbxPCApplyToLowerLevels.Size = new System.Drawing.Size(121, 17);
            this.cbxPCApplyToLowerLevels.TabIndex = 51;
            this.cbxPCApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxPCApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxApplyInherit_CheckedChanged);
            // 
            // txtPurgePlans
            // 
            this.txtPurgePlans.Location = new System.Drawing.Point(281, 60);
            this.txtPurgePlans.Name = "txtPurgePlans";
            this.txtPurgePlans.Size = new System.Drawing.Size(48, 20);
            this.txtPurgePlans.TabIndex = 4;
            this.txtPurgePlans.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPurgePlans.TextChanged += new System.EventHandler(this.txtPurgePlans_TextChanged);
            this.txtPurgePlans.Enter += new System.EventHandler(this.txtPurgePlans_Enter);
            this.txtPurgePlans.Leave += new System.EventHandler(this.txtPurgePlans_Leave);
            // 
            // txtPurgeWeeklyHistory
            // 
            this.txtPurgeWeeklyHistory.Location = new System.Drawing.Point(281, 36);
            this.txtPurgeWeeklyHistory.Name = "txtPurgeWeeklyHistory";
            this.txtPurgeWeeklyHistory.Size = new System.Drawing.Size(48, 20);
            this.txtPurgeWeeklyHistory.TabIndex = 3;
            this.txtPurgeWeeklyHistory.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPurgeWeeklyHistory.TextChanged += new System.EventHandler(this.txtPurgeWeeklyHistory_TextChanged);
            this.txtPurgeWeeklyHistory.Enter += new System.EventHandler(this.txtPurgeWeeklyHistory_Enter);
            this.txtPurgeWeeklyHistory.Leave += new System.EventHandler(this.txtPurgeWeeklyHistory_Leave);
            // 
            // txtPurgeDailyHistory
            // 
            this.txtPurgeDailyHistory.Location = new System.Drawing.Point(281, 12);
            this.txtPurgeDailyHistory.Name = "txtPurgeDailyHistory";
            this.txtPurgeDailyHistory.Size = new System.Drawing.Size(48, 20);
            this.txtPurgeDailyHistory.TabIndex = 2;
            this.txtPurgeDailyHistory.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPurgeDailyHistory.TextChanged += new System.EventHandler(this.txtPurgeDailyHistory_TextChanged);
            this.txtPurgeDailyHistory.Enter += new System.EventHandler(this.txtPurgeDailyHistory_Enter);
            this.txtPurgeDailyHistory.Leave += new System.EventHandler(this.txtPurgeDailyHistory_Leave);
            // 
            // lblPurgePlansTimeframe
            // 
            this.lblPurgePlansTimeframe.Location = new System.Drawing.Point(337, 59);
            this.lblPurgePlansTimeframe.Name = "lblPurgePlansTimeframe";
            this.lblPurgePlansTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblPurgePlansTimeframe.TabIndex = 44;
            this.lblPurgePlansTimeframe.Text = "weeks";
            this.lblPurgePlansTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPurgeWeeklyHistoryTimeframe
            // 
            this.lblPurgeWeeklyHistoryTimeframe.Location = new System.Drawing.Point(337, 35);
            this.lblPurgeWeeklyHistoryTimeframe.Name = "lblPurgeWeeklyHistoryTimeframe";
            this.lblPurgeWeeklyHistoryTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblPurgeWeeklyHistoryTimeframe.TabIndex = 43;
            this.lblPurgeWeeklyHistoryTimeframe.Text = "weeks";
            this.lblPurgeWeeklyHistoryTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPurgeDailyHistoryTimeframe
            // 
            this.lblPurgeDailyHistoryTimeframe.Location = new System.Drawing.Point(337, 11);
            this.lblPurgeDailyHistoryTimeframe.Name = "lblPurgeDailyHistoryTimeframe";
            this.lblPurgeDailyHistoryTimeframe.Size = new System.Drawing.Size(40, 23);
            this.lblPurgeDailyHistoryTimeframe.TabIndex = 42;
            this.lblPurgeDailyHistoryTimeframe.Text = "days";
            this.lblPurgeDailyHistoryTimeframe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPurgeWeeklyHistory
            // 
            this.lblPurgeWeeklyHistory.Location = new System.Drawing.Point(115, 38);
            this.lblPurgeWeeklyHistory.Name = "lblPurgeWeeklyHistory";
            this.lblPurgeWeeklyHistory.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPurgeWeeklyHistory.Size = new System.Drawing.Size(161, 16);
            this.lblPurgeWeeklyHistory.TabIndex = 40;
            this.lblPurgeWeeklyHistory.Text = "Purge weekly history after";
            this.lblPurgeWeeklyHistory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPurgePlans
            // 
            this.lblPurgePlans.Location = new System.Drawing.Point(115, 62);
            this.lblPurgePlans.Name = "lblPurgePlans";
            this.lblPurgePlans.Size = new System.Drawing.Size(161, 16);
            this.lblPurgePlans.TabIndex = 38;
            this.lblPurgePlans.Text = "Purge OTS plans after";
            this.lblPurgePlans.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPurgeDailyHistory
            // 
            this.lblPurgeDailyHistory.Location = new System.Drawing.Point(115, 14);
            this.lblPurgeDailyHistory.Name = "lblPurgeDailyHistory";
            this.lblPurgeDailyHistory.Size = new System.Drawing.Size(161, 16);
            this.lblPurgeDailyHistory.TabIndex = 36;
            this.lblPurgeDailyHistory.Text = "Purge daily history after";
            this.lblPurgeDailyHistory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabSizeCurves
            // 
            this.tabSizeCurves.Controls.Add(this.tabSizeCurvesProperties);
            this.tabSizeCurves.Location = new System.Drawing.Point(4, 40);
            this.tabSizeCurves.Name = "tabSizeCurves";
            this.tabSizeCurves.Size = new System.Drawing.Size(720, 404);
            this.tabSizeCurves.TabIndex = 15;
            this.tabSizeCurves.Text = "Size Curves";
            this.tabSizeCurves.UseVisualStyleBackColor = true;
            // 
            // tabSizeCurvesProperties
            // 
            this.tabSizeCurvesProperties.Controls.Add(this.tabSizeCurvesCriteria);
            this.tabSizeCurvesProperties.Controls.Add(this.tabSizeCurvesTolerance);
            this.tabSizeCurvesProperties.Controls.Add(this.tabSizeCurvesSimilarStore);
            this.tabSizeCurvesProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSizeCurvesProperties.Location = new System.Drawing.Point(0, 0);
            this.tabSizeCurvesProperties.Name = "tabSizeCurvesProperties";
            this.tabSizeCurvesProperties.SelectedIndex = 0;
            this.tabSizeCurvesProperties.Size = new System.Drawing.Size(720, 404);
            this.tabSizeCurvesProperties.TabIndex = 6;
            this.tabSizeCurvesProperties.SelectedIndexChanged += new System.EventHandler(this.tabSizeCurvesProperties_SelectedIndexChanged);
            // 
            // tabSizeCurvesCriteria
            // 
            this.tabSizeCurvesCriteria.Controls.Add(this.splitContainer1);
            this.tabSizeCurvesCriteria.Controls.Add(this.panel2);
            this.tabSizeCurvesCriteria.Location = new System.Drawing.Point(4, 22);
            this.tabSizeCurvesCriteria.Name = "tabSizeCurvesCriteria";
            this.tabSizeCurvesCriteria.Padding = new System.Windows.Forms.Padding(3);
            this.tabSizeCurvesCriteria.Size = new System.Drawing.Size(712, 378);
            this.tabSizeCurvesCriteria.TabIndex = 0;
            this.tabSizeCurvesCriteria.Text = "Criteria";
            this.tabSizeCurvesCriteria.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ugSizeCurvesInheritedCriteria);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ugSizeCurvesCriteria);
            this.splitContainer1.Size = new System.Drawing.Size(706, 342);
            this.splitContainer1.SplitterDistance = 168;
            this.splitContainer1.TabIndex = 7;
            // 
            // ugSizeCurvesInheritedCriteria
            // 
            this.ugSizeCurvesInheritedCriteria.AllowDrop = true;
            appearance73.BackColor = System.Drawing.Color.White;
            appearance73.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance73.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugSizeCurvesInheritedCriteria.DisplayLayout.Appearance = appearance73;
            this.ugSizeCurvesInheritedCriteria.DisplayLayout.InterBandSpacing = 10;
            appearance74.BackColor = System.Drawing.Color.Transparent;
            this.ugSizeCurvesInheritedCriteria.DisplayLayout.Override.CardAreaAppearance = appearance74;
            appearance75.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance75.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance75.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance75.ForeColor = System.Drawing.Color.Black;
            appearance75.TextHAlignAsString = "Left";
            appearance75.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugSizeCurvesInheritedCriteria.DisplayLayout.Override.HeaderAppearance = appearance75;
            appearance76.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSizeCurvesInheritedCriteria.DisplayLayout.Override.RowAppearance = appearance76;
            appearance77.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance77.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance77.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugSizeCurvesInheritedCriteria.DisplayLayout.Override.RowSelectorAppearance = appearance77;
            this.ugSizeCurvesInheritedCriteria.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugSizeCurvesInheritedCriteria.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance78.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance78.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance78.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance78.ForeColor = System.Drawing.Color.Black;
            this.ugSizeCurvesInheritedCriteria.DisplayLayout.Override.SelectedRowAppearance = appearance78;
            this.ugSizeCurvesInheritedCriteria.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSizeCurvesInheritedCriteria.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugSizeCurvesInheritedCriteria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugSizeCurvesInheritedCriteria.Location = new System.Drawing.Point(0, 0);
            this.ugSizeCurvesInheritedCriteria.Name = "ugSizeCurvesInheritedCriteria";
            this.ugSizeCurvesInheritedCriteria.Size = new System.Drawing.Size(706, 168);
            this.ugSizeCurvesInheritedCriteria.TabIndex = 5;
            this.ugSizeCurvesInheritedCriteria.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesInheritedCriteria_AfterCellUpdate);
            this.ugSizeCurvesInheritedCriteria.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSizeCurvesInheritedCriteria_InitializeLayout);
            this.ugSizeCurvesInheritedCriteria.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugSizeCurvesInheritedCriteria_InitializeRow);
            this.ugSizeCurvesInheritedCriteria.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesInheritedCriteria_CellChange);
            this.ugSizeCurvesInheritedCriteria.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugSizeCurvesInheritedCriteria_BeforeExitEditMode);
            this.ugSizeCurvesInheritedCriteria.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugSizeCurvesInheritedCriteria_MouseEnterElement);
            this.ugSizeCurvesInheritedCriteria.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugSizeCurvesInheritedCriteria_MouseDown);
            // 
            // ugSizeCurvesCriteria
            // 
            this.ugSizeCurvesCriteria.AllowDrop = true;
            appearance79.BackColor = System.Drawing.Color.White;
            appearance79.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance79.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugSizeCurvesCriteria.DisplayLayout.Appearance = appearance79;
            this.ugSizeCurvesCriteria.DisplayLayout.InterBandSpacing = 10;
            appearance80.BackColor = System.Drawing.Color.Transparent;
            this.ugSizeCurvesCriteria.DisplayLayout.Override.CardAreaAppearance = appearance80;
            appearance81.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance81.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance81.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance81.ForeColor = System.Drawing.Color.Black;
            appearance81.TextHAlignAsString = "Left";
            appearance81.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugSizeCurvesCriteria.DisplayLayout.Override.HeaderAppearance = appearance81;
            appearance82.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSizeCurvesCriteria.DisplayLayout.Override.RowAppearance = appearance82;
            appearance83.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance83.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance83.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugSizeCurvesCriteria.DisplayLayout.Override.RowSelectorAppearance = appearance83;
            this.ugSizeCurvesCriteria.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugSizeCurvesCriteria.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance84.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance84.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance84.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance84.ForeColor = System.Drawing.Color.Black;
            this.ugSizeCurvesCriteria.DisplayLayout.Override.SelectedRowAppearance = appearance84;
            this.ugSizeCurvesCriteria.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSizeCurvesCriteria.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugSizeCurvesCriteria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugSizeCurvesCriteria.Location = new System.Drawing.Point(0, 0);
            this.ugSizeCurvesCriteria.Name = "ugSizeCurvesCriteria";
            this.ugSizeCurvesCriteria.Size = new System.Drawing.Size(706, 170);
            this.ugSizeCurvesCriteria.TabIndex = 6;
            this.ugSizeCurvesCriteria.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesCriteria_AfterCellUpdate);
            this.ugSizeCurvesCriteria.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSizeCurvesCriteria_InitializeLayout);
            this.ugSizeCurvesCriteria.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugSizeCurvesCriteria_InitializeRow);
            this.ugSizeCurvesCriteria.AfterRowsDeleted += new System.EventHandler(this.ugSizeCurvesCriteria_AfterRowsDeleted);
            this.ugSizeCurvesCriteria.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugSizeCurvesCriteria_AfterRowInsert);
            this.ugSizeCurvesCriteria.AfterRowUpdate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugSizeCurvesCriteria_AfterRowUpdate);
            this.ugSizeCurvesCriteria.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesCriteria_CellChange);
            this.ugSizeCurvesCriteria.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesCriteria_ClickCellButton);
            this.ugSizeCurvesCriteria.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesCriteria_AfterCellListCloseUp);
            this.ugSizeCurvesCriteria.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugSizeCurvesCriteria_BeforeExitEditMode);
            this.ugSizeCurvesCriteria.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ugSizeCurvesCriteria_BeforeRowInsert);
            this.ugSizeCurvesCriteria.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugSizeCurvesCriteria_MouseEnterElement);
            this.ugSizeCurvesCriteria.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugSizeCurvesCriteria_MouseDown);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cbxSZCriteriaApplyToLowerLevels);
            this.panel2.Controls.Add(this.cbxSZCriteriaInheritFromHigherLevel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 345);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(706, 30);
            this.panel2.TabIndex = 7;
            // 
            // cbxSZCriteriaApplyToLowerLevels
            // 
            this.cbxSZCriteriaApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSZCriteriaApplyToLowerLevels.Location = new System.Drawing.Point(571, 3);
            this.cbxSZCriteriaApplyToLowerLevels.Name = "cbxSZCriteriaApplyToLowerLevels";
            this.cbxSZCriteriaApplyToLowerLevels.Size = new System.Drawing.Size(136, 24);
            this.cbxSZCriteriaApplyToLowerLevels.TabIndex = 27;
            this.cbxSZCriteriaApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxSZCriteriaApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxSZCriteriaApplyToLowerLevels_CheckedChanged);
            // 
            // cbxSZCriteriaInheritFromHigherLevel
            // 
            this.cbxSZCriteriaInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSZCriteriaInheritFromHigherLevel.Location = new System.Drawing.Point(421, 3);
            this.cbxSZCriteriaInheritFromHigherLevel.Name = "cbxSZCriteriaInheritFromHigherLevel";
            this.cbxSZCriteriaInheritFromHigherLevel.Size = new System.Drawing.Size(144, 24);
            this.cbxSZCriteriaInheritFromHigherLevel.TabIndex = 28;
            this.cbxSZCriteriaInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxSZCriteriaInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxSZCriteriaInheritFromHigherLevel_CheckedChanged);
            // 
            // tabSizeCurvesTolerance
            // 
            this.tabSizeCurvesTolerance.Controls.Add(this.panel1);
            this.tabSizeCurvesTolerance.Controls.Add(this.gbxSCTMinMaxTolerancePct);
            this.tabSizeCurvesTolerance.Controls.Add(this.gbxSCTApplyChainSales);
            this.tabSizeCurvesTolerance.Controls.Add(this.gbxSCTHigherLevelSalesTolerance);
            this.tabSizeCurvesTolerance.Location = new System.Drawing.Point(4, 22);
            this.tabSizeCurvesTolerance.Name = "tabSizeCurvesTolerance";
            this.tabSizeCurvesTolerance.Padding = new System.Windows.Forms.Padding(3);
            this.tabSizeCurvesTolerance.Size = new System.Drawing.Size(712, 378);
            this.tabSizeCurvesTolerance.TabIndex = 1;
            this.tabSizeCurvesTolerance.Text = "Tolerance";
            this.tabSizeCurvesTolerance.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbxSZToleranceApplyToLowerLevels);
            this.panel1.Controls.Add(this.cbxSZToleranceInheritFromHigherLevel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 345);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(706, 30);
            this.panel1.TabIndex = 8;
            // 
            // cbxSZToleranceApplyToLowerLevels
            // 
            this.cbxSZToleranceApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSZToleranceApplyToLowerLevels.Location = new System.Drawing.Point(571, 3);
            this.cbxSZToleranceApplyToLowerLevels.Name = "cbxSZToleranceApplyToLowerLevels";
            this.cbxSZToleranceApplyToLowerLevels.Size = new System.Drawing.Size(136, 24);
            this.cbxSZToleranceApplyToLowerLevels.TabIndex = 27;
            this.cbxSZToleranceApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxSZToleranceApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxSZToleranceApplyToLowerLevels_CheckedChanged);
            // 
            // cbxSZToleranceInheritFromHigherLevel
            // 
            this.cbxSZToleranceInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSZToleranceInheritFromHigherLevel.Location = new System.Drawing.Point(421, 3);
            this.cbxSZToleranceInheritFromHigherLevel.Name = "cbxSZToleranceInheritFromHigherLevel";
            this.cbxSZToleranceInheritFromHigherLevel.Size = new System.Drawing.Size(144, 24);
            this.cbxSZToleranceInheritFromHigherLevel.TabIndex = 28;
            this.cbxSZToleranceInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxSZToleranceInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxSZToleranceInheritFromHigherLevel_CheckedChanged);
            // 
            // gbxSCTMinMaxTolerancePct
            // 
            this.gbxSCTMinMaxTolerancePct.Controls.Add(this.cbxApplyMinToZeroTolerance);
            this.gbxSCTMinMaxTolerancePct.Controls.Add(this.txtSCTMaximumPct);
            this.gbxSCTMinMaxTolerancePct.Controls.Add(this.lblSCTMaximumPct);
            this.gbxSCTMinMaxTolerancePct.Controls.Add(this.txtSCTMinimumPct);
            this.gbxSCTMinMaxTolerancePct.Controls.Add(this.lblSCTMinimumPct);
            this.gbxSCTMinMaxTolerancePct.Location = new System.Drawing.Point(19, 251);
            this.gbxSCTMinMaxTolerancePct.Name = "gbxSCTMinMaxTolerancePct";
            this.gbxSCTMinMaxTolerancePct.Size = new System.Drawing.Size(578, 88);
            this.gbxSCTMinMaxTolerancePct.TabIndex = 6;
            this.gbxSCTMinMaxTolerancePct.TabStop = false;
            this.gbxSCTMinMaxTolerancePct.Text = "Min/Max Tolerance %";
            // 
            // cbxApplyMinToZeroTolerance
            // 
            this.cbxApplyMinToZeroTolerance.AutoSize = true;
            this.cbxApplyMinToZeroTolerance.Enabled = false;
            this.cbxApplyMinToZeroTolerance.Location = new System.Drawing.Point(58, 65);
            this.cbxApplyMinToZeroTolerance.Name = "cbxApplyMinToZeroTolerance";
            this.cbxApplyMinToZeroTolerance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbxApplyMinToZeroTolerance.Size = new System.Drawing.Size(184, 17);
            this.cbxApplyMinToZeroTolerance.TabIndex = 8;
            this.cbxApplyMinToZeroTolerance.Text = "Apply Minimum to Zero Tolerance";
            this.cbxApplyMinToZeroTolerance.UseVisualStyleBackColor = true;
            this.cbxApplyMinToZeroTolerance.CheckedChanged += new System.EventHandler(this.cbxApplyMinToZeroTolerance_CheckChanged);
            // 
            // txtSCTMaximumPct
            // 
            this.txtSCTMaximumPct.Location = new System.Drawing.Point(368, 28);
            this.txtSCTMaximumPct.Name = "txtSCTMaximumPct";
            this.txtSCTMaximumPct.Size = new System.Drawing.Size(67, 20);
            this.txtSCTMaximumPct.TabIndex = 6;
            this.txtSCTMaximumPct.TextChanged += new System.EventHandler(this.txtSCTMaximumPct_TextChanged);
            // 
            // lblSCTMaximumPct
            // 
            this.lblSCTMaximumPct.AutoSize = true;
            this.lblSCTMaximumPct.Location = new System.Drawing.Point(300, 31);
            this.lblSCTMaximumPct.Name = "lblSCTMaximumPct";
            this.lblSCTMaximumPct.Size = new System.Drawing.Size(62, 13);
            this.lblSCTMaximumPct.TabIndex = 5;
            this.lblSCTMaximumPct.Text = "Maximum %";
            this.lblSCTMaximumPct.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtSCTMinimumPct
            // 
            this.txtSCTMinimumPct.Location = new System.Drawing.Point(175, 28);
            this.txtSCTMinimumPct.Name = "txtSCTMinimumPct";
            this.txtSCTMinimumPct.Size = new System.Drawing.Size(67, 20);
            this.txtSCTMinimumPct.TabIndex = 4;
            this.txtSCTMinimumPct.TextChanged += new System.EventHandler(this.txtSCTMinimumPct_TextChanged);
            // 
            // lblSCTMinimumPct
            // 
            this.lblSCTMinimumPct.AutoSize = true;
            this.lblSCTMinimumPct.Location = new System.Drawing.Point(110, 31);
            this.lblSCTMinimumPct.Name = "lblSCTMinimumPct";
            this.lblSCTMinimumPct.Size = new System.Drawing.Size(59, 13);
            this.lblSCTMinimumPct.TabIndex = 3;
            this.lblSCTMinimumPct.Text = "Minimum %";
            this.lblSCTMinimumPct.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // gbxSCTApplyChainSales
            // 
            this.gbxSCTApplyChainSales.Controls.Add(this.gbxSCTIndexUnits);
            this.gbxSCTApplyChainSales.Controls.Add(this.txtSCTSalesTolerance);
            this.gbxSCTApplyChainSales.Controls.Add(this.lblSCTSalesTolerance);
            this.gbxSCTApplyChainSales.Location = new System.Drawing.Point(19, 143);
            this.gbxSCTApplyChainSales.Name = "gbxSCTApplyChainSales";
            this.gbxSCTApplyChainSales.Size = new System.Drawing.Size(578, 73);
            this.gbxSCTApplyChainSales.TabIndex = 5;
            this.gbxSCTApplyChainSales.TabStop = false;
            this.gbxSCTApplyChainSales.Text = "Apply Chain/Set Sales";
            // 
            // gbxSCTIndexUnits
            // 
            this.gbxSCTIndexUnits.Controls.Add(this.rdoSCTIndexToAverage);
            this.gbxSCTIndexUnits.Controls.Add(this.rdoSCTUnits);
            this.gbxSCTIndexUnits.Location = new System.Drawing.Point(305, 17);
            this.gbxSCTIndexUnits.Name = "gbxSCTIndexUnits";
            this.gbxSCTIndexUnits.Size = new System.Drawing.Size(237, 39);
            this.gbxSCTIndexUnits.TabIndex = 5;
            this.gbxSCTIndexUnits.TabStop = false;
            // 
            // rdoSCTIndexToAverage
            // 
            this.rdoSCTIndexToAverage.AutoSize = true;
            this.rdoSCTIndexToAverage.Location = new System.Drawing.Point(23, 13);
            this.rdoSCTIndexToAverage.Name = "rdoSCTIndexToAverage";
            this.rdoSCTIndexToAverage.Size = new System.Drawing.Size(106, 17);
            this.rdoSCTIndexToAverage.TabIndex = 3;
            this.rdoSCTIndexToAverage.TabStop = true;
            this.rdoSCTIndexToAverage.Text = "Index to Average";
            this.rdoSCTIndexToAverage.UseVisualStyleBackColor = true;
            this.rdoSCTIndexToAverage.CheckedChanged += new System.EventHandler(this.rdoSCTIndexToAverage_CheckedChanged);
            // 
            // rdoSCTUnits
            // 
            this.rdoSCTUnits.AutoSize = true;
            this.rdoSCTUnits.Location = new System.Drawing.Point(161, 13);
            this.rdoSCTUnits.Name = "rdoSCTUnits";
            this.rdoSCTUnits.Size = new System.Drawing.Size(49, 17);
            this.rdoSCTUnits.TabIndex = 4;
            this.rdoSCTUnits.TabStop = true;
            this.rdoSCTUnits.Text = "Units";
            this.rdoSCTUnits.UseVisualStyleBackColor = true;
            this.rdoSCTUnits.CheckedChanged += new System.EventHandler(this.rdoSCTUnits_CheckedChanged);
            // 
            // txtSCTSalesTolerance
            // 
            this.txtSCTSalesTolerance.Location = new System.Drawing.Point(175, 29);
            this.txtSCTSalesTolerance.Name = "txtSCTSalesTolerance";
            this.txtSCTSalesTolerance.Size = new System.Drawing.Size(67, 20);
            this.txtSCTSalesTolerance.TabIndex = 2;
            this.txtSCTSalesTolerance.TextChanged += new System.EventHandler(this.txtSCTSalesTolerance_TextChanged);
            // 
            // lblSCTSalesTolerance
            // 
            this.lblSCTSalesTolerance.AutoSize = true;
            this.lblSCTSalesTolerance.Location = new System.Drawing.Point(85, 32);
            this.lblSCTSalesTolerance.Name = "lblSCTSalesTolerance";
            this.lblSCTSalesTolerance.Size = new System.Drawing.Size(84, 13);
            this.lblSCTSalesTolerance.TabIndex = 1;
            this.lblSCTSalesTolerance.Text = "Sales Tolerance";
            this.lblSCTSalesTolerance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // gbxSCTHigherLevelSalesTolerance
            // 
            this.gbxSCTHigherLevelSalesTolerance.Controls.Add(this.cboSCTHighestLevel);
            this.gbxSCTHigherLevelSalesTolerance.Controls.Add(this.lblSCTMinimumAvgPerSize);
            this.gbxSCTHigherLevelSalesTolerance.Controls.Add(this.lblSCTHighestLevel);
            this.gbxSCTHigherLevelSalesTolerance.Controls.Add(this.txtSCTMinimumAvgPerSize);
            this.gbxSCTHigherLevelSalesTolerance.Location = new System.Drawing.Point(19, 35);
            this.gbxSCTHigherLevelSalesTolerance.Name = "gbxSCTHigherLevelSalesTolerance";
            this.gbxSCTHigherLevelSalesTolerance.Size = new System.Drawing.Size(578, 73);
            this.gbxSCTHigherLevelSalesTolerance.TabIndex = 4;
            this.gbxSCTHigherLevelSalesTolerance.TabStop = false;
            this.gbxSCTHigherLevelSalesTolerance.Text = "Higher Level Sales Tolerance";
            // 
            // cboSCTHighestLevel
            // 
            this.cboSCTHighestLevel.AutoAdjust = true;
            this.cboSCTHighestLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSCTHighestLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSCTHighestLevel.DataSource = null;
            this.cboSCTHighestLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSCTHighestLevel.DropDownWidth = 195;
            this.cboSCTHighestLevel.FormattingEnabled = false;
            this.cboSCTHighestLevel.IgnoreFocusLost = false;
            this.cboSCTHighestLevel.ItemHeight = 13;
            this.cboSCTHighestLevel.Location = new System.Drawing.Point(350, 28);
            this.cboSCTHighestLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cboSCTHighestLevel.MaxDropDownItems = 25;
            this.cboSCTHighestLevel.Name = "cboSCTHighestLevel";
            this.cboSCTHighestLevel.SetToolTip = "";
            this.cboSCTHighestLevel.Size = new System.Drawing.Size(195, 21);
            this.cboSCTHighestLevel.TabIndex = 3;
            this.cboSCTHighestLevel.Tag = null;
            this.cboSCTHighestLevel.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSCTHighestLevel_MIDComboBoxPropertiesChangedEvent);
            this.cboSCTHighestLevel.SelectionChangeCommitted += new System.EventHandler(this.cboSCTHighestLevel_SelectionChangeCommitted);
            // 
            // lblSCTMinimumAvgPerSize
            // 
            this.lblSCTMinimumAvgPerSize.AutoSize = true;
            this.lblSCTMinimumAvgPerSize.Location = new System.Drawing.Point(37, 31);
            this.lblSCTMinimumAvgPerSize.Name = "lblSCTMinimumAvgPerSize";
            this.lblSCTMinimumAvgPerSize.Size = new System.Drawing.Size(132, 13);
            this.lblSCTMinimumAvgPerSize.TabIndex = 0;
            this.lblSCTMinimumAvgPerSize.Text = "Minimum Average per Size";
            this.lblSCTMinimumAvgPerSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSCTHighestLevel
            // 
            this.lblSCTHighestLevel.AutoSize = true;
            this.lblSCTHighestLevel.Location = new System.Drawing.Point(272, 31);
            this.lblSCTHighestLevel.Name = "lblSCTHighestLevel";
            this.lblSCTHighestLevel.Size = new System.Drawing.Size(72, 13);
            this.lblSCTHighestLevel.TabIndex = 2;
            this.lblSCTHighestLevel.Text = "Highest Level";
            this.lblSCTHighestLevel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtSCTMinimumAvgPerSize
            // 
            this.txtSCTMinimumAvgPerSize.Location = new System.Drawing.Point(175, 28);
            this.txtSCTMinimumAvgPerSize.Name = "txtSCTMinimumAvgPerSize";
            this.txtSCTMinimumAvgPerSize.Size = new System.Drawing.Size(67, 20);
            this.txtSCTMinimumAvgPerSize.TabIndex = 1;
            this.txtSCTMinimumAvgPerSize.TextChanged += new System.EventHandler(this.txtSCTMinimumAvgPerSize_TextChanged);
            // 
            // tabSizeCurvesSimilarStore
            // 
            this.tabSizeCurvesSimilarStore.Controls.Add(this.ugSizeCurvesSimilarStore);
            this.tabSizeCurvesSimilarStore.Controls.Add(this.panel4);
            this.tabSizeCurvesSimilarStore.Controls.Add(this.panel3);
            this.tabSizeCurvesSimilarStore.Location = new System.Drawing.Point(4, 22);
            this.tabSizeCurvesSimilarStore.Name = "tabSizeCurvesSimilarStore";
            this.tabSizeCurvesSimilarStore.Size = new System.Drawing.Size(712, 378);
            this.tabSizeCurvesSimilarStore.TabIndex = 2;
            this.tabSizeCurvesSimilarStore.Text = "Similar Store";
            this.tabSizeCurvesSimilarStore.UseVisualStyleBackColor = true;
            // 
            // ugSizeCurvesSimilarStore
            // 
            this.ugSizeCurvesSimilarStore.AllowDrop = true;
            appearance85.BackColor = System.Drawing.Color.White;
            appearance85.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance85.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugSizeCurvesSimilarStore.DisplayLayout.Appearance = appearance85;
            this.ugSizeCurvesSimilarStore.DisplayLayout.InterBandSpacing = 10;
            appearance86.BackColor = System.Drawing.Color.Transparent;
            this.ugSizeCurvesSimilarStore.DisplayLayout.Override.CardAreaAppearance = appearance86;
            appearance87.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance87.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance87.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance87.ForeColor = System.Drawing.Color.Black;
            appearance87.TextHAlignAsString = "Left";
            appearance87.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugSizeCurvesSimilarStore.DisplayLayout.Override.HeaderAppearance = appearance87;
            appearance88.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSizeCurvesSimilarStore.DisplayLayout.Override.RowAppearance = appearance88;
            appearance89.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance89.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance89.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugSizeCurvesSimilarStore.DisplayLayout.Override.RowSelectorAppearance = appearance89;
            this.ugSizeCurvesSimilarStore.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugSizeCurvesSimilarStore.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance90.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance90.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance90.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance90.ForeColor = System.Drawing.Color.Black;
            this.ugSizeCurvesSimilarStore.DisplayLayout.Override.SelectedRowAppearance = appearance90;
            this.ugSizeCurvesSimilarStore.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSizeCurvesSimilarStore.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugSizeCurvesSimilarStore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugSizeCurvesSimilarStore.Location = new System.Drawing.Point(0, 38);
            this.ugSizeCurvesSimilarStore.Name = "ugSizeCurvesSimilarStore";
            this.ugSizeCurvesSimilarStore.Size = new System.Drawing.Size(712, 310);
            this.ugSizeCurvesSimilarStore.TabIndex = 6;
            this.ugSizeCurvesSimilarStore.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSizeCurvesSimilarStore_InitializeLayout);
            this.ugSizeCurvesSimilarStore.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesSimilarStore_CellChange);
            this.ugSizeCurvesSimilarStore.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSizeCurvesSimilarStore_ClickCellButton);
            this.ugSizeCurvesSimilarStore.BeforeCellListDropDown += new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugSizeCurvesSimilarStore_BeforeCellListDropDown);
            this.ugSizeCurvesSimilarStore.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugSizeCurvesSimilarStore_BeforeExitEditMode);
            this.ugSizeCurvesSimilarStore.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugSizeCurvesSimilarStore_MouseEnterElement);
            this.ugSizeCurvesSimilarStore.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugSizeCurvesSimilarStore_DragDrop);
            this.ugSizeCurvesSimilarStore.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugSizeCurvesSimilarStore_DragEnter);
            this.ugSizeCurvesSimilarStore.DragOver += new System.Windows.Forms.DragEventHandler(this.ugGrid_DragOver);
            this.ugSizeCurvesSimilarStore.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ugSizeCurvesSimilarStore_KeyDown);
            this.ugSizeCurvesSimilarStore.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugSizeCurvesSimilarStore_MouseDown);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.cbxSZSimilarStoreApplyToLowerLevels);
            this.panel4.Controls.Add(this.cbxSZSimilarStoreInheritFromHigherLevel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 348);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(712, 30);
            this.panel4.TabIndex = 28;
            // 
            // cbxSZSimilarStoreApplyToLowerLevels
            // 
            this.cbxSZSimilarStoreApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSZSimilarStoreApplyToLowerLevels.Location = new System.Drawing.Point(577, 3);
            this.cbxSZSimilarStoreApplyToLowerLevels.Name = "cbxSZSimilarStoreApplyToLowerLevels";
            this.cbxSZSimilarStoreApplyToLowerLevels.Size = new System.Drawing.Size(136, 24);
            this.cbxSZSimilarStoreApplyToLowerLevels.TabIndex = 27;
            this.cbxSZSimilarStoreApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxSZSimilarStoreApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxSZSimilarStoreApplyToLowerLevels_CheckedChanged);
            // 
            // cbxSZSimilarStoreInheritFromHigherLevel
            // 
            this.cbxSZSimilarStoreInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSZSimilarStoreInheritFromHigherLevel.Location = new System.Drawing.Point(427, 3);
            this.cbxSZSimilarStoreInheritFromHigherLevel.Name = "cbxSZSimilarStoreInheritFromHigherLevel";
            this.cbxSZSimilarStoreInheritFromHigherLevel.Size = new System.Drawing.Size(144, 24);
            this.cbxSZSimilarStoreInheritFromHigherLevel.TabIndex = 28;
            this.cbxSZSimilarStoreInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxSZSimilarStoreInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxSZSimilarStoreInheritFromHigherLevel_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cbSZStoreAttribute);
            this.panel3.Controls.Add(this.lblSZAttribute);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(712, 38);
            this.panel3.TabIndex = 27;
            // 
            // cbSZStoreAttribute
            // 
            this.cbSZStoreAttribute.AllowDrop = true;
            this.cbSZStoreAttribute.AllowUserAttributes = false;
            this.cbSZStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbSZStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbSZStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSZStoreAttribute.Location = new System.Drawing.Point(379, 8);
            this.cbSZStoreAttribute.Name = "cbSZStoreAttribute";
            this.cbSZStoreAttribute.Size = new System.Drawing.Size(200, 21);
            this.cbSZStoreAttribute.TabIndex = 25;
            this.cbSZStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cbSZStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cbSZStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cbSZStoreAttribute_SelectionChangeCommitted);
            // 
            // lblSZAttribute
            // 
            this.lblSZAttribute.Location = new System.Drawing.Point(215, 8);
            this.lblSZAttribute.Name = "lblSZAttribute";
            this.lblSZAttribute.Size = new System.Drawing.Size(152, 23);
            this.lblSZAttribute.TabIndex = 26;
            this.lblSZAttribute.Text = "Store Attribute:";
            this.lblSZAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabSizeOutOfStock
            // 
            this.tabSizeOutOfStock.Controls.Add(this.tabSizeOutOfStockProperties);
            this.tabSizeOutOfStock.Location = new System.Drawing.Point(4, 40);
            this.tabSizeOutOfStock.Name = "tabSizeOutOfStock";
            this.tabSizeOutOfStock.Size = new System.Drawing.Size(720, 404);
            this.tabSizeOutOfStock.TabIndex = 16;
            this.tabSizeOutOfStock.Text = "Out of Stock";
            this.tabSizeOutOfStock.UseVisualStyleBackColor = true;
            // 
            // tabSizeOutOfStockProperties
            // 
            this.tabSizeOutOfStockProperties.Controls.Add(this.tabSizeOutOfStockParms);
            this.tabSizeOutOfStockProperties.Controls.Add(this.tabSizeSellThru);
            this.tabSizeOutOfStockProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSizeOutOfStockProperties.Location = new System.Drawing.Point(0, 0);
            this.tabSizeOutOfStockProperties.Name = "tabSizeOutOfStockProperties";
            this.tabSizeOutOfStockProperties.SelectedIndex = 0;
            this.tabSizeOutOfStockProperties.Size = new System.Drawing.Size(720, 404);
            this.tabSizeOutOfStockProperties.TabIndex = 0;
            this.tabSizeOutOfStockProperties.SelectedIndexChanged += new System.EventHandler(this.tabSizeOutOfStockProperties_SelectedIndexChanged);
            // 
            // tabSizeOutOfStockParms
            // 
            this.tabSizeOutOfStockParms.Controls.Add(this.mcColorSizeByAttribute);
            this.tabSizeOutOfStockParms.Controls.Add(this.panel7);
            this.tabSizeOutOfStockParms.Controls.Add(this.panel5);
            this.tabSizeOutOfStockParms.Location = new System.Drawing.Point(4, 22);
            this.tabSizeOutOfStockParms.Name = "tabSizeOutOfStockParms";
            this.tabSizeOutOfStockParms.Padding = new System.Windows.Forms.Padding(3);
            this.tabSizeOutOfStockParms.Size = new System.Drawing.Size(712, 378);
            this.tabSizeOutOfStockParms.TabIndex = 0;
            this.tabSizeOutOfStockParms.Text = "Out of Stock";
            this.tabSizeOutOfStockParms.UseVisualStyleBackColor = true;
            // 
            // mcColorSizeByAttribute
            // 
            this.mcColorSizeByAttribute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mcColorSizeByAttribute.Location = new System.Drawing.Point(3, 41);
            this.mcColorSizeByAttribute.Name = "mcColorSizeByAttribute";
            this.mcColorSizeByAttribute.Size = new System.Drawing.Size(706, 304);
            this.mcColorSizeByAttribute.TabIndex = 0;
            this.mcColorSizeByAttribute.Text = "midColorSizeByAttribute1";
            this.mcColorSizeByAttribute.ValueChanged += new MIDRetail.Windows.Controls.MIDColorSizeByAttribute.ValueChangedHandler(this.mcColorSizeByAttribute_ValueChanged);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.cboSZOOSSizeGroup);
            this.panel7.Controls.Add(this.lblSZOOSAttributeSet);
            this.panel7.Controls.Add(this.lblSZOOSSizeGroup);
            this.panel7.Controls.Add(this.cboSZOOSStoreAttribute);
            this.panel7.Controls.Add(this.picSZOOSSizeGroupFilter);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(706, 38);
            this.panel7.TabIndex = 2;
            // 
            // cboSZOOSSizeGroup
            // 
            this.cboSZOOSSizeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSZOOSSizeGroup.AutoAdjust = true;
            this.cboSZOOSSizeGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSZOOSSizeGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSZOOSSizeGroup.DataSource = null;
            this.cboSZOOSSizeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSZOOSSizeGroup.DropDownWidth = 174;
            this.cboSZOOSSizeGroup.FormattingEnabled = false;
            this.cboSZOOSSizeGroup.IgnoreFocusLost = false;
            this.cboSZOOSSizeGroup.ItemHeight = 13;
            this.cboSZOOSSizeGroup.Location = new System.Drawing.Point(119, 8);
            this.cboSZOOSSizeGroup.Margin = new System.Windows.Forms.Padding(0);
            this.cboSZOOSSizeGroup.MaxDropDownItems = 25;
            this.cboSZOOSSizeGroup.Name = "cboSZOOSSizeGroup";
            this.cboSZOOSSizeGroup.SetToolTip = "";
            this.cboSZOOSSizeGroup.Size = new System.Drawing.Size(174, 21);
            this.cboSZOOSSizeGroup.TabIndex = 54;
            this.cboSZOOSSizeGroup.Tag = null;
            this.cboSZOOSSizeGroup.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSZOOSSizeGroup_MIDComboBoxPropertiesChangedEvent);
            this.cboSZOOSSizeGroup.SelectionChangeCommitted += new System.EventHandler(this.cboSZOOSSizeGroup_SelectionChangeCommitted);
            // 
            // lblSZOOSAttributeSet
            // 
            this.lblSZOOSAttributeSet.AutoSize = true;
            this.lblSZOOSAttributeSet.Location = new System.Drawing.Point(311, 11);
            this.lblSZOOSAttributeSet.Name = "lblSZOOSAttributeSet";
            this.lblSZOOSAttributeSet.Size = new System.Drawing.Size(74, 13);
            this.lblSZOOSAttributeSet.TabIndex = 58;
            this.lblSZOOSAttributeSet.Text = "Store Attribute";
            // 
            // lblSZOOSSizeGroup
            // 
            this.lblSZOOSSizeGroup.AutoSize = true;
            this.lblSZOOSSizeGroup.Location = new System.Drawing.Point(13, 11);
            this.lblSZOOSSizeGroup.Name = "lblSZOOSSizeGroup";
            this.lblSZOOSSizeGroup.Size = new System.Drawing.Size(59, 13);
            this.lblSZOOSSizeGroup.TabIndex = 57;
            this.lblSZOOSSizeGroup.Text = "Size Group";
            // 
            // cboSZOOSStoreAttribute
            // 
            this.cboSZOOSStoreAttribute.AllowDrop = true;
            this.cboSZOOSStoreAttribute.AllowUserAttributes = false;
            this.cboSZOOSStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSZOOSStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSZOOSStoreAttribute.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboSZOOSStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSZOOSStoreAttribute.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSZOOSStoreAttribute.Location = new System.Drawing.Point(389, 8);
            this.cboSZOOSStoreAttribute.Name = "cboSZOOSStoreAttribute";
            this.cboSZOOSStoreAttribute.Size = new System.Drawing.Size(213, 21);
            this.cboSZOOSStoreAttribute.TabIndex = 56;
            this.cboSZOOSStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboSZOOSStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboSZOOSStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboSZOOSStoreAttribute_SelectionChangeCommitted);
            // 
            // picSZOOSSizeGroupFilter
            // 
            this.picSZOOSSizeGroupFilter.Location = new System.Drawing.Point(90, 8);
            this.picSZOOSSizeGroupFilter.Name = "picSZOOSSizeGroupFilter";
            this.picSZOOSSizeGroupFilter.Size = new System.Drawing.Size(19, 20);
            this.picSZOOSSizeGroupFilter.TabIndex = 55;
            this.picSZOOSSizeGroupFilter.TabStop = false;
            this.picSZOOSSizeGroupFilter.Click += new System.EventHandler(this.picSZOOSSizeGroupFilter_Click);
            this.picSZOOSSizeGroupFilter.MouseHover += new System.EventHandler(this.picSZOOSSizeGroupFilter_MouseHover);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.cbxSZOOSApplyToLowerLevels);
            this.panel5.Controls.Add(this.cbxSZOOSInheritFromHigherLevel);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(3, 345);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(706, 30);
            this.panel5.TabIndex = 1;
            // 
            // cbxSZOOSApplyToLowerLevels
            // 
            this.cbxSZOOSApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSZOOSApplyToLowerLevels.Location = new System.Drawing.Point(577, 3);
            this.cbxSZOOSApplyToLowerLevels.Name = "cbxSZOOSApplyToLowerLevels";
            this.cbxSZOOSApplyToLowerLevels.Size = new System.Drawing.Size(126, 24);
            this.cbxSZOOSApplyToLowerLevels.TabIndex = 29;
            this.cbxSZOOSApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxSZOOSApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxSZOOSApplyToLowerLevels_CheckedChanged);
            // 
            // cbxSZOOSInheritFromHigherLevel
            // 
            this.cbxSZOOSInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSZOOSInheritFromHigherLevel.Location = new System.Drawing.Point(427, 3);
            this.cbxSZOOSInheritFromHigherLevel.Name = "cbxSZOOSInheritFromHigherLevel";
            this.cbxSZOOSInheritFromHigherLevel.Size = new System.Drawing.Size(144, 24);
            this.cbxSZOOSInheritFromHigherLevel.TabIndex = 30;
            this.cbxSZOOSInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxSZOOSInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxSZOOSInheritFromHigherLevel_CheckedChanged);
            // 
            // tabSizeSellThru
            // 
            this.tabSizeSellThru.Controls.Add(this.panel6);
            this.tabSizeSellThru.Controls.Add(this.gbxSizeSellThru);
            this.tabSizeSellThru.Location = new System.Drawing.Point(4, 22);
            this.tabSizeSellThru.Name = "tabSizeSellThru";
            this.tabSizeSellThru.Padding = new System.Windows.Forms.Padding(3);
            this.tabSizeSellThru.Size = new System.Drawing.Size(712, 378);
            this.tabSizeSellThru.TabIndex = 1;
            this.tabSizeSellThru.Text = "Sell Thru";
            this.tabSizeSellThru.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.cbxSZSellThruApplyToLowerLevels);
            this.panel6.Controls.Add(this.cbxSZSellThruInheritFromHigherLevel);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(3, 345);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(706, 30);
            this.panel6.TabIndex = 2;
            // 
            // cbxSZSellThruApplyToLowerLevels
            // 
            this.cbxSZSellThruApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSZSellThruApplyToLowerLevels.Location = new System.Drawing.Point(577, 3);
            this.cbxSZSellThruApplyToLowerLevels.Name = "cbxSZSellThruApplyToLowerLevels";
            this.cbxSZSellThruApplyToLowerLevels.Size = new System.Drawing.Size(126, 24);
            this.cbxSZSellThruApplyToLowerLevels.TabIndex = 29;
            this.cbxSZSellThruApplyToLowerLevels.Text = "Apply to lower levels";
            // 
            // cbxSZSellThruInheritFromHigherLevel
            // 
            this.cbxSZSellThruInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSZSellThruInheritFromHigherLevel.Location = new System.Drawing.Point(427, 3);
            this.cbxSZSellThruInheritFromHigherLevel.Name = "cbxSZSellThruInheritFromHigherLevel";
            this.cbxSZSellThruInheritFromHigherLevel.Size = new System.Drawing.Size(144, 24);
            this.cbxSZSellThruInheritFromHigherLevel.TabIndex = 30;
            this.cbxSZSellThruInheritFromHigherLevel.Text = "Inherit from higher level";
            // 
            // gbxSizeSellThru
            // 
            this.gbxSizeSellThru.Controls.Add(this.txtSSTLimit);
            this.gbxSizeSellThru.Controls.Add(this.lblSizeSellThruLimit);
            this.gbxSizeSellThru.Location = new System.Drawing.Point(54, 48);
            this.gbxSizeSellThru.Name = "gbxSizeSellThru";
            this.gbxSizeSellThru.Size = new System.Drawing.Size(318, 75);
            this.gbxSizeSellThru.TabIndex = 0;
            this.gbxSizeSellThru.TabStop = false;
            // 
            // txtSSTLimit
            // 
            this.txtSSTLimit.Location = new System.Drawing.Point(176, 31);
            this.txtSSTLimit.Name = "txtSSTLimit";
            this.txtSSTLimit.Size = new System.Drawing.Size(100, 20);
            this.txtSSTLimit.TabIndex = 1;
            this.txtSSTLimit.TextChanged += new System.EventHandler(this.txtSSTLimit_TextChanged);
            // 
            // lblSizeSellThruLimit
            // 
            this.lblSizeSellThruLimit.AutoSize = true;
            this.lblSizeSellThruLimit.Location = new System.Drawing.Point(19, 34);
            this.lblSizeSellThruLimit.Name = "lblSizeSellThruLimit";
            this.lblSizeSellThruLimit.Size = new System.Drawing.Size(136, 13);
            this.lblSizeSellThruLimit.TabIndex = 0;
            this.lblSizeSellThruLimit.Text = "Out of Stock Sell Thru Limit";
            // 
            // tabChainSetPct
            // 
            this.tabChainSetPct.Controls.Add(this.grpTabButtonChainSet);
            this.tabChainSetPct.Controls.Add(this.ugChainSetPercent);
            this.tabChainSetPct.Controls.Add(this.label2);
            this.tabChainSetPct.Controls.Add(this.midDateRangeSelector1);
            this.tabChainSetPct.Controls.Add(this.label1);
            this.tabChainSetPct.Controls.Add(this.cbCSPStoreAttribute);
            this.tabChainSetPct.Location = new System.Drawing.Point(4, 40);
            this.tabChainSetPct.Name = "tabChainSetPct";
            this.tabChainSetPct.Padding = new System.Windows.Forms.Padding(3);
            this.tabChainSetPct.Size = new System.Drawing.Size(720, 404);
            this.tabChainSetPct.TabIndex = 16;
            this.tabChainSetPct.Text = "Chain Set %";
            this.tabChainSetPct.UseVisualStyleBackColor = true;
            // 
            // grpTabButtonChainSet
            // 
            this.grpTabButtonChainSet.Controls.Add(this.rbNoActionCSP);
            this.grpTabButtonChainSet.Controls.Add(this.rbApplyChangesToLowerLevelsChainSet);
            this.grpTabButtonChainSet.Controls.Add(this.cbxCSPApplyToLowerLevels);
            this.grpTabButtonChainSet.Controls.Add(this.cbxCSPInheritFromHigherLevel);
            this.grpTabButtonChainSet.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpTabButtonChainSet.Location = new System.Drawing.Point(3, 367);
            this.grpTabButtonChainSet.Name = "grpTabButtonChainSet";
            this.grpTabButtonChainSet.Size = new System.Drawing.Size(714, 34);
            this.grpTabButtonChainSet.TabIndex = 35;
            this.grpTabButtonChainSet.TabStop = false;
            // 
            // rbNoActionCSP
            // 
            this.rbNoActionCSP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbNoActionCSP.AutoSize = true;
            this.rbNoActionCSP.Location = new System.Drawing.Point(559, 10);
            this.rbNoActionCSP.Name = "rbNoActionCSP";
            this.rbNoActionCSP.Size = new System.Drawing.Size(72, 17);
            this.rbNoActionCSP.TabIndex = 37;
            this.rbNoActionCSP.Text = "No Action";
            // 
            // rbApplyChangesToLowerLevelsChainSet
            // 
            this.rbApplyChangesToLowerLevelsChainSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbApplyChangesToLowerLevelsChainSet.AutoSize = true;
            this.rbApplyChangesToLowerLevelsChainSet.Location = new System.Drawing.Point(214, 10);
            this.rbApplyChangesToLowerLevelsChainSet.Name = "rbApplyChangesToLowerLevelsChainSet";
            this.rbApplyChangesToLowerLevelsChainSet.Size = new System.Drawing.Size(174, 17);
            this.rbApplyChangesToLowerLevelsChainSet.TabIndex = 36;
            this.rbApplyChangesToLowerLevelsChainSet.TabStop = true;
            this.rbApplyChangesToLowerLevelsChainSet.Text = "Apply Changes to Lower Levels";
            this.rbApplyChangesToLowerLevelsChainSet.UseVisualStyleBackColor = true;
            this.rbApplyChangesToLowerLevelsChainSet.CheckedChanged += new System.EventHandler(this.rbApplyChangesToLowerLevels_CheckedChanged);
            // 
            // cbxCSPApplyToLowerLevels
            // 
            this.cbxCSPApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxCSPApplyToLowerLevels.AutoSize = true;
            this.cbxCSPApplyToLowerLevels.Location = new System.Drawing.Point(413, 10);
            this.cbxCSPApplyToLowerLevels.Name = "cbxCSPApplyToLowerLevels";
            this.cbxCSPApplyToLowerLevels.Size = new System.Drawing.Size(121, 17);
            this.cbxCSPApplyToLowerLevels.TabIndex = 35;
            this.cbxCSPApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxCSPApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxApplyInherit_CheckedChanged);
            // 
            // cbxCSPInheritFromHigherLevel
            // 
            this.cbxCSPInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxCSPInheritFromHigherLevel.AutoSize = true;
            this.cbxCSPInheritFromHigherLevel.Location = new System.Drawing.Point(55, 10);
            this.cbxCSPInheritFromHigherLevel.Name = "cbxCSPInheritFromHigherLevel";
            this.cbxCSPInheritFromHigherLevel.Size = new System.Drawing.Size(134, 17);
            this.cbxCSPInheritFromHigherLevel.TabIndex = 34;
            this.cbxCSPInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxCSPInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxApplyInherit_CheckedChanged);
            // 
            // ugChainSetPercent
            // 
            this.ugChainSetPercent.AllowDrop = true;
            this.ugChainSetPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ugChainSetPercent.DisplayLayout.Appearance = appearance31;
            this.ugChainSetPercent.DisplayLayout.GroupByBox.Hidden = true;
            this.ugChainSetPercent.DisplayLayout.InterBandSpacing = 10;
            this.ugChainSetPercent.DisplayLayout.MaxColScrollRegions = 1;
            this.ugChainSetPercent.DisplayLayout.MaxRowScrollRegions = 1;
            this.ugChainSetPercent.DisplayLayout.Override.CardAreaAppearance = appearance32;
            this.ugChainSetPercent.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ugChainSetPercent.DisplayLayout.Override.HeaderAppearance = appearance33;
            this.ugChainSetPercent.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ugChainSetPercent.DisplayLayout.Override.RowAppearance = appearance34;
            this.ugChainSetPercent.DisplayLayout.Override.RowSelectorAppearance = appearance35;
            this.ugChainSetPercent.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugChainSetPercent.DisplayLayout.Override.RowSpacingBefore = 2;
            this.ugChainSetPercent.DisplayLayout.Override.SelectedRowAppearance = appearance36;
            this.ugChainSetPercent.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugChainSetPercent.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugChainSetPercent.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ugChainSetPercent.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ugChainSetPercent.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugChainSetPercent.Location = new System.Drawing.Point(23, 57);
            this.ugChainSetPercent.Name = "ugChainSetPercent";
            this.ugChainSetPercent.Size = new System.Drawing.Size(672, 298);
            this.ugChainSetPercent.TabIndex = 34;
            this.ugChainSetPercent.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugChainSetPercent_AfterCellUpdate);
            this.ugChainSetPercent.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugChainSetPercent_InitializeLayout);
            this.ugChainSetPercent.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugChainSetPercent_CellChange);
            this.ugChainSetPercent.BeforeCellActivate += new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugChainSetPercent_BeforeCellActivate);
            this.ugChainSetPercent.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugChainSetPercent_BeforeCellUpdate);
            this.ugChainSetPercent.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugChainSetPercent_MouseEnterElement);
            this.ugChainSetPercent.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugChainSetPercent_DragDrop);
            this.ugChainSetPercent.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugChainSetPercent_DragEnter);
            this.ugChainSetPercent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugChainSetPercent_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(464, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Time Period:";
            // 
            // midDateRangeSelector1
            // 
            this.midDateRangeSelector1.DateRangeForm = null;
            this.midDateRangeSelector1.DateRangeRID = 0;
            this.midDateRangeSelector1.Enabled = false;
            this.midDateRangeSelector1.Location = new System.Drawing.Point(536, 16);
            this.midDateRangeSelector1.Name = "midDateRangeSelector1";
            this.midDateRangeSelector1.Size = new System.Drawing.Size(160, 24);
            this.midDateRangeSelector1.TabIndex = 30;
            this.midDateRangeSelector1.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSelector1_OnSelection);
            this.midDateRangeSelector1.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.midDateRangeSelector1_ClickCellButton);
            this.midDateRangeSelector1.Load += new System.EventHandler(this.midDateRangeSelector1_Load);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(23, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 23);
            this.label1.TabIndex = 29;
            this.label1.Text = "Store Attribute:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbCSPStoreAttribute
            // 
            this.cbCSPStoreAttribute.AllowDrop = true;
            this.cbCSPStoreAttribute.AllowUserAttributes = false;
            this.cbCSPStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbCSPStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbCSPStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCSPStoreAttribute.Location = new System.Drawing.Point(142, 16);
            this.cbCSPStoreAttribute.Name = "cbCSPStoreAttribute";
            this.cbCSPStoreAttribute.Size = new System.Drawing.Size(200, 21);
            this.cbCSPStoreAttribute.TabIndex = 3;
            this.cbCSPStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cbCSPStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cbCSPStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cbCSPStoreAttribute_SelectionChangeCommitted);
            this.cbCSPStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cbCSPStoreAttribute_DragDrop);
            this.cbCSPStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cbCSPStoreAttribute_DragEnter);
            // 
            // tabReservation
            // 
            this.tabReservation.Controls.Add(this.grpTabButtonIMO);
            this.tabReservation.Controls.Add(this.ugReservation);
            this.tabReservation.Controls.Add(this.midAttributeCbx);
            this.tabReservation.Controls.Add(this.lblRsrvStrAtt);
            this.tabReservation.Controls.Add(this.uddFWOSMax);
            this.tabReservation.Location = new System.Drawing.Point(4, 40);
            this.tabReservation.Name = "tabReservation";
            this.tabReservation.Size = new System.Drawing.Size(720, 404);
            this.tabReservation.TabIndex = 16;
            this.tabReservation.Text = "Reservation";
            this.tabReservation.UseVisualStyleBackColor = true;
            // 
            // grpTabButtonIMO
            // 
            this.grpTabButtonIMO.Controls.Add(this.cbxRsrvInheritFromHigherLevel);
            this.grpTabButtonIMO.Controls.Add(this.rbNoActionIMO);
            this.grpTabButtonIMO.Controls.Add(this.rbApplyChangesToLowerLevelsIMO);
            this.grpTabButtonIMO.Controls.Add(this.cbxApplyToLowerLevels);
            this.grpTabButtonIMO.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpTabButtonIMO.Location = new System.Drawing.Point(0, 370);
            this.grpTabButtonIMO.Name = "grpTabButtonIMO";
            this.grpTabButtonIMO.Size = new System.Drawing.Size(720, 34);
            this.grpTabButtonIMO.TabIndex = 34;
            this.grpTabButtonIMO.TabStop = false;
            // 
            // cbxRsrvInheritFromHigherLevel
            // 
            this.cbxRsrvInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxRsrvInheritFromHigherLevel.AutoSize = true;
            this.cbxRsrvInheritFromHigherLevel.Location = new System.Drawing.Point(55, 10);
            this.cbxRsrvInheritFromHigherLevel.Name = "cbxRsrvInheritFromHigherLevel";
            this.cbxRsrvInheritFromHigherLevel.Size = new System.Drawing.Size(134, 17);
            this.cbxRsrvInheritFromHigherLevel.TabIndex = 33;
            this.cbxRsrvInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxRsrvInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxApplyInherit_CheckedChanged);
            // 
            // rbNoActionIMO
            // 
            this.rbNoActionIMO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbNoActionIMO.AutoSize = true;
            this.rbNoActionIMO.Location = new System.Drawing.Point(559, 10);
            this.rbNoActionIMO.Name = "rbNoActionIMO";
            this.rbNoActionIMO.Size = new System.Drawing.Size(72, 17);
            this.rbNoActionIMO.TabIndex = 36;
            this.rbNoActionIMO.Text = "No Action";
            // 
            // rbApplyChangesToLowerLevelsIMO
            // 
            this.rbApplyChangesToLowerLevelsIMO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbApplyChangesToLowerLevelsIMO.AutoSize = true;
            this.rbApplyChangesToLowerLevelsIMO.Location = new System.Drawing.Point(214, 10);
            this.rbApplyChangesToLowerLevelsIMO.Name = "rbApplyChangesToLowerLevelsIMO";
            this.rbApplyChangesToLowerLevelsIMO.Size = new System.Drawing.Size(174, 17);
            this.rbApplyChangesToLowerLevelsIMO.TabIndex = 35;
            this.rbApplyChangesToLowerLevelsIMO.TabStop = true;
            this.rbApplyChangesToLowerLevelsIMO.Text = "Apply Changes to Lower Levels";
            this.rbApplyChangesToLowerLevelsIMO.UseVisualStyleBackColor = true;
            this.rbApplyChangesToLowerLevelsIMO.CheckedChanged += new System.EventHandler(this.rbApplyChangesToLowerLevels_CheckedChanged);
            // 
            // cbxApplyToLowerLevels
            // 
            this.cbxApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxApplyToLowerLevels.AutoSize = true;
            this.cbxApplyToLowerLevels.Location = new System.Drawing.Point(413, 10);
            this.cbxApplyToLowerLevels.Name = "cbxApplyToLowerLevels";
            this.cbxApplyToLowerLevels.Size = new System.Drawing.Size(121, 17);
            this.cbxApplyToLowerLevels.TabIndex = 34;
            this.cbxApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxApplyInherit_CheckedChanged);
            // 
            // ugReservation
            // 
            this.ugReservation.AllowDrop = true;
            this.ugReservation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance91.BackColor = System.Drawing.Color.White;
            appearance91.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance91.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugReservation.DisplayLayout.Appearance = appearance91;
            this.ugReservation.DisplayLayout.InterBandSpacing = 10;
            appearance92.BackColor = System.Drawing.Color.Transparent;
            this.ugReservation.DisplayLayout.Override.CardAreaAppearance = appearance92;
            appearance93.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance93.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance93.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance93.ForeColor = System.Drawing.Color.Black;
            appearance93.TextHAlignAsString = "Left";
            appearance93.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugReservation.DisplayLayout.Override.HeaderAppearance = appearance93;
            this.ugReservation.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            appearance94.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugReservation.DisplayLayout.Override.RowAppearance = appearance94;
            appearance95.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance95.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance95.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugReservation.DisplayLayout.Override.RowSelectorAppearance = appearance95;
            this.ugReservation.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugReservation.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance96.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance96.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance96.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance96.ForeColor = System.Drawing.Color.Black;
            this.ugReservation.DisplayLayout.Override.SelectedRowAppearance = appearance96;
            this.ugReservation.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugReservation.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugReservation.Location = new System.Drawing.Point(16, 43);
            this.ugReservation.Name = "ugReservation";
            this.ugReservation.Size = new System.Drawing.Size(688, 318);
            this.ugReservation.TabIndex = 33;
            this.ugReservation.AfterCellActivate += new System.EventHandler(this.ugReservation_AfterCellActivate);
            this.ugReservation.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugReservation_AfterCellUpdate);
            this.ugReservation.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugReservation_InitializeLayout);
            this.ugReservation.AfterExitEditMode += new System.EventHandler(this.ugReservation_AfterExitEditMode);
            this.ugReservation.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugReservation_BeforeRowUpdate);
            this.ugReservation.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugReservation_CellChange);
			this.ugReservation.AfterColPosChanged += new Infragistics.Win.UltraWinGrid.AfterColPosChangedEventHandler(this.ugReservation_AfterColPosChanged);
            this.ugReservation.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugReservation_AfterCellListCloseUp);
            // 
            // midAttributeCbx
            // 
            this.midAttributeCbx.AllowDrop = true;
            this.midAttributeCbx.AllowUserAttributes = false;
            this.midAttributeCbx.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.midAttributeCbx.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.midAttributeCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.midAttributeCbx.Location = new System.Drawing.Point(304, 16);
            this.midAttributeCbx.Name = "midAttributeCbx";
            this.midAttributeCbx.Size = new System.Drawing.Size(200, 21);
            this.midAttributeCbx.TabIndex = 29;
            this.midAttributeCbx.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.midAttributeCbx_MIDComboBoxPropertiesChangedEvent);
            this.midAttributeCbx.SelectionChangeCommitted += new System.EventHandler(this.midAttributeCbx_SelectionChangeCommitted);
            // 
            // lblRsrvStrAtt
            // 
            this.lblRsrvStrAtt.Location = new System.Drawing.Point(216, 16);
            this.lblRsrvStrAtt.Name = "lblRsrvStrAtt";
            this.lblRsrvStrAtt.Size = new System.Drawing.Size(80, 23);
            this.lblRsrvStrAtt.TabIndex = 30;
            this.lblRsrvStrAtt.Text = "Store Attribute:";
            this.lblRsrvStrAtt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uddFWOSMax
            // 
            this.uddFWOSMax.DisplayLayout.Appearance = appearance1;
            this.uddFWOSMax.DisplayLayout.InterBandSpacing = 10;
            this.uddFWOSMax.DisplayLayout.MaxColScrollRegions = 1;
            this.uddFWOSMax.DisplayLayout.MaxRowScrollRegions = 1;
            this.uddFWOSMax.DisplayLayout.Override.CardAreaAppearance = appearance2;
            this.uddFWOSMax.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.uddFWOSMax.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.uddFWOSMax.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.uddFWOSMax.DisplayLayout.Override.RowAppearance = appearance4;
            this.uddFWOSMax.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.uddFWOSMax.DisplayLayout.Override.RowSelectorWidth = 12;
            this.uddFWOSMax.DisplayLayout.Override.RowSpacingBefore = 2;
            this.uddFWOSMax.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.uddFWOSMax.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.uddFWOSMax.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.uddFWOSMax.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.uddFWOSMax.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.uddFWOSMax.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.uddFWOSMax.Location = new System.Drawing.Point(216, 96);
            this.uddFWOSMax.Name = "uddFWOSMax";
            this.uddFWOSMax.Size = new System.Drawing.Size(160, 104);
            this.uddFWOSMax.TabIndex = 25;
            this.uddFWOSMax.Visible = false;
            this.uddFWOSMax.RowSelected += new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.uddFWOSMax_RowSelected);
            // 
            // lblChainSetPercentInheritance
            // 
            this.lblChainSetPercentInheritance.Location = new System.Drawing.Point(0, 0);
            this.lblChainSetPercentInheritance.Name = "lblChainSetPercentInheritance";
            this.lblChainSetPercentInheritance.Size = new System.Drawing.Size(100, 23);
            this.lblChainSetPercentInheritance.TabIndex = 0;
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Location = new System.Drawing.Point(16, 512);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(24, 23);
            this.btnHelp.TabIndex = 11;
            this.btnHelp.Text = "?";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(497, 512);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(585, 512);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // mnuStoreEligibilityGrid
            // 
            this.mnuStoreEligibilityGrid.Popup += new System.EventHandler(this.mnuStoreEligibilityGrid_Popup);
            // 
            // mnuIMOGrid
            // 
            this.mnuIMOGrid.Popup += new System.EventHandler(this.mnuIMOGrid_Popup);
            // 
            // mnuGridColHeader
            // 
            this.mnuGridColHeader.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSortAsc,
            this.mnuSortDesc,
            this.mnuFind});
            // 
            // mnuSortAsc
            // 
            this.mnuSortAsc.Index = 0;
            this.mnuSortAsc.Text = "Sort Ascending";
            this.mnuSortAsc.Click += new System.EventHandler(this.mnuSortAsc_Click);
            // 
            // mnuSortDesc
            // 
            this.mnuSortDesc.Index = 1;
            this.mnuSortDesc.Text = "Sort Descending";
            this.mnuSortDesc.Click += new System.EventHandler(this.mnuSortDesc_Click);
            // 
            // mnuFind
            // 
            this.mnuFind.Index = 2;
            this.mnuFind.Text = "Search";
            this.mnuFind.Click += new System.EventHandler(this.mnuFind_Click);
            // 
            // mnuStoreCapacityGrid
            // 
            this.mnuStoreCapacityGrid.Popup += new System.EventHandler(this.mnuStoreCapacityGrid_Popup);
            // 
            // picNodeColor
            // 
            this.picNodeColor.Location = new System.Drawing.Point(16, 8);
            this.picNodeColor.Name = "picNodeColor";
            this.picNodeColor.Size = new System.Drawing.Size(32, 24);
            this.picNodeColor.TabIndex = 12;
            this.picNodeColor.TabStop = false;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(673, 512);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 13;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // mnuDailyPercentagesGrid
            // 
            this.mnuDailyPercentagesGrid.Popup += new System.EventHandler(this.mnuDailyPercentagesGrid_Popup);
            // 
            // mnuChainSetPercentGrid
            // 
            this.mnuChainSetPercentGrid.Popup += new System.EventHandler(this.mnuChainSetPercentGrid_Popup);
            // 
            // txtNodeName
            // 
            this.txtNodeName.AllowDrop = true;
            this.txtNodeName.Location = new System.Drawing.Point(120, 16);
            this.txtNodeName.Name = "txtNodeName";
            this.txtNodeName.Size = new System.Drawing.Size(208, 20);
            this.txtNodeName.TabIndex = 1;
            this.txtNodeName.TextChanged += new System.EventHandler(this.txtNodeName_TextChanged);
            this.txtNodeName.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtNodeName_DragDrop);
            this.txtNodeName.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtNodeName_DragEnter);
            this.txtNodeName.DragOver += new System.Windows.Forms.DragEventHandler(this.txtNodeName_DragOver);
            this.txtNodeName.Enter += new System.EventHandler(this.txtNodeName_Enter);
            this.txtNodeName.Leave += new System.EventHandler(this.txtNodeName_Leave);
            // 
            // cmsCharacteristics
            // 
            this.cmsCharacteristics.Name = "cmsCharacteristics";
            this.cmsCharacteristics.Size = new System.Drawing.Size(61, 4);
            // 
            // mnuSizeCurvesSimilarStoreGrid
            // 
            this.mnuSizeCurvesSimilarStoreGrid.Popup += new System.EventHandler(this.mnuSizeCurvesSimilarStoreGrid_Popup);
            // 
            // frmNodeProperties
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(776, 542);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.picNodeColor);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tabNodeProperties);
            this.Controls.Add(this.txtNodeName);
            this.Controls.Add(this.lblNodeName);
            this.Controls.Add(this.btnHelp);
            this.Name = "frmNodeProperties";
            this.Text = "Node Properties";
            this.Load += new System.EventHandler(this.frmNodeProperties_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNodeProperties_KeyDown);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.lblNodeName, 0);
            this.Controls.SetChildIndex(this.txtNodeName, 0);
            this.Controls.SetChildIndex(this.tabNodeProperties, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.picNodeColor, 0);
            this.Controls.SetChildIndex(this.btnApply, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabNodeProperties.ResumeLayout(false);
            this.tabProfile.ResumeLayout(false);
            this.tabProfile.PerformLayout();
            this.gbxApplyNodeProperties.ResumeLayout(false);
            this.gbxApplyNodeProperties.PerformLayout();
            this.gbxActive.ResumeLayout(false);
            this.gbxBasicReplenishment.ResumeLayout(false);
            this.gbxOTSPlanLevelOverride.ResumeLayout(false);
            this.gbxOTSProduct.ResumeLayout(false);
            this.gbxOTSProduct.PerformLayout();
            this.gbxOTSNode.ResumeLayout(false);
            this.gbxOTSNode.PerformLayout();
            this.gbxOTSType.ResumeLayout(false);
            this.gbxProductType.ResumeLayout(false);
            this.tabStoreEligibility.ResumeLayout(false);
            this.grpTabButtonElig.ResumeLayout(false);
            this.grpTabButtonElig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uddFWOSModifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uddSalesModifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uddStockModifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uddEligModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugStoreEligibility)).EndInit();
            this.tabCharacteristics.ResumeLayout(false);
            this.grpTabButtonChar.ResumeLayout(false);
            this.grpTabButtonChar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugCharacteristics)).EndInit();
            this.tabVelocityGrades.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugSellThruPcts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugVelocityGrades)).EndInit();
            this.tabStoreGrades.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugStoreGrades)).EndInit();
            this.tabStockMinMax.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugStockMinMax)).EndInit();
            this.tabStoreCapacity.ResumeLayout(false);
            this.grpTabButtonSC.ResumeLayout(false);
            this.grpTabButtonSC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugStoreCapacity)).EndInit();
            this.tabDailyPercentages.ResumeLayout(false);
            this.grpTabButtonDailyPct.ResumeLayout(false);
            this.grpTabButtonDailyPct.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugDailyPercentages)).EndInit();
            this.tabPurgeCriteria.ResumeLayout(false);
            this.tabPurgeCriteria.PerformLayout();
            this.grpTabButtonPurge.ResumeLayout(false);
            this.grpTabButtonPurge.PerformLayout();
            this.tabSizeCurves.ResumeLayout(false);
            this.tabSizeCurvesProperties.ResumeLayout(false);
            this.tabSizeCurvesCriteria.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugSizeCurvesInheritedCriteria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugSizeCurvesCriteria)).EndInit();
            this.panel2.ResumeLayout(false);
            this.tabSizeCurvesTolerance.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.gbxSCTMinMaxTolerancePct.ResumeLayout(false);
            this.gbxSCTMinMaxTolerancePct.PerformLayout();
            this.gbxSCTApplyChainSales.ResumeLayout(false);
            this.gbxSCTApplyChainSales.PerformLayout();
            this.gbxSCTIndexUnits.ResumeLayout(false);
            this.gbxSCTIndexUnits.PerformLayout();
            this.gbxSCTHigherLevelSalesTolerance.ResumeLayout(false);
            this.gbxSCTHigherLevelSalesTolerance.PerformLayout();
            this.tabSizeCurvesSimilarStore.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugSizeCurvesSimilarStore)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tabSizeOutOfStock.ResumeLayout(false);
            this.tabSizeOutOfStockProperties.ResumeLayout(false);
            this.tabSizeOutOfStockParms.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSZOOSSizeGroupFilter)).EndInit();
            this.panel5.ResumeLayout(false);
            this.tabSizeSellThru.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.gbxSizeSellThru.ResumeLayout(false);
            this.gbxSizeSellThru.PerformLayout();
            this.tabChainSetPct.ResumeLayout(false);
            this.tabChainSetPct.PerformLayout();
            this.grpTabButtonChainSet.ResumeLayout(false);
            this.grpTabButtonChainSet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugChainSetPercent)).EndInit();
            this.tabReservation.ResumeLayout(false);
            this.grpTabButtonIMO.ResumeLayout(false);
            this.grpTabButtonIMO.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugReservation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uddFWOSMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNodeColor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private System.Windows.Forms.Label lblNodeName;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TabPage tabStoreGrades;
		private System.Windows.Forms.TabControl tabNodeProperties;
		private System.Windows.Forms.TabPage tabPurgeCriteria;
		private System.Windows.Forms.TabPage tabProfile;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label lblDescription;
		private Infragistics.Win.UltraWinGrid.UltraDropDown uddEligModel;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugStoreEligibility;
		private System.Windows.Forms.TabPage tabStoreEligibility;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugStoreGrades;
		private System.Windows.Forms.ContextMenu mnuStoreGrades;
		private System.Windows.Forms.ContextMenu mnuCharacteristics;
		private System.Windows.Forms.RadioButton radTypeUndefined;
		private System.Windows.Forms.RadioButton radTypeHardline;
		private System.Windows.Forms.RadioButton radTypeSoftline;
		private System.Windows.Forms.GroupBox gbxProductType;
		private System.Windows.Forms.TextBox txtNodeID;
		private System.Windows.Forms.Label lblNodeID;
		private Infragistics.Win.UltraWinGrid.UltraDropDown uddStockModifier;
		private Infragistics.Win.UltraWinGrid.UltraDropDown uddSalesModifier;
		private System.Windows.Forms.ContextMenu mnuStoreEligibilityGrid;
        private System.Windows.Forms.ContextMenu mnuIMOGrid;    // TT#1401 - Reservation Stores - gtaylor
        private System.Windows.Forms.ContextMenu mnuGridColHeader;
		private System.Windows.Forms.MenuItem mnuSortAsc;
		private System.Windows.Forms.MenuItem mnuSortDesc;
		private System.Windows.Forms.MenuItem mnuFind;
		private System.Windows.Forms.Label lblSEAttribute;
		private System.Windows.Forms.Label lblSCAttribute;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugStoreCapacity;
		private System.Windows.Forms.ContextMenu mnuStoreCapacityGrid;
		private System.Windows.Forms.GroupBox gbxBasicReplenishment;
		private System.Windows.Forms.RadioButton radBasicReplenishmentYes;
		private System.Windows.Forms.RadioButton radBasicReplenishmentNo;
		private System.Windows.Forms.TabPage tabStoreCapacity;
		private System.Windows.Forms.ContextMenu mnuVelocityGradesGrid;
		private System.Windows.Forms.TabPage tabVelocityGrades;
		private System.Windows.Forms.PictureBox picNodeColor;
		private System.Windows.Forms.Label lblPurgeWeeklyHistory;
		private System.Windows.Forms.Label lblPurgePlans;
		private System.Windows.Forms.Label lblPurgeDailyHistory;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.TextBox txtColorGroup;
		private System.Windows.Forms.Label lblColorGroup;
		private System.Windows.Forms.TabPage tabDailyPercentages;
		private System.Windows.Forms.Label lblDPAttribute;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugDailyPercentages;
		private System.Windows.Forms.ContextMenu mnuDailyPercentagesGrid;
		private System.Windows.Forms.Button btnAddDateRange;
		private System.Windows.Forms.Button btnDeleteDateRange;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugVelocityGrades;
        private System.Windows.Forms.TextBox txtNodeName;
		private System.Windows.Forms.CheckBox cbxSGApplyToLowerLevels;
        private System.Windows.Forms.CheckBox cbxVGApplyToLowerLevels;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugSellThruPcts;
		private System.Windows.Forms.ContextMenu mnuSellThruPctsGrid;
		private System.Windows.Forms.Label lblStoreGradesInheritance;
		private System.Windows.Forms.Label lblMinMaxesInheritance;
		private System.Windows.Forms.Label lblPurgePlansTimeframe;
		private System.Windows.Forms.Label lblPurgeWeeklyHistoryTimeframe;
		private System.Windows.Forms.Label lblPurgeDailyHistoryTimeframe;
		private System.Windows.Forms.Label lblSellThruPctsInheritance;
		private System.Windows.Forms.Label lblVelocityGradesInheritance;
		private MIDAttributeComboBox cbSEStoreAttribute;
		private MIDAttributeComboBox cbSCStoreAttribute;
		private MIDAttributeComboBox cbDPStoreAttribute;
		private System.Windows.Forms.TextBox txtPurgeDailyHistory;
		private System.Windows.Forms.TextBox txtPurgeWeeklyHistory;
		private System.Windows.Forms.TextBox txtPurgePlans;
		private System.Windows.Forms.GroupBox gbxOTSType;
		private System.Windows.Forms.CheckBox chkOTSTypeOverrideRegular;
        private System.Windows.Forms.CheckBox chkOTSTypeOverrideTotal;
		private System.Windows.Forms.GroupBox gbxOTSPlanLevelOverride;
		private System.Windows.Forms.Label lblOTSHierarchy;
		private System.Windows.Forms.TextBox txtOTSAnchorNode;
		private System.Windows.Forms.TextBox txtOTSLevelString;
		private System.Windows.Forms.RadioButton radOTSContainsID;
		private System.Windows.Forms.RadioButton radOTSContainsName;
		private System.Windows.Forms.RadioButton radOTSContainsDescription;
		private System.Windows.Forms.GroupBox gbxOTSNode;
		private System.Windows.Forms.RadioButton radOTSLevel;
		private System.Windows.Forms.RadioButton radOTSNode;
		private System.Windows.Forms.GroupBox gbxOTSProduct;
		private System.Windows.Forms.Label lblOTSLevelStartsWith;
		private Infragistics.Win.UltraWinGrid.UltraDropDown uddFWOSModifier;
        private Infragistics.Win.UltraWinGrid.UltraDropDown uddFWOSMax; //TT#108 - MD - DOConnell - FWOS Max Model
		private System.Windows.Forms.TabPage tabStockMinMax;
		private System.Windows.Forms.CheckBox cbxSMMApplyToLowerLevels;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugStockMinMax;
		private System.Windows.Forms.ContextMenu mnuStockMinMax;
		private System.Windows.Forms.Label lblSMMAttributeSet;
		private System.Windows.Forms.Label lblSMMAttribute;
        private MIDAttributeComboBox cbSMMStoreAttribute;
		private System.Windows.Forms.CheckBox cbxVGInheritFromHigherLevel;
		private System.Windows.Forms.CheckBox cbxSGInheritFromHigherLevel;
        private System.Windows.Forms.CheckBox cbxSMMInheritFromHigherLevel;
        private TabPage tabCharacteristics;
		private UltraGrid ugCharacteristics;
        private ContextMenuStrip cmsCharacteristics;
		private Label lblVelocityMinMaxesInheritance;

        private System.Windows.Forms.Label lblChainSetPercentInheritance;
        //Begin TT#1740 - DOConnell - Chain Set Percent Missing Sub Menu
        private System.Windows.Forms.ContextMenu mnuChainSetPercentGrid;
        //End TT#1740 - DOConnell - Chain Set Percent Missing Sub Menu

		#endregion
		private TabPage tabSizeCurves;
		private UltraGrid ugSizeCurvesInheritedCriteria;
		private TabControl tabSizeCurvesProperties;
		private TabPage tabSizeCurvesCriteria;
		private TabPage tabSizeCurvesTolerance;
		private TabPage tabSizeCurvesSimilarStore;
		private CheckBox cbxSZCriteriaInheritFromHigherLevel;
		private CheckBox cbxSZCriteriaApplyToLowerLevels;
		private UltraGrid ugSizeCurvesSimilarStore;
		private SplitContainer splitContainer1;
		private UltraGrid ugSizeCurvesCriteria;
		private ContextMenu mnuSizeCurvesCriteria;
		private MIDAttributeComboBox cbSZStoreAttribute;
		private Label lblSZAttribute;
        private ContextMenu mnuSizeCurvesSimilarStoreGrid;
		private Label lblSCTHighestLevel;
		private TextBox txtSCTMinimumAvgPerSize;
		private Label lblSCTMinimumAvgPerSize;
		private GroupBox gbxSCTApplyChainSales;
		private Label lblSCTSalesTolerance;
		private GroupBox gbxSCTHigherLevelSalesTolerance;
		private TextBox txtSCTSalesTolerance;
		private RadioButton rdoSCTUnits;
		private RadioButton rdoSCTIndexToAverage;
		private GroupBox gbxSCTMinMaxTolerancePct;
		private TextBox txtSCTMinimumPct;
		private Label lblSCTMinimumPct;
		private TextBox txtSCTMaximumPct;
		private Label lblSCTMaximumPct;
		private Panel panel2;
		private Panel panel3;
		private GroupBox gbxSCTIndexUnits;
		private Panel panel1;
		private CheckBox cbxSZToleranceApplyToLowerLevels;
		private CheckBox cbxSZToleranceInheritFromHigherLevel;
		private Panel panel4;
		private CheckBox cbxSZSimilarStoreApplyToLowerLevels;
		private CheckBox cbxSZSimilarStoreInheritFromHigherLevel;
		private TabPage tabSizeOutOfStock;
		private TabControl tabSizeOutOfStockProperties;
		private TabPage tabSizeOutOfStockParms;
		private TabPage tabSizeSellThru;
		private MIDColorSizeByAttribute mcColorSizeByAttribute;
		private GroupBox gbxSizeSellThru;
		private TextBox txtSSTLimit;
		private Label lblSizeSellThruLimit;
		private Panel panel5;
		private CheckBox cbxSZOOSApplyToLowerLevels;
		private CheckBox cbxSZOOSInheritFromHigherLevel;
		private Panel panel6;
		private CheckBox cbxSZSellThruApplyToLowerLevels;
		private CheckBox cbxSZSellThruInheritFromHigherLevel;
        private Panel panel7;
		protected PictureBox picSZOOSSizeGroupFilter;
		private Label lblSZOOSAttributeSet;
		private Label lblSZOOSSizeGroup;
		protected MIDAttributeComboBox cboSZOOSStoreAttribute;
		private GroupBox gbxActive;
        private RadioButton radActiveNo;
        private RadioButton radActiveYes;
        private TabPage tabChainSetPct;
        private Label label1;
        private MIDAttributeComboBox cbCSPStoreAttribute;
        private Label label2;
        private MIDDateRangeSelector midDateRangeSelector1;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugChainSetPercent;
		private GroupBox gbxApplyNodeProperties;
        private Label lblApplyNodePropsFrom;
        private TextBox txtApplyNodePropsFrom;
        private TabPage tabReservation;
        private MIDAttributeComboBox midAttributeCbx;
        private Label lblRsrvStrAtt;
        private UltraGrid ugReservation;
		private CheckBox cbxApplyMinToZeroTolerance;
        private GroupBox grpTabButtonIMO;
        private RadioButton rbApplyChangesToLowerLevelsIMO;
        private RadioButton cbxApplyToLowerLevels;
        private RadioButton cbxRsrvInheritFromHigherLevel;
        private GroupBox grpTabButtonChainSet;
        private RadioButton cbxCSPApplyToLowerLevels;
        private RadioButton cbxCSPInheritFromHigherLevel;
        private RadioButton rbApplyChangesToLowerLevelsChainSet;
        private GroupBox grpTabButtonPurge;
        private RadioButton rbApplyChangesToLowerLevelsPurge;
        private RadioButton cbxPCInheritFromHigherLevel;
        private RadioButton cbxPCApplyToLowerLevels;
        private GroupBox grpTabButtonDailyPct;
        private RadioButton cbxDPInheritFromHigherLevel;
        private RadioButton cbxDPApplyToLowerLevels;
        private RadioButton rbApplyChangesToLowerLevelsDailyPct;
        private GroupBox grpTabButtonSC;
        private RadioButton rbApplyChangesToLowerLevelsSC;
        private RadioButton cbxSCInheritFromHigherLevel;
        private RadioButton cbxSCApplyToLowerLevels;
        private GroupBox grpTabButtonChar;
        private RadioButton rbApplyChangesToLowerLevelsChar;
        private RadioButton cbxCHInheritFromHigherLevel;
        private RadioButton cbxCHApplyToLowerLevels;
        private GroupBox grpTabButtonElig;
        private RadioButton rbApplyChangesToLowerLevelsElig;
        private RadioButton cbxSEInheritFromHigherLevel;
        private RadioButton cbxSEApplyToLowerLevels;
        private RadioButton rbNoActionIMO;
        private RadioButton rbNoActionCSP;
        private RadioButton rbNoActionPC;
        private RadioButton rbNoActionDP;
        private RadioButton rbNoActionSC;
        private RadioButton rbNoActionChar;
        private RadioButton rbNoActionSE;
        private MIDComboBoxEnh cboOTSLevel;
        private MIDComboBoxEnh cbSMMStoreAttributeSet;
        private MIDComboBoxEnh cboSCTHighestLevel;
        private MIDComboBoxEnh cboSZOOSSizeGroup;
        private Label lblHtDummyTimeframe;
        private Label lblHtDropShipTimeframe;
        private Label lblHtASNTimeframe;
        private TextBox txtHtDummy;
        private TextBox txtHtDropShip;
        private TextBox txtHtASN;
        private Label lblHtDummy;
        private Label lblHtDropShip;
        private Label lblHtASN;
        private Label lblHtWorkUpTotTimeframe;
        private TextBox txtHtWorkupTotBy;
        private Label lblHtVSWTimeframe;
        private Label lblHtReserveTimeframe;
        private Label lblHtPurchaseOrderTimeframe;
        private Label lblHtReceiptTimeframe;
        private TextBox txtHtVSW;
        private TextBox txtHtReserve;
        private TextBox txtHtPurchase;
        private TextBox txtHtReceipt;
        private Label lblHtWorkUpTot;
        private Label lblHtVSW;
        private Label lblHtReserve;
        private Label lblHtReceipt;
        private Label lblHtPurchaseOrder;
	}
}
