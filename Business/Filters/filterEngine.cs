using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;

namespace MIDRetail.Business
{
    public static class filterEngine
    {

        static StoreGroupProfile _p = null;  // TT#4596 - JSmith - General Method taking over 2 minutes to process.
        static Dictionary <int, StoreGroupProfile> _storeGroupCache = null; // TT#4596 - JSmith - General Method taking over 2 minutes to process.
        static ArrayList _lock = new ArrayList();  // TT#4596 - JSmith - General Method taking over 2 minutes to process.

        public static ProfileList RunFilter(filter f, ProfileList aProfileList)
        {
            //Begin TT#1588-MD -jsobek -Store Filters - error with <none> filter
            if (f.filterName == "<none>" || f.GetSortByConditionSeq() == -1)
            {
                return aProfileList;
            }
            //End TT#1588-MD -jsobek -Store Filters - error with <none> filter

            // Begin TT#4596 - JSmith - General Method taking over 2 minutes to process.
            lock (_lock.SyncRoot)
            {
                try
                {
                // End TT#4596 - JSmith - General Method taking over 2 minutes to process.
                    ProfileList returnList = new ProfileList(aProfileList.ProfileType);
                    //List<AllocationHeaderProfile> headerSortList = new List<AllocationHeaderProfile>();
                    //List<StoreProfile> storeSortList = new List<StoreProfile>();
                    //List<Profile> sortList = new List<Profile>();

                    _storeGroupCache = new Dictionary<int, StoreGroupProfile>();  // TT#4596 - JSmith - General Method taking over 2 minutes to process.

                    List<int> tempList = new List<int>();

                    ConditionNode cnSortByParent = f.FindConditionNode(f.GetSortByConditionSeq()); //parent seq for sort by
                    bool doSorting = false;



                    List<filterSortMapIndex> mapIndexes = new List<filterSortMapIndex>();





                    List<filterSortClass> sortClassList = new List<filterSortClass>();
                    if (cnSortByParent.ConditionNodes.Count > 0)
                    {
                        int stringOffset = 1;
                        int dateOffset = 6;
                        int intOffset = 11;
                        int doubleOffset = 16;

                        int stringUsed = 0;
                        int dateUsed = 0;
                        int intUsed = 0;
                        int doubleUsed = 0;
                        foreach (ConditionNode cnSort in cnSortByParent.ConditionNodes)
                        {
                            filterSortMapIndex map1 = new filterSortMapIndex();

                            filterSortByDirectionTypes sortByDirection = filterSortByDirectionTypes.FromIndex(cnSort.condition.operatorIndex);
                            if (sortByDirection == filterSortByDirectionTypes.Ascending)
                            {
                                map1.ascending = true;
                            }
                            else
                            {
                                map1.ascending = false;
                            }

                            filterSortByTypes sortByType = filterSortByTypes.FromIndex(cnSort.condition.sortByTypeIndex);
                            int fieldIndex = cnSort.condition.sortByFieldIndex;
                            filterDataTypes dataType;
                            if (sortByType == filterSortByTypes.StoreCharacteristics)
                            {
                                dataType = filterDataHelper.StoreCharacteristicsGetDataType(fieldIndex);
                            }
                            //else if (sortByType == filterSortByTypes.HeaderCharacteristics)
                            //{
                            //    dataType = filterDataHelper.HeaderCharacteristicsGetDataType(fieldIndex);
                            //}
                            else if (sortByType == filterSortByTypes.StoreFields)
                            {
                                dataType = filterStoreFieldTypes.GetValueTypeInfoForField(fieldIndex);
                            }
                            //else if (sortByType == filterSortByTypes.HeaderFields)
                            //{
                            //    dataType = filterHeaderFieldTypes.GetValueTypeForField(fieldIndex);
                            //}
                            else if (sortByType == filterSortByTypes.StoreStatus)
                            {
                                dataType = new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.Integer);
                            }
                            else if (sortByType == filterSortByTypes.Variables)
                            {
                                //dataType = filterDataHelper.VariablesGetDataType(fieldIndex);
                                dataType = new filterDataTypes(filterValueTypes.Numeric, filterNumericTypes.DoubleFreeForm);
                            }
                            else
                            {
                                dataType = new filterDataTypes(filterValueTypes.Text);
                            }

                            filterValueTypes valueType = dataType.valueType;
                            filterNumericTypes numericType = dataType.numericType;

                            if (valueType == filterValueTypes.Text)
                            {
                                map1.mapIndex = stringOffset + stringUsed;
                                stringUsed++;
                            }
                            else if (valueType == filterValueTypes.Date)
                            {
                                map1.mapIndex = dateOffset + dateUsed;
                                dateUsed++;
                            }
                            else if (valueType == filterValueTypes.Numeric)
                            {
                                if (numericType == filterNumericTypes.Integer)
                                {
                                    map1.mapIndex = intOffset + intUsed;
                                    intUsed++;
                                }
                                else
                                {
                                    map1.mapIndex = doubleOffset + doubleUsed;
                                    doubleUsed++;
                                }
                            }

                            mapIndexes.Add(map1);
                        }





                        doSorting = true;
                    }

                    //Step 1 - Apply filter conditions to each profile
                    foreach (Profile prof in aProfileList)
                    {



                        bool finalResult = false;


                        //assuming and-also (&&) and or-else (||) for logic blocks

                        //if (lb != null)
                        //{
                        //    lb.Items.Clear();
                        //}

                        executionOrder = 0;

                        //get initial siblings
                        ConditionNode cn = f.FindConditionNode(f.GetRootConditionSeq()); //parent seq for conditions

                        ClearPriorResults(cn);

                        List<ConditionNode> childrenList = new List<ConditionNode>();

                        foreach (ConditionNode child in cn.ConditionNodes)
                        {
                            child.condition.executeSkipChildren = false;
                            childrenList.Add(child);
                        }
                        finalResult = ProcessCondition(f, childrenList, prof);

                        if (finalResult == true)
                        {
                            tempList.Add(prof.Key);
                            //if (doSorting)
                            //{
                            //    //add things to a list so we can sort
                            //    if (f.filterType == filterTypes.HeaderFilter)
                            //    {
                            //        headerSortList.Add((AllocationHeaderProfile)prof);
                            //    }
                            //    else if (f.filterType == filterTypes.StoreFilter)
                            //    {
                            //        storeSortList.Add((StoreProfile)prof);
                            //    }
                            //    else
                            //    {
                            //        sortList.Add(prof);
                            //    }
                            //}
                            //else
                            //{
                            //    if (f.isLimited)
                            //    {
                            //        //just store key
                            //        tempList.Add(prof.Key);
                            //    }
                            //    else
                            //    {
                            //        //copy profile to return list
                            //        returnList.Add(prof);
                            //    }
                            //}

                        }
                    }

                    //Step 2 - Sort the list based on sort by conditions and limit the results if necessary
                    if (doSorting)
                    {
                        //Get the values from the profile to the filterSortClass  (for just the remaining profiles in the temp list)
                        foreach (int profKey in tempList)
                        {
                            Profile prof = aProfileList.FindKey(profKey);


                            filterSortClass sort1 = new filterSortClass();
                            sort1.profileKey = prof.Key;

                            int mapCount = 0;
                            foreach (ConditionNode cnSort in cnSortByParent.ConditionNodes)
                            {

                                filterSortByTypes sortByType = filterSortByTypes.FromIndex(cnSort.condition.sortByTypeIndex);
                                if (sortByType == filterSortByTypes.StoreCharacteristics)
                                {
                                    filterDataTypes dataType = filterDataHelper.StoreCharacteristicsGetDataType(cnSort.condition.sortByFieldIndex);
                                    int scg_rid = cnSort.condition.sortByFieldIndex;
                                    f.GetStoreCharacteristicValues(scg_rid);


                                    object objValue = f.GetCharacteristicValueForThisStore(scg_rid, prof.Key);

                                    filterValueTypes valueType = dataType.valueType;
                                    // StoreCharGroupProfile scgp = (StoreCharGroupProfile)store.Characteristics[scg_rid];

                                    //foreach (StoreCharGroupProfile scgp in store.Characteristics)
                                    //{
                                    if (objValue != null)
                                    {
                                        //string colName = scgp.Name;
                                        //object aValue = scgp.CharacteristicValue.CharValue;


                                        if (valueType == filterValueTypes.Date)
                                        {
                                            DateTime val1 = Convert.ToDateTime(objValue, CultureInfo.CurrentUICulture);
                                            sort1.SetValue(mapIndexes[mapCount].mapIndex, val1);
                                        }
                                        else if (valueType == filterValueTypes.Dollar)
                                        {
                                            double val1 = Convert.ToDouble(objValue, CultureInfo.CurrentUICulture);
                                            sort1.SetValue(mapIndexes[mapCount].mapIndex, val1);
                                        }
                                        else if (valueType == filterValueTypes.Numeric)
                                        {
                                            int val1 = Convert.ToInt32(objValue, CultureInfo.CurrentUICulture);
                                            sort1.SetValue(mapIndexes[mapCount].mapIndex, val1);
                                        }
                                        else if (valueType == filterValueTypes.Text)
                                        {
                                            string val1 = Convert.ToString(objValue, CultureInfo.CurrentUICulture);
                                            sort1.SetValue(mapIndexes[mapCount].mapIndex, val1);
                                        }
                                        else if (valueType == filterValueTypes.List)
                                        {
                                            ////does this store profile SC_RID match any of the selected sc rids.
                                            //int store_sc_rid = f.GetCharacteristicValueRidForThisStore(scg_rid, store.Key);

                                            //DataRow[] listValues = fc.GetListValues(listValueTypes.StoreCharacteristicRID);
                                            //bool b = listValues.Any(x => (int)x["LIST_VALUE_INDEX"] == store_sc_rid);
                                            //return b;
                                        }


                                    }

                                }
                                else if (sortByType == filterSortByTypes.StoreFields)
                                {
                                    filterStoreFieldTypes fieldType = filterStoreFieldTypes.FromIndex(cnSort.condition.sortByFieldIndex);
                                    filterStoreEnhancedProfile ep = new filterStoreEnhancedProfile();
                                    ep.prof = (StoreProfile)prof;
                                    sort1.SetValue(mapIndexes[mapCount].mapIndex, fieldType.GetStoreValue(ep));
                                }
                                else if (sortByType == filterSortByTypes.StoreStatus)
                                {
                                    int val1 = (int)((StoreProfile)prof).Status;
                                    sort1.SetValue(mapIndexes[mapCount].mapIndex, val1);
                                }

                                else if (sortByType == filterSortByTypes.Variables)
                                {
                                    PlanOpenParms currentPlanOpenParms = null;
                                    if (f.currentPlanCubeGroup != null)
                                    {
                                        currentPlanOpenParms = f.currentPlanCubeGroup.OpenParms;
                                    }


                                    DateRangeProfile varDateRangeProfile;
                                    HierarchyNodeProfile nodeProfile;
                                    VersionProfile versionProfile;
                                    PlanCubeGroup localPlanCubeGroup = SetCubeInfoForSort(f, cnSort.condition, currentPlanOpenParms, out varDateRangeProfile, out nodeProfile, out versionProfile);

                                    //IDictionaryEnumerator _enumerator = f.filterCubeGroupHash.GetEnumerator();
                                    //while (_enumerator.MoveNext())
                                    //{
                                    //    ((CubeGroupHashEntry)_enumerator.Value).FilterCubeGroup.OpenCubeGroup(((CubeGroupHashEntry)_enumerator.Value).FilterOpenParms);
                                    //}

                                    ProfileList dateProfileList;

                                    if (currentPlanOpenParms == null)
                                    {
                                        currentPlanOpenParms = localPlanCubeGroup.OpenParms;
                                    }
                                    if (currentPlanOpenParms.DateRangeProfile.SelectedDateType == eCalendarDateType.Week)
                                    {
                                        dateProfileList = f.cubeSAB.ApplicationServerSession.Calendar.GetWeekRange(currentPlanOpenParms.DateRangeProfile, currentPlanOpenParms.DateRangeProfile.InternalAnchorDate);
                                    }
                                    else
                                    {
                                        dateProfileList = f.cubeSAB.ApplicationServerSession.Calendar.GetPeriodRange(currentPlanOpenParms.DateRangeProfile, currentPlanOpenParms.DateRangeProfile.InternalAnchorDate);
                                    }

                                    double dTotal = 0;
                                    StoreProfile store = (StoreProfile)prof;
                                    //Begin TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
                                    //bool first = true;
                                    //foreach (DateProfile dateProf in dateProfileList)
                                    //{
                                    //    if (first)
                                    //    {
                                    //        first = false;
                                    //        dTotal += ProcessVariableSorting(localPlanCubeGroup, f, cnSort.condition, varDateRangeProfile, nodeProfile, versionProfile, store, currentPlanOpenParms, dateProf);
                                    //    }
                                    //}
                                    // Begin TT#4864 - JSmith - Basis Size Method using a Filter does not return the correct stores.
                                    //dTotal = ProcessVariableSorting(localPlanCubeGroup, f, cnSort.condition, varDateRangeProfile, nodeProfile, versionProfile, store, currentPlanOpenParms);
                                    // Use cube group from conditions if explicitly set.
                                    if (f.filterCubeGroupHash.Count > 0)
                                    {
                                        IDictionaryEnumerator _enumerator = f.filterCubeGroupHash.GetEnumerator();
                                        while (_enumerator.MoveNext())
                                        {
                                            CubeGroupHashEntry cubeGroupHashEntry = (CubeGroupHashEntry)_enumerator.Value;
                                            varDateRangeProfile = cubeGroupHashEntry.FilterOpenParms.DateRangeProfile;
                                            nodeProfile = cubeGroupHashEntry.FilterOpenParms.StoreHLPlanProfile.NodeProfile;
                                            versionProfile = cubeGroupHashEntry.FilterOpenParms.StoreHLPlanProfile.VersionProfile;
                                            localPlanCubeGroup = cubeGroupHashEntry.FilterCubeGroup;
                                            dTotal += ProcessVariableSorting(localPlanCubeGroup, f, cnSort.condition, varDateRangeProfile, nodeProfile, versionProfile, store, currentPlanOpenParms);
                                        }
                                    }
                                    else
                                    {
                                        dTotal = ProcessVariableSorting(localPlanCubeGroup, f, cnSort.condition, varDateRangeProfile, nodeProfile, versionProfile, store, currentPlanOpenParms);
                                    }
                                    // End TT#4864 - JSmith - Basis Size Method using a Filter does not return the correct stores.

                                    //End TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.

                                    sort1.SetValue(mapIndexes[mapCount].mapIndex, dTotal);
                                    //filterDataTypes dataType = filterDataHelper.VariablesGetDataType(cnSort.condition.sortByFieldIndex);

                                    //if (dataType.valueType == filterValueTypes.Numeric)
                                    //{
                                    //    if (dataType.numericType == filterNumericTypes.Integer)
                                    //    {
                                    //        int iTotal = Convert.ToInt32(dTotal);
                                    //        sort1.SetValue(mapIndexes[mapCount].mapIndex, iTotal);
                                    //    }
                                    //    else
                                    //    {
                                    //        sort1.SetValue(mapIndexes[mapCount].mapIndex, dTotal);
                                    //    }
                                    //}
                                }
                                else
                                {

                                }



                            }
                            sortClassList.Add(sort1);
                            mapCount++;
                        }



                        IOrderedEnumerable<filterSortClass> q = SortProfileList(mapIndexes, sortClassList);
                        if (q != null)
                        {
                            int limit = Convert.ToInt32(q.Count());
                            if (f.isLimited)
                            {
                                limit = Convert.ToInt32(f.resultLimit);
                            }

                            int i = 0;
                            while (i < limit && i < q.Count())
                            {
                                filterSortClass fsc = q.ElementAt(i);
                                int key = fsc.profileKey;
                                returnList.Add(aProfileList.FindKey(key));
                                i++;
                            }
                        }

                    }
                    else
                    {
                        int limit = Convert.ToInt32(tempList.Count());
                        if (f.isLimited)
                        {
                            limit = Convert.ToInt32(f.resultLimit);
                        }

                        int i = 0;
                        while (i < limit && i < tempList.Count)
                        {
                            int key = tempList[i];
                            returnList.Add(aProfileList.FindKey(key));
                            i++;
                        }
                    }




                    return returnList;
                // Begin TT#4596 - JSmith - General Method taking over 2 minutes to process.
                }
                finally
                {
                    _p = null;
                    _storeGroupCache = null;
                }
            }
            // End TT#4596 - JSmith - General Method taking over 2 minutes to process.
        }




