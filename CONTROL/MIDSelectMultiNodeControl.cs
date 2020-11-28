using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.Layout;
using Infragistics.Win.UltraWinTree;

namespace MIDRetail.Windows.Controls
{
    public partial class MIDSelectMultiNodeControl : UserControl
    {
        public MIDSelectMultiNodeControl()
        {
            InitializeComponent();
        }


        public bool ShowRootLines
        {
            get 
            { 
                return this.ultraTree1.ShowRootLines; 
            }
            set
            {
                this.ultraTree1.ShowRootLines = value;
            }
        }


        private bool _CheckAllByDefault = false;
        public bool CheckAllByDefault
        {
            get { return _CheckAllByDefault; }
            set
            {
                _CheckAllByDefault = value;
                foreach (UltraTreeNode n in this.ultraTree1.Nodes)
                {
                    if (_CheckAllByDefault)
                        n.CheckedState = CheckState.Checked;
                    else
                        n.CheckedState = CheckState.Unchecked;
                }
            }
        }

        private string _MappingRelationshipColumnKey = string.Empty;
        public string MappingRelationshipColumnKey 
        {
            get { return _MappingRelationshipColumnKey; }
            set { _MappingRelationshipColumnKey = value; }
        }

        private string _Title = string.Empty;
        public string Title 
        { 
            get { return _Title;}
            set { _Title = value; this.ultraToolbarsManager1.Tools["lblTitle"].SharedProps.Caption = _Title; }
        }

        private string _FieldToTag = string.Empty;
        public string FieldToTag
        {
            get { return _FieldToTag; }
            set { _FieldToTag = value; }
        }
        private string _ParentFieldToTag = string.Empty;
        public string ParentFieldToTag
        {
            get { return _ParentFieldToTag; }
            set { _ParentFieldToTag = value; }
        }

        private string _FieldToDisplay = string.Empty;
        public string FieldToDisplay
        {
            get { return _FieldToDisplay; }
            set { _FieldToDisplay = value; }
        }

        private bool _LoadingSelected = false;

        private void MIDSelectMultiNodeControl_Load(object sender, EventArgs e)
        {
            
        }

        public void BindDataSet(DataSet ds)
        {

            this.ultraTree1.ViewStyle = Infragistics.Win.UltraWinTree.ViewStyle.Standard;
            this.ultraTree1.Override.NodeStyle = Infragistics.Win.UltraWinTree.NodeStyle.CheckBox;

            this.ultraTree1.Override.SelectionType = Infragistics.Win.UltraWinTree.SelectType.Extended;

            //	Set 'AutoGenerateColumnSets' to true so that UltraTreeCOlumnSets are automatically generated
            this.ultraTree1.ColumnSettings.AutoGenerateColumnSets = true;

            //	Set 'SynchronizeCurrencyManager' to false to optimize performance
            this.ultraTree1.SynchronizeCurrencyManager = false;

            //	Set 'ViewStyle' to OutlookExpress for a relational display
            //this.ultraTree1.ViewStyle = ViewStyle.OutlookExpress;

            //	Set 'AutoFitColumns' to true to automatically fit all columns
            this.ultraTree1.ColumnSettings.AutoFitColumns = AutoFitColumns.ResizeAllColumns;

            //	Set DataSource/DataMember to bind the UltraTree
            //this.ultraTree1.DataSource = null;
            this.ultraTree1.DataSource = ds;
            this.ultraTree1.DataMember = ds.Tables[0].TableName;

            SetAllNodeTags();
            //this.dt = ds.Tables[0];
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnCheckAll":
                    NodesCheckAll();
                    break;
                case "btnUncheckAll":
                    NodesUnCheckAll();
                    break;

            }
        }

