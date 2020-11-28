namespace MIDRetail.Windows.Controls
{
    partial class filterContainer
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ultraExplorerBar1 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExplorerBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraExplorerBar1
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            this.ultraExplorerBar1.Appearance = appearance1;
            this.ultraExplorerBar1.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraExplorerBar1.ColumnSpacing = 2;
            this.ultraExplorerBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExplorerBar1.GroupSettings.AllowDrag = Infragistics.Win.DefaultableBoolean.False;
            this.ultraExplorerBar1.GroupSettings.ShowExpansionIndicator = Infragistics.Win.DefaultableBoolean.True;
            this.ultraExplorerBar1.GroupSettings.ShowToolTips = Infragistics.Win.DefaultableBoolean.True;
            this.ultraExplorerBar1.GroupSettings.Style = Infragistics.Win.UltraWinExplorerBar.GroupStyle.ControlContainer;
            this.ultraExplorerBar1.GroupSpacing = 3;
            this.ultraExplorerBar1.Location = new System.Drawing.Point(0, 0);
            this.ultraExplorerBar1.Margin = new System.Windows.Forms.Padding(1);
            this.ultraExplorerBar1.Margins.Bottom = 3;
            this.ultraExplorerBar1.Margins.Left = 3;
            this.ultraExplorerBar1.Margins.Right = 3;
            this.ultraExplorerBar1.Margins.Top = 3;
            this.ultraExplorerBar1.Name = "ultraExplorerBar1";
            this.ultraExplorerBar1.NavigationAllowGroupReorder = false;
            this.ultraExplorerBar1.NavigationPaneExpansionMode = Infragistics.Win.UltraWinExplorerBar.NavigationPaneExpansionMode.OnButtonClick;
            this.ultraExplorerBar1.Size = new System.Drawing.Size(319, 441);
            this.ultraExplorerBar1.TabIndex = 3;
            this.ultraExplorerBar1.UseAppStyling = false;
            this.ultraExplorerBar1.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.Office2007;
            // 
            // filterContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraExplorerBar1);
            this.Name = "filterContainer";
            this.Size = new System.Drawing.Size(319, 441);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.filterContainer_Paint);
            this.Resize += new System.EventHandler(this.filterContainer_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExplorerBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar ultraExplorerBar1;

    }
}
