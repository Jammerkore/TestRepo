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

//namespace MIDRetail.Windows
//{
//    /// <summary>
//    /// Summary description for Filter.
//    /// </summary>
//    public class frmFilterProperties : MIDFilterFormBase
//    {
//        #region Windows Form Designer generated code

//        //private Rectangle _dragBoxFromMouseDown;
//        private System.Windows.Forms.TabPage tpgAttributes;
//        private System.Windows.Forms.TabPage tpgData;
//        private System.Windows.Forms.Button btnSave;
//        private System.Windows.Forms.Button btnHelp;
//        private System.Windows.Forms.Button btnCancel;
//        private System.Windows.Forms.TabControl tabFilter;
//        private System.Windows.Forms.ContextMenu mnuLabelMenu;
//        private System.Windows.Forms.MenuItem mniDelete;
//        private System.Windows.Forms.Panel pnlAttrQuery;
//        private System.Windows.Forms.Button btnAttrOr;
//        private System.Windows.Forms.Button btnAttrAnd;
//        private System.Windows.Forms.Button btnAttrRParen;
//        private System.Windows.Forms.Button btnAttrLParen;
//        private System.Windows.Forms.Button btnDataOr;
//        private System.Windows.Forms.Button btnDataAnd;
//        private System.Windows.Forms.Button btnDataRParen;
//        private System.Windows.Forms.Button btnDataLParen;
//        private System.Windows.Forms.Panel pnlDataQuery;
//        private System.Windows.Forms.Button btnDataEqual;
//        private System.Windows.Forms.Button btnDataLess;
//        private System.Windows.Forms.Button btnDataGreater;
//        private System.Windows.Forms.Button btnDataLessEqual;
//        private System.Windows.Forms.Button btnDataGreaterEqual;
//        private System.Windows.Forms.Button btnDataNot;
//        private System.Windows.Forms.ListBox lstVariables;
//        private System.Windows.Forms.Label lblVariable;
//        private System.Windows.Forms.Label label1;
//        private System.Windows.Forms.Label lblVersion;
//        private System.Windows.Forms.Label label3;
//        private System.Windows.Forms.ListBox lstVersions;
//        private System.Windows.Forms.Button btnDataPctOf;
//        private System.Windows.Forms.Button btnDataPctChange;
//        private System.Windows.Forms.Button btnDataAny;
//        private System.Windows.Forms.Button btnDataAll;
//        private System.Windows.Forms.Button btnDataAverage;
//        private System.Windows.Forms.Button btnDataTotal;
//        private System.Windows.Forms.Label lblFilterName;
//        private System.Windows.Forms.Button btnDataChainDetail;
//        private System.Windows.Forms.Button btnDataStoreDetail;
//        private System.Windows.Forms.Button btnDataStoreTotal;
//        private System.Windows.Forms.Button btnDataStoreAverage;
//        private System.Windows.Forms.Button btnSaveAs;
//        private System.Windows.Forms.Button btnDataDate;
//        private System.Windows.Forms.Button btnDataCorresponding;
//        private System.Windows.Forms.Label label2;
//        private System.Windows.Forms.Label label4;
//        private System.Windows.Forms.GroupBox grpPlan;
//        private System.Windows.Forms.Label label6;
//        private System.Windows.Forms.GroupBox grpOperands;
//        private System.Windows.Forms.GroupBox grpQualifiers;
//        private System.Windows.Forms.Label label5;
//        private System.Windows.Forms.Label label8;
//        private System.Windows.Forms.Button btnDataLiteral;
//        private System.Windows.Forms.Button btnDataGrade;
//        private System.Windows.Forms.Label lblStatus;
//        private System.Windows.Forms.ListBox lstStatus;
//        private System.Windows.Forms.Button btnOK;
//        private System.Windows.Forms.TextBox txtFilterName;
//        private System.Windows.Forms.Button btnDataMerchandise;
//        private System.Windows.Forms.TextBox txtMerchandiseEdit;
//        private System.Windows.Forms.Button btnDataJoin;
//        private RadioButton rdoOwner;
//        private RadioButton rdoUser;
//        private RadioButton rdoGlobal;
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.Container components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        protected override void Dispose( bool disposing )
//        {
//            if( disposing )
//            {
//                if(components != null)
//                {
//                    components.Dispose();
//                }

//                this.tabFilter.SizeChanged -= new System.EventHandler(this.tabFilter_SizeChanged);
//                this.btnAttrOr.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnAttrOr.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnAttrOr.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnAttrOr.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnAttrOr.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnAttrAnd.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnAttrAnd.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnAttrAnd.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnAttrAnd.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnAttrAnd.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnAttrRParen.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnAttrRParen.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnAttrRParen.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnAttrRParen.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnAttrRParen.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnAttrLParen.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnAttrLParen.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnAttrLParen.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnAttrLParen.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnAttrLParen.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.pnlAttrQuery.Click -= new System.EventHandler(this.panel_Click);
//                this.pnlAttrQuery.DragEnter -= new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
//                this.pnlDataQuery.DragOver -= new System.Windows.Forms.DragEventHandler(this.panel_DragOver);
//                this.pnlAttrQuery.DragLeave -= new System.EventHandler(this.panel_DragLeave);
//                this.pnlAttrQuery.DragDrop -= new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
//                //Begin TT#211 - JScott - Variable, Version, and Status dropdowns do not align with corresponding text boxes
//                //this.tpgData.MouseEnter -= new System.EventHandler(this.tpgData_MouseEnter);
//                this.lstVariables.MouseLeave -= new System.EventHandler(this.toolList_MouseLeave);
//                this.lstVersions.MouseLeave -= new System.EventHandler(this.toolList_MouseLeave);
//                this.lstStatus.MouseLeave -= new System.EventHandler(this.toolList_MouseLeave);
//                //End TT#211 - JScott - Variable, Version, and Status dropdowns do not align with corresponding text boxes
//                this.btnDataLiteral.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataLiteral.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataLiteral.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataLiteral.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataLiteral.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataGrade.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataGrade.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataGrade.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataGrade.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataGrade.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataPctChange.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataPctChange.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataPctChange.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataPctChange.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataPctChange.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataPctOf.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataPctOf.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataPctOf.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataPctOf.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataPctOf.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataLess.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataLess.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataLess.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataLess.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataLess.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataAnd.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataAnd.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataAnd.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataAnd.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataAnd.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataGreaterEqual.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataGreaterEqual.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataGreaterEqual.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataGreaterEqual.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataGreaterEqual.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataGreater.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataGreater.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataGreater.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataGreater.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataGreater.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataRParen.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataRParen.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataRParen.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataRParen.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataRParen.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataLParen.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataLParen.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataLParen.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataLParen.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataLParen.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataNot.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataNot.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataNot.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataNot.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataNot.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataOr.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.toolButton_KeyPress);
//                this.btnDataOr.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataOr.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataOr.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataOr.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataOr.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataLessEqual.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataLessEqual.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataLessEqual.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataLessEqual.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataLessEqual.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataEqual.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataEqual.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataEqual.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataEqual.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataEqual.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.lblVariable.Click -= new System.EventHandler(this.toolLabel_Click);
//                this.lblVariable.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseUp);
//                this.lblVariable.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseMove);
//                this.lblVariable.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseDown);
//                this.lblVersion.Click -= new System.EventHandler(this.toolLabel_Click);
//                this.lblVersion.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseUp);
//                this.lblVersion.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseMove);
//                this.lblVersion.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseDown);
//                this.lblStatus.Click -= new System.EventHandler(this.toolLabel_Click);
//                this.lblStatus.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseUp);
//                this.lblStatus.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseMove);
//                this.lblStatus.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseDown);
//                this.btnDataStoreAverage.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataStoreAverage.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataStoreAverage.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataStoreAverage.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataStoreAverage.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataStoreTotal.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataStoreTotal.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataStoreTotal.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataStoreTotal.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataStoreTotal.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataChainDetail.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataChainDetail.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataChainDetail.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataChainDetail.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataChainDetail.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataStoreDetail.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataStoreDetail.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataStoreDetail.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataStoreDetail.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataStoreDetail.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataDate.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataDate.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataDate.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataDate.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataDate.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataMerchandise.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataMerchandise.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataMerchandise.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataMerchandise.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataMerchandise.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataAny.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataAny.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataAny.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataAny.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataAny.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataTotal.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataTotal.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataTotal.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataTotal.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataTotal.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataCorresponding.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataCorresponding.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataCorresponding.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataCorresponding.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataCorresponding.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataAverage.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataAverage.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataAverage.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataAverage.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataAverage.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.btnDataAll.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataAll.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataAll.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataAll.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataAll.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//                this.txtMerchandiseEdit.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtMerchandiseEdit_KeyPress);
//                this.txtMerchandiseEdit.Leave -= new System.EventHandler(this.txtMerchandiseEdit_Leave);
//                this.pnlDataQuery.Click -= new System.EventHandler(this.panel_Click);
//                this.pnlDataQuery.DragEnter -= new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
//                this.pnlDataQuery.DragLeave -= new System.EventHandler(this.panel_DragLeave);
//                //Begin TT#211 - JScott - Variable, Version, and Status dropdowns do not align with corresponding text boxes
//                //this.pnlDataQuery.MouseEnter -= new System.EventHandler(this.pnlDataQuery_MouseEnter);
//                //End TT#211 - JScott - Variable, Version, and Status dropdowns do not align with corresponding text boxes
//                this.pnlDataQuery.DragDrop -= new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
//                this.lstVersions.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseDown);
//                this.lstVersions.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseUp);
//                this.lstVersions.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseMove);
//                this.lstVariables.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseDown);
//                this.lstVariables.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseUp);
//                this.lstVariables.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseMove);
//                this.lstStatus.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseDown);
//                this.lstStatus.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseUp);
//                this.lstStatus.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolList_MouseMove);
//                this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
//                this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
//                this.mnuLabelMenu.Popup -= new System.EventHandler(this.mnuLabelMenu_Popup);
//                this.mniDelete.Click -= new System.EventHandler(this.mniDelete_Click);
//                this.btnSaveAs.Click -= new System.EventHandler(this.btnSaveAs_Click);
////Begin Track #5111 - JScott - Add additional filter functionality
//                this.btnDataJoin.Click -= new System.EventHandler(this.toolButton_Click);
//                this.btnDataJoin.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//                this.btnDataJoin.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//                this.btnDataJoin.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//                this.btnDataJoin.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
////End Track #5111 - JScott - Add additional filter functionality
//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                this.rdoUser.CheckedChanged -= new System.EventHandler(this.rdoUser_CheckedChanged);
//                this.rdoGlobal.CheckedChanged -= new System.EventHandler(this.rdoGlobal_CheckedChanged);
//                this.rdoOwner.CheckedChanged -= new System.EventHandler(this.rdoOwner_CheckedChanged);
//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                if(pnlAttrQuery.Tag != null) 
//                {
//                    foreach (QueryOperand operand in ((PanelTag)pnlAttrQuery.Tag).OperandArray) 
//                    {
//                        operand.Label.DragEnter -= new DragEventHandler(((BasicQueryLabel)operand.Label).DragEnterHandler);
//                        operand.Label.DragDrop -= new DragEventHandler(((BasicQueryLabel)operand.Label).DragDropHandler);
//                        operand.Label.DragLeave -= new EventHandler(((BasicQueryLabel)operand.Label).DragLeaveHandler);
//                        operand.Label.MouseDown -= new MouseEventHandler(((BasicQueryLabel)operand.Label).MouseDownHandler);
//                        operand.Label.MouseMove -= new MouseEventHandler(((BasicQueryLabel)operand.Label).MouseMoveHandler);
//                        operand.Label.MouseUp -= new MouseEventHandler(((BasicQueryLabel)operand.Label).MouseUpHandler);
//                        operand.Label.DoubleClick -= new EventHandler(((BasicQueryLabel)operand.Label).DoubleClickHandler);
//                        operand.Label.GiveFeedback -= new GiveFeedbackEventHandler(((BasicQueryLabel)operand.Label).GiveFeedbackHandler);
//                    }
//                }

