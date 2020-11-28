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

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for FilterWizardPercent.
	/// </summary>
	public class FilterWizardPercent : FilterWizardControl
	{
		#region Component Designer generated code
		private FilterWizardConditionPercentPanel pnlPercent;
		private System.Windows.Forms.Label label40;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector mdsDateRange;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.TextBox txtValue;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.TextBox txtMerchandise;
		private System.Windows.Forms.Panel pnlTitle;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
        private MIDComboBoxEnh cboVariable;
        private MIDComboBoxEnh cboVersion;
        private MIDComboBoxEnh cboCubeModifyer;
        private MIDComboBoxEnh cboTimeModifyer;
        private MIDComboBoxEnh cboComparison;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FilterWizardPercent()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public FilterWizardPercent(frmFilterWizard aParentForm, SessionAddressBlock aSAB, int aConditionIdx)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_SAB = aSAB;
			_dateSel = new CalendarDateSelector(_SAB);

			_parentForm = aParentForm;
			_parentForm.SuspendLayout();
			_parentForm.Controls.Add(pnlPercent);
			_parentForm.Controls.Add(pnlTitle);
			_parentForm.ResumeLayout(false);

			pnlPercent.Index = aConditionIdx;
			pnlPercent.Name += "-" + aConditionIdx;
			pnlPercent.ParentControl = this;

			mdsDateRange.DateRangeRID = Include.UndefinedCalendarDateRange;
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
				this.txtValue.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtValue_KeyPress);
				this.txtValue.TextChanged -= new System.EventHandler(this.txtValue_TextChanged);
				this.txtMerchandise.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
				this.txtMerchandise.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
				this.txtMerchandise.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
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
			this.pnlPercent = new MIDRetail.Windows.FilterWizardConditionPercentPanel();
			this.label40 = new System.Windows.Forms.Label();
			this.mdsDateRange = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
			this.label26 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.txtValue = new System.Windows.Forms.TextBox();
			this.label21 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.txtMerchandise = new System.Windows.Forms.TextBox();
			this.pnlTitle = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
            this.cboVariable = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboCubeModifyer = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboTimeModifyer = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboComparison = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.pnlPercent.SuspendLayout();
			this.pnlTitle.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlPercent
			// 
			this.pnlPercent.BackColor = System.Drawing.SystemColors.Control;
			this.pnlPercent.BackPanel = null;
			this.pnlPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPercent.Controls.Add(this.cboComparison);
            this.pnlPercent.Controls.Add(this.cboTimeModifyer);
            this.pnlPercent.Controls.Add(this.cboCubeModifyer);
            this.pnlPercent.Controls.Add(this.cboVersion);
			this.pnlPercent.Controls.Add(this.cboVariable);
            this.pnlPercent.Controls.Add(this.label40);
			this.pnlPercent.Controls.Add(this.mdsDateRange);
			this.pnlPercent.Controls.Add(this.label26);
			this.pnlPercent.Controls.Add(this.label20);
			this.pnlPercent.Controls.Add(this.txtValue);
			this.pnlPercent.Controls.Add(this.label21);
			this.pnlPercent.Controls.Add(this.label22);
			this.pnlPercent.Controls.Add(this.label23);
			this.pnlPercent.Controls.Add(this.label24);
			this.pnlPercent.Controls.Add(this.label25);
			this.pnlPercent.Controls.Add(this.txtMerchandise);
			this.pnlPercent.DefaultControl = null;
			this.pnlPercent.Index = 0;
			this.pnlPercent.IsBackEnabled = true;
			this.pnlPercent.IsNextEnabled = false;
			this.pnlPercent.Location = new System.Drawing.Point(0, 56);
			this.pnlPercent.Name = "pnlPercent";
			this.pnlPercent.NextPanel = null;
			this.pnlPercent.NextText = "Next >";
			this.pnlPercent.ParentControl = null;
			this.pnlPercent.Size = new System.Drawing.Size(600, 280);
			this.pnlPercent.TabIndex = 16;
			this.pnlPercent.TitlePanel = this.pnlTitle;
			// 
			// label40
			// 
			this.label40.Location = new System.Drawing.Point(24, 24);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(72, 16);
			this.label40.TabIndex = 49;
			this.label40.Text = "Variable:";
			// 
			// mdsDateRange
			// 
			this.mdsDateRange.DateRangeForm = null;
			this.mdsDateRange.DateRangeRID = 0;
			this.mdsDateRange.Enabled = false;
			this.mdsDateRange.Location = new System.Drawing.Point(24, 160);
			this.mdsDateRange.Name = "mdsDateRange";
			this.mdsDateRange.Size = new System.Drawing.Size(176, 24);
			this.mdsDateRange.TabIndex = 47;
			this.mdsDateRange.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsDateRange_OnSelection);
			this.mdsDateRange.Click += new System.EventHandler(this.mdsDateRange_Click);
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(456, 24);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(80, 16);
			this.label26.TabIndex = 46;
			this.label26.Text = "Condition:";
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(456, 64);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(80, 16);
			this.label20.TabIndex = 44;
			this.label20.Text = "Percent Value:";
			// 
			// txtValue
			// 
			this.txtValue.AllowDrop = true;
			this.txtValue.Location = new System.Drawing.Point(456, 80);
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(88, 20);
			this.txtValue.TabIndex = 43;
			this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            this.txtValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValue_KeyPress);
            // 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(264, 64);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(104, 16);
			this.label21.TabIndex = 39;
			this.label21.Text = "Time Comparison:";
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(264, 24);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(88, 16);
			this.label22.TabIndex = 38;
			this.label22.Text = "Value Type:";
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(24, 144);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(72, 16);
			this.label23.TabIndex = 37;
			this.label23.Text = "Date Range:";
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(24, 104);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(72, 16);
			this.label24.TabIndex = 36;
			this.label24.Text = "Version:";
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(24, 64);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(72, 16);
			this.label25.TabIndex = 31;
			this.label25.Text = "Merchandise:";
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
			this.pnlTitle.TabIndex = 20;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(48, 32);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(512, 16);
			this.label7.TabIndex = 1;
			this.label7.Text = "Specify the percent variable that the first variable in the condition will be com" +
				"pared to.";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(24, 8);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(256, 16);
			this.label8.TabIndex = 0;
			this.label8.Text = "Percent Variable Definition";
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
            this.cboVariable.TabIndex = 48;
            this.cboVariable.Tag = null;
            this.cboVariable.ValueMember = null;
            this.cboVariable.SelectionChangeCommitted += new System.EventHandler(this.cboVariable_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboVariable.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboVariable_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
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
            this.cboCubeModifyer.Location = new System.Drawing.Point(264, 40);
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
            // cboComparison
            // 
            this.cboComparison.AutoAdjust = true;
            this.cboComparison.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboComparison.DataSource = null;
            this.cboComparison.DisplayMember = null;
            this.cboComparison.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboComparison.DropDownWidth = 0;
            this.cboComparison.Location = new System.Drawing.Point(456, 40);
            this.cboComparison.Name = "cboComparison";
            this.cboComparison.Size = new System.Drawing.Size(121, 21);
            this.cboComparison.TabIndex = 45;
            this.cboComparison.Tag = null;
            this.cboComparison.ValueMember = null;
            // 
			// FilterWizardPercent
			// 
			this.Controls.Add(this.pnlTitle);
			this.Controls.Add(this.pnlPercent);
			this.Name = "FilterWizardPercent";
			this.Size = new System.Drawing.Size(600, 336);
			this.pnlPercent.ResumeLayout(false);
			this.pnlPercent.PerformLayout();
			this.pnlTitle.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private SessionAddressBlock _SAB;
		private CalendarDateSelector _dateSel;

		public FilterWizardConditionPercentPanel MainPanel
		{
			get
			{
				return pnlPercent;
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

        //public System.Windows.Forms.ComboBox ComparisonCombo
        //{
        //    get
        //    {
        //        return cboComparison.ComboBox;
        //    }
        //}

        public MIDComboBoxEnh VersionCombo
        {
            get
            {
                return cboVersion;
            }
        }

        public MIDComboBoxEnh ComparisonCombo
        {
            get
            {
                return cboComparison;
            }
        }
        // End TT#399-MD - JSmith - Cannot Select Date Range within Filter Wizard

		public System.Windows.Forms.TextBox ValueTextBox
		{
			get
			{
				return txtValue;
			}
		}

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
			try
			{
				CheckForValidNext();

				FireNextButtonStatusChangedEvent(pnlPercent.IsNextEnabled);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
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

		private void txtValue_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				CheckForValidNext();

				FireNextButtonStatusChangedEvent(pnlPercent.IsNextEnabled);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void txtValue_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			try
			{
				if (e.KeyChar == 13 || e.KeyChar == 9)
				{
				}
				else if (e.KeyChar == 27)
				{
				}
				else if (Char.IsNumber(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '.' || Char.IsControl(e.KeyChar))
				{
				}
				else
				{
					e.Handled = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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
                // Begin TT#1911
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
                            hnp = _SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, false, true);  //TT#1916 - MD - Correction for color dragdrop  - rbeck 
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

		private void mdsDateRange_Click(object sender, System.EventArgs e)
		{
			try
			{
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

//End Track #5111 - JScott - Add additional filter functionality
		private void CheckForValidNext()
		{
			if (cboVariable.SelectedIndex != -1 && txtValue.Text.Length > 0)
			{
				pnlPercent.IsNextEnabled = true;
			}
			else
			{
				pnlPercent.IsNextEnabled = false;
			}
		}
	}
}
