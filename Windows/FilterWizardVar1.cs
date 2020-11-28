using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Business;
using MIDRetail.Windows.Controls;

using MIDRetail.Business.Allocation;
using MIDRetail.Data;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for FilterWizardVar1.
	/// </summary>
	public class FilterWizardVar1 : FilterWizardControl
	{
		#region Component Designer generated code
		private FilterWizardConditionVar1Panel pnlVar1;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label6;
        private MIDRetail.Windows.Controls.MIDDateRangeSelector mdsDateRange;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtMerchandise;
		private System.Windows.Forms.Panel pnlTitle;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ErrorProvider errorProvider;
		//Begin TT#1532 - DOConnell - Display of Date Range in Filters does not differentitate between Static and Dynamic
        private string _graphicsDir;
        private Image _dynamicToPlanImage;
        private Image _dynamicToCurrentImage;
        private MIDComboBoxEnh cboVariable;
        private MIDComboBoxEnh cboVersion;
        private MIDComboBoxEnh cboCubeModifyer;
        private MIDComboBoxEnh cboTimeModifyer;
        private MIDComboBoxEnh cboCompareTo;
        private IContainer components;

        /// <summary>
        /// Gets the graphics directory.
        /// </summary>
        public string GraphicsDirectory
        {
            get { return _graphicsDir; }
        }
        //End TT#1532 - DOConnell - Display of Date Range in Filters does not differentitate between Static and Dynamic

		public FilterWizardVar1()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public FilterWizardVar1(frmFilterWizard aParentForm, SessionAddressBlock aSAB, int aConditionIdx)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_SAB = aSAB;
			_dateSel = new CalendarDateSelector(_SAB);

			_parentForm = aParentForm;
			_parentForm.SuspendLayout();
			_parentForm.Controls.Add(pnlVar1);
			_parentForm.Controls.Add(pnlTitle);
			_parentForm.ResumeLayout(false);

			pnlVar1.Index = aConditionIdx;
			pnlVar1.Name += "-" + aConditionIdx;
			pnlVar1.ParentControl = this;

			mdsDateRange.DateRangeRID = Include.UndefinedCalendarDateRange;
			//Begin TT#1532 - DOConnell - Display of Date Range in Filters does not differentitate between Static and Dynamic
            // Begin TT#588-MD - JSmith - Remove ApplicationRoot from configuration
            //_graphicsDir = MIDConfigurationManager.AppSettings["ApplicationRoot"] + MIDGraphics.GraphicsDir;
            _graphicsDir = MIDGraphics.MIDGraphicsDir;
            // End TT#588-MD - JSmith - Remove ApplicationRoot from configuration
            _dynamicToPlanImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToPlanImage);
            _dynamicToCurrentImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToCurrentImage);
			//End TT#1532 - DOConnell - Display of Date Range in Filters does not differentitate between Static and Dynamic
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

				this.cboVariable.SelectionChangeCommitted -= new System.EventHandler(this.cboVariable_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboVariable.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboVariable_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

				this.mdsDateRange.Click -= new System.EventHandler(this.mdsDateRange_Click);
				this.mdsDateRange.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsDateRange_OnSelection);
				this.txtMerchandise.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtMerchandise_KeyPress);
				this.txtMerchandise.Validating -= new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
				this.txtMerchandise.Validated -= new System.EventHandler(this.txtMerchandise_Validated);
				this.txtMerchandise.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
				this.txtMerchandise.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
				this.txtMerchandise.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