//                if(pnlDataQuery.Tag != null) 
//                {
//                    foreach (QueryOperand operand in ((PanelTag)pnlDataQuery.Tag).OperandArray) 
//                    {
//                        operand.Label.DragEnter -= new DragEventHandler(((BasicQueryLabel)operand.Label).DragEnterHandler);
//                        operand.Label.DragDrop -= new DragEventHandler(((BasicQueryLabel)operand.Label).DragDropHandler);
//                        operand.Label.DragLeave -= new EventHandler(((BasicQueryLabel)operand.Label).DragLeaveHandler);
//                        operand.Label.MouseDown -= new MouseEventHandler(((BasicQueryLabel)operand.Label).MouseDownHandler);
//                        operand.Label.MouseMove -= new MouseEventHandler(((BasicQueryLabel)operand.Label).MouseMoveHandler);
//                        operand.Label.MouseUp -= new MouseEventHandler(((BasicQueryLabel)operand.Label).MouseUpHandler);
//                        operand.Label.DoubleClick -= new EventHandler(((BasicQueryLabel)operand.Label).DoubleClickHandler);
//                        operand.Label.GiveFeedback -= new GiveFeedbackEventHandler(((BasicQueryLabel)operand.Label).GiveFeedbackHandler);
//                    }
//                }
//            }
//            base.Dispose( disposing );
//        }

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.tabFilter = new System.Windows.Forms.TabControl();
//            this.tpgAttributes = new System.Windows.Forms.TabPage();
//            this.btnAttrOr = new System.Windows.Forms.Button();
//            this.btnAttrAnd = new System.Windows.Forms.Button();
//            this.btnAttrRParen = new System.Windows.Forms.Button();
//            this.btnAttrLParen = new System.Windows.Forms.Button();
//            this.pnlAttrQuery = new System.Windows.Forms.Panel();
//            this.tpgData = new System.Windows.Forms.TabPage();
//            this.lstStatus = new System.Windows.Forms.ListBox();
//            this.txtMerchandiseEdit = new System.Windows.Forms.TextBox();
//            this.label5 = new System.Windows.Forms.Label();
//            this.grpQualifiers = new System.Windows.Forms.GroupBox();
//            this.btnDataGrade = new System.Windows.Forms.Button();
//            this.label8 = new System.Windows.Forms.Label();
//            this.lblStatus = new System.Windows.Forms.Label();
//            this.btnDataLiteral = new System.Windows.Forms.Button();
//            this.btnDataPctChange = new System.Windows.Forms.Button();
//            this.btnDataPctOf = new System.Windows.Forms.Button();
//            this.grpOperands = new System.Windows.Forms.GroupBox();
//            this.btnDataLess = new System.Windows.Forms.Button();
//            this.btnDataAnd = new System.Windows.Forms.Button();
//            this.btnDataGreaterEqual = new System.Windows.Forms.Button();
//            this.btnDataGreater = new System.Windows.Forms.Button();
//            this.btnDataRParen = new System.Windows.Forms.Button();
//            this.btnDataLParen = new System.Windows.Forms.Button();
//            this.btnDataNot = new System.Windows.Forms.Button();
//            this.btnDataOr = new System.Windows.Forms.Button();
//            this.btnDataLessEqual = new System.Windows.Forms.Button();
//            this.btnDataEqual = new System.Windows.Forms.Button();
//            this.lstVariables = new System.Windows.Forms.ListBox();
//            this.lstVersions = new System.Windows.Forms.ListBox();
//            this.grpPlan = new System.Windows.Forms.GroupBox();
//            this.btnDataJoin = new System.Windows.Forms.Button();
//            this.btnDataMerchandise = new System.Windows.Forms.Button();
//            this.label6 = new System.Windows.Forms.Label();
//            this.lblVariable = new System.Windows.Forms.Label();
//            this.lblVersion = new System.Windows.Forms.Label();
//            this.label1 = new System.Windows.Forms.Label();
//            this.label3 = new System.Windows.Forms.Label();
//            this.btnDataStoreAverage = new System.Windows.Forms.Button();
//            this.label2 = new System.Windows.Forms.Label();
//            this.btnDataStoreTotal = new System.Windows.Forms.Button();
//            this.btnDataChainDetail = new System.Windows.Forms.Button();
//            this.btnDataStoreDetail = new System.Windows.Forms.Button();
//            this.btnDataDate = new System.Windows.Forms.Button();
//            this.btnDataAny = new System.Windows.Forms.Button();
//            this.btnDataTotal = new System.Windows.Forms.Button();
//            this.btnDataCorresponding = new System.Windows.Forms.Button();
//            this.label4 = new System.Windows.Forms.Label();
//            this.btnDataAverage = new System.Windows.Forms.Button();
//            this.btnDataAll = new System.Windows.Forms.Button();
//            this.pnlDataQuery = new System.Windows.Forms.Panel();
//            this.btnSave = new System.Windows.Forms.Button();
//            this.lblFilterName = new System.Windows.Forms.Label();
//            this.btnHelp = new System.Windows.Forms.Button();
//            this.btnCancel = new System.Windows.Forms.Button();
//            this.mnuLabelMenu = new System.Windows.Forms.ContextMenu();
//            this.mniDelete = new System.Windows.Forms.MenuItem();
//            this.btnSaveAs = new System.Windows.Forms.Button();
//            this.btnOK = new System.Windows.Forms.Button();
//            this.txtFilterName = new System.Windows.Forms.TextBox();
//            this.rdoOwner = new System.Windows.Forms.RadioButton();
//            this.rdoUser = new System.Windows.Forms.RadioButton();
//            this.rdoGlobal = new System.Windows.Forms.RadioButton();
//            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
//            this.tabFilter.SuspendLayout();
//            this.tpgAttributes.SuspendLayout();
//            this.tpgData.SuspendLayout();
//            this.grpQualifiers.SuspendLayout();
//            this.grpOperands.SuspendLayout();
//            this.grpPlan.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // txtGradeEdit
//            // 
//            this.txtGradeEdit.Location = new System.Drawing.Point(624, 186);
//            // 
//            // txtLiteralEdit
//            // 
//            this.txtLiteralEdit.Location = new System.Drawing.Point(624, 212);
//            // 
//            // utmMain
//            // 
//            this.utmMain.MenuSettings.ForceSerialization = true;
//            this.utmMain.ToolbarSettings.ForceSerialization = true;
//            // 
//            // tabFilter
//            // 
//            this.tabFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
//                        | System.Windows.Forms.AnchorStyles.Left)
//                        | System.Windows.Forms.AnchorStyles.Right)));
//            this.tabFilter.Controls.Add(this.tpgAttributes);
//            this.tabFilter.Controls.Add(this.tpgData);
//            this.tabFilter.Location = new System.Drawing.Point(16, 56);
//            this.tabFilter.Name = "tabFilter";
//            this.tabFilter.SelectedIndex = 0;
//            this.tabFilter.Size = new System.Drawing.Size(712, 490);
//            this.tabFilter.TabIndex = 7;
//            this.tabFilter.SelectedIndexChanged += new System.EventHandler(this.tabFilter_SelectedIndexChanged);
//            this.tabFilter.SizeChanged += new System.EventHandler(this.tabFilter_SizeChanged);
//            // 
//            // tpgAttributes
//            // 
//            this.tpgAttributes.Controls.Add(this.btnAttrOr);
//            this.tpgAttributes.Controls.Add(this.btnAttrAnd);
//            this.tpgAttributes.Controls.Add(this.btnAttrRParen);
//            this.tpgAttributes.Controls.Add(this.btnAttrLParen);
//            this.tpgAttributes.Controls.Add(this.pnlAttrQuery);
//            this.tpgAttributes.Location = new System.Drawing.Point(4, 22);
//            this.tpgAttributes.Name = "tpgAttributes";
//            this.tpgAttributes.Size = new System.Drawing.Size(704, 464);
//            this.tpgAttributes.TabIndex = 0;
//            this.tpgAttributes.Text = "Attributes";
//            // 
//            // btnAttrOr
//            // 
//            this.btnAttrOr.Location = new System.Drawing.Point(16, 16);
//            this.btnAttrOr.Name = "btnAttrOr";
//            this.btnAttrOr.Size = new System.Drawing.Size(40, 24);
//            this.btnAttrOr.TabIndex = 8;
//            this.btnAttrOr.Text = "OR";
//            this.btnAttrOr.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnAttrOr.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnAttrOr.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnAttrOr.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnAttrOr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnAttrAnd
//            // 
//            this.btnAttrAnd.Location = new System.Drawing.Point(56, 16);
//            this.btnAttrAnd.Name = "btnAttrAnd";
//            this.btnAttrAnd.Size = new System.Drawing.Size(40, 24);
//            this.btnAttrAnd.TabIndex = 9;
//            this.btnAttrAnd.Text = "AND";
//            this.btnAttrAnd.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnAttrAnd.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnAttrAnd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnAttrAnd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnAttrAnd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnAttrRParen
//            // 
//            this.btnAttrRParen.Location = new System.Drawing.Point(136, 16);
//            this.btnAttrRParen.Name = "btnAttrRParen";
//            this.btnAttrRParen.Size = new System.Drawing.Size(40, 24);
//            this.btnAttrRParen.TabIndex = 11;
//            this.btnAttrRParen.Text = ")";
//            this.btnAttrRParen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnAttrRParen.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnAttrRParen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnAttrRParen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnAttrRParen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnAttrLParen
//            // 
//            this.btnAttrLParen.Location = new System.Drawing.Point(96, 16);
//            this.btnAttrLParen.Name = "btnAttrLParen";
//            this.btnAttrLParen.Size = new System.Drawing.Size(40, 24);
//            this.btnAttrLParen.TabIndex = 10;
//            this.btnAttrLParen.Text = "(";
//            this.btnAttrLParen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnAttrLParen.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnAttrLParen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnAttrLParen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnAttrLParen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // pnlAttrQuery
//            // 
//            this.pnlAttrQuery.AllowDrop = true;
//            this.pnlAttrQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
//                        | System.Windows.Forms.AnchorStyles.Left)
//                        | System.Windows.Forms.AnchorStyles.Right)));
//            this.pnlAttrQuery.AutoScroll = true;
//            this.pnlAttrQuery.BackColor = System.Drawing.SystemColors.ControlLightLight;
//            this.pnlAttrQuery.Location = new System.Drawing.Point(16, 48);
//            this.pnlAttrQuery.Name = "pnlAttrQuery";
//            this.pnlAttrQuery.Size = new System.Drawing.Size(672, 410);
//            this.pnlAttrQuery.TabIndex = 0;
//            this.pnlAttrQuery.DragOver += new System.Windows.Forms.DragEventHandler(this.panel_DragOver);
//            this.pnlAttrQuery.Click += new System.EventHandler(this.panel_Click);
//            this.pnlAttrQuery.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
//            this.pnlAttrQuery.DragLeave += new System.EventHandler(this.panel_DragLeave);
//            this.pnlAttrQuery.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
//            // 
//            // tpgData
//            // 
//            this.tpgData.Controls.Add(this.lstStatus);
//            this.tpgData.Controls.Add(this.txtMerchandiseEdit);
//            this.tpgData.Controls.Add(this.txtLiteralEdit);
//            this.tpgData.Controls.Add(this.txtGradeEdit);
//            this.tpgData.Controls.Add(this.label5);
//            this.tpgData.Controls.Add(this.grpQualifiers);
//            this.tpgData.Controls.Add(this.grpOperands);
//            this.tpgData.Controls.Add(this.lstVariables);
//            this.tpgData.Controls.Add(this.lstVersions);
//            this.tpgData.Controls.Add(this.grpPlan);
//            this.tpgData.Controls.Add(this.pnlDataQuery);
//            this.tpgData.Location = new System.Drawing.Point(4, 22);
//            this.tpgData.Name = "tpgData";
//            this.tpgData.Size = new System.Drawing.Size(704, 464);
//            this.tpgData.TabIndex = 1;
//            this.tpgData.Text = "Data";
//            this.tpgData.Controls.SetChildIndex(this.pnlDataQuery, 0);
//            this.tpgData.Controls.SetChildIndex(this.grpPlan, 0);
//            this.tpgData.Controls.SetChildIndex(this.lstVersions, 0);
//            this.tpgData.Controls.SetChildIndex(this.lstVariables, 0);
//            this.tpgData.Controls.SetChildIndex(this.grpOperands, 0);
//            this.tpgData.Controls.SetChildIndex(this.grpQualifiers, 0);
//            this.tpgData.Controls.SetChildIndex(this.label5, 0);
//            this.tpgData.Controls.SetChildIndex(this.txtGradeEdit, 0);
//            this.tpgData.Controls.SetChildIndex(this.txtLiteralEdit, 0);
//            this.tpgData.Controls.SetChildIndex(this.txtMerchandiseEdit, 0);
//            this.tpgData.Controls.SetChildIndex(this.lstStatus, 0);
//            // 
//            // lstStatus
//            // 
//            this.lstStatus.Location = new System.Drawing.Point(624, 264);
//            this.lstStatus.Name = "lstStatus";
//            this.lstStatus.Size = new System.Drawing.Size(160, 69);
//            this.lstStatus.TabIndex = 53;
//            this.lstStatus.Visible = false;
//            this.lstStatus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseUp);
//            this.lstStatus.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseMove);
//            this.lstStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseDown);
//            this.lstStatus.MouseLeave += new System.EventHandler(this.toolList_MouseLeave);
//            // 
//            // txtMerchandiseEdit
//            // 
//            this.txtMerchandiseEdit.AcceptsReturn = true;
//            this.txtMerchandiseEdit.AcceptsTab = true;
//            this.txtMerchandiseEdit.BackColor = System.Drawing.SystemColors.Window;
//            this.txtMerchandiseEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.txtMerchandiseEdit.Enabled = false;
//            this.txtMerchandiseEdit.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.txtMerchandiseEdit.Location = new System.Drawing.Point(624, 160);
//            this.txtMerchandiseEdit.Multiline = true;
//            this.txtMerchandiseEdit.Name = "txtMerchandiseEdit";
//            this.txtMerchandiseEdit.Size = new System.Drawing.Size(64, 20);
//            this.txtMerchandiseEdit.TabIndex = 55;
//            this.txtMerchandiseEdit.TabStop = false;
//            this.txtMerchandiseEdit.Visible = false;
//            this.txtMerchandiseEdit.WordWrap = false;
//            this.txtMerchandiseEdit.Leave += new System.EventHandler(this.txtMerchandiseEdit_Leave);
//            this.txtMerchandiseEdit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMerchandiseEdit_KeyPress);
//            // 
//            // label5
//            // 
//            this.label5.Location = new System.Drawing.Point(16, 264);
//            this.label5.Name = "label5";
//            this.label5.Size = new System.Drawing.Size(72, 16);
//            this.label5.TabIndex = 51;
//            this.label5.Text = "Filter Builder";
//            // 
//            // grpQualifiers
//            // 
//            this.grpQualifiers.Controls.Add(this.btnDataGrade);
//            this.grpQualifiers.Controls.Add(this.label8);
//            this.grpQualifiers.Controls.Add(this.lblStatus);
//            this.grpQualifiers.Controls.Add(this.btnDataLiteral);
//            this.grpQualifiers.Controls.Add(this.btnDataPctChange);
//            this.grpQualifiers.Controls.Add(this.btnDataPctOf);
//            this.grpQualifiers.Location = new System.Drawing.Point(344, 160);
//            this.grpQualifiers.Name = "grpQualifiers";
//            this.grpQualifiers.Size = new System.Drawing.Size(264, 104);
//            this.grpQualifiers.TabIndex = 50;
//            this.grpQualifiers.TabStop = false;
//            this.grpQualifiers.Text = "Qualifiers";
//            // 
//            // btnDataGrade
//            // 
//            this.btnDataGrade.Location = new System.Drawing.Point(96, 64);
//            this.btnDataGrade.Name = "btnDataGrade";
//            this.btnDataGrade.Size = new System.Drawing.Size(72, 24);
//            this.btnDataGrade.TabIndex = 40;
//            this.btnDataGrade.Text = "Grade";
//            this.btnDataGrade.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataGrade.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataGrade.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataGrade.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataGrade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // label8
//            // 
//            this.label8.AutoSize = true;
//            this.label8.Location = new System.Drawing.Point(184, 48);
//            this.label8.Name = "label8";
//            this.label8.Size = new System.Drawing.Size(40, 13);
//            this.label8.TabIndex = 39;
//            this.label8.Text = "Status:";
//            // 
//            // lblStatus
//            // 
//            this.lblStatus.BackColor = System.Drawing.SystemColors.ControlLightLight;
//            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.lblStatus.Location = new System.Drawing.Point(184, 66);
//            this.lblStatus.Name = "lblStatus";
//            this.lblStatus.Size = new System.Drawing.Size(64, 20);
//            this.lblStatus.TabIndex = 37;
//            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//            this.lblStatus.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseMove);
//            this.lblStatus.Click += new System.EventHandler(this.toolLabel_Click);
//            this.lblStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseDown);
//            this.lblStatus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseUp);
//            // 
//            // btnDataLiteral
//            // 
//            this.btnDataLiteral.Location = new System.Drawing.Point(16, 64);
//            this.btnDataLiteral.Name = "btnDataLiteral";
//            this.btnDataLiteral.Size = new System.Drawing.Size(72, 24);
//            this.btnDataLiteral.TabIndex = 24;
//            this.btnDataLiteral.Text = "Quantity";
//            this.btnDataLiteral.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataLiteral.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataLiteral.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataLiteral.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataLiteral.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataPctChange
//            // 
//            this.btnDataPctChange.Location = new System.Drawing.Point(96, 24);
//            this.btnDataPctChange.Name = "btnDataPctChange";
//            this.btnDataPctChange.Size = new System.Drawing.Size(72, 24);
//            this.btnDataPctChange.TabIndex = 23;
//            this.btnDataPctChange.Text = "% Change";
//            this.btnDataPctChange.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataPctChange.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataPctChange.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataPctChange.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataPctChange.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataPctOf
//            // 
//            this.btnDataPctOf.Location = new System.Drawing.Point(16, 24);
//            this.btnDataPctOf.Name = "btnDataPctOf";
//            this.btnDataPctOf.Size = new System.Drawing.Size(72, 24);
//            this.btnDataPctOf.TabIndex = 22;
//            this.btnDataPctOf.Text = "% Of";
//            this.btnDataPctOf.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataPctOf.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataPctOf.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataPctOf.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataPctOf.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // grpOperands
//            // 
//            this.grpOperands.Controls.Add(this.btnDataLess);
//            this.grpOperands.Controls.Add(this.btnDataAnd);
//            this.grpOperands.Controls.Add(this.btnDataGreaterEqual);
//            this.grpOperands.Controls.Add(this.btnDataGreater);
//            this.grpOperands.Controls.Add(this.btnDataRParen);
//            this.grpOperands.Controls.Add(this.btnDataLParen);
//            this.grpOperands.Controls.Add(this.btnDataNot);
//            this.grpOperands.Controls.Add(this.btnDataOr);
//            this.grpOperands.Controls.Add(this.btnDataLessEqual);
//            this.grpOperands.Controls.Add(this.btnDataEqual);
//            this.grpOperands.Location = new System.Drawing.Point(16, 160);
//            this.grpOperands.Name = "grpOperands";
//            this.grpOperands.Size = new System.Drawing.Size(312, 104);
//            this.grpOperands.TabIndex = 49;
//            this.grpOperands.TabStop = false;
//            this.grpOperands.Text = "Operands";
//            // 
//            // btnDataLess
//            // 
//            this.btnDataLess.Location = new System.Drawing.Point(136, 24);
//            this.btnDataLess.Name = "btnDataLess";
//            this.btnDataLess.Size = new System.Drawing.Size(40, 24);
//            this.btnDataLess.TabIndex = 17;
//            this.btnDataLess.Text = "<";
//            this.btnDataLess.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataLess.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataLess.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataLess.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataLess.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataAnd
//            // 
//            this.btnDataAnd.Location = new System.Drawing.Point(136, 64);
//            this.btnDataAnd.Name = "btnDataAnd";
//            this.btnDataAnd.Size = new System.Drawing.Size(40, 24);
//            this.btnDataAnd.TabIndex = 13;
//            this.btnDataAnd.Text = "AND";
//            this.btnDataAnd.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataAnd.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataAnd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataAnd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataAnd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataGreaterEqual
//            // 
//            this.btnDataGreaterEqual.Location = new System.Drawing.Point(256, 24);
//            this.btnDataGreaterEqual.Name = "btnDataGreaterEqual";
//            this.btnDataGreaterEqual.Size = new System.Drawing.Size(40, 24);
//            this.btnDataGreaterEqual.TabIndex = 20;
//            this.btnDataGreaterEqual.Text = ">=";
//            this.btnDataGreaterEqual.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataGreaterEqual.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataGreaterEqual.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataGreaterEqual.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataGreaterEqual.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataGreater
//            // 
//            this.btnDataGreater.Location = new System.Drawing.Point(176, 24);
//            this.btnDataGreater.Name = "btnDataGreater";
//            this.btnDataGreater.Size = new System.Drawing.Size(40, 24);
//            this.btnDataGreater.TabIndex = 18;
//            this.btnDataGreater.Text = ">";
//            this.btnDataGreater.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataGreater.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataGreater.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataGreater.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataGreater.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataRParen
//            // 
//            this.btnDataRParen.Location = new System.Drawing.Point(56, 24);
//            this.btnDataRParen.Name = "btnDataRParen";
//            this.btnDataRParen.Size = new System.Drawing.Size(40, 24);
//            this.btnDataRParen.TabIndex = 15;
//            this.btnDataRParen.Text = ")";
//            this.btnDataRParen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataRParen.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataRParen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataRParen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataRParen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataLParen
//            // 
//            this.btnDataLParen.Location = new System.Drawing.Point(16, 24);
//            this.btnDataLParen.Name = "btnDataLParen";
//            this.btnDataLParen.Size = new System.Drawing.Size(40, 24);
//            this.btnDataLParen.TabIndex = 14;
//            this.btnDataLParen.Text = "(";
//            this.btnDataLParen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataLParen.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataLParen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataLParen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataLParen.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataNot
//            // 
//            this.btnDataNot.Location = new System.Drawing.Point(176, 64);
//            this.btnDataNot.Name = "btnDataNot";
//            this.btnDataNot.Size = new System.Drawing.Size(40, 24);
//            this.btnDataNot.TabIndex = 21;
//            this.btnDataNot.Text = "NOT";
//            this.btnDataNot.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataNot.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataNot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataNot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataNot.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataOr
//            // 
//            this.btnDataOr.Location = new System.Drawing.Point(96, 64);
//            this.btnDataOr.Name = "btnDataOr";
//            this.btnDataOr.Size = new System.Drawing.Size(40, 24);
//            this.btnDataOr.TabIndex = 12;
//            this.btnDataOr.Text = "OR";
//            this.btnDataOr.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataOr.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataOr.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataOr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolButton_KeyPress);
//            this.btnDataOr.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataOr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataLessEqual
//            // 
//            this.btnDataLessEqual.Location = new System.Drawing.Point(216, 24);
//            this.btnDataLessEqual.Name = "btnDataLessEqual";
//            this.btnDataLessEqual.Size = new System.Drawing.Size(40, 24);
//            this.btnDataLessEqual.TabIndex = 19;
//            this.btnDataLessEqual.Text = "<=";
//            this.btnDataLessEqual.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataLessEqual.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataLessEqual.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataLessEqual.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataLessEqual.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataEqual
//            // 
//            this.btnDataEqual.Location = new System.Drawing.Point(96, 24);
//            this.btnDataEqual.Name = "btnDataEqual";
//            this.btnDataEqual.Size = new System.Drawing.Size(40, 24);
//            this.btnDataEqual.TabIndex = 16;
//            this.btnDataEqual.Text = "=";
//            this.btnDataEqual.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataEqual.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataEqual.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataEqual.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataEqual.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // lstVariables
//            // 
//            this.lstVariables.Location = new System.Drawing.Point(647, 238);
//            this.lstVariables.Name = "lstVariables";
//            this.lstVariables.Size = new System.Drawing.Size(160, 160);
//            this.lstVariables.TabIndex = 25;
//            this.lstVariables.Visible = false;
//            this.lstVariables.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseUp);
//            this.lstVariables.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseMove);
//            this.lstVariables.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseDown);
//            this.lstVariables.MouseLeave += new System.EventHandler(this.toolList_MouseLeave);
//            // 
//            // lstVersions
//            // 
//            this.lstVersions.Location = new System.Drawing.Point(637, 250);
//            this.lstVersions.Name = "lstVersions";
//            this.lstVersions.Size = new System.Drawing.Size(160, 160);
//            this.lstVersions.TabIndex = 30;
//            this.lstVersions.Visible = false;
//            this.lstVersions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseUp);
//            this.lstVersions.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseMove);
//            this.lstVersions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolList_MouseDown);
//            this.lstVersions.MouseLeave += new System.EventHandler(this.toolList_MouseLeave);
//            // 
//            // grpPlan
//            // 
//            this.grpPlan.Controls.Add(this.btnDataJoin);
//            this.grpPlan.Controls.Add(this.btnDataMerchandise);
//            this.grpPlan.Controls.Add(this.label6);
//            this.grpPlan.Controls.Add(this.lblVariable);
//            this.grpPlan.Controls.Add(this.lblVersion);
//            this.grpPlan.Controls.Add(this.label1);
//            this.grpPlan.Controls.Add(this.label3);
//            this.grpPlan.Controls.Add(this.btnDataStoreAverage);
//            this.grpPlan.Controls.Add(this.label2);
//            this.grpPlan.Controls.Add(this.btnDataStoreTotal);
//            this.grpPlan.Controls.Add(this.btnDataChainDetail);
//            this.grpPlan.Controls.Add(this.btnDataStoreDetail);
//            this.grpPlan.Controls.Add(this.btnDataDate);
//            this.grpPlan.Controls.Add(this.btnDataAny);
//            this.grpPlan.Controls.Add(this.btnDataTotal);
//            this.grpPlan.Controls.Add(this.btnDataCorresponding);
//            this.grpPlan.Controls.Add(this.label4);
//            this.grpPlan.Controls.Add(this.btnDataAverage);
//            this.grpPlan.Controls.Add(this.btnDataAll);
//            this.grpPlan.Location = new System.Drawing.Point(16, 8);
//            this.grpPlan.Name = "grpPlan";
//            this.grpPlan.Size = new System.Drawing.Size(672, 144);
//            this.grpPlan.TabIndex = 48;
//            this.grpPlan.TabStop = false;
//            this.grpPlan.Text = "Select and drop into Filter Builder below:";
//            // 
//            // btnDataJoin
//            // 
//            this.btnDataJoin.Location = new System.Drawing.Point(232, 104);
//            this.btnDataJoin.Name = "btnDataJoin";
//            this.btnDataJoin.Size = new System.Drawing.Size(40, 24);
//            this.btnDataJoin.TabIndex = 49;
//            this.btnDataJoin.Text = "Join";
//            this.btnDataJoin.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataJoin.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataJoin.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataJoin.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataJoin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataMerchandise
//            // 
//            this.btnDataMerchandise.Location = new System.Drawing.Point(320, 64);
//            this.btnDataMerchandise.Name = "btnDataMerchandise";
//            this.btnDataMerchandise.Size = new System.Drawing.Size(80, 24);
//            this.btnDataMerchandise.TabIndex = 48;
//            this.btnDataMerchandise.Text = "Merchandise";
//            this.btnDataMerchandise.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataMerchandise.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataMerchandise.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataMerchandise.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataMerchandise.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // label6
//            // 
//            this.label6.Location = new System.Drawing.Point(408, 68);
//            this.label6.Name = "label6";
//            this.label6.Size = new System.Drawing.Size(256, 16);
//            this.label6.TabIndex = 47;
//            this.label6.Text = "(or drop Merchandise from Merchandise Explorer)";
//            // 
//            // lblVariable
//            // 
//            this.lblVariable.BackColor = System.Drawing.SystemColors.ControlLightLight;
//            this.lblVariable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.lblVariable.Location = new System.Drawing.Point(64, 26);
//            this.lblVariable.Name = "lblVariable";
//            this.lblVariable.Size = new System.Drawing.Size(152, 20);
//            this.lblVariable.TabIndex = 35;
//            this.lblVariable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//            this.lblVariable.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseMove);
//            this.lblVariable.Click += new System.EventHandler(this.toolLabel_Click);
//            this.lblVariable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseDown);
//            this.lblVariable.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseUp);
//            // 
//            // lblVersion
//            // 
//            this.lblVersion.BackColor = System.Drawing.SystemColors.ControlLightLight;
//            this.lblVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.lblVersion.Location = new System.Drawing.Point(64, 66);
//            this.lblVersion.Name = "lblVersion";
//            this.lblVersion.Size = new System.Drawing.Size(152, 20);
//            this.lblVersion.TabIndex = 36;
//            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//            this.lblVersion.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseMove);
//            this.lblVersion.Click += new System.EventHandler(this.toolLabel_Click);
//            this.lblVersion.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseDown);
//            this.lblVersion.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolLabel_MouseUp);
//            // 
//            // label1
//            // 
//            this.label1.AutoSize = true;
//            this.label1.Location = new System.Drawing.Point(16, 28);
//            this.label1.Name = "label1";
//            this.label1.Size = new System.Drawing.Size(48, 13);
//            this.label1.TabIndex = 27;
//            this.label1.Text = "Variable:";
//            // 
//            // label3
//            // 
//            this.label3.AutoSize = true;
//            this.label3.Location = new System.Drawing.Point(16, 68);
//            this.label3.Name = "label3";
//            this.label3.Size = new System.Drawing.Size(45, 13);
//            this.label3.TabIndex = 29;
//            this.label3.Text = "Version:";
//            // 
//            // btnDataStoreAverage
//            // 
//            this.btnDataStoreAverage.Location = new System.Drawing.Point(480, 24);
//            this.btnDataStoreAverage.Name = "btnDataStoreAverage";
//            this.btnDataStoreAverage.Size = new System.Drawing.Size(88, 24);
//            this.btnDataStoreAverage.TabIndex = 27;
//            this.btnDataStoreAverage.Text = "Store Average";
//            this.btnDataStoreAverage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataStoreAverage.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataStoreAverage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataStoreAverage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataStoreAverage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // label2
//            // 
//            this.label2.AutoSize = true;
//            this.label2.Location = new System.Drawing.Point(232, 28);
//            this.label2.Name = "label2";
//            this.label2.Size = new System.Drawing.Size(64, 13);
//            this.label2.TabIndex = 45;
//            this.label2.Text = "Value Type:";
//            // 
//            // btnDataStoreTotal
//            // 
//            this.btnDataStoreTotal.Location = new System.Drawing.Point(392, 24);
//            this.btnDataStoreTotal.Name = "btnDataStoreTotal";
//            this.btnDataStoreTotal.Size = new System.Drawing.Size(88, 24);
//            this.btnDataStoreTotal.TabIndex = 26;
//            this.btnDataStoreTotal.Text = "Store Total";
//            this.btnDataStoreTotal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataStoreTotal.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataStoreTotal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataStoreTotal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataStoreTotal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataChainDetail
//            // 
//            this.btnDataChainDetail.Location = new System.Drawing.Point(568, 24);
//            this.btnDataChainDetail.Name = "btnDataChainDetail";
//            this.btnDataChainDetail.Size = new System.Drawing.Size(88, 24);
//            this.btnDataChainDetail.TabIndex = 28;
//            this.btnDataChainDetail.Text = "Chain Detail";
//            this.btnDataChainDetail.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataChainDetail.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataChainDetail.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataChainDetail.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataChainDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataStoreDetail
//            // 
//            this.btnDataStoreDetail.BackColor = System.Drawing.SystemColors.Control;
//            this.btnDataStoreDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.btnDataStoreDetail.Location = new System.Drawing.Point(304, 24);
//            this.btnDataStoreDetail.Name = "btnDataStoreDetail";
//            this.btnDataStoreDetail.Size = new System.Drawing.Size(88, 24);
//            this.btnDataStoreDetail.TabIndex = 25;
//            this.btnDataStoreDetail.Text = "Store Detail";
//            this.btnDataStoreDetail.UseVisualStyleBackColor = false;
//            this.btnDataStoreDetail.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataStoreDetail.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataStoreDetail.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataStoreDetail.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataStoreDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataDate
//            // 
//            this.btnDataDate.Location = new System.Drawing.Point(232, 64);
//            this.btnDataDate.Name = "btnDataDate";
//            this.btnDataDate.Size = new System.Drawing.Size(80, 24);
//            this.btnDataDate.TabIndex = 34;
//            this.btnDataDate.Text = "DateRange";
//            this.btnDataDate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataDate.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataDate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataAny
//            // 
//            this.btnDataAny.BackColor = System.Drawing.SystemColors.Control;
//            this.btnDataAny.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.btnDataAny.Location = new System.Drawing.Point(112, 104);
//            this.btnDataAny.Name = "btnDataAny";
//            this.btnDataAny.Size = new System.Drawing.Size(40, 24);
//            this.btnDataAny.TabIndex = 29;
//            this.btnDataAny.Text = "Any";
//            this.btnDataAny.UseVisualStyleBackColor = false;
//            this.btnDataAny.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataAny.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataAny.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataAny.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataAny.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataTotal
//            // 
//            this.btnDataTotal.Location = new System.Drawing.Point(272, 104);
//            this.btnDataTotal.Name = "btnDataTotal";
//            this.btnDataTotal.Size = new System.Drawing.Size(40, 24);
//            this.btnDataTotal.TabIndex = 32;
//            this.btnDataTotal.Text = "Tot";
//            this.btnDataTotal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataTotal.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataTotal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataTotal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataTotal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataCorresponding
//            // 
//            this.btnDataCorresponding.Location = new System.Drawing.Point(312, 104);
//            this.btnDataCorresponding.Name = "btnDataCorresponding";
//            this.btnDataCorresponding.Size = new System.Drawing.Size(40, 24);
//            this.btnDataCorresponding.TabIndex = 33;
//            this.btnDataCorresponding.Text = "Corr";
//            this.btnDataCorresponding.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataCorresponding.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataCorresponding.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataCorresponding.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataCorresponding.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // label4
//            // 
//            this.label4.AutoSize = true;
//            this.label4.Location = new System.Drawing.Point(16, 108);
//            this.label4.Name = "label4";
//            this.label4.Size = new System.Drawing.Size(91, 13);
//            this.label4.TabIndex = 46;
//            this.label4.Text = "Time Comparison:";
//            // 
//            // btnDataAverage
//            // 
//            this.btnDataAverage.Location = new System.Drawing.Point(192, 104);
//            this.btnDataAverage.Name = "btnDataAverage";
//            this.btnDataAverage.Size = new System.Drawing.Size(40, 24);
//            this.btnDataAverage.TabIndex = 31;
//            this.btnDataAverage.Text = "Avg";
//            this.btnDataAverage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataAverage.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataAverage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataAverage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataAverage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // btnDataAll
//            // 
//            this.btnDataAll.Location = new System.Drawing.Point(152, 104);
//            this.btnDataAll.Name = "btnDataAll";
//            this.btnDataAll.Size = new System.Drawing.Size(40, 24);
//            this.btnDataAll.TabIndex = 30;
//            this.btnDataAll.Text = "All";
//            this.btnDataAll.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseMove);
//            this.btnDataAll.Click += new System.EventHandler(this.toolButton_Click);
//            this.btnDataAll.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseDown);
//            this.btnDataAll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolButton_MouseUp);
//            this.btnDataAll.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolButton_KeyDown);
//            // 
//            // pnlDataQuery
//            // 
//            this.pnlDataQuery.AllowDrop = true;
//            this.pnlDataQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
//                        | System.Windows.Forms.AnchorStyles.Left)
//                        | System.Windows.Forms.AnchorStyles.Right)));
//            this.pnlDataQuery.AutoScroll = true;
//            this.pnlDataQuery.BackColor = System.Drawing.SystemColors.Window;
//            this.pnlDataQuery.Location = new System.Drawing.Point(16, 280);
//            this.pnlDataQuery.Name = "pnlDataQuery";
//            this.pnlDataQuery.Size = new System.Drawing.Size(672, 170);
//            this.pnlDataQuery.TabIndex = 16;
//            this.pnlDataQuery.DragOver += new System.Windows.Forms.DragEventHandler(this.panel_DragOver);
//            this.pnlDataQuery.Click += new System.EventHandler(this.panel_Click);
//            this.pnlDataQuery.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel_DragDrop);
//            this.pnlDataQuery.DragLeave += new System.EventHandler(this.panel_DragLeave);
//            this.pnlDataQuery.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel_DragEnter);
//            // 
//            // btnSave
//            // 
//            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnSave.Location = new System.Drawing.Point(384, 562);
//            this.btnSave.Name = "btnSave";
//            this.btnSave.Size = new System.Drawing.Size(80, 24);
//            this.btnSave.TabIndex = 2;
//            this.btnSave.Text = "Save";
//            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
//            // 
//            // lblFilterName
//            // 
//            this.lblFilterName.Location = new System.Drawing.Point(12, 19);
//            this.lblFilterName.Name = "lblFilterName";
//            this.lblFilterName.Size = new System.Drawing.Size(64, 23);
//            this.lblFilterName.TabIndex = 27;
//            this.lblFilterName.Text = "Filter Name";
//            this.lblFilterName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//            // 
//            // btnHelp
//            // 
//            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
//            this.btnHelp.Location = new System.Drawing.Point(16, 562);
//            this.btnHelp.Name = "btnHelp";
//            this.btnHelp.Size = new System.Drawing.Size(24, 23);
//            this.btnHelp.TabIndex = 12;
//            this.btnHelp.Text = "?";
//            // 
//            // btnCancel
//            // 
//            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnCancel.Location = new System.Drawing.Point(648, 562);
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.Size = new System.Drawing.Size(80, 24);
//            this.btnCancel.TabIndex = 6;
//            this.btnCancel.Text = "Cancel";
//            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
//            // 
//            // mnuLabelMenu
//            // 
//            this.mnuLabelMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
//            this.mniDelete});
//            this.mnuLabelMenu.Popup += new System.EventHandler(this.mnuLabelMenu_Popup);
//            // 
//            // mniDelete
//            // 
//            this.mniDelete.Index = 0;
//            this.mniDelete.Text = "Delete";
//            this.mniDelete.Click += new System.EventHandler(this.mniDelete_Click);
//            // 
//            // btnSaveAs
//            // 
//            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnSaveAs.Location = new System.Drawing.Point(472, 562);
//            this.btnSaveAs.Name = "btnSaveAs";
//            this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
//            this.btnSaveAs.TabIndex = 3;
//            this.btnSaveAs.Text = "Save As";
//            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
//            // 
//            // btnOK
//            // 
//            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnOK.Location = new System.Drawing.Point(560, 562);
//            this.btnOK.Name = "btnOK";
//            this.btnOK.Size = new System.Drawing.Size(80, 24);
//            this.btnOK.TabIndex = 28;
//            this.btnOK.Text = "OK";
//            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
//            // 
//            // txtFilterName
//            // 
//            this.txtFilterName.Location = new System.Drawing.Point(84, 19);
//            this.txtFilterName.Name = "txtFilterName";
//            this.txtFilterName.Size = new System.Drawing.Size(184, 20);
//            this.txtFilterName.TabIndex = 95;
//            this.txtFilterName.TextChanged += new System.EventHandler(this.txtFilterName_TextChanged);
//            // 
//            // rdoOwner
//            // 
//            this.rdoOwner.Location = new System.Drawing.Point(395, 20);
//            this.rdoOwner.Name = "rdoOwner";
//            this.rdoOwner.Size = new System.Drawing.Size(279, 16);
//            this.rdoOwner.TabIndex = 102;
//            this.rdoOwner.Text = "Owner ({1})";
//            this.rdoOwner.CheckedChanged += new System.EventHandler(this.rdoOwner_CheckedChanged);
//            // 
//            // rdoUser
//            // 
//            this.rdoUser.Location = new System.Drawing.Point(285, 20);
//            this.rdoUser.Name = "rdoUser";
//            this.rdoUser.Size = new System.Drawing.Size(48, 16);
//            this.rdoUser.TabIndex = 99;
//            this.rdoUser.Text = "User";
//            this.rdoUser.CheckedChanged += new System.EventHandler(this.rdoUser_CheckedChanged);
//            // 
//            // rdoGlobal
//            // 
//            this.rdoGlobal.Location = new System.Drawing.Point(336, 20);
//            this.rdoGlobal.Name = "rdoGlobal";
//            this.rdoGlobal.Size = new System.Drawing.Size(56, 16);
//            this.rdoGlobal.TabIndex = 100;
//            this.rdoGlobal.Text = "Global";
//            this.rdoGlobal.CheckedChanged += new System.EventHandler(this.rdoGlobal_CheckedChanged);
//            // 
//            // frmFilterProperties
//            // 
//            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
//            this.ClientSize = new System.Drawing.Size(744, 600);
//            this.Controls.Add(this.rdoOwner);
//            this.Controls.Add(this.rdoUser);
//            this.Controls.Add(this.rdoGlobal);
//            this.Controls.Add(this.txtFilterName);
//            this.Controls.Add(this.btnOK);
//            this.Controls.Add(this.btnSaveAs);
//            this.Controls.Add(this.btnHelp);
//            this.Controls.Add(this.btnCancel);
//            this.Controls.Add(this.btnSave);
//            this.Controls.Add(this.lblFilterName);
//            this.Controls.Add(this.tabFilter);
//            this.Name = "frmFilterProperties";
//            this.Text = "Filter Properties";
//            this.Load += new System.EventHandler(this.frmFilterProperties_Load);
//            this.Activated += new System.EventHandler(this.frmFilterProperties_Activated);
//            this.Controls.SetChildIndex(this.tabFilter, 0);
//            this.Controls.SetChildIndex(this.lblFilterName, 0);
//            this.Controls.SetChildIndex(this.btnSave, 0);
//            this.Controls.SetChildIndex(this.btnCancel, 0);
//            this.Controls.SetChildIndex(this.btnHelp, 0);
//            this.Controls.SetChildIndex(this.btnSaveAs, 0);
//            this.Controls.SetChildIndex(this.btnOK, 0);
//            this.Controls.SetChildIndex(this.txtFilterName, 0);
//            this.Controls.SetChildIndex(this.rdoGlobal, 0);
//            this.Controls.SetChildIndex(this.rdoUser, 0);
//            this.Controls.SetChildIndex(this.rdoOwner, 0);
//            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
//            this.tabFilter.ResumeLayout(false);
//            this.tpgAttributes.ResumeLayout(false);
//            this.tpgData.ResumeLayout(false);
//            this.tpgData.PerformLayout();
//            this.grpQualifiers.ResumeLayout(false);
//            this.grpQualifiers.PerformLayout();
//            this.grpOperands.ResumeLayout(false);
//            this.grpPlan.ResumeLayout(false);
//            this.grpPlan.PerformLayout();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }
//        #endregion

