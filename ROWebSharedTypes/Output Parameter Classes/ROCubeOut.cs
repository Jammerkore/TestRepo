using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MIDRetail.DataCommon;

namespace Logility.ROWebSharedTypes
{
    [DataContract(Name = "ROData", Namespace = "http://Logility.ROWeb/")]
    public class ROData
    {
        [DataMember(IsRequired = true)]
        private Dictionary<eDataType, ROCells> _cells;

        public ROData()
        {
            _cells = new Dictionary<eDataType, ROCells>();
        }

        public Dictionary<eDataType, ROCells> Cells { get { return _cells; } }

        /// <summary>
        /// Adds and instance of the ROCells class to the collection
        /// </summary>
        /// <param name="dataType">The eDataType of the cells</param>
        /// <param name="ROCells">The two dimensional collection of ROCell instances</param>
        public void AddCells(eDataType dataType, ROCells ROCells)
        {
            _cells.Add(dataType, ROCells);
        }

        /// <summary>
        /// Creates a new instance of the ROCells class to add with the data type
        /// </summary>
        /// <param name="dataType">The eDataType of the cells</param>
        /// <param name="iNumberOfRows">The number of cell rows</param>
        /// <param name="iNumberOfColumns">The number of cell columns</param>
        public void AddCells(eDataType dataType, int iNumberOfRows, int iNumberOfColumns)
        {
            ROCells ROCells = new ROCells(iNumberOfRows, iNumberOfColumns);
            _cells.Add(dataType, ROCells);
        }

        /// <summary>
        /// Retrieves the ROCells object associated with the data type.
        /// </summary>
        /// <param name="dataType">The eDataType of the cells</param>
        /// <remarks>Return null if data type is not found.</remarks>
        /// <returns>An instance of the ROCells class for the data type</returns>
        public ROCells GetCells(eDataType dataType)
        {
            ROCells cells;
            if (!_cells.TryGetValue(dataType, out cells))
            {
                return null;
            }
            return cells;
        }
    }

    [DataContract(Name = "ROCells", Namespace = "http://Logility.ROWeb/")]
    public class ROCells
    {
        [DataMember(IsRequired = true)]
        private List<ROColumnAttributes> _columns;
        //[DataMember(IsRequired = true)]
        //private List<string> _rowLabels;
        [DataMember(IsRequired = true)]
        private List<RORowAttributes> _rows;
        [DataMember(IsRequired = true)]
        private ROCell[][] _ROCell;

        /// <summary>
        /// Creates and instance of the ROCells class
        /// </summary>
        public ROCells()
        {
            _columns = new List<ROColumnAttributes>();
            //_rowLabels = new List<string>();
            _rows = new List<RORowAttributes>();
        }

        /// <summary>
        /// Creates and instance of the ROCells class with the specified rows and columns
        /// </summary>
        /// <param name="iNumberOfRows">The number of cell rows</param>
        /// <param name="iNumberOfColumns">The number of cell columns</param>
        public ROCells(int iNumberOfRows, int iNumberOfColumns)
        {
            _ROCell = new ROCell[iNumberOfRows][];
            _columns = new List<ROColumnAttributes>();
            //_rowLabels = new List<string>();
            _rows = new List<RORowAttributes>();
        }

        /// <summary>
        /// The list of column names associated with the cells
        /// </summary>
        public List<ROColumnAttributes> Columns { get { return _columns; } set { _columns = value; } }

        /// <summary>
        /// The list of columns which are selected for display.
        /// </summary>
        public List<ROColumnAttributes> ColumnsByPosition
        {
            get
            {
                return _columns.Where(windowCol => windowCol.DisplayedInWindows).OrderBy(hdr => hdr.ColumnHeader).ThenBy(colPos => colPos.ColumnPosition).ToList();
            }
        }

        ///// <summary>
        ///// The list of row names associated with the cells
        ///// </summary>
        //[ObsoleteAttribute("This property is obsolete. Use Rows instead.", false)] 
        //public List<string> RowLabels { get { return _rowLabels; } set { _rowLabels = value; } }

