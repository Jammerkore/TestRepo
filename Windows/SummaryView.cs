using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using System.Data;
using System.Diagnostics;
using System.Configuration;
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
	public class SummaryView : MIDFormBase
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
		private System.Windows.Forms.Splitter VerticalSplitter1;
		private System.Windows.Forms.VScrollBar vScrollBar2;
		private System.Windows.Forms.Splitter s4;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		private System.Windows.Forms.Splitter s8;
		private System.Windows.Forms.Panel pnlSpacer;
		private C1.Win.C1FlexGrid.C1FlexGrid g5;
		private System.Windows.Forms.Splitter s2;
		private C1.Win.C1FlexGrid.C1FlexGrid g2;
		private System.Windows.Forms.Splitter s6;
		private System.Windows.Forms.Splitter VerticalSplitter2;
		private C1.Win.C1FlexGrid.C1FlexGrid g6;
		private System.Windows.Forms.Splitter s3;
		private C1.Win.C1FlexGrid.C1FlexGrid g3;
		private System.Windows.Forms.Splitter s7;
		private System.Windows.Forms.ContextMenu g2g3ContextMenu;
		private System.Windows.Forms.MenuItem mnuFreezeColumn;
		private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton rbGroupByAttribute;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.RadioButton rbGroupByStoreGrade;
		private C1.Win.C1FlexGrid.C1FlexGrid g1;
		private C1.Win.C1FlexGrid.C1FlexGrid g10;
		private System.Windows.Forms.Splitter s9;
		private C1.Win.C1FlexGrid.C1FlexGrid g11;
		private C1.Win.C1FlexGrid.C1FlexGrid g12;
		private System.Windows.Forms.Splitter s10;
		private System.Windows.Forms.Splitter s11;
		private System.Windows.Forms.Splitter s12;
		private System.Windows.Forms.VScrollBar vScrollBar4;
		private System.Windows.Forms.VScrollBar vScrollBar3;
		private C1.Win.C1FlexGrid.C1FlexGrid g9;
		private C1.Win.C1FlexGrid.C1FlexGrid g8;
		private C1.Win.C1FlexGrid.C1FlexGrid g7;
		private System.Windows.Forms.Button btnProcess;
		private System.ComponentModel.IContainer components;
		// Begin Track #4872 - JSmith - Global/User Attributes
		private MIDAttributeComboBox cmbStoreAttribute;
		// End Track #4872
//BEGIN TT#7 - RBeck - Dynamic dropdowns
		private MIDComboBoxEnh cmbFilter;
		private MIDComboBoxEnh cmbAttributeSet;
		private MIDComboBoxEnh cboAction;
//END TT#7 - RBeck - Dynamic dropdowns
		private string _thisTitle;
		#endregion

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
//BEGIN TT#7 - RBeck - Dynamic dropdowns
                this.cmbStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cmbStoreAttribute_SelectionChangeCommitted);
                this.cmbFilter.SelectionChangeCommitted -= new System.EventHandler(this.cmbFilter_SelectionChangeCommitted);
                this.cmbAttributeSet.SelectionChangeCommitted -= new System.EventHandler(this.cmbAttributeSet_SelectionChangeCommitted);

//END   TT#7 - RBeck - Dynamic dropdowns

			//	this.cmbFilter.SelectionChangeCommitted -= new System.EventHandler(this.cmbFilter_SelectionChangeCommitted); //BEGIN TT#7 - RBeck - Dynamic dropdowns
                this.cmbFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cmbFilter_DragEnter);
                this.cmbFilter.DragOver -= new System.Windows.Forms.DragEventHandler(this.cmbFilter_DragOver);
                this.cmbFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cmbFilter_DragDrop);
				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
				this.btnApply.Click -= new System.EventHandler(this.btnApply_Click);
				this.rbGroupByStoreGrade.CheckedChanged -= new System.EventHandler(this.rbGroupByStoreGrade_CheckedChanged);
				this.rbGroupByAttribute.CheckedChanged -= new System.EventHandler(this.rbGroupByAttribute_CheckedChanged);
                // Begin Track #4872 - JSmith - Global/User Attributes
                this.cmbStoreAttribute.DragOver -= new System.Windows.Forms.DragEventHandler(this.cmbStoreAttribute_DragOver);
                this.cmbStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cmbStoreAttribute_DragEnter);
                // End Track #4872
                this.cmbStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cmbStoreAttribute_DragEnter);
                this.cmbStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cmbStoreAttribute_DragDrop);
                this.cmbAttributeSet.EnabledChanged -= new System.EventHandler(this.cmbAttributeSet_EnabledChanged);
	//	        this.cmbAttributeSet.SelectionChangeCommitted -= new System.EventHandler(this.cmbAttributeSet_SelectionChangeCommitted);    //TT#7 - RBeck - Dynamic dropdowns
    //			this.cboAction.SelectionChangeCommitted -= new System.EventHandler(this.cboAction_SelectionChangeCommitted);                //TT#7 - RBeck - Dynamic dropdowns
				this.g7.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g7.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g7_OwnerDrawCell);
				this.g7.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g7_BeforeScroll);
				this.s9.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
				this.s9.DoubleClick -= new System.EventHandler(this.g10SplitterDoubleClick);
				this.g10.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g10.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g10_OwnerDrawCell);
				this.g10.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g10_BeforeScroll);
				this.s5.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
				this.s5.DoubleClick -= new System.EventHandler(this.g7SplitterDoubleClick);
				this.g4.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g4.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g4_OwnerDrawCell);
				this.g4.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g4_BeforeScroll);
				this.s1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.s1_SplitterMoved);
				this.s1.DoubleClick -= new System.EventHandler(this.g4SplitterDoubleClick);
				this.hScrollBar1.ValueChanged -= new System.EventHandler(this.hScrollBar1_ValueChanged);
				this.g1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
				this.g1.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g1_AfterResizeColumn);
				this.g1.QueryContinueDrag -= new System.Windows.Forms.QueryContinueDragEventHandler(this.GridQueryContinueDrag);
				this.g1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g1.BeforeResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeResizeColumn);
				this.g1.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.GridMouseUp);
				this.VerticalSplitter1.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.VerticalSplitter1_SplitterMoved);
				this.VerticalSplitter1.DoubleClick -= new System.EventHandler(this.VerticalSplitter1_DoubleClick);
				this.s12.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
				this.s12.DoubleClick -= new System.EventHandler(this.g10SplitterDoubleClick);
				this.vScrollBar3.ValueChanged -= new System.EventHandler(this.vScrollBar3_ValueChanged);
				this.vScrollBar4.ValueChanged -= new System.EventHandler(this.vScrollBar4_ValueChanged);
				this.s8.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
				this.s8.DoubleClick -= new System.EventHandler(this.g7SplitterDoubleClick);
				this.vScrollBar2.ValueChanged -= new System.EventHandler(this.vScrollBar2_ValueChanged);
				this.s4.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.s4_SplitterMoved);
				this.s4.DoubleClick -= new System.EventHandler(this.g4SplitterDoubleClick);
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
				this.s6.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
				this.s6.DoubleClick -= new System.EventHandler(this.g7SplitterDoubleClick);
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
				this.g2.DragEnter -= new System.Windows.Forms.DragEventHandler(this.GridDragEnter);
				this.g2.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
				this.g2.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_AfterResizeColumn);
				this.g2.QueryContinueDrag -= new System.Windows.Forms.QueryContinueDragEventHandler(this.GridQueryContinueDrag);
				this.g2.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g2.BeforeResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeResizeColumn);
				this.g2.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.GridMouseUp);
				this.g2.DragDrop -= new System.Windows.Forms.DragEventHandler(this.g2_DragDrop);
				this.hScrollBar2.ValueChanged -= new System.EventHandler(this.hScrollBar2_ValueChanged);
				this.hScrollBar2.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
				this.mnuFreezeColumn.Click -= new System.EventHandler(this.mnuFreezeColumn_Click);
                // begin TT#59 Implement Store Temp Locks
                if (this.SAB.AllowDebugging)
                {
                    this.mnuHeaderAllocationCriteria.Click -= new System.EventHandler(this.mnuHeaderAllocationCriteria_Click);
                }
                // end TT#59 Implement Store Temp Locks
                this.VerticalSplitter2.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.VerticalSplitter2_SplitterMoved);
				this.VerticalSplitter2.DoubleClick -= new System.EventHandler(this.VerticalSplitter2_DoubleClick);
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
				this.s7.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
				this.s7.DoubleClick -= new System.EventHandler(this.g7SplitterDoubleClick);
				this.g6.BeforeEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
				this.g6.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g6.StartEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
				this.g6.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g6_OwnerDrawCell);
				this.g6.BeforeScroll -= new C1.Win.C1FlexGrid.RangeEventHandler(this.g6_BeforeScroll);
				this.g6.AfterEdit -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
				this.g6.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
				this.g6.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
				this.s3.SplitterMoved -= new System.Windows.Forms.SplitterEventHandler(this.s3_SplitterMoved);
				this.s3.DoubleClick -= new System.EventHandler(this.g4SplitterDoubleClick);
				this.g3.DragOver -= new System.Windows.Forms.DragEventHandler(this.GridDragOver);
				this.g3.DragEnter -= new System.Windows.Forms.DragEventHandler(this.GridDragEnter);
				this.g3.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
				this.g3.AfterResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_AfterResizeColumn);
				this.g3.QueryContinueDrag -= new System.Windows.Forms.QueryContinueDragEventHandler(this.GridQueryContinueDrag);
				this.g3.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
				this.g3.BeforeResizeColumn -= new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeResizeColumn);
				this.g3.OwnerDrawCell -= new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g3_OwnerDrawCell);
				this.g3.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.GridMouseUp);
				this.g3.DragDrop -= new System.Windows.Forms.DragEventHandler(this.g3_DragDrop);
				this.hScrollBar3.ValueChanged -= new System.EventHandler(this.hScrollBar3_ValueChanged);
				this.hScrollBar3.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.hScrollBar3_Scroll);
				this.Load -= new System.EventHandler(this.SummaryView_Load);
				this.Activated -= new System.EventHandler(this.SummaryView_Activated);
				this.Deactivate -= new System.EventHandler(this.SummaryView_Deactivate);

                this.cboAction.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboAction_MIDComboBoxPropertiesChangedEvent);
                this.cmbAttributeSet.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cmbAttributeSet_MIDComboBoxPropertiesChangedEvent);
                this.cmbFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cmbFilter_MIDComboBoxPropertiesChangedEvent);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SummaryView));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.cboAction = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cmbAttributeSet = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cmbFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.btnProcess = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbGroupByStoreGrade = new System.Windows.Forms.RadioButton();
            this.rbGroupByAttribute = new System.Windows.Forms.RadioButton();
            this.cmbStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.pnlRowHeaders = new System.Windows.Forms.Panel();
            this.g7 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s9 = new System.Windows.Forms.Splitter();
            this.g10 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s5 = new System.Windows.Forms.Splitter();
            this.g4 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s1 = new System.Windows.Forms.Splitter();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.g1 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.VerticalSplitter1 = new System.Windows.Forms.Splitter();
            this.pnlScrollBars = new System.Windows.Forms.Panel();
            this.s12 = new System.Windows.Forms.Splitter();
            this.vScrollBar3 = new System.Windows.Forms.VScrollBar();
            this.vScrollBar4 = new System.Windows.Forms.VScrollBar();
            this.s8 = new System.Windows.Forms.Splitter();
            this.vScrollBar2 = new System.Windows.Forms.VScrollBar();
            this.s4 = new System.Windows.Forms.Splitter();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.pnlSpacer = new System.Windows.Forms.Panel();
            this.pnlTotals = new System.Windows.Forms.Panel();
            this.g8 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s10 = new System.Windows.Forms.Splitter();
            this.g11 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s6 = new System.Windows.Forms.Splitter();
            this.g5 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s2 = new System.Windows.Forms.Splitter();
            this.g2 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.hScrollBar2 = new System.Windows.Forms.HScrollBar();
            this.g2g3ContextMenu = new System.Windows.Forms.ContextMenu();
            this.mnuFreezeColumn = new System.Windows.Forms.MenuItem();
            this.mnuHeaderAllocationCriteria = new System.Windows.Forms.MenuItem();
            this.VerticalSplitter2 = new System.Windows.Forms.Splitter();
            this.pnlData = new System.Windows.Forms.Panel();
            this.g9 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s11 = new System.Windows.Forms.Splitter();
            this.g12 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s7 = new System.Windows.Forms.Splitter();
            this.g6 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.s3 = new System.Windows.Forms.Splitter();
            this.g3 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.hScrollBar3 = new System.Windows.Forms.HScrollBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlRowHeaders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g1)).BeginInit();
            this.pnlScrollBars.SuspendLayout();
            this.pnlTotals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g2)).BeginInit();
            this.pnlData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g3)).BeginInit();
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
            this.pnlTop.Controls.Add(this.cmbAttributeSet);
            this.pnlTop.Controls.Add(this.cmbFilter);
            this.pnlTop.Controls.Add(this.btnProcess);
            this.pnlTop.Controls.Add(this.btnApply);
            this.pnlTop.Controls.Add(this.groupBox1);
            this.pnlTop.Controls.Add(this.cmbStoreAttribute);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(970, 40);
            this.pnlTop.TabIndex = 0;
            // 
            // cboAction
            // 
            this.cboAction.AutoAdjust = true;
            this.cboAction.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAction.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAction.DataSource = null;
            this.cboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAction.DropDownWidth = 151;
            this.cboAction.FormattingEnabled = false;
            this.cboAction.IgnoreFocusLost = false;
            this.cboAction.ItemHeight = 13;
            this.cboAction.Location = new System.Drawing.Point(632, 8);
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
            // cmbAttributeSet
            // 
            this.cmbAttributeSet.AutoAdjust = true;
            this.cmbAttributeSet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbAttributeSet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbAttributeSet.DataSource = null;
            this.cmbAttributeSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttributeSet.DropDownWidth = 141;
            this.cmbAttributeSet.FormattingEnabled = false;
            this.cmbAttributeSet.IgnoreFocusLost = false;
            this.cmbAttributeSet.ItemHeight = 13;
            this.cmbAttributeSet.Location = new System.Drawing.Point(333, 8);
            this.cmbAttributeSet.Margin = new System.Windows.Forms.Padding(0);
            this.cmbAttributeSet.MaxDropDownItems = 25;
            this.cmbAttributeSet.Name = "cmbAttributeSet";
            this.cmbAttributeSet.SetToolTip = "";
            this.cmbAttributeSet.Size = new System.Drawing.Size(141, 21);
            this.cmbAttributeSet.TabIndex = 4;
            this.cmbAttributeSet.Tag = null;
            this.cmbAttributeSet.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cmbAttributeSet_MIDComboBoxPropertiesChangedEvent);
            this.cmbAttributeSet.SelectionChangeCommitted += new System.EventHandler(this.cmbAttributeSet_SelectionChangeCommitted);
            this.cmbAttributeSet.EnabledChanged += new System.EventHandler(this.cmbAttributeSet_EnabledChanged);
            // 
            // cmbFilter
            // 
            this.cmbFilter.AllowDrop = true;
            this.cmbFilter.AutoAdjust = true;
            this.cmbFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFilter.DataSource = null;
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.DropDownWidth = 141;
            this.cmbFilter.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbFilter.FormattingEnabled = false;
            this.cmbFilter.IgnoreFocusLost = false;
            this.cmbFilter.ItemHeight = 13;
            this.cmbFilter.Location = new System.Drawing.Point(483, 8);
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
            // btnProcess
            // 
            this.btnProcess.Enabled = false;
            this.btnProcess.Location = new System.Drawing.Point(792, 8);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(64, 21);
            this.btnProcess.TabIndex = 7;
            this.btnProcess.Text = "Process";
            this.toolTip1.SetToolTip(this.btnProcess, "Process action");
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(868, 8);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(64, 21);
            this.btnApply.TabIndex = 8;
            this.btnApply.Text = "Apply";
            this.toolTip1.SetToolTip(this.btnApply, "Apply current changes");
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbGroupByStoreGrade);
            this.groupBox1.Controls.Add(this.rbGroupByAttribute);
            this.groupBox1.Location = new System.Drawing.Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(170, 32);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // rbGroupByStoreGrade
            // 
            this.rbGroupByStoreGrade.Location = new System.Drawing.Point(72, 8);
            this.rbGroupByStoreGrade.Name = "rbGroupByStoreGrade";
            this.rbGroupByStoreGrade.Size = new System.Drawing.Size(96, 16);
            this.rbGroupByStoreGrade.TabIndex = 1;
            this.rbGroupByStoreGrade.Text = "Store Grade";
            this.toolTip1.SetToolTip(this.rbGroupByStoreGrade, "Group by Store Grade");
            this.rbGroupByStoreGrade.CheckedChanged += new System.EventHandler(this.rbGroupByStoreGrade_CheckedChanged);
            // 
            // rbGroupByAttribute
            // 
            this.rbGroupByAttribute.Checked = true;
            this.rbGroupByAttribute.Location = new System.Drawing.Point(8, 8);
            this.rbGroupByAttribute.Name = "rbGroupByAttribute";
            this.rbGroupByAttribute.Size = new System.Drawing.Size(64, 16);
            this.rbGroupByAttribute.TabIndex = 0;
            this.rbGroupByAttribute.TabStop = true;
            this.rbGroupByAttribute.Text = "Attribute";
            this.toolTip1.SetToolTip(this.rbGroupByAttribute, "Group by Attribute");
            this.rbGroupByAttribute.CheckedChanged += new System.EventHandler(this.rbGroupByAttribute_CheckedChanged);
            // 
            // cmbStoreAttribute
            // 
            this.cmbStoreAttribute.AllowDrop = true;
            this.cmbStoreAttribute.AllowUserAttributes = false;
            this.cmbStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStoreAttribute.Location = new System.Drawing.Point(184, 8);
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
            this.pnlRowHeaders.Controls.Add(this.g7);
            this.pnlRowHeaders.Controls.Add(this.s9);
            this.pnlRowHeaders.Controls.Add(this.g10);
            this.pnlRowHeaders.Controls.Add(this.s5);
            this.pnlRowHeaders.Controls.Add(this.g4);
            this.pnlRowHeaders.Controls.Add(this.s1);
            this.pnlRowHeaders.Controls.Add(this.hScrollBar1);
            this.pnlRowHeaders.Controls.Add(this.g1);
            this.pnlRowHeaders.Controls.Add(this.panel1);
            this.pnlRowHeaders.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlRowHeaders.Location = new System.Drawing.Point(0, 40);
            this.pnlRowHeaders.Name = "pnlRowHeaders";
            this.pnlRowHeaders.Size = new System.Drawing.Size(80, 374);
            this.pnlRowHeaders.TabIndex = 1;
            // 
            // g7
            // 
            this.g7.AllowEditing = false;
            this.g7.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g7.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g7.ColumnInfo = "10,0,0,0,0,85,Columns:";
            this.g7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g7.ExtendLastCol = true;
            this.g7.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g7.Location = new System.Drawing.Point(0, 156);
            this.g7.Name = "g7";
            this.g7.Rows.DefaultSize = 17;
            this.g7.Rows.Fixed = 0;
            this.g7.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g7.Size = new System.Drawing.Size(80, 110);
            this.g7.TabIndex = 17;
            this.g7.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g7_BeforeScroll);
            this.g7.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g7_OwnerDrawCell);
            this.g7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // s9
            // 
            this.s9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.s9.Location = new System.Drawing.Point(0, 266);
            this.s9.Name = "s9";
            this.s9.Size = new System.Drawing.Size(80, 1);
            this.s9.TabIndex = 16;
            this.s9.TabStop = false;
            this.s9.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
            this.s9.DoubleClick += new System.EventHandler(this.g10SplitterDoubleClick);
            // 
            // g10
            // 
            this.g10.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g10.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g10.ColumnInfo = "10,0,0,0,0,85,Columns:";
            this.g10.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.g10.ExtendLastCol = true;
            this.g10.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g10.Location = new System.Drawing.Point(0, 267);
            this.g10.Name = "g10";
            this.g10.Rows.DefaultSize = 17;
            this.g10.Rows.Fixed = 0;
            this.g10.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g10.Size = new System.Drawing.Size(80, 90);
            this.g10.TabIndex = 15;
            this.g10.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g10_BeforeScroll);
            this.g10.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g10_OwnerDrawCell);
            this.g10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // s5
            // 
            this.s5.Dock = System.Windows.Forms.DockStyle.Top;
            this.s5.Location = new System.Drawing.Point(0, 155);
            this.s5.Name = "s5";
            this.s5.Size = new System.Drawing.Size(80, 1);
            this.s5.TabIndex = 8;
            this.s5.TabStop = false;
            this.s5.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
            this.s5.DoubleClick += new System.EventHandler(this.g7SplitterDoubleClick);
            // 
            // g4
            // 
            this.g4.AllowEditing = false;
            this.g4.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g4.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g4.ColumnInfo = "10,0,0,0,0,75,Columns:0{AllowEditing:False;}\t";
            this.g4.Dock = System.Windows.Forms.DockStyle.Top;
            this.g4.ExtendLastCol = true;
            this.g4.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g4.Location = new System.Drawing.Point(0, 65);
            this.g4.Name = "g4";
            this.g4.Rows.DefaultSize = 17;
            this.g4.Rows.Fixed = 0;
            this.g4.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g4.Size = new System.Drawing.Size(80, 90);
            this.g4.TabIndex = 1;
            this.g4.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g4_BeforeScroll);
            this.g4.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g4_OwnerDrawCell);
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
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 357);
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
            this.g1.ColumnInfo = "10,0,0,0,0,75,Columns:0{AllowEditing:False;Style:\"ImageAlign:CenterTop;\";}\t";
            this.g1.Dock = System.Windows.Forms.DockStyle.Top;
            this.g1.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g1.ExtendLastCol = true;
            this.g1.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g1.Location = new System.Drawing.Point(0, 0);
            this.g1.Name = "g1";
            this.g1.Rows.DefaultSize = 17;
            this.g1.Rows.Fixed = 0;
            this.g1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g1.Size = new System.Drawing.Size(80, 64);
            this.g1.TabIndex = 1;
            this.g1.BeforeResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeResizeColumn);
            this.g1.AfterResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g1_AfterResizeColumn);
            this.g1.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.GridQueryContinueDrag);
            this.g1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            this.g1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
            this.g1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GridMouseUp);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(80, 0);
            this.panel1.TabIndex = 12;
            this.panel1.Visible = false;
            // 
            // VerticalSplitter1
            // 
            this.VerticalSplitter1.Location = new System.Drawing.Point(80, 40);
            this.VerticalSplitter1.Name = "VerticalSplitter1";
            this.VerticalSplitter1.Size = new System.Drawing.Size(1, 374);
            this.VerticalSplitter1.TabIndex = 2;
            this.VerticalSplitter1.TabStop = false;
            this.VerticalSplitter1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.VerticalSplitter1_SplitterMoved);
            this.VerticalSplitter1.DoubleClick += new System.EventHandler(this.VerticalSplitter1_DoubleClick);
            // 
            // pnlScrollBars
            // 
            this.pnlScrollBars.Controls.Add(this.s12);
            this.pnlScrollBars.Controls.Add(this.vScrollBar3);
            this.pnlScrollBars.Controls.Add(this.vScrollBar4);
            this.pnlScrollBars.Controls.Add(this.s8);
            this.pnlScrollBars.Controls.Add(this.vScrollBar2);
            this.pnlScrollBars.Controls.Add(this.s4);
            this.pnlScrollBars.Controls.Add(this.vScrollBar1);
            this.pnlScrollBars.Controls.Add(this.pnlSpacer);
            this.pnlScrollBars.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlScrollBars.Location = new System.Drawing.Point(953, 40);
            this.pnlScrollBars.Name = "pnlScrollBars";
            this.pnlScrollBars.Size = new System.Drawing.Size(17, 374);
            this.pnlScrollBars.TabIndex = 3;
            // 
            // s12
            // 
            this.s12.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.s12.Location = new System.Drawing.Point(0, 266);
            this.s12.Name = "s12";
            this.s12.Size = new System.Drawing.Size(17, 2);
            this.s12.TabIndex = 8;
            this.s12.TabStop = false;
            this.s12.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g10SplitterMoved);
            this.s12.DoubleClick += new System.EventHandler(this.g10SplitterDoubleClick);
            // 
            // vScrollBar3
            // 
            this.vScrollBar3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar3.Location = new System.Drawing.Point(0, 158);
            this.vScrollBar3.Name = "vScrollBar3";
            this.vScrollBar3.Size = new System.Drawing.Size(17, 110);
            this.vScrollBar3.TabIndex = 7;
            this.vScrollBar3.ValueChanged += new System.EventHandler(this.vScrollBar3_ValueChanged);
            // 
            // vScrollBar4
            // 
            this.vScrollBar4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.vScrollBar4.Location = new System.Drawing.Point(0, 268);
            this.vScrollBar4.Name = "vScrollBar4";
            this.vScrollBar4.Size = new System.Drawing.Size(17, 89);
            this.vScrollBar4.TabIndex = 6;
            this.vScrollBar4.ValueChanged += new System.EventHandler(this.vScrollBar4_ValueChanged);
            // 
            // s8
            // 
            this.s8.Dock = System.Windows.Forms.DockStyle.Top;
            this.s8.Location = new System.Drawing.Point(0, 156);
            this.s8.Name = "s8";
            this.s8.Size = new System.Drawing.Size(17, 2);
            this.s8.TabIndex = 3;
            this.s8.TabStop = false;
            this.s8.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
            this.s8.DoubleClick += new System.EventHandler(this.g7SplitterDoubleClick);
            // 
            // vScrollBar2
            // 
            this.vScrollBar2.Dock = System.Windows.Forms.DockStyle.Top;
            this.vScrollBar2.Location = new System.Drawing.Point(0, 66);
            this.vScrollBar2.Name = "vScrollBar2";
            this.vScrollBar2.Size = new System.Drawing.Size(17, 90);
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
            // pnlSpacer
            // 
            this.pnlSpacer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSpacer.Location = new System.Drawing.Point(0, 357);
            this.pnlSpacer.Name = "pnlSpacer";
            this.pnlSpacer.Size = new System.Drawing.Size(17, 17);
            this.pnlSpacer.TabIndex = 4;
            // 
            // pnlTotals
            // 
            this.pnlTotals.Controls.Add(this.g8);
            this.pnlTotals.Controls.Add(this.s10);
            this.pnlTotals.Controls.Add(this.g11);
            this.pnlTotals.Controls.Add(this.s6);
            this.pnlTotals.Controls.Add(this.g5);
            this.pnlTotals.Controls.Add(this.s2);
            this.pnlTotals.Controls.Add(this.g2);
            this.pnlTotals.Controls.Add(this.hScrollBar2);
            this.pnlTotals.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTotals.Location = new System.Drawing.Point(81, 40);
            this.pnlTotals.Name = "pnlTotals";
            this.pnlTotals.Size = new System.Drawing.Size(85, 374);
            this.pnlTotals.TabIndex = 4;
            // 
            // g8
            // 
            this.g8.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g8.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g8.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g8.ExtendLastCol = true;
            this.g8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.g8.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g8.Location = new System.Drawing.Point(0, 156);
            this.g8.Name = "g8";
            this.g8.Rows.DefaultSize = 19;
            this.g8.Rows.Fixed = 0;
            this.g8.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g8.Size = new System.Drawing.Size(85, 110);
            this.g8.StyleInfo = resources.GetString("g8.StyleInfo");
            this.g8.TabIndex = 18;
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
            this.s10.Location = new System.Drawing.Point(0, 266);
            this.s10.Name = "s10";
            this.s10.Size = new System.Drawing.Size(85, 1);
            this.s10.TabIndex = 17;
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
            this.g11.Location = new System.Drawing.Point(0, 267);
            this.g11.Name = "g11";
            this.g11.Rows.DefaultSize = 17;
            this.g11.Rows.Fixed = 0;
            this.g11.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g11.Size = new System.Drawing.Size(85, 90);
            this.g11.TabIndex = 16;
            this.g11.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g11_BeforeScroll);
            this.g11.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g11.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g11.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g11.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g11_OwnerDrawCell);
            this.g11.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g11.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g11.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // s6
            // 
            this.s6.Dock = System.Windows.Forms.DockStyle.Top;
            this.s6.Location = new System.Drawing.Point(0, 155);
            this.s6.Name = "s6";
            this.s6.Size = new System.Drawing.Size(85, 1);
            this.s6.TabIndex = 8;
            this.s6.TabStop = false;
            this.s6.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
            this.s6.DoubleClick += new System.EventHandler(this.g7SplitterDoubleClick);
            // 
            // g5
            // 
            this.g5.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g5.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g5.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g5.Dock = System.Windows.Forms.DockStyle.Top;
            this.g5.ExtendLastCol = true;
            this.g5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.g5.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g5.Location = new System.Drawing.Point(0, 65);
            this.g5.Name = "g5";
            this.g5.Rows.DefaultSize = 19;
            this.g5.Rows.Fixed = 0;
            this.g5.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g5.Size = new System.Drawing.Size(85, 90);
            this.g5.StyleInfo = resources.GetString("g5.StyleInfo");
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
            this.g2.ColumnInfo = "10,0,0,0,0,75,Columns:0{AllowEditing:False;Style:\"ImageAlign:CenterTop;\";}\t";
            this.g2.Dock = System.Windows.Forms.DockStyle.Top;
            this.g2.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g2.ExtendLastCol = true;
            this.g2.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g2.Location = new System.Drawing.Point(0, 0);
            this.g2.Name = "g2";
            this.g2.Rows.DefaultSize = 17;
            this.g2.Rows.Fixed = 0;
            this.g2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g2.Size = new System.Drawing.Size(85, 64);
            this.g2.TabIndex = 0;
            this.g2.BeforeResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeResizeColumn);
            this.g2.AfterResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g2_AfterResizeColumn);
            this.g2.DragDrop += new System.Windows.Forms.DragEventHandler(this.g2_DragDrop);
            this.g2.DragEnter += new System.Windows.Forms.DragEventHandler(this.GridDragEnter);
            this.g2.DragOver += new System.Windows.Forms.DragEventHandler(this.GridDragOver);
            this.g2.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.GridQueryContinueDrag);
            this.g2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            this.g2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GridMouseMove);
            this.g2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GridMouseUp);
            // 
            // hScrollBar2
            // 
            this.hScrollBar2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar2.Location = new System.Drawing.Point(0, 357);
            this.hScrollBar2.Name = "hScrollBar2";
            this.hScrollBar2.Size = new System.Drawing.Size(85, 17);
            this.hScrollBar2.TabIndex = 4;
            this.hScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
            this.hScrollBar2.ValueChanged += new System.EventHandler(this.hScrollBar2_ValueChanged);
            // 
            // g2g3ContextMenu
            // 
            this.g2g3ContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFreezeColumn,
            this.mnuHeaderAllocationCriteria});
            // 
            // mnuFreezeColumn
            // 
            this.mnuFreezeColumn.Index = 0;
            this.mnuFreezeColumn.Text = "Freeze Column";
            this.mnuFreezeColumn.Click += new System.EventHandler(this.mnuFreezeColumn_Click);
            // 
            // mnuHeaderAllocationCriteria
            // 
            this.mnuHeaderAllocationCriteria.Index = 1;
            this.mnuHeaderAllocationCriteria.Text = "";
            // 
            // VerticalSplitter2
            // 
            this.VerticalSplitter2.Location = new System.Drawing.Point(166, 40);
            this.VerticalSplitter2.MinExtra = 0;
            this.VerticalSplitter2.MinSize = 0;
            this.VerticalSplitter2.Name = "VerticalSplitter2";
            this.VerticalSplitter2.Size = new System.Drawing.Size(1, 374);
            this.VerticalSplitter2.TabIndex = 5;
            this.VerticalSplitter2.TabStop = false;
            this.VerticalSplitter2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.VerticalSplitter2_SplitterMoved);
            this.VerticalSplitter2.DoubleClick += new System.EventHandler(this.VerticalSplitter2_DoubleClick);
            // 
            // pnlData
            // 
            this.pnlData.Controls.Add(this.g9);
            this.pnlData.Controls.Add(this.s11);
            this.pnlData.Controls.Add(this.g12);
            this.pnlData.Controls.Add(this.s7);
            this.pnlData.Controls.Add(this.g6);
            this.pnlData.Controls.Add(this.s3);
            this.pnlData.Controls.Add(this.g3);
            this.pnlData.Controls.Add(this.hScrollBar3);
            this.pnlData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlData.Location = new System.Drawing.Point(167, 40);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(786, 374);
            this.pnlData.TabIndex = 6;
            // 
            // g9
            // 
            this.g9.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g9.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g9.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g9.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g9.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g9.Location = new System.Drawing.Point(0, 156);
            this.g9.Name = "g9";
            this.g9.Rows.DefaultSize = 17;
            this.g9.Rows.Fixed = 0;
            this.g9.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g9.Size = new System.Drawing.Size(786, 110);
            this.g9.TabIndex = 19;
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
            this.s11.BackColor = System.Drawing.SystemColors.Window;
            this.s11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.s11.Location = new System.Drawing.Point(0, 266);
            this.s11.Name = "s11";
            this.s11.Size = new System.Drawing.Size(786, 1);
            this.s11.TabIndex = 18;
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
            this.g12.Location = new System.Drawing.Point(0, 267);
            this.g12.Name = "g12";
            this.g12.Rows.DefaultSize = 17;
            this.g12.Rows.Fixed = 0;
            this.g12.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g12.Size = new System.Drawing.Size(786, 90);
            this.g12.TabIndex = 16;
            this.g12.BeforeScroll += new C1.Win.C1FlexGrid.RangeEventHandler(this.g12_BeforeScroll);
            this.g12.BeforeEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeEdit);
            this.g12.StartEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridStartEdit);
            this.g12.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridAfterEdit);
            this.g12.OwnerDrawCell += new C1.Win.C1FlexGrid.OwnerDrawCellEventHandler(this.g12_OwnerDrawCell);
            this.g12.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridKeyDown);
            this.g12.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GridKeyPress);
            this.g12.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridMouseDown);
            // 
            // s7
            // 
            this.s7.Dock = System.Windows.Forms.DockStyle.Top;
            this.s7.Location = new System.Drawing.Point(0, 155);
            this.s7.Name = "s7";
            this.s7.Size = new System.Drawing.Size(786, 1);
            this.s7.TabIndex = 8;
            this.s7.TabStop = false;
            this.s7.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.g7SplitterMoved);
            this.s7.DoubleClick += new System.EventHandler(this.g7SplitterDoubleClick);
            // 
            // g6
            // 
            this.g6.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g6.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g6.ColumnInfo = "10,0,0,0,0,75,Columns:";
            this.g6.Dock = System.Windows.Forms.DockStyle.Top;
            this.g6.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g6.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None;
            this.g6.Location = new System.Drawing.Point(0, 65);
            this.g6.Name = "g6";
            this.g6.Rows.DefaultSize = 17;
            this.g6.Rows.Fixed = 0;
            this.g6.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g6.Size = new System.Drawing.Size(786, 90);
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
            this.s3.Size = new System.Drawing.Size(786, 1);
            this.s3.TabIndex = 10;
            this.s3.TabStop = false;
            this.s3.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.s3_SplitterMoved);
            this.s3.DoubleClick += new System.EventHandler(this.g4SplitterDoubleClick);
            // 
            // g3
            // 
            this.g3.AllowEditing = false;
            this.g3.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
            this.g3.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
            this.g3.ColumnInfo = "10,0,0,0,0,75,Columns:0{AllowEditing:False;Style:\"ImageAlign:CenterTop;\";}\t";
            this.g3.ContextMenu = this.g2g3ContextMenu;
            this.g3.Dock = System.Windows.Forms.DockStyle.Top;
            this.g3.DropMode = C1.Win.C1FlexGrid.DropModeEnum.Manual;
            this.g3.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus;
            this.g3.Location = new System.Drawing.Point(0, 0);
            this.g3.Name = "g3";
            this.g3.Rows.DefaultSize = 17;
            this.g3.Rows.Fixed = 0;
            this.g3.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.g3.Size = new System.Drawing.Size(786, 64);
            this.g3.TabIndex = 0;
            this.g3.BeforeResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.GridBeforeResizeColumn);
            this.g3.AfterResizeColumn += new C1.Win.C1FlexGrid.RowColEventHandler(this.g3_AfterResizeColumn);
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
            // hScrollBar3
            // 
            this.hScrollBar3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar3.Location = new System.Drawing.Point(0, 357);
            this.hScrollBar3.Name = "hScrollBar3";
            this.hScrollBar3.Size = new System.Drawing.Size(786, 17);
            this.hScrollBar3.TabIndex = 4;
            this.hScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar3_Scroll);
            this.hScrollBar3.ValueChanged += new System.EventHandler(this.hScrollBar3_ValueChanged);
            // 
            // SummaryView
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(970, 414);
            this.Controls.Add(this.pnlData);
            this.Controls.Add(this.VerticalSplitter2);
            this.Controls.Add(this.pnlTotals);
            this.Controls.Add(this.pnlScrollBars);
            this.Controls.Add(this.VerticalSplitter1);
            this.Controls.Add(this.pnlRowHeaders);
            this.Controls.Add(this.pnlTop);
            this.MinimumSize = new System.Drawing.Size(0, 300);
            this.Name = "SummaryView";
            this.Activated += new System.EventHandler(this.SummaryView_Activated);
            this.Deactivate += new System.EventHandler(this.SummaryView_Deactivate);
            this.Load += new System.EventHandler(this.SummaryView_Load);
            this.Controls.SetChildIndex(this.pnlTop, 0);
            this.Controls.SetChildIndex(this.pnlRowHeaders, 0);
            this.Controls.SetChildIndex(this.VerticalSplitter1, 0);
            this.Controls.SetChildIndex(this.pnlScrollBars, 0);
            this.Controls.SetChildIndex(this.pnlTotals, 0);
            this.Controls.SetChildIndex(this.VerticalSplitter2, 0);
            this.Controls.SetChildIndex(this.pnlData, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.pnlRowHeaders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g1)).EndInit();
            this.pnlScrollBars.ResumeLayout(false);
            this.pnlTotals.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g2)).EndInit();
            this.pnlData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g3)).EndInit();
            // START TT#5034 - AGallagher - In Style, Size, Summary Review Screen the Tool Tips do not appear for Attribute Set, View, Filter, and Method-Action Windows.  
            this.cmbAttributeSet.SetToolTip = "Attribute Sets";
            this.cmbFilter.SetToolTip = "Filters";
            this.cboAction.SetToolTip = "Actions";
            // END TT#5034 - AGallagher - In Style, Size, Summary Review Screen the Tool Tips do not appear for Attribute Set, View, Filter, and Method-Action Windows.
            this.ResumeLayout(false);

		}
		#endregion

		#region Variable Declarations
		private bool _hSplitMove;