//        public delegate void FilterPropertiesSaveEventHandler(object source, FilterPropertiesSaveEventArgs e);
//        public event FilterPropertiesSaveEventHandler OnFilterPropertiesSaveHandler;
////Begin Track #4052 - JScott - Filters not being enqueued

//        public delegate void FilterPropertiesCloseEventHandler(object source, FilterPropertiesCloseEventArgs e);
//        public event FilterPropertiesCloseEventHandler OnFilterPropertiesCloseHandler;
////End Track #4052 - JScott - Filters not being enqueued

//        #region Fields
//        //private SessionAddressBlock SAB;
//        private StoreFilterProfile _filterProf;
//        private FunctionSecurityProfile _filterUserSecurity;
//        private FunctionSecurityProfile _filterGlobalSecurity;
//        //private FilterDefinition FilterDef;
////Begin Track #4052 - JScott - Filters not being enqueued
//        private bool _readOnly;
////End Track #4052 - JScott - Filters not being enqueued
//        //private int _multiSelectStartIdx;
//        //private int _multiSelectEndIdx;
//        //private int _indexOfItemUnderMouseToDrag;
//        private ProfileList _versionProfList;
//        private ProfileList _variableProfList;
//        private ProfileList _timeTotalVariableProfList;
//        private StoreFilterData _dlFilter;
//        private FolderDataLayer _dlFolder;
//        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        private MIDFilterNode _currParentNode;
//        private MIDFilterNode _userNode;
//        private MIDFilterNode _globalNode;
//        private int _initialUserRID;
//        private int _initialOwnerRID;
//        private bool _canUpdateFilter;
//        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        #endregion