        /// <summary>
        /// The list of row names associated with the cells
        /// </summary>
        public List<RORowAttributes> Rows { get { return _rows; } set { _rows = value; } }

        /// <summary>
        /// The two dimensional array of ROCell instances
        /// </summary>
        public ROCell[][] ROCell { get { return _ROCell; } }

        public ROCell[][] ROCellByPosition
        {
            get
            {
                ROCell[][] _ROCell2 = new ROCell[_ROCell.Length][];
                //Copying the data to another array to keep original data unchanged.
                _ROCell.CopyTo(_ROCell2, 0);

                for (int index = 0; index < _ROCell2.Length; index++)
                {
                    if (_ROCell2[index].Length > 0)
                    { if (_ROCell2[index][0] == null) continue; }

                    var data = _ROCell2[index].Where(inner => inner.DisplayedInWindows);
                    if (data.Count() > 0) { _ROCell2[index] = data.ToArray(); }
                    _ROCell2[index] = _ROCell2[index].OrderBy(hdr => hdr.ColumnHeader).ThenBy(inner => inner.ColumnPosition).ToArray();
                }
                return _ROCell2;
            }
        }


        public void AddCells(int iNumberOfRows, int iNumberOfColumns)
        {
            _ROCell = new ROCell[iNumberOfRows][];
            for (int i = 0; i < iNumberOfRows; i++)
            {
                _ROCell[i] = new ROCell[iNumberOfColumns];

            }
        }

        public int GetIndexOfColumn(string columnName)
        {
            foreach (ROColumnAttributes ca in Columns)
            {
                if (ca.Name == columnName)
                {
                    return ca.ColumnIndex;
                }
            }

            return -1;
        }
    }

    [DataContract(Name = "ROCell", Namespace = "http://Logility.ROWeb/")]
    public class ROCell : IComparable
    {
        [DataMember(IsRequired = true)]
        private eCellDataType _cellDataType;
        [DataMember(IsRequired = true)]
        private eCellValueType _cellValueType;
        [DataMember(IsRequired = true)]
        private object _value;
        [DataMember(IsRequired = true)]
        private ROCellAttributes _cellAttributes;

        [DataMember(IsRequired = true)]
        private int _columnPosition;

        [DataMember(IsRequired = true)]
        private bool _displayedInWindows;

        [DataMember(IsRequired = true)]
        private int _columnHeader;

        /// <summary>
        /// Creates a cell with no value
        /// </summary>
        public ROCell()
        {
            _cellDataType = eCellDataType.None;
            _cellValueType = eCellValueType.Number;
            _value = null;
            _cellAttributes = new ROCellAttributes();
            _columnPosition = 0;
            _displayedInWindows = true;
        }

        /// <summary>
        /// Creates an instance of the class for a numeric value
        /// </summary>
        /// <param name="value">The value for the cell</param>
        public ROCell(eCellDataType cellDataType, object value)
        {
            _cellDataType = cellDataType;
            _value = value;
            _cellAttributes = new ROCellAttributes();
        }

        /// <summary>
        /// Creates an instance of the class for a numeric value
        /// </summary>
        /// <param name="value">The value for the cell</param>
        public ROCell(eCellDataType cellDataType, eCellValueType cellValueType, object value, int columnPosition, bool displayedInWindows, int colHeader)
        {
            _cellDataType = cellDataType;
            _cellValueType = cellValueType;
            _value = value;
            _cellAttributes = new ROCellAttributes();
            _columnPosition = columnPosition;
            _displayedInWindows = displayedInWindows;
            _columnHeader = colHeader;

        }

        public eCellDataType CellDataType { get { return _cellDataType; } set { _cellDataType = value; } }
        public eCellValueType CellValueType { get { return _cellValueType; } set { _cellValueType = value; } }
        public object Value { get { return _value; } set { _value = value; } }
        public string ValueAsString { get { return Convert.ToString(_value); } }

