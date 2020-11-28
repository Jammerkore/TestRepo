namespace MIDRetail.Windows.Controls
{
    partial class HelpHTMLControl
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
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("InlineBrowserToolbar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnBack");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnForward");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnBack");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpHTMLControl));
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnForward");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.HelpHTMLControl_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this._HelpHTMLControl_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._HelpHTMLControl_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._HelpHTMLControl_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._HelpHTMLControl_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.HelpHTMLControl_Fill_Panel.ClientArea.SuspendLayout();
            this.HelpHTMLControl_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // HelpHTMLControl_Fill_Panel
            // 
            // 
            // HelpHTMLControl_Fill_Panel.ClientArea
            // 
            this.HelpHTMLControl_Fill_Panel.ClientArea.Controls.Add(this.webBrowser1);
            this.HelpHTMLControl_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.HelpHTMLControl_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HelpHTMLControl_Fill_Panel.Location = new System.Drawing.Point(0, 50);
            this.HelpHTMLControl_Fill_Panel.Name = "HelpHTMLControl_Fill_Panel";
            this.HelpHTMLControl_Fill_Panel.Size = new System.Drawing.Size(453, 386);
            this.HelpHTMLControl_Fill_Panel.TabIndex = 0;
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(453, 386);
            this.webBrowser1.TabIndex = 1;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // _HelpHTMLControl_Toolbars_Dock_Area_Left
            // 
            this._HelpHTMLControl_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._HelpHTMLControl_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._HelpHTMLControl_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._HelpHTMLControl_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._HelpHTMLControl_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 50);
            this._HelpHTMLControl_Toolbars_Dock_Area_Left.Name = "_HelpHTMLControl_Toolbars_Dock_Area_Left";
            this._HelpHTMLControl_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 386);
            this._HelpHTMLControl_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool5,
            buttonTool6});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
            ultraToolbar1.ShowInToolbarList = false;
            ultraToolbar1.Text = "InlineBrowserToolbar";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            buttonTool7.SharedPropsInternal.AppearancesSmall.Appearance = appearance1;
            buttonTool7.SharedPropsInternal.Caption = "Back";
            buttonTool7.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool7.SharedPropsInternal.Enabled = false;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            buttonTool8.SharedPropsInternal.AppearancesSmall.Appearance = appearance2;
            buttonTool8.SharedPropsInternal.Caption = "Forward";
            buttonTool8.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool8.SharedPropsInternal.Enabled = false;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool7,
            buttonTool8});
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _HelpHTMLControl_Toolbars_Dock_Area_Right
            // 
            this._HelpHTMLControl_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._HelpHTMLControl_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._HelpHTMLControl_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._HelpHTMLControl_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._HelpHTMLControl_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(453, 50);
            this._HelpHTMLControl_Toolbars_Dock_Area_Right.Name = "_HelpHTMLControl_Toolbars_Dock_Area_Right";
            this._HelpHTMLControl_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 386);
            this._HelpHTMLControl_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _HelpHTMLControl_Toolbars_Dock_Area_Top
            // 
            this._HelpHTMLControl_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._HelpHTMLControl_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._HelpHTMLControl_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._HelpHTMLControl_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._HelpHTMLControl_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._HelpHTMLControl_Toolbars_Dock_Area_Top.Name = "_HelpHTMLControl_Toolbars_Dock_Area_Top";
            this._HelpHTMLControl_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(453, 50);
            this._HelpHTMLControl_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _HelpHTMLControl_Toolbars_Dock_Area_Bottom
            // 
            this._HelpHTMLControl_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._HelpHTMLControl_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._HelpHTMLControl_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._HelpHTMLControl_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._HelpHTMLControl_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 436);
            this._HelpHTMLControl_Toolbars_Dock_Area_Bottom.Name = "_HelpHTMLControl_Toolbars_Dock_Area_Bottom";
            this._HelpHTMLControl_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(453, 0);
            this._HelpHTMLControl_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // HelpHTMLControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.HelpHTMLControl_Fill_Panel);
            this.Controls.Add(this._HelpHTMLControl_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._HelpHTMLControl_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._HelpHTMLControl_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._HelpHTMLControl_Toolbars_Dock_Area_Top);
            this.Name = "HelpHTMLControl";
            this.Size = new System.Drawing.Size(453, 436);
            this.HelpHTMLControl_Fill_Panel.ClientArea.ResumeLayout(false);
            this.HelpHTMLControl_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.Misc.UltraPanel HelpHTMLControl_Fill_Panel;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _HelpHTMLControl_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _HelpHTMLControl_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _HelpHTMLControl_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _HelpHTMLControl_Toolbars_Dock_Area_Top;

    }
}