////Begin Track #4052 - JScott - Filters not being enqueued
////		public frmFilterProperties(SessionAddressBlock aSAB, MIDFilterNode aParentNode, int aUserRID)
//        public frmFilterProperties(
//            SessionAddressBlock aSAB,
//            MIDFilterNode aCurrParentNode,
//            MIDFilterNode aUserNode,
//            MIDFilterNode aGlobalNode,
//            int aUserRID,
//            int aOwnerRID,
//            bool aReadOnly)
////End Track #4052 - JScott - Filters not being enqueued
//            : base(aSAB)
//        {
//            InitializeComponent();

//            //SAB = aSAB;
//            _filterProf = new StoreFilterProfile(-1);
//            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//            //_filterProf.UserRID = aUserRID;
//            _currParentNode = aCurrParentNode;
//            _userNode = aUserNode;
//            _globalNode = aGlobalNode;
//            _initialUserRID = aUserRID;
//            _initialOwnerRID = aOwnerRID;
//            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
////Begin Track #4052 - JScott - Filters not being enqueued
//            _readOnly = aReadOnly;
////End Track #4052 - JScott - Filters not being enqueued
//        }

////Begin Track #4052 - JScott - Filters not being enqueued
////		public frmFilterProperties(SessionAddressBlock aSAB, MIDFilterNode aParentNode, StoreFilterProfile aStoreFilterProf)
//        public frmFilterProperties(
//            SessionAddressBlock aSAB,
//            MIDFilterNode aCurrParentNode,
//            MIDFilterNode aUserNode,
//            MIDFilterNode aGlobalNode,
//            StoreFilterProfile aStoreFilterProf,
//            bool aReadOnly)
////End Track #4052 - JScott - Filters not being enqueued
//            : base(aSAB)
//        {
//            InitializeComponent();