        /// <summary>
        ///  stores the column position which helps the user to order the cell data accordingly.
        /// </summary>
        public int ColumnPosition { get { return _columnPosition; } set { _columnPosition = value; } }

        public bool DisplayedInWindows { get { return _displayedInWindows; } set { _displayedInWindows = value; } }

        public int ColumnHeader { get { return _columnHeader; } set { _columnHeader = value; } }

        public ROCellAttributes ROCellAttributes { get { return _cellAttributes; } }

        /// <summary>
        /// Identifies if the cell is valid
        /// </summary>
        public bool IsValid { get { return _cellAttributes.IsValid; } set { _cellAttributes.IsValid = value; } }
        /// <summary>
        /// Identifies if the value of the cell can be modified by the user
        /// </summary>
        public bool IsDisplayOnly { get { return _cellAttributes.IsDisplayOnly; } }
        /// <summary>
        /// Identifies if the value of the cell can be modified by the user
        /// </summary>
        public bool IsEditable { get { return _cellAttributes.IsEditable; } set { _cellAttributes.IsEditable = value; } }
        /// <summary>
        /// Identifies if the value of the cell is numeric
        /// </summary>
        public bool IsNumeric { get { return _cellAttributes.IsNumeric; } set { _cellAttributes.IsNumeric = value; } }
        /// <summary>
        /// Identifies if the value of the cell is numeric
        /// </summary>
        public bool IsNegative { get { return _cellAttributes.IsNegative; } set { _cellAttributes.IsNegative = value; } }
        /// <summary>
        /// Identifies if the cell is for an ineligible item
        /// </summary>
        public bool IsIneligible { get { return _cellAttributes.IsIneligible; } }
        /// <summary>
        /// Identifies if the cell is protected and cannot be editted by a user
        /// </summary>
        public bool IsProtected { get { return _cellAttributes.IsProtected; } }
        /// <summary>
        /// Identifies if the cell has been locked
        /// </summary>
        public bool IsLocked { get { return _cellAttributes.IsLocked; } }
        /// <summary>
        /// Identifies if the cell has been closed
        /// </summary>
        public bool IsClosed { get { return _cellAttributes.IsClosed; } }
        /// <summary>
        /// Identifies if the cell has been changed by a user
        /// </summary>
        public bool IsEdited { get { return _cellAttributes.IsEdited; } }
        /// <summary>
        /// Identifies if the cell has been changed by a user or by the system
        /// </summary>
        public bool IsModified { get { return _cellAttributes.IsModified; } }
        /// <summary>
        /// Identifies if the cell is for a basis
        /// </summary>
        public bool IsBasis { get { return _cellAttributes.IsBasis; } }
        /// <summary>
		/// Identifies if the value is allowed exceed capacity maximum.
		/// </summary>
        public bool MayExceedCapacityMaximum { get { return _cellAttributes.MayExceedCapacityMaximum; } set { _cellAttributes.MayExceedCapacityMaximum = value; } }
        /// <summary>
		/// Identifies if the value is to allowed exceed grade maximum.
		/// </summary>
        public bool MayExceedGradeMaximum { get { return _cellAttributes.MayExceedGradeMaximum; } set { _cellAttributes.MayExceedGradeMaximum = value; } }
        /// <summary>
		/// Identifies if the value is allowed exceed primary maximum.
		/// </summary>
        public bool MayExceedPrimaryMaximum { get { return _cellAttributes.MayExceedPrimaryMaximum; } set { _cellAttributes.MayExceedPrimaryMaximum = value; } }
        /// <summary>
		/// Identifies if a store's allocation is out of balance with its components
		/// </summary>
        public bool StoreAllocationOutOfBalance { get { return _cellAttributes.StoreAllocationOutOfBalance; } set { _cellAttributes.StoreAllocationOutOfBalance = value; } }
        /// <summary>
		/// Identifies when a store exceeds capacity.
		/// </summary>
        public bool StoreExceedsCapacity { get { return _cellAttributes.StoreExceedsCapacity; } set { _cellAttributes.StoreExceedsCapacity = value; } }
        /// <summary>
        /// Identifies how many decimal positions the value is to contain
        /// </summary>
        public int DecimalPositions { get { return _cellAttributes.DecimalPositions; } }

