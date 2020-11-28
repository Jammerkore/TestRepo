namespace MIDRetail.Windows.Controls
{
    partial class StoreProfileMaintControl
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
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("StoreProfileMaintToolbar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnStoreAdd");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnStoreEdit");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnEditFields");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("mnuStores");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnStoreAdd");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreProfileMaintControl));
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnStoreEdit");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnEditFields");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.StoreProfileMaintControl_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.gridFields = new MIDRetail.Windows.Controls.MIDGridFieldEditor();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.gridStores = new MIDRetail.Windows.Controls.MIDGridControl();
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.StoreProfileMaintControl_Fill_Panel.ClientArea.SuspendLayout();
            this.StoreProfileMaintControl_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // StoreProfileMaintControl_Fill_Panel
            // 
            // 
            // StoreProfileMaintControl_Fill_Panel.ClientArea
            // 
            this.StoreProfileMaintControl_Fill_Panel.ClientArea.Controls.Add(this.gridFields);
            this.StoreProfileMaintControl_Fill_Panel.ClientArea.Controls.Add(this.ultraSplitter1);
            this.StoreProfileMaintControl_Fill_Panel.ClientArea.Controls.Add(this.gridStores);
            this.StoreProfileMaintControl_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.StoreProfileMaintControl_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StoreProfileMaintControl_Fill_Panel.Location = new System.Drawing.Point(0, 25);
            this.StoreProfileMaintControl_Fill_Panel.Name = "StoreProfileMaintControl_Fill_Panel";
            this.StoreProfileMaintControl_Fill_Panel.Size = new System.Drawing.Size(900, 501);
            this.StoreProfileMaintControl_Fill_Panel.TabIndex = 0;
            // 
            // gridFields
            // 
            this.gridFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFields.Location = new System.Drawing.Point(648, 0);
            this.gridFields.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.gridFields.Name = "gridFields";
            this.gridFields.Size = new System.Drawing.Size(252, 501);
            this.gridFields.TabIndex = 2;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.Location = new System.Drawing.Point(642, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 0;
            this.ultraSplitter1.Size = new System.Drawing.Size(6, 501);
            this.ultraSplitter1.TabIndex = 1;
            // 
            // gridStores
            // 
            this.gridStores.Dock = System.Windows.Forms.DockStyle.Left;
            this.gridStores.Location = new System.Drawing.Point(0, 0);
            this.gridStores.Name = "gridStores";
            this.gridStores.ShowColumnChooser = true;
            this.gridStores.ShowToolbar = true;
            this.gridStores.Size = new System.Drawing.Size(642, 501);
            this.gridStores.TabIndex = 0;
            // 
            // _StoreProfileMaintControl_Toolbars_Dock_Area_Left
            // 
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Left.Name = "_StoreProfileMaintControl_Toolbars_Dock_Area_Left";
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 501);
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Left.UseAppStyling = false;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool7,
            buttonTool8,
            buttonTool9});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
            ultraToolbar1.Text = "StoreProfileMaintToolbar";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            popupMenuTool2.SharedPropsInternal.Caption = "Stores";
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            buttonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance1;
            buttonTool2.SharedPropsInternal.Caption = "Add Store";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool2.SharedPropsInternal.Enabled = false;
            buttonTool2.SharedPropsInternal.ToolTipText = "Opens a window to add a new store.";
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            buttonTool5.SharedPropsInternal.AppearancesSmall.Appearance = appearance2;
            buttonTool5.SharedPropsInternal.Caption = "Edit Store";
            buttonTool5.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool5.SharedPropsInternal.Enabled = false;
            buttonTool5.SharedPropsInternal.ToolTipText = "Opens a window to edit the selected store.";
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            buttonTool6.SharedPropsInternal.AppearancesSmall.Appearance = appearance3;
            buttonTool6.SharedPropsInternal.Caption = "Edit Fields";
            buttonTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool6.SharedPropsInternal.Enabled = false;
            buttonTool6.SharedPropsInternal.ToolTipText = "Opens a window to edit the selected fields for all stores.";
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool2,
            buttonTool2,
            buttonTool5,
            buttonTool6});
            this.ultraToolbarsManager1.UseAppStyling = false;
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _StoreProfileMaintControl_Toolbars_Dock_Area_Right
            // 
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(900, 25);
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Right.Name = "_StoreProfileMaintControl_Toolbars_Dock_Area_Right";
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 501);
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Right.UseAppStyling = false;
            // 
            // _StoreProfileMaintControl_Toolbars_Dock_Area_Top
            // 
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Top.Name = "_StoreProfileMaintControl_Toolbars_Dock_Area_Top";
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(900, 25);
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Top.UseAppStyling = false;
            // 
            // _StoreProfileMaintControl_Toolbars_Dock_Area_Bottom
            // 
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 526);
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Bottom.Name = "_StoreProfileMaintControl_Toolbars_Dock_Area_Bottom";
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(900, 0);
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            this._StoreProfileMaintControl_Toolbars_Dock_Area_Bottom.UseAppStyling = false;
            // 
            // StoreProfileMaintControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StoreProfileMaintControl_Fill_Panel);
            this.Controls.Add(this._StoreProfileMaintControl_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._StoreProfileMaintControl_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._StoreProfileMaintControl_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._StoreProfileMaintControl_Toolbars_Dock_Area_Top);
            this.Name = "StoreProfileMaintControl";
            this.Size = new System.Drawing.Size(900, 526);
            this.StoreProfileMaintControl_Fill_Panel.ClientArea.ResumeLayout(false);
            this.StoreProfileMaintControl_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.Misc.UltraPanel StoreProfileMaintControl_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _StoreProfileMaintControl_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _StoreProfileMaintControl_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _StoreProfileMaintControl_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _StoreProfileMaintControl_Toolbars_Dock_Area_Top;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private MIDGridControl gridStores;
        private MIDGridFieldEditor gridFields;
    }
}