//            //SAB = aSAB;
//            _filterProf = aStoreFilterProf;
//            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//            _currParentNode = aCurrParentNode;
//            _userNode = aUserNode;
//            _globalNode = aGlobalNode;
//            _initialUserRID = aStoreFilterProf.UserRID;
//            _initialOwnerRID = aStoreFilterProf.OwnerUserRID;
//            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
////Begin Track #4052 - JScott - Filters not being enqueued
//            _readOnly = aReadOnly;
////End Track #4052 - JScott - Filters not being enqueued
//        }

//        public void HandleExceptions(Exception exc)
//        {
//            Debug.WriteLine(exc.ToString());
//            MessageBox.Show(exc.ToString());
//        }

//        private void frmFilterProperties_Load(object sender, System.EventArgs e)
//        {
//            PanelTag panelTag;
//            DataTable dtVariables;
//            DataTable dtVersions;
//            DataTable dtStatus;
//            DataTable dtStatusText;
//            int i;

//            try
//            {
//                FormLoaded = false;

////Begin Track #4052 - JScott - Filters not being enqueued
////				FunctionSecurity = new FunctionSecurityProfile(Convert.ToInt32(eSecurityFunctions.ToolsFilters));
//                FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFilters);
////End Track #4052 - JScott - Filters not being enqueued
//                _filterUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersUser);
//                _filterGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersGlobal);

