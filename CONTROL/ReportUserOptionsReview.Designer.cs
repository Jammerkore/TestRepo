namespace MIDRetail.Windows.Controls
{
    partial class ReportUserOptionsReview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportUserOptionsReview));
            this.ReportUserOptionsReview_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radModifySalesMonitorOff = new System.Windows.Forms.RadioButton();
            this.radModifySalesMonitorOn = new System.Windows.Forms.RadioButton();
            this.chkSales = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radForecastMonitorOff = new System.Windows.Forms.RadioButton();
            this.radForecastMonitorOn = new System.Windows.Forms.RadioButton();
            this.chkForecast = new System.Windows.Forms.CheckBox();
            this.chkSpecifyAuditLevel = new System.Windows.Forms.CheckBox();
            this.cbxAuditLoggingLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblAuditLoggingLevel = new System.Windows.Forms.Label();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._EmailMessageControl_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._EmailMessageControl_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._EmailMessageControl_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.panel3 = new System.Windows.Forms.Panel();
            this.radDCFulfillmentMonitorOff = new System.Windows.Forms.RadioButton();
            this.radDCFulfillmentMonitorOn = new System.Windows.Forms.RadioButton();
            this.chkDCFulfillment = new System.Windows.Forms.CheckBox();
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.SuspendLayout();
            this.ReportUserOptionsReview_Fill_Panel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ReportUserOptionsReview_Fill_Panel
            // 
            // 
            // ReportUserOptionsReview_Fill_Panel.ClientArea
            // 
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.Controls.Add(this.panel3);
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.Controls.Add(this.panel2);
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.Controls.Add(this.panel1);
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.Controls.Add(this.chkSpecifyAuditLevel);
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.Controls.Add(this.cbxAuditLoggingLevel);
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.Controls.Add(this.lblAuditLoggingLevel);
            this.ReportUserOptionsReview_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ReportUserOptionsReview_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReportUserOptionsReview_Fill_Panel.Location = new System.Drawing.Point(0, 25);
            this.ReportUserOptionsReview_Fill_Panel.Name = "ReportUserOptionsReview_Fill_Panel";
            this.ReportUserOptionsReview_Fill_Panel.Size = new System.Drawing.Size(437, 293);
            this.ReportUserOptionsReview_Fill_Panel.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radModifySalesMonitorOff);
            this.panel2.Controls.Add(this.radModifySalesMonitorOn);
            this.panel2.Controls.Add(this.chkSales);
            this.panel2.Location = new System.Drawing.Point(13, 140);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 56);
            this.panel2.TabIndex = 32;
            // 
            // radModifySalesMonitorOff
            // 
            this.radModifySalesMonitorOff.Enabled = false;
            this.radModifySalesMonitorOff.Location = new System.Drawing.Point(86, 24);
            this.radModifySalesMonitorOff.Name = "radModifySalesMonitorOff";
            this.radModifySalesMonitorOff.Size = new System.Drawing.Size(40, 24);
            this.radModifySalesMonitorOff.TabIndex = 33;
            this.radModifySalesMonitorOff.Text = "Off";
            // 
            // radModifySalesMonitorOn
            // 
            this.radModifySalesMonitorOn.Checked = true;
            this.radModifySalesMonitorOn.Enabled = false;
            this.radModifySalesMonitorOn.Location = new System.Drawing.Point(30, 24);
            this.radModifySalesMonitorOn.Name = "radModifySalesMonitorOn";
            this.radModifySalesMonitorOn.Size = new System.Drawing.Size(40, 24);
            this.radModifySalesMonitorOn.TabIndex = 32;
            this.radModifySalesMonitorOn.TabStop = true;
            this.radModifySalesMonitorOn.Text = "On";
            // 
            // chkSales
            // 
            this.chkSales.AutoSize = true;
            this.chkSales.Location = new System.Drawing.Point(3, 3);
            this.chkSales.Name = "chkSales";
            this.chkSales.Size = new System.Drawing.Size(169, 17);
            this.chkSales.TabIndex = 31;
            this.chkSales.Text = "Specify \'Modify Sales\' Monitor:";
            this.chkSales.UseVisualStyleBackColor = true;
            this.chkSales.CheckedChanged += new System.EventHandler(this.chkSales_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radForecastMonitorOff);
            this.panel1.Controls.Add(this.radForecastMonitorOn);
            this.panel1.Controls.Add(this.chkForecast);
            this.panel1.Location = new System.Drawing.Point(13, 78);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 56);
            this.panel1.TabIndex = 31;
            // 
            // radForecastMonitorOff
            // 
            this.radForecastMonitorOff.Enabled = false;
            this.radForecastMonitorOff.Location = new System.Drawing.Point(86, 24);
            this.radForecastMonitorOff.Name = "radForecastMonitorOff";
            this.radForecastMonitorOff.Size = new System.Drawing.Size(40, 24);
            this.radForecastMonitorOff.TabIndex = 33;
            this.radForecastMonitorOff.Text = "Off";
            // 
            // radForecastMonitorOn
            // 
            this.radForecastMonitorOn.Checked = true;
            this.radForecastMonitorOn.Enabled = false;
            this.radForecastMonitorOn.Location = new System.Drawing.Point(30, 24);
            this.radForecastMonitorOn.Name = "radForecastMonitorOn";
            this.radForecastMonitorOn.Size = new System.Drawing.Size(40, 24);
            this.radForecastMonitorOn.TabIndex = 32;
            this.radForecastMonitorOn.TabStop = true;
            this.radForecastMonitorOn.Text = "On";
            // 
            // chkForecast
            // 
            this.chkForecast.AutoSize = true;
            this.chkForecast.Location = new System.Drawing.Point(3, 3);
            this.chkForecast.Name = "chkForecast";
            this.chkForecast.Size = new System.Drawing.Size(146, 17);
            this.chkForecast.TabIndex = 31;
            this.chkForecast.Text = "Specify Forecast Monitor:";
            this.chkForecast.UseVisualStyleBackColor = true;
            this.chkForecast.CheckedChanged += new System.EventHandler(this.chkForecast_CheckedChanged);
            // 
            // chkSpecifyAuditLevel
            // 
            this.chkSpecifyAuditLevel.AutoSize = true;
            this.chkSpecifyAuditLevel.Location = new System.Drawing.Point(15, 20);
            this.chkSpecifyAuditLevel.Name = "chkSpecifyAuditLevel";
            this.chkSpecifyAuditLevel.Size = new System.Drawing.Size(161, 17);
            this.chkSpecifyAuditLevel.TabIndex = 27;
            this.chkSpecifyAuditLevel.Text = "Specify Audit Logging Level:";
            this.chkSpecifyAuditLevel.UseVisualStyleBackColor = true;
            this.chkSpecifyAuditLevel.CheckedChanged += new System.EventHandler(this.chkSpecifyAuditLevel_CheckedChanged);
            // 
            // cbxAuditLoggingLevel
            // 
            this.cbxAuditLoggingLevel.AutoAdjust = true;
            this.cbxAuditLoggingLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxAuditLoggingLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxAuditLoggingLevel.DataSource = null;
            this.cbxAuditLoggingLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAuditLoggingLevel.DropDownWidth = 160;
            this.cbxAuditLoggingLevel.Enabled = false;
            this.cbxAuditLoggingLevel.FormattingEnabled = false;
            this.cbxAuditLoggingLevel.IgnoreFocusLost = false;
            this.cbxAuditLoggingLevel.ItemHeight = 13;
            this.cbxAuditLoggingLevel.Location = new System.Drawing.Point(146, 40);
            this.cbxAuditLoggingLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cbxAuditLoggingLevel.MaxDropDownItems = 25;
            this.cbxAuditLoggingLevel.Name = "cbxAuditLoggingLevel";
            this.cbxAuditLoggingLevel.SetToolTip = "";
            this.cbxAuditLoggingLevel.Size = new System.Drawing.Size(160, 21);
            this.cbxAuditLoggingLevel.TabIndex = 26;
            this.cbxAuditLoggingLevel.Tag = null;
            // 
            // lblAuditLoggingLevel
            // 
            this.lblAuditLoggingLevel.Location = new System.Drawing.Point(26, 40);
            this.lblAuditLoggingLevel.Name = "lblAuditLoggingLevel";
            this.lblAuditLoggingLevel.Size = new System.Drawing.Size(112, 23);
            this.lblAuditLoggingLevel.TabIndex = 25;
            this.lblAuditLoggingLevel.Text = "Audit Logging Level";
            this.lblAuditLoggingLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this._EmailMessageControl_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(437, 25);
            this._EmailMessageControl_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _EmailMessageControl_Toolbars_Dock_Area_Bottom
            // 
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 318);
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.Name = "_EmailMessageControl_Toolbars_Dock_Area_Bottom";
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(437, 0);
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
            this._EmailMessageControl_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 293);
            this._EmailMessageControl_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _EmailMessageControl_Toolbars_Dock_Area_Right
            // 
            this._EmailMessageControl_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EmailMessageControl_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._EmailMessageControl_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._EmailMessageControl_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EmailMessageControl_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(437, 25);
            this._EmailMessageControl_Toolbars_Dock_Area_Right.Name = "_EmailMessageControl_Toolbars_Dock_Area_Right";
            this._EmailMessageControl_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 293);
            this._EmailMessageControl_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.radDCFulfillmentMonitorOff);
            this.panel3.Controls.Add(this.radDCFulfillmentMonitorOn);
            this.panel3.Controls.Add(this.chkDCFulfillment);
            this.panel3.Location = new System.Drawing.Point(16, 202);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 56);
            this.panel3.TabIndex = 33;
            // 
            // radDCFulfillmentMonitorOff
            // 
            this.radDCFulfillmentMonitorOff.Enabled = false;
            this.radDCFulfillmentMonitorOff.Location = new System.Drawing.Point(86, 24);
            this.radDCFulfillmentMonitorOff.Name = "radDCFulfillmentMonitorOff";
            this.radDCFulfillmentMonitorOff.Size = new System.Drawing.Size(40, 24);
            this.radDCFulfillmentMonitorOff.TabIndex = 33;
            this.radDCFulfillmentMonitorOff.Text = "Off";
            // 
            // radDCFulfillmentMonitorOn
            // 
            this.radDCFulfillmentMonitorOn.Checked = true;
            this.radDCFulfillmentMonitorOn.Enabled = false;
            this.radDCFulfillmentMonitorOn.Location = new System.Drawing.Point(30, 24);
            this.radDCFulfillmentMonitorOn.Name = "radDCFulfillmentMonitorOn";
            this.radDCFulfillmentMonitorOn.Size = new System.Drawing.Size(40, 24);
            this.radDCFulfillmentMonitorOn.TabIndex = 32;
            this.radDCFulfillmentMonitorOn.TabStop = true;
            this.radDCFulfillmentMonitorOn.Text = "On";
            // 
            // chkDCFulfillment
            // 
            this.chkDCFulfillment.AutoSize = true;
            this.chkDCFulfillment.Location = new System.Drawing.Point(3, 3);
            this.chkDCFulfillment.Name = "chkDCFulfillment";
            this.chkDCFulfillment.Size = new System.Drawing.Size(169, 17);
            this.chkDCFulfillment.TabIndex = 31;
            this.chkDCFulfillment.Text = "Specify DC Fulfillment Monitor:";
            this.chkDCFulfillment.UseVisualStyleBackColor = true;
            this.chkDCFulfillment.CheckedChanged += new System.EventHandler(this.chkDCFulfillment_CheckedChanged);
            // 
            // ReportUserOptionsReview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ReportUserOptionsReview_Fill_Panel);
            this.Controls.Add(this._EmailMessageControl_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._EmailMessageControl_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._EmailMessageControl_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._EmailMessageControl_Toolbars_Dock_Area_Top);
            this.Name = "ReportUserOptionsReview";
            this.Size = new System.Drawing.Size(437, 318);
            this.Load += new System.EventHandler(this.ReportUserOptionsReview_Load);
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.ResumeLayout(false);
            this.ReportUserOptionsReview_Fill_Panel.ClientArea.PerformLayout();
            this.ReportUserOptionsReview_Fill_Panel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ReportUserOptionsReview_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _EmailMessageControl_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _EmailMessageControl_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _EmailMessageControl_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _EmailMessageControl_Toolbars_Dock_Area_Top;
        private MIDComboBoxEnh cbxAuditLoggingLevel;
        private System.Windows.Forms.Label lblAuditLoggingLevel;
        private System.Windows.Forms.CheckBox chkSpecifyAuditLevel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radModifySalesMonitorOff;
        private System.Windows.Forms.RadioButton radModifySalesMonitorOn;
        private System.Windows.Forms.CheckBox chkSales;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radForecastMonitorOff;
        private System.Windows.Forms.RadioButton radForecastMonitorOn;
        private System.Windows.Forms.CheckBox chkForecast;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton radDCFulfillmentMonitorOff;
        private System.Windows.Forms.RadioButton radDCFulfillmentMonitorOn;
        private System.Windows.Forms.CheckBox chkDCFulfillment;
    }
}
