using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class filterBuilderList : UserControl
    {
        public filterBuilderList()
        {
            InitializeComponent();
            this.ultraToolbarsManager1.Toolbars[0].Visible = false;
        }

        private filterManager manager;
        private List<filterBuilderListNodeContainer> nodeContainerList = new List<filterBuilderListNodeContainer>();

        public filterBuilderListNode selectedNode = null;
        public filterBuilderListNodeContainer selectedNodeContainer = null;

        public void LoadFilter(filterManager manager)
        {
            this.manager = manager;
            manager.AddConditionEvent += new filterManager.AddConditionEventHandler(Handle_AddCondition);
            manager.RemoveConditionEvent += new filterManager.RemoveConditionEventHandler(Handle_RemoveCondition);
            manager.ConditionHasBeenUpdatedEvent += new filterManager.ConditionHasBeenUpdatedEventHandler(Handle_ConditionHasBeenUpdated);

            RenderNodes();
            //select name node
            SelectNode(manager.currentFilter.GetNameConditionSeq());
        }

        private void ultraToolbarsManager1_Toolclick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            // Begin TT#5790 - JSmith - Creating attribute can cause application to crash
			try
            {
			// End TT#5790 - JSmith - Creating attribute can cause application to crash
                switch (e.Tool.Key)
                {
                    case "btnUp":
                        MoveUp();
                        break;
                    case "btnDown":
                        MoveDown();
                        break;
                    case "btnLeft":
                        MoveLeft();
                        break;
                    case "btnRight":
                        MoveRight();
                        break;
                    case "btnRemove":
                        Remove();
                        break;
                }
            // Begin TT#5790 - JSmith - Creating attribute can cause application to crash
			}
            catch (Exception ex)
            {
                string err = "Error executing button " + e.Tool.Key + ": " + ex.ToString();
                ExceptionHandler.HandleException(err);
            }
			// End TT#5790 - JSmith - Creating attribute can cause application to crash
        }
        private void Remove()
        {
            manager.RemoveCondition();
        }
        int letterIndex;
        public void RenderNodes()
        {
            letterIndex = 0;
            ClearUIContainerControls();
            foreach (ConditionNode n in manager.currentFilter.RootConditionNode.ConditionNodes)
            {
                RenderNode(n, false);
            }
            //keep the selected node shown as selected
            if (selectedNode != null)
            {
                selectedNodeContainer = FindUIControl(selectedNode.conditionNode.condition.Seq);
                selectedNode = selectedNodeContainer.uiNode;
                selectedNode.IsSelected = true;
            }
        }

        public void RenderNode(ConditionNode n, bool doSelect)
        {
            string letter = string.Empty;
            filterDictionary et = filterDictionary.FromIndex(n.condition.dictionaryIndex);
            if (et.costToRunEstimate > 0)
            {
                letterIndex++;
                char cPrefix = (char)(letterIndex + 64);
                letter = cPrefix.ToString() + " ";
            }
           
            filterBuilderListNode ln = new filterBuilderListNode(this.manager, n, letter);
            filterBuilderListNodeContainer nc = new filterBuilderListNodeContainer(manager, ln);
            nodeContainerList.Add(nc);
            nc.SetPosition();  //Sets the indentation in the container control

            AddUIContainerToFlowPanel(nc);

            if (doSelect)
            {
                ln.IsSelected = true;
                SelectNewNode(ln);
            }
            ln.NodeClickedEvent += new filterBuilderListNode.NodeClickedEventHandler(Handle_NodeClicked);
            ln.KeyPressDownEvent += new filterBuilderListNode.KeyPressDownEventHandler(Handle_NodeKeyPressDown);

            foreach (ConditionNode c in n.ConditionNodes)
            {
                RenderNode(c, false);
            }
        }

        private void AddUIContainerToFlowPanel(filterBuilderListNodeContainer c)
        {
            this.flowLayoutPanel1.Controls.Add(c);
        }

        private void SetUIContainerChildIndex(filterBuilderListNodeContainer c, int childIndex)
        {
            this.flowLayoutPanel1.Controls.SetChildIndex(c, childIndex);
        }

        private int GetUIContainerIndex(filterBuilderListNodeContainer c)
        {
            return this.flowLayoutPanel1.Controls.IndexOf(c);
        }

        private void RemoveUIContainerFromFlowPanel(filterBuilderListNodeContainer c)
        {
            this.flowLayoutPanel1.Controls.Remove(c);
        }

        private void ClearUIContainerControls()
        {
            // Begin TT#5790 - JSmith - Creating attribute can cause application to crash
            // Objects are not getting collected.  Explicitly dispose after removed from lists
            List<filterBuilderListNodeContainer> controls = new List<filterBuilderListNodeContainer>();
            foreach (filterBuilderListNodeContainer nc in nodeContainerList)
            {
                controls.Add(nc);
            }
            // End TT#5790 - JSmith - Creating attribute can cause application to crash

            this.flowLayoutPanel1.Controls.Clear();
            this.nodeContainerList.Clear();

            // Begin TT#5790 - JSmith - Creating attribute can cause application to crash
            foreach (filterBuilderListNodeContainer nc in controls)
            {
                nc.Dispose();
            }
            // End TT#5790 - JSmith - Creating attribute can cause application to crash
        }

        public filterBuilderListNodeContainer FindUIControl(int seq)
        {
            filterBuilderListNodeContainer nFound = null;
            bool foundNode = false;
            foreach (filterBuilderListNodeContainer nc in nodeContainerList)
            {
                if (foundNode == false)
                {
                    if (nc.uiNode.conditionNode.condition.Seq == seq)
                    {
                        foundNode = true;
                        nFound = nc;
                    }
                }
            }
            return nFound;
        }

        private void Handle_AddCondition(object sender, filterManager.AddConditionEventArgs e)
        {
            RenderNodes();
            SelectNode(e.conditionNodeToAdd.condition.Seq);
        }
        public void SelectNode(int seq)
        {
            filterBuilderListNodeContainer c = FindUIControl(seq);
            if (c != null)
            {
                this.ultraToolbarsManager1.Toolbars[0].Visible = true;
                c.uiNode.IsSelected = true;
                SelectNewNode(c.uiNode);
            }
        }


        private void Handle_RemoveCondition(object sender, filterManager.RemoveConditionEventArgs e)
        {
            //RenderNode(e.conditionNodeToAdd, true);
            selectedNode = null;
            selectedNodeContainer = null;
            this.manager.selectedConditionNode = null;
            RenderNodes();
            if (e.newSelectedSeq != -1)
            {
                filterBuilderListNodeContainer c = FindUIControl(e.newSelectedSeq);
                c.uiNode.IsSelected = true;
                SelectNewNode(c.uiNode);
            }
            else
            {
                this.ultraToolbarsManager1.Toolbars[0].Visible = false;
            }
        }

        private void Handle_ConditionHasBeenUpdated(object sender, filterManager.ConditionHasBeenUpdatedEventArgs e)
        {
            selectedNode.conditionNode.condition = e.condition;
            selectedNode.ShowFormattedText();

        }

        private void Handle_NodeClicked(object sender, filterBuilderListNode.NodeClickedEventArgs e)
        {
            SelectNewNode(e.SelectedNode);
        }
        private void Handle_NodeKeyPressDown(object sender, filterBuilderListNode.KeyPressDownEventArgs e)
        {
            filterBuilderListNode focusedNode = (filterBuilderListNode)sender;
            filterBuilderListNodeContainer c = FindUIControl(focusedNode.conditionNode.condition.Seq);
            int containerIndex = nodeContainerList.IndexOf(c);


            if (e.KeyCode == Keys.Up)
            {
                if (containerIndex != 0)
                {
                    filterBuilderListNodeContainer cPrev = nodeContainerList[containerIndex - 1];
                    cPrev.uiNode.Enter();
                }
                //filterBuilderListNode focusedNode = (filterBuilderListNode)sender;
                //bool hasSiblingPrev = manager.currentFilter.HasPreviousSibling(focusedNode.conditionNode);
                //if (hasSiblingPrev)
                //{
                //    //if (manager.IsOKToSelectNewCondition())
                //    //{
                //    //    ConditionNode nPrev = manager.currentFilter.GetPreviousSibling(focusedNode.conditionNode);
                //    //    filterBuilderListNodeContainer c = FindUIControl(nPrev.condition.conditionSeq);
                //    //    c.uiNode.IsSelected = true;
                //    //    SelectNewNode(c.uiNode);
                //    //}

                //    ConditionNode nPrev = manager.currentFilter.GetPreviousSibling(focusedNode.conditionNode);
                //    filterBuilderListNodeContainer c = FindUIControl(nPrev.condition.conditionSeq);
                //    c.uiNode.Enter();
                //}
                //else
                //{
                //    //goto parent
                //    if (focusedNode.conditionNode.IsRootLevel == false)
                //    {
                //        ConditionNode nParent = focusedNode.conditionNode.Parent;
                //        filterBuilderListNodeContainer c = FindUIControl(nParent.condition.conditionSeq);
                //        c.uiNode.Enter();
                //    }
                //}
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (containerIndex != nodeContainerList.Count - 1)
                {
                    filterBuilderListNodeContainer cNext = nodeContainerList[containerIndex + 1];
                    cNext.uiNode.Enter();
                }
                //filterBuilderListNode focusedNode = (filterBuilderListNode)sender;
                //bool hasSiblingNext = manager.currentFilter.HasNextSibling(focusedNode.conditionNode);
                //if (focusedNode.conditionNode.ConditionNodes.Count > 0)
                //{
                //    ConditionNode nFirstChild = focusedNode.conditionNode.ConditionNodes[0];
                //    filterBuilderListNodeContainer c = FindUIControl(nFirstChild.condition.conditionSeq);
                //    c.uiNode.Enter();
                //}
                //else
                //{
                //    //goto next sibling
                //    if (hasSiblingNext)
                //    {
                //        //if (manager.IsOKToSelectNewCondition())
                //        //{
                //        //    ConditionNode nNext = manager.currentFilter.GetNextSibling(focusedNode.conditionNode);
                //        //    filterBuilderListNodeContainer c = FindUIControl(nNext.condition.conditionSeq);
                //        //    c.uiNode.IsSelected = true;
                //        //    SelectNewNode(c.uiNode);
                //        //}
                //        ConditionNode nNext = manager.currentFilter.GetNextSibling(focusedNode.conditionNode);
                //        filterBuilderListNodeContainer c = FindUIControl(nNext.condition.conditionSeq);
                //        c.uiNode.Enter();
                //    }
                //}
            }
            //else if (e.KeyCode == Keys.Left)
            //{
            //    filterBuilderListNode focusedNode = (filterBuilderListNode)sender;
            //    if (focusedNode.conditionNode.IsRootLevel == false)
            //    {
            //        ConditionNode nParent = focusedNode.conditionNode.Parent;
            //        filterBuilderListNodeContainer c = FindUIControl(nParent.condition.conditionSeq);
            //        c.uiNode.Enter();                  
            //    }
            //}
            //else if (e.KeyCode == Keys.Right)
            //{
            //    filterBuilderListNode focusedNode = (filterBuilderListNode)sender;

            //    if (focusedNode.conditionNode.ConditionNodes.Count > 0)
            //    {
            //        ConditionNode nFirstChild = focusedNode.conditionNode.ConditionNodes[0];
            //        filterBuilderListNodeContainer c = FindUIControl(nFirstChild.condition.conditionSeq);
            //        c.uiNode.Enter();
            //    }
            //}
        }

        private void SelectNewNode(filterBuilderListNode newUINode)
        {
            if (selectedNode != null && selectedNode != newUINode)
            {
                selectedNode.IsSelected = false;
            }
            selectedNode = newUINode;
            selectedNodeContainer = FindUIControl(selectedNode.conditionNode.condition.Seq);
            this.manager.selectedConditionNode = selectedNode.conditionNode;
            this.manager.SelectedConditionChanged(selectedNode.conditionNode.condition);
            CheckArrows();
            this.ultraToolbarsManager1.Tools["btnRemove"].SharedProps.Enabled = CanRemove();
        }

        private void MoveLeft()
        {
            //if (selectedNode.conditionNode.IsRootLevel == false)
            //{
            ConditionNode cn = manager.currentFilter.FindConditionNode(selectedNode.conditionNode.condition.Seq);

            ConditionNode oldParent = cn.Parent;
            //set new parent seq
            int newParentSeq = cn.Parent.condition.ParentSeq;
            cn.condition.ParentSeq = newParentSeq;

            //rather than insert at the end of the parents children, insert just one down
            //cn.condition.conditionSiblingSeq = manager.currentFilter.GetNextSiblingSeq(newParentSeq);
            cn.condition.SiblingSeq = cn.Parent.condition.SiblingSeq + 1;


            cn.Parent.ConditionNodes.Remove(cn);
            cn.Parent = cn.Parent.Parent;
            cn.Parent.ConditionNodes.Insert(cn.condition.SiblingSeq - 1, cn);
            manager.UpdateSiblingSequences(cn.Parent);
            manager.UpdateSiblingSequences(oldParent);

            //update and redraw
            manager.currentFilter.UpdateCondition(cn.condition);
            RenderNodes();
            CheckArrows();
            //}
        }



        private bool CanMoveLeft()
        {
            filterNavigationTypes navType = filterDictionary.FromIndex(this.selectedNode.conditionNode.condition.dictionaryIndex).navigationType;

            if (this.manager.readOnly == true)
            {
                return false;
            }

            if (this.selectedNode.conditionNode.condition.dictionaryIndex == filterDictionary.StoreGroupExclusionList || this.selectedNode.conditionNode.condition.dictionaryIndex == filterDictionary.StoreGroupName || this.selectedNode.conditionNode.condition.dictionaryIndex == filterDictionary.StoreGroupDynamic)
            {
                return false;
            }

            // Begin TT#4559 - JSmith - Filter permissions - Disable arrow and remove buttons if read only
            //if (navType == filterNavigationTypes.Info || navType == filterNavigationTypes.SortBy)
            if (navType == filterNavigationTypes.Info || navType == filterNavigationTypes.SortBy || manager.readOnly)
            // End TT#4559 - JSmith - Filter permissions - Disable arrow and remove buttons if read only
            {
                return false;
            }

            

            if (navType == filterNavigationTypes.Condition && selectedNode.conditionNode.condition.ParentSeq != 2)
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        private void MoveRight()
        {
            //bool hasSiblingPrev = manager.currentFilter.HasPreviousSibling(this.selectedNode.conditionNode);
            //if (hasSiblingPrev)
            //{
            ConditionNode prev = manager.currentFilter.GetPreviousSibling(this.selectedNode.conditionNode);
            ConditionNode cn = manager.currentFilter.FindConditionNode(selectedNode.conditionNode.condition.Seq);

            //set new parent seq
            int newParentSeq = prev.condition.Seq;

            ConditionNode oldParent = cn.Parent;
            cn.condition.ParentSeq = newParentSeq;
            cn.condition.SiblingSeq = manager.currentFilter.GetNextSiblingSeq(newParentSeq);


            cn.Parent.ConditionNodes.Remove(cn);
            cn.Parent = prev;
            cn.Parent.ConditionNodes.Add(cn);

            //update and redraw
            manager.currentFilter.UpdateCondition(cn.condition);
            manager.UpdateSiblingSequences(oldParent);
            RenderNodes();
            CheckArrows();
            //}
        }

        private bool CanMoveRight()
        {
            filterNavigationTypes navType = filterDictionary.FromIndex(this.selectedNode.conditionNode.condition.dictionaryIndex).navigationType;

            if (this.manager.readOnly == true)
            {
                return false;
            }

            if (this.selectedNode.conditionNode.condition.dictionaryIndex == filterDictionary.StoreGroupExclusionList || this.selectedNode.conditionNode.condition.dictionaryIndex == filterDictionary.StoreGroupName || this.selectedNode.conditionNode.condition.dictionaryIndex == filterDictionary.StoreGroupDynamic)
            {
                return false;
            }

            // Begin TT#4559 - JSmith - Filter permissions - Disable arrow and remove buttons if read only
            //if (navType == filterNavigationTypes.Info || navType == filterNavigationTypes.SortBy)
            if (navType == filterNavigationTypes.Info || navType == filterNavigationTypes.SortBy || manager.readOnly)
            // End TT#4559 - JSmith - Filter permissions - Disable arrow and remove buttons if read only
            {
                return false;
            }

            bool hasSiblingPrev = manager.currentFilter.HasPreviousSibling(this.selectedNode.conditionNode);
            if (hasSiblingPrev)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void MoveUp()
        {
            //bool hasSiblingPrev = manager.currentFilter.HasPreviousSibling(this.selectedNode.conditionNode);
            //if (hasSiblingPrev)
            //{
            ConditionNode cn = manager.currentFilter.FindConditionNode(selectedNode.conditionNode.condition.Seq);

            //swap with previous sibling
            ConditionNode prev = manager.currentFilter.GetPreviousSibling(cn);
            prev.condition.SiblingSeq = cn.condition.SiblingSeq;
            cn.condition.SiblingSeq = cn.condition.SiblingSeq - 1;
            cn.Parent.ConditionNodes.Remove(cn);
            cn.Parent.ConditionNodes.Insert(cn.condition.SiblingSeq - 1, cn);
            //update and redraw
            manager.currentFilter.UpdateCondition(prev.condition);
            manager.currentFilter.UpdateCondition(cn.condition);

            RenderNodes();
            CheckArrows();
            //}
        }

        private bool CanMoveUp()
        {
            filterNavigationTypes navType = filterDictionary.FromIndex(this.selectedNode.conditionNode.condition.dictionaryIndex).navigationType;

            if (this.manager.readOnly == true)
            {
                return false;
            }

            // Begin TT#4559 - JSmith - Filter permissions - Disable arrow and remove buttons if read only
            //if (navType == filterNavigationTypes.Info)
            if (navType == filterNavigationTypes.Info || manager.readOnly)
            // End TT#4559 - JSmith - Filter permissions - Disable arrow and remove buttons if read only
            {
                return false;
            }

            if (this.selectedNode.conditionNode.condition.dictionaryIndex == filterDictionary.StoreGroupExclusionList)
            {
                return false;
            }

            bool hasSiblingPrev = manager.currentFilter.HasPreviousSibling(this.selectedNode.conditionNode);
            if (hasSiblingPrev)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void MoveDown()
        {
            //bool hasSiblingNext = manager.currentFilter.HasNextSibling(this.selectedNode.conditionNode);
            //if (hasSiblingNext)
            //{
            ConditionNode cn = manager.currentFilter.FindConditionNode(selectedNode.conditionNode.condition.Seq);

            //swap with next sibling
            ConditionNode next = manager.currentFilter.GetNextSibling(cn);
            next.condition.SiblingSeq = cn.condition.SiblingSeq;
            cn.condition.SiblingSeq = cn.condition.SiblingSeq + 1;
            cn.Parent.ConditionNodes.Remove(cn);
            cn.Parent.ConditionNodes.Insert(cn.condition.SiblingSeq - 1, cn);
            //update and redraw
            manager.currentFilter.UpdateCondition(next.condition);
            manager.currentFilter.UpdateCondition(cn.condition);
            RenderNodes();
            CheckArrows();
            //}
        }

        private bool CanMoveDown()
        {
            filterNavigationTypes navType = filterDictionary.FromIndex(this.selectedNode.conditionNode.condition.dictionaryIndex).navigationType;

            if (this.manager.readOnly == true)
            {
                return false;
            }

            // Begin TT#4559 - JSmith - Filter permissions - Disable arrow and remove buttons if read only
            //if (navType == filterNavigationTypes.Info)
            if (navType == filterNavigationTypes.Info || manager.readOnly)
            // End TT#4559 - JSmith - Filter permissions - Disable arrow and remove buttons if read only
            {
                return false;
            }

            if (this.selectedNode.conditionNode.condition.dictionaryIndex == filterDictionary.StoreGroupExclusionList)
            {
                return false;
            }


            bool hasSiblingNext = manager.currentFilter.HasNextSibling(this.selectedNode.conditionNode);
            if (hasSiblingNext)
            {
                if (this.selectedNode.conditionNode.condition.dictionaryIndex == filterDictionary.StoreGroupName || this.selectedNode.conditionNode.condition.dictionaryIndex == filterDictionary.StoreGroupDynamic)
                {
                    ConditionNode next = manager.currentFilter.GetNextSibling(this.selectedNode.conditionNode);
                    if (next.condition.dictionaryIndex == filterDictionary.StoreGroupExclusionList)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private void CheckArrows()
        {
            //check moving arrows
            this.ultraToolbarsManager1.Tools["btnUp"].SharedProps.Enabled = CanMoveUp();
            this.ultraToolbarsManager1.Tools["btnDown"].SharedProps.Enabled = CanMoveDown();
            this.ultraToolbarsManager1.Tools["btnLeft"].SharedProps.Enabled = CanMoveLeft();
            this.ultraToolbarsManager1.Tools["btnRight"].SharedProps.Enabled = CanMoveRight();
        }
        private bool CanRemove()
        {
            filterNavigationTypes navType = filterDictionary.FromIndex(this.selectedNode.conditionNode.condition.dictionaryIndex).navigationType;

            // Begin TT#4559 - JSmith - Filter permissions - Disable arrow and remove buttons if read only
            if (this.manager.readOnly == false && navType != filterNavigationTypes.Info && this.selectedNode.conditionNode.condition.dictionaryIndex != filterDictionary.StoreGroupExclusionList && !manager.readOnly)
            // End TT#4559 - JSmith - Filter permissions - Disable arrow and remove buttons if read only
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
