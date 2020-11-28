//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using MIDRetail.DataCommon;
//using MIDRetail.Data;
//using MIDRetail.Business;

//namespace MIDRetail.Windows.Controls
//{
//    public interface IFilterElement
//    {
//        void LoadFromCondition(filter f, filterCondition condition);
//        void SaveToCondition(ref filter f, ref filterCondition condition);
//        void SetElementBase(elementBase eb, elementGroupSettings groupSettings);
//        void ClearControls();
//    }
//    public class elementBase
//    {
//        public bool isLoading;
//        public filterManager manager;
//        //elementTypes elementType;
//        public IFilterElement elementInterface;
//        public Type elementUI;
//        public string groupHeading;
//        public bool isField;
//        public bool isVariable;
//        public bool isSortBy;
//        public bool isOperatorNumeric;
//        public bool isOperatorDate;
//        public bool isList;
//        public bool loadFromVariable1 = false;
//        public bool loadFromVariable2 = false;
//        public bool loadFromHeaderMerchandise = false;
//        // public dateTypes dateType;
//        // public numericTypes numericType;
//        public bool useDynamicOperator = false;
//        public valueInfoTypes valueInfoType;

//        public elementBase(filterManager manager, Type elementUI, string groupHeading, bool isList = false)
//        {
//            // this.elementType = elementType;
//            this.manager = manager;
//            this.elementUI = elementUI;
//            this.groupHeading = groupHeading;
//            this.isList = isList;
//        }

//        public void LoadFromCondition(filterCondition condition)
//        {
//            isLoading = true;
//            elementInterface.LoadFromCondition(manager.currentFilter, condition);
//            isLoading = false;
//        }
//        public void SaveToCondition(ref filterCondition condition)
//        {
//            elementInterface.SaveToCondition(ref manager.currentFilter, ref condition);
//        }
//        public void SaveValueInfoTypeToCondition(ref filterCondition condition, valueInfoTypes valueInfoType)
//        {
//            if (valueInfoType.valueType == valueTypes.Text)
//            {
//            }
//            else if (valueInfoType.valueType == valueTypes.Boolean)
//            {
//            }
//            else if (valueInfoType.valueType == valueTypes.List)
//            {
//            }
//            else if (valueInfoType.valueType == valueTypes.Date)
//            {
//                condition.dateTypeIndex = valueInfoType.dateType.Index;
//            }
//            else if (valueInfoType.valueType == valueTypes.Numeric || valueInfoType.valueType == valueTypes.Dollar)
//            {
//                condition.numericTypeIndex = valueInfoType.numericType.Index;
//            }
//        }

//        public void MakeConditionDirty()
//        {
//            if (manager != null && isLoading == false)
//            {
//                manager.MakeConditionDirty();
//            }
//        }

//    }

//    public class elementGroup
//    {
//        public filterTypes filterType { get; set; }
//        public navigationTypes navigationType = navigationTypes.Condition;
//        //public string conditionTypeName { get; set; }
//        public int costToRunEstimate { get; set; }
//        public elementGroupSettings groupSettings = new elementGroupSettings();
//        public List<elementBase> elementList = new List<elementBase>();
//        public elementGroupTypes elementGroupType;

//        public void LoadFromCondition(filterManager manager, filterCondition condition)
//        {
//            foreach (elementBase eb in elementList)
//            {
//                eb.LoadFromCondition(condition);
//            }
//        }
//        public void SaveToCondition(ref filterCondition condition)
//        {
//            foreach (elementBase eb in elementList)
//            {
//                eb.SaveToCondition(ref condition);
//            }

//        }


//        public virtual void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//        }
//        public virtual void SetDefaults(ref filterCondition fc)
//        {
//        }

//        public static void BuildFormattedTextForString(ref string formattedText, optionDefinition options, filterCondition fc)
//        {
//            formattedText += filterUtility.FormatOperator(options, " " + stringOperatorTypes.FromIndex(fc.operatorIndex).description);
//            formattedText += filterUtility.FormatValue(options, " " + fc.valueToCompare);
//        }
//        public static void BuildFormattedTextForBoolean(ref string formattedText, optionDefinition options, filterCondition fc)
//        {
//            formattedText += filterUtility.FormatOperator(options, " " + numericOperatorTypes.FromIndex(fc.operatorIndex).symbol);
//            formattedText += filterUtility.FormatValue(options, " " + fc.valueToCompareBool.ToString());
//        }
//        public static void BuildFormattedTextForDate(ref string formattedText, optionDefinition options, filterCondition fc)
//        {
//            //formattedText += filterUtility.FormatOperator(options, " " + dateOperatorTypes.FromIndex(fc.operatorIndex).description);
//            dateOperatorTypes opType = dateOperatorTypes.FromIndex(fc.operatorIndex);
//            if (opType == dateOperatorTypes.Unrestricted)
//            {
//                formattedText += filterUtility.FormatValue(options, dateOperatorTypes.Unrestricted.description);
//            }
//            else
//            {
//                string outputFormat = string.Empty;
//                dateTypes dType = dateTypes.FromIndex(fc.dateTypeIndex);
//                if (dType == dateTypes.DateAndTime)
//                {
//                    if (opType == dateOperatorTypes.Last7Days)
//                    {
//                        outputFormat = "MM/dd/yyyy";
//                    }
//                    else
//                    {
//                        outputFormat = "MM/dd/yyyy hh:mm tt";
//                    }
//                }
//                else if (dType == dateTypes.TimeOnly)
//                {
//                    outputFormat = "hh:mm tt";
//                }
//                else //if (dType == dateTypes.DateOnly)
//                {
//                    outputFormat = "MM/dd/yyyy";
//                }

//                DateTime dateFrom = DateTime.Now;
//                if (fc.valueToCompareDateFrom != null) dateFrom = (DateTime)fc.valueToCompareDateFrom;
//                DateTime dateTo = DateTime.Now;
//                if (fc.valueToCompareDateTo != null) dateTo = (DateTime)fc.valueToCompareDateTo;

