using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Globalization;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Shared;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for SizeWarehouseMethod.
	/// </summary>
	public class frmSizeWarehouseMethod : SizeMethodsFormBase
	{

//		#region Member Variables
//		private SessionAddressBlock _sab;
//		private MIDWorkflowMethodTreeNode _explorerNode = null;
//		private int _nodeRID = -1;
//		private System.Windows.Forms.Panel pnlGlobalUser;
//		private System.Windows.Forms.RadioButton radGlobal;
//		private System.Windows.Forms.RadioButton radUser;
//		private System.Windows.Forms.TextBox txtMethodDesc;
//		private System.Windows.Forms.TextBox txtMethodName;
//		private System.Windows.Forms.Label label3;
//		private System.Windows.Forms.Button btnClose;
//		private System.Windows.Forms.Button btnSave;
//		private System.Windows.Forms.Button btnProcess;
//		private System.Windows.Forms.TabControl tabControl1;
//		private System.Windows.Forms.TabPage tabProperties;
//		private System.Windows.Forms.TabPage tabMethod;
//		private System.Windows.Forms.TabControl tabControl2;
//		private System.Windows.Forms.TabPage tabGeneral;
//		private System.Windows.Forms.TabPage tabConstraints;
//		private System.Windows.Forms.Panel pnlGridContainer;
//		private Infragistics.Win.UltraWinGrid.UltraGrid ugWorkflows;
////		private string _strMethodType;
////		private SizeOverrideMethod _sizeOverrideMethod;
////		private DataTable _dtEquatesDimSizeList;
////		private DataTable _dtEquatesDimSizeListCell;
////		private DataTable _EquateOverrideDTBackup = null;
////		private DataSet _FringeOverrideDSBackup = null;
////		private bool _EquateRollback = false;
////		private bool _FringeRollback = false;
////		private bool _editFringeByCell = false;
////		private ContextMenu _equatesContext = null;
////
////		private ContextMenu _fringeSetContext = null;
////		private ContextMenu _fringeColorContext = null;
////		private ContextMenu _fringeAllColorContext = null;
////		private ContextMenu _fringeFilterContext = null;
//
//
//
////		private const int C_FRINGE_CONTEXT_SET_APPLY = 0;
////		private const int C_FRINGE_CONTEXT_SET_CLEAR = 1;
////		private const int C_FRINGE_CONTEXT_SET_SEP1 = 2;
////		private const int C_FRINGE_CONTEXT_SET_ADD_FILTER = 3;
////		private const int C_FRINGE_CONTEXT_SET_ADD_COLOR = 4;
////
////		private const int C_FRINGE_CONTEXT_COLOR_CLEAR = 0;
////		private const int C_FRINGE_CONTEXT_COLOR_SEP1 = 1;
////		private const int C_FRINGE_CONTEXT_COLOR_ADD_COLOR = 2;
////		private const int C_FRINGE_CONTEXT_COLOR_ADD_FILTER = 3;
////		private const int C_FRINGE_CONTEXT_COLOR_SEP2 = 4;
////		private const int C_FRINGE_CONTEXT_COLOR_DELETE_COLOR = 5;
////
////		private const int C_FRINGE_CONTEXT_ALL_COLOR_CLEAR = 0;
////		private const int C_FRINGE_CONTEXT_ALL_COLOR_SEP1 = 1;
////		private const int C_FRINGE_CONTEXT_ALL_COLOR_ADD_FILTER = 2;
////
////		private const int C_FRINGE_CONTEXT_FILTER_ADD_FILTER = 0;
////		private const int C_FRINGE_CONTEXT_FILTER_SEP1 = 1;
////		private const int C_FRINGE_CONTEXT_FILTER_DELETE_FILTER = 2;
////
////		private const string C_FRINGE_SET_LEVEL = "SETLEVEL";
////		private const string C_FRINGE_SET_FRINGE = "SETFRINGE";
////		private const string C_FRINGE_ALL_COLOR_FRINGE = "ALLCOLORFRINGE";
////		private const string C_FRINGE_COLOR_FRINGE = "COLORFRINGE";
////		private const string C_FRINGE_SET_COLOR = "SETCOLOR";
////		private const string C_FRINGE_SET_ALL_COLOR = "SETALLCOLOR";
//
//		#endregion
//
//		#region Properties
//
////		/// <summary>
////		/// Property to hold a copy of original DataSet that is
////		/// bound to ugFringe.  Used for a data rollback.
////		/// </summary>
////		private DataSet FringeOverrideDSBackup
////		{
////			get {return _FringeOverrideDSBackup;}
////			set {_FringeOverrideDSBackup = value;}
////		}
////
////		/// <summary>
////		/// Property to hold a copy of original DataTable that is
////		/// bound to ugEquates.  Used for a data rollback.
////		/// </summary>
////		private DataTable EquateOverrideDTBackup
////		{
////			get {return _EquateOverrideDTBackup;}
////			set {_EquateOverrideDTBackup = value;}
////		}
////
////
////		/// <summary>
////		/// Property to determine if a rollback should occur for 
////		/// equates override data.
////		/// </summary>
////		private bool EquateRollback
////		{
////			get {return _EquateRollback;}
////			set {_EquateRollback = value;}
////		}
////
////		/// <summary>
////		/// Property to determine if a rollback should occur for 
////		/// fringe override data.
////		/// </summary>
////		private bool FringeRollback
////		{
////			get {return _FringeRollback;}
////			set {_FringeRollback = value;}
////		}
//
//		#endregion
//		
//		
//		/// <summary>
//		/// Required designer variable.
//		/// </summary>
//		private System.ComponentModel.Container components = null;


		#region Constructor/Dispose

		public frmSizeWarehouseMethod()
		{
			
		}

        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            ((System.ComponentModel.ISupportInitialize)(this.ugRules)).BeginInit();
            this.gbGenericSizeCurve.SuspendLayout();
            this.gbSizeCurve.SuspendLayout();
            this.gbSizeConstraints.SuspendLayout();
            this.gbGenericConstraint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxConstraint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAlternate)).BeginInit();
            this.gbSizeGroup.SuspendLayout();
            this.gbSizeAlternate.SuspendLayout();
            this.gbxNormalizeSizeCurves.SuspendLayout();
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // cboConstraints
            // 
            this.cboConstraints.Location = new System.Drawing.Point(48, 25);
            // 
            // ugRules
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugRules.DisplayLayout.Appearance = appearance1;
            this.ugRules.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugRules.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugRules.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugRules.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugRules.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugRules.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugRules.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugRules.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugRules.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugRules.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            // Begin TT#301-MD - JSmith - Controls are not functioning properly
            //this.ugRules.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugRules_InitializeLayout);
            // End TT#301-MD - JSmith - Controls are not functioning properly
            // 
            // gbSizeConstraints
            // 
            this.gbSizeConstraints.Size = new System.Drawing.Size(289, 171);
            // 
            // gbGenericConstraint
            // 
            this.gbGenericConstraint.Location = new System.Drawing.Point(15, 52);
            // 
            // picBoxConstraint
            // 
            this.picBoxConstraint.Location = new System.Drawing.Point(19, 25);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // frmSizeWarehouseMethod
            // 
            this.ClientSize = new System.Drawing.Size(656, 469);
            this.Name = "frmSizeWarehouseMethod";
            ((System.ComponentModel.ISupportInitialize)(this.ugRules)).EndInit();
            this.gbGenericSizeCurve.ResumeLayout(false);
            this.gbSizeCurve.ResumeLayout(false);
            this.gbSizeCurve.PerformLayout();
            this.gbSizeConstraints.ResumeLayout(false);
            this.gbSizeConstraints.PerformLayout();
            this.gbGenericConstraint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxConstraint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAlternate)).EndInit();
            this.gbSizeGroup.ResumeLayout(false);
            this.gbSizeAlternate.ResumeLayout(false);
            this.gbxNormalizeSizeCurves.ResumeLayout(false);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void ugRules_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        //{
        //    //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
        //    //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
        //    //ugld.ApplyDefaults(e);
        //    MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
        //    // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
        //    //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
        //    ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
        //    // End TT#1164
        //    //End TT#169
        //    // Begin TT#41-MD - GTaylor - UC #2 - hide the inherited elements
        //    lblInventoryBasis.Visible = false;
        //    cboInventoryBasis.Visible = false;
        //    // End TT#41-MD - GTaylor - UC #2
        //}
        // End TT#301-MD - JSmith - Controls are not functioning properly
