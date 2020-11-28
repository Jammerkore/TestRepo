using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common; 
using MIDRetail.Data;
using MIDRetail.DataCommon; 

namespace MIDRetail.Windows.Controls
{
    public partial class MIDAttributeComboBox : MIDWindowsComboBox
    {
        public event MIDComboBoxPropertiesChangedEventHandler MIDComboBoxPropertiesChangedEvent;

        

        private bool _replaceAttribute;
        object _setValue;
        //SecurityAdmin _securityAdmin = null; //TT#827-MD -jsobek -Allocation Reviews Performance
        private bool _allowUserAttributes;
        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        int _correctedSelectedIndex = -1;
        object _currentValue;
        int _currentInt;
        string _currentString;
        // End TT#301-MD - JSmith - Controls are not functioning properly

        public MIDAttributeComboBox()
        {
            InitializeComponent();
            _continueReadOnly = false;
            _allowUserAttributes = false;
        }

        public bool ReplaceAttribute
        {
            get { return _replaceAttribute; }
        }

        public bool AllowUserAttributes
        {
            get { return _allowUserAttributes; }
            set { _allowUserAttributes = value; }
        }

        new public object SelectedValue
        {
            get { return base.SelectedValue; }
            set 
            {
                _setValue = value;
                _currentValue = base.SelectedValue;
                base.SelectedValue = value;
                // fire EventArgs if changed
                if (MIDComboBoxPropertiesChangedEvent != null &&
                    _currentValue != value)
                {
                    MIDComboBoxPropertiesChangedEvent(this, new MIDComboBoxPropertiesChangedEventArgs());
                }
            }
        }

        new public int SelectedIndex
        {
            get
            {
                return base.SelectedIndex;
            }
            set
            {
                _currentInt = base.SelectedIndex;
                base.SelectedIndex = value;
                // fire EventArgs if changed
                if (MIDComboBoxPropertiesChangedEvent != null &&
                    _currentInt != value)
                {
                    MIDComboBoxPropertiesChangedEvent(this, new MIDComboBoxPropertiesChangedEventArgs());
                }
            }
        }

        new public object DataSource
        {
            get
            {
                return (base.DataSource);
            }
            set
            {
                base.DataSource = value;
                // fire EventArgs if changed
                if (MIDComboBoxPropertiesChangedEvent != null &&
                    value != null)
                {
                    MIDComboBoxPropertiesChangedEvent(this, new MIDComboBoxPropertiesChangedEventArgs());
                }
            }
        }

        new public object SelectedItem
        {
            get
            {
                return (base.SelectedItem);
            }
            set
            {
                _currentValue = base.SelectedItem;
                base.SelectedItem = value;
                // fire EventArgs if changed
                if (MIDComboBoxPropertiesChangedEvent != null &&
                    _currentValue != value)
                {
                    MIDComboBoxPropertiesChangedEvent(this, new MIDComboBoxPropertiesChangedEventArgs());
                }
            }
        }

        new public string Text
        {
            get
            {
                return (base.Text);
            }
            set
            {
                _currentString = base.Text;
                base.Text = value;
                // fire EventArgs if changed
                if (MIDComboBoxPropertiesChangedEvent != null &&
                    _currentString != value)
                {
                    MIDComboBoxPropertiesChangedEvent(this, new MIDComboBoxPropertiesChangedEventArgs());
                }
            }
        }

        public void Initialize(SessionAddressBlock aSAB, FunctionSecurityProfile aFunctionSecurity, ArrayList aArrayList,
            bool aAllowUserAttributes)
        {
            _initializing = true;
            base.Initialize(aSAB, aFunctionSecurity);
            ValueMember = "Key";
            DisplayMember = "Name";
            DataSource = aArrayList;
            _allowUserAttributes = aAllowUserAttributes;
            _initializing = false;
        }

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            _correctedSelectedIndex = SelectedIndex; 
            base.OnSelectedIndexChanged(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (SelectedIndex != _correctedSelectedIndex)
            {
                SelectedIndex = _correctedSelectedIndex;
            }
            base.OnLostFocus(e);
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            if (SelectedIndex != _correctedSelectedIndex)
            {
                SelectedIndex = _correctedSelectedIndex;
            }
            base.OnDropDownClosed(e);
        }