//                if (opType == dateOperatorTypes.Specify)
//                {
//                    formattedText += filterUtility.FormatValue(options, dateFrom.ToString(outputFormat));
//                    formattedText += filterUtility.FormatOperator(options, " to ");
//                    formattedText += filterUtility.FormatValue(options, dateTo.ToString(outputFormat));
//                }
//                else if (opType == dateOperatorTypes.Between)
//                {
//                    formattedText += filterUtility.FormatValue(options, dateFrom.ToString(outputFormat));
//                    formattedText += filterUtility.FormatOperator(options, " to ");
//                    formattedText += filterUtility.FormatValue(options, dateTo.ToString(outputFormat));
//                    formattedText += filterUtility.FormatOperator(options, " (" + dateOperatorTypes.Between.description + ")");
//                }
//                else if (opType == dateOperatorTypes.Last7Days)
//                {
//                    formattedText += filterUtility.FormatValue(options, dateFrom.ToString(outputFormat));
//                    formattedText += filterUtility.FormatOperator(options, " to ");
//                    formattedText += filterUtility.FormatValue(options, dateTo.ToString(outputFormat));
//                    formattedText += filterUtility.FormatOperator(options, " (" + dateOperatorTypes.Last7Days.description + ")");
//                }
//                else if (opType == dateOperatorTypes.Last24Hours)
//                {
//                    formattedText += filterUtility.FormatValue(options, dateFrom.ToString(outputFormat));
//                    formattedText += filterUtility.FormatOperator(options, " to ");
//                    formattedText += filterUtility.FormatValue(options, dateTo.ToString(outputFormat));
//                    formattedText += filterUtility.FormatOperator(options, " (" + dateOperatorTypes.Last24Hours.description + ")");
//                }
//            }
//        }
//        public static void BuildFormattedTextForNumeric(ref string formattedText, optionDefinition options, filterCondition fc, valueTypes vt)
//        {
//            numericOperatorTypes opType = numericOperatorTypes.FromIndex(fc.operatorIndex);
//            numericTypes nType = numericTypes.FromIndex(fc.numericTypeIndex);
//            formattedText += filterUtility.FormatOperator(options, " " + opType.symbol);

//            string outputFormat = string.Empty;
//            if (nType == numericTypes.Dollar)
//            {
//                outputFormat = "$###,###,###,###,##0.00";
//            }
//            else if (nType == numericTypes.Integer)
//            {
//                outputFormat = "###,###,###,###,##0";
//            }
//            else //if (nType == numericTypes.DoubleFreeForm)
//            {
//                //setting to 4 decimal places as the default
//                outputFormat = "###,###,###,###,##0.0000";
//            }

//            if (nType == numericTypes.Integer)
//            {
//                if (fc.valueToCompareInt == null)
//                {
//                    fc.valueToCompare = string.Empty;
//                }
//                else
//                {
//                    if (numericOperatorTypes.FromIndex(fc.operatorIndex) == numericOperatorTypes.Between)
//                    {
//                        int i = (int)fc.valueToCompareInt;
//                        int i2 = (int)fc.valueToCompareInt2;
//                        fc.valueToCompare = i.ToString(outputFormat);
//                        fc.valueToCompare += " and ";
//                        fc.valueToCompare += i2.ToString(outputFormat);
//                    }
//                    else
//                    {
//                        int i = (int)fc.valueToCompareInt;
//                        fc.valueToCompare = i.ToString(outputFormat);
//                    }
//                }
//            }
//            else
//            {
//                if (fc.valueToCompareDouble == null)
//                {
//                    fc.valueToCompare = string.Empty;
//                }
//                else
//                {
//                    if (numericOperatorTypes.FromIndex(fc.operatorIndex) == numericOperatorTypes.Between)
//                    {
//                        double d = (double)fc.valueToCompareDouble;
//                        double d2 = (double)fc.valueToCompareDouble2;
//                        fc.valueToCompare = d.ToString(outputFormat);
//                        fc.valueToCompare += " and ";
//                        fc.valueToCompare += d2.ToString(outputFormat);
//                    }
//                    else
//                    {
//                        double d = (double)fc.valueToCompareDouble;
//                        fc.valueToCompare = d.ToString(outputFormat);
//                    }
//                }
//            }



//            formattedText += filterUtility.FormatValue(options, " " + fc.valueToCompare);  //string to hold all numeric types
//        }
//        public static void BuildFormattedTextForSortBy(ref string formattedText, optionDefinition options, filterCondition fc)
//        {
//            if (fc.sortByTypeIndex == sortByTypes.StoreCharacteristics)
//            {
//                formattedText += filterUtility.FormatNormal(options, " " + filterDataHelper.StoreCharacteristicsGetNameFromIndex(fc.sortByFieldIndex));
//            }
//            else if (fc.sortByTypeIndex == sortByTypes.HeaderCharacteristics)
//            {
//                formattedText += filterUtility.FormatNormal(options, " " + filterDataHelper.HeaderCharacteristicsGetNameFromIndex(fc.sortByFieldIndex));
//            }
//            else if (fc.sortByTypeIndex == sortByTypes.StoreFields)
//            {
//                formattedText += filterUtility.FormatNormal(options, " " + storeFieldTypes.FromIndex(fc.sortByFieldIndex).Name);
//            }
//            else if (fc.sortByTypeIndex == sortByTypes.HeaderFields)
//            {
//                formattedText += filterUtility.FormatNormal(options, " " + headerFieldTypes.FromIndex(fc.sortByFieldIndex).Name);
//            }
//            //else if (fc.sortByTypeIndex == sortByTypes.StoreStatus)
//            //{
//            //    formattedText += filterUtility.FormatNormal(options, " " + storeStatusTypes.FromIndex(fc.sortByFieldIndex).Name);
//            //}
//            //else if (fc.sortByTypeIndex == sortByTypes.HeaderStatus)
//            //{
//            //    formattedText += filterUtility.FormatNormal(options, " " + filterData.GetNameForHeaderStatus(fc.sortByFieldIndex));
//            //}
//            else if (fc.sortByTypeIndex == sortByTypes.Variables)
//            {
//                formattedText += filterUtility.FormatNormal(options, " " + filterDataHelper.VariablesGetNameFromIndex(fc.sortByFieldIndex));
//            }
//            else
//            {
//                formattedText += filterUtility.FormatNormal(options, " " + sortByTypes.FromIndex(fc.sortByTypeIndex).Name);
//            }

//            formattedText += filterUtility.FormatOperator(options, " (" + sortByDirectionTypes.FromIndex(fc.operatorIndex).Name + ")");

//        }

//    }

//    public sealed class elementGroupTypes
//    {
//        public static List<elementGroupTypes> valueTypeList = new List<elementGroupTypes>();
//        public static readonly elementGroupTypes FilterName = new elementGroupTypes("FilterName", 0, typeof(elementGroupName), navigationTypes.Info, costToRunEstimate: 0);
//        public static readonly elementGroupTypes FilterFolder = new elementGroupTypes("FilterFolder", 1, typeof(elementGroupFolder), navigationTypes.Info, costToRunEstimate: 0);
//        public static readonly elementGroupTypes ResultLimit = new elementGroupTypes("FilterLimit", 2, typeof(elementGroupLimit), navigationTypes.Info, costToRunEstimate: 0);
//        public static readonly elementGroupTypes InfoFilter = new elementGroupTypes("InfoFilter", 3, typeof(elementGroupInfoFilter), navigationTypes.Info, costToRunEstimate: 0);
//        public static readonly elementGroupTypes InfoConditions = new elementGroupTypes("InfoConditions", 4, typeof(elementGroupInfoConditions), navigationTypes.Info, costToRunEstimate: 0);
//        public static readonly elementGroupTypes InfoSortBy = new elementGroupTypes("InfoSortBy", 5, typeof(elementGroupInfoSortBy), navigationTypes.Info, costToRunEstimate: 0);

