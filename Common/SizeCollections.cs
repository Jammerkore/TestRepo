using System;
using System.Collections;
using System.Collections.Generic; // TT#1391 - TMW New Action
using MIDRetail.DataCommon;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Diagnostics;


namespace MIDRetail.Common
{

    #region All Size Methods Constraints Collections
    #region Set Collection AKA...Store Group Level
    public class ItemSet : IEditableObject
    {

        #region Member Variables
        private CollectionSets parent;
        private int _method_rid;
        private int _sgl_rid;
        //				private bool _fill_zeros_ind;
        //				private int _fill_zeros_qty;
        //				private bool _fill_size_holes_ind;
        //				private int _fill_sequence;
        private int _all_size_min;
        private int _all_size_max;
        private int _all_size_mult;
        //				private int _all_size_rule;
        //				private int _all_size_qty;
        private eSizeMethodRowType _row_type_id;
        private bool inTxn = false;
        private CollectionAllColors colAllColors = new CollectionAllColors();
        private CollectionColors colColors = new CollectionColors();
        #endregion Member Variables

        #region Public Properties

        public eSizeMethodRowType RowTypeID
        {
            get { return _row_type_id; }
            set { _row_type_id = value; }
        }


        public CollectionAllColors collectionAllColors
        {
            get
            {
                return colAllColors;
            }
        }


        public CollectionColors collectionColors
        {
            get
            {
                return colColors;
            }
        }

        public int MethodRid
        {
            get { return _method_rid; }
            set { _method_rid = value; }
        }


        public int SglRid
        {
            get { return _sgl_rid; }
            set { _sgl_rid = value; }
        }


        //				public bool FillZerosInd
        //				{
        //					get {return _fill_zeros_ind;}
        //					set {_fill_zeros_ind = value;}
        //				}
        //
        //
        //				public int FillZerosQty
        //				{
        //					get {return _fill_zeros_qty;}
        //					set {_fill_zeros_qty = value;}
        //				}
        //
        //
        //				public bool FillSizeHolesInd
        //				{
        //					get {return _fill_size_holes_ind;}
        //					set {_fill_size_holes_ind = value;}
        //				}
        //
        //
        //				public int FillSequence
        //				{
        //					get {return _fill_sequence;}
        //					set {_fill_sequence = value;}
        //				}


        public int Min
        {
            get { return _all_size_min; }
            set { _all_size_min = value; }
        }


        public int Max
        {
            get { return _all_size_max; }
            set { _all_size_max = value; }
        }


        public int Mult
        {
            get { return _all_size_mult; }
            set { _all_size_mult = value; }
        }


        //				public int Rule
        //				{
        //					get {return _all_size_rule;}
        //					set {_all_size_rule = value;}
        //				}
        //
        //
        //				public int Qty
        //				{
        //					get {return _all_size_qty;}
        //					set {_all_size_qty = value;}
        //				}


        #endregion Properties

        internal CollectionSets Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }


        #region Constructors

        public ItemSet()// : base()
        {
        }

        public ItemSet(int intMethodRid, int intSglRid, bool blnZerosInd,
            int intZerosQty, bool blnSizeHolesInd, int intFillSequence,
            int intMin, int intMax, int intMult, int intRule, int intQty, eSizeMethodRowType rowTypeID)
            : base()
        {

            _method_rid = intMethodRid;
            _sgl_rid = intSglRid;
            //					_fill_zeros_ind = blnZerosInd;
            //					_fill_zeros_qty = intZerosQty;
            //					_fill_size_holes_ind = blnSizeHolesInd;
            //					_fill_sequence = intFillSequence;
            _all_size_min = intMin;
            _all_size_max = intMax;
            _all_size_mult = intMult;
            //					_all_size_rule = intRule;
            //					_all_size_qty = intQty;
            _row_type_id = rowTypeID;

        }


        public ItemSet(int intMethodRid, DataRow drSetItem)// :base()
        {

            _method_rid = intMethodRid;

            //SET VALUES FROM THE DATAROW
            //===========================
            //					_fill_zeros_ind = (bool)drSetItem["FZ_IND"];
            //					_fill_size_holes_ind = (bool)drSetItem["FSH_IND"];
            _sgl_rid = Convert.ToInt32(drSetItem["SGL_RID"], CultureInfo.CurrentUICulture);
            _row_type_id = (eSizeMethodRowType)drSetItem["ROW_TYPE_ID"];
            //					_fill_zeros_qty = (drSetItem["FILL_ZEROS_QUANTITY"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSetItem["FILL_ZEROS_QUANTITY"],CultureInfo.CurrentUICulture) : Include.UndefinedQuantity;
            //					_fill_sequence = (drSetItem["FILL_SEQUENCE"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSetItem["FILL_SEQUENCE"],CultureInfo.CurrentUICulture) : Include.NoRID;
            _all_size_min = (drSetItem["SIZE_MIN"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSetItem["SIZE_MIN"], CultureInfo.CurrentUICulture) : Include.UndefinedMinimum;
            _all_size_max = (drSetItem["SIZE_MAX"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSetItem["SIZE_MAX"], CultureInfo.CurrentUICulture) : Include.UndefinedMaximum;
            _all_size_mult = (drSetItem["SIZE_MULT"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSetItem["SIZE_MULT"], CultureInfo.CurrentUICulture) : Include.UndefinedMultiple;
            //					_all_size_rule = (drSetItem["SIZE_RULE"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSetItem["SIZE_RULE"],CultureInfo.CurrentUICulture) : Include.UndefinedRule;
            //					_all_size_qty = (drSetItem["SIZE_QUANTITY"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSetItem["SIZE_QUANTITY"],CultureInfo.CurrentUICulture) : Include.UndefinedQuantity;

        }

        #endregion Constructors

        #region Implements IEditableObject
        void IEditableObject.BeginEdit()
        {
            if (!inTxn)
            {
                //this.backupData = setData;
                inTxn = true;
            }
        }

        void IEditableObject.CancelEdit()
        {
            if (inTxn)
            {
                //this.setData = backupData;
                inTxn = false;
            }
        }

        void IEditableObject.EndEdit()
        {
            if (inTxn)
            {
                //backupData = new SetData();
                inTxn = false;
            }
        }



        #endregion

        #region Methods
        private void OnItemChanged()
        {
            if (!inTxn && Parent != null)
            {
                //Parent.CustomerChanged(this);
                Parent.ItemChanged(this);
            }
        }
        #endregion

    }
    [Serializable]	//  MID Track #5092 - Serialization error
    public class CollectionSets : CollectionBase, IBindingList
    {
        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        private ListChangedEventHandler onListChanged;
        private int _sgRid;   // issue 4244
        private Hashtable _storeGroupLevelHash;

        //Change to the name of the object that holds the data in the next 4 items.
        public ItemSet this[int index]
        {
            get
            {
                return (ItemSet)(List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        // BEGIN issue 4244 - stodd 3.8.2007
        public int StoreGroupRid
        {
            get
            {
                return _sgRid;
            }
            set
            {
                _sgRid = value;
            }
        }

        public Hashtable StoreGroupLevelHash
        {
            get
            {
                return _storeGroupLevelHash;
            }
            set
            {
                _storeGroupLevelHash = value;
            }
        }
        // END issue 4244 - stodd 3.8.2007


        public int Add(ItemSet objItemSet)
        {
            return List.Add(objItemSet);
        }

        public ItemSet AddNew()
        {
            return (ItemSet)((IBindingList)this).AddNew();
        }

        public void Remove(ItemSet objItemSet)
        {
            List.Remove(objItemSet);
        }

        //=========================================================================



        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }


        //Change to the name of the object that holds the data in this item.
        protected override void OnClear()
        {
            foreach (ItemSet c in List)
            {
                c.Parent = null;
            }
        }
        //=========================================================================


        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }



        //Change to the name of the object that holds the data here.
        //=========================================================================
        protected override void OnInsertComplete(int index, object value)
        {
            ItemSet c = (ItemSet)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ItemSet c = (ItemSet)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                ItemSet oldcust = (ItemSet)oldValue;
                ItemSet newcust = (ItemSet)newValue;

                oldcust.Parent = null;
                newcust.Parent = this;


                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        //=========================================================================


        //Change to the name of the object that holds the data here.
        //=========================================================================
        // Called by Customer when it changes.
        internal void ItemChanged(ItemSet itemSet)
        {

            int index = List.IndexOf(itemSet);

            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }


        //=========================================================================

        // Implements IBindingList.
        bool IBindingList.AllowEdit
        {
            get { return true; }
        }

        bool IBindingList.AllowNew
        {
            get { return false; }
        }

        bool IBindingList.AllowRemove
        {
            get { return false; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return false; }
        }


        // Events.
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }


        // Methods.
        object IBindingList.AddNew()
        {
            ItemSet c = new ItemSet();
            List.Add(c);
            return c;
        }


        // Unsupported properties.
        bool IBindingList.IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }


        // Unsupported Methods.
        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

    }

    #endregion Set Collection AKA...Store Group Level

    #region All Color Collection
    public class ItemAllColor : MinMaxItemBase, IEditableObject
    {

        #region Member Variables
        private CollectionAllColors parent;
        private bool inTxn = false;
        //private CollectionSizes colAllColorSizes = new CollectionSizes();
        private CollectionSizeDimensions colAllColorSizeDimensions = new CollectionSizeDimensions();
        #endregion Member Variables

        #region Public Properties

        //				public CollectionSizes collectionSizes
        //				{
        //					get {return colAllColorSizes;}
        //				}
        //
        public CollectionSizeDimensions collectionSizeDimensions
        {
            get { return colAllColorSizeDimensions; }
        }

        #endregion Properties

        internal CollectionAllColors Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }


        #region Constructors

        public ItemAllColor()
            : base()
        {
        }


        public ItemAllColor(int intMethodRid, int intColorRid, int intMin,
                            int intMax, int intMult, int intRule, int intQty,
                            int intSizesRid, int intSizeCodeRid, int intDimensionsRid,
                            int intSglRid, eSizeMethodRowType rowTypeID)
            : base(intMethodRid, intColorRid, intMin,
                   intMax, intMult, intRule, intQty,
                   intSizesRid, intSizeCodeRid, intDimensionsRid,
                   intSglRid, rowTypeID)
        {

        }


        public ItemAllColor(int intMethodRid, DataRow drAllColorItem)
            : base(intMethodRid, drAllColorItem)
        {

        }



        #endregion Constructors


        #region Implements IEditableObject
        void IEditableObject.BeginEdit()
        {
            if (!inTxn)
            {
                //this.backupData = allColorData;
                inTxn = true;
            }
        }

        void IEditableObject.CancelEdit()
        {
            if (inTxn)
            {
                //this.allColorData = backupData;
                inTxn = false;
            }
        }

        void IEditableObject.EndEdit()
        {
            if (inTxn)
            {
                //backupData = new AllColorData();
                inTxn = false;
            }
        }



        #endregion


        #region Methods
        private void OnItemChanged()
        {
            if (!inTxn && Parent != null)
            {
                //Parent.CustomerChanged(this);
                Parent.ItemChanged(this);
            }
        }
        #endregion

    }

    public class CollectionAllColors : CollectionBase, IBindingList
    {
        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        private ListChangedEventHandler onListChanged;

        //Change to the name of the object that holds the data in the next 4 items.
        public ItemAllColor this[int index]
        {
            get
            {
                return (ItemAllColor)(List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        public int Add(ItemAllColor value)
        {
            return List.Add(value);
        }

        public ItemAllColor AddNew()
        {
            return (ItemAllColor)((IBindingList)this).AddNew();
        }

        public void Remove(ItemAllColor value)
        {
            List.Remove(value);
        }

        //=========================================================================



        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }


        //Change to the name of the object that holds the data in this item.
        protected override void OnClear()
        {
            foreach (ItemAllColor c in List)
            {
                c.Parent = null;
            }
        }
        //=========================================================================


        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }



        //Change to the name of the object that holds the data here.
        //=========================================================================
        protected override void OnInsertComplete(int index, object value)
        {
            ItemAllColor c = (ItemAllColor)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ItemAllColor c = (ItemAllColor)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                ItemAllColor olddata = (ItemAllColor)oldValue;
                ItemAllColor newdata = (ItemAllColor)newValue;

                olddata.Parent = null;
                newdata.Parent = this;


                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        //=========================================================================


        //Change to the name of the object that holds the data here.
        //=========================================================================
        internal void ItemChanged(ItemAllColor allColorSet)
        {

            int index = List.IndexOf(allColorSet);

            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }


        //=========================================================================

        // Implements IBindingList.
        bool IBindingList.AllowEdit
        {
            get { return true; }
        }

        bool IBindingList.AllowNew
        {
            get { return false; }
        }

        bool IBindingList.AllowRemove
        {
            get { return false; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return false; }
        }

        // Events.
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }


        // Methods.
        object IBindingList.AddNew()
        {
            //ItemAllSize c = new ItemAllSize(this.Count.ToString());
            ItemAllColor c = new ItemAllColor();
            List.Add(c);
            return c;
        }


        // Unsupported properties.
        bool IBindingList.IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }


        // Unsupported Methods.
        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

    }

    #endregion All Color Collection

    #region Color Collection
    public class ItemColor : MinMaxItemBase, IEditableObject
    {

        #region Member Variables
        private CollectionColors parent;
        private bool inTxn = false;
        //private CollectionSizes colColorSizes = new CollectionSizes();
        private CollectionSizeDimensions colColorSizeDimensions = new CollectionSizeDimensions();
        #endregion Member Variables

        #region Public Properties

        //				public CollectionSizes collectionSizes
        //				{
        //					get {return colColorSizes;}
        //				}
        //

        public CollectionSizeDimensions collectionSizeDimensions
        {
            get { return colColorSizeDimensions; }
        }

        #endregion Properties

        internal CollectionColors Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }


        #region Constructors

        public ItemColor()
            : base()
        {
        }


        public ItemColor(int intMethodRid, int intColorRid, int intMin,
                        int intMax, int intMult, int intRule, int intQty,
                        int intSizesRid, int intSizeCodeRid, int intDimensionsRid,
                        int intSglRid, eSizeMethodRowType rowTypeID)
            : base(intMethodRid, intColorRid, intMin,
                   intMax, intMult, intRule, intQty,
                   intSizesRid, intSizeCodeRid, intDimensionsRid,
                   intSglRid, rowTypeID)
        {

        }


        public ItemColor(int intMethodRid, DataRow drColorItem)
            : base(intMethodRid, drColorItem)
        {
        }


        #endregion Constructors

        #region Implements IEditableObject
        void IEditableObject.BeginEdit()
        {
            if (!inTxn)
            {
                //this.backupData = colorData;
                inTxn = true;
            }
        }

        void IEditableObject.CancelEdit()
        {
            if (inTxn)
            {
                //this.colorData = backupData;
                inTxn = false;
            }
        }

        void IEditableObject.EndEdit()
        {
            if (inTxn)
            {
                //backupData = new ColorData();
                inTxn = false;
            }
        }

        #endregion

        #region Methods
        private void OnItemChanged()
        {
            if (!inTxn && Parent != null)
            {
                //Parent.CustomerChanged(this);
                Parent.ItemChanged(this);
            }
        }
        #endregion

    }

    public class CollectionColors : CollectionBase, IBindingList
    {
        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        private ListChangedEventHandler onListChanged;