        public int CompareTo(object obj)
        {
            ROCell cell = (ROCell)obj;

            if (cell.ColumnPosition > this.ColumnPosition)
            {
                return cell.ColumnPosition;
            }
            else

            {
                return this.ColumnPosition;
            }
        }
    }

    [DataContract(Name = "ROCellAttributes", Namespace = "http://Logility.ROWeb/")]
    public class ROCellAttributes
    {
        [DataMember(IsRequired = true)]
        private bool _isValid;
        [DataMember(IsRequired = true)]
        private bool _isDisplayOnly;
        [DataMember(IsRequired = true)]
        private bool _isEditable;
        [DataMember(IsRequired = true)]
        private bool _isNumeric;
        [DataMember(IsRequired = true)]
        private bool _isNegative;
        [DataMember(IsRequired = true)]
        private bool _isIneligible;
        [DataMember(IsRequired = true)]
        private bool _isProtected;
        [DataMember(IsRequired = true)]
        private bool _isLocked;
        [DataMember(IsRequired = true)]
        private bool _isEdited;
        [DataMember(IsRequired = true)]
        private bool _isModified;
        [DataMember(IsRequired = true)]
        private bool _isClosed;
        [DataMember(IsRequired = true)]
        private bool _isBasis;
        [DataMember(IsRequired = true)]
        private int _decimalPositions;

        [DataMember(IsRequired = true)]
        private bool _mayExceedCapacityMaximum;
        [DataMember(IsRequired = true)]
        private bool _mayExceedGradeMaximum;
        [DataMember(IsRequired = true)]
        private bool _mayExceedPrimaryMaximum;
        [DataMember(IsRequired = true)]
        private bool _storeAllocationOutOfBalance;
        [DataMember(IsRequired = true)]
        private bool _storeExceedsCapacity;
        [DataMember(IsRequired = true)]
        private ROComputationCellFlags _rOCellFlags;
        [DataMember(IsRequired = true)]
        private int _rowOrder;
        [DataMember(IsRequired = true)]
        private int _colOrder;
        //[DataMember(IsRequired = true)]
        //private object _userData;
        [DataMember(IsRequired = true)]
        private eVariableStyle _variableStyle;
        [DataMember(IsRequired = true)]
        private double _gradeMax;
        [DataMember(IsRequired = true)]
        private double _primaryMax;
        [DataMember(IsRequired = true)]
        private double _min;

        public eVariableStyle VariableStyle { get { return _variableStyle; } set { _variableStyle = value; } }
        //public object UserData { get { return _userData; } set { _userData = value; } }
        public int ColOrder { get { return _colOrder; } set { _colOrder = value; } }
        public int RowOrder { get { return _rowOrder; } set { _rowOrder = value; } }
        public ROComputationCellFlags ROCellFlags { get { return _rOCellFlags; } set { _rOCellFlags = value; } }



        public ROCellAttributes()
        {
            _isValid = false;
            _isDisplayOnly = true;
            _isEditable = false;
            _isNumeric = false;
            _isNegative = false;
            _isIneligible = false;
            _isProtected = false;
            _isLocked = false;
            _isEdited = false;
            _isModified = false;
            _isBasis = false;
            _decimalPositions = 0;
            _colOrder = -1;
            _rowOrder = -1;
            _gradeMax = double.MaxValue;
            _primaryMax = double.MaxValue;
            _min = 0;
        }

        public bool MayExceedCapacityMaximum { get { return _mayExceedCapacityMaximum; } set { _mayExceedCapacityMaximum = value; } }

        public bool MayExceedGradeMaximum { get { return _mayExceedGradeMaximum; } set { _mayExceedGradeMaximum = value; } }