//        public static readonly elementGroupTypes StoreStatus = new elementGroupTypes("Store Status", 6, typeof(elementGroupStoreStatus), navigationTypes.Condition, costToRunEstimate: 10);
//        public static readonly elementGroupTypes StoreList = new elementGroupTypes("Store List", 7, typeof(elementGroupStoreList), navigationTypes.Condition, costToRunEstimate: 10);
//        public static readonly elementGroupTypes StoreFields = new elementGroupTypes("Store Fields", 8, typeof(elementGroupStoreFields), navigationTypes.Condition, costToRunEstimate: 10);
//        public static readonly elementGroupTypes StoreCharacteristics = new elementGroupTypes("Store Characteristics", 9, typeof(elementGroupStoreCharacteristics), navigationTypes.Condition, costToRunEstimate: 100);
//        public static readonly elementGroupTypes StoreVariableToConstant = new elementGroupTypes("VariableToConstant", 10, typeof(elementGroupStoreVariableToConstant), navigationTypes.Condition, costToRunEstimate: 100);
//        public static readonly elementGroupTypes StoreVariableToVariable = new elementGroupTypes("VariableToVariable", 11, typeof(elementGroupStoreVariableToVariable), navigationTypes.Condition, costToRunEstimate: 200);
//        public static readonly elementGroupTypes StoreVariablePercentage = new elementGroupTypes("VariablePercentage", 12, typeof(elementGroupStoreVariablePercentage), navigationTypes.Condition, costToRunEstimate: 200);

//        public static readonly elementGroupTypes HeaderTypes = new elementGroupTypes("Header Types", 13, typeof(elementGroupHeaderTypes), navigationTypes.Condition, costToRunEstimate: 10);
//        public static readonly elementGroupTypes HeaderStatus = new elementGroupTypes("Header Status", 14, typeof(elementGroupHeaderStatus), navigationTypes.Condition, costToRunEstimate: 10);
//        public static readonly elementGroupTypes HeaderFields = new elementGroupTypes("Header Fields", 15, typeof(elementGroupHeaderFields), navigationTypes.Condition, costToRunEstimate: 10);
//        public static readonly elementGroupTypes HeaderCharacteristics = new elementGroupTypes("Header Characteristics", 16, typeof(elementGroupHeaderCharacteristics), navigationTypes.Condition, costToRunEstimate: 100);
//        public static readonly elementGroupTypes HeaderMerchandise = new elementGroupTypes("Header Merchandise", 17, typeof(elementGroupHeaderMerchandise), navigationTypes.Condition, costToRunEstimate: 10);
//        public static readonly elementGroupTypes HeaderDate = new elementGroupTypes("Header Date", 18, typeof(elementGroupHeaderDate), navigationTypes.Condition, costToRunEstimate: 10);
//        public static readonly elementGroupTypes HeaderReleaseDate = new elementGroupTypes("Header Release Date", 19, typeof(elementGroupHeaderReleaseDate), navigationTypes.Condition, costToRunEstimate: 10);

//        public static readonly elementGroupTypes StoreSortBy = new elementGroupTypes("StoreSortBy", 20, typeof(elementGroupStoreSortBy), navigationTypes.SortBy, costToRunEstimate: 0);
//        public static readonly elementGroupTypes HeaderSortBy = new elementGroupTypes("HeaderSortBy", 21, typeof(elementGroupHeaderSortBy), navigationTypes.SortBy, costToRunEstimate: 0);

//        private elementGroupTypes(string Name, int dbIndex, Type groupType, navigationTypes navigationType, int costToRunEstimate)
//        {
//            this.Name = Name;
//            this.dbIndex = dbIndex;
//            this.groupType = groupType;
//            this.navigationType = navigationType;
//            this.costToRunEstimate = costToRunEstimate;
//            valueTypeList.Add(this);
//        }
//        public string Name { get; private set; }
//        public Type groupType { get; private set; }
//        public navigationTypes navigationType { get; private set; }
//        public int dbIndex { get; private set; }
//        public int costToRunEstimate { get; private set; }
//        public static implicit operator int(elementGroupTypes op) { return op.dbIndex; }


//        public static elementGroupTypes FromIndex(int dbIndex)
//        {
//            elementGroupTypes result = valueTypeList.Find(
//               delegate(elementGroupTypes ft)
//               {
//                   return ft.dbIndex == dbIndex;
//               }
//               );

//            return result;

//        }
//        public static elementGroupTypes FromString(string Name)
//        {
//            elementGroupTypes result = valueTypeList.Find(
//              delegate(elementGroupTypes ft)
//              {
//                  return ft.Name == Name;
//              }
//              );
//            return result;

//        }

//        //public static Type GetGroupFromIndex(int dbIndex)
//        //{
//        //    elementGroupTypes result = valueTypeList.Find(
//        //      delegate(elementGroupTypes ft)
//        //      {
//        //          return ft.dbIndex == dbIndex;
//        //      }
//        //      );

//        //    return result.groupType;

//        //}
//        //public static elementGroup GetGroupFromString(string Name)
//        //{
//        //    //var output = FindAllDerivedTypes<elementGroup>();
//        //    //foreach (var type in output)
//        //    //{
//        //    //    Console.WriteLine(type.Name);
//        //    //}

//        //     elementGroupTypes result = valueTypeList.Find(
//        //      delegate(elementGroupTypes ft)
//        //      {
//        //          return ft.Name == Name;
//        //      }
//        //      );
//        //    return result;
//        //}

//        //public static List<Type> FindAllDerivedTypes<T>()
//        //{
//        //    return FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(T)));
//        //}

//        //public static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
//        //{
//        //    var derivedType = typeof(T);
//        //    return assembly
//        //        .GetTypes()
//        //        .Where(t =>
//        //            t != derivedType &&
//        //            derivedType.IsAssignableFrom(t)
//        //            ).ToList();

//        //} 
//    }

//    public class elementGroupSettings
//    {
//        public string groupTitle;
//        public bool isVisibleInToolbar;
//        public elementFieldSettings fieldSettings = new elementFieldSettings();
//        public elementOperatorSettings operatorSettings = new elementOperatorSettings();
//        public elementValueSettings valueSettings = new elementValueSettings();
//    }
//    public class elementFieldSettings
//    {
//        //public string infoInstructions;
//    }
//    public class elementOperatorSettings
//    {
//        //public string infoInstructions;
//    }
//    public class elementValueSettings
//    {
//        public string infoInstructions;
//        public LoadListDelegate loadFieldList;
//        public LoadListDelegate loadValueList;
//        public LoadValueListFromFieldDelegate loadValueListFromField;
//        public string fieldForData;
//        public string fieldForDisplay;
//        public string valueFieldForData;
//        public string valueFieldForDisplay;
//        public GetValueInfoTypeFromFieldIndexDelegate GetValueInfoTypeFromFieldIndex;
//        public GetNameFromFieldIndexDelegate GetNameFromField;
//        //public bool useValueListFromField;
//        //public int tempFieldIndex;
//        //public LoadRidValuesDelegate LoadRidValues;
//        public listValueTypes listValueType;