//Begin Track #5111 - JScott - Add additional filter functionality
                this.cboTimeModifyer.SelectionChangeCommitted -= new System.EventHandler(this.cboTimeModifyer_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboTimeModifyer.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboTimeModifyer_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
//End Track #5111 - JScott - Add additional filter functionality
				_dateSel.Dispose();
			}
			base.Dispose( disposing );
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
			this.pnlVar1 = new MIDRetail.Windows.FilterWizardConditionVar1Panel();
            this.cboVariable = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.label39 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.mdsDateRange = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
			this.label5 = new System.Windows.Forms.Label();
			this.txtMerchandise = new System.Windows.Forms.TextBox();
			this.pnlTitle = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cboVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboCubeModifyer = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboTimeModifyer = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboCompareTo = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.pnlVar1.SuspendLayout();
			this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlVar1
			// 
			this.pnlVar1.BackColor = System.Drawing.SystemColors.Control;
			this.pnlVar1.BackPanel = null;
			this.pnlVar1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVar1.Controls.Add(this.cboCompareTo);
            this.pnlVar1.Controls.Add(this.cboTimeModifyer);
            this.pnlVar1.Controls.Add(this.cboCubeModifyer);
            this.pnlVar1.Controls.Add(this.cboVersion);
			this.pnlVar1.Controls.Add(this.cboVariable);
            this.pnlVar1.Controls.Add(this.label39);
			this.pnlVar1.Controls.Add(this.label17);
			this.pnlVar1.Controls.Add(this.label16);
			this.pnlVar1.Controls.Add(this.label15);
			this.pnlVar1.Controls.Add(this.label14);
			this.pnlVar1.Controls.Add(this.label6);
			this.pnlVar1.Controls.Add(this.mdsDateRange);
			this.pnlVar1.Controls.Add(this.label5);
			this.pnlVar1.Controls.Add(this.txtMerchandise);
			this.pnlVar1.DefaultControl = null;
			this.pnlVar1.Index = 0;
			this.pnlVar1.IsBackEnabled = true;
			this.pnlVar1.IsNextEnabled = false;
			this.pnlVar1.Location = new System.Drawing.Point(0, 56);
			this.pnlVar1.Name = "pnlVar1";
			this.pnlVar1.NextPanel = null;
			this.pnlVar1.NextText = "Next >";
			this.pnlVar1.ParentControl = null;
			this.pnlVar1.Size = new System.Drawing.Size(600, 280);
			this.pnlVar1.TabIndex = 15;
			this.pnlVar1.TitlePanel = this.pnlTitle;
			// 
            // cboVariable
			// 
            this.cboVariable.AutoAdjust = true;
            this.cboVariable.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboVariable.DataSource = null;
            this.cboVariable.DisplayMember = null;
			this.cboVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVariable.DropDownWidth = 0;
			this.cboVariable.Location = new System.Drawing.Point(24, 40);
			this.cboVariable.Name = "cboVariable";
			this.cboVariable.Size = new System.Drawing.Size(176, 21);
			this.cboVariable.TabIndex = 43;
            this.cboVariable.Tag = null;
            this.cboVariable.ValueMember = null;
			this.cboVariable.SelectionChangeCommitted += new System.EventHandler(this.cboVariable_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboVariable.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboVariable_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            // 
            // label39
            // 
            this.label39.Location = new System.Drawing.Point(24, 24);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(72, 16);
            this.label39.TabIndex = 44;
            this.label39.Text = "Variable:";
            // 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(456, 24);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(80, 16);
			this.label17.TabIndex = 41;
			this.label17.Text = "Compare To:";
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(264, 64);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(112, 16);
			this.label16.TabIndex = 39;
			this.label16.Text = "Time Comparison:";
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(264, 24);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(88, 16);
			this.label15.TabIndex = 38;
			this.label15.Text = "Value Type:";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(24, 144);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(72, 16);
			this.label14.TabIndex = 37;
			this.label14.Text = "Date Range:";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(24, 104);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 16);
			this.label6.TabIndex = 36;
			this.label6.Text = "Version:";
			// 
			// mdsDateRange
			// 
			this.mdsDateRange.DateRangeForm = null;
			this.mdsDateRange.DateRangeRID = 0;
			this.mdsDateRange.Enabled = false;
			this.mdsDateRange.Location = new System.Drawing.Point(24, 160);
			this.mdsDateRange.Name = "mdsDateRange";
			this.mdsDateRange.Size = new System.Drawing.Size(176, 24);
			this.mdsDateRange.TabIndex = 33;
			this.mdsDateRange.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsDateRange_OnSelection);
            this.mdsDateRange.Click += new System.EventHandler(this.mdsDateRange_Click);
            // 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(24, 64);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 16);
			this.label5.TabIndex = 31;
			this.label5.Text = "Merchandise:";
			// 
			// txtMerchandise
			// 
			this.txtMerchandise.AllowDrop = true;
			this.txtMerchandise.Location = new System.Drawing.Point(24, 80);
			this.txtMerchandise.Name = "txtMerchandise";
			this.txtMerchandise.Size = new System.Drawing.Size(176, 20);
			this.txtMerchandise.TabIndex = 30;
            this.txtMerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
            this.txtMerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
            this.txtMerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
			this.txtMerchandise.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMerchandise_KeyPress);
			this.txtMerchandise.Validating += new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
			this.txtMerchandise.Validated += new System.EventHandler(this.txtMerchandise_Validated);
            // 
			// pnlTitle
			// 
			this.pnlTitle.BackColor = System.Drawing.SystemColors.Window;
			this.pnlTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlTitle.Controls.Add(this.label7);
			this.pnlTitle.Controls.Add(this.label8);
			this.pnlTitle.Location = new System.Drawing.Point(0, 0);
			this.pnlTitle.Name = "pnlTitle";
			this.pnlTitle.Size = new System.Drawing.Size(600, 56);
			this.pnlTitle.TabIndex = 21;
			// 
			// label7
			// 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(48, 32);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(512, 16);
			this.label7.TabIndex = 1;
			this.label7.Text = "Specify the first variable that will be compared to another value.";
			// 
			// label8
			// 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(24, 8);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(256, 16);
			this.label8.TabIndex = 0;
			this.label8.Text = "First Variable Definition";
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
            // cboVersion
            // 
            this.cboVersion.AutoAdjust = true;
            this.cboVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboVersion.DataSource = null;
            this.cboVersion.DisplayMember = null;
            this.cboVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVersion.DropDownWidth = 0;
            this.cboVersion.Location = new System.Drawing.Point(24, 120);
            this.cboVersion.Name = "cboVersion";
            this.cboVersion.Size = new System.Drawing.Size(176, 21);
            this.cboVersion.TabIndex = 32;
            this.cboVersion.Tag = null;
            this.cboVersion.ValueMember = null;
            // 
            // cboCubeModifyer
            // 
            this.cboCubeModifyer.AutoAdjust = true;
            this.cboCubeModifyer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCubeModifyer.DataSource = null;
            this.cboCubeModifyer.DisplayMember = null;
            this.cboCubeModifyer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCubeModifyer.DropDownWidth = 0;
            this.cboCubeModifyer.Location = new System.Drawing.Point(267, 40);
            this.cboCubeModifyer.Name = "cboCubeModifyer";
            this.cboCubeModifyer.Size = new System.Drawing.Size(121, 21);
            this.cboCubeModifyer.TabIndex = 34;
            this.cboCubeModifyer.Tag = null;
            this.cboCubeModifyer.ValueMember = null;
            // 
            // cboTimeModifyer
            // 
            this.cboTimeModifyer.AutoAdjust = true;
            this.cboTimeModifyer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboTimeModifyer.DataSource = null;
            this.cboTimeModifyer.DisplayMember = null;
            this.cboTimeModifyer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTimeModifyer.DropDownWidth = 0;
            this.cboTimeModifyer.Location = new System.Drawing.Point(264, 80);
            this.cboTimeModifyer.Name = "cboTimeModifyer";
            this.cboTimeModifyer.Size = new System.Drawing.Size(121, 21);
            this.cboTimeModifyer.TabIndex = 35;
            this.cboTimeModifyer.Tag = null;
            this.cboTimeModifyer.ValueMember = null;
            this.cboTimeModifyer.SelectionChangeCommitted += new System.EventHandler(this.cboTimeModifyer_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboTimeModifyer.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboTimeModifyer_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            // 
            // cboCompareTo
            // 
            this.cboCompareTo.AutoAdjust = true;
            this.cboCompareTo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboCompareTo.DataSource = null;
            this.cboCompareTo.DisplayMember = null;
            this.cboCompareTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCompareTo.DropDownWidth = 0;
            this.cboCompareTo.Location = new System.Drawing.Point(456, 40);
            this.cboCompareTo.Name = "cboCompareTo";
            this.cboCompareTo.Size = new System.Drawing.Size(121, 21);
            this.cboCompareTo.TabIndex = 40;
            this.cboCompareTo.Tag = null;
            this.cboCompareTo.ValueMember = null;
            // 
			// FilterWizardVar1
			// 
			this.Controls.Add(this.pnlTitle);
			this.Controls.Add(this.pnlVar1);
			this.Name = "FilterWizardVar1";
            this.Size = new System.Drawing.Size(600, 377);
            this.pnlVar1.ResumeLayout(false);
            this.pnlVar1.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private SessionAddressBlock _SAB;
		private CalendarDateSelector _dateSel;
		private bool _textChanged = false;
		private bool _priorError = false;

		public FilterWizardConditionVar1Panel MainPanel
		{
			get
			{
				return pnlVar1;
			}
		}

        // Begin TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard
        //public System.Windows.Forms.ComboBox VariableCombo
        //{
        //    get
        //    {
        //        return cboVariable.ComboBox;
        //    }
        //}

        //public System.Windows.Forms.ComboBox CompareToCombo
        //{
        //    get
        //    {
        //        return cboCompareTo.ComboBox;
        //    }
        //}

        //public System.Windows.Forms.ComboBox TimeModifyerCombo
        //{
        //    get
        //    {
        //        return cboTimeModifyer.ComboBox;
        //    }
        //}

        //public System.Windows.Forms.ComboBox CubeModifyerCombo
        //{
        //    get
        //    {
        //        return cboCubeModifyer.ComboBox;
        //    }
        //}

        public MIDComboBoxEnh VariableCombo
		{
			get
			{
                return cboVariable;
			}
		}

        public MIDComboBoxEnh CompareToCombo
		{
			get
			{
				return cboCompareTo;
			}
		}

        public MIDComboBoxEnh TimeModifyerCombo
		{
			get
			{
                return cboTimeModifyer;
			}
		}

        public MIDComboBoxEnh CubeModifyerCombo
		{
			get
			{
                return cboCubeModifyer;
			}
		}
        // End TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard
        

		public MIDRetail.Windows.Controls.MIDDateRangeSelector DateRangeSelector
		{
			get
			{
				return mdsDateRange;
			}
		}

        // Begin TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard
        //public System.Windows.Forms.ComboBox VersionCombo
        //{
        //    get
        //    {
        //        return cboVersion.ComboBox;
        //    }
        //}

        public MIDComboBoxEnh VersionCombo
        {
            get
            {
                return cboVersion;
            }
        }
        // End TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard

		public System.Windows.Forms.TextBox MerchandiseTextBox
		{
			get
			{
				return txtMerchandise;
			}
		}

		public void HandleExceptions(Exception exc)
		{
			Debug.WriteLine(exc.ToString());
			MessageBox.Show(exc.ToString());
		}

		private void cboVariable_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (cboVariable.SelectedIndex != -1)
			{
				pnlVar1.IsNextEnabled = true;
			}
			else
			{
				pnlVar1.IsNextEnabled = false;
			}

			FireNextButtonStatusChangedEvent(pnlVar1.IsNextEnabled);
		}

		private void txtMerchandise_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
                _parentForm.Image_DragEnter(sender, e);
                _parentForm.Merchandise_DragEnter(sender, e);
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void txtMerchandise_DragOver(object sender, DragEventArgs e)
		{
			_parentForm.Image_DragOver(sender, e);
		}

		private void txtMerchandise_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            // Begin TT#1911 - JSmith - Filter Wizard Not Allowing Merchandise @ Class Level
            //IDataObject data;
            // End TT#1911
            TreeNodeClipboardList cbList;
			HierarchyNodeProfile hnp;

			try
			{
                // Begin TT#1911 - JSmith - Filter Wizard Not Allowing Merchandise @ Class Level
                //data = Clipboard.GetDataObject();
                // End TT#1911

                // Begin TT#1911 - JSmith - Filter Wizard Not Allowing Merchandise @ Class Level
				//if (data.GetDataPresent(typeof(TreeNodeClipboardList)))
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                // End TT#1911
				{
                    // Begin TT#1911 - JSmith - Filter Wizard Not Allowing Merchandise @ Class Level
					//cbList = (TreeNodeClipboardList)data.GetData(typeof(TreeNodeClipboardList));
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    // End TT#1911

                    //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
                    if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
					{
                        //if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
                        //{
                            hnp = _SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key,false,true);  //TT#1916 - MD - Correction for color dragdrop  - rbeck 
							txtMerchandise.Text = hnp.Text;
							txtMerchandise.Tag = hnp;
                        //}
                        //else
                        //{
                        //    MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard));
                        //}
					}
					else
					{
						MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
					}
				}
				else
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void txtMerchandise_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			_textChanged = true;
		}

		private void txtMerchandise_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string errorMessage;

			try
			{
				if (txtMerchandise.Text == string.Empty && txtMerchandise.Tag != null)
				{
					txtMerchandise.Text = string.Empty;
					txtMerchandise.Tag = null;
				}
				else
				{
					if (_textChanged)
					{
						_textChanged = false;

						HierarchyNodeProfile hnp = GetNodeProfile(txtMerchandise.Text);
						if (hnp.Key == Include.NoRID)
						{
							_priorError = true;

							errorMessage = string.Format(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), txtMerchandise.Text);
							errorProvider.SetError(txtMerchandise, errorMessage);
							MessageBox.Show(errorMessage);

							e.Cancel = true;
						}
						else 
						{
							txtMerchandise.Text = hnp.Text;
							txtMerchandise.Tag = hnp;
						}	
					}
					else if (_priorError)
					{
						if (txtMerchandise.Tag == null)
						{
							txtMerchandise.Text = string.Empty;
						}
						else
						{
							txtMerchandise.Text = ((HierarchyNodeProfile)txtMerchandise.Tag).Text;
						}
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void txtMerchandise_Validated(object sender, System.EventArgs e)
		{
			try
			{
				errorProvider.SetError(txtMerchandise, string.Empty);
				_textChanged = false;
				_priorError = false;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void mdsDateRange_Click(object sender, System.EventArgs e)
		{
			try
			{
				mdsDateRange.Enabled = true;
				mdsDateRange.DateRangeForm = _dateSel;
				_dateSel.DateRangeRID = mdsDateRange.DateRangeRID;
				_dateSel.AnchorDate = _SAB.ClientServerSession.Calendar.CurrentDate;
				_dateSel.AnchorDateRelativeTo = eDateRangeRelativeTo.Plan;
				_dateSel.AllowDynamicToStoreOpen = true;
				mdsDateRange.ShowSelector();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void mdsDateRange_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			try
			{
				if (e.SelectedDateRange != null)
				{
                    mdsDateRange.DateRangeRID = e.SelectedDateRange.Key;
					//Begin TT#1532 - DOConnell - Display of Date Range in Filters does not differentitate between Static and Dynamic
                    if (e.SelectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
					{
						switch (e.SelectedDateRange.RelativeTo)
						{
							case eDateRangeRelativeTo.Current:
                                mdsDateRange.SetImage(_dynamicToCurrentImage);
								break;
							case eDateRangeRelativeTo.Plan:
                                mdsDateRange.SetImage(_dynamicToPlanImage);
								break;
							default:
								mdsDateRange.SetImage(null);
								break;
						}
					}
					//End TT#1532 - DOConnell - Display of Date Range in Filters does not differentitate between Static and Dynamic
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

//Begin Track #5111 - JScott - Add additional filter functionality
		private void cboTimeModifyer_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (cboTimeModifyer.SelectedIndex != -1)
				{
					if ((eFilterTimeModifyer)((ComboObject)cboTimeModifyer.SelectedItem).Key == eFilterTimeModifyer.Join)
					{
						mdsDateRange.DateRangeRID = Include.UndefinedCalendarDateRange;
						mdsDateRange.Text = string.Empty;
						mdsDateRange.Enabled = false;
					}
					else
					{
						mdsDateRange.Enabled = true;
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cboVariable_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboVariable_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboTimeModifyer_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboTimeModifyer_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

//End Track #5111 - JScott - Add additional filter functionality
		private HierarchyNodeProfile GetNodeProfile(string aProductID)
		{
			string productID;
			string[] pArray;

			try
			{
				productID = aProductID.Trim();
				pArray = productID.Split(new char[] {'['});
				productID = pArray[0].Trim(); 

//				return _SAB.HierarchyServerSession.GetNodeData(productID);
				HierarchyMaintenance hm = new HierarchyMaintenance(_SAB);
				EditMsgs em = new EditMsgs();
				return hm.NodeLookup(ref em, productID, false);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
