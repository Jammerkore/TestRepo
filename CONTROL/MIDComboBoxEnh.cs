using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using MIDRetail.Business;
using MIDRetail.DataCommon;  //TT#7 - RBeck - Dynamic dropdowns

namespace MIDRetail.Windows.Controls
{
    /// <summary>
    /// Summary description for MIDComboBoxEnh.
    ///     This control is intended to be a combo box with an auto adjustment property added to it.
    ///     It also has some properties pre-set so that Suggest/Append is a default, but this
    ///     can be readily changed by the developer at design time. Security features may be added in 
    ///     the future form MID Tag
    /// </summary>
    public partial class MIDComboBoxEnh : UserControl
    {
        public event MIDComboBoxPropertiesChangedEventHandler MIDComboBoxPropertiesChangedEvent;

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        int _correctedSelectedIndex = -1;
        object _currentValue;
        int _currentInt;
        string _currentString;
        bool _ignoreFocusLost = false;
        // End TT#301-MD - JSmith - Controls are not functioning properly
        private ToolTip _toolTip = new ToolTip();

        public MIDComboBoxEnh()
        {
            InitializeComponent();
        }

        #region Properties

        //BEGIN TT#62-MD-VStuart-4.2/5.0 Infrastructure Enhancements
        public MyComboBox ComboBox
        {
            get
            {
                return (comboBox1);
            }
            set
            {
                comboBox1 = value;
            }
        }

        //END TT#62-MD-VStuart-4.2/5.0 Infrastructure Enhancements

        //BEGIN TT#1790-VStuart-Enhanced ComboBox Loses Tool Tip-MID
        //public string SetToolTip
        //{
        //    get
        //    {
        //        return string.Empty;
        //    }
        //    set
        //    {
        //        _toolTip.SetToolTip(comboBox1, value);
        //    }
        //}

        public string SetToolTip
        {
            get
            {
                return _toolTip.GetToolTip(comboBox1);
            }
            set
            {
                _toolTip.SetToolTip(comboBox1, value);
            }
        }


        //END TT#1790-VStuart-Enhanced ComboBox Loses Tool Tip-MID

        public bool IgnoreFocusLost
        {
            get
            {
                return _ignoreFocusLost;
            }
            set
            {
                _ignoreFocusLost = value;
            }
        }

        // Needed property variable to store the state of auto adjustment.
        private Boolean blnAutoAdjust = true;

        // Begin TT#286-MD - JSmith - Global Lock errors on Process -- missing From/To Levels
        new public string Name
        {
            get
            {
                return comboBox1.Name;
            }
            set
            {
                comboBox1.Name = value;
            }
        }
        // End TT#286-MD - JSmith - Global Lock errors on Process -- missing From/To Levels

        public int ItemHeight
        {
            get
            {
                return comboBox1.ItemHeight;
            }
            set
            {
                comboBox1.ItemHeight = value;
            }
        }

        // Begin TT#532-MD - JSmith - Export method tried to create a new method and receive a TargetInvocationException.  Also tried to open all existing methods and received the same error.
        //public string SelectedText
        //{
        //    get
        //    {
        //        return comboBox1.SelectedText;
        //    }
        //    set
        //    {
        //        comboBox1.SelectedText = value;
        //    }
        //}
        public string Get_SelectedText()
        {
            return comboBox1.SelectedText;
        }
        public void Set_SelectedText(string value)
        {
            comboBox1.SelectedText = value;
        }

        //public int SelectionStart
        //{
        //    get
        //    {
        //        return comboBox1.SelectionStart;
        //    }
        //    set
        //    {
        //        comboBox1.SelectionStart = value;
        //    }
        //}
        public int Get_SelectionStart()
        {
            return comboBox1.SelectionStart;
        }
        public void Set_SelectionStart(int value)
        {
            comboBox1.SelectionStart = value;
        }
        // End TT#532-MD - JSmith - Export method tried to create a new method and receive a TargetInvocationException.  Also tried to open all existing methods and received the same error.

        public int MaxDropDownItems
        {
            get
            {
                return comboBox1.MaxDropDownItems;
            }
            set
            {
                comboBox1.MaxDropDownItems = value;
            }
        }
        