//    }



//    public class elementGroupName : elementGroup
//    {
//        public elementGroupName(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Name";
//            base.elementList.Add(new elementBase(manager, typeof(filterElementName), base.groupSettings.groupTitle));
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
//            formattedText += filterUtility.FormatOperator(options, " = ");
//            formattedText += filterUtility.FormatValue(options, fc.valueToCompare);
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.valueToCompare = "New Filter";
//        }
//    }
//    public class elementGroupFolder : elementGroup
//    {
//        //TODO - Handle templates
//        public elementGroupFolder(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Folder";
//            base.elementList.Add(new elementBase(manager, typeof(filterElementFolder), base.groupSettings.groupTitle));
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
//            formattedText += filterUtility.FormatOperator(options, " = ");
//            if (fc.valueToCompareInt == Include.GlobalUserRID)
//            {
//                formattedText += filterUtility.FormatValue(options, "Global");
//            }
//            else
//            {
//                formattedText += filterUtility.FormatValue(options, "User");
//            }
            
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.valueToCompareInt = -1;

//        }
//    }
//    public class elementGroupLimit : elementGroup
//    {
//        public elementGroupLimit(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Result Limit";
//            base.elementList.Add(new elementBase(manager, typeof(filterElementLimit), base.groupSettings.groupTitle));
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
//            formattedText += filterUtility.FormatOperator(options, " = ");
//            formattedText += filterUtility.FormatValue(options, fc.valueToCompare);
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.valueToCompare = "Unrestricted";
//        }
//    }
//    public class elementGroupInfoFilter : elementGroup
//    {
//        public elementGroupInfoFilter(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Filter Information";
//            base.groupSettings.valueSettings.infoInstructions = "Select a line to below to change filter information.";
//            base.elementList.Add(new elementBase(manager, typeof(filterElementInfo), "Filter Information"));
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//        }
//    }
//    public class elementGroupInfoConditions : elementGroup
//    {
//        public elementGroupInfoConditions(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Conditions";
//            base.groupSettings.valueSettings.infoInstructions = "Add conditions using the toolbar above.";
//            base.elementList.Add(new elementBase(manager, typeof(filterElementInfo), "Conditions"));
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//        }
//    }
//    public class elementGroupInfoSortBy : elementGroup
//    {
//        public elementGroupInfoSortBy(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Sort By";
//            base.groupSettings.valueSettings.infoInstructions = "Add sort items using the toolbar above.";
//            base.elementList.Add(new elementBase(manager, typeof(filterElementInfo), "Sort By"));
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatNormal(options, base.groupSettings.groupTitle);
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//        }
//    }
//    public class elementGroupStoreStatus : elementGroup
//    {
//        public elementGroupStoreStatus(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Store Status";
//            base.groupSettings.valueSettings.loadValueList = new LoadListDelegate(storeStatusTypes.ToDataTable);
//            base.groupSettings.valueSettings.fieldForData = "FIELD_INDEX";
//            base.groupSettings.valueSettings.fieldForDisplay = "FIELD_NAME";
//            base.groupSettings.valueSettings.listValueType = listValueTypes.StoreStatus;
//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));
//            base.elementList.Add(new elementBase(manager, typeof(filterElementOperatorIn), "In / Not In"));
//            base.elementList.Add(new elementBase(manager, typeof(filterElementList), base.groupSettings.groupTitle, true));

