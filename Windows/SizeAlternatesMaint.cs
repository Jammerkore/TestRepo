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

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Shared;

using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for SizeAlternatesMaint.
	/// </summary>
	public class SizeAlternatesMaint : ModelFormBase
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        //private Infragistics.Win.UltraWinGrid.UltraGrid ugModel;
        //private System.Windows.Forms.ComboBox cbModelName;
        //private SessionAddressBlock _sab; //TT#1638 - Revised Model Save - RBeck
        //private int _sizeAltRid;

//Begin     TT#1638 - Revised Model Save - RBeck
        //private int _modelRID = Include.NoRID;  
        //private bool _newModel = false;
        //private bool _modelLocked = false;
        //private string _saveAsName;
//End       TT#1638 - Revised Model Save - RBeck

 		private int _currModelIndex = -1;
		private SizeAltModelProfile _sizeAlternateProfile;
		private bool PromptSizeChange = false;
		private SizeModelData _sizeModelData;
		private int _seq;
		private DataSet _dsAlt;
		private bool _showWarningPrompt = true;
		private ArrayList _errMsgs = new ArrayList();
		private SizeCurveGroupProfile _primSizeCurveGroupProfile;
		private SizeCurveGroupProfile _altSizeCurveGroupProfile;

		System.Collections.ArrayList _sizeCodeKeyList; 
		System.Collections.ArrayList _sizeCodeIDList;
		System.Collections.ArrayList _dimIDList;
		System.Collections.ArrayList _sizeKeyList;
		System.Collections.ArrayList _dimKeyList; 
		System.Collections.ArrayList _sizeIDList; 

		System.Collections.ArrayList _altSizeCodeKeyList; 
		System.Collections.ArrayList _altSizeCodeIDList;
		System.Collections.ArrayList _altDimIDList; 
		System.Collections.ArrayList _altSizeKeyList; 
		System.Collections.ArrayList _altDimKeyList;
     
        private System.Windows.Forms.CheckBox cbExpandAll;

//Begin     TT#1638 - MD - Revised Model Save - RBeck
        //private System.Windows.Forms.ComboBox cboPrimSizeCurve;
        //private System.Windows.Forms.ComboBox cboAltSizeCurve;

        private MIDComboBoxEnh cboPrimSizeCurve;
        private MIDComboBoxEnh cboAltSizeCurve;
