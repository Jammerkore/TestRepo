using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinExplorerBar;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class filterContainer : UserControl
    {
        public filterContainer()
        {
            InitializeComponent();
            this.ultraExplorerBar1.GroupSettings.HeaderVisible = Infragistics.Win.DefaultableBoolean.False;
        }

        private filterDictionaryEntry eg;
        private int startingRange;
        private filterManager manager;
        public void MakeElementGroups(filterManager manager, filterDictionaryEntry entry)
        {
            this.eg = entry;
            this.startingRange = entry.elementList.Count;
            this.manager = manager;
            this.ultraExplorerBar1.Groups.Clear();
            int index = 0;
            foreach (elementBase eb in entry.elementList)
            {
                string keyPrefix;
                if (eb.isList)
                {
                    keyPrefix = "list";
                }
                else
                {
                    keyPrefix = "e";
                }

                MakeElementInGroup(keyPrefix + index.ToString(), eb, false, -1); //if needed, add eb.dataType
                index++;
            }
        }

        public void MakeElementInGroup(string key, elementBase eb, bool useValueListFromField, int tempFieldIndex)
        {
            //create an instance of the element
            object o = null;
            if (eb.elementMap == filterElementMap.Calendar)
            {
                o = Activator.CreateInstance(typeof(filterElementCalendar));
            }
            if (eb.elementMap == filterElementMap.DynamicSet)
            {
                o = Activator.CreateInstance(typeof(filterElementDynamicSet));
            }
            if (eb.elementMap == filterElementMap.DynamicSetOverride)
            {
                o = Activator.CreateInstance(typeof(filterElementDynamicSetOverride));
            }
            if (eb.elementMap == filterElementMap.Field)
            {
                o = Activator.CreateInstance(typeof(filterElementField));
            }
            if (eb.elementMap == filterElementMap.Folder)
            {
                o = Activator.CreateInstance(typeof(filterElementFolder));
            }
            if (eb.elementMap == filterElementMap.Info)
            {
                o = Activator.CreateInstance(typeof(filterElementInfo));
            }
            if (eb.elementMap == filterElementMap.Limit)
            {
                o = Activator.CreateInstance(typeof(filterElementLimit));
            }
            if (eb.elementMap == filterElementMap.List)
            {
                o = Activator.CreateInstance(typeof(filterElementList));
            }
            if (eb.elementMap == filterElementMap.Logic)
            {
                o = Activator.CreateInstance(typeof(filterElementLogic));
            }
            if (eb.elementMap == filterElementMap.Merchandise)
            {
                o = Activator.CreateInstance(typeof(filterElementMerchandise));
            }
            if (eb.elementMap == filterElementMap.Name)
            {
                o = Activator.CreateInstance(typeof(filterElementName));
            }
            if (eb.elementMap == filterElementMap.NameAndLimit)
            {
                o = Activator.CreateInstance(typeof(filterElementNameAndLimit));
            }
            if (eb.elementMap == filterElementMap.OperatorDate)
            {
                o = Activator.CreateInstance(typeof(filterElementOperatorDate));
            }
            if (eb.elementMap == filterElementMap.OperatorIn)
            {
                o = Activator.CreateInstance(typeof(filterElementOperatorIn));
            }
            if (eb.elementMap == filterElementMap.OperatorNumeric)
            {
                o = Activator.CreateInstance(typeof(filterElementOperatorNumeric));
            }
            if (eb.elementMap == filterElementMap.OperatorNumericForVariables)
            {
                o = Activator.CreateInstance(typeof(filterElementOperatorNumericForVariables));
            }
            if (eb.elementMap == filterElementMap.OperatorString)
            {
                o = Activator.CreateInstance(typeof(filterElementOperatorString));
            }
            if (eb.elementMap == filterElementMap.OperatorVariablePercentage)
            {
                o = Activator.CreateInstance(typeof(filterElementOperatorVariablePercentage));
            }
            if (eb.elementMap == filterElementMap.SortBy)
            {
                o = Activator.CreateInstance(typeof(filterElementSortBy));
            }
            if (eb.elementMap == filterElementMap.SortByDirection)
            {
                o = Activator.CreateInstance(typeof(filterElementSortByDirection));
            }
            if (eb.elementMap == filterElementMap.SortByType)
            {
                o = Activator.CreateInstance(typeof(filterElementSortByType));
            }
            if (eb.elementMap == filterElementMap.ValueToCompareBool)
            {
                o = Activator.CreateInstance(typeof(filterElementValueToCompareBool));
            }
            if (eb.elementMap == filterElementMap.ValueToCompareDateBetween)
            {
                o = Activator.CreateInstance(typeof(filterElementValueToCompareDateBetween));
				// Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
                if (!eb.AllowTimeSensitiveDateCheck)
                {
                    ((filterElementValueToCompareDateBetween)o).AdjustDateBetweenFields(eb.AllowTimeSensitiveDateCheck, eb.SpecifyWeeks);
                }
				// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
            }
            if (eb.elementMap == filterElementMap.ValueToCompareDateSpecify)
            {
                o = Activator.CreateInstance(typeof(filterElementValueToCompareDateSpecify));
            }
            if (eb.elementMap == filterElementMap.ValueToCompareNumeric)
            {
                o = Activator.CreateInstance(typeof(filterElementValueToCompareNumeric));
            }
            if (eb.elementMap == filterElementMap.ValueToCompareNumericBetween)
            {
                o = Activator.CreateInstance(typeof(filterElementValueToCompareNumericBetween));
            }
            if (eb.elementMap == filterElementMap.ValueToCompareString)
            {
                o = Activator.CreateInstance(typeof(filterElementValueToCompareString));
            }
            if (eb.elementMap == filterElementMap.Variable)
            {
                o = Activator.CreateInstance(typeof(filterElementVariable));
            }
            if (eb.elementMap == filterElementMap.ExclusionList)
            {
                o = Activator.CreateInstance(typeof(filterElementList));
            }
			// Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
            if (eb.elementMap == filterElementMap.OperatorCalendarDate)
            {
                o = Activator.CreateInstance(typeof(filterElementOperatorCalendarDate));
            }
			// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only

            //get the interface from the element
            eb.elementInterface = (IFilterElement)o;
            //controlList.Add(eb.elementInterface);
            //give back a copy to the element for utility functions: MakeDirty, etc.

            //box to user control and set in group in the explorer bar
            UserControl uiControl = (UserControl)o;

            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            //Set the delegates first
            if (eb.isField)
            {
                filterElementField fe = (filterElementField)uiControl;
                fe.makeElementInGroupDelegate = new FilterMakeElementInGroupDelegate(MakeElementInGroup);
                fe.removeDynamicElementsForFieldDelegate = new FilterRemoveDynamicElementsForFieldDelegate(RemoveDynamicElementsForField);

            }
            if (eb.isVariable)
            {
                filterElementVariable fe = (filterElementVariable)uiControl;
                fe.makeElementInGroupDelegate = new FilterMakeElementInGroupDelegate(MakeElementInGroup);
                fe.removeDynamicElementsForFieldDelegate = new FilterRemoveDynamicElementsForFieldDelegate(RemoveDynamicElementsForField);
            }
            if (eb.isOperatorNumeric)
            {
                filterElementOperatorNumeric fe = (filterElementOperatorNumeric)uiControl;
                fe.makeElementInGroupDelegate = new FilterMakeElementInGroupDelegate(MakeElementInGroup);
                fe.removeDynamicElementsForFieldDelegate = new FilterRemoveDynamicElementsForFieldDelegate(RemoveDynamicElementsForField);

            }
            if (eb.isOperatorDate)
            {
                filterElementOperatorDate fe = (filterElementOperatorDate)uiControl;
                fe.makeElementInGroupDelegate = new FilterMakeElementInGroupDelegate(MakeElementInGroup);
                fe.removeDynamicElementsForFieldDelegate = new FilterRemoveDynamicElementsForFieldDelegate(RemoveDynamicElementsForField);

            }
            if (eb.isSortBy)
            {
                filterElementSortByType fe = (filterElementSortByType)uiControl;
                fe.makeElementInGroupDelegate = new FilterMakeElementInGroupDelegate(MakeElementInGroup);
                fe.removeDynamicElementsForFieldDelegate = new FilterRemoveDynamicElementsForFieldDelegate(RemoveDynamicElementsForField);
            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
			if (eb.isOperatorCalendarDate)
            {
                filterElementOperatorCalendarDate fe = (filterElementOperatorCalendarDate)uiControl;
                fe.makeElementInGroupDelegate = new FilterMakeElementInGroupDelegate(MakeElementInGroup);
                fe.removeDynamicElementsForFieldDelegate = new FilterRemoveDynamicElementsForFieldDelegate(RemoveDynamicElementsForField);
            }
            // End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only

            eb.elementInterface.SetElementBase(eb, eg.groupSettings);
      
    
            if (eb.isList)
            {
                filterElementList fe = (filterElementList)uiControl;
                //ef.ValueTypeChangedEvent += new filterElementField.ValueTypeChangedEventHandler(Handle_ValueTypeChanged); 
                fe.SetBinding(useValueListFromField, tempFieldIndex);
            }

            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup group = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup(key);
            group.Text = eb.groupHeading;
            group.Settings.Style = Infragistics.Win.UltraWinExplorerBar.GroupStyle.ControlContainer;

            this.ultraExplorerBar1.Groups.Add(group);
            //if (eb.isList)
            //{
            //    group.Container.Height = this.ultraExplorerBar1.Height - group.Container.Top;
            //}
            //else
            //{
                group.Container.Height = uiControl.Height;
            //}
            group.Container.Controls.Add(uiControl);
            uiControl.Dock = DockStyle.Fill;

            if (eb.isOperatorNumeric)
            {
                filterElementOperatorNumeric fe = (filterElementOperatorNumeric)uiControl;
                fe.SetDefault();
            }
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (eb.isOperatorDate)
            {
                filterElementOperatorDate fe = (filterElementOperatorDate)uiControl;
                fe.SetDefault();
            }
            if (eb.elementMap == filterElementMap.ValueToCompareDateSpecify)
            {
                filterElementValueToCompareDateSpecify fe = (filterElementValueToCompareDateSpecify)uiControl;
                fe.SetDefault();
            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
			// Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
            if (eb.isOperatorCalendarDate)
            {
                filterElementOperatorCalendarDate fe = (filterElementOperatorCalendarDate)uiControl;
                fe.SetDefault();
            }
			// End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
            
            if (eb.isList)
            {
                ResizeListContainer();
            }
        }
        private void RemoveDynamicElementsForField(List<string> keyListToRemove)
        {
            //clear newly adding groups from the list and from the container
            if (keyListToRemove.Count > 0)
            {
                //int ending = countOfNewGroups -1;
                //for (int i = countOfNewGroups -1; i >= 0 ; i--)
                //{
                //    this.ultraExplorerBar1.Groups.RemoveAt(i + 2);

                //}

                foreach (string key in keyListToRemove)
                {
                    //if (this.ultraExplorerBar1.Groups.Contains(key))
                    //{
                    try
                    {
                        this.ultraExplorerBar1.Groups.Remove(this.ultraExplorerBar1.Groups[key]);
                    }
                    catch
                    {
                    }
                    //}
                }

            }
        }

        public void ReDraw(filterOptionDefinition options)
        {
            if (options.ShowGroupingLabels)
            {
                this.ultraExplorerBar1.GroupSettings.HeaderVisible = Infragistics.Win.DefaultableBoolean.True;
            }
            else
            {
                this.ultraExplorerBar1.GroupSettings.HeaderVisible = Infragistics.Win.DefaultableBoolean.False;
            }
        }

        private void filterContainer_Paint(object sender, PaintEventArgs e)
        {
             ResizeListContainer();
        }
        //The space between the outer and inner item elements in each group
        //is going to be the same.  So once a non-zero value of this is found, 
        //it doesn't need to be checked anymore.  
        bool spaceBetweenInnerAndOuterElementsFound = false;
        int spaceBetweenGroupInnerOuterElement = 0;
        private void ResizeListContainer()
        {
            /*	totalHeight - All applicable UI Elements totalled together
             *	itemHeight  - height of the item space for a particular group 
             *	headerHeight - height of the header for a particular group
             *	containerHeight - what the height of the container should be based on the total height of the other UIElements
             *	spaceBetweenGroups - the amount of height between groups
             */
            int totalHeight = 0, itemHeight, headerHeight, containerHeight, spaceBetweenGroups = 0, totalSpaceBetweenGroups = 0;
            int nonListItemHeight = 0; // TT#1381-MD -jsobek -Excessive white space in Filter Details section
            Infragistics.Win.UIElement descendant;
            foreach (Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup group in ultraExplorerBar1.Groups)
            {
                itemHeight = 0;
                headerHeight = 0;

                if (group.UIElement != null)
                {
                    if (spaceBetweenGroups == 0)
                        spaceBetweenGroups = group.UIElement.Rect.Top - ultraExplorerBar1.UIElement.Rect.Top;
                    //if it's not a control container, get how much space its items area takes up
                    if (group.Settings.Style != GroupStyle.ControlContainer)
                    {
                        descendant = group.UIElement.GetDescendant(typeof(ItemAreaOuterUIElement));
                        if (descendant != null)
                        {
                            ItemAreaOuterUIElement itemArea = (ItemAreaOuterUIElement)descendant;
                            itemHeight = itemArea.Rect.Height;
                        }
                    }
                    //otherwise, get how much space is between the control container and the edge of the 
                    //items space that it resides in
                    else
                    {
                        Infragistics.Win.UIElement innerElem = group.UIElement.GetDescendant(typeof(ItemAreaInnerUIElement));
                        Infragistics.Win.UIElement outerElem = group.UIElement.GetDescendant(typeof(ItemAreaOuterUIElement));
                        if (innerElem != null && outerElem != null)
                        {
                            ItemAreaOuterUIElement groupOuter = (ItemAreaOuterUIElement)outerElem;
                            ItemAreaInnerUIElement groupInner = (ItemAreaInnerUIElement)innerElem;

                            //Begin TT#1381-MD -jsobek -Excessive white space in Filter Details section
                            if (group.Key.StartsWith("list") == false)
                            {
                                nonListItemHeight += groupOuter.Rect.Height;
                            }
                            //End TT#1381-MD -jsobek -Excessive white space in Filter Details section

                            if (!spaceBetweenInnerAndOuterElementsFound)
                            {
                                spaceBetweenGroupInnerOuterElement = groupOuter.Rect.Height - groupInner.Rect.Height;
                                if (spaceBetweenGroupInnerOuterElement > 0)
                                    spaceBetweenInnerAndOuterElementsFound = true;
                            }
                        }
                    }
                    descendant = group.UIElement.GetDescendant(typeof(UltraExplorerBarGroupHeaderUIElement));
                    //get the amount of space each group header takes up
                    if (descendant != null)
                    {
                        UltraExplorerBarGroupHeaderUIElement headerArea = (UltraExplorerBarGroupHeaderUIElement)descendant;
                        headerHeight = headerArea.Rect.Height;
                    }
                }
                totalHeight += itemHeight + headerHeight;
           
                totalSpaceBetweenGroups += spaceBetweenGroups;
            }
            //add some space at the bottom
            totalSpaceBetweenGroups += spaceBetweenGroups;

            totalHeight += totalSpaceBetweenGroups + spaceBetweenGroupInnerOuterElement;
            //this calculates how much space is available for a control container
            containerHeight = ultraExplorerBar1.UIElement.Rect.Height - totalHeight;
            //Resize the container based on this value.
            foreach (Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup group in ultraExplorerBar1.Groups)
            {
                if (group.Key.StartsWith("list"))
                {

                    if (containerHeight < 0)
                        group.Settings.ContainerHeight = 0;
                    else
                    {
                        if ((containerHeight - nonListItemHeight) >= 0) // TT#1381-MD -jsobek -Excessive white space in Filter Details section
                        {
                            group.Settings.ContainerHeight = containerHeight - nonListItemHeight;    // TT#1381-MD -jsobek -Excessive white space in Filter Details section
                        }
                    }
                }
            }
        }
        //Begin TT#1381-MD -jsobek -Excessive white space in Filter Details section
        private void filterContainer_Resize(object sender, EventArgs e)
        {
            ResizeListContainer(); //paint event does not fire when making the container smaller, so using resize
        }
        //End TT#1381-MD -jsobek -Excessive white space in Filter Details section
    }
}