        private void ultraTree1_AfterCheck(object sender, NodeEventArgs e)
        {
            if (_LoadingSelected == false)
            {
                foreach (UltraTreeNode n in e.TreeNode.Nodes)
                {
                    n.CheckedState = e.TreeNode.CheckedState;
                }
            }
        }



        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodesCheckAll();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodesUnCheckAll();
        }
        public void NodesCheckAll()
        {
            foreach (UltraTreeNode n in this.ultraTree1.Nodes)
            {
                //n.CheckedState = CheckState.Checked;
                NodeChangeCheckState(n, CheckState.Checked);
            }
        }
        public void NodesUnCheckAll()
        {
            foreach (UltraTreeNode n in this.ultraTree1.Nodes)
            {
                //n.CheckedState = CheckState.Unchecked;
                NodeChangeCheckState(n, CheckState.Unchecked);
            }
        }

        private void NodeChangeCheckState(UltraTreeNode n, CheckState newCheckState)
        {
            if (_LoadingSelected == false)
            {
                n.CheckedState = newCheckState;
                foreach (UltraTreeNode c in n.Nodes)
                {
                    NodeChangeCheckState(c, newCheckState);
                }
            }
        }

        public bool IsEveryNodeSelected()
        {
            bool isEverythingSelected = true;
            foreach (UltraTreeNode n in this.ultraTree1.Nodes)
            {
                isEverythingSelected = NodeIsEveryNodeSelected(n);

                if (isEverythingSelected == false)
                    break;

            }
            return isEverythingSelected;
        }

        private bool NodeIsEveryNodeSelected(UltraTreeNode n)
        {
            bool isEverythingSelected = true;

            if (n.CheckedState != CheckState.Checked)
            {
                return false;
            }

            foreach (UltraTreeNode c in n.Nodes)
            {
                isEverythingSelected = NodeIsEveryNodeSelected(c);

                if (isEverythingSelected == false)
                    break;

            }

            return isEverythingSelected;
        }

  

        private void ultraTree1_ColumnSetGenerated(object sender, ColumnSetGeneratedEventArgs e)
        {
            for (int i = 0; i < e.ColumnSet.Columns.Count; i++)
            {
                UltraTreeNodeColumn column = e.ColumnSet.Columns[i];

                //	Format the DateTime columns with the current culture's ShortDatePattern
                //if (column.DataType == typeof(DateTime))
                //    column.Format = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

                //	Hide the integer columns, which represent record IDs
                //if (column.DataType == typeof(int))
                //    column.Visible = false;

                //	Set the MapToColumn for the relationship ColumnSets
                if (e.ColumnSet.Key == _MappingRelationshipColumnKey)
                    column.MapToColumn = column.Key;

                //	Set the Text property to something more descriptive
                //if (column.Key == "NAME")
                //{
                //    column.Text = "Groups / Users";

                //    //	Make the name column wider
                //    column.LayoutInfo.PreferredLabelSize = new Size(this.ultraTree1.Width / 2, 0);
                //}
                //else
                //    if (column.Key == "PilotDate")
                //    {
                //        column.Text = "First Aired";

                //        //	Center align the text for the first aired date
                //        column.CellAppearance.TextHAlign = HAlign.Center;
                //    }
                //    else
                //        if (column.Key == "Network")
                //            //	Center align the text for the name of the network
                //            column.CellAppearance.TextHAlign = HAlign.Center;
            }

        }

        //private DataTable dt;
        //public void BindDataSet(DataSet ds)
        //{
   
        //    this.ultraTree1.ViewStyle = Infragistics.Win.UltraWinTree.ViewStyle.Standard;
        //    this.ultraTree1.Override.NodeStyle = Infragistics.Win.UltraWinTree.NodeStyle.CheckBox;

        //    this.ultraTree1.Override.SelectionType = Infragistics.Win.UltraWinTree.SelectType.Extended;

        //    //	Set 'AutoGenerateColumnSets' to true so that UltraTreeCOlumnSets are automatically generated
        //    this.ultraTree1.ColumnSettings.AutoGenerateColumnSets = true;

        //    //	Set 'SynchronizeCurrencyManager' to false to optimize performance
        //    this.ultraTree1.SynchronizeCurrencyManager = false;

        //    //	Set 'ViewStyle' to OutlookExpress for a relational display
        //    //this.ultraTree1.ViewStyle = ViewStyle.OutlookExpress;

        //    //	Set 'AutoFitColumns' to true to automatically fit all columns
        //    this.ultraTree1.ColumnSettings.AutoFitColumns = AutoFitColumns.ResizeAllColumns;

        //    //	Set DataSource/DataMember to bind the UltraTree
        //    this.ultraTree1.DataSource = ds;
        //    this.ultraTree1.DataMember = ds.Tables[0].TableName;
        //}

        private void ultraTree1_InitializeDataNode(object sender, InitializeDataNodeEventArgs e)
        {
            e.Node.Expanded = true;
            if (_CheckAllByDefault == true)
            {
                e.Node.CheckedState = CheckState.Checked;
            }
            
        }

        //private void ultraTree1_AfterDataNodesCollectionPopuldated(object sender, AfterDataNodesCollectionPopulatedEventArgs e)
        //{
        //    if (_FieldToTag != string.Empty)
        //    {
        //        foreach (UltraTreeNode n in e.Nodes)
        //        {
        //            SetNodeTags(n);
        //        }
        //    }
        //}

        public void SetAllNodeTags()
        {
            if (_FieldToTag != string.Empty)
            {
                foreach (UltraTreeNode n in this.ultraTree1.Nodes)
                {
                    SetNodeTags(n);
                }
            }
        }

        private void SetNodeTags(UltraTreeNode n)
        {
            if (n.ListObject != null)
            {
                DataRow dr = ((DataRowView)n.ListObject).Row;
                if (dr.Table.Columns.Contains(_FieldToTag) == true)
                {
                    if (_ParentFieldToTag != string.Empty)
                    {
                        n.Tag = dr[_ParentFieldToTag].ToString() + "~" + dr[_FieldToTag].ToString();
                    }
                    else
                    {
                        n.Tag = dr[_FieldToTag];
                    }
                    
                }
                else if (_ParentFieldToTag != string.Empty && dr.Table.Columns.Contains(_ParentFieldToTag) == true)
                {
                    n.Tag = dr[_ParentFieldToTag].ToString();
                }
            }
            foreach (UltraTreeNode c in n.Nodes)
            {
                SetNodeTags(c);
            }
        }

        public string GetSelectedListFromTags(bool includeRootNodes)
        {
            string sList = string.Empty;
            foreach (UltraTreeNode n in this.ultraTree1.Nodes)
            {
                BuildSelectedListFromTags(n, ref sList, includeRootNodes);
            }
            return sList;
        }

        /// <summary>
        /// Recursive function to build a comma delimited list for checked nodes
        /// Assumes the property FieldToTag has been populated
        /// </summary>
        /// <param name="n"></param>
        /// <param name="sList"></param>
        private void BuildSelectedListFromTags(UltraTreeNode n, ref string sList, bool includeRootNodes)
        {
            if ((n.Parent != null || includeRootNodes) && n.CheckedState == CheckState.Checked)
            {
               
                if (n.Tag != null)
                {
                    if (sList == string.Empty)
                    {
                        sList += n.Tag.ToString(); 
                    }
                    else
                    {
                        sList += "," + n.Tag.ToString(); 
                    }
                }  
            }

            foreach (UltraTreeNode c in n.Nodes)
            {
                BuildSelectedListFromTags(c, ref sList, includeRootNodes);
            }

        }
        public DataTable GetSelectedListValuesFromTags(bool includeRootNodes, string fieldForIndex)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(fieldForIndex, typeof(int));
            foreach (UltraTreeNode n in this.ultraTree1.Nodes)
            {
                BuildSelectedListValuesFromTags(n, ref dt, includeRootNodes, fieldForIndex);
            }
            return dt;
        }
        public DataTable GetSelectedListValuesFromTagsWithParent(bool includeRootNodes, string fieldForIndex, string parentFieldIndex)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(fieldForIndex, typeof(int));
            dt.Columns.Add(parentFieldIndex, typeof(int));
            foreach (UltraTreeNode n in this.ultraTree1.Nodes)
            {
                BuildSelectedListValuesFromTagsWithParent(n, ref dt, includeRootNodes, fieldForIndex, parentFieldIndex);
            }
            return dt;
        }

        /// <summary>
        /// Recursive function to build a comma delimited list for checked nodes
        /// Assumes the property FieldToTag has been populated
        /// </summary>
        /// <param name="n"></param>
        /// <param name="sList"></param>
        private void BuildSelectedListValuesFromTags(UltraTreeNode n, ref DataTable dt, bool includeRootNodes, string fieldForIndex)
        {
            if ((n.Parent != null || includeRootNodes) && n.CheckedState == CheckState.Checked)
            {

                if (n.Tag != null)
                {
                    int rid;
                    int.TryParse(n.Tag.ToString(), out rid);
                    DataRow dr = dt.NewRow();
                    dr[fieldForIndex] = rid;
                    dt.Rows.Add(dr);
                }
            }

            foreach (UltraTreeNode c in n.Nodes)
            {
                BuildSelectedListValuesFromTags(c, ref dt, includeRootNodes, fieldForIndex);
            }

        }
        private void BuildSelectedListValuesFromTagsWithParent(UltraTreeNode n, ref DataTable dt, bool includeRootNodes, string fieldForIndex, string parentFieldIndex)
        {
            if ((n.Parent != null || includeRootNodes) && n.CheckedState == CheckState.Checked)
            {

                if (n.Tag != null)
                {
                    if (n.Tag.ToString().Contains("~"))
                    {
                        string[] sSplit = n.Tag.ToString().Split('~');
                        int parentRid;
                        int.TryParse(sSplit[0], out parentRid);

                        int rid;
                        int.TryParse(sSplit[1], out rid);
                        DataRow dr = dt.NewRow();
                        dr[fieldForIndex] = rid;
                        dr[parentFieldIndex] = parentRid;
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        int parentRid;
                        int.TryParse(n.Tag.ToString(), out parentRid);

                        DataRow dr = dt.NewRow();
                        dr[fieldForIndex] = -1;
                        dr[parentFieldIndex] = parentRid;
                        dt.Rows.Add(dr);
                    }
                }
            }

            foreach (UltraTreeNode c in n.Nodes)
            {
                BuildSelectedListValuesFromTagsWithParent(c, ref dt, includeRootNodes, fieldForIndex, parentFieldIndex);
            }

        }
        //public string GetSelectedDisplayList(bool includeRootNodes)
        //{
        //    string sList = string.Empty;
        //    foreach (UltraTreeNode n in this.ultraTree1.Nodes)
        //    {
        //        BuildSelectedDisplayList(n, ref sList, includeRootNodes);
        //    }
        //    return sList;
        //}

        //private void BuildSelectedDisplayList(UltraTreeNode n, ref string sList, bool includeRootNodes)
        //{
        //    if ((n.Parent != null || includeRootNodes) && n.CheckedState == CheckState.Checked)
        //    {
        //        if (n.Tag != null)
        //        {
        //            //assume tag is integer
        //            int RID = (int)n.Tag;
        //            //lookup display value based on RID
        //            DataRow[] drFind = dt.Select(_FieldToTag + " = " + RID);
        //            if (drFind.Length > 0)
        //            {
        //                string displayValue = (string)drFind[0][_FieldToDisplay];

        //                //compare string to see if it goes over the max size
        //                string proposedDisplayList = sList;

        //                if (proposedDisplayList == string.Empty)
        //                {
        //                    proposedDisplayList = displayValue;
        //                }
        //                else
        //                {
        //                    proposedDisplayList += ", " + displayValue;
        //                }

        //                if (IsDisplayStringOverMaxWidth(proposedDisplayList))
        //                {
        //                    proposedDisplayList= sList + "...";
        //                }

        //                sList = proposedDisplayList;
        //            }
        //        }
        //    }

        //    foreach (UltraTreeNode c in n.Nodes)
        //    {
        //        BuildSelectedListFromTags(c, ref sList, includeRootNodes);
        //    }

        //}





        public void SelectNodesFromDelimitedList(string delimitedList)
        {
            _LoadingSelected = true;
            string[] sList = delimitedList.Split(',');
            foreach (string tagToCompare in sList)
            {
                foreach (UltraTreeNode n in this.ultraTree1.Nodes)
                {
                    SelectNodesFromList(n, tagToCompare);
                }
            }
            _LoadingSelected = false;
        }
        public void SelectNodesFromListValues(DataRow[] dtListValues, string fieldForRID)
        {
            _LoadingSelected = true;
            foreach (DataRow dr in dtListValues)
            {
                int rid = (int)dr[fieldForRID];
                foreach (UltraTreeNode n in this.ultraTree1.Nodes)
                {
                    SelectNodesFromList(n, rid.ToString());
                }
            }
            _LoadingSelected = false;
        }

        private void SelectNodesFromList(UltraTreeNode n, string tagToCompare)
        {
            //if (tagToCompare != "All" && tagToCompare != "None" && n.Tag != null)
            //{
            //    if (n.Tag.ToString() == tagToCompare)
            //    {
            //        n.CheckedState = CheckState.Checked;
            //    }
            //    else
            //    {
            //        foreach (UltraTreeNode c in n.Nodes)
            //        {
            //            SelectNodesFromList(c, tagToCompare);
            //        }
            //    }
            //}

            if (tagToCompare != "All" && tagToCompare != "None" && n.Tag != null)
            {
                if (n.Tag.ToString() == tagToCompare)
                {
                    n.CheckedState = CheckState.Checked;
                    return;
                }
            }

            foreach (UltraTreeNode c in n.Nodes)
            {
                SelectNodesFromList(c, tagToCompare);
            }
        }


        public void SelectNodesFromListValuesWithParent(DataRow[] dtListValues, string fieldForRID, string fieldForParent)
        {
            _LoadingSelected = true;
            foreach (DataRow dr in dtListValues)
            {
                int rid = (int)dr[fieldForRID];
                int parentRid = (int)dr[fieldForParent];
                foreach (UltraTreeNode n in this.ultraTree1.Nodes)
                {
                    if (rid == -1)
                    {
                        SelectNodesFromList(n, parentRid.ToString());
                    }
                    else
                    {
                        SelectNodesFromList(n, parentRid.ToString() + "~" + rid.ToString());
                    }
                }
            }
            _LoadingSelected = false;
        }
     


        private void ultraTree1_MouseDown(object sender, MouseEventArgs e)
        {
            UltraTree treeControl = sender as UltraTree;
            UltraTreeUIElement controlElement = treeControl.UIElement;
            UIElement elementAtPoint = controlElement != null ? controlElement.ElementFromPoint(e.Location) : null;
            UltraTreeNode node = null;

            //while ( elementAtPoint != null )
            //{
            if (elementAtPoint is NodeTextUIElement)
                node = elementAtPoint.GetContext(typeof(UltraTreeNode)) as UltraTreeNode;

            elementAtPoint = elementAtPoint.Parent;
            //}

            if (node != null)
                this.OnNodeMouseDown(node);
        }

        private void OnNodeMouseDown(UltraTreeNode node)
        {
            Infragistics.Win.UltraWinTree.UltraTreeNode n = node;
            if (this.ultraTree1.ActiveNode != null)
            {
                if (n.CheckedState == CheckState.Checked)
                {
                    n.CheckedState = CheckState.Unchecked;
                }
                else
                {
                    n.CheckedState = CheckState.Checked;
                }
            }
        }


    }
}
