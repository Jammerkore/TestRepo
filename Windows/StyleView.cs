using System;
using System.Drawing;
using System.Collections; 
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.Globalization;
using Infragistics.Win;
using Infragistics.Win.UltraWinToolbars;
using System.IO;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{

	public class StyleView : MIDFormBase
	{
		#region Windows Form Designer generated code
		
		#region Windows-generated declarations

		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Panel pnlRowHeaders;
		private System.Windows.Forms.HScrollBar hScrollBar1;
		private System.Windows.Forms.Panel pnlScrollBars;
		private System.Windows.Forms.Panel pnlTotals;
		private System.Windows.Forms.HScrollBar hScrollBar2;
		private System.Windows.Forms.Panel pnlData;
		private System.Windows.Forms.HScrollBar hScrollBar3;
		private C1.Win.C1FlexGrid.C1FlexGrid g4;
		private System.Windows.Forms.Splitter s1;
		private System.Windows.Forms.Splitter s5;
		private C1.Win.C1FlexGrid.C1FlexGrid g7;
		private System.Windows.Forms.Splitter s9;
		private C1.Win.C1FlexGrid.C1FlexGrid g10;
		private System.Windows.Forms.Splitter VerticalSplitter1;
		private System.Windows.Forms.VScrollBar vScrollBar2;
		private System.Windows.Forms.Splitter s4;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		private System.Windows.Forms.Splitter s8;
		private System.Windows.Forms.VScrollBar vScrollBar3;
		private System.Windows.Forms.Splitter s12;
		private System.Windows.Forms.VScrollBar vScrollBar4;
		private System.Windows.Forms.Panel pnlSpacer;
		private C1.Win.C1FlexGrid.C1FlexGrid g5;
		private System.Windows.Forms.Splitter s2;
		private C1.Win.C1FlexGrid.C1FlexGrid g2;
		private System.Windows.Forms.Splitter s6;
		private C1.Win.C1FlexGrid.C1FlexGrid g8;
		private System.Windows.Forms.Splitter s10;
		private C1.Win.C1FlexGrid.C1FlexGrid g11;
		private System.Windows.Forms.Splitter VerticalSplitter2;
		private C1.Win.C1FlexGrid.C1FlexGrid g6;
		private System.Windows.Forms.Splitter s3;
		private C1.Win.C1FlexGrid.C1FlexGrid g3;
		private System.Windows.Forms.Splitter s7;
		private C1.Win.C1FlexGrid.C1FlexGrid g9;
		private System.Windows.Forms.Splitter s11;
        private C1.Win.C1FlexGrid.C1FlexGrid g12;
        private System.Windows.Forms.ContextMenu g1ContextMenu;
        private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.MenuItem mnuSortByDefault;
		private System.ComponentModel.IContainer components;
		#endregion

		private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
		private C1.Win.C1FlexGrid.C1FlexGrid g1;
        private System.Windows.Forms.Panel pnlCorner;
		private System.Windows.Forms.MenuItem mnuColumnChooser1;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.RadioButton rbHeader;
		private System.Windows.Forms.RadioButton rbComponent;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Button btnProcess;
		private System.Windows.Forms.GroupBox gbxAvg;
		private System.Windows.Forms.GroupBox gbxGroupBy;
		private System.Windows.Forms.RadioButton rbAllStores;
		private System.Windows.Forms.RadioButton rbSet;
        // Begin Track #4872 - JSmith - Global/User Attributes
        private MIDAttributeComboBox cmbStoreAttribute;
        // End Track #4872
		private System.Windows.Forms.Button btnVelocity;
        private System.Windows.Forms.Button btnAllocate;
        private System.Windows.Forms.MenuItem mnuFreezeColumn1;
        private MIDComboBoxEnh cmbAttributeSet;
        private MIDComboBoxEnh cmbView; 
        private MIDComboBoxEnh cmbFilter;
        private MIDComboBoxEnh cboAction; 
        private AllocationWaferCellChangeList _allocationWaferCellChangeList;	 // TT#59 Implement Temp Locks
        private Label lblGA; // TT#1194-MD - stodd - view GA header
        private System.Windows.Forms.MenuItem mnuHeaderAllocationCriteria;    // TT#59 Implement Temp Locks 

        public StyleView(ExplorerAddressBlock eab, ApplicationSessionTransaction trans) : base(trans.SAB)
		{
			_trans = trans;
			_sab = _trans.SAB;
            _eab = eab;
			_theme = _sab.ClientServerSession.Theme;
			_awExplorer = (MIDRetail.Windows.AllocationWorkspaceExplorer)_trans.AllocationWorkspaceExplorer;
			InitializeComponent();
			_curViewType = _trans.AllocationViewType;
			_curGroupBy = _trans.AllocationGroupBy ;
            _allocationWaferCellChangeList = new AllocationWaferCellChangeList();  // TT#59 Implement Temp Locks
			switch (_curViewType)
			{
				case eAllocationSelectionViewType.Style:
					_curAttribute    = _trans.AllocationStoreAttributeID;
					_curAttributeSet = _trans.AllocationStoreGroupLevel;
					break;

				case eAllocationSelectionViewType.Velocity:
					_curAttribute    = _trans.VelocityStoreGroupRID;
					_curAttributeSet = -1;
					break;
			}		
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}

				this.btnVelocity.Click -= new System.EventHandler(this.btnVelocity_Click);
				this.btnAllocate.Click -= new System.EventHandler(this.btnAllocate_Click);
                //BEGIN TT#6-MD-VStuart - Single Store Select
                //this.cmbFilter.SelectionChangeCommitted -= new System.EventHandler(this.cmbFilter_SelectionChangeCommitted);
                this.cmbFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cmbFilter_DragEnter);
                this.cmbFilter.DragOver -= new System.Windows.Forms.DragEventHandler(this.cmbFilter_DragOver);
                this.cmbFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cmbFilter_DragDrop);
                this.cmbFilter.SelectionChangeCommitted -= new System.EventHandler(this.cmbFilter_SelectionChangeCommitted);
                this.cmbAttributeSet.SelectionChangeCommitted -= new System.EventHandler(this.cmbAttributeSet_SelectionChangeCommitted);
                this.cmbView.SelectionChangeCommitted -= new System.EventHandler(this.cmbView_SelectionChangeCommitted);
                this.cboAction.SelectionChangeCommitted -= new System.EventHandler(this.cboAction_SelectionChangeCommitted);
                //this.cboAction.SelectionChangeCommitted -= new System.EventHandler(this.cboAction_SelectionChangeCommitted);
                //END TT#6-MD-VStuart - Single Store Select
                this.btnApply.Click -= new System.EventHandler(this.btnApply_Click);
				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
                this.rbComponent.CheckedChanged -= new System.EventHandler(this.rbComponent_CheckedChanged);
				this.rbHeader.CheckedChanged -= new System.EventHandler(this.rbHeader_CheckedChanged);
                //this.cmbStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cmbStoreAttribute_SelectionChangeCommitted);    //TT#6-MD-VStuart - Single Store Select
                //this.cmbStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cmbAttributeSet_SelectionChangeCommitted);  //TT#6-MD-VStuart - Single Store Select
                this.cmbStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cmbStoreAttribute_SelectionChangeCommitted);
                // Begin Track #4872 - JSmith - Global/User Attributes
                this.cmbStoreAttribute.DragOver -= new System.Windows.Forms.DragEventHandler(this.cmbStoreAttribute_DragOver);
                this.cmbStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cmbStoreAttribute_DragEnter);
                // End Track #4872
                this.cmbStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cmbStoreAttribute_DragEnter);
                this.cmbStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cmbStoreAttribute_DragDrop);
                this.g4.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g4.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g4.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g4_OwnerDrawCell);
				this.g4.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g4_BeforeScroll);
                // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                this.g4.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
                // End TT#1542
				this.s1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.s1_SplitterMoved);
				this.s1.DoubleClick -= new System.EventHandler(this.g4SplitterDoubleClick);
				this.s5.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
				this.s5.DoubleClick -= new System.EventHandler(this.g7SplitterDoubleClick);
				this.g7.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g7.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g7.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g7_OwnerDrawCell);
				this.g7.DoubleClick -= new System.EventHandler(this.g7SplitterDoubleClick);
				this.g7.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g7_BeforeScroll);
                // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                this.g7.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
                // End TT#1542
				this.s9.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
				this.s9.DoubleClick -= new System.EventHandler(this.g10SplitterDoubleClick);
				this.g10.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g10.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g10.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g10_OwnerDrawCell);
				this.g10.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g10_BeforeScroll);
                // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                this.g10.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
                // End TT#1542
				this.hScrollBar1.ValueChanged -= new System.EventHandler(this.hScrollBar1_ValueChanged);
				this.g1.DragOver -= new System.Windows.Forms.DragEventHandler(this.GridDragOver);
                // Begin Track #6371 - JSmith - Sorting in SKU Review is slow
                //this.g1.Click -= new System.EventHandler(this.GridClick);
                // End Track #6371
				this.g1.DragEnter -= new System.Windows.Forms.DragEventHandler(this.GridDragEnter);
				this.g1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
				this.g1.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g1_AfterResizeColumn);
				this.g1.QueryContinueDrag -= new System.Windows.Forms.QueryContinueDragEventHandler(this.GridQueryContinueDrag);
				this.g1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g1.BeforeResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeResizeColumn);
				this.g1.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.GridMouseUp);
				this.g1.DragDrop -= new System.Windows.Forms.DragEventHandler(this.g1_DragDrop);
                // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                this.g1.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
                // End TT#1542
				this.mnuColumnChooser1.Click -= new System.EventHandler(this.mnuColumnChooser1_Click);
				this.mnuFreezeColumn1.Click -= new System.EventHandler(this.mnuFreezeColumn1_Click);
				this.mnuSortByDefault.Click -= new System.EventHandler(this.mnuSortByDefault_Click);
                // begin TT#59 Implement Store Temp Locks
                if (this.SAB.AllowDebugging)
                {
                    this.mnuHeaderAllocationCriteria.Click -= new System.EventHandler(this.mnuHeaderAllocationCriteria_Click);
                }
                // end TT#59 Implement Store Temp Locks
				this.rbSet.CheckedChanged -= new System.EventHandler(this.rbSet_CheckedChanged);
				this.rbAllStores.CheckedChanged -= new System.EventHandler(this.rbAllStores_CheckedChanged);
				this.VerticalSplitter1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.VerticalSplitter1_SplitterMoved);
				this.VerticalSplitter1.DoubleClick -= new System.EventHandler(this.VerticalSplitter1_DoubleClick);
				this.vScrollBar2.ValueChanged -= new System.EventHandler(this.vScrollBar2_ValueChanged);
				this.s4.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.s4_SplitterMoved);
				this.s4.DoubleClick -= new System.EventHandler(this.g4SplitterDoubleClick);
				this.s8.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
				this.s8.DoubleClick -= new System.EventHandler(this.g7SplitterDoubleClick);
				this.vScrollBar3.ValueChanged -= new System.EventHandler(this.vScrollBar3_ValueChanged);
				this.s12.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
				this.s12.DoubleClick -= new System.EventHandler(this.g10SplitterDoubleClick);
				this.vScrollBar4.ValueChanged -= new System.EventHandler(this.vScrollBar4_ValueChanged);
				this.g5.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g5.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g5.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g5.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g5_OwnerDrawCell);
				this.g5.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g5_BeforeScroll);
				this.g5.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g5.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
				this.g5.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.s2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.s2_SplitterMoved);
				this.s2.DoubleClick -= new System.EventHandler(this.g4SplitterDoubleClick);
				this.g2.DragOver -= new System.Windows.Forms.DragEventHandler(this.GridDragOver);
                // Begin Track #6371 - JSmith - Sorting in SKU Review is slow
                //this.g2.Click -= new System.EventHandler(this.GridClick);
                // End Track #6371
				this.g2.DragEnter -= new System.Windows.Forms.DragEventHandler(this.GridDragEnter);
				this.g2.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
				this.g2.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_AfterResizeColumn);
				this.g2.QueryContinueDrag -= new System.Windows.Forms.QueryContinueDragEventHandler(this.GridQueryContinueDrag);
				this.g2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g2.BeforeResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeResizeColumn);
				this.g2.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.GridMouseUp);
				this.g2.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g2_BeforeScroll);
				this.g2.DragDrop -= new System.Windows.Forms.DragEventHandler(this.g2_DragDrop);
                // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                this.g2.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
                // End TT#1542
				this.s6.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
				this.s6.DoubleClick -= new System.EventHandler(this.g7SplitterDoubleClick);
				this.g8.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g8.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g8.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g8.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g8_OwnerDrawCell);
				this.g8.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g8_BeforeScroll);
				this.g8.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g8.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
				this.g8.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.s10.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
				this.s10.DoubleClick -= new System.EventHandler(this.g10SplitterDoubleClick);
				this.g11.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g11.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g11.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g11.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g11_OwnerDrawCell);
				this.g11.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g11_BeforeScroll);
				this.g11.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g11.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
				this.g11.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.hScrollBar2.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
				this.VerticalSplitter2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.VerticalSplitter2_SplitterMoved);
				this.VerticalSplitter2.DoubleClick -= new System.EventHandler(this.VerticalSplitter2_DoubleClick);
				this.g6.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g6.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g6.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g6.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g6_OwnerDrawCell);
				this.g6.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g6_BeforeScroll);
				this.g6.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g6.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
				this.g6.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
                // Begin TT#3686 - RMatelic - Vecity Rule Type Qty does not accept decimals for WOS or Forward WOS Rules
                this.g6.CellChanged -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g6_CellChanged);
                // End TT#3686  
				this.s3.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.s3_SplitterMoved);
				this.s3.DoubleClick -= new System.EventHandler(this.g4SplitterDoubleClick);
				this.g3.DragOver -= new System.Windows.Forms.DragEventHandler(this.GridDragOver);
                // Begin Track #6371 - JSmith - Sorting in SKU Review is slow
                //this.g3.Click -= new System.EventHandler(this.GridClick);
                // End Track #6371
				this.g3.DragEnter -= new System.Windows.Forms.DragEventHandler(this.GridDragEnter);
				this.g3.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
				this.g3.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_AfterResizeColumn);
				this.g3.QueryContinueDrag -= new System.Windows.Forms.QueryContinueDragEventHandler(this.GridQueryContinueDrag);
				this.g3.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g3.BeforeResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeResizeColumn);
				this.g3.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g3_OwnerDrawCell);
				this.g3.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.GridMouseUp);
				this.g3.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g3_BeforeScroll);
				this.g3.DragDrop -= new System.Windows.Forms.DragEventHandler(this.g3_DragDrop);
                // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                this.g3.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
                // End TT#1542
				this.s7.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
				this.s7.DoubleClick -= new System.EventHandler(this.g7SplitterDoubleClick);
				this.g9.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g9.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g9.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g9.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g9_OwnerDrawCell);
				this.g9.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g9_BeforeScroll);
				this.g9.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g9.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
				this.g9.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.s11.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
				this.s11.DoubleClick -= new System.EventHandler(this.g10SplitterDoubleClick);
				this.g12.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g12.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g12.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g12.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g12_OwnerDrawCell);
				this.g12.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g12_BeforeScroll);
				this.g12.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g12.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
				this.g12.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.hScrollBar3.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.hScrollBar3_Scroll);
				this.Load -= new System.EventHandler(this.StyleView_Load);
				this.Activated -= new System.EventHandler(this.StyleView_Activated);
				this.Deactivate -= new System.EventHandler(this.StyleView_Deactivate);
                //this.cmbView.SelectionChangeCommitted -= new System.EventHandler(this.cmbView_SelectionChangeCommitted);    //TT#6-MD-VStuart - Single Store Select

                this.cboAction.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboAction_MIDComboBoxPropertiesChangedEvent);
                this.cmbFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cmbFilter_MIDComboBoxPropertiesChangedEvent);
                this.cmbView.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cmbView_MIDComboBoxPropertiesChangedEvent);
                this.cmbAttributeSet.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cmbAttributeSet_MIDComboBoxPropertiesChangedEvent);
                this.cmbStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cmbStoreAttribute_MIDComboBoxPropertiesChangedEvent);

				if (_quickFilter !=null)
				{
					_quickFilter.OnValidateFieldsHandler -= new QuickFilter.ValidateFieldsHandler(OnValidateFieldsHandler);
					_quickFilter.OnComponentSelectedIndexChangeEventHandler -= new QuickFilter.ComponentSelectedIndexChangeEventHandler(OnComponentSelectedIndexChangeEventHandler);
					_quickFilter.OnValidateFieldsHandler -= new QuickFilter.ValidateFieldsHandler(OnValidateFieldsHandler);
					_quickFilter.OnCheckBoxCheckChangedEventHandler -= new QuickFilter.CheckBoxCheckChangedEventHandler(OnCheckBoxCheckChangedEventHandler);
				}

				if (_frmThemeProperties != null)
				{
					_frmThemeProperties.ApplyButtonClicked -= new EventHandler(StylePropertiesOnChanged);
				}

                // Begin TT#3513 - JSmith - Clean Up Memory Leaks 
                if (_trans.VelocityWindow != null)
                {
                    ((MIDRetail.Windows.frmVelocityMethod)_trans.VelocityWindow).FrmStyleView = null;
                }
                cmbFilter.Tag = null;
				cmbStoreAttribute.Tag = null;  
                // End TT#3513 - JSmith - Clean Up Memory Leaks
                // Begin TT#1495-MD - RMatelic - ASST- Created a post asst- highlight 1st header open style review all the detail columns appear.  Close style review and reopen and all the detail columns are gone
                if (_trans.StyleView == null)
                {
                    _trans.DetermineShowNeedAnalysis = true;
                }
                // End TT#1495-MD
				if (_trans.StyleView == null &&
					_trans.SummaryView == null &&
					_trans.SizeView == null &&
                    _trans.AssortmentView == null &&
					_trans.VelocityWindow == null)
				{
					_trans.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblGA = new System.Windows.Forms.Label();
            this.cboAction = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cmbFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cmbView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cmbAttributeSet = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.gbxGroupBy = new System.Windows.Forms.GroupBox();
            this.rbComponent = new System.Windows.Forms.RadioButton();
            this.rbHeader = new System.Windows.Forms.RadioButton();
            this.cmbStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.pnlRowHeaders = new System.Windows.Forms.Panel();
            this.g4 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s1 = new System.Windows.Forms.Splitter();
            this.s5 = new System.Windows.Forms.Splitter();
            this.g7 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s9 = new System.Windows.Forms.Splitter();
            this.g10 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.g1 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.g1ContextMenu = new System.Windows.Forms.ContextMenu();
            this.mnuColumnChooser1 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mnuFreezeColumn1 = new System.Windows.Forms.MenuItem();
            this.mnuSortByDefault = new System.Windows.Forms.MenuItem();
            this.mnuHeaderAllocationCriteria = new System.Windows.Forms.MenuItem();
            this.pnlCorner = new System.Windows.Forms.Panel();
            this.btnAllocate = new System.Windows.Forms.Button();
            this.btnVelocity = new System.Windows.Forms.Button();
            this.gbxAvg = new System.Windows.Forms.GroupBox();
            this.rbSet = new System.Windows.Forms.RadioButton();
            this.rbAllStores = new System.Windows.Forms.RadioButton();
            this.VerticalSplitter1 = new System.Windows.Forms.Splitter();
            this.pnlScrollBars = new System.Windows.Forms.Panel();
            this.vScrollBar2 = new System.Windows.Forms.VScrollBar();
            this.s4 = new System.Windows.Forms.Splitter();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.s8 = new System.Windows.Forms.Splitter();
            this.vScrollBar3 = new System.Windows.Forms.VScrollBar();
            this.s12 = new System.Windows.Forms.Splitter();
            this.vScrollBar4 = new System.Windows.Forms.VScrollBar();
            this.pnlSpacer = new System.Windows.Forms.Panel();
            this.pnlTotals = new System.Windows.Forms.Panel();
            this.g5 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s2 = new System.Windows.Forms.Splitter();
            this.g2 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s6 = new System.Windows.Forms.Splitter();
            this.g8 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s10 = new System.Windows.Forms.Splitter();
            this.g11 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.hScrollBar2 = new System.Windows.Forms.HScrollBar();
            this.VerticalSplitter2 = new System.Windows.Forms.Splitter();
            this.pnlData = new System.Windows.Forms.Panel();
            this.g6 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s3 = new System.Windows.Forms.Splitter();
            this.g3 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s7 = new System.Windows.Forms.Splitter();
            this.g9 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s11 = new System.Windows.Forms.Splitter();
            this.g12 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.hScrollBar3 = new System.Windows.Forms.HScrollBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.gbxGroupBy.SuspendLayout();
            this.pnlRowHeaders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g1)).BeginInit();
            this.pnlCorner.SuspendLayout();
            this.gbxAvg.SuspendLayout();
            this.pnlScrollBars.SuspendLayout();
            this.pnlTotals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g11)).BeginInit();
            this.pnlData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g12)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblGA);
            this.pnlTop.Controls.Add(this.cboAction);
            this.pnlTop.Controls.Add(this.cmbFilter);
            this.pnlTop.Controls.Add(this.cmbView);
            this.pnlTop.Controls.Add(this.cmbAttributeSet);
            this.pnlTop.Controls.Add(this.btnApply);
            this.pnlTop.Controls.Add(this.btnProcess);
            this.pnlTop.Controls.Add(this.gbxGroupBy);
            this.pnlTop.Controls.Add(this.cmbStoreAttribute);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1244, 30);
            this.pnlTop.TabIndex = 0;
            // 
            // lblGA
            // 
            this.lblGA.AutoSize = true;
            this.lblGA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGA.ForeColor = System.Drawing.Color.Red;
            this.lblGA.Location = new System.Drawing.Point(1094, 8);
            this.lblGA.Name = "lblGA";
            this.lblGA.Size = new System.Drawing.Size(59, 13);
            this.lblGA.TabIndex = 22;
            this.lblGA.Text = "GA Mode";
            // 
            // cboAction
            // 
            this.cboAction.AutoAdjust = true;
            this.cboAction.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAction.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAction.DataSource = null;
            this.cboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAction.DropDownWidth = 157;
            this.cboAction.FormattingEnabled = false;
            this.cboAction.IgnoreFocusLost = false;
            this.cboAction.ItemHeight = 13;
            this.cboAction.Location = new System.Drawing.Point(777, 4);
            this.cboAction.Margin = new System.Windows.Forms.Padding(0);
            this.cboAction.MaxDropDownItems = 25;
            this.cboAction.Name = "cboAction";
            this.cboAction.SetToolTip = "";
            this.cboAction.Size = new System.Drawing.Size(157, 22);
            this.cboAction.TabIndex = 21;
            this.cboAction.Tag = null;
            this.cboAction.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboAction_MIDComboBoxPropertiesChangedEvent);
            this.cboAction.SelectionChangeCommitted += new System.EventHandler(this.cboAction_SelectionChangeCommitted);
            // 
            // cmbFilter
            // 
            this.cmbFilter.AllowDrop = true;
            this.cmbFilter.AutoAdjust = true;
            this.cmbFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFilter.DataSource = null;
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.DropDownWidth = 147;
            this.cmbFilter.FormattingEnabled = false;
            this.cmbFilter.IgnoreFocusLost = false;
            this.cmbFilter.ItemHeight = 13;
            this.cmbFilter.Location = new System.Drawing.Point(629, 4);
            this.cmbFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cmbFilter.MaxDropDownItems = 25;
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.SetToolTip = "";
            this.cmbFilter.Size = new System.Drawing.Size(147, 22);
            this.cmbFilter.TabIndex = 20;
            this.cmbFilter.Tag = null;
            this.cmbFilter.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cmbFilter_MIDComboBoxPropertiesChangedEvent);
            this.cmbFilter.SelectionChangeCommitted += new System.EventHandler(this.cmbFilter_SelectionChangeCommitted);
            this.cmbFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cmbFilter_DragDrop);
            this.cmbFilter.DragEnter += new System.Windows.Forms.DragEventHandler(this.cmbFilter_DragEnter);
            this.cmbFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cmbFilter_DragOver);
            // 
            // cmbView
            // 
            this.cmbView.AutoAdjust = true;
            this.cmbView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbView.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbView.DataSource = null;
            this.cmbView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbView.DropDownWidth = 141;
            this.cmbView.FormattingEnabled = false;
            this.cmbView.IgnoreFocusLost = false;
            this.cmbView.ItemHeight = 13;
            this.cmbView.Location = new System.Drawing.Point(483, 4);
            this.cmbView.Margin = new System.Windows.Forms.Padding(0);
            this.cmbView.MaxDropDownItems = 25;
            this.cmbView.Name = "cmbView";
            this.cmbView.SetToolTip = "";
            this.cmbView.Size = new System.Drawing.Size(141, 21);
            this.cmbView.TabIndex = 10;
            this.cmbView.Tag = null;
            this.cmbView.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cmbView_MIDComboBoxPropertiesChangedEvent);
            this.cmbView.SelectionChangeCommitted += new System.EventHandler(this.cmbView_SelectionChangeCommitted);
            // 
            // cmbAttributeSet
            // 
            this.cmbAttributeSet.AutoAdjust = true;
            this.cmbAttributeSet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbAttributeSet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbAttributeSet.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.cmbAttributeSet.DataSource = null;
            this.cmbAttributeSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttributeSet.DropDownWidth = 142;
            this.cmbAttributeSet.FormattingEnabled = false;
            this.cmbAttributeSet.IgnoreFocusLost = false;
            this.cmbAttributeSet.ItemHeight = 13;
            this.cmbAttributeSet.Location = new System.Drawing.Point(333, 4);
            this.cmbAttributeSet.Margin = new System.Windows.Forms.Padding(0);
            this.cmbAttributeSet.MaxDropDownItems = 25;
            this.cmbAttributeSet.Name = "cmbAttributeSet";
            this.cmbAttributeSet.SetToolTip = "";
            this.cmbAttributeSet.Size = new System.Drawing.Size(142, 22);
            this.cmbAttributeSet.TabIndex = 0;
            this.cmbAttributeSet.Tag = null;
            this.cmbAttributeSet.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cmbAttributeSet_MIDComboBoxPropertiesChangedEvent);
            this.cmbAttributeSet.SelectionChangeCommitted += new System.EventHandler(this.cmbAttributeSet_SelectionChangeCommitted);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(1015, 4);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(64, 21);
            this.btnApply.TabIndex = 9;
            this.btnApply.Text = "Apply";
            this.toolTip1.SetToolTip(this.btnApply, "Apply current changes");
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Enabled = false;
            this.btnProcess.Location = new System.Drawing.Point(943, 4);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(64, 21);
            this.btnProcess.TabIndex = 8;
            this.btnProcess.Text = "Process";
            this.toolTip1.SetToolTip(this.btnProcess, "Process action");
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // gbxGroupBy
            // 
            this.gbxGroupBy.Controls.Add(this.rbComponent);
            this.gbxGroupBy.Controls.Add(this.rbHeader);
            this.gbxGroupBy.Location = new System.Drawing.Point(8, 0);
            this.gbxGroupBy.Name = "gbxGroupBy";
            this.gbxGroupBy.Size = new System.Drawing.Size(160, 28);
            this.gbxGroupBy.TabIndex = 1;
            this.gbxGroupBy.TabStop = false;
            // 
            // rbComponent
            // 
            this.rbComponent.Location = new System.Drawing.Point(76, 8);
            this.rbComponent.Name = "rbComponent";
            this.rbComponent.Size = new System.Drawing.Size(82, 16);
            this.rbComponent.TabIndex = 2;
            this.rbComponent.TabStop = true;
            this.rbComponent.Text = "Component";
            this.toolTip1.SetToolTip(this.rbComponent, "Group by Component");
            this.rbComponent.CheckedChanged += new System.EventHandler(this.rbComponent_CheckedChanged);
            // 
            // rbHeader
            // 
            this.rbHeader.Checked = true;
            this.rbHeader.Location = new System.Drawing.Point(8, 8);
            this.rbHeader.Name = "rbHeader";
            this.rbHeader.Size = new System.Drawing.Size(64, 16);
            this.rbHeader.TabIndex = 1;
            this.rbHeader.TabStop = true;
            this.rbHeader.Text = "Header";
            this.toolTip1.SetToolTip(this.rbHeader, "Group by Header");
            this.rbHeader.CheckedChanged += new System.EventHandler(this.rbHeader_CheckedChanged);
            // 
            // cmbStoreAttribute
            // 
            this.cmbStoreAttribute.AllowDrop = true;
            this.cmbStoreAttribute.AllowUserAttributes = false;
            this.cmbStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbStoreAttribute.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStoreAttribute.Location = new System.Drawing.Point(184, 4);
            this.cmbStoreAttribute.Name = "cmbStoreAttribute";
            this.cmbStoreAttribute.Size = new System.Drawing.Size(141, 21);
            this.cmbStoreAttribute.TabIndex = 3;
            this.toolTip1.SetToolTip(this.cmbStoreAttribute, "Attributes");
            this.cmbStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cmbStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cmbStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cmbStoreAttribute_SelectionChangeCommitted);
            this.cmbStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cmbStoreAttribute_DragDrop);
            this.cmbStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cmbStoreAttribute_DragEnter);
            this.cmbStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cmbStoreAttribute_DragOver);
            // 
            // pnlRowHeaders
            // 
            this.pnlRowHeaders.Controls.Add(this.g4);
            this.pnlRowHeaders.Controls.Add(this.s1);
            this.pnlRowHeaders.Controls.Add(this.s5);
            this.pnlRowHeaders.Controls.Add(this.g7);
            this.pnlRowHeaders.Controls.Add(this.s9);
            this.pnlRowHeaders.Controls.Add(this.g10);
            this.pnlRowHeaders.Controls.Add(this.hScrollBar1);
            this.pnlRowHeaders.Controls.Add(this.g1);
            this.pnlRowHeaders.Controls.Add(this.pnlCorner);
            this.pnlRowHeaders.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlRowHeaders.Location = new System.Drawing.Point(0, 30);
            this.pnlRowHeaders.Name = "pnlRowHeaders";
            this.pnlRowHeaders.Size = new System.Drawing.Size(80, 296);
            this.pnlRowHeaders.TabIndex = 1;
            // 
            // g4
            // 
            this.g4.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g4.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g4.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g4.ExtendLastCol = true;
            this.g4.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g4.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g4.Location = new System.Drawing.Point(0, 65);
            this.g4.Name = "g4";
            this.g4.Rows.DefaultSize = 17;
            this.g4.Rows.Fixed = 0;
            this.g4.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g4.Size = new System.Drawing.Size(80, 100);
            this.g4.TabIndex = 0;
            this.g4.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g4_BeforeScroll);
            this.g4.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g4.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g4_OwnerDrawCell);
            this.g4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // s1
            // 
            this.s1.Dock = System.Windows.Forms.DockStyle.Top;
            this.s1.Location = new System.Drawing.Point(0, 64);
            this.s1.Name = "s1";
            this.s1.Size = new System.Drawing.Size(80, 1);
            this.s1.TabIndex = 10;
            this.s1.TabStop = false;
            this.s1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.s1_SplitterMoved);
            this.s1.DoubleClick += new System.EventHandler(this.g4SplitterDoubleClick);
            // 
            // s5
            // 
            this.s5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.s5.Location = new System.Drawing.Point(0, 165);
            this.s5.MinExtra = 15;
            this.s5.MinSize = 15;
            this.s5.Name = "s5";
            this.s5.Size = new System.Drawing.Size(80, 1);
            this.s5.TabIndex = 8;
            this.s5.TabStop = false;
            this.s5.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
            this.s5.DoubleClick += new System.EventHandler(this.g7SplitterDoubleClick);
            // 
            // g7
            // 
            this.g7.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g7.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g7.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.g7.ExtendLastCol = true;
            this.g7.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g7.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g7.Location = new System.Drawing.Point(0, 166);
            this.g7.Name = "g7";
            this.g7.Rows.DefaultSize = 17;
            this.g7.Rows.Fixed = 0;
            this.g7.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g7.Size = new System.Drawing.Size(80, 72);
            this.g7.TabIndex = 0;
            this.g7.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g7_BeforeScroll);
            this.g7.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g7.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g7_OwnerDrawCell);
            this.g7.DoubleClick += new System.EventHandler(this.g7SplitterDoubleClick);
            this.g7.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // s9
            // 
            this.s9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.s9.Location = new System.Drawing.Point(0, 238);
            this.s9.Name = "s9";
            this.s9.Size = new System.Drawing.Size(80, 1);
            this.s9.TabIndex = 6;
            this.s9.TabStop = false;
            this.s9.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
            this.s9.DoubleClick += new System.EventHandler(this.g10SplitterDoubleClick);
            // 
            // g10
            // 
            this.g10.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g10.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g10.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g10.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.g10.ExtendLastCol = true;
            this.g10.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g10.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g10.Location = new System.Drawing.Point(0, 239);
            this.g10.Name = "g10";
            this.g10.Rows.DefaultSize = 17;
            this.g10.Rows.Fixed = 0;
            this.g10.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g10.Size = new System.Drawing.Size(80, 40);
            this.g10.TabIndex = 0;
            this.g10.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g10_BeforeScroll);
            this.g10.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g10.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g10_OwnerDrawCell);
            this.g10.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 279);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(80, 17);
            this.hScrollBar1.TabIndex = 4;
            this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // g1
            // 
            this.g1.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None;
            this.g1.AllowEditing = false;
            this.g1.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g1.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g1.ColumnInfo = "10,0,0,0,0,85,Columns:";
            this.g1.ContextMenu = this.g1ContextMenu;
            this.g1.Dock = System.Windows.Forms.DockStyle.Top;
            this.g1.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g1.ExtendLastCol = true;
            this.g1.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g1.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g1.Location = new System.Drawing.Point(0, 40);
            this.g1.Name = "g1";
            this.g1.Rows.DefaultSize = 17;
            this.g1.Rows.Fixed = 0;
            this.g1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g1.Size = new System.Drawing.Size(80, 24);
            this.g1.TabIndex = 13;
            this.g1.BeforeResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeResizeColumn);
            this.g1.AfterResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g1_AfterResizeColumn);
            this.g1.DragDrop += new System.Windows.Forms.DragEventHandler(this.g1_DragDrop);
            this.g1.DragEnter += new System.Windows.Forms.DragEventHandler(this.GridDragEnter);
            this.g1.DragOver += new System.Windows.Forms.DragEventHandler(this.GridDragOver);
            this.g1.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.GridQueryContinueDrag);
            this.g1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            this.g1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
            this.g1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GridMouseUp);
            // 
            // g1ContextMenu
            // 
            this.g1ContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuColumnChooser1,
            this.menuItem3,
            this.mnuFreezeColumn1,
            this.mnuSortByDefault,
            this.mnuHeaderAllocationCriteria});
            // 
            // mnuColumnChooser1
            // 
            this.mnuColumnChooser1.Index = 0;
            this.mnuColumnChooser1.Text = "Column Chooser...";
            this.mnuColumnChooser1.Click += new System.EventHandler(this.mnuColumnChooser1_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "-";
            // 
            // mnuFreezeColumn1
            // 
            this.mnuFreezeColumn1.Index = 2;
            this.mnuFreezeColumn1.Text = "Freeze Column";
            this.mnuFreezeColumn1.Click += new System.EventHandler(this.mnuFreezeColumn1_Click);
            // 
            // mnuSortByDefault
            // 
            this.mnuSortByDefault.Index = 3;
            this.mnuSortByDefault.Text = "Sort By Default";
            this.mnuSortByDefault.Click += new System.EventHandler(this.mnuSortByDefault_Click);
            // 
            // mnuHeaderAllocationCriteria
            // 
            this.mnuHeaderAllocationCriteria.Index = 4;
            this.mnuHeaderAllocationCriteria.Text = "";
            // 
            // pnlCorner
            // 
            this.pnlCorner.Controls.Add(this.btnAllocate);
            this.pnlCorner.Controls.Add(this.btnVelocity);
            this.pnlCorner.Controls.Add(this.gbxAvg);
            this.pnlCorner.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCorner.Location = new System.Drawing.Point(0, 0);
            this.pnlCorner.Name = "pnlCorner";
            this.pnlCorner.Size = new System.Drawing.Size(80, 40);
            this.pnlCorner.TabIndex = 12;
            // 
            // btnAllocate
            // 
            this.btnAllocate.Location = new System.Drawing.Point(312, 6);
            this.btnAllocate.Name = "btnAllocate";
            this.btnAllocate.Size = new System.Drawing.Size(64, 21);
            this.btnAllocate.TabIndex = 14;
            this.btnAllocate.Text = "Allocate";
            this.btnAllocate.Visible = false;
            this.btnAllocate.Click += new System.EventHandler(this.btnAllocate_Click);
            // 
            // btnVelocity
            // 
            this.btnVelocity.Location = new System.Drawing.Point(184, 6);
            this.btnVelocity.Name = "btnVelocity";
            this.btnVelocity.Size = new System.Drawing.Size(94, 21);
            this.btnVelocity.TabIndex = 13;
            this.btnVelocity.Text = "Velocity Method";
            this.btnVelocity.Visible = false;
            this.btnVelocity.Click += new System.EventHandler(this.btnVelocity_Click);
            // 
            // gbxAvg
            // 
            this.gbxAvg.Controls.Add(this.rbSet);
            this.gbxAvg.Controls.Add(this.rbAllStores);
            this.gbxAvg.Location = new System.Drawing.Point(8, 0);
            this.gbxAvg.Name = "gbxAvg";
            this.gbxAvg.Size = new System.Drawing.Size(160, 28);
            this.gbxAvg.TabIndex = 12;
            this.gbxAvg.TabStop = false;
            this.gbxAvg.Visible = false;
            // 
            // rbSet
            // 
            this.rbSet.Location = new System.Drawing.Point(76, 8);
            this.rbSet.Name = "rbSet";
            this.rbSet.Size = new System.Drawing.Size(50, 16);
            this.rbSet.TabIndex = 13;
            this.rbSet.Text = "Set";
            this.toolTip1.SetToolTip(this.rbSet, "View Set average");
            this.rbSet.CheckedChanged += new System.EventHandler(this.rbSet_CheckedChanged);
            // 
            // rbAllStores
            // 
            this.rbAllStores.Location = new System.Drawing.Point(8, 8);
            this.rbAllStores.Name = "rbAllStores";
            this.rbAllStores.Size = new System.Drawing.Size(72, 16);
            this.rbAllStores.TabIndex = 12;
            this.rbAllStores.Text = "All Stores";
            this.toolTip1.SetToolTip(this.rbAllStores, "View All Stores average");
            this.rbAllStores.CheckedChanged += new System.EventHandler(this.rbAllStores_CheckedChanged);
            // 
            // VerticalSplitter1
            // 
            this.VerticalSplitter1.Location = new System.Drawing.Point(80, 30);
            this.VerticalSplitter1.Name = "VerticalSplitter1";
            this.VerticalSplitter1.Size = new System.Drawing.Size(1, 296);
            this.VerticalSplitter1.TabIndex = 2;
            this.VerticalSplitter1.TabStop = false;
            this.VerticalSplitter1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.VerticalSplitter1_SplitterMoved);
            this.VerticalSplitter1.DoubleClick += new System.EventHandler(this.VerticalSplitter1_DoubleClick);
            // 
            // pnlScrollBars
            // 
            this.pnlScrollBars.Controls.Add(this.vScrollBar2);
            this.pnlScrollBars.Controls.Add(this.s4);
            this.pnlScrollBars.Controls.Add(this.vScrollBar1);
            this.pnlScrollBars.Controls.Add(this.s8);
            this.pnlScrollBars.Controls.Add(this.vScrollBar3);
            this.pnlScrollBars.Controls.Add(this.s12);
            this.pnlScrollBars.Controls.Add(this.vScrollBar4);
            this.pnlScrollBars.Controls.Add(this.pnlSpacer);
            this.pnlScrollBars.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlScrollBars.Location = new System.Drawing.Point(1227, 30);
            this.pnlScrollBars.Name = "pnlScrollBars";
            this.pnlScrollBars.Size = new System.Drawing.Size(17, 296);
            this.pnlScrollBars.TabIndex = 3;
            // 
            // vScrollBar2
            // 
            this.vScrollBar2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar2.Location = new System.Drawing.Point(0, 66);
            this.vScrollBar2.Name = "vScrollBar2";
            this.vScrollBar2.Size = new System.Drawing.Size(17, 97);
            this.vScrollBar2.TabIndex = 1;
            this.vScrollBar2.ValueChanged += new System.EventHandler(this.vScrollBar2_ValueChanged);
            // 
            // s4
            // 
            this.s4.Dock = System.Windows.Forms.DockStyle.Top;
            this.s4.Location = new System.Drawing.Point(0, 64);
            this.s4.Name = "s4";
            this.s4.Size = new System.Drawing.Size(17, 2);
            this.s4.TabIndex = 5;
            this.s4.TabStop = false;
            this.s4.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.s4_SplitterMoved);
            this.s4.DoubleClick += new System.EventHandler(this.g4SplitterDoubleClick);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.vScrollBar1.LargeChange = 1;
            this.vScrollBar1.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar1.Maximum = 0;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 64);
            this.vScrollBar1.TabIndex = 0;
            // 
            // s8
            // 
            this.s8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.s8.Location = new System.Drawing.Point(0, 163);
            this.s8.MinExtra = 15;
            this.s8.MinSize = 15;
            this.s8.Name = "s8";
            this.s8.Size = new System.Drawing.Size(17, 2);
            this.s8.TabIndex = 3;
            this.s8.TabStop = false;
            this.s8.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
            this.s8.DoubleClick += new System.EventHandler(this.g7SplitterDoubleClick);
            // 
            // vScrollBar3
            // 
            this.vScrollBar3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.vScrollBar3.Location = new System.Drawing.Point(0, 165);
            this.vScrollBar3.Name = "vScrollBar3";
            this.vScrollBar3.Size = new System.Drawing.Size(17, 72);
            this.vScrollBar3.TabIndex = 2;
            this.vScrollBar3.ValueChanged += new System.EventHandler(this.vScrollBar3_ValueChanged);
            // 
            // s12
            // 
            this.s12.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.s12.Location = new System.Drawing.Point(0, 237);
            this.s12.Name = "s12";
            this.s12.Size = new System.Drawing.Size(17, 2);
            this.s12.TabIndex = 1;
            this.s12.TabStop = false;
            this.s12.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
            this.s12.DoubleClick += new System.EventHandler(this.g10SplitterDoubleClick);
            // 
            // vScrollBar4
            // 
            this.vScrollBar4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.vScrollBar4.Location = new System.Drawing.Point(0, 239);
            this.vScrollBar4.Name = "vScrollBar4";
            this.vScrollBar4.Size = new System.Drawing.Size(17, 40);
            this.vScrollBar4.TabIndex = 3;
            this.vScrollBar4.ValueChanged += new System.EventHandler(this.vScrollBar4_ValueChanged);
            // 
            // pnlSpacer
            // 
            this.pnlSpacer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSpacer.Location = new System.Drawing.Point(0, 279);
            this.pnlSpacer.Name = "pnlSpacer";
            this.pnlSpacer.Size = new System.Drawing.Size(17, 17);
            this.pnlSpacer.TabIndex = 4;
            // 
            // pnlTotals
            // 
            this.pnlTotals.Controls.Add(this.g5);
            this.pnlTotals.Controls.Add(this.s2);
            this.pnlTotals.Controls.Add(this.g2);
            this.pnlTotals.Controls.Add(this.s6);
            this.pnlTotals.Controls.Add(this.g8);
            this.pnlTotals.Controls.Add(this.s10);
            this.pnlTotals.Controls.Add(this.g11);
            this.pnlTotals.Controls.Add(this.hScrollBar2);
            this.pnlTotals.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTotals.Location = new System.Drawing.Point(81, 30);
            this.pnlTotals.Name = "pnlTotals";
            this.pnlTotals.Size = new System.Drawing.Size(85, 296);
            this.pnlTotals.TabIndex = 4;
            // 
            // g5
            // 
            this.g5.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g5.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g5.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g5.ExtendLastCol = true;
            this.g5.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g5.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g5.Location = new System.Drawing.Point(0, 65);
            this.g5.Name = "g5";
            this.g5.Rows.DefaultSize = 17;
            this.g5.Rows.Fixed = 0;
            this.g5.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g5.Size = new System.Drawing.Size(85, 100);
            this.g5.TabIndex = 14;
            this.g5.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g5_BeforeScroll);
            this.g5.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g5.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g5.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g5.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g5_OwnerDrawCell);
            this.g5.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // s2
            // 
            this.s2.Dock = System.Windows.Forms.DockStyle.Top;
            this.s2.Location = new System.Drawing.Point(0, 64);
            this.s2.Name = "s2";
            this.s2.Size = new System.Drawing.Size(85, 1);
            this.s2.TabIndex = 10;
            this.s2.TabStop = false;
            this.s2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.s2_SplitterMoved);
            this.s2.DoubleClick += new System.EventHandler(this.g4SplitterDoubleClick);
            // 
            // g2
            // 
            this.g2.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None;
            this.g2.AllowEditing = false;
            this.g2.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g2.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g2.ColumnInfo = "10,0,0,0,0,85,Columns:";
            this.g2.ContextMenu = this.g1ContextMenu;
            this.g2.Dock = System.Windows.Forms.DockStyle.Top;
            this.g2.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g2.ExtendLastCol = true;
            this.g2.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g2.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g2.Location = new System.Drawing.Point(0, 0);
            this.g2.Name = "g2";
            this.g2.Rows.DefaultSize = 17;
            this.g2.Rows.Fixed = 0;
            this.g2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g2.Size = new System.Drawing.Size(85, 64);
            this.g2.TabIndex = 0;
            this.g2.BeforeResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeResizeColumn);
            this.g2.AfterResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_AfterResizeColumn);
            this.g2.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g2_BeforeScroll);
            this.g2.DragDrop += new System.Windows.Forms.DragEventHandler(this.g2_DragDrop);
            this.g2.DragEnter += new System.Windows.Forms.DragEventHandler(this.GridDragEnter);
            this.g2.DragOver += new System.Windows.Forms.DragEventHandler(this.GridDragOver);
            this.g2.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.GridQueryContinueDrag);
            this.g2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            this.g2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
            this.g2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GridMouseUp);
            // 
            // s6
            // 
            this.s6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.s6.Location = new System.Drawing.Point(0, 165);
            this.s6.MinExtra = 15;
            this.s6.MinSize = 15;
            this.s6.Name = "s6";
            this.s6.Size = new System.Drawing.Size(85, 1);
            this.s6.TabIndex = 8;
            this.s6.TabStop = false;
            this.s6.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
            this.s6.DoubleClick += new System.EventHandler(this.g7SplitterDoubleClick);
            // 
            // g8
            // 
            this.g8.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g8.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g8.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.g8.ExtendLastCol = true;
            this.g8.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g8.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g8.Location = new System.Drawing.Point(0, 166);
            this.g8.Name = "g8";
            this.g8.Rows.DefaultSize = 17;
            this.g8.Rows.Fixed = 0;
            this.g8.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g8.Size = new System.Drawing.Size(85, 72);
            this.g8.TabIndex = 16;
            this.g8.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g8_BeforeScroll);
            this.g8.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g8.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g8.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g8.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g8_OwnerDrawCell);
            this.g8.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g8.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // s10
            // 
            this.s10.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.s10.Location = new System.Drawing.Point(0, 238);
            this.s10.Name = "s10";
            this.s10.Size = new System.Drawing.Size(85, 1);
            this.s10.TabIndex = 6;
            this.s10.TabStop = false;
            this.s10.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
            this.s10.DoubleClick += new System.EventHandler(this.g10SplitterDoubleClick);
            // 
            // g11
            // 
            this.g11.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g11.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g11.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.g11.ExtendLastCol = true;
            this.g11.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g11.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g11.Location = new System.Drawing.Point(0, 239);
            this.g11.Name = "g11";
            this.g11.Rows.DefaultSize = 17;
            this.g11.Rows.Fixed = 0;
            this.g11.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g11.Size = new System.Drawing.Size(85, 40);
            this.g11.TabIndex = 18;
            this.g11.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g11_BeforeScroll);
            this.g11.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g11.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g11.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g11.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g11_OwnerDrawCell);
            this.g11.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g11.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g11.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // hScrollBar2
            // 
            this.hScrollBar2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar2.Location = new System.Drawing.Point(0, 279);
            this.hScrollBar2.Name = "hScrollBar2";
            this.hScrollBar2.Size = new System.Drawing.Size(85, 17);
            this.hScrollBar2.TabIndex = 4;
            this.hScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
            // 
            // VerticalSplitter2
            // 
            this.VerticalSplitter2.Location = new System.Drawing.Point(166, 30);
            this.VerticalSplitter2.MinExtra = 0;
            this.VerticalSplitter2.MinSize = 0;
            this.VerticalSplitter2.Name = "VerticalSplitter2";
            this.VerticalSplitter2.Size = new System.Drawing.Size(1, 296);
            this.VerticalSplitter2.TabIndex = 5;
            this.VerticalSplitter2.TabStop = false;
            this.VerticalSplitter2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.VerticalSplitter2_SplitterMoved);
            this.VerticalSplitter2.DoubleClick += new System.EventHandler(this.VerticalSplitter2_DoubleClick);
            // 
            // pnlData
            // 
            this.pnlData.Controls.Add(this.g6);
            this.pnlData.Controls.Add(this.s3);
            this.pnlData.Controls.Add(this.g3);
            this.pnlData.Controls.Add(this.s7);
            this.pnlData.Controls.Add(this.g9);
            this.pnlData.Controls.Add(this.s11);
            this.pnlData.Controls.Add(this.g12);
            this.pnlData.Controls.Add(this.hScrollBar3);
            this.pnlData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlData.Location = new System.Drawing.Point(167, 30);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(1060, 296);
            this.pnlData.TabIndex = 6;
            // 
            // g6
            // 
            this.g6.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g6.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g6.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g6.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g6.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g6.Location = new System.Drawing.Point(0, 65);
            this.g6.Name = "g6";
            this.g6.Rows.DefaultSize = 17;
            this.g6.Rows.Fixed = 0;
            this.g6.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g6.Size = new System.Drawing.Size(1060, 100);
            this.g6.TabIndex = 15;
            this.g6.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g6_BeforeScroll);
            this.g6.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g6.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g6.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g6.CellChanged += new C1.Win.C1FlexGrid.RowColEventHandler(this.g6_CellChanged);
            this.g6.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g6_OwnerDrawCell);
            this.g6.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // s3
            // 
            this.s3.Dock = System.Windows.Forms.DockStyle.Top;
            this.s3.Location = new System.Drawing.Point(0, 64);
            this.s3.Name = "s3";
            this.s3.Size = new System.Drawing.Size(1060, 1);
            this.s3.TabIndex = 10;
            this.s3.TabStop = false;
            this.s3.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.s3_SplitterMoved);
            this.s3.DoubleClick += new System.EventHandler(this.g4SplitterDoubleClick);
            // 
            // g3
            // 
            this.g3.AllowEditing = false;
            this.g3.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g3.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.Both;
            this.g3.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g3.ColumnInfo = "10,0,0,0,0,85,Columns:";
            this.g3.ContextMenu = this.g1ContextMenu;
            this.g3.Dock = System.Windows.Forms.DockStyle.Top;
            this.g3.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g3.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g3.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g3.Location = new System.Drawing.Point(0, 0);
            this.g3.Name = "g3";
            this.g3.Rows.DefaultSize = 17;
            this.g3.Rows.Fixed = 0;
            this.g3.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g3.Size = new System.Drawing.Size(1060, 64);
            this.g3.TabIndex = 0;
            this.g3.BeforeResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeResizeColumn);
            this.g3.AfterResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_AfterResizeColumn);
            this.g3.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g3_BeforeScroll);
            this.g3.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g3_OwnerDrawCell);
            this.g3.DragDrop += new System.Windows.Forms.DragEventHandler(this.g3_DragDrop);
            this.g3.DragEnter += new System.Windows.Forms.DragEventHandler(this.GridDragEnter);
            this.g3.DragOver += new System.Windows.Forms.DragEventHandler(this.GridDragOver);
            this.g3.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.GridQueryContinueDrag);
            this.g3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            this.g3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
            this.g3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GridMouseUp);
            // 
            // s7
            // 
            this.s7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.s7.Location = new System.Drawing.Point(0, 165);
            this.s7.MinExtra = 15;
            this.s7.MinSize = 15;
            this.s7.Name = "s7";
            this.s7.Size = new System.Drawing.Size(1060, 1);
            this.s7.TabIndex = 8;
            this.s7.TabStop = false;
            this.s7.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
            this.s7.DoubleClick += new System.EventHandler(this.g7SplitterDoubleClick);
            // 
            // g9
            // 
            this.g9.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g9.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g9.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.g9.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g9.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g9.Location = new System.Drawing.Point(0, 166);
            this.g9.Name = "g9";
            this.g9.Rows.DefaultSize = 17;
            this.g9.Rows.Fixed = 0;
            this.g9.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g9.Size = new System.Drawing.Size(1060, 72);
            this.g9.TabIndex = 17;
            this.g9.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g9_BeforeScroll);
            this.g9.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g9.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g9.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g9.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g9_OwnerDrawCell);
            this.g9.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g9.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // s11
            // 
            this.s11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.s11.Location = new System.Drawing.Point(0, 238);
            this.s11.Name = "s11";
            this.s11.Size = new System.Drawing.Size(1060, 1);
            this.s11.TabIndex = 6;
            this.s11.TabStop = false;
            this.s11.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
            this.s11.DoubleClick += new System.EventHandler(this.g10SplitterDoubleClick);
            // 
            // g12
            // 
            this.g12.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g12.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g12.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g12.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.g12.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g12.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g12.Location = new System.Drawing.Point(0, 239);
            this.g12.Name = "g12";
            this.g12.Rows.DefaultSize = 17;
            this.g12.Rows.Fixed = 0;
            this.g12.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g12.Size = new System.Drawing.Size(1060, 40);
            this.g12.TabIndex = 19;
            this.g12.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g12_BeforeScroll);
            this.g12.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g12.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g12.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g12.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g12_OwnerDrawCell);
            this.g12.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g12.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g12.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // hScrollBar3
            // 
            this.hScrollBar3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar3.Location = new System.Drawing.Point(0, 279);
            this.hScrollBar3.Name = "hScrollBar3";
            this.hScrollBar3.Size = new System.Drawing.Size(1060, 17);
            this.hScrollBar3.TabIndex = 4;
            this.hScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar3_Scroll);
            // 
            // StyleView
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1244, 326);
            this.Controls.Add(this.pnlData);
            this.Controls.Add(this.VerticalSplitter2);
            this.Controls.Add(this.pnlTotals);
            this.Controls.Add(this.pnlScrollBars);
            this.Controls.Add(this.VerticalSplitter1);
            this.Controls.Add(this.pnlRowHeaders);
            this.Controls.Add(this.pnlTop);
            this.FormLoaded = true;
            this.MinimumSize = new System.Drawing.Size(0, 300);
            this.Name = "StyleView";
            this.toolTip1.SetToolTip(this, "Apply current changes");
            this.Activated += new System.EventHandler(this.StyleView_Activated);
            this.Deactivate += new System.EventHandler(this.StyleView_Deactivate);
            this.Load += new System.EventHandler(this.StyleView_Load);
            this.Controls.SetChildIndex(this.pnlTop, 0);
            this.Controls.SetChildIndex(this.pnlRowHeaders, 0);
            this.Controls.SetChildIndex(this.VerticalSplitter1, 0);
            this.Controls.SetChildIndex(this.pnlScrollBars, 0);
            this.Controls.SetChildIndex(this.pnlTotals, 0);
            this.Controls.SetChildIndex(this.VerticalSplitter2, 0);
            this.Controls.SetChildIndex(this.pnlData, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.gbxGroupBy.ResumeLayout(false);
            this.pnlRowHeaders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g1)).EndInit();
            this.pnlCorner.ResumeLayout(false);
            this.gbxAvg.ResumeLayout(false);
            this.pnlScrollBars.ResumeLayout(false);
            this.pnlTotals.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g11)).EndInit();
            this.pnlData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g12)).EndInit();
            // START TT#5034 - AGallagher - In Style, Size, Summary Review Screen the Tool Tips do not appear for Attribute Set, View, Filter, and Method-Action Windows.  
            this.cmbAttributeSet.SetToolTip = "Attribute Sets";
            this.cmbView.SetToolTip = "Views";
            this.cmbFilter.SetToolTip = "Filters";
            this.cboAction.SetToolTip = "Actions";
            // END TT#5034 - AGallagher - In Style, Size, Summary Review Screen the Tool Tips do not appear for Attribute Set, View, Filter, and Method-Action Windows.
            this.ResumeLayout(false);

		}
		#endregion

		#region Variable Declarations
		private bool _hSplitMove;
		private bool _isSorting;	//I hate to use flags but at the moment I can't find another solution. This flag is used by g2.Click and g3.Click events. If this flag is true, those click events will perform sorting.
		private int DragStartColumn; //indicates which column is the column that started the drag/drop action.
		private DragState dragState;
		private static int BIGCHANGE = 100;	//the "100" happens to be our chosen number because it makes the scroll bar scroll a reasonable amount.
		private static int SMALLCHANGE = 1; //the "10" is for the same reason above.
		private const int BIGHORIZONTALCHANGE = 5;
		private int HeaderOrComponentGroups; //how many weeks or how many variables are there. This is used mainly by g3 whenever we need to iterate through the header/components.
		private int rowsPerStoreGroup; //in my own definition, I made a "StoreGroup" be all the rows between two store names. For this particular view it should always be 1.
		private FromGrid RightClickedFrom; //indicates which grid the user right clicked from.
		private System.Drawing.Bitmap picLock; //this picture will be put in a cell that's locked.
		private System.Drawing.Bitmap picStyle; //this picture is put on the "Theme Properties" button.
		private bool g1HasColsFrozen;
		private bool g2HasColsFrozen;
		private bool g3HasColsFrozen;
		private int LeftMostColBeforeFreeze1; //for g1
		private int LeftMostColBeforeFreeze2; //for g2
		private int LeftMostColBeforeFreeze3; //for g3
		private int UserSetSplitter1Position; //used to determine the width the user set the HEADERS panel.
		private int UserSetSplitter2Position; //used to determine the width the user set the TOTALS panel.
		private ApplicationSessionTransaction _trans;
		private SessionAddressBlock _sab;
        private ExplorerAddressBlock _eab;
		private Hashtable ColHeaders1 = null; //for show/hide columns in g1
		private Hashtable ColHeaders2 = null; //for show/hide columns in g2  
		private ArrayList _alColHeaders2 = null; //for show/hide columns in g2  
        private string _sortSeqColumn; // MID TRack 6079 qty allocated change not accepted after column sort

	
		//The following 3 vars are needed because we're doing many copy-paste
		//actions from other grid forms. For this particular form, the vars are 0
		//because there is only one item per "store group", and that one item
		//will be shown at all times.
		private int LastInvisibleRowsPerStoreGroup4 = 0; //some rows might be hidden. this is used mainly for determining which cell needs to display a border)
		private int LastInvisibleRowsPerStoreGroup7 = 0; //some rows might be hidden. this is used mainly for determining which cell needs to display a border)
		private int LastInvisibleRowsPerStoreGroup10 = 0;//some rows might be hidden. this is used mainly for determining which cell needs to display a border)
		

		//The following variables are used to store the location of mouse clicks
		//for various grids, because gX.MouseCol and gX.MouseRow don't always work.
		//One simple example to show the need for these variables is the case of 
		//context menus. The user right clicks on a cell, but have to move the mouse
		//furthur down the screen in order to reach a menu item. This action
		//affects the location of the mouse (obviously).
		//These locations will be caught in the "MouseDown" events of diff. grids.
		private int GridMouseRow;
		private int GridMouseCol;

		//private int g2MouseCol;
		//private int g3MouseCol;

		private int rowsPerGroup4;
		private int rowsPerGroup7;
		private int rowsPerGroup10;
		
		private ThemeProperties _frmThemeProperties; //for the theme properties dialog box.
		private Theme _theme;
		private bool _themeChanged = false;

		private SortGridViews frmSortGridViews; //for the sort grid dialog box.
		private System.Data.DataTable _g456GridTable;
		
		//		System.Threading.Thread _styleThread4;
		//		System.Threading.Thread _styleThread5;
		//		System.Threading.Thread _styleThread6;
		//		System.Threading.Thread _styleThread7;
		//		System.Threading.Thread _styleThread8;
		//		System.Threading.Thread _styleThread9;
		//		System.Threading.Thread _styleThread10;
		//		System.Threading.Thread _styleThread11;
		//		System.Threading.Thread _styleThread12;
		
		private bool _loading;
		private bool _changeRowStyle  = true; 
		private int _colsPerGroup1 = 1;
//		private int _colsPerGroup2 = 1;
//		private int _colsPerGroup3 = 1;
		structSort _structSort;
        private structSort _structSortSave; // MID Track 6079 zero qty allocated not accepted after column sort
        private string _sortCriteriaSave;       // MID Track 6079 zero qty allocated not accepted after column sort
        private bool _sortCriteriaChanged;      // MID Track 6079 zero qty allocated not accepted after column sort
        private bool _exporting = false;
        private bool _saveCurrentColumns = true;  // TT#2380 - JSmith - Column Changes in Size Review

		AllocationWaferGroup _wafers;
		AllocationWafer _wafer;
		AllocationWaferCell [,] _cells ;
//		private bool _value2Set = false;
//		private bool _value3Set = false;
//		private int _oldValue3 = 0;
//		ScrollEventType _scroll2EventType;  
//		ScrollEventType _scroll3EventType;  
		private const string _invalidCellValue = " "; 
		private DataView _g456DataView;
		private object _holdValue;
		private int _hdrRow;
		private int _compRow;
		private const int _varProfIndex = 2;
		private bool _setHeaderTags = true;
		private bool _resetV1Splitter;
		private Hashtable CellRows = null; // cross ref to wafer cell for updating 
		private int _curGroupBy = Include.Undefined;
		private int _curAttribute = Include.Undefined;
		private int _curAttributeSet = Include.Undefined;
		private eAllocationSelectionViewType _curViewType;
		private bool _initalLoad;
		private bool _isScrolling; 
		private bool _arrowScroll; 
		private bool _doResize = true; 
//		private bool _hScrollBar3Moved = false; 
        //private bool _vScrollBar2Moved = false;
        private bool _vScrollBar2Moved = false; // TT#1073 - RMatelic - GA style review use the scroll bar to the right and the rows are out of sync
		private bool _showNeedGrid;
//		private bool _bindingFilter;
		private bool _updateFromVelocityWindow = false; // BEGIN END MID Track #2761
		private MIDRetail.Windows.AllocationWorkspaceExplorer _awExplorer;
		private string _thisTitle;
		private AllocationHeaderProfileList _headerList;
//		private Hashtable _readOnlyList = null;
		private int _lastFilterValue;
//		private bool _doThis = true;
//		private DataTable _dtFilterType = null;
//		private DataView _dvFilterType;
		private QuickFilter _quickFilter;
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
		private ArrayList _masterKeyList = new ArrayList();
// (CSMITH) - END MID Track #3219
		private ArrayList _headerArrayList = null;
//		private ArrayList _componentArrayList = null;
		private DataTable _conditionDataTable = null;
		private SortedList _headerSortedList = null;
		private DataTable _componentDataTable = null;
		private DataTable _variableDataTable = null;
		private DataTable _colorDataTable = null;
		private DataTable _packDataTable = null;
//		private ListDictionary _ldRules; 
		private ListDictionary _ldRulesVelocity; 
		private string _lblHeader = null;
		private string _lblColor = null;
		private string _lblPack = null;
		private string _lblComponent = null;
		private string _lblStore = null;
		private string _lblCondition = null;
		private string _lblVariable = null;
		private string _lblValue = null;
		private string _totalVariableName;
		private eQuickFilterType _quickFilterType;
		private int _foundColumn;
		private QuickFilterData _quickFilterData;
		private FunctionSecurityProfile _allocationReviewSummarySecurity;
		private FunctionSecurityProfile _allocationReviewSizeSecurity;
        private FunctionSecurityProfile _allocationReviewAssortmentSecurity;
		private int _changedCellRow;
		private int _changedCellCol;
		private C1.Win.C1FlexGrid.C1FlexGrid _changedGrid = null;
		private string _scroll2Direction;
        private string _includeCurrentSetLabel = null;
        private string _includeAllSetsLabel = null;
        private ProfileList _storeGroupLevelProfileList;
		private string _windowName;
        // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
        private bool _currentRedrawState;
        // End Track #6371
        private bool _columnAdded = false;  // MID Track #6410 - Perf slow chging column chooser in Velocity

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private FunctionSecurityProfile _userViewSecurity;
        private FunctionSecurityProfile _globalViewSecurity;
        private bool _bindingView = false;      
        private int _lastSelectedViewRID;
        private int _viewRID;
        private int _saveAsDetailViewUserRID;
        private string _saveAsDetailViewName;
        private eLayoutID _layoutID; 
        private ArrayList _userRIDList = null;
        private DataTable _dtViews = null;
        private GridViewData _gridViewData;
        private UserGridView _userGridView;
        // End TT#231
        private ArrayList _builtVariables = null;  // TT#318 - RMatelic - View column position not retaining  
        private bool _buttonChanged = false;
        private bool _changingView = false;
        private ArrayList _curColumnsG1 = new ArrayList();       // Begin TT#358/#334/#363 - RMatelic - Velocity View column display issues  
        private ArrayList _curColumnsG2 = new ArrayList();
        private ArrayList _curColumnsG3 = new ArrayList();
        private Hashtable _headerAllColHash = new Hashtable();
        private bool _groupByChanged = false;                  // End TT#358/#334/#363
        private bool _applyPending = false;  // Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set
        private bool _applyPendingMsgDisplayed = false;  // Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set
        private bool _loadedFromAssrt = false; //TT#793-MD-DOConnell - Ran balance size billaterally on a receipt header in an assortment and receive a null reference exception
		#endregion

		#region Misc.
		
		//		private void HandleMIDException(MIDException MIDexc)
		//		{
		//			string Title, errLevel, Msg; 
		//			MessageBoxIcon icon;
		//			MessageBoxButtons buttons;
		//			buttons = MessageBoxButtons.OK;
		//			switch (MIDexc.ErrorLevel)
		//			{
		//				case eErrorLevel.severe:
		//					icon = MessageBoxIcon.Stop;
		//					errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Severe));
		//					break;
		//				
		//				case eErrorLevel.information:
		//					icon = MessageBoxIcon.Information;
		//					errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Information));
		//					break;
		//				
		//				case eErrorLevel.warning:
		//					icon = MessageBoxIcon.Warning;
		//					errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Warning));
		//					break;
		//				
		//				default:
		//					icon = MessageBoxIcon.Stop;
		//					errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Severe));
		//					break;
		//			}
		//			if (MIDexc.InnerException != null)
		//			{
		//				Title = errLevel + " - " + MIDexc.Message;
		//				Msg = MIDexc.InnerException.Message;
		//			}
		//			else
		//			{
		//				Title = errLevel;
		//				Msg = MIDexc.Message;
		//			}
		//			MessageBox.Show(this, Msg, Title,
		//				buttons, icon );
		//		}
		protected void FormatForXP(Control ctl)
		{
			try
			{
				foreach (Control c in ctl.Controls)
					FormatForXP(c);

				if (ctl.GetType().BaseType == typeof(ButtonBase))
					((ButtonBase)ctl).FlatStyle = FlatStyle.System;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		#endregion

		private void StyleView_Load(object sender, System.EventArgs e)
		{
			try     // jae catch errors in caller so that form is closed when an error occurs
			{
                _includeAllSetsLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeAllSets);
                _includeCurrentSetLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeCurrentSet);
				FunctionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewStyle);
				
				// BEGIN MID Track #2551 - security not working
				_allocationReviewSummarySecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSummary);
                if (_sab.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                {
                    _allocationReviewSizeSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSize);
                }
                // Begin TT#2 - JSmith - Assortment Security
                //_allocationReviewAssortmentSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewAssortment);
                _allocationReviewAssortmentSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReview);
                // End TT#2
                // END MID Track #2551

				_windowName = MIDText.GetTextOnly(eMIDTextCode.frm_StyleReview);

                //BEGIN TT#793-MD-DOConnell - Ran balance size billaterally on a receipt header in an assortment and receive a null reference exception
                if (_trans.AssortmentViewSelectionCriteria == null)
                {
                    _loadedFromAssrt = false;
                }
                else
                {
                    _loadedFromAssrt = true;
                }
                //END TT#793-MD-DOConnell - Ran balance size billaterally on a receipt header in an assortment and receive a null reference exception

				_loading = true;
				_initalLoad = true;
				//				SetGridRedraws(false);

                // begin TT#1099 MOVED code to StyleView_Load
                // begin TT#59 Implement Store Temp Locks
                //
                // mnuHeaderAllocationCriteria
                //
                if (this.SAB.AllowDebugging)
                {
                    this.mnuHeaderAllocationCriteria.Index = 4;
                    this.mnuHeaderAllocationCriteria.Text = "Header Allocation Criteria";
                    this.mnuHeaderAllocationCriteria.Click += new System.EventHandler(this.mnuHeaderAllocationCriteria_Click);
                }
                // end TT#59 Implement Store Temp Locks
                // end TT#1099 MOVED code to StyleView_Load

				BuildMenu();

                // Begin TT#231/TT#318 - RMatelic - Add Views to Velocity Matrix and Store Detail
                _builtVariables = new ArrayList();
                _userRIDList = new ArrayList();  
               
                // Begin TT#454 - RMatelic - Add Views in Style Review
                _gridViewData = new GridViewData();
                _userGridView = new UserGridView();
                if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
                {
                    this.rbAllStores.Enabled = true;
                    this.rbSet.Enabled = true;
                    this.btnVelocity.Enabled = true;
                    MIDRetail.Windows.frmVelocityMethod frmVelocity = (MIDRetail.Windows.frmVelocityMethod)_trans.VelocityWindow;
                    _globalViewSecurity = frmVelocity.MethodDetailViewGlobalSecurity;
                    _userViewSecurity = frmVelocity.MethodDetailViewUserSecurity;
                    _layoutID = eLayoutID.velocityStoreDetailGrid;
                }
                else
                {
                    _globalViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalStyleReview);
                    _userViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUserStyleReview);
                    _layoutID = eLayoutID.styleReviewGrid; 
                }
                if (_globalViewSecurity.AllowView)
                {
                    _userRIDList.Add(Include.GlobalUserRID);
                }
                if (_userViewSecurity.AllowView)
                {
                    _userRIDList.Add(SAB.ClientServerSession.UserRID);
                }
                BindViewCombo();
                _viewRID = _userGridView.UserGridView_Read(SAB.ClientServerSession.UserRID, _layoutID);
                // End TT#454

                _lblStore = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreSingular);
                // End TT#231/TT#318

				GetSelectionCriteria();
				
				g1HasColsFrozen = false;
				g2HasColsFrozen = false;
				g3HasColsFrozen = false;
				SetGridRedraws(false);
				//The following 6 lines are used to allow manually drawing borders around cells.
				g3.DrawMode = DrawModeEnum.OwnerDraw;
				g4.DrawMode = DrawModeEnum.OwnerDraw;
				g5.DrawMode = DrawModeEnum.OwnerDraw;
				g6.DrawMode = DrawModeEnum.OwnerDraw;
				g7.DrawMode = DrawModeEnum.OwnerDraw;
				g8.DrawMode = DrawModeEnum.OwnerDraw;
				g9.DrawMode = DrawModeEnum.OwnerDraw;
				g10.DrawMode = DrawModeEnum.OwnerDraw;
				g11.DrawMode = DrawModeEnum.OwnerDraw;
				g12.DrawMode = DrawModeEnum.OwnerDraw;

				BindStoreAttrComboBox();
                //BEGIN TT#6-MD-VStuart - Single Store Select
                if (this.cmbStoreAttribute.SelectedValue != null)
                    PopulateStoreAttributeSet(this.cmbStoreAttribute.SelectedValue.ToString());
                //END TT#6-MD-VStuart - Single Store Select

                BindFilterComboBox();
				BindActionCombo();

                //Begin Track #5858 - Kjohnson - Validating store security only
              //cmbStoreAttribute.Tag = "IgnoreMouseWheel";
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cmbStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cmbStoreAttribute, eMIDControlCode.form_StyleReview);
                //cmbFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cmbFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                // Begin TT#1118 - JSmith - Undesirable curser position
                //cmbStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cmbStoreAttribute, eMIDControlCode.form_StyleReview, FunctionSecurity, true);
                cmbStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cmbStoreAttribute, eMIDControlCode.form_StyleReview, true, FunctionSecurity, true);
                cmbView.Tag = "IgnoreMouseWheel";
                // End TT#1118
                // Begin TT#1118 - JSmith - Undesirable curser position
                //cmbFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cmbFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, true);
                //BEGIN TT#6-MD-VStuart - Single Store Select
                cmbFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cmbFilter.ComboBox, eMIDControlCode.field_Filter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, true, FunctionSecurity, true);
                //END TT#6-MD-VStuart - Single Store Select
                // End TT#1118
                // End TT#44
                //Begin Track #5858 - Kjohnson
                cmbAttributeSet.Tag = "IgnoreMouseWheel";
                cboAction.Tag = "IgnoreMouseWheel";
	
				//this picture will be put in cells to visually indicate to the user
				//that this cell is locked (cannot be "spread" to new data).
				picLock = new Bitmap(GraphicsDirectory + "\\lock.gif");
				picStyle = new Bitmap(GraphicsDirectory + "\\style.gif");
				
				rowsPerStoreGroup = 1;

				HeaderOrComponentGroups = _trans.AllocationCriteriaHeaderCount;

				if ( _trans.AllocationGroupBy == Convert.ToInt32(eAllocationStyleViewGroupBy.Header, CultureInfo.CurrentUICulture) )
				{	
					rbHeader.Checked = true;
					_hdrRow = 0;
					_compRow = 1;
				}
				else
				{
					rbComponent.Checked = true;
					_hdrRow = 1;
					_compRow = 0;
				}
				
                //_lblStore =  MIDText.GetTextOnly(eMIDTextCode.lbl_StoreSingular);     // TT#318
				//	Jellis		_sab.ClientServerSession.Audit.Add_Msg(
				//					eMIDMessageLevel.Debug, 
				//					"Before loading grids", "SKU Review");
				FormatGrids1to12();
				//	Jellis		_sab.ClientServerSession.Audit.Add_Msg(
				//					eMIDMessageLevel.Debug, 
				//					"After loading grids", "SKU Review");
				_structSort = new structSort();
                _structSortSave = _structSort; // MID Track 6079 zero qty allocated not accepted after sort columns
                _sortCriteriaSave = string.Empty; // MID Track 6079 zero qty allocated not accepted after sort columns
                _sortSeqColumn = g4.Cols[1].Name + " " + "ASC"; // MID Track 6079 zero qty allocated not accepted after sort columns
                _sortCriteriaChanged = false; // MID Track 6079 zero qty allocated not accepted after sort columns
				SortByDefault();
				//	Jellis		_sab.ClientServerSession.Audit.Add_Msg(
				//					eMIDMessageLevel.Debug, 
				//					"Before formatting grids", "SKU Review");
				AssignTag();
                _themeChanged = true;  // TT#4188 - JSmith - Themes -  Row height does not save in conistant format across the different review screens.
                ChangeStyle(false);		// TT#1628-MD - stodd - Style review grid out of sync in GA mode
                _themeChanged = false;  // TT#4188 - JSmith - Themes -  Row height does not save in conistant format across the different review screens.
				ApplyPreferences();
				//	Jellis		_sab.ClientServerSession.Audit.Add_Msg(
				//					eMIDMessageLevel.Debug, 
				//					"After formatting grids", "SKU Review");

				// BEGIN MID Track #2747
				//HilightSelectedSet();
				SetRowSplitPosition8();
				ChangeStylesG7();
				// END MID Track #2747
				SetText();
				// Begin MID Track #2551 - check style security for actions
				bool readOnly = false;
				if (!_trans.AnalysisOnly)
				{
					foreach (AllocationHeaderProfile ahp in _headerList)
					{	
						HierarchyNodeSecurityProfile securityNode = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(ahp.StyleHnRID, (int)eSecurityTypes.Allocation);
						if (!securityNode.AllowUpdate)
						{
							readOnly = true;
						}
					}
				}
				if (_trans.DataState == eDataState.ReadOnly || readOnly)
				{
					FunctionSecurity.SetReadOnly();
				}
				// End MID Track #2551
				SetReadOnly(FunctionSecurity.AllowUpdate);
				// BEGIN MID Track #2551 - security not working
				// since read only may protect the radio buttons, need to always unprotect 
				this.rbHeader.Enabled = true;
				this.rbComponent.Enabled = true;

                // Begin TT#1662-MD - RMatelic - In Style Review screen, columns not aligning and totals not populating >>Set Default View if no view assigned
                if (_viewRID == Include.NoRID)
                {
                    if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
                    {
                        _viewRID = Include.DefaultVelocityDetailViewRID;
                    }
                    else
                    {
                        _viewRID = Include.DefaultStyleViewRID;
                    }
                }
                // End TT#1662-MD 

                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
                // Begin TT#454 - RMatelic - Add Views in Style Review - comment out 'if ...
                //if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
                //{
                    if (_viewRID != Include.NoRID)
                    {
                        hScrollBar2.Tag = new ScrollBarValueChanged(ChangeHScrollBar2Value);    // Begin TT#318 - RMatelic - View column position not retaining  
                        hScrollBar3.Tag = new ScrollBarValueChanged(ChangeHScrollBar3Value);    // End TT#318  
                        cmbView.SelectedValue = _viewRID;
                        //this.cmbView_SelectionChangeCommitted(source, new EventArgs()); // TT#294-MD - JSmith - When Opening style review, the view does not open that is selected
                    }
                //}
                // End TT#454
                // End TT#231

				SetActionListEnabled();
				if (_trans.AnalysisOnly)
				{
					btnApply.Enabled = false;
				}	
				else
					btnApply.Enabled = (g4.Rows.Count > 0 && (_trans.DataState == eDataState.Updatable));
				// END MID Track #2551
		
				//format for XP, if applicable
				if (Environment.OSVersion.Version.Major > 4 && Environment.OSVersion.Version.Minor > 0 
					&& System.IO.File.Exists(Application.ExecutablePath + ".manifest"))
					FormatForXP(this);
				
				//SetGridRedraws(true);
				FormLoaded = true;
				SaveCurrentSettings();
				SetNeedAnalysisGrid(g4);
				
				ResizeRows();
				g1.Height = g1.Rows[0].HeightDisplay + s4.Height + 3; 
				pnlCorner.Height = g2.Rows[0].HeightDisplay;
				
				SetV1SplitPosition();
				SetV2SplitPosition();
				UserSetSplitter1Position = VerticalSplitter1.SplitPosition;
				UserSetSplitter2Position = VerticalSplitter2.SplitPosition;
                //SetRowSplitPosition4();       // TT#3029 - RMatelic - Style Summary Review Screen Line Up >> comment out here and move below
//				SetRowSplitPosition8();
                //SetRowSplitPosition12();      // TT#3029 - RMatelic - Style Summary Review Screen Line Up >> comment out here and move below
				hScrollBar2.Tag = new ScrollBarValueChanged(ChangeHScrollBar2Value);
				hScrollBar3.Tag = new ScrollBarValueChanged(ChangeHScrollBar3Value);
				SetHScrollBar1Parameters();
				SetHScrollBar2Parameters();
				SetHScrollBar3Parameters();
				
				SetGridRedraws(true);
				_loading = false;
				
				// Some machines that are being run as remote do not show the Need Grid correctly; the 1st vertical
				// splitter bar is not in the correct location. So, as an attmpt to work around the issue,
				// the following simulates a double click on the splitter bar which resets the splitter location.   
				// 
				System.EventArgs args = new System.EventArgs();
				VerticalSplitter1_DoubleClick(VerticalSplitter1, args);
                // Begin TT#3029 - RMatelic - Style Summary Review Screen Line Up
                this.BeginInvoke(new MethodInvoker(SetRowSplitPosition4));
                this.BeginInvoke(new MethodInvoker(SetRowSplitPosition12));
                // End TT#3029

				// Begin TT#1019 - MD - stodd - prohibit allocation actions against GA - 
                if (_trans.ContainsGroupAllocationHeaders())
                {
                    EnhancedToolTip.SetToolTipWhenDisabled(cboAction, MIDText.GetTextOnly(eMIDTextCode.msg_ActionProtectedGroupAllocation));
                    EnhancedToolTip.SetToolTipWhenDisabled(btnProcess, MIDText.GetTextOnly(eMIDTextCode.msg_ProcessProtectedGroupAllocation));
                    cboAction.Enabled = false;
                    btnProcess.Enabled = false;
                }
				// End TT#1019 - MD - stodd - prohibit allocation actions against GA - 

                lblGA.Visible = _trans.IsGAMode;    // TT#1194-md -stodd - view ga header
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#region Get Allocation Selection Criteria 
		// Begin MID Track 4858 - JSmith - Security changes
//		private void BuildMenu()
//		{
//			//			try   // let parent catch errors so that we stop when one occurs
//			//			{
//			PopupMenuTool fileMenuTool;
//			PopupMenuTool editMenuTool;
//			PopupMenuTool toolsMenuTool;
//
//			ButtonTool btExport;
//			ButtonTool btFind;
//			ButtonTool btQuickFilter;
//
//			utmMain.ImageListSmall = MIDGraphics.ImageList;
//			utmMain.ImageListLarge = MIDGraphics.ImageList;
//
//			fileMenuTool = new PopupMenuTool("file_menu");
//			fileMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File);
//			fileMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.True;
//			utmMain.Tools.Add(fileMenuTool);
//
//			btExport = new ButtonTool("btExport");
//			btExport.SharedProps.Caption = "&Export to Excel";
//			btExport.SharedProps.Shortcut = Shortcut.CtrlE;
//			btExport.SharedProps.MergeOrder = 10;
//			utmMain.Tools.Add(btExport);
//
//			fileMenuTool.Tools.Add(btExport);
//
//			editMenuTool = new PopupMenuTool("edit_menu");
//			editMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit);
//			editMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.False;
//			utmMain.Tools.Add(editMenuTool);
//
//			btFind = new ButtonTool("btFind");
//			btFind.SharedProps.Caption = "&Find";
//			btFind.SharedProps.Shortcut = Shortcut.CtrlF;
//			btFind.SharedProps.MergeOrder = 20;
//			btFind.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.FindImage);
//			utmMain.Tools.Add(btFind);
//			editMenuTool.Tools.Add(btFind);
//
//			toolsMenuTool = new PopupMenuTool("tools_menu");
//			toolsMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools);
//			toolsMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.True;
//			utmMain.Tools.Add(toolsMenuTool);
//
//			btQuickFilter = new ButtonTool("btQuickFilter");
//			btQuickFilter.SharedProps.Caption = "&Quick Filter";
//			btQuickFilter.SharedProps.Shortcut = Shortcut.CtrlQ;
//			btQuickFilter.SharedProps.MergeOrder = 13;
//			utmMain.Tools.Add(btQuickFilter);
//			toolsMenuTool.Tools.Add(btQuickFilter);
//				 
//			//			}
//			//			catch
//			//			{
//			//				HandleException(exc);
//			//			}
//		}

        private void BuildMenu()
        {
            try
			{
        //    ButtonTool btExport;
        //    ButtonTool btFind;
        //    ButtonTool btQuickFilter;

        //    btExport = new ButtonTool("btExport");
        //    btExport.SharedProps.Caption = "&Export to Excel";
        //    btExport.SharedProps.Shortcut = Shortcut.CtrlE;
        //    btExport.SharedProps.MergeOrder = 10;
        //    utmMain.Tools.Add(btExport);

        //    FileMenuTool.Tools.Add(btExport);

        //    btFind = new ButtonTool("btFind");
        //    btFind.SharedProps.Caption = "&Find";
        //    btFind.SharedProps.Shortcut = Shortcut.CtrlF;
        //    btFind.SharedProps.MergeOrder = 20;
        //    btFind.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.FindImage);
        //    utmMain.Tools.Add(btFind);

        //    EditMenuTool.Tools.Add(btFind);

        //    btQuickFilter = new ButtonTool("btQuickFilter");
        //    btQuickFilter.SharedProps.Caption = "&Quick Filter";
        //    btQuickFilter.SharedProps.Shortcut = Shortcut.CtrlQ;
        //    btQuickFilter.SharedProps.MergeOrder = 13;
        //    utmMain.Tools.Add(btQuickFilter);

        //    ToolsMenuTool.Tools.Add(btQuickFilter);
                AddMenuItem(eMIDMenuItem.FileExport);
                AddMenuItem(eMIDMenuItem.EditFind);
                AddMenuItem(eMIDMenuItem.ToolsQuickFilter);
                AddMenuItem(eMIDMenuItem.ToolsTheme);       // MID Track #5006 - Add Theme to Tools menu 
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
		// End MID Track 4858
		private void SetText()
		{
            if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
				this.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_VelocityStoreDetail);
			else
				this.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Style) + " Review";
			_thisTitle = this.Text;
			if (_trans.AnalysisOnly)
				this.Text = this.Text + " - " + "Analysis only";
			else if (HeaderOrComponentGroups > 1)
				this.Text = this.Text + " *";
			else
			{
				AllocationWaferCoordinate wafercoord;
				TagForColumn colTag = new TagForColumn();
				colTag = (TagForColumn)g3.Cols[0].UserData;
				wafercoord = colTag.CubeWaferCoorList[_hdrRow];
				this.Text = this.Text + " - " + wafercoord.Label;
			}
			if (_trans.DataState == eDataState.ReadOnly)
				//	Format_Title(eDataState.ReadOnly,eMIDTextCode.frm_StyleReview,this.Text);
				this.Text = this.Text +   " Read Only"; 
			this.rbHeader.Text = MIDText.GetTextOnly((int)eAllocationStyleViewGroupBy.Header);
			this.rbComponent.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Component);
			this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
			this.btnApply.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Apply);
			this.rbAllStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AllStores);
			this.rbSet.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Set);
			this.btnVelocity.Text = MIDText.GetTextOnly(eMIDTextCode.frm_VelocityMethod);
			this.btnAllocate.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Allocate);
		}	
		private void GetSelectionCriteria()
		{
			//			try   // jae catch the error in the caller so that processing stops!
			//			{
			if (_trans.AllocationCriteriaExists)
			{
				_trans.UpdateAllocationViewSelectionHeaders();
				//	Jellis		_sab.ClientServerSession.Audit.Add_Msg(
				//					eMIDMessageLevel.Debug, 
				//					"Before getting Allocation wafers", "StyleView");

				if (_trans.AnalysisOnly)
				{
					g3.ContextMenu = null;
					this.gbxGroupBy.Enabled = false;	
				}
                
                _headerList = (AllocationHeaderProfileList)_trans.GetMasterProfileList(eProfileType.AllocationHeader); // TT#1185 - Verify ENQ before Update 

				// BEGIN MID Track #2551 - security not working
                // Begin Track #6404 - JSmith - 80302: Units to allocate is calculated when header is Work Up Total Buy
                //if (FunctionSecurity.AllowUpdate && _trans.HeadersEnqueued)
                //if (FunctionSecurity.AllowUpdate && _trans.HeadersEnqueued && !_trans.AnalysisOnly)  // TT#1185 - Verify ENQ before Update
                if (FunctionSecurity.AllowUpdate                                                       // TT#1185 - Verify ENQ before Update
                    && _headerList != null                                                             // TT#1185 - Verify ENQ before Update
                    && _trans.AreHeadersEnqueued(_headerList)                                          // TT#1185 - Verify ENQ before Update
                    && !_trans.AnalysisOnly)
                {
                    // Begin TT#1037 - MD - stodd - read only security - 
                    if (_trans.ContainsGroupAllocationHeaders())
                    {
                        if (_trans.DataState != eDataState.ReadOnly)
                        {
                            _trans.DataState = eDataState.Updatable;
                        }
                        // Implied "else" is that _trans.DataState = eDataState.ReadOnly
                    }
                    else
                    {
                        // End Track #6404
                        _trans.DataState = eDataState.Updatable;
                    }
                    // End TT#1037 - MD - stodd - read only security - 
                }
                else
                {
                    _trans.DataState = eDataState.ReadOnly;
                    // END MID Track #2551
                }
                // Begin TT#456 - RMatelic - Add Views to Size Review
                SetScreenParmsFromView();
                // End TT#456  

				_trans.BuildWaferColumns.Clear();

				switch (_trans.AllocationViewType)
				{
					case eAllocationSelectionViewType.Style:
						foreach(int variable in Enum.GetValues(typeof( eAllocationStyleViewVariableDefault)))
                        {   // Begin TT#318 - RMatelic - View column position not retaining  
                            //_trans.BuildWaferColumnsAdd(0,(eAllocationWaferVariable)variable);
                            //_trans.BuildWaferColumnsAdd(1,(eAllocationWaferVariable)variable);
                            //_trans.BuildWaferColumnsAdd(2,(eAllocationWaferVariable)variable);
                            CheckVariableBuiltArrayList(variable);
                            // End TT#318  
						}
                        AddViewColumns();       // TT#454 - RMatelic - Add Views in Style Review
						break;

					case eAllocationSelectionViewType.Velocity:
						foreach(int variable in Enum.GetValues(typeof( eAllocationVelocityViewVariableDefault)))
                        {   // Begin TT#318 - RMatelic - View column position not retaining  
                            //_trans.BuildWaferColumnsAdd(0,(eAllocationWaferVariable)variable);
                            //_trans.BuildWaferColumnsAdd(1,(eAllocationWaferVariable)variable);
                            //_trans.BuildWaferColumnsAdd(2,(eAllocationWaferVariable)variable);
                            CheckVariableBuiltArrayList(variable);
                            // End TT#318 
						}
                        AddViewColumns();
                        break;
				}
               
				_wafers = _trans.AllocationWafers;
				//	Jellis		_sab.ClientServerSession.Audit.Add_Msg(
				//					eMIDMessageLevel.Debug, 
				//					"After getting Allocation wafers", "StyleView");
				_trans.StyleView = this;
				_showNeedGrid = _trans.AllocationStyleViewIncludesNeedAnalysis;
				//_headerList =(AllocationHeaderProfileList)_trans.GetMasterProfileList(eProfileType.AllocationHeader); // TT#1185 - Verify ENQ before Update
				_curViewType = _trans.AllocationViewType;	
				if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
				{
					gbxAvg.Visible = true;
					btnVelocity.Visible = true;
					btnAllocate.Visible = true;
					rbAllStores.Checked = _trans.VelocityCalculateAverageUsingChain;
					rbSet.Checked = !_trans.VelocityCalculateAverageUsingChain;
					BuildRuleList();
				}

			}
		
			//			}
			//			catch (MIDException MIDexc)
			//			{
			//				switch (MIDexc.ErrorLevel)
			//				{
			//					case eErrorLevel.fatal:
			//						HandleException(MIDexc);
			//						break;
			//
			//					case eErrorLevel.information:
			//					case eErrorLevel.warning:	
			//					case eErrorLevel.severe:
			//						HandleMIDException(MIDexc);
			//						break;
			//					default:
			//						HandleException(MIDexc);
			//						break;
			//				}
			//			}
			//			catch (Exception ex)
			//			{
			//				HandleException(ex);
			//			}
		}

        // Begin TT#318 - RMatelic - View column position not retaining 
        private void SetScreenParmsFromView()
        {
            try
            {
                _buttonChanged = false;
                if (_viewRID > 0)
                {
                    DataRow row = _gridViewData.GridView_Read(_viewRID);
                    if (row != null)
                    {
                        // Set radio buttons which will set the _trans variables
                        if (row["GROUP_BY"] != DBNull.Value)
                        {
                            int groupBy = Convert.ToInt32(row["GROUP_BY"], CultureInfo.CurrentUICulture);
                            if (groupBy == Convert.ToInt32(eAllocationStyleViewGroupBy.Header, CultureInfo.CurrentUICulture))
                            {
                                _trans.AllocationGroupBy = Convert.ToInt32(eAllocationStyleViewGroupBy.Header, CultureInfo.CurrentUICulture);
                                if (!rbHeader.Checked)
                                {
                                    _buttonChanged = true;
                                }
                                rbHeader.Checked = true;
                                _hdrRow = 0;
                                _compRow = 1;
                            }
                            else if (groupBy == Convert.ToInt32(eAllocationStyleViewGroupBy.Components, CultureInfo.CurrentUICulture))
                            {
                                _trans.AllocationGroupBy = Convert.ToInt32(eAllocationStyleViewGroupBy.Components, CultureInfo.CurrentUICulture);
                                if (!rbComponent.Checked)
                                {
                                    _buttonChanged = true;
                                }
                                rbComponent.Checked = true;
                                _hdrRow = 1;
                                _compRow = 0;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void AddViewColumns()
        {
            _columnAdded = false;
            if (_viewRID == 0 || _viewRID == Include.NoRID)    // don't modify current grid appearance 
            {
                return;
            }

            DataTable dtGridViewDetail = _gridViewData.GridViewDetail_Read(_viewRID);

            if (dtGridViewDetail == null || dtGridViewDetail.Rows.Count == 0)
            {
                return;
            }

            string colKey;
            int colKeyInt;
            bool isDisplayed = false;
            foreach (DataRow row in dtGridViewDetail.Rows)
            {
                colKey = Convert.ToString(row["COLUMN_KEY"], CultureInfo.CurrentUICulture);
                if (colKey == _lblStore)
                {
                    continue;
                }    
                colKeyInt = Convert.ToInt32(colKey, CultureInfo.CurrentUICulture);
                isDisplayed = !Include.ConvertCharToBool(Convert.ToChar(row["IS_HIDDEN"], CultureInfo.CurrentUICulture));
                if (isDisplayed)
                {
                    CheckVariableBuiltArrayList(colKeyInt);
                }
            }
        }

        private void CheckVariableBuiltArrayList(int aVariable)
        {
            try
            {
                if (!_builtVariables.Contains(aVariable))
                {
                    _trans.BuildWaferColumnsAdd(0, (eAllocationWaferVariable)aVariable);
                    _trans.BuildWaferColumnsAdd(1, (eAllocationWaferVariable)aVariable);
                    _trans.BuildWaferColumnsAdd(2, (eAllocationWaferVariable)aVariable);
                    _builtVariables.Add(aVariable);
                    _columnAdded = true;
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#318  

		private void BuildRuleList()
		{
			//			try   // let parent catch any errors so that we stop building when an error occurs
			//			{
			//_ldRules = new ListDictionary();
			_ldRulesVelocity = new ListDictionary();
			DataTable ruleTable1 = MIDText.GetLabels((int) eRuleType.None, (int)eRuleType.None);
			foreach (DataRow row in ruleTable1.Rows)
			{
				//_ldRules.Add(Convert.ToUInt32(row["TEXT_CODE"],CultureInfo.CurrentUICulture),row["TEXT_VALUE"]);
				_ldRulesVelocity.Add(Convert.ToUInt32(row["TEXT_CODE"],CultureInfo.CurrentUICulture),row["TEXT_VALUE"]);
			}
            // Begin TT#3686 - RMatelic - Vecity Rule Type Qty does not accept decimals for WOS or Forward WOS Rules
            //DataTable ruleTable2 = MIDText.GetLabels((int) eRuleType.WeeksOfSupply, (int)eRuleType.SizeFillUpTo);
            DataTable ruleTable2 = MIDText.GetLabels((int)eRuleType.WeeksOfSupply, (int)eRuleType.ForwardWeeksOfSupply);
            // End TT#3686

			//foreach (DataRow row2 in ruleTable2.Rows)
			//{
			//	_ldRules.Add(Convert.ToUInt32(row2["TEXT_CODE"],CultureInfo.CurrentUICulture),row2["TEXT_VALUE"]);
			//}
				
				
			if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
			{
				foreach (DataRow dr in ruleTable2.Rows)
				{
					eVelocityRuleType vrt = (eVelocityRuleType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
					if (!Enum.IsDefined(typeof(eVelocityRuleType),vrt))
					{
						dr.Delete();
					}
				}
				ruleTable2.AcceptChanges();		
				
				
				foreach (DataRow row2 in ruleTable2.Rows)
				{
					_ldRulesVelocity.Add(Convert.ToUInt32(row2["TEXT_CODE"],CultureInfo.CurrentUICulture),row2["TEXT_VALUE"]);
				}
			}
            // BEGIN TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
            //// Begin Track 6074 stodd
            //DataTable ruleTable3 = MIDText.GetLabels((int)eRuleType.MinimumBasis, (int)eRuleType.AdMinimumBasis);
		
            //if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
            //{
            //    foreach (DataRow dr in ruleTable3.Rows)
            //    {
            //        eVelocityRuleType vrt = (eVelocityRuleType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
            //        if (!Enum.IsDefined(typeof(eVelocityRuleType), vrt))
            //        {
            //            dr.Delete();
            //        }
            //    }
            //    ruleTable3.AcceptChanges();


            //    foreach (DataRow row2 in ruleTable3.Rows)
            //    {
            //        _ldRulesVelocity.Add(Convert.ToUInt32(row2["TEXT_CODE"], CultureInfo.CurrentUICulture), row2["TEXT_VALUE"]);
            //    }
            //}
            //// End track 6074
            // END TT#505 - AGallagher - Velocity - Apply Min/Max (#58)
			//			}
			//			catch (Exception ex)
			//			{
			//				HandleException(ex);
			//			}
		}
		#endregion
		#region Format Grids (From Form_Load)

		private void SetGridRedraws(bool aValue)
		{
            // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
            try
            {
                _currentRedrawState = aValue;

                g1.Redraw = aValue;
                g2.Redraw = aValue;
                g3.Redraw = aValue;
                g4.Redraw = aValue;
                g5.Redraw = aValue;
                g6.Redraw = aValue;
                g7.Redraw = aValue;
                g8.Redraw = aValue;
                g9.Redraw = aValue;
                g10.Redraw = aValue;
                g11.Redraw = aValue;
                g12.Redraw = aValue;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            // End Track #6371
		}

		private void FormatGrids1to12()
		{
			Formatg1Grid();
			Formatg2Grid();
			Formatg3Grid();
			Formatg456Grids();
            Add456DataRows(_wafers, _g456GridTable, out _g456DataView, g4, g5, g6); 
			FormatGrid7_10 (FromGrid.g7);
			FormatGrid8_12 (FromGrid.g8);
			FormatGrid8_12 (FromGrid.g9);
			FormatGrid7_10 (FromGrid.g10);
			FormatGrid8_12 (FromGrid.g11);
			FormatGrid8_12 (FromGrid.g12);
		}

		private void Formatg1Grid()
		{
			int i, j, k;
			string [,] g1GridValues;
			AllocationWaferCoordinateList wafercoordlist;
			AllocationWaferCoordinate wafercoord;	
		
			if (g1.Tag == null)
			{
				g1.Tag = new GridTag(Convert.ToInt32(FromGrid.g1, CultureInfo.CurrentUICulture), null, null);
			}

			_wafer = _wafers[0,0];
			_cells = _wafer.Cells;
		  
			string[,] ColLabels = _wafers[0,0].ColumnLabels;
			
			g1GridValues = new string[1,(_wafer.Columns.Count + 2)];
			for (i = 0; i < 1; i++)
			{
				g1GridValues[i,0] = _lblStore;

				for (k=1; k<=_wafer.Columns.Count; k++)
				{
					wafercoordlist = (AllocationWaferCoordinateList)_wafer.Columns[k-1];
					wafercoord = (AllocationWaferCoordinate)wafercoordlist[i+2];
					g1GridValues[i,k] = ColLabels[i + 2,(k - 1)];
					ParseColumnHeading(ref g1GridValues[i,k],wafercoord.CoordinateType,wafercoord.CoordinateSubType);
				}
				g1GridValues[i,k] = (" "); // add extra column for grid splitter resizing
			}

			g1.Rows.Count = 1;
			g1.Cols.Count = _wafer.Columns.Count + 2;
			g1.Cols.Fixed = 0;
			g1.Rows.Fixed = 1;		
			g1.AllowDragging = AllowDraggingEnum.None;
			g1.AllowMerging = AllowMergingEnum.RestrictCols;
			
			for (i = 0; i < _wafer.Columns.Count + 2; i++)
			{	
				//Initialize and assign tags
				TagForColumn colTag = new TagForColumn();
				colTag.cellColumn = i; //Used in Find for Store
				if (i > 0 && i < _wafer.Columns.Count + 1 )	// skip header column and extra column for dragging
				{
					colTag.CubeWaferCoorList = (AllocationWaferCoordinateList)_wafer.Columns[i-1];
				}
				g1.Cols[i].UserData = colTag;
                
                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
                if (colTag.CubeWaferCoorList != null)
                {
                    wafercoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                    if (wafercoord != null)
                    {
                        g1.Cols[i].Name = wafercoord.Key.ToString();
                    }        
                }
                else if (i == 0)
                {
                    g1.Cols[i].Name = _lblStore;
                }
                // End TT#231 
			}
			for (i = 0; i < g1.Rows.Count; i++)
			{
				g1.Rows[i].AllowMerging = true;
				for (j = 0; j < _wafer.Columns.Count + 2; j++)
				{
					g1.SetData(i, j, g1GridValues[i,j]); //manually set data to the cells
				}
			}
			g1.Rows[0].TextAlign = TextAlignEnum.CenterBottom;
			g1.AutoSizeCols(0, 0, g1.Rows.Count - 1, g1.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.None);
		}

		private void Formatg2Grid()
		{	
			int i, j, k; 
			string [,] g2GridValues;
			AllocationWaferCoordinateList wafercoordlist;
			AllocationWaferCoordinate wafercoord;	

			if (g2.Tag == null)
			{
				g2.Tag = new GridTag(Convert.ToInt32(FromGrid.g2, CultureInfo.CurrentUICulture), null, null);
			}
			
			_wafer = _wafers[0,1];
			_cells = _wafer.Cells;
		   
			string[,] ColLabels = _wafers[0,1].ColumnLabels;
 			
			g2GridValues = new string[2,_wafer.Columns.Count];

			for (i = 0; i < 2; i++)
			{
				for (k=0; k < _wafer.Columns.Count; k++)
				{
					wafercoordlist = (AllocationWaferCoordinateList)_wafer.Columns[k];
					wafercoord = (AllocationWaferCoordinate)wafercoordlist[i];
					g2GridValues[i,k] = ColLabels[i,k];
					ParseColumnHeading(ref g2GridValues[i,k],wafercoord.CoordinateType,wafercoord.CoordinateSubType);
				}
			}
			g2.Rows.Count = 2;
			g2.Cols.Count = _wafer.Columns.Count;	
			g2.Cols.Fixed = 0;
			g2.Rows.Fixed = _wafer.Rows.Count;
			g2.AllowDragging = AllowDraggingEnum.None;
			g2.AllowMerging = AllowMergingEnum.RestrictCols;
			
			for (i = 0; i < _wafer.Columns.Count; i++)
			{
				//Initialize and assign tags
				TagForColumn colTag = new TagForColumn();
				colTag.CubeWaferCoorList = (AllocationWaferCoordinateList)_wafer.Columns[i];
				colTag.cellColumn = i;
				g2.Cols[i].UserData = colTag;

                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
                if (colTag.CubeWaferCoorList != null)
                {
                    AllocationWaferCoordinate compCoord = colTag.CubeWaferCoorList[_compRow];
                    AllocationWaferCoordinate variableCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                    if (compCoord != null && variableCoord != null)
                    {
                        if (compCoord.CoordinateSubType == (int)eComponentType.Total)
                        {
                            g2.Cols[i].Name = variableCoord.Key.ToString();
                        }
                    }
                }
                // End TT#231 
			}
			for (i = 0; i < g2.Rows.Count; i++)
			{
				g2.Rows[i].AllowMerging = true;
				for (j = 0; j < g2.Cols.Count; j++)
				{
					g2.SetData(i, j, g2GridValues[i,j]);
				}
			}
			g2.Rows[0].TextAlign = TextAlignEnum.CenterBottom;
			g2.Rows[1].TextAlign = TextAlignEnum.CenterBottom;
			if (!_loading)
				g2.AutoSizeCols(0, 0, g2.Rows.Count - 1, g2.Cols.Count - 1, 0, AutoSizeFlags.None);
		 
			g2.AutoSizeRow(0);
			g2.AutoSizeRow(1);
		}

		private void Formatg3Grid()
		{
			int i, j, k; 
			string [,] g3GridValues;
			AllocationWaferCoordinateList wafercoordlist;
			AllocationWaferCoordinate wafercoord;	
				
			if (g3.Tag == null)
			{
				g3.Tag = new GridTag(Convert.ToInt32(FromGrid.g3, CultureInfo.CurrentUICulture), null, null);
			}
			_wafer = _wafers[0,2];
			_cells = _wafer.Cells;
			string[,] ColLabels = _wafers[0,2].ColumnLabels;
		

			g3GridValues = new string[2,_wafer.Columns.Count];
			 
			for (i = 0; i < 2; i++)
			{
				for (k=0; k < _wafer.Columns.Count; k++)
				{
					wafercoordlist = (AllocationWaferCoordinateList)_wafer.Columns[k];
					wafercoord = (AllocationWaferCoordinate)wafercoordlist[i];
					g3GridValues[i,k] = ColLabels[i,k];
                    // Begin TT#2911 - JSmith - Style Review-> columns are not auto sizing when changing from Header to Component
                    //if (_trans.AllocationGroupBy == Convert.ToInt32(eAllocationStyleViewGroupBy.Header, CultureInfo.CurrentUICulture))
                    //{
                    //    ParseColumnHeading(ref g3GridValues[i, k], wafercoord.CoordinateType, wafercoord.CoordinateSubType);
                    //}
                    ParseColumnHeading(ref g3GridValues[i, k], wafercoord.CoordinateType, wafercoord.CoordinateSubType);
                    // End TT#2911 - JSmith - Style Review-> columns are not auto sizing when changing from Header to Component
				}
			}
			g3.Rows.Count = 2;	
			g3.Cols.Count = _wafer.Columns.Count;
			g3.Cols.Fixed = 0;
			g3.Rows.Fixed = _wafer.Rows.Count;
			g3.AllowDragging = AllowDraggingEnum.None;
			g3.AllowMerging = AllowMergingEnum.RestrictCols;
            //Hashtable ht = new Hashtable();

			for (i = 0; i < g3.Cols.Count; i++)
			{
				TagForColumn colTag = new TagForColumn();
				colTag.CubeWaferCoorList = (AllocationWaferCoordinateList)_wafer.Columns[i];
				colTag.cellColumn = i;
				g3.Cols[i].UserData = colTag;
                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                g3.Cols[i].Name = string.Empty;
                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order

                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail

                if (colTag.CubeWaferCoorList != null)
                {
                    AllocationWaferCoordinate compCoord = colTag.CubeWaferCoorList[_compRow];
                    AllocationWaferCoordinate variableCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                    if (compCoord != null && variableCoord != null)
                    {
                        if (compCoord.CoordinateSubType == (int)eComponentType.Total || compCoord.CoordinateSubType == 0)
                        {
                            g3.Cols[i].Name = variableCoord.Key.ToString();
                            //ht.Add(variableCoord.Key, variableCoord.Label);
                        }
                    }
                }
                // End TT#231 
			}
			
			for (i = 0; i < g3.Rows.Count; i++)
			{
                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                //g3.Rows[i].AllowMerging = true;
                if (i < g3.Rows.Count - 1)
                {
                    g3.Rows[i].AllowMerging = true;
                }
                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
				for (j = 0; j < g3.Cols.Count; j++)
				{
					g3.SetData(i, j, g3GridValues[i,j]);
				}
			}
			g3.Rows[0].TextAlign = TextAlignEnum.CenterBottom;
			g3.Rows[1].TextAlign = TextAlignEnum.CenterBottom;
			if (!_loading)
				g3.AutoSizeCols(0, 0, g3.Rows.Count - 1, g3.Cols.Count - 1, 0, AutoSizeFlags.None);
            g3.AutoSizeRow(0);
            g3.AutoSizeRow(1);
		}
	
		private void Formatg456Grids()  
		{
			int i; 
			AllocationWafer wafer;
			DataColumn dataCol;

            _g456GridTable = MIDEnvironment.CreateDataTable();
			
			//1st column (Store) is a label and not in the wafer
			dataCol = new DataColumn();
			dataCol.ColumnName = "Store Need Store";
			_g456GridTable.Columns.Add(dataCol);
			
			//Add Store Need grid 4 columns
			wafer = _wafers[0,0];
			for (i = 0; i < wafer.Columns.Count; i++)
			{
				AddDataColumn(FromGrid.g4, i);
			}
			//Add extra column at end of Store Need grid for grid 
			//splitter expanding puposes
			dataCol = new DataColumn();
			dataCol.ColumnName = "Store Need Empty";
			_g456GridTable.Columns.Add(dataCol);

			//Add Total grid 5 columns
			wafer = _wafers[0,1];
			for (i = 0; i < wafer.Columns.Count; i++)
			{
				AddDataColumn(FromGrid.g5, i);
			}

			//Add Header grid 6 columns	
			wafer = _wafers[0,2];
			for (i = 0; i < wafer.Columns.Count; i++)
			{
				AddDataColumn(FromGrid.g6, i);
			}

            // Begin TT#1387-MD - stodd - GA- headers generic ppks when open style review and process as is set to headers receive error messages.
            // there is a problem with case insensitivity in the DataView. Two packs with a similar name--only the case is different--and the same multiple, generate the same column name.
            // when the view is used as the data source for the grid, the similar columns resolve to the same column on the grid. This causes fewer columns than
            // expected and you get an index out of range error. 
            // The code below adds a space to the end of any matching columns to make it unique.
            for (int c = 0; c < _g456GridTable.Columns.Count; c++)
            {
                for (int d = 0; d < _g456GridTable.Columns.Count; d++)
                {
                    if (c == d) 
                        continue;   // Don't want to compare the same column

                    if (_g456GridTable.Columns[c].ColumnName.ToUpper() == _g456GridTable.Columns[d].ColumnName.ToUpper())
                    {
                        _g456GridTable.Columns[d].ColumnName = _g456GridTable.Columns[d].ColumnName + " ";
                    }
                }
            }
            // End TT#1387-MD - stodd - GA- headers generic ppks when open style review and process as is set to headers receive error messages.
		}
		private void AddDataColumn(FromGrid aGrid, int aCol)
		{
			AllocationWafer wafer = null;
			AllocationWaferCoordinateList wafercoordlist;
			AllocationWaferCoordinate wafercoord;
			AllocationWaferVariable varProf;
			string[,] ColLabels;
			DataColumn dataCol;
			
			dataCol = new DataColumn();

			switch(aGrid)
			{
				case FromGrid.g4:
					wafer = _wafers[0,0];
					break;
				case FromGrid.g5:
					wafer = _wafers[0,1];
					break;
				case FromGrid.g6:
					wafer = _wafers[0,2];
					break;
			}

			ColLabels = wafer.ColumnLabels;
			if (aGrid == FromGrid.g4)
				dataCol.ColumnName = "Store Need " + ColLabels[2,aCol];
			else
				dataCol.ColumnName = ColLabels[_hdrRow,aCol].Trim() + " " +  ColLabels[_compRow,aCol];

			wafercoordlist = (AllocationWaferCoordinateList)wafer.Columns[aCol];
			wafercoord = (AllocationWaferCoordinate)wafercoordlist[_varProfIndex];
			varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord.Key );
			switch (varProf.Format)
			{
				case eAllocationWaferVariableFormat.String:
				case eAllocationWaferVariableFormat.None:
					dataCol.DataType = System.Type.GetType("System.String");
					break;

				case eAllocationWaferVariableFormat.Number:
					//numDec = varProf.NumDecimals; 
					if (varProf.NumDecimals > 0)
					{
						dataCol.DataType = System.Type.GetType("System.Decimal");
						dataCol.ExtendedProperties.Add("NumDecimals", varProf.NumDecimals);
					}
					else
					{
						dataCol.DataType = System.Type.GetType("System.Int32");
						dataCol.ExtendedProperties.Add("IsRuleType",false);
                        // Begin TT#2225 - RMatelic - VSW Modifcations Enhancement
                        if (varProf.Key == Convert.ToInt32(eAllocationWaferVariable.StoreIMOMaxQuantityAllocated, CultureInfo.CurrentUICulture)) 
                        {
                            dataCol.ExtendedProperties.Add("SuppressMaxInt", "SuppressMaxInt");
                        }
                    }   // End TT#2225
					break;
				
				case eAllocationWaferVariableFormat.eRuleType:
					dataCol.DataType = System.Type.GetType("System.UInt32");
					break;

				default:
					dataCol.DataType = System.Type.GetType("System.String");
					break;
			}


            _g456GridTable.Columns.Add(dataCol);
		}

        private void Add456DataRows(AllocationWaferGroup aWafers, DataTable aG456GridTable, out DataView aG456DataView,
               C1FlexGrid aGrid4, C1FlexGrid aGrid5, C1FlexGrid aGrid6)
        {
            int i, j, k, n, nDec;
            int Grid4LastCol, Grid5LastCol, Grid6LastCol;
            decimal decVal;
            AllocationWafer wafer4;
            AllocationWafer wafer5;
            AllocationWafer wafer6;
            AllocationWaferCell[,] cells4;
            AllocationWaferCell[,] cells5;
            AllocationWaferCell[,] cells6;

            DataRow dRow;
            string[,] RowLabels = aWafers[0, 0].RowLabels;
            wafer4 = aWafers[0, 0];
            wafer5 = aWafers[0, 1];
            wafer6 = aWafers[0, 2];

            cells4 = wafer4.Cells;
            cells5 = wafer5.Cells;
            cells6 = wafer6.Cells;
            CellRows = new Hashtable();

            Grid4LastCol = wafer4.Columns.Count + 1;
            Grid5LastCol = Grid4LastCol + wafer5.Columns.Count;
            Grid6LastCol = Grid5LastCol + wafer6.Columns.Count;

            //			g4.Redraw = false;
            //			g5.Redraw = false;
            //			g6.Redraw = false;

            // begin MID Track 6079 Zero Qty Allocated not accepted after sort columns
            string g456DataViewSort = string.Empty;
            int[] g456DataViewSeq = new int[wafer4.Rows.Count];
            g456DataViewSeq.Initialize();
            if (_g456DataView != null)
            {
                if (_g456DataView.Sort != string.Empty)
                {
                    g456DataViewSort = _g456DataView.Sort;
                    if (!_sortCriteriaChanged
                        && _g456DataView.Table.Rows.Count > 0
                        && _g456DataView.Table.Rows.Count == wafer4.Rows.Count)
                    {
                        for (i = 0; i < wafer4.Rows.Count; i++)
                        {
                            g456DataViewSeq[i] = (int)_g456DataView.Table.Rows[i][1];
                        }
                    }
                }
            }
            // end MID Track 6079 Zero Qty Allocated not accepted after sort columns
            
            aG456GridTable.Clear();

            for (i = 0; i < wafer4.Rows.Count; i++)
            {
                dRow = aG456GridTable.NewRow();
                j = 0;
                k = 0;
                n = 0;
                CellRows.Add(RowLabels[i, 0], i);
                foreach (DataColumn dCol in aG456GridTable.Columns)
                {
                    if (j <= Grid4LastCol)
                    {
                        if (j == 0)
                            dRow[dCol] = RowLabels[i, 0];
                        else if (j == 1)    // MID Track 6079 Zero qty allocated not accepted after sort
                            dRow[dCol] = g456DataViewSeq[i]; // MID Track 6079 Zero qty allocated not accepted after sort
                        else if (j == Grid4LastCol)
                            dRow[dCol] = _invalidCellValue;
                        else if (cells4[i, j - 1].CellIsValid)
                        {
                            if (dCol.DataType == typeof(System.String))
                                dRow[dCol] = cells4[i, j - 1].ValueAsString;
                            else if (dCol.DataType == typeof(System.Decimal))
                            {   // the * 1.00m adds the 2 decimal places (.00) to whole numbers without decimal digits
                                decVal = (Convert.ToDecimal(cells4[i, j - 1].Value, CultureInfo.CurrentUICulture) * 1.00m);
                                nDec = Convert.ToInt32(dCol.ExtendedProperties["NumDecimals"], CultureInfo.CurrentUICulture);
                                dRow[dCol] = Decimal.Round(decVal, nDec);
                            }
                            else
                                dRow[dCol] = cells4[i, j - 1].Value;
                        }
                        else
                            dRow[dCol] = _invalidCellValue;
                        j++;
                    }
                    else if (j <= Grid5LastCol)
                    {
                        if (cells5[i, k].CellIsValid)
                        {
                            if (dCol.DataType == typeof(System.String))
                                dRow[dCol] = cells5[i, k].ValueAsString;
                            else
                            // Begin TT#2225 - RMatelic - VSW Modifcations Enhancement
                            {
                                //dRow[dCol] = cells5[i, k].Value;
                                if (dCol.ExtendedProperties.ContainsKey("SuppressMaxInt") && Convert.ToInt32(cells5[i, k].Value, CultureInfo.CurrentUICulture) == int.MaxValue)
                                {
                                    dRow[dCol] = DBNull.Value;
                                }
                                else
                                {
                                    dRow[dCol] = cells5[i, k].Value;
                                }
                            }
                            // End TT#2225
                        }
                        else
                            dRow[dCol] = _invalidCellValue;
                        k++;
                        j++;
                    }
                    else
                    {
                        if (cells6[i, n].CellIsValid)
                        {

                            if (dCol.DataType == typeof(System.String))
                                dRow[dCol] = cells6[i, n].ValueAsString;
                            else if (dCol.DataType == typeof(System.UInt32))
                                dRow[dCol] = Convert.ToUInt32(cells6[i, n].Value, CultureInfo.CurrentUICulture);
                            else
                            // Begin TT#2225 - RMatelic - VSW Modifcations Enhancement
                            {
                                //dRow[dCol] = cells6[i, n].Value;
                                if (dCol.ExtendedProperties.ContainsKey("SuppressMaxInt") && Convert.ToInt32(cells6[i, n].Value, CultureInfo.CurrentUICulture) == int.MaxValue)
                                {
                                    dRow[dCol] = DBNull.Value;
                                }
                                else
                                {
                                    dRow[dCol] = cells6[i, n].Value;
                                }
                            }
                            // End TT#2225
                        }
                        else
                            dRow[dCol] = _invalidCellValue;
                        n++;
                        j++;
                    }
                }
                aG456GridTable.Rows.Add(dRow);
            }
            //Use DataView and bind grids in order to use DataView.Sort. 
            aG456DataView = new DataView(aG456GridTable);
            aGrid4.DataSource = aG456DataView;
            aGrid5.DataSource = aG456DataView;
            aGrid6.DataSource = aG456DataView;

            //All 3 middle grids are bound to the same dataview so all grids  
            //initially have all columns. Remove the columns that do not
            //apply to each grid. 

            aGrid4.Cols.RemoveRange(Grid4LastCol + 1, (wafer5.Columns.Count + wafer6.Columns.Count));

            aGrid5.Cols.RemoveRange(Grid5LastCol + 1, wafer6.Columns.Count);
            aGrid5.Cols.RemoveRange(0, wafer4.Columns.Count + 2);

            aGrid6.Cols.RemoveRange(0, Grid5LastCol + 1);

            aGrid4.Cols.Fixed = 0;
            aGrid4.Rows.Fixed = 0;
            if (aGrid4.Tag == null)
            {
                aGrid4.Tag = new GridTag(Convert.ToInt32(FromGrid.g4, CultureInfo.CurrentUICulture), null, null);
            }
            for (i = 0; i < aGrid4.Rows.Count; i++)
            {
                TagForRow rowTag = new TagForRow();
                aGrid4.Rows[i].UserData = rowTag;
            }
            if (!_exporting)
            {
                aGrid4.AutoSizeCols(0, 0, aGrid4.Rows.Count - 1, aGrid4.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);
            }

            aGrid5.Cols.Fixed = 0;
            aGrid5.Rows.Fixed = 0;
            if (aGrid5.Tag == null)
            {
                aGrid5.Tag = new GridTag(Convert.ToInt32(FromGrid.g5, CultureInfo.CurrentUICulture), null, null);
            }

            for (i = 0; i < aGrid5.Cols.Count; i++)
            {
                aGrid5.Cols[i].TextAlign = TextAlignEnum.GeneralCenter;
            }
            if (!_exporting)
            {
                aGrid5.AutoSizeCols(0, 0, aGrid5.Rows.Count - 1, aGrid5.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);
            }
          
            aGrid6.Cols.Fixed = 0;
            aGrid6.Rows.Fixed = 0;
            if (aGrid6.Tag == null)
            {
                aGrid6.Tag = new GridTag(Convert.ToInt32(FromGrid.g6, CultureInfo.CurrentUICulture), null, null);
            }

            for (i = 0; i < aGrid6.Cols.Count; i++)
            {
                if (aGrid6.Cols[i].DataType == typeof(System.UInt32))
                {
                    aGrid6.Cols[i].DataMap = _ldRulesVelocity;
                }
                aGrid6.Cols[i].TextAlign = TextAlignEnum.GeneralCenter;
            }
            if (!_exporting) 
            {
                // Begin TT#1288-MD - RMatelic - Store Detail column widths are wider than Version 5.3
                //aGrid6.AutoSizeCols(0, 0, aGrid6.Rows.Count - 1, aGrid6.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);
                if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
                {
                    aGrid6.AutoSizeCols();
                }
                else
                {
                    aGrid6.AutoSizeCols(0, 0, aGrid6.Rows.Count - 1, aGrid6.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);
                }
                // End TT#1288-MD
            }
           

            //			g4.Redraw = true;
            //			g5.Redraw = true;
            //			g6.Redraw = true;
            // begin MID Track 6079
            _g456DataView.Sort = g456DataViewSort;
            SetSortSeqColumn();
            // end MID Track 6079
        }
		private void FormatGrid7_10 (FromGrid aGrid)
		{
			int i, j; 
			decimal decVal;
			C1FlexGrid grid = null;	
			AllocationWafer wafer = null;
			AllocationWaferCoordinateList wafercoordlist;
			AllocationWaferCoordinate wafercoord;
			AllocationWaferVariable varProf;
	
			switch(aGrid)
			{
				case FromGrid.g7:
					wafer = _wafers[1,0];
					grid = g7;
					break;
				case FromGrid.g10:
					wafer = _wafers[2,0];
					grid = g10;
					break;
			}
			string[,] RowLabels = wafer.RowLabels;
			_cells = wafer.Cells;

			grid.Rows.Count = wafer.Rows.Count;
			// 2 extra columns; 1 for the row label & 1 for the empty last column 
			grid.Cols.Count = wafer.Columns.Count + 2;
			grid.Cols.Fixed = 0;
			grid.Rows.Fixed = 0;
			if (grid.Tag == null)
			{
				grid.Tag = new GridTag(Convert.ToInt32(aGrid, CultureInfo.CurrentUICulture), null, null);
			}
			 
			for (j = 1; j <= wafer.Columns.Count; j++)
			{
				wafercoordlist = (AllocationWaferCoordinateList)wafer.Columns[j - 1];
				wafercoord = (AllocationWaferCoordinate)wafercoordlist[_varProfIndex];
				varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord.Key );
					 
				if (varProf.Format == eAllocationWaferVariableFormat.Number
					&& varProf.NumDecimals > 0)
					grid.Cols[j].DataType = System.Type.GetType("System.Decimal");
			}
			
			for (i = 0; i < wafer.Rows.Count; i++)
			{
				TagForRow rowTag = new TagForRow();
				grid.Rows[i].UserData = rowTag;
				grid.SetData(i, 0, RowLabels[i,0]);
				for (j = 1; j <= wafer.Columns.Count; j++)
				{
					if (_cells[i,j - 1].CellIsValid)
					{
						wafercoordlist = (AllocationWaferCoordinateList)wafer.Columns[j - 1];
						wafercoord = (AllocationWaferCoordinate)wafercoordlist[_varProfIndex];
						varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord.Key );
					 
						if (varProf.Format == eAllocationWaferVariableFormat.String)
							grid.SetData(i, j,_cells[i,j - 1].ValueAsString);
						else if (grid.Cols[j].DataType == typeof(System.Decimal))
						{   // the * 1.00m adds the 2 decimal places (.00) to whole numbers without decimal digits
							decVal = (Convert.ToDecimal(_cells[i,j - 1].Value, CultureInfo.CurrentUICulture) * 1.00m);
							grid.SetData(i, j, Decimal.Round(decVal,varProf.NumDecimals));
						}	
						else	
							grid.SetData(i, j,_cells[i,j - 1].Value);
					}
					else
						grid.SetData(i, j, _invalidCellValue);
				}
				grid.SetData(i, j, _invalidCellValue);
			}
			grid.AutoSizeCols(0, 0, grid.Rows.Count - 1, grid.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);
		}
		private void FormatGrid8_12 (FromGrid aGrid)
		{
			int i, j; 
			C1FlexGrid grid = null;	
			AllocationWafer wafer = null;
			AllocationWaferCoordinateList wafercoordlist;
			AllocationWaferCoordinate wafercoord;
			AllocationWaferVariable varProf;
		
			switch(aGrid)
			{
				case FromGrid.g8:
					wafer = _wafers[1,1];
					grid = g8;
					break;
				case FromGrid.g9:
					wafer = _wafers[1,2];
					grid = g9;
					break;
				case FromGrid.g11:
					wafer = _wafers[2,1];
					grid = g11;
					break;
				case FromGrid.g12:
					wafer = _wafers[2,2];
					grid = g12;
					break;
			}
			_cells = wafer.Cells;

			grid.Rows.Count = wafer.Rows.Count;
			grid.Cols.Count = wafer.Columns.Count;
			grid.Cols.Fixed = 0;
			grid.Rows.Fixed = 0;
			if (grid.Tag == null)
			{
				grid.Tag = new GridTag(Convert.ToInt32(aGrid, CultureInfo.CurrentUICulture), null, null);
			}
			for (i = 0; i < wafer.Rows.Count; i++)
			{
				for (j = 0; j < wafer.Columns.Count; j++)
				{
					if (_cells[i,j].CellIsValid)
					{
						wafercoordlist = (AllocationWaferCoordinateList)wafer.Columns[j];
						wafercoord = (AllocationWaferCoordinate)wafercoordlist[_varProfIndex];
						varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord.Key );
						if (_cells[i,j].ValueAsString != String.Empty) 
							grid.SetData(i, j,_cells[i,j].ValueAsString);
						else
							grid.SetData(i, j,_cells[i,j].Value);
					}
					else
						grid.SetData(i, j, _invalidCellValue);
				}
			}
			grid.AutoSizeCols(0, 0, grid.Rows.Count - 1, grid.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);
		}
		private void ParseColumnHeading(ref string ColHeading, eAllocationCoordinateType coordType, int coordSubType )
		{
			int len, i;
			char [] cArray;
			string newstring = String.Empty;
			
			if (  (coordType == eAllocationCoordinateType.Component  
				&& (eComponentType)coordSubType == eComponentType.SpecificColor)
				//|| (coordType == eAllocationCoordinateType.PackName  
				//&& (eComponentType)coordSubType == eComponentType.SpecificPack) 
				) 	
			{
				len = ColHeading.Length;
				cArray = ColHeading.ToCharArray(0, len) ;
				for (i = 0; i < len; i++) 
				{
					if (i < 4)
						if (i < len - 1)
							newstring = newstring + cArray[i] + "\r\n";
						else
							newstring = newstring + cArray[i];
					else
						newstring = newstring + cArray[i];
				}
				ColHeading = newstring;
			}
			else
			{
                // Begin TT#3029 - RMatelic - Style Summary Review Screen Line Up
                //newstring = ColHeading.Replace(" ", "\r\n");
                //ColHeading = newstring;
                int maxSpaceCount = 0;
                char oneSpace = ' ';
                len = ColHeading.Length;
				cArray = ColHeading.ToCharArray(0, len) ;
                for (i = 0; i < len; i++)
                {
                    int spaceCount = 0;
                    while (cArray[i] == oneSpace)
                    {
                        spaceCount++;
                        i++;
						// Begin TT#3829 - stodd - Error when processing need and opening style review - 
                        if (i >= cArray.Length)
                        {
                            break;
                        }
						// End TT#3829 - stodd - Error when processing need and opening style review - 
                    }
                    if (spaceCount > maxSpaceCount)
                    {
                        maxSpaceCount = spaceCount;
                    }
                }
                string searchString = string.Empty;
                for (int k = 0; k < maxSpaceCount; k++)
                {
                    searchString += " ";
                }
                newstring = ColHeading;
                for (i = maxSpaceCount; i >= 1; i--)
                {
                    newstring = newstring.Replace(searchString, "\r\n");
                    searchString = searchString.Substring(0, searchString.Length - 1);
                }
                // Begin TT#5466 - JSmith - Header ID in SKU Review is not wrapping
                newstring = newstring.Replace("_", "_" + System.Environment.NewLine);
                // End TT#5466 - JSmith - Header ID in SKU Review is not wrapping
				ColHeading = newstring;
                // End TT#3029
			}
		}
		#endregion

		#region Methods to satisfy IFormBase

		public override void ICut()
		{
		}

		public override void ICopy()
		{
		}

		public override void IPaste()
		{
		}

		public override void IDelete()
		{
		}
		
		public override void IRefresh()
		{
			try
			{
				_trans.RefreshSelectedHeaders();
                // begin MID Track 6079 qty changed not accepted after sort column
                //CriteriaChanged();
                SortCriteriaChanged();
                // end MID Track 6079 qty changed not accepted after sort column();
				UpdateOtherViews();
			}		
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

        public override void IExport()
        {
            try
            {
                Export();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public override void IFind()
        {
            try
            {
                Find();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public override void IQuickFilter()
        {
            try
            {
                QuickFilter();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

		public override void ISave()
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
					ShowSaveDialog();
				else
					SaveChanges();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

        // BEGIN MID Track #5006 - Add Theme to Tools menu
        public override void ITheme()
        {
            try
            {
                Theme();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // END MID Track #5006 

		private void ShowSaveDialog()
		{
			string title;
			WindowSaveItem wsi;
			MIDRetail.Windows.frmVelocityMethod frmVelocity;
			try
			{
				ArrayList WindowNames = new ArrayList();
				wsi = new WindowSaveItem();
				wsi.Name = _trans.VelocityWindow.Text;
				wsi.SaveData = false;
				WindowNames.Add(wsi);

				wsi.Name = this.Text;
				wsi.SaveData = false;
				WindowNames.Add(wsi);
				wsi = new WindowSaveItem();
				
				title = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save) + " " + MIDText.GetTextOnly((int)eMethodType.Velocity);
                SaveDialog frm = new SaveDialog(WindowNames, title);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    foreach (WindowSaveItem wsi2 in frm.SaveList)
                    {
                        if (wsi2.SaveData)
                        {
                            if (wsi2.Name == this.Text)
                                SaveChanges();
                            else
                            {
                                frmVelocity = (MIDRetail.Windows.frmVelocityMethod)_trans.VelocityWindow;
                                frmVelocity.SaveFromExternalSource();
                            }
                        }
                    }
                }

                frm.Dispose();	
 
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}	
		}

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private void ShowSaveAsDialog()
        {
            MIDRetail.Windows.frmVelocityMethod frmVelocity;
            try
            {
                frmSaveAs formSaveAs = new frmSaveAs(_sab);

                frmVelocity = (MIDRetail.Windows.frmVelocityMethod)_trans.VelocityWindow;
                frmVelocity.ABM = frmVelocity.ABM.Copy(_sab.ClientServerSession, true, true);
                frmVelocity.ABM.Key = Include.NoRID;
                frmVelocity.ABM.Method_Change_Type = eChangeType.add;

                formSaveAs.SaveAsName = frmVelocity.ABM.Name;
                if (frmVelocity.ABM.User_RID == Include.GlobalUserRID)
                {
                    formSaveAs.isGlobalChecked = true;
                }
                else
                {
                    formSaveAs.isUserChecked = true;
                }
                formSaveAs.SaveAsMethod = MIDText.GetTextOnly((int)frmVelocity.ABM.MethodType);
                if (frmVelocity.UserSecurity.AllowUpdate &&
                       !frmVelocity.GlobalSecurity.AllowUpdate)
                {
                    formSaveAs.isUserChecked = true;
                    formSaveAs.EnableGlobal = false;
                    formSaveAs.EnableUser = false;
                }
                else if (!frmVelocity.UserSecurity.AllowUpdate &&
                       frmVelocity.GlobalSecurity.AllowUpdate)
                {
                    formSaveAs.isGlobalChecked = true;
                    formSaveAs.EnableGlobal = false;
                    formSaveAs.EnableUser = false;
                }

                formSaveAs.ShowViewOption = true;
                formSaveAs.SaveAsViewName = frmVelocity.SaveAsViewName;
                if (frmVelocity.SaveAsViewUserRID == Include.GlobalUserRID)
                {
                    formSaveAs.isGlobalViewChecked = true;
                }
                else
                {
                    formSaveAs.isUserViewChecked = true;
                }

                formSaveAs.ShowDetailViewOption = true;

                int userRID = SAB.ClientServerSession.UserRID;
                DataRow viewRow = GetStoreDetailViewRow();
                if (viewRow != null)
                {
                    //viewRID = Convert.ToInt32(viewRow["VIEW_RID"], CultureInfo.CurrentUICulture);
                    formSaveAs.SaveAsDetailViewName = Convert.ToString(viewRow["VIEW_ID"], CultureInfo.CurrentUICulture);
                    userRID = Convert.ToInt32(viewRow["USER_RID"], CultureInfo.CurrentUICulture);
                }
                if (userRID == Include.GlobalUserRID)
                {
                    formSaveAs.isGlobalDetailViewChecked = true;
                }
                else
                {
                    formSaveAs.isUserDetailViewChecked = true;
                }
              
                formSaveAs.ShowUserGlobal = true;
                formSaveAs.EnableGlobalView = frmVelocity.MethodViewGlobalSecurity.AllowUpdate;
                formSaveAs.EnableUserView = frmVelocity.MethodViewUserSecurity.AllowUpdate; 
                formSaveAs.EnableGlobalDetail = frmVelocity.MethodDetailViewGlobalSecurity.AllowUpdate;
                formSaveAs.EnableUserDetail = frmVelocity.MethodDetailViewUserSecurity.AllowUpdate; 

                formSaveAs.SaveMethod = false;
                formSaveAs.SaveDetailView = true;
                bool continueSave = false;
                bool saveAsCanceled = false;
                bool successful = true;
                formSaveAs.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                while (!continueSave)
                {
                    formSaveAs.ShowDialog(this);
                    saveAsCanceled = formSaveAs.SaveCanceled;
                    if (!saveAsCanceled)
                    {
                        saveAsCanceled = false;
                        continueSave = true;
                        if (formSaveAs.SaveMethod)  // TT#231  RMatelic - Add Views to Velocity Matrix and Store Detail
                        {
                            frmVelocity.ABM.Name = formSaveAs.SaveAsName;
                            if (formSaveAs.isUserChecked)
                            {
                                frmVelocity.ABM.User_RID = SAB.ClientServerSession.UserRID;
                            }
                            else
                            {
                                frmVelocity.ABM.User_RID = Include.GlobalUserRID;
                            }

                            int newItemKey = Include.NoRID;
                            if (frmVelocity.IsNameDuplicate())
                            {
                                MessageBox.Show(frmVelocity.WorkflowMethodNameMessage, this.Text);
                                continueSave = false;
                            }
                            else if (!frmVelocity.SaveValuesFromExternalSource(ref newItemKey))
                            {
                                MessageBox.Show(frmVelocity.WorkflowMethodNameMessage, this.Text);
                                continueSave = true;
                                successful = false;
                            }
                        }

                        if (successful && continueSave && formSaveAs.SaveView)
                        {
                            frmVelocity.SaveAsViewUserRID = (formSaveAs.isUserViewChecked ? SAB.ClientServerSession.UserRID : Include.GlobalUserRID);
                            frmVelocity.SaveAsViewName = formSaveAs.SaveAsViewName;
                            frmVelocity.SaveGridView();
                        }
                        if (successful && continueSave && formSaveAs.SaveDetailView)
                        {
                            _saveAsDetailViewUserRID = (formSaveAs.isUserDetailViewChecked ? SAB.ClientServerSession.UserRID : Include.GlobalUserRID);
                            _saveAsDetailViewName = formSaveAs.SaveAsDetailViewName;
                            SaveGridView();
                            // Begin TT#330 - RMatelic -Velocity Method View when do a Save As of a view after the Save the method closes
                            //BindViewCombo();
                            //cmbView.SelectedValue = _viewRID;
                            // End TT#330  
                        }
                    }
                    else
                    {
                        continueSave = true;
                        successful = false;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void SaveGridView()
        {
            SaveGridView(_saveAsDetailViewUserRID, _saveAsDetailViewName);
        }

        public void SaveGridView(int aViewUserRID, string aViewName)
        {
            try
            {
                _gridViewData.OpenUpdateConnection();

                try
                {
                    _viewRID = _gridViewData.GridView_GetKey(aViewUserRID, (int)_layoutID, aViewName);
                    if (_viewRID != Include.NoRID)
                    {
                        _gridViewData.GridViewDetail_Delete(_viewRID);
                        // Begin TT#456 - RMatelic - Add views to Size Review
                        _gridViewData.GridView_Update(_viewRID, true, _trans.AllocationGroupBy, Include.NoRID, false, Include.NoRID, false); //TT#1313-MD -jsobek -Header Filters
                        // End TT#456 
                    }
                    else
                    {   
                        // Begin TT#456 - RMatelic - Add views to Size Review
                        //_viewRID = _gridViewData.GridView_Insert(aViewUserRID, (int)_layoutID, aViewName);
                        _viewRID = _gridViewData.GridView_Insert(aViewUserRID, (int)_layoutID, aViewName, false, _trans.AllocationGroupBy, Include.NoRID, false, Include.NoRID, false); //TT#1313-MD -jsobek -Header Filters
                        // End TT#456
                    }

                    ArrayList flexGrids = new ArrayList();
                    flexGrids.Add(g1);
                    flexGrids.Add(g2);
                    flexGrids.Add(g3);
                    // Begin TT#710-MD - JSmith - Adding view for Style Component with multiple header selected causes IndexOutOfRangeException
                    //_gridViewData.GridViewDetail_Insert(_viewRID, flexGrids);
                    if (_trans.AllocationGroupBy == Convert.ToInt32(eAllocationStyleViewGroupBy.Header, CultureInfo.CurrentUICulture) ||
                        _headerList.Count == 1)
                    {
                        _gridViewData.GridViewDetail_Insert(_viewRID, flexGrids);
                    }
                    else
                    {
                        _gridViewData.GridViewDetail_Insert(_viewRID, flexGrids, _headerList.Count);
                    }
                    // End TT#710-MD - JSmith - Adding view for Style Component with multiple header selected causes IndexOutOfRangeException
                    _gridViewData.CommitData();
                }
                catch (Exception exc)
                {
                    ErrorFound = true;
                    _gridViewData.Rollback();
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    _gridViewData.CloseUpdateConnection();
                    // Begin TT#330 - RMatelic -Velocity Method View when do a Save As of a view after the Save the method closes
                    BindViewCombo();
                    cmbView.SelectedValue = _viewRID;
                    //this.cmbView_SelectionChangeCommitted(source, new EventArgs()); // TT#294-MD - JSmith - When Opening style review, the view does not open that is selected
                    // End TT#330  
                }
            }
            catch (Exception exc)
            {
                ErrorFound = true;
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#231  

		public override void ISaveAs()
		{
			try
            {   // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
				//ISave();
                Cursor.Current = Cursors.WaitCursor;
                if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
                {
                    ShowSaveAsDialog();
                }
                else
                {
                    // Begin TT#454 - RMatelic - Add Views in Style Review
                    //SaveChanges();
                    SaveView();
                    // End TT#454
                }
            }   // End TT#231
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		//		public override void IClose()
		//		{
		//			try
		//			{
		//				this.Close();
		//			}
		//			catch (Exception ex)
		//			{
		//				HandleException(ex);
		//			}
		//		}

        // Begin TT#454 - RMatelic - Add Views in Style Review
        private void SaveView()
        {
            try
            {
                ViewParms viewParms = new ViewParms();
                viewParms.LayoutID = (int)eLayoutID.styleReviewGrid;
                viewParms.UpdateHandledByCaller = true;
                viewParms.ViewName = Convert.ToString(cmbView.Text, CultureInfo.CurrentUICulture);
                viewParms.ViewRID = Convert.ToInt32(cmbView.SelectedValue, CultureInfo.CurrentUICulture);
                if (viewParms.ViewRID != Include.NoRID)
                {
                    viewParms.ViewUserRID = Convert.ToInt32(_dtViews.Rows[(int)cmbView.SelectedIndex]["USER_RID"], CultureInfo.CurrentUICulture);
                }
                else
                {
                    viewParms.ViewUserRID = _sab.ClientServerSession.UserRID;
                }
                viewParms.FunctionSecurity = eSecurityFunctions.AllocationViews;
                viewParms.GlobalViewSecurity = eSecurityFunctions.AllocationViewsGlobalStyleReview;
                viewParms.UserViewSecurity = eSecurityFunctions.AllocationViewsUserStyleReview;

                ViewSave gridViewSaveForm = new ViewSave(_sab, viewParms, false);	// TT#1390-MD - stodd - Assortment Workspace Save View lists headers filters instead of assortment header filters.

                gridViewSaveForm.OnViewSaveClosingEventHandler += new ViewSave.ViewSaveClosingEventHandler(OnViewSaveClosing);
                gridViewSaveForm.MdiParent = this.ParentForm.Owner;
                gridViewSaveForm.ShowDialog();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        void OnViewSaveClosing(object aSource, ViewParms aViewParms)
        {
            try
            {
                if (aViewParms.ViewSaved)
                {
                    SaveGridView(aViewParms.ViewUserRID, aViewParms.ViewName);
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
        // End TT#454  

		public void SaveFromExternalSource()
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				SaveChanges();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}
		override protected bool SaveChanges()
		{
			System.EventArgs args = new EventArgs();
            bool headerSavedOK = true;                   // TT#1185 - Verify ENQ before Update
            try
			{

				if (   _trans.DataState == eDataState.Updatable
                    && FunctionSecurity.AllowUpdate                                // TT#1185 - Verify ENQ before Update
                    && _headerList != null                                         // TT#1185 - Verify ENQ before Update
                    && _trans.AreHeadersEnqueued(_headerList))                     // TT#1185 - Verify ENQ before Update
					//&& _trans.HeadersEnqueued && FunctionSecurity.AllowUpdate )  // TT#1185 - Verify ENQ before Update
				{
					// BEGIN MID Track #2532 - Balance proportional does not save
					// btnApply_Click(this,args);
					if ( !CheckForChangedCellOK() )
						return false;
					//if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
					//	btnAllocate_Click(this,args);
					//else
					// RonM took out above 'if...' for 2532 per emails on 6-24-05 
						btnApply_Click(this,args);
					// END MID Track #2532

                    headerSavedOK = _trans.SaveHeaders(); // TT#1185 - Verify ENQ before Update
                    // begin TT#1185 - Verify ENQ before Update
                    if (!headerSavedOK)
                    {
                        string message = MIDText.GetTextOnly((int)eMIDTextCode.msg_al_HeaderUpdateFailed);
                        MessageBox.Show(message, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    // end TT#1185 - Verify ENQ before Update
					UpdateAllocationWorkspace();
				}
				_trans.SaveAllocationDefaults();
                SaveUserGridView();     // TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
				ChangePending = false;
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
            return headerSavedOK;  // TT#1185 - Verify ENQ before Update
            //return true;         // TT#1185 - Verify ENQ before Update
		}

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private void SaveUserGridView() 
        {   // Begin TT#334 -RMatelic -  Unrelated - removal of view is not being retained
            //if (_userGridView != null && _viewRID > 0)  // TT#318 unrelated to TT; error if not velocity 
            if (_userGridView != null && _viewRID != 0)
            // Begin TT#334    
            {
                _userGridView.UserGridView_Update(SAB.ClientServerSession.UserRID, _layoutID, _viewRID);
            }
        }
        // End TT#231  

		private void UpdateAllocationWorkspace()
		{
			int[] hdrIdList;
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
			int[] mstrIdList;
// (CSMITH) - END MID Track #3219
			try
			{
				hdrIdList = new int[_headerList.Count];
				int i = 0;
				foreach (AllocationHeaderProfile ahp in _headerList)
				{	
					hdrIdList[i] = Convert.ToInt32(ahp.Key, CultureInfo.CurrentUICulture );
					i++;
				}
				if (_awExplorer != null)
				{
					_awExplorer.ReloadUpdatedHeaders(hdrIdList);
				}
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
				if (_masterKeyList != null)
				{
					if (_masterKeyList.Count > 0)
					{
						i = 0;

						mstrIdList = new int[_masterKeyList.Count];

						foreach (int mstrKey in _masterKeyList)
						{
							mstrIdList[i++] = mstrKey;
						}

						if (_awExplorer != null)
						{
							_awExplorer.ReloadUpdatedHeaders(mstrIdList);
						}
					}
				}
// (CSMITH) - END MID Track #3219
				// Begin - John Smith - Linked headers
				AllocationProfileList apl = _trans.LinkedHeaderList;
				if (apl.Count > 0)
				{
					i = 0;

					hdrIdList = new int[apl.Count];

					foreach (AllocationProfile ap in apl)
					{
						hdrIdList[i++] = ap.Key;
					}

					if (_awExplorer != null)
					{
						_awExplorer.ReloadUpdatedHeaders(hdrIdList);
					}
				}
				// End - Linked headers
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}

		}
		#endregion
		#region ComboBox Binding and Selection
			
		/// <summary>
		/// Populate all values of the Store_Group_Levels (Attribute Sets)
		/// (based on key from Store_Group) of the cmbStoreAttribute
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cmbStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
        private void cmbStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{ 
				if (!_loading)
				{
                    // Begin Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set
                    if (_applyPending &&
                        !_applyPendingMsgDisplayed)
                    {
                        _applyPendingMsgDisplayed = true;
                        if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ApplyPendingChanges), _thisTitle,
                                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            btnApply_Click(this, new EventArgs());
                        }
                        else
                        {
                            this._allocationWaferCellChangeList = new AllocationWaferCellChangeList();
                        }
                    }
                    // End Development TT#8
					_resetV1Splitter = true;     
					_trans.AllocationStoreAttributeID = Convert.ToInt32(cmbStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
                    //BEGIN TT#2927-MD-VStuart-Error during Single Store Select
					//Begin TT#1517-MD -jsobek -Store Service Optimization
                    //_trans.CurrentStoreGroupProfile = (StoreGroupProfile)(SAB.ApplicationServerSession.GetProfileList(eProfileType.StoreGroup)).FindKey(_trans.AllocationStoreAttributeID);
                    _trans.CurrentStoreGroupProfile = StoreMgmt.StoreGroup_Get(_trans.AllocationStoreAttributeID); //(StoreGroupProfile)StoreMgmt.GetStoreGroupList().FindKey(_trans.AllocationStoreAttributeID); //TT#1517-MD -jsobek -Store Service Optimization
					//End TT#1517-MD -jsobek -Store Service Optimization
                    //END   TT#2927-MD-VStuart-Error during Single Store Select

                    // BEGIN TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
                    if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
                    {
                        _trans.VelocityStoreGroupRID = Convert.ToInt32(cmbStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
                    }
                    // END TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)

                    // Begin Track #6404 - JSmith - 80302: Units to allocate is calculated when header is Work Up Total Buy
                    //ChangePending = true;
                    // End Track #6404
				}
				
				if (this.cmbStoreAttribute.SelectedValue != null)
					PopulateStoreAttributeSet(this.cmbStoreAttribute.SelectedValue.ToString());
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        // Begin Track #4872 - JSmith - Global/User Attributes
        private void cmbStoreAttribute_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cmbStoreAttribute_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }
        // End Track #4872

		private void cmbStoreAttribute_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //Begin Track #5858 - Kjohnson - Validating store security only
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }
		/// <summary>
		/// Populate all values of the Store_Group_Levels (Attribute Sets)
		/// based on the key parameter.
		/// </summary>
		/// <param name="key">SGL_RID</param>
		private void PopulateStoreAttributeSet(string key)
		{
			try
			{
//Begin Track #3767 - JScott - Force client to use cached store group lists in application session
//				ProfileList pl = _sab.StoreServerSession.GetStoreGroupLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture));
				//Begin TT#1517-MD -jsobek -Store Service Optimization
				//ProfileList pl = _sab.ApplicationServerSession.GetStoreGroupLevelListViewProfileList(Convert.ToInt32(key, CultureInfo.CurrentUICulture), false);
                ProfileList pl = StoreMgmt.StoreGroup_GetLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture), false);
				//End TT#1517-MD -jsobek -Store Service Optimization
//End Track #3767 - JScott - Force client to use cached store group lists in application session

				//if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
				//	pl.ArrayList.Add(_totalMatrixSet) ;

                //var z = new EventArgs();	//TT#301-MD-VStuart-Version 5.0-Controls

				// BEGIN MID Track #3834 - error when more than 1 view open and attribute or set is changed
				object obj = pl.Clone();
				ProfileList attList = (ProfileList)obj;
                //BEGIN TT#6-MD-VStuart - Single Store Select
				cmbAttributeSet.ValueMember = "Key";
				cmbAttributeSet.DisplayMember = "Name";
				//cmbAttributeSet.DataSource = pl.ArrayList;
				cmbAttributeSet.DataSource = attList.ArrayList;
                //this.cmbAttributeSet_SelectionChangeCommitted(this, z); //TT#301-MD-VStuart-Version 5.0-Controls
                // END MID Track #3834
                _storeGroupLevelProfileList = attList;

                if (this.cmbAttributeSet.Items.Count > 0)	
				{
					if (_loading)
						this.cmbAttributeSet.SelectedValue = _trans.AllocationStoreGroupLevel;
					else
                        this.cmbAttributeSet.SelectedIndex = 0;
                    //this.cmbAttributeSet_SelectionChangeCommitted(this, z); //TT#7 - RBeck - Dynamic dropdowns

				}
                //this.toolTip1.SetToolTip(this.cmbAttributeSet, "Attribute Sets");
                //AdjustTextWidthComboBox_DropDown(cmbAttributeSet.ComboBox);  // TT#1701 - RMatelic
                //END TT#6-MD-VStuart - Single Store Select
            }
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		/// <summary>
		/// Populate all Store_Groups (Attributes); 1st sel if new else selection made
		/// in load
		/// </summary>
		private void BindStoreAttrComboBox()
		{
            // Begin Track #4872 - JSmith - Global/User Attributes
            FunctionSecurityProfile userAttrSecLvl;
            // End Track #4872
			try
			{
//Begin Track #3767 - JScott - Force client to use cached store group lists in application session
//				ProfileList al = _sab.StoreServerSession.GetStoreGroupListViewList();
				//Begin TT#1517-MD -jsobek -Store Service Optimization
				//ProfileList al = _sab.ApplicationServerSession.GetProfileList(eProfileType.StoreGroupListView);
                ProfileList al = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.MyUserAndGlobal ,true);   // TT#1517-MD - Store Service Optimization - SRISCH - Changed from ALL
				//End TT#1517-MD -jsobek -Store Service Optimization
//End Track #3767 - JScott - Force client to use cached store group lists in application session
				
				// BEGIN MID Track #3834 - error when more than 1 view open and attribute or set is changed
				object obj = al.Clone();
				ProfileList attList = (ProfileList)obj;
                // Begin Track #4872 - JSmith - Global/User Attributes
                //this.cmbStoreAttribute.ValueMember = "Key";
                //this.cmbStoreAttribute.DisplayMember = "Name";
                ////this.cmbStoreAttribute.DataSource = al.ArrayList;
                //this.cmbStoreAttribute.DataSource = attList.ArrayList;
                userAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
                cmbStoreAttribute.Initialize(SAB, FunctionSecurity, attList.ArrayList, !userAttrSecLvl.AccessDenied);
                // END MID Track #3834
                // End Track #4872

                //var z = new EventArgs();  //TT#301-MD-VStuart-Version 5.0-Controls
	
				if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
				{
					this.cmbStoreAttribute.SelectedValue = _trans.VelocityStoreGroupRID;
                    //this.cmbStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); //TT#301-MD-VStuart-Version 5.0-Controls
                    // BEGIN TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
                    //this.cmbStoreAttribute.Enabled = false;
                    this.cmbStoreAttribute.Enabled = true;
                    // END TT#506 - AGallagher - Velocity - Allow Changing Store Attributes (#62)
				}
				else
				{
				    if (_trans.AllocationCriteriaExists)
				    {
				        this.cmbStoreAttribute.SelectedValue = _trans.AllocationStoreAttributeID;
				        //this.cmbStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); //TT#301-MD-VStuart-Version 5.0-Controls
				    }
				    else
				    {
				    this.cmbStoreAttribute.SelectedIndex = 0;
                    //this.cmbStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); //TT#7 - RBeck - Dynamic dropdowns
				    }

			}
                AdjustTextWidthComboBox_DropDown(cmbStoreAttribute);  // TT#1401 - AGallagher - Reservation Stores		
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void BindActionCombo()
		{	
			try 
			{
                // Begin TT#785 - RMatelic - Header Load Interfacing a transaction  trying to Modify a WUB header with a PO type
                //PopulateCommonCriteria((int)eAllocationActionType.StyleNeed,
                //    // BEGIN MID Track 3099 Cancel Size Allocation does not remove Detail Packs
                //    //(int)eAllocationActionType.BackoutDetailPackAllocation, this.cboAction, // MID Track 4554 AnF Enhancement API Workflow
                //    (int)eAllocationActionType.ApplyAPI_Workflow, this.cboAction, // MID Track 4554 AnF Enhancement API Workflow
                //    String.Empty);
                //    //(int)eAllocationActionType.BackoutAllocation, this.cboAction,
                //    //String.Empty);
                //    // END MID Track 3099

                //begin TT#843 - New Size Constraint Balance
                //PopulateCommonCriteria((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.ReapplyTotalAllocation, this.cboAction, String.Empty);
                // begin TT#794 - New Size Balance for Wet Seal
                //PopulateCommonCriteria((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BalanceSizeWithConstraints, this.cboAction, String.Empty);
                //PopulateCommonCriteria((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BalanceSizeBilaterally, this.cboAction, String.Empty);
                //end TT#794 - New Size Balance for Wet Seal
                //BEGIN TT#6-MD-VStuart - Single Store Select
				//PopulateCommonCriteria((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BreakoutSizesAsReceivedWithConstraints, this.cboAction.ComboBox, String.Empty); // TT#1391 - Jellis - Balance Size With Constraint Other Options
                PopulateCommonCriteria((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BalanceToVSW, this.cboAction.ComboBox, String.Empty); // TT#1334-MD - stodd - Balance to VSW Action

                //this.toolTip1.SetToolTip(this.cboAction, "Actions");
                // end TT#843 - New Size Constraint Balance
                // End TT#785
                //AdjustTextWidthComboBox_DropDown(cboAction);  // TT#1401 - AGallagher - Reservation Stores
                //END TT#6-MD-VStuart - Single Store Select
            }
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		/// <summary>
		/// Generic method to populate all MIDComboBoxes that use MIDText.GetLabels function
		/// </summary>
		/// <param name="startVal">enum start value</param>
		/// <param name="endVal">enum end value</param>
		/// <param name="CboBox">MIDComboBox control name</param>
		/// <param name="selectVal">selected value for combo box</param>
		private void PopulateCommonCriteria(int startVal, int endVal, ComboBox CboBox, string selectVal) 
		{
			try
			{
				// BEGIN MID Track 3099 Cancel Size Allocation Returns No action Performed when no bulk size
				//DataRow dr;
				//DataTable dt = MIDEnvironment.CreateDataTable();
				//dt = MIDText.GetLables(startVal, endVal);
				//dr = dt.NewRow();
				//dr["TEXT_CODE"] = 0;
				//dr["TEXT_VALUE"] = String.Empty;
				//dt.Rows.Add(dr);
				//if (!_sab.ClientServerSession.GlobalOptions.SizeNeedInd)
				//{
				//	for (int i = dt.Rows.Count - 1; i >= 0; i--)
				//	{
				//		DataRow dRow = dt.Rows[i];
				//		if ( Convert.ToInt32(dRow["TEXT_CODE"])  ==  Convert.ToInt32(eAllocationActionType.BackoutSizeAllocation)
				//			|| 	Convert.ToInt32(dRow["TEXT_CODE"]) == Convert.ToInt32(eAllocationActionType.BackoutSizeIntransit)
				//			|| 	Convert.ToInt32(dRow["TEXT_CODE"]) == Convert.ToInt32(eAllocationActionType.BalanceSizeNoSubs)
				//			|| 	Convert.ToInt32(dRow["TEXT_CODE"]) == Convert.ToInt32(eAllocationActionType.BalanceSizeWithSubs)
				//			|| 	Convert.ToInt32(dRow["TEXT_CODE"]) == Convert.ToInt32(eAllocationActionType.BreakoutSizesAsReceived)
				//			) 
				//		{
				//			dt.Rows.Remove(dRow);
				//		}
				//	}	 
				//}
				DataRow dr;
				DataTable dt = MIDText.GetLabels(startVal, endVal);
				Hashtable removeEntry = new Hashtable();
				removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutDetailPackAllocation), eAllocationActionType.BackoutDetailPackAllocation);
				removeEntry.Add(Convert.ToInt32(eAllocationActionType.ChargeSizeIntransit), eAllocationActionType.ChargeSizeIntransit);
				removeEntry.Add(Convert.ToInt32(eAllocationActionType.DeleteHeader), eAllocationActionType.DeleteHeader);
				if (!_sab.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
				{
					removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutSizeAllocation), eAllocationActionType.BackoutSizeAllocation);
					removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutSizeIntransit), eAllocationActionType.BackoutSizeIntransit);
					removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeNoSubs), eAllocationActionType.BalanceSizeNoSubs);
					removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithSubs), eAllocationActionType.BalanceSizeWithSubs);
					removeEntry.Add(Convert.ToInt32(eAllocationActionType.BreakoutSizesAsReceived), eAllocationActionType.BreakoutSizesAsReceived);
                    removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithConstraints), eAllocationActionType.BalanceSizeWithConstraints); // TT#843 - New Size Constraint Balance
                    removeEntry.Add(Convert.ToInt32(eAllocationActionType.BalanceSizeWithConstraints), eAllocationActionType.BalanceSizeBilaterally); // TT#794 - New Size Balance for Wet Seal
					removeEntry.Add(Convert.ToInt32(eAllocationActionType.BreakoutSizesAsReceivedWithConstraints), eAllocationActionType.BreakoutSizesAsReceivedWithConstraints); // TT#1391 - Jellis - Balance Size With Constraint Other Options
                    //removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutDetailPackAllocation), eAllocationActionType.BackoutDetailPackAllocation);
				}
                int codeValue;
				for (int i = dt.Rows.Count - 1; i >= 0; i--) 
				{
					dr = dt.Rows[i];
					codeValue = Convert.ToInt32(dr["TEXT_CODE"]);
					if ( removeEntry.Contains(codeValue))  
					{
						dt.Rows.Remove(dr);
					}
					else if (!Enum.IsDefined(typeof(eAllocationActionType),(eAllocationActionType)codeValue))	 
						dt.Rows.Remove(dr);
				}	 
				dr = dt.NewRow();
				dr["TEXT_CODE"] = 0;
				dr["TEXT_VALUE"] = String.Empty;
				dt.Rows.Add(dr);
				//dt.Rows.Add(new object[] { -1, "Select Action...", 0, 0, 0, 0 });
				// END MID Track 3099 Cancel Size Allocation Returns No action Performed when no bulk size

				dt = CheckSecurityForActions(dt);
				// Switch to DataView in order to sort
				DataView dv = new DataView(dt);
                // begin TT#843 - New Size Constraint Balance - Unrelated issue (pull down list not ordered)
                //dv.Sort = "TEXT_CODE";
                dv.Sort = "TEXT_ORDER";
                // end TT#843 - New Size Constraint Balance - Unrelated issue (pull down list not ordered)

				CboBox.DisplayMember = "TEXT_VALUE";
				CboBox.ValueMember = "TEXT_CODE";
				CboBox.DataSource = dv;
		
				CboBox.SelectedValue = 0;
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private DataTable CheckSecurityForActions(DataTable dtActions)
		{
			try 
			{
				// BEGIN MID Track #4357 - security for interfaced and non-interfaced headers
				bool allowAction = true;
				FunctionSecurityProfile actionSecurity = null;
				// END MID Track #4357
				foreach(int action in Enum.GetValues(typeof( eAllocationActionType)))
				{
					// BEGIN MID Track #4357 - security for interfaced and non-interfaced headers
//					FunctionSecurityProfile actionSecurity = _sab.ClientServerSession.GetMyUserActionSecurityAssignment((eAllocationActionType)action);
//					if (actionSecurity.AccessDenied)
					allowAction = true;
					foreach (AllocationHeaderProfile ahp in _headerList)
					{
						actionSecurity = _sab.ClientServerSession.GetMyUserActionSecurityAssignment((eAllocationActionType)action, ahp.IsInterfaced);
						if (actionSecurity.AccessDenied) 
						{
							allowAction = false;
							break;
						}
					}
					if (!allowAction) 
					// END MID Track #4357
					{
						for (int i = dtActions.Rows.Count - 1; i >= 0; i--) 
						{
							DataRow dr = dtActions.Rows[i];
							if ( Convert.ToInt32(dr["TEXT_CODE"])  ==  action) 
							{
								dtActions.Rows.Remove(dr);
							}
						}	 

					}
				}
				return dtActions;
			}
			catch (Exception ex) 
			{			
				HandleException(ex);
				throw;
			}
		}

		private void BindFilterComboBox()
		{
			try
			{
//				_bindingFilter = true;

				_lastFilterValue = _trans.AllocationFilterID;
                //BEGIN TT#6-MD-VStuart - Single Store Select
				cmbFilter.Items.Clear();
				cmbFilter.Items.Add(new FilterNameCombo(-1, Include.GlobalUserRID, "(None)"));  // Issue 3806

				foreach (DataRow row in _trans.AllocationFilterTable.Rows)
				{
					cmbFilter.Items.Add(
						new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture)));
				}

				cmbFilter.SelectedItem = new FilterNameCombo(_trans.AllocationFilterID);

                //this.toolTip1.SetToolTip(this.cmbFilter, "Filter");
                //END TT#6-MD-VStuart - Single Store Select
//				_bindingFilter = false;
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private void BindViewCombo()
        {
            try
            {
                if (_userRIDList.Count > 0)
                {
                    _bindingView = true;
                    // Begin TT#454 - RMatelic - Add Views in Style Review
                    //_dtViews = _gridViewData.GridView_Read((int)eLayoutID.velocityStoreDetailGrid, _userRIDList);
                    //_dtViews.Rows.Add(new object[] { Include.NoRID, SAB.ClientServerSession.UserRID, (int)eLayoutID.velocityStoreDetailGrid, string.Empty });
                    // Begin TT#1117 - JSmith - Global & User Views w/ the same names do not have indicators
                    //_dtViews = _gridViewData.GridView_Read((int)_layoutID, _userRIDList);
                    _dtViews = _gridViewData.GridView_Read((int)_layoutID, _userRIDList, true);
                    // End TT#1117
                    _dtViews.Rows.Add(new object[] { Include.NoRID, SAB.ClientServerSession.UserRID, _layoutID, string.Empty });
                    // End TT#454 
                    _dtViews.PrimaryKey = new DataColumn[] { _dtViews.Columns["VIEW_RID"] };

                    //BEGIN TT#6-MD-VStuart - Single Store Select
                    cmbView.ValueMember = "VIEW_RID";
                    cmbView.DisplayMember = "VIEW_ID";
                    cmbView.DataSource = _dtViews;
                    cmbView.SelectedValue = -1;
                    //this.cmbView_SelectionChangeCommitted(source, new EventArgs()); // TT#294-MD - JSmith - When Opening style review, the view does not open that is selected

                    _bindingView = false;
                }
                else
                {
                    cmbView.Enabled = false;
                }
                //AdjustTextWidthComboBox_DropDown(cmbView);  // TT#1401 - AGallagher - Reservation Stores

                //this.toolTip1.SetToolTip(this.cmbView, "Views");
                //END TT#6-MD-VStuart - Single Store Select
            }   
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // End TT#231 

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cmbFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
        private void cmbFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
				if (cmbFilter.SelectedIndex != -1)
				{
					if (((FilterNameCombo)cmbFilter.SelectedItem).FilterRID == -1)
					{
						cmbFilter.SelectedIndex = -1;
					}
				}
				if (cmbFilter.SelectedIndex != -1)
					_trans.AllocationFilterID = ((FilterNameCombo)cmbFilter.SelectedItem).FilterRID;
				else
					_trans.AllocationFilterID = Include.NoRID;

				if(!_loading)
					ReloadGridData();

			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        private void cmbFilter_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cmbFilter_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void cmbFilter_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //Begin Track #5858 - Kjohnson - Validating store security only
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ((MIDComboBoxEnh)((ComboBox)sender).Parent).FirePropertyChangeEvent();
                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }
		#endregion 

		/// <summary>
		/// determin which rows/columns/cells are lockable or editable and put
		/// that info in the tag (which is the "UserData" property in flexgrid).
		/// </summary>
		private void AssignTag()
		{			
			int col;
			TagForGridData dataTag;
			RowColHeader rch;
			AllocationWaferCoordinate wafercoord;	
			//AllocationWaferCoordinateList wafercoordlist;
			
			//Initialize a GridDataTag, which is going to be assigned to every single cell
			dataTag = new TagForGridData();
			dataTag.IsLocked = false; //initially nothing's locked. Might need to check the business rule on this.

			//Determine which columns and rows are lockable. 
			//Lockable rows/columns are used to determin which cells are editable.
			
			//g1
			_wafer = _wafers[0,0];
			_cells = _wafer.Cells;
			
			for (col = 0; col < g1.Cols.Count; col++)
			{
				TagForColumn colTag = (TagForColumn)g1.Cols[col].UserData;
				if ( Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture) == _lblStore)
					colTag.IsLockable = false;
				else
				{
					//colTag.IsLockable = colTag.CubeWaferCoorList.Cell.CellCanBeChanged;
					if (col < _wafer.Columns.Count && _cells.Length > 0)
						colTag.IsLockable = _cells[0,col].CellCanBeChanged;
					else
						colTag.IsLockable = false;
				}
				
				if (_setHeaderTags)
				{
                    if (Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture) == _lblStore)
                    {
                        colTag.IsDisplayed = true;
                    }
                    else if (_trans.AllocationViewType == eAllocationSelectionViewType.Style &&
                        !_showNeedGrid)
                    {
                        colTag.IsDisplayed = false;
                    }
                    else
                    {   // display default columns	
                        if (colTag.CubeWaferCoorList == null)
                            colTag.IsDisplayed = true;
                        else
                        {
                            //wafercoord = colTag.CubeWaferCoorList[2]; // MID Track 4296 Column Chooser Broken
                            wafercoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable); // MID Track 4296 Column Cooser Broken
                            switch (_trans.AllocationViewType)
                            {
                                case eAllocationSelectionViewType.Style:
                                    eAllocationStyleViewVariableDefault asd = (eAllocationStyleViewVariableDefault)wafercoord.Key;
                                    if (Enum.IsDefined(typeof(eAllocationStyleViewVariableDefault), asd))
                                        colTag.IsDisplayed = true;
                                    else
                                        colTag.IsDisplayed = false;
                                    break;

                                case eAllocationSelectionViewType.Velocity:
                                    eAllocationVelocityViewVariableDefault avd = (eAllocationVelocityViewVariableDefault)wafercoord.Key;
                                    if (Enum.IsDefined(typeof(eAllocationVelocityViewVariableDefault), avd))
                                        colTag.IsDisplayed = true;
                                    else
                                        colTag.IsDisplayed = false;
                                    break;
                            }
                        }
                    }	
					colTag.Sort = SortEnum.none;
				}
				else if (ColHeaders1 != null)	
				{
					if (Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture) == _lblStore)
					{
						colTag.IsDisplayed = true;
						colTag.Sort = SortEnum.none;
					}
					else
					{
						if (colTag.CubeWaferCoorList != null)
						{
							//wafercoord = colTag.CubeWaferCoorList[2]; // MID Track 4296 Column Chooser Broken
							wafercoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
							rch = (RowColHeader)ColHeaders1[wafercoord.Label];
							colTag.IsDisplayed = rch.IsDisplayed;
						}
						else
						{
							colTag.IsDisplayed = true;
							colTag.Sort = SortEnum.none;
						}
					}
				}
				else
				{
					colTag.IsDisplayed = true;
					colTag.Sort = SortEnum.none;
				}
				colTag.IsBuilt = colTag.IsDisplayed;
				g1.Cols[col].UserData = colTag;
			}

			AssignTag_g2g3(FromGrid.g2);    //g2
			AssignTag_g2g3(FromGrid.g3);    //g3 

			AssignTagsg4_g12(FromGrid.g4);  //g4
			AssignTagsg4_g12(FromGrid.g5);  //g5
			AssignTagsg4_g12(FromGrid.g6);  //g6
			AssignTagsg4_g12(FromGrid.g7);  //g7
			AssignTagsg4_g12(FromGrid.g8);  //g8
			AssignTagsg4_g12(FromGrid.g9);  //g9
			AssignTagsg4_g12(FromGrid.g10); //g10
			AssignTagsg4_g12(FromGrid.g11); //g11
			AssignTagsg4_g12(FromGrid.g12); //g12

		}
		private void AssignTag_g2g3 (FromGrid WhichGrid)
		{
			C1FlexGrid grid = null;
			RowColHeader rch;
			AllocationWaferCoordinate wafercoord;	
		
			try
			{
				switch (WhichGrid)
				{
					case FromGrid.g2:
						grid = g2;
						_wafer = _wafers[0,1];
						_cells = _wafer.Cells;
						break;
					case FromGrid.g3:
						grid = g3;
						_wafer = _wafers[0,2];
						_cells = _wafer.Cells;
						break;
				}

				for (int col = 0; col < grid.Cols.Count; col ++)
				{
					TagForColumn colTag = (TagForColumn)grid.Cols[col].UserData;
					//colTag.IsLockable = colTag.CubeWaferCoorList.Cell.CellCanBeChanged;
					
					if (_cells.LongLength > 0)
                        // Begin TT#328-MD - RMatelic -Enum value displaying in Velocity Store Detail
                        //colTag.IsLockable = _cells[0,col].CellCanBeChanged;
                        colTag.IsLockable = _cells[0,colTag.cellColumn].CellCanBeChanged;
                        // End TT#328-MD
					else
						colTag.IsLockable = false;
					if (_setHeaderTags)
					{
						// display default columns		
						//wafercoord = colTag.CubeWaferCoorList[2]; // MID Track 4296 Column Chooser not working
						wafercoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
						switch (_trans.AllocationViewType)
						{
							case eAllocationSelectionViewType.Style:
								eAllocationStyleViewVariableDefault asd = (eAllocationStyleViewVariableDefault)wafercoord.Key;
								if (Enum.IsDefined(typeof(eAllocationStyleViewVariableDefault),asd))
									colTag.IsDisplayed = true;
								else
									colTag.IsDisplayed = false;
								break;

							case eAllocationSelectionViewType.Velocity:
								eAllocationVelocityViewVariableDefault avd = (eAllocationVelocityViewVariableDefault)wafercoord.Key;
								if (Enum.IsDefined(typeof(eAllocationVelocityViewVariableDefault),avd))
									colTag.IsDisplayed = true;
								else
									colTag.IsDisplayed = false;
								break;
						}
						
						colTag.Sort = SortEnum.none;
					}
					else if (ColHeaders2 != null)	
					{
						wafercoord = colTag.CubeWaferCoorList[_compRow];
						rch = (RowColHeader)ColHeaders2[wafercoord.Label];
						if ( rch != null)
						{
							colTag.IsDisplayed = rch.IsDisplayed;
						}
					}
					else
					{
						colTag.IsDisplayed = true;
						colTag.Sort = SortEnum.none;
					}
					if (_trans.AnalysisOnly)
						colTag.IsDisplayed = false;
					colTag.IsBuilt = colTag.IsDisplayed;
					grid.Cols[col].UserData = colTag;
				}

			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void AssignTagsg4_g12 (FromGrid WhichGrid)
		{
			C1FlexGrid grid = null;
			C1FlexGrid colGrid;
			int row, col;
			CellRange cellRange;
			TagForGridData dataTag;
			AllocationWafer wafer = null;
			AllocationWaferCell [,] cells  = null;
		
			try
			{
				//Initialize a GridDataTag, which is going to be assigned to every single cell
				dataTag = new TagForGridData();
				dataTag.IsLocked = false; //initially nothing's locked. Might need to check the business rule on this.
				switch (WhichGrid)
				{
					case FromGrid.g4:
						grid = g4;
						break;
					case FromGrid.g5:
						grid = g5;
						break;
					case FromGrid.g6:
						grid = g6;
						break;
					case FromGrid.g7:
						grid = g7;
						break;
					case FromGrid.g8:
						grid = g8;
						wafer = _wafers[1,1];
						cells = wafer.Cells;
						break;
					case FromGrid.g9:
						grid = g9;
						wafer = _wafers[1,2];
						cells = wafer.Cells;
						break;
					case FromGrid.g10:
						grid = g10;
						break;
					case FromGrid.g11:
						grid = g11;
						wafer = _wafers[2,1];
						cells = wafer.Cells;
						break;
					case FromGrid.g12:
						grid = g12;
						wafer = _wafers[2,2];
						cells = wafer.Cells;
						break;
				}
				if (grid == g4 || grid == g7 || grid == g10) 
				{
					for (row = 0; row < grid.Rows.Count; row++)
					{
						TagForRow rowTag = (TagForRow)grid.Rows[row].UserData;
						rowTag.IsLockable = false;
						rowTag.IsDisplayed = true;
						grid.Rows[row].UserData = rowTag;
					}
				}
				
				if (grid == g8 || grid == g9 || grid == g11 || grid == g12) 
				{
					for (row = 0; row < grid.Rows.Count; row++)
					{
						for (col = 0; col < grid.Cols.Count; col++)
						{
							dataTag.IsEditable = cells[row,col].CellCanBeChanged;
							cellRange = new CellRange();
							cellRange = grid.GetCellRange(row, col);
							cellRange.UserData = dataTag;
						}
					}
				}
				else
				{
					colGrid = GetColumnGrid(grid);
					for (col = 0; col < grid.Cols.Count; col++)
					{
						//Get the column tag (so later we can check to see if the column is lockable.)
						TagForColumn colTag = (TagForColumn)colGrid.Cols[col].UserData;
				
						if (colTag.IsLockable == true)
						{
							dataTag.IsEditable = true;
						}
						else
						{
							dataTag.IsEditable = false;
						}
						//loop through the rows and make each cell none-editable.
						for (row = 0; row < grid.Rows.Count; row++)
						{
							cellRange = new CellRange();
							cellRange = grid.GetCellRange(row, col);
							cellRange.UserData = dataTag;
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
        // begin MID Track 4296 Column Chooser not working
		private AllocationWaferCoordinate GetAllocationCoordinate(AllocationWaferCoordinateList aCoordList, eAllocationCoordinateType aCoordType) 
		{
			AllocationWaferCoordinate wafercoord = null;
			for (int i = 0; i < aCoordList.Count; i++)
			{
				if (aCoordList[i].CoordinateType ==  aCoordType)
				{
					wafercoord = aCoordList[i];
					break;
				}
			}	
			return wafercoord;
		}
		// end MID Track 4296 Column Chooser not working

		/// <summary>
		/// Used to show/hide columns/rows and apply other user-saved preferences
		/// </summary>
		private void ApplyPreferences()
		{
			//this procedure is not well thought out yet.
			//Because I don't have the "save/retrieve" functionality coded yet,
			//I'll just get the defaults. Later, we need to add code to retrieve
			//actual user preferences and modify the code here.

			string colName;
			bool IsDisplayed;
			TagForColumn colTag;
			RowColHeader rch;
			AllocationWaferCoordinate wafercoord;	
					
			ColHeaders1 = new Hashtable();
			ColHeaders2 = new Hashtable();
			//_alColHeaders2 = new ArrayList();
			
			for (int i = 0; i < g1.Cols.Count; i++)
			{
				colTag = (TagForColumn)g1.Cols[i].UserData;
				IsDisplayed = colTag.IsDisplayed;
				if ( Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture) == _lblStore)
					colName = _lblStore;
				else if (colTag.CubeWaferCoorList == null)
					continue;
				else
				{
					//wafercoord = colTag.CubeWaferCoorList[2]; // MID Track 4296 Column Chooser Broken
                    wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable); // MID Track 4296 Column Chooser Broken
					colName = wafercoord.Label;
				}
				ColHeaders1.Add(colName,new RowColHeader(colName, IsDisplayed));
			}
			//show/hide columns
			RightClickedFrom = FromGrid.g1; //to re-use code, pretend the user right-clicked on g4 because the following code needs to know where the "context menu" was clicked.
            ShowHideColHeaders(RightClickedFrom, ColHeaders1, false, g4, g5, g6);

			//ColHeaders2.Clear();
			//			for (int i = 0; i < g2.Cols.Count; i++)
			//			{
			//				colTag = (TagForColumn)g2.Cols[i].UserData;
			//				IsDisplayed = colTag.IsDisplayed;
			//				wafercoord = colTag.CubeWaferCoorList[_compRow];
			//				colName = wafercoord.Label;
			//				ColHeaders2.Add(colName,new RowColHeader(colName, IsDisplayed));
			//			}
			//			//show/hide columns
			//			RightClickedFrom = FromGrid.g2; //to re-use code, pretend the user right-clicked on g4 because the following code needs to know where the "context menu" was clicked.
			//			ShowHideColHeaders(ColHeaders2,false);

			//ColHeaders2.Clear();
			for (int i = 0; i < g3.Cols.Count; i++)
			{
				colTag = (TagForColumn)g3.Cols[i].UserData;
				IsDisplayed = colTag.IsDisplayed;
				wafercoord = colTag.CubeWaferCoorList[_compRow];
				colName = wafercoord.Label;
				rch = (RowColHeader)ColHeaders2[colName];
				if (rch == null)
				{
					ColHeaders2.Add(colName,new RowColHeader(colName, IsDisplayed));
					//_alColHeaders2.Add(colName);
				}
			}
			//show/hide columns
			RightClickedFrom = FromGrid.g3; //to re-use code, pretend the user right-clicked on g3 because the following code needs to know where the "context menu" was clicked.
            ShowHideColHeaders(RightClickedFrom, ColHeaders2, false, g4, g5, g6);
			//ResizeRows();
		}

		private void MiscPositioning()
		{
			
			ResizeRows();
			if (_doResize)
				ResizeColumns();
			else
				_doResize = true;

			//Miscellaneous - setup positions and sizes for splitters and scroll bars.

			if (_resetV1Splitter)
			{
				SetV1SplitPosition();
				_resetV1Splitter = false;
			}
			
			int gridHeight = g1.Height; 
			g1.Height = g1.Rows[0].HeightDisplay + s4.Height + 3; 
			pnlCorner.Height = g2.Rows[0].HeightDisplay;

			// The following is a code around for Anna's grid getting 'stuck' when single 
			// column sort causes grid to change height because of sort arrow placement;
			// If the mouse isn't moved when sort column is clicked the MouseUp event
			// isn't firing so this forces it.
			if (gridHeight != g1.Height && !_loading) 
			{                                          
                GridMouseUp(g1, null);                 
			}                                         
 
			// BEGIN MID Track #2643 - error closing SKU review
			if (!FormIsClosing)
			{
				SetVScrollBar2Parameters();
				SetVScrollBar3Parameters();
				SetVScrollBar4Parameters();
				if (!_loading)
				{
					SetRowSplitPosition4();
					SetHScrollBar1Parameters();
//					SetHScrollBar2Parameters();
//					SetHScrollBar3Parameters();
				
					SetActionListEnabled();
					// BEGIN MID Track #3027 - Null error when closing window
					if (!FormIsClosing)
					{
						if (_trans.AnalysisOnly)
						{
							btnApply.Enabled = false;
						}	
						else
							btnApply.Enabled = (g4.Rows.Count > 0 && (_trans.DataState == eDataState.Updatable));
					}
					// END MID Track #3027
				}
			}
			// END MID Track #2643
		}
		private void SetActionListEnabled()
		{
			bool enableActions = true;
			// BEGIN MID Track #3027 - Null error when closing window
			try
			{
				// BEGIN MID Track #3027 - Null error when closing window
				if (FormIsClosing)
					return;
				// END MID Track #3027

				if (_trans.AnalysisOnly || FunctionSecurity.AllowUpdate == false
					|| _trans.DataState == eDataState.ReadOnly)
					enableActions = false;
                    // begin TT#1185 - Verify ENQ before Update
                else if (g4 == null 
                         || g4.Rows.Count == 0) 
                {
                    enableActions = false;
                }
                    // end TT#1185 - Verify ENQ before Update
				else
				{
					foreach (AllocationHeaderProfile ahp in _headerList)
					{	
						if (ahp.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance)
						{
							enableActions = false;
							break;
						}
                            // begin TT#1185 - Verify ENQ before Update
                        else if (!_trans.IsHeaderEnqueued(ahp.Key))
                        {
                            enableActions = false;
                        }
                            // end TT#1185 - Verify ENQ before Update
							// Begin MID Track #2551 - check style security for actions
						else
						{
							HierarchyNodeSecurityProfile securityNode = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(ahp.StyleHnRID, (int)eSecurityTypes.Allocation);
							if (!securityNode.AllowUpdate)
							{
								enableActions = false;
								break;
							}
						}
						// End MID Track #2551
					}
                    // begin TT#1185 - Verify ENQ before Update
                    //if (enableActions)
                    //{	
                    //    if (g4 == null || g4.Rows.Count == 0)
                    //        enableActions = false;
                    //    else if (!_trans.HeadersEnqueued)  
                    //        enableActions = false;
                    //}
                    // end TT#1185 - Verify ENQ before Update
				}
				cboAction.Enabled = enableActions;

				// Begin TT#1019 - MD - stodd - prohibit allocation actions against GA - 
                if (_trans.ContainsGroupAllocationHeaders())
                {
                    EnhancedToolTip.SetToolTipWhenDisabled(cboAction, MIDText.GetTextOnly(eMIDTextCode.msg_ActionProtectedGroupAllocation));
                    EnhancedToolTip.SetToolTipWhenDisabled(btnProcess, MIDText.GetTextOnly(eMIDTextCode.msg_ProcessProtectedGroupAllocation));
                    cboAction.Enabled = false;
                    btnProcess.Enabled = false;
                }
				// End TT#1019 - MD - stodd - prohibit allocation actions against GA - 
			}
			catch
			{
				throw;
			}
			// END MID Track #3027
		}	
		private void ResizeColumns()
		{
			// begin MID Track 4708 Size Performance Slow
			//g1.AutoSizeCols(); g2.AutoSizeCols();
			//g3.AutoSizeCols(); g4.AutoSizeCols();
			//g5.AutoSizeCols(); g6.AutoSizeCols();
			//g7.AutoSizeCols(); g8.AutoSizeCols();
			//g9.AutoSizeCols(); g10.AutoSizeCols();
			//g11.AutoSizeCols(); g12.AutoSizeCols();
            g1.AutoSizeCols(); g2.AutoSizeCols();
			g3.AutoSizeCols(); g4.AutoSizeCols();
			g7.AutoSizeCols();
			g10.AutoSizeCols(); g11.AutoSizeCols();
			g12.AutoSizeCols();

			int i;
			//Resize Columns on all grids. Line-up data based on the widest column.
			//And while we're in the loop, set the ImageAlign property of each column.
			int MaxColWidth = 0;  //temp variable to hold the currently widest column.
			//int MaxColWidth = g1.Cols[1].Width;
			for (i = 0; i < g1.Cols.Count; i++)
			{	 
				if (g1.Cols[i].Width > MaxColWidth) MaxColWidth = g1.Cols[i].Width;
				//if (i ==  (g1.Cols.Count - 1))
				//	MaxColWidth = 0;
				if (g4.Cols[i].Width > MaxColWidth) MaxColWidth = g4.Cols[i].Width;
				if (g7.Cols[i].Width > MaxColWidth) MaxColWidth = g7.Cols[i].Width;
				if (g10.Cols[i].Width > MaxColWidth) MaxColWidth = g10.Cols[i].Width;
				 
				g1.Cols[i].Width = MaxColWidth;
				g4.Cols[i].Width = MaxColWidth;
				g7.Cols[i].Width = MaxColWidth;
				g10.Cols[i].Width = MaxColWidth;

				//g1.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g4.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g7.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g10.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;

				MaxColWidth = 0;
				//MaxColWidth = g1.Cols[1].Width;
			}
			 
			MaxColWidth = 0; //reset the temp variable.
			for (i = 0; i < g2.Cols.Count; i++)
			{
				//				if (HeaderOrComponentGroups > 1)
				//				{
				if (g2.Cols[i].Width > MaxColWidth) MaxColWidth = g2.Cols[i].Width;
				if (g5.Cols[i].Width > MaxColWidth) MaxColWidth = g5.Cols[i].Width;
				if (g8.Cols[i].Width > MaxColWidth) MaxColWidth = g8.Cols[i].Width;
				if (g11.Cols[i].Width > MaxColWidth) MaxColWidth = g11.Cols[i].Width;
				//				}	
				g2.Cols[i].Width = MaxColWidth;
				g5.Cols[i].Width = MaxColWidth;
				g8.Cols[i].Width = MaxColWidth;
				g11.Cols[i].Width = MaxColWidth;
				
				//g2.Cols[i].ImageAlign = ImageAlignEnum.RightBottom;
				g5.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g8.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g11.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				MaxColWidth = 0;
				
			}
			MaxColWidth = 0; //reset the temp variable.
			for (i = 0; i < g3.Cols.Count; i++)
			{
				if (g3.Cols[i].Width > MaxColWidth) MaxColWidth = g3.Cols[i].Width;
				if (g6.Cols[i].Width > MaxColWidth) MaxColWidth = g6.Cols[i].Width;
				if (g9.Cols[i].Width > MaxColWidth) MaxColWidth = g9.Cols[i].Width;
				if (g12.Cols[i].Width > MaxColWidth) MaxColWidth = g12.Cols[i].Width;
				
				g3.Cols[i].Width = MaxColWidth;
				g6.Cols[i].Width = MaxColWidth;
				g9.Cols[i].Width = MaxColWidth;
				g12.Cols[i].Width = MaxColWidth;

				//g3.Cols[i].ImageAlign = ImageAlignEnum.LeftBottom;
				g6.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g9.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g12.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;

				MaxColWidth = 0;
			}
			if (!_loading)
			{
				SetHScrollBar1Parameters();
				SetHScrollBar2Parameters();
				SetHScrollBar3Parameters();
			}	
		}
		private void ResizeRows()
		{
			//			g1.AutoSizeRows(); 
			//			g2.AutoSizeRows();
			//			g3.AutoSizeRows();

			g1.AutoSizeRow(0); 
			g2.AutoSizeRow(0);
			g2.AutoSizeRow(1);
			g3.AutoSizeRow(0);
			g3.AutoSizeRow(1);
			
			//Resize Rows on all grids. Line-up data based on the tallest row.
			//But in this particular view, g1 has a different number of rows from g2 & g3.
			//So we'll just do it one by one instead of in a loop like in other views.
			int MaxRowHeight = 0;  //temp variable to hold the currently tallest row.
			
			if (g2.Rows[0].HeightDisplay > MaxRowHeight) 
				MaxRowHeight = g2.Rows[0].HeightDisplay;
		
			if (g3.Rows[0].HeightDisplay > MaxRowHeight) 
				MaxRowHeight = g3.Rows[0].HeightDisplay;
			
			if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
				MaxRowHeight += 5; 

			g2.Rows[0].HeightDisplay = MaxRowHeight;
			g3.Rows[0].HeightDisplay = MaxRowHeight;
			
			MaxRowHeight = 0;  //temp variable to hold the currently tallest row.
			if (g1.Rows[0].HeightDisplay > MaxRowHeight) MaxRowHeight = g1.Rows[0].HeightDisplay;
			if (g2.Rows[1].HeightDisplay > MaxRowHeight) MaxRowHeight = g2.Rows[1].HeightDisplay;
			if (g3.Rows[1].HeightDisplay > MaxRowHeight) MaxRowHeight = g3.Rows[1].HeightDisplay;
			g1.Rows[0].HeightDisplay = MaxRowHeight;
			g2.Rows[1].HeightDisplay = MaxRowHeight;
			g3.Rows[1].HeightDisplay = MaxRowHeight;

			if (_themeChanged) // performance change
				ResizeRows_g4_g12();
			
		}
		private void ResizeRows_g4_g12()
		{
			int i, MaxRowHeight = 0;  
			g4.AutoSizeRows();
			g5.AutoSizeRows(); 
			g6.AutoSizeRows();
			g7.AutoSizeRows(); 
			g8.AutoSizeRows();
			g9.AutoSizeRows(); 
			g10.AutoSizeRows();
			g11.AutoSizeRows(); 
			g12.AutoSizeRows();
			for (i = 0; i < g4.Rows.Count; i++)
			{
				if (g4.Rows[i].HeightDisplay > MaxRowHeight) MaxRowHeight = g4.Rows[i].HeightDisplay;
				if (g5.Rows[i].HeightDisplay > MaxRowHeight) MaxRowHeight = g5.Rows[i].HeightDisplay;
				if (g6.Rows[i].HeightDisplay > MaxRowHeight) MaxRowHeight = g6.Rows[i].HeightDisplay;

				g4.Rows[i].HeightDisplay = MaxRowHeight;
				g5.Rows[i].HeightDisplay = MaxRowHeight;
				g6.Rows[i].HeightDisplay = MaxRowHeight;

				MaxRowHeight = 0;
			}
			MaxRowHeight = 0; //reset the temp variable.
			for (i = 0; i < g7.Rows.Count; i++)
			{
				if (g7.Rows[i].HeightDisplay > MaxRowHeight) MaxRowHeight = g7.Rows[i].HeightDisplay;
				if (g8.Rows[i].HeightDisplay > MaxRowHeight) MaxRowHeight = g8.Rows[i].HeightDisplay;
				if (g9.Rows[i].HeightDisplay > MaxRowHeight) MaxRowHeight = g9.Rows[i].HeightDisplay;

				g7.Rows[i].HeightDisplay = MaxRowHeight;
				g8.Rows[i].HeightDisplay = MaxRowHeight;
				g9.Rows[i].HeightDisplay = MaxRowHeight;

				MaxRowHeight = 0;
			}
			MaxRowHeight = 0; //reset the temp variable.
			for (i = 0; i < g10.Rows.Count; i++)
			{
				if (g10.Rows[i].HeightDisplay > MaxRowHeight) MaxRowHeight = g10.Rows[i].HeightDisplay;
				if (g11.Rows[i].HeightDisplay > MaxRowHeight) MaxRowHeight = g11.Rows[i].HeightDisplay;
				if (g12.Rows[i].HeightDisplay > MaxRowHeight) MaxRowHeight = g12.Rows[i].HeightDisplay;

				g10.Rows[i].HeightDisplay = MaxRowHeight;
				g11.Rows[i].HeightDisplay = MaxRowHeight;
				g12.Rows[i].HeightDisplay = MaxRowHeight;

				MaxRowHeight = 0;
			}
		}
		#region Criteria Changed Events
		public void UpdateData()
		{
			GetCurrentSettings();
			CriteriaChanged();
		}

        // Begin TT#2 - RMatelic - Assortment Planning 
        public void UpdateDataReload()
        {
            int i, j;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                _trans.ResetFirstBuild(true);
                //BEGIN TT#572-MD - stodd - Assortment matrix changes are not being reflected on style review
                //_trans.RefreshSelectedHeaders();
                //END TT#572-MD - stodd - Assortment matrix changes are not being reflected on style review
                // Begin TT#597-MD - RMatelic - Dropped header on placeholder while style review is open. Switch to style review and new header is not there
                _trans.RebuildWafers();
                // End TT#597
                _wafers = _trans.AllocationWafers;
                SetGridRedraws(false);
                FormatGrids1to12();
                _setHeaderTags = true;
                AssignTag();
                ChangeStyle();
                ApplyPreferences();
                SetNeedAnalysisGrid(g4);
                // Begin TT#597-MD - RMatelic - Dropped header on placeholder while style review is open. Switch to style review and new header is not there
                //ApplyViewToGridLayout(Convert.ToInt32(cmbView.SelectedValue, CultureInfo.CurrentUICulture));
                ApplyCurrentColumns(); 
                // End TT#597
                SetGridRedraws(true);
              
                vScrollBar2.Value = vScrollBar2.Minimum;
                // BEGIN MID Track #2747 
                for (i = 0; i < g2.Cols.Count; i++)
                {
                    if (g2.Cols[i].Visible)
                    {
                        break;
                    }
                }
                if (i > hScrollBar2.Maximum)
                {
                    i = hScrollBar2.Minimum;
                }

                for (j = 0; j < g3.Cols.Count; j++)
                {
                    if (g3.Cols[j].Visible)
                    {
                        break;
                    }
                }
                if (j > hScrollBar3.Maximum)
                {
                    j = hScrollBar3.Minimum;
                }

                SetScrollBarPosition(hScrollBar2, i);
                SetScrollBarPosition(hScrollBar3, j);
               
                //ChangePending = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
			finally
			{
				Cursor.Current = Cursors.Default;
			}
        }
        // End TT#2

        // begin MID Track 6079 qty allocated change not accepted after column sort
        private void SortCriteriaChanged()
        {
            _structSort = _structSortSave; 
            _g456DataView.Sort = _sortCriteriaSave;
            _sortCriteriaChanged = true;
            CriteriaChanged();
        }
        // end MID Track 6079 qty allocated change not accepted after column sort
		private void CriteriaChanged()
		{
			try
			{

				Cursor.Current = Cursors.WaitCursor;
                SaveCurrentColumns();                   // TT#358/#334/#363 - RMatelic - Velocity View column display issues
				_wafers = _trans.AllocationWafers;
				SetGridRedraws(false);
				Formatg1Grid();
				Formatg2Grid();
				Formatg3Grid();

                Add456DataRows(_wafers, _g456GridTable, out _g456DataView, g4, g5, g6);
				
				FormatGrid7_10 (FromGrid.g7); 
				FormatGrid8_12 (FromGrid.g8); 
				FormatGrid8_12 (FromGrid.g9); 
				FormatGrid7_10 (FromGrid.g10); 
				FormatGrid8_12 (FromGrid.g11); 
				FormatGrid8_12 (FromGrid.g12); 

				g1HasColsFrozen = false;
				g2HasColsFrozen = false;
				g3HasColsFrozen = false;
				
				g3.DrawMode = DrawModeEnum.OwnerDraw;
				g4.DrawMode = DrawModeEnum.OwnerDraw;
				g5.DrawMode = DrawModeEnum.OwnerDraw;
				g6.DrawMode = DrawModeEnum.OwnerDraw;
				g7.DrawMode = DrawModeEnum.OwnerDraw;
				g8.DrawMode = DrawModeEnum.OwnerDraw;
				g9.DrawMode = DrawModeEnum.OwnerDraw;
				g10.DrawMode = DrawModeEnum.OwnerDraw;
				g11.DrawMode = DrawModeEnum.OwnerDraw;
				g12.DrawMode = DrawModeEnum.OwnerDraw;

                //SortColumns();                            // TT#358/#334/#363 - RMatelic - Velocity View column display issues - comment out line
				_setHeaderTags = false;
				AssignTag();
				ChangeStyle();	
				// BEGIN MID Track #2643 - error closing SKU review
				if (!FormIsClosing)
				{
                    // Begin TT#334 - RMatelic - Velocity Views- After selecting allocate the order of the View changes.
                    //ApplyPreferences();
                    //int viewRID = Convert.ToInt32(cmbView.SelectedValue, CultureInfo.CurrentUICulture);
                    //if (viewRID > 0)
                    //{
                    //    ApplyViewToGridLayout(viewRID);
                    //}
                    //else
                    //{
                    //ApplyPreferences();
                    //}
                    // End TT#334
					UserSetSplitter1Position = VerticalSplitter1.SplitPosition;
					UserSetSplitter2Position = VerticalSplitter2.SplitPosition;
					SetNeedAnalysisGrid(g4);
					// BEGIN MID Track #2747 - columns not lined up
                    //SetScrollBarPosition(hScrollBar2, ((GridTag)g2.Tag).CurrentScrollPosition);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) - correct base bug 
                    //SetScrollBarPosition(hScrollBar3, ((GridTag)g3.Tag).CurrentScrollPosition);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) - correct base bug 
					// END MID Track #2747  

                    ApplyCurrentColumns();      // Begin TT#358/#334/#363 - RMatelic - Velocity View column display issues
                    SortColumns();              // End TT#358/#334/#363 
              
                    SetScrollBarPosition(hScrollBar2, ((GridTag)g2.Tag).CurrentScrollPosition);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) - correct base bug 
                    SetScrollBarPosition(hScrollBar3, ((GridTag)g3.Tag).CurrentScrollPosition);  // TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) - correct base bug 

                    // Begin TT#372 - RMatelic - Velocity Detail Screen Horizontal Scroll not working correctly
                    SetHScrollBar1Parameters();
                    SetHScrollBar2Parameters();
                    SetHScrollBar3Parameters();
                    // End TT#372 

                    SetGridRedraws(true);
				}
				// END MID Track #2643 
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
                // Begin Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set
                _applyPending = false;
                _applyPendingMsgDisplayed = false;
                // End Development TT#8
				Cursor.Current = Cursors.Default;
			}

		}	
		#endregion

		#region various repetitive events
		#region ToolClick Event

        #region Export
        private void Export()
        {
            MIDExportFile MIDExportFile = null;
            try
            {
                _exporting = true;
                MIDExport MIDExport = null;
                MIDExport = new MIDExport(SAB, _includeCurrentSetLabel, _includeAllSetsLabel, true);

                MIDExport.AddFileFilter(eExportFilterType.Excel);
                MIDExport.AddFileFilter(eExportFilterType.CSV);
                MIDExport.AddFileFilter(eExportFilterType.XML);
                MIDExport.AddFileFilter(eExportFilterType.All);

                MIDExport.ShowDialog();
                if (!MIDExport.OKClicked)
                {
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                SetGridRedraws(false);
                string fileName = null;
                if (MIDExport.OpenExcel)
                {
                    // generate unique name
                    fileName = Application.LocalUserAppDataPath + @"\mid" + DateTime.Now.Ticks.ToString();
                    if (MIDExport.IncludeFormatting)
                    {
                        fileName += ".xls";
                    }
                    else
                    {
                        fileName += ".xml";
                    }
                }
                else
                {
                    fileName = MIDExport.FileName;
                }

                switch (MIDExport.ExportType)
                {
                    case eExportType.Excel:
                        if (MIDExport.IncludeFormatting)
                        {
                            MIDExportFile = new MIDExportFlexGridToExcel(fileName, MIDExport.IncludeFormatting);
                        }
                        else
                        {
                            MIDExportFile = new MIDExportFlexGridToXML(fileName, MIDExport.IncludeFormatting);
                        }
                        break;
                    case eExportType.XML:
                        MIDExportFile = new MIDExportFlexGridToXML(fileName, MIDExport.IncludeFormatting);
                        break;
                    default:
                        MIDExportFile = new MIDExportFlexGridToCSV(fileName, MIDExport.IncludeFormatting);
                        break;
                }

                if (MIDExportFile != null)
                {
                    // delete file if it's already there
                    if (File.Exists(MIDExportFile.FileName))
                    {
                        File.Delete(MIDExportFile.FileName);
                    }
                    MIDExportFile.OpenFile();
                    // add certain styles to XML style sheet
                    if (MIDExportFile.ExportType == eExportType.XML)
                    {
                        ExportSpecificStyle(MIDExportFile, "g1", g1, "Style1");
                        ExportSpecificStyle(MIDExportFile, "g2", g2, "GroupHeader");
                        ExportSpecificStyle(MIDExportFile, "g2", g2, "ColumnHeading");
                        ExportSpecificStyle(MIDExportFile, "g4", g4, "Style1");
                        ExportSpecificStyle(MIDExportFile, "g4", g4, "Style1Center");
                        ExportSpecificStyle(MIDExportFile, "g5", g5, "Editable1");
                        ExportSpecificStyle(MIDExportFile, "g5", g5, "Negative1");
                        ExportSpecificStyle(MIDExportFile, "g6", g6, "Editable1");
                        ExportSpecificStyle(MIDExportFile, "g6", g6, "Negative1");
                        ExportSpecificStyle(MIDExportFile, "g7", g7, "Style1");
                        ExportSpecificStyle(MIDExportFile, "g8", g8, "Editable1");
                        ExportSpecificStyle(MIDExportFile, "g8", g8, "Negative1");
                        ExportSpecificStyle(MIDExportFile, "g9", g9, "Editable1");
                        ExportSpecificStyle(MIDExportFile, "g9", g9, "Negative1");
                        ExportSpecificStyle(MIDExportFile, "g10", g10, "Style1");
                        ExportSpecificStyle(MIDExportFile, "g11", g11, "Editable1");
                        ExportSpecificStyle(MIDExportFile, "g11", g11, "Negative1");
                        ExportSpecificStyle(MIDExportFile, "g12", g12, "Editable1");
                        ExportSpecificStyle(MIDExportFile, "g12", g12, "Negative1");
                        MIDExportFile.NoMoreStyles();
                    }
                    if (MIDExport.ExportData == eExportData.Current)
                    {
                        ExportCurrentFile(MIDExportFile);
                    }
                    else
                    {
                        ExportAllFile(MIDExportFile);
                    }
                    if (MIDExport.OpenExcel)
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = "Excel.exe";
                        process.StartInfo.Arguments = @"""" + MIDExportFile.FileName + @"""";
                        process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                        process.Start();
                    }
                }
            }
            catch (IOException IOex)
            {
                MessageBox.Show(IOex.Message);
                return;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                if (MIDExportFile != null)
                {
                    MIDExportFile.WriteFile();
                }
                SetGridRedraws(true);
                Cursor.Current = Cursors.Default;
                _exporting = false;
            }
        }

        private void ExportCurrentFile(MIDExportFile aMIDExportFile)
        {
            try
            {
                int detailsPerGroup = 0;

                StoreGroupLevelListViewProfile sglProf = null;
                sglProf = (StoreGroupLevelListViewProfile)_storeGroupLevelProfileList.FindKey(_trans.AllocationStoreGroupLevel);
                ExportData(aMIDExportFile, sglProf, detailsPerGroup, true, true, false);
            }
            catch
            {
                throw;
            }
        }

        private void ExportAllFile(MIDExportFile aMIDExportFile)
        {
            int saveGroupLevel = _trans.AllocationStoreGroupLevel;
            try
            {
                bool addHeader = true;
                bool addSummary = true;
                int setCount = 0;

                //Begin TT#866 - JSmith - Export of multiple sets has the incorrect data in the store columns
                TurnRedrawOff();
                //End TT#866

                foreach (StoreGroupLevelListViewProfile sglProf in _storeGroupLevelProfileList)
                {
                    ++setCount;
					//Begin TT#866 - JSmith - Export of multiple sets has the incorrect data in the store columns
                    //_trans.AllocationStoreGroupLevel = sglProf.Key;
					//End TT#866
                    if (aMIDExportFile.ExportType == eExportType.CSV)
                    {
                        // only add summary at end of CSV file
                        if (setCount == _storeGroupLevelProfileList.Count)
                        {
                            addSummary = true;
                        }
                        else
                        {
                            addSummary = false;
                        }
                    }
                    ExportData(aMIDExportFile, sglProf, rowsPerStoreGroup,
                        addHeader, addSummary, true);
                    // only add heading at beginning of CSV file
                    if (aMIDExportFile.ExportType == eExportType.CSV)
                    {
                        addHeader = false;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                //Begin TT#866 - JSmith - Export of multiple sets has the incorrect data in the store columns
                //_trans.AllocationStoreGroupLevel = saveGroupLevel;
                cmbAttributeSet.SelectedValue = saveGroupLevel;
                //this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs()); // TT#294-MD - JSmith - When Opening style review, the view does not open that is selected
                SetGridRedraws(true);
                 TurnRedrawOn();
                //End TT#866
            }
        }

        private void ExportData(MIDExportFile aMIDExportFile, StoreGroupLevelListViewProfile sglProf, int aRowsPerGroup,
            bool aAddHeader, bool aAddSummary, bool aExportingAll)
        {
            C1.Win.C1FlexGrid.C1FlexGrid exportG4 = null;
            C1.Win.C1FlexGrid.C1FlexGrid exportG5 = null;
            C1.Win.C1FlexGrid.C1FlexGrid exportG6 = null;
            bool removeControls = false;
            try
            {
                string textStyle = null;
                int i;
                int j;
                int totRows = 0;
                int totCols = 0;
                string groupingName = "sheet1";
                AllocationWaferGroup wafers;
                //Begin TT#866 - JSmith - Export of multiple sets has the incorrect data in the store columns
                //DataView g456DataView = null;
                //DataTable g456GridTable = null;
                //End TT#866

                //Begin TT#866 - JSmith - Export of multiple sets has the incorrect data in the store columns
                //if (aExportingAll &&
                //    sglProf.Name != cmbAttributeSet.Text)  // build new grids
                //{
                //    // create work grids and add them to a control so that they will populate
                //    removeControls = true;
                //    exportG4 = new C1FlexGrid();
                //    exportG4.AllowMerging = AllowMergingEnum.Free;
                //    exportG4.Rows.Fixed = 0;
                //    exportG4.Cols.Fixed = 0;
                //    exportG4.Visible = false;
                //    pnlRowHeaders.Controls.Add(exportG4);
                //    exportG5 = new C1FlexGrid();
                //    exportG5.AllowMerging = AllowMergingEnum.Free;
                //    exportG5.Rows.Fixed = 0;
                //    exportG5.Cols.Fixed = 0;
                //    exportG5.Visible = false;
                //    pnlTotals.Controls.Add(exportG5);
                //    exportG6 = new C1FlexGrid();
                //    exportG6.AllowMerging = AllowMergingEnum.Free;
                //    exportG6.Rows.Fixed = 0;
                //    exportG6.Cols.Fixed = 0;
                //    exportG6.Visible = false;
                //    pnlData.Controls.Add(exportG6);

                //    wafers = _trans.AllocationWafers;
                //    g456GridTable = _g456GridTable.Copy();

                //    Add456DataRows(wafers, g456GridTable, out g456DataView, exportG4, exportG5, exportG6);

                //    ShowHideColHeaders(FromGrid.g1, ColHeaders1, false, exportG4, exportG5, exportG6);
                //    ShowHideColHeaders(FromGrid.g3, ColHeaders2, false, exportG4, exportG5, exportG6);
                //    SetNeedAnalysisGrid(exportG4);
                //}
                //else  // use existing grids
                //{
                //    exportG4 = g4;
                //    exportG5 = g5;
                //    exportG6 = g6;
                //}

                if (aExportingAll)
                {
                    cmbAttributeSet.SelectedValue = sglProf.Key;
                    //this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs()); // TT#294-MD - JSmith - When Opening style review, the view does not open that is selected
                }

                exportG4 = g4;
                exportG5 = g5;
                exportG6 = g6;
                //End TT#866

                groupingName = sglProf.Name;

                wafers = _trans.AllocationWafers;

                if (aAddHeader)
                {
                    totRows += wafers[0, 0].ColumnLabels.GetLength(0);
                }
                for (i = 0; i < wafers.RowCount; i++)
                {
                    totRows += wafers[i, 1].Cells.GetLength(0);
                }
                for (j = 0; j < g1.Cols.Count; j++)
                {
                    if (g1.Cols[j].Visible)
                    {
                        totCols += 1;
                    }
                }
                if (pnlTotals.Width > 0)
                {
                    for (j = 0; j < g2.Cols.Count; j++)
                    {
                        if (g2.Cols[j].Visible)
                        {
                            totCols += 1;
                        }
                    }
                }
                for (j = 0; j < g3.Cols.Count; j++)
                {
                    if (g3.Cols[j].Visible)
                    {
                        totCols += 1;
                    }
                }

                aMIDExportFile.AddGrouping(groupingName, totCols);

                aMIDExportFile.SetNumberRowsColumns(totRows, totCols);

                if (aMIDExportFile.IncludeFormatting &&
                    aMIDExportFile.ExportType == eExportType.Excel)
                {
                    ExportSetStyles(aMIDExportFile);
                }

                if (aAddHeader)
                {
                    // add headings
                    for (i = 0; i < g3.Rows.Count; i++)
                    {
                        aMIDExportFile.AddRow();
                        //  add heading columns
                        //negativeStyle = null;
                        textStyle = "g1Style1";
                        for (j = 0; j < g1.Cols.Count; j++)
                        {
                            if (g1.Cols[j].Visible)
                            {
                                if (i < g3.Rows.Count - 1 ||
                                    g1[0, j] == null)
                                {
                                    aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, null);
                                }
                                else
                                {
                                    aMIDExportFile.AddValue(g1[0, j].ToString(), eExportDataType.ColumnHeading, textStyle, null);
                                }
                            }
                        }

                        // add total columns
                        //negativeStyle = null;
                        if (i == 0)
                        {
                            textStyle = "g2GroupHeader";
                        }
                        else
                        {
                            textStyle = "g2ColumnHeading";
                        }
                        if (pnlTotals.Width > 0)
                        {
                            for (j = 0; j < g2.Cols.Count; j++)
                            {
                                if (g2.Cols[j].Visible)
                                {
                                    if (g2[i, j] != null)
                                    {
                                        aMIDExportFile.AddValue(g2[i, j].ToString(), eExportDataType.ColumnHeading, textStyle, null);
                                    }
                                    else
                                    {
                                        aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, null);
                                    }
                                }
                            }
                        }
                        // add detail columns
                        //negativeStyle = null;
                        if (i == 0)
                        {
                            textStyle = "g2GroupHeader";
                        }
                        else
                        {
                            textStyle = "g2ColumnHeading";
                        }
                        for (j = 0; j < g3.Cols.Count; j++)
                        {
                            if (g3.Cols[j].Visible)
                            {
                                if (g3[i, j] != null)
                                {
                                    aMIDExportFile.AddValue(g3[i, j].ToString(), eExportDataType.ColumnHeading, textStyle, null);
                                }
                                else
                                {
                                    aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, null);
                                }
                            }
                        }
                        aMIDExportFile.WriteRow();
                    }
                }

                // add stores for set
                if (exportG4.Rows.Count > 0)
                {
                    ExportAddWafers(aMIDExportFile, exportG4, exportG5, exportG6, null, aRowsPerGroup,
                        "g4Style1", "g4Style1", "g4Style1", "g5Editable1", "g5Negative1", "g6Editable1", "g6Negative1");
                }

                // add set values
                ExportAddWafers(aMIDExportFile, g7, g8, g9, sglProf.Name, aRowsPerGroup,
                        "g7Style1", "g7Style1", "g7Style1", "g8Editable1", "g8Negative1", "g9Editable1", "g9Negative1");

                if (aAddSummary)
                {
                    // add blank line before summary
                    if (aMIDExportFile.ExportType == eExportType.CSV &&
                        aExportingAll)
                    {
                        aMIDExportFile.AddRow();
                        aMIDExportFile.WriteRow();
                    }

                    // add all stores
                    ExportAddWafers(aMIDExportFile, g10, g11, g12, null, aRowsPerGroup,
                        "g10Style1", "g10Style1", "g10Style1", "g11Editable1", "g11Negative1", "g12Editable1", "g12Negative1");
                }

                aMIDExportFile.WriteGrouping();
            }
            catch
            {
                throw;
            }
            finally
            {
                // remove control temporarily added for export
                if (removeControls)
                {
                    if (exportG4 != null &&
                        pnlRowHeaders.Controls.Contains(exportG4))
                    {
                        pnlRowHeaders.Controls.Remove(exportG4);
                    }
                    if (exportG5 != null &&
                        pnlTotals.Controls.Contains(exportG4))
                    {
                        pnlTotals.Controls.Remove(exportG5);
                    }
                    if (exportG6 != null &&
                        pnlData.Controls.Contains(exportG4))
                    {
                        pnlData.Controls.Remove(exportG6);
                    }
                }
            }
        }

        private void ExportAddWafers(MIDExportFile aMIDExportFile, C1FlexGrid aHeadingGrid,
            C1FlexGrid aTotalGrid, C1FlexGrid aDetailGrid,
            string aSetName, int aRowsPerGroup,
            string aHeadingStyle, string aHeadingEditableStyle, string aHeadingNegativeStyle,
            string aTotalEditableStyle, string aTotalNegativeStyle,
            string aDetailEditableStyle, string aDetailNegativeStyle)
        {
            try
            {
                string negativeStyle = null;
                string textStyle = null;
                int i;
                int j;
                eExportDataType dataType;
                //TagForRow rowTag;
                //AllocationWaferCoordinate waferCoord;

                for (i = 0; i < aHeadingGrid.Rows.Count; i++)
                {
                    if (!aHeadingGrid.Rows[i].Visible)
                    {
                        continue;
                    }

                    // only output row for current set
                    if (aSetName != null)
                    {
                        //rowTag = (TagForRow)aHeadingGrid.Rows[i].UserData;
                        //waferCoord = GetAllocationCoordinate(rowTag.CubeWaferCoorList, eAllocationCoordinateType.StoreAllocationNode);
                        //if (waferCoord.Label != aSetName)
                        if (Convert.ToString(aHeadingGrid.GetData(i, 0), CultureInfo.CurrentUICulture) != aSetName)
                        {
                            continue;
                        }
                    }

                    aMIDExportFile.AddRow();

                    // add heading columns
                    negativeStyle = aHeadingNegativeStyle;
                    textStyle = aHeadingEditableStyle;
                    for (j = 0; j < aHeadingGrid.Cols.Count; j++)
                    {
                        if (j == 0)
                        {
                            dataType = eExportDataType.RowHeading;
                        }
                        else
                        {
                            dataType = eExportDataType.Value;
                        }
                        if (aHeadingGrid.Cols[j].Visible)
                        {
                            if (aHeadingGrid[i, j] != null)
                            {
                                aMIDExportFile.AddValue(aHeadingGrid[i, j].ToString(), dataType, textStyle, negativeStyle);
                            }
                            else
                            {
                                aMIDExportFile.AddValue(" ", dataType, textStyle, null);
                            }
                        }
                    }

                    // add total columns
                    if (pnlTotals.Width > 0)
                    {
                        negativeStyle = aTotalNegativeStyle;
                        textStyle = aTotalEditableStyle;
                        for (j = 0; j < aTotalGrid.Cols.Count; j++)
                        {
                            if (aTotalGrid.Cols[j].Visible)
                            {
                                if (aTotalGrid[i, j] != null)
                                {
                                    aMIDExportFile.AddValue(aTotalGrid[i, j].ToString(), eExportDataType.Value, textStyle, negativeStyle);
                                }
                                else
                                {
                                    aMIDExportFile.AddValue(" ", eExportDataType.Value, textStyle, null);
                                }
                            }
                        }
                    }

                    // add detail columns
                    negativeStyle = aDetailNegativeStyle;
                    textStyle = aDetailEditableStyle;
                    for (j = 0; j < aDetailGrid.Cols.Count; j++)
                    {
                        if (aDetailGrid.Cols[j].Visible)
                        {
                            if (aDetailGrid[i, j] != null)
                            {
                                // Begin TT#322 - RMatelic - Export file when doing Attributes byset -export display rule by number
                                //aMIDExportFile.AddValue(aDetailGrid[i, j].ToString(), eExportDataType.Value, textStyle, negativeStyle);
                              
                                string displayValue = aDetailGrid[i, j].ToString();
                                if (aDetailGrid.Cols[j].DataMap != null && aDetailGrid.Cols[j].DataMap == _ldRulesVelocity)
                                {
                                    uint textCode = Convert.ToUInt32(aDetailGrid[i, j], CultureInfo.CurrentUICulture);
                                    if (_ldRulesVelocity.Contains(textCode))
                                    {
                                        displayValue = _ldRulesVelocity[textCode].ToString();
                                    }
                                }
                                aMIDExportFile.AddValue(displayValue, eExportDataType.Value, textStyle, negativeStyle);
                                // End TT#322
                            }
                            else
                            {
                                aMIDExportFile.AddValue(" ", eExportDataType.Value, textStyle, null);
                            }
                        }
                    }
                    aMIDExportFile.WriteRow();
                }
            }
            catch
            {
                throw;
            }
        }

        private string GetCellStyle(C1FlexGrid aGrid, int aTopRow, int aLeftCol,
            int aBottomRow, int aRightCol)
        {
            try
            {
                CellRange cellRange;
                cellRange = aGrid.GetCellRange(aTopRow, aLeftCol, aBottomRow, aRightCol);
                return cellRange.Style.Name;
            }
            catch
            {
                throw;
            }
        }

        private void ExportSetStyles(MIDExportFile aMIDExportFile)
        {
            try
            {
                ExportAddStyles(aMIDExportFile, "g1", g1);
                ExportAddStyles(aMIDExportFile, "g2", g2);
                ExportAddStyles(aMIDExportFile, "g3", g3);
                ExportAddStyles(aMIDExportFile, "g4", g4);
                ExportAddStyles(aMIDExportFile, "g5", g5);
                ExportAddStyles(aMIDExportFile, "g6", g6);
                ExportAddStyles(aMIDExportFile, "g7", g7);
                ExportAddStyles(aMIDExportFile, "g8", g8);
                ExportAddStyles(aMIDExportFile, "g9", g9);
                ExportAddStyles(aMIDExportFile, "g10", g10);
                ExportAddStyles(aMIDExportFile, "g11", g11);
                ExportAddStyles(aMIDExportFile, "g12", g12);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ExportAddStyles(MIDExportFile aMIDExportFile, string aGridName, C1FlexGrid aGrid)
        {
            try
            {
                foreach (CellStyle cellStyle in aGrid.Styles)
                {
                    try
                    {
                        aMIDExportFile.AddStyle(aGridName + cellStyle.Name, cellStyle);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void ExportSpecificStyle(MIDExportFile aMIDExportFile, string aGridName, C1FlexGrid aGrid,
            string aStyleName)
        {
            try
            {
                foreach (CellStyle cellStyle in aGrid.Styles)
                {
                    try
                    {
                        if (cellStyle.Name == aStyleName)
                        {
                            aMIDExportFile.AddStyle(aGridName + cellStyle.Name, cellStyle);
                            break;
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion Export
		#region Find
		private void Find()
		{
			DialogResult diagResult;
			StoreProfile storeProf;
			ProfileXRef storeSetXRef;
			ArrayList totalList;
			int selectRow;
			int storeCol = 0;
			TagForColumn colTag;
		 			
			try
			{
                ProfileList storeProfileList = StoreMgmt.StoreProfiles_GetActiveStoresList(); //_sab.StoreServerSession.GetActiveStoresList();
				_quickFilterType = eQuickFilterType.Find;
				// BEGIN TT#2703 - stodd - select first comboBox on QuickFilter
				_quickFilter = new QuickFilter(eQuickFilterType.Find, 3, true,  "Store:", "Header:", "Component:");
				// END TT#2703 - stodd - select first comboBox on QuickFilter
				_quickFilter.OnValidateFieldsHandler += new QuickFilter.ValidateFieldsHandler(OnValidateFieldsHandler);
				
				_quickFilter.EnableComboBox(0);
				if (_headerList.Count > 1)
					_quickFilter.EnableComboBox(1);
				else	
					_quickFilter.DisableComboBox(1);
				
				_quickFilter.EnableComboBox(2);
                _quickFilter.LoadComboBox(0, storeProfileList.ArrayList);
                //BEGIN TT#6-MD-VStuart - Single Store Select
                _quickFilter.LoadComboBoxAutoFill(0, storeProfileList.ArrayList);
                //END TT#6-MD-VStuart - Single Store Select
				if (_headerList.Count > 1)
				{
					BuildHeaderList();
					_quickFilter.LoadComboBox(1, _headerArrayList);
				}
				 
				_quickFilter.LoadComboBox(2, _alColHeaders2);

				diagResult = _quickFilter.ShowDialog(this);

				if (diagResult == DialogResult.OK)
				{
					selectRow = 0;
				
					if (_quickFilter.GetSelectedIndex(0) >= 0)
					{
						storeProf = (StoreProfile)_quickFilter.GetSelectedItem(0);
						storeSetXRef = (ProfileXRef)_trans.GetProfileXRef(new ProfileXRef(eProfileType.StoreGroupLevel, eProfileType.Store));
						totalList = storeSetXRef.GetTotalList(storeProf.Key);
						cmbAttributeSet.SelectedValue = totalList[0];
                        //this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs()); // TT#294-MD - JSmith - When Opening style review, the view does not open that is selected
						
						// Need to find the Store column since it could have been moved 
						for (int col = 0; col < g1.Cols.Count - 1; col++)
						{
							colTag = (TagForColumn)g1.Cols[col].UserData;
							if (colTag.cellColumn == 0) // This is store column 
							{
								storeCol = col;
								break;
							}
						}
						selectRow = g4.FindRow(storeProf.Text,0,storeCol,false);
						if (selectRow == -1)
							selectRow = 0;
					}
					else
						selectRow = g4.TopRow;

//					_scroll3EventType = ScrollEventType.SmallIncrement;
//					_scroll2EventType = ScrollEventType.SmallIncrement;
					
					SetScrollBarPosition(hScrollBar3, _foundColumn);;
				
					vScrollBar2.Value = selectRow;
					if ( g6.Rows.Count > 0)
						g6.Select(selectRow, _foundColumn);
				}
				_quickFilter.OnValidateFieldsHandler -= new QuickFilter.ValidateFieldsHandler(OnValidateFieldsHandler);
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		private bool ValidFind()
		{
			bool OKToProcess = true;
			AllocationWaferCoordinate wafercoord;
			object headerID, compLabel;
			TagForColumn colTag;
			string strHeader, strComponent, errorMessage = string.Empty;
			string title = _quickFilter.Text; 
			try
			{
				_foundColumn = 0;
				if (_quickFilter.GetSelectedIndex(1) >= 0)
				{
					headerID = _quickFilter.GetSelectedItem(1);
					if (_quickFilter.GetSelectedIndex(2) >= 0)
					{
						bool columnFound = false;
						compLabel = _quickFilter.GetSelectedItem(2);
						for (int i = 0; i < g3.Cols.Count; i++)
						{
							colTag = (TagForColumn)g3.Cols[i].UserData;
							wafercoord = colTag.CubeWaferCoorList[_compRow];
							strComponent = wafercoord.Label;
							wafercoord = colTag.CubeWaferCoorList[_hdrRow];
							strHeader = wafercoord.Label;
							if (strComponent == Convert.ToString(compLabel, CultureInfo.CurrentUICulture)
								&& strHeader ==	Convert.ToString(headerID, CultureInfo.CurrentUICulture))
							{
								columnFound = true;
								_foundColumn = i; 
								break;
							}
						}
						if (!columnFound)
						{
							OKToProcess = false;
							errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ComponentNotInHeader);
							_quickFilter.SetError(2,errorMessage);
							MessageBox.Show(errorMessage, title);
						}
					}	
					else
					{
						for (int i = 0; i < g3.Cols.Count; i++)
						{
							colTag = (TagForColumn)g3.Cols[i].UserData;
							wafercoord = colTag.CubeWaferCoorList[_hdrRow];
							if (wafercoord.Label == Convert.ToString(headerID, CultureInfo.CurrentUICulture)) 			
							{
								_foundColumn = i; 
								break;
							}
						}
					}
				}
				else if (_quickFilter.GetSelectedIndex(2) >= 0)
				{
					compLabel = _quickFilter.GetSelectedItem(2);
					for (int i = 0; i < g3.Cols.Count; i++)
					{
						colTag = (TagForColumn)g3.Cols[i].UserData;
						wafercoord = colTag.CubeWaferCoorList[_compRow];
						if (wafercoord.Label == Convert.ToString(compLabel, CultureInfo.CurrentUICulture)) 
						{
							_foundColumn = i; 
							break;
						}
					}
				}
			   	 
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
			return OKToProcess;
		}
		#endregion Find
		#region Quick Filter
		private void QuickFilter()
		{
			DialogResult diagResult;
			eFilterComparisonType condition;
			eAllocationWaferVariable wVariable;
			DataRow dRow;
			double qty;
			string name; 
			GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
			bool reloadGrid;
			try
			{
				BuildHeaderSortedList();
				BuildColorList();
				BuildPackList(eComponentType.SpecificPack);
				_quickFilterType = eQuickFilterType.QuickFilter;
				_quickFilter = new QuickFilter(eQuickFilterType.QuickFilter, 1, 1, 4, _lblHeader + ":  ", string.Empty, string.Empty, string.Empty);

				_quickFilter.EnableComboBox(0);
				_quickFilter.SetComboBoxSort(0,false);
				_quickFilter.EnableComboBox(1);
				_quickFilter.SetComboBoxSort(1,false);
				_quickFilter.EnableComboBox(2);
				_quickFilter.SetComboBoxSort(2,false);
				_quickFilter.EnableComboBox(3);
				_quickFilter.SetComboBoxSort(3,false);		 
				
				_quickFilter.LoadComboBox(0, _headerSortedList);
				_quickFilter.SetComboxBoxIndex(0,1);
				if (_headerList.Count < 2)
					_quickFilter.DisableComboBoxLeaveSelected(0);

				BuildTotalComponentList();
				_quickFilter.LoadComboBox(1, _componentDataTable, "Name");
				_quickFilter.LoadComboBoxLabel(1, _lblComponent + ":");
				if (_trans.AnalysisOnly)
				{
					_quickFilter.SetComboxBoxIndex(1,1);
					_quickFilter.DisableComboBoxLeaveSelected(1);
				}
				
				BuildVariableList();
				_quickFilter.LoadComboBox(2, _variableDataTable, "TEXT_VALUE");
				_quickFilter.LoadComboBoxLabel(2, _lblVariable + ":");

				BuildConditionList();
				_quickFilter.LoadComboBox(3, _conditionDataTable, "TEXT_VALUE");
				_quickFilter.LoadComboBoxLabel(3, _lblCondition + ":");

				_quickFilter.EnableTextBox(0, true);
				if (_lblValue == null)
					_lblValue = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Value);	
				_quickFilter.LoadTextBoxLabel(0, _lblValue + ":");
				_quickFilter.LoadCheckBoxText(0, "No Quick Filter");

				_quickFilter.OnComponentSelectedIndexChangeEventHandler += new QuickFilter.ComponentSelectedIndexChangeEventHandler(OnComponentSelectedIndexChangeEventHandler);
				_quickFilter.OnValidateFieldsHandler += new QuickFilter.ValidateFieldsHandler(OnValidateFieldsHandler);
				_quickFilter.OnCheckBoxCheckChangedEventHandler += new QuickFilter.CheckBoxCheckChangedEventHandler(OnCheckBoxCheckChangedEventHandler);
				
				if (_quickFilterData.CheckBoxChecked == null)
				{
					_quickFilterData.CheckBoxChecked = new bool[1];
					_quickFilterData.SelectedIndex = new int[4];
					_quickFilterData.TextBoxText = new string[1];
					_quickFilter.SetCheckBox(0, true);
				}
				else
				{
					_quickFilter.SetCheckBox(0, _quickFilterData.CheckBoxChecked[0]);
					if (!_quickFilterData.CheckBoxChecked[0])
					{
						for (int i = 0; i < 4; i++)
						{
							_quickFilter.SetComboxBoxIndex(i,_quickFilterData.SelectedIndex[i]);
						}
						for (int i = 0; i < 1; i++)
						{
							_quickFilter.SetTextBoxText(i,_quickFilterData.TextBoxText[i]);
						}
					}
				}

				diagResult = _quickFilter.ShowDialog(this);
				
				if (diagResult == DialogResult.OK)
				{
					_quickFilterData.CheckBoxChecked.SetValue(_quickFilter.GetCheckBox(0),0);
				
					if (_quickFilterData.CheckBoxChecked[0])
					{
						_trans.ClearAllocationQuickFilter();
						reloadGrid = true;
					}
					else
					{
						for (int i = 0; i < 4; i++)
						{
							//The method subtracts 1 from the index because of the added "None", so add it back 
							_quickFilterData.SelectedIndex[i] = _quickFilter.GetSelectedIndex(i) + 1;
						}
						for (int i = 0; i < 1; i++)
						{
							_quickFilterData.TextBoxText[i] = _quickFilter.GetTextBoxText(i);
						}
						name = Convert.ToString(_quickFilter.GetSelectedItem(0),CultureInfo.CurrentUICulture);
					
						dRow = _componentDataTable.Rows[_quickFilter.GetSelectedIndex(1)];	
						if ((int)dRow["ComponentType"] == (int)eComponentType.Total)
							aComponent = new GeneralComponent(eGeneralComponentType.Total);
						else if ((int)dRow["ComponentType"] == (int)eComponentType.SpecificPack)
						{
							string packName  = Convert.ToString(dRow["Name"], CultureInfo.CurrentUICulture);
							aComponent = aComponent = new AllocationPackComponent(packName);
						}
						else if ((int)dRow["ComponentType"] == (int)eComponentType.Bulk)
						{
							aComponent = new GeneralComponent(eGeneralComponentType.Bulk);
						}
						else if ((int)dRow["ComponentType"] == (int)eComponentType.SpecificColor)
						{
							int colorRID = Convert.ToInt32(dRow["Key"], CultureInfo.CurrentUICulture);
							aComponent = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, colorRID);
						}	

						dRow = _variableDataTable.Rows[_quickFilter.GetSelectedIndex(2)];	
						wVariable = (eAllocationWaferVariable)dRow["TEXT_CODE"];
					
						dRow = _conditionDataTable.Rows[_quickFilter.GetSelectedIndex(3)];	
						condition = (eFilterComparisonType)dRow["TEXT_CODE"];

						qty = Convert.ToDouble(_quickFilter.GetTextBoxText(0).Trim(),CultureInfo.CurrentUICulture);
						if (name == _totalVariableName)
							reloadGrid = _trans.ApplyAllocationQuickFilter(string.Empty, aComponent, wVariable, qty, condition); 
						else
						{
							int headerRID = (int)_headerSortedList.GetKey(_quickFilter.GetSelectedIndex(0));
							reloadGrid = _trans.ApplyAllocationQuickFilter(headerRID, aComponent, wVariable, qty, condition); 
						}
					}
					if (reloadGrid)
						ReloadGridData();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}
	
		private void BuildHeaderList()
		{
			try
			{
				if (_lblHeader == null)
					_lblHeader = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Header);	
				
				if (_headerArrayList == null)
				{
					_headerArrayList = new ArrayList();
					_totalVariableName = MIDText.GetTextOnly((int)eAllocationWaferVariable.Total);
					if (_headerList.Count > 1)
						_headerArrayList.Add(_totalVariableName);
					foreach (AllocationHeaderProfile ahp in _headerList)
					{	
						_headerArrayList.Add(ahp.HeaderID);
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}	

		private void BuildHeaderSortedList()
		{
			try
			{
				if (_lblHeader == null)
					_lblHeader = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Header);	
				
				if (_headerSortedList == null)
				{
					_headerSortedList = new SortedList();
					_totalVariableName = MIDText.GetTextOnly((int)eAllocationWaferVariable.Total);
					if (_headerList.Count > 1)
						_headerSortedList.Add(0, _totalVariableName);
					foreach (AllocationHeaderProfile ahp in _headerList)
					{	
						if (ahp.Key == Include.DefaultHeaderRID)
							ahp.HeaderID = string.Empty;
						_headerSortedList.Add(ahp.Key, ahp.HeaderID);
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}	
		private void BuildColorList()
		{
			Header header;
			bool colorFound;
			try
			{
				if (_lblColor == null)
					_lblColor = MIDText.GetTextOnly((int)eQuickFilterSelectionType.Color);	
				if (_colorDataTable == null)
				{
                    _colorDataTable = MIDEnvironment.CreateDataTable();				
					_colorDataTable.Columns.Add("ColorRID");
					_colorDataTable.Columns.Add("ColorName");
					
					foreach (AllocationHeaderProfile ahp in _headerList)
					{	
						header = new Header();
						DataTable dtBulkColors = header.GetBulkColors(ahp.Key);
						if (dtBulkColors.Rows.Count > 0)
						{	
							foreach (DataRow cRow in dtBulkColors.Rows)
							{
								int colorKey = Convert.ToInt32(cRow["COLOR_CODE_RID"],CultureInfo.CurrentUICulture);
								colorFound = false;
								foreach (DataRow  cdtRow in _colorDataTable.Rows)
								{
									if (Convert.ToInt32(cdtRow["ColorRID"],CultureInfo.CurrentUICulture)== colorKey)
									{
										colorFound = true;
										break;
									}
								}
								if (!colorFound)
								{
									ColorCodeProfile ccp = _sab.HierarchyServerSession.GetColorCodeProfile(colorKey);
									_colorDataTable.Rows.Add( new object[] {colorKey, ccp.ColorCodeName} ) ;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}	
		private void BuildTotalComponentList()
		{
			eComponentType compType;
			TagForColumn colTag;
			AllocationWaferCoordinate waferCoord;
			DataColumn dataCol;
			try
			{
				if (_lblComponent == null)
					_lblComponent = MIDText.GetTextOnly((int)eQuickFilterSelectionType.Component);	

				if (_componentDataTable == null)
				{
                    _componentDataTable = MIDEnvironment.CreateDataTable();
					dataCol = new DataColumn();
					dataCol.ColumnName = "CoordType";
					dataCol.DataType = System.Type.GetType("System.Int32");
					_componentDataTable.Columns.Add(dataCol);

					dataCol = new DataColumn();
					dataCol.ColumnName = "ComponentType";
					dataCol.DataType = System.Type.GetType("System.Int32");
					_componentDataTable.Columns.Add(dataCol);

					dataCol = new DataColumn();
					dataCol.ColumnName = "Key";
					dataCol.DataType = System.Type.GetType("System.Int32");
					_componentDataTable.Columns.Add(dataCol);

					dataCol = new DataColumn();
					dataCol.ColumnName = "Name";
					dataCol.DataType = System.Type.GetType("System.String");
					_componentDataTable.Columns.Add(dataCol);
 
					for (int i = 0; i < g2.Cols.Count; i++)
					{
						colTag = (TagForColumn)g2.Cols[i].UserData;
						waferCoord = colTag.CubeWaferCoorList[_compRow];
						switch (waferCoord.CoordinateType)
						{
							case eAllocationCoordinateType.Component:
								compType = (eComponentType)waferCoord.CoordinateSubType;	
							switch (compType)
							{
								case eComponentType.Total:
									CheckToAddTotal(waferCoord);
									break;
	
								case eComponentType.Bulk:
									CheckToAddBulk(waferCoord);
									break;

								case eComponentType.SpecificColor:
									CheckToAddColor(waferCoord);
									break;
							}
								break;

							case eAllocationCoordinateType.PackName:
								CheckToAddPack(waferCoord);
								break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}

		private void BuildHeaderComponentList(string aHeaderID)
		{
			eComponentType compType;
			TagForColumn colTag;
			AllocationWaferCoordinate waferCoord;
			bool headerStarted = false;
			try
			{
				for (int i = 0; i < g3.Cols.Count; i++)
				{
					colTag = (TagForColumn)g3.Cols[i].UserData;
					waferCoord = colTag.CubeWaferCoorList[_hdrRow];
					if (waferCoord.Label == aHeaderID)
					{
						headerStarted = true;
						waferCoord = colTag.CubeWaferCoorList[_compRow];
						switch (waferCoord.CoordinateType)
						{
							case eAllocationCoordinateType.Component:
								compType = (eComponentType)waferCoord.CoordinateSubType;	
							switch (compType)
							{
								case eComponentType.Total:
									CheckToAddTotal(waferCoord);
									break;

								case eComponentType.Bulk:
									CheckToAddBulk(waferCoord);
									break;

								case eComponentType.SpecificColor:
									CheckToAddColor(waferCoord);
									break;
							}
								break;

							case eAllocationCoordinateType.PackName:
								CheckToAddPack(waferCoord);
								break;
						}
					}
					else if (headerStarted && rbHeader.Checked)
					{
						break;
					}
				}
				 
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}

		private void CheckToAddTotal (AllocationWaferCoordinate aWaferCoord)
		{
				bool rowFound = false;
			try
			{
				foreach (DataRow row in _componentDataTable.Rows)
				{
					if ((int)row["ComponentType"] == (int)eComponentType.Total)
					{
						rowFound = true;
						break;
					}
				}
				if (!rowFound)
					AddComponentDataRow(aWaferCoord);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}	
	
		private void CheckToAddBulk (AllocationWaferCoordinate aWaferCoord)
		{
			bool rowFound = false;
			try
			{
				foreach (DataRow row in _componentDataTable.Rows)
				{
					if ((int)row["ComponentType"] == (int)eComponentType.Bulk)
					{
						rowFound = true;
						break;
					}
				}
				if (!rowFound)
					AddComponentDataRow(aWaferCoord);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}	

		private void CheckToAddPack (AllocationWaferCoordinate aWaferCoord)
		{
			bool rowFound = false;
			try
			{
				foreach (DataRow row in _componentDataTable.Rows)
				{
					if ((int)row["CoordType"] == (int)eAllocationCoordinateType.PackName)
					{
						if (Convert.ToString(row["Name"],CultureInfo.CurrentUICulture)  == aWaferCoord.SubtotalPackName)
						{
							rowFound = true;
							break;
						}
					}
				}
				if (!rowFound)
					AddComponentDataRow(aWaferCoord);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}	

		private void CheckToAddColor (AllocationWaferCoordinate aWaferCoord)
		{
			bool rowFound = false;
			try
			{
				foreach (DataRow row in _componentDataTable.Rows)
				{
					if ((int)row["ComponentType"] == (int)eComponentType.SpecificColor)
					{
						if ((int)row["Key"] == aWaferCoord.Key)
						{
							rowFound = true;
							break;
						}
					}
				}
				if (!rowFound)
					AddComponentDataRow(aWaferCoord);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}	 

		private void AddComponentDataRow (AllocationWaferCoordinate aWaferCoord)
		{
			try
			{
				DataRow dRow = _componentDataTable.NewRow();
				dRow["CoordType"] = (int)(eAllocationCoordinateType)aWaferCoord.CoordinateType;
				dRow["ComponentType"] = aWaferCoord.CoordinateSubType;
				dRow["Key"] = aWaferCoord.Key;
				if (aWaferCoord.CoordinateType == eAllocationCoordinateType.PackName)
					dRow["Name"] = aWaferCoord.SubtotalPackName;
				else if (aWaferCoord.CoordinateSubType == (int)eComponentType.Total) 
					dRow["Name"] = MIDText.GetTextOnly((int)eComponentType.Total);
				else
					dRow["Name"] = aWaferCoord.Label;

				_componentDataTable.Rows.Add(dRow);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}	
		private void BuildSpecificComponentList(eComponentType aCompType)
		{
			try
			{
				switch(aCompType)
				{
					case eComponentType.SpecificColor:
						BuildColorList();
						_quickFilter.EnableComboBox(2);
						_quickFilter.LoadComboBox(2, _colorDataTable, "ColorName");
						_quickFilter.LoadComboBoxLabel(2, _lblColor + ":");
						break;

					case eComponentType.DetailType:
					case eComponentType.GenericType:
					case eComponentType.SpecificPack:
						BuildPackList(aCompType);
						_quickFilter.EnableComboBox(2);
						_quickFilter.LoadComboBox(2, _packDataTable.DefaultView, "PackNameMultiple");
						_quickFilter.LoadComboBoxLabel(2, _lblPack + ":");
						break;
					
					default:
						_quickFilter.DisableComboBox(2);
						_quickFilter.LoadComboBoxLabel(2, string.Empty);
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}	
		private void BuildPackList(eComponentType aPackType)
		{
			Header header;
			string filterString, packNameMultiple;
			bool packFound;
			try
			{
				if (_lblPack == null)
					_lblPack = "Pack";
				if (_packDataTable == null) 
				{
                    _packDataTable = MIDEnvironment.CreateDataTable();			
					_packDataTable.Columns.Add("PackRID");
					_packDataTable.Columns.Add("PackNameMultiple");
					_packDataTable.Columns.Add("GenericInd");

					foreach (AllocationHeaderProfile ahp in _headerList)
					{	
						header = new Header();
						DataTable dtPacks = header.GetPacks(ahp.Key);
						if (dtPacks.Rows.Count > 0)
						{	
							foreach (DataRow pRow in dtPacks.Rows)
							{
								packNameMultiple = Convert.ToString(pRow["HDR_PACK_NAME"],CultureInfo.CurrentUICulture) 
									+ "(" + Convert.ToString(pRow["MULTIPLE"],CultureInfo.CurrentUICulture) 
									+ ")";
								if (Convert.ToChar(pRow["GENERIC_IND"]) == '0')
								{
									packNameMultiple += "+";
								}
								packFound = false;
								foreach (DataRow  pdtRow in _packDataTable.Rows)
								{
									if (Convert.ToString(pdtRow["PackNameMultiple"],CultureInfo.CurrentUICulture) == packNameMultiple)
									{
										packFound = true;
										break;
									}
								}
								if (!packFound)
									_packDataTable.Rows.Add( new object[] { (int) pRow["HDR_PACK_RID"], packNameMultiple, pRow["GENERIC_IND"]} ) ;
							}
						}
					}
				}

				switch (aPackType)
				{
					case eComponentType.GenericType:
						filterString = "GenericInd = '1'";
						break;

					case eComponentType.DetailType:
						filterString = "GenericInd = '0'";
						break;
			
					default:
						filterString = string.Empty;
						break;
				}	
				_packDataTable.DefaultView.RowFilter = filterString;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}	
	
		private void BuildConditionList()
		{
			try
			{
				if (_lblCondition == null)
					_lblCondition = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Condition);	
				
				if (_conditionDataTable == null)
					_conditionDataTable = MIDText.GetLabels((int)eFilterComparisonType.Equal, (int)eFilterComparisonType.NotGreaterEqual);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}
		private void BuildVariableList()
		{
			try
			{
				if (_lblVariable == null)
					_lblVariable = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Variable);	
			
				if (_variableDataTable == null)
				{
					if (_trans.AnalysisOnly)
						_variableDataTable = MIDText.GetLabels((int)eAllocationQuickFilterVariable.Need, (int)eAllocationQuickFilterVariable.PercentNeed);
					else	
						_variableDataTable = MIDText.GetLabels((int)eAllocationQuickFilterVariable.QuantityAllocated, (int)eAllocationQuickFilterVariable.PercentNeed);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}
		private void OnComponentSelectedIndexChangeEventHandler(object source, ComponentSelectedIndexChangeEventArgs e)
		{
			string headerID; 
			switch (e.ComponentIdx)
			{
				case 0:
					if (e.SelectedIdx == 0)
					{
						_componentDataTable = null;
						BuildTotalComponentList();
					}
					else if (e.SelectedIdx > 0)
					{
						_componentDataTable.Clear();
						headerID = Convert.ToString(_quickFilter.GetSelectedItem(0), CultureInfo.CurrentUICulture);
						BuildHeaderComponentList(headerID);
					}
					_quickFilter.LoadComboBox(1, _componentDataTable, "Name");
					break;
			}
		}	

		private void OnCheckBoxCheckChangedEventHandler(object source, CheckChangedEventArgs e)
		{
			switch (e.ComponentIdx)
			{
				case 0:
					
					for (int i = 0; i < 4; i++)
					{
						if (e.Checked)
						{
							if (i == 0)
							{
								_quickFilter.SetComboxBoxIndex(0,1);
								_quickFilter.DisableComboBoxLeaveSelected(i);
							}
							else
							{	
								_quickFilter.DisableComboBox(i);
							}
							_quickFilter.SetError(i,string.Empty);
						}
						else
						{
							if (i == 0)
							{
								if (_headerList.Count == 1)
									_quickFilter.DisableComboBoxLeaveSelected(0);
								else
									_quickFilter.EnableComboBox(i);
								
							}
							else if ( i == 1)
							{
								if (_trans.AnalysisOnly)
								{
									_quickFilter.SetComboxBoxIndex(1,1);
									_quickFilter.DisableComboBoxLeaveSelected(1);
								}
								else
									_quickFilter.EnableComboBox(i);
							}
							else
								_quickFilter.EnableComboBox(i);
						}
					}
					if (e.Checked)
					{
						_quickFilter.SetErrorTextBox(0,string.Empty);
						_quickFilter.SetTextBoxText(0,string.Empty);
						_quickFilter.EnableTextBox(0,false);
					}
					else
					{
						_quickFilter.EnableTextBox(0,true);
					}
					break;
			}
		}	

		private bool OnValidateFieldsHandler(object source)
		{
			bool OKToProcess = true;
			try
			{
				switch (_quickFilterType)
				{
					case eQuickFilterType.QuickFilter:
						if (!ValidQuickFilter())
							OKToProcess = false;
						break;
						 
					case eQuickFilterType.Find:
						if (!ValidFind())
							OKToProcess = false;
						break;
				}
			}
			catch (Exception ex)
			{
				OKToProcess = false;
				HandleException(ex);
			} 
			return OKToProcess;
		}
		private bool ValidQuickFilter()
		{
			bool OKToProcess = true;
			string errorMessage = string.Empty, generalMessage; 
			string title = _quickFilter.Text; 
			try
			{
				if (!_quickFilter.GetCheckBox(0))
				{	
					for (int i=0; i < 4; i++)
					{
						if (_quickFilter.GetSelectedIndex(i) < 0)
						{
							OKToProcess = false;
							if (errorMessage == string.Empty) 
								errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
							_quickFilter.SetError(i,errorMessage);
						}
						else
							_quickFilter.SetError(i,string.Empty);
					}

					for (int i=0; i < 1; i++)
					{
						if (_quickFilter.GetTextBoxText(i).Trim().Length == 0)
						{
							OKToProcess = false;
							if (errorMessage == string.Empty) 
								errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
							
							_quickFilter.SetErrorTextBox(i,errorMessage);
						}
						else if (!ValidQuickFilterText())
							OKToProcess = false;
					}
			 
					if (!OKToProcess)
					{	
						generalMessage = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
						MessageBox.Show(generalMessage, title);
					}
				}
			}
			catch (Exception ex)
			{
				OKToProcess = false;
				HandleException(ex);
			} 
			return OKToProcess;
		}

		private bool ValidQuickFilterText()
		{
			bool OKToProcess = true;
			string errorMessage;
			double dblQty;
			eAllocationQuickFilterVariable qfVariable;
			AllocationWaferVariable varProf;
			try
			{
				if (_quickFilter.GetTextBoxText(0).Trim().Length > 0)
				{
					_quickFilter.SetErrorTextBox(0,string.Empty);
					try
					{
						dblQty = Convert.ToDouble(_quickFilter.GetTextBoxText(0).Trim(), CultureInfo.CurrentUICulture);
						DataRow dRow = _variableDataTable.Rows[_quickFilter.GetSelectedIndex(2)];	
						qfVariable = (eAllocationQuickFilterVariable)dRow["TEXT_CODE"];
						varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)qfVariable);
						switch (varProf.Format)
						{
							case eAllocationWaferVariableFormat.String:
							case eAllocationWaferVariableFormat.None:
								 
								break;

							case eAllocationWaferVariableFormat.Number:
								
								dblQty = Math.Round(dblQty,varProf.NumDecimals);
								_quickFilter.SetTextBoxText(0,dblQty.ToString(CultureInfo.CurrentUICulture));
								if (   qfVariable == eAllocationQuickFilterVariable.QuantityAllocated 
									&& dblQty < 0)
								{
									errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_QtyAllocatedCannotBeNeg); 
									_quickFilter.SetErrorTextBox(0,errorMessage);
									OKToProcess = false;	
								}	
								break;
				
							default:
								
								break;
						}
					}
					catch
					{
						errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
						_quickFilter.SetErrorTextBox(0,errorMessage);
						OKToProcess = false;
					}
				}
			}
			catch (Exception ex)
			{
				OKToProcess = false;
				HandleException(ex);
			} 
			return OKToProcess;	
		}
		#endregion Quick Filter
       
        #region Theme
        // BEGIN MID Track #5006 - Add Theme to Tools menu  
        private void Theme()
        {
            try
            {
                _frmThemeProperties = new ThemeProperties(_theme);
                _frmThemeProperties.ApplyButtonClicked += new EventHandler(StylePropertiesOnChanged);
                _frmThemeProperties.StartPosition = FormStartPosition.CenterParent;

                if (_frmThemeProperties.ShowDialog() == DialogResult.OK)
                {
                    StylePropertiesChanged();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
        // END MID Track #5006
        #endregion
        
        #endregion

        #region Splitters Move Events
        private void s1_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			try
			{
				// If horizontal splitter has moved, reset the other splitter positions
				if (!_hSplitMove)
				{
					_hSplitMove = true;
					if (s1.SplitPosition < g1.Rows[g1.Rows.Count - 1].Bottom + pnlCorner.Height)
					{
						s1.SplitPosition = g1.Rows[g1.Rows.Count - 1].Bottom + s1.Height;
					}
					s2.SplitPosition = s1.SplitPosition + pnlCorner.Height;
					s3.SplitPosition = s1.SplitPosition + pnlCorner.Height;
					s4.SplitPosition = s1.SplitPosition + pnlCorner.Height;
					_hSplitMove = false;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void s2_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			try
			{
				// If horizontal splitter has moved, reset the other splitter positions
				if (!_hSplitMove)
				{
					_hSplitMove = true;

					if (s2.SplitPosition < g3.Rows[g3.Rows.Count - 1].Bottom + s2.Height)
					{
						s2.SplitPosition = g3.Rows[g3.Rows.Count - 1].Bottom + s4.Height;
					}
					s1.SplitPosition = s2.SplitPosition - pnlCorner.Height;
					s3.SplitPosition = s2.SplitPosition;
					s4.SplitPosition = s2.SplitPosition;
					_hSplitMove = false;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void s3_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			try
			{
				// If horizontal splitter has moved, reset the other splitter positions
				if (!_hSplitMove)
				{
					_hSplitMove = true;
					if (s3.SplitPosition < g3.Rows[g3.Rows.Count - 1].Bottom + s3.Height)
					{
						s3.SplitPosition = g3.Rows[g3.Rows.Count - 1].Bottom + s4.Height;
					}
					s1.SplitPosition = s3.SplitPosition - pnlCorner.Height;
					s2.SplitPosition = s3.SplitPosition;
					s4.SplitPosition = s3.SplitPosition;
					_hSplitMove = false;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}			
		}

		private void s4_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{	
			try
			{
				// If horizontal splitter has moved, reset the other splitter positions
				if (!_hSplitMove)
				{
					_hSplitMove = true;
					if (s4.SplitPosition < g3.Rows[g3.Rows.Count - 1].Bottom + s4.Height + 3)
					{
						SetRowSplitPosition4();
					}
					s1.SplitPosition = s4.SplitPosition - pnlCorner.Height;
					s2.SplitPosition = s4.SplitPosition;
					s3.SplitPosition = s4.SplitPosition;
					_hSplitMove = false;
				}
				SetVScrollBar2Parameters();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
	
		private void g7SplitterMoved(object sender, SplitterEventArgs e)
		{
			Splitter splitter;

			try
			{
				if (!_hSplitMove)
				{
					_hSplitMove = true;
					splitter = (Splitter)sender;
					s5.SplitPosition = splitter.SplitPosition;
					s6.SplitPosition = splitter.SplitPosition;
					s7.SplitPosition = splitter.SplitPosition;
					s8.SplitPosition = splitter.SplitPosition;
					SetVScrollBar2Parameters();
					SetVScrollBar3Parameters();
					_hSplitMove = false;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void g10SplitterMoved(object sender, SplitterEventArgs e)
		{
			Splitter splitter;

			try
			{
				if (!_hSplitMove)
				{
					_hSplitMove = true;
					splitter = (Splitter)sender;
					s9.SplitPosition = splitter.SplitPosition;
					s10.SplitPosition = splitter.SplitPosition;
					s11.SplitPosition = splitter.SplitPosition;
					s12.SplitPosition = splitter.SplitPosition;
					SetVScrollBar3Parameters();
					SetVScrollBar4Parameters();
					_hSplitMove = false;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		
		private void VerticalSplitter1_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			try
			{
				if (!_loading)
				{
					SetHScrollBar1Parameters();
					SetHScrollBar2Parameters();
					SetHScrollBar3Parameters();
				}
				UserSetSplitter1Position = VerticalSplitter1.SplitPosition;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void VerticalSplitter1_DoubleClick(object sender, System.EventArgs e)
		{
			try
			{
				SetV1SplitPosition();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void VerticalSplitter2_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			try
			{
				if (!_loading)
				{
					SetHScrollBar2Parameters();
					SetHScrollBar3Parameters();
				}
				UserSetSplitter2Position = VerticalSplitter2.SplitPosition;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}	
		private void VerticalSplitter2_DoubleClick(object sender, System.EventArgs e)
		{
			try
			{
				SetV2SplitPosition();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void g4SplitterDoubleClick(object sender, System.EventArgs e)
		{
			try
			{
				SetRowSplitPosition4();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void g7SplitterDoubleClick(object sender, System.EventArgs e)
		{
			try
			{
				SetRowSplitPosition8();
				SetRowSplitPosition12();
				_changeRowStyle = false;
				HilightSelectedSet();
				_changeRowStyle = true;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void g10SplitterDoubleClick(object sender, System.EventArgs e)
		{
			try
			{
				SetRowSplitPosition12();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetRowSplitPosition4()
		{
			try
			{
				if (!FormIsClosing)
					s4.SplitPosition = g3.Rows[g3.Rows.Count - 1].Bottom + s4.Height + 3;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetRowSplitPosition8()
		{
			try
			{
				// BEGIN MID Track #2643 - error closing SKU review
				if (!FormIsClosing)
					s8.SplitPosition = g7.Rows[0].Bottom + s8.Height + 3;
				// END MID Track #2643  
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetRowSplitPosition12()
		{
			try
			{
				// BEGIN MID Track #2643 - error closing SKU review
				if (!FormIsClosing)
					s12.SplitPosition = g10.Rows[g10.Rows.Count - 1].Bottom + s12.Height + 0 ;
				// END MID Track #2643 
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetV1SplitPosition()
		{
			TagForColumn colTag;
			int lastDisplayedCol = 0;
			try
			{
				if (_showNeedGrid)
				{
					//if (_loading)
					//	VerticalSplitter1.SplitPosition = g1.Cols[g1.Cols.Count - 2].Right + VerticalSplitter1.Width + 3;
					//else	
					//{
						for (int i = 0; i < g1.Cols.Count; i++)
						{
							colTag = (TagForColumn)g1.Cols[i].UserData;
							if (colTag.IsDisplayed)
								lastDisplayedCol = i;
						}
						VerticalSplitter1.SplitPosition = g1.Cols[lastDisplayedCol].Right + VerticalSplitter1.Width + 3;
					//}
				}
				else
				{
					if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)	
					{
						int btnPos =  btnAllocate.Right;
						int colPos = 0;
						for (int j = 0; j < g1.Cols.Count; j++)
						{
							if (g1.Cols[j].Visible)
							{
								colPos += g1.Cols[j].Width;
								if (colPos >= btnPos) 
									break;
							}	
						}
						if (colPos < btnPos) 
							colPos = btnPos;
						VerticalSplitter1.SplitPosition = colPos  + VerticalSplitter1.Width + 3;
					}
					else
						VerticalSplitter1.SplitPosition = g1.Cols[0].Right + VerticalSplitter1.Width + 3;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetV2SplitPosition()
		{
			int i, col = 0, counter = 0;
			try
			{
				// Show 1st 2 coumns 
				if (HeaderOrComponentGroups > 1)
				{
					for (i = 0; i < g2.Cols.Count; i++)
					{
						if (g2.Cols[i].Visible)
						{
							col = i;
							counter++;
							if (counter == 2)
								break;
						}
					}
					VerticalSplitter2.SplitPosition = g2.Cols[col].Right + VerticalSplitter2.Width + 3;
				}
				else // Don't show the middle Total grid when only 1 header
				{
					hScrollBar2.Visible = false;
					VerticalSplitter2.SplitPosition = 0;
					VerticalSplitter2.Visible = false;
				}

			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#endregion
		#region ScrollBars Scroll Events

		private void SetVScrollBar2Parameters()
		{
			try
			{
				rowsPerGroup4 = 1;
				vScrollBar2.Minimum = 0;
				//vScrollBar2.Maximum = CalculateRowMaximumScroll(g4, rowsPerGroup4);

                // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                //vScrollBar2.Maximum = g4.Rows.Count;
                // BEGIN TT#1760 - AGallagher - Error Requesting Style Review
                //vScrollBar2.Maximum = g4.Rows.Count - 1;
                if (g4.Rows.Count > 0)
                {
                    vScrollBar2.Maximum = g4.Rows.Count - 1;
                }
                else
                {
                    vScrollBar2.Maximum = 0;
                }
                // vScrollBar2.Maximum = g4.Rows.Count > 0 ? g4.Rows.Count – 1 : 0;
                // END TT#1760 - AGallagher - Error Requesting Style Review
                // End TT#1542

				vScrollBar2.SmallChange = SMALLCHANGE;
				//vScrollBar2.LargeChange = BIGCHANGE;
				if (g4.Rows.Count > 0)
				{
					vScrollBar2.LargeChange = ( g4.BottomRow - g4.TopRow ) + 1;
				}
				else
					vScrollBar2.LargeChange = 0;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetVScrollBar3Parameters()
		{
//			int maxScroll;
			try
			{
				rowsPerGroup7 = 1;
				//maxScroll = CalculateRowMaximumScroll(g7, rowsPerGroup7);
				vScrollBar3.Minimum = 0;
				//vScrollBar3.Maximum = maxScroll + BIGCHANGE - 1;
				vScrollBar3.Maximum = g7.Rows.Count;
				vScrollBar3.SmallChange = SMALLCHANGE;
				//vScrollBar3.LargeChange = BIGCHANGE;
				if (g7.Rows.Count > 0)
				{
					vScrollBar3.LargeChange = ( g7.BottomRow - g7.TopRow + 1);
				}
				else
					vScrollBar3.LargeChange = 0;
				//				dispRows = ( g7.BottomRow - g7.TopRow );
				//				g7.TopRow = ( foundrow / dispRows ) * dispRows;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetVScrollBar4Parameters()
		{
			try
			{
				rowsPerGroup10 = 1;
				vScrollBar4.Minimum = 0;
				vScrollBar4.Maximum = CalculateRowMaximumScroll(g10, rowsPerGroup10);
				vScrollBar4.SmallChange = SMALLCHANGE;
				vScrollBar4.LargeChange = BIGCHANGE;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void vScrollBar2_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				_isScrolling = true;
                //_vScrollBar2Moved = true;
                _vScrollBar2Moved = true;   // TT#1073 - RMatelic - GA style review use the scroll bar to the right and the rows are out of sync
				g4.TopRow = ((VScrollBar)sender).Value * rowsPerGroup4;
				g5.TopRow = ((VScrollBar)sender).Value * rowsPerGroup4;
				g6.TopRow = ((VScrollBar)sender).Value * rowsPerGroup4;
				SetVScrollBar2Parameters();
				_isScrolling = false;
                //_vScrollBar2Moved = false;
                _vScrollBar2Moved = false;  // TT#1073 - RMatelic - GA style review use the scroll bar to the right and the rows are out of sync
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void vScrollBar3_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				_isScrolling = true;
				g7.TopRow = ((VScrollBar)sender).Value * rowsPerGroup7;
				g8.TopRow = ((VScrollBar)sender).Value * rowsPerGroup7;
				g9.TopRow = ((VScrollBar)sender).Value * rowsPerGroup7;
				_isScrolling = false;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void vScrollBar4_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				_isScrolling = true;
				g10.TopRow = ((VScrollBar)sender).Value * rowsPerGroup7;
				g11.TopRow = ((VScrollBar)sender).Value * rowsPerGroup7;
				g12.TopRow = ((VScrollBar)sender).Value * rowsPerGroup7;
				_isScrolling = false;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		
		private void hScrollBar1_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				_isScrolling = true;
				g1.LeftCol = hScrollBar1.Value;
				g4.LeftCol = hScrollBar1.Value;
				g7.LeftCol = hScrollBar1.Value;
				g10.LeftCol = hScrollBar1.Value;
				SetHScrollBar1Parameters();
				_isScrolling = false;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			
		}
		private void hScrollBar2_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			try
			{
				switch (e.Type)
				{
					
					case ScrollEventType.EndScroll: 
						 
						if (e.NewValue > hScrollBar2.Maximum)
							e.NewValue = hScrollBar2.Maximum;
						else if (e.NewValue < hScrollBar2.Minimum)
							e.NewValue = hScrollBar2.Minimum;	
						 
						if (e.NewValue >= ((GridTag)g2.Tag).CurrentScrollPosition)
							_scroll2Direction = "right";
						else
							_scroll2Direction = "left";

						((ScrollBarValueChanged)hScrollBar2.Tag)(e.NewValue);
						AdjustG3Grid(_scroll2Direction);
						break;
			
					default:
						break;
				}
			}	
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void hScrollBar3_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			AllocationWaferCoordinate wafercoord;
			string curColHdrLabel, curColCompLabel, compRowData, hdrRowData;
			TagForColumn colTag;
            int i;
			try
			{
				switch (e.Type)
				{
					case ScrollEventType.LargeIncrement:
					case ScrollEventType.LargeDecrement:
	
						colTag = (TagForColumn)g3.Cols[g3.LeftCol].UserData;
						wafercoord = colTag.CubeWaferCoorList[_hdrRow];
						curColHdrLabel  = wafercoord.Label;	
						wafercoord = colTag.CubeWaferCoorList[_compRow];
						curColCompLabel  = wafercoord.Label;	
						
						if (e.Type == ScrollEventType.LargeIncrement)
						{
							if (_headerList.Count > 1)	 // BEGIN MID Track #2747 
							{
                                //i = g3.LeftCol + 1;
                                for (i = g3.LeftCol + 1; i < g3.Cols.Count; i++) 
								{
									if (g3.Cols[i].Visible)
									{
										colTag = (TagForColumn)g3.Cols[i].UserData;
										wafercoord = colTag.CubeWaferCoorList[_compRow];
										compRowData = wafercoord.Label;
										wafercoord = colTag.CubeWaferCoorList[_hdrRow];
										hdrRowData = wafercoord.Label;
										if (rbHeader.Checked)
										{
											if (compRowData == curColCompLabel)
											{
												curColHdrLabel = hdrRowData;
												break;
											}
										}
										else if (compRowData != curColCompLabel && hdrRowData == curColHdrLabel)
										{
											curColHdrLabel = hdrRowData;
											break;
										}
										if (i == (g3.Cols.Count - 1))
											i = -1;
									}
								}
							}
							else
								i = g3.LeftCol + BIGHORIZONTALCHANGE;	// END MID Track #2747 
					 	}
						else 
						{
							if (_headerList.Count > 1)  // BEGIN MID Track #2747 
							{
                                //i = g3.LeftCol - 1;
                                for (i = g3.LeftCol - 1; i >= 0; i--) 
								{
									if (g3.Cols[i].Visible)
									{
										colTag = (TagForColumn)g3.Cols[i].UserData;
										wafercoord = colTag.CubeWaferCoorList[_compRow];
										if (rbHeader.Checked)
										{	
											if (wafercoord.Label == curColCompLabel)
												break;
										}
										else if (wafercoord.Label != curColCompLabel)
										{
											break;
										}
									}	
								}
							}
							else
								i = g3.LeftCol - BIGHORIZONTALCHANGE; // END MID Track #2747 
						}
						
						if (i > hScrollBar3.Maximum)
						{
							hScrollBar3.Value = hScrollBar3.Maximum;
							e.NewValue = hScrollBar3.Maximum;
						}	
						else if (i >= 0)
						{
							hScrollBar3.Value = i;
							e.NewValue = i;
						}		
						break;

					case ScrollEventType.EndScroll: 
						bool wentThruLoop = false;
						if (!g3.Cols[e.NewValue].Visible)
						{
							wentThruLoop = true;
							 if (e.NewValue < ((GridTag)g3.Tag).CurrentScrollPosition) 
								 for (i = e.NewValue - 1; i >= 0; i--)
								 {
									 if (g3.Cols[i].Visible)
										 break;
								 }
							else  
								for (i = e.NewValue + 1; i < g3.Cols.Count; i++)
								{
									if (g3.Cols[i].Visible)
										break;
								}

							e.NewValue = i;
							
						}
						
						if (e.NewValue > hScrollBar3.Maximum)
							e.NewValue = hScrollBar3.Maximum;
						else if (e.NewValue < hScrollBar3.Minimum)
							e.NewValue = hScrollBar3.Minimum;	

						if (wentThruLoop) 
							hScrollBar3.Value = e.NewValue;

						((ScrollBarValueChanged)hScrollBar3.Tag)(e.NewValue);
						AdjustG2Grid();
						break;
				}
			}	
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void ChangeHScrollBar2Value(int aNewValue)
		{
			try
			{
                // Begin TT#4236 - RMatelic - Argument Out of Range exception when selecting All Columns in style analysis
                // for some reason in this version and in AnalysisOnly, the LeftCol = -1 and attempting to set it to aNewValue is disregarded. It may be C1.
                if (g2.LeftCol < 0)
                {
                    return;
                }
                // End TT#4236
				_isScrolling = true;
				g2.LeftCol = aNewValue;
				g5.LeftCol = aNewValue;
				g8.LeftCol = aNewValue;
				g11.LeftCol = aNewValue;
				if (g2.LeftCol < aNewValue)
				{
					aNewValue = g2.LeftCol;
					hScrollBar2.Value = aNewValue;
				}
				((GridTag)g2.Tag).CurrentScrollPosition = aNewValue;
				_isScrolling = false;
				_arrowScroll = false;
			}
			catch
			{
				throw;
			}
		}

		private void ChangeHScrollBar3Value(int aNewValue)
		{
			try
			{
                // Begin TT#4236 - RMatelic - Argument Out of Range exception when selecting All Columns in style analysis
                // for some reason in this version and in AnalysisOnly, the LeftCol = -1 and attempting to set it to aNewValue is disregarded. It may be C1.
                if (g3.LeftCol < 0)
                {
                    return;
                }
                // Ene TT#4236
				_isScrolling = true;
				g3.LeftCol = aNewValue;
				g6.LeftCol = aNewValue;
				g9.LeftCol = aNewValue;
				g12.LeftCol = aNewValue;
				if (g3.LeftCol < aNewValue)
				{
					aNewValue = g3.LeftCol;
					hScrollBar3.Value = aNewValue;
				}
				((GridTag)g3.Tag).CurrentScrollPosition = aNewValue;
				_isScrolling = false;
				_arrowScroll = false;
			}
			catch
			{
				throw;
			}
		}

		private void AdjustG2Grid()
		{
			AllocationWaferCoordinate wafercoord;
			string  curColCompLabel;
			TagForColumn colTag;
			int i;
			try
			{
				colTag = (TagForColumn)g3.Cols[g3.LeftCol].UserData;
				wafercoord = colTag.CubeWaferCoorList[_compRow];
				curColCompLabel  = wafercoord.Label;	

				for (i = 0; i < g2.Cols.Count; i++)
				{
					colTag = (TagForColumn)g2.Cols[i].UserData;
					wafercoord = colTag.CubeWaferCoorList[_compRow];
					if (wafercoord.Label == curColCompLabel)
					{
						hScrollBar2.Value = System.Math.Min(i, hScrollBar2.Maximum - hScrollBar2.LargeChange + 1);
						ChangeHScrollBar2Value(hScrollBar2.Value);
						break;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void AdjustG3Grid(string aScrollDirection)
		{
			AllocationWaferCoordinate wafercoord;
			string  curColCompLabel;
			TagForColumn colTag;
			int i;
			try
			{
				colTag = (TagForColumn)g2.Cols[g2.LeftCol].UserData;
				wafercoord = colTag.CubeWaferCoorList[_compRow];
				curColCompLabel  = wafercoord.Label;
                i = g3.LeftCol;
				switch (aScrollDirection)
				{
					case "right":

                        for (i = g3.LeftCol; i < g3.Cols.Count; i++)
						{
							colTag = (TagForColumn)g3.Cols[i].UserData;
							wafercoord = colTag.CubeWaferCoorList[_compRow];
							if (wafercoord.Label == curColCompLabel)
							 	break;
							else if (i == (g3.Cols.Count - 1))
									i = -1;
						}
						break;
					
					case "left":

                        for (i = g3.LeftCol; i < g3.Cols.Count && i >= 0; i--) 
						{
							colTag = (TagForColumn)g3.Cols[i].UserData;
							wafercoord = colTag.CubeWaferCoorList[_compRow];
							if (wafercoord.Label == curColCompLabel)
								break;
							else if (i == 0)
								i =  g3.Cols.Count;
						}
						break;
				}

				hScrollBar3.Value = System.Math.Min(i, hScrollBar3.Maximum - hScrollBar3.LargeChange + 1);
				ChangeHScrollBar3Value(hScrollBar3.Value);
			}
			catch
			{
				throw;
			}
		}

		private void SetScrollBarPosition(ScrollBar aScrollBar, int aPosition)
		{
			try
			{
				if (aPosition != aScrollBar.Value)
				{
					if (aPosition > aScrollBar.Maximum)
						aPosition = aScrollBar.Maximum;
					else if (aPosition < aScrollBar.Minimum)
						aPosition = aScrollBar.Minimum;
					
					aScrollBar.Value = aPosition;
				}

				((ScrollBarValueChanged)aScrollBar.Tag)(aScrollBar.Value);
			}
			catch
			{
				throw;
			}
		}

		private void SetHScrollBar1Parameters()
		{
			int maxScroll;
			try
			{
				// BEGIN MID Track #3027 - Null error when closing window
				if (FormIsClosing)
					return;
				// END MID Track #3027	
				maxScroll = CalculateColMaximumScroll(g1, 1);
				hScrollBar1.Minimum = 0;
				hScrollBar1.Maximum = maxScroll + _colsPerGroup1 - 1;
				hScrollBar1.SmallChange = SMALLCHANGE;
				hScrollBar1.LargeChange = _colsPerGroup1;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		
		private void SetHScrollBar2Parameters()
		{
			try
			{  
				// BEGIN MID Track #3027 - Null error when closing window
				if (FormIsClosing)
					return;
				// END MID Track #3027	
				// for some reason the other CalculateColMaximumScroll 
				// doesn't work correctly for this g2 grid 
				if (hScrollBar2.Visible)
				{
					hScrollBar2.Minimum = 0;
					//hScrollBar2.Maximum = CalculateColMaximumScroll_2(g2, 1);
					hScrollBar2.Maximum = CalculateColMaximumScroll(g2, 1);
					hScrollBar2.SmallChange = SMALLCHANGE;
					hScrollBar2.LargeChange = SMALLCHANGE;  // same increment
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void SetHScrollBar3Parameters()
		{
			try
			{
				// BEGIN MID Track #3027 - Null error when closing window
				if (FormIsClosing)
					return;
				// END MID Track #3027	
				hScrollBar3.Minimum = 0;
				hScrollBar3.Maximum = CalculateColMaximumScroll_2(g3, 1);
				hScrollBar3.SmallChange = SMALLCHANGE;
				hScrollBar3.LargeChange = BIGHORIZONTALCHANGE;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private int CalculateColMaximumScroll(C1FlexGrid aGrid, int aScrollSize)
		{
			int totalColSize;
			int i, j;

			try
			{
				// BEGIN MID Track #3027 - Null error when closing window
				if (aGrid == null)
					return 1;
				// END MID Track #3027
				totalColSize = 0;
				for (i = aGrid.Cols.Count - 1; totalColSize <= aGrid.Width && i >= 0; i -= aScrollSize)
				{
					if ( aGrid.Cols[i].Visible)
						for (j = 0; j < aScrollSize; j++)
						{
							totalColSize += aGrid.Cols[i - j].WidthDisplay;
						}
				}

				if (totalColSize > aGrid.Width)
				{
					i++;
					if (i + aScrollSize < aGrid.Cols.Count)
					{
						i += aScrollSize;
					}
					return i / aScrollSize;
				}
				else
				{
					return 0;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return 0;
			}
		}

		private int CalculateColMaximumScroll_2(C1.Win.C1FlexGrid.C1FlexGrid aGrid, int aNumColsPerGroup)
		{
			// BEGIN MID Track #3027 - Null error when closing window
			if (aGrid == null)
				return 1;
			// END MID Track #3027
			int totalColSize;
			int i, j;
			
			totalColSize = 0;
			for (i = aGrid.Cols.Count - 1; totalColSize <= aGrid.Width && i >= 0; i -= aNumColsPerGroup)
			{
				if ( aGrid.Cols[i].Visible)
					for (j = 0; j < aNumColsPerGroup; j++)
						{
					totalColSize += aGrid.Cols[i - j].WidthDisplay;
					}
			}

			if (totalColSize > aGrid.Width)
			{
				i++;
				if (i + aNumColsPerGroup < aGrid.Cols.Count)
				{
					i += aNumColsPerGroup;
				}
				return (i / aNumColsPerGroup) + BIGHORIZONTALCHANGE - 1;
			}
			else
			{
				return 1;
			}
		}
		private int CalculateRowMaximumScroll(C1FlexGrid aGrid, int aScrollSize)
		{
			int totalRowSize;
			int i, j;

			try
			{
				// BEGIN MID Track #3027 - Null error when closing window
				if (aGrid == null)
					return 1;
				// END MID Track #3027
				totalRowSize = 0;
				for (i = aGrid.Rows.Count - 1; totalRowSize <= aGrid.Height && i >= 0; i -= aScrollSize)
				{
					for (j = 0; j < aScrollSize; j++)
					{
						totalRowSize += aGrid.Rows[i - j].HeightDisplay;
					}
				}

				if (totalRowSize > aGrid.Height)
				{
					i++;
					if (i + aScrollSize < aGrid.Rows.Count)
					{
						i += aScrollSize;
					}
					return (i / aScrollSize) + BIGCHANGE - 1;
				}
				else
				{
					return 1;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return 1;
			}
		}

		#endregion
		#region MouseDown Events
		private void GridMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			int whichGrid;
			C1FlexGrid grid = null;
			bool setDragReady = false;
			bool setAlignment = false;
			try
			{
				grid = (C1FlexGrid)sender;
				whichGrid = ((GridTag)grid.Tag).GridId;
	
				GridMouseRow = grid.MouseRow;
				GridMouseCol = grid.MouseCol;
				
				if (e.Button == MouseButtons.Right)
				{
					RightClickedFrom = (FromGrid)whichGrid;
					
					//if there are columns frozen,
					//put a check mark next to mnuFreezeColumn context menu
					//to visually indicate that a previous freeze has occured.
					switch((FromGrid)whichGrid)
					{
						case FromGrid.g1:
							mnuFreezeColumn1.Checked = g1HasColsFrozen;
							_isSorting = false;
							break;
						case FromGrid.g2:
							//mnuFreezeColumn23.Checked = g2HasColsFrozen;
							mnuFreezeColumn1.Checked = g2HasColsFrozen;
							_isSorting = false;
							break;	
						case FromGrid.g3:
							//mnuFreezeColumn23.Checked = g3HasColsFrozen;
							mnuFreezeColumn1.Checked = g3HasColsFrozen;
							_isSorting = false;
							break;	
						default:
							break;
					}
				} 
				else //Left mouse button clicked
				{
					//If left button is pressed, set dragState to "edragReady" and store the
					//initial row and column range. the "ready" state indicates a drag 
					//operation is to begin if the mouse moves while the mouse is down.
					if (dragState == DragState.dragNone || dragState == DragState.dragReady)
					{
						switch((FromGrid)whichGrid)
						{
							case FromGrid.g1:
								if (g4.Rows.Count > 0)
									setDragReady = true;
								break;
							case FromGrid.g2:
							case FromGrid.g3:
								if (g5.Rows.Count > 0) 
								{
									if (GridMouseRow == 1 && rbHeader.Checked) 
										setDragReady = true;
									else if (GridMouseRow == 0 && rbComponent.Checked) 
										setDragReady = true;
								}
								break;
							case FromGrid.g5:
							case FromGrid.g6:
								if (GridMouseCol == grid.LeftCol)
								{
									setAlignment = true;
									for (int i = grid.LeftCol - 1; i >= 0; i--)
									{
										if (grid.Cols[i].Visible)
										{
											setAlignment = false;
											break;
										}
									}
									if (setAlignment)
									{
										if (grid == g5)
											hScrollBar2.Value = 0;
										else
											hScrollBar3.Value = 0;
									}
								}	
								break;
							
							default:
								break;
						}
					}
					if (setDragReady) 
					{
						dragState = DragState.dragReady;
						_isSorting = true;
						DragStartColumn = GridMouseCol;
					}
					else if (dragState == DragState.dragResize)
					{
						if ( (FromGrid)whichGrid == FromGrid.g1 )
							_isSorting = false;
						else if ( GridMouseRow != 2 )
							_isSorting = false;
					}
					//else
					//	_isSorting = false;
				}

                // Begin Track #6371 - JSmith - Sorting in SKU Review is slow
                // grid does not sort on first click because grid click event does not fire
                // if grid not active.  So, call click code from mouse down
                if ((FromGrid)whichGrid == FromGrid.g1 ||
                    (FromGrid)whichGrid == FromGrid.g2 ||
                    (FromGrid)whichGrid == FromGrid.g3)
                {
                    GridClick(sender);
                }
                // End Track #6371
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#endregion
		#region Before and After Editing Cell Data
		
		private void GridBeforeEdit(object sender, RowColEventArgs e)
		{
			if (this.ExceptionCaught)
			{
				return;
			}
			C1FlexGrid grid;
			string list = string.Empty; 
			try
			{
				grid = (C1FlexGrid)sender;
				TagForGridData DataTag = (TagForGridData)grid.GetCellRange(e.Row, e.Col).UserData;

				if (!DataTag.IsEditable)
				{
					e.Cancel = true;
				}
                // BEGIN TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) - correct base bug 
                else
                {
                    if (grid.Cols[e.Col].DataMap != null && grid.Cols[e.Col].DataMap == _ldRulesVelocity)
                    {
                        int whichGrid = ((GridTag)grid.Tag).GridId;
                        if ((FromGrid)whichGrid == FromGrid.g6)
                        {
                            TagForColumn colTag = (TagForColumn)g3.Cols[e.Col].UserData;
                            if (colTag.CubeWaferCoorList != null)
                            {
                                AllocationWaferCoordinate wafercoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                                if (wafercoord != null && (eAllocationWaferVariable)wafercoord.Key == eAllocationWaferVariable.VelocityInitialRuleType)
                                {
                                    e.Cancel = true;
                                }
                            }
                        }
                    }
                }
                // END TT#507 - AGallagher - Velocity - Add Rule Quantity Requested (#57) - correct base bug 
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void GridStartEdit(object sender, RowColEventArgs e)
		{
			C1FlexGrid grid;

			try
			{
				grid = (C1FlexGrid)sender;
              
				_holdValue = grid[e.Row, e.Col];
				if (!_loading)
				{
					_changedCellRow = e.Row;
					_changedCellCol = e.Col;
					_changedGrid = grid;
                    // begin MID Track 6079 qty allocated changes not accepted after sorting column
                    FromGrid fromColGrid;
                    int imageRow = -1;
                    C1FlexGrid colGrid = null;
                    switch ((FromGrid)((GridTag)grid.Tag).GridId)
                    {
                        case (FromGrid.g4):
                            {
                                colGrid = g1;
                                imageRow = 0;
                                break;
                            }
                        case (FromGrid.g5):
                            {
                                colGrid = g2;
                                imageRow = 1;
                                break;
                            }
                        case (FromGrid.g6):
                            {
                                colGrid = g3;
                                imageRow = 1;
                                break;
                            }
                    }
                    if (imageRow == 1)
                    {
                        TagForColumn ColumnTag = new TagForColumn();
                        ColumnTag = (TagForColumn)colGrid.Cols[_changedCellCol].UserData;
                        if (ColumnTag.Sort != SortEnum.none)
                        {
                            ColumnTag.Sort = SortEnum.none;
                            colGrid.Cols[_changedCellCol].UserData = ColumnTag;
                            colGrid.SetCellImage(imageRow, _changedCellCol, null);
                        }
                    }
                    // end MID Track 6079 qty allocated changes not accepted after sorting column
                    // Begin TT#3686 - RMatelic - Vecity Rule Type Qty does not accept decimals for WOS or Forward WOS Rules
                    if ((FromGrid)((GridTag)grid.Tag).GridId == FromGrid.g6)
                    {
                        CellRange cellRange = g6.GetCellRange(e.Row, e.Col);
                        CellStyle cellStyle = g6.GetCellStyleDisplay(e.Row, e.Col);
                        TagForColumn ColumnTag = (TagForColumn)g3.Cols[e.Col].UserData;
                        bool protect = false;
                        if (RequiresDecimalFormat(ColumnTag, cellRange, ref protect))
                        {
                            cellStyle.Format = "####.0";
                            g6.SetCellStyle(e.Row, e.Col, cellStyle);
                        }
                        else
                        {
                            cellStyle.Format = "######0";
                            g6.SetCellStyle(e.Row, e.Col, cellStyle);
                            if (protect)
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                    // End TT#36862
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void GridAfterEdit(object sender, RowColEventArgs e)
		{
			C1FlexGrid grid = null;
			C1FlexGrid rowGrid;
			C1FlexGrid colGrid;
			TagForColumn colTag;
			AllocationWafer wafer = null;
			AllocationWaferCell [,] cells ; 
			int waferRow = 0, waferCol = 0, whichGrid;
			int cellRow, cellCol, storeCol = 0;
			double cellValue;
			string StoreID;
			
			try
			{
				grid = (C1FlexGrid)sender;
				
				whichGrid = ((GridTag)grid.Tag).GridId;
				
				switch((FromGrid)whichGrid)
				{
					case FromGrid.g5:
						wafer = _wafers[0,1];
						waferRow = 0;
						waferCol = 1;
						break;
					case FromGrid.g6:
						wafer = _wafers[0,2];
						waferRow = 0;
						waferCol = 2;
						break;
					case FromGrid.g8:
						wafer = _wafers[1,1];
						waferRow = 1;
						waferCol = 1;
						break;
					case FromGrid.g9:
						wafer = _wafers[1,2];
						waferRow = 1;
						waferCol = 2;
						break;
					case FromGrid.g11:
						wafer = _wafers[2,1];
						waferRow = 2;
						waferCol = 1;
						break;
					case FromGrid.g12:
						wafer = _wafers[2,2];
						waferRow = 2;
						waferCol = 2;
						break;
				}
				try
				{			
					cellValue = System.Convert.ToDouble(grid[e.Row, e.Col], CultureInfo.CurrentUICulture);
				}
				catch
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_MustBeNumeric,
						MIDText.GetText(eMIDTextCode.msg_MustBeNumeric));
				}	
				cells = wafer.Cells;

				
				rowGrid = GetRowGrid(grid);
				colGrid = GetColumnGrid(grid);
				
				if (   (FromGrid)whichGrid == FromGrid.g5 
					|| (FromGrid)whichGrid == FromGrid.g6) 
				{
					for (int i = 0; i < g1.Cols.Count; i++)
					{
						if ( Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture) == _lblStore)
						{
							storeCol = i;
							break;
						}
					}
					StoreID = Convert.ToString(rowGrid[e.Row, storeCol], CultureInfo.CurrentUICulture);
					cellRow = (Int32)CellRows[StoreID];
					//ResizeColumns();
				}
				else
				{
					cellRow = e.Row;
				}
				
				colTag = (TagForColumn)colGrid.Cols[e.Col].UserData;
				cellCol = colTag.cellColumn;
				
				if (cellValue < cells[cellRow,cellCol].MinimumValue)
				{	
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_MinimumValueExceeded,
						String.Format
						(
						MIDText.GetText(eMIDTextCode.msg_al_MinimumValueExceeded),
						cellValue.ToString(CultureInfo.CurrentUICulture),
						cells[cellRow,cellCol].MinimumValue.ToString(CultureInfo.CurrentUICulture) 
						));
				}	 
				if (cells[cellRow,cellCol].MayExceedPrimaryMaximum == false
					&& cellValue > cells[cellRow,cellCol].PrimaryMaximumValue)
				{	
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_MaximumValueExceeded,
						String.Format
						(
						MIDText.GetText(eMIDTextCode.msg_al_MaximumValueExceeded),
						cellValue.ToString(CultureInfo.CurrentUICulture),
						cells[cellRow,cellCol].PrimaryMaximumValue.ToString(CultureInfo.CurrentUICulture) 
						));
				}	 	
				
				if (cells[cellRow,cellCol].MayExceedGradeMaximum == false 
					&& cellValue > cells[cellRow,cellCol].GradeMaximumValue)
				{	
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_MaximumValueExceeded,
						String.Format
						(
						MIDText.GetText(eMIDTextCode.msg_al_MaximumValueExceeded),
						cellValue.ToString(CultureInfo.CurrentUICulture),
						cells[cellRow,cellCol].GradeMaximumValue.ToString(CultureInfo.CurrentUICulture) 
						));
				}
                // Begin TT#3686 - RMatelic - Vecity Rule Type Qty does not accept decimals for WOS or Forward WOS Rules
                CellRange cellRange = grid.GetCellRange(e.Row, e.Col);
                bool protect = false;
                if ((FromGrid)whichGrid == FromGrid.g6 && RequiresDecimalFormat(colTag, cellRange, ref protect))
                {
                }
                else
                {
                // End TT#3686 - RMatelic
                    if (cellValue % cells[cellRow, cellCol].Multiple != 0)
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_al_MultipleValueIncorrect,
                            String.Format
                            (
                            MIDText.GetText(eMIDTextCode.msg_al_MultipleValueIncorrect),
                            cellValue.ToString(CultureInfo.CurrentUICulture),
                            cells[cellRow, cellCol].Multiple.ToString(CultureInfo.CurrentUICulture)
                            ));
                    }
                }

				// begin TT#59 Implement Temp Locks
                //_trans.SetAllocationCellValue( waferRow, waferCol, cellRow, cellCol, cellValue); 
                _allocationWaferCellChangeList.AddAllocationWaferCellChange(waferRow, waferCol, cellRow, cellCol, cellValue);
                // end TT#59 Implement Temp Locks
				ChangePending = true;
                 // Begin Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set
                _applyPending = true;
                 // End Development TT#8
				//CellRange cellRange = grid.GetCellRange(e.Row, e.Col);  // TT#3686 - RMatelic - Vecity Rule Type Qty does not accept decimals >>> line moved up
				if (Convert.ToDecimal(grid[e.Row, e.Col], CultureInfo.CurrentUICulture) < 0)
				{
					if (cellRange.Style.Name == grid.Styles["Editable1"].Name)
					{
						cellRange.Style = grid.Styles["NegativeEditable1"];
					}
					else if (cellRange.Style.Name == grid.Styles["Editable2"].Name)
					{
						cellRange.Style = grid.Styles["NegativeEditable2"];
					}
				}
				else
				{
					if (cellRange.Style.Name == grid.Styles["NegativeEditable1"].Name)
					{
						cellRange.Style = grid.Styles["Editable1"];
					}
					else if (cellRange.Style.Name == grid.Styles["NegativeEditable2"].Name)
					{
						cellRange.Style = grid.Styles["Editable2"];
					}
				}
				_changedGrid = null;

                // Begin TT#199-MD - RMatelic - Column headers not moving with the cells while using arrow keys
                if ((FromGrid)whichGrid == FromGrid.g6)
                {
                    g3.ScrollPosition = g6.ScrollPosition;
                    g12.ScrollPosition = g6.ScrollPosition;
                }
                // End TT#199-MD
			}	
				//			catch (MIDException MIDexc)
				//			{
				//				switch (MIDexc.ErrorLevel)
				//				{
				//					case eErrorLevel.fatal:
				//						HandleException(MIDexc);
				//						break;
				//
				//					case eErrorLevel.information:
				//					case eErrorLevel.warning:	
				//					case eErrorLevel.severe:
				//						HandleMIDException(MIDexc);
				//						break;
				//				}
				//				grid[e.Row, e.Col] = _holdValue;
				//			}
			catch (Exception ex)
			{
				HandleException(ex);
				grid[e.Row, e.Col] = _holdValue;
			}
		}

		private void GridKeyPress(object sender, KeyPressEventArgs e)
		{
			C1FlexGrid grid;
			System.EventArgs args = new EventArgs();
			try
			{
				grid = (C1FlexGrid)sender;

			 
				if (e.KeyChar == 13)
				{
					try
					{
						btnApply_Click(sender,args);
					}
					catch (Exception ex)
					{
						HandleException(ex);
					}

					e.Handled = true;
		
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		#endregion
		#endregion		

		#region G1, G2 and G3 AfterResizeColumn Events
		private void g1_AfterResizeColumn(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
		{
			try
			{
				//Readjust column sizes for g4, g7, and g10
				g4.Cols[e.Col].Width = g1.Cols[e.Col].Width;
				g7.Cols[e.Col].Width = g1.Cols[e.Col].Width;
				g10.Cols[e.Col].Width = g1.Cols[e.Col].Width;
				dragState = DragState.dragNone;
				g1.AutoSizeRow(0);
				ResizeRows();
				SetHScrollBar1Parameters();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void g2_AfterResizeColumn(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
		{
			try
			{
				//Readjust column sizes for g5, g8, and g11
				g5.Cols[e.Col].Width=g2.Cols[e.Col].Width;
				g8.Cols[e.Col].Width=g2.Cols[e.Col].Width;
				g11.Cols[e.Col].Width=g2.Cols[e.Col].Width;
				dragState = DragState.dragNone;
				g2.AutoSizeRow(1);
				ResizeRows();
				s2.SplitPosition = g2.Rows[g3.Rows.Count - 1].Bottom + s2.Height;
				SetHScrollBar2Parameters();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void g3_AfterResizeColumn(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
		{
			try
			{
				//Readjust column sizes for g6, g9, and g12
				g6.Cols[e.Col].Width=g3.Cols[e.Col].Width;
				g9.Cols[e.Col].Width=g3.Cols[e.Col].Width;
				g12.Cols[e.Col].Width=g3.Cols[e.Col].Width;
				 
				g3.AutoSizeRow(1);
				ResizeRows();
				s3.SplitPosition = g3.Rows[g3.Rows.Count - 1].Bottom + s3.Height;
				SetHScrollBar3Parameters();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#endregion

		#region Code related to drag and drop
		private void GridBeforeResizeColumn(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
		{
			//Since we are resizing, not dragging, we need to set the dragState
			//to "dragResize" so that GridMouseMove event doesn't process the
			//dragging actions.
			try
			{
				dragState = DragState.dragResize;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void GridMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//if the dragState is "dragReady", set the dragState to "started" and begin the drag
			try
			{
				int whichGrid;
				C1FlexGrid grid = null;
							
				if (dragState == DragState.dragReady)
				{
					_isSorting = false;
					dragState = DragState.dragStarted;
					grid = (C1FlexGrid)sender;
					whichGrid = ((GridTag)grid.Tag).GridId;
				 
					switch((FromGrid)whichGrid)
					{
						case FromGrid.g1:
							g2.DropMode = DropModeEnum.None;
							g3.DropMode = DropModeEnum.None;
							g1.DoDragDrop(sender, DragDropEffects.All);
							break;
						case FromGrid.g2:
							g1.DropMode = DropModeEnum.None;
							g3.DropMode = DropModeEnum.None;
							g2.DoDragDrop(sender, DragDropEffects.All);
							break;
						case FromGrid.g3:
							g1.DropMode = DropModeEnum.None;
							g2.DropMode = DropModeEnum.None;
							g3.DoDragDrop(sender, DragDropEffects.All);
							break;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
	
		private void GridMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//if the button is released, set the dragState to "dragNone"
			try
			{
				int whichGrid;
				C1FlexGrid grid = null;
			
				grid = (C1FlexGrid)sender;
				whichGrid = ((GridTag)grid.Tag).GridId;
				dragState = DragState.dragNone;
				switch((FromGrid)whichGrid)
				{
					case FromGrid.g1:
						g2.DropMode = DropModeEnum.Manual;
						g3.DropMode = DropModeEnum.Manual;
						break;
					case FromGrid.g2:
						g1.DropMode = DropModeEnum.Manual;
						g3.DropMode = DropModeEnum.Manual;
						break;
					case FromGrid.g3:
						g1.DropMode = DropModeEnum.Manual;
						g2.DropMode = DropModeEnum.Manual;
						break;
					default:
						break;
				}
				
				
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void GridDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//Check the keystate(state of mouse buttons, among other things). 
			//If the left mouse is down, we're okay. Else, set dragState
			//to "none", which will cause the drag operation to be cancelled at the 
			//next QueryContinueDrag call. This situation occurs if the user
			//releases the mouse button outside of the grid.
			int whichGrid;
			C1FlexGrid grid = null;
			try
			{
				if ((e.KeyState & 0x01) ==1)
				{
					e.Effect=DragDropEffects.All;
				}
				else
				{
					grid = (C1FlexGrid)sender;
					whichGrid = ((GridTag)grid.Tag).GridId;
					dragState = DragState.dragNone;
					switch((FromGrid)whichGrid)
					{
						case FromGrid.g1:
							g2.DropMode = DropModeEnum.Manual;
							g3.DropMode = DropModeEnum.Manual;
							break;
						case FromGrid.g2:
							g1.DropMode = DropModeEnum.Manual;
							g3.DropMode = DropModeEnum.Manual;
							break;
						case FromGrid.g3:
							g1.DropMode = DropModeEnum.Manual;
							g2.DropMode = DropModeEnum.Manual;
							break;
						default:
							break;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void GridDragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//During drag over, if the mouse is dragged to the left- or right-most
			//part of the grid, scroll grid to show the columns.

			int mouseX;
			int whichGrid;
			C1FlexGrid grid = null;
			try
			{
				mouseX = e.X - (this.Left + ((this.Size.Width - this.ClientSize.Width) / 2));
				grid = (C1FlexGrid)sender;
				whichGrid = ((GridTag)grid.Tag).GridId;
				
				switch((FromGrid)whichGrid)
				{
					case FromGrid.g1:
						if (mouseX > pnlRowHeaders.Right - 20 && mouseX < pnlRowHeaders.Right)
						{
							g1.LeftCol ++;
						}
						else if (mouseX > pnlRowHeaders.Left && mouseX < pnlRowHeaders.Left + 20)
						{
							g1.LeftCol --;
						}
						break;
					case FromGrid.g2:
						if (mouseX > pnlTotals.Right - 20 && mouseX < pnlTotals.Right)
						{
							g2.LeftCol ++;
						}
						else if (mouseX > pnlTotals.Left && mouseX < pnlTotals.Left + 20)
						{
							g2.LeftCol --;
						}
						break;
					case FromGrid.g3:
						if (mouseX > pnlData.Right - 20 && mouseX < pnlData.Right)
						{
							g3.LeftCol ++;
						}
						else if (mouseX > pnlData.Left && mouseX < pnlData.Left + 20)
						{
							g3.LeftCol --;
						}
						break;
					default:
						break;

				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		
		private void GridQueryContinueDrag(object sender, System.Windows.Forms.QueryContinueDragEventArgs e)
		{
			//Check to see if the drag should continue. 
			//Cancel if:
			//(1) the escape key is pressed
			//(2) the DragEnter event handler cancels the drag by setting the 
			//		dragState to "none".
			//Otherwise, if the mouse is up, perform a drop, or continue if the 
			//mouse is down.
			bool setDropToManual = false;
			int whichGrid;
			C1FlexGrid grid = null;
			try
			{
				if (e.EscapePressed)
				{
					e.Action = System.Windows.Forms.DragAction.Cancel;
					setDropToManual = true;
					g2.DropMode = DropModeEnum.Manual;
					g3.DropMode = DropModeEnum.Manual;
				}
				else if ((e.KeyState & 0x01) == 0)
				{
					if (dragState == DragState.dragNone)
					{
						e.Action = DragAction.Cancel;
						setDropToManual = true;
						g2.DropMode = DropModeEnum.Manual;
						g3.DropMode = DropModeEnum.Manual;
					}
					else
					{
						e.Action = DragAction.Drop;
						setDropToManual = true;
						g2.DropMode = DropModeEnum.Manual;
						g3.DropMode = DropModeEnum.Manual;
					}
				}
				else
				{
					e.Action = System.Windows.Forms.DragAction.Continue;
				}
				if (setDropToManual) 
				{
					grid = (C1FlexGrid)sender;
					whichGrid = ((GridTag)grid.Tag).GridId;
				
					switch((FromGrid)whichGrid)
					{
						case FromGrid.g1:
							g2.DropMode = DropModeEnum.Manual;
							g3.DropMode = DropModeEnum.Manual;
							break;
						case FromGrid.g2:
							g1.DropMode = DropModeEnum.Manual;
							g3.DropMode = DropModeEnum.Manual;
							break;
						case FromGrid.g3:
							g1.DropMode = DropModeEnum.Manual;
							g2.DropMode = DropModeEnum.Manual;
							break;
						default:
							break;
					}
				}

			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void g1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//This event gets fired once the user releases the mouse button during
			//a drag-drop action.
			
			int DragStopColumn = g1.MouseCol; //which column did the user halt.
			
			try
			{
				// BEGIN MID Track #2765 - system argument error
				// Occurs when user "drops" column outside the last column but still within the grid 
				if (DragStopColumn < 0)
					return; 
				// END MID Track #2765

				if (DragStartColumn != DragStopColumn) //we are moving either direction
				{
					try
					{
						g1.Cols.MoveRange(DragStartColumn, 1, DragStopColumn);
						g4.Cols.MoveRange(DragStartColumn, 1, DragStopColumn); 
						g7.Cols.MoveRange(DragStartColumn, 1, DragStopColumn); 
						g10.Cols.MoveRange(DragStartColumn, 1, DragStopColumn);
                        // Begin TT#2380 - JSmith - Column Changes in Size Review
                        _saveCurrentColumns = true;
                        SaveCurrentColumns();
                        // End TT#2380
					}
					catch (Exception ex)
					{
						HandleException(ex);
					}
				}
				
				//Finally, we want to clear the dragState. 
				//This is an important clean-up step.
				dragState = DragState.dragNone;
				g2.DropMode = DropModeEnum.Manual;
				g3.DropMode = DropModeEnum.Manual;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void g2_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{			
			//This event gets fired once the user releases the mouse button during
			//a drag-drop action.
			//In this procedure, move the columns in both the headings grid(g2)
			//and data grids(g5, g8, g11).

			int DragStopColumn = g2.MouseCol; //which column did the user halt.
			
			try
			{
				// BEGIN MID Track #2765 - system argument error
				// Occurs when user "drops" column outside the last column but still within the grid 
				if (DragStopColumn < 0)
					return; 
				// END MID Track #2765

				if (DragStartColumn != DragStopColumn) //we are moving either direction
				{
					try
					{
						g2.Cols.MoveRange(DragStartColumn, 1, DragStopColumn);
						g5.Cols.MoveRange(DragStartColumn, 1, DragStopColumn); 
						g8.Cols.MoveRange(DragStartColumn, 1, DragStopColumn); 
						g11.Cols.MoveRange(DragStartColumn, 1, DragStopColumn);
                        // Begin TT#2380 - JSmith - Column Changes in Size Review
                        _saveCurrentColumns = true;
                        SaveCurrentColumns();
                        // End TT#2380
					}
					catch (Exception ex)
					{
						HandleException(ex);
					}
				}
				
				//Finally, we want to clear the dragState. 
				//This is an important clean-up step.
				dragState = DragState.dragNone;
				g1.DropMode = DropModeEnum.Manual;
				g3.DropMode = DropModeEnum.Manual;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void g3_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//This event gets fired once the user releases the mouse button during
			//a drag-drop action.
			//In this procedure, move the columns in both the headings grid(g3)
			//and data grids(g6, g9, g12).

			try
			{
				int DragStopColumn = g3.MouseCol; //which column did the user halt.
				// BEGIN MID Track #2765 - system argument error
				// Occurs when user "drops" column outside the last column but still within the grid 
				if (DragStopColumn < 0)
					return; 
				// END MID Track #2765

				if (DragStartColumn != DragStopColumn) //we are moving either direction
				{
					try
					{
						g3.Cols.MoveRange(DragStartColumn, 1, DragStopColumn);
						g6.Cols.MoveRange(DragStartColumn, 1, DragStopColumn); 
						g9.Cols.MoveRange(DragStartColumn, 1, DragStopColumn); 
						g12.Cols.MoveRange(DragStartColumn, 1, DragStopColumn);
                        // Begin TT#2380 - JSmith - Column Changes in Size Review
                        _saveCurrentColumns = true;
                        SaveCurrentColumns();
                        // End TT#2380
					}
					catch (Exception ex)
					{
						HandleException(ex);
					}
				}

				//Finally, we want to clear the dragState. 
				//This is an important clean-up step.
				dragState = DragState.dragNone;
				g1.DropMode = DropModeEnum.Manual;
				g2.DropMode = DropModeEnum.Manual;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#endregion

		#region Code related to "Row/Column Chooser" context menu
		private void mnuColumnChooser1_Click(object sender, System.EventArgs e)
		{
//			SetGridRedraws(false);
            _columnAdded = false;  // MID Track #6410 - Perf slow chging column chooser in Velocity
			ChooseColumn();
            // BEGIN MID Track #6410 - Perf slow chging column chooser in Velocity
            //CriteriaChanged();
            if (_columnAdded)      // MID Track #6410 - Perf slow chging column chooser in Velocity
            {
                CriteriaChanged();
            }
            // END MID Track #6410
//			SetGridRedraws(true);
		}

		private void ChooseColumn()
		{
			AllocationWaferCoordinate wafercoord;	
			try
			{
				ArrayList ColumnHeaders = new ArrayList();
			  
				//The following variable will be passed into the RowColChooser form. 
				//It will tell the RowColChooser form whether to force the user to 
				//pick at least one column when they click the "Apply" button.
				//Most likely for g1, we don't require this, because the Store column
				//is not even an option for them to un-check. Therefore, they can
				//un-check all the columns they see on the RowColChooser form.
				//But for g2 and g3, we want to make sure that at least one column is
				//selected when they leave the column chooser form.
				//bool needsAtLeastOneCol = true; 
				bool needsAtLeastOneCol = false; 
				
				// Set which header row has the component label
				
				switch (RightClickedFrom)
				{
					case FromGrid.g1:
                      
                        // Begin TT#358/#334/#363 - RMatelic - Velocity View column display issues
                        //if (!_showNeedGrid)
                        //    return;
                        if (!_showNeedGrid && _trans.AllocationViewType == eAllocationSelectionViewType.Style)
                        {
                            return;
                        }
                        // End  TT#358/#334/#363  
						
                        // Subtract 1 from the count because of the empty column at the end
						for (int col = 0; col < g1.Cols.Count; col++)
						{
                           	if (   Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture) == _lblStore
								|| Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture).Trim() == string.Empty)
                            {
                                continue;		// Skip 'Store' & extra column -  cannot be hidden
                            }
							//make a new "RowColHeader" object that will be later added to the "ColumnHeaders" arraylist.
                            RowColHeader rch = new RowColHeader();

							//get the tag for the column of the current loop position.
							TagForColumn colTag = new TagForColumn();
							colTag = (TagForColumn)g1.Cols[col].UserData;
							
							if (colTag.CubeWaferCoorList != null)
							{
								//Assign values to the RowColHeader object.
								//wafercoord = colTag.CubeWaferCoorList[2];  // MID Track 4296 Column Chooser Broken
                                wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable); // MID Track 4296 Column Chooser Broken
								rch.Name = wafercoord.Label;
								rch.IsDisplayed = colTag.IsDisplayed;

								//Add this RowColHeader object to the ArrayList.
								ColumnHeaders.Add(rch);
							}
						}
						needsAtLeastOneCol = false;
						break;
	
					case FromGrid.g2:
					case FromGrid.g3:
						bool compFound;
						//Assumption: g3 is always grouped by header
						for (int col = 0; col < g3.Cols.Count; col++)
						{
							//make a new "RowColHeader" object that will be later added to the "ColumnHeaders" arraylist.
							RowColHeader rch2;

							//get the tag for the column of the current loop position.
							TagForColumn colTag = new TagForColumn();
							colTag = (TagForColumn)g3.Cols[col].UserData;

							//Assign values to the RowColHeader object.
							wafercoord = colTag.CubeWaferCoorList[_compRow];
							compFound = false;
							for (int i= 0; i < ColumnHeaders.Count; i++)
							{
								rch2 = (RowColHeader)ColumnHeaders[i];
								if (rch2.Name == wafercoord.Label)
								{
									compFound = true;
									break;
								}
							}
							
							if (!compFound)
							{
								//make a new "RowColHeader" object that will be later added to the "ColumnHeaders" arraylist.
								RowColHeader rch = new RowColHeader();
								rch.Name = wafercoord.Label;
								rch.IsDisplayed = colTag.IsDisplayed;

								//Add this RowColHeader object to the ArrayList.
								ColumnHeaders.Add(rch);
							}
						}
						needsAtLeastOneCol = true;
						break;
					default:
						break;
				}
				
// Begin Track #4868 - JSmith - Variable Groupings
				//RowColChooser frm = new RowColChooser(ColumnHeaders, needsAtLeastOneCol, "Column Chooser");
                RowColChooser frm = new RowColChooser(ColumnHeaders, needsAtLeastOneCol, "Column Chooser", null);
// End Track #4868
				if (frm.ShowDialog() == DialogResult.OK)
				{
                    // Begin TT#2380 - JSmith - Column Changes in Size Review
                    _saveCurrentColumns = true;
                    // End TT#2380
					switch (RightClickedFrom)
					{
						case FromGrid.g1:
							_resetV1Splitter = true;
							ColHeaders1.Clear();
                            // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
                            Application.DoEvents();
                            // End Track #6371
							foreach (RowColHeader header in frm.Headers)
							{
								ColHeaders1.Add(header.Name, header);
							}
                            ShowHideColHeaders(RightClickedFrom, ColHeaders1, true, g4, g5, g6);
							break;
						case FromGrid.g2:
						case FromGrid.g3:
							ColHeaders2.Clear();
							foreach (RowColHeader header in frm.Headers)
							{
								ColHeaders2.Add(header.Name, header);
							}
                            ShowHideColHeaders(RightClickedFrom, ColHeaders2, true, g4, g5, g6);
                            // Begin TT#399 - RMatelic - columns not aligned after adding new columns that exceed page width  
                            //SetScrollBarPosition(hScrollBar2, g2.LeftCol);
                            //SetScrollBarPosition(hScrollBar3, g3.LeftCol);
                            //((GridTag)g2.Tag).CurrentScrollPosition = g2.LeftCol;
                            //((GridTag)g3.Tag).CurrentScrollPosition = g3.LeftCol;
                            SetScrollBarPosition(hScrollBar2, 0);
                            SetScrollBarPosition(hScrollBar3, 0);
                            ((GridTag)g2.Tag).CurrentScrollPosition = 0;
                            ((GridTag)g3.Tag).CurrentScrollPosition = 0;
                            // End TT#399
							break;
					}
//					MiscPositioning();
				}
						
				frm.Dispose();				
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        private void ShowHideColHeaders(FromGrid aFromGrid, Hashtable RowColHeaders, bool fromColChooser,
            C1FlexGrid aG4, C1FlexGrid aG5, C1FlexGrid aG6)
		{
			int i;
			RowColHeader rch;
			string colName;
			AllocationWaferCoordinate wafercoord = null;	

			try
			{
                switch (aFromGrid)
				{
					case FromGrid.g1:
                        
						for (i = 0; i < g1.Cols.Count; i++)
						{
							if (   Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture) == _lblStore
								|| Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture).Trim() == string.Empty)
								continue;	// Skip 'Store' & extra column - cannot be hidden

							TagForColumn colTag = new TagForColumn();
							colTag = (TagForColumn)g1.Cols[i].UserData;

							if (colTag.CubeWaferCoorList != null)
							{
								wafercoord = colTag.CubeWaferCoorList[2];
								AllocationWaferCoordinate variableCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable); // MID Track 4296 Column Chooser Broken
								colName = wafercoord.Label;
								rch = (RowColHeader)RowColHeaders[colName];
								colTag.IsDisplayed = rch.IsDisplayed;
								if (colTag.IsDisplayed &&
									!colTag.IsBuilt &&
									variableCoord != null) // MID Track 4296 Column Chooser Broken
									//wafercoord != null) // MID Track 4296 Column Chooser Broken
								{
									//_trans.BuildWaferColumnsAdd(0,(eAllocationWaferVariable)wafercoord.Key); // MID Track 4296 Column Chooser Broken
									_trans.BuildWaferColumnsAdd(0,(eAllocationWaferVariable)variableCoord.Key); // MID Track 4296 Column Chooser Broken
									colTag.IsBuilt = true;
                                    _columnAdded = true;  // MID Track #6410 - Perf slow chging column chooser in Velocity
                                    // Begin TT#638 - RMatelic - Style Review - Add Basis Vatiables - cosmetic; col initially displayed wide then shrunk; 
                                    // setting the width this makes it less noticable 
                                    if ((eAllocationWaferVariable)variableCoord.Key == eAllocationWaferVariable.BasisGrade) 
                                    {
                                        int defaultWidth = 42;
                                        g1.Cols[i].Width = defaultWidth;
                                        aG4.Cols[i].Width = defaultWidth;
                                        g7.Cols[i].Width = defaultWidth;
                                        g10.Cols[i].Width = defaultWidth;
                                    }    
                                    // End TT#638  
								}
								g1.Cols[i].UserData = colTag;

								//Show/hide relevant columns.
								g1.Cols[i].Visible = rch.IsDisplayed;
                                aG4.Cols[i].Visible = rch.IsDisplayed;
								g7.Cols[i].Visible = rch.IsDisplayed;
								g10.Cols[i].Visible = rch.IsDisplayed;
							}
						}
						if (!fromColChooser && !_exporting)
						{
							CheckVertica1Splitter1();
						}
                        // Begin TT#182 - RMatelic - Velocity Detail - Need Grid not collapsing when columns are removed
                        else if (fromColChooser)
                        {
                            SetV1SplitPosition();
                        }
                        // End TT#182  
   						break;
					case FromGrid.g2: 
					case FromGrid.g3:
                       
						for (i = 0; i < g2.Cols.Count; i++)
						{
							//update the column tag for g2.
							TagForColumn colTag = new TagForColumn();
							colTag = (TagForColumn)g2.Cols[i].UserData;
							wafercoord = colTag.CubeWaferCoorList[_compRow];
                            AllocationWaferCoordinate variableCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable); // MID Track 4296 Column Chooser Broken
							colName = wafercoord.Label;
							rch = (RowColHeader)RowColHeaders[colName];
							colTag.IsDisplayed = rch.IsDisplayed;
							if (colTag.IsDisplayed &&
								!colTag.IsBuilt &&
								variableCoord != null) // MID Track 4296 Column Chooser broken
								//wafercoord != null) // MID Track 4296 Column Chooser broken
							{
								//_trans.BuildWaferColumnsAdd(1,(eAllocationWaferVariable)wafercoord.Key); // MID Track 4296 Column Chooser Broken
								_trans.BuildWaferColumnsAdd(1,(eAllocationWaferVariable)variableCoord.Key); // MID Track 4296 Column Chooser Broken
								colTag.IsBuilt = true;
                                _columnAdded = true;  // MID Track #6410 - Perf slow chging column chooser in Velocity
							}
							g2.Cols[i].UserData = colTag;

							//show/hide relevent columns.
							g2.Cols[i].Visible = rch.IsDisplayed;
							aG5.Cols[i].Visible = rch.IsDisplayed;
							g8.Cols[i].Visible = rch.IsDisplayed;
							g11.Cols[i].Visible = rch.IsDisplayed;
						}
                      
                        if (!fromColChooser && !_exporting)
						{
							CheckVertica1Splitter2();
						}
                        // Begin TT#182 - RMatelic - Velocity Detail - Need Grid not collapsing when columns are removed
                        else if (fromColChooser)
                        {
                            SetV2SplitPosition();
                        }
                        // End TT#182  
						_alColHeaders2 = new ArrayList();
						for (i = 0; i < g3.Cols.Count; i++)
						{
							//update the column tag for g3.
							TagForColumn colTag = new TagForColumn();
							colTag = (TagForColumn)g3.Cols[i].UserData;
							wafercoord = colTag.CubeWaferCoorList[_compRow];
							AllocationWaferCoordinate variableCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable); // MID Track 4296 Column Chooser Broken
							colName = wafercoord.Label;
							rch = (RowColHeader)RowColHeaders[colName];
							 						 
							colTag.IsDisplayed = rch.IsDisplayed;
							if (colTag.IsDisplayed &&
								!colTag.IsBuilt &&
								variableCoord != null) // MID Track 4296 Column Chooser Broken
								//wafercoord != null) // MID Track 4296 Column Chooser Broken
							{
								//_trans.BuildWaferColumnsAdd(2,(eAllocationWaferVariable)wafercoord.Key); // MID Track 4296 Column Chooser Broken
								_trans.BuildWaferColumnsAdd(2,(eAllocationWaferVariable)variableCoord.Key); // MID Track 4296 Column Chooser Broken
								colTag.IsBuilt = true;
                                _columnAdded = true;  // MID Track #6410 - Perf slow chging column chooser in Velocity
							}
							g3.Cols[i].UserData = colTag;

							//show/hide relevant columns.
							g3.Cols[i].Visible = rch.IsDisplayed;
							aG6.Cols[i].Visible = rch.IsDisplayed;
							g9.Cols[i].Visible = rch.IsDisplayed;
							g12.Cols[i].Visible = rch.IsDisplayed;

							if (rch.IsDisplayed)
								_alColHeaders2.Add(colName);
						 
						}
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void CheckVertica1Splitter1()
		{
			//The following code is related to re-adjust the headers panel
			//so that if the number of columns displayed is less than what it
			//used to be, we want to "shrink" the panel so it doesn't take
			//up unnecesary space.
			int SplitterMaxPossiblePosition,lastDisplayedCol = 0;
			TagForColumn colTag;

			if (_loading)
				SplitterMaxPossiblePosition = g1.Cols[g1.Cols.Count - 2].Right + VerticalSplitter1.Width + 3;
			else
			{
				for (int i = 0; i < g1.Cols.Count; i++)
				{
					colTag = (TagForColumn)g1.Cols[i].UserData;
					if (colTag.IsDisplayed)
						lastDisplayedCol = i;
				}
				SplitterMaxPossiblePosition = g1.Cols[lastDisplayedCol].Right + VerticalSplitter1.Width + 3;
			}	
			
			if (SplitterMaxPossiblePosition <= UserSetSplitter1Position)
			{
				//move the splitter to "shrink" the Store Need panel.
				VerticalSplitter1.SplitPosition = SplitterMaxPossiblePosition;
			}
			else if (SplitterMaxPossiblePosition > UserSetSplitter1Position)
			{
				
				if (g1.RightCol == (g1.Cols.Count - 1))
					SetV1SplitPosition();
				else
					VerticalSplitter1.SplitPosition = UserSetSplitter1Position;
			}

			hScrollBar1.Minimum = 0;
			if (_loading)
				hScrollBar1.Maximum = (g1.Cols[g1.Cols.Count - 2].Right - g1.Width) + BIGCHANGE;
			else
				hScrollBar1.Maximum = (g1.Cols[lastDisplayedCol].Right - g1.Width) + BIGCHANGE;
		}
		private void CheckVertica1Splitter2()
		{
			//The following code is related to re-adjust the totals panel
			//so that if the number of columns displayed is less than what it
			//used to be, we want to "shrink" the panel so it doesn't take
			//up unnecesary space.
			int  SplitterMaxPossiblePosition = 0;

			if (HeaderOrComponentGroups > 1)
			{
				SplitterMaxPossiblePosition = g2.Cols[g2.Cols.Count - 1].Right + VerticalSplitter2.Width + 3;
			}
			else
				return;
			if (SplitterMaxPossiblePosition <= UserSetSplitter2Position)
			{
				//move the splitter to "shrink" the TOTALS panel.
				VerticalSplitter2.SplitPosition = SplitterMaxPossiblePosition;
			}
//			else if (SplitterMaxPossiblePosition > UserSetSplitter2Position)
//			{
//				if (g2.RightCol == (g2.Cols.Count - 1))
//			 		VerticalSplitter2.SplitPosition = SplitterMaxPossiblePosition;
//				else
//					VerticalSplitter2.SplitPosition = UserSetSplitter2Position;
//			}
			else
				VerticalSplitter2.SplitPosition = UserSetSplitter2Position;

			if (VerticalSplitter2.SplitPosition < 0)
				VerticalSplitter2.SplitPosition = 0;
		}
		#endregion

		#region Group By Header or Group By Components, Velocity Avg by All Stores or Set
		private void rbHeader_CheckedChanged(object sender, System.EventArgs e)
		{
			//re-arrange columns so that headers are grouped together and the 
			//components are displayed underneath.
			// Begin TT#456 - RMatelic - Add views to Size Review: buttons added to view 
            //if (_loading || !rbHeader.Checked) return;
            if (_loading || !rbHeader.Checked || _changingView)
            {
                return;
            }
            // End TT#456
			_hdrRow = 0;
			_compRow = 1;
            _groupByChanged = true;   // TT#358/#334/#363 - RMatelic - Velocity View column display issues 
			_trans.AllocationGroupBy = Convert.ToInt32(eAllocationStyleViewGroupBy.Header, CultureInfo.CurrentUICulture);
			ReloadGridData();
            _groupByChanged = false;   // TT#358/#334/#363 - RMatelic - Velocity View column display issues 
		}

		private void rbComponent_CheckedChanged(object sender, System.EventArgs e)
		{
			//re-arrange columns so that components are grouped together and the 
			//headers are displayed underneath.	
            // Begin TT#456 - RMatelic - Add views to Size Review: buttons added to view 
            //if (_loading || !rbComponent.Checked) return;
			if (_loading || !rbComponent.Checked || _changingView)
            {
                return;
            }
            // End TT#456
			_hdrRow = 1;
			_compRow = 0;
            _groupByChanged = true;   // TT#358/#334/#363 - RMatelic - Velocity View column display issues 
			_trans.AllocationGroupBy = Convert.ToInt32(eAllocationStyleViewGroupBy.Components, CultureInfo.CurrentUICulture);
			ReloadGridData();
            _groupByChanged = false;  // TT#358/#334/#363 - RMatelic - Velocity View column display issues 
		}
	
		private void rbAllStores_CheckedChanged(object sender, System.EventArgs e)
		{
			if (_loading || !rbAllStores.Checked) return;
			// BEGIN MID Track #2761 - sync Velocity & Store Detail windows	
			_trans.VelocityCalculateAverageUsingChain = true;
			if (!_updateFromVelocityWindow)
				UpdateVelocityWindow();
			// END MID Track #2761  
			ReloadGridData();
		}

		private void rbSet_CheckedChanged(object sender, System.EventArgs e)
		{
			if (_loading || !rbSet.Checked) return;
			// BEGIN MID Track #2761 - sync Velocity & Store Detail windows
			_trans.VelocityCalculateAverageUsingChain = false;
			if (!_updateFromVelocityWindow)
				UpdateVelocityWindow();
			// END MID Track #2761
			ReloadGridData();
		}

		// BEGIN MID Track #2761 - sync Velocity & Store Detail windows
		private void UpdateVelocityWindow()
		{
			MIDRetail.Windows.frmVelocityMethod frmVelocityMethod;
			frmVelocityMethod = (MIDRetail.Windows.frmVelocityMethod)_trans.VelocityWindow; 
			frmVelocityMethod.UpdateFromStoreDetail();
			
		}
	
		public void UpdateFromVelocityWindow()
		{
			_updateFromVelocityWindow = true;
			rbAllStores.Checked = _trans.VelocityCalculateAverageUsingChain;
			rbSet.Checked = !_trans.VelocityCalculateAverageUsingChain;
			_updateFromVelocityWindow = false;
		}
		// END MID Track #2761  

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        public DataRow GetStoreDetailViewRow()
        {
            if (_dtViews == null)
            {
                return null;
            }
            else
            {
                int viewRID = Convert.ToInt32(cmbView.SelectedValue, CultureInfo.CurrentUICulture);
                return _dtViews.Rows.Find(viewRID);
            }
        }
        // End TT#231  

		public void ReloadGridData()
		{
			int i,j;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
                
                // Begin TT#456 - RMatelic - Add views to Size Review
                //SaveCurrentColumns();                   // TT#358/#334/#363 - RMatelic - Velocity View column display issues
                if (!_changingView)
                {
                    SaveCurrentColumns();                   
                }    
                // End TT#456

				_trans.RebuildWafers();
				_wafers = _trans.AllocationWafers;
				SetGridRedraws(false);
				FormatGrids1to12();
                // Begin TT#358/#334/#363 - RMatelic - Velocity View column display issues
                //SortByDefault();
                //_setHeaderTags = true;
                _setHeaderTags = false;
                 // End TT#358/#334/#363 - RMatelic - Velocity View column display issues
				AssignTag();
				ChangeStyle();				
                // Begin TT#318 - RMatelic - View column position not retaining  
                //ApplyPreferences();
                // Begin TT#358/#334/#363 - RMatelic - Velocity View column display issues
                
                if (!_changingView)
                {
                    ApplyPreferences();
                }
                // End TT#358/#334/#363
                // End TT#318
                //SetGridRedraws(true);   
				SetNeedAnalysisGrid(g4);

                // Begin TT#358/#334/#363 - RMatelic - Velocity View column display issues
                // Begin TT#318 - RMatelic - View column position not retaining  
                //Cursor.Current = Cursors.WaitCursor;
                //if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
                //{
                //    ApplyViewToGridLayout(Convert.ToInt32(cmbView.SelectedValue, CultureInfo.CurrentUICulture));
                //}
                if (_changingView)
                {
                    ApplyViewToGridLayout(Convert.ToInt32(cmbView.SelectedValue, CultureInfo.CurrentUICulture));
                }
                else
                {
                    ApplyCurrentColumns();
                    ApplyPreferences();
                }    
                // End TT#358/#334/#363
                SetGridRedraws(true);   
                // End TT#318

				vScrollBar2.Value = vScrollBar2.Minimum;
				// BEGIN MID Track #2747 
				for (i = 0; i < g2.Cols.Count; i++)
				{
					if (g2.Cols[i].Visible)
						break;
				}	
				if (i > hScrollBar2.Maximum)
					i = hScrollBar2.Minimum;
				
				for (j = 0; j < g3.Cols.Count; j++)
				{
					if (g3.Cols[j].Visible)
						break;
				}	
				if (j > hScrollBar3.Maximum)
					j = hScrollBar3.Minimum;

				SetScrollBarPosition(hScrollBar2, i);
				SetScrollBarPosition(hScrollBar3, j);
				// END MID Track #2747  
				ChangePending = true;
				Cursor.Current = Cursors.Default;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#endregion

		#region Sorting columns
		//Summary and note about this region:
		//To sort a single column, the user clicks on a column heading. If the
		//column hasn't been sorted, we sort it in ascending order. Otherwise,
		//we toggle between ascending/descending orders.

		//To sort single column, we want to put the code in g2.Click or g3.Click events.
		//The biggest obstacle is that "g2(or g3).Click" may fire in many 
		//(irrelevant) situations, for example, when user right clicks the heading
		//and selects a context menu item, or clicks the header border to resize the column.
		//Therefore, we need to use the _isSorting flag to determine whether the
		//user really means to sort this column or did some other events trigger
		//it. This "_isSorting" flag is set and updated in many places:
		//g2( or g3).MouseDown|BeforeResizeColumn|AfterResizeColumn|DragDrop...

		public void DoSorts()
		{
			ArrayList al  = new ArrayList();
		    
			BuildColumnList(ref al);
			frmSortGridViews = new SortGridViews(_structSort, al); // columns to sort;
			
			frmSortGridViews.StartPosition = FormStartPosition.CenterParent;

			if (frmSortGridViews.ShowDialog() == DialogResult.OK)
			{
				_structSort = frmSortGridViews.SortInfo;
				SortColumns();
				_setHeaderTags = false;
				_doResize = false;
				AssignTag();
				ChangeStyle();				
				ApplyPreferences();
				_isScrolling = true;
				g4.TopRow = 0;
                _vScrollBar2Moved = true;   // TT#4235 - RMatelic - Style review misaligned when making manual changes >> set switch to force BeforeScroll
                g5.TopRow = 0;
                _vScrollBar2Moved = false;  // TT#4235 - RMatelic - Style review misaligned when making manual changes
				g6.TopRow = 0;
				_isScrolling = false;
			}
			frmSortGridViews.Dispose();	
		}

		private void BuildColumnList(ref ArrayList _al)
		{
			AllocationWaferCoordinate wafercoord;	
			try
			{
				string compName, headerName;;
				
				for (int col = 0; col < g1.Cols.Count; col++)
				{
					//make a new structure object that will be later added to the arraylist.
					SortCriteria sortData = new SortCriteria(); 
					
					//get the tag for the column of the current loop position.
					TagForColumn colTag = new TagForColumn();
					colTag = (TagForColumn)g1.Cols[col].UserData;
					
					//Select only the columns that are displayed
					if (colTag.IsDisplayed)
					{
						//Assign values to the RowColHeader object.
						if ( Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture) == _lblStore)
							compName = _lblStore;
						else
						{
							if (colTag.CubeWaferCoorList == null)
								continue;
							else
							{
								// wafercoord = colTag.CubeWaferCoorList[2]; // MID Track 4296 Column Chooser Broken
                                wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable); // MID Track 4296 Column Chooser Broken
								compName = wafercoord.Label;
							}
						}
						sortData.Column1 = "Store Need";
						sortData.Column2 = compName;
						sortData.Column2Num = col;
						sortData.Column2Grid = FromGrid.g4;
						sortData.SortDirection = SortEnum.none;
						_al.Add(sortData); 
					}
				}
				for (int col = 0; col < g2.Cols.Count; col++)
				{
					//make a new structure object that will be later added to the arraylist.
					SortCriteria sortData = new SortCriteria(); 

					//get the tag for the column of the current loop position.
					TagForColumn colTag = new TagForColumn();
					colTag = (TagForColumn)g2.Cols[col].UserData;

					//Select only the columns that are displayed
					if (colTag.IsDisplayed)
					{
						wafercoord = colTag.CubeWaferCoorList[_hdrRow];
						headerName = wafercoord.Label.Trim();
						wafercoord = colTag.CubeWaferCoorList[_compRow];
						compName = wafercoord.Label;
						sortData.Column1 = headerName;
						sortData.Column2 = compName;
						sortData.Column2Num = col;
						sortData.Column2Grid = FromGrid.g5;
						sortData.SortDirection = SortEnum.none;
						_al.Add(sortData); 
					}
				}	
				for (int col = 0; col < g3.Cols.Count; col++)
				{
					//make a new structure object that will be later added to the arraylist.
					SortCriteria sortData = new SortCriteria(); 
					
					//get the tag for the column of the current loop position.
					TagForColumn colTag = new TagForColumn();
					colTag = (TagForColumn)g3.Cols[col].UserData;

					//Select only the columns that are displayed
					if (colTag.IsDisplayed)
					{
						wafercoord = colTag.CubeWaferCoorList[_hdrRow];
						headerName = wafercoord.Label.Trim();
						wafercoord = colTag.CubeWaferCoorList[_compRow];
						compName = wafercoord.Label;
						sortData.Column1 = headerName;
						sortData.Column2 = compName;
						sortData.Column2Num = col;
						sortData.Column2Grid = FromGrid.g6;
						sortData.SortDirection = SortEnum.none;
						_al.Add(sortData); 
					}
				}
			}			
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void SortColumns()
		{
			string sortString = String.Empty, sortdirection;
			int i;
			if (_structSort.IsSortingByDefault)
			{
				SortByDefault();
				return;
			}

            // BEGIN MID Track #6416 - Attempt to shorten Analysis screen in Velocity and get error		
            // this may not be necessary, but it is put in as a fail safe 
            if (_structSort.SortInfo == null || _structSort.SortInfo.Count == 0)
            {
                return;
            }
            // END MID Track #6416

			SortCriteria sortData = new SortCriteria(); 
			for (i = 0; i < 3; i++)
			{
				mnuSortByDefault.Checked = false;
				sortData = (SortCriteria)_structSort.SortInfo[i];
						
				if (sortData.Column1 != String.Empty)
				{
					switch (sortData.SortDirection) 
					{
						case SortEnum.asc:
							sortdirection = "ASC";
							break;
						case SortEnum.desc:
							sortdirection = "DESC";
							break;
						default:
							sortdirection = "ASC";
							break;
					}
					if (sortString != String.Empty)
					{
						sortString = sortString + ",";
					}
					sortString = sortString
						+ sortData.Column1 + " " 
						+ sortData.Column2 + " "  
						+ sortdirection;
				}
			}
			_g456DataView.Sort = sortString;
			_isSorting = false;
            // begin MID Track 6079
            SetSortSeqColumn();
            // end MID Track 6079
		}
		private void mnuSortByDefault_Click(object sender, System.EventArgs e)
		{
			SortByDefault();
			_setHeaderTags = false;
            // Begin TT#1508 - RMatelic - Display of percent need truncating >> commment out next line
            //_doResize = false;
            // End TT#1508
			AssignTag();
			ChangeStyle();				
			ApplyPreferences();
		}
		private void SortByDefault()
		{
            // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
            ClearGridSortColumns();
            // End TT#231 

			_structSort.IsSortingByDefault = true;  
			mnuSortByDefault.Checked = true;
			//SortString(FromGrid.g7, 0, 0, g7.Rows.Count - rowsPerStoreGroup, SortEnum.asc);

			_g456DataView.Sort = "Store Need Store ASC";
			_isSorting = false;
			//if (g4.Rows.Count > 1)
			//	SortString(FromGrid.g4, 0, 0, g4.Rows.Count - rowsPerStoreGroup, SortEnum.asc);

			//Erase sorting information (including pic) on g2 and g3.
			if (!_loading)
				ClearSortImages();
			_isScrolling = true;	
			g4.TopRow = 0;
            _vScrollBar2Moved = true;   // TT#4235 - RMatelic - Style review misaligned when making manual changes >> set switch to force BeforeScroll
            g5.TopRow = 0;
            _vScrollBar2Moved = false;  // TT#4235 - RMatelic - Style review misaligned when making manual changes
			g6.TopRow = 0;
			_isScrolling = false;
            // begin MID Track 6079
            SetSortSeqColumn();
            // end MID Track 6079
		}

        // begin TT#59 Implement Store Temp Locks
        private void mnuHeaderAllocationCriteria_Click(object sender, System.EventArgs e)
        {
            if (_trans.HeaderInformation == null)
            {
                _trans.HeaderInformation = new HeaderInformation(_trans, this._trans.GetHeaderInformation());
                _trans.HeaderInformation.Show();
            }
            _trans.HeaderInformation.BringToFront();
        }
        // end TT#59 Implement Store Temp Locks

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private void ClearGridSortColumns()
        {
            try
            {
                ClearGridSortColumns(g1);
                ClearGridSortColumns(g2);
                ClearGridSortColumns(g3);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ClearGridSortColumns(C1FlexGrid aGrid)
        {
            try
            {
                for (int i = 0; i < aGrid.Cols.Count; i++)
                {
                    aGrid.Cols[i].Sort = SortFlags.None;
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#231  

        // Begin Track #6371 - JSmith - Sorting in SKU Review is slow
        //private void GridClick(object sender, System.EventArgs e)
        private void GridClick(object sender)
        // End Track #6371
		{
			int whichGrid, imageRow;

			//sorting a single column
			if (_isSorting == false) return;
			if (GridMouseCol < 0) return;
			try
			{
				C1FlexGrid grid = null;
				grid = (C1FlexGrid)sender;
				whichGrid = ((GridTag)grid.Tag).GridId;
				if ((FromGrid)whichGrid == FromGrid.g1)
					//||( (FromGrid)whichGrid == FromGrid.g2 && rbComponent.Checked) )
					imageRow = 0;
				else
					imageRow =1;
			
				//get the column tag and check its current sort status
				TagForColumn ColumnTag = new TagForColumn();
				ColumnTag = (TagForColumn)grid.Cols[GridMouseCol].UserData;

				// BEGIN MID Track #6416 - Attempt to shorten Analysis screen in Velocity and get error		
                if (ColumnTag.CubeWaferCoorList == null)
                {
                    // BEGIN MID Track #6457 - Can not sort on store column in SKU Review, Need Analysis, or Velocity Detail screens.
                    //return;
                    if ((FromGrid)whichGrid == FromGrid.g1)
                    {
                        if (ColumnTag.cellColumn > 0)   // cellColumn for Store Column = 0
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                    // END MID Track #6457
                }
                // END MID Track #6416

				if (ColumnTag.Sort == SortEnum.none || ColumnTag.Sort == SortEnum.asc)
				{
					//Either the column hasn't been sorted or it's currently sorted in asc order.
					//We're going to sort this column in descending order.
					//SortNumber(FromGrid.g5, g2MouseCol, 0, g5.Rows.Count - rowsPerStoreGroup, SortEnum.desc);

					SortSingleColumn(grid, GridMouseCol, SortEnum.desc);
					Bitmap downArrow = new Bitmap(GraphicsDirectory + "\\down.gif");
					grid.SetCellImage(imageRow, GridMouseCol, downArrow);
										 
					//grid.Cols[GridMouseCol].ImageAlign = ImageAlignEnum.RightTop;
				
					ColumnTag.Sort = SortEnum.desc;
					grid.Cols[GridMouseCol].UserData = ColumnTag;
                    grid.Cols[GridMouseCol].Sort = SortFlags.Descending;     // TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
				}
				else if (ColumnTag.Sort == SortEnum.desc)
				{
					//The column is currently sorted in descending order.
					//Sort in ascending order.

					//SortNumber(FromGrid.g5, g2MouseCol, 0, g5.Rows.Count - rowsPerStoreGroup, SortEnum.asc);
					SortSingleColumn(grid, GridMouseCol, SortEnum.asc);
					
					Bitmap upArrow = new Bitmap(GraphicsDirectory + "\\up.gif");
					grid.SetCellImage(imageRow, GridMouseCol, upArrow);
					//grid.Cols[GridMouseCol].ImageAlign = ImageAlignEnum.LeftCenter;
				
					ColumnTag.Sort = SortEnum.asc;
					grid.Cols[GridMouseCol].UserData = ColumnTag;
                    grid.Cols[GridMouseCol].Sort = SortFlags.Ascending;      // TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
				}

				Cursor.Current = Cursors.WaitCursor;
                // Begin Track #6371 - JSmith - Sorting in SKU Review is slow
                SetGridRedraws(false);
                // End Track #6371
				ChangeStylesForG4_G12();
                // Begin Track #6371 - JSmith - Sorting in SKU Review is slow
                SetGridRedraws(true);
                // End Track #6371
				Cursor.Current = Cursors.Default;
				
				_doResize = false;
				MiscPositioning();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SortSingleColumn(C1FlexGrid aGrid, int ColToSort, SortEnum SortDirection)
		{
			string headerName = String.Empty, compName = String.Empty;
			int whichGrid; 
			AllocationWaferCoordinate wafercoord;	
			try
			{
				whichGrid = ((GridTag)aGrid.Tag).GridId;
				_structSort.SortInfo = new ArrayList();
				SortCriteria sortData = new SortCriteria(); 
				TagForColumn colTag = new TagForColumn();

				colTag = (TagForColumn)aGrid.Cols[ColToSort].UserData;

				switch((FromGrid)whichGrid)
				{
					case FromGrid.g1:
						headerName = "Store Need";	
						if ( Convert.ToString(g1.GetData(0, ColToSort), CultureInfo.CurrentUICulture) == _lblStore)
							compName = _lblStore;
						else
						{
							//wafercoord = colTag.CubeWaferCoorList[2]; // MID Track 4296 Column Chooser Broken
							wafercoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable); // MID Track 4296 Column Chooser Broken
							compName = wafercoord.Label;
						}
						break;
					case FromGrid.g2:
					case FromGrid.g3:
						wafercoord = colTag.CubeWaferCoorList[_hdrRow];
						headerName = wafercoord.Label.Trim();
						wafercoord = colTag.CubeWaferCoorList[_compRow];
						compName = wafercoord.Label;
						break;
				}
				_structSort.IsSortingByDefault = false;
				sortData.Column1 = headerName;
				sortData.Column2 = compName;
				sortData.SortDirection = SortDirection;
				_structSort.SortInfo.Add(sortData);
			
				sortData.Column1 = String.Empty;
				sortData.Column2 = String.Empty;
				sortData.SortDirection = SortEnum.none;
				_structSort.SortInfo.Add(sortData);
			
				sortData.Column1 = String.Empty;
				sortData.Column2 = String.Empty;
				sortData.SortDirection = SortEnum.none;
				_structSort.SortInfo.Add(sortData);
				SortColumns();
				
				ClearSortImages();
				_isScrolling = true;
                // Begin TT#2930 - JSmith - Scrolling Issue
                vScrollBar2.Value = 0;
                // End TT#2930 - JSmith - Scrolling Issue
				g4.TopRow = 0;
                _vScrollBar2Moved = true;   // TT#4235 - RMatelic - Style review misaligned when making manual changes >> set switch to force BeforeScroll
                g5.TopRow = 0;
                _vScrollBar2Moved = false;  // TT#4235 - RMatelic - Style review misaligned when making manual changes
				g6.TopRow = 0;
				_isScrolling = false;
                // begin MID Track 6079
                //SetSortSeqColumn();
                // end MID Track 6079
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        // begin MID Track 6079
        private void SetSortSeqColumn()
        {
            if (_g456DataView.Sort == _sortSeqColumn)
            {
                return;
            }
            DataRow[] sortedRows = _g456DataView.Table.Select(string.Empty, _g456DataView.Sort);
            _structSortSave = _structSort;
            _sortCriteriaSave = _g456DataView.Sort;
            _sortCriteriaChanged = false;
            for (int i = 0; i < sortedRows.Length; i++)
            {
                sortedRows[i][1] = i;
            }
            _g456DataView.Sort = _sortSeqColumn;
            this._g456DataView.Table.AcceptChanges();
            
			_structSort.SortInfo = new ArrayList();
			SortCriteria sortData = new SortCriteria(); 
    		_structSort.IsSortingByDefault = false;
			sortData.Column1 = "Store Need";
			sortData.Column2 = g4.Cols[1].Name.Substring(11);
			sortData.SortDirection = SortEnum.asc;
			_structSort.SortInfo.Add(sortData);
		
			sortData.Column1 = String.Empty;
			sortData.Column2 = String.Empty;
			sortData.SortDirection = SortEnum.none;
			_structSort.SortInfo.Add(sortData);
		
			sortData.Column1 = String.Empty;
			sortData.Column2 = String.Empty;
			sortData.SortDirection = SortEnum.none;
			_structSort.SortInfo.Add(sortData);

            //_isScrolling = true;
            //g4.TopRow = 0;
            //g5.TopRow = 0;
            //g6.TopRow = 0;
            //_isScrolling = false;
        }
        // end MID Track 6079
	
		private void ClearSortImages()
		{	
			ClearSortImage(g1);
			ClearSortImage(g2);
			ClearSortImage(g3); 
		}
		private void ClearSortImage(C1FlexGrid aGrid)
		{
			int whichGrid, imageRow;
			TagForColumn ColumnTag = new TagForColumn();
			C1FlexGrid grid = null;
			grid = aGrid;
			whichGrid = ((GridTag)grid.Tag).GridId;
			if ((FromGrid)whichGrid == FromGrid.g1)
				imageRow = 0;
			else
				imageRow =1;
			for (int i = 0; i < grid.Cols.Count; i++)
			{
				ColumnTag = (TagForColumn)grid.Cols[i].UserData;
				ColumnTag.Sort = SortEnum.none;
				grid.Cols[i].UserData = ColumnTag;
				grid.SetCellImage(imageRow, i, null);
			}
			grid.AutoSizeRow(imageRow);
		}

		#endregion		

		private void mnuFreezeColumn1_Click(object sender, System.EventArgs e)
		{
			int i, NumColsScrolledOffScreen, NumColsFrozen;
							
			try
			{
				TagForColumn colTag = new TagForColumn();
				switch(RightClickedFrom)
				{
					case FromGrid.g1:
						NumColsScrolledOffScreen = g1.LeftCol;
						if (mnuFreezeColumn1.Checked == true) //Unfreeze the columns.
						{
							for (i = 0; i < NumColsScrolledOffScreen; i++)
							{
								colTag = (TagForColumn)g1.Cols[i].UserData;
								if (colTag.IsDisplayed == true)
								{
									g1.Cols[i].Visible = true;
									g4.Cols[i].Visible = true;
									g7.Cols[i].Visible = true;
									g10.Cols[i].Visible = true;
								}
							}

							g1.Cols.Frozen = 0;
							g4.Cols.Frozen = 0;
							g7.Cols.Frozen = 0;
							g10.Cols.Frozen = 0;
							g1.LeftCol = LeftMostColBeforeFreeze1;
							g4.LeftCol = LeftMostColBeforeFreeze1;
							g7.LeftCol = LeftMostColBeforeFreeze1;
							g10.LeftCol = LeftMostColBeforeFreeze1;

							g1HasColsFrozen = false;
						}
						else //Freeze the columns.
						{
							LeftMostColBeforeFreeze1 = g1.LeftCol; //this var will be used later when un-freezing.
							NumColsScrolledOffScreen = g1.LeftCol;

							for (i = 0; i < NumColsScrolledOffScreen; i++)
							{
								g1.Cols[i].Visible = false;
								g4.Cols[i].Visible = false;
								g7.Cols[i].Visible = false;
								g10.Cols[i].Visible = false;
							}
							NumColsFrozen = GridMouseCol + 1;
							g1.Cols.Frozen = NumColsFrozen;
							g4.Cols.Frozen = NumColsFrozen;
							g7.Cols.Frozen = NumColsFrozen;
							g10.Cols.Frozen = NumColsFrozen;
							g1.LeftCol = GridMouseCol;
							g4.LeftCol = GridMouseCol;
							g7.LeftCol = GridMouseCol;
							g10.LeftCol = GridMouseCol;

							g1HasColsFrozen = true;
						}
						SetHScrollBar1Parameters();
						break;
					case FromGrid.g2:
						NumColsScrolledOffScreen = g2.LeftCol;
						if (mnuFreezeColumn1.Checked == true) //Unfreeze the columns.
						{
							for (i = 0; i < NumColsScrolledOffScreen; i++)
							{							
								colTag = (TagForColumn)g2.Cols[i].UserData;
								if (colTag.IsDisplayed == true)
								{
									g2.Cols[i].Visible = true;
									g5.Cols[i].Visible = true;
									g8.Cols[i].Visible = true;
									g11.Cols[i].Visible = true;
								}							
							}
							g2.Cols.Frozen = 0;
							g5.Cols.Frozen = 0;
							g8.Cols.Frozen = 0;
							g11.Cols.Frozen = 0;
							g2.LeftCol = LeftMostColBeforeFreeze2;
							g5.LeftCol = LeftMostColBeforeFreeze2;
							g8.LeftCol = LeftMostColBeforeFreeze2;
							g11.LeftCol = LeftMostColBeforeFreeze2;

							g2HasColsFrozen = false;
						}
						else	// freeze the column
						{	
							LeftMostColBeforeFreeze2 = g2.LeftCol; //this var will be used later when un-freezing.

							for (i = 0; i < NumColsScrolledOffScreen; i++)
							{
								g2.Cols[i].Visible = false;
								g5.Cols[i].Visible = false;
								g8.Cols[i].Visible = false;
								g11.Cols[i].Visible = false;
							}
							NumColsFrozen = GridMouseCol + 1;
							g2.Cols.Frozen = NumColsFrozen;
							g5.Cols.Frozen = NumColsFrozen;
							g8.Cols.Frozen = NumColsFrozen;
							g11.Cols.Frozen = NumColsFrozen;

							g2.LeftCol = GridMouseCol;
							g5.LeftCol = GridMouseCol;
							g8.LeftCol = GridMouseCol;
							g11.LeftCol = GridMouseCol;

							g2HasColsFrozen = true;
						}
						SetHScrollBar2Parameters();
						break;
					case FromGrid.g3:
						NumColsScrolledOffScreen = g3.LeftCol;
						if (mnuFreezeColumn1.Checked == true) //Unfreeze the columns.
						{
							for (i = 0; i < NumColsScrolledOffScreen; i++)
							{							
								colTag = (TagForColumn)g3.Cols[i].UserData;
								if (colTag.IsDisplayed == true)
								{
									g3.Cols[i].Visible = true;
									g6.Cols[i].Visible = true;
									g9.Cols[i].Visible = true;
									g12.Cols[i].Visible = true;
								}							
							}
							g3.Cols.Frozen = 0;
							g6.Cols.Frozen = 0;
							g9.Cols.Frozen = 0;
							g12.Cols.Frozen = 0;
							g3.LeftCol = LeftMostColBeforeFreeze3;
							g6.LeftCol = LeftMostColBeforeFreeze3;
							g9.LeftCol = LeftMostColBeforeFreeze3;
							g12.LeftCol = LeftMostColBeforeFreeze3;

							g3HasColsFrozen = false;
						}
						else	// freeze the column
						{	
							LeftMostColBeforeFreeze3 = g3.LeftCol; //this var will be used later when un-freezing.

							for (i = 0; i < NumColsScrolledOffScreen; i++)
							{
								g3.Cols[i].Visible = false;
								g6.Cols[i].Visible = false;
								g9.Cols[i].Visible = false;
								g12.Cols[i].Visible = false;
							}
							NumColsFrozen = GridMouseCol + 1;
							g3.Cols.Frozen = NumColsFrozen;
							g6.Cols.Frozen = NumColsFrozen;
							g9.Cols.Frozen = NumColsFrozen;
							g12.Cols.Frozen = NumColsFrozen;

							g3.LeftCol = GridMouseCol;
							g6.LeftCol = GridMouseCol;
							g9.LeftCol = GridMouseCol;
							g12.LeftCol = GridMouseCol;

							g3HasColsFrozen = true;

						}
						SetHScrollBar3Parameters();
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		
		#region Style-changing codes (colors, fonts, etc)
        private void ChangeStyle(bool aCalculatePositioning = true)		// TT#1628-MD - stodd - Style review grid out of sync in GA mode
		{
			try
			{
				if (_loading) 
				{
					CellStyle cellStyle;
					
					cellStyle = g1.Styles.Add("Style1");
					cellStyle.BackColor = _theme.CornerBackColor;
					cellStyle.ForeColor = _theme.CornerForeColor;
					cellStyle.Font = _theme.CornerFont;
					cellStyle.Border.Style = BorderStyleEnum.None;

					cellStyle = g2.Styles.Add("MerDesc");
					cellStyle.BackColor = _theme.NodeDescriptionBackColor;
					cellStyle.ForeColor = _theme.NodeDescriptionForeColor;
					cellStyle.Font = _theme.NodeDescriptionFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.Margins = new System.Drawing.Printing.Margins(0, 0, 3, 3);
					cellStyle.TextEffect = _theme.NodeDescriptionTextEffects;

					cellStyle = g2.Styles.Add("GroupHeader");
					cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
					cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
					cellStyle.Font = _theme.ColumnGroupHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 3, 3);
					cellStyle.TextEffect = _theme.ColumnGroupHeaderTextEffects;

					cellStyle = g2.Styles.Add("ColumnHeading");
					cellStyle.BackColor = _theme.ColumnHeaderBackColor;
					cellStyle.ForeColor = _theme.ColumnHeaderForeColor;
					cellStyle.Font = _theme.ColumnHeaderFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
					cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
					cellStyle.TextEffect = _theme.ColumnHeaderTextEffects;

					cellStyle = g4.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderBackColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
			
					cellStyle = g4.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					 
					cellStyle = g4.Styles.Add("Style1Center");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderBackColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = TextAlignEnum.CenterCenter;
					 
					cellStyle = g4.Styles.Add("Style2Center");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = TextAlignEnum.CenterCenter;
					
					cellStyle = g4.Styles.Add("Style1Left");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderBackColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = TextAlignEnum.LeftCenter;
					 
					cellStyle = g4.Styles.Add("Style2Left");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = TextAlignEnum.LeftCenter;
					 
					cellStyle = g4.Styles.Add("Style1Reverse");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderBackColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
			
					cellStyle = g4.Styles.Add("Style2Reverse");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderAlternateBackColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					 
					cellStyle = g4.Styles.Add("Style1CenterReverse");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderBackColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = TextAlignEnum.CenterCenter;
					 
					cellStyle = g4.Styles.Add("Style2CenterReverse");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderAlternateBackColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = TextAlignEnum.CenterCenter;
					
					cellStyle = g4.Styles.Add("Style1LeftReverse");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderBackColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = TextAlignEnum.LeftCenter;
					 
					cellStyle = g4.Styles.Add("Style2LeftReverse");
					cellStyle.BackColor = _theme.StoreDetailRowHeaderForeColor;
					cellStyle.ForeColor = _theme.StoreDetailRowHeaderAlternateBackColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = TextAlignEnum.LeftCenter;

					cellStyle = g5.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Editable1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Negative1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("NegativeEditable1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Editable2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Negative2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("NegativeEditable2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
					 
					cellStyle = g5.Styles.Add("Style1Reverse");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Editable1Reverse");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Negative1Reverse");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("NegativeEditable1Reverse");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Style2Reverse");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Editable2Reverse");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Negative2Reverse");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("NegativeEditable2Reverse");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					// BEGIN MID Track #1511  Highlight stores whose allocation is out of balance
					cellStyle = g5.Styles.Add("Balance1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Editable1Balance");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Negative1Balance");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("NegativeEditable1Balance");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Balance2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Editable2Balance");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Negative2Balance");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("NegativeEditable2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
					 
					cellStyle = g5.Styles.Add("Style1ReverseBalance");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Editable1ReverseBalance");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Negative1ReverseBalance");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("NegativeEditable1ReverseBalance");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Style2ReverseBalance");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Editable2ReverseBalance");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("Negative2ReverseBalance");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g5.Styles.Add("NegativeEditable2ReverseBalance");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
					// END MID Track # 1511

					cellStyle = g6.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Editable1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Negative1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("NegativeEditable1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Editable2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Negative2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("NegativeEditable2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
					 
					cellStyle = g6.Styles.Add("Style1Reverse");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Editable1Reverse");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Negative1Reverse");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("NegativeEditable1Reverse");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Style2Reverse");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Editable2Reverse");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Negative2Reverse");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("NegativeEditable2Reverse");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
				 
					// BEGIN MID Track #1511 Highlight Stores whose allocation is out of balance
					cellStyle = g6.Styles.Add("Balance1");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Editable1Balance");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Negative1Balance");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("NegativeEditable1Balance");
					cellStyle.BackColor = _theme.StoreDetailBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Balance2");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
 
					cellStyle = g6.Styles.Add("Editable2Balance");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreDetailForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Negative2Balance");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("NegativeEditable2Balance");
					cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
					 
					cellStyle = g6.Styles.Add("Style1ReverseBalance");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Editable1ReverseBalance");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Negative1ReverseBalance");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("NegativeEditable1ReverseBalance");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Style2ReverseBalance");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Editable2ReverseBalance");
					cellStyle.BackColor = _theme.StoreDetailForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("Negative2ReverseBalance");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g6.Styles.Add("NegativeEditable2ReverseBalance");
					cellStyle.BackColor = _theme.NegativeForeColor;
					cellStyle.ForeColor = _theme.StoreDetailAlternateBackColor;
					cellStyle.Font = _theme.StoreAllocationOutOfBalance;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
					// END MID Track #1511

					cellStyle = g7.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreSetRowHeaderBackColor;
					cellStyle.ForeColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
			
					cellStyle = g7.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreSetRowHeaderAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
								
					cellStyle = g7.Styles.Add("Reverse1");
					cellStyle.BackColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.ForeColor = _theme.StoreSetRowHeaderBackColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;

					cellStyle = g7.Styles.Add("Reverse2");
					cellStyle.BackColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.ForeColor = _theme.StoreSetRowHeaderAlternateBackColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
 
					cellStyle = g7.Styles.Add("Style1Left");
					cellStyle.BackColor = _theme.StoreSetRowHeaderBackColor;
					cellStyle.ForeColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = TextAlignEnum.LeftCenter;

					cellStyle = g7.Styles.Add("Style2Left");
					cellStyle.BackColor = _theme.StoreSetRowHeaderAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = TextAlignEnum.LeftCenter;
								
					cellStyle = g7.Styles.Add("Reverse1Left");
					cellStyle.BackColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.ForeColor = _theme.StoreSetRowHeaderBackColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.TextAlign = TextAlignEnum.LeftCenter;

					cellStyle = g7.Styles.Add("Reverse2Left");
					cellStyle.BackColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.ForeColor = _theme.StoreSetRowHeaderAlternateBackColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;	
					cellStyle.TextAlign = TextAlignEnum.LeftCenter;
 
					cellStyle = g8.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Editable1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Negative1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("NegativeEditable1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Editable2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("Negative2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g8.Styles.Add("NegativeEditable2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Editable1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Negative1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("NegativeEditable1");
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Editable2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("Negative2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g9.Styles.Add("NegativeEditable2");
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g10.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreTotalRowHeaderBackColor;
					cellStyle.ForeColor = _theme.StoreTotalRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;

					cellStyle = g10.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreTotalRowHeaderAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalRowHeaderForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;

					cellStyle = g11.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Editable1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Negative1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("NegativeEditable1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Editable2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("Negative2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g11.Styles.Add("NegativeEditable2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Style1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Editable1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Negative1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("NegativeEditable1");
					cellStyle.BackColor = _theme.StoreTotalBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Style2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Editable2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreTotalForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("Negative2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.DisplayOnlyFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;

					cellStyle = g12.Styles.Add("NegativeEditable2");
					cellStyle.BackColor = _theme.StoreTotalAlternateBackColor;
					cellStyle.ForeColor = _theme.NegativeForeColor;
					cellStyle.Font = _theme.EditableFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
				}
				
				g1.Rows[0].Style = g1.Styles["Style1"];
				g2.Rows[0].Style = g2.Styles["GroupHeader"];
				g2.Rows[1].Style = g2.Styles["ColumnHeading"];
				g3.Rows[0].Style = g2.Styles["GroupHeader"];
				g3.Rows[1].Style = g2.Styles["ColumnHeading"];

				g1.Rows[0].TextAlign = TextAlignEnum.CenterBottom;
				g2.Rows[0].TextAlign = TextAlignEnum.CenterBottom;
				g2.Rows[1].TextAlign = TextAlignEnum.CenterBottom;
				g3.Rows[0].TextAlign = TextAlignEnum.CenterBottom;
				g3.Rows[1].TextAlign = TextAlignEnum.CenterBottom;

				ChangeStylesForG4_G12();
				// Begin TT#1628-MD - stodd - Style review grid out of sync in GA mode
                if (aCalculatePositioning)
                {
                    MiscPositioning();
                }
				// End TT#1628-MD - stodd - Style review grid out of sync in GA mode
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		
		private void ChangeStylesForG4_G12()
		{
			try
			{
				//				if	(_styleThread4 != null && _styleThread4.IsAlive) _styleThread4.Abort();
				//				_styleThread4 = new System.Threading.Thread(new System.Threading.ThreadStart(ChangeStylesG4));
				//				_styleThread4.Start();
				//
				//				if	(_styleThread5 != null && _styleThread5.IsAlive) _styleThread5.Abort();
				//				_styleThread5 = new System.Threading.Thread(new System.Threading.ThreadStart(ChangeStylesG5));
				//				_styleThread5.Start();
				//
				//				if	(_styleThread6 != null && _styleThread6.IsAlive) _styleThread6.Abort();
				//				_styleThread6 = new System.Threading.Thread(new System.Threading.ThreadStart(ChangeStylesG6));
				//				_styleThread6.Start();
				//
				//				if	(_styleThread7 != null && _styleThread7.IsAlive) _styleThread7.Abort();
				//				_styleThread7 = new System.Threading.Thread(new System.Threading.ThreadStart(ChangeStylesG7));
				//				_styleThread7.Start();
				//
				//				if	(_styleThread8 != null && _styleThread8.IsAlive) _styleThread8.Abort();
				//				_styleThread8 = new System.Threading.Thread(new System.Threading.ThreadStart(ChangeStylesG8));
				//				_styleThread8.Start();
				//
				//				if	(_styleThread9 != null && _styleThread9.IsAlive) _styleThread9.Abort();
				//				_styleThread9 = new System.Threading.Thread(new System.Threading.ThreadStart(ChangeStylesG9));
				//				_styleThread9.Start();
				//
				//				if	(_styleThread10 != null && _styleThread10.IsAlive) _styleThread10.Abort();
				//				_styleThread10 = new System.Threading.Thread(new System.Threading.ThreadStart(ChangeStylesG10));
				//				_styleThread10.Start();
				//
				//				if	(_styleThread11 != null && _styleThread11.IsAlive) _styleThread11.Abort();
				//				_styleThread11 = new System.Threading.Thread(new System.Threading.ThreadStart(ChangeStylesG11));
				//				_styleThread11.Start();
				//
				//				if	(_styleThread12 != null && _styleThread12.IsAlive)_styleThread12.Abort();
				//				_styleThread12 = new System.Threading.Thread(new System.Threading.ThreadStart(ChangeStylesG12));
				//				_styleThread12.Start();
				
                // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
                //if (!_loading)
                //    SetGridRedraws(false);
                // End Track #6371
				ChangeStylesG4();
				ChangeStylesG5();
				ChangeStylesG6();
                //if (!_loading)			// BEGIN MID TRACK #2747  // TT#4188 - JSmith - Themes -  Row height does not save in conistant format across the different review screens.
					ChangeStylesG7();	// END MID TRACK #2747 
				ChangeStylesG8();
				ChangeStylesG9();
				ChangeStylesG10();
				ChangeStylesG11();
				ChangeStylesG12();
                // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
                //if (!_loading)
                //    SetGridRedraws(true);
                // End Track #6371
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		// BEGIN MID Track # 1511 Highlight stores whose allocation is out of balance
		/// <summary>
		/// Gets a bool value that indicates whether a store's allocation is out of balance
		/// </summary>
		/// <param name="aGridRow">Grid Row</param>
		/// <returns>True:  Store's allocation is out of balance; FALSE:  Store's allocation is in balance</returns>
		private bool StoreAllocationOutOfBalance(int aGridRow)
		{
			AllocationWafer wafer;
			AllocationWaferCell [,] cells;
			string StoreID;
			int storeCol = 0, cellRow;
			try
			{	
				wafer = _wafers[0,2];
				cells = wafer.Cells;
				for (int i = 0; i < g1.Cols.Count; i++)
				{
					if ( Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture) == _lblStore)
					{
						storeCol = i;
						break;
					}
				}
				StoreID = Convert.ToString(g4[aGridRow, storeCol], CultureInfo.CurrentUICulture);
				cellRow = (Int32)CellRows[StoreID];
				// begin MID Track 3966 Store at capacity not highlighted
				for (int i = 0; i < g3.Cols.Count; i++)
				{
					if (cells[cellRow,i].StoreAllocationOutOfBalance)
					{
						return true;
					}
				}
				// end MID Track 3966 Store at capacity not highlighted
				return cells[cellRow,0].StoreAllocationOutOfBalance;
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return false;
			}
		}
		// END MID Track #1511

		private bool StoreExceedsCapacity(int aGridRow)
		{
			AllocationWafer wafer;
			AllocationWaferCell [,] cells;
			string StoreID;
			int storeCol = 0, cellRow;
			try
			{	
				wafer = _wafers[0,2];
				cells = wafer.Cells;
				for (int i = 0; i < g1.Cols.Count; i++)
				{
					if ( Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture) == _lblStore)
					{
						storeCol = i;
						break;
					}
				}
				StoreID = Convert.ToString(g4[aGridRow, storeCol], CultureInfo.CurrentUICulture);
				cellRow = (Int32)CellRows[StoreID];
				return cells[cellRow,0].StoreExceedsCapacity;
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return false;
			}
		}	
		private void ChangeStylesG4()
		{
			CellRange cellRange;
			bool ExceedsCapacity = false;
			try
			{
				if (g4.Rows.Count == 0)
					return;
				ChangeRowStyles(g4, rowsPerStoreGroup);
				ChangeCellStyle(g4);
				for (int i = 0; i < g4.Rows.Count; i++)	
				{
					for (int j = 0; j < g4.Cols.Count; j++)
					{	
						if ( Convert.ToString(g1.GetData(0, j), CultureInfo.CurrentUICulture) == _lblStore)
						{
							ExceedsCapacity = StoreExceedsCapacity(i);
						}
						cellRange = g4.GetCellRange(i, j);
						if (g4.Cols[j].DataType == typeof(System.String)) 
						{
							if ( Convert.ToString(g1.GetData(0, j), CultureInfo.CurrentUICulture) == _lblStore)
							{
								if (cellRange.Style.Name == g4.Styles["Style1"].Name)	
								{
									if (ExceedsCapacity)
										cellRange.Style = g4.Styles["Style1LeftReverse"];
									else
										cellRange.Style = g4.Styles["Style1Left"];
								}
								else if (cellRange.Style.Name == g4.Styles["Style2"].Name)
								{
									if (ExceedsCapacity)
										cellRange.Style = g4.Styles["Style2LeftReverse"];
									else
										cellRange.Style = g4.Styles["Style2Left"];
								}
							}
							else
							{
								if (cellRange.Style.Name == g4.Styles["Style1"].Name)
								{
									if (ExceedsCapacity)
										cellRange.Style = g4.Styles["Style1CenterReverse"];
									else
										cellRange.Style = g4.Styles["Style1Center"];
								}
								else if (cellRange.Style.Name == g4.Styles["Style2"].Name)
								{
									if (ExceedsCapacity)
										cellRange.Style = g4.Styles["Style2CenterReverse"];
									else
										cellRange.Style = g4.Styles["Style2Center"];
								}
							}
						}
						else if (ExceedsCapacity)
						{
							if (cellRange.Style.Name == g4.Styles["Style1"].Name)
							{
								cellRange.Style = g4.Styles["Style1Reverse"];
							}
							else if (cellRange.Style.Name == g4.Styles["Style2"].Name)
							{
								cellRange.Style = g4.Styles["Style2Reverse"];
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void ChangeStylesG5()
		{
			try
			{
				if (g5.Rows.Count == 0)
					return;
				ChangeRowStyles(g5, rowsPerStoreGroup);
				ChangeCellStyle(g5);
				SetCellStyleG5G6(g5);
				Debug.WriteLine("ChangeStylesG5");
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void ChangeStylesG6()
		{
			try
			{
				if (g6.Rows.Count == 0)
					return;
				ChangeRowStyles(g6, rowsPerStoreGroup);
				ChangeCellStyle(g6);
				SetCellStyleG5G6(g6);
				Debug.WriteLine("ChangeStylesG6");
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void ChangeStylesG7()
		{
			int i;
			CellRange cellRange;
			try
			{
				if (g7.Rows.Count == 0)
					return;
				ChangeRowStyles(g7, rowsPerStoreGroup);
				ChangeCellStyle(g7);
				HilightSelectedSet();
				for (i = 0; i < g7.Rows.Count; i++)
				{
					cellRange = g7.GetCellRange(i, 0);
					if (cellRange.Style.Name == g7.Styles["Style1"].Name)							
						cellRange.Style = g7.Styles["Style1Left"];	
					else if (cellRange.Style.Name == g7.Styles["Style2"].Name)							
						cellRange.Style = g7.Styles["Style2Left"];	 
					else if (cellRange.Style.Name == g7.Styles["Reverse1"].Name)							
						cellRange.Style = g7.Styles["Reverse1Left"];	 
					else if (cellRange.Style.Name == g7.Styles["Reverse2"].Name)							
						cellRange.Style = g7.Styles["Reverse2Left"];	 
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void ChangeStylesG8()
		{
			try
			{
				if (g8.Rows.Count == 0)
					return;
				ChangeRowStyles(g8, rowsPerStoreGroup);
				ChangeCellStyle(g8);
				//SetCellStyle(g8);
				SetCellStyleG8G12(g8);
				Debug.WriteLine("ChangeStylesG8");
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void ChangeStylesG9()
		{
			try
			{
				if (g9.Rows.Count == 0)
					return;
				ChangeRowStyles(g9, rowsPerStoreGroup);
				ChangeCellStyle(g9);
				//SetCellStyle(g9);
				SetCellStyleG8G12(g9);
				Debug.WriteLine("ChangeStylesG9");
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void ChangeStylesG10()
		{
			try
			{
				if (g10.Rows.Count == 0)
					return;
				ChangeRowStyles(g10, rowsPerStoreGroup);
				ChangeCellStyle(g10);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void ChangeStylesG11()
		{
			try
			{
				if (g11.Rows.Count == 0)
					return;
				ChangeRowStyles(g11, rowsPerStoreGroup);
				ChangeCellStyle(g11);
				//SetCellStyle(g11);
				SetCellStyleG8G12(g11);
				Debug.WriteLine("ChangeStylesG11");
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void ChangeStylesG12()
		{
			try
			{
				if (g12.Rows.Count == 0)
					return;
				ChangeRowStyles(g12, rowsPerStoreGroup);
				ChangeCellStyle(g12);
				//SetCellStyle(g12);
				SetCellStyleG8G12(g12);
				Debug.WriteLine("ChangeStylesG12");
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void ChangeRowStyles(C1FlexGrid aGrid, int aRowsPerGroup)
		{
			int i;

			try
			{
				if (aGrid.Rows.Count > 0 && aGrid.Cols.Count > 0)
				{
					if (_theme.ViewStyle == StyleEnum.Plain || _theme.ViewStyle == StyleEnum.Chiseled)
					{
						for (i = 0; i < aGrid.Rows.Count; i++)
						{
							aGrid.Rows[i].Style = aGrid.Styles["Style1"];
						}
					}
					else if (_theme.ViewStyle == StyleEnum.AlterColors)
					{
						for (i = 0; i < aGrid.Rows.Count; i++)
						{
							if (i % (aRowsPerGroup * 2) < aRowsPerGroup)
							{
								aGrid.Rows[i].Style = aGrid.Styles["Style1"];
							}
							else
							{
								aGrid.Rows[i].Style = aGrid.Styles["Style2"];
							}
						}
					}
					else if (_theme.ViewStyle == StyleEnum.HighlightName)
					{
						for (i = 0; i < aGrid.Rows.Count; i++)
						{
							if (i % aRowsPerGroup == 0)
							{
								aGrid.Rows[i].Style = aGrid.Styles["Style1"];
							}
							else
							{
								aGrid.Rows[i].Style = aGrid.Styles["Style2"];
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		
		private void ChangeCellStyle(C1FlexGrid aGrid) 
		{	
			int i;
			CellRange cellRange;
			try
			{
				for (i = 0; i < aGrid.Rows.Count; i++)
				{
					cellRange = aGrid.GetCellRange(i, 0, i, aGrid.Cols.Count - 1);
					cellRange.Style = aGrid.Rows[i].Style;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetCellStyle(C1FlexGrid aGrid)
		{
			C1FlexGrid colGrid;
			colGrid = GetColumnGrid(aGrid);
			CellRange cellRange;
			try
			{
				//Loop through the entire sheet and pick out the negatives and the editables.
				for (int col = 0; col < aGrid.Cols.Count; col ++)
				{
					TagForColumn ColumnTag = (TagForColumn)colGrid.Cols[col].UserData;

					if (ColumnTag.IsLockable == true) //Lockable means some cells are editable.
						//We need to check every cell in this column to see whether
						//it's just editable, or just negative, or both.
					{
						for (int row = 0; row < aGrid.Rows.Count; row ++)
						{
							cellRange = aGrid.GetCellRange(row, col);
							TagForGridData DataTag = (TagForGridData)cellRange.UserData;
							try
							{
								//Is it just editable?
								if (DataTag.IsEditable == true && Convert.ToDecimal(aGrid[row, col], CultureInfo.CurrentUICulture ) >= 0)
								{
									if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)							
										cellRange.Style = aGrid.Styles["Editable1"];							
									else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)							
										cellRange.Style = aGrid.Styles["Editable2"];							
								}
									//Is it just negative?
								else if (DataTag.IsEditable == false && Convert.ToDecimal(aGrid[row, col], CultureInfo.CurrentUICulture) < 0)
								{
									if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)
										cellRange.Style = aGrid.Styles["Negative1"];
									else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)
										cellRange.Style = aGrid.Styles["Negative2"];
								}
									//Is it both negative and editable?
								else if (DataTag.IsEditable == true && Convert.ToDecimal(aGrid[row, col], CultureInfo.CurrentUICulture) < 0)
								{
									if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)
										cellRange.Style = aGrid.Styles["NegativeEditable1"];
									else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)
										cellRange.Style = aGrid.Styles["NegativeEditable2"];
								}
							}
							catch //it's probably a string column
							{
								//Is it just editable?
								if (DataTag.IsEditable == true)
								{
									if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)							
										cellRange.Style = aGrid.Styles["Editable1"];							
									else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)							
										cellRange.Style = aGrid.Styles["Editable2"];							
								}
							}
						}
					}
					else //this column is not lockable (meaning it's not editable).
						//Therefore, there's no need to check every cell tag.
					{
						for (int row = 0; row < aGrid.Rows.Count; row ++)
						{
							cellRange = aGrid.GetCellRange(row, col);
							try
							{							
								//Is it negative?
								if (Convert.ToDecimal(aGrid[row, col], CultureInfo.CurrentUICulture) < 0)
								{
									if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)
										cellRange.Style = aGrid.Styles["Negative1"];
									else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)
										cellRange.Style = aGrid.Styles["Negative2"];
								}
    						}
							catch //it's probably a string column
							{
								//nothing to do, since a string col can't have negatives.
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetCellStyleG5G6(C1FlexGrid aGrid)
		{
			CellRange cellRange;
			C1FlexGrid colGrid;
			bool ExceedsCapacity = false;
			//  BEGIN MID Track #1511 Highlight Stores whose allocation is out of balance
			bool OutOfBalance = false;
			//  END MID Track #1511
			string gridStyle1                                       = aGrid.Styles["Style1"].Name;
			C1.Win.C1FlexGrid.CellStyle gridStyle1Reverse            = aGrid.Styles["Style1Reverse"];
			C1.Win.C1FlexGrid.CellStyle gridEditable1                = aGrid.Styles["Editable1"];
			C1.Win.C1FlexGrid.CellStyle gridEditable1Reverse         = aGrid.Styles["Editable1Reverse"];
			string gridStyle2                                        = aGrid.Styles["Style2"].Name;
			C1.Win.C1FlexGrid.CellStyle gridStyle2Reverse            = aGrid.Styles["Style2Reverse"];
			C1.Win.C1FlexGrid.CellStyle gridEditable2                = aGrid.Styles["Editable2"];
			C1.Win.C1FlexGrid.CellStyle gridEditable2Reverse         = aGrid.Styles["Editable2Reverse"];
			C1.Win.C1FlexGrid.CellStyle gridNegative1                = aGrid.Styles["Negative1"];
			C1.Win.C1FlexGrid.CellStyle gridNegative1Reverse         = aGrid.Styles["Negative1Reverse"];
			C1.Win.C1FlexGrid.CellStyle gridNegative2                = aGrid.Styles["Negative2"];
			C1.Win.C1FlexGrid.CellStyle gridNegative2Reverse         = aGrid.Styles["Negative2Reverse"];
			C1.Win.C1FlexGrid.CellStyle gridNegativeEditable1        = aGrid.Styles["NegativeEditable1"];
			C1.Win.C1FlexGrid.CellStyle gridNegativeEditable1Reverse = aGrid.Styles["NegativeEditable1Reverse"];
			C1.Win.C1FlexGrid.CellStyle gridNegativeEditable2        = aGrid.Styles["NegativeEditable2"];
			C1.Win.C1FlexGrid.CellStyle gridNegativeEditable2Reverse = aGrid.Styles["NegativeEditable2Reverse"];
			// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
			C1.Win.C1FlexGrid.CellStyle gridStyle1ReverseBalance            = aGrid.Styles["Style1ReverseBalance"];
			C1.Win.C1FlexGrid.CellStyle gridBalance1                        = aGrid.Styles["Balance1"];
			C1.Win.C1FlexGrid.CellStyle gridBalance2                        = aGrid.Styles["Balance2"];
			C1.Win.C1FlexGrid.CellStyle gridEditable1Balance                = aGrid.Styles["Editable1Balance"];
			C1.Win.C1FlexGrid.CellStyle gridEditable1ReverseBalance         = aGrid.Styles["Editable1ReverseBalance"];
			C1.Win.C1FlexGrid.CellStyle gridStyle2ReverseBalance            = aGrid.Styles["Style2ReverseBalance"];
			C1.Win.C1FlexGrid.CellStyle gridEditable2Balance                = aGrid.Styles["Editable2Balance"];
			C1.Win.C1FlexGrid.CellStyle gridEditable2ReverseBalance         = aGrid.Styles["Editable2ReverseBalance"];
			C1.Win.C1FlexGrid.CellStyle gridNegative1Balance                = aGrid.Styles["Negative1Balance"];
			C1.Win.C1FlexGrid.CellStyle gridNegative1ReverseBalance         = aGrid.Styles["Negative1ReverseBalance"];
			C1.Win.C1FlexGrid.CellStyle gridNegative2Balance                = aGrid.Styles["Negative2Balance"];
			C1.Win.C1FlexGrid.CellStyle gridNegative2ReverseBalance         = aGrid.Styles["Negative2ReverseBalance"];
			C1.Win.C1FlexGrid.CellStyle gridNegativeEditable1Balance        = aGrid.Styles["NegativeEditable1Balance"];
			C1.Win.C1FlexGrid.CellStyle gridNegativeEditable1ReverseBalance = aGrid.Styles["NegativeEditable1ReverseBalance"];
			C1.Win.C1FlexGrid.CellStyle gridNegativeEditable2Balance        = aGrid.Styles["NegativeEditable2Balance"];
			C1.Win.C1FlexGrid.CellStyle gridNegativeEditable2ReverseBalance = aGrid.Styles["NegativeEditable2ReverseBalance"];
			// END MID Track #1511
			
			try
			{
				colGrid = GetColumnGrid(aGrid);
				bool gridIsPositiveDecimalValue;
				bool gridIsString;
				string gridCell;
				//Loop through the entire sheet and pick out the negatives and the editables.
				for (int row = 0; row < aGrid.Rows.Count; row ++)
				{
					ExceedsCapacity = StoreExceedsCapacity(row);
					// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
					OutOfBalance = this.StoreAllocationOutOfBalance (row);
					// END MID Track #1511

					for (int col = 0; col < aGrid.Cols.Count; col ++)
					{
						cellRange = aGrid.GetCellRange(row, col);

						TagForColumn ColumnTag = (TagForColumn)colGrid.Cols[col].UserData;
						gridCell = aGrid[row, col].ToString();
						if (MIDMath.IsNumber(gridCell))
						{
							gridIsString = false;
							gridIsPositiveDecimalValue =
								MIDMath.IsPositiveNumber(gridCell);
						}
						else
						{
							gridIsString = true;
							gridIsPositiveDecimalValue = false;
						}
						if (ColumnTag.IsLockable == true) //Lockable means some cells are editable.
							//We need to check every cell in this column to see whether
							//it's just editable, or just negative, or both.
						{
							TagForGridData DataTag = (TagForGridData)cellRange.UserData;
							if (gridIsString)
							{	//Is it just editable?
								if (DataTag.IsEditable == true)
								{
									//  BEGIN MID Track #1511 Highlight Stores whose allocation is out of balance
									//									if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)	
									//									{
									//										if (ExceedsCapacity)
									//											cellRange.Style = aGrid.Styles["Editable1Reverse"];
									//										else
									//											cellRange.Style = aGrid.Styles["Editable1"];	
									//									}
									//									else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)	
									//									{
									//										if (ExceedsCapacity)
									//											cellRange.Style = aGrid.Styles["Editable2Reverse"];
									//										else
									//											cellRange.Style = aGrid.Styles["Editable2"];	
									//									}
									if (cellRange.Style.Name == gridStyle1)	
									{
										if (ExceedsCapacity)
										{
											if (OutOfBalance)
											{
												cellRange.Style = gridEditable1ReverseBalance;
											}
											else
											{
												cellRange.Style = gridEditable1Reverse;
											}
										}
										else if (OutOfBalance)
										{
											cellRange.Style = gridEditable1Balance;
										}
										else
										{
											cellRange.Style = gridEditable1;
										}
									}
									else if (cellRange.Style.Name == gridStyle2)	
									{
										if (ExceedsCapacity)
										{
											if (OutOfBalance)
											{
												cellRange.Style = gridEditable2ReverseBalance;
											}
											else
											{
												cellRange.Style = gridEditable2Reverse;
											}
										}
										else if (OutOfBalance)
										{
											cellRange.Style = gridEditable2Balance;
										}
										else
										{
											cellRange.Style = gridEditable2;	
										}
									}
									//  END MID Track #1511
								}
							}
							else
							{
								if (gridIsPositiveDecimalValue)
								{  //Is it just editable?
									if (DataTag.IsEditable == true)
									{
										// BEGIN MID Track #1511 Highlight Stores whose allocation is out of balance
										//										if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)
										//										{	
										//											if (ExceedsCapacity)
										//												cellRange.Style = aGrid.Styles["Editable1Reverse"];
										//											else
										//												cellRange.Style = aGrid.Styles["Editable1"];	
										//										}
										//										else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)	
										//										{
										//											if (ExceedsCapacity)
										//												cellRange.Style = aGrid.Styles["Editable2Reverse"];
										//											else
										//												cellRange.Style = aGrid.Styles["Editable2"];	
										//										}
										if (cellRange.Style.Name == gridStyle1)
										{	
											if (ExceedsCapacity)
											{
												if (OutOfBalance)
												{
													cellRange.Style = gridEditable1ReverseBalance;
												}
												else
												{
													cellRange.Style = gridEditable1Reverse;
												}
											}
											else if (OutOfBalance)
											{
												cellRange.Style = gridEditable1Balance;
											}
											else
											{
                                                cellRange.Style = gridEditable1;
											}
										}
										else if (cellRange.Style.Name == gridStyle2)	
										{
											if (ExceedsCapacity)
											{
												if (OutOfBalance)
												{
													cellRange.Style = gridEditable2ReverseBalance;
												}
												else
												{
													cellRange.Style = gridEditable2Reverse;
												}
											}
											else if (OutOfBalance)
											{
												cellRange.Style = gridEditable2Balance;
											}
											else
											{
												cellRange.Style = gridEditable2;
											}
										}
										// END MID Track #1511
									}
								}
								else
								{   //Is it just negative?
									if (DataTag.IsEditable == false)
									{
										// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
										//										if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)
										//										{
										//											if (ExceedsCapacity)
										//												cellRange.Style = aGrid.Styles["Negative1Reverse"];
										//											else
										//												cellRange.Style = aGrid.Styles["Negative1"];
										//										}
										//										else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)
										//										{
										//											if (ExceedsCapacity)
										//												cellRange.Style = aGrid.Styles["Negative2Reverse"];
										//											else
										//												cellRange.Style = aGrid.Styles["Negative2"];
										//										}
										if (cellRange.Style.Name == gridStyle1)
										{
											if (ExceedsCapacity)
											{
												if (OutOfBalance)
												{
													cellRange.Style = gridNegative1ReverseBalance;
												}
												else
												{
													cellRange.Style = gridNegative1Reverse;
												}
											}
											else if (OutOfBalance)
											{
												cellRange.Style = gridNegative1Balance;
											}
											else
											{
												cellRange.Style = gridNegative1;
											}
										}
										else if (cellRange.Style.Name == gridStyle2)
										{
											if (ExceedsCapacity)
											{
												if (OutOfBalance)
												{
													cellRange.Style = gridNegative2ReverseBalance;
												}
												else
												{
													cellRange.Style = gridNegative2Reverse;
												}
											}
											else if (OutOfBalance)
											{
												cellRange.Style = gridNegative2Balance;
											}
											else
											{
												cellRange.Style = gridNegative2;
											}
										}
										// END MID Track #1511 

									}
									else
									{  //Is it both negative and editable?
										if (DataTag.IsEditable == true)
										{
											// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
											//											if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)
											//											{
											//												if (ExceedsCapacity)
											//													cellRange.Style = aGrid.Styles["NegativeEditable1Reverse"];
											//												else
											//													cellRange.Style = aGrid.Styles["NegativeEditable1"];
											//											}
											//											else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)
											//											{
											//												if (ExceedsCapacity)
											//													cellRange.Style = aGrid.Styles["NegativeEditable2Reverse"];
											//												else 
											//													cellRange.Style = aGrid.Styles["NegativeEditable2"];
											//											}
											if (cellRange.Style.Name == gridStyle1)
											{
												if (ExceedsCapacity)
												{
													if (OutOfBalance)
													{
														cellRange.Style = gridNegativeEditable1ReverseBalance;
													}
													else
													{
														cellRange.Style = gridNegativeEditable1Reverse;
													}
												}
												else if (OutOfBalance)
												{
													cellRange.Style = gridNegativeEditable1Balance;
												}
												else
												{
													cellRange.Style = gridNegativeEditable1;
												}
											}
											else if (cellRange.Style.Name == gridStyle2)
											{
												if (ExceedsCapacity)
												{
													if (OutOfBalance)
													{
														cellRange.Style = gridNegativeEditable2ReverseBalance;
													}
													else
													{
														cellRange.Style = gridNegativeEditable2Reverse;
													}
												}
												else if (OutOfBalance)
												{
													cellRange.Style = gridNegativeEditable2Balance;
												}
												else
												{
													cellRange.Style = gridNegativeEditable2;
												}
											}
											// END MID Track #1511
										}
									}
								}
							}
						}							
						else //this column is not lockable (meaning it's not editable).
							//Therefore, there's no need to check every cell tag.
						{ 
							if (gridIsString)
							{
								// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
								//								if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)
								//								{
								//									if (ExceedsCapacity)
								//										cellRange.Style = aGrid.Styles["Style1Reverse"];
								//								}
								//								else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)
								//								{
								//									if (ExceedsCapacity)
								//										cellRange.Style = aGrid.Styles["Style2Reverse"];
								//								}
								if (cellRange.Style.Name == gridStyle1)
								{
									if (ExceedsCapacity)
									{
										if (OutOfBalance)
										{
											cellRange.Style = gridStyle1ReverseBalance;
										}
										else
										{
											cellRange.Style = gridStyle1Reverse;
										}
									}
									else if (OutOfBalance)
									{
										cellRange.Style = gridBalance1;
									}
								}
								else if (cellRange.Style.Name == gridStyle2)
								{
									if (ExceedsCapacity)
									{
										if (OutOfBalance)
										{
											cellRange.Style = gridStyle2ReverseBalance;
										}
										else
										{
											cellRange.Style = gridStyle2Reverse;
										}
									}
									else if (OutOfBalance)
									{
										cellRange.Style = gridBalance2;
									}
								}		
								// END MID Track #1511
							}
							else
							{   //Is it negative?
								if (gridIsPositiveDecimalValue)
								{
									// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
									//									if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)
									//									{
									//										if (ExceedsCapacity)
									//											cellRange.Style = aGrid.Styles["Style1Reverse"];
									//									}
									//									else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)
									//									{
									//										if (ExceedsCapacity)
									//											cellRange.Style = aGrid.Styles["Style2Reverse"];
									//									}		
									if (cellRange.Style.Name == gridStyle1)
									{
										if (ExceedsCapacity)
										{
											if (OutOfBalance)
											{
												cellRange.Style = gridStyle1ReverseBalance;
											}
											else
											{
												cellRange.Style = gridStyle1Reverse;
											}
										}
										else if (OutOfBalance)
										{
											cellRange.Style = gridBalance1;
										}
									}
									else if (cellRange.Style.Name == gridStyle2)
									{
										if (ExceedsCapacity)
										{
											if (OutOfBalance)
											{
												cellRange.Style = gridStyle2ReverseBalance;
											}
											else
											{
												cellRange.Style = gridStyle2Reverse;
											}
										}
										else if (OutOfBalance)
										{
											cellRange.Style = gridBalance2;
										}
									}	
									// END MID Track #1511
								}
								else
								{
									// BEGIN MID Track #1511 Highlight Stores whose allocation is out of balance
									//									if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)
									//									{
									//										if (ExceedsCapacity)
									//											cellRange.Style = aGrid.Styles["Negative1Reverse"];
									//										else
									//											cellRange.Style = aGrid.Styles["Negative1"];
									//									}
									//									else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)
									//									{
									//										if (ExceedsCapacity)
									//											cellRange.Style = aGrid.Styles["Negative2Reverse"];
									//										else
									//											cellRange.Style = aGrid.Styles["Negative2"];
									//									}
									if (cellRange.Style.Name == gridStyle1)
									{
										if (ExceedsCapacity)
										{
											if (OutOfBalance)
											{
												cellRange.Style = gridNegative1ReverseBalance;
											}
											else
											{
												cellRange.Style = gridNegative1Reverse;
											}
										}
										else if (OutOfBalance)
										{
											cellRange.Style = gridNegative1Balance;
										}
										else
										{
											cellRange.Style = gridNegative1;
										}
									}
									else if (cellRange.Style.Name == gridStyle2)
									{
										if (ExceedsCapacity)
										{
											if (OutOfBalance)
											{
												cellRange.Style = gridNegative2ReverseBalance;
											}
											else
											{
												cellRange.Style = gridNegative2Reverse;
											}
										}
										else if (OutOfBalance)
										{
											cellRange.Style = gridNegative2Balance;
										}
										else
										{
											cellRange.Style = gridNegative2;
										}
									}
									// END MID Track #1511
								}
							}
						}
						//						if (gridIsPositiveDecimalValue)
						//						{
						//							eVelocityRuleType vrt = (eVelocityRuleType)(Convert.ToInt32(gridCell, CultureInfo.CurrentUICulture));
						//							if (Enum.IsDefined(typeof(eVelocityRuleType),vrt))
						//							{
						//								cellRange.Style.DataMap = _ldRulesVelocity;
						//							}
						//						}	
					}
					
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		 
		private void SetCellStyleG8G12(C1FlexGrid aGrid)
		{
			C1FlexGrid colGrid;
			colGrid = GetColumnGrid(aGrid);
			CellRange cellRange;
			try
			{
				//Loop through the entire sheet and pick out the negatives and the editables.
				for (int row = 0; row < aGrid.Rows.Count; row ++)
				{
					for (int col = 0; col < aGrid.Cols.Count; col ++)
					{
						cellRange = aGrid.GetCellRange(row, col);
						TagForGridData DataTag = (TagForGridData)cellRange.UserData;
						try
						{
							//Is it just editable?
							if (DataTag.IsEditable == true && Convert.ToDecimal(aGrid[row, col], CultureInfo.CurrentUICulture ) >= 0)
							{
								if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)							
									cellRange.Style = aGrid.Styles["Editable1"];							
								else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)							
									cellRange.Style = aGrid.Styles["Editable2"];							
							}
								//Is it just negative?
							else if (DataTag.IsEditable == false && Convert.ToDecimal(aGrid[row, col], CultureInfo.CurrentUICulture) < 0)
							{
								if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)
									cellRange.Style = aGrid.Styles["Negative1"];
								else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)
									cellRange.Style = aGrid.Styles["Negative2"];
							}
								//Is it both negative and editable?
							else if (DataTag.IsEditable == true && Convert.ToDecimal(aGrid[row, col], CultureInfo.CurrentUICulture) < 0)
							{
								if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)
									cellRange.Style = aGrid.Styles["NegativeEditable1"];
								else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)
									cellRange.Style = aGrid.Styles["NegativeEditable2"];
							}
						}
						catch //it's probably a string column
						{
							//Is it just editable?
							if (DataTag.IsEditable == true)
							{
								if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)							
									cellRange.Style = aGrid.Styles["Editable1"];							
								else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)							
									cellRange.Style = aGrid.Styles["Editable2"];							
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		 
		private C1FlexGrid GetRowGrid(C1FlexGrid aGrid)
		{
			C1FlexGrid rowGrid = null;
			int whichGrid;
			try
			{
				whichGrid = ((GridTag)aGrid.Tag).GridId;
				switch((FromGrid)whichGrid)
				{
					case FromGrid.g5:
					case FromGrid.g6:
						rowGrid = g4;
						break;
					case FromGrid.g8:
					case FromGrid.g9:
						rowGrid = g7;
						break;
					case FromGrid.g11:
					case FromGrid.g12:
						rowGrid = g10;
						break;
					default:
						rowGrid = null;
						break;
				}
				return rowGrid;
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return rowGrid;
			}
		}

		private C1FlexGrid GetColumnGrid(C1FlexGrid aGrid)
		{
			C1FlexGrid colGrid = null;
			int whichGrid;
			try
			{
				whichGrid = ((GridTag)aGrid.Tag).GridId;
				switch((FromGrid)whichGrid)
				{
					case FromGrid.g4:
					case FromGrid.g7:
					case FromGrid.g10:
						colGrid = g1;
						break;
					case FromGrid.g5:
					case FromGrid.g8:
					case FromGrid.g11:
						colGrid = g2;
						break;
					case FromGrid.g6:
					case FromGrid.g9:
					case FromGrid.g12:
						colGrid = g3;
						break;
					default:
						colGrid = null;
						break;
				}
				return colGrid;
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return colGrid;
			}
		}

		private void btnThemeProperties_Click(object sender, System.EventArgs e)
		{
			_frmThemeProperties = new ThemeProperties(_theme);
			_frmThemeProperties.ApplyButtonClicked += new EventHandler(StylePropertiesOnChanged);
			_frmThemeProperties.StartPosition = FormStartPosition.CenterParent;

			if (_frmThemeProperties.ShowDialog() == DialogResult.OK)
			{
				StylePropertiesChanged();
			}
		}
		private void StylePropertiesOnChanged(object sender, System.EventArgs e)
		{
			//this procedure handles the "Apply" button click event from the
			//StyleProperties form.
			StylePropertiesChanged();
		}
		private void StylePropertiesChanged()
		{
			Cursor.Current = Cursors.WaitCursor;

			_sab.ClientServerSession.Theme = _frmThemeProperties.CurrentTheme;

			_loading = true;
			_themeChanged = true;
			ChangeStyle();
			_loading = false;
			_themeChanged = false;
			Cursor.Current = Cursors.Default;
		}

		private void g3_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
            //if (!((C1FlexGrid)sender).Redraw)
            if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
            {
                return;
            }
            // End Track #6371

			//if (e.Row != 2) return; //we're only interested in the third row.
			
			e.DrawCell();

			//If there's no need to display the vertical group border, we're done.
			if (!_theme.DisplayColumnGroupDivider) 
				return;
			else
			{
				//Otherwise, jump to the last column of each group and put a vertical border
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, _theme.DividerWidth,1,1);

				rec = e.Bounds;
				rec.X = rec.Right - m.Right;
				rec.Width = m.Right;

				//if ((e.Col + 1)%(g3.Cols.Count / HeaderOrComponentGroups) == 0)
				//	g.FillRectangle(_theme.ColumnGroupDividerBrush, rec);
				
				if ( (e.Col < g3.Cols.Count - 1) && Convert.ToString(g3.GetData(0, e.Col)) !=
					Convert.ToString(g3.GetData(0, e.Col+1)) )
					g.FillRectangle(_theme.ColumnGroupDividerBrush, rec);
			}
		}
		private void g4_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
            //if (!((C1FlexGrid)sender).Redraw)
            if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
            {
                return;
            }
            // End Track #6371

			if (g4.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup4 + rowsPerStoreGroup)%rowsPerStoreGroup == 0) 
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				g.FillRectangle(_theme.RowGroupRowHeaderDividerBrush, rec);
			}
			else if (_theme.ViewStyle == StyleEnum.Chiseled &&
				((e.Row + 1 + LastInvisibleRowsPerStoreGroup4)%(rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup4 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + LastInvisibleRowsPerStoreGroup4 + 1)%(rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup4 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}
		}		
		private void g5_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
            //if (!((C1FlexGrid)sender).Redraw)
            if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
            {
                return;
            }
            // End Track #6371

			if (g5.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup4 + rowsPerStoreGroup)%rowsPerStoreGroup == 0) 
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				g.FillRectangle(_theme.RowGroupDividerBrush, rec);
			}
			else if (_theme.ViewStyle == StyleEnum.Chiseled &&
				((e.Row + 1 + LastInvisibleRowsPerStoreGroup4)%(rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup4 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup4)%(rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup4 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}
		}
		private void g6_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
            //if (!((C1FlexGrid)sender).Redraw)
            if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
            {
                return;
            }
            // End Track #6371

           
			if (g6.GetCellStyle(e.Row, e.Col) == null) return;
         
            e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup4 + rowsPerStoreGroup)%rowsPerStoreGroup == 0) 
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				g.FillRectangle(_theme.RowGroupDividerBrush, rec);
			}
			else if (_theme.ViewStyle == StyleEnum.Chiseled &&
				((e.Row + 1 + LastInvisibleRowsPerStoreGroup4)%(rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup4 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup4)%(rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup4 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}

			//If there's no need to display the vertical group border, we're done.
			if (_theme.DisplayColumnGroupDivider == false) 
				return;
			else
			{
				//Otherwise, jump to the last column of each group and put a vertical border
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, _theme.DividerWidth,1,1);

				rec = e.Bounds;
				rec.X = rec.Right - m.Right;
				rec.Width = m.Right;
				
				//if ((e.Col + 1)%(g6.Cols.Count / HeaderOrComponentGroups) == 0)
				//	g.FillRectangle(_theme.ColumnGroupDividerBrush, rec);
				if ( (e.Col < g3.Cols.Count - 1) && Convert.ToString(g3.GetData(0, e.Col)) !=
					Convert.ToString(g3.GetData(0, e.Col+1)) )
					g.FillRectangle(_theme.ColumnGroupDividerBrush, rec);
			}
		}
		private void g7_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
            //if (!((C1FlexGrid)sender).Redraw)
            if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
            {
                return;
            }
            // End Track #6371

			if (g7.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%rowsPerStoreGroup == 0) 
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				g.FillRectangle(_theme.RowGroupRowHeaderDividerBrush, rec);
			}
			else if (_theme.ViewStyle == StyleEnum.Chiseled &&
				((e.Row + 1 + LastInvisibleRowsPerStoreGroup7)%(rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup7)%(rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}
		}
		private void g8_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
            //if (!((C1FlexGrid)sender).Redraw)
            if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
            {
                return;
            }
            // End Track #6371

			if (g8.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%rowsPerStoreGroup == 0) 
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				g.FillRectangle(_theme.RowGroupDividerBrush, rec);
			}
			else if (_theme.ViewStyle == StyleEnum.Chiseled &&
				((e.Row + 1 + LastInvisibleRowsPerStoreGroup7)%(rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup7)%(rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}
		}
		private void g9_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
            //if (!((C1FlexGrid)sender).Redraw)
            if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
            {
                return;
            }
            // End Track #6371

			if (g9.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%rowsPerStoreGroup == 0) 
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				g.FillRectangle(_theme.RowGroupDividerBrush, rec);
			}
			else if (_theme.ViewStyle == StyleEnum.Chiseled &&
				((e.Row + 1 + LastInvisibleRowsPerStoreGroup7)%(rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup7)%(rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}

			//If there's no need to display the vertical group border, we're done.
			if (_theme.DisplayColumnGroupDivider == false) 
				return;
			else
			{
				//Otherwise, jump to the last column of each group and put a vertical border
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, _theme.DividerWidth,1,1);

				rec = e.Bounds;
				rec.X = rec.Right - m.Right;
				rec.Width = m.Right;

				//if ((e.Col + 1)%(g9.Cols.Count / HeaderOrComponentGroups) == 0)
				//	g.FillRectangle(_theme.ColumnGroupDividerBrush, rec);
				if ( (e.Col < g3.Cols.Count - 1) && Convert.ToString(g3.GetData(0, e.Col)) !=
					Convert.ToString(g3.GetData(0, e.Col+1)) )
					g.FillRectangle(_theme.ColumnGroupDividerBrush, rec);
			}
		}
		private void g10_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
            //if (!((C1FlexGrid)sender).Redraw)
            if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
            {
                return;
            }
            // End Track #6371

			if (g10.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup10 + rowsPerStoreGroup)%rowsPerStoreGroup == 0) 
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				g.FillRectangle(_theme.RowGroupRowHeaderDividerBrush, rec);
			}
			else if (_theme.ViewStyle == StyleEnum.Chiseled &&
				((e.Row + 1 + LastInvisibleRowsPerStoreGroup10)%(rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup10 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup10)%(rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup10 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}
		}
		private void g11_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
            //if (!((C1FlexGrid)sender).Redraw)
            if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
            {
                return;
            }
            // End Track #6371

			if (g11.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup10 + rowsPerStoreGroup)%rowsPerStoreGroup == 0) 
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				g.FillRectangle(_theme.RowGroupDividerBrush, rec);
			}
			else if (_theme.ViewStyle == StyleEnum.Chiseled &&
				((e.Row + 1 + LastInvisibleRowsPerStoreGroup10)%(rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup10 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup10)%(rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup10 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}
		}
		private void g12_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            // Begin Track #6371 - KJohnson - Sorting in SKU Review is slow
            //if (!((C1FlexGrid)sender).Redraw)
            if (!_currentRedrawState || !((C1FlexGrid)sender).Redraw)
            {
                return;
            }
            // End Track #6371

			if (g12.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup10 + rowsPerStoreGroup)%rowsPerStoreGroup == 0) 
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				g.FillRectangle(_theme.RowGroupDividerBrush, rec);
			}
			else if (_theme.ViewStyle == StyleEnum.Chiseled &&
				((e.Row + 1 + LastInvisibleRowsPerStoreGroup10)%(rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup10 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup10)%(rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup10 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}

			//If there's no need to display the vertical group border, we're done.
			if (_theme.DisplayColumnGroupDivider == false) 
				return;
			else
			{
				//Otherwise, jump to the last column of each group and put a vertical border
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, _theme.DividerWidth,1,1);

				rec = e.Bounds;
				rec.X = rec.Right - m.Right;
				rec.Width = m.Right;

				//if ((e.Col + 1)%(g12.Cols.Count / HeaderOrComponentGroups) == 0)
				//	g.FillRectangle(_theme.ColumnGroupDividerBrush, rec);
				if ( (e.Col < g3.Cols.Count - 1) && Convert.ToString(g3.GetData(0, e.Col)) !=
					Convert.ToString(g3.GetData(0, e.Col+1)) )
					g.FillRectangle(_theme.ColumnGroupDividerBrush, rec);
			}
		}
		//		private void DrawRows(C1FlexGrid aGrid,	SolidBrush aRowBrush,int aRowsPerGroup,	OwnerDrawCellEventArgs e)
		//		{
		//			Rectangle rectangle;
		//			Graphics graphics;
		//			Margins margins;
		//			C1FlexGrid rowHeaderGrid;
		//
		//			try
		//			{
		//				rowHeaderGrid = ((GridTag)aGrid.Tag).RowHeaderGrid;
		//
		//				if ((_theme.ViewStyle == StyleEnum.Plain || 
		//					_theme.ViewStyle == StyleEnum.AlterColors ||
		//					_theme.ViewStyle == StyleEnum.HighlightName) && 
		//					_theme.DisplayRowGroupDivider == true &&
		//					e.Row % aRowsPerGroup == aRowsPerGroup - 1) 
		//				{
		//					graphics = e.Graphics;
		//					margins = new Margins(1, 1, 1, _theme.DividerWidth);
		//
		//					rectangle = e.Bounds;
		//					rectangle.Y = rectangle.Bottom - margins.Bottom;
		//					rectangle.Height = margins.Bottom;
		//
		//					graphics.FillRectangle(aRowBrush, rectangle);
		//				}
		//				else if (_theme.ViewStyle == StyleEnum.Chiseled &&
		//					e.Row % aRowsPerGroup == aRowsPerGroup - 1) 
		//				{
		//					graphics = e.Graphics;
		//					margins = new Margins(1, 1, 1, _theme.DividerWidth);
		//
		//					rectangle = e.Bounds;
		//					rectangle.Y = rectangle.Bottom - margins.Bottom;
		//					rectangle.Height = margins.Bottom;
		//
		//					if (e.Row % (aRowsPerGroup * 2) == aRowsPerGroup - 1)
		//					{
		//						graphics.FillRectangle(_theme.ChiselLowerBrush, rectangle);
		//					}
		//					else
		//					{
		//						graphics.FillRectangle(_theme.ChiselUpperBrush, rectangle);
		//					}
		//				}
		//			}
		//			catch (Exception ex)
		//			{
		//				HandleException(exc);
		//			}
		//		}

		//		private void DrawColBorders(C1FlexGrid aGrid, OwnerDrawCellEventArgs e)
		//		{
		//			Rectangle rectangle;
		//			Graphics graphics;
		//			Margins margins;
		//
		//			try
		//			{
		//				if (_theme.DisplayColumnGroupDivider) 
		//				{
		//					graphics = e.Graphics;
		//					margins = new Margins(1, _theme.DividerWidth,1,1);
		//
		//					rectangle = e.Bounds;
		//					rectangle.X = rectangle.Right - margins.Right;
		//					rectangle.Width = margins.Right;
		//
		//					if ((e.Col + 1) % ((GridTag)g3.Tag).DetailsPerGroup == 0)
		//					{
		//						graphics.FillRectangle(_theme.ColumnGroupDividerBrush, rectangle);
		//					}
		//				}
		//			}
		//			catch (Exception ex)
		//			{
		//				HandleException(exc);
		//			}
		//		}

		#endregion

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboAction_SelectionChangeCommitted(object sender, System.EventArgs e)
        private void cboAction_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			int val = Convert.ToInt32(cboAction.SelectedValue, CultureInfo.CurrentUICulture);
			btnProcess.Enabled = val != 0;
		}

		private void btnProcess_Click(object sender, System.EventArgs e)
		{
			bool aReviewFlag, aUseSystemTolerancePercent;
			double aTolerancePercent; 
			int aStoreFilter, aWorkFlowStepKey;
			try
			{
				// see if an ACTION has been selected
				int action = Convert.ToInt32(cboAction.SelectedValue, CultureInfo.CurrentUICulture);
				if (action != -1)
				{
                    //BEGIN TT#793-MD-DOConnell - Ran balance size billaterally on a receipt header in an assortment and receive a null reference exception
                    if (!_loadedFromAssrt)
                    {
                        if (!OKToProcess((eAllocationActionType)action))
                            return;
                    }
                    else
                    {
                        if (!OKToProcessAssortment((eAllocationActionType)action))
                            return;
                    }
                    //END TT#793-MD-DOConnell - Ran balance size billaterally on a receipt header in an assortment and receive a null reference exception

					Cursor.Current = Cursors.WaitCursor;
					// set ACTION in transaction
					ApplicationBaseAction aMethod = _trans.CreateNewMethodAction((eMethodType) action);
					GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
					aReviewFlag = false;
					aUseSystemTolerancePercent = true;
					aTolerancePercent = Include.DefaultBalanceTolerancePercent;
					 
					if (_trans.AllocationFilterID != Include.NoRID)
						aStoreFilter = _trans.AllocationFilterID;
					else
						aStoreFilter = Include.AllStoreFilterRID;
					
					aWorkFlowStepKey = -1;
					AllocationWorkFlowStep aAllocationWorkFlowStep 
						= new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
					_trans.DoAllocationAction(aAllocationWorkFlowStep);
				
					eAllocationActionStatus actionStatus = _trans.AllocationActionAllHeaderStatus;
					string message = MIDText.GetTextOnly((int)actionStatus);
					MessageBox.Show(message,_thisTitle,MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // begin TT#241 - MD - JEllis - Header Enqueue Process
                    if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
                        || actionStatus == eAllocationActionStatus.NoHeaderResourceLocks)
                    {
                        ErrorFound = true;
                        CloseOtherViews();
                        if (_trans.StyleView != null)
                        {
                            Close();
                        }
                    }
                    else
                    {
                        // end TT#241 - MD - JEllis - Header Enqueue Process

                        // Start TT#2068-MD TT#2070-MD - AGallagher - After balance issues
                        if (!_trans.UseAssortmentSelectedHeaders)  
                        {
                        // End TT#2068-MD TT#2070-MD - AGallagher - After balance issues
                            _trans.UpdateAllocationViewSelectionHeaders();
                            _trans.NewCriteriaHeaderList();
                            _headerList = (AllocationHeaderProfileList)_trans.GetMasterProfileList(eProfileType.AllocationHeader);
                        }  // TT#2068-MD TT#2070-MD - AGallagher - After balance issues
                      
                        // BEGIN MID Track #2643 - error closing SKU review
                        UpdateAllocationWorkspace();
                        if (action == (int)eAllocationActionType.BackoutAllocation)
                            _trans.RebuildWafers();

                        UpdateOtherViews(); // TT#241 - MD - JEllis - Header Enqueue Process
                        if (!FormIsClosing)
                        // Begin TT#3968 - RMatelic - Style View is off when processing need in style review actions
                        //CriteriaChanged();
                        {
                            CriteriaChanged();
                            vScrollBar2.Value = 0;
                        }
                        // End TT#3968
                    }   // TT#241 - MD - JEllis - Header Enqueue Process

					//Cursor.Current = Cursors.WaitCursor;
					//UpdateAllocationWorkspace();
					// END MID Track #2643  
					Cursor.Current = Cursors.Default;
				}
			}
            // BEGIN TT#1605 - AGallagher - Balance Size with Constriants when receive the warning message 
            catch (MIDException MIDexc)
            {
                if (MIDexc.ErrorNumber == (int)(eMIDTextCode.msg_al_LockTotalExceedsNew))
                {
                    UpdateOtherViews();
                    if (!FormIsClosing)
                        CriteriaChanged();
                    HandleException(MIDexc);
                }
                else
                { HandleException(MIDexc); }
            }
            // END TT#1605 - AGallagher - Balance Size with Constriants when receive the warning message 
				//			catch (MIDException MIDexc)
				//			{
				//				switch (MIDexc.ErrorLevel)
				//				{
				//					case eErrorLevel.fatal:
				//						HandleException(MIDexc);
				//						break;
				//
				//					case eErrorLevel.information:
				//					case eErrorLevel.warning:	
				//					case eErrorLevel.severe:
				//						HandleMIDException(MIDexc);
				//						break;
				//				}
				//			}
			catch ( Exception ex )
			{
				HandleException(ex);
			}
		
		}
		private bool OKToProcess (eAllocationActionType  aAction)
		{
			string errorMessage = string.Empty;
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
			string errorParm = string.Empty;
// (CSMITH) - END MID Track #3219
			bool okToProcess = true;
			eHeaderAllocationStatus headerStatus;
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
			if (_masterKeyList == null)
			{
				_masterKeyList = new ArrayList();
			}
// (CSMITH) - END MID Track #3219
			foreach (AllocationHeaderProfile ahp in _headerList)
			{
//				if ( (ahp.IsDummy || ahp.IsPurchaseOrder) && aAction == eAllocationActionType.Release)
				if (!_sab.ClientServerSession.GlobalOptions.IsReleaseable(ahp.HeaderType) && aAction == eAllocationActionType.Release)
				{
					okToProcess = false;
					if (ahp.IsDummy)
						errorMessage = 	MIDText.GetText(eMIDTextCode.msg_al_CannotReleaseDummy) + ahp.HeaderID;
					else
						errorMessage = MIDText.GetText(eMIDTextCode.msg_al_HeaderTypeCanNotBeReleased) + ": " + ahp.HeaderID;
					
					MessageBox.Show (errorMessage, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				}
                // Begin TT#1966-MD - JSmith - DC Fulfillment
                else if (ahp.IsMasterHeader && aAction == eAllocationActionType.Release)
                {
                    okToProcess = false;
                    errorMessage = MIDText.GetText(eMIDTextCode.msg_al_CannotReleaseMaster) + ahp.HeaderID;

                    MessageBox.Show(errorMessage, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                // End TT#1966-MD - JSmith - DC Fulfillment
				else
				{
					headerStatus = _trans.GetHeaderAllocationStatus(ahp.Key);
					switch (headerStatus)
					{
						case eHeaderAllocationStatus.ReceivedOutOfBalance:
							okToProcess = false;
							break;
						case eHeaderAllocationStatus.ReleaseApproved:
						case eHeaderAllocationStatus.Released:
							if (aAction != eAllocationActionType.Reset &&
								aAction != eAllocationActionType.Release)
							{
								okToProcess = false;
							}
							break;
						default:
							if (aAction == eAllocationActionType.Reset)
							{
								okToProcess = false;
							}
							else if (aAction == eAllocationActionType.ChargeIntransit)
							{
								if (   headerStatus != eHeaderAllocationStatus.AllInBalance
									&& headerStatus != eHeaderAllocationStatus.AllocatedInBalance)
									okToProcess = false;
							}
							break;
					}
				}	 
				if (!okToProcess)
				{
                    errorMessage = string.Format
                        (MIDText.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction),
                        MIDText.GetTextOnly((int)headerStatus));                   // TT#3225 - AnF - Jellis - Size VSW Onhand Out of Synce with Color
                    //MIDText.GetTextOnly((int)ahp.HeaderAllocationStatus) );  // TT#3225 - AnF - Jellis - Size VSW Onhand Out of Synce with Color
						
					MessageBox.Show (errorMessage, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				}
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
				if (okToProcess) 
				{
                    // Begin TT#1966-MD - JSmith- DC Fulfillment
                    if (ahp.IsMasterHeader
                            && ahp.DCFulfillmentProcessed)
                    {
                        errorParm = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed);
                        errorParm = errorParm.Replace("{0}", ahp.HeaderID);

                        errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_ActionNotAllowed), errorParm);

                        MessageBox.Show(errorMessage, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        okToProcess = false;

                        break;
                    }
                    else if (ahp.IsSubordinateHeader
                            && !ahp.DCFulfillmentProcessed)
                    {
                        errorParm = MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed);
                        errorParm = errorParm.Replace("{0}", ahp.HeaderID);

                        errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_ActionNotAllowed), errorParm);

                        MessageBox.Show(errorMessage, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        okToProcess = false;

                        break;
                    }
                    // End TT#1966-MD - JSmith- DC Fulfillment
					if (aAction == eAllocationActionType.BackoutAllocation)
					{
                        // Begin TT#1966-MD - JSmith- DC Fulfillment
                        //int subordRID = Include.NoRID;

                        //string subordID = string.Empty;

                        //Header header = new Header();

                        //subordRID = header.GetSubordForMaster(ahp.Key);

                        //if (subordRID != Include.NoRID)
                        //{
                        //    subordID = header.GetSubordinateID(subordRID);

                        //    errorParm = MIDText.GetTextOnly(eMIDTextCode.msg_al_MasterStillAssigned);
                        //    errorParm = errorParm.Replace("{0}", ahp.HeaderID);
                        //    errorParm = errorParm.Replace("{1}", subordID);

                        //    errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_ActionNotAllowed), errorParm);

                        //    MessageBox.Show (errorMessage, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //    okToProcess = false;

                        //    break;
                        //}
                        // End TT#1966-MD - JSmith- DC Fulfillment
					}
                    // Begin TT#785 - Header Load Interfacing a transaction  trying to Modify a WUB header with a PO type
                    else if (aAction == eAllocationActionType.ReapplyTotalAllocation && ahp.HeaderType == eHeaderType.WorkupTotalBuy)
                    {
                        errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_CannotPerformActionOnHeader),
                                        MIDText.GetTextOnly((int)aAction),
                                        ahp.HeaderID,
                                        MIDText.GetTextOnly((int)ahp.HeaderType));
                        MessageBox.Show(errorMessage, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        okToProcess = false;
                        break;
                    }
                    // End TT#785  
				}

				if (okToProcess)
				{
					int masterRID = Include.NoRID;

					Header header = new Header();

					masterRID = header.GetMasterForSubord(ahp.Key);

					if (masterRID != Include.NoRID)
					{
						if (!_masterKeyList.Contains(masterRID))
						{
							_masterKeyList.Add(masterRID);
						}
					}
				}
// (CSMITH) - END MID Track #3219
			}
			if (okToProcess)
			{	
				if (   aAction == eAllocationActionType.BackoutAllocation 
					|| aAction == eAllocationActionType.BackoutSizeAllocation
					|| aAction == eAllocationActionType.BackoutSizeIntransit
					|| aAction == eAllocationActionType.BackoutStyleIntransit
					|| aAction == eAllocationActionType.Reset)
				{
					errorMessage = string.Format(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ActionWarning),
						MIDText.GetTextOnly((int)aAction) );				
				
					DialogResult diagResult = MessageBox.Show(errorMessage,_thisTitle,
						MessageBoxButtons.YesNo,  MessageBoxIcon.Warning);
					if (diagResult == System.Windows.Forms.DialogResult.No)
						okToProcess = false;
				}
			}
			return okToProcess;
		}


        //BEGIN TT#793-MD-DOConnell - Ran balance size billaterally on a receipt header in an assortment and receive a null reference exception
        private bool OKToProcessAssortment(eAllocationActionType aAction)
        {
            SelectedHeaderList allocHdrList = (SelectedHeaderList)_trans.AssortmentSelectedHdrList;
            string errorMessage = string.Empty;
            string errorParm = string.Empty;
            bool okToProcess = true;
            // BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
            //AllocationProfileList alp = (AllocationProfileList)_transaction.GetMasterProfileList(eProfileType.Allocation);
            AllocationProfileList alp = (AllocationProfileList)_trans.GetMasterProfileList(eProfileType.AssortmentMember);
            // END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member

            foreach (SelectedHeaderProfile shp in allocHdrList)
            {
                // BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
                //AllocationProfile ap = (AllocationProfile)alp.FindKey(key);
                AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
                // END TT#371-MD - stodd -  Velocity Interactive on Assortment

                // Begin #1228 - stodd
                if (ap.HeaderType == eHeaderType.Assortment)
                    continue;
                // End #1228 

                switch (ap.HeaderAllocationStatus)
                {
                    case eHeaderAllocationStatus.ReceivedOutOfBalance:
                        if (aAction != eAllocationActionType.BackoutAllocation)  // allow backout allocation when recv'd out of balance
                        {
                            okToProcess = false;
                        }
                        break;
                    case eHeaderAllocationStatus.ReleaseApproved:
                        if (aAction != eAllocationActionType.Reset &&
                            aAction != eAllocationActionType.Release)
                        {
                            okToProcess = false;
                        }
                        break;
                    case eHeaderAllocationStatus.Released:
                        if (aAction != eAllocationActionType.Reset)
                        {
                            okToProcess = false;
                        }
                        break;
                    default:
                        if (aAction == eAllocationActionType.Reset)
                        {
                            okToProcess = false;
                        }
                        else if (aAction == eAllocationActionType.ChargeIntransit)
                        {
                            if (ap.HeaderAllocationStatus != eHeaderAllocationStatus.AllInBalance
                                && ap.HeaderAllocationStatus != eHeaderAllocationStatus.AllocatedInBalance)
                                okToProcess = false;
                        }
                        break;
                }
                if (!okToProcess)
                {
                    errorMessage = string.Format
                        (MIDText.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction),
                        MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));

                    MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    okToProcess = false;
                    break;
                }

                if (okToProcess)
                {
                    if (!_sab.ClientServerSession.GlobalOptions.IsReleaseable(ap.HeaderType)
                        && aAction == eAllocationActionType.Release)
                    {
                        if (ap.IsDummy)
                        {
                            errorParm = MIDText.GetTextOnly((int)eHeaderType.Dummy) + " "
                                + MIDText.GetTextOnly(eMIDTextCode.lbl_Header);
                        }
                        else
                        {
                            errorParm = MIDText.GetTextOnly((int)ap.HeaderType);
                        }
                        errorMessage = string.Format
                            (MIDText.GetText(eMIDTextCode.msg_ActionNotAllowed),
                            errorParm);
                        MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        okToProcess = false;
                        break;
                    }
                }
            }
            if (okToProcess)
            {
                if (aAction == eAllocationActionType.BackoutAllocation
                    || aAction == eAllocationActionType.BackoutSizeAllocation
                    || aAction == eAllocationActionType.BackoutSizeIntransit
                    || aAction == eAllocationActionType.BackoutStyleIntransit
                    || aAction == eAllocationActionType.Reset)
                {
                    errorMessage = string.Format(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ActionWarning),
                        MIDText.GetTextOnly((int)aAction));

                    DialogResult diagResult = MessageBox.Show(errorMessage, this.Text,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (diagResult == System.Windows.Forms.DialogResult.No)
                        okToProcess = false;
                }
            }

            // BEGIN TT#698-MD - Stodd - add ability for workflows to be run against assortments.
            if (okToProcess)
            {
                okToProcess = VerifySecurity(allocHdrList);
            }
            // END TT#698-MD - Stodd - add ability for workflows to be run against assortments.

            return okToProcess;
        }

        private bool VerifySecurity(SelectedHeaderList selectedHeaderList)
        {
            HierarchyNodeSecurityProfile hierNodeSecProfile;
            try
            {
                bool allowUpdate = true;
                foreach (SelectedHeaderProfile shp in selectedHeaderList)
                {
                    //AllocationProfile ap = _transaction.GetAllocationProfile(shp.Key);
                    AllocationProfile ap = _trans.GetAssortmentMemberProfile(shp.Key);
                    if (ap != null && ap.StyleHnRID > 0)
                    {
                        hierNodeSecProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(ap.StyleHnRID, (int)eSecurityTypes.Allocation);
                        if (!hierNodeSecProfile.AllowUpdate)
                        {
                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(shp.StyleHnRID, false, false);
                            allowUpdate = false;
                            string errorMessage = MIDText.GetText(eMIDTextCode.msg_NotAuthorizedForNode);
                            errorMessage = errorMessage + " Node: " + hnp.Text;
                            SAB.ClientServerSession.Audit.Add_Msg(
                                eMIDMessageLevel.Warning,
                                eMIDTextCode.msg_NotAuthorizedForNode,
                                errorMessage,
                                "Assortment View");
                            break;
                        }
                    }
                    // End TT#2
                }
                return allowUpdate;
            }
            catch
            {
                throw;
            }
        }
        //END TT#793-MD-DOConnell - Ran balance size billaterally on a receipt header in an assortment and receive a null reference exception


        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cmbAttributeSet_SelectionChangeCommitted(object sender, System.EventArgs e)
        private void cmbAttributeSet_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
			if (!_loading &&
                _trans.AllocationStoreGroupLevel != Convert.ToInt32(cmbAttributeSet.SelectedValue))
			{
                // Begin Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set
                if (_applyPending &&
                    !_applyPendingMsgDisplayed)
                {
                    _applyPendingMsgDisplayed = true;
                    if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ApplyPendingChanges), _thisTitle,
                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        btnApply_Click(this, new EventArgs());
                    }
                    else
                    {
                        this._allocationWaferCellChangeList = new AllocationWaferCellChangeList();
                    }
                }
                // End Development TT#8
				_trans.AllocationStoreGroupLevel = Convert.ToInt32(cmbAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
				// begin MID Track 6079 qty changed not accepted after sort column
                //CriteriaChanged();
                SortCriteriaChanged();
                // end MID Track 6079 qty changed not accepted after sort column
			}
		}

		private void HilightSelectedSet()
		{	
			CellRange cellRange;
			string selSet;
			int  i, dispRows, foundRow = 0;
			bool invalidStyle = false;
			selSet = cmbAttributeSet.Text;
			
			for (i = 0; i < g7.Rows.Count; i++) 
			{
				cellRange = g7.GetCellRange(i, 0, i, g7.Cols.Count - 1);
				if (Convert.ToString(g7.GetData(i, 0), CultureInfo.CurrentUICulture) == selSet) 
				{
					foundRow = i;
					if (_changeRowStyle)
					{
						switch (g7.Rows[i].Style.Name)
						{
							case "Style1":
								cellRange.Style = g7.Styles["Reverse1"];
								break;
							case "Style2":
								cellRange.Style = g7.Styles["Reverse2"];
								break;
							default:
								invalidStyle = true;
								break;
						}
						if (invalidStyle)
							throw new Exception("Invalid row style");
					}
					else
						break;
				}
				else
				{
					cellRange.Style = g7.Rows[i].Style;
				}

			}		

			_isScrolling = true;		// BEGIN MID Track #2747
			if (foundRow < g7.TopRow)
				g7.TopRow = foundRow;
			else if (foundRow >= g7.BottomRow)
			{ 
				dispRows = ( g7.BottomRow - g7.TopRow );
				// BEGIN MID Track #2523 - Divide by zero error
				//g7.TopRow = ( foundrow / dispRows ) * dispRows;
				if (dispRows > 0)	
				{	int res = ( foundRow / dispRows ) * dispRows;
					g7.TopRow = res;
				}
				else
					g7.TopRow = foundRow;
				// END MID Track #2523 
			}
			else
				g7.TopRow = foundRow;

			g8.TopRow = g7.TopRow;
			g9.TopRow = g7.TopRow;
			// BEGIN MID Track #2531 - - correct row not showing  
			//			g7.ShowCell(foundrow,0);
			//			g8.ShowCell(foundrow,0);
			//			g9.ShowCell(foundrow,0);
			
			g7.ShowCell(foundRow,g1.LeftCol);
			g8.ShowCell(foundRow,g2.LeftCol);
			g6.LeftCol = g3.LeftCol;
			g9.LeftCol = g3.LeftCol;
			g12.LeftCol = g3.LeftCol;
			g3.ShowCell(_compRow,g3.LeftCol);
			if (g6.Rows.Count > 0)
				g6.ShowCell(g6.TopRow,g6.LeftCol);
			g9.ShowCell(foundRow,g9.LeftCol);
			g12.ShowCell(g12.TopRow,g12.LeftCol);
			// END MID Track #2531
			_isScrolling = false; // END MID Track #2747
			vScrollBar3.Value = foundRow;
		}

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cmbView_SelectionChangeCommitted(object sender, EventArgs e)
        private void cmbView_SelectionChangeCommitted(object sender, EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            try
            {
                if (!_bindingView)
                {
                    _viewRID = Convert.ToInt32(cmbView.SelectedValue, CultureInfo.CurrentUICulture);
                    if (_viewRID != _lastSelectedViewRID)
                    {
                        _lastSelectedViewRID = _viewRID;
                        // Begin TT#318 - RMatelic - View column position not retaining : comment out next line  
                        _changingView = true;
                        if (_loading)
                        {
                            ApplyViewToGridLayout(_viewRID);
                        }
                        else
                        {
                            AddViewColumns();
                            SetScreenParmsFromView();
                            if (_columnAdded || _buttonChanged)
                            {
                                // Begin TT#2380 - JSmith - Column Changes in Size Review
                                _saveCurrentColumns = true;
                                // End TT#2380
                                ReloadGridData();
                                _columnAdded = false;
                                _buttonChanged = false;
                            }
                            else
                            {
                                ApplyViewToGridLayout(_viewRID);
                            }
                        }
                        _changingView = false;
                        // End TT#318  
                        if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
                        {
                            MIDRetail.Windows.frmVelocityMethod frmVelocity = (MIDRetail.Windows.frmVelocityMethod)_trans.VelocityWindow;
                            frmVelocity.GetStoreDetailView();
                        }
                        // Begin TT#407 - RMatelic - View not saving when hitting 'X' in upper right corner to exit; not supposed to, but add 'nag' message   
                        if (_trans.DataState == eDataState.ReadOnly || FunctionSecurity.AllowUpdate == false)
                        {
                            // do not update
                        }
                        else if (!_loading)
                        {
                            ChangePending = true;
                        }
                    }   // End TT#407
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                Cursor.Current = Cursors.Default;
            }
        }

        private void ApplyViewToGridLayout(int aViewRID)
        {
            try
            {
                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                //int visiblePosition, width;
                int visiblePosition = 0, width;
                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                //int sortSequence;
                string gridKey, colKey, errMessage;
                bool isHidden, isGroupByCol;
                eSortDirection sortDirection;
                C1FlexGrid appliedGrid1 = null;
                C1FlexGrid appliedGrid2 = null;
                C1FlexGrid appliedGrid3 = null;
                C1FlexGrid appliedGrid4 = null;
                Hashtable lastGridColUsedHash = new Hashtable();
                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                SortedList slGridViewDetail = new SortedList(new GridViewDetailOrder());
                GridViewDetail gvd;
                string currentGrid = null;
                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                _lastSelectedViewRID = aViewRID;

                if (aViewRID == 0 || aViewRID == Include.NoRID)    // don't modify current grid appearance 
                {
                    return;
                }

                // Begin TT#2909 - JSmith - Size REview view-> change radio button from Size to Variable and the view titles get distorted
                //DataTable dtGridViewDetail = _gridViewData.GridViewDetail_Read(aViewRID);
                DataTable dtGridViewDetail = _gridViewData.GridViewDetail_Read_ByPosition(aViewRID);
                // End TT#2909 - JSmith - Size REview view-> change radio button from Size to Variable and the view titles get distorted

                if (dtGridViewDetail == null || dtGridViewDetail.Rows.Count == 0)
                {
                    errMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_as_GridViewDoesNotExist);
                    MessageBox.Show(errMessage);
                    _lastSelectedViewRID = Include.NoRID;
                    BindViewCombo();
                    return;
                }

                bool sortFound = false;
                C1FlexGrid sortGrid = null;
                Hashtable headerStartColHash = new Hashtable();

                Cursor.Current = Cursors.WaitCursor;

                SetGridRedraws(false);
                foreach (DataRow row in dtGridViewDetail.Rows)
                {
                    gridKey = Convert.ToString(row["BAND_KEY"], CultureInfo.CurrentUICulture);
                    colKey = Convert.ToString(row["COLUMN_KEY"], CultureInfo.CurrentUICulture);
                    // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                    //visiblePosition = Convert.ToInt32(row["VISIBLE_POSITION"], CultureInfo.CurrentUICulture);
                    ++visiblePosition;
                    // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                    isHidden = Include.ConvertCharToBool(Convert.ToChar(row["IS_HIDDEN"], CultureInfo.CurrentUICulture));
                    isGroupByCol = Include.ConvertCharToBool(Convert.ToChar(row["IS_GROUPBY_COL"], CultureInfo.CurrentUICulture));
                    sortDirection = (eSortDirection)Convert.ToInt32(row["SORT_DIRECTION"], CultureInfo.CurrentUICulture);
                    if (row["WIDTH"] != DBNull.Value)
                    {
                        width = Convert.ToInt32(row["WIDTH"], CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        width = -1;
                    }

                    appliedGrid1 = null;
                    switch (gridKey)
                    {
                        case "g1":
                            // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                            if (currentGrid == null)
                            {
                                visiblePosition = 0;
                                currentGrid = g1.Name;
                            }
                            // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                            appliedGrid1 = g1;
                            appliedGrid2 = g4;
                            appliedGrid3 = g7;
                            appliedGrid4 = g10;
                            break;
                        case "g2":
                            // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                            if (gridKey != currentGrid)
                            {
                                visiblePosition = 0;
                                currentGrid = g2.Name;
                            }
                            // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                            appliedGrid1 = g2;
                            appliedGrid2 = g5;
                            appliedGrid3 = g8;
                            appliedGrid4 = g11;
                            break;
                        case "g3":
                            // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                            if (gridKey != currentGrid)
                            {
                                visiblePosition = 0;
                                currentGrid = g3.Name;
                            }
                            // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                            appliedGrid1 = g3;
                            appliedGrid2 = g6;
                            appliedGrid3 = g9;
                            appliedGrid4 = g12;
                            break;
                    }

                    if (appliedGrid1 != null)
                    {
                        if (gridKey == "g3" && _headerList.Count > 1) 
                        {
                            for (int i = 0; i < appliedGrid1.Cols.Count; i++)
                            {
                                TagForColumn colTag = (TagForColumn)appliedGrid1.Cols[i].UserData;
                                AllocationWaferCoordinate wafercoord = colTag.CubeWaferCoorList[_hdrRow];
                                if (!headerStartColHash.ContainsKey(wafercoord.Label))
                                {
                                    headerStartColHash.Add(wafercoord.Label, i);   // need first column for each header
                                }
                            }

                            // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                            //foreach (string headerID in headerStartColHash.Keys)
                            //{
                            int headerPosition = 0;   // TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                            foreach (AllocationHeaderProfile ahp in _headerList)
                            {
                                string headerID = ahp.HeaderID;
                            // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order

                                // Begin TT#2044-MD - AGallagher - In and Asst open style reveiw with VSW variables and the variables do not appear and are not selected in the column chooser.
                                if (ahp.HeaderType == eHeaderType.Placeholder)
                                {
                                    if ((eAssortmentType)ahp.AsrtType == eAssortmentType.PostReceipt || (eAssortmentType)ahp.AsrtType == eAssortmentType.GroupAllocation)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        HierarchyNodeProfile hnp_style = _trans.SAB.HierarchyServerSession.GetNodeData(ahp.StyleHnRID, false);
                                        if (hnp_style.IsVirtual && hnp_style.Purpose == ePurpose.Placeholder)
                                        {
                                            headerID = ahp.HeaderID;
                                        }
                                        else
                                        {
                                            headerID = hnp_style.LevelText;
                                        }
                                    }
                                }
                                // End TT#2044-MD - AGallagher - Size review and size analysis view are distorted or sizes are out of order

                                for (int col = 0; col < appliedGrid1.Cols.Count; col++)
                                {
                                    C1.Win.C1FlexGrid.Column column = appliedGrid1.Cols[col];
                                    TagForColumn colTag = (TagForColumn)column.UserData;
                                    AllocationWaferCoordinate wafercoord = colTag.CubeWaferCoorList[_hdrRow];
                                    if (column.Name == colKey && wafercoord.Label == headerID)
                                    {
                                        int index = column.Index;
                                        int visPosition;
                                        if (_trans.AllocationGroupBy == Convert.ToInt32(eAllocationStyleViewGroupBy.Header, CultureInfo.CurrentUICulture))
                                        {
                                            visPosition = visiblePosition + (int)headerStartColHash[wafercoord.Label];
                                        }
                                        else
                                        {
                                            visPosition = (int)headerStartColHash[wafercoord.Label] + (visiblePosition * _headerList.Count);
                                        }
                                        if (visPosition > (appliedGrid1.Cols.Count - 1))
                                        {
                                            visPosition = appliedGrid1.Cols.Count - 1;
                                        }
                                        // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                        // Begin TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                                        //gvd = new GridViewDetail(colTag.CubeWaferCoorList, visPosition, isHidden, isGroupByCol, sortDirection, width);
                                        gvd = new GridViewDetail(colTag.CubeWaferCoorList, visPosition, isHidden, isGroupByCol, sortDirection, width, headerPosition);
                                        // End TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                                        slGridViewDetail.Add(gvd, gvd);
                                        // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                        
                                        colTag.IsDisplayed = !isHidden;
                                        if (width != -1)
                                        {
                                            appliedGrid1.Cols[index].Width = width;
                                            appliedGrid2.Cols[index].Width = width;
                                            appliedGrid3.Cols[index].Width = width;
                                            appliedGrid4.Cols[index].Width = width;
                                        }
                                        if (sortFound)
                                        {
                                            colTag.Sort = SortEnum.none;
                                        }
                                        else
                                        {
                                            switch (sortDirection)
                                            {
                                                case eSortDirection.Descending:
                                                    colTag.Sort = SortEnum.asc;    // set to opposite and then simulate grid column click to sort
                                                    sortFound = true;
                                                    sortGrid = appliedGrid1;
                                                    GridMouseCol = visPosition;
                                                    break;
                                                case eSortDirection.Ascending:
                                                    colTag.Sort = SortEnum.desc;    // set to opposite and then simulate grid column click to sort
                                                    sortFound = true;
                                                    sortGrid = appliedGrid1;
                                                    GridMouseCol = visPosition;
                                                    break;
                                                default:
                                                    colTag.Sort = SortEnum.none;
                                                    break;
                                            }
                                        }
                                        appliedGrid1.Cols[index].UserData = colTag;
                                        // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                        //appliedGrid1.Cols[index].Move(visPosition);
                                        //appliedGrid2.Cols[index].Move(visPosition);
                                        //appliedGrid3.Cols[index].Move(visPosition);
                                        //appliedGrid4.Cols[index].Move(visPosition);
                                        // End loop since found column
                                        col = appliedGrid1.Cols.Count;
                                        // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                    }
                                }
                                ++headerPosition;   // TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                            }
                        }
                        else if (appliedGrid1.Cols.Contains(colKey))
                        {
                            int index = appliedGrid1.Cols[colKey].Index;
                            TagForColumn colTag = (TagForColumn)appliedGrid1.Cols[colKey].UserData;
                            colTag.IsDisplayed = !isHidden;
                            if (width != -1)
                            {
                                appliedGrid1.Cols[index].Width = width;
                                appliedGrid2.Cols[index].Width = width;
                                appliedGrid3.Cols[index].Width = width;
                                appliedGrid4.Cols[index].Width = width;
                            }
                            switch (sortDirection)
                            {
                                case eSortDirection.Descending:
                                    colTag.Sort = SortEnum.asc;    // set to opposite and then simulate grid column click to sort
                                    sortFound = true;
                                    break;
                                case eSortDirection.Ascending:
                                    colTag.Sort = SortEnum.desc;    // set to opposite and then simulate grid column click to sort
                                    sortFound = true;
                                    break;
                                default:
                                    colTag.Sort = SortEnum.none;
                                    break;
                            }
                            appliedGrid1.Cols[colKey].UserData = colTag;

                            // Begin TT#1265 - RMatelic - Style Reivew error with multi header with no bulk/packs defined
                            if (visiblePosition > appliedGrid1.Cols.Count - 1)
                            {
                                int lastCol = 0; 
                                if (lastGridColUsedHash.Contains(gridKey))
                                {
                                    lastCol = (int)lastGridColUsedHash[gridKey];
                                    lastCol++;
                                    lastGridColUsedHash[gridKey] = lastCol;
                                }
                                else
                                {
                                    lastGridColUsedHash.Add(gridKey, lastCol);
                                }
                                visiblePosition = lastCol;
                            }
                            // End TT#1265

                            appliedGrid1.Cols[index].Move(visiblePosition);
                            appliedGrid2.Cols[index].Move(visiblePosition);
                            appliedGrid3.Cols[index].Move(visiblePosition);
                            appliedGrid4.Cols[index].Move(visiblePosition);
                            if (sortFound)
                            {
                                sortFound = false;
                                sortGrid = appliedGrid1;
                                GridMouseCol = visiblePosition;
                            }
                        }
                    }  
                }

                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                PositionColumns(slGridViewDetail);
                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order

                ApplyPreferences();
                if (_columnAdded || FormLoaded)	// TT#1758-MD - Velocity Store Detail (Style Review) columns are mis-aligned when using a particular view 
                {
                   MiscPositioning();
                }
                if (sortGrid != null)
                {
                    _isSorting = true;
                    this.GridClick(sortGrid);
                    _isSorting = false;
                }
                else
                {
                    // Begin TT#328-MD - RMatelic -Enum value displaying in Velocity Store Detail - extension of TT##322 - put original code back in
                    // Begin TT#322-MD - RMatelic -Enum value displaying in Velocity Store Detail
                    //mnuSortByDefault_Click(mnuSortByDefault, null);
                    //SortByDefault();
                    // End TT#322-MD 
                    mnuSortByDefault_Click(mnuSortByDefault, null);
                    // End TT#328-MD 
                }
            }
            catch
            {
                throw;
            }
            // Begin TT#318 - RMatelic - View column position not retaining
            SetV1SplitPosition();
            SetGridRedraws(true);
            Cursor.Current = Cursors.Default;
            // End TT#318
        }
        // End TT#231

        // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
        private void PositionColumns(SortedList slGridViewDetail)
        {
            int moveTo;
            
            foreach (GridViewDetail gvd in slGridViewDetail.Values)
            {
                //AllocationWaferCoordinate gvdComponentcoord = GetAllocationCoordinate(gvd.AllocationWaferCoordinateList, eAllocationCoordinateType.Component);
                //AllocationWaferCoordinate gvdWafercoord = GetAllocationCoordinate(gvd.AllocationWaferCoordinateList, eAllocationCoordinateType.Header);
                //AllocationWaferCoordinate gvdVariableCoord = GetAllocationCoordinate(gvd.AllocationWaferCoordinateList, eAllocationCoordinateType.Variable);
                AllocationWaferCoordinate gvdComponentcoord = null;
                AllocationWaferCoordinate gvdWafercoord = null;
                AllocationWaferCoordinate gvdVariableCoord = null;
                foreach (AllocationWaferCoordinate awc in gvd.AllocationWaferCoordinateList)
                {
                    switch (awc.CoordinateType)
                    {
                        case eAllocationCoordinateType.Header:
                            gvdWafercoord = awc;
                            break;
                        case eAllocationCoordinateType.Variable:
                            gvdVariableCoord = awc;
                            break;
                        default:
                            gvdComponentcoord = awc;
                            break;
                    }
                }
                for (int col = 0; col < g3.Cols.Count; col++)
                {
                    C1.Win.C1FlexGrid.Column column = g3.Cols[col];
                    TagForColumn colTag = (TagForColumn)column.UserData;
                    //AllocationWaferCoordinate componentcoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Component);
                    //AllocationWaferCoordinate wafercoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Header);
                    //AllocationWaferCoordinate variableCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                    AllocationWaferCoordinate componentcoord = null;
                    AllocationWaferCoordinate wafercoord = null;
                    AllocationWaferCoordinate variableCoord = null;
                    foreach (AllocationWaferCoordinate awc in colTag.CubeWaferCoorList)
                    {
                        switch (awc.CoordinateType)
                        {
                            case eAllocationCoordinateType.Header:
                                wafercoord = awc;
                                break;
                            case eAllocationCoordinateType.Variable:
                                variableCoord = awc;
                                break;
                            default:
                                componentcoord = awc;
                                break;
                        }
                    }
                    //if (gvdWafercoord.Label == wafercoord.Label &&
                    //    gvdVariableCoord.Label == variableCoord.Label)
                    if (gvdComponentcoord.Label == componentcoord.Label && gvdComponentcoord.CoordinateSubType == componentcoord.CoordinateSubType && gvdComponentcoord.Key == componentcoord.Key &&
                        gvdWafercoord.Label == wafercoord.Label && gvdWafercoord.CoordinateSubType == wafercoord.CoordinateSubType && gvdWafercoord.Key == wafercoord.Key &&
                        gvdVariableCoord.Label == variableCoord.Label && gvdVariableCoord.CoordinateSubType == variableCoord.CoordinateSubType && gvdVariableCoord.Key == variableCoord.Key)
                    {
                        if (col != gvd.VisiblePosition)
                        {
                            // Component One removes column before add.
                            // So offset position if moving to right
                            if (gvd.VisiblePosition > col)
                            {
                                moveTo = gvd.VisiblePosition - 1;
                            }
                            else
                            {
                                moveTo = gvd.VisiblePosition;
                            }

                            g3.Cols[col].Move(moveTo);
                            g6.Cols[col].Move(moveTo);
                            g9.Cols[col].Move(moveTo);
                            g12.Cols[col].Move(moveTo);
                        }
                        // End loop since found column
                        col = g3.Cols.Count;
                    }
                }
            }
        }
        // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order

        // Begin TT#358/#334/#363 - RMatelic - Velocity View column display issues
        private void SaveCurrentColumns()
        {
            // Begin TT#2380 - JSmith - Column Changes in Size Review
            if (!_saveCurrentColumns)
            {
                return;
            }
            _saveCurrentColumns = false;
            // End TT#2380

            try
            {
                SaveCurrentColumnsG1();
                SaveCurrentColumnsG2();
                SaveCurrentColumnsG3();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void SaveCurrentColumnsG1()
        {
            try
            {
                _curColumnsG1.Clear();
                for (int col = 0; col < g1.Cols.Count; col++)
                {
                    RowColHeader rch = new RowColHeader();
                    if (Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture) == _lblStore
                     || Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture).Trim() == string.Empty)
                    {
                        if (Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture) == _lblStore)
                        {
                            rch.Name = _lblStore;
                        }
                        else
                        {
                            rch.Name = string.Empty;
                        }
                        rch.IsDisplayed = true;     // 'Store' & extra column cannot be hidden
                    }
                    else
                    {
                        rch.Name = g1.Cols[col].Name;
						rch.IsDisplayed = g1.Cols[col].Visible;
                    }
                    Image image = g1.GetCellImage(0, col);
                    if (image != null)
                    {
                        rch.Image = image;
                    }
                    _curColumnsG1.Add(rch); 
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void SaveCurrentColumnsG2()
        {
            try
            {
                _curColumnsG2.Clear();
                for (int col = 0; col < g2.Cols.Count; col++)
                {
                    TagForColumn colTag = (TagForColumn)g2.Cols[col].UserData;
                    RowColHeader rch = new RowColHeader();
                    AllocationWaferCoordinate wafercoord;

                    rch.Name = g2.Cols[col].Name;
                    if (rch.Name == string.Empty)
                    {
                        if (_groupByChanged)
                        {
                            wafercoord = colTag.CubeWaferCoorList[_hdrRow];
                        }
                        else
                        {
                            wafercoord = colTag.CubeWaferCoorList[_compRow];
                        }
                        rch.Name = wafercoord.Label;
                    }
                    rch.IsDisplayed = g2.Cols[col].Visible;
                    rch.RowColumnIndex = colTag.cellColumn;

                    Image image;
                    image = g2.GetCellImage(1, col);
                   
                    if (image != null)
                    {
                        rch.Image = image;
                    }
                    _curColumnsG2.Add(rch);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void SaveCurrentColumnsG3()
        {
            try
            {
                _curColumnsG3.Clear();
                _headerAllColHash.Clear();
                ArrayList headerColAl;
                for (int col = 0; col < g3.Cols.Count; col++)
                {  
                    TagForColumn colTag = (TagForColumn)g3.Cols[col].UserData;
                    RowColHeader rch = new RowColHeader();
                    AllocationWaferCoordinate wafercoord;

                    rch.Name = g3.Cols[col].Name;
                    if (rch.Name == string.Empty)
                    {
                        if (_groupByChanged)
                        {
                            wafercoord = colTag.CubeWaferCoorList[_hdrRow];
                        }
                        else
                        {
                            wafercoord = colTag.CubeWaferCoorList[_compRow];
                        }
                        rch.Name = wafercoord.Label;
                        if (Enum.IsDefined(typeof(eGeneralComponentType),(eGeneralComponentType)wafercoord.CoordinateSubType))
                        {
                            if (!_curColumnsG3.Contains(rch.Name))
                            {
                                _curColumnsG3.Add(rch.Name);
                            }
                        }
                    }
                    else
                    {
                        if (_groupByChanged)
                        {
                            wafercoord = colTag.CubeWaferCoorList[_hdrRow];
                        }
                        else
                        {
                            wafercoord = colTag.CubeWaferCoorList[_compRow];
                        }
                        rch.Name = wafercoord.Label;        // change name from enum to label
                        if (!_curColumnsG3.Contains(rch.Name))
                        {
                            _curColumnsG3.Add(rch.Name);
                        }
                    }
                    
                    rch.IsDisplayed = g3.Cols[col].Visible;
                    rch.RowColumnWidth = g3.Cols[col].Width;
                    rch.RowColumnIndex = colTag.cellColumn;

                    Image image;
                    image = g3.GetCellImage(1, col);
                    if (image != null)
                    {
                        rch.Image = image;
                    }
                
                    if (_groupByChanged)
                    {
                        wafercoord = colTag.CubeWaferCoorList[_compRow];
                    }
                    else
                    {
                        wafercoord = colTag.CubeWaferCoorList[_hdrRow];
                    }
                    string headerID = wafercoord.Label;
                    if (!_headerAllColHash.ContainsKey(headerID))
                    {
                        headerColAl = new ArrayList();
                        headerColAl.Add(rch);
                        _headerAllColHash.Add(headerID, headerColAl);
                    }
                    else
                    {
                        headerColAl = (ArrayList)_headerAllColHash[headerID];
                        headerColAl.Add(rch);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ApplyCurrentColumns()
        {
            try
            {
                ApplyCurrentColumnsG1();
                ApplyCurrentColumnsG2();
                ApplyCurrentColumnsG3();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ApplyCurrentColumnsG1()
        {
            try
            {
                for (int i = 0; i < _curColumnsG1.Count; i++)
                {
                    RowColHeader rch = (RowColHeader)_curColumnsG1[i];
                    Image image = null;
                    if (g1.Cols.Contains(rch.Name))
                    { 
                        int index = g1.Cols[rch.Name].Index;
                        if (rch.Image != null)
                        {
                            image = rch.Image;
                        }
                        g1.Cols[index].Visible = rch.IsDisplayed;
                        g4.Cols[index].Visible = rch.IsDisplayed;
                        g7.Cols[index].Visible = rch.IsDisplayed;
                        g10.Cols[index].Visible = rch.IsDisplayed;

                        g1.Cols[index].Move(i);
                        g4.Cols[index].Move(i);
                        g7.Cols[index].Move(i);
                        g10.Cols[index].Move(i);
                        g1.SetCellImage(0, i, image);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ApplyCurrentColumnsG2()
        {
            try
            {
                for (int i = 0; i < _curColumnsG2.Count; i++)
                {
                    RowColHeader rch = (RowColHeader)_curColumnsG2[i];
                    Image image = null;
                    if (rch.Image != null)
                    {
                        image = rch.Image;
                    }
                    int index = -1; 
                    if (g2.Cols.Contains(rch.Name))     // Header component columns are not in column name
                    {
                        index = g2.Cols[rch.Name].Index;                       
                    }
                    else
                    {
                        foreach (C1.Win.C1FlexGrid.Column column in g2.Cols)
                        {
                            TagForColumn colTag = (TagForColumn)column.UserData;
                            if (colTag.cellColumn == rch.RowColumnIndex)
                            {
                                index = column.Index;
                                break;
                            }
                        }
                    }
                    if (index > -1)
                    {
                        g2.Cols[index].Visible = rch.IsDisplayed;
                        g5.Cols[index].Visible = rch.IsDisplayed;
                        g8.Cols[index].Visible = rch.IsDisplayed;
                        g11.Cols[index].Visible = rch.IsDisplayed;

                        g2.Cols[index].Move(i);
                        g5.Cols[index].Move(i);
                        g8.Cols[index].Move(i);
                        g11.Cols[index].Move(i);
                        g2.SetCellImage(1, i, image);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ApplyCurrentColumnsG3()
        {
            try
            {
                Hashtable headerStartColHash = new Hashtable();
                ArrayList columnAl = new ArrayList();
                AllocationWaferCoordinate wafercoord = null;
                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                //int lastUsedPosition = 0;
                SortedList slGridViewDetail = new SortedList(new GridViewDetailOrder());
                ArrayList headerComponentColumns = new ArrayList();
                GridViewDetail gvd;
                AllocationWaferCoordinateList allocationWaferCoordinateList = null;
                eSortDirection sortDirection = eSortDirection.None;
                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
				string headerID = string.Empty;	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                for (int i = 0; i < g3.Cols.Count; i++)
                {
                    TagForColumn colTag = (TagForColumn)g3.Cols[i].UserData;
                    wafercoord = colTag.CubeWaferCoorList[_hdrRow];
                    headerID = wafercoord.Label;	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
                    if (!headerStartColHash.ContainsKey(headerID))
                    {
                        headerStartColHash.Add(headerID, i);   // need first column for each header
                    }
                    if (headerStartColHash.Keys.Count == _headerList.Count)
                    {
                        break;
                    }
                }

                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                //foreach (string headerID in _headerAllColHash.Keys)
                //{
                int headerPosition = 0;   // TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                foreach (AllocationHeaderProfile ahp in _headerList)
                {
					// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
					if (ahp.HeaderType == eHeaderType.Assortment)
					{
                        headerID = ahp.HeaderID;
						// Begin TT#1194-MD - stodd - view GA header
                        if (ahp.AsrtType != (int)eAssortmentType.GroupAllocation)
                        {
                            continue;
                        }
						// End TT#1194-MD - stodd - view GA header
					}
					else if (ahp.HeaderType == eHeaderType.Placeholder)
					{
						HierarchyNodeProfile hnp_style = _trans.SAB.HierarchyServerSession.GetNodeData(ahp.StyleHnRID, false);
						if (hnp_style.IsVirtual && hnp_style.Purpose == ePurpose.Placeholder)
						{
							headerID = ahp.HeaderID;
						}
						else
						{
							headerID = hnp_style.LevelText;
						}
					}
					else
                    {
						headerID = ahp.HeaderID;
                    	// Begin TT#3260 - JSmith - Allocation Review Select receive system argument null exception when exporting to Excel
                    	if (_trans.AnalysisOnly &&
                       		 headerID == null)
                    	{
                        	headerID = "default";
                    	}
                    	// End TT#3260 - JSmith - Allocation Review Select receive system argument null exception when exporting to Excel
                    }
                    
                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                    if (headerID != null) //TT#703 - MD - DOConnell - Receive Argument Out of Range exception when draging a header onto an assortment while the Style View Screen is open
                    {
                        ArrayList al = (ArrayList)_headerAllColHash[headerID];

                        if (al != null) //TT#703 - MD - DOConnell - Receive Argument Out of Range exception when draging a header onto an assortment while the Style View Screen is open
                        {
                            for (int i = 0; i < al.Count; i++)
                            {
                                RowColHeader rch = (RowColHeader)al[i];
                                Image image = null;
                                int index = -1;

                                if (rch.Image != null)
                                {
                                    image = rch.Image;
                                }

                                for (int col = 0; col < g3.Cols.Count; col++)
                                {
                                    C1.Win.C1FlexGrid.Column column = g3.Cols[col];
                                    TagForColumn colTag = (TagForColumn)column.UserData;
                                    wafercoord = colTag.CubeWaferCoorList[_hdrRow];
                                    if (wafercoord.Label == headerID)
                                    //if (colTag.cellColumn == rch.RowColumnIndex)
                                    //{
                                    //    //wafercoord = colTag.CubeWaferCoorList[_hdrRow];
                                    //    index = column.Index;
                                    //    break;
                                    //}
                                    {
                                        wafercoord = colTag.CubeWaferCoorList[_compRow];
                                        if (wafercoord.Label == rch.Name)
                                        {
                                            index = column.Index;
                                            // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                            allocationWaferCoordinateList = colTag.CubeWaferCoorList;
                                            switch (colTag.Sort)
                                            {
                                                case SortEnum.desc:
                                                    sortDirection = eSortDirection.Descending;
                                                    break;
                                                case SortEnum.asc:
                                                    sortDirection = eSortDirection.Ascending;
                                                    break;
                                                default:
                                                    sortDirection = eSortDirection.None;
                                                    break;
                                            }
                                            // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                            break;
                                        }
                                    }
                                }
                                if (index > -1)
                                {
                                    g3.Cols[index].Visible = rch.IsDisplayed;
                                    g6.Cols[index].Visible = rch.IsDisplayed;
                                    g9.Cols[index].Visible = rch.IsDisplayed;
                                    g12.Cols[index].Visible = rch.IsDisplayed;
                                    g3.Cols[index].Width = rch.RowColumnWidth;
                                    g6.Cols[index].Width = rch.RowColumnWidth;
                                    g9.Cols[index].Width = rch.RowColumnWidth;
                                    g12.Cols[index].Width = rch.RowColumnWidth;

                                    // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                    int visPosition = 0;
                                    // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                    if (_headerList.Count == 1)
                                    {
                                        // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                        visPosition = i;
                                        // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                        g3.Cols[index].Move(i);
                                        g6.Cols[index].Move(i);
                                        g9.Cols[index].Move(i);
                                        g12.Cols[index].Move(i);
                                        g3.SetCellImage(1, i, image);
                                        g3.SetCellImage(0, i, null);
                                    }
                                    else
                                    {
                                        // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                        //int visPosition = 0;
                                        // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order;
                                        if (_trans.AllocationGroupBy == Convert.ToInt32(eAllocationStyleViewGroupBy.Header, CultureInfo.CurrentUICulture))
                                        {
                                            visPosition = i + (int)headerStartColHash[headerID];
                                            // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                            //g3.Cols[index].Move(visPosition);
                                            //g6.Cols[index].Move(visPosition);
                                            //g9.Cols[index].Move(visPosition);
                                            //g12.Cols[index].Move(visPosition);
                                            // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                        }
                                        else
                                        {
                                            if (_curColumnsG3.Contains(rch.Name))   // column is in all headers
                                            {
                                                int colPos = _curColumnsG3.IndexOf(rch.Name);
                                                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                                visPosition = (int)headerStartColHash[headerID] + (colPos * _headerList.Count);
                                                //g3.Cols[index].Move(visPosition);
                                                //g6.Cols[index].Move(visPosition);
                                                //g9.Cols[index].Move(visPosition);
                                                //g12.Cols[index].Move(visPosition);
                                                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                            }
                                            else                                   // column is in specific header
                                            {
                                                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                                //if (i == 0 && lastUsedPosition == 0)
                                                //{
                                                //    visPosition = 0;
                                                //}
                                                //else
                                                //{
                                                //    lastUsedPosition++;
                                                //    visPosition = lastUsedPosition;
                                                //}
                                                //g3.Cols[index].Move(visPosition);
                                                //g6.Cols[index].Move(visPosition);
                                                //g9.Cols[index].Move(visPosition);
                                                //g12.Cols[index].Move(visPosition);

                                                int colPos = headerComponentColumns.IndexOf(rch.Name);
                                                if (colPos == -1)
                                                {
                                                    headerComponentColumns.Add(rch.Name);
                                                    colPos = headerComponentColumns.Count - 1;
                                                }

                                                //visPosition = (_curColumnsG3.Count * _headerList.Count) + lastUsedPosition;
                                                visPosition = (_curColumnsG3.Count * _headerList.Count) + (colPos * _headerList.Count);
                                                //lastUsedPosition++;
                                                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                            }
                                            // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                            //lastUsedPosition = visPosition;
                                            // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                        }

                                        // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                        if (visPosition > (g3.Cols.Count - 1))
                                        {
                                            visPosition = g3.Cols.Count - 1;
                                        }
                                        //g3.SetCellImage(1, visPosition, image);
                                        //g3.SetCellImage(0, visPosition, null);
                                        g3.SetCellImage(1, index, image);
                                        g3.SetCellImage(0, index, null);
                                        // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                    }
                                    // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                    // Begin TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                                    //gvd = new GridViewDetail(allocationWaferCoordinateList, visPosition, !rch.IsDisplayed, false, sortDirection, rch.RowColumnWidth);
                                    gvd = new GridViewDetail(allocationWaferCoordinateList, visPosition, !rch.IsDisplayed, false, sortDirection, rch.RowColumnWidth, headerPosition);
                                    // End TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                                    slGridViewDetail.Add(gvd, gvd);
                                    sortDirection = eSortDirection.None;
                                    // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                }//TT#703 - MD - DOConnell - Receive Argument Out of Range exception when draging a header onto an assortment while the Style View Screen is open
                            }//TT#703 - MD - DOConnell - Receive Argument Out of Range exception when draging a header onto an assortment while the Style View Screen is open
                        }
                    }
                    ++headerPosition;   // TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                }

                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                PositionColumns(slGridViewDetail);
                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // End TT#358/#334/#363  

		private void btnApply_Click(object sender, System.EventArgs e)
		{
            // Begin TT#1755-MD - stodd - Null Reference exiting Style Review while in GA Mode
            if (g4 == null || g4.Disposing || g5 == null || g5.Disposing || g6 == null || g6.Disposing)
            {
                return;
            }
            // End TT#1755-MD - stodd - Null Reference exiting Style Review while in GA Mode

			try
			{
                // Begin TT#1755-MD - stodd - Null Reference exiting Style Review while in GA Mode
                if (!FormIsClosing)
                {
                    _trans.SetAllocationCellValue(this._allocationWaferCellChangeList); // TT#59 Implement Temp Locks
                    SaveCurrentSettings();
                }
                // End TT#1755-MD - stodd - Null Reference exiting Style Review while in GA Mode

				// BEGIN MID Track #2532 - Balance proportional does not save
				// Move the following to btnAllocate_Click & add new VelocityApplyDetailChanges method
				//if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
				//	_trans.VelocityApplyRulesToStores();
                // begin TT#59 Implement Temp Locks 
                //if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)	
                //{
                //    _trans.VelocityApplyDetailChanges();
                //}
                //// END MID Track #2532
                //UpdateOtherViews();
                //if (!FormIsClosing)
                //{
                //    int i = g4.TopRow;
                //    CriteriaChanged();
                //    _isScrolling = true;
                //    g4.TopRow = i;
                //    g5.TopRow = i;
                //    g6.TopRow = i;
                //    _isScrolling = false;
                //}
                // end TT#59 Implement Temp Locks
			}	
			catch (Exception ex)
			{
				HandleException(ex);
			}
            // begin TT#59 Implement Temp Locks
            finally
            {
                this._allocationWaferCellChangeList = new AllocationWaferCellChangeList();
                // begin TT#59 Implement Temp Locks 
                if (_trans.AllocationViewType == eAllocationSelectionViewType.Velocity)
                {
                    _trans.VelocityApplyDetailChanges();
                }
                UpdateOtherViews();
                if (!FormIsClosing)
                {
                    int i = g4.TopRow;
                    CriteriaChanged();
                    _isScrolling = true;
                    g4.TopRow = i;
                    _vScrollBar2Moved = true;   // TT#4235 - RMatelic - Style review misaligned when making manual changes >> set switch to force BeforeScroll
                    g5.TopRow = i;
                    _vScrollBar2Moved = false;  // TT#4235 - RMatelic - Style review misaligned when making manual changes
                    g6.TopRow = i;
                    _isScrolling = false;
                }
                // end TT#59 Implement Temp Locks
            }
            // end TT#59 Implement Temp Locks
		}
		// BEGIN MID Track #2532 - Balance proportional does not save
		private void btnAllocate_Click(object sender, System.EventArgs e)
		{
			try
			{
				_trans.VelocityApplyDetailChanges();
				_trans.VelocityApplyRulesToStores();
				// BEGIN TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
				if (!_trans.UseAssortmentSelectedHeaders)
				{
					UpdateAllocationWorkspace();
				}
				// END TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
				UpdateOtherViews();
				if (!FormIsClosing)
				{
					int i = g4.TopRow;
					CriteriaChanged();
					_isScrolling = true;
					g4.TopRow = i;
                    _vScrollBar2Moved = true;   // TT#4235 - RMatelic - Style review misaligned when making manual changes >> set switch to force BeforeScroll
                    g5.TopRow = i;
                    _vScrollBar2Moved = false;  // TT#4235 - RMatelic - Style review misaligned when making manual changes
					g6.TopRow = i;
					_isScrolling = false;
				}
			}	
			catch (Exception ex)
			{
				HandleException(ex);
			}
		
		}
		// END MID Track #2532
        // begin TT#241 - MD - JEllis - Header Enqueue Process
        private void CloseOtherViews()
        {
            MIDRetail.Windows.SummaryView frmSummaryView;
            MIDRetail.Windows.SizeView frmSizeView;
            MIDRetail.Windows.AssortmentView frmAssortmentView;
            MIDRetail.Windows.frmVelocityMethod frmVelocityMethod;
            try
            {
                if (_trans.SummaryView != null)
                {
                    frmSummaryView = (MIDRetail.Windows.SummaryView)_trans.SummaryView;
                    if (ErrorFound)
                    {
                        frmSummaryView.ErrorFound = true;
                    }
                    frmSummaryView.Close();
                }
                if (_trans.SizeView != null)
                {
                    frmSizeView = (MIDRetail.Windows.SizeView)_trans.SizeView;
                    if (ErrorFound)
                    {
                        frmSizeView.ErrorFound = true;
                    }
                    frmSizeView.Close();
                }
                if (_trans.AssortmentView != null)
                {
                    frmAssortmentView = (MIDRetail.Windows.AssortmentView)_trans.AssortmentView;
                    if (ErrorFound)
                    {
                        frmAssortmentView.ErrorFound = true;
                    }
                    frmAssortmentView.Close();
                }
                if (_trans.VelocityWindow != null)
                {
                    frmVelocityMethod = (MIDRetail.Windows.frmVelocityMethod)_trans.VelocityWindow;
                    if (ErrorFound)
                    {
                        frmVelocityMethod.ErrorFound = true;
                    }
                    frmVelocityMethod.Close();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // end TT#241 - MD - JEllis - Header Enqueue Process

		public void UpdateOtherViews()
		{
			MIDRetail.Windows.SummaryView frmSummaryView;
			MIDRetail.Windows.SizeView frmSizeView;
            MIDRetail.Windows.AssortmentView frmAssortmentView;
            MIDRetail.Windows.AssortmentView frmGroupView; // TT#488 - MD - Jellis - Group Allocaiton
			try
			{
				SaveCurrentSettings();
				if (_trans.SummaryView != null)
				{
					frmSummaryView = (MIDRetail.Windows.SummaryView)_trans.SummaryView;
					frmSummaryView.UpdateData();
				}
				if (_trans.SizeView != null)
				{
					frmSizeView = (MIDRetail.Windows.SizeView)_trans.SizeView;
					frmSizeView.UpdateData();
				}
                if (_trans.AssortmentView != null)
                {
                    // begin TT#488 - MD - Jellis - Group Allocation
                    //frmAssortmentView = (MIDRetail.Windows.AssortmentView)_trans.AssortmentView; 
                    //frmAssortmentView.UpdateData(true);
                    frmAssortmentView = _trans.AssortmentView as AssortmentView;
                    if (frmAssortmentView != null)
                    {
						// Begin TT#1197-MD - stodd - header status not getting updated correctly - 
                        //frmAssortmentView.UpdateData(true);
                        frmAssortmentView.UpdateDataAndRefresh(true);
						// End TT#1197-MD - stodd - header status not getting updated correctly - 
                    }
                    else
                    {
                        frmGroupView = _trans.AssortmentView as AssortmentView;
                        frmGroupView.UpdateData(true);
                    }
                    // end TT#488 - MD - Jellis - Group Allocaiton
                }
                if ((_trans.SummaryView != null) || (_trans.SizeView != null) || (_trans.AssortmentView != null))
                {
                    GetCurrentSettings();
                }
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		public void Navigate( string navTo )
		{
			System.Windows.Forms.Form frm = null;
			MIDRetail.Windows.SummaryView frmSummaryView = null;
			MIDRetail.Windows.SizeView frmSizeView;
            MIDRetail.Windows.AssortmentView frmAssortmentView;
			bool okToContinue = true;
			try
			{
				switch (navTo)
				{
					case "Summary":
						
						if(_trans.SummaryView != null)
						{
							frmSummaryView = (MIDRetail.Windows.SummaryView)_trans.SummaryView;
							frmSummaryView.Activate();
							return;
						}
						else
						{	// BEGIN MID Track #2551 - security not working
							if (_allocationReviewSummarySecurity.AccessDenied)
							{
								okToContinue = false;
							}
							else
							{
								_trans.AllocationViewType = eAllocationSelectionViewType.Summary;
								_trans.AllocationGroupBy = Convert.ToInt32(eAllocationSummaryViewGroupBy.Attribute, CultureInfo.CurrentUICulture);
								frm = new MIDRetail.Windows.SummaryView(_eab, _trans);
							}
							// END MID Track #2551
						}
						break;
					
					case "Size":
						
						if(_trans.SizeView != null)
						{
							frmSizeView = (MIDRetail.Windows.SizeView)_trans.SizeView; 
							frmSizeView.Activate();
							return;
						}
						else
						{
							// BEGIN MID Track #2551 - security not working
							if (_allocationReviewSizeSecurity.AccessDenied)
							{
								okToContinue = false;
							}
							else
							{
								if (!SizeViewIsValid())
									return;
								_trans.AllocationViewType = eAllocationSelectionViewType.Size;
								_trans.AllocationGroupBy = Convert.ToInt32(eAllocationSizeViewGroupBy.Header, CultureInfo.CurrentUICulture);
								frm = new MIDRetail.Windows.SizeView(_eab, _trans);
							}
							// END MID Track #2551
						}
						break;

                    case "Assortment":

                        if (_trans.AssortmentView != null)
                        {
                            frmAssortmentView = (MIDRetail.Windows.AssortmentView)_trans.AssortmentView;
                            frmAssortmentView.Activate();
                            return;
                        }
                        else
                        {
                            // BEGIN MID Track #2551 - security not working
                            if (_allocationReviewAssortmentSecurity.AccessDenied)
                            {
                                okToContinue = false;
                            }
                            else
                            {
                                _trans.AllocationViewType = eAllocationSelectionViewType.Assortment;
                                _trans.AllocationGroupBy = Convert.ToInt32(eAllocationAssortmentViewGroupBy.Attribute, CultureInfo.CurrentUICulture);
                                frm = new MIDRetail.Windows.AssortmentView(_eab, _trans, eAssortmentWindowType.Assortment);
                            }
                            // END MID Track #2551
                        }
                        break;
				}
				if (!okToContinue)
				{
					MessageBox.Show(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
					return;
				}
				Cursor.Current = Cursors.WaitCursor;
				frm.MdiParent = this.MdiParent;

                // Begin VS2010 WindowState Fix - RMatelic - Maximized window state incorrect when window first opened >>> move WindowState to after Show()
                //frm.WindowState = FormWindowState.Maximized;
                frm.Show();
                frm.WindowState = FormWindowState.Maximized;
                // End VS2010 WindowState Fix
                // Begin TT#199-MD - RMatelic - Column headers not moving with the cells while using arrow keys
                if (navTo == "Size")
                {
                    ((MIDRetail.Windows.SizeView)frm).ResetSplitters();
                }
                // End TT#199-MD
				Cursor.Current = Cursors.Default;
			}	
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private bool SizeViewIsValid()
		{
			bool sizesExist = false;
			Header header;
			// BEGIN MID Track #2547 - Error asking for Analysis Only and Size Review
			// use different error message for Analysis only
			string errorMessage = string.Empty;
			if (_trans.AnalysisOnly)
			{
				// BEGIN MID Track #2959 - - Remove #2547 edit disallowing Size Review for Need Analysis
				//errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeReviewInvalidForAnalysis);
				if (_trans.SizeCurveRID != Include.NoRID)
					sizesExist = true;
				else
					errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeCurveRequiredForAnalysisOnly);
				// END MID Track #2959
			}
			else
			{		
				header = new Header(); 
				foreach (AllocationHeaderProfile ahp in _headerList)
				{	
					if (ahp.AllocationTypeFlags.WorkUpBulkSizeBuy)
						sizesExist = true;
					else if (header.BulkColorSizesExist(ahp.Key))
						sizesExist = true;
					// RonM - temporarily take out allowing pack sizes 
					//else if (header.PackSizesExist(ahp.Key)) 
					//	sizesExist = true;
					
					if (sizesExist)
						break;
				}
				if (!sizesExist)
					errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeReviewInvalid);
			}
			if (!sizesExist)
				MessageBox.Show(errorMessage,this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			// END MID Track #2547
			return sizesExist;
		}	

		private void SaveCurrentSettings()
		{
            // Begin TT#1773 - JSmith - GA -> Velocity-> Qty allocated column-> attempt to make manual change and get unhandled exception
            if (_trans == null)
            {
                return;
            }
            // End TT#1773 - JSmith - GA -> Velocity-> Qty allocated column-> attempt to make manual change and get unhandled exception
			if (rbHeader.Checked)
				_curGroupBy = Convert.ToInt32(eAllocationStyleViewGroupBy.Header, CultureInfo.CurrentUICulture);
			else
				_curGroupBy = Convert.ToInt32(eAllocationStyleViewGroupBy.Components, CultureInfo.CurrentUICulture);
			
			switch (_curViewType)
			{
				case eAllocationSelectionViewType.Style:
					_curAttribute    = _trans.AllocationStoreAttributeID;
					_curAttributeSet = _trans.AllocationStoreGroupLevel;
					break;

				case eAllocationSelectionViewType.Velocity:
					_curAttribute    = _trans.VelocityStoreGroupRID;
					_curAttributeSet = Convert.ToInt32(cmbAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
					break;
			}		
		}
		public void GetCurrentSettings()	// TT#3808 - Group Allocation - Need Action against Headers receives "DuplicateNameException" error - 
		{
			_trans.AllocationViewType = _curViewType;
			_trans.AllocationGroupBy = _curGroupBy;
			_trans.AllocationStoreAttributeID = _curAttribute;
			_trans.AllocationStoreGroupLevel = _curAttributeSet;
		}
		
		override protected void BeforeClosing()
		{
			try
			{
				if ( !CheckForChangedCellOK() )
					return;
			}
			catch (Exception ex)
			{
				HandleException (ex, "StyleView.BeforeClosing");
			}
		}
		override protected void AfterClosing()
		{
			try
			{
				_trans.StyleView = null;
				_trans.CheckForHeaderDequeue();
			}
			catch (Exception ex)
			{
				HandleException (ex, "StyleView.AfterClosing");
			}	
		}

		private bool CheckForChangedCellOK()
		{
			bool okToContinue = true;
			try
			{
				if (_changedGrid != null && _changedGrid.Editor != null)
				{
					RowColEventArgs rowColArgs = new RowColEventArgs(_changedCellRow,_changedCellCol);
					try
					{			
						_changedGrid[_changedCellRow, _changedCellCol] = System.Convert.ToDouble(_changedGrid.Editor.Text, CultureInfo.CurrentUICulture);
						GridAfterEdit(_changedGrid, rowColArgs);
					}
					catch
					{
						throw new MIDException(eErrorLevel.severe,
							(int)eMIDTextCode.msg_MustBeNumeric,
							MIDText.GetText(eMIDTextCode.msg_MustBeNumeric));
//						okToContinue = false;
					}	
					
				}
			}
			catch (Exception ex)
			{
				HandleException (ex, "StyleView.CheckForChangedCellOK");
				okToContinue = false;
			}
			return okToContinue;
		}

		private void StyleView_Activated(object sender, System.EventArgs e)
		{
			try
			{
				if (_initalLoad)
					_initalLoad = false;
				else
				{
					GetCurrentSettings();
					//CriteriaChanged();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void StyleView_Deactivate(object sender, System.EventArgs e)
		{
			SaveCurrentSettings();
		}

		#region Grid BeforeScroll Events

		private void g2_BeforeScroll(object sender, RangeEventArgs e)
		{
			try
			{
                // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                //if (_isScrolling || _arrowScroll)
                //{
                //    BeforeScroll(g2, null, g2, null, g2, null, hScrollBar2);
                //}
                //else
                //    e.Cancel = true;
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g2, null, g2, null, g2, null, hScrollBar2);
                BeforeScroll(g2, null, g2, null, g2, g2, null, hScrollBar2);
                // End TT#2930 - JSmith - Scrolling Issue
                // End TT#1144
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void g3_BeforeScroll(object sender, RangeEventArgs e)
		{
			try
			{
                // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                //if (_isScrolling || _arrowScroll) 
                //{
                //    BeforeScroll(g3, null, g3, null, g2, null, hScrollBar3);
                //}
                //else
                //    e.Cancel = true;
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g3, null, g3, null, g2, null, hScrollBar3);
                BeforeScroll(g3, null, g3, null, g2, g2, null, hScrollBar3);
                // End TT#2930 - JSmith - Scrolling Issue
                // End TT#1144
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void g4_BeforeScroll(object sender, RangeEventArgs e)
		{
			try
			{
                // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                //if (_isScrolling || _arrowScroll) 
                //{
                //    BeforeScroll(g4, g6, g1, g4, g1, vScrollBar2, hScrollBar1);
                //}
                //else
                //    e.Cancel = true;
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g4, g6, g1, g4, g1, vScrollBar2, hScrollBar1);
                BeforeScroll(g4, g6, g1, g4, g1, g5, vScrollBar2, hScrollBar1);
                // End TT#2930 - JSmith - Scrolling Issue
                // End TT#1144
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        private void g5_BeforeScroll(object sender, RangeEventArgs e)
        {
            try
            {
                // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                //if (_isScrolling || _arrowScroll)
                //{
                //    BeforeScroll(g5, g4, g2, g4, g2, vScrollBar2, hScrollBar2);
                //}
                //else
                //    e.Cancel = true;
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g5, g4, g2, g4, g2, vScrollBar2, hScrollBar2);
                //BeforeScroll(g5, g4, g2, g4, g2, g5, vScrollBar2, hScrollBar2);   // TT#3563 - RMatelic - Grid horizontal scrolling issues in Style View and Size View
                // End TT#2930 - JSmith - Scrolling Issue
                // End TT#1144
                // Begin TT#1073 - RMatelic - GA style review use the scroll bar to the right and the rows are out of sync
                // Begin TT#3563 - RMatelic - Grid horizontal scrolling issues in Style View and Size View
                //if (e.NewRange.LeftCol == e.OldRange.LeftCol && e.NewRange.RightCol == e.OldRange.RightCol)
                if (e.NewRange.LeftCol == e.OldRange.LeftCol && e.NewRange.RightCol == e.OldRange.RightCol && !_vScrollBar2Moved)
                // End TT#1073
                {
                    e.Cancel = true;
                }
                else
                {
                    BeforeScroll(g5, g4, g2, g4, g2, g5, vScrollBar2, hScrollBar2);
                }
                // End TT#3563  
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        
        private void g6_BeforeScroll(object sender, RangeEventArgs e)
		{
			try
			{
                // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                //if (_isScrolling || _arrowScroll)  
                //{
                //    BeforeScroll(g6, g4, g3, g4, g3, vScrollBar2, hScrollBar3);
                //}
                //else
                //    e.Cancel = true;
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g6, g4, g3, g4, g3, vScrollBar2, hScrollBar3);
                BeforeScroll(g6, g4, g3, g4, g3, g5, vScrollBar2, hScrollBar3);
                // End TT#2930 - JSmith - Scrolling Issue
                // End TT#1144
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void g7_BeforeScroll(object sender, RangeEventArgs e)
		{
			try
			{
                // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                //if (_isScrolling || _arrowScroll)
                //{
                //    BeforeScroll(g7, g7, g1, g7, g1, vScrollBar3, hScrollBar1);
                //}
                //else
                //    e.Cancel = true;
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g7, g9, g1, g7, g1, vScrollBar3, hScrollBar1);
                BeforeScroll(g7, g9, g1, g7, g1, g8, vScrollBar3, hScrollBar1);
                // End TT#2930 - JSmith - Scrolling Issue
                // End TT#1144
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void g8_BeforeScroll(object sender, RangeEventArgs e)
		{
			try
			{
                // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                //if (_isScrolling || _arrowScroll)
                //{
                //    BeforeScroll(g8, g7, g2, g7, g2, vScrollBar3, hScrollBar2);
                //}
                //else
                //    e.Cancel = true;
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g8, g7, g2, g7, g2, vScrollBar3, hScrollBar2);
                BeforeScroll(g8, g7, g2, g7, g2, g8, vScrollBar3, hScrollBar2);
                // End TT#2930 - JSmith - Scrolling Issue
                // End TT#1144
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void g9_BeforeScroll(object sender, RangeEventArgs e)
		{
			try
			{
                // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                //if (_isScrolling || _arrowScroll)
                //{
                //    BeforeScroll(g9, g7, g3, g7, g3, vScrollBar3, hScrollBar3);
                //}
                //else
                //    e.Cancel = true;
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g9, g7, g3, g7, g3, vScrollBar3, hScrollBar3);
                BeforeScroll(g9, g7, g3, g7, g3, g8, vScrollBar3, hScrollBar3);
                // End TT#2930 - JSmith - Scrolling Issue
                // End TT#1144
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void g10_BeforeScroll(object sender, RangeEventArgs e)
		{
			try
			{
                // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                //if (_isScrolling || _arrowScroll) 
                //{
                //    BeforeScroll(g10, g10, g1, g10, g1, vScrollBar4, hScrollBar1);
                //}
                //else
                //    e.Cancel = true;
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g10, g10, g1, g10, g1, vScrollBar4, hScrollBar1);
                BeforeScroll(g10, g10, g1, g10, g1, g11, vScrollBar4, hScrollBar1);
                // End TT#2930 - JSmith - Scrolling Issue
                // End TT#1144
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void g11_BeforeScroll(object sender, RangeEventArgs e)
		{
			try
			{
                // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                //if (_isScrolling || _arrowScroll) 
                //{
                //    BeforeScroll(g11, g10, g2, g10, g2, vScrollBar4, hScrollBar2);
                //}
                //else
                //    e.Cancel = true;
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g11, g10, g2, g10, g2, vScrollBar4, hScrollBar2);
                BeforeScroll(g11, g10, g2, g10, g2, g11, vScrollBar4, hScrollBar2);
                // End TT#2930 - JSmith - Scrolling Issue
                // End TT#1144
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void g12_BeforeScroll(object sender, RangeEventArgs e)
		{
			try
			{
                // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                //if (_isScrolling || _arrowScroll) 
                //{
                //    BeforeScroll(g12, g10, g3, g10, g3, vScrollBar4, hScrollBar3);
                //}
                //else
                //    e.Cancel = true;
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g12, g10, g3, g10, g3, vScrollBar4, hScrollBar3);
                BeforeScroll(g12, g10, g3, g10, g3, g11, vScrollBar4, hScrollBar3);
                // End TT#2930 - JSmith - Scrolling Issue
                // End TT#1144
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void BeforeScroll(
			C1FlexGrid aGrid,
			C1FlexGrid aRowCompGrid,
			C1FlexGrid aColCompGrid,
			C1FlexGrid aRowHeaderGrid,
			C1FlexGrid aColHeaderGrid,
            C1FlexGrid aTotalsGrid, // TT#2930 - JSmith - Scrolling Issue
			VScrollBar aVScrollBar,
			HScrollBar aHScrollBar)
		{
			try
			{
				if (!_isScrolling)
				{
					if (aRowCompGrid != null && aVScrollBar != null)
					{
						if (aGrid.ScrollPosition.Y < aRowCompGrid.ScrollPosition.Y)
						{
							//aVScrollBar.Value = Math.Min(CalculateGroupFromDetail(aRowHeaderGrid, aGrid.TopRow) + 1, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1);
                            // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                            //aVScrollBar.Value = Math.Min(aGrid.TopRow + 1, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1);

                            // For some reason when the top row is (bottom row - 1), it won't scroll to the bottom row so force the bottom row
                            if (aVScrollBar.Value < aGrid.TopRow)
                            {
                                // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                //aVScrollBar.Value = aGrid.TopRow;
                                SetVScrollValue(aVScrollBar, aGrid.TopRow);
                                // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                            }
                            else if (aVScrollBar.Value == aGrid.TopRow)
                            {
                                if (aVScrollBar.Value == aGrid.BottomRow - 1)
                                {
                                    // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                    //aVScrollBar.Value = aGrid.BottomRow;
                                    SetVScrollValue(aVScrollBar, aGrid.BottomRow);
                                    // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                }
                                // Begin TT#2017 - RMatelic - Scroll MisCalculation
                                else
                                {
                                    Point point = new Point(aRowCompGrid.ScrollPosition.X, aGrid.ScrollPosition.Y);
                                    aRowCompGrid.ScrollPosition = point;
                                    // Begin TT#2930 - JSmith - Scrolling Issue
                                    if (aTotalsGrid != null &&
                                        aGrid.Name != aTotalsGrid.Name)
                                    { 
                                        _vScrollBar2Moved = true;               // TT#4242 - RMatelic - aStyle Review lines misaligned with down arrow
                                        aTotalsGrid.ScrollPosition = point;
                                        _vScrollBar2Moved = false;              // TT#4242 - RMatelic - aStyle Review lines misaligned with down arrow
                                    }
                                    // End #2930 - JSmith - Scrolling Issue
                                }
                                // End TT#2017
                            }
                            // End TT#1144
						}
						else if (aGrid.ScrollPosition.Y > aRowCompGrid.ScrollPosition.Y)
						{
							//aVScrollBar.Value = CalculateGroupFromDetail(aRowHeaderGrid, aGrid.TopRow);
                            // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                            //if (aGrid.TopRow >= 0)
                            //    aVScrollBar.Value =  aGrid.TopRow;

                            if (aGrid.TopRow >= 0)
                            {
                                if (aVScrollBar.Value == aGrid.BottomRow && (aGrid.BottomRow - 1 >= 0))
                                {
                                    // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                    //aVScrollBar.Value = aGrid.BottomRow - 1;
                                    SetVScrollValue(aVScrollBar, aGrid.BottomRow - 1);
                                    // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                }
                                else
                                {
                                    // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                    //aVScrollBar.Value = aGrid.TopRow;
                                    SetVScrollValue(aVScrollBar, aGrid.TopRow);
                                    // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                }
                            }
                            // End TT#1144
						}
					}
					if (aColCompGrid != null && aHScrollBar != null)
					{
                        // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                        // added    '&& _arrowScroll'     to the following conditions
                        if (aGrid.ScrollPosition.X < aColCompGrid.ScrollPosition.X && _arrowScroll)
						{
							//aHScrollBar.Value = Math.Min(CalculateGroupFromDetail(aColHeaderGrid, aGrid.LeftCol) + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1);
                            // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                            //aHScrollBar.Value = Math.Min(aGrid.LeftCol + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1);
                            SetHScrollValue(aHScrollBar, Math.Min(aGrid.LeftCol + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1));
                            // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
						}
                        else if (aGrid.ScrollPosition.X > aColCompGrid.ScrollPosition.X && _arrowScroll)
						{
							//aHScrollBar.Value = CalculateGroupFromDetail(aColHeaderGrid, aGrid.LeftCol);
							if (aGrid.LeftCol >= 0)
                                // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                //aHScrollBar.Value =  aGrid.LeftCol;
                                SetHScrollValue(aHScrollBar, aGrid.LeftCol);
                                // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
						}
                        // Begin TT#3563 - RMatelic - Grid horizontal scrolling issues in Style View and Size View
                        // Begin TT#199-MD - RMatelic - Column headers not moving with the cells while using arrow keys
                        //else if (aGrid.ScrollPosition.X < aColCompGrid.ScrollPosition.X)
                        //{
                        //    if (aGrid.Name == "g6" || aGrid.Name == "g9" || aGrid.Name == "g12")
                        //    {
                        //        Point point = aGrid.ScrollPosition;
                        //        Point g3point = g3.ScrollPosition;
                        //        Point g6point = g6.ScrollPosition;
                        //        Point g9point = g9.ScrollPosition;
                        //        Point g12point = g12.ScrollPosition;
                        //        g3point.X = point.X;
                        //        g6point.X = point.X;
                        //        g9point.X = point.X;
                        //        g12point.X = point.X;
                        //        g3.ScrollPosition = g3point;
                        //        g6.ScrollPosition = g6point;
                        //        g9.ScrollPosition = g9point;
                        //        g12.ScrollPosition = g12point;
                        //    }
                        //}
                        // End TT#199-MD
                        else if (aGrid.ScrollPosition.X != aColCompGrid.ScrollPosition.X)
                        {
                            Point point = aGrid.ScrollPosition;
                            bool scrollRight = (aGrid.ScrollPosition.X < aColCompGrid.ScrollPosition.X) ? true : false;
                            switch (aGrid.Name)
                            {
                                case "g6":
                                case "g9":
                                case "g12":
                                    Point g3point = g3.ScrollPosition;
                                    Point g6point = g6.ScrollPosition;
                                    Point g9point = g9.ScrollPosition;
                                    Point g12point = g12.ScrollPosition;
                                    g3point.X = point.X;
                                    g6point.X = point.X;
                                    g9point.X = point.X;
                                    g12point.X = point.X;
                                    g3.ScrollPosition = g3point;
                                    g6.ScrollPosition = g6point;
                                    g9.ScrollPosition = g9point;
                                    g12.ScrollPosition = g12point;
                                    break;

                                case "g5":
                                case "g8":
                                case "g11":
                                    Point g2point = g2.ScrollPosition;
                                    Point g5point = g5.ScrollPosition;
                                    Point g8point = g8.ScrollPosition;
                                    Point g11point = g11.ScrollPosition;
                                    g2point.X = point.X;
                                    g5point.X = point.X;
                                    g8point.X = point.X;
                                    g11point.X = point.X;
                                    g2.ScrollPosition = g2point;
                                    g5.ScrollPosition = g5point;
                                    g8.ScrollPosition = g8point;
                                    g11.ScrollPosition = g11point;
                                    break;

                                case "g4":
                                case "g7":
                                case "g10":
                                    Point g1point = g1.ScrollPosition;
                                    Point g4point = g4.ScrollPosition;
                                    Point g7point = g7.ScrollPosition;
                                    Point g10point = g10.ScrollPosition;
                                    g1point.X = point.X;
                                    g4point.X = point.X;
                                    g7point.X = point.X;
                                    g10point.X = point.X;
                                    g1.ScrollPosition = g1point;
                                    g4.ScrollPosition = g4point;
                                    g7.ScrollPosition = g7point;
                                    g10.ScrollPosition = g10point;
                                    break;
                            }
                            if (scrollRight)
                            {
                                // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                //aHScrollBar.Value = Math.Min(aGrid.LeftCol + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1);
                                SetHScrollValue(aHScrollBar, Math.Min(aGrid.LeftCol + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1));
                                // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                            }
                            else if (aGrid.LeftCol >= 0)
                            {
                                // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                //aHScrollBar.Value = aGrid.LeftCol;
                                SetHScrollValue(aHScrollBar, aGrid.LeftCol);
                                // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                            }
                        }
                        // End TT#3563 
						if ((aHScrollBar == hScrollBar2 ||  aHScrollBar == hScrollBar3) && _arrowScroll)
                        // End TT#1144
						{
							((ScrollBarValueChanged)aHScrollBar.Tag)(aHScrollBar.Value);
							if (aHScrollBar == hScrollBar3) 
								AdjustG2Grid();
							else
								AdjustG3Grid(_scroll2Direction);
						}	
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
        private void SetHScrollValue(HScrollBar aHScrollBar, int iValue)
        {
            if (iValue < aHScrollBar.Minimum)
            {
                aHScrollBar.Value = aHScrollBar.Minimum;
            }
            else if (iValue > aHScrollBar.Maximum)
            {
                aHScrollBar.Value = aHScrollBar.Maximum;
            }
            else
            {
                aHScrollBar.Value = iValue;
            }
        }

        private void SetVScrollValue(VScrollBar aVScrollBar, int iValue)
        {
            if (iValue < aVScrollBar.Minimum)
            {
                aVScrollBar.Value = aVScrollBar.Minimum;
            }
            else if (iValue > aVScrollBar.Maximum)
            {
                aVScrollBar.Value = aVScrollBar.Maximum;
            }
            else
            {
                aVScrollBar.Value = iValue;
            }
        }
        // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                

		#endregion

		/// <summary>
		/// Used when the user used the arrow buttons to scroll the grids
		/// </summary>
		private void GridKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			C1FlexGrid grid;
			int whichGrid;
			try
			{
				grid = (C1FlexGrid)sender;
				whichGrid = ((GridTag)grid.Tag).GridId;
				switch (e.KeyData)
				{
					case Keys.Right:
						_arrowScroll = true;
						switch((FromGrid)whichGrid)
						{
							case FromGrid.g5:
							case FromGrid.g8:
							case FromGrid.g11:
								_scroll2Direction = "right";
								break;
							default:
								break;
						}
						break;
					case Keys.Left:
						_arrowScroll = true;
						switch((FromGrid)whichGrid)
						{
							case FromGrid.g5:
							case FromGrid.g8:
							case FromGrid.g11:
								_scroll2Direction = "left";
								break;
							default:
								break;
						}
						break;

                    // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                    case Keys.PageDown:
                    case Keys.PageUp:
                        switch ((FromGrid)whichGrid)
                        {
                            case FromGrid.g1:
                            case FromGrid.g2:
                            case FromGrid.g3:
                                e.Handled = true;
                                break;

                            case FromGrid.g4:
                            case FromGrid.g5:
                            case FromGrid.g6:
                                GridKeyDown(vScrollBar2, e);
                                e.Handled = true;
                                break;

                            case FromGrid.g7:
                            case FromGrid.g8:
                            case FromGrid.g9:
                                GridKeyDown(vScrollBar3, e);
                                e.Handled = true;
                                break;

                            case FromGrid.g10:
                            case FromGrid.g11:
                            case FromGrid.g12:
                                GridKeyDown(vScrollBar4, e);
                                e.Handled = true;
                                break;
                        }
                        break;
                    // End TT#1542

					default:
						// BEGIN MID Track #3840 - unhandled exception on PageUp key
						switch((FromGrid)whichGrid)
						{
							case FromGrid.g1:
							case FromGrid.g2:
							case FromGrid.g3:
								e.Handled = true;
								break;
							default:
								break;
						}
						// END MID Track #3840
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
        private void GridKeyDown(VScrollBar aScrollBar, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                C1FlexGrid grid; 
                int rowCalc = 0;
                int rowCount = 0;
                int scrollBarValue = 0;
                int gridLeftCol = 0;
                bool showCell = false;

                if (aScrollBar == vScrollBar2)
                {
                    grid = g4;
                }
                else if (aScrollBar == vScrollBar3)
                {
                    grid = g7;
                }
                else 
                {
                    grid = g10;
                }

                rowCalc = grid.BottomRow - grid.TopRow;

                gridLeftCol = GetGridLeftCol(grid);

                switch (e.KeyData)
                {
                    case Keys.PageDown:
                        scrollBarValue = Math.Min(aScrollBar.Value + rowCalc, aScrollBar.Maximum);
                        if (scrollBarValue > (aScrollBar.Maximum - rowCalc))
                        {
                            scrollBarValue = aScrollBar.Maximum - rowCalc;
                            showCell = true;
                        }
                        aScrollBar.Value = scrollBarValue;
                        if (showCell)
                        {
                            grid.ShowCell(grid.BottomRow, gridLeftCol);
                        }
                        e.Handled = true;
                        break;

                    case Keys.PageUp:
                        aScrollBar.Value = Math.Max(aScrollBar.Value - rowCalc, aScrollBar.Minimum);
                        e.Handled = true;
                        break;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                HandleException(exc);
            }
        }

        private int GetGridLeftCol(C1FlexGrid aGrid)
        {
            int leftColumn = 0;
            try
            {
                for (int i = 0; i < aGrid.Cols.Count; i++)
                {
                    if (aGrid.Cols[i].Visible)
                    {
                        leftColumn = i;
                        break;
                    }
                }
                return leftColumn;
            }
            catch
            {
                throw;
            }
        }
        // End TT#1542

        private void SetNeedAnalysisGrid(C1FlexGrid aG4)
        {
            if (_trans.AllocationViewType == eAllocationSelectionViewType.Style)
            {
                if (_showNeedGrid)
                {
                    this.g1.ContextMenu = this.g1ContextMenu;
                }
                else
                {
                    this.g1.ContextMenu = null;
                    // leave the 1st column (0) visible
                    for (int i = 1; i < g1.Cols.Count; i++)
                    {
                        if (_exporting)
                        {
                            aG4.Cols[i].Visible = g1.Cols[i].Visible;
                        }
                        else
                        {
                            g1.Cols[i].Visible = false;
                            aG4.Cols[i].Visible = false;
                            g7.Cols[i].Visible = false;
                            g10.Cols[i].Visible = false;
                        }
                    }
                }
            }
        }

		private void btnVelocity_Click(object sender, System.EventArgs e)
		{
            // Start TT#2054-MD - AGallagher - Then when apply 2nd rule receive a Null Reference Exception when selecting Apply changes.
            if (_trans.AssortmentView != null)
            {
                this.Close();
                MIDRetail.Windows.frmVelocityMethod frmVelocityMethod;
                frmVelocityMethod = (MIDRetail.Windows.frmVelocityMethod)_trans.VelocityWindow;
                frmVelocityMethod.ActivatedFromStyleView = false; 
                frmVelocityMethod.Activate();
            }
            else
			{
            // End TT#2054-MD - AGallagher - Then when apply 2nd rule receive a Null Reference Exception when selecting Apply changes.
                MIDRetail.Windows.frmVelocityMethod frmVelocityMethod;
			    frmVelocityMethod = (MIDRetail.Windows.frmVelocityMethod)_trans.VelocityWindow;
                frmVelocityMethod.ActivatedFromStyleView = true; //TT#262 - added to tell the form where it is being activated from - apicchetti
			    frmVelocityMethod.Activate();
            }  // TT#2054-MD - AGallagher - Then when apply 2nd rule receive a Null Reference Exception when selecting Apply changes.
		}

		public delegate void ScrollBarValueChanged(int aNewValue);
		
		// BEGIN MID Track #4077 - vertical splitters not correct when window first opens. 
		// only happens in Windows Server 2003 SP1, so this is a workaround
		public void ResetSplitters()
		{
			try
			{
				SetV1SplitPosition();
				SetV2SplitPosition();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		// END MID Track #4077

        private void cboAction_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboAction_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cmbFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cmbFilter_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cmbView_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cmbView_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cmbAttributeSet_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cmbStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cmbStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }
        
        // Begin TT#3686 - RMatelic - Vecity Rule Type Qty does not accept decimals for WOS or Forward WOS Rules
        private bool RequiresDecimalFormat(TagForColumn aColumnTag, CellRange aCellRange, ref bool aProtect)
        {
            try
            {
                bool requiresDecimalFormat = false;
                C1FlexGrid colGrid = null;
                AllocationWaferCoordinate wafercoord = GetAllocationCoordinate(aColumnTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                AllocationWaferVariable varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord.Key);

                if ((eAllocationWaferVariable)varProf.Key == eAllocationWaferVariable.VelocityRuleTypeQty) 
                {
                    AllocationWaferCoordinate wafercoordHdr = GetAllocationCoordinate(aColumnTag.CubeWaferCoorList, eAllocationCoordinateType.Header);
                    colGrid = GetColumnGrid(g6);

                    if ((eAllocationWaferVariable)varProf.Key == eAllocationWaferVariable.VelocityRuleTypeQty)
                    {
                        for (int i = 0; i < colGrid.Cols.Count; i++)
                        {
                            TagForColumn colTag = (TagForColumn)colGrid.Cols[i].UserData;
                            AllocationWaferCoordinate wafercoord2 = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                            AllocationWaferVariable varProf2 = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord2.Key);
                            AllocationWaferCoordinate wafercoordHdr2 = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Header);
                            if ((eAllocationWaferVariable)varProf2.Key == eAllocationWaferVariable.VelocityRuleType && wafercoordHdr2.Key == wafercoordHdr.Key)
                            {
                                CellRange c2 = g6.GetCellRange(aCellRange.TopRow, i);
                                if (Convert.ToInt32(c2.Data, CultureInfo.CurrentUICulture) == Convert.ToInt32(eVelocityRuleType.WeeksOfSupply, CultureInfo.CurrentUICulture)
                                 || Convert.ToInt32(c2.Data, CultureInfo.CurrentUICulture) == Convert.ToInt32(eVelocityRuleType.ForwardWeeksOfSupply, CultureInfo.CurrentUICulture))
                                {
                                    requiresDecimalFormat = true;
                                }
                                else if (Convert.ToInt32(c2.Data, CultureInfo.CurrentUICulture) == Convert.ToInt32(eVelocityRuleType.AbsoluteQuantity, CultureInfo.CurrentUICulture)
                                 || Convert.ToInt32(c2.Data, CultureInfo.CurrentUICulture) == Convert.ToInt32(eVelocityRuleType.ShipUpToQty, CultureInfo.CurrentUICulture))
                                {
                                }
                                else
                                {
                                    aProtect = true;
                                }
                                break;
                            }
                        }
                    }
                }
                return requiresDecimalFormat;
            }
            catch  
            {
                throw;
            }
        }

        private void g6_CellChanged(object sender, RowColEventArgs e)
        {
            try
            {
                if (e.Col == -1)    // Added per CompoenentOne documentation
                {
                    return;
                }
                int col = 0;

                C1FlexGrid colGrid = GetColumnGrid(g6);
                TagForColumn ColumnTag = (TagForColumn)colGrid.Cols[e.Col].UserData;
                AllocationWaferCoordinate wafercoord = GetAllocationCoordinate(ColumnTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                AllocationWaferVariable varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord.Key);
                AllocationWaferCoordinate wafercoordHdr = GetAllocationCoordinate(ColumnTag.CubeWaferCoorList, eAllocationCoordinateType.Header);

                g6.BeginUpdate();
                if ((eAllocationWaferVariable)varProf.Key == eAllocationWaferVariable.VelocityRuleType)
                {
                    CellStyle curCellStyle = g6.GetCellStyleDisplay(e.Row, e.Col); // dummy assign to avoid non-assigment error
                    int cellData = Convert.ToInt32(g6.GetData(e.Row, e.Col), CultureInfo.CurrentUICulture); ;
                    for (int i = 0; i < colGrid.Cols.Count; i++)
                    {
                        TagForColumn colTag = (TagForColumn)colGrid.Cols[i].UserData;
                        AllocationWaferCoordinate wafercoord2 = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                        AllocationWaferVariable varProf2 = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord2.Key);
                        AllocationWaferCoordinate wafercoordHdr2 = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Header);
                        if ((eAllocationWaferVariable)varProf2.Key == eAllocationWaferVariable.VelocityRuleTypeQty && wafercoordHdr2.Key == wafercoordHdr.Key)
                        {
                            curCellStyle = g6.GetCellStyleDisplay(e.Row, i);
                            col = i;
                            break;
                        }
                    }

                    CellStyle cellStyle  = g6.Styles.Add(null);
                    cellStyle.MergeWith(curCellStyle);

                    eVelocityRuleRequiresQuantity vrq = (eVelocityRuleRequiresQuantity)(Convert.ToInt32(cellData, CultureInfo.CurrentUICulture));
                    if (Enum.IsDefined(typeof(eVelocityRuleRequiresQuantity), vrq))
                    {
                        cellStyle.Font = _theme.EditableFont;
                        if (cellData == Convert.ToInt32(eVelocityRuleType.WeeksOfSupply, CultureInfo.CurrentUICulture)
                         || cellData == Convert.ToInt32(eVelocityRuleType.ForwardWeeksOfSupply, CultureInfo.CurrentUICulture))
                        {
                            cellStyle.Format = "##0.0";
                        }
                        else
                        {
                            if (Convert.ToDouble(g6.GetData(e.Row, col), CultureInfo.CurrentUICulture) < 1.0)
                            {
                                g6.SetData(e.Row, col, 0);
                            }
                            cellStyle.Format = "######0";
                        }
                    }
                    else
                    {
                        cellStyle.Font = _theme.DisplayOnlyFont;
                        cellStyle.Format = "######0";
                        g6.SetData(e.Row, col, 0);
                    }
                    g6.SetCellStyle(e.Row, col, cellStyle);
                    // Begin TT#1288-MD - RMatelic - Store Detail column widths are wider than Version 5.3 >> Unrelated - keeps the font italicized
                    curCellStyle.Font = _theme.EditableFont;
                    g6.SetCellStyle(e.Row, e.Col, curCellStyle);
                    // End TT#1288-MD  
                }

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                HandleException(exc);
            }
            finally
            {
                g6.EndUpdate();
            }
        }
        // End TT#3686

        // Begin TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
        override protected eMIDTextCode GetPendingMessage()
        {
            // Begin TT#2063-MD - JSmith - In an Asst close style review with the X in the top right corner.  Receieve mssg about Group allocation and values changing.  The only thing changed was the View.  Would not expect the mssg.
            //AllocationProfile ap = _trans.GetAssortmentMemberProfile(-1);
            AllocationProfile ap = _trans.GetAssortmentProfile();
            // End TT#2063-MD - JSmith - In an Asst close style review with the X in the top right corner.  Receieve mssg about Group allocation and values changing.  The only thing changed was the View.  Would not expect the mssg.
            if (_trans.AssortmentView != null)
                // Begin TT#1973-MD - JSmith - With Group Allocation open - Select to Close Style Review and receive an object reference error.  
                //if (ap.AsrtType != (int)eAssortmentType.GroupAllocation)
                if (ap != null
                    && ap.AsrtType != (int)eAssortmentType.GroupAllocation)
                // End TT#1973-MD - JSmith - With Group Allocation open - Select to Close Style Review and receive an object reference error.
               {
                   return eMIDTextCode.msg_SaveAssortmentPendingChanges;
               }
               else
               {
                    return eMIDTextCode.msg_SaveGroupPendingChanges;
               }    

            else
            {
                return base.GetPendingMessage();
            }
        }
        protected override bool UndoSaveChanges()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (_trans.AssortmentProfile != null)
                {
                    _trans.RefreshSelectedHeaders(false);
                }

                ChangePending = false;

                UpdateOtherViews();

                return true;
            }
            catch
            {
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        // End TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
	}
}
