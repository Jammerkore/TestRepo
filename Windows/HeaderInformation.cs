using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Timers;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public class HeaderInformation : MIDFormBase
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
                _trans.HeaderInformation = null;
                //foreach (RowColChooserPanel rowColPnl in _pageRowColChoosers)
                //{
                //    rowColPnl.Dispose();
                //}

                //this.Load -= new System.EventHandler(this.RowColChooser_Load);
				this.cmdCancel.Click -= new System.EventHandler(this.cmdCancel_Click);
				this.cmdApply.Click -= new System.EventHandler(this.cmdApply_Click);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeaderInformation));
            this.cmdApply = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tabChoosers = new System.Windows.Forms.TabControl();
            this.headerGeneralInfo = new System.Windows.Forms.TabPage();
            this.headerInformationGrid = new MIDFlexGrid();
            this.headerGradeInfo = new System.Windows.Forms.TabPage();
            this.headerGradeInformationGrid = new MIDFlexGrid();
            this.headerColorInfo = new System.Windows.Forms.TabPage();
            this.headerColorInformationGrid = new MIDFlexGrid();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.tabChoosers.SuspendLayout();
            this.headerGeneralInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerInformationGrid)).BeginInit();
            this.headerGradeInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerGradeInformationGrid)).BeginInit();
            this.headerColorInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headerColorInformationGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            this.utmMain.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.utmMain_ToolClick_1);
            // 
            // cmdApply
            // 
            this.cmdApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdApply.Location = new System.Drawing.Point(286, 9);
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.Size = new System.Drawing.Size(72, 23);
            this.cmdApply.TabIndex = 2;
            this.cmdApply.Text = "&Apply";
            this.cmdApply.Visible = false;
            this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Location = new System.Drawing.Point(358, 9);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(72, 23);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "&Cancel";
            this.cmdCancel.Visible = false;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.cmdCancel);
            this.pnlButtons.Controls.Add(this.cmdApply);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 262);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(440, 40);
            this.pnlButtons.TabIndex = 7;
            // 
            // tabChoosers
            // 
            this.tabChoosers.Controls.Add(this.headerGeneralInfo);
            this.tabChoosers.Controls.Add(this.headerGradeInfo);
            this.tabChoosers.Controls.Add(this.headerColorInfo);
            this.tabChoosers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabChoosers.Location = new System.Drawing.Point(0, 0);
            this.tabChoosers.Name = "tabChoosers";
            this.tabChoosers.SelectedIndex = 0;
            this.tabChoosers.Size = new System.Drawing.Size(440, 262);
            this.tabChoosers.TabIndex = 8;
            // 
            // headerGeneralInfo
            // 
            this.headerGeneralInfo.Controls.Add(this.headerInformationGrid);
            this.headerGeneralInfo.Location = new System.Drawing.Point(4, 22);
            this.headerGeneralInfo.Name = "headerGeneralInfo";
            this.headerGeneralInfo.Size = new System.Drawing.Size(432, 236);
            this.headerGeneralInfo.TabIndex = 0;
            this.headerGeneralInfo.Text = "General";
            this.headerGeneralInfo.UseVisualStyleBackColor = true;
            // 
            // headerInformationGrid
            // 
            this.headerInformationGrid.AllowEditing = false;
            this.headerInformationGrid.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.Both;
            this.headerInformationGrid.AutoClipboard = true;
            this.headerInformationGrid.CausesValidation = false;
            this.headerInformationGrid.ColumnInfo = resources.GetString("headerInformationGrid.ColumnInfo");
            this.headerInformationGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerInformationGrid.ExtendLastCol = true;
            this.headerInformationGrid.Location = new System.Drawing.Point(0, 0);
            this.headerInformationGrid.Name = "headerInformationGrid";
            this.headerInformationGrid.Rows.Count = 1;
            this.headerInformationGrid.Rows.DefaultSize = 17;
            this.headerInformationGrid.Size = new System.Drawing.Size(432, 236);
            this.headerInformationGrid.TabIndex = 0;
            this.headerInformationGrid.Click += new System.EventHandler(this.midFlexGrid1_Click);
            // 
            // headerGradeInfo
            // 
            this.headerGradeInfo.Controls.Add(this.headerGradeInformationGrid);
            this.headerGradeInfo.Location = new System.Drawing.Point(4, 22);
            this.headerGradeInfo.Name = "headerGradeInfo";
            this.headerGradeInfo.Padding = new System.Windows.Forms.Padding(3);
            this.headerGradeInfo.Size = new System.Drawing.Size(432, 236);
            this.headerGradeInfo.TabIndex = 1;
            this.headerGradeInfo.Text = "Grade";
            this.headerGradeInfo.UseVisualStyleBackColor = true;
            // 
            // headerGradeInformationGrid
            // 
            this.headerGradeInformationGrid.ColumnInfo = resources.GetString("headerGradeInformationGrid.ColumnInfo");
            this.headerGradeInformationGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerGradeInformationGrid.Location = new System.Drawing.Point(3, 3);
            this.headerGradeInformationGrid.Name = "headerGradeInformationGrid";
            this.headerGradeInformationGrid.Rows.Count = 1;
            this.headerGradeInformationGrid.Rows.DefaultSize = 17;
            this.headerGradeInformationGrid.Size = new System.Drawing.Size(426, 230);
            this.headerGradeInformationGrid.TabIndex = 0;
            // 
            // headerColorInfo
            // 
            this.headerColorInfo.Controls.Add(this.headerColorInformationGrid);
            this.headerColorInfo.Location = new System.Drawing.Point(4, 22);
            this.headerColorInfo.Name = "headerColorInfo";
            this.headerColorInfo.Padding = new System.Windows.Forms.Padding(3);
            this.headerColorInfo.Size = new System.Drawing.Size(432, 236);
            this.headerColorInfo.TabIndex = 2;
            this.headerColorInfo.Text = "Color";
            this.headerColorInfo.UseVisualStyleBackColor = true;
            // 
            // headerColorInformationGrid
            // 
            this.headerColorInformationGrid.ColumnInfo = resources.GetString("headerColorInformationGrid.ColumnInfo");
            this.headerColorInformationGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerColorInformationGrid.Location = new System.Drawing.Point(3, 3);
            this.headerColorInformationGrid.Name = "headerColorInformationGrid";
            this.headerColorInformationGrid.Rows.Count = 1;
            this.headerColorInformationGrid.Rows.DefaultSize = 17;
            this.headerColorInformationGrid.Size = new System.Drawing.Size(426, 230);
            this.headerColorInformationGrid.TabIndex = 0;
            // 
            // HeaderInformation
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(440, 302);
            this.Controls.Add(this.tabChoosers);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "HeaderInformation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Header Information";
            this.Load += new System.EventHandler(this.HeaderInformation_Load);
            this.Controls.SetChildIndex(this.pnlButtons, 0);
            this.Controls.SetChildIndex(this.tabChoosers, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.tabChoosers.ResumeLayout(false);
            this.headerGeneralInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.headerInformationGrid)).EndInit();
            this.headerGradeInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.headerGradeInformationGrid)).EndInit();
            this.headerColorInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.headerColorInformationGrid)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cmdApply;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.TabControl tabChoosers;
        private System.Windows.Forms.TabPage headerGeneralInfo;

		const int cScrollDelayTime = 200;
        private MIDRetail.Windows.Controls.MIDFlexGrid headerInformationGrid;
        private TabPage headerGradeInfo;
        private TabPage headerColorInfo;
        private MIDRetail.Windows.Controls.MIDFlexGrid headerGradeInformationGrid;
        private MIDRetail.Windows.Controls.MIDFlexGrid headerColorInformationGrid;
        private ApplicationSessionTransaction _trans;

        //public HeaderInformation(ArrayList aHeaders, bool aOneHeaderRequired, string aTitle, ArrayList aGroupings)
        public HeaderInformation(ApplicationSessionTransaction aTrans, HeaderInformationStruct[] aHeaderInformationStructList)
        {
            try
            {
                InitializeComponent();
                _trans = aTrans;
                string pctNeedLimit;
                string beginDay;
                foreach (HeaderInformationStruct his in aHeaderInformationStructList)
                {
                    if (his.PercentNeedLimit < Include.DefaultPercentNeedLimit)
                    {
                        pctNeedLimit = his.PercentNeedLimit.ToString(CultureInfo.CurrentUICulture); 
                    }
                    else
                    {
                        pctNeedLimit = " ";
                    }
                    if (his.BeginDay != Include.UndefinedDate)
                    {
                        beginDay = his.BeginDay.ToString(CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        beginDay = " ";
                    }
                    object[] item = {
                       his.HeaderID, 
                       his.HeaderRID, 
                       his.StyleID, 
                       his.OTSForcastNodeID,
                       his.OnhandNodeID,
                       his.FactorPct,
                       his.GradeWeekCount,
                       pctNeedLimit,
                       beginDay,
                       his.CapacityNodeID};
                    this.headerInformationGrid.AddItem(item);
                    string gradeMaximum;
                    string colorMinimum;
                    string colorMaximum;
                    foreach (AllocationGradeBin agb in his.AllocationGrades)
                    {
                        if (agb.GradeMaximum < int.MaxValue)
                        {
                            gradeMaximum = agb.GradeMaximum.ToString(CultureInfo.CurrentUICulture);
                        }
                        else
                        {
                            gradeMaximum = " ";
                        }
                        if (agb.GradeColorMinimum > 0)
                        {
                            colorMinimum = agb.GradeColorMinimum.ToString(CultureInfo.CurrentUICulture);
                        }
                        else
                        {
                            colorMinimum = " ";
                        }
                        if (agb.GradeColorMaximum < int.MaxValue)
                        {
                            colorMaximum = agb.GradeColorMaximum.ToString(CultureInfo.CurrentUICulture);
                        }
                        else
                        {
                            colorMaximum = " ";
                        }
                        object[] item2 = {
                           his.HeaderID,
                           agb.Grade,
                           agb.LowBoundary,
                           agb.GradeMinimum,
                           agb.GradeAdMinimum,
                           gradeMaximum,
                           colorMinimum,
                           colorMaximum};
                        this.headerGradeInformationGrid.AddItem(item2);
                    }
                    foreach (HeaderColorInformationStruct hcis in his.HeaderColorInformationList)
                    {
                        if (hcis.ColorCodeName != null)
                        {
                            if (hcis.ColorMinimum > 0)
                            {
                                colorMinimum = hcis.ColorMinimum.ToString(CultureInfo.CurrentUICulture);
                            }
                            else
                            {
                                colorMinimum = " ";
                            }
                            if (hcis.ColorMaximum < int.MaxValue)
                            {
                                colorMaximum = hcis.ColorMaximum.ToString(CultureInfo.CurrentUICulture);
                            }
                            else
                            {
                                colorMaximum = " ";
                            }
                            object[] item3 = {
                           his.HeaderID,
                           hcis.ColorCodeID,
                           hcis.ColorCodeName,
                           hcis.ColorCodeRID,
                           hcis.ColorHierarchyNodeID,
                           hcis.ColorHierarchyNodeRID,
                           colorMinimum,
                           colorMaximum};
                            this.headerColorInformationGrid.AddItem(item3);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                HandleExceptions(exc);
            }
        }

        private void HandleExceptions(System.Exception exc)
        {
            Debug.WriteLine(exc.ToString());
            MessageBox.Show(exc.ToString());
        }


		private void cmdApply_Click(object sender, System.EventArgs e)
		{
		}

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

        private void midFlexGrid1_Click(object sender, EventArgs e)
        {

        }

        private void HeaderInformation_Load(object sender, EventArgs e)
        {

        }

        private void utmMain_ToolClick_1(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {

        }
	}
}

