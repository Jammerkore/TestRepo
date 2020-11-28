// BEGIN MID Track #5170 - JSmith - Model enhancements
// Too many lines changed to mark.  Use SCM Compare for details.
//End Track #5170
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Globalization;

using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Windows.Controls;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Infragistics.Shared;

namespace MIDRetail.Windows
{
    /// <summary>
    /// Summary description for SizeConstraintsMaint.
    /// </summary>
    public class SizeConstraintsMaint : SizeConstraintsFormBase
    {
        //private System.Windows.Forms.Label lblModelName;
        //private System.Windows.Forms.ComboBox cbModelName;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        //private SessionAddressBlock _sab;//TT#1638 - Revised Model Save - RBeck
        //private bool _newModel = false;

        // Begin TT#1993 - JSmith - Use filter is a selection option in store attribute dropdown menu
        //private System.Windows.Forms.ComboBox cboStoreAttribute;
        private MIDAttributeComboBox cboStoreAttribute;
        // End TT#1993
        private System.Windows.Forms.Label lblSizeGroup;
        //private string _saveAsName;
        private int _currModelIndex = -1;

        //Begin   TT#1638 - MD - Revised Model Save - RBeck
        //private System.Windows.Forms.ComboBox cboSizeGroup;
        //private System.Windows.Forms.ComboBox cboSizeCurve;
        private MIDComboBoxEnh cboSizeGroup;
        private MIDComboBoxEnh cboSizeCurve;
        //End     TT#1638 - MD - Revised Model Save - RBeck


        private System.Windows.Forms.CheckBox cbExpandAll;
        private System.Windows.Forms.Label label3;
        //protected System.Windows.Forms.PictureBox picBoxName;
        protected System.Windows.Forms.PictureBox picBoxGroup;
        protected System.Windows.Forms.PictureBox picBoxCurve;
        private System.Windows.Forms.Label lblSizeCurve;
        private System.Windows.Forms.Label lblStoreAttribute;
        //		private int _modelIndex = -1;
        private const int DEFAULT_FILL_ZEROS_QUANTITY = 1;


