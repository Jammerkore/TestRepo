using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;
using MIDRetail.Common;

namespace MIDRetail.Windows.Controls
{
    public partial class filterBuilderControl : UserControl
    {
        public filterBuilderControl()
        {
            InitializeComponent();

            //ultraSplitter1.Collapsed = true;
            //ultraSplitter1.Visible = false;
            this.ultraSplitter1.Visible = true;
            this.ultraSplitter1.Collapsed = false;

            //Begin TT#1372-MD -jsobek -Filters - Move Condition Display Menu
            if (ExceptionHandler.InDebugMode)
            {
                //this.ultraToolbarsManager1.Tools["mnuDebug"].SharedProps.Visible = true;
            }
            else
            {
                this.ultraToolbarsManager1.Tools["mnuDebug"].SharedProps.Visible = false;
            }
            //End TT#1372-MD -jsobek -Filters - Move Condition Display Menu
        }
        public filterManager manager = null;
        public bool readOnly;

        public void SetManager(filter currentFilter, SessionAddressBlock SAB, System.Windows.Forms.Form calendarDateSelectorForm, SetCalendarDateRangeForPlanDelegate setCalendarDateRangeDelegate, IsStoreGroupOrLevelInUse isStoreGroupOrLevelInUse)
        {
            if (manager == null)
            {
                manager = new filterManager(currentFilter, SAB, calendarDateSelectorForm, setCalendarDateRangeDelegate);
                manager.readOnly = readOnly;
                manager.SelectedConditionChangedEvent += new filterManager.SelectedConditionChangedEventHandler(Handle_SelectedConditionChanged);
                manager.EnableConditionToolbarEvent += new filterManager.EnableConditionToolbarEventHandler(Handle_EnableConditionToolbar);
                manager.DisableConditionToolbarEvent += new filterManager.DisableConditionToolbarEventHandler(Handle_DisableConditionToolbar);
                manager.ApplyConditionChangesEvent += new filterManager.ApplyConditionChangesEventHandler(Handle_ApplyConditionChanges);
                manager.areAllElementsValidDelegate = new filterManager.AreAllElementsValidDelegate(AreAllElementsValid);
                manager.AddConditionEvent += new filterManager.AddConditionEventHandler(Handle_AddCondition); //TT#1484-MD -jsobek -Filters - Adding a condition should mark filter as changed
                manager.RemoveConditionEvent += new filterManager.RemoveConditionEventHandler(Handle_RemoveCondition); //TT#1484-MD -jsobek -Filters - Adding a condition should mark filter as changed
                manager.isStoreGroupOrLevelInUse = isStoreGroupOrLevelInUse;
                //manager.isStoreGroupLevelInUse = isStoreGroupLevelInUse;

                manager.AddConditionEvent += new filterManager.AddConditionEventHandler(Handle_AddCondition); //TT#1484-MD -jsobek -Filters - Adding a condition should mark filter as changed
                manager.RemoveConditionEvent += new filterManager.RemoveConditionEventHandler(Handle_RemoveCondition); //TT#1484-MD -jsobek -Filters - Adding a condition should mark filter as changed
              
                if (filterUtility.getTextFromHierarchyNodeDelegate == null)
                {
                    filterUtility.getTextFromHierarchyNodeDelegate = new GetTextFromHierarchyNodeDelegate(manager.GetTextFromHierarchyNode);
                }
                if (filterDataHelper.SAB == null)
                {
                    filterDataHelper.SAB = SAB;
                }
            }
            if (readOnly)
            {
                this.ultraToolbarsManager1.Tools["btnSaveFilter"].SharedProps.Enabled = false;
                this.ultraToolbarsManager1.Tools["btnSaveAs"].SharedProps.Enabled = false;
                this.ultraToolbarsManager1.Tools["btnSaveAndClose"].SharedProps.Enabled = false;
            }
            //Begin TT#1421-MD -jsobek -Header Filter - Options> Variable Display needs to be removed as it only applies to Store Filters.
            if (currentFilter.filterType == DataCommon.filterTypes.StoreFilter)
            {
                this.ultraToolbarsManager1.Tools["mnuVariableDisplay"].SharedProps.Visible = true;
            }
            else
            {
                this.ultraToolbarsManager1.Tools["mnuVariableDisplay"].SharedProps.Visible = false;
            }
            //End TT#1421-MD -jsobek -Header Filter - Options> Variable Display needs to be removed as it only applies to Store Filters.

            if (currentFilter.filterType == filterTypes.StoreGroupFilter || currentFilter.filterType == filterTypes.StoreGroupDynamicFilter)
            {
                this.ultraToolbarsManager1.Tools["mnuDebug"].SharedProps.Visible = false;
            }

        }



