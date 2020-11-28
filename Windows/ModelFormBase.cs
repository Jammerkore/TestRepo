using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Text.RegularExpressions;   //TT#110-MD-VStuart - In Use Tool

using Infragistics.Win;
using Infragistics.Win.UltraWinToolbars;

using MIDRetail.Business;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for ModelFormBase.
	/// </summary>
	public class ModelFormBase : MIDFormBase
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		protected System.Windows.Forms.Button btnSave;
		protected System.Windows.Forms.Button btnDelete;
		protected System.Windows.Forms.Button btnNew;
		protected System.Windows.Forms.Button btnSaveAs;
		protected System.Windows.Forms.Button btnCancel;
        protected ProfileList _ModelProfileList;
		#region Fields
		//=======
		// FIELDS
		//=======
		private bool _inDesigner = false;
        private bool _performingSaveAs = false;
        protected PictureBox picBoxName;

//Begin   TT#1638 - MD - Revised Model Save - RBeck
        //protected ComboBox cbModelName;
        protected MIDComboBoxEnh cbModelName;
//End     TT#1638 - MD - Revised Model Save - RBeck

        protected Label lblModelName;
        protected Infragistics.Win.UltraWinGrid.UltraGrid ugModel;
       // private Label lblBaseModelName;
		private bool _performingNewModel = false;

//Begin     TT#1638 - Revised Model Save - RBeck
        protected bool      _newModel = false;
        protected string    _saveAsName = string.Empty;
        protected bool      _changeMade = false;
        protected int       _modelRID = Include.NoRID;
        protected string    _currModel = null;
        protected bool      _modelLocked = false;
        protected int       _modelIndex = 0;
        protected bool      defaultErrorFound = false;
