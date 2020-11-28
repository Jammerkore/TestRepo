using System;
using System.Collections.Generic;
using System.Text;

namespace MIDRetail.Windows
{
	partial class ExplorerBase
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
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}

				this.Load -= new System.EventHandler(this.Explorer_Load);
				// Begin TT#1019 - MD - stodd - prohibit allocation actions against GA - 
                if (midEnhancedToolTip != null)
                {
                    midEnhancedToolTip.RemoveAll();
                    midEnhancedToolTip.Dispose();
                    midEnhancedToolTip = null;
                }
				// End TT#1019 - MD - stodd - prohibit allocation actions against GA - 
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
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._inheritanceProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._ExplorerBase_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.utmMain = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._ExplorerBase_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ExplorerBase_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ExplorerBase_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
            this.cmsNodeAction = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiNewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiEditSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.cmiCut = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRename = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiProcessSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.cmiOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiInUse = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiRefreshSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.cmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.midEnhancedToolTip = new MIDRetail.Windows.Controls.MIDEnhancedToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._inheritanceProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.cmsNodeAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // _inheritanceProvider
            // 
            this._inheritanceProvider.ContainerControl = this;
            // 
            // _ExplorerBase_Toolbars_Dock_Area_Left
            // 
            this._ExplorerBase_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ExplorerBase_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._ExplorerBase_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ExplorerBase_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ExplorerBase_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 0);
            this._ExplorerBase_Toolbars_Dock_Area_Left.Name = "_ExplorerBase_Toolbars_Dock_Area_Left";
            this._ExplorerBase_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 150);
            this._ExplorerBase_Toolbars_Dock_Area_Left.ToolbarsManager = this.utmMain;
            // 
            // utmMain
            // 
            this.utmMain.DesignerFlags = 1;
            this.utmMain.DockWithinContainer = this;
            // 
            // _ExplorerBase_Toolbars_Dock_Area_Right
            // 
            this._ExplorerBase_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ExplorerBase_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._ExplorerBase_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ExplorerBase_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ExplorerBase_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(150, 0);
            this._ExplorerBase_Toolbars_Dock_Area_Right.Name = "_ExplorerBase_Toolbars_Dock_Area_Right";
            this._ExplorerBase_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 150);
            this._ExplorerBase_Toolbars_Dock_Area_Right.ToolbarsManager = this.utmMain;
            // 
            // _ExplorerBase_Toolbars_Dock_Area_Top
            // 
            this._ExplorerBase_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ExplorerBase_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._ExplorerBase_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ExplorerBase_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ExplorerBase_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ExplorerBase_Toolbars_Dock_Area_Top.Name = "_ExplorerBase_Toolbars_Dock_Area_Top";
            this._ExplorerBase_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(150, 0);
            this._ExplorerBase_Toolbars_Dock_Area_Top.ToolbarsManager = this.utmMain;
            // 
            // _ExplorerBase_Toolbars_Dock_Area_Bottom
            // 
            this._ExplorerBase_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ExplorerBase_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._ExplorerBase_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ExplorerBase_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ExplorerBase_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 150);
            this._ExplorerBase_Toolbars_Dock_Area_Bottom.Name = "_ExplorerBase_Toolbars_Dock_Area_Bottom";
            this._ExplorerBase_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(150, 0);
            this._ExplorerBase_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.utmMain;
            // 
            // imageListDrag
            // 
            this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // cmsNodeAction
            // 
            this.cmsNodeAction.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiNew,
            this.cmiEditSeparator,
            this.cmiCut,
            this.cmiCopy,
            this.cmiPaste,
            this.cmiDelete,
            this.cmiRename,
            this.cmiProcessSeparator,
            this.cmiOpen,
            this.cmiInUse,
            this.cmiRefreshSeparator,
            this.cmiRefresh});
            this.cmsNodeAction.Name = "cmsNodeAction";
            this.cmsNodeAction.Size = new System.Drawing.Size(118, 220);
            this.cmsNodeAction.Opening += new System.ComponentModel.CancelEventHandler(this.cmsNodeAction_Opening);
            // 
            // cmiNew
            // 
            this.cmiNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiNewFolder,
            this.cmiNewItem});
            this.cmiNew.Name = "cmiNew";
            this.cmiNew.Size = new System.Drawing.Size(117, 22);
            this.cmiNew.Text = "New";
            // 
            // cmiNewFolder
            // 
            this.cmiNewFolder.Name = "cmiNewFolder";
            this.cmiNewFolder.Size = new System.Drawing.Size(107, 22);
            this.cmiNewFolder.Text = "Folder";
            this.cmiNewFolder.Click += new System.EventHandler(this.cmiNewFolder_Click);
            // 
            // cmiNewItem
            // 
            this.cmiNewItem.Name = "cmiNewItem";
            this.cmiNewItem.Size = new System.Drawing.Size(107, 22);
            this.cmiNewItem.Text = "Item";
            this.cmiNewItem.Click += new System.EventHandler(this.cmiNewItem_Click);
            // 
            // cmiEditSeparator
            // 
            this.cmiEditSeparator.Name = "cmiEditSeparator";
            this.cmiEditSeparator.Size = new System.Drawing.Size(114, 6);
            // 
            // cmiCut
            // 
            this.cmiCut.Name = "cmiCut";
            this.cmiCut.Size = new System.Drawing.Size(117, 22);
            this.cmiCut.Text = "Cut";
            this.cmiCut.Click += new System.EventHandler(this.cmiCut_Click);
            // 
            // cmiCopy
            // 
            this.cmiCopy.Name = "cmiCopy";
            this.cmiCopy.Size = new System.Drawing.Size(117, 22);
            this.cmiCopy.Text = "Copy";
            this.cmiCopy.Click += new System.EventHandler(this.cmiCopy_Click);
            // 
            // cmiPaste
            // 
            this.cmiPaste.Name = "cmiPaste";
            this.cmiPaste.Size = new System.Drawing.Size(117, 22);
            this.cmiPaste.Text = "Paste";
            this.cmiPaste.Click += new System.EventHandler(this.cmiPaste_Click);
            // 
            // cmiDelete
            // 
            this.cmiDelete.Name = "cmiDelete";
            this.cmiDelete.Size = new System.Drawing.Size(117, 22);
            this.cmiDelete.Text = "Delete";
            this.cmiDelete.Click += new System.EventHandler(this.cmiDelete_Click);
            // 
            // cmiRename
            // 
            this.cmiRename.Name = "cmiRename";
            this.cmiRename.Size = new System.Drawing.Size(117, 22);
            this.cmiRename.Text = "Rename";
            this.cmiRename.Click += new System.EventHandler(this.cmiRename_Click);
            // 
            // cmiProcessSeparator
            // 
            this.cmiProcessSeparator.Name = "cmiProcessSeparator";
            this.cmiProcessSeparator.Size = new System.Drawing.Size(114, 6);
            // 
            // cmiOpen
            // 
            this.cmiOpen.Name = "cmiOpen";
            this.cmiOpen.Size = new System.Drawing.Size(117, 22);
            this.cmiOpen.Text = "Open...";
            this.cmiOpen.Click += new System.EventHandler(this.cmiOpen_Click);
            // 
            // cmiInUse
            // 
            this.cmiInUse.Name = "cmiInUse";
            this.cmiInUse.Size = new System.Drawing.Size(117, 22);
            this.cmiInUse.Text = "In Use";
            this.cmiInUse.Click += new System.EventHandler(this.cmiInUse_Click);
            // 
            // cmiRefreshSeparator
            // 
            this.cmiRefreshSeparator.Name = "cmiRefreshSeparator";
            this.cmiRefreshSeparator.Size = new System.Drawing.Size(114, 6);
            // 
            // cmiRefresh
            // 
            this.cmiRefresh.Name = "cmiRefresh";
            this.cmiRefresh.Size = new System.Drawing.Size(117, 22);
            this.cmiRefresh.Text = "Refresh";
            this.cmiRefresh.Click += new System.EventHandler(this.cmiRefresh_Click);
            // 
            // ExplorerBase
            // 
            this.ContextMenuStrip = this.cmsNodeAction;
            this.Controls.Add(this._ExplorerBase_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._ExplorerBase_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._ExplorerBase_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._ExplorerBase_Toolbars_Dock_Area_Top);
            this.Name = "ExplorerBase";
            this.Leave += new System.EventHandler(this.ExplorerBase_Leave);
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._inheritanceProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.cmsNodeAction.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		private System.Windows.Forms.ErrorProvider _errorProvider;
		private System.Windows.Forms.ErrorProvider _inheritanceProvider;
		private System.Windows.Forms.ToolTip toolTip1;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager utmMain;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ExplorerBase_Toolbars_Dock_Area_Left;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ExplorerBase_Toolbars_Dock_Area_Right;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ExplorerBase_Toolbars_Dock_Area_Top;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _ExplorerBase_Toolbars_Dock_Area_Bottom;
		private System.Windows.Forms.ImageList imageListDrag;

		#endregion
		private System.Windows.Forms.ContextMenuStrip cmsNodeAction;
		private System.Windows.Forms.ToolStripMenuItem cmiNew;
		private System.Windows.Forms.ToolStripMenuItem cmiNewFolder;
		private System.Windows.Forms.ToolStripMenuItem cmiNewItem;
		private System.Windows.Forms.ToolStripSeparator cmiEditSeparator;
		private System.Windows.Forms.ToolStripMenuItem cmiCut;
		private System.Windows.Forms.ToolStripMenuItem cmiCopy;
		private System.Windows.Forms.ToolStripMenuItem cmiPaste;
		private System.Windows.Forms.ToolStripMenuItem cmiDelete;
		private System.Windows.Forms.ToolStripMenuItem cmiRename;
        private System.Windows.Forms.ToolStripSeparator cmiProcessSeparator;
		private System.Windows.Forms.ToolStripMenuItem cmiOpen;
		private System.Windows.Forms.ToolStripSeparator cmiRefreshSeparator;
        private System.Windows.Forms.ToolStripMenuItem cmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem cmiInUse;
        private Controls.MIDEnhancedToolTip midEnhancedToolTip;
	}
}
