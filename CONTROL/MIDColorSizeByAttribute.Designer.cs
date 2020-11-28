using MIDRetail.DataCommon;
using Infragistics.Win.UltraWinToolbars;

namespace MIDRetail.Windows.Controls
{
	partial class MIDColorSizeByAttribute
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

			if (disposing)
			{
				this.ugAllSize.SelectionDrag -= new System.ComponentModel.CancelEventHandler(this.ugAllSize_SelectionDrag);
				this.ugAllSize.AfterRowsDeleted -= new System.EventHandler(this.ugAllSize_AfterRowsDeleted);
				this.ugAllSize.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ugAllSize_BeforeRowInsert);
				this.ugAllSize.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.ugAllSize_KeyUp);
				this.ugAllSize.BeforeRowUpdate -= new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugAllSize_BeforeRowUpdate);
				this.ugAllSize.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.ugAllSize_BeforeEnterEditMode);
				this.ugAllSize.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugAllSize_MouseDown);
				this.ugAllSize.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.ugAllSize_MouseUp);
				this.ugAllSize.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugAllSize_InitializeLayout);
				this.ugAllSize.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugAllSize_BeforeExitEditMode);
				this.ugAllSize.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugAllSize_AfterRowInsert);
				this.ugAllSize.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.ugAllSize_BeforeCellDeactivate);
				this.ugAllSize.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugAllSize_AfterCellUpdate);
				this.ugAllSize.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugAllSize_InitializeRow);
				this.ugAllSize.AfterRowUpdate -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugAllSize_AfterRowUpdate);
				this.ugAllSize.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugAllSize_MouseEnterElement);
				this.ugAllSize.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.ugAllSize_KeyDown);

				if (ValueChanged != null)
				{
					foreach (ValueChangedHandler handler in ValueChanged.GetInvocationList())
					{
						ValueChanged -= handler;
					}
				}

				Include.DisposeControls(this.Controls);
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
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			this.ugAllSize = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.utmMain = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			((System.ComponentModel.ISupportInitialize)(this.ugAllSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			this.SuspendLayout();
			// 
			// ugAllSize
			// 
			appearance1.BackColor = System.Drawing.Color.White;
			appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
			appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
			this.ugAllSize.DisplayLayout.Appearance = appearance1;
			this.ugAllSize.DisplayLayout.InterBandSpacing = 10;
			appearance2.BackColor = System.Drawing.Color.Transparent;
			this.ugAllSize.DisplayLayout.Override.CardAreaAppearance = appearance2;
			appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
			appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
			appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance3.ForeColor = System.Drawing.Color.Black;
			appearance3.TextHAlignAsString = "Left";
			appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
			this.ugAllSize.DisplayLayout.Override.HeaderAppearance = appearance3;
			appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
			this.ugAllSize.DisplayLayout.Override.RowAppearance = appearance4;
			appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
			appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
			appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.ugAllSize.DisplayLayout.Override.RowSelectorAppearance = appearance5;
			this.ugAllSize.DisplayLayout.Override.RowSelectorWidth = 12;
			this.ugAllSize.DisplayLayout.Override.RowSpacingBefore = 2;
			appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
			appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
			appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance6.ForeColor = System.Drawing.Color.Black;
			this.ugAllSize.DisplayLayout.Override.SelectedRowAppearance = appearance6;
			this.ugAllSize.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
			this.ugAllSize.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
			this.ugAllSize.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ugAllSize.Location = new System.Drawing.Point(0, 0);
			this.ugAllSize.Name = "ugAllSize";
			this.ugAllSize.Size = new System.Drawing.Size(0, 0);
			this.ugAllSize.TabIndex = 0;
			this.ugAllSize.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.ugAllSize_SelectionDrag);
			this.ugAllSize.AfterRowsDeleted += new System.EventHandler(this.ugAllSize_AfterRowsDeleted);
			this.ugAllSize.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ugAllSize_BeforeRowInsert);
			this.ugAllSize.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ugAllSize_KeyUp);
			this.ugAllSize.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugAllSize_BeforeRowUpdate);
			this.ugAllSize.BeforeEnterEditMode += new System.ComponentModel.CancelEventHandler(this.ugAllSize_BeforeEnterEditMode);
			this.ugAllSize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugAllSize_MouseDown);
			this.ugAllSize.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ugAllSize_MouseUp);
			this.ugAllSize.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugAllSize_InitializeLayout);
			this.ugAllSize.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugAllSize_BeforeExitEditMode);
			this.ugAllSize.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugAllSize_AfterRowInsert);
			this.ugAllSize.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.ugAllSize_BeforeCellDeactivate);
			this.ugAllSize.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugAllSize_AfterCellUpdate);
			this.ugAllSize.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugAllSize_InitializeRow);
			this.ugAllSize.AfterRowUpdate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugAllSize_AfterRowUpdate);
			this.ugAllSize.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugAllSize_MouseEnterElement);
			this.ugAllSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ugAllSize_KeyDown);
			// 
			// utmMain
			// 
			this.utmMain.DesignerFlags = 1;
			// 
			// MIDColorSizeByAttribute
			// 
			this.Controls.Add(this.ugAllSize);
			((System.ComponentModel.ISupportInitialize)(this.ugAllSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Infragistics.Win.UltraWinGrid.UltraGrid ugAllSize;
		private System.Windows.Forms.ToolTip toolTip1;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager utmMain;

	}
}
