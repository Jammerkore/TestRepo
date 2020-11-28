using System;
using System.Collections;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Diagnostics;


namespace MIDRetail.Business.Allocation
{

	#region All Size Methods Constraints Collections
		#region Set Collection AKA...Store Group Level
			public class ItemOOSLookupSet:IEditableObject
			{
				#region Member Variables
				private CollectionOOSLookupSets parent;
				private int _hnRid;
				private int _sgl_rid;
				private int _oosQty;
				private eSizeMethodRowType _row_type_id;
				private bool inTxn = false;
				private CollectionOOSLookupAllColors colAllColors = new CollectionOOSLookupAllColors();
				private CollectionOOSLookupColors colColors = new CollectionOOSLookupColors();
				#endregion Member Variables

				#region Public Properties

				public eSizeMethodRowType RowTypeID
				{
					get {return _row_type_id;}
					set {_row_type_id = value;}
				}

				public CollectionOOSLookupAllColors collectionAllColors
				{
					get 
					{
						return colAllColors;
					}
				}

				public CollectionOOSLookupColors collectionColors
				{
					get 
					{
						return colColors;
					}
				}

				public int NodeRid
				{
					get { return _hnRid; }
					set { _hnRid = value; }
				}	


				public int SglRid
				{
					get {return _sgl_rid;}
					set {_sgl_rid = value;}
				}

				public int OOSQuantity
				{
					get {return _oosQty;}
					set { _oosQty = value; }
				}

				#endregion Properties

				internal CollectionOOSLookupSets Parent 
				{
					get 
					{
						return parent;
					}
					set 
					{
						parent = value ;
					}
				}

				#region Constructors

				public ItemOOSLookupSet()// : base()
				{
				}

				public ItemOOSLookupSet(int intHnRid, int intSglRid, int intQty, eSizeMethodRowType rowTypeID) : base()
				{

					_hnRid = intHnRid;
					_sgl_rid = intSglRid;
					_oosQty = intQty;
					_row_type_id = rowTypeID;

				}

