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

	public class SizeView : MIDFormBase
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
		private System.Windows.Forms.ContextMenu g2g3ContextMenu;
		private System.Windows.Forms.ContextMenu g1ContextMenu;
		private System.Windows.Forms.MenuItem mnuSeperator1;
        private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.MenuItem mnuSortByDefault;
		private System.ComponentModel.IContainer components;
		#endregion

		private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
		private C1.Win.C1FlexGrid.C1FlexGrid g1;
		private System.Windows.Forms.Panel pnlCorner;
		private System.Windows.Forms.MenuItem mnuColumnChooser23;
		private System.Windows.Forms.MenuItem mnuFreezeColumn23;
		private System.Windows.Forms.MenuItem mnuColumnChooser1;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.RadioButton rbHeader;
        private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Button btnProcess;
		private System.Windows.Forms.GroupBox gbxGroupBy;
		private System.Windows.Forms.RadioButton rbColor;
		private System.Windows.Forms.GroupBox gbxSeqMatrix;
		private System.Windows.Forms.RadioButton rbSequential;
		private System.Windows.Forms.RadioButton rbSizeMatrix;
		private System.Windows.Forms.GroupBox gbxSizeVar;
		private System.Windows.Forms.RadioButton rbVariable;
        private System.Windows.Forms.RadioButton rbSize;
        // Begin Track #4872 - JSmith - Global/User Attributes
        private MIDAttributeComboBox cmbStoreAttribute;
        // End Track #4872
		private System.Windows.Forms.MenuItem mnuFreezeColumn1;
        private AllocationWaferCellChangeList _allocationWaferCellChangeList; // TT#59 Implement Temp Locks
        private System.Windows.Forms.MenuItem mnuHeaderAllocationCriteria;  // TT#59 Implement Temp Locks
        private MIDComboBoxEnh cmbAttributeSet;
        private MIDComboBoxEnh cmbView;
        private MIDComboBoxEnh cmbFilter;
        private MIDComboBoxEnh cboAction;    
        // Begin TT#2027 - JSmith - System out of Argument Range
        private bool _arrowScroll;
        // End TT#2027
        private bool _saveCurrentColumns = true;  // TT#2380 - JSmith - Column Changes in Size Review


        public SizeView(ExplorerAddressBlock eab, ApplicationSessionTransaction trans): base(trans.SAB)
		{
			_trans = trans;
			_sab = _trans.SAB;
            _eab = eab;
			_theme = _sab.ClientServerSession.Theme;
			_awExplorer = (MIDRetail.Windows.AllocationWorkspaceExplorer)_trans.AllocationWorkspaceExplorer;
			InitializeComponent();
			_curGroupBy = _trans.AllocationGroupBy ;
            _allocationWaferCellChangeList = new AllocationWaferCellChangeList();  // TT#59 Implement Temp Locks
			_curAttribute    = _trans.AllocationStoreAttributeID;
			_curAttributeSet = _trans.AllocationStoreGroupLevel;
		}
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
                //BEGIN TT#6-MD-VStuart - Single Store Select
                //this.cmbFilter.SelectionChangeCommitted -= new System.EventHandler(this.cmbFilter_SelectionChangeCommitted);
                //this.cmbStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cmbStoreAttribute_SelectionChangeCommitted);
                //this.cmbAttributeSet.SelectionChangeCommitted -= new System.EventHandler(this.cmbAttributeSet_SelectionChangeCommitted);
                //this.cmbAttributeSet.SelectionChangeCommitted -= new System.EventHandler(this.cmbAttributeSet_SelectionChangeCommitted);
                //this.cboAction.SelectionChangeCommitted -= new System.EventHandler(this.cboAction_SelectionChangeCommitted);
                this.cmbAttributeSet.SelectionChangeCommitted -= new System.EventHandler(this.cmbAttributeSet_SelectionChangeCommitted);
                this.cmbView.SelectionChangeCommitted -= new System.EventHandler(this.cmbView_SelectionChangeCommitted);
                this.cmbFilter.SelectionChangeCommitted -= new System.EventHandler(this.cmbFilter_SelectionChangeCommitted);
                this.cboAction.SelectionChangeCommitted -= new System.EventHandler(this.cboAction_SelectionChangeCommitted);
                this.cmbFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cmbFilter_DragDrop);
                this.cmbFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cmbFilter_DragEnter);
                this.cmbFilter.DragOver -= new System.Windows.Forms.DragEventHandler(this.cmbFilter_DragOver);
                //END TT#6-MD-VStuart - Single Store Select
                this.btnApply.Click -= new System.EventHandler(this.btnApply_Click);
				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
				this.rbColor.CheckedChanged -= new System.EventHandler(this.rbColor_CheckedChanged);
				this.rbHeader.CheckedChanged -= new System.EventHandler(this.rbHeader_CheckedChanged);
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
                // End TT#1542ty
				
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
                // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                this.g5.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
                // End TT#1542
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
                // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                this.g8.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
                // End TT#1542
				this.g8.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				
				this.s10.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
				this.s10.DoubleClick -= new System.EventHandler(this.g10SplitterDoubleClick);
					
				this.g11.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g11.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g11.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g11.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g11_OwnerDrawCell);
				this.g11.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g11_BeforeScroll);
				this.g11.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
                // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                this.g11.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);  
                // End TT#1542
				this.g11.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				
				this.hScrollBar2.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
				this.mnuColumnChooser23.Click -= new System.EventHandler(this.mnuColumnChooser23_Click);
				
				this.VerticalSplitter2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.VerticalSplitter2_SplitterMoved);
				this.VerticalSplitter2.DoubleClick -= new System.EventHandler(this.VerticalSplitter2_DoubleClick);
				this.g6.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g6.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g6.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g6.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g6_OwnerDrawCell);
				this.g6.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g6_BeforeScroll);
				this.g6.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
                // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                this.g6.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
                // End TT#1542
				this.g6.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
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
                // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                this.g9.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
                // End TT#1542
				this.g9.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
					
				this.s11.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
				this.s11.DoubleClick -= new System.EventHandler(this.g10SplitterDoubleClick);
				
				this.g12.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g12.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g12.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g12.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g12_OwnerDrawCell);
				this.g12.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g12_BeforeScroll);
				this.g12.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
                // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
                this.g12.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
                // End TT#1542
				this.g12.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				
				this.hScrollBar3.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.hScrollBar3_Scroll);
				this.utmMain.ToolClick -= new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.utmMain_ToolClick);
                // Begin TT#1761 - RMatelic - Size Review Lacks Page Down Functionality
                this.vScrollBar2.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.vScrollBar2_Scroll);
                // End TT#1761

                this.cboAction.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboAction_MIDComboBoxPropertiesChangedEvent);
                this.cmbFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cmbFilter_MIDComboBoxPropertiesChangedEvent);
                this.cmbView.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cmbView_MIDComboBoxPropertiesChangedEvent);
                this.cmbAttributeSet.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cmbAttributeSet_MIDComboBoxPropertiesChangedEvent);
                this.cmbStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cmbStoreAttribute_MIDComboBoxPropertiesChangedEvent);
				
				if (_quickFilter !=null)
				{
					_quickFilter.OnComponentSelectedIndexChangeEventHandler -= new QuickFilter.ComponentSelectedIndexChangeEventHandler(OnComponentSelectedIndexChangeEventHandler);
					_quickFilter.OnValidateFieldsHandler -= new QuickFilter.ValidateFieldsHandler(OnValidateFieldsHandler);
				}

				if (_frmThemeProperties != null)
				{
					_frmThemeProperties.ApplyButtonClicked -= new EventHandler(StylePropertiesOnChanged);
				}

                //BEGIN TT#6-MD-VStuart - Single Store Select
                //this.cmbView.SelectionChangeCommitted -= new System.EventHandler(this.cmbView_SelectionChangeCommitted);    // TT#456 - RMatelic - Add view to Size Review 
                //this.cmbAttributeSet.SelectionChangeCommitted -= new System.EventHandler(this.cmbAttributeSet_SelectionChangeCommitted);
                //this.cmbView.SelectionChangeCommitted -= new System.EventHandler(this.cmbView_SelectionChangeCommitted);
                //this.cmbFilter.SelectionChangeCommitted -= new System.EventHandler(this.cmbFilter_SelectionChangeCommitted);
                //this.cboAction.SelectionChangeCommitted -= new System.EventHandler(this.cboAction_SelectionChangeCommitted);
                //END TT#6-MD-VStuart - Single Store Select

				//this.SizeChanged -= new System.EventHandler(this.SizeView_SizeChanged);
				this.Load -= new System.EventHandler(this.SizeView_Load);
				this.Activated -= new System.EventHandler(this.SizeView_Activated);
				this.Deactivate -= new System.EventHandler(this.SizeView_Deactivate);

				if (_trans.StyleView == null &&
					_trans.SummaryView == null &&
					_trans.SizeView == null &&
                    _trans.AssortmentView == null &&
					_trans.VelocityWindow == null)
				{
					_trans.Dispose();
				}
                // Begin TT#607-MD - RMatelic - Size Review not displaying new sizes added when header is dropped on an assortment placeholder
                else
                {
                    _trans.ResetFirstBuild(true);
                    _trans.ResetFirstBuildSize(true);
                    _trans.ResetSizeViewGroups();
                }
                //// End TT#607-MD
			}
			base.Dispose( disposing );
		}

		
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SizeView));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.cboAction = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cmbFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cmbView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cmbAttributeSet = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.gbxGroupBy = new System.Windows.Forms.GroupBox();
            this.rbColor = new System.Windows.Forms.RadioButton();
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
            this.gbxSizeVar = new System.Windows.Forms.GroupBox();
            this.rbVariable = new System.Windows.Forms.RadioButton();
            this.rbSize = new System.Windows.Forms.RadioButton();
            this.gbxSeqMatrix = new System.Windows.Forms.GroupBox();
            this.rbSizeMatrix = new System.Windows.Forms.RadioButton();
            this.rbSequential = new System.Windows.Forms.RadioButton();
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
            this.g2g3ContextMenu = new System.Windows.Forms.ContextMenu();
            this.mnuColumnChooser23 = new System.Windows.Forms.MenuItem();
            this.mnuSeperator1 = new System.Windows.Forms.MenuItem();
            this.mnuFreezeColumn23 = new System.Windows.Forms.MenuItem();
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
            this.gbxSizeVar.SuspendLayout();
            this.gbxSeqMatrix.SuspendLayout();
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
            this.pnlTop.Size = new System.Drawing.Size(1096, 30);
            this.pnlTop.TabIndex = 0;
            // 
            // cboAction
            // 
            this.cboAction.AutoAdjust = true;
            this.cboAction.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAction.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAction.DataSource = null;
            this.cboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAction.DropDownWidth = 154;
            this.cboAction.FormattingEnabled = false;
            this.cboAction.IgnoreFocusLost = false;
            this.cboAction.ItemHeight = 13;
            this.cboAction.Location = new System.Drawing.Point(784, 4);
            this.cboAction.Margin = new System.Windows.Forms.Padding(0);
            this.cboAction.MaxDropDownItems = 25;
            this.cboAction.Name = "cboAction";
            this.cboAction.SetToolTip = "";
            this.cboAction.Size = new System.Drawing.Size(151, 21);
            this.cboAction.TabIndex = 6;
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
            this.cmbFilter.DropDownWidth = 146;
            this.cmbFilter.FormattingEnabled = false;
            this.cmbFilter.IgnoreFocusLost = false;
            this.cmbFilter.ItemHeight = 13;
            this.cmbFilter.Location = new System.Drawing.Point(634, 4);
            this.cmbFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cmbFilter.MaxDropDownItems = 25;
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.SetToolTip = "";
            this.cmbFilter.Size = new System.Drawing.Size(141, 21);
            this.cmbFilter.TabIndex = 10;
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
            this.cmbView.DropDownWidth = 144;
            this.cmbView.FormattingEnabled = false;
            this.cmbView.IgnoreFocusLost = false;
            this.cmbView.ItemHeight = 13;
            this.cmbView.Location = new System.Drawing.Point(484, 4);
            this.cmbView.Margin = new System.Windows.Forms.Padding(0);
            this.cmbView.MaxDropDownItems = 25;
            this.cmbView.Name = "cmbView";
            this.cmbView.SetToolTip = "";
            this.cmbView.Size = new System.Drawing.Size(141, 21);
            this.cmbView.TabIndex = 11;
            this.cmbView.Tag = null;
            this.cmbView.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cmbView_MIDComboBoxPropertiesChangedEvent);
            this.cmbView.SelectionChangeCommitted += new System.EventHandler(this.cmbView_SelectionChangeCommitted);
            // 
            // cmbAttributeSet
            // 
            this.cmbAttributeSet.AutoAdjust = true;
            this.cmbAttributeSet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbAttributeSet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbAttributeSet.DataSource = null;
            this.cmbAttributeSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttributeSet.DropDownWidth = 147;
            this.cmbAttributeSet.FormattingEnabled = false;
            this.cmbAttributeSet.IgnoreFocusLost = false;
            this.cmbAttributeSet.ItemHeight = 13;
            this.cmbAttributeSet.Location = new System.Drawing.Point(331, 4);
            this.cmbAttributeSet.Margin = new System.Windows.Forms.Padding(0);
            this.cmbAttributeSet.MaxDropDownItems = 25;
            this.cmbAttributeSet.Name = "cmbAttributeSet";
            this.cmbAttributeSet.SetToolTip = "";
            this.cmbAttributeSet.Size = new System.Drawing.Size(147, 22);
            this.cmbAttributeSet.TabIndex = 12;
            this.cmbAttributeSet.Tag = null;
            this.cmbAttributeSet.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cmbAttributeSet_MIDComboBoxPropertiesChangedEvent);
            this.cmbAttributeSet.SelectionChangeCommitted += new System.EventHandler(this.cmbAttributeSet_SelectionChangeCommitted);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(1017, 4);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(64, 21);
            this.btnApply.TabIndex = 8;
            this.btnApply.Text = "Apply";
            this.toolTip1.SetToolTip(this.btnApply, "Apply current changes");
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Enabled = false;
            this.btnProcess.Location = new System.Drawing.Point(944, 4);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(64, 21);
            this.btnProcess.TabIndex = 7;
            this.btnProcess.Text = "Process";
            this.toolTip1.SetToolTip(this.btnProcess, "Process action");
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // gbxGroupBy
            // 
            this.gbxGroupBy.Controls.Add(this.rbColor);
            this.gbxGroupBy.Controls.Add(this.rbHeader);
            this.gbxGroupBy.Location = new System.Drawing.Point(8, 0);
            this.gbxGroupBy.Name = "gbxGroupBy";
            this.gbxGroupBy.Size = new System.Drawing.Size(166, 28);
            this.gbxGroupBy.TabIndex = 9;
            this.gbxGroupBy.TabStop = false;
            // 
            // rbColor
            // 
            this.rbColor.Location = new System.Drawing.Point(88, 8);
            this.rbColor.Name = "rbColor";
            this.rbColor.Size = new System.Drawing.Size(82, 16);
            this.rbColor.TabIndex = 2;
            this.rbColor.TabStop = true;
            this.rbColor.Text = "Color";
            this.toolTip1.SetToolTip(this.rbColor, "Group by Color");
            this.rbColor.CheckedChanged += new System.EventHandler(this.rbColor_CheckedChanged);
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
            this.g4.TabIndex = 1;
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
            this.g7.TabIndex = 2;
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
            this.g10.TabIndex = 3;
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
            this.g1.ColumnInfo = "10,1,0,0,0,85,Columns:";
            this.g1.ContextMenu = this.g1ContextMenu;
            this.g1.Dock = System.Windows.Forms.DockStyle.Top;
            this.g1.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g1.ExtendLastCol = true;
            this.g1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.g1.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g1.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g1.Location = new System.Drawing.Point(0, 40);
            this.g1.Name = "g1";
            this.g1.Rows.DefaultSize = 19;
            this.g1.Rows.Fixed = 0;
            this.g1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g1.Size = new System.Drawing.Size(80, 24);
            this.g1.StyleInfo = resources.GetString("g1.StyleInfo");
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
            this.pnlCorner.Controls.Add(this.gbxSizeVar);
            this.pnlCorner.Controls.Add(this.gbxSeqMatrix);
            this.pnlCorner.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCorner.Location = new System.Drawing.Point(0, 0);
            this.pnlCorner.Name = "pnlCorner";
            this.pnlCorner.Size = new System.Drawing.Size(80, 40);
            this.pnlCorner.TabIndex = 12;
            // 
            // gbxSizeVar
            // 
            this.gbxSizeVar.Controls.Add(this.rbVariable);
            this.gbxSizeVar.Controls.Add(this.rbSize);
            this.gbxSizeVar.Location = new System.Drawing.Point(184, 0);
            this.gbxSizeVar.Name = "gbxSizeVar";
            this.gbxSizeVar.Size = new System.Drawing.Size(124, 28);
            this.gbxSizeVar.TabIndex = 2;
            this.gbxSizeVar.TabStop = false;
            // 
            // rbVariable
            // 
            this.rbVariable.Location = new System.Drawing.Point(60, 8);
            this.rbVariable.Name = "rbVariable";
            this.rbVariable.Size = new System.Drawing.Size(64, 16);
            this.rbVariable.TabIndex = 1;
            this.rbVariable.Text = "Variable";
            this.toolTip1.SetToolTip(this.rbVariable, "View by variable");
            this.rbVariable.CheckedChanged += new System.EventHandler(this.rbVariable_CheckedChanged);
            // 
            // rbSize
            // 
            this.rbSize.Location = new System.Drawing.Point(8, 8);
            this.rbSize.Name = "rbSize";
            this.rbSize.Size = new System.Drawing.Size(50, 16);
            this.rbSize.TabIndex = 0;
            this.rbSize.Text = "Size";
            this.toolTip1.SetToolTip(this.rbSize, "View by size");
            this.rbSize.CheckedChanged += new System.EventHandler(this.rbSize_CheckedChanged);
            // 
            // gbxSeqMatrix
            // 
            this.gbxSeqMatrix.Controls.Add(this.rbSizeMatrix);
            this.gbxSeqMatrix.Controls.Add(this.rbSequential);
            this.gbxSeqMatrix.Location = new System.Drawing.Point(8, 0);
            this.gbxSeqMatrix.Name = "gbxSeqMatrix";
            this.gbxSeqMatrix.Size = new System.Drawing.Size(166, 28);
            this.gbxSeqMatrix.TabIndex = 1;
            this.gbxSeqMatrix.TabStop = false;
            // 
            // rbSizeMatrix
            // 
            this.rbSizeMatrix.Location = new System.Drawing.Point(88, 8);
            this.rbSizeMatrix.Name = "rbSizeMatrix";
            this.rbSizeMatrix.Size = new System.Drawing.Size(78, 16);
            this.rbSizeMatrix.TabIndex = 1;
            this.rbSizeMatrix.Text = "Size Matrix";
            this.toolTip1.SetToolTip(this.rbSizeMatrix, "View sizes as rows");
            this.rbSizeMatrix.CheckedChanged += new System.EventHandler(this.rbSizeMatrix_CheckedChanged);
            // 
            // rbSequential
            // 
            this.rbSequential.Location = new System.Drawing.Point(8, 8);
            this.rbSequential.Name = "rbSequential";
            this.rbSequential.Size = new System.Drawing.Size(80, 16);
            this.rbSequential.TabIndex = 0;
            this.rbSequential.Text = "Sequential ";
            this.toolTip1.SetToolTip(this.rbSequential, "View sizes as columns");
            this.rbSequential.CheckedChanged += new System.EventHandler(this.rbSequential_CheckedChanged);
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
            this.pnlScrollBars.Location = new System.Drawing.Point(1079, 30);
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
            this.vScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar2_Scroll);
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
            this.g5.TabIndex = 1;
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
            this.g2.ColumnInfo = "10,1,0,0,0,85,Columns:";
            this.g2.ContextMenu = this.g1ContextMenu;
            this.g2.Dock = System.Windows.Forms.DockStyle.Top;
            this.g2.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g2.ExtendLastCol = true;
            this.g2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.g2.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g2.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g2.Location = new System.Drawing.Point(0, 0);
            this.g2.Name = "g2";
            this.g2.Rows.DefaultSize = 19;
            this.g2.Rows.Fixed = 0;
            this.g2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g2.Size = new System.Drawing.Size(85, 64);
            this.g2.StyleInfo = resources.GetString("g2.StyleInfo");
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
            this.g8.TabIndex = 2;
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
            this.g11.TabIndex = 3;
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
            // g2g3ContextMenu
            // 
            this.g2g3ContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuColumnChooser23,
            this.mnuSeperator1,
            this.mnuFreezeColumn23});
            // 
            // mnuColumnChooser23
            // 
            this.mnuColumnChooser23.Index = 0;
            this.mnuColumnChooser23.Text = "Column Chooser...";
            this.mnuColumnChooser23.Click += new System.EventHandler(this.mnuColumnChooser23_Click);
            // 
            // mnuSeperator1
            // 
            this.mnuSeperator1.Index = 1;
            this.mnuSeperator1.Text = "-";
            // 
            // mnuFreezeColumn23
            // 
            this.mnuFreezeColumn23.Index = 2;
            this.mnuFreezeColumn23.Text = "Freeze Column";
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
            this.pnlData.Size = new System.Drawing.Size(912, 296);
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
            this.g6.Size = new System.Drawing.Size(912, 100);
            this.g6.TabIndex = 1;
            this.g6.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g6_BeforeScroll);
            this.g6.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g6.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g6.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
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
            this.s3.Size = new System.Drawing.Size(912, 1);
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
            this.g3.ColumnInfo = "10,1,0,0,0,85,Columns:";
            this.g3.ContextMenu = this.g1ContextMenu;
            this.g3.Dock = System.Windows.Forms.DockStyle.Top;
            this.g3.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.g3.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g3.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g3.Location = new System.Drawing.Point(0, 0);
            this.g3.Name = "g3";
            this.g3.Rows.DefaultSize = 19;
            this.g3.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g3.Size = new System.Drawing.Size(912, 64);
            this.g3.StyleInfo = resources.GetString("g3.StyleInfo");
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
            this.s7.Size = new System.Drawing.Size(912, 1);
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
            this.g9.Size = new System.Drawing.Size(912, 72);
            this.g9.TabIndex = 2;
            this.g9.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g9_BeforeScroll);
            this.g9.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g9.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g9.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g9.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g9_OwnerDrawCell);
            this.g9.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g9.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            // 
            // s11
            // 
            this.s11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.s11.Location = new System.Drawing.Point(0, 238);
            this.s11.Name = "s11";
            this.s11.Size = new System.Drawing.Size(912, 1);
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
            this.g12.Size = new System.Drawing.Size(912, 40);
            this.g12.TabIndex = 3;
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
            this.hScrollBar3.Size = new System.Drawing.Size(912, 17);
            this.hScrollBar3.TabIndex = 4;
            this.hScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar3_Scroll);
            // 
            // SizeView
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1096, 326);
            this.Controls.Add(this.pnlData);
            this.Controls.Add(this.VerticalSplitter2);
            this.Controls.Add(this.pnlTotals);
            this.Controls.Add(this.pnlScrollBars);
            this.Controls.Add(this.VerticalSplitter1);
            this.Controls.Add(this.pnlRowHeaders);
            this.Controls.Add(this.pnlTop);
            this.FormLoaded = true;
            this.MinimumSize = new System.Drawing.Size(0, 300);
            this.Name = "SizeView";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.toolTip1.SetToolTip(this, "Apply current changes");
            this.Activated += new System.EventHandler(this.SizeView_Activated);
            this.Deactivate += new System.EventHandler(this.SizeView_Deactivate);
            this.Load += new System.EventHandler(this.SizeView_Load);
            this.Controls.SetChildIndex(this.pnlTop, 0);
            this.Controls.SetChildIndex(this.pnlRowHeaders, 0);
            this.Controls.SetChildIndex(this.VerticalSplitter1, 0);
            this.Controls.SetChildIndex(this.pnlScrollBars, 0);
            this.Controls.SetChildIndex(this.pnlTotals, 0);
            this.Controls.SetChildIndex(this.VerticalSplitter2, 0);
            this.Controls.SetChildIndex(this.pnlData, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.gbxGroupBy.ResumeLayout(false);
            this.pnlRowHeaders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g1)).EndInit();
            this.pnlCorner.ResumeLayout(false);
            this.gbxSizeVar.ResumeLayout(false);
            this.gbxSeqMatrix.ResumeLayout(false);
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
		private static int BIGCHANGE = 4;	
		private static int SMALLCHANGE = 1; 
		private const int BIGHORIZONTALCHANGE = 5;
		private const int ROWPAGESIZE = 150;
		private const int COLPAGESIZE = 30;
      	private int HeaderOrComponentGroups; //how many weeks or how many variables are there. This is used mainly by g3 whenever we need to iterate through the header/components.
		private int _rowsPerStoreGroup = 2; 
		private FromGrid RightClickedFrom; //indicates which grid the user right clicked from.
		private System.Drawing.Bitmap picLock; //this picture will be put in a cell that's locked.
		private System.Drawing.Bitmap picStyle; //this picture is put on the "Theme Properties" button.
		private bool g1HasColsFrozen;
		private bool g2HasColsFrozen;
		private bool g3HasColsFrozen;
		private int LeftMostColBeforeFreeze1; //for g1
		private int LeftMostColBeforeFreeze2; //for g2
		private int LeftMostColBeforeFreeze3; //for g3
		private int UserSetSplitter1Position; //used to determine the width the user set the 1st panel.
		private int UserSetSplitter2Position; //used to determine the width the user set the 2nd panel.
		private ApplicationSessionTransaction _trans;
		private SessionAddressBlock _sab;
        private ExplorerAddressBlock _eab;
		private Hashtable ColHeaders1 = null; //for show/hide columns in g1
		private Hashtable ColHeaders2 = null; //for show/hide columns in g2  
		private Hashtable ColHeaders3 = null; //for show/hide columns in g3  
		private ArrayList _alColHeaders2 = null; //for show/hide columns in g2  
		
		private int _lastVisibleG3Column; 
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

		private int _rowsPerGroup4 = 1;
		private int _rowsPerGroup7 = 1;
		private int _dispRowsPerSet = 1;
		private int _rowsPerGroup10;
		
		private ThemeProperties _frmThemeProperties; //for the theme properties dialog box.
		private Theme _theme;
		private bool _themeChanged = false;

		private SortGridViews frmSortGridViews; //for the sort grid dialog box.
		private System.Data.DataTable _g456GridTable;
		private System.Data.DataTable _g8GridTable;
		private System.Data.DataTable _g9GridTable; 
		private System.Data.DataTable _g11GridTable; 
		private System.Data.DataTable _g12GridTable; 
		
		private bool _loading;
		private bool _gridLoading;
		private bool _changeRowStyle  = true;
        private bool _exporting = false;
		private int _colsPerGroup1 = 1;
		private int _colsPerGroup2 = 1; 
		structSort _structSort;

		AllocationWaferGroup _wafers;
		AllocationWafer _wafer;
		AllocationWaferCell [,] _cells ;
		//private bool _value2Set = false;
		//private bool _value3Set = false;
		
		
		private const string _invalidCellValue = " "; 
		private DataView _g456DataView;
		private object _holdValue;
		private bool _setHeaderTags = true;
		private bool _resetV1Splitter;
		private bool _doResetV1 = false;
		private Hashtable CellRows = null; // cross ref to wafer cell for updating 
		private int _curGroupBy = Include.Undefined;
		private int _curAttribute = Include.Undefined;
		private int _curAttributeSet = Include.Undefined;
		private bool _initalLoad;
		private bool _isScrolling; 
		private MIDRetail.Windows.AllocationWorkspaceExplorer _awExplorer;
		private string _thisTitle;
		private AllocationHeaderProfileList _headerList;
		private int _lastFilterValue;
		private QuickFilter _quickFilter;
		//
// (CSMITH) - BEG MID Track #3219: PO / Master Release Enhancement
		private ArrayList _masterKeyList = new ArrayList();
