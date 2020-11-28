//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using MIDRetail.Business;

//namespace MIDRetail.Windows.Controls
//{
//    public static class filterEngine
//    {

//        public static bool RunFilter(filter f)
//        {
//            bool finalResult = false;


//            //assuming and-also (&&) and or-else (||) for logic blocks




//            //get siblings
//            ConditionNode cn = f.FindConditionNode(2); //parent seq for conditions


//            //compare siblings cost to run
//            if (cn.ConditionNodes.Count == 1)
//            {
//                finalResult = ExecuteCondition(cn.condition);
//            }
//            else if (cn.ConditionNodes.Count > 1)
//            {
//                List<costClass> costList = new List<costClass>();
//                foreach (ConditionNode n in cn.ConditionNodes)
//                {
//                    costClass cc = new costClass();
//                    cc.seq = n.condition.Seq;
//                    cc.cost = f.GetCost(n.condition.Seq);
//                    if (n.condition.logicIndex == logicTypes.Or.Index)  //??
//                    {
//                        cc.isOR = true;
//                    }
//                    else
//                    {
//                        cc.isOR = false;
//                    }
//                    costList.Add(cc);
//                }
//                IEnumerable<costClass> query = costList.OrderBy(costClass => costClass.cost);


//                //make groups based on logical operators AND / OR
//                List<costGroup> groupList = new List<costGroup>();
//                costGroup currentGroup = new costGroup();
//                foreach (costClass c in query)
//                {
//                    if (c.isOR)
//                    {
//                        //make new group
//                        if (currentGroup.group.Count > 0) //special check for first condition
//                        {
//                            int gc2 = 0;
//                            foreach (costClass cc in currentGroup.group)
//                            {
//                                gc2 += cc.cost;
//                            }
//                            currentGroup.groupCost = gc2;
//                            groupList.Add(currentGroup);
//                        }
//                        currentGroup = new costGroup();
//                    }

//                    currentGroup.group.Add(c);
//                }
//                int gc = 0;
//                foreach (costClass cc in currentGroup.group)
//                {
//                    gc += cc.cost;
//                }
//                groupList.Add(currentGroup);

//                //costList.Sort();
//                IEnumerable<costGroup> queryGroup = groupList.OrderBy(costGroup => costGroup.groupCost);



//                //write groups to conditions for debugging;
//                foreach (costGroup cg in queryGroup)
//                {
//                    foreach (costClass cst in cg.group)
//                    {
//                        ConditionNode c = f.FindConditionNode(cst.seq);
//                        c.condition.executeGroup = groupList.IndexOf(cg).ToString();
//                    }
//                }



//                bool result = true;
//                foreach (costGroup cg in queryGroup)
//                {
//                    result = true;
//                    foreach (costClass cst in cg.group)
//                    {
//                        ConditionNode c = f.FindConditionNode(cst.seq);
//                        result = ExecuteCondition(c.condition);
//                        if (result == false)
//                        {
//                            break;
//                        }
//                    }
//                    if (result == true)
//                    {
//                        break;
//                    }
//                }
//                finalResult = result;
//            }

//            //execute the first sibling

//            return finalResult;
//        }

//        private static bool ExecuteCondition(filterCondition fc)
//        {
//            fc.executed = "Y";
//            fc.executeResult = "True";
//            return true;
//        }


//    }

//    public class costGroup
//    {
//        public List<costClass> group = new List<costClass>();
//        public int groupCost;
//    }

//    public class costClass //: IComparable<costClass>
//    {
//        public int cost;
//        public int seq;
//        public bool isOR;

//        public int CompareTo(costClass other)
//        {
//            return cost.CompareTo(other.cost);
//        }

//    }
//}
