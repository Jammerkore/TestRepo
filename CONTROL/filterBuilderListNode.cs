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
    public partial class filterBuilderListNode : UserControl
    {
        public filterBuilderListNode()
        {
            InitializeComponent();
        }
        string letterPrefix;
        public filterBuilderListNode(filterManager manager, ConditionNode n, string letterPrefix)
        {
            InitializeComponent();
            this.manager = manager;
            this.conditionNode = n;
            this.e1.TabIndex = n.condition.Seq + 100;
            this.e1.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.letterPrefix = letterPrefix;

            ShowFormattedText();

        }
        public ConditionNode conditionNode;
        private filterManager manager;


        public string FormattedText
        {
            get { return this.e1.Value.ToString(); }
            set { this.e1.Value = value; this.PerformLayout(); }
        }

        public void ShowFormattedText()
        {
            if (manager.Options.ShowExecutionPlan)
            {
                filterDictionary et = filterDictionary.FromIndex(this.conditionNode.condition.dictionaryIndex);
                string executionPlan = string.Empty;
                if (et.costToRunEstimate > 0)
                {
                    executionPlan = " (" + this.letterPrefix + " g=" + this.conditionNode.condition.executeGroup + ", ";
                    executionPlan += "e=" + this.conditionNode.condition.executed + ", ";
                    executionPlan += "r=" + this.conditionNode.condition.executeResult + ", ";
                    executionPlan += "rg=" + this.conditionNode.condition.executeResultForGroup + ")";                   
                }
                this.e1.Value = this.conditionNode.condition.NodeFormattedText + executionPlan;
            }
            else if (manager.Options.ShowEstimatedCost)
            {
                //ConditionBase cb = DefinitionList.FromString(manager.currentFilter.filterType, this.conditionNode.condition.conditionType);
                filterDictionary et = filterDictionary.FromIndex(this.conditionNode.condition.dictionaryIndex);
                string cost = string.Empty;
                if (et.costToRunEstimate > 0)
                {
                    cost = " (Cost = " + et.costToRunEstimate.ToString() + " [" + manager.currentFilter.GetCost(this.conditionNode.condition.Seq) + "]" + ")";
                }
                this.e1.Value = this.conditionNode.condition.NodeFormattedText + cost;
            }
            else
            {
                this.e1.Value = this.conditionNode.condition.NodeFormattedText;
            }
            this.PerformLayout();
        }


        private bool _IsSelected = false;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (value == false)
                {
                    _IsSelected = false;
                    this.pEmpty.Visible = true;
                    this.pSelected.Visible = false;
                    this.pHotTrack.Visible = false;
                    //s1.Items[0].CheckState = CheckState.Unchecked;

                    //this.e1.Appearance.BorderColor = Color.White;
                    this.e1.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.None;
                }
                else
                {
                    _IsSelected = true;
                    this.pEmpty.Visible = false;
                    this.pSelected.Visible = true;
                    this.pHotTrack.Visible = false;
                    //s1.Items[0].CheckState = CheckState.Checked;

                    //this.e1.Appearance.BorderColor = Color.DarkBlue;
                    this.e1.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
                    this.u1.Focus();
                }
            }
        }

        public int ID;


        private void e1_MouseEnter(object sender, EventArgs e)
        {

            if (IsSelected == false)
            {
                this.pEmpty.Visible = false;
                this.pHotTrack.Visible = true;

            }
            this.e1.Appearance.BorderColor = Color.SkyBlue;

        }

        private void e1_MouseLeave(object sender, EventArgs e)
        {

            if (IsSelected == false && this.u1.Focused == false)
            {
                this.pEmpty.Visible = true;
                this.pHotTrack.Visible = false;

            }
            if (this.u1.Focused)
            {
                this.e1.Appearance.BorderColor = Color.SkyBlue;
            }
            else
            {
                this.e1.Appearance.BorderColor = Color.White;
            }
        }

        private void e1_MouseClick(object sender, MouseEventArgs e)
        {
            if (_IsSelected == false)
            {
                if (CanSelectNewNode() == true)
                {
                    this.IsSelected = true;
                    RaiseNodeClickedEvent();
                }
            }
        }
        private bool CanSelectNewNode()
        {
            return this.manager.IsOKToSelectNewCondition();
        }

        //Events
        public event NodeClickedEventHandler NodeClickedEvent;
        public void RaiseNodeClickedEvent()
        {
            if (NodeClickedEvent != null)
                NodeClickedEvent(new object(), new NodeClickedEventArgs(this));
        }
        public class NodeClickedEventArgs
        {
            public NodeClickedEventArgs(filterBuilderListNode SelectedNode) { this.SelectedNode = SelectedNode; }
            public filterBuilderListNode SelectedNode;
        }
        public delegate void NodeClickedEventHandler(object sender, NodeClickedEventArgs e);



        public new void Enter()
        {
            //e1.Focus();
            u1.Focus();
        }

        private void e1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)Keys.Space)
            //{
            //    if (_IsSelected == false)
            //    {
            //        if (CanSelectNewNode() == true)
            //        {
            //            this.IsSelected = true;
            //            RaiseNodeClickedEvent();
            //        }
            //    }
            //}
        }

        private void e1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            //{
            //    RaiseKeyPressDownEvent(e.KeyCode);
            //}
        }



        public event KeyPressDownEventHandler KeyPressDownEvent;
        public void RaiseKeyPressDownEvent(Keys KeyCode)
        {

            if (KeyPressDownEvent != null)
                KeyPressDownEvent(this, new KeyPressDownEventArgs(KeyCode));
        }
        public class KeyPressDownEventArgs
        {
            public KeyPressDownEventArgs(Keys KeyCode) { this.KeyCode = KeyCode; }
            public Keys KeyCode { get; private set; }
        }
        public delegate void KeyPressDownEventHandler(object sender, KeyPressDownEventArgs e);


        private void e1_Leave(object sender, EventArgs e)
        {
            ////if (IsSelected == false)
            ////{
            ////e1.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            //e1.Appearance.BorderColor = System.Drawing.Color.White;
            ////}

            //if (IsSelected == false)
            //{
            //    this.pEmpty.Visible = true;
            //    this.pHotTrack.Visible = false;

            //}
        }

        private void e1_Enter(object sender, EventArgs e)
        {
            ////if (IsSelected == false)
            ////{
            ////e1.BorderStyle = Infragistics.Win.UIElementBorderStyle.Rounded4;
            //e1.Appearance.BorderColor = System.Drawing.Color.SkyBlue;
            ////}
            //if (IsSelected == false)
            //{
            //    this.pEmpty.Visible = false;
            //    this.pHotTrack.Visible = true;
            //}
        }

        private void u1_Leave(object sender, EventArgs e)
        {
            e1.Appearance.BorderColor = System.Drawing.Color.White;


            if (IsSelected == false)
            {
                this.pEmpty.Visible = true;
                this.pHotTrack.Visible = false;

            }
        }
        private void u1_Enter(object sender, EventArgs e)
        {
            e1.Appearance.BorderColor = System.Drawing.Color.SkyBlue;

            if (IsSelected == false)
            {
                this.pEmpty.Visible = false;
                this.pHotTrack.Visible = true;
            }
        }

        private void u1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                if (_IsSelected == false)
                {
                    if (CanSelectNewNode() == true)
                    {
                        this.IsSelected = true;
                        RaiseNodeClickedEvent();
                    }
                }
            }
        }

        private void u1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                RaiseKeyPressDownEvent(e.KeyCode);
            }
        }

    }
}
