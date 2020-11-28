using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

using C1.Win.C1FlexGrid;

using MIDRetail.Common;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;
using MIDRetail.Windows.Controls;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Shared;


//using System.Drawing;
//using System.IO;
//using System.Configuration;
 

namespace MIDRetail.Windows
{
    public partial class frmOverrideLowLevelModel : ModelFormBase
    {
        //private SessionAddressBlock _SAB;//TT#1638 - Revised Model Save - RBeck
        private int _nodeRID;
        private int _last_custom_user_RID;
        private int _last_new_user_RID;
        private int _highLevelnodeRID;
        private int _versionRID;
        private int _ollRID;
        private int _original_entry_ollpRID;
        private int _original_entry_nodeRID;
        private bool _pendingCustomKey;
        private bool _pendingNewKey;
        private System.Data.DataTable _allColorDataTable;
        private ProfileList _versionProfList;
        private ProfileList _overrideProfiles;
        private List<int> _undisplayedNodeList = new List<int>();
        private OverrideLowLevelProfile _ollp;
        private OverrideLowLevelProfile _ollp_Undo;
        //private bool _changeMade = false;//TT#1638 - Revised Model Save - RBeck
        private bool _loadingModelDropdown = false;
        private bool _loadingCboHighLevelMerchandise = false;
        private bool _loadingGrdLowLevelNodes = false;
        private bool _populatingFromToLevels = false;
        private bool _droppingOnTxtMerchandise = false;
        private bool _validatingOnTxtMerchandise = false;
        private bool _newingOnTxtMerchandise = false;
        private FunctionSecurityProfile _userSecurity = null;
        private FunctionSecurityProfile _globalSecurity = null;
        private bool _showCustom;
        // Begin TT#155 - JSmith - Size Curve Method
        private bool _showVersion;
        // End TT#155
        private string _lowLevelText = null;
        private bool _settingModelDropDownIndex = false;
        private bool saveButtonHit = false;
        private bool saveAsButtonHit = false;

        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        private int _currModelRID = Include.Undefined;
        private int _currNodeRID = Include.Undefined;
        private int _currLowLevelOffset = Include.Undefined;
        private int _currHighLevelOffset = Include.Undefined;
        private int _currHighLevelNodeRID = Include.Undefined;
        private bool _showLowLevels = true;
        private bool _rebuildNodeList = false;
        // End TT#988

        // Begin TT#1994 - JSmith - Merchandise list does not change when select different Low Level.
        private bool _forceRebuildNodes = false;
        // End TT#1994


        #region Properties

        public bool ChangeMade
        {
            get { return _changeMade; }
        }

        #endregion Properties


        #region ModelFormBase Overrides

        //=====================================================
        // Delegate and event to pass back override model rid 
        //=====================================================
        public delegate void OverrideLowLevelCloseEventHandler(object source, OverrideLowLevelCloseEventArgs e);
        public event OverrideLowLevelCloseEventHandler OnOverrideLowLevelCloseHandler;

        override public void INew()
        {
            try
            {
                //FormLoaded = true;

                this.Text = "OverrideLowLevelModel"; //TT#1638 - MD - Revised Model Save - RBeck
               
                if (_pendingNewKey)
                {
                    if (_last_new_user_RID != Include.NoRID)
                    {
                        _ollp.DeleteModelWork(_last_new_user_RID);  //<--Delete Old Model From Temp Table

                        _pendingNewKey = false;
                        _last_new_user_RID = Include.NoRID;
                    }
                }

                NewIt("");
            }
            catch
            {
                throw;
            }
        }

        override public void ISave()
        {

            try
            {
                saveButtonHit = true;
                SaveChanges();
                saveButtonHit = false;
            }
            catch
            {
                throw;
            }
        }

        override public void ISaveAs()
        {

            try
            {
                saveAsButtonHit = true;
                SaveAs();
                saveAsButtonHit = false;
            }
            catch
            {
                throw;
            }
        }

        //override public void IDelete()
        //{
        //    try
        //    {
        //        DeleteIt();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        protected override bool UndoSaveChanges()
        {
            try
            {
                //-----Get Rid Of (New) Temp Record---------------------------
                if (_pendingNewKey)
                {
                    if (_last_new_user_RID != Include.NoRID)
                    {
                        _ollp.DeleteModelWork(_last_new_user_RID);  //<--Delete Old Model From Temp Table

                        _pendingNewKey = false;
                        _last_new_user_RID = Include.NoRID;
                    }
                }

                ////-----Get Rid Of (Blank) Temp Record---------------------------
                //if (_pendingCustomKey)
                //{
                //    if (_last_custom_user_RID != Include.NoRID)
                //    {
                //        _ollp = new OverrideLowLevelProfile(_last_custom_user_RID);
                //    }

                //   // _ollp.ModelChangeType = eChangeType.delete;
                //   //_ollp.WriteProfileWork(ref _last_custom_user_RID, _SAB.ClientServerSession.UserRID);
                //    _ollp.DeleteModelWork(_last_custom_user_RID);  //<--Delete Old Model From Temp Table
                //    _pendingCustomKey = false;
                //}


                //if (_ollp_Undo != null)
                //{
                //    _ollp_Undo.DeleteModelDetailsWork(_ollRID);
                //    _ollp_Undo.ModelChangeType = eChangeType.add;
                //    _ollp_Undo.WriteProfileWork(ref _last_custom_user_RID, _SAB.ClientServerSession.UserRID); //<--Rollback Changes
                //}

                LoadOverrideProfiles();

                ChangePending = false;

                return true;
            }
            catch (DatabaseForeignKeyViolation)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch
            {
                throw;
                return false;
            }
        }

        protected override bool SaveChanges()
        {
            try
            {
                int dropDown_ollp = Include.NoRID;
                string dropDown_name = "";
                int tmp_ollp = Include.NoRID;

                Cursor = Cursors.WaitCursor;

                //--Get Previous Select Model Dropdown Item---
                if (cbModelName.SelectedItem != null)
                {
                    dropDown_ollp = ((ComboObject)cbModelName.SelectedItem).Key;
                    dropDown_name = ((ComboObject)cbModelName.SelectedItem).Value;
                }

                if (_ollp.Name != "")
                {
                    _ollp.ModelChangeType = eChangeType.update;
                    SaveDBChanges(cbxSEInheritFromHigherLevel.Checked, cbxSEApplyToLowerLevels.Checked, false);

                    ChangePending = false;

                    if (_ollp.Name == "Custom")
                    {
                        _pendingCustomKey = false;

                        //--Copy Data From Temp Table Back To Permanent Table---
                        _ollRID = _ollp.CopyToPermanentTable(_ollRID);
                        _ollp.DeleteModelWork(_ollp.Key);
                        _ollp.Key = _ollRID;
                        _last_custom_user_RID = _ollRID;
                        LoadOverrideProfiles();

                        LoadGrdLowLevelNodes();
                        LoadModelDropdown();

                        _ollp_Undo = (OverrideLowLevelProfile)_ollp.Copy(); //<--Save a Undo Version

                        if (saveButtonHit)
                        {
                            SetSelectedModelItem(_ollRID);
                            base.Cancel_Click(); //<---Close Down Dialog & Get Out--
                        }
                        else
                        {
                            SetSelectedModelItem(dropDown_ollp);
                        }
                    }
                    else 
                    {
                        _pendingNewKey = false;

                        _ollp_Undo = (OverrideLowLevelProfile)_ollp.Copy(); //<--Save a Undo Version

                        //--Copy Data From Temp Table Back To Permanent Table---
                        _ollRID = _ollp.CopyToPermanentTable(_ollRID);
                        _ollp.Key = _ollRID;
                        LoadOverrideProfiles();

                        LoadGrdLowLevelNodes();
                        LoadModelDropdown();

                        if (saveButtonHit)
                        {
                            SetSelectedModelItem(_ollRID);
                        }
                        else
                        {
                            SetSelectedModelItem(dropDown_ollp);
                        }
                    }
                }
                else
                {
                    string fileName = "";
                    int userRID = Include.NoRID;
                    if (!SaveAsProcessing(ref fileName, ref userRID))
                    {
                        //Begin Track #5917 - KJohnson - bogus "(global)" entry error.
                        if (fileName.Trim() != "")
                        {
                            Creat_New_OLLP_Object(userRID);

                            _ollp.Name = fileName;
                            _ollp.User_RID = userRID;
                            SetScreenRadioBoxes();

                            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                            _ollp.ActiveOnlyInd = cbxActiveOnly.Checked;
                            // End TT#988

                            _ollp.ModelChangeType = eChangeType.update;
                            SaveDBChanges(false, false, false);

                            //--Copy Data From Temp Table Back To Permanent Table---
                            tmp_ollp = _ollp.Key;
                            _ollRID = _ollp.CopyToPermanentTable(_ollRID);
                            _ollp.Key = _ollRID;
                            _ollp.DeleteModelWork(tmp_ollp);
                            _ollp.DeleteModelWork(dropDown_ollp);
                            _ollp.DeleteModelWork(_last_new_user_RID);

                            _pendingNewKey = false;
                            _last_new_user_RID = Include.NoRID;

                            if (userRID == Include.CustomUserRID)
                            {
                                _pendingCustomKey = false;
                                _last_custom_user_RID = _ollRID;
                                if (dropDown_name == "Custom (custom)")
                                {
                                    dropDown_ollp = _last_custom_user_RID;
                                }
                            }

                            LoadOverrideProfiles();

                            _ollp_Undo = (OverrideLowLevelProfile)_ollp.Copy(); //<--Save an Undo Version
                            LoadGrdLowLevelNodes();
                            LoadModelDropdown();
                            if (saveButtonHit)
                            {
                                SetSelectedModelItem(_ollRID);
                            }
                            else
                            {
                                SetSelectedModelItem(dropDown_ollp);
                            }

                            ChangePending = false;

                            SetSecurityOnForm();

                            //btnNew.Enabled = false;
                            //btnSave.Enabled = false;
                            //btnSaveAs.Enabled = false;
                            //btnDelete.Enabled = false;

                            cbxSEInheritFromHigherLevel.Checked = false;
                            cbxSEApplyToLowerLevels.Checked = false;
                        }
                        else 
                        {
                            //-----User Escaped Out Of The Save As Dialog Box-----
                        }
                        //End Track #5917 - KJohnson
                    }
                    else
                    {
                        //-----User Canceled Out Of The Save As Dialog Box-----
                    }
                }

                return true;
            }
            catch
            {
                throw;
                return false;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        protected override void AfterClosing()
        {
            try
            {
                //---Turn Method Forms Back On-------------------------
                foreach (Form childForm in this.MdiParent.MdiChildren)
                {
                    childForm.Enabled = true;
                }

                ////-----Get Rid Of Custom Record If It Is Not Selected-----
                //if ((_last_custom_user_RID != Include.NoRID) && (_last_custom_user_RID != _ollRID))
                //{
                //    if (_last_custom_user_RID != _original_entry_ollpRID)
                //    {
                //        _ollp.DeleteModel(_last_custom_user_RID);
                //    }
                //    //else
                //    //{
                //    //    MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CanNotSetOverrideModelInUse), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    //    _ollRID = _original_entry_ollpRID;
                //    //}
                //}

				_ollp.DeleteModelWork(_ollRID);  //<--Delete Old Model From Temp Table

                //-----Get Rid Of (New) Temp Record---------------------------
                if (_pendingNewKey)
                {
                    if (_last_new_user_RID != Include.NoRID)
                    {
                        _ollp.DeleteModelWork(_last_new_user_RID);  //<--Delete Old Model From Temp Table

                        _pendingNewKey = false;
                        _last_new_user_RID = Include.NoRID;
                    }
                }

                //-----Get Rid Of (Blank) Temp Record---------------------------
                if (_pendingCustomKey)
                {
                    if (_last_custom_user_RID != Include.NoRID)
                    {
                        _ollp.DeleteModelWork(_last_custom_user_RID);  //<--Delete Old Model From Temp Table

                        _last_custom_user_RID = Include.NoRID;
                        _pendingCustomKey = false;
                    }
                }

                //---Create Call Back Object To Pass Back _ollRID To Method---------
                if (OnOverrideLowLevelCloseHandler != null)
                {
                    //---If Number Passed In Is NOT In DB, Set It To Entry Model Number To Pass Back--
                    LoadOverrideProfiles();
                    bool found_ollRID = false;
                    bool found_original_ollRID = false;
                    foreach (OverrideLowLevelProfile ollp in _overrideProfiles)
                    {
                        if (ollp.Key == _ollRID)
                        {
                            found_ollRID = true;
                        }

                        if (ollp.Key == _original_entry_ollpRID)
                        {
                            found_original_ollRID = true;
                        }
                    }

                    //--If _ollRID Not In DB, Program Blows Up If Not Set Back To NoRID Or Original_Entry_OllRID---
                    if (!found_ollRID)
                    {
                        if (found_original_ollRID)
                        {
                            _ollRID = _original_entry_ollpRID;
                        }
                        else
                        {
                            _ollRID = Include.NoRID;
                        }
                    }

                    OnOverrideLowLevelCloseHandler(this, new OverrideLowLevelCloseEventArgs(this._ollRID, this._last_custom_user_RID));
                }

                return;
            }
            catch (DatabaseForeignKeyViolation)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch
            {
                throw;
                return;
            }
        }

        #endregion ModelFormBase Overrides


        public frmOverrideLowLevelModel(SessionAddressBlock aSAB, int aOllRID, int aHN_RID, int aVersionRID, bool showCustom, FunctionSecurityProfile methodSecurity)	// Track #5909
            : base (aSAB)
        {
            _userSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsUserOverrideLowLevels);
            _globalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsGlobalOverrideLowLevels);
            //_SAB = aSAB;//TT#1638 - Revised Model Save - RBeck
            _ollRID = aOllRID;
            _nodeRID = aHN_RID;
            _last_custom_user_RID = Include.NoRID;
            _versionRID = aVersionRID;
            _showCustom = showCustom;
            // Begin TT#155 - JSmith - Size Curve Method
            _showVersion = true;
            // End TT#155
            _lowLevelText = null;
			// Begin Track #5909 - stodd
            //FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsOverrideLowLevels);
			FunctionSecurity = methodSecurity;
			// End Track #5909 - stodd
            InitializeComponent();
        }

