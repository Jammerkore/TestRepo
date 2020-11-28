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
    public partial class filterElementList : UserControl, IFilterElement
    {
        public filterElementList()
        {
            InitializeComponent();


            this.midSelectMultiNodeControl1.ultraTree1.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.midSelectMultiNodeControl1.ultraTree1.AfterCheck += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(ultraTree1_AfterNodeChanged);
        }

        private elementBase eb;
        private filterEntrySettings groupSettings;
        private bool useValueListFromField;
        public void SetElementBase(elementBase eb, filterEntrySettings groupSettings)
        {
            this.eb = eb;
            this.groupSettings = groupSettings;
            if (eb.groupHeading == string.Empty)
            {
                this.midSelectMultiNodeControl1.Title = groupSettings.groupTitle;
            }
            else
            {
                this.midSelectMultiNodeControl1.Title = eb.groupHeading;
            }
            if (eb.manager.readOnly)
            {
                midSelectMultiNodeControl1.Enabled = false;
            }
            //BindTree(groupSettings.valueSettings);
        }
        public void ClearControls()
        {
        }
        public void SetBinding(bool useValueListFromField, int tempFieldIndex)
        {
            if (eb.manager.currentCondition.dictionaryIndex == filterDictionary.Users)
            {
                this.midSelectMultiNodeControl1.ShowRootLines = true;
                this.midSelectMultiNodeControl1.MappingRelationshipColumnKey = "GroupsToUsers";

                this.midSelectMultiNodeControl1.FieldToTag = "USER_RID";
                this.midSelectMultiNodeControl1.ParentFieldToTag = "GROUP_RID";
                this.midSelectMultiNodeControl1.BindDataSet(filterDataHelper.GetActiveUserDataset());
            }
            else
            {
                this.useValueListFromField = useValueListFromField;
                DataSet ds = new DataSet();
                if (useValueListFromField)
                {
                    this.midSelectMultiNodeControl1.FieldToTag = groupSettings.valueFieldForData;
                    ds.Tables.Add(groupSettings.loadValueListFromField(tempFieldIndex).Copy());
                }
                else
                {
                    this.midSelectMultiNodeControl1.FieldToTag = groupSettings.fieldForData;
                    ds.Tables.Add(groupSettings.loadValueList().Copy());
                }
                this.midSelectMultiNodeControl1.BindDataSet(ds);
            }
        }

        private bool IsEveryNodeSelected()
        {
            return this.midSelectMultiNodeControl1.IsEveryNodeSelected();
        }


        public void LoadFromCondition(filter f, filterCondition condition)
        {

            this.midSelectMultiNodeControl1.NodesUnCheckAll();
            if (condition.listConstantType == filterListConstantTypes.All)
            {
                this.midSelectMultiNodeControl1.NodesCheckAll();
            }
            else if (condition.listConstantType == filterListConstantTypes.None)
            {
                //this.midSelectMultiNodeControl1.NodesCheckAll();
            }
            else
            {
                //string delimitedList = condition.listValues;
                //this.midSelectMultiNodeControl1.SelectNodesFromDelimitedList(delimitedList);

                //load a datatable for store_rids, store characteristics rids, header characteristic rids, etc.
       

                if (eb.manager.currentCondition.dictionaryIndex == filterDictionary.Users)
                {
                    DataRow[] dtListValues = condition.GetListValues(groupSettings.listValueType);
                    this.midSelectMultiNodeControl1.SelectNodesFromListValuesWithParent(dtListValues, filterCondition.GetListValueIndexField(), filterCondition.GetListValueParentIndexField());
                }
                else
                {
                    DataRow[] dtListValues = condition.GetListValues(groupSettings.listValueType);
                    this.midSelectMultiNodeControl1.SelectNodesFromListValues(dtListValues, filterCondition.GetListValueIndexField());
                }
            }

        }
        public bool IsValid(filter f, filterCondition condition)
        {
            return true;
        }
        public void SaveToCondition(ref filter f, ref filterCondition condition)
        {
            if (IsEveryNodeSelected())
            {
                condition.listConstantType = filterListConstantTypes.All;
                
                //Begin TT#1420-MD -jsobek -Header Filter using a Header Characteristice when selected results in headers that have the header characteristic and headers that DO NO have the header characteristic.
                //Header characteristic lists do not guarentee a value for all headers, therefore we need to save the list values.
                if (condition.dictionaryIndex == filterDictionary.HeaderCharacteristics)
                {

                    DataTable dt = this.midSelectMultiNodeControl1.GetSelectedListValuesFromTags(true, filterCondition.GetListValueIndexField());
                    if (dt.Rows.Count == 0)
                    {
                        condition.listConstantType = filterListConstantTypes.None;
                        condition.ClearListValues();
                    }
                    else
                    {
                        condition.SaveListValues(dt, groupSettings.listValueType);
                    }
                }
                else
                {
                    condition.ClearListValues();
                }
                //End TT#1420-MD -jsobek -Header Filter using a Header Characteristice when selected results in headers that have the header characteristic and headers that DO NO have the header characteristic.
            }
            else
            {
                if (eb.manager.currentCondition.dictionaryIndex == filterDictionary.Users)
                {
                    DataTable dt = this.midSelectMultiNodeControl1.GetSelectedListValuesFromTagsWithParent(true, filterCondition.GetListValueIndexField(), filterCondition.GetListValueParentIndexField());
                    if (dt.Rows.Count == 0)
                    {
                        condition.listConstantType = filterListConstantTypes.None;
                        condition.ClearListValues();
                    }
                    else
                    {
                        condition.listConstantType = filterListConstantTypes.HasValues;
                        condition.SaveListValues(dt, groupSettings.listValueType);
                    }
                }
                else
                {
                    DataTable dt = this.midSelectMultiNodeControl1.GetSelectedListValuesFromTags(true, filterCondition.GetListValueIndexField());
                    if (dt.Rows.Count == 0)
                    {
                        condition.listConstantType = filterListConstantTypes.None;
                        condition.ClearListValues();
                    }
                    else
                    {
                        condition.listConstantType = filterListConstantTypes.HasValues;
                        condition.SaveListValues(dt, groupSettings.listValueType);
                    }
                }
            }
        }

        private void ultraTree1_AfterNodeChanged(object sender, Infragistics.Win.UltraWinTree.NodeEventArgs e)
        {
            eb.MakeConditionDirty();
        }



    }
}