//                _dlFilter = new StoreFilterData();
//                _dlFolder = new FolderDataLayer();

////Begin Track #4052 - JScott - Filters not being enqueued
////				FunctionSecurity.SetFullControl();
//                if (!_readOnly && (_filterUserSecurity.AllowUpdate || _filterGlobalSecurity.AllowUpdate))
//                {
//                    FunctionSecurity.SetAllowUpdate();
//                }
//                else
//                {
//                    FunctionSecurity.SetReadOnly();
//                }
////End Track #4052 - JScott - Filters not being enqueued
//                SetReadOnly(FunctionSecurity.AllowUpdate);

//                // Create Profile Lists

//                _variableProfList = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;
//                _timeTotalVariableProfList = SAB.ApplicationServerSession.DefaultPlanComputations.PlanTimeTotalVariables.TimeTotalVariableProfileList;
//                _versionProfList = SAB.ClientServerSession.GetUserForecastVersions();

//                tpgAttributes.Tag = pnlAttrQuery;
//                FilterDef = new StoreFilterDefinition(SAB, SAB.ClientServerSession, _dlFilter, new LabelCreatorDelegate(LabelCreator));

//                panelTag = new PanelTag(typeof(AttrQuerySpacerOperand), btnAttrOr, ((StoreFilterDefinition)FilterDef).AttrOperandList);
//                panelTag.AllowedDropTypes.Add(typeof(GenericQueryOperand));
//                panelTag.AllowedDropTypes.Add(typeof(AttrQueryOperand));
//                //panelTag.AllowedDropTypes.Add(typeof(MIDStoreNode));
//                panelTag.AllowedDropTypes.Add(typeof(TreeNodeClipboardList));
//                //panelTag.AllowedClipboardProfileTypes.Add(eClipboardDataType.Attribute);
//                //panelTag.AllowedClipboardProfileTypes.Add(eClipboardDataType.AttributeSet);
//                panelTag.AllowedClipboardProfileTypes.Add(eProfileType.StoreGroup);
//                panelTag.AllowedClipboardProfileTypes.Add(eProfileType.StoreGroupLevel);
//                //Begin Track #6242 - JScott - When creating a filter cannot drag/drop a store attribute on the attribute tab
//                panelTag.AllowedClipboardProfileTypes.Add(eProfileType.Store);
//                //End Track #6242 - JScott - When creating a filter cannot drag/drop a store attribute on the attribute tab

//                pnlAttrQuery.Tag = panelTag;
//                btnAttrAnd.Tag = typeof(GenericQueryAndOperand);
//                btnAttrOr.Tag = typeof(GenericQueryOrOperand);
//                btnAttrLParen.Tag = typeof(GenericQueryLeftParenOperand);
//                btnAttrRParen.Tag = typeof(GenericQueryRightParenOperand);

//                tpgData.Tag = pnlDataQuery;

//                panelTag = new PanelTag(typeof(DataQuerySpacerOperand), btnDataOr, ((StoreFilterDefinition)FilterDef).DataOperandList);
//                panelTag.AllowedDropTypes.Add(typeof(GenericQueryOperand));
//                panelTag.AllowedDropTypes.Add(typeof(DataQueryOperand));

//                pnlDataQuery.Tag = panelTag;
//                btnDataAnd.Tag = typeof(GenericQueryAndOperand);
//                btnDataOr.Tag = typeof(GenericQueryOrOperand);
//                btnDataLParen.Tag = typeof(GenericQueryLeftParenOperand);
//                btnDataRParen.Tag = typeof(GenericQueryRightParenOperand);
//                btnDataEqual.Tag = typeof(DataQueryEqualOperand);
//                btnDataLess.Tag = typeof(DataQueryLessOperand);
//                btnDataGreater.Tag = typeof(DataQueryGreaterOperand);
//                btnDataLessEqual.Tag = typeof(DataQueryLessEqualOperand);
//                btnDataGreaterEqual.Tag = typeof(DataQueryGreaterEqualOperand);
//                btnDataNot.Tag = typeof(DataQueryNotOperand);
//                btnDataStoreDetail.Tag = typeof(DataQueryStoreDetailOperand);
//                btnDataStoreTotal.Tag = typeof(DataQueryStoreTotalOperand);
//                btnDataStoreAverage.Tag = typeof(DataQueryStoreAverageOperand);
//                btnDataChainDetail.Tag = typeof(DataQueryChainDetailOperand);
//                btnDataDate.Tag = typeof(DataQueryDateRangeOperand);
//                btnDataAll.Tag = typeof(DataQueryAllOperand);
//                btnDataAny.Tag = typeof(DataQueryAnyOperand);
////Begin Track #5111 - JScott - Add additional filter functionality
//                btnDataJoin.Tag = typeof(DataQueryJoinOperand);
////End Track #5111 - JScott - Add additional filter functionality
//                btnDataAverage.Tag = typeof(DataQueryAverageOperand);
//                btnDataTotal.Tag = typeof(DataQueryTotalOperand);
//                btnDataCorresponding.Tag = typeof(DataQueryCorrespondingOperand);
//                btnDataLiteral.Tag = typeof(DataQueryLiteralOperand);
//                btnDataGrade.Tag = typeof(DataQueryGradeOperand);
//                btnDataPctChange.Tag = typeof(DataQueryPctChangeOperand);
//                btnDataPctOf.Tag = typeof(DataQueryPctOfOperand);
//                btnDataMerchandise.Tag = typeof(DataQueryNodeOperand);

//                lstVersions.Tag = lblVersion;
//                lstVariables.Tag = lblVariable;
//                lstStatus.Tag = lblStatus;
//                lblVersion.Tag = new LabelTag(lstVersions);
//                lblVariable.Tag = new LabelTag(lstVariables);
//                lblStatus.Tag = new LabelTag(lstStatus);

//                // Setup Variables ListBox

//                dtVariables = MIDEnvironment.CreateDataTable("Variables");
//                dtVariables.Columns.Add("Description", typeof(string));
//                dtVariables.Columns.Add("Profile", typeof(object));

//                foreach (VariableProfile varProf in _variableProfList)
//                {
//                    dtVariables.Rows.Add(new object[] {varProf.VariableName, varProf});
//                }

//                foreach (VariableProfile varProf in _variableProfList)
//                {
//                    for (i = 0; i < varProf.TimeTotalVariables.Count; i++)
//                    {
//                        dtVariables.Rows.Add(new object[] {"[Date Total] " + ((TimeTotalVariableProfile)varProf.TimeTotalVariables[i]).VariableName, new TimeTotalVariableReference((TimeTotalVariableProfile)varProf.TimeTotalVariables[i], varProf, i + 1)});
//                    }
//                }

//                lstVariables.DataSource = dtVariables;
//                lstVariables.DisplayMember = "Description";
//                lstVariables.ValueMember = "Profile";

//                // Setup Versions ListBox

//                dtVersions = MIDEnvironment.CreateDataTable("Versions");
//                dtVersions.Columns.Add("Description", typeof(string));
//                dtVersions.Columns.Add("Profile", typeof(object));

//                foreach (VersionProfile verProf in _versionProfList)
//                {
//                    dtVersions.Rows.Add(new object[] {verProf.Description, verProf});
//                }

//                lstVersions.DataSource = dtVersions;
//                lstVersions.DisplayMember = "Description";
//                lstVersions.ValueMember = "Profile";

//                // Setup Status ListBox

//                dtStatus = MIDEnvironment.CreateDataTable("Status");
//                dtStatus.Columns.Add("Description", typeof(string));
//                dtStatus.Columns.Add("Profile", typeof(object));

//                dtStatusText = MIDText.GetTextType(eMIDTextType.eStoreStatus, eMIDTextOrderBy.TextCode);

//                foreach (DataRow row in dtStatusText.Rows)
//                {
//                    dtStatus.Rows.Add(new object[] {row["TEXT_VALUE"], new StatusProfile((eStoreStatus)Convert.ToInt32(row["TEXT_CODE"]), Convert.ToString(row["TEXT_VALUE"]))});
//                }

//                lstStatus.DataSource = dtStatus;
//                lstStatus.DisplayMember = "Description";
//                lstStatus.ValueMember = "Profile";

//                // Load Filter
//                CurrentPanel = (Panel)tabFilter.SelectedTab.Tag;

//                txtFilterName.Text = _filterProf.Name;

//                if (_filterProf.Key >= 0)
//                {
//                    FilterDef = new StoreFilterDefinition(SAB, SAB.ClientServerSession, _dlFilter, new LabelCreatorDelegate(LabelCreator), _versionProfList, _variableProfList, _timeTotalVariableProfList, _filterProf.Key);
//                }
//                else
//                {
//                    FilterDef = new StoreFilterDefinition(SAB, SAB.ClientServerSession, _dlFilter, new LabelCreatorDelegate(LabelCreator));
//                }

//                ((PanelTag)pnlAttrQuery.Tag).OperandArray = ((StoreFilterDefinition)FilterDef).AttrOperandList;
//                ((PanelTag)pnlDataQuery.Tag).OperandArray = ((StoreFilterDefinition)FilterDef).DataOperandList;

//                RedrawOperands(pnlAttrQuery);
//                RedrawOperands(pnlDataQuery);

//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                SetUserFields(_filterProf, _initialUserRID, _initialOwnerRID);

//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                FormLoaded = true;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void frmFilterProperties_Activated(object sender, System.EventArgs e)
//        {
//            try
//            {
//                txtFilterName.Focus();
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

////Begin Track #4911 - JScott - Filter not being dequeued
//////Begin Track #4052 - JScott - Filters not being enqueued
////		private void frmFilterProperties_Closing(object sender, System.ComponentModel.CancelEventArgs e)
////		{
////			try
////			{
////				if (OnFilterPropertiesCloseHandler != null)
////				{
////					OnFilterPropertiesCloseHandler(this, new FilterPropertiesCloseEventArgs());
////				}
////			}
////			catch (Exception exc)
////			{
////				HandleExceptions(exc);
////			}
////		}
////
//////End Track #4052 - JScott - Filters not being enqueued
//        protected override void AfterClosing()
//        {
//            try
//            {
//                base.AfterClosing ();

//                if (OnFilterPropertiesCloseHandler != null)
//                {
//                    OnFilterPropertiesCloseHandler(this, new FilterPropertiesCloseEventArgs());
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

////End Track #4911 - JScott - Filter not being dequeued
//        //public SessionAddressBlock SAB
//        //{
//        //    get
//        //    {
//        //        return _SAB;
//        //    }
//        //}

//        public ContextMenu LabelMenu
//        {
//            get
//            {
//                return mnuLabelMenu;
//            }
//        }

//        //public TextBox LiteralEditTextBox
//        //{
//        //    get
//        //    {
//        //        return txtLiteralEdit;
//        //    }
//        //}

