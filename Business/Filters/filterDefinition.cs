using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    public delegate DataTable FilterLoadListDelegate();
    public delegate DataTable FilterLoadValueListFromFieldDelegate(int fieldIndex);
    public delegate string FilterGetNameFromFieldIndexDelegate(int fieldIndex);
    public delegate filterDataTypes FilterGetDataTypeFromFieldIndexDelegate(int fieldIndex);
    public delegate void FilterMakeElementInGroupDelegate(string key, elementBase eb, bool useValueListFromField, int tempFieldIndex);
    public delegate void FilterRemoveDynamicElementsForFieldDelegate(List<string> keyListToRemove);

    public class filter : MIDRetail.Common.Filter
    {
        private DataTable dtConditions;
        public int filterRID;
        public filterTypes filterType;
        public string filterName;
        public int userRID;
        public int ownerUserRID;
        public bool isLimited;
        public long resultLimit;

        override public ProfileList ApplyFilter(ProfileList aProfileList)
        {
            return filterEngine.RunFilter(this, aProfileList);
        }
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.Store;
            }
        }

        public filter(int userRID, int ownerUserRID)
        {
            filterName = "New Filter";  
            filterRID = -1;
            this.userRID = userRID;
            this.ownerUserRID = ownerUserRID;

            isLimited = false;

            dtConditions = filterCondition.GetConditionDataTable();

            RootConditionNode = new ConditionNode();
            RootConditionNode.condition = new filterCondition();
            RootConditionNode.ConditionNodes = new List<ConditionNode>();
            RootConditionNode.condition.Seq = -1;

       
        }

        public SessionAddressBlock cubeSAB; //needed to get calendar date ranges
        public ApplicationSessionTransaction cubeTransaction; //needed to create new cube groups
        public PlanCubeGroup currentPlanCubeGroup; //needed for variable comparison in cubes
        public Hashtable filterCubeGroupHash; //needed for variable comparison in cubes
        public void SetExtraInfoForCubes(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, PlanCubeGroup aCurrentPlanCubeGroup)
        {
            cubeSAB = aSAB;
            cubeTransaction = aTransaction;
            currentPlanCubeGroup = aCurrentPlanCubeGroup;
            filterCubeGroupHash = new Hashtable();
            filterDataHelper.SAB = aSAB;
            filterDataHelper.SetVariableKeysFromTransaction(aTransaction);

        }

       

        private DataTable dtStoreCharacteristicValues = null;
        public void GetStoreCharacteristicValues(int scgRID)
        {
            bool hasValuesForThisCharacteristic = false;
            if (dtStoreCharacteristicValues != null)
            {
                DataRow[] drFind = dtStoreCharacteristicValues.Select("SCG_RID=" + scgRID.ToString());
                if (drFind.Length > 0)
                {
                    hasValuesForThisCharacteristic = true;
                }
            }


            if (hasValuesForThisCharacteristic == false)
            {
                FilterData fd = new FilterData();
                dtStoreCharacteristicValues = fd.StoreCharacteristicsGetValuesForFilter(scgRID);
            }
        }
        public object GetCharacteristicValueForThisStore(int scgRID, int storeRID)
        {
            object obj = null;
            DataRow[] drFind = dtStoreCharacteristicValues.Select("SCG_RID=" + scgRID.ToString() + " AND ST_RID=" + storeRID.ToString());
            if (drFind.Length > 0)
            {
                if (drFind[0]["CHAR_VALUE"] != DBNull.Value)
                {
                    obj = drFind[0]["CHAR_VALUE"];
                }
            }
            return obj;
        }
        public int GetCharacteristicValueRidForThisStore(int scgRID, int storeRID)
        {
            int scRID = -1;
            DataRow[] drFind = dtStoreCharacteristicValues.Select("SCG_RID=" + scgRID.ToString() + " AND ST_RID=" + storeRID.ToString());
            if (drFind.Length > 0)
            {
                if (drFind[0]["SC_RID"] != DBNull.Value)
                {
                    scRID = (int)drFind[0]["SC_RID"];
                }
            }
            return scRID;
        }

        //public static DataTable GetFilterDataTable()
        //{
        //    DataTable dtFilters = new DataTable("filters");
        //    dtFilters.Columns.Add("FILTER_NAME");
        //    dtFilters.Columns.Add("FILTER_RID", typeof(int));
        //    dtFilters.Columns.Add("USER_RID", typeof(int));
        //    dtFilters.Columns.Add("OWNER_USER_RID", typeof(int));
        //    dtFilters.Columns.Add("FILTER_TYPE", typeof(int));
        //    dtFilters.Columns.Add("FILTER_TYPE_ID");
        //    //dtFilters.Columns.Add("IS_TEMPLATE", typeof(int));
        //    dtFilters.Columns.Add("IS_LIMITED", typeof(int));
        //    dtFilters.Columns.Add("RESULT_LIMIT", typeof(long));
        //    return dtFilters;
        //}
        public void LoadFromDataRow(DataRow drFilter)
        {
            filterRID = (int)drFilter["FILTER_RID"];
            userRID = (int)drFilter["USER_RID"];
            ownerUserRID = (int)drFilter["OWNER_USER_RID"];
            //int GlobalUserRID = Include.GlobalUserRID; 
            //if (userRID == GlobalUserRID)
            //{
            //    filterFolderUserOrGlobal = "Global";
            //}
            //else
            //{
            //    filterFolderUserOrGlobal = "User";
            //}
            int dbIndex = (int)drFilter["FILTER_TYPE"];
            filterType = filterTypes.FromIndex(dbIndex);
            filterName = (string)drFilter["FILTER_NAME"];
            //if ((int)drFilter["IS_TEMPLATE"] == 1)
            //{
            //    isTemplate = true;
            //}
            //else
            //{
            //    isTemplate = false;
            //}
            //if ((int)drFilter["IS_LIMITED"] == 1)
            //{
            //    isLimited = true;
            //}
            //else
            //{
            //    isLimited = false;
            //}
            isLimited = (bool)drFilter["IS_LIMITED"];
            resultLimit = (int)drFilter["RESULT_LIMIT"];
        }
        public void SaveToDataRow(ref DataRow drFilter)
        {
            drFilter["FILTER_RID"] = filterRID;
            drFilter["USER_RID"] = userRID;
            drFilter["OWNER_USER_RID"] = ownerUserRID;

            drFilter["FILTER_TYPE"] = filterType.dbIndex;
            drFilter["FILTER_TYPE_ID"] = filterType.Name;
            drFilter["FILTER_NAME"] = filterName;
            //if (isTemplate)
            //{
            //    drFilter["IS_TEMPLATE"] = 1;
            //}
            //else
            //{
            //    drFilter["IS_TEMPLATE"] = 0;
            //}
            if (isLimited)
            {
                drFilter["IS_LIMITED"] = 1;
            }
            else
            {
                drFilter["IS_LIMITED"] = 0;
            }
            drFilter["RESULT_LIMIT"] = resultLimit;
        }
        public void LoadConditionsFromDataTable(DataTable dt)
        {
            //foreach (DataRow drCondition in drConditions)
            //{
            //    DataRow dr = dtConditions.NewRow();
            //    filterUtility.DataRowCopy(drCondition, dr);
            //    dtConditions.Rows.Add(dr);

            //    filterCondition fc = new filterCondition();
            //    fc.LoadFromDataRow(drCondition);
            //    BuildConditionNode(fc);
            //}
            //this.dtConditions = dtConditions;

            DataRow[] drRoots = dt.Select("PARENT_SEQ=-1", "SIBLING_SEQ");
            foreach (DataRow drCondition in drRoots)
            {
                LoadCondition(drCondition, dt);
            }

        }
        private void LoadCondition(DataRow drCondition, DataTable dt)
        {
            DataRow dr = this.dtConditions.NewRow();
            filterUtility.DataRowCopy(drCondition, dr);
            this.dtConditions.Rows.Add(dr);

            filterCondition fc = new filterCondition();
            fc.LoadFromDataRow(drCondition);
            BuildConditionNode(fc);
            //Load the children
            DataRow[] drChildren = dt.Select("PARENT_SEQ=" + fc.Seq.ToString(), "SIBLING_SEQ");
            foreach (DataRow drChild in drChildren)
            {
                LoadCondition(drChild, dt);
            }
        }

        /// <summary>
        /// Creates an instance of the entry based on the type provided in the dictionary
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public filterDictionaryEntry GetEntryFromCondition(filterManager manager, filterCondition condition)
        {
            filterDictionaryEntry entry;
            filterDictionary dictionary = filterDictionary.FromIndex(condition.dictionaryIndex);
            object[] args = new object[1];
            args[0] = manager;
            object obj = Activator.CreateInstance(dictionary.entryType, args);
            entry = (filterDictionaryEntry)obj;
            return entry;
        }
        public filterDictionaryEntry GetEntryFromDictionary(filterManager manager, filterDictionary dictionary)
        {
            filterDictionaryEntry entry;      
            object[] args = new object[1];
            args[0] = manager;
            object obj = Activator.CreateInstance(dictionary.entryType, args);
            entry = (filterDictionaryEntry)obj;
            return entry;
        }


        public int GetNextConditionSeq()
        {
            int maxSeq = 0;
            foreach (DataRow dr in dtConditions.Rows)
            {
                int drSeq = (int)dr["SEQ"];
                if (drSeq > maxSeq)
                {
                    maxSeq = drSeq;
                }
            }
            //return (dtConditions.Rows.Count + 1);
            return (maxSeq + 1);
        }
        public DataRowCollection GetConditionDataRows()
        {
            return dtConditions.Rows;
        }


        public int GetNameConditionSeq()
        {
            //Begin TT#1468-MD -jsobek -Header Filter Sort Options
            //if (filterType != filterTypes.HeaderFilter && filterType != filterTypes.AssortmentFilter && filterType != filterTypes.AttributeSetFilter)
            //{
            //    return 4;
            //}
            //else
            //{
            //    return 3;
            //}

            ConditionNode cn = FindConditionNodeByType(filterDictionary.FilterName);
            if (cn == null)
            {
                return -1;
            }
            else
            {
                return cn.condition.Seq;
            }
            //End TT#1468-MD -jsobek -Header Filter Sort Options
        }

        


        public int GetSortByConditionSeq()
        {
            //Begin TT#1468-MD -jsobek -Header Filter Sort Options
            //if (filterType != filterTypes.HeaderFilter && filterType != filterTypes.AssortmentFilter && filterType != filterTypes.AttributeSetFilter)
            //{
            //    return 3;
            //}
            //else
            //{
            //    return -1;
            //}

            ConditionNode cn = FindConditionNodeByType(filterDictionary.InfoSortBy);
            if (cn == null)
            {
                return -1;
            }
            else
            {
                return cn.condition.Seq;
            }
            //End TT#1468-MD -jsobek -Header Filter Sort Options
        }
        public int GetRootConditionSeq()
        {
                return 2;  
        }

        public void AddInitialConditions(filterManager manager)
        {
            //Add Filter Info
            filterCondition filterInfo = new filterCondition();

            filterInfo.dictionaryIndex = filterDictionary.InfoFilter.dbIndex; // "InfoFilter";
            //filterInfo.navigationType = navigationTypes.Info;
            filterInfo.conditionFilterRID = filterRID;
            filterInfo.Seq = GetNextConditionSeq();
            filterInfo.ParentSeq = -1;
            filterInfo.SiblingSeq = GetNextSiblingSeq(filterInfo.ParentSeq);

            filterDictionaryEntry eg = GetEntryFromCondition(manager, filterInfo);
            eg.BuildFormattedText(manager.Options, ref filterInfo);

            DataRow drNewCondtion = dtConditions.NewRow();
            filterInfo.SaveToDataRow(ref drNewCondtion);
            dtConditions.Rows.Add(drNewCondtion);
            BuildConditionNode(filterInfo);




            //Add Condition Parent
            filterCondition conditionParent = new filterCondition();
            conditionParent.dictionaryIndex = filterDictionary.InfoConditions.dbIndex; //"InfoConditions";
            //conditionParent.navigationType = navigationTypes.Info;
            conditionParent.conditionFilterRID = filterRID;
            conditionParent.Seq = dtConditions.Rows.Count + 1;
            conditionParent.ParentSeq = -1;
            conditionParent.SiblingSeq = GetNextSiblingSeq(conditionParent.ParentSeq);

            filterDictionaryEntry eg2 = GetEntryFromCondition(manager, conditionParent);
            eg2.BuildFormattedText(manager.Options, ref conditionParent);

            DataRow drNewCondtion2 = dtConditions.NewRow();
            conditionParent.SaveToDataRow(ref drNewCondtion2);
            dtConditions.Rows.Add(drNewCondtion2);
            BuildConditionNode(conditionParent); ;

            if (filterType != filterTypes.StoreGroupFilter && filterType != filterTypes.StoreGroupDynamicFilter) //TT#1313-MD -jsobek -Header Filters
            {
                //Add SortBy Parent
                filterCondition sortByParent = new filterCondition();
                sortByParent.dictionaryIndex = filterDictionary.InfoSortBy.dbIndex; //"InfoSortBy";
                //sortByParent.navigationType = navigationTypes.Info;
                sortByParent.conditionFilterRID = filterRID;
                sortByParent.Seq = dtConditions.Rows.Count + 1;
                sortByParent.ParentSeq = -1;
                sortByParent.SiblingSeq = GetNextSiblingSeq(sortByParent.ParentSeq);
                
                filterDictionaryEntry eg3 = GetEntryFromCondition(manager, sortByParent);
                eg3.BuildFormattedText(manager.Options, ref sortByParent);

                DataRow drNewCondtion3 = dtConditions.NewRow();
                sortByParent.SaveToDataRow(ref drNewCondtion3);
                dtConditions.Rows.Add(drNewCondtion3);
                BuildConditionNode(sortByParent);

            }
        

            //Add Filter Name
            filterCondition fcFilterName = new filterCondition();
            fcFilterName.dictionaryIndex = filterDictionary.FilterName.dbIndex; //"FilterName";
            //fcFilterName.navigationType = navigationTypes.Info;
            fcFilterName.conditionFilterRID = filterRID;
            fcFilterName.Seq = dtConditions.Rows.Count + 1;
            fcFilterName.valueToCompare = filterName;

            filterDictionaryEntry egFilterName = GetEntryFromCondition(manager, fcFilterName);
            egFilterName.SetDefaults(ref fcFilterName);
            egFilterName.BuildFormattedText(manager.Options, ref fcFilterName);


            fcFilterName.ParentSeq = 1;
            fcFilterName.SiblingSeq = 1;
            DataRow drNewCondtion4 = dtConditions.NewRow();
            fcFilterName.SaveToDataRow(ref drNewCondtion4);
            dtConditions.Rows.Add(drNewCondtion4);
            BuildConditionNode(fcFilterName);

            //Add Folder
            // Begin TT#4628 - JSmith - Merchandise Explorer-> Search Filter-> throws unhandled exception error
            if (filterType != filterTypes.ProductFilter)
            {
            // End TT#4628 - JSmith - Merchandise Explorer-> Search Filter-> throws unhandled exception error
                filterCondition fcFolder = new filterCondition();
                fcFolder.dictionaryIndex = filterDictionary.FilterFolder.dbIndex; //"FilterFolder";
                //fcFolder.navigationType = navigationTypes.Info;
                fcFolder.conditionFilterRID = filterRID;
                fcFolder.Seq = dtConditions.Rows.Count + 1;
                fcFolder.valueToCompareInt = manager.currentFilter.ownerUserRID;
                //if (manager.currentFilter.userRID == Include.GlobalUserRID)
                //{
                //    fcFolder.valueToCompareInt = Include.GlobalUserRID;
                //}
                //else
                //{
                //    fcFolder.valueToCompareInt = -1;
                //}


                filterDictionaryEntry egFolder = GetEntryFromCondition(manager, fcFolder);
                egFolder.SetDefaults(ref fcFolder);
                egFolder.BuildFormattedText(manager.Options, ref fcFolder);


                fcFolder.ParentSeq = 1;
                fcFolder.SiblingSeq = 2;
                DataRow drNewCondtion5 = dtConditions.NewRow();
                fcFolder.SaveToDataRow(ref drNewCondtion5);
                dtConditions.Rows.Add(drNewCondtion5);
                BuildConditionNode(fcFolder);
            // Begin TT#4628 - JSmith - Merchandise Explorer-> Search Filter-> throws unhandled exception error
            }
            // End TT#4628 - JSmith - Merchandise Explorer-> Search Filter-> throws unhandled exception error

            //Begin TT#1414-MD -jsobek -Attribute Set Filter
            if (filterType != filterTypes.StoreGroupFilter && filterType != filterTypes.StoreGroupDynamicFilter) 
            {
                //Add Limit
                filterCondition fcLimit = new filterCondition();
                fcLimit.dictionaryIndex = filterDictionary.ResultLimit.dbIndex; //"FilterLimit";
                //fcLimit.navigationType = navigationTypes.Info;
                fcLimit.conditionFilterRID = filterRID;
                fcLimit.Seq = dtConditions.Rows.Count + 1;
                fcLimit.valueToCompare = "Unrestricted";

                filterDictionaryEntry egLimit = GetEntryFromCondition(manager, fcLimit);
                egLimit.SetDefaults(ref fcLimit);
                egLimit.BuildFormattedText(manager.Options, ref fcLimit);

                fcLimit.ParentSeq = 1;
                fcLimit.SiblingSeq = 3;
                DataRow drNewCondtion6 = dtConditions.NewRow();
                fcLimit.SaveToDataRow(ref drNewCondtion6);
                dtConditions.Rows.Add(drNewCondtion6);
                BuildConditionNode(fcLimit);
            }


            if (filterType == filterTypes.StoreGroupFilter || filterType == filterTypes.StoreGroupDynamicFilter) 
            {
                if (filterType == filterTypes.StoreGroupFilter)
                {
                    filterCondition fcFirstSet = new filterCondition();
                    fcFirstSet.dictionaryIndex = filterDictionary.StoreGroupName;
                    fcFirstSet.conditionFilterRID = filterRID;
                    fcFirstSet.Seq = dtConditions.Rows.Count + 1;
                    //fcFirstSet.valueToCompare = "Unrestricted";

                    filterDictionaryEntry egFirstSet = GetEntryFromCondition(manager, fcFirstSet);
                    egFirstSet.SetDefaults(ref fcFirstSet);
                    egFirstSet.BuildFormattedText(manager.Options, ref fcFirstSet);

                    fcFirstSet.ParentSeq = 2;
                    fcFirstSet.SiblingSeq = 1;
                    DataRow drNewCondtion7 = dtConditions.NewRow();
                    fcFirstSet.SaveToDataRow(ref drNewCondtion7);
                    dtConditions.Rows.Add(drNewCondtion7);
                    BuildConditionNode(fcFirstSet);
                }
                else if (filterType == filterTypes.StoreGroupDynamicFilter)
                {
                    filterCondition fcFirstDynamicSet = new filterCondition();
                    fcFirstDynamicSet.dictionaryIndex = filterDictionary.StoreGroupDynamic;
                    fcFirstDynamicSet.conditionFilterRID = filterRID;
                    fcFirstDynamicSet.Seq = dtConditions.Rows.Count + 1;
                    //fcFirstSet.valueToCompare = "Unrestricted";

                    filterDictionaryEntry egFirstSet = GetEntryFromCondition(manager, fcFirstDynamicSet);
                    egFirstSet.SetDefaults(ref fcFirstDynamicSet);
                    egFirstSet.BuildFormattedText(manager.Options, ref fcFirstDynamicSet);

                    fcFirstDynamicSet.ParentSeq = 2;
                    fcFirstDynamicSet.SiblingSeq = 1;
                    DataRow drNewCondtion7 = dtConditions.NewRow();
                    fcFirstDynamicSet.SaveToDataRow(ref drNewCondtion7);
                    dtConditions.Rows.Add(drNewCondtion7);
                    BuildConditionNode(fcFirstDynamicSet);
                }

                //Add available store condition
                filterCondition fcAvailableStores = new filterCondition();
                fcAvailableStores.dictionaryIndex = filterDictionary.StoreGroupExclusionList;
                fcAvailableStores.conditionFilterRID = filterRID;
                fcAvailableStores.Seq = dtConditions.Rows.Count + 1;
                //fcFirstSet.valueToCompare = "Unrestricted";

                filterDictionaryEntry egAvailableStores = GetEntryFromCondition(manager, fcAvailableStores);
                egAvailableStores.SetDefaults(ref fcAvailableStores);
                egAvailableStores.BuildFormattedText(manager.Options, ref fcAvailableStores);

                fcAvailableStores.ParentSeq = 2;
                fcAvailableStores.SiblingSeq = 2;
                DataRow drNewCondtion8 = dtConditions.NewRow();
                fcAvailableStores.SaveToDataRow(ref drNewCondtion8);
                dtConditions.Rows.Add(drNewCondtion8);
                BuildConditionNode(fcAvailableStores);
            }
            //End TT#1414-MD -jsobek -Attribute Set Filter
        }

        public ConditionNode InsertCondition(ConditionNode selectedConditionNode, filterCondition fc)
        {
            bool doInsert = false;
            filterNavigationTypes selectedNavType = filterDictionary.FromIndex(selectedConditionNode.condition.dictionaryIndex).navigationType;
            filterNavigationTypes conditionNavType = filterDictionary.FromIndex(fc.dictionaryIndex).navigationType;
            if (conditionNavType == filterNavigationTypes.SortBy)
            {
                fc.ParentSeq = GetSortByConditionSeq(); // the default Parent Seq for sort bys
                fc.SiblingSeq = GetNextSiblingSeq(fc.ParentSeq);
            }
            else
            {
                if (this.filterType == filterTypes.StoreGroupFilter || this.filterType == filterTypes.StoreGroupDynamicFilter) //TT#1414-MD -jsobek -Attribute Set Filter
                {
                    //Special handling for inserting conditions on a store group


                    if (this.filterType == filterTypes.StoreGroupFilter && fc.dictionaryIndex == filterDictionary.StoreGroupName)
                    {
                        //Store Groups always get added to the top level
                        fc.ParentSeq = 2; // the default Parent Seq for conditions
 
                        ConditionNode availableStoresNode = this.FindConditionNodeByType(filterDictionary.StoreGroupExclusionList);
                        int nextSiblingSeq = GetNextSiblingSeq(fc.ParentSeq);
                        availableStoresNode.condition.SiblingSeq = nextSiblingSeq;
                        fc.SiblingSeq = nextSiblingSeq - 1;

                        this.UpdateCondition(availableStoresNode.condition);
                        doInsert = true;
                    }
                    else if (this.filterType == filterTypes.StoreGroupDynamicFilter && fc.dictionaryIndex == filterDictionary.StoreGroupDynamic)
                    {
                        //Store Groups always get added to the top level
                        fc.ParentSeq = 2; // the default Parent Seq for conditions

                        ConditionNode availableStoresNode = this.FindConditionNodeByType(filterDictionary.StoreGroupExclusionList);
                        int nextSiblingSeq = GetNextSiblingSeq(fc.ParentSeq);
                        availableStoresNode.condition.SiblingSeq = nextSiblingSeq;
                        fc.SiblingSeq = nextSiblingSeq - 1;

                        this.UpdateCondition(availableStoresNode.condition);
                        doInsert = true;
                    }
                    else
                    {

                        bool useSelectedCondition;
                        if (selectedConditionNode != null && selectedNavType == filterNavigationTypes.Condition)
                        {
                            useSelectedCondition = true;
                        }
                        else
                        {
                            useSelectedCondition = false;
                        }

                        //Do not allow conditions to go into the Available Stores group
                        if (selectedConditionNode.condition.dictionaryIndex == filterDictionary.StoreGroupExclusionList.dbIndex)
                        {
                            useSelectedCondition = false;
                        }


                        if (useSelectedCondition)
                        {
                            if (selectedConditionNode.condition.ParentSeq == this.GetRootConditionSeq())
                            {
                                fc.ParentSeq = selectedConditionNode.condition.Seq;
                                fc.SiblingSeq = GetNextSiblingSeq(fc.ParentSeq);
                            }
                            else
                            {
                                fc.ParentSeq = selectedConditionNode.condition.ParentSeq;
                                fc.SiblingSeq = GetNextSiblingSeq(fc.ParentSeq);
                            }
                        }
                        else
                        {
                            //use the first group under the root condition node
                            ConditionNode cnRootCondition = this.FindConditionNode(this.GetRootConditionSeq()); //parent seq for conditions  
                            fc.ParentSeq = cnRootCondition.ConditionNodes[0].condition.Seq;
                            fc.SiblingSeq = GetNextSiblingSeq(fc.ParentSeq);
                        }
                    }
                }
                else
                {
                    if (selectedConditionNode != null && selectedNavType == filterNavigationTypes.Condition)
                    {
                        fc.ParentSeq = selectedConditionNode.condition.ParentSeq;
                        fc.SiblingSeq = GetNextSiblingSeq(selectedConditionNode.condition.ParentSeq);
                    }
                    else
                    {
                        fc.ParentSeq = 2; // the default Parent Seq for conditions
                        fc.SiblingSeq = GetNextSiblingSeq(fc.ParentSeq);
                    }
                }
            }



            DataRow drNewCondtion = dtConditions.NewRow();
            fc.SaveToDataRow(ref drNewCondtion);
            dtConditions.Rows.Add(drNewCondtion);

            return BuildConditionNode(fc, doInsert);
        }
        public ConditionNode BuildConditionNode(filterCondition fc, bool doInsert = false)
        {
            ConditionNode n = new ConditionNode();
            n.ConditionNodes = new List<ConditionNode>();
            n.condition = fc;


            if (fc.ParentSeq == -1)
            {
                //n.NodeLevel = 1; //root level starts at one
                // n.IsRootLevel = true;
                //just add to root
                //ConditionNodes.Add(n);
                n.Parent = RootConditionNode;
                RootConditionNode.ConditionNodes.Add(n);
            }
            else
            {
                ConditionNode parent = FindConditionNode(fc.ParentSeq);
                //if (parent.ConditionNodes == null)
                //{
                //    parent.ConditionNodes = new List<ConditionNode>();
                //}
                //n.NodeLevel = parent.NodeLevel + 1;
                n.Parent = parent;

                if (doInsert)
                {
                    parent.ConditionNodes.Insert(fc.SiblingSeq -1, n);
                }
                else
                {
                    parent.ConditionNodes.Add(n);
                }
            }
            return n;
        }
        public void UpdateCondition(filterCondition fc)
        {
            int seq = fc.Seq;
            DataRow[] drfind = this.dtConditions.Select("SEQ = " + seq.ToString());
            fc.SaveToDataRow(ref drfind[0]);
        }

        // public List<ConditionNode> ConditionNodes = new List<ConditionNode>();
        public ConditionNode RootConditionNode;

        public ConditionNode FindConditionNode(int seq)
        {
            ConditionNode nFound = null;
            bool isFound = false;
            foreach (ConditionNode n in RootConditionNode.ConditionNodes)
            {
                if (isFound == false)
                {
                    FindNode(n, seq, ref isFound, ref nFound);
                }
            }
            return nFound;
        }
        private void FindNode(ConditionNode n, int seq, ref bool isFound, ref ConditionNode foundNode)
        {
            if (n.condition.Seq == seq)
            {
                isFound = true;
                foundNode = n;
            }
            else
            {
                foreach (ConditionNode c in n.ConditionNodes)
                {
                    if (isFound == false)
                    {
                        FindNode(c, seq, ref isFound, ref foundNode);
                    }
                }
            }

        }

        //Begin TT#1468-MD -jsobek -Header Filter Sort Options
        public ConditionNode FindConditionNodeByType(filterDictionary dictionaryType)
        {
            ConditionNode nFound = null;
            bool isFound = false;
            foreach (ConditionNode n in RootConditionNode.ConditionNodes)
            {
                if (isFound == false)
                {
                    FindNodeByType(n, dictionaryType, ref isFound, ref nFound);
                }
            }
            return nFound;
        }
        private void FindNodeByType(ConditionNode n, filterDictionary dictionaryType, ref bool isFound, ref ConditionNode foundNode)
        {
            if (n.condition.dictionaryIndex == dictionaryType)
            {
                isFound = true;
                foundNode = n;
            }
            else
            {
                foreach (ConditionNode c in n.ConditionNodes)
                {
                    if (isFound == false)
                    {
                        FindNodeByType(c, dictionaryType, ref isFound, ref foundNode);
                    }
                }
            }

        }
        //End TT#1468-MD -jsobek -Header Filter Sort Options

        //Begin TT#1517-MD -jsobek -Store Service Optimization
        public ConditionNode FindConditionNodeByRid(int conditionRidToFind)
        {
            ConditionNode nFound = null;
            bool isFound = false;
            foreach (ConditionNode n in RootConditionNode.ConditionNodes)
            {
                if (isFound == false)
                {
                    FindNodeByRid(n, conditionRidToFind, ref isFound, ref nFound);
                }
            }
            return nFound;
        }
        private void FindNodeByRid(ConditionNode n, int conditionRidToFind, ref bool isFound, ref ConditionNode foundNode)
        {
            if (n.condition.conditionRID == conditionRidToFind)
            {
                isFound = true;
                foundNode = n;
            }
            else
            {
                foreach (ConditionNode c in n.ConditionNodes)
                {
                    if (isFound == false)
                    {
                        FindNodeByRid(c, conditionRidToFind, ref isFound, ref foundNode);
                    }
                }
            }

        }
        public ConditionNode FindConditionNodeByLevelRID(int levelRidToFind)
        {
            ConditionNode nFound = null;
            bool isFound = false;
            foreach (ConditionNode n in RootConditionNode.ConditionNodes)
            {
                if (isFound == false)
                {
                    FindNodeByLevelRID(n, levelRidToFind, ref isFound, ref nFound);
                }
            }
            return nFound;
        }
        private void FindNodeByLevelRID(ConditionNode n, int levelRidToFind, ref bool isFound, ref ConditionNode foundNode)
        {
            if ((n.condition.dictionaryIndex == filterDictionary.StoreGroupName || n.condition.dictionaryIndex == filterDictionary.StoreGroupDynamic) && n.condition.fieldIndex == levelRidToFind)
            {
                isFound = true;
                foundNode = n;
            }
            else
            {
                foreach (ConditionNode c in n.ConditionNodes)
                {
                    if (isFound == false)
                    {
                        FindNodeByLevelRID(c, levelRidToFind, ref isFound, ref foundNode);
                    }
                }
            }

        }

        public ConditionNode FindSetConditionNodeByName(string levelNameToFind)
        {
            ConditionNode nFound = null;
            bool isFound = false;
            foreach (ConditionNode n in RootConditionNode.ConditionNodes)
            {
                if (isFound == false)
                {
                    FindSetNodeByName(n, levelNameToFind, ref isFound, ref nFound);
                }
            }
            return nFound;
        }
        private void FindSetNodeByName(ConditionNode n, string levelNameToFind, ref bool isFound, ref ConditionNode foundNode)
        {
            if ((n.condition.dictionaryIndex == filterDictionary.StoreGroupName || n.condition.dictionaryIndex == filterDictionary.StoreGroupDynamic) && n.condition.valueToCompare == levelNameToFind)
            {
                isFound = true;
                foundNode = n;
            }
            else
            {
                foreach (ConditionNode c in n.ConditionNodes)
                {
                    if (isFound == false)
                    {
                        FindSetNodeByName(c, levelNameToFind, ref isFound, ref foundNode);
                    }
                }
            }

        }


        public ConditionNode FindGroupConditionNodeBySeq(int seq)
        {
            ConditionNode nFound = null;
            bool isFound = false;
            int findCount = 0;
            foreach (ConditionNode n in RootConditionNode.ConditionNodes)
            {
                if (isFound == false)
                {
                    FindGroupNodeBySeq(n, seq, ref findCount, ref isFound, ref nFound);
                }
            }
            return nFound;
        }
        private void FindGroupNodeBySeq(ConditionNode n, int seq, ref int findCount, ref bool isFound, ref ConditionNode foundNode)
        {
            if (n.condition.dictionaryIndex == filterDictionary.StoreGroupName)
            {
                if (findCount == seq)
                {
                    isFound = true;
                    foundNode = n;
                }
                else
                {
                    findCount++;
                }
            }
            else
            {
                foreach (ConditionNode c in n.ConditionNodes)
                {
                    if (isFound == false)
                    {
                        FindGroupNodeBySeq(c, seq, ref findCount, ref isFound, ref foundNode);
                    }
                }
            }

        }
        //End TT#1517-MD -jsobek -Store Service Optimization

        public void RebuildText(filterManager manager, ConditionNode n)
        {
            if (n.condition.Seq != -1) //Do not build text for the root node
            {
                filterDictionaryEntry eg = GetEntryFromCondition(manager, n.condition); 
                eg.BuildFormattedText(manager.Options, ref n.condition);
            }

            foreach (ConditionNode c in n.ConditionNodes)
            {
                RebuildText(manager, c);
            }

        }



        /// <summary>
        /// Gets the cost for the node plus all of its children
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public int GetCost(int seq)
        {
            ConditionNode startingNode = FindConditionNode(seq);
            filterDictionary et = filterDictionary.FromIndex(startingNode.condition.dictionaryIndex);
            int cost = et.costToRunEstimate;
            foreach (ConditionNode n in startingNode.ConditionNodes)
            {
                GetCostForNode(n, ref cost);
            }
            return cost;
        }
        private void GetCostForNode(ConditionNode n, ref int cost)
        {
            filterDictionary et = filterDictionary.FromIndex(n.condition.dictionaryIndex);
            cost += et.costToRunEstimate;
            foreach (ConditionNode c in n.ConditionNodes)
            {
                GetCostForNode(c, ref cost);
            }
        }

        public bool IsLastChild(ConditionNode n)
        {
            DataRow[] drNextSiblings = this.dtConditions.Select("PARENT_SEQ = " + n.condition.ParentSeq.ToString() + " AND SIBLING_SEQ > " + n.condition.SiblingSeq.ToString(), "SIBLING_SEQ");
            if (drNextSiblings.Length > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool HasSiblings(ConditionNode n)
        {
            DataRow[] drSiblings = this.dtConditions.Select("PARENT_SEQ = " + n.condition.ParentSeq.ToString() + " AND SEQ <> " + n.condition.Seq.ToString());
            if (drSiblings.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<int> GetSiblingSequences(ConditionNode n)
        {
            List<int> siblingSequences = new List<int>();
            DataRow[] drSiblings = this.dtConditions.Select("PARENT_SEQ = " + n.condition.ParentSeq.ToString() + " AND SEQ <> " + n.condition.Seq.ToString());
            foreach (DataRow drSibling in drSiblings)
            {
                siblingSequences.Add((int)drSibling["SEQ"]);
            }
            return siblingSequences;
        }
        public bool HasPreviousSibling(ConditionNode n)
        {
            DataRow[] drPrevSiblings = this.dtConditions.Select("PARENT_SEQ = " + n.condition.ParentSeq.ToString() + " AND SIBLING_SEQ < " + n.condition.SiblingSeq.ToString(), "SIBLING_SEQ");
            if (drPrevSiblings.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ConditionNode GetPreviousSibling(ConditionNode n)
        {
            DataRow[] drPrevSiblings = this.dtConditions.Select("PARENT_SEQ = " + n.condition.ParentSeq.ToString() + " AND SIBLING_SEQ < " + n.condition.SiblingSeq.ToString(), "SIBLING_SEQ");
            if (drPrevSiblings.Length > 0)
            {

                return FindConditionNode((int)drPrevSiblings[drPrevSiblings.Length - 1]["SEQ"]);
            }
            else
            {
                return null;
            }
        }
        public bool HasNextSibling(ConditionNode n)
        {
            DataRow[] drNextSiblings = this.dtConditions.Select("PARENT_SEQ = " + n.condition.ParentSeq.ToString() + " AND SIBLING_SEQ > " + n.condition.SiblingSeq.ToString(), "SIBLING_SEQ");
            if (drNextSiblings.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ConditionNode GetNextSibling(ConditionNode n)
        {
            DataRow[] drNextSiblings = this.dtConditions.Select("PARENT_SEQ = " + n.condition.ParentSeq.ToString() + " AND SIBLING_SEQ > " + n.condition.SiblingSeq.ToString(), "SIBLING_SEQ");
            if (drNextSiblings.Length > 0)
            {

                return FindConditionNode((int)drNextSiblings[0]["SEQ"]);
            }
            else
            {
                return null;
            }
        }
        public int GetNextSiblingSeq(int parentSeq)
        {
            DataRow[] drPrevSiblings = this.dtConditions.Select("PARENT_SEQ = " + parentSeq.ToString(), "SIBLING_SEQ");
            if (drPrevSiblings.Length > 0)
            {
                return (int)drPrevSiblings[drPrevSiblings.Length - 1]["SIBLING_SEQ"] + 1;
            }
            else
            {
                return 1;
            }
        }

        public void UpdateConditionNodes()
        {
            foreach (ConditionNode n in RootConditionNode.ConditionNodes)
            {
                UpdateConditionNode(n);
            }

        }
        private void UpdateConditionNode(ConditionNode n)
        {
            UpdateCondition(n.condition);
            foreach (ConditionNode c in n.ConditionNodes)
            {
                UpdateConditionNode(c);
            }
        }
        public void RemoveConditionNode(ConditionNode n)
        {
            int seqToRemove = n.condition.Seq;
            n.Parent.ConditionNodes.Remove(n);

            //now remove node and all of its children recursively from the datatable
            DataRow[] drfind = this.dtConditions.Select("SEQ = " + seqToRemove.ToString());

            //dtConditions.Rows.Remove(drfind[0]);
            RemoveChildren(drfind[0]);
        }

        private void RemoveChildren(DataRow drToRemove)
        {
            int seq = (int)drToRemove["SEQ"];
            dtConditions.Rows.Remove(drToRemove);
            DataRow[] drchildren = this.dtConditions.Select("PARENT_SEQ = " + seq.ToString());
            foreach (DataRow dr in drchildren)
            {
                RemoveChildren(dr);
            }
        }

    }

    public class filterCondition
    {
        public int conditionRID; //CONDITION_RID
        public int conditionFilterRID; //FILTER_RID

        public int Seq; //SEQ
        public int ParentSeq; //PARENT_SEQ
        public int SiblingSeq; //SIBLING_SEQ

        public int dictionaryIndex; //ELEMENT_GROUP_TYPE_INDEX

        public int logicIndex = -1; //LOGIC_INDEX
        public int fieldIndex = -1; //FIELD_INDEX
        public int operatorIndex = -1; //OPERATOR_INDEX

        public int valueTypeIndex = -1; //VALUE_TYPE_INDEX
        public int dateTypeIndex = -1; //DATE_TYPE_INDEX
        public int numericTypeIndex = -1; //NUMERIC_TYPE_INDEX

        public string valueToCompare = string.Empty; //VALUE_TO_COMPARE
        public double? valueToCompareDouble = null; //VALUE_TO_COMPARE_DOUBLE
        public double? valueToCompareDouble2 = null;  //VALUE_TO_COMPARE_DOUBLE2
        public int? valueToCompareInt = null; //VALUE_TO_COMPARE_INT
        public int? valueToCompareInt2 = null;  //VALUE_TO_COMPARE_INT2

        public bool? valueToCompareBool = null; //VALUE_TO_COMPARE_BOOL

        public DateTime? valueToCompareDateFrom = null; //VALUE_TO_COMPARE_DATE_FROM
        public DateTime? valueToCompareDateTo = null; //VALUE_TO_COMPARE_DATE_TO
        public int valueToCompareDateBetweenFromDays = -1; //VALUE_TO_COMPARE_DATE_FROM_DAYS
        public int valueToCompareDateBetweenToDays = -1; //VALUE_TO_COMPARE_DATE_TO_DAYS

        //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        public int? lastOperatorIndexNumeric = null;
        public int? lastOperatorIndexNumericForVariable = null;
        public int? lastOperatorIndexString = null;
        public int? lastOperatorIndexDate = null;
        public int? lastOperatorIndexVariablePercentage = null;

        public string lastValueToCompare = string.Empty;
        public double? lastValueToCompareDouble = null;
        public double? lastValueToCompareDouble2 = null;
        public int? lastValueToCompareInt = null;
        public int? lastValueToCompareInt2 = null;
        public bool? lastValueToCompareBool = null;
        public DateTime? lastValueToCompareDateFrom = null;
        public DateTime? lastValueToCompareDateTo = null;
        public int lastValueToCompareDateBetweenFromDays = -1;
        public int lastValueToCompareDateBetweenToDays = -1;

        public int? lastVariable1_VersionIndex = null;
        public int? lastVariable1_VariableValueTypeIndex = null;
        public int? lastVariable1_TimeTypeIndex = null;
        public int? lastVariable1_HN_RID = null;
        public int? lastVariable1_CDR_RID = null;

        public int? lastVariable2_VersionIndex = null;
        public int? lastVariable2_VariableValueTypeIndex = null;
        public int? lastVariable2_TimeTypeIndex = null;
        public int? lastVariable2_HN_RID = null;
        public int? lastVariable2_CDR_RID = null;
        //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables

        public int variable1_Index = -1;
        public int variable1_VersionIndex = -1;
        public int variable1_HN_RID = -1;
        public int variable1_CDR_RID = Include.UndefinedCalendarDateRange;
        public int variable1_VariableValueTypeIndex = -1;
        public int variable1_TimeTypeIndex = -1;
        //public int variable1_IsTimeTotal = 0;

        public int operatorVariablePercentageIndex = -1;

        public int variable2_Index = -1;
        public int variable2_VersionIndex = -1;
        public int variable2_HN_RID = -1;
        public int variable2_CDR_RID = Include.UndefinedCalendarDateRange;
        public int variable2_VariableValueTypeIndex = -1;
        public int variable2_TimeTypeIndex = -1;
        //public int variable2_IsTimeTotal = 0;

        public int headerMerchandise_HN_RID = -1;
        //public int headerMerchandise_PH_RID = -1;

        public int sortByTypeIndex = -1; //SORT_BY_TYPE_INDEX
        public int sortByFieldIndex = -1; //SORT_BY_FIELD_INDEX


        public filterListConstantTypes listConstantType = filterListConstantTypes.None;
        public DataTable dtListValues;

        public string NodeFormattedText; //Shows in the tree control. Not saved on database, but rebuilt everytime the filter is loaded.

        public string executeGroup;
        public string executed;
        public string executeResult;
        public string executeResultForGroup;
        public bool executeSkipChildren = false;

        public filterCondition()
        {
            this.dtListValues = filterCondition.GetListValuesDataTable();
        }

        public static DataTable GetConditionDataTable()
        {
            DataTable dtConditions = new DataTable("conditions");
            dtConditions.Columns.Add("CONDITION_RID", typeof(int));
            dtConditions.Columns.Add("FILTER_RID", typeof(int));

            dtConditions.Columns.Add("SEQ", typeof(int));
            dtConditions.Columns.Add("PARENT_SEQ", typeof(int));
            dtConditions.Columns.Add("SIBLING_SEQ", typeof(int));

            dtConditions.Columns.Add("ELEMENT_GROUP_TYPE_INDEX", typeof(int));

            dtConditions.Columns.Add("LOGIC_INDEX", typeof(int));
            dtConditions.Columns.Add("FIELD_INDEX", typeof(int));
            dtConditions.Columns.Add("OPERATOR_INDEX", typeof(int));

            dtConditions.Columns.Add("VALUE_TYPE_INDEX", typeof(int));
            dtConditions.Columns.Add("DATE_TYPE_INDEX", typeof(int));
            dtConditions.Columns.Add("NUMERIC_TYPE_INDEX", typeof(int));

            dtConditions.Columns.Add("VALUE_TO_COMPARE");
            dtConditions.Columns.Add("VALUE_TO_COMPARE_DOUBLE", typeof(double));
            dtConditions.Columns.Add("VALUE_TO_COMPARE_DOUBLE2", typeof(double));
            dtConditions.Columns.Add("VALUE_TO_COMPARE_INT", typeof(int));
            dtConditions.Columns.Add("VALUE_TO_COMPARE_INT2", typeof(int));
            dtConditions.Columns.Add("VALUE_TO_COMPARE_BOOL", typeof(int));
            dtConditions.Columns.Add("VALUE_TO_COMPARE_DATE_FROM", typeof(DateTime));
            dtConditions.Columns.Add("VALUE_TO_COMPARE_DATE_TO", typeof(DateTime));
            dtConditions.Columns.Add("VALUE_TO_COMPARE_DATE_DAYS_FROM", typeof(int));
            dtConditions.Columns.Add("VALUE_TO_COMPARE_DATE_DAYS_TO", typeof(int));

            dtConditions.Columns.Add("VAR1_VARIABLE_INDEX", typeof(int));
            dtConditions.Columns.Add("VAR1_VERSION_INDEX", typeof(int));
            dtConditions.Columns.Add("VAR1_HN_RID", typeof(int));
            dtConditions.Columns.Add("VAR1_CDR_RID", typeof(int));
            dtConditions.Columns.Add("VAR1_VALUE_TYPE_INDEX", typeof(int));
            dtConditions.Columns.Add("VAR1_TIME_INDEX", typeof(int));
            dtConditions.Columns.Add("VAR1_IS_TIME_TOTAL", typeof(int));

            dtConditions.Columns.Add("VAR_PERCENTAGE_OPERATOR_INDEX", typeof(int));

            dtConditions.Columns.Add("VAR2_VARIABLE_INDEX", typeof(int));
            dtConditions.Columns.Add("VAR2_VERSION_INDEX", typeof(int));
            dtConditions.Columns.Add("VAR2_HN_RID", typeof(int));
            dtConditions.Columns.Add("VAR2_CDR_RID", typeof(int));
            dtConditions.Columns.Add("VAR2_VALUE_TYPE_INDEX", typeof(int));
            dtConditions.Columns.Add("VAR2_TIME_INDEX", typeof(int));
            dtConditions.Columns.Add("VAR2_IS_TIME_TOTAL", typeof(int));

            dtConditions.Columns.Add("HEADER_HN_RID", typeof(int));

            dtConditions.Columns.Add("SORT_BY_TYPE_INDEX", typeof(int));
            dtConditions.Columns.Add("SORT_BY_FIELD_INDEX", typeof(int));


            //dtConditions.Columns.Add("LIST_VALUES");   
            dtConditions.Columns.Add("LIST_VALUE_CONSTANT_INDEX", typeof(int));



            return dtConditions;
        }
        public void LoadFromDataRow(DataRow drCondition)
        {
            this.conditionRID = (int)drCondition["CONDITION_RID"];
            this.conditionFilterRID = (int)drCondition["FILTER_RID"];

            this.Seq = (int)drCondition["SEQ"];
            this.ParentSeq = (int)drCondition["PARENT_SEQ"];
            this.SiblingSeq = (int)drCondition["SIBLING_SEQ"];

            this.dictionaryIndex = GetIntFieldFromDataRow(drCondition, "ELEMENT_GROUP_TYPE_INDEX");

            this.fieldIndex = GetIntFieldFromDataRow(drCondition, "FIELD_INDEX");
            this.logicIndex = GetIntFieldFromDataRow(drCondition, "LOGIC_INDEX");
            this.operatorIndex = GetIntFieldFromDataRow(drCondition, "OPERATOR_INDEX");

            this.valueTypeIndex = GetIntFieldFromDataRow(drCondition, "VALUE_TYPE_INDEX");
            this.dateTypeIndex = GetIntFieldFromDataRow(drCondition, "DATE_TYPE_INDEX");
            this.numericTypeIndex = GetIntFieldFromDataRow(drCondition, "NUMERIC_TYPE_INDEX");

            this.valueToCompare = GetStringFieldFromDataRow(drCondition, "VALUE_TO_COMPARE");
            this.valueToCompareDouble = GetDoubleFieldFromDataRow(drCondition, "VALUE_TO_COMPARE_DOUBLE");
            this.valueToCompareDouble2 = GetDoubleFieldFromDataRow(drCondition, "VALUE_TO_COMPARE_DOUBLE2");
            this.valueToCompareInt = GetNullableIntFieldFromDataRow(drCondition, "VALUE_TO_COMPARE_INT");
            this.valueToCompareInt2 = GetNullableIntFieldFromDataRow(drCondition, "VALUE_TO_COMPARE_INT2");
            this.valueToCompareBool = GetBoolFieldFromDataRow(drCondition, "VALUE_TO_COMPARE_BOOL");

            this.valueToCompareDateFrom = GetDateFieldFromDataRow(drCondition, "VALUE_TO_COMPARE_DATE_FROM");
            this.valueToCompareDateTo = GetDateFieldFromDataRow(drCondition, "VALUE_TO_COMPARE_DATE_TO");
            this.valueToCompareDateBetweenFromDays = GetIntFieldFromDataRow(drCondition, "VALUE_TO_COMPARE_DATE_DAYS_FROM");
            this.valueToCompareDateBetweenToDays = GetIntFieldFromDataRow(drCondition, "VALUE_TO_COMPARE_DATE_DAYS_TO");

            this.variable1_Index = GetIntFieldFromDataRow(drCondition, "VAR1_VARIABLE_INDEX");
            this.variable1_VersionIndex = GetIntFieldFromDataRow(drCondition, "VAR1_VERSION_INDEX");
            this.variable1_HN_RID = GetIntFieldFromDataRow(drCondition, "VAR1_HN_RID");
            this.variable1_CDR_RID = GetDateIntFieldFromDataRow(drCondition, "VAR1_CDR_RID"); //TT#1448-MD -jsobek -Store Filter - Cleared a Date range and saved. Received a Null Reference Exception. selecected OK and the calendar dates were gone.
            this.variable1_VariableValueTypeIndex = GetIntFieldFromDataRow(drCondition, "VAR1_VALUE_TYPE_INDEX");
            this.variable1_TimeTypeIndex = GetIntFieldFromDataRow(drCondition, "VAR1_TIME_INDEX");
            //this.variable1_IsTimeTotal = GetIntFieldFromDataRow(drCondition, "VAR1_IS_TIME_TOTAL");

            this.operatorVariablePercentageIndex = GetIntFieldFromDataRow(drCondition, "VAR_PERCENTAGE_OPERATOR_INDEX");

            this.variable2_Index = GetIntFieldFromDataRow(drCondition, "VAR2_VARIABLE_INDEX");
            this.variable2_VersionIndex = GetIntFieldFromDataRow(drCondition, "VAR2_VERSION_INDEX");
            this.variable2_HN_RID = GetIntFieldFromDataRow(drCondition, "VAR2_HN_RID");
            this.variable2_CDR_RID = GetDateIntFieldFromDataRow(drCondition, "VAR2_CDR_RID"); //TT#1448-MD -jsobek -Store Filter - Cleared a Date range and saved. Received a Null Reference Exception. selecected OK and the calendar dates were gone.
            this.variable2_VariableValueTypeIndex = GetIntFieldFromDataRow(drCondition, "VAR2_VALUE_TYPE_INDEX");
            this.variable2_TimeTypeIndex = GetIntFieldFromDataRow(drCondition, "VAR2_TIME_INDEX");
            //this.variable2_IsTimeTotal = GetIntFieldFromDataRow(drCondition, "VAR2_IS_TIME_TOTAL");

            this.headerMerchandise_HN_RID = GetIntFieldFromDataRow(drCondition, "HEADER_HN_RID");
            //this.headerMerchandise_PH_RID = GetIntFieldFromDataRow(drCondition, "HEADER_PH_RID");

            this.sortByTypeIndex = GetIntFieldFromDataRow(drCondition, "SORT_BY_TYPE_INDEX");
            this.sortByFieldIndex = GetIntFieldFromDataRow(drCondition, "SORT_BY_FIELD_INDEX");

            int listConstantTypeIndex = GetIntFieldFromDataRow(drCondition, "LIST_VALUE_CONSTANT_INDEX");
            if (listConstantTypeIndex == -1)
            {
                listConstantTypeIndex = 0;
            }
            this.listConstantType = filterListConstantTypes.FromIndex(listConstantTypeIndex);

            this.dtListValues = GetListValuesDataTable();
        }

        public void SaveToDataRow(ref DataRow drCondition)
        {
            drCondition["CONDITION_RID"] = this.conditionRID;
            drCondition["FILTER_RID"] = this.conditionFilterRID;

            drCondition["SEQ"] = this.Seq;
            drCondition["PARENT_SEQ"] = this.ParentSeq;
            drCondition["SIBLING_SEQ"] = this.SiblingSeq;

            drCondition["ELEMENT_GROUP_TYPE_INDEX"] = this.dictionaryIndex;

            SaveIntFieldToDataRow(drCondition, "LOGIC_INDEX", this.logicIndex);
            SaveIntFieldToDataRow(drCondition, "OPERATOR_INDEX", this.operatorIndex);
            SaveIntFieldToDataRow(drCondition, "FIELD_INDEX", this.fieldIndex);

            SaveIntFieldToDataRow(drCondition, "VALUE_TYPE_INDEX", this.valueTypeIndex);
            SaveIntFieldToDataRow(drCondition, "DATE_TYPE_INDEX", this.dateTypeIndex);
            SaveIntFieldToDataRow(drCondition, "NUMERIC_TYPE_INDEX", this.numericTypeIndex);

            SaveStringFieldToDataRow(drCondition, "VALUE_TO_COMPARE", this.valueToCompare);


            if (this.valueToCompareDouble == null)
            {
                drCondition["VALUE_TO_COMPARE_DOUBLE"] = DBNull.Value;
            }
            else
            {
                drCondition["VALUE_TO_COMPARE_DOUBLE"] = this.valueToCompareDouble;
            }
            if (this.valueToCompareDouble2 == null)
            {
                drCondition["VALUE_TO_COMPARE_DOUBLE2"] = DBNull.Value;
            }
            else
            {
                drCondition["VALUE_TO_COMPARE_DOUBLE2"] = this.valueToCompareDouble2;
            }
            if (this.valueToCompareInt == null)
            {
                drCondition["VALUE_TO_COMPARE_INT"] = DBNull.Value;
            }
            else
            {
                drCondition["VALUE_TO_COMPARE_INT"] = this.valueToCompareInt;
            }
            if (this.valueToCompareInt2 == null)
            {
                drCondition["VALUE_TO_COMPARE_INT2"] = DBNull.Value;
            }
            else
            {
                drCondition["VALUE_TO_COMPARE_INT2"] = this.valueToCompareInt2;
            }

            if (this.valueToCompareBool != null)
            {
                if (this.valueToCompareBool == true)
                {
                    drCondition["VALUE_TO_COMPARE_BOOL"] = 1;
                }
                else
                {
                    drCondition["VALUE_TO_COMPARE_BOOL"] = 0;
                }
            }
            else
            {
                drCondition["VALUE_TO_COMPARE_BOOL"] = DBNull.Value;
            }

            if (this.valueToCompareDateFrom == null)
            {
                drCondition["VALUE_TO_COMPARE_DATE_FROM"] = DBNull.Value;
            }
            else
            {
                drCondition["VALUE_TO_COMPARE_DATE_FROM"] = this.valueToCompareDateFrom;
            }
            if (this.valueToCompareDateTo == null)
            {
                drCondition["VALUE_TO_COMPARE_DATE_TO"] = DBNull.Value;
            }
            else
            {
                drCondition["VALUE_TO_COMPARE_DATE_TO"] = this.valueToCompareDateTo;
            }

            SaveIntFieldToDataRow(drCondition, "VALUE_TO_COMPARE_DATE_DAYS_FROM", this.valueToCompareDateBetweenFromDays);
            SaveIntFieldToDataRow(drCondition, "VALUE_TO_COMPARE_DATE_DAYS_TO", this.valueToCompareDateBetweenToDays);

            SaveIntFieldToDataRow(drCondition, "VAR1_VARIABLE_INDEX", this.variable1_Index);
            SaveIntFieldToDataRow(drCondition, "VAR1_VERSION_INDEX", this.variable1_VersionIndex);
            SaveIntFieldToDataRow(drCondition, "VAR1_HN_RID", this.variable1_HN_RID);
            SaveIntFieldToDataRow(drCondition, "VAR1_CDR_RID", this.variable1_CDR_RID);
            SaveIntFieldToDataRow(drCondition, "VAR1_VALUE_TYPE_INDEX", this.variable1_VariableValueTypeIndex);
            SaveIntFieldToDataRow(drCondition, "VAR1_TIME_INDEX", this.variable1_TimeTypeIndex);
            //SaveIntFieldToDataRow(drCondition, "VAR1_IS_TIME_TOTAL", this.variable1_IsTimeTotal);
    

            SaveIntFieldToDataRow(drCondition, "VAR_PERCENTAGE_OPERATOR_INDEX", this.operatorVariablePercentageIndex);

            SaveIntFieldToDataRow(drCondition, "VAR2_VARIABLE_INDEX", this.variable2_Index);
            SaveIntFieldToDataRow(drCondition, "VAR2_VERSION_INDEX", this.variable2_VersionIndex);
            SaveIntFieldToDataRow(drCondition, "VAR2_HN_RID", this.variable2_HN_RID);
            SaveIntFieldToDataRow(drCondition, "VAR2_CDR_RID", this.variable2_CDR_RID);
            SaveIntFieldToDataRow(drCondition, "VAR2_VALUE_TYPE_INDEX", this.variable2_VariableValueTypeIndex);
            SaveIntFieldToDataRow(drCondition, "VAR2_TIME_INDEX", this.variable2_TimeTypeIndex);
            //SaveIntFieldToDataRow(drCondition, "VAR2_IS_TIME_TOTAL", this.variable2_IsTimeTotal);
    

            SaveIntFieldToDataRow(drCondition, "HEADER_HN_RID", this.headerMerchandise_HN_RID);
            //SaveIntFieldToDataRow(drCondition, "HEADER_PH_RID", this.headerMerchandise_PH_RID);

            SaveIntFieldToDataRow(drCondition, "SORT_BY_TYPE_INDEX", this.sortByTypeIndex);
            SaveIntFieldToDataRow(drCondition, "SORT_BY_FIELD_INDEX", this.sortByFieldIndex);

            if (this.listConstantType == filterListConstantTypes.None)
            {
                drCondition["LIST_VALUE_CONSTANT_INDEX"] = DBNull.Value;
            }
            else
            {
                drCondition["LIST_VALUE_CONSTANT_INDEX"] = this.listConstantType.dbIndex;
            }
        }
        public static DataTable GetListValuesDataTable()
        {
            DataTable dtListValues = new DataTable("listvalues");
            //dtListValues.Columns.Add("LIST_VALUE_RID", typeof(int));
            dtListValues.Columns.Add("CONDITION_RID", typeof(int));
            dtListValues.Columns.Add("LIST_VALUE_TYPE_INDEX", typeof(int));
            dtListValues.Columns.Add("LIST_VALUE_INDEX", typeof(int));
            dtListValues.Columns.Add("LIST_VALUE_PARENT_INDEX", typeof(int));
            return dtListValues;
        }
        public static string GetListValueIndexField()
        {
            return "LIST_VALUE_INDEX";
        }
        public static string GetListValueParentIndexField()
        {
            return "LIST_VALUE_PARENT_INDEX";
        }
        public void LoadListValuesFromDataRowArray(DataRow[] drListValuesForCondition)
        {
            foreach (DataRow drListValue in drListValuesForCondition)
            {
                DataRow dr = dtListValues.NewRow();
                filterUtility.DataRowCopy(drListValue, dr);
                dtListValues.Rows.Add(dr);
            }
        }
        public void ClearListValues()
        {
            this.dtListValues.Rows.Clear();

        }
        public void SaveListValues(DataTable dt, filterListValueTypes listValueType)
        {
            this.dtListValues.Rows.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                DataRow drNew = dtListValues.NewRow();
                drNew["CONDITION_RID"] = this.conditionRID;
                drNew["LIST_VALUE_TYPE_INDEX"] = listValueType.Index;
                drNew["LIST_VALUE_INDEX"] = dr["LIST_VALUE_INDEX"];
                if (dt.Columns.Contains("LIST_VALUE_PARENT_INDEX"))
                {
                    drNew["LIST_VALUE_PARENT_INDEX"] = dr["LIST_VALUE_PARENT_INDEX"];
                }
        
                dtListValues.Rows.Add(drNew);
            }
        }
        public DataRow[] GetListValues(filterListValueTypes listValueType)
        {
            return dtListValues.Select("LIST_VALUE_TYPE_INDEX = " + listValueType.Index);
        }
        private string GetStringFieldFromDataRow(DataRow drCondition, string field)
        {
            if (drCondition[field] != DBNull.Value)
            {
                return (string)drCondition[field];
            }
            else
            {
                return string.Empty;
            }
        }
        private void SaveStringFieldToDataRow(DataRow drCondition, string field, string val)
        {
            if (val == string.Empty)
            {
                drCondition[field] = DBNull.Value;
            }
            else
            {
                drCondition[field] = val;
            }
        }
        private double? GetDoubleFieldFromDataRow(DataRow drCondition, string field)
        {
            if (drCondition[field] != DBNull.Value)
            {
                return (double)drCondition[field];
            }
            else
            {
                return null;
            }
        }
        private DateTime? GetDateFieldFromDataRow(DataRow drCondition, string field)
        {
            if (drCondition[field] != DBNull.Value)
            {
                return (DateTime)drCondition[field];
            }
            else
            {
                return null;
            }
        }
        private bool? GetBoolFieldFromDataRow(DataRow drCondition, string field)
        {
            if (drCondition[field] != DBNull.Value)
            {
                //if ((int)drCondition[field] == 0)
                //{
                //    return false;
                //}
                //else
                //{
                //    return true;
                //}
                return (bool)drCondition[field];
            }
            else
            {
                return null;
            }
        }
        private int GetIntFieldFromDataRow(DataRow drCondition, string field)
        {
            if (drCondition[field] != DBNull.Value)
            {
                return (int)drCondition[field];
            }
            else
            {
                return -1;
            }
        }
        //Begin TT#1448-MD -jsobek -Store Filter - Cleared a Date range and saved. Received a Null Reference Exception. selecected OK and the calendar dates were gone.
        private int GetDateIntFieldFromDataRow(DataRow drCondition, string field)
        {
            if (drCondition[field] != DBNull.Value)
            {
                return (int)drCondition[field];
            }
            else
            {
                return Include.UndefinedCalendarDateRange;
            }
        }
        //End TT#1448-MD -jsobek -Store Filter - Cleared a Date range and saved. Received a Null Reference Exception. selecected OK and the calendar dates were gone.
        private void SaveIntFieldToDataRow(DataRow drCondition, string field, int val)
        {
            if (val == -1)
            {
                drCondition[field] = DBNull.Value;
            }
            else
            {
                drCondition[field] = val;
            }
        }
        private void SaveNullableIntFieldToDataRow(DataRow drCondition, string field, int? val)
        {
            if (val == null)
            {
                drCondition[field] = DBNull.Value;
            }
            else
            {
                drCondition[field] = val;
            }
        }
        private int? GetNullableIntFieldFromDataRow(DataRow drCondition, string field)
        {
            if (drCondition[field] != DBNull.Value)
            {
                return (int)drCondition[field];
            }
            else
            {
                return null;
            }
        }
    }

    public class ConditionNode
    {
        public filterCondition condition;
        public List<ConditionNode> ConditionNodes;
        public ConditionNode Parent;
        public bool IsRootLevel
        {
            get
            {
                if (condition.ParentSeq == -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public int NodeLevel
        {
            get
            {
                return GetNodeLevel(this);
            }
        }
        public bool IsSelected = false;


        private int GetNodeLevel(ConditionNode n)
        {
            int nLevel = 0;
            if (n.Parent != null)
            {
                FindNodeLevel(n.Parent, ref nLevel);
            }
            return nLevel;
        }
        private void FindNodeLevel(ConditionNode n, ref int nLevel)
        {
            nLevel += 1;
            if (n.Parent != null)
            {
                FindNodeLevel(n.Parent, ref nLevel);
            }
        }
    }

    

 
}
