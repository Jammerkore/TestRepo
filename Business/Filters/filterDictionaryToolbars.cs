using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    public class filterToolbarButton
    {
        public bool startGroup;
        public string buttonText;
        public string toolTip;
        public filterDictionary dictionaryEntry;
        public filterToolbarImages buttonImage;
        public filterToolbarButton(filterDictionary dictionaryEntry, bool startGroup = false, string buttonText = "")
        {
            this.startGroup = startGroup;
            this.dictionaryEntry = dictionaryEntry;
            this.buttonImage = dictionaryEntry.buttonImage;
            this.toolTip = dictionaryEntry.buttonToolTip;
            if (buttonText == string.Empty)
            {
                this.buttonText = dictionaryEntry.buttonText;
            }
        }
    }
    public sealed class filterToolbars
    {
        public static List<filterToolbars> sortByTypeList = new List<filterToolbars>();
        public static readonly filterToolbars StoreToolbar = new filterToolbars("Store Toolbar", 0, filterTypes.StoreFilter, new List<filterToolbarButton>() { new filterToolbarButton(filterDictionary.StoreList), new filterToolbarButton(filterDictionary.StoreFields, true), new filterToolbarButton(filterDictionary.StoreStatus), new filterToolbarButton(filterDictionary.StoreCharacteristics, true), new filterToolbarButton(filterDictionary.StoreAttributeSet), new filterToolbarButton(filterDictionary.StoreVariableToConstant, true), new filterToolbarButton(filterDictionary.StoreVariableToVariable), new filterToolbarButton(filterDictionary.StoreVariablePercentage), new filterToolbarButton(filterDictionary.StoreSortBy, true) });
        public static readonly filterToolbars HeaderToolbar = new filterToolbars("Header Toolbar", 1, filterTypes.HeaderFilter, new List<filterToolbarButton>() { new filterToolbarButton(filterDictionary.HeaderTypes), new filterToolbarButton(filterDictionary.HeaderFields, true), new filterToolbarButton(filterDictionary.HeaderStatus), new filterToolbarButton(filterDictionary.HeaderCharacteristics, true), new filterToolbarButton(filterDictionary.HeaderMerchandise), new filterToolbarButton(filterDictionary.HeaderDate, true), new filterToolbarButton(filterDictionary.HeaderReleaseDate), new filterToolbarButton(filterDictionary.HeaderSortBy, true) });
        // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
        //public static readonly filterToolbars AssortmentToolbar = new filterToolbars("Assortment Toolbar", 2, filterTypes.AssortmentFilter, new List<filterToolbarButton>() { new filterToolbarButton(filterDictionary.HeaderFields, true), new filterToolbarButton(filterDictionary.HeaderStatus), new filterToolbarButton(filterDictionary.HeaderCharacteristics, true), new filterToolbarButton(filterDictionary.HeaderMerchandise), new filterToolbarButton(filterDictionary.HeaderDate, true), new filterToolbarButton(filterDictionary.HeaderReleaseDate) });
        public static readonly filterToolbars AssortmentToolbar = new filterToolbars("Assortment Toolbar", 2, filterTypes.AssortmentFilter, new List<filterToolbarButton>() { new filterToolbarButton(filterDictionary.AssortmentTypes, true), new filterToolbarButton(filterDictionary.AssortmentFields, true), new filterToolbarButton(filterDictionary.AssortmentStatus), new filterToolbarButton(filterDictionary.AssortmentMerchandise, true), new filterToolbarButton(filterDictionary.AssortmentDate, true), new filterToolbarButton(filterDictionary.AssortmentSortBy, true) });
        // End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
        public static readonly filterToolbars AttributeSetToolbar = new filterToolbars("Attribute Set Toolbar", 3, filterTypes.StoreGroupFilter, new List<filterToolbarButton>() { new filterToolbarButton(filterDictionary.StoreGroupName), new filterToolbarButton(filterDictionary.StoreList, true), new filterToolbarButton(filterDictionary.StoreFields), new filterToolbarButton(filterDictionary.StoreStatus), new filterToolbarButton(filterDictionary.StoreCharacteristics, true) });
        public static readonly filterToolbars ProductToolbar = new filterToolbars("Product Toolbar", 4, filterTypes.ProductFilter, new List<filterToolbarButton>() { new filterToolbarButton(filterDictionary.ProductAny), new filterToolbarButton(filterDictionary.ProductID), new filterToolbarButton(filterDictionary.ProductName), new filterToolbarButton(filterDictionary.ProductDescrip), new filterToolbarButton(filterDictionary.ProductChar, true), new filterToolbarButton(filterDictionary.ProductActive), new filterToolbarButton(filterDictionary.ProductMerchandise, true), new filterToolbarButton(filterDictionary.ProductLevels), new filterToolbarButton(filterDictionary.ProductHierarchies), new filterToolbarButton(filterDictionary.ProductSortBy, true) });
        public static readonly filterToolbars AuditToolbar = new filterToolbars("Audit Toolbar", 5, filterTypes.AuditFilter, new List<filterToolbarButton>() { new filterToolbarButton(filterDictionary.AuditStartTime), new filterToolbarButton(filterDictionary.AuditDetailTime), new filterToolbarButton(filterDictionary.AuditFields, true), new filterToolbarButton(filterDictionary.AuditExecutionStatus), new filterToolbarButton(filterDictionary.AuditCompletionStatus), new filterToolbarButton(filterDictionary.Users), new filterToolbarButton(filterDictionary.AuditProcessMessageLevel, true), new filterToolbarButton(filterDictionary.AuditDetailMessageLevel), new filterToolbarButton(filterDictionary.AuditSortBy, true) });
        public static readonly filterToolbars AttributeSetDynamicToolbar = new filterToolbars("Attribute Set Dynamic Toolbar", 6, filterTypes.StoreGroupDynamicFilter, new List<filterToolbarButton>() { new filterToolbarButton(filterDictionary.StoreGroupDynamic) });


        private filterToolbars(string Name, int dbIndex, filterTypes filterType, List<filterToolbarButton> buttonList)
        {
            this.Name = Name;
            this.Index = dbIndex;
            this.filterType = filterType;
            this.buttonList = buttonList;
            sortByTypeList.Add(this);
        }
        public string Name { get; private set; }
        public int Index { get; private set; }
        public filterTypes filterType;
        public List<filterToolbarButton> buttonList { get; private set; }
        public static implicit operator int(filterToolbars op) { return op.Index; }


        public static filterToolbars FromIndex(int dbIndex)
        {
            filterToolbars result = sortByTypeList.Find(
               delegate(filterToolbars ft)
               {
                   return ft.Index == dbIndex;
               }
               );
            if (result != null)
            {
                return result;
            }
            else
            {
                //toolbar was not found in the list
                return null;
            }
        }
        public static filterToolbars FromFilterType(filterTypes filterType)
        {
            filterToolbars result = sortByTypeList.Find(
               delegate(filterToolbars ft)
               {
                   return ft.filterType == filterType;
               }
               );
            if (result != null)
            {
                return result;
            }
            else
            {
                //toolbar was not found in the list
                return null;
            }
        }


    }
}
