using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Data;
using MIDRetail.Data;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    /// <summary>
    /// Used to save and retrieve information from the tag.
    /// </summary>
    public abstract class MIDTag
    {
        #region Fields

        private SessionAddressBlock _SAB;
        private object _MIDTagData;

        #endregion Fields

        #region Constructors
        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// 

        public MIDTag()
        {
            _SAB = null;
            _MIDTagData = null;
        }

        public MIDTag(SessionAddressBlock aSAB)
        {
            _SAB = aSAB;
            _MIDTagData = null;
        }

        #endregion Constructors
        
        #region Properties
		public SessionAddressBlock SAB
		{
            get { return _SAB; }
            set { _SAB = value; }
        }

        /// <summary>
        /// Gets or sets the object containing the data for the control.
        /// </summary>
        public object MIDTagData
        {
            get { return _MIDTagData; }
            set { _MIDTagData = value; }
        }

        #endregion Properties

        #region Methods
        #endregion Methods
    }


    /// <summary>
    /// Used to save and retrieve information from the tagControl.
    /// </summary>
	// Begin TT#3513 - JSmith - Clean Up Memory Leaks
	//public abstract class MIDControlTag : MIDTag
    public abstract class MIDControlTag : MIDTag, IDisposable
	// End TT#3513 - JSmith - Clean Up Memory Leaks
    {
        #region Fields
        private System.Windows.Forms.Control _control;
        private eMIDControlCode _parentFormID;
        private eMIDControlCode _controlID;
        private eSecurityTypes _securityTypes;
        private eSecuritySelectType _securityActions;
        private string _message;
        private string _helpText;
        private int _key;
        private object _priorValue;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private bool _ignoreWheelMouse;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        public MIDControlTag()
        {
            _control = null;
            _parentFormID = eMIDControlCode.Unassigned;
            _controlID = eMIDControlCode.Unassigned;
            _securityTypes = eSecurityTypes.None;
            _ignoreWheelMouse = false;
            _securityActions = eSecuritySelectType.None;
            ConstructorCommon();
        }

        public MIDControlTag(SessionAddressBlock aSAB, Control aControl, eMIDControlCode aControlID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions)
            : base(aSAB)
        {
            _control = aControl;
            _parentFormID = eMIDControlCode.Unassigned ;
            _controlID = aControlID;
            _securityTypes = aSecurityTypes;
            _ignoreWheelMouse = false;
            _securityActions = aSecurityActions;
            ConstructorCommon();
        }

        public MIDControlTag(SessionAddressBlock aSAB, Control aControl, eMIDControlCode aParentFormID, eMIDControlCode aControlID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions)
            : base(aSAB)
        {
            _control = aControl;
            _parentFormID = aParentFormID;
            _controlID = aControlID;
            _securityTypes = aSecurityTypes;
            _ignoreWheelMouse = false;
            _securityActions = aSecurityActions;
            ConstructorCommon();
        }

        public MIDControlTag(SessionAddressBlock aSAB, Control aControl, eMIDControlCode aParentFormID, eMIDControlCode aControlID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions, bool aIgnoreWheelMouse)
            : base(aSAB)
        {
            _control = aControl;
            _parentFormID = aParentFormID;
            _controlID = aControlID;
            _securityTypes = aSecurityTypes;
            IgnoreWheelMouse = aIgnoreWheelMouse;
            _securityActions = aSecurityActions;
            ConstructorCommon();
        }

        private void ConstructorCommon()
        {
            _message = null;
            _helpText = null;
            _errorProvider = new ErrorProvider();
            _key = Include.NoRID;
            _priorValue = null;
        }

        #endregion Constructors

        // Begin TT#3513 - JSmith - Clean Up Memory Leaks
        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _control = null;
                _priorValue = null;
                _errorProvider = null;
            }
        }

        ~MIDControlTag()
        {
            Dispose(true);
        }

        #endregion IDisposable Members
		// End TT#3513 - JSmith - Clean Up Memory Leaks

        #region Properties
        /// <summary>
        /// Gets or sets the ErrorProvider.
        /// </summary>
        public ErrorProvider ErrorProvider
        {
            get { return _errorProvider; }
            set { _errorProvider = value; }
        }

        /// <summary>
        /// Gets or sets the Key.
        /// </summary>
        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// Gets or sets the eMIDControlCode of the parent form of the control.
        /// </summary>
        public eMIDControlCode ParentFormID
        {
            get { return _parentFormID; }
            set { _parentFormID = value; }
        }

        /// <summary>
        /// Gets or sets the eMIDControlCodeID of the control.
        /// </summary>
        public eMIDControlCode ControlID
        {
            get { return _controlID; }
            set { _controlID = value; }
        }

        /// <summary>
        /// Gets or sets the eMIDControlCode of the control.
        /// </summary>
        public Control MIDControl
        {
            get { return _control; }
            set { _control = value; }
        }

        /// <summary>
        /// Gets or sets the message associated with the cell.
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// Gets or sets the help text associated with the cell.
        /// </summary>
        public string HelpText
        {
            get { return _helpText; }
            set { _helpText = value; }
        }

        /// <summary>
        /// Gets or sets the type of security for the control.
        /// </summary>
        public eSecurityTypes SecurityTypes
        {
            get { return _securityTypes; }
            set
            {
                if (value != _securityTypes)
                {
                    _key = Include.NoRID;
                }
                _securityTypes = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of security for the control.
        /// </summary>
        public eSecuritySelectType SecurityActions
        {
            get { return _securityActions; }
            set { _securityActions = value; }
        }

        /// <summary>
        /// Gets or sets the object containing the data for the control.
        /// </summary>
        public object PriorValue
        {
            get { return _priorValue; }
            set { _priorValue = value; }
        }

        /// <summary>
        /// Gets or sets the flag identifying if the wheel mouse is to be ignored for the control.
        /// </summary>
        public bool IgnoreWheelMouse
        {
            get { return _ignoreWheelMouse; }
            set { _ignoreWheelMouse = value; }
        }

        #endregion Properties

        #region Methods
        abstract public bool IsAuthorized(eSecurityTypes aSecuritypes, eSecuritySelectType aSecuritySelectType);
        #endregion Methods
    }




    /// <summary>
    /// Used to save and retrieve TextBox information from the tagControl.
    /// </summary>
    abstract public class MIDTextBoxTag : MIDControlTag
    {
        #region Fields
        private bool _textChanged = false;
        private bool _priorError = false;
        private SecurityProfile _securityProfile;
        private bool _processingDragDrop = false;  // TT#2900 - JSmith - Style Color Forecast Method Issue
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        public MIDTextBoxTag()
            : base()
        {
        }

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock.</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aControlID">The eMIDControlCode of the control.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        public MIDTextBoxTag(SessionAddressBlock aSAB, TextBox aControl, eMIDControlCode aControlID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions)
            : base(aSAB, aControl, aControlID, aSecurityTypes, aSecurityActions)
        {
        }

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aParentFormID">The eMIDControlCode of the parent form.</param>
        /// <param name="aControlID">The eMIDControlCode of the control.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        public MIDTextBoxTag(SessionAddressBlock aSAB, TextBox aControl, eMIDControlCode aParentFormID, eMIDControlCode aControlID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions)
            : base(aSAB, aControl, aParentFormID, aControlID, aSecurityTypes, aSecurityActions)
        {
        }

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aParentFormID">The eMIDControlCode of the parent form.</param>
        /// <param name="aControlID">The eMIDControlCode of the control.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        /// <param name="aIgnoreWheelMouse">The flag identifying if the wheel mouse is to be ignored for the control</param>
        public MIDTextBoxTag(SessionAddressBlock aSAB, TextBox aControl, eMIDControlCode aParentFormID, eMIDControlCode aControlID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions, bool aIgnoreWheelMouse)
            : base(aSAB, aControl, aParentFormID, aControlID, aSecurityTypes, aSecurityActions, aIgnoreWheelMouse)
        {
        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets or sets the TextChanged.
        /// </summary>
        public bool TextChanged
        {
            get { return _textChanged; }
            set { _textChanged = value; }
        }

        /// <summary>
        /// Gets or sets the PriorError.
        /// </summary>
        public bool PriorError
        {
            get { return _priorError; }
            set { _priorError = value; }
        }

        /// <summary>
        /// Gets or sets the SecurityProfile.
        /// </summary>
        public SecurityProfile SecurityProfile
        {
            get { return _securityProfile; }
            set { _securityProfile = value; }
        }

        // Begin TT#2900 - JSmith - Style Color Forecast Method Issue
        /// <summary>
        /// Gets or sets the PriorError.
        /// </summary>
        public bool ProcessingDragDrop
        {
            get { return _processingDragDrop; }
            set { _processingDragDrop = value; }
        }
        // End TT#2900 - JSmith - Style Color Forecast Method Issue

        #endregion Properties

        #region Methods

        override public bool IsAuthorized(eSecurityTypes aSecuritypes, eSecuritySelectType aSecuritySelectType)
        {
            return false;
        }

        private HierarchyNodeProfile GetNodeProfile(string aProductID)
        {
            string desc = string.Empty;
            try
            {
                string productID = aProductID.Trim();
                string[] pArray = productID.Split(new char[] { '[' });
                productID = pArray[0].Trim();
                HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                EditMsgs em = new EditMsgs();
                return hm.NodeLookup(ref em, productID, false);
            }
            catch
            {
                throw;
            }
        }

        bool ApplySecurity()
        {
            bool securityOk = true;
            if (_securityProfile.AllowUpdate)
            {
            }
            else
            {
            }

            if (_securityProfile.AllowExecute)
            {
            }
            else
            {
            }

            return securityOk;
        }

        #endregion Methods

        #region Event Handlers

        abstract public bool TextBox_DragDrop(object sender, DragEventArgs e);

        abstract protected void TextBox_KeyDown(object sender, KeyEventArgs e);

        // Begin TT#351-MD - JSmith - Object reference error when keying Merchandise
        abstract protected void TextBox_TextChanged(object sender, EventArgs e);
        // End TT#351-MD - JSmith - Object reference error when keying Merchandise

        abstract protected void TextBox_Enter(object sender, EventArgs e);

        abstract protected void TextBox_DragEnter(object sender, DragEventArgs e);

        abstract protected void TextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e);

        abstract protected void TextBox_Validated(object sender, EventArgs e);

        #endregion Event Handlers
    }



    /// <summary>
    /// Used to save and retrieve Combo information from the tagControl.
    /// </summary>
    abstract public class MIDComboBoxTag : MIDControlTag
    {
        #region Fields
        private bool _textChanged = false;
        private bool _priorError = false;
        private SecurityProfile _securityProfile;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        public MIDComboBoxTag()
            : base()
        {
        }

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock.</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aControlID">The eMIDControlCode of the control.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        public MIDComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eMIDControlCode aControlID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions)
            : base(aSAB, aControl, aControlID, aSecurityTypes, aSecurityActions)
        {
        }

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aParentFormID">The eMIDControlCode of the parent form.</param>
        /// <param name="aControlID">The eMIDControlCode of the control.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        public MIDComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eMIDControlCode aParentFormID, eMIDControlCode aControlID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions)
            : base(aSAB, aControl, aParentFormID, aControlID, aSecurityTypes, aSecurityActions)
        {
        }

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aParentFormID">The eMIDControlCode of the parent form.</param>
        /// <param name="aControlID">The eMIDControlCode of the control.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        /// <param name="aIgnoreWheelMouse">The flag identifying if the wheel mouse is to be ignored for the control</param>
        public MIDComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eMIDControlCode aParentFormID, eMIDControlCode aControlID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions, bool aIgnoreWheelMouse)
            : base(aSAB, aControl, aParentFormID, aControlID, aSecurityTypes, aSecurityActions, aIgnoreWheelMouse)
        {
        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets or sets the TextChanged.
        /// </summary>
        public bool TextChanged
        {
            get { return _textChanged; }
            set { _textChanged = value; }
        }

        /// <summary>
        /// Gets or sets the PriorError.
        /// </summary>
        public bool PriorError
        {
            get { return _priorError; }
            set { _priorError = value; }
        }

        /// <summary>
        /// Gets or sets the SecurityProfile.
        /// </summary>
        public SecurityProfile SecurityProfile
        {
            get { return _securityProfile; }
            set { _securityProfile = value; }
        }

        #endregion Properties

        #region Methods
        override public bool IsAuthorized(eSecurityTypes aSecuritypes, eSecuritySelectType aSecuritySelectType)
        {
            return false;
        }

        private HierarchyNodeProfile GetNodeProfile(string aProductID)
        {
            string desc = string.Empty;
            try
            {
                string productID = aProductID.Trim();
                string[] pArray = productID.Split(new char[] { '[' });
                productID = pArray[0].Trim();
                HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                EditMsgs em = new EditMsgs();
                return hm.NodeLookup(ref em, productID, false);
            }
            catch
            {
                throw;
            }
        }

        bool ApplySecurity()
        {
            bool securityOk = true;
            if (_securityProfile.AllowUpdate)
            {
            }
            else
            {
            }

            if (_securityProfile.AllowExecute)
            {
            }
            else
            {
            }

            return securityOk;
        }

        #endregion Methods

        #region Event Handlers

        abstract protected void ComboBox_DragEnter(object sender, DragEventArgs e);

        abstract public bool ComboBox_DragDrop(object sender, DragEventArgs e);

        #endregion Event Handlers
    }




    /// <summary>
    /// Used to save and retrieve TextBox information from the tagControl.
    /// </summary>
    public class MIDMerchandiseTextBoxTag : MIDTextBoxTag , IDisposable
    {
        #region Fields
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        public MIDMerchandiseTextBoxTag()
            : base()
        {
        }

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock.</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        public MIDMerchandiseTextBoxTag(SessionAddressBlock aSAB, TextBox aControl, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions)
            : base(aSAB, aControl, eMIDControlCode.field_Merchandise, aSecurityTypes, aSecurityActions)
        {
            AddEventHandlers();
        }

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aParentFormID">The eMIDControlCode of the parent form.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        public MIDMerchandiseTextBoxTag(SessionAddressBlock aSAB, TextBox aControl, eMIDControlCode aParentFormID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions)
            : base(aSAB, aControl, aParentFormID, eMIDControlCode.field_Merchandise, aSecurityTypes, aSecurityActions)
        {
            AddEventHandlers();
        }

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aParentFormID">The eMIDControlCode of the parent form.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        /// <param name="aIgnoreWheelMouse">The flag identifying if the wheel mouse is to be ignored for the control</param>
        public MIDMerchandiseTextBoxTag(SessionAddressBlock aSAB, TextBox aControl, eMIDControlCode aParentFormID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions, bool aIgnoreWheelMouse)
            : base(aSAB, aControl, aParentFormID, eMIDControlCode.field_Merchandise, aSecurityTypes, aSecurityActions, aIgnoreWheelMouse)
        {
            AddEventHandlers();
        }

        #endregion Constructors

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveEventHandlers();
            }
        }

        ~MIDMerchandiseTextBoxTag()
        {
            Dispose(true);
        }

        #endregion IDisposable Members

        #region Properties
        #endregion Properties

        #region Methods

        override public bool IsAuthorized(eSecurityTypes aSecurityTypes, eSecuritySelectType aSecuritySelectType)
        {
            bool validNode = false;	
            int nodeRid = Include.NoRID;
            string errorMessage = string.Empty;
            string item = string.Empty;
            HierarchyNodeProfile hnp;

            if (MIDControl.Tag != null)
            {
                if (MIDControl.Tag.GetType() == typeof(HierarchyNodeProfile))
                {
                    hnp = (HierarchyNodeProfile)MIDControl.Tag;
                    nodeRid = hnp.Key;
                }
                else if (MIDControl.Tag.GetType() == typeof(MIDMerchandiseTextBoxTag))
                {
                    if (((MIDTag)MIDControl.Tag).MIDTagData != null)
                    {
                        hnp = (HierarchyNodeProfile)((MIDTag)MIDControl.Tag).MIDTagData;
                        nodeRid = hnp.Key;
                    }
                }

                if (nodeRid != Include.NoRID)
                {
                    validNode = true;
                    HierarchyNodeSecurityProfile nodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(nodeRid, (int)aSecurityTypes);
                    if (nodeSecurity.AccessDenied)
                    {
                        validNode = false;
                        if (System.Convert.ToBoolean(aSecuritySelectType & eSecuritySelectType.View))
                        {
                            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode);
                        }
                        else
                        {
                            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
                            item = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
                            errorMessage = errorMessage + item + ".";
                        }
                        ErrorProvider.SetError(MIDControl, errorMessage);
                    }
                    else if (nodeSecurity.IsReadOnly)
                    {
                        if (System.Convert.ToBoolean(aSecuritySelectType & eSecuritySelectType.Update))
                        {
                            validNode = false;
                            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
                            item = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
                            errorMessage = errorMessage + item + ".";
                            ErrorProvider.SetError(MIDControl, errorMessage);
                        }
                    }
                }
            }
            if (validNode)
                ErrorProvider.SetError(MIDControl, string.Empty);

            return validNode;
        }

        private void AddEventHandlers()
        {
            if (MIDControl != null)
            {
               AddEventHandlersForTextBox();
            }
        }

        private void AddEventHandlersForTextBox()
        {
            switch (ControlID)
            {
                case eMIDControlCode.field_Merchandise:
                  //((System.Windows.Forms.TextBox)MIDControl).DragDrop += new DragEventHandler(TextBox_DragDrop);
                    ((System.Windows.Forms.TextBox)MIDControl).DragEnter += new DragEventHandler(TextBox_DragEnter);
                    ((System.Windows.Forms.TextBox)MIDControl).Validating += new System.ComponentModel.CancelEventHandler(TextBox_Validating);
                    ((System.Windows.Forms.TextBox)MIDControl).Validated += new EventHandler(TextBox_Validated);
                    ((System.Windows.Forms.TextBox)MIDControl).Enter += new EventHandler(TextBox_Enter);
                    ((System.Windows.Forms.TextBox)MIDControl).KeyDown += new KeyEventHandler(TextBox_KeyDown);
                    // Begin TT#351-MD - JSmith - Object reference error when keying Merchandise
                    ((System.Windows.Forms.TextBox)MIDControl).TextChanged += new EventHandler(TextBox_TextChanged);
                    // End TT#351-MD - JSmith - Object reference error when keying Merchandise
                    break;
            }
        }

        private void RemoveEventHandlers()
        {
            if (MIDControl != null)
            {
               RemoveEventHandlersForTextBox();
            }
        }

        private void RemoveEventHandlersForTextBox()
        {
            switch (ControlID)
            {
                case eMIDControlCode.field_Merchandise:
                  //((System.Windows.Forms.TextBox)MIDControl).DragDrop -= new DragEventHandler(TextBox_DragDrop);
                    ((System.Windows.Forms.TextBox)MIDControl).DragEnter -= new DragEventHandler(TextBox_DragEnter);
                    ((System.Windows.Forms.TextBox)MIDControl).Validating -= new System.ComponentModel.CancelEventHandler(TextBox_Validating);
                    // Begin TT#856 - JSmith - Out of memory
                    ((System.Windows.Forms.TextBox)MIDControl).Validated -= new EventHandler(TextBox_Validated);
                    // End TT#856
                    ((System.Windows.Forms.TextBox)MIDControl).Enter -= new EventHandler(TextBox_Enter);
                    ((System.Windows.Forms.TextBox)MIDControl).KeyDown -= new KeyEventHandler(TextBox_KeyDown);
                    // Begin TT#351-MD - JSmith - Object reference error when keying Merchandise
                    ((System.Windows.Forms.TextBox)MIDControl).TextChanged -= new EventHandler(TextBox_TextChanged);
                    // End TT#351-MD - JSmith - Object reference error when keying Merchandise
                    break;
            }
        }

        private HierarchyNodeProfile GetNodeProfile(string aProductID)
        {
            string desc = string.Empty;
            try
            {
                string productID = aProductID.Trim();
                string[] pArray = productID.Split(new char[] { '[' });
                productID = pArray[0].Trim();
                HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                EditMsgs em = new EditMsgs();
                return hm.NodeLookup(ref em, productID, false);
            }
            catch
            {
                throw;
            }
        }

        bool ApplySecurity()
        {
            bool securityOk = true;
            if (SecurityProfile.AllowUpdate)
            {
            }
            else
            {
            }

            if (SecurityProfile.AllowExecute)
            {
            }
            else
            {
            }

            return securityOk;
        }

        #endregion Methods

        #region Event Handlers

        override protected void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
			// Begin TT#729 - stodd - style color name changing back to style name
            if (((MIDTextBoxTag)(((TextBox)sender).Tag)).PriorValue != null &&
                ((TextBox)sender).Text != ((MIDTextBoxTag)(((TextBox)sender).Tag)).PriorValue.ToString())
			{
				TextChanged = true;
			}
			// End TT#729
            ErrorProvider.SetError((TextBox)sender, string.Empty);
        }

        // Begin TT#351-MD - JSmith - Object reference error when keying Merchandise
        override protected void TextBox_TextChanged(object sender, EventArgs e)
        {
            // Begin TT#2900 - JSmith - Style Color Forecast Method Issue
            if (ProcessingDragDrop)
            {
                return;
            }
            // End TT#2900 - JSmith - Style Color Forecast Method Issue

            if (((MIDTextBoxTag)(((TextBox)sender).Tag)).PriorValue != null &&
                ((TextBox)sender).Text != ((MIDTextBoxTag)(((TextBox)sender).Tag)).PriorValue.ToString())
            {
                TextChanged = true;
            }
        }
        // End TT#351-MD - JSmith - Object reference error when keying Merchandise

        override protected void TextBox_Enter(object sender, EventArgs e)
        {
            PriorValue = ((TextBox)sender).Text;
        }

        override public bool TextBox_DragDrop(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList;
            TextBox txtMerchandise;
            string errorMessage;

            // Begin TT#2900 - JSmith - Style Color Forecast Method Issue
            ProcessingDragDrop = true;
            // End TT#2900 - JSmith - Style Color Forecast Method Issue

            ErrorProvider.SetError((TextBox)sender, string.Empty);

            txtMerchandise = (TextBox)sender;
            if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
            {
                cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                {
                    //if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
                    //{
                        if (SecurityProfile == null ||
                            cbList.ClipboardProfile.Key != Key)
                        {
                            SecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(cbList.ClipboardProfile.Key, (int)SecurityTypes);
                            Key = cbList.ClipboardProfile.Key;
                        }

                        if (SecurityProfile.AccessDenied)
                        {
                            errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode),
                            txtMerchandise.Text);
                            ErrorProvider.SetError(txtMerchandise, errorMessage);
                        }
                        else
                        {
                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                            txtMerchandise.Text = hnp.Text;
                            ((MIDTag)txtMerchandise.Tag).MIDTagData = hnp;
                            return true;
                        }
                    //}
                }
            }
            // Begin TT#2900 - JSmith - Style Color Forecast Method Issue
            ProcessingDragDrop = false;
            // End TT#2900 - JSmith - Style Color Forecast Method Issue
            return false;
        }

        override protected void TextBox_DragEnter(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList;
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
            {
                cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                {
                    if (SecurityProfile == null ||
                        cbList.ClipboardProfile.Key != Key)
                    {
                        SecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(cbList.ClipboardProfile.Key, (int)SecurityTypes);
                        Key = cbList.ClipboardProfile.Key;
                    }

                    if (!SecurityProfile.AccessDenied)
                    {
                        e.Effect = DragDropEffects.All;
                    }
                }
            }
        }

        override protected void TextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string errorMessage;
            HierarchyNodeProfile hnp;
            TextBox txtMerchandise;

            ErrorProvider.SetError((TextBox)sender, string.Empty);

            txtMerchandise = (TextBox)sender;
            if ((txtMerchandise.Text.Trim() == string.Empty) && (txtMerchandise.Tag != null))
            {
                txtMerchandise.Text = string.Empty;
                ((MIDTag)txtMerchandise.Tag).MIDTagData = null;
            }
            else
            {
                if (TextChanged)
                {
                    TextChanged = false;
                    hnp = GetNodeProfile(txtMerchandise.Text);
                    if (hnp.Key == Include.NoRID)
                    {
                        PriorError = true;

                        errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
                            txtMerchandise.Text);
                        ErrorProvider.SetError(txtMerchandise, errorMessage);
                      //MessageBox.Show(errorMessage);
                        if (PriorValue != null)
                        {
                            txtMerchandise.Text = Convert.ToString(PriorValue, CultureInfo.CurrentUICulture);
                        }

                        e.Cancel = true;
                    }
                    else
                    {
                        SecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(hnp.Key, (int)SecurityTypes);

                        if (SecurityProfile.AccessDenied ||
                            ((System.Convert.ToBoolean(SecurityActions & eSecuritySelectType.View) && !SecurityProfile.AllowView) &&
                            (System.Convert.ToBoolean(SecurityActions & eSecuritySelectType.Update) && !SecurityProfile.AllowUpdate) &&
                            (System.Convert.ToBoolean(SecurityActions & eSecuritySelectType.Delete) && !SecurityProfile.AllowDelete)))
                        {
                            errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode),
                            txtMerchandise.Text);
                            ErrorProvider.SetError(txtMerchandise, errorMessage);
                            MessageBox.Show(errorMessage);
                            if (PriorValue != null)
                            {
                                txtMerchandise.Text = Convert.ToString(PriorValue, CultureInfo.CurrentUICulture);
                            }

                            e.Cancel = true;
                        }
                        else 
                        {
                            txtMerchandise.Text = hnp.Text;
                            ((MIDTag)txtMerchandise.Tag).MIDTagData = hnp;
                        }
                    }
                }
            }
        }

        override protected void TextBox_Validated(object sender, EventArgs e)
        {
            TextChanged = false;
            PriorError = false;
            ErrorProvider.SetError((TextBox)sender, string.Empty);
        }
        #endregion Event Handlers
    }




    /// <summary>
    /// Used to save and retrieve Combo information from the tagControl.
    /// </summary>
    public class MIDVersionComboBoxTag : MIDComboBoxTag , IDisposable
    {
        #region Fields
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        public MIDVersionComboBoxTag(): base()
        {
        }

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock.</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        public MIDVersionComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions)
            : base(aSAB, aControl, eMIDControlCode.field_Version, aSecurityTypes, aSecurityActions)
        {
            AddEventHandlers();
        }

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aParentFormID">The eMIDControlCode of the parent form.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        public MIDVersionComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eMIDControlCode aParentFormID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions)
            : base(aSAB, aControl, aParentFormID, eMIDControlCode.field_Version, aSecurityTypes, aSecurityActions)
        {
            AddEventHandlers();
        }

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aParentFormID">The eMIDControlCode of the parent form.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        /// <param name="aIgnoreWheelMouse">The flag identifying if the wheel mouse is to be ignored for the control</param>
        public MIDVersionComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eMIDControlCode aParentFormID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions, bool aIgnoreWheelMouse)
            : base(aSAB, aControl, aParentFormID, eMIDControlCode.field_Version, aSecurityTypes, aSecurityActions, aIgnoreWheelMouse)
        {
            AddEventHandlers();
        }

        #endregion Constructors

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveEventHandlers();
            }
        }

        ~MIDVersionComboBoxTag()
        {
            Dispose(true);
        }

        #endregion IDisposable Members

        #region Properties
        #endregion Properties

        #region Methods
        override public bool IsAuthorized(eSecurityTypes aSecurityTypes, eSecuritySelectType aSecuritySelectType)
        {
            bool validNode = false;
            int nodeRid = Include.NoRID;
            string errorMessage = string.Empty;
            string item = string.Empty;
            HierarchyNodeProfile hnp;

            if (MIDControl.Tag != null)
            {
                if (MIDControl.Tag.GetType() == typeof(HierarchyNodeProfile))
                {
                    hnp = (HierarchyNodeProfile)MIDControl.Tag;
                    nodeRid = hnp.Key;
                }
                else if (MIDControl.Tag.GetType() == typeof(MIDMerchandiseTextBoxTag))
                {
                    if (((MIDTag)MIDControl.Tag).MIDTagData != null)
                    {
                        hnp = (HierarchyNodeProfile)((MIDTag)MIDControl.Tag).MIDTagData;
                        nodeRid = hnp.Key;
                    }
                }

                if (nodeRid != Include.NoRID)
                {
                    validNode = true;
                    HierarchyNodeSecurityProfile nodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(nodeRid, (int)aSecurityTypes);
                    if (nodeSecurity.AccessDenied)
                    {
                        validNode = false;
                        if (System.Convert.ToBoolean(aSecuritySelectType & eSecuritySelectType.View))
                        {
                            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode);
                        }
                        else
                        {
                            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
                            item = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);
                            errorMessage = errorMessage + item + ".";
                        }
                        ErrorProvider.SetError(MIDControl, errorMessage);
                    }
                    else if (nodeSecurity.IsReadOnly)
                    {
                        if (System.Convert.ToBoolean(aSecuritySelectType & eSecuritySelectType.Update))
                        {
                            validNode = false;
                            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
                            item = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);
                            errorMessage = errorMessage + item + ".";
                            ErrorProvider.SetError(MIDControl, errorMessage);
                        }
                    }
                }
            }
            if (validNode)
                ErrorProvider.SetError(MIDControl, string.Empty);

            return validNode;
        }

        private void AddEventHandlers()
        {
            if (MIDControl != null)
            {
                AddEventHandlersForComboBox();
            }
        }

        private void AddEventHandlersForComboBox()
        {
            switch (ControlID)
            {
                case eMIDControlCode.field_Version:
                  //((System.Windows.Forms.ComboBox)MIDControl).DragDrop += new DragEventHandler(ComboBox_DragDrop);
                    // Begin TT#3513 - JSmith - Clean Up Memory Leaks
                    //((System.Windows.Forms.ComboBox)MIDControl).DragEnter += new DragEventHandler(ComboBox_DragEnter);
                    if (MIDControl is MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)
                    {
                        ((MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)MIDControl).DragEnter += new DragEventHandler(ComboBox_DragEnter);
                    }
                    else if (MIDControl is System.Windows.Forms.ComboBox)
                    {
                        ((System.Windows.Forms.ComboBox)MIDControl).DragEnter += new DragEventHandler(ComboBox_DragEnter);
                    }
                    // End TT#3513 - JSmith - Clean Up Memory Leaks
                    break;
            }
        }

        private void RemoveEventHandlers()
        {
            if (MIDControl != null)
            {
                RemoveEventHandlersForComboBox();
            }
        }

        private void RemoveEventHandlersForComboBox()
        {
            switch (ControlID)
            {
                case eMIDControlCode.field_Version:
                  //((System.Windows.Forms.ComboBox)MIDControl).DragDrop -= new DragEventHandler(ComboBox_DragDrop);
                    // Begin TT#3513 - JSmith - Clean Up Memory Leaks
                    //((System.Windows.Forms.ComboBox)MIDControl).DragEnter -= new DragEventHandler(ComboBox_DragEnter);
                    if (MIDControl is MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)
                    {
                        ((MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)MIDControl).DragEnter -= new DragEventHandler(ComboBox_DragEnter);
                    }
                    else if (MIDControl is System.Windows.Forms.ComboBox)
                    {
                        ((System.Windows.Forms.ComboBox)MIDControl).DragEnter -= new DragEventHandler(ComboBox_DragEnter);
                    }
                    // End TT#3513 - JSmith - Clean Up Memory Leaks
                    break;
            }
        }

        private HierarchyNodeProfile GetNodeProfile(string aProductID)
        {
            string desc = string.Empty;
            try
            {
                string productID = aProductID.Trim();
                string[] pArray = productID.Split(new char[] { '[' });
                productID = pArray[0].Trim();
                HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                EditMsgs em = new EditMsgs();
                return hm.NodeLookup(ref em, productID, false);
            }
            catch
            {
                throw;
            }
        }

        bool ApplySecurity()
        {
            bool securityOk = true;
            if (SecurityProfile.AllowUpdate)
            {
            }
            else
            {
            }

            if (SecurityProfile.AllowExecute)
            {
            }
            else
            {
            }

            return securityOk;
        }

        #endregion Methods

        #region Event Handlers

        override protected void ComboBox_DragEnter(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
            e.Effect = DragDropEffects.None;

            if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
            {
                cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                if (cbList.ClipboardDataType == eProfileType.FilterStore)
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

        override public bool ComboBox_DragDrop(object sender, DragEventArgs e)
        {
            return false;
        }

        #endregion Event Handlers
    }




    /// <summary>
    /// Used to save and retrieve Combo information from the tagControl.
    /// </summary>
    public class MIDStoreFilterComboBoxTag : MIDComboBoxTag, IDisposable
    {
        #region Fields
        // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
        private bool _allowUserFilters;
        // End TT#44
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
        //public MIDStoreFilterComboBoxTag()
        //    : base()
        //{
        //}
        public MIDStoreFilterComboBoxTag(SecurityProfile aSecurityProfile, bool aAllowUserFilters)
            : base()
        {
            SecurityProfile = aSecurityProfile;
            _allowUserFilters = aAllowUserFilters;
        }
        // End TT#44

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock.</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
        //public MIDStoreFilterComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions)
        //    : base(aSAB, aControl, eMIDControlCode.field_Filter, aSecurityTypes, aSecurityActions)
        //{
        //    AddEventHandlers();
        //}
        public MIDStoreFilterComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions, SecurityProfile aSecurityProfile, bool aAllowUserFilters)
            : base(aSAB, aControl, eMIDControlCode.field_Filter, aSecurityTypes, aSecurityActions)
        {
            SecurityProfile = aSecurityProfile;
            _allowUserFilters = aAllowUserFilters;
            AddEventHandlers();
        }
        // End TT#44

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aParentFormID">The eMIDControlCode of the parent form.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
        //public MIDStoreFilterComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eMIDControlCode aParentFormID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions)
        //    : base(aSAB, aControl, aParentFormID, eMIDControlCode.field_Filter, aSecurityTypes, aSecurityActions)
        //{
        //    AddEventHandlers();
        //}
        public MIDStoreFilterComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eMIDControlCode aParentFormID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions, SecurityProfile aSecurityProfile, bool aAllowUserFilters)
            : base(aSAB, aControl, aParentFormID, eMIDControlCode.field_Filter, aSecurityTypes, aSecurityActions)
        {
            SecurityProfile = aSecurityProfile;
            _allowUserFilters = aAllowUserFilters;
            AddEventHandlers();
        }
        // End TT#44

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aParentFormID">The eMIDControlCode of the parent form.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        /// <param name="aIgnoreWheelMouse">The flag identifying if the wheel mouse is to be ignored for the control</param>
        // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
        //public MIDStoreFilterComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eMIDControlCode aParentFormID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions, bool aIgnoreWheelMouse)
        //    : base(aSAB, aControl, aParentFormID, eMIDControlCode.field_Filter, aSecurityTypes, aSecurityActions, aIgnoreWheelMouse)
        //{
        //    AddEventHandlers();
        //}
        public MIDStoreFilterComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eMIDControlCode aParentFormID, eSecurityTypes aSecurityTypes, eSecuritySelectType aSecurityActions, bool aIgnoreWheelMouse, SecurityProfile aSecurityProfile, bool aAllowUserFilters)
            : base(aSAB, aControl, aParentFormID, eMIDControlCode.field_Filter, aSecurityTypes, aSecurityActions, aIgnoreWheelMouse)
        {
            SecurityProfile = aSecurityProfile;
            _allowUserFilters = aAllowUserFilters;
            AddEventHandlers();
        }
        // End TT#44

        #endregion Constructors

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveEventHandlers();
            }
        }

        ~MIDStoreFilterComboBoxTag()
        {
            Dispose(true);
        }

        #endregion IDisposable Members

        #region Properties
        // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
        public bool AllowUserFilters
        {
            get { return _allowUserFilters; }
            set { _allowUserFilters = value; }
        }
        // End TT#44
        #endregion Properties

        #region Methods
        override public bool IsAuthorized(eSecurityTypes aSecurityTypes, eSecuritySelectType aSecuritySelectType)
        {
            bool validNode = false;
            int nodeRid = Include.NoRID;
            string errorMessage = string.Empty;
            string item = string.Empty;
            HierarchyNodeProfile hnp;

            if (MIDControl.Tag != null)
            {
                if (MIDControl.Tag.GetType() == typeof(HierarchyNodeProfile))
                {
                    hnp = (HierarchyNodeProfile)MIDControl.Tag;
                    nodeRid = hnp.Key;
                }
                else if (MIDControl.Tag.GetType() == typeof(MIDMerchandiseTextBoxTag))
                {
                    if (((MIDTag)MIDControl.Tag).MIDTagData != null)
                    {
                        hnp = (HierarchyNodeProfile)((MIDTag)MIDControl.Tag).MIDTagData;
                        nodeRid = hnp.Key;
                    }
                }

                if (nodeRid != Include.NoRID)
                {
                    validNode = true;
                    HierarchyNodeSecurityProfile nodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(nodeRid, (int)aSecurityTypes);
                    if (nodeSecurity.AccessDenied)
                    {
                        validNode = false;
                        if (System.Convert.ToBoolean(aSecuritySelectType & eSecuritySelectType.View))
                        {
                            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode);
                        }
                        else
                        {
                            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
                            item = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter);
                            errorMessage = errorMessage + item + ".";
                        }
                        ErrorProvider.SetError(MIDControl, errorMessage);
                    }
                    else if (nodeSecurity.IsReadOnly)
                    {
                        if (System.Convert.ToBoolean(aSecuritySelectType & eSecuritySelectType.Update))
                        {
                            validNode = false;
                            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
                            item = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter);
                            errorMessage = errorMessage + item + ".";
                            ErrorProvider.SetError(MIDControl, errorMessage);
                        }
                    }
                }
            }
            if (validNode)
                ErrorProvider.SetError(MIDControl, string.Empty);

            return validNode;
        }

        private void AddEventHandlers()
        {
            if (MIDControl != null)
            {
                AddEventHandlersForComboBox();
            }
        }

        private void AddEventHandlersForComboBox()
        {
            switch (ControlID)
            {
                case eMIDControlCode.field_Filter:
                  //((System.Windows.Forms.ComboBox)MIDControl).DragDrop += new DragEventHandler(ComboBox_DragDrop);
                    // Begin TT#3513 - JSmith - Clean Up Memory Leaks
                    //((System.Windows.Forms.ComboBox)MIDControl).DragEnter += new DragEventHandler(ComboBox_DragEnter);
                    if (MIDControl is MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)
                    {
                        ((MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)MIDControl).DragEnter += new DragEventHandler(ComboBox_DragEnter);
                    }
                    else if (MIDControl is System.Windows.Forms.ComboBox)
                    {
                        ((System.Windows.Forms.ComboBox)MIDControl).DragEnter += new DragEventHandler(ComboBox_DragEnter);
                    }
                    // End TT#3513 - JSmith - Clean Up Memory Leaks
                    break;
            }
        }

        private void RemoveEventHandlers()
        {
            if (MIDControl != null)
            {
                RemoveEventHandlersForComboBox();
            }
        }

        private void RemoveEventHandlersForComboBox()
        {
            switch (ControlID)
            {
                case eMIDControlCode.field_Filter:
                  //((System.Windows.Forms.ComboBox)MIDControl).DragDrop -= new DragEventHandler(ComboBox_DragDrop);
                    // Begin TT#3513 - JSmith - Clean Up Memory Leaks
                    //((System.Windows.Forms.ComboBox)MIDControl).DragEnter -= new DragEventHandler(ComboBox_DragEnter);
                    if (MIDControl is MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)
                    {
                        ((MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)MIDControl).DragEnter -= new DragEventHandler(ComboBox_DragEnter);
                    }
                    else if (MIDControl is System.Windows.Forms.ComboBox)
                    {
                        ((System.Windows.Forms.ComboBox)MIDControl).DragEnter -= new DragEventHandler(ComboBox_DragEnter);
                    }
                    // End TT#3513 - JSmith - Clean Up Memory Leaks
                    break;
            }
        }

        private HierarchyNodeProfile GetNodeProfile(string aProductID)
        {
            string desc = string.Empty;
            try
            {
                string productID = aProductID.Trim();
                string[] pArray = productID.Split(new char[] { '[' });
                productID = pArray[0].Trim();
                HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                EditMsgs em = new EditMsgs();
                return hm.NodeLookup(ref em, productID, false);
            }
            catch
            {
                throw;
            }
        }

        bool ApplySecurity()
        {
            bool securityOk = true;
            if (SecurityProfile.AllowUpdate)
            {
            }
            else
            {
            }

            if (SecurityProfile.AllowExecute)
            {
            }
            else
            {
            }

            return securityOk;
        }

        #endregion Methods

        #region Event Handlers

        override protected void ComboBox_DragEnter(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
           e.Effect = DragDropEffects.None;

           if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
           {
               cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
               if (cbList.ClipboardDataType == eProfileType.FilterStore)
               {
                   // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                   //e.Effect = DragDropEffects.All;
                   if (!_allowUserFilters &&
                           cbList.ClipboardProfile.OwnerUserRID != Include.GlobalUserRID)
                   {
                       e.Effect = DragDropEffects.None;
                   }
                   else if (SecurityProfile.AllowUpdate ||
                           SecurityProfile.AllowView)
                   {
                       e.Effect = DragDropEffects.All;
                   }
                   else
                   {
                       e.Effect = DragDropEffects.None;
                   }
                   // End TT#44
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

        override public bool ComboBox_DragDrop(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
            ComboBox cboFilter;
            MIDFilterNode filterNode;
            StoreFilterProfile filterProf;
            string errorMessage;
            int idx;

            try
            {
                ErrorProvider.SetError((ComboBox)sender, string.Empty);

                cboFilter = (ComboBox)sender;
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    if (cbList.ClipboardDataType == eProfileType.FilterStore)
                    {
                        //if (cbp.ClipboardData.GetType() == typeof(FilterClipboardData))
                        //if (cbp.ClipboardData.GetType() == typeof(MIDFilterNode))
                        //{
                        if (cbList.ClipboardProfile.FunctionSecurityProfile == null)
                            {
                                errorMessage = "Security object not passed into MIDTag.";
                                ErrorProvider.SetError(MIDControl, errorMessage);
                                return false;
                            }

                        Key = cbList.ClipboardProfile.Key;

                        if (cbList.ClipboardProfile.FunctionSecurityProfile.AccessDenied)
                            {
                                errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode),
                                cboFilter.Text);
                                ErrorProvider.SetError(cboFilter, errorMessage);
                            }
                            else
                            {
                                //filterNode = (MIDFilterNode)((FilterClipboardData)cbp.ClipboardData).FilterNode;
                                filterNode = (MIDFilterNode)cbList.ClipboardProfile.Node;
                                //filterProf = (StoreFilterProfile)filterNode.FolderItem.ChildObject;
                                filterProf = (StoreFilterProfile)filterNode.Profile;
                                idx = cboFilter.Items.IndexOf(new FilterNameCombo(filterProf.Key, filterProf.UserRID, filterProf.Name));
                                if ((idx != -1) && (idx != cboFilter.SelectedIndex))
                                {
                                    cboFilter.SelectedIndex = idx;
                                }

                                return true;
                            }
                        //}
                    }
                }
                return false;
            }
            catch
            {
                throw;
            }
        }

        #endregion Event Handlers
    }




    /// <summary>
    /// Used to save and retrieve Combo information from the tagControl.
    /// </summary>
    public class MIDStoreAttributeComboBoxTag : MIDComboBoxTag, IDisposable
    {
        #region Fields
        // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
        private bool _allowUserAttributes;
        // End TT#44
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
        //public MIDStoreAttributeComboBoxTag()
        //    : base()
        public MIDStoreAttributeComboBoxTag(SecurityProfile aSecurityProfile, bool aAllowUserAttributes)
            : base()
        {
            SecurityProfile = aSecurityProfile;
            _allowUserAttributes = aAllowUserAttributes;
        }
        // End TT#44

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock.</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aSecurityTypes">The security types appropriate for this control.</param>
        /// <param name="aSecurityActions">The appropriate eSecuritySelectType(s) for the control.</param>
        // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
        //public MIDStoreAttributeComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl)
        //    : base(aSAB, aControl, eMIDControlCode.field_Filter, eSecurityTypes.None, eSecuritySelectType.None)
        public MIDStoreAttributeComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, SecurityProfile aSecurityProfile, bool aAllowUserAttributes)
            : base(aSAB, aControl, eMIDControlCode.field_Filter, eSecurityTypes.None, eSecuritySelectType.None)
        {
            SecurityProfile = aSecurityProfile; 
            _allowUserAttributes = aAllowUserAttributes;
            AddEventHandlers();
        }
        // End TT#44

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aParentFormID">The eMIDControlCode of the parent form.</param>
        // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
        //public MIDStoreAttributeComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eMIDControlCode aParentFormID)
        //    : base(aSAB, aControl, aParentFormID, eMIDControlCode.field_Filter, eSecurityTypes.None, eSecuritySelectType.None)
        public MIDStoreAttributeComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eMIDControlCode aParentFormID, SecurityProfile aSecurityProfile, bool aAllowUserAttributes)
            : base(aSAB, aControl, aParentFormID, eMIDControlCode.field_Filter, eSecurityTypes.None, eSecuritySelectType.None)
        {
            SecurityProfile = aSecurityProfile;
            _allowUserAttributes = aAllowUserAttributes;
            AddEventHandlers();
        }
        // End TT#44

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="aSAB">The SessionAddressBlock</param>
        /// <param name="aControl">The control for which the tag will be associated.</param>
        /// <param name="aParentFormID">The eMIDControlCode of the parent form.</param>
        /// <param name="aIgnoreWheelMouse">The flag identifying if the wheel mouse is to be ignored for the control</param>
        // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
        //public MIDStoreAttributeComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eMIDControlCode aParentFormID, bool aIgnoreWheelMouse)
        //    : base(aSAB, aControl, aParentFormID, eMIDControlCode.field_Filter, eSecurityTypes.None, eSecuritySelectType.None, aIgnoreWheelMouse)
        public MIDStoreAttributeComboBoxTag(SessionAddressBlock aSAB, ComboBox aControl, eMIDControlCode aParentFormID, bool aIgnoreWheelMouse, SecurityProfile aSecurityProfile, bool aAllowUserAttributes)
            : base(aSAB, aControl, aParentFormID, eMIDControlCode.field_Filter, eSecurityTypes.None, eSecuritySelectType.None, aIgnoreWheelMouse)
        {
            SecurityProfile = aSecurityProfile;
            _allowUserAttributes = aAllowUserAttributes;
            AddEventHandlers();
        }
        // End TT#44

        #endregion Constructors

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveEventHandlers();
            }
        }

        ~MIDStoreAttributeComboBoxTag()
        {
            Dispose(true);
        }

        #endregion IDisposable Members

        #region Properties
        // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
        public bool AllowUserAttributes
        {
            get { return _allowUserAttributes; }
            set { _allowUserAttributes = value; }
        }
        // End TT#44
        #endregion Properties

        #region Methods

        private void AddEventHandlers()
        {
            if (MIDControl != null)
            {
                AddEventHandlersForComboBox();
            }
        }

        private void AddEventHandlersForComboBox()
        {
            switch (ControlID)
            {
                case eMIDControlCode.field_Filter:
                    //((System.Windows.Forms.ComboBox)MIDControl).DragDrop += new DragEventHandler(ComboBox_DragDrop);
                    // Begin TT#3513 - JSmith - Clean Up Memory Leaks
                    //((System.Windows.Forms.ComboBox)MIDControl).DragEnter += new DragEventHandler(ComboBox_DragEnter);
                    if (MIDControl is MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)
                    {
                        ((MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)MIDControl).DragEnter += new DragEventHandler(ComboBox_DragEnter);
                    }
                    else if (MIDControl is System.Windows.Forms.ComboBox)
                    {
                        ((System.Windows.Forms.ComboBox)MIDControl).DragEnter += new DragEventHandler(ComboBox_DragEnter);
                    }
                    // End TT#3513 - JSmith - Clean Up Memory Leaks
                    break;
            }
        }

        private void RemoveEventHandlers()
        {
            if (MIDControl != null)
            {
                RemoveEventHandlersForComboBox();
            }
        }

        private void RemoveEventHandlersForComboBox()
        {
            switch (ControlID)
            {
                case eMIDControlCode.field_Filter:
                    //((System.Windows.Forms.ComboBox)MIDControl).DragDrop -= new DragEventHandler(ComboBox_DragDrop);
                    // Begin TT#3513 - JSmith - Clean Up Memory Leaks
                    //((System.Windows.Forms.ComboBox)MIDControl).DragEnter -= new DragEventHandler(ComboBox_DragEnter);
                    if (MIDControl is MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)
                    {
                        ((MIDRetail.Windows.Controls.MIDComboBoxEnh.MyComboBox)MIDControl).DragEnter -= new DragEventHandler(ComboBox_DragEnter);
                    }
                    else if (MIDControl is System.Windows.Forms.ComboBox)
                    {
                        ((System.Windows.Forms.ComboBox)MIDControl).DragEnter -= new DragEventHandler(ComboBox_DragEnter);
                    }
                    // End TT#3513 - JSmith - Clean Up Memory Leaks
                    break;
            }
        }

        private HierarchyNodeProfile GetNodeProfile(string aProductID)
        {
            string desc = string.Empty;
            try
            {
                string productID = aProductID.Trim();
                string[] pArray = productID.Split(new char[] { '[' });
                productID = pArray[0].Trim();
                HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                EditMsgs em = new EditMsgs();
                return hm.NodeLookup(ref em, productID, false);
            }
            catch
            {
                throw;
            }
        }

        #endregion Methods

        #region Event Handlers

        override protected void ComboBox_DragEnter(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
            e.Effect = DragDropEffects.None;

            if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
            {
                cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                // Begin TT#30 - JSmith - Drag and Drop an attribute from the Store Explorer into a method or view does not work
                //if (cbList.ClipboardDataType == eProfileType.StoreGroupListView)
                if (cbList.ClipboardDataType == eProfileType.StoreGroup)
                // End TT#30
                {
                    // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                    //e.Effect = DragDropEffects.All;
                    if (!_allowUserAttributes &&
                            cbList.ClipboardProfile.OwnerUserRID != Include.GlobalUserRID)
                    {
                        e.Effect = DragDropEffects.None;
                    }
                    else if (SecurityProfile.AllowUpdate ||
                            SecurityProfile.AllowView)
                    {
                        e.Effect = DragDropEffects.All;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                    // End TT#44
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

        override public bool ComboBox_DragDrop(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
            ComboBox cboAttribSet;
            //MIDStoreNodeClipboardData storeNode;
            int idx;

            try
            {
                ErrorProvider.SetError((ComboBox)sender, string.Empty);

                cboAttribSet = (ComboBox)sender;
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    // Begin TT#30 - JSmith - Drag and Drop an attribute from the Store Explorer into a method or view does not work
                    //if (cbList.ClipboardDataType == eProfileType.StoreGroupListView)
                    if (cbList.ClipboardDataType == eProfileType.StoreGroup)
                    // End TT#30
                    {
                        //if (cbp.ClipboardData.GetType() == typeof(MIDStoreNodeClipboardData))
                        //{
                            //storeNode = (MIDStoreNodeClipboardData)cbp.ClipboardData;
                        Key = cbList.ClipboardProfile.Key;

                            if (cboAttribSet.Items.Count > 0) 
                            {
                                if (cboAttribSet.Items[0].GetType() == typeof(StoreGroupListViewProfile))
                                {
                                    idx = cboAttribSet.Items.IndexOf(new StoreGroupListViewProfile(cbList.ClipboardProfile.Key));
                                    if ((idx != -1) && (idx != cboAttribSet.SelectedIndex))
                                    {
                                        cboAttribSet.SelectedIndex = idx;
                                    }
                                }
                                else if (cboAttribSet.Items[0].GetType() == typeof(DataRowView))
                                {
                                    for (idx = 0; idx < cboAttribSet.Items.Count; idx++)
                                    {
                                        if ((int)((DataRowView)cboAttribSet.Items[idx]).Row[0] == cbList.ClipboardProfile.Key)
                                        {
                                            cboAttribSet.SelectedValue = cbList.ClipboardProfile.Key;
                                            break;
                                        }
                                    }
                                }
                            }
                        //}
                    }
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        #endregion Event Handlers
    }

}
