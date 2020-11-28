using System;
using System.Drawing;
using System.Collections;
using System.Data;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;   //TT#301-MD-VStuart-Version 5.0-Controls

namespace MIDRetail.Windows
{
	public struct QuickFilterData
	{
		public int [] SelectedIndex;
		public string [] TextBoxText;
		public bool [] CheckBoxChecked;
	}	
		
	/// <summary>
	/// Summary description for QuickFilter.
	/// </summary>
	public class QuickFilter : MIDFormBase
	{
		// BEGIN TT#2703 - stodd - select first comboBox on QuickFilter
		private bool _selectAndExpandFirstComboBox = false;
		public bool SelectAndExpandFirstComboBox
		{
			get { return _selectAndExpandFirstComboBox; }
			set { _selectAndExpandFirstComboBox = value; }
		}
		// END TT#2703 - stodd - select first comboBox on QuickFilter

		#region Windows Form Designer generated code
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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

				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.Closing -= new System.ComponentModel.CancelEventHandler(this.QuickFilter_Closing);
				this.Load -= new System.EventHandler(this.QuickFilter_Load);
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.CausesValidation = false;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(204, 272);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(124, 272);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// QuickFilter
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(288, 302);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "QuickFilter";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.QuickFilter_Closing);
			this.Load += new System.EventHandler(this.QuickFilter_Load);
			this.ResumeLayout(false);

		}
		#endregion

		public delegate void ComponentSelectedIndexChangeEventHandler(object source, ComponentSelectedIndexChangeEventArgs e);
		public event ComponentSelectedIndexChangeEventHandler OnComponentSelectedIndexChangeEventHandler;
		public delegate void CheckBoxCheckChangedEventHandler(object source, CheckChangedEventArgs e);
		public event CheckBoxCheckChangedEventHandler OnCheckBoxCheckChangedEventHandler;
		public delegate bool ValidateFieldsHandler(object source);
		public event ValidateFieldsHandler OnValidateFieldsHandler;
	  	const int MIDPadding = 24;
		const int MIDSpacing = 36;
		private int _numComponents;
		private int _numTextBoxes = 0;
		private int _numCheckBoxes = 0;
		private ComponentEntryInfo[] _componentInfo;
		private string[] _componentLabels;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private eMDIChildCloseAction _closingAction;
		private TextEntryInfo[] _textInfo;
		private CheckBoxEntryInfo[] _checkBoxInfo;

		public QuickFilter(eQuickFilterType aQuickFilterType, int aNumComponents, params string[] aComponentLabels)
		{
			//
			// Required for Windows Form Designer support
			//
			try
			{
				InitializeComponent();

				if (aComponentLabels.Length != aNumComponents)
				{
					throw new Exception("Invalid number of component labels");
				}

				if (aQuickFilterType == eQuickFilterType.QuickFilter)
				{
					this.Text = "Quick Filter";
				}
				else
				{
					this.Text = "Find";
				}

				_numComponents = aNumComponents;
				_componentLabels = (string[])aComponentLabels.Clone();
				_componentInfo = null;
				_closingAction = eMDIChildCloseAction.Cancel;
				// BEGIN TT#2703 - stodd - select first comboBox on QuickFilter
				_selectAndExpandFirstComboBox = false;
				// END TT#2703 - stodd - select first comboBox on QuickFilter
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEGIN TT#2703 - stodd - select first comboBox on QuickFilter
		/// <summary>
		/// Includes the selectAndExpand parm.
		/// </summary>
		/// <param name="aQuickFilterType"></param>
		/// <param name="aNumComponents"></param>
		/// <param name="selectAndExpand"></param>
		/// <param name="aComponentLabels"></param>
		public QuickFilter(eQuickFilterType aQuickFilterType, int aNumComponents, bool selectAndExpand, params string[] aComponentLabels)
		{
			try
			{
				InitializeComponent();
				if (aComponentLabels.Length != aNumComponents)
				{
					throw new Exception("Invalid number of component labels");
				}
				if (aQuickFilterType == eQuickFilterType.QuickFilter)
				{
					this.Text = "Quick Filter";
				}
				else
				{
					this.Text = "Find";
				}
				_numComponents = aNumComponents;
				_componentLabels = (string[])aComponentLabels.Clone();
				_componentInfo = null;
				_closingAction = eMDIChildCloseAction.Cancel;
				_selectAndExpandFirstComboBox = selectAndExpand;
				
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		// END TT#2703 - stodd - select first comboBox on QuickFilter

		// Constructor Overload to include text boxes and check boxes
		public QuickFilter(eQuickFilterType aQuickFilterType, int aNumTextBoxes, int aNumCheckBoxes, int aNumComponents, params string[] aComponentLabels )
		{
			//
			// Required for Windows Form Designer support
			//
			try
			{
				InitializeComponent();

				if (aComponentLabels.Length != aNumComponents)
				{
					throw new Exception("Invalid number of component labels");
				}

				if (aQuickFilterType == eQuickFilterType.QuickFilter)
				{
					this.Text = "Quick Filter";
				}
				else
				{
					this.Text = "Find";
				}

				_numComponents = aNumComponents;
				_componentLabels = (string[])aComponentLabels.Clone();
				_componentInfo = null;
				 
				_numTextBoxes = aNumTextBoxes;
				_numCheckBoxes = aNumCheckBoxes;
				_textInfo = null;
				_checkBoxInfo = null;

				_closingAction = eMDIChildCloseAction.Cancel;
				
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private ComponentEntryInfo[] ComponentInfo
		{
			get
			{
				int i;

				try
				{
					if (_componentInfo == null)
					{
						_componentInfo = new ComponentEntryInfo[_numComponents];

						for (i = 0; i < _numComponents; i++)
						{
							_componentInfo[i] = new ComponentEntryInfo(this, i);
							_componentInfo[i].LabelText = _componentLabels[i];
						}
					}

					return _componentInfo;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}


		private TextEntryInfo[] TextInfo
		{
			get
			{
				int i;

				try
				{
					if (_textInfo == null)
					{
						_textInfo = new TextEntryInfo[_numTextBoxes];

						for (i = 0; i < _numTextBoxes; i++)
						{
							_textInfo[i] = new TextEntryInfo(this, i);
							_textInfo[i].LabelText = string.Empty;
						}
					}

					return _textInfo;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		
		private CheckBoxEntryInfo[] CheckBoxInfo
		{
			get
			{
				int i;

				try
				{
					if (_checkBoxInfo == null)
					{
						_checkBoxInfo = new CheckBoxEntryInfo[_numCheckBoxes];

						for (i = 0; i < _numCheckBoxes; i++)
						{
							_checkBoxInfo[i] = new CheckBoxEntryInfo(this, i);
						}
					}

					return _checkBoxInfo;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private void QuickFilter_Load(object sender, System.EventArgs e)
		{
			int i, j;
			int maxLabelWidth;

			try
			{
				maxLabelWidth = 0;

				for (i = 0; i < _numComponents; i++)
				{
					this.Controls.Add(ComponentInfo[i].Label);
					this.Controls.Add(ComponentInfo[i].ComboBox);

					ComponentInfo[i].Label.AutoSize = true;

					maxLabelWidth = System.Math.Max(maxLabelWidth, ComponentInfo[i].Label.Width);
				}

				for (i = 0; i < _numComponents; i++)
				{
					ComponentInfo[i].Label.AutoSize = false;
					ComponentInfo[i].Label.Width = maxLabelWidth;
					ComponentInfo[i].Label.Top = (i * MIDSpacing) + MIDPadding;
					ComponentInfo[i].Label.Left = MIDPadding;

					ComponentInfo[i].ComboBox.Top = (i * MIDSpacing) + MIDPadding;
					ComponentInfo[i].ComboBox.Left = maxLabelWidth + MIDPadding;
					ComponentInfo[i].ComboBox.Width = this.Width - ComponentInfo[i].ComboBox.Left - MIDSpacing;
				}
				
				j = i;
				for (i = 0; i < _numTextBoxes; i++)
				{
					this.Controls.Add(TextInfo[i].Label);
					this.Controls.Add(TextInfo[i].TextBox);

					TextInfo[i].Label.AutoSize = true;

					maxLabelWidth = System.Math.Max(maxLabelWidth, TextInfo[i].Label.Width);
				}	
				 
				for (i = 0; i < _numTextBoxes; i++)
				{
					TextInfo[i].Label.AutoSize = false;
					TextInfo[i].Label.Width = maxLabelWidth;
					TextInfo[i].Label.Top = (j * MIDSpacing) + MIDPadding;
					TextInfo[i].Label.Left = MIDPadding;

					TextInfo[i].TextBox.Top = (j * MIDSpacing) + MIDPadding;
					TextInfo[i].TextBox.Left = maxLabelWidth + MIDPadding;
					TextInfo[i].TextBox.Width = this.Width - TextInfo[i].TextBox.Left - MIDSpacing;
					j++;
				}
				
				for (i = 0; i < _numCheckBoxes; i++)
				{
					this.Controls.Add(CheckBoxInfo[i].CheckBox);
				}	
				 
				for (i = 0; i < _numCheckBoxes; i++)
				{
					CheckBoxInfo[i].CheckBox.Top = (j * MIDSpacing) + MIDPadding;
					CheckBoxInfo[i].CheckBox.Left = maxLabelWidth + MIDPadding;
					CheckBoxInfo[i].CheckBox.Width = this.Width - CheckBoxInfo[i].CheckBox.Left - MIDSpacing;
					j++;
				}

				this.Height = ((_numComponents + _numTextBoxes + _numCheckBoxes + 1) * MIDSpacing) + MIDPadding + btnOK.Height + 20;

				// BEGIN TT#2703 - stodd - select first comboBox on QuickFilter
				if (SelectAndExpandFirstComboBox)
				{
					ComponentInfo[0].ComboBox.Select();
					// Uncomment to implement where the comboBox is already expanded.
					//ComponentInfo[0].ComboBox.DroppedDown = true;
					Cursor.Current = Cursors.Default;
				}
				// END TT#2703 - stodd - select first comboBox on QuickFilter
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void LoadComboBox(int aComponentIdx, DataTable aDTValues, string aDisplayColumn)
		{
			try
			{
				ComponentInfo[aComponentIdx].ComboBox.Items.Clear();
				ComponentInfo[aComponentIdx].ComboBox.Items.Add("(None)");

				foreach (DataRow row in aDTValues.Rows)
				{
					ComponentInfo[aComponentIdx].ComboBox.Items.Add(row[aDisplayColumn]);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void LoadComboBox(int aComponentIdx, DataView aDVValues, string aDisplayColumn)
		{
			try
			{
				ComponentInfo[aComponentIdx].ComboBox.Items.Clear();
				ComponentInfo[aComponentIdx].ComboBox.Items.Add("(None)");

				foreach (DataRowView row in aDVValues)
				{
					ComponentInfo[aComponentIdx].ComboBox.Items.Add(row[aDisplayColumn]);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void LoadComboBox(int aComponentIdx, ArrayList aValueList)
		{
			try
			{
				ComponentInfo[aComponentIdx].ComboBox.Items.Clear();
				ComponentInfo[aComponentIdx].ComboBox.Items.Add("(None)");

				foreach (object obj in aValueList)
				{
					ComponentInfo[aComponentIdx].ComboBox.Items.Add(obj);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        //BEGIN TT#6-MD-VStuart - Single Store Select
        // Note: This is a supplementary call that adds the suggest/append and autojust capability to the combo box call above.
        public void LoadComboBoxAutoFill(int aComponentIdx, ArrayList aValueList)
        {
            //This section gives the user the autocomplete feature with suggest and append in the dropdown.
            ComponentInfo[aComponentIdx].ComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComponentInfo[aComponentIdx].ComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            //BEGIN TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
            //This routine adjust the width of the drop down to the longest entry.
            //AdjustTextWidthComboBox_DropDown(ComponentInfo[aComponentIdx].ComboBox);
            //END TT#301-MD-VStuart-Version 5.0-Controls are not functioning properly
        }
        //END TT#6-MD-VStuart - Single Store Select 

        public void LoadComboBox(int aComponentIdx, SortedList aSortedList)
		{
			try
			{
				ComponentInfo[aComponentIdx].ComboBox.Items.Clear();
				ComponentInfo[aComponentIdx].ComboBox.Items.Add("(None)");

				foreach (DictionaryEntry entry in aSortedList)
				{
					ComponentInfo[aComponentIdx].ComboBox.Items.Add(entry.Value);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void ClearComboBox(int aComponentIdx)
		{
			try
			{
				ComponentInfo[aComponentIdx].ComboBox.Items.Clear();
				ComponentInfo[aComponentIdx].ComboBox.Text = String.Empty;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		public void SetComboxBoxIndex(int aComponentIdx, int aIndex )
		{
			try
			{
				ComponentInfo[aComponentIdx].ComboBox.SelectedIndex = aIndex;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void EnableComboBox(int aComponentIdx)
		{
			try
			{
				ComponentInfo[aComponentIdx].ComboBox.Enabled = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void DisableComboBox(int aComponentIdx)
		{
			try
			{
				ComponentInfo[aComponentIdx].ComboBox.SelectedIndex = -1;
				ComponentInfo[aComponentIdx].ComboBox.Enabled = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		public void DisableComboBoxLeaveSelected(int aComponentIdx)
		{
			try
			{
				ComponentInfo[aComponentIdx].ComboBox.Enabled = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void EnableTextBox(int aTextBoxIdx, bool EnableControl)
		{
			try
			{
				if (EnableControl)
					TextInfo[aTextBoxIdx].TextBox.Enabled = true;
				else
					TextInfo[aTextBoxIdx].TextBox.Enabled = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		public void SetCheckBox(int aCbxIdx,  bool aChecked)
		{
			try
			{
				if (aChecked)
					CheckBoxInfo[aCbxIdx].CheckBox.Checked = true;
				else
					CheckBoxInfo[aCbxIdx].CheckBox.Checked = false;

			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		public bool GetCheckBox(int aCbxIdx)
		{
			try
			{
				return CheckBoxInfo[aCbxIdx].CheckBox.Checked;  
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void LoadComboBoxLabel(int aComponentIdx, string aLabel)
		{
			try
			{
				ComponentInfo[aComponentIdx].LabelText = aLabel;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		public void LoadTextBoxLabel(int aTextBoxIdx, string aLabel)
		{
			try
			{
				TextInfo[aTextBoxIdx].LabelText = aLabel;
           	}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		public void LoadCheckBoxText(int aCbxIdx, string aText)
		{
			try
			{
				CheckBoxInfo[aCbxIdx].CheckBox.Text = aText;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void SetComboBoxSort(int aComponentIdx, bool IsSorted)
		{
			try
			{
				ComponentInfo[aComponentIdx].ComboBox.Sorted = IsSorted;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public int GetSelectedIndex(int aComponentIdx)
		{
			try
			{
				return ComponentInfo[aComponentIdx].ComboBox.SelectedIndex - 1;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public object GetSelectedItem(int aComponentIdx)
		{
			try
			{
				return ComponentInfo[aComponentIdx].ComboBox.SelectedItem;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void SetError(int aComponentIdx, string ErrorMessage)
		{
			try
			{
				ErrorProvider.SetError(ComponentInfo[aComponentIdx].ComboBox, ErrorMessage);
				 
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public string GetTextBoxText(int aTextBoxIdx)
		{
			try
			{
				return TextInfo[aTextBoxIdx].TextBox.Text;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void SetTextBoxText(int aTextBoxIdx, string aValue)
		{
			try
			{
				TextInfo[aTextBoxIdx].TextBox.Text = aValue;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void SetErrorTextBox(int aTextBoxIdx, string ErrorMessage)
		{
			try
			{
				ErrorProvider.SetError(TextInfo[aTextBoxIdx].TextBox, ErrorMessage);
				 
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void ComboBoxSelectionChangeCommitted(object sender, System.EventArgs e)
		{
			int componentIdx;

			try
			{
                componentIdx = (int)((ComboBox)sender).Tag;

				if (ComponentInfo[componentIdx].ComboBox.SelectedIndex != -1)
				{
					if (ComponentInfo[componentIdx].ComboBox.SelectedIndex == 0)
					{
						ComponentInfo[componentIdx].ComboBox.SelectedIndex = -1;
					}
				}

				if (OnComponentSelectedIndexChangeEventHandler != null)
				{
					OnComponentSelectedIndexChangeEventHandler(this, new ComponentSelectedIndexChangeEventArgs(componentIdx, ComponentInfo[componentIdx].ComboBox.SelectedIndex - 1));
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

        public void MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.ComboBoxSelectionChangeCommitted(((MIDComboBoxEnh)source).ComboBox, new EventArgs());
        }

		public void CheckBoxCheckChanged(object sender, System.EventArgs e)
		{
			int componentIdx;

			try
			{
				componentIdx = (int)((System.Windows.Forms.CheckBox)sender).Tag;

				if (OnCheckBoxCheckChangedEventHandler != null)
				{
					OnCheckBoxCheckChangedEventHandler(this, new CheckChangedEventArgs(componentIdx, CheckBoxInfo[componentIdx].CheckBox.Checked));
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void QuickFilter_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (_closingAction == eMDIChildCloseAction.OK)
			{
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
			else
			{
				this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if (OnValidateFieldsHandler != null)
			{
				if (!OnValidateFieldsHandler(this))
					return;
			}
			_closingAction = eMDIChildCloseAction.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			_closingAction = eMDIChildCloseAction.Cancel;
			this.Close();
		}
	}

    public class ComponentEntryInfo 
	{
		private QuickFilter _form;
		private int _componentIdx;
        private MIDComboBoxEnh _comboBox;   //TT#301-MD-VStuart-Version 5.0-Controls
		private System.Windows.Forms.Label _label;

		public ComponentEntryInfo(QuickFilter aForm, int aComponentIdx)
		{
			_form = aForm;
			_componentIdx = aComponentIdx;
		}

        //Begin TT#301-MD-VStuart-Version 5.0-Controls
		//new public System.Windows.Forms.ComboBox ComboBox
        new public MIDComboBoxEnh ComboBox
		{
			get
			{
				try
				{
					if (_comboBox == null)
					{
                        _comboBox = new MIDComboBoxEnh();
						_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        //End TT#301-MD-VStuart-Version 5.0-Controls
						_comboBox.Text = System.String.Empty;
						_comboBox.Tag = _componentIdx;
						_comboBox.SelectionChangeCommitted += new System.EventHandler(_form.ComboBoxSelectionChangeCommitted);
                        _comboBox.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(_form.MIDComboBoxPropertiesChangedEvent);
						_comboBox.SelectedIndex = -1;
						_comboBox.Sorted = true;
					}
                    return _comboBox;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		public System.Windows.Forms.Label Label
		{
			get
			{
				try
				{
					if (_label == null)
					{
						_label = new System.Windows.Forms.Label();
						_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
					}

					return _label;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		public string LabelText
		{
			set
			{
				try
				{
					Label.Text = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
	}

	public class TextEntryInfo
	{
		private QuickFilter _form;
		private int _textIdx;
		private System.Windows.Forms.TextBox _textBox;
		private System.Windows.Forms.Label _label;

		public TextEntryInfo(QuickFilter aForm, int aTextIdx)
		{
			_form = aForm;
			_textIdx = aTextIdx;
		}

		public System.Windows.Forms.TextBox TextBox
		{
			get
			{
				try
				{
					if (_textBox == null)
					{
						_textBox = new System.Windows.Forms.TextBox();
						_textBox.Text = System.String.Empty;
						_textBox.Tag = _textIdx;
						//_textBox.Validating += new System.ComponentModel.CancelEventHandler(_form.TextBoxValidating);
					}
					return _textBox;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		public System.Windows.Forms.Label Label
		{
			get
			{
				try
				{
					if (_label == null)
					{
						_label = new System.Windows.Forms.Label();
						_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
					}

					return _label;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		public string LabelText
		{
			set
			{
				try
				{
					Label.Text = value;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
	}

	public class CheckBoxEntryInfo
	{
		private QuickFilter _form;
		private int _cbxIdx;
		private System.Windows.Forms.CheckBox _checkBox;
		//private System.Windows.Forms.Label _label;

		public CheckBoxEntryInfo(QuickFilter aForm, int aCbxIdx)
		{
			_form = aForm;
			_cbxIdx = aCbxIdx;
		}

		public System.Windows.Forms.CheckBox CheckBox
		{
			get
			{
				try
				{
					if (_checkBox == null)
					{
						_checkBox = new System.Windows.Forms.CheckBox();
						_checkBox.Text = System.String.Empty;
						_checkBox.Tag = _cbxIdx;
						_checkBox.CheckedChanged += new System.EventHandler(_form.CheckBoxCheckChanged);
					}

					return _checkBox;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
	}

	public class ComponentSelectedIndexChangeEventArgs : EventArgs
	{
		private int _componentIdx;
		private int _selectedIdx;

		public ComponentSelectedIndexChangeEventArgs(int aComponentIdx, int aSelectedIdx)
		{
			_componentIdx = aComponentIdx;
			_selectedIdx = aSelectedIdx;
		}

		public int ComponentIdx
		{
			get
			{
				return _componentIdx;
			}
		}

		public int SelectedIdx
		{
			get
			{
				return _selectedIdx;
			}
		}
	}

	public class CheckChangedEventArgs : EventArgs
	{
		private int _componentIdx;
		private bool _checked;

		public CheckChangedEventArgs(int aComponentIdx, bool aChecked)
		{
			_componentIdx = aComponentIdx;
			_checked = aChecked;
		}

		public int ComponentIdx
		{
			get
			{
				return _componentIdx;
			}
		}

		public bool Checked
		{
			get
			{
				return _checked;
			}
		}
	}
}
