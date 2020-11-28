namespace MIDRetail.Windows.Controls
{
    partial class EmailMessageControl
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
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("emailSend");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("emailAddAttachment");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("emailRemoveAttachments");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("emailSend");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmailMessageControl));
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("emailAddAttachment");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("emailRemoveAttachments");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.txtAttachments = new System.Windows.Forms.TextBox();
            this.lblAttachments = new System.Windows.Forms.Label();
            this.EmailMessageControl_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.emailFieldEntry1 = new MIDRetail.Windows.Controls.EmailFieldEntry();
            this._EmailMessageControl_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._EmailMessageControl_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._EmailMessageControl_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.EmailMessageControl_Fill_Panel.ClientArea.SuspendLayout();
            this.EmailMessageControl_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAttachments
            // 
            this.txtAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAttachments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAttachments.Location = new System.Drawing.Point(73, 6);
            this.txtAttachments.Name = "txtAttachments";
            this.txtAttachments.ReadOnly = true;
            this.txtAttachments.Size = new System.Drawing.Size(543, 20);
            this.txtAttachments.TabIndex = 9;
            // 
            // lblAttachments
            // 
            this.lblAttachments.AutoSize = true;
            this.lblAttachments.Location = new System.Drawing.Point(3, 8);
            this.lblAttachments.Name = "lblAttachments";
            this.lblAttachments.Size = new System.Drawing.Size(69, 13);
            this.lblAttachments.TabIndex = 8;
            this.lblAttachments.Text = "Attachments:";
            // 
            // EmailMessageControl_Fill_Panel
            // 
            // 
            // EmailMessageControl_Fill_Panel.ClientArea
            // 
            this.EmailMessageControl_Fill_Panel.ClientArea.Controls.Add(this.emailFieldEntry1);
            this.EmailMessageControl_Fill_Panel.ClientArea.Controls.Add(this.txtAttachments);
            this.EmailMessageControl_Fill_Panel.ClientArea.Controls.Add(this.lblAttachments);
            this.EmailMessageControl_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.EmailMessageControl_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmailMessageControl_Fill_Panel.Location = new System.Drawing.Point(0, 25);
            this.EmailMessageControl_Fill_Panel.Name = "EmailMessageControl_Fill_Panel";
            this.EmailMessageControl_Fill_Panel.Size = new System.Drawing.Size(624, 289);
            this.EmailMessageControl_Fill_Panel.TabIndex = 0;
            // 
            // emailFieldEntry1
            // 
            this.emailFieldEntry1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emailFieldEntry1.emailBody = "";
            this.emailFieldEntry1.emailCC = "";
            this.emailFieldEntry1.emailFrom = "";
            this.emailFieldEntry1.emailSubject = "";
            this.emailFieldEntry1.emailTo = "";
            this.emailFieldEntry1.Location = new System.Drawing.Point(16, 32);
            this.emailFieldEntry1.MinimumSize = new System.Drawing.Size(405, 191);
            this.emailFieldEntry1.Name = "emailFieldEntry1";
            this.emailFieldEntry1.requireBody = true;
            this.emailFieldEntry1.requireFrom = true;
            this.emailFieldEntry1.requireSubject = true;
            this.emailFieldEntry1.requireTo = true;
            this.emailFieldEntry1.requireValidFromWithTo = false;
            this.emailFieldEntry1.Size = new System.Drawing.Size(604, 254);
            this.emailFieldEntry1.TabIndex = 10;
            // 
            // _EmailMessageControl_Toolbars_Dock_Area_Left
            // 
            this._EmailMessageControl_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EmailMessageControl_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._EmailMessageControl_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._EmailMessageControl_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EmailMessageControl_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._EmailMessageControl_Toolbars_Dock_Area_Left.Name = "_EmailMessageControl_Toolbars_Dock_Area_Left";
            this._EmailMessageControl_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 289);
            this._EmailMessageControl_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool3,
            buttonTool4});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
            ultraToolbar1.ShowInToolbarList = false;
            ultraToolbar1.Text = "EmailMessageToolbar";
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
            buttonTool2.SharedPropsInternal.Caption = "Send";
            buttonTool2.SharedPropsInternal.Category = "Email";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            buttonTool5.SharedPropsInternal.AppearancesSmall.Appearance = appearance2;
            buttonTool5.SharedPropsInternal.Caption = "Add Attachment";
            buttonTool5.SharedPropsInternal.Category = "Email";
            buttonTool5.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            buttonTool6.SharedPropsInternal.AppearancesSmall.Appearance = appearance3;
            buttonTool6.SharedPropsInternal.Caption = "Remove Attachments";
            buttonTool6.SharedPropsInternal.Category = "Email";
            buttonTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool2,
            buttonTool5,
            buttonTool6});
            this.ultraToolbarsManager1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _EmailMessageControl_Toolbars_Dock_Area_Right
            // 
            this._EmailMessageControl_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EmailMessageControl_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._EmailMessageControl_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._EmailMessageControl_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EmailMessageControl_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(624, 25);
            this._EmailMessageControl_Toolbars_Dock_Area_Right.Name = "_EmailMessageControl_Toolbars_Dock_Area_Right";
            this._EmailMessageControl_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 289);
            this._EmailMessageControl_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _EmailMessageControl_Toolbars_Dock_Area_Top
            // 
            this._EmailMessageControl_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EmailMessageControl_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._EmailMessageControl_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._EmailMessageControl_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EmailMessageControl_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._EmailMessageControl_Toolbars_Dock_Area_Top.Name = "_EmailMessageControl_Toolbars_Dock_Area_Top";
            this._EmailMessageControl_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(624, 25);
            this._EmailMessageControl_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _EmailMessageControl_Toolbars_Dock_Area_Bottom
            // 
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 314);
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.Name = "_EmailMessageControl_Toolbars_Dock_Area_Bottom";
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(624, 0);
            this._EmailMessageControl_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // EmailMessageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EmailMessageControl_Fill_Panel);
            this.Controls.Add(this._EmailMessageControl_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._EmailMessageControl_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._EmailMessageControl_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._EmailMessageControl_Toolbars_Dock_Area_Top);
            this.MinimumSize = new System.Drawing.Size(425, 314);
            this.Name = "EmailMessageControl";
            this.Size = new System.Drawing.Size(624, 314);
            this.EmailMessageControl_Fill_Panel.ClientArea.ResumeLayout(false);
            this.EmailMessageControl_Fill_Panel.ClientArea.PerformLayout();
            this.EmailMessageControl_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtAttachments;
        private System.Windows.Forms.Label lblAttachments;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.Misc.UltraPanel EmailMessageControl_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _EmailMessageControl_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _EmailMessageControl_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _EmailMessageControl_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _EmailMessageControl_Toolbars_Dock_Area_Top;
        private EmailFieldEntry emailFieldEntry1;
    }
}
