using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Business;
using MIDRetail.Windows.Controls;    // Begin TT#110-MD - RMatelic - In Use Tool
using Infragistics.Win.UltraWinGrid; // End TT#110-MD

namespace MIDRetail.Windows
{
    /// <summary>
    /// Summary description for InUseDialog.
    /// </summary>
    public class InUseDialog : MIDFormBase
    {
        private System.Windows.Forms.Label lblLine1;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugInUseByData;
        private System.Windows.Forms.Button btOk;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private string _title;
        private string _message;
        private Panel pnlTop;
        private Label lblLine2;
        private Panel pblBottom;
        private InUseInfo _inUseInfo;
 
        //BEGIN TT#110-MD-VStuart - In Use Tool
        private DataTable _inUseDataTable;
        private DataTable _inUseHeadings;
        private bool _allowDelete;
        private Label lblCFNo;
        private Label lblCF;
        public int _numHeaders;
        public string itemtitle;
        //END TT#110-MD-VStuart - In Use Tool

        public bool[] PersonalHierarchy = null;  // TT#3630 - JSmith - Delete My Hierarchy

        public DataTable InUseHeadings { get { return _inUseHeadings; } }
        public DataTable InUseDatatable { get { return _inUseDataTable; } }
        public bool AllowDelete { get { return _allowDelete; } }

        public InUseDialog(string title, string message, InUseInfo inUseInfo)
        {
            _title = title;
            _message = message;
            _inUseInfo = inUseInfo;
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //

        }

        // Begin TT#110-MD - RMatelic - In Use Tool
        public InUseDialog(InUseInfo inUseInfo)
        {
  
            _inUseInfo = inUseInfo;

            InitializeComponent();

            //lblLine1.Text = MIDText.GetTextOnly(eMIDTextCode.frm_InUse);
            //lblLine2.Text = MIDText.GetTextOnly(eMIDTextCode.msg_IsUseByTheFollowing) + itemtitle;
            Icon = new System.Drawing.Icon(MIDGraphics.ImageDir + "\\" + MIDGraphics.ApplicationIcon);
            
        }