        //Change to the name of the object that holds the data in the next 4 items.
        public ItemColor this[int index]
        {
            get
            {
                return (ItemColor)(List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        public int Add(ItemColor value)
        {
            return List.Add(value);
        }

        public ItemColor AddNew()
        {
            return (ItemColor)((IBindingList)this).AddNew();
        }

        public void Remove(ItemColor value)
        {
            List.Remove(value);
        }

        //=========================================================================



        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }


        //Change to the name of the object that holds the data in this item.
        protected override void OnClear()
        {
            foreach (ItemColor c in List)
            {
                c.Parent = null;
            }
        }
        //=========================================================================


        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }



        //Change to the name of the object that holds the data here.
        //=========================================================================
        protected override void OnInsertComplete(int index, object value)
        {
            ItemColor c = (ItemColor)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ItemColor c = (ItemColor)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                ItemColor olddata = (ItemColor)oldValue;
                ItemColor newdata = (ItemColor)newValue;

                olddata.Parent = null;
                newdata.Parent = this;


                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        //=========================================================================


        //Change to the name of the object that holds the data here.
        //=========================================================================
        internal void ItemChanged(ItemColor colorSet)
        {

            int index = List.IndexOf(colorSet);

            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }


        //=========================================================================

        // Implements IBindingList.
        bool IBindingList.AllowEdit
        {
            get { return true; }
        }

        bool IBindingList.AllowNew
        {
            get { return true; }
        }

        bool IBindingList.AllowRemove
        {
            get { return true; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return false; }
        }


        // Events.
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }


        // Methods.
        object IBindingList.AddNew()
        {
            //IMPORTANT TO BE SURE THIS IS CORRECT OR AN INVALID CAST ERROR WILL
            //BE RAISED.
            ItemColor c = new ItemColor();
            List.Add(c);
            return c;
        }


        // Unsupported properties.
        bool IBindingList.IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }


        // Unsupported Methods.
        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

    }

    #endregion Color Collection

    #region Size Collection
    public class ItemSize : MinMaxItemBase, IEditableObject
    {

        #region Member Variables
        private CollectionSizes parent;
        private bool inTxn = false;
        //private CollectionSizeDimensions colSizeDimensions = new CollectionSizeDimensions();

        #endregion Member Variables

        #region Public Properties

        //					public CollectionSizeDimensions collectionSizeDimensions
        //					{
        //						get {return colSizeDimensions;}
        //					}
        //
        #endregion Properties

        internal CollectionSizes Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }


        #region Constructors

        public ItemSize()
            : base()
        {
        }


        public ItemSize(int intMethodRid, int intColorRid, int intMin,
                        int intMax, int intMult, int intRule, int intQty,
                        int intSizesRid, int intSizeCodeRid, int intDimensionsRid,
                        int intSglRid, eSizeMethodRowType rowTypeID)
            : base(intMethodRid, intColorRid, intMin,
                   intMax, intMult, intRule, intQty,
                   intSizesRid, intSizeCodeRid, intDimensionsRid,
                   intSglRid, rowTypeID)
        {

        }


        public ItemSize(int intMethodRid, DataRow drSize)
            : base(intMethodRid, drSize)
        {
        }


        #endregion Constructors

        #region Implements IEditableObject

        void IEditableObject.BeginEdit()
        {
            if (!inTxn)
            {
                //this.backupData = sizeData;
                inTxn = true;
            }
        }

        void IEditableObject.CancelEdit()
        {
            if (inTxn)
            {
                //this.sizeData = backupData;
                inTxn = false;
            }
        }

        void IEditableObject.EndEdit()
        {
            if (inTxn)
            {
                //backupData = new SizeData();
                inTxn = false;
            }
        }

        #endregion