//		private bool _isSorting;	
		private int DragStartColumn; //indicates which column is the column that started the drag/drop action.
		private DragState dragState;
		private static int BIGCHANGE = 5;	
		private static int SMALLCHANGE = 1;  
		private const int BIGHORIZONTALCHANGE = 5;
		private int WeekOrVariableGroups = 1; //This variable has no use in this form and will always have a value of 1. 
		private int rowsPerStoreGroup = 1; 
		private int LastInvisibleRowsPerStoreGroup4; //some rows might be hidden. this is used mainly for determining the border-bearing cell. (which cell need to display a border)
		private int LastInvisibleRowsPerStoreGroup7;//some rows might be hidden. this is used mainly for determining the border-bearing cell. (which cell need to display a border)
		private int LastInvisibleRowsPerStoreGroup10 = 0;
		private FromGrid RightClickedFrom; //indicates which grid the user right clicked from.
		private string ImageDir;
		private System.Drawing.Bitmap picLock; //this picture will be put in a cell that's locked.
		private System.Drawing.Bitmap picStyle; //this picture is put on the "Theme Properties" button.
		private bool g1HasColsFrozen;
		private bool g2HasColsFrozen;
		private bool g3HasColsFrozen;
		private int LeftMostColBeforeFreeze1;
		private int LeftMostColBeforeFreeze2;
		private int LeftMostColBeforeFreeze3;
		private int UserSetSplitter1Position;
		private int UserSetSplitter2Position; //used to determine the width the user set the TOTALS panel.
		//		private CubeWaferCoordinateList[] ColTagCoorList;
		//		private CubeWaferCoordinateList[] RowTagCoorList;
		//private ArrayList RowHeaders4 ;
		//private ArrayList RowHeaders7 ;
		private ApplicationSessionTransaction _trans;
		private SessionAddressBlock _sab;
        private ExplorerAddressBlock _eab;

		//the following variables are used for getting data from the "cube"
		//StorePlanMaintCubeGroup _storeMaintCubeGroup;

		//CubeWaferCoordinateList _commonWaferCoordinateList;

		//System.Threading.Thread _pageThread5;
		//System.Threading.Thread _pageThread6;
		//System.Threading.Thread _pageThread8;
		//System.Threading.Thread _pageThread9;
		//System.Threading.Thread _pageThread11;
		//System.Threading.Thread _pageThread12;
		//System.Threading.Thread _styleThread4;
		//System.Threading.Thread _styleThread5;
		//System.Threading.Thread _styleThread6;
		//System.Threading.Thread _styleThread7;
		//System.Threading.Thread _styleThread8;
		//System.Threading.Thread _styleThread9;
		
		//The following variables are used to store the location of mouse clicks
		//for various grids, because gX.MouseCol and gX.MouseRow don't always work.
		//One simple example to show the need for these variables is the case of 
		//context menus. The user right clicks on a cell, but have to move the mouse
		//furthur down the screen in order to reach a menu item. This action
		//affects the location of the mouse (obviously).
		//These locations will be caught in the "MouseDown" events of diff. grids.
		private int GridMouseRow;
		private int GridMouseCol;
				
		private ThemeProperties _frmThemeProperties; //for the properties dialog box.
		private Theme _theme;
	
		private bool _loading;
		private int _rowsPerGroup4;
		private int _rowsPerGroup7;
		private int _rowsPerGroup10;
		private int _colsPerGroup1 = 1;
		private int _colsPerGroup2 = 1;
		private int _colsPerGroup3 = 3;
		private object _holdValue;
		ScrollEventType _scroll2EventType;  
		ScrollEventType _scroll3EventType;  
		AllocationWaferGroup _wafers;
		AllocationWafer _wafer;
		AllocationWaferCell [,] _cells ;
		private bool _setHeaderTags = true;
		private bool _resetV1Splitter;
		private int _curGroupBy = Include.Undefined;
		private int _curAttribute = Include.Undefined;
		private int _curAttributeSet = Include.Undefined;
		private bool _initalLoad;
		private const string _invalidCellValue = " "; 
		private const int _varProfIndex = 2;
		private bool _rbToggled;
		private int _curg3LeftCol;
		private string _showCol;
		private int _minFrozenCols = 3;
		private bool _isScrolling;
		private MIDRetail.Windows.AllocationWorkspaceExplorer _awExplorer;
		private AllocationHeaderProfileList _headerList;
