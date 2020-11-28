namespace StoreBinConverter
{
    partial class StoreBinConverterForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel2 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel3 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel4 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("IsChecked");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Description");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Status");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Progress");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("UltraToolbar1");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnProcess");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnStop");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnProcess");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreBinConverterForm));
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("btnStop");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.ultraStatusBar1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.StoreBinConverterForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.txtLog = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this._StoreBinConverterForm_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._StoreBinConverterForm_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._StoreBinConverterForm_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._StoreBinConverterForm_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraTextEditor1 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.taskControlContainer1 = new StoreBinConverter.TaskControlContainer();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).BeginInit();
            this.StoreBinConverterForm_Fill_Panel.ClientArea.SuspendLayout();
            this.StoreBinConverterForm_Fill_Panel.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraStatusBar1
            // 
            this.ultraStatusBar1.Location = new System.Drawing.Point(0, 746);
            this.ultraStatusBar1.Name = "ultraStatusBar1";
            appearance1.TextHAlignAsString = "Left";
            ultraStatusPanel1.Appearance = appearance1;
            ultraStatusPanel1.Key = "pnlTimeLabel";
            ultraStatusPanel1.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Automatic;
            ultraStatusPanel1.Text = "Total Time Elapsed:";
            ultraStatusPanel1.Width = 150;
            ultraStatusPanel2.Key = "pnlTime";
            ultraStatusPanel3.Key = "pnlDBLabel";
            ultraStatusPanel3.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Automatic;
            ultraStatusPanel3.Text = "Database:";
            ultraStatusPanel4.Key = "pnlDB";
            ultraStatusPanel4.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Automatic;
            this.ultraStatusBar1.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1,
            ultraStatusPanel2,
            ultraStatusPanel3,
            ultraStatusPanel4});
            this.ultraStatusBar1.Size = new System.Drawing.Size(984, 23);
            this.ultraStatusBar1.TabIndex = 0;
            this.ultraStatusBar1.Text = "ultraStatusBar1";
            // 
            // StoreBinConverterForm_Fill_Panel
            // 
            // 
            // StoreBinConverterForm_Fill_Panel.ClientArea
            // 
            this.StoreBinConverterForm_Fill_Panel.ClientArea.Controls.Add(this.ultraPanel2);
            this.StoreBinConverterForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.StoreBinConverterForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StoreBinConverterForm_Fill_Panel.Location = new System.Drawing.Point(0, 50);
            this.StoreBinConverterForm_Fill_Panel.Name = "StoreBinConverterForm_Fill_Panel";
            this.StoreBinConverterForm_Fill_Panel.Size = new System.Drawing.Size(984, 696);
            this.StoreBinConverterForm_Fill_Panel.TabIndex = 1;
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.txtLog);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraTextEditor1);
            this.ultraPanel2.ClientArea.Controls.Add(this.splitter1);
            this.ultraPanel2.ClientArea.Controls.Add(this.ultraPanel1);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel2.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(984, 696);
            this.ultraPanel2.TabIndex = 7;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 476);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(984, 3);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // ultraPanel1
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            this.ultraPanel1.Appearance = appearance2;
            this.ultraPanel1.AutoScroll = true;
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.taskControlContainer1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(984, 476);
            this.ultraPanel1.TabIndex = 1;
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(0, 500);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Scrollbars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(984, 196);
            this.txtLog.TabIndex = 10;
            // 
            // _StoreBinConverterForm_Toolbars_Dock_Area_Left
            // 
            this._StoreBinConverterForm_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 50);
            this._StoreBinConverterForm_Toolbars_Dock_Area_Left.Name = "_StoreBinConverterForm_Toolbars_Dock_Area_Left";
            this._StoreBinConverterForm_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 696);
            this._StoreBinConverterForm_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _StoreBinConverterForm_Toolbars_Dock_Area_Right
            // 
            this._StoreBinConverterForm_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(984, 50);
            this._StoreBinConverterForm_Toolbars_Dock_Area_Right.Name = "_StoreBinConverterForm_Toolbars_Dock_Area_Right";
            this._StoreBinConverterForm_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 696);
            this._StoreBinConverterForm_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _StoreBinConverterForm_Toolbars_Dock_Area_Top
            // 
            this._StoreBinConverterForm_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._StoreBinConverterForm_Toolbars_Dock_Area_Top.Name = "_StoreBinConverterForm_Toolbars_Dock_Area_Top";
            this._StoreBinConverterForm_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(984, 50);
            this._StoreBinConverterForm_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _StoreBinConverterForm_Toolbars_Dock_Area_Bottom
            // 
            this._StoreBinConverterForm_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._StoreBinConverterForm_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 746);
            this._StoreBinConverterForm_Toolbars_Dock_Area_Bottom.Name = "_StoreBinConverterForm_Toolbars_Dock_Area_Bottom";
            this._StoreBinConverterForm_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(984, 0);
            this._StoreBinConverterForm_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraDataSource1
            // 
            ultraDataColumn1.AllowDBNull = Infragistics.Win.DefaultableBoolean.False;
            ultraDataColumn1.DataType = typeof(bool);
            ultraDataColumn1.DefaultValue = false;
            ultraDataColumn2.DefaultValue = "Step";
            ultraDataColumn4.DataType = typeof(int);
            ultraDataColumn4.DefaultValue = 0;
            ultraDataColumn4.ReadOnly = Infragistics.Win.DefaultableBoolean.True;
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3,
            ultraDataColumn4});
            // 
            // ultraTextEditor1
            // 
            this.ultraTextEditor1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraTextEditor1.Location = new System.Drawing.Point(0, 479);
            this.ultraTextEditor1.Name = "ultraTextEditor1";
            this.ultraTextEditor1.ReadOnly = true;
            this.ultraTextEditor1.Size = new System.Drawing.Size(984, 21);
            this.ultraTextEditor1.TabIndex = 3;
            this.ultraTextEditor1.Text = "Converter Log:";
            // 
            // taskControlContainer1
            // 
            this.taskControlContainer1.AutoScroll = true;
            this.taskControlContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taskControlContainer1.Location = new System.Drawing.Point(0, 0);
            this.taskControlContainer1.Name = "taskControlContainer1";
            this.taskControlContainer1.Size = new System.Drawing.Size(984, 476);
            this.taskControlContainer1.TabIndex = 1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool3});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
            ultraToolbar1.Text = "UltraToolbar1";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            buttonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance3;
            buttonTool2.SharedPropsInternal.Caption = "Process";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            buttonTool4.SharedPropsInternal.AppearancesSmall.Appearance = appearance4;
            buttonTool4.SharedPropsInternal.Caption = "Stop";
            buttonTool4.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool2,
            buttonTool4});
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // StoreBinConverterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 769);
            this.Controls.Add(this.StoreBinConverterForm_Fill_Panel);
            this.Controls.Add(this._StoreBinConverterForm_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._StoreBinConverterForm_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._StoreBinConverterForm_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this.ultraStatusBar1);
            this.Controls.Add(this._StoreBinConverterForm_Toolbars_Dock_Area_Top);
            this.MinimumSize = new System.Drawing.Size(1000, 38);
            this.Name = "StoreBinConverterForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Store Bin Converter Utility";
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).EndInit();
            this.StoreBinConverterForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.StoreBinConverterForm_Fill_Panel.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ClientArea.PerformLayout();
            this.ultraPanel2.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBar1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.Misc.UltraPanel StoreBinConverterForm_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _StoreBinConverterForm_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _StoreBinConverterForm_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _StoreBinConverterForm_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _StoreBinConverterForm_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtLog;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private TaskControlContainer taskControlContainer1;
        private System.Windows.Forms.Splitter splitter1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditor1;
    }
}