        public SizeConstraintsMaint(SessionAddressBlock SAB)
            : base(SAB)
        {
            //_sab = SAB;   //TT#1638 - Revised Model Save - RBeck
            InitializeComponent();
            FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeConstraints);
            DisplayPictureBoxImages();
            SetPictureBoxTags();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            PromptAttributeChange = false;

            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                this.cboSizeGroup.SelectedIndexChanged -= new System.EventHandler(this.cboSizeGroup_SelectedIndexChanged);
                this.cboSizeCurve.SelectedIndexChanged -= new System.EventHandler(this.cboSizeCurve_SelectedIndexChanged);
                this.cboStoreAttribute.SelectedIndexChanged -= new System.EventHandler(this.cboStoreAttribute_SelectedIndexChanged);
                this.cboStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
                this.cboStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
                this.cboStoreAttribute.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
                //this.cbModelName_SelectionChangeCommitted -= new System.EventHandler(this.cbModelName_SelectionChangeCommitted);
                this.ugModel.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugModel_MouseDown);
                this.ugModel.AfterRowActivate -= new System.EventHandler(this.ugModel_AfterRowActivate);
                this.ugModel.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugModel_AfterRowInsert);
                this.ugModel.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_AfterCellUpdate);
                this.ugModel.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.ugModel_BeforeEnterEditMode);
                this.ugModel.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugModel_InitializeLayout);
            }
            base.Dispose(disposing);
        }

        private void SizeConstraintsMaint_Load(object sender, System.EventArgs e)
        {
            try
            {
                base.FormLoaded = false;

                if (FunctionSecurity.AllowUpdate)
                {
                    Format_Title(eDataState.Updatable, eMIDTextCode.frm_SizeConstraintsModel, null);
                }
                else
                {
                    Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_SizeConstraintsModel, null);
                    //					this.ugModel.Enabled = false;
                }

                BindModelNameComboBox();
                //				BindSizeGroupComboBox();
                //				BindSizeCurveComboBox();
                //				BindStoreAttrComboBox();

                if (cbModelName.SelectedIndex != Include.NoRID)
                {
                    DataRowView aRow = (DataRowView)cbModelName.SelectedItem;
                    SizeConstraintModelRid = Convert.ToInt32(aRow.Row["SIZE_CONSTRAINT_RID"]);
                }

                if (SizeGroupRid != Include.NoRID)
                {
                    base.GetSizesUsing = eGetSizes.SizeGroupRID;
                    base.GetDimensionsUsing = eGetDimensions.SizeGroupRID;
                }
                else
                {
                    base.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
                    base.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
                }
                //Begin Track #5858 - KJohnson - Validating store security only
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute);
                cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, FunctionSecurity, false);
                // End TT#44
                //End Track #5858

                // BEGIN MID Track #4970 - emulate other models 
				// BEGIN TT#827 - MD - stodd - performance - this call was not needed
                //base.CreateConstraintData();
				// END TT#827 - MD - stodd - performance
                // Begin Track #3711 - JScott - Remove this binding, as the next call to InitializeForm rebound data.  This rebinding was causing a problem in Infragistics 5.3.
                //				BindAllSizeGrid(SizeConstraints);
                // End Track #3711

                if (SizeConstraintModelRid == Include.NoRID)
                    InitializeForm();
                else
                {
                    InitializeForm(SizeConstraintModelRid);
                }


                SetReadOnly(FunctionSecurity.AllowUpdate);
                //===========================================
                // Specific security setting for this form
                //===========================================
                this.cbModelName.Enabled = true;
                cbExpandAll.Enabled = true;
                if (FunctionSecurity.AllowDelete && SizeConstraintModelRid != Include.NoRID)
                {
                    btnDelete.Enabled = true;
                    btnInUse.Enabled = true; //TT#110-MD-VStuart - In Use Tool
                }
                SetMaskedComboBoxesEnabled();
                btnSave.Enabled = false;
                btnSaveAs.Enabled = false;
                btnDelete.Enabled = false;
                btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
                cboSizeGroup.Enabled = false;
                cboSizeCurve.Enabled = false;
                cboStoreAttribute.Enabled = false;
                SetText();
                FormLoaded = true;
                // END MID Track #4970

            }
            catch (Exception ex)
            {
                HandleException(ex, "Common_Load");
            }
        }
        // BEGIN MID Track #4970 - additional logic
        private void SetText()
        {
            try
            {
                this.lblModelName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ModelName);
                this.lblSizeCurve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SizeCurve);
                this.lblSizeGroup.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Size_Group);
                this.lblStoreAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Store_Attribute);
                this.cbExpandAll.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExpandAll);
            }
            catch
            {
                throw;
            }
        }
        // END MID Track #4970
        override public void InitializeForm()
        {
            try
            {
                FormLoaded = false;

                InitializeBaseForm();

                Cursor = Cursors.WaitCursor;

                // BEGIN MID Track #4970 - modify to emulate other models				 
                cboSizeGroup.Enabled = true;
                cboSizeCurve.Enabled = true;
                cboStoreAttribute.Enabled = true;
                base.CheckForDequeue();
                // END MID Track #4970 

                base.SizeConstraintProfile = new SizeConstraintModelProfile(Include.NoRID);

                //SetReadOnly(true);
                PromptAttributeChange = false;
                PromptSizeChange = false;

                //Begin TT#1638 - MD - Revised Model Save - RBeck
                //base.FormLoaded = false;
                //btnDelete.Enabled = false;
                //btnSave.Enabled = true;
                //btnSaveAs.Enabled = false;
                //_newModel = true;
                //_saveAsName = string.Empty;
                //this.cbModelName.SelectedIndex = -1;
                //this.cbModelName.Text = "(new model)";
                //this.cbModelName.Enabled = false;     
                //End   TT#1638 - MD - Revised Model Save - RBeck

                PromptAttributeChange = false;
                cboSizeGroup.SelectedValue = this.SizeGroupRid;
                PromptAttributeChange = false;

                cboSizeCurve.SelectedValue = this.SizeCurveGroupRid;
                if (SizeGroupRid != Include.NoRID)
                {
                    base.GetSizesUsing = eGetSizes.SizeGroupRID;
                    base.GetDimensionsUsing = eGetDimensions.SizeGroupRID;
                }
                else
                {
                    base.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
                    base.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
                }

                PromptAttributeChange = false;
                if (PerformingNewModel)
                {
                    if (cboStoreAttribute.DataSource == null)
                    {
                        BindStoreAttrComboBox();
                    }
                    if (StoreGroupRid == Include.NoRID)
                    {
                        cboStoreAttribute.SelectedIndex = 0;
                        StoreGroupRid = Convert.ToInt32(cboStoreAttribute.SelectedValue.ToString(), CultureInfo.CurrentUICulture);
                    }
                    else
                        cboStoreAttribute.SelectedValue = this.StoreGroupRid;

                    if (SAB.ApplicationServerSession.GlobalOptions.SizeGroupCharMask == string.Empty)
                    {
                        BindSizeGroupComboBox();
                    }
                    if (SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask == string.Empty)
                    {
                        BindSizeCurveComboBox();
                    }

                    base.CreateConstraintData();
                    BindAllSizeGrid(SizeConstraints);
                }

                Format_Title(eDataState.New, eMIDTextCode.frm_SizeConstraintsModel, null);
                PromptAttributeChange = true;
                PromptSizeChange = true;
                ChangePending = false;
                FormLoaded = true;
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        override public void InitializeForm(int modelRid)
        {
            try
            {
                base.FormLoaded = false;
                // BEGIN MID Track #4970 - modify to emulate other models
                eDataState dataState;
                bool allowUpdate = FunctionSecurity.AllowUpdate;
                Cursor = Cursors.WaitCursor;
                CheckForDequeue();
                _newModel = false;

                if (allowUpdate)
                {
                    btnSaveAs.Enabled = true;
                    SizeConstraintProfile = base.GetModelForUpdate(modelRid);
                    if (SizeConstraintProfile.ModelLockStatus == eLockStatus.ReadOnly)
                    {
                        allowUpdate = false;
                    }
                    else if (SizeConstraintProfile.ModelLockStatus == eLockStatus.Cancel)
                    {
                        this.cbModelName.SelectedIndex = _currModelIndex;
                        return;
                    }
                }
                else
                {
                    SizeConstraintProfile = base.GetSizeConstraintModelData(modelRid);
                }

                if (!allowUpdate)
                {
                    ModelLocked = false;
                    dataState = eDataState.ReadOnly;
                }
                else
                {
                    ModelLocked = true;
                    dataState = eDataState.Updatable;
                }
                SizeConstraintProfile = new SizeConstraintModelProfile(modelRid);
                _saveAsName = string.Empty;
                PromptAttributeChange = false;

                if (SAB.ApplicationServerSession.GlobalOptions.SizeGroupCharMask != string.Empty)
                {
                    BindSizeGroupComboBox(SizeConstraintProfile.SizeGroupRid);
                }
                else
                {
                    BindSizeGroupComboBox();
                }
                if (SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask != string.Empty)
                {
                    BindSizeCurveComboBox(SizeConstraintProfile.SizeCurveGroupRid);
                }
                else
                {
                    BindSizeCurveComboBox();
                }

                if (cboStoreAttribute.DataSource == null)
                {
                    BindStoreAttrComboBox();
                }

                //The PromptSizeChange is set so the user doesn't get the warning message when the RID's change.
                PromptSizeChange = false;
                cboSizeGroup.SelectedValue = this.SizeGroupRid;
                PromptSizeChange = false;
                cboSizeCurve.SelectedValue = this.SizeCurveGroupRid;
                if (SizeGroupRid != Include.NoRID)
                {
                    base.GetSizesUsing = eGetSizes.SizeGroupRID;
                    base.GetDimensionsUsing = eGetDimensions.SizeGroupRID;
                }
                else
                {
                    base.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
                    base.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
                }

                PromptAttributeChange = false;
                cboStoreAttribute.SelectedValue = this.StoreGroupRid;

                SizeConstraintModelProfile sCP = SAB.HierarchyServerSession.GetSizeConstraintModelData(modelRid); // TT#1638 - MD - Revised Model Save - RBeck

                base.CreateConstraintData();
                DataSetBackup = SizeConstraints.Copy();  // backup this Model's grid data
                BindAllSizeGrid(SizeConstraints);

                //Format_Title(dataState, eMIDTextCode.frm_SizeConstraintsModel, null);
                Format_Title(dataState, eMIDTextCode.frm_SizeConstraintsModel, sCP.SizeConstraintName); // TT#1638 - MD - Revised Model Save - RBeck

                DetermineControlsEnabled(allowUpdate);
                PromptAttributeChange = true;
                PromptSizeChange = true;
                ChangePending = false;
                FormLoaded = true;
                CheckExpandAll();

                if (!FunctionSecurity.AllowUpdate)
                {
                    SetControlReadOnly(this.ugModel, !FunctionSecurity.AllowUpdate);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
        public override void ShowInUse()
        {
            var emp = new SizeConstraintModelProfile(SizeConstraintModelRid);
            eProfileType type = emp.ProfileType;
            int rid = SizeConstraintModelRid;
            base.ShowInUse(type, rid, inQuiry2);
        }
        //END TT#110-MD-VStuart - In Use Tool

        private void DetermineControlsEnabled(bool allowUpdate)
        {
            try
            {
                SetReadOnly(allowUpdate);
                cbExpandAll.Enabled = true;

                //Begin     TT#1638 - MD - Revised Model Save - RBeck

                DetermineSecurity();

                //if (!allowUpdate)
                //{
                //    if (FunctionSecurity.AllowUpdate)
                //    {
                //        btnNew.Enabled = true;			// row is enqueued, so New is okay
                //    }
                //    //					ugModel.Enabled = false; 
                //}
                //else
                //{
                //    if (FunctionSecurity.AllowDelete && cbModelName.SelectedIndex > -1)
                //    {
                //        btnDelete.Enabled = true;
                //    }
                //    else
                //    {
                //        btnDelete.Enabled = false;
                //    }
                //    //					ugModel.Enabled = true; 
                //    btnSaveAs.Enabled = true;
                //}
                //End       TT#1638 - MD - Revised Model Save - RBeck

            }
            catch
            {
                throw;
            }
        }

        //TT#3094 - MD - Swapped filter boxes - RBeck (picBoxCurve and picBoxGroup were moved to be opposite 
        //                                              their respective cboSizeCurve and cboSizeGroup)
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.cboSizeGroup = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboSizeCurve = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblSizeGroup = new System.Windows.Forms.Label();
            this.lblSizeCurve = new System.Windows.Forms.Label();
            this.lblStoreAttribute = new System.Windows.Forms.Label();
            this.cbExpandAll = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.picBoxGroup = new System.Windows.Forms.PictureBox();
            this.picBoxCurve = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(600, 478);
            // 
            // picBoxName
            // 
            this.picBoxName.TabIndex = 54;
            // 
            // cbModelName
            // 
            this.cbModelName.Size = new System.Drawing.Size(247, 21);
            this.cbModelName.TabIndex = 31;
            // 
            // lblModelName
            // 
            this.lblModelName.Size = new System.Drawing.Size(75, 16);
            this.lblModelName.TabIndex = 30;
            // 
            // ugModel
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugModel.DisplayLayout.Appearance = appearance1;
            this.ugModel.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugModel.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugModel.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugModel.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugModel.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugModel.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugModel.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugModel.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugModel.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugModel.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugModel.Location = new System.Drawing.Point(16, 158);
            this.ugModel.Size = new System.Drawing.Size(679, 297);
            this.ugModel.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_AfterCellUpdate);
            this.ugModel.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugModel_InitializeLayout);
            this.ugModel.AfterRowActivate += new System.EventHandler(this.ugModel_AfterRowActivate);
            this.ugModel.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugModel_AfterRowInsert);
            this.ugModel.BeforeEnterEditMode += new System.ComponentModel.CancelEventHandler(this.ugModel_BeforeEnterEditMode);
            this.ugModel.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugModel_BeforeExitEditMode);
            this.ugModel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugModel_MouseDown);
            // 
            // btnInUse
            // 
            this.btnInUse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // cboSizeGroup
            // 
            this.cboSizeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSizeGroup.AutoAdjust = true;
            this.cboSizeGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSizeGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSizeGroup.DataSource = null;
            this.cboSizeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSizeGroup.DropDownWidth = 247;
            this.cboSizeGroup.FormattingEnabled = false;
            this.cboSizeGroup.IgnoreFocusLost = false;
            this.cboSizeGroup.ItemHeight = 13;
            this.cboSizeGroup.Location = new System.Drawing.Point(127, 79);
            this.cboSizeGroup.Margin = new System.Windows.Forms.Padding(0);
            this.cboSizeGroup.MaxDropDownItems = 8;
            this.cboSizeGroup.Name = "cboSizeGroup";
            this.cboSizeGroup.SetToolTip = "";
            this.cboSizeGroup.Size = new System.Drawing.Size(248, 21);
            this.cboSizeGroup.TabIndex = 33;
            this.cboSizeGroup.Tag = null;
            this.cboSizeGroup.SelectedIndexChanged += new System.EventHandler(this.cboSizeGroup_SelectedIndexChanged);
            // 
            // cboSizeCurve
            // 
            this.cboSizeCurve.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSizeCurve.AutoAdjust = true;
            this.cboSizeCurve.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSizeCurve.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSizeCurve.DataSource = null;
            this.cboSizeCurve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSizeCurve.DropDownWidth = 247;
            this.cboSizeCurve.FormattingEnabled = false;
            this.cboSizeCurve.IgnoreFocusLost = false;
            this.cboSizeCurve.ItemHeight = 13;
            this.cboSizeCurve.Location = new System.Drawing.Point(127, 47);
            this.cboSizeCurve.Margin = new System.Windows.Forms.Padding(0);
            this.cboSizeCurve.MaxDropDownItems = 8;
            this.cboSizeCurve.Name = "cboSizeCurve";
            this.cboSizeCurve.SetToolTip = "";
            this.cboSizeCurve.Size = new System.Drawing.Size(248, 21);
            this.cboSizeCurve.TabIndex = 34;
            this.cboSizeCurve.Tag = null;
            this.cboSizeCurve.SelectedIndexChanged += new System.EventHandler(this.cboSizeCurve_SelectedIndexChanged);
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.AllowDrop = true;
            this.cboStoreAttribute.AllowUserAttributes = false;
            this.cboStoreAttribute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreAttribute.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreAttribute.Location = new System.Drawing.Point(127, 111);
            this.cboStoreAttribute.Name = "cboStoreAttribute";
            this.cboStoreAttribute.Size = new System.Drawing.Size(248, 21);
            this.cboStoreAttribute.TabIndex = 35;
            this.cboStoreAttribute.SelectedIndexChanged += new System.EventHandler(this.cboStoreAttribute_SelectedIndexChanged);
            this.cboStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
            this.cboStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            this.cboStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
            // 
            // lblSizeGroup
            // 
            this.lblSizeGroup.Location = new System.Drawing.Point(35, 81);
            this.lblSizeGroup.Name = "lblSizeGroup";
            this.lblSizeGroup.Size = new System.Drawing.Size(63, 16);
            this.lblSizeGroup.TabIndex = 36;
            this.lblSizeGroup.Text = "Size Group";
            this.lblSizeGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSizeCurve
            // 
            this.lblSizeCurve.Location = new System.Drawing.Point(31, 49);
            this.lblSizeCurve.Name = "lblSizeCurve";
            this.lblSizeCurve.Size = new System.Drawing.Size(66, 16);
            this.lblSizeCurve.TabIndex = 37;
            this.lblSizeCurve.Text = "Size Curve";
            this.lblSizeCurve.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStoreAttribute
            // 
            this.lblStoreAttribute.Location = new System.Drawing.Point(38, 112);
            this.lblStoreAttribute.Name = "lblStoreAttribute";
            this.lblStoreAttribute.Size = new System.Drawing.Size(80, 16);
            this.lblStoreAttribute.TabIndex = 38;
            this.lblStoreAttribute.Text = "Store Attribute";
            this.lblStoreAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbExpandAll
            // 
            this.cbExpandAll.Location = new System.Drawing.Point(16, 135);
            this.cbExpandAll.Name = "cbExpandAll";
            this.cbExpandAll.Size = new System.Drawing.Size(104, 16);
            this.cbExpandAll.TabIndex = 39;
            this.cbExpandAll.Text = "Expand All";
            this.cbExpandAll.CheckedChanged += new System.EventHandler(this.cbExpandAll_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(51, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 12);
            this.label3.TabIndex = 40;
            this.label3.Text = "- Or -";
            // 
            // picBoxGroup
            // 
            this.picBoxGroup.Location = new System.Drawing.Point(103, 79);
            this.picBoxGroup.Name = "picBoxGroup";
            this.picBoxGroup.Size = new System.Drawing.Size(19, 20);
            this.picBoxGroup.TabIndex = 55;
            this.picBoxGroup.TabStop = false;
            this.picBoxGroup.Click += new System.EventHandler(this.pictureBox1_Click);
            this.picBoxGroup.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            // 
            // picBoxCurve
            // 
            this.picBoxCurve.Location = new System.Drawing.Point(103, 48);
            this.picBoxCurve.Name = "picBoxCurve";
            this.picBoxCurve.Size = new System.Drawing.Size(19, 20);
            this.picBoxCurve.TabIndex = 56;
            this.picBoxCurve.TabStop = false;
            this.picBoxCurve.Click += new System.EventHandler(this.pictureBox1_Click);
            this.picBoxCurve.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            // 
            // SizeConstraintsMaint
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(720, 517);
            this.Controls.Add(this.picBoxCurve);
            this.Controls.Add(this.picBoxGroup);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbExpandAll);
            this.Controls.Add(this.lblStoreAttribute);
            this.Controls.Add(this.lblSizeCurve);
            this.Controls.Add(this.lblSizeGroup);
            this.Controls.Add(this.cboStoreAttribute);
            this.Controls.Add(this.cboSizeCurve);
            this.Controls.Add(this.cboSizeGroup);
            this.MaximizeBox = false;
            this.Name = "SizeConstraintsMaint";
            this.Text = "Size Constraints Model Maintenance";
            this.Load += new System.EventHandler(this.SizeConstraintsMaint_Load);
            this.Controls.SetChildIndex(this.btnInUse, 0);
            this.Controls.SetChildIndex(this.btnNew, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnSaveAs, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblModelName, 0);
            this.Controls.SetChildIndex(this.cbModelName, 0);
            this.Controls.SetChildIndex(this.cboSizeGroup, 0);
            this.Controls.SetChildIndex(this.cboSizeCurve, 0);
            this.Controls.SetChildIndex(this.cboStoreAttribute, 0);
            this.Controls.SetChildIndex(this.lblSizeGroup, 0);
            this.Controls.SetChildIndex(this.lblSizeCurve, 0);
            this.Controls.SetChildIndex(this.lblStoreAttribute, 0);
            this.Controls.SetChildIndex(this.cbExpandAll, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.ugModel, 0);
            this.Controls.SetChildIndex(this.picBoxName, 0);
            this.Controls.SetChildIndex(this.picBoxGroup, 0);
            this.Controls.SetChildIndex(this.picBoxCurve, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region IFormBase Members
        //override public void ICut()
        //{

        //}

        //override public void ICopy()
        //{

        //}

        //override public void IPaste()
        //{

        //}	

        //override public void ISave()
        //{
        //    try
        //    {
        //        this.Cursor = Cursors.WaitCursor;
        //        if (NameValid())	// MID Track #4970 - add NameValid
        //        {
        //            PerformingSaveAs = false;
        //            SaveChanges();
        //        }
        //    }		
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Default;
        //    }
        //}

        //override public void ISaveAs()
        //{
        //    try
        //    {
        //        this.Cursor = Cursors.WaitCursor;
        //        if (NameValid())	// MID Track #4970 - add NameValid
        //        {
        //            _newModel = true;
        //            _saveAsName = string.Empty;
        //            PerformingSaveAs = true;
        //            SaveChanges();
        //        }
        //    }		
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Default;
        //    }
        //}

        override protected void DeleteModel()
        {
            TransactionData td = new TransactionData();
            try
            {
                int currIndex = cbModelName.SelectedIndex;
                bool itemDeleted = false;
                if (this.SizeConstraintModelRid != Include.NoRID)
                {
                    string text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDelete);
                    text = text.Replace("{0}", "Size Constraints Model: " + SizeConstraintProfile.SizeConstraintName);
                    if (MessageBox.Show(text, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == DialogResult.Yes)
                    //BEGIN TT#601-MD-VStuart-FWOS Model attempts delete while InUse
                    {
                        //If the RID is InUse don't delete. If RID is NOT InUse go ahead and delete.
                        var emp = new SizeConstraintModelProfile(SizeConstraintModelRid);
                        eProfileType type = emp.ProfileType;
                        int rid = SizeConstraintModelRid;

                        if (!CheckInUse(type, rid, false))
                        {
                            this.ModelChangeType = eChangeType.delete;
                            td.OpenUpdateConnection();
                            // This uses the SizeConstraintRid as defined in the base clase
                            DeleteSizeConstraintChildren(td);
                            this.SizeModelData.SizeConstraintModel_Delete(SizeConstraintModelRid, td);
                            td.CommitData();
                            DataTable dt = (DataTable) cbModelName.DataSource;
                            DataRow[] rows =
                                dt.Select("SIZE_CONSTRAINT_RID = " +
                                          SizeConstraintModelRid.ToString(CultureInfo.CurrentUICulture));
                            dt.Rows.Remove(rows[0]);
                            dt.AcceptChanges();

                            itemDeleted = true;
                        } // END MID Track #4970  
                    }
                    //END  TT#601-MD-VStuart-FWOS Model attempts delete while InUse
                }

                if (itemDeleted)
                {
                    if (cbModelName.Items.Count > 0)
                    {
                        int nextItem;
                        if (currIndex >= cbModelName.Items.Count)
                        {
                            nextItem = cbModelName.Items.Count - 1;
                        }
                        else
                        {
                            nextItem = currIndex;
                        }

                        DataRowView aRow = (DataRowView)cbModelName.SelectedItem;
                        SizeConstraintModelRid = Convert.ToInt32(aRow.Row["SIZE_CONSTRAINT_RID"]);
                        InitializeForm(SizeConstraintModelRid);
                    }
                    else
                    {
                        InitializeForm();
                    }
                }
            }
            catch (DatabaseForeignKeyViolation)
            {
                td.Rollback();
                //BEGIN TT#110-MD-VStuart - In Use Tool
                //MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
                ShowInUse();
                //END TT#110-MD-VStuart - In Use Tool
            }
            catch (Exception exception)
            {
                td.Rollback();
                HandleException(exception);
            }
            finally
            {
                td.CloseUpdateConnection();
            }
        }
        //Begin     TT#1638 - Revised Model Save - RBeck
        //override public void IRefresh()
        //{

        //}

        //private void btnSave_Click(object sender, System.EventArgs e)
        //{
        //    ISave();
        //}

        //private void btnNew_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        FormLoaded = false;
        //        CheckForPendingChanges();
        //        InitializeForm();
        //    }
        //    catch(Exception exception)
        //    {
        //        HandleException(exception);
        //    }
        //}

        //private void btnDelete_Click(object sender, System.EventArgs e)
        //{
        //    IDelete();
        //}

        //private void btnClose_Click(object sender, System.EventArgs e)
        //{
        //    Close();
        //}
        //End   TT#1638 - MD - Revised Model Save - RBeck 

        #endregion

        private void BindModelNameComboBox()
        {
            try
            {
                // Begin TT#827-MD - JSmith - Performance 
                if (SAB.ApplicationServerSession.GlobalOptions.SizeConstraintCharMask != string.Empty)
                {
                    cbModelName.DataSource = null;
                    return;
                }
                // End TT#827-MD - JSmith - Performance

                DataTable dtSizeModel = SizeModelData.SizeConstraintModel_Read();

                //Begin TT#1638 - MD - Revised Model Save - RBeck            
                //DataRow row = dtSizeModel.NewRow();
                //row["SIZE_CONSTRAINT_RID"] = Include.NoRID;
                //row["SIZE_CONSTRAINT_NAME"] = string.Empty;
                //dtSizeModel.Rows.InsertAt(row, 0);
                //dtSizeModel.AcceptChanges();
                //End   TT#1638 - MD - Revised Model Save - RBeck 

                cbModelName.DataSource = dtSizeModel;
                cbModelName.DisplayMember = "SIZE_CONSTRAINT_NAME";
                cbModelName.ValueMember = "SIZE_CONSTRAINT_RID";
                // Begin TT#398-MD - JSmith - Cannot save changes in Size Constraint Model after making changes->Unable to cast object of type error message
                cbModelName.SelectedIndex = Include.Undefined;
                // End TT#398-MD - JSmith - Cannot save changes in Size Constraint Model after making changes->Unable to cast object of type error message

                //AdjustTextWidthComboBox_DropDown(cbModelName);  TT#1638 - MD - Revised Model Save - RBeck

            }
            catch (Exception ex)
            {
                HandleException(ex, "BindModelNameComboBox");
            }
        }

        // BEGIN MID Track #4396 - Justin Bolles - Size Dropdown Filter
        private void BindModelNameComboBox(string aFilterString, bool aCaseSensitive)
        {
            try
            {
                // Originally tried RowFilter but received invalid rowfilter msg when multiple wild cards
                //aFilterString = aFilterString.Replace("*", "%");
                //aFilterString = aFilterString.Replace("'", "''");	// for string with single quote

                //string whereClause = "SIZE_CONSTRAINT_NAME LIKE " + "'" + aFilterString + "'";
                //if (!aCaseSensitive)
                //{
                //    whereClause += Include.CaseInsensitiveCollation;
                //}

                aFilterString = aFilterString.Replace("*", "%");
               // DataTable dtSizeModel = SizeModelData.SizeConstraintModel_FilterRead(whereClause);
                DataTable dtSizeModel;
                if (aCaseSensitive)
                {
                    dtSizeModel = SizeModelData.SizeConstraintModel_FilterReadCaseSensitive(aFilterString);
                }
                else
                {
                    dtSizeModel = SizeModelData.SizeConstraintModel_FilterReadCaseInsensitive(aFilterString);
                }
            
            //Begin  TT#3039 - MD -  Size Constraint Model Selection - R Beck
                //Begin TT#1638 - MD - Revised Model Save - RBeck
                DataRow emptyRow = dtSizeModel.NewRow();
                emptyRow["SIZE_CONSTRAINT_NAME"] = "";
                emptyRow["SIZE_CONSTRAINT_RID"] = Include.NoRID;
                dtSizeModel.Rows.Add(emptyRow);
                dtSizeModel.DefaultView.Sort = "SIZE_CONSTRAINT_NAME ASC";
                dtSizeModel.AcceptChanges();
                //End   TT#1638 - MD - Revised Model Save - RBeck 
            //End    TT#3039 - MD - Revised Model Save - RBeck
                cbModelName.DataSource = dtSizeModel;
                cbModelName.DisplayMember = "SIZE_CONSTRAINT_NAME";
                cbModelName.ValueMember = "SIZE_CONSTRAINT_RID";
                cbModelName.Enabled = true;

                //AdjustTextWidthComboBox_DropDown(cbModelName);  TT#1638 - MD - Revised Model Save - RBeck

            }
            catch (Exception ex)
            {
                HandleException(ex, "BindSizeConstraintsComboBox");
            }
        }
        // END MID Track #4396

        protected override bool SaveChanges()
        {
            try
            {
                bool continueSave = false;
                bool saveAsCanceled = false;


                if (!ErrorFound)
                {
                    //BEGIN TT#3972-MD-VStuart-Able to create size constraint model with blank name
                    //if (_newModel && _saveAsName == string.Empty)
                    if (_newModel)
                    //END TT#3972-MD-VStuart-Able to create size constraint model with blank name
                    {
                        frmSaveAs formSaveAs = new frmSaveAs(SAB);
                        formSaveAs.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

                        if (PerformingSaveAs)
                        {
                            if (cbModelName.SelectedIndex != -1)
                            {
                                formSaveAs.SaveAsName = cbModelName.Text.Trim();
                            }
                        }
                        while (!continueSave)
                        {
                            formSaveAs.ShowDialog(this);
                            saveAsCanceled = formSaveAs.SaveCanceled;

                            if (!saveAsCanceled)
                            {
                                SizeConstraintModelProfile checkExists = GetConstrainModel(formSaveAs.SaveAsName);
                                if (checkExists.Key == Include.NoRID)
                                {
                                    this.SizeConstraintModelName = formSaveAs.SaveAsName;
                                    continueSave = true;
                                }
                                else
                                {
                                    if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName), this.Text,
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                        == DialogResult.No)
                                    {
                                        saveAsCanceled = true;
                                        continueSave = true;
                                    }
                                    else
                                    {
                                        saveAsCanceled = false;
                                        continueSave = true;
                                        PerformingSaveAs = false;  // RO-4989 - Ability to Save As a duplicate Size Constraint name
                                        CheckForDequeue();  // RO-4989 - Ability to Save As a duplicate Size Constraint name

                                        SizeConstraintModelProfile constraintModel = GetConstrainModel(formSaveAs.SaveAsName);
                                        this.ModelChangeType = eChangeType.update;
                                        this.SizeConstraintModelRid = constraintModel.Key;
                                        this.SizeConstraintModelName = formSaveAs.SaveAsName;  // RO-4989 - Ability to Save As a duplicate Size Constraint name
                                        //_saveAsName = formSaveAs.SaveAsName;
                                    }
                                }
                            }
                            else
                            {
                                continueSave = true;
                            }
                        }
                    }

                    if (!saveAsCanceled)
                    {
                        TransactionData td = new TransactionData();
                        if (PerformingSaveAs)
                        {
                            CheckForDequeue();
                            ModelChangeType = eChangeType.add;
                            SizeConstraintModelRid = Include.NoRID;
                        }

                        //BEGIN TT#3972-MD-VStuart-Able to create size constraint model with blank name
                        if (!string.IsNullOrEmpty(SizeConstraintModelName))
                        {
                            td.OpenUpdateConnection();
                            SizeConstraintModelRid = SizeModelData.SizeConstraintModel_Update_Insert(this.SizeConstraintModelRid,
                                this.SizeConstraintModelName, this.SizeGroupRid, this.SizeCurveGroupRid, this.StoreGroupRid, td);
                            InsertUpdateSizeConstraints(td);
                            td.CommitData();
                            td.CloseUpdateConnection();
                            ChangePending = false;
                            if (this.ModelChangeType == eChangeType.add)
                            {

                                DataTable dt = (DataTable)cbModelName.DataSource;
                                dt.Rows.Add(new object[] { SizeConstraintModelRid, 
                                                           SizeConstraintModelName, 
									                       StoreGroupRid, 
                                                           SizeGroupRid, 
                                                           SizeCurveGroupRid }
                                            );
                                dt.AcceptChanges();
                                cbModelName.SelectedValue = SizeConstraintModelRid;
                            }
                        }
                        else
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NameRequiredToSave), this.Text,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _newModel = true;
                            SaveChanges();
                        }
                        //END TT#3972-MD-VStuart-Able to create size constraint model with blank name
                    }
                    else
                    {
                        PerformingSaveAs = false;  // RO-4989 - Ability to Save As a duplicate Size Constraint name
                        _newModel = false;
                        _saveAsName = SizeConstraintModelName;
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SaveCanceled), this.Text,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //BEGIN TT#3972-MD-VStuart-Able to create size constraint model with blank name
                        if (string.IsNullOrEmpty(SizeConstraintModelName))
                        {
                            _newModel = true;
                            continueSave = false;
                        }
                        //END TT#3972-MD-VStuart-Able to create size constraint model with blank name
                    }
                    this.cbModelName.Enabled = true;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }

            return true;
        }

        private SizeConstraintModelProfile GetConstrainModel(string modelName)
        {
            MaintainSizeConstraints maint = new MaintainSizeConstraints(this.SizeModelData);
            return maint.GetConstrainModel(modelName);
        }

        // BEGIN MID Track #4970 - added name change logic
        override protected bool NameValid()
        {
            bool isValid = true;
            try
            {
                if (_newModel)
                {
                    return isValid;
                }
                int modelRID = 0;
                string modelName = cbModelName.Text.Trim();
                if (modelName != SizeConstraintProfile.SizeConstraintName.Trim())
                {
                    DataTable dt = this.SizeModelData.SizeConstraintModel_Read(modelName);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow aRow = dt.Rows[0];
                        modelRID = Convert.ToInt32(aRow["SIZE_CONSTRAINT_RID"], CultureInfo.CurrentUICulture);
                        foreach (ComboObject comboObj in cbModelName.Items)
                        {
                            if (comboObj.Value == SizeConstraintProfile.SizeConstraintName.Trim() &&
                                comboObj.Key != modelRID)
                            {
                                isValid = false;
                                break;
                            }
                        }
                    }

                    if (isValid)
                    {
                        // update Drop down
                        foreach (ComboObject comboObj in cbModelName.Items)
                        {
                            if (comboObj.Value == SizeConstraintProfile.SizeConstraintName.Trim())
                            {
                                modelRID = comboObj.Key;
                                cbModelName.Items.Remove(comboObj);
                                cbModelName.Items.Add(new ComboObject(modelRID, modelName));
                                break;
                            }
                        }
                        SizeConstraintProfile.SizeConstraintName = modelName;
                    }
                    else
                    {
                        if (MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName), this.Text,
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            cbModelName.Text = SizeConstraintProfile.SizeConstraintName.Trim();
                        }
                    }
                }
            }
            catch
            {
                isValid = false;
                throw;
            }
            return isValid;
        }
        // END MID Track #4970 


        /// <summary>
        /// Clears the size group combo box.
        /// </summary>
        private void ClearModelNameComboBox()
        {
            try
            {
                cbModelName.DataSource = null;
                cbModelName.Items.Clear();
            }
            catch (Exception ex)
            {
                HandleException(ex, "SizeMethodFormBase.ClearModelNameComboBox");
            }
        }

        /// <summary>
        /// Clears the size group combo box.
        /// </summary>
        private void ClearSizeGroupComboBox()
        {
            try
            {
                cboSizeGroup.DataSource = null;
                cboSizeGroup.Items.Clear();
            }
            catch (Exception ex)
            {
                HandleException(ex, "SizeMethodFormBase.ClearSizeGroupComboBox");
            }
        }

        /// <summary>
        /// Clears the size curve combo box.
        /// </summary>
        private void ClearSizeCurveComboBox()
        {
            try
            {
                cboSizeCurve.DataSource = null;
                cboSizeCurve.Items.Clear();
            }
            catch (Exception ex)
            {
                HandleException(ex, "SizeMethodFormBase.ClearSizeCurveComboBox");
            }
        }

        /// <summary>
        /// Clears the filter combo box.
        /// </summary>
        private void ClearAttributeComboBox()
        {
            try
            {
                cboStoreAttribute.DataSource = null;
                cboStoreAttribute.Items.Clear();
            }
            catch (Exception ex)
            {
                HandleException(ex, "SizeMethodFormBase.ClearAttributeComboBox");
            }
        }


        /// <summary>
        /// Populates the size curve combo box.
        /// </summary>
        private void BindSizeCurveComboBox()
        {
            try
            {
                DataTable dtSizeCurve = MIDEnvironment.CreateDataTable();
                SizeCurve objSizeCurve = new SizeCurve();
                dtSizeCurve = objSizeCurve.GetSizeCurveGroups();
                dtSizeCurve.Rows.Add(new object[] { Include.NoRID, string.Empty });

                DataView dvSizeCurve = new DataView(dtSizeCurve, "", "SIZE_CURVE_GROUP_NAME", DataViewRowState.CurrentRows);
                ClearSizeCurveComboBox();

                //cboSizeCurve.DataSource = dvSizeCurve; //dtSizeCurve;		// MID Track #5331 - '..DataRowView' Text in drop down
                cboSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";		//					move 'DataSource =  '
                cboSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";
                cboSizeCurve.DataSource = dvSizeCurve;						// MID Track #5331

                //AdjustTextWidthComboBox_DropDown(cboSizeCurve);  TT#1638 - MD - Revised Model Save - RBeck

            }
            catch (Exception ex)
            {
                HandleException(ex, "BindSizeCurveComboBox");
            }
        }

        // BEGIN MID Track #4396 - Justin Bolles - Size Dropdown Filter
        virtual protected void BindSizeCurveComboBox(bool includeEmptySelection, string aFilterString, bool aCaseSensitive)
        {
            try
            {
                DataTable dtSizeCurve = MIDEnvironment.CreateDataTable();
                SizeCurve objSizeCurve = new SizeCurve();
                object selectedValue = cboSizeCurve.SelectedValue;

                // RowFilter didn't work with multiple wild cards 
                aFilterString = aFilterString.Replace("*", "%");
                //aFilterString = aFilterString.Replace("'", "''");	// for string with single quote

                //string whereClause = "SIZE_CURVE_GROUP_NAME LIKE " + "'" + aFilterString + "'";
                //if (!aCaseSensitive)
                //{
                //    whereClause += Include.CaseInsensitiveCollation;
                //}

                //dtSizeCurve = objSizeCurve.GetFilteredSizeCurveGroups(whereClause);
                if (aCaseSensitive)
                {
                    dtSizeCurve = objSizeCurve.GetFilteredSizeCurveGroupsCaseSensitive(aFilterString);
                }
                else
                {
                    dtSizeCurve = objSizeCurve.GetFilteredSizeCurveGroupsCaseInsensitive(aFilterString);
                }

                if (includeEmptySelection)
                {
                    dtSizeCurve.Rows.Add(new object[] { Include.NoRID, "" });
                }

                DataView dvSizeCurve = new DataView(dtSizeCurve, "", "SIZE_CURVE_GROUP_NAME", DataViewRowState.CurrentRows);
                cboSizeCurve.DataSource = dvSizeCurve;
                cboSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";
                cboSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";

                //AdjustTextWidthComboBox_DropDown(cboSizeCurve);  TT#1638 - MD - Revised Model Save - RBeck

                if (selectedValue != null)
                {
                    cboSizeCurve.SelectedValue = selectedValue;
                }
                cboSizeCurve.Enabled = true;
            }
            catch (Exception ex)
            {
                HandleException(ex, "BindSizeCurveComboBox");
            }
        }
        // END MID Track #4396

        /// <summary>
        /// Populates the size curve combo box.
        /// </summary>
        private void BindSizeCurveComboBox(int SizeGroupId)
        {
            try
            {
                DataTable dtSizeCurve = MIDEnvironment.CreateDataTable();
                SizeCurve objSizeCurve = new SizeCurve();
                dtSizeCurve = objSizeCurve.GetSpecificSizeCurveGroup(SizeGroupId);
                object selectedValue = cboSizeCurve.SelectedValue;

                ClearSizeCurveComboBox();

                cboSizeCurve.DataSource = dtSizeCurve; //dtSizeCurve;
                cboSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";
                cboSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";

                //AdjustTextWidthComboBox_DropDown(cboSizeCurve);  TT#1638 - MD - Revised Model Save - RBeck

                if (selectedValue != null)
                {
                    cboSizeCurve.SelectedValue = selectedValue;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "BindSizeCurveComboBox");
            }
        }

        /// <summary>
        /// Populates the size group combo box.
        /// </summary>
        private void BindSizeGroupComboBox()
        {
            try
            {
                DataTable dtGroups = MIDEnvironment.CreateDataTable("Groups");

                //_sgl = new MIDRetail.Business.Allocation.SizeGroupList(eProfileType.SizeGroup);

                //				sizeGroupList.LoadAll(false);
                //_sgl.LoadAll(false);
                SizeGroup sizeGroupData = new SizeGroup();
                DataTable dt = sizeGroupData.GetSizeGroups(false);

                dtGroups.Columns.Add("Key");
                dtGroups.Columns.Add("Name");

                dtGroups.Rows.Add(new object[] { Include.NoRID, "" });
                //				foreach(SizeGroupProfile sgp in sizeGroupList.ArrayList)
                //				{
                //					dtGroups.Rows.Add(new object[] { sgp.Key, sgp.SizeGroupName });
                //				}
                int key;
                string name;
                foreach (DataRow dr in dt.Rows)
                {
                    key = Convert.ToInt32(dr["SIZE_GROUP_RID"], CultureInfo.CurrentCulture);
                    name = Convert.ToString(dr["SIZE_GROUP_NAME"], CultureInfo.CurrentCulture);
                    dtGroups.Rows.Add(new object[] { key, name });
                }

                ClearSizeGroupComboBox();

                cboSizeGroup.DataSource = dtGroups;
                cboSizeGroup.DisplayMember = "Name";
                cboSizeGroup.ValueMember = "Key";

                //AdjustTextWidthComboBox_DropDown(cboSizeGroup);  TT#1638 - MD - Revised Model Save - RBeck

            }
            catch (Exception ex)
            {
                HandleException(ex, "BindSizeGroupComboBox");
            }
        }

        /// <summary>
        /// Populates the size group combo box.
        /// </summary>
        private void BindSizeGroupComboBox(int aGroupRID)
        {
            try
            {
                SizeGroup sizeGroupData = new SizeGroup();
                DataTable dtGroups = sizeGroupData.GetSizeGroup(aGroupRID);

                cboSizeGroup.DataSource = null;
                //				cboSizeGroup.Items.Clear();
                cboSizeGroup.DataSource = dtGroups;
                cboSizeGroup.DisplayMember = "SIZE_GROUP_NAME";
                cboSizeGroup.ValueMember = "SIZE_GROUP_RID";

                //AdjustTextWidthComboBox_DropDown(cboSizeGroup);  TT#1638 - MD - Revised Model Save - RBeck

            }
            catch (Exception ex)
            {
                HandleException(ex, "BindSizeGroupComboBox");
            }
        }

        // BEGIN MID Track #4396 - Justin Bolles - Size Dropdown Filter
        virtual protected void BindSizeGroupComboBox(bool includeEmptySelection, string aFilterString, bool aCaseSensitive)
        {
            try
            {
                DataTable dtGroups = MIDEnvironment.CreateDataTable("Groups");
                // Originally tried RowFilter but received invalid rowfilter msg when multiple wild cards
                object selectedValue = cboSizeGroup.SelectedValue;

                SizeGroup sizeGroup = new SizeGroup();
                aFilterString = aFilterString.Replace("*", "%");
               // aFilterString = aFilterString.Replace("'", "''");	// for string with single quote

                //string whereClause = "SIZE_GROUP_NAME LIKE '" + aFilterString + "'";
                //if (!aCaseSensitive)
                //{
                //    whereClause += Include.CaseInsensitiveCollation;
                //}
                //dtGroups = sizeGroup.SizeGroup_FilterRead(whereClause);
                if (aCaseSensitive)
                {
                    dtGroups = sizeGroup.SizeGroup_FilterReadCaseSensitive(aFilterString);
                }
                else
                {
                    dtGroups = sizeGroup.SizeGroup_FilterReadCaseInsensitive(aFilterString);
                }

                if (includeEmptySelection)
                {
                    dtGroups.Rows.Add(new object[] { Include.NoRID, "" });
                }

                dtGroups.DefaultView.Sort = "SIZE_GROUP_NAME ASC";
                dtGroups.Columns["SIZE_GROUP_RID"].ColumnName = "Key";
                dtGroups.Columns["SIZE_GROUP_NAME"].ColumnName = "Name";
                dtGroups.AcceptChanges();

                cboSizeGroup.DataSource = dtGroups;
                cboSizeGroup.DisplayMember = "Name";
                cboSizeGroup.ValueMember = "Key";

                //AdjustTextWidthComboBox_DropDown(cboSizeGroup);  TT#1638 - MD - Revised Model Save - RBeck

                if (selectedValue != null)
                {
                    cboSizeGroup.SelectedValue = selectedValue;
                }
                cboSizeGroup.Enabled = true;
            }
            catch (Exception ex)
            {
                HandleException(ex, "BindSizeGroupComboBox");
            }
        }
        // END MID Track #4396


        /// <summary>
        /// Populate all Store_Groups (Attributes); 1st sel if new else selection made
        /// in load
        /// </summary>
        private void BindStoreAttrComboBox()
        {
            try
            {
                ClearAttributeComboBox();
                // BEGIN MID Track 4246 - Attribute list needs to be in alphabetical order 
                //cboStoreAttribute.DataSource = storeData.StoreGroup_Read(eDataOrderBy.RID);
                // Begin TT#1993 - JSmith - Use filter is a selection option in store attribute dropdown menu
                //cboStoreAttribute.DataSource = storeData.StoreGroup_Read(eDataOrderBy.ID);
                // END MID Track 4246
                //cboStoreAttribute.ValueMember = "SG_RID";
                //cboStoreAttribute.DisplayMember = "SG_ID";
                ProfileList storeGroupList = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.All, true); //SAB.StoreServerSession.GetStoreGroupListViewList(eStoreGroupSelectType.All, true);
                FunctionSecurityProfile functionSecuritySizeConstraintsModels = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeConstraints);
                cboStoreAttribute.Initialize(SAB, functionSecuritySizeConstraintsModels, storeGroupList.ArrayList, true);

                //AdjustTextWidthComboBox_DropDown(cboStoreAttribute);  TT#1638 - MD - Revised Model Save - RBeck

                // End TT#1993
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //TT#1638 - Revised Model Save - RBeck
        //private void cbModelName_SelectedIndexChanged(object sender, System.EventArgs e)
        //protected override void cbModelName_SelectedIndexChanged(object sender, System.EventArgs e)
        protected override void cbModelName_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            try
            {
                if (FormLoaded)
                {
                    CheckForPendingChanges();
                    AfterClosing();  // handles rollback of grid changes

                    if (cbModelName.SelectedIndex >= 0)
                    {
                        DataRowView aRow = (DataRowView)cbModelName.SelectedItem;
                        this.SizeConstraintModelRid = Convert.ToInt32(aRow.Row["SIZE_CONSTRAINT_RID"]);
                        //Begin TT#1638 - MD _ BLANK model name error - rbeck
                        //InitializeForm(SizeConstraintModelRid);                     
                        //_currModelIndex = cbModelName.SelectedIndex;
                        //End   TT#1638 - MD _ BLANK model name error - rbeck
                        // Begin TT#194 - JSmith - Size constraints models -> blank rows are existing models-> delete flag on DB
                        // Begin TT#398-MD - JSmith - Cannot save changes in Size Constraint Model after making changes->Unable to cast object of type error message
                        InitializeForm(SizeConstraintModelRid);
                        _currModelIndex = cbModelName.SelectedIndex;
                        btnSave.Enabled = true;
                        btnDelete.Enabled = true;
                        btnInUse.Enabled = true;    //TT#110-MD-VStuart - In Use Tool
                        //if (cbModelName.SelectedIndex > 0)
                        //{
                        //    //Begin TT#1638 - MD - BLANK model name error - rbeck
                        //    InitializeForm(SizeConstraintModelRid);
                        //    _currModelIndex = cbModelName.SelectedIndex;
                        //    //End   TT#1638 - MD - BLANK model name error - rbeck
                        //    btnSave.Enabled = true;
                        //    btnDelete.Enabled = true;
                        //}
                        //else
                        //{
                        //    btnSave.Enabled = false;
                        //    btnDelete.Enabled = false;
                        //}
                        // End TT#398-MD - JSmith - Cannot save changes in Size Constraint Model after making changes->Unable to cast object of type error message
                        // End TT#194
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void cboSizeGroup_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            TransactionData td = new TransactionData();
            try
            {
                if (base.FormLoaded)
                {
                    // Begin TT#3228 - JSmith - Creating new Size Constraint Model- try to delete size group and get Null REf Exception error
                    //int newRid = Convert.ToInt32(cboSizeGroup.SelectedValue.ToString(), CultureInfo.CurrentUICulture); ;
                    int newRid;
                    if (cboSizeGroup.SelectedValue == null)
                    {
                        newRid = Include.NoRID;
                    }
                    else
                    {
                        newRid = Convert.ToInt32(cboSizeGroup.SelectedValue.ToString(), CultureInfo.CurrentUICulture);
                    }
                    // End TT#3228 - JSmith - Creating new Size Constraint Model- try to delete size group and get Null REf Exception error
                    if (newRid != SizeGroupRid) // If selection has really changed...
                    {
                        if (PromptSizeChange)
                        {
                            if (ShowWarningPrompt() == DialogResult.Yes)
                            {
                                td.OpenUpdateConnection();
                                DeleteSizeConstraintChildren(td);
                                td.CommitData();

                                this.SizeGroupRid = newRid;
                                if (SizeGroupRid != Include.NoRID)
                                {
                                    // Change the other (Size Curve) combo to "empty"
                                    if (this.SizeCurveGroupRid != Include.NoRID)
                                    {
                                        SizeCurveGroupRid = Include.NoRID; ;
                                        PromptSizeChange = false;
                                        cboSizeCurve.SelectedValue = Include.NoRID;
                                    }
                                    GetSizesUsing = eGetSizes.SizeGroupRID;
                                    GetDimensionsUsing = eGetDimensions.SizeGroupRID;
                                }

                                this.CreateConstraintData();
                                BindAllSizeGrid(this.SizeConstraints);
                                CheckExpandAll();
                            }
                            else
                            {
                                //Shut off the prompt so the combo can be reset to original value.
                                PromptSizeChange = false;
                                cboSizeGroup.SelectedValue = this.SizeConstraintProfile.SizeGroupRid;
                            }
                        }
                        else
                        {
                            //Turn the prompt back on.
                            PromptSizeChange = true;
                        }
                    }
                }
            }
            catch (DatabaseForeignKeyViolation)
            {
                td.Rollback();
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
                return;
            }
            catch (Exception ex)
            {
                td.Rollback();
                HandleException(ex, "cboSizeGroup_SelectedIndexChanged");
            }
            finally
            {
                td.CloseUpdateConnection();
            }
        }

        private void cboSizeCurve_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            TransactionData td = new TransactionData();
            try
            {
                if (base.FormLoaded)
                {
                    // Begin TT#3228 - JSmith - Creating new Size Constraint Model- try to delete size group and get Null REf Exception error
                    //int newRid = Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(), CultureInfo.CurrentUICulture); ;
                    int newRid;
                    if (cboSizeCurve.SelectedValue == null)
                    {
                        newRid = Include.NoRID;
                    }
                    else
                    {
                        newRid = Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(), CultureInfo.CurrentUICulture);
                    }
                    // End TT#3228 - JSmith - Creating new Size Constraint Model- try to delete size group and get Null REf Exception error
                    if (newRid != SizeCurveGroupRid) // If selection has really changed...
                    {

                        if (PromptSizeChange)
                        {
                            if (ShowWarningPrompt() == DialogResult.Yes)
                            {
                                td.OpenUpdateConnection();
                                DeleteSizeConstraintChildren(td);
                                td.CommitData();

                                this.SizeCurveGroupRid = newRid;
                                if (SizeCurveGroupRid != Include.NoRID)
                                {
                                    // Change the other (Size Curve) combo to "empty"
                                    if (this.SizeGroupRid != Include.NoRID)
                                    {
                                        SizeGroupRid = Include.NoRID; ;
                                        PromptSizeChange = false;
                                        cboSizeGroup.SelectedValue = Include.NoRID;
                                    }
                                    GetSizesUsing = eGetSizes.SizeCurveGroupRID;
                                    GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
                                }

                                this.CreateConstraintData();
                                BindAllSizeGrid(this.SizeConstraints);
                                CheckExpandAll();
                            }
                            else
                            {
                                //Shut off the prompt so the combo can be reset to original value.
                                PromptSizeChange = false;
                                cboSizeGroup.SelectedValue = this.SizeConstraintProfile.SizeGroupRid;
                            }
                        }
                        else
                        {
                            //Turn the prompt back on.
                            PromptSizeChange = true;
                        }
                    }
                }
            }
            catch (DatabaseForeignKeyViolation)
            {
                td.Rollback();
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
                return;
            }
            catch (Exception ex)
            {
                td.Rollback();
                HandleException(ex, "cboSizeCurve_SelectedIndexChanged");
            }
            finally
            {
                td.CloseUpdateConnection();
            }
        }

        private void cboStoreAttribute_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            TransactionData td = new TransactionData();
            try
            {
                if (base.FormLoaded)
                {
                    //					if (this.SizeConstraintProfile.StoreGroupRid == Include.NoRID)
                    //					{
                    //						if (cboStoreAttribute.SelectedValue != null)
                    //						{
                    //							this.StoreGroupRid = Convert.ToInt32(cboStoreAttribute.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
                    //							this.CreateConstraintData();
                    //							BindAllSizeGrid(this.SizeConstraints);
                    //						}
                    //					}
                    //					else
                    {
                        if (PromptAttributeChange)
                        {
                            if (ShowWarningPrompt() == DialogResult.Yes)
                            {
                                //Changing store attribute will not cause equate data to be deleted.
                                //EquateRollback = false;

                                //Changing store attribute will cause fringe data to be deleted.
                                //FringeRollback = true;

                                td.OpenUpdateConnection();
                                DeleteSizeConstraintChildren(td);
                                td.CommitData();
                                this.StoreGroupRid = Convert.ToInt32(cboStoreAttribute.SelectedValue.ToString(), CultureInfo.CurrentUICulture);
                                this.CreateConstraintData();
                                BindAllSizeGrid(this.SizeConstraints);
                                CheckExpandAll();
                            }
                            else
                            {
                                //Shut off the prompt so the combo can be reset to original value.
                                PromptAttributeChange = false;
                                cboStoreAttribute.SelectedValue = this.SizeConstraintProfile.StoreGroupRid;
                            }
                        }
                        else
                        {
                            //Turn the prompt back on.
                            PromptAttributeChange = true;
                        }
                    }



                }
            }
            catch (DatabaseForeignKeyViolation)
            {
                td.Rollback();
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
                return;
            }
            catch (Exception ex)
            {
                td.Rollback();
                HandleException(ex, "cboStoreAttribute_SelectedIndexChanged");
            }
            finally
            {
                td.CloseUpdateConnection();
            }
        }

        private void cboStoreAttribute_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboStoreAttribute_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void cboStoreAttribute_DragDrop(object sender, DragEventArgs e)
        {
            //Begin Track #5858 - Kjohnson - Validating store security only
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }

        /// <summary>
        /// Private method, enables or disables the buttons on the AddNewBox on ugModel.
        /// Called from ugModel_AfterRowActivate
        /// </summary>
        /// <param name="Defaults"></param>
        private void ToggleAddNewBoxButtons(bool Defaults)
        {
            try
            {
                //THESE TWO BANDS SHOULD NEVER BE ALLOWED TO ADD NEW OR DELETE
                //============================================================
                ugModel.DisplayLayout.Bands[C_SET].Override.AllowAddNew = AllowAddNew.No;
                ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Override.AllowAddNew = AllowAddNew.No;
                ugModel.DisplayLayout.Bands[C_SET].Override.AllowDelete = DefaultableBoolean.False;
                ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Override.AllowDelete = DefaultableBoolean.False;
                //============================================================

                if (FunctionSecurity.AllowUpdate)
                {

                    if (Defaults)
                    {
                        //SETUP FOR WHEN THE ACTIVE ROW IS NOT THE ALLSIZE ROW FROM THE SET BAND
                        ugModel.DisplayLayout.Bands[C_SET_CLR].Override.AllowAddNew = AllowAddNew.Yes;

                        if (this.SizeConstraintProfile.SizeCurveGroupRid != Include.NoRID)
                        {
                            ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Override.AllowAddNew = AllowAddNew.Yes;
                            ugModel.DisplayLayout.Bands[C_CLR_SZ].Override.AllowAddNew = AllowAddNew.Yes;
                            ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Override.AllowAddNew = AllowAddNew.Yes;
                            ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Override.AllowAddNew = AllowAddNew.Yes;
                        }
                    }
                    else
                    {
                        //SETUP FOR WHEN THE ACTIVE ROW IS THE ALLSIZE ROW FROM THE SET BAND
                        ugModel.DisplayLayout.Bands[C_SET_CLR].Override.AllowAddNew = AllowAddNew.No;

                        //if (_fillSizeHolesMethod.SizeGroupRid != Include.NoRID)
                        if (SizeConstraintProfile.SizeCurveGroupRid != Include.NoRID)
                        {
                            ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Override.AllowAddNew = AllowAddNew.No;
                            ugModel.DisplayLayout.Bands[C_CLR_SZ].Override.AllowAddNew = AllowAddNew.No;
                            ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Override.AllowAddNew = AllowAddNew.No;
                            ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Override.AllowAddNew = AllowAddNew.No;
                        }
                    }
                }
                else
                {
                    foreach (UltraGridBand ugb in ugModel.DisplayLayout.Bands)
                    {
                        ugb.Override.AllowAddNew = AllowAddNew.No;
                        ugb.Override.AllowDelete = DefaultableBoolean.False;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "ToggleAddNewBoxButtons");
            }

        }

        private void ugModel_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                UltraGrid myGrid = (UltraGrid)sender;
                UltraGridRow activeRow = e.Cell.Row;

                switch (e.Cell.Column.Key.ToUpper())
                {
                    case "FZ_IND":
                        //DEFAULT THE FILL_ZEROS_QUANTITY IF THE CURRENT VALUE IS ''
                        if ((bool)e.Cell.Value)
                        {
                            if (activeRow.Cells["FILL_ZEROS_QUANTITY"].Text.Trim() == string.Empty)
                            {
                                activeRow.Cells["FILL_ZEROS_QUANTITY"].Value = DEFAULT_FILL_ZEROS_QUANTITY.ToString();
                            }
                        }
                        else
                        {
                            if (activeRow.Cells["FILL_ZEROS_QUANTITY"].Text.Trim() != string.Empty)
                            {
                                activeRow.Cells["FILL_ZEROS_QUANTITY"].Value = string.Empty;
                            }
                        }
                        break;
                    case "DIMENSIONS_RID":
                        switch (activeRow.Band.Key.ToUpper())
                        {
                            case C_CLR_SZ_DIM:
                                //Pass the Size Band for the Dimension.
                                RemapSizesToNewDimension(activeRow.ChildBands[C_CLR_SZ]);
                                break;
                            case C_ALL_CLR_SZ_DIM:
                                //Pass the Size Band for the Dimension.
                                RemapSizesToNewDimension(activeRow.ChildBands[C_ALL_CLR_SZ]);
                                break;
                        }

                        break;
                }

                if (EditByCell)
                {
                    IsActiveRowValid(activeRow);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "ugModel_AfterCellUpdate");
            }
        }

        private void ugModel_AfterRowActivate(object sender, System.EventArgs e)
        {
            try
            {
                UltraGridRow myRow = ((UltraGrid)sender).ActiveRow;

                switch (myRow.Band.Key.ToUpper())
                {
                    case C_SET:
                        if ((eSizeMethodRowType)myRow.Cells["ROW_TYPE_ID"].Value == eSizeMethodRowType.AllSize)
                        {
                            ToggleAddNewBoxButtons(false);
                        }
                        else
                        {
                            ToggleAddNewBoxButtons(true);
                        }
                        break;
                    default:
                        ToggleAddNewBoxButtons(true);
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "ugModel_AfterRowActivate");
            }

        }

        private void ugModel_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            try
            {
                ChangePending = true;
                bool hasValues;
                UltraGrid myGrid = (UltraGrid)sender;

                switch (e.Row.Band.Key.ToUpper())
                {
                    case C_CLR_SZ:
                    case C_ALL_CLR_SZ:
                        hasValues = CreateSizeCellList(e.Row,
                            e.Row.Band,
                            myGrid.DisplayLayout.ValueLists["SizeCell"]);

                        if (!hasValues)
                        {
                            MessageBox.Show("All available sizes are being used for this dimension.\nThis row will be removed.",
                                "MIDRetail",
                                MessageBoxButtons.OK);
                            e.Row.Delete(false);
                        }
                        break;
                    case C_CLR_SZ_DIM:
                    case C_ALL_CLR_SZ_DIM:

                        hasValues = CreateDimensionCellList(e.Row,
                            e.Row.Band,
                            myGrid.DisplayLayout.ValueLists["DimensionCell"]);

                        if (!hasValues)
                        {
                            MessageBox.Show("All available dimensions are being used for this color.\nThis row will be removed.",
                                "MIDRetail",
                                MessageBoxButtons.OK);
                            e.Row.Delete(false);
                        }
                        break;
                    case C_SET_CLR:

                        hasValues = CreateColorCellList(e.Row,
                            e.Row.Band,
                            myGrid.DisplayLayout.ValueLists["ColorCell"]);

                        if (!hasValues)
                        {
                            MessageBox.Show("All available colors are being used for this set.\nThis row will be removed.",
                                "MIDRetail",
                                MessageBoxButtons.OK);
                            e.Row.Delete(false);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                e.Row.Delete(false);
                HandleException(ex, "ugModel_BeforeEnterEditMode");
            }

        }

        private void ugModel_BeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                UltraGrid myGrid = (UltraGrid)sender;
                UltraGridRow myRow = myGrid.ActiveRow;

                switch (myRow.Band.Key.ToUpper())
                {
                    case C_CLR_SZ:
                    case C_ALL_CLR_SZ:
                        if (myGrid.ActiveCell.Column.Key == "SIZE_CODE_RID")
                        {
                            CreateSizeCellList(myRow,
                                myGrid.ActiveCell.Band,
                                myGrid.DisplayLayout.ValueLists["SizeCell"]);

                            myGrid.ActiveCell.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                            myGrid.ActiveCell.ValueList = myGrid.DisplayLayout.ValueLists["SizeCell"];
                        }
                        break;
                    case C_CLR_SZ_DIM:
                    case C_ALL_CLR_SZ_DIM:
                        if (myGrid.ActiveCell.Column.Key == "DIMENSIONS_RID")
                        {
                            CreateDimensionCellList(myRow,
                                myGrid.ActiveCell.Band,
                                myGrid.DisplayLayout.ValueLists["DimensionCell"]);

                            myGrid.ActiveCell.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                            myGrid.ActiveCell.ValueList = myGrid.DisplayLayout.ValueLists["DimensionCell"];
                        }
                        break;
                    case C_SET_CLR:
                        if (myGrid.ActiveCell.Column.Key == "COLOR_CODE_RID")
                        {
                            CreateColorCellList(myRow,
                                myGrid.ActiveCell.Band,
                                myGrid.DisplayLayout.ValueLists["ColorCell"]);

                            myGrid.ActiveCell.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                            myGrid.ActiveCell.ValueList = myGrid.DisplayLayout.ValueLists["ColorCell"];
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                HandleException(ex, "ugModel_BeforeEnterEditMode");
            }
        }

        private void ugModel_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        {
            try
            {
                switch (this.ugModel.ActiveCell.Column.Key)
                {
                    case "SIZE_MIN":
                        if (!IsValidMinMax(ugModel.ActiveRow, ugModel.ActiveCell.Text, ugModel.ActiveRow.Cells["SIZE_MAX"].Text))
                        {
                            e.Cancel = true;
                            //_invalidCell = ugModel.ActiveCell;
                        }
                        break;
                    case "SIZE_MAX":
                        if (!IsValidMinMax(ugModel.ActiveRow, ugModel.ActiveRow.Cells["SIZE_MIN"].Text, ugModel.ActiveCell.Text))
                        {
                            e.Cancel = true;
                            //_invalidCell = ugModel.ActiveCell;
                        }
                        break;
                }
            }
            catch (FormatException)
            {
                string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FormatInvalid);
                MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;

            }
            catch (Exception err)
            {
                // Begin MID Issue 2453
                e.Cancel = true;
                this.HandleException(err, false);
                // End MID Issue 2453
            }
        }

        private bool IsValidMinMax(UltraGridRow row, string minValue, string maxValue)
        {
            bool valid = true;

            //=======================================================================
            // If either MIN or MAX is Null or empty, then there is no reason to compare them
            //=======================================================================
            if (minValue == null || maxValue == null)
                valid = true;
            else if (minValue == string.Empty || maxValue == string.Empty)
                valid = true;
            else
            {
                int min = Convert.ToInt32(minValue);
                int max = Convert.ToInt32(maxValue);

                if (min > max)
                {
                    string errorMessage = "Minimum must be less than or equal to Maximum.";
                    MessageBox.Show(errorMessage, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    valid = false;
                }
            }
            return valid;
        }

        private void ugModel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {

                    if (!ShowContext)
                    {
                        return;
                    }

                    UltraGrid myGrid = (UltraGrid)sender;
                    UltraGridRow activeRow = myGrid.ActiveRow;

                    switch (activeRow.Band.Key.ToUpper())
                    {
                        case C_SET:
                            if ((eSizeMethodRowType)(int)activeRow.Cells["ROW_TYPE_ID"].Value == eSizeMethodRowType.AllSize)
                            {
                                SetContext.MenuItems[C_CONTEXT_SET_ADD_CLR].Enabled = false;
                            }
                            else
                            {
                                SetContext.MenuItems[C_CONTEXT_SET_ADD_CLR].Enabled = true;
                            }
                            myGrid.ContextMenu = SetContext;
                            break;
                        case C_CLR_SZ_DIM:
                        case C_ALL_CLR_SZ_DIM:
                            myGrid.ContextMenu = DimensionContext;
                            break;
                        case C_CLR_SZ:
                        case C_ALL_CLR_SZ:
                            myGrid.ContextMenu = SizeContext;
                            break;
                        case C_SET_CLR:
                        case C_SET_ALL_CLR:

                            if ((eSizeMethodRowType)(int)activeRow.Cells["ROW_TYPE_ID"].Value == eSizeMethodRowType.Color)
                            {
                                ColorContext.MenuItems[C_CONTEXT_CLR_ADD_CLR].Enabled = true;
                                ColorContext.MenuItems[C_CONTEXT_CLR_DELETE].Enabled = true;
                            }
                            else
                            {
                                ColorContext.MenuItems[C_CONTEXT_CLR_ADD_CLR].Enabled = false;
                                ColorContext.MenuItems[C_CONTEXT_CLR_DELETE].Enabled = false;
                            }

                            if (this.SizeConstraintProfile.SizeCurveGroupRid == Include.NoRID)
                            {
                                ColorContext.MenuItems[C_CONTEXT_CLR_ADD_SZ_DIM].Enabled = false;
                            }
                            else
                            {
                                ColorContext.MenuItems[C_CONTEXT_CLR_ADD_SZ_DIM].Enabled = true;
                            }

                            myGrid.ContextMenu = ColorContext;
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "ugModel_MouseDown");
            }
        }

        private void cbExpandAll_CheckedChanged(object sender, System.EventArgs e)
        {
            CheckExpandAll();
        }

        private void CheckExpandAll()
        {
            if (cbExpandAll.Checked)
                ugModel.Rows.ExpandAll(true);
            else
                ugModel.Rows.CollapseAll(true);
        }

        #region Size Dropdown Filter
        // BEGIN MID Track #4396 - Justin Bolles - Size Dropdown Filter
        //private void picBoxFilter_MouseHover(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        string message = MIDText.GetTextOnly((int)eMIDTextCode.tt_ClickToFilterDropDown);
        //        ToolTip.Active = true; 
        //        ToolTip.SetToolTip((System.Windows.Forms.Control)sender, message);
        //    }
        //    catch( Exception exception )
        //    {
        //        HandleException(exception);
        //    }
        //}
        protected override void BindComboBoxes(string picBoxName,
                                        string aFilterString,
                                        bool aCaseSensitive)
        {
            switch (picBoxName)
            {
                case "picBoxName":
                    BindModelNameComboBox(aFilterString, aCaseSensitive);
                    break;

                case "picBoxGroup":
                    BindSizeGroupComboBox(true, aFilterString, aCaseSensitive);
                    break;

                case "picBoxCurve":
                    BindSizeCurveComboBox(true, aFilterString, aCaseSensitive);
                    break;
            }
        }

        //        private void picBox_Click(object sender, System.EventArgs e)
        //        {
        ////			BindModelNameComboBox();

        //            try
        //            {	
        //                string enteredMask = string.Empty;
        //                bool caseSensitive = false;
        //                PictureBox picBox = (PictureBox)sender;

        //                if (CharMaskFromDialogOK(picBox, ref enteredMask, ref caseSensitive))
        //                {
        //                    base.FormLoaded = false;
        //                    //MessageBox.Show("Filter selection process not yet available");
        //                    switch (picBox.Name)
        //                    {
        //                        case "picBoxName":
        //                            BindModelNameComboBox(enteredMask, caseSensitive);
        //                            break;

        //                        case "picBoxGroup":
        //                            BindSizeGroupComboBox(true, enteredMask, caseSensitive);
        //                            break;

        //                        case "picBoxCurve":
        //                            BindSizeCurveComboBox(true, enteredMask, caseSensitive);
        //                            break;

        //                    }
        //                    base.FormLoaded = true;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                HandleException(ex);
        //            }
        //        }

        protected void DisplayPictureBoxImages()
        {
            DisplayPictureBoxImage(picBoxName);
            DisplayPictureBoxImage(picBoxGroup);
            DisplayPictureBoxImage(picBoxCurve);
        }

        protected void SetPictureBoxTags()
        {
            picBoxName.Tag = SAB.ApplicationServerSession.GlobalOptions.SizeConstraintCharMask;
            picBoxGroup.Tag = SAB.ApplicationServerSession.GlobalOptions.SizeGroupCharMask;
            picBoxCurve.Tag = SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask;
        }

        protected void SetMaskedComboBoxesEnabled()
        {
            if (SAB.ApplicationServerSession.GlobalOptions.SizeGroupCharMask != string.Empty)
            {
                this.cboSizeGroup.Enabled = false;
            }
            if (SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask != string.Empty)
            {
                this.cboSizeCurve.Enabled = false;
            }
            if (SAB.ApplicationServerSession.GlobalOptions.SizeConstraintCharMask != string.Empty)
            {
                this.cbModelName.Enabled = false;
            }

            picBoxName.Enabled = true;		// MID Track #5256 - If security View only, can't select Size Curves when mask 
            // exists becuse of above check, but picBox is disabled from SetReadOnly; 
            // this overrides SetReadOnly
        }

        // Begin TT#1638 - JSmith - Revised Model Save
        //private void DisplayPictureBoxImage(System.Windows.Forms.PictureBox aPicBox)
        //{
        //    Image image;
        //    try
        //    {
        //        image = Image.FromFile(MIDGraphics.ImageDir + "\\" + MIDGraphics.MagnifyingGlassImage);
        //        SizeF sizef = new SizeF(aPicBox.Width, aPicBox.Height);
        //        Size size = Size.Ceiling(sizef);
        //        Bitmap bitmap = new Bitmap(image, size);
        //        aPicBox.Image = bitmap;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        throw;
        //    }
        //}
        // End TT#1638 - JSmith - Revised Model Save

        //private bool CharMaskFromDialogOK(PictureBox aPicBox, ref string aEnteredMask, ref bool aCaseSensitive)
        //{
        //    bool maskOK = false;
        //    string errMessage = string.Empty;

        //    try
        //    {
        //        bool cancelAction = false;
        //        string dialogLabel = MIDText.GetTextOnly((int)eMIDTextCode.lbl_FilterSelection);
        //        string textLabel = MIDText.GetTextOnly((int)eMIDTextCode.lbl_FilterSelectionText);

        //        string globalMask = Convert.ToString(aPicBox.Tag, CultureInfo.CurrentUICulture);

        //        NameDialog nameDialog = new NameDialog(dialogLabel, textLabel, globalMask);
        //        nameDialog.AllowCaseSensitive();

        //        while (!(maskOK || cancelAction))
        //        {
        //            nameDialog.StartPosition = FormStartPosition.CenterParent;
        //            nameDialog.TreatEmptyAsCancel = false;
        //            DialogResult dialogResult = nameDialog.ShowDialog();

        //            if (dialogResult == DialogResult.Cancel)
        //                cancelAction = true;
        //            else
        //            {
        //                maskOK = false;
        //                aEnteredMask = nameDialog.TextValue.Trim();
        //                aCaseSensitive = nameDialog.CaseSensitive;
        //                maskOK = (globalMask == string.Empty) ? true : EnteredMaskOK(aPicBox, aEnteredMask, globalMask);

        //                if (!maskOK)
        //                {
        //                    errMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_FilterGlobalOptionMismatch);
        //                    MessageBox.Show(errMessage, dialogLabel, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }
        //            }
        //        }

        //        if (cancelAction)
        //        {
        //            maskOK = false;
        //        }
        //        else
        //        {
        //            nameDialog.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }

        //    if (!aEnteredMask.EndsWith("*"))
        //    {
        //        aEnteredMask += "*";
        //    }

        //    return maskOK;
        //}

        //private bool EnteredMaskOK(PictureBox aPicBox, string aEnteredMask, string aGlobalMask)
        //{
        //    bool maskOK = true;
        //    try
        //    {
        //        //bool okToContinue = true;
        //        int gXCount = 0;
        //        int eXCount = 0;
        //        int gWCount = 0;
        //        int eWCount = 0;
        //        char wildCard = '*';

        //        char[] cGlobalArray = aGlobalMask.ToCharArray(0, aGlobalMask.Length);
        //        for (int i = 0; i < cGlobalArray.Length; i++)
        //        {
        //            if (cGlobalArray[i] == wildCard)
        //            {
        //                gWCount++;
        //            }
        //            else
        //            {
        //                gXCount++;
        //            }
        //        }
        //        char[] cEnteredArray = aEnteredMask.ToCharArray(0, aEnteredMask.Length);
        //        for (int i = 0; i < cEnteredArray.Length; i++)
        //        {
        //            if (cEnteredArray[i] == wildCard)
        //            {
        //                eWCount++;
        //            }
        //            else
        //            {
        //                eXCount++;
        //            }
        //        }

        //        if (eXCount < gXCount)
        //        {
        //            maskOK = false;
        //        }
        //        else if (eXCount > gXCount && gWCount == 0)
        //        {
        //            maskOK = false;
        //        }
        //        else if (aEnteredMask.Length < aGlobalMask.Length && !aGlobalMask.EndsWith("*"))
        //        {
        //            maskOK = false;
        //        }
        //        string[] globalParts = aGlobalMask.Split(new char[] { '*' });
        //        string[] enteredParts = aEnteredMask.Split(new char[] { '*' });
        //        int gLastEntry = globalParts.Length - 1;
        //        int eLastEntry = enteredParts.Length - 1;
        //        if (enteredParts[0].Length < globalParts[0].Length)
        //        {
        //            maskOK = false;
        //        }
        //        else if (enteredParts[eLastEntry].Length < globalParts[gLastEntry].Length)
        //        {
        //            maskOK = false;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return maskOK;
        //}

        //private void btnCancel_Click(object sender, System.EventArgs e)
        //{

        //}
        // END MID Track #4396 - Justin Bolles - Size Dropdown Filter
        #endregion

        private void ugModel_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            // End TT#1164
            //End TT#169
        }

        public void pictureBox1_Click(object sender, System.EventArgs e)
        {
            try
            {
                string enteredMask = string.Empty;
                bool caseSensitive = false;
                PictureBox picBox = (PictureBox)sender;

                if (CharMaskFromDialogOK(picBox, ref enteredMask, ref caseSensitive))
                {
                    base.FormLoaded = false;
                    //MessageBox.Show("Filter selection process not yet available");
                    string _picBoxName = picBox.Name;
                    BindComboBoxes(_picBoxName, enteredMask, caseSensitive);
                    base.FormLoaded = true;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void pictureBox1_MouseHover(object sender, System.EventArgs e)
        {
            try
            {
                string message = MIDText.GetTextOnly((int)eMIDTextCode.tt_ClickToFilterDropDown);
                ToolTip.Active = true;
                ToolTip.SetToolTip((System.Windows.Forms.Control)sender, message);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }
    }
}