        public bool MayExceedPrimaryMaximum { get { return _mayExceedPrimaryMaximum; } set { _mayExceedPrimaryMaximum = value; } }

        public bool StoreAllocationOutOfBalance { get { return _storeAllocationOutOfBalance; } set { _storeAllocationOutOfBalance = value; } }

        public bool StoreExceedsCapacity { get { return _storeExceedsCapacity; } set { _storeExceedsCapacity = value; } }
        /// <summary>
        /// Identifies if the cell is valid
        /// </summary>
        public bool IsValid { get { return _isValid; } set { _isValid = value; } }
        /// <summary>
        /// Identifies if the value of the cell can be modified by the user
        /// </summary>
        public bool IsDisplayOnly { get { return _isDisplayOnly; } set { _isDisplayOnly = value; } }
        /// <summary>
        /// Identifies if the value of the cell can be modified by the user
        /// </summary>
        public bool IsEditable { get { return _isEditable; } set { _isEditable = value; } }
        /// <summary>
        /// Identifies if the value of the cell is numeric
        /// </summary>
        public bool IsNumeric { get { return _isNumeric; } set { _isNumeric = value; } }
        /// <summary>
        /// Identifies if the value of the cell is numeric
        /// </summary>
        public bool IsNegative { get { return _isNegative; } set { _isNegative = value; } }
        /// <summary>
        /// Identifies if the cell is for an ineligible item
        /// </summary>
        public bool IsIneligible { get { return _isIneligible; } set { _isIneligible = value; } }
        /// <summary>
        /// Identifies if the cell is protected and cannot be editted by a user
        /// </summary>
        public bool IsProtected { get { return _isProtected; } set { _isProtected = value; } }
        /// <summary>
        /// Identifies if the cell has been locked
        /// </summary>
        public bool IsLocked { get { return _isLocked; } set { _isLocked = value; } }
        /// <summary>
        /// Identifies if the cell can be changed by a user
        /// </summary>
        //public bool IsEditable { get { return _isEditable; } set { _isEditable = value; } }
        /// <summary>
        /// Identifies if the cell has been changed by a user
        /// </summary>
        public bool IsEdited { get { return _isEdited; } set { _isEdited = value; } }
        /// <summary>
        /// Identifies if the cell has been changed by a user or by the system
        /// </summary>
        public bool IsModified { get { return _isModified; } set { _isModified = value; } }
        /// <summary>
        /// Identifies if the cell is closed
        /// </summary>
        public bool IsClosed { get { return _isClosed; } set { _isClosed = value; } }
        /// <summary>
        /// Identifies if the cell if for a basis
        /// </summary>
        public bool IsBasis { get { return _isBasis; } set { _isBasis = value; } }
        /// <summary>
        /// Identifies how many decimal positions the value is to contain
        /// </summary>
        public int DecimalPositions { get { return _decimalPositions; } set { _decimalPositions = value; } }
        /// <summary>
        /// Identifies the grade maximum value for the cell
        /// </summary>
        public double GradeMaximumValue { get { return _gradeMax; } set { _gradeMax = value; } }
        /// <summary>
        /// Identifies the primary maximum value for the cell
        /// </summary>
        public double PrimaryMaximumValue { get { return _primaryMax; } set { _primaryMax = value; } }
        /// <summary>
        /// Identifies the minimum value for the cell
        /// </summary>
        public double MinimumValue { get { return _min; } set { _min = value; } }

        //[Serializable]
        public struct ROComputationCellFlags
        {
            //=======
            // FIELDS
            //=======

            private ushort _cellFlags;

            //===========
            // PROPERTIES
            //===========

            /// <summary>
            /// Gets the Flags
            /// </summary>

            public ushort Flags
            {
                get
                {
                    return _cellFlags;
                }
                set
                {
                    _cellFlags = value;
                }
            }

            //========
            // METHODS
            //========

            /// <summary>
            /// Clears all flags.
            /// </summary>

            public void Clear()
            {
                try
                {
                    _cellFlags = 0;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }
    }