//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatLogic(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
//            formattedText += filterUtility.FormatOperator(options, " " + listOperatorTypes.FromIndex(fc.operatorIndex).symbol);
//            formattedText += filterUtility.FormatValue(options, " (");
//            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(listValueTypes.StoreStatus), base.groupSettings.valueSettings.loadValueList(), base.groupSettings.valueSettings.fieldForData, base.groupSettings.valueSettings.fieldForDisplay));
//            formattedText += filterUtility.FormatValue(options, ")");
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            fc.operatorIndex = listOperatorTypes.Includes;
//            fc.listConstantType = listConstantTypes.None;
//        }
//    }
//    public class elementGroupHeaderStatus : elementGroup
//    {
//        public elementGroupHeaderStatus(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Header Status";
//            base.groupSettings.valueSettings.loadValueList = new LoadListDelegate(filterDataHelper.HeaderStatusesGetDataTable);
//            base.groupSettings.valueSettings.fieldForData = "FIELD_INDEX";
//            base.groupSettings.valueSettings.fieldForDisplay = "FIELD_NAME";
//            base.groupSettings.valueSettings.listValueType = listValueTypes.HeaderStatus;
//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));
//            base.elementList.Add(new elementBase(manager, typeof(filterElementOperatorIn), "In / Not In"));
//            base.elementList.Add(new elementBase(manager, typeof(filterElementList), base.groupSettings.groupTitle, true));
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatLogic(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
//            formattedText += filterUtility.FormatOperator(options, " " + listOperatorTypes.FromIndex(fc.operatorIndex).symbol);
//            formattedText += filterUtility.FormatValue(options, " (");
//            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(listValueTypes.HeaderStatus), base.groupSettings.valueSettings.loadValueList(), base.groupSettings.valueSettings.fieldForData, base.groupSettings.valueSettings.fieldForDisplay));
//            formattedText += filterUtility.FormatValue(options, ")");
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            fc.operatorIndex = listOperatorTypes.Includes;
//            fc.listConstantType = listConstantTypes.None;
//        }
//    }
//    public class elementGroupHeaderTypes : elementGroup
//    {
//        public elementGroupHeaderTypes(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Header Types";
//            base.groupSettings.valueSettings.loadValueList = new LoadListDelegate(filterDataHelper.HeaderTypesGetDataTable);
//            base.groupSettings.valueSettings.fieldForData = "FIELD_INDEX";
//            base.groupSettings.valueSettings.fieldForDisplay = "FIELD_NAME";
//            base.groupSettings.valueSettings.listValueType = listValueTypes.HeaderTypes;
//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));
//            base.elementList.Add(new elementBase(manager, typeof(filterElementOperatorIn), "In / Not In"));
//            base.elementList.Add(new elementBase(manager, typeof(filterElementList), base.groupSettings.groupTitle, true));
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatLogic(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
//            formattedText += filterUtility.FormatOperator(options, " " + listOperatorTypes.FromIndex(fc.operatorIndex).symbol);
//            formattedText += filterUtility.FormatValue(options, " (");
//            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(listValueTypes.HeaderTypes), base.groupSettings.valueSettings.loadValueList(), base.groupSettings.valueSettings.fieldForData, base.groupSettings.valueSettings.fieldForDisplay));
//            formattedText += filterUtility.FormatValue(options, ")");
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            fc.operatorIndex = listOperatorTypes.Includes;
//            fc.listConstantType = listConstantTypes.None;
//        }
//    }
//    public class elementGroupStoreList : elementGroup
//    {
//        public elementGroupStoreList(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Store List";
//            base.groupSettings.valueSettings.loadValueList = new LoadListDelegate(filterDataHelper.StoresGetDataTable);
//            base.groupSettings.valueSettings.fieldForData = "STORE_RID";
//            base.groupSettings.valueSettings.fieldForDisplay = "STORE_NAME";
//            base.groupSettings.valueSettings.listValueType = listValueTypes.StoreRID;
//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));
//            base.elementList.Add(new elementBase(manager, typeof(filterElementOperatorIn), "In / Not In"));
//            base.elementList.Add(new elementBase(manager, typeof(filterElementList), base.groupSettings.groupTitle, true));
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatLogic(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += filterUtility.FormatNormal(options, " Store");
//            formattedText += filterUtility.FormatOperator(options, " " + listOperatorTypes.FromIndex(fc.operatorIndex).symbol);
//            formattedText += filterUtility.FormatValue(options, " (");
//            formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(listValueTypes.StoreRID), base.groupSettings.valueSettings.loadValueList(), base.groupSettings.valueSettings.fieldForData, base.groupSettings.valueSettings.fieldForDisplay));
//            formattedText += filterUtility.FormatValue(options, ")");
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            fc.operatorIndex = listOperatorTypes.Includes;
//            fc.listConstantType = listConstantTypes.None;
//        }
//    }
//    public class elementGroupStoreFields : elementGroup
//    {
//        public elementGroupStoreFields(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Store Fields";
//            base.groupSettings.valueSettings.loadFieldList = new LoadListDelegate(storeFieldTypes.ToDataTable);
//            base.groupSettings.valueSettings.GetValueInfoTypeFromFieldIndex = new GetValueInfoTypeFromFieldIndexDelegate(storeFieldTypes.GetValueTypeInfoForField);
//            base.groupSettings.valueSettings.fieldForData = "FIELD_INDEX";
//            base.groupSettings.valueSettings.fieldForDisplay = "FIELD_NAME";

//            base.groupSettings.valueSettings.GetNameFromField = new GetNameFromFieldIndexDelegate(storeFieldTypes.GetNameFromIndex);
//            base.groupSettings.valueSettings.loadValueListFromField = new LoadValueListFromFieldDelegate(storeFieldTypes.GetValueListForStoreFields);
//            base.groupSettings.valueSettings.valueFieldForData = "VALUE_INDEX";
//            base.groupSettings.valueSettings.valueFieldForDisplay = "VALUE_NAME";
//            base.groupSettings.valueSettings.listValueType = listValueTypes.StoreField;
//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));
//            elementBase eb = new elementBase(manager, typeof(filterElementField), "Store Field");
//            eb.isField = true;
//            base.elementList.Add(eb);
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatLogic(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += filterUtility.FormatNormal(options, " " + storeFieldTypes.FromIndex(fc.fieldIndex).Name);
//            valueTypes vt = valueTypes.FromIndex(fc.valueTypeIndex);
//            if (vt == valueTypes.List)
//            {
//                formattedText += filterUtility.FormatOperator(options, " " + listOperatorTypes.FromIndex(fc.operatorIndex).symbol);
//                formattedText += filterUtility.FormatValue(options, " (");
//                formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(listValueTypes.StoreField), base.groupSettings.valueSettings.loadValueListFromField(fc.fieldIndex), base.groupSettings.valueSettings.valueFieldForData, base.groupSettings.valueSettings.valueFieldForDisplay));
//                formattedText += filterUtility.FormatValue(options, ")");
//            }
//            else if (vt == valueTypes.Text)
//            {
//                BuildFormattedTextForString(ref formattedText, options, fc);
//            }
//            else if (vt == valueTypes.Boolean)
//            {
//                BuildFormattedTextForBoolean(ref formattedText, options, fc);
//            }
//            else if (vt == valueTypes.Date)
//            {
//                BuildFormattedTextForDate(ref formattedText, options, fc);
//            }
//            else if (vt == valueTypes.Numeric || vt == valueTypes.Dollar)
//            {
//                BuildFormattedTextForNumeric(ref formattedText, options, fc, vt);
//            }

//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            fc.fieldIndex = 0;
//            fc.operatorIndex = stringOperatorTypes.Contains;
//        }
//    }
//    public class elementGroupHeaderFields : elementGroup
//    {
//        public elementGroupHeaderFields(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Header Fields";
//            base.groupSettings.valueSettings.loadFieldList = new LoadListDelegate(headerFieldTypes.ToDataTable);
//            base.groupSettings.valueSettings.GetValueInfoTypeFromFieldIndex = new GetValueInfoTypeFromFieldIndexDelegate(headerFieldTypes.GetValueTypeForField);
//            base.groupSettings.valueSettings.fieldForData = "FIELD_INDEX";
//            base.groupSettings.valueSettings.fieldForDisplay = "FIELD_NAME";

//            base.groupSettings.valueSettings.GetNameFromField = new GetNameFromFieldIndexDelegate(headerFieldTypes.GetNameFromIndex);
//            base.groupSettings.valueSettings.loadValueListFromField = new LoadValueListFromFieldDelegate(headerFieldTypes.GetValueListForHeaderFields);
//            base.groupSettings.valueSettings.valueFieldForData = "VALUE_INDEX";
//            base.groupSettings.valueSettings.valueFieldForDisplay = "VALUE_NAME";
//            base.groupSettings.valueSettings.listValueType = listValueTypes.HeaderField;
//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));
//            elementBase eb = new elementBase(manager, typeof(filterElementField), "Header Field");
//            eb.isField = true;
//            base.elementList.Add(eb);
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatLogic(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += filterUtility.FormatNormal(options, " " + headerFieldTypes.FromIndex(fc.fieldIndex).Name);
//            valueTypes vt = valueTypes.FromIndex(fc.valueTypeIndex);
//            if (vt == valueTypes.List)
//            {
//                formattedText += filterUtility.FormatOperator(options, " " + listOperatorTypes.FromIndex(fc.operatorIndex).symbol);
//                formattedText += filterUtility.FormatValue(options, " (");
//                formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(listValueTypes.HeaderField), base.groupSettings.valueSettings.loadValueListFromField(fc.fieldIndex), base.groupSettings.valueSettings.valueFieldForData, base.groupSettings.valueSettings.valueFieldForDisplay));
//                formattedText += filterUtility.FormatValue(options, ")");
//            }
//            else if (vt == valueTypes.Text)
//            {
//                BuildFormattedTextForString(ref formattedText, options, fc);
//            }
//            else if (vt == valueTypes.Boolean)
//            {
//                BuildFormattedTextForBoolean(ref formattedText, options, fc);
//            }
//            else if (vt == valueTypes.Date)
//            {
//                BuildFormattedTextForDate(ref formattedText, options, fc);
//            }
//            else if (vt == valueTypes.Numeric || vt == valueTypes.Dollar)
//            {
//                BuildFormattedTextForNumeric(ref formattedText, options, fc, vt);
//            }

//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            fc.fieldIndex = 0;
//            fc.operatorIndex = stringOperatorTypes.Contains;
//        }
//    }
//    public class elementGroupStoreCharacteristics : elementGroup
//    {
//        public elementGroupStoreCharacteristics(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Store Characteristics";
//            base.groupSettings.valueSettings.loadFieldList = new LoadListDelegate(filterDataHelper.StoreCharacteristicsGetDataTable);
//            base.groupSettings.valueSettings.GetValueInfoTypeFromFieldIndex = new GetValueInfoTypeFromFieldIndexDelegate(filterDataHelper.StoreCharacteristicsGetValueInfoType);
//            base.groupSettings.valueSettings.GetNameFromField = new GetNameFromFieldIndexDelegate(filterDataHelper.StoreCharacteristicsGetNameFromIndex);
//            base.groupSettings.valueSettings.fieldForData = "FIELD_INDEX";
//            base.groupSettings.valueSettings.fieldForDisplay = "FIELD_NAME";

//            base.groupSettings.valueSettings.loadValueListFromField = new LoadValueListFromFieldDelegate(filterDataHelper.StoreCharacteristicsGetValuesForGroup);
//            base.groupSettings.valueSettings.valueFieldForData = "SC_RID";
//            base.groupSettings.valueSettings.valueFieldForDisplay = "CHAR_VALUE";
//            base.groupSettings.valueSettings.listValueType = listValueTypes.StoreCharacteristicRID;
//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));
//            elementBase eb = new elementBase(manager, typeof(filterElementField), "Characteristics");
//            eb.isField = true;
//            base.elementList.Add(eb);
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatLogic(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.valueSettings.GetNameFromField(fc.fieldIndex));
//            valueTypes vt = valueTypes.FromIndex(fc.valueTypeIndex);
//            if (vt == valueTypes.List)
//            {
//                formattedText += filterUtility.FormatOperator(options, " " + listOperatorTypes.FromIndex(fc.operatorIndex).symbol);
//                formattedText += filterUtility.FormatValue(options, " (");
//                formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(listValueTypes.StoreCharacteristicRID), base.groupSettings.valueSettings.loadValueListFromField(fc.fieldIndex), base.groupSettings.valueSettings.valueFieldForData, base.groupSettings.valueSettings.valueFieldForDisplay));
//                formattedText += filterUtility.FormatValue(options, ")");
//            }
//            else if (vt == valueTypes.Text)
//            {
//                BuildFormattedTextForString(ref formattedText, options, fc);
//            }
//            else if (vt == valueTypes.Boolean)
//            {
//                BuildFormattedTextForBoolean(ref formattedText, options, fc);
//            }
//            else if (vt == valueTypes.Date)
//            {
//                BuildFormattedTextForDate(ref formattedText, options, fc);
//            }
//            else if (vt == valueTypes.Numeric || vt == valueTypes.Dollar)
//            {
//                BuildFormattedTextForNumeric(ref formattedText, options, fc, vt);
//            }


//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            //assumes there is a characteristic
//            //assumes the first characterist has index of one
//            //assumes first characteristic is a string type
//            fc.fieldIndex = 1;
//            fc.operatorIndex = stringOperatorTypes.Contains;
//        }
//    }
//    public class elementGroupHeaderCharacteristics : elementGroup
//    {
//        public elementGroupHeaderCharacteristics(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Header Characteristics";
//            base.groupSettings.valueSettings.loadFieldList = new LoadListDelegate(filterDataHelper.HeaderCharacteristicsGetDataTable);
//            base.groupSettings.valueSettings.GetValueInfoTypeFromFieldIndex = new GetValueInfoTypeFromFieldIndexDelegate(filterDataHelper.HeaderCharacteristicsGetValueInfoType);
//            base.groupSettings.valueSettings.GetNameFromField = new GetNameFromFieldIndexDelegate(filterDataHelper.HeaderCharacteristicsGetNameFromIndex);
//            base.groupSettings.valueSettings.fieldForData = "FIELD_INDEX";
//            base.groupSettings.valueSettings.fieldForDisplay = "FIELD_NAME";

//            base.groupSettings.valueSettings.loadValueListFromField = new LoadValueListFromFieldDelegate(filterDataHelper.HeaderCharacteristicsGetValuesForGroup);
//            base.groupSettings.valueSettings.valueFieldForData = "HC_RID";
//            base.groupSettings.valueSettings.valueFieldForDisplay = "CHAR_VALUE";
//            base.groupSettings.valueSettings.listValueType = listValueTypes.HeaderCharacteristicRID;
//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));
//            elementBase eb = new elementBase(manager, typeof(filterElementField), "Characteristics");
//            eb.isField = true;
//            base.elementList.Add(eb);
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatLogic(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.valueSettings.GetNameFromField(fc.fieldIndex));
//            valueTypes vt = valueTypes.FromIndex(fc.valueTypeIndex);
//            if (vt == valueTypes.List)
//            {
//                formattedText += filterUtility.FormatOperator(options, " " + listOperatorTypes.FromIndex(fc.operatorIndex).symbol);
//                formattedText += filterUtility.FormatValue(options, " (");
//                formattedText += filterUtility.FormatValue(options, filterUtility.GetDisplayList(fc.listConstantType, fc.GetListValues(listValueTypes.HeaderCharacteristicRID), base.groupSettings.valueSettings.loadValueListFromField(fc.fieldIndex), base.groupSettings.valueSettings.valueFieldForData, base.groupSettings.valueSettings.valueFieldForDisplay));
//                formattedText += filterUtility.FormatValue(options, ")");
//            }
//            else if (vt == valueTypes.Text)
//            {
//                BuildFormattedTextForString(ref formattedText, options, fc);
//            }
//            else if (vt == valueTypes.Boolean)
//            {
//                BuildFormattedTextForBoolean(ref formattedText, options, fc);
//            }
//            else if (vt == valueTypes.Date)
//            {
//                BuildFormattedTextForDate(ref formattedText, options, fc);
//            }
//            else if (vt == valueTypes.Numeric || vt == valueTypes.Dollar)
//            {
//                BuildFormattedTextForNumeric(ref formattedText, options, fc, vt);
//            }


//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            //assumes there is a characteristic
//            //assumes the first characterist has index of one
//            //assumes first characteristic is a string type
//            fc.fieldIndex = 1;
//            fc.operatorIndex = stringOperatorTypes.Contains;
//        }
//    }
//    public class elementGroupHeaderMerchandise : elementGroup
//    {
//        public elementGroupHeaderMerchandise(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Merchandise";
//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));
//            elementBase b1 = new elementBase(manager, typeof(filterElementMerchandise), "Merchandise");
//            b1.loadFromHeaderMerchandise = true;
//            base.elementList.Add(b1);
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatOperator(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += filterUtility.FormatNormal(options, " " + base.groupSettings.groupTitle);
//            formattedText += filterUtility.FormatOperator(options, " " + numericOperatorTypes.FromIndex(fc.operatorIndex).symbol);
//            formattedText += filterUtility.FormatValue(options, " " + filterUtility.getTextFromHierarchyNodeDelegate(fc.headerMerchandise_HN_RID));
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            fc.operatorIndex = numericOperatorTypes.DoesEqual;
//            fc.headerMerchandise_HN_RID = -1;
//        }
//    }
//    public class elementGroupStoreVariablePercentage : elementGroup
//    {
//        public elementGroupStoreVariablePercentage(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Variable Percentage";

//            base.groupSettings.valueSettings.loadFieldList = new LoadListDelegate(filterDataHelper.VariablesGetDataTable);
//            base.groupSettings.valueSettings.GetValueInfoTypeFromFieldIndex = new GetValueInfoTypeFromFieldIndexDelegate(filterDataHelper.VariablesGetValueInfoType);
//            base.groupSettings.valueSettings.fieldForData = "FIELD_INDEX";
//            base.groupSettings.valueSettings.fieldForDisplay = "FIELD_NAME";

//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));

//            elementBase b1 = new elementBase(manager, typeof(filterElementVariable), "1st Variable:");
//            b1.loadFromVariable1 = true;
//            b1.loadFromVariable2 = false;
//            base.elementList.Add(b1);

//            elementBase bPercentOp = new elementBase(manager, typeof(filterElementOperatorVariablePercentage), "Percent Operator");
//            valueInfoTypes vInfo2 = new valueInfoTypes(valueTypes.Numeric);
//            vInfo2.numericType = numericTypes.DoubleFreeForm;
//            bPercentOp.valueInfoType = vInfo2;
//            base.elementList.Add(bPercentOp);

//            elementBase b2 = new elementBase(manager, typeof(filterElementVariable), "2nd Variable:");
//            b2.loadFromVariable1 = false;
//            b2.loadFromVariable2 = true;
//            base.elementList.Add(b2);

//            elementBase bOp = new elementBase(manager, typeof(filterElementOperatorNumeric), "Operator");
//            bOp.isOperatorNumeric = true;
//            valueInfoTypes vInfo = new valueInfoTypes(valueTypes.Numeric);
//            vInfo.numericType = numericTypes.DoubleFreeForm;
//            bOp.valueInfoType = vInfo;
//            base.elementList.Add(bOp);

//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatLogic(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += " " + filterUtility.BuildFormattedTextForVariable(options, fc, true, false);
//            formattedText += filterUtility.FormatOperator(options, " " + percentageOperatorTypes.FromIndex(fc.operatorVariablePercentageIndex).symbol);
//            formattedText += " " + filterUtility.BuildFormattedTextForVariable(options, fc, false, true);
//            //formattedText += filterUtility.FormatOperator(options, " " + numericOperatorTypes.FromIndex(fc.operatorIndex).symbol);
//            //formattedText += filterUtility.FormatValue(options, " " + fc.valueToCompare);
//            BuildFormattedTextForNumeric(ref formattedText, options, fc, valueTypes.Numeric);
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            fc.variable1_Index = filterDataHelper.VariablesGetIndexFromName("Sales Reg");
//            fc.variable1_VersionIndex = filterDataHelper.VersionsGetIndexFromName("Default to Plan");
//            fc.variable1_HN_RID = -1;
//            fc.variable1_CDR_RID = Include.UndefinedCalendarDateRange; 
//            fc.variable1_VariableValueTypeIndex = variableValueTypes.StoreDetail.dbIndex;
//            fc.variable1_TimeTypeIndex = variableTimeTypes.Any.dbIndex;

//            fc.operatorVariablePercentageIndex = percentageOperatorTypes.PercentOf;

//            fc.variable2_Index = filterDataHelper.VariablesGetIndexFromName("Stock Reg");
//            fc.variable2_VersionIndex = filterDataHelper.VersionsGetIndexFromName("Default to Plan");
//            fc.variable2_HN_RID = -1;
//            fc.variable2_CDR_RID = Include.UndefinedCalendarDateRange;
//            fc.variable2_VariableValueTypeIndex = variableValueTypes.StoreDetail.dbIndex;
//            fc.variable2_TimeTypeIndex = variableTimeTypes.Any.dbIndex;

//            fc.operatorIndex = numericOperatorTypes.GreaterThan;
//            fc.valueTypeIndex = valueTypes.Numeric;
//            fc.numericTypeIndex = numericTypes.DoubleFreeForm;
//            fc.valueToCompareDouble = .25;
//        }
//    }
//    public class elementGroupStoreVariableToConstant : elementGroup
//    {
//        public elementGroupStoreVariableToConstant(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Variable to Constant";
//            base.groupSettings.valueSettings.loadFieldList = new LoadListDelegate(filterDataHelper.VariablesGetDataTable);
//            base.groupSettings.valueSettings.GetValueInfoTypeFromFieldIndex = new GetValueInfoTypeFromFieldIndexDelegate(filterDataHelper.VariablesGetValueInfoType);
//            base.groupSettings.valueSettings.fieldForData = "FIELD_INDEX";
//            base.groupSettings.valueSettings.fieldForDisplay = "FIELD_NAME";

//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));

//            elementBase b1 = new elementBase(manager, typeof(filterElementVariable), "Variable:");
//            b1.isVariable = true;
//            b1.loadFromVariable1 = true;
//            b1.loadFromVariable2 = false;
//            b1.useDynamicOperator = true;
//            base.elementList.Add(b1);

//            //elementBase bOp = new elementBase(manager, typeof(filterElementOperatorNumeric), "Operator");
//            //bOp.isOperatorNumeric = true;
//            //base.elementList.Add(bOp);
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatLogic(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += " " + filterUtility.BuildFormattedTextForVariable(options, fc, true, false);

//            BuildFormattedTextForNumeric(ref formattedText, options, fc, valueTypes.Numeric);
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            fc.variable1_Index = filterDataHelper.VariablesGetIndexFromName("Sales Reg");
//            fc.variable1_VersionIndex = filterDataHelper.VersionsGetIndexFromName("Default to Plan");
//            fc.variable1_HN_RID = -1;
//            fc.variable1_CDR_RID = Include.UndefinedCalendarDateRange;
//            fc.variable1_VariableValueTypeIndex = variableValueTypes.StoreDetail.dbIndex;
//            fc.variable1_TimeTypeIndex = variableTimeTypes.Any.dbIndex;

//            fc.operatorIndex = numericOperatorTypes.GreaterThan;
//            fc.valueTypeIndex = valueTypes.Numeric;
//            fc.numericTypeIndex = numericTypes.DoubleFreeForm;

//            fc.valueToCompareDouble = 500;
//        }
//    }
//    public class elementGroupStoreVariableToVariable : elementGroup
//    {
//        public elementGroupStoreVariableToVariable(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Variable to Variable";
//            base.groupSettings.valueSettings.loadFieldList = new LoadListDelegate(filterDataHelper.VariablesGetDataTable);
//            base.groupSettings.valueSettings.GetValueInfoTypeFromFieldIndex = new GetValueInfoTypeFromFieldIndexDelegate(filterDataHelper.VariablesGetValueInfoType);
//            base.groupSettings.valueSettings.fieldForData = "FIELD_INDEX";
//            base.groupSettings.valueSettings.fieldForDisplay = "FIELD_NAME";

//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));

//            elementBase b1 = new elementBase(manager, typeof(filterElementVariable), "1st Variable:");
//            b1.loadFromVariable1 = true;
//            b1.loadFromVariable2 = false;
//            base.elementList.Add(b1);

//            elementBase bOp = new elementBase(manager, typeof(filterElementOperatorNumericForVariables), "Operator");
//            base.elementList.Add(bOp);

//            elementBase b2 = new elementBase(manager, typeof(filterElementVariable), "2nd Variable:");
//            b2.loadFromVariable1 = false;
//            b2.loadFromVariable2 = true;
//            base.elementList.Add(b2);


//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatLogic(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += " " + filterUtility.BuildFormattedTextForVariable(options, fc, true, false);
//            formattedText += filterUtility.FormatOperator(options, " " + numericOperatorTypes.FromIndex(fc.operatorIndex).symbol);
//            formattedText += " " + filterUtility.BuildFormattedTextForVariable(options, fc, false, true);
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            fc.variable1_Index = filterDataHelper.VariablesGetIndexFromName("Sales Reg");
//            fc.variable1_VersionIndex = filterDataHelper.VersionsGetIndexFromName("Default to Plan");
//            fc.variable1_HN_RID = -1;
//            fc.variable1_CDR_RID = Include.UndefinedCalendarDateRange;
//            fc.variable1_VariableValueTypeIndex = variableValueTypes.StoreDetail.dbIndex;
//            fc.variable1_TimeTypeIndex = variableTimeTypes.Any.dbIndex;

//            fc.operatorIndex = numericOperatorTypes.LessThan;

//            fc.variable2_Index = filterDataHelper.VariablesGetIndexFromName("Stock Reg");
//            fc.variable2_VersionIndex = filterDataHelper.VersionsGetIndexFromName("Default to Plan");
//            fc.variable2_HN_RID = -1;
//            fc.variable2_CDR_RID = Include.UndefinedCalendarDateRange;
//            fc.variable2_VariableValueTypeIndex = variableValueTypes.StoreDetail.dbIndex;
//            fc.variable2_TimeTypeIndex = variableTimeTypes.Any.dbIndex;
//        }
//    }
//    public class elementGroupHeaderDate : elementGroup
//    {
//        public elementGroupHeaderDate(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Header Date";
//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));
//            elementBase eb = new elementBase(manager, typeof(filterElementOperatorDate), "Header Date:");
//            eb.isOperatorDate = true;
//            valueInfoTypes vInfo = new valueInfoTypes(valueTypes.Date);
//            vInfo.dateType = dateTypes.DateOnly;
//            eb.valueInfoType = vInfo;

//            base.elementList.Add(eb);
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatLogic(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += filterUtility.FormatNormal(options, " Header Date: ");
//            BuildFormattedTextForDate(ref formattedText, options, fc);


//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            fc.fieldIndex = 0;
//            fc.operatorIndex = dateOperatorTypes.Unrestricted;
//            fc.valueToCompareDateBetweenFromDays = -7;
//            fc.valueToCompareDateBetweenToDays = 0;
//        }
//    }
//    public class elementGroupHeaderReleaseDate : elementGroup
//    {
//        public elementGroupHeaderReleaseDate(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Release Date";
//            base.elementList.Add(new elementBase(manager, typeof(filterElementLogic), "And / Or"));
//            elementBase eb = new elementBase(manager, typeof(filterElementOperatorDate), "Rls Date:");
//            eb.isOperatorDate = true;
//            valueInfoTypes vInfo = new valueInfoTypes(valueTypes.Date);
//            vInfo.dateType = dateTypes.DateAndTime;
//            eb.valueInfoType = vInfo;
//            base.elementList.Add(eb);
//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            formattedText += filterUtility.FormatLogic(options, logicTypes.FromIndex(fc.logicIndex).Name);
//            formattedText += filterUtility.FormatNormal(options, " Release Date: ");
//            BuildFormattedTextForDate(ref formattedText, options, fc);


//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.logicIndex = logicTypes.And;
//            fc.fieldIndex = 0;
//            fc.operatorIndex = dateOperatorTypes.Unrestricted;
//            fc.valueToCompareDateBetweenFromDays = -7;
//            fc.valueToCompareDateBetweenToDays = 0;
//        }
//    }
//    public class elementGroupStoreSortBy : elementGroup
//    {
//        public elementGroupStoreSortBy(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Store Sort By";
//            //base.elementList.Add(new elementBase(manager, typeof(filterElementSortByDirection), "Direction"));
//            elementBase eb = new elementBase(manager, typeof(filterElementSortByType), "Sort By Type");
//            eb.isSortBy = true;
//            base.elementList.Add(eb);


//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            BuildFormattedTextForSortBy(ref formattedText, options, fc);
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.sortByTypeIndex = sortByTypes.StoreFields;
//            fc.sortByFieldIndex = storeFieldTypes.StoreName;
//            fc.operatorIndex = sortByDirectionTypes.Ascending;
//        }
//    }
//    public class elementGroupHeaderSortBy : elementGroup
//    {
//        public elementGroupHeaderSortBy(filterManager manager)
//        {
//            base.groupSettings.groupTitle = "Header Sort By";


//            //base.elementList.Add(new elementBase(manager, typeof(filterElementSortByDirection), "Direction"));
//            elementBase eb = new elementBase(manager, typeof(filterElementSortByType), "Sort By Type");
//            eb.isSortBy = true;
//            base.elementList.Add(eb);




//        }
//        public override void BuildFormattedText(optionDefinition options, ref filterCondition fc)
//        {
//            string formattedText = string.Empty;
//            BuildFormattedTextForSortBy(ref formattedText, options, fc);
//            fc.NodeFormattedText = formattedText;
//        }
//        public override void SetDefaults(ref filterCondition fc)
//        {
//            fc.sortByTypeIndex = sortByTypes.HeaderFields;
//            fc.sortByFieldIndex = headerFieldTypes.HeaderID;
//            fc.operatorIndex = sortByDirectionTypes.Ascending;
//        }
//    }
 
//}
