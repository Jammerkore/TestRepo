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
    public delegate DataTable LoadListDelegate();
    public delegate DataTable LoadValueListFromFieldDelegate(int fieldIndex);
    public delegate string GetNameFromFieldIndexDelegate(int fieldIndex);
    public delegate valueInfoTypes GetValueInfoTypeFromFieldIndexDelegate(int fieldIndex);
    public delegate void MakeElementInGroupDelegate(string key, elementBase eb, bool useValueListFromField, int tempFieldIndex);
    public delegate void RemoveDynamicElementsForFieldDelegate(List<string> keyListToRemove);


    public class filter : MIDRetail.Common.Filter
    {
        //public DataSet dsConditions;
        private DataTable dtConditions;
        public int filterRID;
        public filterTypes filterType;
        public string filterName;
        public int userRID;
        public int ownerUserRID;
        //public string filterFolderUserOrGlobal;
        // public bool isTemplate;
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
            //filterFolderUserOrGlobal = "User";
            filterRID = -1;
            this.userRID = userRID;
            this.ownerUserRID = ownerUserRID;
            //this.currentUserRID = userRID;
            //isTemplate = false;
            isLimited = false;

            //dsConditions = new DataSet();
            dtConditions = filterCondition.GetConditionDataTable();



            //dsConditions.Tables.Add(dtConditions);


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

        public static DataTable GetFilterDataTable()
        {
            DataTable dtFilters = new DataTable("filters");
            dtFilters.Columns.Add("FILTER_NAME");
            dtFilters.Columns.Add("FILTER_RID", typeof(int));
            dtFilters.Columns.Add("USER_RID", typeof(int));
            dtFilters.Columns.Add("OWNER_USER_RID", typeof(int));
            dtFilters.Columns.Add("FILTER_TYPE", typeof(int));
            dtFilters.Columns.Add("FILTER_TYPE_ID");
            //dtFilters.Columns.Add("IS_TEMPLATE", typeof(int));
            dtFilters.Columns.Add("IS_LIMITED", typeof(int));
            dtFilters.Columns.Add("RESULT_LIMIT", typeof(long));
            return dtFilters;
        }
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

        public elementGroup GetElementGroupFromCondition(filterManager manager, filterCondition condition)
        {
            elementGroup eg;
            elementGroupTypes et = elementGroupTypes.FromIndex(condition.elementGroupTypeIndex);
            object[] args = new object[1];
            args[0] = manager;
            object obj = Activator.CreateInstance(et.groupType, args);
            eg = (elementGroup)obj;
            return eg;
        }


        public int GetNextConditionSeq()
        {
            return (dtConditions.Rows.Count + 1);
        }
        public DataRowCollection GetConditionDataRows()
        {
            return dtConditions.Rows;
        }

        public void AddInitialConditions(filterManager manager)
        {
            //Add Filter Info
            filterCondition filterInfo = new filterCondition();

            filterInfo.elementGroupTypeIndex = elementGroupTypes.InfoFilter.dbIndex; // "InfoFilter";
            //filterInfo.navigationType = navigationTypes.Info;
            filterInfo.conditionFilterRID = filterRID;
            filterInfo.Seq = GetNextConditionSeq();
            filterInfo.ParentSeq = -1;
            filterInfo.SiblingSeq = GetNextSiblingSeq(filterInfo.ParentSeq);

            elementGroup eg = GetElementGroupFromCondition(manager, filterInfo);
            eg.BuildFormattedText(manager.Options, ref filterInfo);

            DataRow drNewCondtion = dtConditions.NewRow();
            filterInfo.SaveToDataRow(ref drNewCondtion);
            dtConditions.Rows.Add(drNewCondtion);
            BuildConditionNode(filterInfo);




            //Add Condition Parent
            filterCondition conditionParent = new filterCondition();
            conditionParent.elementGroupTypeIndex = elementGroupTypes.InfoConditions.dbIndex; //"InfoConditions";
            //conditionParent.navigationType = navigationTypes.Info;
            conditionParent.conditionFilterRID = filterRID;
            conditionParent.Seq = dtConditions.Rows.Count + 1;
            conditionParent.ParentSeq = -1;
            conditionParent.SiblingSeq = GetNextSiblingSeq(conditionParent.ParentSeq);

            elementGroup eg2 = GetElementGroupFromCondition(manager, conditionParent);
            eg2.BuildFormattedText(manager.Options, ref conditionParent);

            DataRow drNewCondtion2 = dtConditions.NewRow();
            conditionParent.SaveToDataRow(ref drNewCondtion2);
            dtConditions.Rows.Add(drNewCondtion2);
            BuildConditionNode(conditionParent); ;

            //Add SortBy Parent
            filterCondition sortByParent = new filterCondition();
            sortByParent.elementGroupTypeIndex = elementGroupTypes.InfoSortBy.dbIndex; //"InfoSortBy";
            //sortByParent.navigationType = navigationTypes.Info;
            sortByParent.conditionFilterRID = filterRID;
            sortByParent.Seq = dtConditions.Rows.Count + 1;
            sortByParent.ParentSeq = -1;
            sortByParent.SiblingSeq = GetNextSiblingSeq(sortByParent.ParentSeq);

            elementGroup eg3 = GetElementGroupFromCondition(manager, sortByParent);
            eg3.BuildFormattedText(manager.Options, ref sortByParent);

            DataRow drNewCondtion3 = dtConditions.NewRow();
            sortByParent.SaveToDataRow(ref drNewCondtion3);
            dtConditions.Rows.Add(drNewCondtion3);
            BuildConditionNode(sortByParent);


            //Add Filter Name
            filterCondition fcFilterName = new filterCondition();
            fcFilterName.elementGroupTypeIndex = elementGroupTypes.FilterName.dbIndex; //"FilterName";
            //fcFilterName.navigationType = navigationTypes.Info;
            fcFilterName.conditionFilterRID = filterRID;
            fcFilterName.Seq = dtConditions.Rows.Count + 1;
            fcFilterName.valueToCompare = filterName;

            elementGroup egFilterName = GetElementGroupFromCondition(manager, fcFilterName);
            egFilterName.SetDefaults(ref fcFilterName);
            egFilterName.BuildFormattedText(manager.Options, ref fcFilterName);


            fcFilterName.ParentSeq = 1;
            fcFilterName.SiblingSeq = 1;
            DataRow drNewCondtion4 = dtConditions.NewRow();
            fcFilterName.SaveToDataRow(ref drNewCondtion4);
            dtConditions.Rows.Add(drNewCondtion4);
            BuildConditionNode(fcFilterName);

            //Add Folder
            filterCondition fcFolder = new filterCondition();
            fcFolder.elementGroupTypeIndex = elementGroupTypes.FilterFolder.dbIndex; //"FilterFolder";
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

            elementGroup egFolder = GetElementGroupFromCondition(manager, fcFolder);
            egFolder.SetDefaults(ref fcFolder);
            egFolder.BuildFormattedText(manager.Options, ref fcFolder);


            fcFolder.ParentSeq = 1;
            fcFolder.SiblingSeq = 2;
            DataRow drNewCondtion5 = dtConditions.NewRow();
            fcFolder.SaveToDataRow(ref drNewCondtion5);
            dtConditions.Rows.Add(drNewCondtion5);
            BuildConditionNode(fcFolder);

            //Add Limit
            filterCondition fcLimit = new filterCondition();
            fcLimit.elementGroupTypeIndex = elementGroupTypes.ResultLimit.dbIndex; //"FilterLimit";
            //fcLimit.navigationType = navigationTypes.Info;
            fcLimit.conditionFilterRID = filterRID;
            fcLimit.Seq = dtConditions.Rows.Count + 1;
            fcLimit.valueToCompare = "Unrestricted";

            elementGroup egLimit = GetElementGroupFromCondition(manager, fcLimit);
            egLimit.SetDefaults(ref fcLimit);
            egLimit.BuildFormattedText(manager.Options, ref fcLimit);

            fcLimit.ParentSeq = 1;
            fcLimit.SiblingSeq = 3;
            DataRow drNewCondtion6 = dtConditions.NewRow();
            fcLimit.SaveToDataRow(ref drNewCondtion6);
            dtConditions.Rows.Add(drNewCondtion6);
            BuildConditionNode(fcLimit);
        }

        public ConditionNode InsertCondition(ConditionNode selectedConditionNode, filterCondition fc)
        {
            navigationTypes selectedNavType = elementGroupTypes.FromIndex(selectedConditionNode.condition.elementGroupTypeIndex).navigationType;
            navigationTypes conditionNavType = elementGroupTypes.FromIndex(fc.elementGroupTypeIndex).navigationType;
            if (conditionNavType == navigationTypes.SortBy)
            {
                fc.ParentSeq = 3; // the default Parent Seq for sort bys
                fc.SiblingSeq = GetNextSiblingSeq(fc.ParentSeq);
            }
            else
            {
                if (selectedConditionNode != null && selectedNavType == navigationTypes.Condition)
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



            DataRow drNewCondtion = dtConditions.NewRow();
            fc.SaveToDataRow(ref drNewCondtion);
            dtConditions.Rows.Add(drNewCondtion);

            return BuildConditionNode(fc);
        }
        public ConditionNode BuildConditionNode(filterCondition fc)
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
              
                parent.ConditionNodes.Add(n);
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

        public void RebuildText(filterManager manager, ConditionNode n)
        {
            if (n.condition.Seq != -1) //Do not build text for the root node
            {
                elementGroup eg = GetElementGroupFromCondition(manager, n.condition); 
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
            elementGroupTypes et = elementGroupTypes.FromIndex(startingNode.condition.elementGroupTypeIndex);
            int cost = et.costToRunEstimate;
            foreach (ConditionNode n in startingNode.ConditionNodes)
            {
                GetCostForNode(n, ref cost);
            }
            return cost;
        }
        private void GetCostForNode(ConditionNode n, ref int cost)
        {
            elementGroupTypes et = elementGroupTypes.FromIndex(n.condition.elementGroupTypeIndex);
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

    public class filterCondition
    {
        public int conditionRID; //CONDITION_RID
        public int conditionFilterRID; //FILTER_RID

        public int Seq; //SEQ
        public int ParentSeq; //PARENT_SEQ
        public int SiblingSeq; //SIBLING_SEQ

        public int elementGroupTypeIndex; //ELEMENT_GROUP_TYPE_INDEX

        public int logicIndex; //LOGIC_INDEX
        public int fieldIndex; //FIELD_INDEX
        public int operatorIndex; //OPERATOR_INDEX

        public int valueTypeIndex; //VALUE_TYPE_INDEX
        public int dateTypeIndex; //DATE_TYPE_INDEX
        public int numericTypeIndex; //NUMERIC_TYPE_INDEX

        public string valueToCompare; //VALUE_TO_COMPARE
        public double? valueToCompareDouble; //VALUE_TO_COMPARE_DOUBLE
        public double? valueToCompareDouble2;  //VALUE_TO_COMPARE_DOUBLE2
        public int? valueToCompareInt; //VALUE_TO_COMPARE_INT
        public int? valueToCompareInt2;  //VALUE_TO_COMPARE_INT2

        public bool valueToCompareBool; //VALUE_TO_COMPARE_BOOL

        public DateTime? valueToCompareDateFrom; //VALUE_TO_COMPARE_DATE_FROM
        public DateTime? valueToCompareDateTo; //VALUE_TO_COMPARE_DATE_TO
        public int valueToCompareDateBetweenFromDays; //VALUE_TO_COMPARE_DATE_FROM_DAYS
        public int valueToCompareDateBetweenToDays; //VALUE_TO_COMPARE_DATE_TO_DAYS

        public int variable1_Index;
        public int variable1_VersionIndex;
        public int variable1_HN_RID = -1;
        public int variable1_CDR_RID = Include.UndefinedCalendarDateRange;
        public int variable1_VariableValueTypeIndex;
        public int variable1_TimeTypeIndex;
        public int variable1_IsTimeTotal;

        public int operatorVariablePercentageIndex;

        public int variable2_Index;
        public int variable2_VersionIndex;
        public int variable2_HN_RID = -1;
        public int variable2_CDR_RID = Include.UndefinedCalendarDateRange;
        public int variable2_VariableValueTypeIndex;
        public int variable2_TimeTypeIndex;
        public int variable2_IsTimeTotal;

        public int headerMerchandise_HN_RID;

        public int sortByTypeIndex; //SORT_BY_TYPE_INDEX
        public int sortByFieldIndex; //SORT_BY_FIELD_INDEX


        //public string listValues; 

        public listConstantTypes listConstantType = listConstantTypes.None;
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

            this.elementGroupTypeIndex = GetIntFieldFromDataRow(drCondition, "ELEMENT_GROUP_TYPE_INDEX");

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
            this.variable1_CDR_RID = GetIntFieldFromDataRow(drCondition, "VAR1_CDR_RID");
            this.variable1_VariableValueTypeIndex = GetIntFieldFromDataRow(drCondition, "VAR1_VALUE_TYPE_INDEX");
            this.variable1_TimeTypeIndex = GetIntFieldFromDataRow(drCondition, "VAR1_TIME_INDEX");

            this.operatorVariablePercentageIndex = GetIntFieldFromDataRow(drCondition, "VAR_PERCENTAGE_OPERATOR_INDEX");

            this.variable2_Index = GetIntFieldFromDataRow(drCondition, "VAR2_VARIABLE_INDEX");
            this.variable2_VersionIndex = GetIntFieldFromDataRow(drCondition, "VAR2_VERSION_INDEX");
            this.variable2_HN_RID = GetIntFieldFromDataRow(drCondition, "VAR2_HN_RID");
            this.variable2_CDR_RID = GetIntFieldFromDataRow(drCondition, "VAR2_CDR_RID");
            this.variable2_VariableValueTypeIndex = GetIntFieldFromDataRow(drCondition, "VAR2_VALUE_TYPE_INDEX");
            this.variable2_TimeTypeIndex = GetIntFieldFromDataRow(drCondition, "VAR2_TIME_INDEX");

            this.headerMerchandise_HN_RID = GetIntFieldFromDataRow(drCondition, "HEADER_HN_RID");

            this.sortByTypeIndex = GetIntFieldFromDataRow(drCondition, "SORT_BY_TYPE_INDEX");
            this.sortByFieldIndex = GetIntFieldFromDataRow(drCondition, "SORT_BY_FIELD_INDEX");


            this.listConstantType = listConstantTypes.FromIndex(GetIntFieldFromDataRow(drCondition, "LIST_VALUE_CONSTANT_INDEX"));

            this.dtListValues = GetListValuesDataTable();
        }

        public void SaveToDataRow(ref DataRow drCondition)
        {
            drCondition["CONDITION_RID"] = this.conditionRID;
            drCondition["FILTER_RID"] = this.conditionFilterRID;

            drCondition["SEQ"] = this.Seq;
            drCondition["PARENT_SEQ"] = this.ParentSeq;
            drCondition["SIBLING_SEQ"] = this.SiblingSeq;

            drCondition["ELEMENT_GROUP_TYPE_INDEX"] = this.elementGroupTypeIndex;

            drCondition["LOGIC_INDEX"] = this.logicIndex;
            drCondition["OPERATOR_INDEX"] = this.operatorIndex;
            drCondition["FIELD_INDEX"] = this.fieldIndex;

            drCondition["VALUE_TYPE_INDEX"] = this.valueTypeIndex;
            drCondition["DATE_TYPE_INDEX"] = this.dateTypeIndex;
            drCondition["NUMERIC_TYPE_INDEX"] = this.numericTypeIndex;

            drCondition["VALUE_TO_COMPARE"] = this.valueToCompare;

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
            if (this.valueToCompareBool == true)
            {
                drCondition["VALUE_TO_COMPARE_BOOL"] = 1;
            }
            else
            {
                drCondition["VALUE_TO_COMPARE_BOOL"] = 0;
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
            drCondition["VALUE_TO_COMPARE_DATE_DAYS_FROM"] = this.valueToCompareDateBetweenFromDays;
            drCondition["VALUE_TO_COMPARE_DATE_DAYS_TO"] = this.valueToCompareDateBetweenToDays;


            drCondition["VAR1_VARIABLE_INDEX"] = this.variable1_Index;
            drCondition["VAR1_VERSION_INDEX"] = this.variable1_VersionIndex;
            drCondition["VAR1_HN_RID"] = this.variable1_HN_RID;
            drCondition["VAR1_CDR_RID"] = this.variable1_CDR_RID;
            drCondition["VAR1_VALUE_TYPE_INDEX"] = this.variable1_VariableValueTypeIndex;
            drCondition["VAR1_TIME_INDEX"] = this.variable1_TimeTypeIndex;

            drCondition["VAR_PERCENTAGE_OPERATOR_INDEX"] = this.operatorVariablePercentageIndex;

            drCondition["VAR2_VARIABLE_INDEX"] = this.variable2_Index;
            drCondition["VAR2_VERSION_INDEX"] = this.variable2_VersionIndex;
            drCondition["VAR2_HN_RID"] = this.variable2_HN_RID;
            drCondition["VAR2_CDR_RID"] = this.variable2_CDR_RID;
            drCondition["VAR2_VALUE_TYPE_INDEX"] = this.variable2_VariableValueTypeIndex;
            drCondition["VAR2_TIME_INDEX"] = this.variable2_TimeTypeIndex;

            drCondition["HEADER_HN_RID"] = this.headerMerchandise_HN_RID;

            drCondition["SORT_BY_TYPE_INDEX"] = this.sortByTypeIndex;
            drCondition["SORT_BY_FIELD_INDEX"] = this.sortByFieldIndex;

            drCondition["LIST_VALUE_CONSTANT_INDEX"] = this.listConstantType.dbIndex;
        }
        public static DataTable GetListValuesDataTable()
        {
            DataTable dtListValues = new DataTable("listvalues");
            //dtListValues.Columns.Add("LIST_VALUE_RID", typeof(int));
            dtListValues.Columns.Add("CONDITION_RID", typeof(int));
            dtListValues.Columns.Add("LIST_VALUE_TYPE_INDEX", typeof(int));
            dtListValues.Columns.Add("LIST_VALUE_INDEX", typeof(int));
            return dtListValues;
        }
        public static string GetListValueIndexField()
        {
            return "LIST_VALUE_INDEX";
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
        public void SaveListValues(DataTable dt, listValueTypes listValueType)
        {
            this.dtListValues.Rows.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                DataRow drNew = dtListValues.NewRow();
                drNew["CONDITION_RID"] = this.conditionRID;
                drNew["LIST_VALUE_TYPE_INDEX"] = listValueType.Index;
                drNew["LIST_VALUE_INDEX"] = dr["LIST_VALUE_INDEX"];
                dtListValues.Rows.Add(drNew);
            }
        }
        public DataRow[] GetListValues(listValueTypes listValueType)
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
        private DateTime GetDateFieldFromDataRow(DataRow drCondition, string field)
        {
            if (drCondition[field] != DBNull.Value)
            {
                return (DateTime)drCondition[field];
            }
            else
            {
                return DateTime.Parse("1/1/2001");
            }
        }
        private bool GetBoolFieldFromDataRow(DataRow drCondition, string field)
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
                return false;
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

    #region FilterNameCombo Class
    /// <summary>
    /// Class that defines the contents of the FilterName combo box.
    /// </summary>

    public class FilterNameCombo
    {
        //=======
        // FIELDS
        //=======

        private int _filterRID;
        private int _userRID;
        private string _filterName;
        private string _displayName;
        // Begin TT#1125 - JSmith - Global/User should be consistent
        //SecurityAdmin secAdmin; //TT#827-MD -jsobek -Allocation Reviews Performance
        // End TT#1125

        //=============
        // CONSTRUCTORS
        //=============

        public FilterNameCombo(int aFilterRID)
        {
            _filterRID = aFilterRID;
        }

        // Begin TT#1125 - JSmith - Global/User should be consistent
        //public FilterNameCombo(int aFilterRID, int aUserRID, string aFilterName)
        //{
        //    _filterRID = aFilterRID;
        //    _userRID = aUserRID;
        //    _filterName = aFilterName;
        //    if (aUserRID == Include.GlobalUserRID) // Issue 3806
        //    {
        //        _displayName = _filterName;
        //    }
        //    else
        //    {
        //        _displayName = _filterName + " (User)";
        //    }
        //}
        public FilterNameCombo(int aFilterRID, int aUserRID, string aFilterName)
        {
            _filterRID = aFilterRID;
            _userRID = aUserRID;
            _filterName = aFilterName;
            if (aUserRID == Include.GlobalUserRID) // Issue 3806
            {
                _displayName = _filterName;
            }
            else
            {
                //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                //secAdmin = new SecurityAdmin();
                //_displayName = _filterName + " (" + secAdmin.GetUserName(aUserRID) + ")";
                _displayName = _filterName + " (" + UserNameStorage.GetUserName(aUserRID) + ")";
                //End TT#827-MD -jsobek -Allocation Reviews Performance
            }
        }
        // End TT#1125

        //===========
        // PROPERTIES
        //===========

        public int FilterRID
        {
            get
            {
                return _filterRID;
            }
        }

        public int UserRID
        {
            get
            {
                return _userRID;
            }
        }

        public string FilterName
        {
            get
            {
                return _filterName;
            }
        }

        //========
        // METHODS
        //========

        override public string ToString()
        {
            return _displayName;
        }

        override public bool Equals(object obj)
        {
            if (((FilterNameCombo)obj).FilterRID == _filterRID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        override public int GetHashCode()
        {
            return _filterRID;
        }
    }
    #endregion

    #region FilterDragObject
    /// <summary>
    /// Class that defines the FilterDragObject, which is a generic object used during drag events.
    /// </summary>

    public class FilterDragObject
    {
        //=======
        // FIELDS
        //=======

        public object DragObject;
        public string Text;

        //=============
        // CONSTRUCTORS
        //=============

        public FilterDragObject(object aDragObject)
        {
            DragObject = aDragObject;
            Text = null;
        }

        public FilterDragObject(object aDragObject, string aText)
        {
            DragObject = aDragObject;
            Text = aText;
        }

        //===========
        // PROPERTIES
        //===========

        //========
        // METHODS
        //========
    }
    #endregion
}