        private static void ClearPriorResults(ConditionNode cn)
        {
            cn.condition.executed = string.Empty;
            cn.condition.executeGroup = string.Empty;
            cn.condition.executeResult = string.Empty;
            cn.condition.executeResultForGroup = string.Empty;
            foreach (ConditionNode c in cn.ConditionNodes)
            {
                ClearPriorResults(c);
            }
        }

        //private static void ExecuteConditionChildren(filter f, ConditionNode cn, ref bool finalResult)
        //{
        //    //compare siblings cost to run
        //    if (cn.ConditionNodes.Count == 1)
        //    {
        //        finalResult = ExecuteCondition(f, cn.condition);
        //    }
        //    else if (cn.ConditionNodes.Count > 1)
        //    {
        //        List<costClass> costList = new List<costClass>();
        //        foreach (ConditionNode n in cn.ConditionNodes)
        //        {
        //            costClass cc = new costClass();
        //            cc.seq = n.condition.Seq;
        //            cc.cost = f.GetCost(n.condition.Seq);
        //            if (n.condition.logicIndex == logicTypes.Or.Index)  //??
        //            {
        //                cc.isOR = true;
        //            }
        //            else
        //            {
        //                cc.isOR = false;
        //            }
        //            costList.Add(cc);
        //        }
        //        //IEnumerable<costClass> query = costList.OrderBy(costClass => costClass.cost);


        //        //make groups based on logical operators AND / OR
        //        List<costGroup> groupList = new List<costGroup>();
        //        costGroup currentGroup = new costGroup();
        //        //foreach (costClass c in query)
        //        foreach (costClass c in costList)
        //        {
        //            if (c.isOR)
        //            {
        //                //make new group
                      
        //                //if (currentGroup.group.Count > 0) //special check for first condition
        //                //{
        //                    int gc2 = 0;
        //                    foreach (costClass cc in currentGroup.group)
        //                    {
        //                        gc2 += cc.cost;
        //                    }
        //                    currentGroup.groupCost = gc2;
        //                    groupList.Add(currentGroup);
        //                //}
        //                    currentGroup = new costGroup();
        //            }

        //            currentGroup.group.Add(c);
        //        }
        //        int gc = 0;
        //        foreach (costClass cc in currentGroup.group)
        //        {
        //            gc += cc.cost;
        //        }
        //        currentGroup.groupCost = gc;
        //        groupList.Add(currentGroup);

        //        //costList.Sort();
        //        IEnumerable<costGroup> queryGroup = groupList.OrderBy(costGroup => costGroup.groupCost);



        //        //write groups to conditions for debugging;
        //        foreach (costGroup cg in queryGroup)
        //        {
        //            foreach (costClass cst in cg.group)
        //            {
        //                ConditionNode c = f.FindConditionNode(cst.seq);
        //                c.condition.executeGroup = groupList.IndexOf(cg).ToString();
        //            }
        //        }



        //        bool result = true;
        //        foreach (costGroup cg in queryGroup)
        //        {
        //            result = true;
        //            foreach (costClass cst in cg.group)
        //            {
        //                ConditionNode c = f.FindConditionNode(cst.seq);
        //                result = ExecuteCondition(f, c.condition);
        //                if (result == false)
        //                {
        //                    break;
        //                }
        //                //now execute the children of this condition
        //                ExecuteConditionChildren(f, c, ref finalResult);

        //            }
        //            if (result == true)
        //            {
        //                break;
        //            }
        //        }
        //        finalResult = result;
        //    }

        //}

