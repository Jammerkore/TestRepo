namespace MIDRetail.Windows.Controls
{
    partial class ReportAllocationByStore
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
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("EmailMessageToolbar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reportView");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("reportView");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportAllocationByStore));
            this.ReportUserOptionsReview_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.cboStore = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblStore = new System.Windows.Forms.Label();
            this.lblTypes = new System.Windows.Forms.Label();
            this.menuListBox = new System.Windows.Forms.ContextMenu();
            this.mniSelectAll = new System.Windows.Forms.MenuItem();
            this.mniClearAll = new System.Windows.Forms.MenuItem();
            this.lstTypes = new System.Windows.Forms.CheckedListBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lstStatus = new System.Windows.Forms.CheckedListBox();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._EmailMessageControl_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._EmailMessageControl_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._EmailMessageControl_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.SuspendLayout();
            this.ReportUserOptionsReview_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ReportUserOptionsReview_Fill_Panel
            // 
            // 
            // ReportUserOptionsReview_Fill_Panel.ClientArea
            // 
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.Controls.Add(this.cboStore);
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.Controls.Add(this.lblStore);
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.Controls.Add(this.lblTypes);
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.Controls.Add(this.lstTypes);
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.Controls.Add(this.lblStatus);
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.Controls.Add(this.lstStatus);
            this.ReportUserOptionsReview_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ReportUserOptionsReview_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReportUserOptionsReview_Fill_Panel.Location = new System.Drawing.Point(0, 25);
            this.ReportUserOptionsReview_Fill_Panel.Name = "ReportUserOptionsReview_Fill_Panel";
            this.ReportUserOptionsReview_Fill_Panel.Size = new System.Drawing.Size(394, 325);
            this.ReportUserOptionsReview_Fill_Panel.TabIndex = 0;
            // 
            // cboStore
            // 
            this.cboStore.AllowDrop = true;
            this.cboStore.AllowUserAttributes = false;
            this.cboStore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStore.FormattingEnabled = true;
            this.cboStore.Location = new System.Drawing.Point(82, 6);
            this.cboStore.Name = "cboStore";
            this.cboStore.Size = new System.Drawing.Size(297, 21);
            this.cboStore.TabIndex = 12;
            // 
            // lblStore
            // 
            this.lblStore.Location = new System.Drawing.Point(41, 8);
            this.lblStore.Name = "lblStore";
            this.lblStore.Size = new System.Drawing.Size(80, 16);
            this.lblStore.TabIndex = 11;
            this.lblStore.Text = "Store:";
            // 
            // lblTypes
            // 
            this.lblTypes.Location = new System.Drawing.Point(10, 41);
            this.lblTypes.Name = "lblTypes";
            this.lblTypes.Size = new System.Drawing.Size(100, 16);
            this.lblTypes.TabIndex = 10;
            this.lblTypes.Text = "Types";
            this.lblTypes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // menuListBox
            // 
            this.menuListBox.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniSelectAll,
            this.mniClearAll});
            // 
            // mniSelectAll
            // 
            this.mniSelectAll.Index = 0;
            this.mniSelectAll.Text = "Select All";
            this.mniSelectAll.Click += new System.EventHandler(this.mniSelectAll_Click);
            // 
            // mniClearAll
            // 
            this.mniClearAll.Index = 1;
            this.mniClearAll.Text = "Clear All";
            this.mniClearAll.Click += new System.EventHandler(this.mniClearAll_Click);
            // 
            // lstTypes
            // 
            this.lstTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstTypes.CheckOnClick = true;
            this.lstTypes.ContextMenu = this.menuListBox;
            this.lstTypes.Location = new System.Drawing.Point(13, 60);
            this.lstTypes.Name = "lstTypes";
            this.lstTypes.Size = new System.Drawing.Size(144, 244);
            this.lstTypes.TabIndex = 9;
            this.lstTypes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstTypes_MouseDown);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(174, 41);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 16);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lstStatus
            // 
            this.lstStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstStatus.CheckOnClick = true;
            this.lstStatus.ContextMenu = this.menuListBox;
            this.lstStatus.Location = new System.Drawing.Point(177, 60);
            this.lstStatus.Name = "lstStatus";
            this.lstStatus.Size = new System.Drawing.Size(202, 244);
            this.lstStatus.TabIndex = 6;
            this.lstStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstStatus_MouseDown);
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
            ultraToolbar1.ShowInToolbarList = false;
            ultraToolbar1.Text = "ReportToolbar";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            this.ultraToolbarsManager1.ToolbarSettings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            this.ultraToolbarsManager1.ToolbarSettings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            this.ultraToolbarsManager1.ToolbarSettings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            this.ultraToolbarsManager1.ToolbarSettings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            this.ultraToolbarsManager1.ToolbarSettings.AllowDockTop = Infragistics.Win.DefaultableBoolean.True;
            this.ultraToolbarsManager1.ToolbarSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            this.ultraToolbarsManager1.ToolbarSettings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            buttonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance1;
            buttonTool2.SharedPropsInternal.Caption = "View Report";
            buttonTool2.SharedPropsInternal.Category = "Reports";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool2});
            this.ultraToolbarsManager1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _EmailMessageControl_Toolbars_Dock_Area_Top
            // 
            this._EmailMessageControl_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EmailMessageControl_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._EmailMessageControl_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._EmailMessageControl_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EmailMessageControl_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._EmailMessageControl_Toolbars_Dock_Area_Top.Name = "_EmailMessageControl_Toolbars_Dock_Area_Top";
            this._EmailMessageControl_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(394, 25);
            this._EmailMessageControl_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _EmailMessageControl_Toolbars_Dock_Area_Bottom
            // 
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 350);
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.Name = "_EmailMessageControl_Toolbars_Dock_Area_Bottom";
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(394, 0);
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _EmailMessageControl_Toolbars_Dock_Area_Left
            // 
            this._EmailMessageControl_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EmailMessageControl_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._EmailMessageControl_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._EmailMessageControl_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EmailMessageControl_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._EmailMessageControl_Toolbars_Dock_Area_Left.Name = "_EmailMessageControl_Toolbars_Dock_Area_Left";
            this._EmailMessageControl_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 325);
            this._EmailMessageControl_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _EmailMessageControl_Toolbars_Dock_Area_Right
            // 
            this._EmailMessageControl_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EmailMessageControl_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._EmailMessageControl_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._EmailMessageControl_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EmailMessageControl_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(394, 25);
            this._EmailMessageControl_Toolbars_Dock_Area_Right.Name = "_EmailMessageControl_Toolbars_Dock_Area_Right";
            this._EmailMessageControl_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 325);
            this._EmailMessageControl_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ReportAllocationByStore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ReportUserOptionsReview_Fill_Panel);
            this.Controls.Add(this._EmailMessageControl_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._EmailMessageControl_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._EmailMessageControl_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._EmailMessageControl_Toolbars_Dock_Area_Top);
            this.Name = "ReportAllocationByStore";
            this.Size = new System.Drawing.Size(394, 350);
            this.Load += new System.EventHandler(this.ReportAllocationByStore_Load);
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.ResumeLayout(false);
            this.ReportUserOptionsReview_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ReportUserOptionsReview_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _EmailMessageControl_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _EmailMessageControl_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _EmailMessageControl_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _EmailMessageControl_Toolbars_Dock_Area_Top;
        private System.Windows.Forms.CheckedListBox lstStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckedListBox lstTypes;
        private System.Windows.Forms.Label lblTypes;
        private System.Windows.Forms.ContextMenu menuListBox;
        private System.Windows.Forms.MenuItem mniSelectAll;
        private System.Windows.Forms.MenuItem mniClearAll;
        private MIDAttributeComboBox cboStore;
        private System.Windows.Forms.Label lblStore;
    }
}