        //
        // Summary:
        //     Gets or sets a value indicating whether the combo box should be auto adjusted for the largest element width.
        //
        // Returns:
        //     true if the combo box should be auto adjusted; otherwise, false. The default is false.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     An attempt was made to auto adjust a System.Windows.Forms.ComboBox that is attached
        //     to a data source.
        public bool AutoAdjust
        {
            get
            {
                return blnAutoAdjust;
            }
            set
            {
                blnAutoAdjust = value;
            }
        }
        
        //public string Name
        //{
        //    get
        //    {
        //        return (comboBox1.Name);
        //    }
        //    set
        //    {
        //        comboBox1.Name = value;
        //    }
        //}
        //public int TabIndex { get; set; }
        //public object Tag { get; set; }
        public ComboBox.ObjectCollection Items
        {
            get
            {
                return (comboBox1.Items);
            }
        }
        public AutoCompleteMode AutoCompleteMode
        {
            get
            {
                return (comboBox1.AutoCompleteMode);
            }
            set
            {
                comboBox1.AutoCompleteMode = value;
            }
        }

        public AutoCompleteSource AutoCompleteSource
        {
            get
            {
                return (comboBox1.AutoCompleteSource);
            }
            set
            {
                comboBox1.AutoCompleteSource = value;
            }
        }

        public bool FormattingEnabled
        {
            get
            {
                return (comboBox1.FormattingEnabled);
            }
            set
            {
                comboBox1.FormattingEnabled = value;
            }
        }

        

        //
        // Summary:
        //     Gets or sets a value specifying the style of the combo box.
        //
        // Returns:
        //     One of the System.Windows.Forms.ComboBoxStyle values. The default is DropDown.
        //
        // Exceptions:
        //   System.ComponentModel.InvalidEnumArgumentException:
        //     The assigned value is not one of the System.Windows.Forms.ComboBoxStyle values.
        [RefreshProperties(RefreshProperties.Repaint)]
        //[SRDescription("ComboBoxStyleDescr")]
        //[SRCategory("CatAppearance")]
        //public ComboBoxStyle DropDownStyle { get; set; }
        public ComboBoxStyle DropDownStyle
        {
            get
            {
                return (comboBox1.DropDownStyle);
            }
            set
            {
                comboBox1.DropDownStyle = value;
            }
        }

        //
        // Summary:
        //     Gets or sets the width of the of the drop-down portion of a combo box.
        //
        // Returns:
        //     The width, in pixels, of the drop-down box.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The specified value is less than one.
        //[SRCategory("CatBehavior")]
        //[SRDescription("ComboBoxDropDownWidthDescr")]
        //public int DropDownWidth { get; set; }
        public int DropDownWidth
        {
            get
            {
                return (comboBox1.DropDownWidth);
            }
            set
            {
                if (value > 0)
                {
                    comboBox1.DropDownWidth = value;
                }
            }
        }

        // From ListControl
        //
        // Summary:
        //     Gets or sets the value of the member property specified by the System.Windows.Forms.ListControl.ValueMember
        //     property.
        //
        // Returns:
        //     An object containing the value of the member of the data source specified
        //     by the System.Windows.Forms.ListControl.ValueMember property.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The assigned value is null or the empty string ("").
        [Browsable(false)]
        [DefaultValue("")]
        [Bindable(true)]
        //[SRCategory("CatData")]
        //[SRDescription("ListControlSelectedValueDescr")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public object SelectedValue { get; set; }
        public object SelectedValue
        {
            get
            {
                return (comboBox1.SelectedValue);
            }
            set
            {
                _currentValue = comboBox1.SelectedValue;
                comboBox1.SelectedValue = value;
                // fire EventArgs if changed
                if (MIDComboBoxPropertiesChangedEvent != null &&
                    _currentValue != value)
                {
                    MIDComboBoxPropertiesChangedEvent(this, new MIDComboBoxPropertiesChangedEventArgs());
                }
            }
        }