        private static bool  ProcessCondition(filter f, List<ConditionNode> conditionList, Profile prof)
        {
            if (conditionList.Count == 1)
            {
                return ExecuteCondition(f, conditionList[0].condition, prof);
            }
            else
            {

                List<costClass> costList = new List<costClass>();
                foreach (ConditionNode n in conditionList)
                {
                    costClass cc = new costClass();
                    cc.seq = n.condition.Seq;
                    cc.cost = f.GetCost(n.condition.Seq);
                    if (n.condition.logicIndex == filterLogicTypes.Or.Index)
                    {
                        cc.isOR = true;
                    }
                    else
                    {
                        cc.isOR = false;
                    }
                    costList.Add(cc);
                }


                //make groups based on logical operators AND / OR
                List<costGroup> groupList = new List<costGroup>();
                costGroup currentGroup = new costGroup();
                bool firstOne = true;
                foreach (costClass c in costList)
                {
                    if (c.isOR && firstOne == false)
                    {
                        //make new group
                        groupList.Add(currentGroup);
                        currentGroup = new costGroup();
                    }

                    currentGroup.group.Add(c);
                    firstOne = false;
                }

                groupList.Add(currentGroup);


                //Calculate the costs for each group
                foreach (costGroup cg in groupList)
                {
                    int gc = 0;
                    foreach (costClass cc in cg.group)
                    {
                        gc += cc.cost;
                    }
                    cg.groupCost = gc;
                }


            


                //Sort the groups based on group cost
                IEnumerable<costGroup> queryGroup = groupList.OrderBy(costGroup => costGroup.groupCost);


                //write groups to conditions for debugging purposes
                foreach (costGroup cg in queryGroup)
                {
                    foreach (costClass cst in cg.group)
                    {
                        ConditionNode c = f.FindConditionNode(cst.seq);
                        c.condition.executeGroup += "-" + groupList.IndexOf(cg).ToString();
                    }
                }




                bool result = true;
                foreach (costGroup cg in queryGroup)
                {
                    result = true;
                    foreach (costClass cst in cg.group)
                    {
                        ConditionNode c = f.FindConditionNode(cst.seq);


                        if (c.condition.executeSkipChildren == true || c.ConditionNodes.Count == 0)
                        {
                            result = ExecuteCondition(f, c.condition, prof);
                        }
                        else
                        {
                            List<ConditionNode> mePlusChildrenList = new List<ConditionNode>();
                            c.condition.executeSkipChildren = true;
                            mePlusChildrenList.Add(c);

                            foreach (ConditionNode cn in c.ConditionNodes)
                            {
                                cn.condition.executeGroup += "-" + groupList.IndexOf(cg).ToString();
                                cn.condition.executeSkipChildren = false;
                                mePlusChildrenList.Add(cn);
                            }
                            result = ProcessCondition(f, mePlusChildrenList, prof);
                        }


                        if (result == false)
                        {
                            c.condition.executeResultForGroup = "F";
                            break;
                        }
                        

                    }
                 
                    if (result == true)
                    {
                        foreach (costClass cst in cg.group)
                        {
                            ConditionNode c = f.FindConditionNode(cst.seq);
                            c.condition.executeResultForGroup = "T";
                        }
                  
                        break;
                    }
                }
                return result;

            }



        }



  
        private static int executionOrder = 0;
        private static bool ExecuteCondition(filter f, filterCondition fc, Profile profileToCompare)
        {
           

            fc.executed = executionOrder.ToString();
            executionOrder++;

            if (f.filterType == filterTypes.HeaderFilter || f.filterType == filterTypes.AssortmentFilter)
            {
                //AllocationHeaderProfile prof = (AllocationHeaderProfile)profileToCompare;

                //filterDictionary et = filterDictionary.FromIndex(fc.dictionaryIndex);
                //if (et == filterDictionary.HeaderTypes)
                //{
                //    DataRow[] drHeaderTypes = fc.GetListValues(filterListValueTypes.HeaderTypes);
                //    int curHeaderType = (int)prof.HeaderType;
                //    bool b = drHeaderTypes.Any(x => (int)x["LIST_VALUE_INDEX"] == curHeaderType);  //TO DO - Test this

                //    return b;
                //}
                //else
                //{
                    return false;
                //}
            }
            else if (f.filterType == filterTypes.StoreFilter)
            {
                StoreProfile store = (StoreProfile)profileToCompare;
                filterStoreEnhancedProfile eProf = new filterStoreEnhancedProfile();
                eProf.prof = store;

                filterDictionary et = filterDictionary.FromIndex(fc.dictionaryIndex);
                if (et == filterDictionary.StoreFields)
                {          
                    filterStoreFieldTypes fieldType = filterStoreFieldTypes.FromIndex(fc.fieldIndex);                
                    return fieldType.CompareStoreValue(eProf, fc);
                }
                else if (et == filterDictionary.StoreCharacteristics)
                {
                    int scg_rid = fc.fieldIndex;
                    f.GetStoreCharacteristicValues(scg_rid);

                
                    object objValue = f.GetCharacteristicValueForThisStore(scg_rid, store.Key);

                    filterValueTypes valueType = filterValueTypes.FromIndex(fc.valueTypeIndex);
                       // StoreCharGroupProfile scgp = (StoreCharGroupProfile)store.Characteristics[scg_rid];

                        //foreach (StoreCharGroupProfile scgp in store.Characteristics)
                        //{
                    if (objValue != null)
                    {
                            //string colName = scgp.Name;
                            //object aValue = scgp.CharacteristicValue.CharValue;


                            if (valueType == filterValueTypes.Date)
                                {
                                    DateTime val1 = Convert.ToDateTime(objValue, CultureInfo.CurrentUICulture);
                                    return filterDataHelper.CompareToDate(val1, fc);
                                }
                            else if (valueType == filterValueTypes.Dollar)
                                {
                                    double val1 = Convert.ToDouble(objValue, CultureInfo.CurrentUICulture);
                                    return filterDataHelper.CompareToDouble(val1, fc);
                                }
                            else if (valueType == filterValueTypes.Numeric)
                                {
                                    int val1 = Convert.ToInt32(objValue, CultureInfo.CurrentUICulture);
                                    return filterDataHelper.CompareToInt(val1, fc);
                                }
                            else if (valueType == filterValueTypes.Text)
                                {
                                    string val1 = Convert.ToString(objValue, CultureInfo.CurrentUICulture);
                                    return filterDataHelper.CompareToString(val1, fc);
                                }
                            else if (valueType == filterValueTypes.List)
                                {
                                    //does this store profile SC_RID match any of the selected sc rids.
                                    int store_sc_rid = f.GetCharacteristicValueRidForThisStore(scg_rid, store.Key);

                                    DataRow[] listValues = fc.GetListValues(filterListValueTypes.StoreCharacteristicRID);

                                    // Begin TT#5029 - JSmith - Issue with header filter when using 'excludes' in the characteristic field
                                    //bool b = listValues.Any(x => (int)x["LIST_VALUE_INDEX"] == store_sc_rid);
                                    bool b;
                                    if (fc.operatorIndex == 1) // Not In
                                    {
                                        b = !listValues.Any(x => (int)x["LIST_VALUE_INDEX"] == store_sc_rid);
                                    }
                                    else
                                    {
                                        b = listValues.Any(x => (int)x["LIST_VALUE_INDEX"] == store_sc_rid);
                                    }
                                    // End TT#5029 - JSmith - Issue with header filter when using 'excludes' in the characteristic field
                                    return b;
                                }
                            else
                            {
                                return false;
                            }
                                
                    }
                    // Begin TT#5029 - JSmith - Issue with header filter when using 'excludes' in the characteristic field
                    // Field not set using list and Not In
                    else if (valueType == filterValueTypes.List &&
                        fc.operatorIndex == 1)
                    {
                        return true;
                    }
                    // End TT#5029 - JSmith - Issue with header filter when using 'excludes' in the characteristic field
                    else
                    {
                        return false;
                    }
                   
                }
                else if (et == filterDictionary.StoreAttributeSet)
                {
                    // Begin TT#4596 - JSmith - General Method taking over 2 minutes to process.
                    //StoreGroupProfile p = f.cubeSAB.StoreServerSession.GetStoreGroup(fc.fieldIndex);
                    if (_p == null ||
                        _p.Key != fc.fieldIndex)
                    {
                        if (!_storeGroupCache.TryGetValue(fc.fieldIndex, out _p))
                        {
                            _p = StoreMgmt.StoreGroup_GetFilled(fc.fieldIndex);
                            _storeGroupCache.Add(_p.Key, _p);
                        }
                    }
                    // End TT#4596 - JSmith - General Method taking over 2 minutes to process.
                
                    //Profile a = a1.FindKey(key);
                    // Begin TT#4596 - JSmith - General Method taking over 2 minutes to process.
                    //if (p != null)  
                    if (_p != null)
                    // End TT#4596 - JSmith - General Method taking over 2 minutes to process.
                    {
                        filterListOperatorTypes listOp = filterListOperatorTypes.FromIndex(fc.operatorIndex);
                        bool b = false;

                        DataRow[] drGroups = fc.GetListValues(filterListValueTypes.StoreGroupLevel);  //a list of all the groups they selected

                        // Begin TT#4596 - JSmith - General Method taking over 2 minutes to process.
                        //foreach (StoreGroupLevelProfile v in p.GroupLevels) //p.GroupLevels is a list of all current possible groups for this attribute set
                        foreach (StoreGroupLevelProfile v in _p.GroupLevels) //p.GroupLevels is a list of all current possible groups for this attribute set
                        // End TT#4596 - JSmith - General Method taking over 2 minutes to process.
                        {
                            bool searchThisGroup;
                            if (fc.listConstantType == filterListConstantTypes.All)
                            {
                                searchThisGroup = true;
                            }
                            else if (fc.listConstantType == filterListConstantTypes.None)
                            {
                                searchThisGroup = false;
                            }
                            else
                            {
                                searchThisGroup = drGroups.Any(x => (int)x["LIST_VALUE_INDEX"] == v.Key);
                            }

                            if (searchThisGroup)
                            {
                                Profile a = v.Stores.FindKey(store.Key);
                                if (a != null)
                                {
                                    //we found a group that contains our store
                                    b = true;
                                    break;
                                }
                               
                            }
                            
                        }
                        if (listOp == filterListOperatorTypes.Excludes)
                        {
                            b = !b;
                        }
                        return b;
                    }
                    else
                    {
                        return false;
                    }

                    

                }
                else if (et == filterDictionary.StoreList)
                {                   
                    int storeRID = eProf.prof.Key;
                    return filterDataHelper.CompareInList(filterListValueTypes.StoreRID, storeRID, fc);
                }
                else if (et == filterDictionary.StoreStatus)
                {
                    int storeStatus = (int)eProf.prof.Status;
                    //Begin TT#4466 -jsobek -Store Filter -Store Status
                    //return filterDataHelper.CompareInList(filterListValueTypes.StoreStatus, storeStatus, fc);
                    
                    filterStoreStatusTypes filterStatus = filterStoreStatusTypes.FromTextCode(storeStatus);
                    return filterDataHelper.CompareInList(filterListValueTypes.StoreStatus, filterStatus.dbIndex, fc);
                    //End TT#4466 -jsobek -Store Filter -Store Status
                }
                else if (et == filterDictionary.StoreVariableToConstant)
                {
                    PlanOpenParms currentPlanOpenParms = null;
                    if (f.currentPlanCubeGroup != null)
                    {
                        currentPlanOpenParms = f.currentPlanCubeGroup.OpenParms;
                    }


                    DateRangeProfile varDateRangeProfile;
                    HierarchyNodeProfile nodeProfile;
                    VersionProfile versionProfile;
                    PlanCubeGroup localPlanCubeGroup = SetCubeInfo(true, f, fc, currentPlanOpenParms, out varDateRangeProfile, out nodeProfile, out versionProfile);
                   
                    //IDictionaryEnumerator _enumerator = f.filterCubeGroupHash.GetEnumerator();
                    //while (_enumerator.MoveNext())
                    //{
                    //    ((CubeGroupHashEntry)_enumerator.Value).FilterCubeGroup.OpenCubeGroup(((CubeGroupHashEntry)_enumerator.Value).FilterOpenParms);
                    //}




                    //Begin TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
                    //ProfileList dateProfileList;

                    //if (currentPlanOpenParms == null)
                    //{
                    //    currentPlanOpenParms = localPlanCubeGroup.OpenParms;
                    //}
                    //if (currentPlanOpenParms.DateRangeProfile.SelectedDateType == eCalendarDateType.Week)
                    //{
                    //    dateProfileList = f.cubeSAB.ApplicationServerSession.Calendar.GetWeekRange(currentPlanOpenParms.DateRangeProfile, currentPlanOpenParms.DateRangeProfile.InternalAnchorDate);
                    //}
                    //else
                    //{
                    //    dateProfileList = f.cubeSAB.ApplicationServerSession.Calendar.GetPeriodRange(currentPlanOpenParms.DateRangeProfile, currentPlanOpenParms.DateRangeProfile.InternalAnchorDate);
                    //}

                 
                    //foreach (DateProfile dateProf in dateProfileList)
                    //{
                    //    FilterMonitor.FilterMonitorEntry fme = null;
                    //    if (FilterMonitor.doMonitor)
                    //    {
                    //        fme = new FilterMonitor.FilterMonitorEntry();
                    //        fme.filterName = f.filterName;
                    //        filterManager manager = new filterManager(f, null, null, null);
                    //        manager.Options.ColorFormatBlackBlueRed = false;
                    //        manager.Options.ColorFormatBlackRed = false;
                    //        manager.Options.ColorFormatGreenBlueRed = false;
                    //        f.RebuildText(manager, f.FindConditionNode(fc.Seq));
                    //        fme.conditionText = filterUtility.UnEscapeStringToFormat(f.FindConditionNode(fc.Seq).condition.NodeFormattedText);
                    //        fme.storeID = store.StoreId;
                    //        fme.outerWeek = dateProf.Date.ToShortDateString();
                    //    }
                    //    bool b = ProcessVariableToConstant(localPlanCubeGroup, f, fc, varDateRangeProfile, nodeProfile, versionProfile, store, currentPlanOpenParms, dateProf, fme);
                       
                    //    if (b == true)
                    //    {
                    //        return b;
                    //    }
                    //}
                    //return false;

                    FilterMonitor.FilterMonitorEntry fme = null;
                    if (FilterMonitor.doMonitor)
                    {
                        fme = new FilterMonitor.FilterMonitorEntry();
                        fme.filterName = f.filterName;
                        filterManager manager = new filterManager(f, null, null, null);
                        manager.Options.ColorFormatBlackBlueRed = false;
                        manager.Options.ColorFormatBlackRed = false;
                        manager.Options.ColorFormatGreenBlueRed = false;
                        f.RebuildText(manager, f.FindConditionNode(fc.Seq));
                        fme.conditionText = filterUtility.UnEscapeStringToFormat(f.FindConditionNode(fc.Seq).condition.NodeFormattedText);
                        fme.storeID = store.StoreId;
                        //fme.outerWeek = dateProf.Date.ToShortDateString();
                    }
                    bool b = ProcessVariableToConstant(localPlanCubeGroup, f, fc, varDateRangeProfile, nodeProfile, versionProfile, store, currentPlanOpenParms, fme);
                    return b;
                    //End TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
                }
                else if (et == filterDictionary.StoreVariableToVariable)
                {
                    PlanOpenParms currentPlanOpenParms = null;
                    if (f.currentPlanCubeGroup != null)
                    {
                        currentPlanOpenParms = f.currentPlanCubeGroup.OpenParms;
                    }


                    DateRangeProfile varDateRangeProfile;
                    HierarchyNodeProfile nodeProfile;
                    VersionProfile versionProfile;
                    PlanCubeGroup localPlanCubeGroup = SetCubeInfo(true, f, fc, currentPlanOpenParms, out varDateRangeProfile, out nodeProfile, out versionProfile);

                    DateRangeProfile var2_DateRangeProfile;
                    HierarchyNodeProfile var2_nodeProfile;
                    VersionProfile var2_versionProfile;
                    PlanCubeGroup var2_localPlanCubeGroup = SetCubeInfo(false, f, fc, currentPlanOpenParms, out var2_DateRangeProfile, out var2_nodeProfile, out var2_versionProfile);
                    //IDictionaryEnumerator _enumerator = f.filterCubeGroupHash.GetEnumerator();
                    //while (_enumerator.MoveNext())
                    //{
                    //    ((CubeGroupHashEntry)_enumerator.Value).FilterCubeGroup.OpenCubeGroup(((CubeGroupHashEntry)_enumerator.Value).FilterOpenParms);
                    //}

                    //Begin TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
                    //ProfileList dateProfileList;

                    //if (currentPlanOpenParms == null)
                    //{
                    //    currentPlanOpenParms = localPlanCubeGroup.OpenParms;
                    //}
                    //if (currentPlanOpenParms.DateRangeProfile.SelectedDateType == eCalendarDateType.Week)
                    //{
                    //    dateProfileList = f.cubeSAB.ApplicationServerSession.Calendar.GetWeekRange(currentPlanOpenParms.DateRangeProfile, currentPlanOpenParms.DateRangeProfile.InternalAnchorDate);
                    //}
                    //else
                    //{
                    //    dateProfileList = f.cubeSAB.ApplicationServerSession.Calendar.GetPeriodRange(currentPlanOpenParms.DateRangeProfile, currentPlanOpenParms.DateRangeProfile.InternalAnchorDate);
                    //}


                    //foreach (DateProfile dateProf in dateProfileList)
                    //{
                    //    FilterMonitor.FilterMonitorEntry fme = null;
                    //    if (FilterMonitor.doMonitor)
                    //    {
                    //        fme = new FilterMonitor.FilterMonitorEntry();
                    //        fme.filterName = f.filterName;
                    //        filterManager manager = new filterManager(f, null, null, null);
                    //        manager.Options.ColorFormatBlackBlueRed = false;
                    //        manager.Options.ColorFormatBlackRed = false;
                    //        manager.Options.ColorFormatGreenBlueRed = false;
                    //        f.RebuildText(manager, f.FindConditionNode(fc.Seq));
                    //        fme.conditionText = filterUtility.UnEscapeStringToFormat(f.FindConditionNode(fc.Seq).condition.NodeFormattedText);
                    //        fme.storeID = store.StoreId;
                    //        fme.outerWeek = dateProf.Date.ToShortDateString();
                    //    }
                    //    bool b = ProcessVariableToVariable(f, fc, store, currentPlanOpenParms, dateProf, localPlanCubeGroup, varDateRangeProfile, nodeProfile, versionProfile, var2_localPlanCubeGroup, var2_DateRangeProfile, var2_nodeProfile, var2_versionProfile);
                    //    if (b == true)
                    //    {
                    //        return b;
                    //    }
                    //}
                    //return false;

                    FilterMonitor.FilterMonitorEntry fme = null;
                    if (FilterMonitor.doMonitor)
                    {
                        fme = new FilterMonitor.FilterMonitorEntry();
                        fme.filterName = f.filterName;
                        filterManager manager = new filterManager(f, null, null, null);
                        manager.Options.ColorFormatBlackBlueRed = false;
                        manager.Options.ColorFormatBlackRed = false;
                        manager.Options.ColorFormatGreenBlueRed = false;
                        f.RebuildText(manager, f.FindConditionNode(fc.Seq));
                        fme.conditionText = filterUtility.UnEscapeStringToFormat(f.FindConditionNode(fc.Seq).condition.NodeFormattedText);
                        fme.storeID = store.StoreId;
                        //fme.outerWeek = dateProf.Date.ToShortDateString();
                    }
                    bool b = ProcessVariableToVariable(f, fc, store, currentPlanOpenParms, localPlanCubeGroup, varDateRangeProfile, nodeProfile, versionProfile, var2_localPlanCubeGroup, var2_DateRangeProfile, var2_nodeProfile, var2_versionProfile, fme);
                    return b;
                    //End TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
                }
                else if (et == filterDictionary.StoreVariablePercentage)
                {
                    PlanOpenParms currentPlanOpenParms = null;
                    if (f.currentPlanCubeGroup != null)
                    {
                        currentPlanOpenParms = f.currentPlanCubeGroup.OpenParms;
                    }


                    DateRangeProfile varDateRangeProfile;
                    HierarchyNodeProfile nodeProfile;
                    VersionProfile versionProfile;
                    PlanCubeGroup localPlanCubeGroup = SetCubeInfo(true, f, fc, currentPlanOpenParms, out varDateRangeProfile, out nodeProfile, out versionProfile);

                    DateRangeProfile var2_DateRangeProfile;
                    HierarchyNodeProfile var2_nodeProfile;
                    VersionProfile var2_versionProfile;
                    PlanCubeGroup var2_localPlanCubeGroup = SetCubeInfo(false, f, fc, currentPlanOpenParms, out var2_DateRangeProfile, out var2_nodeProfile, out var2_versionProfile);
                    //IDictionaryEnumerator _enumerator = f.filterCubeGroupHash.GetEnumerator();
                    //while (_enumerator.MoveNext())
                    //{
                    //    ((CubeGroupHashEntry)_enumerator.Value).FilterCubeGroup.OpenCubeGroup(((CubeGroupHashEntry)_enumerator.Value).FilterOpenParms);
                    //}

                    //Begin TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
                    //ProfileList dateProfileList;

                    //if (currentPlanOpenParms == null)
                    //{
                    //    currentPlanOpenParms = localPlanCubeGroup.OpenParms;
                    //}
                    //if (currentPlanOpenParms.DateRangeProfile.SelectedDateType == eCalendarDateType.Week)
                    //{
                    //    dateProfileList = f.cubeSAB.ApplicationServerSession.Calendar.GetWeekRange(currentPlanOpenParms.DateRangeProfile, currentPlanOpenParms.DateRangeProfile.InternalAnchorDate);
                    //}
                    //else
                    //{
                    //    dateProfileList = f.cubeSAB.ApplicationServerSession.Calendar.GetPeriodRange(currentPlanOpenParms.DateRangeProfile, currentPlanOpenParms.DateRangeProfile.InternalAnchorDate);
                    //}


                    //foreach (DateProfile dateProf in dateProfileList)                  
                    //{
                    //    FilterMonitor.FilterMonitorEntry fme = null;
                    //    if (FilterMonitor.doMonitor)
                    //    {
                    //        fme = new FilterMonitor.FilterMonitorEntry();
                    //        fme.filterName = f.filterName;
                    //        filterManager manager = new filterManager(f, null, null, null);
                    //        manager.Options.ColorFormatBlackBlueRed = false;
                    //        manager.Options.ColorFormatBlackRed = false;
                    //        manager.Options.ColorFormatGreenBlueRed = false;
                    //        f.RebuildText(manager, f.FindConditionNode(fc.Seq));
                    //        fme.conditionText = filterUtility.UnEscapeStringToFormat(f.FindConditionNode(fc.Seq).condition.NodeFormattedText);
                    //        fme.storeID = store.StoreId;
                    //        fme.outerWeek = dateProf.Date.ToShortDateString();
                    //    }
                    //    bool b = ProcessVariablePercentage(f, fc, store, currentPlanOpenParms, dateProf, localPlanCubeGroup, varDateRangeProfile, nodeProfile, versionProfile, var2_localPlanCubeGroup, var2_DateRangeProfile, var2_nodeProfile, var2_versionProfile, fme);
                    //    if (b == true)
                    //    {
                    //        return b;
                    //    }
                    //}
                    //return false;


                    FilterMonitor.FilterMonitorEntry fme = null;
                    if (FilterMonitor.doMonitor)
                    {
                        fme = new FilterMonitor.FilterMonitorEntry();
                        fme.filterName = f.filterName;
                        filterManager manager = new filterManager(f, null, null, null);
                        manager.Options.ColorFormatBlackBlueRed = false;
                        manager.Options.ColorFormatBlackRed = false;
                        manager.Options.ColorFormatGreenBlueRed = false;
                        f.RebuildText(manager, f.FindConditionNode(fc.Seq));
                        fme.conditionText = filterUtility.UnEscapeStringToFormat(f.FindConditionNode(fc.Seq).condition.NodeFormattedText);
                        fme.storeID = store.StoreId;
                        //fme.outerWeek = dateProf.Date.ToShortDateString();
                    }
                    bool b = ProcessVariablePercentage(f, fc, store, currentPlanOpenParms, localPlanCubeGroup, varDateRangeProfile, nodeProfile, versionProfile, var2_localPlanCubeGroup, var2_DateRangeProfile, var2_nodeProfile, var2_versionProfile, fme);
                    return b;
                    //End TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //TO DO - remove after debugging
                if (fc.valueToCompare.Contains("True"))
                {
                    fc.executeResult = "T";
                    return true;

                }
                else
                {
                    fc.executeResult = "F";
                    return false;
                }
            }

        }

        private static PlanCubeGroup SetCubeInfo(bool useVar1, filter f, filterCondition fc, PlanOpenParms currentPlanOpenParms, out DateRangeProfile varDateRangeProfile, out HierarchyNodeProfile nodeProfile, out VersionProfile versionProfile)
        {
            variableValueTypes valueType;
            if (useVar1)
            {
                valueType = variableValueTypes.FromIndex(fc.variable1_VariableValueTypeIndex);
            }
            else
            {
                valueType = variableValueTypes.FromIndex(fc.variable2_VariableValueTypeIndex);
            }
            PlanCubeGroupWaferCubeFlags cumulatedFlags = SetCubeFlagsForVariable(valueType);
            //DateRangeProfile varDateRangeProfile;
            if (useVar1)
            {
                varDateRangeProfile = SetDateRangeProfileForVariable(f.cubeSAB, fc.variable1_CDR_RID);
            }
            else
            {
                varDateRangeProfile = SetDateRangeProfileForVariable(f.cubeSAB, fc.variable2_CDR_RID);
            }

           // HierarchyNodeProfile nodeProfile;
            if (useVar1)
            {
                nodeProfile = SetNodeProfileForVariable(f.cubeSAB, fc.variable1_HN_RID);
            }
            else
            {
                nodeProfile = SetNodeProfileForVariable(f.cubeSAB, fc.variable2_HN_RID);
            }

            //VersionProfile versionProfile;
            if (useVar1)
            {
                versionProfile = SetVersionProfileForVariable(f.cubeSAB, fc.variable1_VersionIndex);
            }
            else
            {
                versionProfile = SetVersionProfileForVariable(f.cubeSAB, fc.variable2_VersionIndex);
            }

            bool chainCurrentCubeGroup;
            bool storeCurrentCubeGroup;
            SetCurrentCubeGroupFlagsForVariable(out chainCurrentCubeGroup, out storeCurrentCubeGroup, f.currentPlanCubeGroup, nodeProfile, versionProfile, varDateRangeProfile, currentPlanOpenParms, cumulatedFlags);
            
            //set the date range equal to plan date range if no date range was specified
            if (varDateRangeProfile == null)
            {
                varDateRangeProfile = currentPlanOpenParms.DateRangeProfile;
            }
            else
            {
                if (varDateRangeProfile.RelativeTo == eDateRangeRelativeTo.Plan)
                {
                    if (currentPlanOpenParms == null)
                    {
                        throw new FilterUsesCurrentPlanException();
                    }

                    varDateRangeProfile = f.cubeSAB.ApplicationServerSession.Calendar.GetDateRange(varDateRangeProfile.Key, currentPlanOpenParms.DateRangeProfile.Key);
                }
            }

            return SetLocalCubeGroupForVariable(chainCurrentCubeGroup, storeCurrentCubeGroup, f, ref nodeProfile, ref versionProfile, ref varDateRangeProfile, currentPlanOpenParms, cumulatedFlags);

        }
        private static PlanCubeGroup SetCubeInfoForSort(filter f, filterCondition fc, PlanOpenParms currentPlanOpenParms, out DateRangeProfile varDateRangeProfile, out HierarchyNodeProfile nodeProfile, out VersionProfile versionProfile)
        {

            variableValueTypes valueType = variableValueTypes.StoreDetail;
          
            PlanCubeGroupWaferCubeFlags cumulatedFlags = SetCubeFlagsForVariable(valueType);
            //DateRangeProfile varDateRangeProfile;

            varDateRangeProfile = null; // SetDateRangeProfileForVariable(f.cubeSAB, Include.UndefinedCalendarDateRange);
          

            // HierarchyNodeProfile nodeProfile;

            nodeProfile = null; // SetNodeProfileForVariable(f.cubeSAB, fc.variable1_HN_RID);
          

            //VersionProfile versionProfile;

            versionProfile = null; // SetVersionProfileForVariable(f.cubeSAB, fc.variable1_VersionIndex);
           

            bool chainCurrentCubeGroup;
            bool storeCurrentCubeGroup;
            SetCurrentCubeGroupFlagsForVariable(out chainCurrentCubeGroup, out storeCurrentCubeGroup, f.currentPlanCubeGroup, nodeProfile, versionProfile, varDateRangeProfile, currentPlanOpenParms, cumulatedFlags);

            //set the date range equal to plan date range if no date range was specified
            if (varDateRangeProfile == null)
            {
                varDateRangeProfile = currentPlanOpenParms.DateRangeProfile;
            }
            else
            {
                if (varDateRangeProfile.RelativeTo == eDateRangeRelativeTo.Plan)
                {
                    if (currentPlanOpenParms == null)
                    {
                        throw new FilterUsesCurrentPlanException();
                    }

                    varDateRangeProfile = f.cubeSAB.ApplicationServerSession.Calendar.GetDateRange(varDateRangeProfile.Key, currentPlanOpenParms.DateRangeProfile.Key);
                }
            }

            return SetLocalCubeGroupForVariable(chainCurrentCubeGroup, storeCurrentCubeGroup, f, ref nodeProfile, ref versionProfile, ref varDateRangeProfile, currentPlanOpenParms, cumulatedFlags);

        }

        private static void InitializeCubeGroupHashEntry(CubeGroupHashEntry aCubeGroupHashEntry, PlanOpenParms _currentPlanOpenParms, SessionAddressBlock _SAB, ApplicationSessionTransaction _transaction, DateRangeProfile aDateRangeProf)
        {
            try
            {
                aCubeGroupHashEntry.FilterCubeGroup = new FilterCubeGroup(_SAB, _transaction);
                //Begin Track #6251 - JScott - Get System Null Ref Excp using filter
                //aCubeGroupHashEntry.FilterOpenParms = new FilterOpenParms(ePlanSessionType.StoreSingleLevel, _SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name);
                aCubeGroupHashEntry.FilterOpenParms = new FilterOpenParms(ePlanSessionType.None, _SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name);
                //End Track #6251 - JScott - Get System Null Ref Excp using filter
                aCubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.NodeProfile = null;
                aCubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.VersionProfile = null;
                aCubeGroupHashEntry.FilterOpenParms.StoreHLPlanProfile.NodeProfile = null;
                aCubeGroupHashEntry.FilterOpenParms.StoreHLPlanProfile.VersionProfile = null;
                aCubeGroupHashEntry.FilterOpenParms.DateRangeProfile = aDateRangeProf;
                aCubeGroupHashEntry.FilterOpenParms.StoreGroupRID = Include.NoRID;
                aCubeGroupHashEntry.FilterOpenParms.IneligibleStores = true;
                aCubeGroupHashEntry.FilterOpenParms.SimilarStores = false;

                if (_currentPlanOpenParms.StoreGroupRID == Include.NoRID)
                {
                    aCubeGroupHashEntry.FilterOpenParms.StoreGroupRID = Include.AllStoreGroupRID;
                }
                else
                {
                    aCubeGroupHashEntry.FilterOpenParms.StoreGroupRID = _currentPlanOpenParms.StoreGroupRID;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private static bool ProcessVariableToConstant(PlanCubeGroup localPlanCubeGroup, filter f, filterCondition fc, DateRangeProfile varDateRangeProfile, HierarchyNodeProfile nodeProfile, VersionProfile versionProfile, StoreProfile storeProfile, PlanOpenParms currentPlanOpenParms, FilterMonitor.FilterMonitorEntry fme = null) //DateProfile currentDateProfile, FilterMonitor.FilterMonitorEntry fme = null) //TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
        {

            //Begin TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
            //double[] firstValues = GetVariableValuesFromCondition(localPlanCubeGroup, true, f, fc, varDateRangeProfile, nodeProfile, versionProfile, storeProfile, currentPlanOpenParms, currentDateProfile, fme);
            double[] firstValues = GetVariableValuesFromCondition(localPlanCubeGroup, true, f, fc, varDateRangeProfile, nodeProfile, versionProfile, storeProfile, currentPlanOpenParms, fme);
            //End TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.

            double[] secondValues = new double[1];

            double valToCompare = -1;


            if (filterDataHelper.VariablesGetIsGrade(fc.variable1_Index)) //special handling of Grade variable
            {
                 if (fc.valueToCompare != null) 
                 {
                     //Begin TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results
                     //valToCompare = (double)localPlanCubeGroup.Transaction.GetStoreGradeList(nodeProfile.Key).GetStoreGradeKey(fc.valueToCompare);

                     //List<StoreGradeProfile> gradeList = localPlanCubeGroup.Transaction.GetStoreGradeList(nodeProfile.Key).GetStoreGradeList();
                     foreach (StoreGradeProfile sgp in localPlanCubeGroup.Transaction.GetStoreGradeList(nodeProfile.Key))
                     {
                         bool found = filterDataHelper.CompareToString(sgp.StoreGrade, fc);
                         if (found)
                         {
                             valToCompare = (double)sgp.Key;
                             break;
                         }
                     }
                     //End TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results

                     
                 }
                          
            }
            else if (fc.valueTypeIndex == filterValueTypes.Numeric)
            {
                if (fc.numericTypeIndex == filterNumericTypes.Integer)
                {
                     Double.TryParse(fc.valueToCompareInt.ToString(), out valToCompare);
                }
                else
                {
                      valToCompare = (double)fc.valueToCompareDouble;
                }
            }
     

      
         

            secondValues[0] = valToCompare;
            //Begin TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results
            if (filterDataHelper.VariablesGetIsGrade(fc.variable1_Index)) //special handling of Grade variable
            {
                //override the numerical operator index since it is being used as a string operator
                string detailedComparison = string.Empty;
                return EvaluateCondition(ref detailedComparison, firstValues, secondValues, variableTimeTypes.FromIndex(fc.variable1_TimeTypeIndex), variableTimeTypes.Any, fc, filterNumericOperatorTypes.DoesEqual);
            }
            else
            {
                string detailedComparison = string.Empty;
                bool retValue = EvaluateCondition(ref detailedComparison, firstValues, secondValues, variableTimeTypes.FromIndex(fc.variable1_TimeTypeIndex), variableTimeTypes.Any, fc);
                detailedComparison = retValue.ToString() + System.Environment.NewLine + detailedComparison;
                if (fme != null)
                {
                    string variableName = filterDataHelper.VariablesGetNameFromIndex(fc.variable1_Index);
                    fme.AddDateAndFieldColumn("", variableName + "(2nd)");
                    fme.AddDateAndFieldColumn("", variableName + "(Result)");
    
                    fme.AddDateAndFieldValue("", variableName + "(2nd)", valToCompare.ToString());
                    fme.AddDateAndFieldValue("", variableName + "(Result)", detailedComparison);
                    FilterMonitor.AddRow(fme);
                }
                return retValue;
            }
            //End TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results
        }
        private static double ProcessVariableSorting(PlanCubeGroup localPlanCubeGroup, filter f, filterCondition fc, DateRangeProfile varDateRangeProfile, HierarchyNodeProfile nodeProfile, VersionProfile versionProfile, StoreProfile storeProfile, PlanOpenParms currentPlanOpenParms) //, DateProfile currentDateProfile)      //TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
        {
            //double[] firstValues = GetVariableValuesFromCondition(localPlanCubeGroup, true, f, fc, varDateRangeProfile, nodeProfile, versionProfile, storeProfile, currentPlanOpenParms, currentDateProfile);
            variableValueTypes valueType = variableValueTypes.StoreDetail;


            bool isTimeTotalVariable = false;           
            if (filterDataHelper.VariablesGetIsTimeTotal(fc.sortByFieldIndex) != 0)
            {
                isTimeTotalVariable = true;
            }
            
            variableTimeTypes timeType = variableTimeTypes.Total;

            //Begin TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
            //ProfileList detailDateProfList = SetDateRangeProfileForVariable(f.cubeSAB, varDateRangeProfile, timeType, storeProfile, currentDateProfile);
            ProfileList detailDateProfList = SetDateRangeProfileForVariable(f.cubeSAB, varDateRangeProfile, timeType, storeProfile);

            //End TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.

            int varIndex = fc.sortByFieldIndex;
           
            double[] valueList = GetValuesForVariable(isTimeTotalVariable, varIndex, valueType, timeType, localPlanCubeGroup, nodeProfile, versionProfile, varDateRangeProfile, storeProfile, detailDateProfList);

            return valueList[0];             
        }

        //private static bool ProcessVariableToVariable(filter f, filterCondition fc, StoreProfile storeProfile, PlanOpenParms currentPlanOpenParms, DateProfile currentDateProfile, PlanCubeGroup var1_localPlanCubeGroup, DateRangeProfile var1_DateRangeProfile, HierarchyNodeProfile var1_nodeProfile, VersionProfile var1_versionProfile, PlanCubeGroup var2_localPlanCubeGroup, DateRangeProfile var2_DateRangeProfile, HierarchyNodeProfile var2_nodeProfile, VersionProfile var2_versionProfile, FilterMonitor.FilterMonitorEntry fme = null)
        private static bool ProcessVariableToVariable(filter f, filterCondition fc, StoreProfile storeProfile, PlanOpenParms currentPlanOpenParms, PlanCubeGroup var1_localPlanCubeGroup, DateRangeProfile var1_DateRangeProfile, HierarchyNodeProfile var1_nodeProfile, VersionProfile var1_versionProfile, PlanCubeGroup var2_localPlanCubeGroup, DateRangeProfile var2_DateRangeProfile, HierarchyNodeProfile var2_nodeProfile, VersionProfile var2_versionProfile, FilterMonitor.FilterMonitorEntry fme = null) //TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
        {
            //Begin TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
            //double[] firstValues = GetVariableValuesFromCondition(var1_localPlanCubeGroup, true, f, fc, var1_DateRangeProfile, var1_nodeProfile, var1_versionProfile, storeProfile, currentPlanOpenParms, currentDateProfile, fme);
            //double[] secondValues = GetVariableValuesFromCondition(var2_localPlanCubeGroup, false, f, fc, var2_DateRangeProfile, var2_nodeProfile, var2_versionProfile, storeProfile, currentPlanOpenParms, currentDateProfile, fme);
            double[] firstValues = GetVariableValuesFromCondition(var1_localPlanCubeGroup, true, f, fc, var1_DateRangeProfile, var1_nodeProfile, var1_versionProfile, storeProfile, currentPlanOpenParms, fme);
            double[] secondValues = GetVariableValuesFromCondition(var2_localPlanCubeGroup, false, f, fc, var2_DateRangeProfile, var2_nodeProfile, var2_versionProfile, storeProfile, currentPlanOpenParms, fme);
            //End TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.

            string detailedComparison = string.Empty;
            bool retValue = EvaluateCondition(ref detailedComparison, firstValues, secondValues, variableTimeTypes.FromIndex(fc.variable1_TimeTypeIndex), variableTimeTypes.FromIndex(fc.variable2_TimeTypeIndex), fc);
            detailedComparison = retValue.ToString() + System.Environment.NewLine + detailedComparison;
            if (fme != null)
            {
                string variableName = filterDataHelper.VariablesGetNameFromIndex(fc.variable1_Index);
                fme.AddDateAndFieldColumn("", variableName + "(Result)");
                fme.AddDateAndFieldValue("", variableName + "(Result)", detailedComparison);
                FilterMonitor.AddRow(fme);
            }
            return retValue;
        }

        //private static bool ProcessVariablePercentage(filter f, filterCondition fc, StoreProfile storeProfile, PlanOpenParms currentPlanOpenParms, DateProfile currentDateProfile, PlanCubeGroup var1_localPlanCubeGroup, DateRangeProfile var1_DateRangeProfile, HierarchyNodeProfile var1_nodeProfile, VersionProfile var1_versionProfile, PlanCubeGroup var2_localPlanCubeGroup, DateRangeProfile var2_DateRangeProfile, HierarchyNodeProfile var2_nodeProfile, VersionProfile var2_versionProfile, FilterMonitor.FilterMonitorEntry fme = null)
        private static bool ProcessVariablePercentage(filter f, filterCondition fc, StoreProfile storeProfile, PlanOpenParms currentPlanOpenParms, PlanCubeGroup var1_localPlanCubeGroup, DateRangeProfile var1_DateRangeProfile, HierarchyNodeProfile var1_nodeProfile, VersionProfile var1_versionProfile, PlanCubeGroup var2_localPlanCubeGroup, DateRangeProfile var2_DateRangeProfile, HierarchyNodeProfile var2_nodeProfile, VersionProfile var2_versionProfile, FilterMonitor.FilterMonitorEntry fme = null) //TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
        {
            //Begin TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
            //double[] firstValues = GetVariableValuesFromCondition(var1_localPlanCubeGroup, true, f, fc, var1_DateRangeProfile, var1_nodeProfile, var1_versionProfile, storeProfile, currentPlanOpenParms, currentDateProfile, fme);       
            //double[] pctValues = GetVariableValuesFromCondition(var2_localPlanCubeGroup, false, f, fc, var2_DateRangeProfile, var2_nodeProfile, var2_versionProfile, storeProfile, currentPlanOpenParms, currentDateProfile, fme);

            double[] firstValues = GetVariableValuesFromCondition(var1_localPlanCubeGroup, true, f, fc, var1_DateRangeProfile, var1_nodeProfile, var1_versionProfile, storeProfile, currentPlanOpenParms, fme);
            double[] pctValues = GetVariableValuesFromCondition(var2_localPlanCubeGroup, false, f, fc, var2_DateRangeProfile, var2_nodeProfile, var2_versionProfile, storeProfile, currentPlanOpenParms, fme);
            //End TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
            
            double[] secondValues = new double[1];
            secondValues[0] = (double)fc.valueToCompareDouble;

            filterPercentageOperatorTypes percentOp = filterPercentageOperatorTypes.FromIndex(fc.operatorVariablePercentageIndex);

            bool doPercentOf = false;
            bool doPercentChange = false;
            if (percentOp == filterPercentageOperatorTypes.PercentOf)
            {
                doPercentOf = true;
            }
            if (percentOp == filterPercentageOperatorTypes.PercentChange)
            {
                doPercentChange = true;
            }

            bool retValue = EvaluatePctCondition(firstValues, pctValues, secondValues, variableTimeTypes.FromIndex(fc.variable1_TimeTypeIndex), variableTimeTypes.FromIndex(fc.variable2_TimeTypeIndex), variableTimeTypes.Any, fc, doPercentOf, doPercentChange);
            if (fme != null)
            {
                string variableName = filterDataHelper.VariablesGetNameFromIndex(fc.variable1_Index);
                fme.AddDateAndFieldColumn("", variableName + "(%Constant)");
                fme.AddDateAndFieldColumn("", variableName + "(Result)");
                fme.AddDateAndFieldValue("", variableName + "(%Constant)", secondValues[0].ToString());
                fme.AddDateAndFieldValue("", variableName + "(Result)", retValue.ToString());
                FilterMonitor.AddRow(fme);
            }
            return retValue;
        }
        
        private static bool EvaluatePctCondition(
            double[] aFirstValues,
            double[] aPctValues,
            double[] aSecondValues,
            variableTimeTypes aFirstTimeMod,
            variableTimeTypes aPctTimeMod,
            variableTimeTypes aSecondTimeMod,
            filterCondition fc,
            bool aPctOf,
            bool aPctChange)
        {
            bool result;
            int i;

            try
            {
                result = false;

                for (i = 0; i < aFirstValues.Length; i++)
                {
                    result = EvaluatePctValue(
                        aFirstValues,
                        aPctValues,
                        aSecondValues,
                        aFirstTimeMod,
                        aPctTimeMod,
                        aSecondTimeMod,
                        fc,
                        aPctOf,
                        aPctChange,
                        i,
                        aFirstValues[i]);

                    if (result != (aFirstTimeMod == variableTimeTypes.All))
                    {
                        break;
                    }
                }

                return result;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private static bool EvaluatePctValue(
            double[] aFirstValues,
            double[] aPctValues,
            double[] aSecondValues,
            variableTimeTypes aFirstTimeMod,
            variableTimeTypes aPctTimeMod,
            variableTimeTypes aSecondTimeMod,
            filterCondition fc,
            bool aPctOf,
            bool aPctChange,
            int aFirstIndex,
            double aFirstValue)
        {
            bool result;
            int i;

            try
            {
                result = (aFirstTimeMod == variableTimeTypes.All);

                if (aPctTimeMod == variableTimeTypes.Corresponding)
                {
                    if (aFirstIndex < aPctValues.Length)
                    {
                        result = EvaluatePctSecondValue(
                            aFirstValues,
                            aPctValues,
                            aSecondValues,
                            aFirstTimeMod,
                            aPctTimeMod,
                            aSecondTimeMod,
                            fc,
                            aPctOf,
                            aPctChange,
                            aFirstIndex,
                            aFirstValue,
                            aPctValues[aFirstIndex]);
                    }
                }
                else
                {
                    for (i = 0; i < aPctValues.Length; i++)
                    {
                        result = EvaluatePctSecondValue(
                            aFirstValues,
                            aPctValues,
                            aSecondValues,
                            aFirstTimeMod,
                            aPctTimeMod,
                            aSecondTimeMod,
                            fc,
                            aPctOf,
                            aPctChange,
                            aFirstIndex,
                            aFirstValue,
                            aPctValues[i]);

                        if (result != (aPctTimeMod == variableTimeTypes.All))
                        {
                            break;
                        }
                    }
                }

                return result;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private static bool EvaluatePctSecondValue(
            double[] aFirstValues,
            double[] aPctValues,
            double[] aSecondValues,
            variableTimeTypes aFirstTimeMod,
            variableTimeTypes aPctTimeMod,
            variableTimeTypes aSecondTimeMod,
            filterCondition fc, 
            bool aPctOf,
            bool aPctChange,
            int aFirstIndex,
            double aFirstValue,
            double aPctValue)
        {
            bool result;
            int i;

            try
            {
                result = (aPctTimeMod == variableTimeTypes.All);

                if (aSecondTimeMod == variableTimeTypes.Corresponding)
                {
                    if (aFirstIndex < aSecondValues.Length)
                    {
                        result = ComparePctValues(
                            aFirstValues,
                            aPctValues,
                            aSecondValues,
                            aFirstTimeMod,
                            aPctTimeMod,
                            aSecondTimeMod,
                            fc,
                            aPctOf,
                            aPctChange,
                            aFirstIndex,
                            aFirstValue,
                            aPctValue,
                            aSecondValues[aFirstIndex]);
                    }
                }
                else
                {
                    for (i = 0; i < aSecondValues.Length; i++)
                    {
                        result = ComparePctValues(
                            aFirstValues,
                            aPctValues,
                            aSecondValues,
                            aFirstTimeMod,
                            aPctTimeMod,
                            aSecondTimeMod,
                            fc,
                            aPctOf,
                            aPctChange,
                            aFirstIndex,
                            aFirstValue,
                            aPctValue,
                            aSecondValues[i]);

                        if (result != (aSecondTimeMod == variableTimeTypes.All))
                        {
                            break;
                        }
                    }
                }

                return result;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private static bool ComparePctValues(
            double[] aFirstValues,
            double[] aPctValues,
            double[] aSecondValues,
            variableTimeTypes aFirstTimeMod,
            variableTimeTypes aPctTimeMod,
            variableTimeTypes aSecondTimeMod,
            filterCondition fc,
            bool aPctOf,
            bool aPctChange,
            int aFirstIndex,
            double aFirstValue,
            double aPctValue,
            double val2)
        {
            double val1;

            try
            {
                //Begin TT#1668-MD jsobek -Filter - Based on Filter set up want to see stores
                if (aPctValue == 0)
                {
                    //use zero for the divide by zero case
                    val1 = 0;
                }
                else
                {
                    if (aPctOf)
                    {
                        val1 = (aFirstValue / aPctValue) * 100;
                    }
                    else
                    {
                        val1 = ((aFirstValue - aPctValue) / aPctValue) * 100;
                    }
                }
                //End TT#1668-MD jsobek -Filter - Based on Filter set up want to see stores

                filterNumericOperatorTypes numericOp = filterNumericOperatorTypes.FromIndex(fc.operatorIndex);

                if (numericOp == filterNumericOperatorTypes.DoesEqual)
                {
                    return (val1 == val2);
                }
                else if (numericOp == filterNumericOperatorTypes.DoesNotEqual)
                {
                    return (val1 != val2);
                }
                else if (numericOp == filterNumericOperatorTypes.GreaterThan)
                {
                    return (val1 > val2);
                }
                else if (numericOp == filterNumericOperatorTypes.GreaterThanOrEqualTo)
                {
                    return (val1 >= val2);
                }
                else if (numericOp == filterNumericOperatorTypes.LessThan)
                {
                    return (val1 < val2);
                }
                else if (numericOp == filterNumericOperatorTypes.LessThanOrEqualTo)
                {
                    return (val1 <= val2);
                }
                else if (numericOp == filterNumericOperatorTypes.Between)
                {
                    //Begin TT#1508-MD -jsobek -Store Filters - Fix between operator for integer types
                    double val3;
                    if (fc.numericTypeIndex == filterNumericTypes.Integer)
                    {
                        val3 = (double)fc.valueToCompareInt2;
                    }
                    else
                    {
                        val3 = (double)fc.valueToCompareDouble2;
                    }
                    //End TT#1508-MD -jsobek -Store Filters - Fix between operator for integer types
                    return (val1 >= val2 && val1 <= val3);
                }
                else
                {
                    // compare as does equal
                    return (val1 == val2);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private static double[] GetVariableValuesFromCondition(PlanCubeGroup localPlanCubeGroup, bool useVar1, filter f, filterCondition fc, DateRangeProfile varDateRangeProfile, HierarchyNodeProfile nodeProfile, VersionProfile versionProfile, StoreProfile storeProfile, PlanOpenParms currentPlanOpenParms, FilterMonitor.FilterMonitorEntry fme = null) //DateProfile currentDateProfile, FilterMonitor.FilterMonitorEntry fme = null)   //TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
        {
            variableValueTypes valueType;
            if (useVar1)
            {
                valueType = variableValueTypes.FromIndex(fc.variable1_VariableValueTypeIndex);
            }
            else
            {
                valueType = variableValueTypes.FromIndex(fc.variable2_VariableValueTypeIndex);
            }
            //PlanCubeGroupWaferCubeFlags cumulatedFlags = SetCubeFlagsForVariable(valueType);

            //DateRangeProfile varDateRangeProfile;
            //if (useVar1)
            //{
            //    varDateRangeProfile = SetDateRangeProfileForVariable(f.cubeSAB, fc.variable1_CDR_RID);
            //}
            //else
            //{
            //    varDateRangeProfile = SetDateRangeProfileForVariable(f.cubeSAB, fc.variable2_CDR_RID);
            //}

            //HierarchyNodeProfile nodeProfile;
            //if (useVar1)
            //{
            //    nodeProfile = SetNodeProfileForVariable(f.cubeSAB, fc.variable1_HN_RID);
            //}
            //else
            //{
            //    nodeProfile = SetNodeProfileForVariable(f.cubeSAB, fc.variable2_HN_RID);
            //}

            //VersionProfile versionProfile;
            //if (useVar1)
            //{
            //    versionProfile = SetVersionProfileForVariable(f.cubeSAB, fc.variable1_VersionIndex);
            //}
            //else
            //{
            //    versionProfile = SetVersionProfileForVariable(f.cubeSAB, fc.variable2_VersionIndex);
            //}

            //bool chainCurrentCubeGroup;
            //bool storeCurrentCubeGroup;
            //SetCurrentCubeGroupFlagsForVariable(out chainCurrentCubeGroup, out storeCurrentCubeGroup, f.currentPlanCubeGroup, nodeProfile, versionProfile, varDateRangeProfile, currentPlanOpenParms, cumulatedFlags);


            ////set the date range equal to plan date range if no date range was specified
            //if (varDateRangeProfile == null)
            //{
            //    varDateRangeProfile = currentPlanOpenParms.DateRangeProfile;
            //}
            //else
            //{
            //    if (varDateRangeProfile.RelativeTo == eDateRangeRelativeTo.Plan)
            //    {
            //        if (currentPlanOpenParms == null)
            //        {
            //            throw new FilterUsesCurrentPlanException();
            //        }

            //        varDateRangeProfile = f.cubeSAB.ApplicationServerSession.Calendar.GetDateRange(varDateRangeProfile.Key, currentPlanOpenParms.DateRangeProfile.Key);
            //    }
            //}

            ////Set the local plan cube group
            //PlanCubeGroup localPlanCubeGroup = SetLocalCubeGroupForVariable(chainCurrentCubeGroup, storeCurrentCubeGroup, f, nodeProfile, versionProfile, varDateRangeProfile, currentPlanOpenParms, cumulatedFlags);

            bool isTimeTotalVariable = false;
            if (useVar1)
            {
                if (filterDataHelper.VariablesGetIsTimeTotal(fc.variable1_Index) != 0)
                {
                    isTimeTotalVariable = true;
                }
            }
            else
            {
                if (filterDataHelper.VariablesGetIsTimeTotal(fc.variable2_Index) != 0)
                {
                    isTimeTotalVariable = true;
                }
            }
           

            variableTimeTypes timeType;          
            if (useVar1)
            {
                timeType = variableTimeTypes.FromIndex(fc.variable1_TimeTypeIndex);
            }
            else
            {
                timeType = variableTimeTypes.FromIndex(fc.variable2_TimeTypeIndex);
            }

            //Begin TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
            //ProfileList detailDateProfList = SetDateRangeProfileForVariable(f.cubeSAB, varDateRangeProfile, timeType, storeProfile, currentDateProfile);
            ProfileList detailDateProfList = SetDateRangeProfileForVariable(f.cubeSAB, varDateRangeProfile, timeType, storeProfile);
            //End TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.

            int varIndex;         
            if (useVar1)
            {
                varIndex = fc.variable1_Index;
            }
            else
            {
                varIndex = fc.variable2_Index;
            }

            string varDescriptionSuffix;
            if (useVar1)
            {
                varDescriptionSuffix = "(1st)";
            }
            else
            {
                varDescriptionSuffix = "(2nd)";
            }

            return GetValuesForVariable(isTimeTotalVariable, varIndex, valueType, timeType, localPlanCubeGroup, nodeProfile, versionProfile, varDateRangeProfile, storeProfile, detailDateProfList, fme, varDescriptionSuffix);

        }

        private static double[] GetValuesForVariable(bool isTimeTotalVariable, int variableIndex, variableValueTypes variableValueType, variableTimeTypes variableTimeType, PlanCubeGroup localPlanCubeGroup, HierarchyNodeProfile nodeProfile, VersionProfile versionProfile, DateRangeProfile varDateRangeProfile, StoreProfile aStoreProf, ProfileList aDetailDateProfList, FilterMonitor.FilterMonitorEntry fme = null, string varDescriptionSuffix = "")
        {
            double[] valueList = null;
            PlanCellReference planCellRef;
            double totalValue;
            int count;

            try
            {
               
                eCubeType cubeType = SetCubeTypeForVariable(isTimeTotalVariable, variableValueType, varDateRangeProfile);

                planCellRef = (PlanCellReference)localPlanCubeGroup.GetCube(cubeType).CreateCellReference();
                planCellRef[eProfileType.Version] = versionProfile.Key;
                planCellRef[eProfileType.HierarchyNode] = nodeProfile.Key;

                if (variableValueType == variableValueTypes.StoreAverage)
                {
                    planCellRef[eProfileType.QuantityVariable] = filterDataHelper.StoreAverageQuantityKey;
                }
                else
                {
                    planCellRef[eProfileType.QuantityVariable] = filterDataHelper.ValueQuantityKey;
                }
                int varKey;
                int timeTotalKey;
                filterDataHelper.VariablesGetTimeTotalKeys(variableIndex, out varKey, out timeTotalKey);
                switch (cubeType.Id)
                {
                    case eCubeType.cStorePlanWeekDetail:
                    case eCubeType.cStorePlanPeriodDetail:
                    case eCubeType.cStorePlanStoreTotalWeekDetail:
                    case eCubeType.cStorePlanStoreTotalPeriodDetail:
                    case eCubeType.cChainPlanWeekDetail:
                    case eCubeType.cChainPlanPeriodDetail:

                        planCellRef[eProfileType.Variable] = varKey; // ((DataQueryPlanVariableOperand)aVariableOperand).VariableProfile.Key;                  
                        break;

                    case eCubeType.cStorePlanDateTotal:
                    case eCubeType.cStorePlanStoreTotalDateTotal:
                    case eCubeType.cChainPlanDateTotal:

                        
                        planCellRef[eProfileType.Variable] = varKey;
                        planCellRef[eProfileType.TimeTotalVariable] = timeTotalKey;
                        break;
                }

                switch (cubeType.Id)
                {
                    case eCubeType.cStorePlanWeekDetail:
                    case eCubeType.cStorePlanPeriodDetail:
                    case eCubeType.cStorePlanDateTotal:

                        planCellRef[eProfileType.Store] = aStoreProf.Key;
                        break;
                }

                switch (cubeType.Id)
                {
                    case eCubeType.cStorePlanWeekDetail:
                    case eCubeType.cStorePlanPeriodDetail:
                    case eCubeType.cStorePlanStoreTotalWeekDetail:
                    case eCubeType.cStorePlanStoreTotalPeriodDetail:
                    case eCubeType.cChainPlanWeekDetail:
                    case eCubeType.cChainPlanPeriodDetail:

                        if (variableTimeType == variableTimeTypes.Average || variableTimeType == variableTimeTypes.Total)
                        {
                            valueList = new double[1];
                        }
                        else
                        {
                            valueList = new double[aDetailDateProfList.Count];
                        }

                        totalValue = 0;
                        count = 0;

                        foreach (DateProfile dateProf in aDetailDateProfList)
                        {
                            switch (cubeType.Id)
                            {
                                case eCubeType.cStorePlanWeekDetail:
                                case eCubeType.cStorePlanStoreTotalWeekDetail:
                                case eCubeType.cChainPlanWeekDetail:

                                    planCellRef[eProfileType.Week] = dateProf.Key;
                                    break;

                                case eCubeType.cStorePlanPeriodDetail:
                                case eCubeType.cStorePlanStoreTotalPeriodDetail:
                                case eCubeType.cChainPlanPeriodDetail:

                                    planCellRef[eProfileType.Period] = dateProf.Key;
                                    break;
                            }

                            if (variableTimeType == variableTimeTypes.Average || variableTimeType == variableTimeTypes.Total)
                            {
                                totalValue += planCellRef.CurrentCellValue;
                            }
                            else
                            {
                                valueList[count] = planCellRef.CurrentCellValue;
                                if (fme != null)
                                {
                                    string variableName = filterDataHelper.VariablesGetNameFromIndex(variableIndex);
                                    fme.AddDateAndFieldColumn(dateProf.Date.ToShortDateString(), variableName + varDescriptionSuffix);
                                    fme.AddDateAndFieldValue(dateProf.Date.ToShortDateString(), variableName + varDescriptionSuffix, planCellRef.CurrentCellValue.ToString());
                                }
                            }

                            count++;
                        }

                        if (variableTimeType == variableTimeTypes.Average)
                        {
                            //Begin TT#1668-MD -jsobek -Filter - Based on Filter set up want to see stores
                            if (count == 0)
                            {
                                //use zero for the divide by zero case
                                valueList[0] = 0;
                            }
                            else
                            {
                                valueList[0] = totalValue / count;
                            }
                            //End TT#1668-MD -jsobek -Filter - Based on Filter set up want to see stores
                            if (fme != null)
                            {
                                string variableName = filterDataHelper.VariablesGetNameFromIndex(variableIndex);
                                fme.AddDateAndFieldColumn("", variableName + varDescriptionSuffix + "(Avg)");
                                fme.AddDateAndFieldValue("", variableName + varDescriptionSuffix + "(Avg)", valueList[0].ToString());
                            }
                         
                        }
                        else if (variableTimeType == variableTimeTypes.Total)
                        {
                            valueList[0] = totalValue;
                            if (fme != null)
                            {
                                string variableName = filterDataHelper.VariablesGetNameFromIndex(variableIndex);
                                fme.AddDateAndFieldColumn("", variableName + varDescriptionSuffix + "(Total)");
                                fme.AddDateAndFieldValue("", variableName + varDescriptionSuffix + "(Total)", valueList[0].ToString());
                            }
                        }

                        break;

                    case eCubeType.cStorePlanDateTotal:
                    case eCubeType.cStorePlanStoreTotalDateTotal:
                    case eCubeType.cChainPlanDateTotal:

                        valueList = new double[1];
                        //try
                        //{
                            valueList[0] = planCellRef.CurrentCellValue;
                            if (fme != null)
                            {
                                string variableName = filterDataHelper.VariablesGetNameFromIndex(variableIndex);
                                fme.AddDateAndFieldColumn("", variableName + varDescriptionSuffix + "(Chain)");
                                fme.AddDateAndFieldValue("", variableName + varDescriptionSuffix + "(Chain)", valueList[0].ToString());
                            }
                        //}
                        //catch (Exception exc)
                        //{
                        //    valueList[0] = 0;
                        //}
                        
                        break;
                }

                return valueList;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


        private static void SetCurrentCubeGroupFlagsForVariable(out bool chainCurrentCubeGroup, out bool storeCurrentCubeGroup, PlanCubeGroup currentPlanCubeGroup, HierarchyNodeProfile nodeProfile, VersionProfile versionProfile, DateRangeProfile varDateRangeProfile, PlanOpenParms currentPlanOpenParms, PlanCubeGroupWaferCubeFlags cumulatedFlags)
        {
         
            if (currentPlanCubeGroup != null)
            {
                chainCurrentCubeGroup = true;
                storeCurrentCubeGroup = true;


                if (varDateRangeProfile != null)
                {
                    if (varDateRangeProfile.CompareTo(currentPlanOpenParms.DateRangeProfile) != 0)
                    {
                        chainCurrentCubeGroup = false;
                        storeCurrentCubeGroup = false;
                    }
                }

                if (cumulatedFlags.isChainPlan)
                {

                    if (nodeProfile != null)
                    {
                        if (nodeProfile.Key != currentPlanOpenParms.ChainHLPlanProfile.NodeProfile.Key)
                        {
                            chainCurrentCubeGroup = false;
                        }
                    }


                    if (versionProfile != null)
                    {
                        if (versionProfile.Key != currentPlanOpenParms.ChainHLPlanProfile.VersionProfile.Key)
                        {
                            chainCurrentCubeGroup = false;
                        }
                    }
                }

                if (cumulatedFlags.isStorePlan)
                {
                    if (nodeProfile != null)
                    {
                        if (nodeProfile.Key != currentPlanOpenParms.StoreHLPlanProfile.NodeProfile.Key)
                        {
                            storeCurrentCubeGroup = false;
                        }
                    }


                    if (versionProfile != null)
                    {
                        if (versionProfile.Key != currentPlanOpenParms.StoreHLPlanProfile.VersionProfile.Key)
                        {
                            storeCurrentCubeGroup = false;
                        }
                    }
                }
            }
            else
            {
                chainCurrentCubeGroup = false;
                storeCurrentCubeGroup = false;
            }
        }

        private static eCubeType SetCubeTypeForVariable(bool isTimeTotalVariable, variableValueTypes variableValueType, DateRangeProfile varDateRangeProfile)
        {
            eCubeType cubeType = eCubeType.None;
            if (variableValueType == variableValueTypes.StoreAverage || variableValueType == variableValueTypes.StoreTotal)
            {
                if (isTimeTotalVariable == false)
                {
                    if (varDateRangeProfile.SelectedDateType == eCalendarDateType.Week)
                    {
                        cubeType = eCubeType.StorePlanStoreTotalWeekDetail;
                    }
                    else
                    {
                        cubeType = eCubeType.StorePlanStoreTotalPeriodDetail;
                    }
                }
                else
                {
                    cubeType = eCubeType.StorePlanStoreTotalDateTotal;
                }

            }
            else if (variableValueType == variableValueTypes.ChainDetail)
            {
                if (isTimeTotalVariable == false)
                {
                    if (varDateRangeProfile.SelectedDateType == eCalendarDateType.Week)
                    {
                        cubeType = eCubeType.ChainPlanWeekDetail;
                    }
                    else
                    {
                        cubeType = eCubeType.ChainPlanPeriodDetail;
                    }
                }
                else
                {
                    cubeType = eCubeType.ChainPlanDateTotal;
                }
            }
            else
            {
                if (isTimeTotalVariable == false)
                {
                    if (varDateRangeProfile.SelectedDateType == eCalendarDateType.Week)
                    {
                        cubeType = eCubeType.StorePlanWeekDetail;
                    }
                    else
                    {
                        cubeType = eCubeType.StorePlanPeriodDetail;
                    }
                }
                else
                {
                    cubeType = eCubeType.StorePlanDateTotal;
                }
            }
            return cubeType;
        }
        private static PlanCubeGroup SetLocalCubeGroupForVariable(bool chainCurrentCubeGroup, bool storeCurrentCubeGroup, filter f, ref HierarchyNodeProfile nodeProfile, ref VersionProfile versionProfile, ref DateRangeProfile varDateRangeProfile, PlanOpenParms currentPlanOpenParms, PlanCubeGroupWaferCubeFlags cumulatedFlags)
        {
            PlanCubeGroup localPlanCubeGroup = null;

            if (cumulatedFlags.isChainPlan)
            {
                bool buildGroup = false;


                if (chainCurrentCubeGroup && f.currentPlanCubeGroup != null)
                {
                    if (f.currentPlanCubeGroup.GetCube(eCubeType.ChainPlanWeekDetail) == null ||
                        f.currentPlanCubeGroup.GetCube(eCubeType.ChainPlanPeriodDetail) == null ||
                        f.currentPlanCubeGroup.GetCube(eCubeType.ChainPlanDateTotal) == null)
                    {
                        buildGroup = true;
                    }
                }
                else
                {
                    buildGroup = true;
                }

                if (nodeProfile == null)
                {
                    nodeProfile = currentPlanOpenParms.ChainHLPlanProfile.NodeProfile;
                }

                if (versionProfile == null)
                {
                    versionProfile = currentPlanOpenParms.ChainHLPlanProfile.VersionProfile;
                }

                if (buildGroup)
                {
                    CubeGroupHashEntry cubeGroupHashEntry = new CubeGroupHashEntry(nodeProfile.Key, versionProfile.Key, varDateRangeProfile.Key);

                    if (!f.filterCubeGroupHash.Contains(cubeGroupHashEntry))
                    {
                        InitializeCubeGroupHashEntry(cubeGroupHashEntry, currentPlanOpenParms, f.cubeSAB, f.cubeTransaction, varDateRangeProfile);
                        f.filterCubeGroupHash.Add(cubeGroupHashEntry, cubeGroupHashEntry);
                    }
                    else
                    {
                        cubeGroupHashEntry = (CubeGroupHashEntry)f.filterCubeGroupHash[cubeGroupHashEntry];
                    }

                    cubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.NodeProfile = nodeProfile;
                    cubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.VersionProfile = versionProfile;
                    cubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile.SetReadOnly();


                    cubeGroupHashEntry.FilterOpenParms.AddChainCube(eCubeType.ChainPlanWeekDetail);
                    cubeGroupHashEntry.FilterOpenParms.AddChainCube(eCubeType.ChainPlanPeriodDetail);
                    cubeGroupHashEntry.FilterOpenParms.AddChainCube(eCubeType.ChainPlanDateTotal);


                    localPlanCubeGroup = cubeGroupHashEntry.FilterCubeGroup;
                    //open the local plan cube group
                    localPlanCubeGroup.OpenCubeGroup(cubeGroupHashEntry.FilterOpenParms);
                }
                else
                {
                    localPlanCubeGroup = f.currentPlanCubeGroup;
                }

            }
            else if (cumulatedFlags.isStorePlan)
            {
                bool buildGroup = false;


                if (storeCurrentCubeGroup && f.currentPlanCubeGroup != null)
                {

                    if (f.currentPlanCubeGroup.GetCube(eCubeType.ChainPlanWeekDetail) == null ||
                        f.currentPlanCubeGroup.GetCube(eCubeType.ChainPlanPeriodDetail) == null ||
                        f.currentPlanCubeGroup.GetCube(eCubeType.ChainPlanDateTotal) == null ||
                        f.currentPlanCubeGroup.GetCube(eCubeType.StorePlanWeekDetail) == null ||

                        f.currentPlanCubeGroup.GetCube(eCubeType.StorePlanPeriodDetail) == null ||
                        f.currentPlanCubeGroup.GetCube(eCubeType.StorePlanDateTotal) == null ||
                        f.currentPlanCubeGroup.GetCube(eCubeType.StorePlanStoreTotalWeekDetail) == null ||
                        f.currentPlanCubeGroup.GetCube(eCubeType.StorePlanStoreTotalPeriodDetail) == null ||
                        f.currentPlanCubeGroup.GetCube(eCubeType.StorePlanStoreTotalDateTotal) == null)
                    {
                        buildGroup = true;
                    }
                }
                else
                {
                    buildGroup = true;
                }

                if (nodeProfile == null)
                {
                    nodeProfile = currentPlanOpenParms.StoreHLPlanProfile.NodeProfile;
                }

                if (versionProfile == null)
                {
                    versionProfile = currentPlanOpenParms.StoreHLPlanProfile.VersionProfile;
                }

                if (buildGroup)
                {
                    CubeGroupHashEntry cubeGroupHashEntry = new CubeGroupHashEntry(nodeProfile.Key, versionProfile.Key, varDateRangeProfile.Key);

                    if (!f.filterCubeGroupHash.Contains(cubeGroupHashEntry))
                    {
                        InitializeCubeGroupHashEntry(cubeGroupHashEntry, currentPlanOpenParms, f.cubeSAB, f.cubeTransaction, varDateRangeProfile);
                        f.filterCubeGroupHash.Add(cubeGroupHashEntry, cubeGroupHashEntry);
                    }
                    else
                    {
                        cubeGroupHashEntry = (CubeGroupHashEntry)f.filterCubeGroupHash[cubeGroupHashEntry];
                    }


                    cubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.NodeProfile = nodeProfile;
                    cubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.VersionProfile = versionProfile;
                    cubeGroupHashEntry.FilterOpenParms.ChainHLPlanProfile.NodeProfile.ChainSecurityProfile.SetReadOnly();

                    cubeGroupHashEntry.FilterOpenParms.StoreHLPlanProfile.NodeProfile = nodeProfile;
                    cubeGroupHashEntry.FilterOpenParms.StoreHLPlanProfile.VersionProfile = versionProfile;
                    cubeGroupHashEntry.FilterOpenParms.StoreHLPlanProfile.NodeProfile.StoreSecurityProfile.SetReadOnly();


                    cubeGroupHashEntry.FilterOpenParms.AddChainCube(eCubeType.ChainPlanWeekDetail);
                    cubeGroupHashEntry.FilterOpenParms.AddChainCube(eCubeType.ChainPlanPeriodDetail);
                    cubeGroupHashEntry.FilterOpenParms.AddChainCube(eCubeType.ChainPlanDateTotal);
                    cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanWeekDetail);
                    cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanPeriodDetail);
                    cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanDateTotal);
                    cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanGroupTotalWeekDetail);
                    cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanGroupTotalPeriodDetail);
                    cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanGroupTotalDateTotal);
                    cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanStoreTotalWeekDetail);
                    cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanStoreTotalPeriodDetail);
                    cubeGroupHashEntry.FilterOpenParms.AddStoreCube(eCubeType.StorePlanStoreTotalDateTotal);

                    localPlanCubeGroup = cubeGroupHashEntry.FilterCubeGroup;
                    //open the local plan cube group
                    localPlanCubeGroup.OpenCubeGroup(cubeGroupHashEntry.FilterOpenParms);
                }
                else
                {
                    localPlanCubeGroup = f.currentPlanCubeGroup;
                }
            }
            return localPlanCubeGroup;
        }

        private static bool EvaluateCondition(
            ref string detailedComparison,
            double[] aFirstValues,
            double[] aSecondValues,
            variableTimeTypes aFirstTimeMod,
            variableTimeTypes aSecondTimeMod,
            filterCondition fc,
            filterNumericOperatorTypes numericOperatorOverrideForGrades = null //TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results         
            )
        {
            bool result;
            int i;

            try
            {
                result = false;

                for (i = 0; i < aFirstValues.Length; i++)
                {
                    result = EvaluateSecondValue(
                        ref detailedComparison,
                        aFirstValues,
                        aSecondValues,
                        aFirstTimeMod,
                        aSecondTimeMod,
                        fc,
                        i,
                        aFirstValues[i],
                        numericOperatorOverrideForGrades //TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results
                        );

                    if (result != (aFirstTimeMod == variableTimeTypes.All))
                    {
                        break;
                    }
                }

                return result;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private static bool EvaluateSecondValue(
            ref string detailedComparison,
            double[] aFirstValues,
            double[] aSecondValues,
            variableTimeTypes aFirstTimeMod,
            variableTimeTypes aSecondTimeMod,
            filterCondition fc,
            int aFirstIndex,
            double aFirstValue,
            filterNumericOperatorTypes numericOperatorOverrideForGrades = null //TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results
            )
        {
            bool result;
            int i;

            try
            {
                result = (aFirstTimeMod == variableTimeTypes.All);

                if (aSecondTimeMod == variableTimeTypes.Corresponding)
                {
                    if (aFirstIndex < aSecondValues.Length)
                    {
                        result = CompareValues(
                            ref detailedComparison,
                            fc,
                            aFirstValue,
                            aSecondValues[aFirstIndex], numericOperatorOverrideForGrades); //TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results
                    }
                }
                else
                {
                    for (i = 0; i < aSecondValues.Length; i++)
                    {
                        result = CompareValues(
                            ref detailedComparison,
                            fc,
                            aFirstValue,
                            aSecondValues[i], numericOperatorOverrideForGrades); //TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results

                        if (result != (aSecondTimeMod == variableTimeTypes.All))
                        {
                            break;
                        }
                    }
                }

                return result;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        private static bool CompareValues(ref string detailedComparison, filterCondition fc, double val1, double val2, filterNumericOperatorTypes numericOperatorOverrideForGrades = null) //TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results
		{
			try
			{
                //Begin TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results
                filterNumericOperatorTypes numericOp;
                if (numericOperatorOverrideForGrades == null)
                {
                    numericOp = filterNumericOperatorTypes.FromIndex(fc.operatorIndex);
                }
                else
                {
                    numericOp = numericOperatorOverrideForGrades;
                }
                //End TT#1343-MD -jsobek -Store Filter - "equals exactly" returns incorrect results

                if (numericOp == filterNumericOperatorTypes.DoesEqual)
                {
                    detailedComparison += val1.ToString() + " == " + val2.ToString() + System.Environment.NewLine;
                    return (val1 == val2);
                }
                else if (numericOp == filterNumericOperatorTypes.DoesNotEqual)
                {
                    detailedComparison += val1.ToString() + " != " + val2.ToString() + System.Environment.NewLine;
                    return (val1 != val2);
                }
                else if (numericOp == filterNumericOperatorTypes.GreaterThan)
                {
                    detailedComparison += val1.ToString() + " > " + val2.ToString() + System.Environment.NewLine;
                    return (val1 > val2);
                }
                else if (numericOp == filterNumericOperatorTypes.GreaterThanOrEqualTo)
                {
                    detailedComparison += val1.ToString() + " >= " + val2.ToString() + System.Environment.NewLine;
                    return (val1 >= val2);
                }
                else if (numericOp == filterNumericOperatorTypes.LessThan)
                {
                    detailedComparison += val1.ToString() + " < " + val2.ToString() + System.Environment.NewLine;
                    return (val1 < val2);
                }
                else if (numericOp == filterNumericOperatorTypes.LessThanOrEqualTo)
                {
                    detailedComparison += val1.ToString() + " <= " + val2.ToString() + System.Environment.NewLine;
                    return (val1 <= val2);
                }
                else if (numericOp == filterNumericOperatorTypes.Between)            
                {
                    //Begin TT#1508-MD -jsobek -Store Filters - Fix between operator for integer types
                    double val3;
                    if (fc.numericTypeIndex == filterNumericTypes.Integer)
                    {
                        val3 = (double)fc.valueToCompareInt2;
                    }
                    else
                    {
                        val3 = (double)fc.valueToCompareDouble2;
                    }
                    //End TT#1508-MD -jsobek -Store Filters - Fix between operator for integer types
                    detailedComparison += val1.ToString() + " >= " + val2.ToString() + " && " + val1.ToString() + " <= " + val3.ToString() + System.Environment.NewLine;
                    return (val1 >= val2 && val1 <= val3);
                }
                else
                {
                    // compare as does equal
                    detailedComparison += val1.ToString() + " == " + val2.ToString() + System.Environment.NewLine;
                    return (val1 == val2);
                }
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	
        private static ProfileList SetDateRangeProfileForVariable(SessionAddressBlock SAB, DateRangeProfile varDateRangeProfile, variableTimeTypes variableTimeType, StoreProfile aStoreProf) //, DateProfile aCurrentDateProf) TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
       {
            DateRangeProfile dateRangeProf;
            ProfileList detailDateProfList;

            try
            {
                //Begin TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
                //if (variableTimeType != variableTimeTypes.Join)
                //{
                    DateTime _nullDate = new DateTime(1, 1, 1);
                    if (varDateRangeProfile.RelativeTo == eDateRangeRelativeTo.StoreOpen && aStoreProf.SellingOpenDt != _nullDate)
                    {
                        dateRangeProf = SAB.ApplicationServerSession.Calendar.GetDateRange(varDateRangeProfile.Key, SAB.ApplicationServerSession.Calendar.GetDay(aStoreProf.SellingOpenDt));
                    }
                    else
                    {
                        dateRangeProf = varDateRangeProfile;
                    }

                    if (dateRangeProf.SelectedDateType == eCalendarDateType.Week)
                    {
                        detailDateProfList = SAB.ApplicationServerSession.Calendar.GetWeekRange(dateRangeProf, dateRangeProf.InternalAnchorDate);
                    }
                    else
                    {
                        detailDateProfList = SAB.ApplicationServerSession.Calendar.GetPeriodRange(dateRangeProf, dateRangeProf.InternalAnchorDate);
                    }
                //}
                //else
                //{
                //    detailDateProfList = new ProfileList(aCurrentDateProf.ProfileType);
                //    detailDateProfList.Add(aCurrentDateProf);
                //}
                //End TT#1501-MD -jsobek -Store Filter - Join option does not return the correct results.
                return detailDateProfList;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private static PlanCubeGroupWaferCubeFlags SetCubeFlagsForVariable(variableValueTypes varValueType)
        {
            PlanCubeGroupWaferCubeFlags cumulatedFlags = new PlanCubeGroupWaferCubeFlags();
        
            if (varValueType == variableValueTypes.StoreTotal || varValueType == variableValueTypes.StoreAverage)
            {
                cumulatedFlags.isStorePlan = true;
                cumulatedFlags.isStoreTotal = true;
            }
            else if (varValueType == variableValueTypes.ChainDetail)
            {
                cumulatedFlags.isChainPlan = true;
            }
            else
            {
                cumulatedFlags.isStorePlan = true;
                cumulatedFlags.isStore = true;
            }

            //if (variableOperand.GetType() == typeof(DataQueryTimeTotalVariableOperand))
            //{
            //    cumulatedFlags.isDate = true;
            //}
            return cumulatedFlags;
        }
        private static DateRangeProfile SetDateRangeProfileForVariable(SessionAddressBlock SAB, int dateRangeRID)
        {
            if (dateRangeRID == -1 || dateRangeRID == Include.UndefinedCalendarDateRange)
            {
                return null;
            }
            else
            {
                return SAB.ClientServerSession.Calendar.GetDateRange(dateRangeRID, SAB.ClientServerSession.Calendar.CurrentDate);
            }
        }
        private static HierarchyNodeProfile SetNodeProfileForVariable(SessionAddressBlock SAB, int hnRID)
        {
            if (hnRID == -1)
            {
                return null;
            }
            else
            {
                return SAB.HierarchyServerSession.GetNodeData(hnRID, false, true);
            }
        }

        private static VersionProfile SetVersionProfileForVariable(SessionAddressBlock SAB, int versionRID)
        {
            if (versionRID == -1)
            {
                return null;
            }
            else
            {
                ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
				return fvpb.Build(versionRID);
                //      ProfileList versionProfList = SAB.ClientServerSession.GetUserForecastVersions();
            }
        }


        private static IOrderedEnumerable<filterSortClass> SortProfileList(List<filterSortMapIndex> mapIndexes, List<filterSortClass> listToSort)
        {
            IOrderedEnumerable<filterSortClass> q = null;

            foreach (filterSortMapIndex sortMapIndex in mapIndexes)
            {
             
                switch (sortMapIndex.mapIndex)
                {
                    case 1:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortString1);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortString1);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortString1);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortString1);
                            }
                        }
                        break;
                    case 2:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortString2);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortString2);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortString2);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortString2);
                            }
                        }
                        break;
                    case 3:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortString3);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortString3);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortString3);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortString3);
                            }
                        }
                        break;
                    case 4:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortString4);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortString4);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortString4);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortString4);
                            }
                        }
                        break;
                    case 5:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortString5);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortString5);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortString5);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortString5);
                            }
                        }
                        break;
                    case 6:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortDate1);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortDate1);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortDate1);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortDate1);
                            }
                        }
                        break;
                    case 7:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortDate2);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortDate2);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortDate2);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortDate2);
                            }
                        }
                        break;
                    case 8:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortDate3);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortDate3);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortDate3);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortDate3);
                            }
                        }
                        break;
                    case 9:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortDate4);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortDate4);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortDate4);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortDate4);
                            }
                        }
                        break;
                    case 10:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortDate5);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortDate5);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortDate5);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortDate5);
                            }
                        }
                        break;
                    case 11:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortInt1);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortInt1);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortInt1);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortInt1);
                            }
                        }
                        break;
                    case 12:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortInt2);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortInt2);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortInt2);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortInt2);
                            }
                        }
                        break;
                    case 13:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortInt3);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortInt3);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortInt3);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortInt3);
                            }
                        }
                        break;
                    case 14:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortInt4);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortInt4);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortInt4);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortInt4);
                            }
                        }
                        break;
                    case 15:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortInt5);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortInt5);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortInt5);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortInt5);
                            }
                        }
                        break;
                    case 16:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortDouble1);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortDouble1);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortDouble1);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortDouble1);
                            }
                        }
                        break;
                    case 17:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortDouble2);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortDouble2);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortDouble2);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortDouble2);
                            }
                        }
                        break;
                    case 18:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortDouble3);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortDouble3);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortDouble3);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortDouble3);
                            }
                        }
                        break;
                    case 19:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortDouble4);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortDouble4);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortDouble4);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortDouble4);
                            }
                        }
                        break;
                    case 20:
                        if (sortMapIndex.ascending)
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderBy(x => x.sortDouble5);
                            }
                            else
                            {
                                q = q.ThenBy(x => x.sortDouble5);
                            }
                        }
                        else
                        {
                            if (q == null)
                            {
                                q = listToSort.OrderByDescending(x => x.sortDouble5);
                            }
                            else
                            {
                                q = q.ThenByDescending(x => x.sortDouble5);
                            }
                        }
                        break;
               }
            }

            return q;
        }
    }

    public class costGroup
    {
        public List<costClass> group = new List<costClass>();
        public int groupCost;
    }

    public class costClass //: IComparable<costClass>
    {
        public int cost;
        public int seq;
        public bool isOR;

        public int CompareTo(costClass other)
        {
            return cost.CompareTo(other.cost);
        }

    }
    public class filterSortClass
    {
        public int profileKey;

        public string sortString1;
        public string sortString2;
        public string sortString3;
        public string sortString4;
        public string sortString5;

        public DateTime sortDate1;
        public DateTime sortDate2;
        public DateTime sortDate3;
        public DateTime sortDate4;
        public DateTime sortDate5;

        public int sortInt1;
        public int sortInt2;
        public int sortInt3;
        public int sortInt4;
        public int sortInt5;

        public double sortDouble1;
        public double sortDouble2;
        public double sortDouble3;
        public double sortDouble4;
        public double sortDouble5;

        public void SetValue(int mapIndex, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string val1 = Convert.ToString(value);
                SetValue(mapIndex, val1);
            }
            else if (value.GetType() == typeof(DateTime))
            {
                DateTime val1 = Convert.ToDateTime(value);
                SetValue(mapIndex, val1);
            }
            else if (value.GetType() == typeof(int))
            {
                int val1 = Convert.ToInt32(value);
                SetValue(mapIndex, val1);
            }
            else if (value.GetType() == typeof(double))
            {
                double val1 = Convert.ToDouble(value);
                SetValue(mapIndex, val1);
            }
        }
        public void SetValue(int mapIndex, string value)
        {
            if (mapIndex == 1)
            {
                sortString1 = value;
            }
            else if (mapIndex == 2)
            {
                sortString2 = value;
            }
            else if (mapIndex == 3)
            {
                sortString3 = value;
            }
            else if (mapIndex == 4)
            {
                sortString4 = value;
            }
            else if (mapIndex == 5)
            {
                sortString5 = value;
            }
        }
        public void SetValue(int mapIndex, DateTime value)
        {
            if (mapIndex == 6)
            {
                sortDate1 = value;
            }
            else if (mapIndex == 7)
            {
                sortDate2 = value;
            }
            else if (mapIndex == 8)
            {
                sortDate3 = value;
            }
            else if (mapIndex == 9)
            {
                sortDate4 = value;
            }
            else if (mapIndex == 10)
            {
                sortDate5 = value;
            }
        }
        public void SetValue(int mapIndex, int value)
        {
            if (mapIndex == 11)
            {
                sortInt1 = value;
            }
            else if (mapIndex == 12)
            {
                sortInt2 = value;
            }
            else if (mapIndex == 13)
            {
                sortInt3 = value;
            }
            else if (mapIndex == 14)
            {
                sortInt4 = value;
            }
            else if (mapIndex == 15)
            {
                sortInt5 = value;
            }
        }
        public void SetValue(int mapIndex, double value)
        {
            if (mapIndex == 16)
            {
                sortDouble1 = value;
            }
            else if (mapIndex == 17)
            {
                sortDouble2 = value;
            }
            else if (mapIndex == 18)
            {
                sortDouble3 = value;
            }
            else if (mapIndex == 19)
            {
                sortDouble4 = value;
            }
            else if (mapIndex == 20)
            {
                sortDouble5 = value;
            }
        }
       
       
    }
    public class filterSortMapIndex
    {
        public int mapIndex;
        public bool ascending = true;
    }


     //private bool HeaderStyleOkay(UltraGridRow aRow)
     //   {
     //       bool styleOkay = true;
     //       try
     //       {
     //           if (_allocWorkFilterProfile.HnRID != Include.NoRID)
     //           {
     //               HierarchyNodeProfile filterHnp = _SAB.HierarchyServerSession.GetNodeData(_allocWorkFilterProfile.HnRID);
     //               if (filterHnp.NodeLevel == 0 && filterHnp.HomeHierarchyType == eHierarchyType.organizational)
     //               {
     //                   // do nothing; top main hierarchy node same as no node specified 
     //               }
     //               else
     //               {
     //                   int styleHnRID = Convert.ToInt32(aRow.Cells["StyleHnRID"].Value, CultureInfo.CurrentUICulture);
     //                   // Begin TT#1612 - RMatelic - Multi-header error 
     //                   //HierarchyNodeProfile styleHnp = _SAB.HierarchyServerSession.GetNodeData(styleHnRID); >>> not used
     //                   //NodeAncestorList nal = _SAB.HierarchyServerSession.GetNodeAncestorList(styleHnRID, filterHnp.HierarchyRID);
     //                   // Begin Modify TT#1612 after discovering a failed scenario - eliminate 'if...' condition and search all hierarchies
     //                   //NodeAncestorList nal;
     //                   //if (filterHnp.HomeHierarchyType == eHierarchyType.alternate)
     //                   //{
     //                   //    nal = _SAB.HierarchyServerSession.GetNodeAncestorList(styleHnRID, filterHnp.HierarchyRID, eHierarchySearchType.AlternateHierarchiesOnly);
     //                   //}
     //                   //else
     //                   //{
     //                   //    nal = _SAB.HierarchyServerSession.GetNodeAncestorList(styleHnRID, filterHnp.HierarchyRID, eHierarchySearchType.HomeHierarchyOnly);
     //                   //}
     //                   // End TT#1612
     //                   NodeAncestorList nal = _SAB.HierarchyServerSession.GetNodeAncestorList(styleHnRID, filterHnp.HierarchyRID, eHierarchySearchType.AllHierarchies);
     //                   // End Modify TT#1612
     //                   if (!nal.Contains(_allocWorkFilterProfile.HnRID))
     //                   {
     //                       styleOkay = false;
     //                   }
     //               }
     //           }     
     //       }
     //       catch
     //       {
     //           throw;
     //       }
     //       return styleOkay;
     //   }  
 

      //private bool HeaderDatesOkay(UltraGridRow aRow)
      //  {
      //      bool datesOkay = true;
      //      //Hashtable multiHeaderListHash;
      //      DateTime headerDate, releaseDate;
      //      string releaseDateStr;
      //      try
      //      {
      //          headerDate = Convert.ToDateTime(aRow.Cells["Date"].Value, CultureInfo.CurrentUICulture);

      //          releaseDateStr = Convert.ToString(aRow.Cells["Release"].Value, CultureInfo.CurrentUICulture);
      //          if (releaseDateStr == null || releaseDateStr == string.Empty)
      //          {
      //              releaseDate = Include.UndefinedDate;
      //          }
      //          else
      //          {
      //              releaseDate = Convert.ToDateTime(releaseDateStr, CultureInfo.CurrentUICulture);
      //          }

      //          switch (_allocWorkFilterProfile.HeaderDateType)
      //          {
      //              case eFilterDateType.all:
      //                  break;

      //              case eFilterDateType.today:
      //                  if (headerDate.Date != DateTime.Today.Date)
      //                  {
      //                      datesOkay = false;
      //                  }
      //                  break;

      //              case eFilterDateType.specify:
      //                  if (headerDate.Date < _allocWorkFilterProfile.HeaderDateFrom.Date ||
      //                      headerDate.Date > _allocWorkFilterProfile.HeaderDateTo.Date)
      //                  {
      //                      datesOkay = false;
      //                  }
      //                  break;

      //              case eFilterDateType.between:
      //                  if (headerDate.Date < DateTime.Today.Date.Add(new TimeSpan(_allocWorkFilterProfile.HeaderDateBetweenFrom, 0, 0, 0, 0)) ||
      //                      headerDate.Date > DateTime.Today.Date.Add(new TimeSpan(_allocWorkFilterProfile.HeaderDateBetweenTo, 0, 0, 0, 0)))
      //                  {
      //                      datesOkay = false;
      //                  }
      //                  break;
      //          }

      //          if (datesOkay && releaseDate != Include.UndefinedDate)
      //          {
      //              switch (_allocWorkFilterProfile.ReleaseDateType)
      //              {
      //                  case eFilterDateType.all:
      //                      break;

      //                  case eFilterDateType.today:
      //                      if (releaseDate.Date != DateTime.Today.Date)
      //                      {
      //                          datesOkay = false;
      //                      }
      //                      break;

      //                  case eFilterDateType.specify:
      //                      if (releaseDate.Date < _allocWorkFilterProfile.ReleaseDateFrom.Date ||
      //                          releaseDate.Date > _allocWorkFilterProfile.ReleaseDateTo.Date)
      //                      {
      //                          datesOkay = false;
      //                      }
      //                      break;

      //                  case eFilterDateType.between:
      //                      if (releaseDate.Date < DateTime.Today.Date.Add(new TimeSpan(_allocWorkFilterProfile.ReleaseDateBetweenFrom, 0, 0, 0, 0)) ||
      //                          releaseDate.Date > DateTime.Today.Date.Add(new TimeSpan(_allocWorkFilterProfile.ReleaseDateBetweenTo, 0, 0, 0, 0)))
      //                      {
      //                          datesOkay = false;
      //                      }
      //                      break;
      //              }
      //          }
      //      }
      //      catch
      //      {
      //          throw;
      //      }
      //      return datesOkay;
      //  }

    
}