        public void LoadFilter(filter currentFilter, SessionAddressBlock SAB, System.Windows.Forms.Form calendarDateSelectorForm, SetCalendarDateRangeForPlanDelegate setCalendarDateRangeDelegate, IsStoreGroupOrLevelInUse isStoreGroupOrLevelInUse)
        {
            SetManager(currentFilter, SAB, calendarDateSelectorForm, setCalendarDateRangeDelegate, isStoreGroupOrLevelInUse);
            currentFilter.RebuildText(manager, currentFilter.RootConditionNode); //rebuild the formatted text so we do not have to store it on the database

            //dynamically build the toolbar
            filterToolbars filterToolbar = filterToolbars.FromFilterType(currentFilter.filterType);
            int ikey = 0;
            foreach (filterToolbarButton btn in filterToolbar.buttonList)
            {
                string toolKey = "k" + ikey.ToString();
                Infragistics.Win.UltraWinToolbars.ButtonTool t = new Infragistics.Win.UltraWinToolbars.ButtonTool(toolKey);
                t.SharedProps.AppearancesSmall.Appearance.Image = this.imageList1.Images[btn.buttonImage.Index];
                t.SharedProps.AppearancesLarge.Appearance.Image = this.imageList1.Images[btn.buttonImage.Index];
                t.SharedProps.Caption = btn.buttonText;
                if (manager.Options.ShowToolTips)
                {
                    t.SharedProps.ToolTipText = btn.toolTip;
                }
                t.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
                if (readOnly)
                {
                    t.SharedProps.Enabled = false;
                }
                //Begin TT#4980 - bvaughan - Error selecting Characteristics when no Product Characterisitcs are defined.
                else if (currentFilter.filterType == filterTypes.ProductFilter && btn.dictionaryEntry.entryType == typeof(entryProductCharacteristics))
                {
                    ProductCharProfileList productCharProfileList = SAB.HierarchyServerSession.GetProductCharacteristics();
                    if (productCharProfileList.Count == 0)
                    {
                        t.SharedProps.Enabled = false;
                    }
                }
                //End TT#4980

                this.ultraToolbarsManager1.Tools.Add(t);
                this.ultraToolbarsManager1.Toolbars["QuickAdd"].Tools.AddTool(toolKey);
                if (btn.startGroup)
                {
                    this.ultraToolbarsManager1.Toolbars["QuickAdd"].Tools[toolKey].InstanceProps.IsFirstInGroup = true;
                }
                this.ultraToolbarsManager1.Toolbars["QuickAdd"].Tools[toolKey].Tag = btn.dictionaryEntry;
                ikey++;
            }

            //pass the manager to the tree
            this.filterBuilderList1.LoadFilter(manager);  
        }
        public void SelectCondition(int levelRID, string levelName)
        {
            if (levelRID != Include.NoRID)
            {
                //this.filterBuilder1.SelectCondition(conditionRID);
                //SelectNode(manager.currentFilter.GetNameConditionSeq());
                ConditionNode cn = manager.currentFilter.FindConditionNodeByLevelRID(levelRID);
                if (cn != null)
                {
                    this.filterBuilderList1.SelectNode(cn.condition.Seq);
                }
            }
            else
            {
                ConditionNode cn = manager.currentFilter.FindSetConditionNodeByName(levelName);
                if (cn != null)
                {
                    this.filterBuilderList1.SelectNode(cn.condition.Seq);
                }
            }
        }
    


        private delegate void LoadConditionDelegate(filterManager manager, filterCondition fc);
        private delegate bool IsValidConditionDelegate(filterManager manager, filterCondition fc);
        private delegate void SaveConditionDelegate(ref filterCondition fc);
        private delegate void BuildFormattedTextDelegate(filterOptionDefinition options, ref filterCondition fc);

        private LoadConditionDelegate loadConditionDelegate;
        private IsValidConditionDelegate isValidConditionDelegate;
        private SaveConditionDelegate saveConditionDelegate;
        private BuildFormattedTextDelegate buildFormattedTextDelegate;

        private void Handle_SelectedConditionChanged(object sender, filterManager.SelectedConditionChangedEventArgs e)
        {
            //show the UI component group control for this condition type  
            filterDictionaryEntry eg = manager.currentFilter.GetEntryFromCondition(manager, e.condition);
            this.filterContainer1.MakeElementGroups(manager, eg);
            eg.LoadFromCondition(this.manager, e.condition);
            loadConditionDelegate = new LoadConditionDelegate(eg.LoadFromCondition);
            isValidConditionDelegate = new IsValidConditionDelegate(eg.IsValid);
            saveConditionDelegate = new SaveConditionDelegate(eg.SaveToCondition);
            buildFormattedTextDelegate = new BuildFormattedTextDelegate(eg.BuildFormattedText);
        }