//End     TT#1638 - MD - Revised Model Save - RBeck

        //protected System.Windows.Forms.PictureBox picBoxName;
		protected System.Windows.Forms.PictureBox picBoxPrimaryCurve;
		protected System.Windows.Forms.PictureBox picBoxAltCurve;
        //private System.Windows.Forms.Label lblModelName;
		private System.Windows.Forms.Label lblPrimarySizeCurve;
		private System.Windows.Forms.Label lblAlternateSizeCurve; 
		System.Collections.ArrayList _altSizeIDList; 
		private SizeCurve _sizeCurveData;			// MID Track #5343 

		protected ArrayList ErrorMessages
		{
			get {return _errMsgs;}
		}

		public SizeModelData SizeModelData
		{
			get	
			{
				if (_sizeModelData == null)
				{
					_sizeModelData = new SizeModelData();
				}
				return _sizeModelData;
			}
		}

		public SizeAlternatesMaint(SessionAddressBlock SAB) : base (SAB)
		{
            //_sab = SAB;//TT#1638 - Revised Model Save - RBeck
			InitializeComponent();

			FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeAlternates);

			_sizeCodeKeyList = new ArrayList(); 
			_sizeCodeIDList = new ArrayList();
			_dimIDList = new ArrayList(); 
			_sizeKeyList = new ArrayList(); 
			_dimKeyList = new ArrayList();
			_sizeIDList = new ArrayList();

			_altSizeCodeKeyList = new ArrayList(); 
			_altSizeCodeIDList = new ArrayList();
			_altDimIDList = new ArrayList(); 
			_altSizeKeyList = new ArrayList(); 
			_altDimKeyList = new ArrayList(); 
			_altSizeIDList = new ArrayList();
			_sizeCurveData = new SizeCurve();		// MID Track #5343 
			DisplayPictureBoxImages();
			SetPictureBoxTags();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}

				this.ugModel.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugModel_AfterRowInsert);
				this.ugModel.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugModel_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ugModel);
                //End TT#169
                //this.cbModelName.SelectedIndexChanged -= new System.EventHandler(this.cbModelName_SelectedIndexChanged);
				this.cboPrimSizeCurve.SelectedIndexChanged -= new System.EventHandler(this.cboPrimSizeCurve_SelectedIndexChanged);
				this.cboAltSizeCurve.SelectedIndexChanged -= new System.EventHandler(this.cboAltSizeCurve_SelectedIndexChanged);
				this.Load -= new System.EventHandler(this.SizeAlternatesMaint_Load);
			}
			base.Dispose( disposing );
		}

		// BEGIN MID Track #4970 - modify to emulate other models 
		override protected void AfterClosing()
		{
			try
			{
				CheckForDequeue();
			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeAlternatesMaint.AfterClosing");
			}
		}
		// END MID Track #4970  

		private void SizeAlternatesMaint_Load(object sender, System.EventArgs e)
		{
			try
			{		
				base.FormLoaded = false;

				if (FunctionSecurity.AllowUpdate)
				{
					Format_Title(eDataState.Updatable, eMIDTextCode.frm_SizeAlternativeModel, null);						
				}
				else
				{
					Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_SizeAlternativeModel, null);
//					this.ugModel.Enabled = false;
				}

				BindModelNameComboBox();
				BindBothSizeGroupsComboBox();
				
				// BEGIN MID Track #4970 - modify to emulate other models
				//_sizeAltRid = Include.NoRID;
				//if (cbModelName.SelectedIndex != Include.NoRID)
				//{
				//	DataRowView aRow = (DataRowView)cbModelName.SelectedItem;
				//	_sizeAltRid = Convert.ToInt32(aRow.Row["SIZE_ALTERNATE_RID"]);	
				//}
				// END MID Track #4970  

				SetReadOnly(FunctionSecurity.AllowUpdate);
				// BEGIN MID Track #4970 - modify to emulate other models
				//this.cbModelName.Enabled = true;
					
				//if (_sizeAltRid == Include.NoRID)
				//	InitializeForm();
				//else
				//{
				//	InitializeForm(_sizeAltRid);
				//}
				//===========================================
				// Specific security setting for this form
				//===========================================
				cbExpandAll.Enabled = true;
				//if (FunctionSecurity.AllowDelete && _sizeAltRid != Include.NoRID)
				//	btnDelete.Enabled = true;
			 
				btnSave.Enabled = false;
				btnDelete.Enabled = false;
                btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
				 
				cboAltSizeCurve.Enabled = false;
				cboPrimSizeCurve.Enabled = false;
				SetText();
				FormLoaded = true;
				// END MID Track #4970 
			}
			catch( Exception ex )
			{
				HandleException(ex, "SizeAlternatesMaint_Load");
			}
		}
		// BEGIN MID Track #4970 - additional logic
		private void SetText()
		{
			try
			{
				this.lblModelName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ModelName);
				this.lblPrimarySizeCurve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PrimarySizeCurve);
				this.lblAlternateSizeCurve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AlternateSizeCurve);
				this.cbExpandAll.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExpandAll);
			}
			catch  
			{
				throw;
			}
		}
		// END MID Track #4970  

        //BEGIN TT#110-MD-VStuart - In Use Tool
        public override void ShowInUse()
        {
            var emp = new SizeAltModelProfile(_modelRID);
            eProfileType type = emp.ProfileType;
            int rid = _modelRID;
            base.ShowInUse(type, rid, inQuiry2);
        }
        //END TT#110-MD-VStuart - In Use Tool

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.ugAltGrid
		/// </summary>
		private void InitializeComponent()
		{
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.lblPrimarySizeCurve = new System.Windows.Forms.Label();
            this.lblAlternateSizeCurve = new System.Windows.Forms.Label();
            this.cboPrimSizeCurve = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboAltSizeCurve = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cbExpandAll = new System.Windows.Forms.CheckBox();
            this.picBoxPrimaryCurve = new System.Windows.Forms.PictureBox();
            this.picBoxAltCurve = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxPrimaryCurve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAltCurve)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(391, 16);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(631, 16);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(551, 16);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Location = new System.Drawing.Point(471, 16);
            // 
            // picBoxName
            // 
            this.picBoxName.Location = new System.Drawing.Point(114, 16);
            this.picBoxName.TabIndex = 55;
            // 
            // cbModelName
            // 
            this.cbModelName.Location = new System.Drawing.Point(138, 16);
            this.cbModelName.TabIndex = 2;
            // 
            // lblModelName
            // 
            this.lblModelName.Location = new System.Drawing.Point(32, 18);
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
            this.ugModel.Location = new System.Drawing.Point(25, 146);
            this.ugModel.Size = new System.Drawing.Size(670, 310);
            this.ugModel.TabIndex = 1;
            this.ugModel.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugModel_InitializeLayout);
            this.ugModel.AfterExitEditMode += new System.EventHandler(this.ugModel_AfterExitEditMode);
            this.ugModel.AfterRowsDeleted += new System.EventHandler(this.ugModel_AfterRowsDeleted);
            this.ugModel.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugModel_AfterRowInsert);
            this.ugModel.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ugModel_BeforeRowInsert);
            this.ugModel.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugModel_MouseEnterElement);
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
            // lblPrimarySizeCurve
            // 
            this.lblPrimarySizeCurve.Location = new System.Drawing.Point(8, 48);
            this.lblPrimarySizeCurve.Name = "lblPrimarySizeCurve";
            this.lblPrimarySizeCurve.Size = new System.Drawing.Size(100, 16);
            this.lblPrimarySizeCurve.TabIndex = 3;
            this.lblPrimarySizeCurve.Text = "Primary Size Curve";
            this.lblPrimarySizeCurve.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAlternateSizeCurve
            // 
            this.lblAlternateSizeCurve.Location = new System.Drawing.Point(0, 80);
            this.lblAlternateSizeCurve.Name = "lblAlternateSizeCurve";
            this.lblAlternateSizeCurve.Size = new System.Drawing.Size(108, 20);
            this.lblAlternateSizeCurve.TabIndex = 4;
            this.lblAlternateSizeCurve.Text = "Alternate Size Curve";
            this.lblAlternateSizeCurve.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboPrimSizeCurve
            // 
            this.cboPrimSizeCurve.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPrimSizeCurve.AutoAdjust = true;
            this.cboPrimSizeCurve.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboPrimSizeCurve.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPrimSizeCurve.DataSource = null;
            this.cboPrimSizeCurve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrimSizeCurve.DropDownWidth = 243;
            this.cboPrimSizeCurve.FormattingEnabled = false;
            this.cboPrimSizeCurve.IgnoreFocusLost = false;
            this.cboPrimSizeCurve.ItemHeight = 13;
            this.cboPrimSizeCurve.Location = new System.Drawing.Point(138, 48);
            this.cboPrimSizeCurve.Margin = new System.Windows.Forms.Padding(0);
            this.cboPrimSizeCurve.MaxDropDownItems = 8;
            this.cboPrimSizeCurve.Name = "cboPrimSizeCurve";
            this.cboPrimSizeCurve.Size = new System.Drawing.Size(248, 21);
            this.cboPrimSizeCurve.TabIndex = 5;
            this.cboPrimSizeCurve.Tag = null;
            this.cboPrimSizeCurve.SelectedIndexChanged += new System.EventHandler(this.cboPrimSizeCurve_SelectedIndexChanged);
            // 
            // cboAltSizeCurve
            // 
            this.cboAltSizeCurve.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAltSizeCurve.AutoAdjust = true;
            this.cboAltSizeCurve.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAltSizeCurve.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAltSizeCurve.DataSource = null;
            this.cboAltSizeCurve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAltSizeCurve.DropDownWidth = 243;
            this.cboAltSizeCurve.FormattingEnabled = false;
            this.cboAltSizeCurve.IgnoreFocusLost = false;
            this.cboAltSizeCurve.ItemHeight = 13;
            this.cboAltSizeCurve.Location = new System.Drawing.Point(138, 80);
            this.cboAltSizeCurve.Margin = new System.Windows.Forms.Padding(0);
            this.cboAltSizeCurve.MaxDropDownItems = 8;
            this.cboAltSizeCurve.Name = "cboAltSizeCurve";
            this.cboAltSizeCurve.Size = new System.Drawing.Size(248, 21);
            this.cboAltSizeCurve.TabIndex = 6;
            this.cboAltSizeCurve.Tag = null;
            this.cboAltSizeCurve.SelectedIndexChanged += new System.EventHandler(this.cboAltSizeCurve_SelectedIndexChanged);
            // 
            // cbExpandAll
            // 
            this.cbExpandAll.Location = new System.Drawing.Point(25, 112);
            this.cbExpandAll.Name = "cbExpandAll";
            this.cbExpandAll.Size = new System.Drawing.Size(104, 16);
            this.cbExpandAll.TabIndex = 11;
            this.cbExpandAll.Text = "Expand All";
            this.cbExpandAll.CheckedChanged += new System.EventHandler(this.cbExpandAll_CheckedChanged);
            // 
            // picBoxPrimaryCurve
            // 
            this.picBoxPrimaryCurve.Location = new System.Drawing.Point(114, 48);
            this.picBoxPrimaryCurve.Name = "picBoxPrimaryCurve";
            this.picBoxPrimaryCurve.Size = new System.Drawing.Size(19, 20);
            this.picBoxPrimaryCurve.TabIndex = 56;
            this.picBoxPrimaryCurve.TabStop = false;
            this.picBoxPrimaryCurve.Click += new System.EventHandler(this.pictureBox1_Click);
            this.picBoxPrimaryCurve.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            // 
            // picBoxAltCurve
            // 
            this.picBoxAltCurve.Location = new System.Drawing.Point(114, 80);
            this.picBoxAltCurve.Name = "picBoxAltCurve";
            this.picBoxAltCurve.Size = new System.Drawing.Size(19, 20);
            this.picBoxAltCurve.TabIndex = 57;
            this.picBoxAltCurve.TabStop = false;
            this.picBoxAltCurve.Click += new System.EventHandler(this.pictureBox1_Click);
            this.picBoxAltCurve.MouseHover += new System.EventHandler(this.pictureBox1_MouseHover);
            // 
            // SizeAlternatesMaint
            // 
            this.ClientSize = new System.Drawing.Size(720, 517);
            this.Controls.Add(this.picBoxAltCurve);
            this.Controls.Add(this.picBoxPrimaryCurve);
            this.Controls.Add(this.cbExpandAll);
            this.Controls.Add(this.cboAltSizeCurve);
            this.Controls.Add(this.cboPrimSizeCurve);
            this.Controls.Add(this.lblAlternateSizeCurve);
            this.Controls.Add(this.lblPrimarySizeCurve);
            this.Name = "SizeAlternatesMaint";
            this.Text = "Size Alternates Model Maintenance";
            this.Load += new System.EventHandler(this.SizeAlternatesMaint_Load);
            this.Controls.SetChildIndex(this.btnInUse, 0);
            this.Controls.SetChildIndex(this.btnNew, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnSaveAs, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.lblModelName, 0);
            this.Controls.SetChildIndex(this.ugModel, 0);
            this.Controls.SetChildIndex(this.cbModelName, 0);
            this.Controls.SetChildIndex(this.lblPrimarySizeCurve, 0);
            this.Controls.SetChildIndex(this.lblAlternateSizeCurve, 0);
            this.Controls.SetChildIndex(this.cboPrimSizeCurve, 0);
            this.Controls.SetChildIndex(this.cboAltSizeCurve, 0);
            this.Controls.SetChildIndex(this.cbExpandAll, 0);
            this.Controls.SetChildIndex(this.picBoxName, 0);
            this.Controls.SetChildIndex(this.picBoxPrimaryCurve, 0);
            this.Controls.SetChildIndex(this.picBoxAltCurve, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxPrimaryCurve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAltCurve)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		override public void InitializeForm()
		{
			try
			{
                FormLoaded = false;

                InitializeBaseForm(); 

                Cursor = Cursors.WaitCursor;
								 
				// BEGIN MID Track #4970 - modify to emulate other models	 
				cboPrimSizeCurve.Enabled = true;
				cboAltSizeCurve.Enabled = true;
				CheckForDequeue();
				// END MID Track #4970 
				 
				_sizeAlternateProfile = new SizeAltModelProfile(Include.NoRID);
				 
				//SetReadOnly(true);
				PromptSizeChange = false;

//Begin   TT#1638 - MD - Revised Model Save - RBeck
                //btnSave.Enabled = false;
                //btnSaveAs.Enabled = false;
                //btnDelete.Enabled = false;
                //_saveAsName = string.Empty;
                //_newModel = true;
                //this.cbModelName.SelectedIndex = -1;
                //this.cbModelName.Text = "(new model)";
                //this.cbModelName.Enabled = false;    
//End   TT#1638 - MD - Revised Model Save - RBeck

				if (PerformingNewModel)
				{
					if (SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask == string.Empty)
					{
						BindBothSizeGroupsComboBox();
					}

					if (cboPrimSizeCurve.Items.Count > 0)
					{
						cboPrimSizeCurve.SelectedIndex = 0;
						_sizeAlternateProfile.PrimarySizeCurveRid = Convert.ToInt32(cboPrimSizeCurve.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
					}
					if (cboAltSizeCurve.Items.Count > 0)
					{
						cboAltSizeCurve.SelectedIndex = 0;
						_sizeAlternateProfile.AlternateSizeCurveRid = Convert.ToInt32(cboAltSizeCurve.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
					}

					this.BindAlternateGrid();
				}

				Format_Title(eDataState.New, eMIDTextCode.frm_SizeAlternativeModel, null);
				DetermineControlsEnabled(true);	// MID Track #4970
				PromptSizeChange = true;
				ChangePending = false;
                //cbModelName.SelectedIndex = 0;//TT#1638 - MD - Revised Model Save - RBeck
				FormLoaded = true;

            }
			catch(Exception exception)
			{
				HandleException(exception);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}        
        
//Begin     TT#1638 - MD - Revised Model Save - RBeck
        override public bool InitializeForm(string modelName)
        {
            try
            {
                if (FormLoaded)
                {
                    CheckForPendingChanges();
                    if (cbModelName.SelectedIndex >= 0)
                    {
                        DataRowView aRow = (DataRowView)cbModelName.SelectedItem;
                        // BEGIN MID Track #5343 - Null object reference 
                        //this._sizeAlternateProfile.Key = Convert.ToInt32(aRow.Row["SIZE_ALTERNATE_RID"]);	
                        //InitializeForm(_sizeAlternateProfile.Key);
                        int altRID = Convert.ToInt32(aRow.Row["SIZE_ALTERNATE_RID"]);
                        InitializeForm(altRID);
                        return true;
                    }	// END MID Track #5343
                    return true;
                }
                return true;
            }
            catch (Exception exception)
            {
                HandleException(exception);
                return true;
            }
        }
//End    TT#1638 - MD - Revised Model Save - RBeck
        override public void InitializeForm(int modelRid)       //TT#1638 - MD - Revised Model Save - RBeck
		{
			try
			{
				base.FormLoaded = false;
			 	// BEGIN MID Track #4970 - modify to emulate other models
				eDataState dataState  = eDataState.New;
				bool allowUpdate = FunctionSecurity.AllowUpdate;
				Cursor = Cursors.WaitCursor;
				CheckForDequeue();
				
				_newModel = false;
			 
				if (modelRid > Include.NoRID)
				{
					if (allowUpdate)
					{
						_sizeAlternateProfile = (SizeAltModelProfile) SAB.HierarchyServerSession.GetModelDataForUpdate(eModelType.SizeAlternates, modelRid, true);  
						if (_sizeAlternateProfile.ModelLockStatus == eLockStatus.ReadOnly)
						{
							allowUpdate = false;
						}
						else if (_sizeAlternateProfile.ModelLockStatus == eLockStatus.Cancel)
						{
							this.cbModelName.SelectedIndex = _currModelIndex;
                            //return false;
                            return ;
						}
					}
					else
					{
						_sizeAlternateProfile = SAB.HierarchyServerSession.GetSizeAltModelData(modelRid);
					}

					if (!allowUpdate)
					{
						_modelLocked = false;
						dataState = eDataState.ReadOnly;
					}
					else
					{
						_modelLocked = true;
						dataState = eDataState.Updatable;
					}
				}
				else
				{
					_sizeAlternateProfile = new SizeAltModelProfile(modelRid);
				}

				_modelRID = _sizeAlternateProfile.Key;
				_saveAsName = string.Empty;

				if (SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask != string.Empty)
				{
					BindPrimarySizeGroupsComboBox(_sizeAlternateProfile.PrimarySizeCurveRid);
					BindAltSizeGroupsComboBox(_sizeAlternateProfile.AlternateSizeCurveRid);
				}
				else
				{
					BindBothSizeGroupsComboBox();
				}

                SizeAltModelProfile sAltP = SAB.HierarchyServerSession.GetSizeAltModelData(modelRid);  // TT#1638 - MD - Revised Model Save - RBeck
				
                PromptSizeChange = false;
				cboPrimSizeCurve.SelectedValue = _sizeAlternateProfile.PrimarySizeCurveRid;
				PromptSizeChange = false;
				cboAltSizeCurve.SelectedValue = _sizeAlternateProfile.AlternateSizeCurveRid;

				this.BindAlternateGrid();

                //Format_Title(dataState, eMIDTextCode.frm_SizeAlternativeModel, null);
                Format_Title(dataState, eMIDTextCode.frm_SizeAlternativeModel, sAltP.SizeAlternateName);  // TT#1638 - MD - Revised Model Save - RBeck
				DetermineControlsEnabled(allowUpdate);

				if (_modelRID == Include.NoRID)
				{
					cboPrimSizeCurve.Enabled = false;
					cboAltSizeCurve.Enabled = false;
				}
				// END MID Track #4970 
				PromptSizeChange = true;
				ChangePending = false;
				FormLoaded = true;
				CheckExpandAll();

				if (!FunctionSecurity.AllowUpdate)
				{
					SetControlReadOnly(this.ugModel, !FunctionSecurity.AllowUpdate);						
				}                
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
			finally
			{
				Cursor = Cursors.Default;
                //return true;
                 
			}
		}

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
                //    btnSave.Enabled = true;
                //    btnSaveAs.Enabled = true; 
                //}
//End       TT#1638 - MD - Revised Model Save - RBeck
	
			}
			catch
			{
				throw;
			}
		}	

		private bool LoadPrimarySizeArrays()
		{
			bool success = false;
			_sizeCodeKeyList.Clear();
			_sizeCodeIDList.Clear();
			_sizeKeyList.Clear();
			_sizeIDList.Clear();
			_dimKeyList.Clear();
			_dimIDList.Clear();

			if (this._sizeAlternateProfile.PrimarySizeCurveRid != Include.NoRID) 
			{
				_primSizeCurveGroupProfile = new SizeCurveGroupProfile(_sizeAlternateProfile.PrimarySizeCurveRid);
				ProfileList scl = _primSizeCurveGroupProfile.SizeCodeList;
				//productCatStr = ((SizeCodeProfile)(scl.ArrayList[0])).SizeCodeProductCategory;
				
				// begin MID Track 5812 Size Alternate Model Gets Error on Create
				if (scl.Count > 0)
				{
					// end MID Track 5812 Size Alternate Model Gets Error on Create
					foreach(SizeCodeProfile scp in scl.ArrayList) 
					{
						if (scp.Key == -1) 
						{
							throw new MIDException (eErrorLevel.severe,
								(int)eMIDTextCode.msg_CantRetrieveSizeCode,
								MIDText.GetText(eMIDTextCode.msg_CantRetrieveSizeCode));
						}
						_sizeCodeKeyList.Add(scp.Key);
						_sizeCodeIDList.Add(scp.SizeCodeID);
						if (!_sizeIDList.Contains(scp.SizeCodePrimary))
						{
							_sizeIDList.Add(scp.SizeCodePrimary);
							_sizeKeyList.Add(scp.SizeCodePrimaryRID);
						}
						if (!_dimIDList.Contains(scp.SizeCodeSecondary)) 
						{
							_dimIDList.Add(scp.SizeCodeSecondary);
							_dimKeyList.Add(scp.SizeCodeSecondaryRID);
						}
					}
					success = true;
				} // MID Track 5812 Size Alternate Model Gets Error on Create
			}

			return success;
		}

		private bool LoadAlternateSizeArrays()
		{
			bool success = false;
			_altSizeCodeKeyList.Clear();
			_altSizeCodeIDList.Clear();
			_altSizeKeyList.Clear();
			_altSizeIDList.Clear();
			_altDimKeyList.Clear();
			_altDimIDList.Clear();

			if (this._sizeAlternateProfile.AlternateSizeCurveRid != Include.NoRID) 
			{
				_altSizeCurveGroupProfile = new SizeCurveGroupProfile(_sizeAlternateProfile.AlternateSizeCurveRid);
				ProfileList scl = _altSizeCurveGroupProfile.SizeCodeList;

				//productCatStr = ((SizeCodeProfile)(scl.ArrayList[0])).SizeCodeProductCategory;
				
				foreach(SizeCodeProfile scp in scl.ArrayList) 
				{
					if (scp.Key == -1) 
					{
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_CantRetrieveSizeCode,
							MIDText.GetText(eMIDTextCode.msg_CantRetrieveSizeCode));
					}
					_altSizeCodeKeyList.Add(scp.Key);
					_altSizeCodeIDList.Add(scp.SizeCodeID);
					if (!_altSizeIDList.Contains(scp.SizeCodePrimary))
					{
						_altSizeIDList.Add(scp.SizeCodePrimary);
						_altSizeKeyList.Add(scp.SizeCodePrimaryRID);
					}
					if (!_altDimIDList.Contains(scp.SizeCodeSecondary)) 
					{
						_altDimIDList.Add(scp.SizeCodeSecondary);
						_altDimKeyList.Add(scp.SizeCodeSecondaryRID);
					}
				}
				success = true;
			}

			return success;
		}

		private void BindAlternateGrid() 
		{

//			SizeCodeList scl = this._sab.HierarchyServerSession.GetSizeCodeList(
//				productCatStr, eSearchContent.WholeField,
//				size, eSearchContent.WholeField, 
//				width, eSearchContent.WholeField);

			try
			{
//				string dim;
				bool proceed = LoadPrimarySizeArrays();
				if (proceed)
					proceed = LoadAlternateSizeArrays();
				if (proceed)
				{
					ugModel.DataSource = BuildAlternateDataSet();

					// build just the first primary row-empty
					if (this._sizeAlternateProfile.AlternateSizeList.Count == 0)
					{
//						dim = (string)_dimIDList[0];
//						if ((_dimIDList.Count == 1) && ((dim == Include.NoSecondarySize || dim.Trim() == string.Empty)))
//						{
//							object[] aRow = new object[4];
//							aRow[0] = 1;
//							aRow[1] = "Primary";
//							aRow[2] = "";
//							aRow[3] = this._sizeCodeIDList[0];
//							dsAlt.Tables["Primary"].Rows.Add(aRow);
//						}
//						else
						{
							object[] aRow = new object[4];
							aRow[0] = 1;
							aRow[1] = "Primary";
//							aRow[2] = this._dimIDList[0];
//							aRow[3] = this._sizeIDList[0];
							aRow[2] = this._dimKeyList[0];
							aRow[3] = this._sizeKeyList[0];

							_dsAlt.Tables["Primary"].Rows.Add(aRow);
						}
						_seq = 1;
						_showWarningPrompt = false;
					}
					else
					{
						_seq = 0;
						foreach (SizeAlternatePrimary sap in _sizeAlternateProfile.AlternateSizeList)
						{
							object[] aRow = new object[4];
							aRow[0] = sap.Sequence;
							_seq = sap.Sequence;
							aRow[1] = "Primary";
							aRow[2] = sap.DimensionRid;
							aRow[3] = sap.SizeRid;
							_dsAlt.Tables["Primary"].Rows.Add(aRow);
							foreach (SizeAlternate sa in sap.AlternateList)
							{
								object[] aAltRow = new object[4];
								aAltRow[0] = sap.Sequence;  //sap is not a mistake.  The seq should be same as parent.
								aAltRow[1] = "Alternate";
								aAltRow[2] = sa.DimensionRid;
								aAltRow[3] = sa.SizeRid;
								_dsAlt.Tables["Alternate"].Rows.Add(aAltRow);
							}
						}
						_showWarningPrompt = true;
					}


//					dim = (string)_dimIDList[0];
//					if ((_dimIDList.Count == 1) && ((dim == Include.NoSecondarySize || dim.Trim() == string.Empty)))
//					{
//						//ugModel.DisplayLayout.Bands[0].Columns["Dimension"].Hidden = true;
//						ugModel.DisplayLayout.Bands[0].Columns["size"].ValueList = ugModel.DisplayLayout.ValueLists["DimensionSize"];
//						ugModel.DisplayLayout.Bands[0].Columns["Dimension"].ValueList = ugModel.DisplayLayout.ValueLists["Dimensions"];
//					}
//					else
					{
						ugModel.DisplayLayout.Bands[0].Columns["Size"].ValueList = ugModel.DisplayLayout.ValueLists["Sizes"];
						ugModel.DisplayLayout.Bands[0].Columns["Dimension"].ValueList = ugModel.DisplayLayout.ValueLists["Dimensions"];
					}
//					dim = (string)_altDimIDList[0];
//					if ((_altDimIDList.Count == 1) && ((dim == Include.NoSecondarySize || dim.Trim() == string.Empty)))
//					{
//						//ugModel.DisplayLayout.Bands[1].Columns["Dimension"].Hidden = true;
//						ugModel.DisplayLayout.Bands[1].Columns["size"].ValueList = ugModel.DisplayLayout.ValueLists["AltDimensionSize"];
//						ugModel.DisplayLayout.Bands[1].Columns["Dimension"].ValueList = ugModel.DisplayLayout.ValueLists["AltDimensions"];
//					}
//					else
					{
						ugModel.DisplayLayout.Bands[1].Columns["Size"].ValueList = ugModel.DisplayLayout.ValueLists["AltSizes"];
						ugModel.DisplayLayout.Bands[1].Columns["Dimension"].ValueList = ugModel.DisplayLayout.ValueLists["AltDimensions"];
					}

				}
				else
				{
					ugModel.DataSource = null;
				}

			}
			catch
			{
				throw;
			}
		}

		private DataSet BuildAlternateDataSet()
		{
			_dsAlt = MIDEnvironment.CreateDataSet("Alternate");

			DataTable dtPrimary = MIDEnvironment.CreateDataTable("Primary");
			dtPrimary.Columns.Add("Seq");
			dtPrimary.Columns["Seq"].DataType = System.Type.GetType("System.Int32");
			dtPrimary.Columns.Add("Description");
			dtPrimary.Columns["Description"].Caption = string.Empty;
			dtPrimary.Columns.Add("Dimension");
			dtPrimary.Columns.Add("Size");
			dtPrimary.PrimaryKey = new DataColumn[] {dtPrimary.Columns["Seq"]};

			_dsAlt.Tables.Add(dtPrimary);

			DataTable dtAlternate = MIDEnvironment.CreateDataTable("Alternate");
			dtAlternate.Columns.Add("Seq");
			dtAlternate.Columns["Seq"].DataType = System.Type.GetType("System.Int32");
			dtAlternate.Columns.Add("Description");
			dtAlternate.Columns["Description"].Caption = string.Empty;
			dtAlternate.Columns.Add("Dimension");
			dtAlternate.Columns.Add("Size");

			_dsAlt.Tables.Add(dtAlternate);

			_dsAlt.Relations.Add(new DataRelation("Primary",
				new DataColumn[] { _dsAlt.Tables["Primary"].Columns["Seq"]
								 },
				new DataColumn[] { _dsAlt.Tables["Alternate"].Columns["Seq"]
								 }));

			return _dsAlt;
		}

		private void BindModelNameComboBox()
		{
			try
			{
				DataTable dtSizeModel = SizeModelData.SizeAlternateModel_Read();

//Begin TT#1638 - MD - Revised Model Save - RBeck 
                //DataRow row = dtSizeModel.NewRow();
                //row["SIZE_ALTERNATE_RID"] = Include.NoRID;
                //row["SIZE_ALTERNATE_NAME"] = string.Empty;
                //dtSizeModel.Rows.InsertAt(row, 0);
                //dtSizeModel.AcceptChanges();
//End   TT#1638 - MD - Revised Model Save - RBeck 

				cbModelName.DataSource = dtSizeModel;
				cbModelName.DisplayMember = "SIZE_ALTERNATE_NAME";
				cbModelName.ValueMember = "SIZE_ALTERNATE_RID";
                cbModelName.SelectedIndex = Include.Undefined;

                // AdjustTextWidthComboBox_DropDown(cbModelName); TT#1638 - MD - Revised Model Save - RBeck
                                 
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
                base.FormLoaded = false;
                // Originally tried RowFilter but received invalid rowfilter msg when multiple wild cards
                aFilterString = aFilterString.Replace("*", "%");
                //aFilterString = aFilterString.Replace("'", "''");	// for string with single quote

                //string whereClause = "SIZE_ALTERNATE_NAME LIKE " + "'" + aFilterString + "'";
                //if (!aCaseSensitive)
                //{
                //    whereClause += Include.CaseInsensitiveCollation;
                //}
                //DataTable dtSizeModel = SizeModelData.SizeAlternateModel_FilterRead(whereClause);
                DataTable dtSizeModel;
                if (aCaseSensitive)
                {
                    dtSizeModel = SizeModelData.SizeAlternateModel_FilterReadCaseSensitive(aFilterString);
                }
                else
                {
                    dtSizeModel = SizeModelData.SizeAlternateModel_FilterReadCaseInsensitive(aFilterString);
                }
                

//Begin TT#1638 - MD - Revised Model Save - RBeck
                //DataRow emptyRow = dtSizeModel.NewRow();
                //emptyRow["SIZE_ALTERNATE_NAME"] = "";
                //emptyRow["SIZE_ALTERNATE_RID"] = Include.NoRID;
                //dtSizeModel.Rows.Add(emptyRow);
                //dtSizeModel.DefaultView.Sort = "SIZE_ALTERNATE_NAME ASC";
                //dtSizeModel.AcceptChanges();
//End  TT#1638 - MD - Revised Model Save - RBeck

                cbModelName.DataSource = dtSizeModel;
                cbModelName.DisplayMember = "SIZE_ALTERNATE_NAME";
                cbModelName.ValueMember = "SIZE_ALTERNATE_RID";
                cbModelName.Enabled = true;
                base.FormLoaded = true;

                //AdjustTextWidthComboBox_DropDown(cbModelName); TT#1638 - MD - Revised Model Save - RBeck

            }
            catch (Exception ex)
            {
                HandleException(ex, "BindSizeConstraintsComboBox");
            }
        }
        // END MID Track #4396

		private void BindBothSizeGroupsComboBox()
		{
			try
			{	// BEGIN MID Track #5343 - improve visual - rearrange methods
				cboPrimSizeCurve.DataSource = null;				
				cboPrimSizeCurve.Items.Clear();
				cboAltSizeCurve.DataSource = null;
				cboAltSizeCurve.Items.Clear();					
				
				DataTable dtGroups = _sizeCurveData.GetSizeCurveGroups();
				DataTable dtAltGroups = _sizeCurveData.GetSizeCurveGroups();
				
				// PRIMARY
				cboPrimSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";
				cboPrimSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";
				cboPrimSizeCurve.DataSource = dtGroups;			// MID Track #5343 'DataRowView...' text displaying
																// move DataSource = .... after DisplayMember & ValueMember 

                //AdjustTextWidthComboBox_DropDown(cboPrimSizeCurve); TT#1638 - MD - Revised Model Save - RBeck  
                                 
				// ALTERNATE
				cboAltSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";
				cboAltSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";
				cboAltSizeCurve.DataSource = dtAltGroups;		// MID Track #5343 'DataRowView...' text displaying
																// move DataSource = .... after DisplayMember & ValueMember

                //AdjustTextWidthComboBox_DropDown(cboAltSizeCurve);  TT#1638 - MD - Revised Model Save - RBeck 
 
                // END MID Track #5343	
			}
			catch (Exception ex)
			{
				HandleException(ex, "BindBothSizeGroupComboBox");
			}
		}

		private void BindPrimarySizeGroupsComboBox(int aGroupRID)
		{
			try
			{
				// PRIMARY
				cboPrimSizeCurve.DataSource = null;
				cboPrimSizeCurve.Items.Clear();
				 
				DataTable dtGroups = _sizeCurveData.GetSpecificSizeCurveGroup(aGroupRID);
							
				//cboPrimSizeCurve.DataSource = dtGroups;					// MID Track #5343 'DataRowView...' text displaying
				cboPrimSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";
				cboPrimSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";
				cboPrimSizeCurve.DataSource = dtGroups;						// MID Track #5343 'DataRowView...' text displaying

                //AdjustTextWidthComboBox_DropDown(cboPrimSizeCurve);   TT#1638 - MD - Revised Model Save - RBeck
 
			}
			catch (Exception ex)
			{
				HandleException(ex, "BindBothSizeGroupComboBox");
			}
		}

		private void BindAltSizeGroupsComboBox(int aGroupRID)
		{
			try
			{
				// ALTERNATE
				cboAltSizeCurve.DataSource = null;
				cboAltSizeCurve.Items.Clear();

				DataTable dtAltGroups = _sizeCurveData.GetSpecificSizeCurveGroup(aGroupRID);
								
				//cboAltSizeCurve.DataSource = dtAltGroups;					// MID Track #5343 'DataRowView...' text displaying
				cboAltSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";
				cboAltSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";
				cboAltSizeCurve.DataSource = dtAltGroups;					// MID Track #5343 'DataRowView...' text displaying

                //AdjustTextWidthComboBox_DropDown(cboAltSizeCurve);  TT#1638 - MD - Revised Model Save - RBeck
			}
			catch (Exception ex)
			{
				HandleException(ex, "BindBothSizeGroupComboBox");
			}
		}

		private void CheckForDequeue()
		{
			try
			{
				if (_modelLocked)
				{
					SAB.HierarchyServerSession.DequeueModel(eModelType.SizeAlternates, _modelRID);
					_modelLocked = false;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		// BEGIN MID Track #4396 - Justin Bolles - Size Dropdown Filter
		virtual protected void BindAltCurveComboBox(bool includeEmptySelection, string aFilterString,
			bool aCaseSensitive)
		{
			try
			{
				base.FormLoaded = false;
				object selectedValue = cboAltSizeCurve.SelectedValue;
				DataTable dtSizeCurve = MIDEnvironment.CreateDataTable();
				SizeCurve objSizeCurve = new SizeCurve();
								
				// RowFilter didn't work with multiple wild cards 
				aFilterString = aFilterString.Replace("*","%");
                //aFilterString = aFilterString.Replace("'","''");	// for string with single quote
			
                //string whereClause = "SIZE_CURVE_GROUP_NAME LIKE " +  "'" + aFilterString + "'";	
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
					dtSizeCurve.Rows.Add(new object[] { Include.NoRID, ""} );
				}

				DataView dvSizeCurve = new DataView(dtSizeCurve, "", "SIZE_CURVE_GROUP_NAME", DataViewRowState.CurrentRows);
				cboAltSizeCurve.DataSource = dvSizeCurve;  
				cboAltSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";
				cboAltSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";
				cboAltSizeCurve.Enabled = true;

                //AdjustTextWidthComboBox_DropDown(cboAltSizeCurve); TT#1638 - MD - Revised Model Save - RBeck

				if (selectedValue != null)
				{
					cboAltSizeCurve.SelectedValue = selectedValue;
				}
				base.FormLoaded = true;
			}		
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeCurveComboBox");
			}
		}
		// END MID Track #4396

		// BEGIN MID Track #4396 - Justin Bolles - Size Dropdown Filter
		virtual protected void BindPrimaryCurveComboBox(bool includeEmptySelection, string aFilterString,
			bool aCaseSensitive)
		{
			try
			{
				base.FormLoaded = false;
				object selectedValue = cboPrimSizeCurve.SelectedValue;
				DataTable dtSizeCurve = MIDEnvironment.CreateDataTable();
				SizeCurve objSizeCurve = new SizeCurve();
								
				// RowFilter didn't work with multiple wild cards 
				aFilterString = aFilterString.Replace("*","%");
                //aFilterString = aFilterString.Replace("'","''");	// for string with single quote
			
                //string whereClause = "SIZE_CURVE_GROUP_NAME LIKE " +  "'" + aFilterString + "'";
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
					dtSizeCurve.Rows.Add(new object[] { Include.NoRID, ""} );
				}

				DataView dvSizeCurve = new DataView(dtSizeCurve, "", "SIZE_CURVE_GROUP_NAME", DataViewRowState.CurrentRows);
				cboPrimSizeCurve.DataSource = dvSizeCurve;  
				cboPrimSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";
				cboPrimSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";
				cboPrimSizeCurve.Enabled = true;

                //AdjustTextWidthComboBox_DropDown(cboPrimSizeCurve); TT#1638 - MD - Revised Model Save - RBeck

				if (selectedValue != null)
				{
					cboPrimSizeCurve.SelectedValue = selectedValue;
				}
				base.FormLoaded = true;
			}		
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeCurveComboBox");
			}
		}
		// END MID Track #4396
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
                    if (cbModelName.SelectedIndex >= 0)
                    {
                        DataRowView aRow = (DataRowView)cbModelName.SelectedItem;
                        // BEGIN MID Track #5343 - Null object reference 
                        //this._sizeAlternateProfile.Key = Convert.ToInt32(aRow.Row["SIZE_ALTERNATE_RID"]);	
                        //InitializeForm(_sizeAlternateProfile.Key);
                        int altRID = Convert.ToInt32(aRow.Row["SIZE_ALTERNATE_RID"]);
                        InitializeForm(altRID);
                    }	// END MID Track #5343
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

		private void cboPrimSizeCurve_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			TransactionData td = new TransactionData();
			try
			{
				if (base.FormLoaded)
				{
					if (PromptSizeChange)
					{
						if (!_showWarningPrompt || ShowWarningPrompt() == DialogResult.Yes)
						{														
							this._sizeAlternateProfile.PrimarySizeCurveRid = Convert.ToInt32(cboPrimSizeCurve.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
							_sizeAlternateProfile.ClearAltSizeList();
							this.BindAlternateGrid();
							ChangePending = true;
						}
						else
						{
							//Shut off the prompt so the combo can be reset to original value.
							PromptSizeChange = false;
							cboPrimSizeCurve.SelectedValue = this._sizeAlternateProfile.PrimarySizeCurveRid;
						}
					}
					else
					{
						//Turn the prompt back on.
						PromptSizeChange = true;
					}
				}
			}
			catch(DatabaseForeignKeyViolation)
			{
				td.Rollback();
				MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
				return;
			}
			catch (Exception ex)
			{
				td.Rollback();
				HandleException(ex, "cboPrimSizeCurve_SelectedIndexChanged");
			}
			finally
			{
				td.CloseUpdateConnection();
			}
		}

		private void cboAltSizeCurve_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			TransactionData td = new TransactionData();
			try
			{
				if (base.FormLoaded)
				{
					if (PromptSizeChange)
					{
						if (!_showWarningPrompt ||ShowWarningPrompt() == DialogResult.Yes)
						{															
							this._sizeAlternateProfile.AlternateSizeCurveRid = Convert.ToInt32(cboAltSizeCurve.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
							_sizeAlternateProfile.ClearAltSizeList();
							this.BindAlternateGrid();
							ChangePending = true;
						}
						else
						{
							//Shut off the prompt so the combo can be reset to original value.
							PromptSizeChange = false;
							cboAltSizeCurve.SelectedValue = this._sizeAlternateProfile.AlternateSizeCurveRid;
						}
					}
					else
					{
						//Turn the prompt back on.
						PromptSizeChange = true;
					}
				}
			}
			catch(DatabaseForeignKeyViolation)
			{
				td.Rollback();
				MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
				return;
			}
			catch (Exception ex)
			{
				td.Rollback();
				HandleException(ex, "cboAltSizeCurve_SelectedIndexChanged");
			}
			finally
			{
				td.CloseUpdateConnection();
			}
		}

		private void ugModel_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			try
			{
				//Create any context menus that may be used on the grid.
				//BuildContextMenus();
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                //ugld.ApplyDefaults(e);
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
                //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
                ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
                // End TT#1164
                //End TT#169

				//Set cancel update action
				ugModel.RowUpdateCancelAction = RowUpdateCancelAction.RetainDataAndActivation;

				ugModel.DisplayLayout.ValueLists.Clear();
				ugModel.DisplayLayout.AddNewBox.Hidden = false;
				ugModel.DisplayLayout.Override.SelectTypeCell = SelectType.ExtendedAutoDrag;

				ugModel.DisplayLayout.Bands[0].Columns["Description"].Header.Caption = string.Empty;
				ugModel.DisplayLayout.Bands[0].Columns["Description"].CellActivation = Activation.NoEdit;
				ugModel.DisplayLayout.Bands[0].Columns["Seq"].Hidden = true;

				ugModel.DisplayLayout.Bands[1].Columns["Description"].Header.Caption = string.Empty;
				ugModel.DisplayLayout.Bands[1].Columns["Description"].CellActivation = Activation.NoEdit;
				ugModel.DisplayLayout.Bands[1].Columns["Seq"].Hidden = true;

				ugModel.DisplayLayout.Bands[1].AddButtonCaption = "Alternate";
				ugModel.DisplayLayout.Bands[1].ColHeadersVisible = false;

				InitializeValueLists();
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugModel_InitializeLayout");
			}	
		}


		private void ugModel_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			if (e.Row.Band.Index == 1)
			{
				// I discovered that just updating the GRID with the corect value wasn't enough for the value to
				// make it to the DB in some cases.
				// So per Infragistics documentation, I search for the correct row in the datatable and update it
				// instead.
				UltraGridRow parentGridRow = e.Row.ParentRow;
				//object [] parentKey = new object[1];
				//parentKey[0] = parentGridRow.Cells["Seq"].Value;
				int parentSeq =  Convert.ToInt32(parentGridRow.Cells["Seq"].Value, CultureInfo.CurrentUICulture);
//				DataSet ds = ugModel.DataSource;
//				DataRow parentDBRow = _storeCharDataSet.Tables["STORE_CHAR_GROUP"].Rows.Find(parentKey);
//				if (parentDBRow != null)
//					parentDBRow["SCG_LIST_IND"] = "1";
				// set dummy key in new row
				e.Row.Cells["Seq"].Value = parentSeq;
				e.Row.Cells["Description"].Value = "Alternate";
				e.Row.Cells["Size"].Value = -1;
				e.Row.Cells["Dimension"].Value = "-1";
			}
			else
			{
				e.Row.Cells["Seq"].Value = ++_seq;
				e.Row.Cells["Description"].Value = "Primary";
				e.Row.Cells["Size"].Value = -1;
				e.Row.Cells["Dimension"].Value = "-1";
			}
			ChangePending = true;
		}


		private void InitializeValueLists()
		{
			try
			{
				//============
				// Primary
				//============
				ugModel.DisplayLayout.ValueLists.Add("DimensionSize");
				ugModel.DisplayLayout.ValueLists["DimensionSize"].ValueListItems.Add(Include.NoRID, " ");
				for (int s=0;s<_sizeCodeKeyList.Count;s++)
				{
					int aSizeRid = (int)_sizeCodeKeyList[s];
					string aSize = (string)_sizeCodeIDList[s];
					ugModel.DisplayLayout.ValueLists["DimensionSize"].ValueListItems.Add(aSizeRid, aSize);
				}

				ugModel.DisplayLayout.ValueLists.Add("Dimensions");
				ugModel.DisplayLayout.ValueLists["Dimensions"].ValueListItems.Add(Include.NoRID, " ");
				//myGrid.DisplayLayout.ValueLists["Dimensions"].SortStyle = ValueListSortStyle.Ascending;
				for (int s=0;s<_dimKeyList.Count;s++)
				{
					int aWidthRid = (int)_dimKeyList[s];
					string width = (string)_dimIDList[s];
					ugModel.DisplayLayout.ValueLists["Dimensions"].ValueListItems.Add(aWidthRid, width);
				}

				ugModel.DisplayLayout.ValueLists.Add("Sizes");
				ugModel.DisplayLayout.ValueLists["Sizes"].ValueListItems.Add(Include.NoRID, " ");
				for (int s=0;s<_sizeKeyList.Count;s++)
				{
					int aSizeRid = (int)_sizeKeyList[s];
					string aSize = (string)_sizeIDList[s];
					ugModel.DisplayLayout.ValueLists["Sizes"].ValueListItems.Add(aSizeRid, aSize);
				}

				//============
				// Alternate
				//============
				ugModel.DisplayLayout.ValueLists.Add("AltDimensionSize");
				ugModel.DisplayLayout.ValueLists["AltDimensionSize"].ValueListItems.Add(Include.NoRID, " ");
				for (int s=0;s<this._altSizeCodeKeyList.Count;s++)
				{
					int aSizeRid = (int)_altSizeCodeKeyList[s];
					string aSize = (string)_altSizeCodeIDList[s];
					ugModel.DisplayLayout.ValueLists["AltDimensionSize"].ValueListItems.Add(aSizeRid, aSize);
				}

				ugModel.DisplayLayout.ValueLists.Add("AltDimensions");
				ugModel.DisplayLayout.ValueLists["AltDimensions"].ValueListItems.Add(Include.NoRID, " ");
				//myGrid.DisplayLayout.ValueLists["Dimensions"].SortStyle = ValueListSortStyle.Ascending;
				for (int s=0;s<_altDimKeyList.Count;s++)
				{
					int aWidthRid = (int)_altDimKeyList[s];
					string width = (string)_altDimIDList[s];
					ugModel.DisplayLayout.ValueLists["AltDimensions"].ValueListItems.Add(aWidthRid, width);
				}

				ugModel.DisplayLayout.ValueLists.Add("AltSizes");
				ugModel.DisplayLayout.ValueLists["AltSizes"].ValueListItems.Add(Include.NoRID, " ");
				for (int s=0;s<_altSizeKeyList.Count;s++)
				{
					int aSizeRid = (int)_altSizeKeyList[s];
					string aSize = (string)_altSizeIDList[s];
					ugModel.DisplayLayout.ValueLists["AltSizes"].ValueListItems.Add(aSizeRid, aSize);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "InitializeValueLists");
			}
		}


		
		protected DialogResult ShowWarningPrompt()
		{
			
			
			try
			{
				DialogResult drResult;

				drResult = DialogResult.Yes;
				ChangePending = false;

				drResult = MessageBox.Show("Warning:\nChanging this value will cause the current Alternates information to be immediately erased." +
					"\nTo return to the original Alternates information close the form without saving or updating.\nDo you wish to continue?",
					"Confirmation",
					MessageBoxButtons.YesNo);

				if (drResult == DialogResult.Yes)
				{
					ChangePending = true;
				}

				return drResult;
			}
			catch (Exception ex)
			{
				HandleException(ex, "ShowWarningPrompt");
				return DialogResult.No;
			}	

			
		}


		private bool DeleteSizeAlternateChildren(TransactionData td)
		{
			bool Successful;
			Successful = true;

			try
			{
				Successful = SizeModelData.DeleteSizeAlternateChildren(this._sizeAlternateProfile.Key, td);

			}
			catch(Exception)
			{
				throw;
			}

			return Successful;

		}

		private bool InsertSizeAlternateChildren(TransactionData td)
		{
			bool successful = true;

			try
			{
				successful = DeleteSizeAlternateChildren(td);
				if (successful)
				{
					foreach (SizeAlternatePrimary sap in _sizeAlternateProfile.AlternateSizeList)
					{
						SizeModelData.SizeAltPrimarySize_insert(_sizeAlternateProfile.Key, sap.Sequence, sap.SizeRid, sap.DimensionRid, td);
						int altSeq = 0;
						foreach (SizeAlternate sa in sap.AlternateList)
						{
							SizeModelData.SizeAltAlternateSize_insert(_sizeAlternateProfile.Key, sap.Sequence, ++altSeq, sa.SizeRid, sa.DimensionRid, td);
						}
					}
				}				
			}
			catch
			{
				throw;
			}

			return successful;

		}

		protected override bool SaveChanges()
		{
			try
			{
				bool continueSave = false;
				bool saveAsCanceled = false;

				if (!ErrorFound)
				{
                    //BEGIN TT#4184-MD-VStuart-Able to create size constraint model with blank name
                    //if (_newModel && _saveAsName == string.Empty)
                    if (_newModel)
                    //END TT#4184-MD-VStuart-Able to create size constraint model with blank name
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
								SizeAltModelProfile checkExists = GetAlternateModel(formSaveAs.SaveAsName);
								if (checkExists.Key == Include.NoRID)
								{
									this._sizeAlternateProfile.SizeAlternateName = formSaveAs.SaveAsName;
									continueSave = true;
								}
								else
								{
									if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName),  this.Text,
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

                                        SizeAltModelProfile altModel = GetAlternateModel(formSaveAs.SaveAsName);
										this._sizeAlternateProfile.ModelChangeType = eChangeType.update;
                                        this._sizeAlternateProfile.Key = altModel.Key;
                                        this._sizeAlternateProfile.SizeAlternateName = formSaveAs.SaveAsName;  // RO-4989 - Ability to Save As a duplicate Size Constraint name
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
						SaveGridToProfile();
						TransactionData td = new TransactionData();

						if (PerformingSaveAs)
						{
							CheckForDequeue();
							_sizeAlternateProfile.ModelChangeType = eChangeType.add;
							_sizeAlternateProfile.Key = Include.NoRID;
						}

                        //BEGIN TT#4184-MD-VStuart-Able to create size constraint model with blank name
                        if (!string.IsNullOrEmpty(_sizeAlternateProfile.SizeAlternateName))
                        {
						    td.OpenUpdateConnection();
						    // Save sile alternate model data
						    int SizeAltModelRid = SizeModelData.SizeAlternateModel_Insert(this._sizeAlternateProfile.Key,
							    _sizeAlternateProfile.SizeAlternateName, _sizeAlternateProfile.PrimarySizeCurveRid, 
							    _sizeAlternateProfile.AlternateSizeCurveRid, td);
						    this._sizeAlternateProfile.Key = SizeAltModelRid;
						    // Save Grid data
						    InsertSizeAlternateChildren(td);
						    td.CommitData();
						    td.CloseUpdateConnection();
						    ChangePending = false;
						    // adds row to cbModelName 
						    if (_sizeAlternateProfile.ModelChangeType == eChangeType.add)
						    {
    //Begin     TT#1638 - MD - Revised Model Save - RBeck
							    DataTable dt = (DataTable)cbModelName.DataSource;
                                dt.Rows.Add(new object[] { SizeAltModelRid, _sizeAlternateProfile.SizeAlternateName } );
                                 //dt.Rows.Add(new object[] { _sizeAlternateProfile.Key, 
                                 //                           _sizeAlternateProfile.SizeAlternateName, 							
                                 //                           _sizeAlternateProfile.PrimarySizeCurveRid, 
                                 //                           _sizeAlternateProfile.AlternateSizeCurveRid }
                                 //            );
    //End     TT#1638 - MD - Revised Model Save - RBeck
							    dt.AcceptChanges();
							    cbModelName.SelectedValue = _sizeAlternateProfile.Key;
						    }
                        }
                        else
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NameRequiredToSave), this.Text,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _newModel = true;
                            SaveChanges();
                        }
                        //END TT#4184-MD-VStuart-Able to create size constraint model with blank name
                    }
					else
					{
                        PerformingSaveAs = false;  // RO-4989 - Ability to Save As a duplicate Size Constraint name
						_newModel = false;
						_saveAsName = _sizeAlternateProfile.SizeAlternateName;
						MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SaveCanceled),  this.Text,
							MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //BEGIN TT#4184-MD-VStuart-Able to create size constraint model with blank name
                        if (string.IsNullOrEmpty(_sizeAlternateProfile.SizeAlternateName))
                        {
                            _newModel = true;
                            continueSave = false;
                        }
                        //END TT#4184-MD-VStuart-Able to create size constraint model with blank name
					}
					this.cbModelName.Enabled = true;
					_sizeAlternateProfile.ModelChangeType = eChangeType.update;
					_newModel = false;
					Format_Title(eDataState.Updatable, eMIDTextCode.frm_SizeAlternativeModel, null);
                    if (FunctionSecurity.AllowDelete)
                    {
                        btnDelete.Enabled = true;
                        btnInUse.Enabled = true; //TT#110-MD-VStuart - In Use Tool
                    }
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}

			return true;
		}

		/// <summary>
		/// reads the data from the grid and places it in the current SizeAlternateProfile
		/// it does this by using the DataSet DataSource. 
		/// </summary>
		private void SaveGridToProfile()
		{
			try
			{
				// Clean up old data
				_sizeAlternateProfile.ClearAltSizeList();
				
				//ugModel.UpdateData();
				_dsAlt.AcceptChanges();
				DataTable dtPrimary = _dsAlt.Tables["Primary"];
				DataTable dtAlternate = _dsAlt.Tables["Alternate"];

				foreach (DataRow aRow in dtPrimary.Rows)
				{
					DataRow [] childRows = aRow.GetChildRows("Primary");
					// This check keeps from adding Primary rows that have no children.
					if (childRows.Length > 0)
					{
						SizeAlternatePrimary aPrimary = new SizeAlternatePrimary();
						aPrimary.Sequence = Convert.ToInt32(aRow["Seq"], CultureInfo.CurrentUICulture);
						if (aRow["Size"] == DBNull.Value)
							aPrimary.SizeRid = Include.NoRID;
						else
							aPrimary.SizeRid = Convert.ToInt32(aRow["Size"], CultureInfo.CurrentUICulture);
						if (aRow["Dimension"] == DBNull.Value)
							aPrimary.DimensionRid = Include.NoRID;
						else
							aPrimary.DimensionRid = Convert.ToInt32(aRow["Dimension"], CultureInfo.CurrentUICulture);

						DataRow [] altRows = dtAlternate.Select("Seq = " + aPrimary.Sequence.ToString(CultureInfo.CurrentUICulture));

						int seq = 0;
						foreach (DataRow aAlt in altRows)
						{	
							SizeAlternate aAlternate = new SizeAlternate();
							aAlternate.Sequence = ++seq;
							if (aAlt["Size"] == DBNull.Value)
								aAlternate.SizeRid = Include.NoRID;
							else
								aAlternate.SizeRid = Convert.ToInt32(aAlt["Size"], CultureInfo.CurrentUICulture);
							if (aAlt["Dimension"] == DBNull.Value)
								aAlternate.DimensionRid = Include.NoRID;
							else
								aAlternate.DimensionRid = Convert.ToInt32(aAlt["Dimension"], CultureInfo.CurrentUICulture);					
							aPrimary.AlternateList.Add(aAlternate);
						}
						this._sizeAlternateProfile.AlternateSizeList.Add(aPrimary);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private SizeAltModelProfile GetAlternateModel(string modelName)
		{
			SizeAltModelProfile aModel = null;
			DataTable dt = this.SizeModelData.SizeAlternateModel_Read(modelName);
			if (dt.Rows.Count == 0)
			{
				aModel = new SizeAltModelProfile(Include.NoRID);
			}
			else
			{	
				DataRow aRow = dt.Rows[0];
				int aModelKey = Convert.ToInt32(aRow["SIZE_ALTERNATE_RID"], CultureInfo.CurrentUICulture);
				aModel = new SizeAltModelProfile(aModelKey);
			}
			return aModel;
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
				if (modelName != _sizeAlternateProfile.SizeAlternateName.Trim())
				{
					DataTable dt = this.SizeModelData.SizeAlternateModel_Read(modelName);
					if (dt.Rows.Count > 0)
					{
						DataRow aRow = dt.Rows[0];
						modelRID = Convert.ToInt32(aRow["SIZE_ALTERNATE_RID"], CultureInfo.CurrentUICulture);
						foreach (ComboObject comboObj in cbModelName.Items)
						{
							if (comboObj.Value == _sizeAlternateProfile.SizeAlternateName.Trim() &&
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
							if (comboObj.Value == _sizeAlternateProfile.SizeAlternateName.Trim())
							{
								modelRID = comboObj.Key;
								cbModelName.Items.Remove(comboObj);
								cbModelName.Items.Add(new ComboObject(modelRID, modelName));
 
								break;
							}
						}
						_sizeAlternateProfile.SizeAlternateName = modelName;
					}
					else
					{
						if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName),  this.Text,
							MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.No) 
						{
							cbModelName.Text = _sizeAlternateProfile.SizeAlternateName.Trim();
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

        override protected bool VerifyGrid()
		{
			bool isValid = true;

			//================================================================
			// Make sure that there is at least one primary/Alternate defined
			//================================================================
			bool hasChild = false;
			foreach (UltraGridRow primRow in ugModel.Rows)
			{
				if (primRow.HasChild())
				{
					hasChild = true;
					break;
				}
			}
			if (!hasChild)
			{
				isValid = false;
				MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_OneAlternateRequired), this.Text,
					             MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			//================================================================
			// Make sure each row has atleast one size or dimension selection
			//================================================================
			if (isValid)
			{
				foreach (UltraGridRow primRow in ugModel.Rows)
				{
					primRow.RowSelectorAppearance.Image = null;
					if (!VerifyRow(primRow))
					{
						isValid = false;
					}
					else
					{
						foreach (UltraGridRow altRow in primRow.ChildBands[0].Rows)
						{
							altRow.RowSelectorAppearance.Image = null;
							if (!VerifyRow(altRow))
							{
								isValid = false;
							}
						}
					}
				}
			}


			// Check for size/dimension combinations that are not in the size group
			if (isValid)
			{
				foreach (UltraGridRow primRow in ugModel.Rows)
				{
					int sizeValue = Convert.ToInt32(primRow.Cells["Size"].Value, CultureInfo.CurrentUICulture);
					int dimValue = Convert.ToInt32(primRow.Cells["Dimension"].Value, CultureInfo.CurrentUICulture);

					if (sizeValue == Include.NoRID || dimValue == Include.NoRID)
					{
					}
					else
					{
						ArrayList sizeCodeList = _primSizeCurveGroupProfile.GetSizeCodeList(sizeValue, dimValue);

						if (sizeCodeList.Count == 0)
						{
							isValid = false;
							string msg = "Size/Dimension combination is not in Size Group";
							ErrorMessages.Add(msg);
							AttachErrors(primRow.Cells["Size"]);
							ErrorMessages.Add(msg);
							AttachErrors(primRow.Cells["Dimension"]);
						}
						
						foreach (UltraGridRow altRow in primRow.ChildBands[0].Rows)
						{
							sizeValue = Convert.ToInt32(altRow.Cells["Size"].Value, CultureInfo.CurrentUICulture);
							dimValue = Convert.ToInt32(altRow.Cells["Dimension"].Value, CultureInfo.CurrentUICulture);

							sizeCodeList = _altSizeCurveGroupProfile.GetSizeCodeList(sizeValue, dimValue);
							if (sizeCodeList.Count == 0)
							{
								isValid = false;
								string msg = "Size/Dimension combination is not in Size Group";
								ErrorMessages.Add(msg);
								AttachErrors(primRow.Cells["Size"]);
								ErrorMessages.Add(msg);
								AttachErrors(primRow.Cells["Dimension"]);
							}
						}
						
					}
				}
			}

			//=========================================================================================
			// Check that primary / Alternate selections are valid
			// if primary only contains a size, but not dimension, then Alternates must do the same.
			// if primary only contains a dimension, but no size, then alternates must do the same.
			// if primary contains both size and dimension, alternates must contain both too.
			//=========================================================================================
			if (isValid)
			{
				foreach (UltraGridRow primRow in ugModel.Rows)
				{
					int sizeValue = Convert.ToInt32(primRow.Cells["Size"].Value, CultureInfo.CurrentUICulture);
					int dimValue = Convert.ToInt32(primRow.Cells["Dimension"].Value, CultureInfo.CurrentUICulture);

					if (sizeValue == Include.NoRID)
					{
						foreach (UltraGridRow altRow in primRow.ChildBands[0].Rows)
						{
							sizeValue = Convert.ToInt32(altRow.Cells["Size"].Value, CultureInfo.CurrentUICulture);
							dimValue = Convert.ToInt32(altRow.Cells["Dimension"].Value, CultureInfo.CurrentUICulture);

							if (sizeValue != Include.NoRID)
							{
								isValid = false;
								string msg = "For this Primary selection, Alternates must contain only Dimension selections";
								ErrorMessages.Add(msg);
								AttachErrors(altRow.Cells["Size"]);
								//ErrorMessages.Add(msg);
								//AttachErrors(primRow.Cells["Dimension"]);
							}
						}
					}
					else if (dimValue == Include.NoRID)
					{
						foreach (UltraGridRow altRow in primRow.ChildBands[0].Rows)
						{
							sizeValue = Convert.ToInt32(altRow.Cells["Size"].Value, CultureInfo.CurrentUICulture);
							dimValue = Convert.ToInt32(altRow.Cells["Dimension"].Value, CultureInfo.CurrentUICulture);

							if (dimValue != Include.NoRID)
							{
								isValid = false;
								string msg = "For this Primary selection, Alternates must contain only Size selections";
								//ErrorMessages.Add(msg);
								//AttachErrors(primRow.Cells["Size"]);
								ErrorMessages.Add(msg);
								AttachErrors(altRow.Cells["Dimension"]);
							}
						}
					}
					else
					{
						foreach (UltraGridRow altRow in primRow.ChildBands[0].Rows)
						{
							sizeValue = Convert.ToInt32(altRow.Cells["Size"].Value, CultureInfo.CurrentUICulture);
							dimValue = Convert.ToInt32(altRow.Cells["Dimension"].Value, CultureInfo.CurrentUICulture);

							if (dimValue == Include.NoRID || sizeValue == Include.NoRID)
							{
								isValid = false;
								string msg = "For this Primary selection, Alternates must contain both a Size and Dimension selection";
								if (sizeValue == Include.NoRID)
								{
									ErrorMessages.Add(msg);
									AttachErrors(altRow.Cells["Size"]);
								}
								if (dimValue == Include.NoRID)
								{
									ErrorMessages.Add(msg);
									AttachErrors(altRow.Cells["Dimension"]);
								}
							}
						}
					}
				}
			}

			//=====================================================================
			// Check for duplicate selections: either dup primaries or dup Alternates
			//=====================================================================
			if (isValid)
			{
				ArrayList primSizeList = new ArrayList();
				ArrayList primDimList = new ArrayList();
				ArrayList primRowList = new ArrayList();
				ArrayList altSizeList = new ArrayList();
				ArrayList altDimList = new ArrayList();
				ArrayList altRowList = new ArrayList();

				foreach (UltraGridRow primRow in ugModel.Rows)
				{
					int sizeValue = Convert.ToInt32(primRow.Cells["Size"].Value, CultureInfo.CurrentUICulture);
					int dimValue = Convert.ToInt32(primRow.Cells["Dimension"].Value, CultureInfo.CurrentUICulture);

					if (!CheckForDupRows(primRow, sizeValue, dimValue, primSizeList, primDimList, primRowList))
						isValid = false;

//					foreach (UltraGridRow altRow in primRow.ChildBands[0].Rows)
//					{
//						sizeValue = Convert.ToInt32(altRow.Cells["Size"].Value, CultureInfo.CurrentUICulture);
//						dimValue = Convert.ToInt32(altRow.Cells["Dimension"].Value, CultureInfo.CurrentUICulture);
//
//						if (!CheckForDupRows(altRow, sizeValue, dimValue, altSizeList, altDimList, altRowList))
//						{
//							isValid = false;
//							ugModel.Rows.ExpandAll(true);
//							cbExpandAll.Checked = true;
//						}
//					}
					
				}
			}

			//==================================================================
			// Warn of Primary size that have no Alternate sizes assigned
			// (Parent rows without child rows)
			//==================================================================
			if (isValid)
			{
				foreach (UltraGridRow primRow in ugModel.Rows)
				{
					if (!primRow.HasChild())
					{
						isValid = false;
					
					}
				}

				if (!isValid)
				{
					string msg = "Primary Sizes were found with no Alternate Sizes assigned. \n" +
						"Press OK to ignore these and continue. \n" +
						"Press Cancel to cancel save and correct information.";
					DialogResult aResult = MessageBox.Show(msg, "Primary Rows found with no Alternates assigned", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
					if (aResult == DialogResult.OK)
						isValid = true;
					else
						isValid = false;
				}
			}
			else
			{
				MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorsFoundReviewCorrect));
			}

			return isValid;
		}

		private bool CheckForDupRows(UltraGridRow aRow, int sizeValue, int dimValue, ArrayList sizeList, ArrayList dimList, ArrayList rowList)
		{
			bool isValid = true;
			bool found = false;
			int cnt = sizeList.Count, i=0;
			for (i=0;i<cnt;i++)
			{
				if (sizeValue == (int)sizeList[i] 
					&& dimValue == (int)dimList[i])
				{
					found = true;
					UltraGridRow ugRow = (UltraGridRow)rowList[i];
					ugRow.RowSelectorAppearance.Image = ErrorImage;
					aRow.RowSelectorAppearance.Image = ErrorImage;
					if (aRow.Band.Index == 0)
					{
						ugRow.Tag = "Duplicate primary size values defined";
						aRow.Tag = "Duplicate primary size values defined";
					}
					else
					{
						ugRow.Tag = "Duplicate alternate size values defined";
						aRow.Tag = "Duplicate alternate size values defined";
					}

					isValid = false;
				}
			}
			if (!found)
			{
				sizeList.Add(sizeValue);
				dimList.Add(dimValue);
				rowList.Add(aRow);
			}

			return isValid;
		}



		private bool VerifyRow(UltraGridRow aRow)
		{
			bool isValid = true;

			ErrorMessages.Clear();
			UltraGridCell sizeCell = aRow.Cells["Size"];
			UltraGridCell dimCell = aRow.Cells["Dimension"];

			sizeCell.Appearance.Image = null;
			sizeCell.Tag = null;

			dimCell.Appearance.Image = null;
			dimCell.Tag = null;

			
			if (sizeCell.Text.Trim() == string.Empty && dimCell.Text.Trim() == string.Empty)
			{
				isValid = false;

				string msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
				msg = msg.Replace("{0}","Dimension or Size");
				ErrorMessages.Add(msg);
				AttachErrors(sizeCell);
				ErrorMessages.Add(msg);
				AttachErrors(dimCell);
			}			

			return isValid;
		}

		private void AttachErrors(UltraGridCell activeCell)
		{
			try
			{
				activeCell.Appearance.Image = ErrorImage;

				for (int errIdx=0; errIdx <= ErrorMessages.Count - 1; errIdx++)
				{
					activeCell.Tag = (errIdx == 0) ? ErrorMessages[errIdx] : activeCell.Tag + "\n" + ErrorMessages[errIdx];
				}

				ErrorMessages.Clear();
			}
			catch (Exception ex)
			{
				HandleException(ex, "AttachErrors(UltraGridCell activeCell)");
			}	
		}

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
        //        if (NameValid() && VerifyGrid())		// MID Track #4970 - add NameValid
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
        //        if (NameValid() && VerifyGrid())		// MID Track #4970 - add NameValid
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
				if (this._sizeAlternateProfile.Key != Include.NoRID)
				{
					string text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDelete);
					text =  text.Replace("{0}", "Size Alternate Model: " + _sizeAlternateProfile.SizeAlternateName);
				    if (MessageBox.Show(text, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				        == DialogResult.Yes)
				    //BEGIN TT#601-MD-VStuart-FWOS Model attempts delete while InUse
				    {
				        //If the RID is InUse don't delete. If RID is NOT InUse go ahead and delete.
                        var emp = new SizeAltModelProfile(_modelRID);
				        eProfileType type = emp.ProfileType;
				        int rid = _modelRID;

				        if (!CheckInUse(type, rid, false))
				        {
				            _sizeAlternateProfile.ModelChangeType = eChangeType.delete;
				            td.OpenUpdateConnection();
				            // This uses the Key(SizeAltRid) as defined in the SizeAlternateProfile clase
				            DeleteSizeAlternateChildren(td);
				            this.SizeModelData.SizeAlternateModel_Delete(this._sizeAlternateProfile.Key, td);
				            td.CommitData();
				            DataTable dt = (DataTable) cbModelName.DataSource;
				            DataRow[] rows =
				                dt.Select("SIZE_ALTERNATE_RID = " + _sizeAlternateProfile.Key.ToString(CultureInfo.CurrentUICulture));
				            dt.Rows.Remove(rows[0]);
				            dt.AcceptChanges();

				            //_changeMade = true;
				            itemDeleted = true;
				            //FormLoaded = false;				
				        }
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
						int SizeAltModelRid = Convert.ToInt32(aRow.Row["SIZE_ALTERNATE_RID"]);	
						InitializeForm(SizeAltModelRid);
					}
					else
					{
						InitializeForm();
					}
				}
			}
			catch(DatabaseForeignKeyViolation)
			{
				td.Rollback();
                //BEGIN TT#110-MD-VStuart - In Use Tool
                //MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
                ShowInUse();
                //END TT#110-MD-VStuart - In Use Tool
			}
			catch(Exception exception)
			{
				td.Rollback();
				HandleException(exception);
			}
			finally
			{
				td.CloseUpdateConnection();
			}
		}

//Begin TT#1638 - MD - Revised Model Save - RBeck 
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

		private void ugModel_AfterExitEditMode(object sender, System.EventArgs e)
		{
			this._showWarningPrompt = true;
			ChangePending = true;
		}

		private void ugModel_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			try
			{
				UltraGrid myGrid = (UltraGrid)sender;
				UltraGridRow activeRow = myGrid.ActiveRow;

				if (activeRow != null)
				{
					if (activeRow.IsAddRow)
					{
						activeRow.Update();
						ugModel.ActiveRow = activeRow;
					}

					//Fixes an issue with the SortIndicator being set.
					//***********************************************************
					if (activeRow.HasChild() && activeRow.Expanded == false)
					{
						activeRow.Expanded = true;
					}
					//***********************************************************

					if (!VerifyRow(activeRow))
					{
						e.Cancel = true;
					}
				}

			}
			catch( Exception ex )
			{
				e.Cancel = true;
				HandleException(ex, "ugModel_BeforeRowInsert");
			}
		}

		private void ugModel_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				UltraGridCell gridCell = (UltraGridCell)e.Element.GetContext(typeof(UltraGridCell));
				if (gridCell != null) 
				{
					switch (gridCell.Column.Style)
					{
						case Infragistics.Win.UltraWinGrid.ColumnStyle.TriStateCheckBox:
						case Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox:
							ugModel.DisplayLayout.Override.CellClickAction = CellClickAction.Default;
							break;
						default:
						switch (gridCell.Column.DataType.Name.ToUpper())
						{
							case "BOOLEAN":
								ugModel.DisplayLayout.Override.CellClickAction = CellClickAction.Default;
								break;
							default:
								ugModel.DisplayLayout.Override.CellClickAction = CellClickAction.CellSelect;
								break;
						}
							break;
					}
				}

				ShowUltraGridToolTip(ugModel, e);

				UltraGridRow aRow = (UltraGridRow)e.Element.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridRow));
				if (aRow != null)
				{
					if (aRow.Tag != null) 
					{
						if (aRow.Tag.GetType() == typeof(System.String))
						{
							ToolTip.Active = true; 
							ToolTip.SetToolTip(ugModel, (string)aRow.Tag);
						}
					}
				}
			}
			catch( Exception ex )
			{
				HandleException(ex, "ugModel_MouseEnterElement");
			}
		}

		private void ugModel_AfterRowsDeleted(object sender, System.EventArgs e)
		{
			ChangePending = true;
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
		protected void DisplayPictureBoxImages()
		{
			DisplayPictureBoxImage(picBoxAltCurve);
			DisplayPictureBoxImage(picBoxName);
			DisplayPictureBoxImage(picBoxPrimaryCurve);
		}
		
		protected void SetPictureBoxTags()
		{
			picBoxName.Tag= SAB.ApplicationServerSession.GlobalOptions.SizeAlternateCharMask;
			picBoxPrimaryCurve.Tag = SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask;
			picBoxAltCurve.Tag = SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask;
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

        protected override void BindComboBoxes(string picBoxName,
                                        string aFilterString,
                                        bool aCaseSensitive)
        {
            switch (picBoxName)
            {
                case "picBoxName":
                    BindModelNameComboBox(aFilterString, aCaseSensitive);
                    break;

                case "picBoxPrimaryCurve":
                    BindPrimaryCurveComboBox(true, aFilterString, aCaseSensitive);
                    break;

                case "picBoxAltCurve":
                    BindAltCurveComboBox(true, aFilterString, aCaseSensitive);
                    break;
            }
        } 
      
        //private void picBoxFilter_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {	
        //        string enteredMask = string.Empty;
        //        bool caseSensitive = false;
        //        PictureBox picBox = (PictureBox)sender;

        //        if (CharMaskFromDialogOK(picBox, ref enteredMask, ref caseSensitive))
        //        {
        //            base.FormLoaded = false;
        //            //MessageBox.Show("Filter selection process not yet available");
        //            switch (picBox.Name)
        //            {
        //                case "picBoxName":
        //                    BindModelNameComboBox(enteredMask, caseSensitive);
        //                    break;
						
        //                case "picBoxPrimaryCurve":
        //                    BindPrimaryCurveComboBox(true, enteredMask, caseSensitive);
        //                    break;

        //                case "picBoxAltCurve":
        //                    BindAltCurveComboBox(true, enteredMask, caseSensitive);
        //                    break;
        //            }
        //            base.FormLoaded = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

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
        //        string[] globalParts = aGlobalMask.Split(new char[] {'*'});
        //        string[] enteredParts = aEnteredMask.Split(new char[] {'*'});
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

        //protected void SetMaskedComboBoxesEnabled()
        //{
        //    if (SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask != string.Empty)
        //    {
        //        this.cboAltSizeCurve.Enabled = false;
        //        this.cboPrimSizeCurve.Enabled = false;
        //    }

        //    if (SAB.ApplicationServerSession.GlobalOptions.SizeAlternateCharMask != string.Empty)
        //    {
        //        this.cbModelName.Enabled = false;
        //    }

        //    picBoxName.Enabled = true;		// MID Track #5256 - If security View only, can't select Size Alternates when mask 
        //                                        // exists becuse of above check, but picBox is disabled from SetReadOnly; 
        //                                        // this overrides SetReadOnly

        //}
        // //END MID Track #4396
		#endregion
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
