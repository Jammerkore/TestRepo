using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Business;

namespace MIDRetail.Windows.Controls
{
    public partial class filterElementMerchandise : UserControl, IFilterElement
    {
        public filterElementMerchandise()
        {
            InitializeComponent();
        }

        private elementBase eb;
        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            this.selectSingleHierarchyNodeControl1.LoadData(eb.manager.SAB, SelectSingleHierarchyNodeControl.AllowedNodeLevel.NoSize); //TT#1313-MD -jsobek -Header Filters
            this.selectSingleHierarchyNodeControl1.NodeChangedEvent += new SelectSingleHierarchyNodeControl.NodeChangedEventHandler(HierarchyNodeChanged);
            this.selectSingleHierarchyNodeControl1.IsDirtyEvent += new SelectSingleHierarchyNodeControl.IsDirtyEventHandler(Handle_IsDirtyEvent);
            if (eb.manager.readOnly)
            {
                selectSingleHierarchyNodeControl1.Enabled = false;
            }
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            eb.isLoading = true;

            this.useVariable1 = eb.loadFromVariable1;
            this.useVariable2 = eb.loadFromVariable2;
            if (this.useVariable1)
            {
                if (eb.manager.currentCondition.lastVariable1_HN_RID != null)
                {
                    int hnRID = (int)eb.manager.currentCondition.lastVariable1_HN_RID;
                    HierarchyNodeProfile hnp = eb.manager.SAB.HierarchyServerSession.GetNodeData(hnRID, true, true);
                    this.selectSingleHierarchyNodeControl1.SetNode(hnp);
                }
            }
            else if (useVariable2)
            {
                if (eb.manager.currentCondition.lastVariable2_HN_RID != null)
                {
                    int hnRID = (int)eb.manager.currentCondition.lastVariable2_HN_RID;
                    HierarchyNodeProfile hnp = eb.manager.SAB.HierarchyServerSession.GetNodeData(hnRID, true, true);
                    this.selectSingleHierarchyNodeControl1.SetNode(hnp);
                }         
            }
            //if (eb.manager.currentFilter.filterType == filterTypes.ProductFilter)
            //{
            //    //get the default merchandise node from the first selected node in the explorer
            //    HierarchyNodeProfile hnp2 = SharedControlRoutines.GetFirstSelectedMerchandiseNode();

            //    if (hnp2 != null)
            //    {
            //        //this.selectSingleHierarchyNodeControl1.SetNode(hnp2);
            //        eb.manager.currentCondition.headerMerchandise_HN_RID = hnp2.Key;
            //        filterDictionaryEntry eg = eb.manager.currentFilter.GetEntryFromCondition(eb.manager, eb.manager.currentCondition);
            //        eg.BuildFormattedText(eb.manager.Options, ref eb.manager.currentCondition);
            //        //hnRID = hnp2.Key;
            //    }

            //}

            eb.isLoading = false;
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }
        public void ClearControls()
        {
        }
        private bool useVariable1 = false;
        private bool useVariable2 = false;
        private bool useHeaderMerchandise = false;
        public void LoadFromCondition(filter f, filterCondition condition)
        {
            this.useVariable1 = eb.loadFromVariable1;
            this.useVariable2 = eb.loadFromVariable2;
            this.useHeaderMerchandise = eb.loadFromHeaderMerchandise;

            int hnRID = Include.NoRID;
         
            if (useHeaderMerchandise)
            {
                hnRID = condition.headerMerchandise_HN_RID;
                //int hierarchyRID = condition.headerMerchandise_PH_RID;
                //HierarchyNodeProfile hnp = eb.manager.SAB.HierarchyServerSession.GetNodeData(hierarchyRID, hnRID, true, true);
                //HierarchyNodeProfile hnp = eb.manager.SAB.HierarchyServerSession.GetNodeData(hnRID, true, true);
                //this.selectSingleHierarchyNodeControl1.SetNode(hnp);

                //if (eb.manager.currentFilter.filterType == filterTypes.ProductFilter && hnRID == Include.NoRID)
                //{
                //    //get the default merchandise node from the first selected node in the explorer
                //    HierarchyNodeProfile hnp2 = SharedControlRoutines.GetFirstSelectedMerchandiseNode();

                //    if (hnp2 != null)
                //    {
                //        //this.selectSingleHierarchyNodeControl1.SetNode(hnp2);
                //        eb.manager.currentCondition.headerMerchandise_HN_RID = hnp2.Key;
                //        filterDictionaryEntry eg = eb.manager.currentFilter.GetEntryFromCondition(eb.manager, eb.manager.currentCondition);
                //        eg.BuildFormattedText(eb.manager.Options, ref eb.manager.currentCondition);
                //        hnRID = hnp2.Key;
                //    }

                //}
 

            }
            else if (useVariable1)
            {
                hnRID = condition.variable1_HN_RID;
    
				eb.manager.currentCondition.lastVariable1_HN_RID = condition.variable1_HN_RID;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            }
            else if (useVariable2)
            {
                hnRID = condition.variable2_HN_RID;
   
				eb.manager.currentCondition.lastVariable2_HN_RID = condition.variable2_HN_RID;  //TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            }
            //HierarchyNodeProfile hnp = new HierarchyNodeProfile(hnRID);
            HierarchyNodeProfile hnp = eb.manager.SAB.HierarchyServerSession.GetNodeData(hnRID, true, true);
            this.selectSingleHierarchyNodeControl1.SetNode(hnp);
            
        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return this.selectSingleHierarchyNodeControl1.IsValid(raiseChangedEvent: false); //TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
        }

        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
       
            int hnRID = this.selectSingleHierarchyNodeControl1.GetNode();
            this.selectSingleHierarchyNodeControl1._textChanged = false; //prevent validation when losing focus since we already validated control

            //int.TryParse(txtMerchandise.Text, out hnRID);

            if (useHeaderMerchandise)
            {
                condition.headerMerchandise_HN_RID = hnRID;
                //int hierarchyRID = this.selectSingleHierarchyNodeControl1.GetHierarchyRID();
                //condition.headerMerchandise_PH_RID = hierarchyRID;
                condition.valueToCompare = this.selectSingleHierarchyNodeControl1.GetText();
            }
            else if (useVariable1)
            {
                condition.variable1_HN_RID = hnRID;
                //condition.valueToCompare = this.selectSingleHierarchyNodeControl1.GetText();
            }
            else if (useVariable2)
            {
                condition.variable2_HN_RID = hnRID;
                //condition.valueToCompare = this.selectSingleHierarchyNodeControl1.GetText();
            }

        }

        private void HierarchyNodeChanged(object sender, SelectSingleHierarchyNodeControl.NodeChangedEventArgs e)
        {
            eb.MakeConditionDirty();
            //Begin TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
            if (eb.isLoading == false)
            {      
                if (this.useVariable1)
                {
                    int hnRID = this.selectSingleHierarchyNodeControl1.GetNode();
                    eb.manager.currentCondition.lastVariable1_HN_RID = hnRID;
                }
                else if (useVariable2)
                {
                    int hnRID = this.selectSingleHierarchyNodeControl1.GetNode();
                    eb.manager.currentCondition.lastVariable2_HN_RID = hnRID;
                }
            }
            //End TT#1345-MD -jsobek -Store Filters - Hold operator and value when switching variables
        }

        //Begin TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
        private void Handle_IsDirtyEvent(object sender, SelectSingleHierarchyNodeControl.IsDirtyEventArgs e)
        {
            eb.MakeConditionDirty();
        }
        //Begin TT#1373-MD -jsobek -Key in Style\Color for Merchandise and only the Color displays in the Details 
        
    }
}