        #region Methods
        private void OnItemChanged()
        {
            if (!inTxn && Parent != null)
            {
                //Parent.CustomerChanged(this);
                Parent.ItemChanged(this);
            }
        }
        #endregion

    }

    public class CollectionSizes : CollectionBase, IBindingList
    {
        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        private ListChangedEventHandler onListChanged;

        //Change to the name of the object that holds the data in the next 4 items.
        public ItemSize this[int index]
        {
            get
            {
                return (ItemSize)(List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        public int Add(ItemSize value)
        {
            return List.Add(value);
        }

        public ItemSize AddNew()
        {
            return (ItemSize)((IBindingList)this).AddNew();
        }

        public void Remove(ItemSize value)
        {
            List.Remove(value);
        }

        //=========================================================================



        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }


        //Change to the name of the object that holds the data in this item.
        protected override void OnClear()
        {
            foreach (ItemSize c in List)
            {
                c.Parent = null;
            }
        }
        //=========================================================================


        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }



        //Change to the name of the object that holds the data here.
        //=========================================================================
        protected override void OnInsertComplete(int index, object value)
        {
            ItemSize c = (ItemSize)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ItemSize c = (ItemSize)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                ItemSize olddata = (ItemSize)oldValue;
                ItemSize newdata = (ItemSize)newValue;

                olddata.Parent = null;
                newdata.Parent = this;


                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        //=========================================================================


        //Change to the name of the object that holds the data here.
        //=========================================================================
        internal void ItemChanged(ItemSize sizeSet)
        {

            int index = List.IndexOf(sizeSet);

            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }


        //=========================================================================

        // Implements IBindingList.
        bool IBindingList.AllowEdit
        {
            get { return true; }
        }

        bool IBindingList.AllowNew
        {
            get { return true; }
        }

        bool IBindingList.AllowRemove
        {
            get { return true; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return false; }
        }


        // Events.
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }


        // Methods.
        object IBindingList.AddNew()
        {
            //IMPORTANT TO BE SURE THIS IS CORRECT OR AN INVALID CAST ERROR WILL
            //BE RAISED.
            ItemSize c = new ItemSize();
            List.Add(c);
            return c;
        }


        // Unsupported properties.
        bool IBindingList.IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }


        // Unsupported Methods.
        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

    }

    #endregion Size Collection

    #region Size Dimension Collection
    public class ItemSizeDimension : MinMaxItemBase, IEditableObject
    {

        #region Member Variables
        private CollectionSizeDimensions parent;
        private CollectionSizes colSizes = new CollectionSizes();
        private bool inTxn = false;
        #endregion Member Variables

        #region Public Properties

        public CollectionSizes collectionSizes
        {
            get { return colSizes; }
        }

        #endregion Properties

        internal CollectionSizeDimensions Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }


        #region Constructors

        public ItemSizeDimension()
            : base()
        {
        }


        public ItemSizeDimension(int intMethodRid, int intColorRid, int intMin,
                                int intMax, int intMult, int intRule, int intQty,
                                int intSizesRid, int intSizeCodeRid, int intDimensionsRid,
                                int intSglRid, eSizeMethodRowType rowTypeID)
            : base(intMethodRid, intColorRid, intMin,
                   intMax, intMult, intRule, intQty,
                   intSizesRid, intSizeCodeRid, intDimensionsRid,
                   intSglRid, rowTypeID)
        {

        }


        public ItemSizeDimension(int intMethodRid, DataRow drSizeDimension)
            : base(intMethodRid, drSizeDimension)
        {

        }


        #endregion Constructors

        #region Implements IEditableObject
        void IEditableObject.BeginEdit()
        {
            if (!inTxn)
            {
                //this.backupData = sizeDimensionData;
                inTxn = true;
            }
        }

        void IEditableObject.CancelEdit()
        {
            if (inTxn)
            {
                //this.sizeDimensionData = backupData;
                inTxn = false;
            }
        }

        void IEditableObject.EndEdit()
        {
            if (inTxn)
            {
                //backupData = new SizeDimensionData();
                inTxn = false;
            }
        }

        #endregion

        #region Methods
        private void OnItemChanged()
        {
            if (!inTxn && Parent != null)
            {
                //Parent.CustomerChanged(this);
                Parent.ItemChanged(this);
            }
        }
        #endregion

    }

    public class CollectionSizeDimensions : CollectionBase, IBindingList
    {
        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        private ListChangedEventHandler onListChanged;

        //Change to the name of the object that holds the data in the next 4 items.
        public ItemSizeDimension this[int index]
        {
            get
            {
                return (ItemSizeDimension)(List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        public int Add(ItemSizeDimension value)
        {
            return List.Add(value);
        }

        public ItemSizeDimension AddNew()
        {
            return (ItemSizeDimension)((IBindingList)this).AddNew();
        }

        public void Remove(ItemSizeDimension value)
        {
            List.Remove(value);
        }

        //=========================================================================



        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }


        //Change to the name of the object that holds the data in this item.
        protected override void OnClear()
        {
            foreach (ItemSizeDimension c in List)
            {
                c.Parent = null;
            }
        }
        //=========================================================================


        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }



        //Change to the name of the object that holds the data here.
        //=========================================================================
        protected override void OnInsertComplete(int index, object value)
        {
            ItemSizeDimension c = (ItemSizeDimension)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ItemSizeDimension c = (ItemSizeDimension)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                ItemSizeDimension olddata = (ItemSizeDimension)oldValue;
                ItemSizeDimension newdata = (ItemSizeDimension)newValue;

                olddata.Parent = null;
                newdata.Parent = this;


                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        //=========================================================================


        //Change to the name of the object that holds the data here.
        //=========================================================================
        internal void ItemChanged(ItemSizeDimension sizeDimensionSet)
        {

            int index = List.IndexOf(sizeDimensionSet);

            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }


        //=========================================================================

        // Implements IBindingList.
        bool IBindingList.AllowEdit
        {
            get { return true; }
        }

        bool IBindingList.AllowNew
        {
            get { return true; }
        }

        bool IBindingList.AllowRemove
        {
            get { return true; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return false; }
        }


        // Events.
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }


        // Methods.
        object IBindingList.AddNew()
        {
            //IMPORTANT TO BE SURE THIS IS CORRECT OR AN INVALID CAST ERROR WILL
            //BE RAISED.
            ItemSizeDimension c = new ItemSizeDimension();
            List.Add(c);
            return c;
        }

        // Unsupported properties.
        bool IBindingList.IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }


        // Unsupported Methods.
        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

    }

    #endregion Size Dimension Collection

    #region MinMaxItemBase
    public class MinMaxItemBase
    {
        #region Member Variables
        private int _method_rid;
        private int _min;
        private int _max;
        private int _mult;
        //				private int _rule;
        //				private int _qty;
        private int _color_code_rid;
        private int _sizes_rid;
        private int _size_code_rid;
        private int _dimensions_rid;
        private int _sgl_rid;
        private eSizeMethodRowType _row_type_id;

        #endregion

        #region Constructors

        public MinMaxItemBase()
        {
            _method_rid = Include.NoRID;
            _row_type_id = 0;
            _size_code_rid = Include.NoRID;
            _sizes_rid = Include.NoRID;
            _dimensions_rid = Include.NoRID;
            _min = 0;
            _max = int.MaxValue;
            _mult = Include.Undefined;
            //						_rule = Include.Undefined;
            //						_qty = 0;
            _sgl_rid = Include.NoRID;
            _color_code_rid = Include.NoRID;
        }

        //public MinMaxItemBase(int sglRid, int colorRid, int sizeCodeRid) // MID Track 3492 Size Need with Contraints not allocating correctly
        public MinMaxItemBase(int sglRid, int colorRid, SizeCodeProfile aSizeCodeProfile) // MID Track 3492 Size Need with Constraints not allocating correctly
        {
            _method_rid = Include.NoRID;
            _row_type_id = 0;
            // begin MID Track 3492 Size Need with Constraints not allocating correctly
            //_size_code_rid = sizeCodeRid;
            //_sizes_rid = Include.NoRID;      
            //_dimensions_rid = Include.NoRID; 
            _size_code_rid = aSizeCodeProfile.Key;
            _sizes_rid = aSizeCodeProfile.SizeCodePrimaryRID;
            _dimensions_rid = aSizeCodeProfile.SizeCodeSecondaryRID;
            // end MID Track 3492 Size Need with Constraints not allocating correctly
            _min = 0;
            _max = int.MaxValue;
            _mult = Include.Undefined;
            //						_rule = Include.Undefined;
            //						_qty = 0;
            _sgl_rid = sglRid;
            _color_code_rid = colorRid;
        }


        public MinMaxItemBase(int intMethodRid, int intColorRid, int intMin,
                            int intMax, int intMult, int intRule, int intQty,
                            int intSizesRid, int intSizeCodeRid, int intDimensionsRid,
                            int intSglRid, eSizeMethodRowType rowTypeID)
        {
            _method_rid = intMethodRid;
            _row_type_id = rowTypeID;
            _size_code_rid = intSizeCodeRid;
            _sizes_rid = intSizesRid;
            _dimensions_rid = intDimensionsRid;
            _min = intMin;
            _max = intMax;
            _mult = intMult;
            //						_rule = intRule;
            //						_qty = intQty;
            _sgl_rid = intSglRid;
            _color_code_rid = intColorRid;
        }


        public MinMaxItemBase(int intMethodRid, DataRow drSize)
        {
            _method_rid = intMethodRid;

            //SET VALUES FROM THE DATAROW
            //===========================
            _row_type_id = (eSizeMethodRowType)drSize["ROW_TYPE_ID"];
            _min = (drSize["SIZE_MIN"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SIZE_MIN"], CultureInfo.CurrentUICulture) : Include.UndefinedMinimum;
            _max = (drSize["SIZE_MAX"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SIZE_MAX"], CultureInfo.CurrentUICulture) : Include.UndefinedMaximum;
            _mult = (drSize["SIZE_MULT"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SIZE_MULT"], CultureInfo.CurrentUICulture) : Include.UndefinedMultiple;
            //						_rule = (drSize["SIZE_RULE"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SIZE_RULE"],CultureInfo.CurrentUICulture) : Include.UndefinedRule;
            //						_qty = (drSize["SIZE_QUANTITY"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SIZE_QUANTITY"],CultureInfo.CurrentUICulture) : Include.UndefinedQuantity;
            _color_code_rid = (drSize["COLOR_CODE_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["COLOR_CODE_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;
            _sizes_rid = (drSize["SIZES_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SIZES_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;
            _dimensions_rid = (drSize["DIMENSIONS_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["DIMENSIONS_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;
            _size_code_rid = (drSize["SIZE_CODE_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SIZE_CODE_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;
            _sgl_rid = (drSize["SGL_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SGL_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;

            //					foreach (DataColumn dc in drSize.Table.Columns)
            //					{
            //						switch (dc.ColumnName.ToUpper())
            //						{
            //							case "ROW_TYPE_ID":
            //								_row_type_id = (eSizeMethodRowType)drSize[dc];
            //								break;
            //							case "SIZE_MIN":
            //								_min = (drSize[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize[dc],CultureInfo.CurrentUICulture) : Include.UndefinedMinimum;
            //								break;
            //							case "SIZE_MAX":
            //								_max = (drSize[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize[dc],CultureInfo.CurrentUICulture) : Include.UndefinedMaximum;
            //								break;
            //							case "SIZE_MULT":
            //								_mult = (drSize[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize[dc],CultureInfo.CurrentUICulture) : Include.UndefinedMultiple;
            //								break;
            //							case "SIZE_RULE":
            //								_rule = (drSize[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize[dc],CultureInfo.CurrentUICulture) : Include.UndefinedRule;
            //								break;
            //							case "SIZE_QUANTITY":
            //								_qty = (drSize[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize[dc],CultureInfo.CurrentUICulture) : Include.UndefinedQuantity;
            //								break;
            //							case "COLOR_CODE_RID":
            //								_color_code_rid = (drSize[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize[dc],CultureInfo.CurrentUICulture) : Include.NoRID;
            //								break;
            //							case "SIZES_RID":
            //								_sizes_rid = (drSize[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize[dc],CultureInfo.CurrentUICulture) : Include.NoRID;
            //								break;
            //							case "DIMENSIONS_RID":
            //								_dimensions_rid = (drSize[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize[dc],CultureInfo.CurrentUICulture) : Include.NoRID;
            //								break;
            //							case "SIZE_CODE_RID":
            //								_size_code_rid = (drSize[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize[dc],CultureInfo.CurrentUICulture) : Include.NoRID;
            //								break;
            //							case "SGL_RID":
            //								_sgl_rid = (drSize[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize[dc],CultureInfo.CurrentUICulture) : Include.NoRID;
            //								break;
            //						}
            //					}
        }


        #endregion

        #region IMinMaxItem Members


        public int MethodRid
        {
            get { return _method_rid; }
            set { _method_rid = value; }
        }

        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public int Mult
        {
            get { return _mult; }
            set { _mult = value; }
        }

        //				public int Rule
        //				{
        //					get {return _rule;}
        //					set {_rule = value;}
        //				}
        //
        //				public int Qty
        //				{
        //					get {return _qty;}
        //					set {_qty = value;}
        //				}

        public int SizeCodeRid
        {
            get { return _size_code_rid; }
            set { _size_code_rid = value; }
        }

        public int SizesRid
        {
            get { return _sizes_rid; }
            set { _sizes_rid = value; }
        }

        public int ColorCodeRid
        {
            get { return _color_code_rid; }
            set { _color_code_rid = value; }
        }

        public int DimensionsRid
        {
            get { return _dimensions_rid; }
            set { _dimensions_rid = value; }
        }

        public int SglRid
        {
            get { return _sgl_rid; }
            set { _sgl_rid = value; }
        }


        public eSizeMethodRowType RowTypeID
        {
            get { return _row_type_id; }
            set { _row_type_id = value; }
        }
        #endregion
    }
    #endregion

    #endregion

    // begin MID Track 3619 Remove Fringe
    // #region Fringe Override Collections
    //	#region Set Collection AKA...Store Group Level
    //		public class ItemFringeSet:FringeItemBase, IEditableObject 
    //		{
    //
    //			#region Member Variables
    //				private CollectionFringeSets parent;
    //				private bool inTxn = false;
    //				private CollectionFringeFilters colFringeFilters = new CollectionFringeFilters();
    //				private CollectionFringeAllColors colAllColors = new CollectionFringeAllColors();
    //				private CollectionFringeColors colColors = new CollectionFringeColors();
    //			#endregion Member Variables
    // 
    //			#region Public Properties
    //				public CollectionFringeFilters collectionFringeFilters
    //				{
    //					get 
    //					{
    //						return colFringeFilters;
    //					}
    //				}
    //
    //
    //				public CollectionFringeAllColors collectionAllColors
    //				{
    //					get 
    //					{
    //						return colAllColors;
    //					}
    //				}
    //
    //
    //				public CollectionFringeColors collectionColors
    //				{
    //					get 
    //					{
    //						return colColors;
    //					}
    //				}
    //
    //			#endregion Properties
    //
    //			internal CollectionFringeSets Parent 
    //			{
    //				get 
    //				{
    //					return parent;
    //				}
    //				set 
    //				{
    //					parent = value ;
    //				}
    //			}
    //
    //
    //			#region Constructors
    //
    //			public ItemFringeSet(): base()
    //			{
    //			}
    //
    //
    //			public ItemFringeSet(int intMethodRid, int intColorRid, int intSglRid, 
    //								 int intFringeOrder, double dblFringeColorPercent, int intFringeCondition,
    //								 int intFringeCriteria, int intFringeOperator, int intFringeFilterValue,
    //								 eSizeMethodRowType rowTypeID) : base(intMethodRid, intColorRid, intSglRid, 
    //																	 intFringeOrder,dblFringeColorPercent, intFringeCondition,
    //																	 intFringeCriteria, intFringeOperator, intFringeFilterValue,
    //																	 rowTypeID)
    //			{
    //
    //			}
    //
    //
    //			public ItemFringeSet(int intMethodRid, DataRow drFringeItem):base(intMethodRid, drFringeItem)
    //			{
    //
    //			}
    //
    //
    //			#endregion Constructors
    //
    //			#region Implements IEditableObject
    //			void IEditableObject.BeginEdit() 
    //			{
    //				if (!inTxn) 
    //				{
    //					//this.backupData = setData;
    //					inTxn = true;
    //				}
    //			}
    //
    //			void IEditableObject.CancelEdit() 
    //			{
    //				if (inTxn) 
    //				{
    //					//this.setData = backupData;
    //					inTxn = false;
    //				}
    //			}
    //
    //			void IEditableObject.EndEdit() 
    //			{
    //				if (inTxn) 
    //				{
    //					//backupData = new SetData();
    //					inTxn = false;
    //				}
    //			}
    //
    //
    //
    //			#endregion
    //
    //			#region Methods
    //			private void OnItemChanged() 
    //			{
    //				if (!inTxn && Parent != null) 
    //				{
    //					//Parent.CustomerChanged(this);
    //					Parent.ItemChanged(this);
    //				}
    //			}
    //			#endregion
    //
    //		}
    //
    //		public class CollectionFringeSets : CollectionBase, IBindingList
    //		{
    //			private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
    //			private ListChangedEventHandler onListChanged;
    //
    //			//Change to the name of the object that holds the data in the next 4 items.
    //			public ItemFringeSet this[int index] 
    //			{
    //				get 
    //				{
    //					return (ItemFringeSet)(List[index]);
    //				}
    //				set 
    //				{
    //					List[index] = value;
    //				}
    //			}
    //
    //			public int Add (ItemFringeSet objFringeSet) 
    //			{
    //				return List.Add(objFringeSet);
    //			}
    //
    //			public ItemFringeSet AddNew() 
    //			{
    //				return (ItemFringeSet)((IBindingList)this).AddNew();
    //			}
    //
    //			public void Remove (ItemFringeSet objFringeSet) 
    //			{
    //				List.Remove(objFringeSet);
    //			}
    //
    //			//=========================================================================
    //
    //
    //
    //			protected virtual void OnListChanged(ListChangedEventArgs ev) 
    //			{
    //				if (onListChanged != null) 
    //				{
    //					onListChanged(this, ev);
    //				}
    //			}
    //						    
    //
    //			//Change to the name of the object that holds the data in this item.
    //			protected override void OnClear() 
    //			{
    //				foreach (ItemFringeSet c in List) 
    //				{
    //					c.Parent = null;
    //				}
    //			}
    //			//=========================================================================
    //
    //
    //			protected override void OnClearComplete() 
    //			{
    //				OnListChanged(resetEvent);
    //			}
    //
    //
    //
    //			//Change to the name of the object that holds the data here.
    //			//=========================================================================
    //			protected override void OnInsertComplete(int index, object value) 
    //			{
    //				ItemFringeSet c = (ItemFringeSet)value;
    //				c.Parent = this;
    //				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
    //			}
    //
    //			protected override void OnRemoveComplete(int index, object value) 
    //			{
    //				ItemFringeSet c = (ItemFringeSet)value;
    //				c.Parent = this;
    //				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
    //			}
    //
    //			protected override void OnSetComplete(int index, object oldValue, object newValue) 
    //			{
    //				if (oldValue != newValue) 
    //				{
    //
    //					ItemFringeSet oldvalue = (ItemFringeSet)oldValue;
    //					ItemFringeSet newvalue = (ItemFringeSet)newValue;
    //						            
    //					oldvalue.Parent = null;
    //					newvalue.Parent = this;
    //						            
    //						            
    //					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
    //				}
    //			}
    //
    //			//=========================================================================
    //
    //
    //			//Change to the name of the object that holds the data here.
    //			//=========================================================================
    //			// Called by Customer when it changes.
    //			internal void ItemChanged(ItemFringeSet fringeSet) 
    //			{
    //  						        
    //				int index = List.IndexOf(fringeSet);
    //						        
    //				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
    //			}
    //
    //
    //			//=========================================================================
    //
    //			// Implements IBindingList.
    //			bool IBindingList.AllowEdit 
    //			{ 
    //				get { return true ; }
    //			}
    //
    //			bool IBindingList.AllowNew 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //			bool IBindingList.AllowRemove 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //			bool IBindingList.SupportsChangeNotification 
    //			{ 
    //				get { return true ; }
    //			}
    //						    
    //			bool IBindingList.SupportsSearching 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //			bool IBindingList.SupportsSorting 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //
    //			// Events.
    //			public event ListChangedEventHandler ListChanged 
    //			{
    //				add 
    //				{
    //					onListChanged += value;
    //				}
    //				remove 
    //				{
    //					onListChanged -= value;
    //				}
    //			}
    //
    //
    //			// Methods.
    //			object IBindingList.AddNew() 
    //			{
    //				ItemFringeSet c = new ItemFringeSet();
    //				List.Add(c);
    //				return c;
    //			}
    //
    //
    //			// Unsupported properties.
    //			bool IBindingList.IsSorted 
    //			{ 
    //				get { throw new NotSupportedException(); }
    //			}
    //
    //			ListSortDirection IBindingList.SortDirection 
    //			{ 
    //				get { throw new NotSupportedException(); }
    //			}
    //
    //			PropertyDescriptor IBindingList.SortProperty 
    //			{ 
    //				get { throw new NotSupportedException(); }
    //			}
    //
    //
    //			// Unsupported Methods.
    //			void IBindingList.AddIndex(PropertyDescriptor property) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			int IBindingList.Find(PropertyDescriptor property, object key) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			void IBindingList.RemoveIndex(PropertyDescriptor property) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			void IBindingList.RemoveSort() 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //		}
    // 
    //	#endregion Set Collection AKA...Store Group Level
    //
    //	#region FringeFilter
    //		public class ItemFringeFilter:FringeItemBase, IEditableObject 
    //		{
    //
    //			#region Member Variables
    //			private CollectionFringeFilters parent;
    //			private bool inTxn = false;
    //			//				private CollectionAllColors colAllColors = new CollectionAllColors();
    //			//				private CollectionColors colColors = new CollectionColors();
    //			#endregion Member Variables
    //
    //			#region Public Properties
    //			//				public CollectionAllColors collectionAllColors
    //			//				{
    //			//					get 
    //			//					{
    //			//						return colAllColors;
    //			//					}
    //			//				}
    //			//
    //			//
    //			//				public CollectionColors collectionColors
    //			//				{
    //			//					get 
    //			//					{
    //			//						return colColors;
    //			//					}
    //			//				}
    //
    //
    //			#endregion Properties
    //
    //			internal CollectionFringeFilters Parent 
    //			{
    //				get 
    //				{
    //					return parent;
    //				}
    //				set 
    //				{
    //					parent = value ;
    //				}
    //			}
    //
    //
    //			#region Constructors
    //
    //			public ItemFringeFilter(): base()
    //			{
    //			}
    //
    //			public ItemFringeFilter(int intMethodRid, int intColorRid, int intSglRid, 
    //									int intFringeOrder, double dblFringeColorPercent, int intFringeCondition,
    //									int intFringeCriteria, int intFringeOperator, int intFringeFilterValue,
    //									eSizeMethodRowType rowTypeID) : base(intMethodRid, intColorRid, intSglRid, 
    //																		 intFringeOrder,dblFringeColorPercent, intFringeCondition,
    //																		 intFringeCriteria, intFringeOperator, intFringeFilterValue,
    //																		 rowTypeID)
    //			{
    //
    //			}
    //
    //
    //			public ItemFringeFilter(int intMethodRid, DataRow drFringeItem):base(intMethodRid, drFringeItem)
    //			{
    //
    //			}
    //
    //
    //			#endregion Constructors
    //
    //			#region Implements IEditableObject
    //			void IEditableObject.BeginEdit() 
    //			{
    //				if (!inTxn) 
    //				{
    //					//this.backupData = setData;
    //					inTxn = true;
    //				}
    //			}
    //
    //			void IEditableObject.CancelEdit() 
    //			{
    //				if (inTxn) 
    //				{
    //					//this.setData = backupData;
    //					inTxn = false;
    //				}
    //			}
    //
    //			void IEditableObject.EndEdit() 
    //			{
    //				if (inTxn) 
    //				{
    //					//backupData = new SetData();
    //					inTxn = false;
    //				}
    //			}
    //
    //
    //
    //			#endregion
    //
    //			#region Methods
    //			private void OnItemChanged() 
    //			{
    //				if (!inTxn && Parent != null) 
    //				{
    //					//Parent.CustomerChanged(this);
    //					Parent.ItemChanged(this);
    //				}
    //			}
    //			#endregion
    //
    //		}
    //
    //		public class CollectionFringeFilters : CollectionBase, IBindingList
    //		{
    //			private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
    //			private ListChangedEventHandler onListChanged;
    //
    //			//Change to the name of the object that holds the data in the next 4 items.
    //			public ItemFringeFilter this[int index] 
    //			{
    //				get 
    //				{
    //					return (ItemFringeFilter)(List[index]);
    //				}
    //				set 
    //				{
    //					List[index] = value;
    //				}
    //			}
    // 
    //			public int Add (ItemFringeFilter objFringeFilter) 
    //			{
    //				return List.Add(objFringeFilter);
    //			}
    //
    //			public ItemFringeFilter AddNew() 
    //			{
    //				return (ItemFringeFilter)((IBindingList)this).AddNew();
    //			}
    //
    //			public void Remove (ItemFringeFilter objFringeFilter) 
    //			{
    //				List.Remove(objFringeFilter);
    //			}
    //
    //			//=========================================================================
    //
    //
    //
    // 			protected virtual void OnListChanged(ListChangedEventArgs ev) 
    //			{
    //				if (onListChanged != null) 
    //				{
    //					onListChanged(this, ev);
    //				}
    //			}
    //								    
    //
    //			//Change to the name of the object that holds the data in this item.
    //			protected override void OnClear() 
    //			{
    //				foreach (ItemFringeFilter c in List) 
    //				{
    //					c.Parent = null;
    //				}
    //			}
    //			//=========================================================================
    //
    //
    //			protected override void OnClearComplete() 
    //			{
    //				OnListChanged(resetEvent);
    //			}
    //
    //
    //
    //			//Change to the name of the object that holds the data here.
    //			//=========================================================================
    //			protected override void OnInsertComplete(int index, object value) 
    //			{
    //				ItemFringeFilter c = (ItemFringeFilter)value;
    //				c.Parent = this;
    //				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
    //			}
    //
    //			protected override void OnRemoveComplete(int index, object value) 
    //			{
    //				ItemFringeFilter c = (ItemFringeFilter)value;
    //				c.Parent = this;
    //				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
    //			}
    //
    //			protected override void OnSetComplete(int index, object oldValue, object newValue) 
    //			{
    //				if (oldValue != newValue) 
    //				{
    //
    //					ItemFringeFilter oldvalue = (ItemFringeFilter)oldValue;
    //					ItemFringeFilter newvalue = (ItemFringeFilter)newValue;
    //								            
    //					oldvalue.Parent = null;
    //					newvalue.Parent = this;
    //								            
    //								            
    //					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
    //				}
    //			}
    //
    //			//=========================================================================
    //
    //
    //			//Change to the name of the object that holds the data here.
    //			//=========================================================================
    //			// Called by Customer when it changes.
    //			internal void ItemChanged(ItemFringeFilter fringeFilter) 
    //			{
    //								        
    //				int index = List.IndexOf(fringeFilter);
    //								        
    //				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
    //			}
    //
    //
    //			//=========================================================================
    //
    //			// Implements IBindingList.
    //			bool IBindingList.AllowEdit 
    //			{ 
    //				get { return true ; }
    //			}
    //
    //			bool IBindingList.AllowNew 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //			bool IBindingList.AllowRemove 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //			bool IBindingList.SupportsChangeNotification 
    //			{ 
    //				get { return true ; }
    //			}
    //								    
    //			bool IBindingList.SupportsSearching 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //			bool IBindingList.SupportsSorting 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //
    //			// Events.
    //			public event ListChangedEventHandler ListChanged 
    //			{
    //				add 
    //				{
    //					onListChanged += value;
    //				}
    //				remove 
    //				{
    //					onListChanged -= value;
    //				}
    //			}
    //
    //
    //			// Methods.
    //			object IBindingList.AddNew() 
    //			{
    //				ItemFringeFilter c = new ItemFringeFilter();
    //				List.Add(c);
    //				return c;
    //			}
    //
    //
    //			// Unsupported properties.
    //			bool IBindingList.IsSorted 
    //			{ 
    //				get { throw new NotSupportedException(); }
    //			}
    //
    //			ListSortDirection IBindingList.SortDirection 
    //			{ 
    //				get { throw new NotSupportedException(); }
    //			}
    //
    //			PropertyDescriptor IBindingList.SortProperty 
    //			{ 
    //				get { throw new NotSupportedException(); }
    //			}
    //
    //
    //			// Unsupported Methods.
    //			void IBindingList.AddIndex(PropertyDescriptor property) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			int IBindingList.Find(PropertyDescriptor property, object key) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			void IBindingList.RemoveIndex(PropertyDescriptor property) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			void IBindingList.RemoveSort() 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //		}
    //
    //	#endregion
    //
    //	#region All Colors
    //		public class ItemFringeAllColor:FringeItemBase, IEditableObject 
    //		{
    //
    //			#region Member Variables
    //			private CollectionFringeAllColors parent;
    //			private bool inTxn = false;
    //			private CollectionFringeFilters colFringeFilters = new CollectionFringeFilters();
    //			#endregion Member Variables
    //
    //			#region Public Properties
    //				public CollectionFringeFilters collectionFringeFilters
    //				{
    //					get 
    //					{
    //						return colFringeFilters;
    //					}
    //				}
    //
    //			#endregion Properties
    //
    //			internal CollectionFringeAllColors Parent 
    //			{
    //				get 
    //				{
    //					return parent;
    //				}
    //				set 
    //				{
    //					parent = value ;
    //				}
    //			}
    //
    //
    //			#region Constructors
    //
    //			public ItemFringeAllColor(): base()
    //			{
    //			}
    //  
    //			public ItemFringeAllColor(int intMethodRid, int intColorRid, int intSglRid, 
    //									int intFringeOrder, double dblFringeColorPercent, int intFringeCondition,
    //									int intFringeCriteria, int intFringeOperator, int intFringeFilterValue,
    //									eSizeMethodRowType rowTypeID) : base(intMethodRid, intColorRid, intSglRid, 
    //																		intFringeOrder,dblFringeColorPercent, intFringeCondition,
    //																		intFringeCriteria, intFringeOperator, intFringeFilterValue,
    //																		rowTypeID)
    //			{
    //
    //			}
    //
    //
    //			public ItemFringeAllColor(int intMethodRid, DataRow drFringeItem):base(intMethodRid, drFringeItem)
    //			{
    //
    //			}
    //
    //
    //			#endregion Constructors
    //
    //			#region Implements IEditableObject
    //			void IEditableObject.BeginEdit() 
    //			{
    //				if (!inTxn) 
    //				{
    //					//this.backupData = setData;
    //					inTxn = true;
    //				}
    //			}
    //
    //			void IEditableObject.CancelEdit() 
    //			{
    //				if (inTxn) 
    //				{
    //					//this.setData = backupData;
    //					inTxn = false;
    //				}
    //			}
    //
    //			void IEditableObject.EndEdit() 
    //			{
    //				if (inTxn) 
    //				{
    //					//backupData = new SetData();
    //					inTxn = false;
    //				}
    //			}
    //
    //
    //
    //			#endregion
    //
    //			#region Methods
    //			private void OnItemChanged() 
    //			{
    //				if (!inTxn && Parent != null) 
    //				{
    //					//Parent.CustomerChanged(this);
    //					Parent.ItemChanged(this);
    //				}
    //			}
    //			#endregion
    //
    //		}
    //
    //		public class CollectionFringeAllColors : CollectionBase, IBindingList
    //		{
    //			private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
    //			private ListChangedEventHandler onListChanged;
    //
    //			//Change to the name of the object that holds the data in the next 4 items.
    //			public ItemFringeAllColor this[int index] 
    //			{
    //				get 
    //				{
    //					return (ItemFringeAllColor)(List[index]);
    //				}
    //				set 
    //				{
    //					List[index] = value;
    //				}
    //			}
    //
    //			public int Add (ItemFringeAllColor objFringeAllColor) 
    //			{
    //				return List.Add(objFringeAllColor);
    //			}
    //
    //			public ItemFringeAllColor AddNew() 
    //			{
    //				return (ItemFringeAllColor)((IBindingList)this).AddNew();
    //			}
    //
    //			public void Remove (ItemFringeAllColor objFringeAllColor) 
    //			{
    //				List.Remove(objFringeAllColor);
    //			}
    //
    //			//=========================================================================
    //
    //
    //
    //			protected virtual void OnListChanged(ListChangedEventArgs ev) 
    //			{
    //				if (onListChanged != null) 
    //				{
    //					onListChanged(this, ev);
    //				}
    //			}
    //										    
    //
    //			//Change to the name of the object that holds the data in this item.
    //			protected override void OnClear() 
    //			{
    //				foreach (ItemFringeAllColor c in List) 
    //				{
    //					c.Parent = null;
    //				}
    //			}
    //			//=========================================================================
    //
    //
    //			protected override void OnClearComplete() 
    //			{
    //				OnListChanged(resetEvent);
    //			}
    //
    //
    //
    //			//Change to the name of the object that holds the data here.
    //			//=========================================================================
    //			protected override void OnInsertComplete(int index, object value) 
    //			{
    //				ItemFringeAllColor c = (ItemFringeAllColor)value;
    //				c.Parent = this;
    //				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
    //			}
    //
    //			protected override void OnRemoveComplete(int index, object value) 
    //			{
    //				ItemFringeAllColor c = (ItemFringeAllColor)value;
    //				c.Parent = this;
    //				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
    //			}
    //
    //			protected override void OnSetComplete(int index, object oldValue, object newValue) 
    //			{
    //				if (oldValue != newValue) 
    //				{
    //
    //					ItemFringeAllColor oldvalue = (ItemFringeAllColor)oldValue;
    //					ItemFringeAllColor newvalue = (ItemFringeAllColor)newValue;
    //										            
    //					oldvalue.Parent = null;
    //					newvalue.Parent = this;
    //										            
    //										            
    //					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
    //				}
    //			}
    //
    //			//=========================================================================
    //
    //
    //			//Change to the name of the object that holds the data here.
    //			//=========================================================================
    //			// Called by Customer when it changes.
    //			internal void ItemChanged(ItemFringeAllColor fringeAllColor) 
    //			{
    //										        
    //				int index = List.IndexOf(fringeAllColor);
    //										        
    //				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
    //			}
    //
    //
    //			//=========================================================================
    //
    //			// Implements IBindingList.
    //			bool IBindingList.AllowEdit 
    //			{ 
    //				get { return true ; }
    //			}
    //
    //			bool IBindingList.AllowNew 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //			bool IBindingList.AllowRemove 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //			bool IBindingList.SupportsChangeNotification 
    //			{ 
    //				get { return true ; }
    //			}
    //										    
    //			bool IBindingList.SupportsSearching 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //			bool IBindingList.SupportsSorting 
    //			{ 
    //				get { return false ; }
    //			}
    // 
    //
    //			// Events.
    //			public event ListChangedEventHandler ListChanged 
    //			{
    //				add 
    //				{
    //					onListChanged += value;
    //				}
    //				remove 
    //				{
    //					onListChanged -= value;
    //				}
    //			}
    //
    //
    //			// Methods.
    //			object IBindingList.AddNew() 
    //			{
    //				ItemFringeAllColor c = new ItemFringeAllColor();
    //				List.Add(c);
    //				return c;
    //			}
    //
    //
    //			// Unsupported properties.
    //			bool IBindingList.IsSorted 
    //			{ 
    //				get { throw new NotSupportedException(); }
    //			}
    //
    //			ListSortDirection IBindingList.SortDirection 
    //			{ 
    //				get { throw new NotSupportedException(); }
    //			}
    //
    //			PropertyDescriptor IBindingList.SortProperty 
    //			{ 
    //				get { throw new NotSupportedException(); }
    //			}
    //
    //
    //			// Unsupported Methods.
    //			void IBindingList.AddIndex(PropertyDescriptor property) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			int IBindingList.Find(PropertyDescriptor property, object key) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			void IBindingList.RemoveIndex(PropertyDescriptor property) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			void IBindingList.RemoveSort() 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //		}
    //
    //	#endregion
    //
    //	#region Colors
    //		public class ItemFringeColor:FringeItemBase, IEditableObject 
    //		{
    //
    //			#region Member Variables
    //				private CollectionFringeColors parent;
    //				private bool inTxn = false;
    //				private CollectionFringeFilters colFringeFilters = new CollectionFringeFilters();
    //			#endregion Member Variables
    //
    //			#region Public Properties
    //				public CollectionFringeFilters collectionFringeFilters
    //				{
    //					get 
    //					{
    //						return colFringeFilters;
    //					}
    //				}
    //			#endregion Properties
    //
    //			internal CollectionFringeColors Parent 
    //			{
    //				get 
    //				{
    //					return parent;
    //				}
    //				set 
    //				{
    //					parent = value ;
    //				}
    //			}
    //
    //
    //			#region Constructors
    //
    //			public ItemFringeColor(): base()
    //			{
    //			}
    //
    //			public ItemFringeColor(int intMethodRid, int intColorRid, int intSglRid, 
    //								int intFringeOrder, double dblFringeColorPercent, int intFringeCondition,
    //								int intFringeCriteria, int intFringeOperator, int intFringeFilterValue,
    //								eSizeMethodRowType rowTypeID) : base(intMethodRid, intColorRid, intSglRid, 
    //																		intFringeOrder,dblFringeColorPercent, intFringeCondition,
    //																		intFringeCriteria, intFringeOperator, intFringeFilterValue,
    //																		rowTypeID)
    //			{
    //
    //			}
    //
    //
    //			public ItemFringeColor(int intMethodRid, DataRow drFringeItem):base(intMethodRid, drFringeItem)
    //			{
    //
    //			}
    //
    //
    //			#endregion Constructors
    //
    //			#region Implements IEditableObject
    //			void IEditableObject.BeginEdit() 
    //			{
    //				if (!inTxn) 
    //				{
    //					//this.backupData = setData;
    //					inTxn = true;
    //				}
    //			}
    //
    //			void IEditableObject.CancelEdit() 
    //			{
    //				if (inTxn) 
    //				{
    //					//this.setData = backupData;
    //					inTxn = false;
    //				}
    //			}
    //
    //			void IEditableObject.EndEdit() 
    //			{
    //				if (inTxn) 
    //				{
    //					//backupData = new SetData();
    //					inTxn = false;
    //				}
    //			}
    //
    // 
    //
    //			#endregion
    //
    //			#region Methods
    //			private void OnItemChanged() 
    //			{
    //				if (!inTxn && Parent != null) 
    //				{
    //					//Parent.CustomerChanged(this);
    //					Parent.ItemChanged(this);
    //				}
    //			}
    //			#endregion
    //
    //		}
    //
    //		public class CollectionFringeColors : CollectionBase, IBindingList
    //		{
    //			private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
    //			private ListChangedEventHandler onListChanged;
    //
    //			//Change to the name of the object that holds the data in the next 4 items.
    //			public ItemFringeColor this[int index] 
    //			{
    //				get 
    //				{
    //					return (ItemFringeColor)(List[index]);
    //				}
    //				set 
    //				{
    //					List[index] = value;
    //				}
    //			}
    //
    //			public int Add (ItemFringeColor objFringeColor) 
    //			{
    //				return List.Add(objFringeColor);
    //			}
    //
    //			public ItemFringeColor AddNew() 
    //			{
    //				return (ItemFringeColor)((IBindingList)this).AddNew();
    //			}
    //
    //			public void Remove (ItemFringeColor objFringeColor) 
    //			{
    //				List.Remove(objFringeColor);
    //			}
    //
    // 			//=========================================================================
    //
    //
    //
    //			protected virtual void OnListChanged(ListChangedEventArgs ev) 
    //			{
    //				if (onListChanged != null) 
    //				{
    //					onListChanged(this, ev);
    //				}
    //			}
    //												    
    //
    //			//Change to the name of the object that holds the data in this item.
    //			protected override void OnClear() 
    //			{
    //				foreach (ItemFringeColor c in List) 
    //				{
    //					c.Parent = null;
    //				}
    //			}
    //			//=========================================================================
    //
    //
    //			protected override void OnClearComplete() 
    //			{
    //				OnListChanged(resetEvent);
    //			}
    //
    // 
    //
    //			//Change to the name of the object that holds the data here.
    //			//=========================================================================
    //			protected override void OnInsertComplete(int index, object value) 
    //			{
    //				ItemFringeColor c = (ItemFringeColor)value;
    //				c.Parent = this;
    //				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
    //			}
    //
    //			protected override void OnRemoveComplete(int index, object value) 
    //			{
    //				ItemFringeColor c = (ItemFringeColor)value;
    //				c.Parent = this;
    //				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
    //			}
    //
    //			protected override void OnSetComplete(int index, object oldValue, object newValue) 
    //			{
    //				if (oldValue != newValue) 
    //				{
    //
    //					ItemFringeColor oldvalue = (ItemFringeColor)oldValue;
    //					ItemFringeColor newvalue = (ItemFringeColor)newValue;
    //												            
    //					oldvalue.Parent = null;
    //					newvalue.Parent = this;
    //												            
    //												            
    //					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
    //				}
    //			}
    //
    //			//=========================================================================
    //
    //
    //			//Change to the name of the object that holds the data here.
    //			//=========================================================================
    //			// Called by Customer when it changes.
    //			internal void ItemChanged(ItemFringeColor fringeColor) 
    //			{
    //												        
    //				int index = List.IndexOf(fringeColor);
    //												        
    //				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
    //			}
    //
    //
    //			//=========================================================================
    //
    //			// Implements IBindingList.
    //			bool IBindingList.AllowEdit 
    //			{ 
    //				get { return true ; }
    //			}
    //
    //			bool IBindingList.AllowNew 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //			bool IBindingList.AllowRemove 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //			bool IBindingList.SupportsChangeNotification 
    //			{ 
    //				get { return true ; }
    //			}
    //												    
    //			bool IBindingList.SupportsSearching 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //			bool IBindingList.SupportsSorting 
    //			{ 
    //				get { return false ; }
    //			}
    //
    //
    //			// Events.
    //			public event ListChangedEventHandler ListChanged 
    //			{
    //				add 
    //				{
    //					onListChanged += value;
    //				}
    //				remove 
    //				{
    //					onListChanged -= value;
    //				}
    //			}
    //
    //
    //			// Methods.
    //			object IBindingList.AddNew() 
    //			{
    //				ItemFringeColor c = new ItemFringeColor();
    //				List.Add(c);
    //				return c;
    //			}
    //
    //
    //			// Unsupported properties.
    //			bool IBindingList.IsSorted 
    //			{ 
    //				get { throw new NotSupportedException(); }
    //			}
    //
    //			ListSortDirection IBindingList.SortDirection 
    //			{ 
    //				get { throw new NotSupportedException(); }
    //			}
    //
    //			PropertyDescriptor IBindingList.SortProperty 
    //			{ 
    //				get { throw new NotSupportedException(); }
    //			}
    //
    //
    //			// Unsupported Methods.
    //			void IBindingList.AddIndex(PropertyDescriptor property) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			int IBindingList.Find(PropertyDescriptor property, object key) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			void IBindingList.RemoveIndex(PropertyDescriptor property) 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //			void IBindingList.RemoveSort() 
    //			{
    //				throw new NotSupportedException(); 
    //			}
    //
    //		}
    //
    //	#endregion
    //
    //	#region FringeItemBase
    //		public class FringeItemBase
    //		{
    //			#region Member Variables
    //				private int _method_rid;
    //				private int _sgl_rid;
    //				private int _fringe_order;
    //				private int _color_code_rid;
    //				private double _fringe_color_percent;
    //				private int _fringe_condition_enum;
    //				private int _fringe_criteria_enum;
    //				private int _fringe_operator_enum;
    //				private int _fringe_filter_value;
    //				private eSizeMethodRowType _row_type_id;
    //				private int _fringe_rid; //Value generated dynamically
    //			#endregion
    //
    //			#region Constructors
    //
    //				public FringeItemBase()
    //				{
    //
    //				}
    //
    //
    //				public FringeItemBase(int intMethodRid, int intColorRid, int intSglRid, 
    //									int intFringeOrder, double dblFringeColorPercent, int intFringeCondition,
    //									int intFringeCriteria, int intFringeOperator, int intFringeFilterValue,
    //									eSizeMethodRowType rowTypeID)
    //				{
    //					_method_rid = intMethodRid;
    //					_row_type_id = rowTypeID;
    //					_sgl_rid = intSglRid;
    //					_fringe_order = intFringeOrder;
    //					_fringe_color_percent = dblFringeColorPercent;
    //					_fringe_condition_enum = intFringeCondition;
    //					_fringe_criteria_enum = intFringeCriteria;
    //					_fringe_operator_enum = intFringeOperator;
    //					_fringe_filter_value = intFringeFilterValue;
    //					_color_code_rid = intColorRid;
    //				}
    //
    //
    //				public FringeItemBase(int intMethodRid, DataRow drFringe)
    //				{
    //					_method_rid = intMethodRid;
    //
    //					//DEFAULT VALUES
    //					_sgl_rid = Include.NoRID;
    //					_fringe_order = Include.Undefined;
    //					_fringe_color_percent = Include.Undefined;
    //					_fringe_condition_enum = Include.Undefined;
    //					_fringe_criteria_enum = Include.Undefined;
    //					_fringe_operator_enum = Include.Undefined;
    //					_fringe_filter_value = Include.Undefined;
    //					_color_code_rid = Include.NoRID;
    //					_fringe_rid = Include.NoRID;
    //
    //					//SET VALUES FROM THE DATAROW
    //					//===========================
    //					foreach (DataColumn dc in drFringe.Table.Columns)
    //					{
    //						switch (dc.ColumnName.ToUpper())
    //						{
    //							case "ROW_TYPE_ID":
    //								_row_type_id = (eSizeMethodRowType)drFringe[dc];
    //								break;
    //							case "FRINGE_ORDER":
    //								_fringe_order = (drFringe[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drFringe[dc],CultureInfo.CurrentUICulture) : Include.Undefined;
    //								break;
    //							case "FRINGE_FILTER_VALUE":
    //								_fringe_filter_value = (drFringe[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drFringe[dc],CultureInfo.CurrentUICulture) : Include.Undefined;
    //								break;
    //							case "FRINGE_OPERATOR_ENUM":
    //								_fringe_operator_enum = (drFringe[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drFringe[dc],CultureInfo.CurrentUICulture) : Include.Undefined;
    //								break;
    //							case "FRINGE_CRITERIA_ENUM":
    //								_fringe_criteria_enum = (drFringe[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drFringe[dc],CultureInfo.CurrentUICulture) : Include.Undefined;
    //								break;
    //							case "COLOR_CODE_RID":
    //								_color_code_rid = (drFringe[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drFringe[dc],CultureInfo.CurrentUICulture) : Include.NoRID;
    //								break;
    //							case "FRINGE_CONDITION_ENUM":
    //								_fringe_condition_enum = (drFringe[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drFringe[dc],CultureInfo.CurrentUICulture) : Include.Undefined;
    //								break;
    //							case "FRINGE_COLOR_PERCENT":
    //								_fringe_color_percent = (drFringe[dc].ToString().Trim() != string.Empty) ? Convert.ToDouble(drFringe[dc],CultureInfo.CurrentUICulture) : Include.Undefined;
    //								break;
    //							case "SGL_RID":
    //								_sgl_rid = (drFringe[dc].ToString().Trim() != string.Empty) ? Convert.ToInt32(drFringe[dc],CultureInfo.CurrentUICulture) : Include.NoRID;
    //								break;
    //						}
    //					}
    //				}
    //
    //
    //			#endregion
    //
    //			#region IMinMaxItem Members
    //
    //				public int MethodRid
    //				{
    //					get {return _method_rid;}
    //					set {_method_rid = value;}	
    //				}
    //
    //				public int FringeRid
    //				{
    //					get {return _fringe_rid;}
    //					set {_fringe_rid = value;}
    //				}
    //
    //				public int SglRid
    //				{
    //					get {return _sgl_rid;}
    //					set {_sgl_rid = value;}
    //				}
    //
    //				public int ColorCodeRid
    //				{
    //					get {return _color_code_rid;}
    //					set {_color_code_rid = value;}
    //				}
    //
    //				public int FringeOrder
    //				{
    //					get {return _fringe_order;}
    //					set {_fringe_order = value;}
    //				}
    // 
    //				public double FringeColorPercent
    //				{
    //					get {return _fringe_color_percent;}
    //					set {_fringe_color_percent = value;}
    //				}
    //
    //				public int FringeCondition
    //				{
    //					get {return _fringe_condition_enum;}
    //					set {_fringe_condition_enum = value;}
    //				}
    //
    //				public int FringeCriteria
    //				{
    //					get {return _fringe_criteria_enum;}
    //					set {_fringe_criteria_enum = value;}
    //				}
    //
    //				public int FringeOperator
    //				{
    //					get {return _fringe_operator_enum;}
    //					set {_fringe_operator_enum = value;}
    //				}
    //
    //				public int FringeFilterValue
    //				{
    //					get {return _fringe_filter_value;}
    //					set {_fringe_filter_value = value;}
    //				}
    //
    //				public eSizeMethodRowType RowTypeID
    //				{
    //					get {return _row_type_id;}
    //					set {_row_type_id = value;}
    //				}
    //				#endregion
    //		}
    //	#endregion
    //#endregion
    // end MID Track 3619 Remove Fringe

    #region All Size Rule Collections
    public class ItemRuleSet : IEditableObject
    {
        private CollectionRuleSets parent;
        private int _method_rid;
        private int _sgl_rid;
        private int _all_size_rule;
        private int _all_size_qty;
        private eSizeMethodRowType _row_type_id;
        private bool inTxn = false;
        private CollectionRuleAllColors colAllColors = new CollectionRuleAllColors();
        private CollectionRuleColors colColors = new CollectionRuleColors();


        #region Public Properties

        public eSizeMethodRowType RowTypeID
        {
            get { return _row_type_id; }
            set { _row_type_id = value; }
        }


        public CollectionRuleAllColors collectionRuleAllColors
        {
            get
            {
                return colAllColors;
            }
        }


        public CollectionRuleColors collectionRuleColors
        {
            get
            {
                return colColors;
            }
        }

        public int MethodRid
        {
            get { return _method_rid; }
            set { _method_rid = value; }
        }


        public int SglRid
        {
            get { return _sgl_rid; }
            set { _sgl_rid = value; }
        }

        public int Rule
        {
            get { return _all_size_rule; }
            set { _all_size_rule = value; }
        }


        public int Qty
        {
            get { return _all_size_qty; }
            set { _all_size_qty = value; }
        }


        #endregion Properties

        internal CollectionRuleSets Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        public ItemRuleSet()// : base()
        {
        }

        public ItemRuleSet(int intMethodRid, int intSglRid, int intRule, int intQty, eSizeMethodRowType rowTypeID)
            : base()
        {

            _method_rid = intMethodRid;
            _sgl_rid = intSglRid;
            _all_size_rule = intRule;
            _all_size_qty = intQty;
            _row_type_id = rowTypeID;

        }


        public ItemRuleSet(int intMethodRid, DataRow drSetItem)// :base()
        {

            _method_rid = intMethodRid;

            //SET VALUES FROM THE DATAROW
            //===========================
            _sgl_rid = Convert.ToInt32(drSetItem["SGL_RID"], CultureInfo.CurrentUICulture);
            _row_type_id = (eSizeMethodRowType)drSetItem["ROW_TYPE_ID"];
            _all_size_rule = (drSetItem["SIZE_RULE"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSetItem["SIZE_RULE"], CultureInfo.CurrentUICulture) : Include.UndefinedRule;
            _all_size_qty = (drSetItem["SIZE_QUANTITY"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSetItem["SIZE_QUANTITY"], CultureInfo.CurrentUICulture) : Include.UndefinedQuantity;

        }

        #region Implements IEditableObject
        void IEditableObject.BeginEdit()
        {
            if (!inTxn)
            {
                //this.backupData = setData;
                inTxn = true;
            }
        }

        void IEditableObject.CancelEdit()
        {
            if (inTxn)
            {
                //this.setData = backupData;
                inTxn = false;
            }
        }

        void IEditableObject.EndEdit()
        {
            if (inTxn)
            {
                //backupData = new SetData();
                inTxn = false;
            }
        }
        #endregion

        #region Methods
        private void OnItemChanged()
        {
            if (!inTxn && Parent != null)
            {
                //Parent.CustomerChanged(this);
                Parent.ItemChanged(this);
            }
        }
        #endregion

    }

    public class CollectionRuleSets : CollectionBase, IBindingList
    {
        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        private ListChangedEventHandler onListChanged;

        //Change to the name of the object that holds the data in the next 4 items.
        public ItemRuleSet this[int index]
        {
            get
            {
                return (ItemRuleSet)(List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        public int Add(ItemRuleSet objItemSet)
        {
            return List.Add(objItemSet);
        }

        public ItemRuleSet AddNew()
        {
            return (ItemRuleSet)((IBindingList)this).AddNew();
        }

        public void Remove(ItemRuleSet objItemSet)
        {
            List.Remove(objItemSet);
        }

        //=========================================================================



        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }


        //Change to the name of the object that holds the data in this item.
        protected override void OnClear()
        {
            foreach (ItemRuleSet c in List)
            {
                c.Parent = null;
            }
        }
        //=========================================================================


        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }



        //Change to the name of the object that holds the data here.
        //=========================================================================
        protected override void OnInsertComplete(int index, object value)
        {
            ItemRuleSet c = (ItemRuleSet)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ItemRuleSet c = (ItemRuleSet)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                ItemRuleSet oldcust = (ItemRuleSet)oldValue;
                ItemRuleSet newcust = (ItemRuleSet)newValue;

                oldcust.Parent = null;
                newcust.Parent = this;


                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        //=========================================================================


        //Change to the name of the object that holds the data here.
        //=========================================================================
        // Called by Customer when it changes.
        internal void ItemChanged(ItemRuleSet itemSet)
        {

            int index = List.IndexOf(itemSet);

            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }


        //=========================================================================

        // Implements IBindingList.
        bool IBindingList.AllowEdit
        {
            get { return true; }
        }

        bool IBindingList.AllowNew
        {
            get { return false; }
        }

        bool IBindingList.AllowRemove
        {
            get { return false; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return false; }
        }


        // Events.
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }


        // Methods.
        object IBindingList.AddNew()
        {
            ItemRuleSet c = new ItemRuleSet();
            List.Add(c);
            return c;
        }


        // Unsupported properties.
        bool IBindingList.IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }


        // Unsupported Methods.
        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

    }


    #region All Color Collection
    public class ItemRuleAllColor : RuleItemBase, IEditableObject
    {

        #region Member Variables
        private CollectionRuleAllColors parent;
        private bool inTxn = false;
        //private CollectionSizes colAllColorSizes = new CollectionSizes();
        private CollectionRuleSizeDimensions colAllColorSizeDimensions = new CollectionRuleSizeDimensions();
        #endregion Member Variables

        #region Public Properties

        //				public CollectionSizes collectionSizes
        //				{
        //					get {return colAllColorSizes;}
        //				}
        //
        public CollectionRuleSizeDimensions collectionRuleSizeDimensions
        {
            get { return colAllColorSizeDimensions; }
        }

        #endregion Properties

        internal CollectionRuleAllColors Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }


        #region Constructors

        public ItemRuleAllColor()
            : base()
        {
        }


        public ItemRuleAllColor(int intMethodRid, int intColorRid, int intRule, int intQty,
            int intSizesRid, int intSizeCodeRid, int intDimensionsRid,
            int intSglRid, eSizeMethodRowType rowTypeID)
            : base(intMethodRid, intColorRid, intRule, intQty,
                intSizesRid, intSizeCodeRid, intDimensionsRid,
                intSglRid, rowTypeID)
        {

        }


        public ItemRuleAllColor(int intMethodRid, DataRow drAllColorItem)
            : base(intMethodRid, drAllColorItem)
        {

        }



        #endregion Constructors


        #region Implements IEditableObject
        void IEditableObject.BeginEdit()
        {
            if (!inTxn)
            {
                //this.backupData = allColorData;
                inTxn = true;
            }
        }

        void IEditableObject.CancelEdit()
        {
            if (inTxn)
            {
                //this.allColorData = backupData;
                inTxn = false;
            }
        }

        void IEditableObject.EndEdit()
        {
            if (inTxn)
            {
                //backupData = new AllColorData();
                inTxn = false;
            }
        }



        #endregion


        #region Methods
        private void OnItemChanged()
        {
            if (!inTxn && Parent != null)
            {
                //Parent.CustomerChanged(this);
                Parent.ItemChanged(this);
            }
        }
        #endregion

    }

    public class CollectionRuleAllColors : CollectionBase, IBindingList
    {
        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        private ListChangedEventHandler onListChanged;

        //Change to the name of the object that holds the data in the next 4 items.
        public ItemRuleAllColor this[int index]
        {
            get
            {
                return (ItemRuleAllColor)(List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        public int Add(ItemRuleAllColor value)
        {
            return List.Add(value);
        }

        public ItemRuleAllColor AddNew()
        {
            return (ItemRuleAllColor)((IBindingList)this).AddNew();
        }

        public void Remove(ItemRuleAllColor value)
        {
            List.Remove(value);
        }

        //=========================================================================



        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }


        //Change to the name of the object that holds the data in this item.
        protected override void OnClear()
        {
            foreach (ItemRuleAllColor c in List)
            {
                c.Parent = null;
            }
        }
        //=========================================================================


        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }



        //Change to the name of the object that holds the data here.
        //=========================================================================
        protected override void OnInsertComplete(int index, object value)
        {
            ItemRuleAllColor c = (ItemRuleAllColor)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ItemRuleAllColor c = (ItemRuleAllColor)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                ItemRuleAllColor olddata = (ItemRuleAllColor)oldValue;
                ItemRuleAllColor newdata = (ItemRuleAllColor)newValue;

                olddata.Parent = null;
                newdata.Parent = this;


                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        //=========================================================================


        //Change to the name of the object that holds the data here.
        //=========================================================================
        internal void ItemChanged(ItemRuleAllColor allColorSet)
        {

            int index = List.IndexOf(allColorSet);

            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }


        //=========================================================================

        // Implements IBindingList.
        bool IBindingList.AllowEdit
        {
            get { return true; }
        }

        bool IBindingList.AllowNew
        {
            get { return false; }
        }

        bool IBindingList.AllowRemove
        {
            get { return false; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return false; }
        }

        // Events.
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }


        // Methods.
        object IBindingList.AddNew()
        {
            //ItemAllSize c = new ItemAllSize(this.Count.ToString());
            ItemRuleAllColor c = new ItemRuleAllColor();
            List.Add(c);
            return c;
        }


        // Unsupported properties.
        bool IBindingList.IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }


        // Unsupported Methods.
        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

    }

    #endregion All Color Collection

    #region Color Collection
    public class ItemRuleColor : RuleItemBase, IEditableObject
    {

        #region Member Variables
        private CollectionRuleColors parent;
        private bool inTxn = false;
        //private CollectionSizes colColorSizes = new CollectionSizes();
        private CollectionRuleSizeDimensions colColorSizeDimensions = new CollectionRuleSizeDimensions();
        #endregion Member Variables

        #region Public Properties

        //				public CollectionSizes collectionSizes
        //				{
        //					get {return colColorSizes;}
        //				}
        //

        public CollectionRuleSizeDimensions collectionRuleSizeDimensions
        {
            get { return colColorSizeDimensions; }
        }

        #endregion Properties

        internal CollectionRuleColors Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }


        #region Constructors

        public ItemRuleColor()
            : base()
        {
        }


        public ItemRuleColor(int intMethodRid, int intColorRid, int intRule, int intQty,
            int intSizesRid, int intSizeCodeRid, int intDimensionsRid,
            int intSglRid, eSizeMethodRowType rowTypeID)
            : base(intMethodRid, intColorRid, intRule, intQty,
                intSizesRid, intSizeCodeRid, intDimensionsRid,
                intSglRid, rowTypeID)
        {

        }


        public ItemRuleColor(int intMethodRid, DataRow drColorItem)
            : base(intMethodRid, drColorItem)
        {
        }


        #endregion Constructors

        #region Implements IEditableObject
        void IEditableObject.BeginEdit()
        {
            if (!inTxn)
            {
                //this.backupData = colorData;
                inTxn = true;
            }
        }

        void IEditableObject.CancelEdit()
        {
            if (inTxn)
            {
                //this.colorData = backupData;
                inTxn = false;
            }
        }

        void IEditableObject.EndEdit()
        {
            if (inTxn)
            {
                //backupData = new ColorData();
                inTxn = false;
            }
        }

        #endregion

        #region Methods
        private void OnItemChanged()
        {
            if (!inTxn && Parent != null)
            {
                //Parent.CustomerChanged(this);
                Parent.ItemChanged(this);
            }
        }
        #endregion

    }

    public class CollectionRuleColors : CollectionBase, IBindingList
    {
        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        private ListChangedEventHandler onListChanged;

        //Change to the name of the object that holds the data in the next 4 items.
        public ItemRuleColor this[int index]
        {
            get
            {
                return (ItemRuleColor)(List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        public int Add(ItemRuleColor value)
        {
            return List.Add(value);
        }

        public ItemRuleColor AddNew()
        {
            return (ItemRuleColor)((IBindingList)this).AddNew();
        }

        public void Remove(ItemRuleColor value)
        {
            List.Remove(value);
        }

        //=========================================================================



        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }


        //Change to the name of the object that holds the data in this item.
        protected override void OnClear()
        {
            foreach (ItemRuleColor c in List)
            {
                c.Parent = null;
            }
        }
        //=========================================================================


        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }



        //Change to the name of the object that holds the data here.
        //=========================================================================
        protected override void OnInsertComplete(int index, object value)
        {
            ItemRuleColor c = (ItemRuleColor)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ItemRuleColor c = (ItemRuleColor)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                ItemRuleColor olddata = (ItemRuleColor)oldValue;
                ItemRuleColor newdata = (ItemRuleColor)newValue;

                olddata.Parent = null;
                newdata.Parent = this;


                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        //=========================================================================


        //Change to the name of the object that holds the data here.
        //=========================================================================
        internal void ItemChanged(ItemRuleColor colorSet)
        {

            int index = List.IndexOf(colorSet);

            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }


        //=========================================================================

        // Implements IBindingList.
        bool IBindingList.AllowEdit
        {
            get { return true; }
        }

        bool IBindingList.AllowNew
        {
            get { return true; }
        }

        bool IBindingList.AllowRemove
        {
            get { return true; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return false; }
        }


        // Events.
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }


        // Methods.
        object IBindingList.AddNew()
        {
            //IMPORTANT TO BE SURE THIS IS CORRECT OR AN INVALID CAST ERROR WILL
            //BE RAISED.
            ItemRuleColor c = new ItemRuleColor();
            List.Add(c);
            return c;
        }


        // Unsupported properties.
        bool IBindingList.IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }


        // Unsupported Methods.
        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

    }

    #endregion Color Collection

    #region Size Collection
    public class ItemRuleSize : RuleItemBase, IEditableObject
    {

        #region Member Variables
        private CollectionRuleSizes parent;
        private bool inTxn = false;
        //private CollectionSizeDimensions colSizeDimensions = new CollectionSizeDimensions();

        #endregion Member Variables

        #region Public Properties

        //					public CollectionSizeDimensions collectionSizeDimensions
        //					{
        //						get {return colSizeDimensions;}
        //					}
        //
        #endregion Properties

        internal CollectionRuleSizes Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }


        #region Constructors

        public ItemRuleSize()
            : base()
        {
        }


        public ItemRuleSize(int intMethodRid, int intColorRid, int intRule, int intQty,
            int intSizesRid, int intSizeCodeRid, int intDimensionsRid,
            int intSglRid, eSizeMethodRowType rowTypeID)
            : base(intMethodRid, intColorRid, intRule, intQty,
                intSizesRid, intSizeCodeRid, intDimensionsRid,
                intSglRid, rowTypeID)
        {

        }


        public ItemRuleSize(int intMethodRid, DataRow drSize)
            : base(intMethodRid, drSize)
        {
        }


        #endregion Constructors

        #region Implements IEditableObject

        void IEditableObject.BeginEdit()
        {
            if (!inTxn)
            {
                //this.backupData = sizeData;
                inTxn = true;
            }
        }

        void IEditableObject.CancelEdit()
        {
            if (inTxn)
            {
                //this.sizeData = backupData;
                inTxn = false;
            }
        }

        void IEditableObject.EndEdit()
        {
            if (inTxn)
            {
                //backupData = new SizeData();
                inTxn = false;
            }
        }

        #endregion


        #region Methods
        private void OnItemChanged()
        {
            if (!inTxn && Parent != null)
            {
                //Parent.CustomerChanged(this);
                Parent.ItemChanged(this);
            }
        }
        #endregion

    }

    public class CollectionRuleSizes : CollectionBase, IBindingList
    {
        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        private ListChangedEventHandler onListChanged;

        //Change to the name of the object that holds the data in the next 4 items.
        public ItemRuleSize this[int index]
        {
            get
            {
                return (ItemRuleSize)(List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        public int Add(ItemRuleSize value)
        {
            return List.Add(value);
        }

        public ItemRuleSize AddNew()
        {
            return (ItemRuleSize)((IBindingList)this).AddNew();
        }

        public void Remove(ItemRuleSize value)
        {
            List.Remove(value);
        }

        //=========================================================================



        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }


        //Change to the name of the object that holds the data in this item.
        protected override void OnClear()
        {
            foreach (ItemRuleSize c in List)
            {
                c.Parent = null;
            }
        }
        //=========================================================================


        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }



        //Change to the name of the object that holds the data here.
        //=========================================================================
        protected override void OnInsertComplete(int index, object value)
        {
            ItemRuleSize c = (ItemRuleSize)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ItemRuleSize c = (ItemRuleSize)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                ItemRuleSize olddata = (ItemRuleSize)oldValue;
                ItemRuleSize newdata = (ItemRuleSize)newValue;

                olddata.Parent = null;
                newdata.Parent = this;


                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        //=========================================================================


        //Change to the name of the object that holds the data here.
        //=========================================================================
        internal void ItemChanged(ItemRuleSize sizeSet)
        {

            int index = List.IndexOf(sizeSet);

            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }


        //=========================================================================

        // Implements IBindingList.
        bool IBindingList.AllowEdit
        {
            get { return true; }
        }

        bool IBindingList.AllowNew
        {
            get { return true; }
        }

        bool IBindingList.AllowRemove
        {
            get { return true; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return false; }
        }


        // Events.
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }


        // Methods.
        object IBindingList.AddNew()
        {
            //IMPORTANT TO BE SURE THIS IS CORRECT OR AN INVALID CAST ERROR WILL
            //BE RAISED.
            ItemRuleSize c = new ItemRuleSize();
            List.Add(c);
            return c;
        }


        // Unsupported properties.
        bool IBindingList.IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }


        // Unsupported Methods.
        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

    }

    #endregion Size Collection

    #region Size Dimension Collection
    public class ItemRuleSizeDimension : RuleItemBase, IEditableObject
    {

        #region Member Variables
        private CollectionRuleSizeDimensions parent;
        private CollectionRuleSizes colSizes = new CollectionRuleSizes();
        private bool inTxn = false;
        #endregion Member Variables

        #region Public Properties

        public CollectionRuleSizes collectionRuleSizes
        {
            get { return colSizes; }
        }

        #endregion Properties

        internal CollectionRuleSizeDimensions Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }


        #region Constructors

        public ItemRuleSizeDimension()
            : base()
        {
        }


        public ItemRuleSizeDimension(int intMethodRid, int intColorRid, int intRule, int intQty,
            int intSizesRid, int intSizeCodeRid, int intDimensionsRid,
            int intSglRid, eSizeMethodRowType rowTypeID)
            : base(intMethodRid, intColorRid, intRule, intQty,
                intSizesRid, intSizeCodeRid, intDimensionsRid,
                intSglRid, rowTypeID)
        {

        }


        public ItemRuleSizeDimension(int intMethodRid, DataRow drSizeDimension)
            : base(intMethodRid, drSizeDimension)
        {

        }


        #endregion Constructors

        #region Implements IEditableObject
        void IEditableObject.BeginEdit()
        {
            if (!inTxn)
            {
                //this.backupData = sizeDimensionData;
                inTxn = true;
            }
        }

        void IEditableObject.CancelEdit()
        {
            if (inTxn)
            {
                //this.sizeDimensionData = backupData;
                inTxn = false;
            }
        }

        void IEditableObject.EndEdit()
        {
            if (inTxn)
            {
                //backupData = new SizeDimensionData();
                inTxn = false;
            }
        }

        #endregion

        #region Methods
        private void OnItemChanged()
        {
            if (!inTxn && Parent != null)
            {
                //Parent.CustomerChanged(this);
                Parent.ItemChanged(this);
            }
        }
        #endregion

    }

    public class CollectionRuleSizeDimensions : CollectionBase, IBindingList
    {
        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        private ListChangedEventHandler onListChanged;

        //Change to the name of the object that holds the data in the next 4 items.
        public ItemRuleSizeDimension this[int index]
        {
            get
            {
                return (ItemRuleSizeDimension)(List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        public int Add(ItemRuleSizeDimension value)
        {
            return List.Add(value);
        }

        public ItemRuleSizeDimension AddNew()
        {
            return (ItemRuleSizeDimension)((IBindingList)this).AddNew();
        }

        public void Remove(ItemRuleSizeDimension value)
        {
            List.Remove(value);
        }

        //=========================================================================



        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }


        //Change to the name of the object that holds the data in this item.
        protected override void OnClear()
        {
            foreach (ItemRuleSizeDimension c in List)
            {
                c.Parent = null;
            }
        }
        //=========================================================================


        protected override void OnClearComplete()
        {
            OnListChanged(resetEvent);
        }



        //Change to the name of the object that holds the data here.
        //=========================================================================
        protected override void OnInsertComplete(int index, object value)
        {
            ItemRuleSizeDimension c = (ItemRuleSizeDimension)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ItemRuleSizeDimension c = (ItemRuleSizeDimension)value;
            c.Parent = this;
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            if (oldValue != newValue)
            {

                ItemRuleSizeDimension olddata = (ItemRuleSizeDimension)oldValue;
                ItemRuleSizeDimension newdata = (ItemRuleSizeDimension)newValue;

                olddata.Parent = null;
                newdata.Parent = this;


                OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        //=========================================================================


        //Change to the name of the object that holds the data here.
        //=========================================================================
        internal void ItemChanged(ItemRuleSizeDimension sizeDimensionSet)
        {

            int index = List.IndexOf(sizeDimensionSet);

            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }


        //=========================================================================

        // Implements IBindingList.
        bool IBindingList.AllowEdit
        {
            get { return true; }
        }

        bool IBindingList.AllowNew
        {
            get { return true; }
        }

        bool IBindingList.AllowRemove
        {
            get { return true; }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool IBindingList.SupportsSorting
        {
            get { return false; }
        }


        // Events.
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }


        // Methods.
        object IBindingList.AddNew()
        {
            //IMPORTANT TO BE SURE THIS IS CORRECT OR AN INVALID CAST ERROR WILL
            //BE RAISED.
            ItemRuleSizeDimension c = new ItemRuleSizeDimension();
            List.Add(c);
            return c;
        }

        // Unsupported properties.
        bool IBindingList.IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }


        // Unsupported Methods.
        void IBindingList.AddIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }

    }

    #endregion Size Dimension Collection


    public class RuleItemBase
    {
        #region Member Variables
        private int _method_rid;
        private int _rule;
        private int _qty;
        private int _color_code_rid;
        private int _sizes_rid;
        private int _size_code_rid;
        private int _dimensions_rid;
        private int _sgl_rid;
        private eSizeMethodRowType _row_type_id;

        #endregion



        public RuleItemBase()
        {
            _method_rid = Include.NoRID;
            _row_type_id = 0;
            _size_code_rid = Include.NoRID;
            _sizes_rid = Include.NoRID;
            _dimensions_rid = Include.NoRID;
            _rule = Include.Undefined;
            _qty = 0;
            _sgl_rid = Include.NoRID;
            _color_code_rid = Include.NoRID;
        }

        //public RuleItemBase(int sglRid, int colorRid, int sizeCodeRid) // MID Track 3492 Size Need Method with constraints not allocating correctly
        public RuleItemBase(int sglRid, int colorRid, SizeCodeProfile aSizeCodeProfile) // MID Track 3492 Size Need Method with constraints not allocating correctly
        {
            _method_rid = Include.NoRID;
            _row_type_id = 0;
            // begin MID Track 3492 Size Need Method with constraints not allocating correctly
            //_size_code_rid = sizeCodeRid;
            //_sizes_rid = Include.NoRID;
            //_dimensions_rid = Include.NoRID;
            _size_code_rid = aSizeCodeProfile.Key;
            _sizes_rid = aSizeCodeProfile.SizeCodePrimaryRID;
            _dimensions_rid = aSizeCodeProfile.SizeCodeSecondaryRID;
            // end MID Track 3492 Size Need Method with constraints not allocating correctly
            _rule = Include.Undefined;
            _qty = 0;
            _sgl_rid = sglRid;
            _color_code_rid = colorRid;
        }


        public RuleItemBase(int intMethodRid, int intColorRid, int intRule, int intQty,
            int intSizesRid, int intSizeCodeRid, int intDimensionsRid,
            int intSglRid, eSizeMethodRowType rowTypeID)
        {
            _method_rid = intMethodRid;
            _row_type_id = rowTypeID;
            _size_code_rid = intSizeCodeRid;
            _sizes_rid = intSizesRid;
            _dimensions_rid = intDimensionsRid;
            _rule = intRule;
            _qty = intQty;
            _sgl_rid = intSglRid;
            _color_code_rid = intColorRid;
        }


        public RuleItemBase(int intMethodRid, DataRow drSize)
        {
            _method_rid = intMethodRid;

            //SET VALUES FROM THE DATAROW
            //===========================
            _row_type_id = (eSizeMethodRowType)drSize["ROW_TYPE_ID"];
            _rule = (drSize["SIZE_RULE"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SIZE_RULE"], CultureInfo.CurrentUICulture) : Include.UndefinedRule;
            _qty = (drSize["SIZE_QUANTITY"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SIZE_QUANTITY"], CultureInfo.CurrentUICulture) : Include.UndefinedQuantity;
            _color_code_rid = (drSize["COLOR_CODE_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["COLOR_CODE_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;
            _sizes_rid = (drSize["SIZES_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SIZES_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;
            _dimensions_rid = (drSize["DIMENSIONS_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["DIMENSIONS_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;
            _size_code_rid = (drSize["SIZE_CODE_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SIZE_CODE_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;
            _sgl_rid = (drSize["SGL_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SGL_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;
        }






        public int MethodRid
        {
            get { return _method_rid; }
            set { _method_rid = value; }
        }

        public int Rule
        {
            get { return _rule; }
            set { _rule = value; }
        }

        public int Qty
        {
            get { return _qty; }
            set { _qty = value; }
        }

        public int SizeCodeRid
        {
            get { return _size_code_rid; }
            set { _size_code_rid = value; }
        }

        public int SizesRid
        {
            get { return _sizes_rid; }
            set { _sizes_rid = value; }
        }

        public int ColorCodeRid
        {
            get { return _color_code_rid; }
            set { _color_code_rid = value; }
        }

        public int DimensionsRid
        {
            get { return _dimensions_rid; }
            set { _dimensions_rid = value; }
        }

        public int SglRid
        {
            get { return _sgl_rid; }
            set { _sgl_rid = value; }
        }


        public eSizeMethodRowType RowTypeID
        {
            get { return _row_type_id; }
            set { _row_type_id = value; }
        }
    #endregion
    }

    // begin TT#1432 - Size Dimension Constraints not working
}
    // moved the Collection Decoder to Business.Allocation

    //#region Collection Decoder

    //public class CollectionDecoder
    //{
    //    //=============
    //    // Variables
    //    //=============
    //    private CollectionSets _collectionSets;
    //    //private CollectionFringeSets _collectionFringeSets;
    //    private CollectionRuleSets _collectionRuleSets;
    //    private eSizeCollectionType _collectionType;
    //    private bool _continue;
    //    private Hashtable _minMaxHash; // MID Track 3844 Constraints not working
    //    private Hashtable _ruleHash;   // MID Track 3844 Constraints not working
    //    private Hashtable _sglHash;   // MID Track 4244 Constraints not working
    //    private Dictionary<int, SizeCodeProfile> _sizeCodeProfileDict; // TT#1391 - TMW New Action

    //    //=============
    //    // CONstructors
    //    //=============

    //    public CollectionDecoder(CollectionSets aCollection, Hashtable sglHash) //Issue 4244
    //    {
    //        _collectionSets = aCollection;
    //        _sglHash = sglHash;			// Issue 4244
    //        _collectionType = eSizeCollectionType.MinMaxCollection;
    //        _minMaxHash = new Hashtable(); // MID Track 3844 Constraints not working
    //        _sizeCodeProfileDict = new Dictionary<int, SizeCodeProfile>(); // TT#1391 - TMW New Action
    //    }

    //    // These rules are used during fringe processing
    //    public CollectionDecoder(CollectionRuleSets aCollection, Hashtable sglHash)  // Issue 4244
    //    {
    //        _sglHash = sglHash;			// Issue 4244
    //        _collectionRuleSets = aCollection;
    //        _collectionType = eSizeCollectionType.RulesCollection;
    //        _ruleHash = new Hashtable(); // MID Track 3844 Constraints not working
    //        _sizeCodeProfileDict = new Dictionary<int, SizeCodeProfile>(); // TT#1391 - TMW New Action
    //    }


    //    //public object GetItemForStore(int stRid, int colorRid, SizeCodeProfile sizeCodeProfile)  // TT#1391 - TMW New Action
    //    public object GetItemForStore(int stRid, int colorRid, int sizeCodeRID)                    // TT#1391 - TMW New Action
    //    {
    //        try
    //        {
    //            int sglRid = Include.NoRID;
    //            if (_sglHash.ContainsKey(stRid))
    //                sglRid = (int)_sglHash[stRid];

    //            //object returnObj = GetItem(sglRid, colorRid, sizeCodeProfile); // TT#1391 - TMW New Action
    //            object returnObj = GetItem(sglRid, colorRid, sizeCodeRID);       // TT31391 - TMW New Action
				
    //            return returnObj;
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //    }

    //    // public object GetItem(int sglRid, int colorRid, int sizeCodeRid) // MID Track 3492 Size Need Method with Contraints not allocating correctly
    //    //public object GetItem(int sglRid, int colorRid, SizeCodeProfile sizeCodeProfile) // MID Track 3492 Size Need Method with Constraints not allocating correctly  // TT#1391 - TMW New Action
    //    public object GetItem(int sglRid, int colorRid, int sizeCodeRid)    // TT#1391 - TMW New Action
    //    {
    //        //int sizeCodeRid = sizeCodeProfile.Key;        // TT#1391 - TMW New Action
    //        object returnObj = null;
    //        MinMaxItemBase minMax = null;
    //        RuleItemBase rule = null;
    //        _continue = true;
    //        if (_collectionType == eSizeCollectionType.MinMaxCollection)
    //        {
    //            //minMax = GetItemMinMax(sglRid, colorRid, sizeCodeProfile); // MID Track 3492 Size Need Method with constraint not allocating correctly // TT#1391 - TMW New Action
    //            minMax = GetItemMinMax(sglRid, colorRid, sizeCodeRid);       // TT#1391 - TMW New Action
    //            returnObj = minMax;
    //        }
    //        else if (_collectionType == eSizeCollectionType.RulesCollection)
    //        {
    //            //rule = GetItemRule(sglRid, colorRid, sizeCodeProfile); // MID Track 3492 Size Need Method with Constraints not allocating correctly  // TT#1391 - TMW New Action
    //            rule = GetItemRule(sglRid, colorRid, sizeCodeRid);       // TT#1391 - TMW New Action
    //            returnObj = rule;
    //        }

    //        return returnObj;
    //    }

    //    public void DebugSetsCollection()
    //    {
    //        //PROCESS SETS AND ALL DESCENDANTS
    //        Debug.WriteLine("---------------------------------------------");
    //        if (_collectionType == eSizeCollectionType.RulesCollection)
    //        {
    //            foreach (ItemRuleSet oItemSet in _collectionRuleSets)
    //            {
    //                Debug.WriteLine("RID " + oItemSet.MethodRid.ToString() +
    //                    " SGL " + oItemSet.SglRid.ToString() + " RULE " + oItemSet.Rule.ToString() +
    //                    " QTY " + oItemSet.Qty.ToString());
									
    //                //PROCESS ALL COLOR LEVEL AND ALL DESCENDANTS
    //                foreach (ItemRuleAllColor oItemAllColor in oItemSet.collectionRuleAllColors)
    //                {
    //                    DebugItem(1, oItemAllColor);

    //                    foreach (ItemRuleSizeDimension oItemSizeDimension in oItemAllColor.collectionRuleSizeDimensions)
    //                    {
    //                        DebugItem(2, oItemSizeDimension);

    //                        foreach (ItemRuleSize oItemSize in oItemSizeDimension.collectionRuleSizes)
    //                        {
    //                            DebugItem(3, oItemSize);
    //                        }
    //                    }
    //                }

    //                //PROCESS COLOR LEVEL AND ALL DESCENDANTS
    //                foreach (ItemRuleColor oItemColor in oItemSet.collectionRuleColors)
    //                {
    //                    DebugItem(1, oItemColor);

    //                    foreach (ItemRuleSizeDimension oItemSizeDimension in oItemColor.collectionRuleSizeDimensions)
    //                    {
    //                        DebugItem(2, oItemSizeDimension);

    //                        foreach (ItemRuleSize oItemSize in oItemSizeDimension.collectionRuleSizes)
    //                        {
    //                            DebugItem(3, oItemSize);
    //                        }

    //                    }
    //                }
    //            }

    //        }
    //    }

    //    private void DebugItem(int level, RuleItemBase pItem)
    //    {
    //        string sLevel = string.Empty;
    //        switch (level)
    //        {
    //            case 1:
    //                sLevel = "  ";
    //                break;
    //            case 2: 
    //                sLevel = "    ";
    //                break;
    //            case 3:
    //                sLevel = "      ";
    //                break;
    //            default:
    //                sLevel = "--";
    //                break;
    //        }
    //        Debug.WriteLine(sLevel + "SGL " + pItem.SglRid.ToString() +
    //            " COLOR " + pItem.ColorCodeRid.ToString() +
    //            " DIM " + pItem.DimensionsRid.ToString() +
    //            " SIZE/SIZES " + pItem.SizeCodeRid.ToString() + "/" + pItem.SizesRid.ToString() +
    //            " Rule " + pItem.Rule.ToString() +
    //            " QTY " + pItem.Qty.ToString());
    //    }
    //    #region SizeCodeProfile
    //    private SizeCodeProfile GetSizeCodeProfile(int aSizeCodeRID)
    //    {
    //        SizeCodeProfile sizeCodeProfile;
    //        if (!_sizeCodeProfileDict.TryGetValue(aSizeCodeRID, out sizeCodeProfile))
    //        {
    //            sizeCodeProfile = new SizeCodeProfile(aSizeCodeRID);
    //            _sizeCodeProfileDict.Add(aSizeCodeRID, sizeCodeProfile);
    //        }
    //        return sizeCodeProfile;
    //    }
    //    #endregion SizeCodeProfile
    //    #region MinMax (Constraints)
    //    // J.Ellis unnecessary comments removed
    //    //private MinMaxItemBase GetItemMinMax(int sglRid, int colorRid, SizeCodeProfile aSizeCodeProfile) // MID Track 3492 Size Need with constraints not allocating correctly  // TT#1391 - TMW New Action
    //    private MinMaxItemBase GetItemMinMax(int sglRid, int colorRid, int sizeCodeRid)                    // TT#1391 - TMW New Action
    //    {
    //        // begin MID Track 3844 Constraints not working
    //        MinMaxItemBase minMax;
    //        long minMaxSGL_ColorKey = ((long) sglRid << 32) + colorRid;
    //        Hashtable sglColorHash;
    //        if (_minMaxHash.Contains(minMaxSGL_ColorKey))
    //        {
    //            sglColorHash = (Hashtable)_minMaxHash[minMaxSGL_ColorKey];
    //        }
    //        else
    //        {
    //            sglColorHash = new Hashtable();
    //            _minMaxHash.Add(minMaxSGL_ColorKey, sglColorHash);
    //        }
    //        // begin TT#1391 - TMW new Action
    //        //if (sglColorHash.Contains(aSizeCodeProfile.Key))
    //        //{
    //        //    minMax = (MinMaxItemBase)sglColorHash[aSizeCodeProfile.Key];
    //        //}
    //        //else
    //        minMax = (MinMaxItemBase)sglColorHash[sizeCodeRid];
    //        if (minMax == null)
    //            // end TT#1391 - TMW New Action
    //        {
    //            // end MID Track 3844 Constratints not working
    //            try
    //            {
    //                // begin TT#1391 - TMW New Action
    //                SizeCodeProfile aSizeCodeProfile = GetSizeCodeProfile(sizeCodeRid);
    //                // end TT#1391 - TMW New Action
    //                minMax = new MinMaxItemBase(sglRid, colorRid, aSizeCodeProfile); // MID Track 3844 Constraints not working
    //                _continue = true; // MID Track 3844 Constraints not working
    //                while (_continue)
    //                {
    //                    minMax = SearchMinMaxCollection(minMax);
    //                    // begin MID Track 3844 Constraints not working
    //                    //if ((minMax.Min != 0 && minMax.Max != int.MaxValue && minMax.Mult != Include.Undefined)
    //                    //	 || minMax.DimensionsRid == Include.MaskedRID && minMax.SizeCodeRid == Include.MaskedRID && minMax.ColorCodeRid == Include.MaskedRID && minMax.SglRid == Include.MaskedRID) // MID Track 3844 Constraints not working
    //                    //	//|| minMax.DimensionsRid ==Include.NoRID && minMax.SizeCodeRid == Include.NoRID && minMax.ColorCodeRid == Include.NoRID && minMax.SglRid == Include.NoRID) // MID Track 3844 Constraints not working // MID Track 3492 Size Need with constraints not allocating correctly
    //                    //{
    //                    //	_continue = false;
    //                    //}
    //                    if (minMax.Min != 0 && minMax.Max != int.MaxValue && minMax.Mult != Include.Undefined)
    //                    {
    //                        _continue = false;
    //                    }
    //                        // end MID Track 3844 Constraints not working
    //                    else
    //                    {
    //                        // begin MID Track 3492 Size Need with constraints not allocating correctly
    //                        if (minMax.ColorCodeRid != Include.MaskedRID) // MID Track 3844 Constraints not working
    //                        {
    //                            if (minMax.DimensionsRid != Include.MaskedRID) // MID Track 3844 Constraints not working
    //                            {
    //                                if (minMax.SizeCodeRid != Include.MaskedRID) // MID Track 3844 Constraints not working
    //                                {
    //                                    minMax.SizeCodeRid = Include.MaskedRID; // MID Track 3844 Constraints not working
    //                                }
    //                                else
    //                                {
    //                                    minMax.DimensionsRid = Include.MaskedRID; // MID Track 3844 Constraints not working
    //                                    //minMax.SizeCodeRid = aSizeCodeProfile.Key; // MID Track 3844 Constraint not working
    //                                }
    //                            }
    //                            else
    //                            {
    //                                minMax.DimensionsRid = aSizeCodeProfile.SizeCodeSecondaryRID;
    //                                minMax.SizeCodeRid = aSizeCodeProfile.Key;
    //                                minMax.ColorCodeRid = Include.MaskedRID; // MID Track 3844 Constraints not working
    //                            }
    //                        }
    //                        else
    //                        {
    //                            if (minMax.DimensionsRid != Include.MaskedRID) // MID Track 3844 Constraints not working  
    //                            {
    //                                if (minMax.SizeCodeRid != Include.MaskedRID) // MID Track 3844 Constraints not working
    //                                {
    //                                    minMax.SizeCodeRid = Include.MaskedRID; // MID Track 3844 Constraints not working
    //                                }
    //                                else
    //                                {
    //                                    minMax.DimensionsRid = Include.MaskedRID; // MID Track 3844 Constraints not working
    //                                    //minMax.SizeCodeRid = aSizeCodeProfile.Key; // MID Track 3844 Contraints not working
    //                                }
    //                            }
    //                            else
    //                            {
    //                                // begin MID Track 3844 Constraints not working
    //                                if (minMax.SglRid == Include.NoRID)
    //                                {
    //                                    _continue = false;
    //                                }
    //                                else
    //                                {
    //                                    minMax.SglRid = Include.NoRID; // MID Track 3844 Constraints not working
    //                                    minMax.ColorCodeRid = colorRid;
    //                                    minMax.DimensionsRid = aSizeCodeProfile.SizeCodeSecondaryRID; 
    //                                    minMax.SizeCodeRid = aSizeCodeProfile.Key; // MID Track 3492 Size Need Method with constraint not allocating correctly
    //                                }
    //                                // end MID Track 3844 Constraints not working
    //                            }
    //                        }
    //                    }
    //                }
    //                sglColorHash.Add(aSizeCodeProfile.Key, minMax); // MID Track 3844 Constraints not working
    //                //return minMax;

    //            }
    //            catch (Exception)
    //            {
    //                throw;
    //            }
    //        }
    //        return minMax; // MID Track 3844 Constraints not working
    //    }

    //    public MinMaxItemBase SearchMinMaxCollection(MinMaxItemBase minMax)
    //    {
    //        foreach (ItemSet oItemSet in _collectionSets)
    //        {
    //            if (oItemSet.SglRid == minMax.SglRid) 
    //            {
    //                if (minMax.ColorCodeRid == Include.MaskedRID) // MID Track 3844 Constraints not working
    //                {
    //                    //PROCESS ALL COLOR LEVEL AND ALL DESCENDANTS
    //                    foreach (ItemAllColor oItemAllColor in oItemSet.collectionAllColors)
    //                    {
    //                        foreach (ItemSizeDimension oItemSizeDimension in oItemAllColor.collectionSizeDimensions)
    //                        {
    //                            if (oItemSizeDimension.DimensionsRid == minMax.DimensionsRid) // MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                            {                                                             // MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                                foreach (ItemSize oItemSize in oItemSizeDimension.collectionSizes)
    //                                {
    //                                    if (oItemSize.SizeCodeRid == minMax.SizeCodeRid) // MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                                    {                                                // MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                                        if (minMax.Min == 0)
    //                                            minMax.Min = oItemSize.Min;
    //                                        if (minMax.Max == int.MaxValue)
    //                                            minMax.Max = oItemSize.Max;
    //                                        if (minMax.Mult == Include.Undefined)
    //                                            minMax.Mult = oItemSize.Mult;
    //                                        break;                                       // MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                                    }                                                // MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                                }
    //                                // begin MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                                if (minMax.Min == 0)
    //                                    minMax.Min = oItemSizeDimension.Min;
    //                                if (minMax.Max == int.MaxValue)
    //                                    minMax.Max = oItemSizeDimension.Max;
    //                                if (minMax.Mult == Include.Undefined)
    //                                    minMax.Mult = oItemSizeDimension.Mult;
    //                                break;
    //                                // end  MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                            }   // MID Track 3492 Size Need With Constraints Not allocating Correctly
    //                        }

    //                        if (minMax.Min == 0)
    //                            minMax.Min = oItemAllColor.Min;
    //                        if (minMax.Max == int.MaxValue)
    //                            minMax.Max = oItemAllColor.Max;
    //                        if (minMax.Mult == Include.Undefined)
    //                            minMax.Mult = oItemAllColor.Mult;
    //                        break;
    //                    }
    //                }
    //                else
    //                {
    //                    //PROCESS COLOR LEVEL AND ALL DESCENDANTS
    //                    foreach (ItemColor oItemColor in oItemSet.collectionColors)
    //                    {
    //                        if (oItemColor.ColorCodeRid == minMax.ColorCodeRid)
    //                        {
    //                            foreach (ItemSizeDimension oItemSizeDimension in oItemColor.collectionSizeDimensions)
    //                            {
    //                                if (oItemSizeDimension.DimensionsRid == minMax.DimensionsRid) // MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                                {                                                            // MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                                    foreach (ItemSize oItemSize in oItemSizeDimension.collectionSizes)
    //                                    {
    //                                        if (oItemSize.SizeCodeRid == minMax.SizeCodeRid)
    //                                        {
    //                                            minMax.Min = oItemSize.Min;
    //                                            minMax.Max = oItemSize.Max;
    //                                            minMax.Mult = oItemSize.Mult;
    //                                            break;
    //                                        }
    //                                    }
    //                                    // begin MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                                    if (minMax.Min == 0)
    //                                        minMax.Min = oItemSizeDimension.Min;
    //                                    if (minMax.Max == int.MaxValue)
    //                                        minMax.Max = oItemSizeDimension.Max;
    //                                    if (minMax.Mult == Include.Undefined)
    //                                        minMax.Mult = oItemSizeDimension.Mult;
    //                                    break;
    //                                    // end  MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                                }   // MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                            }
								
    //                            if (minMax.Min == 0)
    //                                minMax.Min = oItemColor.Min;
    //                            if (minMax.Max == int.MaxValue)
    //                                minMax.Max = oItemColor.Max;
    //                            if (minMax.Mult == Include.Undefined)
    //                                minMax.Mult = oItemColor.Mult;
    //                            break; // MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //                        }
    //                    }
    //                }

    //                if (minMax.Min == 0)
    //                    minMax.Min = oItemSet.Min;
    //                if (minMax.Max == int.MaxValue)
    //                    minMax.Max = oItemSet.Max;
    //                if (minMax.Mult == Include.Undefined)
    //                    minMax.Mult = oItemSet.Mult;
    //                break; // MID Track 3492 Size Need With Constraints Not Allocating Correctly
    //            }
    //        }

    //        return minMax;
    //    }
    //    #endregion

    //    #region Size Rule
    //    // J. Ellis Removed unnecessary comments for readablity // MID Track 3844 Contraints not working
    //    //private RuleItemBase GetItemRule(int sglRid, int colorRid, SizeCodeProfile aSizeCodeProfile) // MID Track 3492 Size Need Method with constraints not allocating correctly // TT#1391 - TMW New Action
    //    private RuleItemBase GetItemRule(int sglRid, int colorRid, int sizeCodeRid)                    // TT#1391 - TMW New Action
    //    {
    //        // begin MID Track 3844 Constraints not working
    //        RuleItemBase rule;
    //        long ruleSGL_ColorKey = ((long) sglRid << 32) + colorRid;
    //        Hashtable sglColorHash;
    //        if (_ruleHash.Contains(ruleSGL_ColorKey))
    //        {
    //            sglColorHash = (Hashtable)_ruleHash[ruleSGL_ColorKey];
    //        }
    //        else
    //        {
    //            sglColorHash = new Hashtable();
    //            _ruleHash.Add(ruleSGL_ColorKey, sglColorHash);
    //        }
    //        // begin TT#1391 - TMW New Action
    //        //if (sglColorHash.Contains(aSizeCodeProfile.Key))
    //        //{
    //        //    rule = (RuleItemBase)sglColorHash[aSizeCodeProfile.Key];
    //        //}
    //        //else
    //        rule = (RuleItemBase)sglColorHash[sizeCodeRid];
    //        if (rule == null)
    //            // end TT#1391 - TMW New Action
    //        {
    //            // end MID Track 3844 Constraints not working
    //            try
    //            {
    //                // begin TT#1391 - TMW New Action
    //                SizeCodeProfile aSizeCodeProfile = GetSizeCodeProfile(sizeCodeRid);
    //                // end TT#1391 - TMW New Action
    //                rule = new RuleItemBase(sglRid, colorRid, aSizeCodeProfile); // MID Track 3844 Constraints not working // MID Track 3492 Size Need Method with Constraints not allocating correctly
    //                _continue = true; // MID Track 3844 Constraints not working
    //                while (_continue)
    //                {
    //                    rule = SearchRuleCollection(rule);
    //                    // begin MID Track 3844 Constraints not working
    //                    //if ((rule.Rule != Include.Undefined)
    //                    //	|| rule.DimensionsRid == Include.MaskedRID && rule.SizeCodeRid == Include.MaskedRID && rule.ColorCodeRid == Include.MaskedRID && rule.SglRid == Include.NoRID) // MID Track 3844 Constraints not working
    //                    //{
    //                    //	_continue = false;
    //                    //}
    //                    if (rule.Rule != Include.Undefined)
    //                    {
    //                        _continue = false;
    //                    }
    //                    else
    //                    {
    //                        // end MID Track 3844 Constraints not working
    //                        // begin MID Track 3492 Size Need with constraints not allocating correctly
    //                        if (rule.ColorCodeRid != Include.MaskedRID) // MID Track 3844 Constraints not working
    //                        {
    //                            if (rule.DimensionsRid != Include.MaskedRID) // MID Track 3844 Constraints not working
    //                            {
    //                                if (rule.SizeCodeRid != Include.MaskedRID) // MID Track 3844 Constraints not working
    //                                {
    //                                    rule.SizeCodeRid = Include.MaskedRID; // MID Track 3844 Constraints not working
    //                                }
    //                                else
    //                                {
    //                                    rule.DimensionsRid = Include.MaskedRID; // MID Track 3844 Constraints not working
    //                                    //	rule.SizeCodeRid = aSizeCodeProfile.Key; // MID Track 3844 Constraints not working
    //                                }
    //                            }
    //                            else
    //                            {
    //                                rule.DimensionsRid = aSizeCodeProfile.SizeCodeSecondaryRID;
    //                                rule.SizeCodeRid = aSizeCodeProfile.Key;
    //                                rule.ColorCodeRid = Include.MaskedRID; // MID Track 3844 Constraints not working
    //                            }
    //                        }
    //                        else
    //                        {
    //                            if (rule.DimensionsRid != Include.MaskedRID) // MID Track 3844 Constraints not working  
    //                            {
    //                                if (rule.SizeCodeRid != Include.MaskedRID) // MID Track 3844 Constraints not working
    //                                {
    //                                    rule.SizeCodeRid = Include.MaskedRID; // MID Track 3844 Constraints not working
    //                                }
    //                                else
    //                                {
    //                                    rule.DimensionsRid = Include.MaskedRID; // MID Track 3844 Constraints not working
    //                                    // rule.SizeCodeRid = aSizeCodeProfile.Key; // MID Track 3844 Constraints not working
    //                                }
    //                            }
    //                            else
    //                            {
    //                                // begin MID Track Constraints not working
    //                                if (rule.SglRid == Include.NoRID) // MID Track 3844 Constraints not working
    //                                {
    //                                    _continue = false;
    //                                }
    //                                else
    //                                {
    //                                    rule.SglRid = Include.NoRID; // MID Track 3844 Constraints not working
    //                                    rule.ColorCodeRid = colorRid;
    //                                    rule.DimensionsRid = aSizeCodeProfile.SizeCodeSecondaryRID; 
    //                                    rule.SizeCodeRid = aSizeCodeProfile.Key; // MID Track 3492 Size Need Method with constraint not allocating correctly
    //                                }
    //                                // end MID Track Constraints not working
    //                            }
    //                        }
    //                    } // MID Track 3844 Constraints not working
    //                } 
    //                sglColorHash.Add(aSizeCodeProfile.Key, rule); // MID track 3844 Constraints not working
    //                //return rule; // MID Track 3844 Constraints not working

    //            }
    //            catch (Exception)
    //            {
    //                throw;
    //            }
    //        } // MID Track 3844 Constraints not working
    //        return rule; // MID Track 3844 Constraints not working
    //    }

    //    public RuleItemBase SearchRuleCollection(RuleItemBase aRuleItem)
    //    {
    //        foreach (ItemRuleSet oItemRuleSet in _collectionRuleSets)
    //        {
    //            if (oItemRuleSet.SglRid == aRuleItem.SglRid) 
    //            {
    //                if (aRuleItem.ColorCodeRid == Include.MaskedRID) // MID Track 3844 Constraints not working
    //                {
    //                    //PROCESS ALL COLOR LEVEL AND ALL DESCENDANTS
    //                    foreach (ItemRuleAllColor oItemRuleAllColor in oItemRuleSet.collectionRuleAllColors)
    //                    {
    //                        foreach (ItemRuleSizeDimension oItemRuleSizeDimension in oItemRuleAllColor.collectionRuleSizeDimensions)
    //                        {
    //                            if (oItemRuleSizeDimension.DimensionsRid == aRuleItem.DimensionsRid)
    //                            {
    //                                foreach (ItemRuleSize oItemRuleSize in oItemRuleSizeDimension.collectionRuleSizes) // MID Track 3782 Rules Decoder not working
    //                                {                                                                                  // MID Track 3782 Rules Decoder not working
    //                                    if (oItemRuleSize.SizeCodeRid == aRuleItem.SizeCodeRid)
    //                                    {
    //                                        aRuleItem.Rule = oItemRuleSize.Rule;
    //                                        aRuleItem.Qty = oItemRuleSize.Qty;
    //                                        break;
    //                                    }
    //                                }
    //                                if (aRuleItem.Rule == Include.Undefined)                                           // MID Track 3782 Rules Decoder not working
    //                                {                                                                                  // MID Track 3782 Rules Decoder not working
    //                                    aRuleItem.Rule = oItemRuleSizeDimension.Rule;                                  // MID Track 3782 Rules Decoder not working
    //                                    aRuleItem.Qty = oItemRuleSizeDimension.Qty;                                    // MID Track 3782 Rules Decoder not working
    //                                }                                                                                  // MID Track 3782 Rules Decoder not working
    //                                break;                                                                         // MID Track 3782 Rules Decoder not working
    //                            }                                                                                      // MID Track 3782 Rules Decoder not working
    //                        }

    //                        if (aRuleItem.Rule == Include.Undefined)
    //                        {
    //                            aRuleItem.Rule = oItemRuleAllColor.Rule;
    //                            aRuleItem.Qty = oItemRuleAllColor.Qty;
    //                        }
    //                        break;
    //                    }
    //                }
    //                else
    //                {
    //                    //PROCESS COLOR LEVEL AND ALL DESCENDANTS
    //                    foreach (ItemRuleColor oItemRuleColor in oItemRuleSet.collectionRuleColors)
    //                    {
    //                        if (oItemRuleColor.ColorCodeRid == aRuleItem.ColorCodeRid)
    //                        {
    //                            foreach (ItemRuleSizeDimension oItemRuleSizeDimension in oItemRuleColor.collectionRuleSizeDimensions)
    //                            {
    //                                if (oItemRuleSizeDimension.DimensionsRid == aRuleItem.DimensionsRid) // MID Track 3782 Rules Decoder not working
    //                                {                                                                    // MID Track 3782 Rules Decoder not working
    //                                    foreach (ItemRuleSize oItemRuleSize in oItemRuleSizeDimension.collectionRuleSizes)
    //                                    {
    //                                        if (oItemRuleSize.SizeCodeRid == aRuleItem.SizeCodeRid)
    //                                        {
    //                                            aRuleItem.Rule = oItemRuleSize.Rule;
    //                                            aRuleItem.Qty = oItemRuleSize.Qty;
    //                                            break;
    //                                        }
    //                                    }
    //                                    if (aRuleItem.Rule == Include.Undefined)                                           // MID Track 3782 Rules Decoder not working
    //                                    {                                                                                  // MID Track 3782 Rules Decoder not working
    //                                        aRuleItem.Rule = oItemRuleSizeDimension.Rule;                                  // MID Track 3782 Rules Decoder not working
    //                                        aRuleItem.Qty = oItemRuleSizeDimension.Qty;                                    // MID Track 3782 Rules Decoder not working
    //                                    }                                                                                  // MID Track 3782 Rules Decoder not working
    //                                    break;                                                                         // MID Track 3782 Rules Decoder not working
    //                                }                                                                                      // MID Track 3782 Rules Decoder not working
    //                            }
								
    //                            if (aRuleItem.Rule == Include.Undefined)
    //                            {
    //                                aRuleItem.Rule = oItemRuleColor.Rule;
    //                                aRuleItem.Qty = oItemRuleColor.Qty;
    //                            }
    //                            break;
    //                        }
    //                    }
    //                }
    //                if (aRuleItem.Rule == Include.Undefined)
    //                {
    //                    aRuleItem.Rule = oItemRuleSet.Rule;
    //                    aRuleItem.Qty = oItemRuleSet.Qty;
    //                }
    //                break;
    //            }
    //        }

    //        return aRuleItem;
    //    }
    //    #endregion
    //}


    //#endregion 

//}
// end TT#1432 Size Dimension Constraints not working