//        //public TextBox GradeEditTextBox
//        //{
//        //    get
//        //    {
//        //        return txtGradeEdit;
//        //    }
//        //}

//        //public Panel CurrentPanel
//        //{
//        //    get
//        //    {
//        //        try
//        //        {
//        //            return (Panel)tabFilter.SelectedTab.Tag;
//        //        }
//        //        catch (Exception exc)
//        //        {
//        //            string message = exc.ToString();
//        //            throw;
//        //        }
//        //    }
//        //}

//        //public int MultiSelectStartIdx
//        //{
//        //    get
//        //    {
//        //        return _multiSelectStartIdx;
//        //    }
//        //    set
//        //    {
//        //        _multiSelectStartIdx = value;
//        //    }
//        //}

//        //public int MultiSelectEndIdx
//        //{
//        //    get
//        //    {
//        //        return _multiSelectEndIdx;
//        //    }
//        //    set
//        //    {
//        //        _multiSelectEndIdx = value;
//        //    }
//        //}

//        #region Event Handlers
//        #region frmFilterProperties Events

//        public void ClearForm()
//        {
//            PanelTag panelTag;

//            try
//            {
//                panelTag = (PanelTag)((Panel)tpgAttributes.Tag).Tag;

//                foreach (QueryOperand operand in panelTag.OperandArray)
//                {
//                    operand.Label.Parent = null;
//                }

//                panelTag.Clear();

//                panelTag = (PanelTag)((Panel)tpgData.Tag).Tag;

//                foreach (QueryOperand operand in panelTag.OperandArray)
//                {
//                    operand.Label.Parent = null;
//                }

//                panelTag.Clear();

//                ChangePending = false;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        #endregion
		
//        #region tabFilter Events
//        private void tabFilter_SizeChanged(object sender, System.EventArgs e)
//        {
//            try
//            {
//                if (FormLoaded)
//                {
//                    CurrentPanelRedrawOperands();
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }
//        #endregion

//        //Begin TT#211 - JScott - Variable, Version, and Status dropdowns do not align with corresponding text boxes
//        //#region tpgData Events
//        //private void tpgData_MouseEnter(object sender, System.EventArgs e)
//        //{
//        //    try
//        //    {
//        //        lstVariables.Visible = false;
//        //        lstVersions.Visible = false;
//        //        lstStatus.Visible = false;
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        HandleExceptions(exc);
//        //    }
//        //}
//        //#endregion

//        //#region pnlDataQuery Events
//        //private void pnlDataQuery_MouseEnter(object sender, System.EventArgs e)
//        //{
//        //    try
//        //    {
//        //        lstVariables.Visible = false;
//        //        lstVersions.Visible = false;
//        //        lstStatus.Visible = false;
//        //    }
//        //    catch (Exception exc)
//        //    {
//        //        HandleExceptions(exc);
//        //    }
//        //}
//        //#endregion

//        //End TT#211 - JScott - Variable, Version, and Status dropdowns do not align with corresponding text boxes
//        #region panel Events
//        private void panel_Click(object sender, EventArgs e)
//        {
//            BasePanel_Click(sender, e);
//        }

//        public void panel_DragEnter(object sender, DragEventArgs e)
//        {
//            BasePanel_DragEnter(sender, e);
//        }

//        private void panel_DragOver(object sender, DragEventArgs e)
//        {
//            BasePanel_DragOver(sender, e);
//        }

//        public void panel_DragDrop(object sender, DragEventArgs e)
//        {
//            BasePanel_DragDrop(sender, e);
//        }

//        public void panel_DragLeave(object sender, EventArgs e)
//        {
//            BasePanel_DragLeave(sender, e);
//        }
//        #endregion

//        #region Miscellaneous Events
//        private void mniDelete_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                CurrentPanelDeleteSelectedOperands();
//                CurrentPanelRedrawOperands();
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void mnuLabelMenu_Popup(object sender, EventArgs e)
//        {
//            try
//            {
//                if (((PanelTag)CurrentPanel.Tag).SelectedOperandList.Count == 0)
//                {
//                    mniDelete.Enabled = false;
//                }
//                else
//                {
//                    mniDelete.Enabled = true;
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void txtFilterName_TextChanged(object sender, System.EventArgs e)
//        {
//            if (FormLoaded)
//            {
//                ChangePending = true;
//            }
//        }

//        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        private void rdoUser_CheckedChanged(object sender, EventArgs e)
//        {
//            if (FormLoaded)
//            {
//                ChangePending = true;
//                EnableSaveButtons();
//            }
//        }

//        private void rdoGlobal_CheckedChanged(object sender, EventArgs e)
//        {
//            if (FormLoaded)
//            {
//                ChangePending = true;
//                EnableSaveButtons();
//            }
//        }

//        private void rdoOwner_CheckedChanged(object sender, EventArgs e)
//        {
//            if (FormLoaded)
//            {
//                ChangePending = true;
//                EnableSaveButtons();
//            }
//        }

//        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        private void btnOK_Click(object sender, System.EventArgs e)
//        {
//            try
//            {
//                if (Save(eUpdateMode.Update))
//                {
//                    this.Close();
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void btnCancel_Click(object sender, System.EventArgs e)
//        {
//            try
//            {
//                this.Close();
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void btnSave_Click(object sender, System.EventArgs e)
//        {
//            try
//            {
//                Save(eUpdateMode.Update);
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }
		
//        private void btnSaveAs_Click(object sender, System.EventArgs e)
//        {
//            try
//            {
//                Save(eUpdateMode.Create);
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }
//        #endregion

//        #region txtLiteralEdit Events
//        private void txtLiteralEdit_Leave(object sender, System.EventArgs e)
//        {
//            try
//            {
//                txtLiteralEdit.Enabled = false;
//                txtLiteralEdit.Visible = false;
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void txtLiteralEdit_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
//        {
//            try
//            {
//                if (e.KeyChar == 13 || e.KeyChar == 9)
//                {
//                    ((DataQueryLiteralOperand)txtLiteralEdit.Tag).LiteralValue = Convert.ToDouble(txtLiteralEdit.Text, CultureInfo.CurrentUICulture);
//                    CurrentPanelClearSelectedOperands();
//                    CurrentPanelRedrawOperands();
//                    ((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
//                    e.Handled = true;
//                }
//                else if (e.KeyChar == 27)
//                {
//                    ((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
//                    e.Handled = true;
//                }
//                else if (Char.IsNumber(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '.' || Char.IsControl(e.KeyChar))
//                {
//                }
//                else
//                {
//                    e.Handled = true;
//                }
//            }
//            catch (FormatException)
//            {
//            }
//            catch (InvalidCastException)
//            {
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }
//        #endregion

//        #region txtMerchandiseEdit Events
//        private void txtMerchandiseEdit_Leave(object sender, System.EventArgs e)
//        {
//            try
//            {
//                txtMerchandiseEdit.Enabled = false;
//                txtMerchandiseEdit.Visible = false;
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void txtMerchandiseEdit_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
//        {
//            string productID;
//            string[] pArray;
//            HierarchyNodeProfile hnp;
//            string errorMessage;

//            try
//            {
//                if (e.KeyChar == 13 || e.KeyChar == 9)
//                {
//                    productID = txtMerchandiseEdit.Text.Trim();
//                    pArray = productID.Split(new char[] {'['});
//                    productID = pArray[0].Trim();

////					hnp = SAB.HierarchyServerSession.GetNodeData(productID);
//                    HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
//                    EditMsgs em = new EditMsgs();
//                    hnp =  hm.NodeLookup(ref em, productID, false);

//                    if (hnp.Key == Include.NoRID)
//                    {
//                        errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), txtMerchandiseEdit.Text);
//                        MessageBox.Show(errorMessage);
//                        ((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
//                    }
//                    else 
//                    {
//                        ((DataQueryNodeOperand)txtMerchandiseEdit.Tag).VariableOperand.NodeProfile = hnp;
//                        CurrentPanelClearSelectedOperands();
//                        CurrentPanelRedrawOperands();
//                        ((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
//                    }	

//                    e.Handled = true;
//                }
//                else if (e.KeyChar == 27)
//                {
//                    ((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
//                    e.Handled = true;
//                }
//            }
//            catch (FormatException)
//            {
//            }
//            catch (InvalidCastException)
//            {
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }
//        #endregion

//        #region txtGradeEdit Events
//        private void txtGradeEdit_Leave(object sender, System.EventArgs e)
//        {
//            try
//            {
//                txtGradeEdit.Enabled = false;
//                txtGradeEdit.Visible = false;
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        private void txtGradeEdit_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
//        {
//            try
//            {
//                if (e.KeyChar == 13 || e.KeyChar == 9)
//                {
//                    ((DataQueryGradeOperand)txtGradeEdit.Tag).GradeValue = txtGradeEdit.Text;
//                    CurrentPanelClearSelectedOperands();
//                    CurrentPanelRedrawOperands();
//                    ((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
//                    e.Handled = true;
//                }
//                else if (e.KeyChar == 27)
//                {
//                    ((PanelTag)CurrentPanel.Tag).DefaultControl.Focus();
//                    e.Handled = true;
//                }
//                else if (Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '.' || Char.IsControl(e.KeyChar))
//                {
//                }
//                else
//                {
//                    e.Handled = true;
//                }
//            }
//            catch (FormatException)
//            {
//            }
//            catch (InvalidCastException)
//            {
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }
//        #endregion

//        #region toolList Events
//        private void toolList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            BaseToolList_MouseDown(sender, e);
//        }

//        private void toolList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            BaseToolList_MouseMove(sender, e);
//        }

//        private void toolList_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            BaseToolList_MouseUp(sender, e);
//        }
//        //Begin TT#211 - JScott - Variable, Version, and Status dropdowns do not align with corresponding text boxes

//        private void toolList_MouseLeave(object sender, EventArgs e)
//        {
//            BaseToolList_MouseLeave(sender, e);
//        }
//        //End TT#211 - JScott - Variable, Version, and Status dropdowns do not align with corresponding text boxes
//        #endregion

//        #region toolLabel Events
//        private void toolLabel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            BaseToolLabel_MouseDown(sender, e);
//        }

//        private void toolLabel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            BaseToolLabel_MouseMove(sender, e);
//        }

//        private void toolLabel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
//        {
//            BaseToolLabel_MouseUp(sender, e);
//        }

//        private void toolLabel_Click(object sender, System.EventArgs e)
//        {
//            BaseToolLabel_Click(sender, e);
//        }
//        #endregion

//        #region toolButton Events
//        private void toolButton_Click(object sender, EventArgs e)
//        {
//            BaseToolButton_Click(sender, e);
//        }

//        private void toolButton_MouseDown(object sender, MouseEventArgs e)
//        {
//            BaseToolButton_MouseDown(sender, e);
//        }

//        private void toolButton_MouseMove(object sender, MouseEventArgs e)
//        {
//            BaseToolButton_MouseMove(sender, e);
//        }

//        private void toolButton_MouseUp(object sender, MouseEventArgs e)
//        {
//            BaseToolButton_MouseUp(sender, e);
//        }

//        private void toolButton_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
//        {
//            BaseToolButton_KeyDown(sender, e);
//        }

//        private void toolButton_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
//        {
//            BaseToolButton_KeyPress(sender, e);
//        }
//        #endregion
//        #endregion

//        #region Public Methods
//        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        private void EnableSaveButtons()
//        {
//            try
//            {
//                btnSave.Enabled = _canUpdateFilter;
//                btnOK.Enabled = _canUpdateFilter;
//                btnSaveAs.Enabled = false;

//                if (_filterProf.Key != Include.NoRID)
//                {
//                    if (txtFilterName.Text.Trim() != _filterProf.Name.Trim() &&
//                        !rdoOwner.Checked)
//                    {
//                        btnSaveAs.Enabled = _canUpdateFilter;
//                    }