//		private Hashtable _readOnlyList = null;
//		private bool _bindingFilter;
		private int _lastFilterValue;
//		private DataTable _dtFilterType = null;
//		private DataView _dvFilterType;
		private QuickFilter _quickFilter;
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
		private string _lblHeader = null;
		private string _lblColor = null;
		private string _lblPack = null;
		private string _lblComponent = null;
		private string _lblAttribute = null;
		private string _lblAttributeSet = null;
		private string _lblStoreGrade = null;
		private string _lblSet = null;
		private string _lblCondition = null;
		private string _lblVariable = null;
		private string _lblValue = null;
		private string _totalVariableName;
		private eQuickFilterType _quickFilterType;
		private int _foundColumn;
		private int _foundRow;
		private ProfileList _attrSetProfileList;
		private ArrayList _componentList = null;
		private ArrayList _gradeList = null;
		private QuickFilterData _quickFilterData;
		private int _hdrRow = 0;
		private int _compRow = 1;
		private FunctionSecurityProfile _allocationReviewStyleSecurity;
		private FunctionSecurityProfile _allocationReviewSizeSecurity;
        private FunctionSecurityProfile _allocationReviewAssortmentSecurity;
		private int _changedCellRow;
		private int _changedCellCol;
		private C1.Win.C1FlexGrid.C1FlexGrid _changedGrid = null;
        private string _includeCurrentSetLabel = null;
        private string _includeAllSetsLabel = null;
		private string _windowName;
        private AllocationWaferCellChangeList _allocationWaferCellChangeList; // TT#59 Implement Temp Locks
        private System.Windows.Forms.MenuItem mnuHeaderAllocationCriteria;    // TT#59 Implement Temp Locks
        #endregion
		
		private void HandleMIDException(MIDException MIDexc)
		{
			string Title, errLevel, Msg; 
			MessageBoxIcon icon;
			MessageBoxButtons buttons;
			buttons = MessageBoxButtons.OK;
			switch (MIDexc.ErrorLevel)
			{
				case eErrorLevel.severe:
					icon = MessageBoxIcon.Stop;
					errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Severe));
					break;
				
				case eErrorLevel.information:
					icon = MessageBoxIcon.Information;
					errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Information));
					break;
				
				case eErrorLevel.warning:
					icon = MessageBoxIcon.Warning;
					errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Warning));
					break;

                //Begin TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
                case eErrorLevel.error:
                    icon = MessageBoxIcon.Error;
                    errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Error));
                    break;

                //End TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
                default:
					icon = MessageBoxIcon.Stop;
					errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Severe));
					break;
			}
			if (MIDexc.InnerException != null)
			{
				Title = errLevel + " - " + MIDexc.Message;
				Msg = MIDexc.InnerException.Message;
			}
			else
			{
				Title = errLevel;
				Msg = MIDexc.Message;
			}
			MessageBox.Show(this, Msg, Title,
				buttons, icon );
		}
		protected void FormatForXP( Control ctl )
		{
			try
			{
				foreach ( Control c in ctl.Controls )
					FormatForXP(c);
	
				if ( ctl.GetType().BaseType == typeof(ButtonBase) )
					((ButtonBase)ctl).FlatStyle = FlatStyle.System;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}


        public SummaryView(ExplorerAddressBlock eab, ApplicationSessionTransaction trans): base(trans.SAB)
		{
			_trans = trans;
			_sab = _trans.SAB;
            _eab = eab;
			_theme = _sab.ClientServerSession.Theme;
			_awExplorer = (MIDRetail.Windows.AllocationWorkspaceExplorer)_trans.AllocationWorkspaceExplorer;
			InitializeComponent();
			_curGroupBy = _trans.AllocationGroupBy ;
			_curAttribute    = _trans.AllocationStoreAttributeID;
			_curAttributeSet = _trans.AllocationStoreGroupLevel;
            _allocationWaferCellChangeList = new AllocationWaferCellChangeList();  // TT#59 Implement Temp Locks
        }
		private void SummaryView_Load(object sender, System.EventArgs e)
		{		
			try
			{
                // begin TT#59 Implement Store Temp Locks
                //
                // mnuHeaderAllocationCriteria
                //
                if (this.SAB.AllowDebugging)
                {
                    this.mnuHeaderAllocationCriteria.Index = 1;
                    this.mnuHeaderAllocationCriteria.Text = "Header Allocation Criteria";
                    this.mnuHeaderAllocationCriteria.Click += new System.EventHandler(this.mnuHeaderAllocationCriteria_Click);
                }
                // end TT#59 Implement Store Temp Locks

                _includeAllSetsLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeAllSets);
                _includeCurrentSetLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeCurrentSet);
				FunctionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSummary);
				
				// BEGIN MID Track #2551 - security not working
				_allocationReviewStyleSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewStyle);
                // Begin TT#2 - JSmith - Assortment Security
                //_allocationReviewAssortmentSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewAssortment);
                _allocationReviewAssortmentSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReview);
                // End TT#2
                if (_sab.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
					_allocationReviewSizeSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSize);
				// END MID Track #2551

				_windowName = MIDText.GetTextOnly(eMIDTextCode.frm_SummaryReview);
			
				_loading = true;
				_initalLoad = true;
				Cursor.Current = Cursors.WaitCursor;

				BuildMenu();
 
				GetSelectionCriteria();

				g1HasColsFrozen = false;
				g2HasColsFrozen = false;
				g3HasColsFrozen = false;

				//The following 6 lines are used to allow manually drawing borders around cells.
				g3.DrawMode=DrawModeEnum.OwnerDraw;
				g4.DrawMode=DrawModeEnum.OwnerDraw;
				g5.DrawMode=DrawModeEnum.OwnerDraw;
				g6.DrawMode=DrawModeEnum.OwnerDraw;
				g7.DrawMode=DrawModeEnum.OwnerDraw;
				g8.DrawMode=DrawModeEnum.OwnerDraw;
				g9.DrawMode=DrawModeEnum.OwnerDraw;
				g10.DrawMode=DrawModeEnum.OwnerDraw;
				g11.DrawMode=DrawModeEnum.OwnerDraw;
				g12.DrawMode=DrawModeEnum.OwnerDraw;
				
				//Bind and select combo boxes
				BindStoreAttrComboBox();
				BindFilterComboBox();
				BindActionCombo();
				if ( _trans.AllocationGroupBy == Convert.ToInt32(eAllocationSummaryViewGroupBy.Attribute, CultureInfo.CurrentUICulture) )
				{	
					rbGroupByAttribute.Checked = true;
					cmbAttributeSet.Enabled = false;
				}
				else
				{
					rbGroupByStoreGrade.Checked = true;
				} 
                //Begin Track #5858 - Kjohnson - Validating store security only
              //cmbStoreAttribute.Tag = "IgnoreMouseWheel";
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cmbStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cmbStoreAttribute, eMIDControlCode.form_SummaryReview);
                //cmbFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cmbFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                // Begin TT#1118 - JSmith - Undesirable curser position
                //cmbStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cmbStoreAttribute, eMIDControlCode.form_SummaryReview, FunctionSecurity, true);
                cmbStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cmbStoreAttribute, eMIDControlCode.form_SummaryReview, true, FunctionSecurity, true);
                
                //cmbFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cmbFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, true);
                cmbFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cmbFilter.ComboBox, eMIDControlCode.field_Filter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, true, FunctionSecurity, true);
                // End TT#1118

                // End TT#44
                //Begin Track #5858 - Kjohnson
				cmbAttributeSet.Tag = "IgnoreMouseWheel";
				cboAction.Tag = "IgnoreMouseWheel";
				
				//This picture will be put in cells to visually indicate to the user
				//that this cell is locked (cannot be "spread" to new data).
                // Begin TT#588-MD - JSmith - Remove ApplicationRoot from configuration
                //ImageDir = MIDConfigurationManager.AppSettings[Include.MIDApplicationRoot] + MIDGraphics.GraphicsDir;
                ImageDir = MIDGraphics.MIDGraphicsDir;
                // End TT#588-MD - JSmith - Remove ApplicationRoot from configuration
				picLock = new Bitmap(ImageDir + "\\lock.gif");
				picStyle = new Bitmap(ImageDir + "\\style.gif");
							
				g4.Cols[0].TextAlign = TextAlignEnum.LeftTop;
				g7.Cols[0].TextAlign = TextAlignEnum.LeftTop;
				_lblSet = MIDText.GetTextOnly(eMIDTextCode.lbl_Set);
				FormatGrids1to12();

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
				// BEGIN MID Track #2551 - security not working
				// since read only may protect the radio buttons, need to always unprotect 
				this.rbGroupByStoreGrade.Enabled = true;
				this.rbGroupByAttribute.Enabled = true; 

				SetActionListEnabled();
				if (_trans.AnalysisOnly)
				{
					btnApply.Enabled = false;
				}	
				else
					btnApply.Enabled = (g8.Rows.Count > 0 && (_trans.DataState == eDataState.Updatable));
				// END MID Track #2551

				// Format for XP, if applicable
				if ( Environment.OSVersion.Version.Major > 4 && Environment.OSVersion.Version.Minor > 0 && System.IO.File.Exists( Application.ExecutablePath + ".manifest") )
					FormatForXP(this);
				 
				SaveCurrentSettings();
				if (rbGroupByStoreGrade.Checked)
				{
					g3.Cols.Frozen = _minFrozenCols;
					g6.Cols.Frozen = _minFrozenCols;
					g9.Cols.Frozen = _minFrozenCols;
					g12.Cols.Frozen = _minFrozenCols;
				}

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
				Cursor.Current = Cursors.Default;
			}
			catch (Exception ex)
			{
				HandleException(ex);
				Cursor.Current = Cursors.Default;
			}
		}
 	
		// Begin MID Track 4858 - JSmith - Security changes
//		private void BuildMenu()
//		{
//			try
//			{
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
//				btQuickFilter = new ButtonTool("btQuickFilter");
//				btQuickFilter.SharedProps.Caption = "&Quick Filter";
//				btQuickFilter.SharedProps.Shortcut = Shortcut.CtrlQ;
//				btQuickFilter.SharedProps.MergeOrder = 13;
//				utmMain.Tools.Add(btQuickFilter);
//				toolsMenuTool.Tools.Add(btQuickFilter);
//			
//			}
//			catch (Exception exc)
//			{
//				HandleException(exc);
//			}
//		}

		private void BuildMenu()
		{
			try
			{
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
			this.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Style)
				+ " "  
				+ MIDText.GetTextOnly(eMIDTextCode.frm_SummaryReview);
				//+ " Review";
			_thisTitle = this.Text;
			if (_trans.AnalysisOnly)
				this.Text = this.Text + " - " + "Analysis only";
			else if (_trans.AllocationCriteriaHeaderCount > 1)
				this.Text = this.Text + " *";
			else
			{
				AllocationWafer wafer = _wafers[1,0];
				this.Text = this.Text + " - " + wafer.RowLabels[0,0];				 
			}
			if (_trans.DataState == eDataState.ReadOnly)
				this.Text = this.Text +   " Read Only";

			rbGroupByAttribute.Text = MIDText.GetTextOnly((int)eAllocationSummaryViewGroupBy.Attribute);
			rbGroupByStoreGrade.Text = MIDText.GetTextOnly((int)eAllocationSummaryViewGroupBy.StoreGrade);
			_lblStoreGrade = rbGroupByStoreGrade.Text;
			_lblAttribute = MIDText.GetTextOnly(eMIDTextCode.lbl_Attribute);			 
			_lblAttributeSet = MIDText.GetTextOnly(eMIDTextCode.lbl_AttributeSet);			 
			this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
			this.btnApply.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Apply);
		}	
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
					_resetV1Splitter = true;     
					_trans.AllocationStoreAttributeID = Convert.ToInt32(cmbStoreAttribute.SelectedValue, CultureInfo.CurrentUICulture);
					ChangePending = true;
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
				//ProfileList pl = _sab.StoreServerSession.GetStoreGroupLevelList(Convert.ToInt32(key, CultureInfo.CurrentUICulture));
//Begin Track #3767 - JScott - Force client to use cached store group lists in application session
//				_attrSetProfileList = _sab.StoreServerSession.GetStoreGroupLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture));
				// BEGIN MID Track #3834 
				//_attrSetProfileList = _sab.ApplicationServerSession.GetStoreGroupLevelListViewProfileList(Convert.ToInt32(key, CultureInfo.CurrentUICulture), false);
				// END MID Track #3834
//End Track #3767 - JScott - Force client to use cached store group lists in application session
				
				// BEGIN MID Track #3834 - error when more than 1 view open and attribute or set is changed
				//Begin TT#1517-MD -jsobek -Store Service Optimization
				//ProfileList pl = _sab.ApplicationServerSession.GetStoreGroupLevelListViewProfileList(Convert.ToInt32(key, CultureInfo.CurrentUICulture), false);
                ProfileList pl = StoreMgmt.StoreGroup_GetLevelListViewList(Convert.ToInt32(key, CultureInfo.CurrentUICulture), false);
				//End TT#1517-MD -jsobek -Store Service Optimization
				object obj = pl.Clone();
				_attrSetProfileList = (ProfileList)obj;
				// END MID Track #3834
			
				this.cmbAttributeSet.ValueMember = "Key";
				this.cmbAttributeSet.DisplayMember = "Name";
				//this.cmbAttributeSet.DataSource = pl.ArrayList;
				this.cmbAttributeSet.DataSource = _attrSetProfileList.ArrayList;
                //this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs()); // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected

				if (this.cmbAttributeSet.Items.Count > 0)
                //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                {   
                    int indexCurrent = this.cmbAttributeSet.SelectedIndex; //Current Index.

                    if (rbGroupByAttribute.Checked)
                        this.cmbAttributeSet.SelectedIndex = -1;
                    else
                    {
                        if (_loading && _trans.AllocationCriteriaExists)
                        {
                            this.cmbAttributeSet.SelectedValue = _trans.AllocationStoreGroupLevel;
                        }
                        else
                        {
                            this.cmbAttributeSet.SelectedIndex = 0;
                        }
                    }

                    int indexChk = this.cmbAttributeSet.SelectedIndex;  //New index if changed.
                    if (indexChk != indexCurrent)
                    {
                        //this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs()); // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected
                    }
                    //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                }
			   // AdjustTextWidthComboBox_DropDown(cmbAttributeSet);  // TT#1701 - RMatelic
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cmbAttributeSet_EnabledChanged(object sender, System.EventArgs e)
		{
            //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
            int indexCurrent = this.cmbAttributeSet.SelectedIndex; //Current Index.
            if (this.cmbAttributeSet.Enabled && this.cmbAttributeSet.Items.Count > 0)
                this.cmbAttributeSet.SelectedIndex = 0;

            else
            {
                this.cmbAttributeSet.SelectedIndex = -1;
            }
		    int indexChk = this.cmbAttributeSet.SelectedIndex;  //New index if changed.
           if (indexChk != indexCurrent)
           {
               //this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs());
               // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected
           }
            //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
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
                ProfileList al = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.MyUserAndGlobal, true);  // TT#1517-MD - Store Service Optimization - SRISCH - Changed from ALL
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
		
				// BEGIN MID Track #4664 - error changing attribute from Velocity
				// Solution: protect attribute drop down to be consistent with StyleView
                //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                int indexCurrent = this.cmbStoreAttribute.SelectedIndex; //Current Index.
				if (_trans.VelocityCriteriaExists)
				{
					this.cmbStoreAttribute.SelectedValue = _trans.VelocityStoreGroupRID;
					this.cmbStoreAttribute.Enabled = false;
				}
				else // END MID Track #4664 
				{
					if (_trans.AllocationCriteriaExists)
						this.cmbStoreAttribute.SelectedValue = _trans.AllocationStoreAttributeID;
					else
						this.cmbStoreAttribute.SelectedIndex = 0;
				}
                int indexChk = this.cmbStoreAttribute.SelectedIndex;  //New index if changed.
                if (indexChk != indexCurrent)
                {
                    //this.cmbStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
                    // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected
                }
                //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                AdjustTextWidthComboBox_DropDown(cmbStoreAttribute);  // TT#1401 - AGallagher - Reservation Stores
			}
			catch (Exception ex)
			{
				HandleException(ex);	
			}
		}
		private void BindFilterComboBox()
		{
			try
			{
//				_bindingFilter = true;

				_lastFilterValue = _trans.AllocationFilterID;
				cmbFilter.Items.Clear();
				cmbFilter.Items.Add(new FilterNameCombo(-1, Include.GlobalUserRID, "(None)"));  // Issue 3806

				foreach (DataRow row in _trans.AllocationFilterTable.Rows)
				{
					cmbFilter.Items.Add(
						new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture))); // TT#7 - RBeck - Dynamic dropdowns
				}

				cmbFilter.SelectedItem = new FilterNameCombo(_trans.AllocationFilterID);
				//   AdjustTextWidthComboBox_DropDown(cmbFilter);  // TT#1401 - AGallagher - Reservation Stores //  TT#7 - RBeck - Dynamic dropdowns
