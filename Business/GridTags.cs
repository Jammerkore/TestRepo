using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using C1.Win.C1FlexGrid;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
    public delegate void PageLoadDelegate(C1FlexGrid aGrid, int aStartRow, int aStartCol, int aEndRow, int aEndCol, int aPriority);

	/// <summary>
	/// this tells us which grid we're dealing with.
	/// </summary>
	
	public enum FromGrid 
	{
		g1 = 1,
		g2 = 2,
		g3 = 3,
		g4 = 4,
		g5 = 5,
		g6 = 6,
		g7 = 7,
		g8 = 8,
		g9 = 9,
		g10 = 10,
		g11 = 11,
		g12 = 12
	}
	
	/// <summary>
	/// How something is sorted: ascending/descending, or "none" for not sorted.
	/// </summary>
	
	public enum SortEnum
	{
		none = 0,
		asc = 1,
		desc = 2
	}
	
	/// <summary>
	/// This describes the action the user is try to take while dragging the 
	/// mouse pointer.
	/// </summary>
	
	public enum DragState
	{
		dragNone,
		dragReady,
		dragStarted,
		dragResize
	}
	
	/// <summary>
	/// This is a "tag" that captures everything that applies to a column.
	/// </summary>
	
	public struct TagForColumn
	{
		public bool IsLockable;
		public bool IsDisplayed;
		public bool IsBuilt;
		public SortEnum Sort;
		public int cellColumn;
		public AllocationWaferCoordinateList CubeWaferCoorList;
	}
	
	/// <summary>
	/// This is a "tag" that captures everything that applies to a row.
	/// </summary>
	
	public struct TagForRow
	{
		public bool IsLockable;
		public bool IsDisplayed;
		public AllocationWaferCoordinateList CubeWaferCoorList;
		public int cellRow;
	}
	
	/// <summary>
	/// This is a "tag" that captures everything for a single cell.
	/// </summary>
	
	public struct TagForGridData
	{
		public bool IsEditable;
		public bool IsLocked;
		public bool IsOutOfBalance;
	}
	
	public struct Fonts
	{
		public Font g1;			//"Store"
		public Font MerDesc;	//"Mens's Sweater"
		public Font GroupHeader;	//"Week" or "Total" (if grouped by period) or "Stock", "Sales", etc (if grouped by variable)
		public Font ColumnHeading;  //"Week" (if grouped by variable) or "Stock", "Sales", etc (if grouped by period)
		public Font Main;	//Main font in g5, g6, g8, g9, g11, g12
		public Font Editable; //For cells that are editable
		public Font RowHeader; //in g4, g7, and g10
		public Font IneligStores; //"Ineligible Stores"
		public Font Lock; //"Ineligible Stores"
	}
	
	public struct TextEffects
	{
		public TextEffectEnum MerDesc;
		public TextEffectEnum GroupHeader;
		public TextEffectEnum ColumnHeading;
	}
	
	/// <summary>
	/// back ground colors
	/// </summary>
	
	public struct BGColors
	{
		public Color g1;			//"Store"
		public Color MerDesc;		//"Mens's Sweater"
		public Color GroupHeader;	//"Week" or "Total" (if grouped by period) or "Stock", "Sales", etc (if grouped by variable)
		public Color ColumnHeading;  //"Week" (if grouped by variable) or "Stock", "Sales", etc (if grouped by period)
		public Color g4Color1;
		public Color g4Color2;
		public Color g5g6Color1;
		public Color g5g6Color2;
		public Color g7Color1;
		public Color g7Color2;
		public Color g8g9Color1;
		public Color g8g9Color2;
		public Color g10Color1;
		public Color g10Color2;
		public Color g11g12Color1;
		public Color g11g12Color2;
	}
	
	/// <summary>
	/// text colors/font colors
	/// </summary>
	
	public struct ForeColors
	{
		public Color g1;			//"Store"
		public Color MerDesc;	//"Mens's Sweater"
		public Color GroupHeader;	//"Week" or "Total" (if grouped by period) or "Stock", "Sales", etc (if grouped by variable)
		public Color ColumnHeading;  //"Week" (if grouped by variable) or "Stock", "Sales", etc (if grouped by period)
		public Color Negative; //All the grids will use the same color for negative numbers.
		public Color g4;
		public Color g5g6;
		public Color g7;
		public Color g8g9;
		public Color g10;
		public Color g11g12;
	}
	
	/// <summary>
	/// This structure itself is not used directly in code. Rather, the 
	/// "BorderColors" structure uses a variable of THIS structure, so that we can
	/// use something like: BorderColors.BorderBrushes.horizontal.
	/// Check the last item in the "BorderColors" structure for more clarity.
	/// </summary>
	
	public struct Brushes
	{
		public SolidBrush horizontal;
		public SolidBrush vertical;
		public SolidBrush rowHeader;
		public SolidBrush chiselUpper;
		public SolidBrush chiselLower;
	}
	
	public struct BorderColors
	{
		public Color Vertical;
		public Color Horizontal;
		public Color RowHeader;
		public Color ChiseledUpper; //this won't apply if the style itself is not chiseled.
		public Color ChiseledLower; //this won't apply if the style itself is not chiseled.
		public Brushes BorderBrushes; //type "Brushes" is another structure we declared earlier.
	}
	
	/// <summary>
	/// Misc. properties about a style/theme.
	/// </summary>
	
	public struct ViewStyle
	{
		public StyleEnum Style;
		public bool DisplayColGroupBorder; //whether each var/period group should be segregated by a vertical bar.
		public bool DisplayRowGroupBorder; //whether each store/chain group should be segregated by a horizontal bar.
		public BorderStyleEnum borderStyle;
		public Color borderColor;		//the color surrounding each cell.
		public int DividerWidth;		//width of ver. and hor. group dividers
	}

	//-------------------------------------------------
	// The following two classes have replaced TagForColumn and TagForRow structures in PlanView.cs.  The reason these were redefined as classes is because of the
	// boxing and unboxing that occurs when storing value types (structs, numbers, etc) in an object variable.  When storing information in an object variable,
	// it is recommended to used classes.  The other screens that use TagForRow and TagForColumn should eventually be changed to use these classes.
	//-------------------------------------------------

	/// <summary>
	/// The ColumnHeaderTag class describes the information that is stored in the column's UserData field of the header grids.
	/// </summary>

	public class ColumnHeaderTag
	{
		//=======
		// FIELDS
		//=======

		private SortEnum _sort;
		private CubeWaferCoordinateList _cubeWaferCoorList;
		private RowColProfileHeader _groupRowColHeader;
		private RowColProfileHeader _detailRowColHeader;
		private int _order;
		private string[] _scrollDisplay;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ColumnHeaderTag, using the given WaferCoordinateList, group RowColProfileHeader and detail RowColProfileHeader.
		/// </summary>
		/// <param name="aCoorList">
		/// A WaferCoordinateList object that describes the contents of the column.
		/// </param>
		/// <param name="aGroupRowColHeader">
		/// A RowColProfileHeader object that describes the group level information.
		/// </param>
		/// <param name="aDetailRowColHeader">
		/// A RowColProfileHeader object that describes the detail level information.
		/// </param>

		public ColumnHeaderTag(CubeWaferCoordinateList aCoorList, RowColProfileHeader aGroupRowColHeader, RowColProfileHeader aDetailRowColHeader, int aOrder, string aScrollDisplay)
		{
			_sort = SortEnum.none;
			_cubeWaferCoorList = aCoorList;
			_groupRowColHeader = aGroupRowColHeader;
			_detailRowColHeader = aDetailRowColHeader;
			_order = aOrder;
			_scrollDisplay = aScrollDisplay.Split('|');
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the SortEnum that describes the sort type of this column.
		/// </summary>

		public SortEnum Sort
		{
			get
			{
				return _sort;
			}
			set
			{
				_sort = value;
			}
		}

		/// <summary>
		/// Gets or sets the WaferCoordinateList that describes the contents of the column.
		/// </summary>

		public CubeWaferCoordinateList CubeWaferCoorList
		{
		    get
		    {
		        return _cubeWaferCoorList;
		    }
		    set
		    {
		        _cubeWaferCoorList = value;
		    }
		}

		/// <summary>
		/// Gets or sets the RowColProfileHeader object that contains displayable information.
		/// </summary>

		public RowColProfileHeader GroupRowColHeader
		{
			get
			{
				return _groupRowColHeader;
			}
			set
			{
				_groupRowColHeader = value;
			}
		}

		/// <summary>
		/// Gets or sets the RowColProfileHeader object that contains displayable information.
		/// </summary>

		public RowColProfileHeader DetailRowColHeader
		{
			get
			{
				return _detailRowColHeader;
			}
			set
			{
				_detailRowColHeader = value;
			}
		}

		/// <summary>
		/// Gets the order value that contains the column order.
		/// </summary>

		public int Order
		{
			get
			{
				return _order;
			}
		}

		/// <summary>
		/// Gets the string to display on scroll.
		/// </summary>

		public string[] ScrollDisplay
		{
			get
			{
				return _scrollDisplay;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The RowHeaderTag class describes the information that is stored in the row's UserData field of the header grids.
	/// </summary>

	public class RowHeaderTag
	{
		//=======
		// FIELDS
		//=======

		private CubeWaferCoordinateList _cubeWaferCoorList;
		private RowColProfileHeader _groupRowColHeader;
		private RowColProfileHeader _detailRowColHeader;
		private int _order;
		private string _rowHeading;
		private string[] _scrollDisplay;
		private bool _loadData;
		private DataRow _dataRow;
		private bool _drawBorder;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of RowHeaderTag, using the given WaferCoordinateList and RowColProfileHeader.
		/// </summary>
		/// <param name="aCoorList">
		/// A WaferCoordinateList object that describes the contents of the row.
		/// </param>
		/// <param name="aGroupRowColHeader">
		/// A RowColProfileHeader object that describes the group level information.
		/// </param>
		/// <param name="aDetailRowColHeader">
		/// A RowColProfileHeader object that describes the detail level information.
		/// </param>
		/// <param name="aOrder">
		/// The current order of the row.
		/// </param>
		/// <param name="aRowHeading">
		/// The row heading.
		/// </param>
		/// <param name="aScrollDisplay">
		/// The string that is displayed during a scroll-drag event.
		/// </param>

		public RowHeaderTag(CubeWaferCoordinateList aCoorList, RowColProfileHeader aGroupRowColHeader, RowColProfileHeader aDetailRowColHeader, int aOrder, string aRowHeading, string aScrollDisplay)
		{
			_cubeWaferCoorList = aCoorList;
			_groupRowColHeader = aGroupRowColHeader;
			_detailRowColHeader = aDetailRowColHeader;
			_order = aOrder;
			_rowHeading = aRowHeading;
			_scrollDisplay = aScrollDisplay.Split('|');
			_loadData = true;
			_drawBorder = false;
		}

		/// <summary>
		/// Creates a new instance of RowHeaderTag, using the given WaferCoordinateList and RowColProfileHeader.
		/// </summary>
		/// <param name="aCoorList">
		/// A WaferCoordinateList object that describes the contents of the row.
		/// </param>
		/// <param name="aGroupRowColHeader">
		/// A RowColProfileHeader object that describes the group level information.
		/// </param>
		/// <param name="aDetailRowColHeader">
		/// A RowColProfileHeader object that describes the detail level information.
		/// </param>
		/// <param name="aOrder">
		/// The current order of the row.
		/// </param>
		/// <param name="aRowHeading">
		/// The row heading.
		/// </param>
		/// <param name="aScrollDisplay">
		/// The string that is displayed during a scroll-drag event.
		/// </param>

		public RowHeaderTag(CubeWaferCoordinateList aCoorList, RowColProfileHeader aGroupRowColHeader, RowColProfileHeader aDetailRowColHeader, int aOrder, string aRowHeading, string aScrollDisplay, DataRow aDataRow)
		{
			_cubeWaferCoorList = aCoorList;
			_groupRowColHeader = aGroupRowColHeader;
			_detailRowColHeader = aDetailRowColHeader;
			_order = aOrder;
			_rowHeading = aRowHeading;
			_scrollDisplay = aScrollDisplay.Split('|');
			_dataRow = aDataRow;
			_loadData = true;
			_drawBorder = false;
		}

		/// <summary>
		/// Creates a new instance of RowHeaderTag, using the given WaferCoordinateList and RowColProfileHeader.
		/// </summary>
		/// <param name="aCoorList">
		/// A WaferCoordinateList object that describes the contents of the row.
		/// </param>
		/// <param name="aGroupRowColHeader">
		/// A RowColProfileHeader object that describes the group level information.
		/// </param>
		/// <param name="aDetailRowColHeader">
		/// A RowColProfileHeader object that describes the detail level information.
		/// </param>
		/// <param name="aOrder">
		/// The current order of the row.
		/// </param>
		/// <param name="aRowHeading">
		/// The row heading.
		/// </param>
		/// <param name="aScrollDisplay">
		/// The string that is displayed during a scroll-drag event.
		/// </param>
		/// <param name="aLoadData">
		/// A boolean indicating whether the row should be loaded by the data load routines.
		/// </param>

		public RowHeaderTag(CubeWaferCoordinateList aCoorList, RowColProfileHeader aGroupRowColHeader, RowColProfileHeader aDetailRowColHeader, int aOrder, string aRowHeading, string aScrollDisplay, bool aLoadData)
		{
			_cubeWaferCoorList = aCoorList;
			_groupRowColHeader = aGroupRowColHeader;
			_detailRowColHeader = aDetailRowColHeader;
			_order = aOrder;
			_rowHeading = aRowHeading;
			_scrollDisplay = aScrollDisplay.Split('|');
			_loadData = aLoadData;
			_drawBorder = false;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the WaferCoordinateList that describes the contents of the row.
		/// </summary>

		public CubeWaferCoordinateList CubeWaferCoorList
		{
			get
			{
				return _cubeWaferCoorList;
			}
			set
			{
				_cubeWaferCoorList = value;
			}
		}

		/// <summary>
		/// Gets or sets the RowColProfileHeader object that contains displayable information.
		/// </summary>

		public RowColProfileHeader GroupRowColHeader
		{
			get
			{
				return _groupRowColHeader;
			}
			set
			{
				_groupRowColHeader = value;
			}
		}

		/// <summary>
		/// Gets or sets the RowColProfileHeader object that contains displayable information.
		/// </summary>

		public RowColProfileHeader DetailRowColHeader
		{
			get
			{
				return _detailRowColHeader;
			}
			set
			{
				_detailRowColHeader = value;
			}
		}

		/// <summary>
		/// Gets the order value that contains the row order.
		/// </summary>

		public int Order
		{
			get
			{
				return _order;
			}
		}

		/// <summary>
		/// Gets the string for the Row Heading.
		/// </summary>

		public string RowHeading
		{
			get
			{
				return _rowHeading;
			}
		}

		/// <summary>
		/// Gets the string to display on scroll.
		/// </summary>

		public string[] ScrollDisplay
		{
			get
			{
				return _scrollDisplay;
			}
		}

		/// <summary>
		/// Gets the DataRow object associated with this grid row.
		/// </summary>

		public DataRow DataRow
		{
			get
			{
				return _dataRow;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if data should be loaded to this row.
		/// </summary>

		public bool LoadData
		{
			get
			{
				return _loadData;
			}
		}

		/// <summary>
		/// Gets the boolean indicating if a border should be draw for this row.
		/// </summary>

		public bool DrawBorder
		{
			get
			{
				return _drawBorder;
			}
			set
			{
				_drawBorder = value;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The ColumnTag class describes the information that is stored in the column's UserData field of the detail grids.
	/// </summary>

	public class ColumnTag
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ColumnTag.
		/// </summary>

		public ColumnTag()
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The RowTag class describes the information that is stored in the row's UserData field of the detail grids.
	/// </summary>

	public class RowTag
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of RowTag.
		/// </summary>

		public RowTag()
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The BaseGridTag class describes the information that is stored in the Grid's tag field.
	/// </summary>

	abstract public class BaseGridTag
	{
		//=======
		// FIELDS
		//=======

		protected int _gridId;
		protected C1.Win.C1FlexGrid.C1FlexGrid _rowHeaderGrid;
		protected C1.Win.C1FlexGrid.C1FlexGrid _colHeaderGrid;

		protected int _rowsPerRowGroup;
		protected int _rowGroupsPerGrid;
		protected int _rowsPerScroll;
		protected int _colsPerColGroup;
		protected int _colGroupsPerGrid;
		protected int _colsPerScroll;
		protected int _currentRowScrollPosition;
		protected int _currentColScrollPosition;
		protected int _currentScrollGroup;
		//Begin Track #5659 - JScott - All numbers disappeared
		protected eScrollType _scrollType;
		//End Track #5659 - JScott - All numbers disappeared

		protected bool _hasColsFrozen;
		//Begin Track #5003 - JScott - Freeze Rows
		protected bool _hasRowsFrozen;
		protected int _leftMostColBeforeFreeze;
		protected int _topMostRowBeforeFreeze;
		//End Track #5003 - JScott - Freeze Rows
		protected int _mouseDownRow;
		protected int _mouseDownCol;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of GridTag, assigning the given grid id to the _gridId field.
		/// </summary>
		/// <param name="aGridId">
		/// The numeric id that uniquely identifies the grid.
		/// </param>

		public BaseGridTag(int aGridId, C1.Win.C1FlexGrid.C1FlexGrid aRowHeaderGrid, C1.Win.C1FlexGrid.C1FlexGrid aColHeaderGrid)
		{
			_gridId = aGridId;
			_rowHeaderGrid = aRowHeaderGrid;
			_colHeaderGrid = aColHeaderGrid;

			_rowsPerRowGroup = 0;
			_rowGroupsPerGrid = 0;
			_rowsPerScroll = 0;
			_colsPerColGroup = 0;
			_colGroupsPerGrid = 0;
			_colsPerScroll = 0;
			_currentRowScrollPosition = 0;
			_currentColScrollPosition = 0;
			_currentScrollGroup = 0;
			//Begin Track #5659 - JScott - All numbers disappeared
			_scrollType = eScrollType.None;
			//End Track #5659 - JScott - All numbers disappeared

			_hasColsFrozen = false;
			//Begin Track #5003 - JScott - Freeze Rows
			_hasRowsFrozen = false;
			_leftMostColBeforeFreeze = -1;
			_topMostRowBeforeFreeze = -1;
			//End Track #5003 - JScott - Freeze Rows
			_mouseDownRow = -1;
			_mouseDownCol = -1;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the unique id of the grid.
		/// </summary>

		public int GridId
		{
			get
			{
				return _gridId;
			}
		}

		/// <summary>
		/// Returns the FlexGrid that contains the RowHeaders.
		/// </summary>

		public C1.Win.C1FlexGrid.C1FlexGrid RowHeaderGrid
		{
			get
			{
				return _rowHeaderGrid;
			}
		}

		/// <summary>
		/// Returns the FlexGrid that contains the ColHeaders.
		/// </summary>

		public C1.Win.C1FlexGrid.C1FlexGrid ColHeaderGrid
		{
			get
			{
				return _colHeaderGrid;
			}
		}

		/// <summary>
		/// Gets or sets the detail per group.
		/// </summary>

		public int DetailsPerGroup
		{
			get
			{
				return _rowsPerRowGroup;
			}
			set
			{
				_rowsPerRowGroup = value;
			}
		}

		/// <summary>
		/// Gets or sets the rows per group.
		/// </summary>

		public int GroupsPerGrid
		{
			get
			{
				return _rowGroupsPerGrid;
			}
			set
			{
				_rowGroupsPerGrid = value;
			}
		}

		/// <summary>
		/// Gets or sets the units per scroll.
		/// </summary>

		public int UnitsPerScroll
		{
			get
			{
				return _rowsPerScroll;
			}
			set
			{
				_rowsPerScroll = value;
				// Begin Track #6130 stodd
				if (_rowsPerScroll == 0)
					_rowsPerScroll = 1;
				// End Track #6130
			}
		}

		/// <summary>
		/// Gets or sets the current scroll position.
		/// </summary>

		public int CurrentScrollPosition
		{
			get
			{
				return _currentRowScrollPosition;
			}
			set
			{
				_currentRowScrollPosition = value;
				//Begin Track #5659 - JScott - All numbers disappeared

				if (_scrollType == eScrollType.Line)
				{
					if (_rowsPerScroll != 0)
					{
						_currentScrollGroup = _currentRowScrollPosition / _rowsPerScroll;
					}
					else
					{
						_currentScrollGroup = 0;
					}
				}
				else
				{
					_currentScrollGroup = _currentRowScrollPosition;
				}
				//End Track #5659 - JScott - All numbers disappeared
			}
		}

		/// <summary>
		/// Gets or sets the rows per group.
		/// </summary>

		public int RowsPerRowGroup
		{
			get
			{
				return _rowsPerRowGroup;
			}
			set
			{
				_rowsPerRowGroup = value;
			}
		}

		/// <summary>
		/// Gets or sets the row groups per grid.
		/// </summary>

		public int RowGroupsPerGrid
		{
			get
			{
				return _rowGroupsPerGrid;
			}
			set
			{
				_rowGroupsPerGrid = value;
			}
		}

		/// <summary>
		/// Gets or sets the rows per scroll.
		/// </summary>

		public int RowsPerScroll
		{
			get
			{
				return _rowsPerScroll;
			}
			set
			{
				_rowsPerScroll = value;
			}
		}

		/// <summary>
		/// Gets or sets the cols per group.
		/// </summary>

		public int ColsPerColGroup
		{
			get
			{
				return _colsPerColGroup;
			}
			set
			{
				_colsPerColGroup = value;
			}
		}

		/// <summary>
		/// Gets or sets the col groups per grid.
		/// </summary>

		public int ColGroupsPerGrid
		{
			get
			{
				return _colGroupsPerGrid;
			}
			set
			{
				_colGroupsPerGrid = value;
			}
		}

		/// <summary>
		/// Gets or sets the cols per scroll.
		/// </summary>

		public int ColsPerScroll
		{
			get
			{
				return _colsPerScroll;
			}
			set
			{
				_colsPerScroll = value;
			}
		}

		/// <summary>
		/// Gets or sets the current col scroll position.
		/// </summary>

		public int CurrentColScrollPosition
		{
			get
			{
				return _currentColScrollPosition;
			}
			set
			{
				_currentColScrollPosition = value;
			}
		}

		/// <summary>
		/// Gets or sets the current scroll group.
		/// </summary>

		public int CurrentScrollGroup
		{
			get
			{
				return _currentScrollGroup;
			}
		}

		//Begin Track #5659 - JScott - All numbers disappeared
		/// <summary>
		/// Gets or sets the scroll type.
		/// </summary>

		public eScrollType ScrollType
		{
			get
			{
				return _scrollType;
			}
			set
			{
				_scrollType = value;
			}
		}

		//End Track #5659 - JScott - All numbers disappeared
		/// <summary>
		/// Gets or sets the boolean indicating if there are frozen columns in the grid.
		/// </summary>

		public bool HasColsFrozen
		{
			get
			{
				return _hasColsFrozen;
			}
			set
			{
				_hasColsFrozen = value;
			}
		}

		//Begin Track #5003 - JScott - Freeze Rows
		/// <summary>
		/// Gets or sets the boolean indicating if there are frozen rows in the grid.
		/// </summary>

		public bool HasRowsFrozen
		{
			get
			{
				return _hasRowsFrozen;
			}
			set
			{
				_hasRowsFrozen = value;
			}
		}

		/// <summary>
		/// Returns the SortedList that contains the SortedRowHeaders.
		/// </summary>

		public int LeftMostColBeforeFreeze
		{
			get
			{
				return _leftMostColBeforeFreeze;
			}
			set
			{
				_leftMostColBeforeFreeze = value;
			}
		}

		/// <summary>
		/// Returns the SortedList that contains the SortedRowHeaders.
		/// </summary>

		public int TopMostRowBeforeFreeze
		{
			get
			{
				return _topMostRowBeforeFreeze;
			}
			set
			{
				_topMostRowBeforeFreeze = value;
			}
		}

		//End Track #5003 - JScott - Freeze Rows
		/// <summary>
		/// Gets or sets the row where the mouse was clicked.
		/// </summary>

		public int MouseDownRow
		{
			get
			{
				return _mouseDownRow;
			}
			set
			{
				_mouseDownRow = value;
			}
		}

		/// <summary>
		/// Gets or sets the column where the mouse was clicked.
		/// </summary>

		public int MouseDownCol
		{
			get
			{
				return _mouseDownCol;
			}
			set
			{
				_mouseDownCol = value;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The GridTag class describes the information that is stored in the Grid's tag field.
	/// </summary>

	public class GridTag : BaseGridTag
	{
		//=======
		// FIELDS
		//=======

		private int _loadStartTopRow;
		private int _loadStartBottomRow;
		private int _loadStartLeftCol;
		private int _loadStartRightCol;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of GridTag, assigning the given grid id to the _gridId field.
		/// </summary>
		/// <param name="aGridId">
		/// The numeric id that uniquely identifies the grid.
		/// </param>

        public GridTag(int aGridId, C1.Win.C1FlexGrid.C1FlexGrid aRowHeaderGrid, C1.Win.C1FlexGrid.C1FlexGrid aColHeaderGrid)
			: base(aGridId, aRowHeaderGrid, aColHeaderGrid)
		{
			_loadStartTopRow = -1;
			_loadStartBottomRow = -1;
			_loadStartLeftCol = -1;
			_loadStartRightCol = -1;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets starting top row for the grid load thread.
		/// </summary>

		public int LoadStartTopRow
		{
			get
			{
				return _loadStartTopRow;
			}
			set
			{
				_loadStartTopRow = value;
			}
		}

		/// <summary>
		/// Gets or sets starting bottom row for the grid load thread.
		/// </summary>

		public int LoadStartBottomRow
		{
			get
			{
				return _loadStartBottomRow;
			}
			set
			{
				_loadStartBottomRow = value;
			}
		}

		/// <summary>
		/// Gets or sets starting left column for the grid load thread.
		/// </summary>

		public int LoadStartLeftCol
		{
			get
			{
				return _loadStartLeftCol;
			}
			set
			{
				_loadStartLeftCol = value;
			}
		}

		/// <summary>
		/// Gets or sets starting right column for the grid load thread.
		/// </summary>

		public int LoadStartRightCol
		{
			get
			{
				return _loadStartRightCol;
			}
			set
			{
				_loadStartRightCol = value;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The PagingGridTag class describes the information that is stored in the Grid's tag field.
	/// </summary>

	public class PagingGridTag : BaseGridTag
	{
		//=======
		// FIELDS
		//=======

        private C1.Win.C1FlexGrid.C1FlexGrid _grid;
		private PageLoadDelegate _pageLoadDelegate;
		private int _rowPageSize;
		private int _colPageSize;
		private ArrayList _selectableColHeaders;
		private ArrayList _selectableRowHeaders;
		private SortedList _sortedColHeaders;
		private SortedList _sortedRowHeaders;
		private bool _showRowBorders;
		private bool _showColBorders;
		//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
		private int _sortImageRow;
		//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw

		private bool _visible;
		private int _visibleRowCount;
		//Begin Track #5659 - JScott - All numbers disappeared
		//private eScrollType _scrollType;
		//End Track #5659 - JScott - All numbers disappeared
		private int _initialVisibleRows;
		private PageLoadInfo[,] _pagesLoadInfo;

		//========
		// CLASSES
		//========

		public class PageLoadInfo
		{
			//=======
			// FIELDS
			//=======

			private PagingGridTag _gridTag;
            private C1.Win.C1FlexGrid.C1FlexGrid _grid;
			private Point _page;
			private PageLoadDelegate _pageLoadDelegate;

			private bool _isLoaded;
			private bool _isLoading;
			private Thread _thread;
			private int _priority;

			//=============
			// CONSTRUCTORS
			//=============

            public PageLoadInfo(PagingGridTag aGridTag, C1.Win.C1FlexGrid.C1FlexGrid aGrid, Point aPage, PageLoadDelegate aPageLoadDelegate)
			{
				_gridTag = aGridTag;
				_grid = aGrid;
				_page = aPage;
				_pageLoadDelegate = aPageLoadDelegate;
			}

			//===========
			// PROPERTIES
			//===========

			public bool isLoaded
			{
				get
				{
					if (!_isLoading)
					{
						return _isLoaded;
					}
					else
					{
						return false;
					}
				}
				set
				{
					_isLoaded = value;
				}
			}

			public bool isLoading
			{
				get
				{
					return _isLoading;
				}
			}

			//========
			// METHODS
			//========

			public void WaitForPageLoad()
			{
				try
				{
					if (_thread != null && _isLoading)
					{
						_thread.Join();
					}
				}
				catch (Exception)
				{
					throw;
				}
			}

			public void LoadPage()
			{
				try
				{
					if (_isLoading)
					{
						WaitForPageLoad();
					}
					else if (!_isLoaded)
					{
						_priority = 2;
						LoadMyPage();
					}
				}
				catch (Exception)
				{
					throw;
				}
			}

			public void LoadPageInBackground()
			{
				try
				{
					if (!_isLoading && !_isLoaded)
					{
						_isLoading = true;
						_priority = 3;
						_thread = new Thread(new ThreadStart(LoadMyPage));
						_thread.Name = "PageLoadThread - " + _page.X + "," + _page.Y;
						_thread.Start();
					}
				}
				catch (Exception)
				{
					throw;
				}
			}

			private void LoadMyPage()
			{
				Rectangle pageBound;
				try
				{
					_isLoading = true;
					pageBound = _gridTag.GetPageBoundary(_page);

					if (_grid.InvokeRequired)
					{
						_grid.Invoke(_pageLoadDelegate, _grid, pageBound.X, pageBound.Y, pageBound.Width, pageBound.Height, _priority);
					}
					else
					{
						_pageLoadDelegate(_grid, pageBound.X, pageBound.Y, pageBound.Width, pageBound.Height, _priority);
					}

					_isLoaded = true;
				}
				catch (EndPageLoadThreadException)
				{
					_isLoaded = false;
				}
				catch (Exception)
				{
					_isLoaded = false;
					throw;
				}
				finally
				{
					_isLoading = false;
				}
			}
		}

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PagingGridTag.
		/// </summary>
		/// <param name="aGridId">
		/// The numeric id that uniquely identifies the grid.
		/// </param>
		/// <param name="aGrid">
		/// The C1FlexGrid this instance will be attached to.
		/// </param>
		/// <param name="aRowHeaderGrid">
		/// The C1FlexGrid that contains the Row Headers.
		/// </param>
		/// <param name="aColHeaderGrid">
		/// The C1FlexGrid that contains the Column Headers.
		/// </param>
		/// <param name="aPageLoadDelegate">
		/// A delegate that points to a page load routine.
		/// </param>

		public PagingGridTag(
			int aGridId,
			C1FlexGrid aGrid,
			C1FlexGrid aRowHeaderGrid,
			C1FlexGrid aColHeaderGrid,
			PageLoadDelegate aPageLoadDelegate,
			int aRowPageSize,
			int aColPageSize)
			: base(aGridId, aRowHeaderGrid, aColHeaderGrid)
		{
			Initialize(aGridId, aGrid, aRowHeaderGrid, aColHeaderGrid, aPageLoadDelegate, aRowPageSize, aColPageSize, null, null, null, null, false, false);
		}

		/// <summary>
		/// Creates a new instance of PagingGridTag.
		/// </summary>
		/// <param name="aGridId">
		/// The numeric id that uniquely identifies the grid.
		/// </param>
		/// <param name="aGrid">
		/// The C1FlexGrid this instance will be attached to.
		/// </param>
		/// <param name="aRowHeaderGrid">
		/// The C1FlexGrid that contains the Row Headers.
		/// </param>
		/// <param name="aColHeaderGrid">
		/// The C1FlexGrid that contains the Column Headers.
		/// </param>
		/// <param name="aPageLoadDelegate">
		/// A delegate that points to a page load routine.
		/// </param>

        public PagingGridTag(
			int aGridId,
			C1FlexGrid aGrid,
			C1FlexGrid aRowHeaderGrid,
			C1FlexGrid aColHeaderGrid,
			PageLoadDelegate aPageLoadDelegate,
			int aRowPageSize,
			int aColPageSize,
			ArrayList aSelectableColHeaders,
			ArrayList aSelectableRowHeaders,
			SortedList aSortedColHeaders,
			SortedList aSortedRowHeaders)
			: base(aGridId, aRowHeaderGrid, aColHeaderGrid)
		{
			Initialize(aGridId, aGrid, aRowHeaderGrid, aColHeaderGrid, aPageLoadDelegate, aRowPageSize, aColPageSize, aSelectableColHeaders, aSelectableRowHeaders, aSortedColHeaders, aSortedRowHeaders, true, true);
		}

		/// <summary>
		/// Creates a new instance of PagingGridTag.
		/// </summary>
		/// <param name="aGridId">
		/// The numeric id that uniquely identifies the grid.
		/// </param>
		/// <param name="aGrid">
		/// The C1FlexGrid this instance will be attached to.
		/// </param>
		/// <param name="aRowHeaderGrid">
		/// The C1FlexGrid that contains the Row Headers.
		/// </param>
		/// <param name="aColHeaderGrid">
		/// The C1FlexGrid that contains the Column Headers.
		/// </param>
		/// <param name="aPageLoadDelegate">
		/// A delegate that points to a page load routine.
		/// </param>

		public PagingGridTag(
			int aGridId,
			C1FlexGrid aGrid,
			C1FlexGrid aRowHeaderGrid,
			C1FlexGrid aColHeaderGrid,
			PageLoadDelegate aPageLoadDelegate,
			int aRowPageSize,
			int aColPageSize,
			ArrayList aSelectableColHeaders,
			ArrayList aSelectableRowHeaders,
			SortedList aSortedColHeaders,
			SortedList aSortedRowHeaders,
			bool aShowRowBorders,
			bool aShowColBorders)
			: base(aGridId, aRowHeaderGrid, aColHeaderGrid)
		{
			Initialize(aGridId, aGrid, aRowHeaderGrid, aColHeaderGrid, aPageLoadDelegate, aRowPageSize, aColPageSize, aSelectableColHeaders, aSelectableRowHeaders, aSortedColHeaders, aSortedRowHeaders, aShowRowBorders, aShowColBorders);
		}

		private void Initialize(
			int aGridId,
			C1FlexGrid aGrid,
			C1FlexGrid aRowHeaderGrid,
			C1FlexGrid aColHeaderGrid,
			PageLoadDelegate aPageLoadDelegate,
			int aRowPageSize,
			int aColPageSize,
			ArrayList aSelectableColHeaders,
			ArrayList aSelectableRowHeaders,
			SortedList aSortedColHeaders,
			SortedList aSortedRowHeaders,
			bool aShowRowBorders,
			bool aShowColBorders)
		{
			_grid = aGrid;
			_pageLoadDelegate = aPageLoadDelegate;
			_rowPageSize = aRowPageSize;
			_colPageSize = aColPageSize;
			_selectableColHeaders = aSelectableColHeaders;
			_selectableRowHeaders = aSelectableRowHeaders;
			_sortedColHeaders = aSortedColHeaders;
			_sortedRowHeaders = aSortedRowHeaders;
			_showRowBorders = aShowRowBorders;
			_showColBorders = aShowColBorders;
			//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
			_sortImageRow = -1;
			//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw

			_visibleRowCount = 0;
			//Begin Track #5659 - JScott - All numbers disappeared
			//_scrollType = eScrollType.None;
			//End Track #5659 - JScott - All numbers disappeared
			_initialVisibleRows = -1;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the ArrayList that contains the SelectableColHeaders.
		/// </summary>

		public ArrayList SelectableColHeaders
		{
			get
			{
				return _selectableColHeaders;
			}
		}

		/// <summary>
		/// Returns the ArrayList that contains the SelectableRowHeaders.
		/// </summary>

		public ArrayList SelectableRowHeaders
		{
			get
			{
				return _selectableRowHeaders;
			}
		}

		/// <summary>
		/// Returns the SortedList that contains the SortedColHeaders.
		/// </summary>

		public SortedList SortedColHeaders
		{
			get
			{
				return _sortedColHeaders;
			}
		}

		/// <summary>
		/// Returns the SortedList that contains the SortedRowHeaders.
		/// </summary>

		public SortedList SortedRowHeaders
		{
			get
			{
				return _sortedRowHeaders;
			}
		}

		/// <summary>
		/// Gets or sets the Visible value for the grid.
		/// </summary>

		public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				_visible = value;
			}
		}

		/// <summary>
		/// Gets or sets the visible row count.
		/// </summary>

		public int VisibleRowCount
		{
			get
			{
				return _visibleRowCount;
			}
			set
			{
				_visibleRowCount = value;
			}
		}
		
		//Begin Track #5659 - JScott - All numbers disappeared
		///// <summary>
		///// Gets or sets the scroll type.
		///// </summary>

		//public eScrollType ScrollType
		//{
		//    get
		//    {
		//        return _scrollType;
		//    }
		//    set
		//    {
		//        _scrollType = value;
		//    }
		//}

		//End Track #5659 - JScott - All numbers disappeared
		/// <summary>
		/// Gets or sets the initial visible row count.
		/// </summary>

		public int InitialVisibleRows
		{
			get
			{
				return _initialVisibleRows;
			}
			set
			{
				_initialVisibleRows = value;
			}
		}

		/// <summary>
		/// Gets or sets the current row scroll position.
		/// </summary>

		public int CurrentRowScrollPosition
		{
			get
			{
				return _currentRowScrollPosition;
			}
			set
			{
				_currentRowScrollPosition = value;
				//Begin Track #5659 - JScott - All numbers disappeared

				//if (_scrollType == eScrollType.Line)
				//{
				//    if (_rowsPerScroll != 0)
				//    {
				//        _currentScrollGroup = _currentRowScrollPosition / _rowsPerScroll;
				//    }
				//    else
				//    {
				//        _currentScrollGroup = 0;
				//    }
				//}
				//else
				//{
				//    _currentScrollGroup = _currentRowScrollPosition;
				//}
				//End Track #5659 - JScott - All numbers disappeared
			}
		}

		/// <summary>
		/// Gets the PagesLoadInfo array.
		/// </summary>

		private PageLoadInfo[,] PagesLoadInfo
		{
			get
			{
				try
				{
					if (_pagesLoadInfo == null)
					{
						if (_rowHeaderGrid != null && _colHeaderGrid != null)
						{
							_pagesLoadInfo = new PageLoadInfo[(int)System.Math.Ceiling((double)_rowHeaderGrid.Rows.Count / _rowPageSize), (int)System.Math.Ceiling((double)_colHeaderGrid.Cols.Count / _colPageSize)];
						}
						else
						{
							_pagesLoadInfo = new PageLoadInfo[0, 0];
						}
					}

					return _pagesLoadInfo;
				}
				catch (Exception)
				{
					throw;
				}
			}
		}

		public bool ShowRowBorders
		{
			get
			{
				return _showRowBorders;
			}
		}

		public bool ShowColBorders
		{
			get
			{
				return _showColBorders;
			}
		}

		//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
		public int SortImageRow
		{
			get
			{
				return _sortImageRow;
			}
			set
			{
				_sortImageRow = value;
			}
		}

		//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
		//========
		// METHODS
		//========

		public void AllocatePageArray()
		{
			_pagesLoadInfo = null;
		}

		public void LoadPage(Point aPage)
		{
			try
			{
				GetPageLoadInfo(aPage).LoadPage();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void LoadPageInBackground(Point aPage)
		{
			try
			{
				GetPageLoadInfo(aPage).LoadPageInBackground();
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void WaitForPageLoads()
		{
			int x;
			int y;

			try
			{
				for (x = 0; x < PagesLoadInfo.GetUpperBound(0); x++)
				{
					for (y = 0; y < PagesLoadInfo.GetUpperBound(1); y++)
					{
						GetPageLoadInfo(new Point(x, y)).WaitForPageLoad();
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public ArrayList GetPagesToLoad(int aStartRow, int aStartCol, int aEndRow, int aEndCol)
		{
			Point topLeftPage;
			Point bottomRightPage;
			ArrayList outPoints;
			Point newPage;
			int x;
			int y;

			try
			{
				topLeftPage = CalculatePage(aStartRow, aStartCol);
				bottomRightPage = CalculatePage(aEndRow, aEndCol);
				outPoints = new ArrayList(); 

				for (x = topLeftPage.X; x <= bottomRightPage.X; x++)
				{
					for (y = topLeftPage.Y; y <= bottomRightPage.Y; y++)
					{
						newPage = new Point(x, y);

						if (!GetPageLoadInfo(newPage).isLoaded)
						{
							outPoints.Add(newPage);
						}
					}
				}

				return outPoints;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public ArrayList GetSurroundingPagesToLoad(int aStartRow, int aStartCol, int aEndRow, int aEndCol)
		{
			Point topLeftPage;
			Point bottomRightPage;
			ArrayList outPoints;
			Point newPage;
			int x;
			int y;

			try
			{
				topLeftPage = CalculatePage(aStartRow, aStartCol);
				bottomRightPage = CalculatePage(aEndRow, aEndCol);
				outPoints = new ArrayList(); 

				for (x = topLeftPage.X; x <= bottomRightPage.X; x++)
				{
					if (topLeftPage.Y > 0)
					{
						newPage = new Point(x, topLeftPage.Y - 1);

						if (!GetPageLoadInfo(newPage).isLoaded)
						{
							outPoints.Add(newPage);
						}
					}

					if (bottomRightPage.Y < PagesLoadInfo.GetUpperBound(1))
					{
						newPage = new Point(x, bottomRightPage.Y + 1);

						if (!GetPageLoadInfo(newPage).isLoaded)
						{
							outPoints.Add(newPage);
						}
					}
				}

				for (y = topLeftPage.Y; y <= bottomRightPage.Y; y++)
				{
					if (topLeftPage.X > 0)
					{
						newPage = new Point(topLeftPage.X - 1, y);

						if (!GetPageLoadInfo(newPage).isLoaded)
						{
							outPoints.Add(newPage);
						}
					}

					if (bottomRightPage.X < PagesLoadInfo.GetUpperBound(0))
					{
						newPage = new Point(bottomRightPage.X + 1, y);

						if (!GetPageLoadInfo(newPage).isLoaded)
						{
							outPoints.Add(newPage);
						}
					}
				}

				return outPoints;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Rectangle GetPageBoundary(Point aPoint)
		{
			try
			{
				return new Rectangle(
					aPoint.X * _rowPageSize,
					aPoint.Y * _colPageSize,
					Math.Min(((aPoint.X + 1) * _rowPageSize) - 1, _rowHeaderGrid.Rows.Count - 1),
					Math.Min(((aPoint.Y + 1) * _colPageSize - 1), _colHeaderGrid.Cols.Count - 1));
			}
			catch (Exception)
			{
				throw;
			}
		}

		private PageLoadInfo GetPageLoadInfo(Point aPage)
		{
			try
			{
				if (PagesLoadInfo[aPage.X, aPage.Y] == null)
				{
					PagesLoadInfo[aPage.X, aPage.Y] = new PageLoadInfo(this, _grid, aPage, _pageLoadDelegate);
				}

				return PagesLoadInfo[aPage.X, aPage.Y];
			}
			catch (Exception)
			{
				throw;
			}
		}

		private Point CalculatePage(int aRow, int aCol)
		{
			try
			{
				aRow = Math.Max(aRow, 0);
				aRow = Math.Min(aRow, _rowHeaderGrid.Rows.Count - 1);
				aCol = Math.Max(aCol, 0);
				aCol = Math.Min(aCol, _colHeaderGrid.Cols.Count - 1);

				return new Point(aRow / _rowPageSize, aCol / _colPageSize);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	/// <summary>
	/// The CellTag structure describes the information that is stored for each cell on the grid.  The CellTag values are stored in an array the same
	/// size as the grid.  CellTag is constructed as a structure to reduce memory usage.
	/// </summary>

	public struct CellTag
	{
		//=======
		// FIELDS
		//=======

		const int CellFlagsInitedFlag = 0x01;
		const int isCellNegativeFlag = 0x02;

		private byte _viewFlags;
		private ComputationCellFlags _cellFlags;

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========

		public bool CellFlagsInited
		{
			get
			{
				try
				{
					return ((_viewFlags & CellFlagsInitedFlag) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_viewFlags = (byte)(_viewFlags | CellFlagsInitedFlag);
					}
					else
					{
						_viewFlags = (byte)(_viewFlags & ~CellFlagsInitedFlag);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		public bool isCellNegative
		{
			get
			{
				try
				{
					return ((_viewFlags & isCellNegativeFlag) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_viewFlags = (byte)(_viewFlags | isCellNegativeFlag);
					}
					else
					{
						_viewFlags = (byte)(_viewFlags & ~isCellNegativeFlag);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		public ComputationCellFlags ComputationCellFlags
		{
			get
			{
				return _cellFlags;
			}
			set
			{
				_cellFlags = value;
				CellFlagsInited = true;
			}
		}

		//========
		// METHODS
		//========
	}
}