        //private void MIDAttributeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        private void MIDAttributeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            StoreGroupProfile sgp;
            StoreGroupListViewProfile sglvp;
            ArrayList al;
            try
            {
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                _correctedSelectedIndex = SelectedIndex;
                // End TT#301-MD - JSmith - Controls are not functioning properly

                if (!_initializing)
                {
                    // if the value is not in the list, let the user know
                    if (base.SelectedIndex == Include.Undefined &&
                        _setValue != null &&
                        DataSource != null) // the dispose clears the DataSource by setting it to null
                    {
                        string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AttributeNotAccessibleWarning);
                        sgp = StoreMgmt.StoreGroup_Get(Convert.ToInt32(_setValue)); //SAB.StoreServerSession.GetStoreGroup(Convert.ToInt32(_setValue));
                        message = message.Replace("{0}", sgp.GroupId);

                        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                        //if (_securityAdmin == null)
                        //{
                        //   _securityAdmin = new SecurityAdmin();
                        //}
                        //message = message.Replace("{1}", _securityAdmin.GetUserName(sgp.OwnerUserRID));
                        message = message.Replace("{1}", UserNameStorage.GetUserName(sgp.OwnerUserRID));
                        //End TT#827-MD -jsobek -Allocation Reviews Performance

                        if (MessageBox.Show(message, " ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                            == DialogResult.No)
                        {
                            _initializing = true;
                            // clear datasource and add only obsolete item for readonly display
                            DataSource = null;
                            sglvp = new StoreGroupListViewProfile(sgp.Key);
                            sglvp.Name = sgp.GroupId;
                            sglvp.OwnerUserRID = sgp.OwnerUserRID;
                            sglvp.IsDynamicGroup = sgp.IsDynamicGroup;
                            sglvp.FilterRID = sgp.FilterRID;
                            sglvp.Version = sgp.Version;
                            al = new ArrayList();
                            al.Add(sglvp);
                            DataSource = al;
                            SelectedValue = sgp.Key;
                            _continueReadOnly = true;
                            _replaceAttribute = false;
                            _initializing = false;
                        }
                        else
                        {
                            _initializing = true;
                            SelectedIndex = Include.Undefined;
                            SelectedValue = "";
                            _replaceAttribute = true;
                            _initializing = false;
                        }
                        _setValue = null;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void MIDAttributeComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void MIDAttributeComboBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
            {
                TreeNodeClipboardList cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                //if (cbp.ClipboardDataType == eClipboardDataType.Attribute)
                // Begin TT#30 - JSmith - Drag and Drop an attribute from the Store Explorer into a method or view does not work
                //if (cbList.ClipboardDataType == eProfileType.StoreGroupListView)
                if (cbList.ClipboardDataType == eProfileType.StoreGroup)
                // End TT#30 - JSmith
                {
                    if (!_allowUserAttributes &&
                            cbList.ClipboardProfile.OwnerUserRID != Include.GlobalUserRID)
                        {
                            e.Effect = DragDropEffects.None;
                        }
                        else if (FunctionSecurity.AllowUpdate ||
                        FunctionSecurity.AllowView)
                    {
                        SelectedValue = cbList.ClipboardProfile.Key;
                        Invalidate();
                    }
                }
            }
        }

        private void MIDAttributeComboBox_DragEnter(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList;

            try
            {
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //if (cbp.ClipboardDataType == eClipboardDataType.Attribute)
                    // Begin TT#30 - JSmith - Drag and Drop an attribute from the Store Explorer into a method or view does not work
                    //if (cbList.ClipboardDataType == eProfileType.StoreGroupListView)
                    if (cbList.ClipboardDataType == eProfileType.StoreGroup)
                    // End TT#30
                    {
                        if (!_allowUserAttributes &&
                            cbList.ClipboardProfile.OwnerUserRID != Include.GlobalUserRID)
                        {
                            e.Effect = DragDropEffects.None;
                        }
                        else if (FunctionSecurity.AllowUpdate ||
                            FunctionSecurity.AllowView)
                        {
                            e.Effect = DragDropEffects.All;
                        }
                        else
                        {
                            e.Effect = DragDropEffects.None;
                        }
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //BEGIN TT#306-MD-VStuart-Version 5.0-Size Review not working correctly.
        private void MIDAttributeComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Here we check for the Tab key and when true set the text
            // property and send the Enter Key or the Tab Key if NOT dropped down.
            if ((e.KeyCode == Keys.Tab) && (this.DroppedDown == true))
            {
                try
                {
                    if (this.SelectedItem.ToString() != null)
                    {
                        this.Text = this.SelectedItem.ToString();
                        SendKeys.Send("{Enter}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0} Caught exception, a comboBox is NULL.", ex);
                }
            }
        }
        //END TT#306-MD-VStuart-Version 5.0-Size Review not working correctly.
    }
}