//				_bindingFilter = false;
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

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
				//PopulateCommonCriteria((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BreakoutSizesAsReceivedWithConstraints, this.cboAction.ComboBox, String.Empty); // TT#1391 - Jellis - Balance Size With Constraint Other Options
                PopulateCommonCriteria((int)eAllocationActionType.StyleNeed, (int)eAllocationActionType.BalanceToVSW, this.cboAction.ComboBox, String.Empty); // TT#1334-MD - stodd - Balance to VSW Action
                // end TT#794 - New Size Balance for Wet Seal
                // end TT#843 - New Size Constraint Balance
                // End TT#785
				// AdjustTextWidthComboBox_DropDown(cboAction);  // TT#1401 - AGallagher - Reservation Stores // TT#7 - RBeck - Dynamic dropdowns
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
				    removeEntry.Add(Convert.ToInt32(eAllocationActionType.BreakoutSizesAsReceivedWithConstraints), eAllocationActionType.BreakoutSizesAsReceivedWithConstraints); // TT#1391 - JEllis - Balance Size With Constraint Other Options
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
                //this.cboAction_SelectionChangeCommitted(source, new EventArgs()); // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected

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

		#endregion 
		#region Get Allocation Selection Criteria 
		private void GetSelectionCriteria()
		{
			bool showNeedGrid; 
			try
			{
				if (_trans.AllocationCriteriaExists)
				{
					_trans.UpdateAllocationViewSelectionHeaders();

                    _headerList = (AllocationHeaderProfileList)_trans.GetMasterProfileList(eProfileType.AllocationHeader); // TT#1185 - Verify ENQ before Update
                    // BEGIN MID Track #2551 - security not working
                    // Begin Track #6404 - JSmith - 80302: Units to allocate is calculated when header is Work Up Total Buy
                    //if (FunctionSecurity.AllowUpdate && _trans.HeadersEnqueued)
                    //if (FunctionSecurity.AllowUpdate && _trans.HeadersEnqueued && !_trans.AnalysisOnly) // TT#1185 - Verify ENQ before Update
                    if (FunctionSecurity.AllowUpdate                                                      // TT#1185 - Verify ENQ before Update
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
					
					_wafers = _trans.AllocationWafers;
					_trans.SummaryView = this;
					
					// Although this window doesn't use this switch, it is necessary
					// to have _trans go thru the routine
					showNeedGrid = _trans.AllocationStyleViewIncludesNeedAnalysis;
                    //_headerList = (AllocationHeaderProfileList)_trans.GetMasterProfileList(eProfileType.AllocationHeader); // TT#1185 - Verify ENQ before Update
				}
			}
			catch (Exception)
			{
//				HandleException(ex);
				throw;
			}
		}
		#endregion
		#region Format Grids
		private void FormatGrids1to12()
		{
			try
			{
				Formatg1Grid();
				Formatg2Grid();
				Formatg3Grid();
				//if (_showCol == "left")
				//	_curg3LeftCol = g3.LeftCol;
				FormatGrid4_10 (FromGrid.g4);
				FormatGrid5_12 (FromGrid.g5);
				FormatGrid5_12 (FromGrid.g6);
				FormatGrid4_10 (FromGrid.g7);
				FormatGrid5_12 (FromGrid.g8);
				FormatGrid5_12 (FromGrid.g9);
				FormatGrid4_10 (FromGrid.g10);
				FormatGrid5_12 (FromGrid.g11);
				FormatGrid5_12 (FromGrid.g12);
				
				//SortToDefault(); //for some reason the first time doesn't sort right. Look into this issue later.
				AssignTag();
				ChangeStyle();	
				ApplyPreferences();
				 
				UserSetSplitter1Position = VerticalSplitter1.SplitPosition;
				UserSetSplitter2Position = VerticalSplitter2.SplitPosition;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void Formatg1Grid()
		{
			int i, j;
			// Independent header grid not related to wafers			
			g1.Rows.Count = 2;
			g1.Cols.Count = 2;
			g1.Cols.Fixed = 0;
			g1.Rows.Fixed = 2;	
			 
			if (g1.Tag == null)
			{
				g1.Tag = new GridTag(Convert.ToInt32(FromGrid.g1, CultureInfo.CurrentUICulture), null, null);
			}
			for (i = 0; i < g1.Cols.Count; i++)
			{
				TagForColumn ColTag = new TagForColumn();
				g1.Cols[i].UserData = ColTag;
			}
			for ( i = 0; i < g1.Rows.Count; i++)
			{
				for ( j = 0; i < g1.Cols.Count; i++)
				{
					if (i == 0)
						g1.SetData(i,j,_lblSet);
					else
						g1.SetData(i,j,_invalidCellValue);
				}
			}
			
			g1.AllowDragging = AllowDraggingEnum.None;
			g1.AllowMerging = AllowMergingEnum.RestrictCols;
			for (i = 0; i < g1.Rows.Count; i++)
			{
				g1.Rows[i].AllowMerging = true;
			}
			 
			g1.AutoSizeCols(0, 0, g1.Rows.Count - 1, g1.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.None);
		}
		private void Formatg2Grid()
		{
			int i, j, k; 
			string [,] g2GridValues;

			if (g2.Tag == null)
			{
				g2.Tag = new GridTag(Convert.ToInt32(FromGrid.g2, CultureInfo.CurrentUICulture), null, null);
			}
			
			_wafer = _wafers[0,0];
			_cells = _wafer.Cells;
		   
			string[,] ColLabels = _wafers[0,0].ColumnLabels;
 			
			g2GridValues = new string[2,_wafer.Columns.Count];

			for (i = 0; i < 2; i++)
			{
				for (k=0; k < _wafer.Columns.Count; k++)
				{
					//wafercoordlist = (AllocationWaferCoordinateList)wafer.Columns[k];
					//wafercoord = (AllocationWaferCoordinate)wafercoordlist[i];
					g2GridValues[i,k] = ColLabels[i,k];
					//ParseColumnHeading(ref g2GridValues[i,k],wafercoord.CoordinateType,wafercoord.CoordinateSubType);
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
			}
			for (i = 0; i < g2.Rows.Count; i++)
			{
				g2.Rows[i].AllowMerging = true;
				for (j = 0; j < g2.Cols.Count; j++)
				{
					g2.SetData(i, j, g2GridValues[i,j]);
				}
			}
			g2.Rows[0].TextAlign = TextAlignEnum.RightBottom;
			g2.Rows[1].TextAlign = TextAlignEnum.RightBottom;
			g2.AutoSizeCols(0, 0, g2.Rows.Count - 1, g2.Cols.Count - 1, 0, AutoSizeFlags.None);

		}
		private void Formatg3Grid()
		{
			int i, j, k; 
			string [,] g3GridValues;
				
			if (g3.Tag == null)
			{
				g3.Tag = new GridTag(Convert.ToInt32(FromGrid.g3, CultureInfo.CurrentUICulture), null, null);
			}
			_wafer = _wafers[0,1];
			_cells = _wafer.Cells;
			string[,] ColLabels = _wafers[0,1].ColumnLabels;
			
			g3GridValues = new string[2,_wafer.Columns.Count];
			_gradeList = new ArrayList();
			for (i = 0; i < 2; i++)
			{
				for (k=0; k < _wafer.Columns.Count; k++)
				{
					if (rbGroupByAttribute.Checked)
						g3GridValues[i,k] = ColLabels[i,k];
					else
					{
						g3GridValues[i,k] = ColLabels[i+1,k];
						if ( i == 0 && k > 2 )
						{
							bool gradeFound = false;
							string grade;
							for (j = 0; j < _gradeList.Count; j++)	
							{
								grade = Convert.ToString(_gradeList[j],CultureInfo.CurrentUICulture); 
								if (grade == g3GridValues[i,k])
								{
									gradeFound = true;
									break;
								}
							}
							if (!gradeFound)
								_gradeList.Add(g3GridValues[i,k]);
						}
					}	
				}
			}
			g3.Rows.Count = 2;	
			g3.Cols.Count = _wafer.Columns.Count;
			g3.Cols.Fixed = 0;
			g3.Rows.Fixed = _wafer.Rows.Count;
			g3.AllowDragging = AllowDraggingEnum.None;
			g3.AllowMerging = AllowMergingEnum.RestrictCols;
			
			for (i = 0; i < g3.Cols.Count; i++)
			{
				TagForColumn colTag = new TagForColumn();
				colTag.CubeWaferCoorList = (AllocationWaferCoordinateList)_wafer.Columns[i];
				colTag.cellColumn = i;
				g3.Cols[i].UserData = colTag;
			}
			
			for (i = 0; i < g3.Rows.Count; i++)
			{
				g3.Rows[i].AllowMerging = true;
				for (j = 0; j < g3.Cols.Count; j++)
				{
					g3.SetData(i, j, g3GridValues[i,j]);
				}
			}
			g3.Rows[0].TextAlign = TextAlignEnum.CenterBottom;
			g3.Rows[1].TextAlign = TextAlignEnum.CenterBottom;
			g3.AutoSizeCols(0, 0, g3.Rows.Count - 1, g3.Cols.Count - 1, 0, AutoSizeFlags.None);
		}

		private void FormatGrid4_10 (FromGrid aGrid)
		{
			int i, j; 
			//decimal decVal;
			C1FlexGrid grid = null;	
			AllocationWafer wafer = null;
			//AllocationWaferCoordinateList wafercoordlist;
			//AllocationWaferCoordinate wafercoord;
			//AllocationWaferVariable varProf;
	
			switch(aGrid)
			{
				case FromGrid.g4:
					wafer = _wafers[0,0];
					grid = g4;
					break;
				case FromGrid.g7:
					wafer = _wafers[1,0];
					grid = g7;
					_componentList = new ArrayList();
					break;
				case FromGrid.g10:
					wafer = _wafers[2,0];
					grid = g10;
					break;
			}
			string[,] RowLabels = wafer.RowLabels;
			_cells = wafer.Cells;

			grid.Rows.Count = wafer.Rows.Count;
			
			grid.Cols.Count = 2;
			grid.Cols.Fixed = 0;
			grid.Rows.Fixed = 0;
			if (grid.Tag == null)
			{
				grid.Tag = new GridTag(Convert.ToInt32(aGrid, CultureInfo.CurrentUICulture), null, null);
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
			//grid.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
			grid.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free;
			grid.Cols[0].AllowMerging = true;
			for (i = 0; i < wafer.Rows.Count; i++)
			{
				TagForRow rowTag = new TagForRow();
				rowTag.CubeWaferCoorList = (AllocationWaferCoordinateList)wafer.Rows[i];
				grid.Rows[i].UserData = rowTag;
				for (j = 0; j < grid.Cols.Count; j++)
				{
					if (RowLabels[i,j] == String.Empty)
						grid.SetData(i, j, _invalidCellValue);
					else
					{
						grid.SetData(i, j, RowLabels[i,j]);
						if (grid == g7 && j == 1) 
							AddToComponentList(RowLabels[i,j]);
					}
				}
				//				for (j = 1; j <= wafer.Columns.Count; j++)
				//				{
				//					if (cells[i,j - 1].CellIsValid)
				//					{
				//						wafercoordlist = (AllocationWaferCoordinateList)wafer.Columns[j - 1];
				//						wafercoord = (AllocationWaferCoordinate)wafercoordlist[_varProfIndex];
				//						varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord.Key );
				//					 
				//						if (varProf.Format == eAllocationWaferVariableFormat.String)
				//							grid.SetData(i, j, cells[i,j - 1].ValueAsString);
				//						else if (grid.Cols[j].DataType == typeof(System.Decimal))
				//						{   // the * 1.00m adds the 2 decimal places (.00) to whole numbers without decimal digits
				//							decVal = (Convert.ToDecimal(cells[i,j - 1].Value, CultureInfo.CurrentUICulture) * 1.00m);
				//							grid.SetData(i, j, Decimal.Round(decVal,varProf.NumDecimals));
				//						}	
				//						else	
				//							grid.SetData(i, j, cells[i,j - 1].Value);
				//					}
				//					else
				//						grid.SetData(i, j, _invalidCellValue);
				//				}
				//				grid.SetData(i, j, _invalidCellValue);
			}
			grid.AutoSizeCols(0, 0, grid.Rows.Count - 1, grid.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);
		}
		private void AddToComponentList(string aLabel)
		{
			bool compFound = false;
			string comp; 
			for (int i = 0; i < _componentList.Count; i++)	
			{
				comp = Convert.ToString(_componentList[i],CultureInfo.CurrentUICulture); 
				if (comp == aLabel)
				{
					compFound = true;
					break;
				}
			}
			if (!compFound)
				_componentList.Add(aLabel);
		}	
		private void FormatGrid5_12 (FromGrid aGrid)
		{
			int i, j; 
			C1FlexGrid grid = null;	
			AllocationWafer wafer = null;
			//AllocationWaferCoordinateList wafercoordlist;
			//AllocationWaferCoordinate wafercoord;
			//AllocationWaferVariable varProf;
			CellRange cellRange;
			TagForGridData dataTag;

			switch(aGrid)
			{
				case FromGrid.g5:
					wafer = _wafers[0,0];
					grid = g5;
					break;
				case FromGrid.g6:
					wafer = _wafers[0,1];
					grid = g6;
					break;
				case FromGrid.g8:
					wafer = _wafers[1,0];
					grid = g8;
					break;
				case FromGrid.g9:
					wafer = _wafers[1,1];
					grid = g9;
					break;
				case FromGrid.g11:
					wafer = _wafers[2,0];
					grid = g11;
					break;
				case FromGrid.g12:
					wafer = _wafers[2,1];
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
						//						wafercoordlist = (AllocationWaferCoordinateList)wafer.Columns[j];
						//						wafercoord = (AllocationWaferCoordinate)wafercoordlist[_varProfIndex];
						//						varProf = AllocationWaferVariables.GetVariableProfile((eAllocationWaferVariable)wafercoord.Key );
						if (_cells[i,j].ValueAsString != String.Empty) 
							grid.SetData(i, j, _cells[i,j].ValueAsString);
						else
							grid.SetData(i, j, _cells[i,j].Value);
					}
					else
					{
						grid.SetData(i, j, _invalidCellValue);
					}
					dataTag = new TagForGridData();
					dataTag.IsLocked = false;
					cellRange = new CellRange();
					cellRange = grid.GetCellRange(i, j); 
					dataTag.IsEditable = _cells[i,j].CellCanBeChanged;
					cellRange.UserData = dataTag;
				}
			}
			//			for (i = 0; i < grid.Cols.Count; i++)
			//			{
			//				TagForColumn colTag = new TagForColumn();
			//				colTag.CubeWaferCoorList = (AllocationWaferCoordinateList)wafer.Columns[i];
			//				colTag.cellColumn = i;
			//				grid.Cols[i].UserData = colTag;
			//			}
			grid.AutoSizeCols(0, 0, grid.Rows.Count - 1, grid.Cols.Count - 1, 0, C1.Win.C1FlexGrid.AutoSizeFlags.SameSize);
		}
		#endregion
		
		private void AssignTag()
		{
			int col;
			
			try
			{
				//g1
				for (col = 0; col < g1.Cols.Count; col++)
				{
					TagForColumn ColumnTag = (TagForColumn)g1.Cols[col].UserData;
					ColumnTag.IsLockable = false;
					ColumnTag.IsDisplayed = true;
					ColumnTag.Sort = SortEnum.none;
					g1.Cols[col].UserData = ColumnTag;
				}

				AssignTag_g2g3(FromGrid.g2);    //g2
				AssignTag_g2g3(FromGrid.g3);    //g3 
				AssignTagsg4_g12(FromGrid.g4);  //g4
				//				AssignTagsg4_g12(FromGrid.g5);  //g5
				//				AssignTagsg4_g12(FromGrid.g6);  //g6
				AssignTagsg4_g12(FromGrid.g7);  //g7
				//				AssignTagsg4_g12(FromGrid.g8);  //g8
				//				AssignTagsg4_g12(FromGrid.g9);  //g9
				AssignTagsg4_g12(FromGrid.g10); //g10
				//				AssignTagsg4_g12(FromGrid.g11); //g11
				//				AssignTagsg4_g12(FromGrid.g12); //g12

			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void AssignTag_g2g3 (FromGrid WhichGrid)
		{
			C1FlexGrid grid = null;
			//RowColHeader rch;
			try
			{
				switch (WhichGrid)
				{
					case FromGrid.g2:
						grid = g2;
						break;
					case FromGrid.g3:
						grid = g3;
						break;
				}

				for (int col = 0; col < grid.Cols.Count; col ++)
				{
					TagForColumn colTag = (TagForColumn)grid.Cols[col].UserData;
					//colTag.IsLockable = colTag.CubeWaferCoorList.Cell.CellCanBeChanged;
					
					if (_setHeaderTags)
					{
						colTag.IsDisplayed = true;
						colTag.Sort = SortEnum.none;
					}
						//else if (ColHeaders2 != null)	
						//{
						//	wafercoord = colTag.CubeWaferCoorList[_compRow];
						//	rch = (RowColHeader)ColHeaders2[wafercoord.Label];
						//	if ( rch != null)
						//	{
						//		colTag.IsDisplayed = rch.IsDisplayed;
						//	}
						//}
					else
					{
						colTag.IsDisplayed = true;
						colTag.Sort = SortEnum.none;
					}
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
			//C1FlexGrid colGrid;
			int row;
			//int col;
			//CellRange cellRange;
			TagForGridData dataTag;
		
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
						break;
					case FromGrid.g9:
						grid = g9;
						break;
					case FromGrid.g10:
						grid = g10;
						break;
					case FromGrid.g11:
						grid = g11;
						break;
					case FromGrid.g12:
						grid = g12;
						break;
				}
				if (grid == g4 || grid == g7 || grid == g10) 
				{
					for (row = 0; row < grid.Rows.Count; row++)
					{
						TagForRow rowTag = (TagForRow)grid.Rows[row].UserData;
						rowTag.IsLockable = false;
						if (grid == g7 && _trans.AnalysisOnly)
							rowTag.IsDisplayed = false;
						else
							rowTag.IsDisplayed = true;
						grid.Rows[row].UserData = rowTag;
					}
				}

				//				colGrid = GetColumnGrid(grid);
				//				if (grid == g8 || grid == g9 || grid == g11 || grid == g12)
				//					colGrid = grid;
				//				for (col = 0; col < grid.Cols.Count; col++)
				//				{
				//					//Get the column tag (so later we can check to see if the column is lockable.)
				//					TagForColumn colTag = (TagForColumn)colGrid.Cols[col].UserData;
				//				
				//					if (colTag.IsLockable == true)
				//					{
				//						dataTag.IsEditable = true;
				//					}
				//					else
				//					{
				//						dataTag.IsEditable = false;
				//					}
				//					//loop through the rows and make each cell none-editable.
				//					for (row = 0; row < grid.Rows.Count; row++)
				//					{
				//						cellRange = new CellRange();
				//						cellRange = grid.GetCellRange(row, col);
				//						cellRange.UserData = dataTag;
				//					}
				//				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void ApplyPreferences()
		{
			if (_trans.AnalysisOnly)
			{
				for (int i = 0; i < g7.Rows.Count; i++)
				{
					g7.Rows[i].Visible = false;
					g8.Rows[i].Visible = false;
					g9.Rows[i].Visible = false;
				}
			}	
		}
		private void MiscPositioning()
		{
			try
			{
                //Begin TT#1436 - JSmith - Hide OTS Variance row
                g4.Rows[4].Visible = false;
                g5.Rows[4].Visible = false;
                g6.Rows[4].Visible = false;
                //End TT#1436

				ResizeRows();
				ResizeColumns();
				
				// Miscellaneous - setup positions and sizes for splitters and scroll bars.
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

				SetRowSplitPosition4();
				SetRowSplitPosition8();
				SetRowSplitPosition12();
			 
				SetVScrollBar2Parameters();
				SetVScrollBar3Parameters();
				SetVScrollBar4Parameters();
		
				RefreshHorizontalScrollBars(1);
				RefreshHorizontalScrollBars(2);
				RefreshHorizontalScrollBars(3);
				if (rbGroupByStoreGrade.Checked)
				{
					if ( g3.Cols.Count > _minFrozenCols)
						hScrollBar3.Minimum = _minFrozenCols;
				}
				if (!_loading)
				{
					SetActionListEnabled();
					if (_trans.AnalysisOnly)
					{
						btnApply.Enabled = false;
					}	
					else
						btnApply.Enabled = (g8.Rows.Count > 0 && (_trans.DataState == eDataState.Updatable));
				}
				 
			}	
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetActionListEnabled()
		{
			bool enableActions = true;
			if (_trans.AnalysisOnly || FunctionSecurity.AllowUpdate == false
				|| _trans.DataState == eDataState.ReadOnly)
				enableActions = false;
                // begin TT#1185 - Verify ENQ before Update
            else if (g4.Rows.Count == 0)
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
                        break;
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
		private void ResizeColumns()
		{
			g1.AutoSizeCols();g2.AutoSizeCols();
			g3.AutoSizeCols();g4.AutoSizeCols();
			g5.AutoSizeCols();g6.AutoSizeCols();
			g7.AutoSizeCols();g8.AutoSizeCols();
			g9.AutoSizeCols();g10.AutoSizeCols();
			g11.AutoSizeCols();g12.AutoSizeCols();

			int i;
			//Resize Columns on all grids. Line-up data based on the widest column.
			//And while we're in the loop, set the ImageAlign property of each column.
			int MaxColWidth = 0;  //temp variable to hold the currently widest column.
			for (i = 0; i < g4.Cols.Count; i++)
			{
				if (g1.Cols[i].Width > MaxColWidth) MaxColWidth = g1.Cols[i].Width;
				if (g4.Cols[i].Width > MaxColWidth) MaxColWidth = g4.Cols[i].Width;
				if (g7.Cols[i].Width > MaxColWidth) MaxColWidth = g7.Cols[i].Width;
				if (g10.Cols[i].Width > MaxColWidth) MaxColWidth = g10.Cols[i].Width;
			
				g1.Cols[i].Width = MaxColWidth;
				g4.Cols[i].Width = MaxColWidth;
				g7.Cols[i].Width = MaxColWidth;
				g10.Cols[i].Width = MaxColWidth;
				
				//  the following few lines of code sets the ImageAlign property of the column.
				g1.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g4.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g7.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g10.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				 
				MaxColWidth = 0;
			}
			MaxColWidth = 0; //reset the temp variable.
			for (i = 0; i < g2.Cols.Count; i++)
			{
				if (g2.Cols[i].Width > MaxColWidth) MaxColWidth = g2.Cols[i].Width;
				if (g5.Cols[i].Width > MaxColWidth) MaxColWidth = g5.Cols[i].Width;
				if (g8.Cols[i].Width > MaxColWidth) MaxColWidth = g8.Cols[i].Width;
				if (g11.Cols[i].Width > MaxColWidth) MaxColWidth = g11.Cols[i].Width;

				g2.Cols[i].Width = MaxColWidth;
				g5.Cols[i].Width = MaxColWidth;
				g8.Cols[i].Width = MaxColWidth;
				g11.Cols[i].Width = MaxColWidth;
				
				// the following few lines of code sets the ImageAlign property of the column.
				g2.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
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
				
				// the following few lines of code sets the ImageAlign property of the column.
				g3.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g6.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g9.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				g12.Cols[i].ImageAlign = ImageAlignEnum.RightCenter;
				MaxColWidth = 0;
			}
			if (_showCol == "left")
			{
				g3.LeftCol = _curg3LeftCol;
				g6.LeftCol = _curg3LeftCol;
				g9.LeftCol = _curg3LeftCol;
				g12.LeftCol = _curg3LeftCol;
			}
			else if (_showCol == "right")
			{
				//g3.RightCol = g3.Cols.Count - 1;
				g3.ShowCell(0,g3.Cols.Count - 1);
				g6.ShowCell(0,g3.Cols.Count - 1);
				g9.ShowCell(0,g3.Cols.Count - 1);
				g12.ShowCell(0,g3.Cols.Count - 1);
			} 
			_showCol = string.Empty;
			SetHScrollBarParameters();
		} 
		private void ResizeRows()
		{
			
			g1.AutoSizeRows();g2.AutoSizeRows();
			g3.AutoSizeRows();g4.AutoSizeRows();
			g5.AutoSizeRows();g6.AutoSizeRows();
			g7.AutoSizeRows();g8.AutoSizeRows();
			g9.AutoSizeRows();g10.AutoSizeRows();
			g11.AutoSizeRows();g12.AutoSizeRows();

			int i;

			//Resize Rows on all grids. Line-up data based on the tallest row.
			int MaxRowHeight = 0;  //temp variable to hold the currently tallest row.
			for (i = 0; i < g1.Rows.Count; i++)
			{
				if (g1.Rows[i].HeightDisplay > MaxRowHeight) MaxRowHeight = g1.Rows[i].HeightDisplay;
				if (g2.Rows[i].HeightDisplay > MaxRowHeight) MaxRowHeight = g2.Rows[i].HeightDisplay;
				if (g3.Rows[i].HeightDisplay > MaxRowHeight) MaxRowHeight = g3.Rows[i].HeightDisplay;

				g1.Rows[i].HeightDisplay = MaxRowHeight;
				g2.Rows[i].HeightDisplay = MaxRowHeight;
				g3.Rows[i].HeightDisplay = MaxRowHeight;

				MaxRowHeight = 0;
			}
			MaxRowHeight = 0; //reset the temp variable.
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

		private void mnuFreezeColumn_Click(object sender, System.EventArgs e)
		{
			int i, NumColsScrolledOffScreen, NumColsFrozen;
							
			try
			{
				TagForColumn colTag = new TagForColumn();
				switch(RightClickedFrom)
				{
					case FromGrid.g1:
						NumColsScrolledOffScreen = g1.LeftCol;
						if (mnuFreezeColumn.Checked == true) //Unfreeze the columns.
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
						RefreshHorizontalScrollBars(1);
						break;
					case FromGrid.g2:
						NumColsScrolledOffScreen = g2.LeftCol;
						if (mnuFreezeColumn.Checked == true) //Unfreeze the columns.
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
						RefreshHorizontalScrollBars(2);
						break;
					case FromGrid.g3:
						if (rbGroupByStoreGrade.Checked)
							if (GridMouseCol < 3) 
								return;
							else
								NumColsScrolledOffScreen = 0;
						else
							NumColsScrolledOffScreen = g3.LeftCol;
						if (mnuFreezeColumn.Checked == true) //Unfreeze the columns.
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
							if (rbGroupByStoreGrade.Checked)
							{
								g3.Cols.Frozen = _minFrozenCols;
								g6.Cols.Frozen = _minFrozenCols;
								g9.Cols.Frozen = _minFrozenCols;
								g12.Cols.Frozen = _minFrozenCols;
							}
							else
							{
								g3.Cols.Frozen = 0;
								g6.Cols.Frozen = 0;
								g9.Cols.Frozen = 0;
								g12.Cols.Frozen = 0;
							}
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
						RefreshHorizontalScrollBars(3);
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		
	
		#region Style-changing codes (colors, fonts, etc)
		private void ChangeStyle()
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

					cellStyle = g1.Styles.Add("GroupHeader");
					cellStyle.BackColor = _theme.ColumnGroupHeaderBackColor;
					cellStyle.ForeColor = _theme.ColumnGroupHeaderForeColor;
					cellStyle.Font = _theme.ColumnGroupHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
					cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 3, 3);
					cellStyle.TextEffect = _theme.ColumnGroupHeaderTextEffects;

					cellStyle = g1.Styles.Add("ColumnHeading");
					cellStyle.BackColor = _theme.ColumnHeaderBackColor;
					cellStyle.ForeColor = _theme.ColumnHeaderForeColor;
					cellStyle.Font = _theme.ColumnHeaderFont;
					cellStyle.Border.Style = _theme.CellBorderStyle;
					cellStyle.Border.Color = _theme.CellBorderColor;
					cellStyle.Margins = new System.Drawing.Printing.Margins(3, 3, 1, 1);
					cellStyle.TextEffect = _theme.ColumnHeaderTextEffects;

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

					cellStyle = g7.Styles.Add("Style1");
					//cellStyle.BackColor = _theme.StoreSetRowHeaderBackColor;
					//cellStyle.ForeColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.BackColor = _theme.StoreSetBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
			
					cellStyle = g7.Styles.Add("Style2");
					//cellStyle.BackColor = _theme.StoreSetRowHeaderAlternateBackColor;
					//cellStyle.ForeColor = _theme.StoreSetRowHeaderForeColor;
					cellStyle.BackColor = _theme.StoreSetAlternateBackColor;
					cellStyle.ForeColor = _theme.StoreSetForeColor;
					cellStyle.Font = _theme.RowHeaderFont;
					cellStyle.Border.Style = BorderStyleEnum.None;
			
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
				//Assign styles to grids.
				//CellRange r = new CellRange();
				
				// g1
				g1.Rows[0].Style = g1.Styles["GroupHeader"];
				g1.Rows[1].Style = g1.Styles["ColumnHeading"];
				
				//g2
				g2.Rows[0].Style = g2.Styles["GroupHeader"];
				g2.Rows[1].Style = g2.Styles["ColumnHeading"];
			
				//g3
				g3.Rows[0].Style = g2.Styles["GroupHeader"];
				g3.Rows[1].Style = g2.Styles["ColumnHeading"];
				
				ChangeStylesForG4_G12();
				MiscPositioning();
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
				
				ChangeStylesG4();
				ChangeStylesG5();
				ChangeStylesG6();
				ChangeStylesG7();
				ChangeStylesG8();
				ChangeStylesG9();
				ChangeStylesG10();
				ChangeStylesG11();
				ChangeStylesG12();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void ChangeStylesG4()
		{
			try
			{
				if (g4.Rows.Count == 0)
					return;
				ChangeRowStyles(g4, rowsPerStoreGroup);
				ChangeCellStyle(g4);
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
				SetCellStyle(g5);
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
				if (g6.Rows.Count == 0 || g6.Cols.Count == 0)
					return;
				ChangeRowStyles(g6, rowsPerStoreGroup);
				ChangeCellStyle(g6);
				SetCellStyle(g6);
				Debug.WriteLine("ChangeStylesG6");
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void ChangeStylesG7()
		{
			try
			{
				if (g7.Rows.Count == 0)
					return;
				//ChangeRowStyles(g7, rowsPerStoreGroup);
				ChangeRowStyles7_9(g7);
				ChangeCellStyle(g7);
				//				 g7.Cols[0].AllowMerging = true;
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
				//ChangeRowStyles(g8, rowsPerStoreGroup);
				ChangeRowStyles7_9(g8);
				ChangeCellStyle(g8);
				SetCellStyle(g8);
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
				if (g9.Rows.Count == 0 || g9.Cols.Count == 0)
					return;
				//ChangeRowStyles(g9, rowsPerStoreGroup);
				ChangeRowStyles7_9(g9);
				ChangeCellStyle(g9);
				SetCellStyle(g9);
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
				g10.Cols[0].TextAlign = TextAlignEnum.LeftTop;	
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
				SetCellStyle(g11);
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
				if (g12.Rows.Count == 0 || g12.Cols.Count == 0)
					return;
				ChangeRowStyles(g12, rowsPerStoreGroup);
				ChangeCellStyle(g12);
				SetCellStyle(g12);
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
		private void ChangeRowStyles7_9(C1FlexGrid aGrid)
		{
			int i;
			string headerID = string.Empty;
			string rowStyle;
			bool bStyle1;
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
					else if (_theme.ViewStyle == StyleEnum.AlterColors 
						|| _theme.ViewStyle == StyleEnum.HighlightName) 
					{	
						headerID = Convert.ToString(g7.GetData(0,0));
						rowStyle = "Style1";
						bStyle1 = true;
						for (i = 0; i < aGrid.Rows.Count; i++)
						{
							if (Convert.ToString(g7.GetData(i,0)) != headerID)
							{
								headerID = Convert.ToString(g7.GetData(i,0));
								bStyle1 = !bStyle1;
							}
							if (bStyle1)
								rowStyle = "Style1";
							else
								rowStyle = "Style2";
							aGrid.Rows[i].Style = aGrid.Styles[rowStyle];
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
				//				if (aGrid == g7)
				//					aGrid.Cols[0].AllowMerging = true;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetCellStyle(C1FlexGrid aGrid)
		{
			CellRange cellRange;
			try
			{
				//Loop through the entire sheet and pick out the negatives and the editables.
				for (int col = 0; col < aGrid.Cols.Count; col ++)
				{
					for (int row = 0; row < aGrid.Rows.Count; row ++)
					{
						cellRange = aGrid.GetCellRange(row, col);
						TagForGridData DataTag = (TagForGridData)cellRange.UserData;
						try
						{
							//Is it just editable?
							if (DataTag.IsEditable == true && Convert.ToDecimal(aGrid[row, col]) >= 0)
							{
								if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)							
									cellRange.Style = aGrid.Styles["Editable1"];							
								else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)							
									cellRange.Style = aGrid.Styles["Editable2"];							
							}
								//Is it just negative?
							else if (DataTag.IsEditable == false && Convert.ToDecimal(aGrid[row, col]) < 0)
							{
								if (cellRange.Style.Name == aGrid.Styles["Style1"].Name)
									cellRange.Style = aGrid.Styles["Negative1"];
								else if (cellRange.Style.Name == aGrid.Styles["Style2"].Name)
									cellRange.Style = aGrid.Styles["Negative2"];
							}
								//Is it both negative and editable?
							else if (DataTag.IsEditable == true && Convert.ToDecimal(aGrid[row, col]) < 0)
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

		private void ChangeRegion(int RegionIndex){}
		
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
			ChangeStyle();
			_loading = false;
			Cursor.Current = Cursors.Default;
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
			else if (_theme.ViewStyle == StyleEnum.Chiseled  
				&&
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
				(e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%rowsPerStoreGroup == 0) 
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				g.FillRectangle(_theme.RowGroupRowHeaderDividerBrush, rec);

				//for some reason, for Summary View, this OwnerDrawCell event only catches column 1. To make
				//sure the border extends the entire width of g7, we manually get the rectangle that covers
				//column 0.
				rec = g7.GetCellRect(e.Row, e.Col - 1, false);
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;
				g.FillRectangle(_theme.RowGroupRowHeaderDividerBrush, rec);
			}
			else if (_theme.ViewStyle == StyleEnum.Chiseled )
				//				&&
				//				((e.Row + 1 + LastInvisibleRowsPerStoreGroup7)%(rowsPerStoreGroup*2) == 0 ||
				//				(e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				//				if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup7)%(rowsPerStoreGroup*2) == 0)
				//				{
				//					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				//
				//					//for some reason, for Summary View, this OwnerDrawCell event only catches column 1. To make
				//					//sure the border extends the entire width of g7, we manually get the rectangle that covers
				//					//column 0.
				//					rec = g7.GetCellRect(e.Row, e.Col - 1, false);
				//					rec.Y = rec.Bottom - m.Bottom;
				//					rec.Height = m.Bottom;
				//					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				//				}
				//				else if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0)
				//				{
				//					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				//
				//					//for some reason, for Summary View, this OwnerDrawCell event only catches column 1. To make
				//					//sure the border extends the entire width of g7, we manually get the rectangle that covers
				//					//column 0.
				//					rec = g7.GetCellRect(e.Row, e.Col - 1, false);
				//					rec.Y = rec.Bottom - m.Bottom;
				//					rec.Height = m.Bottom;
				//					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				//				}
				
				if ( (e.Row < g7.Rows.Count - 1) && Convert.ToString(g7.GetData(e.Row, 0)) !=
					Convert.ToString(g7.GetData(e.Row+1, 0)) )
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else
					if (e.Col == 0)
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				else
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
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
			else if (_theme.ViewStyle == StyleEnum.Chiseled)
				//				&&
				//				((e.Row + 1 + LastInvisibleRowsPerStoreGroup7)%(rowsPerStoreGroup*2) == 0 ||
				//				(e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				//				if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup7)%(rowsPerStoreGroup*2) == 0)
				//				{
				//					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				//				}
				//				else if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0)
				//				{
				//					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				//				}
				if ( (e.Row < g7.Rows.Count - 1) && Convert.ToString(g7.GetData(e.Row, 0)) !=
					Convert.ToString(g7.GetData(e.Row+1, 0)) )
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
			}
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
			else if (_theme.ViewStyle == StyleEnum.Chiseled )
				//				&&
				//				((e.Row + 1 + LastInvisibleRowsPerStoreGroup7)%(rowsPerStoreGroup*2) == 0 ||
				//				(e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0))
			{
				Rectangle rec;
				Graphics g = e.Graphics;
				System.Drawing.Printing.Margins m = new System.Drawing.Printing.Margins(1, 1, 1, _theme.DividerWidth);

				rec = e.Bounds;
				rec.Y = rec.Bottom - m.Bottom;
				rec.Height = m.Bottom;

				//				if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup7)%(rowsPerStoreGroup*2) == 0)
				//				{
				//					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				//				}
				//				else if ((e.Row + 1 + LastInvisibleRowsPerStoreGroup7 + rowsPerStoreGroup)%(rowsPerStoreGroup * 2) == 0)
				//				{
				//					g.FillRectangle(_theme.ChiselUpperBrush, rec);
				//				}
				if ( (e.Row < g7.Rows.Count - 1) && Convert.ToString(g7.GetData(e.Row, 0)) !=
					Convert.ToString(g7.GetData(e.Row+1, 0)) )
				{
					g.FillRectangle(_theme.ChiselLowerBrush, rec);
				}
				else
					g.FillRectangle(_theme.ChiselUpperBrush, rec);
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
            if (!((C1FlexGrid)sender).Redraw)
            {
                return;
            }

			if (g12.GetCellStyle(e.Row, e.Col) == null) return;
			try
			{
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

					if ( (e.Col < g3.Cols.Count - 1) && Convert.ToString(g3.GetData(0, e.Col)) !=
						Convert.ToString(g3.GetData(0, e.Col+1)) )
						g.FillRectangle(_theme.ColumnGroupDividerBrush, rec);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#endregion
		#region Criteria Changed Events
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
				
				_wafers = _trans.AllocationWafers;
				
				FormatGrids1to12();

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
				
				//SortColumns();
				_setHeaderTags = false;
				//				AssignTag();
				//				ChangeStyle();				
				//				ApplyPreferences();
				//				UserSetSplitter1Position = VerticalSplitter1.SplitPosition;
				//				UserSetSplitter2Position = VerticalSplitter2.SplitPosition;
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

		public void ReloadGridData()
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				_trans.RebuildWafers();
				_wafers = _trans.AllocationWafers;
				FormatGrids1to12();
			
				//ChangePending = true;
				Cursor.Current = Cursors.Default;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		#endregion
		#region various repetitive events
		

		#region Find
		private void Find()
		{
			DialogResult diagResult;
			ArrayList setList;
			string attSet, storeGrade;
			int key = 0;
			try
			{
				_quickFilterType = eQuickFilterType.Find;
				if (_lblHeader == null)
					_lblHeader = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Header);	
				
				if (_lblComponent == null)
					_lblComponent = MIDText.GetTextOnly((int)eQuickFilterSelectionType.Component) + ":";	

				_quickFilter = new QuickFilter(eQuickFilterType.Find, 4, _lblHeader + ":", _lblComponent + ":", _lblAttributeSet + ":", _lblStoreGrade + ":");
						
				_quickFilter.OnValidateFieldsHandler += new QuickFilter.ValidateFieldsHandler(OnValidateFieldsHandler);
		
				if (_headerList.Count > 1)
					_quickFilter.EnableComboBox(0);
				else	
					_quickFilter.DisableComboBox(0);
				
				_quickFilter.EnableComboBox(1);
				_quickFilter.SetComboBoxSort(1,false);
				_quickFilter.EnableComboBox(2);
				_quickFilter.SetComboBoxSort(2,false);

				if (_headerList.Count > 1)
				{
					BuildHeaderList();
					_quickFilter.LoadComboBox(0, _headerArrayList);
				}
				_quickFilter.LoadComboBox(1, _componentList);

				//BEGIN TT#6-MD-VStuart - Single Store Select
				// This code was added just to populate the '_attrSetProfileList' variable.
				if (this.cmbStoreAttribute.SelectedValue != null)
					PopulateStoreAttributeSet(this.cmbStoreAttribute.SelectedValue.ToString());
				//END TT#6-MD-VStuart - Single Store Select

				setList = new ArrayList();
				foreach (StoreGroupLevelListViewProfile sglp in _attrSetProfileList)
				{
					setList.Add(sglp.Name);
				}
			 	_quickFilter.LoadComboBox(2, setList);
				
				if (_gradeList.Count > 0)
				{
					_quickFilter.EnableComboBox(3);
					_quickFilter.SetComboBoxSort(3,false);	
					_quickFilter.LoadComboBox(3, _gradeList);
				}
				else
					_quickFilter.DisableComboBox(3);

				diagResult = _quickFilter.ShowDialog(this);

				if (diagResult == DialogResult.OK)
				{
					if (_quickFilter.GetSelectedIndex(2) >= 0)
					{
						attSet = Convert.ToString(_quickFilter.GetSelectedItem(2),CultureInfo.CurrentUICulture);
						if (rbGroupByAttribute.Checked)
						{	
							for (int i = 0; i < g3.Cols.Count; i++)
							{
								if ( Convert.ToString(g3.GetData(0,i),CultureInfo.CurrentUICulture) == attSet)
								{
									_foundColumn = i; 
									break;
								}
							}
						}
						else
							
							foreach (StoreGroupLevelListViewProfile sglp in _attrSetProfileList)
							{
								if (sglp.Name == attSet)
								{
									key = sglp.Key;
									break;
								}	
							}
                        if (key > 0)
                        {
                            //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                            int indexCurrent = this.cmbAttributeSet.SelectedIndex; //Current Index.
                            cmbAttributeSet.SelectedValue = key;

                            int indexChk = this.cmbAttributeSet.SelectedIndex;  //New index if changed.
                            if (indexChk != indexCurrent)
                            {
                                //this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs());
                                // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected
                            }
                            //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                        }
                        

					}

					if (_quickFilter.GetSelectedIndex(3) >= 0)
					{
						storeGrade = Convert.ToString(_quickFilter.GetSelectedItem(3),CultureInfo.CurrentUICulture);
						for (int i = 0; i < g3.Cols.Count; i++)
						{
							if ( Convert.ToString(g3.GetData(0,i),CultureInfo.CurrentUICulture) == storeGrade)
							{
								_foundColumn = i; 
								break;
							}
						}
					}
					_scroll3EventType = ScrollEventType.SmallIncrement;
					_scroll2EventType = ScrollEventType.SmallIncrement;
					
					if (_foundColumn > hScrollBar3.Maximum)
						hScrollBar3.Value = hScrollBar3.Maximum;
					else if (_foundColumn < hScrollBar3.Minimum)
						hScrollBar3.Value = hScrollBar3.Minimum;
					else
						hScrollBar3.Value = _foundColumn;
				
					if (_foundRow > vScrollBar3.Maximum)
						vScrollBar3.Value = vScrollBar3.Maximum;
					else
	                    vScrollBar3.Value = _foundRow;

					g9.Select(_foundRow, _foundColumn);
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		private bool ValidFind()
		{
			bool OKToProcess = true;
			string headerID, compLabel;
			string strHeader, strComponent, errorMessage = string.Empty;
			string title = _quickFilter.Text; 
			try
			{
				_foundRow = 0;
				if (_quickFilter.GetSelectedIndex(0) >= 0)
				{
					headerID = Convert.ToString(_quickFilter.GetSelectedItem(0),CultureInfo.CurrentUICulture);
					if (_quickFilter.GetSelectedIndex(1) >= 0)
					{
						bool rowFound = false;
						compLabel = _quickFilter.GetSelectedItem(1).ToString();
						for (int i = 0; i < g7.Rows.Count; i++)
						{
							strHeader	= Convert.ToString(g7.GetData(i,0),CultureInfo.CurrentUICulture);
							strComponent  = Convert.ToString(g7.GetData(i,1),CultureInfo.CurrentUICulture);
 
							if (strHeader == headerID && strComponent == compLabel)
							{
								rowFound = true;
								_foundRow = i; 
								break;
							}
						}
						if (!rowFound)
						{
							OKToProcess = false;
							errorMessage = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ComponentNotInHeader);
							_quickFilter.SetError(1,errorMessage);
							MessageBox.Show(errorMessage, title);
						}
					}	
					else
					{
						_foundRow = g7.FindRow(headerID,0,0,false);
						if (_foundRow == -1)
							_foundRow = 0;
					}
				}
				else if (_quickFilter.GetSelectedIndex(1) >= 0)
				{
					compLabel = Convert.ToString(_quickFilter.GetSelectedItem(1),CultureInfo.CurrentUICulture);
					_foundRow = g7.FindRow(compLabel,0,1,false);
					if (_foundRow == -1)
						_foundRow = 0; 
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
				if (_headerList.Count == 1)
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
			TagForRow rowTag;
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
 
					for (int i = 0; i < g7.Rows.Count; i++)
					{
						rowTag = (TagForRow)g7.Rows[i].UserData;
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
					waferCoord = rowTag.CubeWaferCoorList[_hdrRow];
					if (waferCoord.Label == aHeaderID)
					{
						headerStarted = true;
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

        #region Export
        private void Export()
        {
            MIDExportFile MIDExportFile = null;
            try
            {
                MIDExport MIDExport = null;
                bool showCurrentAll = false;
                if (rbGroupByStoreGrade.Checked)
                {
                    showCurrentAll = true;
                }
                MIDExport = new MIDExport(SAB, _includeCurrentSetLabel, _includeAllSetsLabel, showCurrentAll);

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
                        ExportSpecificStyle(MIDExportFile, "g1", g1, "GroupHeader");
                        ExportSpecificStyle(MIDExportFile, "g1", g1, "ColumnHeading");
                        ExportSpecificStyle(MIDExportFile, "g2", g2, "GroupHeader");
                        ExportSpecificStyle(MIDExportFile, "g2", g2, "ColumnHeading");
                        ExportSpecificStyle(MIDExportFile, "g4", g4, "Style1");
                        ExportSpecificStyle(MIDExportFile, "g4", g4, "Style1Center");
                        ExportSpecificStyle(MIDExportFile, "g5", g5, "Editable1");
                        ExportSpecificStyle(MIDExportFile, "g5", g5, "Editable2");
                        ExportSpecificStyle(MIDExportFile, "g5", g5, "Negative1");
                        ExportSpecificStyle(MIDExportFile, "g6", g6, "Editable1");
                        ExportSpecificStyle(MIDExportFile, "g6", g6, "Editable2");
                        ExportSpecificStyle(MIDExportFile, "g6", g6, "Negative1");
                        ExportSpecificStyle(MIDExportFile, "g7", g7, "Style1");
                        ExportSpecificStyle(MIDExportFile, "g7", g7, "Style2");
                        ExportSpecificStyle(MIDExportFile, "g8", g8, "Editable1");
                        ExportSpecificStyle(MIDExportFile, "g8", g8, "Editable2");
                        ExportSpecificStyle(MIDExportFile, "g8", g8, "Negative1");
                        ExportSpecificStyle(MIDExportFile, "g8", g8, "Negative2");
                        ExportSpecificStyle(MIDExportFile, "g9", g9, "Editable1");
                        ExportSpecificStyle(MIDExportFile, "g9", g9, "Editable2");
                        ExportSpecificStyle(MIDExportFile, "g9", g9, "Negative1");
                        ExportSpecificStyle(MIDExportFile, "g9", g9, "Negative2");
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void ExportCurrentFile(MIDExportFile aMIDExportFile)
        {
            try
            {
                int detailsPerGroup = 0;

                ExportData(aMIDExportFile, null, detailsPerGroup, true, true, false);
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
                bool addSummary = false;
                int setCount = 0;

                //Begin TT#866 - JSmith - Export of multiple sets has the incorrect data in the store columns
                TurnRedrawOff();
                //End TT#866

                foreach (StoreGroupLevelListViewProfile sglProf in _attrSetProfileList)
                {
                    ++setCount;
                    //Begin TT#866 - JSmith - Export of multiple sets has the incorrect data in the store columns
                    //_trans.AllocationStoreGroupLevel = sglProf.Key;
                    //End TT#866
                    if (aMIDExportFile.ExportType == eExportType.CSV)
                    {
                        // only add summary at end of CSV file
                        if (setCount == _attrSetProfileList.Count)
                        {
                            addSummary = true;
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

                //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
                int indexCurrent = this.cmbAttributeSet.SelectedIndex; //Current Index.

                cmbAttributeSet.SelectedValue = saveGroupLevel;

                int indexChk = this.cmbAttributeSet.SelectedIndex;  //New index if changed.
                if (indexChk != indexCurrent)
                {
                    //this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs());
                    // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected
                }
                //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly

                TurnRedrawOn();
                //End TT#866
            }
        }

        private void ExportData(MIDExportFile aMIDExportFile, StoreGroupLevelListViewProfile sglProf, int aRowsPerGroup,
            bool aAddHeader, bool aAddSummary, bool aExportingAll)
        {
            try
            {
                //string negativeStyle = null;
                string textStyle = null;
                int i;
                int j;
                int totRows = 0;
                int totCols = 0;
                string groupingName = "sheet1";

                AllocationWaferGroup wafers;
                AllocationWafer headingWafer;
                AllocationWaferCell[,] headingCells;
                string[,] headingColLabels;
                string[,] headingRowLabels;
                AllocationWafer totalWafer;
                AllocationWaferCell[,] totalCells;
                string[,] totalColLabels;
                AllocationWafer detailWafer;
                AllocationWaferCell[,] detailCells;
                string[,] detailColLabels;

                if (sglProf == null)
                {
                    groupingName = "Summary";
                }
                else
                {
                    groupingName = sglProf.Name;
                }

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

                headingWafer = wafers[0, 0];
                headingCells = headingWafer.Cells;
                headingColLabels = wafers[0, 0].ColumnLabels;
                headingRowLabels = wafers[0, 0].RowLabels;

                totalWafer = wafers[0, 0];
                totalCells = totalWafer.Cells;
                totalColLabels = wafers[0, 0].ColumnLabels;

                detailWafer = wafers[0, 1];
                detailCells = detailWafer.Cells;
                detailColLabels = wafers[0, 1].ColumnLabels;
                if (aAddHeader)
                {
                    // add headings
                    for (i = 0; i < headingColLabels.GetLength(0); i++)
                    {
                        aMIDExportFile.AddRow();
                        // add store column
                        //negativeStyle = null;
                        if (i == 0)
                        {
                            textStyle = "g1GroupHeader";
                        }
                        else
                        {
                            textStyle = "g1ColumnHeading";
                        }
                        for (j = 0; j < 2; j++)
                        {
                            if (i == 0)
                            {
                                aMIDExportFile.AddValue(_lblSet, eExportDataType.ColumnHeading, textStyle, null);
                            }
                            else
                            {
                                aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, null);
                            }
                        }
                        // add All Store columns
                        //negativeStyle = null;
                        if (i == 0)
                        {
                            textStyle = "g2GroupHeader";
                        }
                        else
                        {
                            textStyle = "g2ColumnHeading";
                        }
                        for (j = 0; j < headingColLabels.GetLength(1); j++)
                        {
                            if (g2.Cols[j].Visible)
                            {
                                if (headingColLabels[i, j] != null)
                                {
                                    aMIDExportFile.AddValue(headingColLabels[i, j], eExportDataType.ColumnHeading, textStyle, null);
                                }
                                else
                                {
                                    aMIDExportFile.AddValue(" ", eExportDataType.ColumnHeading, textStyle, null);
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
                        for (j = 0; j < detailColLabels.GetLength(1); j++)
                        {
                            if (g3.Cols[j].Visible)
                            {
                                if (detailColLabels[i, j] != null)
                                {
                                    aMIDExportFile.AddValue(detailColLabels[i, j], eExportDataType.ColumnHeading, textStyle, null);
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

                // add values
                if (headingRowLabels.GetLength(0) > 0)
                {
                    ExportAddWafers(aMIDExportFile, headingWafer, totalWafer, detailWafer, null, aRowsPerGroup,
                        "g4Style1", "g4Style1", "g4Style1", "g5Editable1", "g5Negative1", "g6Editable1", "g6Negative1",
                        null, null, null, null, null, null, null);
                }

                // add header values
                ExportAddWafers(aMIDExportFile, wafers[1, 0], wafers[1, 0], wafers[1, 1], groupingName, aRowsPerGroup,
                        "g7Style1", "g7Style1", "g7Style1", "g8Editable1", "g8Negative1", "g9Editable1", "g9Negative1",
                        "g7Style2", "g7Style2", "g7Style2", "g8Editable2", "g8Negative2", "g9Editable2", "g9Negative2");

                if (aAddSummary)
                {
                    // add blank line before summary
                    if (aMIDExportFile.ExportType == eExportType.CSV &&
                        aExportingAll)
                    {
                        aMIDExportFile.AddRow();
                        aMIDExportFile.WriteRow();
                    }
                    // add totals
                    ExportAddWafers(aMIDExportFile, wafers[2, 0], wafers[2, 0], wafers[2, 1], null, aRowsPerGroup,
                        "g10Style1", "g10Style1", "g10Style1", "g11Editable1", "g11Negative1", "g12Editable1", "g12Negative1",
                        null, null, null, null, null, null, null);
                }

                aMIDExportFile.WriteGrouping();
            }
            catch
            {
                throw;
            }
        }

        private void ExportAddWafers(MIDExportFile aMIDExportFile, AllocationWafer aHeadingWafer,
            AllocationWafer aTotalWafer, AllocationWafer aDetailWafer, string aSetName, int aRowsPerGroup,
            string aHeadingStyle, string aHeadingEditableStyle, string aHeadingNegativeStyle,
            string aTotalEditableStyle, string aTotalNegativeStyle,
            string aDetailEditableStyle, string aDetailNegativeStyle,
            string aAltHeadingStyle, string aAltHeadingEditableStyle, string aAltHeadingNegativeStyle,
            string aAltTotalEditableStyle, string aAltTotalNegativeStyle,
            string aAltDetailEditableStyle, string aAltDetailNegativeStyle)
        {
            try
            {
                string negativeStyle = null;
                string textStyle = null;
                int i;
                int j;
                AllocationWaferCell[,] headingCells;
                string[,] headingColLabels;
                string[,] headingRowLabels;
                AllocationWaferCell[,] totalCells;
                string[,] totalColLabels;
                AllocationWaferCell[,] detailCells;
                string[,] detailColLabels;
                int dataCol;
                int rowHeadingColumns;
                int row;
                string headingStyle = aHeadingStyle;
                string headingEditableStyle = aHeadingEditableStyle;
                string headingNegativeStyle = aHeadingNegativeStyle;
                string totalEditableStyle = aTotalEditableStyle;
                string totalNegativeStyle = aTotalNegativeStyle;
                string detailEditableStyle = aDetailEditableStyle;
                string detailNegativeStyle = aDetailNegativeStyle;
                string col0 = null;
                bool useBaseStyle = true;

                headingCells = aHeadingWafer.Cells;
                headingColLabels = aHeadingWafer.ColumnLabels;
                headingRowLabels = aHeadingWafer.RowLabels;
                totalCells = aTotalWafer.Cells;
                totalColLabels = aTotalWafer.ColumnLabels;
                detailCells = aDetailWafer.Cells;
                detailColLabels = aDetailWafer.ColumnLabels;

                for (row = 0; row < headingRowLabels.GetLength(0); row++)
                {
                    i = row;
                    if (aAltHeadingStyle != null)
                    {
                        if (col0 == null)
                        {
                            if (headingRowLabels[i, 0] != null)
                            {
                                col0 = headingRowLabels[i, 0].ToString();
                            }
                        }
                        else
                        {
                            if (headingRowLabels[i, 0] != null &&
                                headingRowLabels[i, 0] != col0)
                            {
                                col0 = headingRowLabels[i, 0].ToString();
                                useBaseStyle = !useBaseStyle;
                                if (useBaseStyle)
                                {
                                    headingStyle = aHeadingStyle;
                                    headingEditableStyle = aHeadingEditableStyle;
                                    headingNegativeStyle = aHeadingNegativeStyle;
                                    totalEditableStyle = aTotalEditableStyle;
                                    totalNegativeStyle = aTotalNegativeStyle;
                                    detailEditableStyle = aDetailEditableStyle;
                                    detailNegativeStyle = aDetailNegativeStyle;
                                }
                                else
                                {
                                    headingStyle = aAltHeadingStyle;
                                    headingEditableStyle = aAltHeadingEditableStyle;
                                    headingNegativeStyle = aAltHeadingNegativeStyle;
                                    totalEditableStyle = aAltTotalEditableStyle;
                                    totalNegativeStyle = aAltTotalNegativeStyle;
                                    detailEditableStyle = aAltDetailEditableStyle;
                                    detailNegativeStyle = aAltDetailNegativeStyle;
                                }
                            }
                        }
                    }

                    aMIDExportFile.AddRow();
                    dataCol = 0;
                    // add row headings
                    rowHeadingColumns = headingRowLabels.GetLength(1);
                    negativeStyle = null;
                    textStyle = headingStyle;
                    for (j = 0; j < headingRowLabels.GetLength(1); j++)
                    {
                        if (g1.Cols[j].Visible)
                        {
                            if (headingRowLabels[i, j] != null)
                            {
                                aMIDExportFile.AddValue(headingRowLabels[i, j].ToString(), eExportDataType.RowHeading, textStyle, negativeStyle);
                            }
                            else
                            {
                                aMIDExportFile.AddValue(" ", eExportDataType.RowHeading, textStyle, negativeStyle);
                            }
                        }
                        ++dataCol;
                    }
                    // add all stores columns
                    negativeStyle = headingNegativeStyle;
                    textStyle = headingEditableStyle;
                    for (j = 0; j < headingCells.GetLength(1); j++)
                    {
                        if (g2.Cols[j].Visible)
                        {
                            if (headingCells[i, j] != null &&
                                headingCells[i, j].CellIsValid)
                            {
                                if (headingCells[i, j].ValueAsString != String.Empty)
                                {
                                    aMIDExportFile.AddValue(headingCells[i, j].ValueAsString, eExportDataType.Value, textStyle, negativeStyle);
                                }
                                else
                                {
                                    aMIDExportFile.AddValue(headingCells[i, j].Value.ToString(), eExportDataType.Value, textStyle, negativeStyle);
                                }
                            }
                            else
                            {
                                aMIDExportFile.AddValue(" ", false, false, 0, eExportDataType.Value, textStyle, negativeStyle);
                            }
                        }
                        ++dataCol;
                    }

                    // add detail columns
                    negativeStyle = detailNegativeStyle;
                    textStyle = detailEditableStyle;
                    for (j = 0; j < detailCells.GetLength(1); j++)
                    {
                        if (g3.Cols[j].Visible)
                        {
                            if (detailCells[i, j] != null &&
                                detailCells[i, j].CellIsValid)
                            {
                                if (detailCells[i, j].ValueAsString != String.Empty)
                                {
                                    aMIDExportFile.AddValue(detailCells[i, j].ValueAsString, eExportDataType.Value, textStyle, negativeStyle);
                                }
                                else
                                {
                                    aMIDExportFile.AddValue(detailCells[i, j].Value.ToString(), eExportDataType.Value, textStyle, negativeStyle);
                                }
                            }
                            else
                            {
                                aMIDExportFile.AddValue(" ", false, false, 0, eExportDataType.Value, textStyle, negativeStyle);
                            }
                        }
                        ++dataCol;
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
		#region Splitters Move Events
		private void s1_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			try
			{
				// If horizontal splitter has moved, reset the other splitter positions
				if (!_hSplitMove)
				{
					_hSplitMove = true;
					if (s1.SplitPosition < g3.Rows[g3.Rows.Count - 1].Bottom + s1.Height + 2)
					{
						s1.SplitPosition = g3.Rows[g3.Rows.Count - 1].Bottom + s1.Height + 2;
					}
					s2.SplitPosition = s1.SplitPosition;
					s3.SplitPosition = s1.SplitPosition;
					s4.SplitPosition = s1.SplitPosition;
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
					if (s2.SplitPosition < g3.Rows[g3.Rows.Count - 1].Bottom + s2.Height + 2)
					{
						s2.SplitPosition = g3.Rows[g3.Rows.Count - 1].Bottom + s2.Height + 2;
					}
					s1.SplitPosition = s2.SplitPosition;
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
					if (s3.SplitPosition < g3.Rows[g3.Rows.Count - 1].Bottom + s3.Height + 2)
					{
						s3.SplitPosition = g3.Rows[g3.Rows.Count - 1].Bottom + s3.Height + 2;
					}
					s1.SplitPosition = s3.SplitPosition;
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
					if (s4.SplitPosition < g3.Rows[g3.Rows.Count - 1].Bottom + s4.Height + 2)
					{
						SetRowSplitPosition4();
					}
					s1.SplitPosition = s4.SplitPosition;
					s2.SplitPosition = s4.SplitPosition;
					s3.SplitPosition = s4.SplitPosition;
					_hSplitMove = false;
				}
				//vScrollBar1.Maximum = (g3.Rows[g3.Rows.Count - 1].Bottom - g3.Height)+ BIGCHANGE;
				//vScrollBar2.Maximum = (g6.Rows[g6.Rows.Count - 1].Bottom - g6.Height)+ BIGCHANGE;
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
				RefreshHorizontalScrollBars(1);
				RefreshHorizontalScrollBars(2);
				RefreshHorizontalScrollBars(3);
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
				RefreshHorizontalScrollBars(2);
				RefreshHorizontalScrollBars(3);
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
				//s4.SplitPosition = g3.Rows[g3.Rows.Count - 1].Bottom + s4.Height + 2;
				s4.SplitPosition = g1.Rows[g1.Rows.Count - 1].Bottom + s4.Height + 2;
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
				s8.SplitPosition = g4.Rows[g4.Rows.Count-1].Bottom + s8.Height; 
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
                // Begin TT#1717-MD - JSmith - Summary Review for Group recieves Argument Out Of Range Exception 
                //int gHeight, rHeight = 0;
                //gHeight = g7.Height;
                //for (int i = g7.TopRow; i <= g7.BottomRow; i++)
                //{
                //    rHeight += g7.Rows[i].HeightDisplay;
                //}

                //if (g10.Rows.Count > 4)
                //    //	&& (gHeight <= rHeight))
                //    s12.SplitPosition = g10.Rows[4].Bottom + s12.Height ;
                //else
                //    s12.SplitPosition = g10.Rows[g10.Rows.Count - 1].Bottom + s12.Height ;
						
                //if (gHeight < rHeight)
                //    s12.SplitPosition = s12.SplitPosition - (rHeight - gHeight); 

                if (!FormIsClosing)
                    s12.SplitPosition = g10.Rows[g10.Rows.Count - 1].Bottom + s12.Height + 0;
                // End TT#1717-MD - JSmith - Summary Review for Group recieves Argument Out Of Range Exception 
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
				VerticalSplitter1.SplitPosition = g4.Cols[g4.Cols.Count - 1].Right + VerticalSplitter1.Width + 3;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void SetV2SplitPosition()
		{
			try
			{
				VerticalSplitter2.SplitPosition = g2.Cols[g2.Cols.Count - 1].Right + VerticalSplitter2.Width + 3;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		} 
		#endregion

		#region ScrollBars Scroll Events
		private void RefreshHorizontalScrollBars(int WhichOneToRefresh)
		{
			try
			{
				switch(WhichOneToRefresh)
				{
					case 1:
						SetHScrollBar1Parameters();
						break;
					case 2:
						SetHScrollBar2Parameters();
						break;
					case 3:
						SetHScrollBar3Parameters();
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
		private void SetVScrollBar2Parameters()
		{
			try
			{
				_rowsPerGroup4 = 1;
				vScrollBar2.Minimum = 0;
				vScrollBar2.Maximum = CalculateRowMaximumScroll(g4, _rowsPerGroup4);
				vScrollBar2.SmallChange = SMALLCHANGE;
				vScrollBar2.LargeChange = BIGCHANGE;
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
				_rowsPerGroup7 = 1;
				vScrollBar3.Minimum = 0;
				vScrollBar3.Maximum = CalculateRowMaximumScroll(g7, _rowsPerGroup7);
				vScrollBar3.SmallChange = SMALLCHANGE;
				vScrollBar3.LargeChange = BIGCHANGE;
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
				_rowsPerGroup10 = 1;
				vScrollBar4.Minimum = 0;
				vScrollBar4.Maximum = CalculateRowMaximumScroll(g10, _rowsPerGroup10);
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
				g7.TopRow = ((VScrollBar)sender).Value * _rowsPerGroup7;
				g8.TopRow = ((VScrollBar)sender).Value * _rowsPerGroup7;
				g9.TopRow = ((VScrollBar)sender).Value * _rowsPerGroup7;
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
				RefreshHorizontalScrollBars(1);
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
				_scroll2EventType = e.Type; 
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void hScrollBar2_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				_isScrolling = true;
				g2.LeftCol = hScrollBar2.Value;
				g5.LeftCol = hScrollBar2.Value;
				g8.LeftCol = hScrollBar2.Value;
				g11.LeftCol = hScrollBar2.Value;
					
				switch(_scroll2EventType)
				{
					case ScrollEventType.LargeIncrement:
					case ScrollEventType.SmallIncrement:
						if (g2.LeftCol == g2.RightCol)
							VerticalSplitter2.SplitPosition = g2.Cols[g2.LeftCol].WidthDisplay + VerticalSplitter2.Width + 3;
						break;
				}
				RefreshHorizontalScrollBars(2);
				_isScrolling = false;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			
		}
		private void hScrollBar3_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			try
			{    
				_scroll3EventType = e.Type; 
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void hScrollBar3_ValueChanged(object sender, System.EventArgs e)
		{
			_isScrolling = true;
			g3.LeftCol = hScrollBar3.Value;
			g6.LeftCol = hScrollBar3.Value;
			g9.LeftCol = hScrollBar3.Value;
			g12.LeftCol = hScrollBar3.Value;
			if (g3.RightCol == g3.Cols.Count - 1)
			{
				g3.ShowCell(0,g3.Cols.Count - 1);
				g6.ShowCell(0,g3.Cols.Count - 1);
				g9.ShowCell(0,g3.Cols.Count - 1);
				g12.ShowCell(0,g3.Cols.Count - 1);
			}
			RefreshHorizontalScrollBars(3);
			_isScrolling = false;
		}

	
		private void SetHScrollBarParameters()
		{
			SetHScrollBar1Parameters();
			SetHScrollBar2Parameters();
			SetHScrollBar3Parameters();
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
			{
				maxScroll = CalculateColMaximumScroll(g2, 1);
				hScrollBar2.Minimum = 0;
				hScrollBar2.Maximum = maxScroll + _colsPerGroup2 - 1;
				hScrollBar2.SmallChange = SMALLCHANGE;
				hScrollBar2.LargeChange = _colsPerGroup2;
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
				maxScroll = CalculateColMaximumScroll(g3, 1);
				if (rbGroupByStoreGrade.Checked)
				{
					hScrollBar3.Minimum = _minFrozenCols;
					hScrollBar3.Maximum = maxScroll + _colsPerGroup3 +  _minFrozenCols - 1;
				}
				else
				{
					hScrollBar3.Minimum = 0;
					hScrollBar3.Maximum = maxScroll + _colsPerGroup3 - 1;
				}
				hScrollBar3.SmallChange = SMALLCHANGE;
				hScrollBar3.LargeChange = _colsPerGroup3;
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
							mnuFreezeColumn.Checked = g1HasColsFrozen;
//							_isSorting = false;
							break;
						case FromGrid.g2:
							mnuFreezeColumn.Checked = g2HasColsFrozen;
//							_isSorting = false;
							break;	
						case FromGrid.g3:
							mnuFreezeColumn.Checked = g3HasColsFrozen;
//							_isSorting = false;
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
									//if (GridMouseRow == 1 && rbHeader.Checked) 
									//	setDragReady = true;
									//else if (GridMouseRow == 0 && rbComponent.Checked) 
									//	setDragReady = true;
									if (GridMouseRow == 1) 
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
//						_isSorting = true;
						DragStartColumn = GridMouseCol;
					}
//					else if (dragState == DragState.dragResize)
//					{
//						if ( (FromGrid)whichGrid == FromGrid.g1 )
//							_isSorting = false;
//						else if ( GridMouseRow != 2 )
//							_isSorting = false;
//					}
					//else
					//	_isSorting = false;
				}		
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
			if (!_loading)
			{
				grid = (C1FlexGrid)sender;
				TagForGridData DataTag = (TagForGridData)grid.GetCellRange(e.Row, e.Col).UserData;
				if (!DataTag.IsEditable)
					e.Cancel = true;
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
			double cellValue;
			AllocationWafer wafer = null;
			AllocationWaferCell [,] cells ;
			int waferRow = 0, waferCol = 0, whichGrid;
			try
			{
				grid = (C1FlexGrid)sender;
				whichGrid = ((GridTag)grid.Tag).GridId;
				switch((FromGrid)whichGrid)
				{
					case FromGrid.g5:
						wafer = _wafers[0,0];
						waferRow = 0;
						waferCol = 0;
						break;
					case FromGrid.g6:
						wafer = _wafers[0,1];
						waferRow = 0;
						waferCol = 1;
						break;
					case FromGrid.g8:
						wafer = _wafers[1,0];
						waferRow = 1;
						waferCol = 0;
						break;
					case FromGrid.g9:
						wafer = _wafers[1,1];
						waferRow = 1;
						waferCol = 1;
						break;
					case FromGrid.g11:
						wafer = _wafers[2,0];
						waferRow = 2;
						waferCol = 0;
						break;
					case FromGrid.g12:
						wafer = _wafers[2,1];
						waferRow = 2;
						waferCol = 1;
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
				
				if (cellValue < cells[e.Row,e.Col].MinimumValue)
				{	
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_MinimumValueExceeded,
						String.Format
						(
						MIDText.GetText(eMIDTextCode.msg_al_MinimumValueExceeded),
						cellValue.ToString(CultureInfo.CurrentUICulture),
						cells[e.Row,e.Col].MinimumValue.ToString(CultureInfo.CurrentUICulture) 
						));
				}	 
				if (cells[e.Row,e.Col].MayExceedPrimaryMaximum == false
					&& cellValue > cells[e.Row,e.Col].PrimaryMaximumValue)
				{	
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_MaximumValueExceeded,
						String.Format
						(
						MIDText.GetText(eMIDTextCode.msg_al_MaximumValueExceeded),
						cellValue.ToString(CultureInfo.CurrentUICulture),
						cells[e.Row,e.Col].PrimaryMaximumValue.ToString(CultureInfo.CurrentUICulture) 
						));
				}	 	
				
				if (cells[e.Row,e.Col].MayExceedGradeMaximum == false
					&& cellValue > cells[e.Row,e.Col].GradeMaximumValue)
				{	
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_MaximumValueExceeded,
						String.Format
						(
						MIDText.GetText(eMIDTextCode.msg_al_MaximumValueExceeded),
						cellValue.ToString(CultureInfo.CurrentUICulture),
						cells[e.Row,e.Col].GradeMaximumValue.ToString(CultureInfo.CurrentUICulture) 
						));
				}	
				if (cellValue % cells[e.Row,e.Col].Multiple != 0)
				{	
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_al_MultipleValueIncorrect,
						String.Format
						(
						MIDText.GetText(eMIDTextCode.msg_al_MultipleValueIncorrect),
						cellValue.ToString(CultureInfo.CurrentUICulture),
						cells[e.Row,e.Col].Multiple.ToString(CultureInfo.CurrentUICulture) 
						));
				}	
				
                // begin TT#59 Implement Temp Locks
                //_trans.SetAllocationCellValue( waferRow,  waferCol,
				//	e.Row, e.Col, System.Convert.ToDouble(grid[e.Row, e.Col])); 
                _allocationWaferCellChangeList.AddAllocationWaferCellChange(waferRow, waferCol, e.Row, e.Col, System.Convert.ToDouble(grid[e.Row, e.Col]));
                // end TT#59 Implement Temp Locks
                ChangePending = true;
				CellRange cellRange = grid.GetCellRange(e.Row, e.Col);
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
			}	
			catch (MIDException MIDexc)
			{
				switch (MIDexc.ErrorLevel)
				{
					case eErrorLevel.fatal:
						HandleException(MIDexc);
						break;

					case eErrorLevel.information:
					case eErrorLevel.warning:	
					case eErrorLevel.severe:
						HandleMIDException(MIDexc);
						break;
				}
				grid[e.Row, e.Col] = _holdValue;
			}
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
				RefreshHorizontalScrollBars(1);
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
				RefreshHorizontalScrollBars(2);
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
				RefreshHorizontalScrollBars(3);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#endregion

		#region Code related to drag and drop of g2 and g3
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
//					_isSorting = false;
					dragState = DragState.dragStarted;
					grid = (C1FlexGrid)sender;
					whichGrid = ((GridTag)grid.Tag).GridId;
				 
					switch((FromGrid)whichGrid)
					{
						case FromGrid.g2:
							g3.DropMode = DropModeEnum.None;
							g2.DoDragDrop(sender, DragDropEffects.All);
							break;
						case FromGrid.g3:
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
					case FromGrid.g2:
						g3.DropMode = DropModeEnum.Manual;
						break;
					case FromGrid.g3:
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
						case FromGrid.g2:
							g3.DropMode = DropModeEnum.Manual;
							break;
						case FromGrid.g3:
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
				}
				else if ((e.KeyState & 0x01) == 0)
				{
					if (dragState == DragState.dragNone)
					{
						e.Action = DragAction.Cancel;
						setDropToManual = true;
					}
					else
					{
						e.Action = DragAction.Drop;
						setDropToManual = true;
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
						case FromGrid.g2:
							g3.DropMode = DropModeEnum.Manual;
							break;
						case FromGrid.g3:
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
 
				if (DragStartColumn < DragStopColumn) //we are moving from left to right
				{
					try
					{
						g2.Cols.MoveRange(DragStartColumn, 1, DragStopColumn);
						g5.Cols.MoveRange(DragStartColumn, 1, DragStopColumn); 
						g8.Cols.MoveRange(DragStartColumn, 1, DragStopColumn); 
					}
					catch (Exception ex)
					{
						HandleException(ex);
					}
				}
				
				if (DragStartColumn > DragStopColumn) //move right to left
				{
					try
					{
						g2.Cols.MoveRange(DragStartColumn, 1, DragStopColumn); 
						g5.Cols.MoveRange(DragStartColumn, 1, DragStopColumn); 
						g8.Cols.MoveRange(DragStartColumn, 1, DragStopColumn); 
					}
					catch (Exception ex)
					{
						HandleException(ex);
					}
				}
				
				//Finally, we want to clear the dragState. 
				//This is an important clean-up step.
				dragState = DragState.dragNone;
				g3.DropMode=DropModeEnum.Manual;
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

				//The following two variables are used to represent the equivalent column number
				//to the first group. For example, say we have columns 0, 1, 2, 3, 4, 5. Assume we
				//have two groups (be it 2 weeks or 2 variables), then columns 0, 1, 2 belongs
				//to the first group and columns 3, 4, 5 belongs to the second group. Column 
				//#3 will be the equivalent of column 0 in group TWO, and column #4 will be the
				//equivalent of column 1 in group TWO, and so on.
				int ColNum1stGroupEquiv_Start; //the column that the user began the drag.
				int ColNum1stGroupEquiv_Stop; //the column that the user stop the drag (or made the drop).

				int i; //temp variable for looping uses.
				
				//The following two variables are temporarily used to represent the columns that the
				//user began and stopped the drag, but it will be a calculated value once
				//we get into looping through the groups.
				int StartCol;
				int StopCol;

				int NumColsInOneGroup = g3.Cols.Count/WeekOrVariableGroups; //the number of columns in one group of data.

				ColNum1stGroupEquiv_Start = DragStartColumn % (g3.Cols.Count / WeekOrVariableGroups);
				ColNum1stGroupEquiv_Stop = 	DragStopColumn % (g3.Cols.Count / WeekOrVariableGroups);
			
				if (DragStartColumn < DragStopColumn) //we are moving from left to right
				{
					try
					{
						for (i = 0; i < WeekOrVariableGroups; i++)
						{
							StartCol = i * NumColsInOneGroup + ColNum1stGroupEquiv_Start;
							StopCol = i * NumColsInOneGroup + ColNum1stGroupEquiv_Stop;

							//for some reason, we must use "+ 1" to the stop column index
							//in order to get the columns to move correctly. Also, we're always
							//moving just one column at a time.
							g3.Cols.MoveRange(StartCol, 1, StopCol);
							g6.Cols.MoveRange(StartCol, 1, StopCol);
							g9.Cols.MoveRange(StartCol, 1, StopCol);
						}
					}
					catch (Exception ex)
					{
						HandleException(ex);
					}
					
				}
				else if (DragStartColumn > DragStopColumn) //move right to left
				{
					try
					{
						for (i = 0; i < WeekOrVariableGroups; i++)
						{
							StartCol = i * NumColsInOneGroup + ColNum1stGroupEquiv_Start;
							StopCol = i * NumColsInOneGroup + ColNum1stGroupEquiv_Stop;
						
							g3.Cols.MoveRange(StartCol, 1, StopCol); 
							g6.Cols.MoveRange(StartCol, 1, StopCol);
							g9.Cols.MoveRange(StartCol, 1, StopCol); 
						}
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
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#endregion
	
		#region Code related to the "Row/Column Chooser" context menu
		private void mnuColumnChooser_Click(object sender, System.EventArgs e)
		{
			try
			{
				ArrayList ColumnHeaders = new ArrayList();

				if (RightClickedFrom == FromGrid.g2)
				{
					//Assumption: g2 is always grouped by time
					for (int col = 0; col < g2.Cols.Count; col++)
					{
						//make a new "RowColHeader" object which will be later added to the "ColumnHeaders" arraylist.
						RowColHeader ach = new RowColHeader();

						//get the tag for the column of the current loop position.
						TagForColumn colTag = new TagForColumn();
						colTag = (TagForColumn)g2.Cols[col].UserData;

						//Assign values to the RowColHeader object.
						ach.Name = g2.GetData(2, col).ToString();
						ach.IsDisplayed = colTag.IsDisplayed;

						//Add this RowColHeader object to the ArrayList.
						ColumnHeaders.Add(ach);
					}
				}
				else if (RightClickedFrom == FromGrid.g3)
				{
					if (rbGroupByAttribute.Checked == true)
					{
						for (int col = 0; col < g3.Cols.Count/WeekOrVariableGroups; col++)
						{
							//make a new "RowColHeader" object which will be later added to the "ColumnHeaders" arraylist.
							RowColHeader ach = new RowColHeader();

							//get the tag for the column of the current loop position.
							TagForColumn colTag = new TagForColumn();
							colTag = (TagForColumn)g3.Cols[col].UserData;

							//Assign values to the RowColHeader object.
							ach.Name = g3.GetData(2, col).ToString();
							ach.IsDisplayed = colTag.IsDisplayed;

							//Add this RowColHeader object to the ArrayList.
							ColumnHeaders.Add(ach);
						}
					}
					else if (rbGroupByStoreGrade.Checked == true)
					{
						for (int col = 0; col < g3.Cols.Count; col += g3.Cols.Count/WeekOrVariableGroups)
						{
							//make a new "RowColHeader" object which will be later added to the "ColumnHeaders" arraylist.
							RowColHeader ach = new RowColHeader();

							//get the tag for the column of the current loop position.
							TagForColumn colTag = new TagForColumn();
							colTag = (TagForColumn)g3.Cols[col].UserData;

							//Assign values to the RowColHeader object.
							ach.Name = g3.GetData(1, col).ToString();
							ach.IsDisplayed = colTag.IsDisplayed;

							//Add this RowColHeader object to the ArrayList.
							ColumnHeaders.Add(ach);
						}
					}
				}

// Begin Track #4868 - JSmith - Variable Groupings
				//RowColChooser frm = new RowColChooser(ColumnHeaders, true, "Column Chooser");
                RowColChooser frm = new RowColChooser(ColumnHeaders, true, "Column Chooser", null);
// End Track #4868
				if (frm.ShowDialog(this) == DialogResult.OK)
				{
					ShowHideHeaders(frm.Headers);
				}
				frm.Dispose();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
//		private void mnuRowChooser_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				ArrayList rowHeaders = new ArrayList();
//
//				//assign the correct arraylist to be passed in to the CHOOSER form.
//				switch (RightClickedFrom)
//				{
//					case FromGrid.g4:
//						rowHeaders = RowHeaders4;
//						break;
//					case FromGrid.g7:
//						rowHeaders = RowHeaders7;
//						break;
//					default:
//						break;
//				}
//
//				//display the CHOOSER form and show/hide rows.
//				//this.Cursor = Cursors.WaitCursor;
//				RowColChooser frm = new RowColChooser(rowHeaders, false);
//				if (frm.ShowDialog() == DialogResult.OK)
//				{
//					ShowHideHeaders(frm.Headers);
//				}
//
//				frm.Dispose();
//				ResizeRows();
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex);
//			}
//		}	
		private void ShowHideHeaders(ArrayList RowColHeaders)
		{
			int i, j;
			RowColHeader rch;
			TagForRow RowTag;

			try
			{
				switch (RightClickedFrom)//if (RightClickedFrom == FromGrid.g2)
				{
					case FromGrid.g2:
						for (i = 0; i < g2.Cols.Count; i++)
						{
							RowColHeader ch = (RowColHeader)RowColHeaders[i];

							//update the column tag for g2.
							TagForColumn ColumnTag = new TagForColumn();
							ColumnTag = (TagForColumn)g2.Cols[i].UserData;
							ColumnTag.IsDisplayed = ch.IsDisplayed;
							g2.Cols[i].UserData = ColumnTag;

							//show/hide relevant columns.
							g2.Cols[i].Visible = ch.IsDisplayed;
							g5.Cols[i].Visible = ch.IsDisplayed;
							g8.Cols[i].Visible = ch.IsDisplayed;
						}

						//The following code is related to re-adjusting the totals panel
						//so that if the number of columns displayed is less than what it
						//used to be, we want to "shrink" this panel so it doesn't take
						//up unnecessary space.
						int SplitterMaxPossiblePosition = g2.Cols[g2.Cols.Count - 1].Right + VerticalSplitter2.Width;
						if (SplitterMaxPossiblePosition <= UserSetSplitter2Position)
						{
							int temp = UserSetSplitter2Position;
						
							//Move the splitter to "shrink" the TOTALS panel.
							VerticalSplitter2.SplitPosition = SplitterMaxPossiblePosition;

							//Retain the splitter position the user set earlier.
							UserSetSplitter2Position = temp;
						}
						else if (SplitterMaxPossiblePosition > UserSetSplitter2Position)
						{
							//We'll only allow the TOTALS panel to grow back to whatever
							//width the user set it previously.
							VerticalSplitter2.SplitPosition = UserSetSplitter2Position;
						}
			
						hScrollBar2.Minimum=0;
						hScrollBar2.Maximum = (g2.Cols[g2.Cols.Count - 1].Right - g2.Width) + BIGCHANGE;

						break;
				
					case FromGrid.g3: 
						if (rbGroupByAttribute.Checked == true)
						{
							int numColsInGroup = g3.Cols.Count / WeekOrVariableGroups;
							for (i = 0; i < g3.Cols.Count; i+=numColsInGroup)
							{
								for (j = 0; j < numColsInGroup; j++)
								{
									RowColHeader ch = (RowColHeader)RowColHeaders[j];

									//update the column tag for g3.
									TagForColumn ColumnTag = new TagForColumn();
									ColumnTag = (TagForColumn)g3.Cols[i+j].UserData;
									ColumnTag.IsDisplayed = ch.IsDisplayed;
									g3.Cols[i+j].UserData = ColumnTag;

									//show/hide relevant columns.
									g3.Cols[i+j].Visible = ch.IsDisplayed;
									g6.Cols[i+j].Visible = ch.IsDisplayed;
									g9.Cols[i+j].Visible = ch.IsDisplayed;
								}
							}
						}
						else if (rbGroupByStoreGrade.Checked == true)
						{
							int numColsInGroup = g3.Cols.Count / WeekOrVariableGroups;
						
							for (i = 0; i < WeekOrVariableGroups; i++)
							{
								RowColHeader ch = (RowColHeader)RowColHeaders[i];

								for (j = i * numColsInGroup; j < (i + 1) * numColsInGroup; j++)
								{								
									//update the column tag for g3.
									TagForColumn ColumnTag = new TagForColumn();
									ColumnTag = (TagForColumn)g3.Cols[j].UserData;
									ColumnTag.IsDisplayed = ch.IsDisplayed;
									g3.Cols[j].UserData = ColumnTag;

									//show/hide relevant columns.
									g3.Cols[j].Visible = ch.IsDisplayed;
									g6.Cols[j].Visible = ch.IsDisplayed;
									g9.Cols[j].Visible = ch.IsDisplayed;
								}
							}
						}

						hScrollBar3.Minimum=0;
						hScrollBar3.Maximum = (g3.Cols[g3.Cols.Count - 1].Right - g3.Width) + BIGCHANGE;

						break;

					case FromGrid.g4:						
						for (i = 0; i < g4.Rows.Count; i += 4)
						{
							//% to Change:
							rch = (RowColHeader)RowColHeaders[0];
							//update the row tag for g4
							RowTag = new TagForRow();
							RowTag = (TagForRow)g4.Rows[i+1].UserData;
							RowTag.IsDisplayed = rch.IsDisplayed;
							g4.Rows[i+1].UserData = RowTag;
							//show/hide relevant rows.
							g4.Rows[i+1].Visible = rch.IsDisplayed;
							g5.Rows[i+1].Visible = rch.IsDisplayed;
							g6.Rows[i+1].Visible = rch.IsDisplayed;
							if (RowTag.IsDisplayed == true)
								g4.Rows[i+1].HeightDisplay = 50;

							//% to Total:
							rch = (RowColHeader)RowColHeaders[1]; 
							//update the row tag
							RowTag = new TagForRow();
							RowTag = (TagForRow)g4.Rows[i+2].UserData;
							RowTag.IsDisplayed = rch.IsDisplayed;
							g4.Rows[i+2].UserData = RowTag;
							//show/hide relevant rows.
							g4.Rows[i+2].Visible = rch.IsDisplayed;
							g5.Rows[i+2].Visible = rch.IsDisplayed;
							g6.Rows[i+2].Visible = rch.IsDisplayed;
							if (RowTag.IsDisplayed == true)
								g4.Rows[i+2].HeightDisplay = 50;

							//% to Set:
							rch = (RowColHeader)RowColHeaders[2]; 
							//update the row tag
							RowTag = new TagForRow();
							RowTag = (TagForRow)g4.Rows[i+3].UserData;
							RowTag.IsDisplayed = rch.IsDisplayed;
							g4.Rows[i+3].UserData = RowTag;
							//show/hide relevant rows.
							g4.Rows[i+3].Visible = rch.IsDisplayed;
							g5.Rows[i+3].Visible = rch.IsDisplayed;
							g6.Rows[i+3].Visible = rch.IsDisplayed;
							if (RowTag.IsDisplayed == true)
								g4.Rows[i+3].HeightDisplay = 50;
						}
						UpdateVisibleRowsPerStoreGroup(FromGrid.g4);
						break;

					case FromGrid.g7:
						for (i = 0; i < g7.Rows.Count; i += 4)
						{
							//% to Change:
							rch = (RowColHeader)RowColHeaders[0];
							//update the row tag for g7
							RowTag = new TagForRow();
							RowTag = (TagForRow)g7.Rows[i+1].UserData;
							RowTag.IsDisplayed = rch.IsDisplayed;
							g7.Rows[i+1].UserData = RowTag;
							//show/hide relevant rows.
							g7.Rows[i+1].Visible = rch.IsDisplayed;
							g8.Rows[i+1].Visible = rch.IsDisplayed;
							g9.Rows[i+1].Visible = rch.IsDisplayed;
							if (RowTag.IsDisplayed == true)
								g7.Rows[i+1].HeightDisplay = 50;

							//% to Total:
							rch = (RowColHeader)RowColHeaders[1]; 
							//update the row tag
							RowTag = new TagForRow();
							RowTag = (TagForRow)g7.Rows[i+2].UserData;
							RowTag.IsDisplayed = rch.IsDisplayed;
							g7.Rows[i+2].UserData = RowTag;
							//show/hide relevant rows.
							g7.Rows[i+2].Visible = rch.IsDisplayed;
							g8.Rows[i+2].Visible = rch.IsDisplayed;
							g9.Rows[i+2].Visible = rch.IsDisplayed;
							if (RowTag.IsDisplayed == true)
								g7.Rows[i+2].HeightDisplay = 50;

							//% to Set:
							rch = (RowColHeader)RowColHeaders[2]; 
							//update the row tag
							RowTag = new TagForRow();
							RowTag = (TagForRow)g7.Rows[i+3].UserData;
							RowTag.IsDisplayed = rch.IsDisplayed;
							g7.Rows[i+3].UserData = RowTag;
							//show/hide relevant rows.
							g7.Rows[i+3].Visible = rch.IsDisplayed;
							g8.Rows[i+3].Visible = rch.IsDisplayed;
							g9.Rows[i+3].Visible = rch.IsDisplayed;
							if (RowTag.IsDisplayed == true)
								g7.Rows[i+3].HeightDisplay = 50;
						}
						UpdateVisibleRowsPerStoreGroup(FromGrid.g7);
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
		
		private void UpdateVisibleRowsPerStoreGroup (FromGrid WhichGrid)
		{
			int i;
			int InvisiblePctRows = 0;
			TagForRow rowTag = new TagForRow();
			
			switch (WhichGrid)
			{
				case FromGrid.g4:
					for (i = 3; i >= 1; i--)	//1 to 3 is "% to Change", "% to Total", and "% to Set"
					{
						rowTag = (TagForRow)g4.Rows[i].UserData;
						if (rowTag.IsDisplayed == false)
							InvisiblePctRows ++;
						else
							//we only want to count the number of invisible rows from the back 
							//(until we hit a visible row. Then everything befor that doesn't matter).
							break;
					}
					LastInvisibleRowsPerStoreGroup4 = InvisiblePctRows;
					break;
				case FromGrid.g7:
					for (i = 3; i >= 1; i--)	//1 to 3 is "% to Change", "% to Total", and "% to Set"
					{
						rowTag = (TagForRow)g7.Rows[i].UserData;
						if (rowTag.IsDisplayed == false)
							InvisiblePctRows ++;
						else
							//we only want to count the number of invisible rows from the back 
							//(until we hit a visible row. Then everything befor that doesn't matter).
							break;
					}
					LastInvisibleRowsPerStoreGroup7 = InvisiblePctRows;
					break;				
				default:
					break;
			}
		}
		#endregion
	
		#region Group By Attribute or Group By Volume Group
		private void rbGroupByStoreGrade_CheckedChanged(object sender, System.EventArgs e)
		{	
			// For some reason, changing cmbAttributeSet.Enabled fires the
			// _SelectionChangeCommitted event which we don't want so 
			// the _rbToggled switch is set to bypass that event.
			_rbToggled = true; 
			cmbAttributeSet.Enabled = rbGroupByStoreGrade.Checked;
			_rbToggled = false; 
			//GridViews.SummaryViewIsGroupedByVolumnGroup = rbGroupByStoreGrade.Checked;
            if (!rbGroupByStoreGrade.Checked) return;
			Cursor.Current = Cursors.WaitCursor;
			_trans.AllocationGroupBy = Convert.ToInt32(eAllocationSummaryViewGroupBy.StoreGrade);

            //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
            int indexCurrent = this.cmbAttributeSet.SelectedIndex; //Current Index.

            this.cmbAttributeSet.SelectedValue = _trans.AllocationStoreGroupLevel;

            int indexChk = this.cmbAttributeSet.SelectedIndex;  //New index if changed.
            if (indexChk != indexCurrent)
            {
                //this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs());
                // TT#294-MD - RBeck - When Opening style review, the view does not open that is selected
            }
            //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly

			_trans.RebuildWafers();
			_wafers = _trans.AllocationWafers;
			FormatGrids1to12();
			
			g3.Cols.Frozen = _minFrozenCols;
			g6.Cols.Frozen = _minFrozenCols;
			g9.Cols.Frozen = _minFrozenCols;
			g12.Cols.Frozen = _minFrozenCols; 
			g3HasColsFrozen = false;	 
			ChangePending = true;
			Cursor.Current = Cursors.Default;
			_rbToggled = false; 
		}

		private void rbGroupByAttribute_CheckedChanged(object sender, System.EventArgs e)
		{
			if (_loading) return;
			if (!rbGroupByAttribute.Checked) return;
			Cursor.Current = Cursors.WaitCursor;
			_trans.AllocationGroupBy = Convert.ToInt32(eAllocationSummaryViewGroupBy.Attribute);
			_trans.RebuildWafers();
			_wafers = _trans.AllocationWafers;
			FormatGrids1to12();
			g3.Cols.Frozen = 0;
			g6.Cols.Frozen = 0;
			g9.Cols.Frozen = 0;
			g12.Cols.Frozen = 0; 
			g3HasColsFrozen = false;	 
			ChangePending = true;
			hScrollBar3.Value = 0;
			Cursor.Current = Cursors.Default;
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
			SortToDefault();
		}
		private void mnuSortByDefault_Click(object sender, System.EventArgs e)
		{
			SortToDefault();
		}
		private void SortToDefault()
		{
			//First, we'll sort the store sets in assending order.
			SortString(0, 0, g7.Rows.Count - rowsPerStoreGroup, SortEnum.asc);			
		}
		
		private void SortString(int ColToSort, int Lo, int Hi, SortEnum SortDirection)
		{
			int lo, hi;
			string mid;
			
			lo = Lo;
			hi = Hi;
			
			//order data from smaller to larger
			if (Hi > Lo)
			{
				//Arbitrarily establishing partition element as the midpoint
				//(The following formula seems like it can be algebracally reduced to just
				//(Lo + Hi)/2, but we must round the result to be divisible by the
				//number of rows per store group. That's why I'm dividing AND multiplying
				//by the "rowsPerStoreGroup" variable.
				int middleRow = (Lo/rowsPerStoreGroup + Hi/rowsPerStoreGroup)/2 * rowsPerStoreGroup;  
				mid = g7[middleRow, ColToSort].ToString();

				//loop through the list until indices cross
				while (lo <= hi)
				{
					//Find the first element that is greater than or equal to
					//the partition element starting from the left index.
					while ((lo < Hi) && (g7[lo, ColToSort].ToString().CompareTo(mid) < 0))
						lo += rowsPerStoreGroup;

					//find an element that is smaller than or equal to 
					//the partition element starting from the right index
					while ((hi > Lo) && (g7[hi, ColToSort].ToString().CompareTo(mid) > 0))
						hi -= rowsPerStoreGroup;

					//if the indexes have not crossed, swap
					if (lo < hi)
					{
						SwitchRowPositions(lo, hi);

						lo += rowsPerStoreGroup;
						hi -= rowsPerStoreGroup;
					}
					else if (lo == hi)
					{
						lo += rowsPerStoreGroup;
						hi -= rowsPerStoreGroup;
					}
				}

				//if the right index has not reached the left side of the list,
				//sort the left partition.
				if (Lo < hi)
					SortString(ColToSort, Lo, hi, SortDirection);

				//if the left index has not reached the right side of the list,
				//sort the right partition.
				if (lo < Hi)
					SortString(ColToSort, lo, Hi, SortDirection);
			}
		}
		private void SwitchRowPositions(int OldIndex, int NewIndex)
		{
			//this procedure is solely used by sorting columns.

			try
			{				
				g7.Rows.MoveRange(OldIndex, rowsPerStoreGroup, NewIndex);
				g7.Rows.MoveRange(NewIndex - rowsPerStoreGroup, rowsPerStoreGroup, OldIndex);
				g8.Rows.MoveRange(OldIndex, rowsPerStoreGroup, NewIndex);
				g8.Rows.MoveRange(NewIndex - rowsPerStoreGroup, rowsPerStoreGroup, OldIndex);
				g9.Rows.MoveRange(OldIndex, rowsPerStoreGroup, NewIndex);
				g9.Rows.MoveRange(NewIndex - rowsPerStoreGroup, rowsPerStoreGroup, OldIndex);
						
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#endregion		

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cmbAttributeSet_SelectionChangeCommitted(object sender, System.EventArgs e)
        private void cmbAttributeSet_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			if (!_loading && !_rbToggled && Convert.ToInt32(cmbAttributeSet.SelectedValue, CultureInfo.CurrentUICulture) > 0 ) 
			{
				_trans.AllocationStoreGroupLevel = Convert.ToInt32(cmbAttributeSet.SelectedValue, CultureInfo.CurrentUICulture);
				CriteriaChanged();
				_showCol = string.Empty;
			}
		}
		private void btnApply_Click(object sender, System.EventArgs e)
		{
			try
			{
                _trans.SetAllocationCellValue(this._allocationWaferCellChangeList); // TT#59 Implement Temp Locks
                bool useLeftCol = false;
				_curg3LeftCol = g3.LeftCol;
				SaveCurrentSettings();
				if (g3.RightCol == (g3.Cols.Count - 1))
					_showCol = "right";
				else
				{
					_showCol = "left";
					useLeftCol = true;
				}
				if (!FormIsClosing)
				{
					_isScrolling = true; 
					CriteriaChanged();
					if (useLeftCol) 
						hScrollBar3.Value = _curg3LeftCol;
					else
						hScrollBar3.Value = hScrollBar3.Maximum - 2;
					_isScrolling = false;
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
        // begin TT#241 - MD JEllis - Header Enqueue Process
        private void CloseOtherViews()  
        {
            MIDRetail.Windows.StyleView frmStyleView;
            MIDRetail.Windows.SizeView frmSizeView;
            MIDRetail.Windows.AssortmentView frmAssortmentView;
            try
            {
                if (_trans.StyleView != null)
                {
                    frmStyleView = (MIDRetail.Windows.StyleView)_trans.StyleView;
                    if (ErrorFound)
                    {
                        frmStyleView.ErrorFound = true;
                    }
                    frmStyleView.Close();
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
        // end TT#241 - MD JEllis - Header Enqueue Process

		public void UpdateOtherViews() 
		{
			MIDRetail.Windows.StyleView frmStyleView;
			MIDRetail.Windows.SizeView frmSizeView;
            MIDRetail.Windows.AssortmentView frmAssortmentView;
            MIDRetail.Windows.AssortmentView frmGroupView; // TT#488 - MD - Jellis - Group Allocation
			try
			{
				SaveCurrentSettings();
				if (_trans.StyleView != null)
				{
					frmStyleView = (MIDRetail.Windows.StyleView)_trans.StyleView;
					frmStyleView.UpdateData();
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
                        frmAssortmentView.UpdateData(true);
                    }
                    else
                    {
                        frmGroupView = _trans.AssortmentView as AssortmentView;
                        frmGroupView.UpdateData(true);
                    }
                    // end TT#488 - MD - Jellis - Group Allocaiton
                }
                if ((_trans.StyleView != null) || (_trans.SizeView != null) || (_trans.AssortmentView != null))
                {
                    GetCurrentSettings();
                }
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#region IFormBase Members
		override public void ICut()
		{
			try
			{
				MessageBox.Show("Not implemented yet");
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		override public void ICopy()
		{
			try
			{
				MessageBox.Show("Not implemented yet");
				
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		override public void IPaste()
		{
			try
			{
				MessageBox.Show("Not implemented yet");

			}		
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}	

//		override public void IClose()
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

		override public void ISave()
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

		override public void ISaveAs()
		{
			try
			{
				MessageBox.Show("Not implemented yet");

			}		
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		override public void IDelete()
		{
			try
			{
				MessageBox.Show("Not implemented yet");

			}		
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}
		public override void IRefresh()
		{
			try
			{
				//MessageBox.Show("Not implemented yet");
				_trans.RefreshSelectedHeaders();
				CriteriaChanged();
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

		override protected bool SaveChanges()
		{
            bool headerSavedOK = true;                   // TT#1185 - Verify ENQ before Update
            try
			{
				if (   _trans.DataState == eDataState.Updatable
                    && FunctionSecurity.AllowUpdate                               // TT#1185 - Verify ENQ before Update 
                    && _headerList != null                                        // TT#1185 - Verify ENQ before Update
                    && _trans.AreHeadersEnqueued(_headerList))                    // TT#1185 - Verify ENQ before Update
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
				ChangePending = false;
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
            return headerSavedOK;  // TT#1185 - Verify ENQ before Update
            //return true;         // TT#1185 - Verify ENQ before Update
        }
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
		public void Navigate( string navTo )
		{
			System.Windows.Forms.Form frm = null;
			MIDRetail.Windows.StyleView frmStyleView;
			MIDRetail.Windows.SizeView frmSizeView;
            MIDRetail.Windows.AssortmentView frmAssortmentView;
			bool okToContinue = true;
			try
			{
				switch (navTo)
				{
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
								_trans.AllocationGroupBy = Convert.ToInt32(eAllocationSizeViewGroupBy.Header);
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

				// BEGIN MID Track #4077 - vertical splitters not correct when window first opens. 
				// only happens in Windows Server 2003 SP1, so this is a workaround
				if (navTo == "Style") 
				{
					((MIDRetail.Windows.StyleView)frm).ResetSplitters();
				}
				// END MID Track #4077
                // Begin TT#199-MD - RMatelic - Column headers not moving with the cells while using arrow keys
                else if (navTo == "Size")
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
			if (rbGroupByAttribute.Checked)
				_curGroupBy = Convert.ToInt32(eAllocationSummaryViewGroupBy.Attribute);
			else
				_curGroupBy = Convert.ToInt32(eAllocationSummaryViewGroupBy.StoreGrade);
            _curAttribute    = _trans.AllocationStoreAttributeID;
			_curAttributeSet = _trans.AllocationStoreGroupLevel;
		}
		public void GetCurrentSettings()	// TT#3808 - Group Allocation - Need Action against Headers receives "DuplicateNameException" error - 
		{
			_trans.AllocationViewType = eAllocationSelectionViewType.Summary;
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
				HandleException (ex, "SummaryView.BeforeClosing");
			}
		}
		override protected void AfterClosing()
		{
			try
			{
				_trans.SummaryView = null;
				_trans.CheckForHeaderDequeue();
			}
			catch (Exception ex)
			{
				HandleException(ex, "SummaryView.AfterClosing");
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

		private void SummaryView_Activated(object sender, System.EventArgs e)
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

		private void SummaryView_Deactivate(object sender, System.EventArgs e)
		{
			SaveCurrentSettings();
		}

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
					if (!OKToProcess((eAllocationActionType)action) )
						return;
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
                        if (_trans.SummaryView != null)
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
                        CriteriaChanged();
                        //Cursor.Current = Cursors.WaitCursor;
                        //UpdateAllocationWorkspace();
                        Cursor.Current = Cursors.Default;
                    }   // TT#241 - MD - JEllis - Header Enqueue Process
				}
			}
            catch (MIDException MIDexc)
			{
                // BEGIN TT#1605 - AGallagher - Balance Size with Constriants when receive the warning message 
                if (MIDexc.ErrorNumber == (int)(eMIDTextCode.msg_al_LockTotalExceedsNew))
                {
                    UpdateOtherViews();
                    if (!FormIsClosing)
                        CriteriaChanged();
                    HandleException(MIDexc);
                }
                else
                {
                    // END TT#1605 - AGallagher - Balance Size with Constriants when receive the warning message 
                    switch (MIDexc.ErrorLevel)
                    {
                        case eErrorLevel.fatal:
                            HandleException(MIDexc);
                            break;

                        case eErrorLevel.information:
                        case eErrorLevel.warning:
                        case eErrorLevel.severe:
                            HandleMIDException(MIDexc);
                            break;
                    }
                }  // TT#1605 - AGallagher - Balance Size with Constriants when receive the warning message 
			}
			catch (Exception ex)
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
				BeforeScroll(g2, null, g5, null, g2, null, hScrollBar2);
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
				BeforeScroll(g3, null, g6, null, g2, null, hScrollBar3);
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
				BeforeScroll(g4, g6, g1, g4, g1, vScrollBar2, hScrollBar1);
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
				BeforeScroll(g5, g4, g2, g4, g2, vScrollBar2, hScrollBar2);
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
				BeforeScroll(g6, g4, g3, g4, g3, vScrollBar2, hScrollBar3);
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
				BeforeScroll(g7, g9, g1, g7, g1, vScrollBar3, hScrollBar1);
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
				BeforeScroll(g8, g7, g2, g7, g2, vScrollBar3, hScrollBar2);
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
				BeforeScroll(g9, g7, g3, g7, g3, vScrollBar3, hScrollBar3);
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
				BeforeScroll(g10, g12, g1, g10, g1, vScrollBar4, hScrollBar1);
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
				BeforeScroll(g11, g10, g2, g10, g2, vScrollBar4, hScrollBar2);
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
				BeforeScroll(g12, g10, g3, g10, g3, vScrollBar4, hScrollBar3);
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
                            // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                            //aVScrollBar.Value = Math.Min(aGrid.TopRow + 1, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1);
                            SetVScrollValue(aVScrollBar, Math.Min(aGrid.TopRow + 1, aVScrollBar.Maximum - aVScrollBar.LargeChange + 1));
                            // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
						}
						else if (aGrid.ScrollPosition.Y > aRowCompGrid.ScrollPosition.Y)
						{
							//aVScrollBar.Value = CalculateGroupFromDetail(aRowHeaderGrid, aGrid.TopRow);
							if (aGrid.TopRow >= 0)
                                // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                //aVScrollBar.Value =  aGrid.TopRow;
                                SetVScrollValue(aVScrollBar, aGrid.TopRow);
                                // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
						}
					}
					if (aColCompGrid != null && aHScrollBar != null)
					{
						if (aGrid.ScrollPosition.X < aColCompGrid.ScrollPosition.X)
						{
							//aHScrollBar.Value = Math.Min(CalculateGroupFromDetail(aColHeaderGrid, aGrid.LeftCol) + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1);
                            // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                            //aHScrollBar.Value = Math.Min(aGrid.LeftCol + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1);
                            SetHScrollValue(aHScrollBar, Math.Min(aGrid.LeftCol + 1, aHScrollBar.Maximum - aHScrollBar.LargeChange + 1));
                            // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
						}
						else if (aGrid.ScrollPosition.X > aColCompGrid.ScrollPosition.X)
						{
							//aHScrollBar.Value = CalculateGroupFromDetail(aColHeaderGrid, aGrid.LeftCol);
							if (aGrid.LeftCol < aHScrollBar.Minimum)
								aHScrollBar.Value = aHScrollBar.Minimum;
							else if (aGrid.LeftCol > aHScrollBar.Maximum)
								aHScrollBar.Value = aHScrollBar.Maximum;
							else
                                // Begin TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
                                //aHScrollBar.Value =  aGrid.LeftCol;
                                SetHScrollValue(aHScrollBar, aGrid.LeftCol);
                                // End TT#1971-MD - JSmith - Receive System Argument Exception when process Need with style review open.
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
					switch((FromGrid)whichGrid)
					{
						case FromGrid.g5:
						case FromGrid.g8:
						case FromGrid.g11:
							_scroll2EventType = ScrollEventType.SmallIncrement;
							break;
						case FromGrid.g6:
						case FromGrid.g9:
						case FromGrid.g12:
							_scroll3EventType = ScrollEventType.SmallIncrement;
							break;
					}
						break;
					
					case Keys.Left:
						switch((FromGrid)whichGrid)
						{
							case FromGrid.g5:
							case FromGrid.g8:
							case FromGrid.g11:
								_scroll2EventType = ScrollEventType.SmallDecrement;
								break;
							case FromGrid.g6:
							case FromGrid.g9:
							case FromGrid.g12:
								_scroll3EventType = ScrollEventType.SmallDecrement;
								break;
						}
							break;
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

		#endregion

        private void cboAction_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboAction_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cmbAttributeSet_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cmbAttributeSet_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cmbFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cmbFilter_SelectionChangeCommitted(source, new EventArgs());
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
}
