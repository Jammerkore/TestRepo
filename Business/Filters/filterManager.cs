using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

using MIDRetail.DataCommon;
using MIDRetail.Data;


namespace MIDRetail.Business
{
    /// <summary>
    /// The filter manager is responsible for wiring up UI events and handling UI logic
    /// </summary>
    public class filterManager
    {
        public filterManager(filter f, SessionAddressBlock SAB, System.Windows.Forms.Form calendarDateSelectorForm, SetCalendarDateRangeForPlanDelegate setCalendarDateRangeDelegate)
        {
            this.currentFilter = f;
            this.SAB = SAB;
            this.calendarDateSelectorForm = calendarDateSelectorForm;
            this.setCalendarDateRangeDelegate = setCalendarDateRangeDelegate;
        }
        public filter currentFilter;
        public filterCondition currentCondition;
        private bool currentConditionIsDirty = false;
        //options
        public filterOptionDefinition Options = new filterOptionDefinition();
        public SessionAddressBlock SAB;
        //private CalendarDateSelector _dateSel;
        public System.Windows.Forms.Form calendarDateSelectorForm;
        public SetCalendarDateRangeForPlanDelegate setCalendarDateRangeDelegate;
        public bool readOnly;

       public IsStoreGroupOrLevelInUse isStoreGroupOrLevelInUse;
       //public IsStoreGroupLevelInUse isStoreGroupLevelInUse;

        public string GetTextFromHierarchyNode(int hnRID)
        {
            if (hnRID == Include.NoRID) //TT#1388-MD -jsobek -Product Filter
            {
                return string.Empty;
            }
            else
            {
                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(hnRID, true, true);
                if (hnp.Text == null)
                {
                    return string.Empty;
                }
                else
                {
                    //Begin TT#1313-MD -jsobek -Header Filters                              
                    string displayText = hnp.Text;
                    if (hnp.HomeHierarchyType == eHierarchyType.alternate) //Display the name of the alternate hierarchy
                    {
                        //    MerchandiseHierarchyData mData = new MerchandiseHierarchyData();
                        //organizationalPhRID = mData.Hierarchy_Read_Organizational_RID();
                        string hierarchyID = SAB.HierarchyServerSession.GetHierarchyIdByRID(hnp.HierarchyRID);
                        if (hierarchyID != string.Empty)
                        {
                            displayText += " (" + hierarchyID + ")";
                        }
                    }
                    return displayText;
                    //End TT#1313-MD -jsobek -Header Filters
                }
            }
        }



        public delegate bool AreAllElementsValidDelegate();
        public AreAllElementsValidDelegate areAllElementsValidDelegate;

        public bool IsOKToSelectNewCondition()
        {
            if (currentConditionIsDirty)
            {
                // Begin TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition
                string msgText = MIDText.GetText(eMIDTextCode.msg_ApplyChangesCondition);
                string msgCaption = MIDText.GetTextOnly(eMIDTextCode.msg_ApplyChanges);
                // Not a condition
                if (currentCondition.dictionaryIndex == filterDictionary.FilterName.dbIndex
                    || currentCondition.dictionaryIndex == filterDictionary.FilterFolder.dbIndex
                    || currentCondition.dictionaryIndex == filterDictionary.ResultLimit.dbIndex)
                {
                    msgText = MIDText.GetText(eMIDTextCode.msg_ApplyChanges);
                }
                DialogResult result = MessageBox.Show(msgText, msgCaption, MessageBoxButtons.YesNoCancel);  
                // End TT#1348-MD - stodd - Store Filters - Change "Save Changes" prompt to be consistent with "Apply Changes" condition
                if (result == DialogResult.Yes)
                {
                    if (areAllElementsValidDelegate() == true)
                    {
                        RaiseApplyConditionChangesEvent();
                    }
                    else
                    {
                        currentConditionIsDirty = true; //still dirty
                        return false; //we cannot move on in the case of invalid selections
                    }
                }
                else if (result == DialogResult.No)
                {
                    //continue on
                    currentConditionIsDirty = false;
                    RaiseDisableConditionToolbarEvent();
                }
                else
                {
                    // e.Cancel = true;
                    return false;
                }
            }
            return true;
        }
        public void MakeConditionDirty()
        {
            currentConditionIsDirty = true;
            RaiseEnableConditionToolbarEvent();
        }
        public void SaveCurrentCondition()
        {
            currentFilter.UpdateCondition(currentCondition);
            currentConditionIsDirty = false;
            RaiseDisableConditionToolbarEvent();

            //show the new formatted text
            RaiseConditionHasBeenUpdatedEvent(currentCondition);
        }
        public void ReloadCurrentCondition()
        {
            currentConditionIsDirty = false;
            RaiseDisableConditionToolbarEvent();
        }