//End       TT#1638 - Revised Model Save - RBeck
        //BEGIN TT#110-MD-VStuart - In Use Tool
        protected Button btnInUse;
	    private ArrayList _ridList;
	    protected bool _isInQuiry = true;
        //END TT#110-MD-VStuart - In Use Tool

		#endregion Fields


		#region Properties
		//============
		// PROPERTIES
		//============

		/// <summary>
		/// Gets or sets a flag identifying if a Save As action is being performed.
		/// </summary>
		public bool PerformingSaveAs
		{
			get {return _performingSaveAs;}
			set {_performingSaveAs = value;}
		}

		/// <summary>
		/// Gets or sets a flag identifying if a New Model action is being performed.
		/// </summary>
		public bool PerformingNewModel
		{
			get {return _performingNewModel;}
			set {_performingNewModel = value;}
		}
		
		#endregion Properties

		public ModelFormBase()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_inDesigner = true;
		}

		public ModelFormBase(SessionAddressBlock aSAB) : base (aSAB)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			if (aSAB == null)
			{
				throw new Exception("SessionAddressBlock is required");
			}
            ActiveControl = btnNew;
			_inDesigner = false;
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
				this.btnDelete.Click -= new System.EventHandler(this.btnDelete_Click);
				this.btnNew.Click -= new System.EventHandler(this.btnNew_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				this.btnSaveAs.Click -= new System.EventHandler(this.btnSaveAs_Click);

                this.btnSave.EnabledChanged -= new System.EventHandler(this.btnSave_EnabledChanged);
                this.btnSaveAs.EnabledChanged -= new System.EventHandler(this.btnSaveAs_EnabledChanged);
                this.btnDelete.EnabledChanged -= new System.EventHandler(this.btnDelete_EnabledChanged);
                this.btnNew.EnabledChanged -= new System.EventHandler(this.btnNew_EnabledChanged);
                //this.cbModelName.SelectedIndexChanged -= new System.EventHandler(this.cbModelName_SelectedIndexChanged);
                //this.ugModel.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_CellChange);
                //this.ugModel.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugModel_MouseEnterElement);
                //this.ugModel.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugModel_AfterRowInsert);
                //this.ugModel.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_ClickCellButton);
                //this.ugModel.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugModel_InitializeLayout);
                this.ugModel.AfterRowsDeleted -= new EventHandler(ugModel_AfterRowsDeleted);  // TT#4063 - JSmith - Sales Modifier> Trying to Delete a Row in an existing Model and it will not allow a Save after the Delete.

            }
			base.Dispose( disposing );
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.picBoxName = new System.Windows.Forms.PictureBox();
            this.cbModelName = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblModelName = new System.Windows.Forms.Label();
            this.ugModel = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnInUse = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(380, 16);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 21);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.EnabledChanged += new System.EventHandler(this.btnSave_EnabledChanged);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(620, 16);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 21);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "Delete";
            this.btnDelete.EnabledChanged += new System.EventHandler(this.btnDelete_EnabledChanged);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Location = new System.Drawing.Point(540, 16);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 21);
            this.btnNew.TabIndex = 14;
            this.btnNew.Text = "New";
            this.btnNew.EnabledChanged += new System.EventHandler(this.btnNew_EnabledChanged);
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.Location = new System.Drawing.Point(460, 16);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(75, 21);
            this.btnSaveAs.TabIndex = 17;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.EnabledChanged += new System.EventHandler(this.btnSaveAs_EnabledChanged);
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(620, 481);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 20);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // picBoxName
            // 
            this.picBoxName.BackColor = System.Drawing.SystemColors.Highlight;
            this.picBoxName.Location = new System.Drawing.Point(103, 16);
            this.picBoxName.Name = "picBoxName";
            this.picBoxName.Size = new System.Drawing.Size(19, 20);
            this.picBoxName.TabIndex = 56;
            this.picBoxName.TabStop = false;
            this.picBoxName.Click += new System.EventHandler(this.picBoxFilter_Click);
            this.picBoxName.MouseHover += new System.EventHandler(this.picBoxFilter_MouseHover);
            // 
            // cbModelName
            // 
            this.cbModelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbModelName.AutoAdjust = true;
            this.cbModelName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbModelName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbModelName.DataSource = null;
            this.cbModelName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModelName.DropDownWidth = 112;
            this.cbModelName.FormattingEnabled = false;
            this.cbModelName.IgnoreFocusLost = false;
            this.cbModelName.ItemHeight = 13;
            this.cbModelName.Location = new System.Drawing.Point(127, 16);
            this.cbModelName.Margin = new System.Windows.Forms.Padding(0);
            this.cbModelName.MaxDropDownItems = 8;
            this.cbModelName.Name = "cbModelName";
            this.cbModelName.Size = new System.Drawing.Size(248, 21);
            this.cbModelName.TabIndex = 55;
            this.cbModelName.Tag = null;
            this.cbModelName.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cbModelName_MIDComboBoxPropertiesChangedEvent);
            this.cbModelName.SelectionChangeCommitted += new System.EventHandler(this.cbModelName_SelectionChangeCommitted);
            this.cbModelName.MouseHover += new System.EventHandler(this.cbModelName_MouseHover);
            // 
            // lblModelName
            // 
            this.lblModelName.Location = new System.Drawing.Point(22, 18);
            this.lblModelName.Name = "lblModelName";
            this.lblModelName.Size = new System.Drawing.Size(76, 16);
            this.lblModelName.TabIndex = 57;
            this.lblModelName.Text = "Model Name";
            this.lblModelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ugModel
            // 
            this.ugModel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.ugModel.Location = new System.Drawing.Point(24, 72);
            this.ugModel.Name = "ugModel";
            this.ugModel.Size = new System.Drawing.Size(610, 391);
            this.ugModel.TabIndex = 58;
            this.ugModel.AfterRowsDeleted += new EventHandler(ugModel_AfterRowsDeleted);
            // 
            // btnInUse
            // 
            this.btnInUse.Location = new System.Drawing.Point(25, 477);
            this.btnInUse.Name = "btnInUse";
            this.btnInUse.Size = new System.Drawing.Size(75, 23);
            this.btnInUse.TabIndex = 59;
            this.btnInUse.Text = "In Use";
            this.btnInUse.UseVisualStyleBackColor = true;
            this.btnInUse.Click += new System.EventHandler(this.btnInUse_Click);
            // 
            // ModelFormBase
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(720, 517);
            this.Controls.Add(this.btnInUse);
            this.Controls.Add(this.ugModel);
            this.Controls.Add(this.lblModelName);
            this.Controls.Add(this.picBoxName);
            this.Controls.Add(this.cbModelName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveAs);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnNew);
            this.Name = "ModelFormBase";
            this.Text = "ModelFormBase";
            this.Load += new System.EventHandler(this.ModelFormBase_Load);
            this.Controls.SetChildIndex(this.btnNew, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnSaveAs, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.cbModelName, 0);
            this.Controls.SetChildIndex(this.picBoxName, 0);
            this.Controls.SetChildIndex(this.lblModelName, 0);
            this.Controls.SetChildIndex(this.ugModel, 0);
            this.Controls.SetChildIndex(this.btnInUse, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void ModelFormBase_Load(object sender, System.EventArgs e)
		{
			if (!_inDesigner)
			{
				BuildMenu();
				SetText();
				btnSaveAs.Enabled = false;
                DisplayPictureBoxImage(picBoxName);
			}
		}

        private void SetText()   
		{
            this.lblModelName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ModelName) + ":";
            this.btnNew.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_New);
			this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
			this.btnSaveAs.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_SaveAs);
			this.btnDelete.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Delete);
            this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
            this.btnInUse.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_In_Use);  //TT#110-MD-VStuart - In Use Tool
        }

		private void BuildMenu()
		{
			try
			{
                AddMenuItem(eMIDMenuItem.FileNew);
                btnSave.Enabled = false;
                btnSaveAs.Enabled = false;
                btnDelete.Enabled = false;
                btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		virtual public void InitializeForm()
		{
            throw new Exception("InitializeForm not overridden");
		}
//Begin     TT#1638 - MD - Revised Model Save - RBeck       
        virtual public void InitializeForm(int modelRid)
        {
            throw new Exception("modelRid InitializeForm not overridden");
            // return true;
        }
 
        virtual public bool InitializeForm(string currModel)
        {
            throw new Exception("currModel InitializeForm not overridden");
           // return true;
        }
//End       TT#1638 - MD - Revised Model Save - RBeck
        protected void btnSave_Click(object sender, System.EventArgs e)
		{
			ISave();
		}

		private void btnSaveAs_Click(object sender, System.EventArgs e)
		{
			ISaveAs();
		}

		private void btnNew_Click(object sender, System.EventArgs e)
		{
			INew();
		}

        protected void btnDelete_Click(object sender, System.EventArgs e)
        {
            inQuiry2 = false;
			IDelete();
		}

        protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				// BEGIN TT#696 - Overrid Low Level Model going blank on Close.
				DialogResult = DialogResult.Cancel;
				// END TT#696 - Overrid Low Level Model going blank on Close.
				Cancel_Click();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

        private void btnSave_EnabledChanged(object sender, EventArgs e)
        {
            if (btnSave.Enabled)
            {
                EnableMenuItem(this, eMIDMenuItem.FileSave);
                EnableMenuItem(this, eMIDMenuItem.FileSaveAs);
            }
            else
            {
                DisableMenuItem(this, eMIDMenuItem.FileSave);
            }
        }

        private void btnSaveAs_EnabledChanged(object sender, EventArgs e)
        {
            if (btnSaveAs.Enabled)
            {
                EnableMenuItem(this, eMIDMenuItem.FileSaveAs);
            }
            else
            {
                DisableMenuItem(this, eMIDMenuItem.FileSaveAs);
            }
        }

        private void btnNew_EnabledChanged(object sender, EventArgs e)
        {
            if (btnNew.Enabled)
            {
                EnableMenuItem(this, eMIDMenuItem.FileNew);
            }
            else
            {
                DisableMenuItem(this, eMIDMenuItem.FileNew);
            }
        }

        private void btnDelete_EnabledChanged(object sender, EventArgs e)
        {
            if (btnDelete.Enabled)
            {
                EnableMenuItem(this, eMIDMenuItem.EditDelete);
            }
            else
            {
                DisableMenuItem(this, eMIDMenuItem.EditDelete);
            }
        }

        //BEGIN TT#601-MD-VStuart-FWOS Model attempts delete while InUse
        /// <summary>
        /// Checks for InUse during a delete.
        /// </summary>
        /// <param name="type">The model eProfileType in question.</param>
        /// <param name="rid">The model in question.</param>
        /// <param name="inQuiry2">False if this is a mandatory request.</param>
        protected bool CheckInUse(eProfileType type, int rid, bool inQuiry2)
        {
            bool isInUse = true;
            if (rid != Include.NoRID)
            {
                _ridList = new ArrayList();
                _ridList.Add(rid);
                //string inUseTitle = Regex.Replace(type.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
                string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(type); //TT#4304 -jsobek -Store Characteristic In Use not being reported on Store Filters
                bool inQuiry = inQuiry2;
                DisplayInUseForm(_ridList, type, inUseTitle, inQuiry, out isInUse);
            }
            return isInUse;
        }
        //END  TT#601-MD-VStuart-FWOS Model attempts delete while InUse

        //BEGIN TT#110-MD-VStuart - In Use Tool
        /// <summary>
        /// Shows the In Use dialog.
        /// </summary>
        /// <param name="type">The model eProfileType in question.</param>
        /// <param name="rid">The model in question.</param> 
        /// <param name="inQuiry2">False if this is a mandatory request.</param> 
        internal void ShowInUse(eProfileType type, int rid, bool inQuiry2)
        {
            if (rid != Include.NoRID)
            {
                _ridList = new ArrayList();
                _ridList.Add(rid);
                //string inUseTitle = Regex.Replace(type.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
                string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(type); //TT#4304 -jsobek -Store Characteristic In Use not being reported on Store Filters
                bool display = true;
                bool inQuiry = inQuiry2;
                DisplayInUseForm(_ridList, type, inUseTitle, ref display, inQuiry);
            }
        }
        ////END TT#110-MD-VStuart - In Use Tool

        //Begin  TT#1638 - MD - Revised Model Save - RBeck
        protected virtual void LoadModelComboBox()
        { 
            _ModelProfileList = GetModels();
            //_ModelProfileList = SAB.HierarchyServerSession.GetEligModels();
            cbModelName.Items.Clear();

            foreach (ModelName modelName in _ModelProfileList.ArrayList)
            {
                cbModelName.Items.Add(
                    new ModelNameCombo(modelName.Key, modelName.ModelID));
            }

            if (_ModelProfileList.ArrayList.Count == 0)
            {
                cbModelName.Enabled = false;
            }
            // AdjustTextWidthComboBox_DropDown(cbModelName);        
        }

        protected virtual void LoadModelComboBox(ProfileList aModelProfileList)
        {
            _ModelProfileList = aModelProfileList;
            cbModelName.Items.Clear();

            foreach (ModelName modelName in _ModelProfileList.ArrayList)
            {
                cbModelName.Items.Add(
                    new ModelNameCombo(modelName.Key, modelName.ModelID));
            }
            //AdjustTextWidthComboBox_DropDown(cbModelName);   //TT#1638 - MD - Revised Model Save - RBeck    
        }
//End    TT#1638 - MD - Revised Model Save - RBeck 
        protected virtual ProfileList GetModels()
        {
            throw new Exception("GetModels of cbModelName not overridden");
        }

        protected int LocateModelIndex(string aModelName)
        {
            int modelIndex = -1;
            int i = 0;

            foreach (ModelName modelName in _ModelProfileList.ArrayList)
            {
                if (aModelName == modelName.ModelID)
                {
                    modelIndex = i;
                    break;
                }
                ++i;
            }
            return modelIndex;
        }
  
		#region IFormBase Members

//Begin TT#1638 - Revised Model Save - RBeck 

        protected virtual void InitializeBaseForm()
        {            
            _newModel = true;
            _modelRID = -1;
            _currModel = null;
            _modelLocked = false;
            _saveAsName = string.Empty;

            btnSave.Enabled = true;
            btnSaveAs.Enabled = false;
            btnDelete.Enabled = false;
            btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
            this.cbModelName.Text = null;
            this.cbModelName.SelectedIndex = -1;
        }

		override public void INew()
		{
			try
			{
				_performingNewModel = true;
				
                CheckForPendingChanges();
 
                InitializeForm();      
 
				_performingNewModel = false;
 
			}
			catch(Exception exception)
			{
				HandleException(exception);
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

        //}

        //override public void ISaveAs()
        //{

        //}
     
        //override public void IDelete()
        //{
			
        //}

        //override public void IRefresh()
        //{
			
        //}
//End     TT#1638 - Revised Model Save - RBeck
		
		#endregion

//Begin     TT#1638 - MD - Revised Model Save - RBeck
        protected virtual void DetermineSecurity()
        {
            try
            {
                if (FunctionSecurity.AllowDelete &&
                    cbModelName.SelectedIndex > -1)
                {
                    btnDelete.Enabled = true;
                }
                else
                {
                    btnDelete.Enabled = false;
                }

                if (FunctionSecurity.AllowUpdate)
                {
                    btnNew.Enabled = true;
                    btnSave.Enabled = true;
                    //btnSaveAs.Enabled = true; //TT#1638 - MD - Revised Model Save - RBeck
                    btnSaveAs.Enabled = !_newModel;
                }
                else
                {
                    btnNew.Enabled = false;
                    btnSave.Enabled = false;
                    btnSaveAs.Enabled = false;
                    btnInUse.Enabled = true; //TT#3185-M-VStuart-In Use does not work in all explorers-ANFUser
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        protected virtual void ModelType(int saveKey)
        {
            throw new Exception("ModelType not overridden");
        }
        
        protected virtual void ModelText(string _saveAsName)
        {
            throw new Exception("ModelText not overridden");
        }
        
        protected virtual void ModelPreSave()
        {
            throw new Exception("ModelPreSave not overridden");
        }

        protected virtual void DeleteModel()
        {
            throw new Exception("DeleteModel not overridden");
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
	    public virtual void ShowInUse()
        {
            throw new Exception("ShowInUse not overridden");
        }
        //END TT#110-MD-VStuart - In Use Tool

        protected virtual void GetModelRID(string _saveAsName, ref int _modelRID)
		{
            throw new Exception("GetModelRID not overridden");	
		}

        protected virtual int GetProfileKey(string SaveAsName)
        {
            throw new Exception("GetProfileKey not overridden");
        }

        protected virtual void AlterModelChangeType(eChangeType changeType)
        {
            throw new Exception("AlterModelChangeType not overridden");
        }
        
        protected virtual void DefaultErrorFound(bool _defaultErrorFound)
        {
            throw new Exception("DefaultErrorFound not overridden");
        }
 
        protected virtual void BindComboBoxes(  string picBoxName,
                                                string aFilterString,
                                                bool aCaseSensitive )
        {
            try
            {
                bool includeEmptySelection;

                ModelName mn;
                base.FormLoaded = false;
                 
                aFilterString = aFilterString.Replace("*", "%");
                aFilterString = aFilterString.Replace("'", "''");	// for string with single quote

                //string whereClause = "{MODEL_NAME} LIKE " + "'" + aFilterString + "'";
                //if (!aCaseSensitive)
                //{
                //    whereClause += Include.CaseInsensitiveCollation;
                //}

                //whereClause += " ORDER BY MODEL_NAME";

                DataTable dtModels = GetFilteredModels(aFilterString, aCaseSensitive);

                ModelNameList modelList = new ModelNameList(eProfileType.ModelName);
                
                includeEmptySelection = (dtModels.Rows.Count == 0);

                foreach (DataRow row in dtModels.Rows)
                {
                    mn = new ModelName(Convert.ToInt32(row["MODEL_RID"]));
					mn.ModelID = Convert.ToString(row["MODEL_NAME"]);
					modelList.Add(mn);          
                }

                if (includeEmptySelection)
                {
                    cbModelName.Items.Add(new ModelNameCombo(Include.NoRID, ""));
                    mn = new ModelName(Include.NoRID);
					mn.ModelID = Convert.ToString("");
					modelList.Add(mn);
                    cbModelName.SelectedIndex = -1;
                }

                LoadModelComboBox(modelList);
                cbModelName.Enabled = true;
                base.FormLoaded = true;
            }
            catch (Exception ex)
            {
                HandleException(ex, "BindComboBoxes");
            }  
        }

        protected virtual DataTable GetFilteredModels(string nameFilter, bool isCaseSensitive)
        {
            throw new Exception("GetFilteredModels not overridden");
        }

        protected void cbModelName_MouseHover(object sender, EventArgs e)
        {
            string message = null;
            message = cbModelName.Text;
            ShowToolTip(sender, e, message);
        }
     
        protected void getSaveAsName(string _currModel,
                                     bool _newModel,
                                     ref string _saveAsName,
                                     ref int _modelRID,
                                     bool _modelLocked,
                                     ref bool _changeMade,
                                     ref bool saveAsCanceled)
        {
            if (_newModel && (_saveAsName == string.Empty))
            {
                bool continueSave = false;

                frmSaveAs formSaveAs = new frmSaveAs(SAB);

                formSaveAs.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

                if (PerformingSaveAs)
                {
                    formSaveAs.SaveAsName = _currModel;
                }

                while (!continueSave)
                {
                    formSaveAs.ShowDialog(this);
                    saveAsCanceled = formSaveAs.SaveCanceled;

                    if (!saveAsCanceled)
                    {
                        if (GetProfileKey(formSaveAs.SaveAsName) == -1)
                        {
                            _saveAsName = formSaveAs.SaveAsName;
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

                                AlterModelChangeType(eChangeType.update);
                                _modelRID = GetProfileKey(formSaveAs.SaveAsName);
                                _saveAsName = formSaveAs.SaveAsName;
                            }
                        }
                    }
                    else
                    {
                        continueSave = true;
                    }
                }
            }
        } // * * * * * * * * * * *  getSaveAsName  * * * * *    

        protected void ModelSave(string _saveAsName,
                                 int _modelRID,
                                 bool _modelLocked,
                             ref bool _changeMade)
        //   bool _changeMade) TT#2986 - MD - Newly created modeles not in dropdown list - RBeck
        {
            int saveKey;
            _modelRID = GetProfileKey(_saveAsName);
            saveKey = _modelRID;
            GetModelRID(_saveAsName, ref _modelRID);
            _changeMade = true;
            ChangePending = false;
            btnSave.Enabled = false;
            if (_newModel)
            {
                FormLoaded = false;
                LoadModelComboBox();
                if (_modelLocked &&
                    saveKey != Include.NoRID)
                {
                    ModelType(saveKey);
                    _modelLocked = false;
                }
                InitializeForm(_saveAsName);
                FormLoaded = true;
                ModelText(_saveAsName);
                _newModel = false;
            }
        }

        virtual protected bool NameValid()
        {
            bool isValid ;
            if (_newModel)
            {
                isValid = true;
                return isValid;
            }
            string modelName = cbModelName.Text.Trim();
            isValid = (modelName.Length > 0);
            if (!isValid) MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NameRequiredToSave));
            return isValid;
        }

        virtual protected bool VerifyGrid()
        {
            bool isValid = true;
            return isValid;
        }
 
        override public void ISave()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (NameValid() && VerifyGrid())
                {
                    PerformingSaveAs = false;
                    SaveChanges();
                }
             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        override public void ISaveAs()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (NameValid() && VerifyGrid())		 
                {
                    _newModel = true;
                    _saveAsName = string.Empty;
                    PerformingSaveAs = true;
                    SaveChanges();
                }
             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        override protected bool SaveChanges()
        {
            try
            {
                ModelPreSave();
                if (!ErrorFound)
                {
                    bool saveAsCanceled = false;

                    getSaveAsName(_currModel,
                                  _newModel,
                                  ref _saveAsName,
                                  ref _modelRID,
                                   _modelLocked,
                                  ref _changeMade,
                                  ref saveAsCanceled);
                    if (!saveAsCanceled)

                        ModelSave(_saveAsName,
                                  _modelRID,
                                  _modelLocked,
                                  ref _changeMade);
                    //_changeMade);  TT#2986 - MD - Newly created modeles not in dropdown list - RBeck
                    else
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SaveCanceled), this.Text,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    this.cbModelName.Enabled = true;
                }
                else
                {
                    DefaultErrorFound(defaultErrorFound);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return true;
        }

        protected virtual void cbModelName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (FormLoaded)
                {
                    CheckForPendingChanges();
                    if (cbModelName.SelectedIndex >= 0)
                    {
                        //_modelIndex = cbModelName.SelectedIndex;             //TT#1638 - MD - Revised Model Save - RBeck
                        // string modelName = ((ModelName)(_eligModelProfileList[_modelIndex])).ModelID;    //TT#1638 - MD - Revised Model Save - RBeck
                        string modelName = cbModelName.Text.ToString();
                        InitializeForm(modelName);
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        protected virtual void cbModelName_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            try
            {
                if (FormLoaded)
                {
                    CheckForPendingChanges();
                    if (cbModelName.SelectedIndex >= 0)
                    {
                        //_modelIndex = cbModelName.SelectedIndex;             //TT#1638 - MD - Revised Model Save - RBeck
                        // string modelName = ((ModelName)(_eligModelProfileList[_modelIndex])).ModelID;    //TT#1638 - MD - Revised Model Save - RBeck
                        string modelName = cbModelName.Text.ToString();
                        InitializeForm(modelName);
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        override public void IDelete()
        {
            try
            {
                DeleteModel();
            }
            catch
            {
                throw;
            }
        }

        override public void IRefresh()
        {

        }
  
        protected void picBoxFilter_Click(object sender, System.EventArgs e)
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

        protected void picBoxFilter_MouseHover(object sender, System.EventArgs e)
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

        protected bool CharMaskFromDialogOK(PictureBox aPicBox, ref string aEnteredMask, ref bool aCaseSensitive)
        {
            bool maskOK = false;
            string errMessage = string.Empty;

            try
            {
                bool cancelAction = false;
                string dialogLabel = MIDText.GetTextOnly((int)eMIDTextCode.lbl_FilterSelection);
                string textLabel = MIDText.GetTextOnly((int)eMIDTextCode.lbl_FilterSelectionText);

                string globalMask = Convert.ToString(aPicBox.Tag, CultureInfo.CurrentUICulture);

                NameDialog nameDialog = new NameDialog(dialogLabel, textLabel, globalMask);
                nameDialog.AllowCaseSensitive();

                while (!(maskOK || cancelAction))
                {
                    nameDialog.StartPosition = FormStartPosition.CenterParent;
                    nameDialog.TreatEmptyAsCancel = false;
                    DialogResult dialogResult = nameDialog.ShowDialog();

                    if (dialogResult == DialogResult.Cancel)
                        cancelAction = true;
                    else
                    {
                        maskOK = false;
                        aEnteredMask = nameDialog.TextValue.Trim();
                        aCaseSensitive = nameDialog.CaseSensitive;
                        maskOK = (globalMask == string.Empty) ? true : EnteredMaskOK(aPicBox, aEnteredMask, globalMask);


                        if (!maskOK)
                        {
                            errMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_FilterGlobalOptionMismatch);
                            MessageBox.Show(errMessage, dialogLabel, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                if (cancelAction)
                {
                    maskOK = false;
                }
                else
                {
                    nameDialog.Dispose();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            if (!aEnteredMask.EndsWith("*"))
            {
                aEnteredMask += "*";
            }

            return maskOK;
        }

        protected bool EnteredMaskOK(PictureBox aPicBox, string aEnteredMask, string aGlobalMask)
        {
            bool maskOK = true;
            try
            {
                //bool okToContinue = true;
                int gXCount = 0;
                int eXCount = 0;
                int gWCount = 0;
                int eWCount = 0;
                char wildCard = '*';

                char[] cGlobalArray = aGlobalMask.ToCharArray(0, aGlobalMask.Length);
                for (int i = 0; i < cGlobalArray.Length; i++)
                {
                    if (cGlobalArray[i] == wildCard)
                    {
                        gWCount++;
                    }
                    else
                    {
                        gXCount++;
                    }
                }
                char[] cEnteredArray = aEnteredMask.ToCharArray(0, aEnteredMask.Length);
                for (int i = 0; i < cEnteredArray.Length; i++)
                {
                    if (cEnteredArray[i] == wildCard)
                    {
                        eWCount++;
                    }
                    else
                    {
                        eXCount++;
                    }
                }

                if (eXCount < gXCount)
                {
                    maskOK = false;
                }
                else if (eXCount > gXCount && gWCount == 0)
                {
                    maskOK = false;
                }
                else if (aEnteredMask.Length < aGlobalMask.Length && !aGlobalMask.EndsWith("*"))
                {
                    maskOK = false;
                }
                string[] globalParts = aGlobalMask.Split(new char[] { '*' });
                string[] enteredParts = aEnteredMask.Split(new char[] { '*' });
                int gLastEntry = globalParts.Length - 1;
                int eLastEntry = enteredParts.Length - 1;
                if (enteredParts[0].Length < globalParts[0].Length)
                {
                    maskOK = false;
                }
                else if (enteredParts[eLastEntry].Length < globalParts[gLastEntry].Length)
                {
                    maskOK = false;
                }
            }
            catch
            {
                throw;
            }
            return maskOK;
        }

        protected void DisplayPictureBoxImage(System.Windows.Forms.PictureBox aPicBox)
        {
            Image image;
            try
            {
                image = Image.FromFile(MIDGraphics.ImageDir + "\\" + MIDGraphics.MagnifyingGlassImage);
                SizeF sizef = new SizeF(aPicBox.Width, aPicBox.Height);
                Size size = Size.Ceiling(sizef);
                Bitmap bitmap = new Bitmap(image, size);
                aPicBox.Image = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        //End     TT#1638 - MD - Revised Model Save - RBeck

//Begin     TT#395 - MD - Override Low Level not displayed - RBeck
        void cbModelName_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cbModelName_SelectionChangeCommitted(source, new EventArgs());
        }
//End      TT#395 - MD - Override Low Level not displayed - RBeck

        //BEGIN TT#110-MD-VStuart - In Use Tool
        private void btnInUse_Click(object sender, EventArgs e)
        {
            inQuiry2 = true;
            ShowInUse();
        }
        //END TT#110-MD-VStuart - In Use Tool

        //BEGIN TT#110-MD-VStuart - In Use Tool
        /// <summary>
        /// This value determines if this is just for information or is mandatory.
        /// </summary>
        public bool inQuiry2
        {
            get { return _isInQuiry; }
            set { _isInQuiry = value; }
        }
        //END TT#110-MD-VStuart - In Use Tool

        // Begin TT#4063 - JSmith - Sales Modifier> Trying to Delete a Row in an existing Model and it will not allow a Save after the Delete.
        void ugModel_AfterRowsDeleted(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
                btnSave.Enabled = true;
            }
        }
        // End TT#4063 - JSmith - Sales Modifier> Trying to Delete a Row in an existing Model and it will not allow a Save after the Delete.
    }   
}
