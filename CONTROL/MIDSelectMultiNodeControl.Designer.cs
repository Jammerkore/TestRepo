namespace MIDRetail.Windows.Controls
{
    partial class MIDSelectMultiNodeControl
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
            Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("SelectUserToolbar");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool5 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblTitle");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool6 = new Infragistics.Win.UltraWinToolbars.LabelTool(" blankLabel");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnCheckAll");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnUncheckAll");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnCheckAll");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MIDSelectMultiNodeControl));
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnUncheckAll");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool7 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblTitle");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool8 = new Infragistics.Win.UltraWinToolbars.LabelTool(" blankLabel");
            this.MIDSelectMultiNodeControl_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.ultraTree1 = new Infragistics.Win.UltraWinTree.UltraTree();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.MIDSelectMultiNodeControl_Fill_Panel.ClientArea.SuspendLayout();
            this.MIDSelectMultiNodeControl_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTree1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // MIDSelectMultiNodeControl_Fill_Panel
            // 
            // 
            // MIDSelectMultiNodeControl_Fill_Panel.ClientArea
            // 
            this.MIDSelectMultiNodeControl_Fill_Panel.ClientArea.Controls.Add(this.ultraTree1);
            this.MIDSelectMultiNodeControl_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.MIDSelectMultiNodeControl_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MIDSelectMultiNodeControl_Fill_Panel.Location = new System.Drawing.Point(0, 27);
            this.MIDSelectMultiNodeControl_Fill_Panel.Name = "MIDSelectMultiNodeControl_Fill_Panel";
            this.MIDSelectMultiNodeControl_Fill_Panel.Size = new System.Drawing.Size(402, 318);
            this.MIDSelectMultiNodeControl_Fill_Panel.TabIndex = 0;
            // 
            // ultraTree1
            // 
            this.ultraTree1.ContextMenuStrip = this.contextMenuStrip1;
            this.ultraTree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTree1.FullRowSelect = true;
            this.ultraTree1.Location = new System.Drawing.Point(0, 0);
            this.ultraTree1.Name = "ultraTree1";
            _override1.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            _override1.AllowPaste = Infragistics.Win.DefaultableBoolean.False;
            _override1.HotTracking = Infragistics.Win.DefaultableBoolean.True;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.ForeColor = System.Drawing.Color.Black;
            _override1.SelectedNodeAppearance = appearance1;
            this.ultraTree1.Override = _override1;
            this.ultraTree1.Size = new System.Drawing.Size(402, 318);
            this.ultraTree1.TabIndex = 0;
            this.ultraTree1.UseAppStyling = false;
            this.ultraTree1.AfterCheck += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.ultraTree1_AfterCheck);
            this.ultraTree1.InitializeDataNode += new Infragistics.Win.UltraWinTree.InitializeDataNodeEventHandler(this.ultraTree1_InitializeDataNode);
            this.ultraTree1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ultraTree1_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.clearAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(123, 48);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // _MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left
            // 
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left.Name = "_MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left";
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 318);
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left.UseAppStyling = false;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            labelTool6.InstanceProps.Width = 29;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            labelTool5,
            labelTool6,
            buttonTool5,
            buttonTool6});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
            ultraToolbar1.ShowInToolbarList = false;
            ultraToolbar1.Text = "SelectUserToolbar";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            buttonTool7.SharedPropsInternal.AppearancesSmall.Appearance = appearance2;
            buttonTool7.SharedPropsInternal.Caption = "Select All";
            buttonTool7.SharedPropsInternal.ToolTipText = "Select All";
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            buttonTool8.SharedPropsInternal.AppearancesSmall.Appearance = appearance3;
            buttonTool8.SharedPropsInternal.Caption = "Clear All";
            buttonTool8.SharedPropsInternal.ToolTipText = "Clear All";
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlignAsString = "Left";
            labelTool7.SharedPropsInternal.AppearancesSmall.Appearance = appearance4;
            labelTool7.SharedPropsInternal.Caption = "Select Groups && Users";
            labelTool8.SharedPropsInternal.Caption = " ";
            labelTool8.SharedPropsInternal.Spring = true;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool7,
            buttonTool8,
            labelTool7,
            labelTool8});
            this.ultraToolbarsManager1.UseAppStyling = false;
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right
            // 
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(402, 27);
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right.Name = "_MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right";
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 318);
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right.UseAppStyling = false;
            // 
            // _MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top
            // 
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top.Name = "_MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top";
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(402, 27);
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top.UseAppStyling = false;
            // 
            // _MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom
            // 
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 345);
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom.Name = "_MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom";
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(402, 0);
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom.UseAppStyling = false;
            // 
            // MIDSelectMultiNodeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MIDSelectMultiNodeControl_Fill_Panel);
            this.Controls.Add(this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top);
            this.Name = "MIDSelectMultiNodeControl";
            this.Size = new System.Drawing.Size(402, 345);
            this.Load += new System.EventHandler(this.MIDSelectMultiNodeControl_Load);
            this.MIDSelectMultiNodeControl_Fill_Panel.ClientArea.ResumeLayout(false);
            this.MIDSelectMultiNodeControl_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTree1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.Misc.UltraPanel MIDSelectMultiNodeControl_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _MIDSelectMultiNodeControl_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _MIDSelectMultiNodeControl_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _MIDSelectMultiNodeControl_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _MIDSelectMultiNodeControl_Toolbars_Dock_Area_Top;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        public Infragistics.Win.UltraWinTree.UltraTree ultraTree1;
    }
}