        public void SelectedConditionChanged(filterCondition newSelectedCondition)
        {
            currentConditionIsDirty = false;
            this.currentCondition = newSelectedCondition;
            RaiseSelectedConditionChangedEvent(this.currentCondition);
        }


        public ConditionNode selectedConditionNode = null;  //used to determine position of new nodes


        public void AddCondition(filterDictionary dictionary)
        {
            //Limit the number of sort by conditions to 5
    
            if (dictionary.navigationType == filterNavigationTypes.SortBy)
            {
                ConditionNode cnSortByParent = this.currentFilter.FindConditionNode(this.currentFilter.GetSortByConditionSeq()); //parent seq for sort by
                int sortByCount = cnSortByParent.ConditionNodes.Count;
                if (sortByCount == 5)
                {
                    MessageBox.Show(filterMessages.overSortByMaxConditions);
                    return;
                }
            }

          
            filterCondition fc = new filterCondition();
            fc.dictionaryIndex = dictionary.dbIndex;
            fc.conditionFilterRID = this.currentFilter.filterRID;
            fc.Seq = currentFilter.GetNextConditionSeq();

            filterDictionaryEntry entry = currentFilter.GetEntryFromCondition(this, fc);
            entry.SetDefaults(ref fc);
            entry.BuildFormattedText(this.Options, ref fc);

            if (this.currentFilter.filterType == filterTypes.StoreGroupFilter || this.currentFilter.filterType == filterTypes.StoreGroupDynamicFilter)
            {
                ConditionNode cnRootCondition = this.currentFilter.FindConditionNode(this.currentFilter.GetRootConditionSeq()); //parent seq for conditions  

                if (cnRootCondition.ConditionNodes.Count == 1 && (fc.dictionaryIndex != filterDictionary.StoreGroupName && fc.dictionaryIndex != filterDictionary.StoreGroupDynamic))
                {
                    //User must have a group beyond Available Stores in order to add conditions
                    MessageBox.Show("Attribute Filter must contain a group besides Available Stores in order to add conditions.");
                }
                else
                {
                    ConditionNode cn = currentFilter.InsertCondition(selectedConditionNode, fc);
                    RaiseAddConditionEvent(cn);
                }
            }
            else
            {
                ConditionNode cn = currentFilter.InsertCondition(selectedConditionNode, fc);
                RaiseAddConditionEvent(cn);
            }
           
        }
        public void RemoveCondition()
        {
            int newSelectedSeq = -1;
            //find a new node to select
            ConditionNode cn = currentFilter.FindConditionNode(currentCondition.Seq);

            bool isOkToRemove = true;
            if (currentFilter.filterType == filterTypes.StoreGroupFilter)
            {
                if (cn.condition.dictionaryIndex == filterDictionary.StoreGroupName)
                {
                    int levelRID = cn.condition.fieldIndex;
                    if (levelRID != Include.NoRID)
                    {
                        if (this.isStoreGroupOrLevelInUse(eProfileType.StoreGroupLevel, levelRID) == true)
                        {
                            isOkToRemove = false;
                        }
                    }
                }
            }


            if (isOkToRemove)
            {
                bool hasPrevSibling = currentFilter.HasPreviousSibling(cn);
                if (hasPrevSibling)
                {
                    ConditionNode prev = currentFilter.GetPreviousSibling(cn);
                    newSelectedSeq = prev.condition.Seq;
                }
                else
                {
                    if (cn.IsRootLevel)
                    {
                        bool hasNextSibling = currentFilter.HasNextSibling(cn);
                        if (hasNextSibling)
                        {
                            ConditionNode next = currentFilter.GetNextSibling(cn);
                            newSelectedSeq = next.condition.Seq;
                        }
                    }
                    else
                    {
                        //select parent
                        newSelectedSeq = cn.Parent.condition.Seq;
                    }
                }
                //remove from datatable
                ConditionNode parent = cn.Parent;
                currentFilter.RemoveConditionNode(cn);
                UpdateSiblingSequences(parent);
                RaiseRemoveConditionEvent(newSelectedSeq);
            }
        }

