using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDRetail.Business
{
    /// <summary>
    /// This class is used by the filter container to map dictionary entries to their UI elements
    /// </summary>
    public sealed class filterElementMap
    {
        public static List<filterElementMap> typeList = new List<filterElementMap>();
        public static readonly filterElementMap Calendar = new filterElementMap(0);
        public static readonly filterElementMap Field = new filterElementMap(1);
        public static readonly filterElementMap Folder = new filterElementMap(2);
        public static readonly filterElementMap Info = new filterElementMap(3);
        public static readonly filterElementMap Limit = new filterElementMap(4);
        public static readonly filterElementMap List = new filterElementMap(5);
        public static readonly filterElementMap Logic = new filterElementMap(6);
        public static readonly filterElementMap Merchandise = new filterElementMap(7);
        public static readonly filterElementMap Name = new filterElementMap(8);
        public static readonly filterElementMap OperatorDate = new filterElementMap(9);
        public static readonly filterElementMap OperatorIn = new filterElementMap(10);
        public static readonly filterElementMap OperatorNumeric = new filterElementMap(11);
        public static readonly filterElementMap OperatorNumericForVariables = new filterElementMap(12);
        public static readonly filterElementMap OperatorString = new filterElementMap(13);
        public static readonly filterElementMap OperatorVariablePercentage = new filterElementMap(14);
        public static readonly filterElementMap SortBy = new filterElementMap(15);
        public static readonly filterElementMap SortByDirection = new filterElementMap(16);
        public static readonly filterElementMap SortByType = new filterElementMap(17);
        public static readonly filterElementMap ValueToCompareBool = new filterElementMap(18);
        public static readonly filterElementMap ValueToCompareDateBetween = new filterElementMap(19);
        public static readonly filterElementMap ValueToCompareDateSpecify = new filterElementMap(20);
        public static readonly filterElementMap ValueToCompareNumeric = new filterElementMap(21);
        public static readonly filterElementMap ValueToCompareNumericBetween = new filterElementMap(22);
        public static readonly filterElementMap ValueToCompareString = new filterElementMap(23);
        public static readonly filterElementMap Variable = new filterElementMap(24);
        public static readonly filterElementMap ExclusionList = new filterElementMap(25);
        public static readonly filterElementMap NameAndLimit = new filterElementMap(26);
        public static readonly filterElementMap DynamicSet = new filterElementMap(27);
        public static readonly filterElementMap DynamicSetOverride = new filterElementMap(28);
        public static readonly filterElementMap OperatorCalendarDate = new filterElementMap(29);   // TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only

        private filterElementMap(int Index)
        {
            this.Index = Index;
            typeList.Add(this);
        }

        public int Index { get; private set; }
        public static implicit operator int(filterElementMap op) { return op.Index; }


        public static filterElementMap FromIndex(int Index)
        {
            filterElementMap result = typeList.Find(
               delegate(filterElementMap ft)
               {
                   return ft.Index == Index;
               }
               );
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

    }
}