        // From ListControl
        //
        // Summary:
        //     Gets or sets the property to display for this System.Windows.Forms.ListControl.
        //
        // Returns:
        //     A System.String specifying the name of an object property that is contained
        //     in the collection specified by the System.Windows.Forms.ListControl.DataSource
        //     property. The default is an empty string ("").
        [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        [DefaultValue("")]
        //[SRDescription("ListControlDisplayMemberDescr")]
        //[SRCategory("CatData")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        //public string DisplayMember { get; set; }
        public string DisplayMember
        {
            get
            {
                return (comboBox1.DisplayMember);
            }
            set
            {
                comboBox1.DisplayMember = value;
            }
        }

        // From ListControl
        //
        // Summary:
        //     Gets or sets the property to use as the actual value for the items in the
        //     System.Windows.Forms.ListControl.
        //
        // Returns:
        //     A System.String representing the name of an object property that is contained
        //     in the collection specified by the System.Windows.Forms.ListControl.DataSource
        //     property. The default is an empty string ("").
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The specified property cannot be found on the object specified by the System.Windows.Forms.ListControl.DataSource
        //     property.
        //[SRCategory("CatData")]
        [DefaultValue("")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        //[SRDescription("ListControlValueMemberDescr")]
        //public string ValueMember { get; set; }
        public string ValueMember
        {
            get
            {
                return (comboBox1.ValueMember);
            }
            set
            {
                comboBox1.ValueMember = value;
            }
        }

        // From ComboBox
        //
        // Summary:
        //     Gets or sets the index specifying the currently selected item.
        //
        // Returns:
        //     A zero-based index of the currently selected item. A value of negative one
        //     (-1) is returned if no item is selected.
        //
        // Exceptions:
        //   System.ArgumentOutOfRangeException:
        //     The specified index is less than or equal to -2.  -or- The specified index
        //     is greater than or equal to the number of items in the combo box.
        //private int _SelectedIndex;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get
            {
                return comboBox1.SelectedIndex;
            }
            set
            {
                _currentInt = comboBox1.SelectedIndex;
                comboBox1.SelectedIndex = value;
                // fire EventArgs if changed
                if (MIDComboBoxPropertiesChangedEvent != null &&
                    _currentInt != value)
                {
                    MIDComboBoxPropertiesChangedEvent(this, new MIDComboBoxPropertiesChangedEventArgs());
                }
            }
        }

        // From ComboBox
        // Summary:
        //     Gets or sets the data source for this System.Windows.Forms.ComboBox.
        //
        // Returns:
        //     An object that implements the System.Collections.IList interface, such as
        //     a System.Data.DataSet or an System.Array. The default is null.
        [DefaultValue("")]
        [AttributeProvider(typeof(IListSource))]
        //[SRCategory("CatData")]
        //[SRDescription("ListControlDataSourceDescr")]
        [RefreshProperties(RefreshProperties.Repaint)]
        //public object DataSource { get; set; }
        public object DataSource
        {
            get
            {
                return (comboBox1.DataSource);
            }
            set
            {
                comboBox1.DataSource = value;
                // fire EventArgs if changed
                if (MIDComboBoxPropertiesChangedEvent != null &&
                    value != null)
                {
                    MIDComboBoxPropertiesChangedEvent(this, new MIDComboBoxPropertiesChangedEventArgs());
                }
            }
        }

        // From Control
        // Summary:
        //     Gets or sets the object that contains data about the control.
        //
        // Returns:
        //     An System.Object that contains data about the control. The default is null.
        [Bindable(true)]
        //[SRCategory("CatData")]
        //[SRDescription("ControlTagDescr")]
        [DefaultValue("")]
        [Localizable(false)]
        [TypeConverter(typeof(StringConverter))]
        //public object Tag { get; set; }
        public object Tag
        {
            get
            {
                return (comboBox1.Tag);
            }
            set
            {
                comboBox1.Tag = value;
            }
        }

        //From ComboBox
        // Summary:
        //     Gets or sets currently selected item in the System.Windows.Forms.ComboBox.
        //
        // Returns:
        //     The object that is the currently selected item or null if there is no currently
        //     selected item.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //[SRDescription("ComboBoxSelectedItemDescr")]
        [Browsable(false)]
        [Bindable(true)]
        //public object SelectedItem { get; set; }
        public object SelectedItem
        {
            get
            {
                return (comboBox1.SelectedItem);
            }
            set
            {
                _currentValue = comboBox1.SelectedItem;
                comboBox1.SelectedItem = value;
                // fire EventArgs if changed
                if (MIDComboBoxPropertiesChangedEvent != null &&
                    _currentValue != value)
                {
                    MIDComboBoxPropertiesChangedEvent(this, new MIDComboBoxPropertiesChangedEventArgs());
                }
            }
        }

        //Begin TT#301-MD-VStuart-Version 5.0-Controls
        //
        // Summary:
        //     Gets or sets a value indicating whether the items in the combo box are sorted.
        //
        // Returns:
        //     true if the combo box is sorted; otherwise, false. The default is false.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     An attempt was made to sort a System.Windows.Forms.ComboBox that is attached
        //     to a data source.
        [DefaultValue(false)]
        public bool Sorted 
        {
            get
            {
                return (comboBox1.Sorted);
            }
            set
            {
                comboBox1.Sorted = value;
            }
        }
        //End TT#301-MD-VStuart-Version 5.0-Controls
        override public string Text
        {
            get
            {
                return (comboBox1.Text);
            }
            set
            {
                _currentString = comboBox1.Text;
                comboBox1.Text = value;
                // fire EventArgs if changed
                if (MIDComboBoxPropertiesChangedEvent != null &&
                    _currentString != value)
                {
                    MIDComboBoxPropertiesChangedEvent(this, new MIDComboBoxPropertiesChangedEventArgs());
                }
            }
        }

        new public bool Enabled
        {
            get
            {
                return (comboBox1.Enabled);
            }
            set
            {
                comboBox1.Enabled = value;
            }
        }

        // Begin TT#2659 - JSmith - Spacing and Hover functionality
        //new public bool Visible
        //{
        //    get
        //    {
        //        return (comboBox1.Visible);
        //    }
        //    set
        //    {
        //        comboBox1.Visible = value;
        //    }
        //}
        // End TT#2659 - JSmith - Spacing and Hover functionality

        override public bool AllowDrop
        {
            get
            {
                return (comboBox1.AllowDrop);
            }
            set
            {
                comboBox1.AllowDrop = value;
            }
        }

		// BEGIN TT#2703 - stodd - select first comboBox on QuickFilter
		public bool DroppedDown
		{
			set
			{
				comboBox1.DroppedDown = value;
			}
		}
		// END TT#2703 - stodd - select first comboBox on QuickFilter

        override public bool Focused
        {
            get
            {
                return (comboBox1.Focused);
            }
        }

        public event EventHandler SelectedIndexChanged
        {
            add { comboBox1.SelectedIndexChanged += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.SelectedIndexChanged -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        public event EventHandler SelectionChangeCommitted
        {
            add { comboBox1.SelectionChangeCommitted += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.SelectionChangeCommitted -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        public event EventHandler SelectedValueChanged
        {
            add { comboBox1.SelectedValueChanged += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.SelectedValueChanged -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        new public event DragEventHandler DragDrop
        {
            add { comboBox1.DragDrop += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.DragDrop -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        new public event DragEventHandler DragEnter
        {
            add { comboBox1.DragEnter += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.DragEnter -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        new public event DragEventHandler DragOver
        {
            add { comboBox1.DragOver += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.DragOver -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        new public event KeyEventHandler KeyDown
        {
            add { comboBox1.KeyDown += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.KeyDown -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        new public event CancelEventHandler Validating
        {
            add { comboBox1.Validating += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.Validating -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        new public event EventHandler Validated
        {
            add { comboBox1.Validated += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.Validated -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        new public event EventHandler EnabledChanged
        {
            add { comboBox1.EnabledChanged += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.EnabledChanged -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        new public event EventHandler MouseHover
        {
            add { comboBox1.MouseHover += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.MouseHover -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        public event EventHandler DropDown
        {
            add { comboBox1.DropDown += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.DropDown -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        public event EventHandler DropDownClosed
        {
            add { comboBox1.DropDownClosed += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.DropDownClosed -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        //BEGIN TT#306-MD-VStuart-Version 5.0-Style Review Attribute drop down and the Tab key not getting expected results
        new public event EventHandler TextChanged
        {
            add { comboBox1.TextChanged += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.TextChanged -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }

        public event EventHandler TextUpdate
        {
            add { comboBox1.TextUpdate += value; }
            remove
            {
                //Begin TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
                if (comboBox1 != null)
                {
                    comboBox1.TextUpdate -= value;
                }
                //End TT#1290-MD -jsobek -Receive Unhandled Exception after closing Report and clicking Cancel on the Seleciton
            }
        }
        //END TT#306-MD-VStuart-Version 5.0-Style Review Attribute drop down and the Tab key not getting expected results

        #endregion Properties

        #region Methods

        public void FirePropertyChangeEvent()
        {
            if (MIDComboBoxPropertiesChangedEvent != null)
            {
                MIDComboBoxPropertiesChangedEvent(this, new MIDComboBoxPropertiesChangedEventArgs());
            }
        }

        //AutoAdjust Method
        // BEGIN TT#1701 - AGallagher - Dynamic drop-down fields - Infrastructure change only
        public void AdjustTextWidthComboBox_DropDown(object sender)
        {
            try
            {
                ComboBox senderComboBox = (ComboBox)sender;
                int width = senderComboBox.DropDownWidth;
                Graphics g = senderComboBox.CreateGraphics();
                Font font = senderComboBox.Font;
                //checks if a scrollbar will be displayed.
                //If yes, then get its width to adjust the size of the drop down list.
                int vertScrollBarWidth =
                    (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                    ? SystemInformation.VerticalScrollBarWidth : 0;

                //Loop through list items and check size of each items.
                //set the width of the drop down list to the width of the largest item.
                int newWidth;
                foreach (object o in ((ComboBox)sender).Items)
                {
                    if (o != null)
                    {
                        string s = string.Empty;
                        string displayMember = senderComboBox.DisplayMember;
                        if (o.GetType() == typeof(DataRowView))
                        {
                            DataRowView drv = (DataRowView)o;

                            if (drv.Row.Table.Columns.Contains(displayMember))
                            {
                                s = drv[displayMember].ToString();
                            }
                            else
                            {
                                s = o.ToString();
                            }
                        }
//Begin TT#7 - RBeck - Dynamic dropdowns
                        else if (o.GetType() == typeof(VersionProfile))
                        {
                           VersionProfile member = (VersionProfile)o;
                            s = member.Description;
                        }
//End   TT#7 - RBeck - Dynamic dropdowns

                            // Vic - I commentted out the section below to make it more generic.
                            // Also since these types didn't exist in my demo I had to omit them.
                        else if (o.GetType() == typeof(StoreGroupListViewProfile))
                        {
                            StoreGroupListViewProfile sgp = (StoreGroupListViewProfile)o;
                            s = sgp.GroupId;
                        }
                        else if (o.GetType() == typeof(StoreGroupLevelListViewProfile))
                        {
                            StoreGroupLevelListViewProfile sglp = (StoreGroupLevelListViewProfile)o;
                            s = sglp.Name;
                        }
                        else
                        {
                            Type oType = o.GetType();   //>> uncomment if a type needs to be looked at to cast
                            s = o.ToString();
                        }
                        newWidth = (int)g.MeasureString(s.Trim(), font).Width
                                    + vertScrollBarWidth;
                        if (width < newWidth)
                        {
                            width = newWidth;
                        }
                    }
                }
                senderComboBox.DropDownWidth = width;
            }
            catch (Exception objException)
            {
                //Catch objException
                string message = objException.ToString();
                throw;
            }
        }
        // END TT#1701 - AGallagher - Dynamic drop-down fields - Infrastructure change only

        public int FindStringExact(string s)
        {
            return comboBox1.FindStringExact(s);
        }

        #endregion Methods

        #region events

        //
        // Summary:
        //      This event turns on auto adjustment of the user controls drop down combo box
        //      when the boolean property is set to true.
        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            if (blnAutoAdjust == true)
                AdjustTextWidthComboBox_DropDown(this.comboBox1);
        }

        //
        // Summary:
        //     Occurs when the drop-down portion of a System.Windows.Forms.ComboBox is shown.
        //[SRDescription("ComboBoxOnDropDownDescr")]
        //[SRCategory("CatBehavior")]
        //public event EventHandler DropDown;

        //
        // Summary:
        //     Occurs when the drop-down portion of the System.Windows.Forms.ComboBox is
        //     no longer visible.
        //[SRCategory("CatBehavior")]
        //[SRDescription("ComboBoxOnDropDownClosedDescr")]
        //public event EventHandler DropDownClosed;

        // From ComboBox
        // Summary:
        //     Occurs when the selected item has changed and that change is displayed in
        //     the System.Windows.Forms.ComboBox.
        //[SRCategory("CatBehavior")]
        //[SRDescription("selectionChangeCommittedEventDescr")]
        //public event EventHandler SelectionChangeCommitted;

        //
        // Summary:
        //     Occurs when the System.Windows.Forms.SelectedIndex property has
        //     changed.
        //[SRDescription("selectedIndexChangedEventDescr")]
        //[SRCategory("CatBehavior")]
        //public event EventHandler SelectedIndexChanged;

        //BEGIN TT#62-MD-VStuart-4.2/5.0 Infrastructure Enhancements
        //
        // Summary:
        //     Occurs when a key is pressed while the control has focus.
        //[SRDescription("ControlOnKeyDownDescr")]
        //[SRCategory("CatKey")]
        //public event KeyEventHandler KeyDown;

        private void MIDComboBoxEnh_Load(object sender, EventArgs e)
        {
            comboBox1.Name = this.Name;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _correctedSelectedIndex = comboBox1.SelectedIndex;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _correctedSelectedIndex = comboBox1.SelectedIndex;
        }

        void comboBox1_LostFocus(object sender, EventArgs e)
        {
            if (!_ignoreFocusLost)
            {
                // Begin TT#3916 - JSmith\VStuart - Get ArgumentOutOfRange exception dragging merchandise to method
                //if (comboBox1.SelectedIndex != _correctedSelectedIndex)
                if (comboBox1.SelectedIndex != _correctedSelectedIndex &&
                     _correctedSelectedIndex < comboBox1.Items.Count - 1)
                // End TT#3916 - JSmith\VStuart - Get ArgumentOutOfRange exception dragging merchandise to method
                {
                    comboBox1.SelectedIndex = _correctedSelectedIndex;
                }
            }

        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            // Begin TT#3916 - JSmith\VStuart - Get ArgumentOutOfRange exception dragging merchandise to method
            //if (comboBox1.SelectedIndex != _correctedSelectedIndex)
            if (comboBox1.SelectedIndex != _correctedSelectedIndex &&
                _correctedSelectedIndex < comboBox1.Items.Count - 1)
            // End TT#3916 - JSmith\VStuart - Get ArgumentOutOfRange exception dragging merchandise to method
            {
                comboBox1.SelectedIndex = _correctedSelectedIndex;
            }
        }
        //END TT#62-MD-VStuart-4.2/5.0 Infrastructure Enhancements



        #endregion events
    }

    #region MIDComboBoxEnh Properties Changed Event
    /// <summary>
    /// Identifies an OptionPackProfile whose properties have changed
    /// </summary>
    [Serializable]
    public class MIDComboBoxPropertiesChangedEventArgs : EventArgs
    {

        public MIDComboBoxPropertiesChangedEventArgs()
        {

        }
        
    }
    /// <summary>
    /// MIDComboBoxPropertiesChangedEventHandler Delegate
    /// </summary>
    /// <param name="source">Object firing the event</param>
    /// <param name="args">Arguments passed by the event</param>
    public delegate void MIDComboBoxPropertiesChangedEventHandler(object source, MIDComboBoxPropertiesChangedEventArgs args);

    #endregion MIDComboBoxEnh Properties Changed Event

}
