//using System;
//using System.Collections;
//using System.ComponentModel;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.Globalization;
//using System.IO;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Windows.Forms;
//using MIDRetail.DataCommon;
//using MIDRetail.Common;
//using MIDRetail.Data;
//using MIDRetail.Business;
//using MIDRetail.Windows.Controls;
//using Infragistics.Win.UltraWinGrid;
//
//namespace MIDRetail.Windows
//{
//	/// <summary>
//	/// Summary description for Filter.
//	/// </summary>
//	public class frmFilter : MIDFormBase
//	{
//		#region Windows Form Designer generated code
//
//		private Rectangle _dragBoxFromMouseDown;
//		private System.Windows.Forms.TabPage tpgAttributes;
//		private System.Windows.Forms.TabPage tpgData;
//		private System.Windows.Forms.Button btnSave;
//		private System.Windows.Forms.Button btnNew;
//		private System.Windows.Forms.Button btnHelp;
//		private System.Windows.Forms.Button btnCancel;
//		private System.Windows.Forms.TabControl tabFilter;
//		private System.Windows.Forms.ContextMenu mnuLabelMenu;
//		private System.Windows.Forms.MenuItem mniDelete;
//		private System.Windows.Forms.Panel pnlAttrQuery;
//		private System.Windows.Forms.Button btnAttrOr;
//		private System.Windows.Forms.Button btnAttrAnd;
//		private System.Windows.Forms.Button btnAttrRParen;
//		private System.Windows.Forms.Button btnAttrLParen;
//		private System.Windows.Forms.Button btnDataOr;
//		private System.Windows.Forms.Button btnDataAnd;
//		private System.Windows.Forms.Button btnDataRParen;
//		private System.Windows.Forms.Button btnDataLParen;
//		private System.Windows.Forms.Panel pnlDataQuery;
//		private System.Windows.Forms.Button btnDataEqual;
//		private System.Windows.Forms.Button btnDataLess;
//		private System.Windows.Forms.Button btnDataGreater;
//		private System.Windows.Forms.Button btnDataLessEqual;
//		private System.Windows.Forms.Button btnDataGreaterEqual;
//		private System.Windows.Forms.Button btnDataNot;
//		private System.Windows.Forms.ListBox lstVariables;
//		private System.Windows.Forms.Label lblVariable;
//		private System.Windows.Forms.Label label1;
//		private System.Windows.Forms.Label lblVersion;
//		private System.Windows.Forms.Label label3;
//		private System.Windows.Forms.ListBox lstVersions;
//		private System.Windows.Forms.Button btnDataPctOf;
//		private System.Windows.Forms.Button btnDataPctChange;
//		private System.Windows.Forms.Button btnDataAny;
//		private System.Windows.Forms.Button btnDataAll;
//		private System.Windows.Forms.Button btnDataAverage;
//		private System.Windows.Forms.Button btnDataTotal;
//		private System.Windows.Forms.TextBox txtLiteralEdit;
//		private System.Windows.Forms.Label lblFilterName;
//		private System.Windows.Forms.Button btnDataChainDetail;
//		private System.Windows.Forms.Button btnDataStoreDetail;
//		private System.Windows.Forms.Button btnDataStoreTotal;
//		private System.Windows.Forms.Button btnDataStoreAverage;
//		private System.Windows.Forms.Button btnSaveAs;
//		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboFilterName;
//		private System.Windows.Forms.Button btnDataDate;
//		private System.Windows.Forms.Button btnDataCorresponding;
//		private System.Windows.Forms.Label label2;
//		private System.Windows.Forms.Label label4;
//		private System.Windows.Forms.GroupBox grpPlan;
//		private System.Windows.Forms.Label label6;
//		private System.Windows.Forms.GroupBox grpOperands;
//		private System.Windows.Forms.GroupBox grpQualifiers;
//		private System.Windows.Forms.Label label5;
//		private System.Windows.Forms.Label label8;
//		private System.Windows.Forms.Button btnDataLiteral;
//		private System.Windows.Forms.TextBox txtGradeEdit;
//		private System.Windows.Forms.Button btnDataGrade;
//		private System.Windows.Forms.Label lblStatus;
//		private System.Windows.Forms.ListBox lstStatus;
//		/// <summary>
//		/// Required designer variable.
//		/// </summary>
//		private System.ComponentModel.Container components = null;
//
//		public frmFilter(SessionAddressBlock aSAB)
//			: base(aSAB)
//		{
//			//
//			// Required for Windows Form Designer support
//			//
//			InitializeComponent();
//
//			_SAB = aSAB;
//		}
//
//		/// <summary>
//		/// Clean up any resources being used.
//		/// </summary>
//		protected override void Dispose( bool disposing )
//		{
//			if( disposing )
//			{
//				if(components != null)
//				{
//					components.Dispose();
//				}
//
//				this.tabFilter.SizeChanged -= new System.EventHandler(this.tabFilter_SizeChanged);
//				this.btnAttrOr.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnAttrOr.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnAttrOr.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnAttrOr.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnAttrOr.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnAttrAnd.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnAttrAnd.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnAttrAnd.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnAttrAnd.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnAttrAnd.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnAttrRParen.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnAttrRParen.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnAttrRParen.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnAttrRParen.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnAttrRParen.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnAttrLParen.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnAttrLParen.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnAttrLParen.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnAttrLParen.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnAttrLParen.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.pnlAttrQuery.Click -= new System.EventHandler(this.panel_Click);
//				this.pnlAttrQuery.DragEnter -= new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
//				this.pnlAttrQuery.DragLeave -= new System.EventHandler(this.panel_DragLeave);
//				this.pnlAttrQuery.DragDrop -= new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
//				this.tpgData.MouseEnter -= new System.EventHandler(this.tpgData_MouseEnter);
//				this.btnDataLiteral.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataLiteral.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataLiteral.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataLiteral.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataLiteral.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataGrade.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataGrade.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataGrade.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataGrade.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataGrade.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataPctChange.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataPctChange.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataPctChange.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataPctChange.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataPctChange.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataPctOf.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataPctOf.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataPctOf.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataPctOf.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataPctOf.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataLess.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataLess.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataLess.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataLess.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataLess.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataAnd.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataAnd.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataAnd.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataAnd.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataAnd.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataGreaterEqual.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataGreaterEqual.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataGreaterEqual.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataGreaterEqual.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataGreaterEqual.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataGreater.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataGreater.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataGreater.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataGreater.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataGreater.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataRParen.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataRParen.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataRParen.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataRParen.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataRParen.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataLParen.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataLParen.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataLParen.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataLParen.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataLParen.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataNot.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataNot.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataNot.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataNot.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataNot.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataOr.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.toolButton_KeyPress);
//				this.btnDataOr.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataOr.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataOr.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataOr.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataOr.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataLessEqual.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataLessEqual.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataLessEqual.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataLessEqual.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataLessEqual.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataEqual.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataEqual.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataEqual.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataEqual.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataEqual.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.lblVariable.Click -= new System.EventHandler(this.toolLabel_Click);
//				this.lblVariable.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseUp);
//				this.lblVariable.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseMove);
//				this.lblVariable.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseDown);
//				this.lblVersion.Click -= new System.EventHandler(this.toolLabel_Click);
//				this.lblVersion.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseUp);
//				this.lblVersion.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseMove);
//				this.lblVersion.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseDown);
//				this.lblStatus.Click -= new System.EventHandler(this.toolLabel_Click);
//				this.lblStatus.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseUp);
//				this.lblStatus.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseMove);
//				this.lblStatus.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseDown);
//				this.btnDataStoreAverage.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataStoreAverage.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataStoreAverage.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataStoreAverage.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataStoreAverage.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataStoreTotal.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataStoreTotal.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataStoreTotal.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataStoreTotal.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataStoreTotal.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataChainDetail.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataChainDetail.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataChainDetail.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataChainDetail.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataChainDetail.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataStoreDetail.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataStoreDetail.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataStoreDetail.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataStoreDetail.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataStoreDetail.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataDate.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataDate.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataDate.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataDate.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataDate.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataAny.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataAny.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataAny.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataAny.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataAny.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataTotal.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataTotal.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataTotal.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataTotal.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataTotal.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataCorresponding.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataCorresponding.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataCorresponding.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataCorresponding.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataCorresponding.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataAverage.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataAverage.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataAverage.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataAverage.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataAverage.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.btnDataAll.Click -= new System.EventHandler(this.toolButton_Click);
//				this.btnDataAll.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//				this.btnDataAll.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//				this.btnDataAll.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//				this.btnDataAll.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//				this.txtLiteralEdit.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtLiteralEdit_KeyPress);
//				this.txtLiteralEdit.Leave -= new System.EventHandler(this.txtLiteralEdit_Leave);
//				this.txtGradeEdit.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtGradeEdit_KeyPress);
//				this.txtGradeEdit.Leave -= new System.EventHandler(this.txtGradeEdit_Leave);
//				this.pnlDataQuery.Click -= new System.EventHandler(this.panel_Click);
//				this.pnlDataQuery.DragEnter -= new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
//				this.pnlDataQuery.DragLeave -= new System.EventHandler(this.panel_DragLeave);
//				this.pnlDataQuery.MouseEnter -= new System.EventHandler(this.pnlDataQuery_MouseEnter);
//				this.pnlDataQuery.DragDrop -= new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
//				this.lstVersions.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseDown);
//				this.lstVersions.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseUp);
//				this.lstVersions.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseMove);
//				this.lstVariables.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseDown);
//				this.lstVariables.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseUp);
//				this.lstVariables.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseMove);
//				this.lstStatus.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseDown);
//				this.lstStatus.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseUp);
//				this.lstStatus.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseMove);
//				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
//				//			this.btnDelete.Click -= new System.EventHandler(this.btnDelete_Click);
//				this.btnNew.Click -= new System.EventHandler(this.btnNew_Click);
//				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
//				this.mnuLabelMenu.Popup -= new System.EventHandler(this.mnuLabelMenu_Popup);
//				this.mniDelete.Click -= new System.EventHandler(this.mniDelete_Click);
//				this.btnSaveAs.Click -= new System.EventHandler(this.btnSaveAs_Click);
//				this.cboFilterName.SelectionChangeCommitted -= new System.EventHandler(this.cboFilterName_SelectionChangeCommitted);
//
//				if(pnlAttrQuery.Tag != null) 
//				{
//					foreach (QueryOperand operand in ((PanelTag)pnlAttrQuery.Tag).OperandArray) 
//					{
//						operand.Label.DragEnter -= new DragEventHandler(((BasicQueryLabel)operand.Label).DragEnterHandler);
//						operand.Label.DragDrop -= new DragEventHandler(((BasicQueryLabel)operand.Label).DragDropHandler);
//						operand.Label.DragLeave -= new EventHandler(((BasicQueryLabel)operand.Label).DragLeaveHandler);
//						operand.Label.MouseDown -= new MouseEventHandler(((BasicQueryLabel)operand.Label).MouseDownHandler);
//						operand.Label.MouseMove -= new MouseEventHandler(((BasicQueryLabel)operand.Label).MouseMoveHandler);
//						operand.Label.MouseUp -= new MouseEventHandler(((BasicQueryLabel)operand.Label).MouseUpHandler);
//						operand.Label.DoubleClick -= new EventHandler(((BasicQueryLabel)operand.Label).DoubleClickHandler);
//						operand.Label.GiveFeedback -= new GiveFeedbackEventHandler(((BasicQueryLabel)operand.Label).GiveFeedbackHandler);
//					}
//				}
//
//				if(pnlDataQuery.Tag != null) 
//				{
//					foreach (QueryOperand operand in ((PanelTag)pnlDataQuery.Tag).OperandArray) 
//					{
//						operand.Label.DragEnter -= new DragEventHandler(((BasicQueryLabel)operand.Label).DragEnterHandler);
//						operand.Label.DragDrop -= new DragEventHandler(((BasicQueryLabel)operand.Label).DragDropHandler);
//						operand.Label.DragLeave -= new EventHandler(((BasicQueryLabel)operand.Label).DragLeaveHandler);
//						operand.Label.MouseDown -= new MouseEventHandler(((BasicQueryLabel)operand.Label).MouseDownHandler);
//						operand.Label.MouseMove -= new MouseEventHandler(((BasicQueryLabel)operand.Label).MouseMoveHandler);
//						operand.Label.MouseUp -= new MouseEventHandler(((BasicQueryLabel)operand.Label).MouseUpHandler);
//						operand.Label.DoubleClick -= new EventHandler(((BasicQueryLabel)operand.Label).DoubleClickHandler);
//						operand.Label.GiveFeedback -= new GiveFeedbackEventHandler(((BasicQueryLabel)operand.Label).GiveFeedbackHandler);
//					}
//				}
//			}
//			base.Dispose( disposing );
//		}
//
//		/// <summary>
//		/// Required method for Designer support - do not modify
//		/// the contents of this method with the code editor.
//		/// </summary>
//		private void InitializeComponent()
//		{
//			this.tabFilter = new System.Windows.Forms.TabControl();
//			this.tpgAttributes = new System.Windows.Forms.TabPage();
//			this.btnAttrOr = new System.Windows.Forms.Button();
//			this.btnAttrAnd = new System.Windows.Forms.Button();
//			this.btnAttrRParen = new System.Windows.Forms.Button();
//			this.btnAttrLParen = new System.Windows.Forms.Button();
//			this.pnlAttrQuery = new System.Windows.Forms.Panel();
//			this.tpgData = new System.Windows.Forms.TabPage();
//			this.txtGradeEdit = new System.Windows.Forms.TextBox();
//			this.label5 = new System.Windows.Forms.Label();
//			this.grpQualifiers = new System.Windows.Forms.GroupBox();
//			this.btnDataGrade = new System.Windows.Forms.Button();
//			this.label8 = new System.Windows.Forms.Label();
//			this.lblStatus = new System.Windows.Forms.Label();
//			this.btnDataLiteral = new System.Windows.Forms.Button();
//			this.btnDataPctChange = new System.Windows.Forms.Button();
//			this.btnDataPctOf = new System.Windows.Forms.Button();
//			this.grpOperands = new System.Windows.Forms.GroupBox();
//			this.btnDataLess = new System.Windows.Forms.Button();
//			this.btnDataAnd = new System.Windows.Forms.Button();
//			this.btnDataGreaterEqual = new System.Windows.Forms.Button();
//			this.btnDataGreater = new System.Windows.Forms.Button();
//			this.btnDataRParen = new System.Windows.Forms.Button();
//			this.btnDataLParen = new System.Windows.Forms.Button();
//			this.btnDataNot = new System.Windows.Forms.Button();
//			this.btnDataOr = new System.Windows.Forms.Button();
//			this.btnDataLessEqual = new System.Windows.Forms.Button();
//			this.btnDataEqual = new System.Windows.Forms.Button();
//			this.grpPlan = new System.Windows.Forms.GroupBox();
//			this.label6 = new System.Windows.Forms.Label();
//			this.lblVariable = new System.Windows.Forms.Label();
//			this.lblVersion = new System.Windows.Forms.Label();
//			this.label1 = new System.Windows.Forms.Label();
//			this.label3 = new System.Windows.Forms.Label();
//			this.btnDataStoreAverage = new System.Windows.Forms.Button();
//			this.label2 = new System.Windows.Forms.Label();
//			this.btnDataStoreTotal = new System.Windows.Forms.Button();
//			this.btnDataChainDetail = new System.Windows.Forms.Button();
//			this.btnDataStoreDetail = new System.Windows.Forms.Button();
//			this.btnDataDate = new System.Windows.Forms.Button();
//			this.btnDataAny = new System.Windows.Forms.Button();
//			this.btnDataTotal = new System.Windows.Forms.Button();
//			this.btnDataCorresponding = new System.Windows.Forms.Button();
//			this.label4 = new System.Windows.Forms.Label();
//			this.btnDataAverage = new System.Windows.Forms.Button();
//			this.btnDataAll = new System.Windows.Forms.Button();
//			this.txtLiteralEdit = new System.Windows.Forms.TextBox();
//			this.pnlDataQuery = new System.Windows.Forms.Panel();
//			this.lstVersions = new System.Windows.Forms.ListBox();
//			this.lstVariables = new System.Windows.Forms.ListBox();
//			this.lstStatus = new System.Windows.Forms.ListBox();
//			this.btnSave = new System.Windows.Forms.Button();
//			this.btnNew = new System.Windows.Forms.Button();
//			this.lblFilterName = new System.Windows.Forms.Label();
//			this.btnHelp = new System.Windows.Forms.Button();
//			this.btnCancel = new System.Windows.Forms.Button();
//			this.mnuLabelMenu = new System.Windows.Forms.ContextMenu();
//			this.mniDelete = new System.Windows.Forms.MenuItem();
//			this.btnSaveAs = new System.Windows.Forms.Button();
//			this.cboFilterName = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
//			this.tabFilter.SuspendLayout();
//			this.tpgAttributes.SuspendLayout();
//			this.tpgData.SuspendLayout();
//			this.grpQualifiers.SuspendLayout();
//			this.grpOperands.SuspendLayout();
//			this.grpPlan.SuspendLayout();
//			this.SuspendLayout();
//			// 
//			// tabFilter
//			// 
//			this.tabFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
//				| System.Windows.Forms.AnchorStyles.Left) 
//				| System.Windows.Forms.AnchorStyles.Right)));
//			this.tabFilter.Controls.Add(this.tpgAttributes);
//			this.tabFilter.Controls.Add(this.tpgData);
//			this.tabFilter.Location = new System.Drawing.Point(16, 56);
//			this.tabFilter.Name = "tabFilter";
//			this.tabFilter.SelectedIndex = 0;
//			this.tabFilter.Size = new System.Drawing.Size(712, 584);
//			this.tabFilter.TabIndex = 7;
//			this.tabFilter.SizeChanged += new System.EventHandler(this.tabFilter_SizeChanged);
//			// 
//			// tpgAttributes
//			// 
//			this.tpgAttributes.Controls.Add(this.btnAttrOr);
//			this.tpgAttributes.Controls.Add(this.btnAttrAnd);
//			this.tpgAttributes.Controls.Add(this.btnAttrRParen);
//			this.tpgAttributes.Controls.Add(this.btnAttrLParen);
//			this.tpgAttributes.Controls.Add(this.pnlAttrQuery);
//			this.tpgAttributes.Location = new System.Drawing.Point(4, 22);
//			this.tpgAttributes.Name = "tpgAttributes";
//			this.tpgAttributes.Size = new System.Drawing.Size(704, 558);
//			this.tpgAttributes.TabIndex = 0;
//			this.tpgAttributes.Text = "Attributes";
//			// 
//			// btnAttrOr
//			// 
//			this.btnAttrOr.Location = new System.Drawing.Point(16, 16);
//			this.btnAttrOr.Name = "btnAttrOr";
//			this.btnAttrOr.Size = new System.Drawing.Size(40, 24);
//			this.btnAttrOr.TabIndex = 8;
//			this.btnAttrOr.Text = "OR";
//			this.btnAttrOr.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnAttrOr.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnAttrOr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnAttrOr.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnAttrOr.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnAttrAnd
//			// 
//			this.btnAttrAnd.Location = new System.Drawing.Point(56, 16);
//			this.btnAttrAnd.Name = "btnAttrAnd";
//			this.btnAttrAnd.Size = new System.Drawing.Size(40, 24);
//			this.btnAttrAnd.TabIndex = 9;
//			this.btnAttrAnd.Text = "AND";
//			this.btnAttrAnd.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnAttrAnd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnAttrAnd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnAttrAnd.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnAttrAnd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnAttrRParen
//			// 
//			this.btnAttrRParen.Location = new System.Drawing.Point(136, 16);
//			this.btnAttrRParen.Name = "btnAttrRParen";
//			this.btnAttrRParen.Size = new System.Drawing.Size(40, 24);
//			this.btnAttrRParen.TabIndex = 11;
//			this.btnAttrRParen.Text = ")";
//			this.btnAttrRParen.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnAttrRParen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnAttrRParen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnAttrRParen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnAttrRParen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnAttrLParen
//			// 
//			this.btnAttrLParen.Location = new System.Drawing.Point(96, 16);
//			this.btnAttrLParen.Name = "btnAttrLParen";
//			this.btnAttrLParen.Size = new System.Drawing.Size(40, 24);
//			this.btnAttrLParen.TabIndex = 10;
//			this.btnAttrLParen.Text = "(";
//			this.btnAttrLParen.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnAttrLParen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnAttrLParen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnAttrLParen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnAttrLParen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// pnlAttrQuery
//			// 
//			this.pnlAttrQuery.AllowDrop = true;
//			this.pnlAttrQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
//				| System.Windows.Forms.AnchorStyles.Left) 
//				| System.Windows.Forms.AnchorStyles.Right)));
//			this.pnlAttrQuery.AutoScroll = true;
//			this.pnlAttrQuery.BackColor = System.Drawing.SystemColors.ControlLightLight;
//			this.pnlAttrQuery.Location = new System.Drawing.Point(16, 48);
//			this.pnlAttrQuery.Name = "pnlAttrQuery";
//			this.pnlAttrQuery.Size = new System.Drawing.Size(672, 496);
//			this.pnlAttrQuery.TabIndex = 0;
//			this.pnlAttrQuery.Click += new System.EventHandler(this.panel_Click);
//			this.pnlAttrQuery.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
//			this.pnlAttrQuery.DragLeave += new System.EventHandler(this.panel_DragLeave);
//			this.pnlAttrQuery.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
//			// 
//			// tpgData
//			// 
//			this.tpgData.Controls.Add(this.txtGradeEdit);
//			this.tpgData.Controls.Add(this.label5);
//			this.tpgData.Controls.Add(this.grpQualifiers);
//			this.tpgData.Controls.Add(this.grpOperands);
//			this.tpgData.Controls.Add(this.grpPlan);
//			this.tpgData.Controls.Add(this.txtLiteralEdit);
//			this.tpgData.Controls.Add(this.pnlDataQuery);
//			this.tpgData.Controls.Add(this.lstVersions);
//			this.tpgData.Controls.Add(this.lstVariables);
//			this.tpgData.Controls.Add(this.lstStatus);
//			this.tpgData.Location = new System.Drawing.Point(4, 22);
//			this.tpgData.Name = "tpgData";
//			this.tpgData.Size = new System.Drawing.Size(704, 558);
//			this.tpgData.TabIndex = 1;
//			this.tpgData.Text = "Data";
//			this.tpgData.MouseEnter += new System.EventHandler(this.tpgData_MouseEnter);
//			// 
//			// txtGradeEdit
//			// 
//			this.txtGradeEdit.AcceptsReturn = true;
//			this.txtGradeEdit.AcceptsTab = true;
//			this.txtGradeEdit.BackColor = System.Drawing.SystemColors.Window;
//			this.txtGradeEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//			this.txtGradeEdit.Enabled = false;
//			this.txtGradeEdit.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
//			this.txtGradeEdit.Location = new System.Drawing.Point(624, 184);
//			this.txtGradeEdit.Multiline = true;
//			this.txtGradeEdit.Name = "txtGradeEdit";
//			this.txtGradeEdit.Size = new System.Drawing.Size(64, 20);
//			this.txtGradeEdit.TabIndex = 54;
//			this.txtGradeEdit.TabStop = false;
//			this.txtGradeEdit.Text = "";
//			this.txtGradeEdit.Visible = false;
//			this.txtGradeEdit.WordWrap = false;
//			this.txtGradeEdit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGradeEdit_KeyPress);
//			this.txtGradeEdit.Leave += new System.EventHandler(this.txtGradeEdit_Leave);
//			// 
//			// label5
//			// 
//			this.label5.Location = new System.Drawing.Point(16, 264);
//			this.label5.Name = "label5";
//			this.label5.Size = new System.Drawing.Size(72, 16);
//			this.label5.TabIndex = 51;
//			this.label5.Text = "Filter Builder";
//			// 
//			// grpQualifiers
//			// 
//			this.grpQualifiers.Controls.Add(this.btnDataGrade);
//			this.grpQualifiers.Controls.Add(this.label8);
//			this.grpQualifiers.Controls.Add(this.lblStatus);
//			this.grpQualifiers.Controls.Add(this.btnDataLiteral);
//			this.grpQualifiers.Controls.Add(this.btnDataPctChange);
//			this.grpQualifiers.Controls.Add(this.btnDataPctOf);
//			this.grpQualifiers.Location = new System.Drawing.Point(344, 160);
//			this.grpQualifiers.Name = "grpQualifiers";
//			this.grpQualifiers.Size = new System.Drawing.Size(264, 104);
//			this.grpQualifiers.TabIndex = 50;
//			this.grpQualifiers.TabStop = false;
//			this.grpQualifiers.Text = "Qualifiers";
//			// 
//			// btnDataGrade
//			// 
//			this.btnDataGrade.Location = new System.Drawing.Point(96, 64);
//			this.btnDataGrade.Name = "btnDataGrade";
//			this.btnDataGrade.Size = new System.Drawing.Size(72, 24);
//			this.btnDataGrade.TabIndex = 40;
//			this.btnDataGrade.Text = "Grade";
//			this.btnDataGrade.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataGrade.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataGrade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataGrade.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataGrade.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// label8
//			// 
//			this.label8.AutoSize = true;
//			this.label8.Location = new System.Drawing.Point(184, 48);
//			this.label8.Name = "label8";
//			this.label8.Size = new System.Drawing.Size(39, 16);
//			this.label8.TabIndex = 39;
//			this.label8.Text = "Status:";
//			// 
//			// lblStatus
//			// 
//			this.lblStatus.BackColor = System.Drawing.SystemColors.ControlLightLight;
//			this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//			this.lblStatus.Location = new System.Drawing.Point(184, 66);
//			this.lblStatus.Name = "lblStatus";
//			this.lblStatus.Size = new System.Drawing.Size(64, 20);
//			this.lblStatus.TabIndex = 37;
//			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//			this.lblStatus.Click += new System.EventHandler(this.toolLabel_Click);
//			this.lblStatus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseUp);
//			this.lblStatus.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseMove);
//			this.lblStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseDown);
//			// 
//			// btnDataLiteral
//			// 
//			this.btnDataLiteral.Location = new System.Drawing.Point(16, 64);
//			this.btnDataLiteral.Name = "btnDataLiteral";
//			this.btnDataLiteral.Size = new System.Drawing.Size(72, 24);
//			this.btnDataLiteral.TabIndex = 24;
//			this.btnDataLiteral.Text = "Quantity";
//			this.btnDataLiteral.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataLiteral.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataLiteral.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataLiteral.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataLiteral.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataPctChange
//			// 
//			this.btnDataPctChange.Location = new System.Drawing.Point(96, 24);
//			this.btnDataPctChange.Name = "btnDataPctChange";
//			this.btnDataPctChange.Size = new System.Drawing.Size(72, 24);
//			this.btnDataPctChange.TabIndex = 23;
//			this.btnDataPctChange.Text = "% Change";
//			this.btnDataPctChange.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataPctChange.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataPctChange.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataPctChange.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataPctChange.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataPctOf
//			// 
//			this.btnDataPctOf.Location = new System.Drawing.Point(16, 24);
//			this.btnDataPctOf.Name = "btnDataPctOf";
//			this.btnDataPctOf.Size = new System.Drawing.Size(72, 24);
//			this.btnDataPctOf.TabIndex = 22;
//			this.btnDataPctOf.Text = "% Of";
//			this.btnDataPctOf.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataPctOf.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataPctOf.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataPctOf.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataPctOf.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// grpOperands
//			// 
//			this.grpOperands.Controls.Add(this.btnDataLess);
//			this.grpOperands.Controls.Add(this.btnDataAnd);
//			this.grpOperands.Controls.Add(this.btnDataGreaterEqual);
//			this.grpOperands.Controls.Add(this.btnDataGreater);
//			this.grpOperands.Controls.Add(this.btnDataRParen);
//			this.grpOperands.Controls.Add(this.btnDataLParen);
//			this.grpOperands.Controls.Add(this.btnDataNot);
//			this.grpOperands.Controls.Add(this.btnDataOr);
//			this.grpOperands.Controls.Add(this.btnDataLessEqual);
//			this.grpOperands.Controls.Add(this.btnDataEqual);
//			this.grpOperands.Location = new System.Drawing.Point(16, 160);
//			this.grpOperands.Name = "grpOperands";
//			this.grpOperands.Size = new System.Drawing.Size(312, 104);
//			this.grpOperands.TabIndex = 49;
//			this.grpOperands.TabStop = false;
//			this.grpOperands.Text = "Operands";
//			// 
//			// btnDataLess
//			// 
//			this.btnDataLess.Location = new System.Drawing.Point(136, 24);
//			this.btnDataLess.Name = "btnDataLess";
//			this.btnDataLess.Size = new System.Drawing.Size(40, 24);
//			this.btnDataLess.TabIndex = 17;
//			this.btnDataLess.Text = "<";
//			this.btnDataLess.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataLess.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataLess.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataLess.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataLess.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataAnd
//			// 
//			this.btnDataAnd.Location = new System.Drawing.Point(136, 64);
//			this.btnDataAnd.Name = "btnDataAnd";
//			this.btnDataAnd.Size = new System.Drawing.Size(40, 24);
//			this.btnDataAnd.TabIndex = 13;
//			this.btnDataAnd.Text = "AND";
//			this.btnDataAnd.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataAnd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataAnd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataAnd.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataAnd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataGreaterEqual
//			// 
//			this.btnDataGreaterEqual.Location = new System.Drawing.Point(256, 24);
//			this.btnDataGreaterEqual.Name = "btnDataGreaterEqual";
//			this.btnDataGreaterEqual.Size = new System.Drawing.Size(40, 24);
//			this.btnDataGreaterEqual.TabIndex = 20;
//			this.btnDataGreaterEqual.Text = ">=";
//			this.btnDataGreaterEqual.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataGreaterEqual.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataGreaterEqual.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataGreaterEqual.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataGreaterEqual.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataGreater
//			// 
//			this.btnDataGreater.Location = new System.Drawing.Point(176, 24);
//			this.btnDataGreater.Name = "btnDataGreater";
//			this.btnDataGreater.Size = new System.Drawing.Size(40, 24);
//			this.btnDataGreater.TabIndex = 18;
//			this.btnDataGreater.Text = ">";
//			this.btnDataGreater.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataGreater.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataGreater.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataGreater.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataGreater.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataRParen
//			// 
//			this.btnDataRParen.Location = new System.Drawing.Point(56, 24);
//			this.btnDataRParen.Name = "btnDataRParen";
//			this.btnDataRParen.Size = new System.Drawing.Size(40, 24);
//			this.btnDataRParen.TabIndex = 15;
//			this.btnDataRParen.Text = ")";
//			this.btnDataRParen.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataRParen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataRParen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataRParen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataRParen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataLParen
//			// 
//			this.btnDataLParen.Location = new System.Drawing.Point(16, 24);
//			this.btnDataLParen.Name = "btnDataLParen";
//			this.btnDataLParen.Size = new System.Drawing.Size(40, 24);
//			this.btnDataLParen.TabIndex = 14;
//			this.btnDataLParen.Text = "(";
//			this.btnDataLParen.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataLParen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataLParen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataLParen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataLParen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataNot
//			// 
//			this.btnDataNot.Location = new System.Drawing.Point(176, 64);
//			this.btnDataNot.Name = "btnDataNot";
//			this.btnDataNot.Size = new System.Drawing.Size(40, 24);
//			this.btnDataNot.TabIndex = 21;
//			this.btnDataNot.Text = "NOT";
//			this.btnDataNot.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataNot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataNot.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataNot.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataNot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataOr
//			// 
//			this.btnDataOr.Location = new System.Drawing.Point(96, 64);
//			this.btnDataOr.Name = "btnDataOr";
//			this.btnDataOr.Size = new System.Drawing.Size(40, 24);
//			this.btnDataOr.TabIndex = 12;
//			this.btnDataOr.Text = "OR";
//			this.btnDataOr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolButton_KeyPress);
//			this.btnDataOr.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataOr.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataOr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataOr.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataOr.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataLessEqual
//			// 
//			this.btnDataLessEqual.Location = new System.Drawing.Point(216, 24);
//			this.btnDataLessEqual.Name = "btnDataLessEqual";
//			this.btnDataLessEqual.Size = new System.Drawing.Size(40, 24);
//			this.btnDataLessEqual.TabIndex = 19;
//			this.btnDataLessEqual.Text = "<=";
//			this.btnDataLessEqual.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataLessEqual.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataLessEqual.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataLessEqual.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataLessEqual.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataEqual
//			// 
//			this.btnDataEqual.Location = new System.Drawing.Point(96, 24);
//			this.btnDataEqual.Name = "btnDataEqual";
//			this.btnDataEqual.Size = new System.Drawing.Size(40, 24);
//			this.btnDataEqual.TabIndex = 16;
//			this.btnDataEqual.Text = "=";
//			this.btnDataEqual.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataEqual.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataEqual.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataEqual.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataEqual.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// grpPlan
//			// 
//			this.grpPlan.Controls.Add(this.label6);
//			this.grpPlan.Controls.Add(this.lblVariable);
//			this.grpPlan.Controls.Add(this.lblVersion);
//			this.grpPlan.Controls.Add(this.label1);
//			this.grpPlan.Controls.Add(this.label3);
//			this.grpPlan.Controls.Add(this.btnDataStoreAverage);
//			this.grpPlan.Controls.Add(this.label2);
//			this.grpPlan.Controls.Add(this.btnDataStoreTotal);
//			this.grpPlan.Controls.Add(this.btnDataChainDetail);
//			this.grpPlan.Controls.Add(this.btnDataStoreDetail);
//			this.grpPlan.Controls.Add(this.btnDataDate);
//			this.grpPlan.Controls.Add(this.btnDataAny);
//			this.grpPlan.Controls.Add(this.btnDataTotal);
//			this.grpPlan.Controls.Add(this.btnDataCorresponding);
//			this.grpPlan.Controls.Add(this.label4);
//			this.grpPlan.Controls.Add(this.btnDataAverage);
//			this.grpPlan.Controls.Add(this.btnDataAll);
//			this.grpPlan.Location = new System.Drawing.Point(16, 8);
//			this.grpPlan.Name = "grpPlan";
//			this.grpPlan.Size = new System.Drawing.Size(672, 144);
//			this.grpPlan.TabIndex = 48;
//			this.grpPlan.TabStop = false;
//			this.grpPlan.Text = "Select and drop into Filter Builder below:";
//			// 
//			// label6
//			// 
//			this.label6.Location = new System.Drawing.Point(368, 68);
//			this.label6.Name = "label6";
//			this.label6.Size = new System.Drawing.Size(296, 16);
//			this.label6.TabIndex = 47;
//			this.label6.Text = "(Merchandise must be dropped from Hierarchy Explorer)";
//			// 
//			// lblVariable
//			// 
//			this.lblVariable.BackColor = System.Drawing.SystemColors.ControlLightLight;
//			this.lblVariable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//			this.lblVariable.Location = new System.Drawing.Point(64, 26);
//			this.lblVariable.Name = "lblVariable";
//			this.lblVariable.Size = new System.Drawing.Size(152, 20);
//			this.lblVariable.TabIndex = 35;
//			this.lblVariable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//			this.lblVariable.Click += new System.EventHandler(this.toolLabel_Click);
//			this.lblVariable.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseUp);
//			this.lblVariable.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseMove);
//			this.lblVariable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseDown);
//			// 
//			// lblVersion
//			// 
//			this.lblVersion.BackColor = System.Drawing.SystemColors.ControlLightLight;
//			this.lblVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//			this.lblVersion.Location = new System.Drawing.Point(64, 66);
//			this.lblVersion.Name = "lblVersion";
//			this.lblVersion.Size = new System.Drawing.Size(152, 20);
//			this.lblVersion.TabIndex = 36;
//			this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//			this.lblVersion.Click += new System.EventHandler(this.toolLabel_Click);
//			this.lblVersion.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseUp);
//			this.lblVersion.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseMove);
//			this.lblVersion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseDown);
//			// 
//			// label1
//			// 
//			this.label1.AutoSize = true;
//			this.label1.Location = new System.Drawing.Point(16, 28);
//			this.label1.Name = "label1";
//			this.label1.Size = new System.Drawing.Size(49, 16);
//			this.label1.TabIndex = 27;
//			this.label1.Text = "Variable:";
//			// 
//			// label3
//			// 
//			this.label3.AutoSize = true;
//			this.label3.Location = new System.Drawing.Point(16, 68);
//			this.label3.Name = "label3";
//			this.label3.Size = new System.Drawing.Size(46, 16);
//			this.label3.TabIndex = 29;
//			this.label3.Text = "Version:";
//			// 
//			// btnDataStoreAverage
//			// 
//			this.btnDataStoreAverage.Location = new System.Drawing.Point(480, 24);
//			this.btnDataStoreAverage.Name = "btnDataStoreAverage";
//			this.btnDataStoreAverage.Size = new System.Drawing.Size(88, 24);
//			this.btnDataStoreAverage.TabIndex = 27;
//			this.btnDataStoreAverage.Text = "Store Average";
//			this.btnDataStoreAverage.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataStoreAverage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataStoreAverage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataStoreAverage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataStoreAverage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// label2
//			// 
//			this.label2.AutoSize = true;
//			this.label2.Location = new System.Drawing.Point(232, 28);
//			this.label2.Name = "label2";
//			this.label2.Size = new System.Drawing.Size(65, 16);
//			this.label2.TabIndex = 45;
//			this.label2.Text = "Value Type:";
//			// 
//			// btnDataStoreTotal
//			// 
//			this.btnDataStoreTotal.Location = new System.Drawing.Point(392, 24);
//			this.btnDataStoreTotal.Name = "btnDataStoreTotal";
//			this.btnDataStoreTotal.Size = new System.Drawing.Size(88, 24);
//			this.btnDataStoreTotal.TabIndex = 26;
//			this.btnDataStoreTotal.Text = "Store Total";
//			this.btnDataStoreTotal.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataStoreTotal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataStoreTotal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataStoreTotal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataStoreTotal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataChainDetail
//			// 
//			this.btnDataChainDetail.Location = new System.Drawing.Point(568, 24);
//			this.btnDataChainDetail.Name = "btnDataChainDetail";
//			this.btnDataChainDetail.Size = new System.Drawing.Size(88, 24);
//			this.btnDataChainDetail.TabIndex = 28;
//			this.btnDataChainDetail.Text = "Chain Detail";
//			this.btnDataChainDetail.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataChainDetail.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataChainDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataChainDetail.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataChainDetail.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataStoreDetail
//			// 
//			this.btnDataStoreDetail.BackColor = System.Drawing.SystemColors.Control;
//			this.btnDataStoreDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
//			this.btnDataStoreDetail.Location = new System.Drawing.Point(304, 24);
//			this.btnDataStoreDetail.Name = "btnDataStoreDetail";
//			this.btnDataStoreDetail.Size = new System.Drawing.Size(88, 24);
//			this.btnDataStoreDetail.TabIndex = 25;
//			this.btnDataStoreDetail.Text = "Store Detail";
//			this.btnDataStoreDetail.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataStoreDetail.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataStoreDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataStoreDetail.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataStoreDetail.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataDate
//			// 
//			this.btnDataDate.Location = new System.Drawing.Point(232, 64);
//			this.btnDataDate.Name = "btnDataDate";
//			this.btnDataDate.Size = new System.Drawing.Size(80, 24);
//			this.btnDataDate.TabIndex = 34;
//			this.btnDataDate.Text = "DateRange";
//			this.btnDataDate.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataDate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataDate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataAny
//			// 
//			this.btnDataAny.BackColor = System.Drawing.SystemColors.Control;
//			this.btnDataAny.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
//			this.btnDataAny.Location = new System.Drawing.Point(112, 104);
//			this.btnDataAny.Name = "btnDataAny";
//			this.btnDataAny.Size = new System.Drawing.Size(40, 24);
//			this.btnDataAny.TabIndex = 29;
//			this.btnDataAny.Text = "Any";
//			this.btnDataAny.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataAny.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataAny.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataAny.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataAny.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataTotal
//			// 
//			this.btnDataTotal.Location = new System.Drawing.Point(232, 104);
//			this.btnDataTotal.Name = "btnDataTotal";
//			this.btnDataTotal.Size = new System.Drawing.Size(40, 24);
//			this.btnDataTotal.TabIndex = 32;
//			this.btnDataTotal.Text = "Tot";
//			this.btnDataTotal.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataTotal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataTotal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataTotal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataTotal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataCorresponding
//			// 
//			this.btnDataCorresponding.Location = new System.Drawing.Point(272, 104);
//			this.btnDataCorresponding.Name = "btnDataCorresponding";
//			this.btnDataCorresponding.Size = new System.Drawing.Size(40, 24);
//			this.btnDataCorresponding.TabIndex = 33;
//			this.btnDataCorresponding.Text = "Corr";
//			this.btnDataCorresponding.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataCorresponding.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataCorresponding.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataCorresponding.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataCorresponding.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// label4
//			// 
//			this.label4.AutoSize = true;
//			this.label4.Location = new System.Drawing.Point(16, 108);
//			this.label4.Name = "label4";
//			this.label4.Size = new System.Drawing.Size(97, 16);
//			this.label4.TabIndex = 46;
//			this.label4.Text = "Time Comparison:";
//			// 
//			// btnDataAverage
//			// 
//			this.btnDataAverage.Location = new System.Drawing.Point(192, 104);
//			this.btnDataAverage.Name = "btnDataAverage";
//			this.btnDataAverage.Size = new System.Drawing.Size(40, 24);
//			this.btnDataAverage.TabIndex = 31;
//			this.btnDataAverage.Text = "Avg";
//			this.btnDataAverage.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataAverage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataAverage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataAverage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataAverage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// btnDataAll
//			// 
//			this.btnDataAll.Location = new System.Drawing.Point(152, 104);
//			this.btnDataAll.Name = "btnDataAll";
//			this.btnDataAll.Size = new System.Drawing.Size(40, 24);
//			this.btnDataAll.TabIndex = 30;
//			this.btnDataAll.Text = "All";
//			this.btnDataAll.Click += new System.EventHandler(this.toolButton_Click);
//			this.btnDataAll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//			this.btnDataAll.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//			this.btnDataAll.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//			this.btnDataAll.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//			// 
//			// txtLiteralEdit
//			// 
//			this.txtLiteralEdit.AcceptsReturn = true;
//			this.txtLiteralEdit.AcceptsTab = true;
//			this.txtLiteralEdit.BackColor = System.Drawing.SystemColors.Window;
//			this.txtLiteralEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//			this.txtLiteralEdit.Enabled = false;
//			this.txtLiteralEdit.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
//			this.txtLiteralEdit.Location = new System.Drawing.Point(624, 160);
//			this.txtLiteralEdit.Multiline = true;
//			this.txtLiteralEdit.Name = "txtLiteralEdit";
//			this.txtLiteralEdit.Size = new System.Drawing.Size(64, 20);
//			this.txtLiteralEdit.TabIndex = 40;
//			this.txtLiteralEdit.TabStop = false;
//			this.txtLiteralEdit.Text = "";
//			this.txtLiteralEdit.Visible = false;
//			this.txtLiteralEdit.WordWrap = false;
//			this.txtLiteralEdit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLiteralEdit_KeyPress);
//			this.txtLiteralEdit.Leave += new System.EventHandler(this.txtLiteralEdit_Leave);
//			// 
//			// pnlDataQuery
//			// 
//			this.pnlDataQuery.AllowDrop = true;
//			this.pnlDataQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
//				| System.Windows.Forms.AnchorStyles.Left) 
//				| System.Windows.Forms.AnchorStyles.Right)));
//			this.pnlDataQuery.AutoScroll = true;
//			this.pnlDataQuery.BackColor = System.Drawing.SystemColors.Window;
//			this.pnlDataQuery.Location = new System.Drawing.Point(16, 280);
//			this.pnlDataQuery.Name = "pnlDataQuery";
//			this.pnlDataQuery.Size = new System.Drawing.Size(672, 264);
//			this.pnlDataQuery.TabIndex = 16;
//			this.pnlDataQuery.Click += new System.EventHandler(this.panel_Click);
//			this.pnlDataQuery.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
//			this.pnlDataQuery.DragLeave += new System.EventHandler(this.panel_DragLeave);
//			this.pnlDataQuery.MouseEnter += new System.EventHandler(this.pnlDataQuery_MouseEnter);
//			this.pnlDataQuery.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
//			// 
//			// lstVersions
//			// 
//			this.lstVersions.Location = new System.Drawing.Point(512, 232);
//			this.lstVersions.Name = "lstVersions";
//			this.lstVersions.Size = new System.Drawing.Size(160, 160);
//			this.lstVersions.TabIndex = 30;
//			this.lstVersions.Visible = false;
//			this.lstVersions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseDown);
//			this.lstVersions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseUp);
//			this.lstVersions.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseMove);
//			// 
//			// lstVariables
//			// 
//			this.lstVariables.Location = new System.Drawing.Point(552, 216);
//			this.lstVariables.Name = "lstVariables";
//			this.lstVariables.Size = new System.Drawing.Size(160, 160);
//			this.lstVariables.TabIndex = 25;
//			this.lstVariables.Visible = false;
//			this.lstVariables.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseDown);
//			this.lstVariables.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseUp);
//			this.lstVariables.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseMove);
//			// 
//			// lstStatus
//			// 
//			this.lstStatus.Location = new System.Drawing.Point(576, 200);
//			this.lstStatus.Name = "lstStatus";
//			this.lstStatus.Size = new System.Drawing.Size(160, 69);
//			this.lstStatus.TabIndex = 53;
//			this.lstStatus.Visible = false;
//			this.lstStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseDown);
//			this.lstStatus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseUp);
//			this.lstStatus.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseMove);
//			// 
//			// btnSave
//			// 
//			this.btnSave.Location = new System.Drawing.Point(304, 16);
//			this.btnSave.Name = "btnSave";
//			this.btnSave.TabIndex = 2;
//			this.btnSave.Text = "Save";
//			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
//			// 
//			// btnNew
//			// 
//			this.btnNew.Location = new System.Drawing.Point(464, 16);
//			this.btnNew.Name = "btnNew";
//			this.btnNew.TabIndex = 4;
//			this.btnNew.Text = "New";
//			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
//			// 
//			// lblFilterName
//			// 
//			this.lblFilterName.Location = new System.Drawing.Point(40, 16);
//			this.lblFilterName.Name = "lblFilterName";
//			this.lblFilterName.Size = new System.Drawing.Size(64, 23);
//			this.lblFilterName.TabIndex = 27;
//			this.lblFilterName.Text = "Filter Name";
//			this.lblFilterName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//			// 
//			// btnHelp
//			// 
//			this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
//			this.btnHelp.Location = new System.Drawing.Point(16, 656);
//			this.btnHelp.Name = "btnHelp";
//			this.btnHelp.Size = new System.Drawing.Size(24, 23);
//			this.btnHelp.TabIndex = 12;
//			this.btnHelp.Text = "?";
//			// 
//			// btnCancel
//			// 
//			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//			this.btnCancel.Location = new System.Drawing.Point(656, 656);
//			this.btnCancel.Name = "btnCancel";
//			this.btnCancel.TabIndex = 6;
//			this.btnCancel.Text = "Cancel";
//			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
//			// 
//			// mnuLabelMenu
//			// 
//			this.mnuLabelMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
//																						 this.mniDelete});
//			this.mnuLabelMenu.Popup += new System.EventHandler(this.mnuLabelMenu_Popup);
//			// 
//			// mniDelete
//			// 
//			this.mniDelete.Index = 0;
//			this.mniDelete.Text = "Delete";
//			this.mniDelete.Click += new System.EventHandler(this.mniDelete_Click);
//			// 
//			// btnSaveAs
//			// 
//			this.btnSaveAs.Location = new System.Drawing.Point(384, 16);
//			this.btnSaveAs.Name = "btnSaveAs";
//			this.btnSaveAs.TabIndex = 3;
//			this.btnSaveAs.Text = "Save As";
//			this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
//			// 
//			// cboFilterName
//			// 
//			this.cboFilterName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//			this.cboFilterName.Location = new System.Drawing.Point(112, 16);
//			this.cboFilterName.Name = "cboFilterName";
//			this.cboFilterName.Size = new System.Drawing.Size(184, 21);
//			this.cboFilterName.TabIndex = 1;
//			this.cboFilterName.SelectionChangeCommitted += new System.EventHandler(this.cboFilterName_SelectionChangeCommitted);
//			// 
//			// frmFilter
//			// 
//			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
//			this.ClientSize = new System.Drawing.Size(744, 694);
//			this.Controls.Add(this.cboFilterName);
//			this.Controls.Add(this.btnSaveAs);
//			this.Controls.Add(this.btnHelp);
//			this.Controls.Add(this.btnCancel);
//			this.Controls.Add(this.btnSave);
//			this.Controls.Add(this.btnNew);
//			this.Controls.Add(this.lblFilterName);
//			this.Controls.Add(this.tabFilter);
//			this.FormLoaded = true;
//			this.Name = "frmFilter";
//			this.Text = "Filter";
//			this.tabFilter.ResumeLayout(false);
//			this.tpgAttributes.ResumeLayout(false);
//			this.tpgData.ResumeLayout(false);
//			this.grpQualifiers.ResumeLayout(false);
//			this.grpOperands.ResumeLayout(false);
//			this.grpPlan.ResumeLayout(false);
//			this.ResumeLayout(false);
//
//		}
//		#endregion
//
//		#region Fields
//		private SessionAddressBlock _SAB;
//		private ToolTip _toolTip;
//		private FunctionSecurityProfile _filterUserSecurity;
//		private FunctionSecurityProfile _filterGlobalSecurity;
//		private FilterDefinition _currentFilter;
//		private int _multiSelectStartIdx;
//		private int _multiSelectEndIdx;
//		private int _indexOfItemUnderMouseToDrag;
//		private ProfileList _versionProfList;
//		private ProfileList _variableProfList;
//		private ProfileList _timeTotalVariableProfList;
//		private StoreFilterData _storeFilterDL;
//		private DataTable _dtFilter;
//		#endregion
//
//		#region Properties
//		public SessionAddressBlock SAB
//		{
//			get
//			{
//				return _SAB;
//			}
//		}
//
//		public ContextMenu LabelMenu
//		{
//			get
//			{
//				return mnuLabelMenu;
//			}
//		}
//
//		public TextBox LiteralEditTextBox
//		{
//			get
//			{
//				return txtLiteralEdit;
//			}
//		}
//
//		public TextBox GradeEditTextBox
//		{
//			get
//			{
//				return txtGradeEdit;
//			}
//		}
//
//		public Panel CurrentPanel
//		{
//			get
//			{
//				try
//				{
//					return (Panel)tabFilter.SelectedTab.Tag;
//				}
//				catch (Exception exc)
//				{
//					throw;
//				}
//			}
//		}
//
//		public ProfileList VersionProfileList
//		{
//			get
//			{
//				return _versionProfList;
//			}
//		}
//
//		public ProfileList VariableProfileList
//		{
//			get
//			{
//				return _variableProfList;
//			}
//		}
//
//		public ProfileList TimeTotalVariableProfileList
//		{
//			get
//			{
//				return _timeTotalVariableProfList;
//			}
//		}
//
//		public int MultiSelectStartIdx
//		{
//			get
//			{
//				return _multiSelectStartIdx;
//			}
//			set
//			{
//				_multiSelectStartIdx = value;
//			}
//		}
//
//		public int MultiSelectEndIdx
//		{
//			get
//			{
//				return _multiSelectEndIdx;
//			}
//			set
//			{
//				_multiSelectEndIdx = value;
//			}
//		}
//		#endregion
//
//		#region Event Handlers
//		#region frmFilter Events
//
//		public void InitializeForm() {
//			InitializeForm(-1);
//		}
//		public void InitializeForm(int selectFilterID)
//		{
//			PanelTag panelTag;
//			DataTable dtVariables;
//			DataTable dtVersions;
//			DataTable dtStatus;
//			DataTable dtStatusText;
//			int i;
//
//			try
//			{
//				cboFilterName.Tag = "IgnoreMouseWheel";
//
//				_toolTip = new ToolTip();
//				_storeFilterDL = new StoreFilterData();
//
//				_filterUserSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersUser);
//				_filterGlobalSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersGlobal);
//
//				// Create Profile Lists
//
//				_variableProfList = _SAB.ApplicationServerSession.Variables.VariableProfileList;
//				_timeTotalVariableProfList = _SAB.ApplicationServerSession.TimeTotalVariables.TimeTotalVariableProfileList;
//				_versionProfList = _SAB.ClientServerSession.GetUserForecastVersions();
//
//				tpgAttributes.Tag = pnlAttrQuery;
//				_currentFilter = new FilterDefinition(_SAB, _SAB.ClientServerSession, _storeFilterDL, new LabelCreatorDelegate(LabelCreator));
//
//				panelTag = new PanelTag(typeof(AttrQuerySpacerOperand), btnAttrOr, _currentFilter.AttrOperandList);
//				panelTag.AllowedDropTypes.Add(typeof(GenericQueryOperand));
//				panelTag.AllowedDropTypes.Add(typeof(AttrQueryOperand));
//				panelTag.AllowedDropTypes.Add(typeof(MIDStoreNode));
//
//				pnlAttrQuery.Tag = panelTag;
//				btnAttrAnd.Tag = typeof(GenericQueryAndOperand);
//				btnAttrOr.Tag = typeof(GenericQueryOrOperand);
//				btnAttrLParen.Tag = typeof(GenericQueryLeftParenOperand);
//				btnAttrRParen.Tag = typeof(GenericQueryRightParenOperand);
//
//				tpgData.Tag = pnlDataQuery;
//
//				panelTag = new PanelTag(typeof(DataQuerySpacerOperand), btnDataOr, _currentFilter.DataOperandList);
//				panelTag.AllowedDropTypes.Add(typeof(GenericQueryOperand));
//				panelTag.AllowedDropTypes.Add(typeof(DataQueryOperand));
//
//				pnlDataQuery.Tag = panelTag;
//				btnDataAnd.Tag = typeof(GenericQueryAndOperand);
//				btnDataOr.Tag = typeof(GenericQueryOrOperand);
//				btnDataLParen.Tag = typeof(GenericQueryLeftParenOperand);
//				btnDataRParen.Tag = typeof(GenericQueryRightParenOperand);
//				btnDataEqual.Tag = typeof(DataQueryEqualOperand);
//				btnDataLess.Tag = typeof(DataQueryLessOperand);
//				btnDataGreater.Tag = typeof(DataQueryGreaterOperand);
//				btnDataLessEqual.Tag = typeof(DataQueryLessEqualOperand);
//				btnDataGreaterEqual.Tag = typeof(DataQueryGreaterEqualOperand);
//				btnDataNot.Tag = typeof(DataQueryNotOperand);
//				btnDataStoreDetail.Tag = typeof(DataQueryStoreDetailOperand);
//				btnDataStoreTotal.Tag = typeof(DataQueryStoreTotalOperand);
//				btnDataStoreAverage.Tag = typeof(DataQueryStoreAverageOperand);
//				btnDataChainDetail.Tag = typeof(DataQueryChainDetailOperand);
//				btnDataDate.Tag = typeof(DataQueryDateRangeOperand);
//				btnDataAll.Tag = typeof(DataQueryAllOperand);
//				btnDataAny.Tag = typeof(DataQueryAnyOperand);
//				btnDataAverage.Tag = typeof(DataQueryAverageOperand);
//				btnDataTotal.Tag = typeof(DataQueryTotalOperand);
//				btnDataCorresponding.Tag = typeof(DataQueryCorrespondingOperand);
//				btnDataLiteral.Tag = typeof(DataQueryLiteralOperand);
//				btnDataGrade.Tag = typeof(DataQueryGradeOperand);
//				btnDataPctChange.Tag = typeof(DataQueryPctChangeOperand);
//				btnDataPctOf.Tag = typeof(DataQueryPctOfOperand);
//
//				lstVersions.Tag = lblVersion;
//				lstVariables.Tag = lblVariable;
//				lstStatus.Tag = lblStatus;
//				lblVersion.Tag = new LabelTag(lstVersions);
//				lblVariable.Tag = new LabelTag(lstVariables);
//				lblStatus.Tag = new LabelTag(lstStatus);
//
//				// Setup Variables ListBox
//
//				dtVariables = MIDEnvironment.CreateDataTable("Variables");
//				dtVariables.Columns.Add("Description", typeof(string));
//				dtVariables.Columns.Add("Profile", typeof(object));
//
//				foreach (VariableProfile varProf in _variableProfList)
//				{
//					dtVariables.Rows.Add(new object[] {varProf.VariableName, varProf});
//				}
//
//				foreach (VariableProfile varProf in _variableProfList)
//				{
//					for (i = 0; i < varProf.TimeTotalVariables.Count; i++)
//					{
//						dtVariables.Rows.Add(new object[] {"[Date Total] " + ((TimeTotalVariableProfile)varProf.TimeTotalVariables[i]).VariableName, new TimeTotalVariableReference((TimeTotalVariableProfile)varProf.TimeTotalVariables[i], varProf, i + 1)});
//					}
//				}
//
//				lstVariables.DataSource = dtVariables;
//				lstVariables.DisplayMember = "Description";
//				lstVariables.ValueMember = "Profile";
//
//				// Setup Versions ListBox
//
//				dtVersions = MIDEnvironment.CreateDataTable("Versions");
//				dtVersions.Columns.Add("Description", typeof(string));
//				dtVersions.Columns.Add("Profile", typeof(object));
//
//				foreach (VersionProfile verProf in _versionProfList)
//				{
//					dtVersions.Rows.Add(new object[] {verProf.Description, verProf});
//				}
//
//				lstVersions.DataSource = dtVersions;
//				lstVersions.DisplayMember = "Description";
//				lstVersions.ValueMember = "Profile";
//
//				// Setup Status ListBox
//
//				dtStatus = MIDEnvironment.CreateDataTable("Status");
//				dtStatus.Columns.Add("Description", typeof(string));
//				dtStatus.Columns.Add("Profile", typeof(object));
//
//				dtStatusText = MIDText.GetTextType(eMIDTextType.eStoreStatus, eMIDTextOrderBy.TextCode);
//
//				foreach (DataRow row in dtStatusText.Rows)
//				{
//					dtStatus.Rows.Add(new object[] {row["TEXT_VALUE"], new StatusProfile((eStoreStatus)Convert.ToInt32(row["TEXT_CODE"]), Convert.ToString(row["TEXT_VALUE"]))});
//				}
//
//				lstStatus.DataSource = dtStatus;
//				lstStatus.DisplayMember = "Description";
//				lstStatus.ValueMember = "Profile";
//
//				// MIDFormBase stuff
//
//				FunctionSecurity = new FunctionSecurityProfile(Convert.ToInt32(eSecurityFunctions.ToolsFilters));
//				FunctionSecurity.SetFullControl();
////				AllowUpdate = true;  Security changes 1/24/05 vg
//				SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg
//
//				// Load Filters
//				int returnID = BindFilterComboBox(selectFilterID);
//
//				FormLoaded = true;
//
//				// Select filter becaause a key was passed in and we found it while
//				// adding to the combobox
//				if(returnID > -1)
//					cboFilterName.SelectedIndex = returnID;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public void ClearForm()
//		{
//			PanelTag panelTag;
//
//			try
//			{
//				panelTag = (PanelTag)((Panel)tpgAttributes.Tag).Tag;
//
//				foreach (QueryOperand operand in panelTag.OperandArray)
//				{
//					operand.Label.Parent = null;
//				}
//
//				panelTag.Clear();
//
//				panelTag = (PanelTag)((Panel)tpgData.Tag).Tag;
//
//				foreach (QueryOperand operand in panelTag.OperandArray)
//				{
//					operand.Label.Parent = null;
//				}
//
//				panelTag.Clear();
//
//				ChangePending = false;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public void HandleExceptions(Exception exc)
//		{
//			Debug.WriteLine(exc.ToString());
//			MessageBox.Show(exc.ToString());
//		}
//
//		#endregion
//		
//		#region tabFilter Events
//		private void tabFilter_SizeChanged(object sender, System.EventArgs e)
//		{
//			try
//			{
//				CurrentPanelRedrawOperands();
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region tpgData Events
//		private void tpgData_MouseEnter(object sender, System.EventArgs e)
//		{
//			try
//			{
//				lstVariables.Visible = false;
//				lstVersions.Visible = false;
//				lstStatus.Visible = false;
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region pnlDataQuery Events
//		private void pnlDataQuery_MouseEnter(object sender, System.EventArgs e)
//		{
//			try
//			{
//				lstVariables.Visible = false;
//				lstVersions.Visible = false;
//				lstStatus.Visible = false;
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region panel Events
//		private void panel_Click(object sender, EventArgs e)
//		{
//			try
//			{
//				((PanelTag)((Panel)sender).Tag).LastOperandClicked = null;
//				ClearSelectedOperands((Panel)sender);
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		public void panel_DragEnter(object sender, DragEventArgs e)
//		{
//			FilterDragObject filterDragObject;
//
//			try
//			{
//				if (e.Data.GetDataPresent(typeof(FilterDragObject)))
//				{
//					filterDragObject = (FilterDragObject)e.Data.GetData(typeof(FilterDragObject));
//					foreach (Type allowType in ((PanelTag)((Panel)sender).Tag).AllowedDropTypes)
//					{
//						if (filterDragObject.DragObject.GetType().IsSubclassOf(allowType) || filterDragObject.DragObject.GetType() == allowType)
//						{
//							if (!filterDragObject.DragObject.GetType().IsSubclassOf(typeof(QueryOperand)) ||
//								(filterDragObject.DragObject.GetType().IsSubclassOf(typeof(QueryOperand)) && ((QueryOperand)filterDragObject.DragObject).isMainOperand))
//							{
//								ClearSelectedOperands((Panel)sender);
//								e.Effect = e.AllowedEffect;
//								return;
//							}
//						}
//					}
//				}
//				else
//				{
//					foreach (Type allowType in ((PanelTag)((Panel)sender).Tag).AllowedDropTypes)
//					{
//						if (e.Data.GetDataPresent(allowType))
//						{
//							ClearSelectedOperands((Panel)sender);
//							e.Effect = e.AllowedEffect;
//							return;
//						}
//					}
//				}
//
//				e.Effect = DragDropEffects.None;
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		public void panel_DragDrop(object sender, DragEventArgs e)
//		{
//			QueryOperand operand;
//			MIDStoreNode storeNode;
//			AttrQueryAttributeMainOperand attrMainOperand;
//			AttrQueryStoreMainOperand AttrQueryStoreMainOperand;
//			FilterDragObject dragObject;
//
//			try
//			{
//				HighlightLineLastOperand((Panel)sender, e);
//
//				if (e.Data.GetDataPresent(typeof(FilterDragObject)))
//				{
//					dragObject = (FilterDragObject)e.Data.GetData(typeof(FilterDragObject));
//					if (dragObject.DragObject.GetType().IsSubclassOf(typeof(QueryOperand)))
//					{
//						operand = (QueryOperand)dragObject.DragObject;
//						AddOperand((Panel)sender, operand);
//						ClearSelectedOperands((Panel)sender);
//						RedrawOperands((Panel)sender);
//						return;
//					}
//				}
//				else if (e.Data.GetDataPresent(typeof(MIDStoreNode)))
//				{
//					storeNode = (MIDStoreNode)e.Data.GetData(typeof(MIDStoreNode));
//					switch(storeNode.StoreNodeType)
//					{
//						case eStoreNodeType.group:
//							attrMainOperand = new AttrQueryAttributeMainOperand(_currentFilter, storeNode.GroupRID);
//							attrMainOperand.AddAllSets();
//							AddOperand((Panel)sender, attrMainOperand);
//							RedrawOperands((Panel)sender);
//							break;
//							
//						case eStoreNodeType.groupLevel:
//							attrMainOperand = new AttrQueryAttributeMainOperand(_currentFilter, storeNode.GroupRID);
//							attrMainOperand.AddSet(storeNode.GroupLevelRID);
//							AddOperand((Panel)sender, attrMainOperand);
//							RedrawOperands((Panel)sender);
//							break;
//
//						case eStoreNodeType.store:
//							AttrQueryStoreMainOperand = new AttrQueryStoreMainOperand(_currentFilter);
//							AttrQueryStoreMainOperand.AddStore(storeNode.StoreRID);
//							AddOperand((Panel)sender, AttrQueryStoreMainOperand);
//							RedrawOperands((Panel)sender);
//							break;
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		public void panel_DragLeave(object sender, EventArgs e)
//		{
//			try
//			{
//				ClearSelectedOperands((Panel)sender);
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region mniDelete Events
//		private void mniDelete_Click(object sender, EventArgs e)
//		{
//			try
//			{
//				CurrentPanelDeleteSelectedOperands();
//				CurrentPanelRedrawOperands();
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region mnuLabelMenu Events
//		private void mnuLabelMenu_Popup(object sender, EventArgs e)
//		{
//			try
//			{
//				if (((PanelTag)CurrentPanel.Tag).SelectedOperandList.Count == 0)
//				{
//					mniDelete.Enabled = false;
//				}
//				else
//				{
//					mniDelete.Enabled = true;
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region btnSave Events
//		private void btnSave_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Save();
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region void btnSaveAs Events
//		private void btnSaveAs_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				SaveAs();
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region btnNew Events
//		private void btnNew_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				if (CheckForPendingChanges())
//				{
//					cboFilterName.SelectedIndex = -1;
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region btnDelete Events
//		private void btnDelete_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				if (CheckForPendingChanges())
//				{
//					DeleteFilter();
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region btnCancel Events
//		private void btnCancel_Click(object sender, System.EventArgs e)
//		{
//			this.Close();
//		}
//		#endregion
//
//		#region cboFilterName Events
//		private void cboFilterName_SelectionChangeCommitted(object sender, System.EventArgs e)
//		{
//			try
//			{
//				if (FormLoaded)
//				{
//					if (CheckForPendingChanges())
//					{
//						ClearForm();
//						if (cboFilterName.SelectedIndex >= 0)
//						{
//							_currentFilter = new FilterDefinition(_SAB, _SAB.ClientServerSession, _storeFilterDL, new LabelCreatorDelegate(LabelCreator), _versionProfList, _variableProfList, _timeTotalVariableProfList, ((FilterNameCombo)cboFilterName.SelectedItem).FilterRID);
//						}
//						else
//						{
//							_currentFilter = new FilterDefinition(_SAB, _SAB.ClientServerSession, _storeFilterDL, new LabelCreatorDelegate(LabelCreator));
//						}
//
//						((PanelTag)pnlAttrQuery.Tag).OperandArray = _currentFilter.AttrOperandList;
//						((PanelTag)pnlDataQuery.Tag).OperandArray = _currentFilter.DataOperandList;
//
//						RedrawOperands(pnlAttrQuery);
//						RedrawOperands(pnlDataQuery);
//
//						ChangePending = false;
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region txtLiteralEdit Events
//		private void txtLiteralEdit_Leave(object sender, System.EventArgs e)
//		{
//			try
//			{
//				txtLiteralEdit.Enabled = false;
//				txtLiteralEdit.Visible = false;
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		private void txtLiteralEdit_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
//		{
//			try
//			{
//				if (e.KeyChar == 13 || e.KeyChar == 9)
//				{
//					((DataQueryLiteralOperand)txtLiteralEdit.Tag).LiteralValue = Convert.ToDouble(txtLiteralEdit.Text, CultureInfo.CurrentUICulture);
//					CurrentPanelClearSelectedOperands();
//					CurrentPanelRedrawOperands();
//					((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
//					e.Handled = true;
//				}
//				else if (e.KeyChar == 27)
//				{
//					((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
//					e.Handled = true;
//				}
//				else if (Char.IsNumber(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '.' || Char.IsControl(e.KeyChar))
//				{
//				}
//				else
//				{
//					e.Handled = true;
//				}
//			}
//			catch (FormatException)
//			{
//			}
//			catch (InvalidCastException)
//			{
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region txtGradeEdit Events
//		private void txtGradeEdit_Leave(object sender, System.EventArgs e)
//		{
//			try
//			{
//				txtGradeEdit.Enabled = false;
//				txtGradeEdit.Visible = false;
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		private void txtGradeEdit_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
//		{
//			try
//			{
//				if (e.KeyChar == 13 || e.KeyChar == 9)
//				{
//					((DataQueryGradeOperand)txtGradeEdit.Tag).GradeValue = txtGradeEdit.Text;
//					CurrentPanelClearSelectedOperands();
//					CurrentPanelRedrawOperands();
//					((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
//					e.Handled = true;
//				}
//				else if (e.KeyChar == 27)
//				{
//					((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
//					e.Handled = true;
//				}
//				else if (Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '.' || Char.IsControl(e.KeyChar))
//				{
//				}
//				else
//				{
//					e.Handled = true;
//				}
//			}
//			catch (FormatException)
//			{
//			}
//			catch (InvalidCastException)
//			{
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region toolList Events
//		private void toolList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
//		{
//			Size dragSize;
//		
//			try
//			{
//				_indexOfItemUnderMouseToDrag = ((ListBox)sender).IndexFromPoint(e.X, e.Y);
//
//				if (_indexOfItemUnderMouseToDrag != ListBox.NoMatches) 
//				{
//					dragSize = SystemInformation.DragSize;
//					_dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width /2), e.Y - (dragSize.Height /2)), dragSize);
//				} 
//				else
//				{
//					_dragBoxFromMouseDown = Rectangle.Empty;
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		private void toolList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
//		{
//			ListBox listBox;
//			Label label;
//			LabelTag labelTag;
//			object listItem;
//			object profile;
//
//			try
//			{
//				listBox = (ListBox)sender;
//				label = (Label)listBox.Tag;
//				labelTag = (LabelTag)label.Tag;
//
//				if ((e.Button & MouseButtons.Left) == MouseButtons.Left) 
//				{
//					if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y)) 
//					{
//						listBox.Visible = false;
//						listItem = listBox.Items[_indexOfItemUnderMouseToDrag];
//
//						if (listItem.GetType() == typeof(DataRowView))
//						{
//							profile = ((DataRowView)listItem).Row["Profile"];
//							if (profile.GetType() == typeof(VariableProfile))
//							{
//								labelTag.CurrentObject = profile;
//								label.Text = ((VariableProfile)profile).VariableName;
//								DoDragDrop(new FilterDragObject(new DataQueryPlanVariableOperand(_currentFilter, (VariableProfile)profile)), DragDropEffects.Move);
//							}
//							else if (profile.GetType() == typeof(TimeTotalVariableReference))
//							{
//								labelTag.CurrentObject = profile;
//								label.Text = ((TimeTotalVariableReference)profile).TimeTotalVariableProfile.VariableName;
//								DoDragDrop(new FilterDragObject(new DataQueryTimeTotalVariableOperand(_currentFilter, (TimeTotalVariableReference)profile)), DragDropEffects.Move);
//							}
//							else if (profile.GetType() == typeof(VersionProfile))
//							{
//								labelTag.CurrentObject = profile;
//								label.Text = ((VersionProfile)profile).Description;
//								DoDragDrop(new FilterDragObject(profile), DragDropEffects.Move);
//							}
//							else if (profile.GetType() == typeof(StatusProfile))
//							{
//								labelTag.CurrentObject = profile;
//								label.Text = ((StatusProfile)profile).Description;
//								DoDragDrop(new FilterDragObject(new DataQueryStatusOperand(_currentFilter, (eStoreStatus)((StatusProfile)profile).Key)), DragDropEffects.Move);
//							}
//						}
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		private void toolList_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
//		{
//			ListBox listBox;
//			Label label;
//			LabelTag labelTag;
//
//			try
//			{
//				listBox = (ListBox)sender;
//				label = (Label)listBox.Tag;
//				labelTag = (LabelTag)label.Tag;
//
//				_dragBoxFromMouseDown = Rectangle.Empty;
//
//				if (listBox.SelectedValue != null)
//				{
//					if (listBox.SelectedItem.GetType() == typeof(DataRowView))
//					{
//						if (listBox.SelectedValue.GetType() == typeof(VariableProfile))
//						{
//							labelTag.CurrentObject = listBox.SelectedValue;
//							label.Text = ((VariableProfile)listBox.SelectedValue).VariableName;
//						}
//						else if (((ListBox)sender).SelectedValue.GetType() == typeof(TimeTotalVariableReference))
//						{
//							labelTag.CurrentObject = listBox.SelectedValue;
//							label.Text = ((TimeTotalVariableReference)listBox.SelectedValue).TimeTotalVariableProfile.VariableName;
//						}
//						else if (((ListBox)sender).SelectedValue.GetType() == typeof(VersionProfile))
//						{
//							labelTag.CurrentObject = listBox.SelectedValue;
//							label.Text = ((VersionProfile)listBox.SelectedValue).Description;
//						}
//						else if (((ListBox)sender).SelectedValue.GetType() == typeof(StatusProfile))
//						{
//							labelTag.CurrentObject = listBox.SelectedValue;
//							label.Text = ((StatusProfile)listBox.SelectedValue).Description;
//						}
//					}
//				}
//
//				listBox.Visible = false;
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region toolLabel Events
//		private void toolLabel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
//		{
//			Size dragSize;
//		
//			try
//			{
//				dragSize = SystemInformation.DragSize;
//				_dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width /2), e.Y - (dragSize.Height /2)), dragSize);
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		private void toolLabel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
//		{
//			Label label;
//			LabelTag labelTag;
//			object currObject;
//
//			try
//			{
//				label = (Label)sender;
//				labelTag = (LabelTag)label.Tag;
//
//				if ((e.Button & MouseButtons.Left) == MouseButtons.Left) 
//				{
//					if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y)) 
//					{
//						labelTag.ListBox.Visible = false;
//						currObject = labelTag.CurrentObject;
//
//						if (labelTag.CurrentObject != null)
//						{
//							if (currObject.GetType() == typeof(VariableProfile))
//							{
//								DoDragDrop(new FilterDragObject(new DataQueryPlanVariableOperand(_currentFilter, (VariableProfile)currObject)), DragDropEffects.Move);
//							}
//							else if (currObject.GetType() == typeof(TimeTotalVariableReference))
//							{
//								DoDragDrop(new FilterDragObject(new DataQueryTimeTotalVariableOperand(_currentFilter, (TimeTotalVariableReference)currObject)), DragDropEffects.Move);
//							}
//							else if (currObject.GetType() == typeof(VersionProfile))
//							{
//								DoDragDrop(new FilterDragObject(currObject), DragDropEffects.Move);
//							}
//							else if (currObject.GetType() == typeof(StatusProfile))
//							{
//								DoDragDrop(new FilterDragObject(new DataQueryStatusOperand(_currentFilter, (eStoreStatus)((StatusProfile)currObject).Key)), DragDropEffects.Move);
//							}
//						}
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		private void toolLabel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
//		{
//			_dragBoxFromMouseDown = Rectangle.Empty;
//		}
//
//		private void toolLabel_Click(object sender, System.EventArgs e)
//		{
//			Label label;
//			LabelTag labelTag;
//
//			try
//			{
//				label = (Label)sender;
//				labelTag = (LabelTag)label.Tag;
//
//				if (labelTag.ListBox.Visible)
//				{
//					labelTag.ListBox.Visible = false;
//				}
//				else
//				{
//					labelTag.ListBox.Location = tpgData.PointToClient(label.PointToScreen(new Point(0, label.Height)));
//					labelTag.ListBox.Width = label.Width;
//					labelTag.ListBox.BringToFront();
//					labelTag.ListBox.Visible = true;
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//
//		#region toolButton Events
//		private void toolButton_Click(object sender, EventArgs e)
//		{
//			QueryOperand operand;
//
//			try
//			{
//				operand = (QueryOperand)Activator.CreateInstance((Type)((Button)sender).Tag, new object[] {_currentFilter});
//				if (operand.isMainOperand)
//				{
//					CurrentPanelAddOperand(operand);
//					CurrentPanelRedrawOperands();
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		private void toolButton_MouseDown(object sender, MouseEventArgs e)
//		{
//			Size dragSize;
//		
//			try
//			{
//				dragSize = SystemInformation.DragSize;
//				_dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width /2), e.Y - (dragSize.Height /2)), dragSize);
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		private void toolButton_MouseMove(object sender, MouseEventArgs e)
//		{
//			try
//			{
//				if ((e.Button & MouseButtons.Left) == MouseButtons.Left) 
//				{
//					if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y)) 
//					{
//						DoDragDrop(new FilterDragObject((QueryOperand)Activator.CreateInstance((Type)((Button)sender).Tag, new object[] {_currentFilter})), DragDropEffects.Move);
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		private void toolButton_MouseUp(object sender, MouseEventArgs e)
//		{
//			_dragBoxFromMouseDown = Rectangle.Empty;
//		}
//
//		private void toolButton_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
//		{
//			try
//			{
//				switch (e.KeyCode)
//				{
//					case Keys.Delete :
//
//						if (((PanelTag)CurrentPanel.Tag).SelectedOperandList.Count > 0)
//						{
//							CurrentPanelDeleteSelectedOperands();
//							CurrentPanelRedrawOperands();
//						}
//
//						break;
//				}
//
//				e.Handled = true;
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		private void toolButton_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
//		{
//			try
//			{
//				if (((PanelTag)CurrentPanel.Tag).SelectedOperandList.Count == 1)
//				{
//					((BasicQueryLabel)((QueryOperand)((PanelTag)CurrentPanel.Tag).SelectedOperandList[0]).Label).StartEdit(e.KeyChar);
//				}
//
//				e.Handled = true;
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//		#endregion
//
//		#region Public Methods
//		override protected bool SaveChanges()
//		{
//			try
//			{
//				if (Save())
//				{
//					ErrorFound = false;
//				}
//				else
//				{
//					ErrorFound = true;
//				}
//
//				return true;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public void AddOperand(Panel aPanel, QueryOperand aOperand)
//		{
//			int startIdx;
//
//			try
//			{
//				if (((PanelTag)aPanel.Tag).SelectedOperandList.Count == 0)
//				{
//					aOperand.Label.Parent = aPanel;
//					((PanelTag)aPanel.Tag).OperandArray.Add(aOperand);
//				}
//				else
//				{
//					startIdx = (int)((QueryOperand)((PanelTag)aPanel.Tag).SelectedOperandList[0]).Label.Tag;
//
//					DeleteSelectedOperands(aPanel);
//
//					aOperand.Label.Parent = aPanel;
//
//					((PanelTag)aPanel.Tag).OperandArray.Insert(startIdx, aOperand);
//				}
//
//				ChangePending = true;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		private void DeleteSelectedOperands(Panel aPanel)
//		{
//			try
//			{
//				if (((PanelTag)aPanel.Tag).SelectedOperandList.Count > 0)
//				{
//					foreach (QueryOperand operand in ((PanelTag)aPanel.Tag).SelectedOperandList)
//					{
//						operand.OnDelete();
//						operand.Label.Parent = null;
//					}
//
//					((PanelTag)aPanel.Tag).OperandArray.RemoveRange((int)((QueryOperand)((PanelTag)aPanel.Tag).SelectedOperandList[0]).Label.Tag, ((PanelTag)aPanel.Tag).SelectedOperandList.Count);
//
//					ClearSelectedOperands(aPanel);
//
//					ChangePending = true;
//				}
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public void ClearSelectedOperands(Panel aPanel)
//		{
//			try
//			{
//				foreach (QueryOperand operand in ((PanelTag)aPanel.Tag).SelectedOperandList)
//				{
//					((BasicQueryLabel)operand.Label).Unhighlight();
//				}
//
//				((PanelTag)aPanel.Tag).SelectedOperandList.Clear();
//
//				ChangePending = true;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public void RedrawOperands(Panel aPanel)
//		{
//			ArrayList operandRedrawList;
//			ArrayList newOperandArray;
//			QueryOperand spacer;
//			int line;
//			int index;
//			int left;
//			int top;
//			int spacerHeight;
//			int spacerWidth;
//
//			try
//			{
//				operandRedrawList = new ArrayList();
//				newOperandArray = new ArrayList();
//
//				line = 0;
//				index = 0;
//				top = 0;
//				left = 0;
//				((PanelTag)aPanel.Tag).LineOperandList.Clear();
//
//				spacer = (QueryOperand)System.Activator.CreateInstance(((PanelTag)aPanel.Tag).SpacerOperandType, new object[] {_currentFilter});
//				spacerHeight = spacer.Label.Height;
//				spacerWidth = spacer.Label.Width;
//
//				((PanelTag)aPanel.Tag).LineOperandList.Add(new LineInfo(0, spacerHeight - 1));
//
//				foreach (QueryOperand operand in ((PanelTag)aPanel.Tag).OperandArray)
//				{
//					operand.Label.Parent = null;
//				}
//
//				foreach (QueryOperand operand in ((PanelTag)aPanel.Tag).OperandArray)
//				{
//					if (operand.GetType() != ((PanelTag)aPanel.Tag).SpacerOperandType)
//					{
//						operandRedrawList.Clear();
//						operand.OnRedraw(operandRedrawList);
//
//						foreach (QueryOperand newOperand in operandRedrawList)
//						{
//							if (((LineInfo)((PanelTag)aPanel.Tag).LineOperandList[line]).LineOperands.Count > 1 && newOperand.Label.Width + spacerWidth + left > aPanel.Width)
//							{
//								line++;
//								top += newOperand.Label.Height;
//								left = 0;
//
//								((PanelTag)aPanel.Tag).LineOperandList.Add(new LineInfo(top, top + spacerHeight - 1));
//							}
//
//							if (newOperand.isMainOperand)
//							{
//								spacer = (QueryOperand)System.Activator.CreateInstance(((PanelTag)aPanel.Tag).SpacerOperandType, new object[] {_currentFilter});
//								spacer.Label.Parent = aPanel;
//								spacer.Label.Tag = index;
//								spacer.Label.Left = left;
//								spacer.Label.Top = top;
//								left += spacer.Label.Width;
//								index++;
//
//								newOperandArray.Add(spacer);
//								((LineInfo)((PanelTag)aPanel.Tag).LineOperandList[line]).LineOperands.Add(spacer);
//							}
//
//							newOperand.Label.Parent = aPanel;
//							newOperand.Label.Tag = index;
//							newOperand.Label.Left = left;
//							newOperand.Label.Top = top;
//							left += newOperand.Label.Width;
//							index++;
//
//							newOperandArray.Add(newOperand);
//							((LineInfo)((PanelTag)aPanel.Tag).LineOperandList[line]).LineOperands.Add(newOperand);
//						}
//					}
//				}
//
//				((PanelTag)aPanel.Tag).OperandArray.Clear();
//				((PanelTag)aPanel.Tag).OperandArray.AddRange(newOperandArray);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public void CurrentPanelAddOperand(QueryOperand aOperand)
//		{
//			try
//			{
//				AddOperand(CurrentPanel, aOperand);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		private void CurrentPanelDeleteSelectedOperands()
//		{
//			try
//			{
//				DeleteSelectedOperands(CurrentPanel);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public void CurrentPanelClearSelectedOperands()
//		{
//			try
//			{
//				ClearSelectedOperands(CurrentPanel);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public void CurrentPanelRedrawOperands()
//		{
//			try
//			{
//				RedrawOperands(CurrentPanel);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public void CurrentPanelSelectOperandRange(int aStartIdx, int aEndIdx)
//		{
//			int i;
//			int startIdx;
//			int endIdx;
//
//			try
//			{
//				if (aStartIdx <= aEndIdx)
//				{
//					startIdx = aStartIdx;
//					endIdx = aEndIdx;
//				}
//				else
//				{
//					startIdx = aEndIdx;
//					endIdx = aStartIdx;
//				}
//
//				((PanelTag)CurrentPanel.Tag).SelectedOperandList.Clear();
//
//				for (i = 0; i < endIdx; i++)
//				{
//					((BasicQueryLabel)((QueryOperand)((PanelTag)CurrentPanel.Tag).OperandArray[i]).Label).Unhighlight();
//				}
//			
//				for (i = startIdx; i <= endIdx; i++)
//				{
//					((BasicQueryLabel)((QueryOperand)((PanelTag)CurrentPanel.Tag).OperandArray[i]).Label).Highlight();
//					((PanelTag)CurrentPanel.Tag).SelectedOperandList.Add(((PanelTag)CurrentPanel.Tag).OperandArray[i]);
//				}
//		
//				for (i = endIdx + 1; i < ((PanelTag)CurrentPanel.Tag).OperandArray.Count; i++)
//				{
//					((BasicQueryLabel)((QueryOperand)((PanelTag)CurrentPanel.Tag).OperandArray[i]).Label).Unhighlight();
//				}
//
//				((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		private int BindFilterComboBox(int selectFilterID)
//		{
//			ArrayList userRIDList;
//			bool holdFormLoaded;
//
//			try
//			{
//				holdFormLoaded = FormLoaded;
//				FormLoaded = false;
//				userRIDList = new ArrayList();
//
//				if (_filterUserSecurity.AllowUpdate || _filterUserSecurity.AllowView)
//				{
//					userRIDList.Add(_SAB.ClientServerSession.UserRID);
//				}
//
//				if (_filterGlobalSecurity.AllowUpdate || _filterGlobalSecurity.AllowView)
//				{
//					userRIDList.Add(Include.UndefinedUserRID);
//				}
//
//				_dtFilter = _storeFilterDL.StoreFilter_Read(userRIDList);
//				cboFilterName.Items.Clear();
//
//				int returnID = -1;
//				foreach (DataRow row in _dtFilter.Rows)
//				{
//					int i = cboFilterName.Items.Add(
//						new FilterNameCombo(Convert.ToInt32(row["STORE_FILTER_RID"], CultureInfo.CurrentUICulture),
//						Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
//						Convert.ToString(row["STORE_FILTER_NAME"], CultureInfo.CurrentUICulture)));
//					if(Convert.ToInt32(row["STORE_FILTER_RID"], CultureInfo.CurrentUICulture) == selectFilterID)
//						returnID = i;
//				}
//
//				FormLoaded = holdFormLoaded;
//
//				ClearForm();
//				return returnID;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		private void HighlightLineLastOperand(Panel aPanel, DragEventArgs e)
//		{
//			Point mousePoint;
//			int i;
//			int line;
//			QueryOperand insertOperand;
//
//			try
//			{
//				mousePoint = aPanel.PointToClient(new Point(e.X, e.Y));
//				line = ((PanelTag)aPanel.Tag).LineOperandList.Count + 1;
//
//				for (i = 0; i < ((PanelTag)aPanel.Tag).LineOperandList.Count; i++)
//				{
//					if (mousePoint.Y <= ((LineInfo)((PanelTag)aPanel.Tag).LineOperandList[i]).LineYEnd)
//					{
//						line = i + 1;
//						break;
//					}
//				}
//
//				if (line < ((PanelTag)aPanel.Tag).LineOperandList.Count)
//				{
//					insertOperand = null;
//					while (insertOperand == null && line < ((PanelTag)aPanel.Tag).LineOperandList.Count)
//					{
//						foreach (QueryOperand operand in ((LineInfo)((PanelTag)aPanel.Tag).LineOperandList[line]).LineOperands)
//						{
//							if (operand.GetType() == ((PanelTag)aPanel.Tag).SpacerOperandType)
//							{
//								insertOperand = operand;
//								break;
//							}
//						}
//						line++;
//					}
//					if (insertOperand != null)
//					{
//						((BasicQueryLabel)insertOperand.Label).HighlightClick();
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		private bool Save()
//		{
//			try
//			{
//				if (cboFilterName.SelectedIndex == -1)
//				{
//					return SaveAs();
//				}
//				else
//				{
//					if ((((FilterNameCombo)cboFilterName.SelectedItem).UserRID != Include.UndefinedUserRID && !_filterUserSecurity.AllowUpdate) ||
//						(((FilterNameCombo)cboFilterName.SelectedItem).UserRID == Include.UndefinedUserRID && !_filterGlobalSecurity.AllowUpdate))
//					{
//						MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
//						return false;
//					}
//
//					_currentFilter.SaveFilter(((FilterNameCombo)cboFilterName.SelectedItem).FilterRID, ((FilterNameCombo)cboFilterName.SelectedItem).UserRID, ((FilterNameCombo)cboFilterName.SelectedItem).FilterName);
//
//					ChangePending = false;
//					return true;
//				}
//			}
//			catch (FilterSyntaxErrorException exc)
//			{
//				switch (exc.FilterListType)
//				{
//					case eFilterListType.Attribute :
//						tabFilter.SelectedTab = tpgAttributes;
//						break;
//
//					case eFilterListType.Data :
//						tabFilter.SelectedTab = tpgData;
//						break;
//				}
//
//				if (exc.ErrorOperand != null)
//				{
//					((BasicQueryLabel)exc.ErrorOperand.Label).HighlightClick();
//				}
//
//				System.Windows.Forms.MessageBox.Show(this, exc.Message, "Filter Syntax Error");
//
//				return false;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		private bool SaveAs()
//		{
//			frmSaveAs formSaveAs;
//			bool continueSave;
//			bool saveCancelled;
//			int filterKey = 0;
//			int userRID = 0;
//			int newFilterKey = 0;
//
//			try
//			{
//// BEGIN MID Track #2618 - Save-as being allowed with no update security
////				if (_filterUserSecurity.AccessDenied && _filterGlobalSecurity.AccessDenied)
//				if (!_filterUserSecurity.AllowUpdate && !_filterGlobalSecurity.AllowUpdate)
//// END MID Track #2618 - Save-as being allowed with no update security
//				{
//					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
//					return false;
//				}
//
//				_currentFilter.CheckSyntax();
//
//				formSaveAs = new frmSaveAs(_SAB);
//				formSaveAs.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
//				formSaveAs.ShowUserGlobal = true;
//
//				if (cboFilterName.SelectedIndex != -1)
//				{
//					formSaveAs.SaveAsName = ((FilterNameCombo)cboFilterName.SelectedItem).FilterName;
//				}
//
//				if (_filterUserSecurity.IsReadOnly)
//				{
//					formSaveAs.isGlobalChecked = true;
//				}
//				else if (_filterGlobalSecurity.IsReadOnly)
//				{
//					formSaveAs.isUserChecked = true;
//				}
//				else
//				{
//					if (cboFilterName.SelectedIndex != -1 && ((FilterNameCombo)cboFilterName.SelectedItem).UserRID == Include.UndefinedUserRID)
//					{
//						formSaveAs.isGlobalChecked = true;
//					}
//					else
//					{
//						formSaveAs.isUserChecked = true;
//					}
//				}
//
//				if (_filterGlobalSecurity.AllowUpdate)
//				{
//					formSaveAs.isGlobalEnabled = true;
//				}
//				else
//				{
//					formSaveAs.isGlobalEnabled = false;
//				}
//
//				if (_filterUserSecurity.AllowUpdate)
//				{
//					formSaveAs.isUserEnabled = true;
//				}
//				else
//				{
//					formSaveAs.isUserEnabled = false;
//				}
//
//				continueSave = false;
//				saveCancelled = false;
//
//				while (!continueSave && !saveCancelled)
//				{
//					formSaveAs.ShowDialog(this);
//					saveCancelled = formSaveAs.SaveCanceled;
//
//					if (!saveCancelled)
//					{
//						if (formSaveAs.SaveAsName.Trim().Length > 0)
//						{
//							if (formSaveAs.isGlobalChecked)
//							{
//								userRID = Include.UndefinedUserRID;
//							}
//							else
//							{
//								userRID = _SAB.ClientServerSession.UserRID;
//							}
//
//							filterKey = _storeFilterDL.StoreFilter_GetKey(userRID, formSaveAs.SaveAsName);
//
//							if (filterKey == -1)
//							{
//								continueSave = true;
//							}
//							else
//							{
//								if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//								{
//									continueSave = true;
//								}
//							}
//						}
//						else
//						{
//							continueSave = true;
//						}
//					}
//				}
//
//				if (!saveCancelled)
//				{
//					newFilterKey = _currentFilter.SaveFilter(filterKey, userRID, formSaveAs.SaveAsName);
//					BindFilterComboBox(-1);
//					cboFilterName.SelectedIndex = cboFilterName.Items.IndexOf(new FilterNameCombo(newFilterKey, userRID, formSaveAs.SaveAsName));
//					ChangePending = false;
//				}
//
//				return true;
//			}
//			catch (FilterSyntaxErrorException exc)
//			{
//				switch (exc.FilterListType)
//				{
//					case eFilterListType.Attribute :
//						tabFilter.SelectedTab = tpgAttributes;
//						break;
//
//					case eFilterListType.Data :
//						tabFilter.SelectedTab = tpgData;
//						break;
//				}
//
//				if (exc.ErrorOperand != null)
//				{
//					((BasicQueryLabel)exc.ErrorOperand.Label).HighlightClick();
//				}
//
//				System.Windows.Forms.MessageBox.Show(this, exc.Message, "Filter Syntax Error");
//
//				return false;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		private void DeleteFilter()
//		{
//			int filterKey;
//			string filterName;
//
//			try
//			{
//				if (cboFilterName.SelectedIndex != -1)
//				{
//					if ((((FilterNameCombo)cboFilterName.SelectedItem).UserRID != Include.UndefinedUserRID && _filterUserSecurity.AllowUpdate) ||
//						(((FilterNameCombo)cboFilterName.SelectedItem).UserRID == Include.UndefinedUserRID && _filterGlobalSecurity.AllowUpdate))
//					{
//						filterKey = ((FilterNameCombo)cboFilterName.SelectedItem).FilterRID;
//						filterName = ((FilterNameCombo)cboFilterName.SelectedItem).FilterName;
//
//						string text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteFilter);
//						text = text.Replace("{0}", filterName);
//
//						if (MessageBox.Show(text, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
//						{
//							_storeFilterDL.OpenUpdateConnection();
//
//							try
//							{
//								_storeFilterDL.StoreFilter_Delete(filterKey);
//								_storeFilterDL.CommitData();
//								BindFilterComboBox(-1);
//							}
//							catch (Exception exc)
//							{
//								_storeFilterDL.Rollback();
//								throw;
//							}
//							finally
//							{
//								_storeFilterDL.CloseUpdateConnection();
//							}
//						}
//					}
//					else
//					{
//						MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized));
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public Label LabelCreator(FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//		{
//			Type type;
//
//			type = aQueryOperand.GetType();
//
//			if (type == typeof(GenericQueryAndOperand))
//			{
//				return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(GenericQueryOrOperand))
//			{
//				return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(GenericQueryLeftParenOperand))
//			{
//				return new GenericQueryLeftParenLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(GenericQueryRightParenOperand))
//			{
//				return new GenericQueryRightParenLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(AttrQueryAttributeMainOperand))
//			{
//				return new AttrQueryAttributeMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(AttrQueryAttributeDetailOperand))
//			{
//				return new AttrQueryAttributeDetailLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(AttrQueryAttributeSeparatorOperand))
//			{
//				return new AttrQueryAttributeMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(AttrQueryAttributeEndOperand))
//			{
//				return new AttrQueryAttributeMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(AttrQueryStoreMainOperand))
//			{
//				return new AttrQueryStoreMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(AttrQueryStoreDetailOperand))
//			{
//				return new AttrQueryStoreDetailLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(AttrQueryStoreSeparatorOperand))
//			{
//				return new AttrQueryStoreMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(AttrQueryStoreEndOperand))
//			{
//				return new AttrQueryStoreMainSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(AttrQuerySpacerOperand))
//			{
//				return new AttrQuerySpacerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryPlanVariableOperand))
//			{
//				return new DataQueryVariableSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryTimeTotalVariableOperand))
//			{
//				return new DataQueryVariableSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryPlanBeginOperand))
//			{
//				return new DataQueryVariableSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryPlanSeparatorOperand))
//			{
//				return new DataQueryVariableSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryPlanEndOperand))
//			{
//				return new DataQueryVariableSeparatorEndLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryNodeOperand))
//			{
//				return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryVersionOperand))
//			{
//				return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryDateRangeOperand))
//			{
//				return new DataQueryDateRangeLabel(this, aFilterDef, aQueryOperand, aForeColor, aText, (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[] { SAB }));
//			}
//			else if (type == typeof(DataQueryCubeModifyerOperand))
//			{
//				return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryStoreDetailOperand))
//			{
//				return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryStoreTotalOperand))
//			{
//				return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryStoreAverageOperand))
//			{
//				return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryChainDetailOperand))
//			{
//				return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryTimeModifyerOperand))
//			{
//				return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryAllOperand))
//			{
//				return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryAnyOperand))
//			{
//				return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryAverageOperand))
//			{
//				return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryTotalOperand))
//			{
//				return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryCorrespondingOperand))
//			{
//				return new DataQueryNodeVersionModifyerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryEqualOperand))
//			{
//				return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryLessOperand))
//			{
//				return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryGreaterOperand))
//			{
//				return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryLessEqualOperand))
//			{
//				return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryGreaterEqualOperand))
//			{
//				return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryNotOperand))
//			{
//				return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryPctChangeOperand))
//			{
//				return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryPctOfOperand))
//			{
//				return new BasicQueryLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryLiteralOperand))
//			{
//				return new DataQueryLiteralLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryGradeOperand))
//			{
//				return new DataQueryGradeLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQueryStatusOperand))
//			{
//				return new DataQueryStatusLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else if (type == typeof(DataQuerySpacerOperand))
//			{
//				return new DataQuerySpacerLabel(this, aFilterDef, aQueryOperand, aForeColor, aText);
//			}
//			else
//			{
//				throw new Exception("Invalid Operand Type in LabelCreator");
//			}
//		}
//		#endregion
//
//		#region IFormBase Members
//		override public void ICut()
//		{
//			try
//			{
//				MessageBox.Show("Not implemented yet");
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		override public void ICopy()
//		{
//			try
//			{
//				MessageBox.Show("Not implemented yet");
//			}
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		override public void IPaste()
//		{
//			try
//			{
//				MessageBox.Show("Not implemented yet");
//			}		
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}	
//
//		override public void ISave()
//		{
//			try
//			{
//				Save();
//			}		
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		override public void ISaveAs()
//		{
//			try
//			{
//				SaveAs();
//			}		
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//
//		override public void IDelete()
//		{
//			try
//			{
//				if (CheckForPendingChanges())
//				{
//					DeleteFilter();
//				}
//			}		
//			catch (Exception exc)
//			{
//				HandleExceptions(exc);
//			}
//		}
//		#endregion
//	}
//
//	#region FilterDragObject
//	/// <summary>
//	/// Class that defines the FilterDragObject, which is a generic object used during drag events.
//	/// </summary>
//
//	public class FilterDragObject
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		public object DragObject;
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public FilterDragObject(object aDragObject)
//		{
//			DragObject = aDragObject;
//		}
//
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//	}
//	#endregion
//
//	#region StatusProfile
//	/// <summary>
//	/// Class that defines the StatusProfile, that is used during the drag operation of a Status.
//	/// </summary>
//
//	public class StatusProfile : Profile
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		private string _description;
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public StatusProfile(eStoreStatus aStatus, string aDescription)
//			: base((int)aStatus)
//		{
//			_description = aDescription;
//		}
//
//		//===========
//		// PROPERTIES
//		//===========
//
//		public override eProfileType ProfileType
//		{
//			get
//			{
//				return eProfileType.StoreStatus;
//			}
//		}
//
//		public string Description
//		{
//			get
//			{
//				return _description;
//			}
//		}
//
//		//========
//		// METHODS
//		//========
//	}
//	#endregion
//
//	#region FilterNameCombo Class
//	/// <summary>
//	/// Class that defines the contents of the FilterName combo box.
//	/// </summary>
//
//	public class FilterNameCombo
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		private int _filterRID;
//		private int _userRID;
//		private string _filterName;
//		private string _displayName;
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public FilterNameCombo(int aFilterRID)
//		{
//			_filterRID = aFilterRID;
//		}
//
//		public FilterNameCombo(int aFilterRID, int aUserRID, string aFilterName)
//		{
//			_filterRID = aFilterRID;
//			_userRID = aUserRID;
//			_filterName = aFilterName;
//			if (aUserRID == Include.UndefinedUserRID)
//			{
//				_displayName = _filterName;
//			}
//			else
//			{
//				_displayName = _filterName + " (User)";
//			}
//		}
//
//		//===========
//		// PROPERTIES
//		//===========
//
//		public int FilterRID
//		{
//			get
//			{
//				return _filterRID;
//			}
//		}
//
//		public int UserRID
//		{
//			get
//			{
//				return _userRID;
//			}
//		}
//
//		public string FilterName
//		{
//			get
//			{
//				return _filterName;
//			}
//		}
//
//		//========
//		// METHODS
//		//========
//
//		override public string ToString()
//		{
//			return _displayName;
//		}
//
//		override public bool Equals(object obj)
//		{
//			if (((FilterNameCombo)obj).FilterRID == _filterRID)
//			{
//				return true;
//			}
//			else
//			{
//				return false;
//			}
//		}
//
//		override public int GetHashCode()
//		{
//			return _filterRID;
//		}
//	}
//	#endregion
//
//	#region LineInfo Class
//	/// <summary>
//	/// Class that defines area for holding information about each line in the query panel.
//	/// </summary>
//
//	public class LineInfo
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		private ArrayList _lineOperands;
//		private int _lineYStart;
//		private int _lineYEnd;
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public LineInfo()
//		{
//			_lineOperands = new ArrayList();
//			_lineYStart = 0;
//			_lineYEnd = 0;
//		}
//
//		public LineInfo(int aLineYStart)
//		{
//			_lineOperands = new ArrayList();
//			_lineYStart = aLineYStart;
//			_lineYEnd = 0;
//		}
//
//		public LineInfo(int aLineYStart, int aLineYEnd)
//		{
//			_lineOperands = new ArrayList();
//			_lineYStart = aLineYStart;
//			_lineYEnd = aLineYEnd;
//		}
//
//		//===========
//		// PROPERTIES
//		//===========
//
//		public ArrayList LineOperands
//		{
//			get
//			{
//				return _lineOperands;
//			}
//		}
//
//		public int LineYStart
//		{
//			get
//			{
//				return _lineYStart;
//			}
//			set
//			{
//				_lineYStart = value;
//			}
//		}
//
//		public int LineYEnd
//		{
//			get
//			{
//				return _lineYEnd;
//			}
//			set
//			{
//				_lineYEnd = value;
//			}
//		}
//
//		//========
//		// METHODS
//		//========
//	}
//	#endregion
//
//	#region PanelTag Class
//	/// <summary>
//	/// Class that defines the tag information stored in the filter query panels.
//	/// </summary>
//
//	public class PanelTag
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		private Type _spacerOperandType;
//		private Control _defaultControl;
//		private ArrayList _operandArray;
//		private ArrayList _selectedOperandList;
//		private ArrayList _lineOperandList;
//		private ArrayList _allowedDropTypes;
//		private QueryOperand _lastOperandClicked;
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public PanelTag(Type aSpacerOperandType, Control aDefaultControl, ArrayList aOperandArray)
//		{
//			try
//			{
//				_spacerOperandType = aSpacerOperandType;
//				_defaultControl = aDefaultControl;
//				_operandArray = aOperandArray;
//				_selectedOperandList = new ArrayList();
//				_lineOperandList = new ArrayList();
//				_allowedDropTypes = new ArrayList();
//				_lastOperandClicked = null;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		//===========
//		// PROPERTIES
//		//===========
//
//		public Type SpacerOperandType
//		{
//			get
//			{
//				return _spacerOperandType;
//			}
//		}
//
//		public Control DefaultControl
//		{
//			get
//			{
//				return _defaultControl;
//			}
//		}
//
//		public ArrayList OperandArray
//		{
//			get
//			{
//				return _operandArray;
//			}
//			set
//			{
//				_operandArray = value;
//			}
//		}
//
//		public ArrayList SelectedOperandList
//		{
//			get
//			{
//				return _selectedOperandList;
//			}
//		}
//
//		public ArrayList LineOperandList
//		{
//			get
//			{
//				return _lineOperandList;
//			}
//		}
//
//		public ArrayList AllowedDropTypes
//		{
//			get
//			{
//				return _allowedDropTypes;
//			}
//		}
//
//		public QueryOperand LastOperandClicked
//		{
//			get
//			{
//				return _lastOperandClicked;
//			}
//			set
//			{
//				_lastOperandClicked = value;
//			}
//		}
//
//		//========
//		// METHODS
//		//========
//
//		public void Clear()
//		{
//			try
//			{
//				_selectedOperandList.Clear();
//				_lineOperandList.Clear();
//				_lastOperandClicked = null;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//	}
//	#endregion
//
//	#region LabelTag Class
//	/// <summary>
//	/// Class that defines the tag information stored in the filter query panels.
//	/// </summary>
//
//	public class LabelTag
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		private ListBox _listBox;
//		private object _currObject;
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public LabelTag(ListBox aListBox)
//		{
//			_listBox = aListBox;
//			_currObject = null;
//		}
//
//		//===========
//		// PROPERTIES
//		//===========
//
//		public ListBox ListBox
//		{
//			get
//			{
//				return _listBox;
//			}
//		}
//
//		public Object CurrentObject
//		{
//			get
//			{
//				return _currObject;
//			}
//			set
//			{
//				_currObject = value;
//			}
//		}
//
//		//========
//		// METHODS
//		//========
//	}
//	#endregion
//
//	#region MultiSelect Class
//	/// <summary>
//	/// Class that identifies a multi-select operation in a drag-drop function.
//	/// </summary>
//
//	public class MultiSelect
//	{
//	}
//	#endregion
//
//	#region QueryOperand Class
//	/// <summary>
//	/// Class that defines the base QueryOperand Class.
//	/// </summary>
//
//	public class BasicQueryLabel : Label
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		protected frmFilter _form;
//		protected FilterDefinition _filterDef;
//		protected QueryOperand _queryOperand;
//		private Color _myDefaultBackColor;
//		private Color _myDefaultForeColor;
//		private Rectangle _dragBoxFromMouseDown;
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public BasicQueryLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base()
//		{
//			_form = aForm;
//			_filterDef = aFilterDef;
//			_queryOperand = aQueryOperand;
//
//			MyDefaultBackColor = Color.White;
//			MyDefaultForeColor = aForeColor;
//
//			Text = aText;
//
//			Initialize();
//		}
//
//		public BasicQueryLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor)
//			: base()
//		{
//			_form = aForm;
//			_filterDef = aFilterDef;
//			_queryOperand = aQueryOperand;
//
//			MyDefaultBackColor = Color.White;
//			MyDefaultForeColor = aForeColor;
//
//			Initialize();
//		}
//
//		//===========
//		// PROPERTIES
//		//===========
//
//		private void Initialize()
//		{
//			AutoSize = true;
//			BorderStyle = BorderStyle.None;
//			AllowDrop = true;
//			TextAlign = ContentAlignment.MiddleRight;
//			ContextMenu = _form.LabelMenu;
//			Font = new Font("Arial", 9);
//			DragEnter += new DragEventHandler(DragEnterHandler);
//			DragDrop += new DragEventHandler(DragDropHandler);
//			DragLeave += new EventHandler(DragLeaveHandler);
//			MouseDown += new MouseEventHandler(MouseDownHandler);
//			MouseMove += new MouseEventHandler(MouseMoveHandler);
//			MouseUp += new MouseEventHandler(MouseUpHandler);
//			DoubleClick += new EventHandler(DoubleClickHandler);
//			GiveFeedback += new GiveFeedbackEventHandler(GiveFeedbackHandler);
//		}
//
//		public Color MyDefaultBackColor
//		{
//			get
//			{
//				return _myDefaultBackColor;
//			}
//			set
//			{
//				_myDefaultBackColor = value;
//				BackColor = _myDefaultBackColor;
//			}
//		}
//
//		public Color MyDefaultForeColor
//		{
//			get
//			{
//				return _myDefaultForeColor;
//			}
//			set
//			{
//				_myDefaultForeColor = value;
//				ForeColor = _myDefaultForeColor;
//			}
//		}
//
//		//========
//		// METHODS
//		//========
//
//		public void Highlight()
//		{
//			try
//			{
//				BackColor = Color.Blue;
//				ForeColor = Color.White;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public void Unhighlight()
//		{
//			try
//			{
//				BackColor = _myDefaultBackColor;
//				ForeColor = _myDefaultForeColor;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		virtual public void HighlightClick()
//		{
//			try
//			{
//				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//				_form.CurrentPanelSelectOperandRange((int)Tag, (int)Tag);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		virtual public void HighlightDragEnter(out int aStartIdx, out int aEndIdx)
//		{
//			try
//			{
//				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//				aStartIdx = (int)Tag;
//				aEndIdx = aStartIdx;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		virtual public void StartEdit(char aKeyPressed)
//		{
//		}
//
//		public void MouseDownHandler(object sender, MouseEventArgs e)
//		{
//			Size dragSize;
//		
//			try
//			{
//				if ((e.Button & MouseButtons.Left) == MouseButtons.Left) 
//				{
//					dragSize = SystemInformation.DragSize;
//					_dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width /2), e.Y - (dragSize.Height /2)), dragSize);
//				}
//			}
//			catch (Exception exc)
//			{
//				_form.HandleExceptions(exc);
//			}
//		}
//
//		public void MouseMoveHandler(object sender, MouseEventArgs e)
//		{
//			int startIdx;
//			int endIdx;
//
//			try
//			{
//				if ((e.Button & MouseButtons.Left) == MouseButtons.Left) 
//				{
//					if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y)) 
//					{
//						HighlightDragEnter(out startIdx, out endIdx);
//						_form.MultiSelectStartIdx = startIdx;
//						_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
//						DoDragDrop(new MultiSelect(), DragDropEffects.Move);
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				_form.HandleExceptions(exc);
//			}
//		}
//
//		virtual public void MouseUpHandler(object sender, MouseEventArgs e)
//		{
//			try
//			{
//				_dragBoxFromMouseDown = Rectangle.Empty;
//				if ((e.Button & MouseButtons.Left) == MouseButtons.Left) 
//				{
//					HighlightClick();
//				}
//			}
//			catch (Exception exc)
//			{
//				_form.HandleExceptions(exc);
//			}
//		}
//
//		virtual public void DoubleClickHandler(object sender, EventArgs e)
//		{
//		}
//
//		public void GiveFeedbackHandler(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
//		{
//			e.UseDefaultCursors = false;
//		}
//
//		virtual public void DragEnterHandler(object sender, DragEventArgs e)
//		{
//			int startIdx;
//			int endIdx;
//			FilterDragObject filterDragObject;
//			MIDTreeNode treeNode;
//
//			try
//			{
//				if (e.Data.GetDataPresent(typeof(MultiSelect)))
//				{
//					HighlightDragEnter(out startIdx, out endIdx);
//					_form.CurrentPanelSelectOperandRange(_form.MultiSelectStartIdx, endIdx);
//					e.Effect = e.AllowedEffect;
//					return;
//				}
//				else
//				{
//					if (e.Data.GetDataPresent(typeof(FilterDragObject)))
//					{
//						filterDragObject = (FilterDragObject)e.Data.GetData(typeof(FilterDragObject));
//
//						if (filterDragObject.DragObject.GetType() == typeof(DataQueryDateRangeOperand)  ||
//							filterDragObject.DragObject.GetType().IsSubclassOf(typeof(DataQueryCubeModifyerOperand)) ||
//							filterDragObject.DragObject.GetType().IsSubclassOf(typeof(DataQueryTimeModifyerOperand)) ||
//							filterDragObject.DragObject.GetType() == typeof(VersionProfile))
//						{
//							if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
//							{
//								HighlightDragEnter(out startIdx, out endIdx);
//								_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
//								e.Effect = e.AllowedEffect;
//								return;
//							}
//						}
//						else
//						{
//							foreach (Type allowType in ((PanelTag)_form.CurrentPanel.Tag).AllowedDropTypes)
//							{
//								if (filterDragObject.DragObject.GetType().IsSubclassOf(allowType) || filterDragObject.DragObject.GetType() == allowType)
//								{
//									if (!filterDragObject.DragObject.GetType().IsSubclassOf(typeof(QueryOperand)) ||
//										(filterDragObject.DragObject.GetType().IsSubclassOf(typeof(QueryOperand)) && ((QueryOperand)filterDragObject.DragObject).isMainOperand))
//									{
//										HighlightDragEnter(out startIdx, out endIdx);
//										_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
//										e.Effect = e.AllowedEffect;
//										return;
//									}
//								}
//							}
//						}
//					}
//					else if (e.Data.GetDataPresent(typeof(MIDTreeNode)))
//					{
//						if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
//						{
//							treeNode = (MIDTreeNode)e.Data.GetData(typeof(MIDTreeNode));
//							if (treeNode.NodeType == eHierarchyNodeType.TreeNode)
//							{
//								HighlightDragEnter(out startIdx, out endIdx);
//								_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
//								e.Effect = e.AllowedEffect;
//								return;
//							}
//						}
//					}
//					else
//					{
//						foreach (Type allowType in ((PanelTag)_form.CurrentPanel.Tag).AllowedDropTypes)
//						{
//							if (e.Data.GetDataPresent(allowType))
//							{
//								HighlightDragEnter(out startIdx, out endIdx);
//								_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
//								e.Effect = e.AllowedEffect;
//								return;
//							}
//						}
//					}
//				}
//
//				e.Effect = DragDropEffects.None;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		virtual public void DragDropHandler(object sender, DragEventArgs e)
//		{
//			int startIdx;
//			int endIdx;
//			QueryOperand operand;
//			MIDTreeNode treeNode;
//			DataQueryVariableOperand variableOperand;
//			MIDStoreNode storeNode;
//			AttrQueryAttributeMainOperand attrMainOperand;
//			AttrQueryStoreMainOperand AttrQueryStoreMainOperand;
//			FilterDragObject dragObject;
//
//			try
//			{
//				if (e.Data.GetDataPresent(typeof(MultiSelect)))
//				{
//					HighlightDragEnter(out startIdx, out endIdx);
//					_form.MultiSelectEndIdx = endIdx;
//					_form.CurrentPanelSelectOperandRange(_form.MultiSelectStartIdx, _form.MultiSelectEndIdx);
//				}
//				else if (e.Data.GetDataPresent(typeof(FilterDragObject)))
//				{
//					dragObject = (FilterDragObject)e.Data.GetData(typeof(FilterDragObject));
//					if (dragObject.DragObject.GetType() == typeof(DataQueryDateRangeOperand))
//					{
//						if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
//						{
//							variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
//							variableOperand.DateRangeProfile = new DateRangeProfile(Include.UndefinedCalendarDateRange);
//							variableOperand.DateRangeProfile.DisplayDate = "<DateRange>";
//							_form.CurrentPanelClearSelectedOperands();
//							_form.CurrentPanelRedrawOperands();
//						}
//					}
//					else if (dragObject.DragObject.GetType().IsSubclassOf(typeof(DataQueryCubeModifyerOperand)))
//					{
//						if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
//						{
//							variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
//							variableOperand.CubeModifyer = ((DataQueryCubeModifyerOperand)dragObject.DragObject).CubeModifyer;
//							_form.CurrentPanelClearSelectedOperands();
//							_form.CurrentPanelRedrawOperands();
//						}
//					}
//					else if (dragObject.DragObject.GetType().IsSubclassOf(typeof(DataQueryTimeModifyerOperand)))
//					{
//						if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
//						{
//							variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
//							variableOperand.TimeModifyer = ((DataQueryTimeModifyerOperand)dragObject.DragObject).TimeModifyer;
//							_form.CurrentPanelClearSelectedOperands();
//							_form.CurrentPanelRedrawOperands();
//						}
//					}
//					else if (dragObject.DragObject.GetType() == typeof(VersionProfile))
//					{
//						if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
//						{
//							variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
//							variableOperand.VersionProfile = (VersionProfile)dragObject.DragObject;
//							_form.CurrentPanelClearSelectedOperands();
//							_form.CurrentPanelRedrawOperands();
//						}
//					}
//					else if (dragObject.DragObject.GetType().IsSubclassOf(typeof(QueryOperand)))
//					{
//						operand = (QueryOperand)dragObject.DragObject;
//						_form.CurrentPanelAddOperand(operand);
//						_form.CurrentPanelClearSelectedOperands();
//						_form.CurrentPanelRedrawOperands();
//						return;
//					}
//				}
//				else if (e.Data.GetDataPresent(typeof(MIDTreeNode)))
//				{
//					treeNode = (MIDTreeNode)e.Data.GetData(typeof(MIDTreeNode));
//					if (treeNode.NodeType == eHierarchyNodeType.TreeNode)
//					{
//						if (_queryOperand.GetType().IsSubclassOf(typeof(DataQueryPlanOperand)))
//						{
//							variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
//							variableOperand.NodeProfile = _form.SAB.HierarchyServerSession.GetNodeData(treeNode.NodeRID);
//							_form.CurrentPanelClearSelectedOperands();
//							_form.CurrentPanelRedrawOperands();
//						}
//					}
//				}
//				else if (e.Data.GetDataPresent(typeof(MIDStoreNode)))
//				{
//					storeNode = (MIDStoreNode)e.Data.GetData(typeof(MIDStoreNode));
//					switch(storeNode.StoreNodeType)
//					{
//						case eStoreNodeType.group:
//							attrMainOperand = new AttrQueryAttributeMainOperand(_filterDef, storeNode.GroupRID);
//							attrMainOperand.AddAllSets();
//							_form.CurrentPanelAddOperand(attrMainOperand);
//							_form.CurrentPanelRedrawOperands();
//							break;
//								
//						case eStoreNodeType.groupLevel:
//							if (_queryOperand.GetType().IsSubclassOf(typeof(AttrQueryAttributeOperand)) && storeNode.GroupRID == ((AttrQueryAttributeOperand)_queryOperand).AttributeRID)
//							{
//								attrMainOperand = ((AttrQueryAttributeOperand)_queryOperand).AttrMainOperand;
//								attrMainOperand.AddSet(storeNode.GroupLevelRID);
//								_form.CurrentPanelClearSelectedOperands();
//								_form.CurrentPanelRedrawOperands();
//							}
//							else
//							{
//								attrMainOperand = new AttrQueryAttributeMainOperand(_filterDef, storeNode.GroupRID);
//								attrMainOperand.AddSet(storeNode.GroupLevelRID);
//								_form.CurrentPanelAddOperand(attrMainOperand);
//								_form.CurrentPanelRedrawOperands();
//							}
//							break;
//
//						case eStoreNodeType.store:
//							if (_queryOperand.GetType().IsSubclassOf(typeof(AttrQueryStoreOperand)))
//							{
//								AttrQueryStoreMainOperand = ((AttrQueryStoreOperand)_queryOperand).StoreMainOperand;
//								AttrQueryStoreMainOperand.AddStore(storeNode.StoreRID);
//								_form.CurrentPanelClearSelectedOperands();
//								_form.CurrentPanelRedrawOperands();
//							}
//							else
//							{
//								AttrQueryStoreMainOperand = new AttrQueryStoreMainOperand(_filterDef);
//								AttrQueryStoreMainOperand.AddStore(storeNode.StoreRID);
//								_form.CurrentPanelAddOperand(AttrQueryStoreMainOperand);
//								_form.CurrentPanelRedrawOperands();
//							}
//							break;
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		virtual public void DragLeaveHandler(object sender, EventArgs e)
//		{
//			try
//			{
//				_form.CurrentPanelClearSelectedOperands();
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//	}
//	#endregion
//
//	#region GenericQueryOperand Classes
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	// Generic Query labels
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//
//	/// <summary>
//	/// Class that defines a Left Parenthesis label.
//	/// </summary>
//
//	public class GenericQueryLeftParenLabel : BasicQueryLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public GenericQueryLeftParenLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//	
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//
//		override public void HighlightClick()
//		{
//			int i;
//			int currLevel;
//			int startIdx;
//			int endIdx;
//
//			try
//			{
//				if (((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked == null || (int)Tag != (int)((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked.Label.Tag)
//				{
//					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = _queryOperand;
//					_form.CurrentPanelSelectOperandRange((int)Tag, (int)Tag);
//				}
//				else
//				{
//					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//					startIdx = (int)Tag;
//					endIdx = startIdx;
//					currLevel = 0;
//
//					for (i = (int)Tag + 1; i < ((PanelTag)_form.CurrentPanel.Tag).OperandArray.Count; i++)
//					{
//						if (((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType() == typeof(GenericQueryRightParenOperand))
//						{
//							if (currLevel == 0)
//							{
//								endIdx = i;
//								break;
//							}
//							else
//							{
//								currLevel--;
//							}
//						}
//						else
//						{
//							if (((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType() == typeof(GenericQueryLeftParenOperand))
//							{
//								currLevel++;
//							}
//						}
//					}
//
//					_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
//				}
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//	}
//
//	/// <summary>
//	/// Class that defines a Right Parenthesis label.
//	/// </summary>
//
//	public class GenericQueryRightParenLabel : BasicQueryLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public GenericQueryRightParenLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//	
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//
//		override public void HighlightClick()
//		{
//			int i;
//			int currLevel;
//			int startIdx;
//			int endIdx;
//
//			try
//			{
//				_form.CurrentPanelClearSelectedOperands();
//
//				if (((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked == null || (int)Tag != (int)((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked.Label.Tag)
//				{
//					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = _queryOperand;
//					_form.CurrentPanelSelectOperandRange((int)Tag, (int)Tag);
//				}
//				else
//				{
//					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//					startIdx = (int)Tag;
//					endIdx = startIdx;
//					currLevel = 0;
//
//					for (i = (int)Tag - 1; i >= 0; i--)
//					{
//						if (((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType() == typeof(GenericQueryLeftParenOperand))
//						{
//							if (currLevel == 0)
//							{
//								endIdx = i;
//								break;
//							}
//							else
//							{
//								currLevel--;
//							}
//						}
//						else
//						{
//							if (((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType() == typeof(GenericQueryRightParenOperand))
//							{
//								currLevel++;
//							}
//						}
//					}
//
//					_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
//				}
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//	}
//	#endregion
//
//	#region AttrQueryOperand Classes
//	/// <summary>
//	/// Abstract class that defines an attribute.
//	/// </summary>
//
//	abstract public class AttrQueryAttributeLabel : BasicQueryLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public AttrQueryAttributeLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//	
//		//========
//		// METHODS
//		//========
//
//		override public void HighlightDragEnter(out int aStartIdx, out int aEndIdx)
//		{
//			try
//			{
//				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//				HighlightAttribute(out aStartIdx, out aEndIdx);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public void HighlightAttribute(out int aStartIdx, out int aEndIdx)
//		{
//			int i;
//
//			try
//			{
//				for (i = (int)Tag; i >= 0 && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType() != typeof(AttrQueryAttributeMainOperand); i--)
//				{
//				}
//			
//				aStartIdx = i;
//
//				for (i = aStartIdx;  i < ((PanelTag)_form.CurrentPanel.Tag).OperandArray.Count && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType().IsSubclassOf(typeof(AttrQueryAttributeOperand)); i++)
//				{
//				}
//
//				aEndIdx = i - 1;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//	}
//
//	/// <summary>
//	/// Abstract class that defines a store.
//	/// </summary>
//
//	abstract public class AttrQueryStoreLabel : BasicQueryLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public AttrQueryStoreLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//	
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//
//		override public void HighlightDragEnter(out int aStartIdx, out int aEndIdx)
//		{
//			try
//			{
//				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//				HighlightStore(out aStartIdx, out aEndIdx);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public void HighlightStore(out int aStartIdx, out int aEndIdx)
//		{
//			int i;
//
//			try
//			{
//				for (i = (int)Tag; i >= 0 && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType().IsSubclassOf(typeof(AttrQueryStoreOperand)); i--)
//				{
//				}
//			
//				aStartIdx = i + 1;
//
//				for (i = aStartIdx;  i < ((PanelTag)_form.CurrentPanel.Tag).OperandArray.Count && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType().IsSubclassOf(typeof(AttrQueryStoreOperand)); i++)
//				{
//				}
//
//				aEndIdx = i - 1;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//	}
//
//	/// <summary>
//	/// Class that defines the attribute portion of an attribute definition label.
//	/// </summary>
//
//	public class AttrQueryAttributeMainSeparatorEndLabel : AttrQueryAttributeLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public AttrQueryAttributeMainSeparatorEndLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//	
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//
//		override public void HighlightClick()
//		{
//			int startIdx;
//			int endIdx;
//
//			try
//			{
//				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//				HighlightAttribute(out startIdx, out endIdx);
//				_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//	}
//
//	/// <summary>
//	/// Class that defines the attribute portion of an attribute definition label.
//	/// </summary>
//
//	public class AttrQueryAttributeDetailLabel : AttrQueryAttributeLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public AttrQueryAttributeDetailLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//	
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//	}
//
//	/// <summary>
//	/// Class that defines the store label portion of a store definition label.
//	/// </summary>
//
//	public class AttrQueryStoreMainSeparatorEndLabel : AttrQueryStoreLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public AttrQueryStoreMainSeparatorEndLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//	
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//
//		override public void HighlightClick()
//		{
//			int startIdx;
//			int endIdx;
//
//			try
//			{
//				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//				HighlightStore(out startIdx, out endIdx);
//				_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//	}
//
//	/// <summary>
//	/// Class that defines the store label portion of a store definition label.
//	/// </summary>
//
//	public class AttrQueryStoreDetailLabel : AttrQueryStoreLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public AttrQueryStoreDetailLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//	
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//	}
//
//	/// <summary>
//	/// Abstract class that defines a spacer.
//	/// </summary>
//
//	public class AttrQuerySpacerLabel : BasicQueryLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public AttrQuerySpacerLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor)
//		{
//			int height;
//
//			Text = " ";
//			height = Height;
//			AutoSize = false;
//			Width = 3;
//			Height = height;
//		}
//	
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//
//		override public void MouseUpHandler(object sender, MouseEventArgs e)
//		{
//		}
//	}
//	#endregion
//
//	#region DataQueryOperand Classes
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	// Data Query labels
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//	//==================================================================================
//
//	/// <summary>
//	/// Abstract class that defines a plan
//	/// </summary>
//
//	abstract public class DataQueryPlanLabel : BasicQueryLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public DataQueryPlanLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//	
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//
//		override public void HighlightDragEnter(out int aStartIdx, out int aEndIdx)
//		{
//			try
//			{
//				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//				HighlightPlan(out aStartIdx, out aEndIdx);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		public void HighlightPlan(out int aStartIdx, out int aEndIdx)
//		{
//			int i;
//
//			try
//			{
//				for (i = (int)Tag; i >= 0 && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType().IsSubclassOf(typeof(DataQueryPlanOperand)) && !((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType().IsSubclassOf(typeof(DataQueryVariableOperand)); i--)
//				{
//				}
//			
//				aStartIdx = i;
//
//				for (i = aStartIdx;  i < ((PanelTag)_form.CurrentPanel.Tag).OperandArray.Count && ((PanelTag)_form.CurrentPanel.Tag).OperandArray[i].GetType().IsSubclassOf(typeof(DataQueryPlanOperand)); i++)
//				{
//				}
//
//				aEndIdx = i - 1;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//	}
//
//	/// <summary>
//	/// Class that defines the main portion of a plan label.
//	/// </summary>
//
//	public class DataQueryVariableSeparatorEndLabel : DataQueryPlanLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public DataQueryVariableSeparatorEndLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//	
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//
//		override public void HighlightClick()
//		{
//			int startIdx;
//			int endIdx;
//
//			try
//			{
//				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//				HighlightPlan(out startIdx, out endIdx);
//				_form.CurrentPanelSelectOperandRange(startIdx, endIdx);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//	}
//
//	/// <summary>
//	/// Abstract class that defines the base DataQueryOperand class.  This class defines the base level functionality for a label that is displayed
//	/// in the data query panel.
//	/// </summary>
//
//	public class DataQueryNodeVersionModifyerLabel : DataQueryPlanLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public DataQueryNodeVersionModifyerLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//	
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//	}
//
//	/// <summary>
//	/// Abstract class that defines the base DataQueryOperand class.  This class defines the base level functionality for a label that is displayed
//	/// in the data query panel.
//	/// </summary>
//
//	public class DataQueryDateRangeLabel : DataQueryPlanLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		private CalendarDateSelector _calDateSel;
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public DataQueryDateRangeLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText, CalendarDateSelector aCalDateSel)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//			DataQueryVariableOperand variableOperand;
//
//			try
//			{
//				_calDateSel = aCalDateSel;
//				_calDateSel = new CalendarDateSelector(_form.SAB);
//				_calDateSel.AllowDynamicToStoreOpen = true;
//				_calDateSel.AnchorDate = _form.SAB.ClientServerSession.Calendar.CurrentDate;
//
//				variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
//
//				if (variableOperand.DateRangeProfile != null)
//				{
//					_calDateSel.DateRangeRID = variableOperand.DateRangeProfile.Key;
//					_calDateSel.AnchorDateRelativeTo = variableOperand.DateRangeProfile.RelativeTo;
//				}
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//
//		override public void HighlightClick()
//		{
//			DialogResult dateSelResult;
//			DataQueryVariableOperand variableOperand;
//
//			try
//			{
//				_form.CurrentPanelClearSelectedOperands();
//
//				if (((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked != null && (int)Tag == (int)((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked.Label.Tag)
//				{
//					variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
//
////					if (_calDateSel == null)
////					{
////						_calDateSel = new CalendarDateSelector(_form.SAB);
////						_calDateSel.AllowDynamicToStoreOpen = true;
////						_calDateSel.AnchorDate = _form.SAB.ClientServerSession.Calendar.CurrentDate;
////						if (variableOperand.DateRangeProfile != null)
////						{
////							_calDateSel.DateRangeRID = variableOperand.DateRangeProfile.Key;
////							_calDateSel.AnchorDateRelativeTo = variableOperand.DateRangeProfile.RelativeTo;
////						}
////					}
//
//					_calDateSel.StartPosition = FormStartPosition.CenterScreen;
//					dateSelResult = _calDateSel.ShowDialog();
//
//					if (dateSelResult == DialogResult.OK)
//					{
////						variableOperand = ((DataQueryPlanOperand)_queryOperand).VariableOperand;
//						variableOperand.DateRangeProfile = (DateRangeProfile)_calDateSel.Tag;
//						_form.CurrentPanelClearSelectedOperands();
//						_form.CurrentPanelRedrawOperands();
//					}
//
//					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//				}
//				else
//				{
//					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = _queryOperand;
//					_form.CurrentPanelSelectOperandRange((int)Tag, (int)Tag);
//				}
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//	}
//
//	/// <summary>
//	/// Class that defines an Not label.
//	/// </summary>
//
//	public class DataQueryLiteralLabel : BasicQueryLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public DataQueryLiteralLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//
//		override public void StartEdit(char aKeyPressed)
//		{
//			try
//			{
//				_form.LiteralEditTextBox.Tag = _queryOperand;
//				_form.LiteralEditTextBox.Width = Math.Max(70, Width);
//				_form.LiteralEditTextBox.Left = _form.CurrentPanel.Left + Left;
//				_form.LiteralEditTextBox.Top = _form.CurrentPanel.Top + Top;
//				_form.LiteralEditTextBox.BringToFront();
//				_form.LiteralEditTextBox.Visible = true;
//				_form.LiteralEditTextBox.Enabled = true;
//				_form.LiteralEditTextBox.Focus();
//
//				if (aKeyPressed != Char.MinValue && (Char.IsNumber(aKeyPressed) || aKeyPressed == '-' || aKeyPressed == '.'))
//				{
//					_form.LiteralEditTextBox.Text = Convert.ToString(aKeyPressed, CultureInfo.CurrentUICulture);
//					_form.LiteralEditTextBox.Select(_form.LiteralEditTextBox.TextLength + 1, 0);
//				}
//				else
//				{
//					_form.LiteralEditTextBox.Text = ((DataQueryLiteralOperand)_queryOperand).LiteralValue.ToString(CultureInfo.CurrentUICulture);
//					_form.LiteralEditTextBox.SelectAll();
//				}
//
//				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		override public void HighlightClick()
//		{
//			try
//			{
//				_form.CurrentPanelClearSelectedOperands();
//
//				if (((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked != null && (int)Tag == (int)((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked.Label.Tag)
//				{
//
//					StartEdit(Char.MinValue);
//
//					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//				}
//				else
//				{
//					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = _queryOperand;
//					_form.CurrentPanelSelectOperandRange((int)Tag, (int)Tag);
//				}
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		override public void DoubleClickHandler(object sender, EventArgs e)
//		{
//			try
//			{
//				StartEdit(Char.MinValue);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//	}
//
//	/// <summary>
//	/// Class that defines an Not label.
//	/// </summary>
//
//	public class DataQueryGradeLabel : BasicQueryLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public DataQueryGradeLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//
//		override public void StartEdit(char aKeyPressed)
//		{
//			try
//			{
//				_form.GradeEditTextBox.Tag = _queryOperand;
//				_form.GradeEditTextBox.Width = Math.Max(70, Width);
//				_form.GradeEditTextBox.Left = _form.CurrentPanel.Left + Left;
//				_form.GradeEditTextBox.Top = _form.CurrentPanel.Top + Top;
//				_form.GradeEditTextBox.BringToFront();
//				_form.GradeEditTextBox.Visible = true;
//				_form.GradeEditTextBox.Enabled = true;
//				_form.GradeEditTextBox.Focus();
//
//				if (aKeyPressed != Char.MinValue && (Char.IsLetterOrDigit(aKeyPressed) || aKeyPressed == '-' || aKeyPressed == '.'))
//				{
//					_form.GradeEditTextBox.Text = Convert.ToString(aKeyPressed, CultureInfo.CurrentUICulture);
//					_form.GradeEditTextBox.Select(_form.GradeEditTextBox.TextLength + 1, 0);
//				}
//				else
//				{
//					_form.GradeEditTextBox.Text = ((DataQueryGradeOperand)_queryOperand).GradeValue.ToString(CultureInfo.CurrentUICulture);
//					_form.GradeEditTextBox.SelectAll();
//				}
//
//				((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		override public void HighlightClick()
//		{
//			try
//			{
//				_form.CurrentPanelClearSelectedOperands();
//
//				if (((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked != null && (int)Tag == (int)((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked.Label.Tag)
//				{
//
//					StartEdit(Char.MinValue);
//
//					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = null;
//				}
//				else
//				{
//					((PanelTag)_form.CurrentPanel.Tag).LastOperandClicked = _queryOperand;
//					_form.CurrentPanelSelectOperandRange((int)Tag, (int)Tag);
//				}
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		override public void DoubleClickHandler(object sender, EventArgs e)
//		{
//			try
//			{
//				StartEdit(Char.MinValue);
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//	}
//
//	/// <summary>
//	/// Class that defines an Not label.
//	/// </summary>
//
//	public class DataQueryStatusLabel : BasicQueryLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public DataQueryStatusLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor, aText)
//		{
//		}
//
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//	}
//
//	/// <summary>
//	/// Abstract class that defines a spacer.
//	/// </summary>
//
//	public class DataQuerySpacerLabel : BasicQueryLabel
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		public DataQuerySpacerLabel(frmFilter aForm, FilterDefinition aFilterDef, QueryOperand aQueryOperand, Color aForeColor, string aText)
//			: base(aForm, aFilterDef, aQueryOperand, aForeColor)
//		{
//			int height;
//
//			Text = " ";
//			height = Height;
//			AutoSize = false;
//			Width = 3;
//			Height = height;
//		}
//
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//
//		override public void MouseUpHandler(object sender, MouseEventArgs e)
//		{
//		}
//	}
//	#endregion
//}