    [DataContract(Name = "ROColumnAttributes", Namespace = "http://Logility.ROWeb/")]
    public class ROColumnAttributes
    {
        [DataMember(IsRequired = true)]
        private string _columnName;
        [DataMember(IsRequired = true)]
        private string _columnLabel;
        [DataMember(IsRequired = true)]
        private string _groupName;
        [DataMember(IsRequired = true)]
        private int _columnIndex;
        [DataMember(IsRequired = true)]
        private int _columnPosition;
        [DataMember(IsRequired = true)]
        private int _colHeader;
        [DataMember(IsRequired = true)]
        private bool _isColDisplayedInWindows;
        [DataMember(IsRequired = true)]
        private int _waferNo;

        public ROColumnAttributes(string columnName, int columnIndex)
        {
            _columnName = columnName;
            _columnLabel = columnName;
            _groupName = "";
            _columnIndex = columnIndex;

        }
        public ROColumnAttributes(string columnName, int columnIndex, int columnPosition, int colHeader, bool isColumnInWindows, int waferNo)
        {
            _columnName = columnName;
            _columnLabel = columnName;
            _groupName = "";
            _columnIndex = columnIndex;
            _columnPosition = columnPosition;
            _colHeader = colHeader;
            _isColDisplayedInWindows = isColumnInWindows;
            _waferNo = waferNo;
        }

        public ROColumnAttributes(string columnName, string label, string groupName, int columnIndex, int columnPosition)
        {
            _columnName = columnName;
            _columnLabel = label;
            _groupName = groupName;
            _columnIndex = columnIndex;
            _columnPosition = columnPosition;
        }

        public string Name { get { return _columnName; } }
        public string Label { get { return _columnLabel; } }
        public string GroupName { get { return _groupName; } }
        public int ColumnIndex { get { return _columnIndex; } }
        public int ColumnPosition { get { return _columnPosition; } }

        public int WaferNo { get { return _waferNo; } }
        public int ColumnHeader { get { return _colHeader; } }

        public bool DisplayedInWindows { get { return _isColDisplayedInWindows; } }
    }

    [DataContract(Name = "RORowAttributes", Namespace = "http://Logility.ROWeb/")]
    public class RORowAttributes
    {
        [DataMember(IsRequired = true)]
        private string _rowLabel;
        [DataMember(IsRequired = true)]
        private int _rowLevel;


        public RORowAttributes(string rowLabel, int rowLevel = 0)
        {
            _rowLabel = rowLabel;
            _rowLevel = rowLevel;
        }

        public string RowLabel { get { return _rowLabel; } }
        public int RowLevel { get { return _rowLevel; } }
    }

    [DataContract(Name = "ROGridData", Namespace = "http://Logility.ROWeb/")]
    public class ROGridData : RONoDataOut
    {
        [DataMember(IsRequired = true)]
        private ROData _gridData;

        [DataMember(IsRequired = true)]
        private int _iFirstRowItem;

        [DataMember(IsRequired = true)]
        private int _iLastRowItem;

        [DataMember(IsRequired = true)]
        private int _iTotalRowItems;

        [DataMember(IsRequired = true)]
        private int _iNumberOfRows;

        [DataMember(IsRequired = true)]
        private int _iFirstColItem;

        [DataMember(IsRequired = true)]
        private int _iLastColItem;

        [DataMember(IsRequired = true)]
        private int _iTotalColItems;

        [DataMember(IsRequired = true)]
        private int _iNumberOfColumns;

        public ROGridData(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, ROData dataOutput,
            int iFirstRowItem, int iLastRowItem, int iTotalRowItems, int iNumberOfRows,
            int iFirstColItem, int iLastColItem, int iTotalColItems, int iNumberOfColumns
            )
            : base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _gridData = dataOutput;
            _iFirstRowItem = iFirstRowItem;
            _iLastRowItem = iLastRowItem;
            _iTotalRowItems = iTotalRowItems;
            _iNumberOfRows = iNumberOfRows;
            _iFirstColItem = iFirstColItem;
            _iLastColItem = iLastColItem;
            _iTotalColItems = iTotalColItems;
            _iNumberOfColumns = iNumberOfColumns;
        }

