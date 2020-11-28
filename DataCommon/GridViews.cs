using System;
using System.Drawing;

namespace MIDRetail.DataCommon
{
	public class RowColHeader
	{
		//=======
		// FIELDS
		//=======

		private string _name;			//the text value of the row header (what's displayed to the user)
//Begin Modification - JScott - Export Method - Part 9
		private bool _isSelectable;		//whether this header shows in the selectable list.
//End Modification - JScott - Export Method - Part 9
		private bool _isDisplayed;		//whether this header is shown or hidden.
		private bool _defaultDisplay;	//used to store the original display value.
// Begin Track #4868 - JSmith - Variable Groupings
        private string _grouping;       //used to build hierarchy of nodes
// End Track #4868
        private Image _image;           // Begin TT#358/#334/#363 - RMatelic - Velocity View column display issues
        private int _rowColWidth = 0;
        private int _rowColIndex = 0;   // End TT#358/#334/#363 
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of RowColHeader.
		/// </summary>

		public RowColHeader()
		{
			_name = "";
//Begin Modification - JScott - Export Method - Part 9
			_isSelectable = true;
//End Modification - JScott - Export Method - Part 9
			_isDisplayed = false;
			_defaultDisplay = _isDisplayed;
// Begin Track #4868 - JSmith - Variable Groupings
            _grouping = null;
// End Track #4868
		}

		/// <summary>
		/// Creates a new instance of RowColHeader using the given Name and boolean.
		/// </summary>
		/// <param name="aName">
		/// A string that contains the name of the Row/Col header.
		/// </param>
		/// <param name="aIsDisplayed">
		/// A boolean indicating if the Row/Col header is currently displayed.
		/// </param>
		
		public RowColHeader(string aName, bool aIsDisplayed)
		{
			_name = aName;
//Begin Modification - JScott - Export Method - Part 9
			_isSelectable = true;
//End Modification - JScott - Export Method - Part 9
			_isDisplayed = aIsDisplayed;
			_defaultDisplay = _isDisplayed;
// Begin Track #4868 - JSmith - Variable Groupings
            _grouping = null;
// End Track #4868
		}

// Begin Track #4868 - JSmith - Variable Groupings

        /// <summary>
        /// Creates a new instance of RowColHeader using the given Name and boolean.
        /// </summary>
        /// <param name="aName">
        /// A string that contains the name of the Row/Col header.
        /// </param>
        /// <param name="aIsDisplayed">
        /// A boolean indicating if the Row/Col header is currently displayed.
        /// </param>
        /// <param name="aGrouping">
        /// The grouping where the current Row/Col header is to be displayed.
        /// </param>

        public RowColHeader(string aName, bool aIsDisplayed, string aGrouping)
        {
            _name = aName;
            _isSelectable = true;
            _isDisplayed = aIsDisplayed;
            _defaultDisplay = _isDisplayed;
            _grouping = aGrouping;
        }
// End Track #4868

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets or sets the Name of the Row/Col header.
		/// </summary>

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

//Begin Modification - JScott - Export Method - Part 9
		/// <summary>
		/// Gets or sets the boolean indicating if the Row/Col header is selectable.
		/// </summary>

		public bool IsSelectable
		{
			get
			{
				return _isSelectable;
			}
			set
			{
				_isSelectable = value;
			}
		}

//End Modification - JScott - Export Method - Part 9
		/// <summary>
		/// Gets or sets the boolean indicating if the Row/Col header is displayed.
		/// </summary>

		public bool IsDisplayed
		{
			get
			{
//Begin Modification - JScott - Export Method - Part 9
//				return _isDisplayed;
				return _isSelectable && _isDisplayed;
//End Modification - JScott - Export Method - Part 9
			}
			set
			{
				_isDisplayed = value;
			}
		}

		/// <summary>
		/// Gets the boolean indicating the default display value.
		/// </summary>

		public bool DefaultDisplay
		{
			get
			{
				return _defaultDisplay;
			}
			// BEGIN TT#490-MD - stodd -  post-receipts should not show placeholders
			set
			{
				_defaultDisplay = value;
			}
			// END TT#490-MD - stodd -  post-receipts should not show placeholders
		}

// Begin Track #4868 - JSmith - Variable Groupings
        /// <summary>
        /// Gets or sets the Grouping of the Row/Col header.
        /// </summary>

        public string Grouping
        {
            get
            {
                return _grouping;
            }
            set
            {
                _grouping = value;
            }
        }
// End Track #4868

        // Begin TT#358/#334/#363 - RMatelic - Velocity View column display issues
        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
            }
        }

        public int RowColumnIndex
        {
            get
            {
                return _rowColIndex;
            }
            set
            {
                _rowColIndex = value;
            }
        }

        public int RowColumnWidth
        {
            get
            {
                return _rowColWidth;
            }
            set
            {
                _rowColWidth = value;
            }
        }
        // End TT#358/#334/#363 
		//========
		// METHODS
		//========