// (CSMITH) - END MID Track #3219
		private ArrayList _headerArrayList = null;
		private DataTable _conditionDataTable = null;
		private SortedList _headerSortedList = null;
		private DataTable _componentDataTable = null;
		private DataTable _variableDataTable = null;
		private DataTable _colorDataTable = null;
		private DataTable _packDataTable = null;
		//private ArrayList _sizeArrayList = null; // MID Track 3611 Quick Filter not working in Size Review
		private DataTable _sizeDataTable = null;   // MID Track 3611 Quick Filter not working in Size Review
	
		private string _lblHeader = null;
		private string _lblColor = null;
		private string _lblPack = null;
		private string _lblComponent = null;
		private string _lblStore = null;
		private string _lblVariable;
		private string _lblCondition = null;
		private string _lblValue = null;
		private string _lblDimension = null;
		private string _lblDimSize = null;
		private string _lblGrade = null;
		private string _totalVariableName;
		private string _lblNoSecondarySize = null;
		private string _noSizeDimensionLbl;
		// begin MID Track 4708 Size Performance Slow
		private string _lblShipDate = MIDText.GetTextOnly((int)eAllocationWaferVariable.ShipToDay);
		private string _lblNeedDate = MIDText.GetTextOnly((int)eAllocationWaferVariable.NeedDay);
        // end MID Track 4708 Size Performance Slow
		private eQuickFilterType _quickFilterType;
		private int _foundColumn;
		private int _foundRow;
		private QuickFilterData _quickFilterData;
		private int _grid4LastCol;
		private int _grid5LastCol;
		private int _grid6LastCol;
		private FunctionSecurityProfile _allocationReviewSummarySecurity;
		private FunctionSecurityProfile _allocationReviewStyleSecurity;
        private FunctionSecurityProfile _allocationReviewAssortmentSecurity;
		private int _changedCellRow;
		private int _changedCellCol;
		private C1.Win.C1FlexGrid.C1FlexGrid _changedGrid = null;
        private string _includeCurrentSetLabel = null;
        private string _includeAllSetsLabel = null;
        private ProfileList _storeGroupLevelProfileList;
		private string _windowName;
        private bool _gridRebuildInProgress = false; // TT#1009 - RMatelic Unhandled exception while working in size review;  ignor grid sort while screen is rebuilding 
        // Begin TT#456 - RMatelic - Add Views to Size Review
        private FunctionSecurityProfile _userViewSecurity;
        private FunctionSecurityProfile _globalViewSecurity;
        private bool _bindingView = false;
        private bool _changingView = false;
        private bool _buttonChanged = false;
        private int _dimensionLastPosition = 5;
        private int _lastSelectedViewRID;
        private int _viewRID;
        private eLayoutID _layoutID;
        private ArrayList _userRIDList = null;
        private DataTable _dtViews = null;
        private GridViewData _gridViewData;
        private UserGridView _userGridView;
        private ArrayList _builtVariables = null;   
        private ArrayList _curColumnsG1 = new ArrayList();        
        private ArrayList _curColumnsG2 = new ArrayList();
        private ArrayList _curColumnsG3 = new ArrayList();
        private ArrayList _sizeList;
        private Hashtable _sizeAllColHash = new Hashtable();
        private ArrayList _sizeAllColArrayList = new ArrayList(); // TT#3011 - JSmith - Size Review -> change from Size Matrix to Sequential -> Null Ref Error message
        private Hashtable _labelColNameHash;
        private bool _columnAdded = false; 
        //private bool _groupByChanged = false;    
        // End TT#456                
        private bool _applyPending = false;  // Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set
        private bool _applyPendingMsgDisplayed = false;  // Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set
        private bool _fromAssrtReload = false;    // TT#607-MD - RMatelic - Size Review not displaying new sizes added when header is dropped on an assortment placeholder
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

		private void SizeView_Load(object sender, System.EventArgs e)
		{
			//try
			//{
			Cursor.Current = Cursors.WaitCursor;
            _includeAllSetsLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeAllSets);
            _includeCurrentSetLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeCurrentSet);
			FunctionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSize);
			
			// BEGIN MID Track #2551 - security not working
			_allocationReviewSummarySecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSummary);
			_allocationReviewStyleSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewStyle);
            // Begin TT#2 - JSmith - Assortment Security
            //_allocationReviewAssortmentSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewAssortment);
            _allocationReviewAssortmentSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReview);
            // End TT#2
            // END MID Track #2551

			_windowName = MIDText.GetTextOnly(eMIDTextCode.frm_SizeReview);
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
			SetGridRedraws(false);
            // begin TT#1099 Moved this code to SizeView_LoadMethod
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
            // end TT#1099 Moved this code to SizeView_LoadMethod	
			BuildMenu();

            // Begin TT#456 - RMatelic - Add Views to Size Review
            _builtVariables = new ArrayList();
            _userRIDList = new ArrayList();

            _gridViewData = new GridViewData();
            _userGridView = new UserGridView();

            _globalViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsGlobalSizeReview);
            _userViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationViewsUserSizeReview);
            _layoutID = eLayoutID.sizeReviewGrid;

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
            // End TT#456

			GetSelectionCriteria();
			if (this.ExceptionCaught)
			{
				return;
			}
					
			g1HasColsFrozen = false;
			g2HasColsFrozen = false;
			g3HasColsFrozen = false;

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
            //cmbStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cmbStoreAttribute, eMIDControlCode.form_SizeReview);
            //cmbFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cmbFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
            // Begin TT#1118 - JSmith - Undesirable curser position
            //cmbStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cmbStoreAttribute, eMIDControlCode.form_SizeReview, FunctionSecurity, true);
            cmbStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cmbStoreAttribute, eMIDControlCode.form_SizeReview, true, FunctionSecurity, true);
            cmbView.Tag = "IgnoreMouseWheel";   //TT#6-MD-VStuart - Single Store Select
            // End TT#1118
            // Begin TT#1118 - JSmith - Undesirable curser position
            //cmbFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cmbFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, true);
            //BEGIN TT#6-MD-VStuart - Single Store Select
            cmbFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cmbFilter.ComboBox, eMIDControlCode.field_Filter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, true, FunctionSecurity, true);
            //END TT#6-MD-VStuart - Single Store Select
            // End TT#1118
            // End TT#44
            //Begin Track #5858 - Kjohnson
            //BEGIN TT#6-MD-VStuart - Single Store Select
            cmbAttributeSet.Tag = "IgnoreMouseWheel";
            cboAction.Tag = "IgnoreMouseWheel";
            //END TT#6-MD-VStuart - Single Store Select
	
			//this picture will be put in cells to visually indicate to the user
			//that this cell is locked (cannot be "spread" to new data).
			picLock = new Bitmap(GraphicsDirectory + "\\lock.gif");
			picStyle = new Bitmap(GraphicsDirectory + "\\style.gif");

			HeaderOrComponentGroups = _trans.AllocationCriteriaHeaderCount;

			if ( _trans.AllocationGroupBy == Convert.ToInt32(eAllocationSizeViewGroupBy.Header, CultureInfo.CurrentUICulture) )
			{	
				rbHeader.Checked = true;
			}
			else
			{
				rbColor.Checked = true;
			}
			//SetText();	
			_lblStore =  MIDText.GetTextOnly(eMIDTextCode.lbl_StoreSingular);
			_lblVariable = MIDText.GetTextOnly(eMIDTextCode.lbl_Variable);
			_lblHeader = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Header);
			_lblColor =  MIDText.GetTextOnly((int)eMIDTextCode.lbl_Color);
			_lblDimension = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Dimension);

			_lblComponent = MIDText.GetTextOnly((int)eQuickFilterSelectionType.Component);
			_lblDimSize = MIDText.GetTextOnly((int)eMIDTextCode.lbl_DimSize);
			_lblNoSecondarySize = MIDText.GetTextOnly((int) eMIDTextCode.str_NoSecondarySize); 
			// BEGIN MID Track #3125 - Grade Values not displaying
			_lblGrade = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Grade);
			// END MID Track #3125
			// BEGIN MID Track #3942  'None' is now another name for NoSecondarySize 
			_noSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize);
			// END MID Track #3942
			BuildColorList();
			 
			BIGCHANGE = _headerList.Count;
			if ( _colorDataTable.Rows.Count > 0)
				BIGCHANGE *= _colorDataTable.Rows.Count;
			
			//_sab.ClientServerSession.Audit.Add_Msg(
			//	eMIDMessageLevel.Information, 
			//	"Before loading grids", "Size Review");			 
			FormatGrids1to12();
			//_sab.ClientServerSession.Audit.Add_Msg(
			//	eMIDMessageLevel.Information, 
			//	"After loading grids", "Size Review");
            
			_structSort = new structSort();
			SortByDefault();
			//			_sab.ClientServerSession.Audit.Add_Msg(
			//				eMIDMessageLevel.Information, 
			//				"Before formatting grids", "Size Review");
			AssignTag();
            _themeChanged = true;  // TT#4188 - JSmith - Themes -  Row height does not save in conistant format across the different review screens.
			DefineStyles();
			SetStyles();
            _themeChanged = false;  // TT#4188 - JSmith - Themes -  Row height does not save in conistant format across the different review screens.
			SetGridStyles(true,true);
			ApplyPreferences();
   			//			_sab.ClientServerSession.Audit.Add_Msg(
			//				eMIDMessageLevel.Information, 
			//				"After formatting grids", "Size Review");

			UserSetSplitter1Position = VerticalSplitter1.SplitPosition;
			UserSetSplitter2Position = VerticalSplitter2.SplitPosition;
			SetRowSplitPosition8();
			SetRowSplitPosition12();
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
			this.rbColor.Enabled = true; 
			this.rbSequential.Enabled = true; 
			this.rbSizeMatrix.Enabled = true; 
			this.rbVariable.Enabled = true; 
			this.rbSize.Enabled = true;

            // Begin TT#1662-MD - RMatelic - In Style Review screen, columns not aligning and totals not populating >>Set Default View if no view assigned
            if (_viewRID == Include.NoRID)
            {
                _viewRID = Include.DefaultSizeViewRID;
            }
            // End TT#1662-MD 

            // Begin TT#456 - RMatelic - Add Views to Size Review
            if (_viewRID != Include.NoRID)
            {
                // Begin TT#2360 - RMatelic - Received an error going into the size review
                //hScrollBar2.Tag = new ScrollBarValueChanged(ChangeHScrollBar2Value);
                //hScrollBar3.Tag = new ScrollBarValueChanged(ChangeHScrollBar3Value);
                //cmbView.SelectedValue = _viewRID;
                if (ViewIsValid(_viewRID))
                {
                    hScrollBar2.Tag = new ScrollBarValueChanged(ChangeHScrollBar2Value);
                    hScrollBar3.Tag = new ScrollBarValueChanged(ChangeHScrollBar3Value);
                    cmbView.SelectedValue = _viewRID;
                    //this.cmbView_SelectionChangeCommitted(source, new EventArgs()); //TT#306-MD-VStuart-Version 5.0-Size Review not working correctly. // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected
                }
                else
                {
                    _viewRID = Include.NoRID;
                    if (VerticalSplitter1.SplitPosition < gbxSizeVar.Right)
                    {
                        VerticalSplitter1.SplitPosition = gbxSizeVar.Right;
                    }
                }
                // End TT#2360
            }
            // End TT#456  
			
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
			
			hScrollBar2.Tag = new ScrollBarValueChanged(ChangeHScrollBar2Value);
			hScrollBar3.Tag = new ScrollBarValueChanged(ChangeHScrollBar3Value);
			SetHScrollBar1Parameters();
			SetHScrollBar2Parameters();
			SetHScrollBar3Parameters();

			SetGridRedraws(true);
           
			FormLoaded = true;
			SaveCurrentSettings();
			if (_trans.AnalysisOnly)
				this.gbxGroupBy.Enabled = false;

			// Begin TT#1019 - MD - stodd - prohibit allocation actions against GA - 
            if (_trans.ContainsGroupAllocationHeaders())
            {
                EnhancedToolTip.SetToolTipWhenDisabled(cboAction, MIDText.GetTextOnly(eMIDTextCode.msg_ActionProtectedGroupAllocation));
                EnhancedToolTip.SetToolTipWhenDisabled(btnProcess, MIDText.GetTextOnly(eMIDTextCode.msg_ProcessProtectedGroupAllocation));
                cboAction.Enabled = false;
                btnProcess.Enabled = false;
            }
			// End TT#1019 - MD - stodd - prohibit allocation actions against GA - 

			_loading = false;
            
			//}
			//catch (Exception ex)
			//{
			//	HandleException(ex);
			//}
			//finally
			//{
			Cursor.Current = Cursors.Default;
			//}
		}
		#region Get Allocation Selection Criteria 
		// Begin MID Track 4858 - JSmith - Security changes
		private void BuildMenu()
		{
			try
			{
//				PopupMenuTool fileMenuTool;
//				PopupMenuTool editMenuTool;
//				PopupMenuTool toolsMenuTool;
//
//				ButtonTool btExport;
//				ButtonTool btFind;
//				ButtonTool btQuickFilter;
//
//				utmMain.ImageListSmall = MIDGraphics.ImageList;
//				utmMain.ImageListLarge = MIDGraphics.ImageList;
//
//				fileMenuTool = new PopupMenuTool("file_menu");
//				fileMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File);
//				fileMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.True;
//				utmMain.Tools.Add(fileMenuTool);
//
//				btExport = new ButtonTool("btExport");
//				btExport.SharedProps.Caption = "&Export to Excel";
//				btExport.SharedProps.Shortcut = Shortcut.CtrlE;
//				btExport.SharedProps.MergeOrder = 10;
//				utmMain.Tools.Add(btExport);
//
//				fileMenuTool.Tools.Add(btExport);
//
//				editMenuTool = new PopupMenuTool("edit_menu");
//				editMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit);
//				editMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.False;
//				utmMain.Tools.Add(editMenuTool);
//
//				btFind = new ButtonTool("btFind");
//				btFind.SharedProps.Caption = "&Find";
//				btFind.SharedProps.Shortcut = Shortcut.CtrlF;
//				btFind.SharedProps.MergeOrder = 20;
//				btFind.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.FindImage);
//				utmMain.Tools.Add(btFind);
//				editMenuTool.Tools.Add(btFind);
//
//				toolsMenuTool = new PopupMenuTool("tools_menu");
//				toolsMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools);
//				toolsMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.True;
//				utmMain.Tools.Add(toolsMenuTool);
//				
//			 	btQuickFilter = new ButtonTool("btQuickFilter");
//				btQuickFilter.SharedProps.Caption = "&Quick Filter";
//				btQuickFilter.SharedProps.Shortcut = Shortcut.CtrlQ;
//				btQuickFilter.SharedProps.MergeOrder = 13;
//				utmMain.Tools.Add(btQuickFilter);
//				toolsMenuTool.Tools.Add(btQuickFilter);
//			 
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
			this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_SizeReview);
			_thisTitle = this.Text;
			if (_trans.AnalysisOnly)
				this.Text = this.Text + " - " + "Analysis only";
			else if (_headerList.Count > 1)
				this.Text = this.Text + " *";
			else
			{
				AllocationHeaderProfile ahp = (AllocationHeaderProfile)_headerList[0];
				this.Text = this.Text + " - " + ahp.HeaderID;
			}
			if (_trans.DataState == eDataState.ReadOnly)
				//	Format_Title(eDataState.ReadOnly,eMIDTextCode.frm_StyleReview,this.Text);
				this.Text = this.Text +   " Read Only"; 
			this.rbHeader.Text = MIDText.GetTextOnly((int)eAllocationSizeViewGroupBy.Header);
			this.rbColor.Text = MIDText.GetTextOnly((int)eAllocationSizeViewGroupBy.Color);
			this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
			this.btnApply.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Apply);
					
			//			_lblStore =  MIDText.GetTextOnly(eMIDTextCode.lbl_StoreSingular);
			//			_lblVariable = MIDText.GetTextOnly(eMIDTextCode.lbl_Variable);
			//			_lblHeader = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Header);
			//			_lblColor =  MIDText.GetTextOnly((int)eMIDTextCode.lbl_Color);
			//			_lblDimension = "Dimension"; // TODO: mske label soft
			//			_lblComponent = MIDText.GetTextOnly((int)eQuickFilterSelectionType.Component);

            this.toolTip1.SetToolTip(this.cmbView, MIDText.GetTextOnly(eMIDTextCode.lbl_Views));  // TT#456 - RMatelic - Add views to SIze Review
		}	
		private void GetSelectionCriteria()
		{
			try
			{
				if (_trans.AllocationCriteriaExists)
				{
					_trans.UpdateAllocationViewSelectionHeaders();
//					_sab.ClientServerSession.Audit.Add_Msg(
//						eMIDMessageLevel.Information, 
//						"Before getting Allocation wafers", "Size Review");

                    _headerList = (AllocationHeaderProfileList)_trans.GetMasterProfileList(eProfileType.AllocationHeader); // TT#1185 - Verify ENQ before Update
                    // BEGIN MID Track #2551 - security not working
                    // Begin Track #6404 - JSmith - 80302: Units to allocate is calculated when header is Work Up Total Buy
                    //if (FunctionSecurity.AllowUpdate && _trans.HeadersEnqueued)
                    //if (FunctionSecurity.AllowUpdate && _trans.HeadersEnqueued && !_trans.AnalysisOnly) // TT#1185 - Verify ENQ before Update
                    if (FunctionSecurity.AllowUpdate                                                      // TT#1185 - Verify ENQ before Update 
                        && _headerList != null                                                            // TT#1185 - Verify ENQ before Update
                        && _trans.AreHeadersEnqueued(_headerList)                                         // TT#1185 - Verify ENQ before Update  
                        && !_trans.AnalysisOnly)                                                          // TT#1185 - Verify ENQ before Update
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
                    }
                    
                    // Begin TT#456 - RMatelic - Add Views to Size Review
                    SetScreenParmsFromView();
                    // End TT#456  
					
                    _trans.BuildWaferColumns.Clear();
					if (_trans.AnalysisOnly)
					{
						foreach(int variable in Enum.GetValues(typeof( eAllocationSizeNeedAnalysisVariableDefault)))
						{
                            // Begin TT#456 - RMatelic - Add Views to Size Review
                            //_trans.BuildWaferColumnsAdd(0,(eAllocationWaferVariable)variable);
                            //_trans.BuildWaferColumnsAdd(1,(eAllocationWaferVariable)variable);
                            //_trans.BuildWaferColumnsAdd(2,(eAllocationWaferVariable)variable);
                            CheckVariableBuiltArrayList(variable);
                            // End TT#456
						}
                        AddViewColumns();       // TT#456 - RMatelic - AAdd Views to Size Review
					}
					else
					{
						foreach(int variable in Enum.GetValues(typeof( eAllocationSizeViewVariableDefault)))
						{
                            // Begin TT#456 - RMatelic - Add Views to Size Review
                            //_trans.BuildWaferColumnsAdd(0,(eAllocationWaferVariable)variable);
                            //_trans.BuildWaferColumnsAdd(1,(eAllocationWaferVariable)variable);
                            //_trans.BuildWaferColumnsAdd(2,(eAllocationWaferVariable)variable);
                            CheckVariableBuiltArrayList(variable);
                            // End TT#456
						}
                        AddViewColumns();       // TT#456 - RMatelic - AAdd Views to Size Review
					}

					// END MID Track #2551

					_wafers = _trans.AllocationWafers;
					//Begin TT#793 - JScott - Size need successful -> get null reference error when selecting Size REview -> application goes into a loop size review never opens

					if (_wafers[0, 2].Columns.Count == 0)
					{
						throw new NoSizesToDisplayException();
					}

					//End TT#793 - JScott - Size need successful -> get null reference error when selecting Size REview -> application goes into a loop size review never opens
//					_sab.ClientServerSession.Audit.Add_Msg(
//						eMIDMessageLevel.Information, 
//						"After getting Allocation wafers", "Size Review");
					_trans.SizeView = this;
                    //_headerList = (AllocationHeaderProfileList)_trans.GetMasterProfileList(eProfileType.AllocationHeader); // TT#1185 - Verify ENQ before Update
					
					rbSequential.Checked = _trans.AllocationViewIsSequential;
					rbSizeMatrix.Checked = !_trans.AllocationViewIsSequential;
					
					if (_trans.AllocationSecondaryGroupBy == Convert.ToInt32(eAllocationSizeView2ndGroupBy.Size, CultureInfo.CurrentUICulture))
						rbSize.Checked = true;
					else if (_trans.AllocationSecondaryGroupBy ==  Convert.ToInt32(eAllocationSizeView2ndGroupBy.Variable, CultureInfo.CurrentUICulture))
						rbVariable.Checked = true;
					
					_rowsPerStoreGroup = _trans.SizeViewRowVariableCount;
				}
			}
			//Begin TT#793 - JScott - Size need successful -> get null reference error when selecting Size REview -> application goes into a loop size review never opens
			//catch 
			//{
			//    throw;
			//}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			//End TT#793 - JScott - Size need successful -> get null reference error when selecting Size REview -> application goes into a loop size review never opens
		}

        // Begin TT#456 - RMatelic - Add Views to Size Review
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
                            if (groupBy == Convert.ToInt32(eAllocationSizeViewGroupBy.Header, CultureInfo.CurrentUICulture))
                            {
                                _trans.AllocationGroupBy = Convert.ToInt32(eAllocationSizeViewGroupBy.Header, CultureInfo.CurrentUICulture);
                                if (!rbHeader.Checked)
                                {
                                    _buttonChanged = true;
                                }
                                rbHeader.Checked = true;
                            }
                            else if (groupBy == Convert.ToInt32(eAllocationSizeViewGroupBy.Color, CultureInfo.CurrentUICulture))
                            {
                                _trans.AllocationGroupBy = Convert.ToInt32(eAllocationSizeViewGroupBy.Color, CultureInfo.CurrentUICulture);
                                if (!rbColor.Checked)
                                {
                                    _buttonChanged = true;
                                }
                                rbColor.Checked = true;
                            }
                        }
                        if (row["GROUP_BY_SECONDARY"] != DBNull.Value)
                        {
                            int groupBySecondary = Convert.ToInt32(row["GROUP_BY_SECONDARY"], CultureInfo.CurrentUICulture);
                            if (groupBySecondary == Convert.ToInt32(eAllocationSizeView2ndGroupBy.Size, CultureInfo.CurrentUICulture))
                            {
                                _trans.AllocationSecondaryGroupBy = Convert.ToInt32(eAllocationSizeView2ndGroupBy.Size, CultureInfo.CurrentUICulture);
                                if (!rbSize.Checked)
                                {
                                    _buttonChanged = true;
                                }
                                rbSize.Checked = true;
                            }
                            else if (groupBySecondary == Convert.ToInt32(eAllocationSizeView2ndGroupBy.Variable, CultureInfo.CurrentUICulture))
                            {
                                _trans.AllocationSecondaryGroupBy = Convert.ToInt32(eAllocationSizeView2ndGroupBy.Variable, CultureInfo.CurrentUICulture);
                                if (!rbVariable.Checked)
                                {
                                    _buttonChanged = true;
                                }
                                rbVariable.Checked = true;
                            }
                        }
                        if (row["IS_SEQUENTIAL"] != DBNull.Value)
                        {
                            _trans.AllocationViewIsSequential = Include.ConvertCharToBool(Convert.ToChar(row["IS_SEQUENTIAL"], CultureInfo.CurrentUICulture));
                            if (_trans.AllocationViewIsSequential)
                            {
                                if (!rbSequential.Checked)
                                {
                                    _buttonChanged = true;
                                }
                                rbSequential.Checked = true;
                            }
                            else  
                            {
                                if (!rbSizeMatrix.Checked)
                                {
                                    _buttonChanged = true;
                                }
                                rbSizeMatrix.Checked = true;
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
            try
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
            catch
            {
                throw;
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
        // End TT#456 
		#endregion
		#region Format Grids (From Form_Load)

		private void SetGridRedraws(bool aValue)
		{
			// For some reason, the BeforeScroll events were firing on gthe Redrawe after the 
			//grids have already been  scrolled so the _isScrolling switch is set.
			if (aValue)
				_isScrolling = true;
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
			if (aValue)
				_isScrolling = false;
		}

		private void FormatGrids1to12()
		{
			_g8GridTable = null;
			_g9GridTable = null;
			_g11GridTable = null;
			_g12GridTable = null;

			Formatg1Grid();
			Formatg2Grid();
			Formatg3Grid();
			Formatg456Grids();
            Add456DataRows(_wafers, _g456GridTable, out _g456DataView, g4, g5, g6); 
			FormatGrid7_10 (FromGrid.g7);
			//FormatGrid8_12 (FromGrid.g8);
			//FormatGrid8_12 (FromGrid.g9);
			FormatGrid7_10 (FromGrid.g10);
			//FormatGrid8_12 (FromGrid.g11);
			//FormatGrid8_12 (FromGrid.g12);
			FormatGridFromDataTable (FromGrid.g8);
			FormatGridFromDataTable (FromGrid.g9);
			FormatGridFromDataTable (FromGrid.g11);
			FormatGridFromDataTable (FromGrid.g12);

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
			string[,] RowLabels = _wafers[0,0].RowLabels;
			
			int colLabelCount = RowLabels.GetLength(1);
			if (colLabelCount == 0)
			{
				if (rbSizeMatrix.Checked)
					colLabelCount = 5;
				else
					colLabelCount = 4;
			}
            // Begin TT#456 - RMatelic - Add Views to Size Review
            _labelColNameHash = new Hashtable();
            // End TT#456
            g1GridValues = new string[1, (colLabelCount + _wafer.Columns.Count + 1)];
            for (i = 0; i < 1; i++)
            {
                g1GridValues[i, 0] = _lblStore;

                for (k = 1; k <= _wafer.Columns.Count; k++)
                {
                    wafercoordlist = (AllocationWaferCoordinateList)_wafer.Columns[k - 1];
                    wafercoord = (AllocationWaferCoordinate)wafercoordlist[i + 2];
                    g1GridValues[i, k] = ColLabels[i + 2, (k - 1)];
                    ParseColumnHeading(ref g1GridValues[i, k], wafercoord.CoordinateType, wafercoord.CoordinateSubType);
                    // Begin TT#456 - RMatelic - Add Views to Size Review
                    _labelColNameHash.Add(wafercoord.Label, wafercoord.Key);
                    // End TT#456
                }

                if (rbHeader.Checked)
                {
                    g1GridValues[i, k] = _lblHeader;
                    k++;
                    g1GridValues[i, k] = _lblColor;
                }
                else
                {
                    g1GridValues[i, k] = _lblColor;
                    k++;
                    g1GridValues[i, k] = _lblHeader;
                }
                if (rbSizeMatrix.Checked)
                {
                    k++;
                    g1GridValues[i, k] = _lblDimension;
                }
                k++;
                g1GridValues[i, k] = _lblVariable;
                k++;			// add extra column for grid splitter resizing
                g1GridValues[i, k] = (" ");
            }

            // Begin TT#456 - RMatelic - Add Views to Size Review
            _labelColNameHash.Add(_lblStore, (int)eMIDTextCode.lbl_StoreSingular);
            _labelColNameHash.Add(_lblHeader, (int)eMIDTextCode.lbl_Header);
            _labelColNameHash.Add(_lblColor, (int)eMIDTextCode.lbl_Color);
            _labelColNameHash.Add(_lblDimension, (int)eMIDTextCode.lbl_Dimension);
            _labelColNameHash.Add(_lblVariable, (int)eMIDTextCode.lbl_Variable);
            // End TT#456

			g1.Rows.Count = 1;
			g1.Cols.Count = colLabelCount + _wafer.Columns.Count + 1;
			g1.Cols.Fixed = 0;
			g1.Rows.Fixed = 1;		
			g1.AllowDragging = AllowDraggingEnum.None;
			g1.AllowMerging = AllowMergingEnum.RestrictCols;
		
			for (i = 0; i < g1.Cols.Count; i++)
			{	
				//Initialize and assign tags
				TagForColumn colTag = new TagForColumn();
				colTag.cellColumn = i; //Used in Find for Store
				if (i > 0 && i < _wafer.Columns.Count + 1 )	// skip header column and extra column for dragging
				{
					colTag.CubeWaferCoorList = (AllocationWaferCoordinateList)_wafer.Columns[i-1];
				}
				g1.Cols[i].UserData = colTag;
			}
			for (i = 0; i < g1.Rows.Count; i++)
			{
				g1.Rows[i].AllowMerging = true;
				for (j = 0; j < g1.Cols.Count; j++)
				{
					g1.SetData(i, j, g1GridValues[i,j]); //manually set data to the cells
                    // Begin TT#456 - RMatelic - Add Views to Size Review
                    if (_labelColNameHash.ContainsKey(g1GridValues[i,j]))
                    {
                        g1.Cols[j].Name = Convert.ToString(_labelColNameHash[g1GridValues[i, j]], CultureInfo.CurrentUICulture);
                    }
                    // End TT#456
				}
			}
			g1.Rows[0].TextAlign = TextAlignEnum.CenterBottom;
			g1.AutoSizeCols(0, 0, g1.Rows.Count - 1, g1.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.None);
		}

		private void Formatg2Grid()
		{
			int i, j, k; 
			string [,] g2GridValues;
			int rowAdjCount = 0;
			AllocationWaferCoordinateList wafercoordlist;
			AllocationWaferCoordinate wafercoord;	
				
			if (g2.Tag == null)
			{
				g2.Tag = new GridTag(Convert.ToInt32(FromGrid.g2, CultureInfo.CurrentUICulture), null, null);
			}
			_wafer = _wafers[0,1];
			_cells = _wafer.Cells;
			string[,] ColLabels = _wafer.ColumnLabels;
			
			int rowCount = ColLabels.GetLength(0); 	
			int colCount = _wafer.Columns.Count;
			for (i=0; i < rowCount; i++)
			{
				bool rowEmpty = true;
				for (j=0; j<colCount; j++)
				{
					if (ColLabels[i,j].Trim() != null &&
						ColLabels[i,j].Trim() != string.Empty)
					{
						rowEmpty = false;
					}
				}
				if (rowEmpty)
				{
					rowAdjCount += 1;
				}
			}
			g2.Rows.Count = rowCount - rowAdjCount;
			g2.Cols.Count = colCount;
			g2.Cols.Fixed = 0;
			g2.Rows.Fixed = _wafer.Rows.Count;
			g2.AllowDragging = AllowDraggingEnum.None;
			g2.AllowMerging = AllowMergingEnum.RestrictCols;

			g2GridValues = new string[g2.Rows.Count,g2.Cols.Count]; 
			
			for (i = 0; i < g2.Rows.Count; i++)   
			{
				for (k=0; k < g2.Cols.Count; k++)
				{
					wafercoordlist = (AllocationWaferCoordinateList)_wafer.Columns[k];
					wafercoord = (AllocationWaferCoordinate)wafercoordlist[i];
					g2GridValues[i,k] = ColLabels[i+rowAdjCount,k];
					//if (_trans.AllocationGroupBy == Convert.ToInt32(eAllocationStyleViewGroupBy.Header, CultureInfo.CurrentUICulture))
					//if (_trans.SizeViewVariableOrSizeGroupBy == eAllocationSizeView2ndGroupBy.Size)	
					//{
					ParseColumnHeading(ref g2GridValues[i,k],wafercoord.CoordinateType,wafercoord.CoordinateSubType);
					//}
				}
			}
			//			g2.Rows.Count = 3; //jae	
			//			g2.Cols.Count = _wafer.Columns.Count;
			//			g2.Cols.Fixed = 0;
			//			g2.Rows.Fixed = _wafer.Rows.Count;
			//			g2.AllowDragging = AllowDraggingEnum.None;
			//			g2.AllowMerging = AllowMergingEnum.RestrictCols;
			
			for (i = 0; i < g2.Cols.Count; i++)
			{
				TagForColumn colTag = new TagForColumn();
				colTag.CubeWaferCoorList = (AllocationWaferCoordinateList)_wafer.Columns[i];
				colTag.cellColumn = i;
				g2.Cols[i].UserData = colTag;

                // Begin TT#456 - RMatelic - Add Views to Size Review
                AllocationWaferCoordinate variableCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                if (variableCoord != null)
                {
                    g2.Cols[i].Name = variableCoord.Key.ToString();
                }
                // End TT#456 
			}
			
			for (i = 0; i < g2.Rows.Count; i++)
			{
				g2.Rows[i].AllowMerging = true;
				for (j = 0; j < g2.Cols.Count; j++)
				{
					g2.SetData(i, j, g2GridValues[i,j]);
				}
				g2.Rows[i].TextAlign = TextAlignEnum.CenterBottom;
			}
		
			if (!_loading)
			{	
				g2.AutoSizeCols(0, 0, g2.Rows.Count - 1, g2.Cols.Count - 1, 0, AutoSizeFlags.None);
				g2.AutoSizeRows(0, 0, g2.Rows.Count - 1, g2.Cols.Count - 1, 0, AutoSizeFlags.None);
			}
		}
	
		private void Formatg3Grid()
		{
			int i, j, k; 
			string [,] g3GridValues;
			int rowAdjCount = 0;
			AllocationWaferCoordinateList wafercoordlist;
			AllocationWaferCoordinate wafercoord;	
				
			if (g3.Tag == null)
			{
				g3.Tag = new GridTag(Convert.ToInt32(FromGrid.g3, CultureInfo.CurrentUICulture), null, null);
			}
			_wafer = _wafers[0,2];
			_cells = _wafer.Cells;
			string[,] ColLabels = _wafer.ColumnLabels;
			int [] labelIndex; 
			int idx = 0;
			int rowCount = ColLabels.GetLength(0); 	
			int colCount = _wafer.Columns.Count;
			labelIndex = new int[rowCount];
			for (i=0; i < rowCount; i++)
			{
				bool rowEmpty = true;
				for (j=0; j<colCount; j++)
				{
					if (ColLabels[i,j] != "" &&
						ColLabels[i,j] != null &&
						ColLabels[i,j] != string.Empty)
					{
						rowEmpty = false;
					}
				}
				if (rowEmpty)
				{
					rowAdjCount += 1;
				}
				else
				{				
					labelIndex[idx] = i;
					idx++;
				}
			}
			g3.Rows.Count = rowCount - rowAdjCount;
			g3.Cols.Count = colCount;
			g3.Cols.Fixed = 0;
			g3.Rows.Fixed = _wafer.Rows.Count;
			g3.AllowDragging = AllowDraggingEnum.None;
            g3.AllowMerging = AllowMergingEnum.RestrictCols;
       
			g3GridValues = new string[g3.Rows.Count,g3.Cols.Count]; 
			
			for (i = 0; i < g3.Rows.Count; i++)   
			{
				for (k=0; k < g3.Cols.Count; k++)
				{
					wafercoordlist = (AllocationWaferCoordinateList)_wafer.Columns[k];
					wafercoord = (AllocationWaferCoordinate)wafercoordlist[i];
					
					idx = labelIndex[i];
					g3GridValues[i,k] = ColLabels[idx,k];
					
					// begin MID Track #2462 - Size headings should display in primary/secondary
					
					//if (   g3GridValues[i,k].Length > _lblNoSecondarySize.Length
					// 	&& g3GridValues[i,k].Substring(0,_lblNoSecondarySize.Length) == _lblNoSecondarySize)
					
					// The order of the size labels has been reversed so we now need to check the end of the string 
					if (   g3GridValues[i,k].Length > _lblNoSecondarySize.Length
						&& g3GridValues[i,k].EndsWith(_lblNoSecondarySize))
						// end MID Track #2462
					{
						g3GridValues[i,k] = g3GridValues[i,k].Replace("|" + _lblNoSecondarySize, string.Empty);
					}
					// BEGIN MID Track #3942  'None' is now another name for NoSecondarySize 
					else if (   g3GridValues[i,k].Length > _noSizeDimensionLbl.Length
							 && g3GridValues[i,k].EndsWith(_noSizeDimensionLbl))
					{
						g3GridValues[i,k] = g3GridValues[i,k].Replace("|" + _noSizeDimensionLbl, string.Empty);
					}
					// END MID Track #3942
					if (_trans.SizeViewVariableOrSizeGroupBy == eAllocationSizeView2ndGroupBy.Size && i == 1)		
					{
						ParseColumnHeading(ref g3GridValues[i,k],wafercoord.CoordinateType,wafercoord.CoordinateSubType);
					}
				}
			}

            _sizeList = new ArrayList();
			for (i = 0; i < g3.Cols.Count; i++)
			{
				TagForColumn colTag = new TagForColumn();
				colTag.CubeWaferCoorList = (AllocationWaferCoordinateList)_wafer.Columns[i];
				colTag.cellColumn = i;
				g3.Cols[i].UserData = colTag;
                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                g3.Cols[i].Name = string.Empty;
                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order

                // Begin TT#456 - RMatelic - Add Views to Size Review
                AllocationWaferCoordinate variableCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                if (variableCoord != null)
                {
                    g3.Cols[i].Name = variableCoord.Key.ToString();
                }
                AllocationWaferCoordinate sizeCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.PrimarySize);
                if (sizeCoord != null)
                {
                    if (!_sizeList.Contains(sizeCoord.Label))
                    {
                        _sizeList.Add(sizeCoord.Label);
                    }
                }
                // End TT#456 
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
				g3.Rows[i].TextAlign = TextAlignEnum.CenterBottom;
			}
		
			if (!_loading)
			{
                // Begin TT#2885 - JSmith - Adding Variables, wording disappears
                // Begin TT#2905 - JSmith - Size Review-> Qty Allocated column clears when changing view attribute or from matrix to sequential
                //ApplyCurrentColumnsG3();
                SetColumnsG3Visible();
                // End TT#2905 - JSmith - Size Review-> Qty Allocated column clears when changing view attribute or from matrix to sequential
                // End TT#2885 - JSmith - Adding Variables, wording disappears
				g3.AutoSizeRows(0, 0, g3.Rows.Count - 1, g3.Cols.Count - 1, 0, AutoSizeFlags.None);
				g3.AutoSizeCols(0, 0, g3.Rows.Count - 1, g3.Cols.Count - 1, 0, AutoSizeFlags.None);
				
			}
		}
	
		private void Formatg456Grids()  
		{
			int i; 
			AllocationWafer wafer;
			DataColumn dataCol;

            _g456GridTable = MIDEnvironment.CreateDataTable();
			
			//1st column (Store) is a label and not in the wafer
			dataCol = new DataColumn();
			dataCol.ColumnName = _lblStore + " " + _lblStore;
			_g456GridTable.Columns.Add(dataCol);
			
			//Add other columns
			wafer = _wafers[0,0];
			for (i = 0; i < wafer.Columns.Count; i++)
			{
				AddDataColumn(FromGrid.g4, i);
			}
			
			dataCol = new DataColumn();
			if (rbHeader.Checked)
				dataCol.ColumnName = _lblStore + " " + _lblHeader;
			else
				dataCol.ColumnName = _lblStore + " " + _lblColor;
			_g456GridTable.Columns.Add(dataCol);

			dataCol = new DataColumn();
			if (rbHeader.Checked)
				dataCol.ColumnName = _lblStore + " " + _lblColor;
			else
				dataCol.ColumnName = _lblStore + " " + _lblHeader;
			_g456GridTable.Columns.Add(dataCol);

			if (rbSizeMatrix.Checked)
			{
				dataCol = new DataColumn();
				dataCol.ColumnName = _lblStore + " " + _lblDimension;	
				_g456GridTable.Columns.Add(dataCol);
			}
			
			dataCol = new DataColumn();
			dataCol.ColumnName = _lblStore + " " + _lblVariable;	
			_g456GridTable.Columns.Add(dataCol);
	
			//Add extra column at end of Store grid for grid 
			//splitter expanding puposes
			dataCol = new DataColumn();
			dataCol.ColumnName = _lblStore + " Empty";
			_g456GridTable.Columns.Add(dataCol);

			g4.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
			
			//Add grid 5 columns
			wafer = _wafers[0,1];
			for (i = 0; i < wafer.Columns.Count; i++)
			{
				AddDataColumn(FromGrid.g5, i);
			}

			//Add grid 6 columns	
			wafer = _wafers[0,2];
			for (i = 0; i < wafer.Columns.Count; i++)
			{
				AddDataColumn(FromGrid.g6, i);
			}
		}
		private void AddDataColumn(FromGrid aGrid, int aCol)
		{
            // TT#1144 - Unrelated to issue >>> add  try .... catch
            try
            {
                AllocationWafer wafer = null;
                AllocationWaferCoordinateList wafercoordlist;
                AllocationWaferCoordinate wafercoord;
                AllocationWaferVariable varProf;
                string[,] ColLabels;
                DataColumn dataCol;

                dataCol = new DataColumn();

                switch (aGrid)
                {
                    case FromGrid.g4:
                        wafer = _wafers[0, 0];
                        break;
                    case FromGrid.g5:
                        wafer = _wafers[0, 1];
                        break;
                    case FromGrid.g6:
                        wafer = _wafers[0, 2];
                        break;
                }

			wafercoordlist = (AllocationWaferCoordinateList)wafer.Columns[aCol];
			
			ColLabels = wafer.ColumnLabels;
			if (aGrid == FromGrid.g4)
				//dataCol.ColumnName =_lblStore + " " + ColLabels[2,aCol];
				dataCol.ColumnName = _lblStore;
			else if (aGrid == FromGrid.g5)
			{
				dataCol.ColumnName = _lblColor;
				//int labelRowCount = ColLabels.GetLength(0);
				//for (int i=0; i<labelRowCount; i++)
				//{
				//	if (ColLabels[i,aCol] != string.Empty)
				//	{
				//		dataCol.ColumnName += ColLabels[i,aCol] + " ";
				//	}
				//}
			}
			else
			{
				wafercoord =  GetAllocationCoordinate(wafercoordlist, eAllocationCoordinateType.PrimarySize);
				dataCol.ColumnName = wafercoord.Label;
			}
		
			wafercoord =  GetAllocationCoordinate(wafercoordlist, eAllocationCoordinateType.Variable);
			dataCol.ColumnName += " " + wafercoord.Label;			
					
			varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord.Key );
			switch (varProf.Format)
			{
				case eAllocationWaferVariableFormat.String:
				case eAllocationWaferVariableFormat.None:
					dataCol.DataType = System.Type.GetType("System.String");
					break;

				case eAllocationWaferVariableFormat.Number:
					//if (varProf.NumDecimals > 0)
					//{
					dataCol.DataType = System.Type.GetType("System.Decimal");
					dataCol.ExtendedProperties.Add("NumDecimals", varProf.NumDecimals);
					//}
					//else
					//{
					//	dataCol.DataType = System.Type.GetType("System.Int32");
					//	dataCol.ExtendedProperties.Add("IsRuleType",false);
					//}
                    // Begin TT#2225 - RMatelic - VSW Modifcations Enhancement
                    if (varProf.Key == Convert.ToInt32(eAllocationWaferVariable.StoreIMOMaxQuantityAllocated, CultureInfo.CurrentUICulture))
                    {
                        dataCol.ExtendedProperties.Add("SuppressMaxInt", "SuppressMaxInt");
                    }
                    // End TT#2225
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
            catch (Exception ex)
            {
                HandleException(ex);
            }
		}
	
		private AllocationWaferCoordinate GetAllocationCoordinate(AllocationWaferCoordinateList aCoordList, eAllocationCoordinateType aCoordType) 
		{
			AllocationWaferCoordinate wafercoord = null;
			for (int i = 0; i < aCoordList.Count; i++)
			{
				wafercoord = aCoordList[i];
				if (wafercoord.CoordinateType ==  aCoordType)
					break;
			}	
			return wafercoord;
		}

		private AllocationWaferCoordinate GetSecondaryCoordinate(AllocationWaferCoordinateList aCoordList, eAllocationCoordinateType aCoordType) 
		{
			AllocationWaferCoordinate wafercoord = null;
			for (int i = 0; i < aCoordList.Count; i++)
			{
				wafercoord = aCoordList[i];
				if (wafercoord.CoordinateType ==  eAllocationCoordinateType.SecondarySizeTotal ||
					wafercoord.CoordinateType ==  eAllocationCoordinateType.SecondarySize)
				{
					break;
				}
				else
				{
					wafercoord = null;
				}
			}	
			return wafercoord;
		}

        private void Add456DataRows(AllocationWaferGroup aWafers, DataTable aG456GridTable, out DataView aG456DataView,
            C1FlexGrid aGrid4, C1FlexGrid aGrid5, C1FlexGrid aGrid6)
        {
            int i, j, k, n, col;
            //int Grid4LastCol, Grid5LastCol, Grid6LastCol;
            decimal decVal;
            bool waferColFound;
            AllocationWaferCoordinateList wafercoordlist;
            AllocationWaferCoordinate wafercoord;
            AllocationWaferVariable varProf;

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

            string rowLabel, colName;
            int colLabelCount = RowLabels.GetLength(1);
            if (colLabelCount == 0)
            {
                if (rbSizeMatrix.Checked)
                    colLabelCount = 5;
                else
                    colLabelCount = 4;
            }
            _grid4LastCol = colLabelCount + wafer4.Columns.Count;
            _grid5LastCol = _grid4LastCol + wafer5.Columns.Count;
            _grid6LastCol = _grid5LastCol + wafer6.Columns.Count;

            aG456GridTable.Clear();

			ArrayList secondarySizes = new ArrayList();		// MID Track #5309 - grid lines inconsistent			
            for (i = 0; i < wafer4.Rows.Count; i++)
            {
                wafercoordlist = (AllocationWaferCoordinateList)wafer4.Rows[i];
                wafercoord = GetAllocationCoordinate(wafercoordlist, eAllocationCoordinateType.Variable);
                varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord.Key);

                // BEGIN MID Track #5309 - grid lines inconsistent
				AllocationWaferCoordinate secCoord = GetSecondaryCoordinate(wafercoordlist, eAllocationCoordinateType.SecondarySizeTotal);
				if (secCoord != null)
				{
					if (!secondarySizes.Contains(secCoord.Label))
					{
						secondarySizes.Add(secCoord.Label);
					}		
				}
                // END MID Track #5309
				
                dRow = aG456GridTable.NewRow();
                j = 0;
                k = 0;
                n = 0;
                rowLabel = null;
                for (int c = 0; c < colLabelCount; c++)
                {
                    rowLabel += RowLabels[i, c] + " ";
                }

                // BEGIN MID Track #2511 - Sort not working
                //CellRows.Add(rowLabel,i);
                TagForRow rowTag = new TagForRow();
                rowTag.cellRow = i;
                rowTag.CubeWaferCoorList = (AllocationWaferCoordinateList)wafer4.Rows[i];
                wafercoord = GetAllocationCoordinate(rowTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                if (wafercoord.Key == (int)eAllocationWaferVariable.PctToTotal)
                    rowTag.IsDisplayed = false;
                else
                {
                    rowTag.IsDisplayed = true;

                    //	jae			CellRows.Add(RowLabels[i,0], i );
                    foreach (DataColumn dCol in aG456GridTable.Columns)
                    {
                        if (j <= _grid4LastCol)
                        {
                            colName = Convert.ToString(g1.GetData(0, j), CultureInfo.CurrentUICulture);
                            if (colName == _lblStore)
                                dRow[dCol] = RowLabels[i, j];
                            else if (colName.Trim() == string.Empty)
                                dRow[dCol] = _invalidCellValue;
                            else
                            {
                                waferColFound = false;
                                for (col = 0; col < wafer4.Columns.Count; col++)
                                {
                                    wafercoordlist = (AllocationWaferCoordinateList)wafer4.Columns[col];
                                    wafercoord = GetAllocationCoordinate(wafercoordlist, eAllocationCoordinateType.Variable);
                                    if (wafercoord.Label == colName)
                                    {
                                        waferColFound = true;
                                        break;
                                    }
                                }
                                if (waferColFound)
                                {
                                    if (cells4[i, col].CellIsValid)
                                    {
                                        if (dCol.DataType == typeof(System.String))
                                            dRow[dCol] = cells4[i, col].ValueAsString;
                                        //else if (dCol.DataType == typeof(System.Decimal))
                                        //{   // the * 1.00m adds the 2 decimal places (.00) to whole numbers without decimal digits
                                        //	decVal = (Convert.ToDecimal(cells4[i,col].Value, CultureInfo.CurrentUICulture) * 1.00m);
                                        //	nDec = Convert.ToInt32(dCol.ExtendedProperties["NumDecimals"], CultureInfo.CurrentUICulture);
                                        //	dRow[dCol] = Decimal.Round(decVal,nDec);
                                        //}	 
                                        else
                                            dRow[dCol] = cells4[i, col].Value;
                                    }
                                    else
                                        dRow[dCol] = _invalidCellValue;
                                }
                                else if (j < colLabelCount + 1)
                                    dRow[dCol] = RowLabels[i, j - 1];
                                else
                                    dRow[dCol] = _invalidCellValue;
                            }
                            j++;
                        }
                        else if (j <= _grid5LastCol)
                        {
                            if (cells5[i, k].CellIsValid)
                            {
                                if (dCol.DataType == typeof(System.String))
                                    dRow[dCol] = cells5[i, k].ValueAsString;
                                else if (varProf.NumDecimals > 0)
                                {   // the * 1.00m adds the 2 decimal places (.00) to whole numbers without decimal digits
                                    decVal = (Convert.ToDecimal(cells5[i, k].Value, CultureInfo.CurrentUICulture) * 1.00m);
                                    dRow[dCol] = Decimal.Round(decVal, varProf.NumDecimals);
                                }
                                else
                                // Begin TT#2225 - RMatelic - VSW Modifcations Enhancement
                                    //dRow[dCol] = cells5[i, k].Value;
                                {
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
                            // 						else                                   // J.Ellis
                            //							dRow[dCol] = cells5[i,k].Value ;   // J.Ellis
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
                                else if (varProf.NumDecimals > 0)
                                {   // the * 1.00m adds the 2 decimal places (.00) to whole numbers without decimal digits
                                    decVal = (Convert.ToDecimal(cells6[i, n].Value, CultureInfo.CurrentUICulture) * 1.00m);
                                    dRow[dCol] = Decimal.Round(decVal, varProf.NumDecimals);
                                }
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
                            //			  			else                                  // J.Ellis 
                            //							dRow[dCol] = cells6[i,n].Value;   // J.Ellis
                            n++;
                            j++;
                        }
                    }
                    aG456GridTable.Rows.Add(dRow);
                    CellRows.Add(rowLabel, rowTag);
                    // END MID Track #2511
                }
            }

            // BEGIN MID Track #5309 = grid lines inconsistent	 
            if (secondarySizes.Count > 0)
            {
                _rowsPerStoreGroup = secondarySizes.Count;
            }
            else
            {
                _rowsPerStoreGroup = 1;
            }
            // END MID Track #5309

            //Use DataView and bind grids in order to use DataView.Sort. 
            aG456DataView = new DataView(aG456GridTable);
            aGrid4.DataSource = aG456DataView;
            aGrid5.DataSource = aG456DataView;
            aGrid6.DataSource = aG456DataView;

            //All 3 middle grids are bound to the same dataview so all grids  
            //initially have all columns. Remove the columns that do not
            //apply to each grid. 

            aGrid4.Cols.RemoveRange(_grid4LastCol + 1, (wafer5.Columns.Count + wafer6.Columns.Count));

            aGrid5.Cols.RemoveRange(_grid5LastCol + 1, wafer6.Columns.Count);
            aGrid5.Cols.RemoveRange(0, _grid4LastCol + 1);

            aGrid6.Cols.RemoveRange(0, _grid5LastCol + 1);

            aGrid4.Cols.Fixed = 0;
            aGrid4.Rows.Fixed = 0;
            if (aGrid4.Tag == null)
            {
                aGrid4.Tag = new GridTag(Convert.ToInt32(FromGrid.g4, CultureInfo.CurrentUICulture), null, null);
                ((GridTag)aGrid4.Tag).DetailsPerGroup = _rowsPerStoreGroup;
                ((GridTag)aGrid4.Tag).UnitsPerScroll = _rowsPerStoreGroup;
            }
            //			for (i = 0; i < g4.Cols.Count; i++)
            //			{
            //				g4.Cols[i].AllowMerging = true;
            //			}
            if (_headerList.Count > 1)
                aGrid4.AllowMerging = AllowMergingEnum.RestrictAll;

            for (i = 0; i < aGrid4.Rows.Count; i++)
            {
                TagForRow rowTag = new TagForRow();
                rowTag.CubeWaferCoorList = (AllocationWaferCoordinateList)wafer4.Rows[i];
                aGrid4.Rows[i].UserData = rowTag;
            }
            //g4.AutoSizeCols(0, 0, g4.Rows.Count - 1, g4.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);

            aGrid5.Cols.Fixed = 0;
            aGrid5.Rows.Fixed = 0;
            if (aGrid5.Tag == null)
            {
                aGrid5.Tag = new GridTag(Convert.ToInt32(FromGrid.g5, CultureInfo.CurrentUICulture), null, null);
                ((GridTag)aGrid5.Tag).DetailsPerGroup = _rowsPerStoreGroup;
                ((GridTag)aGrid5.Tag).UnitsPerScroll = _rowsPerStoreGroup;
            }

            //for (i = 0; i < g5.Cols.Count; i++)
            //{
            //	g5.Cols[i].TextAlign = TextAlignEnum.GeneralCenter;
            //}
            //g5.AutoSizeCols(0, 0, g5.Rows.Count - 1, g5.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);

            aGrid6.Cols.Fixed = 0;
            aGrid6.Rows.Fixed = 0;
            if (aGrid6.Tag == null)
            {
                aGrid6.Tag = new GridTag(Convert.ToInt32(FromGrid.g6, CultureInfo.CurrentUICulture), null, null);
                ((GridTag)aGrid6.Tag).DetailsPerGroup = _rowsPerStoreGroup;
                ((GridTag)aGrid6.Tag).UnitsPerScroll = _rowsPerStoreGroup;
            }

            //for (i = 0; i < g6.Cols.Count; i++)
            //{
            //	g6.Cols[i].TextAlign = TextAlignEnum.GeneralCenter;
            //}
            //g6.AutoSizeCols(0, 0, g6.Rows.Count - 1, g6.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);

            //			g4.Redraw = true;
            //			g5.Redraw = true;
            //			g6.Redraw = true;
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
			//grid.Cols.Count = wafer.Columns.Count + 2;
			grid.Cols.Count = g4.Cols.Count;
			grid.Cols.Fixed = 0;
			grid.Rows.Fixed = 0;
			if (grid.Tag == null)
			{
				grid.Tag = new GridTag(Convert.ToInt32(aGrid, CultureInfo.CurrentUICulture), null, null);
				((GridTag)grid.Tag).DetailsPerGroup = _rowsPerStoreGroup;
				((GridTag)grid.Tag).UnitsPerScroll = _rowsPerStoreGroup; 
			}
			 
			//			for (j = 1; j <= wafer.Columns.Count; j++)
			//			{
			//				wafercoordlist = (AllocationWaferCoordinateList)wafer.Columns[j - 1];
			//				wafercoord = (AllocationWaferCoordinate)wafercoordlist[_varProfIndex];
			//				varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord.Key );
			//					 
			//				if (varProf.Format == eAllocationWaferVariableFormat.Number
			//					&& varProf.NumDecimals > 0)
			//					grid.Cols[j].DataType = System.Type.GetType("System.Decimal");
			//			}
			int colCount = RowLabels.GetLength(1);
			for (i = 0; i < wafer.Rows.Count; i++)
			{
				TagForRow rowTag = new TagForRow();
				rowTag.CubeWaferCoorList = (AllocationWaferCoordinateList)wafer.Rows[i];  
				grid.Rows[i].UserData = rowTag;
//				string rowLabel = null;
//				for (int c=0; c<colCount; c++)
//				{
//					rowLabel += RowLabels[i,c] + " ";
//				}
				//				grid.SetData(i,0,rowLabel);
				grid.SetData(i, 0, RowLabels[i,0]);
				for (j = 1; j <= wafer.Columns.Count; j++)
				{
					if (_cells[i,j - 1].CellIsValid)
					{
						wafercoordlist = (AllocationWaferCoordinateList)wafer.Columns[j - 1];
						wafercoord =  GetAllocationCoordinate(wafercoordlist, eAllocationCoordinateType.Variable);
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
				for (int c = 1; c<colCount; c++)		
				{
					grid.SetData(i, j, RowLabels[i,c]);
					j++;
				}
				grid.SetData(i, j, _invalidCellValue);
			}
//			for (i = 0; i < grid.Cols.Count; i++)
//			{
//				grid.Cols[i].AllowMerging = true;
//			}
			grid.AllowMerging = AllowMergingEnum.RestrictAll;
			grid.AutoSizeCols(0, 0, grid.Rows.Count - 1, grid.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);
		}
		private void FormatGrid8_12 (FromGrid aGrid)
		{
			int i, j; 
			C1FlexGrid grid = null;	
			AllocationWafer wafer = null;
			AllocationWaferCell [,] cells ;
		
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
			cells = wafer.Cells;
			
			grid.Rows.Count = wafer.Rows.Count;
			grid.Cols.Count = wafer.Columns.Count;
			grid.Cols.Fixed = 0;
			grid.Rows.Fixed = 0;
			if (grid.Tag == null)
			{
				grid.Tag = new GridTag(Convert.ToInt32(aGrid, CultureInfo.CurrentUICulture), null, null);
				((GridTag)grid.Tag).DetailsPerGroup = _rowsPerStoreGroup;
				((GridTag)grid.Tag).UnitsPerScroll = _rowsPerStoreGroup; 
			}
			for (i = 0; i < wafer.Rows.Count; i++)
			{
				for (j = 0; j < wafer.Columns.Count; j++)
				{
					if (cells[i,j].CellIsValid)
					{
						if (cells[i,j].ValueAsString != String.Empty) 
							grid.SetData(i, j,cells[i,j].ValueAsString);
						else
							grid.SetData(i, j,cells[i,j].Value);
					}
					else
						grid.SetData(i, j, _invalidCellValue);
				}
			}
			grid.AutoSizeCols(0, 0, grid.Rows.Count - 1, grid.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);
		}
		private void FormatGridFromDataTable (FromGrid aGrid)
		{
			int i, j; 
			decimal decVal;
			C1FlexGrid grid = null;	
			AllocationWaferCoordinateList wafercoordlist;
			AllocationWaferCoordinate wafercoord;
			AllocationWaferVariable varProf;
			AllocationWafer wafer = null;
			AllocationWaferCell [,] cells ;
			System.Data.DataTable gridTable;
			DataRow dRow;
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
			cells = wafer.Cells;
			
			//grid.Rows.Count = wafer.Rows.Count;
			//grid.Cols.Count = wafer.Columns.Count;
			grid.Cols.Fixed = 0;
			grid.Rows.Fixed = 0;
			if (grid.Tag == null)
			{
				grid.Tag = new GridTag(Convert.ToInt32(aGrid, CultureInfo.CurrentUICulture), null, null);
				((GridTag)grid.Tag).DetailsPerGroup = _rowsPerStoreGroup;
				((GridTag)grid.Tag).UnitsPerScroll = _rowsPerStoreGroup; 
			}
			gridTable = GetTableStructure(aGrid);

			for (i = 0; i < wafer.Rows.Count; i++)
			{
				wafercoordlist = (AllocationWaferCoordinateList)wafer.Rows[i];
				wafercoord =  GetAllocationCoordinate(wafercoordlist, eAllocationCoordinateType.Variable);
				varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord.Key );
				dRow = gridTable.NewRow();
				j = 0;
				foreach ( DataColumn dCol in gridTable.Columns)
				{
					if (cells[i,j].CellIsValid)
					{
						if (cells[i,j].ValueAsString != String.Empty) 
							dRow[dCol] = cells[i,j].ValueAsString;
						else if (varProf.NumDecimals > 0)
						{   // the * 1.00m adds the 2 decimal places (.00) to whole numbers without decimal digits
							decVal = (Convert.ToDecimal(cells[i,j].Value, CultureInfo.CurrentUICulture) * 1.00m);
							dRow[dCol] = Decimal.Round(decVal,varProf.NumDecimals);
						}	
						else
							dRow[dCol] = cells[i,j].Value;
					}
					//					else                                 // J.Ellis 
					//						dRow[dCol] = cells[i,j].Value;   // J.Ellis
					j++;			 
				}
				gridTable.Rows.Add(dRow);
			}
			grid.DataSource = gridTable;
			grid.AutoSizeCols(0, 0, grid.Rows.Count - 1, grid.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);
			switch(aGrid)
			{
				case FromGrid.g8:
					_g8GridTable = gridTable.Copy();
					break;	
				case FromGrid.g9:
					_g9GridTable = gridTable.Copy();
					break;
				case FromGrid.g11:
					_g11GridTable = gridTable.Copy();
					break;
				case FromGrid.g12:
					_g12GridTable = gridTable.Copy();
					break;
			}
		}

		private DataTable GetTableStructure(FromGrid aGrid)
		{
			C1FlexGrid grid = null;	
			AllocationWafer wafer = null;
			System.Data.DataTable gridTable = null;
			DataColumn dataCol;

			switch(aGrid)
			{
				case FromGrid.g8:
					wafer = _wafers[1,1];
					grid = g8;
					if (_g8GridTable != null)
					{
						_g8GridTable.Clear();
						gridTable = _g8GridTable.Clone();
					}	
					break;
				case FromGrid.g9:
					wafer = _wafers[1,2];
					grid = g9;
					if (_g9GridTable != null)
					{
						_g9GridTable.Clear();
						gridTable = _g9GridTable.Clone();
					}	
					break;
				case FromGrid.g11:
					wafer = _wafers[2,1];
					grid = g11;
					if (_g11GridTable != null)
					{
						_g11GridTable.Clear();
						gridTable = _g11GridTable.Clone();
					}	
					break;
				case FromGrid.g12:
					wafer = _wafers[2,2];
					grid = g12;
					if (_g12GridTable != null)
					{
						_g12GridTable.Clear();
						gridTable = _g12GridTable.Clone();
					}	
					break;
			}

			if (gridTable == null)
			{
                gridTable = MIDEnvironment.CreateDataTable(); 
				for (int i = 0; i < wafer.Columns.Count; i++)
				{
					dataCol = AddDataColumn2(aGrid, i);
					gridTable.Columns.Add(dataCol);
				}
			}
			
			return gridTable;
		}
			
		private DataColumn AddDataColumn2(FromGrid aGrid, int aCol)
		{
			AllocationWafer wafer = null;
			AllocationWaferCoordinateList wafercoordlist;
			AllocationWaferCoordinate wafercoord;
			AllocationWaferVariable varProf;
			DataColumn dataCol;
		
			dataCol = new DataColumn();

			switch(aGrid)
			{
				case FromGrid.g8:
					wafer = _wafers[1,1];
					break;
				case FromGrid.g9:
					wafer = _wafers[1,2];
					break;
				case FromGrid.g11:
					wafer = _wafers[2,1];
					break;
				case FromGrid.g12:
					wafer = _wafers[2,2];
					break;
			}

			wafercoordlist = (AllocationWaferCoordinateList)wafer.Columns[aCol];
			if (aGrid == FromGrid.g8 || aGrid == FromGrid.g11)
				dataCol.ColumnName = _lblColor;
			else
			{	
				wafercoord =  GetAllocationCoordinate(wafercoordlist, eAllocationCoordinateType.PrimarySize);
				dataCol.ColumnName = wafercoord.Label;
			}
		
			wafercoord =  GetAllocationCoordinate(wafercoordlist, eAllocationCoordinateType.Variable);
			dataCol.ColumnName += " " + wafercoord.Label;			
					
			varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord.Key );
			switch (varProf.Format)
			{
				case eAllocationWaferVariableFormat.String:
				case eAllocationWaferVariableFormat.None:
					dataCol.DataType = System.Type.GetType("System.String");
					break;

				case eAllocationWaferVariableFormat.Number:
					//if (varProf.NumDecimals > 0)
					//{
					dataCol.DataType = System.Type.GetType("System.Decimal");
					dataCol.ExtendedProperties.Add("NumDecimals", varProf.NumDecimals);
					//}
					//else
					//{
					//	dataCol.DataType = System.Type.GetType("System.Int32");
					//	dataCol.ExtendedProperties.Add("IsRuleType",false);
					//}
					break;
				
				case eAllocationWaferVariableFormat.eRuleType:
					dataCol.DataType = System.Type.GetType("System.UInt32");
					break;

				default:
					dataCol.DataType = System.Type.GetType("System.String");
					break;
			}
			return dataCol;
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
				newstring = ColHeading.Replace(" ", "\r\n");
				ColHeading = newstring;
			}
		}
		

		//		private void RemoveColHeadingWrap(ref string ColHeading, bool RemoveWrap, eAllocationCoordinateType coordType, int coordSubType)
		//		{
		//			if (  (coordType == eAllocationCoordinateType.Component  
		//			       &&  (eComponentType)coordSubType == eComponentType.SpecificColor)
		//                || (coordType == eAllocationCoordinateType.PackName
		//					&& (eComponentType)coordSubType == eComponentType.SpecificPack) )
		//				ColHeading = ColHeading.Replace("\r\n", "");
		//			else if (RemoveWrap)
		//				ColHeading = ColHeading.Replace("\r\n"," ");
		//
		//		}
		
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
				CriteriaChanged();
				UpdateOtherViews();
			}		
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		public override void ISave()
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
					foreach (WindowSaveItem  wsi2 in frm.SaveList)
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
		public override void ISaveAs()
		{
			try
			{
                // Begin TT#456 - RMatelic - Add Views to Size Review
                //ISave();
                SaveView();
                // End TT#456  
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        // Begin TT#456 - RMatelic - Add Views to Size Review
        private void SaveView()
        {
            try
            {
                ViewParms viewParms = new ViewParms();
                viewParms.LayoutID = (int)_layoutID;
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
                viewParms.GlobalViewSecurity = eSecurityFunctions.AllocationViewsGlobalSizeReview;
                viewParms.UserViewSecurity = eSecurityFunctions.AllocationViewsUserSizeReview;

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
                        _gridViewData.GridView_Update(_viewRID, true, _trans.AllocationGroupBy, _trans.AllocationSecondaryGroupBy, _trans.AllocationViewIsSequential, Include.NoRID, false); //TT#1313-MD -jsobek -Header Filters
                    }
                    else
                    {
                        _viewRID = _gridViewData.GridView_Insert(aViewUserRID, (int)_layoutID, aViewName, true, _trans.AllocationGroupBy,
                                   _trans.AllocationSecondaryGroupBy, _trans.AllocationViewIsSequential, Include.NoRID, false); //TT#1313-MD -jsobek -Header Filters
                    }

                    ArrayList flexGrids = new ArrayList();
                    flexGrids.Add(g1);
                    flexGrids.Add(g2);
                    if (rbSize.Checked)
                    {
                        flexGrids.Add(g3);
                    }  
                    else
                    {
                        flexGrids.Add(CopyAndSequenceGridColumns(g3));
                    }
                    
                    _gridViewData.GridViewDetail_Insert(_viewRID, flexGrids);
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
                    BindViewCombo();
                    cmbView.SelectedValue = _viewRID;
                    //this.cmbView_SelectionChangeCommitted(source, new EventArgs()); //TT#306-MD-VStuart-Version 5.0-Size Review not working correctly. // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected
                }
            }
            catch (Exception exc)
            {
                ErrorFound = true;
                string message = exc.ToString();
                throw;
            }
        }

        private C1FlexGrid CopyAndSequenceGridColumns(C1FlexGrid aGrid)
        {
            C1FlexGrid gridCopy = new C1FlexGrid();
            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                aGrid.WriteXml(ms);
                ms.Position = 0;
                gridCopy.ReadXml(ms);
                gridCopy.Name = aGrid.Name;

                Hashtable colHashList = new Hashtable();
                int j = -1; 
                for (int i = 0; i < gridCopy.Cols.Count; i++)
                {
                    C1.Win.C1FlexGrid.Column column = gridCopy.Cols[i];
                    if (!colHashList.ContainsKey(column.Name))
                    {
                        j++;
                        column.Move(j);
                        colHashList.Add(column.Name, column);
                    }
                }
            }
            catch
            {
                throw;
            }
            return gridCopy;
        }
        // End TT#456  

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
            bool headerSavedOK = true;                   // TT#1185 - Verify ENQ before Update
            try
			{
                if (   _trans.DataState == eDataState.Updatable
                    && FunctionSecurity.AllowUpdate                               // TT#1185 - Verify ENQ before Update 
                    && _headerList != null                                        // TT#1185 - Verify ENQ before Update
                    && _trans.AreHeadersEnqueued(_headerList))                     // TT#1185 - Verify ENQ before Update
					//&& _trans.HeadersEnqueued && FunctionSecurity.AllowUpdate ) // TT#1185 - Verify ENQ before Update
				{
					if ( !CheckForChangedCellOK() )
						return false;
                    headerSavedOK = _trans.SaveHeaders(); // TT#1185 - Verify ENQ before Update
                    // begin TT#1185 - Verify ENQ before Update
                    if (!headerSavedOK)
                    {
                        string message = MIDText.GetTextOnly((int)eMIDTextCode.msg_al_HeaderUpdateFailed);
                        MessageBox.Show(message, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    // end TT#1185 - Verify ENQ before Update					UpdateOtherViews();
					UpdateAllocationWorkspace();
				}
				_trans.SaveAllocationDefaults();
                SaveUserGridView();     // TT#456 - RMatelic - Add views to Size Review
				ChangePending = false;
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
            return headerSavedOK;  // TT#1185 - Verify ENQ before Update
            //return true;         // TT#1185 - Verify ENQ before Update
		}

        // Begin TT#456 - RMatelic - Add views to Size Review
        private void SaveUserGridView()
        {
            try
            {
                if (_userGridView != null && _viewRID != 0)
                {
                    _userGridView.UserGridView_Update(SAB.ClientServerSession.UserRID, _layoutID, _viewRID);
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#456

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

        //BEGIN TT#6-MD-VStuart - Single Store Select
        ///// <summary>
        ///// Populate all values of the Store_Group_Levels (Attribute Sets)
        ///// (based on key from Store_Group) of the cmbStoreAttribute
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void cmbStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
        //{
        //    try
        //    { 
        //        if (!_loading)
        //        {
        //            // Begin Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set
        //            if (_applyPending &&
        //                !_applyPendingMsgDisplayed)
        //            {
        //                _applyPendingMsgDisplayed = true;
        //                if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ApplyPendingChanges), _thisTitle,
        //                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //                {
        //                    btnApply_Click(this, new EventArgs());
        //                }
        //                else
        //                {
        //                    this._allocationWaferCellChangeList = new AllocationWaferCellChangeList();
        //                }
        //            }
        //            // End Development TT#8
        //            _resetV1Splitter = true;     
        //            _trans.AllocationStoreAttributeID = Convert.ToInt32(cmbStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
        //            // Begin Track #6404 - JSmith - 80302: Units to allocate is calculated when header is Work Up Total Buy
        //            //ChangePending = true;
        //            // End Track #6404
        //        }
				
        //        if (this.cmbStoreAttribute.SelectedValue != null)
        //            PopulateStoreAttributeSet(this.cmbStoreAttribute.SelectedValue.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}
        //END TT#6-MD-VStuart - Single Store Select	

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

                //var z = new EventArgs();	//TT#301-MD-VStuart-Version 5.0-Controls
				// BEGIN MID Track #3834 - error when more than 1 view open and attribute or set is changed
				object obj = pl.Clone();
				ProfileList attList = (ProfileList)obj;
                //BEGIN TT#6-MD-VStuart - Single Store Select
                cmbAttributeSet.ValueMember = "Key";
                cmbAttributeSet.DisplayMember = "Name";
				//cmbAttributeSet.DataSource = pl.ArrayList;
                cmbAttributeSet.DataSource = attList.ArrayList;
                ////this.cmbStoreAttribute_SelectionChangeCommitted(this, z);	//TT#301-MD-VStuart-Version 5.0-Controls    //TT#306-MD-VStuart-Version 5.0-Style Review Attribute drop down
				// END MID Track #3834
                _storeGroupLevelProfileList = attList;

				if (this.cmbAttributeSet.Items.Count > 0)	
				{
                    if (_loading && _trans.AllocationCriteriaExists)
                    {
                        this.cmbAttributeSet.SelectedValue = _trans.AllocationStoreGroupLevel;
                        ////this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs()); // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected    //TT#306-MD-VStuart-Version 5.0-Style Review Attribute drop down
                    }
                    else
                    {
                        //TT#7 - RBeck - Dynamic dropdowns
                        //this.cmbAttributeSet.SelectedIndex = 0 will fire a SelectionChangeCommitted event but not a SelectionChangeCommitted event.
                        //This must be done to be consistant with prior code.

                        this.cmbAttributeSet.SelectedIndex = 0;
                        //this.cmbAttributeSet_SelectionChangeCommitted(this, z); //TT#7 - RBeck - Dynamic dropdowns    //TT#306-MD-VStuart-Version 5.0-Style Review Attribute drop down
                    }

				}
                //cmbAttributeSet.ComboBox.Width = 141;
                //cmbAttributeSet.ComboBox.Height = 21;
                //cmbAttributeSet.ComboBox.Name = cmbAttributeSet.Name;  //TT#7 - RBeck - Dynamic dropdowns
                this.toolTip1.SetToolTip(this.cmbAttributeSet, "Attribute Sets");
                //AdjustTextWidthComboBox_DropDown(cmbAttributeSet);  // TT#1701 - RMatelic
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
				//ProfileList al = _sab.ApplicationServerSession.GetProfileList(eProfileType.StoreGroupListView);
                ProfileList al = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.MyUserAndGlobal, true); // TT#1517-MD - Store Service Optimization - SRISCH Changed from ALL
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

                //EventArgs z = new EventArgs();  //TT#7 - RBeck - Dynamic dropdowns


                // BEGIN MID Track #4664 - error changing attribute from Velocity
				// Solution: protect attribute drop down to be consistent with StyleView
				if (_trans.VelocityCriteriaExists)
				{
					this.cmbStoreAttribute.SelectedValue = _trans.VelocityStoreGroupRID;
                    ////this.cmbStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); //TT#306-MD-VStuart-Version 5.0-Size Review not working correctly. // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected
					this.cmbStoreAttribute.Enabled = false;
				}
				else // END MID Track #4664 
				{
                    if (_trans.AllocationCriteriaExists)
                    {
                        this.cmbStoreAttribute.SelectedValue = _trans.AllocationStoreAttributeID;
                        ////this.cmbStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); //TT#306-MD-VStuart-Version 5.0-Size Review not working correctly. // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected
                    }
                    else

                        //TT#7 - RBeck - Dynamic dropdowns
                        //this.cmbAttributeSet.SelectedIndex = 0 will fire a SelectionChangeCommitted event but not a SelectionChangeCommitted event.
                        //This must be done to be consistant with prior code.

                        this.cmbStoreAttribute.SelectedIndex = 0;
                        //this.cmbStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); //TT#7 - RBeck - Dynamic dropdowns
				}
                //BEGIN TT#6-MD-VStuart - Single Store Select
                cmbStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
                cmbStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
                //END TT#6-MD-VStuart - Single Store Select
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
                //BEGIN TT#6-MD-VStuart - Single Store Select
				//PopulateCommonCriteria((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BreakoutSizesAsReceivedWithConstraints, this.cboAction.ComboBox, String.Empty); // TT#1391 - Jellis - Balance Size With Constraint Other Options
                PopulateCommonCriteria((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BalanceToVSW, this.cboAction.ComboBox, String.Empty);       // TT#1334-MD - stodd - Balance to VSW Action
                //cboAction.ComboBox.Width = 151;
                //cboAction.ComboBox.Height = 21;
                //cboAction.ComboBox.Name = cboAction.Name;  //TT#7 - RBeck - Dynamic dropdowns
                this.toolTip1.SetToolTip(this.cboAction, "Actions");
                // end TT#794 - New Size Balance for Wet Seal
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
				int codeValue;
				// BEGIN MID Track 3099 Cancel Size Allocation Returns No action Performed when no bulk size
				//DataRow dr;
				//DataTable dt = MIDEnvironment.CreateDataTable();
				//dt = MIDText.GetLables(startVal, endVal);
				//dr = dt.NewRow();
				//dr["TEXT_CODE"] = 0;
				//dr["TEXT_VALUE"] = String.Empty;
				// BEGIN MID Track #2941 - Process list needs to be rearranged  
				//dr["TEXT_ORDER"] = 0;
				// END MID Track #2941
				//dt.Rows.Add(dr);
				// BEGIN MID Track #2470 - add Charge Size Intransit action
				// Remove Delete Header from list
				//for (int i = dt.Rows.Count - 1; i >= 0; i--)
				//{
				//	DataRow dRow = dt.Rows[i];
				//	codeValue = Convert.ToInt32(dRow["TEXT_CODE"], CultureInfo.CurrentUICulture);
				//	if (codeValue == 0)
				//	{
				//	}
				//	else if ( codeValue == Convert.ToInt32(eAllocationActionType.DeleteHeader, CultureInfo.CurrentUICulture))
				//		dt.Rows.Remove(dRow);
				//	else if (!Enum.IsDefined(typeof(eAllocationActionType),(eAllocationActionType)codeValue))	 
				//		dt.Rows.Remove(dRow);
				//}	 
				// END MID Track #2470

				DataRow dr;
				DataTable dt = MIDText.GetLabels(startVal, endVal);
				Hashtable removeEntry = new Hashtable();
				removeEntry.Add(Convert.ToInt32(eAllocationActionType.DeleteHeader), eAllocationActionType.DeleteHeader);
				removeEntry.Add(Convert.ToInt32(eAllocationActionType.BackoutDetailPackAllocation), eAllocationActionType.BackoutDetailPackAllocation);

				for (int i = dt.Rows.Count - 1; i >= 0; i--) 
				{
					dr = dt.Rows[i];
					codeValue = Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture);
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
				// BEGIN MID Track #2941 - Process list needs to be rearranged  
				//dv.Sort = "TEXT_CODE";
				dv.Sort = "TEXT_ORDER";
				// END MID Track #2941
			
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
                //AdjustTextWidthComboBox_DropDown(cmbFilter);  // TT#1401 - AGallagher - Reservation Stores	
                //cmbFilter.ComboBox.Width = 141;
                //cmbFilter.ComboBox.Height = 21;
                //cmbFilter.ComboBox.Name = cmbFilter.Name;  //TT#7 - RBeck - Dynamic dropdowns
                this.toolTip1.SetToolTip(this.cmbFilter, "Filter");
                //END TT#6-MD-VStuart - Single Store Select
            }
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

        //BEGIN TT#6-MD-VStuart - Single Store Select
        //private void cmbFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbFilter.SelectedIndex != -1)
        //        {
        //            if (((FilterNameCombo)cmbFilter.SelectedItem).FilterRID == -1)
        //            {
        //                cmbFilter.SelectedIndex = -1;
        //            }
        //        }
        //        if (cmbFilter.SelectedIndex != -1)
        //            _trans.AllocationFilterID = ((FilterNameCombo)cmbFilter.SelectedItem).FilterRID;
        //        else
        //            _trans.AllocationFilterID = Include.NoRID;

        //        if(!_loading)
        //            ReloadGridData();

        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}
        //END TT#6-MD-VStuart - Single Store Select

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
                    ChangePending = true;
                    ((MIDComboBoxEnh)((ComboBox)sender).Parent).FirePropertyChangeEvent();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }

        // Begin TT#456 - RMatelic - Add Views to Size Review
        private void BindViewCombo()
        {
            try
            {
                if (_userRIDList.Count > 0)
                {
                    _bindingView = true;
                    // Begin TT#1117 - JSmith - Global & User Views w/ the same names do not have indicators
                    //_dtViews = _gridViewData.GridView_Read((int)_layoutID, _userRIDList);
                    _dtViews = _gridViewData.GridView_Read((int)_layoutID, _userRIDList, true);
                    // End TT#1117
                    _dtViews.Rows.Add(new object[] { Include.NoRID, SAB.ClientServerSession.UserRID, _layoutID, string.Empty });
                    _dtViews.PrimaryKey = new DataColumn[] { _dtViews.Columns["VIEW_RID"] };

                    //BEGIN TT#6-MD-VStuart - Single Store Select
                    cmbView.ValueMember = "VIEW_RID";
                    cmbView.DisplayMember = "VIEW_ID";
                    cmbView.DataSource = _dtViews;
                    cmbView.SelectedValue = -1;
                    //this.cmbView_SelectionChangeCommitted(source, new EventArgs()); //TT#306-MD-VStuart-Version 5.0-Size Review not working correctly. // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected

                    _bindingView = false;
                }
                else
                {
                    cmbView.Enabled = false;
                }
                //AdjustTextWidthComboBox_DropDown(cmbView);  // TT#1401 - AGallagher - Reservation Stores

                this.toolTip1.SetToolTip(this.cmbView, "Views");
                //END TT#6-MD-VStuart - Single Store Select
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //BEGIN TT#6-MD-VStuart - Single Store Select
        //private void cmbView_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!_bindingView)
        //        {
        //            _viewRID = Convert.ToInt32(cmbView.SelectedValue, CultureInfo.CurrentUICulture);
        //            if (_viewRID != _lastSelectedViewRID)
        //            {
        //                _lastSelectedViewRID = _viewRID;
        //                _changingView = true;
        //                if (_loading)
        //                {
        //                    ApplyViewToGridLayout(_viewRID);
        //                }
        //                else
        //                {
        //                    AddViewColumns();
        //                    SetScreenParmsFromView();
        //                    if (_columnAdded || _buttonChanged)
        //                    {
        //                        ReloadGridData();
        //                        _columnAdded = false;
        //                        _buttonChanged = false;
        //                    }
        //                    else
        //                    {
        //                        ApplyViewToGridLayout(_viewRID);
        //                    }
        //                }
        //                _changingView = false;
                      
        //                // View not saving when hitting 'X' in upper right corner to exit; not supposed to, but add 'nag' message   
        //                if (_trans.DataState == eDataState.ReadOnly || FunctionSecurity.AllowUpdate == false)
        //                {
        //                    // do not update
        //                }
        //                else if (!_loading)
        //                {
        //                    ChangePending = true;
        //                }
        //            }    
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //        Cursor.Current = Cursors.Default;
        //    }
        //}
        //END TT#6-MD-VStuart - Single Store Select
		
        // Begin TT#2360 - RMatelic - Received an error going into the size review
        private bool ViewIsValid(int aViewRID)
        {
            try
            {
                if (aViewRID == 0 || aViewRID == Include.NoRID)    
                {
                    return true;
                }

                // Begin TT#298-MD - JSmith - Size Analysis Review-> change view to KH-all size columns and get Index out of range error
                //string gridKey, errorMessage;
                //int g1Count = 0, g2Count = 0, g3Count = 0;

                //DataTable dtGridViewDetail = _gridViewData.GridViewDetail_Read(aViewRID);
                //foreach (DataRow row in dtGridViewDetail.Rows)
                //{
                //    gridKey = Convert.ToString(row["BAND_KEY"], CultureInfo.CurrentUICulture);
                //    switch (gridKey)
                //    {
                //        case "g1":
                //            g1Count++;
                //            break;

                //        case "g2":
                //            g2Count++;
                //            break;

                //        case "g3":
                //            g3Count++;
                //            break;
                //    }
                //}

                //// Begin TT#102 MD - JSmith - Index Out of Range exception error
                ////if (g3Count > g3.Cols.Count || (_trans.AnalysisOnly && g2Count > g2.Cols.Count))
                //if (g3Count > g3.Cols.Count || (!_trans.AnalysisOnly && g2Count > g2.Cols.Count))
                //// End TT#102 MD
                //{
                //    errorMessage = "Selected view contains columns that are not on the screen; view selection is ignored";
                //    MessageBox.Show(errorMessage, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return false;
                //}
                // End TT#298-MD - JSmith - Size Analysis Review-> change view to KH-all size columns and get Index out of range error
                return true;
            }
            catch
            {
                throw;
            }
        }
        // End TT#2360  

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
                Hashtable sizeStartColHash = new Hashtable();

                Cursor.Current = Cursors.WaitCursor;
                SetGridRedraws(false);

                // Begin TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                // set all columns not displayed so columns not in view will not be displayed
                for (int i = 0; i < g3.Cols.Count; i++)
                {
                    g3.Cols[i].Visible = false;
                    TagForColumn colTag = (TagForColumn)g3.Cols[i].UserData;
                    colTag.IsDisplayed = false;
                    g3.Cols[i].UserData = colTag;
                }
                // End TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis

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
                        if (gridKey == "g3" && _sizeList.Count > 1)
                        {
                            for (int i = 0; i < appliedGrid1.Cols.Count; i++)
                            {
                                TagForColumn colTag = (TagForColumn)appliedGrid1.Cols[i].UserData;
                                AllocationWaferCoordinate waferCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.PrimarySize);
                                if (!sizeStartColHash.ContainsKey(waferCoord.Label))
                                {
                                    sizeStartColHash.Add(waferCoord.Label, i);   // need first column for each size
                                }
                            }

                            // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                            //foreach (string sizeID in sizeStartColHash.Keys)
                            int sizePosition = 0;   // TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                            foreach (string sizeID in _sizeList)
                            // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                            {
                                for (int col = 0; col < appliedGrid1.Cols.Count; col++)
                                {
                                    C1.Win.C1FlexGrid.Column column = appliedGrid1.Cols[col];
                                    TagForColumn colTag = (TagForColumn)column.UserData;
                                    AllocationWaferCoordinate waferCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.PrimarySize);
                                    if (column.Name == colKey && waferCoord.Label == sizeID)
                                    {
                                        int index = column.Index;
                                        int visPosition;
                                        if (_trans.AllocationSecondaryGroupBy == Convert.ToInt32(eAllocationSizeView2ndGroupBy.Size, CultureInfo.CurrentUICulture))
                                        {
                                            visPosition = visiblePosition + (int)sizeStartColHash[waferCoord.Label];
                                        }
                                        else
                                        {
                                            visPosition = (int)sizeStartColHash[waferCoord.Label] + (visiblePosition * _sizeList.Count);
                                        }
                                        if (visPosition > (appliedGrid1.Cols.Count - 1))
                                        {
                                            visPosition = appliedGrid1.Cols.Count - 1;
                                        }
                                        // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                        // Begin TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                                        int primaryPosition = 0;
                                        int secondaryPosition = 0;
                                        if (_trans.AllocationSecondaryGroupBy == Convert.ToInt32(eAllocationSizeView2ndGroupBy.Size, CultureInfo.CurrentUICulture))
                                        {
                                            primaryPosition = sizePosition;
                                            secondaryPosition = visiblePosition;
                                        }
                                        else
                                        {
                                            primaryPosition = visiblePosition;
                                            secondaryPosition = sizePosition;
                                        }
                                        //gvd = new GridViewDetail(colTag.CubeWaferCoorList, visPosition, isHidden, isGroupByCol, sortDirection, width);
                                        gvd = new GridViewDetail(colTag.CubeWaferCoorList, primaryPosition, isHidden, isGroupByCol, sortDirection, width, secondaryPosition);
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
                                ++sizePosition;   // TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
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

                            // Begin TT#298-MD - JSmith - Size Analysis Review-> change view to KH-all size columns and get Index out of range error
                            if (visiblePosition > appliedGrid1.Cols.Count - 1)
                            {
                                visiblePosition = appliedGrid1.Cols.Count - 1;
                            }
                            // End TT#298-MD - JSmith - Size Analysis Review-> change view to KH-all size columns and get Index out of range error

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
                if (_columnAdded)
                {
                    // Begin TT#1095 - JSmith - Size Review Sorting
                    //MiscPositioning();
                    MiscPositioning(true);
                    // End TT#1095
                }
                if (sortGrid != null)
                {
                    _isSorting = true;
                    this.GridClick(sortGrid);
                    _isSorting = false;
                }
                else
                {
                    mnuSortByDefault_Click(mnuSortByDefault, null);
                }
                CheckVertica1Splitter1();
            }
            catch
            {
                throw;
            }
            SetGridRedraws(true);
            Cursor.Current = Cursors.Default;
           
        }

        // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
        private void PositionColumns(SortedList slGridViewDetail)
        {
            // Begin TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
            // For Debugging
            //int c = 0;
            //foreach (GridViewDetail gvd in slGridViewDetail.Values)
            //{
            //    AllocationWaferCoordinate gvdWafercoord = null;
            //    AllocationWaferCoordinate gvdVariableCoord = null;
            //    foreach (AllocationWaferCoordinate awc in gvd.AllocationWaferCoordinateList)
            //    {
            //        switch (awc.CoordinateType)
            //        {
            //            case eAllocationCoordinateType.PrimarySize:
            //                gvdWafercoord = awc;
            //                break;
            //            case eAllocationCoordinateType.Variable:
            //                gvdVariableCoord = awc;
            //                break;
            //            default:
            //                break;
            //        }
            //    }

            //    Debug.WriteLine("col:" + c.ToString("D3") + "--gvd.VPos:" + gvd.VisiblePosition.ToString("D3") + ";gvd.SPos:" + gvd.SecondaryPosition.ToString("D1") + ";gvd.isHidden:" + gvd.isHidden
            //    + ";gvdW.Lbl:" + gvdWafercoord.Label + ";gvdW.CST:" + gvdWafercoord.CoordinateSubType + ";gvdW.Key:" + gvdWafercoord.Key
            //    + ";gvdV.Lbl:" + gvdVariableCoord.Label + ";gvdV.CST:" + gvdVariableCoord.CoordinateSubType + ";gvdV.Key:" + gvdVariableCoord.Key);

            //    c++;
            //}

            //Debug.WriteLine("Before Move");
            //for (int col = 0; col < g3.Cols.Count; col++)
            //{
            //    C1.Win.C1FlexGrid.Column column = g3.Cols[col];
            //    TagForColumn colTag = (TagForColumn)column.UserData;
            //    AllocationWaferCoordinate wafercoord = null;
            //    AllocationWaferCoordinate variableCoord = null;
            //    foreach (AllocationWaferCoordinate awc in colTag.CubeWaferCoorList)
            //    {
            //        switch (awc.CoordinateType)
            //        {
            //            case eAllocationCoordinateType.PrimarySize:
            //                wafercoord = awc;
            //                break;
            //            case eAllocationCoordinateType.Variable:
            //                variableCoord = awc;
            //                break;
            //            default:
            //                break;
            //        }
            //    }

            //    Debug.WriteLine("col:" + col + "--wafercoord.Label:" + wafercoord.Label + ";wafercoord.CoordinateSubType:" + wafercoord.CoordinateSubType + ";wafercoord.Key:" + wafercoord.Key
            //    + ";variableCoord.Label:" + variableCoord.Label + ";variableCoord.CoordinateSubType:" + variableCoord.CoordinateSubType + ";variableCoord.Key:" + variableCoord.Key + ";Visible:" + column.Visible);
            //}
            // End TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis

            int moveTo = 0;
            
            foreach (GridViewDetail gvd in slGridViewDetail.Values)
            {
                //AllocationWaferCoordinate gvdWafercoord = GetAllocationCoordinate(gvd.AllocationWaferCoordinateList, eAllocationCoordinateType.PrimarySize);
                //AllocationWaferCoordinate gvdVariableCoord = GetAllocationCoordinate(gvd.AllocationWaferCoordinateList, eAllocationCoordinateType.Variable);
                AllocationWaferCoordinate gvdWafercoord = null;
                AllocationWaferCoordinate gvdVariableCoord = null;
                foreach (AllocationWaferCoordinate awc in gvd.AllocationWaferCoordinateList)
                {
                    switch (awc.CoordinateType)
                    {
                        case eAllocationCoordinateType.PrimarySize:
                            gvdWafercoord = awc;
                            break;
                        case eAllocationCoordinateType.Variable:
                            gvdVariableCoord = awc;
                            break;
                        default:
                            break;
                    }
                }
                for (int col = 0; col < g3.Cols.Count; col++)
                {
                    C1.Win.C1FlexGrid.Column column = g3.Cols[col];
                    TagForColumn colTag = (TagForColumn)column.UserData;
                    //AllocationWaferCoordinate wafercoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.PrimarySize);
                    //AllocationWaferCoordinate variableCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                    AllocationWaferCoordinate wafercoord = null;
                    AllocationWaferCoordinate variableCoord = null;
                    foreach (AllocationWaferCoordinate awc in colTag.CubeWaferCoorList)
                    {
                        switch (awc.CoordinateType)
                        {
                            case eAllocationCoordinateType.PrimarySize:
                                wafercoord = awc;
                                break;
                            case eAllocationCoordinateType.Variable:
                                variableCoord = awc;
                                break;
                            default:
                                break;
                        }
                    }
                    //if (gvdWafercoord.Label == wafercoord.Label &&
                    //    gvdVariableCoord.Label == variableCoord.Label)
                    if (gvdWafercoord.Label == wafercoord.Label && gvdWafercoord.CoordinateSubType == wafercoord.CoordinateSubType && gvdWafercoord.Key == wafercoord.Key &&
                        gvdVariableCoord.Label == variableCoord.Label && gvdVariableCoord.CoordinateSubType == variableCoord.CoordinateSubType && gvdVariableCoord.Key == variableCoord.Key)
                    {
                        if (col != gvd.VisiblePosition)
                        {
                            // Begin TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                            // Component One removes column before add.
                            // So offset position if moving to right
                            //if (gvd.VisiblePosition > col)
                            //{
                            //    moveTo = gvd.VisiblePosition - 1;
                            //}
                            //else
                            //{
                            //    moveTo = gvd.VisiblePosition;
                            //}
                            // End TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis

                            g3.Cols[col].Move(moveTo);
                            g6.Cols[col].Move(moveTo);
                            g9.Cols[col].Move(moveTo);
                            g12.Cols[col].Move(moveTo);
                        }
                        // End loop since found column
                        col = g3.Cols.Count;
                        // Begin TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                        ++moveTo;
                        // End TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                    }
                }
            }

            // Begin TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
            // For debugging
            //Debug.WriteLine("After Move");
            //for (int col = 0; col < g3.Cols.Count; col++)
            //{
            //    C1.Win.C1FlexGrid.Column column = g3.Cols[col];
            //    TagForColumn colTag = (TagForColumn)column.UserData;
            //    AllocationWaferCoordinate wafercoord = null;
            //    AllocationWaferCoordinate variableCoord = null;
            //    foreach (AllocationWaferCoordinate awc in colTag.CubeWaferCoorList)
            //    {
            //        switch (awc.CoordinateType)
            //        {
            //            case eAllocationCoordinateType.PrimarySize:
            //                wafercoord = awc;
            //                break;
            //            case eAllocationCoordinateType.Variable:
            //                variableCoord = awc;
            //                break;
            //            default:
            //                break;
            //        }
            //    }

            //    Debug.WriteLine("col:" + col + "--wafercoord.Label:" + wafercoord.Label + ";wafercoord.CoordinateSubType:" + wafercoord.CoordinateSubType + ";wafercoord.Key:" + wafercoord.Key
            //    + ";variableCoord.Label:" + variableCoord.Label + ";variableCoord.CoordinateSubType:" + variableCoord.CoordinateSubType + ";variableCoord.Key:" + variableCoord.Key + ";Visible:" + column.Visible);
            //}
            // End TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
        }
        // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order

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
                    if (Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture).Trim() == string.Empty)
                    {
                        rch.Name = string.Empty;
                        rch.IsDisplayed = true;     // extra column cannot be hidden
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
                    AllocationWaferCoordinate waferCoord;

                    rch.Name = g2.Cols[col].Name;
                    if (rch.Name == string.Empty)
                    {
                        waferCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                        rch.Name = waferCoord.Label;
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
                _sizeAllColHash = new Hashtable();
                _sizeAllColArrayList = new ArrayList();  // TT#3011 - JSmith - Size Review -> change from Size Matrix to Sequential -> Null Ref Error message
                ArrayList sizeColAl;
                for (int col = 0; col < g3.Cols.Count; col++)
                {
                    TagForColumn colTag = (TagForColumn)g3.Cols[col].UserData;
                    RowColHeader rch = new RowColHeader();
                    AllocationWaferCoordinate waferCoord;

                    rch.Name = g3.Cols[col].Name;
                    if (rch.Name == string.Empty)
                    {
                        waferCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                        rch.Name = waferCoord.Label;
                        if (Enum.IsDefined(typeof(eGeneralComponentType), (eGeneralComponentType)waferCoord.CoordinateSubType))
                        {
                            if (!_curColumnsG3.Contains(rch.Name))
                            {
                                _curColumnsG3.Add(rch.Name);
                            }
                        }
                    }
                    else
                    {
                        waferCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                        rch.Name = waferCoord.Label;        // change name from enum to label
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

                    waferCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.PrimarySize);
                    string sizeID = waferCoord.Label;
                    if (!_sizeAllColHash.ContainsKey(sizeID))
                    {
                        sizeColAl = new ArrayList();
                        sizeColAl.Add(rch);
                        _sizeAllColHash.Add(sizeID, sizeColAl);
                        _sizeAllColArrayList.Add(sizeID);  // TT#3011 - JSmith - Size Review -> change from Size Matrix to Sequential -> Null Ref Error message
                    }
                    else
                    {
                        sizeColAl = (ArrayList)_sizeAllColHash[sizeID];
                        sizeColAl.Add(rch);
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

                        if (i < g1.Cols.Count)
                        {
                            g1.Cols[index].Move(i);
                            g4.Cols[index].Move(i);
                            g7.Cols[index].Move(i);
                            g10.Cols[index].Move(i);
                            g1.SetCellImage(0, i, image);
                        }
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
                AllocationWaferCoordinate waferCoord = null;
                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                //int lastUsedPosition = 0;
                SortedList slGridViewDetail = new SortedList(new GridViewDetailOrder());
                ArrayList headerComponentColumns = new ArrayList();
                GridViewDetail gvd;
                AllocationWaferCoordinateList allocationWaferCoordinateList = null;
                eSortDirection sortDirection = eSortDirection.None;
                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order

                Hashtable sizeStartColHash = new Hashtable();
                for (int i = 0; i < g3.Cols.Count; i++)
                {
                    TagForColumn colTag = (TagForColumn)g3.Cols[i].UserData;
                    waferCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.PrimarySize);
                    string sizeID = waferCoord.Label;
                    if (!sizeStartColHash.ContainsKey(sizeID))
                    {
                        sizeStartColHash.Add(sizeID, i);   
                    }
                    if (sizeStartColHash.Keys.Count == _sizeList.Count)
                    {
                        break;
                    }
                }

                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                //foreach (string sizeID in _sizeAllColHash.Keys)
                // Begin TT#3011 - JSmith - Size Review -> change from Size Matrix to Sequential -> Null Ref Error message
                //foreach (string sizeID in _sizeList)
                int sizePosition = 0;  // TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                foreach (string sizeID in _sizeAllColArrayList)
                // ENd TT#3011 - JSmith - Size Review -> change from Size Matrix to Sequential -> Null Ref Error message
                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                {
                    ArrayList al = (ArrayList)_sizeAllColHash[sizeID];
                    // Begin TT#607-MD - RMatelic - Size Review not displaying new sizes added when header is dropped on an assortment placeholder
                    if (al == null)
                    {
                        al = new ArrayList();
                        for (int j = 0; j < _curColumnsG3.Count; j++)
                        {
                            string name = _curColumnsG3[j].ToString();
                            RowColHeader rch = (RowColHeader)ColHeaders3[name];
                             al.Add(rch);
                        }
                        _sizeAllColHash.Add(sizeID, al);
                    }
                    // End TT#607-MD  
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
                            waferCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.PrimarySize);
                            if (waferCoord.Label == sizeID)
                            {
                                waferCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                                if (waferCoord.Label == rch.Name)
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
                            // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                            int visPosition = 0;
                            // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                            g3.Cols[index].Visible = rch.IsDisplayed;
                            g6.Cols[index].Visible = rch.IsDisplayed;
                            g9.Cols[index].Visible = rch.IsDisplayed;
                            g12.Cols[index].Visible = rch.IsDisplayed;
                            g3.Cols[index].Width = rch.RowColumnWidth;
                            g6.Cols[index].Width = rch.RowColumnWidth;
                            g9.Cols[index].Width = rch.RowColumnWidth;
                            g12.Cols[index].Width = rch.RowColumnWidth;

                            if (_sizeList.Count == 1)
                            {
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
                                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                if (_trans.AllocationSecondaryGroupBy == Convert.ToInt32(eAllocationSizeView2ndGroupBy.Size, CultureInfo.CurrentUICulture))
                                {
                                    visPosition = i + (int)sizeStartColHash[sizeID];
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
                                        visPosition = (int)sizeStartColHash[sizeID] + (colPos * _sizeList.Count);
                                        // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
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

                                        //visPosition = (_curColumnsG3.Count * _sizeList.Count) + lastUsedPosition;
                                        visPosition = (_curColumnsG3.Count * _sizeList.Count) + (colPos * _sizeList.Count);
                                        // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                    }
                                    //lastUsedPosition = visPosition;
                                }
                                // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                                //g3.SetCellImage(1, visPosition, image);
                                //g3.SetCellImage(0, visPosition, null);
                                g3.SetCellImage(1, index, image);
                                g3.SetCellImage(0, index, null);
                                // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
                            }

                            // Begin TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
                            int primaryPosition = 0;
                            int secondaryPosition = 0;
                            if (_trans.AllocationSecondaryGroupBy == Convert.ToInt32(eAllocationSizeView2ndGroupBy.Size, CultureInfo.CurrentUICulture))
                            {
                                primaryPosition = sizePosition;
                                if (_curColumnsG3.Contains(rch.Name))   // column is in all headers
                                {
                                    secondaryPosition = _curColumnsG3.IndexOf(rch.Name);
                                }
                                else
                                {
                                    secondaryPosition = headerComponentColumns.IndexOf(rch.Name);
                                }
                            }
                            else
                            {
                                if (_curColumnsG3.Contains(rch.Name))   // column is in all headers
                                {
                                    primaryPosition = _curColumnsG3.IndexOf(rch.Name);
                                }
                                else
                                {
                                    primaryPosition = headerComponentColumns.IndexOf(rch.Name);
                                }
                                secondaryPosition = sizePosition;
                            }
                            //gvd = new GridViewDetail(allocationWaferCoordinateList, visPosition, !rch.IsDisplayed, false, sortDirection, rch.RowColumnWidth);
                            gvd = new GridViewDetail(allocationWaferCoordinateList, primaryPosition, !rch.IsDisplayed, false, sortDirection, rch.RowColumnWidth, secondaryPosition);
                            // End TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis

                            slGridViewDetail.Add(gvd, gvd);
                            sortDirection = eSortDirection.None;
                        }
                    }
                    ++sizePosition;  // TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
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
        // End TT#456

        // Begin TT#2905 - JSmith - Size Review-> Qty Allocated column clears when changing view attribute or from matrix to sequential
        private void SetColumnsG3Visible()
        {
            try
            {
                AllocationWaferCoordinate waferCoord = null;

                foreach (string sizeID in _sizeAllColHash.Keys)
                {
                    ArrayList al = (ArrayList)_sizeAllColHash[sizeID];
                    for (int i = 0; i < al.Count; i++)
                    {
                        RowColHeader rch = (RowColHeader)al[i];
                        int index = -1;

                        for (int col = 0; col < g3.Cols.Count; col++)
                        {
                            C1.Win.C1FlexGrid.Column column = g3.Cols[col];
                            TagForColumn colTag = (TagForColumn)column.UserData;
                            waferCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.PrimarySize);
                            if (waferCoord.Label == sizeID)
                            {
                                waferCoord = GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
                                if (waferCoord.Label == rch.Name)
                                {
                                    index = column.Index;
                                    break;
                                }
                            }
                        }
                        if (index > -1)
                        {
                            g3.Cols[index].Visible = rch.IsDisplayed;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // End TT#2905 - JSmith - Size Review-> Qty Allocated column clears when changing view attribute or from matrix to sequential
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
			string colName;
						
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
				colTag.IsLockable = false;
				
				if (_setHeaderTags)
				{
					colName = Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture);
					if (colName == _lblStore)
						colTag.IsDisplayed = true;
					else if (colName == _lblHeader)
					{
						if (_trans.AnalysisOnly)
							colTag.IsDisplayed = false;
						else
							colTag.IsDisplayed = true;
					}
					else if (colName == _lblVariable)
					{
						colTag.IsDisplayed = false;
					}
					else if (colTag.CubeWaferCoorList == null)
						colTag.IsDisplayed = true;
					else // if (col <= _wafer.Columns.Count)
					{   // display default columns		
						wafercoord = colTag.CubeWaferCoorList[2];
						// BEGIN MID Track 3077 Size Analysis Default Variable s/b OH + IT
						if (_trans.AnalysisOnly)
						{
							eAllocationSizeNeedAnalysisVariableDefault snva = (eAllocationSizeNeedAnalysisVariableDefault)wafercoord.Key;
							if (Enum.IsDefined(typeof(eAllocationSizeNeedAnalysisVariableDefault), snva))
							{
								colTag.IsDisplayed = true;
							}
							else
							{
								colTag.IsDisplayed = false;
							}
						}
						else
						{
							eAllocationSizeViewVariableDefault asd = (eAllocationSizeViewVariableDefault)wafercoord.Key;
							if (Enum.IsDefined(typeof(eAllocationSizeViewVariableDefault),asd))
								colTag.IsDisplayed = true;
							else
								colTag.IsDisplayed = false;
						}
						// END MID Track 3077
					}
					//else
					//	colTag.IsDisplayed = true;

					colTag.Sort = SortEnum.none;
				}
				else if (ColHeaders1 != null)	
				{
					if (   Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture) == _lblStore
						|| Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture).Trim() == string.Empty)
					{
						colTag.IsDisplayed = true;
						colTag.Sort = SortEnum.none;
					}
					else
					{
						if (colTag.CubeWaferCoorList != null)
						{
							wafercoord = colTag.CubeWaferCoorList[2];
							colName = wafercoord.Label;
						}
						else
						{
							colName = Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture);
						}	
						rch = (RowColHeader)ColHeaders1[colName];
                        // Begin TT#456 - RMatelic - Add Views to Size Review
                        //colTag.IsDisplayed = rch.IsDisplayed;
                        if (rch != null)
                        {
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
					
					if (_cells.LongLength > 0)
						colTag.IsLockable = _cells[0,col].CellCanBeChanged;
					else
						colTag.IsLockable = false;

					wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
					
					if (_setHeaderTags)
					{
						// display default columns	
						// BEGIN MID Track 3077 Size Analysis Default Variable s/b OH + IT
						if (_trans.AnalysisOnly)
						{
							eAllocationSizeNeedAnalysisVariableDefault snva = (eAllocationSizeNeedAnalysisVariableDefault)wafercoord.Key;
							if (Enum.IsDefined(typeof(eAllocationSizeNeedAnalysisVariableDefault), snva))
							{
								colTag.IsDisplayed = true;
							}
							else
							{
								colTag.IsDisplayed = false;
							}
						}
						else
						{
							eAllocationSizeViewVariableDefault asd = (eAllocationSizeViewVariableDefault)wafercoord.Key;
                            // Begin TT#3109 - JSmith - Size Review Screen View not updating when changed
                            //if (Enum.IsDefined(typeof(eAllocationSizeViewVariableDefault),asd))
                            if (Enum.IsDefined(typeof(eAllocationSizeViewVariableDefault), asd) &&
                                _viewRID == Include.NoRID)
                            // End TT#3109 - JSmith - Size Review Screen View not updating when changed
								colTag.IsDisplayed = true;
							else
								colTag.IsDisplayed = false;
						}
						// END MID Track 3077 Size Analysis Default Variable s/b OH + IT
						colTag.Sort = SortEnum.none;
					}
					else if (WhichGrid == FromGrid.g2)
					{
						if (ColHeaders2 != null)
						{
							rch = (RowColHeader)ColHeaders2[wafercoord.Label];
							if ( rch != null)
								colTag.IsDisplayed = rch.IsDisplayed;
						}
						else
						{
							colTag.IsDisplayed = true;
							colTag.Sort = SortEnum.none;
						}
					}
					else
					{
						if (ColHeaders3 != null)
						{
							rch = (RowColHeader)ColHeaders3[wafercoord.Label];
							if ( rch != null)
								colTag.IsDisplayed = rch.IsDisplayed;
						}
						else
						{
							colTag.IsDisplayed = true;
							colTag.Sort = SortEnum.none;
						}
					}
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
			C1FlexGrid rowGrid;
			int row, col;
			CellRange cellRange;
			TagForGridData dataTag;
			TagForColumn colTag;
			int cellRow, cellCol, storeCol = 0, headerCol = 0;
			int colorCol = 0, varCol = 0, dimCol = 0;
			string StoreID, colLabel;
			AllocationWafer wafer = null;
			AllocationWaferCell [,] cells ; 
			AllocationWaferCoordinate waferCoord;
		
			try
			{
				switch (WhichGrid)
				{
					case FromGrid.g4:
                        grid = g4;
						break;
					case FromGrid.g5:
						grid = g5;
						wafer = _wafers[0,1];
						break;
					case FromGrid.g6:
						grid = g6;
						wafer = _wafers[0,2];
						break;
					case FromGrid.g7:
						grid = g7;
						break;
					case FromGrid.g8:
						grid = g8;
						wafer = _wafers[1,1];
						break;
					case FromGrid.g9:
						grid = g9;
						wafer = _wafers[1,2];
						break;
					case FromGrid.g10:
						grid = g10;
						break;
					case FromGrid.g11:
						grid = g11;
						wafer = _wafers[2,1];
						break;
					case FromGrid.g12:
						grid = g12;
						wafer = _wafers[2,2];
						break;
				}
				
                if (grid == g4 || grid == g7 || grid == g10) 
				{	// BEGIN MID Track #2511 - Sort not working
					if  (grid == g7 || grid == g10) 
					// END MID Track #2511
					{
						for (row = 0; row < grid.Rows.Count; row++)
						{
							TagForRow rowTag = (TagForRow)grid.Rows[row].UserData;
							rowTag.IsLockable = false;
							waferCoord =  GetAllocationCoordinate(rowTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
							if (waferCoord.Key == (int)eAllocationWaferVariable.PctToTotal)
								rowTag.IsDisplayed = false;
							else
								rowTag.IsDisplayed = true;
							grid.Rows[row].UserData = rowTag;
						}
					}
					return;
				}
				cells = wafer.Cells;
				colGrid = GetColumnGrid(grid);
				rowGrid = GetRowGrid(grid);

				if (   (FromGrid)WhichGrid == FromGrid.g5 
					|| (FromGrid)WhichGrid == FromGrid.g6) 
				{
					for (int i = 0; i < g1.Cols.Count; i++)
					{
						colLabel = Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture);
						if (colLabel == _lblStore)
							storeCol = i;
						else if (colLabel == _lblHeader)
							headerCol = i;
						else if (colLabel == _lblColor)
							colorCol = i;
						else if (colLabel == _lblVariable)
							varCol = i;
						else if (colLabel == _lblDimension)
							dimCol = i;
					}
				}

				for (row = 0; row < grid.Rows.Count; row++)
				{
					if (   (FromGrid)WhichGrid == FromGrid.g5 
						|| (FromGrid)WhichGrid == FromGrid.g6) 
					{
						StoreID = Convert.ToString(rowGrid[row, storeCol], CultureInfo.CurrentUICulture) + " " ;
						if (rbHeader.Checked)
						{	
							StoreID += Convert.ToString(rowGrid[row, headerCol], CultureInfo.CurrentUICulture)+  " " ;
							StoreID += Convert.ToString(rowGrid[row, colorCol], CultureInfo.CurrentUICulture) + " " ;
						}
						else
						{
							StoreID += Convert.ToString(rowGrid[row, colorCol], CultureInfo.CurrentUICulture) + " " ;
							StoreID += Convert.ToString(rowGrid[row, headerCol], CultureInfo.CurrentUICulture)+  " " ;
						}
						if (rbSizeMatrix.Checked)
							StoreID += Convert.ToString(rowGrid[row, dimCol], CultureInfo.CurrentUICulture) + " " ;

						StoreID += Convert.ToString(rowGrid[row, varCol], CultureInfo.CurrentUICulture) + " " ;
						
						// BEGIN MID Track #2511 - Sort not working
						//cellRow = (Int32)CellRows[StoreID];
						TagForRow rowTag = (TagForRow)CellRows[StoreID];
						cellRow = rowTag.cellRow;
						// END MID Track #2511
					}
					else
					{
						cellRow = row;
					}

					for (col = 0; col < grid.Cols.Count; col++)
					{
						colTag = (TagForColumn)colGrid.Cols[col].UserData;
						cellCol = colTag.cellColumn;
						dataTag = new TagForGridData();
						dataTag.IsLocked = false; 
						dataTag.IsEditable = cells[cellRow,cellCol].CellCanBeChanged;
						// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
						dataTag.IsOutOfBalance = cells[cellRow,cellCol].StoreAllocationOutOfBalance;
						// END MID Track #1511
						cellRange = new CellRange();
						cellRange = grid.GetCellRange(row, col);
						cellRange.UserData = dataTag;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>
		/// Used to show/hide columns/rows and apply other user-saved preferences
		/// </summary>
		private void ApplyPreferences()
		{
			string colName;
			bool IsDisplayed;
			TagForColumn colTag;
			RowColHeader rch;
			AllocationWaferCoordinate wafercoord;	

			// BEGIN MID Track #3161 = selected visible columns not remaining selected
			//	ColHeaders1 = new Hashtable();
			// Since window opens as Matrix, all columns are present
            // Begin TT#456 - Add views to Size Review  ; remove if...
            //if (_loading)	
            //{
            // End TT#456  
				ColHeaders1 = new Hashtable();
				for (int i = 0; i < g1.Cols.Count; i++)
				{
					colTag = (TagForColumn)g1.Cols[i].UserData;
					IsDisplayed = colTag.IsDisplayed;
					if (colTag.CubeWaferCoorList != null)
					{
						wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
						colName = wafercoord.Label;
					}
					else
						colName = Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture);

                    // begin TT#980 Item already in dictionary
                    //ColHeaders1.Add(colName,new RowColHeader(colName, IsDisplayed));
                    rch = (RowColHeader)ColHeaders1[colName];
                    if (rch == null)
                        ColHeaders1.Add(colName, new RowColHeader(colName, IsDisplayed));
                    // end TT#980 Item already in dictionary
				}
            //} //  TT#456 - Add views to Size Review
			// END MID Track #3161 

			ColHeaders2 = new Hashtable();
			ColHeaders3 = new Hashtable();

			// BEGIN MID Track #3161 = selected visible columns not remaining selected		
			//for (int i = 0; i < g1.Cols.Count; i++)
			//{
			//	colTag = (TagForColumn)g1.Cols[i].UserData;
			//	IsDisplayed = colTag.IsDisplayed;
			//	if (colTag.CubeWaferCoorList != null)
			//	{
			//		wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
			//		colName = wafercoord.Label;
			//	}
			//	else
			//		colName = Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture);
			//	
			//	ColHeaders1.Add(colName,new RowColHeader(colName, IsDisplayed));
			//}
			// END MID Track #3161 

			//show/hide columns
			RightClickedFrom = FromGrid.g1; //to re-use code, pretend the user right-clicked on g4 because the following code needs to know where the "context menu" was clicked.
            ShowHideColHeaders(RightClickedFrom, ColHeaders1, false, g4); 

			for (int i = 0; i < g2.Cols.Count; i++)
			{
				colTag = (TagForColumn)g2.Cols[i].UserData;
				IsDisplayed = colTag.IsDisplayed;
				wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
				colName = wafercoord.Label;
				// begin TT#980 Item already in dictionary
                //ColHeaders2.Add(colName,new RowColHeader(colName, IsDisplayed));
                rch = (RowColHeader)ColHeaders2[colName];
                if (rch == null)
                    ColHeaders2.Add(colName, new RowColHeader(colName, IsDisplayed));
                // end TT#980 Item already in dictionary
			}
			//show/hide columns
			RightClickedFrom = FromGrid.g2; //to re-use code, pretend the user right-clicked on g2 because the following code needs to know where the "context menu" was clicked.
            ShowHideColHeaders(RightClickedFrom, ColHeaders2, false, g5);
			
			for (int i = 0; i < g3.Cols.Count; i++)
			{
				colTag = (TagForColumn)g3.Cols[i].UserData;
				IsDisplayed = colTag.IsDisplayed;
				wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
				colName = wafercoord.Label;
				rch = (RowColHeader)ColHeaders3[colName];
				if (rch == null)
					ColHeaders3.Add(colName,new RowColHeader(colName, IsDisplayed));
			}
			//show/hide columns
			RightClickedFrom = FromGrid.g3; //to re-use code, pretend the user right-clicked on g3 because the following code needs to know where the "context menu" was clicked.
            ShowHideColHeaders(RightClickedFrom, ColHeaders3, false, g6);

			ShowHideRows();
		}

        // Begin TT#1095 - JSmith - Size Review Sorting
        //private void MiscPositioning()
        private void MiscPositioning(bool aResizeColumns)
        // End TT#1095
		{
			ResizeRows();
            // Begin TT#1095 - JSmith - Size Review Sorting
            //ResizeColumns();
            if (aResizeColumns)
            {
                ResizeColumns();
            }
            // ENd TT#1095

			//Miscellaneous - setup positions and sizes for splitters and scroll bars.
			
			if (_loading)
			{
                SetV1SplitPosition();
                SetV2SplitPosition();
			}
			else if (_resetV1Splitter)
			{
				SetV1SplitPosition();
				_resetV1Splitter = false;
			}
			 
			//g1.Height = g1.Rows[0].HeightDisplay + s4.Height + 3; 

			int gridHeight = g1.Height;
			g1.Height = g1.Rows[0].HeightDisplay + s4.Height; 
			pnlCorner.Height = g2.Rows[0].HeightDisplay;
			// The following is a code around for Anna's grid getting 'stuck' when single 
			// column sort causes grid to change height because of sort arrow placement;
			// If the mouse isn't moved when sort column is clicked the MouseUp event
			// isn't firing so this forces it.
			if (gridHeight != g1.Height && !_loading) 
			{                                          
				GridMouseUp(g1, null);                 
			}     

			SetRowSplitPosition4();
			if (!FormIsClosing)
			{
				SetVScrollBar2Parameters();
				SetVScrollBar3Parameters();
				SetVScrollBar4Parameters();
			
				

				if (!_loading)
				{
					SetHScrollBar1Parameters();
					SetHScrollBar2Parameters();
					SetHScrollBar3Parameters();
					SetActionListEnabled();
					if (_trans.AnalysisOnly)
					{
						btnApply.Enabled = false;
					}	
					else
						btnApply.Enabled = (g4.Rows.Count > 0 && (_trans.DataState == eDataState.Updatable));
				}
			}
				 
		}
		private void SetActionListEnabled()
		{
			bool enableActions = true;
			if (_trans.AnalysisOnly || FunctionSecurity.AllowUpdate == false)
				enableActions = false;
                // begin TT#1185 - Verify ENQ before Update
            else if (g4 != null
                     && g4.Rows.Count == 0)
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
                //    if (g4.Rows.Count == 0)
                //        enableActions = false;
                //    else if (!_trans.AreSelectedHeadersEnqueued()) // TT#1185 - Verifye ENQ before Update
                //        //else if (!_trans.HeadersEnqueued)  // TT#1185 - Verify ENQ before Update

                //        enableActions = false;
                //    //enableActions = (g4.Rows.Count > 0) && (_trans.DataState == eDataState.Updatable));
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
		private void ResizeColumns()
		{
			int i, MinColWidth = 0;
			g1.AutoSizeCols(); g2.AutoSizeCols();
            // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary) - move commented code down 
            //g3.AutoSizeCols(); 
            // End TT#234  
			for (i = 0; i < g1.Cols.Count; i++)
			{	
				g4.Cols[i].AllowMerging = false;
				g7.Cols[i].AllowMerging = false;
				g10.Cols[i].AllowMerging = false;
			}
			// Begin MID Track 4708 Size Performance: Removed AutoSizeCols
            //g4.AutoSizeCols();
			//g5.AutoSizeCols();
			//g6.AutoSizeCols();
			//g7.AutoSizeCols(); g8.AutoSizeCols();
			//g9.AutoSizeCols(); g10.AutoSizeCols();
			//g11.AutoSizeCols(); g12.AutoSizeCols();
			//g10.AutoSizeCols();
			//g11.AutoSizeCols();
			//g12.AutoSizeCols();
			g7.AutoSizeCols();
			g10.AutoSizeCols();
			g11.AutoSizeCols();
			g12.AutoSizeCols();
			// end MID Track 4708 Size Performance: Removed AutoSizeCols

			//Resize Columns on all grids. Line-up data based on the widest column.
			//And while we're in the loop, set the ImageAlign property of each column.
				 
			for (i = 0; i < g1.Cols.Count; i++)
			{	
				MinColWidth = g1.Cols[i].Width;
				//MinColWidth = Math.Max(MinColWidth, g4.Cols[i].Width); 
				MinColWidth = Math.Max(MinColWidth, g7.Cols[i].Width);
				MinColWidth = Math.Max(MinColWidth, g10.Cols[i].Width);
				// begin MID Track 4708 Size performance slow
		    	if (Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture) == _lblStore)
				{
					MinColWidth = (int)(1.5 * MinColWidth); // make store column wider than normal
				}
				MinColWidth = MinColWidth + 5;
				// end MID track 4708 Size Performance slow
				g1.Cols[i].Width = MinColWidth;
				g4.Cols[i].Width = MinColWidth;
				g7.Cols[i].Width = MinColWidth;
				g10.Cols[i].Width = MinColWidth;
				
				// BEGIN MID Track #3125 - Grade Values not displaying
				//g4.Cols[i].AllowMerging = true;
				if (g1.Cols[i].Caption != _lblGrade)
					g4.Cols[i].AllowMerging = true;
				// END MID Track #3125  
				
				g7.Cols[i].AllowMerging = true;
				g10.Cols[i].AllowMerging = true;

				g4.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g7.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g10.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
			
			}
			
			for (i = 0; i < g2.Cols.Count; i++)
			{	
				MinColWidth = g2.Cols[i].Width;
				// Begin MID Track 4708 Size Performance: Removed AutoSizeCols
				//MinColWidth = Math.Max(MinColWidth, g5.Cols[i].Width);
				//MinColWidth = Math.Max(MinColWidth, g8.Cols[i].Width);
				// end MID Track 4708 Size Performance: Removed AutoSizeCols
				TagForColumn colTag = new TagForColumn();
				colTag = (TagForColumn)g2.Cols[i].UserData;
				AllocationWaferCoordinate wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
				if (wafercoord.Label == _lblShipDate
					|| wafercoord.Label == _lblNeedDate)
				{
					MinColWidth = (int)(1.9*MinColWidth +.5); // MID Track Size Performance
				}
				else
				{
					MinColWidth = Math.Max(MinColWidth, g11.Cols[i].Width);
					MinColWidth = MinColWidth + 5;
				}

				g2.Cols[i].Width = MinColWidth;
				g5.Cols[i].Width = MinColWidth;
				g8.Cols[i].Width = MinColWidth;
				g11.Cols[i].Width = MinColWidth;
				
				g5.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g8.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g11.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;

			}
            // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
            //     Unrelated to actual issue but more noticeable when column headings are long 
            //     Top column heading width may be truncated because AutoSizeCols is ignored on Merged columns, so disable merge, autosize, set widths, then re-enable merge
            g3.AllowMerging = AllowMergingEnum.None;
            g3.AutoSizeCols();
            // End TT#234  
			for (i = 0; i < g3.Cols.Count; i++)
			{	
				MinColWidth = g3.Cols[i].Width;
				// Begin MID Track 4708 Size Performance: Removed AutoSizeCols
				//MinColWidth = Math.Max(MinColWidth, g6.Cols[i].Width);
				//MinColWidth = Math.Max(MinColWidth, g9.Cols[i].Width);
				// end MID Track 4708 Size Performance: Removed AutoSizeCols
				MinColWidth = Math.Max(MinColWidth, g12.Cols[i].Width);
				MinColWidth = MinColWidth + 5; // MID Track 4708 Size Performance

				g3.Cols[i].Width = MinColWidth;
				g6.Cols[i].Width = MinColWidth;
				g9.Cols[i].Width = MinColWidth;
				g12.Cols[i].Width = MinColWidth;
				
				g6.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g9.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g12.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
			}
            // Begin TT#234 - RMatelic - Error on multi Header when different size RIDs have the same column name(secondary)
            g3.AllowMerging = AllowMergingEnum.RestrictCols;
            // End TT#234 
		}
		private void ResizeRows()
		{
            g1.AutoSizeRows();
            g2.AutoSizeRows();
            g3.AutoSizeRows();

			//Resize Rows on all grids. Line-up data based on the tallest row.
			//But in this particular view, g1 has a different number of rows from g2 & g3.
			//So we'll just do it one by one instead of in a loop like in other views.
			int MaxRowHeight = 0;  //temp variable to hold the currently tallest row.
			if (g2.Rows[0].HeightDisplay > MaxRowHeight) 
				MaxRowHeight = g2.Rows[0].HeightDisplay;

			//if (g3.Rows[0].HeightDisplay > MaxRowHeight) 
			//		MaxRowHeight = g3.Rows[0].HeightDisplay;
 
			MaxRowHeight += 5;  // Adds additional space for radio button groups
			g2.Rows[0].HeightDisplay = MaxRowHeight;
			g3.Rows[0].HeightDisplay = MaxRowHeight;
			
			MaxRowHeight = 0;  //temp variable to hold the currently tallest row.
			if (g1.Rows[0].HeightDisplay > MaxRowHeight) MaxRowHeight = g1.Rows[0].HeightDisplay;
			if (g2.Rows.Count > 1)
			{
				if (g2.Rows[1].HeightDisplay > MaxRowHeight) MaxRowHeight = g2.Rows[1].HeightDisplay;
			}
			if (g3.Rows.Count > 1)
			{
				if (g3.Rows[1].HeightDisplay > MaxRowHeight) MaxRowHeight = g3.Rows[1].HeightDisplay;
			}
			g1.Rows[0].HeightDisplay = MaxRowHeight;
			if (g2.Rows.Count > 1)
			{
				g2.Rows[1].HeightDisplay = MaxRowHeight;
			}
			if (g3.Rows.Count > 1)
			{
				g3.Rows[1].HeightDisplay = MaxRowHeight;
			}

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
        // Begin TT#607-MD - RMatelic - Size Review not displaying new sizes added when header is dropped on an assortment placeholder
        public void UpdateDataReload()
        {
            try
            {
                _saveCurrentColumns = true;
                SaveCurrentColumns();
                _trans.ResetFirstBuild(true);
                _trans.ResetFirstBuildSize(true);
                _trans.ResetSizeViewGroups();
                
                 
                _fromAssrtReload = true;
                _g8GridTable = null;
                _g9GridTable = null;
                _g11GridTable = null;
                _g12GridTable = null;
                UpdateData();
                _fromAssrtReload = false;
            }
            catch
            {
                throw;
            }
        }
        // End  TT#607 

		public void UpdateData()
		{
			GetCurrentSettings();
			CriteriaChanged();
		}
		private void CriteriaChanged()
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;

                // Begin TT#1009 - RMatelic Unhandled exception while working in size review;  
                _gridRebuildInProgress = true;  
                if (!_trans.AnalysisOnly)
                {
                    gbxGroupBy.Enabled = false;
                }
                gbxSeqMatrix.Enabled = false;
                gbxSizeVar.Enabled = false;
                cmbFilter.Enabled = false;
                btnApply.Enabled = false;
                btnProcess.Enabled = false;
                cboAction.Enabled = false;
                // End TT#1009

                // Begin TT#3613 - JSmith - Size Review:  Sizes get wiped off of the display when changes are made
                _saveCurrentColumns = true;
                // End TT#3613 - JSmith - Size Review:  Sizes get wiped off of the display when changes are made
				// Begin TT#607-MD - RMatelic - Size Review not displaying new sizes added when header is dropped on an assortment placeholder
                if (!_fromAssrtReload)
                {
                    SaveCurrentColumns();
                }
                // End TT#697-MD
				
                _wafers = _trans.AllocationWafers;
				SetGridRedraws(false);
				Formatg1Grid();
				Formatg2Grid();
				Formatg3Grid();
              
                // Begin TT#607-MD - RMatelic - Size Review not displaying new sizes added when header is dropped on an assortment placeholder
                if (_fromAssrtReload)
                {
                    Formatg456Grids();
                }
                // End TT#697-MD

                Add456DataRows(_wafers, _g456GridTable, out _g456DataView, g4, g5, g6); 
				
				FormatGrid7_10 (FromGrid.g7); 
				//FormatGrid8_12 (FromGrid.g8); 
				//FormatGrid8_12 (FromGrid.g9);
				FormatGrid7_10 (FromGrid.g10); 
				//FormatGrid8_12 (FromGrid.g11); 
				//FormatGrid8_12 (FromGrid.g12); 
			 
				FormatGridFromDataTable (FromGrid.g8);
				FormatGridFromDataTable (FromGrid.g9);
				FormatGridFromDataTable (FromGrid.g11);
				FormatGridFromDataTable (FromGrid.g12);
			
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

                SortColumns(_g456DataView, g4, g5, g6);       // TT#456 - RMatelic - Add views to Size Review
				_setHeaderTags = false;
				AssignTag();
				SetStyles();
				if (!FormIsClosing)
				{			
					SetGridStyles(false,true);
                    //ApplyPreferences();           // TT#456 - RMatelic - Add views to Size Review
					// BEGIN MID Track #2980 - can't scroll to right end of grid
					SetV1SplitPosition();
					// END MID Track #2980   
					UserSetSplitter1Position = VerticalSplitter1.SplitPosition;
					UserSetSplitter2Position = VerticalSplitter2.SplitPosition;
                    // Begin TT#456 - RMatelic -Add views to Size Review
                    //SetScrollBarPosition(hScrollBar2, ((GridTag)g2.Tag).CurrentScrollPosition);
                    //SetScrollBarPosition(hScrollBar3, ((GridTag)g3.Tag).CurrentScrollPosition);
                    ApplyCurrentColumns();
                    //SortColumns(_g456DataView, g4, g5, g6);
                    ShowHideRows();
                    SetScrollBarPosition(hScrollBar2, ((GridTag)g2.Tag).CurrentScrollPosition);
                    SetScrollBarPosition(hScrollBar3, ((GridTag)g3.Tag).CurrentScrollPosition);
                    SetHScrollBar1Parameters();
                    SetHScrollBar2Parameters();
                    SetHScrollBar3Parameters();
                    // End TT#456 

					SetGridRedraws(true);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
                // Begin TT#1009 - RMatelic Unhandled exception while working in size review;
                if (!_trans.AnalysisOnly)
                {
                    gbxGroupBy.Enabled = true;
                }
                gbxSeqMatrix.Enabled = true;
                gbxSizeVar.Enabled = true;
                if (!_trans.AnalysisOnly)
                {
                    btnApply.Enabled = (g4.Rows.Count > 0 && (_trans.DataState == eDataState.Updatable));
                }
                //BEGIN TT#6-MD-VStuart - Single Store Select
                cmbFilter.Enabled = true;  //TT#6-MD-VStuart - Single Store Select
                //cboAction_SelectionChangeCommitted(cboAction, null);
                cboAction_SelectionChangeCommitted(cboAction.ComboBox, null);
                //END TT#6-MD-VStuart - Single Store Select
                SetActionListEnabled();
                _gridRebuildInProgress = false;  
                // End TT#1009 
                // Begin Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set
                _applyPending = false;
                _applyPendingMsgDisplayed = false;
                // End Development TT#8
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
                        ExportSpecificStyle(MIDExportFile, "g7", g7, "Reverse1Left");
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
            int rowsPerStoreGroup = 1;
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
                ////this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs()); //TT#306-MD-VStuart-Version 5.0-Size Review not working correctly. // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected
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
                //Begin TT#866 - JSmith - Export of multiple sets has the incorrect data in the store columns
                //AllocationWaferGroup wafers;
                //DataView g456DataView = null;
                //DataTable g456GridTable = null;

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

                //    ShowHideColHeaders(FromGrid.g1, ColHeaders1, false, exportG4);
                //    ShowHideColHeaders(FromGrid.g2, ColHeaders2, false, exportG5);
                //    ShowHideColHeaders(FromGrid.g3, ColHeaders3, false, exportG6);
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
                    ////this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs()); //TT#306-MD-VStuart-Version 5.0-Size Review not working correctly. // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected
                }

                exportG4 = g4;
                exportG5 = g5;
                exportG6 = g6;
                //End TT#866

                groupingName = sglProf.Name;


                if (aAddHeader)
                {
                    totRows += g1.Rows.Count;
                }
                totRows += g4.Rows.Count;
                totRows += g7.Rows.Count;
                totRows += g10.Rows.Count;

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

                CellStyle style = g7.GetCellStyle(0, 0);
                // add set values
                ExportAddWafers(aMIDExportFile, g7, g8, g9, sglProf.Name, aRowsPerGroup,
                        "g7Reverse1Left", "g7Reverse1Left", "g7Reverse1Left", "g8Editable1", "g8Negative1", "g9Editable1", "g9Negative1");

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
                TagForRow rowTag;
                AllocationWaferCoordinate waferCoord;

                for (i = 0; i < aHeadingGrid.Rows.Count; i++)
                {
                    if (!aHeadingGrid.Rows[i].Visible)
                    {
                        continue;
                    }

                    // only output row for current set
                    if (aSetName != null)
                    {
                        rowTag = (TagForRow)aHeadingGrid.Rows[i].UserData;
                        waferCoord = GetAllocationCoordinate(rowTag.CubeWaferCoorList, eAllocationCoordinateType.StoreAllocationNode);
                        if (waferCoord.Label != aSetName)
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
                                aMIDExportFile.AddValue(aDetailGrid[i, j].ToString(), eExportDataType.Value, textStyle, negativeStyle);
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
				 			
			try
			{
				_foundRow = 0;
				_foundColumn = 0;
                ProfileList storeProfileList = StoreMgmt.StoreProfiles_GetActiveStoresList(); //_sab.StoreServerSession.GetActiveStoresList();
				
				BuildSizeList();
				_quickFilterType = eQuickFilterType.Find;
				//_quickFilter = new QuickFilter(eQuickFilterType.Find, 5, _lblStore + ": ",
				//	                       _lblHeader + ": ", _lblColor + ": ", "Size: ", _lblComponent + ": ");
				// BEGIN TT#2703 - stodd - select first comboBox on QuickFilter
				_quickFilter = new QuickFilter(eQuickFilterType.Find, 4, true, _lblStore + ": ",
					_lblColor + ": ", _lblDimSize + ": ", _lblComponent + ": ");
				_quickFilter.OnValidateFieldsHandler += new QuickFilter.ValidateFieldsHandler(OnValidateFieldsHandler);
				// END TT#2703 - stodd - select first comboBox on QuickFilter
				
				_quickFilter.EnableComboBox(0);
                _quickFilter.LoadComboBox(0, storeProfileList.ArrayList);
                //BEGIN TT#6-MD-VStuart - Single Store Select
                _quickFilter.LoadComboBoxAutoFill(0, storeProfileList.ArrayList);
                //END TT#6-MD-VStuart - Single Store Select


				//				if (_headerList.Count > 1)
				//				{	
				//					_quickFilter.EnableComboBox(1);
				//					BuildHeaderList();
				//					_quickFilter.LoadComboBox(1, _headerArrayList);
				//				}
				//				else	
				//					_quickFilter.DisableComboBox(1);
				
				if (_colorDataTable.Rows.Count > 1)
				{
					_quickFilter.EnableComboBox(1);
					_quickFilter.LoadComboBox(1, _colorDataTable, "ColorName");
				}
				else	
					_quickFilter.DisableComboBox(1);
				
				_quickFilter.EnableComboBox(2);
				//_quickFilter.LoadComboBox(2, _sizeArrayList);  // MID Track 3611 Quick Filter not working in Size Review
				_quickFilter.LoadComboBox(2, _sizeDataTable, "SizeName"); // MID Track 3611 Quick Filter not working in Size Review

				_quickFilter.EnableComboBox(3);
				_quickFilter.LoadComboBox(3, _alColHeaders2);

				diagResult = _quickFilter.ShowDialog(this);

				if (diagResult == DialogResult.OK)
				{
					SetScrollBarPosition(hScrollBar3, _foundColumn);

					int barValue = _foundRow / _rowsPerGroup4;
					if (barValue > vScrollBar2.Maximum)
						vScrollBar2.Value = vScrollBar2.Maximum;
					else if (barValue < vScrollBar2.Minimum)
						vScrollBar2.Value = vScrollBar2.Minimum;
					else
						vScrollBar2.Value = barValue ;
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
			bool OKToProcess = true, rowFound = false;
			StoreProfile storeProf;
			ProfileXRef storeSetXRef;
			ArrayList totalList;
			int storeCol = 0, headerCol = 0, colorCol = 0, varCol = 0, dimCol = 0;
			string headerID = null, colorLabel = null;
			string  colLabel, errorMessage = string.Empty, hdrValue, colorValue;
			string title = _quickFilter.Text; 
			try
			{
				//_foundRow = 0;
				for (int i = 0; i < g1.Cols.Count - 1; i++)
				{
					colLabel = Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture);
					if (colLabel == _lblStore)
						storeCol = i;
					else if (colLabel == _lblHeader)
						headerCol = i;
					else if (colLabel == _lblColor)
						colorCol = i;
					else if (colLabel == _lblVariable)
						varCol = i;
					else if (colLabel == _lblDimension)
						dimCol = i;
				}
				if (_quickFilter.GetSelectedIndex(0) >= 0)
				{
					storeProf = (StoreProfile)_quickFilter.GetSelectedItem(0);
					storeSetXRef = (ProfileXRef)_trans.GetProfileXRef(new ProfileXRef(eProfileType.StoreGroupLevel, eProfileType.Store));
					totalList = storeSetXRef.GetTotalList(storeProf.Key);
					cmbAttributeSet.SelectedValue = totalList[0];
                    ////this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs()); //TT#306-MD-VStuart-Version 5.0-Size Review not working correctly. // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected
				
					_foundRow = g4.FindRow(storeProf.Text,0,storeCol,false);
					if (_foundRow == -1)
						_foundRow = 0;
				}
				else
					//_foundRow = g4.TopRow;
					_foundRow = 0;

				//if (_quickFilter.GetSelectedIndex(1) >= 0)
				// 	headerID = Convert.ToString(_quickFilter.GetSelectedItem(1),CultureInfo.CurrentUICulture);
							
				if (_quickFilter.GetSelectedIndex(1) >= 0)
					colorLabel = Convert.ToString(_quickFilter.GetSelectedItem(1),CultureInfo.CurrentUICulture);

				// header is has been taken out for the time being so it
				// should always = null for now. Left code in in case it retuurns as
				// selection option
				if (headerID != null || colorLabel != null)
				{
					for (int i = _foundRow; i < g4.Rows.Count; i++)
					{
						hdrValue = Convert.ToString(g4.GetData(i, headerCol), CultureInfo.CurrentUICulture);
						colorValue = Convert.ToString(g4.GetData(i, colorCol), CultureInfo.CurrentUICulture);
						if (headerID != null && colorLabel != null)
						{
							if (hdrValue == headerID && colorValue == colorLabel)
							{	
								_foundRow = i;
								rowFound = true;
								break;
							}
						}
						else if (headerID != null)
						{
							if (hdrValue == headerID)
							{
								_foundRow = i;
								rowFound = true;
								break;
							}
						}
						else
						{
							if (colorValue == colorLabel)
							{
								_foundRow = i;
								rowFound = true;
								break;
							}
						}
					}
					if (!rowFound)
					{
						OKToProcess = false;
						errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ComponentNotInHeader);
						_quickFilter.SetError(2,errorMessage);
						MessageBox.Show(errorMessage, title);
					}
				}
				if (OKToProcess)
					FindColumn();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
			return OKToProcess;
		}
	
		private void FindColumn()
		{
			AllocationWaferCoordinate wafercoord;
			object varLabel = null, sizeLabel = null;
			TagForColumn colTag;
			try
			{
				//_foundColumn = 0;
				
				if (_quickFilter.GetSelectedIndex(2) >= 0)
					sizeLabel = _quickFilter.GetSelectedItem(2);
				
				if (_quickFilter.GetSelectedIndex(3) >= 0)
					varLabel = _quickFilter.GetSelectedItem(3);
			
				if (sizeLabel == null && varLabel == null) // Nothing was selected
					return;
				
				for (int i = 0; i < g3.Cols.Count; i++)
				{
					colTag = (TagForColumn)g3.Cols[i].UserData;
					if (sizeLabel != null)
					{
						wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.PrimarySize);
						if (wafercoord.Label == Convert.ToString(sizeLabel, CultureInfo.CurrentUICulture)) 
						{
							if (varLabel != null)
							{
								wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
								if (wafercoord.Label == Convert.ToString(varLabel, CultureInfo.CurrentUICulture)) 
								{ 
									_foundColumn = i; 
									break;
								}
							}
							else
							{
								_foundColumn = i; 
								break;
							}
						}
					}
					else // only variable selected 
					{
						wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
						if (wafercoord.Label == Convert.ToString(varLabel, CultureInfo.CurrentUICulture)) 
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
			GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
			bool reloadGrid;
			try
			{
				//BuildHeaderSortedList();  // header is not included for now
				
				BuildPackList(eComponentType.SpecificPack);
				_quickFilterType = eQuickFilterType.QuickFilter;
				//_quickFilter = new QuickFilter(eQuickFilterType.QuickFilter, 1, 1, 4, _lblHeader + ":  ", string.Empty, string.Empty, string.Empty);
				_quickFilter = new QuickFilter(eQuickFilterType.QuickFilter, 1, 1, 4, string.Empty, string.Empty, string.Empty, string.Empty);	
				_quickFilter.EnableComboBox(0);
				_quickFilter.SetComboBoxSort(0,false);
				_quickFilter.EnableComboBox(1);
				_quickFilter.SetComboBoxSort(1,false);
				_quickFilter.EnableComboBox(2);
				_quickFilter.SetComboBoxSort(2,false);
				_quickFilter.EnableComboBox(3);
				_quickFilter.SetComboBoxSort(3,false);		 
				
				//_quickFilter.LoadComboBox(0, _headerSortedList);
				//_quickFilter.SetComboxBoxIndex(0,1);
				//if (_headerList.Count == 1)
				//	_quickFilter.DisableComboBoxLeaveSelected(0);

				BuildTotalComponentList();
				_quickFilter.LoadComboBox(0, _componentDataTable, "Name");
				_quickFilter.LoadComboBoxLabel(0, _lblComponent + ":");
				if (_componentDataTable.Rows.Count == 1)
					_quickFilter.DisableComboBoxLeaveSelected(0);
				
				BuildSizeList();
				//_quickFilter.LoadComboBox(1, _sizeArrayList); // MID Track 3611 Quick Filter not working in Size Review
				_quickFilter.LoadComboBox(1, _sizeDataTable, "SizeName"); // MID Track 3611 Quick Filter not working in Size Review
				_quickFilter.LoadComboBoxLabel(1, _lblDimSize + ":");

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
					_quickFilterData.CheckBoxChecked[0] = true;
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
						//name = Convert.ToString(_quickFilter.GetSelectedItem(0),CultureInfo.CurrentUICulture);
					
						dRow = _componentDataTable.Rows[_quickFilter.GetSelectedIndex(0)];	
						eComponentType compType = (eComponentType)dRow["ComponentType"];
						
						switch (compType)
						{
							case eComponentType.Total:
								aComponent = new GeneralComponent(eGeneralComponentType.Total);
								break;
							
							case eComponentType.SpecificPack:
								string packName  = Convert.ToString(dRow["Name"], CultureInfo.CurrentUICulture);
								aComponent = aComponent = new AllocationPackComponent(packName);
								break;

							case eComponentType.Bulk:
								aComponent = new GeneralComponent(eGeneralComponentType.Bulk);
								break;

							case eComponentType.SpecificColor:
							{
								int colorRID = Convert.ToInt32(dRow["Key"], CultureInfo.CurrentUICulture);
								//aComponent = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, colorRID);
								// begin MID Track 3611 Quick Filter not working in Size Review
								int sizeQuickSelectIndex = _quickFilter.GetSelectedIndex(1);
								if (sizeQuickSelectIndex < 0)
								{
									aComponent = new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, colorRID);
								}
								else
								{
									dRow = _sizeDataTable.Rows[sizeQuickSelectIndex];
									eSpecificBulkType sizeType = (eSpecificBulkType)Convert.ToInt32(dRow["SizeType"], CultureInfo.CurrentUICulture);
									switch (sizeType)
									{
										case eSpecificBulkType.SpecificSizePrimaryDim:
										{
											aComponent = new AllocationColorSizeComponent(
												new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, colorRID),
												new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificSizePrimaryDim, (int)(Convert.ToInt64(dRow["PrimeSecondSizeRIDs"], CultureInfo.CurrentUICulture)>> 32) ));
											break;
										}
										case eSpecificBulkType.SpecificSizeSecondaryDim:
										{
											aComponent = new AllocationColorSizeComponent(
												new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, colorRID),
												new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificSizeSecondaryDim, (int)(Convert.ToInt64(dRow["PrimeSecondSizeRIDs"], CultureInfo.CurrentUICulture))));
											break;
										}
										case eSpecificBulkType.SpecificSize:
										{
											int sizeRID = (int)((SizeCodeProfile)(_trans.GetSizeCodeByPrimarySecondary(
												(int)(Convert.ToInt64(dRow["PrimeSecondSizeRIDs"], CultureInfo.CurrentUICulture)>> 32),
												(int)(Convert.ToInt64(dRow["PrimeSecondSizeRIDs"], CultureInfo.CurrentUICulture)))).ArrayList[0]).Key;
											aComponent = new AllocationColorSizeComponent(
												new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, colorRID),
												new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificSize, sizeRID));
											break;
										}
										default:
										{
											aComponent = new AllocationColorSizeComponent(
												new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificColor, colorRID),
												new GeneralComponent(eGeneralComponentType.AllSizes));
											break;
										}
									}
								}
								//new AllocationColorOrSizeComponent(eSpecificBulkType.SpecificSize, Convert.ToInt32(dRow["SizeRID"], CultureInfo.CurrentUICulture)));
								// end MID Track 3611 Quick Filter not working in Size Review
								break;
							}
						}

						dRow = _variableDataTable.Rows[_quickFilter.GetSelectedIndex(2)];	
						wVariable = (eAllocationWaferVariable)dRow["TEXT_CODE"];
					
						dRow = _conditionDataTable.Rows[_quickFilter.GetSelectedIndex(3)];	
						condition = (eFilterComparisonType)dRow["TEXT_CODE"];

						qty = Convert.ToDouble(_quickFilter.GetTextBoxText(0).Trim(),CultureInfo.CurrentUICulture);
						//if (name == _totalVariableName)
						reloadGrid = _trans.ApplyAllocationQuickFilter(string.Empty, aComponent, wVariable, qty, condition); 
						//else
						//{
						//	int headerRID = (int)_headerSortedList.GetKey(_quickFilter.GetSelectedIndex(0));
						//	reloadGrid = _trans.ApplyAllocationQuickFilter(headerRID, aComponent, wVariable, qty, condition); 
						//}
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
			catch (Exception ex)
			{
				HandleException(ex);
			} 
		}
	
		private void BuildSizeList()
		{
			_sizeDataTable = this._trans.QuickFilterSizesDropDown;
		}	

		private void BuildTotalComponentList()
		{
			eComponentType compType;
			TagForRow rowTag;
			AllocationWaferCoordinate waferCoord;
			DataColumn dataCol;
			try
			{
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
 
					for (int i = 0; i < g4.Rows.Count; i++)
					{
						rowTag = (TagForRow)g4.Rows[i].UserData;
						waferCoord =  GetAllocationCoordinate( rowTag.CubeWaferCoorList, eAllocationCoordinateType.Component);
						if (waferCoord != null)
						{
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
							//break;
						}
						else
						{
							waferCoord =  GetAllocationCoordinate( rowTag.CubeWaferCoorList, eAllocationCoordinateType.PackName);
							if (waferCoord != null)
							{
								CheckToAddPack(waferCoord);
							}
							//break;
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
			TagForRow rowTag;
			AllocationWaferCoordinate waferCoord;
			bool headerStarted = false;
			try
			{
				for (int i = 0; i < g7.Rows.Count; i++)
				{
					rowTag = (TagForRow)g7.Rows[i].UserData;
					int _hdrRow = 1;
					waferCoord = rowTag.CubeWaferCoorList[_hdrRow];
					if (waferCoord.Label == aHeaderID)
					{
						headerStarted = true;
						int _compRow = 2;
						waferCoord = rowTag.CubeWaferCoorList[_compRow];
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
					else if (headerStarted)
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
						//BuildColorList();
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
			//string headerID;
			switch (e.ComponentIdx)
			{
				case 0:
					//					if (e.SelectedIdx == 0)
					//					{
					//						_componentDataTable = null;
					//						BuildTotalComponentList();
					//					}
					//					else if (e.SelectedIdx > 0)
					//					{
					//						_componentDataTable.Clear();
					//						headerID = Convert.ToString(_quickFilter.GetSelectedItem(0), CultureInfo.CurrentUICulture);
					//						BuildHeaderComponentList(headerID);
					//					}
					//					_quickFilter.LoadComboBox(1, _componentDataTable, "Name");
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
								if (_componentDataTable.Rows.Count == 1)
									_quickFilter.DisableComboBoxLeaveSelected(0);
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
						if (_quickFilter.GetSelectedIndex(i) < 0 // MID Track 3611 allow "none" for Size/Dim
							&& i != 1)                           // MID Track 3611 allow "none" for Size/Dim
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
					//if (s4.SplitPosition < g3.Rows[g3.Rows.Count - 1].Bottom + s4.Height + 3)
					if (s4.SplitPosition < g3.Rows[g3.Rows.Count - 1].Bottom + s4.Height )
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
				//s4.SplitPosition = g3.Rows[g3.Rows.Count - 1].Bottom + s4.Height + 3;
				s4.SplitPosition = g3.Rows[g3.Rows.Count - 1].Bottom + s4.Height;
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
                // MID Track #5309 = grid lines inconsistent -- add *2 in calc 
                s8.SplitPosition = g7.Rows[(_dispRowsPerSet * 2) - 1].Bottom + s8.Height + 2;
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
                s12.SplitPosition = g10.Rows[g10.Rows.Count - 1].Bottom + s12.Height + 0;
      		}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetV1SplitPosition()
		{
			try
			{
				int gbxPos =  gbxSizeVar.Right;
				int colPos = 0;
				for (int j = 0; j < g1.Cols.Count; j++)
				{
					if (g1.Cols[j].Visible)
					{
						colPos += g1.Cols[j].Width;
						//if (colPos >= gbxPos) 
						//	break;
					}	
				}
				if (colPos < gbxPos) 
					colPos = gbxPos;
                if (_loading)
                {
                    UserSetSplitter1Position = colPos + VerticalSplitter1.Width + 3;
                }
                else
                {
                    VerticalSplitter1.SplitPosition = colPos + VerticalSplitter1.Width + 3;
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
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#endregion
		#region ScrollBars Scroll Events

		private void SetVScrollBar2Parameters()
		{
			//int maxScroll;
			try
			{
				//_rowsPerGroup4 = _rowsPerStoreGroup;
				_rowsPerGroup4 = 1;
				vScrollBar2.Minimum = 0;
				vScrollBar2.Maximum = CalculateRowMaximumScroll(g4,_rowsPerGroup4);
				
				vScrollBar2.SmallChange = SMALLCHANGE;
				vScrollBar2.LargeChange = _dispRowsPerSet;
				if (vScrollBar2.Maximum > 1)
				  vScrollBar2.Maximum += (vScrollBar2.LargeChange * 2) + 1;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetVScrollBar3Parameters()
		{
			try
			{
				_rowsPerGroup7 = _rowsPerStoreGroup;
                // BEGIN MID Track #5309 - grid lines inconsistent
                //vScrollBar3.Maximum = CalculateRowMaximumScroll(g7, _rowsPerGroup7);
				
				vScrollBar3.Minimum = 0;
			
				vScrollBar3.SmallChange = SMALLCHANGE;
				vScrollBar3.LargeChange = _dispRowsPerSet;
                vScrollBar3.Maximum = CalculateRowMaximumScroll_3(g7, _rowsPerGroup7);
                // END MID Track #5309
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
				_rowsPerGroup10 = _rowsPerStoreGroup;
				vScrollBar4.Minimum = 0;
				vScrollBar4.Maximum = CalculateRowMaximumScroll_2(g10, _rowsPerGroup10);
				vScrollBar4.SmallChange = SMALLCHANGE;
				vScrollBar4.LargeChange = BIGCHANGE;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        // Begin TT#1761 - RMatelic - Size Review Lacks Page Down Functionality
        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                if (e.Type != ScrollEventType.LargeIncrement && e.Type != ScrollEventType.LargeDecrement)
                {
                    return;
                }

                System.Windows.Forms.KeyEventArgs keyEventArgs = null;
                switch (e.Type)
                {
                    case ScrollEventType.LargeIncrement:
                        keyEventArgs = new KeyEventArgs(Keys.PageDown);
                        break;

                    case ScrollEventType.LargeDecrement:
                        keyEventArgs = new KeyEventArgs(Keys.PageUp);
                        break;
                }
                GridKeyDown(vScrollBar2, keyEventArgs);
                e.NewValue = vScrollBar2.Value;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // End TT#1761
     
       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void vScrollBar2_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				_isScrolling = true;
				g4.TopRow = ((VScrollBar)sender).Value * _rowsPerGroup4;
				g5.TopRow = ((VScrollBar)sender).Value * _rowsPerGroup4;
				g6.TopRow = ((VScrollBar)sender).Value * _rowsPerGroup4;
				_isScrolling = false;
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
                int newValue = ((VScrollBar)sender).Value * 2;
                g7.TopRow = newValue;
                g8.TopRow = newValue;
                g9.TopRow = newValue;
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
				g10.TopRow = ((VScrollBar)sender).Value * _rowsPerGroup10;
				g11.TopRow = ((VScrollBar)sender).Value * _rowsPerGroup10;
				g12.TopRow = ((VScrollBar)sender).Value * _rowsPerGroup10;
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
						
						((ScrollBarValueChanged)hScrollBar2.Tag)(e.NewValue);
					
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
			int i;
			try
			{    
				switch (e.Type)
				{
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

		private void ChangeHScrollBar2Value(int aNewValue)
		{
			try
			{
				_isScrolling = true;
				g2.LeftCol = aNewValue;
				g5.LeftCol = aNewValue;
				g8.LeftCol = aNewValue;
				g11.LeftCol = aNewValue;
				if (g2.LeftCol < aNewValue)
				{
					aNewValue = g2.LeftCol;
                    //Begin TT#1319-MD -jsobek -Allocation>Review>Select>Size and Size Analysis after filling in data and select OK receive a system argument out of range exception
                    if (aNewValue < hScrollBar2.Minimum)
                    {
                        aNewValue = hScrollBar2.Minimum;
                    }
                    else if (aNewValue > hScrollBar2.Maximum)
                    {
                        aNewValue = hScrollBar2.Maximum;
                    }
					hScrollBar2.Value = aNewValue;
                    //End TT#1319-MD -jsobek -Allocation>Review>Select>Size and Size Analysis after filling in data and select OK receive a system argument out of range exception
				}
				((GridTag)g2.Tag).CurrentScrollPosition = aNewValue;
				_isScrolling = false;
                // Begin TT#2027 - JSmith - System out of Argument Range
                _arrowScroll = false;
                // End TT#2027
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void ChangeHScrollBar3Value(int aNewValue)
		{
			try
			{
				_isScrolling = true;
				g3.LeftCol = aNewValue;
				g6.LeftCol = aNewValue;
				g9.LeftCol = aNewValue;
				g12.LeftCol = aNewValue;
				if (g3.LeftCol < aNewValue)
				{
					aNewValue = g3.LeftCol;
                    // Begin TT#1243 - JSmith - Velocity Sotre detail / Tools Refresh get message
                    if (aNewValue < 0)
                    { 
                        aNewValue = 0; 
                    }
                    // End TT#1243 - JSmith - Velocity Sotre detail / Tools Refresh get message
					hScrollBar3.Value = aNewValue;
				}
				((GridTag)g3.Tag).CurrentScrollPosition = aNewValue;
				_isScrolling = false;
                // Begin TT#2027 - JSmith - System out of Argument Range
                _arrowScroll = false;
                // End TT#2027
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
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
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SetHScrollBar1Parameters()
		{
			int maxScroll;
			try
			{
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
			int maxScroll;
			try
			{   // for some reason the other CalculateColMaximumScroll 
				// doesn't work correctly for this g2 grid 
				if (hScrollBar2.Visible)
				{
					maxScroll = CalculateColMaximumScroll_2(g2, 1);
					hScrollBar2.Minimum = 0;
					hScrollBar2.Maximum = maxScroll + _colsPerGroup2 - 1;
					hScrollBar2.SmallChange = SMALLCHANGE;
					hScrollBar2.LargeChange = BIGHORIZONTALCHANGE;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void SetHScrollBar3Parameters()
		{
			int maxScroll;
			try
			{
				//maxScroll = CalculateColMaximumScroll_2(g3, 1);
				maxScroll = CalculateColMaximumScroll(g3,1);
				hScrollBar3.Minimum = 0;
				//hScrollBar3.Maximum = maxScroll 
			
				hScrollBar3.SmallChange = SMALLCHANGE;
				if (ColHeaders3 == null || ColHeaders3.Count == 0)
				{
					hScrollBar3.Maximum = maxScroll;
					hScrollBar3.LargeChange = BIGHORIZONTALCHANGE;
				}
				else
				{
					hScrollBar3.Maximum = maxScroll + ColHeaders3.Count - 1;
					hScrollBar3.LargeChange = ColHeaders3.Count;
				}	
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
				totalColSize = 0;
				for (i = aGrid.Cols.Count - 1; totalColSize <= aGrid.Width && i >= 0; i -= aScrollSize)
				{
					if (aGrid.Cols[i].Visible)
					{
						for (j = 0; j < aScrollSize; j++)
						{
							totalColSize += aGrid.Cols[i - j].WidthDisplay;
						}
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
			int totalColSize;
			int i, j;

			totalColSize = 0;
			for (i = aGrid.Cols.Count - 1; totalColSize <= aGrid.Width && i >= 0; i -= aNumColsPerGroup)
			{
                // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review >>> add ' if ...' condition
                if (aGrid.Cols[i].Visible)
                {
                    for (j = 0; j < aNumColsPerGroup; j++)
                    {
                        totalColSize += aGrid.Cols[i - j].WidthDisplay;
                    }
                }
			}   // End TT#1144 

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

        // BEGIN MID Track #5309 - grid lines inconsistent - this change isn't related to the lines
        private int CalculateRowMaximumScroll_2(C1FlexGrid aGrid, int aScrollSize)
        {
            int totalRowSize;
            int i, j, k = 0;

            try
            {
                totalRowSize = 0;
                for (i = aGrid.Rows.Count - 1; totalRowSize <= aGrid.Height && i >= 0; i--)
                {
                    if (aGrid.Rows[i].Visible)
                    {
                        for (j = 0; j < aScrollSize; j++)
                        {
                            k = i - j;
                            while (k >= 0 && !aGrid.Rows[k].Visible)
                            {
                                k--;
                            }
                            if (k >= 0)     //  Fix Size View Out of Range error
                            {
                                totalRowSize += aGrid.Rows[k].HeightDisplay;
                            }
                        }
                        i = k;
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
		
        private int CalculateRowMaximumScroll_3(C1FlexGrid aGrid, int aScrollSize)
        {
            int totalRowSize, dispRowCount = 0;
            int i, j, k = 0;
            try
            {
                totalRowSize = 0;
                for (i = aGrid.Rows.Count - 1; totalRowSize <= aGrid.Height && i >= 0; i--)
                {
                    if (aGrid.Rows[i].Visible)
                    {
                        for (j = 0; j < aScrollSize; j++)
                        {
                            k = i - j;
                            while (k >= 0 && !aGrid.Rows[k].Visible)
                            {
                                k--;
                            }
                            if (k >= 0)  // Fix:  Indes Out of Range
                            {
                                totalRowSize += aGrid.Rows[k].HeightDisplay;
                            }
                        }
                        i = k;
                    }
                }
                dispRowCount = (aGrid.BottomRow - aGrid.TopRow);


                if (totalRowSize > aGrid.Height)
                {
                    if (dispRowCount >= _dispRowsPerSet)
                    {
                        return (_dispRowsPerSet * cmbAttributeSet.Items.Count) - 1;
                    }
                    else
                    {
                        return (aGrid.Rows.Count + dispRowCount) / 2;
                    }
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
        // END MID Track #5309

		public delegate void ScrollBarValueChanged(int aNewValue);


		#endregion
		#region MouseDown Events
		private void GridMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			int whichGrid;
			C1FlexGrid grid = null;
			bool setDragReady = false;
			
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
                                    // Begin TT#3745 & TT#1178-MD - RMatelic - GA Matrix Urban- Size review with sequential selected cannot sort high to low on the columns
                                    //if (GridMouseRow == 1 && rbHeader.Checked) 
                                    //    setDragReady = true;
                                    //else if (GridMouseRow == 0 && rbColor.Checked)
                                    // End TT#3745 & TT#1178-MD
										setDragReady = true;
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
			C1FlexGrid grid;
			try
			{
				if (_loading) return;

				grid = (C1FlexGrid)sender; 
			 
				if (grid == g4 || grid == g7 || grid == g10) 
				 	e.Cancel = true;
				else
				{
					TagForGridData DataTag = (TagForGridData)grid.GetCellRange(e.Row, e.Col).UserData;
					if (!DataTag.IsEditable)
						e.Cancel = true;
				}
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
			int cellRow, cellCol, storeCol = 0, headerCol = 0;
			int colorCol = 0, varCol = 0, dimCol = 0;
			double cellValue;
			string StoreID, colLabel;
			
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
						colLabel = Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture);
						if (colLabel == _lblStore)
							storeCol = i;
						else if (colLabel == _lblHeader)
							headerCol = i;
						else if (colLabel == _lblColor)
							colorCol = i;
						else if (colLabel == _lblVariable)
							varCol = i;
						else if (colLabel == _lblDimension)
							dimCol = i;
					}
					StoreID = Convert.ToString(rowGrid[e.Row, storeCol], CultureInfo.CurrentUICulture) + " " ;
					if (rbHeader.Checked)
					{	
						StoreID += Convert.ToString(rowGrid[e.Row, headerCol], CultureInfo.CurrentUICulture)+  " " ;
						StoreID += Convert.ToString(rowGrid[e.Row, colorCol], CultureInfo.CurrentUICulture) + " " ;
					}
					else
					{
						StoreID += Convert.ToString(rowGrid[e.Row, colorCol], CultureInfo.CurrentUICulture) + " " ;
						StoreID += Convert.ToString(rowGrid[e.Row, headerCol], CultureInfo.CurrentUICulture)+  " " ;
					}
					if (rbSizeMatrix.Checked)
						StoreID += Convert.ToString(rowGrid[e.Row, dimCol], CultureInfo.CurrentUICulture) + " " ;

					StoreID += Convert.ToString(rowGrid[e.Row, varCol], CultureInfo.CurrentUICulture) + " " ;
					
					// BEGIN MID Track #2511 - Sort not working
					//cellRow = (Int32)CellRows[StoreID];
					TagForRow rowTag = (TagForRow)CellRows[StoreID];
					cellRow = rowTag.cellRow;
					// END MID Track #2511
					ResizeColumns();
					SetHScrollBar1Parameters();
					SetHScrollBar2Parameters();
					SetHScrollBar3Parameters();
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
				double test = cellValue % cells[cellRow,cellCol].Multiple;
				if (cellValue % cells[cellRow,cellCol].Multiple != 0)
				{	
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_MultipleValueIncorrect,
						String.Format
						(
						MIDText.GetText(eMIDTextCode.msg_al_MultipleValueIncorrect),
						cellValue.ToString(CultureInfo.CurrentUICulture),
						cells[cellRow,cellCol].Multiple.ToString(CultureInfo.CurrentUICulture) 
						));
				}	
				_changedGrid = null;
				// begin TT#59 Implement Temp Locks
                //_trans.SetAllocationCellValue( waferRow, waferCol, cellRow, cellCol, cellValue); 
                _allocationWaferCellChangeList.AddAllocationWaferCellChange(waferRow, waferCol, cellRow, cellCol, cellValue);
                // end TT#59 Implement Temp Locks
				ChangePending = true;
                // Begin Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set
                _applyPending = true;
                // End Development TT#8
				ChangeCellStyles(grid, colGrid, e.Row, e.Col);

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
		// BEGIN MID Track #3840 - unhandled exception on PageUp key
		private void GridKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			C1FlexGrid grid;
			int whichGrid;
			try
			{
				grid = (C1FlexGrid)sender;
				whichGrid = ((GridTag)grid.Tag).GridId;
				
				switch((FromGrid)whichGrid)
				{
					case FromGrid.g1:
					case FromGrid.g2:
					case FromGrid.g3:
						e.Handled = true;
						break;
                    // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality 
                    case FromGrid.g4:                       
                    case FromGrid.g5:
                    case FromGrid.g6:
                        GridKeyDown(vScrollBar2, e);
                        // Begin TT#1917 - RMatelic - Style/Size Review Tab Down Capabilities && TT#2026-Size Allocation does not hold current column when manually entering size quantities
                        //e.Handled = true;
                        // End TT#1917 && TT#2026
                        break;

                    case FromGrid.g7:
                    case FromGrid.g8:
                    case FromGrid.g9:
                        GridKeyDown(vScrollBar3, e);
                        // Begin TT#1917 - RMatelic - Style/Size Review Tab Down Capabilities && TT#2026-Size Allocation does not hold current column when manually entering size quantities
                        //e.Handled = true;
                        // End TT#1917 && TT#2026
                        break;

                    case FromGrid.g10:
                    case FromGrid.g11:
                    case FromGrid.g12:
                        GridKeyDown(vScrollBar4, e);
                        // Begin TT#1917 - RMatelic - Style/Size Review Tab Down Capabilities && TT#2026-Size Allocation does not hold current column when manually entering size quantities
                        //e.Handled = true;
                        // End TT#1917 && TT#2026
                        break;
                    // End TT#1542
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		// END MID Track #3840

        // Begin TT#1542 - RMatelic - Size Review lacks Page Down functionality
        private void GridKeyDown(VScrollBar aScrollBar, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                int rowCalc = 0;

                if (aScrollBar == vScrollBar2)
                {
                    rowCalc = g4.BottomRow - g4.TopRow;
                }
                else if (aScrollBar == vScrollBar3)
                {
                    if (g7.BottomRow > 0 && g7.BottomRow == g7.TopRow)
                    {
                        rowCalc = 1;
                    }
                    else
                    {
                        rowCalc = (g7.BottomRow - g7.TopRow) / 2;
                    }
                }
                else if (aScrollBar == vScrollBar4)
                {
                    rowCalc = g10.BottomRow - g10.TopRow;
                }

                switch (e.KeyData)
                {
                    case Keys.PageDown:
                        aScrollBar.Value = Math.Min(aScrollBar.Value + rowCalc, aScrollBar.Maximum);
                        e.Handled = true;
                        break;

                    case Keys.PageUp:
                        aScrollBar.Value = Math.Max(aScrollBar.Value - rowCalc, aScrollBar.Minimum);
                        e.Handled = true;
                        break;
                    // Begin TT#2027 - JSmith - System out of Argument Range
                    case Keys.Right:
                        _arrowScroll = true;
                        break;
                    case Keys.Left:
                        _arrowScroll = true;
                        break;
                    // End TT#2027

                    // Begin TT#222-MD - RMatelic - Arrow keys not moving up and down in size review
                    case Keys.Up:
                    case Keys.Down:
                        break;
                    // End TT#222-MD

                    // Begin TT#199-MD - RMatelic - Column headers not moving with the cells while using arrow keys
                    default:
                        e.Handled = true;
                        break;
                    // End TT#199-MD
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                HandleException(exc);
            }
        }
        // End TT#1542
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
            _columnAdded = false;   // TT#456 - RMatelic - Add views to Size Review
			ChooseColumn();
            // Begin TT#456 - RMatelic - Add views to Size Review
            //CriteriaChanged();  // MID Track 5531: Intransit not showing in Size Review
            if (_columnAdded)
            {
                CriteriaChanged();
            }
            // End TT#456
		}
        private void mnuColumnChooser23_Click(object sender, System.EventArgs e)
		{
			ChooseColumn();
			// Begin TT#456 - RMatelic - Add views to Size Review
            //CriteriaChanged();  // MID Track 5531: Intransit not showing in Size Review
            if (_columnAdded)
            {
                CriteriaChanged();
            }
            // End TT#456

		}
        private void ChooseColumn()
		{
			AllocationWaferCoordinate wafercoord = null;	
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
						// Skip the first column; 'Store' cannot be hidden
						for (int col = 0; col < g1.Cols.Count; col++)
						{
							if (   Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture) == _lblStore
								|| Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture).Trim() == string.Empty
								|| (Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture) == _lblHeader && _trans.AnalysisOnly))
								continue;		// Skip 'Store' & extra column -  cannot be hidden

							//make a new "RowColHeader" object that will be later added to the "ColumnHeaders" arraylist.
							RowColHeader rch = new RowColHeader();

							//get the tag for the column of the current loop position.
							TagForColumn colTag = new TagForColumn();
							colTag = (TagForColumn)g1.Cols[col].UserData;

							//Assign values to the RowColHeader object.
							if (colTag.CubeWaferCoorList != null)
							{	
								wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
								rch.Name = wafercoord.Label;
							}
							else
								rch.Name = Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture);
							
							rch.IsDisplayed = colTag.IsDisplayed;

							//Add this RowColHeader object to the ArrayList.
							ColumnHeaders.Add(rch);
						}

						needsAtLeastOneCol = false;
						break;
	
					case FromGrid.g2:
						
						for (int col = 0; col < g2.Cols.Count; col++)
						{
							//make a new "RowColHeader" object that will be later added to the "ColumnHeaders" arraylist.
							RowColHeader rch = new RowColHeader();

							//get the tag for the column of the current loop position.
							TagForColumn colTag = new TagForColumn();
							colTag = (TagForColumn)g2.Cols[col].UserData;
							wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
							rch.Name = wafercoord.Label;
							rch.IsDisplayed = colTag.IsDisplayed;

							//Add this RowColHeader object to the ArrayList.
							ColumnHeaders.Add(rch);
						}
						needsAtLeastOneCol = true;
						break;	

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
							wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
							
							//Assign values to the RowColHeader object.
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
							// BEGIN MID Track #3161 = selected visible columns not remaining selected
							//ColHeaders1.Clear();
							//foreach (RowColHeader header in frm.Headers)
							//{
							//	ColHeaders1.Add(header.Name, header);
							//}
							foreach (RowColHeader header in frm.Headers)
							{
								if (ColHeaders1.Contains(header.Name))
								{
									ColHeaders1.Remove(header.Name);
								}
								ColHeaders1.Add(header.Name, header);
							}
							// END MID Track #3161 
                            ShowHideColHeaders(RightClickedFrom, ColHeaders1, true, g4);
							break;
						case FromGrid.g2:
							ColHeaders2.Clear();
							foreach (RowColHeader header in frm.Headers)
							{
								ColHeaders2.Add(header.Name, header);
							}
                            ShowHideColHeaders(RightClickedFrom, ColHeaders2, true, g5);
							SetScrollBarPosition(hScrollBar2, g2.LeftCol);
							((GridTag)g2.Tag).CurrentScrollPosition = g2.LeftCol;
							break;
						case FromGrid.g3:
							ColHeaders3.Clear();
							foreach (RowColHeader header in frm.Headers)
							{
								ColHeaders3.Add(header.Name, header);
							}
                            ShowHideColHeaders(RightClickedFrom, ColHeaders3, true, g6);
							SetScrollBarPosition(hScrollBar3, g3.LeftCol);
							((GridTag)g3.Tag).CurrentScrollPosition = g3.LeftCol;
							break;
					}
                    // Begin TT#1095 - JSmith - Size Review Sorting
                    //MiscPositioning();
                    MiscPositioning(true);
                    // End TT#1095
				}
						
				frm.Dispose();				
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        private void ShowHideColHeaders(FromGrid aFromGrid, Hashtable RowColHeaders,
            bool fromColChooser, C1FlexGrid aStoreGrid)
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
							string label = Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture);
							if ( label == _lblStore
								|| label == this._lblHeader
								|| label == " "
								|| label == string.Empty)
								//  'Store' & extra column - cannot be hidden
							{
								g1.Cols[i].Visible = true;
                                aStoreGrid.Cols[i].Visible = true;
                             	g7.Cols[i].Visible = true;
								g10.Cols[i].Visible = true;
							}	
							else
							{
								TagForColumn colTag = new TagForColumn();
								colTag = (TagForColumn)g1.Cols[i].UserData;
								if (colTag.CubeWaferCoorList != null)
								{	
									wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
									colName = wafercoord.Label;
								}
								else
									colName = label;
							 
								rch = (RowColHeader)RowColHeaders[colName];
								colTag.IsDisplayed = rch.IsDisplayed;
								if (colTag.IsDisplayed &&
									!colTag.IsBuilt &&
									wafercoord != null)
								{
									_trans.BuildWaferColumnsAdd(0,(eAllocationWaferVariable)wafercoord.Key);
									colTag.IsBuilt = true;
                                    _columnAdded = true;  // TT#456 - RMatelic - Add views to Size Review
								}
								g1.Cols[i].UserData = colTag;

								//Show/hide relevant columns.
								g1.Cols[i].Visible = rch.IsDisplayed;
                                aStoreGrid.Cols[i].Visible = rch.IsDisplayed;
								g7.Cols[i].Visible = rch.IsDisplayed;
								g10.Cols[i].Visible = rch.IsDisplayed;
							}
						}
                        if (!fromColChooser && !_exporting)
						{
                            // Begin TT#456 - RMatelic - Add views to Size Review
                            //CheckVertica1Splitter1();
                            if (_loading)
                            {
                                if (_viewRID == Include.NoRID)
                                {
                                    CheckVertica1Splitter1();
                                }
                            }
                            else
                            {
                                CheckVertica1Splitter1();
                            }
                        }   // End TT#456
						break;
					case FromGrid.g2: 
						//case FromGrid.g3:
						for (i = 0; i < g2.Cols.Count; i++)
						{
							//update the column tag for g2.
							TagForColumn colTag = new TagForColumn();
							colTag = (TagForColumn)g2.Cols[i].UserData;
							wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
							colName = wafercoord.Label;
							rch = (RowColHeader)RowColHeaders[colName];
							colTag.IsDisplayed = rch.IsDisplayed;
							if (colTag.IsDisplayed &&
								!colTag.IsBuilt)
							{
								_trans.BuildWaferColumnsAdd(1,(eAllocationWaferVariable)wafercoord.Key);
								colTag.IsBuilt = true;
                                _columnAdded = true;  //TT#456 - RMatelic - Add views to Size Review
							}
							g2.Cols[i].UserData = colTag;

							//show/hide relevent columns.
							g2.Cols[i].Visible = rch.IsDisplayed;
							aStoreGrid.Cols[i].Visible = rch.IsDisplayed;
							g8.Cols[i].Visible = rch.IsDisplayed;
							g11.Cols[i].Visible = rch.IsDisplayed;
						}
                        if (!fromColChooser && !_exporting)
						{
							CheckVertica1Splitter2();
						}
						break;
					case FromGrid.g3:
						_alColHeaders2 = new ArrayList();
						for (i = 0; i < g3.Cols.Count; i++)
						{
							//update the column tag for g3.
							TagForColumn colTag = new TagForColumn();
							colTag = (TagForColumn)g3.Cols[i].UserData;
							wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
							colName = wafercoord.Label;
							rch = (RowColHeader)RowColHeaders[colName];
							 						 
							colTag.IsDisplayed = rch.IsDisplayed;
							if (colTag.IsDisplayed &&
								!colTag.IsBuilt)
							{
								_trans.BuildWaferColumnsAdd(2,(eAllocationWaferVariable)wafercoord.Key);
								colTag.IsBuilt = true;
                                _columnAdded = true;  //TT#456 - RMatelic - Add views to Size Review
							}
							g3.Cols[i].UserData = colTag;

							//show/hide relevant columns.
							g3.Cols[i].Visible = rch.IsDisplayed;
							aStoreGrid.Cols[i].Visible = rch.IsDisplayed;
							g9.Cols[i].Visible = rch.IsDisplayed;
							g12.Cols[i].Visible = rch.IsDisplayed;

							if (rch.IsDisplayed)
							{
								_lastVisibleG3Column = i;
								if (!_alColHeaders2.Contains(colName))
									_alColHeaders2.Add(colName);
							}
						}
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void ShowHideRows()
		{
			// temporary code to hide rows; should be modified when Row Chooser is implemented 
			TagForRow rowTag;
			int i, storeCol = 0;
			string colLabel, rowLabel;
			try
			{
				for (i = 0; i < g1.Cols.Count; i++)
				{
					colLabel = Convert.ToString(g1.GetData(0, i), CultureInfo.CurrentUICulture);
					if (colLabel == _lblStore)
					{
						storeCol = i;
						break;
					}
				}
				_dispRowsPerSet = 0;
				rowLabel = Convert.ToString(g7.GetData(0, storeCol), CultureInfo.CurrentUICulture);
				for (i = 0; i < g7.Rows.Count; i++)
				{
					rowTag = (TagForRow)g7.Rows[i].UserData;
					g7.Rows[i].Visible = rowTag.IsDisplayed; 
					g8.Rows[i].Visible = rowTag.IsDisplayed;
					g9.Rows[i].Visible = rowTag.IsDisplayed;
                    // BEGIN MID Track #5309 - grid lines inconsistent
                    //if ( Convert.ToString(g7.GetData(i, storeCol), CultureInfo.CurrentUICulture) == rowLabel)
                    //	_dispRowsPerSet++;
                    if (Convert.ToString(g7.GetData(i, storeCol), CultureInfo.CurrentUICulture) == rowLabel
                         && g7.Rows[i].Visible)
                    {
                        _dispRowsPerSet++;
                    }
                }   // END MID Track #5309

				for (i = 0; i < g10.Rows.Count; i++)
				{
					rowTag = (TagForRow)g10.Rows[i].UserData;
					g10.Rows[i].Visible = rowTag.IsDisplayed; 
					g11.Rows[i].Visible = rowTag.IsDisplayed;
					g12.Rows[i].Visible = rowTag.IsDisplayed;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

		}
	
		// BEGIN MID Track #2511 - Sort not working
		private void ProcessAfterSort()
		{
			try
			{
				AssignTagsg4_g12(FromGrid.g5);  //g5
				AssignTagsg4_g12(FromGrid.g6);  //g6
				SetStoreRowsStyle();
                // Begin TT#1095 - JSmith - Size Review Sorting
                //MiscPositioning();
                MiscPositioning(false);
                // End TT#1095
				// SetGridStyles(false,true);/ BEGIN/END MID Track #4623 - Grid error after sort - move method to after SetScrollBar....
				vScrollBar2.Value = vScrollBar2.Minimum;
		
				// BEGIN MID Track #4528 - columns not aligned after sort
				SetScrollBarPosition(hScrollBar2, ((GridTag)g2.Tag).CurrentScrollPosition);
				SetScrollBarPosition(hScrollBar3, ((GridTag)g3.Tag).CurrentScrollPosition);
				// END MID Track #4528
			
				SetGridStyles(false,true); // BEGIN/END MID Track #4623 - Grid error after sort
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		
		private void SetStoreRowsStyle()
		{	
			try
			{
				for (int row = 0; row < g4.Rows.Count; row++)
				{
					ChangeRowStylesG4G5G6(row, _rowsPerGroup4);
//					for (int col = 0; col < g4.Cols.Count - 1; col++)
//					{	
//						if (g4.Cols[col].DataType == typeof(System.String)) 
//						{
//							if (Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture) == _lblStore)
//							{
//								if (g4.Rows[row].Style.Name == "Style1")
//								{
//									g4.SetCellStyle(row, col, g4.Styles["Style1Left"]);
//								}
//								else if (g4.Rows[row].Style.Name == "Style2")
//								{
//									g4.SetCellStyle(row, col, g4.Styles["Style2Left"]);
//								}
//							}
//							else
//							{
//								if (g4.Rows[row].Style.Name == "Style1")
//								{
//									g4.SetCellStyle(row, col, g4.Styles["Style1Center"]);
//								}
//								else if (g4.Rows[row].Style.Name == "Style2")
//								{
//									g4.SetCellStyle(row, col, g4.Styles["Style2Center"]);
//								}
//							}
//						}
//					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}	
		// END MID Track #2511
		private void CheckVertica1Splitter1()
		{
			//The following code is related to re-adjust the headers panel
			//so that if the number of columns displayed is less than what it
			//used to be, we want to "shrink" the panel so it doesn't take
			//up unnecesary space.
			int SplitterMaxPossiblePosition, lastDisplayedCol = 0;
			int gbxPos =  gbxSizeVar.Right;
			int colPos = 0;
			for (int j = 0; j < g1.Cols.Count; j++)
			{
				if (g1.Cols[j].Visible)
				{
					lastDisplayedCol = j;
					colPos += g1.Cols[j].Width;
					//if (colPos >= gbxPos) 
					//	break;
				}	
			}
			if (colPos < gbxPos) 
				colPos = gbxPos;
			SplitterMaxPossiblePosition = colPos  + VerticalSplitter1.Width + 3;
			if (SplitterMaxPossiblePosition <= UserSetSplitter1Position)
			{
				//move the splitter to "shrink" the Store Need panel.
               	VerticalSplitter1.SplitPosition = SplitterMaxPossiblePosition;
			}
			else if (SplitterMaxPossiblePosition > UserSetSplitter1Position)
			{
				
				if (g1.RightCol == (g1.Cols.Count - 1) || _doResetV1)
				{
					SetV1SplitPosition();
					_doResetV1 = false;
				}
				else
					VerticalSplitter1.SplitPosition = UserSetSplitter1Position;
			}

			hScrollBar1.Minimum = 0;
			if (_loading)
				hScrollBar1.Maximum = (g1.Cols[g1.Cols.Count - 1].Right - g1.Width) + BIGCHANGE;
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

			SplitterMaxPossiblePosition = g2.Cols[g2.Cols.Count - 1].Right + VerticalSplitter2.Width + 3;
			
			if (SplitterMaxPossiblePosition <= UserSetSplitter2Position)
			{
				//move the splitter to "shrink" the TOTALS panel.
				VerticalSplitter2.SplitPosition = SplitterMaxPossiblePosition;
			}
			else if (SplitterMaxPossiblePosition > UserSetSplitter2Position)
			{
				if (g2.RightCol == (g2.Cols.Count - 1))
					VerticalSplitter2.SplitPosition = SplitterMaxPossiblePosition;
				else
					VerticalSplitter2.SplitPosition = UserSetSplitter2Position;
			}
            // BEGIN MID Track #5377 - middle grid collapses after column selection
            //						could not reproduduce in deveopment; modified below as a guess 
            //if (VerticalSplitter2.SplitPosition < 0)
            //	VerticalSplitter2.SplitPosition = 0;
            if (VerticalSplitter2.SplitPosition <= 0)
            {
                SetV2SplitPosition();
            }
        }   // BEGIN MID Track #5377
		#endregion

		#region Group By Radio buttons
		private void rbHeader_CheckedChanged(object sender, System.EventArgs e)
		{
            // Begin T#456 - RMatelic - Add views to Size Review 
            //if (_loading || !rbHeader.Checked) return;  
            if (_loading || !rbHeader.Checked || _changingView)
            {
                return;
            }
            // End TT#456
			_trans.AllocationGroupBy = Convert.ToInt32(eAllocationSizeViewGroupBy.Header, CultureInfo.CurrentUICulture);
			ReloadGridData();
		}
		
		private void rbColor_CheckedChanged(object sender, System.EventArgs e)
		{
            // Begin T#456 - RMatelic - Add views to Size Review 
            //if (_loading || !rbColor.Checked) return;
            if (_loading || !rbColor.Checked || _changingView)
            {
                return;
            }
            // End TT#456
			_trans.AllocationGroupBy = Convert.ToInt32(eAllocationSizeViewGroupBy.Color, CultureInfo.CurrentUICulture);
			ReloadGridData();
		}
			
		private void rbSequential_CheckedChanged(object sender, System.EventArgs e)
		{
            // Begin T#456 - RMatelic - Add views to Size Review 
            //if (_loading) return;   
            if (_loading || _changingView)
            {
                return;
            }
            // End TT#456
			_trans.AllocationViewIsSequential = rbSequential.Checked;
			if (!rbSequential.Checked) return;
			_doResetV1 = true;
            // Begin TT#456 - RMatelic - Add Views to Size Review
            for (int col = 0; col < g1.Cols.Count; col++)
            {
                if (g1.Cols[col].Name == Convert.ToString((int)eMIDTextCode.lbl_Dimension, CultureInfo.CurrentUICulture))
                {
                    _dimensionLastPosition = g1.Cols[col].Index;
                    break;
                }
            }
            // End TT#456 
			ReloadGridData();
		}

		private void rbSizeMatrix_CheckedChanged(object sender, System.EventArgs e)
		{
            // Begin T#456 - RMatelic - Add views to Size Review 
            //if (_loading || !rbSizeMatrix.Checked) return;
            if (_loading || !rbSizeMatrix.Checked || _changingView)
            {
                return;
            }
            // End TT#456
            // Begin TT#1095 - JSmith - Size Review Sorting
            ClearSortImages();
            // End TT#1095
			_doResetV1 = true;
			ReloadGridData();
            // Begin TT#456 - RMatelic - Add Views to Size Review
            for (int col = 0; col < g1.Cols.Count; col++)
            {
                if (g1.Cols[col].Name == Convert.ToString((int)eMIDTextCode.lbl_Dimension, CultureInfo.CurrentUICulture))
                {
                    g1.Cols[col].Move(_dimensionLastPosition);
                    g4.Cols[col].Move(_dimensionLastPosition);
                    g7.Cols[col].Move(_dimensionLastPosition);
                    g10.Cols[col].Move(_dimensionLastPosition);
                    break;
                }
            }
        }   // End TT#456  

		private void rbSize_CheckedChanged(object sender, System.EventArgs e)
		{
            // Begin T#456 - RMatelic - Add views to Size Review 
            //if (_loading || !rbSize.Checked) return;
            if (_loading || !rbSize.Checked || _changingView)
            {
                return;
            }
            // End TT#456
			_trans.AllocationSecondaryGroupBy = Convert.ToInt32(eAllocationSizeView2ndGroupBy.Size, CultureInfo.CurrentUICulture);
			ReloadGridData();
		}

		private void rbVariable_CheckedChanged(object sender, System.EventArgs e)
		{
            // Begin T#456 - RMatelic - Add views to Size Review 
            //if (_loading || !rbVariable.Checked) return;
            if (_loading || !rbVariable.Checked || _changingView)
            {
                return;
            }
            // End TT#456
			_trans.AllocationSecondaryGroupBy = Convert.ToInt32(eAllocationSizeView2ndGroupBy.Variable, CultureInfo.CurrentUICulture);
			ReloadGridData();
		}

		public void ReloadGridData()
		{
			int i,j;
            try
            {
                // Begin TT#1009 - RMatelic Unhandled exception while working in size review; believed to be timing issue of interleafed events
                //     disable drop downs until this event is completed; 
                _gridRebuildInProgress = true;  
                cmbAttributeSet.Enabled = false;
                cmbStoreAttribute.Enabled = false;
                cmbFilter.Enabled = false;
                btnApply.Enabled = false;
                btnProcess.Enabled = false;
                cboAction.Enabled = false;
                // End TT#1109
                Cursor.Current = Cursors.WaitCursor;
                if (!_changingView)
                {
                    SaveCurrentColumns();                   // TT#456 - RMatelic - Add views to Size Review
                }    
				_trans.RebuildWafers();
				_wafers = _trans.AllocationWafers;
				Cursor.Current = Cursors.WaitCursor;
				//_resetV1Splitter = true;
				SetGridRedraws(false);
				FormatGrids1to12();
                SortByDefault();
				// BEGIN MID Track #3161 = selected visible columns not remaining selected
				//_setHeaderTags = true;
				_setHeaderTags = false;
				// END MID Track #3161
				AssignTag();
				SetStyles();
				SetGridStyles(true,true);
               
                // Begin TT#456 - RMatelic - Add views to Size Review
                //ApplyPreferences();
                if (_changingView)
                {
                    ApplyViewToGridLayout(Convert.ToInt32(cmbView.SelectedValue, CultureInfo.CurrentUICulture));
                }
                else
                {
                    ApplyCurrentColumns();
                    ApplyPreferences();
                }    
                // End TT#456 

				ChangePending = true;
                SetRowSplitPosition8();
                SetRowSplitPosition12();
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
				// BEGIN MID Track #2980 - can't scroll to right end of grid
				SetV1SplitPosition();
				// END MID Track #2980
				SetScrollBarPosition(hScrollBar2, i);
				SetScrollBarPosition(hScrollBar3, j);
                HilightSelectedSet();    // MID Track #5616 - wrong data showing in g8 & g9 when set is changed 
                SetGridRedraws(true);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            // Begin TT#1009 - RMatelic Unhandled exception while working in size review; believed to be timing issue of interleafed events
            //     disable drop downs until this event is completed; 
            finally
            {
                cmbAttributeSet.Enabled = true;
                if (!_trans.VelocityCriteriaExists)
                {
                    cmbStoreAttribute.Enabled = true;
                }
                if (!_trans.AnalysisOnly)
                {
                    btnApply.Enabled = (g4.Rows.Count > 0 && (_trans.DataState == eDataState.Updatable));
                }
                //BEGIN TT#6-MD-VStuart - Single Store Select
                cmbFilter.Enabled = true;  //TT#6-MD-VStuart - Single Store Select
                //cboAction_SelectionChangeCommitted(cboAction, null);
                cboAction_SelectionChangeCommitted(cboAction.ComboBox, null);
                //END TT#6-MD-VStuart - Single Store Select
                SetActionListEnabled();
                _gridRebuildInProgress = false;  
            }
            // End TT#1109
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
			// BEGIN MID Track #3107 - sorting doesn't keep Matrix grouping; for now disable sort in Matrix 
			if (rbSizeMatrix.Checked)
				return;
			// END MID Track #3107

            // Begin TT#1009 - RMatelic Unhandled exception while working in size review;  ignor grid sort while screen is rebuilding 
            if (_gridRebuildInProgress)
            {
                return;
            }
            // End TT#1009

			ArrayList al  = new ArrayList();
		    
			BuildColumnList(ref al);
			frmSortGridViews = new SortGridViews(_structSort, al); // columns to sort;
			
			frmSortGridViews.StartPosition = FormStartPosition.CenterParent;

			if (frmSortGridViews.ShowDialog() == DialogResult.OK)
			{
				_structSort = frmSortGridViews.SortInfo;
				// BEGIN MID Track #2511 - Sort not working	 
            	g5.Redraw = false;	// BEGIN MID Track #4528 - columns not aligned after sort
				g6.Redraw = false;  // END MID Track #4528  

                SortColumns(_g456DataView, g4, g5, g6); ;
				Cursor.Current = Cursors.WaitCursor;
				ProcessAfterSort();

				g5.Redraw = true; // BEGIN MID Track #4528 - columns not aligned after sort
				g6.Redraw = true; // END MID Track #4528  

				// END MID Track #2511
			}
			frmSortGridViews.Dispose();	
			
			Cursor.Current = Cursors.Default;
		}

		private void BuildColumnList(ref ArrayList _al)
		{
			AllocationWaferCoordinate wafercoord;
			try
			{
				string varName, sizeName;;
				
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
						//if ( Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture) == _lblStore)
						//	varName = _lblStore;
						if (Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture).Trim() == string.Empty)
							continue;
						else
						{
							if (colTag.CubeWaferCoorList == null)
								varName = Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture);
							else
							{
								wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
								varName = wafercoord.Label;
							}
						}
						sortData.Column1 = _lblStore;
						sortData.Column2 = varName;
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
						sizeName = _lblColor;
						wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
						varName = wafercoord.Label;
						sortData.Column1 = sizeName;
						sortData.Column2 = varName;
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
						wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.PrimarySize);
						sizeName = wafercoord.Label;
						wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
						varName = wafercoord.Label;
						sortData.Column1 = sizeName;
						sortData.Column2 = varName;
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

        private void SortColumns(DataView aG456DataView,
            C1FlexGrid aGrid4, C1FlexGrid aGrid5, C1FlexGrid aGrid6)
        {
            string sortString = String.Empty, sortdirection;
            int i;
            if (_structSort.IsSortingByDefault)
            {
                SortByDefault();
                return;
            }
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
            if (!_exporting)
            {
                ClearSortImages();
            }
            for (i = 0; i < aGrid4.Rows.Count; i++)
            {
                aGrid4.Rows[i].Visible = true;
                aGrid5.Rows[i].Visible = true;
                aGrid6.Rows[i].Visible = true;
            }

            aG456DataView.Sort = sortString;

            _isSorting = false;
        }

		private void mnuSortByDefault_Click(object sender, System.EventArgs e)
		{
			// BEGIN MID Track #2511 - Sort not working	
			//SortByDefault();
			//_setHeaderTags = false;
			//AssignTag();
			//SetStyles();	
			//SetGridStyles(false,true);
			//ApplyPreferences();
		
			Cursor.Current = Cursors.WaitCursor;
          
			g5.Redraw = false;	// BEGIN MID Track #4528 - columns not aligned after sort
			g6.Redraw = false;  // END MID Track #4528  

			SortByDefault();
			Cursor.Current = Cursors.WaitCursor;
			ProcessAfterSort();

			g5.Redraw = true;	// BEGIN MID Track #4528 - columns not aligned after sort
			g6.Redraw = true;  // END MID Track #4528  

			Cursor.Current = Cursors.Default;
			// END MID Track #2511
		}
		private void SortByDefault()
		{
			_structSort.IsSortingByDefault = true;  
			mnuSortByDefault.Checked = true;
			
			_g456DataView.Sort =_lblStore + " " + _lblStore + " ASC";
			_isSorting = false;
			
			//Erase sorting information (including pic) on g2 and g3.
			if (!_loading)
				ClearSortImages();

			vScrollBar2.Value = vScrollBar2.Minimum;
		}
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
				// BEGIN MID Track #3107 - sorting doesn't keep Matrix grouping; for now disable sort in Matrix 
				if (rbSizeMatrix.Checked)
					return;
				// END MID Track #3107

                // Begin TT#1009 - RMatelic Unhandled exception while working in size review;  ignor grid sort while screen is rebuilding 
                if (_gridRebuildInProgress)
                {
                    return;
                } 
                // End TT#1009

				C1FlexGrid grid = null;
				grid = (C1FlexGrid)sender;
				whichGrid = ((GridTag)grid.Tag).GridId;
				if ((FromGrid)whichGrid == FromGrid.g1)
					if (GridMouseCol > 0)
						return;
					else
						imageRow = 0;
				else
					imageRow =1;
	
				//get the column tag and check its current sort status
				TagForColumn ColumnTag = new TagForColumn();
				ColumnTag = (TagForColumn)grid.Cols[GridMouseCol].UserData;

				g5.Redraw = false;	// BEGIN MID Track #4528 - columns not aligned after sort
				g6.Redraw = false;  // END MID Track #4528  

				if (ColumnTag.Sort == SortEnum.none || ColumnTag.Sort == SortEnum.asc)
				{
					//Either the column hasn't been sorted or it's currently sorted in asc order.
					//We're going to sort this column in descending order.
					
					SortSingleColumn(grid, GridMouseCol, SortEnum.desc);
					Bitmap downArrow = new Bitmap(GraphicsDirectory + "\\down.gif");
					grid.SetCellImage(imageRow, GridMouseCol, downArrow);
										 
					//grid.Cols[GridMouseCol].ImageAlign = ImageAlignEnum.RightTop;
				
					ColumnTag.Sort = SortEnum.desc;
					grid.Cols[GridMouseCol].UserData = ColumnTag;
				}
				else if (ColumnTag.Sort == SortEnum.desc)
				{
					//The column is currently sorted in descending order.
					//Sort in ascending order.
				
					SortSingleColumn(grid, GridMouseCol, SortEnum.asc);
					
					Bitmap upArrow = new Bitmap(GraphicsDirectory + "\\up.gif");
					grid.SetCellImage(imageRow, GridMouseCol, upArrow);
					//grid.Cols[GridMouseCol].ImageAlign = ImageAlignEnum.LeftCenter;
				
					ColumnTag.Sort = SortEnum.asc;
					grid.Cols[GridMouseCol].UserData = ColumnTag;
				}

				Cursor.Current = Cursors.WaitCursor;
				// BEGIN MID Track #2511 - Sort not working 
				//SetStylesForG4_G12();
				//SetGridStyles(false,false);
				//Cursor.Current = Cursors.Default;
				//MiscPositioning();

				ProcessAfterSort();

				g5.Redraw = true;	// BEGIN MID Track #4528 - columns not aligned after sort
				g6.Redraw = true;	// END MID Track #4528 

				Cursor.Current = Cursors.Default;
				// END MID Track #2511
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
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

		private void SortSingleColumn(C1FlexGrid aGrid, int ColToSort, SortEnum SortDirection)
		{
			string sizeName = String.Empty, varName = String.Empty;
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
						sizeName =  _lblStore;	
						if ( Convert.ToString(g1.GetData(0, ColToSort), CultureInfo.CurrentUICulture) == _lblStore)
							varName = _lblStore;
						else
						{
							wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
							varName = wafercoord.Label;
						}
						break;
					case FromGrid.g2:
						sizeName = _lblColor;
						wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
						varName = wafercoord.Label;
						break;
					case FromGrid.g3:
						wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.PrimarySize);
						sizeName = wafercoord.Label;
						wafercoord =  GetAllocationCoordinate(colTag.CubeWaferCoorList, eAllocationCoordinateType.Variable);
						varName = wafercoord.Label;
						break;
				}
				_structSort.IsSortingByDefault = false;
				sortData.Column1 = sizeName;
				sortData.Column2 = varName;
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
                SortColumns(_g456DataView, g4, g5, g6);
				
				//ClearSortImages();
				g4.TopRow = 0;
				g5.TopRow = 0;
				g6.TopRow = 0;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
	
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
				imageRow = 1;
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

		private void DefineStyles()
		{
			try
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
				cellStyle.TextAlign = TextAlignEnum.LeftTop;
 
				cellStyle = g4.Styles.Add("Style1Center");
				cellStyle.BackColor = _theme.StoreDetailRowHeaderBackColor;
				cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
				cellStyle.Font = _theme.RowHeaderFont;
				cellStyle.Border.Style = BorderStyleEnum.None;
				cellStyle.TextAlign = TextAlignEnum.LeftTop;
					 
				cellStyle = g4.Styles.Add("Style2Center");
				cellStyle.BackColor = _theme.StoreDetailRowHeaderAlternateBackColor;
				cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
				cellStyle.Font = _theme.RowHeaderFont;
				cellStyle.Border.Style = BorderStyleEnum.None;
				cellStyle.TextAlign = TextAlignEnum.LeftTop;
					
				cellStyle = g4.Styles.Add("Style1Left");
				cellStyle.BackColor = _theme.StoreDetailRowHeaderBackColor;
				cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
				cellStyle.Font = _theme.RowHeaderFont;
				cellStyle.Border.Style = BorderStyleEnum.None;
				cellStyle.TextAlign = TextAlignEnum.LeftTop;
					 
				cellStyle = g4.Styles.Add("Style2Left");
				cellStyle.BackColor = _theme.StoreDetailRowHeaderAlternateBackColor;
				cellStyle.ForeColor = _theme.StoreDetailRowHeaderForeColor;
				cellStyle.Font = _theme.RowHeaderFont;
				cellStyle.Border.Style = BorderStyleEnum.None;
				cellStyle.TextAlign = TextAlignEnum.LeftTop;
					 
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
			
				// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
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

				cellStyle = g5.Styles.Add("NegativeEditable2Balance");
				cellStyle.BackColor = _theme.StoreDetailAlternateBackColor;
				cellStyle.ForeColor = _theme.NegativeForeColor;
				cellStyle.Font = _theme.StoreAllocationOutOfBalance;
				cellStyle.Border.Style = _theme.CellBorderStyle;
				cellStyle.Border.Color = _theme.CellBorderColor;
				// END MID Track #1511

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

				// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
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
				// END MID Track #1511

				cellStyle = g7.Styles.Add("Style1");
				cellStyle.BackColor = _theme.StoreSetRowHeaderBackColor;
				cellStyle.ForeColor = _theme.StoreSetRowHeaderForeColor;
				cellStyle.Font = _theme.RowHeaderFont;
				cellStyle.Border.Style = BorderStyleEnum.None;
				cellStyle.TextAlign = TextAlignEnum.LeftTop;
			
				cellStyle = g7.Styles.Add("Style2");
				cellStyle.BackColor = _theme.StoreSetRowHeaderAlternateBackColor;
				cellStyle.ForeColor = _theme.StoreSetRowHeaderForeColor;
				cellStyle.Font = _theme.RowHeaderFont;
				cellStyle.Border.Style = BorderStyleEnum.None;
				cellStyle.TextAlign = TextAlignEnum.LeftTop;
								
				cellStyle = g7.Styles.Add("Reverse1");
				cellStyle.BackColor = _theme.StoreSetRowHeaderForeColor;
				cellStyle.ForeColor = _theme.StoreSetRowHeaderBackColor;
				cellStyle.Font = _theme.RowHeaderFont;
				cellStyle.Border.Style = BorderStyleEnum.None;
				cellStyle.TextAlign = TextAlignEnum.LeftTop;

				cellStyle = g7.Styles.Add("Reverse2");
				cellStyle.BackColor = _theme.StoreSetRowHeaderForeColor;
				cellStyle.ForeColor = _theme.StoreSetRowHeaderAlternateBackColor;
				cellStyle.Font = _theme.RowHeaderFont;
				cellStyle.Border.Style = BorderStyleEnum.None;
				cellStyle.TextAlign = TextAlignEnum.LeftTop;
 
				cellStyle = g7.Styles.Add("Style1Left");
				cellStyle.BackColor = _theme.StoreSetRowHeaderBackColor;
				cellStyle.ForeColor = _theme.StoreSetRowHeaderForeColor;
				cellStyle.Font = _theme.RowHeaderFont;
				cellStyle.Border.Style = BorderStyleEnum.None;
				cellStyle.TextAlign = TextAlignEnum.LeftTop;

				cellStyle = g7.Styles.Add("Style2Left");
				cellStyle.BackColor = _theme.StoreSetRowHeaderAlternateBackColor;
				cellStyle.ForeColor = _theme.StoreSetRowHeaderForeColor;
				cellStyle.Font = _theme.RowHeaderFont;
				cellStyle.Border.Style = BorderStyleEnum.None;
				cellStyle.TextAlign = TextAlignEnum.LeftTop;
								
				cellStyle = g7.Styles.Add("Reverse1Left");
				cellStyle.BackColor = _theme.StoreSetRowHeaderForeColor;
				cellStyle.ForeColor = _theme.StoreSetRowHeaderBackColor;
				cellStyle.Font = _theme.RowHeaderFont;
				cellStyle.Border.Style = BorderStyleEnum.None;
				cellStyle.TextAlign = TextAlignEnum.LeftTop;

				cellStyle = g7.Styles.Add("Reverse2Left");
				cellStyle.BackColor = _theme.StoreSetRowHeaderForeColor;
				cellStyle.ForeColor = _theme.StoreSetRowHeaderAlternateBackColor;
				cellStyle.Font = _theme.RowHeaderFont;
				cellStyle.Border.Style = BorderStyleEnum.None;	
				cellStyle.TextAlign = TextAlignEnum.LeftTop;
 
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
				cellStyle.TextAlign = TextAlignEnum.LeftTop;

				cellStyle = g10.Styles.Add("Style2");
				cellStyle.BackColor = _theme.StoreTotalRowHeaderAlternateBackColor;
				cellStyle.ForeColor = _theme.StoreTotalRowHeaderForeColor;
				cellStyle.Font = _theme.RowHeaderFont;
				cellStyle.Border.Style = BorderStyleEnum.None;
				cellStyle.TextAlign = TextAlignEnum.LeftTop;

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
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetStyles() 
		{
			try
			{
				g1.Rows[0].Style = g1.Styles["Style1"];
				g1.Rows[0].TextAlign = TextAlignEnum.CenterBottom;

				g2.Rows[0].Style = g2.Styles["GroupHeader"];
				g2.Rows[0].TextAlign = TextAlignEnum.CenterBottom;
				if (g2.Rows.Count > 1)
				{
					g2.Rows[1].Style = g2.Styles["ColumnHeading"];
					g2.Rows[1].TextAlign = TextAlignEnum.CenterBottom;
				}

				g3.Rows[0].Style = g2.Styles["GroupHeader"];
				g3.Rows[0].TextAlign = TextAlignEnum.CenterBottom;
				if (g3.Rows.Count > 1)
				{
					g3.Rows[1].Style = g2.Styles["ColumnHeading"];
					g3.Rows[1].TextAlign = TextAlignEnum.CenterBottom;
				}
				Cursor.Current = Cursors.WaitCursor;
				SetStylesForG4_G12();
                // Begin TT#1095 - JSmith - Size Review Sorting
                //MiscPositioning();
                MiscPositioning(true);
                // End TT#1095

			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		
		private void SetStylesForG4_G12()
		{
			try
			{
				SetStylesG4();
				SetStylesG5();
				SetStylesG6();
				SetStylesG7();
				SetStylesG8();
				SetStylesG9();
				SetStylesG10();
				SetStylesG11();
				SetStylesG12();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void SetStylesG4()
		{
//			TagForRow rowTag;
			try
			{
				if (g4.Rows.Count == 0)
					return;
				//ChangeRowStyles(g4, _rowsPerStoreGroup);
				ChangeRowStyles(g4, _rowsPerGroup4);
				
				for (int row = 0; row < g4.Rows.Count; row++)	
				{
					//rowTag = (TagForRow)g4.Rows[row].UserData;
					//if(!rowTag.IsDisplayed)
					//	continue;
					 
					for (int col = 0; col < g4.Cols.Count - 1; col++)
					{	
						if (g4.Cols[col].DataType == typeof(System.String)) 
						{
							if (Convert.ToString(g1.GetData(0, col), CultureInfo.CurrentUICulture) == _lblStore)
							{
								if (g4.Rows[row].Style.Name == "Style1")
								{
									g4.SetCellStyle(row, col, g4.Styles["Style1Left"]);
								}
								else if (g4.Rows[row].Style.Name == "Style2")
								{
									g4.SetCellStyle(row, col, g4.Styles["Style2Left"]);
								}
							}
							else
							{
								if (g4.Rows[row].Style.Name == "Style1")
								{
									g4.SetCellStyle(row, col, g4.Styles["Style1Center"]);
								}
								else if (g4.Rows[row].Style.Name == "Style2")
								{
									g4.SetCellStyle(row, col, g4.Styles["Style2Center"]);
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
		private void SetStylesG5()
		{
			try
			{
				if (g5.Rows.Count == 0)
					return;
				//ChangeRowStyles(g5, _rowsPerStoreGroup);
				ChangeRowStyles(g5,_rowsPerGroup4);
				//SetCellStyleG5G6(g5);
				Debug.WriteLine("SetStylesG5");
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetStylesG6()
		{
			try
			{
				if (g6.Rows.Count == 0)
					return;
				//ChangeRowStyles(g6, _rowsPerStoreGroup);
				ChangeRowStyles(g6, _rowsPerGroup4);
				//SetCellStyleG5G6(g6);
				Debug.WriteLine("SetStylesG6");
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetStylesG7()
		{
			CellRange cellRange;
			try
			{
				if (g7.Rows.Count == 0)
					return;
                ChangeRowStyles(g7, _rowsPerStoreGroup);
				HilightSelectedSet();
				for (int row = 0; row < g7.Rows.Count; row++)
				{
					cellRange = g7.GetCellRange(row, 0);
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
		private void SetStylesG8()
		{
			try
			{
				if (g8.Rows.Count == 0)
					return;
				ChangeRowStyles(g8, _rowsPerStoreGroup);
				//SetCellStyle(g8);
				Debug.WriteLine("SetStylesG8");
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetStylesG9()
		{
			try
			{
				if (g9.Rows.Count == 0)
					return;
				ChangeRowStyles(g9, _rowsPerStoreGroup);
				//SetCellStyle(g9);
				Debug.WriteLine("SetStylesG9");
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetStylesG10()
		{
			try
			{
				if (g10.Rows.Count == 0)
					return;
				ChangeRowStyles(g10, _rowsPerStoreGroup);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void SetStylesG11()
		{
			try
			{
				if (g11.Rows.Count == 0)
					return;
				ChangeRowStyles(g11, _rowsPerStoreGroup);
				//SetCellStyle(g11);
				Debug.WriteLine("SetStylesG11");
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetStylesG12()
		{
			try
			{
				if (g12.Rows.Count == 0)
					return;
				ChangeRowStyles(g12, _rowsPerStoreGroup);
				//SetCellStyle(g12);
				Debug.WriteLine("SetStylesG12");
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void ChangeRowStylesG4G5G6(int aRow, int aRowsPerGroup)
		{
			try
			{ 
				switch (_theme.ViewStyle)
				{
					case StyleEnum.Plain:
					case StyleEnum.Chiseled:
					    g4.Rows[aRow].Style = g4.Styles["Style1"];
					    g5.Rows[aRow].Style = g5.Styles["Style1"];
					    g6.Rows[aRow].Style = g6.Styles["Style1"];
						break;
					case StyleEnum.AlterColors:
						 
						if (aRow % (aRowsPerGroup * 2) < aRowsPerGroup)
						{
							g4.Rows[aRow].Style = g4.Styles["Style1"];
							g5.Rows[aRow].Style = g5.Styles["Style1"];
							g6.Rows[aRow].Style = g6.Styles["Style1"];
						}
						else
						{
							g4.Rows[aRow].Style = g4.Styles["Style2"];
							g5.Rows[aRow].Style = g5.Styles["Style2"];
							g6.Rows[aRow].Style = g6.Styles["Style2"];
						}
						 
						break;
					case StyleEnum.HighlightName:
						 
							if (aRow % aRowsPerGroup == 0)
							{
								g4.Rows[aRow].Style = g4.Styles["Style1"];
								g5.Rows[aRow].Style = g5.Styles["Style1"];
								g6.Rows[aRow].Style = g6.Styles["Style1"];
							}
							else
							{
								g4.Rows[aRow].Style = g4.Styles["Style2"];
								g5.Rows[aRow].Style = g5.Styles["Style2"];
								g6.Rows[aRow].Style = g6.Styles["Style2"];
							}
						 
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void ChangeRowStyles(C1FlexGrid aGrid, int aRowsPerGroup)
		{
			int i;
			C1FlexGrid rowGrid;
		
			try
			{
				rowGrid = GetRowGrid(aGrid);
				if (aGrid.Rows.Count > 0 && aGrid.Cols.Count > 0)
				{
					switch (_theme.ViewStyle)
					{
						case StyleEnum.Plain:
						case StyleEnum.Chiseled:
							for (i = 0; i < aGrid.Rows.Count; i++)
							{
								aGrid.Rows[i].Style = aGrid.Styles["Style1"];
							}
							break;
						case StyleEnum.AlterColors:
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
							break;
						case StyleEnum.HighlightName:
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
							break;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private bool DisplayGridRow(C1FlexGrid aGrid, int aRow)
		{
			bool displayRow = true;
			TagForRow rowTag;
			try
			{
				if (aGrid == g4)
				{

				}
				else 
				{
					rowTag = (TagForRow)aGrid.Rows[aRow].UserData;
					displayRow = rowTag.IsDisplayed;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			return displayRow;
		}

		private void SetCellStyle(C1FlexGrid aGrid)
		{
			C1FlexGrid colGrid;
			colGrid = GetColumnGrid(aGrid);
			CellRange cellRange;
			bool gridIsPositiveDecimalValue;
			bool gridIsString;
			string gridCell;
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
							if (gridIsString)
							{	//Is it just editable?
								if (DataTag.IsEditable == true)
								{	
									if (aGrid.Rows[row].Style.Name == "Style1")
									{
										aGrid.SetCellStyle(row, col, aGrid.Styles["Editable1"]);
									}
									else if (aGrid.Rows[row].Style.Name == "Style2")
									{
										aGrid.SetCellStyle(row, col, aGrid.Styles["Editable2"]);
									}
								}
							}
							else
							{
								if (gridIsPositiveDecimalValue)
								{  //Is it just editable?
									if (DataTag.IsEditable == true)
									{
										if (aGrid.Rows[row].Style.Name == "Style1")
										{
											aGrid.SetCellStyle(row, col, aGrid.Styles["Editable1"]);							
										}
										else if (aGrid.Rows[row].Style.Name == "Style2")
										{
											aGrid.SetCellStyle(row, col, aGrid.Styles["Editable2"]);
										}
									}

								}
								else
								{   //Is it just negative?
									if (DataTag.IsEditable == false)
									{
										if (aGrid.Rows[row].Style.Name == "Style1")
										{
											aGrid.SetCellStyle(row, col, aGrid.Styles["Negative1"]);
										}
										else if (aGrid.Rows[row].Style.Name == "Style2")
										{
											aGrid.SetCellStyle(row, col, aGrid.Styles["Negative2"]);
										}
									}
									else
									{  //Is it both negative and editable?
										if (DataTag.IsEditable == true)
										{
											if (aGrid.Rows[row].Style.Name == "Style1")
												aGrid.SetCellStyle(row, col, aGrid.Styles["NegativeEditable1"]);
											else if (aGrid.Rows[row].Style.Name == "Style2")
												aGrid.SetCellStyle(row, col, aGrid.Styles["NegativeEditable1"]);
											
										}
									}
								}
							}
						}
					}
					else //this column is not lockable (meaning it's not editable).
						//Therefore, there's no need to check every cell tag.
					{
						for (int row = 0; row < aGrid.Rows.Count; row ++)
						{
							gridCell = aGrid[row, col].ToString();
							if (MIDMath.IsNumber(gridCell))
							{
								//Is it negative?
								if (!MIDMath.IsPositiveNumber(gridCell))
								{
									if (aGrid.Rows[row].Style.Name == "Style1")
										aGrid.SetCellStyle(row, col, aGrid.Styles["Negative1"]);
									 
									else if (aGrid.Rows[row].Style.Name == "Style2")
										aGrid.SetCellStyle(row, col, aGrid.Styles["Negative1"]);
									 
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
		private void SetCellStyleG5G6(C1FlexGrid aGrid)
		{
			CellRange cellRange;
			C1FlexGrid colGrid;
			try
			{
				colGrid = GetColumnGrid(aGrid);
				bool gridIsPositiveDecimalValue;
				bool gridIsString;
				string gridCell;
				//Loop through the entire sheet and pick out the negatives and the editables.
				for (int row = 0; row < aGrid.Rows.Count; row ++)
				{
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
									if (aGrid.Rows[row].Style.Name == "Style1")
									{
										aGrid.SetCellStyle(row, col, aGrid.Styles["Editable1"]);	
									}
									else if (aGrid.Rows[row].Style.Name == "Style2")
									{
										aGrid.SetCellStyle(row, col, aGrid.Styles["Editable2"]);	
									}
								}
							}
							else
							{
								if (gridIsPositiveDecimalValue)
								{  //Is it just editable?
									if (DataTag.IsEditable == true)
									{
										if (aGrid.Rows[row].Style.Name == "Style1")
										{	
											aGrid.SetCellStyle(row, col, aGrid.Styles["Editable1"]);	
										}
										else if (aGrid.Rows[row].Style.Name == "Style2")	
										{
											aGrid.SetCellStyle(row, col, aGrid.Styles["Editable2"]);
										}
									}
								}
								else
								{   //Is it just negative?
									if (DataTag.IsEditable == false)
									{
										if (aGrid.Rows[row].Style.Name == "Style1")
										{
											aGrid.SetCellStyle(row, col, aGrid.Styles["Negative1"]);
										}
										else if (aGrid.Rows[row].Style.Name == "Style2")
										{
											aGrid.SetCellStyle(row, col, aGrid.Styles["Negative2"]);
										}
									}
									else
									{  //Is it both negative and editable?
										if (DataTag.IsEditable == true)
										{
											if (aGrid.Rows[row].Style.Name == "Style1")
											{
												aGrid.SetCellStyle(row, col, aGrid.Styles["NegativeEditable1"]);
											}
											else if (aGrid.Rows[row].Style.Name == "Style2")
											{
												aGrid.SetCellStyle(row, col, aGrid.Styles["NegativeEditable2"]);
											}
										}
									}
								}
							}
						}							
						else //this column is not lockable (meaning it's not editable).
							//Therefore, there's no need to check every cell tag.
						{ 
							if (!gridIsString)
							{   //Is it negative?
								if (!gridIsPositiveDecimalValue)
								{
									if (aGrid.Rows[row].Style.Name == "Style1")
									{
										aGrid.SetCellStyle(row, col, aGrid.Styles["Negative1"]);
									}
									else if (aGrid.Rows[row].Style.Name == "Style2")
									{
										aGrid.SetCellStyle(row, col, aGrid.Styles["Negative2"]);
									}
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

		private C1FlexGrid GetRowGrid(C1FlexGrid aGrid)
		{
			C1FlexGrid rowGrid = null;
			int whichGrid;
			try
			{
				whichGrid = ((GridTag)aGrid.Tag).GridId;
				switch((FromGrid)whichGrid)
				{
					case FromGrid.g4:	
					case FromGrid.g5:
					case FromGrid.g6:
						rowGrid = g4;
						break;
					case FromGrid.g7:
					case FromGrid.g8:
					case FromGrid.g9:
						rowGrid = g7;
						break;
					case FromGrid.g10:
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

			_themeChanged = true;
			DefineStyles();
			SetStyles();
			_themeChanged = false;
			SetCellFormats();
			Cursor.Current = Cursors.Default;
		}
		
		private void SetGridStyles(bool aFirstLoad, bool aReload)
		{
			try
			{
				if (aReload || (!aReload && _gridLoading))
				{
					//Set the current visible page of values

					if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
					{
						SetFirstPage(g5, g4, g2, aFirstLoad);
					}
					if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
					{
						SetFirstPage(g6, g4, g3, aFirstLoad);
					}
					if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
					{
						SetFirstPage(g8, g7, g2, aFirstLoad);
					}
					if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
					{
						SetFirstPage(g9, g7, g3, aFirstLoad);
					}
					if (g11.Rows.Count > 0 && g11.Cols.Count > 0)
					{
						SetFirstPage(g11, g10, g2, aFirstLoad);
					}
					if (g12.Rows.Count > 0 && g12.Cols.Count > 0)
					{
						SetFirstPage(g12, g10, g3, aFirstLoad);
					}

					SetAllGridPages();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		public void SetAllGridPages()
		{
			try
			{
				_gridLoading = true;

				if (g11.Rows.Count > 0 && g11.Cols.Count > 0)
				{
					SetPages(g11, g10, g2);
				}
				if (g12.Rows.Count > 0 && g12.Cols.Count > 0)
				{
					SetPages(g12, g10, g3);
				}
				if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
				{
					SetPages(g8, g7, g2);
				}
				if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
				{
					SetPages(g9, g7, g3);
				}
				if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
				{
					SetPages(g5, g4, g2);
				}
				if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
				{
					SetPages(g6, g4, g3);
				}

				_gridLoading = false;

				//#if (DEBUG)
				//				_sab.ClientServerSession.Audit.Add_Msg(
				//					eMIDMessageLevel.Debug, 
				//					"Presentation Timers (milliseconds): Pre page load = " + _totalPrePageLoadTime.TotalMilliseconds +
				//					", First page load = " + _totalFirstPageLoadTime.TotalMilliseconds +
				//					", Post page load = " + _totalPostPageLoadTime.TotalMilliseconds, "PlanView");
				//
				//				_sab.ClientServerSession.Audit.Add_Msg(
				//					eMIDMessageLevel.Debug, 
				//					"Summary Timers (milliseconds): DB Read, build and load cube = " + _storeMaintCubeGroup.GetDBReadAndLoadTime() + 
				//					", Retrieve pages = " + _totalPageLoadTime.TotalMilliseconds + 
				//					", Retrieve pages - build only = " + _storeMaintCubeGroup.GetPageBuildTime() +
				//					", Load pages = " + _totalGridLoadTime.TotalMilliseconds, "PlanView");
				//
				//				_sab.ClientServerSession.Audit.Add_Msg(
				//					eMIDMessageLevel.Debug, 
				//					"Detail Timers (milliseconds):  Value-type init = " + _storeMaintCubeGroup.GetValueInitTime() +
				//					", Value-type init - calcs only = " + _storeMaintCubeGroup.GetValueInitCalcTime() +
				//					", Comparative-type init = " + _storeMaintCubeGroup.GetComparativeInitTime() +
				//					", Comparative-type init - calcs only = " + _storeMaintCubeGroup.GetComparativeInitCalcTime(), "PlanView");
				//#endif
			}
	    	catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void SetFirstPage(C1FlexGrid aGrid, C1FlexGrid aRowGrid, C1FlexGrid aColumnGrid, bool aFirstLoad)
		{
			// Clear load count flags

			try
			{
				// Calculate page retrieval size

				((GridTag)aGrid.Tag).LoadStartTopRow = aGrid.TopRow;
				((GridTag)aGrid.Tag).LoadStartLeftCol = aGrid.LeftCol;

				if (aFirstLoad)
				{
					((GridTag)aGrid.Tag).LoadStartBottomRow = Math.Min(aGrid.TopRow + ROWPAGESIZE, aGrid.Rows.Count - 1);
					((GridTag)aGrid.Tag).LoadStartRightCol = Math.Min(aGrid.LeftCol + COLPAGESIZE, aGrid.Cols.Count - 1);
				}
				else
				{
					((GridTag)aGrid.Tag).LoadStartBottomRow = aGrid.BottomRow;
					((GridTag)aGrid.Tag).LoadStartRightCol = aGrid.RightCol;
				}

				// Set page

				SetGridCellFormats(
					aGrid,
					aRowGrid,
					aColumnGrid,
					((GridTag)aGrid.Tag).LoadStartTopRow,
					((GridTag)aGrid.Tag).LoadStartLeftCol,
					((GridTag)aGrid.Tag).LoadStartBottomRow,
					((GridTag)aGrid.Tag).LoadStartRightCol);
					
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void SetPages(C1FlexGrid aGrid, C1FlexGrid aRowGrid, C1FlexGrid aColumnGrid)
		{
			int startTopRow;
			int startBottomRow;
			int startLeftCol;
			int startRightCol;
			int numRowsToRead;
			int numColsToRead;
			int rightFirstCol;
			int rightLastCol;
			int rightFirstRow;
			int rightLastRow;
			int downFirstCol;
			int downLastCol;
			int downFirstRow;
			int downLastRow;
			int leftFirstCol;
			int leftLastCol;
			int leftFirstRow;
			int leftLastRow;
			int upFirstCol;
			int upLastCol;
			int upFirstRow;
			int upLastRow;

			try
			{
				startTopRow = ((GridTag)aGrid.Tag).LoadStartTopRow;
				startBottomRow = ((GridTag)aGrid.Tag).LoadStartBottomRow;
				startLeftCol = ((GridTag)aGrid.Tag).LoadStartLeftCol;
				startRightCol = ((GridTag)aGrid.Tag).LoadStartRightCol;

				numRowsToRead = System.Math.Max(ROWPAGESIZE, startBottomRow - startTopRow);
				numColsToRead = System.Math.Max(COLPAGESIZE, startRightCol - startLeftCol);

				upLastCol = startRightCol;
				upLastRow = startTopRow - 1;
				downFirstCol = startLeftCol;
				downFirstRow = startBottomRow + 1;
				leftFirstRow = startTopRow;
				leftLastCol = startLeftCol - 1;
				rightLastRow = startBottomRow;
				rightFirstCol = startRightCol + 1;

				//Retrieve pages
				while ((upLastRow >= aGrid.Rows.Fixed && upLastCol >= aGrid.Cols.Fixed) ||
					(downFirstRow < aGrid.Rows.Count && downFirstCol < aGrid.Cols.Count) ||
					(leftLastCol >= aGrid.Cols.Fixed && leftFirstRow < aGrid.Rows.Count) ||
					(rightFirstCol < aGrid.Cols.Count && rightLastRow >= aGrid.Rows.Fixed))
				{
					upFirstCol = System.Math.Max(aGrid.Cols.Fixed, upLastCol - numColsToRead);
					downLastCol = System.Math.Min(aGrid.Cols.Count - 1, downFirstCol + numColsToRead);
					leftLastRow = System.Math.Min(aGrid.Rows.Count - 1, leftFirstRow + numRowsToRead);
					rightFirstRow = System.Math.Max(aGrid.Rows.Fixed, rightLastRow - numRowsToRead);

					while ((upLastRow >= aGrid.Rows.Fixed && upLastCol >= aGrid.Cols.Fixed) ||
						(downFirstRow < aGrid.Rows.Count && downFirstCol < aGrid.Cols.Count) ||
						(leftLastCol >= aGrid.Cols.Fixed && leftFirstRow < aGrid.Rows.Count) ||
						(rightFirstCol < aGrid.Cols.Count && rightLastRow >= aGrid.Rows.Fixed))
					{
						if (upLastRow >= aGrid.Rows.Fixed && upLastCol >= aGrid.Cols.Fixed)
						{
							upFirstRow = System.Math.Max(aGrid.Rows.Fixed, upLastRow - numRowsToRead);
							SetGridCellFormats(aGrid, aRowGrid, aColumnGrid, upFirstRow, upFirstCol, upLastRow, upLastCol);
							upLastRow = upFirstRow - 1;
						}
						if (downFirstRow < aGrid.Rows.Count && downFirstCol < aGrid.Cols.Count)
						{
							downLastRow = System.Math.Min(aGrid.Rows.Count - 1, downFirstRow + numRowsToRead);
							SetGridCellFormats(aGrid, aRowGrid, aColumnGrid, downFirstRow, downFirstCol, downLastRow, downLastCol);
							downFirstRow = downLastRow + 1;
						}
						if (leftLastCol >= aGrid.Cols.Fixed && leftFirstRow < aGrid.Rows.Count)
						{
							leftFirstCol = System.Math.Max(aGrid.Cols.Fixed, leftLastCol - numColsToRead);
							SetGridCellFormats(aGrid, aRowGrid, aColumnGrid, leftFirstRow, leftFirstCol, leftLastRow, leftLastCol);
							leftLastCol = leftFirstCol - 1;
						}

						if (rightFirstCol < aGrid.Cols.Count && rightLastRow >= aGrid.Rows.Fixed)
						{
							rightLastCol = System.Math.Min(aGrid.Cols.Count - 1, rightFirstCol + numColsToRead);
							SetGridCellFormats(aGrid, aRowGrid, aColumnGrid, rightFirstRow, rightFirstCol, rightLastRow, rightLastCol);
							rightFirstCol = rightLastCol + 1;
						}
					}

					upLastCol = upFirstCol - 1;
					upLastRow = startTopRow - 1;
					downFirstCol = downLastCol + 1;
					downFirstRow = startBottomRow + 1;
					leftFirstRow = leftLastRow + 1;
					leftLastCol = startLeftCol - 1;
					rightLastRow = rightFirstRow - 1;
					rightFirstCol = startRightCol + 1;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SetCellFormats()
		{
			try
			{
				//Set the current visible page formats

				if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
				{
					SetGridCellFormats(g5, g4, g2, g5.TopRow, g5.LeftCol, g5.BottomRow, g5.RightCol);
				}
				if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
				{
					SetGridCellFormats(g6, g4, g3, g6.TopRow, g6.LeftCol, g6.BottomRow, g6.RightCol);
				}
				if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
				{
					SetGridCellFormats(g8, g7, g2, g8.TopRow, g8.LeftCol, g8.BottomRow, g8.RightCol);
				}
				if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
				{
					SetGridCellFormats(g9, g7, g3, g9.TopRow, g9.LeftCol, g9.BottomRow, g9.RightCol);
				}
				if (g11.Rows.Count > 0 && g11.Cols.Count > 0)
				{
					SetGridCellFormats(g11, g10, g2, g11.TopRow, g11.LeftCol, g11.BottomRow, g11.RightCol);
				}
				if (g12.Rows.Count > 0 && g12.Cols.Count > 0)
				{
					SetGridCellFormats(g12, g10, g3, g12.TopRow, g12.LeftCol, g12.BottomRow, g12.RightCol);
				}

				SetAllCellFormats();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void SetAllCellFormats()
		{
			try
			{
				if (g5.Rows.Count > 0 && g5.Cols.Count > 0)
				{
					SetGridCellFormats(g5, g4, g2, g5.Rows.Fixed, g5.Cols.Fixed, g5.Rows.Count - 1, g5.Cols.Count - 1);
				}
				if (g6.Rows.Count > 0 && g6.Cols.Count > 0)
				{
					SetGridCellFormats(g6, g4, g3, g6.Rows.Fixed, g6.Cols.Fixed, g6.Rows.Count - 1, g6.Cols.Count - 1);
				}
				if (g8.Rows.Count > 0 && g8.Cols.Count > 0)
				{
					SetGridCellFormats(g8, g7, g2, g8.Rows.Fixed, g8.Cols.Fixed, g8.Rows.Count - 1, g8.Cols.Count - 1);
				}
				if (g9.Rows.Count > 0 && g9.Cols.Count > 0)
				{
					SetGridCellFormats(g9, g7, g3, g9.Rows.Fixed, g9.Cols.Fixed, g9.Rows.Count - 1, g9.Cols.Count - 1);
				}
				if (g11.Rows.Count > 0 && g11.Cols.Count > 0)
				{
					SetGridCellFormats(g11, g10, g2, g11.Rows.Fixed, g11.Cols.Fixed, g11.Rows.Count - 1, g11.Cols.Count - 1);
				}
				if (g12.Rows.Count > 0 && g12.Cols.Count > 0)
				{
					SetGridCellFormats(g12, g10, g3, g12.Rows.Fixed, g12.Cols.Fixed, g12.Rows.Count - 1, g12.Cols.Count - 1);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void SetGridCellFormats(
			C1FlexGrid aGrid,
			C1FlexGrid aRowGrid,
			C1FlexGrid aColumnGrid,
			int aStartRow,
			int aStartCol,
			int aStopRow,
			int aStopCol)
		{
			int row, col;
			
			try
			{
				if (aStartRow <= aStopRow && aStartCol <= aStopCol)
				{
					for (row = aStartRow; row <= aStopRow; row++)
					{
						//if (aRowGrid == g4)
						//{
						//	TagForRow rowTag = (TagForRow)aRowGrid.Rows[row].UserData;
						//	if (!rowTag.IsDisplayed)
						//		continue;
						//}
						for (col = aStartCol; col <= aStopCol; col++)
						{
							ChangeCellStyles(aGrid, aColumnGrid, row, col);
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// begin MID Track 4708 Size Performance Slow
		private string _lastCellStyleName = string.Empty;
		private C1.Win.C1FlexGrid.CellStyle _lastCellGridStyle;
		private C1FlexGrid _lastGrid = null;
		private void SetGridCellStyle(C1FlexGrid aGrid, int aRow, int aCol, string aCellStyleName)
		{
            // BEGIN MID Track #5617 - null reference error 
            //     added try...catch; couldn't determine anything as null in the catch so ignore for now
            //                         
            try
            {
                if (_lastGrid == null
                    || _lastGrid != aGrid
                    || _lastCellStyleName != aCellStyleName)
                {
                    _lastCellStyleName = aCellStyleName;
                    _lastCellGridStyle = aGrid.Styles[aCellStyleName];
                    _lastGrid = aGrid;
                }
                aGrid.SetCellStyle(aRow, aCol, _lastCellGridStyle);
            }
            catch
            {
                //throw;
            }
        }	// END MID Track #5617
		// end MID Track 4708 Size Performance Slow

        private void ChangeCellStyles(C1FlexGrid aGrid, C1FlexGrid aColumnGrid, int aRow, int aCol)
		{
			CellRange cellRange;
			bool gridIsPositiveDecimalValue;
			bool gridIsString;
			string gridCell;
			try
			{
                //Begin TT#1319-MD -jsobek -Allocation>Review>Select>Size and Size Analysis after filling in data and select OK receive a system argument out of range exception
                if (aCol > aColumnGrid.Cols.Count || aCol == -1)
                {
                    return;
                }
                //End TT#1319-MD -jsobek -Allocation>Review>Select>Size and Size Analysis after filling in data and select OK receive a system argument out of range exception
				TagForColumn ColumnTag = (TagForColumn)aColumnGrid.Cols[aCol].UserData;

				//if (ColumnTag.IsLockable == true) //Lockable means some cells are editable.
					//We need to check every cell in this column to see whether
					//it's just editable, or just negative, or both.
					//
					//
					// RonM -  When there is more than 1 header, the top rows of the middle grids
					//         are Total rows the are not editable, and the ColumnTag.IsLockable
					//         is set to false, but that does not apply to every row in this case 
					//         so we still need to check each cell  
				
				if (ColumnTag.IsLockable == true
                    // BEGIN TT#1401 - AGallagher - VSW
                   	//|| (_headerList.Count > 1 && (aGrid == g5 || aGrid == g8 || aGrid == g11) ) )
                    || (_headerList.Count > 0 && (aGrid == g5 || aGrid == g6 || aGrid == g8 || aGrid == g11)))
                    // END TT#1401 - AGallagher - VSW
				{
					
					cellRange = aGrid.GetCellRange(aRow, aCol);
					TagForGridData DataTag = (TagForGridData)cellRange.UserData;
					gridCell = aGrid[aRow, aCol].ToString();
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
					if (gridIsString)
					{	//Is it just editable?
						if (DataTag.IsEditable == true)
						{	
							// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
//							if (aGrid.Rows[aRow].Style.Name == "Style1")
//								aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable1"]);
//							else if (aGrid.Rows[aRow].Style.Name == "Style2")
//								aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable2"]);
							if (aGrid.Rows[aRow].Style.Name == "Style1")
							{
								if (DataTag.IsOutOfBalance)
								{
									// begin MID track 4708 Size performance Slow
									//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable1Balance"]);
									SetGridCellStyle(aGrid, aRow, aCol, "Editable1Balance");
									// end MID track 4708 Size performance Slow
								}
								else
								{
									// begin MID track 4708 Size Performance Slow
									//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable1"]);
									SetGridCellStyle(aGrid, aRow, aCol, "Editable1");
									// end MID Track 4708 Size performance Slow
								}
							}
							else if (aGrid.Rows[aRow].Style.Name == "Style2")
							{
								if (DataTag.IsOutOfBalance)
								{
									// begin MID track 4708 Size Performance slow
									//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable2Balance"]);
									SetGridCellStyle(aGrid, aRow, aCol, "Editable2Balance");
									// end MID Track 4708 Size Performance slow
								}
								else
								{
									// begin MID track 4708 Size Performance slow
									//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable2"]);
									SetGridCellStyle(aGrid, aRow, aCol, "Editable2");
									// end MID Track 4708 Size Performance slow
								}
							}
							// END MID Track #1511
						}
                        // BEGIN MID Track #5309 - Grid lines inconsistent
                        else
                        {
                            string style = aGrid.Rows[aRow].Style.Name;
                            SetGridCellStyle(aGrid, aRow, aCol, style);
                        }
                    }   // END MID Track #5309
					else
					{
						if (gridIsPositiveDecimalValue)
						{  //Is it just editable?
							if (DataTag.IsEditable == true)
							{
    							// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
								// if (aGrid.Rows[aRow].Style.Name == "Style1")
								//  	aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable1"]);
								// else if (aGrid.Rows[aRow].Style.Name == "Style2")
								//		aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable2"]);
								if (aGrid.Rows[aRow].Style.Name == "Style1")
								{
									if (DataTag.IsOutOfBalance)
									{
										// begin MID Track 4708 Size Performance Slow
										//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable1Balance"]);
										SetGridCellStyle(aGrid, aRow, aCol, "Editable1Balance");
										// end MID track 4708 Size Performance Slow
									}
									else
									{
                                        // begin MID Track 4708 Size Performance Slow
										//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable1"]);
										SetGridCellStyle(aGrid, aRow, aCol, "Editable1");
										// end MID Track 4708 Size Performance Slow
									}
								}
								else if (aGrid.Rows[aRow].Style.Name == "Style2")
								{
									if (DataTag.IsOutOfBalance)
									{
										// begin MID track 4708 Size Performance Slow
										//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable2Balance"]);
										SetGridCellStyle(aGrid, aRow, aCol, "Editable2Balance");
										// end MID Track 4708 Size Performance Slow
									}
									else
									{
										// begin MID track 4708 Size Performance Slow
										//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Editable2"]);
										SetGridCellStyle(aGrid, aRow, aCol, "Editable2");
										// end MID track 4708 Size Performance Slow
									}
								}
								// END MID Track #1511
							}
							// BEGIN MID Track #5309 - Grid lines inconsistent
							else	
							{
								string style = aGrid.Rows[aRow].Style.Name;
								SetGridCellStyle(aGrid, aRow, aCol, style);
							}		
						}	// END MID Track #5309
						else
						{   //Is it just negative?
							if (DataTag.IsEditable == false)
							{
								// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
								// if (aGrid.Rows[aRow].Style.Name == "Style1")
								//  	aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Negative1"]);
								// else if (aGrid.Rows[aRow].Style.Name == "Style2")
								//		aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Negative2"]);
								if (aGrid.Rows[aRow].Style.Name == "Style1")
								{
									if (DataTag.IsOutOfBalance)
									{
										// begin MID Track 4708 Size Performance Slow
										//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Negative1Balance"]);
										SetGridCellStyle(aGrid, aRow, aCol, "Negative1Balance");
										// end MID Track 4708 Size Performance Slow
									}
									else
									{
										// begin MID Track 4708 Size Performance Slow
										//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Negative1"]);
										SetGridCellStyle(aGrid, aRow, aCol, "Negative1");
										// end MID Track 4708 Size Performance Slow
									}
								}
								else if (aGrid.Rows[aRow].Style.Name == "Style2")
								{
									if (DataTag.IsOutOfBalance)
									{
										// begin MID track 4708 Size Performanc Slow
										//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Negative2Balance"]);
										SetGridCellStyle(aGrid, aRow, aCol, "Negative2Balance");
										// end MID Track 4708 Size Performance Slow
									}
									else
									{
										// begin MID Track 4708 Size Performance Slow
										//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Negative2"]);
										SetGridCellStyle(aGrid, aRow, aCol, "Negative2");
										// end MID track 4708 Size Performance Slow
									}
								}
								// END MID Track #1511
							}
							else
							{  //Is it both negative and editable?
								if (DataTag.IsEditable == true)
								{
									// BEGIN MID Track #1511 Highlight stores whose allocation is out of balance
									// if (aGrid.Rows[aRow].Style.Name == "Style1")
									//  	aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["NegativeEditable1"]);
									// else if (aGrid.Rows[aRow].Style.Name == "Style2")
									//		aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["NegativeEditable2"]);
									if (aGrid.Rows[aRow].Style.Name == "Style1")
									{
										if (DataTag.IsOutOfBalance)
										{
											// begin MID track 4708 Size Performance Slow
											//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["NegativeEditable1Balance"]);
											SetGridCellStyle(aGrid, aRow, aCol, "NegativeEditable1Balance");
											// end MID Track 4708 Size Performance Slow
										}
										else
										{
											// begin MID track 4708 Size performanc Slow
											//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["NegativeEditable1"]);
											SetGridCellStyle(aGrid, aRow, aCol, "NegativeEditable1");
											// end MID Track 4708 Size Performance Slow
										}
									}
									else if (aGrid.Rows[aRow].Style.Name == "Style2")
									{
										if (DataTag.IsOutOfBalance)
										{
											// begin MID track 4708 Size Performance Slow
											//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["NegativeEditable2Balance"]);
											SetGridCellStyle(aGrid, aRow, aCol, "NegativeEditable2Balance");
											// end MID track 4708 Size performance Slow
										}
										else
										{
											// begin MID track 4708 Size Performance Slow
											//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["NegativeEditable2"]);
											SetGridCellStyle(aGrid, aRow, aCol, "NegativeEditable2");
											// end MID track 4708 Size Performance Slow
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
					gridCell = aGrid[aRow, aCol].ToString();
					if (MIDMath.IsNumber(gridCell))
					{
						//Is it negative?
						if (!MIDMath.IsPositiveNumber(gridCell))
						{
							if (aGrid.Rows[aRow].Style.Name == "Style1")
								// begin MID Track 4708 Size Performance Slow
								//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Negative1"]);
							{
								SetGridCellStyle(aGrid, aRow, aCol, "Negative1");
							}
								// end MID Track 4708 Size performance Slow
							else if (aGrid.Rows[aRow].Style.Name == "Style2")
								// begin MID Track 4708 Size Performance Slow
								//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Negative2"]);
							{
								SetGridCellStyle(aGrid, aRow, aCol, "Negative2");
							}
							// end MID Track 4708 Size Performanc Slow
						}
						// BEGIN MID Track #3112 - positive/negative colors not working when sorting 
						else
						{
							if (aGrid.Rows[aRow].Style.Name == "Style1")
								// begin MID track 4708 Size performance Slow
								//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Style1"]);
							{
								SetGridCellStyle(aGrid, aRow, aCol, "Style1");
							}
								// end MID Track 4708 Size Performance Slow
							else if (aGrid.Rows[aRow].Style.Name == "Style2")
								// begin MID Track 4708 Size Performance Slow
								//aGrid.SetCellStyle(aRow, aCol, aGrid.Styles["Style2"]);
							{
								SetGridCellStyle(aGrid, aRow, aCol, "Style2");
							}
							// end MID Track 4708 Size Performance Slow
						}
						// END MID Track #3112
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

		}	

		private void g3_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            if (!((C1FlexGrid)sender).Redraw)
            {
                return;
            }

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

					
				if ( (e.Col < g3.Cols.Count - 1) && Convert.ToString(g3.GetData(0, e.Col)) !=
					Convert.ToString(g3.GetData(0, e.Col+1)) )
					g.FillRectangle(_theme.ColumnGroupDividerBrush, rec);
			}
		}
		
		private void g4_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            if (!((C1FlexGrid)sender).Redraw)
            {
                return;
            }

			if (g4.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + _rowsPerStoreGroup)%_rowsPerStoreGroup == 0) 
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
				((e.Row + 1 )%(_rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 +_rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row  + 1)%(_rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + _rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}
		}		
		private void g5_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            if (!((C1FlexGrid)sender).Redraw)
            {
                return;
            }

			if (g5.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + _rowsPerStoreGroup)%_rowsPerStoreGroup == 0) 
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
				((e.Row + 1 )%(_rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + _rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 )%(_rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + _rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}
		}
		private void g6_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            if (!((C1FlexGrid)sender).Redraw)
            {
                return;
            }

			if (g6.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + _rowsPerStoreGroup)%_rowsPerStoreGroup == 0) 
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
				((e.Row + 1 )%(_rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + _rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 )%(_rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + _rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0)
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
				
				if ( (e.Col < g3.Cols.Count - 1) && Convert.ToString(g3.GetData(0, e.Col)) !=
					Convert.ToString(g3.GetData(0, e.Col+1)) )
					g.FillRectangle(_theme.ColumnGroupDividerBrush, rec);
			}
		}
		private void g7_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            if (!((C1FlexGrid)sender).Redraw)
            {
                return;
            }

			if (g7.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + _rowsPerStoreGroup)%_rowsPerStoreGroup == 0) 
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
				((e.Row + 1 )%(_rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + _rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 )%(_rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + _rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}
		}
		private void g8_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            if (!((C1FlexGrid)sender).Redraw)
            {
                return;
            }

			if (g8.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();
            DrawRowBorders(_rowsPerStoreGroup, e);
		}

		private void g9_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            if (!((C1FlexGrid)sender).Redraw)
            {
                return;
            }

			if (g9.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();
            DrawRowBorders(_rowsPerStoreGroup, e);
			
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

				if ( (e.Col < g3.Cols.Count - 1) && Convert.ToString(g3.GetData(0, e.Col)) !=
					Convert.ToString(g3.GetData(0, e.Col+1)) )
					g.FillRectangle(_theme.ColumnGroupDividerBrush, rec);
			}
		}
		private void g10_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            if (!((C1FlexGrid)sender).Redraw)
            {
                return;
            }

			if (g10.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + _rowsPerStoreGroup)%_rowsPerStoreGroup == 0) 
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
				((e.Row + 1 )%(_rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + _rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 )%(_rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + _rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}
		}
		private void g11_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            if (!((C1FlexGrid)sender).Redraw)
            {
                return;
            }

			if (g11.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + _rowsPerStoreGroup)%_rowsPerStoreGroup == 0) 
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
				((e.Row + 1 )%(_rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + _rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 )%(_rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1 + _rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0)
				{
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				}
			}
		}
		private void g12_OwnerDrawCell(object sender, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
		{
            // added to get around Component One problem
            if (!((C1FlexGrid)sender).Redraw)
            {
                return;
            }

			if (g12.GetCellStyle(e.Row, e.Col) == null) return;

			e.DrawCell();

			if ((_theme.ViewStyle == StyleEnum.Plain || 
				_theme.ViewStyle == StyleEnum.AlterColors ||
				_theme.ViewStyle == StyleEnum.HighlightName) && 
				_theme.DisplayRowGroupDivider == true &&
				(e.Row + 1 + _rowsPerStoreGroup)%_rowsPerStoreGroup == 0) 
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
				((e.Row + 1 )%(_rowsPerStoreGroup*2) == 0 ||
				(e.Row + 1 + _rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				if ((e.Row + 1 )%(_rowsPerStoreGroup*2) == 0)
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else if ((e.Row + 1  + _rowsPerStoreGroup)%(_rowsPerStoreGroup * 2) == 0)
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

				if ( (e.Col < g3.Cols.Count - 1) && Convert.ToString(g3.GetData(0, e.Col)) !=
					Convert.ToString(g3.GetData(0, e.Col+1)) )
					g.FillRectangle(_theme.ColumnGroupDividerBrush, rec);
			}
		}

        private void DrawRowBorders(int aRowsPerGroup, C1.Win.C1FlexGrid.OwnerDrawCellEventArgs e)
        {
            Rectangle rectangle;
            Graphics graphics;
            System.Drawing.Printing.Margins margins;

            try
            {
                if ((_theme.ViewStyle == StyleEnum.Plain ||
                     _theme.ViewStyle == StyleEnum.AlterColors ||
                     _theme.ViewStyle == StyleEnum.HighlightName) &&
                     _theme.DisplayRowGroupDivider == true &&
                    ((e.Row / 2) + 1 + aRowsPerGroup) % aRowsPerGroup == 0)
                {
                    graphics = e.Graphics;
                    margins = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

                    rectangle = e.Bounds;
                    rectangle.Y = rectangle.Bottom - margins.Bottom;
                    rectangle.Height = margins.Bottom;

                    graphics.FillRectangle(_theme.RowGroupDividerBrush, rectangle);
                }
                else if (_theme.ViewStyle == StyleEnum.Chiseled &&
                    (((e.Row / 2) + 1) % (aRowsPerGroup * 2) == 0 ||
                     ((e.Row / 2) + 1 + aRowsPerGroup) % (aRowsPerGroup * 2) == 0))
                {
                    graphics = e.Graphics;
                    margins = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

                    rectangle = e.Bounds;
                    rectangle.Y = rectangle.Bottom - margins.Bottom;
                    rectangle.Height = margins.Bottom;

                    if (((e.Row / 2) + 1) % (aRowsPerGroup * 2) == 0)
                    {
                        graphics.FillRectangle(_theme.ChiselLowerBrush, rectangle);
                    }
                    else if (((e.Row / 2) + 1 + aRowsPerGroup) % (aRowsPerGroup * 2) == 0)
                    {
                        graphics.FillRectangle(_theme.ChiselUpperBrush, rectangle);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
		#endregion

        //BEGIN TT#6-MD-VStuart - Single Store Select
        //private void cboAction_SelectionChangeCommitted(object sender, System.EventArgs e)
        //{
        //    int val = Convert.ToInt32(cboAction.SelectedValue, CultureInfo.CurrentUICulture);
        //    btnProcess.Enabled = val != 0;
        //}
        //END TT#6-MD-VStuart - Single Store Select

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
                        if (_trans.SizeView != null)
                        {
                            Close();
                        }
                    }
                    else
                    {
                        // end TT#241 - MD - JEllis - Header Enqueue Process

                        _trans.UpdateAllocationViewSelectionHeaders();
                        _trans.NewCriteriaHeaderList();
                        _headerList = (AllocationHeaderProfileList)_trans.GetMasterProfileList(eProfileType.AllocationHeader);
                        UpdateAllocationWorkspace();
                        if (action == (int)eAllocationActionType.BackoutAllocation)
                            _trans.RebuildWafers();

                        UpdateOtherViews();
                        if (!FormIsClosing)
                            CriteriaChanged();

                        //Cursor.Current = Cursors.WaitCursor;
                        //UpdateAllocationWorkspace();
                        Cursor.Current = Cursors.Default;
                    }  // TT#241 - MD - JEllis - Header Enqueue Process
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
                        errorMessage = MIDText.GetText(eMIDTextCode.msg_al_CannotReleaseDummy) + ahp.HeaderID;
                    else
                        errorMessage = MIDText.GetText(eMIDTextCode.msg_al_HeaderTypeCanNotBeReleased) + ": " + ahp.HeaderID;

                    MessageBox.Show(errorMessage, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                if (headerStatus != eHeaderAllocationStatus.AllInBalance
                                    && headerStatus != eHeaderAllocationStatus.AllocatedInBalance)
                                    okToProcess = false;
                            }
                            break;
                    }
                    if (!okToProcess)
                    {
                        errorMessage = string.Format
                            (MIDText.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction),
                            MIDText.GetTextOnly((int)headerStatus));                   // TT#3225 - AnF - Jellis - Size VSW Onhand Out of Synce with Color
                            //MIDText.GetTextOnly((int)ahp.HeaderAllocationStatus) );  // TT#3225 - AnF - Jellis - Size VSW Onhand Out of Synce with Color

                        MessageBox.Show(errorMessage, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
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

                        //    MessageBox.Show(errorMessage, _thisTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                if (aAction == eAllocationActionType.BackoutAllocation
                    || aAction == eAllocationActionType.BackoutSizeAllocation
                    || aAction == eAllocationActionType.BackoutSizeIntransit
                    || aAction == eAllocationActionType.BackoutStyleIntransit
                    || aAction == eAllocationActionType.Reset)
                {
                    errorMessage = string.Format(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ActionWarning),
                        MIDText.GetTextOnly((int)aAction));

                    DialogResult diagResult = MessageBox.Show(errorMessage, _thisTitle,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
		

        ////BEGIN TT#6-MD-VStuart - Single Store Select
        ////private void cmbAttributeSet_SelectionChangeCommitted(object sender, System.EventArgs e)
        ////{
        ////    if (!_loading)
        ////    {
        ////        // Begin Development TT#8 - JSmith - Hold qty in last set entered or force Apply before changing Attribute set
        ////        if (_applyPending &&
        ////            !_applyPendingMsgDisplayed)
        ////        {
        ////            _applyPendingMsgDisplayed = true;
        ////            if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ApplyPendingChanges), _thisTitle,
        ////                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        ////            {
        ////                btnApply_Click(this, new EventArgs());
        ////            }
        ////            else
        ////            {
        ////                this._allocationWaferCellChangeList = new AllocationWaferCellChangeList();
        ////            }
        ////        }
        ////        // End Development TT#8
        ////        _trans.AllocationStoreGroupLevel = Convert.ToInt32(cmbAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
        ////        _resetV1Splitter = true;
        ////        CriteriaChanged();
        ////        // BEGIN MID Track #2938 - set totals wrong. For some reason, grids g8 and g9 are not
        ////        // scrolling to the correct row when the set is changed, even though they appear to be set 
        ////        // correctly in HilightSelectedSet(). Added the re-execution of HilightSelectedSet() 
        ////        // and adjust the rows again. 
        ////        HilightSelectedSet();
        ////        // END MID Track #2938
        ////    }
        ////}   
        ////END TT#6-MD-VStuart - Single Store Select

        //BEGIN TT#6-MD-VStuart - Single Store Select
        private void cmbAttributeSet_ScreenUpdate(object sender, System.EventArgs e)
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
                _trans.AllocationStoreGroupLevel = Convert.ToInt32(cmbAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                _resetV1Splitter = true;
                CriteriaChanged();
                // BEGIN MID Track #2938 - set totals wrong. For some reason, grids g8 and g9 are not
                // scrolling to the correct row when the set is changed, even though they appear to be set 
                // correctly in HilightSelectedSet(). Added the re-execution of HilightSelectedSet() 
                // and adjust the rows again. 
                HilightSelectedSet();
                // END MID Track #2938
            }
        }
        //END TT#6-MD-VStuart - Single Store Select

		private void HilightSelectedSet()
		{	
			CellRange cellRange;
			string selSet;
			int  i, foundrow = 0;
			bool invalidStyle = false;
			bool firstRowFound = false;
			selSet = cmbAttributeSet.Text;
			for (i = 0; i < g7.Rows.Count; i++) 
			{
				cellRange = g7.GetCellRange(i, 0, i, g7.Cols.Count - 1);
				if (Convert.ToString(g7.GetData(i, 0), CultureInfo.CurrentUICulture) == selSet) 
				{
					if (!firstRowFound)
					{
						firstRowFound = true;
						foundrow = i;
					}
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

			vScrollBar3.Value = System.Math.Min((foundrow / _rowsPerGroup7), vScrollBar3.Maximum - vScrollBar3.LargeChange + 1);

//			g8.TopRow = g7.TopRow;
//			g9.TopRow = g7.TopRow;
            // BEGIN MID Track #5616 - wrong data showing in g8 & g9 when set is changed 
            //g7.ShowCell(foundrow,0);
            //g8.ShowCell(foundrow,0);
            //g9.ShowCell(foundrow,0);

            _isScrolling = true;
            g7.ShowCell(foundrow, 0);

            for (i = 0; i < g8.Cols.Count; i++)
            {
                if (g8.Cols[i].Visible)
                    break;
            }

            int j;
            for (j = 0; j < g9.Cols.Count; j++)
            {
                if (g9.Cols[j].Visible)
                    break;
            }
            if (i < g8.Cols.Count)      // TT#456 - RMatelic - Add views to Size Review: add if... 
            {
                g8.ShowCell(foundrow, i);
            }
            if (j < g9.Cols.Count)      // TT#456 - RMatelic - Add views to Size Review: add if...
            {
                g9.ShowCell(foundrow, j);
            }
            _isScrolling = false;
            // END MID Track #5616	
		}

		private void btnApply_Click(object sender, System.EventArgs e)
		{
			try
			{
                _trans.SetAllocationCellValue(this._allocationWaferCellChangeList); // TT#59 Implement Temp Locks
				SaveCurrentSettings();
				if (!FormIsClosing)
				{
					int i = g4.TopRow;
					SetGridRedraws(false);
					CriteriaChanged();
					g4.TopRow = i;
					g5.TopRow = i;
					g6.TopRow = i;
					SetGridRedraws(true);
				}
				UpdateOtherViews();
			}	
			catch (Exception ex)
			{
				HandleException(ex);
			}
            // begin TT#59 Implement Temp Locks
            finally
            {
                this._allocationWaferCellChangeList = new AllocationWaferCellChangeList();
            }
            // end TT#59 Implement Temp Locks
		}

         // begin TT#241 - MD Jellis - Header Enqueue Process
        private void CloseOtherViews()
        {
            MIDRetail.Windows.SummaryView frmSummaryView;
            MIDRetail.Windows.StyleView frmStyleView;
            MIDRetail.Windows.AssortmentView frmAssortmentView;
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
                if (_trans.StyleView != null)
                {
                    frmStyleView = (MIDRetail.Windows.StyleView)_trans.StyleView;
                    if (ErrorFound)
                    {
                        frmStyleView.ErrorFound = true;
                    }
                    frmStyleView.Close();
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
                    MIDRetail.Windows.frmVelocityMethod frmVelocityMethod = (MIDRetail.Windows.frmVelocityMethod)_trans.VelocityWindow;
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
        // end TT#241 - MD Jellis - Header Enqueue Process
        public void UpdateOtherViews() 
        {
            MIDRetail.Windows.SummaryView frmSummaryView;
            // BEGIN MID Track #2507 - change in size view not updating style view
            MIDRetail.Windows.StyleView frmStyleView;
            MIDRetail.Windows.AssortmentView frmAssortmentView;
            MIDRetail.Windows.AssortmentView frmGroupView;
            // END MID Track #2507
            try
            {
                SaveCurrentSettings();
                if (_trans.SummaryView != null)
                {
                    frmSummaryView = (MIDRetail.Windows.SummaryView)_trans.SummaryView;
                    frmSummaryView.UpdateData();
                }
                // BEGIN MID Track #2507
                if (_trans.StyleView != null)
                {
                    frmStyleView = (MIDRetail.Windows.StyleView)_trans.StyleView;
                    frmStyleView.UpdateData();
                }
                if (_trans.AssortmentView != null)
                {
                    // begin TT#488 - MD - Jellis - Group Allocation
                    //frmAssortmentView = (MIDRetail.Windows.AssortmentView)_trans.AssortmentView; 
                    //frmAssortmentView.UpdateData(true);
                    frmAssortmentView = _trans.AssortmentView as AssortmentView;
                    if (frmAssortmentView != null)
                    {
                        // Begin TT#1514-MD - stodd - Size Review and Placeholder QTY do not match after processing Action Balance Size Bilaterally
                        //frmAssortmentView.UpdateData(true);
                        frmAssortmentView.UpdateDataAndRefresh(true);
                        // End TT#1514-MD - stodd - Size Review and Placeholder QTY do not match after processing Action Balance Size Bilaterally
                    }
                    else
                    {
                        frmGroupView = _trans.AssortmentView as AssortmentView;
                        frmGroupView.UpdateData(true);
                    }
                    // end TT#488 - MD - Jellis - Group Allocaiton;
                }
                if ((_trans.SummaryView != null) || (_trans.StyleView != null) || (_trans.AssortmentView != null))
                {
                    GetCurrentSettings();
                }
                // END MID Track #2507 
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

		public void Navigate( string navTo )
		{
			System.Windows.Forms.Form frm = null;
			MIDRetail.Windows.SummaryView frmSummaryView;
			MIDRetail.Windows.StyleView frmStyleView;
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
					
					case "Style":
						
						if(_trans.StyleView != null)
						{
							frmStyleView = (MIDRetail.Windows.StyleView)_trans.StyleView;
							frmStyleView.Activate();
							return;
						}
						else
						{
							// BEGIN MID Track #2551 - security not working
							if (_allocationReviewStyleSecurity.AccessDenied)
							{
								okToContinue = false;
							}
							else
							{
								_trans.AllocationViewType = eAllocationSelectionViewType.Style;
								_trans.AllocationGroupBy = Convert.ToInt32(eAllocationStyleViewGroupBy.Header);
                                frm = new MIDRetail.Windows.StyleView(_eab, _trans);
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

				// BEGIN MID Track #4077 - vertical splitters not correct when window first opens. 
				// only happens in Windows Server 2003 SP1, so this is a workaround
				if (navTo == "Style") 
				{
					((MIDRetail.Windows.StyleView)frm).ResetSplitters();
				}
				// END MID Track #4077
				Cursor.Current = Cursors.Default;
	 
			}	
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SaveCurrentSettings()
		{
			if (rbHeader.Checked)
				_curGroupBy = Convert.ToInt32(eAllocationSizeViewGroupBy.Header, CultureInfo.CurrentUICulture);
			else
				_curGroupBy = Convert.ToInt32(eAllocationSizeViewGroupBy.Color, CultureInfo.CurrentUICulture);
		
			_curAttribute    = _trans.AllocationStoreAttributeID;
			_curAttributeSet = _trans.AllocationStoreGroupLevel;
		}
		public void GetCurrentSettings()	//TT#3808 - Group Allocation - Need Action against Headers receives "DuplicateNameException" error - 
		{
			_trans.AllocationViewType = eAllocationSelectionViewType.Size;
			_trans.AllocationGroupBy = _curGroupBy;
			_trans.AllocationStoreAttributeID = _curAttribute;
			_trans.AllocationStoreGroupLevel = _curAttributeSet;
		}

		override protected void BeforeClosing()
		{
			try
			{
                // Begin TT#1639 - RMatelic - Error message on Header with Piggybacking Workflow
                //if (!CheckForChangedCellOK())
                //    return;
                if (_gridRebuildInProgress)
                {
                    CancelFormClosing = true;
                }
                else if (!CheckForChangedCellOK())
                {
                    CancelFormClosing = true;
                }
                // End TT#1639
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
				_trans.SizeView = null;
				_trans.CheckForHeaderDequeue();
			}
			catch (Exception ex)
			{
				HandleException(ex,"SizeView.AfterClosing");
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
						okToContinue = false;
						throw new MIDException(eErrorLevel.severe,
							(int)eMIDTextCode.msg_MustBeNumeric,
							MIDText.GetText(eMIDTextCode.msg_MustBeNumeric));
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


		private void SizeView_Activated(object sender, System.EventArgs e)
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

		private void SizeView_Deactivate(object sender, System.EventArgs e)
		{
			SaveCurrentSettings();
		}

		#region Grid BeforeScroll Events

		//		private void HighlightActiveAttributeSet()
		//		{
		//			int i;
		//
		//			for (i = 0; i < g7.Rows.Count; i++)
		//			{
		//				if (((RowHeaderTag)g7.Rows[i].UserData).GroupRowColHeader.Profile.Key == (int)cmbAttributeSet.SelectedValue)
		//				{
		//					vScrollBar3.Value = System.Math.Min(CalculateGroupFromDetail(g7, i), vScrollBar3.Maximum - vScrollBar3.LargeChange + 1);
		//					break;
		//				}
		//			}
		//		}

		private void g2_BeforeScroll(object sender, RangeEventArgs e)
		{
			try
			{
                // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                //if (_isScrolling)
                //{
                //    BeforeScroll(g2, null, g5, null, g2, null, hScrollBar2);
                //}
                //else
                //{
                //    e.Cancel = true;
                //}
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g2, null, g5, null, g2, null, hScrollBar2);
                BeforeScroll(g2, null, g5, null, g2, g2, null, hScrollBar2);
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
                //if (_isScrolling)
                //{
                //    BeforeScroll(g3, null, g6, null, g2, null, hScrollBar3);
                //}
                //else
                //{
                //    e.Cancel = true;
                //}
                // Begin TT#199-MD - RMatelic - Column headers not moving with the cells while using arrow keys
                //BeforeScroll(g3, null, g6, null, g2, null, hScrollBar3);
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g3, null, g3, null, g2, null, hScrollBar3);
                BeforeScroll(g3, null, g3, null, g2, g2, null, hScrollBar3);
                // End TT#2930 - JSmith - Scrolling Issue
                // End TT#199-MD
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
                //if (_isScrolling)
                //{
                //    BeforeScroll(g4, g6, g1, g4, g1, vScrollBar2, hScrollBar1); 
                //}
                //else
                //{
                //    e.Cancel = true;
                //}
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
                //if (_isScrolling)
                //{
                //    BeforeScroll(g5, g4, g2, g4, g2, vScrollBar2, hScrollBar2);
                //}
                //else
                //{
                //    e.Cancel = true;
                //}
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g5, g4, g2, g4, g2, vScrollBar2, hScrollBar2);
                BeforeScroll(g5, g4, g2, g4, g2, g5, vScrollBar2, hScrollBar2);
                // End TT#2930 - JSmith - Scrolling Issue
                // End TT#1144
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
                //if (_isScrolling)
                //{
                //    BeforeScroll(g6, g4, g3, g4, g3, vScrollBar2, hScrollBar3);
                //}
                //else
                //{
                //    e.Cancel = true;
                //}
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
                //if (_isScrolling)
                //{
                //    BeforeScroll(g7, g7, g1, g7, g1, vScrollBar3, hScrollBar1);
                //}	
                //else
                //{
                //    e.Cancel = true;
                //}
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
                //if (_isScrolling)
                //{
                //    BeforeScroll(g8, g7, g2, g7, g2, vScrollBar3, hScrollBar2);
                //}
                //else
                //{
                //    e.Cancel = true;
                //}
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
                //if (_isScrolling)
                //{
                //    BeforeScroll(g9, g7, g3, g7, g3, vScrollBar3, hScrollBar3);
                //}
                //else
                //{
                //    e.Cancel = true;
                //}
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
                //if (_isScrolling)
                //{
                //    BeforeScroll(g10, g12, g1, g10, g1, vScrollBar4, hScrollBar1);
                //}
                //else
                //{
                //    e.Cancel = true;
                //}
                // Begin TT#2930 - JSmith - Scrolling Issue
                //BeforeScroll(g10, g12, g1, g10, g1, vScrollBar4, hScrollBar1);
                BeforeScroll(g10, g12, g1, g10, g1, g11, vScrollBar4, hScrollBar1);
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
                //if (_isScrolling)
                //{
                //    BeforeScroll(g11, g10, g2, g10, g2, vScrollBar4, hScrollBar2);
                //}
                //else
                //{
                //    e.Cancel = true;
                //}
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
                //if (_isScrolling)
                //{
                //    BeforeScroll(g12, g10, g3, g10, g3, vScrollBar4, hScrollBar3);
                //}
                //else
                //{
                //    e.Cancel = true;
                //}
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
                            // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                            //aVScrollBar.Value = Math.Min(CalculateGroupFromDetail(aRowHeaderGrid, aGrid.TopRow) + 1, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1);
                            ////aVScrollBar.Value = Math.Min(aGrid.TopRow + 1, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1);
                            if (aVScrollBar == vScrollBar3)
                            {
                                int saveScrollVarValue = aVScrollBar.Value;
                                // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                //aVScrollBar.Value = Math.Min(aGrid.TopRow / 2, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1);
                                SetVScrollValue(aVScrollBar, Math.Min(aGrid.TopRow / 2, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1));
                                // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                // If scroll value does not change, force a change or the grids may misalign
                                if (aVScrollBar.Value == saveScrollVarValue)
                                {
                                    aVScrollBar.Value = aVScrollBar.Minimum;
                                    aVScrollBar.Value = aVScrollBar.Maximum;
                                    // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                    //aVScrollBar.Value = saveScrollVarValue;
                                    SetVScrollValue(aVScrollBar, saveScrollVarValue);
                                    // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                }
                            }
                            else if (aVScrollBar == vScrollBar4)
                            {
                                int saveScrollVarValue = aVScrollBar.Value;
                                // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                //aVScrollBar.Value = Math.Min(aGrid.TopRow / _rowsPerGroup10, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1);
                                SetVScrollValue(aVScrollBar, Math.Min(aGrid.TopRow / _rowsPerGroup10, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1));
                                // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                // If scroll value does not change, force a change or the grids may misalign
                                if (aVScrollBar.Value == saveScrollVarValue)
                                {
                                    aVScrollBar.Value = aVScrollBar.Minimum;
                                    aVScrollBar.Value = aVScrollBar.Maximum;
                                    // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                    //aVScrollBar.Value = saveScrollVarValue;
                                    SetVScrollValue(aVScrollBar, saveScrollVarValue);
                                    // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                }
                            } 
                            else
                            {
                                // Begin TT#2930 - JSmith - Scrolling Issue
                                //aVScrollBar.Value = Math.Min(aGrid.TopRow, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1);
                                if (aVScrollBar.Value != Math.Min(aGrid.TopRow, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1))
                                {
                                    // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                    //aVScrollBar.Value = Math.Min(aGrid.TopRow, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1);
                                    SetVScrollValue(aVScrollBar, Math.Min(aGrid.TopRow, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1));
                                    // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                }
                                else if (aTotalsGrid != null &&
                                        aGrid.Name != aTotalsGrid.Name &&
                                    aGrid.ScrollPosition.Y != aTotalsGrid.ScrollPosition.Y)
                                {
                                    Point point = new Point(aRowCompGrid.ScrollPosition.X, aGrid.ScrollPosition.Y);
                                    //aRowCompGrid.ScrollPosition = point;
                                    //aTotalsGrid.ScrollPosition = point;
                                    switch (aGrid.Name)
                                    {
                                        case "g4":
                                            g5.ScrollPosition = point;
                                            g6.ScrollPosition = point;
                                            break;
                                        case "g5":
                                            g4.ScrollPosition = point;
                                            g6.ScrollPosition = point;
                                            break;
                                        case "g6":
                                            g4.ScrollPosition = point;
                                            g5.ScrollPosition = point;
                                            break;
                                        case "g7":
                                            g8.ScrollPosition = point;
                                            g9.ScrollPosition = point;
                                            break;
                                        case "g8":
                                            g7.ScrollPosition = point;
                                            g9.ScrollPosition = point;
                                            break;
                                        case "g9":
                                            g7.ScrollPosition = point;
                                            g8.ScrollPosition = point;
                                            break;
                                    }
                                }
                                // End TT#2930 - JSmith - Scrolling Issue
                            }
                            // End TT#1144
						}
						else if (aGrid.ScrollPosition.Y > aRowCompGrid.ScrollPosition.Y)
						{
                            // Begin TT#1144 - RMatelic - Issue with scrolling in Velocity Store Detail / Size Review / Style Review
                            //aVScrollBar.Value = CalculateGroupFromDetail(aRowHeaderGrid, aGrid.TopRow);
                            ////if (aGrid.TopRow >= 0)
                            ////	aVScrollBar.Value =  aGrid.TopRow;
                            if (aVScrollBar == vScrollBar3)
                            {
                                // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                //aVScrollBar.Value = aGrid.TopRow / 2;
                                SetVScrollValue(aVScrollBar, aGrid.TopRow / 2);
                                // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                            }
                            else if (aVScrollBar == vScrollBar4)
                            {
                                // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                //aVScrollBar.Value = aGrid.TopRow / _rowsPerGroup10;
                                SetVScrollValue(aVScrollBar, aGrid.TopRow / _rowsPerGroup10);
                                // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                            }
                            else
                            {
                                if (aGrid.TopRow >= 0)
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
                        // Begin TT#2027 - JSmith - System out of Argument Range
                        //if (aGrid.ScrollPosition.X < aColCompGrid.ScrollPosition.X)
                        if (aGrid.ScrollPosition.X < aColCompGrid.ScrollPosition.X && _arrowScroll)
                        // End TT#2027
						{
							//aHScrollBar.Value = Math.Min(CalculateGroupFromDetail(aColHeaderGrid, aGrid.LeftCol) + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1);
                            // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                            //aHScrollBar.Value = Math.Min(aGrid.LeftCol + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1);
                            SetHScrollValue(aHScrollBar, Math.Min(aGrid.LeftCol + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1));
                            // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
						}
                        // Begin TT#2027 - JSmith - System out of Argument Range
                        //else if (aGrid.ScrollPosition.X > aColCompGrid.ScrollPosition.X)
                        else if (aGrid.ScrollPosition.X > aColCompGrid.ScrollPosition.X && _arrowScroll)
                        // End TT#2027
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
                        // Begin TT#2027 - JSmith - System out of Argument Range
                        //if (aHScrollBar == hScrollBar2 || aHScrollBar == hScrollBar3)
                        //if (aHScrollBar == hScrollBar2 || aHScrollBar == hScrollBar3 && _arrowScroll)
                        //// End TT#2027
                        //    ((ScrollBarValueChanged)aHScrollBar.Tag)(aHScrollBar.Value);
                        if ((aHScrollBar == hScrollBar2 || aHScrollBar == hScrollBar3) && _arrowScroll)
                        {
                            ((ScrollBarValueChanged)aHScrollBar.Tag)(aHScrollBar.Value);
                        }
                        // End TT#3563  
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


		private int CalculateGroupFromDetail(C1FlexGrid aGrid, int aDetail)
		{
			try
			{
				if (((GridTag)aGrid.Tag).UnitsPerScroll > 0)
				{
					return aDetail / ((GridTag)aGrid.Tag).UnitsPerScroll;
				}
				else
				{
					return 0;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		#endregion
		
		// Begin TT#199-MD - RMatelic - Column headers not moving with the cells while using arrow keys
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
        // End TT#199-MD  

        //BEGIN TT#6-MD-VStuart - Single Store Select
        private void cmbStoreAttribute_SelectionChangeCommitted(object sender, EventArgs e)
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
                    //_trans.CurrentStoreGroupProfile = (StoreGroupProfile)(SAB.ApplicationServerSession.GetProfileList(eProfileType.StoreGroup)).FindKey(_trans.AllocationStoreAttributeID);
                    _trans.CurrentStoreGroupProfile = StoreMgmt.StoreGroup_Get(_trans.AllocationStoreAttributeID); //(StoreGroupProfile)StoreMgmt.GetStoreGroupList().FindKey(_trans.AllocationStoreAttributeID); //TT#1517-MD -jsobek -Store Service Optimization
                    //END   TT#2927-MD-VStuart-Error during Single Store Select
                    // Begin Track #6404 - JSmith - 80302: Units to allocate is calculated when header is Work Up Total Buy
                    //ChangePending = true;
                    // End Track #6404
                }

                if (this.cmbStoreAttribute.SelectedValue != null)
                    PopulateStoreAttributeSet(this.cmbStoreAttribute.SelectedValue.ToString());

                cmbAttributeSet_ScreenUpdate(sender, e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        //END TT#6-MD-VStuart - Single Store Select

        //BEGIN TT#6-MD-VStuart - Single Store Select
        private void cmbAttributeSet_SelectionChangeCommitted(object sender, EventArgs e)
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
                _trans.AllocationStoreGroupLevel = Convert.ToInt32(cmbAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
                _resetV1Splitter = true;
                CriteriaChanged();
                // Begin TT#2779 - JSmith - Object Reference Error When Exporting From Size Review
                ApplyPreferences();
                // End TT#2779 - JSmith - Object Reference Error When Exporting From Size Review
                // BEGIN MID Track #2938 - set totals wrong. For some reason, grids g8 and g9 are not
                // scrolling to the correct row when the set is changed, even though they appear to be set 
                // correctly in HilightSelectedSet(). Added the re-execution of HilightSelectedSet() 
                // and adjust the rows again. 
                HilightSelectedSet();
                // END MID Track #2938
            }
        }
        //END TT#6-MD-VStuart - Single Store Select

        //BEGIN TT#6-MD-VStuart - Single Store Select
        private void cmbView_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (!_bindingView)
                {
                    _viewRID = Convert.ToInt32(cmbView.SelectedValue, CultureInfo.CurrentUICulture);
                    if (_viewRID != _lastSelectedViewRID)
                    {
                        // Begin TT#2360 - RMatelic - Received an error going into the size review
                        if (!ViewIsValid(_viewRID))
                        {
                            _viewRID = _lastSelectedViewRID;
                            cmbView.SelectedValue = _viewRID;
                            //this.cmbView_SelectionChangeCommitted(source, new EventArgs());
                            return;
                        }
                        // End TT#2360
                        _lastSelectedViewRID = _viewRID;
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
                      
                        // View not saving when hitting 'X' in upper right corner to exit; not supposed to, but add 'nag' message   
                        if (_trans.DataState == eDataState.ReadOnly || FunctionSecurity.AllowUpdate == false)
                        {
                            // do not update
                        }
                        else if (!_loading)
                        {
                            ChangePending = true;
                        }
                    }    
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmbFilter_SelectionChangeCommitted(object sender, EventArgs e)
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

                if (!_loading)
                    ReloadGridData();

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cboAction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int val = Convert.ToInt32(cboAction.SelectedValue, CultureInfo.CurrentUICulture);
            btnProcess.Enabled = val != 0;
        }

        //private void cmbStoreAttribute_SelectionChangeCommitted(object sender, EventArgs e)
        //{

        //}
        //END TT#6-MD-VStuart - Single Store Select

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

    // Begin TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
    public class GridViewDetail
    {
        AllocationWaferCoordinateList _allocationWaferCoordinateList;
        int _visiblePosition;
        bool _isHidden;
        bool _isGroupByCol;
        eSortDirection _sortDirection;
        int _width;
        int _secondaryPosition;  // TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis

        public GridViewDetail(AllocationWaferCoordinateList aAllocationWaferCoordinateList,
            int aVisiblePosition,
            bool aIsHidden,
            bool aIsGroupByCol,
            eSortDirection aSortDirection,
            // Begin TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
            //int aWidth)
            int aWidth,
            int aSecondaryPosition)
            // End TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
        {
            _allocationWaferCoordinateList = aAllocationWaferCoordinateList;
            _visiblePosition = aVisiblePosition;
            _isHidden = aIsHidden;
            _isGroupByCol = aIsGroupByCol;
            _sortDirection = aSortDirection;
            _width = aWidth;
            _secondaryPosition = aSecondaryPosition;  // TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
        }
        public AllocationWaferCoordinateList AllocationWaferCoordinateList
        {
            get { return _allocationWaferCoordinateList; }
            set { _allocationWaferCoordinateList = value; }
        }
        public int VisiblePosition
        {
            get { return _visiblePosition; }
            set { _visiblePosition = value; }
        }
        public bool isHidden
        {
            get { return _isHidden; }
            set { _isHidden = value; }
        }
        public bool isGroupByCol
        {
            get { return _isGroupByCol; }
            set { _isGroupByCol = value; }
        }
        public eSortDirection SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        // Begin TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
        public int SecondaryPosition
        {
            get { return _secondaryPosition; }
            set { _secondaryPosition = value; }
        }
        // End TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
    }

    /// <summary>
    /// Indicates the descending sequence of two packs.
    /// </summary>
    public class GridViewDetailOrder : IComparer
    {
        /// <summary>
        /// Compares packs x and y and inicates the ascending sequence of the two. 
        /// </summary>
        /// <param name="x">First of two GridViewDetail's</param>
        /// <param name="y">Second of two GridViewDetail's</param>
        /// <returns>-1 if the pack multiple of x is less than the pack multiple of y; +1 if the pack multiple of y is less than the pack multiple of x; -1 if the pack multiples are equal and packs to allocate on y is less than the packs to allocate on x; +1 if the multiples are equal and the packs to allocate on x are less than the packs to allocate on y; 0 otherwise (equal).</returns>
        public int Compare(object x, object y)
        {
            if (!(x is GridViewDetail && y is GridViewDetail))
            {
                throw new ArgumentException("only GridViewDetail OrderBy objects");  
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return +1;
            }
            if (((GridViewDetail)x).VisiblePosition < ((GridViewDetail)y).VisiblePosition)
            {
                return -1;
            }
            if (((GridViewDetail)x).VisiblePosition > ((GridViewDetail)y).VisiblePosition)
            {
                return +1;
            }
            // Begin TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
            if (((GridViewDetail)x).SecondaryPosition < ((GridViewDetail)y).SecondaryPosition)
            {
                return -1;
            }
            if (((GridViewDetail)x).SecondaryPosition > ((GridViewDetail)y).SecondaryPosition)
            {
                return +1;
            }
            if (((GridViewDetail)x).isHidden != ((GridViewDetail)y).isHidden)
            {
                return -1;
            }
            // End TT#3589 - JSmith - Sizes in Wrong Order in Size Need Analysis
            return +1;
        }
    }
    // End TT#2922 - JSmith - Size review and size analysis view are distorted or sizes are out of order
}
