using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    public sealed class filterDictionary
    {
        public static List<filterDictionary> dictionaryList = new List<filterDictionary>();
        public static readonly filterDictionary FilterName = new filterDictionary(0, typeof(entryFilterName), filterNavigationTypes.Info, filterToolbarImages.None, "", "", costToRunEstimate: 0);
        public static readonly filterDictionary FilterFolder = new filterDictionary(1, typeof(entryFolder), filterNavigationTypes.Info, filterToolbarImages.None, "", "", costToRunEstimate: 0);
        public static readonly filterDictionary ResultLimit = new filterDictionary(2, typeof(entryResultLimit), filterNavigationTypes.Info, filterToolbarImages.None, "", "", costToRunEstimate: 0);
        public static readonly filterDictionary InfoFilter = new filterDictionary(3, typeof(entryInfoFilter), filterNavigationTypes.Info, filterToolbarImages.None, "", "", costToRunEstimate: 0);
        public static readonly filterDictionary InfoConditions = new filterDictionary(4, typeof(entryInfoConditions), filterNavigationTypes.Info, filterToolbarImages.None, "", "", costToRunEstimate: 0);
        public static readonly filterDictionary InfoSortBy = new filterDictionary(5, typeof(entryInfoSortBy), filterNavigationTypes.Info, filterToolbarImages.None, "", "", costToRunEstimate: 0);

        public static readonly filterDictionary StoreStatus = new filterDictionary(6, typeof(entryStoreStatus), filterNavigationTypes.Condition, filterToolbarImages.Status, "Status", "Adds a Store Status condition to the filter.", costToRunEstimate: 10);
        public static readonly filterDictionary StoreList = new filterDictionary(7, typeof(entryStoreList), filterNavigationTypes.Condition, filterToolbarImages.StoreList, "Store List", "Adds a Store List condition to the filter.", costToRunEstimate: 10);
        public static readonly filterDictionary StoreFields = new filterDictionary(8, typeof(entryStoreFields), filterNavigationTypes.Condition, filterToolbarImages.Field, "Field", "Adds a Store Field condition to the filter.", costToRunEstimate: 10);
        public static readonly filterDictionary StoreCharacteristics = new filterDictionary(9, typeof(entryStoreCharacteristics), filterNavigationTypes.Condition, filterToolbarImages.Characteristics, "Char", "Adds a Store Characteristic condition to the filter.", costToRunEstimate: 100);
        public static readonly filterDictionary StoreAttributeSet = new filterDictionary(22, typeof(entryStoreAttributeSet), filterNavigationTypes.Condition, filterToolbarImages.AttributeSet, "Attr Set", "Adds an Attribute Set condition to the filter.", costToRunEstimate: 100);
        public static readonly filterDictionary StoreVariableToConstant = new filterDictionary(10, typeof(entryStoreVariableToConstant), filterNavigationTypes.Condition, filterToolbarImages.VariableToConstant, "Var-Cnst", "Adds a Variable to Constant condition to the filter.", costToRunEstimate: 800);
        public static readonly filterDictionary StoreVariableToVariable = new filterDictionary(11, typeof(entryStoreVariableToVariable), filterNavigationTypes.Condition, filterToolbarImages.VariableToVariable, "Var-Var", "Adds a Variable to Variable condition to the filter.", costToRunEstimate: 2000);
        public static readonly filterDictionary StoreVariablePercentage = new filterDictionary(12, typeof(entryStoreVariablePercentage), filterNavigationTypes.Condition, filterToolbarImages.VariablePercentage, "Var %", "Adds a Variable Percentage condition to the filterr.", costToRunEstimate: 2000);

        public static readonly filterDictionary HeaderTypes = new filterDictionary(13, typeof(entryHeaderTypes), filterNavigationTypes.Condition, filterToolbarImages.HeaderType, "Hdr Type", "Adds a Header Type condition to the filter.", costToRunEstimate: 10);
        public static readonly filterDictionary HeaderStatus = new filterDictionary(14, typeof(entryHeaderStatus), filterNavigationTypes.Condition, filterToolbarImages.Status, "Status", "Adds a Header Status condition to the filter.", costToRunEstimate: 10);
        public static readonly filterDictionary HeaderFields = new filterDictionary(15, typeof(entryHeaderFields), filterNavigationTypes.Condition, filterToolbarImages.Field, "Field", "Adds a Header Field condition to the filter.", costToRunEstimate: 10);
        public static readonly filterDictionary HeaderCharacteristics = new filterDictionary(16, typeof(entryHeaderCharacteristics), filterNavigationTypes.Condition, filterToolbarImages.Characteristics, "Char", "Adds a Header Characteristic condition to the filter.", costToRunEstimate: 100);
        public static readonly filterDictionary HeaderMerchandise = new filterDictionary(17, typeof(entryHeaderMerchandise), filterNavigationTypes.Condition, filterToolbarImages.HeaderMerchandise, "Merchandise", "Adds a Merchandise condition to the filter.", costToRunEstimate: 10);
        public static readonly filterDictionary HeaderDate = new filterDictionary(18, typeof(entryHeaderDate), filterNavigationTypes.Condition, filterToolbarImages.Date1, "Hdr Date", "Adds a Header Date condition to the filter.", costToRunEstimate: 10);
        public static readonly filterDictionary HeaderReleaseDate = new filterDictionary(19, typeof(entryHeaderReleaseDate), filterNavigationTypes.Condition, filterToolbarImages.Date2, "Rls Date", "Adds a Release Date condition to the filter.", costToRunEstimate: 10);

        public static readonly filterDictionary StoreSortBy = new filterDictionary(20, typeof(entryStoreSortBy), filterNavigationTypes.SortBy, filterToolbarImages.SortBy, "Sort", "Adds a Sort By entry to the filter.", costToRunEstimate: 0);
        public static readonly filterDictionary HeaderSortBy = new filterDictionary(21, typeof(entryHeaderSortBy), filterNavigationTypes.SortBy, filterToolbarImages.SortBy, "Sort", "Adds a Sort By entry to the filter.", costToRunEstimate: 0); //TT#1468-MD -jsobek -Header Filter Sort Options

        //Begin TT#1388-MD -jsobek -Product Filters
        public static readonly filterDictionary ProductAny = new filterDictionary(23, typeof(entryProductAny), filterNavigationTypes.Condition, filterToolbarImages.ProductAny, "Any", "Allows searching ID, Name, and Description at the same time.", costToRunEstimate: 10);
        public static readonly filterDictionary ProductID = new filterDictionary(24, typeof(entryProductID), filterNavigationTypes.Condition, filterToolbarImages.ProductID, "ID", "Allows searching by just ID.", costToRunEstimate: 10);
        public static readonly filterDictionary ProductName = new filterDictionary(25, typeof(entryProductName), filterNavigationTypes.Condition, filterToolbarImages.ProductName, "Name", "Allows searching by just Name.", costToRunEstimate: 1);
        public static readonly filterDictionary ProductDescrip = new filterDictionary(26, typeof(entryProductDescrip), filterNavigationTypes.Condition, filterToolbarImages.ProductDescrip, "Descrip", "Allows searching by just Description.", costToRunEstimate: 10);
        public static readonly filterDictionary ProductChar = new filterDictionary(27, typeof(entryProductCharacteristics), filterNavigationTypes.Condition, filterToolbarImages.Characteristics, "Char", "Allows searching by product characteristics.", costToRunEstimate: 10);
        public static readonly filterDictionary ProductActive = new filterDictionary(28, typeof(entryProductActive), filterNavigationTypes.Condition, filterToolbarImages.ProductActive, "Active", "Allows for narrowing the search to only active nodes.", costToRunEstimate: 10);
        public static readonly filterDictionary ProductMerchandise = new filterDictionary(29, typeof(entryProductMerchandise), filterNavigationTypes.Condition, filterToolbarImages.HeaderMerchandise, "Merchandise", "Sets the starting node for search.", costToRunEstimate: 10);
        public static readonly filterDictionary ProductLevels = new filterDictionary(30, typeof(entryProductLevels), filterNavigationTypes.Condition, filterToolbarImages.ProductLevels, "Levels", "Limits the search to selected hierarchy levels.", costToRunEstimate: 10);
        public static readonly filterDictionary ProductHierarchies = new filterDictionary(31, typeof(entryProductHierarchies), filterNavigationTypes.Condition, filterToolbarImages.ProductHierarchies, "Hierarchies", "Limits the search to selected hierarchies.", costToRunEstimate: 10);
        public static readonly filterDictionary ProductSortBy = new filterDictionary(32, typeof(entryProductSortBy), filterNavigationTypes.SortBy, filterToolbarImages.SortBy, "Sort", "Sorts the search by the selected field.", costToRunEstimate: 0);
        //End TT#1388-MD -jsobek -Product Filters

        //Begin TT#1443-MD -jsobek -Audit Filter
        public static readonly filterDictionary Users = new filterDictionary(33, typeof(entryUsers), filterNavigationTypes.Condition, filterToolbarImages.User, "User", "Allows searching by user or groups of users.", costToRunEstimate: 10);
   
        public static readonly filterDictionary AuditStartTime = new filterDictionary(34, typeof(entryAuditProcessTime), filterNavigationTypes.Condition, filterToolbarImages.Date1, "Start Time", "Allows searching by process start date and time.", costToRunEstimate: 10);
        public static readonly filterDictionary AuditDetailTime = new filterDictionary(35, typeof(entryAuditDetailTime), filterNavigationTypes.Condition, filterToolbarImages.Date2, "Detail Time", "Allows searching by detail start date and time.", costToRunEstimate: 10);
        public static readonly filterDictionary AuditFields = new filterDictionary(36, typeof(entryAuditFields), filterNavigationTypes.Condition, filterToolbarImages.Field, "Field", "Allows searching by various audit fields.", costToRunEstimate: 10);
        public static readonly filterDictionary AuditExecutionStatus = new filterDictionary(37, typeof(entryAuditExecutionStatus), filterNavigationTypes.Condition, filterToolbarImages.Status, "Exec Status", "Allows searching by process execution status.", costToRunEstimate: 10);
        public static readonly filterDictionary AuditCompletionStatus = new filterDictionary(38, typeof(entryAuditCompletionStatus), filterNavigationTypes.Condition, filterToolbarImages.Status2, "Comp Status", "Allows searching by process completion status.", costToRunEstimate: 10); 
        public static readonly filterDictionary AuditProcessMessageLevel = new filterDictionary(39, typeof(entryAuditProcessMessageLevel), filterNavigationTypes.Condition, filterToolbarImages.ProductLevels, "Msg Lvl", "Restricts the search to the selected highest process messages levels.", costToRunEstimate: 10);
        public static readonly filterDictionary AuditDetailMessageLevel = new filterDictionary(40, typeof(entryAuditDetailMessageLevel), filterNavigationTypes.Condition, filterToolbarImages.ProductHierarchies, "Detail Lvl", "Restricts the search to the selected highest detail messages levels.", costToRunEstimate: 10);
        public static readonly filterDictionary AuditSortBy = new filterDictionary(41, typeof(entryAuditSortBy), filterNavigationTypes.SortBy, filterToolbarImages.SortBy, "Sort", "Sorts the search by the selected field.", costToRunEstimate: 0);
        //End TT#1443-MD -jsobek -Audit Filter

        //Begin TT#1414-MD -jsobek -Attribute Set Filter
        public static readonly filterDictionary StoreGroupName = new filterDictionary(42, typeof(entryStoreGroupName), filterNavigationTypes.Condition, filterToolbarImages.ProductLevels, "Set", "Inserts a new store attribute set.", costToRunEstimate: 10);
        public static readonly filterDictionary StoreGroupExclusionList = new filterDictionary(43, typeof(entryStoreGroupExclusionList), filterNavigationTypes.Condition, filterToolbarImages.ProductAny, "Avail Stores", "Remaining stores.", costToRunEstimate: 10);
        public static readonly filterDictionary StoreGroupDynamic = new filterDictionary(44, typeof(entryStoreGroupDynamic), filterNavigationTypes.Condition, filterToolbarImages.AttributeSet, "Dynamic Set", "Inserts a new dynamic store attribute set.", costToRunEstimate: 10);
        public static readonly filterDictionary StoreGroupOverride = new filterDictionary(45, typeof(entryStoreGroupOverride), filterNavigationTypes.Condition, filterToolbarImages.AttributeSet, "Override", "Inserts a new dynamic set override condition.", costToRunEstimate: 10);
      
        //End TT#1414-MD -jsobek -Attribute Set Filter

        // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only 
        public static readonly filterDictionary AssortmentTypes = new filterDictionary(46, typeof(entryAssortmentTypes), filterNavigationTypes.Condition, filterToolbarImages.HeaderType, "Type", "Adds an Assortment Type condition to the filter.", costToRunEstimate: 10);
        public static readonly filterDictionary AssortmentStatus = new filterDictionary(47, typeof(entryAssortmentStatus), filterNavigationTypes.Condition, filterToolbarImages.Status, "Status", "Adds an Assortment Status condition to the filter.", costToRunEstimate: 10);
        public static readonly filterDictionary AssortmentFields = new filterDictionary(48, typeof(entryAssortmentFields), filterNavigationTypes.Condition, filterToolbarImages.Field, "Field", "Adds an Assortment Field condition to the filter.", costToRunEstimate: 10);
        public static readonly filterDictionary AssortmentMerchandise = new filterDictionary(49, typeof(entryAssortmentMerchandise), filterNavigationTypes.Condition, filterToolbarImages.HeaderMerchandise, "Apply To", "Adds an Apply To condition to the filter.", costToRunEstimate: 10);
        public static readonly filterDictionary AssortmentDate = new filterDictionary(50, typeof(entryAssortmentDate), filterNavigationTypes.Condition, filterToolbarImages.Date1, "Date", "Adds an Assortment Date condition to the filter.", costToRunEstimate: 10);
        public static readonly filterDictionary AssortmentSortBy = new filterDictionary(51, typeof(entryAssortmentSortBy), filterNavigationTypes.SortBy, filterToolbarImages.SortBy, "Sort", "Adds a Sort By entry to the filter.", costToRunEstimate: 0); 
        // End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only 

        private filterDictionary(int dbIndex, Type entryType, filterNavigationTypes navigationType, filterToolbarImages buttonImage, string buttonText, string buttonToolTip, int costToRunEstimate)
        {
            this.dbIndex = dbIndex;
            this.entryType = entryType;
            this.navigationType = navigationType;
            this.costToRunEstimate = costToRunEstimate;
            this.buttonImage = buttonImage;
            this.buttonText = buttonText;
            this.buttonToolTip = buttonToolTip;
            dictionaryList.Add(this);
        }

        public Type entryType { get; private set; }
        public filterNavigationTypes navigationType { get; private set; }
        public int dbIndex { get; private set; }
        public int costToRunEstimate { get; private set; }
        public filterToolbarImages buttonImage { get; private set; }
        public string buttonText { get; private set; }
        public string buttonToolTip { get; private set; }
        public static implicit operator int(filterDictionary op) { return op.dbIndex; }


        public static filterDictionary FromIndex(int dbIndex)
        {
            filterDictionary result = dictionaryList.Find(
               delegate(filterDictionary ft)
               {
                   return ft.dbIndex == dbIndex;
               }
               );
            return result;
        }
    }

   

}