//Begin Modification - JScott - Export Method - Part 9

		virtual public void CopyFrom(RowColHeader aRowColHdr)
		{
			try
			{
				_name = aRowColHdr._name;
				_isSelectable = aRowColHdr._isSelectable;
				_isDisplayed = aRowColHdr._isDisplayed;
				_defaultDisplay = aRowColHdr._defaultDisplay;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
//End Modification - JScott - Export Method - Part 9
	}

	/// <summary>
	/// The RowColHeader class defines a RowColHeader that contains a Profile that describes the data.
	/// </summary>

	public class RowColProfileHeader : RowColHeader
	{
		//=======
		// FIELDS
		//=======

		private Profile _profile;
		private int _sequence;
		private bool _isSummarized;

		//=============
		// CONSTRUCTORS
		//=============

//Begin Modification - JScott - Export Method - Part 9
		/// <summary>
		/// Creates a new instance of RowColProfileHeader.
		/// </summary>
		
		public RowColProfileHeader()
			: base()
		{
		}

//End Modification - JScott - Export Method - Part 9
		/// <summary>
		/// Creates a new instance of RowColProfileHeader using the given Name, display value, and Profile.
		/// </summary>
		/// <param name="aName">
		/// The displayable name associated with the row/col.
		/// </param>
		/// <param name="aIsDisplayed">
		/// A boolean indicating if the row/col is displayed.
		/// </param>
		/// <param name="aProfile">
		/// The Profile that is associated with this row/col.
		/// </param>

        public RowColProfileHeader(string aName, bool aIsDisplayed, int aSequence, Profile aProfile)
            : base(aName, aIsDisplayed)
        {
            _profile = aProfile;
            _sequence = aSequence;
            _isSummarized = false;
        }

// Begin Track #4868 - JSmith - Variable Groupings
        /// <summary>
        /// Creates a new instance of RowColProfileHeader using the given Name, display value, and Profile.
        /// </summary>
        /// <param name="aName">
        /// The displayable name associated with the row/col.
        /// </param>
        /// <param name="aIsDisplayed">
        /// A boolean indicating if the row/col is displayed.
        /// </param>
        /// <param name="aProfile">
        /// The Profile that is associated with this row/col.
        /// </param>
        /// <param name="aGrouping">
        /// The grouping where the current Row/Col header is to be displayed.
        /// </param>

        public RowColProfileHeader(string aName, bool aIsDisplayed, int aSequence, Profile aProfile, string aGrouping)
            : base(aName, aIsDisplayed, aGrouping)
        {
            _profile = aProfile;
            _sequence = aSequence;
            _isSummarized = false;
        }
// End Track #4868

		/// <summary>
		/// Creates a new instance of RowColProfileHeader using the given Name, display value, and Profile.
		/// </summary>
		/// <param name="aName">
		/// The displayable name associated with the row/col.
		/// </param>
		/// <param name="aIsDisplayed">
		/// A boolean indicating if the row/col is displayed.
		/// </param>
		/// <param name="aIsSummarized">
		/// A boolean indicating if the row/col is summarized.
		/// </param>
		/// <param name="aProfile">
		/// The Profile that is associated with this row/col.
		/// </param>

		public RowColProfileHeader(string aName, bool aIsDisplayed, bool aIsSummarized, int aSequence, Profile aProfile)
			: base(aName, aIsDisplayed)
		{
			_profile = aProfile;
			_sequence = aSequence;
			_isSummarized = aIsSummarized;
		}

// Begin Track #4868 - JSmith - Variable Groupings
        /// <summary>
        /// Creates a new instance of RowColProfileHeader using the given Name, display value, and Profile.
        /// </summary>
        /// <param name="aName">
        /// The displayable name associated with the row/col.
        /// </param>
        /// <param name="aIsDisplayed">
        /// A boolean indicating if the row/col is displayed.
        /// </param>
        /// <param name="aIsSummarized">
        /// A boolean indicating if the row/col is summarized.
        /// </param>
        /// <param name="aProfile">
        /// The Profile that is associated with this row/col.
        /// </param>
        /// <param name="aGrouping">
        /// The grouping where the current Row/Col header is to be displayed.
        /// </param>

        public RowColProfileHeader(string aName, bool aIsDisplayed, bool aIsSummarized, int aSequence, Profile aProfile, string aGrouping)
            : base(aName, aIsDisplayed, aGrouping)
        {
            _profile = aProfile;
            _sequence = aSequence;
            _isSummarized = aIsSummarized;
        }
// End Track #4868

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the Profile associated with this row/col.
		/// </summary>

		public Profile Profile
		{
			get
			{
				return _profile;
			}
		}

		/// <summary>
		/// Gets the integer indicating the column sequence.
		/// </summary>

		public int Sequence
		{
			get
			{
				return _sequence;
			}
			set
			{
				_sequence = value;
			}
		}

		/// <summary>
		/// Gets or sets the boolean indicating if the Row/Col header is summarized.
		/// </summary>

		public bool IsSummarized
		{
			get
			{
				return _isSummarized;
			}
			set
			{
				_isSummarized = value;
			}
		}

		//========
		// METHODS
		//========

		override public string ToString()
		{
			return _profile.ToString();
		}
//Begin Modification - JScott - Export Method - Part 9

		override public void CopyFrom(RowColHeader aRowColHdr)
		{
			try
			{
				base.CopyFrom(aRowColHdr);

				_profile = ((RowColProfileHeader)aRowColHdr)._profile;
				_sequence = ((RowColProfileHeader)aRowColHdr)._sequence;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public RowColProfileHeader Copy()
		{
			RowColProfileHeader rowColHdr;

			try
			{
				rowColHdr = new RowColProfileHeader();
				rowColHdr.CopyFrom(this);
				return rowColHdr;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
//End Modification - JScott - Export Method - Part 9
	}
}