				public ItemOOSLookupSet(int intHnRid, DataRow drSetItem)// :base()
				{

					_hnRid = intHnRid;

					//===========================
					//SET VALUES FROM THE DATAROW
					//===========================
					_sgl_rid = Convert.ToInt32(drSetItem["SGL_RID"],CultureInfo.CurrentUICulture);
					_row_type_id = (eSizeMethodRowType)drSetItem["ROW_TYPE_ID"];
					_oosQty = (drSetItem["OOS_QUANTITY"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSetItem["OOS_QUANTITY"], CultureInfo.CurrentUICulture) : Include.UndefinedQuantity;
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
			[Serializable]	
			public class CollectionOOSLookupSets : CollectionBase, IBindingList
			{
				private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
				private ListChangedEventHandler onListChanged;
				private int _sgRid;   
				private Hashtable _storeGroupLevelHash;

				public ItemOOSLookupSet this[int index] 
				{
					get 
					{
						return (ItemOOSLookupSet)(List[index]);
					}
					set 
					{
						List[index] = value;
					}
				}

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

				public int Add(ItemOOSLookupSet objItemSet) 
				{
					return List.Add(objItemSet);
				}

				public ItemOOSLookupSet AddNew() 
				{
					return (ItemOOSLookupSet)((IBindingList)this).AddNew();
				}

				public void Remove(ItemOOSLookupSet objItemSet) 
				{
					List.Remove(objItemSet);
				}

				protected virtual void OnListChanged(ListChangedEventArgs ev) 
				{
					if (onListChanged != null) 
					{
						onListChanged(this, ev);
					}
				}
					    
				protected override void OnClear() 
				{
					foreach (ItemOOSLookupSet c in List) 
					{
						c.Parent = null;
					}
				}
				//=========================================================================


				protected override void OnClearComplete() 
				{
					OnListChanged(resetEvent);
				}

				protected override void OnInsertComplete(int index, object value) 
				{
					ItemOOSLookupSet c = (ItemOOSLookupSet)value;
					c.Parent = this;
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
				}

				protected override void OnRemoveComplete(int index, object value) 
				{
					ItemOOSLookupSet c = (ItemOOSLookupSet)value;
					c.Parent = this;
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
				}

				protected override void OnSetComplete(int index, object oldValue, object newValue) 
				{
					if (oldValue != newValue) 
					{

						ItemOOSLookupSet oldcust = (ItemOOSLookupSet)oldValue;
						ItemOOSLookupSet newcust = (ItemOOSLookupSet)newValue;
					            
						oldcust.Parent = null;
						newcust.Parent = this;
					            
					            
						OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
					}
				}

				//=========================================================================

				// Called by Customer when it changes.
				internal void ItemChanged(ItemOOSLookupSet ItemOOSLookupSet) 
				{
					        
					int index = List.IndexOf(ItemOOSLookupSet);
					        
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
				}


				//=========================================================================

				// Implements IBindingList.
				bool IBindingList.AllowEdit 
				{ 
					get { return true ; }
				}

				bool IBindingList.AllowNew 
				{ 
					get { return false ; }
				}

				bool IBindingList.AllowRemove 
				{ 
					get { return false ; }
				}

				bool IBindingList.SupportsChangeNotification 
				{ 
					get { return true ; }
				}
					    
				bool IBindingList.SupportsSearching 
				{ 
					get { return false ; }
				}

				bool IBindingList.SupportsSorting 
				{ 
					get { return false ; }
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
					ItemOOSLookupSet c = new ItemOOSLookupSet();
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
			public class ItemOOSLookupAllColor : SizeOOSLookupItemBase, IEditableObject
			{

				#region Member Variables
				private CollectionOOSLookupAllColors parent;
				private bool inTxn = false;
				private CollectionOOSLookupSizeDimensions colAllColorSizeDimensions = new CollectionOOSLookupSizeDimensions();
				#endregion Member Variables

				#region Public Properties


				public CollectionOOSLookupSizeDimensions collectionSizeDimensions
				{
					get {return colAllColorSizeDimensions;}
				}

				#endregion Properties

				internal CollectionOOSLookupAllColors Parent 
				{
					get 
					{
						return parent;
					}
					set 
					{
						parent = value ;
					}
				}


				#region Constructors

				public ItemOOSLookupAllColor() : base()
				{
				}


				public ItemOOSLookupAllColor(int intMethodRid, int intColorRid, int intQty,
									int intSizesRid, int intSizeCodeRid, int intDimensionsRid, 
									int intSglRid, eSizeMethodRowType rowTypeID) : base(intMethodRid, intColorRid, intQty,
																						intSizesRid, intSizeCodeRid, intDimensionsRid, 
																						intSglRid, rowTypeID)
				{

				}


				public ItemOOSLookupAllColor(int intMethodRid, DataRow drAllColorItem) : base(intMethodRid, drAllColorItem)
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

			public class CollectionOOSLookupAllColors : CollectionBase, IBindingList
			{
				private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
				private ListChangedEventHandler onListChanged;

				//Change to the name of the object that holds the data in the next 4 items.
				public ItemOOSLookupAllColor this[int index] 
				{
					get 
					{
						return (ItemOOSLookupAllColor)(List[index]);
					}
					set 
					{
						List[index] = value;
					}
				}

				public int Add (ItemOOSLookupAllColor value) 
				{
					return List.Add(value);
				}

				public ItemOOSLookupAllColor AddNew() 
				{
					return (ItemOOSLookupAllColor)((IBindingList)this).AddNew();
				}

				public void Remove (ItemOOSLookupAllColor value) 
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
					foreach (ItemOOSLookupAllColor c in List) 
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
					ItemOOSLookupAllColor c = (ItemOOSLookupAllColor)value;
					c.Parent = this;
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
				}

				protected override void OnRemoveComplete(int index, object value) 
				{
					ItemOOSLookupAllColor c = (ItemOOSLookupAllColor)value;
					c.Parent = this;
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
				}

				protected override void OnSetComplete(int index, object oldValue, object newValue) 
				{
					if (oldValue != newValue) 
					{

						ItemOOSLookupAllColor olddata = (ItemOOSLookupAllColor)oldValue;
						ItemOOSLookupAllColor newdata = (ItemOOSLookupAllColor)newValue;
							            
						olddata.Parent = null;
						newdata.Parent = this;
							            
							            
						OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
					}
				}

				//=========================================================================


				//Change to the name of the object that holds the data here.
				//=========================================================================
				internal void ItemChanged(ItemOOSLookupAllColor allColorSet) 
				{
							        
					int index = List.IndexOf(allColorSet);
							        
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
				}


				//=========================================================================

				// Implements IBindingList.
				bool IBindingList.AllowEdit 
				{ 
					get { return true ; }
				}

				bool IBindingList.AllowNew 
				{ 
					get { return false ; }
				}

				bool IBindingList.AllowRemove 
				{ 
					get { return false ; }
				}

				bool IBindingList.SupportsChangeNotification 
				{ 
					get { return true ; }
				}
							    
				bool IBindingList.SupportsSearching 
				{ 
					get { return false ; }
				}

				bool IBindingList.SupportsSorting 
				{ 
					get { return false ; }
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
					ItemOOSLookupAllColor c = new ItemOOSLookupAllColor();
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
			public class ItemOOSLookupColor : SizeOOSLookupItemBase, IEditableObject
			{

				#region Member Variables
				private CollectionOOSLookupColors parent;
				private bool inTxn = false;
				//private CollectionOOSLookupSizes colColorSizes = new CollectionOOSLookupSizes();
				private CollectionOOSLookupSizeDimensions colColorSizeDimensions = new CollectionOOSLookupSizeDimensions();
				#endregion Member Variables

				#region Public Properties

				public CollectionOOSLookupSizeDimensions collectionSizeDimensions
				{
					get {return colColorSizeDimensions;}
				}

				#endregion Properties

				internal CollectionOOSLookupColors Parent 
				{
					get 
					{
						return parent;
					}
					set 
					{
						parent = value ;
					}
				}


				#region Constructors

				public ItemOOSLookupColor() : base()
				{
				}


				public ItemOOSLookupColor(int intMethodRid, int intColorRid, int intQty,
								int intSizesRid, int intSizeCodeRid, int intDimensionsRid, 
								int intSglRid, eSizeMethodRowType rowTypeID) : base(intMethodRid, intColorRid, intQty,
																					intSizesRid, intSizeCodeRid, intDimensionsRid, 
																					intSglRid, rowTypeID)
				{

				}


				public ItemOOSLookupColor(int intMethodRid, DataRow drColorItem) : base(intMethodRid, drColorItem)
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

			public class CollectionOOSLookupColors : CollectionBase, IBindingList
			{
				private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
				private ListChangedEventHandler onListChanged;

				//Change to the name of the object that holds the data in the next 4 items.
				public ItemOOSLookupColor this[int index] 
				{
					get 
					{
						return (ItemOOSLookupColor)(List[index]);
					}
					set 
					{
						List[index] = value;
					}
				}

				public int Add (ItemOOSLookupColor value) 
				{
					return List.Add(value);
				}

				public ItemOOSLookupColor AddNew() 
				{
					return (ItemOOSLookupColor)((IBindingList)this).AddNew();
				}

				public void Remove (ItemOOSLookupColor value) 
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
					foreach (ItemOOSLookupColor c in List) 
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
					ItemOOSLookupColor c = (ItemOOSLookupColor)value;
					c.Parent = this;
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
				}

				protected override void OnRemoveComplete(int index, object value) 
				{
					ItemOOSLookupColor c = (ItemOOSLookupColor)value;
					c.Parent = this;
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
				}

				protected override void OnSetComplete(int index, object oldValue, object newValue) 
				{
					if (oldValue != newValue) 
					{

						ItemOOSLookupColor olddata = (ItemOOSLookupColor)oldValue;
						ItemOOSLookupColor newdata = (ItemOOSLookupColor)newValue;
									            
						olddata.Parent = null;
						newdata.Parent = this;
									            
									            
						OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
					}
				}

				//=========================================================================


				//Change to the name of the object that holds the data here.
				//=========================================================================
				internal void ItemChanged(ItemOOSLookupColor colorSet) 
				{
									        
					int index = List.IndexOf(colorSet);
									        
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
				}


				//=========================================================================

				// Implements IBindingList.
				bool IBindingList.AllowEdit 
				{ 
					get { return true ; }
				}

				bool IBindingList.AllowNew 
				{ 
					get { return true ; }
				}

				bool IBindingList.AllowRemove 
				{ 
					get { return true ; }
				}

				bool IBindingList.SupportsChangeNotification 
				{ 
					get { return true ; }
				}
									    
				bool IBindingList.SupportsSearching 
				{ 
					get { return false ; }
				}

				bool IBindingList.SupportsSorting 
				{ 
					get { return false ; }
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
					ItemOOSLookupColor c = new ItemOOSLookupColor();
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
			public class ItemOOSLookupSize : SizeOOSLookupItemBase, IEditableObject
			{

				#region Member Variables
				private CollectionOOSLookupSizes parent;
				private bool inTxn = false;
				//private CollectionOOSLookupSizeDimensions colSizeDimensions = new CollectionOOSLookupSizeDimensions();

				#endregion Member Variables

				#region Public Properties

//					public CollectionOOSLookupSizeDimensions collectionSizeDimensions
//					{
//						get {return colSizeDimensions;}
//					}
//
				#endregion Properties

				internal CollectionOOSLookupSizes Parent 
				{
					get 
					{
						return parent;
					}
					set 
					{
						parent = value ;
					}
				}


				#region Constructors

				public ItemOOSLookupSize() : base()
				{
				}


				public ItemOOSLookupSize(int intMethodRid, int intColorRid, int intQty,
								int intSizesRid, int intSizeCodeRid, int intDimensionsRid, 
								int intSglRid, eSizeMethodRowType rowTypeID) : base(intMethodRid, intColorRid, intQty,
																					intSizesRid, intSizeCodeRid, intDimensionsRid, 
																					intSglRid, rowTypeID)
				{
						
				}


				public ItemOOSLookupSize(int intMethodRid, DataRow drSize) : base(intMethodRid, drSize)
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

			public class CollectionOOSLookupSizes : CollectionBase, IBindingList
			{
				private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
				private ListChangedEventHandler onListChanged;

				//Change to the name of the object that holds the data in the next 4 items.
				public ItemOOSLookupSize this[int index] 
				{
					get 
					{
						return (ItemOOSLookupSize)(List[index]);
					}
					set 
					{
						List[index] = value;
					}
				}

				public int Add (ItemOOSLookupSize value) 
				{
					return List.Add(value);
				}

				public ItemOOSLookupSize AddNew() 
				{
					return (ItemOOSLookupSize)((IBindingList)this).AddNew();
				}

				public void Remove (ItemOOSLookupSize value) 
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
					foreach (ItemOOSLookupSize c in List) 
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
					ItemOOSLookupSize c = (ItemOOSLookupSize)value;
					c.Parent = this;
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
				}

				protected override void OnRemoveComplete(int index, object value) 
				{
					ItemOOSLookupSize c = (ItemOOSLookupSize)value;
					c.Parent = this;
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
				}

				protected override void OnSetComplete(int index, object oldValue, object newValue) 
				{
					if (oldValue != newValue) 
					{

						ItemOOSLookupSize olddata = (ItemOOSLookupSize)oldValue;
						ItemOOSLookupSize newdata = (ItemOOSLookupSize)newValue;
											            
						olddata.Parent = null;
						newdata.Parent = this;
											            
											            
						OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
					}
				}

				//=========================================================================


				//Change to the name of the object that holds the data here.
				//=========================================================================
				internal void ItemChanged(ItemOOSLookupSize sizeSet) 
				{
											        
					int index = List.IndexOf(sizeSet);
											        
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
				}


				//=========================================================================

				// Implements IBindingList.
				bool IBindingList.AllowEdit 
				{ 
					get { return true ; }
				}

				bool IBindingList.AllowNew 
				{ 
					get { return true ; }
				}

				bool IBindingList.AllowRemove 
				{ 
					get { return true ; }
				}

				bool IBindingList.SupportsChangeNotification 
				{ 
					get { return true ; }
				}
											    
				bool IBindingList.SupportsSearching 
				{ 
					get { return false ; }
				}

				bool IBindingList.SupportsSorting 
				{ 
					get { return false ; }
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
					ItemOOSLookupSize c = new ItemOOSLookupSize();
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
			public class ItemOOSLookupSizeDimension : SizeOOSLookupItemBase, IEditableObject
			{

				#region Member Variables
				private CollectionOOSLookupSizeDimensions parent;
				private CollectionOOSLookupSizes colSizes = new CollectionOOSLookupSizes();
				private bool inTxn = false;
				#endregion Member Variables

				#region Public Properties

				public CollectionOOSLookupSizes collectionSizes
				{
					get {return colSizes;}
				}

				#endregion Properties

				internal CollectionOOSLookupSizeDimensions Parent 
				{
					get 
					{
						return parent;
					}
					set 
					{
						parent = value ;
					}
				}


				#region Constructors

				public ItemOOSLookupSizeDimension() : base()
				{
				}


				public ItemOOSLookupSizeDimension(int intMethodRid, int intColorRid, int intQty,
										int intSizesRid, int intSizeCodeRid, int intDimensionsRid, 
										int intSglRid, eSizeMethodRowType rowTypeID) : base(intMethodRid, intColorRid, intQty,
																							intSizesRid, intSizeCodeRid, intDimensionsRid, 
																							intSglRid, rowTypeID)
				{

				}


				public ItemOOSLookupSizeDimension(int intMethodRid, DataRow drSizeDimension) : base(intMethodRid, drSizeDimension)
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

			public class CollectionOOSLookupSizeDimensions : CollectionBase, IBindingList
			{
				private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
				private ListChangedEventHandler onListChanged;

				//Change to the name of the object that holds the data in the next 4 items.
				public ItemOOSLookupSizeDimension this[int index] 
				{
					get 
					{
						return (ItemOOSLookupSizeDimension)(List[index]);
					}
					set 
					{
						List[index] = value;
					}
				}

				public int Add (ItemOOSLookupSizeDimension value) 
				{
					return List.Add(value);
				}

				public ItemOOSLookupSizeDimension AddNew() 
				{
					return (ItemOOSLookupSizeDimension)((IBindingList)this).AddNew();
				}

				public void Remove (ItemOOSLookupSizeDimension value) 
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
					foreach (ItemOOSLookupSizeDimension c in List) 
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
					ItemOOSLookupSizeDimension c = (ItemOOSLookupSizeDimension)value;
					c.Parent = this;
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
				}

				protected override void OnRemoveComplete(int index, object value) 
				{
					ItemOOSLookupSizeDimension c = (ItemOOSLookupSizeDimension)value;
					c.Parent = this;
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
				}

				protected override void OnSetComplete(int index, object oldValue, object newValue) 
				{
					if (oldValue != newValue) 
					{

						ItemOOSLookupSizeDimension olddata = (ItemOOSLookupSizeDimension)oldValue;
						ItemOOSLookupSizeDimension newdata = (ItemOOSLookupSizeDimension)newValue;
														        
						olddata.Parent = null;
						newdata.Parent = this;
														        
														        
						OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
					}
				}

				//=========================================================================


				//Change to the name of the object that holds the data here.
				//=========================================================================
				internal void ItemChanged(ItemOOSLookupSizeDimension sizeDimensionSet) 
				{
														    
					int index = List.IndexOf(sizeDimensionSet);
														    
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
				}


				//=========================================================================

				// Implements IBindingList.
				bool IBindingList.AllowEdit 
				{ 
					get { return true ; }
				}

				bool IBindingList.AllowNew 
				{ 
					get { return true ; }
				}

				bool IBindingList.AllowRemove 
				{ 
					get { return true ; }
				}

				bool IBindingList.SupportsChangeNotification 
				{ 
					get { return true ; }
				}
														
				bool IBindingList.SupportsSearching 
				{ 
					get { return false ; }
				}

				bool IBindingList.SupportsSorting 
				{ 
					get { return false ; }
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
					ItemOOSLookupSizeDimension c = new ItemOOSLookupSizeDimension();
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


	#endregion

	#region Collection Decoder

	public class CollectionOOSLookupDecoder
	{
		//=============
		// Variables
		//=============
		private CollectionOOSLookupSets _collectionSets;
		//private CollectionRuleSets _collectionRuleSets;
		private eSizeCollectionType _collectionType;
		private bool _continue;
		private Hashtable _oosLookupHash; 
		//private Hashtable _ruleHash;   
		private Hashtable _sglHash;   

		//=============
		// CONstructors
		//=============

		public CollectionOOSLookupDecoder(CollectionOOSLookupSets aCollection, Hashtable sglHash) 
		{
			_collectionSets = aCollection;
			_sglHash = sglHash;			
			_collectionType = eSizeCollectionType.SizeOOSLookupCollection;
			_oosLookupHash = new Hashtable(); 
		}

		public object GetItemForStore(int stRid, int colorRid, SizeCodeProfile sizeCodeProfile) 
		{
			try
			{
				int sglRid = Include.NoRID;
				if (_sglHash.ContainsKey(stRid))
					sglRid = (int)_sglHash[stRid];

				object returnObj = GetItem(sglRid, colorRid, sizeCodeProfile);
				
				return returnObj;
			}
			catch
			{
				throw;
			}
		}

		public object GetItem(int sglRid, int colorRid, SizeCodeProfile sizeCodeProfile) 
		{
			int sizeCodeRid = sizeCodeProfile.Key;
			object returnObj = null;
			SizeOOSLookupItemBase oosLookup = null;
			RuleItemBase rule = null;
			_continue = true;
			
			oosLookup = GetItemOOSLookup(sglRid, colorRid, sizeCodeProfile); 
			returnObj = oosLookup;
			

			return returnObj;
		}

		public void DebugSetsCollection()
		{
			//PROCESS SETS AND ALL DESCENDANTS
			Debug.WriteLine("---------------------------------------------");
			if (_collectionType == eSizeCollectionType.SizeOOSLookupCollection)
			{
				foreach (ItemOOSLookupSet oItemSet in _collectionSets)
				{
					Debug.WriteLine("RID " + oItemSet.NodeRid.ToString() +
						" SGL " + oItemSet.SglRid.ToString() + 
						" QTY " + oItemSet.OOSQuantity.ToString());

					//PROCESS ALL COLOR LEVEL AND ALL DESCENDANTS
					foreach (ItemOOSLookupAllColor oItemAllColor in oItemSet.collectionAllColors)
					{
						DebugItem(1, oItemAllColor);

						foreach (ItemOOSLookupSizeDimension oItemSizeDimension in oItemAllColor.collectionSizeDimensions)
						{
							DebugItem(2, oItemSizeDimension);

							foreach (ItemOOSLookupSize oItemSize in oItemSizeDimension.collectionSizes)
							{
								DebugItem(3, oItemSize);
							}
						}
					}

					//PROCESS COLOR LEVEL AND ALL DESCENDANTS
					foreach (ItemOOSLookupColor oItemColor in oItemSet.collectionColors)
					{
						DebugItem(1, oItemColor);

						foreach (ItemOOSLookupSizeDimension oItemSizeDimension in oItemColor.collectionSizeDimensions)
						{
							DebugItem(2, oItemSizeDimension);

							foreach (ItemOOSLookupSize oItemSize in oItemSizeDimension.collectionSizes)
							{
								DebugItem(3, oItemSize);
							}

						}
					}
				}

			}
		}

		private void DebugItem(int level, SizeOOSLookupItemBase pItem)
		{
			string sLevel = string.Empty;
			switch (level)
			{
				case 1:
					sLevel = "  ";
					break;
				case 2: 
					sLevel = "    ";
					break;
				case 3:
					sLevel = "      ";
					break;
				default:
					sLevel = "--";
					break;
			}
			Debug.WriteLine(sLevel + "SGL " + pItem.SglRid.ToString() +
				" COLOR " + pItem.ColorCodeRid.ToString() +
				" DIM " + pItem.DimensionsRid.ToString() +
				" SIZE/SIZES " + pItem.SizeCodeRid.ToString() + "/" + pItem.SizesRid.ToString() +
				" QTY " + pItem.OOSQuantity.ToString());
		}

		#region oosLookup (Constraints)
		private SizeOOSLookupItemBase GetItemOOSLookup(int sglRid, int colorRid, SizeCodeProfile aSizeCodeProfile) 
		{
			SizeOOSLookupItemBase oosLookup;
			long oosLookupSGL_ColorKey = ((long) sglRid << 32) + colorRid;
			Hashtable sglColorHash;
			if (_oosLookupHash.Contains(oosLookupSGL_ColorKey))
			{
				sglColorHash = (Hashtable)_oosLookupHash[oosLookupSGL_ColorKey];
			}
			else
			{
				sglColorHash = new Hashtable();
				_oosLookupHash.Add(oosLookupSGL_ColorKey, sglColorHash);
			}
			if (sglColorHash.Contains(aSizeCodeProfile.Key))
			{
				oosLookup = (SizeOOSLookupItemBase)sglColorHash[aSizeCodeProfile.Key];
			}
			else
			{
				try
				{
					oosLookup = new SizeOOSLookupItemBase(sglRid, colorRid, aSizeCodeProfile); 
					_continue = true; 
					while (_continue)
					{
						oosLookup = SearchOOSLookupCollection(oosLookup);
					if (oosLookup.OOSQuantity != 0)
						{
							_continue = false;
						}
						else
						{
							if (oosLookup.ColorCodeRid != Include.MaskedRID) 
							{
								if (oosLookup.DimensionsRid != Include.MaskedRID) 
								{
									if (oosLookup.SizeCodeRid != Include.MaskedRID) 
									{
										oosLookup.SizeCodeRid = Include.MaskedRID; 
									}
									else
									{
										oosLookup.DimensionsRid = Include.MaskedRID; 
									}
								}
								else
								{
									oosLookup.DimensionsRid = aSizeCodeProfile.SizeCodeSecondaryRID;
									oosLookup.SizeCodeRid = aSizeCodeProfile.Key;
									oosLookup.ColorCodeRid = Include.MaskedRID; 
								}
							}
							else
							{
								if (oosLookup.DimensionsRid != Include.MaskedRID)   
								{
									if (oosLookup.SizeCodeRid != Include.MaskedRID) 
									{
										oosLookup.SizeCodeRid = Include.MaskedRID; 
									}
									else
									{
										oosLookup.DimensionsRid = Include.MaskedRID; 
									}
								}
								else
								{
									if (oosLookup.SglRid == Include.NoRID)
									{
										_continue = false;
									}
									else
									{
										oosLookup.SglRid = Include.NoRID; 
										oosLookup.ColorCodeRid = colorRid;
										oosLookup.DimensionsRid = aSizeCodeProfile.SizeCodeSecondaryRID;
										oosLookup.SizeCodeRid = aSizeCodeProfile.Key; 
									}
								}
							}
						}
					}
					sglColorHash.Add(aSizeCodeProfile.Key, oosLookup); 
					//return oosLookup;

				}
				catch (Exception)
				{
					throw;
				}
			}
			return oosLookup; 
		}

		public SizeOOSLookupItemBase SearchOOSLookupCollection(SizeOOSLookupItemBase oosLookup)
		{
			foreach (ItemOOSLookupSet oItemSet in _collectionSets)
			{
				if (oItemSet.SglRid == oosLookup.SglRid) 
				{
					if (oosLookup.ColorCodeRid == Include.MaskedRID) 
					{
						//PROCESS ALL COLOR LEVEL AND ALL DESCENDANTS
						foreach (ItemOOSLookupAllColor oItemAllColor in oItemSet.collectionAllColors)
						{
							foreach (ItemOOSLookupSizeDimension oItemSizeDimension in oItemAllColor.collectionSizeDimensions)
							{
								if (oItemSizeDimension.DimensionsRid == oosLookup.DimensionsRid) 
								{                                                             
									foreach (ItemOOSLookupSize oItemSize in oItemSizeDimension.collectionSizes)
									{
										if (oItemSize.SizeCodeRid == oosLookup.SizeCodeRid) 
										{                                                
											if (oosLookup.OOSQuantity == 0)
												oosLookup.OOSQuantity = oItemSize.OOSQuantity;
											
											break;                                       
										}                                                
									}
									if (oosLookup.OOSQuantity == 0)
										oosLookup.OOSQuantity = oItemSizeDimension.OOSQuantity;
									
									break;
								}   
							}

							if (oosLookup.OOSQuantity == 0)
								oosLookup.OOSQuantity = oItemAllColor.OOSQuantity;
							break;
						}
					}
					else
					{
						//PROCESS COLOR LEVEL AND ALL DESCENDANTS
						foreach (ItemOOSLookupColor oItemColor in oItemSet.collectionColors)
						{
							if (oItemColor.ColorCodeRid == oosLookup.ColorCodeRid)
							{
								foreach (ItemOOSLookupSizeDimension oItemSizeDimension in oItemColor.collectionSizeDimensions)
								{
									if (oItemSizeDimension.DimensionsRid == oosLookup.DimensionsRid) 
									{                                                            
										foreach (ItemOOSLookupSize oItemSize in oItemSizeDimension.collectionSizes)
										{
											if (oItemSize.SizeCodeRid == oosLookup.SizeCodeRid)
											{
												oosLookup.OOSQuantity = oItemSize.OOSQuantity;
												break;
											}
										}
										if (oosLookup.OOSQuantity == 0)
											oosLookup.OOSQuantity = oItemSizeDimension.OOSQuantity;
										break;
									}   
								}

								if (oosLookup.OOSQuantity == 0)
									oosLookup.OOSQuantity = oItemColor.OOSQuantity;
								break; 
							}
						}
					}

					if (oosLookup.OOSQuantity == 0)
						oosLookup.OOSQuantity = oItemSet.OOSQuantity;
					break; 
				}
			}

			return oosLookup;
		}
		#endregion

	}


	#endregion 

	#region All SizeOutOfStockLookup classes

	#region SizeOOSLookupItemBase
	public class SizeOOSLookupItemBase
	{
		#region Member Variables
		private int _hnRid;
		private int _oosQty;

		private int _color_code_rid;
		private int _sizes_rid;
		private int _size_code_rid;
		private int _dimensions_rid;
		private int _sgl_rid;
		private eSizeMethodRowType _row_type_id;

		#endregion

		#region Constructors

		public SizeOOSLookupItemBase()
		{
			_hnRid = Include.NoRID;
			_row_type_id = 0;
			_size_code_rid = Include.NoRID;
			_sizes_rid = Include.NoRID;
			_dimensions_rid = Include.NoRID;
			_oosQty = 0;
			_sgl_rid = Include.NoRID;
			_color_code_rid = Include.NoRID;
		}

		public SizeOOSLookupItemBase(int sglRid, int colorRid, SizeCodeProfile aSizeCodeProfile) 
		{
			_hnRid = Include.NoRID;
			_row_type_id = 0;
			_size_code_rid = aSizeCodeProfile.Key;
			_sizes_rid = aSizeCodeProfile.SizeCodePrimaryRID;
			_dimensions_rid = aSizeCodeProfile.SizeCodeSecondaryRID;
			_oosQty = 0;
			_sgl_rid = sglRid;
			_color_code_rid = colorRid;
		}


		public SizeOOSLookupItemBase(int intHnRid, int intColorRid, 
							int intValue,
							int intSizesRid, int intSizeCodeRid, int intDimensionsRid,
							int intSglRid, eSizeMethodRowType rowTypeID)
		{
			_hnRid = intHnRid;
			_row_type_id = rowTypeID;
			_size_code_rid = intSizeCodeRid;
			_sizes_rid = intSizesRid;
			_dimensions_rid = intDimensionsRid;
			_oosQty = intValue;
			_sgl_rid = intSglRid;
			_color_code_rid = intColorRid;
		}


		public SizeOOSLookupItemBase(int intHnRid, DataRow drSize)
		{
			_hnRid = intHnRid;

			//SET VALUES FROM THE DATAROW
			//===========================
			_row_type_id = (eSizeMethodRowType)drSize["ROW_TYPE_ID"];
			_oosQty = (drSize["OOS_QUANTITY"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["OOS_QUANTITY"], CultureInfo.CurrentUICulture) : Include.UndefinedMinimum;
			_color_code_rid = (drSize["COLOR_CODE_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["COLOR_CODE_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;
			_sizes_rid = (drSize["SIZES_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SIZES_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;
			_dimensions_rid = (drSize["DIMENSIONS_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["DIMENSIONS_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;
			_size_code_rid = (drSize["SIZE_CODE_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SIZE_CODE_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;
			_sgl_rid = (drSize["SGL_RID"].ToString().Trim() != string.Empty) ? Convert.ToInt32(drSize["SGL_RID"], CultureInfo.CurrentUICulture) : Include.NoRID;

		}


		#endregion

		#region SizeOutOfStockLookupItemBase Members


		public int NodeRid
		{
			get { return _hnRid; }
			set { _hnRid = value; }
		}

		public int OOSQuantity
		{
			get { return _oosQty; }
			set { _oosQty = value; }
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
	#endregion


	#endregion
}