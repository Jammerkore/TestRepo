using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
    public partial class PlanChainLadderToolControl : UserControl
    {
        public PlanChainLadderToolControl()
        {
            InitializeComponent();
        }

        private void PlanChainLadderToolControl_Load(object sender, EventArgs e)
        {
        }
        public void Initialize()
        {
            cboView.Tag = "IgnoreMouseWheel";
            cboUnitScaling.Tag = "IgnoreMouseWheel";
            cboDollarScaling.Tag = "IgnoreMouseWheel";
        }



        private bool _changePending;
        public bool ChangePending
        {
            get { return _changePending; }
        }

        private void HandleExceptions(System.Exception exc)
        {
            Debug.WriteLine(exc.ToString());
            throw exc;
            // MessageBox.Show(exc.ToString());
        }
        //private void cboFilter_DropDown(object sender, System.EventArgs e)
        //{
        //    RaiseFilterDropDownEvent();
        //}

        //private void cboFilter_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        //{
        //    try
        //    {
        //        //Image_DragEnter(sender, e);
        //        RaiseImage_DragEnterEvent(sender, e);
        //        if (e.Data.GetDataPresent(typeof(MIDRetail.Windows.MIDFilterNode)))
        //        {
        //            e.Effect = DragDropEffects.All;
        //        }
        //        else
        //        {
        //            e.Effect = DragDropEffects.None;
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        HandleExceptions(exc);
        //    }
        //}

        //private void cboFilter_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        //{
        //    try
        //    {
        //        bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

        //        if (isSuccessfull)
        //        {
        //            _changePending = true;
        //            ((MIDRetail.Windows.Controls.MIDComboBoxEnh)((ComboBox)sender).Parent).FirePropertyChangeEvent();
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        HandleExceptions(exc);
        //    }
        //}


        //private void cboFilter_DragOver(object sender, DragEventArgs e)
        //{
        //    RaiseImage_DragOverEvent(sender, e);
        //}


        //private void cboFilter_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    RaiseFilterSelectionChangedCommittedEvent(this.cboFilter.SelectedItem, this.cboFilter.SelectedIndex);
        //}
 
        //private void cboStoreAttribute_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    //Need Event
        //}
       
        //private void cboAttributeSet_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    //Need Event
        //}
        //private void cboStoreAttribute_DragEnter(object sender, DragEventArgs e)
        //{
        //    RaiseImage_DragEnterEvent(sender, e);
        //}

        //private void cboStoreAttribute_DragOver(object sender, DragEventArgs e)
        //{
        //    RaiseImage_DragOverEvent(sender, e);
        //}
    

        private void cboDollarScaling_MIDComboBoxPropertiesChangedEvent(object source, Windows.Controls.MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboDollarScaling_SelectionChangeCommitted(source, new EventArgs());
        }

        private void cboUnitScaling_MIDComboBoxPropertiesChangedEvent(object source, Windows.Controls.MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboUnitScaling_SelectionChangeCommitted(source, new EventArgs());
        }

        //private void cboFilter_MIDComboBoxPropertiesChangedEvent(object source, Windows.Controls.MIDComboBoxPropertiesChangedEventArgs args)
        //{
        //    this.cboFilter_SelectionChangeCommitted(source, new EventArgs());
        //}
        //private void cboAttributeSet_MIDComboBoxPropertiesChangedEvent(object source, Windows.Controls.MIDComboBoxPropertiesChangedEventArgs args)
        //{
        //    this.cboAttributeSet_SelectionChangeCommitted(source, new EventArgs());
        //}

        //private void cboStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, Windows.Controls.MIDComboBoxPropertiesChangedEventArgs args)
        //{
        //    this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        //}
        private void cboUnitScaling_SelectionChangeCommitted(object sender, EventArgs e)
        {
        }
        private void cboDollarScaling_SelectionChangeCommitted(object sender, EventArgs e)
        {
        }
        private void optGroupByTime_CheckedChanged(object sender, System.EventArgs e)
        {
        }
        private void optGroupByVariable_CheckedChanged(object sender, System.EventArgs e)
        {
        }

       

        public class Image_DragEnterEventArgs
        {
            public Image_DragEnterEventArgs(object dragControl, System.Windows.Forms.DragEventArgs e) { this.dragControl = dragControl; this.e = e; }
            public object dragControl { get; private set; } 
            public System.Windows.Forms.DragEventArgs e { get; private set; } 
        }
        public delegate void Image_DragEnterEventHandler(object sender, Image_DragEnterEventArgs e);
        public event Image_DragEnterEventHandler Image_DragEnterEvent;
        protected virtual void RaiseImage_DragEnterEvent(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (Image_DragEnterEvent != null)
                Image_DragEnterEvent(this, new Image_DragEnterEventArgs(sender, e));
        }


        public class Image_DragOverEventArgs
        {
            public Image_DragOverEventArgs(object dragControl, System.Windows.Forms.DragEventArgs e) { this.dragControl = dragControl; this.e = e; }
            public object dragControl { get; private set; }
            public System.Windows.Forms.DragEventArgs e { get; private set; }
        }
        public delegate void Image_DragOverEventHandler(object sender, Image_DragOverEventArgs e);
        public event Image_DragOverEventHandler Image_DragOverEvent;
        protected virtual void RaiseImage_DragOverEvent(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (Image_DragOverEvent != null)
                Image_DragOverEvent(this, new Image_DragOverEventArgs(sender, e));
        }


        //public class FilterSelectionChangedCommittedEventArgs
        //{
        //    public FilterSelectionChangedCommittedEventArgs(object SelectedItem, int SelectedIndex) { this.SelectedItem = SelectedItem; this.SelectedIndex = SelectedIndex; }
        //    public object SelectedItem { get; private set; }
        //    public int SelectedIndex { get; private set; }
        //}
        //public delegate void FilterSelectionChangedCommittedEventHandler(object sender, FilterSelectionChangedCommittedEventArgs e);
        //public event FilterSelectionChangedCommittedEventHandler FilterSelectionChangedCommittedEvent;
        //protected virtual void RaiseFilterSelectionChangedCommittedEvent(object SelectedItem, int SelectedIndex)
        //{
        //    if (FilterSelectionChangedCommittedEvent != null)
        //        FilterSelectionChangedCommittedEvent(this, new FilterSelectionChangedCommittedEventArgs(SelectedItem, SelectedIndex));
        //}

        //public class FilterDropDownEventArgs
        //{
        //    public FilterDropDownEventArgs() { }
        //}
        //public delegate void FilterDropDownEventHandler(object sender, FilterDropDownEventArgs e);
        //public event FilterDropDownEventHandler FilterDropDownEvent;
        //protected virtual void RaiseFilterDropDownEvent()
        //{
        //    if (FilterDropDownEvent != null)
        //        FilterDropDownEvent(this, new FilterDropDownEventArgs());
        //}

        private bool _bindingView = false;
        private int _lastViewValue;
        public void BindViewComboBox(DataTable _dtView, int viewRID)
        {
            try
            {
                _bindingView = true;

                _lastViewValue = viewRID;
                cboView.ValueMember = "VIEW_RID";
                cboView.DisplayMember = "DISPLAY_ID";
                cboView.DataSource = _dtView;
                cboView.SelectedValue = viewRID;

                _bindingView = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public Controls.MIDComboBoxEnh GetViewComboBox()
        {
            return this.cboView;
        }
        private void cboView_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int selectedValue = (int)cboView.SelectedValue;

            if ((!_bindingView && selectedValue != _lastViewValue) || (_bindingView && selectedValue == _lastViewValue))
            {
                _lastViewValue = selectedValue;
                RaiseViewSelectionChangedCommittedEvent(this.cboView.SelectedItem, this.cboView.SelectedIndex);
            }
        }

        private void cboView_MIDComboBoxPropertiesChangedEvent(object source, Windows.Controls.MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboView_SelectionChangeCommitted(source, new EventArgs());
        }

        public class ViewSelectionChangedCommittedEventArgs
        {
            public ViewSelectionChangedCommittedEventArgs(object SelectedItem, int SelectedIndex) { this.SelectedItem = SelectedItem; this.SelectedIndex = SelectedIndex; }
            public object SelectedItem { get; private set; }
            public int SelectedIndex { get; private set; }
        }
        public delegate void ViewSelectionChangedCommittedEventHandler(object sender, ViewSelectionChangedCommittedEventArgs e);
        public event ViewSelectionChangedCommittedEventHandler ViewSelectionChangedCommittedEvent;
        protected virtual void RaiseViewSelectionChangedCommittedEvent(object SelectedItem, int SelectedIndex)
        {
            if (ViewSelectionChangedCommittedEvent != null)
                ViewSelectionChangedCommittedEvent(this, new ViewSelectionChangedCommittedEventArgs(SelectedItem, SelectedIndex));
        }


        #region "Apply Button"
        private void btnApply_Click(object sender, System.EventArgs e)
        {
            RaiseApplyEvent();
        }
        public class ApplyEventArgs
        {
            public ApplyEventArgs() { }
        }
        public delegate void ApplyEventHandler(object sender, ApplyEventArgs e);
        public event ApplyEventHandler ApplyEvent;
        protected virtual void RaiseApplyEvent()
        {
            if (ApplyEvent != null)
                ApplyEvent(this, new ApplyEventArgs());
        }
        #endregion


    }
}