        public ROData GridData { get { return _gridData; } }

        public int FirstRowItem { get { return _iFirstRowItem; } }

        public int LastRowItem { get { return _iLastRowItem; } }

        public int TotalRowItems { get { return _iTotalRowItems; } }

        public int NumberOfRows { get { return _iNumberOfRows; } }

        public int FirstColItem { get { return _iFirstColItem; } }

        public int LastColItem { get { return _iLastColItem; } }

        public int TotalColItems { get { return _iTotalColItems; } }

        public int NumberOfColumns { get { return _iNumberOfColumns; } }
    }

    
    [DataContract(Name = "ROCubeMetadata", Namespace = "http://Logility.ROWeb/")]
    public class ROCubeMetadata : RONoDataOut
    {
        [DataMember(IsRequired = true)]
        private eGridOrientation _gridOrientation;
        [DataMember(IsRequired = true)]
        private List<ROCubeMetadatAttribute> _cubeMetadataAttributes;

        public ROCubeMetadata(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID)
            : base(ROReturnCode, sROMessage, ROInstanceID)
        {
        }

        public ROCubeMetadata(eROReturnCode ROReturnCode, string sROMessage, long ROInstanceID, eGridOrientation gridOrientation, ROCubeMetadatAttribute cubeMetadataAttributes)
            : base(ROReturnCode, sROMessage, ROInstanceID)
        {
            _gridOrientation = gridOrientation;
            _cubeMetadataAttributes = CubeMetadataAttributes;
        }
        public eGridOrientation GridOrientation { get { return _gridOrientation; } set { _gridOrientation = value; } }

        /// <summary>
        /// The list of cube metadata attributes 
        /// </summary>
        public List<ROCubeMetadatAttribute> CubeMetadataAttributes { get { return _cubeMetadataAttributes; } set { _cubeMetadataAttributes = value; } }
    }

    [DataContract(Name = "ROCubeMetadatAttribute", Namespace = "http://Logility.ROWeb/")]
    public class ROCubeMetadatAttribute
    {

        [DataMember(IsRequired = true)]
        private eDataType _dataType;
        [DataMember(IsRequired = true)]
        private int _totalRowCount;
        [DataMember(IsRequired = true)]
        private int _rowCountPerGrouping;
        [DataMember(IsRequired = true)]
        private int _totalColCount;
        [DataMember(IsRequired = true)]
        private int _colCountPerGrouping;

        /// <summary>
        /// Creates a Cube Metadata Attribute row with no value
        /// </summary>
        public ROCubeMetadatAttribute()
        {
            _dataType = eDataType.None;
            _totalRowCount = 0;
            _rowCountPerGrouping = 0;
            _totalColCount = 0;
            _colCountPerGrouping = 0;
        }

        /// <summary>
        /// Creates an instance of the class with values
        /// </summary>
        /// <param name="value">The value for the Cube Metadata Attributes</param>
        public ROCubeMetadatAttribute(eDataType dataType, int totalRowCount, int rowCountPerGrouping, int totalColCount, int colCountPerGrouping)
        {
            _dataType = dataType;
            _totalRowCount = totalRowCount;
            _rowCountPerGrouping = rowCountPerGrouping;
            _totalColCount = totalColCount;
            _colCountPerGrouping = colCountPerGrouping;
        }

        public eDataType DataType { get { return _dataType; } set { _dataType = value; } }
        public int TotalRowCount { get { return _totalRowCount; } set { _totalRowCount = value; } }
        public int RowCountPerGrouping { get { return _rowCountPerGrouping; } set { _rowCountPerGrouping = value; } }
        public int TotalColCount { get { return _totalColCount; } set { _totalColCount = value; } }
        public int ColCountPerGrouping { get { return _colCountPerGrouping; } set { _colCountPerGrouping = value; } }

    }
}