        public void UpdateSiblingSequences(ConditionNode cn)
        {
            //resequence all the sibling sequences at this node level
            int newSiblingSeq = 1;
            foreach (ConditionNode n in cn.ConditionNodes)
            {
                n.condition.SiblingSeq = newSiblingSeq;
                currentFilter.UpdateCondition(n.condition);
                newSiblingSeq++;
            }
        }




        public event EnableConditionToolbarEventHandler EnableConditionToolbarEvent;
        public void RaiseEnableConditionToolbarEvent()
        {
            if (EnableConditionToolbarEvent != null)
                EnableConditionToolbarEvent(new object(), new EnableConditionToolbarEventArgs());
        }
        public class EnableConditionToolbarEventArgs
        {
            public EnableConditionToolbarEventArgs() { }
        }
        public delegate void EnableConditionToolbarEventHandler(object sender, EnableConditionToolbarEventArgs e);


        public event DisableConditionToolbarEventHandler DisableConditionToolbarEvent;
        public void RaiseDisableConditionToolbarEvent()
        {
            if (DisableConditionToolbarEvent != null)
                DisableConditionToolbarEvent(new object(), new DisableConditionToolbarEventArgs());
        }
        public class DisableConditionToolbarEventArgs
        {
            public DisableConditionToolbarEventArgs() { }
        }
        public delegate void DisableConditionToolbarEventHandler(object sender, DisableConditionToolbarEventArgs e);



        public event ApplyConditionChangesEventHandler ApplyConditionChangesEvent;
        public void RaiseApplyConditionChangesEvent()
        {
            if (ApplyConditionChangesEvent != null)
                ApplyConditionChangesEvent(new object(), new ApplyConditionChangesEventArgs());
        }
        public class ApplyConditionChangesEventArgs
        {
            public ApplyConditionChangesEventArgs() { }
        }
        public delegate void ApplyConditionChangesEventHandler(object sender, ApplyConditionChangesEventArgs e);


        public event SelectedConditionChangedEventHandler SelectedConditionChangedEvent;
        public void RaiseSelectedConditionChangedEvent(filterCondition condition)
        {

            if (SelectedConditionChangedEvent != null)
                SelectedConditionChangedEvent(new object(), new SelectedConditionChangedEventArgs(condition));
        }
        public class SelectedConditionChangedEventArgs
        {
            public SelectedConditionChangedEventArgs(filterCondition condition) { this.condition = condition; }
            public filterCondition condition { get; private set; }
        }
        public delegate void SelectedConditionChangedEventHandler(object sender, SelectedConditionChangedEventArgs e);


        public event AddConditionEventHandler AddConditionEvent;
        public void RaiseAddConditionEvent(ConditionNode conditionNodeToAdd)
        {

            if (AddConditionEvent != null)
                AddConditionEvent(new object(), new AddConditionEventArgs(conditionNodeToAdd));
        }
        public class AddConditionEventArgs
        {
            public AddConditionEventArgs(ConditionNode conditionNodeToAdd) { this.conditionNodeToAdd = conditionNodeToAdd; }
            public ConditionNode conditionNodeToAdd { get; private set; }
        }
        public delegate void AddConditionEventHandler(object sender, AddConditionEventArgs e);


        public event RemoveConditionEventHandler RemoveConditionEvent;
        public void RaiseRemoveConditionEvent(int newSelectedSeq)
        {

            if (RemoveConditionEvent != null)
                RemoveConditionEvent(new object(), new RemoveConditionEventArgs(newSelectedSeq));
        }
        public class RemoveConditionEventArgs
        {
            public RemoveConditionEventArgs(int newSelectedSeq) { this.newSelectedSeq = newSelectedSeq; }
            public int newSelectedSeq { get; private set; }
        }
        public delegate void RemoveConditionEventHandler(object sender, RemoveConditionEventArgs e);



        public event ConditionHasBeenUpdatedEventHandler ConditionHasBeenUpdatedEvent;
        public void RaiseConditionHasBeenUpdatedEvent(filterCondition condition)
        {

            if (ConditionHasBeenUpdatedEvent != null)
                ConditionHasBeenUpdatedEvent(new object(), new ConditionHasBeenUpdatedEventArgs(condition));
        }
        public class ConditionHasBeenUpdatedEventArgs
        {
            public ConditionHasBeenUpdatedEventArgs(filterCondition condition) { this.condition = condition; }
            public filterCondition condition { get; private set; }
        }
        public delegate void ConditionHasBeenUpdatedEventHandler(object sender, ConditionHasBeenUpdatedEventArgs e);
    }
}
