using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for Create Master Headers Method.
	/// </summary>
	public class frmCreateMasterHeadersMethod : WorkflowMethodFormBase
	{
		#region Properties

        private System.Windows.Forms.PictureBox pictureBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ImageList Icons;

		private CreateMasterHeadersMethod _CreateMasterHeadersMethod = null;
		private int _nodeRID = -1;
        private bool _skipEdit = false;
        private bool _bypass = false;  // TT#2064-MD - AGallagher - User Header Filter can be saved in a a Global Method

		private TabPage tabMethod;
        private TabControl tabCreateMasterHeadersMethod;
        private TabPage tabProperties;
        private Label lblCreateMasterHeadersTabText;
        private UltraGrid ugWorkflows;
        private GroupBox gbxMain;
        private UltraGrid ugMerchandise;
        private UltraGrid ugOverride;
        private ContextMenu mnuGrids;
        private CheckBox cbxUseSelectedHeaders;
		
		/// <summary>
		/// Gets the id of the node.
		/// </summary>
		public int NodeRID 
		{
			get { return _nodeRID ; }
		}

		#endregion

        public frmCreateMasterHeadersMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB)
            : base(SAB, aEAB, eMIDTextCode.frm_CreateMasterHeaders, eWorkflowMethodType.Method)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserCreateMasterHeaders);
			GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalCreateMasterHeaders);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{				
				if(components != null)
				{
					components.Dispose();
				}
                this.cbxUseSelectedHeaders.CheckedChanged -= new System.EventHandler(this.cbxUseSelectedHeaders_CheckedChanged);
                this.ugOverride.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugOverride_AfterCellUpdate);
                this.ugOverride.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugOverride_InitializeLayout);
                this.ugOverride.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugOverride_AfterRowInsert);
                this.ugOverride.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugOverride_MouseEnterElement);
                this.ugMerchandise.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMerchandise_AfterCellUpdate);
                this.ugMerchandise.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugMerchandise_InitializeLayout);
                this.ugMerchandise.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugMerchandise_AfterRowInsert);
                this.ugMerchandise.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugMerchandise_MouseEnterElement);
                this.ugMerchandise.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugMerchandise_DragDrop);
                this.ugMerchandise.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugMerchandise_DragEnter);
                this.ugMerchandise.DragOver -= new System.Windows.Forms.DragEventHandler(this.ugMerchandise_DragOver);
               
				if (ApplicationTransaction != null)
				{
					ApplicationTransaction.Dispose();
				}
			}
			base.Dispose( disposing );
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridLayout ultraGridLayout1 = new Infragistics.Win.UltraWinGrid.UltraGridLayout();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridLayout ultraGridLayout2 = new Infragistics.Win.UltraWinGrid.UltraGridLayout();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.gbxMain = new System.Windows.Forms.GroupBox();
            this.ugOverride = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ugMerchandise = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.lblCreateMasterHeadersTabText = new System.Windows.Forms.Label();
            this.cbxUseSelectedHeaders = new System.Windows.Forms.CheckBox();
            this.tabCreateMasterHeadersMethod = new System.Windows.Forms.TabControl();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuGrids = new System.Windows.Forms.ContextMenu();
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabMethod.SuspendLayout();
            this.gbxMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugOverride)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugMerchandise)).BeginInit();
            this.tabCreateMasterHeadersMethod.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(608, 569);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(512, 569);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(40, 569);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // Icons
            // 
            this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.Icons.ImageSize = new System.Drawing.Size(16, 16);
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.gbxMain);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(640, 462);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // gbxMain
            // 
            this.gbxMain.Controls.Add(this.ugOverride);
            this.gbxMain.Controls.Add(this.ugMerchandise);
            this.gbxMain.Controls.Add(this.lblCreateMasterHeadersTabText);
            this.gbxMain.Controls.Add(this.cbxUseSelectedHeaders);
            this.gbxMain.Location = new System.Drawing.Point(15, 15);
            this.gbxMain.Name = "gbxMain";
            this.gbxMain.Size = new System.Drawing.Size(622, 434);
            this.gbxMain.TabIndex = 4;
            this.gbxMain.TabStop = false;
            // 
            // ugOverride
            // 
            this.ugOverride.AllowDrop = true;
            this.ugOverride.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ugOverride.DisplayLayout.AddNewBox.Hidden = false;
            this.ugOverride.DisplayLayout.AddNewBox.Prompt = " Add ...";
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugOverride.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.AddButtonCaption = " Action";
            this.ugOverride.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.ugOverride.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugOverride.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugOverride.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugOverride.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugOverride.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugOverride.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugOverride.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugOverride.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugOverride.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugOverride.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            ultraGridLayout1.AddNewBox.Hidden = false;
            this.ugOverride.Layouts.Add(ultraGridLayout1);
            this.ugOverride.Location = new System.Drawing.Point(9, 378);
            this.ugOverride.Name = "ugOverride";
            this.ugOverride.Size = new System.Drawing.Size(592, 43);
            this.ugOverride.TabIndex = 6;
            this.ugOverride.Visible = false;
            this.ugOverride.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugOverride_AfterCellUpdate);
            this.ugOverride.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugOverride_InitializeLayout);
            this.ugOverride.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugOverride_AfterRowInsert);
            this.ugOverride.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugOverride_MouseEnterElement);
            // 
            // ugMerchandise
            // 
            this.ugMerchandise.AllowDrop = true;
            this.ugMerchandise.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ugMerchandise.DisplayLayout.AddNewBox.Hidden = false;
            this.ugMerchandise.DisplayLayout.AddNewBox.Prompt = " Add ...";
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugMerchandise.DisplayLayout.Appearance = appearance7;
            ultraGridBand2.AddButtonCaption = " Action";
            this.ugMerchandise.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.ugMerchandise.DisplayLayout.InterBandSpacing = 10;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.ugMerchandise.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugMerchandise.DisplayLayout.Override.HeaderAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugMerchandise.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugMerchandise.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.ugMerchandise.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugMerchandise.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.ugMerchandise.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.ugMerchandise.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugMerchandise.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            ultraGridLayout2.AddNewBox.Hidden = false;
            this.ugMerchandise.Layouts.Add(ultraGridLayout2);
            this.ugMerchandise.Location = new System.Drawing.Point(9, 70);
            this.ugMerchandise.Name = "ugMerchandise";
            this.ugMerchandise.Size = new System.Drawing.Size(592, 351);
            this.ugMerchandise.TabIndex = 5;
            this.ugMerchandise.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMerchandise_AfterCellUpdate);
            this.ugMerchandise.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugMerchandise_InitializeLayout);
            this.ugMerchandise.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugMerchandise_AfterRowInsert);
            this.ugMerchandise.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugMerchandise_MouseEnterElement);
            this.ugMerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugMerchandise_DragDrop);
            this.ugMerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugMerchandise_DragEnter);
            this.ugMerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.ugMerchandise_DragOver);
            // 
            // lblCreateMasterHeadersTabText
            // 
            this.lblCreateMasterHeadersTabText.AutoSize = true;
            this.lblCreateMasterHeadersTabText.Location = new System.Drawing.Point(6, 16);
            this.lblCreateMasterHeadersTabText.Name = "lblCreateMasterHeadersTabText";
            this.lblCreateMasterHeadersTabText.Size = new System.Drawing.Size(480, 15);
            this.lblCreateMasterHeadersTabText.TabIndex = 3;
            this.lblCreateMasterHeadersTabText.Text = "Create Master Headers for same Style/Color Headers within the Filtered List of He" +
    "aders";
            // 
            // cbxUseSelectedHeaders
            // 
            this.cbxUseSelectedHeaders.AutoSize = true;
            this.cbxUseSelectedHeaders.Location = new System.Drawing.Point(9, 45);
            this.cbxUseSelectedHeaders.Name = "cbxUseSelectedHeaders";
            this.cbxUseSelectedHeaders.Size = new System.Drawing.Size(308, 19);
            this.cbxUseSelectedHeaders.TabIndex = 4;
            this.cbxUseSelectedHeaders.Text = "Use Selected Headers (from Allocation Workspace)";
            this.cbxUseSelectedHeaders.UseVisualStyleBackColor = true;
            this.cbxUseSelectedHeaders.CheckedChanged += new System.EventHandler(this.cbxUseSelectedHeaders_CheckedChanged);
            // 
            // tabCreateMasterHeadersMethod
            // 
            this.tabCreateMasterHeadersMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCreateMasterHeadersMethod.Controls.Add(this.tabMethod);
            this.tabCreateMasterHeadersMethod.Controls.Add(this.tabProperties);
            this.tabCreateMasterHeadersMethod.Location = new System.Drawing.Point(36, 64);
            this.tabCreateMasterHeadersMethod.Name = "tabCreateMasterHeadersMethod";
            this.tabCreateMasterHeadersMethod.SelectedIndex = 0;
            this.tabCreateMasterHeadersMethod.Size = new System.Drawing.Size(648, 488);
            this.tabCreateMasterHeadersMethod.TabIndex = 14;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(640, 462);
            this.tabProperties.TabIndex = 2;
            this.tabProperties.Text = "Properties";
            this.tabProperties.UseVisualStyleBackColor = true;
            // 
            // ugWorkflows
            // 
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugWorkflows.DisplayLayout.Appearance = appearance13;
            this.ugWorkflows.DisplayLayout.InterBandSpacing = 10;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            this.ugWorkflows.DisplayLayout.Override.CardAreaAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.TextHAlignAsString = "Left";
            appearance15.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugWorkflows.DisplayLayout.Override.HeaderAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.Override.RowAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorAppearance = appearance17;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugWorkflows.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.ugWorkflows.DisplayLayout.Override.SelectedRowAppearance = appearance18;
            this.ugWorkflows.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugWorkflows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ugWorkflows.Location = new System.Drawing.Point(0, 0);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(640, 462);
            this.ugWorkflows.TabIndex = 2;
            // 
            // frmCreateMasterHeadersMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(720, 606);
            this.Controls.Add(this.tabCreateMasterHeadersMethod);
            this.Name = "frmCreateMasterHeadersMethod";
            this.Text = "Create Master Headers Method";
            this.Controls.SetChildIndex(this.tabCreateMasterHeadersMethod, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabMethod.ResumeLayout(false);
            this.gbxMain.ResumeLayout(false);
            this.gbxMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugOverride)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugMerchandise)).EndInit();
            this.tabCreateMasterHeadersMethod.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		

		/// <summary>
		/// Opens a new Create Master Headers Method.
		/// </summary>
		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
				_CreateMasterHeadersMethod = new CreateMasterHeadersMethod(SAB,Include.NoRID);
				ABM = _CreateMasterHeadersMethod;
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.AllocationMethodsUserCreateMasterHeaders, eSecurityFunctions.AllocationMethodsGlobalCreateMasterHeaders);

				Common_Load(aParentNode.GlobalUserType);				 

			}
			catch(Exception ex)
			{
				HandleException(ex, "Create Master Headers Method Constructor");
				FormLoadError = true;
			}
		}
		/// <summary>
		/// Opens an existing Create Master Headers Method.
		/// </summary>
		/// <param name="aMethodRID">aMethodRID</param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			try
			{       
				_nodeRID = aNodeRID;
				_CreateMasterHeadersMethod = new CreateMasterHeadersMethod(SAB,aMethodRID);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.AllocationMethodsUserCreateMasterHeaders, eSecurityFunctions.AllocationMethodsGlobalCreateMasterHeaders);
			
				Common_Load(aNode.GlobalUserType);
			}
			catch (Exception err)
			{
				HandleException(err);
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Deletes a Create Master Headers Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		override public bool DeleteWorkflowMethod(int aMethodRID)
		{
			try
			{       
                _CreateMasterHeadersMethod = new CreateMasterHeadersMethod(SAB,aMethodRID);
				return Delete();
			}
			catch(DatabaseForeignKeyViolation keyVio)
			{
				throw keyVio;
			}
			catch (Exception err)
			{
				HandleException(err);
				FormLoadError = true;
			}

			return true;
		}

		/// <summary>
		/// Renames a Create Master Headers Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{
                _CreateMasterHeadersMethod = new CreateMasterHeadersMethod(SAB, aMethodRID);
				return Rename(aNewName);
			}
			catch (Exception err)
			{
				HandleException(err);
				FormLoadError = true;
			}
			return false;
		}

		/// <summary>
		/// Processes a method.
		/// </summary>
		/// <param name="aWorkflowRID">The record ID of the method</param>
		override public void ProcessWorkflowMethod(int aMethodRID)
		{
			try
			{
                _CreateMasterHeadersMethod = new CreateMasterHeadersMethod(SAB, aMethodRID);
				ProcessAction(eMethodType.CreateMasterHeaders, true);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}

		private void Common_Load(eGlobalUserType aGlobalUserType)
		{
			try
			{
				SetText();		
	
				Name = MIDText.GetTextOnly((int)eMethodType.CreateMasterHeaders);
                if (_CreateMasterHeadersMethod.Method_Change_Type == eChangeType.add)
                {
                    Format_Title(eDataState.New, eMIDTextCode.frm_CreateMasterHeaders, null);
                }
                else if (FunctionSecurity.AllowUpdate)
                {
                    Format_Title(eDataState.Updatable, eMIDTextCode.frm_CreateMasterHeaders, _CreateMasterHeadersMethod.Name);
                }
                else
                {
                    Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_CreateMasterHeaders, _CreateMasterHeadersMethod.Name);
                }

                if (FunctionSecurity.AllowExecute)
                {
                    btnProcess.Enabled = true;
                }
                else
                {
                    btnProcess.Enabled = false;
                }

                cbxUseSelectedHeaders.Checked = _CreateMasterHeadersMethod.UseSelectedHeaders;
                ugMerchandise.DataSource = _CreateMasterHeadersMethod.dtMerchandise;
                ugOverride.DataSource = _CreateMasterHeadersMethod.dtOverride;

                LoadWorkflows();

			}
			catch( Exception exception )
			{
				HandleException(exception);
			}		
		}

		private void SetText()
		{
			try
			{
                if (_CreateMasterHeadersMethod.Method_Change_Type == eChangeType.update)
                {
                    this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
                }
                else
                {
                    this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
                }

                this.lblCreateMasterHeadersTabText.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_CreateMasterHeadersTabText);
                this.cbxUseSelectedHeaders.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_CreateMasterHeadersUseSelectedText);            
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

        
		#region Properties Tab - Workflows
		/// <summary>
		/// Fill the workflow grid
		/// </summary>
		private void LoadWorkflows()
		{
			try
			{
                GetWorkflows(_CreateMasterHeadersMethod.Key, ugWorkflows);
                tabCreateMasterHeadersMethod.Controls.Remove(tabProperties);
            }
			catch(Exception ex)
			{
				HandleException(ex, "LoadWorkflows");
			}
		}
		#endregion

		#region Save Button	
		protected override void Call_btnSave_Click()
		{
			try
			{
				base.btnSave_Click();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}	
		}
		#endregion
		
	

		protected override void Call_btnProcess_Click()
		{
			try
			{
				ProcessAction(eMethodType.CreateMasterHeaders);

				// as part of the  processing we saved the info, so it should be changed to update.
				
				if (!ErrorFound)
				{
					_CreateMasterHeadersMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);			
				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "Call_btnProcess_Click");
			}
		}
	
		private bool MethodChanges()
		{
			try
			{
				//Method Name
				if (_CreateMasterHeadersMethod.Name != this.txtName.Text)
					return true;

				//Method Description
				if (_CreateMasterHeadersMethod.Method_Description != this.txtDesc.Text)
					return true;
			
				//Global and User Radio Buttons
				if (radGlobal.Checked)
				{
					if (_CreateMasterHeadersMethod.GlobalUserType != eGlobalUserType.Global)
						return true;
				}
				else
				{
					if (_CreateMasterHeadersMethod.GlobalUserType != eGlobalUserType.User)
						return true;
				}

				return false;
			}
			catch (Exception ex)
			{
				HandleException(ex);
				return false;
			}
		}
		
        #region Create Master Headers Method Changes

        private void cbxUseSelectedHeaders_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                _CreateMasterHeadersMethod.UseSelectedHeaders = cbxUseSelectedHeaders.Checked;
            }

            if (cbxUseSelectedHeaders.Checked)
            {
                ugMerchandise.Enabled = false;
                ugOverride.Enabled = false;
            }
            else
            {
                ugMerchandise.Enabled = true;
                ugOverride.Enabled = true;
            }
        }
        
		#endregion
	 
		#region WorkflowMethodFormBase Overrides 

		/// <summary>
		/// Gets if workflow or method.
		/// </summary>
		override protected eWorkflowMethodIND WorkflowMethodInd()
		{
			return eWorkflowMethodIND.Methods;	
		}

		/// <summary>
		/// Use to set the method name, description, user and global radio buttons
		/// </summary>
		override protected void SetCommonFields()
		{
			WorkflowMethodName = txtName.Text;
			WorkflowMethodDescription = txtDesc.Text;
			GlobalRadioButton = radGlobal;
			UserRadioButton = radUser;
		}

		/// <summary>
		/// Use to set the specific fields in method object before updating
		/// </summary>
		override protected void SetSpecificFields()
		{
            _CreateMasterHeadersMethod.UseSelectedHeaders = cbxUseSelectedHeaders.Checked;
            _CreateMasterHeadersMethod.dtMerchandise = (DataTable)ugMerchandise.DataSource;
            _CreateMasterHeadersMethod.dtOverride = (DataTable)ugOverride.DataSource;
		}

		/// <summary>
		/// Use to validate the fields that are specific to this method type
		/// </summary>
		override protected bool ValidateSpecificFields()
		{
            bool methodFieldsValid = true;
            string errorMessage = null;
            int rowCount = 0;


            foreach (UltraGridRow row in ugMerchandise.Rows)
            {
                bool errorFound = false;

                if (row.Cells["Merchandise"].Text.Length > 0 
                    || row.Cells["Filter"].Text.Length > 0)
                {
                    ++rowCount;
                }

                if (row.Cells["Merchandise"].Text.Length > 0 &&
                    row.Cells["Filter"].Text.Length == 0)
                {
                    errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                    errorFound = true;
                }

                if (errorFound)
                {
                    row.Cells["Filter"].Appearance.Image = ErrorImage;
                    row.Cells["Filter"].Tag = errorMessage;
                    methodFieldsValid = false;
                }
                else
                {
                    row.Cells["Merchandise"].Appearance.Image = null;
                    row.Cells["Merchandise"].Tag = null;
                    row.Cells["Filter"].Appearance.Image = null;
                    row.Cells["Filter"].Tag = null;
                }
            }

            if (rowCount == 0
                && !cbxUseSelectedHeaders.Checked)
            {
                methodFieldsValid = false;
                ErrorProvider.SetError(ugMerchandise, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MasterHeaderAtLeastOneEntryRequired));
            }
            else
            {
                ErrorProvider.SetError(ugMerchandise, string.Empty);
            }
			
			return methodFieldsValid;
		}

		/// <summary>
		/// Use to set the errors to the screen
		/// </summary>
		override protected void HandleErrors()
		{
			if (!WorkflowMethodNameValid)
			{
				ErrorProvider.SetError (txtName,WorkflowMethodNameMessage);
			}
			else
			{
				ErrorProvider.SetError (txtName,string.Empty);
			}
			if (!WorkflowMethodDescriptionValid)
			{
				ErrorProvider.SetError (txtDesc,WorkflowMethodDescriptionMessage);
			}
			else
			{
				ErrorProvider.SetError (txtDesc,string.Empty);
			}
			if (!UserGlobalValid)
			{
				ErrorProvider.SetError (pnlGlobalUser,UserGlobalMessage);
			}
			else
			{
				ErrorProvider.SetError (pnlGlobalUser,string.Empty);
			}
		}

		/// <summary>
		/// Use to set the specific method object before updating
		/// </summary>
		override protected void SetObject()
		{
			try
			{
				ABM = _CreateMasterHeadersMethod;
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		/// <summary>
		/// Use to return the explorer node selected when form was opened
		/// </summary>
		override protected MIDWorkflowMethodTreeNode GetExplorerNode()
		{
			return ExplorerNode;
		}

        // Begin TT#2062-MD - JSmith - "Can not call base method" error when change from Global to User
		override protected void BuildAttributeList()
        {
            // BEGIN TT#2064-MD - AGallagher - User Header Filter can be saved in a a Global Method
            PopulateMerchandiseValueLists();
            if (radGlobal.Checked)
            {
                _bypass = true;
                foreach (UltraGridRow gridRow in ugMerchandise.Rows)
                {
                    gridRow.Cells["Filter"].Value = Include.NoRID;
                }
                _bypass = false;
            }
            // END TT#2064-MD - AGallagher - User Header Filter can be saved in a a Global Method
        }
		// End TT#2062-MD - JSmith - "Can not call base method" error when change from Global to User

		#endregion WorkflowMethodFormBase Overrides		

		#region IFormBase Members
		override public void ISave()
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				SaveChanges();
			}		
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}
		#endregion

		override protected bool ApplySecurity()	// Track 5871 stodd
		{
			bool securityOk = true; // track #5871 stodd
			if (FunctionSecurity.AllowUpdate)
			{
				btnSave.Enabled = true;
			}
			else
			{
				btnSave.Enabled = false;
			}

			if (FunctionSecurity.AllowExecute)
			{
				btnProcess.Enabled = true;
			}
			else
			{
				btnProcess.Enabled = false;
			}
			return securityOk;	// Track 5871 stodd
		}

        private void ugWorkflows_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
        }

        private void ugMerchandise_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            AddMerchandiseValueLists();
            MerchandiseGridLayout();
            PopulateMerchandiseValueLists();
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            BuildGridContextMenu();
        }

        private void ugOverride_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            AddOverrideValueLists();
            OverrideGridLayout();
            PopulateOverrideValueLists();
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
        }

        private void AddMerchandiseValueLists()
        {
            try
            {
                this.ugMerchandise.DisplayLayout.ValueLists.Clear();
                this.ugMerchandise.DisplayLayout.ValueLists.Add("Filter");

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void AddOverrideValueLists()
        {
            try
            {
                this.ugOverride.DisplayLayout.ValueLists.Clear();
                this.ugOverride.DisplayLayout.ValueLists.Add("Field");

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void PopulateMerchandiseValueLists()
        {
            try
            {
                Infragistics.Win.ValueListItem valListItem;

                this.ugMerchandise.DisplayLayout.ValueLists["Filter"].ValueListItems.Clear();

                FilterData filterData = new FilterData();
                DataTable dtHeaderFilters;
                if (FunctionSecurity.IsReadOnly) //readonly
                {
                    //load all the header filters so anyone can view anything
                    dtHeaderFilters = filterData.ReadFiltersForType(filterTypes.HeaderFilter);
                }
                else if (radGlobal.Checked) //global
                {
                    ArrayList userRIDList = new ArrayList();
                    userRIDList.Add(Include.GlobalUserRID);
                    dtHeaderFilters = filterData.FilterRead(filterTypes.HeaderFilter, eProfileType.FilterHeader, userRIDList);
                }
                else //user
                {
                    ArrayList userRIDList = new ArrayList();
                    userRIDList.Add(Include.GlobalUserRID);
                    userRIDList.Add(SAB.ClientServerSession.UserRID);
                    dtHeaderFilters = filterData.FilterRead(filterTypes.HeaderFilter, eProfileType.FilterHeader, userRIDList);
                }

                foreach (DataRow row in dtHeaderFilters.Rows)
                {
                    valListItem = new Infragistics.Win.ValueListItem();
                    valListItem.DataValue = Convert.ToInt32(row["FILTER_RID"]);
                    valListItem.DisplayText = Convert.ToString(row["FILTER_NAME"]);
                    this.ugMerchandise.DisplayLayout.ValueLists["Filter"].ValueListItems.Add(valListItem);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void PopulateOverrideValueLists()
        {
            try
            {
                Infragistics.Win.ValueListItem valListItem;

                this.ugOverride.DisplayLayout.ValueLists["Field"].ValueListItems.Clear();

                HeaderCharList hcl = new HeaderCharList();
                foreach (HeaderFieldCharEntry hcfe in hcl.BuildHeaderCharList())
                {
                    valListItem = new Infragistics.Win.ValueListItem();
                    valListItem.DataValue = hcfe.Key;
                    valListItem.DisplayText = hcfe.Text;
                    valListItem.Tag = hcfe;
                    this.ugOverride.DisplayLayout.ValueLists["Field"].ValueListItems.Add(valListItem);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void MerchandiseGridLayout()
        {
            try
            {
                this.ugMerchandise.DisplayLayout.AddNewBox.Hidden = false;
                this.ugMerchandise.DisplayLayout.GroupByBox.Hidden = true;
                this.ugMerchandise.DisplayLayout.GroupByBox.Prompt = string.Empty;
                this.ugMerchandise.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                this.ugMerchandise.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_CreateMasterHeadersAddMerchandise);
                this.ugMerchandise.DisplayLayout.Bands[0].Override.RowSelectors = DefaultableBoolean.True;

                this.ugMerchandise.DisplayLayout.Bands[0].Columns["METHOD_RID"].Hidden = true;
                this.ugMerchandise.DisplayLayout.Bands[0].Columns["HN_RID"].Hidden = true;
                this.ugMerchandise.DisplayLayout.Bands[0].Columns["SEQ"].Hidden = true;
                this.ugMerchandise.DisplayLayout.Bands[0].Columns["Merchandise"].Width = 250;
                this.ugMerchandise.DisplayLayout.Bands[0].Columns["FILTER_RID"].Hidden = true;
                this.ugMerchandise.DisplayLayout.Bands[0].Columns["Filter"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.ugMerchandise.DisplayLayout.Bands[0].Columns["Filter"].Width = 250;
                this.ugMerchandise.DisplayLayout.Bands[0].Columns["Filter"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                this.ugMerchandise.DisplayLayout.Bands[0].Columns["Filter"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                this.ugMerchandise.DisplayLayout.Bands[0].Columns["Filter"].ValueList = ugMerchandise.DisplayLayout.ValueLists["Filter"];

                this.ugMerchandise.DisplayLayout.CaptionVisible = DefaultableBoolean.True;
                this.ugMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_CreateMasterHeadersMerchandiseGrid);
                this.ugMerchandise.DisplayLayout.Bands[0].Columns["Merchandise"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_CreateMasterHeadersMerchandise);
                this.ugMerchandise.DisplayLayout.Bands[0].Columns["Filter"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_CreateMasterHeadersFilter);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void OverrideGridLayout()
        {
            try
            {
                this.ugOverride.DisplayLayout.AddNewBox.Hidden = false;
                this.ugOverride.DisplayLayout.GroupByBox.Hidden = true;
                this.ugOverride.DisplayLayout.GroupByBox.Prompt = string.Empty;
                this.ugOverride.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
                this.ugOverride.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_CreateMasterHeadersAddOverride);
                this.ugOverride.DisplayLayout.Bands[0].Override.RowSelectors = DefaultableBoolean.True;

                this.ugOverride.DisplayLayout.Bands[0].Columns["METHOD_RID"].Hidden = true;
                this.ugOverride.DisplayLayout.Bands[0].Columns["SEQ"].Hidden = true;
                this.ugOverride.DisplayLayout.Bands[0].Columns["OVERRIDE_TYPE"].Hidden = true;
                this.ugOverride.DisplayLayout.Bands[0].Columns["HCG_RID"].Hidden = true;
                this.ugOverride.DisplayLayout.Bands[0].Columns["HEADER_FIELD"].Hidden = true;
                this.ugOverride.DisplayLayout.Bands[0].Columns["HEADER_CHAR_FIELD"].Width = 250;
                this.ugOverride.DisplayLayout.Bands[0].Columns["HEADER_CHAR_FIELD"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                this.ugOverride.DisplayLayout.Bands[0].Columns["OVERRIDE_VALUE"].Width = 250;
                this.ugOverride.DisplayLayout.Bands[0].Columns["HEADER_CHAR_FIELD"].AutoCompleteMode = Infragistics.Win.AutoCompleteMode.SuggestAppend;
                this.ugOverride.DisplayLayout.Bands[0].Columns["HEADER_CHAR_FIELD"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                this.ugOverride.DisplayLayout.Bands[0].Columns["HEADER_CHAR_FIELD"].ValueList = ugOverride.DisplayLayout.ValueLists["Field"];

                this.ugOverride.DisplayLayout.CaptionVisible = DefaultableBoolean.True;
                this.ugOverride.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_CreateMasterHeadersOverrideGrid);
                this.ugOverride.DisplayLayout.Bands[0].Columns["HEADER_CHAR_FIELD"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_CreateMasterHeadersOverrideHeaderChar);
                this.ugOverride.DisplayLayout.Bands[0].Columns["OVERRIDE_VALUE"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_CreateMasterHeadersOverrideValue);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugMerchandise_AfterCellUpdate(object sender, CellEventArgs e)
        {
            string errorMessage = string.Empty, productID;

            try
            {
                if (_skipEdit)
                {
                    _skipEdit = false;
                    return;
                }

                switch (e.Cell.Column.Key)
                {
                    case "Filter":
                    {
                        _skipEdit = true;
                        if (Convert.ToInt32(e.Cell.Value) == Include.NoRID)
                        {
                            e.Cell.Row.Cells["FILTER_RID"].Value = System.DBNull.Value;
                            // BEGIN TT#2064-MD - AGallagher - User Header Filter can be saved in a a Global Method
                            if (_bypass)
                            { 
                                e.Cell.Value = string.Empty;  
                            }
                            // END TT#2064-MD - AGallagher - User Header Filter can be saved in a a Global Method
                        }
                        else
                        {
                            e.Cell.Row.Cells["FILTER_RID"].Value = Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture);
                        }
                        // BEGIN TT#2064-MD - AGallagher - User Header Filter can be saved in a a Global Method
                        if (!_bypass)
                        {
                        // END TT#2064-MD - AGallagher - User Header Filter can be saved in a a Global Method
                            int filterRID = (int)ugMerchandise.ActiveCell.ValueListResolved.GetValue(ugMerchandise.ActiveCell.ValueListResolved.SelectedItemIndex);
                            ugMerchandise.ActiveCell.Row.Cells["FILTER_RID"].Value = filterRID;
                        }  // TT#2064-MD - AGallagher - User Header Filter can be saved in a a Global Method

                        break;
                    }
                    case "Merchandise":
					{
                        productID = e.Cell.Value.ToString().Trim();
                        if (productID.Length == 0)
                        {
                            e.Cell.Row.Cells["HN_RID"].Value = Include.NoRID;
                            _skipEdit = true;
                            e.Cell.Value = string.Empty;
                        }
                        else
                        {
                            HierarchyNodeProfile hnp = GetNodeProfile(e.Cell.Text);
                            if (hnp.Key == Include.NoRID)
                            {
                                errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
                                    productID);
                                MessageBox.Show(errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                e.Cell.Row.Cells["HN_RID"].Value = hnp.Key;
                                _skipEdit = true;
                                e.Cell.Value = hnp.Text;
                            }
                        }

                        break;
					}
                }

                if (FormLoaded)
                {
                    ChangePending = true;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private HierarchyNodeProfile GetNodeProfile(string aProductID)
        {
            string desc = string.Empty;
            try
            {
                string productID = aProductID.Trim();
                string[] pArray = productID.Split(new char[] { '[' });
                productID = pArray[0].Trim();
                HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                EditMsgs em = new EditMsgs();
                return hm.NodeLookup(ref em, productID, false);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        private void ugOverride_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                switch (e.Cell.Column.Key)
                {
                    case "HEADER_CHAR_FIELD":

                        if (Convert.ToInt32(e.Cell.Value) == Include.NoRID)
                        {
                            e.Cell.Row.Cells["HCG_RID"].Value = System.DBNull.Value;
                        }
                        else if (Convert.ToInt32(e.Cell.Value) < 1)  // is header field
                        {
                            e.Cell.Row.Cells["HEADER_FIELD"].Value = Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture);
                            e.Cell.Row.Cells["OVERRIDE_TYPE"].Value = 'H';
                        }
                        else
                        {
                            e.Cell.Row.Cells["HCG_RID"].Value = Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture);
                            e.Cell.Row.Cells["OVERRIDE_TYPE"].Value = 'C';
                        }

                        break;
                }

                if (FormLoaded)
                {
                    ChangePending = true;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugMerchandise_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void ugMerchandise_DragOver(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
            try
            {
                Image_DragOver(sender, e);
                if (!FunctionSecurity.AllowUpdate)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                Infragistics.Win.UIElement aUIElement;

                aUIElement = ugMerchandise.DisplayLayout.UIElement.ElementFromPoint(ugMerchandise.PointToClient(new Point(e.X, e.Y)));

                if (aUIElement == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                UltraGridRow aRow;
                aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow));

                if (aRow == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                UltraGridCell aCell = (UltraGridCell)aUIElement.GetContext(typeof(UltraGridCell));
                if (aCell == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                if (aCell == aRow.Cells["Merchandise"] && FunctionSecurity.AllowUpdate)
                {
                    if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                    {
                        cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                        if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                        {
                            e.Effect = DragDropEffects.All;
                        }
                    }
                }
                else if (aCell == aRow.Cells["Filter"] && FunctionSecurity.AllowUpdate)
                {
                    if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                    {
                        cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                        if (cbList.ClipboardDataType == eProfileType.FilterHeader)
                        {
                            e.Effect = DragDropEffects.All;
                        }
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ugMerchandise_DragDrop(object sender, DragEventArgs e)
        {
            HierarchyNodeProfile hnp;
            TreeNodeClipboardList cbList = null;
            try
            {
                Infragistics.Win.UIElement aUIElement;

                aUIElement = ugMerchandise.DisplayLayout.UIElement.ElementFromPoint(ugMerchandise.PointToClient(new Point(e.X, e.Y)));

                if (aUIElement == null)
                {
                    return;
                }

                UltraGridRow aRow;
                aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow));

                if (aRow == null)
                {
                    return;
                }

                UltraGridCell aCell = (UltraGridCell)aUIElement.GetContext(typeof(UltraGridCell));
                if (aCell == null)
                {
                    return;
                }

                if (aCell == aRow.Cells["Merchandise"])
                {
                    //// Create a new instance of the DataObject interface.

                    if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                    {
                        cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                        if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                        {
                            _skipEdit = true;
                            hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                            aCell.Value = hnp.Text;
                            aRow.Cells["HN_RID"].Value = hnp.Key;
                            _skipEdit = false;

                        }
                        else
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                        }
                    }
                    else
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                    }
                }
                else if (aCell == aRow.Cells["Filter"])
                {
                    //// Create a new instance of the DataObject interface.

                    if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                    {
                        cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                        if (cbList.ClipboardDataType == eProfileType.FilterHeader)
                        {
                            _skipEdit = true;
                            aCell.Value = cbList.ClipboardProfile.Key;
                            aRow.Cells["FILTER_RID"].Value = cbList.ClipboardProfile.Key;
                            _skipEdit = false;

                        }
                        else
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                        }
                    }
                    else
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugMerchandise_AfterRowInsert(object sender, RowEventArgs e)
        {
            e.Row.Cells["METHOD_RID"].Value = _CreateMasterHeadersMethod.Key;
            e.Row.Cells["SEQ"].Value = ugMerchandise.Rows.Count;
            e.Row.Cells["HN_RID"].Value = Include.NoRID;
        }

        private void ugOverride_AfterRowInsert(object sender, RowEventArgs e)
        {
            e.Row.Cells["METHOD_RID"].Value = _CreateMasterHeadersMethod.Key;
            e.Row.Cells["SEQ"].Value = ugOverride.Rows.Count;
        }

        private void ugMerchandise_MouseEnterElement(object sender, UIElementEventArgs e)
        {
            ShowUltraGridToolTip(ugMerchandise, e);
        }

        private void ugOverride_MouseEnterElement(object sender, UIElementEventArgs e)
        {
            ShowUltraGridToolTip(ugOverride, e);
        }

        private void BuildGridContextMenu()
        {
            MenuItem mnuItemInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
            MenuItem mnuItemInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
            MenuItem mnuItemInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
            MenuItem mnuItemDelete = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
            MenuItem mnuItemDeleteAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteAll));

            mnuGrids.MenuItems.Add(mnuItemInsert);
            mnuItemInsert.MenuItems.Add(mnuItemInsertBefore);
            mnuItemInsert.MenuItems.Add(mnuItemInsertAfter);
            mnuGrids.MenuItems.Add(mnuItemDelete);
            mnuGrids.MenuItems.Add(mnuItemDeleteAll);

            mnuItemInsert.Click += new System.EventHandler(this.mnuGridsItemInsert_Click);
            mnuItemInsertBefore.Click += new System.EventHandler(this.mnuGridsItemInsertBefore_Click);
            mnuItemInsertAfter.Click += new System.EventHandler(this.mnuGridsItemInsertAfter_Click);
            mnuItemDelete.Click += new System.EventHandler(this.mnuGridsItemDelete_Click);
            mnuItemDeleteAll.Click += new System.EventHandler(this.mnuGridsItemDeleteAll_Click);

            ugMerchandise.ContextMenu = mnuGrids;
        }

        private void mnuGridsItemInsert_Click(object sender, System.EventArgs e)
        {
        }

        private void mnuGridsItemInsertBefore_Click(object sender, System.EventArgs e)
        {
            try
            {
                int rowPosition = 0;
                if (ugMerchandise.Rows.Count > 0)
                {
                    if (this.ugMerchandise.ActiveRow == null) return;
                    rowPosition = Convert.ToInt32(this.ugMerchandise.ActiveRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture);
                    // increment the position of the active row to end of grid
                    foreach (UltraGridRow gridRow in ugMerchandise.Rows)
                    {
                        if (Convert.ToInt32(gridRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture) >= rowPosition)
                        {
                            gridRow.Cells["SEQ"].Value = Convert.ToInt32(gridRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture) + 1;
                        }
                    }
                }
                UltraGridRow addedRow = this.ugMerchandise.DisplayLayout.Bands[0].AddNew();
                addedRow.Cells["SEQ"].Value = rowPosition;
                this.ugMerchandise.DisplayLayout.Bands[0].SortedColumns.Clear();
                this.ugMerchandise.DisplayLayout.Bands[0].SortedColumns.Add("SEQ", false);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void mnuGridsItemInsertAfter_Click(object sender, System.EventArgs e)
        {
            try
            {
                int rowPosition = 0;
                if (ugMerchandise.Rows.Count > 0)
                {
                    if (this.ugMerchandise.ActiveRow == null) return;
                    rowPosition = Convert.ToInt32(this.ugMerchandise.ActiveRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture);
                    // increment the position of the active row to end of grid
                    foreach (UltraGridRow gridRow in ugMerchandise.Rows)
                    {
                        if (Convert.ToInt32(gridRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture) > rowPosition)
                        {
                            gridRow.Cells["SEQ"].Value = Convert.ToInt32(gridRow.Cells["SEQ"].Value, CultureInfo.CurrentUICulture) + 1;
                        }
                    }
                }
                UltraGridRow addedRow = this.ugMerchandise.DisplayLayout.Bands[0].AddNew();
                addedRow.Cells["SEQ"].Value = rowPosition + 1;
                this.ugMerchandise.DisplayLayout.Bands[0].SortedColumns.Clear();
                this.ugMerchandise.DisplayLayout.Bands[0].SortedColumns.Add("SEQ", false);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void mnuGridsItemDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                ugMerchandise.DeleteSelectedRows();
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void mnuGridsItemDeleteAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                _CreateMasterHeadersMethod.dtMerchandise.Clear();
                _CreateMasterHeadersMethod.dtMerchandise.AcceptChanges();
                ChangePending = true;
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
	}


    
}