        // End TT#110-MD

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.lblLine1 = new System.Windows.Forms.Label();
            this.ugInUseByData = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btOk = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblCFNo = new System.Windows.Forms.Label();
            this.lblCF = new System.Windows.Forms.Label();
            this.lblLine2 = new System.Windows.Forms.Label();
            this.pblBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugInUseByData)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pblBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // lblLine1
            // 
            this.lblLine1.Location = new System.Drawing.Point(10, 4);
            this.lblLine1.Name = "lblLine1";
            this.lblLine1.Size = new System.Drawing.Size(297, 18);
            this.lblLine1.TabIndex = 0;
            this.lblLine1.Text = "line 1";
            // 
            // ugInUseByData
            // 
            this.ugInUseByData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugInUseByData.DisplayLayout.Appearance = appearance1;
            this.ugInUseByData.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ugInUseByData.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance3.BackColor2 = System.Drawing.SystemColors.Control;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ugInUseByData.DisplayLayout.GroupByBox.PromptAppearance = appearance3;
            this.ugInUseByData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ugInUseByData.Location = new System.Drawing.Point(3, 2);
            this.ugInUseByData.Name = "ugInUseByData";
            this.ugInUseByData.Size = new System.Drawing.Size(447, 312);
            this.ugInUseByData.TabIndex = 1;
            this.ugInUseByData.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugInUseByData_InitializeLayout);
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.Location = new System.Drawing.Point(366, 371);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 2;
            this.btOk.Text = "&OK";
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblCFNo);
            this.pnlTop.Controls.Add(this.lblCF);
            this.pnlTop.Controls.Add(this.lblLine2);
            this.pnlTop.Controls.Add(this.lblLine1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(454, 44);
            this.pnlTop.TabIndex = 4;
            // 
            // lblCFNo
            // 
            this.lblCFNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCFNo.AutoSize = true;
            this.lblCFNo.Location = new System.Drawing.Point(331, 4);
            this.lblCFNo.Name = "lblCFNo";
            this.lblCFNo.Size = new System.Drawing.Size(23, 13);
            this.lblCFNo.TabIndex = 3;
            this.lblCFNo.Text = "NN";
            this.lblCFNo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblCF
            // 
            this.lblCF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCF.AutoSize = true;
            this.lblCF.Location = new System.Drawing.Point(364, 4);
            this.lblCF.Name = "lblCF";
            this.lblCF.Size = new System.Drawing.Size(86, 13);
            this.lblCF.TabIndex = 2;
            this.lblCF.Text = "Conflict(s) Found";
            // 
            // lblLine2
            // 
            this.lblLine2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLine2.ForeColor = System.Drawing.Color.Red;
            this.lblLine2.Location = new System.Drawing.Point(10, 22);
            this.lblLine2.Name = "lblLine2";
            this.lblLine2.Size = new System.Drawing.Size(424, 19);
            this.lblLine2.TabIndex = 1;
            this.lblLine2.Text = "line 2";
            // 
            // pblBottom
            // 
            this.pblBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pblBottom.Controls.Add(this.ugInUseByData);
            this.pblBottom.Location = new System.Drawing.Point(0, 44);
            this.pblBottom.Name = "pblBottom";
            this.pblBottom.Size = new System.Drawing.Size(454, 315);
            this.pblBottom.TabIndex = 5;
            // 
            // InUseDialog
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(454, 401);
            this.Controls.Add(this.pblBottom);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.btOk);
            this.MaximizeBox = false;
            this.Name = "InUseDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "In Use";
            this.Load += new System.EventHandler(this.InUseDialog_Load);
            this.Controls.SetChildIndex(this.btOk, 0);
            this.Controls.SetChildIndex(this.pnlTop, 0);
            this.Controls.SetChildIndex(this.pblBottom, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugInUseByData)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pblBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private void InUseDialog_Load(object sender, System.EventArgs e)
        {
            try
            {
                //BEGIN TT#110-MD-VStuart - In Use Tool
                itemtitle = _inUseInfo.ItemTitle;
                lblLine1.Text = itemtitle;
                //lblLine2.Text = MIDText.GetTextOnly(eMIDTextCode.msg_IsUseByTheFollowing);
                this.ugInUseByData.DataSource = _inUseDataTable;
                //this.Text = _title;
                // Begin TT#110-MD - RMatelic - In Use Tool
                //this.lblLine2.Text = _message;
                //this.lblLine1.Text = _inUseInfo.ItemName;
                // End TT#110-MD -  
                //this.ugInUseByData.DataSource = _inUseInfo.InUseTable;
                //END TT#110-MD-VStuart - In Use Tool
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        private void btOk_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void ugInUseByData_InitializeLayout(object sender,
                                                    Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
                e.Layout.Override.HeaderClickAction = HeaderClickAction.SortMulti;
                e.Layout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                e.Layout.Override.SelectTypeGroupByRow = SelectType.Extended;

                //BEGIN TT#110-MD-VStuart - In Use Tool
                //Hide the columns that we are not using.
                for (int i = _numHeaders; i < 9; i++)
                {
                    ugInUseByData.DisplayLayout.Bands[0].Columns[i].Hidden = true;
                }

                // Begin TT#480-MD - RMatelic - In Use Dialog grid needs to be display only
                foreach (UltraGridBand band in e.Layout.Bands)
                {
                    foreach (UltraGridColumn col in band.Columns)
                    {
                        if (!col.Hidden)
                        {
                            col.PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
                        }
                        col.CellActivation = Activation.NoEdit;
                    }
                }
                // End TT#480-MD 

                //Make the first column in the data table the default group-by column.
                UltraGridColumn groupByCol = e.Layout.Bands[0].Columns[0];
                bool sortDescending = false;
                bool isGroupByCol = true;
                e.Layout.Bands[0].SortedColumns.Add(groupByCol, sortDescending, isGroupByCol);
                foreach (UltraGridRow row in ugInUseByData.Rows)
                {
                    row.ExpandAll();
                }
                // Begin TT#480-MD - RMatelic - In Use Dialog grid needs to be display only >>> this code was moved above
                //foreach (UltraGridColumn col in e.Layout.Bands[0].Columns)
                //{
                //    if (!col.Hidden)
                //    {
                //        col.PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
                //    }
                //}
                // End TT#480-MD 
                //END TT#110-MD-VStuart - In Use Tool
            }
            catch
            {
                throw;
            }
        }

        private void SetText()
        {
            try
            {
                this.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_In_Use);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
        /// <summary>
        /// This method resolves the display of In Use data.
        /// </summary>
        /// <param name="display"></param> This parameter is always true. It essentially is not used.
        /// <param name="inQuiry"></param> This value is true if display of the dialog is mandatory.
        public void ResolveInUseData(ref bool display, bool inQuiry)
        {
            bool showDialog;
            ResolveInUseData(ref display, inQuiry, false, out showDialog);
        }

        /// <summary>
        /// This method resolves the display of In Use data.
        /// </summary>
        /// <param name="display">This parameter is always true. It essentially is not used.</param> 
        /// <param name="inQuiry">This value is true if display of the dialog is mandatory.</param> 
        public void ResolveInUseData(ref bool display, bool inQuiry, bool deleting, out bool showDialog)
        {
            //BEGIN TT#110-MD-VStuart - In Use Tool
            try
            {
                bool allowDelete;
                //bool allowDeleteAll = true;
                showDialog = false;
                _allowDelete = true;
                SystemData sd = new SystemData();

                _inUseHeadings = sd.GetInUseHeaders((int)_inUseInfo.ItemProfileType);
                _inUseDataTable = new DataTable();
                int i = 0;   // TT#3630 - JSmith - Delete My Hierarchy
                foreach (int rid in _inUseInfo.ItemRIDs)
                {
                    DataTable dt = sd.GetInUseData(rid, (int)_inUseInfo.ItemProfileType, out allowDelete);
                    // Begin TT#3630 - JSmith - Delete My Hierarchy
                    if (PersonalHierarchy != null &&
                        PersonalHierarchy[i])
                    {
                        allowDelete = true;
                        foreach (DataRow row in dt.Rows)
                        {
                            switch (Convert.ToString(row["Header2"]).Trim())
                            {
                                case "Chain Forecast":
                                case "Chain Weekly History":
                                case "Store Forecast":
                                case "Store Daily History":
                                case "Store Weekly History":
                                    row["Header6"] = "Reference";
                                    break;
                            }
                            if (Convert.ToString(row["Header6"]).Trim() == "In Use")
                            {
                                allowDelete = false;
                            }
                        }
                    }
                    // End TT#3630 - JSmith - Delete My Hierarchy

                    if (!allowDelete)
                    {
                        _allowDelete = false;
                        // If we get here we don't want to allow a delete.
                        //allowDeleteAll = false;
                        showDialog = true;

                        MIDEnvironment.Message = ((int)eMIDTextCode.msg_DeleteInUseWarning).ToString(CultureInfo.CurrentUICulture) + ":" + MIDText.GetText(textCode: eMIDTextCode.msg_DeleteInUseWarning);
                        MIDEnvironment.requestFailed = true;
                    }
                    DataTable dtinUseDataTable = dt.Clone();

                    DataRowCollection inUseRows = dt.Rows;

                    foreach (DataRow row in inUseRows)
                    {
                        dtinUseDataTable.ImportRow(row);
                    }
                    _inUseDataTable.Merge(dtinUseDataTable);
                    i++;   // TT#3630 - JSmith - Delete My Hierarchy
                }
                _numHeaders = _inUseHeadings.Rows.Count;
                lblCFNo.Text = _inUseDataTable.Rows.Count.ToString(CultureInfo.CurrentUICulture);

                if (inQuiry) //For mandatory display of the dialog.
                {
                    display = true;
                    lblLine2.Text = "";
                }
                else
                {   //Optional display when a delete is not permitted.
                    if (_inUseDataTable.Rows.Count > 0)
                    {
                        display = true;
                        lblLine2.Text = "Delete(s) can not be accomplished due to conflicts.";
                    }
                }
                int c = 0;
                foreach (DataRow row in _inUseHeadings.Rows)
                {
                    string heading = row[0].ToString();
                    _inUseDataTable.Columns[c].Caption = heading;
                    c++;
                }
            }
            //END TT#110-MD-VStuart - In Use Tool
            catch (Exception ex)
            {
                throw;
            }
        }
        //END TT#110-MD-VStuart - In Use Tool


        public void ResolveInUseDataAndShowProgress(ref bool display, bool inQuiry, bool deleting, out bool showDialog, StoreMgmt.ProgressBarOptions pBarOpt)
        {
            //BEGIN TT#110-MD-VStuart - In Use Tool
            try
            {
                bool allowDelete;
                //bool allowDeleteAll = true;
                showDialog = false;
                SystemData sd = new SystemData();

                _inUseHeadings = sd.GetInUseHeaders((int)_inUseInfo.ItemProfileType);
                _inUseDataTable = new DataTable();
                int i = 0;   // TT#3630 - JSmith - Delete My Hierarchy


                if (pBarOpt.useProgressBar)
                {
                    pBarOpt.progressBarUpdateMax(pBarOpt, _inUseInfo.ItemRIDs.Count);
                    pBarOpt.progressBarUpdateText(pBarOpt, "Checking In Use...");
                }

                foreach (int rid in _inUseInfo.ItemRIDs)
                {
                    DataTable dt = sd.GetInUseData(rid, (int)_inUseInfo.ItemProfileType, out allowDelete);
                    // Begin TT#3630 - JSmith - Delete My Hierarchy
                    if (PersonalHierarchy != null &&
                        PersonalHierarchy[i])
                    {
                        allowDelete = true;
                        foreach (DataRow row in dt.Rows)
                        {
                            switch (Convert.ToString(row["Header2"]).Trim())
                            {
                                case "Chain Forecast":
                                case "Chain Weekly History":
                                case "Store Forecast":
                                case "Store Daily History":
                                case "Store Weekly History":
                                    row["Header6"] = "Reference";
                                    break;
                            }
                            if (Convert.ToString(row["Header6"]).Trim() == "In Use")
                            {
                                allowDelete = false;
                            }
                        }
                    }
                    // End TT#3630 - JSmith - Delete My Hierarchy

                    if (!allowDelete)
                    {
                        // If we get here we don't want to allow a delete.
                        //allowDeleteAll = false;
                        showDialog = true;

                        MIDEnvironment.Message = ((int)eMIDTextCode.msg_DeleteInUseWarning).ToString(CultureInfo.CurrentUICulture) + ":" + MIDText.GetText(textCode: eMIDTextCode.msg_DeleteInUseWarning);
                        MIDEnvironment.requestFailed = true;
                    }
                    DataTable dtinUseDataTable = dt.Clone();

                    DataRowCollection inUseRows = dt.Rows;

                    foreach (DataRow row in inUseRows)
                    {
                        dtinUseDataTable.ImportRow(row);
                    }
                    _inUseDataTable.Merge(dtinUseDataTable);


                    if (pBarOpt.useProgressBar)
                    {
                       
                        pBarOpt.progressBarIncrement(pBarOpt);
                    }

                    i++;   // TT#3630 - JSmith - Delete My Hierarchy
                }


                if (pBarOpt.useProgressBar)
                {
                    pBarOpt.progressBarClose(pBarOpt);
                }

                _numHeaders = _inUseHeadings.Rows.Count;
                lblCFNo.Text = _inUseDataTable.Rows.Count.ToString(CultureInfo.CurrentUICulture);

                if (inQuiry) //For mandatory display of the dialog.
                {
                    display = true;
                    lblLine2.Text = "";
                }
                else
                {   //Optional display when a delete is not permitted.
                    if (_inUseDataTable.Rows.Count > 0)
                    {
                        display = true;
                        lblLine2.Text = "Delete(s) can not be accomplished due to conflicts.";
                    }
                }
                int c = 0;
                foreach (DataRow row in _inUseHeadings.Rows)
                {
                    string heading = row[0].ToString();
                    _inUseDataTable.Columns[c].Caption = heading;
                    c++;
                }
            }
            //END TT#110-MD-VStuart - In Use Tool
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