//                    if ((rdoUser.Checked && rdoUser.Tag == null) ||
//                        (rdoGlobal.Checked && rdoGlobal.Tag == null) ||
//                        (rdoOwner.Checked && rdoOwner.Tag == null))
//                    {
//                        btnSaveAs.Enabled = _canUpdateFilter;
//                        btnSave.Enabled = false;
//                        btnOK.Enabled = false;
//                    }
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        override protected bool SaveChanges()
//        {
//            try
//            {
//                if (Save(eUpdateMode.Update))
//                {
//                    ErrorFound = false;
//                }
//                else
//                {
//                    ErrorFound = true;
//                }

//                return true;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private bool Save(eUpdateMode aUpdateMode)
//        {
//            Cursor.Current = Cursors.WaitCursor;

//            try
//            {
//                if (CheckValues(aUpdateMode))
//                {
//                    return SaveFilterValues(aUpdateMode);
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//            finally
//            {
//                Cursor.Current = Cursors.Default;
//            }
//        }

//        private bool CheckValues(eUpdateMode aUpdateMode)
//        {
//            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//            int newUserRID;
//            int newOwnerRID;

//            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//            try
//            {
//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //if ((_filterProf.UserRID != Include.GlobalUserRID && !_filterUserSecurity.AllowUpdate) ||  // Issue 3806
//                //    (_filterProf.UserRID == Include.GlobalUserRID && !_filterGlobalSecurity.AllowUpdate))  // Issue 3806
//                //{
//                //    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
//                //    return false;
//                //}
//                newUserRID = GetNewUserRID();
//                newOwnerRID = GetNewOwnerRID();

//                if (rdoUser.Checked && !_filterUserSecurity.AllowUpdate ||
//                    rdoGlobal.Checked && !_filterGlobalSecurity.AllowUpdate)
//                {
//                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return false;
//                }
//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//                if (txtFilterName.Text.Trim().Length == 0)
//                {
//                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FilterNameRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return false;
//                }

//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //if (_filterProf.Key == -1 || aUpdateMode == eUpdateMode.Create || _filterProf.Name != txtFilterName.Text)
//                //{
//                //    if (_dlFilter.StoreFilter_GetKey(_filterProf.UserRID, txtFilterName.Text.Trim()) != -1)
//                if (_filterProf.Key == -1 ||
//                    aUpdateMode == eUpdateMode.Create ||
//                    _filterProf.Name != txtFilterName.Text ||
//                    _filterProf.UserRID != newUserRID ||
//                    _filterProf.OwnerUserRID != newOwnerRID)
//                {
//                //    if (_dlFilter.StoreFilter_GetKey(newUserRID, txtFilterName.Text.Trim()) != -1)
//                ////End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                //    {
//                //        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FilterNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
//                //        return false;
//                //    }
//                }

//                return true;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private bool SaveFilterValues(eUpdateMode aUpdateMode)
//        {
//            StoreFilterProfile filterProf;
//            //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//            MIDFilterNode saveToNode;
//            int ownerRID;
//            int userRID;
//            //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders

//            try
//            {
//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                saveToNode = GetNewSaveToNode();
//                userRID = GetNewUserRID();
//                ownerRID = GetNewOwnerRID();

//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                if (aUpdateMode == eUpdateMode.Create)
//                {
//                    filterProf = (StoreFilterProfile)_filterProf.Clone();
//                }
//                else
//                {
//                    filterProf = _filterProf;
//                }

//                filterProf.Name = txtFilterName.Text.Trim();

//                if (filterProf.Key == Include.NoRID || aUpdateMode == eUpdateMode.Create)
//                {
//                    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                    filterProf.Key = Include.NoRID;
//                    filterProf.UserRID = userRID;
//                    filterProf.OwnerUserRID = ownerRID;
//                    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                    filterProf.Key = FilterDef.SaveFilter(filterProf.Key, filterProf.UserRID, filterProf.Name);

//                    _dlFolder.OpenUpdateConnection();

//                    try
//                    {
//                        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                        //_dlFolder.Folder_Item_Insert(_parentNode.FolderItem.ItemRID, _filterProf.Key, eFolderChildType.Filter);
//                        _dlFolder.Folder_Item_Insert(saveToNode.Profile.Key, filterProf.Key, eProfileType.StoreFilter);
//                        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                        _dlFolder.CommitData();
//                    }
//                    catch (Exception exc)
//                    {
//                        string message = exc.ToString();
//                        throw;
//                    }
//                    finally
//                    {
//                        _dlFolder.CloseUpdateConnection();
//                    }
//                }
//                else
//                {
//                    FilterDef.SaveFilter(filterProf.Key, filterProf.UserRID, filterProf.Name);
//                }

//                ChangePending = false;

//                if (OnFilterPropertiesSaveHandler != null)
//                {
//                    //OnFilterPropertiesSaveHandler(this, new FilterPropertiesSaveEventArgs(saveToNode, filterProf));
//                }

//                //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                SetUserFields(filterProf, filterProf.UserRID, filterProf.OwnerUserRID);

//                //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//                return true;
//            }
//            catch (FilterSyntaxErrorException exc)
//            {
//                switch (exc.FilterListType)
//                {
//                    case eFilterListType.Attribute :
//                        tabFilter.SelectedTab = tpgAttributes;
//                        break;

//                    case eFilterListType.Data :
//                        tabFilter.SelectedTab = tpgData;
//                        break;
//                }

//                if (exc.ErrorOperand != null)
//                {
//                    ((BasicQueryLabel)exc.ErrorOperand.Label).HighlightClick();
//                }

//                System.Windows.Forms.MessageBox.Show(this, exc.Message, "Filter Syntax Error");

//                return false;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        private void SetUserFields(StoreFilterProfile aFilterProf, int aInitialUserRID, int aInitialOwnerRID)
//        {
//            try
//            {
//                _filterProf = aFilterProf;
//                _initialUserRID = aInitialUserRID;
//                _initialOwnerRID = aInitialOwnerRID;
	
//                if (_initialOwnerRID != _initialUserRID)
//                {
//                    rdoOwner.Visible = true;
//                    rdoOwner.Text = rdoOwner.Text.Replace("{1}", SAB.ClientServerSession.GetUserName(_initialOwnerRID));
//                }
//                else
//                {
//                    rdoOwner.Visible = false;
//                }

//                rdoUser.Enabled = true;
//                rdoUser.Tag = null;
//                rdoGlobal.Enabled = true;
//                rdoGlobal.Tag = null;
//                rdoOwner.Enabled = true;
//                rdoOwner.Tag = null;

//                // Check appropriate User Radio Button

//                if (_initialOwnerRID == Include.GlobalUserRID)
//                {
//                    if (SAB.ClientServerSession.UserRID == Include.GlobalUserRID)
//                    {
//                        rdoUser.Checked = true;
//                        rdoUser.Tag = true;
//                    }
//                    else
//                    {
//                        rdoGlobal.Checked = true;
//                        rdoGlobal.Tag = true;
//                    }
//                }
//                else if (_initialOwnerRID == SAB.ClientServerSession.UserRID || _initialOwnerRID == Include.NoRID)
//                {
//                    rdoUser.Checked = true;
//                    rdoUser.Tag = true;
//                }
//                else
//                {
//                    rdoOwner.Checked = true;
//                    rdoOwner.Tag = true;
//                }

//                // Enable User Radio Buttons

//                if (_readOnly || !_filterGlobalSecurity.AllowUpdate || SAB.ClientServerSession.UserRID == Include.GlobalUserRID)
//                {
//                    rdoGlobal.Enabled = false;
//                }

//                if (_readOnly || !_filterUserSecurity.AllowUpdate)
//                {
//                    rdoUser.Enabled = false;
//                }

//                if (_readOnly || !rdoOwner.Visible || !_filterUserSecurity.AllowUpdate)
//                {
//                    rdoOwner.Enabled = false;
//                }

//                // Enable Save/OK/Save As Button

//                if (!_readOnly &&
//                    ((rdoGlobal.Checked && rdoGlobal.Enabled) ||
//                    (rdoUser.Checked && rdoUser.Enabled) ||
//                    (rdoOwner.Checked && rdoOwner.Enabled)))
//                {
//                    _canUpdateFilter = true;
//                }
//                else
//                {
//                    _canUpdateFilter = false;
//                }

//                EnableSaveButtons();
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private int GetNewUserRID()
//        {
//            try
//            {
//                if (rdoOwner.Checked)
//                {
//                    return _initialUserRID;
//                }
//                else if (rdoUser.Checked)
//                {
//                    return SAB.ClientServerSession.UserRID;
//                }
//                else if (rdoGlobal.Checked)
//                {
//                    return Include.GlobalUserRID;
//                }
//                else
//                {
//                    return Include.SystemUserRID;
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private int GetNewOwnerRID()
//        {
//            try
//            {
//                if (rdoOwner.Checked)
//                {
//                    return _initialOwnerRID;
//                }
//                else if (rdoUser.Checked)
//                {
//                    return SAB.ClientServerSession.UserRID;
//                }
//                else if (rdoGlobal.Checked)
//                {
//                    return Include.GlobalUserRID;
//                }
//                else
//                {
//                    return Include.SystemUserRID;
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private MIDFilterNode GetNewSaveToNode()
//        {
//            try
//            {
//                if (rdoOwner.Checked)
//                {
//                    return _currParentNode;
//                }
//                else if (rdoUser.Checked)
//                {
//                    if (rdoUser.Tag != null)
//                    {
//                        return _currParentNode;
//                    }
//                    else
//                    {
//                        return _userNode;
//                    }
//                }
//                else
//                {
//                    if (rdoGlobal.Tag != null)
//                    {
//                        return _currParentNode;
//                    }
//                    else
//                    {
//                        return _globalNode;
//                    }
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
//        #endregion

//        #region IFormBase Members
//        override public void ICut()
//        {
//            try
//            {
//                MessageBox.Show("Not implemented yet");
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        override public void ICopy()
//        {
//            try
//            {
//                MessageBox.Show("Not implemented yet");
//            }
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        override public void IPaste()
//        {
//            try
//            {
//                MessageBox.Show("Not implemented yet");
//            }		
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }	

//        override public void ISave()
//        {
//            try
//            {
//                Save(eUpdateMode.Update);
//            }		
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }

//        override public void ISaveAs()
//        {
//            try
//            {
//                Save(eUpdateMode.Create);
//            }		
//            catch (Exception exc)
//            {
//                HandleExceptions(exc);
//            }
//        }
//        #endregion

//        private void tabFilter_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (tabFilter.SelectedTab != null)
//            {
//                CurrentPanel = (Panel)tabFilter.SelectedTab.Tag;
//            }
//        }
//    }

//    //public class FilterPropertiesSaveEventArgs : EventArgs
//    //{
//    //    private MIDFilterNode _parentNode;
//    //    private StoreFilterProfile _filterProf;

//    //    public FilterPropertiesSaveEventArgs(MIDFilterNode aParentNode, StoreFilterProfile aFilterProf)
//    //    {
//    //        _parentNode = aParentNode;
//    //        _filterProf = aFilterProf;
//    //    }

//    //    public MIDFilterNode ParentNode
//    //    {
//    //        get
//    //        {
//    //            return _parentNode;
//    //        }
//    //    }

//    //    public StoreFilterProfile FilterProfile
//    //    {
//    //        get
//    //        {
//    //            return _filterProf;
//    //        }
//    //    }
//    //}
////Begin Track #4052 - JScott - Filters not being enqueued

//    public class FilterPropertiesCloseEventArgs : EventArgs
//    {
//    }
//    public class FilterPropertiesSaveEventArgs : EventArgs
//    {
//        private MIDFilterNode _parentNode;
//        private filter _filter;

//        public FilterPropertiesSaveEventArgs(MIDFilterNode aParentNode, filter aFilter)
//        {
//            _parentNode = aParentNode;
//            _filter = aFilter;
//        }

//        public MIDFilterNode ParentNode
//        {
//            get
//            {
//                return _parentNode;
//            }
//        }

//        public filter SavedFilter
//        {
//            get
//            {
//                return _filter;
//            }
//        }
//    }
////End Track #4052 - JScott - Filters not being enqueued
//}