//			public frmSizeWarehouseMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB)
//			{
//				try
//				{
//					_sab = SAB;
//
//					//
//					// Required for Windows Form Designer support
//					//
//					InitializeComponent();
//
//				}
//				catch(Exception ex)
//				{
//					FormLoadError = true;
//					throw ex;
//				}
//			}

			/// <summary>
			/// Clean up any resources being used.
			/// </summary>
//			protected override void Dispose( bool disposing )
//			{
//				if( disposing )
//				{
//					if(components != null)
//					{
//						components.Dispose();
//					}
//				}
//				base.Dispose( disposing );
//			}
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //if (components != null)
                //{
                //    components.Dispose();
                //}
                base.Dispose(disposing);

                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.ugRules.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugRules_InitializeLayout);
                ////Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                //ugld.DetachGridEventHandlers(ugRules);
                ////End TT#169
                // End TT#301-MD - JSmith - Controls are not functioning properly
            }
            base.Dispose(disposing);
        }
		#endregion

//		#region Windows Form Designer generated code
//		/// <summary>
//		/// Required method for Designer support - do not modify
//		/// the contents of this method with the code editor.
//		/// </summary>
//		private void InitializeComponent()
//		{
//			this.pnlGlobalUser = new System.Windows.Forms.Panel();
//			this.radGlobal = new System.Windows.Forms.RadioButton();
//			this.radUser = new System.Windows.Forms.RadioButton();
//			this.txtMethodDesc = new System.Windows.Forms.TextBox();
//			this.txtMethodName = new System.Windows.Forms.TextBox();
//			this.label3 = new System.Windows.Forms.Label();
//			this.btnClose = new System.Windows.Forms.Button();
//			this.btnSave = new System.Windows.Forms.Button();
//			this.btnProcess = new System.Windows.Forms.Button();
//			this.tabControl1 = new System.Windows.Forms.TabControl();
//			this.tabMethod = new System.Windows.Forms.TabPage();
//			this.tabControl2 = new System.Windows.Forms.TabControl();
//			this.tabGeneral = new System.Windows.Forms.TabPage();
//			this.tabConstraints = new System.Windows.Forms.TabPage();
//			this.pnlGridContainer = new System.Windows.Forms.Panel();
//			this.tabProperties = new System.Windows.Forms.TabPage();
//			this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
//			((System.ComponentModel.ISupportInitialize)(this.ugAllSize)).BeginInit();
//			this.pnlGlobalUser.SuspendLayout();
//			this.tabControl1.SuspendLayout();
//			this.tabMethod.SuspendLayout();
//			this.tabControl2.SuspendLayout();
//			this.tabGeneral.SuspendLayout();
//			this.tabConstraints.SuspendLayout();
//			this.pnlGridContainer.SuspendLayout();
//			this.tabProperties.SuspendLayout();
//			((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
//			this.SuspendLayout();
//			// 
//			// ugAllSize
//			// 
//			this.ugAllSize.DisplayLayout.ForceSerialization = true;
//			this.ugAllSize.Dock = System.Windows.Forms.DockStyle.Fill;
//			this.ugAllSize.Location = new System.Drawing.Point(0, 0);
//			this.ugAllSize.Name = "ugAllSize";
//			this.ugAllSize.Size = new System.Drawing.Size(656, 336);
//			// 
//			// chkAltSize
//			// 
//			this.chkAltSize.Location = new System.Drawing.Point(112, 88);
//			this.chkAltSize.Name = "chkAltSize";
//			// 
//			// chkSizeEquates
//			// 
//			this.chkSizeEquates.Location = new System.Drawing.Point(112, 112);
//			this.chkSizeEquates.Name = "chkSizeEquates";
//			// 
//			// lblSizeGroup
//			// 
//			this.lblSizeGroup.Location = new System.Drawing.Point(16, 64);
//			this.lblSizeGroup.Name = "lblSizeGroup";
//			// 
//			// cboSizeGroup
//			// 
//			this.cboSizeGroup.Location = new System.Drawing.Point(112, 64);
//			this.cboSizeGroup.Name = "cboSizeGroup";
//			// 
//			// lblStoreAttribute
//			// 
//			this.lblStoreAttribute.Location = new System.Drawing.Point(120, 18);
//			this.lblStoreAttribute.Name = "lblStoreAttribute";
//			// 
//			// cboStoreAttribute
//			// 
//			this.cboStoreAttribute.Location = new System.Drawing.Point(208, 16);
//			this.cboStoreAttribute.Name = "cboStoreAttribute";
//			// 
//			// cboFilter
//			// 
//			this.cboFilter.Location = new System.Drawing.Point(112, 16);
//			this.cboFilter.Name = "cboFilter";
//			// 
//			// lblFilter
//			// 
//			this.lblFilter.Location = new System.Drawing.Point(16, 16);
//			this.lblFilter.Name = "lblFilter";
//			// 
//			// chkFringe
//			// 
//			this.chkFringe.Location = new System.Drawing.Point(112, 136);
//			this.chkFringe.Name = "chkFringe";
//			// 
//			// cboSizeCurve
//			// 
//			this.cboSizeCurve.Location = new System.Drawing.Point(112, 40);
//			this.cboSizeCurve.Name = "cboSizeCurve";
//			// 
//			// lblSizeCurve
//			// 
//			this.lblSizeCurve.Location = new System.Drawing.Point(16, 40);
//			this.lblSizeCurve.Name = "lblSizeCurve";
//			// 
//			// pnlGlobalUser
//			// 
//			this.pnlGlobalUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//			this.pnlGlobalUser.Controls.Add(this.radGlobal);
//			this.pnlGlobalUser.Controls.Add(this.radUser);
//			this.pnlGlobalUser.Location = new System.Drawing.Point(576, 8);
//			this.pnlGlobalUser.Name = "pnlGlobalUser";
//			this.pnlGlobalUser.Size = new System.Drawing.Size(136, 32);
//			this.pnlGlobalUser.TabIndex = 33;
//			// 
//			// radGlobal
//			// 
//			this.radGlobal.Location = new System.Drawing.Point(16, 8);
//			this.radGlobal.Name = "radGlobal";
//			this.radGlobal.Size = new System.Drawing.Size(56, 20);
//			this.radGlobal.TabIndex = 0;
//			this.radGlobal.Text = "Global";
//			// 
//			// radUser
//			// 
//			this.radUser.Location = new System.Drawing.Point(80, 8);
//			this.radUser.Name = "radUser";
//			this.radUser.Size = new System.Drawing.Size(56, 20);
//			this.radUser.TabIndex = 1;
//			this.radUser.Text = "User";
//			// 
//			// txtMethodDesc
//			// 
//			this.txtMethodDesc.Location = new System.Drawing.Point(256, 16);
//			this.txtMethodDesc.Name = "txtMethodDesc";
//			this.txtMethodDesc.Size = new System.Drawing.Size(310, 20);
//			this.txtMethodDesc.TabIndex = 32;
//			this.txtMethodDesc.Text = "";
//			// 
//			// txtMethodName
//			// 
//			this.txtMethodName.Location = new System.Drawing.Point(56, 16);
//			this.txtMethodName.Name = "txtMethodName";
//			this.txtMethodName.Size = new System.Drawing.Size(190, 20);
//			this.txtMethodName.TabIndex = 31;
//			this.txtMethodName.Text = "";
//			// 
//			// label3
//			// 
//			this.label3.AutoSize = true;
//			this.label3.Location = new System.Drawing.Point(8, 16);
//			this.label3.Name = "label3";
//			this.label3.Size = new System.Drawing.Size(42, 16);
//			this.label3.TabIndex = 30;
//			this.label3.Text = "Method";
//			// 
//			// btnClose
//			// 
//			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//			this.btnClose.Location = new System.Drawing.Point(632, 504);
//			this.btnClose.Name = "btnClose";
//			this.btnClose.TabIndex = 36;
//			this.btnClose.Text = "Close";
//			// 
//			// btnSave
//			// 
//			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//			this.btnSave.Location = new System.Drawing.Point(552, 504);
//			this.btnSave.Name = "btnSave";
//			this.btnSave.TabIndex = 35;
//			this.btnSave.Text = "Save";
//			// 
//			// btnProcess
//			// 
//			this.btnProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
//			this.btnProcess.Location = new System.Drawing.Point(16, 504);
//			this.btnProcess.Name = "btnProcess";
//			this.btnProcess.TabIndex = 34;
//			this.btnProcess.Text = "Process";
//			// 
//			// tabControl1
//			// 
//			this.tabControl1.Controls.Add(this.tabMethod);
//			this.tabControl1.Controls.Add(this.tabProperties);
//			this.tabControl1.Location = new System.Drawing.Point(8, 48);
//			this.tabControl1.Name = "tabControl1";
//			this.tabControl1.SelectedIndex = 0;
//			this.tabControl1.Size = new System.Drawing.Size(704, 448);
//			this.tabControl1.TabIndex = 37;
//			// 
//			// tabMethod
//			// 
//			this.tabMethod.Controls.Add(this.tabControl2);
//			this.tabMethod.Location = new System.Drawing.Point(4, 22);
//			this.tabMethod.Name = "tabMethod";
//			this.tabMethod.Size = new System.Drawing.Size(696, 422);
//			this.tabMethod.TabIndex = 0;
//			this.tabMethod.Text = "Method";
//			// 
//			// tabControl2
//			// 
//			this.tabControl2.Controls.Add(this.tabGeneral);
//			this.tabControl2.Controls.Add(this.tabConstraints);
//			this.tabControl2.Location = new System.Drawing.Point(8, 8);
//			this.tabControl2.Name = "tabControl2";
//			this.tabControl2.SelectedIndex = 0;
//			this.tabControl2.Size = new System.Drawing.Size(680, 408);
//			this.tabControl2.TabIndex = 0;
//			// 
//			// tabGeneral
//			// 
//			this.tabGeneral.Controls.Add(this.chkAltSize);
//			this.tabGeneral.Controls.Add(this.chkFringe);
//			this.tabGeneral.Controls.Add(this.chkSizeEquates);
//			this.tabGeneral.Controls.Add(this.lblSizeCurve);
//			this.tabGeneral.Controls.Add(this.cboSizeCurve);
//			this.tabGeneral.Controls.Add(this.cboSizeGroup);
//			this.tabGeneral.Controls.Add(this.lblSizeGroup);
//			this.tabGeneral.Controls.Add(this.lblFilter);
//			this.tabGeneral.Controls.Add(this.cboFilter);
//			this.tabGeneral.Location = new System.Drawing.Point(4, 22);
//			this.tabGeneral.Name = "tabGeneral";
//			this.tabGeneral.Size = new System.Drawing.Size(672, 382);
//			this.tabGeneral.TabIndex = 0;
//			this.tabGeneral.Text = "General";
//			// 
//			// tabConstraints
//			// 
//			this.tabConstraints.Controls.Add(this.lblStoreAttribute);
//			this.tabConstraints.Controls.Add(this.cboStoreAttribute);
//			this.tabConstraints.Controls.Add(this.pnlGridContainer);
//			this.tabConstraints.Location = new System.Drawing.Point(4, 22);
//			this.tabConstraints.Name = "tabConstraints";
//			this.tabConstraints.Size = new System.Drawing.Size(672, 382);
//			this.tabConstraints.TabIndex = 1;
//			this.tabConstraints.Text = "Constraints";
//			this.tabConstraints.Controls.SetChildIndex(this.pnlGridContainer, 0);
//			this.tabConstraints.Controls.SetChildIndex(this.cboStoreAttribute, 0);
//			this.tabConstraints.Controls.SetChildIndex(this.lblStoreAttribute, 0);
//			// 
//			// pnlGridContainer
//			// 
//			this.pnlGridContainer.Controls.Add(this.ugAllSize);
//			this.pnlGridContainer.Location = new System.Drawing.Point(8, 40);
//			this.pnlGridContainer.Name = "pnlGridContainer";
//			this.pnlGridContainer.Size = new System.Drawing.Size(656, 336);
//			this.pnlGridContainer.TabIndex = 0;
//			// 
//			// tabProperties
//			// 
//			this.tabProperties.Controls.Add(this.ugWorkflows);
//			this.tabProperties.Location = new System.Drawing.Point(4, 22);
//			this.tabProperties.Name = "tabProperties";
//			this.tabProperties.Size = new System.Drawing.Size(696, 422);
//			this.tabProperties.TabIndex = 1;
//			this.tabProperties.Text = "Properties";
//			// 
//			// ugWorkflows
//			// 
//			this.ugWorkflows.Location = new System.Drawing.Point(16, 16);
//			this.ugWorkflows.Name = "ugWorkflows";
//			this.ugWorkflows.Size = new System.Drawing.Size(648, 344);
//			this.ugWorkflows.TabIndex = 1;
//			this.ugWorkflows.Text = "Workflows";
//			// 
//			// frmSizeWarehouseMethod
//			// 
//			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
//			this.ClientSize = new System.Drawing.Size(720, 541);
//			this.Controls.Add(this.tabControl1);
//			this.Controls.Add(this.btnClose);
//			this.Controls.Add(this.btnSave);
//			this.Controls.Add(this.btnProcess);
//			this.Controls.Add(this.pnlGlobalUser);
//			this.Controls.Add(this.txtMethodDesc);
//			this.Controls.Add(this.txtMethodName);
//			this.Controls.Add(this.label3);
//			this.Name = "frmSizeWarehouseMethod";
//			this.Text = "SizeWarehouseMethod";
//			((System.ComponentModel.ISupportInitialize)(this.ugAllSize)).EndInit();
//			this.pnlGlobalUser.ResumeLayout(false);
//			this.tabControl1.ResumeLayout(false);
//			this.tabMethod.ResumeLayout(false);
//			this.tabControl2.ResumeLayout(false);
//			this.tabGeneral.ResumeLayout(false);
//			this.tabConstraints.ResumeLayout(false);
//			this.pnlGridContainer.ResumeLayout(false);
//			this.tabProperties.ResumeLayout(false);
//			((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
//			this.ResumeLayout(false);
//
//		}
//		#endregion

	}
}