		public frmOverrideLowLevelModel(SessionAddressBlock aSAB, int aOllRID, int aHN_RID, int aVersionRID, string lowLevelText, FunctionSecurityProfile methodSecurity)	// Track #5909
            : base (aSAB)
        {
            _userSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsUserOverrideLowLevels);
            _globalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsGlobalOverrideLowLevels);
            //_SAB = aSAB;//TT#1638 - Revised Model Save - RBeck
            _ollRID = aOllRID;
            _nodeRID = aHN_RID;
            _last_custom_user_RID = Include.NoRID;
            _last_new_user_RID = Include.NoRID;
            _versionRID = aVersionRID;
            _showCustom = true;
            // Begin TT#155 - JSmith - Size Curve Method
            _showVersion = true;
            // End TT#155
            _lowLevelText = lowLevelText;
			// Begin Track #5909 - stodd
			//FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsOverrideLowLevels);
			FunctionSecurity = methodSecurity;
			// End Track #5909 - stodd
            InitializeComponent();
        }

		public frmOverrideLowLevelModel(SessionAddressBlock aSAB, int aOllRID, int aHN_RID, int aVersionRID, string lowLevelText, int customOllRID, FunctionSecurityProfile methodSecurity)	// Track #5909
            : base (aSAB)
        {
            _userSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsUserOverrideLowLevels);
            _globalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsGlobalOverrideLowLevels);
            //_SAB = aSAB;//TT#1638 - Revised Model Save - RBeck
            _ollRID = aOllRID;
            _nodeRID = aHN_RID;
            _last_custom_user_RID = customOllRID;
            _last_new_user_RID = Include.NoRID;
            _versionRID = aVersionRID;
            _showCustom = true;
            // Begin TT#155 - JSmith - Size Curve Method
            _showVersion = true;
            // End TT#155
            _lowLevelText = lowLevelText;
			// Begin Track #5909 - stodd
			//FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsOverrideLowLevels);
			FunctionSecurity = methodSecurity;
			// End Track #5909 - stodd
            InitializeComponent();
        }

        // Begin TT#155 - JSmith - Size Curve Method
        public frmOverrideLowLevelModel(SessionAddressBlock aSAB, int aOllRID, int aHN_RID, int aVersionRID, string lowLevelText, int customOllRID, bool aShowVersion, FunctionSecurityProfile methodSecurity)
            : base (aSAB)
        {
            _userSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsUserOverrideLowLevels);
            _globalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsGlobalOverrideLowLevels);
            //_SAB = aSAB;//TT#1638 - Revised Model Save - RBeck
            _ollRID = aOllRID;
            _nodeRID = aHN_RID;
            _last_custom_user_RID = customOllRID;
            _last_new_user_RID = Include.NoRID;
            _versionRID = aVersionRID;
            _showCustom = true;
            // Begin TT#155 - JSmith - Size Curve Method
            _showVersion = aShowVersion;
            // End TT#155
            _lowLevelText = lowLevelText;
            FunctionSecurity = methodSecurity;
            InitializeComponent();
        }
        // End TT#155

        private void frmOverrideLowLevelModel_Load(object sender, EventArgs e)
        {
            FormLoaded = false;

            SetText();

            CreateVersionComboLists();

            _original_entry_ollpRID = _ollRID;
            _original_entry_nodeRID = _nodeRID;
            _pendingNewKey = false;
            _pendingCustomKey = false;

            if (_ollRID == Include.NoRID)
            {
                if (_showCustom)
                {
                    //Begin Track #5921 - KJohnson - No unsaved Custom shows in Model dropdown list.
                    CreateCustomEntryInDropDown();
                    //End Track #5921 - KJohnson
                }
                else 
                {
                    ClearForm();
                }
            }
            else
            {
                _ollp = new OverrideLowLevelProfile(_ollRID);
                _nodeRID = _ollp.HN_RID;
                _ollRID = _ollp.Key;

                //--Look To See If We Have A Custom OLLRID--------
                if (_ollp.User_RID == Include.CustomUserRID)
                {
                    if (_original_entry_nodeRID != _ollp.HN_RID)
                    {
                        //Begin Track #6099 - KJohnson - Override low-level has wrong data after keying mew node.
                        _ollp.DeleteModel(_ollRID);
                        CreateCustomEntryInDropDown();

                        //_ollRID = Include.NoRID;
                        //_nodeRID = _original_entry_nodeRID;

                        //_ollp = new OverrideLowLevelProfile(Include.NoRID);
                        //_ollp.HN_RID = _nodeRID;
                        //_ollp.User_RID = Include.CustomUserRID;
                        //_ollp.Name = "";

                        //if (!_ollp.CustomExists(_last_custom_user_RID))
                        //{
                        //    //---Build New Custom-------------------
                        //    NewIt("Custom");
                        //}
                        //else
                        //{
                        //    //---Load Old Custom--------------------
                        //    _ollp = new OverrideLowLevelProfile(_last_custom_user_RID);
                        //    _ollRID = _last_custom_user_RID;
                        //}
                        //End Track #6099 - KJohnson
                    }
                    //_last_custom_user_RID = _ollRID;  //<---Begin Track #6099 - KJohnson - Override low-level has wrong data after keying mew node.
                }
                else 
                {
                    //Begin Track #5921 - KJohnson - No unsaved Custom shows in Model dropdown list.
                    if (_showCustom)
                    {
                        CreateCustomEntryInDropDown();
                        _ollRID = _original_entry_ollpRID;
                        _nodeRID = _original_entry_nodeRID;
                    }
                    else
                    {
                        ClearForm();
                    }
                    //End Track #5921 - KJohnson
                }
            }

            LoadOverrideProfiles();

            LoadModelDropdown();
            SetSelectedModelItem(_ollRID);
            SetScreenRadioBoxes();
            SetSecurityOnForm();

            //---Always Turn These Off (Security Doesn't Matter)---
            radGlobal.Enabled = false;
            radUser.Enabled = false;
            radCustom.Enabled = false;

            if (_overrideProfiles.Count == 0)
            {
                btnNew.Enabled = true;
                btnSave.Enabled = false;
                btnSaveAs.Enabled = false;
                btnDelete.Enabled = false;
                btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
            }
            else if (_ollRID == Include.NoRID)
            {
                //btnNew.Enabled = false;
                btnSave.Enabled = false;
                btnSaveAs.Enabled = false;
                btnDelete.Enabled = false;
                btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
            }

            cbxSEInheritFromHigherLevel.Checked = false;
            cbxSEApplyToLowerLevels.Checked = false;

            txtMerchandise.Focus();

            FormLoaded = true;
        }

        private void CreateCustomEntryInDropDown()
        {
            //Begin Track #5921 - KJohnson - No unsaved Custom shows in Model dropdown list.
            try
            {
                _ollp = new OverrideLowLevelProfile(Include.NoRID);
                //_ollp.User_RID = _SAB.ClientServerSession.UserRID;
                //_ollp.User_RID = Include.GlobalUserRID;
                _ollp.User_RID = Include.CustomUserRID;
                _ollp.Name = "Custom";

                if (!_ollp.CustomExists(_last_custom_user_RID))
                {
                    //---Build New Custom-------------------
                    NewIt("Custom");
                }
                else
                {
                    //---Load Old Custom--------------------
                    _ollp = new OverrideLowLevelProfile(_last_custom_user_RID);
                    _ollRID = _last_custom_user_RID;
                }
        }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            //End Track #5921 - KJohnson
        }

        private void LoadOverrideProfiles()
        {
            try
            {
                _overrideProfiles = OverrideLowLevelProfile.LoadAllProfiles(_last_custom_user_RID, SAB.ClientServerSession.UserRID, _globalSecurity.AllowView, _userSecurity.AllowView, _last_custom_user_RID);

                if (_pendingCustomKey)
                {
                    OverrideLowLevelProfile mp = new OverrideLowLevelProfile(_last_custom_user_RID);
                    mp.LoadProfileWork(_last_custom_user_RID);
                    _overrideProfiles.Insert(0, mp);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        //override public bool InitializeForm(string currModel)
        //{         
        //     return true;
        //}

        private void SetText()
        {
            try
            {

                //this.modelLabel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OverrideModel_Model) + ":";
                this.lblModelName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OverrideModel_Model) + ":";
                this.MerchandiseLabel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise) + ":";
                this.highLevelLabel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_HighLevel) + ":";
                this.lowLevelLabel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OverrideModel_Low_Level) + ":";
                this.cbxSEInheritFromHigherLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherit_From_Higher_Level);
                this.cbxSEApplyToLowerLevels.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Apply_To_Lower_Levels);
                // BEGIN TT#1216 - AGallagher - Use of Low Level Override Models in OTS Forecast / Methods not working logically
                this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);     
                // END TT#1216 - AGallagher - Use of Low Level Override Models in OTS Forecast / Methods not working logically
                // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                cbxActiveOnly.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Active_Only);
                // End TT#988
				
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private bool NewIt(string type)
        {
            try
            {
                if ((_ollRID != _last_custom_user_RID) || (_pendingCustomKey == false))
                {
                    _ollp.DeleteModelWork(_ollRID);
                }

                if (type == "")
                {
                    ClearForm();
                }
                _settingModelDropDownIndex = true;
                cbModelName.SelectedIndex = -1;
                _settingModelDropDownIndex = false;

                SetSecurityOnForm();

                radGlobal.Checked = false;
                radUser.Checked = false;
                if (type == "Custom")
                {
                    radCustom.Checked = true;
                }
                else
                {
                    radCustom.Checked = false;
                }

                if (_original_entry_nodeRID == Include.NoRID)
                {
                    //btnNew.Enabled = false;
                    btnSave.Enabled = false;
                    btnSaveAs.Enabled = false;
                    btnDelete.Enabled = false;
                    btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool

                    ChangePending = false;
                }
                else
                {
                    _newingOnTxtMerchandise = true;
                    if (_original_entry_nodeRID != Include.NoRID)
                    {
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_original_entry_nodeRID);
                        txtMerchandise.Text = hnp.Text;
                        txtMerchandise.Tag = hnp.Key;
                        _nodeRID = hnp.Key;
                        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                        cbxActiveOnly.Checked = false;
                        // End TT#988

                        PopulateFromToLevels(hnp, cboHighLevel.ComboBox, 0, true);
                        PopulateFromToLevels(hnp, cboLowLevel.ComboBox, 0, true);
                        LoadCboHighLevelMerchandise();
                    }
                    _newingOnTxtMerchandise = false;

                    ProcessNewOLLID(type);

                    btnNew.Enabled = false;
                    //btnSaveAs.Enabled = false;
                    btnDelete.Enabled = false;
                    btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool

                    ChangePending = false;
                }

                txtMerchandise.Enabled = true;
                cbxSEInheritFromHigherLevel.Enabled = false;
                cbxSEApplyToLowerLevels.Enabled = false;
                cbxSEInheritFromHigherLevel.Checked = false;
                cbxSEApplyToLowerLevels.Checked = false;

                txtMerchandise.Focus();

                return false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

//Begin TT#394 - MD - RBeck -  delete receive unhandled exception
        //private bool DeleteIt()
        override protected void DeleteModel()
        {
            try
            {
                if (
                    MessageBox.Show("Are you sure you want to delete this model?", "Delete Override Low Level Model",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) ==
                    DialogResult.Yes &&
                    cbModelName.SelectedIndex >= 0)
                //BEGIN TT#601-MD-VStuart-FWOS Model attempts delete while InUse
                {
                    //If the RID is InUse don't delete. If RID is NOT InUse go ahead and delete.
                    var emp = new OverrideLowLevelProfile(_ollRID);
                    eProfileType type = emp.ProfileType;
                    int rid = _ollRID;

                    if (!CheckInUse(type, rid, false))
                    {
                        //if (cbModelName.SelectedIndex < 0)//TT#394 - MD - RBeck -  delete receive unhandled exception 
                        //return false;//TT#394 - MD - RBeck -  delete receive unhandled exception 

                        _ollp.DeleteModel(_ollRID);
                        _ollp.DeleteModelWork(_ollRID);

                        if (_ollp.User_RID == Include.CustomUserRID)
                        {
                            _last_custom_user_RID = Include.NoRID;
                            _pendingCustomKey = false;
                        }

                        ClearForm();
                        LoadModelDropdown();

                        ////Begin Track #5921 - KJohnson - No unsaved Custom shows in Model dropdown list.
                        ////_settingModelDropDownIndex = true;
                        //cbModelName.SelectedIndex = -1; //TT#394 - MD - RBeck -  delete receive unhandled exception 
                        ////_settingModelDropDownIndex = false;

                        //Begin TT#394 - MD - RBeck -  delete receive unhandled exception
                        // These changes will make OverrideLowLevelModel function like the other Model methods
                        // Begin TT#460-MD - JSmith - Deleting Override Low Level Model throws an error.
                        //cbModelName.SelectedIndex = 0;
                        if (cbModelName.Items.Count > 0)
                        {
                            cbModelName.SelectedIndex = 0;
                        }
                        else
                        {
                            cbModelName.SelectedIndex = -1;
                        }
                        // End TT#460-MD - JSmith - Deleting Override Low Level Model throws an error.
                        //   this.cbModelName_SelectionChangeCommitted(this, new EventArgs());
                        //  SetSecurityOnForm();

                        ////btnNew.Enabled = false;
                        //  btnSave.Enabled = false;
                        //  btnSaveAs.Enabled = false;
                        //  btnDelete.Enabled = false;
                        //  //End Track #5921
                        //End   TT#394 - MD - RBeck -  delete receive unhandled exception   

                        cbxSEInheritFromHigherLevel.Checked = false;
                        cbxSEApplyToLowerLevels.Checked = false;

                        this.Text = "OverrideLowLevelModel"; //TT#1638 - MD - Revised Model Save - RBeck
                    }
                    //return false;//TT#394 - MD - RBeck -  delete receive unhandled exception 
                }
                //END  TT#601-MD-VStuart-FWOS Model attempts delete while InUse
            }
            catch (DatabaseForeignKeyViolation)
            {
                //BEGIN TT#110-MD-VStuart - In Use Tool
                //MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowInUse();
                //END TT#110-MD-VStuart - In Use Tool
                //return false;//TT#394 - MD - RBeck -  delete receive unhandled exception 
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

//End   TT#394 - MD - RBeck -  delete receive unhandled exception

        private bool SaveAs()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                string fileName = "";
                int userRID = Include.NoRID;
                int dropDown_ollp = Include.NoRID;
                string dropDown_name = "";
                int tmp_ollp = Include.NoRID;
                if (!SaveAsProcessing(ref fileName, ref userRID))
                {
                    //Begin Track #5918 - KJohnson - bogus "Name already Exist" error.
                    if (fileName.Trim() != "")
                    {
                        //--Get Previous Select Model Dropdown Item---
                        if (cbModelName.SelectedItem != null)
                        {
                            dropDown_ollp = ((ComboObject)cbModelName.SelectedItem).Key;
                            dropDown_name = ((ComboObject)cbModelName.SelectedItem).Value;
                        }

                        OverrideLowLevelProfile ollp_Holder = _ollp.Copy();

                        Creat_New_OLLP_Object(userRID);

                        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                        //_ollp.DetailList = ollp_Holder.DetailList;
                        if (_showLowLevels)
                        {
                            _ollp.DetailList = ollp_Holder.DetailList;
                        }
                        // End TT#988
                        _ollp.Name = fileName;
                        _ollp.User_RID = userRID;
                        SetScreenRadioBoxes();

                        SaveDBChanges(false, false, true);

                        //--Copy Data From Temp Table Back To Permanent Table---
                        tmp_ollp = _ollp.Key;
                        _ollRID = _ollp.CopyToPermanentTable(_ollRID);
                        _ollp.Key = _ollRID;
                        _ollp.DeleteModelWork(tmp_ollp);

                        //Begin Track #5919 & #5920 - KJohnson - You also get a free Custom (2 Customs Now) in the Model "Drop Down".
                        if ((dropDown_ollp == _last_custom_user_RID) && _pendingCustomKey)
                        {
                            //----Do Nothing------
                        }
                        else
                        {
                            //--Only Delete If Not A Custom
                            _ollp.DeleteModelWork(dropDown_ollp);
                        }
                        //End Track #5919 & #5920 - KJohnson

                        _ollp.DeleteModelWork(_last_new_user_RID);

                        _pendingNewKey = false;
                        _last_new_user_RID = Include.NoRID;

                        if (userRID == Include.CustomUserRID)
                        {
                            _pendingCustomKey = false;
                            _last_custom_user_RID = _ollRID;
                            if (dropDown_name == "Custom (custom)")
                            {
                                dropDown_ollp = _last_custom_user_RID;
                            }
                        }

                        LoadOverrideProfiles();

                        //Begin Track #5919 & #5920 - KJohnson - You also get a free Custom (2 Customs Now) in the Model "Drop Down".
                        //if (((_ollp_Undo != null) && (userRID != Include.CustomUserRID)) &&
                        //    (cbModelName.SelectedItem != null))
                        //{
                        //    //---Restore The Contents Of The Original Node Back--------
                        //    _ollp_Undo.DeleteModelDetailsWork(_ollp_Undo.Key);
                        //    _ollp_Undo.ModelChangeType = eChangeType.add;
                        //    _ollp_Undo.WriteProfile(ref _last_custom_user_RID, _SAB.ClientServerSession.UserRID); //<--Rollback Changes

                        //    LoadOverrideProfiles();
                        //}
                        //End Track #5919 & #5920 - KJohnson

                        _ollp_Undo = (OverrideLowLevelProfile)_ollp.Copy(); //<--Save a Undo Version

                        LoadGrdLowLevelNodes();
                        LoadModelDropdown();
                        if (saveAsButtonHit)
                        {
                            SetSelectedModelItem(_ollRID);
                        }
                        else
                        {
                            SetSelectedModelItem(dropDown_ollp);
                        }

                        ChangePending = false;

                        SetSecurityOnForm();

                        //btnNew.Enabled = false;
                        //btnSave.Enabled = false;
                        //btnSaveAs.Enabled = false;
                        //btnDelete.Enabled = false;

                        cbxSEInheritFromHigherLevel.Checked = false;
                        cbxSEApplyToLowerLevels.Checked = false;
                    }
                    else 
                    {
                        //-----User Escaped Out Of The Save As Dialog Box-----
                    }
                    //End Track #5918
                }
                else
                {
                    //-----User Canceled Out Of The Save As Dialog Box-----
                }

                return false;
            }
            catch (DatabaseForeignKeyViolation)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void Creat_New_OLLP_Object(int userRID)
        {
            if (userRID == Include.CustomUserRID)
            {
                if (_last_custom_user_RID != Include.NoRID)
                {
                    if (_ollp.CustomExists(_last_custom_user_RID))
                    {
                        //--Record Already Exist In Permanent Database----
                        _ollp.DeleteModel(_last_new_user_RID);
                        _ollp.DeleteModelWork(_last_new_user_RID);
                        _ollp.DeleteModelWork(_ollRID);
                        _ollp.DeleteModelWork(_last_custom_user_RID);
                        _ollp.DeleteModelDetails(_last_custom_user_RID);
                        _ollp.CopyToWorkTables(_last_custom_user_RID);
                        // Begin TT#1994 - JSmith - Merchandise list does not change when select different Low Level.
                        _forceRebuildNodes = true;
                        // End TT#1994

                        _ollp = new OverrideLowLevelProfile(_last_custom_user_RID);
                        _ollRID = _last_custom_user_RID;

                        _ollp.ModelChangeType = eChangeType.update;
                    }
                    else
                    {
                        if (_pendingCustomKey)
                        {
                            //--Record Is A Pending Custom----------------
                            _ollp.DeleteModel(_last_new_user_RID);
                            _ollp.DeleteModelWork(_last_new_user_RID);

                            _ollp = new OverrideLowLevelProfile(_last_custom_user_RID);
                            _ollRID = _last_custom_user_RID;

                            _ollp.ModelChangeType = eChangeType.update;
                        }
                        else 
                        {
                            if (_pendingNewKey)
                            {
                                _ollp = new OverrideLowLevelProfile(_last_new_user_RID);
                                _ollRID = _last_new_user_RID;

                                _ollp.ModelChangeType = eChangeType.update;
                            }
                            else
                            {
                                //_ollp = new OverrideLowLevelProfile(Include.NoRID);
                                //_ollRID = Include.NoRID;

                                _ollp.ModelChangeType = eChangeType.add;
                            }
                        }
                    }
                }
                else
                {
                    if (_pendingNewKey)
                    {
                        _ollp = new OverrideLowLevelProfile(_last_new_user_RID);
                        _ollRID = _last_new_user_RID;

                        _ollp.ModelChangeType = eChangeType.update;
                    }
                    else
                    {
                        //_ollp = new OverrideLowLevelProfile(Include.NoRID);
                        //_ollRID = Include.NoRID;

                        _ollp.ModelChangeType = eChangeType.add;
                    }
                }

                _pendingCustomKey = false;
                _last_custom_user_RID = Include.NoRID;
            }
            else
            {
                if (_pendingNewKey)
                {
                    _ollp = new OverrideLowLevelProfile(_last_new_user_RID);
                    _ollRID = _last_new_user_RID;

                    _ollp.ModelChangeType = eChangeType.update;
                }
                else
                {
                    //_ollp = new OverrideLowLevelProfile(Include.NoRID);
                    //_ollRID = Include.NoRID;

                    _ollp.ModelChangeType = eChangeType.add;
                }
            }

            // Begin TT#1994 - JSmith - Merchandise list does not change when select different Low Level.
            _forceRebuildNodes = false;
            // End TT#1994
        }

        private void txtMerchandise_DragEnter(object sender, DragEventArgs e)
        {
            Merchandise_DragEnter(sender, e);
        }

        private void SetSelectedModelItem(int ollRID)
        {
            _loadingModelDropdown = true;

            int i = 0;
            bool foundIt = false;
            foreach (ComboObject hnp2 in cbModelName.Items)
            {
                if (hnp2.Key == ollRID)
                {
                    foundIt = true;
                    //_settingModelDropDownIndex = true;
                    cbModelName.SelectedIndex = i;
                   // this.cbModelName_SelectionChangeCommitted(this, new EventArgs());
                    //_settingModelDropDownIndex = false;
                    _ollp_Undo = (OverrideLowLevelProfile)_ollp.Copy(); //<--Save a Undo Version
                    break;
                }
                i++;
            }

            SetSecurityOnForm();

            cbxSEInheritFromHigherLevel.Checked = false;
            cbxSEApplyToLowerLevels.Checked = false;

            if (cbModelName.Items.Count > 0 && !foundIt)
            {
                ClearForm();
                _settingModelDropDownIndex = true;
                cbModelName.SelectedIndex = -1;
                _settingModelDropDownIndex = false;

                btnSave.Enabled = false;
                btnSaveAs.Enabled = false;
                btnDelete.Enabled = false;
                btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
                //cboHighLevel.Enabled = false;
                //cboLowLevel.Enabled = false;
                //cboHighLevelMerchandise.Enabled = false;
                //txtMerchandise.Focus();

                //cbxSEInheritFromHigherLevel.Enabled = false;
                //cbxSEApplyToLowerLevels.Enabled = false;
            }
            else
            {
                //btnSave.Enabled = false;
            }

            _loadingModelDropdown = false;
        }

        private void txtMerchandise_DragDrop(object sender, DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
            TreeNodeClipboardProfile cbProf = null;
            try
            {
                _ollp.DeleteModelDetailsWork(_ollRID);

                if (_pendingNewKey)
                {
                    if (_last_new_user_RID != Include.NoRID)
                    {
                        _ollp.DeleteModelWork(_last_new_user_RID);  //<--Delete Old Model From Temp Table

                        _pendingNewKey = false;
                        _last_new_user_RID = Include.NoRID;
                    }
                }

                //ClipboardProfile cbp = GetClipboardData(eClipboardDataType.HierarchyNode);
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    txtMerchandise.Text = cbList.ClipboardProfile.Text;
                    txtMerchandise.Tag = cbList.ClipboardProfile.Key;
                    // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                    cbxActiveOnly.Checked = false;
                    // End TT#988
                    _nodeRID = cbList.ClipboardProfile.Key;

                    _droppingOnTxtMerchandise = true;
                    if (_nodeRID != Include.NoRID)
                    {
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_nodeRID);
                        PopulateFromToLevels(hnp, cboHighLevel.ComboBox, 0, true);
                        PopulateFromToLevels(hnp, cboLowLevel.ComboBox, 0, true);
                        LoadCboHighLevelMerchandise();
                    }
                    _droppingOnTxtMerchandise = false;

                    ProcessNewOLLID("");

                    SetSecurityOnForm();

                    btnNew.Enabled = false;
                    //btnSaveAs.Enabled = false;
                    btnDelete.Enabled = false;
                    btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
                }
            }
            catch (BadDataInClipboardException)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private bool SaveDBChanges(bool inheritFromHigherLevelChecked, bool applyToLowerLevelsChecked, bool useDetailListToStoreDB)
        {
            try
            {
                if (!_loadingModelDropdown &&
                    !_loadingCboHighLevelMerchandise &&
                    !_loadingGrdLowLevelNodes &&
                    !_populatingFromToLevels &&
                    !_droppingOnTxtMerchandise &&
                    !_validatingOnTxtMerchandise &&
                    !_newingOnTxtMerchandise)
                {
                    if ((inheritFromHigherLevelChecked == true) || (applyToLowerLevelsChecked == true))
                    {
                        if (inheritFromHigherLevelChecked == true)
                        {
                            foreach (UltraGridRow gridRow in ugModel.Rows)
                            {
                                string productId = Convert.ToString(gridRow.Cells["Merchandise"].Value, CultureInfo.CurrentCulture);
                                _ollp.DeleteModelDetailsWork(_ollRID, Convert.ToInt32(gridRow.Cells["Key"].Value));
                            }
                        }

                        if (applyToLowerLevelsChecked == true)
                        {
                            HierarchySessionTransaction hierTran = new HierarchySessionTransaction(SAB);
                            foreach (UltraGridRow gridRow in ugModel.Rows)
                            {
                                string productId = Convert.ToString(gridRow.Cells["Merchandise"].Value, CultureInfo.CurrentCulture);
                                hierTran.DeleteOverrideList(_ollRID, Convert.ToInt32(gridRow.Cells["Key"].Value));
                            }
                        }

                        cbxSEInheritFromHigherLevel.Checked = false;
                        cbxSEApplyToLowerLevels.Checked = false;
                        LoadGrdLowLevelNodes();
                    }
                    else
                    {
                        //_ollp.Key = _ollRID;
                        _ollp.HN_RID = _nodeRID;
                        _ollp.High_Level_HN_RID = _highLevelnodeRID;
                        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                        _ollp.ActiveOnlyInd = cbxActiveOnly.Checked;
                        // End TT#988
                        if (_ollp.User_RID == Include.NoRID)
                        {
                            //_ollp.User_RID = _SAB.ClientServerSession.UserRID;
							// Begin Track #5909 stodd
							if (_globalSecurity.AllowUpdate)
								_ollp.User_RID = Include.GlobalUserRID; //<--Always use Global so it will come back in the read_all
							else if (_userSecurity.AllowUpdate)
								_ollp.User_RID = SAB.ClientServerSession.UserRID;
							// End Track #5909
							//_ollp.User_RID = Include.CustomUserRID;
                        }
                        if (cboHighLevel.SelectedItem != null)
                        {
                            _ollp.HighLevelSeq = ((HighLevelCombo)cboHighLevel.SelectedItem).HighLevelSequence;
                            _ollp.HighLevelOffset = ((HighLevelCombo)cboHighLevel.SelectedItem).HighLevelOffset;
                            _ollp.HighLevelType = ((HighLevelCombo)cboHighLevel.SelectedItem).HighLevelType;
                        }
                        if (cboLowLevel.SelectedItem != null)
                        {
                            _ollp.LowLevelSeq = ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelSequence;
                            _ollp.LowLevelOffset = ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelOffset;
                            _ollp.LowLevelType = ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelType;
                        }

                        if (useDetailListToStoreDB)
                        {
                            OverrideLowLevelProfile ollp_Holder = new OverrideLowLevelProfile(_ollRID);
                            ollp_Holder.LoadProfileWork(_ollRID);
                            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                            //_ollp.DetailList = ollp_Holder.DetailList;
                            if (_showLowLevels)
                            {
                                _ollp.DetailList = ollp_Holder.DetailList;
                            }
                            // End TT#988

                            foreach (UltraGridRow gridRow in ugModel.Rows)
                            {
                                int versionValue = Convert.ToInt32(gridRow.Cells["Version"].Value);
                                bool excludeValue = Convert.ToBoolean(gridRow.Cells["Exclude"].Value);

                                bool originalVersionChanged = false;
                                if (Convert.ToInt32(gridRow.Cells["OriginalVersion"].Value) != versionValue)
                                {
                                    originalVersionChanged = true;
                                }

                                bool originalExcludeChanged = false;
                                if (Convert.ToBoolean(gridRow.Cells["OriginalExclude"].Value) != excludeValue)
                                {
                                    originalExcludeChanged = true;
                                }

                                if (originalVersionChanged || originalExcludeChanged)
                                {
                                    OverrideLowLevelDetailProfile ollpd = new OverrideLowLevelDetailProfile(Include.NoRID);
                                    string productId = Convert.ToString(gridRow.Cells["Merchandise"].Value, CultureInfo.CurrentCulture);
                                    ollpd.Key = Convert.ToInt32(gridRow.Cells["Key"].Value);
                                    ollpd.Model_RID = _ollRID;
                                    ollpd.Version_RID = versionValue;
                                    ollpd.Exclude_Ind = excludeValue;

                                    foreach (OverrideLowLevelDetailProfile detailItem in ollp_Holder.DetailList)
                                    {
                                       if (detailItem.Key == ollpd.Key)
                                       {
                                           _ollp.DetailList.Remove(detailItem);
                                       }
                                    }
                                    _ollp.DetailList.Add(ollpd);
                                }
                            }
                        }
                        else
                        {
                            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                            //_ollp.DetailList.Clear();
                            if (_showLowLevels)
                            {
                                _ollp.DetailList.Clear();
                            }
                            // End TT#988
                            foreach (UltraGridRow gridRow in ugModel.Rows)
                            {
                                bool originalVersionChanged = false;
                                if (Convert.ToInt32(gridRow.Cells["OriginalVersion"].Value) != Convert.ToInt32(gridRow.Cells["Version"].Value))
                                {
                                    originalVersionChanged = true;
                                }

                                bool originalExcludeChanged = false;
                                if (Convert.ToBoolean(gridRow.Cells["OriginalExclude"].Value) != Convert.ToBoolean(gridRow.Cells["Exclude"].Value))
                                {
                                    originalExcludeChanged = true;
                                }

                                if (originalVersionChanged || originalExcludeChanged)
                                {
                                    OverrideLowLevelDetailProfile ollpd = new OverrideLowLevelDetailProfile(Include.NoRID);
                                    string productId = Convert.ToString(gridRow.Cells["Merchandise"].Value, CultureInfo.CurrentCulture);
                                    ollpd.Key = Convert.ToInt32(gridRow.Cells["Key"].Value);
                                    ollpd.Model_RID = _ollRID;
                                    ollpd.Version_RID = Convert.ToInt32(gridRow.Cells["Version"].Value);
                                    ollpd.Exclude_Ind = Convert.ToBoolean(gridRow.Cells["Exclude"].Value);

                                    _ollp.DetailList.Add(ollpd);
                                }
                            }
                        }

						_ollp.WriteProfileWork(ref _last_custom_user_RID, SAB.ClientServerSession.UserRID);

                        //---Determine If This Is A New Key-----------------
                        bool foundIt = false;
                        if (_overrideProfiles != null)
                        {
                            foreach (OverrideLowLevelProfile ollp in _overrideProfiles)
                            {
                                if (ollp.Key == _ollp.Key)
                                {
                                    foundIt = true;
                                    break;
                                }
                            }
                        }

                        if (!foundIt && _ollp.Name == "Custom")
                        {
                            _pendingCustomKey = true;
                        }

                        if (!foundIt && _ollp.Name == "")
                        {
                            _pendingNewKey = true;
                        }

                    }

                    _ollRID = _ollp.Key;
                    LoadOverrideProfiles();
                }
                return true;
            }
            catch (DatabaseForeignKeyViolation)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch
            {
                throw;
                return false;
            }
        }

        private void ProcessNewOLLID(string newNodeName)
        {
            if (cbModelName.SelectedIndex == -1)
            {
                if (newNodeName == "Custom")
                {
                    //----Custom Record--------------------------
                    _ollp.Name = newNodeName;

                    _ollp.ModelChangeType = eChangeType.add;
                    SaveDBChanges(false, false, false); //<--The information set in the populate commands above
                    //   must have their info saved to the database before
                    //   the GetOverrideList will work to fill the fill the 
                    //   ugModel grid.

                    _pendingCustomKey = true;
                    _last_custom_user_RID = _ollRID;
                }
                else
                {
                    //----Temp Record--------------------------
                    if (_ollp.Name == null)
                    {
                        _ollp.Name = "";
                    }

                    _ollp.ModelChangeType = eChangeType.add;
                    SaveDBChanges(false, false, false); //<--The information set in the populate commands above
                    //   must have their info saved to the database before
                    //   the GetOverrideList will work to fill the fill the 
                    //   ugModel grid.

                    _pendingNewKey = true;
                    _last_new_user_RID = _ollRID;
                }
            }

            cboHighLevel.Enabled = true;
            cboLowLevel.Enabled = true;
            cboHighLevelMerchandise.Enabled = true;

            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
            _rebuildNodeList = true;
            // End TT#988
            LoadGrdLowLevelNodes();
            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
            _rebuildNodeList = false;
            // End TT#988
        }

        private void txtMerchandise_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtMerchandise_Validating(object sender, CancelEventArgs e)
        {
            string productID;
            if (txtMerchandise.Modified)
            {
                if (txtMerchandise.Text.Trim().Length > 0)
                {
                    productID = txtMerchandise.Text.Trim();
                    _nodeRID = GetNodeText(ref productID);
                    if (_nodeRID == Include.NoRID)
                    {
                        MessageBox.Show(productID + " is not valid; please enter or drag and drop a node from the tree", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        //---Put Old Value BackColor Into Merchandise_DragEnter Box------
                        if (Convert.ToInt32(txtMerchandise.Tag) != Include.NoRID)
                        {
                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(txtMerchandise.Tag));
                            txtMerchandise.Text = hnp.Text;
                            txtMerchandise.Tag = hnp.Key;
                            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                            cbxActiveOnly.Checked = false;
                            // End TT#988
                        }

                        ChangePending = false;

                        SetSelectedModelItem(_ollRID);
                    }
                    else
                    {
                        _ollp.DeleteModelDetailsWork(_ollRID);

                        if (_pendingNewKey)
                        {
                            if (_last_new_user_RID != Include.NoRID)
                            {
                                _ollp.DeleteModelWork(_last_new_user_RID);  //<--Delete Old Model From Temp Table

                                _pendingNewKey = false;
                                _last_new_user_RID = Include.NoRID;
                            }
                        }

                        txtMerchandise.Text = productID;
                        txtMerchandise.Tag = _nodeRID;
                        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                        cbxActiveOnly.Checked = false;
                        // End TT#988

                        _validatingOnTxtMerchandise = true;
                        if (_nodeRID != Include.NoRID)
                        {
                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_nodeRID);
                            PopulateFromToLevels(hnp, cboHighLevel.ComboBox, 0, true);
                            PopulateFromToLevels(hnp, cboLowLevel.ComboBox, 0, true);
                            LoadCboHighLevelMerchandise();
                        }
                        _validatingOnTxtMerchandise = false;

                        ProcessNewOLLID("");
                    }
                }
                else
                {
                    txtMerchandise.Tag = null;
                }
            }
        }

        private void LoadModelDropdown()
        {
            _loadingModelDropdown = true;
            cbModelName.Items.Clear();
            //SecurityAdmin secAdmin = new SecurityAdmin(); //TT#827-MD -jsobek -Allocation Reviews Performance

            ////---Add Custom First--------------------------------------
            //foreach (OverrideLowLevelProfile ollp in _overrideProfiles)
            //{
            //    string newName = ollp.Name;
            //    switch (ollp.User_RID)
            //    {
            //        case Include.CustomUserRID:
            //            newName = newName + " (custom)";
            //            cbModelName.Items.Add(new ComboObject(ollp.Key, newName));
            //            break;
            //    }
            //}

            //---Add Global & Users After Custom-----------------------
            foreach (OverrideLowLevelProfile ollp in _overrideProfiles)
            {
                string newName = ollp.Name;
                switch (ollp.User_RID)
                {
                    // Begin TT#1125 - JSmith - Global/User should be consistent
                    //case Include.GlobalUserRID:
                    //    newName = newName + " (global)";
                    //    cbModelName.Items.Add(new ComboObject(ollp.Key, newName));
                    //    break;
                    //case Include.CustomUserRID:
                    //    newName = newName + " (custom)";
                    //    cbModelName.Items.Add(new ComboObject(ollp.Key, newName));
                    //    break;
                    //default:
                    //    newName = newName + " (" + secAdmin.GetUserName(ollp.User_RID) + ")";
                    //    cbModelName.Items.Add(new ComboObject(ollp.Key, newName));
                    //    break;
                    case Include.GlobalUserRID:
                        cbModelName.Items.Add(new ComboObject(ollp.Key, newName));
                        break;
                    case Include.CustomUserRID:
                        newName = newName + " (Custom)";
                        cbModelName.Items.Add(new ComboObject(ollp.Key, newName));
                        break;
                    default:
                        //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                        //newName = newName + " (" + secAdmin.GetUserName(ollp.User_RID) + ")";
                        newName = newName + " (" + UserNameStorage.GetUserName(ollp.User_RID) + ")";
                        //End TT#827-MD -jsobek -Allocation Reviews Performance
                        cbModelName.Items.Add(new ComboObject(ollp.Key, newName));
                        break;
                    // End TT#1125
                }
            }
            _loadingModelDropdown = false;

            // AdjustTextWidthComboBox_DropDown(cbModelName);  TT#1638 - MD - Revised Model Save - RBeck  
        }

        private void PopulateFromToLevels(HierarchyNodeProfile aHierarchyNodeProfile, ComboBox aComboBox, int toOffset, bool setItemIndex)
        {
            try
            {
                _populatingFromToLevels = true;
                HierarchyProfile hierProf;
                object oldSelectedItem = null;
                if (aComboBox.SelectedItem != null)
                {
                    oldSelectedItem = aComboBox.SelectedItem;
                }
                aComboBox.Items.Clear();

                int offset = 0;
                int fromLimit = 0;
                if (aComboBox.Name == "cboHighLevel")
                {
                    fromLimit = -1;
                    offset = 0;
                }
                else
                {
                    fromLimit = 0;
                    offset = 1;
                }

                if (aHierarchyNodeProfile != null)
                {
                    hierProf = SAB.HierarchyServerSession.GetHierarchyData(aHierarchyNodeProfile.HierarchyRID);
                    if (hierProf.HierarchyType == eHierarchyType.organizational)
                    {
                        for (int i = aHierarchyNodeProfile.HomeHierarchyLevel + offset; i <= hierProf.HierarchyLevels.Count + fromLimit; i++)
                        {
                            if (i == 0) // hierarchy
                            {
                                if (aComboBox.Name == "cboHighLevel")
                                {
                                    aComboBox.Items.Add(
                                            new HighLevelCombo(eHighLevelsType.HierarchyLevel,
                                            0,
                                            0,
                                            hierProf.HierarchyID));
                                }
                                else
                                {
                                    if (cboHighLevel.Items.Count > 0)
                                    {
                                        aComboBox.Items.Add(
                                               new LowLevelCombo(eLowLevelsType.HierarchyLevel,
                                               0,
                                               0,
                                               hierProf.HierarchyID));
                                    }
                                }
                            }
                            else
                            {
                                HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[i];
                                if (hlp != null)
                                {
                                    if (aComboBox.Name == "cboHighLevel")
                                    {
                                        aComboBox.Items.Add(
                                            new HighLevelCombo(eHighLevelsType.HierarchyLevel,
											//Begin Track #5866 - JScott - Matrix Balance does not work
											//0,
											i - aHierarchyNodeProfile.HomeHierarchyLevel,
											//End Track #5866 - JScott - Matrix Balance does not work
											hlp.Key,
                                            hlp.LevelID));
                                    }
                                    else
                                    {
                                        if (cboHighLevel.Items.Count > 0)
                                        {
                                            aComboBox.Items.Add(
                                                new LowLevelCombo(eLowLevelsType.HierarchyLevel,
												//Begin Track #5866 - JScott - Matrix Balance does not work
												//0,
												i - aHierarchyNodeProfile.HomeHierarchyLevel,
												//End Track #5866 - JScott - Matrix Balance does not work
												hlp.Key,
                                                hlp.LevelID));
                                        }
                                    }
                                }
                                else
                                {
                                    if (aComboBox.Name == "cboHighLevel")
                                    {
                                        cboHighLevel.Items.Clear();
                                    }
                                    else
                                    {
                                        cboLowLevel.Items.Clear();
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        HierarchyProfile mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();

                        int highestGuestLevel = SAB.HierarchyServerSession.GetHighestGuestLevel(aHierarchyNodeProfile.Key);

                        // add guest levels to comboBox
                        if (highestGuestLevel != int.MaxValue)
                        {
                            //for (int i = highestGuestLevel; i <= mainHierProf.HierarchyLevels.Count; i++)
                            //{
                            //    if (i == 0)
                            //    {
                            //        if (aComboBox.Name == "cboHighLevel")
                            //        {
                            //            aComboBox.Items.Add(
                            //                new HighLevelCombo(eHighLevelsType.HierarchyLevel,
                            //                0,
                            //                0,
                            //                "Root"));
                            //        }
                            //        else 
                            //        {
                            //            aComboBox.Items.Add(
                            //                new LowLevelCombo(eLowLevelsType.HierarchyLevel,
                            //                0,
                            //                0,
                            //                "Root"));
                            //        }
                            //    }
                            //    else
                            //    {
                            //        HierarchyLevelProfile hlp = (HierarchyLevelProfile)mainHierProf.HierarchyLevels[i];
                            //        if (aComboBox.Name == "cboHighLevel")
                            //        {
                            //            aComboBox.Items.Add(
                            //            new HighLevelCombo(eHighLevelsType.HierarchyLevel,
                            //            0,
                            //            hlp.Key,
                            //            hlp.LevelID));
                            //        }
                            //        else 
                            //        {
                            //            aComboBox.Items.Add(
                            //            new LowLevelCombo(eLowLevelsType.HierarchyLevel,
                            //            0,
                            //            hlp.Key,
                            //            hlp.LevelID));
                            //        }
                            //    }
                            //}
                        }

                        // add offsets to comboBox
                        
                        //BEGIN TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                        //int longestBranchCount = SAB.HierarchyServerSession.GetLongestBranch(aHierarchyNodeProfile.Key);
                        DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(aHierarchyNodeProfile.Key);
                        int longestBranchCount = hierarchyLevels.Rows.Count - 1;
                        //END TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                        if (aComboBox.Name == "cboHighLevel")
                        {
                            offset = -1;
                            longestBranchCount = longestBranchCount + 1;
                        }
                        else
                        {
                            offset = 0;
                        }

                        for (int i = 0; i < longestBranchCount + fromLimit; i++)
                        {
                            ++offset;
                            if (aComboBox.Name == "cboHighLevel")
                            {
                                aComboBox.Items.Add(
                                new HighLevelCombo(eHighLevelsType.LevelOffset,
                                offset,
                                0,
                                null));
                            }
                            else
                            {
                                aComboBox.Items.Add(
                                new LowLevelCombo(eLowLevelsType.LevelOffset,
                                offset,
                                0,
                                null));
                            }
                        }
                    }

                    if (aComboBox.Items.Count > 0 && setItemIndex)
                    {
                        if (toOffset > 0)
                        {
                            int count = aComboBox.Items.Count;
                            for (int i = 0; i < toOffset; i++)
                            {
                                aComboBox.Items.RemoveAt(0);
                            }

                            if (oldSelectedItem != null && aComboBox.Items.IndexOf(oldSelectedItem) > -1)
                            {
                                aComboBox.SelectedIndex = aComboBox.Items.IndexOf(oldSelectedItem);
                            }
                            else
                            {
                                if (aComboBox.Items.Count > 0)
                                {
                                    aComboBox.SelectedIndex = 0;
                                }
                            }
                        }
                        else
                        {
                            if (oldSelectedItem != null && aComboBox.Items.IndexOf(oldSelectedItem) > -1 && aComboBox.Name == "cboLowLevel")
                            {
                                aComboBox.SelectedIndex = aComboBox.Items.IndexOf(oldSelectedItem);
                            }
                            else
                            {
                                if (aComboBox.Name == "cboLowLevel")
                                {
                                    //---Try To Set SelectedIndex Of LowLevel To Incomming Text---
                                    bool foundIt = false;
                                    foreach (LowLevelCombo cboItem in aComboBox.Items)
                                    {
                                        if (cboItem.ToString() == _lowLevelText)  // TT#55 - KJohnson - Override Level option needs to reflect Low level already selected(in all review screens and methods with override level option)
                                        {
                                            aComboBox.SelectedIndex = cboLowLevel.Items.IndexOf(cboItem);
                                            foundIt = true;
                                            break;
                                        }
                                    }

                                    if (!foundIt)
                                    {
                                        if (aComboBox.Items.Count > 0)
                                        {
                                            aComboBox.SelectedIndex = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    if (aComboBox.Items.Count > 0)
                                    {
                                        aComboBox.SelectedIndex = 0;
                                    }
                                }
                            }
                        }

                    }
                }
                _populatingFromToLevels = false;

                //AdjustTextWidthComboBox_DropDown(aComboBox);TT#1638 - MD - Revised Model Save - RBeck
            }
            catch (Exception exc)
            {
                _populatingFromToLevels = false;
                string message = exc.ToString();
                throw;
            }
        }

        private void LoadGrdLowLevelNodes()
        {
            try
            {
                // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                if (cboHighLevel.SelectedItem == null ||
                    cboLowLevel.SelectedItem == null)
                {
                    return;
                }

                int highLevelOffset = cboHighLevel.Items.IndexOf(new HighLevelCombo(((HighLevelCombo)cboHighLevel.SelectedItem).HighLevelType, ((HighLevelCombo)cboHighLevel.SelectedItem).HighLevelOffset, ((HighLevelCombo)cboHighLevel.SelectedItem).HighLevelSequence, ""));
                int lowLevelOffset = cboLowLevel.Items.IndexOf(new LowLevelCombo(((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelType, ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelOffset, ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelSequence, ""));

                // Begin TT#1994 - JSmith - Merchandise list does not change when select different Low Level.
                if (!_forceRebuildNodes)
                {
                    if ((_currModelRID == _ollRID &&
                        _currNodeRID == _nodeRID &&
                        _currLowLevelOffset == lowLevelOffset &&
                        _currHighLevelOffset == highLevelOffset &&
                        _currHighLevelNodeRID == _highLevelnodeRID) ||
                        !_rebuildNodeList)
                    {
                        return;
                    }
                    else if (_loadingCboHighLevelMerchandise ||
                        _populatingFromToLevels)
                    {
                        return;
                    }
                }
                // End TT#1994

                _currModelRID = _ollRID;
                _currNodeRID = _nodeRID;
                _currLowLevelOffset = lowLevelOffset;
                _currHighLevelOffset = highLevelOffset;
                _currHighLevelNodeRID = _highLevelnodeRID;
                // End TT#988

                if (_allColorDataTable != null)
                {
                    _allColorDataTable.Rows.Clear();
                    _allColorDataTable.AcceptChanges();
                }

                if (cboLowLevel.SelectedIndex == -1)
                {
                    return;
                }

                _loadingGrdLowLevelNodes = true;
                _allColorDataTable = new System.Data.DataTable("allColorDataTable");
                _allColorDataTable.Locale = CultureInfo.InvariantCulture;
                DataColumn dataColumn;
                DataRow dRow;

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Key";
                dataColumn.Caption = "Key";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                dataColumn.AllowDBNull = true;
                _allColorDataTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.String");
                dataColumn.ColumnName = "Merchandise";
                dataColumn.Caption = "Merchandise";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                dataColumn.AllowDBNull = true;
                _allColorDataTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "OriginalVersion";
                dataColumn.Caption = "OriginalVersion";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                dataColumn.AllowDBNull = true;
                _allColorDataTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Version";
                dataColumn.Caption = "Version";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                dataColumn.AllowDBNull = true;
                _allColorDataTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "OriginalExclude";
                dataColumn.Caption = "OriginalExclude";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                dataColumn.AllowDBNull = true;
                _allColorDataTable.Columns.Add(dataColumn);

                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Exclude";
                dataColumn.Caption = "Exclude";
                dataColumn.ReadOnly = false;
                dataColumn.Unique = false;
                dataColumn.AllowDBNull = true;
                _allColorDataTable.Columns.Add(dataColumn);

                // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                dataColumn = new DataColumn();
                dataColumn.DataType = System.Type.GetType("System.Int32");
                dataColumn.ColumnName = "Inactive";
                dataColumn.Caption = "Inactive";
                dataColumn.ReadOnly = true;
                dataColumn.Unique = false;
                dataColumn.AllowDBNull = true;
                _allColorDataTable.Columns.Add(dataColumn);
                // End TT#988

                HierarchySessionTransaction hierTran = new HierarchySessionTransaction(SAB);
                HierarchyProfile hier = SAB.HierarchyServerSession.GetMainHierarchyData();

                // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                //int highLevelOffset = cboHighLevel.Items.IndexOf(new HighLevelCombo(((HighLevelCombo)cboHighLevel.SelectedItem).HighLevelType, ((HighLevelCombo)cboHighLevel.SelectedItem).HighLevelOffset, ((HighLevelCombo)cboHighLevel.SelectedItem).HighLevelSequence, ""));
                //int lowLevelOffset = cboLowLevel.Items.IndexOf(new LowLevelCombo(((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelType, ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelOffset, ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelSequence, ""));
                // End TT#988

                LowLevelVersionOverrideProfileList overrideList = hierTran.GetOverrideList(_ollRID,
                                                                  _nodeRID,
                                                                  Include.NoRID,
                                                                  lowLevelOffset + highLevelOffset + 1,
                                                                  _highLevelnodeRID,
                                                                  true,
                                                                  false,
                                                                  true);

                // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                _showLowLevels = true;
                if (overrideList.Count > 500)
                {
                    string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoItemsExceedsMaximum, overrideList.Count.ToString());
                    if (MessageBox.Show(message, "Continue", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        ugModel.DataSource = null;
                        _loadingGrdLowLevelNodes = false;
                        _showLowLevels = false;
                        return;
                    }
                }
                // End TT#988

                foreach (LowLevelVersionOverrideProfile llvop in overrideList)
                {
                    if (llvop.NodeProfile != null)
                    {
                        HierarchyNodeProfile hnp = llvop.NodeProfile;

                        if ((hnp.LevelType == eHierarchyLevelType.Color) && (hnp.ColorOrSizeCodeRID == 0))
                        {
                            // Through Out "UNKNOWN Color" (don't add this record)
                        }
                        else
                        {
                            dRow = _allColorDataTable.NewRow();
                            dRow["Key"] = hnp.Key;
                            dRow["Merchandise"] = hnp.Text;

                            //------Check Version-------------
                            dRow["OriginalVersion"] = llvop.VersionProfile.Key;
                            dRow["Version"] = llvop.VersionProfile.Key;

                            //------Check Exclude-------------
                            if (llvop.Exclude == true)
                            {
                                dRow["OriginalExclude"] = true;
                                dRow["Exclude"] = true;
                            }
                            else
                            {
                                dRow["OriginalExclude"] = false;
                                dRow["Exclude"] = false;
                            }

                            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                            if (!hnp.Active)
                            {
                                dRow["Inactive"] = true;
                            }
                            else
                            {
                                dRow["Inactive"] = false;
                            }
                            // End TT#988

                            _allColorDataTable.Rows.Add(dRow);
                        }
                    }
                }

                ugModel.DataSource = null;
                this.ugModel.DataSource = _allColorDataTable;
                this.ugModel.DisplayLayout.GroupByBox.Hidden = true;
                this.ugModel.DisplayLayout.GroupByBox.Prompt = "";
                this.ugModel.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;

                GridCellTag cellTag;
                foreach (UltraGridRow gridRow in ugModel.Rows)
                {
                    gridRow.Cells["Merchandise"].Activation = Activation.Disabled;
                    // Begin TT#1479 - JSmith - Inactive Field on Override Low Level displays "Must be an Integer"
                    gridRow.Cells["Inactive"].Activation = Activation.NoEdit;
                    // End TT#1479

                    foreach (LowLevelVersionOverrideProfile llvop in overrideList)
                    {
                        if (llvop.NodeProfile != null)
                        {
                            HierarchyNodeProfile hnp = llvop.NodeProfile;
                            if (Convert.ToInt32(hnp.Key) == Convert.ToInt32(gridRow.Cells["Key"].Value))
                            {
                                //----Set Version Description--------------------
                                cellTag = new GridCellTag();
                                if (llvop.VersionOverrideNodeProfile != null)
                                {
                                    cellTag.HelpText = llvop.VersionOverrideNodeProfile.Text;
                                }
                                gridRow.Cells["Version"].Tag = cellTag;

                                //----Set Version Appearance Image--------------------
                                if (llvop.VersionIsOverridden == true)
                                {
                                    gridRow.Cells["Version"].Appearance.Image = InheritanceImage;
                                }
                                else
                                {
                                    gridRow.Cells["Version"].Appearance.Image = null;
                                }

                                //----Set Exclude Description--------------------
                                cellTag = new GridCellTag();
                                if (llvop.ExcludeOverrideNodeProfile != null)
                                {
                                    cellTag.HelpText = llvop.ExcludeOverrideNodeProfile.Text;
                                }
                                gridRow.Cells["Exclude"].Tag = cellTag;

                                //----Set Exclude Appearance Image--------------------
                                if (llvop.ExcludeIsOverridden == true)
                                {
                                    gridRow.Cells["Exclude"].Appearance.Image = InheritanceImage;
                                }
                                else
                                {
                                    gridRow.Cells["Exclude"].Appearance.Image = null;
                                }

                                break;
                            }
                        }
                    }
                }
                _loadingGrdLowLevelNodes = false;
                _allColorDataTable.AcceptChanges();
            }
            catch (Exception exc)
            {
                _loadingGrdLowLevelNodes = false;
                string message = exc.ToString();
                throw;
            }
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
        public override void ShowInUse()
        {
            var emp = new OverrideLowLevelProfile(_currModelRID);
            eProfileType type = emp.ProfileType;
            int rid = _currModelRID;
            base.ShowInUse(type, rid, inQuiry2);
        }
        //END TT#110-MD-VStuart - In Use Tool

        private void LoadCboHighLevelMerchandise()
        {
            LowLevelVersionOverrideProfileList overrideList = null;
            HierarchySessionTransaction hTran = new HierarchySessionTransaction(this.SAB);

            if (cboHighLevel.SelectedIndex == -1)
            {
                return;
            }

            _loadingCboHighLevelMerchandise = true;

            int offset = cboHighLevel.Items.IndexOf(new HighLevelCombo(((HighLevelCombo)cboHighLevel.SelectedItem).HighLevelType, ((HighLevelCombo)cboHighLevel.SelectedItem).HighLevelOffset, ((HighLevelCombo)cboHighLevel.SelectedItem).HighLevelSequence, ""));
            //HierarchyNodeList levelNodes = new HierarchyNodeList(eProfileType.HierarchyNode);
            if (offset > 0)
                //levelNodes = _SAB.HierarchyServerSession.GetDescendantData(_nodeRID, offset, false, eNodeSelectType.All);
                overrideList = hTran.GetOverrideList(_ollRID, _nodeRID, _versionRID,
                                                                               offset, _nodeRID, true, false, true);


            //if (levelNodes.ArrayList.Count > 0)
            //{
            //    cboHighLevelMerchandise.Items.Clear();
            //    foreach (HierarchyNodeProfile hnp in levelNodes)
            //    {
            //        cboHighLevelMerchandise.Items.Add(new ComboObject((int)hnp.Key, hnp.Text));
            //    }
            //}
            if (overrideList != null &&
                overrideList.ArrayList.Count > 0)
            {
                cboHighLevelMerchandise.Items.Clear();
                foreach (LowLevelVersionOverrideProfile llvop in overrideList)
                {
                    if (!llvop.Exclude)
                    {
                        cboHighLevelMerchandise.Items.Add(new ComboObject((int)llvop.NodeProfile.Key, llvop.NodeProfile.Text));
                    }
                }
            }
            else
            {
                cboHighLevelMerchandise.Items.Clear();
                cboHighLevelMerchandise.Items.Add(new ComboObject((int)txtMerchandise.Tag, txtMerchandise.Text));
            }

            if (cboHighLevelMerchandise.Items.Count > 0)
            {
                cboHighLevelMerchandise.SelectedIndex = 0;
            }

            _loadingCboHighLevelMerchandise = false;

            //AdjustTextWidthComboBox_DropDown(cboHighLevelMerchandise);TT#1638 - MD - Revised Model Save - RBeck
        }

        private void SetScreenRadioBoxes()
        {
            switch (_ollp.User_RID)
            {
                case Include.GlobalUserRID:
                    radGlobal.Checked = true;
                    radUser.Checked = false;
                    radCustom.Checked = false;
                    break;
                case Include.CustomUserRID:
                    radGlobal.Checked = false;
                    radUser.Checked = false;
                    radCustom.Checked = true;
                    break;
                default:
                    radGlobal.Checked = false;
                    radUser.Checked = true;
                    radCustom.Checked = false;
                    break;
            }
        }

        private int GetNodeText(ref string aProductID)
        {
            string desc = string.Empty;
            try
            {
                string productID = aProductID;
                string[] pArray = productID.Split(new char[] { '[' });
                productID = pArray[0].Trim();

                HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                EditMsgs em = new EditMsgs();
                HierarchyNodeProfile hnp = hm.NodeLookup(ref em, productID, false);
                if (hnp.Key == Include.NoRID)
                    return Include.NoRID;
                else
                {
                    aProductID = hnp.Text;
                    return hnp.Key;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return Include.NoRID;
            }
        }

        private void cboHighLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                if (_populatingFromToLevels == false)
                {
                    _ollp.ModelChangeType = eChangeType.update;
                    SaveDBChanges(false, false, false);
                }
            }

            if (_nodeRID != Include.NoRID)
            {
                LoadCboHighLevelMerchandise();
                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_nodeRID);
                if (!_populatingFromToLevels)
                {
                    if (cboHighLevel.SelectedIndex >= 0)
                    {
                        //--Subtract Off Top Items In cboLowLevel------
                        PopulateFromToLevels(hnp, cboLowLevel.ComboBox, cboHighLevel.SelectedIndex, true);
                    }
                }
            }

            if (FormLoaded)
            {

                ChangePending = true;

                SetSecurityOnForm();

                btnNew.Enabled = false;
                //btnSaveAs.Enabled = false;
                btnDelete.Enabled = false;
                btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool

                cbxSEInheritFromHigherLevel.Checked = false;
                cbxSEApplyToLowerLevels.Checked = false;
            }

            // Begin TT#3970 - DOConnell - Changing High Level selection does not trigger Merchandise to refresh.
            _rebuildNodeList = true;
			
            LoadGrdLowLevelNodes();
			
            _rebuildNodeList = false;
			// END TT#3970 - DOConnell - Changing High Level selection does not trigger Merchandise to refresh.
        }

        private void cboLowLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (FormLoaded)
                {
                    if (_populatingFromToLevels == false)
                    {
                        _ollp.ModelChangeType = eChangeType.update;
                        SaveDBChanges(false, false, false);
                    }

                    ChangePending = true;

                    SetSecurityOnForm();

                    btnNew.Enabled = false;
                    //btnSaveAs.Enabled = false;
                    btnDelete.Enabled = false;
                    btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool

                    cbxSEInheritFromHigherLevel.Checked = false;
                    cbxSEApplyToLowerLevels.Checked = false;
                }

                // Begin TT#1994 - JSmith - Merchandise list does not change when select different Low Level.
                _rebuildNodeList = true;
                // End TT#1994
                LoadGrdLowLevelNodes();
                // Begin TT#1994 - JSmith - Merchandise list does not change when select different Low Level.
                _rebuildNodeList = false;
                // End TT#1994

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        protected override DataTable GetFilteredModels(string overrideLowLevelModelNameFilter, bool isCaseSensitive)
        {
            try
            {
                MerchandiseHierarchyData modelsData = new MerchandiseHierarchyData();

                return modelsData.GetFilteredOverrideLowLevelModels(overrideLowLevelModelNameFilter, isCaseSensitive);
            }
            catch (Exception ex)
            {
                HandleException(ex, "GetFilteredModels");
                throw ex;
            }
        }


        override protected void LoadModelComboBox(ProfileList aModelProfileList)
        {
            //SecurityAdmin secAdmin = new SecurityAdmin(); //TT#827-MD -jsobek -Allocation Reviews Performance
            ForecastBalanceProfileList forecastBalanceProfileList = new ForecastBalanceProfileList(true);
            _ModelProfileList = aModelProfileList;
            cbModelName.Items.Clear();

            foreach (OverrideLowLevelProfile ollp in _overrideProfiles)
            {
                string newName;
                foreach (ModelName modelName in _ModelProfileList.ArrayList)
                {
                    if (ollp.Key == modelName.Key)
                    {
                        newName = ollp.Name;
                        switch (ollp.User_RID)
                        {
                            case Include.GlobalUserRID:
                                cbModelName.Items.Add(new ComboObject(ollp.Key, newName));
                                break;
                            case Include.CustomUserRID:
                                newName += "Custom";
                                cbModelName.Items.Add(new ComboObject(ollp.Key, newName));
                                break;
                            default:
                                //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                                //newName += " (" + secAdmin.GetUserName(ollp.User_RID) + ")";
                                newName += " (" + UserNameStorage.GetUserName(ollp.User_RID) + ")";
                                //End TT#827-MD -jsobek -Allocation Reviews Performance
                                cbModelName.Items.Add(new ComboObject(ollp.Key, newName));
                                break;
                        }
                    }
                }
            }
        }
//End       TT#1638 - Revised Model Save - RBeck

        //private void cbModelName_SelectedValueChanged(object sender, EventArgs e) 
        protected override void cbModelName_SelectionChangeCommitted(object sender, System.EventArgs e)
        {

            if (!_settingModelDropDownIndex)
            {
                if (FormLoaded)
                {
                    if (_loadingModelDropdown == false)
                    {
                        _ollp.ModelChangeType = eChangeType.update;
                        CheckForPendingChanges();
                    }
                }

                FormLoaded = false;

                if (cbModelName.SelectedIndex >= 0)
                {
                    this.Text = "OverrideLowLevelModel - " + cbModelName.SelectedItem.ToString(); //TT#1638 - MD - Revised Model Save - RBeck
                    
                    if (_pendingNewKey)
                    {
                        if (_last_new_user_RID != Include.NoRID)
                        {
                            _ollp.DeleteModelWork(_last_new_user_RID);  //<--Delete Old Model From Temp Table

                            _pendingNewKey = false;
                            _last_new_user_RID = Include.NoRID;
                        }
                    }

                    int old_ollRID = _ollRID;
                    _ollp = (OverrideLowLevelProfile)_overrideProfiles.FindKey(((ComboObject)cbModelName.SelectedItem).Key);
                    _ollp_Undo = (OverrideLowLevelProfile)_ollp.Copy(); //<--Save a Undo Version
                    SetSecurityOnForm();

                    //btnSave.Enabled = false;

                    if (_ollp.HN_RID != Include.NoRID)
                    {
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_ollp.HN_RID);
                        _nodeRID = _ollp.HN_RID;
                        _ollRID = _ollp.Key;
                        txtMerchandise.Text = hnp.Text;
                        txtMerchandise.Tag = hnp.Key;
                        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                        cbxActiveOnly.Checked = _ollp.ActiveOnlyInd;
                        // End TT#988
                        PopulateFromToLevels(hnp, cboHighLevel.ComboBox, 0, false);
                        cboHighLevel.SelectedIndex = cboHighLevel.Items.IndexOf(new HighLevelCombo(_ollp.HighLevelType, _ollp.HighLevelOffset, _ollp.HighLevelSeq, ""));
                        PopulateFromToLevels(hnp, cboLowLevel.ComboBox, 0, false);
                        cboLowLevel.SelectedIndex = cboLowLevel.Items.IndexOf(new LowLevelCombo(_ollp.LowLevelType, _ollp.LowLevelOffset, _ollp.LowLevelSeq, ""));
                        if (cboHighLevel.SelectedIndex > 0)
                        {
                            //--Subtract Off Top Items In cboLowLevel------
                            PopulateFromToLevels(hnp, cboLowLevel.ComboBox, cboHighLevel.SelectedIndex, true);
                        }

                        LoadCboHighLevelMerchandise();
                    }

                    if (_ollp.High_Level_HN_RID != Include.NoRID)
                    {
                        HierarchyNodeProfile hnp_high_level = SAB.HierarchyServerSession.GetNodeData(_ollp.High_Level_HN_RID);
                        cboHighLevelMerchandise.SelectedItem = new ComboObject(hnp_high_level.Key, hnp_high_level.Text);
                    }


                    //--Copy Permanent Table Data To Temp Table---
                    if ((_ollRID == _last_custom_user_RID) && (_ollRID != Include.NoRID))  //Begin Track #5921
                    {
                        if (_pendingCustomKey) 
                        {
                            if (old_ollRID != _last_custom_user_RID)
                            {
                                _ollp.DeleteModelWork(old_ollRID);  //<--Delete Old Model From Temp Table
                            }
                        }
                        else 
                        {
							
							_ollp.DeleteModelWork(old_ollRID);  //<--Delete Old Model From Temp Table
							_ollp.CopyToWorkTables(_ollRID);
                            // Begin TT#1994 - JSmith - Merchandise list does not change when select different Low Level.
                            _forceRebuildNodes = true;
                            // End TT#1994
                        }
                    }
                    else if ((_ollRID == _last_new_user_RID) && (_ollRID != Include.NoRID))  //Begin Track #5921
                    {
                        if (_pendingNewKey) 
                        {
                            //--Do Nothing----
                        } 
                        else
                        {
                            _ollp.DeleteModelWork(old_ollRID);  //<--Delete Old Model From Temp Table
                            _ollp.CopyToWorkTables(_ollRID);
                            // Begin TT#1994 - JSmith - Merchandise list does not change when select different Low Level.
                            _forceRebuildNodes = true;
                            // End TT#1994
                        }
                    }
                    else 
                    {
                        if (_pendingCustomKey)
                        {
                            if (old_ollRID != _last_custom_user_RID)
                            {
                                _ollp.DeleteModelWork(old_ollRID);  //<--Delete Old Model From Temp Table
                            }
                        }
                        else 
                        {
                            _ollp.DeleteModelWork(old_ollRID);  //<--Delete Old Model From Temp Table
                        }
                        _ollp.CopyToWorkTables(_ollRID);
                        // Begin TT#1994 - JSmith - Merchandise list does not change when select different Low Level.
                        _forceRebuildNodes = true;
                        // End TT#1994
                    }
                    // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                    _rebuildNodeList = true;
                    // End TT#988

                    LoadGrdLowLevelNodes();
                    SetScreenRadioBoxes();

                    // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                    _rebuildNodeList = false;
                    // End TT#988

                }
                else
                {
                    this.Text = "OverrideLowLevelModel"; //TT#1638 - MD - Revised Model Save - RBeck
                    
                    SetSecurityOnForm();
                    btnSave.Enabled = false;

                    ClearForm();
                }

                FormLoaded = true;
            }

            // Begin TT#1994 - JSmith - Merchandise list does not change when select different Low Level.
            _forceRebuildNodes = false;
            // End TT#1994
        }

        private void SetSecurityOnForm()
        {
            switch (_ollp.User_RID)
            {
                case Include.NoRID:
                    SetReadOnly(FunctionSecurity.AllowUpdate);
                    break;
                case Include.GlobalUserRID:
                    if (_globalSecurity.IsReadOnly)
                    {
                        SetReadOnly(FunctionSecurity.IsReadOnly);
                    }
                    else
                    {
                        SetReadOnly(FunctionSecurity.AllowUpdate);
                    }
                    break;
                case Include.CustomUserRID:
                    SetReadOnly(FunctionSecurity.AllowUpdate);
                    break;
                default:
                    if (_userSecurity.IsReadOnly)
                    {
                        SetReadOnly(FunctionSecurity.IsReadOnly);
                    }
                    else
                    {
                        SetReadOnly(FunctionSecurity.AllowUpdate);
                    }
                    break;
            }

            //---Always Turn These Off (Security Doesn't Matter)---
            radGlobal.Enabled = false;
            radUser.Enabled = false;
            radCustom.Enabled = false;

            //---Always Turn These On (Security Doesn't Matter)---
            //txtMerchandise.Enabled = true;
            btnNew.Enabled = true;
            btnCancel.Enabled = true;
            cbModelName.Enabled = true;

            //---Enable & Disable "Delete" Button------
            if (_ollp.User_RID == Include.GlobalUserRID)
            {
                //---Look For Global Security----
                if (!_globalSecurity.AllowDelete)
                {
                    btnDelete.Enabled = false;
                    btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
                }
                else
                {
                    btnDelete.Enabled = true;
                    btnInUse.Enabled = true;    //TT#110-MD-VStuart - In Use Tool
                }
            }
            else if (_ollp.User_RID == Include.CustomUserRID)
            {
                //---Look For Custom Security---
                btnDelete.Enabled = true;
                btnInUse.Enabled = true;    //TT#110-MD-VStuart - In Use Tool
            }
            else
            {
                //---Look For User Security----
                if (!_userSecurity.AllowDelete)
                {
                    btnDelete.Enabled = false;
                    btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
                }
                else
                {
                    btnDelete.Enabled = true;
                    btnInUse.Enabled = true;    //TT#110-MD-VStuart - In Use Tool
                }
            }
			// Begin Track #6259 stodd
			if (!_globalSecurity.AllowUpdate && !_userSecurity.AllowUpdate)
			{
				btnSaveAs.Enabled = false;
				btnNew.Enabled = false;
			}
			// End Track # 6259
        }

        private void ClearForm()
        {
            LoadOverrideProfiles();
            txtMerchandise.Text = "";
            // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
            cbxActiveOnly.Checked = false;
            // End TT#988
            cboHighLevel.Items.Clear();
            cboLowLevel.Items.Clear();
            cboHighLevelMerchandise.Items.Clear();
            //grdLowLevelVersions.Rows.RemoveRange(1, grdLowLevelVersions.Rows.Count - 1);
            if (_allColorDataTable != null)
            {
                _allColorDataTable.Rows.Clear();
                _allColorDataTable.AcceptChanges();
            }
            _ollp = new OverrideLowLevelProfile(Include.NoRID);
            _ollRID = Include.NoRID;
            ChangePending = false;
        }

        private bool SaveAsProcessing(ref string fileName, ref int userRID)
        {
            int localUserRID = SAB.ClientServerSession.UserRID;
            frmSaveAs formSaveAs = new frmSaveAs(SAB);
            formSaveAs.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            formSaveAs.ShowCustom = _showCustom;
            formSaveAs.ShowUserGlobal = true;

            //---Look For Global Security-----------------------------
            if (_globalSecurity.IsReadOnly)
            {
                //----Read Only--------
                formSaveAs.EnableGlobal = false;
            }

            //---Look For User Security-----------------------------
            if (_userSecurity.IsReadOnly)
            {
                //----Read Only--------
                formSaveAs.EnableUser = false;
            }

            if (formSaveAs.EnableGlobal)
            {
                formSaveAs.isGlobalChecked = true;
            }
            else if (formSaveAs.EnableCustom)
            {
                formSaveAs.isCustomChecked = true;
            }
            else if (formSaveAs.EnableUser)
            {
                formSaveAs.isUserChecked = true;
            }

            bool foundIt = false;
            do
            {

                formSaveAs.ShowDialog(this);
                fileName = formSaveAs.SaveAsName;
                if (formSaveAs.isCustomChecked)
                {
                    foundIt = false;
                }
                else
                {
                    // Begin TT#1126 - JSmith - Cannot have override low level models with the same name for global and user
                    //foundIt = _ollp.ModelNameExists(fileName);
                    if (formSaveAs.isGlobalChecked)
                    {
                        userRID = Include.GlobalUserRID;
                    }
                    else if (formSaveAs.isUserChecked)
                    {
                        userRID = localUserRID;
                    }
                    else if (formSaveAs.isCustomChecked)
                    {
                        userRID = Include.CustomUserRID;
                    }
                    foundIt = _ollp.ModelNameExists(fileName, userRID);
                    // End TT#1126
                    if (foundIt && !formSaveAs.SaveCanceled)
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NameAlreadyExist),
                            this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            while (foundIt && !formSaveAs.SaveCanceled);

            // Begin TT#1126 - JSmith - Cannot have override low level models with the same name for global and user
            //if (formSaveAs.isGlobalChecked)
            //{
            //    userRID = Include.GlobalUserRID;
            //}
            //else if (formSaveAs.isUserChecked)
            //{
            //    userRID = localUserRID;
            //}
            //else if (formSaveAs.isCustomChecked)
            //{
            //    userRID = Include.CustomUserRID;
            //}
            // Begin TT#1126
            return formSaveAs.SaveCanceled;
        }

        private void cbModelName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (FormLoaded)
            {
            }
        }

        private void mniClearAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                ChangePending = true;
                foreach (UltraGridRow gridRow in ugModel.Rows)
                {
                    gridRow.Cells["Exclude"].Value = false;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void mniSelectAll_Click(object sender, System.EventArgs e)
        {
            try
            {
                ChangePending = true;
                foreach (UltraGridRow gridRow in ugModel.Rows)
                {
                    gridRow.Cells["Exclude"].Value = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private int GetVersionRID(string aVersionDescription)
        {
            try
            {
                foreach (VersionProfile versionProfile in _versionProfList)
                {
                    if (versionProfile.Description == aVersionDescription)
                    {
                        return versionProfile.Key;
                    }
                }
                return Include.NoRID;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void txtMerchandise_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void CreateVersionComboLists()
        {
            Infragistics.Win.ValueList vl;
            Infragistics.Win.ValueListItem vli;

            try
            {
                vl = ugModel.DisplayLayout.ValueLists.Add("VersionValueList");

                vli = new Infragistics.Win.ValueListItem();
                vli.DataValue = Include.NoRID;
                vli.DisplayText = "Default Version";
                vl.ValueListItems.Add(vli);

                _versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
                foreach (VersionProfile versionProfile in _versionProfList)
                {
                    vli = new Infragistics.Win.ValueListItem();
                    vli.DataValue = versionProfile.Key;
                    vli.DisplayText = versionProfile.Description;
                    vl.ValueListItems.Add(vli);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void ugModel_CellChange(object sender, CellEventArgs e)
        {
            if (FormLoaded)
            {
                ugModel.UpdateData();

                e.Cell.Row.Cells["Version"].Appearance.Image = null;
                e.Cell.Row.Cells["Exclude"].Appearance.Image = null;

                ChangePending = true;

                SetSecurityOnForm();

                btnNew.Enabled = false;
                //btnSaveAs.Enabled = false;
                btnDelete.Enabled = false;
                btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool

                cbxSEInheritFromHigherLevel.Checked = false;
                cbxSEApplyToLowerLevels.Checked = false;
            }
        }

        private void ugModel_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
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

                // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
                //e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                // End TT#1164

                ugModel.DisplayLayout.MaxRowScrollRegions = 1;

                //Prevent the user from re-arranging columns.
                ugModel.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;
                ugModel.DisplayLayout.Bands[0].ColHeaderLines = 1;

                ugModel.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                ugModel.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;

                //Set the header captions.
                int visiblePosition = 0;

                ugModel.DisplayLayout.Bands[0].Columns["Key"].Hidden = true;

                ugModel.DisplayLayout.Bands[0].Columns["Key"].Header.VisiblePosition = ++visiblePosition;
                ugModel.DisplayLayout.Bands[0].Columns["Key"].Header.Caption = "Key";

                ugModel.DisplayLayout.Bands[0].Columns["Merchandise"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                ugModel.DisplayLayout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = ++visiblePosition;
                ugModel.DisplayLayout.Bands[0].Columns["Merchandise"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
                ugModel.DisplayLayout.Bands[0].Columns["Merchandise"].MinWidth = 100;
                // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
                //ugModel.DisplayLayout.Bands[0].Columns["Merchandise"].Width = 300;
                ugModel.DisplayLayout.Bands[0].Columns["Merchandise"].Width = 250;
                // End TT#1164

                ugModel.DisplayLayout.Bands[0].Columns["OriginalVersion"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                ugModel.DisplayLayout.Bands[0].Columns["OriginalVersion"].Header.VisiblePosition = ++visiblePosition;
                ugModel.DisplayLayout.Bands[0].Columns["OriginalVersion"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);
                ugModel.DisplayLayout.Bands[0].Columns["OriginalVersion"].MinWidth = 100;
                ugModel.DisplayLayout.Bands[0].Columns["OriginalVersion"].Width = 300;
                ugModel.DisplayLayout.Bands[0].Columns["OriginalVersion"].Hidden = true;

                ugModel.DisplayLayout.Bands[0].Columns["Version"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ugModel.DisplayLayout.Bands[0].Columns["Version"].ValueList = ugModel.DisplayLayout.ValueLists["VersionValueList"];
                ugModel.DisplayLayout.Bands[0].Columns["Version"].Header.VisiblePosition = ++visiblePosition;
                ugModel.DisplayLayout.Bands[0].Columns["Version"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);
                ugModel.DisplayLayout.Bands[0].Columns["Version"].MinWidth = 100;
                // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
                //ugModel.DisplayLayout.Bands[0].Columns["Version"].Width = 300;
                ugModel.DisplayLayout.Bands[0].Columns["Version"].Width = 100;
                // End TT#1164
                // Begin TT#155 - JSmith - Size Curve Method
                if (!_showVersion)
                {
                    ugModel.DisplayLayout.Bands[0].Columns["Version"].Hidden = true;
                }
                // End TT#155

                ugModel.DisplayLayout.Bands[0].Columns["OriginalExclude"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
                ugModel.DisplayLayout.Bands[0].Columns["OriginalExclude"].Header.VisiblePosition = ++visiblePosition;
                ugModel.DisplayLayout.Bands[0].Columns["OriginalExclude"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Exclude);
                ugModel.DisplayLayout.Bands[0].Columns["OriginalExclude"].MinWidth = 55;
                ugModel.DisplayLayout.Bands[0].Columns["OriginalExclude"].Hidden = true;

                ugModel.DisplayLayout.Bands[0].Columns["Exclude"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                ugModel.DisplayLayout.Bands[0].Columns["Exclude"].Header.VisiblePosition = ++visiblePosition;
                ugModel.DisplayLayout.Bands[0].Columns["Exclude"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Exclude);
                ugModel.DisplayLayout.Bands[0].Columns["Exclude"].MinWidth = 55;

                // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                ugModel.DisplayLayout.Bands[0].Columns["Inactive"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                ugModel.DisplayLayout.Bands[0].Columns["Inactive"].Header.VisiblePosition = ++visiblePosition;
                ugModel.DisplayLayout.Bands[0].Columns["Inactive"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Inactive);
                ugModel.DisplayLayout.Bands[0].Columns["Inactive"].MinWidth = 55;
                // End TT#988

                // Begin TT#1278-MD - JSmith - Merchandise in different order when drag merchandise to field
                ugModel.DisplayLayout.Bands[0].Columns["Merchandise"].SortIndicator = SortIndicator.Ascending;
                // End TT#1278-MD - JSmith - Merchandise in different order when drag merchandise to field

                ugModel.ContextMenu = menuListBox;
            }
            catch (Exception err)
            {
                HandleException(err);
            }
        }

        private void ugModel_AfterRowInsert(object sender, RowEventArgs e)
        {
            try
            {
                //The "IsIncluded" column is a checkbox column. Right after we inserted a
                //new row, we want to default this column to be checked (true). If we
                //don't, it will default to a grayed-out check, like the 3rd state in a 
                //tri-state checkbox, even if we explicitly set this column to be a normal
                //checkbox.

                if (e.Row.Band == ugModel.DisplayLayout.Bands[0])
                {
                }

                if (FormLoaded)
                {
                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void ugModel_MouseEnterElement(object sender, UIElementEventArgs e)
        {
            try
            {
                ShowUltraGridToolTip(ugModel, e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ugModel_AfterCellUpdate(object sender, CellEventArgs e)
        {
            bool errorFound = false;
            string errorMessage = string.Empty, productID;
            try
            {
                if (FormLoaded)
                {
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void cboHighLevelMerchandise_SelectedIndexChanged(object sender, EventArgs e)
        {
            _highLevelnodeRID = Include.NoRID;
            if (cboHighLevelMerchandise.SelectedValue != null)
            {
                _highLevelnodeRID = (int)cboHighLevelMerchandise.SelectedValue;
            }
            else
            {
                if (cboHighLevelMerchandise.SelectedItem != null)
                {
                    _highLevelnodeRID = ((ComboObject)cboHighLevelMerchandise.SelectedItem).Key;
                }
            }

            if (FormLoaded)
            {
                if (_loadingCboHighLevelMerchandise == false)
                {
                    _ollp.ModelChangeType = eChangeType.update;
                    SaveDBChanges(false, false, false);
                }

                // Begin TT#1994 - JSmith - Merchandise list does not change when select different Low Level.
                _rebuildNodeList = true;
                // End TT#1994
                LoadGrdLowLevelNodes();
                // Begin TT#1994 - JSmith - Merchandise list does not change when select different Low Level.
                _rebuildNodeList = false;
                // End TT#1994

                ChangePending = true;

                SetSecurityOnForm();

                btnNew.Enabled = false;
                //btnSaveAs.Enabled = false;
                btnDelete.Enabled = false;
                btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool

                cbxSEInheritFromHigherLevel.Checked = false;
                cbxSEApplyToLowerLevels.Checked = false;
            }
        }

        protected override void BeforeClosing()
        {
            try
            {
                base.BeforeClosing();
            }
            catch (Exception exc)
            {
                string msg = exc.ToString();
                throw;
            }
        }

        private void ugModel_ClickCellButton(object sender, CellEventArgs e)
        {

        }

        private void ugModel_CellListSelect(object sender, CellEventArgs e)
        {

        }

        private void cbxSEInheritFromHigherLevel_CheckedChanged(object sender, EventArgs e)
        {
            ChangePending = true;

            SetSecurityOnForm();

            btnNew.Enabled = false;
            //btnSaveAs.Enabled = false;
            btnDelete.Enabled = false;
            btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
        }

        private void cbxSEApplyToLowerLevels_CheckedChanged(object sender, EventArgs e)
        {
            ChangePending = true;

            SetSecurityOnForm();

            btnNew.Enabled = false;
            //btnSaveAs.Enabled = false;
            btnDelete.Enabled = false;
            btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
        }

        // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
        private void cbxActiveOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
            }
        }
        // End TT#988

        private void txtMerchandise_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;

                SetSecurityOnForm();

                btnNew.Enabled = false;
                //btnSaveAs.Enabled = false;
                btnDelete.Enabled = false;
                btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool

                cbxSEInheritFromHigherLevel.Checked = false;
                cbxSEApplyToLowerLevels.Checked = false;
            }
        }

        private void cboHighLevelMerchandise_MouseHover(object sender, EventArgs e)
        {
            string message = null;
            message = cboHighLevelMerchandise.Text;
            ShowToolTip(sender, e, message);
        }

        private void cboHighLevel_MouseHover(object sender, EventArgs e)
        {
            string message = null;
            message = cboHighLevel.Text;
            ShowToolTip(sender, e, message);
        }

        private void cboLowLevel_MouseHover(object sender, EventArgs e)
        {
            string message = null;
            message = cboLowLevel.Text;
            ShowToolTip(sender, e, message);
        }

        private void btnInUse_Click(object sender, EventArgs e)
        {

        }
//Begin     TT#1638 - MD - Revised Model Save - RBeck
        //private void cbModelName_MouseHover(object sender, EventArgs e)
        //{
        //    string message = null;
        //    message = cbModelName.Text;
        //    ShowToolTip(sender, e, message);
        //}
//End       TT#1638 - MD - Revised Model Save - RBeck
    }

    public class OverrideLowLevelCloseEventArgs : EventArgs
    {
        private int _ollRid;
        private int _customOllRid;

        public OverrideLowLevelCloseEventArgs(int OLLRid, int customOLLRid)
        {
			_ollRid = OLLRid;
			_customOllRid = customOLLRid;
        }

        public int aOllRid
        {
            get
            {

                return _ollRid;
            }
        }
        public int aCustomOllRid
        {
            get
            {

                return _customOllRid;
            }
        }
    }

}