        private void RebuildText()
        {
            foreach (DataRow dr in manager.currentFilter.GetConditionDataRows())
            {
                int seq = (int)dr["SEQ"];
                ConditionNode cn = manager.currentFilter.FindConditionNode(seq);
                filterDictionaryEntry entry = manager.currentFilter.GetEntryFromCondition(manager, cn.condition);
                entry.BuildFormattedText(manager.Options, ref cn.condition);
                //manager.currentFilter.UpdateCondition(cn.condition);
            }
            this.filterBuilderList1.RenderNodes();
        }
        private void Handle_EnableConditionToolbar(object sender, filterManager.EnableConditionToolbarEventArgs e)
        {
            this.ultraToolbarsManager2.Tools["btnCancel"].SharedProps.Enabled = true;
            this.ultraToolbarsManager2.Tools["btnApply"].SharedProps.Enabled = true;
        }
        public void Handle_DisableConditionToolbar(object sender, filterManager.DisableConditionToolbarEventArgs e)
        {
            this.ultraToolbarsManager2.Tools["btnCancel"].SharedProps.Enabled = false;
            this.ultraToolbarsManager2.Tools["btnApply"].SharedProps.Enabled = false;
        }


        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnSaveFilter":
                    Save();
                    break;
                case "btnSaveAs":
                    SaveAs();
                    break;
                case "btnSaveAndClose":
                    if (Save() == true)
                    {
                        RaiseCloseFilterEvent();
                    }
                    break;
                case "btnHtmlHelp":
                    ShowHelpPopup();
                    break;
                case "btnUnrestricted":
                    bool unrestricted = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)e.Tool).Checked;
                    if (unrestricted)
                    {
                        SetUnrestricted();
                    }
                    else
                    {
                        SetRestricted();
                    }
                    break;
                case "btnRestricted":
                    bool unrestricted2 = !((Infragistics.Win.UltraWinToolbars.StateButtonTool)e.Tool).Checked;
                    if (unrestricted2)
                    {
                        SetUnrestricted();
                    }
                    else
                    {
                        SetRestricted();
                    }
                    break;
                //case "btnShowGroupingLabels":
                //    bool showGroupingLabels = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)e.Tool).Checked;
                //    manager.Options.ShowGroupingLabels = showGroupingLabels;

                //    this.filterContainer1.ReDraw(manager.Options);
                //    break;
                case "btnShowEstimatedCost":
                    bool showEstimatedCost = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)e.Tool).Checked;
                    manager.Options.ShowEstimatedCost = showEstimatedCost;
                    RebuildText();
                    break;
                case "btnShowExecutionPlan":
                    bool showExecutionPlan = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)e.Tool).Checked;
                    manager.Options.ShowExecutionPlan = showExecutionPlan;
                    RebuildText();
                    break;
                case "btnShowMonitor":
                    bool showMonitor = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)e.Tool).Checked;

                    RaiseShowFilterMonitorEvent(showMonitor);

                    break;
                case "btnShowToolTips":
                    bool showToolTips = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)e.Tool).Checked;
                    manager.Options.ShowToolTips = showToolTips;

                    Infragistics.Win.DefaultableBoolean b;
                    if (showToolTips)
                    {
                        b = Infragistics.Win.DefaultableBoolean.True;
                    }
                    else
                    {
                        b = Infragistics.Win.DefaultableBoolean.False;
                    }

                    this.ultraToolbarsManager1.Toolbars["QuickAdd"].Settings.ShowToolTips = b;

                    break;
                case "btnVariableDisplayNameOnly":
                    manager.Options.VariableDisplayNameOnly = true;
                    manager.Options.VariableDisplayDescriptive = false;
                    RebuildText();
                    break;
                case "btnVariableDisplayShort":
                    manager.Options.VariableDisplayNameOnly = false;
                    manager.Options.VariableDisplayDescriptive = true;
                    RebuildText();
                    break;
                case "btnColorFormatBlackBlue":
                    manager.Options.ColorFormatBlackBlueRed = true;
                    manager.Options.ColorFormatGreenBlueRed = false;
                    manager.Options.ColorFormatBlackRed = false;
                    RebuildText();
                    break;
                case "btnColorFormatBlackRed":
                    manager.Options.ColorFormatBlackBlueRed = false;
                    manager.Options.ColorFormatGreenBlueRed = false;
                    manager.Options.ColorFormatBlackRed = true;
                    RebuildText();
                    break;
                case "btnColorFormatGreenBlueRed":
                    manager.Options.ColorFormatBlackBlueRed = false;
                    manager.Options.ColorFormatGreenBlueRed = true;
                    manager.Options.ColorFormatBlackRed = false;
                    RebuildText();
                    break;
                default:
                    if (e.Tool.Tag != null)
                    {
                        filterDictionary dictionaryEntry = (filterDictionary)e.Tool.Tag;
                        AddConditionFromToolbarButton(dictionaryEntry);
                    }
                    break;
            }
        }
        private void AddConditionFromToolbarButton(filterDictionary dictionaryEntry)
        {
            if (manager.IsOKToSelectNewCondition() == true)
            {
                manager.AddCondition(dictionaryEntry);
            }
        }
        private void ultraToolbarsManager1_BeforeToolExitEditMode(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolExitEditModeEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "txtMaxResults":
                    SetMaxLimit(((Infragistics.Win.UltraWinToolbars.TextBoxTool)e.Tool).Text, e);
                    break;
            }
        }

        private bool _isUnrestricted = true;
        private int _maxLimit = 5000;
        private int _messageUpperLimit = 100000;
        private void SetUnrestricted()
        {
            _isUnrestricted = true;
            DisplayCurrentResultLimit();

        }
        private void SetRestricted()
        {
            _isUnrestricted = false;
            DisplayCurrentResultLimit();
        }
        private void DisplayCurrentResultLimit()
        {
            if (_isUnrestricted)
            {
                ((Infragistics.Win.UltraWinToolbars.PopupMenuTool)this.ultraToolbarsManager1.Tools["lblCurrentResultLimit"]).SharedProps.Caption = "Unrestricted";
            }
            else
            {
                ((Infragistics.Win.UltraWinToolbars.PopupMenuTool)this.ultraToolbarsManager1.Tools["lblCurrentResultLimit"]).SharedProps.Caption = _maxLimit.ToString() + " rows";
            }
        }
        private void SetMaxLimit(string newLimitText, Infragistics.Win.UltraWinToolbars.BeforeToolExitEditModeEventArgs e)
        {
            int tempLimit;
            if (int.TryParse(newLimitText, out tempLimit) == false)
            {
                string sMsg = filterMessages.maxLimitInvalidNumber;

                if (e.ForceExit)
                {
                    e.RestoreOriginalValue = true;
                    sMsg += "\r\n" + filterMessages.maxLimitRestoringOriginal;
                }
                else
                {
                    e.Cancel = true;
                }
                MessageBox.Show(sMsg, filterMessages.maxLimitInvalidNumberTitle);
                return;
                //MIDText.GetTextOnly(eMIDTextCode.lbl_Message_Breaddown);
            }
            if (tempLimit < 50)
            {
                String sMsg = filterMessages.maxLimitLowestValue;
                if (e.ForceExit)
                {
                    e.RestoreOriginalValue = true;
                    sMsg += "\r\n" + filterMessages.maxLimitRestoringOriginal;
                }
                else
                {
                    e.Cancel = true;
                }
                MessageBox.Show(sMsg, filterMessages.maxLimitInvalidNumberTitle);
                return;
            }
            if (tempLimit > _messageUpperLimit)
            {
                String sMsg = filterMessages.maxLimitHighestValue;
                if (e.ForceExit)
                {
                    e.RestoreOriginalValue = true;
                    sMsg += "\r\n" + filterMessages.maxLimitRestoringOriginal;
                }
                else
                {
                    e.Cancel = true;
                }
                MessageBox.Show(sMsg, filterMessages.maxLimitInvalidNumberTitle);
                return;
            }
            _maxLimit = tempLimit;
            DisplayCurrentResultLimit();
        }

        public bool isNewFilter = true;
        public bool Save()	// TT#1350-MD - stodd - Prompt user when closing form when changes pending
        {
            //Begin TT#1382-MD -jsobek -Filter Name is not updated on Save & Close
            if (manager.IsOKToSelectNewCondition() == false)  //Can update the name if they did not apply changes
            {
                return false;
            }
            //End TT#1382-MD -jsobek -Filter Name is not updated on Save & Close


            //Do not allow saving if Store Load API is running
            if (manager.currentFilter.filterType == filterTypes.StoreGroupFilter || manager.currentFilter.filterType == filterTypes.StoreGroupDynamicFilter)
            {
                GenericEnqueue genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, manager.SAB.ClientServerSession.UserRID, manager.SAB.ClientServerSession.ThreadID);
                if (genericEnqueueStoreLoad.DoesHaveConflicts())
                {
                    MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(manager.SAB, genericEnqueueStoreLoad, "Store Load API");
                    return false;
                }
            }



            ConditionNode cn = manager.currentFilter.FindConditionNode(manager.currentFilter.GetNameConditionSeq());
            string filterName = cn.condition.valueToCompare; //((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Tools["txtFilterName"]).Text;
            //cn = manager.currentFilter.FindConditionNode(5);
            //string filterFolder = cn.condition.valueToCompare; //((Infragistics.Win.UltraWinToolbars.ComboBoxTool)this.ultraToolbarsManager1.Tools["cboConditionTypes"]).Text;



            if (filterName.Trim().Length == 0)
            {
                // Begin TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition
                string msgText =  MIDText.GetText(eMIDTextCode.msg_ProvideValidFilterName);
                string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_FilterNameInvalid);
                MessageBox.Show(msgText, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                // End TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition
                return false;
            }

            FilterData filterData = new FilterData();

            //if (filterData.DoesFilterNameAlreadyExist(manager.currentFilter.filterType, filterName, manager.currentFilter.filterRID) == true)
            int key = filterData.FilterGetKey(manager.currentFilter.filterType, manager.currentFilter.ownerUserRID, filterName);	// TT#1364-MD - stodd - Both store and header filters allow duplicate filter names - 
            if (key != -1 && key != manager.currentFilter.filterRID)
            {
                // Begin TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition
                string msgText = MIDText.GetText(eMIDTextCode.msg_FilterNameExists);
                string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_FilterAlreadyExists);
                MessageBox.Show(msgText, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //MessageBox.Show("A filter with that name already exists.  Please choose a different name.", "Filter Name Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                // End TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition
                return false;
            }

            // Begin TT#1927-MD - JSmith - Able to Save a Dynamic and Manual Store Attribute with the same Name.  Would not think this would be allowed.
            if (manager.currentFilter.filterType == filterTypes.StoreGroupFilter
                || manager.currentFilter.filterType == filterTypes.StoreGroupDynamicFilter)
            {
                if ((manager.currentFilter.filterRID == Include.NoRID
                    || manager.currentFilter.filterName != filterName)
                    && StoreMgmt.DoesGroupNameExist(filterName, manager.currentFilter.ownerUserRID))
                {
                    string msgText = MIDText.GetText(eMIDTextCode.msg_AttributeNameExists);
                    string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_AttributeAlreadyExists);
                    MessageBox.Show(msgText, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            // End TT#1927-MD - JSmith - Able to Save a Dynamic and Manual Store Attribute with the same Name.  Would not think this would be allowed.

            if (manager.currentFilter.filterType == filterTypes.StoreGroupFilter)
            {
                List<levelInfo> tempLevelList = new List<levelInfo>();
                int iLevelSeq = 0; 
                ConditionNode conditionRoot = manager.currentFilter.FindConditionNode(manager.currentFilter.GetRootConditionSeq());
                foreach (ConditionNode cnLevel in conditionRoot.ConditionNodes)
                {
                    if (cnLevel.condition.dictionaryIndex == filterDictionary.StoreGroupName)
                    {
                        levelInfo li = new levelInfo(-1, cnLevel.condition.valueToCompare, iLevelSeq, -1, eGroupLevelTypes.Normal, 0);
                        tempLevelList.Add(li);
                        iLevelSeq++;
                    }
                }

                if (tempLevelList.Count == 0)
                {
                    string msgText = MIDText.GetText(eMIDTextCode.msg_StoreGroupFilterMustContainOneSet); //"Attributes must have one set defined.";
                    string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_StoreGroupFilterMustContainOneSetCaption);  //Invalid Attribute Filter
                    MessageBox.Show(msgText, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                bool foundDuplicateLevelName = false;
                bool foundBlankLevelName = false;  // TT#1900-MD -JSmith - Versioning Test- Static Attribute able to save a set with a BLANK name/
                int iLevel = 0;
                //string lastLevelName = tempLevelList[0].levelName;
                while (foundDuplicateLevelName == false && iLevel <= tempLevelList.Count -1)
                {
                    // Begin TT#1900-MD -JSmith - Versioning Test- Static Attribute able to save a set with a BLANK name/
                    //string levelName = tempLevelList[iLevel].levelName;
                    string levelName = tempLevelList[iLevel].levelName.Trim();
                    if (string.IsNullOrEmpty(levelName))
                    {
                        foundBlankLevelName = true;
                    }
                    // End TT#1900-MD -JSmith - Versioning Test- Static Attribute able to save a set with a BLANK name/
                    int levelSeq = tempLevelList[iLevel].levelSeq;
                    foreach (levelInfo li in tempLevelList)
                    {
                        if (li.levelSeq != levelSeq && li.levelName == levelName)
                        {
                            foundDuplicateLevelName = true;
                        }
                    }
                    iLevel++;
                }

                if (foundDuplicateLevelName)
                {
                    string msgText = MIDText.GetText(eMIDTextCode.msg_StoreGroupFilterCannotHaveDuplicateSetNames); //"Duplicate set names are not permitted."
                    string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_StoreGroupFilterCannotHaveDuplicateSetNamesCaption); //Invalid Attribute Filter
                    MessageBox.Show(msgText, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                // Begin TT#1900-MD -JSmith - Versioning Test- Static Attribute able to save a set with a BLANK name/
                else if (foundBlankLevelName)
                {
                    string msgText = MIDText.GetText(eMIDTextCode.msg_StoreGroupFilterCannotHaveBlankSetNames); //"Blank set names are not permitted."
                    string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_StoreGroupFilterCannotHaveDuplicateSetNamesCaption); //Invalid Attribute Filter
                    MessageBox.Show(msgText, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                // End TT#1900-MD -JSmith - Versioning Test- Static Attribute able to save a set with a BLANK name/
            }
            if (manager.currentFilter.filterType == filterTypes.StoreGroupDynamicFilter)
            {
                List<levelInfo> tempLevelList = new List<levelInfo>();
                int iLevelSeq = 0;
                ConditionNode conditionRoot = manager.currentFilter.FindConditionNode(manager.currentFilter.GetRootConditionSeq());
                foreach (ConditionNode cnLevel in conditionRoot.ConditionNodes)
                {
                    if (cnLevel.condition.dictionaryIndex == filterDictionary.StoreGroupDynamic)
                    {
                        levelInfo li = new levelInfo(-1, cnLevel.condition.valueToCompare, iLevelSeq, -1, eGroupLevelTypes.Normal, cnLevel.condition.operatorIndex);
                        tempLevelList.Add(li);
                        iLevelSeq++;
                    }
                }

                if (tempLevelList.Count == 0)
                {
                    string msgText = MIDText.GetText(eMIDTextCode.msg_StoreGroupFilterMustContainOneSet); //"Attributes must have one set defined.";
                    string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_StoreGroupFilterMustContainOneSetCaption);  //Invalid Attribute Filter
                    MessageBox.Show(msgText, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                bool foundDuplicateField = false;
                int iLevel = 0;
                while (foundDuplicateField == false && iLevel <= tempLevelList.Count - 1)
                {
                    int levelVersion = tempLevelList[iLevel].levelVersion;
                    int levelSeq = tempLevelList[iLevel].levelSeq;
                    foreach (levelInfo li in tempLevelList)
                    {
                        if (li.levelSeq != levelSeq && li.levelVersion == levelVersion)
                        {
                            foundDuplicateField = true;
                        }
                    }
                    iLevel++;
                }

                if (foundDuplicateField)
                {
                    string msgText = MIDText.GetText(eMIDTextCode.msg_StoreGroupDynamicFilterCannotHaveDuplicateField); //"Cannot use the same field or characteristic in a dynamic set."
                    string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_StoreGroupDynamicFilterCannotHaveDuplicateFieldCaption); //Invalid Dynamic Attribute Filter
                    MessageBox.Show(msgText, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            manager.currentFilter.filterName = filterName;
            //manager.currentFilter.filterFolderUserOrGlobal = filterFolder;
            //manager.currentFilter.isLimited = isLimited;
            // manager.currentFilter.resultLimit = resultLimit;

            try
            {
                Cursor.Current = Cursors.WaitCursor; //TT#1313-MD -jsobek -Header Filters
                if (isNewFilter)
                {
                    InsertFilter(filterData, manager.currentFilter);
                }
                else
                {
                    UpdateFilter(filterData, manager.currentFilter);
                }
                RaiseSaveFilterEvent(manager.currentFilter, isNewFilter);
                isNewFilter = false;
                RaiseChangePendingEvent(false);  // TT#1350-MD - stodd - Prompt user when closing form when changes pending
            }
            finally
            {
                Cursor.Current = Cursors.Default; //TT#1313-MD -jsobek -Header Filters
            }
                
            return true;
          

        }

        /// <summary>
        /// Make them pick a new name before Save As
        /// </summary>
        /// <returns></returns>
        private bool SaveAs()
        {
            //Begin TT#1382-MD -jsobek -Filter Name is not updated on Save & Close
            if (manager.IsOKToSelectNewCondition() == false)  //Can update the name if they did not apply changes
            {
                return false;
            }
            //End TT#1382-MD -jsobek -Filter Name is not updated on Save & Close


            //Do not allow saving if Store Load API is running
            if (manager.currentFilter.filterType == filterTypes.StoreGroupFilter || manager.currentFilter.filterType == filterTypes.StoreGroupDynamicFilter)
            {
                GenericEnqueue genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, manager.SAB.ClientServerSession.UserRID, manager.SAB.ClientServerSession.ThreadID);
                if (genericEnqueueStoreLoad.DoesHaveConflicts())
                {
                    MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(manager.SAB, genericEnqueueStoreLoad, "Store Load API");
                    return false;
                }
            }



            ConditionNode cn = manager.currentFilter.FindConditionNode(manager.currentFilter.GetNameConditionSeq());
            string filterName = cn.condition.valueToCompare; 



            if (filterName.Trim().Length == 0)
            {
                // Begin TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition
                string msgText = MIDText.GetText(eMIDTextCode.msg_ProvideValidFilterName);
                string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_FilterNameInvalid);
                MessageBox.Show(msgText, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //MessageBox.Show("Please provide a valid filter name.", "Filter Name Invalid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                // End TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition
                return false;
            }

            FilterData filterData = new FilterData();
            int key = filterData.FilterGetKey(manager.currentFilter.filterType, manager.currentFilter.ownerUserRID, filterName);	// TT#1364-MD - stodd - Both store and header filters allow duplicate filter names - 

            if (key != -1)
            {
                // Begin TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition
                string msgText = MIDText.GetText(eMIDTextCode.msg_FilterNameExists);
                string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_FilterAlreadyExists);
                MessageBox.Show(msgText, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //MessageBox.Show("A filter with that name already exists.  Please choose a different name.", "Filter Name Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                // End TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition
                return false;
            }

            // Begin TT#1927-MD - JSmith - Able to Save a Dynamic and Manual Store Attribute with the same Name.  Would not think this would be allowed.
            if (manager.currentFilter.filterType == filterTypes.StoreGroupFilter
                || manager.currentFilter.filterType == filterTypes.StoreGroupDynamicFilter)
            {
                if (manager.currentFilter.filterName != filterName
                    && StoreMgmt.DoesGroupNameExist(filterName, manager.currentFilter.ownerUserRID))
                {
                    string msgText = MIDText.GetText(eMIDTextCode.msg_AttributeNameExists);
                    string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_AttributeAlreadyExists);
                    MessageBox.Show(msgText, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            // End TT#1927-MD - JSmith - Able to Save a Dynamic and Manual Store Attribute with the same Name.  Would not think this would be allowed.

            try
            {
                Cursor.Current = Cursors.WaitCursor; //TT#1313-MD -jsobek -Header Filters
                manager.currentFilter.filterName = filterName;
                //manager.currentFilter.filterFolderUserOrGlobal = filterFolder;
                //manager.currentFilter.isLimited = isLimited;
                // manager.currentFilter.resultLimit = resultLimit;



                //if (isNewFilter)
                //{
                    InsertFilter(filterData, manager.currentFilter);


                //}
                //else
                //{
                //    UpdateFilter(filterData, manager.currentFilter);
                //}
     
                RaiseSaveFilterEvent(manager.currentFilter, true);
                isNewFilter = false;
                RaiseChangePendingEvent(false);  // TT#1350-MD - stodd - Prompt user when closing form when changes pending
            }
            finally
            {
                Cursor.Current = Cursors.Default; //TT#1313-MD -jsobek -Header Filters
            }

            return true;


        }

        private void InsertFilter(FilterData filterData, filter f)
        {
            try
            {
                int newfilterRID = filterData.InsertFilter(f.filterType, f.userRID, f.ownerUserRID, f.filterName, f.isLimited, Convert.ToInt32(f.resultLimit));
                f.filterRID = newfilterRID;


                //this.ultraTree1.ExpandAll();
                InsertConditions(filterData, f);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        private void InsertConditions(FilterData filterData, filter f)
        {
            foreach (DataRow drCondition in f.GetConditionDataRows())
            {
                int conditionSeq = (int)drCondition["SEQ"];
                ConditionNode cn = f.FindConditionNode(conditionSeq);
                filterCondition c= cn.condition;
                int newConditionRID = filterData.InsertCondition(aFILTER_RID: f.filterRID,
                                                                aSEQ: c.Seq,
                                                                aPARENT_SEQ: c.ParentSeq,
                                                                aSIBLING_SEQ: c.SiblingSeq,
                                                                aELEMENT_GROUP_TYPE_INDEX: c.dictionaryIndex,
                                                                aLOGIC_INDEX: c.logicIndex,
                                                                aFIELD_INDEX: c.fieldIndex,
                                                                aOPERATOR_INDEX: c.operatorIndex,
                                                                aVALUE_TYPE_INDEX: c.valueTypeIndex,
                                                                aDATE_TYPE_INDEX: c.dateTypeIndex,
                                                                aNUMERIC_TYPE_INDEX: c.numericTypeIndex,
                                                                aVALUE_TO_COMPARE: c.valueToCompare,
                                                                aVALUE_TO_COMPARE_DOUBLE: c.valueToCompareDouble,
                                                                aVALUE_TO_COMPARE_DOUBLE2: c.valueToCompareDouble2,
                                                                aVALUE_TO_COMPARE_INT: c.valueToCompareInt,
                                                                aVALUE_TO_COMPARE_INT2: c.valueToCompareInt2,
                                                                aVALUE_TO_COMPARE_BOOL: c.valueToCompareBool,
                                                                aVALUE_TO_COMPARE_DATE_FROM: c.valueToCompareDateFrom,
                                                                aVALUE_TO_COMPARE_DATE_TO: c.valueToCompareDateTo,
                                                                aVALUE_TO_COMPARE_DATE_DAYS_FROM: c.valueToCompareDateBetweenFromDays,
                                                                aVALUE_TO_COMPARE_DATE_DAYS_TO: c.valueToCompareDateBetweenToDays,
                                                                aVAR1_VARIABLE_INDEX: c.variable1_Index,
                                                                aVAR1_VERSION_INDEX: c.variable1_VersionIndex,
                                                                aVAR1_HN_RID: c.variable1_HN_RID,
                                                                aVAR1_CDR_RID: c.variable1_CDR_RID,
                                                                aVAR1_VALUE_TYPE_INDEX: c.variable1_VariableValueTypeIndex,
                                                                aVAR1_TIME_INDEX: c.variable1_TimeTypeIndex,
                                                                aVAR_PERCENTAGE_OPERATOR_INDEX: c.operatorVariablePercentageIndex,
                                                                aVAR2_VARIABLE_INDEX: c.variable2_Index,
                                                                aVAR2_VERSION_INDEX: c.variable2_VersionIndex,
                                                                aVAR2_HN_RID: c.variable2_HN_RID,
                                                                aVAR2_CDR_RID: c.variable2_CDR_RID, 
                                                                aVAR2_VALUE_TYPE_INDEX: c.variable2_VariableValueTypeIndex,
                                                                aVAR2_TIME_INDEX: c.variable2_TimeTypeIndex,
                                                                aHEADER_HN_RID: c.headerMerchandise_HN_RID,
                                                                //aHEADER_PH_RID: c.headerMerchandise_PH_RID, 
                                                                aSORT_BY_TYPE_INDEX: c.sortByTypeIndex, 
                                                                aSORT_BY_FIELD_INDEX: c.sortByFieldIndex,
                                                                aLIST_VALUE_CONSTANT_INDEX: c.listConstantType.dbIndex,
                                                                aDATE_CDR_RID: c.date_CDR_RID   // TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
                                                                );
                cn.condition.conditionRID = newConditionRID;

                foreach (DataRow drlistValue in cn.condition.dtListValues.Rows)
                {
                    drlistValue["CONDITION_RID"] = newConditionRID;
                }
                if (cn.condition.dtListValues.Rows.Count > 0) //TT#1388-MD -jsobek -Product Filters
                {
                    filterData.InsertListValues(cn.condition.dtListValues);
                }

            }
        }
        private void UpdateFilter(FilterData filterData, filter f)
        {
            // Begin TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
            //filterData.UpdateFilter(f.filterRID, f.userRID, f.filterName, f.isLimited, Convert.ToInt32(f.resultLimit));
            filterData.UpdateFilter(f.filterRID, f.userRID, f.ownerUserRID, f.filterName, f.isLimited, Convert.ToInt32(f.resultLimit));
            // End TT#1907-MD - JSmith - Header Filter Change from User to Global - receive Unhandled Exception
            UpdateConditions(filterData, f);
        }
        private void UpdateConditions(FilterData filterData, filter f)
        {
            //remove conditions and readd them
            DeleteConditions(filterData, f);
            InsertConditions(filterData, f);
        }
        private void DeleteConditions(FilterData filterData, filter f)
        {
            filterData.DeleteFilterConditions(f.filterRID);
        }
        private void ShowHelpPopup()
        {
            HelpHTMLForm f = new HelpHTMLForm();
            string spath = string.Empty;

            string appStartupPath = Application.StartupPath;
            #if (DEBUG)
                            spath = System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(appStartupPath).ToString().Trim()).ToString().Trim()) + @"\ApplicationClient\HelpFiles";
            #else
                            spath = System.IO.Directory.GetParent(appStartupPath).ToString().Trim() + @"\Client\HelpFiles";
            #endif

            spath += "\\" + manager.currentFilter.filterType.helpFileName;
            f.LoadHelpFile("file:\\" + spath);
            f.Show();
        }

        private void ultraToolbarManager1_ToolValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "cboConditionTypes":
                    string conditionType = ((Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool).Text;
                    break;
            }
        }

        private void ultraToolbarsManager2_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnApply":
                    if (AreAllElementsValid())
                    {
                        ApplyConditionChanges();
                    }
                    break;
                case "btnCancel":
                    CancelConditionChanges();
                    break;
            }
        }


        private bool AreAllElementsValid()
        {
            return isValidConditionDelegate(manager, manager.currentCondition);
        }

        private void ApplyConditionChanges()
        {
                saveConditionDelegate(ref manager.currentCondition);
                buildFormattedTextDelegate(manager.Options, ref manager.currentCondition);
                manager.SaveCurrentCondition();

                // Begin TT#1351-MD - stodd - change window text when filter name changes - 
                //update the form title when changing the filter name
                filterDictionary et = filterDictionary.FromIndex(manager.currentCondition.dictionaryIndex);
                if (et == filterDictionary.FilterName)
                {
                    RaiseUpdateTitleEvent(manager.currentCondition.valueToCompare);
                }
                // End TT#1351-MD - stodd - change window text when filter name changes - 

                RaiseWindowDirtyEvent(true);	// TT#1350-MD - stodd - Prompt user when closing form when changes pending
        }

        private void CancelConditionChanges()
        {
            loadConditionDelegate(manager, manager.currentCondition);
            manager.ReloadCurrentCondition();
        }

        private void Handle_ApplyConditionChanges(object sender, filterManager.ApplyConditionChangesEventArgs e)
        {
           // ApplyConditionChanges();

            //if (AreAllElementsValid())
            //{
                ApplyConditionChanges();
            //}
        }

        //Begin TT#1484-MD -jsobek -Filters - Adding a condition should mark filter as changed
        private void Handle_AddCondition(object sender, filterManager.AddConditionEventArgs e)
        {
            this.RaiseWindowDirtyEvent(true);
        }
        private void Handle_RemoveCondition(object sender, filterManager.RemoveConditionEventArgs e)
        {
            this.RaiseWindowDirtyEvent(true);
        }
        //Delete TT#1484-MD -jsobek -Filters - Adding a condition should mark filter as changed

        public event SaveFilterEventHandler SaveFilterEvent;
        public void RaiseSaveFilterEvent(filter filterToSave, bool IsNewFilter)
        {
            if (SaveFilterEvent != null)
                SaveFilterEvent(new object(), new SaveFilterEventArgs(filterToSave, IsNewFilter));
        }
        public class SaveFilterEventArgs
        {
            public SaveFilterEventArgs(filter filterToSave, bool isNewFilter) { this.filterToSave = filterToSave; this.isNewFilter = isNewFilter; }
            public filter filterToSave { get; private set; }
            public bool isNewFilter;
        }
        public delegate void SaveFilterEventHandler(object sender, SaveFilterEventArgs e);

        public event CloseFilterEventHandler CloseFilterEvent;
        public void RaiseCloseFilterEvent()
        {
            if (CloseFilterEvent != null)
                CloseFilterEvent(new object(), new CloseFilterEventArgs());
        }
        public class CloseFilterEventArgs
        {
            public CloseFilterEventArgs() { }
        }
        public delegate void CloseFilterEventHandler(object sender, CloseFilterEventArgs e);

		// Begin TT#1351-MD - stodd - change window text when filter name changes - 
        public event UpdateTitleEventHandler UpdateTitleEvent;
        public void RaiseUpdateTitleEvent(string newFilterName)
        {
            if (UpdateTitleEvent != null)
                UpdateTitleEvent(new object(), new UpdateTitleEventArgs(newFilterName));
        }
        public class UpdateTitleEventArgs
        {
            public UpdateTitleEventArgs(string newFilterName) { this.newFilterName = newFilterName; }
            public string newFilterName { get; private set; }
        }
        public delegate void UpdateTitleEventHandler(object sender, UpdateTitleEventArgs e);
		// End TT#1351-MD - stodd - change window text when filter name changes - 

		// Begin TT#1350-MD - stodd - Prompt user when closing form when changes pending
        public event UpdateWindowDirtyEventHandler UpdateWindowDirtyEvent;
        public void RaiseWindowDirtyEvent(bool isWindowDirty)
        {
            if (UpdateWindowDirtyEvent != null)
                UpdateWindowDirtyEvent(new object(), new UpdateWindowDirtyEventArgs(isWindowDirty));
        }
        public class UpdateWindowDirtyEventArgs
        {
            public UpdateWindowDirtyEventArgs(bool isWindowDirty) { this.isWindowDirty = isWindowDirty; }
            public bool isWindowDirty { get; private set; }
        }
        public delegate void UpdateWindowDirtyEventHandler(object sender, UpdateWindowDirtyEventArgs e);

        public event UpdateChangePendingEventHandler UpdateChangePendingEvent;
        public void RaiseChangePendingEvent(bool isChangePending)
        {
            if (UpdateChangePendingEvent != null)
                UpdateChangePendingEvent(new object(), new UpdateChangePendingEventArgs(isChangePending));
        }
        public class UpdateChangePendingEventArgs
        {
            public UpdateChangePendingEventArgs(bool isChangePending) { this.isChangePending = isChangePending; }
            public bool isChangePending { get; private set; }
        }
        public delegate void UpdateChangePendingEventHandler(object sender, UpdateChangePendingEventArgs e);
		// End TT#1350-MD - stodd - Prompt user when closing form when changes pending


        public event ShowFilterMonitorEventHandler ShowFilterMonitorEvent;
        public void RaiseShowFilterMonitorEvent(bool showMonitor)
        {
            if (ShowFilterMonitorEvent != null)
                ShowFilterMonitorEvent(new object(), new ShowFilterMonitorEventArgs(showMonitor));
        }
        public class ShowFilterMonitorEventArgs
        {
            public ShowFilterMonitorEventArgs(bool showMonitor) { this.showMonitor = showMonitor; }
            public bool showMonitor { get; private set; }
        }
        public delegate void ShowFilterMonitorEventHandler(object sender, ShowFilterMonitorEventArgs e);
    }
}
